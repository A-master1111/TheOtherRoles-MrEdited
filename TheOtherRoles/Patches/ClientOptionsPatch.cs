using HarmonyLib;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using TheOtherRoles.Utilities;
using TMPro;
using UnityEngine.Events;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;
using System.Reflection;
using TheOtherRoles.Players;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace TheOtherRoles.Patches 
{
    [HarmonyPatch]
    public static class ClientOptionsPatch
    {
        static SelectionBehaviour[] AllOptions = {
            new SelectionBehaviour(new TranslationInfo("MainMenu", 2), () => MapOptions.ghostsSeeTasks = TheOtherRolesPlugin.GhostsSeeTasks.Value = !TheOtherRolesPlugin.GhostsSeeTasks.Value, TheOtherRolesPlugin.GhostsSeeTasks.Value),
            new SelectionBehaviour(new TranslationInfo("MainMenu", 3), () => MapOptions.ghostsSeeVotes = TheOtherRolesPlugin.GhostsSeeVotes.Value = !TheOtherRolesPlugin.GhostsSeeVotes.Value, TheOtherRolesPlugin.GhostsSeeVotes.Value),
            new SelectionBehaviour(new TranslationInfo("MainMenu", 4), () => MapOptions.ghostsSeeRoles = TheOtherRolesPlugin.GhostsSeeRoles.Value = !TheOtherRolesPlugin.GhostsSeeRoles.Value, TheOtherRolesPlugin.GhostsSeeRoles.Value),
            new SelectionBehaviour(new TranslationInfo("MainMenu", 5), () => MapOptions.ghostsSeeModifier = TheOtherRolesPlugin.GhostsSeeModifier.Value = !TheOtherRolesPlugin.GhostsSeeModifier.Value, TheOtherRolesPlugin.GhostsSeeModifier.Value),
            new SelectionBehaviour(new TranslationInfo("MainMenu", 6), () => MapOptions.showRoleSummary = TheOtherRolesPlugin.ShowRoleSummary.Value = !TheOtherRolesPlugin.ShowRoleSummary.Value, TheOtherRolesPlugin.ShowRoleSummary.Value),
            new SelectionBehaviour(new TranslationInfo("MainMenu", 7), () => MapOptions.showLighterDarker = TheOtherRolesPlugin.ShowLighterDarker.Value = !TheOtherRolesPlugin.ShowLighterDarker.Value, TheOtherRolesPlugin.ShowLighterDarker.Value),
            new SelectionBehaviour(new TranslationInfo("MainMenu", 8), () => MapOptions.enableSoundEffects = TheOtherRolesPlugin.EnableSoundEffects.Value = !TheOtherRolesPlugin.EnableSoundEffects.Value, TheOtherRolesPlugin.EnableSoundEffects.Value),
        };

        public static bool isOpenPreset = false;
        public static GameObject popUp;
        static TextMeshPro titleText;
        static ToggleButtonBehaviour buttonPrefab;
        static Vector3? _origin;
        static ToggleButtonBehaviour moreOptions;
        static TextMeshPro optionTitle;
        const string PresetNameTitle = "PresetName,";


        public class PresetInfo
        {
            public string presetName { get; set; }
            public long registTime { get; set; }

            public static void InheritData(string presetName) {
                var property = typeof(BepInEx.Configuration.ConfigFile).GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance);
                var getter = property.GetGetMethod(true);
                if (getter == null) return;

                var orphanedEntries = getter.Invoke(TheOtherRolesPlugin.Instance.Config, new object[0]) as Dictionary<BepInEx.Configuration.ConfigDefinition, string>;
                string path = GetFilePathRandom();
                using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    string section = GetSectionName(presetName);
                    sw.WriteLine("# [CustomPreset]");
                    sw.WriteLine(string.Format("{0},{1}", "PresetName", presetName));
                    var configDefinition = new BepInEx.Configuration.ConfigDefinition(section, "0");
                    if (orphanedEntries.TryGetValue(configDefinition, out string value) && long.TryParse(value, out long time))
                        sw.WriteLine(string.Format("{0},{1}", 0, time));
                    BasicOptions.Inherit(section, orphanedEntries, sw);
                    foreach (CustomOption option in CustomOption.options)
                    {
                        if (option.id == 0) continue;

                        int v = option.defaultSelection;
                        configDefinition = new BepInEx.Configuration.ConfigDefinition(section, option.id.ToString());
                        if (orphanedEntries.TryGetValue(configDefinition, out string value2))
                            int.TryParse(value2, out v);
                        sw.WriteLine(string.Format("{0},{1}", option.id, v));
                    }

                    // Remove
                    BasicOptions.Remove(section, orphanedEntries);
                    configDefinition = new BepInEx.Configuration.ConfigDefinition(section, "0");
                    if (!TheOtherRolesPlugin.Instance.Config.Remove(configDefinition))
                        orphanedEntries.Remove(configDefinition);
                    foreach (var option in CustomOption.options)
                    {
                        configDefinition = new BepInEx.Configuration.ConfigDefinition(section, option.id.ToString());
                        if (!TheOtherRolesPlugin.Instance.Config.Remove(configDefinition))
                            orphanedEntries.Remove(configDefinition);
                    }
                }
            }

            public PresetInfo(string presetName, string filePath = "") {
               SetPresetName(presetName);
               this.filePath = !string.IsNullOrEmpty(filePath) ? filePath : GetFilePathRandom();

                if (File.Exists(filePath))
                {
                    using (var sr = new StreamReader(filePath, Encoding.UTF8))
                    {
                        int count = 0;
                        while (!sr.EndOfStream)
                        {
                            string text = sr.ReadLine();
                            if (count > 1)
                            {
                                var elements = text.Split(',');
                                if (elements.Length >= 2 && int.TryParse(elements[0], out var key))
                                    optionValueTable.Add(key, elements[1]);
                            }
                            ++count;
                        }
                    }

                    if (optionValueTable.ContainsKey(0) && long.TryParse(optionValueTable[0], out long time))
                        SetRegistTime(time);
                }
            }

            public void SetRegistTime(long time) {
                registTime = time;
            }

            public string GetDescription(int newlineCount = -1) {
                string description = "";
                int roleCount = 0;
                bool isJapanese = AmongUs.Data.DataManager.Settings.Language.CurrentLanguage == SupportedLangs.Japanese;
                int roleCountMax = isJapanese ? 6 : 8;
                foreach (var option in CustomOption.options) {
                    if (option.id == 0) continue;
                    int v = option.defaultSelection;
                    if (optionValueTable.TryGetValue(option.id, out string value))
                        int.TryParse(value, out v);
                    if (option.isRoleHeader() && v > 0) {
                        ++roleCount;
                        if (roleCount <= roleCountMax) {
                            if (roleCount > 1 && !(newlineCount > 0 && (roleCount - 1) % newlineCount == 0)) description += "/";
                            description += option.getTitle();
                            if (roleCount != roleCountMax && newlineCount > 0 && roleCount % newlineCount == 0) description += "\n";
                        }
                    }
                }
                if (roleCount > roleCountMax)
                {
                    int addCount = roleCount - roleCountMax;
                    string text = addCount.ToString();
                    if (isJapanese)
                        text = Regex.Replace(text, "[0-9]", p => ((char)(p.Value[0] - '0' + '‚O')).ToString());
                    description += string.Format(ModTranslation.GetString("MainMenu", 17), text);
                }
                else if (roleCount == 0)
                {
                    description += ModTranslation.GetString("MainMenu", 28);
                }

                if (newlineCount != -1)
				{
                    int count = Mathf.Min(roleCount, roleCountMax);
                    if (count > newlineCount)
                        description = "\n" + description;
                    else
                        description = "\n\n" + description;
                }
                return description;
            }

            public void Save() {
                using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    sw.WriteLine("# [CustomPreset]");
                    sw.WriteLine(string.Format("{0},{1}", "PresetName", presetName));
                    sw.WriteLine(string.Format("{0},{1}", 0, registTime));
                    BasicOptions.Save(optionValueTable, sw);
                    foreach (CustomOption option in CustomOption.options)
                    {
                        if (option.id == 0) continue;
                        int value = option.selection;
                        if (optionValueTable.TryGetValue(option.id, out string v))
                            int.TryParse(v, out value);
                        else
							optionValueTable[option.id] = value.ToString();
                        sw.WriteLine(string.Format("{0},{1}", option.id, value));
                    }
                }
            }

            public void Load() {
                BasicOptions.Load(optionValueTable);
                foreach (CustomOption option in CustomOption.options) {
                    if (option.id == 0) continue;
                    int v = option.defaultSelection;
                    if (optionValueTable.TryGetValue(option.id, out string value))
                        int.TryParse(value, out v);
                    option.updateSelection(v, false);
                }
                CustomOption.ShareOptionSelections();
                CachedPlayer.LocalPlayer.PlayerControl.RpcSyncSettings(GameOptionsData.hostOptionsData);
            }

            public void Rename(string newPresetName) {
                SetPresetName(newPresetName);
                Save();
            }

            public void Delete(bool isSave = true) {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            void SetPresetName(string presetName) {
                this.presetName = !string.IsNullOrEmpty(presetName) ? presetName : "NewPreset";
            }

            static string GetSectionName(string presetName) {
                return $"CustomPreset_{presetName}";
			}

            static string GetFilePathRandom() {
                string dir = Path.GetDirectoryName(Application.dataPath) + @"\CustomPreset\";
                string path;
                do
                {
                    path = dir + Helpers.RandomString(16) + ".csv";
                } while (File.Exists(path));
                return path;
            }

            string filePath;
            Dictionary<int, string> optionValueTable = new();
        }

        const int PresetInfoOnePageViewMax = 4;
        static OptionsMenuBehaviour _instance = null;
        static List<PresetInfo> presetInfoList = new List<PresetInfo>();
        static List<GameObject> presetInfoObjList = new List<GameObject>();
        static int presetInfoPageNow = 0;
        static int presetInfoPageMax = 0;
        static TextMeshPro presetTitle = null;
        static GameObject presetRoot = null;
        static SelectionBehaviour prevPresetPageInfo = null;
        static SelectionBehaviour nextPresetPageInfo = null;
        static SelectionBehaviour createNewPresetInfo = null;
        static GameObject createNewPresetPopUp = null;
        static EditName createNewPresetEditName = null;
        static GameObject renamePresetPopUp = null;
        static EditName renamePresetEditName = null;

        //static Dictionary<BepInEx.Configuration.ConfigDefinition, string> orphanedEntries = null;
        
        static SelectionBehaviourObservable tabObservable = new SelectionBehaviourObservable();
        static SelectionBehaviour optionTabInfo = null;
        static SelectionBehaviour presetTabInfo = null;

        static SelectionBehaviour.InitDesc optionButtonDesc;
        static SelectionBehaviour.InitDesc presetButtonDesc;
        static SelectionBehaviour.InitDesc tabDesc;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
        public static void MainMenuManager_StartPostfix(MainMenuManager __instance) {
            // Prefab for the title
            var tmp = __instance.Announcement.transform.Find("Title_Text").gameObject.GetComponent<TextMeshPro>();
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.transform.localPosition += Vector3.left * 0.2f;
            titleText = Object.Instantiate(tmp);
            Object.Destroy(titleText.GetComponent<TextTranslatorTMP>());
            titleText.gameObject.SetActive(false);
            Object.DontDestroyOnLoad(titleText);
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Start))]
        public static void OptionsMenuBehaviour_StartPostfix(OptionsMenuBehaviour __instance) {
            _instance = __instance;
            if (!__instance.CensorChatButton) return;

            if (!popUp) {
                CreateCustom(__instance);
            }

            if (!buttonPrefab) {
                buttonPrefab = Object.Instantiate(__instance.CensorChatButton);
                Object.DontDestroyOnLoad(buttonPrefab);
                buttonPrefab.name = "CensorChatPrefab";
                buttonPrefab.gameObject.SetActive(false);
            }

            SetUpOptions();
            InitializeMoreButton(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Open))]
        public static void OptionsMenuBehaviour_OpenPostfix(OptionsMenuBehaviour __instance) {
            if (isOpenPreset) {
                __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => {
                    OnMoreButton(__instance);
                })));
            }
        }

        public static void UpdateTranslations() {
            if (moreOptions != null)
                moreOptions.Text.text = ModTranslation.GetString("MainMenu", 18);
            if (optionTitle != null)
                optionTitle.text = ModTranslation.GetString("MainMenu", 18);

            tabObservable.Clear();
            UpdateOptionContents();
            UpdatePresetButtons();
            UpdateOptionTabs();
            UpdatePresetInfo();
        }

        static void CreateCustom(OptionsMenuBehaviour prefab) {
            popUp = Object.Instantiate(prefab.gameObject);
            Object.DontDestroyOnLoad(popUp);
            var transform = popUp.transform;
            var pos = transform.localPosition;
            pos.z = -810f;
            transform.localPosition = pos;

            Object.Destroy(popUp.GetComponent<OptionsMenuBehaviour>());
            foreach (var gObj in popUp.gameObject.GetAllChilds()) {
                switch (gObj.name) {
                    case "Background":
                    case "CloseButton":
                        {
                        }
                        break;
                    default:
                        {
                            Object.Destroy(gObj);
                        }
                        break;
                }
            }

            popUp.SetActive(false);
        }

        static void InitializeMoreButton(OptionsMenuBehaviour __instance) {
            moreOptions = Object.Instantiate(buttonPrefab, __instance.CensorChatButton.transform.parent);
            var transform = __instance.CensorChatButton.transform;
            __instance.CensorChatButton.Text.transform.localScale = new Vector3(1 / 0.66f, 1, 1);
            _origin ??= transform.localPosition;

            transform.localPosition = _origin.Value + Vector3.left * 0.45f;
            transform.localScale = new Vector3(0.66f, 1, 1);
            __instance.EnableFriendInvitesButton.transform.localScale = new Vector3(0.66f, 1, 1);
            __instance.EnableFriendInvitesButton.transform.localPosition += Vector3.right * 0.5f;
            __instance.EnableFriendInvitesButton.Text.transform.localScale = new Vector3(1.2f, 1, 1);

            moreOptions.transform.localPosition = _origin.Value + Vector3.right * 4f / 3f;
            moreOptions.transform.localScale = new Vector3(0.66f, 1, 1);
            moreOptions.gameObject.SetActive(true);
            moreOptions.Text.text = ModTranslation.GetString("MainMenu", 18);
            moreOptions.Text.transform.localScale = new Vector3(1 / 0.66f, 1, 1);
            var moreOptionsButton = moreOptions.GetComponent<PassiveButton>();
            moreOptionsButton.OnClick = new ButtonClickedEvent();
            moreOptionsButton.OnClick.AddListener((Action)(() => {
                OnMoreButton(__instance);
            }));
        }

        static void RefreshOpen() {
            popUp.gameObject.SetActive(false);
            popUp.gameObject.SetActive(true);
            SetUpOptions();
        }

        static void OnMoreButton(OptionsMenuBehaviour __instance) {
            if (!popUp) return;

            if (__instance.transform.parent && __instance.transform.parent == FastDestroyableSingleton<HudManager>.Instance.transform) {
                popUp.transform.SetParent(FastDestroyableSingleton<HudManager>.Instance.transform);
                popUp.transform.localPosition = new Vector3(0, 0, -800f);
            } else {
                popUp.transform.SetParent(null);
                Object.DontDestroyOnLoad(popUp);
            }

            RefreshOpen();
        }

        static void SetUpOptions() {
            if (popUp.transform.GetComponentInChildren<ToggleButtonBehaviour>()) {
                if (isOpenPreset && presetTabInfo != null) {
                    isOpenPreset = false;
                    presetTabInfo.Select();
                }
                if (createNewPresetInfo != null) {
                    createNewPresetInfo.SetActive(AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started);
                }
                UpdatePresetInfo();
                return;
            }
            tabObservable.Clear();

            bool isJapanese = AmongUs.Data.DataManager.Settings.Language.CurrentLanguage == SupportedLangs.Japanese;

            // Tab
            tabDesc = new SelectionBehaviour.InitDesc();
            tabDesc.buttonPrefab = buttonPrefab;
            tabDesc.parent = popUp.transform;
            tabDesc.observable = tabObservable;
            tabDesc.font = titleText.font;
            tabDesc.selectColor = Color.white;
            tabDesc.unselectColor = new Color32(85, 85, 85, byte.MaxValue);
            tabDesc.mouseOverColor = Color.green;
            tabDesc.buttonSize = new Vector2(1f, .5f);
            tabDesc.colliderButtonSize = new Vector2(1f, .5f);

            // Option Tab
            var optionRoot = new GameObject("OptionRoot");
            optionRoot.transform.SetParent(popUp.transform);
            optionRoot.transform.localPosition = Vector3.zero;
            optionRoot.transform.localScale = Vector3.one;
            optionTabInfo = new SelectionBehaviour(new TranslationInfo("MainMenu", 9), () => { return true; }, true, optionRoot);

            // Preset Tab
            presetRoot = new GameObject("PresetRoot");
            presetRoot.transform.SetParent(popUp.transform);
            presetRoot.transform.localPosition = Vector3.zero;
            presetRoot.transform.localScale = Vector3.one;
            presetTabInfo = new SelectionBehaviour(new TranslationInfo("MainMenu", 10), () => { return true; }, false, presetRoot);

            UpdateOptionTabs();
            presetTabInfo.ChangeButtonState(false);

            // Option Title
            optionTitle = Object.Instantiate(titleText, optionRoot.transform);
            optionTitle.GetComponent<RectTransform>().localPosition = new Vector3(0, 1.8f, -.5f);
            optionTitle.gameObject.SetActive(true);
            optionTitle.text = ModTranslation.GetString("MainMenu", 18);
            optionTitle.name = "OptionTitleText";

            // Option Contents
            optionButtonDesc = new SelectionBehaviour.InitDesc();
            optionButtonDesc.buttonPrefab = buttonPrefab;
            optionButtonDesc.parent = optionRoot.transform;
            optionButtonDesc.font = titleText.font;
            UpdateOptionContents();

            // Preset Title
            presetTitle = Object.Instantiate(titleText, presetRoot.transform);
            presetTitle.GetComponent<RectTransform>().localPosition = new Vector3(0, 1.8f, -.5f);
            presetTitle.gameObject.SetActive(true);
            presetTitle.name = "PresetTitleText";

            // Preset Contents
            // List
            presetInfoList.Clear();

            // Create CustomPreset Folder
            string dir = Path.GetDirectoryName(Application.dataPath) + @"\CustomPreset\";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string[] fileNames = Directory.GetFiles(dir, "*.csv");
            foreach (string path in fileNames)
            {
                using (var sr = new StreamReader(path, Encoding.UTF8))
				{
                    TheOtherRolesPlugin.Logger.LogMessage(string.Format("[PRESET CHECK]: {0}", path));
                    var text = sr.ReadLine();
                    if (text == "# [CustomPreset]")
					{
                        string name = sr.ReadLine();
                        if (name.Contains(PresetNameTitle))
						{
                            string presetName = name.Substring(name.IndexOf(PresetNameTitle) + PresetNameTitle.Length);
                            presetInfoList.Add(new PresetInfo(presetName, path));
                            TheOtherRolesPlugin.Logger.LogMessage(string.Format("[PRESET LOADED]: {0},{1}", presetName, path));
                        }
                    }
                }
            }

            presetInfoList.Sort((l, r) => {
                long c = l.registTime - r.registTime;
                if (c > 0) return 1;
                if (c < 0) return -1;
                return 0;
            });

            // Buttons
            presetButtonDesc = new SelectionBehaviour.InitDesc();
            presetButtonDesc.buttonPrefab = buttonPrefab;
            presetButtonDesc.parent = presetRoot.transform;
            presetButtonDesc.font = titleText.font;
            presetButtonDesc.selectColor = Color.white;
            presetButtonDesc.unselectColor = Color.white;
            createNewPresetInfo = new SelectionBehaviour(new TranslationInfo("MainMenu", 11), () => { return OnCreateNewPreset(); });
            prevPresetPageInfo = new SelectionBehaviour(new TranslationInfo("MainMenu", 12), () => { return OnPrevPresetPage(); });
            nextPresetPageInfo = new SelectionBehaviour(new TranslationInfo("MainMenu", 13), () => { return OnNextPresetPage(); });

            UpdatePresetButtons();

            presetInfoPageNow = 1;
            presetInfoPageMax = ((presetInfoList.Count - 1) / PresetInfoOnePageViewMax) + 1;

            UpdatePresetInfo();

            if (isOpenPreset) {
                isOpenPreset = false;
                presetTabInfo.Select();
            }
        }

        static void UpdateOptionContents() {
            if (optionButtonDesc == null)
                return;
            bool isJapanese = AmongUs.Data.DataManager.Settings.Language.CurrentLanguage == SupportedLangs.Japanese;
            optionButtonDesc.fontSize = isJapanese ? 1.5f : 2.5f;
            optionButtonDesc.buttonScale = isJapanese ? 0.9f : 1.0f;
            for (var i = 0; i < AllOptions.Length; i++)
            {
                optionButtonDesc.pos = new Vector3(i % 2 == 0 ? -1.17f : 1.17f, 1.2f - i / 2 * 0.8f, -.5f);
                AllOptions[i].Initialize(optionButtonDesc);
            }
        }

        static void UpdateOptionTabs() {
            if (tabDesc == null)
                return;
            bool isJapanese = AmongUs.Data.DataManager.Settings.Language.CurrentLanguage == SupportedLangs.Japanese;
            tabDesc.fontSize = isJapanese ? 1.5f : 2.5f;

            tabDesc.pos = new Vector3(-.7f, 2.35f, -.5f);
            optionTabInfo.Initialize(tabDesc);
            tabDesc.pos = new Vector3(.7f, 2.35f, -.5f);
            presetTabInfo.Initialize(tabDesc);
        }

        static void UpdatePresetButtons() {
            if (presetButtonDesc == null)
                return;

            bool isJapanese = AmongUs.Data.DataManager.Settings.Language.CurrentLanguage == SupportedLangs.Japanese;
            presetButtonDesc.fontSize = isJapanese ? 2f : 2.5f;
            presetButtonDesc.pos = new Vector3(0f, -2.3f, -.5f);
            presetButtonDesc.buttonSize = new Vector2(2f, .5f);
            presetButtonDesc.colliderButtonSize = new Vector2(2f, .5f);
            createNewPresetInfo.Initialize(presetButtonDesc);
            createNewPresetInfo.SetActive(AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started);

            presetButtonDesc.buttonSize = new Vector2(1f, .5f);
            presetButtonDesc.colliderButtonSize = new Vector2(1f, .5f);
            presetButtonDesc.pos = new Vector3(-.7f, -1.7f, -.5f);
            prevPresetPageInfo.Initialize(presetButtonDesc);

            presetButtonDesc.pos = new Vector3(.7f, -1.7f, -.5f);
            nextPresetPageInfo.Initialize(presetButtonDesc);
        }

        static bool OnCreateNewPreset() {
            if (createNewPresetPopUp) {
                var button = createNewPresetPopUp.transform.FindChild("SubmitButton").GetComponentInChildren<PassiveButton>();
                _instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) => {
                    createNewPresetEditName.nameText.nameSource.SetText("");
                    createNewPresetPopUp.gameObject.SetLayerRecursively(button.gameObject.layer);
                })));
                createNewPresetPopUp.SetActive(true);
                return false;
            }

            createNewPresetPopUp = Object.Instantiate(AccountManager.Instance.accountTab.editNameScreen.gameObject, popUp.transform);
            Object.DontDestroyOnLoad(createNewPresetPopUp);

            var pos = createNewPresetPopUp.transform.localPosition;
            pos.z = -50f;
            createNewPresetPopUp.transform.localPosition = pos;
            createNewPresetPopUp.SetActive(true);

            _instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) => { 
                createNewPresetEditName = createNewPresetPopUp.GetComponentInChildren<EditName>();
                createNewPresetEditName.nameText.nameSource.SetText("");
                createNewPresetEditName.nameText.nameSource.characterLimit = 18;
                var titleText = createNewPresetPopUp.transform.FindChild("TitleText_TMP").GetComponent<TextMeshPro>();
                titleText.SetText(ModTranslation.GetString("MainMenu", 19));
                var nameText = createNewPresetPopUp.transform.FindChild("ChangeNameTitle_TMP").GetComponent<TextMeshPro>();
                nameText.SetText(ModTranslation.GetString("MainMenu", 20));
                var submitText = createNewPresetPopUp.transform.FindChild("SubmitButton").GetComponentInChildren<TextMeshPro>();
                submitText.SetText(ModTranslation.GetString("MainMenu", 21));
                var backText = createNewPresetPopUp.transform.FindChild("BackButton").GetComponentInChildren<TextMeshPro>();
                backText.SetText(ModTranslation.GetString("MainMenu", 22));
                var submitButton = createNewPresetPopUp.transform.FindChild("SubmitButton").GetComponentInChildren<PassiveButton>();
                submitButton.OnClick = new ButtonClickedEvent();
                submitButton.OnClick.AddListener((Action)(() => {
                    if (!createNewPresetPopUp) return;
                    CreateNewPreset(createNewPresetEditName.nameText.nameSource.text);
                    createNewPresetEditName.Close();
                }));
                createNewPresetPopUp.transform.FindChild("RandomizeName").gameObject.SetActive(false);
                createNewPresetPopUp.gameObject.SetLayerRecursively(submitButton.gameObject.layer);
            })));

            return false;
        }

        static bool OnRenamePreset(PresetInfo info) {
            if (renamePresetPopUp) {
                var button = renamePresetPopUp.transform.FindChild("SubmitButton").GetComponentInChildren<PassiveButton>();
                _instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) => {
                    renamePresetEditName.nameText.nameSource.SetText(info.presetName);
                    renamePresetPopUp.gameObject.SetLayerRecursively(button.gameObject.layer);
                })));
                button.OnClick = new ButtonClickedEvent();
                button.OnClick.AddListener((Action)(() => {
                    if (!renamePresetPopUp) return;
                    info.Rename(renamePresetEditName.nameText.nameSource.text);
                    UpdatePresetInfo();
                    renamePresetEditName.Close();
                }));
                renamePresetPopUp.SetActive(true);
                return false;
            }

            renamePresetPopUp = Object.Instantiate(AccountManager.Instance.accountTab.editNameScreen.gameObject, popUp.transform);
            Object.DontDestroyOnLoad(renamePresetPopUp);

            var pos = renamePresetPopUp.transform.localPosition;
            pos.z = -50f;
            renamePresetPopUp.transform.localPosition = pos;
            renamePresetPopUp.SetActive(true);

            _instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) => {
                renamePresetEditName = renamePresetPopUp.GetComponentInChildren<EditName>();
                renamePresetEditName.nameText.nameSource.SetText(info.presetName);
                renamePresetEditName.nameText.nameSource.characterLimit = 18;
                var titleText = renamePresetPopUp.transform.FindChild("TitleText_TMP").GetComponent<TextMeshPro>();
                titleText.SetText(ModTranslation.GetString("MainMenu", 23));
                var nameText = renamePresetPopUp.transform.FindChild("ChangeNameTitle_TMP").GetComponent<TextMeshPro>();
                nameText.SetText(ModTranslation.GetString("MainMenu", 24));
                var submitText = renamePresetPopUp.transform.FindChild("SubmitButton").GetComponentInChildren<TextMeshPro>();
                submitText.SetText(ModTranslation.GetString("MainMenu", 25));
                var backText = renamePresetPopUp.transform.FindChild("BackButton").GetComponentInChildren<TextMeshPro>();
                backText.SetText(ModTranslation.GetString("MainMenu", 26));
                var submitButton = renamePresetPopUp.transform.FindChild("SubmitButton").GetComponentInChildren<PassiveButton>();
                submitButton.OnClick = new ButtonClickedEvent();
                submitButton.OnClick.AddListener((Action)(() => {
                    if (!renamePresetPopUp) return;
                    info.Rename(renamePresetEditName.nameText.nameSource.text);
                    UpdatePresetInfo();
                    renamePresetEditName.Close();
                }));
                renamePresetPopUp.transform.FindChild("RandomizeName").gameObject.SetActive(false);
                renamePresetPopUp.gameObject.SetLayerRecursively(submitButton.gameObject.layer);
            })));

            return false;
        }

        static bool OnPrevPresetPage() {
            if (presetInfoPageMax > 0) {
                if (--presetInfoPageNow <= 0)
                    presetInfoPageNow = presetInfoPageMax;
                UpdatePresetInfo();
            }
            return false;
        }

        static bool OnNextPresetPage() {
            if (presetInfoPageMax > 0) {
                if (++presetInfoPageNow > presetInfoPageMax)
                    presetInfoPageNow = 1;
                UpdatePresetInfo();
            }
            return false;
        }

        static void CreateNewPreset(string name) {
            long registTime = DateTime.Now.Ticks;
            var presetInfo = new PresetInfo(name);
            presetInfoList.Add(presetInfo);
            presetInfoPageNow = presetInfoPageMax = ((presetInfoList.Count - 1) / PresetInfoOnePageViewMax) + 1;

            presetInfo.SetRegistTime(registTime);
            presetInfo.Save();
            UpdatePresetInfo();
        }

        static void UpdatePresetInfo() {
            if (buttonPrefab == null || presetRoot == null) return;

            for (int i = 0; i < presetInfoObjList.Count; ++i) {
                if (presetInfoObjList[i] != null)
                    GameObject.Destroy(presetInfoObjList[i]);
            }
            presetInfoObjList.Clear();
            bool isJapanese = AmongUs.Data.DataManager.Settings.Language.CurrentLanguage == SupportedLangs.Japanese;

            SelectionBehaviour.InitDesc desc = new SelectionBehaviour.InitDesc();
            desc.buttonPrefab = buttonPrefab;
            desc.parent = presetRoot.transform;
            desc.font = titleText.font;
            if (isJapanese)
                desc.fontSize = 1.5f;
            desc.selectColor = Color.white;
            desc.unselectColor = Color.white;
            desc.mouseOverColor = Color.white;
            desc.buttonSize = new Vector2(5f, .6f);
            desc.colliderButtonSize = new Vector2(5f, .6f);
            desc.textAlignment = TextAlignmentOptions.MidlineLeft;
            desc.textOffset = new Vector2(.1f, -.05f);

            SelectionBehaviour.InitDesc subButtonDesc = new SelectionBehaviour.InitDesc();
            subButtonDesc.buttonPrefab = buttonPrefab;
            subButtonDesc.font = titleText.font;
            subButtonDesc.mouseOverColor = Color.green;
            subButtonDesc.buttonSize = new Vector2(.63f, .3f);
            subButtonDesc.colliderButtonSize = new Vector2(.63f, .3f);

            for (int i = 0; i < PresetInfoOnePageViewMax; ++i) {
                int idx = (presetInfoPageNow - 1) * PresetInfoOnePageViewMax + i;
                if (idx >= presetInfoList.Count)
                    break;
                var info = presetInfoList[idx];
                var presetInfo = new SelectionBehaviour(new TranslationInfo(info.presetName), () => { return false; });
                desc.pos = new Vector3(0f, 1.18f - i * .75f, -.5f);
                if (isJapanese)
                    desc.buttonName = string.Format("{0}<size=65%>{1}", info.presetName, info.GetDescription(3));
                else
                    desc.buttonName = string.Format("{0}\n<size=70%>{1}", info.presetName, info.GetDescription());
                presetInfo.Initialize(desc);
                presetInfoObjList.Add(presetInfo._transform.gameObject);

                if (AmongUsClient.Instance.AmHost && AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) {
                    subButtonDesc.parent = presetInfo._transform;
                    subButtonDesc.pos = new Vector3(.6f, .1f, -5f);
                    subButtonDesc.selectColor = Color.yellow;
                    subButtonDesc.unselectColor = subButtonDesc.selectColor;
                    var loadButtonInfo = new SelectionBehaviour(new TranslationInfo("MainMenu", 14), () => {
                        info.Load();
                        return false;
                    });
                    loadButtonInfo.Initialize(subButtonDesc);
                }

                if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) {
                    subButtonDesc.parent = presetInfo._transform;
                    subButtonDesc.pos = new Vector3(1.3f, .1f, -5f);
                    subButtonDesc.selectColor = Color.cyan;
                    subButtonDesc.unselectColor = subButtonDesc.selectColor;
                    var renameButtonInfo = new SelectionBehaviour(new TranslationInfo("MainMenu", 15), () => {
                        OnRenamePreset(info);
                        return false;
                    });
                    renameButtonInfo.Initialize(subButtonDesc);
                }

                subButtonDesc.parent = presetInfo._transform;
                subButtonDesc.pos = new Vector3(2.0f, .1f, -5f);
                subButtonDesc.selectColor = new Color32(235, 76, 70, 0xff);
                subButtonDesc.unselectColor = subButtonDesc.selectColor;
                var deleteButtonInfo = new SelectionBehaviour(new TranslationInfo("MainMenu", 16), () => {
                    info.Delete();
                    presetInfoList.Remove(info);
                    presetInfoPageMax = ((presetInfoList.Count - 1) / PresetInfoOnePageViewMax) + 1;
                    presetInfoPageNow = Mathf.Min(presetInfoPageNow, presetInfoPageMax);
                    UpdatePresetInfo();
                    return false;
                });
                deleteButtonInfo.Initialize(subButtonDesc);
            }

            prevPresetPageInfo._transform.gameObject.SetActive(presetInfoPageMax > 1);
            nextPresetPageInfo._transform.gameObject.SetActive(presetInfoPageMax > 1);
            presetTitle.text = String.Format(ModTranslation.GetString("MainMenu", 27), presetInfoPageNow, presetInfoPageMax);
        }

        static IEnumerable<GameObject> GetAllChilds(this GameObject Go) {
            for (var i = 0; i < Go.transform.childCount; i++) {
                yield return Go.transform.GetChild(i).gameObject;
            }
        }

        static void SetLayerRecursively(this GameObject self, int layer) {
            self.layer = layer;
            for (int i = 0; i < self.transform.childCount; ++i)
                self.transform.GetChild(i).gameObject.SetLayerRecursively(layer);
        }
    }
}
