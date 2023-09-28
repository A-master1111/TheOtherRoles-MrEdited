using System.Collections.Generic;
using TheOtherRoles.Utilities;
using UnityEngine;

namespace TheOtherRoles{
    static class MapOptions {
        // Set values
        public static int maxNumberOfMeetings = 10;
        public static bool blockSkippingInEmergencyMeetings = false;
        public static bool noVoteIsSelfVote = false;
        public static bool hidePlayerNames = false;
        public static bool hideOutOfSightNametags = false;
        public static bool ghostsSeeRoles = true;
        public static bool ghostsSeeModifier = true;
        public static bool ghostsSeeTasks = true;
        public static bool ghostsSeeVotes = true;
        public static bool showRoleSummary = true;
        public static bool allowParallelMedBayScans = false;
        public static bool showLighterDarker = true;
        public static bool enableSoundEffects = true;
        public static bool enableHorseMode = false;
        public static bool shieldFirstKill = false;
        public static CustomGamemodes gameMode = CustomGamemodes.Classic;

        // Updating values
        public static int meetingsCount = 0;
        public static List<SurvCamera> camerasToAdd = new List<SurvCamera>();
        public static List<Vent> ventsToSeal = new List<Vent>();
        public static Dictionary<byte, PoolablePlayer> playerIcons = new Dictionary<byte, PoolablePlayer>();
        public static float adminTimer = 0f;
        public static float vitalsTimer = 0f;
        public static float securityCameraTimer = 0f;
        public static TMPro.TextMeshPro adminTimerText = null;
        public static string firstKillName;
        public static PlayerControl firstKillPlayer;
        public static TMPro.TextMeshPro vitalsTimerText = null;
        public static TMPro.TextMeshPro securityCameraTimerText = null;

        const float TimerUIBaseX = -3.5f;
        const float TimerUIMoveX = 2.5f;

        public static void clearAndReloadMapOptions() {
            reloadPluginOptions();

            meetingsCount = 0;
            camerasToAdd = new List<SurvCamera>();
            ventsToSeal = new List<Vent>();
            playerIcons = new Dictionary<byte, PoolablePlayer>(); ;

            adminTimer = CustomOptionHolder.adminTimer.getFloat();
            vitalsTimer = CustomOptionHolder.vitalsTimer.getFloat();
            securityCameraTimer = CustomOptionHolder.securityCameraTimer.getFloat();

            UpdateTimer();

            maxNumberOfMeetings = Mathf.RoundToInt(CustomOptionHolder.maxNumberOfMeetings.getSelection());
            blockSkippingInEmergencyMeetings = CustomOptionHolder.blockSkippingInEmergencyMeetings.getBool();
            noVoteIsSelfVote = CustomOptionHolder.noVoteIsSelfVote.getBool();
            hidePlayerNames = CustomOptionHolder.hidePlayerNames.getBool();
            hideOutOfSightNametags = CustomOptionHolder.hideOutOfSightNametags.getBool();
            allowParallelMedBayScans = CustomOptionHolder.allowParallelMedBayScans.getBool();
            shieldFirstKill = CustomOptionHolder.shieldFirstKill.getBool();
            firstKillPlayer = null;
        }

        public static void reloadPluginOptions() {
            ghostsSeeRoles = TheOtherRolesPlugin.GhostsSeeRoles.Value;
            ghostsSeeModifier = TheOtherRolesPlugin.GhostsSeeModifier.Value;
            ghostsSeeTasks = TheOtherRolesPlugin.GhostsSeeTasks.Value;
            ghostsSeeVotes = TheOtherRolesPlugin.GhostsSeeVotes.Value;
            showRoleSummary = TheOtherRolesPlugin.ShowRoleSummary.Value;
            showLighterDarker = TheOtherRolesPlugin.ShowLighterDarker.Value;
            enableSoundEffects = TheOtherRolesPlugin.EnableSoundEffects.Value;
            enableHorseMode = TheOtherRolesPlugin.EnableHorseMode.Value;
            Patches.ShouldAlwaysHorseAround.isHorseMode = TheOtherRolesPlugin.EnableHorseMode.Value;
        }

        public static void MeetingEndedUpdate()
        {
            UpdateTimer();
        }

