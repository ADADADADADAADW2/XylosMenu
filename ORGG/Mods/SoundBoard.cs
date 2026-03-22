using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using orgg.Menu;
using StupidTemplate.Classes;
using StupidTemplate.Menu;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

namespace StupidTemplate.Mods
{
    [BepInPlugin("org.orgg.soundboard", "SoundBoardMod", "1.0.0")]
    public class SoundBoard : BaseUnityPlugin
    {
        public static SoundBoard Instance;
        public static List<AudioClip> loadedClips = new List<AudioClip>();
        public static List<string> clipNames = new List<string>();
        
        public static bool transmitToOthers = false;
        private static int soundboardCategoryIndex = 15; 
        
        public static int selectedSoundIndex = 0;
        public static float soundVolume = 1f;
        public static bool noAudioSelf = false;
        private static AudioSource currentLocalSource;

        private void Awake()
        {
            Instance = this;
            LoadSounds();
        }

        public static void LoadSounds()
        {
            if (Instance == null) return;
            string path = Path.Combine(Paths.PluginPath, "SoundBoard");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            loadedClips.Clear();
            clipNames.Clear();
            selectedSoundIndex = 0;

            string[] files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                string lower = file.ToLower();
                if (lower.EndsWith(".wav") || lower.EndsWith(".ogg") || lower.EndsWith(".mp3") || lower.EndsWith(".mp4"))
                {
                    Instance.StartCoroutine(Instance.LoadClip(file));
                }
            }
        }

