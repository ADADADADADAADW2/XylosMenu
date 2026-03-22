using Photon.Voice;
using System;
using UnityEngine;

namespace StupidTemplate.Patches.Stuff
{
    public class VoiceManager : IAudioDesc, IAudioReader<float>, IDisposable
    {
        public static VoiceManager instance;
        public static bool loudMic = false;
        public static bool highPitch = false;

        private string deviceName;
        private AudioClip micClip;
        private int lastReadPos = 0;

        public int SamplingRate => highPitch ? 24000 : 48000;
        public int Channels => 1;
        public string Error => null;

        public static VoiceManager Get()
        {
            if (instance == null) instance = new VoiceManager();
            return instance;
        }

        public VoiceManager()
        {
            if (Microphone.devices.Length > 0)
            {
                deviceName = Microphone.devices[0];
                micClip = Microphone.Start(deviceName, true, 10, 48000);
            }
        }

        public void Dispose()
        {
            Microphone.End(deviceName);
        }

        public bool Read(float[] buffer)
        {
            if (micClip == null) return false;

            int micPos = Microphone.GetPosition(deviceName);
            if (micPos < 0 || micPos == lastReadPos) return false;

            int samplesToRead = buffer.Length;
            float[] temp = new float[samplesToRead];
            
            micClip.GetData(temp, lastReadPos);
            lastReadPos = (lastReadPos + samplesToRead) % micClip.samples;

            for (int i = 0; i < buffer.Length; i++)
            {
                float sample = temp[i];
                if (loudMic)
                {
                    sample *= 1500f; // Distort
                }
                buffer[i] = sample;
            }

            return true;
        }
    }
}
