using HarmonyLib;
using System;
using Hazel;
using UnityEngine;
using System.Linq;
using static TheOtherRoles.TheOtherRoles;
using static TheOtherRoles.GameHistory;
using static TheOtherRoles.MapOptions;
using System.Collections.Generic;
using TheOtherRoles.Players;
using TheOtherRoles.Utilities;
using TheOtherRoles.Objects;
using TheOtherRoles.CustomGameModes;

namespace TheOtherRoles.Patches {

    [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    public static class VentCanUsePatch
    {
        public static bool Prefix(Vent __instance, ref float __result, [HarmonyArgument(0)] GameData.PlayerInfo pc, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse) {
            float num = float.MaxValue;

            // Task Vs Mode
            if (CustomOptionHolder.enabledTaskVsMode.getBool()) {
                canUse = false;
                couldUse = false;
                __result = num;
                return false;
            }

            PlayerControl @object = pc.Object;

            bool roleCouldUse = @object.roleCanUseVents();

            if (__instance.name.StartsWith("SealedVent_")) {
                canUse = couldUse = false;
                __result = num;
                return false;
            }

            // Submerged Compatability if needed:
            if (SubmergedCompatibility.IsSubmerged) {
                // as submerged does, only change stuff for vents 9 and 14 of submerged. Code partially provided by AlexejheroYTB
                if (SubmergedCompatibility.getInTransition()) {
                    __result = float.MaxValue;
                    return canUse = couldUse = false;
                }                
                switch (__instance.Id) {
                    case 9:  // Cannot enter vent 9 (Engine Room Exit Only Vent)!
                        if (CachedPlayer.LocalPlayer.PlayerControl.inVent) break;
                        __result = float.MaxValue;
                        return canUse = couldUse = false;                    
                    case 14: // Lower Central
                        __result = float.MaxValue;
                        couldUse = roleCouldUse && !pc.IsDead && (@object.CanMove || @object.inVent);
                        canUse = couldUse;
                        if (canUse) {
                            Vector3 center = @object.Collider.bounds.center;
                            Vector3 position = __instance.transform.position;
                            __result = Vector2.Distance(center, position);
                            canUse &= __result <= __instance.UsableDistance;
                        }
                        return false;
                }
            }

            var usableDistance = __instance.UsableDistance;
            if (__instance.name.StartsWith("JackInTheBoxVent_")) {
                if(Trickster.trickster != CachedPlayer.LocalPlayer.PlayerControl) {
                    // Only the Trickster can use the Jack-In-The-Boxes!
                    canUse = false;
                    couldUse = false;
                    __result = num;
                    return false;
                } else {
                    // Reduce the usable distance to reduce the risk of gettings stuck while trying to jump into the box if it's placed near objects
                    usableDistance = 0.4f;
                }
            }

            couldUse = (@object.inVent || roleCouldUse) && !pc.IsDead && (@object.CanMove || @object.inVent);
            canUse = couldUse;
            if (canUse) {
                Vector3 center = @object.Collider.bounds.center;
                Vector3 position = __instance.transform.position;
                num = Vector2.Distance(center, position);
                canUse &= (num <= usableDistance && !PhysicsHelpers.AnythingBetween(@object.Collider, center, position, Constants.ShipOnlyMask, false));
            }
            __result = num;
            return false;
        }
    }

    [HarmonyPatch(typeof(VentButton), nameof(VentButton.DoClick))]
    class VentButtonDoClickPatch
    {
        static bool Prefix(VentButton __instance) {
            // Manually modifying the VentButton to use Vent.Use again in order to trigger the Vent.Use prefix patch
		    if (__instance.currentTarget != null && !Deputy.handcuffedKnows.ContainsKey(CachedPlayer.LocalPlayer.PlayerId)) __instance.currentTarget.Use();
            return false;
        }
    }

    [HarmonyPatch(typeof(Vent), nameof(Vent.Use))]
    public static class VentUsePatch
    {
        public static bool Prefix(Vent __instance) {
            // Deputy handcuff disables the vents
            if (Deputy.handcuffedPlayers.Contains(CachedPlayer.LocalPlayer.PlayerId)) {
                Deputy.setHandcuffedKnows();
                return false;
            }
            if (Trapper.playersOnMap.Contains(CachedPlayer.LocalPlayer.PlayerControl)) return false;

            bool canUse;
            bool couldUse;
            __instance.CanUse(CachedPlayer.LocalPlayer.Data, out canUse, out couldUse);
            if (!canUse) return false; // No need to execute the native method as using is disallowed anyways

            bool isEnter = !CachedPlayer.LocalPlayer.PlayerControl.inVent;
            bool canMoveInVents = CachedPlayer.LocalPlayer.PlayerControl != Spy.spy && !Trapper.playersOnMap.Contains(CachedPlayer.LocalPlayer.PlayerControl);
            if (canMoveInVents) canMoveInVents = CachedPlayer.LocalPlayer.PlayerControl != Madmate.madmate;
            if (canMoveInVents)
			{
                if (CachedPlayer.LocalPlayer.PlayerControl == MadmateKiller.madmateKiller && 
                    CachedPlayer.LocalPlayer.PlayerControl.Data.RoleType == RoleTypes.Crewmate &&
                    !MadmateKiller.canMoveVents)
				{
                    canMoveInVents = false;
                }
            }
            
            if (__instance.name.StartsWith("JackInTheBoxVent_")) {
                __instance.SetButtons(isEnter && canMoveInVents);
                MessageWriter writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UseUncheckedVent, Hazel.SendOption.Reliable);
                writer.WritePacked(__instance.Id);
                writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                writer.Write(isEnter ? byte.MaxValue : (byte)0);
                writer.EndMessage();
                RPCProcedure.useUncheckedVent(__instance.Id, CachedPlayer.LocalPlayer.PlayerId, isEnter ? byte.MaxValue : (byte)0);
                SoundEffectsManager.play("tricksterUseBoxVent");
                return false;
            }

            if (isEnter) {
                CachedPlayer.LocalPlayer.PlayerPhysics.RpcEnterVent(__instance.Id);
            } else {
                CachedPlayer.LocalPlayer.PlayerPhysics.RpcExitVent(__instance.Id);
            }
            __instance.SetButtons(isEnter && canMoveInVents);
            return false;
        }
    }
    [HarmonyPatch(typeof(Vent), nameof(Vent.EnterVent))]
    public static class EnterVentAnimPatch
    {
        public static bool Prefix(Vent __instance, [HarmonyArgument(0)] PlayerControl pc) {
            return pc.AmOwner;
        }
    }

