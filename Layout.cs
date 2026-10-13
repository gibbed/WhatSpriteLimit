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

internal static class Layout
{
    public const int Slots = 16;
    public const int OldSlots = 15;
    public const int SlotSize = 0x32D6;

    public const int OldIds = 10;

    public const int BaseSlots = 10; // HD handles slots 0..9
    public const int ExtraSlots = Slots - BaseSlots; // 6: slots 10..15

    public const long PageSize = 0x5298;

    public const uint ExtraIdBase = 0x2883u;
    public const uint IdToIndex = 0x2708u;
    public const uint ExtraIndexBase = ExtraIdBase - IdToIndex; // 0x17B
    public const uint ExtraIdCount = ExtraSlots * 16; // 0x60
    public const uint ExtraHandleMinimum = 0x800u + ExtraIndexBase; // 0x97B

    public const ushort RenderDrawListBound = (ushort)(ExtraIndexBase + ExtraIdCount - 1u); // 0x1DA
}
