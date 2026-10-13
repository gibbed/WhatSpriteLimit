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

using WhatSpriteLimit.Template;
using IReloadedHooks = Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;

#if DEBUG
using System.Diagnostics;
#endif

namespace WhatSpriteLimit;

public class Mod : ModBase
{
    public Mod(ModContext context)
    {
        var loader = context.ModLoader;
        var logger = context.Logger;

        void Log(string message) => logger.WriteLine($"[What Sprite Limit?] {message}");

        if (loader.GetController<IReloadedHooks>().TryGetTarget(out var hooks) == false)
        {
            Log("reloaded.sharedlib.hooks is not loaded; not patching");
            return;
        }

        if (loader.GetController<IStartupScanner>().TryGetTarget(out var scanner) == false)
        {
            Log("Reloaded.Memory.SigScan.ReloadedII is not loaded; not patching");
            return;
        }

#if DEBUG
        // Attaches debugger in debug mode; ignored in release.
        Debugger.Launch();
#endif

        // the roof... the roof...
        TheRoofIsOnFire.Start(loader, hooks, scanner, Log);
    }

    #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Mod() { }
#pragma warning restore CS8618
    #endregion
}
