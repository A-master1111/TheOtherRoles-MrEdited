global using Il2CppInterop.Runtime;
global using Il2CppInterop.Runtime.Attributes;
global using Il2CppInterop.Runtime.InteropTypes;
global using Il2CppInterop.Runtime.InteropTypes.Arrays;
global using Il2CppInterop.Runtime.Injection;

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using TheOtherRoles.Modules;
using TheOtherRoles.Players;
using TheOtherRoles.Utilities;
using Il2CppSystem.Security.Cryptography;
using Il2CppSystem.Text;
using Reactor.Networking.Attributes;
using AmongUs.Data;
using TheOtherRoles.Patches;
using static TheOtherRoles.Patches.OnGameEndPatch;
using System.IO;
using System.Reflection;

namespace TheOtherRoles
{
    [BepInPlugin(Id, "The Other Roles", VersionString)]
    [BepInDependency(SubmergedCompatibility.SUBMERGED_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInProcess("Among Us.exe")]
    [ReactorModFlags(Reactor.Networking.ModFlags.RequireOnAllClients)]
    public class TheOtherRolesPlugin : BasePlugin
    {
        public const string Id = "me.eisbison.theotherroles";

        public const string VersionString = "2.9.5.1";
        public static uint betaDays = 0;  // amount of days for the build to be usable (0 for infinite!)

        public static System.Version Version = System.Version.Parse(VersionString);
        internal static BepInEx.Logging.ManualLogSource Logger;

        public Harmony Harmony { get; } = new Harmony(Id);
        public static TheOtherRolesPlugin Instance;

        public static int optionsPage = 1;
        public static int optionsPageMax = optionsPage + 1;

        public static ConfigEntry<string> DebugMode { get; private set; }
        public static ConfigEntry<bool> ViewSeacretMode { get; private set; }
        public static ConfigEntry<bool> GhostsSeeTasks { get; set; }
        public static ConfigEntry<bool> GhostsSeeRoles { get; set; }
        public static ConfigEntry<bool> GhostsSeeModifier { get; set; }
        public static ConfigEntry<bool> GhostsSeeVotes{ get; set; }
        public static ConfigEntry<bool> ShowRoleSummary { get; set; }
        public static ConfigEntry<bool> ShowLighterDarker { get; set; }
        public static ConfigEntry<bool> EnableSoundEffects { get; set; }
        public static ConfigEntry<bool> EnableHorseMode { get; set; }
        public static ConfigEntry<string> Ip { get; set; }
        public static ConfigEntry<ushort> Port { get; set; }
        public static ConfigEntry<string> ShowPopUpVersion { get; set; }

        public static Sprite ModStamp;
        public static Sprite CustomPreset;

        public static IRegionInfo[] defaultRegions;

        // This is part of the Mini.RegionInstaller, Licensed under GPLv3
        // file="RegionInstallPlugin.cs" company="miniduikboot">
        public static void UpdateRegions() {
            ServerManager serverManager = FastDestroyableSingleton<ServerManager>.Instance;
            var regions = new IRegionInfo[] {
                new DnsRegionInfo("45.125.46.201", "FangKuaiYa-Weekend", StringNames.NoTranslation, "45.125.46.201", 22000, false).CastFast<IRegionInfo>(),
                new DnsRegionInfo(Ip.Value, "Custom", StringNames.NoTranslation, Ip.Value, Port.Value, false).CastFast<IRegionInfo>()
            };
            
            IRegionInfo ? currentRegion = serverManager.CurrentRegion;
            Logger.LogInfo($"Adding {regions.Length} regions");
            foreach (IRegionInfo region in regions) {
                if (region == null) 
                    Logger.LogError("Could not add region");
                else {
                    if (currentRegion != null && region.Name.Equals(currentRegion.Name, StringComparison.OrdinalIgnoreCase)) 
                        currentRegion = region;               
                    serverManager.AddOrUpdateRegion(region);
                }
            }

            // AU remembers the previous region that was set, so we need to restore it
            if (currentRegion != null) {
                Logger.LogDebug("Resetting previous region");
                serverManager.SetRegion(currentRegion);
            }
        }

        public override void Load() {
            Logger = Log;
            Instance = this;
#if false
            Helpers.checkBeta(); // Exit if running an expired beta
#endif
            ModTranslation.Load();
            DebugMode = Config.Bind("Custom", "Enable Debug Mode", "false");
            ViewSeacretMode = Config.Bind("Custom", "View Seacret Mode", false);
            GhostsSeeTasks = Config.Bind("Custom", "Ghosts See Remaining Tasks", true);
            GhostsSeeRoles = Config.Bind("Custom", "Ghosts See Roles", true);
            GhostsSeeModifier = Config.Bind("Custom", "Ghosts See Modifier", true);
            GhostsSeeVotes = Config.Bind("Custom", "Ghosts See Votes", true);
            ShowRoleSummary = Config.Bind("Custom", "Show Role Summary", true);
            ShowLighterDarker = Config.Bind("Custom", "Show Lighter / Darker", false);
            EnableSoundEffects = Config.Bind("Custom", "Enable Sound Effects", true);
            EnableHorseMode = Config.Bind("Custom", "Enable Horse Mode", false);
            ShowPopUpVersion = Config.Bind("Custom", "Show PopUp", "0");

            Ip = Config.Bind("Custom", "Custom Server IP", "127.0.0.1");
            Port = Config.Bind("Custom", "Custom Server Port", (ushort)22023);
            defaultRegions = ServerManager.DefaultRegions;

            UpdateRegions();

            DebugMode = Config.Bind("Custom", "Enable Debug Mode", "false");
            Harmony.PatchAll();

            CustomOptionHolder.Load();
            CustomColors.Load();

            if (BepInExUpdater.UpdateRequired)
            {
                AddComponent<BepInExUpdater>();
                return;
            }
            
            SubmergedCompatibility.Initialize();
            AddComponent<ModUpdateBehaviour>();

            BasicOptions.Init();
            InheritCustomPreset();
        }

        public static void InheritCustomPreset()
		{
            // Create CustomPreset Folder
            string path = Path.GetDirectoryName(Application.dataPath) + @"\CustomPreset\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Inherit
            bool isSave = false;
            string customPresetPrefix = "CustomPreset_";
            var property = typeof(ConfigFile).GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance);
            var getter = property.GetGetMethod(true);
            if (getter != null)
            {
                var orphanedEntries = getter.Invoke(Instance.Config, new object[0]) as Dictionary<ConfigDefinition, string>;
                for (int i = 0; i < orphanedEntries.Count; ++i)
                {
                    string section = orphanedEntries.ElementAt(i).Key.Section;
                    if (section.Contains(customPresetPrefix))
                    {
                        var configDefinition = new ConfigDefinition(section, "0");
                        if (orphanedEntries.TryGetValue(configDefinition, out string value) && long.TryParse(value, out long time))
						{
                            string name = section.Substring(section.IndexOf(customPresetPrefix) + customPresetPrefix.Length);
                            ClientOptionsPatch.PresetInfo.InheritData(name);
                            Logger.LogMessage(string.Format("[OLD PRESET INHERIT]: {0}", name));
                            isSave = true;
                        }
                    }
                }
            }
            for (int i = 0; i < Instance.Config.Count; ++i)
            {
                string section = Instance.Config.ElementAt(i).Key.Section;
                if (section.Contains(customPresetPrefix))
                {
                    string name = section.Substring(section.IndexOf(customPresetPrefix) + customPresetPrefix.Length);
                    Logger.LogMessage(string.Format("[OLD PRESET INHERIT]: {0}", name));
                    ClientOptionsPatch.PresetInfo.InheritData(name);
                    isSave = true;
                }
            }
            if (isSave)
                Instance.Config.Save();
        }

        public static Sprite GetModStamp() {
            if (ModStamp) return ModStamp;
            return ModStamp = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.ModStamp.png", 150f);
        }
        public static Sprite GetCustomPreset() {
            if (CustomPreset) return CustomPreset;
            return CustomPreset = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CustomPreset.png", 150f);
        }
    }

