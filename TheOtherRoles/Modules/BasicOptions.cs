using System.Collections.Generic;
using BepInEx.Configuration;
using System;
using System.IO;

namespace TheOtherRoles
{
    public static class BasicOptions
    {
        public static void Save(Dictionary<int, string> optionTable, StreamWriter sw) {
            var optionData = AmongUsClient.Instance.AmHost ? GameOptionsData.hostOptionsData : GameOptionsData.GameHostOptions;

            // Generic options
            Save(mapId.id, optionData.MapId, optionTable, sw);
            Save(playerSpeedMod.id, optionData.PlayerSpeedMod, optionTable, sw);
            Save(crewLightMod.id, optionData.CrewLightMod, optionTable, sw);
            Save(impostorLightMod.id, optionData.ImpostorLightMod, optionTable, sw);
            Save(killCooldown.id, optionData.KillCooldown, optionTable, sw);
            Save(numCommonTasks.id, optionData.NumCommonTasks, optionTable, sw);
            Save(numLongTasks.id, optionData.NumLongTasks, optionTable, sw);
            Save(numShortTasks.id, optionData.numShortTasks, optionTable, sw);
            Save(numEmergencyMeetings.id, optionData.NumEmergencyMeetings, optionTable, sw);
            Save(emergencyCooldown.id, optionData.EmergencyCooldown, optionTable, sw);
            Save(numImpostors.id, optionData.NumImpostors, optionTable, sw);
            Save(ghostsDoTasks.id, optionData.GhostsDoTasks, optionTable, sw);
            Save(killDistance.id, optionData.KillDistance, optionTable, sw);
            Save(discussionTime.id, optionData.DiscussionTime, optionTable, sw);
            Save(votingTime.id, optionData.VotingTime, optionTable, sw);
            Save(confirmImpostor.id, optionData.ConfirmImpostor, optionTable, sw);
            Save(visualTasks.id, optionData.VisualTasks, optionTable, sw);
            Save(anonymousVotes.id, optionData.AnonymousVotes, optionTable, sw);
            Save(taskBarMode.id, optionData.TaskBarMode, optionTable, sw);
            Save(isDefaults.id, optionData.isDefaults, optionTable, sw);

            // Role options
            Save(shapeshifterLeaveSkin.id, optionData.RoleOptions.ShapeshifterLeaveSkin, optionTable, sw);
            Save(shapeshifterCooldown.id, optionData.RoleOptions.ShapeshifterCooldown, optionTable, sw);
            Save(shapeshifterDuration.id, optionData.RoleOptions.ShapeshifterDuration, optionTable, sw);
            Save(scientistCooldown.id, optionData.RoleOptions.ScientistCooldown, optionTable, sw);
            Save(scientistBatteryCharge.id, optionData.RoleOptions.ScientistBatteryCharge, optionTable, sw);
            Save(guardianAngelCooldown.id, optionData.RoleOptions.GuardianAngelCooldown, optionTable, sw);
            Save(impostorsCanSeeProtect.id, optionData.RoleOptions.ImpostorsCanSeeProtect, optionTable, sw);
            Save(protectionDurationSeconds.id, optionData.RoleOptions.ProtectionDurationSeconds, optionTable, sw);
            Save(engineerCooldown.id, optionData.RoleOptions.EngineerCooldown, optionTable, sw);
            Save(engineerInVentMaxTime.id, optionData.RoleOptions.EngineerInVentMaxTime, optionTable, sw);

            if (optionData.RoleOptions.roleRates.ContainsKey(RoleTypes.Scientist))
            {
                var roleRate = optionData.RoleOptions.roleRates[RoleTypes.Scientist];
                Save(scientistMaxCount.id, roleRate.MaxCount, optionTable, sw);
                Save(scientistChance.id, roleRate.Chance, optionTable, sw);
            }
            if (optionData.RoleOptions.roleRates.ContainsKey(RoleTypes.Engineer))
            {
                var roleRate = optionData.RoleOptions.roleRates[RoleTypes.Engineer];
                Save(engineerMaxCount.id, roleRate.MaxCount, optionTable, sw);
                Save(engineerChance.id, roleRate.Chance, optionTable, sw);
            }
            if (optionData.RoleOptions.roleRates.ContainsKey(RoleTypes.GuardianAngel))
            {
                var roleRate = optionData.RoleOptions.roleRates[RoleTypes.GuardianAngel];
                Save(guardianAngelMaxCount.id, roleRate.MaxCount, optionTable, sw);
                Save(guardianAngelChance.id, roleRate.Chance, optionTable, sw);
            }
            if (optionData.RoleOptions.roleRates.ContainsKey(RoleTypes.Shapeshifter))
            {
                var roleRate = optionData.RoleOptions.roleRates[RoleTypes.Shapeshifter];
                Save(shapeshifterMaxCount.id, roleRate.MaxCount, optionTable, sw);
                Save(shapeshifterChance.id, roleRate.Chance, optionTable, sw);
            }
        }

