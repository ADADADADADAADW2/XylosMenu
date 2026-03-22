using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace StupidTemplate.Patches
{
    [HarmonyPatch(typeof(GorillaPlayerScoreboardLine), "UpdateLine")]
    public class ScoreboardPatch
    {
        private static void Postfix(GorillaPlayerScoreboardLine __instance)
        {
            if (__instance.linePlayer != null)
            {
                foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
                {
                    if (p.UserId == __instance.linePlayer.UserId)
                    {
                        if (p.CustomProperties.ContainsKey("MenuUser") && p.CustomProperties["MenuUser"].ToString() == ".orgg")
                        {
                            __instance.playerName.color = Color.magenta;
                        }
                        break;
                    }
                }
            }
        }
    }
}
