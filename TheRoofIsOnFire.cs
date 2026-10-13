/* Copyright (c) 2026 Rick (rick 'at' gibbed 'dot' us)
 *
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 *
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 *
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System.Diagnostics;
using System.Runtime.InteropServices;
using Reloaded.Hooks.Definitions;
using Reloaded.Memory;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Native.Windows;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;

namespace WhatSpriteLimit;

internal static unsafe class TheRoofIsOnFire
{
    //private const byte EntdLimit = 15;
    private const byte EntdLimit = 16;

    private static T Read<T>(nuint at) where T : unmanaged =>
        Memory.Instance.Read<T>(at);

    private static void Write<T>(nuint at, T value) where T : unmanaged =>
        Memory.Instance.Write(at, in value);

    private static void Copy(nuint from, nuint to, int length) =>
        Memory.Instance.WriteRaw(to, Memory.Instance.ReadRaw(from, length));

    private static nuint _Module;
    private static nuint[] _Addresses = Array.Empty<nuint>();

    private static nuint A(Signature s) => _Addresses[(int)s];

    private static nuint _BuildActorPrimitiveSheetTable; // 0x1407151F0: 0xAA entries
    private static nuint _ReleaseRenderResourcesPages; // 0x142EA63E0: 20 pages

    private static nuint _SlotTable; // Slots * SlotSize
    private static nuint _IdTable; // Slots uints
    private static nuint _Snapshot15; // slot 15 copy for the battle snapshot

    private static readonly nuint[] _ExtraPages = new nuint[Layout.ExtraSlots];
    private static readonly int[] _ExtraIndices = new int[Layout.ExtraSlots];
    private static readonly int[] _ExtraSheets = new int[Layout.ExtraSlots];

    private static bool IsExtraIndex(ulong index) =>
        index - Layout.ExtraIndexBase < Layout.ExtraIdCount;

    private static int ExtraSlotFromIndex(ulong index) =>
        (int)((index - Layout.ExtraIndexBase) >> 4);

    private static delegate* unmanaged<nuint, nuint> _PageCtor; // 0x1403D0D4C
    private static delegate* unmanaged<nuint, void> _PageReset; // 0x1403D0E50
    private static delegate* unmanaged<nuint, int, int, void> _CellFree; // 0x1403D11DC
    private static delegate* unmanaged<int, uint, int> _PaletteAlloc; // 0x14041E448

    private static readonly List<IAsmHook> _Hooks = [];

    private static nuint RelocationTarget(in Relocation r, nuint slotTable, int slots, nuint idTable) => r.Base switch
    {
        RelocationBase.SlotBase => slotTable + r.Offset,
        RelocationBase.SlotEnd => slotTable + (nuint)(slots * Layout.SlotSize) + r.Offset,
        _ => idTable + r.Offset,
    };

    public static void Start(IModLoader loader, IReloadedHooks hooks, IStartupScanner scanner, Action<string> log)
    {
        _Module = (nuint)Process.GetCurrentProcess().MainModule!.BaseAddress;
        _Addresses = new nuint[Enum.GetValues<Signature>().Length];

        int remaining = Signatures.Patterns.Length;
        foreach (var (signature, pattern, offset) in Signatures.Patterns)
        {
            scanner.AddMainModuleScan(pattern, result =>
            {
                if (result.Found == true)
                {
                    _Addresses[(int)signature] = (nuint)((nint)_Module + result.Offset + offset);
                }
            });
        }

        loader.OnModLoaderInitialized += () => OnAllModsLoaded(hooks, log);
    }

    private static void OnAllModsLoaded(IReloadedHooks hooks, Action<string> log)
    {
        foreach (var (derived, from, delta) in Signatures.Derived)
        {
            if (_Addresses[(int)from] != 0u)
            {
                _Addresses[(int)derived] = (nuint)((nint)_Addresses[(int)from] + delta);
            }
        }

        Install(hooks, log);
    }

    private static void Install(IReloadedHooks hooks, Action<string> log)
    {
        var missing = Enumerable.Range(0, _Addresses.Length)
            .Where(i => _Addresses[i] == 0u)
            .Select(i => ((Signature)i).ToString()).ToList();
        if (missing.Count > 0)
        {
            log($"signatures not found ({string.Join(", ", missing)}): wrong exe version?; not patching");
            return;
        }

        byte[] oldClear = { 0x8A, 0xFA, 0x02, 0x00 }; // 15 * 0x32D6
        if (Expect(A(Signature.RenderDrawListBound) + 3u, 0x7A, 0x01, 0x00, 0x00) == false || // cmp r8d, 0x17A
            Expect(A(Signature.LoadBattleUnitsLimitMov) + 1u, 0x09) == false || // mov ecx, 9
            Expect(A(Signature.LoadBattleUnitsLimitCmp) + 2u, 0x09) == false || // cmp eax, 9
            Expect(A(Signature.InitBattleIdLoop) + 2u, 0x0A) == false || // cmp eax, 10
            Expect(A(Signature.AllocateSpriteSlotBound1) + 2u, 0x0F) == false || // cmp edx, 15
            Expect(A(Signature.AllocateSpriteSlotBound2) + 2u, 0x0F) == false || // ''
            Expect(A(Signature.AllocateSpriteSlotBound3) + 2u, 0x0F) == false || // ''
            Expect(A(Signature.ClearBattleStateSlots) + 2u, oldClear) == false || // mov r8d, 0x2FA8A
            Expect(A(Signature.SaveBattleSnapshotSize) + 2u, oldClear) == false ||
            Expect(A(Signature.RestoreBattleSnapshotSize) + 2u, oldClear) == false)
        {
            log("unexpected bytes at the patch targets; not patching");
            return;
        }

        // every table reference must agree on one 15-slot table and one 10-entry id table.
        nuint oldSlots = Target(Relocations.Table[0]);
        nuint oldIds = Target(Relocations.Table.First(r => r.Base == RelocationBase.IdBase));
        foreach (var r in Relocations.Table)
        {
            if (Target(r) != RelocationTarget(r, oldSlots, Layout.OldSlots, oldIds))
            {
                log($"{r.Signature} is not as expected; not patching");
                return;
            }
        }

        _BuildActorPrimitiveSheetTable = ImageTarget(A(Signature.BuildActorPrimitiveSheetTableRef), 4); // mov ecx, [r11+rax*4+disp32]
        _ReleaseRenderResourcesPages = RipTarget(A(Signature.ReleaseRenderResourcesPagesRef), 3, 7); // lea rbx, [rip+disp32]

        _PageCtor = (delegate* unmanaged<nuint, nuint>)A(Signature.PageCtor);
        _PageReset = (delegate* unmanaged<nuint, void>)A(Signature.PageReset);
        _CellFree = (delegate* unmanaged<nuint, int, int, void>)A(Signature.CellFree);
        _PaletteAlloc = (delegate* unmanaged<int, uint, int>)A(Signature.PaletteAlloc);

        // The base page loops the handlers mirror must address the same page array.
        if (RipTarget(A(Signature.RenderDrawListPagesRef), 3, 7) != _ReleaseRenderResourcesPages || // lea rsi, [rip+disp32]
            RipTarget(A(Signature.PaletteAllocPagesRef) + 7u, 3, 7) != _ReleaseRenderResourcesPages || // lea rax, [rip+disp32]
            RipTarget(A(Signature.ClearPagesUsed), 3, 7) != _ReleaseRenderResourcesPages + 0x14u) // lea rdx, [rip+disp32]
        {
            log("page loops don't address the page array as expected; not patching");
            return;
        }

        if (AllocateMemory() == false)
        {
            log($"could not allocate near the image; not patching");
            return;
        }

        foreach (nuint P in _ExtraPages)
        {
            _PageCtor(P);
        }

        try
        {
            void AddMidHook(Signature signature, int stolen, delegate* unmanaged<MidHook.Context*, void> fn) =>
                _Hooks.Add(MidHook.Create(hooks, A(signature), stolen, fn));

            AddMidHook(Signature.BuildActorPrimitivePalette, 5, &OnBuildActorPrimitivePalette);
            AddMidHook(Signature.BuildSpritePrimitive, 6, &OnBuildSpritePrimitive);
            AddMidHook(Signature.PrimitiveTextureCell, 7, &OnPrimitiveTextureCell);
            AddMidHook(Signature.DrawEntryTexture, 9, &OnDrawEntryTexture);
            AddMidHook(Signature.RenderDrawListMarkHandles, 6, &OnRenderDrawListMarkHandles);
            AddMidHook(Signature.RenderDrawListPageRect, 9, &OnRenderDrawListPageRect);
            AddMidHook(Signature.RenderDrawListAgeCells, 9, &OnRenderDrawListAgeCells);
            AddMidHook(Signature.ClearPagesUsed, 7, &OnClearPagesUsed);
            AddMidHook(Signature.ReleaseRenderResourcesPages, 5, &OnReleaseRenderResourcesPages);
            AddMidHook(Signature.SaveBattleSnapshot, 6, &OnSaveBattleSnapshot);
            AddMidHook(Signature.RestoreBattleSnapshot, 7, &OnRestoreBattleSnapshot);
        }
        catch (Exception ex)
        {
            log($"could not create hooks ({ex.Message}); not patching");
            _Hooks.Clear();
            return;
        }

        foreach (var h in _Hooks)
        {
            h.Activate();
        }

        ApplyRelocations(oldSlots, oldIds);

        Patch(A(Signature.AllocateSpriteSlotBound1) + 2u, Layout.Slots); // cmp edx, 15 -> 16 (slot allocator)
        Patch(A(Signature.AllocateSpriteSlotBound2) + 2u, Layout.Slots); // ''
        Patch(A(Signature.AllocateSpriteSlotBound3) + 2u, Layout.Slots); // ''
        Patch(A(Signature.InitBattleIdLoop) + 2u, Layout.Slots); // cmp eax, 10 -> 16
        Patch(A(Signature.ClearBattleStateSlots) + 2u, BitConverter.GetBytes(Layout.Slots * Layout.SlotSize)); // memset size 0x32D60
        Patch(A(Signature.RenderDrawListBound) + 3u, BitConverter.GetBytes(Layout.StageDBound)); // cmp r8d, 0x17A -> 0x1DA

        Patch(A(Signature.LoadBattleUnitsLimitMov) + 1u, EntdLimit);
        Patch(A(Signature.LoadBattleUnitsLimitCmp) + 2u, EntdLimit);

        log($"installed, sprite limit is {EntdLimit}");
    }

    // 0x140261846 `cmp edi, 0x11; jne`
    [UnmanagedCallersOnly]
    private static void OnBuildActorPrimitivePalette(MidHook.Context* c)
    {
        // rdi = tpage
        // rsi = actor
        // rbx = primitive entry
        if ((uint)c->rdi != 0x11u)
        {
            return;
        }

        nuint actor = (nuint)c->rsi;
        int slot = Read<byte>(actor + 9u);
        if (slot < Layout.BaseSlots || slot >= Layout.Slots)
        {
            return;
        }

        int i = slot - Layout.BaseSlots;

        byte sprite = Read<byte>(actor + 0xAu);

        int sheet = sprite < 0xAA
            ? Read<int>(_BuildActorPrimitiveSheetTable + sprite * 4u)
            : -1;
        if (sheet == -1)
        {
            c->rip = A(Signature.BuildActorPrimitiveNoSheet);
            return;
        }

        _ExtraSheets[i] = sheet;
        ushort clut = Read<ushort>((nuint)c->rbx + 0x16u);
        int palette = _PaletteAlloc(_ExtraIndices[i], clut);
        c->rax = Layout.ExtraIdBase + (uint)i * 16u + (uint)palette;
        c->rip = A(Signature.BuildActorPrimitiveTail);
    }

    // 0x14025DF40
    [UnmanagedCallersOnly]
    private static void OnBuildSpritePrimitive(MidHook.Context* c)
    {
        // edx = texture ID
        if ((uint)c->rdx - Layout.ExtraIdBase < Layout.ExtraIdCount)
        {
            c->rip = A(Signature.BuildSpritePrimitiveStore);
        }
    }

    // 0x14041B104
    [UnmanagedCallersOnly]
    private static void OnPrimitiveTextureCell(MidHook.Context* c)
    {
        // ecx = ID
        // edx = HD flag
        uint id = (uint)c->rcx;
        if (id - Layout.ExtraIdBase >= Layout.ExtraIdCount || (uint)c->rdx == 0u)
        {
            return;
        }

        // reuses the slot-9 tail (page = r15/16 + 0xD)
        int i = (int)(id - Layout.ExtraIdBase) >> 4;
        c->rsi = id - Layout.IdToIndex;
        c->r15 = (uint)((_ExtraIndices[i] - 0xD) * 16 + (int)((id - Layout.ExtraIdBase) & 15u));
        c->rip = A(Signature.PrimitiveTextureCellTail);
    }

    // 0x14041D87B
    [UnmanagedCallersOnly]
    private static void OnDrawEntryTexture(MidHook.Context* c)
    {
        // r10 = index
        // rbx = draw list
        // esi = 0x800 + index.
        if (IsExtraIndex(c->r10) == false)
        {
            return;
        }

        int i = ExtraSlotFromIndex(c->r10);
        int palette = (int)(c->r10 - Layout.ExtraIndexBase) & 15;
        long offset = (long)Read<int>((nuint)c->rbx) * 0x98;
        nuint entries = Read<nuint>((nuint)c->rbx + 0x10u);
        Write(entries + (nuint)offset + 0x2Cu, (uint)c->rsi);
        c->r8 = Read<uint>(_ExtraPages[i] + 0xA04u + (nuint)(palette * 8));
        c->rax = (uint)_ExtraSheets[i];
        c->rdx = (ulong)offset;
        c->rip = A(Signature.DrawEntryTexturePost);
    }

    // 0x140415EC7
    [UnmanagedCallersOnly]
    private static void OnRenderDrawListMarkHandles(MidHook.Context* c)
    {
        // keep extra-page handles out of a stack array sized for 0x17B handles.
        if (Read<int>((nuint)c->rcx) >= (int)Layout.ExtraHandleMinimum)
        {
            c->rip = A(Signature.RenderDrawListMarkHandlesSkip);
        }
    }

    // 0x140416381
    [UnmanagedCallersOnly]
    private static void OnRenderDrawListPageRect(MidHook.Context* c)
    {
        // r8 = index
        if (IsExtraIndex(c->r8) == false)
        {
            return;
        }
        int i = ExtraSlotFromIndex(c->r8);
        long rect = Read<int>((nuint)(c->rbx + c->r15 + 0x48u));
        c->r8 = (ulong)(rect * 5);
        c->r12 = (uint)_ExtraIndices[i]; // page index
        c->rdi = (ulong)(_ExtraIndices[i] * Layout.PageSize + rect * 0x14); // rect entry
        c->rip = A(Signature.RenderDrawListPageRectPost);
    }

    // 0x140415FF8
    [UnmanagedCallersOnly]
    private static void OnRenderDrawListAgeCells(MidHook.Context* c)
    {
        foreach (nuint page in _ExtraPages)
        {
            for (int i = 0; i < 0x80; i++)
            {
                nuint t = page + 0xB08u + (nuint)(i * 0x90);
                int timer = Read<int>(t);
                if (timer <= 0)
                {
                    continue;
                }

                if (Read<int>(t - 4u) != 0)
                {
                    Write(t, 0x3C);
                    Write(t - 4u, 0);
                    continue;
                }

                int v = timer - 1;
                Write(t, v);
                
                int count = Read<int>(page + 0x5290u);
                if (count > 0x40 || v == 0)
                {
                    _CellFree(page, i, 1);
                    continue; 
                }

                if (v == 0x1E)
                {
                    Write(t - 8u, -1);
                    Write(t + 12u, 0);
                    Write(t - 4u, 0); 
                }
            }
        }
    }

    // 0x14041E3D8
    [UnmanagedCallersOnly]
    private static void OnClearPagesUsed(MidHook.Context* c)
    {
        // per-frame "used" flag clear
        foreach (nuint page in _ExtraPages)
        {
            for (int i = 0; i < 0x80; i++)
            {
                nuint u = page + 0x14u + (nuint)(i * 0x14);
                if (Read<int>(u) == 0)
                {
                    Write(u - 8u, 0);
                }

                Write(u, 0);
            }
            for (int i = 0; i < 16; i++)
            {
                nuint u = page + 0xA08u + (nuint)(i * 8);
                if (Read<int>(u) == 0)
                {
                    Write(u - 4u, -1);
                }

                Write(u, 0);
            }
        }
    }

    // 0x1401E758C
    [UnmanagedCallersOnly]
    private static void OnReleaseRenderResourcesPages(MidHook.Context* c)
    {
        // after base resets its 20 pages.
        for (int i = 0; i < Layout.ExtraSlots; i++)
        {
            _PageReset(_ExtraPages[i]);
            _ExtraSheets[i] = -1;
        }
    }

    // 0x1401D5355
    [UnmanagedCallersOnly]
    private static void OnSaveBattleSnapshot(MidHook.Context* c)
    {
        // right after the snapshot save copies slots 0..14 (0x2FA8A bytes)
        Copy(_SlotTable + Layout.OldSlots * Layout.SlotSize, _Snapshot15, Layout.SlotSize);
    }

    // 0x1402CE788
    [UnmanagedCallersOnly]
    private static void OnRestoreBattleSnapshot(MidHook.Context* c)
    {
        // right after the snapshot restore copies slots 0..14 back
        Copy(_Snapshot15, _SlotTable + Layout.OldSlots * Layout.SlotSize, Layout.SlotSize);
    }

    private static bool Expect(nuint at, params byte[] bytes) =>
        Memory.Instance.ReadRaw(at, bytes.Length).AsSpan().SequenceEqual(bytes);

    private static void Patch(nuint at, params byte[] bytes) =>
        Memory.Instance.SafeWrite(at, bytes);

    private static nuint RipTarget(nuint ins, int dispOffset, int length) =>
        (nuint)((nint)ins + length + Read<int>(ins + (nuint)dispOffset));

    private static nuint ImageTarget(nuint ins, int dispOffset) =>
        _Module + Read<uint>(ins + (nuint)dispOffset);

    private static nuint Target(in Relocation r) => r.RipRelative == true
        ? RipTarget(A(r.Signature), r.DisplacementOffset, r.Length)
        : ImageTarget(A(r.Signature), r.DisplacementOffset);

    private static bool AllocateMemory()
    {
        nuint imageEnd = _Module + (uint)Process.GetCurrentProcess().MainModule!.ModuleMemorySize;
        const int tablesSize = 0x34000 /*slots*/ + 0x100 /*ids*/;
        const int fixedSize = 0x3400 /*snap*/;

        nuint tables = AllocateTables(imageEnd, tablesSize);
        if (tables == 0u)
        {
            return false;
        }

        nuint pagesSize = (nuint)(fixedSize + (Layout.ExtraSlots + 1) * Layout.PageSize);
        nuint block = AllocatePages(imageEnd, pagesSize);
        if (block == 0u)
        {
            Kernel32.VirtualFree(tables, 0u, Kernel32.MEM_ALLOCATION_TYPE.MEM_RELEASE);
            return false;
        }

        _SlotTable = tables;
        _IdTable = tables + 0x34000u;
        _Snapshot15 = block;

        ulong start = block + fixedSize;
        ulong rem = (start - _ReleaseRenderResourcesPages) % (ulong)Layout.PageSize;
        ulong p0 = rem != 0u ? start + ((ulong)Layout.PageSize - rem) : start;
        long k0 = (long)((p0 - _ReleaseRenderResourcesPages) / (ulong)Layout.PageSize);
        for (int i = 0; i < Layout.ExtraSlots; i++)
        {
            _ExtraPages[i] = (nuint)(p0 + (ulong)(i * Layout.PageSize));
            _ExtraIndices[i] = (int)(k0 + i);
            _ExtraSheets[i] = -1;
        }

        return true;
    }

    private static nuint TryAllocate(nuint at, nuint size) =>
        Kernel32.VirtualAlloc(at, size,
            Kernel32.MEM_ALLOCATION_TYPE.MEM_RESERVE | Kernel32.MEM_ALLOCATION_TYPE.MEM_COMMIT,
            Kernel32.MEM_PROTECTION.PAGE_READWRITE);

    private static nuint AllocateTables(nuint imageEnd, nuint size)
    {
        for (nuint at = (imageEnd + 0xFFFFu) & ~(nuint)0xFFFFu; at + size - _Module < 0x7FFFFFFFu; at += 0x10000u)
        {
            nuint block = TryAllocate(at, size);
            if (block != 0u)
            {
                return block;
            }
        }

        for (nuint at = (_Module - size) & ~(nuint)0xFFFFu; imageEnd - at < 0x7FFFFFFFu; at -= 0x10000u)
        {
            nuint block = TryAllocate(at, size);
            if (block != 0u)
            {
                return block;
            }
        }

        return 0u;
    }

    private static nuint AllocatePages(nuint imageEnd, nuint size)
    {
        ulong limit = (ulong)(((1L << 26) - Layout.ExtraSlots - 1) * Layout.PageSize);
        for (nuint at = (imageEnd + 0xFFFFu) & ~(nuint)0xFFFFu; at + size - _ReleaseRenderResourcesPages < limit; at += 0x1000000u)
        {
            nuint block = TryAllocate(at, size);
            if (block != 0u)
            {
                return block;
            }
        }

        return 0u;
    }

    private static void ApplyRelocations(nuint oldSlots, nuint oldIds)
    {
        Copy(oldSlots, _SlotTable, Layout.OldSlots * Layout.SlotSize);
        Copy(oldIds, _IdTable, Layout.OldIds * 4);
        for (int i = Layout.OldIds; i < Layout.Slots; i++)
        {
            Write(_IdTable + (nuint)(i * 4), -1);
        }

        foreach (var r in Relocations.Table)
        {
            nuint ins = A(r.Signature);
            nuint target = RelocationTarget(r, _SlotTable, Layout.Slots, _IdTable);
            long displacement = r.RipRelative == true
                ? (long)target - (long)(ins + (nuint)r.Length)
                : (long)target - (long)_Module;
            Patch(ins + (nuint)r.DisplacementOffset, BitConverter.GetBytes((int)displacement));
        }
    }
}
