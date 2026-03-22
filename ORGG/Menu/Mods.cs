using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;

using Photon.Pun;
using Photon.Realtime;
using GorillaLocomotion;
using GorillaNetworking;
using GorillaGameModes;
using GorillaTagScripts;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

using UnityEngine.InputSystem;

using TMPro;

using orgg.Menu;



namespace StupidTemplate.Menu

{

    class mods

    {
        public static float rotationX = 0f;
        public static float rotationY = 0f;
        public static float cameraDistance = 3.0f;
        public static float cameraHeight = 1.0f;
        public static float antiReportDistance = 0.35f;
        public static string[] Zoner = { "Forest", "Cave", "Canyon", "Mountain", "Sky", "Basement", "Beach", "Clouds" };
        public static bool buttonSoundsEnabled = false;
        public static float pullPower = 0.05f;
        private static readonly Dictionary<bool, bool> previousTouchingGround = new Dictionary<bool, bool>();

        public static bool instantTag = false;

        private static Vector3 RandomVector3() => new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        private static Quaternion RandomQuaternion() => UnityEngine.Random.rotationUniform;



        public static void SetCustomMenuProperties()
        {
            if (PhotonNetwork.InRoom && PhotonNetwork.LocalPlayer != null)
            {
                ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
                if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("MenuUser"))
                {
                    props.Add("MenuUser", ".orgg");
                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);
                }
            }
        }

        public static void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        public static void AntiReport()
        {
                foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                {
                    if (line.linePlayer != null && line.linePlayer.ActorNumber == NetworkSystem.Instance.LocalPlayer.ActorNumber)
                    {
                        Transform reportButton = line.reportButton.gameObject.transform;
                        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerListOthers)
                        {
                            VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(p);
                            if (vrrig != null)
                            {
                                float dist = Vector3.Distance(vrrig.rightHandTransform.position, reportButton.position);
                                float dist2 = Vector3.Distance(vrrig.leftHandTransform.position, reportButton.position);
                                if (dist < antiReportDistance || dist2 < antiReportDistance)
                                {
                                    PhotonNetwork.Disconnect();
                                    StupidTemplate.Notifications.NotifiLib.SendNotification("<color=red>[ANTI-REPORT]</color> Someone attempted to report you, disconnected.");
                                }
                            }
                        }
                    }
                }
        }

        public static void VisualizeAntiReport()
        {
                foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                {
                    if (line.linePlayer != null && line.linePlayer.ActorNumber == NetworkSystem.Instance.LocalPlayer.ActorNumber)
                    {
                        Transform reportButton = line.reportButton.gameObject.transform;
                        if (reportButton != null)
                        {
                            GameObject visualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            GameObject.Destroy(visualizer.GetComponent<Collider>());
                            visualizer.transform.position = reportButton.position;
                            visualizer.transform.localScale = new Vector3(antiReportDistance, antiReportDistance, antiReportDistance);
                            visualizer.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f, 0.3f);
                            visualizer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                            GameObject.Destroy(visualizer, Time.deltaTime);
                        }
                    }
                }
        }

        public static void placeholder()
        {
        }


        public static void Ghostmonke()
        {
            if (ControllerInputPoller.instance.rightControllerSecondaryButton)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void DisableGhostmonke()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void InvisMonke()
        {
            if (ControllerInputPoller.instance.rightControllerSecondaryButton)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(9999f, 9999f, 9999f);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void DisableInvisMonke()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void SolidPlayer()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (var p in PhotonNetwork.PlayerListOthers)
            {
                VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(p);
                if (vrrig != null)
                {
                    foreach (var c in vrrig.GetComponentsInChildren<Collider>())
                    {
                        c.isTrigger = false;
                    }
                }
            }
        }
        public static void DisableSolidPlayer()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (var p in PhotonNetwork.PlayerListOthers)
            {
                VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(p);
                if (vrrig != null)
                {
                    foreach (var c in vrrig.GetComponentsInChildren<Collider>())
                    {
                        c.isTrigger = true;
                    }
                }
            }
        }

        private static GameObject rightHandTrail;
        private static GameObject leftHandTrail;
        public static void HandTrails()
        {
                if (rightHandTrail == null)
                {
                    rightHandTrail = new GameObject("RightTrail");
                    TrailRenderer tr = rightHandTrail.AddComponent<TrailRenderer>();
                    tr.time = 0.5f;
                    tr.startWidth = 0.05f;
                    tr.endWidth = 0.01f;
                    tr.material = new Material(Shader.Find("GUI/Text Shader"));
                    tr.startColor = Color.cyan;
                    tr.endColor = Color.clear;
                }
                rightHandTrail.transform.position = GorillaTagger.Instance.rightHandTransform.position;

                if (leftHandTrail == null)
                {
                    leftHandTrail = new GameObject("LeftTrail");
                    TrailRenderer tr = leftHandTrail.AddComponent<TrailRenderer>();
                    tr.time = 0.5f;
                    tr.startWidth = 0.05f;
                    tr.endWidth = 0.01f;
                    tr.material = new Material(Shader.Find("GUI/Text Shader"));
                    tr.startColor = Color.magenta;
                    tr.endColor = Color.clear;
                }
                leftHandTrail.transform.position = GorillaTagger.Instance.leftHandTransform.position;
        }
        public static void DisableHandTrails()
        {
            if (rightHandTrail != null) { GameObject.Destroy(rightHandTrail); rightHandTrail = null; }
            if (leftHandTrail != null) { GameObject.Destroy(leftHandTrail); leftHandTrail = null; }
        }

        public static void ESP()
        {
                if (!PhotonNetwork.InRoom) return;
                Vector3 headPos = GorillaTagger.Instance.headCollider.transform.position;
                Material mat = new Material(Shader.Find("GUI/Text Shader"));
                foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerListOthers)
                {
                    VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(p);
                    if (vrrig != null)
                    {
                        GameObject line = new GameObject("ESPLine");
                        LineRenderer lr = line.AddComponent<LineRenderer>();
                        lr.startWidth = 0.01f;
                        lr.endWidth = 0.01f;
                        lr.material = mat;
                        lr.startColor = Color.red;
                        lr.endColor = Color.yellow;
                        lr.positionCount = 2;
                        lr.SetPosition(0, headPos);
                        lr.SetPosition(1, vrrig.transform.position);
                        GameObject.Destroy(line, Time.deltaTime);
                    }
                }
        }
        public static void DisableESP() {} // Line ESP handles its own destruction via Time.deltaTime

        private static Dictionary<VRRig, Material[]> originalMaterials = new Dictionary<VRRig, Material[]>();
        public static void Chams()
        {
                if (!PhotonNetwork.InRoom) return;
                Material mat = new Material(Shader.Find("GUI/Text Shader"));
                foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerListOthers)
                {
                    VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(p);
                    if (vrrig != null && vrrig.mainSkin != null)
                    {
                        if (!originalMaterials.ContainsKey(vrrig))
                        {
                            originalMaterials.Add(vrrig, vrrig.mainSkin.materials);
                        }
                        vrrig.mainSkin.material.shader = mat.shader;
                        vrrig.mainSkin.material.color = Color.green;
                    }
                }
        }
        public static void DisableChams()
        {
            foreach (var pair in originalMaterials)
            {
                if (pair.Key != null && pair.Key.mainSkin != null)
                {
                    pair.Key.mainSkin.materials = pair.Value;
                }
            }
            originalMaterials.Clear();
        }

        public static void BoxESP()
        {
                if (!PhotonNetwork.InRoom) return;
                Material mat = new Material(Shader.Find("GUI/Text Shader"));
                foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerListOthers)
                {
                    VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(p);
                    if (vrrig != null)
                    {
                        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        GameObject.Destroy(box.GetComponent<Collider>());
                        box.transform.position = vrrig.transform.position + Vector3.up * 0.5f;
                        box.transform.localScale = new Vector3(0.5f, 0.8f, 0.5f);
                        box.GetComponent<Renderer>().material = mat;
                        box.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f, 0.3f);
                        GameObject.Destroy(box, Time.deltaTime);
                    }
                }
        }
        public static void DisableBoxESP() {}

        public static void Tracers()
        {
                if (!PhotonNetwork.InRoom) return;
                Vector3 handPos = GorillaTagger.Instance.rightHandTransform.position;
                Material mat = new Material(Shader.Find("GUI/Text Shader"));
                foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerListOthers)
                {
                    VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(p);
                    if (vrrig != null)
                    {
                        GameObject line = new GameObject("Tracer");
                        LineRenderer lr = line.AddComponent<LineRenderer>();
                        lr.startWidth = 0.02f;
                        lr.endWidth = 0.02f;
                        lr.material = mat;
                        lr.startColor = Color.cyan;
                        lr.endColor = Color.cyan;
                        lr.positionCount = 2;
                        lr.SetPosition(0, handPos);
                        lr.SetPosition(1, vrrig.transform.position + Vector3.up * 0.5f);
                        GameObject.Destroy(line, Time.deltaTime);
                    }
                }
        }
        public static void DisableTracers() {}

        private static float lastCrumbTime = 0f;
        public static void BreadCrumbs()
        {
                if (Time.time - lastCrumbTime < 0.2f) return;
                lastCrumbTime = Time.time;
                GameObject crumb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                GameObject.Destroy(crumb.GetComponent<Collider>());
                crumb.transform.position = GorillaTagger.Instance.headCollider.transform.position;
                crumb.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                crumb.GetComponent<Renderer>().material = new Material(Shader.Find("GUI/Text Shader"));
                crumb.GetComponent<Renderer>().material.color = Color.yellow;
                GameObject.Destroy(crumb, 10f);
        }

        public static void NameTags()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerListOthers)
            {
                VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(p);
                if (vrrig != null)
                {
                    GameObject tag = new GameObject("NameTag");

                    tag.transform.position = vrrig.transform.position + Vector3.up * 1.5f;
                    
                    int r = Mathf.Clamp(Mathf.RoundToInt(vrrig.playerColor.r * 9f), 0, 9);
                    int g = Mathf.Clamp(Mathf.RoundToInt(vrrig.playerColor.g * 9f), 0, 9);
                    int b = Mathf.Clamp(Mathf.RoundToInt(vrrig.playerColor.b * 9f), 0, 9);
                    string colorStr = $"<color=red>{r}</color> <color=green>{g}</color> <color=blue>{b}</color>";
                    
                    int fps = Mathf.RoundToInt(1.0f / Time.smoothDeltaTime);
                    string fpsStr = fps >= 72 ? $"<color=green>{fps}</color>" : (fps > 45 ? $"<color=yellow>{fps}</color>" : $"<color=red>{fps}</color>");

                    string userId = string.IsNullOrEmpty(p.UserId) ? "UNKNOWN_ID" : p.UserId.ToUpper();
                    string createDate = "UNKNOWN DATE"; 
                    string actorStr = p.ActorNumber.ToString();
                    string masterStr = p.IsMasterClient ? "True" : "False";

                    TextMesh tm = tag.AddComponent<TextMesh>();
                    tm.text = $"USERNAME : {p.NickName.ToUpper()}\nID : {userId}\nPLATFORM : OCULUS\nFPS : {fpsStr}\nCREATION : {createDate}\nCOLOR : {colorStr}\nACTOR : {actorStr}\nMASTER : {masterStr}";

                    tm.fontSize = 64;
                    tm.characterSize = 0.018f;
                    tm.alignment = TextAlignment.Left; 
                    tm.anchor = TextAnchor.MiddleCenter;
                    tm.color = Color.white;
                    tm.richText = true;

                    GameObject shadow = new GameObject("Shadow");
                    shadow.transform.SetParent(tag.transform);
                    shadow.transform.localPosition = new Vector3(0.015f, -0.015f, 0.01f);
                    TextMesh tm2 = shadow.AddComponent<TextMesh>();
                    tm2.text = $"<color=black>USERNAME : {p.NickName.ToUpper()}\nID : {userId}\nPLATFORM : OCULUS\nFPS : {fps}\nCREATION : {createDate}\nCOLOR : {r} {g} {b}\nACTOR : {actorStr}\nMASTER : {masterStr}</color>";
                    tm2.fontSize = 64;
                    tm2.characterSize = 0.018f;
                    tm2.alignment = TextAlignment.Left;
                    tm2.anchor = TextAnchor.MiddleCenter;
                    tm2.richText = true;

                    tag.transform.LookAt(GorillaTagger.Instance.headCollider.transform);
                    tag.transform.Rotate(0, 180f, 0);
                    GameObject.Destroy(tag, Time.deltaTime);
                }
            }
        }

        public static void DumpCosmetics()
        {
            string text = "";
            foreach (var hat in CosmeticsController.instance.allCosmetics)
            {
                text += hat.itemName + " ; " + hat.displayName + " (override " + hat.overrideDisplayName + ") ; " + hat.cost + "SR ; canTryOn = " + hat.canTryOn + "\n";
            }
            System.IO.File.WriteAllText("CosmeticsDump.txt", text);
        }

        private static readonly Dictionary<VRRig, GameObject> castingNameTags = new Dictionary<VRRig, GameObject>();

        public static void CastingTags()
        {
            bool hoc = Buttons.GetIndex("Hidden on Camera").enabled;

            List<KeyValuePair<VRRig, GameObject>> nametagsCopy = castingNameTags.ToList();
            foreach (var nametag in nametagsCopy.Where(nametag => !VRRigCache.ActiveRigs.Contains(nametag.Key)))
            {
                Object.Destroy(nametag.Value);
                castingNameTags.Remove(nametag.Key);
            }

            foreach (VRRig vrrig in VRRigCache.ActiveRigs)
            {
                try
                {
                    if (!vrrig.isLocal || selfNameTag)
                    {
                        if (!castingNameTags.ContainsKey(vrrig))
                        {
                            GameObject go = new GameObject("Seralyth_CastingTag");
                            go.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

                            TextMeshPro text = go.AddComponent<TextMeshPro>();
                            text.fontSize = 4.8f;
                            text.alignment = TextAlignmentOptions.Center;
                            text.richText = true;

                            text.spriteAsset = InfoSprites;

                            castingNameTags.Add(vrrig, go);
                        }

                        GameObject nameTag = castingNameTags[vrrig];
                        TextMeshPro tmp = nameTag.GetOrAddComponent<TextMeshPro>();

                        if (hoc)
                            nameTag.layer = 19;
                        else
                            nameTag.layer = 0;

                        if (NameTagOptimize())
                        {
                            string sprite = $"<size=120%><sprite name=\"{vrrig.GetPlatform()}\"></size>";
                            string playerName = CleanPlayerName(GetPlayerFromVRRig(vrrig).NickName);

                            tmp.SafeSetText($"{sprite}<space=-0.2em>{playerName}");

                            tmp.color = Color.white;

                            tmp.SafeSetFontStyle(activeFontStyle);
                            tmp.SafeSetFont(activeFont);
                        }

                        if (nameTagChams)
                            tmp.Chams();

                        nameTag.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f) * vrrig.scaleFactor;
                        nameTag.transform.position = GetNameTagPosition(vrrig);
                        nameTag.transform.LookAt(Camera.main.transform.position);
                        nameTag.transform.Rotate(0f, 180f, 0f);
                    }
                }
                catch { }
            }
        }

        public static void DisableCastingTags()
        {
            foreach (KeyValuePair<VRRig, GameObject> nametag in castingNameTags)
                Object.Destroy(nametag.Value);

            castingNameTags.Clear();
        }

        public static void SpawnRobux()
        {

            if (Time.frameCount % 20 == 0)
            {
                StupidTemplate.Notifications.NotifiLib.SendNotification("<color=green>+ 10,000 ROBUX</color>");
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(111, false, 1f); 
            }
        }

        public static void DisableSpawnRobux() { }

        public static void RainbowBodySS()
        {
                float r = Mathf.Sin(Time.time * 2f) * 0.5f + 0.5f;
                float g = Mathf.Sin(Time.time * 2f + 2f) * 0.5f + 0.5f;
                float b = Mathf.Sin(Time.time * 2f + 4f) * 0.5f + 0.5f;
                GorillaTagger.Instance.UpdateColor(r, g, b);
        }
        public static void DisableRainbowBodySS()
        {
            GorillaTagger.Instance.UpdateColor(0.5f, 0.5f, 0.5f);
        }

        public static void StrobeBodySS()
        {
            float t = Mathf.Floor(Time.time * 10f) % 2 == 0 ? 1f : 0f;
            GorillaTagger.Instance.UpdateColor(t, t, t);
        }
        public static void DisableStrobeBodySS()
        {
            GorillaTagger.Instance.UpdateColor(0.5f, 0.5f, 0.5f);
        }


        public static void LowGravity()
        {
            Physics.gravity = new Vector3(0, -3f, 0);
        }
        public static void DisableLowGravity()
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }

        public static void ZeroGravity()
        {
                if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f)
                    Physics.gravity = Vector3.zero;
                else
                    Physics.gravity = new Vector3(0, -9.81f, 0);
        }
        public static void DisableZeroGravity()
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }

        public static void SpeedBoost()
        {
            GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed = speedMult;
            GorillaLocomotion.GTPlayer.Instance.jumpMultiplier = 1.1f + (speedMult / 10f); // Adjust jump multiplier based on speed
        }
        public static void DisableSpeedBoost()
        {
            GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed = 6.5f;
            GorillaLocomotion.GTPlayer.Instance.jumpMultiplier = 1.1f;
        }



        public static void HighJump()
        {
            GorillaLocomotion.GTPlayer.Instance.jumpMultiplier = 1.5f;
        }
        public static void DisableHighJump()
        {
            GorillaLocomotion.GTPlayer.Instance.jumpMultiplier = 1.1f;
        }

        public static void SuperJump()
        {
            GorillaLocomotion.GTPlayer.Instance.jumpMultiplier = 2.5f;
        }

        public static void Fly()
        {
                Rigidbody rb = GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody;
                if (ControllerInputPoller.instance.rightControllerGripFloat > 0.5f)
                {
                    rb.useGravity = false;
                    rb.velocity = GorillaTagger.Instance.headCollider.transform.forward * flyMult;
                }
                else
                {
                    rb.useGravity = true;
                }
        }
        public static void DisableFly()
        {
            GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.useGravity = true;
        }

        private static GameObject rightPlat;
        private static GameObject leftPlat;
        public static void Platforms()
        {
            if (ControllerInputPoller.instance.rightControllerGripFloat > 0.5f)
            {
                if (rightPlat == null)
                {
                    rightPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    rightPlat.transform.localScale = new Vector3(0.3f, 0.05f, 0.3f);
                    rightPlat.GetComponent<Renderer>().material = new Material(Shader.Find("Sprites/Default"));
                    rightPlat.GetComponent<Renderer>().material.color = new Color(0f, 0f, 0f, 0.8f);
                }
                rightPlat.transform.position = GorillaTagger.Instance.rightHandTransform.position + new Vector3(0, -0.1f, 0);
                rightPlat.transform.rotation = Quaternion.identity;
            }
            else if (rightPlat != null)
            {
                Rigidbody rb = rightPlat.AddComponent<Rigidbody>();
                rb.velocity = GorillaTagger.Instance.rightHandTransform.GetComponent<Rigidbody>() != null ? GorillaTagger.Instance.rightHandTransform.GetComponent<Rigidbody>().velocity : Vector3.zero;
                GameObject.Destroy(rightPlat, 3f);
                rightPlat = null;
            }

            if (ControllerInputPoller.instance.leftControllerGripFloat > 0.5f)
            {
                if (leftPlat == null)
                {
                    leftPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    leftPlat.transform.localScale = new Vector3(0.3f, 0.05f, 0.3f);
                    leftPlat.GetComponent<Renderer>().material = new Material(Shader.Find("Sprites/Default"));
                    leftPlat.GetComponent<Renderer>().material.color = new Color(0f, 0f, 0f, 0.8f);
                }
                leftPlat.transform.position = GorillaTagger.Instance.leftHandTransform.position + new Vector3(0, -0.1f, 0);
                leftPlat.transform.rotation = Quaternion.identity;
            }
            else if (leftPlat != null)
            {
                Rigidbody rb = leftPlat.AddComponent<Rigidbody>();
                rb.velocity = GorillaTagger.Instance.leftHandTransform.GetComponent<Rigidbody>() != null ? GorillaTagger.Instance.leftHandTransform.GetComponent<Rigidbody>().velocity : Vector3.zero;
                GameObject.Destroy(leftPlat, 3f);
                leftPlat = null;
            }
        }
        
        private static GameObject rightInvisPlat;
        private static GameObject leftInvisPlat;
        public static void InvisPlatforms()
        {
            if (ControllerInputPoller.instance.rightControllerGripFloat > 0.5f)
            {
                if (rightInvisPlat == null)
                {
                    rightInvisPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    rightInvisPlat.transform.localScale = new Vector3(0.3f, 0.05f, 0.3f);
                    UnityEngine.Object.Destroy(rightInvisPlat.GetComponent<Renderer>()); // Invisible
                }
                rightInvisPlat.transform.position = GorillaTagger.Instance.rightHandTransform.position + new Vector3(0, -0.1f, 0);
                rightInvisPlat.transform.rotation = Quaternion.identity;
            }
            else if (rightInvisPlat != null)
            {
                rightInvisPlat.AddComponent<Rigidbody>();
                GameObject.Destroy(rightInvisPlat, 3f);
                rightInvisPlat = null;
            }

            if (ControllerInputPoller.instance.leftControllerGripFloat > 0.5f)
            {
                if (leftInvisPlat == null)
                {
                    leftInvisPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    leftInvisPlat.transform.localScale = new Vector3(0.3f, 0.05f, 0.3f);
                    UnityEngine.Object.Destroy(leftInvisPlat.GetComponent<Renderer>()); // Invisible
                }
                leftInvisPlat.transform.position = GorillaTagger.Instance.leftHandTransform.position + new Vector3(0, -0.1f, 0);
                leftInvisPlat.transform.rotation = Quaternion.identity;
            }
            else if (leftInvisPlat != null)
            {
                leftInvisPlat.AddComponent<Rigidbody>();
                GameObject.Destroy(leftInvisPlat, 3f);
                leftInvisPlat = null;
            }
        }

        public static void DisableInvisPlatforms()
        {
            if (rightInvisPlat != null) { GameObject.Destroy(rightInvisPlat); rightInvisPlat = null; }
            if (leftInvisPlat != null) { GameObject.Destroy(leftInvisPlat); leftInvisPlat = null; }
        }

        public static void DisablePlatforms()
        {
            if (rightPlat != null) { GameObject.Destroy(rightPlat); rightPlat = null; }
            if (leftPlat != null) { GameObject.Destroy(leftPlat); leftPlat = null; }
        }

        public static void IronMonke()
        {
                Rigidbody rb = GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody;
                if (ControllerInputPoller.instance.rightControllerGripFloat > 0.5f)
                    rb.AddForce(-GorillaTagger.Instance.rightHandTransform.up * 15f, ForceMode.Acceleration);
                if (ControllerInputPoller.instance.leftControllerGripFloat > 0.5f)
                    rb.AddForce(-GorillaTagger.Instance.leftHandTransform.up * 15f, ForceMode.Acceleration);
        }

        public static void Freeze()
        {
                Rigidbody rb = GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody;
                if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f)
                {
                    rb.velocity = Vector3.zero;
                    rb.useGravity = false;
                }
                else
                {
                    rb.useGravity = true;
                }
        }
        public static void DisableFreeze()
        {
            GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.useGravity = true;
        }

        private static bool noClipEnabled = false;
        public static void Noclip()
        {
                bool triggerDown = ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f;
                if (triggerDown && !noClipEnabled)
                {
                    foreach (MeshCollider mc in Resources.FindObjectsOfTypeAll<MeshCollider>())
                        mc.enabled = false;
                    noClipEnabled = true;
                }
                else if (!triggerDown && noClipEnabled)
                {
                    foreach (MeshCollider mc in Resources.FindObjectsOfTypeAll<MeshCollider>())
                        mc.enabled = true;
                    noClipEnabled = false;
                }
        }
        public static void DisableNoclip()
        {
                if (noClipEnabled)
                {
                    foreach (MeshCollider mc in Resources.FindObjectsOfTypeAll<MeshCollider>())
                        mc.enabled = true;
                    noClipEnabled = false;
                }
        }


        private static SpringJoint rightWebJoint;
        private static SpringJoint leftWebJoint;
        private static LineRenderer rightWebLine;
        private static LineRenderer leftWebLine;

        public static void Spiderman()
        {

            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f)
            {
                if (rightWebJoint == null)
                {
                    if (Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.forward, out RaycastHit hit, 200f))
                    {
                        rightWebJoint = GorillaLocomotion.GTPlayer.Instance.gameObject.AddComponent<SpringJoint>();
                        rightWebJoint.autoConfigureConnectedAnchor = false;
                        rightWebJoint.connectedAnchor = hit.point;
                        float dist = Vector3.Distance(GorillaTagger.Instance.rightHandTransform.position, hit.point);
                        rightWebJoint.maxDistance = dist * 0.8f;
                        rightWebJoint.minDistance = 0f;
                        rightWebJoint.spring = 80f;
                        rightWebJoint.damper = 7f;
                        rightWebJoint.massScale = 1f;

                        rightWebLine = new GameObject("RightWeb").AddComponent<LineRenderer>();
                        rightWebLine.startWidth = 0.015f;
                        rightWebLine.endWidth = 0.005f;
                        rightWebLine.material = new Material(Shader.Find("Sprites/Default"));
                        rightWebLine.material.color = Color.white;
                        rightWebLine.positionCount = 2;
                    }
                }
                else
                {
                    rightWebLine.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                    rightWebLine.SetPosition(1, rightWebJoint.connectedAnchor);
                }
            }
            else
            {
                if (rightWebJoint != null) { GameObject.Destroy(rightWebJoint); rightWebJoint = null; }
                if (rightWebLine != null) { GameObject.Destroy(rightWebLine.gameObject); rightWebLine = null; }
            }

            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f)
            {
                if (leftWebJoint == null)
                {
                    if (Physics.Raycast(GorillaTagger.Instance.leftHandTransform.position, GorillaTagger.Instance.leftHandTransform.forward, out RaycastHit hit, 200f))
                    {
                        leftWebJoint = GorillaLocomotion.GTPlayer.Instance.gameObject.AddComponent<SpringJoint>();
                        leftWebJoint.autoConfigureConnectedAnchor = false;
                        leftWebJoint.connectedAnchor = hit.point;
                        float dist = Vector3.Distance(GorillaTagger.Instance.leftHandTransform.position, hit.point);
                        leftWebJoint.maxDistance = dist * 0.8f;
                        leftWebJoint.minDistance = 0f;
                        leftWebJoint.spring = 80f;
                        leftWebJoint.damper = 7f;
                        leftWebJoint.massScale = 1f;

                        leftWebLine = new GameObject("LeftWeb").AddComponent<LineRenderer>();
                        leftWebLine.startWidth = 0.015f;
                        leftWebLine.endWidth = 0.005f;
                        leftWebLine.material = new Material(Shader.Find("Sprites/Default"));
                        leftWebLine.material.color = Color.white;
                        leftWebLine.positionCount = 2;
                    }
                }
                else
                {
                    leftWebLine.SetPosition(0, GorillaTagger.Instance.leftHandTransform.position);
                    leftWebLine.SetPosition(1, leftWebJoint.connectedAnchor);
                }
            }
            else
            {
                if (leftWebJoint != null) { GameObject.Destroy(leftWebJoint); leftWebJoint = null; }
                if (leftWebLine != null) { GameObject.Destroy(leftWebLine.gameObject); leftWebLine = null; }
            }
        }

        public static void DisableSpiderman()
        {
            if (rightWebJoint != null) { GameObject.Destroy(rightWebJoint); rightWebJoint = null; }
            if (rightWebLine != null) { GameObject.Destroy(rightWebLine.gameObject); rightWebLine = null; }
            if (leftWebJoint != null) { GameObject.Destroy(leftWebJoint); leftWebJoint = null; }
            if (leftWebLine != null) { GameObject.Destroy(leftWebLine.gameObject); leftWebLine = null; }
        }

        private static float originalScale = -1f;
        public static void GiantMonke()
        {
            if (originalScale < 0f) originalScale = GorillaLocomotion.GTPlayer.Instance.scale;
            if (ControllerInputPoller.instance.rightControllerGripFloat > 0.5f)
                GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(originalScale * 5f, originalScale * 5f, originalScale * 5f);
            else
                GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(originalScale, originalScale, originalScale);
        }
        public static void DisableGiantMonke()
        {
            if (originalScale > 0f) GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(originalScale, originalScale, originalScale);
            originalScale = -1f;
        }

        private static float originalScaleTiny = -1f;
        public static void TinyMonke()
        {
            if (originalScaleTiny < 0f) originalScaleTiny = GorillaLocomotion.GTPlayer.Instance.scale;
            if (ControllerInputPoller.instance.leftControllerGripFloat > 0.5f)
                GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(originalScaleTiny * 0.15f, originalScaleTiny * 0.15f, originalScaleTiny * 0.15f);
            else
                GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(originalScaleTiny, originalScaleTiny, originalScaleTiny);
        }
        public static void DisableTinyMonke()
        {
            if (originalScaleTiny > 0f) GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(originalScaleTiny, originalScaleTiny, originalScaleTiny);
            originalScaleTiny = -1f;
        }

        public static void SpinMonke()
        {
            if (ControllerInputPoller.instance.rightControllerGripFloat > 0.5f)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.Rotate(0f, 600f * Time.deltaTime, 0f);
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        public static void FlickRightHand()
        {
            if (ControllerInputPoller.instance.rightControllerGripFloat > 0.5f)
                GorillaTagger.Instance.offlineVRRig.rightHandTransform.parent.localScale = new Vector3(5f, 5f, 5f);
            else
                GorillaTagger.Instance.offlineVRRig.rightHandTransform.parent.localScale = Vector3.one;
        }
        public static void DisableFlickRightHand()
        {
            GorillaTagger.Instance.offlineVRRig.rightHandTransform.parent.localScale = Vector3.one;
        }

        public static void FlickLeftHand()
        {
            if (ControllerInputPoller.instance.leftControllerGripFloat > 0.5f)
                GorillaTagger.Instance.offlineVRRig.leftHandTransform.parent.localScale = new Vector3(5f, 5f, 5f);
            else
                GorillaTagger.Instance.offlineVRRig.leftHandTransform.parent.localScale = Vector3.one;
        }
        public static void DisableFlickLeftHand()
        {
            GorillaTagger.Instance.offlineVRRig.leftHandTransform.parent.localScale = Vector3.one;
        }

        public static void Spaz()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.offlineVRRig.transform.Rotate(
                Random.Range(-50f, 50f), 
                Random.Range(-50f, 50f), 
                Random.Range(-50f, 50f)
            );
        }
        public static void DisableSpaz()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void CopyMonke()
        {
            if (ControllerInputPoller.instance.rightControllerGripFloat > 0.5f && PhotonNetwork.InRoom)
            {
                Photon.Realtime.Player closest = null;
                float dist = float.MaxValue;
                foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerListOthers)
                {
                    VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(p);
                    if (vrrig != null)
                    {
                        float d = Vector3.Distance(GorillaLocomotion.GTPlayer.Instance.transform.position, vrrig.transform.position);
                        if (d < dist)
                        {
                            dist = d;
                            closest = p;
                        }
                    }
                }
                if (closest != null)
                {
                    VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(closest);
                    if (vrrig != null)
                    {
                        GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        GorillaLocomotion.GTPlayer.Instance.transform.position = vrrig.transform.position;
                    }
                }
            }
        }

        public static void BarrelRoll()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.offlineVRRig.transform.Rotate(0f, 0f, 300f * Time.deltaTime);
        }
        public static void DisableBarrelRoll()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void Helicopter()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.offlineVRRig.transform.Rotate(0f, 800f * Time.deltaTime, 0f);
        }
        public static void DisableHelicopter()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void HearSelf()
        {
            if (Photon.Voice.PUN.PhotonVoiceNetwork.Instance.PrimaryRecorder != null)
            {
                Photon.Voice.PUN.PhotonVoiceNetwork.Instance.PrimaryRecorder.DebugEchoMode = true;
            }
        }
        public static void DisableHearSelf()
        {
            if (Photon.Voice.PUN.PhotonVoiceNetwork.Instance.PrimaryRecorder != null)
            {
                Photon.Voice.PUN.PhotonVoiceNetwork.Instance.PrimaryRecorder.DebugEchoMode = false;
            }
        }

        private static bool tpCooldown = false;
        public static void TeleportToPlayer()
        {
                if (!PhotonNetwork.InRoom) return;
                if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f && !tpCooldown)
                {
                    tpCooldown = true;
                    Photon.Realtime.Player[] others = PhotonNetwork.PlayerListOthers;
                    if (others.Length > 0)
                    {
                        Photon.Realtime.Player target = others[Random.Range(0, others.Length)];
                        VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(target);
                        if (vrrig != null)
                        {
                            GorillaLocomotion.GTPlayer.Instance.transform.position = vrrig.transform.position;
                        }
                    }
                }
                if (ControllerInputPoller.instance.rightControllerIndexFloat < 0.3f)
                    tpCooldown = false;
        }





        public static void MuteSelf()
        {
            if (Photon.Voice.PUN.PhotonVoiceNetwork.Instance.PrimaryRecorder != null)
            {
                Photon.Voice.PUN.PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = false;
            }
        }

        public static void DisableMuteSelf()
        {
            if (Photon.Voice.PUN.PhotonVoiceNetwork.Instance.PrimaryRecorder != null)
            {
                Photon.Voice.PUN.PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = true;
            }
        }

        public static void MuteGun()
        {
            VRRig target = GetGunTarget(out bool isShooting);
            if (target != null && isShooting)
            {
                foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                {
                    if (line.linePlayer != null && GorillaGameManager.instance.FindPlayerVRRig(line.linePlayer) == target)
                    {
                        line.muteButton.SendMessage("OnBoxTriggered");
                        string nick = line.linePlayer?.NickName ?? "Player";
                        StupidTemplate.Notifications.NotifiLib.SendNotification("<color=red>[MUTE]</color> Muted/Unmuted " + nick);
                        break;
                    }
                }
            }
        }

        public static GameObject GunSphere;

        private static LineRenderer lineRenderer;

        private static float timeCounter = 0f;

        private static Vector3[] linePositions;

        private static Vector3 previousControllerPosition;



        public static float num = 10f;
        public static float speedMult = 9.5f;
        public static float flyMult = 10f;

        public static void CycleSpeedMult()
        {
            if (speedMult == 9.5f) speedMult = 12.5f;
            else if (speedMult == 12.5f) speedMult = 15f;
            else speedMult = 9.5f;
            SpeedBoost();
        }


        public static void CycleFlyMult()
        {
            if (flyMult == 10f) flyMult = 15f;
            else if (flyMult == 15f) flyMult = 25f;
            else flyMult = 10f;
        }



        public static void GunSmoothNess()

        {

            if (num == 10f)

                num = 15f;  // Super smooth (slower)

            else if (num == 15f)

                num = 5f;   // Fast (no smoothness)

            else

                num = 10f;  // Normal smoothness

        }


        public static List<(Color color, string name)> colorCycle = new List<(Color, string)>

