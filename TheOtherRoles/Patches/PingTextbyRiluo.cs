using HarmonyLib;
using Reactor.Networking;
using TheOtherRoles;
using UnityEngine;

namespace TheOtherRolesMR.PingTextByRluo
{
    //[HarmonyPriority(Priority.VeryHigh)] // to show this message first, or be overrided if any plugins do
    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    public static class PingText
    {

        [HarmonyPostfix]
        public static void Postfix(PingTracker __instance)
        {
            var position = __instance.GetComponent<AspectPosition>();
            position.DistanceFromEdge = new Vector3(3.6f, 0.1f, 0);
            position.AdjustPosition();

            __instance.text.text =
                $"{(ModTranslation.GetString("Mod-PingText",1))}" + (AmongUsClient.Instance.Ping) + $@"{(ModTranslation.GetString("Mod-PingText", 2))}" +
                (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started
                    ? "" : "");
        }
    }
}
