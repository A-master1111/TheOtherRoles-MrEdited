using HarmonyLib;
using System;
using static TheOtherRoles.TheOtherRoles;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TheOtherRoles.Utilities;

namespace TheOtherRoles.Patches {
    class TaskMasterTaskHelper
    {
        static int taskMasterAddCommonTasks = 0;
        static int taskMasterAddLongTasks = 0;
        static int taskMasterAddShortTasks = 0;

        public static int GetTaskMasterTasks()
        {
            return taskMasterAddCommonTasks > 0 ? taskMasterAddCommonTasks : Mathf.RoundToInt(CustomOptionHolder.taskMasterExtraCommonTasks.getFloat()) +
                   taskMasterAddLongTasks > 0 ? taskMasterAddLongTasks : Mathf.RoundToInt(CustomOptionHolder.taskMasterExtraLongTasks.getFloat()) +
                   taskMasterAddShortTasks > 0 ? taskMasterAddShortTasks : Mathf.RoundToInt(CustomOptionHolder.taskMasterExtraShortTasks.getFloat());
        }

        public static byte[] GetTaskMasterTasks(PlayerControl pc)
        {
            if (!TaskMaster.isTaskMaster(pc.PlayerId))
                return null;

            List<byte> list = new List<byte>(10);
            taskMasterAddCommonTasks = SetTasksToList(
                ref list,
                MapUtilities.CachedShipStatus.CommonTasks.ToList(),
                Mathf.RoundToInt(CustomOptionHolder.taskMasterExtraCommonTasks.getFloat()));
            taskMasterAddLongTasks = SetTasksToList(
                ref list,
                MapUtilities.CachedShipStatus.LongTasks.ToList(),
                Mathf.RoundToInt(CustomOptionHolder.taskMasterExtraLongTasks.getFloat()));
            taskMasterAddShortTasks = SetTasksToList(
                ref list,
                MapUtilities.CachedShipStatus.NormalTasks.ToList(),
                Mathf.RoundToInt(CustomOptionHolder.taskMasterExtraShortTasks.getFloat()));

            return list.ToArray();
        }

        private static int SetTasksToList(
            ref List<byte> list,
            List<NormalPlayerTask> playerTasks,
            int numConfiguredTasks)
        {
            if (numConfiguredTasks == 0)
                return 0;
            List<TaskTypes> taskTypesList = new List<TaskTypes>();
            playerTasks.Shuffle();
            int count = 0;
            int numTasks = Math.Min(playerTasks.Count, numConfiguredTasks);
            for (int i = 0; i < playerTasks.Count; i++) {
                if (taskTypesList.Contains(playerTasks[i].TaskType))
                    continue;
                taskTypesList.Add(playerTasks[i].TaskType);
                ++count;
                list.Add((byte)playerTasks[i].Index);
                if (count >= numTasks)
                    break;
            }
            return count; 
        }
    }
}

