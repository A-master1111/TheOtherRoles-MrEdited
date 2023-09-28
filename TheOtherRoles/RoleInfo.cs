using System.Linq;
using System;
using System.Collections.Generic;
using TheOtherRoles.Players;
using static TheOtherRoles.TheOtherRoles;
using UnityEngine;
using TheOtherRoles.Utilities;


namespace TheOtherRoles
{
    class RoleInfo {
        public Color color { get; private set; }
        public RoleId roleId { get; private set; }
        public bool isNeutral { get; private set; }
        public bool isModifier { get; private set; }
        public string name { get { return name_ != null ? name_.GetString() : ""; } }
        public string introDescription { get { return introDescription_ != null ? introDescription_.GetString() : ""; } }
        public string shortDescription { get { return shortDescription_ != null ? shortDescription_.GetString() : ""; } }

        RoleInfo(Color color, RoleId roleId, bool isNeutral = false, bool isModifier = false, TranslationInfo name = null, TranslationInfo introDescription = null, TranslationInfo shortDescription = null) {
            this.color = color;
            this.name_ = name != null ? name : ModTranslation.GetRoleName(roleId, color);
            this.introDescription_ = introDescription != null ? introDescription : ModTranslation.GetRoleIntroDesc(roleId, color);
            this.shortDescription_ = shortDescription != null ? shortDescription : ModTranslation.GetRoleShortDesc(roleId, color);
            this.roleId = roleId;
            this.isNeutral = isNeutral;
            this.isModifier = isModifier;
        }

        TranslationInfo name_ = null;
        TranslationInfo introDescription_ = null;
        TranslationInfo shortDescription_ = null;

        public static RoleInfo jester = new RoleInfo(Jester.color, RoleId.Jester, true);
        public static RoleInfo mayor = new RoleInfo(Mayor.color, RoleId.Mayor);
        public static RoleInfo portalmaker = new RoleInfo(Portalmaker.color, RoleId.Portalmaker);
        public static RoleInfo engineer = new RoleInfo(Engineer.color, RoleId.Engineer);
        public static RoleInfo sheriff = new RoleInfo(Sheriff.color, RoleId.Sheriff);
        public static RoleInfo deputy = new RoleInfo(Sheriff.color, RoleId.Deputy);
        public static RoleInfo lighter = new RoleInfo(Lighter.color, RoleId.Lighter);
        public static RoleInfo godfather = new RoleInfo(Godfather.color, RoleId.Godfather);
        public static RoleInfo mafioso = new RoleInfo(Mafioso.color, RoleId.Mafioso);
        public static RoleInfo janitor = new RoleInfo(Janitor.color, RoleId.Janitor);
        public static RoleInfo morphling = new RoleInfo(Morphling.color, RoleId.Morphling);
        public static RoleInfo camouflager = new RoleInfo(Camouflager.color, RoleId.Camouflager);
        public static RoleInfo evilHacker = new RoleInfo(EvilHacker.color, RoleId.EvilHacker);
        public static RoleInfo vampire = new RoleInfo(Vampire.color, RoleId.Vampire);
        public static RoleInfo eraser = new RoleInfo(Eraser.color, RoleId.Eraser);
        public static RoleInfo trickster = new RoleInfo(Trickster.color, RoleId.Trickster);
        public static RoleInfo cleaner = new RoleInfo(Cleaner.color, RoleId.Cleaner);
        public static RoleInfo warlock = new RoleInfo(Warlock.color, RoleId.Warlock);
        public static RoleInfo bountyHunter = new RoleInfo(BountyHunter.color, RoleId.BountyHunter);
        public static RoleInfo detective = new RoleInfo(Detective.color, RoleId.Detective);
        public static RoleInfo timeMaster = new RoleInfo(TimeMaster.color, RoleId.TimeMaster);
        public static RoleInfo medic = new RoleInfo(Medic.color, RoleId.Medic);
        public static RoleInfo swapper = new RoleInfo(Swapper.color, RoleId.Swapper);
        public static RoleInfo seer = new RoleInfo(Seer.color, RoleId.Seer);
        public static RoleInfo hacker = new RoleInfo(Hacker.color, RoleId.Hacker);
        public static RoleInfo tracker = new RoleInfo(Tracker.color, RoleId.Tracker);
        public static RoleInfo snitch = new RoleInfo(Snitch.color, RoleId.Snitch);
        public static RoleInfo jackal = new RoleInfo(Jackal.color, RoleId.Jackal, true);
        public static RoleInfo sidekick = new RoleInfo(Sidekick.color, RoleId.Sidekick, true);
        public static RoleInfo spy = new RoleInfo(Spy.color, RoleId.Spy);
        public static RoleInfo securityGuard = new RoleInfo(SecurityGuard.color, RoleId.SecurityGuard);
        public static RoleInfo arsonist = new RoleInfo(Arsonist.color, RoleId.Arsonist, true);
        public static RoleInfo goodGuesser = new RoleInfo(Guesser.color, RoleId.NiceGuesser);
        public static RoleInfo badGuesser = new RoleInfo(Palette.ImpostorRed, RoleId.EvilGuesser);
        public static RoleInfo vulture = new RoleInfo(Vulture.color, RoleId.Vulture, true);
        public static RoleInfo medium = new RoleInfo(Medium.color, RoleId.Medium);
        public static RoleInfo madmate = new RoleInfo(Madmate.color, RoleId.Madmate);
        public static RoleInfo trapper = new RoleInfo(Trapper.color, RoleId.Trapper);
        public static RoleInfo lawyer = new RoleInfo(Lawyer.color, RoleId.Lawyer, true);
        public static RoleInfo prosecutor = new RoleInfo(Lawyer.color, RoleId.Prosecutor, true);
        public static RoleInfo pursuer = new RoleInfo(Pursuer.color, RoleId.Pursuer);
        public static RoleInfo impostor = new RoleInfo(Palette.ImpostorRed, RoleId.Impostor);
        public static RoleInfo crewmate = new RoleInfo(Color.white, RoleId.Crewmate);
        public static RoleInfo witch = new RoleInfo(Witch.color, RoleId.Witch);
        public static RoleInfo ninja = new RoleInfo(Ninja.color, RoleId.Ninja);
        public static RoleInfo thief = new RoleInfo(Thief.color, RoleId.Thief, true);

