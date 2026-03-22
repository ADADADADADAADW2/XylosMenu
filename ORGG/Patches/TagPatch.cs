using GorillaGameModes;
using HarmonyLib;
using Photon.Pun;
using StupidTemplate.Menu;
using System.Collections.Generic;
using UnityEngine;
using GorillaNetworking;

namespace StupidTemplate.Patches
{
    [HarmonyPatch(typeof(GameMode), "ReportTag")]
    public class TagPatch
    {
        public static readonly List<NetPlayer> taggedPlayers = new List<NetPlayer>();

        public static bool enabled;
        public static float tagDelay;
        public static int tagCount;

        private static void PlaySound(string name) { }

        public static void Postfix(NetPlayer player)
        {
            if (enabled && PhotonNetwork.InRoom)
            {
                if (Time.time > tagDelay)
                {
                    taggedPlayers.Clear();
                    tagCount = 0;
                    tagDelay = Time.time + 1f;
                }
                
                if (player != null && !taggedPlayers.Contains(player))
                {
                    taggedPlayers.Add(player);
                    tagCount++;
                    StupidTemplate.Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=green>TAG</color><color=grey>]</color> Tagged: " + (player.NickName ?? "Player"));
                }
            }
        }
    }
}
