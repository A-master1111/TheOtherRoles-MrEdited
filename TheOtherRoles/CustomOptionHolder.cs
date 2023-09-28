using System.Collections.Generic;
using UnityEngine;
using static TheOtherRoles.TheOtherRoles;
using Types = TheOtherRoles.CustomOption.CustomOptionType;

namespace TheOtherRoles {
    public class CustomOptionHolder {
        public static TranslationInfo[] rates = new[] {new TranslationInfo("0%"), new TranslationInfo("10%"), new TranslationInfo("20%"), new TranslationInfo("30%"), new TranslationInfo("40%"), new TranslationInfo("50%"), new TranslationInfo("60%"), new TranslationInfo("70%"), new TranslationInfo("80%"), new TranslationInfo("90%"), new TranslationInfo("100%")};
        public static TranslationInfo[] ratesModifier = new[] {new TranslationInfo("1"), new TranslationInfo("2"), new TranslationInfo("3")};
        public static TranslationInfo[] presets = new[] {new TranslationInfo("Opt-General", 1), new TranslationInfo("Opt-General", 2), new TranslationInfo("Opt-General", 3), new TranslationInfo("Opt-General", 4), new TranslationInfo("Opt-General", 5)};

        public static readonly Color AdminColor = new Color32(30, 144, 255, 255);
        public static readonly Color VitalColor = Color.green;
        public static readonly Color SecurityCameraColor = Color.red;
        public static readonly Color BurgerMinigameColor = Color.white;
        public static readonly Color AirShipColor = Color.yellow;

        public static CustomOption presetSelection;
        public static CustomOption activateRoles;
        public static CustomOption crewmateRolesCountMin;
        public static CustomOption crewmateRolesCountMax;
        public static CustomOption neutralRolesCountMin;
        public static CustomOption neutralRolesCountMax;
        public static CustomOption impostorRolesCountMin;
        public static CustomOption impostorRolesCountMax;
        public static CustomOption modifiersCountMin;
        public static CustomOption modifiersCountMax;

        // MR ========================================================
        public static CustomOption enabledTaskVsMode;
        public static CustomOption taskVsMode_EnabledMakeItTheSameTaskAsTheHost;
        public static CustomOption taskVsMode_Vision;
        public static CustomOption taskVsMode_EnabledBurgerMakeMode;
        public static CustomOption taskVsMode_BurgerMakeMode_BurgerLayers;
        public static CustomOption taskVsMode_BurgerMakeMode_MakeBurgerNums;

        public static CustomOption enabledHappyBirthdayMode;
        public static CustomOption happyBirthdayMode_Target;
        public static CustomOption happyBirthdayMode_EnabledTargetImpostor;
        public static CustomOption happyBirthdayMode_CakeType;
        public static CustomOption happyBirthdayMode_TargetBirthMonth;
        public static CustomOption happyBirthdayMode_TargetBirthDay;

        public static CustomOption burgerMinigameBurgerMinLayers;
        public static CustomOption burgerMinigameBurgerMaxLayers;

        public static CustomOption adminTimer;
        public static CustomOption enabledAdminTimer;
        public static CustomOption viewAdminTimer;

        public static CustomOption airshipHeliSabotageSystemTimeLimit;
        public static CustomOption hideTaskOverlayOnSabMap;
        public static CustomOption delayBeforeMeeting;
        public static CustomOption hideOutOfSightNametags;

        public static CustomOption alwaysConsumeKillCooldown;
        public static CustomOption stopConsumeKillCooldownInVent;
        public static CustomOption stopConsumeKillCooldownOnSwitchingTask;

        public static CustomOption vitalsTimer;
        public static CustomOption enabledVitalsTimer;
        public static CustomOption viewVitalsTimer;

        public static CustomOption securityCameraTimer;
        public static CustomOption enabledSecurityCameraTimer;
        public static CustomOption viewSecurityCameraTimer;
        // ===========================================================

        public static CustomOption mafiaSpawnRate;
        public static CustomOption janitorCooldown;

        public static CustomOption morphlingSpawnRate;
        public static CustomOption morphlingCooldown;
        public static CustomOption morphlingDuration;

        public static CustomOption camouflagerSpawnRate;
        public static CustomOption camouflagerCooldown;
        public static CustomOption camouflagerDuration;

        public static CustomOption evilHackerSpawnRate;
        public static CustomOption evilHackerCanMoveEvenIfUsesAdmin;
        public static CustomOption evilHackerCanCreateMadmate;
        public static CustomOption createdMadmateCanDieToSheriff;
        public static CustomOption createdMadmateCanEnterVents;
        public static CustomOption createdMadmateHasImpostorVision;
        public static CustomOption createdMadmateNoticeImpostors;
        public static CustomOption createdMadmateExileCrewmate;

        public static CustomOption vampireSpawnRate;
        public static CustomOption vampireKillDelay;
        public static CustomOption vampireCooldown;
        public static CustomOption vampireCanKillNearGarlics;

        public static CustomOption eraserSpawnRate;
        public static CustomOption eraserCooldown;
        public static CustomOption eraserCanEraseAnyone;
        public static CustomOption guesserSpawnRate;
        public static CustomOption guesserIsImpGuesserRate;
        public static CustomOption guesserNumberOfShots;
        public static CustomOption guesserHasMultipleShotsPerMeeting;
        public static CustomOption guesserKillsThroughShield;
        public static CustomOption guesserEvilCanKillSpy;
        public static CustomOption guesserSpawnBothRate;
        public static CustomOption guesserCantGuessSnitchIfTaksDone;

        public static CustomOption jesterSpawnRate;
        public static CustomOption jesterCanCallEmergency;
        public static CustomOption jesterHasImpostorVision;

        public static CustomOption arsonistSpawnRate;
        public static CustomOption arsonistCooldown;
        public static CustomOption arsonistDuration;

        public static CustomOption jackalSpawnRate;
        public static CustomOption jackalKillCooldown;
        public static CustomOption jackalCreateSidekickCooldown;
        public static CustomOption jackalCanUseVents;
        public static CustomOption jackalCanCreateSidekick;
        public static CustomOption sidekickPromotesToJackal;
        public static CustomOption sidekickCanKill;
        public static CustomOption sidekickCanUseVents;
        public static CustomOption jackalPromotedFromSidekickCanCreateSidekick;
        public static CustomOption jackalCanCreateSidekickFromImpostor;
        public static CustomOption jackalAndSidekickHaveImpostorVision;

        public static CustomOption bountyHunterSpawnRate;
        public static CustomOption bountyHunterBountyDuration;
        public static CustomOption bountyHunterReducedCooldown;
        public static CustomOption bountyHunterPunishmentTime;
        public static CustomOption bountyHunterShowArrow;
        public static CustomOption bountyHunterArrowUpdateIntervall;

        public static CustomOption witchSpawnRate;
        public static CustomOption witchCooldown;
        public static CustomOption witchAdditionalCooldown;
        public static CustomOption witchCanSpellAnyone;
        public static CustomOption witchSpellCastingDuration;
        public static CustomOption witchTriggerBothCooldowns;
        public static CustomOption witchVoteSavesTargets;

        public static CustomOption ninjaSpawnRate;
        public static CustomOption ninjaCooldown;
        public static CustomOption ninjaKnowsTargetLocation;
        public static CustomOption ninjaTraceTime;
        public static CustomOption ninjaTraceColorTime;
        public static CustomOption ninjaInvisibleDuration;

        public static CustomOption mayorSpawnRate;
        public static CustomOption mayorCanSeeVoteColors;
        public static CustomOption mayorTasksNeededToSeeVoteColors;
        public static CustomOption mayorMeetingButton;
        public static CustomOption mayorMaxRemoteMeetings;

        public static CustomOption portalmakerSpawnRate;
        public static CustomOption portalmakerCooldown;
        public static CustomOption portalmakerUsePortalCooldown;
        public static CustomOption portalmakerLogOnlyColorType;
        public static CustomOption portalmakerLogHasTime;

        public static CustomOption engineerSpawnRate;
        public static CustomOption engineerNumberOfFixes;
        public static CustomOption engineerHighlightForImpostors;
        public static CustomOption engineerHighlightForTeamJackal;

        public static CustomOption sheriffSpawnRate;
        public static CustomOption sheriffCooldown;
        public static CustomOption sheriffNumberOfShots;
        public static CustomOption sheriffCanKillNeutrals;
        public static CustomOption deputySpawnRate;

        public static CustomOption deputyNumberOfHandcuffs;
        public static CustomOption deputyHandcuffCooldown;
        public static CustomOption deputyGetsPromoted;
        public static CustomOption deputyKeepsHandcuffs;
        public static CustomOption deputyHandcuffDuration;
        public static CustomOption deputyKnowsSheriff;

        public static CustomOption lighterSpawnRate;
        public static CustomOption lighterModeLightsOnVision;
        public static CustomOption lighterModeLightsOffVision;
        public static CustomOption lighterCooldown;
        public static CustomOption lighterDuration;

        public static CustomOption detectiveSpawnRate;
        public static CustomOption detectiveAnonymousFootprints;
        public static CustomOption detectiveFootprintIntervall;
        public static CustomOption detectiveFootprintDuration;
        public static CustomOption detectiveReportNameDuration;
        public static CustomOption detectiveReportColorDuration;

        public static CustomOption timeMasterSpawnRate;
        public static CustomOption timeMasterCooldown;
        public static CustomOption timeMasterRewindTime;
        public static CustomOption timeMasterShieldDuration;

        public static CustomOption medicSpawnRate;
        public static CustomOption medicShowShielded;
        public static CustomOption medicShowAttemptToShielded;
        public static CustomOption medicSetOrShowShieldAfterMeeting;
        public static CustomOption medicShowAttemptToMedic;
        public static CustomOption medicSetShieldAfterMeeting;

        public static CustomOption swapperSpawnRate;
        public static CustomOption swapperCanCallEmergency;
        public static CustomOption swapperCanOnlySwapOthers;
        public static CustomOption swapperSwapsNumber;
        public static CustomOption swapperRechargeTasksNumber;

        public static CustomOption seerSpawnRate;
        public static CustomOption seerMode;
        public static CustomOption seerSoulDuration;
        public static CustomOption seerLimitSoulDuration;

        public static CustomOption hackerSpawnRate;
        public static CustomOption hackerCooldown;
        public static CustomOption hackerHackeringDuration;
        public static CustomOption hackerOnlyColorType;
        public static CustomOption hackerToolsNumber;
        public static CustomOption hackerRechargeTasksNumber;
        public static CustomOption hackerNoMove;

        public static CustomOption trackerSpawnRate;
        public static CustomOption trackerUpdateIntervall;
        public static CustomOption trackerResetTargetAfterMeeting;
        public static CustomOption trackerCanTrackCorpses;
        public static CustomOption trackerCorpsesTrackingCooldown;
        public static CustomOption trackerCorpsesTrackingDuration;

        public static CustomOption snitchSpawnRate;
        public static CustomOption snitchLeftTasksForReveal;
        public static CustomOption snitchIncludeTeamJackal;
        public static CustomOption snitchTeamJackalUseDifferentArrowColor;

