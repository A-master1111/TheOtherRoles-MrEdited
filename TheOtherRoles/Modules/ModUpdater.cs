using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using AmongUs.Data;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Unity.IL2CPP;
using BepInEx.Unity.IL2CPP.Utils;
using Mono.Cecil;
using Newtonsoft.Json.Linq;
using TMPro;
using Twitch;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Action = System.Action;
using IntPtr = System.IntPtr;
using Version = SemanticVersioning.Version;

namespace TheOtherRoles.Modules 
{
    public class ModUpdateBehaviour : MonoBehaviour
    {
        public static readonly bool CheckForSubmergedUpdates = true;
        public static bool showPopUp = true;
        public static bool updateInProgress = false;

        public static AudioClip selectSfx = null;

        public static ModUpdateBehaviour Instance { get; private set; }
        public ModUpdateBehaviour(IntPtr ptr) : base(ptr) { }
        public class UpdateData
        {
            public JObject Request;
            public string Version { get { return ver != null ? "v" + ver : Tag; } }
            public string Content { get { return DataManager.Settings.Language.CurrentLanguage == SupportedLangs.Japanese ? ContentJp : ContentDefault; } }

            string ContentDefault;
            string ContentJp;
            string Tag;
            Version ver;

            public UpdateData(JObject data)
            {
                var t = data["tag_name"]?.ToString();
                int p = t.IndexOf('v');
                Tag = t.Substring(p != -1 ? p + 1 : 0);
                if (!SemanticVersioning.Version.TryParse(Tag, out ver))
                    ver = null;
                Request = data;

                ContentDefault = data["body"]?.ToString();
                var content = ContentDefault;
                int jpTagStartIndex = content.IndexOf("### JP");
                int jpTagEndIndex = jpTagStartIndex != -1 ? jpTagStartIndex + "### JP".Length + 2 : -1;
                int tagStartIndex = content.IndexOf("### EN");
                int tagEndIndex = tagStartIndex != -1 ? tagStartIndex + "### EN".Length + 2 : -1;

                if (jpTagStartIndex == -1 && tagStartIndex == -1) {
                    ContentDefault = ContentJp = content;
                } else if (jpTagStartIndex != -1 && tagStartIndex == -1) {
                    ContentDefault = ContentJp = content.Substring(tagEndIndex);
                } else if (jpTagStartIndex == -1 && tagStartIndex != -1) {
                    ContentDefault = ContentJp = content.Substring(jpTagEndIndex);
                } else {
                    if (jpTagStartIndex < tagEndIndex) {
                        ContentJp = content.Substring(jpTagEndIndex, tagStartIndex - jpTagEndIndex);
                        ContentDefault = content.Substring(tagEndIndex);
                    } else {
                        ContentDefault = content.Substring(tagEndIndex, jpTagStartIndex - tagEndIndex);
                        ContentJp = content.Substring(jpTagEndIndex);
                    }
                }
            }

            public bool IsNewer(Version version)
            {
                if (ver == null) return false;
                return ver > version;
            }
        }

        public UpdateData TORUpdate;
        public UpdateData SubmergedUpdate;

        [HideFromIl2Cpp]
        public UpdateData RequiredUpdateData => TORUpdate ?? SubmergedUpdate;
        
