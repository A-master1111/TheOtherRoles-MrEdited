using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;
using static TheOtherRoles.TheOtherRoles;
using TheOtherRoles.Modules;
using HarmonyLib;
using Hazel;
using TheOtherRoles.Players;
using TheOtherRoles.Utilities;
using System.Threading.Tasks;
using System.Net;
using System.Globalization;
using TheOtherRoles.CustomGameModes;

namespace TheOtherRoles {

    public enum MurderAttemptResult {
        PerformKill,
        SuppressKill,
        BlankKill,
    }

    public enum CustomGamemodes {
        Classic,
        Guesser,
        HideNSeek
    }

    public enum MapId {
        Skeld = 0,
        MiraHQ = 1,
        Polus = 2,
        //Dleks = 3, // deactivated
        Airship = 4,
    }

    public static class Helpers {

        public static Dictionary<string, Sprite> CachedSprites = new();

        public static bool gameStarted
        {
            get
            {
                return AmongUsClient.Instance?.GameState == InnerNet.InnerNetClient.GameStates.Started;
            }
        }

        public static Sprite loadSpriteFromResources(Texture2D texture, float pixelsPerUnit, Rect textureRect, Vector2 pivot)
        {
            return Sprite.Create(texture, textureRect, pivot, pixelsPerUnit);
        }

        public static Sprite loadSpriteFromResources(string path, float pixelsPerUnit) {
            try
            {
                if (CachedSprites.TryGetValue(path + pixelsPerUnit, out var sprite)) return sprite;
                Texture2D texture = loadTextureFromResources(path);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
                sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
                return CachedSprites[path + pixelsPerUnit] = sprite;
            } catch {
                System.Console.WriteLine("Error loading sprite from path: " + path);
            }
            return null;
        }

        public static unsafe Texture2D loadTextureFromResources(string path) {
            try {
                Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(path);
                var length = stream.Length;
                var byteTexture = new Il2CppStructArray<byte>(length);
                stream.Read(new Span<byte>(IntPtr.Add(byteTexture.Pointer, IntPtr.Size * 4).ToPointer(), (int) length));
                ImageConversion.LoadImage(texture, byteTexture, false);
                return texture;
            } catch {
                System.Console.WriteLine("Error loading texture from resources: " + path);
            }
            return null;
        }

        public static Texture2D loadTextureFromDisk(string path) {
            try {          
                if (File.Exists(path))     {
                    Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                    var byteTexture = Il2CppSystem.IO.File.ReadAllBytes(path);
                    ImageConversion.LoadImage(texture, byteTexture, false);
                    return texture;
                }
            } catch {
                System.Console.WriteLine("Error loading texture from disk: " + path);
            }
            return null;
        }

        public static AudioClip loadAudioClipFromResources(string path, string clipName = "UNNAMED_TOR_AUDIO_CLIP") {
            // must be "raw (headerless) 2-channel signed 32 bit pcm (le)" (can e.g. use Audacity® to export)
            try {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(path);
                var byteAudio = new byte[stream.Length];
                _ = stream.Read(byteAudio, 0, (int)stream.Length);
                float[] samples = new float[byteAudio.Length / 4]; // 4 bytes per sample
                int offset;
                for (int i = 0; i < samples.Length; i++) {
                    offset = i * 4;
                    samples[i] = (float)BitConverter.ToInt32(byteAudio, offset) / Int32.MaxValue;
                }
                int channels = 2;
                int sampleRate = 48000;
                AudioClip audioClip = AudioClip.Create(clipName, samples.Length, channels, sampleRate, false);
                audioClip.SetData(samples, 0);
                return audioClip;
            } catch {
                System.Console.WriteLine("Error loading AudioClip from resources: " + path);
            }
            return null;

            /* Usage example:
            AudioClip exampleClip = Helpers.loadAudioClipFromResources("TheOtherRoles.Resources.exampleClip.raw");
            if (Constants.ShouldPlaySfx()) SoundManager.Instance.PlaySound(exampleClip, false, 0.8f);
            */
        }
        public static bool existVitals() {
            switch ((MapId)PlayerControl.GameOptions.MapId) {
                case MapId.Polus:
                case MapId.Airship:
                    return true;
            }
            return false;
        }

