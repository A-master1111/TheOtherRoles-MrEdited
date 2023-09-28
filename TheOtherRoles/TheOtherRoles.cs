using System.Linq;
using System.Text;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using TheOtherRoles.Objects;
using TheOtherRoles.Players;
using TheOtherRoles.Utilities;
using Hazel;
using TheOtherRoles.CustomGameModes;
using static TheOtherRoles.TheOtherRoles;
using AmongUs.Data;

namespace TheOtherRoles
{
    [HarmonyPatch]
    public static class TheOtherRoles
    {
        public static System.Random rnd = new System.Random((int)DateTime.Now.Ticks);

        public static void clearAndReloadRoles() {
            Jester.clearAndReload();
            Mayor.clearAndReload();
            Portalmaker.clearAndReload();
            Engineer.clearAndReload();
            Sheriff.clearAndReload();
            Deputy.clearAndReload();
            Lighter.clearAndReload();
            Godfather.clearAndReload();
            Mafioso.clearAndReload();
            Janitor.clearAndReload();
            Detective.clearAndReload();
            TimeMaster.clearAndReload();
            Medic.clearAndReload();
            Shifter.clearAndReload();
            Swapper.clearAndReload();
            Lovers.clearAndReload();
            Seer.clearAndReload();
            Morphling.clearAndReload();
            Camouflager.clearAndReload();
            EvilHacker.clearAndReload();
            Hacker.clearAndReload();
            Tracker.clearAndReload();
            Vampire.clearAndReload();
            Snitch.clearAndReload();
            Jackal.clearAndReload();
            Sidekick.clearAndReload();
            Eraser.clearAndReload();
            Spy.clearAndReload();
            Trickster.clearAndReload();
            Cleaner.clearAndReload();
            Warlock.clearAndReload();
            SecurityGuard.clearAndReload();
            Arsonist.clearAndReload();
            BountyHunter.clearAndReload();
            Vulture.clearAndReload();
            Medium.clearAndReload();
            Madmate.clearAndReload();
            Lawyer.clearAndReload();
            Pursuer.clearAndReload();
            Witch.clearAndReload();
            Ninja.clearAndReload();
            Thief.clearAndReload();
            Trapper.clearAndReload();
            Yasuna.clearAndReload();
            YasunaJr.clearAndReload();
            DoorHacker.clearAndReload();
            Kataomoi.clearAndReload();
            KillerCreator.clearAndReload();
            MadmateKiller.clearAndReload();
            TaskMaster.clearAndReload();

            // Modifier
            Bait.clearAndReload();
            Bloody.clearAndReload();
            AntiTeleport.clearAndReload();
            Tiebreaker.clearAndReload();
            Sunglasses.clearAndReload();
            Mini.clearAndReload();
            Vip.clearAndReload();
            Invert.clearAndReload();
            Chameleon.clearAndReload();

            // Task Vs Mode
            TaskRacer.clearAndReload();
            
            // Gamemodes
            HandleGuesser.clearAndReload();
            HideNSeek.clearAndReload();
        }

        public static class Jester
        {
            public static PlayerControl jester;
            public static Color color = new Color32(236, 98, 165, byte.MaxValue);

            public static bool triggerJesterWin = false;
            public static bool canCallEmergency = true;
            public static bool hasImpostorVision = false;

            public static void clearAndReload() {
                jester = null;
                triggerJesterWin = false;
                canCallEmergency = CustomOptionHolder.jesterCanCallEmergency.getBool();
                hasImpostorVision = CustomOptionHolder.jesterHasImpostorVision.getBool();
            }
        }
        
        public static class Portalmaker {
            public static PlayerControl portalmaker;
            public static Color color = new Color32(69, 69, 169, byte.MaxValue);

            public static float cooldown;
            public static float usePortalCooldown;
            public static bool logOnlyHasColors;
            public static bool logShowsTime;

            private static Sprite placePortalButtonSprite;
            private static Sprite usePortalButtonSprite;
            private static Sprite logSprite;

            public static Sprite getPlacePortalButtonSprite() {
                if (placePortalButtonSprite) return placePortalButtonSprite;
                placePortalButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.PlacePortalButton.png", 115f);
                return placePortalButtonSprite;
            }

            public static Sprite getUsePortalButtonSprite() {
                if (usePortalButtonSprite) return usePortalButtonSprite;
                usePortalButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.UsePortalButton.png", 115f);
                return usePortalButtonSprite;
            }

            public static Sprite getLogSprite() {
                if (logSprite) return logSprite;
                logSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.DoorLogsButton].Image;
                return logSprite;
            }

            public static void clearAndReload() {
                portalmaker = null;
                cooldown = CustomOptionHolder.portalmakerCooldown.getFloat();
                usePortalCooldown = CustomOptionHolder.portalmakerUsePortalCooldown.getFloat();
                logOnlyHasColors = CustomOptionHolder.portalmakerLogOnlyColorType.getBool();
                logShowsTime = CustomOptionHolder.portalmakerLogHasTime.getBool();
            }


        }

        public static class Mayor {
            public static PlayerControl mayor;
            public static Color color = new Color32(32, 77, 66, byte.MaxValue);
            public static Minigame emergency = null;
            public static Sprite emergencySprite = null;
            public static int remoteMeetingsLeft = 1;

            public static bool canSeeVoteColors = false;
            public static int tasksNeededToSeeVoteColors;
            public static bool meetingButton = true;

            public static Sprite getMeetingSprite()
            {
                if (emergencySprite) return emergencySprite;
                emergencySprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.EmergencyButton.png", 550f);
                return emergencySprite;
            }

            public static void clearAndReload() {
                mayor = null;
                emergency = null;
                emergencySprite = null;
		        remoteMeetingsLeft = Mathf.RoundToInt(CustomOptionHolder.mayorMaxRemoteMeetings.getFloat()); 
                canSeeVoteColors = CustomOptionHolder.mayorCanSeeVoteColors.getBool();
                tasksNeededToSeeVoteColors = (int)CustomOptionHolder.mayorTasksNeededToSeeVoteColors.getFloat();
                meetingButton = CustomOptionHolder.mayorMeetingButton.getBool();
            }
        }

        public static class Engineer
        {
            public static PlayerControl engineer;
            public static Color color = new Color32(0, 40, 245, byte.MaxValue);
            private static Sprite buttonSprite;

            public static int remainingFixes = 1;
            public static bool highlightForImpostors = true;
            public static bool highlightForTeamJackal = true;

            public static Sprite getButtonSprite() {
                if (buttonSprite) return buttonSprite;
                buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.RepairButton.png", 115f);
                return buttonSprite;
            }

            public static void clearAndReload() {
                engineer = null;
                remainingFixes = Mathf.RoundToInt(CustomOptionHolder.engineerNumberOfFixes.getFloat());
                highlightForImpostors = CustomOptionHolder.engineerHighlightForImpostors.getBool();
                highlightForTeamJackal = CustomOptionHolder.engineerHighlightForTeamJackal.getBool();
            }
        }

        public static class Godfather
        {
            public static PlayerControl godfather;
            public static Color color = Palette.ImpostorRed;

            public static void clearAndReload() {
                godfather = null;
            }
        }

        public static class Mafioso
        {
            public static PlayerControl mafioso;
            public static Color color = Palette.ImpostorRed;

            public static void clearAndReload() {
                mafioso = null;
            }
        }


        public static class Janitor
        {
            public static PlayerControl janitor;
            public static Color color = Palette.ImpostorRed;

            public static float cooldown = 30f;

            private static Sprite buttonSprite;
            public static Sprite getButtonSprite() {
                if (buttonSprite) return buttonSprite;
                buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CleanButton.png", 115f);
                return buttonSprite;
            }

            public static void clearAndReload() {
                janitor = null;
                cooldown = CustomOptionHolder.janitorCooldown.getFloat();
            }
        }

        public static class Sheriff
        {
            public static PlayerControl sheriff;
            public static Color color = new Color32(248, 205, 70, byte.MaxValue);

            public static float cooldown = 30f;
            public static int remainingShots = 1;
            public static bool canKillNeutrals = false;
            public static bool spyCanDieToSheriff = false;
            public static bool madmateCanDieToSheriff = false;
            public static bool madmateKillerCanDieToSheriff = false;

            public static PlayerControl currentTarget;

            public static PlayerControl formerDeputy;  // Needed for keeping handcuffs + shifting
            public static PlayerControl formerSheriff;  // When deputy gets promoted...

            public static void replaceCurrentSheriff(PlayerControl deputy) {
                if (!formerSheriff) formerSheriff = sheriff;
                sheriff = deputy;
                currentTarget = null;
                cooldown = CustomOptionHolder.sheriffCooldown.getFloat();
            }

            public static void clearAndReload() {
                sheriff = null;
                currentTarget = null;
                formerDeputy = null;
                formerSheriff = null;
                cooldown = CustomOptionHolder.sheriffCooldown.getFloat();
                remainingShots = Mathf.RoundToInt(CustomOptionHolder.sheriffNumberOfShots.getFloat());
                canKillNeutrals = CustomOptionHolder.sheriffCanKillNeutrals.getBool();
                spyCanDieToSheriff = CustomOptionHolder.spyCanDieToSheriff.getBool();
                madmateKillerCanDieToSheriff = CustomOptionHolder.madmateKillerCanDieToSheriff.getBool();
                madmateCanDieToSheriff = CustomOptionHolder.madmateCanDieToSheriff.getBool();
                if (CustomOptionHolder.evilHackerSpawnRate.getSelection() > 0 &&
                    CustomOptionHolder.evilHackerCanCreateMadmate.getBool())
                    madmateCanDieToSheriff = CustomOptionHolder.createdMadmateCanDieToSheriff.getBool();
            }
        }

        public static class Deputy
        {
            public static PlayerControl deputy;
            public static Color color = Sheriff.color;

            public static PlayerControl currentTarget;
            public static List<byte> handcuffedPlayers = new List<byte>();
            public static int promotesToSheriff; // No: 0, Immediately: 1, After Meeting: 2
            public static bool keepsHandcuffsOnPromotion;
            public static float handcuffDuration;
            public static float remainingHandcuffs;
            public static float handcuffCooldown;
            public static bool knowsSheriff;
            public static Dictionary<byte, float> handcuffedKnows = new Dictionary<byte, float>();

            private static Sprite buttonSprite;
            private static Sprite handcuffedSprite;

            public static Sprite getButtonSprite() {
                if (buttonSprite) return buttonSprite;
                buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.DeputyHandcuffButton.png", 115f);
                return buttonSprite;
            }

            public static Sprite getHandcuffedButtonSprite() {
                if (handcuffedSprite) return handcuffedSprite;
                handcuffedSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.DeputyHandcuffed.png", 115f);
                return handcuffedSprite;
            }

            // Can be used to enable / disable the handcuff effect on the target's buttons
            public static void setHandcuffedKnows(bool active = true) {
                if (active) {
                    byte localPlayerId = CachedPlayer.LocalPlayer.PlayerId;
                    handcuffedKnows.Add(localPlayerId, handcuffDuration);
                    handcuffedPlayers.RemoveAll(x => x == localPlayerId);
                    SoundEffectsManager.play("deputyHandcuff");
                }

                HudManagerStartPatch.setAllButtonsHandcuffedStatus(active);
            }

            public static void clearAndReload() {
                deputy = null;
                currentTarget = null;
                handcuffedPlayers = new List<byte>();
                handcuffedKnows = new Dictionary<byte, float>();
                HudManagerStartPatch.setAllButtonsHandcuffedStatus(false, true);
                promotesToSheriff = CustomOptionHolder.deputyGetsPromoted.getSelection();
                remainingHandcuffs = CustomOptionHolder.deputyNumberOfHandcuffs.getFloat();
                handcuffCooldown = CustomOptionHolder.deputyHandcuffCooldown.getFloat();
                keepsHandcuffsOnPromotion = CustomOptionHolder.deputyKeepsHandcuffs.getBool();
                handcuffDuration = CustomOptionHolder.deputyHandcuffDuration.getFloat();
                knowsSheriff = CustomOptionHolder.deputyKnowsSheriff.getBool();
            }
        }

        public static class Lighter
        {
            public static PlayerControl lighter;
            public static Color color = new Color32(238, 229, 190, byte.MaxValue);

            public static float lighterModeLightsOnVision = 2f;
            public static float lighterModeLightsOffVision = 0.75f;

            public static float cooldown = 30f;
            public static float duration = 5f;

            public static float lighterTimer = 0f;

            private static Sprite buttonSprite;
            public static Sprite getButtonSprite() {
                if (buttonSprite) return buttonSprite;
                buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.LighterButton.png", 115f);
                return buttonSprite;
            }

            public static void clearAndReload() {
                lighter = null;
                lighterTimer = 0f;
                cooldown = CustomOptionHolder.lighterCooldown.getFloat();
                duration = CustomOptionHolder.lighterDuration.getFloat();
                lighterModeLightsOnVision = CustomOptionHolder.lighterModeLightsOnVision.getFloat();
                lighterModeLightsOffVision = CustomOptionHolder.lighterModeLightsOffVision.getFloat();
            }
        }

        public static class Detective
        {
            public static PlayerControl detective;
            public static Color color = new Color32(45, 106, 165, byte.MaxValue);

            public static float footprintIntervall = 1f;
            public static float footprintDuration = 1f;
            public static bool anonymousFootprints = false;
            public static float reportNameDuration = 0f;
            public static float reportColorDuration = 20f;
            public static float timer = 6.2f;

