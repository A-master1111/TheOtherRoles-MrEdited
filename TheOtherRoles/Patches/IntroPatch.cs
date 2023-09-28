using HarmonyLib;
using System;
using static TheOtherRoles.TheOtherRoles;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using TheOtherRoles.Players;
using TheOtherRoles.Utilities;
using TheOtherRoles.CustomGameModes;

namespace TheOtherRoles.Patches {
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
    class IntroCutsceneOnDestroyPatch
    {
        public static PoolablePlayer playerPrefab;
        public static void Prefix(IntroCutscene __instance) {
            // Generate and initialize player icons
            int playerCounter = 0;
            int hideNSeekCounter = 0;
            if (CachedPlayer.LocalPlayer != null && FastDestroyableSingleton<HudManager>.Instance != null) {
                Vector3 bottomLeft = new Vector3(-FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.x, FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.y, FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.z);
                foreach (PlayerControl p in CachedPlayer.AllPlayers) {
                    GameData.PlayerInfo data = p.Data;
                    PoolablePlayer player = UnityEngine.Object.Instantiate<PoolablePlayer>(__instance.PlayerPrefab, FastDestroyableSingleton<HudManager>.Instance.transform);
                    playerPrefab = __instance.PlayerPrefab;
                    p.SetPlayerMaterialColors(player.cosmetics.currentBodySprite.BodySprite);
                    player.SetSkin(data.DefaultOutfit.SkinId, data.DefaultOutfit.ColorId);
                    player.cosmetics.SetHat(data.DefaultOutfit.HatId, data.DefaultOutfit.ColorId);
                   // PlayerControl.SetPetImage(data.DefaultOutfit.PetId, data.DefaultOutfit.ColorId, player.PetSlot);
                    player.cosmetics.nameText.text = data.PlayerName;
                    player.SetFlipX(true);
                    MapOptions.playerIcons[p.PlayerId] = player;
                    player.gameObject.SetActive(false);

                    if (CachedPlayer.LocalPlayer.PlayerControl == Arsonist.arsonist && p != Arsonist.arsonist) {
                        player.transform.localPosition = bottomLeft + new Vector3(-0.25f, -0.25f, 0) + Vector3.right * playerCounter++ * 0.35f;
                        player.transform.localScale = Vector3.one * 0.2f;
                        player.setSemiTransparent(true);
                        player.gameObject.SetActive(true);
                    } else if (HideNSeek.isHideNSeekGM) {
                        if (HideNSeek.isHunted() && p.Data.Role.IsImpostor) {
                            player.transform.localPosition = bottomLeft + new Vector3(-0.25f, 0.4f, 0) + Vector3.right * playerCounter++ * 0.6f;
                            player.transform.localScale = Vector3.one * 0.3f;
                            player.cosmetics.nameText.text += $"{Helpers.cs(Color.red, ModTranslation.GetString("HideNSeek", 1))}";
                            player.gameObject.SetActive(true);
                        } else if (!p.Data.Role.IsImpostor) {
                            player.transform.localPosition = bottomLeft + new Vector3(-0.35f, -0.25f, 0) + Vector3.right * hideNSeekCounter++ * 0.35f;
                            player.transform.localScale = Vector3.one * 0.2f;
                            player.setSemiTransparent(true);
                            player.gameObject.SetActive(true);
                        }

                    } else if (CachedPlayer.LocalPlayer.PlayerControl == Kataomoi.kataomoi && p == Kataomoi.target) {
                        player.transform.localPosition = bottomLeft + new Vector3(-0.25f, 0f, 0);
                        player.transform.localScale = Vector3.one * 0.4f;
                        player.gameObject.SetActive(true);
                    } else if (TaskRacer.isTaskRacer(CachedPlayer.LocalPlayer.PlayerControl)) { // Task Vs Mode
                        var position = bottomLeft + new Vector3(-0.55f, -0.45f, 0) + Vector3.right * playerCounter++ * 0.35f;
                        TaskRacer.rankUIPositions.Add(position);
                        player.transform.localPosition = position;
                        player.transform.localScale = Vector3.one * 0.2f;
                        player.setSemiTransparent(false);
                        player.gameObject.SetActive(true);

                        int index = playerCounter - 1;
                        var taskFinishedMark = new GameObject("TaskFinishedMark_" + (index + 1));

                        var rend = taskFinishedMark.AddComponent<SpriteRenderer>();
                        rend.sprite = TaskRacer.getTaskFinishedSprites();
                        rend.color = new Color(1, 1, 1, 1);
                        taskFinishedMark.transform.parent = FastDestroyableSingleton<HudManager>.Instance.transform;
                        taskFinishedMark.transform.localPosition = position;
                        taskFinishedMark.transform.localScale = Vector3.one * 0.8f;
                        taskFinishedMark.SetActive(false);
                        TaskRacer.taskFinishedMarkTable.Add(p.PlayerId, taskFinishedMark);

                        if (playerCounter >= 1 && playerCounter <= 3) {
                            TaskRacer.rankMarkObjects[index] = new GameObject("RankMarkObject_" + (index + 1));
                            rend = TaskRacer.rankMarkObjects[index].AddComponent<SpriteRenderer>();
                            rend.sprite = TaskRacer.getRankGameSprites(playerCounter);
                            rend.color = new Color(1, 1, 1, 1);
                            TaskRacer.rankMarkObjects[index].transform.parent = FastDestroyableSingleton<HudManager>.Instance.transform;
                            TaskRacer.rankMarkObjects[index].transform.localPosition = position + new Vector3(0f, 0.39f, -8f);
                            TaskRacer.rankMarkObjects[index].transform.localScale = Vector3.one * 0.8f;
                        }
                    } else {   //  This can be done for all players not just for the bounty hunter as it was before. Allows the thief to have the correct position and scaling
                        player.transform.localPosition = bottomLeft + new Vector3(-0.25f, 0f, 0);
                        player.transform.localScale = Vector3.one * 0.4f;
                        player.gameObject.SetActive(false);
                    } 
                }
            }

            // Force Bounty Hunter to load a new Bounty when the Intro is over
            if (BountyHunter.bounty != null && CachedPlayer.LocalPlayer.PlayerControl == BountyHunter.bountyHunter) {
                BountyHunter.bountyUpdateTimer = 0f;
                if (FastDestroyableSingleton<HudManager>.Instance != null) {
                    Vector3 bottomLeft = new Vector3(-FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.x, FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.y, FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.z) + new Vector3(-0.25f, 1f, 0);
                    BountyHunter.cooldownText = UnityEngine.Object.Instantiate<TMPro.TextMeshPro>(FastDestroyableSingleton<HudManager>.Instance.KillButton.cooldownTimerText, FastDestroyableSingleton<HudManager>.Instance.transform);
                    BountyHunter.cooldownText.alignment = TMPro.TextAlignmentOptions.Center;
                    BountyHunter.cooldownText.transform.localPosition = bottomLeft + new Vector3(0f, -1f, -1f);
                    BountyHunter.cooldownText.gameObject.SetActive(true);
                }
            }

            // Force Reload of SoundEffectHolder
            SoundEffectsManager.Load();

            // First kill
            if (AmongUsClient.Instance.AmHost && MapOptions.shieldFirstKill && MapOptions.firstKillName != "" && !HideNSeek.isHideNSeekGM) {
                PlayerControl target = PlayerControl.AllPlayerControls.ToArray().ToList().FirstOrDefault(x => x.Data.PlayerName.Equals(MapOptions.firstKillName));
                if (target != null) {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetFirstKill, Hazel.SendOption.Reliable, -1);
                    writer.Write(target.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.setFirstKill(target.PlayerId);
                }
            }

            MapOptions.firstKillName = "";

            if (HideNSeek.isHideNSeekGM)
            {
                foreach (PlayerControl player in HideNSeek.getHunters())
                {
                    player.moveable = false;
                    player.NetTransform.Halt();
                    HideNSeek.timer = HideNSeek.hunterWaitingTime;
                    FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(HideNSeek.hunterWaitingTime, new Action<float>((p) => {
                        if (p == 1f)
                        {
                            player.moveable = true;
                            HideNSeek.timer = CustomOptionHolder.hideNSeekTimer.getFloat() * 60;
                            HideNSeek.isWaitingTimer = false;
                        }
                    })));
                }

                if (HideNSeek.polusVent == null && PlayerControl.GameOptions.MapId == 2)
                {
                    var list = GameObject.FindObjectsOfType<Vent>().ToList();
                    var adminVent = list.FirstOrDefault(x => x.gameObject.name == "AdminVent");
                    var bathroomVent = list.FirstOrDefault(x => x.gameObject.name == "BathroomVent");
                    HideNSeek.polusVent = UnityEngine.Object.Instantiate<Vent>(adminVent);
                    HideNSeek.polusVent.gameObject.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
                    HideNSeek.polusVent.transform.position = new Vector3(36.55068f, -21.5168f, -0.0215168f);
                    HideNSeek.polusVent.Left = adminVent;
                    HideNSeek.polusVent.Right = bathroomVent;
                    HideNSeek.polusVent.Center = null;
                    HideNSeek.polusVent.Id = MapUtilities.CachedShipStatus.AllVents.Select(x => x.Id).Max() + 1; // Make sure we have a unique id
                    var allVentsList = MapUtilities.CachedShipStatus.AllVents.ToList();
                    allVentsList.Add(HideNSeek.polusVent);
                    MapUtilities.CachedShipStatus.AllVents = allVentsList.ToArray();
                    HideNSeek.polusVent.gameObject.SetActive(true);
                    HideNSeek.polusVent.name = "newVent_" + HideNSeek.polusVent.Id;

                    adminVent.Center = HideNSeek.polusVent;
                    bathroomVent.Center = HideNSeek.polusVent;
                }

                ShipStatusPatch.originalNumCrewVisionOption = PlayerControl.GameOptions.CrewLightMod;
                ShipStatusPatch.originalNumImpVisionOption = PlayerControl.GameOptions.ImpostorLightMod;
                ShipStatusPatch.originalNumKillCooldownOption = PlayerControl.GameOptions.killCooldown;

                PlayerControl.GameOptions.ImpostorLightMod = CustomOptionHolder.hideNSeekHunterVision.getFloat();
                PlayerControl.GameOptions.CrewLightMod = CustomOptionHolder.hideNSeekHuntedVision.getFloat();
                PlayerControl.GameOptions.KillCooldown = CustomOptionHolder.hideNSeekKillCooldown.getFloat();
            }