        public static RoleInfo hunter = new RoleInfo(Palette.ImpostorRed, RoleId.Impostor, false, false, ModTranslation.GetRoleName(RoleId.Hunter, Palette.ImpostorRed), ModTranslation.GetRoleIntroDesc(RoleId.Hunter, Palette.ImpostorRed), ModTranslation.GetRoleShortDesc(RoleId.Hunter, Palette.ImpostorRed));
        public static RoleInfo hunted = new RoleInfo(Color.white, RoleId.Crewmate, false, false, ModTranslation.GetRoleName(RoleId.Hunted, Color.white), ModTranslation.GetRoleIntroDesc(RoleId.Hunted, Color.white), ModTranslation.GetRoleShortDesc(RoleId.Hunted, Color.white));

        public static RoleInfo yasuna = new RoleInfo(Yasuna.color, RoleId.Yasuna);
        public static RoleInfo yasunaJr = new RoleInfo(YasunaJr.color, RoleId.YasunaJr);
        public static RoleInfo evilYasuna = new RoleInfo(Palette.ImpostorRed, RoleId.EvilYasuna);
        public static RoleInfo taskMaster = new RoleInfo(TaskMaster.color, RoleId.TaskMaster);
        public static RoleInfo doorHacker = new RoleInfo(DoorHacker.color, RoleId.DoorHacker);
        public static RoleInfo kataomoi = new RoleInfo(Kataomoi.color, RoleId.Kataomoi, true);
        public static RoleInfo killerCreator = new RoleInfo(KillerCreator.color, RoleId.KillerCreator);
        public static RoleInfo madmateKiller = new RoleInfo(MadmateKiller.color, RoleId.MadmateKiller);

        // Task Vs Mode
        public static RoleInfo taskRacer = new RoleInfo(TaskRacer.color, RoleId.TaskRacer);

        // Modifier
        public static RoleInfo bloody = new RoleInfo(Color.yellow, RoleId.Bloody, false, true);
        public static RoleInfo antiTeleport = new RoleInfo(Color.yellow, RoleId.AntiTeleport, false, true);
        public static RoleInfo tiebreaker = new RoleInfo(Color.yellow, RoleId.Tiebreaker, false, true);
        public static RoleInfo bait = new RoleInfo(Color.yellow, RoleId.Bait, false, true);
        public static RoleInfo sunglasses = new RoleInfo(Color.yellow, RoleId.Sunglasses, false, true);
        public static RoleInfo lover = new RoleInfo(Lovers.color, RoleId.Lover, false, true);
        public static RoleInfo mini = new RoleInfo(Color.yellow, RoleId.Mini, false, true);
        public static RoleInfo vip = new RoleInfo(Color.yellow, RoleId.Vip, false, true);
        public static RoleInfo invert = new RoleInfo(Color.yellow, RoleId.Invert, false, true);
        public static RoleInfo chameleon = new RoleInfo(Color.yellow, RoleId.Chameleon, false, true);
        public static RoleInfo shifter = new RoleInfo(Color.yellow, RoleId.Shifter, false, true);