    // Deactivate bans, since I always leave my local testing game and ban myself
    [HarmonyPatch(typeof(StatsManager), nameof(StatsManager.AmBanned), MethodType.Getter)]
    public static class AmBannedPatch
    {
        public static void Postfix(out bool __result)
        {
            __result = false;
        }
    }
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.Awake))]
    public static class ChatControllerAwakePatch {
        private static void Prefix() {
            if (!EOSManager.Instance.isKWSMinor) {
                DataManager.Settings.Multiplayer.ChatMode = InnerNet.QuickChatModes.FreeChatOrQuickChat;
            }
        }
    }
    
    // Debugging tools
    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class DebugManager
    {
        private static readonly string passwordHash = "7cd0fe883b2ef9e6079632d0bbcaa230194eeda5b8b8a57a15d59214e09fc358";
        public static List<PlayerControl> bots = new List<PlayerControl>();

        public static bool EnableDebugMode()
		{
            var builder = new StringBuilder();
            SHA256 sha = SHA256Managed.Create();
            Byte[] hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(TheOtherRolesPlugin.DebugMode.Value));
            foreach (var b in hashed)
            {
                builder.Append(b.ToString("x2"));
            }
            string enteredHash = builder.ToString();
            return enteredHash == passwordHash;
        }

        public static void Postfix(KeyboardJoystick __instance)
        {
            if (AmongUsClient.Instance.AmHost && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started)
            {
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F5))
                    CheckEndCriteriaPatch.UncheckedEndGame((GameOverReason)CustomGameOverReason.ForceEnd, false);
            }

            // Check if debug mode is active.
            if (!EnableDebugMode()) return;

            if (Input.GetKey(KeyCode.LeftControl)) {
                // Spawn dummys
                if (Input.GetKeyDown(KeyCode.F)) {
                    var playerControl = UnityEngine.Object.Instantiate(AmongUsClient.Instance.PlayerPrefab);
                    var i = playerControl.PlayerId = (byte)GameData.Instance.GetAvailableId();

                    bots.Add(playerControl);
                    GameData.Instance.AddPlayer(playerControl);
                    AmongUsClient.Instance.Spawn(playerControl, -2, InnerNet.SpawnFlags.None);

                    playerControl.transform.position = CachedPlayer.LocalPlayer.transform.position;
#if true
                    playerControl.GetComponent<DummyBehaviour>().enabled = true;
                    playerControl.NetTransform.enabled = false;
#else
                    playerControl.GetComponent<DummyBehaviour>().enabled = false;
                    playerControl.isDummy = false;
                    playerControl.notRealPlayer = true;
                    playerControl.NetTransform.enabled = true;
#endif
                    playerControl.SetName(Helpers.RandomString(10));
                    playerControl.SetColor((byte)Helpers.random.Next(Palette.PlayerColors.Length));
                    GameData.Instance.RpcSetTasks(playerControl.PlayerId, new byte[0]);
                }

                // Terminate round
                if(Input.GetKeyDown(KeyCode.L)) {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ForceEnd, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.forceEnd();
                }
            }