            if (Kataomoi.kataomoi != null && CachedPlayer.LocalPlayer.PlayerControl == Kataomoi.kataomoi) {
                if (FastDestroyableSingleton<HudManager>.Instance != null) {
                    Vector3 bottomLeft = new Vector3(-FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.x, FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.y, FastDestroyableSingleton<HudManager>.Instance.UseButton.transform.localPosition.z) + new Vector3(-0.25f, 1f, 0);
                    Kataomoi.stareText = UnityEngine.Object.Instantiate(FastDestroyableSingleton<HudManager>.Instance.KillButton.cooldownTimerText, FastDestroyableSingleton<HudManager>.Instance.transform);
                    Kataomoi.stareText.alignment = TMPro.TextAlignmentOptions.Center;
                    Kataomoi.stareText.transform.localPosition = bottomLeft + new Vector3(0f, -1f, -1f);
                    Kataomoi.stareText.gameObject.SetActive(true);

                    Kataomoi.gaugeRenderer[0] = UnityEngine.Object.Instantiate(FastDestroyableSingleton<HudManager>.Instance.KillButton.graphic, FastDestroyableSingleton<HudManager>.Instance.transform);
                    var killButton = Kataomoi.gaugeRenderer[0].GetComponent<KillButton>();
                    killButton.SetCoolDown(0.00000001f, 0.00000001f);
                    killButton.SetFillUp(0.00000001f, 0.00000001f);
                    killButton.SetDisabled();
                    Helpers.hideGameObjects(Kataomoi.gaugeRenderer[0].gameObject);
                    var components = killButton.GetComponents<Component>();
                    foreach (var c in components) {
                        if ((c as KillButton) == null && (c as SpriteRenderer) == null)
                            GameObject.Destroy(c);
                    }

                    Kataomoi.gaugeRenderer[0].sprite = Kataomoi.getLoveGaugeSprite(0);
                    Kataomoi.gaugeRenderer[0].color = new Color32(175, 175, 176, 255);
                    Kataomoi.gaugeRenderer[0].size = new Vector2(300f, 64f);
                    Kataomoi.gaugeRenderer[0].gameObject.SetActive(true);
                    Kataomoi.gaugeRenderer[0].transform.localPosition = new Vector3(-3.354069f, -2.429999f, -8f);
                    Kataomoi.gaugeRenderer[0].transform.localScale = Vector3.one;

                    Kataomoi.gaugeRenderer[1] = UnityEngine.Object.Instantiate(Kataomoi.gaugeRenderer[0], FastDestroyableSingleton<HudManager>.Instance.transform);
                    Kataomoi.gaugeRenderer[1].sprite = Kataomoi.getLoveGaugeSprite(1);
                    Kataomoi.gaugeRenderer[1].size = new Vector2(261f, 7f);
                    Kataomoi.gaugeRenderer[1].color = Kataomoi.color;
                    Kataomoi.gaugeRenderer[1].transform.localPosition = new Vector3(-3.482069f, -2.626999f, -8.1f);
                    Kataomoi.gaugeRenderer[1].transform.localScale = Vector3.one;

                    Kataomoi.gaugeRenderer[2] = UnityEngine.Object.Instantiate(Kataomoi.gaugeRenderer[0], FastDestroyableSingleton<HudManager>.Instance.transform);
                    Kataomoi.gaugeRenderer[2].sprite = Kataomoi.getLoveGaugeSprite(2);
                    Kataomoi.gaugeRenderer[2].color = Kataomoi.gaugeRenderer[0].color;
                    Kataomoi.gaugeRenderer[2].size = new Vector2(300f, 64f);
                    Kataomoi.gaugeRenderer[2].transform.localPosition = new Vector3(-3.354069f, -2.429999f, -8.2f);
                    Kataomoi.gaugeRenderer[2].transform.localScale = Vector3.one;

                    Kataomoi.gaugeTimer = 1.0f;
                }
            }

