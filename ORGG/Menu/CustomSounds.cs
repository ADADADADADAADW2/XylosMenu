using System;
using System.IO;
using UnityEngine;
using StupidTemplate.Mods;
using orgg.Menu;
using Photon.Pun;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace StupidTemplate.Menu
{
    public class CustomSounds
    {
        public static string buttonSoudFolder = Path.Combine(SoundBoard.baseSoundPath, "ButtonSOUD");
        
        public static void CycleButtonSound()
        {
            try {
                if (!Directory.Exists(buttonSoudFolder)) return;
                string[] files = Directory.GetFiles(buttonSoudFolder, "*.mp3");
                if (files.Length == 0) return;
                
                int currentIndex = -1;
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].ToLower().Replace("/", "\\") == SoundBoard.currentButtonSoundPath.ToLower().Replace("/", "\\"))
                    {
                        currentIndex = i;
                        break;
                    }
                }
                
                currentIndex = (currentIndex + 1) % files.Length;
                SoundBoard.currentButtonSoundPath = files[currentIndex].Replace("/", "\\");
                

            } catch (Exception ex) {
                UnityEngine.Debug.LogError("Error cycling button sound: " + ex.Message);
            }
        }

        private static bool lastRightTrigger = false;
        private static bool lastLeftTrigger = false;

        public static void Spiderman()
        {
            mods.Spiderman();

            bool rightTrigger = ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f;
            bool leftTrigger = ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f;
            
            if (rightTrigger && !lastRightTrigger)
            {

                SoundBoard.PlaySpecialSound(SoundBoard.slingshotSoundPath);
            }
            if (leftTrigger && !lastLeftTrigger)
            {
                SoundBoard.PlaySpecialSound(SoundBoard.slingshotSoundPath);
            }
            
            lastRightTrigger = rightTrigger;
            lastLeftTrigger = leftTrigger;
        }

        public static void PlatformSpam()
        {
            GameObject plat = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject.Destroy(plat.GetComponent<BoxCollider>());
            plat.transform.position = GorillaTagger.Instance.rightHandTransform.position + new Vector3(0, -0.05f, 0);
            plat.transform.localScale = new Vector3(0.2f, 0.05f, 0.2f);
            plat.GetComponent<Renderer>().material.shader = Shader.Find("Sprites/Default");
            plat.GetComponent<Renderer>().material.color = Color.black;
            GameObject.Destroy(plat, 1f);

            GameObject plat2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject.Destroy(plat2.GetComponent<BoxCollider>());
            plat2.transform.position = GorillaTagger.Instance.leftHandTransform.position + new Vector3(0, -0.05f, 0);
            plat2.transform.localScale = new Vector3(0.2f, 0.05f, 0.2f);
            plat2.GetComponent<Renderer>().material.shader = Shader.Find("Sprites/Default");
            plat2.GetComponent<Renderer>().material.color = Color.black;
            GameObject.Destroy(plat2, 1f);
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

                    string velocity = vrrig.GetComponent<Rigidbody>() != null ? vrrig.GetComponent<Rigidbody>().velocity.magnitude.ToString("F1") : "0.0";
                    string velStr = $"<color=cyan>{velocity}</color>";

                    TextMesh tm = tag.AddComponent<TextMesh>();
                    tm.text = $"USERNAME : {p.NickName.ToUpper()}\nID : {userId}\nPLATFORM : OCULUS\nFPS : {fpsStr}\nVELOCITY : {velStr}\nCREATION : {createDate}\nCOLOR : {colorStr}\nACTOR : {actorStr}\nMASTER : {masterStr}";

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
                    tm2.text = $"<color=black>USERNAME : {p.NickName.ToUpper()}\nID : {userId}\nPLATFORM : OCULUS\nFPS : {fps}\nVELOCITY : {velocity}\nCREATION : {createDate}\nCOLOR : {r} {g} {b}\nACTOR : {actorStr}\nMASTER : {masterStr}</color>";
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

        public static string spotifySong = "Not Playing";
        public static Texture2D spotifyImage = null;
        private static string lastFetchedSong = "";
        private static bool isFetching = false;

        public static void UpdateSpotifyMetadata()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName("Spotify");
                if (processes.Length > 0)
                {
                    string title = "";
                    foreach (var p in processes)
                    {
                        if (!string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle != "Spotify Free" && p.MainWindowTitle != "Spotify Premium" && p.MainWindowTitle != "Spotify")
                        {
                            title = p.MainWindowTitle;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(title))
                    {
                        spotifySong = "Paused / Opening...";
                    }
                    else
                    {
                        spotifySong = title;
                        if (spotifySong != lastFetchedSong && !isFetching)
                        {
                            lastFetchedSong = spotifySong;
                            StupidTemplate.Menu.Main.CoroutineHandler.Instance.StartCoroutine(FetchSpotifyCover(spotifySong));
                        }
                    }
                }
                else
                {
                    spotifySong = "Spotify Closed";
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("Error updating Spotify info: " + ex.Message);
            }
        }

        private static System.Collections.IEnumerator FetchSpotifyCover(string query)
        {
            isFetching = true;
            string url = "https://itunes.apple.com/search?term=" + UnityWebRequest.EscapeURL(query) + "&entity=song&limit=1";
            
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.Success)
                {
                    string json = www.downloadHandler.text;

                    int imgIndex = json.IndexOf("artworkUrl100\":\"");
                    if (imgIndex != -1)
                    {
                        imgIndex += "artworkUrl100\":\"".Length;
                        int end = json.IndexOf("\"", imgIndex);
                        string imgUrl = json.Substring(imgIndex, end - imgIndex).Replace("\\/", "/");
                        

                        imgUrl = imgUrl.Replace("100x100", "600x600");

                        using (UnityWebRequest imgReq = UnityWebRequestTexture.GetTexture(imgUrl))
                        {
                            yield return imgReq.SendWebRequest();
                            if (imgReq.result == UnityWebRequest.Result.Success)
                            {
                                spotifyImage = DownloadHandlerTexture.GetContent(imgReq);
                            }
                        }
                    }
                }
            }
            isFetching = false;
        }
    }
}