        public static bool existSecurityCamera() {
            switch ((MapId)PlayerControl.GameOptions.MapId) {
                case MapId.Skeld:
                case MapId.Polus:
                case MapId.Airship:
                    return true;
            }
            return false;
        }

        public static int GetClientId(PlayerControl control)
        {
            for (int i = 0; i < AmongUsClient.Instance.allClients.Count; i++) {
                InnerNet.ClientData data = AmongUsClient.Instance.allClients[i];
                if (data.Character == control)
                    return data.Id;
            }
            return -1;
        }

        public static PlayerControl firstImpostorById()
        {
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
            {
                if (player.Data.Role.IsImpostor)
                    return player;
            }
            return null;
        }

        public static PlayerControl playerById(byte id)
        {
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
                if (player.PlayerId == id)
                    return player;
            return null;
        }
        
        public static Dictionary<byte, PlayerControl> allPlayersById()
        {
            Dictionary<byte, PlayerControl> res = new Dictionary<byte, PlayerControl>();
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
                res.Add(player.PlayerId, player);
            return res;
        }

        public static void handleVampireBiteOnBodyReport() {
            // Murder the bitten player and reset bitten (regardless whether the kill was successful or not)
            Helpers.checkMurderAttemptAndKill(Vampire.vampire, Vampire.bitten, true, false);
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.VampireSetBitten, Hazel.SendOption.Reliable, -1);
            writer.Write(byte.MaxValue);
            writer.Write(byte.MaxValue);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.vampireSetBitten(byte.MaxValue, byte.MaxValue);
        }

        public static void refreshRoleDescription(PlayerControl player) {
            List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(player);
            if (infos.Count > 0 && infos[0].roleId == RoleId.TaskMaster && !player.Data.IsDead && TaskMaster.becomeATaskMasterWhenCompleteAllTasks && !TaskMaster.isTaskComplete)
                infos[0] = RoleInfo.crewmate;
            List<string> taskTexts = new(infos.Count); 

            foreach (var roleInfo in infos)
            {
                taskTexts.Add(getRoleString(roleInfo));
            }
            
            var toRemove = new List<PlayerTask>();
            foreach (PlayerTask t in player.myTasks.GetFastEnumerator()) 
            {
                var textTask = t.TryCast<ImportantTextTask>();
                if (textTask == null) continue;
                
                var currentText = textTask.Text;
                
                if (taskTexts.Contains(currentText)) taskTexts.Remove(currentText); // TextTask for this RoleInfo does not have to be added, as it already exists
                else toRemove.Add(t); // TextTask does not have a corresponding RoleInfo and will hence be deleted
            }   

            foreach (PlayerTask t in toRemove) {
                t.OnRemove();
                player.myTasks.Remove(t);
                UnityEngine.Object.Destroy(t.gameObject);
            }

            // Add TextTask for remaining RoleInfos
            foreach (string title in taskTexts) {
                var task = new GameObject("RoleTask").AddComponent<ImportantTextTask>();
                task.transform.SetParent(player.transform, false);
                task.Text = title;
                player.myTasks.Insert(0, task);
            }
        }

        internal static string getRoleString(RoleInfo roleInfo)
        {
            if (roleInfo.roleId == RoleId.Jackal) 
            {

                var strId = Jackal.canCreateSidekick ? 1 : 2;
                return cs(roleInfo.color, string.Format(ModTranslation.GetString("Game-Jackal", strId), roleInfo.name));  
            }

            if (roleInfo.roleId == RoleId.Invert) 
            {
                return cs(roleInfo.color, $"{roleInfo.name}: {roleInfo.shortDescription} ({Invert.meetings})");
            }
            
            return cs(roleInfo.color, $"{roleInfo.name}: {roleInfo.shortDescription}");
        }
        
        public static bool isLighterColor(int colorId) {
            return CustomColors.lighterColors.Contains(colorId);
        }

        public static bool isCustomServer() {
            if (FastDestroyableSingleton<ServerManager>.Instance == null) return false;
            StringNames n = FastDestroyableSingleton<ServerManager>.Instance.CurrentRegion.TranslateName;
            return n != StringNames.ServerNA && n != StringNames.ServerEU && n != StringNames.ServerAS;
        }