    [HarmonyPatch(typeof(Vent), nameof(Vent.ExitVent))]
    public static class ExitVentAnimPatch
    {
        public static bool Prefix(Vent __instance, [HarmonyArgument(0)] PlayerControl pc) {
            return pc.AmOwner;
        }
    }

    [HarmonyPatch(typeof(Vent), nameof(Vent.MoveToVent))]
    public static class MoveToVentPatch {
        public static bool Prefix(Vent otherVent) {
            bool b = !Trapper.playersOnMap.Contains(CachedPlayer.LocalPlayer.PlayerControl);
            if (b && CachedPlayer.LocalPlayer.PlayerControl.cosmetics.Visible)
            {
                RPCProcedure.ventMoveInvisible(CachedPlayer.LocalPlayer.PlayerId);
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.VentMoveInvisible, Hazel.SendOption.Reliable, -1);
                writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
            return b;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    class VentButtonVisibilityPatch
    {
        static void Postfix(PlayerControl __instance) {
            if (__instance.AmOwner && __instance.roleCanUseVents() && FastDestroyableSingleton<HudManager>.Instance.ReportButton.isActiveAndEnabled) {
                FastDestroyableSingleton<HudManager>.Instance.ImpostorVentButton.Show();
            }
        }
    }

    [HarmonyPatch(typeof(VentButton), nameof(VentButton.SetTarget))]
    class VentButtonSetTargetPatch
    {
        static Sprite defaultVentSprite = null;
        static void Postfix(VentButton __instance) {
            // Trickster render special vent button
            if (Trickster.trickster != null && Trickster.trickster == CachedPlayer.LocalPlayer.PlayerControl) {
                if (defaultVentSprite == null) defaultVentSprite = __instance.graphic.sprite;
                bool isSpecialVent = __instance.currentTarget != null && __instance.currentTarget.gameObject != null && __instance.currentTarget.gameObject.name.StartsWith("JackInTheBoxVent_");
                __instance.graphic.sprite = isSpecialVent ? Trickster.getTricksterVentButtonSprite() : defaultVentSprite;
                __instance.buttonLabelText.enabled = !isSpecialVent;
            }
        }
    }

    [HarmonyPatch(typeof(KillButton), nameof(KillButton.SetTarget))]
    public static class KillButtonSetTargetPatch
    {
        public static void Postfix(KillButton __instance, [HarmonyArgument(0)] PlayerControl target)
        {
            if (!CustomOptionHolder.impostorCanKillCustomRolesInTheVent.getBool() && target.roleCanUseVents() && target.inVent)
                __instance.SetTarget(null);
        }
    }

    [HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
    class KillButtonDoClickPatch
    {
        public static bool Prefix(KillButton __instance) {
            if (__instance.isActiveAndEnabled && __instance.currentTarget && !__instance.isCoolingDown && !CachedPlayer.LocalPlayer.Data.IsDead && CachedPlayer.LocalPlayer.PlayerControl.CanMove) {
                // Deputy handcuff update.
                if (Deputy.handcuffedPlayers.Contains(CachedPlayer.LocalPlayer.PlayerId)) {
                    Deputy.setHandcuffedKnows();
                    return false;
                }

                // Use an unchecked kill command, to allow shorter kill cooldowns etc. without getting kicked
                MurderAttemptResult res = Helpers.checkMurderAttemptAndKill(CachedPlayer.LocalPlayer.PlayerControl, __instance.currentTarget);
                // Handle blank kill
                if (res == MurderAttemptResult.BlankKill) {
                    CachedPlayer.LocalPlayer.PlayerControl.killTimer = PlayerControl.GameOptions.KillCooldown;
                    if (CachedPlayer.LocalPlayer.PlayerControl == Cleaner.cleaner)
                        Cleaner.cleaner.killTimer = HudManagerStartPatch.cleanerCleanButton.Timer = HudManagerStartPatch.cleanerCleanButton.MaxTimer;
                    else if (CachedPlayer.LocalPlayer.PlayerControl == Warlock.warlock)
                        Warlock.warlock.killTimer = HudManagerStartPatch.warlockCurseButton.Timer = HudManagerStartPatch.warlockCurseButton.MaxTimer;
                    else if (CachedPlayer.LocalPlayer.PlayerControl == Mini.mini && Mini.mini.Data.Role.IsImpostor)
                        Mini.mini.SetKillTimer(PlayerControl.GameOptions.KillCooldown * (Mini.isGrownUp() ? 0.66f : 2f));
                    else if (CachedPlayer.LocalPlayer.PlayerControl == Witch.witch)
                        Witch.witch.killTimer = HudManagerStartPatch.witchSpellButton.Timer = HudManagerStartPatch.witchSpellButton.MaxTimer;
                    else if (CachedPlayer.LocalPlayer.PlayerControl == Ninja.ninja)
                        Ninja.ninja.killTimer = HudManagerStartPatch.ninjaButton.Timer = HudManagerStartPatch.ninjaButton.MaxTimer;
                }
                __instance.SetTarget(null);
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(SabotageButton), nameof(SabotageButton.Refresh))]
    class SabotageButtonRefreshPatch
    {
        static void Postfix() {
            // Mafia disable sabotage button for Janitor and sometimes for Mafioso
            bool blockSabotageJanitor = (Janitor.janitor != null && Janitor.janitor == CachedPlayer.LocalPlayer.PlayerControl);
            bool blockSabotageMafioso = (Mafioso.mafioso != null && Mafioso.mafioso == CachedPlayer.LocalPlayer.PlayerControl && Godfather.godfather != null && !Godfather.godfather.Data.IsDead);
            if (blockSabotageJanitor || blockSabotageMafioso) {
                FastDestroyableSingleton<HudManager>.Instance.SabotageButton.SetDisabled();
            }
        }
    }

    [HarmonyPatch(typeof(ReportButton), nameof(ReportButton.DoClick))]
    class ReportButtonDoClickPatch
    {
        public static bool Prefix(ReportButton __instance) {
            if (__instance.isActiveAndEnabled && Deputy.handcuffedPlayers.Contains(CachedPlayer.LocalPlayer.PlayerId) && __instance.graphic.color == Palette.EnabledColor) Deputy.setHandcuffedKnows();
            return !Deputy.handcuffedKnows.ContainsKey(CachedPlayer.LocalPlayer.PlayerId);
        }
    }

    [HarmonyPatch(typeof(EmergencyMinigame), nameof(EmergencyMinigame.Update))]
    class EmergencyMinigameUpdatePatch
    {
        static void Postfix(EmergencyMinigame __instance) {
            var roleCanCallEmergency = true;
            var statusText = "";

            // Deactivate emergency button for Swapper
            if (Swapper.swapper != null && Swapper.swapper == CachedPlayer.LocalPlayer.PlayerControl && !Swapper.canCallEmergency) {
                roleCanCallEmergency = false;
                statusText = ModTranslation.GetString("Game-Swapper", 1);
            }
            // Potentially deactivate emergency button for Jester
            if (Jester.jester != null && Jester.jester == CachedPlayer.LocalPlayer.PlayerControl && !Jester.canCallEmergency) {
                roleCanCallEmergency = false;
                statusText = ModTranslation.GetString("Game-Jester", 1);
            }
            // Potentially deactivate emergency button for Lawyer/Prosecutor
            if (Lawyer.lawyer != null && Lawyer.lawyer == CachedPlayer.LocalPlayer.PlayerControl && !Lawyer.canCallEmergency) {
                roleCanCallEmergency = false;
                statusText = ModTranslation.GetString("Game-Lawer", 1);
                if (Lawyer.isProsecutor) statusText = ModTranslation.GetString("Game-Lawer", 2);
            }

            if (!roleCanCallEmergency) {
                __instance.StatusText.text = statusText;
                __instance.NumberText.text = string.Empty;
                __instance.ClosedLid.gameObject.SetActive(true);
                __instance.OpenLid.gameObject.SetActive(false);
                __instance.ButtonActive = false;
                return;
            }

            // Handle max number of meetings
            if (__instance.state == 1) {
                int localRemaining = CachedPlayer.LocalPlayer.PlayerControl.RemainingEmergencies;
                int teamRemaining = Mathf.Max(0, maxNumberOfMeetings - meetingsCount);
                int remaining = Mathf.Min(localRemaining, (Mayor.mayor != null && Mayor.mayor == CachedPlayer.LocalPlayer.PlayerControl) ? 1 : teamRemaining);
                __instance.NumberText.text = string.Format(ModTranslation.GetString("Game-General", 7), localRemaining, teamRemaining);
                __instance.ButtonActive = remaining > 0;
                __instance.ClosedLid.gameObject.SetActive(!__instance.ButtonActive);
                __instance.OpenLid.gameObject.SetActive(__instance.ButtonActive);
                return;
            }
        }
    }


    [HarmonyPatch(typeof(Console), nameof(Console.CanUse))]
    public static class ConsoleCanUsePatch
    {
        public static bool Prefix(ref float __result, Console __instance, [HarmonyArgument(0)] GameData.PlayerInfo pc, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse) {
            canUse = couldUse = false;

            // Task Vs Mode
            if (CustomOptionHolder.enabledTaskVsMode.getBool()) {
                float num = float.MaxValue;
                Vector2 truePosition = pc.Object.GetTruePosition();
                Vector3 position = __instance.transform.position;
                couldUse = (!pc.IsDead || (PlayerControl.GameOptions.GhostsDoTasks && !__instance.GhostsIgnored)) && pc.Object.CanMove && (!__instance.onlySameRoom || __instance.InRoom(truePosition)) && (!__instance.onlyFromBelow || truePosition.y < position.y) && __instance.FindTask(pc.Object);
                canUse = couldUse;
                if (canUse) {
                    num = Vector2.Distance(truePosition, __instance.transform.position);
                    canUse &= (num <= __instance.UsableDistance);
                    if (__instance.checkWalls) {
                        canUse &= !PhysicsHelpers.AnythingBetween(truePosition, position, Constants.ShadowMask, false);
                    }
                }
                __result = num;
                return false;
            }

            if (Swapper.swapper != null && Swapper.swapper == CachedPlayer.LocalPlayer.PlayerControl)
                return !__instance.TaskTypes.Any(x => x == TaskTypes.FixLights || x == TaskTypes.FixComms);
            if (Madmate.madmate != null && Madmate.madmate == CachedPlayer.LocalPlayer.PlayerControl && __instance.AllowImpostor)
                return !__instance.TaskTypes.Any(x => x == TaskTypes.FixLights || x == TaskTypes.FixComms);
            if (MadmateKiller.madmateKiller != null && MadmateKiller.madmateKiller == CachedPlayer.LocalPlayer.PlayerControl && MadmateKiller.madmateKiller.Data.RoleType == RoleTypes.Crewmate && __instance.AllowImpostor)
                return !__instance.TaskTypes.Any(x => (!MadmateKiller.canFixLightsTask && x == TaskTypes.FixLights) || (!MadmateKiller.canFixCommsTask && x == TaskTypes.FixComms));

            if (__instance.AllowImpostor) return true;
            if (!Helpers.hasFakeTasks(pc.Object)) return true;
            __result = float.MaxValue;
            return false;
        }
    }

    [HarmonyPatch(typeof(TuneRadioMinigame), nameof(TuneRadioMinigame.Begin))]
    class CommsMinigameBeginPatch
    {
        static void Postfix(TuneRadioMinigame __instance) {
            // Block Swapper from fixing comms. Still looking for a better way to do this, but deleting the task doesn't seem like a viable option since then the camera, admin table, ... work while comms are out
            if ((Swapper.swapper != null && Swapper.swapper == CachedPlayer.LocalPlayer.PlayerControl) ||
                (Madmate.madmate != null && Madmate.madmate == CachedPlayer.LocalPlayer.PlayerControl) ||
                (MadmateKiller.madmateKiller != null && MadmateKiller.madmateKiller == CachedPlayer.LocalPlayer.PlayerControl && MadmateKiller.madmateKiller.Data.RoleType == RoleTypes.Crewmate && !MadmateKiller.canFixCommsTask)) {
                __instance.Close();
            }
        }
    }

    [HarmonyPatch(typeof(SwitchMinigame), nameof(SwitchMinigame.Begin))]
    class LightsMinigameBeginPatch
    {
        static void Postfix(SwitchMinigame __instance) {
            // Block Swapper from fixing lights. One could also just delete the PlayerTask, but I wanted to do it the same way as with coms for now.
            if ((Swapper.swapper != null && Swapper.swapper == CachedPlayer.LocalPlayer.PlayerControl) ||
                (Madmate.madmate != null && Madmate.madmate == CachedPlayer.LocalPlayer.PlayerControl) ||
                (MadmateKiller.madmateKiller != null && MadmateKiller.madmateKiller == CachedPlayer.LocalPlayer.PlayerControl && MadmateKiller.madmateKiller.Data.RoleType == RoleTypes.Crewmate && !MadmateKiller.canFixLightsTask)) {
                __instance.Close();
            }
        }
    }

    [HarmonyPatch]
    class VitalsMinigamePatch
    {
        private static List<TMPro.TextMeshPro> hackerTexts = new List<TMPro.TextMeshPro>();

        [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Begin))]
        class VitalsMinigameStartPatch
        {
            static void Postfix(VitalsMinigame __instance) {
                if (Hacker.hacker != null && CachedPlayer.LocalPlayer.PlayerControl == Hacker.hacker) {
                    hackerTexts = new List<TMPro.TextMeshPro>();
                    foreach (VitalsPanel panel in __instance.vitals) {
                        TMPro.TextMeshPro text = UnityEngine.Object.Instantiate(__instance.SabText, panel.transform);
                        hackerTexts.Add(text);
                        UnityEngine.Object.DestroyImmediate(text.GetComponent<AlphaBlink>());
                        text.gameObject.SetActive(false);
                        text.transform.localScale = Vector3.one * 0.75f;
                        text.transform.localPosition = new Vector3(-0.75f, -0.23f, 0f);

                    }
                }

                //Fix Visor in Vitals
                foreach (VitalsPanel panel in __instance.vitals) {
                    if (panel.PlayerIcon != null && panel.PlayerIcon.cosmetics.skin != null) {
                        panel.PlayerIcon.cosmetics.skin.transform.position = new Vector3(0, 0, 0f);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Update))]
        class VitalsMinigameUpdatePatch
        {

            static void Postfix(VitalsMinigame __instance) {
                // Consume time to see vital if the player is alive
                if (!CachedPlayer.LocalPlayer.PlayerControl.Data.IsDead) {
                    if (CustomOptionHolder.enabledVitalsTimer.getBool()) {
                        if (MapOptions.vitalsTimer <= 0) {
                            __instance.SabText.gameObject.SetActive(true);
                            __instance.SabText.text = ModTranslation.GetString("Game-General", 8);
                            for (int k = 0; k < __instance.vitals.Length; k++) {
                                VitalsPanel vitalsPanel = __instance.vitals[k];
                                vitalsPanel.gameObject.SetActive(false);
                            }
                            return;
                        }

                        // Consume the time via RPC
                        float delta = Time.unscaledDeltaTime;
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                            CachedPlayer.LocalPlayer.PlayerControl.NetId,
                            (byte)CustomRPC.ConsumeVitalTime,
                            Hazel.SendOption.Reliable,
                            -1);
                        writer.Write(delta);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.consumeVitalTime(delta);
                    }
                }

                // Hacker show time since death

                if (Hacker.hacker != null && Hacker.hacker == CachedPlayer.LocalPlayer.PlayerControl && Hacker.hackerTimer > 0) {
                    for (int k = 0; k < __instance.vitals.Length; k++) {
                        VitalsPanel vitalsPanel = __instance.vitals[k];
                        GameData.PlayerInfo player = vitalsPanel.PlayerInfo;

                        // Hacker update
                        if (vitalsPanel.IsDead) {
                            DeadPlayer deadPlayer = deadPlayers?.Where(x => x.player?.PlayerId == player?.PlayerId)?.FirstOrDefault();
                            if (deadPlayer != null && deadPlayer.timeOfDeath != null && k < hackerTexts.Count && hackerTexts[k] != null) {
                                float timeSinceDeath = ((float)(DateTime.UtcNow - deadPlayer.timeOfDeath).TotalMilliseconds);
                                hackerTexts[k].gameObject.SetActive(true);
                                hackerTexts[k].text = string.Format(ModTranslation.GetString("Game-Hacker", 1), Math.Round(timeSinceDeath / 1000));
                            }
                        }
                    }
                } else {
                    foreach (TMPro.TextMeshPro text in hackerTexts)
                        if (text != null && text.gameObject != null)
                            text.gameObject.SetActive(false);
                }
            }
        }
    }

    [HarmonyPatch(typeof(SystemConsole), nameof(SystemConsole.CanUse))]
    class SystemConsoleCanUsePatch
    {
        public static bool Prefix(SystemConsole __instance,
            ref float __result,
            [HarmonyArgument(0)] GameData.PlayerInfo pc,
            [HarmonyArgument(1)] out bool canUse,
            [HarmonyArgument(2)] out bool couldUse) {
            canUse = couldUse = false;
            __result = float.MaxValue;

            // Task Vs Mode
            if (CustomOptionHolder.enabledTaskVsMode.getBool())
                return false;

            if (CachedPlayer.LocalPlayer.PlayerControl.Data.IsDead)
                return true;

            // Vitals
            if (CustomOptionHolder.enabledVitalsTimer.getBool() && __instance.gameObject.name.Contains("panel_vitals"))
                return MapOptions.vitalsTimer > 0;
            // Camera
            if (CustomOptionHolder.enabledSecurityCameraTimer.getBool() && (__instance.gameObject.name.Contains("task_cams") || __instance.gameObject.name.Contains("SurvConsole") || __instance.gameObject.name.Contains("Surv_Panel")))
                return MapOptions.securityCameraTimer > 0;

            return true;
        }
    }

    [HarmonyPatch]
    class AdminPanelPatch
    {
        static Dictionary<SystemTypes, List<Color>> players = new Dictionary<SystemTypes, System.Collections.Generic.List<Color>>();

        [HarmonyPatch(typeof(MapConsole), nameof(MapConsole.CanUse))]
        class MapConsoleCanUsePatch
        {
            public static bool Prefix(MapConsole __instance,
                ref float __result,
                [HarmonyArgument(0)] GameData.PlayerInfo pc,
                [HarmonyArgument(1)] out bool canUse,
                [HarmonyArgument(2)] out bool couldUse) {
                canUse = couldUse = false;
                __result = float.MaxValue;

                // Task Vs Mode
                if (CustomOptionHolder.enabledTaskVsMode.getBool())
                    return false;

                if (CachedPlayer.LocalPlayer.PlayerControl.Data.IsDead)
                    return true;

                if (CustomOptionHolder.enabledAdminTimer.getBool())
                    return MapOptions.adminTimer > 0;

                return true;
            }
        }

        [HarmonyPatch(typeof(MapCountOverlay), nameof(MapCountOverlay.Update))]
        class MapCountOverlayUpdatePatch
        {
            static bool Prefix(MapCountOverlay __instance) {
                // Save colors for the Hacker
                __instance.timer += Time.deltaTime;
                if (__instance.timer < 0.1f) {
                    return false;
                }

                // Consume time to see admin map if the player is alive
                if (!CachedPlayer.LocalPlayer.PlayerControl.Data.IsDead &&
                    !(EvilHacker.evilHacker != null && EvilHacker.evilHacker == CachedPlayer.LocalPlayer.PlayerControl && EvilHacker.isMobile) &&
                    CustomOptionHolder.enabledAdminTimer.getBool()) {
                    // Show the grey map if players ran out of admin time.
                    if (MapOptions.adminTimer <= 0) {
                        __instance.isSab = true;
                        __instance.BackgroundColor.SetColor(Palette.DisabledGrey);
                        return false;
                    }

                    // Consume the time via RPC
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                        CachedPlayer.LocalPlayer.PlayerControl.NetId,
                        (byte)CustomRPC.ConsumeAdminTime,
                        Hazel.SendOption.Reliable,
                        -1);
                    writer.Write(__instance.timer);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.consumeAdminTime(__instance.timer);
                }

                __instance.timer = 0f;
                players = new Dictionary<SystemTypes, List<Color>>();
                bool commsActive = false;
                    foreach (PlayerTask task in CachedPlayer.LocalPlayer.PlayerControl.myTasks.GetFastEnumerator())
                        if (task.TaskType == TaskTypes.FixComms) commsActive = true;       


                if (!__instance.isSab && commsActive) {
                    __instance.isSab = true;
                    __instance.BackgroundColor.SetColor(Palette.DisabledGrey);
                    __instance.SabotageText.gameObject.SetActive(true);
                    return false;
                }
                if (__instance.isSab && !commsActive) {
                    __instance.isSab = false;
                    __instance.BackgroundColor.SetColor(Color.green);
                    __instance.SabotageText.gameObject.SetActive(false);
                }

                for (int i = 0; i < __instance.CountAreas.Length; i++) {
                    CounterArea counterArea = __instance.CountAreas[i];
                    List<Color> roomColors = new List<Color>();
                    players.Add(counterArea.RoomType, roomColors);

                    if (!commsActive) {
                        PlainShipRoom plainShipRoom = MapUtilities.CachedShipStatus.FastRooms[counterArea.RoomType];

                        if (plainShipRoom != null && plainShipRoom.roomArea) {
                            int num = plainShipRoom.roomArea.OverlapCollider(__instance.filter, __instance.buffer);
                            int num2 = num;

                            // ロミジュリと絵画の部屋をアドミンの対象から外す
                            if (CustomOptionHolder.airshipChangeOldAdmin.getBool() && (counterArea.RoomType == SystemTypes.Ventilation || counterArea.RoomType == SystemTypes.HallOfPortraits))
                                num2 = 0;

                            for (int j = 0; j < num; j++) {
                                Collider2D collider2D = __instance.buffer[j];
                                if (!(collider2D.tag == "DeadBody")) {
                                    PlayerControl component = collider2D.GetComponent<PlayerControl>();
                                    if (!component || component.Data == null || component.Data.Disconnected || component.Data.IsDead) {
                                        num2--;
                                    } else if (component?.cosmetics?.currentBodySprite?.BodySprite?.material != null) {
                                        Color color = component.cosmetics.currentBodySprite.BodySprite.material.GetColor("_BodyColor");
                                        if (Hacker.onlyColorType) {
                                            var id = Mathf.Max(0, Palette.PlayerColors.IndexOf(color));
                                            color = Helpers.isLighterColor((byte)id) ? Palette.PlayerColors[7] : Palette.PlayerColors[6];
                                        }
                                        roomColors.Add(color);
                                    }
                                } else {
                                    DeadBody component = collider2D.GetComponent<DeadBody>();
                                    if (component) {
                                        GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);
                                        if (playerInfo != null) {
                                            var color = Palette.PlayerColors[playerInfo.DefaultOutfit.ColorId];
                                            if (Hacker.onlyColorType)
                                                color = Helpers.isLighterColor(playerInfo.DefaultOutfit.ColorId) ? Palette.PlayerColors[7] : Palette.PlayerColors[6];
                                            roomColors.Add(color);
                                        }
                                    }
                                }
                            }
                            if (num2 < 0) num2 = 0;
                            counterArea.UpdateCount(num2);
                        } else {
                            Debug.LogWarning("Couldn't find counter for:" + counterArea.RoomType);
                        }
                    } else {
                        counterArea.UpdateCount(0);
                    }
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(CounterArea), nameof(CounterArea.UpdateCount))]
        class CounterAreaUpdateCountPatch
        {
            private static Material defaultMat;
            private static Material newMat;
            static void Postfix(CounterArea __instance) {
                // Hacker display saved colors on the admin panel
                bool showHackerInfo = Hacker.hacker != null && Hacker.hacker == CachedPlayer.LocalPlayer.PlayerControl && Hacker.hackerTimer > 0;
                if (players.ContainsKey(__instance.RoomType)) {
                    List<Color> colors = players[__instance.RoomType];
                    int i = -1;
                    foreach (var icon in __instance.myIcons.GetFastEnumerator())
                    {
                        i += 1;
                        SpriteRenderer renderer = icon.GetComponent<SpriteRenderer>();

                        if (renderer != null) {
                            if (defaultMat == null) defaultMat = renderer.material;
                            if (newMat == null) newMat = UnityEngine.Object.Instantiate<Material>(defaultMat);
                            if (showHackerInfo && colors.Count > i) {
                                renderer.material = newMat;
                                var color = colors[i];
                                renderer.material.SetColor("_BodyColor", color);
                                var id = Palette.PlayerColors.IndexOf(color);
                                if (id < 0) {
                                    renderer.material.SetColor("_BackColor", color);
                                } else {
                                    renderer.material.SetColor("_BackColor", Palette.ShadowColors[id]);
                                }
                                renderer.material.SetColor("_VisorColor", Palette.VisorColor);
                            } else {
                                renderer.material = defaultMat;
                            }
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch]
    class SurveillanceMinigamePatch
    {
        private static int page = 0;
        private static float timer = 0f;
        [HarmonyPatch(typeof(SurveillanceMinigame), nameof(SurveillanceMinigame.Begin))]
        class SurveillanceMinigameBeginPatch
        {
            public static void Postfix(SurveillanceMinigame __instance) {
                // Add securityGuard cameras
                page = 0;
                timer = 0;
                if (MapUtilities.CachedShipStatus.AllCameras.Length > 4 && __instance.FilteredRooms.Length > 0) {
                    __instance.textures = __instance.textures.ToList().Concat(new RenderTexture[MapUtilities.CachedShipStatus.AllCameras.Length - 4]).ToArray();
                    for (int i = 4; i < MapUtilities.CachedShipStatus.AllCameras.Length; i++) {
                        SurvCamera surv = MapUtilities.CachedShipStatus.AllCameras[i];
                        Camera camera = UnityEngine.Object.Instantiate<Camera>(__instance.CameraPrefab);
                        camera.transform.SetParent(__instance.transform);
                        camera.transform.position = new Vector3(surv.transform.position.x, surv.transform.position.y, 8f);
                        camera.orthographicSize = 2.35f;
                        RenderTexture temporary = RenderTexture.GetTemporary(256, 256, 16, (RenderTextureFormat)0);
                        __instance.textures[i] = temporary;
                        camera.targetTexture = temporary;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(SurveillanceMinigame), nameof(SurveillanceMinigame.Update))]
        class SurveillanceMinigameUpdatePatch
        {

            public static bool Prefix(SurveillanceMinigame __instance) {
                // Consume time to see security camera if the player is alive
                if (!CachedPlayer.LocalPlayer.PlayerControl.Data.IsDead) {
                    if (CustomOptionHolder.enabledSecurityCameraTimer.getBool()) {
                        if (MapOptions.securityCameraTimer <= 0) {
                            for (int i = 0; i < __instance.SabText.Length; i++) {
                                __instance.SabText[i].text = ModTranslation.GetString("Game-General", 9);
                                __instance.SabText[i].gameObject.SetActive(true);
                            }
                            for (int i = 0; i < __instance.ViewPorts.Length; i++)
                                __instance.ViewPorts[i].sharedMaterial = __instance.StaticMaterial;
                            return false;
                        }

                        // Consume the time via RPC
                        float delta = Time.unscaledDeltaTime;
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                            CachedPlayer.LocalPlayer.PlayerControl.NetId,
                            (byte)CustomRPC.ConsumeSecurityCameraTime,
                            Hazel.SendOption.Reliable,
                            -1);
                        writer.Write(delta);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.consumeSecurityCameraTime(delta);
                    }
                }

                // Update normal and securityGuard cameras
                timer += Time.deltaTime;
                int numberOfPages = Mathf.CeilToInt(MapUtilities.CachedShipStatus.AllCameras.Length / 4f);

                bool update = false;

                if (timer > 3f || Input.GetKeyDown(KeyCode.RightArrow)) {
                    update = true;
                    timer = 0f;
                    page = (page + 1) % numberOfPages;
                } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    page = (page + numberOfPages - 1) % numberOfPages;
                    update = true;
                    timer = 0f;
                }

                if ((__instance.isStatic || update) && !PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(CachedPlayer.LocalPlayer.PlayerControl)) {
                    __instance.isStatic = false;
                    for (int i = 0; i < __instance.ViewPorts.Length; i++) {
                        __instance.ViewPorts[i].sharedMaterial = __instance.DefaultMaterial;
                        __instance.SabText[i].gameObject.SetActive(false);
                        if (page * 4 + i < __instance.textures.Length)
                            __instance.ViewPorts[i].material.SetTexture("_MainTex", __instance.textures[page * 4 + i]);
                        else
                            __instance.ViewPorts[i].sharedMaterial = __instance.StaticMaterial;
                    }
                } else if (!__instance.isStatic && PlayerTask.PlayerHasTaskOfType<HudOverrideTask>(CachedPlayer.LocalPlayer.PlayerControl)) {
                    __instance.isStatic = true;
                    for (int j = 0; j < __instance.ViewPorts.Length; j++) {
                        __instance.ViewPorts[j].sharedMaterial = __instance.StaticMaterial;
                        __instance.SabText[j].gameObject.SetActive(true);
                    }
                }
                return false;
            }
        }
    }

    [HarmonyPatch]
    class PlanetSurveillanceMinigamePatch
    {

        [HarmonyPatch(typeof(PlanetSurveillanceMinigame), nameof(PlanetSurveillanceMinigame.NextCamera))]
        class NextCamera
        {
            public static bool Prefix(PlanetSurveillanceMinigame __instance, [HarmonyArgument(0)] int direction) {
                if (!CachedPlayer.LocalPlayer.PlayerControl.Data.IsDead && CustomOptionHolder.enabledSecurityCameraTimer.getBool()) {
                    if (MapOptions.securityCameraTimer <= 0)
                        return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PlanetSurveillanceMinigame), nameof(PlanetSurveillanceMinigame.Update))]
        class Update
        {

            public static bool Prefix(PlanetSurveillanceMinigame __instance) {
#if false
                Func<PlayerTask, bool> p = (t) => { return t.TaskType == TaskTypes.FixComms; };
                bool commsActive = CachedPlayer.LocalPlayer.PlayerControl.myTasks.Find(p) != null;
#endif
                bool commsActive = PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(CachedPlayer.LocalPlayer.PlayerControl);
                if (commsActive) {
                    __instance.SabText.gameObject.SetActive(true);
                    __instance.ViewPort.sharedMaterial = __instance.StaticMaterial;
                }

                // Consume time to see security camera if the player is alive
                if (!CachedPlayer.LocalPlayer.PlayerControl.Data.IsDead) {
                    if (CustomOptionHolder.enabledSecurityCameraTimer.getBool()) {
                        if (!commsActive && MapOptions.securityCameraTimer <= 0) {
                            __instance.SabText.text = ModTranslation.GetString("Game-General", 9);
                            __instance.SabText.gameObject.SetActive(true);
                            __instance.ViewPort.sharedMaterial = __instance.StaticMaterial;
                            return false;
                        }
                        // Consume the time via RPC
                        float delta = Time.unscaledDeltaTime;
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                            CachedPlayer.LocalPlayer.PlayerControl.NetId,
                            (byte)CustomRPC.ConsumeSecurityCameraTime,
                            Hazel.SendOption.Reliable,
                            -1);
                        writer.Write(delta);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.consumeSecurityCameraTime(delta);
                    }
                }

                return false;
            }
        }
    }

    [HarmonyPatch(typeof(MedScanMinigame), nameof(MedScanMinigame.FixedUpdate))]
    class MedScanMinigameFixedUpdatePatch
    {
        static void Prefix(MedScanMinigame __instance) {
            if (MapOptions.allowParallelMedBayScans) {
                __instance.medscan.CurrentUser = CachedPlayer.LocalPlayer.PlayerId;
                __instance.medscan.UsersList.Clear();
            }
        }
    }

    [HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.ShowSabotageMap))]
    class ShowSabotageMapPatch
    {
        static bool Prefix(MapBehaviour __instance) {
            if (HideNSeek.isHideNSeekGM)
                return HideNSeek.canSabotage;

            // Task Vs Mode
            if (TaskRacer.isValid()) {
                if (__instance.IsOpen) {
                    __instance.Close();
                    return false;
                }
                if (!CachedPlayer.LocalPlayer.PlayerControl.CanMove)
                    return false;
                CachedPlayer.LocalPlayer.PlayerControl.SetPlayerMaterialColors(__instance.HerePoint);
                __instance.GenericShow();
                __instance.taskOverlay.Show();
                __instance.ColorControl.SetColor(new Color(0.05f, 0.2f, 1f, 1f));
                DestroyableSingleton<HudManager>.Instance.SetHudActive(false);
                return false;
            }
            return true;
        }

        static void Postfix(MapBehaviour __instance) {
            if (CustomOptionHolder.hideTaskOverlayOnSabMap.getBool())
                __instance.taskOverlay.Hide();
        }
    }

    [HarmonyPatch]
    public static class MapBehaviourPatch2
    {
        public static void ResetIcons() {
            if (kataomoiMark != null) {
                GameObject.Destroy(kataomoiMark.gameObject);
                kataomoiMark = null;
            }
        }

        [HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.GenericShow))]
        class GenericShowPatch
        {
            static void Postfix(MapBehaviour __instance) {
                if (Kataomoi.kataomoi == CachedPlayer.LocalPlayer.PlayerControl) {
                    if (kataomoiMark == null) {
                        kataomoiMark = UnityEngine.Object.Instantiate(__instance.HerePoint, __instance.HerePoint.transform.parent);
                        kataomoiMark.sprite = GetKataomoiMarkSprite();
                        kataomoiMark.transform.localScale = Vector3.one * 0.5f;
                        kataomoiMark.enabled = IsShowKataomoiMark();
                        kataomoiMark.color = Kataomoi.color;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.FixedUpdate))]
        class FixedUpdatePatch
        {
            static void Postfix(MapBehaviour __instance) {

                if (Kataomoi.kataomoi == CachedPlayer.LocalPlayer.PlayerControl) {
                    bool isShowKataomoiMark = IsShowKataomoiMark();
                    kataomoiMark.enabled = isShowKataomoiMark;
                    if (isShowKataomoiMark) {
                        Vector3 vector = Kataomoi.target.transform.position;
                        vector /= MapUtilities.CachedShipStatus.MapScale;
                        vector.x *= Mathf.Sign(MapUtilities.CachedShipStatus.transform.localScale.x);
                        vector.z = -1f;
                        kataomoiMark.transform.localPosition = vector;
                    }
                }
            }
        }

        static bool IsShowKataomoiMark() {
            return Kataomoi.kataomoi == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.PlayerControl.isDead() && Kataomoi.target != null && !Kataomoi.target.isDead() && Kataomoi.isSearch;
        }

        static SpriteRenderer kataomoiMark;
        static Sprite kataomoiMarkSprite;

        static Sprite GetKataomoiMarkSprite() {
            if (kataomoiMarkSprite) return kataomoiMarkSprite;
            kataomoiMarkSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.KataomoiMark.png", 115f);
            return kataomoiMarkSprite;
        }
    }

    [HarmonyPatch(typeof(ProgressTracker), nameof(ProgressTracker.FixedUpdate))]
    class ProgressTrackerFixedUpdatePatch
    {
        static void Postfix(ProgressTracker __instance) {
            // Task Vs Mode
            if (TaskRacer.isValid())
                __instance.gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(GameData), nameof(GameData.HandleDisconnect), new[] {typeof(PlayerControl), typeof(DisconnectReasons) })]
    class GameDataHandleDisconnectPatch
    {
        static void Postfix(GameData __instance, [HarmonyArgument(0)] PlayerControl player, [HarmonyArgument(1)] DisconnectReasons reason) {
            // Task Vs Mode
            if (TaskRacer.isValid()) {
                var taskRacer = TaskRacer.getTaskRacer(player.PlayerId);
                if (taskRacer != null)
                    TaskRacer.onDisconnect(taskRacer);
            }
        }
    }
}