{

    (new Color(189f / 255f, 251f / 255f, 204f / 255f), "mint"),

    (new Color(255f / 255f, 229f / 255f, 180f / 255f), "peach"),

    (new Color(134f / 255f, 169f / 255f, 188f / 255f), "dustyBlue"),

    (new Color(200f / 255f, 162f / 255f, 200f / 255f), "lilac"),

    (new Color(255f / 255f, 255f / 255f, 204f / 255f), "paleYellow"),

    (new Color(255f / 255f, 182f / 255f, 193f / 255f), "softPink"),

    (new Color(230f / 255f, 230f / 255f, 250f / 255f), "lavender"),

    (new Color(211f / 255f, 211f / 255f, 211f / 255f), "lightGray"),

    (new Color(169f / 255f, 169f / 255f, 169f / 255f), "warmGray"),

    (new Color(255f / 255f, 255f / 255f, 240f / 255f), "ivory"),

    (new Color(245f / 255f, 240f / 255f, 195f / 255f), "beige"),

    (new Color(128f / 255f, 128f / 255f, 0f / 255f), "olive"),

    (new Color(210f / 255f, 180f / 255f, 140f / 255f), "tan"),

    (new Color(133f / 255f, 153f / 255f, 56f / 255f), "mossGreen"),

    (new Color(194f / 255f, 178f / 255f, 128f / 255f), "sand"),

    (new Color(176f / 255f, 153f / 255f, 128f / 255f), "maincolor")

};



        public static (Color color, string name) currentGunColor = colorCycle[0];


        public static void CycleGunColor()

        {

            int currentIndex = colorCycle.IndexOf(currentGunColor);

            currentGunColor = colorCycle[(currentIndex + 1) % colorCycle.Count];  // Move to the next color

        }



        public static bool isSphereEnabled = true;

        private static VRRig lockedRig = null;
        private static GameObject haloObj = null;

        public static VRRig GetGunTarget(out bool isShooting)
        {
            isShooting = false;
            if (ControllerInputPoller.instance.rightControllerGripFloat > 0.1f || Input.GetMouseButton(1))
            {

                Vector3 aimPoint;
                VRRig hoveredRig = null;
                
                if (lockedRig != null)
                {

                    aimPoint = lockedRig.transform.position;
                    hoveredRig = lockedRig;
                }
                else
                {
                    bool hitSomething = false;
                    RaycastHit hitinfo = default;

                    bool isMouseAiming = Input.GetMouseButton(1);
                    if (UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.rightButton.isPressed)
                    {
                        isMouseAiming = true;
                    }

                    if (isMouseAiming) // Mouse Right Click
                    {
                        Camera cam = null;
                        foreach (Camera c in Camera.allCameras)
                        {
                            if (c.enabled && (c.name.Contains("Shoulder") || c.name.Contains("Main") || c.name.Contains("Camera")))
                            {
                                cam = c;
                                if (c.name.Contains("Shoulder")) break;
                            }
                        }
                        if (cam == null) cam = Camera.main;

                        if (cam != null)
                        {
                            Vector3 mousePos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
                            if (UnityEngine.InputSystem.Mouse.current != null)
                            {
                                mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
                            }
                            else if (Input.mousePresent)
                            {
                                mousePos = Input.mousePosition;
                            }
                            
                            Ray ray = cam.ScreenPointToRay(mousePos);
                            hitSomething = Physics.Raycast(ray, out hitinfo, 100);
                        }
                    }
                    
                    if (!hitSomething)
                    {
                        hitSomething = Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.forward, out hitinfo);
                    }

                    if (hitSomething)
                    {
                        hoveredRig = hitinfo.collider.GetComponentInParent<VRRig>();
                        if (hoveredRig != null)
                        {
                            aimPoint = hoveredRig.transform.position; // Gun lock
                        }
                        else
                        {
                            aimPoint = hitinfo.point;
                        }
                    }
                    else
                    {
                        aimPoint = GorillaTagger.Instance.rightHandTransform.position + GorillaTagger.Instance.rightHandTransform.forward * 5f;
                    }
                }
                
                isShooting = (ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f || (UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.isPressed) || Input.GetMouseButton(0));
                if (isShooting && hoveredRig != null)
                {
                    lockedRig = hoveredRig;
                }
                else if (!isShooting)
                {
                    lockedRig = null; 
                    if (haloObj != null)
                    {
                        UnityEngine.Object.Destroy(haloObj);
                        haloObj = null;
                    }
                }

                if (hoveredRig != null && !isShooting)
                {
                    if (haloObj == null)
                    {
                        haloObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                        haloObj.transform.localScale = new Vector3(0.5f, 0.02f, 0.5f);
                        UnityEngine.Object.Destroy(haloObj.GetComponent<Collider>());
                        UnityEngine.Object.Destroy(haloObj.GetComponent<Rigidbody>());
                        haloObj.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                    }
                    haloObj.GetComponent<Renderer>().material.color = currentGunColor.color;
                    haloObj.transform.position = hoveredRig.transform.position + new Vector3(0f, 0.5f, 0f);
                }
                else if (hoveredRig == null && haloObj != null)
                {
                    UnityEngine.Object.Destroy(haloObj);
                    haloObj = null;
                }

                if (GunSphere == null)
                {
                    GunSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    GunSphere.transform.localScale = isSphereEnabled ? new Vector3(0.1f, 0.1f, 0.1f) : new Vector3(0f, 0f, 0f);
                    GunSphere.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                    GunSphere.GetComponent<Renderer>().material.color = currentGunColor.color;  
                    UnityEngine.Object.Destroy(GunSphere.GetComponent<BoxCollider>());
                    UnityEngine.Object.Destroy(GunSphere.GetComponent<Rigidbody>());
                    UnityEngine.Object.Destroy(GunSphere.GetComponent<Collider>());

                    lineRenderer = GunSphere.AddComponent<LineRenderer>();
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.widthCurve = AnimationCurve.Linear(0, 0.01f, 1, 0.01f);
                    lineRenderer.startColor = currentGunColor.color;  
                    lineRenderer.endColor = currentGunColor.color;

                    linePositions = new Vector3[50];
                    for (int i = 0; i < linePositions.Length; i++) linePositions[i] = GorillaTagger.Instance.rightHandTransform.position;
                }

                GunSphere.transform.position = aimPoint;
                GunSphere.transform.localScale = isSphereEnabled ? new Vector3(0.1f, 0.1f, 0.1f) : new Vector3(0f, 0f, 0f);

                timeCounter += Time.deltaTime;
                Vector3 pos1 = GorillaTagger.Instance.rightHandTransform.position;
                Vector3 controller = pos1 - previousControllerPosition;
                previousControllerPosition = pos1;

                for (int i = 0; i < linePositions.Length; i++)
                {
                    float t = i / (float)(linePositions.Length - 1);
                    Vector3 linePos = Vector3.Lerp(pos1, aimPoint, t);
                    linePositions[i] += controller * 0.5f;
                    linePositions[i] += UnityEngine.Random.insideUnitSphere * 0.01f;
                    linePositions[i] = Vector3.Lerp(linePositions[i], linePos, Time.deltaTime * num);
                }

                lineRenderer.positionCount = linePositions.Length;
                lineRenderer.SetPositions(linePositions);
                GunSphere.GetComponent<Renderer>().material.color = currentGunColor.color;
                lineRenderer.startColor = currentGunColor.color;
                lineRenderer.endColor = currentGunColor.color;

                return hoveredRig;
            }
            else
            {
                lockedRig = null;
                if (GunSphere != null)
                {
                    UnityEngine.Object.Destroy(GunSphere);
                    UnityEngine.Object.Destroy(lineRenderer);
                    timeCounter = 0f;
                    linePositions = null;
                }
                if (haloObj != null)
                {
                    UnityEngine.Object.Destroy(haloObj);
                    haloObj = null;
                }
                return null;
            }
        }

        public static void GunTemplate()
        {
            GetGunTarget(out bool isShooting);
        }


        public static void LongArms()
        {
            GorillaTagger.Instance.offlineVRRig.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        public static void DisableLongArms()
        {
            GorillaTagger.Instance.offlineVRRig.transform.localScale = Vector3.one;
        }

        public static void AntiTag()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerListOthers)
            {
                VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(p);
                if (vrrig != null)
                {
                    if (Classes.DGEasyCode.isTagged(vrrig) && !Classes.DGEasyCode.isTagged(GorillaTagger.Instance.offlineVRRig))
                    {
                        float distance = Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, vrrig.transform.position);
                        if (distance < 4.0f) 
                        {
                            GorillaLocomotion.GTPlayer.Instance.transform.position += Vector3.up * 8f;
                            break;
                        }
                    }
                }
            }
        }

        public static void TagAura()
        {
            Classes.DGEasyCode.TagAura(5f);
        }

        public static void DisableTagAura() { }
        
        public static void UnTagAll()
        {
            if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
            {
                foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
                {
                    GorillaGameManager.instance.photonView.RPC("ReportTagRPC", RpcTarget.All, new object[] { p });
                }
                StupidTemplate.Notifications.NotifiLib.SendNotification("<color=green>[SUCCESS]</color> Untagged everyone!");
            }
        }
        public static void DisableUnTagAll() { }
        
        public static void UnTagGun() 
        { 
            VRRig target = GetGunTarget(out bool isShooting);
            if (target != null && isShooting && PhotonNetwork.IsMasterClient)
            {
                Photon.Realtime.Player p = null;
                foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                {
                    if (GorillaGameManager.instance.FindPlayerVRRig(player) == target)
                    {
                        p = player;
                        break;
                    }
                }
                if (p != null)
                {
                    GorillaGameManager.instance.photonView.RPC("ReportTagRPC", RpcTarget.All, new object[] { p });
                    StupidTemplate.Notifications.NotifiLib.SendNotification("<color=green>[SUCCESS]</color> Untagged " + p.NickName);
                }
            }
        }
        
        public static void DisableUnTagGun() { if (GunSphere != null) UnityEngine.Object.Destroy(GunSphere); }

        public static void TagGun()
        {
            VRRig target = GetGunTarget(out bool isShooting);
            if (target != null && isShooting)
            {
                if (GorillaGameManager.instance == null || !PhotonNetwork.InRoom) return;

                if (!Classes.DGEasyCode.isTagged(GorillaTagger.Instance.offlineVRRig) && !PhotonNetwork.IsMasterClient)
                {
                    StupidTemplate.Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You must be tagged.");
                    return;
                }

                if (Classes.DGEasyCode.isTagged(target)) return;

                Classes.DGEasyCode.RPCProtection();

                Vector3 originalPos = GorillaLocomotion.GTPlayer.Instance.transform.position;
                GorillaLocomotion.GTPlayer.Instance.transform.position = target.transform.position;
                GorillaTagger.Instance.offlineVRRig.transform.position = target.transform.position;

                GorillaGameManager.instance.ReportTag(target.Creator, PhotonNetwork.LocalPlayer);

                GorillaLocomotion.GTPlayer.Instance.transform.position = originalPos;
            }
        }

        public static void DisableTagGun() { if (GunSphere != null) UnityEngine.Object.Destroy(GunSphere); }

        public static void TpGun()
        {
            VRRig target = GetGunTarget(out bool isShooting);
            if (isShooting)
            {
                if (target != null)
                {
                    GorillaLocomotion.GTPlayer.Instance.transform.position = target.transform.position + new Vector3(0, 0.5f, 0);
                }
                else if (GunSphere != null)
                {
                    GorillaLocomotion.GTPlayer.Instance.transform.position = GunSphere.transform.position;
                }
            }
        }
        public static void DisableTpGun() { if (GunSphere != null) UnityEngine.Object.Destroy(GunSphere); }

        public static void TagAll()
        {
            if (GorillaGameManager.instance == null || !PhotonNetwork.InRoom) return;
            if (!Classes.DGEasyCode.isTagged(GorillaTagger.Instance.offlineVRRig) && !PhotonNetwork.IsMasterClient) return;

            Classes.DGEasyCode.RPCProtection();
            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                VRRig rig = GorillaGameManager.instance.FindPlayerVRRig(player);
                if (rig != null && !Classes.DGEasyCode.isTagged(rig))
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = rig.transform.position;
                    GorillaGameManager.instance.ReportTag(player, PhotonNetwork.LocalPlayer);
                }
            }
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void ActivateGrayAll()
        {
            if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
            {
                GreyZoneManager.Instance.ActivateGreyZoneAuthority();
            }
        }

        public static void DeactivateGrayAll()
        {
            if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
            {
                GreyZoneManager.Instance.DeactivateGreyZoneAuthority();
            }
        }

        public static void SlowAll()
        {
            if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient && GorillaGameManager.instance is GorillaTagManager tagManager)
            {
                foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
                {
                    tagManager.photonView.RPC("SetFreezeStatus", RpcTarget.All, new object[] { p, true });
                }
            }
        }

        public static void FreezeAll()
        {
            Classes.DGEasyCode.FreezeAll();
        }
        
        private static bool isSpoofingPos;

        public static void JumpscareGun()
        {
            Classes.DGEasyCode.JumpscareGun();
        }

        public static void DisableJumpscareGun() { 
            if (GunSphere != null) UnityEngine.Object.Destroy(GunSphere); 
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void TeleportSafe(Vector3 targetPosition)
        {
            if (isSpoofingPos) return;
            GorillaTagger.Instance.StartCoroutine(SerializeDelay(targetPosition, 0.1f));
        }

        public static System.Collections.IEnumerator SerializeDelay(Vector3 targetPosition, float delay)
        {
            isSpoofingPos = true;
            Vector3 startPos = GorillaTagger.Instance.offlineVRRig.transform.position;

            GorillaTagger.Instance.offlineVRRig.transform.position = targetPosition;

            yield return new WaitForSecondsRealtime(delay);

            GorillaTagger.Instance.offlineVRRig.transform.position = startPos;
            GorillaTagger.Instance.offlineVRRig.enabled = true;
            isSpoofingPos = false;
        }

        public static void AntiCrash()
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerListOthers)
                {
                    VRRig rig = GorillaGameManager.instance?.FindPlayerVRRig(p);
                    if (rig != null && (rig.transform.position.y > 2000f || rig.transform.position.y < -2000f))
                    {
                        rig.gameObject.SetActive(false);
                    }
                }
            }
        }

        public static void WaterBalloonAura()
        {
                if (!PhotonNetwork.InRoom) return;
                
                if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f) 
                {
                    Transform rightHand = GorillaTagger.Instance.rightHandTransform;
                    foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerListOthers)
                    {
                        VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(p);
                        if (vrrig != null)
                        {
                            Vector3 dir = (vrrig.transform.position - rightHand.position).normalized;


                        }
                    }
                }
        }

        public static void SnowballAura()
        {
                if (!PhotonNetwork.InRoom) return;

                if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f) 
                {
                    Transform rightHand = GorillaTagger.Instance.rightHandTransform;
                    foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerListOthers)
                    {
                        VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(p);
                        if (vrrig != null)
                        {
                            Vector3 dir = (vrrig.transform.position - rightHand.position).normalized;


                        }
                    }
                }
        }

        public static PhotonView barrelView;

        public static void SendBarrelProjectile(Vector3 position, Vector3 velocity, Quaternion rotation, RaiseEventOptions options)
        {
            if (barrelView == null)
            {
                foreach (MonoBehaviour mb in UnityEngine.Object.FindObjectsOfType<MonoBehaviour>())
                {
                    if (mb.GetType().Name.Contains("ScienceExperimentManager"))
                    {
                        barrelView = mb.GetComponent<PhotonView>();
                        break;
                    }
                }
            }
            
            if (barrelView != null)
            {
                Classes.DGEasyCode.RPCProtection();
                if (options.TargetActors != null && options.TargetActors.Length > 0)
                {
                    foreach (int actorId in options.TargetActors)
                    {
                        Photon.Realtime.Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorId);
                        if (p != null)
                        {
                            barrelView.RPC("FireBarrelCannonRPC", p, new object[] { position, velocity });
                        }
                    }
                }
                else
                {
                    barrelView.RPC("FireBarrelCannonRPC", RpcTarget.All, new object[] { position, velocity });
                }
            }
        }

        public static void BarrelFlingGun()
        {
            VRRig target = GetGunTarget(out bool isShooting);
            if (target != null && isShooting)
            {
                if (Time.time > rpcSpamDelay)
                {
                    rpcSpamDelay = Time.time + 0.1f;
                    var player = Classes.RigManager.GetPlayerFromVRRig(target);
                    if (player != null)
                    {
                        SendBarrelProjectile(target.transform.position, new Vector3(0f, 50f, 0f), Quaternion.identity, new RaiseEventOptions { TargetActors = new[] { player.ActorNumber } });
                    }
                }
            }
        }
        public static void DisableBarrelFlingGun() { if (GunSphere != null) UnityEngine.Object.Destroy(GunSphere); }

        public static void BarrelFlingAura()
        {
            if (!PhotonNetwork.InRoom) return;
            
            if (Time.time > rpcSpamDelay)
            {
                rpcSpamDelay = Time.time + 0.1f;
                
                foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerListOthers)
                {
                    VRRig vrrig = GorillaGameManager.instance?.FindPlayerVRRig(p);
                    if (vrrig != null)
                    {
                        float dist = Vector3.Distance(vrrig.transform.position, GorillaTagger.Instance.bodyCollider.transform.position);
                        if (dist < 8f)
                        {
                            SendBarrelProjectile(vrrig.transform.position, new Vector3(0f, 50f, 0f), Quaternion.identity, new RaiseEventOptions { TargetActors = new[] { p.ActorNumber } });
                        }
                    }
                }
            }
        }

        public static void FreezeGun()
        {
            VRRig target = GetGunTarget(out bool isShooting);
            if (target != null && isShooting)
            {
                if (Time.time > rpcSpamDelay)
                {
                    rpcSpamDelay = Time.time + 0.2f;
                    var player = Classes.RigManager.GetPlayerFromVRRig(target);
                    if (player != null)
                    {
                        Classes.DGEasyCode.FreezeAll(1, new RaiseEventOptions { TargetActors = new[] { player.ActorNumber } });
                    }
                }
            }
        }
        public static void DisableFreezeGun() { if (GunSphere != null) UnityEngine.Object.Destroy(GunSphere); }

        private static float ghostReactorDelay = 0f;
       
        public static void CreateItems(object target, int[] hashes, Vector3[] positions, Quaternion[] rotations, long[] sendData = null, GameEntityManager manager = null)
        {
           
            GameEntityManager gameEntityManager = manager ?? ManagerRegistry.GhostReactor.GameEntityManager;
            if (NetworkSystem.Instance.IsMasterClient)
            {
                if (Time.time < ghostReactorDelay)
                    return;

                ghostReactorDelay = Time.time + 0.1f; // Simplified delay if callLimiters is missing or incorrect

                if (target is NetPlayer netPlayer)
                    target = NetPlayerToPlayer(netPlayer);

                sendData ??= Enumerable.Repeat(0L, hashes.Length).ToArray();

                byte[] data = new byte[15360];
                MemoryStream memoryStream = new MemoryStream(data);
                BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
                binaryWriter.Write(hashes.Length);

                for (int i = 0; i < hashes.Length; i++)
                {
                    binaryWriter.Write(manager.CreateTypeNetId(hashes[i]));
                    binaryWriter.Write(hashes[i]);
                    binaryWriter.Write(BitPackUtils.PackWorldPosForNetwork(positions[i]));
                    binaryWriter.Write(BitPackUtils.PackQuaternionForNetwork(rotations[i]));
                    binaryWriter.Write(sendData[i]);
                    binaryWriter.Write(gameEntityManager.GetInvalidNetId());
                }

                byte[] array = GZipHelper.CompressBuffer(data);

                object[] createData = {
                    (int)manager.zone,
                    array
                };

                switch (target)
                {
                    case RpcTarget rpcTarget:
                        gameEntityManager.photonView.RPC("CreateItemsRPC", rpcTarget, createData);
                        break;
                    case Player player:
                        gameEntityManager.photonView.RPC("CreateItemsRPC", player, createData);
                        break;
                }

                Classes.DGEasyCode.RPCProtection();
            }
            else
            {
                // Fallback for non-master
                // CreateItem(target, hashes[0], positions[0], rotations[0], Vector3.zero, Vector3.zero, sendData.Length > 0 ? sendData[1] : 0L, manager);
            }
        }

        public static void SIDrawGun()
        {
            VRRig target = GetGunTarget(out bool isShooting);
            if (isShooting)
            {
                Vector3 pointerPos = GunSphere != null ? GunSphere.transform.position : GorillaTagger.Instance.rightHandTransform.position;
                int hash = objectIds[UnityEngine.Random.Range(0, objectIds.Length)];
                CreateItems(RpcTarget.All, new int[] { hash }, new Vector3[] { pointerPos }, new Quaternion[] { Quaternion.identity }, null, ManagerRegistry.SuperInfection.GameEntityManager);
                var GunData = RenderGun();
            }
        }

        public static void CoreSpamMaster()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (Time.time > rpcSpamDelay)
            {
                rpcSpamDelay = Time.time + 0.1f;
                Classes.DGEasyCode.RPCProtection();
                Vector3 pos = GorillaTagger.Instance.rightHandTransform.position;
                int hash = UnityEngine.Animator.StringToHash("GhostReactorCollectibleCore");
                CreateItems(RpcTarget.Others, new int[] { hash }, new Vector3[] { pos }, new Quaternion[] { UnityEngine.Random.rotation }, null, ManagerRegistry.GhostReactor.GameEntityManager);
            }
        }


            
        public static int[] objectIds = new int[] { 0 };
        public static object RenderGun() => null;

        

        
        public static float ghostReactorDelayValue = 0f;

        
        


       
        
        


    
        private static Player NetPlayerToPlayer(NetPlayer player) { return PhotonNetwork.CurrentRoom.GetPlayer(player.ActorNumber); }
        


        



        public static Material BoardMat;
        public static System.DateTime menuLoadTime = System.DateTime.Now;



        public static void ChangeMapInfoText()
        {
            var mapInfo = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/MapInfo_TMP")?.GetComponent<TMPro.TextMeshPro>();
            if (mapInfo != null)
            {
                mapInfo.text = ".orgg ON TOP";
                mapInfo.color = Color.white;
                mapInfo.fontSize = 42;
                mapInfo.alignment = TMPro.TextAlignmentOptions.Center;
            }
        }

        public static void Boards()
        {
            var tmp = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText")?.GetComponent<TMPro.TextMeshPro>(); 
            if (tmp != null)
            {
                System.TimeSpan uptime = System.DateTime.Now - menuLoadTime; 
                string uptimeStr = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)uptime.TotalHours, uptime.Minutes, uptime.Seconds); 
                string playerName = PhotonNetwork.LocalPlayer != null ? PhotonNetwork.LocalPlayer.NickName : "Not Connected"; 
                string room = PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.Name : "Not In Room"; 
                int players = PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.PlayerCount : 0; 
                tmp.text = ".orgg\n------------------------------\n\nPlayer: " + playerName + "\nRoom: " + room + "\nPlayers: " + players + "\nUptime: " + uptimeStr + "\n\nStatus: Undetected\n\nPress Q to open menu on PC\n------------------------------"; 
            }
            
            string[] currentSpin = { "-", "/", "|", "\\" }; 
            int spinnerSpeed = Mathf.FloorToInt(Time.time * 3f) % currentSpin.Length; 
            string spinner = currentSpin[spinnerSpeed]; 
            
            var motdHeading = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdHeadingText")?.GetComponent<TMPro.TextMeshPro>();
            if (motdHeading != null) motdHeading.text = $"[{spinner}] .orgg INFO BOARD [{spinner}]"; 
            
            var cocHeading = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConductHeadingText")?.GetComponent<TMPro.TextMeshPro>();
            if (cocHeading != null) cocHeading.text = $"[{spinner}] .orgg [{spinner}]"; 
            
            var mapInfoTmp = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/MapInfo_TMP")?.GetComponent<TMPro.TextMeshPro>();
            if (mapInfoTmp != null) mapInfoTmp.text = $"[{spinner}] .orgg ON TOP [{spinner}]"; 
            
            var cocBody = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/COCBodyText_TitleData")?.GetComponent<TMPro.TextMeshPro>();
            if (cocBody != null)
            {
                cocBody.text = ".orgg Mod Menu\n------------------------------\n\nFree & Open Source\n\nReport bugs or suggestions on Discord!\nIf a mod gets you banned, report it!\n\nHave fun with .orgg\n------------------------------"; 
            }
            
            if (BoardMat == null)
            {
                BoardMat = new Material(Shader.Find("Unlit/Color"));
                BoardMat.color = new Color(0.15f, 0.05f, 0.25f, 1f); // Visible dark purple
            }
            string[] boardPaths = new string[]
            {
                "Environment Objects/LocalObjects_Prefab/TreeRoom/GorillaComputerObject/ComputerUI/monitor/monitorScreen",
                "Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomBoundaryStones/BoundaryStoneSet_Forest/wallmonitorforestbg",
                "Environment Objects/LocalObjects_Prefab/TreeRoom/motdBackground",
                "Environment Objects/LocalObjects_Prefab/TreeRoom/COCBackground",
                "Environment Objects/LocalObjects_Prefab/TreeRoom/MapInfo_Background",
                "Environment Objects/LocalObjects_Prefab/TreeRoom/motdBoardBackground",
                "Environment Objects/LocalObjects_Prefab/TreeRoom/COCBoardBackground",
            };
            for (int i = 0; i < boardPaths.Length; i++)
            {
                GameObject obj = GameObject.Find(boardPaths[i]);
                if (obj != null)
                {
                    Renderer ren = obj.GetComponent<Renderer>();
                    if (ren != null) ren.material = BoardMat;
                }
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        private const byte VK_MEDIA_PLAY_PAUSE = 0xB3;
        private const byte VK_MEDIA_NEXT_TRACK = 0xB0;
        private const byte VK_MEDIA_PREV_TRACK = 0xB1;
        private const byte VK_VOLUME_UP = 0xAF;
        private const byte VK_VOLUME_DOWN = 0xAE;
        private const uint KEYEVENTF_KEYDOWN = 0x0000;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        private static void PressMediaKey(byte key)
        {
            keybd_event(key, 0, KEYEVENTF_KEYDOWN, 0);
            keybd_event(key, 0, KEYEVENTF_KEYUP, 0);
        }

        public static void SpotifyPlayPause()
        {
            PressMediaKey(VK_MEDIA_PLAY_PAUSE);
            StupidTemplate.Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=green>SPOTIFY</color><color=grey>]</color> Play/Pause");
        }

        public static void SpotifyNext()
        {
            PressMediaKey(VK_MEDIA_NEXT_TRACK);
            StupidTemplate.Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=green>SPOTIFY</color><color=grey>]</color> Next Track");
        }

        public static void SpotifyPrevious()
        {
            PressMediaKey(VK_MEDIA_PREV_TRACK);
            StupidTemplate.Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=green>SPOTIFY</color><color=grey>]</color> Previous Track");
        }

        public static void SpotifyVolumeUp()
        {
            for (int i = 0; i < 5; i++) PressMediaKey(VK_VOLUME_UP);
            StupidTemplate.Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=green>SPOTIFY</color><color=grey>]</color> Volume Up");
        }

        public static void SpotifyVolumeDown()
        {
            for (int i = 0; i < 5; i++) PressMediaKey(VK_VOLUME_DOWN);
            StupidTemplate.Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=green>SPOTIFY</color><color=grey>]</color> Volume Down");
        }

        public static void CycleAntiReportDistance()
        {
            if (antiReportDistance == 0.35f) antiReportDistance = 0.5f;
            else if (antiReportDistance == 0.5f) antiReportDistance = 1.0f;
            else antiReportDistance = 0.35f;
        }

        public static int gunStyleIndex = 0;
        public static string[] gunStyleNames = new string[] { "Normal", "Wavy", "Zigzag" };

        public static void CycleGunStyle()
        {
            gunStyleIndex = (gunStyleIndex + 1) % gunStyleNames.Length;
        }

        public static void WavyGun()
        {
            VRRig target = GetGunTarget(out bool isShooting);
            if (lineRenderer != null && linePositions != null)
            {
                Vector3 start = GorillaTagger.Instance.rightHandTransform.position;
                Vector3 end = GunSphere != null ? GunSphere.transform.position : start;

                for (int i = 0; i < linePositions.Length; i++)
                {
                    float t = i / (float)(linePositions.Length - 1);
                    Vector3 basePos = Vector3.Lerp(start, end, t);

                    if (gunStyleIndex == 1)
                    {
                        float wave = Mathf.Sin(t * 12f + Time.time * 8f) * 0.08f;
                        Vector3 right = Vector3.Cross((end - start).normalized, Vector3.up).normalized;
                        basePos += right * wave;
                    }
                    else if (gunStyleIndex == 2)
                    {
                        float zig = (i % 2 == 0 ? 1f : -1f) * 0.05f;
                        Vector3 right = Vector3.Cross((end - start).normalized, Vector3.up).normalized;
                        basePos += right * zig;
                    }

                    linePositions[i] = basePos;
                }

                lineRenderer.positionCount = linePositions.Length;
                lineRenderer.SetPositions(linePositions);
            }
        }

        private static bool noGravMenuActive = false;
        public static void NoGravMenu()
        {
            noGravMenuActive = true;
            Physics.gravity = Vector3.zero;
        }
        public static void PullMod()
        {
            ProcessPullHand(false);
            ProcessPullHand(true);
        }

        public static void ProcessPullHand(bool left)
        {
            if ((left ? !Classes.DGEasyCode.LeftGrab : !Classes.DGEasyCode.RightGrab))
                return;

            bool touchingGround = GTPlayer.Instance.IsHandTouching(left);
            previousTouchingGround.TryGetValue(left, out bool wasTouchingGround);

            if (!touchingGround && wasTouchingGround)
            {
                
                Vector3 handPos = left ? GorillaTagger.Instance.offlineVRRig.leftHandTransform.position : GorillaTagger.Instance.offlineVRRig.rightHandTransform.position;
                RaycastHit hitInfo;
                if (Physics.SphereCast(handPos, 0.25f, -Vector3.up, out hitInfo, 1f))
                {
                    Vector3 normal = hitInfo.normal;
                    Vector3 velocity = GorillaTagger.Instance.headCollider.attachedRigidbody.linearVelocity;
                    Vector3 xzVelocity = new Vector3(velocity.x, 0, velocity.z);
                    
                    GTPlayer.Instance.transform.position += (xzVelocity - normal * Vector3.Dot(xzVelocity, normal)).normalized 
                        * (xzVelocity.magnitude / GTPlayer.Instance.maxJumpSpeed * (pullPower * 5f));
                }
            }

            previousTouchingGround[left] = touchingGround;
        }

        public static void CyclePullPower()
        {
            if (pullPower < 0.2f)
                pullPower += 0.05f;
            else
                pullPower = 0.05f;
            StupidTemplate.Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=green>PULL</color><color=grey>]</color> Power: " + pullPower.ToString("F2"));
        }

        public static void DisableNoGravMenu()
        {
            noGravMenuActive = false;
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
        public static void ThirdPersonVisual()
        {
            if (!GorillaTagger.Instance.offlineVRRig.enabled)
                GorillaTagger.Instance.offlineVRRig.enabled = true;

            if (Mouse.current != null && Mouse.current.rightButton.isPressed)
            {
                rotationX += Mouse.current.delta.x.ReadValue() * 0.2f;
                rotationY -= Mouse.current.delta.y.ReadValue() * 0.2f;
            }

            Transform head = GorillaTagger.Instance.headCollider.transform;
            Quaternion cameraRotation = Quaternion.Euler(rotationY, rotationX, 0f);
            Vector3 targetPosition = head.position + (cameraRotation * new Vector3(0f, 0f, -cameraDistance)) + (Vector3.up * cameraHeight);
            
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, Time.deltaTime * 10f);
            Camera.main.transform.LookAt(head.position + (Vector3.up * 0.25f));
        }

        public static void DisableThirdPersonVisual()
        {
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;
        }

        public static void FirstPersonVisual()
        {
            Camera.main.transform.position = GorillaTagger.Instance.headCollider.transform.position;
            Camera.main.transform.rotation = GorillaTagger.Instance.headCollider.transform.rotation;
        }

        public static void DisableFirstPersonVisual()
        {
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;
        }

        private static GameObject waterBlock;
        public static void AirSwim()
        {
            if (waterBlock == null)
            {
                waterBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
                UnityEngine.Object.Destroy(waterBlock.GetComponent<Renderer>());
                waterBlock.GetComponent<BoxCollider>().isTrigger = true;
                // Using reflection/string based if the type is elusive, but we'll try to find it
                if (waterBlock.GetComponent("GorillaWaterTrigger") == null)
                    waterBlock.AddComponent(System.Type.GetType("GorillaLocomotion.Swimming.GorillaWaterTrigger, Assembly-CSharp") ?? System.Type.GetType("GorillaWaterTrigger, Assembly-CSharp"));
                waterBlock.layer = 28;
                waterBlock.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            waterBlock.transform.position = GorillaTagger.Instance.bodyCollider.transform.position;
        }

        public static void DisableAirSwim()
        {
            if (waterBlock != null)
            {
                UnityEngine.Object.Destroy(waterBlock);
                waterBlock = null;
            }
        }

        public static void SpoofFPS()
        {
            StupidTemplate.Patches.FPSPatch.enabled = true;
            StupidTemplate.Patches.FPSPatch.spoofFPSValue = UnityEngine.Random.Range(0, 255);
        }

        public static void DisableSpoofFPS()
        {
            StupidTemplate.Patches.FPSPatch.enabled = false;
        }

        public static void SwimFast()
        {
            // Trying common property names for swimming speed
            var player = GorillaLocomotion.GTPlayer.Instance;
            try { player.GetType().GetProperty("waterSpeed")?.SetValue(player, 70f); } catch {}
            try { player.GetType().GetField("waterSpeed")?.SetValue(player, 70f); } catch {}
        }

        public static void DisableSwimFast()
        {
            var player = GorillaLocomotion.GTPlayer.Instance;
            try { player.GetType().GetProperty("waterSpeed")?.SetValue(player, 35f); } catch {}
            try { player.GetType().GetField("waterSpeed")?.SetValue(player, 35f); } catch {}
        }

        public static void JoinRandom()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.Disconnect();
                return;
            }
            GameObject val = GameObject.Find(Zoner[Random.Range(0, Zoner.Length)]);
            if (val != null)
            {
                Component trigger = val.GetComponent("GorillaNetworkJoinTrigger") ?? val.GetComponent("GorillaLevelJoinTrigger");
                if (trigger != null)
                {
                    trigger.SendMessage("OnBoxTriggered");
                }
            }
        }

        public static void JoystickDisconnect()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton || ControllerInputPoller.instance.leftControllerPrimaryButton)
            {
                PhotonNetwork.Disconnect();
            }
        }

        public static void UnmuteSelf()
        {
            if (Photon.Voice.PUN.PhotonVoiceNetwork.Instance.PrimaryRecorder != null)
                Photon.Voice.PUN.PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = true;
        }

        public static void UnmuteAll()
        {
            foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
            {
                if (line.linePlayer != null)
                {
                    line.muteButton.SendMessage("OnBoxTriggered");
                }
            }
        }

        public static void UnmuteGun()
        {
            VRRig target = GetGunTarget(out bool isShooting);
            if (target != null && isShooting)
            {
                foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                {
                    if (line.linePlayer != null && GorillaGameManager.instance.FindPlayerVRRig(line.linePlayer) == target)
                    {
                        line.muteButton.SendMessage("OnBoxTriggered");
                        break;
                    }
                }
            }
        }

        public static void ReportGun()
        {
            VRRig target = GetGunTarget(out bool isShooting);
            if (target != null && isShooting)
            {
                foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                {
                    if (line.linePlayer != null && GorillaGameManager.instance.FindPlayerVRRig(line.linePlayer) == target)
                    {
                        line.reportButton.SendMessage("OnBoxTriggered");
                        string nick = line.linePlayer?.NickName ?? "Player";
                        StupidTemplate.Notifications.NotifiLib.SendNotification("<color=red>[REPORT]</color> Reported " + nick);
                        break;
                    }
                }
            }
        }

        public static void NoFinger()
        {
    
             GorillaTagger.Instance.offlineVRRig.leftHandTransform.localScale = Vector3.one; 
        }

        public static void DisableNoFinger() { }

        public static void LoudMicrophone()
        {
            StupidTemplate.Patches.Stuff.VoiceManager.loudMic = true;
        }

        public static void DisableLoudMicrophone()
        {
            StupidTemplate.Patches.Stuff.VoiceManager.loudMic = false;
        }

        public static void HighPitchMicrophone()
        {
            StupidTemplate.Patches.Stuff.VoiceManager.highPitch = true;
        }

        public static void DisableHighPitchMicrophone()
        {
            StupidTemplate.Patches.Stuff.VoiceManager.highPitch = false;
        }

        public static void SolidWater()
        {
            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (obj.name.ToLower().Contains("water"))
                {
                    obj.layer = LayerMask.NameToLayer("Default");
                }
            }
        }

        public static void DisableSolidWater()
        {
            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (obj.name.ToLower().Contains("water"))
                {
                    obj.layer = 4; // Reset to water layer
                }
            }
        }

        private static GameObject pearl;
        public static void Enderpearl()
        {
            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f)
            {
                if (pearl == null)
                {
                    pearl = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    pearl.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
                    pearl.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    pearl.AddComponent<Rigidbody>().velocity = GorillaTagger.Instance.rightHandTransform.forward * 25f;
                    pearl.GetComponent<Renderer>().material.shader = Shader.Find("Sprites/Default");
                    pearl.GetComponent<Renderer>().material.color = Color.green;
                    GameObject.Destroy(pearl, 4f);
                }
            }
            else if (pearl != null)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position = pearl.transform.position;
                GameObject.Destroy(pearl);
                pearl = null;
            }
        }

        public static void Frozone()
        {
            if (ControllerInputPoller.instance.rightControllerGripFloat > 0.5f)
            {
                GameObject ice = GameObject.CreatePrimitive(PrimitiveType.Cube);
                ice.transform.position = GorillaTagger.Instance.rightHandTransform.position + new Vector3(0, -0.05f, 0);
                ice.transform.localScale = new Vector3(0.3f, 0.05f, 0.3f);
                ice.GetComponent<Renderer>().material.shader = Shader.Find("Sprites/Default");
                ice.GetComponent<Renderer>().material.color = new Color(0.5f, 0.8f, 1f, 0.5f);
                GameObject.Destroy(ice.GetComponent<Rigidbody>()); // No gravity for frozone trails
                GameObject.Destroy(ice, 2f);
            }
            if (ControllerInputPoller.instance.leftControllerGripFloat > 0.5f)
            {
                GameObject ice = GameObject.CreatePrimitive(PrimitiveType.Cube);
                ice.transform.position = GorillaTagger.Instance.leftHandTransform.position + new Vector3(0, -0.05f, 0);
                ice.transform.localScale = new Vector3(0.3f, 0.05f, 0.3f);
                ice.GetComponent<Renderer>().material.shader = Shader.Find("Sprites/Default");
                ice.GetComponent<Renderer>().material.color = new Color(0.5f, 0.8f, 1f, 0.5f);
                GameObject.Destroy(ice.GetComponent<Rigidbody>());
                GameObject.Destroy(ice, 2f);
            }
        }

        public static void PlatformSpam()
        {
            GameObject plat = GameObject.CreatePrimitive(PrimitiveType.Cube);
            plat.transform.position = GorillaTagger.Instance.rightHandTransform.position + new Vector3(0, -0.1f, 0);
            plat.transform.localScale = new Vector3(0.2f, 0.05f, 0.2f);
            plat.GetComponent<Renderer>().material.shader = Shader.Find("Sprites/Default");
            plat.GetComponent<Renderer>().material.color = Color.black;
            GameObject.Destroy(plat, 1f);

            GameObject plat2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            plat2.transform.position = GorillaTagger.Instance.leftHandTransform.position + new Vector3(0, -0.1f, 0);
            plat2.transform.localScale = new Vector3(0.2f, 0.05f, 0.2f);
            plat2.GetComponent<Renderer>().material.shader = Shader.Find("Sprites/Default");
        }

     

        private static float rpcSpamDelay = 0f;


        public static void WaterSpam()
        {
            if (!PhotonNetwork.InRoom) return;
            if (Time.time > rpcSpamDelay)
            {
                rpcSpamDelay = Time.time + 0.1f;
                Classes.DGEasyCode.RPCProtection();
                GorillaTagger.Instance.offlineVRRig.GetComponent<PhotonView>().RPC("RPC_PlaySplashEffect", RpcTarget.Others, new object[] { 
                    GorillaTagger.Instance.headCollider.transform.position,
                    Quaternion.identity,
                    4f,
                    0.1f,
                    true,
                    false
                });
            }
        }
        public static void SpamSlamEffects()
        {
            if (!PhotonNetwork.InRoom || Time.time < rpcSpamDelay) return;
            rpcSpamDelay = Time.time + 0.1f;
            Classes.DGEasyCode.RPCProtection();
            GorillaTagger.Instance.offlineVRRig.GetComponent<PhotonView>().RPC("RPC_PlayHandTap", RpcTarget.Others, new object[] { 
                68,
                false,
                999f
            });
        }

        public static void SpamDrumEffects()
        {
            if (!PhotonNetwork.InRoom || Time.time < rpcSpamDelay) return;
            rpcSpamDelay = Time.time + 0.1f;
            Classes.DGEasyCode.RPCProtection();
            GorillaTagger.Instance.offlineVRRig.GetComponent<PhotonView>().RPC("RPC_PlayDrum", RpcTarget.Others, new object[] { 
                68,
                0.999f
            });
        }

        public static void SpamWhackAMole()
        {
            if (!PhotonNetwork.InRoom || Time.time < rpcSpamDelay) return;
            rpcSpamDelay = Time.time + 0.1f;
            Classes.DGEasyCode.RPCProtection();
            GorillaTagger.Instance.offlineVRRig.GetComponent<PhotonView>().RPC("RPC_PlayHandTap", RpcTarget.Others, new object[] { 
                Random.Range(0, 200),
                false,
                999f
            });
        }

        public static void DisableNetworkTriggers() => StupidTemplate.Patches.Triggers.NetworkTriggerPatch.enabled = true;
        public static void EnableNetworkTriggers() => StupidTemplate.Patches.Triggers.NetworkTriggerPatch.enabled = false;
        public static void LagAll()
        {
            Classes.DGEasyCode.LagAll();
        }

        public static void KickAllMaster()
        {
            Classes.DGEasyCode.KickAll();
        }

        public static void CrashAllMaster()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            Classes.DGEasyCode.RPCProtection();
            for (int i = 0; i < 50; i++)
            {
                GorillaTagger.Instance.offlineVRRig.GetComponent<PhotonView>().RPC("RPC_PlayHandTap", RpcTarget.Others, new object[] { 
                    Random.Range(0, 999),
                    false,
                    999f
                });
            }
        }

        public static void WideMenu()
        {
            StupidTemplate.Settings.menuSize = new Vector3(0.1f, 1.6f, 1.05f);
        }
        public static void DisableWideMenu()
        {
            StupidTemplate.Settings.menuSize = new Vector3(0.1f, 1f, 1.05f);
        }

        public static void TinyMenu()
        {
            StupidTemplate.Settings.menuSize = new Vector3(0.1f, 0.5f, 0.55f);
        }
        public static void DisableTinyMenu()
        {
            StupidTemplate.Settings.menuSize = new Vector3(0.1f, 1f, 1.05f);
        }

        public static void WatchMenu()
        {
            StupidTemplate.Settings.menuSize = new Vector3(0.05f, 0.35f, 0.4f);
        }
        public static void DisableWatchMenu()
        {
            StupidTemplate.Settings.menuSize = new Vector3(0.1f, 1f, 1.05f);
        }

        public static void JoystickFly()
        {
            GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().useGravity = false;
            
            Vector2 joystick = ControllerInputPoller.instance.leftControllerPrimary2DAxis;
            if (joystick.magnitude > 0.1f)
            {
                Transform cam = Camera.main.transform;
                Vector3 move = cam.forward * joystick.y + cam.right * joystick.x;
                GorillaLocomotion.GTPlayer.Instance.transform.position += move * 15f * Time.deltaTime;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        public static void DisableJoystickFly()
        {
            GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().useGravity = true;
        }

        private static bool freeCamEnabled = false;
        public static void FreeCam()
        {
            if (!freeCamEnabled)
            {
                foreach (MeshCollider mc in Resources.FindObjectsOfTypeAll<MeshCollider>())
                    mc.enabled = false;
                freeCamEnabled = true;
            }
            JoystickFly();
        }

        public static void DisableFreeCam()
        {
            if (freeCamEnabled)
            {
                foreach (MeshCollider mc in Resources.FindObjectsOfTypeAll<MeshCollider>())
                    mc.enabled = true;
                freeCamEnabled = false;
            }
            DisableJoystickFly();
        }

        private static bool openOrClose = false;
        public static void ElevatorDoorSpam()
        {
            try
            {
                openOrClose = !openOrClose;
                object instance = typeof(GRElevatorManager).GetField("_instance", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
                if (instance != null)
                {
                    object location = instance.GetType().GetField("currentLocation", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(instance);
                    if (location != null)
                    {
                        typeof(GRElevatorManager).GetMethod("ElevatorButtonPressed", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(instance, new object[] { openOrClose ? GRElevator.ButtonType.Open : GRElevator.ButtonType.Close, location });
                    }
                    else
                    {
                        // Try PascalCase
                        location = instance.GetType().GetField("CurrentLocation", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(instance);
                        if (location != null)
                            typeof(GRElevatorManager).GetMethod("ElevatorButtonPressed", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(instance, new object[] { openOrClose ? GRElevator.ButtonType.Open : GRElevator.ButtonType.Close, location });
                    }
                }
                Classes.DGEasyCode.RPCProtection();
            }
            catch { }
        }

        public static void CopyIdSelf()
        {
            GUIUtility.systemCopyBuffer = PhotonNetwork.LocalPlayer.UserId;
            StupidTemplate.Notifications.NotifiLib.SendNotification("<color=green>[ID]</color> Copied your ID: " + PhotonNetwork.LocalPlayer.UserId);
        }

        public static void CopyIdGun()
        {
            VRRig target = GetGunTarget(out bool isShooting);
            if (target != null && isShooting)
            {
                var player = Classes.RigManager.GetPlayerFromVRRig(target);
                if (player != null)
                {
                    GUIUtility.systemCopyBuffer = player.UserId;
                    StupidTemplate.Notifications.NotifiLib.SendNotification("<color=green>[ID]</color> Copied ID: " + player.UserId);
                }
            }
        }

        public static void CopyIdAll()
        {
            if (!PhotonNetwork.InRoom) return;
            string ids = string.Join(", ", PhotonNetwork.PlayerList.Select(p => p.UserId));
            GUIUtility.systemCopyBuffer = ids;
            StupidTemplate.Notifications.NotifiLib.SendNotification("<color=green>[ID]</color> Copied all IDs in room.");
        }

        private static Texture2D kingTexture = null;
        private static GameObject kingBadgeObject = null;
        private static string kingId = "229E637640C86841";

        public static void KingBadge()
        {
            try
            {
                if (kingTexture == null)
                {
                    string path = Path.Combine(Path.GetDirectoryName(typeof(mods).Assembly.Location), "Menu", "King.png");
                    if (File.Exists(path))
                    {
                        kingTexture = new Texture2D(2, 2);
                        kingTexture.LoadImage(File.ReadAllBytes(path));
                    }
                    else
                    {
                       
                        path = Path.Combine(Path.GetDirectoryName(typeof(mods).Assembly.Location), "King.png");
                        if (File.Exists(path))
                        {
                            kingTexture = new Texture2D(2, 2);
                            kingTexture.LoadImage(File.ReadAllBytes(path));
                        }
                    }
                }

                bool foundKing = false;
                if (PhotonNetwork.InRoom)
                {
                    foreach (var p in PhotonNetwork.PlayerList)
                    {
                        if (p.UserId == kingId)
                        {
                            VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(p);
                            if (vrrig != null)
                            {
                                if (kingBadgeObject == null)
                                {
                                    kingBadgeObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                                    UnityEngine.Object.Destroy(kingBadgeObject.GetComponent<Collider>());
                                    kingBadgeObject.GetComponent<Renderer>().material = new Material(Shader.Find("Sprites/Default"));
                                    if (kingTexture != null) kingBadgeObject.GetComponent<Renderer>().material.mainTexture = kingTexture;
                                    kingBadgeObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                                }
                                
                                kingBadgeObject.transform.position = vrrig.headMesh.transform.position + new Vector3(0, 0.35f, 0);
                                kingBadgeObject.transform.LookAt(Camera.main.transform);
                                kingBadgeObject.transform.Rotate(0, 180, 0);
                                foundKing = true;
                                break;
                            }
                        }
                    }
                }

                if (!foundKing && kingBadgeObject != null)
                {
                    UnityEngine.Object.Destroy(kingBadgeObject);
                    kingBadgeObject = null;
                }
            }
            catch { }
        }

        public static void DisableKingBadge()
        {
            if (kingBadgeObject != null)
            {
                UnityEngine.Object.Destroy(kingBadgeObject);
                kingBadgeObject = null;
            }
        }

        public static void AntiModerator()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerListOthers)
            {
                VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(player);
                if (vrrig != null)
                {
                    string cosmetics = vrrig.GetType().GetField("concatentatedCosmetics", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(vrrig) as string;
                    if (cosmetics == null) cosmetics = vrrig.GetType().GetField("concatenatedCosmetics", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(vrrig) as string;
                    
                    if (cosmetics != null && (cosmetics.Contains("LBAAK") || cosmetics.Contains("LBAAD") || cosmetics.Contains("LMAPY")))
                    {
                        PhotonNetwork.Disconnect();
                        StupidTemplate.Notifications.NotifiLib.SendNotification("<color=red>[SAFETY]</color> Moderator detected! Disconnected.");
                    }
                }
            }
        }

        public static void OpenGtagFolder()
        {
            string filePath = typeof(mods).Assembly.Location.Split(new string[] { "BepInEx" }, StringSplitOptions.None)[0];
            System.Diagnostics.Process.Start(filePath);
        }

        public static void PCMovement()
        {
            float speed = 8f;
            Vector3 move = Vector3.zero;
            if (BepInEx.UnityInput.Current.GetKey(KeyCode.W)) move += Camera.main.transform.forward;
            if (BepInEx.UnityInput.Current.GetKey(KeyCode.S)) move -= Camera.main.transform.forward;
            if (BepInEx.UnityInput.Current.GetKey(KeyCode.D)) move += Camera.main.transform.right;
            if (BepInEx.UnityInput.Current.GetKey(KeyCode.A)) move -= Camera.main.transform.right;

            move.y = 0;
            if (move.magnitude > 0.1f)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += move.normalized * speed * Time.deltaTime;
            }
        }

        private static Vector3? oldLocalPosition = null;
        private static GameObject GunPointer = null;
        public static void PCButtonClick()
        {
            if (Mouse.current.leftButton.isPressed && GunPointer == null)
            {
                if (Main.TPC != null)
                {
                    Ray ray = Main.TPC.ScreenPointToRay(Mouse.current.position.ReadValue());
                    if (Physics.Raycast(ray, out var Ray, 512f, NoInvisLayerMask()))
                    {
                        oldLocalPosition ??= GorillaTagger.Instance.rightHandTriggerCollider.transform.localPosition;
                        GorillaTagger.Instance.rightHandTriggerCollider.GetComponent<TransformFollow>().enabled = false;
                        GorillaTagger.Instance.rightHandTriggerCollider.transform.position = Ray.point;
                    }
                }
            }
            else
            {
                if (oldLocalPosition != null)
                {
                    GorillaTagger.Instance.rightHandTriggerCollider.transform.localPosition = oldLocalPosition.Value;
                    oldLocalPosition = null;
                }
                GorillaTagger.Instance.rightHandTriggerCollider.GetComponent<TransformFollow>().enabled = true;
            }
        }

        public static int NoInvisLayerMask()
        {
            return ~(1 << 2);
        }

        // ===========================
        // ORBITAL SPHERES
        // ===========================
        private static GameObject[] orbitalSpheres = null;
        private static readonly int orbitalSphereCount = 8;

        public static void OrbitalSpheres()
        {
            if (orbitalSpheres == null || orbitalSpheres[0] == null)
            {
                orbitalSpheres = new GameObject[orbitalSphereCount];
                for (int i = 0; i < orbitalSphereCount; i++)
                {
                    orbitalSpheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    GameObject.Destroy(orbitalSpheres[i].GetComponent<Collider>());
                    orbitalSpheres[i].transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                    orbitalSpheres[i].GetComponent<Renderer>().material = new Material(Shader.Find("GUI/Text Shader"));
                }
            }

            Vector3 center = GorillaTagger.Instance.bodyCollider.transform.position;
            float t = Time.time;
            float radius = 0.55f;

            for (int i = 0; i < orbitalSphereCount; i++)
            {
                if (orbitalSpheres[i] == null) continue;
                float angleOffset = (i / (float)orbitalSphereCount) * Mathf.PI * 2f;
                float speed = 2.5f;
                float tiltAngle = (i % 3) * 60f * Mathf.Deg2Rad;

                float angle = t * speed + angleOffset;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                float y = Mathf.Sin(angle + tiltAngle) * 0.3f;

                orbitalSpheres[i].transform.position = center + new Vector3(x, y, z);

                float hue = (i / (float)orbitalSphereCount + t * 0.15f) % 1f;
                orbitalSpheres[i].GetComponent<Renderer>().material.color = Color.HSVToRGB(hue, 1f, 1f);
            }
        }

        public static void DisableOrbitalSpheres()
        {
            if (orbitalSpheres != null)
            {
                foreach (var s in orbitalSpheres)
                    if (s != null) GameObject.Destroy(s);
                orbitalSpheres = null;
            }
        }

        // ===========================
        // LIGHTNING HANDS
        // ===========================
        private static LineRenderer lightningLine = null;

        public static void LightningHands()
        {
            if (lightningLine == null)
            {
                GameObject go = new GameObject("LightningHands");
                lightningLine = go.AddComponent<LineRenderer>();
                lightningLine.material = new Material(Shader.Find("GUI/Text Shader"));
                lightningLine.startWidth = 0.015f;
                lightningLine.endWidth = 0.015f;
                lightningLine.positionCount = 12;
            }

            Vector3 start = GorillaTagger.Instance.leftHandTransform.position;
            Vector3 end = GorillaTagger.Instance.rightHandTransform.position;
            float intensity = Mathf.Sin(Time.time * 30f) * 0.5f + 0.5f;
            Color c = Color.Lerp(Color.cyan, Color.white, intensity);
            lightningLine.startColor = c;
            lightningLine.endColor = c;

            for (int i = 0; i < 12; i++)
            {
                float t = i / 11f;
                Vector3 pos = Vector3.Lerp(start, end, t);
                if (i > 0 && i < 11)
                    pos += UnityEngine.Random.insideUnitSphere * 0.05f;
                lightningLine.SetPosition(i, pos);
            }
        }

        public static void DisableLightningHands()
        {
            if (lightningLine != null)
            {
                GameObject.Destroy(lightningLine.gameObject);
                lightningLine = null;
            }
        }

        // ===========================
        // METEOR SHOWER
        // ===========================
        private static float meteorDelay = 0f;

        public static void MeteorShower()
        {
            if (Time.time < meteorDelay) return;
            meteorDelay = Time.time + 0.3f;

            Vector3 spawnCenter = GorillaTagger.Instance.headCollider.transform.position;
            for (int i = 0; i < 3; i++)
            {
                Vector3 spawnPos = spawnCenter + new Vector3(
                    UnityEngine.Random.Range(-8f, 8f),
                    UnityEngine.Random.Range(12f, 18f),
                    UnityEngine.Random.Range(-8f, 8f)
                );

                GameObject meteor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                meteor.transform.position = spawnPos;
                meteor.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

                Rigidbody rb = meteor.AddComponent<Rigidbody>();
                rb.velocity = new Vector3(
                    UnityEngine.Random.Range(-2f, 2f),
                    UnityEngine.Random.Range(-18f, -12f),
                    UnityEngine.Random.Range(-2f, 2f)
                );

                TrailRenderer trail = meteor.AddComponent<TrailRenderer>();
                trail.time = 0.6f;
                trail.startWidth = 0.12f;
                trail.endWidth = 0f;
                trail.material = new Material(Shader.Find("GUI/Text Shader"));

                float hue = UnityEngine.Random.Range(0f, 1f);
                trail.startColor = Color.HSVToRGB(hue, 0.8f, 1f);
                trail.endColor = Color.clear;

                meteor.GetComponent<Renderer>().material = new Material(Shader.Find("GUI/Text Shader"));
                meteor.GetComponent<Renderer>().material.color = Color.HSVToRGB(hue, 0.6f, 1f);

                GameObject.Destroy(meteor, 4f);
            }
        }

        public static void DisableMeteorShower() { }
    }

   
    public class ManagerRegistry
    {
        public static GhostReactor GhostReactor { get; set; } = new GhostReactor();
        public static SuperInfection SuperInfection { get; set; } = new SuperInfection();
    }
    public class GhostReactor { public GameEntityManager GameEntityManager { get; set; } = new GameEntityManager(); }
    public class SuperInfection { public GameEntityManager GameEntityManager { get; set; } = new GameEntityManager(); }
    public class GameEntityManager : MonoBehaviour
    {
        public int zone = 0;
        public PhotonView photonView;
        public enum RPC { CreateItems }
        public int GetInvalidNetId() => 0;
        public int CreateTypeNetId(int hash) => hash;
        public class RpcSpamChecks
        {
            public class CallLimiters
            {
                public float GetDelay() => 0.1f;
            }
            public Dictionary<int, CallLimiters> m_callLimiters = new Dictionary<int, CallLimiters>();
        }
        public RpcSpamChecks m_RpcSpamChecks = new RpcSpamChecks();
    }
    
    // Removed duplicate NetPlayer mock as it is already in Assembly-CSharp.dll

    public class BitPackUtils
    {
        public static long PackWorldPosForNetwork(Vector3 pos) => 0L; 
        public static long PackQuaternionForNetwork(Quaternion rot) => 0L;
    }
    public class GZipHelper
    {
        public static byte[] CompressBuffer(byte[] buffer)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(ms, CompressionMode.Compress))
                {
                    gzip.Write(buffer, 0, buffer.Length);
                }
                return ms.ToArray();
            }
        }
    }
    public class NetworkSystem
    {
        public static NetworkSystem Instance { get; set; } = new NetworkSystem();
        public bool IsMasterClient => PhotonNetwork.IsMasterClient;
        public Photon.Realtime.Player LocalPlayer => PhotonNetwork.LocalPlayer;
    }
}