        public static void Load(Dictionary<int, string> optionTable) {
            if (optionTable == null) return;

            // Generic options
            if (optionTable.TryGetValue(mapId.id, out string s) && byte.TryParse(s, out var mapId_))
                GameOptionsData.hostOptionsData.MapId = mapId_;
            if (optionTable.TryGetValue(playerSpeedMod.id, out s) && float.TryParse(s, out var playerSpeedMod_))
                GameOptionsData.hostOptionsData.PlayerSpeedMod = playerSpeedMod_;
            if (optionTable.TryGetValue(crewLightMod.id, out s) && float.TryParse(s, out var crewLightMod_))
                GameOptionsData.hostOptionsData.CrewLightMod = crewLightMod_;
            if (optionTable.TryGetValue(impostorLightMod.id, out s) && float.TryParse(s, out var impostorLightMod_))
                GameOptionsData.hostOptionsData.ImpostorLightMod = impostorLightMod_;
            if (optionTable.TryGetValue(killCooldown.id, out s) && float.TryParse(s, out var killCooldown_))
                GameOptionsData.hostOptionsData.KillCooldown = killCooldown_;
            if (optionTable.TryGetValue(numCommonTasks.id, out s) && int.TryParse(s, out var numCommonTasks_))
                GameOptionsData.hostOptionsData.NumCommonTasks = numCommonTasks_;
            if (optionTable.TryGetValue(numLongTasks.id, out s) && int.TryParse(s, out var numLongTasks_))
                GameOptionsData.hostOptionsData.NumLongTasks = numLongTasks_;
            if (optionTable.TryGetValue(numShortTasks.id, out s) && int.TryParse(s, out var numShortTasks_))
                GameOptionsData.hostOptionsData.NumShortTasks = numShortTasks_;
            if (optionTable.TryGetValue(numEmergencyMeetings.id, out s) && int.TryParse(s, out var numEmergencyMeetings_))
                GameOptionsData.hostOptionsData.NumEmergencyMeetings = numEmergencyMeetings_;
            if (optionTable.TryGetValue(emergencyCooldown.id, out s) && int.TryParse(s, out var emergencyCooldown_))
                GameOptionsData.hostOptionsData.EmergencyCooldown = emergencyCooldown_;
            if (optionTable.TryGetValue(numImpostors.id, out s) && int.TryParse(s, out var numImpostors_))
                GameOptionsData.hostOptionsData.NumImpostors = numImpostors_;
            if (optionTable.TryGetValue(ghostsDoTasks.id, out s) && bool.TryParse(s, out var ghostsDoTasks_))
                GameOptionsData.hostOptionsData.GhostsDoTasks = ghostsDoTasks_;
            if (optionTable.TryGetValue(killDistance.id, out s) && int.TryParse(s, out var killDistance_))
                GameOptionsData.hostOptionsData.KillDistance = killDistance_;
            if (optionTable.TryGetValue(discussionTime.id, out s) && int.TryParse(s, out var discussionTime_))
                GameOptionsData.hostOptionsData.DiscussionTime = discussionTime_;
            if (optionTable.TryGetValue(votingTime.id, out s) && int.TryParse(s, out var votingTime_))
                GameOptionsData.hostOptionsData.VotingTime = votingTime_;
            if (optionTable.TryGetValue(confirmImpostor.id, out s) && bool.TryParse(s, out var confirmImpostor_))
                GameOptionsData.hostOptionsData.ConfirmImpostor = confirmImpostor_;
            if (optionTable.TryGetValue(visualTasks.id, out s) && bool.TryParse(s, out var visualTasks_))
                GameOptionsData.hostOptionsData.VisualTasks = visualTasks_;
            if (optionTable.TryGetValue(anonymousVotes.id, out s) && bool.TryParse(s, out var anonymousVotes_))
                GameOptionsData.hostOptionsData.AnonymousVotes = anonymousVotes_;
            if (optionTable.TryGetValue(taskBarMode.id, out s) && Enum.TryParse<TaskBarMode>(s, out var taskBarMode_))
                GameOptionsData.hostOptionsData.TaskBarMode = taskBarMode_;
            if (optionTable.TryGetValue(isDefaults.id, out s) && bool.TryParse(s, out var isDefaults_))
                GameOptionsData.hostOptionsData.isDefaults = isDefaults_;

            // Role options
            if (optionTable.TryGetValue(shapeshifterLeaveSkin.id, out s) && bool.TryParse(s, out var shapeshifterLeaveSkin_))
                GameOptionsData.hostOptionsData.RoleOptions.ShapeshifterLeaveSkin = shapeshifterLeaveSkin_;
            if (optionTable.TryGetValue(shapeshifterCooldown.id, out s) && float.TryParse(s, out var shapeshifterCooldown_))
                GameOptionsData.hostOptionsData.RoleOptions.ShapeshifterCooldown = shapeshifterCooldown_;
            if (optionTable.TryGetValue(shapeshifterDuration.id, out s) && float.TryParse(s, out var shapeshifterDuration_))
                GameOptionsData.hostOptionsData.RoleOptions.ShapeshifterDuration = shapeshifterDuration_;
            if (optionTable.TryGetValue(scientistCooldown.id, out s) && float.TryParse(s, out var scientistCooldown_))
                GameOptionsData.hostOptionsData.RoleOptions.ScientistCooldown = scientistCooldown_;
            if (optionTable.TryGetValue(scientistBatteryCharge.id, out s) && float.TryParse(s, out var scientistBatteryCharge_))
                GameOptionsData.hostOptionsData.RoleOptions.ScientistBatteryCharge = scientistBatteryCharge_;
            if (optionTable.TryGetValue(guardianAngelCooldown.id, out s) && float.TryParse(s, out var guardianAngelCooldown_))
                GameOptionsData.hostOptionsData.RoleOptions.GuardianAngelCooldown = guardianAngelCooldown_;
            if (optionTable.TryGetValue(impostorsCanSeeProtect.id, out s) && bool.TryParse(s, out var impostorsCanSeeProtect_))
                GameOptionsData.hostOptionsData.RoleOptions.ImpostorsCanSeeProtect = impostorsCanSeeProtect_;
            if (optionTable.TryGetValue(protectionDurationSeconds.id, out s) && float.TryParse(s, out var protectionDurationSeconds_))
                GameOptionsData.hostOptionsData.RoleOptions.ProtectionDurationSeconds = protectionDurationSeconds_;
            if (optionTable.TryGetValue(engineerCooldown.id, out s) && float.TryParse(s, out var engineerCooldown_))
                GameOptionsData.hostOptionsData.RoleOptions.EngineerCooldown = engineerCooldown_;
            if (optionTable.TryGetValue(engineerInVentMaxTime.id, out s) && float.TryParse(s, out var engineerInVentMaxTime_))
                GameOptionsData.hostOptionsData.RoleOptions.EngineerInVentMaxTime = engineerInVentMaxTime_;
            if (optionTable.TryGetValue(scientistMaxCount.id, out s) && int.TryParse(s, out var scientistMaxCount_) &&
                optionTable.TryGetValue(scientistChance.id, out s) && int.TryParse(s, out var scientistChance_))
                GameOptionsData.hostOptionsData.RoleOptions.SetRoleRate(RoleTypes.Scientist, scientistMaxCount_, scientistChance_);
            if (optionTable.TryGetValue(engineerMaxCount.id, out s) && int.TryParse(s, out var engineerMaxCount_) &&
                optionTable.TryGetValue(engineerChance.id, out s) && int.TryParse(s, out var engineerChance_))
                GameOptionsData.hostOptionsData.RoleOptions.SetRoleRate(RoleTypes.Engineer, engineerMaxCount_, engineerChance_);
            if (optionTable.TryGetValue(guardianAngelMaxCount.id, out s) && int.TryParse(s, out var guardianAngelMaxCount_) &&
                optionTable.TryGetValue(guardianAngelChance.id, out s) && int.TryParse(s, out var guardianAngelChance_))
                GameOptionsData.hostOptionsData.RoleOptions.SetRoleRate(RoleTypes.GuardianAngel, guardianAngelMaxCount_, guardianAngelChance_);
            if (optionTable.TryGetValue(shapeshifterMaxCount.id, out s) && int.TryParse(s, out var shapeshifterMaxCount_) &&
                optionTable.TryGetValue(shapeshifterChance.id, out s) && int.TryParse(s, out var shapeshifterChance_))
                GameOptionsData.hostOptionsData.RoleOptions.SetRoleRate(RoleTypes.Shapeshifter, shapeshifterMaxCount_, shapeshifterChance_);
        }