        public void Awake()
        {
            if (Instance) Destroy(this);
            Instance = this;
            
            SceneManager.add_sceneLoaded((System.Action<Scene, LoadSceneMode>) (OnSceneLoaded));
            this.StartCoroutine(CoCheckUpdates());
            
            foreach (var file in Directory.GetFiles(Paths.PluginPath, "*.old"))
            {
                File.Delete(file);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (updateInProgress || scene.name != "MainMenu") return;
            if (selectSfx == null)
                selectSfx = AccountManager.Instance.accountTab.resendEmailButton.GetComponent<PassiveButton>().ClickSound;

            if (RequiredUpdateData is null) {
                showPopUp = false;
                return;
            }

            var template = GameObject.Find("ExitGameButton");
            if (!template) return;
            
            var button = Instantiate(template, null);
            var buttonTransform = button.transform;
            var pos = buttonTransform.localPosition;
            pos.y += 1.2f;
            buttonTransform.localPosition = pos;

            PassiveButton passiveButton = button.GetComponent<PassiveButton>();
            SpriteRenderer buttonSprite = button.GetComponent<SpriteRenderer>();
            passiveButton.OnClick = new Button.ButtonClickedEvent();
            passiveButton.OnClick.AddListener((Action) (() =>
            {
                this.StartCoroutine(CoUpdate());
                button.SetActive(false);
            }));

            var text = button.transform.GetChild(0).GetComponent<TMP_Text>();
            string t = ModTranslation.GetString("Mod-Updater", 1);
            if (TORUpdate is null && SubmergedUpdate is not null) t = SubmergedCompatibility.Loaded ? ModTranslation.GetString("Mod-Updater", 2) : ModTranslation.GetString("Mod-Updater", 3);

            StartCoroutine(Effects.Lerp(0.1f, (System.Action<float>)(p => text.SetText(t))));

            buttonSprite.color = text.color = Color.blue;
            passiveButton.OnMouseOut.AddListener((Action)(() => buttonSprite.color = text.color = Color.blue));

            var isSubmerged = TORUpdate == null;
            var mgr = FindObjectOfType<MainMenuManager>(true);

            if (!isSubmerged) {
                try {
                    string updateVersion = TORUpdate.Content[^5..];
                    if (Version.Parse(TheOtherRolesPlugin.VersionString).BaseVersion() < Version.Parse(updateVersion).BaseVersion()) {
                        passiveButton.OnClick.RemoveAllListeners();
                        passiveButton.OnClick = new Button.ButtonClickedEvent();
                        passiveButton.OnClick.AddListener((Action)(() => {
                            mgr.StartCoroutine(CoShowAnnouncement(ModTranslation.GetString("Mod-Updater", 4)));
                        }));
                    }
                } catch {  
                    TheOtherRolesPlugin.Logger.LogError("parsing version for auto updater failed :(");
                }

            }

            if (isSubmerged && !SubmergedCompatibility.Loaded) showPopUp = false;
            if (showPopUp) {
                var data = isSubmerged ? SubmergedUpdate : TORUpdate;

                var announcement = string.Format(ModTranslation.GetString("Mod-Updater", 5), isSubmerged ? ModTranslation.GetString("Opt-General", 68) : "THE OTHER ROLES MR", data.Version, data.Content);
                mgr.StartCoroutine(CoShowAnnouncement(announcement));
            }
            showPopUp = false;
        }
        
        [HideFromIl2Cpp]
        public IEnumerator CoUpdate()
        {
            updateInProgress = true;
            var isSubmerged = TORUpdate is null;
            var updateName = (isSubmerged ? ModTranslation.GetString("Opt-General", 68) : "The Other Roles MR");
            
            var popup = Instantiate(TwitchManager.Instance.TwitchPopup);
            popup.TextAreaTMP.fontSize *= 0.7f;
            popup.TextAreaTMP.enableAutoSizing = false;
            
            popup.Show();

            var button = popup.transform.GetChild(2).gameObject;
            button.SetActive(false);
            popup.TextAreaTMP.text = string.Format(ModTranslation.GetString("Mod-Updater", 6), updateName);
            
            var download = Task.Run(DownloadUpdate);
            while (!download.IsCompleted) yield return null;
            
            button.SetActive(true);
            popup.TextAreaTMP.text = download.Result ? string.Format(ModTranslation.GetString("Mod-Updater", 7), updateName) : ModTranslation.GetString("Mod-Updater", 8);
        }

        [HideFromIl2Cpp]
        public IEnumerator CoShowAnnouncement(string announcement)
        {
            var popUp = Instantiate(FindObjectOfType<AnnouncementPopUp>(true));
            popUp.gameObject.SetActive(true);
            yield return popUp.Init();
            var last = DataManager.Announcements.LastViewedAnnouncement;
            last.Id = 1;
            last.Text = announcement;
            SelectableHyperLinkHelper.DestroyGOs(popUp.selectableHyperLinks, name);
            popUp.AnnounceTextMeshPro.enableAutoSizing = true;
            popUp.AnnounceTextMeshPro.text = announcement;
        }

        [HideFromIl2Cpp]
        public static IEnumerator CoCheckUpdates()
        {
            var torUpdateCheck = Task.Run(() => Instance.GetGithubUpdate("ÈÕÂä", "TheOtherRoles-MR"));
            while (!torUpdateCheck.IsCompleted) yield return null;
            Announcement.updateData = torUpdateCheck.Result;
            if (torUpdateCheck.Result != null && torUpdateCheck.Result.IsNewer(Version.Parse(TheOtherRolesPlugin.VersionString)))
            {
                Instance.TORUpdate = torUpdateCheck.Result;
            }

            if (CheckForSubmergedUpdates)
            {
                var submergedUpdateCheck = Task.Run(() => Instance.GetGithubUpdate("SubmergedAmongUs", "Submerged"));
                while (!submergedUpdateCheck.IsCompleted) yield return null;
                if (submergedUpdateCheck.Result != null && (!SubmergedCompatibility.Loaded || submergedUpdateCheck.Result.IsNewer(SubmergedCompatibility.Version)))
                {
                    Instance.SubmergedUpdate = submergedUpdateCheck.Result;
                }
            }
            
            Instance.OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        [HideFromIl2Cpp]
        public async Task<UpdateData> GetGithubUpdate(string owner, string repo)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "TheOtherRoles MR Updater");

            var req = await client.GetAsync($"https://api.github.com/repos/{owner}/{repo}/releases/latest", HttpCompletionOption.ResponseContentRead);
            if (!req.IsSuccessStatusCode) return null;

            var dataString = await req.Content.ReadAsStringAsync();
            JObject data = JObject.Parse(dataString);
            return new UpdateData(data);
        }