        public static bool hasFakeTasks(this PlayerControl player) {
            if (player == Madmate.madmate)
                return !Madmate.noticeImpostors;

            return (player == Jester.jester || player == Jackal.jackal || player == Sidekick.sidekick || player == Arsonist.arsonist || player == Vulture.vulture || Jackal.formerJackals.Any(x => x == player) || player == Kataomoi.kataomoi || player == MadmateKiller.madmateKiller);
        }

        public static bool canBeErased(this PlayerControl player) {
            return (player != Jackal.jackal && player != Sidekick.sidekick && !Jackal.formerJackals.Any(x => x == player));
        }

        public static void clearAllTasks(this PlayerControl player) {
            if (player == null) return;
            foreach (var playerTask in player.myTasks.GetFastEnumerator())
            {
                playerTask.OnRemove();
                UnityEngine.Object.Destroy(playerTask.gameObject);
            }
            player.myTasks.Clear();
            
            if (player.Data != null && player.Data.Tasks != null)
                player.Data.Tasks.Clear();
        }

        public static void setSemiTransparent(this PoolablePlayer player, bool value) {
            float alpha = value ? 0.25f : 1f;
            foreach (SpriteRenderer r in player.gameObject.GetComponentsInChildren<SpriteRenderer>())
                r.color = new Color(r.color.r, r.color.g, r.color.b, alpha);
            player.cosmetics.nameText.color = new Color(player.cosmetics.nameText.color.r, player.cosmetics.nameText.color.g, player.cosmetics.nameText.color.b, alpha);
        }

        public static string GetString(this TranslationController t, StringNames key, params Il2CppSystem.Object[] parts) {
            return t.GetString(key, parts);
        }

