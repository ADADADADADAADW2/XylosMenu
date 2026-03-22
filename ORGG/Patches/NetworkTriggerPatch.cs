using GorillaNetworking;
using HarmonyLib;

namespace StupidTemplate.Patches.Triggers
{
    [HarmonyPatch(typeof(GorillaNetworkJoinTrigger), "OnBoxTriggered")]
    public class NetworkTriggerPatch
    {
        public static bool enabled;

        public static bool Prefix() => !enabled;
    }
}