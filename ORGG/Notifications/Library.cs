using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using UnityEngine;
using UnityEngine.UI;
using static StupidTemplate.Settings;
using orgg.Menu;

namespace StupidTemplate.Notifications
{
    [BepInPlugin("org.gorillatag.lars.notifications2", "NotificationLibrary", "1.0.5")]
    public class NotifiLib : BaseUnityPlugin
    {
        public class PCNotification
        {
            public string Text;
            public float TimeAlive;
        }
        
        public static List<PCNotification> ActivePCNotifications = new List<PCNotification>();

        private void Awake()
        {
            base.Logger.LogInfo("Plugin NotificationLibrary is loaded!");
        }

        private void Init()
        {
            this.MainCamera = GameObject.Find("Main Camera");
            this.HUDObj = new GameObject();
            this.HUDObj2 = new GameObject();
            this.HUDObj2.name = "NOTIFICATIONLIB_HUD_OBJ";
            this.HUDObj.name = "NOTIFICATIONLIB_HUD_OBJ";
            this.HUDObj.AddComponent<Canvas>();
            this.HUDObj.AddComponent<CanvasScaler>();
            this.HUDObj.AddComponent<GraphicRaycaster>();
            this.HUDObj.GetComponent<Canvas>().enabled = true;
            this.HUDObj.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            this.HUDObj.GetComponent<Canvas>().worldCamera = this.MainCamera.GetComponent<Camera>();
            this.HUDObj.GetComponent<RectTransform>().sizeDelta = new Vector2(5f, 5f);
            this.HUDObj.GetComponent<RectTransform>().position = new Vector3(this.MainCamera.transform.position.x, this.MainCamera.transform.position.y, this.MainCamera.transform.position.z);
            this.HUDObj2.transform.position = new Vector3(this.MainCamera.transform.position.x, this.MainCamera.transform.position.y, this.MainCamera.transform.position.z - 4.6f);
            this.HUDObj.transform.parent = this.HUDObj2.transform;
            this.HUDObj.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 1.6f);
            Vector3 eulerAngles = this.HUDObj.GetComponent<RectTransform>().rotation.eulerAngles;
            eulerAngles.y = -270f;
            this.HUDObj.transform.localScale = new Vector3(1f, 1f, 1f);
            this.HUDObj.GetComponent<RectTransform>().rotation = Quaternion.Euler(eulerAngles);
            this.Testtext = new GameObject
            {
                transform =
                {
                    parent = this.HUDObj.transform
                }
            }.AddComponent<Text>();
            this.Testtext.text = "";
            this.Testtext.fontSize = 30;
            this.Testtext.font = currentFont;
            this.Testtext.rectTransform.sizeDelta = new Vector2(450f, 210f);
            this.Testtext.alignment = TextAnchor.LowerLeft;
            this.Testtext.rectTransform.localScale = new Vector3(0.00333333333f, 0.00333333333f, 0.33333333f);
            this.Testtext.rectTransform.localPosition = new Vector3(-1f, -1f, -0.5f);
            this.Testtext.material = this.AlertText;
            NotifiLib.NotifiText = this.Testtext;
        }

        private void FixedUpdate()
        {
            bool flag = !this.HasInit && GameObject.Find("Main Camera") != null;
            if (flag)
            {
                this.Init();
                this.HasInit = true;
            }
            this.HUDObj2.transform.position = new Vector3(this.MainCamera.transform.position.x, this.MainCamera.transform.position.y, this.MainCamera.transform.position.z);
            this.HUDObj2.transform.rotation = this.MainCamera.transform.rotation;
            if (this.Testtext.text != "")
            {
                this.NotificationDecayTimeCounter++;
                if (this.NotificationDecayTimeCounter > this.NotificationDecayTime)
                {
                    this.Notifilines = null;
                    this.newtext = "";
                    this.NotificationDecayTimeCounter = 0;
                    this.Notifilines = Enumerable.ToArray<string>(Enumerable.Skip<string>(this.Testtext.text.Split(Environment.NewLine.ToCharArray()), 1));
                    foreach (string text in this.Notifilines)
                    {
                        if (text != "")
                        {
                            this.newtext = this.newtext + text + "\n";
                        }
                    }
                    this.Testtext.text = this.newtext;
                }
            }
            else
            {
                this.NotificationDecayTimeCounter = 0;
            }
        }

