using HarmonyLib;
using TheOtherRoles.Players;
using UnityEngine;

namespace TheOtherRoles.Patches {
    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.Awake))]
    public static class PlayerPhysiscsAwakePatch
    {
        [HarmonyPostfix]
        public static void Postfix(PlayerPhysics __instance)
        {
            if (!__instance.body) return;
            __instance.body.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
    public static class PlayerPhysicsFixedUpdatePatch
    {
        public static void Postfix(PlayerPhysics __instance)
        {
            if (__instance.AmOwner &&
                !CachedPlayer.LocalPlayer.Data.IsDead &&
                Invert.invert.FindAll(x => x.PlayerId == CachedPlayer.LocalPlayer.PlayerId).Count > 0 &&
                Invert.meetings > 0 &&
                GameData.Instance &&
                __instance.myPlayer.CanMove)
                __instance.body.velocity *= -1;

            Kataomoi.fixedUpdate(__instance);
        }
    }

    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.WalkPlayerTo))]
    class PlayerPhysicsWalkPlayerToPatch
    {
        private static Vector2 offset = Vector2.zero;
        public static void Prefix(PlayerPhysics __instance)
        {
            bool correctOffset = Camouflager.camouflageTimer <= 0f && (__instance.myPlayer == Mini.mini || (Morphling.morphling != null && __instance.myPlayer == Morphling.morphling && Morphling.morphTarget == Mini.mini && Morphling.morphTimer > 0f));
            correctOffset = correctOffset && !(Mini.mini == Morphling.morphling && Morphling.morphTimer > 0f);
            if (correctOffset)
            {
                float currentScaling = (Mini.growingProgress() + 1) * 0.5f;
                __instance.myPlayer.Collider.offset = currentScaling * Mini.defaultColliderOffset * Vector2.down;
            }
        }
    }
}