#if false
            if (Input.GetKey(KeyCode.LeftControl))
			{
                if (FastDestroyableSingleton<HudManager>.Instance != null)
				{
                    if (Input.GetKeyDown(KeyCode.Alpha9))
                    {
                        if (debugText != null)
                        {
                            if (debugText.gameObject != null)
                                GameObject.DestroyImmediate(debugText.gameObject);
                            debugText = null;
                        }
                    }

                    if (Input.GetKey(KeyCode.Alpha0))
                    {
                        if (debugText == null || debugText.gameObject == null)
                        {
                            RoomTracker roomTracker = FastDestroyableSingleton<HudManager>.Instance.roomTracker;
                            GameObject gameObject = UnityEngine.Object.Instantiate(roomTracker.gameObject);
                            UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<RoomTracker>());
                            gameObject.transform.SetParent(FastDestroyableSingleton<HudManager>.Instance.transform);
                            gameObject.transform.localPosition = new Vector3(0, 0, -930f);
                            gameObject.transform.localScale = Vector3.one * 1f;
                            debugText = gameObject.GetComponent<TMPro.TMP_Text>();
                            debugText.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                        }

                        StringBuilder builder = new();
                        {
                        }

                        debugText.text = builder.ToString();
                        debugText.gameObject.SetActive(true);
                    }
                    else if (debugText != null)
                    {
                        debugText.gameObject.SetActive(false);
                    }
                }
 

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    for (int i = 0; i < bots.Count; ++i)
                    {
                        int index = random.Next(bots.Count);
                        while (bots[index].Data.IsDead)
                            index = random.Next(bots.Count);
                        MeetingHud.Instance.CmdCastVote(bots[i].PlayerId, bots[index].PlayerId);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedMurderPlayer, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(CachedPlayer.LocalPlayer.PlayerId);
                    killWriter.Write(CachedPlayer.LocalPlayer.PlayerId);
                    killWriter.Write(byte.MaxValue);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.uncheckedMurderPlayer(CachedPlayer.LocalPlayer.PlayerId, CachedPlayer.LocalPlayer.PlayerId, Byte.MaxValue);
                }
            }
 
#endif
        }

#if false
        static TMPro.TMP_Text debugText;
#endif
    }
}