        public static string cs(Color c, string s) {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b), ToByte(c.a), s);
        }
 
        private static byte ToByte(float f) {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }

        public static KeyValuePair<byte, int> MaxPair(this Dictionary<byte, int> self, out bool tie) {
            tie = true;
            KeyValuePair<byte, int> result = new KeyValuePair<byte, int>(byte.MaxValue, int.MinValue);
            foreach (KeyValuePair<byte, int> keyValuePair in self)
            {
                if (keyValuePair.Value > result.Value)
                {
                    result = keyValuePair;
                    tie = false;
                }
                else if (keyValuePair.Value == result.Value)
                {
                    tie = true;
                }
            }
            return result;
        }

        public static bool hidePlayerName(PlayerControl target) {
            return hidePlayerName(PlayerControl.LocalPlayer, target);
        }

        public static bool hidePlayerName(PlayerControl source, PlayerControl target) {
            if (source != target && Kataomoi.isStalking(target)) return true; // Hide kataomoi nametags

            if (source == target) return false;
            if (source == null || target == null) return true;
            if (source.isDead()) return false;
            if (target.isDead()) return true;

            if (Camouflager.camouflageTimer > 0f) return true; // No names are visible
            if (Ninja.isInvisble && Ninja.ninja == target) return true;
            if (MapOptions.hideOutOfSightNametags && gameStarted && MapUtilities.CachedShipStatus != null && source.transform != null && target.transform != null)
            {
                float distMod = 1.025f;
                float distance = Vector3.Distance(source.transform.position, target.transform.position);
                bool anythingBetween = PhysicsHelpers.AnythingBetween(source.GetTruePosition(), target.GetTruePosition(), Constants.ShadowMask, false);

                if (distance > MapUtilities.CachedShipStatus.CalculateLightRadius(source.Data) * distMod || anythingBetween) return true;
            }
            if (!MapOptions.hidePlayerNames) return false; // All names are visible
            if (source == null || target == null) return true;
            if (source == target) return false; // Player sees his own name
            if (source.Data.Role.IsImpostor && (target.Data.Role.IsImpostor || target == Spy.spy || target == Sidekick.sidekick && Sidekick.wasTeamRed || target == Jackal.jackal && Jackal.wasTeamRed)) return false; // Members of team Impostors see the names of Impostors/Spies
            if ((source == Lovers.lover1 || source == Lovers.lover2) && (target == Lovers.lover1 || target == Lovers.lover2)) return false; // Members of team Lovers see the names of each other
            if ((source == Jackal.jackal || source == Sidekick.sidekick) && (target == Jackal.jackal || target == Sidekick.sidekick || target == Jackal.fakeSidekick)) return false; // Members of team Jackal see the names of each other
            if (Deputy.knowsSheriff && (source == Sheriff.sheriff || source == Deputy.deputy) && (target == Sheriff.sheriff || target == Deputy.deputy)) return false; // Sheriff & Deputy see the names of each other

            return true;
        }

        public static void setDefaultLook(this PlayerControl target) {
            target.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
        }

        public static void setLook(this PlayerControl target, String playerName, int colorId, string hatId, string visorId, string skinId, string petId) {
            target.RawSetColor(colorId);
            target.RawSetVisor(visorId, colorId);
            target.RawSetHat(hatId, colorId);
            target.RawSetName(hidePlayerName(CachedPlayer.LocalPlayer.PlayerControl, target) ? "" : playerName);

            SkinViewData nextSkin = FastDestroyableSingleton<HatManager>.Instance.GetSkinById(skinId).viewData.viewData;
            PlayerPhysics playerPhysics = target.MyPhysics;
            AnimationClip clip = null;
            var spriteAnim = playerPhysics.myPlayer.cosmetics.skin.animator;
            var currentPhysicsAnim = playerPhysics.Animator.GetCurrentAnimation();
            if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.RunAnim) clip = nextSkin.RunAnim;
            else if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.SpawnAnim) clip = nextSkin.SpawnAnim;
            else if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.EnterVentAnim) clip = nextSkin.EnterVentAnim;
            else if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.ExitVentAnim) clip = nextSkin.ExitVentAnim;
            else if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.IdleAnim) clip = nextSkin.IdleAnim;
            else clip = nextSkin.IdleAnim;
            float progress = playerPhysics.Animator.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            playerPhysics.myPlayer.cosmetics.skin.skin = nextSkin;
            playerPhysics.myPlayer.cosmetics.skin.UpdateMaterial();

            spriteAnim.Play(clip, 1f);
            spriteAnim.m_animator.Play("a", 0, progress % 1);
            spriteAnim.m_animator.Update(0f);

            if (target.cosmetics.currentPet) UnityEngine.Object.Destroy(target.cosmetics.currentPet.gameObject);
            target.cosmetics.currentPet = UnityEngine.Object.Instantiate<PetBehaviour>(FastDestroyableSingleton<HatManager>.Instance.GetPetById(petId).viewData.viewData);
            target.cosmetics.currentPet.transform.position = target.transform.position;
            target.cosmetics.currentPet.Source = target;
            target.cosmetics.currentPet.Visible = target.Visible;
            target.SetPlayerMaterialColors(target.cosmetics.currentPet.rend);

            Chameleon.update();  // so that morphling and camo wont make the chameleons visible
        }

        public static void showFlash(Color color, float duration=1f) {
            if (FastDestroyableSingleton<HudManager>.Instance == null || FastDestroyableSingleton<HudManager>.Instance.FullScreen == null) return;
            FastDestroyableSingleton<HudManager>.Instance.FullScreen.gameObject.SetActive(true);
            FastDestroyableSingleton<HudManager>.Instance.FullScreen.enabled = true;
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) => {
                var renderer = FastDestroyableSingleton<HudManager>.Instance.FullScreen;

                if (p < 0.5) {
                    if (renderer != null)
                        renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01(p * 2 * 0.75f));
                } else {
                    if (renderer != null)
                        renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01((1 - p) * 2 * 0.75f));
                }
                if (p == 1f && renderer != null) renderer.enabled = false;
            })));
        }

        public static bool roleCanUseVents(this PlayerControl player) {
            if (player == null) return false;

            bool roleCouldUse = false;
            if (Engineer.engineer != null && Engineer.engineer == player)
                roleCouldUse = true;
            else if (Jackal.canUseVents && Jackal.jackal != null && Jackal.jackal == player)
                roleCouldUse = true;
            else if (Sidekick.canUseVents && Sidekick.sidekick != null && Sidekick.sidekick == player)
                roleCouldUse = true;
            else if (Spy.canEnterVents && Spy.spy != null && Spy.spy == player)
                roleCouldUse = true;
            else if (Vulture.canUseVents && Vulture.vulture != null && Vulture.vulture == player)
                roleCouldUse = true;
            else if (Madmate.canEnterVents && Madmate.madmate != null && Madmate.madmate == player)
                roleCouldUse = true;
            else if (MadmateKiller.canEnterVents && MadmateKiller.madmateKiller != null && MadmateKiller.madmateKiller == player)
                roleCouldUse = true;
            else if (Thief.canUseVents &&  Thief.thief != null && Thief.thief == player)
                roleCouldUse = true;
            else if (player.Data?.Role != null && player.Data.Role.CanVent)  {
                if (Janitor.janitor != null && Janitor.janitor == CachedPlayer.LocalPlayer.PlayerControl)
                    roleCouldUse = false;
                else if (Mafioso.mafioso != null && Mafioso.mafioso == CachedPlayer.LocalPlayer.PlayerControl && Godfather.godfather != null && !Godfather.godfather.Data.IsDead)
                    roleCouldUse = false;
                else
                    roleCouldUse = true;
            }
            return roleCouldUse;
        }

        public static MurderAttemptResult checkMuderAttempt(PlayerControl killer, PlayerControl target, bool blockRewind = false) {
            var targetRole = RoleInfo.getRoleInfoForPlayer(target, false).FirstOrDefault();

            // Modified vanilla checks
            if (AmongUsClient.Instance.IsGameOver) return MurderAttemptResult.SuppressKill;
            if (killer == null || killer.Data == null || killer.Data.IsDead || killer.Data.Disconnected) return MurderAttemptResult.SuppressKill; // Allow non Impostor kills compared to vanilla code
            if (target == null || target.Data == null || target.Data.IsDead || target.Data.Disconnected) return MurderAttemptResult.SuppressKill; // Allow killing players in vents compared to vanilla code

            // Handle first kill attempt
            if (MapOptions.shieldFirstKill && MapOptions.firstKillPlayer == target) return MurderAttemptResult.SuppressKill;

            // Handle blank shot
            if (Pursuer.blankedList.Any(x => x.PlayerId == killer.PlayerId)) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetBlanked, Hazel.SendOption.Reliable, -1);
                writer.Write(killer.PlayerId);
                writer.Write((byte)0);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.setBlanked(killer.PlayerId, 0);

                return MurderAttemptResult.BlankKill;
            }

            // Block impostor shielded kill
            if (Medic.shielded != null && Medic.shielded == target) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(killer.NetId, (byte)CustomRPC.ShieldedMurderAttempt, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.shieldedMurderAttempt();
                SoundEffectsManager.play("fail");
                return MurderAttemptResult.SuppressKill;
            }

            // Block impostor not fully grown mini kill
            else if (Mini.mini != null && target == Mini.mini && !Mini.isGrownUp()) {
                return MurderAttemptResult.SuppressKill;
            }

            // Block Time Master with time shield kill
            else if (TimeMaster.shieldActive && TimeMaster.timeMaster != null && TimeMaster.timeMaster == target) {
                if (!blockRewind) { // Only rewind the attempt was not called because a meeting startet 
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(killer.NetId, (byte)CustomRPC.TimeMasterRewindTime, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.timeMasterRewindTime();
                }
                return MurderAttemptResult.SuppressKill;
            }

            // Thief if hit crew only kill if setting says so, but also kill the thief.
            else if (killer == Thief.thief && !target.Data.Role.IsImpostor && !new List<RoleInfo> {RoleInfo.jackal, Thief.canKillSheriff ? RoleInfo.sheriff : null, RoleInfo.sidekick }.Contains(targetRole)) {
                Thief.suicideFlag = true;
                return MurderAttemptResult.SuppressKill;
            }

            // Block hunted with time shield kill
            else if (Hunted.timeshieldActive.Contains(target.PlayerId)) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(killer.NetId, (byte)CustomRPC.HuntedRewindTime, Hazel.SendOption.Reliable, -1);
                writer.Write(target.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.huntedRewindTime(target.PlayerId);

                return MurderAttemptResult.SuppressKill;
            }

            return MurderAttemptResult.PerformKill;
        }

        public static MurderAttemptResult checkMurderAttemptAndKill(PlayerControl killer, PlayerControl target, bool isMeetingStart = false, bool showAnimation = true)  {
            // The local player checks for the validity of the kill and performs it afterwards (different to vanilla, where the host performs all the checks)
            // The kill attempt will be shared using a custom RPC, hence combining modded and unmodded versions is impossible

            MurderAttemptResult murder = checkMuderAttempt(killer, target, isMeetingStart);
            if (murder == MurderAttemptResult.PerformKill) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedMurderPlayer, Hazel.SendOption.Reliable, -1);
                writer.Write(killer.PlayerId);
                writer.Write(target.PlayerId);
                writer.Write(showAnimation ? Byte.MaxValue : 0);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.uncheckedMurderPlayer(killer.PlayerId, target.PlayerId, showAnimation ? Byte.MaxValue : (byte)0);
            }
            return murder;
        }
    
        public static void shareGameVersion() {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.VersionHandshake, Hazel.SendOption.Reliable, -1);
            writer.Write((byte)TheOtherRolesPlugin.Version.Major);
            writer.Write((byte)TheOtherRolesPlugin.Version.Minor);
            writer.Write((byte)TheOtherRolesPlugin.Version.Build);
            writer.Write(AmongUsClient.Instance.AmHost ? Patches.GameStartManagerPatch.timer : -1f);
            writer.WritePacked(AmongUsClient.Instance.ClientId);
            writer.Write((byte)(TheOtherRolesPlugin.Version.Revision < 0 ? 0xFF : TheOtherRolesPlugin.Version.Revision));
            writer.Write(Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId.ToByteArray());
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.versionHandshake(TheOtherRolesPlugin.Version.Major, TheOtherRolesPlugin.Version.Minor, TheOtherRolesPlugin.Version.Build, TheOtherRolesPlugin.Version.Revision, Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId, AmongUsClient.Instance.ClientId);
        }

        public static List<PlayerControl> getKillerTeamMembers(PlayerControl player) {
            List<PlayerControl> team = new List<PlayerControl>();
            foreach(PlayerControl p in CachedPlayer.AllPlayers) {
                if (player.Data.Role.IsImpostor && p.Data.Role.IsImpostor && player.PlayerId != p.PlayerId && team.All(x => x.PlayerId != p.PlayerId)) team.Add(p);
                else if (player == Jackal.jackal && p == Sidekick.sidekick) team.Add(p); 
                else if (player == Sidekick.sidekick && p == Jackal.jackal) team.Add(p);
            }
            
            return team;
        }

        public static bool isNeutral(PlayerControl player) {
            RoleInfo roleInfo = RoleInfo.getRoleInfoForPlayer(player, false).FirstOrDefault();
            if (roleInfo != null)
                return roleInfo.isNeutral;
            return false;
        }


        public static bool zoomOutStatus = false;
        public static void toggleZoom(bool reset=false) {
            float orthographicSize = reset || zoomOutStatus ? 3f : 12f;

            zoomOutStatus = !zoomOutStatus && !reset;
            Camera.main.orthographicSize = orthographicSize;
            foreach (var cam in Camera.allCameras) {
                if (cam != null && cam.gameObject.name == "UI Camera") cam.orthographicSize = orthographicSize;  // The UI is scaled too, else we cant click the buttons. Downside: map is super small.
            }

            HudManagerStartPatch.zoomOutButton.Sprite = zoomOutStatus ? Helpers.loadSpriteFromResources("TheOtherRoles.Resources.PlusButton.png", 75f) : Helpers.loadSpriteFromResources("TheOtherRoles.Resources.MinusButton.png", 150f);
            HudManagerStartPatch.zoomOutButton.PositionOffset = zoomOutStatus ? new Vector3(0f, 3f, 0) : new Vector3(0.4f, 2.8f, 0);
            ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height); // This will move button positions to the correct position.
        }