        public static void Inherit(string section, Dictionary<ConfigDefinition, string> orphanedEntries, StreamWriter sw)
        {
            // Generic options
            if (mapId.Load(section, orphanedEntries, out byte byteValue))
                sw.WriteLine(string.Format("{0},{1}", mapId.id, byteValue));
            if (playerSpeedMod.Load(section, orphanedEntries, out float floatValue))
                sw.WriteLine(string.Format("{0},{1}", playerSpeedMod.id, floatValue));
            if (crewLightMod.Load(section, orphanedEntries, out floatValue))
                sw.WriteLine(string.Format("{0},{1}", crewLightMod.id, floatValue));
            if (impostorLightMod.Load(section, orphanedEntries, out floatValue))
                sw.WriteLine(string.Format("{0},{1}", impostorLightMod.id, floatValue));
            if (killCooldown.Load(section, orphanedEntries, out floatValue))
                sw.WriteLine(string.Format("{0},{1}", killCooldown.id, floatValue));
            if (numCommonTasks.Load(section, orphanedEntries, out int intValue))
                sw.WriteLine(string.Format("{0},{1}", numCommonTasks.id, intValue));
            if (numLongTasks.Load(section, orphanedEntries, out intValue))
                sw.WriteLine(string.Format("{0},{1}", numLongTasks.id, intValue));
            if (numShortTasks.Load(section, orphanedEntries, out intValue))
                sw.WriteLine(string.Format("{0},{1}", numShortTasks.id, intValue));
            if (numEmergencyMeetings.Load(section, orphanedEntries, out intValue))
                sw.WriteLine(string.Format("{0},{1}", numEmergencyMeetings.id, intValue));
            if (emergencyCooldown.Load(section, orphanedEntries, out intValue))
                sw.WriteLine(string.Format("{0},{1}", emergencyCooldown.id, intValue));
            if (numImpostors.Load(section, orphanedEntries, out intValue))
                sw.WriteLine(string.Format("{0},{1}", numImpostors.id, intValue));
            if (ghostsDoTasks.Load(section, orphanedEntries, out bool boolValue))
                sw.WriteLine(string.Format("{0},{1}", ghostsDoTasks.id, boolValue));
            if (killDistance.Load(section, orphanedEntries, out intValue))
                sw.WriteLine(string.Format("{0},{1}", killDistance.id, intValue));
            if (discussionTime.Load(section, orphanedEntries, out intValue))
                sw.WriteLine(string.Format("{0},{1}", discussionTime.id, intValue));
            if (votingTime.Load(section, orphanedEntries, out intValue))
                sw.WriteLine(string.Format("{0},{1}", votingTime.id, intValue));
            if (confirmImpostor.Load(section, orphanedEntries, out boolValue))
                sw.WriteLine(string.Format("{0},{1}", confirmImpostor.id, boolValue));
            if (visualTasks.Load(section, orphanedEntries, out boolValue))
                sw.WriteLine(string.Format("{0},{1}", visualTasks.id, boolValue));
            if (anonymousVotes.Load(section, orphanedEntries, out boolValue))
                sw.WriteLine(string.Format("{0},{1}", anonymousVotes.id, boolValue));
            if (taskBarMode.Load(section, orphanedEntries, out TaskBarMode taskBarModeValue))
                sw.WriteLine(string.Format("{0},{1}", taskBarMode.id, taskBarModeValue));
            if (isDefaults.Load(section, orphanedEntries, out boolValue))
                sw.WriteLine(string.Format("{0},{1}", isDefaults.id, boolValue));

            // Role options
            if (shapeshifterLeaveSkin.Load(section, orphanedEntries, out boolValue))
                sw.WriteLine(string.Format("{0},{1}", shapeshifterLeaveSkin.id, boolValue));
            if (shapeshifterCooldown.Load(section, orphanedEntries, out floatValue))
                sw.WriteLine(string.Format("{0},{1}", shapeshifterCooldown.id, floatValue));
            if (shapeshifterDuration.Load(section, orphanedEntries, out floatValue))
                sw.WriteLine(string.Format("{0},{1}", shapeshifterDuration.id, floatValue));
            if (scientistCooldown.Load(section, orphanedEntries, out floatValue))
                sw.WriteLine(string.Format("{0},{1}", scientistCooldown.id, floatValue));
            if (scientistBatteryCharge.Load(section, orphanedEntries, out floatValue))
                sw.WriteLine(string.Format("{0},{1}", scientistBatteryCharge.id, floatValue));
            if (guardianAngelCooldown.Load(section, orphanedEntries, out floatValue))
                sw.WriteLine(string.Format("{0},{1}", guardianAngelCooldown.id, floatValue));
            if (impostorsCanSeeProtect.Load(section, orphanedEntries, out boolValue))
                sw.WriteLine(string.Format("{0},{1}", impostorsCanSeeProtect.id, boolValue));
            if (protectionDurationSeconds.Load(section, orphanedEntries, out floatValue))
                sw.WriteLine(string.Format("{0},{1}", protectionDurationSeconds.id, floatValue));
            if (engineerCooldown.Load(section, orphanedEntries, out floatValue))
                sw.WriteLine(string.Format("{0},{1}", engineerCooldown.id, floatValue));
            if (engineerInVentMaxTime.Load(section, orphanedEntries, out floatValue))
                sw.WriteLine(string.Format("{0},{1}", engineerInVentMaxTime.id, floatValue));

            if (scientistMaxCount.Load(section, orphanedEntries, out intValue) && scientistChance.Load(section, orphanedEntries, out int intValue2))
			{
                sw.WriteLine(string.Format("{0},{1}", scientistMaxCount.id, intValue));
                sw.WriteLine(string.Format("{0},{1}", scientistChance.id, intValue2));
            }
            if (engineerMaxCount.Load(section, orphanedEntries, out intValue) && engineerChance.Load(section, orphanedEntries, out intValue2))
			{
                sw.WriteLine(string.Format("{0},{1}", engineerMaxCount.id, intValue));
                sw.WriteLine(string.Format("{0},{1}", engineerChance.id, intValue2));
            }
            if (guardianAngelMaxCount.Load(section, orphanedEntries, out intValue) && guardianAngelChance.Load(section, orphanedEntries, out intValue2))
			{
                sw.WriteLine(string.Format("{0},{1}", guardianAngelMaxCount.id, intValue));
                sw.WriteLine(string.Format("{0},{1}", guardianAngelChance.id, intValue2));
            }
            if (shapeshifterMaxCount.Load(section, orphanedEntries, out intValue) && shapeshifterChance.Load(section, orphanedEntries, out intValue2))
			{
                sw.WriteLine(string.Format("{0},{1}", shapeshifterMaxCount.id, intValue));
                sw.WriteLine(string.Format("{0},{1}", shapeshifterChance.id, intValue2));
            }
        }

