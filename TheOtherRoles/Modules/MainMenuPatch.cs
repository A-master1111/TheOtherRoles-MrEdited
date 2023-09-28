using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;
using TheOtherRoles.Patches;

namespace TheOtherRoles.Modules
{
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public class MainMenuPatch
    {
        private static bool horseButtonState = MapOptions.enableHorseMode;
        private static Sprite horseModeOffSprite = null;
        private static Sprite horseModeOnSprite = null;
        private static GameObject bottomTemplate;
        private static AnnouncementPopUp popUp;

        private static void Prefix(MainMenuManager __instance) {
            CustomHatLoader.LaunchHatFetcher();
            var template = GameObject.Find("ExitGameButton");
            // 备用QQ群
            var buttonDiscord = UnityEngine.Object.Instantiate(template, null);
            buttonDiscord.transform.localPosition = new Vector3(buttonDiscord.transform.localPosition.x, buttonDiscord.transform.localPosition.y + 1.8f, buttonDiscord.transform.localPosition.z);

            var textDiscord = buttonDiscord.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            __instance.StartCoroutine(Effects.Lerp(0.1f, new System.Action<float>((p) =>
            {
                textDiscord.SetText("副QQ群");
            })));

            PassiveButton passiveButtonDiscord = buttonDiscord.GetComponent<PassiveButton>();
            SpriteRenderer buttonSpriteDiscord = buttonDiscord.GetComponent<SpriteRenderer>();

            passiveButtonDiscord.OnClick = new Button.ButtonClickedEvent();
            passiveButtonDiscord.OnClick.AddListener((System.Action)(() => Application.OpenURL("http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=5gouDDt7_qj2HuoZK-0AibW5nZiYJJ3C&authKey=tfkYiKl3MvRK0nKSa96Ngnhd1QHNk2cFLwTAuGoacydi6BYZXd%2FKhLoCcuxxc48%2F&noverify=0&group_code=741550628")));

            Color discordColor = new Color32(88, 101, 242, byte.MaxValue);
            buttonSpriteDiscord.color = textDiscord.color = discordColor;
            passiveButtonDiscord.OnMouseOut.AddListener((System.Action)delegate
            {
                buttonSpriteDiscord.color = textDiscord.color = discordColor;
            });


            // GitHub按键
            var buttonTwitter = UnityEngine.Object.Instantiate(template, null);
            buttonTwitter.transform.localPosition = new Vector3(buttonTwitter.transform.localPosition.x, buttonTwitter.transform.localPosition.y + 2.4f, buttonTwitter.transform.localPosition.z);

            var textTwitter = buttonTwitter.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            __instance.StartCoroutine(Effects.Lerp(0.1f, new System.Action<float>((p) =>
            {
                textTwitter.SetText("GitHub");
            })));

            PassiveButton passiveButtonTwitter = buttonTwitter.GetComponent<PassiveButton>();
            SpriteRenderer buttonSpriteTwitter = buttonTwitter.GetComponent<SpriteRenderer>();

            passiveButtonTwitter.OnClick = new Button.ButtonClickedEvent();
            passiveButtonTwitter.OnClick.AddListener((System.Action)(() => Application.OpenURL("https://github.com/MrFangkuai/TheOtherRolesMrEdited")));

            Color twitterColor = new Color32(29, 161, 242, byte.MaxValue);
            buttonSpriteTwitter.color = textTwitter.color = twitterColor;
            passiveButtonTwitter.OnMouseOut.AddListener((System.Action)delegate
            {
                buttonSpriteTwitter.color = textTwitter.color = twitterColor;
            });

            if (template == null) return;

            var buttonQQ = UnityEngine.Object.Instantiate(template, null);
            buttonQQ.transform.localPosition = new Vector3(buttonQQ.transform.localPosition.x, buttonQQ.transform.localPosition.y + 0.6f, buttonQQ.transform.localPosition.z);

            var textQQ = buttonQQ.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            __instance.StartCoroutine(Effects.Lerp(0.1f, new System.Action<float>((p) => {
                textQQ.SetText("主QQ群");
            })));

            PassiveButton passiveButtonQQ = buttonQQ.GetComponent<PassiveButton>();
            SpriteRenderer buttonSpriteQQ = buttonQQ.GetComponent<SpriteRenderer>();

            passiveButtonQQ.OnClick = new Button.ButtonClickedEvent();
            passiveButtonQQ.OnClick.AddListener((System.Action)(() => Application.OpenURL("http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=q7jwai3-C2UPGxBsyo4yfomw6G_3oQU8&authKey=VLgQv1AkJ5JoP2ep5PK4LhvQC2zsV7LqfHuG0v03%2B0amCdv3%2BimT2DOGv3JB3GNH&noverify=0&group_code=928569362\r\n")));

            Color QQColor = new Color32(88, 101, 242, byte.MaxValue);
            buttonSpriteQQ.color = textQQ.color = QQColor;
            passiveButtonQQ.OnMouseOut.AddListener((System.Action)delegate {
                buttonSpriteQQ.color = textQQ.color = QQColor;
            });

            // Horse mode stuff
            var horseModeSelectionBehavior = new SelectionBehaviour(new TranslationInfo("MainMenu", 1), () => MapOptions.enableHorseMode = TheOtherRolesPlugin.EnableHorseMode.Value = !TheOtherRolesPlugin.EnableHorseMode.Value, TheOtherRolesPlugin.EnableHorseMode.Value);

            bottomTemplate = GameObject.Find("InventoryButton");
            if (bottomTemplate == null) return;
            var horseButton = Object.Instantiate(bottomTemplate, bottomTemplate.transform.parent);
            var passiveHorseButton = horseButton.GetComponent<PassiveButton>();
            var spriteHorseButton = horseButton.GetComponent<SpriteRenderer>();

            horseModeOffSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.HorseModeButtonOff.png", 75f);
            horseModeOnSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.HorseModeButtonOn.png", 75f);

            spriteHorseButton.sprite = horseButtonState ? horseModeOnSprite : horseModeOffSprite;

            passiveHorseButton.OnClick = new ButtonClickedEvent();
            passiveHorseButton.OnClick.AddListener((Action)(() => {
                horseButtonState = horseModeSelectionBehavior.onClick();
                if (horseButtonState) {
                    if (horseModeOnSprite == null) horseModeOnSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.HorseModeButtonOn.png", 75f);
                    spriteHorseButton.sprite = horseModeOnSprite;
                } else {
                    if (horseModeOffSprite == null) horseModeOffSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.HorseModeButtonOff.png", 75f);
                    spriteHorseButton.sprite = horseModeOffSprite;
                }
                CredentialsPatch.LogoPatch.updateSprite();
                // Avoid wrong Player Particles floating around in the background
                var particles = GameObject.FindObjectOfType<PlayerParticles>();
                if (particles != null) {
                    particles.pool.ReclaimAll();
                    particles.Start();
                }
            }));

            // TOR credits button
            if (bottomTemplate == null) return;
            var creditsButton = Object.Instantiate(bottomTemplate, bottomTemplate.transform.parent);
            var passiveCreditsButton = creditsButton.GetComponent<PassiveButton>();
            var spriteCreditsButton = creditsButton.GetComponent<SpriteRenderer>();

            spriteCreditsButton.sprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CreditsButton.png", 75f);

            passiveCreditsButton.OnClick = new ButtonClickedEvent();

            passiveCreditsButton.OnClick.AddListener((System.Action)delegate {
                // do stuff
                if (popUp != null) Object.Destroy(popUp);
                popUp = Object.Instantiate(Object.FindObjectOfType<AnnouncementPopUp>(true));
                popUp.gameObject.SetActive(true);
                popUp.Init();
                //SelectableHyperLinkHelper.DestroyGOs(popUp.selectableHyperLinks, "test");
                string creditsString =
(ModTranslation.GetString("Mod-Notice", 1));
                creditsString +=
(ModTranslation.GetString("Mod-Notice", 2));
                creditsString += "</align>";
                popUp.AnnounceTextMeshPro.text = creditsString;
                __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => {
                    if (p == 1) {
                        var titleText = GameObject.Find("Title_Text").GetComponent<TMPro.TextMeshPro>();
                        if (titleText != null) titleText.text = "公告&贡献者";
                    }
                })));
            });
            
        }

        public static void Postfix(MainMenuManager __instance) {
            __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => {
                if (p == 1) {
                    bottomTemplate = GameObject.Find("InventoryButton");
                    foreach (Transform tf in bottomTemplate.transform.parent.GetComponentsInChildren<Transform>()) {
                        tf.localPosition = new Vector2(tf.localPosition.x * 0.8f, tf.localPosition.y);
                    }
                }
            })));

        }
    }

    [HarmonyPatch(typeof(AnnouncementPopUp), nameof(AnnouncementPopUp.UpdateAnnounceText))]
    public static class Announcement
    {
        public static ModUpdateBehaviour.UpdateData updateData = null;
        public static bool Prefix(AnnouncementPopUp __instance)
        {
            if (ModUpdateBehaviour.showPopUp || updateData == null) return true;

            var text = __instance.AnnounceTextMeshPro;
            text.text = $"<size=150%><color=#FC0303>THE OTHER ROLES MR</color></size> {(updateData.Version)}\n{(updateData.Content)}";

            return false;
        }
    }
}