        private bool TryUpdateSubmergedInternally()
        {
            if (SubmergedUpdate == null) return false;
            try
            {
                if (!SubmergedCompatibility.LoadedExternally) return false;
                var thisAsm = Assembly.GetCallingAssembly();
                var resourceName = thisAsm.GetManifestResourceNames().FirstOrDefault(s => s.EndsWith("Submerged.dll"));
                if (resourceName == default) return false;

                using var submergedStream = thisAsm.GetManifestResourceStream(resourceName)!;
                var asmDef = AssemblyDefinition.ReadAssembly(submergedStream, TypeLoader.ReaderParameters);
                var pluginType = asmDef.MainModule.Types.FirstOrDefault(t => t.IsSubtypeOf(typeof(BasePlugin)));
                var info = IL2CPPChainloader.ToPluginInfo(pluginType, "");
                if (SubmergedUpdate.IsNewer(info.Metadata.Version)) return false;
                File.Delete(SubmergedCompatibility.Assembly.Location);

            }
            catch (Exception e)
            {
                TheOtherRolesPlugin.Logger.LogError(e);
                return false;
            }
            return true;
        }
            
        
        [HideFromIl2Cpp]
        public async Task<bool> DownloadUpdate()
        {
            var isSubmerged = TORUpdate is null;
            if (isSubmerged && TryUpdateSubmergedInternally()) return true;
            var data = isSubmerged ? SubmergedUpdate : TORUpdate;
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "TheOtherRoles MR Updater");
            
            JToken assets = data.Request["assets"];
            string downloadURI = "";
            for (JToken current = assets.First; current != null; current = current.Next) 
            {
                string browser_download_url = current["browser_download_url"]?.ToString();
                if (browser_download_url != null && current["content_type"] != null) {
                    if (current["content_type"].ToString().Equals("application/x-msdownload") &&
                        browser_download_url.EndsWith(".dll")) {
                        downloadURI = browser_download_url;
                        break;
                    }
                }
            }

            if (downloadURI.Length == 0) return false;

            var res = await client.GetAsync(downloadURI, HttpCompletionOption.ResponseContentRead);
            string filePath = Path.Combine(Paths.PluginPath, isSubmerged ? "Submerged.dll" : "TheOtherRoles.dll");
            if (File.Exists(filePath + ".old")) File.Delete(filePath + ".old");
            if (File.Exists(filePath)) File.Move(filePath, filePath + ".old");

            await using var responseStream = await res.Content.ReadAsStreamAsync();
            await using var fileStream = File.Create(filePath);
            await responseStream.CopyToAsync(fileStream);

            return true;
        }
    }
}