        public static void Remove(string section, Dictionary<ConfigDefinition, string> orphanedEntries, bool isSave = false) {
            // Generic options
            mapId.Remove(section, orphanedEntries);
            playerSpeedMod.Remove(section, orphanedEntries);
            crewLightMod.Remove(section, orphanedEntries);
            impostorLightMod.Remove(section, orphanedEntries);
            killCooldown.Remove(section, orphanedEntries);
            numCommonTasks.Remove(section, orphanedEntries);
            numLongTasks.Remove(section, orphanedEntries);
            numShortTasks.Remove(section, orphanedEntries);
            numEmergencyMeetings.Remove(section, orphanedEntries);
            emergencyCooldown.Remove(section, orphanedEntries);
            numImpostors.Remove(section, orphanedEntries);
            ghostsDoTasks.Remove(section, orphanedEntries);
            killDistance.Remove(section, orphanedEntries);
            discussionTime.Remove(section, orphanedEntries);
            votingTime.Remove(section, orphanedEntries);
            confirmImpostor.Remove(section, orphanedEntries);
            visualTasks.Remove(section, orphanedEntries);
            anonymousVotes.Remove(section, orphanedEntries);
            taskBarMode.Remove(section, orphanedEntries);
            isDefaults.Remove(section, orphanedEntries);


            // Role options
            shapeshifterLeaveSkin.Remove(section, orphanedEntries);
            shapeshifterCooldown.Remove(section, orphanedEntries);
            shapeshifterDuration.Remove(section, orphanedEntries);
            scientistCooldown.Remove(section, orphanedEntries);
            scientistBatteryCharge.Remove(section, orphanedEntries);
            guardianAngelCooldown.Remove(section, orphanedEntries);
            impostorsCanSeeProtect.Remove(section, orphanedEntries);
            protectionDurationSeconds.Remove(section, orphanedEntries);
            engineerCooldown.Remove(section, orphanedEntries);
            engineerInVentMaxTime.Remove(section, orphanedEntries);


            scientistMaxCount.Remove(section, orphanedEntries);
            scientistChance.Remove(section, orphanedEntries);
            engineerMaxCount.Remove(section, orphanedEntries);
            engineerChance.Remove(section, orphanedEntries);
            guardianAngelMaxCount.Remove(section, orphanedEntries);
            guardianAngelChance.Remove(section, orphanedEntries);
            shapeshifterMaxCount.Remove(section, orphanedEntries);
            shapeshifterChance.Remove(section, orphanedEntries);

            if (isSave)
                TheOtherRolesPlugin.Instance.Config.Save();
        }