            public static void clearAndReload() {
                detective = null;
                anonymousFootprints = CustomOptionHolder.detectiveAnonymousFootprints.getBool();
                footprintIntervall = CustomOptionHolder.detectiveFootprintIntervall.getFloat();
                footprintDuration = CustomOptionHolder.detectiveFootprintDuration.getFloat();
                reportNameDuration = CustomOptionHolder.detectiveReportNameDuration.getFloat();
                reportColorDuration = CustomOptionHolder.detectiveReportColorDuration.getFloat();
                timer = 6.2f;
            }
        }
    }

    public static class TimeMaster
    {
        public static PlayerControl timeMaster;
        public static Color color = new Color32(112, 142, 239, byte.MaxValue);

        public static bool reviveDuringRewind = false;
        public static float rewindTime = 3f;
        public static float shieldDuration = 3f;
        public static float cooldown = 30f;

        public static bool shieldActive = false;
        public static bool isRewinding = false;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TimeShieldButton.png", 115f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            timeMaster = null;
            isRewinding = false;
            shieldActive = false;
            rewindTime = CustomOptionHolder.timeMasterRewindTime.getFloat();
            shieldDuration = CustomOptionHolder.timeMasterShieldDuration.getFloat();
            cooldown = CustomOptionHolder.timeMasterCooldown.getFloat();
        }
    }

    public static class Medic
    {
        public static PlayerControl medic;
        public static PlayerControl shielded;
        public static PlayerControl futureShielded;

        public static Color color = new Color32(126, 251, 194, byte.MaxValue);
        public static bool usedShield;

        public static int showShielded = 0;
        public static bool showAttemptToShielded = false;
        public static bool showAttemptToMedic = false;
        public static bool setShieldAfterMeeting = false;
        public static bool showShieldAfterMeeting = false;
        public static bool meetingAfterShielding = false;

        public static Color shieldedColor = new Color32(0, 221, 255, byte.MaxValue);
        public static PlayerControl currentTarget;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.ShieldButton.png", 115f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            medic = null;
            shielded = null;
            futureShielded = null;
            currentTarget = null;
            usedShield = false;
            showShielded = CustomOptionHolder.medicShowShielded.getSelection();
            showAttemptToShielded = CustomOptionHolder.medicShowAttemptToShielded.getBool();
            showAttemptToMedic = CustomOptionHolder.medicShowAttemptToMedic.getBool();
            setShieldAfterMeeting = CustomOptionHolder.medicSetOrShowShieldAfterMeeting.getSelection() == 2;
            showShieldAfterMeeting = CustomOptionHolder.medicSetOrShowShieldAfterMeeting.getSelection() == 1;
            meetingAfterShielding = false;
        }
    }

    public static class Swapper {
        public static PlayerControl swapper;
        public static Color color = new Color32(134, 55, 86, byte.MaxValue);
        private static Sprite spriteCheck;
        public static bool canCallEmergency = false;
        public static bool canOnlySwapOthers = false;
        public static int charges;
        public static float rechargeTasksNumber;
        public static float rechargedTasks;
 
        public static byte playerId1 = Byte.MaxValue;
        public static byte playerId2 = Byte.MaxValue;

        public static Sprite getCheckSprite() {
            if (spriteCheck) return spriteCheck;
            spriteCheck = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.SwapperCheck.png", 150f);
            return spriteCheck;
        }

        public static void clearAndReload() {
            swapper = null;
            playerId1 = Byte.MaxValue;
            playerId2 = Byte.MaxValue;
            canCallEmergency = CustomOptionHolder.swapperCanCallEmergency.getBool();
            canOnlySwapOthers = CustomOptionHolder.swapperCanOnlySwapOthers.getBool();
            charges = Mathf.RoundToInt(CustomOptionHolder.swapperSwapsNumber.getFloat());
            rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.swapperRechargeTasksNumber.getFloat());
            rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.swapperRechargeTasksNumber.getFloat());
        }
    }

    public static class Lovers
    {
        public static PlayerControl lover1;
        public static PlayerControl lover2;
        public static Color color = new Color32(232, 57, 185, byte.MaxValue);

        public static bool bothDie = true;
        public static bool enableChat = true;
        // Lovers save if next to be exiled is a lover, because RPC of ending game comes before RPC of exiled
        public static bool notAckedExiledIsLover = false;

        public static bool existing() {
            return lover1 != null && lover2 != null && !lover1.Data.Disconnected && !lover2.Data.Disconnected;
        }

        public static bool existingAndAlive() {
            return existing() && !lover1.Data.IsDead && !lover2.Data.IsDead && !notAckedExiledIsLover; // ADD NOT ACKED IS LOVER
        }

        public static bool existingWithKiller() {
            return existing() && (lover1 == Jackal.jackal || lover2 == Jackal.jackal
                               || lover1 == Sidekick.sidekick || lover2 == Sidekick.sidekick
                               || lover1.Data.Role.IsImpostor || lover2.Data.Role.IsImpostor);
        }

        public static bool hasAliveKillingLover(this PlayerControl player) {
            if (!Lovers.existingAndAlive() || !existingWithKiller())
                return false;
            return (player != null && (player == lover1 || player == lover2));
        }

        public static void clearAndReload() {
            lover1 = null;
            lover2 = null;
            notAckedExiledIsLover = false;
            bothDie = CustomOptionHolder.modifierLoverBothDie.getBool();
            enableChat = CustomOptionHolder.modifierLoverEnableChat.getBool();
        }

        public static PlayerControl getPartner(this PlayerControl player) {
            if (player == null)
                return null;
            if (lover1 == player)
                return lover2;
            if (lover2 == player)
                return lover1;
            return null;
        }
    }

    public static class Seer
    {
        public static PlayerControl seer;
        public static Color color = new Color32(97, 178, 108, byte.MaxValue);
        public static List<Vector3> deadBodyPositions = new List<Vector3>();

        public static float soulDuration = 15f;
        public static bool limitSoulDuration = false;
        public static int mode = 0;

        private static Sprite soulSprite;
        public static Sprite getSoulSprite() {
            if (soulSprite) return soulSprite;
            soulSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.Soul.png", 500f);
            return soulSprite;
        }

        public static void clearAndReload() {
            seer = null;
            deadBodyPositions = new List<Vector3>();
            limitSoulDuration = CustomOptionHolder.seerLimitSoulDuration.getBool();
            soulDuration = CustomOptionHolder.seerSoulDuration.getFloat();
            mode = CustomOptionHolder.seerMode.getSelection();
        }
    }

    public static class Morphling
    {
        public static PlayerControl morphling;
        public static Color color = Palette.ImpostorRed;
        private static Sprite sampleSprite;
        private static Sprite morphSprite;

        public static float cooldown = 30f;
        public static float duration = 10f;

        public static PlayerControl currentTarget;
        public static PlayerControl sampledTarget;
        public static PlayerControl morphTarget;
        public static float morphTimer = 0f;

        public static void resetMorph() {
            morphTarget = null;
            morphTimer = 0f;
            if (morphling == null) return;
            morphling.setDefaultLook();
        }

        public static void clearAndReload() {
            resetMorph();
            morphling = null;
            currentTarget = null;
            sampledTarget = null;
            morphTarget = null;
            morphTimer = 0f;
            cooldown = CustomOptionHolder.morphlingCooldown.getFloat();
            duration = CustomOptionHolder.morphlingDuration.getFloat();
        }

        public static Sprite getSampleSprite() {
            if (sampleSprite) return sampleSprite;
            sampleSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.SampleButton.png", 115f);
            return sampleSprite;
        }

        public static Sprite getMorphSprite() {
            if (morphSprite) return morphSprite;
            morphSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.MorphButton.png", 115f);
            return morphSprite;
        }
    }

    public static class Camouflager
    {
        public static PlayerControl camouflager;
        public static Color color = Palette.ImpostorRed;

        public static float cooldown = 30f;
        public static float duration = 10f;
        public static float camouflageTimer = 0f;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CamoButton.png", 115f);
            return buttonSprite;
        }

        public static void resetCamouflage() {
            camouflageTimer = 0f;
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
                p.setDefaultLook();
        }

        public static void clearAndReload() {
            resetCamouflage();
            camouflager = null;
            camouflageTimer = 0f;
            cooldown = CustomOptionHolder.camouflagerCooldown.getFloat();
            duration = CustomOptionHolder.camouflagerDuration.getFloat();
        }
    }

    public static class EvilHacker
    {
        public static PlayerControl evilHacker;
        public static Color color = Palette.ImpostorRed;

        public static bool canCreateMadmate = false;
        public static bool isMobile = false;
        public static PlayerControl currentTarget;

        private static Sprite buttonSprite;
        private static Sprite madmateButtonSprite;

        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
#if true
            byte mapId = PlayerControl.GameOptions.MapId;
            UseButtonSettings button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.PolusAdminButton]; // Polus
            if (mapId == 0 || mapId == 3) button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.AdminMapButton]; // Skeld || Dleks
            else if (mapId == 1) button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.MIRAAdminButton]; // Mira HQ
            else if (mapId == 4) button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.AirshipAdminButton]; // Airship
            buttonSprite = button.Image;
#else
            buttonSprite = DestroyableSingleton<TranslationController>.Instance.GetImage(ImageNames.AirshipAdminButton);
