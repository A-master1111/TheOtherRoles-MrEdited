using HarmonyLib;
using System.Linq;
using System.Collections.Generic;
using TheOtherRoles.Utilities;

namespace TheOtherRoles.Patches {
    [HarmonyPatch(typeof(NormalPlayerTask), nameof(NormalPlayerTask.PickRandomConsoles))]
    class NormalPlayerTaskPatch {
        static void Postfix(NormalPlayerTask __instance, TaskTypes taskType, byte[] consoleIds) {
            if (!CustomOptionHolder.enableRandomizationInFixWiringTask.getBool() || taskType != TaskTypes.FixWiring) {
                return;
            }
            List<Console> list = (from t in MapUtilities.CachedShipStatus.AllConsoles
                                          where t.TaskTypes.Contains(taskType)
                                          select t).ToList<Console>();
            List<Console> list2 = new List<Console>(list);
            for (int i = 0; i < __instance.Data.Length; i++) {
                int index = UnityEngine.Random.Range(0, list2.Count<Console>());
                __instance.Data[i] = (byte)list2[index].ConsoleId;
                list2.RemoveAt(index);
            }
        }
    }

}