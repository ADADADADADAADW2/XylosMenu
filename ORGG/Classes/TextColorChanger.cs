using UnityEngine;
using UnityEngine.UI;

namespace StupidTemplate.Classes
{
    public class TextColorChanger : TimedBehaviour
    {
        public override void Start()
        {
            base.Start();
            text = base.GetComponent<Text>();
            Update();
        }

        public override void Update()
        {
            base.Update();
            if (colorInfo != null && text != null)
            {
                if (StupidTemplate.Settings.gradientText)
                {
                   
                    // Smooth theme-based transition instead of flashing
                    Color themeColor = StupidTemplate.Settings.textColors[0];
                    Color lighterColor = Color.Lerp(themeColor, Color.white, 0.5f); // 50% lighter
                    float t = Mathf.Sin(Time.time * 2f) * 0.5f + 0.5f; // Smooth wave
                    text.color = Color.Lerp(themeColor, lighterColor, t);
                }
                else if (!colorInfo.copyRigColors)
                {
                    Color color;
                    if (colorInfo.isRainbow)
                    {
                        float h = (Time.frameCount / 180f) % 1f;
                        color = UnityEngine.Color.HSVToRGB(h, 1f, 1f);
                    }
                    else
                    {
                        if (gradient == null)
                        {
                            gradient = new Gradient { colorKeys = colorInfo.colors };
                        }
                        color = gradient.Evaluate((Time.time / 2f) % 1);
                    }
                    text.color = color;
                }
                else
                {
                    text.color = GorillaTagger.Instance.offlineVRRig.mainSkin.material.color;
                }
            }
        }




        public Text text;
        public ExtGradient colorInfo;
        private Gradient gradient;
    }
}