        private IEnumerator LoadClip(string filePath)
        {
            AudioType audioType = AudioType.UNKNOWN;
            if (filePath.ToLower().EndsWith(".wav")) audioType = AudioType.WAV;
            else if (filePath.ToLower().EndsWith(".ogg")) audioType = AudioType.OGGVORBIS;
            else if (filePath.ToLower().EndsWith(".mp3")) audioType = AudioType.MPEG;
            else if (filePath.ToLower().EndsWith(".mp4")) audioType = AudioType.UNKNOWN;

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + filePath, audioType))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    if (clip != null)
                    {
                        loadedClips.Add(clip);
                        clipNames.Add(Path.GetFileNameWithoutExtension(filePath));
                        RebuildMenuButtons();
                    }
                }
            }
        }

        public static void NextSound()
        {
            if (loadedClips.Count == 0) return;
            selectedSoundIndex = (selectedSoundIndex + 1) % loadedClips.Count;
            UpdateSelectionText();
        }

        public static void PrevSound()
        {
            if (loadedClips.Count == 0) return;
            selectedSoundIndex--;
            if (selectedSoundIndex < 0) selectedSoundIndex = loadedClips.Count - 1;
            UpdateSelectionText();
        }

        public static void VolUp()
        {
            soundVolume += 0.1f;
            if (soundVolume > 1f) soundVolume = 1f;
            UpdateSelectionText();
            if (currentLocalSource != null) currentLocalSource.volume = soundVolume;
        }

        public static void VolDown()
        {
            soundVolume -= 0.1f;
            if (soundVolume < 0f) soundVolume = 0f;
            UpdateSelectionText();
            if (currentLocalSource != null) currentLocalSource.volume = soundVolume;
        }

        public static void ToggleNoAudioSelf()
        {
            noAudioSelf = !noAudioSelf;
            if (noAudioSelf && currentLocalSource != null)
            {
                currentLocalSource.volume = 0f;
            }
        }

        public static void UpdateSelectionText()
        {
            if (Buttons.buttons.Length > soundboardCategoryIndex)
            {
                foreach (var btn in Buttons.buttons[soundboardCategoryIndex])
                {
                    if (btn.buttonText.StartsWith("Selected:"))
                        btn.buttonText = "Selected: " + (clipNames.Count > 0 ? clipNames[selectedSoundIndex] : "None");
                    if (btn.buttonText.StartsWith("Volume:"))
                        btn.buttonText = "Volume: " + Mathf.RoundToInt(soundVolume * 100f) + "%";
                }
            }
        }

        public static void StopSound()
        {
            if (currentLocalSource != null)
            {
                currentLocalSource.Stop();
                GameObject.Destroy(currentLocalSource.gameObject);
                currentLocalSource = null;
            }

            Recorder recorder = PhotonVoiceNetwork.Instance.PrimaryRecorder;
            if (recorder != null)
            {
                // Unhook the audio clip safely and switch back to microphone
                if (recorder.SourceType == Recorder.InputSourceType.AudioClip)
                {
                    recorder.SourceType = Recorder.InputSourceType.Microphone;
                    recorder.RestartRecording(true);
                }
            }
        }

        public static void RebuildMenuButtons()
        {
            if (Buttons.buttons.Length <= soundboardCategoryIndex) return;

            List<ButtonInfo> sboardButtons = new List<ButtonInfo>();
            sboardButtons.Add(new ButtonInfo { buttonText = "home", method = () => Global.ReturnHome(), isTogglable = false, toolTip = "Return to home." });
            sboardButtons.Add(new ButtonInfo { buttonText = "Selected: " + (clipNames.Count > 0 ? clipNames[selectedSoundIndex] : "None"), method = () => { }, isTogglable = false, toolTip = "The currently selected sound." });
            sboardButtons.Add(new ButtonInfo { buttonText = "Next Sound", method = () => NextSound(), isTogglable = false, toolTip = "Next sound in the folder." });
            sboardButtons.Add(new ButtonInfo { buttonText = "Prev Sound", method = () => PrevSound(), isTogglable = false, toolTip = "Previous sound in the folder." });
            sboardButtons.Add(new ButtonInfo { buttonText = "Volume: " + Mathf.RoundToInt(soundVolume * 100f) + "%", method = () => { }, isTogglable = false, toolTip = "Current Volume." });
            sboardButtons.Add(new ButtonInfo { buttonText = "Volume +", method = () => VolUp(), isTogglable = false, toolTip = "Increase volume." });
            sboardButtons.Add(new ButtonInfo { buttonText = "Volume -", method = () => VolDown(), isTogglable = false, toolTip = "Decrease volume." });
            sboardButtons.Add(new ButtonInfo { buttonText = "Play Sound", method = () => PlaySound(selectedSoundIndex), isTogglable = false, toolTip = "Play the selected sound." });
            sboardButtons.Add(new ButtonInfo { buttonText = "Stop Sound", method = () => StopSound(), isTogglable = false, toolTip = "Stop the currently playing sound." });
            sboardButtons.Add(new ButtonInfo { buttonText = "Refresh", method = () => LoadSounds(), isTogglable = false, toolTip = "Reloads sounds from the SoundBoard folder." });
            sboardButtons.Add(new ButtonInfo { buttonText = "Transmit Over Mic", method = () => ToggleMic(), enableMethod = () => ToggleMic(true), disableMethod = () => ToggleMic(false), isTogglable = true, enabled = transmitToOthers, toolTip = "If ON, others hear the sounds played." });
            sboardButtons.Add(new ButtonInfo { buttonText = "No Audio Self", method = () => ToggleNoAudioSelf(), enableMethod = () => { noAudioSelf = true; }, disableMethod = () => { noAudioSelf = false; }, isTogglable = true, enabled = noAudioSelf, toolTip = "If ON, you won't hear the played sounds locally." });
            
            for (int i = 0; i < loadedClips.Count; i++)
            {
                int index = i; 
                sboardButtons.Add(new ButtonInfo {
                    buttonText = clipNames[index],
                    method = () => { selectedSoundIndex = index; UpdateSelectionText(); PlaySound(index); },
                    isTogglable = false,
                    toolTip = "Plays " + clipNames[index]
                });
            }

            Buttons.buttons[soundboardCategoryIndex] = sboardButtons.ToArray(); 
        }

        public static void ToggleMic(bool force = false)
        {
            transmitToOthers = !transmitToOthers;
        }

        public static void PlaySound(int index)
        {
            StopSound();

            if (index < 0 || index >= loadedClips.Count) return;
            AudioClip clip = loadedClips[index];

            if (transmitToOthers)
            {
                Recorder recorder = PhotonVoiceNetwork.Instance.PrimaryRecorder;
                if (recorder != null)
                {
                    recorder.SourceType = Recorder.InputSourceType.AudioClip;
                    recorder.AudioClip = clip;
                    recorder.LoopAudioClip = false;
                    recorder.RestartRecording(true);
                    
                    Instance.StartCoroutine(Instance.ResetMicAfter(clip.length));
                }
            }

            // Always play locally so the user can hear their own transmitted sound, unless No Audio Self is checked.
            if (!noAudioSelf)
            {
                PlayLocally(clip);
            }
        }

        private static void PlayLocally(AudioClip clip)
        {
            GameObject audioObj = new GameObject("LocalSound");
            audioObj.transform.position = GorillaTagger.Instance.headCollider.transform.position;
            currentLocalSource = audioObj.AddComponent<AudioSource>();
            currentLocalSource.clip = clip;
            currentLocalSource.volume = soundVolume;
            currentLocalSource.Play();
            GameObject.Destroy(audioObj, clip.length + 0.1f);
        }

        private IEnumerator ResetMicAfter(float length)
        {
            yield return new WaitForSeconds(length);
            StopSound();
        }
        public static void PlaySpecialSound(string filePath)
        {
            if (Instance == null) return;
            Instance.StartCoroutine(Instance.PlayFromPath(filePath));
        }

        private IEnumerator PlayFromPath(string filePath)
        {
            if (!File.Exists(filePath)) yield break;
            
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + filePath, AudioType.MPEG))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    if (clip != null) PlayLocally(clip);
                }
            }
        }

        public static string baseSoundPath = Path.Combine(Paths.PluginPath, "AetherSounds");
        public static string currentButtonSoundPath = Path.Combine(Paths.PluginPath, "AetherSounds", "ButtonSOUD", "button-click.mp3");
        public static string bootupSoundPath = Path.Combine(Paths.PluginPath, "AetherSounds", "modsSOUD", "oculus-quest-2-boot-up-sfx.mp3");
        public static string shutdownSoundPath = Path.Combine(Paths.PluginPath, "AetherSounds", "modsSOUD", "oculus-quest-2-shutdown-sfx.mp3");
        public static string slingshotSoundPath = Path.Combine(Paths.PluginPath, "AetherSounds", "modsSOUD", "roblox-slingshot_MLNAoYk.mp3");
    }
}