        public static void Init()
        {
            defaultData = new GameOptionsData();

            // Generic options : 890000000-
            //keywords = new Option<InnerNet.GameKeywords>(890000000, defaultData.Keywords);
            //maxPlayers = new Option<int>(890000001, defaultData.MaxPlayers);
            mapId = new Option<byte>(890000002, defaultData.MapId);
            playerSpeedMod = new Option<float>(890000003, defaultData.PlayerSpeedMod);
            crewLightMod = new Option<float>(890000004, defaultData.CrewLightMod);
            impostorLightMod = new Option<float>(890000005, defaultData.ImpostorLightMod);
            killCooldown = new Option<float>(890000006, defaultData.KillCooldown);
            numCommonTasks = new Option<int>(890000007, defaultData.NumCommonTasks);
            numLongTasks = new Option<int>(890000008, defaultData.NumLongTasks);
            numShortTasks = new Option<int>(890000009, defaultData.NumShortTasks);
            numEmergencyMeetings = new Option<int>(890000010, defaultData.NumEmergencyMeetings);
            emergencyCooldown = new Option<int>(890000011, defaultData.EmergencyCooldown);
            numImpostors = new Option<int>(890000012, defaultData.NumImpostors);
            ghostsDoTasks = new Option<bool>(890000013, defaultData.GhostsDoTasks);
            killDistance = new Option<int>(890000014, defaultData.KillDistance);
            discussionTime = new Option<int>(890000015, defaultData.DiscussionTime);
            votingTime = new Option<int>(890000016, defaultData.VotingTime);
            confirmImpostor = new Option<bool>(890000017, defaultData.ConfirmImpostor);
            visualTasks = new Option<bool>(890000018, defaultData.VisualTasks);
            anonymousVotes = new Option<bool>(890000019, defaultData.AnonymousVotes);
            taskBarMode = new Option<TaskBarMode>(890000020, defaultData.TaskBarMode);
            isDefaults = new Option<bool>(890000021, defaultData.isDefaults);

            // Role options : 891000000-
            shapeshifterLeaveSkin = new Option<bool>(891000000, defaultData.RoleOptions.ShapeshifterLeaveSkin);
            shapeshifterCooldown = new Option<float>(891000001, defaultData.RoleOptions.ShapeshifterCooldown);
            shapeshifterDuration = new Option<float>(891000002, defaultData.RoleOptions.ShapeshifterDuration);
            scientistCooldown = new Option<float>(891000003, defaultData.RoleOptions.ScientistCooldown);
            scientistBatteryCharge = new Option<float>(891000004, defaultData.RoleOptions.ScientistBatteryCharge);
            guardianAngelCooldown = new Option<float>(891000005, defaultData.RoleOptions.GuardianAngelCooldown);
            impostorsCanSeeProtect = new Option<bool>(891000006, defaultData.RoleOptions.ImpostorsCanSeeProtect);
            protectionDurationSeconds = new Option<float>(891000007, defaultData.RoleOptions.ProtectionDurationSeconds);
            engineerCooldown = new Option<float>(891000008, defaultData.RoleOptions.EngineerCooldown);
            engineerInVentMaxTime = new Option<float>(891000009, defaultData.RoleOptions.EngineerInVentMaxTime);

            // Role generic options : 892000000-
            scientistMaxCount = new Option<int>(892000000, 0);
            scientistChance = new Option<int>(892000001, 0);

            engineerMaxCount = new Option<int>(892000100, 0);
            engineerChance = new Option<int>(892000101, 0);

            guardianAngelMaxCount = new Option<int>(892000200, 0);
            guardianAngelChance = new Option<int>(892000201, 0);

            shapeshifterMaxCount = new Option<int>(892000300, 0);
            shapeshifterChance = new Option<int>(892000301, 0);
        }

