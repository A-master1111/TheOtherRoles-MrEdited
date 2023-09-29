using HarmonyLib;
using System.Reflection;
using UnityEngine;
using System.IO;
using System.Collections.Generic;


namespace TheOtherRoles
{

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public class MODCursor
    {
        public static Dictionary<string, Sprite> CachedSprites = new();
        private static void Prefix(MainMenuManager __instance)
        {
            Sprite sprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.Cursor.png", 115f);
            Cursor.SetCursor(sprite.texture, Vector2.zero, CursorMode.Auto);
        }
    }
}