            // Task Vs Mode
            if (TaskRacer.isValid()) {
                TaskRacer.startText = UnityEngine.Object.Instantiate(FastDestroyableSingleton<HudManager>.Instance.KillButton.cooldownTimerText, FastDestroyableSingleton<HudManager>.Instance.transform);
                TaskRacer.startText.rectTransform.sizeDelta = new Vector2(600, TaskRacer.startText.rectTransform.sizeDelta.y * 2);
                TaskRacer.startText.name = "TaskVsMode_Start";

                TaskRacer.timerText = UnityEngine.Object.Instantiate(FastDestroyableSingleton<HudManager>.Instance.KillButton.cooldownTimerText, FastDestroyableSingleton<HudManager>.Instance.transform);
                //TaskRacer.timerText.rectTransform.sizeDelta = new Vector2(600, TaskRacer.startText.rectTransform.sizeDelta.y * 2);
                //TaskRacer.timerText.alignment = TMPro.TextAlignmentOptions.BaselineLeft;
                TaskRacer.timerText.rectTransform.sizeDelta = new Vector2(600, TaskRacer.startText.rectTransform.sizeDelta.y);
                TaskRacer.timerText.transform.localPosition = new Vector3(-4.0f, 2.76f, TaskRacer.timerText.transform.localPosition.z);
                TaskRacer.timerText.transform.localScale *= 0.3f;
                TaskRacer.timerText.name = "TaskVsMode_Timer";
                TaskRacer.timerText.gameObject.SetActive(false);

                if (PlayerControl.GameOptions.MapId != (byte)MapId.Airship) {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                        CachedPlayer.LocalPlayer.PlayerControl.NetId,
                        (byte)CustomRPC.TaskVsMode_Ready,
                        Hazel.SendOption.Reliable,
                        -1);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerControl.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.taskVsModeReady(CachedPlayer.LocalPlayer.PlayerControl.PlayerId);
                }