        public static List<RoleInfo> allRoleInfos = new List<RoleInfo>() {
            impostor,
            godfather,
            mafioso,
            janitor,
            morphling,
            camouflager,
            evilHacker,
            vampire,
            eraser,
            trickster,
            cleaner,
            warlock,
            bountyHunter,
            witch,
            ninja,
            goodGuesser,
            badGuesser,
            lover,
            jester,
            arsonist,
            jackal,
            sidekick,
            vulture,
            pursuer,
            lawyer,
            thief,
            prosecutor,
            crewmate,
            mayor,
            portalmaker,
            engineer,
            sheriff,
            deputy,
            lighter,
            detective,
            timeMaster,
            medic,
            swapper,
            seer,
            hacker,
            tracker,
            snitch,
            spy,
            securityGuard,
            bait,
            medium,
            trapper,
            madmate,
            bloody,
            antiTeleport,
            tiebreaker,
            sunglasses,
            mini,
            vip,
            invert,
            chameleon,
            shifter,
            yasuna,
            yasunaJr,
            evilYasuna,
            taskMaster,
            doorHacker,
            kataomoi,
            killerCreator,
            madmateKiller,

            // Task Vs Mode
            taskRacer,
        };

        public static List<RoleInfo> getRoleInfoForPlayer(PlayerControl p, bool showModifier = true) {
            List<RoleInfo> infos = new List<RoleInfo>();
            if (p == null) return infos;

            // Modifier
            if (showModifier) {
                // after dead modifier
                if (!CustomOptionHolder.modifiersAreHidden.getBool() || PlayerControl.LocalPlayer.Data.IsDead || AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Ended)
                {
                    if (Bait.bait.Any(x => x.PlayerId == p.PlayerId)) infos.Add(bait);
                    if (Bloody.bloody.Any(x => x.PlayerId == p.PlayerId)) infos.Add(bloody);
                    if (Vip.vip.Any(x => x.PlayerId == p.PlayerId)) infos.Add(vip);
                }
                if (p == Lovers.lover1 || p == Lovers.lover2) infos.Add(lover);
                if (p == Tiebreaker.tiebreaker) infos.Add(tiebreaker);
                if (AntiTeleport.antiTeleport.Any(x => x.PlayerId == p.PlayerId)) infos.Add(antiTeleport);
                if (Sunglasses.sunglasses.Any(x => x.PlayerId == p.PlayerId)) infos.Add(sunglasses);
                if (p == Mini.mini) infos.Add(mini);
                if (Invert.invert.Any(x => x.PlayerId == p.PlayerId)) infos.Add(invert);
                if (Chameleon.chameleon.Any(x => x.PlayerId == p.PlayerId)) infos.Add(chameleon);
                if (p == Shifter.shifter) infos.Add(shifter);
            }

            int count = infos.Count;  // Save count after modifiers are added so that the role count can be checked

            // Special roles
            if (p == Jester.jester) infos.Add(jester);
            if (p == Mayor.mayor) infos.Add(mayor);
            if (p == Portalmaker.portalmaker) infos.Add(portalmaker);
            if (p == Engineer.engineer) infos.Add(engineer);
            if (p == Sheriff.sheriff || p == Sheriff.formerSheriff) infos.Add(sheriff);
            if (p == Deputy.deputy) infos.Add(deputy);
            if (p == Lighter.lighter) infos.Add(lighter);
            if (p == Godfather.godfather) infos.Add(godfather);
            if (p == Mafioso.mafioso) infos.Add(mafioso);
            if (p == Janitor.janitor) infos.Add(janitor);
            if (p == Morphling.morphling) infos.Add(morphling);
            if (p == Camouflager.camouflager) infos.Add(camouflager);
            if (p == EvilHacker.evilHacker) infos.Add(evilHacker);
            if (p == Vampire.vampire) infos.Add(vampire);
            if (p == Eraser.eraser) infos.Add(eraser);
            if (p == Trickster.trickster) infos.Add(trickster);
            if (p == Cleaner.cleaner) infos.Add(cleaner);
            if (p == Warlock.warlock) infos.Add(warlock);
            if (p == Witch.witch) infos.Add(witch);
            if (p == Ninja.ninja) infos.Add(ninja);
            if (p == Detective.detective) infos.Add(detective);
            if (p == TimeMaster.timeMaster) infos.Add(timeMaster);
            if (p == Medic.medic) infos.Add(medic);
            if (p == Swapper.swapper) infos.Add(swapper);
            if (p == Seer.seer) infos.Add(seer);
            if (p == Hacker.hacker) infos.Add(hacker);
            if (p == Tracker.tracker) infos.Add(tracker);
            if (p == Snitch.snitch) infos.Add(snitch);
            if (p == Jackal.jackal || (Jackal.formerJackals != null && Jackal.formerJackals.Any(x => x.PlayerId == p.PlayerId))) infos.Add(jackal);
            if (p == Sidekick.sidekick) infos.Add(sidekick);
            if (p == Spy.spy) infos.Add(spy);
            if (p == SecurityGuard.securityGuard) infos.Add(securityGuard);
            if (p == Arsonist.arsonist) infos.Add(arsonist);
            if (p == Guesser.niceGuesser) infos.Add(goodGuesser);
            if (p == Guesser.evilGuesser) infos.Add(badGuesser);
            if (p == BountyHunter.bountyHunter) infos.Add(bountyHunter);
            if (p == Vulture.vulture) infos.Add(vulture);
            if (p == Medium.medium) infos.Add(medium);
            if (p == Madmate.madmate) infos.Add(madmate);
            if (p == Lawyer.lawyer && !Lawyer.isProsecutor) infos.Add(lawyer);
            if (p == Lawyer.lawyer && Lawyer.isProsecutor) infos.Add(prosecutor);
            if (p == Trapper.trapper) infos.Add(trapper);
            if (p == Pursuer.pursuer) infos.Add(pursuer);
            if (p == Thief.thief) infos.Add(thief);
            if (p == Yasuna.yasuna) infos.Add(p.Data.Role.IsImpostor ? evilYasuna : yasuna);
            if (p == YasunaJr.yasunaJr) infos.Add(yasunaJr);
            if (p == TaskMaster.taskMaster) infos.Add(taskMaster);
            if (p == DoorHacker.doorHacker) infos.Add(doorHacker);
            if (p == Kataomoi.kataomoi) infos.Add(kataomoi);
            if (p == KillerCreator.killerCreator) infos.Add(killerCreator);
            if (p == MadmateKiller.madmateKiller) infos.Add(madmateKiller);

            // Task Vs Mode
            if (TaskRacer.isTaskRacer(p))
                infos.Add(taskRacer);

            // Default roles (just impostor, just crewmate, or hunter / hunted for hide n seek
            if (infos.Count == count)
            {
                if (p.Data.Role.IsImpostor)
                    infos.Add(MapOptions.gameMode == CustomGamemodes.HideNSeek ? RoleInfo.hunter : RoleInfo.impostor);
                else
                    infos.Add(MapOptions.gameMode == CustomGamemodes.HideNSeek ? RoleInfo.hunted : RoleInfo.crewmate);
            }

            return infos;
        }

        public static String GetRolesString(PlayerControl p, bool useColors, bool showModifier, bool isDead) {
            string roleName;

            // Task Vs Mode
            if (TaskRacer.isValid()) {
                roleName = TaskRacer.getRankText(TaskRacer.getRank(p), useColors);
            } else {
                var roleList = getRoleInfoForPlayer(p, showModifier);
                if (roleList.Count > 0 && roleList[0].roleId == RoleId.TaskMaster && !isDead && TaskMaster.becomeATaskMasterWhenCompleteAllTasks && !TaskMaster.isTaskComplete)
                    roleList[0] = RoleInfo.crewmate;

                roleName = String.Join(" ", roleList.Select(x => useColors ? Helpers.cs(x.color, x.name) : x.name).ToArray());
                if (Lawyer.target != null && p.PlayerId == Lawyer.target.PlayerId && CachedPlayer.LocalPlayer.PlayerControl != Lawyer.target) roleName += (useColors ? Helpers.cs(Pursuer.color, " §") : " §");
                if (HandleGuesser.isGuesserGm && HandleGuesser.isGuesser(p.PlayerId)) roleName += ModTranslation.GetString("Game-Guesser", 1);
            }
            return roleName;
        }
    }
}
