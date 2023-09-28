using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using PowerTools;
using TheOtherRoles.Objects;
using TheOtherRoles.Players;
using TheOtherRoles.Utilities;
using TMPro;
using UnityEngine;

namespace TheOtherRoles.Patches
{

    [HarmonyPatch]
    public class SpawnInMinigamePatch
    {
        private static PassiveButton selected = null;
        public static List<SpawnCandidate> SpawnCandidates;
        public static SynchronizeData synchronizeData = new();
        public static bool isFirstSpawn = true;
        public static float initialDoorCooldown { get { return CustomOptionHolder.airshipInitialDoorCooldown.getFloat(); } }
        public static float initialSabotageCooldown { get { return CustomOptionHolder.airshipInitialSabotageCooldown.getFloat(); } }
        public enum SynchronizeTag
        {
            PreSpawnMinigame,
        }
        public class SynchronizeData
        {
            private Dictionary<SynchronizeTag, ulong> dic;

            public SynchronizeData()
            {
                dic = new Dictionary<SynchronizeTag, ulong>();
            }

            public void Synchronize(SynchronizeTag tag, byte playerId)
            {
                if (!dic.ContainsKey(tag)) dic[tag] = 0;

                dic[tag] |= (ulong)1 << playerId;
            }

            public bool Align(SynchronizeTag tag, bool withGhost, bool withSurvivor = true)
            {
                bool result = true;

                dic.TryGetValue(tag, out ulong value);

                foreach (PlayerControl pc in CachedPlayer.AllPlayers)
                {
                    if (pc.Data.IsDead ? withGhost : withSurvivor)
                        result &= (value & ((ulong)1 << pc.PlayerId)) != 0;
                }

                return result;
            }

            public void Reset(SynchronizeTag tag)
            {
                dic[tag] = 0;
            }

            public void Initialize()
            {
                dic.Clear();
            }

        }
        public static void reset()
        {
            isFirstSpawn = true;
            resetSpawnCandidates();
        }
        public static void resetSpawnCandidates()
        {
            SpawnCandidates = new List<SpawnCandidate>();
            if (CustomOptionHolder.airshipAdditionalSpawn.getBool())
            {
                SpawnCandidates.Add(new SpawnCandidate(StringNames.VaultRoom, new Vector2(-8.8f, 8.6f), "TheOtherRoles.Resources.Locations.VaultButton.png", "rollover_brig"));
                SpawnCandidates.Add(new SpawnCandidate(StringNames.MeetingRoom, new Vector2(11.0f, 14.7f), "TheOtherRoles.Resources.Locations.MeetingButton.png", "rollover_brig"));
                SpawnCandidates.Add(new SpawnCandidate(StringNames.Cockpit, new Vector2(-22.0f, -1.2f), "TheOtherRoles.Resources.Locations.CockpitButton.png", "rollover_brig"));
                SpawnCandidates.Add(new SpawnCandidate(StringNames.Electrical, new Vector2(16.4f, -8.5f), "TheOtherRoles.Resources.Locations.ElectricalButton.png", "rollover_brig"));
                SpawnCandidates.Add(new SpawnCandidate(StringNames.Lounge, new Vector2(30.9f, 7.5f), "TheOtherRoles.Resources.Locations.LoungeButton.png", "rollover_brig"));
                SpawnCandidates.Add(new SpawnCandidate(StringNames.Medical, new Vector2(25.5f, -5.0f), "TheOtherRoles.Resources.Locations.MedicalButton.png", "rollover_brig"));
                SpawnCandidates.Add(new SpawnCandidate(StringNames.Security, new Vector2(10.3f, -16.2f), "TheOtherRoles.Resources.Locations.SecurityButton.png", "rollover_brig"));
                SpawnCandidates.Add(new SpawnCandidate(StringNames.ViewingDeck, new Vector2(-14.1f, -16.2f), "TheOtherRoles.Resources.Locations.ViewingButton.png", "rollover_brig"));
                SpawnCandidates.Add(new SpawnCandidate(StringNames.Armory, new Vector2(-10.7f, -6.3f), "TheOtherRoles.Resources.Locations.ArmoryButton.png", "rollover_brig"));
                SpawnCandidates.Add(new SpawnCandidate(StringNames.Comms, new Vector2(-11.8f, 3.2f), "TheOtherRoles.Resources.Locations.CommunicationsButton.png", "rollover_brig"));
                SpawnCandidates.Add(new SpawnCandidate(StringNames.Showers, new Vector2(20.8f, 2.8f), "TheOtherRoles.Resources.Locations.ShowersButton.png", "rollover_brig"));
                SpawnCandidates.Add(new SpawnCandidate(StringNames.GapRoom, new Vector2(13.8f, 6.4f), "TheOtherRoles.Resources.Locations.GapButton.png", "rollover_brig"));
                foreach (var spawnCandidate in SpawnCandidates)
                {
                    spawnCandidate.ReloadTexture();
                }
            }
        }

