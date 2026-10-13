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

namespace WhatSpriteLimit;

using S = Signature;

internal enum Signature
{
    BuildActorPrimitivePalette, // 0x140261846
    BuildSpritePrimitive, // 0x14025DF40
    PrimitiveTextureCell, // 0x14041B104
    DrawEntryTexture, // 0x14041D87B
    RenderDrawListMarkHandles, // 0x140415EC7
    RenderDrawListPageRect, // 0x140416381
    RenderDrawListAgeCells, // 0x140415FF8
    ClearPagesUsed, // 0x14041E3D8
    ReleaseRenderResourcesPages, // 0x1401E758C
    BuildActorPrimitiveNoSheet, // 0x14026194D
    BuildActorPrimitiveTail, // 0x1402618BC
    BuildSpritePrimitiveStore, // 0x14025DF4D
    PrimitiveTextureCellTail, // 0x14041B191
    DrawEntryTexturePost, // 0x14041D974
    RenderDrawListMarkHandlesSkip, // 0x140415EDD
    RenderDrawListPageRectPost, // 0x1404163EC
    PageCtor, // 0x1403D0D4C
    PageReset, // 0x1403D0E50
    CellFree, // 0x1403D11DC
    PaletteAlloc, // 0x14041E448
    RenderDrawListBound, // 0x1404161E5
    LoadBattleUnitsLimitMov, // 0x14027135B  set_monsterwork
    LoadBattleUnitsLimitCmp, // 0x140271370  set_monsterwork
    InitBattleIdLoop, // 0x140210C96  battle
    BuildActorPrimitiveSheetTableRef, // 0x1402618FC
    ReleaseRenderResourcesPagesRef, // 0x1401E756A
    RenderDrawListPagesRef, // 0x140415F72
    PaletteAllocPagesRef, // 0x14041E452
    FieldActorSlot, // 0x140260E7E
    FieldActorSprite, // 0x140261714
    FieldPrimitiveCLUT, // 0x140261836
    FieldDrawEntry, // 0x14041D886
    FieldRect, // 0x1404163CD
    FieldCellTimer, // 0x140415F7F
    FieldCellExpire, // 0x140415F9F
    FieldCellReset, // 0x140415FC8
    FieldCellUsed, // 0x14041E3F1
    FieldPaletteUsed, // 0x14041E412
    FieldPalette, // 0x14041E4A4
    SaveBattleSnapshot, // 0x1401D5355  RESUME_SET_WORK_MU
    RestoreBattleSnapshot, // 0x1402CE788  ios_ResumeBeforeBattle
    AllocateSpriteSlotBound1, // 0x140269018  getFontID
    AllocateSpriteSlotBound2, // 0x14026904E  getFontID
    AllocateSpriteSlotBound3, // 0x140269068  getFontID
    ClearBattleStateSlots, // 0x1401D457B  ClearWorkingMemory
    SaveBattleSnapshotSize, // 0x1401D5343  RESUME_SET_WORK_MU
    RestoreBattleSnapshotSize, // 0x1402CE732  ios_ResumeBeforeBattle
    SlotRef00, // 0x1401D4574  ClearWorkingMemory
    SlotRef01, // 0x1401D5141  RESUME_SET_WORK_MU
    SlotRef02, // 0x1401D5349  RESUME_SET_WORK_MU
    SlotRef03, // 0x140210BF9  battle
    SlotRef04, // 0x140210C03  battle
    SlotRef05, // 0x1402127E7  battle
    SlotRef06, // 0x140268FFA  getFontID
    SlotRef07, // 0x140269091  Event_SetAnimationBrightColor
    SlotRef08, // 0x1402690AE  Event_SetAnimationBrightColor
    SlotRef09, // 0x14026922F  Event_SetAnimationBrightColor
    SlotRef10, // 0x1402692D9  loadClutData
    SlotRef11, // 0x14026A2DB  requestDefaultGraphic
    SlotRef12, // 0x14026C3A0  expandExtendPattern
    SlotRef13, // 0x14026E167  EndAnimation
    SlotRef14, // 0x14026E87D
    SlotRef15, // 0x14026E884
    SlotRef16, // 0x14026F5FB  InitAnimationInit
    SlotRef17, // 0x14026F605  InitAnimationInit
    SlotRef18, // 0x14026FD27  entryAnimations
    SlotRef19, // 0x14026FD3D  entryAnimations
    SlotRef20, // 0x140273516  activateCompressedAnimation
    SlotRef21, // 0x1402CE73E  ios_ResumeBeforeBattle
    IdRef00, // 0x1401F3902
    IdRef01, // 0x140210C83  battle
    IdRef02, // 0x140214168
    IdRef03, // 0x140215093  FillUnitStatusWindow
    IdRef04, // 0x14021567C  DrawCharacterCombatTimeline_maybe
    IdRef05, // 0x14021FD29  drawmessagepolygons
    IdRef06, // 0x14022197D  GetFaceClutFromUnitPalette
    IdRef07, // 0x14022992C  disp_status_common
    IdRef08, // 0x14026E88B
    IdRef09, // 0x14026E9BB
    IdRef10, // 0x1402E0536  disp_status2sub
    IdRef11, // 0x140304C1E  ResumeSaveEffectState
    IdRef12, // 0x140304C29  ResumeSaveEffectState
    IdRef13, // 0x140304C35  ResumeSaveEffectState
    IdRef14, // 0x140304CBB  ResumeSaveEffectState
    IdRef15, // 0x140304CC5  ResumeSaveEffectState
    IdRef16, // 0x140304CD0  ResumeSaveEffectState
    IdRef17, // 0x140339789  Ddisp_personalwindow
    IdRef18, // 0x140398DFA
}