#if false
        public static void checkBeta() {
            if (TheOtherRolesPlugin.betaDays > 0) {
                var compileTime = new DateTime(Builtin.CompileTime, DateTimeKind.Utc);  // This may show as an error, but it is not, compilation will work!
                DateTime now;
                // Get time from the internet, so no-one can cheat it.
                try {
                    using (var response =
                      WebRequest.Create("http://www.google.com").GetResponse())
                        now = DateTime.ParseExact(response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
                } catch (WebException e) {
                    TheOtherRolesPlugin.Logger.LogMessage($"{e}");
                    now = DateTime.Now; //In case something goes wrong. 
                }
                if ((now - compileTime).TotalDays > TheOtherRolesPlugin.betaDays) {
                    TheOtherRolesPlugin.Logger.LogMessage($"Beta expired!");
                    BepInExUpdater.MessageBox(IntPtr.Zero, "BETA is expired. You cannot play this version anymore.", "The Other Roles Beta", 1);
                    Application.Quit();

                } else TheOtherRolesPlugin.Logger.LogMessage($"Beta will remain runnable for {(DateTime.Now - compileTime).TotalDays - TheOtherRolesPlugin.betaDays} days!");
            }
        }
#endif
        public static bool hasImpVision(GameData.PlayerInfo player) {
            return player.Role.IsImpostor
                || (Madmate.madmate != null && Madmate.madmate.PlayerId == player.PlayerId && Madmate.hasImpostorVision)
                || (MadmateKiller.madmateKiller != null && MadmateKiller.madmateKiller.PlayerId == player.PlayerId && MadmateKiller.hasImpostorVision)
                || ((Jackal.jackal != null && Jackal.jackal.PlayerId == player.PlayerId || Jackal.formerJackals.Any(x => x.PlayerId == player.PlayerId)) && Jackal.hasImpostorVision)
                || (Sidekick.sidekick != null && Sidekick.sidekick.PlayerId == player.PlayerId && Sidekick.hasImpostorVision)
                || (Spy.spy != null && Spy.spy.PlayerId == player.PlayerId && Spy.hasImpostorVision)
                || (Jester.jester != null && Jester.jester.PlayerId == player.PlayerId && Jester.hasImpostorVision)
                || (Thief.thief != null && Thief.thief.PlayerId == player.PlayerId && Thief.hasImpostorVision);
        }
        
        public static object TryCast(this Il2CppObjectBase self, Type type)
        {
            return AccessTools.Method(self.GetType(), nameof(Il2CppObjectBase.TryCast)).MakeGenericMethod(type).Invoke(self, Array.Empty<object>());
        }

        public class DestroyInfo
        {
            public DestroyInfo(string searchName, bool isFindChild = true) {
                this.searchName = searchName;
                this.isDestroyChild = isFindChild;
            }

            public string searchName = "";
            public bool isDestroyChild = true;
        }

        public static bool isDead(this PlayerControl player) {
            return player?.Data?.IsDead == true || player?.Data?.Disconnected == true;
        }

        public static bool isAlive(this PlayerControl player) {
            return !isDead(player);
        }

        public static bool destroyGameObjects(GameObject root, DestroyInfo[] excludeInfos = null, GameObject obj = null) {
            if (obj == null)
                obj = root;

            DestroyInfo findInfo = null;
            if (excludeInfos != null)
                findInfo = Array.Find(excludeInfos, (info) => info.searchName == obj.name);
            if (obj != root && findInfo == null) {
                UnityEngine.Object.DestroyImmediate(obj);
                return true;
            }

            if (findInfo == null || findInfo.isDestroyChild) {
                for (int i = 0; i < obj.transform.GetChildCount(); ++i) {
                    if (destroyGameObjects(root, excludeInfos, obj.transform.GetChild(i).gameObject))
                        --i;
                }
            }
            return false;
        }

        public static bool hideGameObjects(GameObject root, DestroyInfo[] excludeInfos = null, GameObject obj = null) {
            if (obj == null)
                obj = root;

            DestroyInfo findInfo = null;
            if (excludeInfos != null)
                findInfo = Array.Find(excludeInfos, (info) => info.searchName == obj.name);
            if (obj != root && findInfo == null) {
                obj.SetActive(false);
                return true;
            }

            if (findInfo == null || findInfo.isDestroyChild) {
                for (int i = 0; i < obj.transform.GetChildCount(); ++i) {
                    if (destroyGameObjects(root, excludeInfos, obj.transform.GetChild(i).gameObject))
                        --i;
                }
            }
            return false;
        }

        public static void logTransform(string name, Transform t, int index = 0) {
            string space = "";
            for (int i = 0; i < index; ++i)
                space += "  ";
            TheOtherRolesPlugin.Logger.LogMessage(string.Format("[{0}]{1}{2}[{3}]", name, space, t.name,
                t.gameObject.activeSelf ? "o" : "x"));
            for (int i = 0; i < t.GetChildCount(); ++i)
                logTransform(name, t.GetChild(i), index + 1);
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static readonly System.Random random = new System.Random((int)DateTime.Now.Ticks);
    }
}