        static void Save(int id, bool value, Dictionary<int, string> optionTable, StreamWriter sw)
        {
            var v = value;
            if (optionTable.TryGetValue(id, out string s))
                bool.TryParse(s, out v);
            else
                optionTable[id] = value.ToString();
            sw.WriteLine(string.Format("{0},{1}", id, v));
        }

        static void Save(int id, byte value, Dictionary<int, string> optionTable, StreamWriter sw)
        {
            var v = value;
            if (optionTable.TryGetValue(id, out string s))
                byte.TryParse(s, out v);
            else
                optionTable[id] = value.ToString();
            sw.WriteLine(string.Format("{0},{1}", id, v));
        }

        static void Save(int id, int value, Dictionary<int, string> optionTable, StreamWriter sw)
        {
            var v = value;
            if (optionTable.TryGetValue(id, out string s))
                int.TryParse(s, out v);
            else
                optionTable[id] = value.ToString();
            sw.WriteLine(string.Format("{0},{1}", id, v));
        }

        static void Save(int id, float value, Dictionary<int, string> optionTable, StreamWriter sw)
        {
            var v = value;
            if (optionTable.TryGetValue(id, out string s))
                float.TryParse(s, out v);
            else
                optionTable[id] = value.ToString();
            sw.WriteLine(string.Format("{0},{1}", id, v));
        }

