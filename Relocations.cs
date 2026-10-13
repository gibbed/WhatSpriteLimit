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

using RB = RelocationBase;
using S = Signature;

internal enum RelocationBase
{
    SlotBase,
    SlotEnd,
    IdBase,
}

internal readonly record struct Relocation(S Signature, int Length, int DisplacementOffset, bool RipRelative, RB Base, uint Offset);

internal static class Relocations
{
    public static readonly Relocation[] Table =
    {
        new(S.SlotRef00, 7, 3, true, RB.SlotBase, 0x0u), // lea rcx, [rip + 0xb2c025]
        new(S.SlotRef01, 7, 3, true, RB.SlotBase, 0x0u), // lea rbp, [rip + 0xb2b458]
        new(S.SlotRef02, 7, 3, true, RB.SlotBase, 0x0u), // lea rdx, [rip + 0xb2b250]
        new(S.SlotRef03, 7, 3, true, RB.SlotBase, 0x1u), // lea rax, [rip + 0xaef9a1]
        new(S.SlotRef04, 7, 3, true, RB.SlotEnd,  0x1u), // lea rcx, [rip + 0xb1f421]
        new(S.SlotRef05, 7, 3, true, RB.SlotBase, 0x1u), // lea rax, [rip + 0xaeddb3]
        new(S.SlotRef06, 7, 3, true, RB.SlotBase, 0x0u), // lea r10, [rip + 0xa9759f]
        new(S.SlotRef07, 7, 3, true, RB.SlotBase, 0x30D6u), // lea r9, [rip + 0xa9a5de]
        new(S.SlotRef08, 7, 3, true, RB.SlotBase, 0x0u), // lea r8, [rip + 0xa974eb]
        new(S.SlotRef09, 7, 3, true, RB.SlotBase, 0x30D6u), // lea rax, [rip + 0xa9a440]
        new(S.SlotRef10, 7, 3, true, RB.SlotBase, 0x30D6u), // lea rax, [rip + 0xa9a396]
        new(S.SlotRef11, 7, 3, true, RB.SlotBase, 0x1u), // lea r8, [rip + 0xa962bf]
        new(S.SlotRef12, 7, 3, true, RB.SlotBase, 0x2u), // lea rax, [rip + 0xa941fb]
        new(S.SlotRef13, 8, 4, false, RB.SlotBase, 0x0u), // mov byte ptr [rcx + rbx + 0xd005a0], r11b
        new(S.SlotRef14, 7, 3, false, RB.SlotBase, 0x30D6u), // lea r11, [rdx + 0xd03676]
        new(S.SlotRef15, 7, 3, false, RB.SlotBase, 0x2u), // lea rsi, [rdx + 0xd005a2]
        new(S.SlotRef16, 7, 3, true, RB.SlotBase, 0x0u), // lea rax, [rip + 0xa90f9e]
        new(S.SlotRef17, 7, 3, true, RB.SlotEnd,  0x0u), // lea rcx, [rip + 0xac0a1e]
        new(S.SlotRef18, 7, 3, true, RB.SlotBase, 0x1u), // lea rcx, [rip + 0xa90873]
        new(S.SlotRef19, 7, 3, true, RB.SlotEnd,  0x1u), // lea rax, [rip + 0xac02e7]
        new(S.SlotRef20, 7, 3, true, RB.SlotBase, 0x2u), // lea rax, [rip + 0xa8d085]
        new(S.SlotRef21, 7, 3, true, RB.SlotBase, 0x0u), // lea rcx, [rip + 0xa31e5b]
        new(S.IdRef00, 7, 3, false, RB.IdBase, 0x0u), // mov ebx, dword ptr [rdx + rbx*4 + 0x37a59e0]
        new(S.IdRef01, 7, 3, true, RB.IdBase, 0x0u), // lea rcx, [rip + 0x3594d56]
        new(S.IdRef02, 8, 4, false, RB.IdBase, 0x0u), // mov edx, dword ptr [r8 + rdx*4 + 0x37a59e0]
        new(S.IdRef03, 7, 3, false, RB.IdBase, 0x0u), // mov edi, dword ptr [rdx + rdi*4 + 0x37a59e0]
        new(S.IdRef04, 7, 3, false, RB.IdBase, 0x0u), // mov ebx, dword ptr [rdx + rbx*4 + 0x37a59e0]
        new(S.IdRef05, 7, 3, false, RB.IdBase, 0x0u), // mov eax, dword ptr [rcx + rax*4 + 0x37a59e0]
        new(S.IdRef06, 8, 4, false, RB.IdBase, 0x0u), // mov r9d, dword ptr [r10 + rax*4 + 0x37a59e0]
        new(S.IdRef07, 8, 4, false, RB.IdBase, 0x0u), // mov r11d, dword ptr [rsi + r11*4 + 0x37a59e0]
        new(S.IdRef08, 7, 3, false, RB.IdBase, 0x0u), // mov dword ptr [rdx + rax*4 + 0x37a59e0], ecx
        new(S.IdRef09, 7, 3, false, RB.IdBase, 0x0u), // mov dword ptr [rdx + rax*4 + 0x37a59e0], ecx
        new(S.IdRef10, 8, 4, false, RB.IdBase, 0x0u), // mov r11d, dword ptr [rsi + r11*4 + 0x37a59e0]
        new(S.IdRef11, 7, 3, true, RB.IdBase, 0x0u), // movaps xmmword ptr [rip + 0x34a0dbb], xmm0
        new(S.IdRef12, 7, 3, true, RB.IdBase, 0x10u), // movaps xmmword ptr [rip + 0x34a0dc0], xmm1
        new(S.IdRef13, 8, 4, true, RB.IdBase, 0x20u), // movsd qword ptr [rip + 0x34a0dc3], xmm0
        new(S.IdRef14, 7, 3, true, RB.IdBase, 0x0u), // movaps xmm0, xmmword ptr [rip + 0x34a0d1e]
        new(S.IdRef15, 7, 3, true, RB.IdBase, 0x10u), // movaps xmm1, xmmword ptr [rip + 0x34a0d24]
        new(S.IdRef16, 8, 4, true, RB.IdBase, 0x20u), // movsd xmm0, qword ptr [rip + 0x34a0d28]
        new(S.IdRef17, 7, 3, false, RB.IdBase, 0x0u), // mov eax, dword ptr [rax + rcx*4 + 0x37a59e0]
        new(S.IdRef18, 7, 3, false, RB.IdBase, 0x0u), // mov eax, dword ptr [rax + rcx*4 + 0x37a59e0]
    };
}