        public static void SendNotification(string NotificationText)
        {
            if (!disableNotifications)
            {
                try
                {
                    if (NotifiLib.IsEnabled && NotifiLib.PreviousNotifi != NotificationText)
                    {
                        if (!NotificationText.Contains(Environment.NewLine))
                        {
                            NotificationText += Environment.NewLine;
                        }
                        NotifiLib.NotifiText.text = NotifiLib.NotifiText.text + NotificationText;
                        NotifiLib.NotifiText.supportRichText = true;
                        NotifiLib.PreviousNotifi = NotificationText;

                        ActivePCNotifications.Add(new PCNotification { Text = NotificationText.TrimEnd('\r', '\n'), TimeAlive = 0f });
                    }
                }
                catch
                {
                    UnityEngine.Debug.LogError("Notification failed, object probably nil due to third person ; " + NotificationText);
                }
            }
        }

        public static void ClearAllNotifications()
        {
            NotifiLib.NotifiText.text = "";
            ActivePCNotifications.Clear();
        }

        public static void ClearPastNotifications(int amount)
        {
            string text = "";
            foreach (string text2 in Enumerable.ToArray<string>(Enumerable.Skip<string>(NotifiLib.NotifiText.text.Split(Environment.NewLine.ToCharArray()), amount)))
            {
                if (text2 != "")
                {
                    text = text + text2 + "\n";
                }
            }
            ActivePCNotifications.Clear(); // Keep them locked together
            NotifiLib.NotifiText.text = text;
        }

        private GameObject HUDObj;

        private GameObject HUDObj2;

        private GameObject MainCamera;

        private Text Testtext;

        private Material AlertText = new Material(Shader.Find("GUI/Text Shader"));

        private int NotificationDecayTime = 250;

        private int NotificationDecayTimeCounter;

        public static int NoticationThreshold = 30;

        private string[] Notifilines;

        private string newtext;

        public static string PreviousNotifi;

        private bool HasInit;

        private static Text NotifiText;

        public static bool IsEnabled = true;

        private static Texture2D bgTexture;
        private static Texture2D accentTexture;
        private static GUIStyle notifStyle;

        private void OnGUI()
        {
            if (!enablePcNotifications) return;

            if (bgTexture == null)
            {
                bgTexture = new Texture2D(1, 1);
                bgTexture.SetPixel(0, 0, new Color(0.12f, 0.12f, 0.12f, 0.95f)); // Modern sleek dark box
                bgTexture.Apply();
            }

            if (accentTexture == null)
            {
                accentTexture = new Texture2D(1, 1);
                accentTexture.SetPixel(0, 0, Color.white);
                accentTexture.Apply();
            }

            if (notifStyle == null)
            {
                notifStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 22,
                    fontStyle = FontStyle.Bold,
                    richText = true,
                    wordWrap = false,
                    alignment = TextAnchor.MiddleLeft
                };
                notifStyle.normal.textColor = Color.white;
            }

            GUIStyle titleStyle = new GUIStyle(notifStyle)
            {
                fontSize = 38,
                fontStyle = FontStyle.BoldAndItalic,
                alignment = TextAnchor.UpperRight
            };

            GUIStyle modStyle = new GUIStyle(notifStyle)
            {
                fontSize = 16,
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleRight
            };

            Color themeColor = Color.white;
            if (backgroundColor != null && backgroundColor.colors != null && backgroundColor.colors.Length > 0)
            {
                themeColor = backgroundColor.colors[0].color;
            }