        static void Save(int id, TaskBarMode value, Dictionary<int, string> optionTable, StreamWriter sw)
        {
            var v = value;
            if (optionTable.TryGetValue(id, out string s))
                Enum.TryParse(s, out v);
            else
                optionTable[id] = value.ToString();
            sw.WriteLine(string.Format("{0},{1}", id, v));
        }

        class Option<T>
        {
            public int id { get; private set; }
            public T defaultData { get; private set; }

            public Option(int id, T defaultData) {
                this.id = id;
                this.defaultData = defaultData;
            }

            public void Save(string section, Dictionary<ConfigDefinition, string> orphanedEntries, T value) {
                var configDefinition = new ConfigDefinition(section, id.ToString());
                if (!orphanedEntries.ContainsKey(configDefinition))
                    orphanedEntries.Add(configDefinition, value.ToString());
                else
                    orphanedEntries[configDefinition] = value.ToString();
            }

            public bool Load(string section, Dictionary<ConfigDefinition, string> orphanedEntries, out int outValue) {
                outValue = 0;
                var configDefinition = new ConfigDefinition(section, id.ToString());
                return orphanedEntries.TryGetValue(configDefinition, out string value) && int.TryParse(value, out outValue);
            }

            public bool Load(string section, Dictionary<ConfigDefinition, string> orphanedEntries, out float outValue) {
                outValue = 0;
                var configDefinition = new ConfigDefinition(section, id.ToString());
                return orphanedEntries.TryGetValue(configDefinition, out string value) && float.TryParse(value, out outValue);
            }