        private static void resetButtons()
        {
            // MapUtilities.CachedShipStatus.Systems[SystemTypes.Sabotage].Cast<SabotageSystemType>().ForceSabTime(10f);
            isFirstSpawn = false;
            if (CustomOptionHolder.airshipSetOriginalCooldown.getBool())
            {
                CachedPlayer.LocalPlayer.PlayerControl.SetKillTimerUnchecked(PlayerControl.GameOptions.killCooldown);
                foreach (var b in CustomButton.buttons)
                {
                    b.Timer = b.MaxTimer;
                }
            }
            else
            {
                CachedPlayer.LocalPlayer.PlayerControl.SetKillTimerUnchecked(10f);
                CustomButton.buttons.ForEach(x => x.Timer = 10f);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SpawnInMinigame), nameof(SpawnInMinigame.Begin))]
        public static bool Prefix(SpawnInMinigame __instance, PlayerTask task)
        {
            if (TaskRacer.isValid())
            {
                var locationButtonList = new List<PassiveButton>();

                locationButtonList.AddRange(__instance.LocationButtons);
                for (int i = 0, n = __instance.Locations.Length - __instance.LocationButtons.Length; i < n; ++i)
                {
                    var button = UnityEngine.Object.Instantiate(locationButtonList[i], locationButtonList[i].transform.parent);
                    button.transform.localPosition = new Vector3(button.transform.localPosition.x, button.transform.localPosition.y + 1.8f, button.transform.localPosition.z);
                    locationButtonList.Add(button);
                }

                __instance.LocationButtons = locationButtonList.ToArray();
                if (!CustomOptionHolder.taskVsMode_EnabledBurgerMakeMode.getBool())
                {
                    foreach (var button in __instance.LocationButtons)
                    {
                        button.transform.localScale *= 0.8f;
                    }
                }
                CustomButton.stopCountdown = false;
                return true;
            }

            CustomButton.stopCountdown = true;
            // base.Begin(task);
            __instance.MyTask = task;
            __instance.MyNormTask = task as NormalPlayerTask;
            if (CachedPlayer.LocalPlayer.PlayerControl)
            {
                if (MapBehaviour.Instance)
                {
                    MapBehaviour.Instance.Close();
                }
                CachedPlayer.LocalPlayer.PlayerControl.NetTransform.Halt();
            }
            __instance.StartCoroutine(__instance.CoAnimateOpen());


            List<SpawnInMinigame.SpawnLocation> list = __instance.Locations.ToList<SpawnInMinigame.SpawnLocation>();
            foreach (var spawnCandidate in SpawnCandidates)
            {
                SpawnInMinigame.SpawnLocation spawnlocation = new()
                {
                    Location = spawnCandidate.SpawnLocation,
                    Image = spawnCandidate.GetSprite(),
                    Name = spawnCandidate.LocationKey,
                    Rollover = new AnimationClip(),
                    RolloverSfx = __instance.DefaultRolloverSound
                };
                list.Add(spawnlocation);
            }

            SpawnInMinigame.SpawnLocation[] array = list.ToArray<SpawnInMinigame.SpawnLocation>();
            array.Shuffle(0);
            array = (from s in array.Take(__instance.LocationButtons.Length)
                     orderby s.Location.x, s.Location.y descending
                     select s).ToArray<SpawnInMinigame.SpawnLocation>();
            CachedPlayer.LocalPlayer.PlayerControl.NetTransform.RpcSnapTo(new Vector2(-25f, 40f));

            for (int i = 0; i < __instance.LocationButtons.Length; i++)
            {
                PassiveButton passiveButton = __instance.LocationButtons[i];
                SpawnInMinigame.SpawnLocation pt = array[i];
                passiveButton.OnClick.AddListener((UnityEngine.Events.UnityAction)(() => SpawnAt(__instance, pt.Location)));
                passiveButton.GetComponent<SpriteAnim>().Stop();
                passiveButton.GetComponent<SpriteRenderer>().sprite = pt.Image;
                // passiveButton.GetComponentInChildren<TextMeshPro>().text = FastDestroyableSingleton<TranslationController>.Instance.GetString(pt.Name, Array.Empty<object>());
                passiveButton.GetComponentInChildren<TextMeshPro>().text = FastDestroyableSingleton<TranslationController>.Instance.GetString(pt.Name, new Il2CppReferenceArray<Il2CppSystem.Object>(0));
                ButtonAnimRolloverHandler component = passiveButton.GetComponent<ButtonAnimRolloverHandler>();
                component.StaticOutImage = pt.Image;
                component.RolloverAnim = pt.Rollover;
                component.HoverSound = pt.RolloverSfx ? pt.RolloverSfx : __instance.DefaultRolloverSound;
            }


            CachedPlayer.LocalPlayer.PlayerControl.gameObject.SetActive(false);
            CachedPlayer.LocalPlayer.PlayerControl.NetTransform.RpcSnapTo(new Vector2(-25f, 40f));
            if (!CustomOptionHolder.airshipRandomSpawn.getBool())
            {
                __instance.StartCoroutine(__instance.RunTimer());
            }
            ControllerManager.Instance.OpenOverlayMenu(__instance.name, null, __instance.DefaultButtonSelected, __instance.ControllerSelectable, false);
            PlayerControl.HideCursorTemporarily();
            ConsoleJoystick.SetMode_Menu();
            if (CustomOptionHolder.airshipRandomSpawn.getBool())
            {
                if (__instance.LocationButtons.Count > 0)
                    __instance.LocationButtons[UnityEngine.Random.Range(0, __instance.LocationButtons.Count)].ReceiveClickUp();
            }
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SpawnInMinigame), nameof(SpawnInMinigame.Begin))]
        public static void Postfix(SpawnInMinigame __instance)
        {
            selected = null;

            if (TaskRacer.isValid() && CustomOptionHolder.taskVsMode_EnabledBurgerMakeMode.getBool())
            {
                for (int i = 0; i < __instance.LocationButtons.Length; ++i)
                    __instance.LocationButtons[i].gameObject.SetActive(i == 1);

                for (int i = 0; i < __instance.Locations.Length; ++i)
                {
                    if (__instance.Locations[i].Name == StringNames.Kitchen)
                    {
                        PassiveButton passiveButton = __instance.LocationButtons[1];
                        SpawnInMinigame.SpawnLocation pt = __instance.Locations[i];
                        passiveButton.GetComponent<SpriteAnim>().Stop();
                        passiveButton.GetComponent<SpriteRenderer>().sprite = pt.Image;
                        passiveButton.GetComponentInChildren<TextMeshPro>().text = FastDestroyableSingleton<TranslationController>.Instance.GetString(pt.Name, new Il2CppReferenceArray<Il2CppSystem.Object>(0));
                        passiveButton.transform.localScale *= 1.5f;

                        ButtonAnimRolloverHandler component = passiveButton.GetComponent<ButtonAnimRolloverHandler>();
                        component.StaticOutImage = pt.Image;
                        component.RolloverAnim = pt.Rollover;
                        component.HoverSound = (pt.RolloverSfx ? pt.RolloverSfx : __instance.DefaultRolloverSound);

                        for (int j = 0; j < __instance.LocationButtons.Length; ++j)
                        {
                            __instance.LocationButtons[j].OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                            __instance.LocationButtons[j].OnClick.AddListener((Action)(() =>
                            {
                                __instance.SpawnAt(pt);
                            }));
                        }
                        break;
                    }
                }
            }

            if (!CustomOptionHolder.airshipSynchronizedSpawning.getBool() || CustomOptionHolder.airshipRandomSpawn.getBool()) return;
            foreach (var button in __instance.LocationButtons)
            {
                button.OnClick.AddListener((UnityEngine.Events.UnityAction)(() =>
                {
                    if (selected == null)
                        selected = button;
                }
                ));
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SpawnInMinigame), nameof(SpawnInMinigame.Close))]
        public static void Postfix2(SpawnInMinigame __instance)
        {
            // Task Vs Mode
            if (TaskRacer.isValid())
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                    Players.CachedPlayer.LocalPlayer.PlayerControl.NetId,
                    (byte)CustomRPC.TaskVsMode_Ready,
                    Hazel.SendOption.Reliable,
                    -1);
                writer.Write(Players.CachedPlayer.LocalPlayer.PlayerControl.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.taskVsModeReady(Players.CachedPlayer.LocalPlayer.PlayerControl.PlayerId);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SpawnInMinigame._RunTimer_d__10), nameof(SpawnInMinigame._RunTimer_d__10.MoveNext))]
        public static void Postfix(SpawnInMinigame._RunTimer_d__10 __instance)
        {
            if (!CustomOptionHolder.airshipSynchronizedSpawning.getBool() || CustomOptionHolder.airshipRandomSpawn.getBool()) return;
            if (selected != null)
                __instance.__4__this.Text.text = ModTranslation.GetString("Game-General", 10);
        }

        public static void Synchronize(SynchronizeTag tag, byte playerId)
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.Synchronize, Hazel.SendOption.Reliable, -1);
            writer.Write(playerId);
            writer.Write((int)tag);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.synchronize(playerId, (int)tag);
        }

        public static void SpawnAt(SpawnInMinigame __instance, Vector3 spawnAt)
        {
            if (!CustomOptionHolder.airshipSynchronizedSpawning.getBool() || CustomOptionHolder.airshipRandomSpawn.getBool())
            {
                if (isFirstSpawn) resetButtons();
                CustomButton.stopCountdown = false;
                if (__instance.amClosing != Minigame.CloseState.None)
                {
                    return;
                }
                __instance.gotButton = true;
                CachedPlayer.LocalPlayer.PlayerControl.gameObject.SetActive(true);
                __instance.StopAllCoroutines();
                CachedPlayer.LocalPlayer.PlayerControl.NetTransform.RpcSnapTo(spawnAt);
                FastDestroyableSingleton<HudManager>.Instance.PlayerCam.SnapToTarget();
                __instance.Close();
            }
            else
            {
                Synchronize(SynchronizeTag.PreSpawnMinigame, CachedPlayer.LocalPlayer.PlayerControl.PlayerId);
                if (__instance.amClosing != Minigame.CloseState.None)
                {
                    return;
                }
                if (__instance.gotButton) return;

                __instance.gotButton = true;


                foreach (var button in __instance.LocationButtons)
                {
                    button.enabled = false;
                }

                __instance.StartCoroutine(Effects.Lerp(10f, new Action<float>((p) =>
                {
                    float time = p * 10f;


                    foreach (var button in __instance.LocationButtons)
                    {
                        if (selected == button)
                        {
                            if (time > 0.3f)
                            {
                                float x = button.transform.localPosition.x;
                                if (x < 0f) x += 10f * Time.deltaTime;
                                if (x > 0f) x -= 10f * Time.deltaTime;
                                if (Mathf.Abs(x) < 10f * Time.deltaTime) x = 0f;
                                button.transform.localPosition = new Vector3(x, button.transform.localPosition.y, button.transform.localPosition.z);
                            }
                        }
                        else
                        {
                            var color = button.GetComponent<SpriteRenderer>().color;
                            float a = color.a;
                            if (a > 0f) a -= 2f * Time.deltaTime;
                            if (a < 0f) a = 0f;
                            button.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, a);
                            button.GetComponentInChildren<TextMeshPro>().color = new Color(1f, 1f, 1f, a);
                        }

                        if (__instance.amClosing != Minigame.CloseState.None) return;

                        if (synchronizeData.Align(SynchronizeTag.PreSpawnMinigame, false) || p == 1f)
                        {
                            CachedPlayer.LocalPlayer.PlayerControl.gameObject.SetActive(true);
                            __instance.StopAllCoroutines();
                            CachedPlayer.LocalPlayer.PlayerControl.NetTransform.RpcSnapTo(spawnAt);
                            FastDestroyableSingleton<HudManager>.Instance.PlayerCam.SnapToTarget();
                            synchronizeData.Reset(SynchronizeTag.PreSpawnMinigame);
                            __instance.Close();
                            CustomButton.stopCountdown = false;
                            // サボタージュのクールダウンをリセット
                            SabotageSystemType saboSystem = MapUtilities.CachedShipStatus.Systems[SystemTypes.Sabotage].Cast<SabotageSystemType>();
                            AccessTools.PropertySetter(typeof(SabotageSystemType), "IsDirty").Invoke(saboSystem, new object[]
                            {
                                true
                            });
                            saboSystem.ForceSabTime(0f);
                            saboSystem.Timer = initialSabotageCooldown;
                            DoorsSystemType doorSystem = MapUtilities.CachedShipStatus.Systems[SystemTypes.Doors].Cast<DoorsSystemType>();
                            AccessTools.PropertySetter(typeof(DoorsSystemType), "IsDirty").Invoke(doorSystem, new object[]
                            {
                                true
                            });
                            doorSystem.timers[SystemTypes.MainHall] = initialDoorCooldown;
                            doorSystem.timers[SystemTypes.Brig] = initialDoorCooldown;
                            doorSystem.timers[SystemTypes.Comms] = initialDoorCooldown;
                            doorSystem.timers[SystemTypes.Medical] = initialDoorCooldown;
                            doorSystem.timers[SystemTypes.Engine] = initialDoorCooldown;
                            doorSystem.timers[SystemTypes.Records] = initialDoorCooldown;
                            doorSystem.timers[SystemTypes.Kitchen] = initialDoorCooldown;

                            if (isFirstSpawn) resetButtons();
                        }
                    }

                })));

                return;

            }

        }

    }
}