#endif
            return buttonSprite;
        }

        public static Sprite getMadmateButtonSprite() {
            if (madmateButtonSprite) return madmateButtonSprite;
            madmateButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.SidekickButton.png", 115f);
            return madmateButtonSprite;
        }

        public static void clearAndReload() {
            evilHacker = null;
            currentTarget = null;
            isMobile = false;
            canCreateMadmate = CustomOptionHolder.evilHackerCanCreateMadmate.getBool();
        }
    }

    public static class Hacker
    {
        public static PlayerControl hacker;
        public static Minigame vitals = null;
        public static Minigame doorLog = null;
        public static Color color = new Color32(117, 250, 76, byte.MaxValue);

        public static float cooldown = 30f;
        public static float duration = 10f;
        public static float toolsNumber = 5f;
        public static bool onlyColorType = false;
        public static float hackerTimer = 0f;
        public static int rechargeTasksNumber = 2;
        public static int rechargedTasks = 2;
        public static int chargesVitals = 1;
        public static int chargesAdminTable = 1;
        public static bool cantMove = true;

        private static Sprite buttonSprite;
        private static Sprite vitalsSprite;
        private static Sprite logSprite;
        private static Sprite adminSprite;

        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.HackerButton.png", 115f);
            return buttonSprite;
        }

        public static Sprite getVitalsSprite() {
            if (vitalsSprite) return vitalsSprite;
            vitalsSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.VitalsButton].Image;
            return vitalsSprite;
        }

        public static Sprite getLogSprite() {
            if (logSprite) return logSprite;
            logSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.DoorLogsButton].Image;
            return logSprite;
        }

        public static Sprite getAdminSprite() {
            byte mapId = PlayerControl.GameOptions.MapId;
            UseButtonSettings button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.PolusAdminButton]; // Polus
            if (mapId == 0 || mapId == 3) button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.AdminMapButton]; // Skeld || Dleks
            else if (mapId == 1) button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.MIRAAdminButton]; // Mira HQ
            else if (mapId == 4) button = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.AirshipAdminButton]; // Airship
            adminSprite = button.Image;
            return adminSprite;
        }

        public static void clearAndReload() {
            hacker = null;
            vitals = null;
            doorLog = null;
            hackerTimer = 0f;
            adminSprite = null;
            cooldown = CustomOptionHolder.hackerCooldown.getFloat();
            duration = CustomOptionHolder.hackerHackeringDuration.getFloat();
            onlyColorType = CustomOptionHolder.hackerOnlyColorType.getBool();
            toolsNumber = CustomOptionHolder.hackerToolsNumber.getFloat();
            rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.hackerRechargeTasksNumber.getFloat());
            rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.hackerRechargeTasksNumber.getFloat());
            chargesVitals = Mathf.RoundToInt(CustomOptionHolder.hackerToolsNumber.getFloat()) / 2;
            chargesAdminTable = Mathf.RoundToInt(CustomOptionHolder.hackerToolsNumber.getFloat()) / 2;
            cantMove = CustomOptionHolder.hackerNoMove.getBool();
        }
    }

    public static class Tracker
    {
        public static PlayerControl tracker;
        public static Color color = new Color32(100, 58, 220, byte.MaxValue);
        public static List<Arrow> localArrows = new List<Arrow>();

        public static float updateIntervall = 5f;
        public static bool resetTargetAfterMeeting = false;
        public static bool canTrackCorpses = false;
        public static float corpsesTrackingCooldown = 30f;
        public static float corpsesTrackingDuration = 5f;
        public static float corpsesTrackingTimer = 0f;
        public static List<Vector3> deadBodyPositions = new List<Vector3>();

        public static PlayerControl currentTarget;
        public static PlayerControl tracked;
        public static bool usedTracker = false;
        public static float timeUntilUpdate = 0f;
        public static Arrow arrow = new Arrow(Color.blue);

        private static Sprite trackCorpsesButtonSprite;
        public static Sprite getTrackCorpsesButtonSprite() {
            if (trackCorpsesButtonSprite) return trackCorpsesButtonSprite;
            trackCorpsesButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.PathfindButton.png", 115f);
            return trackCorpsesButtonSprite;
        }

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TrackerButton.png", 115f);
            return buttonSprite;
        }

        public static void resetTracked() {
            currentTarget = tracked = null;
            usedTracker = false;
            if (arrow?.arrow != null) UnityEngine.Object.Destroy(arrow.arrow);
            arrow = new Arrow(Color.blue);
            if (arrow.arrow != null) arrow.arrow.SetActive(false);
        }

        public static void clearAndReload() {
            tracker = null;
            resetTracked();
            timeUntilUpdate = 0f;
            updateIntervall = CustomOptionHolder.trackerUpdateIntervall.getFloat();
            resetTargetAfterMeeting = CustomOptionHolder.trackerResetTargetAfterMeeting.getBool();
            if (localArrows != null) {
                foreach (Arrow arrow in localArrows)
                    if (arrow?.arrow != null)
                        UnityEngine.Object.Destroy(arrow.arrow);
            }
            deadBodyPositions = new List<Vector3>();
            corpsesTrackingTimer = 0f;
            corpsesTrackingCooldown = CustomOptionHolder.trackerCorpsesTrackingCooldown.getFloat();
            corpsesTrackingDuration = CustomOptionHolder.trackerCorpsesTrackingDuration.getFloat();
            canTrackCorpses = CustomOptionHolder.trackerCanTrackCorpses.getBool();
        }
    }

    public static class Vampire
    {
        public static PlayerControl vampire;
        public static Color color = Palette.ImpostorRed;

        public static float delay = 10f;
        public static float cooldown = 30f;
        public static bool canKillNearGarlics = true;
        public static bool localPlacedGarlic = false;
        public static bool garlicsActive = true;

        public static PlayerControl currentTarget;
        public static PlayerControl bitten;
        public static bool targetNearGarlic = false;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.VampireButton.png", 115f);
            return buttonSprite;
        }

        private static Sprite garlicButtonSprite;
        public static Sprite getGarlicButtonSprite() {
            if (garlicButtonSprite) return garlicButtonSprite;
            garlicButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.GarlicButton.png", 115f);
            return garlicButtonSprite;
        }

        public static void clearAndReload() {
            vampire = null;
            bitten = null;
            targetNearGarlic = false;
            localPlacedGarlic = false;
            currentTarget = null;
            garlicsActive = CustomOptionHolder.vampireSpawnRate.getSelection() > 0;
            delay = CustomOptionHolder.vampireKillDelay.getFloat();
            cooldown = CustomOptionHolder.vampireCooldown.getFloat();
            canKillNearGarlics = CustomOptionHolder.vampireCanKillNearGarlics.getBool();
        }
    }

    public static class Snitch
    {
        public static PlayerControl snitch;
        public static Color color = new Color32(184, 251, 79, byte.MaxValue);

        public static List<Arrow> localArrows = new List<Arrow>();
        public static int taskCountForReveal = 1;
        public static bool includeTeamJackal = false;
        public static bool teamJackalUseDifferentArrowColor = true;


        public static void clearAndReload() {
            if (localArrows != null) {
                foreach (Arrow arrow in localArrows)
                    if (arrow?.arrow != null)
                        UnityEngine.Object.Destroy(arrow.arrow);
            }
            localArrows = new List<Arrow>();
            taskCountForReveal = Mathf.RoundToInt(CustomOptionHolder.snitchLeftTasksForReveal.getFloat());
            includeTeamJackal = CustomOptionHolder.snitchIncludeTeamJackal.getBool();
            teamJackalUseDifferentArrowColor = CustomOptionHolder.snitchTeamJackalUseDifferentArrowColor.getBool();
            snitch = null;
        }
    }

    public static class Jackal
    {
        public static PlayerControl jackal;
        public static Color color = new Color32(0, 180, 235, byte.MaxValue);
        public static PlayerControl fakeSidekick;
        public static PlayerControl currentTarget;
        public static List<PlayerControl> formerJackals = new List<PlayerControl>();

        public static float cooldown = 30f;
        public static float createSidekickCooldown = 30f;
        public static bool canUseVents = true;
        public static bool canCreateSidekick = true;
        public static Sprite buttonSprite;
        public static bool jackalPromotedFromSidekickCanCreateSidekick = true;
        public static bool canCreateSidekickFromImpostor = true;
        public static bool hasImpostorVision = false;
        public static bool wasTeamRed;
        public static bool wasImpostor;
        public static bool wasSpy;

        public static Sprite getSidekickButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.SidekickButton.png", 115f);
            return buttonSprite;
        }

        public static void removeCurrentJackal() {
            if (!formerJackals.Any(x => x.PlayerId == jackal.PlayerId)) formerJackals.Add(jackal);
            jackal = null;
            currentTarget = null;
            fakeSidekick = null;
            cooldown = CustomOptionHolder.jackalKillCooldown.getFloat();
            createSidekickCooldown = CustomOptionHolder.jackalCreateSidekickCooldown.getFloat();
        }

        public static void clearAndReload() {
            jackal = null;
            currentTarget = null;
            fakeSidekick = null;
            cooldown = CustomOptionHolder.jackalKillCooldown.getFloat();
            createSidekickCooldown = CustomOptionHolder.jackalCreateSidekickCooldown.getFloat();
            canUseVents = CustomOptionHolder.jackalCanUseVents.getBool();
            canCreateSidekick = CustomOptionHolder.jackalCanCreateSidekick.getBool();
            jackalPromotedFromSidekickCanCreateSidekick = CustomOptionHolder.jackalPromotedFromSidekickCanCreateSidekick.getBool();
            canCreateSidekickFromImpostor = CustomOptionHolder.jackalCanCreateSidekickFromImpostor.getBool();
            formerJackals.Clear();
            hasImpostorVision = CustomOptionHolder.jackalAndSidekickHaveImpostorVision.getBool();
            wasTeamRed = wasImpostor = wasSpy = false;
        }

    }

    public static class Sidekick
    {
        public static PlayerControl sidekick;
        public static Color color = new Color32(0, 180, 235, byte.MaxValue);

        public static PlayerControl currentTarget;

        public static bool wasTeamRed;
        public static bool wasImpostor;
        public static bool wasSpy;

        public static float cooldown = 30f;
        public static bool canUseVents = true;
        public static bool canKill = true;
        public static bool promotesToJackal = true;
        public static bool hasImpostorVision = false;

        public static void clearAndReload() {
            sidekick = null;
            currentTarget = null;
            cooldown = CustomOptionHolder.jackalKillCooldown.getFloat();
            canUseVents = CustomOptionHolder.sidekickCanUseVents.getBool();
            canKill = CustomOptionHolder.sidekickCanKill.getBool();
            promotesToJackal = CustomOptionHolder.sidekickPromotesToJackal.getBool();
            hasImpostorVision = CustomOptionHolder.jackalAndSidekickHaveImpostorVision.getBool();
            wasTeamRed = wasImpostor = wasSpy = false;
        }
    }

    public static class Eraser
    {
        public static PlayerControl eraser;
        public static Color color = Palette.ImpostorRed;

        public static List<byte> alreadyErased = new List<byte>();

        public static List<PlayerControl> futureErased = new List<PlayerControl>();
        public static PlayerControl currentTarget;
        public static float cooldown = 30f;
        public static bool canEraseAnyone = false;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.EraserButton.png", 115f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            eraser = null;
            futureErased = new List<PlayerControl>();
            currentTarget = null;
            cooldown = CustomOptionHolder.eraserCooldown.getFloat();
            canEraseAnyone = CustomOptionHolder.eraserCanEraseAnyone.getBool();
            alreadyErased = new List<byte>();
        }
    }

    public static class Spy
    {
        public static PlayerControl spy;
        public static Color color = Palette.ImpostorRed;

        public static bool impostorsCanKillAnyone = true;
        public static bool canEnterVents = false;
        public static bool hasImpostorVision = false;

        public static void clearAndReload() {
            spy = null;
            impostorsCanKillAnyone = CustomOptionHolder.spyImpostorsCanKillAnyone.getBool();
            canEnterVents = CustomOptionHolder.spyCanEnterVents.getBool();
            hasImpostorVision = CustomOptionHolder.spyHasImpostorVision.getBool();
        }
    }

    public static class Trickster
    {
        public static PlayerControl trickster;
        public static Color color = Palette.ImpostorRed;
        public static float placeBoxCooldown = 30f;
        public static float lightsOutCooldown = 30f;
        public static float lightsOutDuration = 10f;
        public static float lightsOutTimer = 0f;

        private static Sprite placeBoxButtonSprite;
        private static Sprite lightOutButtonSprite;
        private static Sprite tricksterVentButtonSprite;

        public static Sprite getPlaceBoxButtonSprite() {
            if (placeBoxButtonSprite) return placeBoxButtonSprite;
            placeBoxButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.PlaceJackInTheBoxButton.png", 115f);
            return placeBoxButtonSprite;
        }

        public static Sprite getLightsOutButtonSprite() {
            if (lightOutButtonSprite) return lightOutButtonSprite;
            lightOutButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.LightsOutButton.png", 115f);
            return lightOutButtonSprite;
        }

        public static Sprite getTricksterVentButtonSprite() {
            if (tricksterVentButtonSprite) return tricksterVentButtonSprite;
            tricksterVentButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TricksterVentButton.png", 115f);
            return tricksterVentButtonSprite;
        }

        public static void clearAndReload() {
            trickster = null;
            lightsOutTimer = 0f;
            placeBoxCooldown = CustomOptionHolder.tricksterPlaceBoxCooldown.getFloat();
            lightsOutCooldown = CustomOptionHolder.tricksterLightsOutCooldown.getFloat();
            lightsOutDuration = CustomOptionHolder.tricksterLightsOutDuration.getFloat();
            JackInTheBox.UpdateStates(); // if the role is erased, we might have to update the state of the created objects
        }

    }

    public static class Cleaner
    {
        public static PlayerControl cleaner;
        public static Color color = Palette.ImpostorRed;

        public static float cooldown = 30f;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CleanButton.png", 115f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            cleaner = null;
            cooldown = CustomOptionHolder.cleanerCooldown.getFloat();
        }
    }

    public static class Warlock
    {

        public static PlayerControl warlock;
        public static Color color = Palette.ImpostorRed;

        public static PlayerControl currentTarget;
        public static PlayerControl curseVictim;
        public static PlayerControl curseVictimTarget;

        public static float cooldown = 30f;
        public static float rootTime = 5f;

        private static Sprite curseButtonSprite;
        private static Sprite curseKillButtonSprite;

        public static Sprite getCurseButtonSprite() {
            if (curseButtonSprite) return curseButtonSprite;
            curseButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CurseButton.png", 115f);
            return curseButtonSprite;
        }

        public static Sprite getCurseKillButtonSprite() {
            if (curseKillButtonSprite) return curseKillButtonSprite;
            curseKillButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CurseKillButton.png", 115f);
            return curseKillButtonSprite;
        }

        public static void clearAndReload() {
            warlock = null;
            currentTarget = null;
            curseVictim = null;
            curseVictimTarget = null;
            cooldown = CustomOptionHolder.warlockCooldown.getFloat();
            rootTime = CustomOptionHolder.warlockRootTime.getFloat();
        }

        public static void resetCurse() {
            HudManagerStartPatch.warlockCurseButton.Timer = HudManagerStartPatch.warlockCurseButton.MaxTimer;
            HudManagerStartPatch.warlockCurseButton.Sprite = Warlock.getCurseButtonSprite();
            HudManagerStartPatch.warlockCurseButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            currentTarget = null;
            curseVictim = null;
            curseVictimTarget = null;
        }
    }

    public static class SecurityGuard
    {
        public static PlayerControl securityGuard;
        public static Color color = new Color32(195, 178, 95, byte.MaxValue);

        public static float cooldown = 30f;
        public static int remainingScrews = 7;
        public static int totalScrews = 7;
        public static int ventPrice = 1;
        public static int camPrice = 2;
        public static int placedCameras = 0;
        public static float duration = 10f;
        public static int maxCharges = 5;
        public static int rechargeTasksNumber = 3;
        public static int rechargedTasks = 3;
        public static int charges = 1;
        public static bool cantMove = true;
        public static Vent ventTarget = null;
        public static Minigame minigame = null;

        private static Sprite closeVentButtonSprite;
        public static Sprite getCloseVentButtonSprite() {
            if (closeVentButtonSprite) return closeVentButtonSprite;
            closeVentButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CloseVentButton.png", 115f);
            return closeVentButtonSprite;
        }

        private static Sprite placeCameraButtonSprite;
        public static Sprite getPlaceCameraButtonSprite() {
            if (placeCameraButtonSprite) return placeCameraButtonSprite;
            placeCameraButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.PlaceCameraButton.png", 115f);
            return placeCameraButtonSprite;
        }

        private static Sprite animatedVentSealedSprite;
        private static float lastPPU;
        public static Sprite getAnimatedVentSealedSprite() {
            float ppu = 185f;
            if (SubmergedCompatibility.IsSubmerged) ppu = 120f;
            if (lastPPU != ppu) {
                animatedVentSealedSprite = null;
                lastPPU = ppu;
            }
            if (animatedVentSealedSprite) return animatedVentSealedSprite;
            animatedVentSealedSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.AnimatedVentSealed.png", ppu);
            return animatedVentSealedSprite;
        }

        private static Sprite staticVentSealedSprite;
        public static Sprite getStaticVentSealedSprite() {
            if (staticVentSealedSprite) return staticVentSealedSprite;
            staticVentSealedSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.StaticVentSealed.png", 160f);
            return staticVentSealedSprite;
        }

        private static Sprite submergedCentralUpperVentSealedSprite;
        public static Sprite getSubmergedCentralUpperSealedSprite() {
            if (submergedCentralUpperVentSealedSprite) return submergedCentralUpperVentSealedSprite;
            submergedCentralUpperVentSealedSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CentralUpperBlocked.png", 145f);
            return submergedCentralUpperVentSealedSprite;
        }

        private static Sprite submergedCentralLowerVentSealedSprite;
        public static Sprite getSubmergedCentralLowerSealedSprite() {
            if (submergedCentralLowerVentSealedSprite) return submergedCentralLowerVentSealedSprite;
            submergedCentralLowerVentSealedSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CentralLowerBlocked.png", 145f);
            return submergedCentralLowerVentSealedSprite;
        }

        private static Sprite camSprite;
        public static Sprite getCamSprite() {
            if (camSprite) return camSprite;
            camSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.CamsButton].Image;
            return camSprite;
        }

        private static Sprite logSprite;
        public static Sprite getLogSprite() {
            if (logSprite) return logSprite;
            logSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.DoorLogsButton].Image;
            return logSprite;
        }

        public static void clearAndReload() {
            securityGuard = null;
            ventTarget = null;
            minigame = null;
            duration = CustomOptionHolder.securityGuardCamDuration.getFloat();
            maxCharges = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamMaxCharges.getFloat());
            rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamRechargeTasksNumber.getFloat());
            rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamRechargeTasksNumber.getFloat());
            charges = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamMaxCharges.getFloat()) / 2;
            placedCameras = 0;
            cooldown = CustomOptionHolder.securityGuardCooldown.getFloat();
            totalScrews = remainingScrews = Mathf.RoundToInt(CustomOptionHolder.securityGuardTotalScrews.getFloat());
            camPrice = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamPrice.getFloat());
            ventPrice = Mathf.RoundToInt(CustomOptionHolder.securityGuardVentPrice.getFloat());
            cantMove = CustomOptionHolder.securityGuardNoMove.getBool();
        }
    }

    public static class Arsonist
    {
        public static PlayerControl arsonist;
        public static Color color = new Color32(238, 112, 46, byte.MaxValue);

        public static float cooldown = 30f;
        public static float duration = 3f;
        public static bool triggerArsonistWin = false;

        public static PlayerControl currentTarget;
        public static PlayerControl douseTarget;
        public static List<PlayerControl> dousedPlayers = new List<PlayerControl>();

        private static Sprite douseSprite;
        public static Sprite getDouseSprite() {
            if (douseSprite) return douseSprite;
            douseSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.DouseButton.png", 115f);
            return douseSprite;
        }

        private static Sprite igniteSprite;
        public static Sprite getIgniteSprite() {
            if (igniteSprite) return igniteSprite;
            igniteSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.IgniteButton.png", 115f);
            return igniteSprite;
        }

        public static bool dousedEveryoneAlive() {
            return CachedPlayer.AllPlayers.All(x => { return x.PlayerControl == Arsonist.arsonist || x.Data.IsDead || x.Data.Disconnected || Arsonist.dousedPlayers.Any(y => y.PlayerId == x.PlayerId); });
        }

        public static void clearAndReload() {
            arsonist = null;
            currentTarget = null;
            douseTarget = null;
            triggerArsonistWin = false;
            dousedPlayers = new List<PlayerControl>();
            foreach (PoolablePlayer p in MapOptions.playerIcons.Values) {
                if (p != null && p.gameObject != null) p.gameObject.SetActive(false);
            }
            cooldown = CustomOptionHolder.arsonistCooldown.getFloat();
            duration = CustomOptionHolder.arsonistDuration.getFloat();
        }
    }

    public static class Guesser
    {
        public static PlayerControl niceGuesser;
        public static PlayerControl evilGuesser;
        public static Color color = new Color32(255, 255, 0, byte.MaxValue);

        public static int remainingShotsEvilGuesser = 2;
        public static int remainingShotsNiceGuesser = 2;

        public static bool isGuesser(byte playerId) {
            if ((niceGuesser != null && niceGuesser.PlayerId == playerId) || (evilGuesser != null && evilGuesser.PlayerId == playerId)) return true;
            return false;
        }

        public static void clear(byte playerId) {
            if (niceGuesser != null && niceGuesser.PlayerId == playerId) niceGuesser = null;
            else if (evilGuesser != null && evilGuesser.PlayerId == playerId) evilGuesser = null;
        }

        public static int remainingShots(byte playerId, bool shoot = false) {
            int remainingShots = remainingShotsEvilGuesser;
            if (niceGuesser != null && niceGuesser.PlayerId == playerId) {
                remainingShots = remainingShotsNiceGuesser;
                if (shoot) remainingShotsNiceGuesser = Mathf.Max(0, remainingShotsNiceGuesser - 1);
            } else if (shoot) {
                remainingShotsEvilGuesser = Mathf.Max(0, remainingShotsEvilGuesser - 1);
            }
            return remainingShots;
        }

        public static void clearAndReload() {
            niceGuesser = null;
            evilGuesser = null;
            remainingShotsEvilGuesser = Mathf.RoundToInt(CustomOptionHolder.guesserNumberOfShots.getFloat());
            remainingShotsNiceGuesser = Mathf.RoundToInt(CustomOptionHolder.guesserNumberOfShots.getFloat());
        }
    }

    public static class BountyHunter
    {
        public static PlayerControl bountyHunter;
        public static Color color = Palette.ImpostorRed;

        public static Arrow arrow;
        public static float bountyDuration = 30f;
        public static bool showArrow = true;
        public static float bountyKillCooldown = 0f;
        public static float punishmentTime = 15f;
        public static float arrowUpdateIntervall = 10f;

        public static float arrowUpdateTimer = 0f;
        public static float bountyUpdateTimer = 0f;
        public static PlayerControl bounty;
        public static TMPro.TextMeshPro cooldownText;

        public static void clearAndReload() {
            arrow = new Arrow(color);
            bountyHunter = null;
            bounty = null;
            arrowUpdateTimer = 0f;
            bountyUpdateTimer = 0f;
            if (arrow != null && arrow.arrow != null) UnityEngine.Object.Destroy(arrow.arrow);
            arrow = null;
            if (cooldownText != null && cooldownText.gameObject != null) UnityEngine.Object.Destroy(cooldownText.gameObject);
            cooldownText = null;
            foreach (PoolablePlayer p in MapOptions.playerIcons.Values) {
                if (p != null && p.gameObject != null) p.gameObject.SetActive(false);
            }


            bountyDuration = CustomOptionHolder.bountyHunterBountyDuration.getFloat();
            bountyKillCooldown = CustomOptionHolder.bountyHunterReducedCooldown.getFloat();
            punishmentTime = CustomOptionHolder.bountyHunterPunishmentTime.getFloat();
            showArrow = CustomOptionHolder.bountyHunterShowArrow.getBool();
            arrowUpdateIntervall = CustomOptionHolder.bountyHunterArrowUpdateIntervall.getFloat();
        }
    }

    public static class Vulture
    {
        public static PlayerControl vulture;
        public static Color color = new Color32(139, 69, 19, byte.MaxValue);
        public static List<Arrow> localArrows = new List<Arrow>();
        public static float cooldown = 30f;
        public static int vultureNumberToWin = 4;
        public static int eatenBodies = 0;
        public static bool triggerVultureWin = false;
        public static bool canUseVents = true;
        public static bool showArrows = true;
        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.VultureButton.png", 115f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            vulture = null;
            vultureNumberToWin = Mathf.RoundToInt(CustomOptionHolder.vultureNumberToWin.getFloat());
            eatenBodies = 0;
            cooldown = CustomOptionHolder.vultureCooldown.getFloat();
            triggerVultureWin = false;
            canUseVents = CustomOptionHolder.vultureCanUseVents.getBool();
            showArrows = CustomOptionHolder.vultureShowArrows.getBool();
            if (localArrows != null) {
                foreach (Arrow arrow in localArrows)
                    if (arrow?.arrow != null)
                        UnityEngine.Object.Destroy(arrow.arrow);
            }
            localArrows = new List<Arrow>();
        }
    }


    public static class Medium
    {
        public static PlayerControl medium;
        public static DeadPlayer target;
        public static DeadPlayer soulTarget;
        public static Color color = new Color32(98, 120, 115, byte.MaxValue);
        public static List<Tuple<DeadPlayer, Vector3>> deadBodies = new List<Tuple<DeadPlayer, Vector3>>();
        public static List<Tuple<DeadPlayer, Vector3>> featureDeadBodies = new List<Tuple<DeadPlayer, Vector3>>();
        public static List<SpriteRenderer> souls = new List<SpriteRenderer>();
        public static DateTime meetingStartTime = DateTime.UtcNow;

        public static float cooldown = 30f;
        public static float duration = 3f;
        public static bool oneTimeUse = false;

        private static Sprite soulSprite;
        public static Sprite getSoulSprite() {
            if (soulSprite) return soulSprite;
            soulSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.Soul.png", 500f);
            return soulSprite;
        }

        private static Sprite question;
        public static Sprite getQuestionSprite() {
            if (question) return question;
            question = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.MediumButton.png", 115f);
            return question;
        }

        public static void clearAndReload() {
            medium = null;
            target = null;
            soulTarget = null;
            deadBodies = new List<Tuple<DeadPlayer, Vector3>>();
            featureDeadBodies = new List<Tuple<DeadPlayer, Vector3>>();
            souls = new List<SpriteRenderer>();
            meetingStartTime = DateTime.UtcNow;
            cooldown = CustomOptionHolder.mediumCooldown.getFloat();
            duration = CustomOptionHolder.mediumDuration.getFloat();
            oneTimeUse = CustomOptionHolder.mediumOneTimeUse.getBool();
        }
    }

    public static class Madmate
    {
        public static PlayerControl madmate;
        public static Color color = Palette.ImpostorRed;

        public static bool canEnterVents = false;
        public static bool hasImpostorVision = false;
        public static bool noticeImpostors = false;
        public static bool exileCrewmate = false;

        public static void clearAndReload() {
            madmate = null;
            CustomOption opCanEnterVents = CustomOptionHolder.madmateCanEnterVents;
            CustomOption opHasImpostorVision = CustomOptionHolder.madmateHasImpostorVision;
            CustomOption opNoticeImpostors = CustomOptionHolder.madmateNoticeImpostors;
            CustomOption opExileCrewmate = CustomOptionHolder.madmateExileCrewmate;

            if (CustomOptionHolder.evilHackerSpawnRate.getSelection() > 0 &&
                CustomOptionHolder.evilHackerCanCreateMadmate.getBool()) {
                // Madmate should be configurable from EvilHacker options if EvilHacker can make a madmate
                opCanEnterVents = CustomOptionHolder.createdMadmateCanEnterVents;
                opHasImpostorVision = CustomOptionHolder.createdMadmateHasImpostorVision;
                opNoticeImpostors = CustomOptionHolder.createdMadmateNoticeImpostors;
                opExileCrewmate = CustomOptionHolder.createdMadmateExileCrewmate;
            }
            canEnterVents = opCanEnterVents.getBool();
            hasImpostorVision = opHasImpostorVision.getBool();
            noticeImpostors = opNoticeImpostors.getBool();
            exileCrewmate = opExileCrewmate.getBool();
        }
    }

    public static class Lawyer
    {
        public static PlayerControl lawyer;
        public static PlayerControl target;
        public static Color color = new Color32(134, 153, 25, byte.MaxValue);
        public static Sprite targetSprite;
        public static bool triggerProsecutorWin = false;
        public static bool isProsecutor = false;
        public static bool canCallEmergency = true;

        public static float vision = 1f;
        public static bool lawyerKnowsRole = false;
        public static bool targetCanBeJester = false;
        public static bool targetWasGuessed = false;

        public static Sprite getTargetSprite() {
            if (targetSprite) return targetSprite;
            targetSprite = Helpers.loadSpriteFromResources("", 150f);
            return targetSprite;
        }

        public static void clearAndReload(bool clearTarget = true) {
            lawyer = null;
            if (clearTarget) {
                target = null;
                targetWasGuessed = false;
            }
            isProsecutor = false;
            triggerProsecutorWin = false;
            vision = CustomOptionHolder.lawyerVision.getFloat();
            lawyerKnowsRole = CustomOptionHolder.lawyerKnowsRole.getBool();
            targetCanBeJester = CustomOptionHolder.lawyerTargetCanBeJester.getBool();
            canCallEmergency = CustomOptionHolder.jesterCanCallEmergency.getBool();
        }
    }

    public static class Pursuer
    {
        public static PlayerControl pursuer;
        public static PlayerControl target;
        public static Color color = Lawyer.color;
        public static List<PlayerControl> blankedList = new List<PlayerControl>();
        public static int blanks = 0;
        public static Sprite blank;
        public static bool notAckedExiled = false;

        public static float cooldown = 30f;
        public static int blanksNumber = 5;

        public static Sprite getTargetSprite() {
            if (blank) return blank;
            blank = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.PursuerButton.png", 115f);
            return blank;
        }

        public static void clearAndReload() {
            pursuer = null;
            target = null;
            blankedList = new List<PlayerControl>();
            blanks = 0;
            notAckedExiled = false;

            cooldown = CustomOptionHolder.pursuerCooldown.getFloat();
            blanksNumber = Mathf.RoundToInt(CustomOptionHolder.pursuerBlanksNumber.getFloat());
        }
    }

    public static class Witch
    {
        public static PlayerControl witch;
        public static Color color = Palette.ImpostorRed;

        public static List<PlayerControl> futureSpelled = new List<PlayerControl>();
        public static PlayerControl currentTarget;
        public static PlayerControl spellCastingTarget;
        public static float cooldown = 30f;
        public static float spellCastingDuration = 2f;
        public static float cooldownAddition = 10f;
        public static float currentCooldownAddition = 0f;
        public static bool canSpellAnyone = false;
        public static bool triggerBothCooldowns = true;
        public static bool witchVoteSavesTargets = true;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.SpellButton.png", 115f);
            return buttonSprite;
        }

        private static Sprite spelledOverlaySprite;
        public static Sprite getSpelledOverlaySprite() {
            if (spelledOverlaySprite) return spelledOverlaySprite;
            spelledOverlaySprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.SpellButtonMeeting.png", 225f);
            return spelledOverlaySprite;
        }


        public static void clearAndReload() {
            witch = null;
            futureSpelled = new List<PlayerControl>();
            currentTarget = spellCastingTarget = null;
            cooldown = CustomOptionHolder.witchCooldown.getFloat();
            cooldownAddition = CustomOptionHolder.witchAdditionalCooldown.getFloat();
            currentCooldownAddition = 0f;
            canSpellAnyone = CustomOptionHolder.witchCanSpellAnyone.getBool();
            spellCastingDuration = CustomOptionHolder.witchSpellCastingDuration.getFloat();
            triggerBothCooldowns = CustomOptionHolder.witchTriggerBothCooldowns.getBool();
            witchVoteSavesTargets = CustomOptionHolder.witchVoteSavesTargets.getBool();
        }
    }

    public static class Ninja {
        public static PlayerControl ninja;
        public static Color color = Palette.ImpostorRed;

        public static PlayerControl ninjaMarked;
        public static PlayerControl currentTarget;
        public static float cooldown = 30f;
        public static float traceTime = 1f;
        public static bool knowsTargetLocation = false;
        public static float invisibleDuration = 5f;

        public static float invisibleTimer = 0f;
        public static bool isInvisble = false;
        private static Sprite markButtonSprite;
        private static Sprite killButtonSprite;
        public static Arrow arrow = new Arrow(Color.black);
        public static Sprite getMarkButtonSprite() {
            if (markButtonSprite) return markButtonSprite;
            markButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.NinjaMarkButton.png", 115f);
            return markButtonSprite;
        }

        public static Sprite getKillButtonSprite() {
            if (killButtonSprite) return killButtonSprite;
            killButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.NinjaAssassinateButton.png", 115f);
            return killButtonSprite;
        }

        public static void clearAndReload() {
            ninja = null;
            currentTarget = ninjaMarked = null;
            cooldown = CustomOptionHolder.ninjaCooldown.getFloat();
            knowsTargetLocation = CustomOptionHolder.ninjaKnowsTargetLocation.getBool();
            traceTime = CustomOptionHolder.ninjaTraceTime.getFloat();
            invisibleDuration = CustomOptionHolder.ninjaInvisibleDuration.getFloat();
            invisibleTimer = 0f;
            isInvisble = false;
            if (arrow?.arrow != null) UnityEngine.Object.Destroy(arrow.arrow);
            arrow = new Arrow(Color.black);
            if (arrow.arrow != null) arrow.arrow.SetActive(false);
        }
    }

    public static class Thief
    {
        public static PlayerControl thief;
        public static Color color = new Color32(71, 99, 45, Byte.MaxValue);
        public static PlayerControl currentTarget;
        public static PlayerControl formerThief;

        public static float cooldown = 30f;

        public static bool suicideFlag = false;  // Used as a flag for suicide

        public static bool hasImpostorVision;
        public static bool canUseVents;
        public static bool canKillSheriff;

        public static void clearAndReload()
        {
            thief = null;
            suicideFlag = false;
            currentTarget = null;
            formerThief = null;
            hasImpostorVision = CustomOptionHolder.thiefHasImpVision.getBool();  // todo option and implementation
            cooldown = CustomOptionHolder.thiefCooldown.getFloat();
            canUseVents = CustomOptionHolder.thiefCanUseVents.getBool();
            canKillSheriff = CustomOptionHolder.thiefCanKillSheriff.getBool();
        }
    }

    public static class Trapper
    {
        public static PlayerControl trapper;
        public static Color color = new Color32(110, 57, 105, byte.MaxValue);

        public static float cooldown = 30f;
        public static int maxCharges = 5;
        public static int rechargeTasksNumber = 3;
        public static int rechargedTasks = 3;
        public static int charges = 1;
        public static int trapCountToReveal = 2;
        public static List<PlayerControl> playersOnMap = new List<PlayerControl>();
        public static bool anonymousMap = false;
        public static int infoType = 0; // 0 = Role, 1 = Good/Evil, 2 = Name
        public static float trapDuration = 5f;

        private static Sprite trapButtonSprite;

        public static Sprite getButtonSprite()
        {
            if (trapButtonSprite) return trapButtonSprite;
            trapButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.Trapper_Place_Button.png", 115f);
            return trapButtonSprite;
        }

        public static void clearAndReload()
        {
            trapper = null;
            cooldown = CustomOptionHolder.trapperCooldown.getFloat();
            maxCharges = Mathf.RoundToInt(CustomOptionHolder.trapperMaxCharges.getFloat());
            rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.trapperRechargeTasksNumber.getFloat());
            rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.trapperRechargeTasksNumber.getFloat());
            charges = Mathf.RoundToInt(CustomOptionHolder.trapperMaxCharges.getFloat()) / 2;
            trapCountToReveal = Mathf.RoundToInt(CustomOptionHolder.trapperTrapNeededTriggerToReveal.getFloat());
            playersOnMap = new List<PlayerControl>();
            anonymousMap = CustomOptionHolder.trapperAnonymousMap.getBool();
            infoType = CustomOptionHolder.trapperInfoType.getSelection();
            trapDuration = CustomOptionHolder.trapperTrapDuration.getFloat();
        }
    }

    public static class Yasuna
    {
        public static PlayerControl yasuna;
        public static Color color = new Color32(90, 255, 25, byte.MaxValue);
        public static byte specialVoteTargetPlayerId = byte.MaxValue;
        private static int _remainingSpecialVotes = 1;
        private static Sprite targetSprite;

        public static void clearAndReload() {
            yasuna = null;
            _remainingSpecialVotes = Mathf.RoundToInt(CustomOptionHolder.yasunaNumberOfSpecialVotes.getFloat());
            specialVoteTargetPlayerId = byte.MaxValue;
        }

        public static Sprite getTargetSprite(bool isImpostor) {
            if (targetSprite) return targetSprite;
            targetSprite = Helpers.loadSpriteFromResources(isImpostor ? "TheOtherRoles.Resources.EvilYasunaTargetIcon.png" : "TheOtherRoles.Resources.YasunaTargetIcon.png", 150f);
            return targetSprite;
        }

        public static int remainingSpecialVotes(bool isVote = false) {
            if (yasuna == null)
                return 0;

            if (isVote)
                _remainingSpecialVotes = Mathf.Max(0, _remainingSpecialVotes - 1);
            return _remainingSpecialVotes;
        }

        public static bool isYasuna(byte playerId) {
            return yasuna != null && yasuna.PlayerId == playerId;
        }
    }

    public static class YasunaJr
    {
        public static PlayerControl yasunaJr;
        public static Color color = new Color32(182, 255, 153, byte.MaxValue);
        public static byte specialVoteTargetPlayerId = byte.MaxValue;
        private static int _remainingSpecialVotes = 1;
        private static Sprite targetSprite;

        public static void clearAndReload()
        {
            yasunaJr = null;
            _remainingSpecialVotes = Mathf.RoundToInt(CustomOptionHolder.yasunaJrNumberOfSpecialVotes.getFloat());
            specialVoteTargetPlayerId = byte.MaxValue;
        }

        public static Sprite getTargetSprite(bool isImpostor)
        {
            if (targetSprite) return targetSprite;
            targetSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.YasunaTargetIcon.png", 150f);
            return targetSprite;
        }

        public static int remainingSpecialVotes(bool isVote = false)
        {
            if (yasunaJr == null)
                return 0;

            if (isVote)
                _remainingSpecialVotes = Mathf.Max(0, _remainingSpecialVotes - 1);
            return _remainingSpecialVotes;
        }

        public static bool isYasunaJr(byte playerId)
        {
            return yasunaJr != null && yasunaJr.PlayerId == playerId;
        }
    }

    public static class TaskMaster
    {
        public static PlayerControl taskMaster = null;
        public static bool becomeATaskMasterWhenCompleteAllTasks = false;
        public static Color color = new Color32(225, 86, 75, byte.MaxValue);
        public static bool isTaskComplete = false;
        public static byte clearExTasks = 0;
        public static byte allExTasks = 0;
        public static byte oldTaskMasterPlayerId = byte.MaxValue;
        public static bool triggerTaskMasterWin = false;

        public static void clearAndReload() {
            taskMaster = null;
            becomeATaskMasterWhenCompleteAllTasks = CustomOptionHolder.taskMasterBecomeATaskMasterWhenCompleteAllTasks.getBool();
            isTaskComplete = false;
            clearExTasks = 0;
            allExTasks = 0;
            oldTaskMasterPlayerId = byte.MaxValue;
            triggerTaskMasterWin = false;
        }

        public static bool isTaskMaster(byte playerId) {
            return taskMaster != null && taskMaster.PlayerId == playerId;
        }
    }

    public static class DoorHacker
    {
        public static PlayerControl doorHacker;
        public static Color color = Palette.ImpostorRed;

        public static float cooldown = 30f;
        public static float duration = 3f;
        public static float doorHackerTimer = 0f;
        public static int remainingUses = -1;

        private static Il2CppArrayBase<PlainDoor> doors = null;
        private static List<bool> enableDoors = null;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.DoorHackerButton.png", 115f);
            return buttonSprite;
        }

        public static void DisableDoors(int playerId) {
            if (doorHacker == null || doorHacker.PlayerId != playerId) return;
            if (remainingUses == 0) return;

            if (playerId == CachedPlayer.LocalPlayer.PlayerControl.PlayerId) {
                doors = GameObject.FindObjectsOfType<PlainDoor>();
                if (doors != null && doors.Count > 0) {
                    doorHackerTimer = duration;
                    enableDoors = new List<bool>();
                    for (int i = 0; i < doors.Count; ++i) {
                        enableDoors.Add(doors[i].myCollider.enabled);
                        doors[i].myCollider.enabled = false;
                    }
                }
            } else {
                doorHacker.Collider.isTrigger = true;
            }
        }

        public static void ResetDoors(bool consumeRemain = false) {
            if (doorHacker == null) return;
            if (consumeRemain && remainingUses != -1)
                --remainingUses;
            doorHackerTimer = 0f;
            doorHacker.Collider.isTrigger = false;
            if (doors == null) return;
            for (int i = 0; i < doors.Count; ++i)
                doors[i].myCollider.enabled = enableDoors[i];
            enableDoors.Clear();
            doors = null;
        }

        public static void clearAndReload() {
            cooldown = CustomOptionHolder.doorHackerCooldown.getFloat();
            duration = CustomOptionHolder.doorHackerDuration.getFloat();
            int num = Mathf.RoundToInt(CustomOptionHolder.doorHackerNumberOfUses.getFloat());
            remainingUses = num == 0 ? -1 : num;

            ResetDoors();
        }
    }

    [HarmonyPatch]
    public static class Kataomoi
    {
        public static PlayerControl kataomoi;
        public static Color color = Lovers.color;

        public static float stareCooldown = 30f;
        public static float stareDuration = 3f;
        public static int stareCount = 1;
        public static int stareCountMax = 1;
        public static float stalkingCooldown = 30f;
        public static float stalkingDuration = 5f;
        public static float stalkingFadeTime = 0.5f;
        public static float searchCooldown = 30f;
        public static float searchDuration = 5f;
        public static bool isSearch = false;
        public static float stalkingTimer = 0f;
        public static float stalkingEffectTimer = 0f;
        public static bool triggerKataomoiWin = false;
        public static PlayerControl target = null;
        public static PlayerControl currentTarget = null;
        public static TMPro.TextMeshPro stareText = null;
        public static SpriteRenderer[] gaugeRenderer = new SpriteRenderer[3];
        public static Arrow arrow;
        public static float gaugeTimer = 0.0f;
        public static float baseGauge = 0f;

        static bool _isStalking = false;

        static Sprite stareSprite;
        public static Sprite getStareSprite() {
            if (stareSprite) return stareSprite;
            stareSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.KataomoiStareButton.png", 115f);
            return stareSprite;
        }

        static Sprite loveSprite;
        public static Sprite getLoveSprite() {
            if (loveSprite) return loveSprite;
            loveSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.KataomoiLoveButton.png", 115f);
            return loveSprite;
        }

        static Sprite searchSprite;
        public static Sprite getSearchSprite() {
            if (searchSprite) return searchSprite;
            searchSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.KataomoiSearchButton.png", 115f);
            return searchSprite;
        }

        static Sprite stalkingSprite;
        public static Sprite getStalkingSprite() {
            if (stalkingSprite) return stalkingSprite;
            stalkingSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.KataomoiStalkingButton.png", 115f);
            return stalkingSprite;
        }

        static Sprite[] loveGaugeSprites = new Sprite[3];
        public static Sprite getLoveGaugeSprite(int index) {
            if (index < 0 || index >= loveGaugeSprites.Length) return null;
            if (loveGaugeSprites[index]) return loveGaugeSprites[index];

            int id = 0;
            switch (index) {
                case 0: id = 1; break;
                case 1: id = 2; break;
                case 2: id = 11; break;
            }
            loveGaugeSprites[index] = Helpers.loadSpriteFromResources(String.Format("TheOtherRoles.Resources.KataomoiGauge_{0:d2}.png", id), 115f);
            return loveGaugeSprites[index];
        }

        public static void doStare() {
            baseGauge = getLoveGauge();
            gaugeTimer = 1.0f;
            stareCount = Mathf.Max(0, stareCount - 1);

            if (gaugeRenderer[2] != null && stareCount == 0) {
                gaugeRenderer[2].color = color;
            }
            if (Constants.ShouldPlaySfx()) SoundManager.Instance.PlaySound(DestroyableSingleton<HudManager>.Instance.TaskCompleteSound, false, 0.8f);
        }

        public static void doStalking() {
            if (kataomoi == null) return;
            stalkingTimer = stalkingDuration;
            _isStalking = true;
        }

        public static void resetStalking() {
            if (kataomoi == null) return;
            _isStalking = false;
            setAlpha(1.0f);
        }

        public static bool isStalking(PlayerControl player) {
            if (player == null || player != kataomoi) return false;
            return _isStalking && stalkingTimer > 0;
        }

        public static bool isStalking() {
            return isStalking(kataomoi);
        }

        public static void doSearch() {
            if (kataomoi == null) return;
            isSearch = true;
        }

        public static void resetSearch() {
            if (kataomoi == null) return;
            isSearch = false;
        }

        public static bool canLove() {
            return stareCount <= 0;
        }

        public static float getLoveGauge() {
            return 1.0f - (stareCountMax == 0 ? 0f : (float)stareCount / (float)stareCountMax);
        }

        public static void clearAndReload() {
            resetStalking();

            kataomoi = null;
            stareCooldown = CustomOptionHolder.kataomoiStareCooldown.getFloat();
            stareDuration = CustomOptionHolder.kataomoiStareDuration.getFloat();
            stareCount = stareCountMax = (int)CustomOptionHolder.kataomoiStareCount.getFloat();
            stalkingCooldown = CustomOptionHolder.kataomoiStalkingCooldown.getFloat();
            stalkingDuration = CustomOptionHolder.kataomoiStalkingDuration.getFloat();
            stalkingFadeTime = CustomOptionHolder.kataomoiStalkingFadeTime.getFloat();
            searchCooldown = CustomOptionHolder.kataomoiSearchCooldown.getFloat();
            searchDuration = CustomOptionHolder.kataomoiSearchDuration.getFloat();
            isSearch = false;
            stalkingTimer = 0f;
            stalkingEffectTimer = 0f;
            triggerKataomoiWin = false;
            target = null;
            currentTarget = null;
            if (stareText != null && stareText.gameObject != null) UnityEngine.Object.Destroy(stareText.gameObject);
            stareText = null;
            if (arrow != null && arrow.arrow != null) UnityEngine.Object.Destroy(arrow.arrow);
            for (int i = 0; i < gaugeRenderer.Length; ++i) {
                if (gaugeRenderer[i] != null) {
                    UnityEngine.Object.Destroy(gaugeRenderer[i].gameObject);
                    gaugeRenderer[i] = null;
                }
            }
            arrow = null;
            gaugeTimer = 0.0f;
            baseGauge = 0.0f;
        }

        public static void fixedUpdate(PlayerPhysics __instance) {
            if (kataomoi == null) return;
            if (kataomoi != __instance.myPlayer) return;

            if (gaugeRenderer[1] != null && gaugeTimer > 0.0f)
            {
                gaugeTimer = Mathf.Max(gaugeTimer - Time.fixedDeltaTime, 0.0f);
                float gauge = getLoveGauge();
                float nowGauge = Mathf.Lerp(baseGauge, gauge, 1.0f - gaugeTimer);
                gaugeRenderer[1].transform.localPosition = new Vector3(Mathf.Lerp(-3.470784f - 1.121919f, -3.470784f, nowGauge), -2.626999f, -8.1f);
                gaugeRenderer[1].transform.localScale = new Vector3(nowGauge, 1, 1);
            }

            if (kataomoi.isDead()) return;
            if (_isStalking && stalkingTimer > 0)
            {
                kataomoi.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 0f);
                stalkingTimer = Mathf.Max(0f, stalkingTimer - Time.fixedDeltaTime);
                if (stalkingFadeTime > 0)
                {
                    float elapsedTime = stalkingDuration - stalkingTimer;
                    float alpha = Mathf.Min(elapsedTime, stalkingFadeTime) / stalkingFadeTime;
                    alpha = Mathf.Clamp(1f - alpha, CachedPlayer.LocalPlayer.PlayerControl == kataomoi || CachedPlayer.LocalPlayer.PlayerControl.isDead() ? 0.1f : 0f, 1f);
                    setAlpha(alpha);
                }
                else
                {
                    setAlpha(CachedPlayer.LocalPlayer.PlayerControl == kataomoi ? 0.1f : 0f);
                }

                if (stalkingTimer <= 0f)
                {
                    _isStalking = false;
                    stalkingEffectTimer = stalkingFadeTime;
                }
            }
            else if (!_isStalking && stalkingEffectTimer > 0)
            {
                stalkingEffectTimer = Mathf.Max(0f, stalkingEffectTimer - Time.fixedDeltaTime);
                if (stalkingFadeTime > 0)
                {
                    float elapsedTime = stalkingFadeTime - stalkingEffectTimer;
                    float alpha = Mathf.Min(elapsedTime, stalkingFadeTime) / stalkingFadeTime;
                    alpha = Mathf.Clamp(alpha, CachedPlayer.LocalPlayer.PlayerControl == kataomoi || CachedPlayer.LocalPlayer.PlayerControl.isDead() ? 0.1f : 0f, 1f);
                    setAlpha(alpha);
                }
                else
                {
                    setAlpha(1.0f);
                }
            }
            else
            {
                setAlpha(1.0f);
            }
        }

        static void setAlpha(float alpha) {
            if (kataomoi == null) return;
            var color = Color.Lerp(Palette.ClearWhite, Palette.White, alpha);
            try {
                if (kataomoi.cosmetics?.currentBodySprite?.BodySprite != null)
                    kataomoi.cosmetics.currentBodySprite.BodySprite.color = color;

                if (kataomoi.cosmetics?.skin?.layer != null)
                    kataomoi.cosmetics.skin.layer.color = color;

                if (kataomoi.cosmetics.hat != null)
                    kataomoi.cosmetics.hat.SetMaterialColor(color.ToInteger(true));

                if (kataomoi.cosmetics.currentPet?.rend != null)
                    kataomoi.cosmetics.currentPet.rend.color = color;

                if (kataomoi.cosmetics.currentPet?.shadowRend != null)
                    kataomoi.cosmetics.currentPet.shadowRend.color = color;

                if (kataomoi.cosmetics.visor != null)
                    kataomoi.cosmetics.visor.Alpha = alpha;
            } catch { }
        }
    }

    // Task Vs Mode
    [HarmonyPatch]
    public static class TaskRacer
    {
        public static Color color = new Color32(255, 116, 78, byte.MaxValue);
        public static Color firstRankColor = new Color32(214, 161, 64, byte.MaxValue);
        public static Color secondRankColor = new Color32(190, 193, 195, byte.MaxValue);
        public static Color thirdRankColor = new Color32(196, 112, 34, byte.MaxValue);
        public static Color defaultRankColor = new Color32(255, 255, 255, byte.MaxValue);

        public static TMPro.TextMeshPro startText = null;
        public static TMPro.TextMeshPro timerText = null;
        public static bool triggerTaskVsModeEnd = false;
        public static List<Vector3> rankUIPositions = new();
        public static Dictionary<byte, GameObject> taskFinishedMarkTable = new();

        public static List<Info> taskRacers = new List<Info>();
        public static SpriteRenderer[] rankGameSpriteRenderer = new SpriteRenderer[3];
        public static GameObject[] rankMarkObjects = new GameObject[3];
        public static float coolTime = 0;
        public static float effectDuration = 3.0f;
        public static float vision = 2.0f;
        public static ulong recordTime = ulong.MaxValue;

        static List<MoveInfo> moveInfoList = new();
        static float startTime = 0.0f;
        static float fadeTime = 0.0f;
        static bool _canMove = false;
        static int timeSecOld = -1;

        static Info self = null;
        static DateTime selfStartDateTime = default;
        static DateTime selfEndDateTime = default;
        static byte[] hostTaskTypeIds = null;
        static Dictionary<uint, byte[]> hostTaskIdDataTable = new();
        static Vector2 retireStartPos = Vector2.zero;

        public class Info
        {
            public PlayerControl player = null;
            public bool isReady = false;
            public int taskCount = 0;
            public int taskCompleteCount = -1;
            public ulong taskCompleteTime = ulong.MaxValue;

            public Info(PlayerControl player) {
                this.player = player;
                if (CachedPlayer.LocalPlayer.PlayerControl == player)
                    self = this;
            }

            public void updateTaskInfo() {
                taskCount = player.Data.Tasks.Count;
                taskCompleteCount = 0;
                for (int i = 0; i < taskCount; ++i) {
                    if (player.Data.Tasks[i].Complete)
                        ++taskCompleteCount;
                }
            }
            public bool isTaskComplete() {
                return player.isDead() || player.isDummy || player.notRealPlayer || (taskCompleteCount != -1 && taskCount == taskCompleteCount);
            }

            public bool isSelf()
            {
                return self == this;
            }
        }

        public class MoveInfo
        {
            public Transform moveObj = null;
            Vector3 pos = default;
            Vector3 movePos = default;
            float swapTime = 0.0f;
            float timer = 0.0f;

            public MoveInfo(Transform moveObj, Vector3 movePos, float swapTime) {
                this.moveObj = moveObj;
                this.pos = moveObj.localPosition;
                this.movePos = movePos;
                this.swapTime = this.timer = swapTime;
            }

            public bool update() {
                if (timer <= 0.0f) return false;
                timer = Mathf.Max(timer - Time.unscaledDeltaTime, 0.0f);
                moveObj.localPosition = Vector3.Lerp(pos, movePos, 1.0f - (timer / swapTime));
                return timer > 0.0f;
            }
        }

        public static Color getRankTextColor(int rank) {
            Color color = Palette.White;
            switch (rank) {
                case 1:
                    color = firstRankColor;
                    break;
                case 2:
                    color = secondRankColor;
                    break;
                case 3:
                    color = thirdRankColor;
                    break;
                default:
                    color = defaultRankColor;
                    break;
            }
            return color;
        }

        public static string getRankText(int rank, bool is_deco = false) {
            string rankText = "";
            switch (rank) {
                case 1:
                case 2:
                case 3:
                    rankText = ModTranslation.GetString("TaskVsMode", rank);
                    break;
                default:
                    rankText = string.Format(ModTranslation.GetString("TaskVsMode", 4), rank);
                    break;
            }
            return is_deco ? Helpers.cs(getRankTextColor(rank), rankText) : rankText;
        }

        public static int getRank(PlayerControl p) {
            for (int i = 0; i < taskRacers.Count; ++i) {
                if (taskRacers[i].player == p)
                    return i + 1;
            }
            return -1;
        }

        public static void startGame() {
            recordTime = TaskVsMode.GetRecordTime();
            startTime = 3.0f;
            _canMove = false;
            timerText.gameObject.SetActive(true);
            sortTaskRacers();
        }

        public static void setTaskCompleteTimeSec(byte playerId, ulong timeMilliSec) {
            var taskRacer = getTaskRacer(playerId);
            if (taskRacer == null) return;

            taskRacer.taskCompleteTime = timeMilliSec;
        }

        public static void updateTask(PlayerControl pc) {
            var taskRacer = getTaskRacer(pc.PlayerId);
            if (taskRacer == null) return;
            taskRacer.updateTaskInfo();

            // All task completed
            if (self.player == pc && self.isTaskComplete()) {
                selfEndDateTime = DateTime.Now;
                ulong completeTime = (ulong)(selfEndDateTime - selfStartDateTime).TotalMilliseconds;
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                    CachedPlayer.LocalPlayer.PlayerControl.NetId,
                    (byte)CustomRPC.TaskVsMode_AllTaskCompleted,
                    Hazel.SendOption.Reliable,
                    -1);
                writer.Write(self.player.PlayerId);
                writer.Write(completeTime);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.taskVsModeAllTaskCompleted(self.player.PlayerId, completeTime);

                // Set record time
                if (self.taskCompleteTime < recordTime)
                    TaskVsMode.SetRecordTime(self.taskCompleteTime);
            }

            updateControl();
        }

        public static void updateControl() {
            updateVisible();
            sortTaskRacers();

            bool isEnd = true;
            foreach (var p in taskRacers) {
                if (!p.isTaskComplete()) {
                    isEnd = false;
                    break;
                }
            }
            if (isEnd) {
                triggerTaskVsModeEnd = true;
            }
        }

        public static void update() {
            if (!isValid()) return;

            if (hostTaskTypeIds != null && CustomOptionHolder.taskVsMode_EnabledMakeItTheSameTaskAsTheHost.getBool()) {
                applyHostTasks();
            }

            // Start Direction
            if (startTime > 0.0f) {
                startTime = Mathf.Max(0, startTime - Time.unscaledDeltaTime);
                int t = (int)Math.Floor(startTime) + 1;
                if (timeSecOld != t) {
                    timeSecOld = t;
                    if (Modules.ModUpdateBehaviour.selectSfx != null)
                        SoundManager.Instance.PlaySound(Modules.ModUpdateBehaviour.selectSfx, false, 1.0f);
                }
                startText.text = t.ToString();
                float scale = Mathf.Lerp(1.0f, 1.5f, 1.0f - (startTime % 1.0f));
                startText.transform.localScale = new Vector3(scale, scale, scale);
                if (startTime == 0.0f) {
                    _canMove = true;
                    fadeTime = 1.5f;
                    SoundManager.Instance.PlaySound(DestroyableSingleton<HudManager>.Instance.TaskCompleteSound, false, 0.8f);
                    selfStartDateTime = DateTime.Now;
                }
            } else if (fadeTime > 0.0f) {
                startText.text = "GO!!";
                fadeTime = Mathf.Max(0, fadeTime - Time.unscaledDeltaTime);
                float scale = Mathf.Lerp(1.0f, 1.5f, 1 - (fadeTime / 1.5f));
                startText.transform.localScale = new Vector3(scale, scale, scale);
                startText.color = new Color(color.r, color.g, color.b, fadeTime / 1.5f);
            }

            // Timer
            //if (_canMove)
            {
                var builder = new StringBuilder();

                if (self.taskCompleteTime != ulong.MaxValue)
                {
                    builder.AppendFormat(Helpers.cs(color, "TIME<pos=0.7%>{0:D2}:{1:D2}:{2:D3}"), self.taskCompleteTime / 60000, (self.taskCompleteTime / 1000) % 60, self.taskCompleteTime % 1000);
                }
                else
                {
                    if (startTime > 0.0f)
                    {
                        builder.AppendFormat(Helpers.cs(color, "TIME<pos=0.7%>00:00:000"), self.taskCompleteTime / 60000, (self.taskCompleteTime / 1000) % 60, self.taskCompleteTime % 1000);
                    }
                    else if (self.player.isDead())
                    {
                        builder.AppendFormat(Helpers.cs(Color.gray, "TIME<pos=0.7%>xx:xx:xxx"));
                    }
                    else
                    {
                        var timeSpan = (self.isTaskComplete() ? selfEndDateTime : DateTime.Now) - selfStartDateTime;
                        builder.AppendFormat(Helpers.cs(Color.white, "TIME<pos=0.7%>{0:D2}:{1:D2}:{2:D3}"), (int)timeSpan.TotalMinutes, (int)timeSpan.TotalSeconds % 60, (int)timeSpan.TotalMilliseconds % 1000);
                    }
                }

                ulong recordTime_ = recordTime;
                bool isNewRecord = false;
                if (self.taskCompleteTime != ulong.MaxValue && self.taskCompleteTime < recordTime_)
                {
                    recordTime_ = self.taskCompleteTime;
                    isNewRecord = true;
                }

                if (recordTime_ != ulong.MaxValue)
                    builder.AppendFormat(Helpers.cs(Color.green, "\nRECORD TIME<pos=0.7%>{0:D2}:{1:D2}:{2:D3}"), recordTime_ / 60000, (recordTime_ / 1000) % 60, recordTime_ % 1000);
                else
                    builder.AppendFormat(Helpers.cs(Color.gray, "\nRECORD TIME<pos=0.7%>xx:xx:xxx"));

                if (isNewRecord)
                    builder.AppendFormat("\n{0}", Helpers.cs(Color.yellow, "*NEW RECORD*"));

                timerText.text = builder.ToString();
            }

            // Move
            for (int i = moveInfoList.Count - 1; i >= 0; --i) {
                if (!moveInfoList[i].update())
                    moveInfoList.RemoveAt(i);
            }
        }

        public static void onRetireStart(Vector2 pos) {
            retireStartPos = pos;
        }

        public static bool isRetireCancel(Vector2 pos) {
            return retireStartPos != pos;
        }

        public static bool isValid() {
            return taskRacers.Count > 0;
        }

        public static bool isTaskRacer(PlayerControl p) {
            return getTaskRacer(p.PlayerId) != null;
        }

        public static bool isTaskRacer(byte playerId) {
            return getTaskRacer(playerId) != null;
        }

        public static Info getTaskRacer(byte playerId) {
            for (int i = 0; i < taskRacers.Count; ++i) {
                if (taskRacers[i].player.PlayerId == playerId)
                    return taskRacers[i];
            }
            return null;
        }

        public static bool canMove() {
            return _canMove;
        }

        public static bool isReadyAll() {
            return getTaskRacerReadyCount() == getTaskRacerMaxCount();
        }

        public static int getTaskRacerReadyCount() {
            int count = 0;
            for (int i = 0; i < taskRacers.Count; ++i) {
                if (taskRacers[i].isReady || taskRacers[i].player.notRealPlayer)
                    ++count;
            }
            return count;
        }

        public static int getTaskRacerMaxCount() {
            return taskRacers.Count;
        }

        public static void addTaskRacer(PlayerControl p) {
            if (getTaskRacer(p.PlayerId) != null) return;
            taskRacers.Add(new Info(p));
        }

        public static void clearAndReload() {
            taskRacers.Clear();
            moveInfoList.Clear();
            rankUIPositions.Clear();
            if (startText != null && startText.gameObject != null) UnityEngine.Object.Destroy(startText.gameObject);
            startText = null;
            if (timerText != null && timerText.gameObject != null) UnityEngine.Object.Destroy(timerText.gameObject);
            timerText = null;
            startTime = 0.0f;
            fadeTime = 0.0f;
            recordTime = ulong.MaxValue;
            timeSecOld = -1;
            _canMove = false;
            triggerTaskVsModeEnd = false;
            self = null;
            selfStartDateTime = default;
            selfEndDateTime = default;
            hostTaskTypeIds = null;
            hostTaskIdDataTable.Clear();
            for (int i = 0; i < rankGameSpriteRenderer.Length; ++i) {
                if (rankGameSpriteRenderer[i] != null && rankGameSpriteRenderer[i].gameObject != null)
                    UnityEngine.Object.Destroy(rankGameSpriteRenderer[i].gameObject);
            }
            for (int i = 0; i < rankMarkObjects.Length; ++i) {
                if (rankMarkObjects[i] != null && rankMarkObjects[i].gameObject != null)
                    UnityEngine.Object.Destroy(rankMarkObjects[i].gameObject);
            }
            foreach (var mark in taskFinishedMarkTable) {
                if (mark.Value != null)
                    UnityEngine.Object.Destroy(mark.Value);
            }
            taskFinishedMarkTable.Clear();
            vision = CustomOptionHolder.taskVsMode_Vision.getFloat();
        }

        public static void onReady(Info taskRacer) {
            if (taskRacer == null) return;
            taskRacer.isReady = true;
            if (startText != null)
                startText.text = string.Format(ModTranslation.GetString("TaskVsMode", 5), getTaskRacerReadyCount(), getTaskRacerMaxCount());

            if (AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost && isReadyAll()) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                    CachedPlayer.LocalPlayer.PlayerControl.NetId,
                    (byte)CustomRPC.TaskVsMode_Start,
                    Hazel.SendOption.Reliable,
                    -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.taskVsModeStart();
            }
        }

        public static void onDisconnect(Info taskRacer) {
            if (taskRacer == null) return;
            if (!taskRacer.isReady)
                onReady(taskRacer);

            updateControl();
        }

        public static List<byte> generateBurgerTasks(int numBurgerTasks)
        {
            var task = MapUtilities.CachedShipStatus.NormalTasks.FirstOrDefault((t) => t.TaskType == TaskTypes.MakeBurger);
            if (task == null) return null;
            numBurgerTasks = Mathf.Min(numBurgerTasks, 1);
            var tasks = new Il2CppSystem.Collections.Generic.List<byte>();
            var hashSet = new Il2CppSystem.Collections.Generic.HashSet<TaskTypes>();
            var shortTasks = new Il2CppSystem.Collections.Generic.List<NormalPlayerTask>();
            for (int i = 0; i < numBurgerTasks; ++i)
                shortTasks.Add(task);
            int start = 0;
            MapUtilities.CachedShipStatus.AddTasksFromList(ref start, numBurgerTasks, tasks, hashSet, shortTasks);
            return tasks.ToArray().ToList();
        }

        static void updateVisible() {
            foreach (var taskRacer in taskRacers) {
                taskFinishedMarkTable[taskRacer.player.PlayerId].SetActive(!taskRacer.player.isDead() && taskRacer.taskCompleteCount != -1 && taskRacer.taskCount == taskRacer.taskCompleteCount);
                MapOptions.playerIcons[taskRacer.player.PlayerId].setSemiTransparent(taskRacer.player.isDead());
            }
        }

        static void sortTaskRacers() {
            var taskRacersOld = new List<Info>();
            taskRacersOld.AddRange(taskRacers);
            taskRacers.Sort((a, b) => {
                bool isAliveA = a.player.isAlive();
                bool isAliveB = b.player.isAlive();
                if (isAliveA && !isAliveB) return -1;
                if (!isAliveA && isAliveB) return 1;

                if (a.taskCompleteTime != ulong.MaxValue && b.taskCompleteTime == ulong.MaxValue) return -1;
                if (a.taskCompleteTime == ulong.MaxValue && b.taskCompleteTime != ulong.MaxValue) return 1;
                if (a.taskCompleteTime != ulong.MaxValue && b.taskCompleteTime != ulong.MaxValue) {
                    if (a.taskCompleteTime < b.taskCompleteTime) return -1;
                    if (a.taskCompleteTime > b.taskCompleteTime) return 1;
                }

                int sortOrder = b.taskCompleteCount - a.taskCompleteCount;
                if (sortOrder != 0) return sortOrder;

                return 0;
            });

            int taskRacersCount = taskRacersOld.Count;
            for (int i = 0; i < taskRacersCount; ++i) {
                if (taskRacersOld[i] == taskRacers[i]) continue;
                for (int j = 0; j < taskRacersCount; ++j) {
                    if (taskRacersOld[i] != taskRacers[j]) continue;
                    var p = MapOptions.playerIcons[taskRacersOld[i].player.PlayerId];
                    for (int k = 0; k < moveInfoList.Count; ++k) {
                        if (moveInfoList[k].moveObj == p) {
                            moveInfoList.RemoveAt(k);
                            break;
                        }
                    }
                    moveInfoList.Add(new MoveInfo(p.transform, rankUIPositions[j], 0.6f));

                    var p2 = taskFinishedMarkTable[taskRacersOld[i].player.PlayerId];
                    for (int k = 0; k < moveInfoList.Count; ++k) {
                        if (moveInfoList[k].moveObj == p2) {
                            moveInfoList.RemoveAt(k);
                            break;
                        }
                    }
                    moveInfoList.Add(new MoveInfo(p2.transform, rankUIPositions[j], 0.6f));
                }
            }
        }

        static Sprite retireSprite = null;
        public static Sprite getRetireSprites() {
            if (retireSprite == null)
                retireSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TaskVsMode_RetireButton.png", 115f);
            return retireSprite;
        }

        static Sprite taskFinishedSprite = null;
        public static Sprite getTaskFinishedSprites() {
            if (taskFinishedSprite == null)
                taskFinishedSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.TaskVsMode_Finished.png", 115f);
            return taskFinishedSprite;
        }

        static Sprite[] rankGameSprites = new Sprite[3];
        public static Sprite getRankGameSprites(int rank) {
            if (rank < 1 || rank > 3) return null;
            int index = rank - 1;
            if (rankGameSprites[index]) return rankGameSprites[index];

            int id = 0;
            switch (rank) {
                case 1: id = 11; break;
                case 2: id = 12; break;
                case 3: id = 13; break;
            }
            rankGameSprites[index] = Helpers.loadSpriteFromResources(String.Format("TheOtherRoles.Resources.TaskVsMode_{0:d2}.png", id), 115f);
            return rankGameSprites[index];
        }

        public static void setHostTasks(byte[] taskTypeIds) {
            hostTaskTypeIds = taskTypeIds;
        }

        public static void setHostTaskDetail(uint taskId, byte[] data) {
            hostTaskIdDataTable.Add(taskId, data);
        }

        static void applyHostTasks() {
            if (hostTaskTypeIds == null || hostTaskTypeIds.Length == 0) return;

            for (int i = 0; i < taskRacers.Count; ++i) {
                var playerData = taskRacers[i].player.Data;
                playerData.Object.clearAllTasks();
                playerData.Tasks = new Il2CppSystem.Collections.Generic.List<GameData.TaskInfo>(hostTaskTypeIds.Length);
                for (int j = 0; j < hostTaskTypeIds.Length; j++) {
                    playerData.Tasks.Add(new GameData.TaskInfo(hostTaskTypeIds[j], (uint)j));
                    playerData.Tasks[j].Id = (uint)j;
                }
                for (int j = 0; j < playerData.Tasks.Count; j++) {
                    GameData.TaskInfo taskInfo = playerData.Tasks[j];
                    NormalPlayerTask normalPlayerTask = UnityEngine.Object.Instantiate(MapUtilities.CachedShipStatus.GetTaskById(taskInfo.TypeId), playerData.Object.transform);
                    normalPlayerTask.Id = taskInfo.Id;
                    normalPlayerTask.Owner = playerData.Object;
                    normalPlayerTask.Initialize();
                    if (hostTaskIdDataTable.ContainsKey(normalPlayerTask.Id)) {
                        normalPlayerTask.Data = hostTaskIdDataTable[normalPlayerTask.Id];
                        switch (normalPlayerTask.TaskType) {
                            case TaskTypes.FixWiring:
                            case TaskTypes.VentCleaning:
                                List<Console> list = (from t in MapUtilities.CachedShipStatus.AllConsoles
                                                      where t.TaskTypes.Contains(normalPlayerTask.TaskType)
                                                      select t).ToList();
                                Console console = list.First((Console v) => v.ConsoleId == normalPlayerTask.Data[0]);
                                normalPlayerTask.StartAt = console.Room;
                                break;
                        }
                    }
                    playerData.Object.myTasks.Add(normalPlayerTask);
                }
            }
            hostTaskTypeIds = null;
            hostTaskIdDataTable.Clear();
        }
    }

    public static class KillerCreator {
        public static Color color = Palette.ImpostorRed;
        public static PlayerControl killerCreator;
        public static PlayerControl currentTarget;
        static Sprite killerReserveButtonSprite;


        public static Sprite getKillerReserveButtonSprite() {
            if (killerReserveButtonSprite) return killerReserveButtonSprite;
            killerReserveButtonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.SidekickButton.png", 115f);
            return killerReserveButtonSprite;
        }

        public static void clearAndReload() {
            killerCreator = null;
            currentTarget = null;
        }
    }

    public static class MadmateKiller
    {
        public static Color color = Palette.ImpostorRed;
        public static PlayerControl madmateKiller = null;
        public static bool canEnterVents = false;
        public static bool canMoveVents = false;
        public static bool hasImpostorVision = false;
        public static bool noticeImpostors = false;
        public static bool canFixLightsTask = false;
        public static bool canFixCommsTask = false;

        public static void clearAndReload() {
            madmateKiller = null;
            canEnterVents = CustomOptionHolder.madmateKillerCanEnterVents.getBool();
            canMoveVents = CustomOptionHolder.madmateKillerCanMoveVents.getBool();
            hasImpostorVision = CustomOptionHolder.madmateKillerHasImpostorVision.getBool();
            noticeImpostors = CustomOptionHolder.madmateKillerNoticeImpostors.getBool();
            canFixLightsTask = CustomOptionHolder.madmateKillerCanFixLightsTask.getBool();
            canFixCommsTask = CustomOptionHolder.madmateKillerCanFixCommsTask.getBool();
        }
    }

    // Modifier
    public static class Bait {
        public static List<PlayerControl> bait = new List<PlayerControl>();
        public static Dictionary<DeadPlayer, float> active = new Dictionary<DeadPlayer, float>();

        public static float reportDelayMin = 0f;
        public static float reportDelayMax = 0f;
        public static bool showKillFlash = true;

        public static void clearAndReload() {
            bait = new List<PlayerControl>();
            active = new Dictionary<DeadPlayer, float>();
            reportDelayMin = CustomOptionHolder.modifierBaitReportDelayMin.getFloat();
            reportDelayMax = CustomOptionHolder.modifierBaitReportDelayMax.getFloat();
            if (reportDelayMin > reportDelayMax) reportDelayMin = reportDelayMax;
            showKillFlash = CustomOptionHolder.modifierBaitShowKillFlash.getBool();
        }
    }

    public static class Bloody {
        public static List<PlayerControl> bloody = new List<PlayerControl>();
        public static Dictionary<byte, float> active = new Dictionary<byte, float>();
        public static Dictionary<byte, byte> bloodyKillerMap = new Dictionary<byte, byte>();

        public static float duration = 5f;

        public static void clearAndReload() {
            bloody = new List<PlayerControl>();
            active = new Dictionary<byte, float>();
            bloodyKillerMap = new Dictionary<byte, byte>();
            duration = CustomOptionHolder.modifierBloodyDuration.getFloat();
        }
    }

    public static class AntiTeleport {
        public static List<PlayerControl> antiTeleport = new List<PlayerControl>();
        public static Vector3 position;

        public static void clearAndReload() {
            antiTeleport = new List<PlayerControl>();
            position = Vector3.zero;
        }

        public static void setPosition() {
            if (position == Vector3.zero) return;  // Check if this has been set, otherwise first spawn on submerged will fail
            if (antiTeleport.FindAll(x => x.PlayerId == CachedPlayer.LocalPlayer.PlayerId).Count > 0) {
                CachedPlayer.LocalPlayer.NetTransform.RpcSnapTo(position);
                if (SubmergedCompatibility.IsSubmerged) {
                    SubmergedCompatibility.ChangeFloor(position.y > -7);
                }
            }
        }
    }

    public static class Tiebreaker {
        public static PlayerControl tiebreaker;

        public static bool isTiebreak = false;

        public static void clearAndReload() {
            tiebreaker = null;
            isTiebreak = false;
        }
    }

    public static class Sunglasses {
        public static List<PlayerControl> sunglasses = new List<PlayerControl>();
        public static int vision = 1;

        public static void clearAndReload() {
            sunglasses = new List<PlayerControl>();
            vision = CustomOptionHolder.modifierSunglassesVision.getSelection() + 1;
        }
    }
    public static class Mini {
        public static PlayerControl mini;
        public static Color color = Color.yellow;
        public const float defaultColliderRadius = 0.2233912f;
        public const float defaultColliderOffset = 0.3636057f;

        public static float growingUpDuration = 400f;
        public static bool isGrowingUpInMeeting = true;
        public static DateTime timeOfGrowthStart = DateTime.UtcNow;
        public static DateTime timeOfMeetingStart = DateTime.UtcNow;
        public static float ageOnMeetingStart = 0f;
        public static bool triggerMiniLose = false;

        public static void clearAndReload() {
            mini = null;
            triggerMiniLose = false;
            growingUpDuration = CustomOptionHolder.modifierMiniGrowingUpDuration.getFloat();
            isGrowingUpInMeeting = CustomOptionHolder.modifierMiniGrowingUpInMeeting.getBool();
            timeOfGrowthStart = DateTime.UtcNow;
        }

        public static float growingProgress() {
            if (timeOfGrowthStart == null) return 0f;

            float timeSinceStart = (float)(DateTime.UtcNow - timeOfGrowthStart).TotalMilliseconds;
            return Mathf.Clamp(timeSinceStart / (growingUpDuration * 1000), 0f, 1f);
        }

        public static bool isGrownUp() {
            return growingProgress() == 1f;
        }

    }
    public static class Vip {
        public static List<PlayerControl> vip = new List<PlayerControl>();
        public static bool showColor = true;

        public static void clearAndReload() {
            vip = new List<PlayerControl>();
            showColor = CustomOptionHolder.modifierVipShowColor.getBool();
        }
    }

    public static class Invert {
        public static List<PlayerControl> invert = new List<PlayerControl>();
        public static int meetings = 3;

        public static void clearAndReload() {
            invert = new List<PlayerControl>();
            meetings = (int) CustomOptionHolder.modifierInvertDuration.getFloat();
        }
    }

    public static class Chameleon {
        public static List<PlayerControl> chameleon = new List<PlayerControl>();
        public static float minVisibility = 0.2f;
        public static float holdDuration = 1f;
        public static float fadeDuration = 0.5f;
        public static Dictionary<byte, float> lastMoved;

        public static void clearAndReload() {
            chameleon = new List<PlayerControl>();
            lastMoved = new Dictionary<byte, float>();
            holdDuration = CustomOptionHolder.modifierChameleonHoldDuration.getFloat();
            fadeDuration = CustomOptionHolder.modifierChameleonFadeDuration.getFloat();
            minVisibility = CustomOptionHolder.modifierChameleonMinVisibility.getSelection() / 10f;
        }

        public static float visibility(byte playerId) {
            float visibility = 1f;
            if (lastMoved != null && lastMoved.ContainsKey(playerId)) {
                var tStill = Time.time - lastMoved[playerId];
                if (tStill > holdDuration) {
                    if (tStill - holdDuration > fadeDuration) visibility = minVisibility;
                    else visibility = (1 - (tStill - holdDuration) / fadeDuration) * (1 - minVisibility) + minVisibility;
                }
            }
            if (PlayerControl.LocalPlayer.Data.IsDead && visibility < 0.1f) {  // Ghosts can always see!
                visibility = 0.1f;
            }
            return visibility;
        }

        public static void update() {
            foreach (var chameleonPlayer in chameleon) {
                if (chameleonPlayer == Ninja.ninja && Ninja.isInvisble) continue;  // Dont make Ninja visible...
                // check movement by animation
                PlayerPhysics playerPhysics = chameleonPlayer.MyPhysics;
                var currentPhysicsAnim = playerPhysics.Animator.GetCurrentAnimation();
                if (currentPhysicsAnim != playerPhysics.CurrentAnimationGroup.IdleAnim) {
                    lastMoved[chameleonPlayer.PlayerId] = Time.time;
                }
                // calculate and set visibility
                float visibility = Chameleon.visibility(chameleonPlayer.PlayerId);
                float petVisibility = visibility;
                if (chameleonPlayer.Data.IsDead) {
                    visibility = 0.5f;
                    petVisibility = 1f;
                }

                try {  // Sometimes renderers are missing for weird reasons. Try catch to avoid exceptions
                    chameleonPlayer.cosmetics.currentBodySprite.BodySprite.color = chameleonPlayer.cosmetics.currentBodySprite.BodySprite.color.SetAlpha(visibility);
                    if (DataManager.Settings.Accessibility.ColorBlindMode) chameleonPlayer.cosmetics.colorBlindText.color = chameleonPlayer.cosmetics.colorBlindText.color.SetAlpha(visibility);
                    chameleonPlayer.SetHatAndVisorAlpha(visibility);
                    chameleonPlayer.cosmetics.skin.layer.color = chameleonPlayer.cosmetics.skin.layer.color.SetAlpha(visibility);
                    chameleonPlayer.cosmetics.nameText.color = chameleonPlayer.cosmetics.nameText.color.SetAlpha(visibility);
                    chameleonPlayer.cosmetics.currentPet.rend.color = chameleonPlayer.cosmetics.currentPet.rend.color.SetAlpha(petVisibility);
                    chameleonPlayer.cosmetics.currentPet.shadowRend.color = chameleonPlayer.cosmetics.currentPet.shadowRend.color.SetAlpha(petVisibility);
                } catch { }
            }
                
        }
    }

    public static class Shifter {
        public static PlayerControl shifter;

        public static PlayerControl futureShift;
        public static PlayerControl currentTarget;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.ShiftButton.png", 115f);
            return buttonSprite;
        }

        public static void shiftRole (PlayerControl player1, PlayerControl player2, bool repeat = true) {
            if (Mayor.mayor != null && Mayor.mayor == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Mayor.mayor = player1;
            }
            else if (Portalmaker.portalmaker != null && Portalmaker.portalmaker == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Portalmaker.portalmaker = player1;
            }
            else if (Engineer.engineer != null && Engineer.engineer == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Engineer.engineer = player1;
            }
            else if (Sheriff.sheriff != null && Sheriff.sheriff == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                if (Sheriff.formerDeputy != null && Sheriff.formerDeputy == Sheriff.sheriff) Sheriff.formerDeputy = player1;  // Shifter also shifts info on promoted deputy (to get handcuffs)
                Sheriff.sheriff = player1;
            }
            else if (Deputy.deputy != null && Deputy.deputy == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Deputy.deputy = player1;
            }
            else if (Lighter.lighter != null && Lighter.lighter == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Lighter.lighter = player1;
            }
            else if (Detective.detective != null && Detective.detective == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Detective.detective = player1;
            }
            else if (TimeMaster.timeMaster != null && TimeMaster.timeMaster == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                TimeMaster.timeMaster = player1;
            }
            else if (Medic.medic != null && Medic.medic == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Medic.medic = player1;
            }
            else if (Swapper.swapper != null && Swapper.swapper == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Swapper.swapper = player1;
            }
            else if (Seer.seer != null && Seer.seer == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Seer.seer = player1;
            }
            else if (Hacker.hacker != null && Hacker.hacker == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Hacker.hacker = player1;
            }
            else if (Tracker.tracker != null && Tracker.tracker == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Tracker.tracker = player1;
            }
            else if (Snitch.snitch != null && Snitch.snitch == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Snitch.snitch = player1;
            }
            else if (Spy.spy != null && Spy.spy == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Spy.spy = player1;
            }
            else if (SecurityGuard.securityGuard != null && SecurityGuard.securityGuard == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                SecurityGuard.securityGuard = player1;
            }
            else if (Guesser.niceGuesser != null && Guesser.niceGuesser == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Guesser.niceGuesser = player1;
            }
            else if (Medium.medium != null && Medium.medium == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Medium.medium = player1;
            }
            else if (Pursuer.pursuer != null && Pursuer.pursuer == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Pursuer.pursuer = player1;
            }
            else if (Trapper.trapper != null && Trapper.trapper == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Trapper.trapper = player1;
            }
            else if (Yasuna.yasuna != null && Yasuna.yasuna == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Yasuna.yasuna = player1;
            }
            else if (YasunaJr.yasunaJr != null && YasunaJr.yasunaJr == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                YasunaJr.yasunaJr = player1;
            }
            else if (TaskMaster.taskMaster != null && TaskMaster.taskMaster == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                TaskMaster.taskMaster = player1;
            }
            else if (Madmate.madmate != null && Madmate.madmate == player2)
            {
                if (repeat) shiftRole(player2, player1, false);
                Madmate.madmate = player1;
            }
        }

        public static void clearAndReload() {
            shifter = null;
            currentTarget = null;
            futureShift = null;
        }
    }
}