        public static CustomOption spySpawnRate;
        public static CustomOption spyCanDieToSheriff;
        public static CustomOption spyImpostorsCanKillAnyone;
        public static CustomOption spyCanEnterVents;
        public static CustomOption spyHasImpostorVision;

        public static CustomOption tricksterSpawnRate;
        public static CustomOption tricksterPlaceBoxCooldown;
        public static CustomOption tricksterLightsOutCooldown;
        public static CustomOption tricksterLightsOutDuration;

        public static CustomOption cleanerSpawnRate;
        public static CustomOption cleanerCooldown;
        
        public static CustomOption warlockSpawnRate;
        public static CustomOption warlockCooldown;
        public static CustomOption warlockRootTime;

        public static CustomOption securityGuardSpawnRate;
        public static CustomOption securityGuardCooldown;
        public static CustomOption securityGuardTotalScrews;
        public static CustomOption securityGuardCamPrice;
        public static CustomOption securityGuardVentPrice;
        public static CustomOption securityGuardCamDuration;
        public static CustomOption securityGuardCamMaxCharges;
        public static CustomOption securityGuardCamRechargeTasksNumber;
        public static CustomOption securityGuardNoMove;

        public static CustomOption vultureSpawnRate;
        public static CustomOption vultureCooldown;
        public static CustomOption vultureNumberToWin;
        public static CustomOption vultureCanUseVents;
        public static CustomOption vultureShowArrows;

        public static CustomOption mediumSpawnRate;
        public static CustomOption mediumCooldown;
        public static CustomOption mediumDuration;
        public static CustomOption mediumOneTimeUse;

        public static CustomOption madmateSpawnRate;
        public static CustomOption madmateCanDieToSheriff;
        public static CustomOption madmateCanEnterVents;
        public static CustomOption madmateHasImpostorVision;
        public static CustomOption madmateNoticeImpostors;
        public static CustomOption madmateCommonTasks;
        public static CustomOption madmateShortTasks;
        public static CustomOption madmateLongTasks;
        public static CustomOption madmateExileCrewmate;

        public static CustomOption lawyerSpawnRate;
        public static CustomOption lawyerIsProsecutorChance;
        public static CustomOption lawyerTargetCanBeJester;
        public static CustomOption lawyerVision;
        public static CustomOption lawyerKnowsRole;
        public static CustomOption lawyerCanCallEmergency;
        public static CustomOption pursuerCooldown;
        public static CustomOption pursuerBlanksNumber;

        public static CustomOption thiefSpawnRate;
        public static CustomOption thiefCooldown;
        public static CustomOption thiefHasImpVision;
        public static CustomOption thiefCanUseVents;
        public static CustomOption thiefCanKillSheriff;


        public static CustomOption trapperSpawnRate;
        public static CustomOption trapperCooldown;
        public static CustomOption trapperMaxCharges;
        public static CustomOption trapperRechargeTasksNumber;
        public static CustomOption trapperTrapNeededTriggerToReveal;
        public static CustomOption trapperAnonymousMap;
        public static CustomOption trapperInfoType;
        public static CustomOption trapperTrapDuration;

        // MR ========================================================
        public static CustomOption yasunaSpawnRate;
        public static CustomOption yasunaIsImpYasunaRate;
        public static CustomOption yasunaNumberOfSpecialVotes;
        public static CustomOption yasunaSpecificMessageMode;

        public static CustomOption yasunaJrSpawnRate;
        public static CustomOption yasunaJrNumberOfSpecialVotes;

        public static CustomOption taskMasterSpawnRate;
        public static CustomOption taskMasterBecomeATaskMasterWhenCompleteAllTasks;
        public static CustomOption taskMasterExtraCommonTasks;
        public static CustomOption taskMasterExtraShortTasks;
        public static CustomOption taskMasterExtraLongTasks;

        public static CustomOption doorHackerSpawnRate;
        public static CustomOption doorHackerNumberOfUses;
        public static CustomOption doorHackerCooldown;
        public static CustomOption doorHackerDuration;

        public static CustomOption killerCreatorSpawnRate;
        public static CustomOption madmateKillerCanDieToSheriff;
        public static CustomOption madmateKillerCanEnterVents;
        public static CustomOption madmateKillerCanMoveVents;
        public static CustomOption madmateKillerHasImpostorVision;
        public static CustomOption madmateKillerNoticeImpostors;
        public static CustomOption madmateKillerCanFixLightsTask;
        public static CustomOption madmateKillerCanFixCommsTask;

        public static CustomOption kataomoiSpawnRate;
        public static CustomOption kataomoiStareCooldown;
        public static CustomOption kataomoiStareDuration;
        public static CustomOption kataomoiStareCount;
        public static CustomOption kataomoiStalkingCooldown;
        public static CustomOption kataomoiStalkingDuration;
        public static CustomOption kataomoiStalkingFadeTime;
        public static CustomOption kataomoiSearchCooldown;
        public static CustomOption kataomoiSearchDuration;
        // ===========================================================

        public static CustomOption modifiersAreHidden;

        public static CustomOption modifierBait;
        public static CustomOption modifierBaitQuantity;
        public static CustomOption modifierBaitReportDelayMin;
        public static CustomOption modifierBaitReportDelayMax;
        public static CustomOption modifierBaitShowKillFlash;

        public static CustomOption modifierLover;
        public static CustomOption modifierLoverImpLoverRate;
        public static CustomOption modifierLoverBothDie;
        public static CustomOption modifierLoverEnableChat;

        public static CustomOption modifierBloody;
        public static CustomOption modifierBloodyQuantity;
        public static CustomOption modifierBloodyDuration;

        public static CustomOption modifierAntiTeleport;
        public static CustomOption modifierAntiTeleportQuantity;

        public static CustomOption modifierTieBreaker;

        public static CustomOption modifierSunglasses;
        public static CustomOption modifierSunglassesQuantity;
        public static CustomOption modifierSunglassesVision;
        
        public static CustomOption modifierMini;
        public static CustomOption modifierMiniGrowingUpDuration;
        public static CustomOption modifierMiniGrowingUpInMeeting;

        public static CustomOption modifierVip;
        public static CustomOption modifierVipQuantity;
        public static CustomOption modifierVipShowColor;

        public static CustomOption modifierInvert;
        public static CustomOption modifierInvertQuantity;
        public static CustomOption modifierInvertDuration;

        public static CustomOption modifierChameleon;
        public static CustomOption modifierChameleonQuantity;
        public static CustomOption modifierChameleonHoldDuration;
        public static CustomOption modifierChameleonFadeDuration;
        public static CustomOption modifierChameleonMinVisibility;

        public static CustomOption modifierShifter;

        public static CustomOption maxNumberOfMeetings;
        public static CustomOption blockSkippingInEmergencyMeetings;
        public static CustomOption noVoteIsSelfVote;
        public static CustomOption hidePlayerNames;
        public static CustomOption allowParallelMedBayScans;
        public static CustomOption shieldFirstKill;

        // MR ========================================================
        public static CustomOption enableRandomizationInFixWiringTask;
        public static CustomOption impostorCanKillCustomRolesInTheVent;
        public static CustomOption airshipWallCheckOnTasks;
        public static CustomOption airshipRandomSpawn;
        public static CustomOption airshipAdditionalSpawn;
        public static CustomOption airshipSynchronizedSpawning;
        public static CustomOption airshipSetOriginalCooldown;
        public static CustomOption airshipInitialDoorCooldown;
        public static CustomOption airshipInitialSabotageCooldown;
        public static CustomOption airshipChangeOldAdmin;
        // ===========================================================

        public static CustomOption dynamicMap;
        public static CustomOption dynamicMapEnableSkeld;
        public static CustomOption dynamicMapEnableMira;
        public static CustomOption dynamicMapEnablePolus;
        public static CustomOption dynamicMapEnableAirShip;
        public static CustomOption dynamicMapEnableSubmerged;

        //Guesser Gamemode
        public static CustomOption guesserGamemodeCrewNumber;
        public static CustomOption guesserGamemodeNeutralNumber;
        public static CustomOption guesserGamemodeImpNumber;
        public static CustomOption guesserForceJackalGuesser;
        public static CustomOption guesserGamemodeHaveModifier;
        public static CustomOption guesserGamemodeNumberOfShots;
        public static CustomOption guesserGamemodeHasMultipleShotsPerMeeting;
        public static CustomOption guesserGamemodeKillsThroughShield;
        public static CustomOption guesserGamemodeEvilCanKillSpy;
        public static CustomOption guesserGamemodeCantGuessSnitchIfTaksDone;

        // Hide N Seek Gamemode
        public static CustomOption hideNSeekHunterCount;
        public static CustomOption hideNSeekKillCooldown;
        public static CustomOption hideNSeekHunterVision;
        public static CustomOption hideNSeekHuntedVision;
        public static CustomOption hideNSeekTimer;
        public static CustomOption hideNSeekCommonTasks;
        public static CustomOption hideNSeekShortTasks;
        public static CustomOption hideNSeekLongTasks;
        public static CustomOption hideNSeekTaskWin;
        public static CustomOption hideNSeekTaskPunish;
        public static CustomOption hideNSeekCanSabotage;
        public static CustomOption hideNSeekMap;
        public static CustomOption hideNSeekHunterWaiting;

        public static CustomOption hunterLightCooldown;
        public static CustomOption hunterLightDuration;
        public static CustomOption hunterLightVision;
        public static CustomOption hunterLightPunish;
        public static CustomOption hunterAdminCooldown;
        public static CustomOption hunterAdminDuration;
        public static CustomOption hunterAdminPunish;
        public static CustomOption hunterArrowCooldown;
        public static CustomOption hunterArrowDuration;
        public static CustomOption hunterArrowPunish;

        public static CustomOption huntedShieldCooldown;
        public static CustomOption huntedShieldDuration;
        public static CustomOption huntedShieldRewindTime;
        public static CustomOption huntedShieldNumber;

        internal static Dictionary<byte, byte[]> blockedRolePairings = new Dictionary<byte, byte[]>();

