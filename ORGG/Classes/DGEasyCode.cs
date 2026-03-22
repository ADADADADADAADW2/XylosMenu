using GorillaGameModes;
using GorillaLocomotion;
using GorillaNetworking;
using GorillaTagScripts;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;

namespace StupidTemplate.Classes
{
    internal class DGEasyCode
    {
        public static void RPCProtection()
        {
            if (!PhotonNetwork.InRoom) return;
            try
            {
                if (MonkeAgent.instance != null)
                {
                    MonkeAgent.instance.rpcErrorMax = int.MaxValue;
                    MonkeAgent.instance.rpcCallLimit = int.MaxValue;
                    MonkeAgent.instance.logErrorMax = int.MaxValue;
                }
                PhotonNetwork.MaxResendsBeforeDisconnect = int.MaxValue;
                PhotonNetwork.QuickResends = int.MaxValue;
            }
            catch { }
        }

        public static void EnableRig(bool Enable)
            => GorillaTagger.Instance.offlineVRRig.enabled = Enable;

        public static void SetRigPosition(Vector3 position)
            => GorillaTagger.Instance.offlineVRRig.transform.position = position;

        public static void TeleportTo(Vector3 position)
            => GorillaLocomotion.GTPlayer.Instance.transform.position = position;

        public static void SetPlayerVelocity(Vector3 velocity)
            => GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = velocity;

        public static void Disconnect()
            => PhotonNetwork.Disconnect();

        public static bool MasterClient
            => PhotonNetwork.IsMasterClient;

        public static bool RightGrab
            => ControllerInputPoller.instance.rightGrab;

        public static bool LeftGrab
            => ControllerInputPoller.instance.leftGrab;

        public static bool RightTrigger
            => ControllerInputPoller.instance.rightControllerIndexFloat > .6;

        public static bool LeftTrigger
            => ControllerInputPoller.instance.leftControllerIndexFloat > .6;

        public static bool LeftX
            => ControllerInputPoller.instance.leftControllerPrimaryButton;

        public static bool LeftY
            => ControllerInputPoller.instance.leftControllerSecondaryButton;