internal static class Signatures
{
    public static readonly (S Signature, string Pattern, int Offset)[] Patterns =
    {
        (S.BuildActorPrimitivePalette, "83 FF 11 75 ?? 83 3D ?? ?? ?? ?? FF", 0),
        (S.BuildSpritePrimitive, "8D 82 7D D8 FF FF", 0),
        (S.PrimitiveTextureCell, "44 8D B9 7D D8 FF FF", 0),
        (S.DrawEntryTexture, "41 8D 42 85 3D 8F 00 00 00", 0),
        (S.RenderDrawListMarkHandles, "81 39 00 08 00 00", 0),
        (S.RenderDrawListPageRect, "41 8D 40 85 3D 8F 00 00 00", 0),
        (S.RenderDrawListAgeCells, "7C ?? 45 85 ED 0F 84 ?? ?? ?? ??", 2),
        (S.ClearPagesUsed, "48 8D 15 ?? ?? ?? ?? 41 BA 14 00 00 00", 0),
        (S.ReleaseRenderResourcesPages, "48 3B D8 75 ?? E8 ?? ?? ?? ?? E8 ?? ?? ?? ??", 5),
        (S.BuildActorPrimitiveNoSheet, "41 BD 0D 00 00 00 4C 8B 65 70", 0),
        (S.BuildActorPrimitiveTail, "4C 8B 4C 24 28 41 BD 0D 00 00 00", 0),
        (S.BuildSpritePrimitiveStore, "89 57 40 41 8B DE", 0),
        (S.PrimitiveTextureCellTail, "66 44 39 4B 44 74 ??", 0),
        (S.DrawEntryTexturePost, "48 8B 4B 10 44 8B 4C 24 58", 0),
        (S.RenderDrawListMarkHandlesSkip, "48 03 CB 48 83 EA 01 75 ?? 41 8B F1", 0),
        (S.RenderDrawListPageRectPost, "48 8D 05 ?? ?? ?? ?? 41 8B D2", 0),
        (S.PageCtor, "48 89 5C 24 08 48 89 6C 24 10 48 89 74 24 18 57 48 83 EC 20 BD 80 00 00 00", 0),
        (S.PageReset, "C3 CC CC 48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 20 48 8B D9 33 FF", 3),
        (S.CellFree, "C3 CC 40 53 48 83 EC 20 48 63 C2", 2),
        (S.PaletteAlloc, "48 89 5C 24 08 48 63 C1", 0),
        (S.RenderDrawListBound, "41 81 F8 7A 01 00 00", 0),
        (S.LoadBattleUnitsLimitMov, "B9 09 00 00 00 44 89 3D ?? ?? ?? ?? 2B C8", 0),
        (S.LoadBattleUnitsLimitCmp, "83 F8 09 49 8B D7", 0),
        (S.InitBattleIdLoop, "48 8D 49 04 83 F8 0A", 4),
        (S.BuildActorPrimitiveSheetTableRef, "41 8B 8C 83 ?? ?? ?? ?? 41 3B C8", 0),
        (S.ReleaseRenderResourcesPagesRef, "E8 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 1D ?? ?? ?? ?? 48 8B CB", 10),
        (S.RenderDrawListPagesRef, "48 8D 35 ?? ?? ?? ?? 44 8B E7", 0),
        (S.PaletteAllocPagesRef, "4C 69 C0 98 52 00 00 48 8D 05 ?? ?? ?? ??", 0),
        (S.FieldActorSlot, "0F B6 46 09 66 83 E8 07", 0),
        (S.FieldActorSprite, "05 43 27 00 00 E9 ?? ?? ?? ?? 80 7E 0A AA 0F 83 ?? ?? ?? ?? 0F B6 46 0A", 10),
        (S.FieldPrimitiveCLUT, "8D 48 F8 0F B7 53 16", 3),
        (S.FieldDrawEntry, "48 63 0B 48 69 D1 98 00 00 00 48 8B 4B 10 89 74 0A 2C", 0),
        (S.FieldRect, "4A 63 44 3B 48 41 C1 FC 04 41 83 C4 0D 48 8D 14 80 49 63 C4 48 69 C8 98 52 00 00 48 8D 3C 91", 0),
        (S.FieldCellTimer, "48 8D 9E 08 0B 00 00 8B 03 85 C0 7E ?? 44 39 4B FC 75 ?? FF C8 89 03 83 BE ?? ?? ?? ?? 40", 0),
        (S.FieldCellExpire, "83 F8 1E 75 ?? 83 4B F8 FF 44 89 4B 0C", 0),
        (S.FieldCellReset, "C7 03 3C 00 00 00 44 89 4B FC 41 FF C6 48 81 C3 90 00 00 00", 0),
        (S.FieldCellUsed, "41 B9 80 00 00 00 44 39 19 75 ?? 49 8D 04 10 44 89 5C 08 F8 44 89 19 48 83 C1 14", 0),
        (S.FieldPaletteUsed, "48 8D 82 F4 09 00 00 41 8D 49 10 44 39 18 75 ?? 83 48 FC FF 44 89 18 48 83 C0 08", 0),
        (S.FieldPalette, "48 63 C1 41 89 9C C0 04 0A 00 00 EB ?? 48 63 C1 41 C7 84 C0 08 0A 00 00 01 00 00 00", 0),
        (S.SaveBattleSnapshot, "8B 05 ?? ?? ?? ?? 89 83 A8 04 00 00", 0),
        (S.RestoreBattleSnapshot, "F3 0F 7F 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ??", 13),
        (S.AllocateSpriteSlotBound1, "41 3B C1 74 ?? FF C2 83 FA 0F", 7),
        (S.AllocateSpriteSlotBound2, "42 80 3C 10 00 74 ?? FF C2 83 FA 0F", 9),
        (S.AllocateSpriteSlotBound3, "0F B7 C2 83 FA 0F", 3),
        (S.ClearBattleStateSlots, "41 B8 8A FA 02 00 E8 ?? ?? ?? ??", 0),
        (S.SaveBattleSnapshotSize, "41 B8 8A FA 02 00 48 8D 15 ?? ?? ?? ??", 0),
        (S.RestoreBattleSnapshotSize, "48 89 01 41 B8 8A FA 02 00", 3),
        (S.SlotRef00, "48 8D 0D ?? ?? ?? ?? 41 B8 8A FA 02 00", 0),
        (S.SlotRef01, "48 8D 2D ?? ?? ?? ?? 4C 8D 3D ?? ?? ?? ??", 0),
        (S.SlotRef03, "48 8D 05 ?? ?? ?? ?? 44 88 28", 0),
        (S.SlotRef04, "44 88 28 48 8D 0D ?? ?? ?? ??", 3),
        (S.SlotRef05, "48 8D 05 ?? ?? ?? ?? 0F B6 3C 02 48 8D 04 49", 0),
        (S.SlotRef06, "4C 8D 15 ?? ?? ?? ?? 4C 63 C2", 0),
        (S.SlotRef07, "0F B6 41 09 4C 8D 0D ?? ?? ?? ??", 4),
        (S.SlotRef08, "48 69 D0 D6 32 00 00 4C 8D 05 ?? ?? ?? ??", 7),
        (S.SlotRef09, "41 8B CF 48 8D 05 ?? ?? ?? ??", 3),
        (S.SlotRef10, "48 03 D0 48 8D 05 ?? ?? ?? ?? EB ??", 3),
        (S.SlotRef11, "0F B6 41 09 4C 8D 05 ?? ?? ?? ??", 4),
        (S.SlotRef12, "48 8D 05 ?? ?? ?? ?? 4C 8B C3 48 03 D0", 0),
        (S.SlotRef13, "44 88 9C 19 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ??", 0),
        (S.SlotRef14, "41 0F B6 48 0A 4C 8D 9A ?? ?? ?? ??", 5),
        (S.SlotRef15, "48 8D B2 ?? ?? ?? ?? 89 8C 82 ?? ?? ?? ??", 0),
        (S.SlotRef16, "48 8D 05 ?? ?? ?? ?? 44 88 00", 0),
        (S.SlotRef17, "44 88 00 48 8D 0D ?? ?? ?? ?? 48 05 D6 32 00 00", 3),
        (S.SlotRef18, "44 0F BF C2 48 8D 0D ?? ?? ?? ??", 4),
        (S.SlotRef19, "48 81 C1 D6 32 00 00 48 8D 05 ?? ?? ?? ??", 7),
        (S.SlotRef20, "48 8D 05 ?? ?? ?? ?? 4C 89 44 24 28", 0),
        (S.SlotRef21, "41 B8 8A FA 02 00 8B 05 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ??", 12),
        (S.IdRef00, "8B 9C 9A ?? ?? ?? ?? 48 63 8D D8 00 00 00", 0),
        (S.IdRef01, "48 8D 0D ?? ?? ?? ?? 41 8B C5", 0),
        (S.IdRef02, "48 83 FA 0A 73 ?? 41 8B 94 90 ?? ?? ?? ??", 6),
        (S.IdRef03, "48 83 FF 0A 73 ?? 8B BC BA ?? ?? ?? ??", 6),
        (S.IdRef04, "8B 9C 9A ?? ?? ?? ?? 48 63 8C 24 A0 01 00 00", 0),
        (S.IdRef05, "8B 84 81 ?? ?? ?? ?? 8B D0 8B CF", 0),
        (S.IdRef06, "45 8B 8C 82 ?? ?? ?? ?? 41 8B C1", 0),
        (S.IdRef07, "66 03 8C 46 ?? ?? 67 00", 264),
        (S.IdRef08, "89 8C 82 ?? ?? ?? ?? 41 0F B6 40 09", 0),
        (S.IdRef09, "89 8C 82 ?? ?? ?? ?? 83 FB 09", 0),
        (S.IdRef10, "66 03 8C 46 ?? ?? 68 00", 264),
        (S.IdRef11, "C3 0F 10 01 0F 29 05 ?? ?? ?? ??", 4),
        (S.IdRef12, "0F 29 0D ?? ?? ?? ?? F2 0F 10 41 20", 0),
        (S.IdRef13, "F2 0F 11 05 ?? ?? ?? ?? 41 0F 10 00", 0),
        (S.IdRef15, "C3 0F 28 05 ?? ?? ?? ?? 0F 11 01 0F 28 0D ?? ?? ?? ??", 11),
        (S.IdRef16, "0F 11 49 10 F2 0F 10 05 ?? ?? ?? ??", 4),
        (S.IdRef17, "48 8B 0D ?? ?? ?? ?? 4C 8D 96 2F 03 00 00 48 89 45 10 45 8B F5 66 44 89 BD D8 00 00 00", -62),
        (S.IdRef18, "48 8B 0D ?? ?? ?? ?? 4C 8D 96 2F 03 00 00 48 89 45 10 45 8B F5 66 44 89 BD C8 00 00 00", -62),
    };

    public static readonly (S Signature, S From, int Delta)[] Derived =
    {
        (S.SlotRef02, S.SaveBattleSnapshotSize, 6),
        (S.IdRef14, S.IdRef15, -10),
    };
}
