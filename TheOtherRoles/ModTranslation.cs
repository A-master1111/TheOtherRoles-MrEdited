using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using TheOtherRoles.Patches;
using UnityEngine;


namespace TheOtherRoles
{
	public class TranslationInfo
	{
        public TranslationInfo(string text)
            : this(text, Color.white)
        {
        }

        public TranslationInfo(string text, Color color)
		{
            this.text = text;
            this.color = color;
        }

        public TranslationInfo(string category, int id)
            : this(category, id, Color.white)
        {
        }

        public TranslationInfo(string category, int id, Color color)
		{
            this.category = category;
            this.id = id;
            this.color = color;
        }

        public TranslationInfo(RoleId roleId)
            : this(roleId, Color.white)
        {
        }

        public TranslationInfo(RoleId roleId, Color color)
        {
            this.roleId = roleId;
            this.color = color;
        }

        public void AddHeadText(string text)
		{
            headText = text;
        }
        public void AddTailText(string text)
        {
            tailText = text;
        }

        public string GetString()
		{
            if (!string.IsNullOrEmpty(text))
                return Helpers.cs(color, headText + text + tailText);
            if (roleId != RoleId.Max)
                return Helpers.cs(color, headText + ModTranslation.GetRoleName(roleId, color) + tailText);
            return Helpers.cs(color, headText + ModTranslation.GetString(category, id) + tailText);
		}

        public override string ToString()
        {
            return GetString();
        }

        string headText;
        string text;
        string tailText;
        string category;
        int id;
        Color color;
        RoleId roleId = RoleId.Max;
    }


	
    public class ModTranslation
    {
        // Dictionary<Category, Dictionary<Category-Id, Dictionary<Lang-Id, Str>>>
        public static Dictionary<string, Dictionary<int, Dictionary<int, string>>> stringTable;

        public static void Load()
        {
            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("TheOtherRoles.Resources.stringData.json");
            var byteArray = new byte[stream.Length];
            var read = stream.Read(byteArray, 0, (int)stream.Length);
            string json = System.Text.Encoding.UTF8.GetString(byteArray);
            stringTable = new();
            JObject parsed = JObject.Parse(json);

            for (int i = 0; i < parsed.Count; i++)
            {
                JProperty token = parsed.ChildrenTokens[i].TryCast<JProperty>();
                if (token == null) continue;
                var val = token.Value.TryCast<JObject>();
                if (token.HasValues)
                {
                    string categoryStr = token.Name;
                    int index = categoryStr.IndexOf(",");
                    string categoryName = categoryStr.Substring(0, index);
                    int categoryId = int.Parse(categoryStr.Substring(index + 1));
                    
                    if (!stringTable.TryGetValue(categoryName, out var t))
					{
						t = new();
                        stringTable.Add(categoryName, t);
                    }

                    var strings = new Dictionary<int, string>();
                    for (int j = 0; j < (int)SupportedLangs.Irish + 1; j++)
                    {
                        string key = j.ToString();
                        var text = val[key]?.TryCast<JValue>().Value.ToString();

                        if (text != null && text.Length > 0)
                        {
                            if (text == blankText) strings[j] = "";
                            else strings[j] = text;
                        }
                    }

                    t[categoryId] = strings;
                }
            }
            //TheOtherRolesPlugin.Instance.Log.LogMessage($"Language: {stringTable.Keys}");
        }

        public static string GetString(string category, int id, string def = null)
        {
            TheOtherRolesPlugin.Instance.Log.LogMessage($"category:{category}, id:{id}, def:{def}");
            if (!stringTable.TryGetValue(category, out var t))
                return def;
            if (!t.TryGetValue(id, out var t2))
                return def;
            int langId = (int)AmongUs.Data.DataManager.Settings.Language.CurrentLanguage;
            if (t2.ContainsKey(langId))
                return t2[langId];
            else if (t2.ContainsKey(defaultLangId))
                return t2[defaultLangId];

            return def;
        }

        public static TranslationInfo GetRoleName(RoleId roleId, Color? color = null)
        {
            return new TranslationInfo("Role-Name", GetRoleStringId(roleId), color.HasValue ? color.Value : Color.white);
        }

        public static TranslationInfo GetRoleIntroDesc(RoleId roleId, Color? color = null)
        {
            return new TranslationInfo("Role-IntroDesc", GetRoleStringId(roleId), color.HasValue ? color.Value : Color.white);
        }

