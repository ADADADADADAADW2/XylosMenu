using HarmonyLib;
using Photon.Pun;
using System;
using UnityEngine;

namespace StupidTemplate.Patches
{
    [HarmonyPatch(typeof(MonkeAgent))]
    internal class AntiCheat
    {
        [HarmonyPatch("SendReport")]
        [HarmonyPrefix]
        private static bool NoSendReport(string susReason, string susId, string susNick)
        {
            return false;
        }

        [HarmonyPatch("LogErrorCount")]
        [HarmonyPrefix]
        private static bool NoLogErrorCount(string logString, string stackTrace, LogType type)
        {
            return false;
        }

        [HarmonyPatch("IncrementRPCCallLocal")]
        [HarmonyPrefix]
        private static bool NoIncrementRPCCallLocal(PhotonMessageInfoWrapped infoWrapped, string rpcFunction)
        {
            return false;
        }

        [HarmonyPatch("GetRPCCallTracker")]
        [HarmonyPrefix]
        private static bool NoGetRPCCallTracker()
        {
            return false;
        }

        [HarmonyPatch("IncrementRPCCall", new Type[] { typeof(PhotonMessageInfo), typeof(string) })]
        [HarmonyPrefix]
        private static bool NoIncrementRPCCall(PhotonMessageInfo info, string callingMethod = "")
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(VRRig), "IncrementRPC", new Type[] { typeof(PhotonMessageInfoWrapped), typeof(string) })]
    internal class NoIncrementRPC
    {
        private static bool Prefix(PhotonMessageInfoWrapped info, string sourceCall)
        {
            return false;
        }
    }
}