        public static void JumpscareGun()
        {
            VRRig target = GetGunTarget(out bool isShooting);
            if (target != null && isShooting)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                Vector3 headPos = target.headMesh.transform.position;
                Vector3 forward = target.headMesh.transform.forward;
                
                GorillaTagger.Instance.offlineVRRig.transform.position = headPos + forward * 0.2f;
                GorillaTagger.Instance.offlineVRRig.transform.LookAt(headPos);
                
                GorillaTagger.Instance.offlineVRRig.rightHandTransform.position = headPos + UnityEngine.Random.insideUnitSphere * 1.5f;
                GorillaTagger.Instance.offlineVRRig.leftHandTransform.position = headPos + UnityEngine.Random.insideUnitSphere * 1.5f;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = UnityEngine.Random.rotationUniform;
                GorillaTagger.Instance.offlineVRRig.transform.localScale = new Vector3(2f, 2f, 2f);
            }
            else
            {
                if (!GorillaTagger.Instance.offlineVRRig.enabled)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                    GorillaTagger.Instance.offlineVRRig.transform.localScale = Vector3.one;
                }
            }
        }

        public static VRRig GetGunTarget(out bool isShooting)
        {
            isShooting = RightTrigger;
            if (!isShooting) return null;

            RaycastHit hit;
            if (Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.forward, out hit, 100f))
            {
                VRRig hitRig = hit.collider.GetComponentInParent<VRRig>();
                if (hitRig != null && hitRig != GorillaTagger.Instance.offlineVRRig)
                {
                    return hitRig;
                }
            }
            return null;
        }

        public static void TagAura(float radius = 5f)
        {
            if (GorillaGameManager.instance == null || !PhotonNetwork.InRoom) return;
            if (!isTagged(GorillaTagger.Instance.offlineVRRig)) return;

            RPCProtection();
            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                VRRig rig = GorillaGameManager.instance.FindPlayerVRRig(player);
                if (rig != null && Vector3.Distance(GorillaTagger.Instance.offlineVRRig.transform.position, rig.transform.position) < radius)
                {
                    if (!isTagged(rig))
                    {
                        GorillaGameManager.instance.ReportTag(player, NetworkSystem.Instance.LocalPlayer);
                    }
                }
            }
        }

        public static void TagAll()
        {
            if (GorillaGameManager.instance == null || !PhotonNetwork.InRoom) return;
            if (!isTagged(GorillaTagger.Instance.offlineVRRig)) return;

            RPCProtection();
            Vector3 oldPos = GorillaLocomotion.GTPlayer.Instance.transform.position;
            
            // Enable Noclip
            foreach (MeshCollider mc in Resources.FindObjectsOfTypeAll<MeshCollider>()) mc.enabled = false;

            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                VRRig rig = GorillaGameManager.instance.FindPlayerVRRig(player);
                if (rig != null && !isTagged(rig))
                {
                    GorillaLocomotion.GTPlayer.Instance.transform.position = rig.transform.position;
                    GorillaTagger.Instance.offlineVRRig.transform.position = rig.transform.position;
                    // Tag
                    GorillaGameManager.instance.ReportTag(player, NetworkSystem.Instance.LocalPlayer);
                }
            }

            // Teleport Back
            GorillaLocomotion.GTPlayer.Instance.transform.position = oldPos;

            // Disable Noclip (Restore Mesh Colliders)
            foreach (MeshCollider mc in Resources.FindObjectsOfTypeAll<MeshCollider>()) mc.enabled = true;
        }

        public static void SlowAll()
        {
            if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient && GorillaGameManager.instance is GorillaTagManager tagManager)
            {
                foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
                {
                    RPCProtection();
                    tagManager.photonView.RPC("SetFreezeStatus", RpcTarget.All, new object[] { p, true });
                }
            }
        }

        public static void FreezeAll()
        {
            FreezeAll(11);
        }
        private static float freezeDelay = 0f;
        public static void FreezeAll(int eventCount = 11, RaiseEventOptions options = null)
        {
            if (!PhotonNetwork.InRoom) return;

            options ??= new RaiseEventOptions
            {
                TargetActors = new[] { -1 }
            };

            if (Time.time > freezeDelay)
            {
                RPCProtection();
                for (int i = 0; i < eventCount; i++)
                    PhotonNetwork.RaiseEvent(54, new object[] { "" }, options, SendOptions.SendUnreliable);

                freezeDelay = Time.time + 0.2f;
            }
        }

        public static bool isTagged(VRRig vrrig)
        {
            if (vrrig == null) return false;
            
            List<NetPlayer> infectedPlayers = InfectedList();
            if (infectedPlayers != null && infectedPlayers.Contains(vrrig.Creator)) return true;

            if (vrrig.mainSkin != null && vrrig.mainSkin.material != null)
            {
                string matName = vrrig.mainSkin.material.name.ToLower();
                if (matName.Contains("lava") || matName.Contains("rock") || matName.Contains("infected")) return true;
            }

            return false;
        }

        public static List<NetPlayer> InfectedList()
        {
            List<NetPlayer> infected = new List<NetPlayer>();

            if (!PhotonNetwork.InRoom || GorillaGameManager.instance == null)
                return infected;

            if (GorillaGameManager.instance is GorillaTagManager tagManager)
            {
                if (tagManager.isCurrentlyTag)
                {
                    if (tagManager.currentIt != null) infected.Add(tagManager.currentIt);
                }
                else
                {
                    if (tagManager.currentInfected != null) infected.AddRange(tagManager.currentInfected);
                }
            }
            
            return infected;
        }

        public static void LagAll()
        {
            if (!PhotonNetwork.InRoom) return;
            RPCProtection();
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            for (int i = 0; i < 75; i++)
            {
                PhotonNetwork.RaiseEvent(200, null, options, SendOptions.SendUnreliable);
            }
        }

        public static void KickAll()
        {
            if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
            {
                foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerListOthers)
                {
                    PhotonNetwork.CloseConnection(p);
                }
            }
        }
    }
}