        public static TranslationInfo GetRoleShortDesc(RoleId roleId, Color? color = null)
        {
            return new TranslationInfo("Role-ShortDesc", GetRoleStringId(roleId), color.HasValue ? color.Value : Color.white);
        }

        static int GetRoleStringId(RoleId roleId)
		{
            int id = -1;
            switch (roleId)
			{
                case RoleId.Jester:        id = 1; break;
                case RoleId.Mayor:         id = 2; break;
                case RoleId.Portalmaker:   id = 3; break;
                case RoleId.Engineer:      id = 4; break;
                case RoleId.Sheriff:       id = 5; break;
                case RoleId.Deputy:        id = 6; break;
                case RoleId.Lighter:       id = 7; break;
                case RoleId.Godfather:     id = 8; break;
                case RoleId.Mafioso:       id = 9; break;
                case RoleId.Janitor:       id = 10; break;
                case RoleId.Detective:     id = 11; break;
                case RoleId.TimeMaster:    id = 12; break;
                case RoleId.Medic:         id = 13; break;
                case RoleId.Swapper:       id = 14; break;
                case RoleId.Seer:          id = 15; break;
                case RoleId.Morphling:     id = 16; break;
                case RoleId.Camouflager:   id = 17; break;
                case RoleId.EvilHacker:    id = 18; break;
                case RoleId.Hacker:        id = 19; break;
                case RoleId.Tracker:       id = 20; break;
                case RoleId.Vampire:       id = 21; break;
                case RoleId.Snitch:        id = 22; break;
                case RoleId.Jackal:        id = 23; break;
                case RoleId.Sidekick:      id = 24; break;
                case RoleId.Eraser:        id = 25; break;
                case RoleId.Spy:           id = 26; break;
                case RoleId.Trickster:     id = 27; break;
                case RoleId.Cleaner:       id = 28; break;
                case RoleId.Warlock:       id = 29; break;
                case RoleId.SecurityGuard: id = 30; break;
                case RoleId.Arsonist:      id = 31; break;
                case RoleId.EvilGuesser:   id = 32; break;
                case RoleId.NiceGuesser:   id = 33; break;
                case RoleId.BountyHunter:  id = 34; break;
                case RoleId.Vulture:       id = 35; break;
                case RoleId.Medium:        id = 36; break;
                case RoleId.Trapper:       id = 37; break;
                case RoleId.Madmate:       id = 38; break;
                case RoleId.Lawyer:        id = 39; break;
                case RoleId.Prosecutor:    id = 40; break;
                case RoleId.Pursuer:       id = 41; break;
                case RoleId.Witch:         id = 42; break;
                case RoleId.Ninja:         id = 43; break;
                case RoleId.Thief:         id = 44; break;
                case RoleId.EvilYasuna:    id = 45; break;
                case RoleId.Yasuna:        id = 46; break;
                case RoleId.YasunaJr:      id = 47; break;
                case RoleId.TaskMaster:    id = 48; break;
                case RoleId.DoorHacker:    id = 49; break;
                case RoleId.Kataomoi:      id = 50; break;
                case RoleId.KillerCreator: id = 51; break;
                case RoleId.MadmateKiller: id = 52; break;
                case RoleId.Crewmate:      id = 53; break;
                case RoleId.Impostor:      id = 54; break;
                case RoleId.Lover:         id = 55; break;
                case RoleId.Bait:          id = 56; break;
                case RoleId.Bloody:        id = 57; break;
                case RoleId.AntiTeleport:  id = 58; break;
                case RoleId.Tiebreaker:    id = 59; break;
                case RoleId.Sunglasses:    id = 60; break;
                case RoleId.Mini:          id = 61; break;
                case RoleId.Vip:           id = 62; break;
                case RoleId.Invert:        id = 63; break;
                case RoleId.Chameleon:     id = 64; break;
                case RoleId.Shifter:       id = 65; break;
                case RoleId.TaskRacer:     id = 66; break;
                case RoleId.Hunter:        id = 10000; break;
                case RoleId.Hunted:        id = 10001; break;
            }

            return id;
		}

        const string blankText = "[BLANK]";
        const int defaultLangId = (int)SupportedLangs.English;
    }

    [HarmonyPatch(typeof(LanguageSetter), nameof(LanguageSetter.SetLanguage))]
    class SetLanguagePatch
    {
        static void Postfix()
        {
            ClientOptionsPatch.UpdateTranslations();
        }
    }
}
