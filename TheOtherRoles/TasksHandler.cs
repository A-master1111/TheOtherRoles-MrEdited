using HarmonyLib;
using System;
using TheOtherRoles.Utilities;
using Hazel;
using TheOtherRoles.Patches;
using TheOtherRoles.Players;

namespace TheOtherRoles {
    [HarmonyPatch]
    public static class TasksHandler {

        public static Tuple<int, int> taskInfo(GameData.PlayerInfo playerInfo, bool madmateCount = false, bool isResult = false) {
            int TotalTasks = 0;
            int CompletedTasks = 0;
            bool isMadmate = madmateCount && playerInfo.Object == Madmate.madmate && CachedPlayer.LocalPlayer.PlayerControl == Madmate.madmate;
            if (!playerInfo.Disconnected && playerInfo.Tasks != null &&
                playerInfo.Object &&
                playerInfo.Role && playerInfo.Role.TasksCountTowardProgress &&
                (playerInfo.Object != Madmate.madmate || isMadmate) &&
                !playerInfo.Object.hasFakeTasks()
                ) {
                bool isOldTaskMasterEx = TaskMaster.taskMaster && TaskMaster.oldTaskMasterPlayerId == playerInfo.PlayerId;
                bool isTaskMasterEx = TaskMaster.taskMaster && TaskMaster.taskMaster == playerInfo.Object && TaskMaster.isTaskComplete;
                if (isOldTaskMasterEx || (!isResult && isTaskMasterEx)) {
                    TotalTasks = CompletedTasks = PlayerControl.GameOptions.NumCommonTasks + PlayerControl.GameOptions.NumLongTasks + PlayerControl.GameOptions.NumShortTasks;
                } else {
                    foreach (var playerInfoTask in playerInfo.Tasks.GetFastEnumerator()) {
                        if (playerInfoTask.Complete) CompletedTasks++;
                        TotalTasks++;
                    }
                }
                if (isMadmate)
                    TotalTasks = MadmateTaskHelper.madmateTasks;
            }
            return Tuple.Create(CompletedTasks, TotalTasks);
        }

        [HarmonyPatch(typeof(GameData), nameof(GameData.RecomputeTaskCounts))]
        private static class GameDataRecomputeTaskCountsPatch {
            private static bool Prefix(GameData __instance) {
               

                var totalTasks = 0;
                var completedTasks = 0;
                
                foreach (var playerInfo in GameData.Instance.AllPlayers.GetFastEnumerator())
                {
                    if (playerInfo.Object
                        && playerInfo.Object.hasAliveKillingLover() // Tasks do not count if a Crewmate has an alive killing Lover
                        || playerInfo.PlayerId == Lawyer.lawyer?.PlayerId // Tasks of the Lawyer do not count
                        || (playerInfo.PlayerId == Pursuer.pursuer?.PlayerId && Pursuer.pursuer.Data.IsDead) // Tasks of the Pursuer only count, if he's alive
                        || playerInfo.PlayerId == Thief.thief?.PlayerId // Thief's tasks only count after joining crew team as sheriff (and then the thief is not the thief anymore)
                       )
                        continue;
                    var (playerCompleted, playerTotal) = taskInfo(playerInfo);
                    totalTasks += playerTotal;
                    completedTasks += playerCompleted;
                }
                
                __instance.TotalTasks = totalTasks;
                __instance.CompletedTasks = completedTasks;
                return false;
            }
        }

        [HarmonyPatch(typeof(GameData), nameof(GameData.CompleteTask))]
        private static class GameDataCompleteTaskPatch {
            private static void Postfix(GameData __instance, [HarmonyArgument(0)] PlayerControl pc, [HarmonyArgument(1)] uint taskId) {

                if (TaskRacer.isValid()) {
                    TaskRacer.updateTask(pc);
                }

                if (AmongUsClient.Instance.AmHost && !pc.Data.IsDead && TaskMaster.isTaskMaster(pc.PlayerId)) {
                    byte clearTasks = 0;
                    for (int i = 0; i < pc.Data.Tasks.Count; ++i) {
                        if (pc.Data.Tasks[i].Complete)
                            ++clearTasks;
                    }
                    bool allTasksCompleted = clearTasks == pc.Data.Tasks.Count;
                    Action action = () => {
                        if (TaskMaster.isTaskComplete) {
                            byte clearTasks = 0;
                            for (int i = 0; i < pc.Data.Tasks.Count; ++i) {
                                if (pc.Data.Tasks[i].Complete)
                                    ++clearTasks;
                            }
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.TaskMasterUpdateExTasks, Hazel.SendOption.Reliable, -1);
                            writer.Write(clearTasks);
                            writer.Write((byte)pc.Data.Tasks.Count);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.taskMasterUpdateExTasks(clearTasks, (byte)pc.Data.Tasks.Count);
                        }
                    };

                    if (allTasksCompleted) {
                        if (!TaskMaster.isTaskComplete) {
                            byte[] taskTypeIds = TaskMasterTaskHelper.GetTaskMasterTasks(pc);
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.TaskMasterSetExTasks, Hazel.SendOption.Reliable, -1);
                            writer.Write(pc.PlayerId);
                            writer.Write(byte.MaxValue);
                            writer.Write(taskTypeIds);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.taskMasterSetExTasks(pc.PlayerId, byte.MaxValue, taskTypeIds);
                            action();
                        } else if (!TaskMaster.triggerTaskMasterWin) {
                            action();
                            TaskMaster.triggerTaskMasterWin = true;
                        }
                    } else {
                        action();
                    }
                }
            }
        }
    }
}
