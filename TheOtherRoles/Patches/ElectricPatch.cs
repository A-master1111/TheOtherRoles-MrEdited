using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Hazel;
using UnityEngine;
using static TheOtherRoles.GameHistory;
using static TheOtherRoles.TheOtherRoles;

namespace TheOtherRoles.Patches
{
    [Harmony]
    public class ElectricPatch
    {
        public static bool onTask = false;
        public static void reset()
        {
            onTask = false;
        }

        static bool done = false;
        static DateTime lastUpdate;

        [HarmonyPatch(typeof(SwitchMinigame), nameof(SwitchMinigame.Begin))]
        class SwitchMinigameStartPatch
        {
            static void Postfix(VitalsMinigame __instance)
            {
                onTask = true;
                done = false;
            }
        }

        [HarmonyPatch(typeof(SwitchMinigame), nameof(SwitchMinigame.FixedUpdate))]
        class SwitchMinigameClosePatch
        {
            static void Postfix(SwitchMinigame __instance)
            {
                lastUpdate = DateTime.UtcNow;
                DestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) =>
                {
                    if (p == 1f)
                    {
                        float diff = (float)((DateTime.UtcNow - lastUpdate).TotalMilliseconds);
                        if (diff > 100 && !done)
                        {
                            done = true;
                            onTask = false;
                        }
                    }
                })));
            }
        }

    }
}
