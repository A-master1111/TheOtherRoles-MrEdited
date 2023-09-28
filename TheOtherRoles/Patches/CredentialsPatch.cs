using HarmonyLib;
using System;
using TheOtherRoles;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles.Players;
using TheOtherRoles.Utilities;
using UnityEngine;
using static UnityEngine.UI.Button;

namespace TheOtherRoles.Patches {

    [HarmonyPatch]
    public static class CredentialsPatch {
        public static string fullCredentialsVersion = 
$@"{(ModTranslation.GetString("Mod-Participant", 1))} v{TheOtherRolesPlugin.Version.ToString() + (TheOtherRolesPlugin.betaDays>0 ? "-BETA": "")}";
        public static string fullCredentials =
(ModTranslation.GetString("Mod-Participant", 2));

        public static string mainMenuCredentials =
(ModTranslation.GetString("Mod-Participant", 3));

        public static string contributorsCredentials =
(ModTranslation.GetString("Mod-Participant", 4));

        [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
        private static class VersionShowerPatch
        {
            static void Postfix(VersionShower __instance) {
                var amongUsLogo = GameObject.Find("bannerLogo_AmongUs");
                if (amongUsLogo == null) return;

                var credentials = UnityEngine.Object.Instantiate<TMPro.TextMeshPro>(__instance.text);
                credentials.transform.position = new Vector3(0, 0, 0);
                credentials.SetText($"{(ModTranslation.GetString("Mod-Participant", 5))} v{TheOtherRolesPlugin.Version.ToString() + (TheOtherRolesPlugin.betaDays > 0 ? "-BETA" : "")}\n<size=30f%>\n</size>{mainMenuCredentials}\n<size=30%>\n</size>{contributorsCredentials}");
                credentials.alignment = TMPro.TextAlignmentOptions.Center;
                credentials.fontSize *= 0.75f;

                credentials.transform.SetParent(amongUsLogo.transform);
            }
        }

        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        public static class PingTrackerPatch
        {
            public static GameObject modStamp;
            public static GameObject customPreset;
            static void Prefix(PingTracker __instance) {
                if (modStamp == null) {
                    modStamp = new GameObject("ModStamp");
                    var rend = modStamp.AddComponent<SpriteRenderer>();
                    rend.sprite = TheOtherRolesPlugin.GetModStamp();
                    rend.color = new Color(1, 1, 1, 0.5f);
                    modStamp.transform.parent = __instance.transform.parent;
                    modStamp.transform.localScale *= SubmergedCompatibility.Loaded ? 0 : 0.6f;
                }
                if (customPreset == null) {
                    var buttonBehaviour = UnityEngine.Object.Instantiate(FastDestroyableSingleton<HudManager>.Instance.GameMenu.CensorChatButton);
                    buttonBehaviour.Text.text = "";
                    buttonBehaviour.Background.sprite = TheOtherRolesPlugin.GetCustomPreset();
                    buttonBehaviour.Background.color = new Color(1, 1, 1, 1);
                    customPreset = buttonBehaviour.gameObject;
                    customPreset.name = "CustomPreset";
                    customPreset.transform.parent = __instance.transform.parent;
                    customPreset.transform.localScale = new Vector3(0.2f, 1.2f, 1.2f) * 1.2f;
                    customPreset.SetActive(true);
                    var button = buttonBehaviour.GetComponent<PassiveButton>();
                    button.ClickSound = null;
                    button.OnMouseOver = new UnityEngine.Events.UnityEvent();
                    button.OnMouseOut = new UnityEngine.Events.UnityEvent();
                    button.OnClick = new ButtonClickedEvent();
                    button.OnClick.AddListener((Action)(() => {
                        ClientOptionsPatch.isOpenPreset = true;
                        FastDestroyableSingleton<HudManager>.Instance.GameMenu.Open();
                    }));
                }
                float offset = (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started) ? 0.75f : 0f;
                modStamp.transform.position = FastDestroyableSingleton<HudManager>.Instance.MapButton.transform.position + Vector3.down * offset;
                if (customPreset) {
                    customPreset.transform.position = modStamp.transform.position + Vector3.down * 0.75f;
                    if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && customPreset.gameObject.activeSelf)
                        customPreset.gameObject.SetActive(false);
                }

            }

            static void Postfix(PingTracker __instance){
                __instance.text.alignment = TMPro.TextAlignmentOptions.TopRight;
                if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started) {
                    string gameModeText = $"";
                    if (HideNSeek.isHideNSeekGM) gameModeText = ModTranslation.GetString("Credentials", 1);
                    else if (HandleGuesser.isGuesserGm) gameModeText = ModTranslation.GetString("Credentials", 2);
                    if (gameModeText != "") gameModeText = Helpers.cs(Color.yellow, gameModeText) + "\n";
                    __instance.text.text = $"{(ModTranslation.GetString("Mod-Participant", 1))} v{TheOtherRolesPlugin.Version.ToString() + (TheOtherRolesPlugin.betaDays > 0 ? "-BETA" : "")}\n{gameModeText}" + __instance.text.text;
                    if (CachedPlayer.LocalPlayer.Data.IsDead || (!(CachedPlayer.LocalPlayer.PlayerControl == null) && (CachedPlayer.LocalPlayer.PlayerControl == Lovers.lover1 || CachedPlayer.LocalPlayer.PlayerControl == Lovers.lover2))) {
                        __instance.transform.localPosition = new Vector3(3.45f, __instance.transform.localPosition.y, __instance.transform.localPosition.z);
                    } else {
                        __instance.transform.localPosition = new Vector3(4.2f, __instance.transform.localPosition.y, __instance.transform.localPosition.z);
                    }
                } else {
                    string gameModeText = $"";
                    if (MapOptions.gameMode == CustomGamemodes.HideNSeek) gameModeText = ModTranslation.GetString("Credentials", 1);
                    else if (MapOptions.gameMode == CustomGamemodes.Guesser) gameModeText = ModTranslation.GetString("Credentials", 2);
                    if (gameModeText != "") gameModeText = Helpers.cs(Color.yellow, gameModeText) + "\n";

                    __instance.text.text = $"{fullCredentialsVersion}\n  {gameModeText + fullCredentials}\n {__instance.text.text}";
                    __instance.transform.localPosition = new Vector3(3.5f, __instance.transform.localPosition.y, __instance.transform.localPosition.z);
                }
            }
        }

        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
        public static class LogoPatch
        {
            public static SpriteRenderer renderer;
            public static Sprite bannerSprite;
            public static Sprite horseBannerSprite;
            private static PingTracker instance;
            static void Postfix(PingTracker __instance) {
                var amongUsLogo = GameObject.Find("bannerLogo_AmongUs");
                if (amongUsLogo != null) {
                    amongUsLogo.transform.localScale *= 0.6f;
                    amongUsLogo.transform.position += Vector3.up * 0.25f;
                }

                var torLogo = new GameObject("bannerLogo_TOR");
                torLogo.transform.position = Vector3.up;
                renderer = torLogo.AddComponent<SpriteRenderer>();
                loadSprites();
                renderer.sprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.Banner.png", 300f);

                instance = __instance;
                loadSprites();
                renderer.sprite = MapOptions.enableHorseMode ? horseBannerSprite : bannerSprite;

                // Task Vs Mode
                TaskRacer.clearAndReload();
            }

            public static void loadSprites() {
                if (bannerSprite == null) bannerSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.Banner.png", 300f);
                if (horseBannerSprite == null) horseBannerSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.bannerTheHorseRoles.png", 300f);
            }

            public static void updateSprite() {
                loadSprites();
                if (renderer != null) {
                    float fadeDuration = 1f;
                    instance.StartCoroutine(Effects.Lerp(fadeDuration, new Action<float>((p) => {
                        renderer.color = new Color(1, 1, 1, 1 - p);
                        if (p == 1) {
                            renderer.sprite = MapOptions.enableHorseMode ? horseBannerSprite : bannerSprite;
                            instance.StartCoroutine(Effects.Lerp(fadeDuration, new Action<float>((p) => {
                                renderer.color = new Color(1, 1, 1, p);
                            })));
                        }
                    })));
                }
            }
        }
    }
}