            public bool Load(string section, Dictionary<ConfigDefinition, string> orphanedEntries, out byte outValue) {
                outValue = 0;
                var configDefinition = new ConfigDefinition(section, id.ToString());
                return orphanedEntries.TryGetValue(configDefinition, out string value) && byte.TryParse(value, out outValue);
            }

            public bool Load(string section, Dictionary<ConfigDefinition, string> orphanedEntries, out bool outValue) {
                outValue = false;
                var configDefinition = new ConfigDefinition(section, id.ToString());
                return orphanedEntries.TryGetValue(configDefinition, out string value) && bool.TryParse(value, out outValue);
            }

            public bool Load(string section, Dictionary<ConfigDefinition, string> orphanedEntries, out TaskBarMode outValue) {
                outValue = TaskBarMode.Normal;
                var configDefinition = new ConfigDefinition(section, id.ToString());
                return orphanedEntries.TryGetValue(configDefinition, out string value) && TaskBarMode.TryParse(value, out outValue);
            }

            public bool Remove(string section, Dictionary<ConfigDefinition, string> orphanedEntries) {
                var configDefinition = new ConfigDefinition(section, id.ToString());
                if (orphanedEntries.ContainsKey(configDefinition)) {
                    orphanedEntries.Remove(configDefinition);
                    return true;
                }
                return false;
            }
        }

        // Generic options
        //static Option<InnerNet.GameKeywords> keywords;
        //static Option<int> maxPlayers;
        static Option<byte> mapId;
        static Option<float> playerSpeedMod;
        static Option<float> crewLightMod;
        static Option<float> impostorLightMod;
        static Option<float> killCooldown;
        static Option<int> numCommonTasks;
        static Option<int> numLongTasks;
        static Option<int> numShortTasks;
        static Option<int> numEmergencyMeetings;
        static Option<int> emergencyCooldown;
        static Option<int> numImpostors;
        static Option<bool> ghostsDoTasks;
        static Option<int> killDistance;
        static Option<int> discussionTime;
        static Option<int> votingTime;
        static Option<bool> confirmImpostor;
        static Option<bool> visualTasks;
        static Option<bool> anonymousVotes;
        static Option<TaskBarMode> taskBarMode;
        static Option<bool> isDefaults;

        // Role options
        static Option<bool> shapeshifterLeaveSkin;
        static Option<float> shapeshifterCooldown;
        static Option<float> shapeshifterDuration;
        static Option<float> scientistCooldown;
        static Option<float> scientistBatteryCharge;
        static Option<float> guardianAngelCooldown;
        static Option<bool> impostorsCanSeeProtect;
        static Option<float> protectionDurationSeconds;
        static Option<float> engineerCooldown;
        static Option<float> engineerInVentMaxTime;

        static Option<int> scientistMaxCount;
        static Option<int> scientistChance;

        static Option<int> engineerMaxCount;
        static Option<int> engineerChance;

        static Option<int> guardianAngelMaxCount;
        static Option<int> guardianAngelChance;

        static Option<int> shapeshifterMaxCount;
        static Option<int> shapeshifterChance;

        static GameOptionsData defaultData = null;
    }
}