        public static string cs(Color c, string s) {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b), ToByte(c.a), s);
        }
 
        private static byte ToByte(float f) {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }

        public static void Load() {
            
            
            // Role Options
            presetSelection = CustomOption.Create(0, Types.General, new TranslationInfo("Opt-General", 6, new Color(204f / 255f, 204f / 255f, 0, 1f)), presets, null, true);
            activateRoles = CustomOption.Create(1, Types.General, new TranslationInfo("Opt-General", 7, new Color(204f / 255f, 204f / 255f, 0, 1f)), true, null, true);

            // Using new id's for the options to not break compatibilty with older versions
            crewmateRolesCountMin = CustomOption.Create(300, Types.General, new TranslationInfo("Opt-General", 8, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f, null, true);
            crewmateRolesCountMax = CustomOption.Create(301, Types.General, new TranslationInfo("Opt-General", 9, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f);
            neutralRolesCountMin = CustomOption.Create(302, Types.General, new TranslationInfo("Opt-General", 10, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f);
            neutralRolesCountMax = CustomOption.Create(303, Types.General, new TranslationInfo("Opt-General", 11, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f);
            impostorRolesCountMin = CustomOption.Create(304, Types.General, new TranslationInfo("Opt-General", 12, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f);
            impostorRolesCountMax = CustomOption.Create(305, Types.General, new TranslationInfo("Opt-General", 13, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f);
            modifiersCountMin = CustomOption.Create(306, Types.General, new TranslationInfo("Opt-General", 14, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f);
            modifiersCountMax = CustomOption.Create(307, Types.General, new TranslationInfo("Opt-General", 15, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f);

            // custom options
            enabledTaskVsMode = CustomOption.Create(900010001, Types.General, new TranslationInfo("Opt-General", 16, TaskRacer.color), false, null, true);
            taskVsMode_EnabledMakeItTheSameTaskAsTheHost = CustomOption.Create(900010002, Types.General, new TranslationInfo("Opt-General", 17, TaskRacer.color), true, enabledTaskVsMode);
            taskVsMode_Vision = CustomOption.Create(900010003, Types.General, new TranslationInfo("Opt-General", 18, TaskRacer.color), 1.5f, 0.25f, 5f, 0.25f, enabledTaskVsMode);
            taskVsMode_EnabledBurgerMakeMode = CustomOption.Create(900010004, Types.General, new TranslationInfo("Opt-General", 19, TaskRacer.color), false, enabledTaskVsMode);
            taskVsMode_BurgerMakeMode_BurgerLayers = CustomOption.Create(900010005, Types.General, new TranslationInfo("Opt-General", 20, TaskRacer.color), Patches.BurgerMinigameBeginPatch.DefaultBurgerLayers, Patches.BurgerMinigameBeginPatch.MinBurgerLayers, Patches.BurgerMinigameBeginPatch.MaxBurgerLayers, 1f, taskVsMode_EnabledBurgerMakeMode);
            taskVsMode_BurgerMakeMode_MakeBurgerNums = CustomOption.Create(900010006, Types.General, new TranslationInfo("Opt-General", 21, TaskRacer.color), 5f, 1f, 100f, 1f, taskVsMode_EnabledBurgerMakeMode);

            burgerMinigameBurgerMinLayers = CustomOption.Create(900000006, Types.General, new TranslationInfo("Opt-General", 22, BurgerMinigameColor), Patches.BurgerMinigameBeginPatch.DefaultBurgerLayers, Patches.BurgerMinigameBeginPatch.MinBurgerLayers, Patches.BurgerMinigameBeginPatch.MaxBurgerLayers, 1f, null, true);
            burgerMinigameBurgerMaxLayers = CustomOption.Create(900000007, Types.General, new TranslationInfo("Opt-General", 23, BurgerMinigameColor), Patches.BurgerMinigameBeginPatch.DefaultBurgerLayers, Patches.BurgerMinigameBeginPatch.MinBurgerLayers, Patches.BurgerMinigameBeginPatch.MaxBurgerLayers, 1f, null);

            enabledAdminTimer = CustomOption.Create(998, Types.General, new TranslationInfo("Opt-General", 24, AdminColor), true, null, true);
            adminTimer = CustomOption.Create(999, Types.General, new TranslationInfo("Opt-General", 25, AdminColor), 10f, 0f, 120f, 1f, enabledAdminTimer);
            viewAdminTimer = CustomOption.Create(10000, Types.General, new TranslationInfo("Opt-General", 26, AdminColor), true, enabledAdminTimer);

            enabledVitalsTimer = CustomOption.Create(900000001, Types.General, new TranslationInfo("Opt-General", 27, VitalColor), false, null, true);
            vitalsTimer = CustomOption.Create(900000000, Types.General, new TranslationInfo("Opt-General", 28, VitalColor), 10f, 0f, 120f, 1f, enabledVitalsTimer);
            viewVitalsTimer = CustomOption.Create(900000004, Types.General, new TranslationInfo("Opt-General", 29, VitalColor), true, enabledVitalsTimer);

            enabledSecurityCameraTimer = CustomOption.Create(900000003, Types.General, new TranslationInfo("Opt-General", 30, SecurityCameraColor), false, null, true);
            securityCameraTimer = CustomOption.Create(900000002, Types.General, new TranslationInfo("Opt-General", 31, SecurityCameraColor), 10f, 0f, 120f, 1f, enabledSecurityCameraTimer);
            viewSecurityCameraTimer = CustomOption.Create(900000005, Types.General, new TranslationInfo("Opt-General", 32, SecurityCameraColor), true, enabledSecurityCameraTimer);

            hideTaskOverlayOnSabMap = CustomOption.Create(997, Types.General, new TranslationInfo("Opt-General", 33), false, null, true);
            delayBeforeMeeting = CustomOption.Create(9921, Types.General, new TranslationInfo("Opt-General", 34), 2f, 0f, 10f, 0.25f);
            hideOutOfSightNametags = CustomOption.Create(550, Types.General, new TranslationInfo("Opt-General", 35), false);
            alwaysConsumeKillCooldown = CustomOption.Create(9911, Types.General, new TranslationInfo("Opt-General", 36), false);
            stopConsumeKillCooldownInVent = CustomOption.Create(9912, Types.General, new TranslationInfo("Opt-General", 37), false, alwaysConsumeKillCooldown);
            stopConsumeKillCooldownOnSwitchingTask = CustomOption.Create(9931, Types.General, new TranslationInfo("Opt-General", 38), false, alwaysConsumeKillCooldown);
            enableRandomizationInFixWiringTask = CustomOption.Create(920000000, Types.General, new TranslationInfo("Opt-General", 39), false);
            impostorCanKillCustomRolesInTheVent = CustomOption.Create(920000003, Types.General, new TranslationInfo("Opt-General", 40), false);

            airshipHeliSabotageSystemTimeLimit = CustomOption.Create(996, Types.General, new TranslationInfo("Opt-General", 41, AirShipColor), 90f, 5f, 120f, 5f, null, true);
            airshipWallCheckOnTasks = CustomOption.Create(920000001, Types.General, new TranslationInfo("Opt-General", 42, AirShipColor), false);
            airshipRandomSpawn = CustomOption.Create(9916, Types.General, new TranslationInfo("Opt-General", 43, AirShipColor), false);
            airshipAdditionalSpawn = CustomOption.Create(9917, Types.General, new TranslationInfo("Opt-General", 44, AirShipColor), false);
            airshipSynchronizedSpawning = CustomOption.Create(9918, Types.General, new TranslationInfo("Opt-General", 45, AirShipColor), false);
            airshipSetOriginalCooldown = CustomOption.Create(9919, Types.General, new TranslationInfo("Opt-General", 46, AirShipColor), false);
            airshipInitialDoorCooldown = CustomOption.Create(9923, Types.General, new TranslationInfo("Opt-General", 47, AirShipColor), 0f, 0f, 60f, 1f);
            airshipInitialSabotageCooldown = CustomOption.Create(9924, Types.General, new TranslationInfo("Opt-General", 48, AirShipColor), 15f, 0f, 60f, 1f);
            airshipChangeOldAdmin = CustomOption.Create(9925, Types.General, new TranslationInfo("Opt-General", 49, AirShipColor), false);


            enabledHappyBirthdayMode = CustomOption.Create(900020000, Types.General, new TranslationInfo("Opt-General", 50, Color.green), false, null, true);
            happyBirthdayMode_Target = CustomOption.Create(900020001, Types.General, new TranslationInfo("Opt-General", 51, Color.green), 0f, 0f, byte.MaxValue, 1f, enabledHappyBirthdayMode);
            happyBirthdayMode_EnabledTargetImpostor = CustomOption.Create(900020002, Types.General, new TranslationInfo("Opt-General", 52, Color.green), false, enabledHappyBirthdayMode);
            happyBirthdayMode_CakeType = CustomOption.Create(900020003, Types.General, new TranslationInfo("Opt-General", 53, Color.green), 0f, 0f, (int)Objects.BirthdayCake.CakeType.sizeof_CakeType - 1, 1f, enabledHappyBirthdayMode);
            happyBirthdayMode_TargetBirthMonth = CustomOption.Create(900020004, Types.General, new TranslationInfo("Opt-General", 54, Color.green), 0f, 0f, 12f, 1f, enabledHappyBirthdayMode);
            happyBirthdayMode_TargetBirthDay = CustomOption.Create(900020005, Types.General, new TranslationInfo("Opt-General", 55, Color.green), 0f, 0f, 31f, 1f, enabledHappyBirthdayMode);


            // MR ========================================================
            // YASUNA (Crewmate)
            yasunaSpawnRate = CustomOption.Create(910000000, Types.Crewmate, new TranslationInfo(RoleId.Yasuna, Yasuna.color), rates, null, true);
            yasunaIsImpYasunaRate = CustomOption.Create(910000002, Types.Crewmate, new TranslationInfo("Opt-Yasuna", 1), rates, yasunaSpawnRate);
            yasunaNumberOfSpecialVotes = CustomOption.Create(910000001, Types.Crewmate, new TranslationInfo("Opt-Yasuna", 2), 1f, 1f, 15, 1f, yasunaSpawnRate);
            // Change the message when the target is exiled by Yasuna's system to Yasuna-specific message (Japanese-only)
            yasunaSpecificMessageMode = CustomOption.Create(910000003, Types.Crewmate, new TranslationInfo("Opt-Yasuna", 3), false, yasunaSpawnRate);

            // YASUNA JR.(Crewmate)
            yasunaJrSpawnRate = CustomOption.Create(910000500, Types.Crewmate, new TranslationInfo(RoleId.YasunaJr, YasunaJr.color), rates, null, true);
            yasunaJrNumberOfSpecialVotes = CustomOption.Create(910000501, Types.Crewmate, new TranslationInfo("Opt-YasunaJr", 1), 1f, 1f, 15, 1f, yasunaJrSpawnRate);

            // TASK MASTER (Crewmate)
            taskMasterSpawnRate = CustomOption.Create(910000100, Types.Crewmate, new TranslationInfo(RoleId.TaskMaster, TaskMaster.color), rates, null, true);
            taskMasterBecomeATaskMasterWhenCompleteAllTasks = CustomOption.Create(910000104, Types.Crewmate, new TranslationInfo("Opt-TaskMaster", 1), false, taskMasterSpawnRate);
            taskMasterExtraCommonTasks = CustomOption.Create(910000101, Types.Crewmate, new TranslationInfo("Opt-TaskMaster", 2), 2f, 0f, 4f, 1f, taskMasterSpawnRate);
            taskMasterExtraShortTasks = CustomOption.Create(910000102, Types.Crewmate, new TranslationInfo("Opt-TaskMaster", 3), 2f, 0f, 23f, 1f, taskMasterSpawnRate);
            taskMasterExtraLongTasks = CustomOption.Create(910000103, Types.Crewmate, new TranslationInfo("Opt-TaskMaster", 4), 2f, 0f, 15f, 1f, taskMasterSpawnRate);

            // KATAOMOI (Neutral)
            kataomoiSpawnRate = CustomOption.Create(910000300, Types.Neutral, new TranslationInfo(RoleId.Kataomoi, Kataomoi.color), rates, null, true);
            kataomoiStareCooldown = CustomOption.Create(910000301, Types.Neutral, new TranslationInfo("Opt-Kataomoi", 1), 20f, 2.5f, 60f, 2.5f, kataomoiSpawnRate);
            kataomoiStareDuration = CustomOption.Create(910000302, Types.Neutral, new TranslationInfo("Opt-Kataomoi", 2), 3f, 1f, 10f, 1f, kataomoiSpawnRate);
            kataomoiStareCount = CustomOption.Create(910000303, Types.Neutral, new TranslationInfo("Opt-Kataomoi", 3), 5f, 1f, 100f, 1f, kataomoiSpawnRate);
            kataomoiStalkingCooldown = CustomOption.Create(910000304, Types.Neutral, new TranslationInfo("Opt-Kataomoi", 4), 20f, 2.5f, 60f, 2.5f, kataomoiSpawnRate);
            kataomoiStalkingDuration = CustomOption.Create(910000305, Types.Neutral, new TranslationInfo("Opt-Kataomoi", 5), 10f, 1f, 30f, 1f, kataomoiSpawnRate);
            kataomoiStalkingFadeTime = CustomOption.Create(910000306, Types.Neutral, new TranslationInfo("Opt-Kataomoi", 6), 0.5f, 0.0f, 2.5f, 0.5f, kataomoiSpawnRate);
            kataomoiSearchCooldown = CustomOption.Create(910000307, Types.Neutral, new TranslationInfo("Opt-Kataomoi", 7), 10f, 2.5f, 60f, 2.5f, kataomoiSpawnRate);
            kataomoiSearchDuration = CustomOption.Create(910000308, Types.Neutral, new TranslationInfo("Opt-Kataomoi", 8), 10f, 1f, 30f, 1f, kataomoiSpawnRate);

            // DOOR HACKER (Impostor)
            doorHackerSpawnRate = CustomOption.Create(910000200, Types.Impostor, new TranslationInfo(RoleId.DoorHacker, DoorHacker.color), rates, null, true);
            doorHackerNumberOfUses = CustomOption.Create(910000203, Types.Impostor, new TranslationInfo("Opt-DoorHacker", 1), 0f, 0f, 15, 1f, doorHackerSpawnRate);
            doorHackerCooldown = CustomOption.Create(910000201, Types.Impostor, new TranslationInfo("Opt-DoorHacker", 2), 30f, 0f, 60f, 2.5f, doorHackerSpawnRate);
            doorHackerDuration = CustomOption.Create(910000202, Types.Impostor, new TranslationInfo("Opt-DoorHacker", 3), 5f, 1f, 30f, 0.5f, doorHackerSpawnRate);

            // KILLER CREATOR (Impostor)
            killerCreatorSpawnRate = CustomOption.Create(910000400, Types.Impostor, new TranslationInfo(RoleId.KillerCreator, KillerCreator.color), rates, null, true);
            madmateKillerCanDieToSheriff = CustomOption.Create(910000401, Types.Impostor, new TranslationInfo("Opt-KillerCreator", 1), false, killerCreatorSpawnRate);
            madmateKillerCanEnterVents = CustomOption.Create(910000402, Types.Impostor, new TranslationInfo("Opt-KillerCreator", 2), false, killerCreatorSpawnRate);
            madmateKillerCanMoveVents = CustomOption.Create(910000407, Types.Impostor, new TranslationInfo("Opt-KillerCreator", 3), false, killerCreatorSpawnRate);
            madmateKillerHasImpostorVision = CustomOption.Create(910000403, Types.Impostor, new TranslationInfo("Opt-KillerCreator", 4), false, killerCreatorSpawnRate);
            madmateKillerNoticeImpostors = CustomOption.Create(910000404, Types.Impostor, new TranslationInfo("Opt-KillerCreator", 5), false, killerCreatorSpawnRate);
            madmateKillerCanFixLightsTask = CustomOption.Create(910000405, Types.Impostor, new TranslationInfo("Opt-KillerCreator", 6), false, killerCreatorSpawnRate);
            madmateKillerCanFixCommsTask = CustomOption.Create(910000406, Types.Impostor, new TranslationInfo("Opt-KillerCreator", 7), false, killerCreatorSpawnRate);
            // ===========================================================

            mafiaSpawnRate = CustomOption.Create(10, Types.Impostor, new TranslationInfo(RoleId.Mafioso, Janitor.color), rates, null, true);
            janitorCooldown = CustomOption.Create(11, Types.Impostor, new TranslationInfo("Opt-Mafia", 1), 30f, 10f, 60f, 2.5f, mafiaSpawnRate);

            morphlingSpawnRate = CustomOption.Create(20, Types.Impostor, new TranslationInfo(RoleId.Morphling, Morphling.color), rates, null, true);
            morphlingCooldown = CustomOption.Create(21, Types.Impostor, new TranslationInfo("Opt-Morphling", 1), 30f, 10f, 60f, 2.5f, morphlingSpawnRate);
            morphlingDuration = CustomOption.Create(22, Types.Impostor, new TranslationInfo("Opt-Morphling", 2), 10f, 1f, 20f, 0.5f, morphlingSpawnRate);

            camouflagerSpawnRate = CustomOption.Create(30, Types.Impostor, new TranslationInfo(RoleId.Camouflager, Camouflager.color), rates, null, true);
            camouflagerCooldown = CustomOption.Create(31, Types.Impostor, new TranslationInfo("Opt-Camouflager", 1), 30f, 10f, 60f, 2.5f, camouflagerSpawnRate);
            camouflagerDuration = CustomOption.Create(32, Types.Impostor, new TranslationInfo("Opt-Camouflager", 2), 10f, 1f, 20f, 0.5f, camouflagerSpawnRate);

            // custom options for evilHacker
            evilHackerSpawnRate = CustomOption.Create(900, Types.Impostor, new TranslationInfo(RoleId.EvilHacker, EvilHacker.color), rates, null, true);
            evilHackerCanMoveEvenIfUsesAdmin = CustomOption.Create(1913, Types.Impostor, new TranslationInfo("Opt-EvilHacker", 1), true, evilHackerSpawnRate);
            evilHackerCanCreateMadmate = CustomOption.Create(901, Types.Impostor, new TranslationInfo("Opt-EvilHacker", 2), false, evilHackerSpawnRate);
            createdMadmateCanDieToSheriff = CustomOption.Create(902, Types.Impostor, new TranslationInfo("Opt-EvilHacker", 3), false, evilHackerCanCreateMadmate);
            createdMadmateCanEnterVents = CustomOption.Create(903, Types.Impostor, new TranslationInfo("Opt-EvilHacker", 4), false, evilHackerCanCreateMadmate);
            createdMadmateHasImpostorVision = CustomOption.Create(904, Types.Impostor, new TranslationInfo("Opt-EvilHacker", 5), false, evilHackerCanCreateMadmate);
            createdMadmateNoticeImpostors = CustomOption.Create(905, Types.Impostor, new TranslationInfo("Opt-EvilHacker", 6), false, evilHackerCanCreateMadmate);
            createdMadmateExileCrewmate = CustomOption.Create(906, Types.Impostor, new TranslationInfo("Opt-EvilHacker", 7), false, evilHackerCanCreateMadmate);

            vampireSpawnRate = CustomOption.Create(40, Types.Impostor, new TranslationInfo(RoleId.Vampire, Vampire.color), rates, null, true);
            vampireKillDelay = CustomOption.Create(41, Types.Impostor, new TranslationInfo("Opt-Vampire", 1), 10f, 1f, 20f, 1f, vampireSpawnRate);
            vampireCooldown = CustomOption.Create(42, Types.Impostor, new TranslationInfo("Opt-Vampire", 2), 30f, 10f, 60f, 2.5f, vampireSpawnRate);
            vampireCanKillNearGarlics = CustomOption.Create(43, Types.Impostor, new TranslationInfo("Opt-Vampire", 3), true, vampireSpawnRate);

            eraserSpawnRate = CustomOption.Create(230, Types.Impostor, new TranslationInfo(RoleId.Eraser, Eraser.color), rates, null, true);
            eraserCooldown = CustomOption.Create(231, Types.Impostor, new TranslationInfo("Opt-Eraser", 1), 30f, 10f, 120f, 5f, eraserSpawnRate);
            eraserCanEraseAnyone = CustomOption.Create(232, Types.Impostor, new TranslationInfo("Opt-Eraser", 2), false, eraserSpawnRate);

            tricksterSpawnRate = CustomOption.Create(250, Types.Impostor, new TranslationInfo(RoleId.Trickster, Trickster.color), rates, null, true);
            tricksterPlaceBoxCooldown = CustomOption.Create(251, Types.Impostor, new TranslationInfo("Opt-Trickster", 1), 10f, 2.5f, 30f, 2.5f, tricksterSpawnRate);
            tricksterLightsOutCooldown = CustomOption.Create(252, Types.Impostor, new TranslationInfo("Opt-Trickster", 2), 30f, 10f, 60f, 5f, tricksterSpawnRate);
            tricksterLightsOutDuration = CustomOption.Create(253, Types.Impostor, new TranslationInfo("Opt-Trickster", 3), 15f, 5f, 60f, 2.5f, tricksterSpawnRate);

            cleanerSpawnRate = CustomOption.Create(260, Types.Impostor, new TranslationInfo(RoleId.Cleaner, Cleaner.color), rates, null, true);
            cleanerCooldown = CustomOption.Create(261, Types.Impostor, new TranslationInfo("Opt-Cleaner", 1), 30f, 10f, 60f, 2.5f, cleanerSpawnRate);

            warlockSpawnRate = CustomOption.Create(270, Types.Impostor, new TranslationInfo(RoleId.Warlock, Cleaner.color), rates, null, true);
            warlockCooldown = CustomOption.Create(271, Types.Impostor, new TranslationInfo("Opt-Warlock", 1), 30f, 10f, 60f, 2.5f, warlockSpawnRate);
            warlockRootTime = CustomOption.Create(272, Types.Impostor, new TranslationInfo("Opt-Warlock", 2), 5f, 0f, 15f, 1f, warlockSpawnRate);

            bountyHunterSpawnRate = CustomOption.Create(320, Types.Impostor, new TranslationInfo(RoleId.BountyHunter, BountyHunter.color), rates, null, true);
            bountyHunterBountyDuration = CustomOption.Create(321, Types.Impostor, new TranslationInfo("Opt-BountyHunter", 1),  60f, 10f, 180f, 10f, bountyHunterSpawnRate);
            bountyHunterReducedCooldown = CustomOption.Create(322, Types.Impostor, new TranslationInfo("Opt-BountyHunter", 2), 2.5f, 0f, 30f, 2.5f, bountyHunterSpawnRate);
            bountyHunterPunishmentTime = CustomOption.Create(323, Types.Impostor, new TranslationInfo("Opt-BountyHunter", 3), 20f, 0f, 60f, 2.5f, bountyHunterSpawnRate);
            bountyHunterShowArrow = CustomOption.Create(324, Types.Impostor, new TranslationInfo("Opt-BountyHunter", 4), true, bountyHunterSpawnRate);
            bountyHunterArrowUpdateIntervall = CustomOption.Create(325, Types.Impostor, new TranslationInfo("Opt-BountyHunter", 5), 15f, 2.5f, 60f, 2.5f, bountyHunterShowArrow);

            witchSpawnRate = CustomOption.Create(370, Types.Impostor, new TranslationInfo(RoleId.Witch, Witch.color), rates, null, true);
            witchCooldown = CustomOption.Create(371, Types.Impostor, new TranslationInfo("Opt-Witch", 1), 30f, 10f, 120f, 5f, witchSpawnRate);
            witchAdditionalCooldown = CustomOption.Create(372, Types.Impostor, new TranslationInfo("Opt-Witch", 2), 10f, 0f, 60f, 5f, witchSpawnRate);
            witchCanSpellAnyone = CustomOption.Create(373, Types.Impostor, new TranslationInfo("Opt-Witch", 3), false, witchSpawnRate);
            witchSpellCastingDuration = CustomOption.Create(374, Types.Impostor, new TranslationInfo("Opt-Witch", 4), 1f, 0f, 10f, 1f, witchSpawnRate);
            witchTriggerBothCooldowns = CustomOption.Create(375, Types.Impostor, new TranslationInfo("Opt-Witch", 5), true, witchSpawnRate);
            witchVoteSavesTargets = CustomOption.Create(376, Types.Impostor, new TranslationInfo("Opt-Witch", 6), true, witchSpawnRate);

            ninjaSpawnRate = CustomOption.Create(380, Types.Impostor, new TranslationInfo(RoleId.Ninja, Ninja.color), rates, null, true);
            ninjaCooldown = CustomOption.Create(381, Types.Impostor, new TranslationInfo("Opt-Ninja", 1), 30f, 10f, 120f, 5f, ninjaSpawnRate);
            ninjaKnowsTargetLocation = CustomOption.Create(382, Types.Impostor, new TranslationInfo("Opt-Ninja", 2), true, ninjaSpawnRate);
            ninjaTraceTime = CustomOption.Create(383, Types.Impostor, new TranslationInfo("Opt-Ninja", 3), 5f, 1f, 20f, 0.5f, ninjaSpawnRate);
            ninjaTraceColorTime = CustomOption.Create(384, Types.Impostor, new TranslationInfo("Opt-Ninja", 4), 2f, 0f, 20f, 0.5f, ninjaSpawnRate);
            ninjaInvisibleDuration = CustomOption.Create(385, Types.Impostor, new TranslationInfo("Opt-Ninja", 5), 3f, 0f, 20f, 1f, ninjaSpawnRate);

            guesserSpawnRate = CustomOption.Create(310, Types.Neutral, new TranslationInfo(RoleId.NiceGuesser, Guesser.color), rates, null, true);
            guesserIsImpGuesserRate = CustomOption.Create(311, Types.Neutral, new TranslationInfo("Opt-Guesser", 1), rates, guesserSpawnRate);
            guesserNumberOfShots = CustomOption.Create(312, Types.Neutral, new TranslationInfo("Opt-Guesser", 2), 2f, 1f, 15f, 1f, guesserSpawnRate);
            guesserHasMultipleShotsPerMeeting = CustomOption.Create(313, Types.Neutral, new TranslationInfo("Opt-Guesser", 3), false, guesserSpawnRate);
            guesserKillsThroughShield  = CustomOption.Create(315, Types.Neutral, new TranslationInfo("Opt-Guesser", 4), true, guesserSpawnRate);
            guesserEvilCanKillSpy  = CustomOption.Create(316, Types.Neutral, new TranslationInfo("Opt-Guesser", 5), true, guesserSpawnRate);
            guesserSpawnBothRate = CustomOption.Create(317, Types.Neutral, new TranslationInfo("Opt-Guesser", 6), rates, guesserSpawnRate);
            guesserCantGuessSnitchIfTaksDone = CustomOption.Create(318, Types.Neutral, new TranslationInfo("Opt-Guesser", 7), true, guesserSpawnRate);

            jesterSpawnRate = CustomOption.Create(60, Types.Neutral, new TranslationInfo(RoleId.Jester, Jester.color), rates, null, true);
            jesterCanCallEmergency = CustomOption.Create(61, Types.Neutral, new TranslationInfo("Opt-Jester", 1), true, jesterSpawnRate);
            jesterHasImpostorVision = CustomOption.Create(62, Types.Neutral, new TranslationInfo("Opt-Jester", 2), false, jesterSpawnRate);

            arsonistSpawnRate = CustomOption.Create(290, Types.Neutral, new TranslationInfo(RoleId.Arsonist, Arsonist.color), rates, null, true);
            arsonistCooldown = CustomOption.Create(291, Types.Neutral, new TranslationInfo("Opt-Arsonist", 1), 12.5f, 2.5f, 60f, 2.5f, arsonistSpawnRate);
            arsonistDuration = CustomOption.Create(292, Types.Neutral, new TranslationInfo("Opt-Arsonist", 2), 3f, 1f, 10f, 1f, arsonistSpawnRate);

            jackalSpawnRate = CustomOption.Create(220, Types.Neutral, new TranslationInfo(RoleId.Jackal, Jackal.color), rates, null, true);
            jackalKillCooldown = CustomOption.Create(221, Types.Neutral, new TranslationInfo("Opt-Jackal", 1), 30f, 10f, 60f, 2.5f, jackalSpawnRate);
            jackalCreateSidekickCooldown = CustomOption.Create(222, Types.Neutral, new TranslationInfo("Opt-Jackal", 2), 30f, 10f, 60f, 2.5f, jackalSpawnRate);
            jackalCanUseVents = CustomOption.Create(223, Types.Neutral, new TranslationInfo("Opt-Jackal", 3), true, jackalSpawnRate);
            jackalCanCreateSidekick = CustomOption.Create(224, Types.Neutral, new TranslationInfo("Opt-Jackal", 4), false, jackalSpawnRate);
            sidekickPromotesToJackal = CustomOption.Create(225, Types.Neutral, new TranslationInfo("Opt-Jackal", 5), false, jackalCanCreateSidekick);
            sidekickCanKill = CustomOption.Create(226, Types.Neutral, new TranslationInfo("Opt-Jackal", 6), false, jackalCanCreateSidekick);
            sidekickCanUseVents = CustomOption.Create(227, Types.Neutral, new TranslationInfo("Opt-Jackal", 7), true, jackalCanCreateSidekick);
            jackalPromotedFromSidekickCanCreateSidekick = CustomOption.Create(228, Types.Neutral, new TranslationInfo("Opt-Jackal", 8), true, sidekickPromotesToJackal);
            jackalCanCreateSidekickFromImpostor = CustomOption.Create(229, Types.Neutral, new TranslationInfo("Opt-Jackal", 9), true, jackalCanCreateSidekick);
            jackalAndSidekickHaveImpostorVision = CustomOption.Create(430, Types.Neutral, new TranslationInfo("Opt-Jackal", 10), false, jackalSpawnRate);

            vultureSpawnRate = CustomOption.Create(340, Types.Neutral, new TranslationInfo(RoleId.Vulture, Vulture.color), rates, null, true);
            vultureCooldown = CustomOption.Create(341, Types.Neutral, new TranslationInfo("Opt-Vulture", 1), 15f, 10f, 60f, 2.5f, vultureSpawnRate);
            vultureNumberToWin = CustomOption.Create(342, Types.Neutral, new TranslationInfo("Opt-Vulture", 2), 4f, 1f, 10f, 1f, vultureSpawnRate);
            vultureCanUseVents = CustomOption.Create(343, Types.Neutral, new TranslationInfo("Opt-Vulture", 3), true, vultureSpawnRate);
            vultureShowArrows = CustomOption.Create(344, Types.Neutral, new TranslationInfo("Opt-Vulture", 4), true, vultureSpawnRate);

            lawyerSpawnRate = CustomOption.Create(350, Types.Neutral, new TranslationInfo(RoleId.Lawyer, Lawyer.color), rates, null, true);
            lawyerIsProsecutorChance = CustomOption.Create(358, Types.Neutral, new TranslationInfo("Opt-Lawyer", 1), rates, lawyerSpawnRate);
            lawyerVision = CustomOption.Create(354, Types.Neutral, new TranslationInfo("Opt-Lawyer", 2), 1f, 0.25f, 3f, 0.25f, lawyerSpawnRate);
            lawyerKnowsRole = CustomOption.Create(355, Types.Neutral, new TranslationInfo("Opt-Lawyer", 3), false, lawyerSpawnRate);
            lawyerCanCallEmergency = CustomOption.Create(352, Types.Neutral, new TranslationInfo("Opt-Lawyer", 4), true, lawyerSpawnRate);
            lawyerTargetCanBeJester = CustomOption.Create(351, Types.Neutral, new TranslationInfo("Opt-Lawyer", 5), false, lawyerSpawnRate);
            pursuerCooldown = CustomOption.Create(356, Types.Neutral, new TranslationInfo("Opt-Lawyer", 6), 30f, 5f, 60f, 2.5f, lawyerSpawnRate);
            pursuerBlanksNumber = CustomOption.Create(357, Types.Neutral, new TranslationInfo("Opt-Lawyer", 7), 5f, 1f, 20f, 1f, lawyerSpawnRate);

            mayorSpawnRate = CustomOption.Create(80, Types.Crewmate, new TranslationInfo(RoleId.Mayor, Mayor.color), rates, null, true);
            mayorCanSeeVoteColors = CustomOption.Create(81, Types.Crewmate, new TranslationInfo("Opt-Mayor", 1), false, mayorSpawnRate);
            mayorTasksNeededToSeeVoteColors = CustomOption.Create(82, Types.Crewmate, new TranslationInfo("Opt-Mayor", 2), 5f, 0f, 20f, 1f, mayorCanSeeVoteColors);
            mayorMeetingButton = CustomOption.Create(83, Types.Crewmate, new TranslationInfo("Opt-Mayor", 3), true, mayorSpawnRate);
            mayorMaxRemoteMeetings = CustomOption.Create(84, Types.Crewmate, new TranslationInfo("Opt-Mayor", 4), 1f, 1f, 5f, 1f, mayorMeetingButton);

            engineerSpawnRate = CustomOption.Create(90, Types.Crewmate, new TranslationInfo(RoleId.Engineer, Engineer.color), rates, null, true);
            engineerNumberOfFixes = CustomOption.Create(91, Types.Crewmate, new TranslationInfo("Opt-Engineer", 1), 1f, 1f, 3f, 1f, engineerSpawnRate);
            engineerHighlightForImpostors = CustomOption.Create(92, Types.Crewmate, new TranslationInfo("Opt-Engineer", 2), true, engineerSpawnRate);
            engineerHighlightForTeamJackal = CustomOption.Create(93, Types.Crewmate, new TranslationInfo("Opt-Engineer", 3), true, engineerSpawnRate);

            sheriffSpawnRate = CustomOption.Create(100, Types.Crewmate, new TranslationInfo(RoleId.Sheriff, Sheriff.color), rates, null, true);
            sheriffCooldown = CustomOption.Create(101, Types.Crewmate, new TranslationInfo("Opt-Sheriff", 1), 30f, 10f, 60f, 2.5f, sheriffSpawnRate);
            sheriffCanKillNeutrals = CustomOption.Create(102, Types.Crewmate, new TranslationInfo("Opt-Sheriff", 2), false, sheriffSpawnRate);
            sheriffNumberOfShots = CustomOption.Create(920, Types.Crewmate, new TranslationInfo("Opt-Sheriff", 3), 1f, 1f, 15, 1f, sheriffSpawnRate);
            deputySpawnRate = CustomOption.Create(103, Types.Crewmate, new TranslationInfo("Opt-Sheriff", 4), rates, sheriffSpawnRate);
            deputyNumberOfHandcuffs = CustomOption.Create(104, Types.Crewmate, new TranslationInfo("Opt-Sheriff", 5), 3f, 1f, 10f, 1f, deputySpawnRate);
            deputyHandcuffCooldown = CustomOption.Create(105, Types.Crewmate, new TranslationInfo("Opt-Sheriff", 6), 30f, 10f, 60f, 2.5f, deputySpawnRate);
            deputyHandcuffDuration = CustomOption.Create(106, Types.Crewmate, new TranslationInfo("Opt-Sheriff", 7), 15f, 5f, 60f, 2.5f, deputySpawnRate);
            deputyKnowsSheriff = CustomOption.Create(107, Types.Crewmate, new TranslationInfo("Opt-Sheriff", 8), true, deputySpawnRate);
            deputyGetsPromoted = CustomOption.Create(108, Types.Crewmate, new TranslationInfo("Opt-Sheriff", 9), new[] { new TranslationInfo("Opt-Sheriff", 100), new TranslationInfo("Opt-Sheriff", 101), new TranslationInfo("Opt-Sheriff", 102) }, deputySpawnRate);
            deputyKeepsHandcuffs = CustomOption.Create(109, Types.Crewmate, new TranslationInfo("Opt-Sheriff", 10), true, deputyGetsPromoted);

            lighterSpawnRate = CustomOption.Create(110, Types.Crewmate, new TranslationInfo(RoleId.Lighter, Lighter.color), rates, null, true);
            lighterModeLightsOnVision = CustomOption.Create(111, Types.Crewmate, new TranslationInfo("Opt-Lighter", 1), 2f, 0.25f, 5f, 0.25f, lighterSpawnRate);
            lighterModeLightsOffVision = CustomOption.Create(112, Types.Crewmate, new TranslationInfo("Opt-Lighter", 2), 0.75f, 0.25f, 5f, 0.25f, lighterSpawnRate);
            lighterCooldown = CustomOption.Create(113, Types.Crewmate, new TranslationInfo("Opt-Lighter", 3), 30f, 5f, 120f, 5f, lighterSpawnRate);
            lighterDuration = CustomOption.Create(114, Types.Crewmate, new TranslationInfo("Opt-Lighter", 4), 5f, 2.5f, 60f, 2.5f, lighterSpawnRate);

            detectiveSpawnRate = CustomOption.Create(120, Types.Crewmate, new TranslationInfo(RoleId.Detective, Detective.color), rates, null, true);
            detectiveAnonymousFootprints = CustomOption.Create(121, Types.Crewmate, new TranslationInfo("Opt-Detective", 1), false, detectiveSpawnRate); 
            detectiveFootprintIntervall = CustomOption.Create(122, Types.Crewmate, new TranslationInfo("Opt-Detective", 2), 0.5f, 0.25f, 10f, 0.25f, detectiveSpawnRate);
            detectiveFootprintDuration = CustomOption.Create(123, Types.Crewmate, new TranslationInfo("Opt-Detective", 3), 5f, 0.25f, 10f, 0.25f, detectiveSpawnRate);
            detectiveReportNameDuration = CustomOption.Create(124, Types.Crewmate, new TranslationInfo("Opt-Detective", 4), 0, 0, 60, 2.5f, detectiveSpawnRate);
            detectiveReportColorDuration = CustomOption.Create(125, Types.Crewmate, new TranslationInfo("Opt-Detective", 5), 20, 0, 120, 2.5f, detectiveSpawnRate);

            timeMasterSpawnRate = CustomOption.Create(130, Types.Crewmate, new TranslationInfo(RoleId.TimeMaster, TimeMaster.color), rates, null, true);
            timeMasterCooldown = CustomOption.Create(131, Types.Crewmate, new TranslationInfo("Opt-TimeMaster", 1), 30f, 10f, 120f, 2.5f, timeMasterSpawnRate);
            timeMasterRewindTime = CustomOption.Create(132, Types.Crewmate, new TranslationInfo("Opt-TimeMaster", 2), 3f, 1f, 10f, 1f, timeMasterSpawnRate);
            timeMasterShieldDuration = CustomOption.Create(133, Types.Crewmate, new TranslationInfo("Opt-TimeMaster", 3), 3f, 1f, 20f, 1f, timeMasterSpawnRate);

            medicSpawnRate = CustomOption.Create(140, Types.Crewmate, new TranslationInfo(RoleId.Medic, Medic.color), rates, null, true);
            medicShowShielded = CustomOption.Create(143, Types.Crewmate, new TranslationInfo("Opt-Medic", 1), new[] { new TranslationInfo("Opt-Medic", 100), new TranslationInfo("Opt-Medic", 101), new TranslationInfo("Opt-Medic", 102)}, medicSpawnRate);
            medicShowAttemptToShielded = CustomOption.Create(144, Types.Crewmate, new TranslationInfo("Opt-Medic", 2), false, medicSpawnRate);
            medicSetOrShowShieldAfterMeeting = CustomOption.Create(145, Types.Crewmate, new TranslationInfo("Opt-Medic", 3), new[] { new TranslationInfo("Opt-Medic", 110), new TranslationInfo("Opt-Medic", 111), new TranslationInfo("Opt-Medic", 112) }, medicSpawnRate);
            medicShowAttemptToMedic = CustomOption.Create(146, Types.Crewmate, new TranslationInfo("Opt-Medic", 4), false, medicSpawnRate);

            swapperSpawnRate = CustomOption.Create(150, Types.Crewmate, new TranslationInfo(RoleId.Swapper, Swapper.color), rates, null, true);
            swapperCanCallEmergency = CustomOption.Create(151, Types.Crewmate, new TranslationInfo("Opt-Swapper", 1), false, swapperSpawnRate);
            swapperCanOnlySwapOthers = CustomOption.Create(152, Types.Crewmate, new TranslationInfo("Opt-Swapper", 2), false, swapperSpawnRate);
            swapperSwapsNumber = CustomOption.Create(153, Types.Crewmate, new TranslationInfo("Opt-Swapper", 3), 1f, 0f, 5f, 1f, swapperSpawnRate);
            swapperRechargeTasksNumber = CustomOption.Create(154, Types.Crewmate, new TranslationInfo("Opt-Swapper", 4), 2f, 1f, 10f, 1f, swapperSpawnRate);

            seerSpawnRate = CustomOption.Create(160, Types.Crewmate, new TranslationInfo(RoleId.Seer, Seer.color), rates, null, true);
            seerMode = CustomOption.Create(161, Types.Crewmate, new TranslationInfo("Opt-Seer", 1), new[] { new TranslationInfo("Opt-Seer", 100), new TranslationInfo("Opt-Seer", 101), new TranslationInfo("Opt-Seer", 102) }, seerSpawnRate);
            seerLimitSoulDuration = CustomOption.Create(163, Types.Crewmate, new TranslationInfo("Opt-Seer", 2), false, seerSpawnRate);
            seerSoulDuration = CustomOption.Create(162, Types.Crewmate, new TranslationInfo("Opt-Seer", 3), 15f, 0f, 120f, 5f, seerLimitSoulDuration);
        
            hackerSpawnRate = CustomOption.Create(170, Types.Crewmate, new TranslationInfo(RoleId.Hacker, Hacker.color), rates, null, true);
            hackerCooldown = CustomOption.Create(171, Types.Crewmate, new TranslationInfo("Opt-Hacker", 1), 30f, 5f, 60f, 5f, hackerSpawnRate);
            hackerHackeringDuration = CustomOption.Create(172, Types.Crewmate, new TranslationInfo("Opt-Hacker", 2), 10f, 2.5f, 60f, 2.5f, hackerSpawnRate);
            hackerOnlyColorType = CustomOption.Create(173, Types.Crewmate, new TranslationInfo("Opt-Hacker", 3), false, hackerSpawnRate);
            hackerToolsNumber = CustomOption.Create(174, Types.Crewmate, new TranslationInfo("Opt-Hacker", 4), 5f, 1f, 30f, 1f, hackerSpawnRate);
            hackerRechargeTasksNumber = CustomOption.Create(175, Types.Crewmate, new TranslationInfo("Opt-Hacker", 5), 2f, 1f, 5f, 1f, hackerSpawnRate);
            hackerNoMove = CustomOption.Create(176, Types.Crewmate, new TranslationInfo("Opt-Hacker", 6), true, hackerSpawnRate);

            trackerSpawnRate = CustomOption.Create(200, Types.Crewmate, new TranslationInfo(RoleId.Tracker, Tracker.color), rates, null, true);
            trackerUpdateIntervall = CustomOption.Create(201, Types.Crewmate, new TranslationInfo("Opt-Tracker", 1), 5f, 1f, 30f, 1f, trackerSpawnRate);
            trackerResetTargetAfterMeeting = CustomOption.Create(202, Types.Crewmate, new TranslationInfo("Opt-Tracker", 2), false, trackerSpawnRate);
            trackerCanTrackCorpses = CustomOption.Create(203, Types.Crewmate, new TranslationInfo("Opt-Tracker", 3), true, trackerSpawnRate);
            trackerCorpsesTrackingCooldown = CustomOption.Create(204, Types.Crewmate, new TranslationInfo("Opt-Tracker", 4), 30f, 5f, 120f, 5f, trackerCanTrackCorpses);
            trackerCorpsesTrackingDuration = CustomOption.Create(205, Types.Crewmate, new TranslationInfo("Opt-Tracker", 5), 5f, 2.5f, 30f, 2.5f, trackerCanTrackCorpses);
                           
            snitchSpawnRate = CustomOption.Create(210, Types.Crewmate, new TranslationInfo(RoleId.Snitch, Snitch.color), rates, null, true);
            snitchLeftTasksForReveal = CustomOption.Create(211, Types.Crewmate, new TranslationInfo("Opt-Snitch", 1), 1f, 0f, 5f, 1f, snitchSpawnRate);
            snitchIncludeTeamJackal = CustomOption.Create(212, Types.Crewmate, new TranslationInfo("Opt-Snitch", 2), false, snitchSpawnRate);
            snitchTeamJackalUseDifferentArrowColor = CustomOption.Create(213, Types.Crewmate, new TranslationInfo("Opt-Snitch", 3), true, snitchIncludeTeamJackal);

            spySpawnRate = CustomOption.Create(240, Types.Crewmate, new TranslationInfo(RoleId.Spy, Spy.color), rates, null, true);
            spyCanDieToSheriff = CustomOption.Create(241, Types.Crewmate, new TranslationInfo("Opt-Spy", 1), false, spySpawnRate);
            spyImpostorsCanKillAnyone = CustomOption.Create(242, Types.Crewmate, new TranslationInfo("Opt-Spy", 2), true, spySpawnRate);
            spyCanEnterVents = CustomOption.Create(243, Types.Crewmate, new TranslationInfo("Opt-Spy", 3), false, spySpawnRate);
            spyHasImpostorVision = CustomOption.Create(244, Types.Crewmate, new TranslationInfo("Opt-Spy", 4), false, spySpawnRate);

            portalmakerSpawnRate = CustomOption.Create(390, Types.Crewmate, new TranslationInfo(RoleId.Portalmaker, Portalmaker.color), rates, null, true);
            portalmakerCooldown = CustomOption.Create(391, Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 1), 30f, 10f, 60f, 2.5f, portalmakerSpawnRate);
            portalmakerUsePortalCooldown = CustomOption.Create(392, Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 2), 30f, 10f, 60f, 2.5f, portalmakerSpawnRate);
            portalmakerLogOnlyColorType = CustomOption.Create(393, Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 3), true, portalmakerSpawnRate);
            portalmakerLogHasTime = CustomOption.Create(394, Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 4), true, portalmakerSpawnRate);

            securityGuardSpawnRate = CustomOption.Create(280, Types.Crewmate, new TranslationInfo(RoleId.SecurityGuard, SecurityGuard.color), rates, null, true);
            securityGuardCooldown = CustomOption.Create(281, Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 1), 30f, 10f, 60f, 2.5f, securityGuardSpawnRate);
            securityGuardTotalScrews = CustomOption.Create(282, Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 2), 7f, 1f, 15f, 1f, securityGuardSpawnRate);
            securityGuardCamPrice = CustomOption.Create(283, Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 3), 2f, 1f, 15f, 1f, securityGuardSpawnRate);
            securityGuardVentPrice = CustomOption.Create(284, Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 4), 1f, 1f, 15f, 1f, securityGuardSpawnRate);
            securityGuardCamDuration = CustomOption.Create(285, Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 5), 10f, 2.5f, 60f, 2.5f, securityGuardSpawnRate);
            securityGuardCamMaxCharges = CustomOption.Create(286, Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 6), 5f, 1f, 30f, 1f, securityGuardSpawnRate);
            securityGuardCamRechargeTasksNumber = CustomOption.Create(287, Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 7), 3f, 1f, 10f, 1f, securityGuardSpawnRate);
            securityGuardNoMove = CustomOption.Create(288, Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 8), true, securityGuardSpawnRate);

            mediumSpawnRate = CustomOption.Create(360, Types.Crewmate, new TranslationInfo(RoleId.Medium, Medium.color), rates, null, true);
            mediumCooldown = CustomOption.Create(361, Types.Crewmate, new TranslationInfo("Opt-Medium", 1), 30f, 5f, 120f, 5f, mediumSpawnRate);
            mediumDuration = CustomOption.Create(362, Types.Crewmate, new TranslationInfo("Opt-Medium", 2), 3f, 0f, 15f, 1f, mediumSpawnRate);
            mediumOneTimeUse = CustomOption.Create(363, Types.Crewmate, new TranslationInfo("Opt-Medium", 3), false, mediumSpawnRate);

            thiefSpawnRate = CustomOption.Create(400, Types.Neutral, new TranslationInfo(RoleId.Thief, Thief.color), rates, null, true);
            thiefCooldown = CustomOption.Create(401, Types.Neutral, new TranslationInfo("Opt-Thief", 1), 30f, 5f, 120f, 5f, thiefSpawnRate);
            thiefCanKillSheriff = CustomOption.Create(402, Types.Neutral, new TranslationInfo("Opt-Thief", 2), true, thiefSpawnRate);
            thiefHasImpVision = CustomOption.Create(403, Types.Neutral, new TranslationInfo("Opt-Thief", 3), true, thiefSpawnRate);
            thiefCanUseVents = CustomOption.Create(404, Types.Neutral, new TranslationInfo("Opt-Thief", 4), true, thiefSpawnRate);

            trapperSpawnRate = CustomOption.Create(410, Types.Crewmate, new TranslationInfo(RoleId.Trapper, Trapper.color), rates, null, true);
            trapperCooldown = CustomOption.Create(420, Types.Crewmate, new TranslationInfo("Opt-Trapper", 1), 30f, 5f, 120f, 5f, trapperSpawnRate);
            trapperMaxCharges = CustomOption.Create(440, Types.Crewmate, new TranslationInfo("Opt-Trapper", 2), 5f, 1f, 15f, 1f, trapperSpawnRate);
            trapperRechargeTasksNumber = CustomOption.Create(450, Types.Crewmate, new TranslationInfo("Opt-Trapper", 3), 2f, 1f, 15f, 1f, trapperSpawnRate);
            trapperTrapNeededTriggerToReveal = CustomOption.Create(451, Types.Crewmate, new TranslationInfo("Opt-Trapper", 4), 3f, 2f, 10f, 1f, trapperSpawnRate);
            trapperAnonymousMap = CustomOption.Create(452, Types.Crewmate, new TranslationInfo("Opt-Trapper", 5), false, trapperSpawnRate);
            trapperInfoType = CustomOption.Create(453, Types.Crewmate, new TranslationInfo("Opt-Trapper", 6), new[] { new TranslationInfo("Opt-Trapper", 100), new TranslationInfo("Opt-Trapper", 101), new TranslationInfo("Opt-Trapper", 102) }, trapperSpawnRate);
            trapperTrapDuration = CustomOption.Create(454, Types.Crewmate, new TranslationInfo("Opt-Trapper", 7), 5f, 1f, 15f, 1f, trapperSpawnRate);

            madmateSpawnRate = CustomOption.Create(910, Types.Crewmate, new TranslationInfo(RoleId.Madmate, Madmate.color), rates, null, true);
            madmateCanDieToSheriff = CustomOption.Create(911, Types.Crewmate, new TranslationInfo("Opt-Madmate", 1), false, madmateSpawnRate);
            madmateCanEnterVents = CustomOption.Create(912, Types.Crewmate, new TranslationInfo("Opt-Madmate", 2), false, madmateSpawnRate);
            madmateHasImpostorVision = CustomOption.Create(913, Types.Crewmate, new TranslationInfo("Opt-Madmate", 3), false, madmateSpawnRate);
            madmateNoticeImpostors = CustomOption.Create(914, Types.Crewmate, new TranslationInfo("Opt-Madmate", 4), false, madmateSpawnRate);
            madmateCommonTasks = CustomOption.Create(915, Types.Crewmate, new TranslationInfo("Opt-Madmate", 5), 0f, 0f, 4f, 1f, madmateNoticeImpostors);
            madmateShortTasks = CustomOption.Create(916, Types.Crewmate, new TranslationInfo("Opt-Madmate", 6), 0f, 0f, 23f, 1f, madmateNoticeImpostors);
            madmateLongTasks = CustomOption.Create(917, Types.Crewmate, new TranslationInfo("Opt-Madmate", 7), 0f, 0f, 15f, 1f, madmateNoticeImpostors);
            madmateExileCrewmate = CustomOption.Create(918, Types.Crewmate, new TranslationInfo("Opt-Madmate", 8), false, madmateSpawnRate);

            // Modifier (1000 - 1999)
            modifiersAreHidden = CustomOption.Create(1009, Types.Modifier, new TranslationInfo("Opt-General", 56, Color.yellow), true, null, true);

            modifierBloody = CustomOption.Create(1000, Types.Modifier, new TranslationInfo(RoleId.Bloody, Color.yellow), rates, null, true);
            modifierBloodyQuantity = CustomOption.Create(1001, Types.Modifier, new TranslationInfo("Opt-Bloody", 1, Color.yellow), ratesModifier, modifierBloody);
            modifierBloodyDuration = CustomOption.Create(1002, Types.Modifier, new TranslationInfo("Opt-Bloody", 2), 10f, 3f, 60f, 1f, modifierBloody);

            modifierAntiTeleport = CustomOption.Create(1010, Types.Modifier, new TranslationInfo(RoleId.AntiTeleport, Color.yellow), rates, null, true);
            modifierAntiTeleportQuantity = CustomOption.Create(1011, Types.Modifier, new TranslationInfo("Opt-AntiTeleport", 1, Color.yellow), ratesModifier, modifierAntiTeleport);

            modifierTieBreaker = CustomOption.Create(1020, Types.Modifier, new TranslationInfo(RoleId.Tiebreaker, Color.yellow), rates, null, true);

            modifierBait = CustomOption.Create(1030, Types.Modifier, new TranslationInfo(RoleId.Bait, Color.yellow), rates, null, true);
            modifierBaitQuantity = CustomOption.Create(1031, Types.Modifier, new TranslationInfo("Opt-Bait", 1, Color.yellow), ratesModifier, modifierBait);
            modifierBaitReportDelayMin = CustomOption.Create(1032, Types.Modifier, new TranslationInfo("Opt-Bait", 2), 0f, 0f, 10f, 1f, modifierBait);
            modifierBaitReportDelayMax = CustomOption.Create(1033, Types.Modifier, new TranslationInfo("Opt-Bait", 3), 0f, 0f, 10f, 1f, modifierBait);
            modifierBaitShowKillFlash = CustomOption.Create(1034, Types.Modifier, new TranslationInfo("Opt-Bait", 4), true, modifierBait);

            modifierLover = CustomOption.Create(1040, Types.Modifier, new TranslationInfo(RoleId.Lover, Color.yellow), rates, null, true);
            modifierLoverImpLoverRate = CustomOption.Create(1041, Types.Modifier, new TranslationInfo("Opt-Lovers", 1), rates, modifierLover);
            modifierLoverBothDie = CustomOption.Create(1042, Types.Modifier, new TranslationInfo("Opt-Lovers", 2), true, modifierLover);
            modifierLoverEnableChat = CustomOption.Create(1043, Types.Modifier, new TranslationInfo("Opt-Lovers", 3), true, modifierLover);

            modifierSunglasses = CustomOption.Create(1050, Types.Modifier, new TranslationInfo(RoleId.Sunglasses, Color.yellow), rates, null, true);
            modifierSunglassesQuantity = CustomOption.Create(1051, Types.Modifier, new TranslationInfo("Opt-Sunglasses", 1, Color.yellow), ratesModifier, modifierSunglasses);
            modifierSunglassesVision = CustomOption.Create(1052, Types.Modifier, new TranslationInfo("Opt-Sunglasses", 2), new[] { new TranslationInfo("-10%"), new TranslationInfo("-20%"), new TranslationInfo("-30%"), new TranslationInfo("-40%"), new TranslationInfo("-50%") }, modifierSunglasses);

            modifierMini = CustomOption.Create(1061, Types.Modifier, new TranslationInfo(RoleId.Mini, Color.yellow), rates, null, true);
            modifierMiniGrowingUpDuration = CustomOption.Create(1062, Types.Modifier, new TranslationInfo("Opt-Mini", 1), 400f, 100f, 1500f, 100f, modifierMini);
            modifierMiniGrowingUpInMeeting = CustomOption.Create(1063, Types.Modifier, new TranslationInfo("Opt-Mini", 2), true, modifierMini);

            modifierVip = CustomOption.Create(1070, Types.Modifier, new TranslationInfo(RoleId.Vip, Color.yellow), rates, null, true);
            modifierVipQuantity = CustomOption.Create(1071, Types.Modifier, new TranslationInfo("Opt-Vip", 1, Color.yellow), ratesModifier, modifierVip);
            modifierVipShowColor = CustomOption.Create(1072, Types.Modifier, new TranslationInfo("Opt-Vip", 2), true, modifierVip);

            modifierInvert = CustomOption.Create(1080, Types.Modifier, new TranslationInfo(RoleId.Invert, Color.yellow), rates, null, true);
            modifierInvertQuantity = CustomOption.Create(1081, Types.Modifier, new TranslationInfo("Opt-Invert", 1, Color.yellow), ratesModifier, modifierInvert);
            modifierInvertDuration = CustomOption.Create(1082, Types.Modifier, new TranslationInfo("Opt-Invert", 2), 3f, 1f, 15f, 1f, modifierInvert);

            modifierChameleon = CustomOption.Create(1090, Types.Modifier, new TranslationInfo(RoleId.Chameleon, Color.yellow), rates, null, true);
            modifierChameleonQuantity = CustomOption.Create(1091, Types.Modifier, new TranslationInfo("Opt-Chameleon", 1, Color.yellow), ratesModifier, modifierChameleon);
            modifierChameleonHoldDuration = CustomOption.Create(1092, Types.Modifier, new TranslationInfo("Opt-Chameleon", 2), 3f, 1f, 10f, 0.5f, modifierChameleon);
            modifierChameleonFadeDuration = CustomOption.Create(1093, Types.Modifier, new TranslationInfo("Opt-Chameleon", 3), 1f, 0.25f, 10f, 0.25f, modifierChameleon);
            modifierChameleonMinVisibility = CustomOption.Create(1094, Types.Modifier, new TranslationInfo("Opt-Chameleon", 4), new[] { new TranslationInfo("0%"), new TranslationInfo("10%"), new TranslationInfo("20%"), new TranslationInfo("30%"), new TranslationInfo("40%"), new TranslationInfo("50%") }, modifierChameleon);

            modifierShifter = CustomOption.Create(1100, Types.Modifier, new TranslationInfo(RoleId.Shifter, Color.yellow), rates, null, true);

            // Guesser Gamemode (2000 - 2999)
            guesserGamemodeCrewNumber = CustomOption.Create(2001, Types.Guesser, new TranslationInfo("Opt-Guessers-General", 1, Guesser.color), 15f, 1f, 15f, 1f, null, true);
            guesserGamemodeNeutralNumber = CustomOption.Create(2002, Types.Guesser, new TranslationInfo("Opt-Guessers-General", 2, Guesser.color), 15f, 1f, 15f, 1f, null, true);
            guesserGamemodeImpNumber = CustomOption.Create(2003, Types.Guesser, new TranslationInfo("Opt-Guessers-General", 3, Guesser.color), 15f, 1f, 15f, 1f, null, true);
            guesserForceJackalGuesser = CustomOption.Create(2007, Types.Guesser, new TranslationInfo("Opt-Guessers-General", 4), false, null, true);
            guesserGamemodeHaveModifier = CustomOption.Create(2004, Types.Guesser, new TranslationInfo("Opt-Guessers-General", 5), true, null);
            guesserGamemodeNumberOfShots = CustomOption.Create(2005, Types.Guesser, new TranslationInfo("Opt-Guessers-General", 6), 3f, 1f, 15f, 1f, null);
            guesserGamemodeHasMultipleShotsPerMeeting = CustomOption.Create(2006, Types.Guesser, new TranslationInfo("Opt-Guessers-General", 7), false, null);
            guesserGamemodeKillsThroughShield = CustomOption.Create(2008, Types.Guesser, new TranslationInfo("Opt-Guessers-General", 8), true, null);
            guesserGamemodeEvilCanKillSpy = CustomOption.Create(2009, Types.Guesser, new TranslationInfo("Opt-Guessers-General", 9), true, null);
            guesserGamemodeCantGuessSnitchIfTaksDone = CustomOption.Create(2010, Types.Guesser, new TranslationInfo("Opt-Guessers-General", 10), true, null);

            // Hide N Seek Gamemode (3000 - 3999)
            hideNSeekMap = CustomOption.Create(3020, Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 1, Color.yellow), new[] { new TranslationInfo("Opt-General", 64), new TranslationInfo("Opt-General", 65), new TranslationInfo("Opt-General", 66), new TranslationInfo("Opt-General", 67), new TranslationInfo("Opt-General", 68) }, null, true);
            hideNSeekHunterCount = CustomOption.Create(3000, Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 2, Color.yellow), 1f, 1f, 3f, 1f);
            hideNSeekKillCooldown = CustomOption.Create(3021, Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 3, Color.yellow), 10f, 2.5f, 60f, 2.5f);
            hideNSeekHunterVision = CustomOption.Create(3001, Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 4, Color.yellow), 0.5f, 0.25f, 2f, 0.25f);
            hideNSeekHuntedVision = CustomOption.Create(3002, Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 5, Color.yellow), 2f, 0.25f, 5f, 0.25f);
            hideNSeekCommonTasks = CustomOption.Create(3023, Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 6, Color.yellow), 1f, 0f, 4f, 1f);
            hideNSeekShortTasks = CustomOption.Create(3024, Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 7, Color.yellow), 3f, 1f, 23f, 1f);
            hideNSeekLongTasks = CustomOption.Create(3025, Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 8, Color.yellow), 3f, 0f, 15f, 1f);
            hideNSeekTimer = CustomOption.Create(3003, Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 9, Color.yellow), 5f, 1f, 30f, 1f);
            hideNSeekTaskWin = CustomOption.Create(3004, Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 10, Color.yellow), false);
            hideNSeekTaskPunish = CustomOption.Create(3017, Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 11, Color.yellow), 10f, 0f, 30f, 1f);
            hideNSeekCanSabotage = CustomOption.Create(3019, Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 12, Color.yellow), false);
            hideNSeekHunterWaiting = CustomOption.Create(3022, Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 13, Color.yellow), 15f, 2.5f, 60f, 2.5f);

            hunterLightCooldown = CustomOption.Create(3005, Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 1, Color.red), 30f, 5f, 60f, 1f, null, true);
            hunterLightDuration = CustomOption.Create(3006, Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 2, Color.red), 5f, 1f, 60f, 1f);
            hunterLightVision = CustomOption.Create(3007, Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 3, Color.red), 3f, 1f, 5f, 0.25f);
            hunterLightPunish = CustomOption.Create(3008, Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 4, Color.red), 5f, 0f, 30f, 1f);
            hunterAdminCooldown = CustomOption.Create(3009, Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 5, Color.red), 30f, 5f, 60f, 1f);
            hunterAdminDuration = CustomOption.Create(3010, Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 6, Color.red), 5f, 1f, 60f, 1f);
            hunterAdminPunish = CustomOption.Create(3011, Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 7, Color.red), 5f, 0f, 30f, 1f);
            hunterArrowCooldown = CustomOption.Create(3012, Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 8, Color.red), 30f, 5f, 60f, 1f);
            hunterArrowDuration = CustomOption.Create(3013, Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 9, Color.red), 5f, 0f, 60f, 1f);
            hunterArrowPunish = CustomOption.Create(3014, Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 10, Color.red), 5f, 0f, 30f, 1f);
            huntedShieldCooldown = CustomOption.Create(3015, Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 11, Color.gray), 30f, 5f, 60f, 1f, null, true);
            huntedShieldDuration = CustomOption.Create(3016, Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 12, Color.gray), 5f, 1f, 60f, 1f);
            huntedShieldRewindTime = CustomOption.Create(3018, Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 13, Color.gray), 3f, 1f, 10f, 1f);
            huntedShieldNumber = CustomOption.Create(3026, Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 14, Color.gray), 3f, 1f, 15f, 1f);

            // Other options
            maxNumberOfMeetings = CustomOption.Create(3, Types.General, new TranslationInfo("Opt-General", 57), 10, 0, 15, 1, null, true);
            blockSkippingInEmergencyMeetings = CustomOption.Create(4, Types.General, new TranslationInfo("Opt-General", 58), false);
            noVoteIsSelfVote = CustomOption.Create(5, Types.General, new TranslationInfo("Opt-General", 59), false, blockSkippingInEmergencyMeetings);
            hidePlayerNames = CustomOption.Create(6, Types.General, new TranslationInfo("Opt-General", 60), false);
            allowParallelMedBayScans = CustomOption.Create(7, Types.General, new TranslationInfo("Opt-General", 61), false);
            shieldFirstKill = CustomOption.Create(8, Types.General, new TranslationInfo("Opt-General", 62), false);
            dynamicMap = CustomOption.Create(500, Types.General, new TranslationInfo("Opt-General", 63), false, null, false);
            dynamicMapEnableSkeld = CustomOption.Create(501, Types.General, new TranslationInfo("Opt-General", 64), rates, dynamicMap, false);
            dynamicMapEnableMira = CustomOption.Create(502, Types.General, new TranslationInfo("Opt-General", 65), rates, dynamicMap, false);
            dynamicMapEnablePolus = CustomOption.Create(503, Types.General, new TranslationInfo("Opt-General", 66), rates, dynamicMap, false);
            dynamicMapEnableAirShip = CustomOption.Create(504, Types.General, new TranslationInfo("Opt-General", 67), rates, dynamicMap, false);
            dynamicMapEnableSubmerged = CustomOption.Create(505, Types.General, new TranslationInfo("Opt-General", 68), rates, dynamicMap, false);

            blockedRolePairings.Add((byte)RoleId.Vampire, new [] { (byte)RoleId.Warlock});
            blockedRolePairings.Add((byte)RoleId.Warlock, new [] { (byte)RoleId.Vampire});
            blockedRolePairings.Add((byte)RoleId.Spy, new [] { (byte)RoleId.Mini});
            blockedRolePairings.Add((byte)RoleId.Mini, new [] { (byte)RoleId.Spy});
            blockedRolePairings.Add((byte)RoleId.Vulture, new [] { (byte)RoleId.Cleaner});
            blockedRolePairings.Add((byte)RoleId.Cleaner, new [] { (byte)RoleId.Vulture});
        }
    }
}