            Color modTextColor = new Color(
                Mathf.Lerp(themeColor.r, 1f, 0.55f),
                Mathf.Lerp(themeColor.g, 1f, 0.55f),
                Mathf.Lerp(themeColor.b, 1f, 0.55f)
            );

            float rightEdge = Screen.width - 10f;
            float curY = 8f;

            GUIContent titleContent = new GUIContent(".orgg");
            Vector2 titleSize = titleStyle.CalcSize(titleContent);
            GUI.color = themeColor;
            GUI.Label(new Rect(rightEdge - titleSize.x, curY, titleSize.x, titleSize.y), titleContent, titleStyle);
            GUI.color = Color.white;
            curY += titleSize.y + 2f;

            float lineWidth = 250f;
            GUI.color = themeColor;
            GUI.DrawTexture(new Rect(rightEdge - lineWidth, curY, lineWidth, 2f), accentTexture);
            GUI.color = Color.white;
            curY += 6f;

            foreach (var category in Buttons.buttons)
            {
                foreach (var button in category)
                {
                    if (button.enabled && button.isTogglable && !button.buttonText.Contains("Counter") && !button.buttonText.Contains("Menu"))
                    {
                        GUIContent bContent = new GUIContent(button.buttonText);
                        Vector2 size = modStyle.CalcSize(bContent);

                        float itemWidth = size.x + 16f;
                        float itemHeight = size.y + 2f;
                        float itemX = rightEdge - itemWidth;

                        GUI.color = new Color(0f, 0f, 0f, 0.4f);
                        GUI.DrawTexture(new Rect(itemX, curY, itemWidth, itemHeight), accentTexture);

                        GUI.color = modTextColor;
                        GUI.Label(new Rect(itemX, curY + 1f, itemWidth - 8f, size.y), bContent, modStyle);
                        GUI.color = Color.white;

                        curY += itemHeight;
                    }
                }
            }

            if (ActivePCNotifications.Count == 0) return;

            float padding = 15f;
            float startY = Screen.height - 50f; 
            float maxLife = 4.0f;

            for (int i = ActivePCNotifications.Count - 1; i >= 0; i--)
            {
                var notif = ActivePCNotifications[i];
                notif.TimeAlive += Time.deltaTime;
                
                if (notif.TimeAlive > maxLife)
                {
                    ActivePCNotifications.RemoveAt(i);
                    continue;
                }

                float slideX = 25f;
                float animLen = 0.25f;
                if (notif.TimeAlive < animLen)
                {

                    float t = notif.TimeAlive / animLen;
                    t = t * t * (3f - 2f * t); // Smoothstep
                    slideX = Mathf.Lerp(-500f, 25f, t);
                }
                else if (notif.TimeAlive > maxLife - animLen)
                {

                    float t = (notif.TimeAlive - (maxLife - animLen)) / animLen;
                    t = t * t * (3f - 2f * t);
                    slideX = Mathf.Lerp(25f, -500f, t);
                }

                GUIContent content = new GUIContent(notif.Text);
                Vector2 textSize = notifStyle.CalcSize(content);

                float boxWidth = textSize.x + (padding * 2f) + 10f;
                float boxHeight = textSize.y + (padding * 1.5f);

                startY -= (boxHeight + 10f); // Move up dynamically dependent on absolute sizes

                Rect boxRect = new Rect(slideX, startY, boxWidth, boxHeight);
                GUI.DrawTexture(boxRect, bgTexture);

                if (backgroundColor != null && backgroundColor.colors != null && backgroundColor.colors.Length > 0)
                {
                    GUI.color = backgroundColor.colors[0].color; // matches the main pulse gradients perfectly
                    GUI.DrawTexture(new Rect(boxRect.x, boxRect.y, 6f, boxRect.height), accentTexture);
                    GUI.color = Color.white;
                }

                Rect textRect = new Rect(boxRect.x + padding + 6f, boxRect.y, textSize.x, boxHeight);
                GUI.Label(textRect, notif.Text, notifStyle);
            }
        }
    }
}


