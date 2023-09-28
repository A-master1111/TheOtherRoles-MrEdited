using System.Collections.Generic;
using BepInEx.Configuration;

namespace TheOtherRoles
{
    public static class TaskVsMode
    {
        public static ulong GetRecordTime()
        {
            if (!CustomOptionHolder.enabledTaskVsMode.getBool())
                return ulong.MaxValue;

            int key = CustomOptionHolder.taskVsMode_EnabledBurgerMakeMode.getBool() ? GetBurgerRecordKey() : GetRecordKey();
            return Get(key).Get();
        }

        public static void SetRecordTime(ulong time)
        {
            if (!CustomOptionHolder.enabledTaskVsMode.getBool())
                return;

            int key = CustomOptionHolder.taskVsMode_EnabledBurgerMakeMode.getBool() ? GetBurgerRecordKey() : GetRecordKey();
            Get(key).Set(time);
        }

        static int GetRecordKey()
        {
            var optionData = AmongUsClient.Instance.AmHost ? GameOptionsData.hostOptionsData : GameOptionsData.GameHostOptions;
            return optionData.MapId * 1000000 + PlayerControl.GameOptions.NumLongTasks * 10000 + PlayerControl.GameOptions.NumCommonTasks * 100 + PlayerControl.GameOptions.NumShortTasks;
        }

        static int GetBurgerRecordKey()
        {
            return BurgerMakeModeBaseKey + CustomOptionHolder.taskVsMode_BurgerMakeMode_MakeBurgerNums.getInt() * 1000 + CustomOptionHolder.taskVsMode_BurgerMakeMode_BurgerLayers.getInt();
        }

        static RecordData Get(int key)
        {
            if (!burgerRecordSaveDataTable.TryGetValue(key, out var data))
                data = new RecordData(key);
            return data;
        }

        class RecordData
        {
            public RecordData(int key)
            {
                entry = TheOtherRolesPlugin.Instance.Config.Bind<ulong>($"TaskVsMode_Record", key.ToString(), ulong.MaxValue);
            }

            public ulong Get()
            {
                return entry != null ? entry.Value : ulong.MaxValue;
            }

            public void Set(ulong time)
            {
                if (entry != null)
                    entry.Value = time;
            }

            ConfigEntry<ulong> entry = null;
        }
        static Dictionary<int, RecordData> burgerRecordSaveDataTable = new();
        static int BurgerMakeModeBaseKey = 100000000;
    }
}