                // Task Vs Mode
                IntroPatch.IntroPatchHelper.CheckTaskRacer();
            }

            if (PlayerControl.GameOptions.MapId == (byte)MapId.Airship) {
                if (CustomOptionHolder.airshipWallCheckOnTasks.getBool()) {
                    var objList = GameObject.FindObjectsOfType<Console>().ToList();
                    objList.Find(x => x.name == "task_garbage1").checkWalls = true;
                    objList.Find(x => x.name == "task_garbage2").checkWalls = true;
                    objList.Find(x => x.name == "task_garbage3").checkWalls = true;
                    objList.Find(x => x.name == "task_garbage4").checkWalls = true;
                    objList.Find(x => x.name == "task_garbage5").checkWalls = true;
                    objList.Find(x => x.name == "task_shower").checkWalls = true;
                    objList.Find(x => x.name == "task_developphotos").checkWalls = true;
                    objList.Find(x => x.name == "DivertRecieve" && x.Room == SystemTypes.Armory).checkWalls = true;
                    objList.Find(x => x.name == "DivertRecieve" && x.Room == SystemTypes.MainHall).checkWalls = true;
                }

                // アーカイブのアドミンを消す
                if (CustomOptionHolder.airshipChangeOldAdmin.getBool()) {
                    GameObject records = DestroyableSingleton<ShipStatus>.Instance.FastRooms[SystemTypes.Records].gameObject;
                    records.GetComponentsInChildren<MapConsole>().Where(x => x.name == "records_admin_map").FirstOrDefault()?.gameObject.SetActive(false);
                }
            }
        }
    }

    [HarmonyPatch]
    class IntroPatch {
        public static void setupIntroTeamIcons(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
            // Intro solo teams

            /*
             * Madmate is a solo team as well
             * This code is redundant, but this part should be decoupled from the original code
             * to merge future changes
             */
            if (CachedPlayer.LocalPlayer.PlayerControl == Madmate.madmate || Helpers.isNeutral(CachedPlayer.LocalPlayer.PlayerControl)) {
                var soloTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                soloTeam.Add(CachedPlayer.LocalPlayer.PlayerControl);
                yourTeam = soloTeam;
            }

            // Add the Spy to the Impostor team (for the Impostors)
            if (Spy.spy != null && CachedPlayer.LocalPlayer.Data.Role.IsImpostor) {
                List<PlayerControl> players = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
                var fakeImpostorTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>(); // The local player always has to be the first one in the list (to be displayed in the center)
                fakeImpostorTeam.Add(CachedPlayer.LocalPlayer.PlayerControl);
                foreach (PlayerControl p in players) {
                    if (CachedPlayer.LocalPlayer.PlayerControl != p && (p == Spy.spy || p.Data.Role.IsImpostor))
                        fakeImpostorTeam.Add(p);
                }
                yourTeam = fakeImpostorTeam;
            }
        }

        public static void setupIntroTeam(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
            List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(CachedPlayer.LocalPlayer.PlayerControl);
            RoleInfo roleInfo = infos.Where(info => !info.isModifier).FirstOrDefault();
            if (roleInfo == null) return;
            if (roleInfo.roleId == RoleId.TaskRacer) {
                __instance.BackgroundBar.material.color = roleInfo.color;
                __instance.TeamTitle.text = ModTranslation.GetString("Intro", 1);
                __instance.TeamTitle.color = roleInfo.color;
                __instance.ImpostorText.gameObject.SetActive(false);
            } else if (roleInfo.isNeutral) {
                var neutralColor = new Color32(76, 84, 78, 255);
                __instance.BackgroundBar.material.color = neutralColor;
                __instance.TeamTitle.text = ModTranslation.GetString("Intro", 2);
                __instance.TeamTitle.color = neutralColor;
            } else if (roleInfo.roleId == RoleId.Madmate) {
                __instance.BackgroundBar.material.color = roleInfo.color;
                __instance.TeamTitle.text = roleInfo.name;
                __instance.TeamTitle.color = roleInfo.color;
                __instance.ImpostorText.gameObject.SetActive(true);
                __instance.ImpostorText.text = ModTranslation.GetString("Intro", 3);
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
        class ShowRolePatch
        {
            public static void Postfix(IntroCutscene __instance) {
                if (!CustomOptionHolder.activateRoles.getBool()) return; // Don't override the intro of the vanilla roles
                if (IntroCutsceneShowRoleUpdatePatch.introCutscene != null)
                    IntroCutsceneShowRoleUpdatePatch.introCutscene = null;
                else
                    IntroCutsceneShowRoleUpdatePatch.introCutscene = __instance;
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.CreatePlayer))]
        class CreatePlayerPatch {
            public static void Postfix(IntroCutscene __instance, bool impostorPositioning, ref PoolablePlayer __result) {
                if (impostorPositioning) __result.SetNameColor(Palette.ImpostorRed);
            }
        }

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class IntroCutsceneShowRoleUpdatePatch
        {
            public static IntroCutscene introCutscene;

            public static void Postfix(HudManager __instance) {
                UpdateRoleText();
            }

            static void UpdateRoleText() {
                if (introCutscene == null) return;

                List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(CachedPlayer.LocalPlayer.PlayerControl);
                RoleInfo roleInfo = infos.Where(info => !info.isModifier).FirstOrDefault();
                RoleInfo modifierInfo = infos.Where(info => info.isModifier).FirstOrDefault();
                introCutscene.RoleBlurbText.text = "";
                if (roleInfo.roleId == RoleId.TaskMaster && TaskMaster.becomeATaskMasterWhenCompleteAllTasks)
                    roleInfo = RoleInfo.crewmate;

                if (roleInfo != null) {
                    introCutscene.RoleText.text = roleInfo.name;
                    introCutscene.RoleText.color = roleInfo.color;
                    introCutscene.RoleBlurbText.text = roleInfo.introDescription;
                    introCutscene.RoleBlurbText.color = roleInfo.color;
                }
                if (modifierInfo != null) {
                    if (modifierInfo.roleId != RoleId.Lover)
                        introCutscene.RoleBlurbText.text += Helpers.cs(modifierInfo.color, $"\n{modifierInfo.introDescription}");
                    else {

                        
                        PlayerControl otherLover = CachedPlayer.LocalPlayer.PlayerControl == Lovers.lover1 ? Lovers.lover2 : Lovers.lover1;
                        introCutscene.RoleBlurbText.text += Helpers.cs(Lovers.color, string.Format(ModTranslation.GetString("Intro", 4), otherLover?.Data?.PlayerName ?? ""));
                    }
                }
                if (infos.Any(info => info.roleId == RoleId.Kataomoi)) {
                    introCutscene.RoleBlurbText.text += Helpers.cs(Lovers.color, string.Format(ModTranslation.GetString("Intro", 5), Kataomoi.target?.Data?.PlayerName ?? ""));
                }
                if (Deputy.knowsSheriff && Deputy.deputy != null && Sheriff.sheriff != null) {
                    if (infos.Any(info => info.roleId == RoleId.Sheriff))
                        introCutscene.RoleBlurbText.text += Helpers.cs(Sheriff.color, string.Format(ModTranslation.GetString("Intro", 6), Deputy.deputy?.Data?.PlayerName ?? ""));
                    else if (infos.Any(info => info.roleId == RoleId.Deputy))
                        introCutscene.RoleBlurbText.text += Helpers.cs(Sheriff.color, string.Format(ModTranslation.GetString("Intro", 7), Sheriff.sheriff?.Data?.PlayerName ?? ""));
                }
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        class BeginCrewmatePatch {
            public static void Prefix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) {
                if (TaskRacer.isValid())
                    teamToDisplay = PlayerControl.AllPlayerControls;
                else
                    setupIntroTeamIcons(__instance, ref teamToDisplay);
            }

            public static void Postfix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) {
                if (TaskRacer.isValid())
                    teamToDisplay = PlayerControl.AllPlayerControls;

                 setupIntroTeam(__instance, ref teamToDisplay);

                /*
                 * Workaround
                 * reset and re-assign tasks
                 * This should be done before a game starting and after tasks assinged
                 * If you have an idea, please send me a pull request!
                 */
                if (Madmate.madmate != null && CachedPlayer.LocalPlayer.PlayerControl == Madmate.madmate
                    && Madmate.noticeImpostors) {
                    MadmateTaskHelper.SetMadmateTasks();
                }
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
        class BeginImpostorPatch {
            public static void Prefix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                if (TaskRacer.isValid())
                    yourTeam = PlayerControl.AllPlayerControls;
                else
                    setupIntroTeamIcons(__instance, ref yourTeam);
            }

            public static void Postfix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                if (TaskRacer.isValid())
                    yourTeam = PlayerControl.AllPlayerControls;

                setupIntroTeam(__instance, ref yourTeam);
            }
        }

        public static class IntroPatchHelper
        {
            public static void CheckTaskRacer() {
                // Task Vs Mode
                if (!TaskRacer.isValid())
                    return;

                if (AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost) {
                    List<byte> taskTypeIdList = null;
                    if (CustomOptionHolder.taskVsMode_EnabledBurgerMakeMode.getBool())
                    {
                        int num = CustomOptionHolder.taskVsMode_BurgerMakeMode_MakeBurgerNums.getInt();
                        taskTypeIdList = TaskRacer.generateBurgerTasks(num);
                        for (int i = taskTypeIdList.Count; i < num; ++i)
                            taskTypeIdList.Add(taskTypeIdList[0]);
                    }

                    // Init host's tasks.
                    if (taskTypeIdList == null && CustomOptionHolder.taskVsMode_EnabledMakeItTheSameTaskAsTheHost.getBool())
                    {
                        taskTypeIdList = new List<byte>();
                        for (int i = 0; i < PlayerControl.LocalPlayer.Data.Tasks.Count; ++i)
                            taskTypeIdList.Add(PlayerControl.LocalPlayer.Data.Tasks[i].TypeId);
                    }

                    if (taskTypeIdList == null) return;

                    var taskIdDataTable = new Dictionary<uint, byte[]>();
                    var playerData = CachedPlayer.LocalPlayer.PlayerControl.Data;
                    playerData.Object.clearAllTasks();
                    playerData.Tasks = new Il2CppSystem.Collections.Generic.List<GameData.TaskInfo>(taskTypeIdList.Count);
                    for (int j = 0; j < taskTypeIdList.Count; j++) {
                        playerData.Tasks.Add(new GameData.TaskInfo(taskTypeIdList[j], (uint)j));
                        playerData.Tasks[j].Id = (uint)j;
                    }
                    for (int j = 0; j < playerData.Tasks.Count; j++) {
                        GameData.TaskInfo taskInfo = playerData.Tasks[j];
                        NormalPlayerTask normalPlayerTask = UnityEngine.Object.Instantiate(MapUtilities.CachedShipStatus.GetTaskById(taskInfo.TypeId), playerData.Object.transform);
                        normalPlayerTask.Id = taskInfo.Id;
                        normalPlayerTask.Owner = playerData.Object;
                        normalPlayerTask.Initialize();
                        if (normalPlayerTask.Data != null && normalPlayerTask.Data.Length > 0)
                            taskIdDataTable.Add(normalPlayerTask.Id, normalPlayerTask.Data);
                        playerData.Object.myTasks.Add(normalPlayerTask);
                    }
                    foreach (var pair in taskIdDataTable) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                            CachedPlayer.LocalPlayer.PlayerControl.NetId,
                            (byte)CustomRPC.TaskVsMode_MakeItTheSameTaskAsTheHostDetail,
                            Hazel.SendOption.Reliable,
                            -1);
                        writer.Write(pair.Key);
                        writer.Write(pair.Value);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        TaskRacer.setHostTaskDetail(pair.Key, pair.Value);
                    }

                    MessageWriter writer2 = AmongUsClient.Instance.StartRpcImmediately(
                        CachedPlayer.LocalPlayer.PlayerControl.NetId,
                        (byte)CustomRPC.TaskVsMode_MakeItTheSameTaskAsTheHost,
                        Hazel.SendOption.Reliable,
                        -1);
                    byte[] taskTypeIds = taskTypeIdList.ToArray();
                    if (taskTypeIdList.Count > 0)
                        writer2.Write(taskTypeIds);
                    AmongUsClient.Instance.FinishRpcImmediately(writer2);
                    TaskRacer.setHostTasks(taskTypeIds);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Constants), nameof(Constants.ShouldHorseAround))]
    public static class ShouldAlwaysHorseAround {
        public static bool isHorseMode;
        public static bool Prefix(ref bool __result) {
            if (isHorseMode != MapOptions.enableHorseMode && LobbyBehaviour.Instance != null) __result = isHorseMode;
            else {
                __result = MapOptions.enableHorseMode;
                isHorseMode = MapOptions.enableHorseMode;
            }
            return false;
        }
    }
}