        public static int UpdateAdminTimerText(int viewIndex)
        {
            if (!CustomOptionHolder.enabledAdminTimer.getBool() || !CustomOptionHolder.viewAdminTimer.getBool() || CustomOptionHolder.enabledTaskVsMode.getBool())
                return viewIndex;
            if (FastDestroyableSingleton<HudManager>.Instance == null)
                return viewIndex;
            adminTimerText = UnityEngine.Object.Instantiate(FastDestroyableSingleton<HudManager>.Instance.TaskText, FastDestroyableSingleton<HudManager>.Instance.transform);
            adminTimerText.color = CustomOptionHolder.AdminColor;
            adminTimerText.transform.localPosition = new Vector3(TimerUIBaseX + TimerUIMoveX * viewIndex, -4.0f, 0);
            if (adminTimer > 0)
                adminTimerText.text = string.Format(ModTranslation.GetString("Game-General", 1), adminTimer);
            else
                adminTimerText.text = ModTranslation.GetString("Game-General", 2);
            adminTimerText.gameObject.SetActive(true);

            return viewIndex + 1;
        }

        private static void ClearAdminTimerText()
        {
            if (adminTimerText == null)
                return;
            UnityEngine.Object.Destroy(adminTimerText);
            adminTimerText = null;
        }

        public static int UpdateVitalsTimerText(int viewIndex) {
            if (!CustomOptionHolder.enabledVitalsTimer.getBool() || !CustomOptionHolder.viewVitalsTimer.getBool() ||  CustomOptionHolder.enabledTaskVsMode.getBool())
                return viewIndex;
            if (FastDestroyableSingleton<HudManager>.Instance == null)
                return viewIndex;
            vitalsTimerText = UnityEngine.Object.Instantiate(FastDestroyableSingleton<HudManager>.Instance.TaskText, FastDestroyableSingleton<HudManager>.Instance.transform);
            vitalsTimerText.color = CustomOptionHolder.VitalColor;
            vitalsTimerText.transform.localPosition = new Vector3(TimerUIBaseX + TimerUIMoveX * viewIndex, -4.0f, 0);
            if (vitalsTimer > 0)
                vitalsTimerText.text = string.Format(ModTranslation.GetString("Game-General", 3), vitalsTimer);
            else
                vitalsTimerText.text = ModTranslation.GetString("Game-General", 4);
            vitalsTimerText.gameObject.SetActive(true);

            return viewIndex + 1;
        }

        private static void ClearVitalsTimerText() {
            if (vitalsTimerText == null)
                return;
            UnityEngine.Object.Destroy(vitalsTimerText);
            vitalsTimerText = null;
        }


        public static int UpdateSecurityCameraTimerText(int viewIndex) {
            if (!CustomOptionHolder.enabledSecurityCameraTimer.getBool() || !CustomOptionHolder.viewSecurityCameraTimer.getBool() ||  CustomOptionHolder.enabledTaskVsMode.getBool())
                return viewIndex;
            if (FastDestroyableSingleton<HudManager>.Instance == null)
                return viewIndex;
            securityCameraTimerText = UnityEngine.Object.Instantiate(FastDestroyableSingleton<HudManager>.Instance.TaskText, FastDestroyableSingleton<HudManager>.Instance.transform);
            securityCameraTimerText.color = CustomOptionHolder.SecurityCameraColor;
            securityCameraTimerText.transform.localPosition = new Vector3(TimerUIBaseX + TimerUIMoveX * viewIndex, -4.0f, 0);
            if (securityCameraTimer > 0)
                securityCameraTimerText.text = string.Format(ModTranslation.GetString("Game-General", 5), securityCameraTimer);
            else
                securityCameraTimerText.text = ModTranslation.GetString("Game-General", 6);
            securityCameraTimerText.gameObject.SetActive(true);

            return viewIndex + 1;
        }

        private static void ClearSecurityCameraTimerText() {
            if (securityCameraTimerText == null)
                return;
            UnityEngine.Object.Destroy(securityCameraTimerText);
            securityCameraTimerText = null;
        }


        private static void UpdateTimer() {

            int viewIndex = 0;
            ClearAdminTimerText();
            viewIndex = UpdateAdminTimerText(viewIndex);

            if (Helpers.existVitals()) {
                ClearVitalsTimerText();
                viewIndex = UpdateVitalsTimerText(viewIndex);
            }

            if (Helpers.existSecurityCamera()) {
                ClearSecurityCameraTimerText();
                viewIndex = UpdateSecurityCameraTimerText(viewIndex);
            }
        }
    }
}
