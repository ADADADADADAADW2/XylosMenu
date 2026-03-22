using System;
using System.Linq;
using UnityEngine;
using BepInEx;
using orgg.Menu;
using StupidTemplate.Mods;
using StupidTemplate.Classes;

namespace StupidTemplate.Menu
{
    [BepInPlugin("org.orgg.pcgui", "PCGUI", "1.0.0")]
    public class PCGUI : BaseUnityPlugin
    {
        private Rect windowRect = new Rect(20, 20, 900, 550);
        private bool showMenu = true;
        private float menuAlpha = 1f;
        private float menuScale = 1f;

        private Texture2D bgTexture;
        private Texture2D topBarTexture;
        private Texture2D btnTexture;
        private Texture2D btnHoverTexture;
        private Texture2D btnActiveTexture;
        private Texture2D underlineTexture;
        private Texture2D watermarkTexture;

        private GUIStyle windowStyle;
        private GUIStyle tabStyle;
        private GUIStyle tabActiveStyle;
        private GUIStyle titleStyle;
        private GUIStyle versionStyle;
        private GUIStyle searchStyle;
        private GUIStyle modBtnStyle;
        private GUIStyle modBtnActiveStyle;

        private Vector2 scrollPosition;
        private Vector2 tabScrollPosition;
        private string searchText = "";

        private bool secretTabUnlocked = false;
        private string secretKeyInput = "";
        
        private string accessDeniedMessage = "";
        private float accessDeniedTime = 0f;
        
        // Beta GUI Window
        public static bool showBetaGUI = false;
        private Rect betaWindowRect = new Rect(950, 20, 600, 500);

        // Animation states
        private float animatedTabX = 20f;
        private float animatedTabWidth = 0f;
        private float targetTabX = 20f;
        private float targetTabWidth = 0f;

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private void OnEnable()
        {
            bgTexture = MakeTex(2, 2, new Color(0.04f, 0.04f, 0.05f, 0.5f)); // Transparent bg
            topBarTexture = MakeTex(2, 2, new Color(0.06f, 0.06f, 0.08f, 0.4f));
            btnTexture = MakeTex(2, 2, new Color(1f, 1f, 1f, 0.02f)); 
            btnHoverTexture = MakeTex(2, 2, new Color(1f, 1f, 1f, 0.05f));
            btnActiveTexture = MakeTex(2, 2, new Color(1f, 1f, 1f, 0.02f)); 
            underlineTexture = MakeTex(2, 2, new Color(0.45f, 0.2f, 0.9f, 1f));
            
            // Load watermark logo
            LoadWatermark();
        }

        private void LoadWatermark()
        {
            string path = System.IO.Path.Combine(BepInEx.Paths.PluginPath, "StupidTemplate", "Menu", "logo.png");
            if (!System.IO.File.Exists(path)) 
                path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "BepInEx", "plugins", "StupidTemplate", "Menu", "logo.png");
            
            if (System.IO.File.Exists(path))
            {
                byte[] fileData = System.IO.File.ReadAllBytes(path);
                watermarkTexture = new Texture2D(2, 2);
                watermarkTexture.LoadImage(fileData);
            }
        }

        private void Update()
        {
            if (UnityInput.Current.GetKeyDown(KeyCode.L))
            {
                showMenu = !showMenu;
                StupidTemplate.Settings.enablePcGUI = showMenu; 
            }

            if (showMenu != StupidTemplate.Settings.enablePcGUI)
            {
                showMenu = StupidTemplate.Settings.enablePcGUI;
            }

            // Smooth Window Fade In/Out
            float delta = Time.deltaTime * 15f;
            menuAlpha = Mathf.Lerp(menuAlpha, showMenu ? 1f : 0f, delta);
            menuScale = Mathf.Lerp(menuScale, showMenu ? 1f : 0.95f, delta);

            // Smooth Tab Sliding
            animatedTabX = Mathf.Lerp(animatedTabX, targetTabX, delta);
            animatedTabWidth = Mathf.Lerp(animatedTabWidth, targetTabWidth, delta);
        }

        private void OnGUI()
        {
            if (menuAlpha < 0.01f) return;

            InitStyles();

            GUI.color = new Color(1, 1, 1, menuAlpha);
            GUI.backgroundColor = Color.clear;

            // Apply slight scale pop effect by shifting rect temporarily
            float w = windowRect.width * menuScale;
            float h = windowRect.height * menuScale;
            float cx = windowRect.x + (windowRect.width - w) / 2f;
            float cy = windowRect.y + (windowRect.height - h) / 2f;
            Rect scaledRect = new Rect(cx, cy, w, h);

            scaledRect = GUI.Window(999, scaledRect, DrawWindow, "");
            
            // Re-apply dragged rect mapping back to standard
            if (showMenu)
            {
                if (GUI.changed || Event.current.type == EventType.MouseDrag)
                {
                    windowRect.x = scaledRect.x;
                    windowRect.y = scaledRect.y;
                }
            }
            
            // Draw Beta GUI window if unlocked and enabled
            if (showBetaGUI && secretTabUnlocked)
            {
                GUI.color = Color.white;
                betaWindowRect = GUI.Window(1000, betaWindowRect, DrawBetaWindow, "");
            }
        }

        private void InitStyles()
        {
            if (windowStyle == null)
            {
                windowStyle = new GUIStyle(GUI.skin.window);
                windowStyle.normal.background = bgTexture;
                windowStyle.onNormal.background = bgTexture;
                windowStyle.padding = new RectOffset(0, 0, 0, 0);
                windowStyle.border = new RectOffset(0, 0, 0, 0);

                tabStyle = new GUIStyle(GUI.skin.button);
                tabStyle.normal.background = null;
                tabStyle.hover.background = null;
                tabStyle.active.background = null;
                tabStyle.normal.textColor = new Color(0.4f, 0.4f, 0.45f);
                tabStyle.hover.textColor = new Color(0.8f, 0.8f, 0.85f);
                tabStyle.fontSize = 14;
                tabStyle.alignment = TextAnchor.MiddleCenter;

                tabActiveStyle = new GUIStyle(tabStyle);
                tabActiveStyle.normal.textColor = Color.white;
                tabActiveStyle.hover.textColor = Color.white;
                tabActiveStyle.fontStyle = FontStyle.Bold;

                titleStyle = new GUIStyle(GUI.skin.label);
                titleStyle.fontSize = 22;
                titleStyle.fontStyle = FontStyle.Bold;
                titleStyle.normal.textColor = new Color(0.55f, 0.3f, 1f); 

                versionStyle = new GUIStyle(GUI.skin.label);
                versionStyle.fontSize = 12;
                versionStyle.normal.textColor = new Color(0.4f, 0.4f, 0.45f);

                searchStyle = new GUIStyle(GUI.skin.textField);
                searchStyle.normal.background = btnTexture;
                searchStyle.hover.background = btnHoverTexture;
                searchStyle.focused.background = btnHoverTexture;
                searchStyle.normal.textColor = new Color(0.4f, 0.4f, 0.7f);
                searchStyle.alignment = TextAnchor.MiddleLeft;

                modBtnStyle = new GUIStyle(GUI.skin.button);
                modBtnStyle.normal.background = btnTexture;
                modBtnStyle.hover.background = btnHoverTexture;
                modBtnStyle.active.background = btnActiveTexture;
                modBtnStyle.normal.textColor = new Color(0.6f, 0.6f, 0.65f);
                modBtnStyle.hover.textColor = Color.white;
                modBtnStyle.alignment = TextAnchor.MiddleLeft;
                modBtnStyle.padding = new RectOffset(20, 10, 0, 0);
                modBtnStyle.fontSize = 15;
                modBtnStyle.fontStyle = FontStyle.Bold;

                modBtnActiveStyle = new GUIStyle(modBtnStyle);
                modBtnActiveStyle.normal.textColor = Color.white;
                modBtnActiveStyle.normal.background = btnActiveTexture;
                modBtnActiveStyle.hover.background = btnActiveTexture;
            }
        }

        private void DrawWindow(int windowID)
        {
            GUI.DrawTexture(new Rect(0, 0, windowRect.width, windowRect.height), bgTexture);
            GUI.DrawTexture(new Rect(0, 0, windowRect.width, 50), topBarTexture);

            // Pulsing title color animation
            float pulse = Mathf.Sin(Time.time * 4f) * 0.5f + 0.5f;
            titleStyle.normal.textColor = Color.Lerp(new Color(0.45f, 0.2f, 0.9f), new Color(0.7f, 0.4f, 1f), pulse);
            
            GUI.Label(new Rect(25, 12, 100, 30), StupidTemplate.PluginInfo.Name, titleStyle);
            GUI.Label(new Rect(95, 20, 100, 30), "v" + StupidTemplate.PluginInfo.Version, versionStyle);

            float searchWidth = 180f;

            // Search bar - centered in top bar
            Rect searchRect = new Rect(windowRect.width - searchWidth - 20, 15, searchWidth, 24);
            searchText = GUI.TextField(searchRect, searchText, searchStyle);
            if (string.IsNullOrEmpty(searchText))
            {
                GUI.Label(searchRect, "  🔍 search...", new GUIStyle(GUI.skin.label) { normal = { textColor = new Color(0.3f, 0.3f, 0.4f) }, fontSize = 13 });
            }

            // Draw watermark if texture is loaded
            if (watermarkTexture != null)
            {
                float logoSize = 35f;
                Rect logoRect = new Rect(windowRect.width - searchWidth - logoSize - 30, 8, logoSize, logoSize);
                GUI.DrawTexture(logoRect, watermarkTexture, ScaleMode.ScaleToFit);
            }

            float tabHeight = 40f;
            GUI.DrawTexture(new Rect(0, 50, windowRect.width, tabHeight), MakeTex(2, 2, new Color(0.05f, 0.05f, 0.07f, 0.5f)));

            tabScrollPosition = GUI.BeginScrollView(
                new Rect(20, 50, windowRect.width - 40, tabHeight), 
                tabScrollPosition, 
                new Rect(0, 0, 1200, tabHeight), 
                false, false, GUIStyle.none, GUIStyle.none);

            float currentX = 0f;

            DrawTab("user", 0, ref currentX);

            if (Buttons.buttons.Length > 0)
            {
                foreach (var catBtn in Buttons.buttons[0])
                {
                    if (catBtn.buttonText.ToLower() != "settings" && !catBtn.buttonText.ToLower().Contains("home"))
                    {
                        DrawTab(catBtn.buttonText, -1, ref currentX, catBtn);
                    }
                }
            }

            // Remove secret unlocked tab
            // Draw the smooth sliding underline
            if (animatedTabWidth > 0)
            {
                GUI.DrawTexture(new Rect(animatedTabX + 10, tabHeight - 2, animatedTabWidth - 20, 2), underlineTexture);
            }

            GUI.EndScrollView();

            GUILayout.BeginArea(new Rect(25, 105, windowRect.width - 50, windowRect.height - 120));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUIStyle.none);

            if (Main.buttonsType == 0) // ? Tab (Home) - User Info
            {
                DrawUserLayout();
            }
            else if (Main.buttonsType == 14) // Spotify Custom Layout
            {
                DrawSpotifyLayout();
            }
            else
            {
                var currentButtons = Buttons.buttons[Main.buttonsType];
                var displayButtons = currentButtons.Where(b => b.buttonText.ToLower() != "home" && b.buttonText.ToLower() != "disconnect").ToArray();

                if (!string.IsNullOrEmpty(searchText))
                {
                    displayButtons = Buttons.buttons.SelectMany(x => x)
                        .Where(b => b.buttonText.ToLower().Contains(searchText.ToLower()) && b.buttonText.ToLower() != "home" && b.buttonText.ToLower() != "disconnect")
                        .ToArray();
                }

                float btnHeight = 45f;
                float spacingY = 12f;
                float spacingX = 20f;

                int columns = 2;
                int rows = Mathf.CeilToInt((float)displayButtons.Length / columns);
                float btnWidth = (windowRect.width - 50 - spacingX) / 2;

                for (int i = 0; i < displayButtons.Length; i++)
                {
                    int row = i / columns;
                    int col = i % columns;

                    float xPos = col * (btnWidth + spacingX);
                    float yPos = row * (btnHeight + spacingY);

                    Rect btnRect = new Rect(xPos, yPos, btnWidth, btnHeight);
                    var btn = displayButtons[i];

                    bool isEnabled = btn.enabled;
                    GUIStyle activeStyle = isEnabled ? modBtnActiveStyle : modBtnStyle;

                    if (GUI.Button(btnRect, btn.buttonText, activeStyle))
                    {
                        if (btn.isTogglable)
                        {
                            btn.enabled = !btn.enabled;
                            if (btn.enabled)
                            {
                                if (btn.enableMethod != null) btn.enableMethod.Invoke();
                                else if (btn.method != null) btn.method.Invoke();
                            }
                            else
                            {
                                if (btn.disableMethod != null) btn.disableMethod.Invoke();
                            }
                        }
                        else
                        {
                            if (btn.method != null) btn.method.Invoke();
                        }
                    }
                }
                GUILayout.Space(rows * (btnHeight + spacingY));
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private Texture2D spotifyCoverTex;

        private void DrawSpotifyLayout()
        {
            if (spotifyCoverTex == null)
            {
                string path = System.IO.Path.Combine(BepInEx.Paths.PluginPath, "StupidTemplate", "Menu", "spotify_placeholder.png");
                if (!System.IO.File.Exists(path)) path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "BepInEx", "plugins", "StupidTemplate", "Menu", "spotify_placeholder.png");
                
                if (System.IO.File.Exists(path))
                {
                    byte[] fileData = System.IO.File.ReadAllBytes(path);
                    spotifyCoverTex = new Texture2D(2, 2);
                    spotifyCoverTex.LoadImage(fileData);
                }
                else
                {
                    // Fallback transparent/dark texture
                    spotifyCoverTex = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.5f));
                }
            }

            float contentWidth = windowRect.width - 50;
            
            // Layout dimensions based on screenshot
            float imageSize = 240f;
            float centerX = (contentWidth - imageSize) / 2f;
            
            // Image
            Rect imageRect = new Rect(centerX, 20f, imageSize, imageSize);
            Texture2D imgToDraw = StupidTemplate.Menu.CustomSounds.spotifyImage != null ? StupidTemplate.Menu.CustomSounds.spotifyImage : spotifyCoverTex;
            GUI.DrawTexture(imageRect, imgToDraw, ScaleMode.ScaleToFit);
            
            // Outline for image
            GUI.DrawTexture(new Rect(imageRect.x, imageRect.y, imageRect.width, 2), underlineTexture); // Top
            GUI.DrawTexture(new Rect(imageRect.x, imageRect.y + imageRect.height - 2, imageRect.width, 2), underlineTexture); // Bottom
            GUI.DrawTexture(new Rect(imageRect.x, imageRect.y, 2, imageRect.height), underlineTexture); // Left
            GUI.DrawTexture(new Rect(imageRect.x + imageRect.width - 2, imageRect.y, 2, imageRect.height), underlineTexture); // Right

            // Song name
            GUIStyle songStyle = new GUIStyle(GUI.skin.label);
            songStyle.fontSize = 22;
            songStyle.fontStyle = FontStyle.Bold;
            songStyle.alignment = TextAnchor.MiddleCenter;
            songStyle.normal.textColor = Color.white;
            
            Rect songRect = new Rect(centerX, imageRect.yMax + 15, imageSize, 30);
            GUI.Label(songRect, StupidTemplate.Menu.CustomSounds.spotifySong, songStyle);

            // Outline for song name
            GUI.DrawTexture(new Rect(songRect.x, songRect.y, songRect.width, 2), underlineTexture); // Top
            GUI.DrawTexture(new Rect(songRect.x, songRect.y + songRect.height - 2, songRect.width, 2), underlineTexture); // Bottom
            GUI.DrawTexture(new Rect(songRect.x, songRect.y, 2, songRect.height), underlineTexture); // Left
            GUI.DrawTexture(new Rect(songRect.x + songRect.width - 2, songRect.y, 2, songRect.height), underlineTexture); // Right

            // Controls
            float btnWidth = 80f;
            float btnHeight = 40f;
            float spacing = 20f;
            float totalControlsWidth = (btnWidth * 3) + (spacing * 2);
            float startX = (contentWidth - totalControlsWidth) / 2f;
            float controlsY = songRect.yMax + 20f;

            GUIStyle controlBtnStyle = new GUIStyle(modBtnStyle);
            controlBtnStyle.alignment = TextAnchor.MiddleCenter;
            controlBtnStyle.padding = new RectOffset(0, 0, 0, 0);

            // Previous
            Rect prevRect = new Rect(startX, controlsY, btnWidth, btnHeight);
            if (GUI.Button(prevRect, "Prev", controlBtnStyle))
            {
                StupidTemplate.Menu.mods.SpotifyPrevious();
            }
            
            // Outline Prev
            GUI.DrawTexture(new Rect(prevRect.x, prevRect.y, prevRect.width, 2), underlineTexture);
            GUI.DrawTexture(new Rect(prevRect.x, prevRect.y + prevRect.height - 2, prevRect.width, 2), underlineTexture);
            GUI.DrawTexture(new Rect(prevRect.x, prevRect.y, 2, prevRect.height), underlineTexture);
            GUI.DrawTexture(new Rect(prevRect.x + prevRect.width - 2, prevRect.y, 2, prevRect.height), underlineTexture);

            // Play/Pause
            Rect playRect = new Rect(startX + btnWidth + spacing, controlsY, btnWidth, btnHeight);
            if (GUI.Button(playRect, "Pause", controlBtnStyle))
            {
                StupidTemplate.Menu.mods.SpotifyPlayPause();
            }

            // Outline Play
            GUI.DrawTexture(new Rect(playRect.x, playRect.y, playRect.width, 2), underlineTexture);
            GUI.DrawTexture(new Rect(playRect.x, playRect.y + playRect.height - 2, playRect.width, 2), underlineTexture);
            GUI.DrawTexture(new Rect(playRect.x, playRect.y, 2, playRect.height), underlineTexture);
            GUI.DrawTexture(new Rect(playRect.x + playRect.width - 2, playRect.y, 2, playRect.height), underlineTexture);

            // Next
            Rect nextRect = new Rect(startX + (btnWidth * 2) + (spacing * 2), controlsY, btnWidth, btnHeight);
            if (GUI.Button(nextRect, "Next", controlBtnStyle))
            {
                StupidTemplate.Menu.mods.SpotifyNext();
            }

            // Outline Next
            GUI.DrawTexture(new Rect(nextRect.x, nextRect.y, nextRect.width, 2), underlineTexture);
            GUI.DrawTexture(new Rect(nextRect.x, nextRect.y + nextRect.height - 2, nextRect.width, 2), underlineTexture);
            GUI.DrawTexture(new Rect(nextRect.x, nextRect.y, 2, nextRect.height), underlineTexture);
            GUI.DrawTexture(new Rect(nextRect.x + nextRect.width - 2, nextRect.y, 2, nextRect.height), underlineTexture);

            GUILayout.Space(controlsY + btnHeight + 50f);
        }

        private void DrawUserLayout()
        {
            float contentWidth = windowRect.width - 50;
            float startY = 30f;
            
            GUIStyle headerStyle = new GUIStyle(GUI.skin.label);
            headerStyle.fontSize = 24;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.alignment = TextAnchor.MiddleCenter;
            headerStyle.normal.textColor = Color.white;
            
            GUIStyle infoStyle = new GUIStyle(GUI.skin.label);
            infoStyle.fontSize = 16;
            infoStyle.alignment = TextAnchor.MiddleCenter;
            infoStyle.normal.textColor = new Color(0.8f, 0.8f, 0.8f);
            
            // Header
            GUI.Label(new Rect(25, startY, contentWidth, 30), "User Profile", headerStyle);
            startY += 50;
            
            // Player Info
            string playerName = Photon.Pun.PhotonNetwork.LocalPlayer != null ? Photon.Pun.PhotonNetwork.LocalPlayer.NickName : "Unknown";
            GUI.Label(new Rect(25, startY, contentWidth, 25), $"Name: {playerName}", infoStyle);
            startY += 30;
            
            bool inRoom = Photon.Pun.PhotonNetwork.InRoom;
            string roomName = inRoom ? Photon.Pun.PhotonNetwork.CurrentRoom.Name : "Not in room";
            GUI.Label(new Rect(25, startY, contentWidth, 25), $"Room: {roomName}", infoStyle);
            startY += 50;
            
            // Secret Key Section
            GUI.DrawTexture(new Rect(100, startY, contentWidth - 150, 2), underlineTexture);
            startY += 30;
            
            GUI.Label(new Rect(25, startY, contentWidth, 30), "Beta Access", headerStyle);
            startY += 40;
            
            if (!secretTabUnlocked)
            {
                GUIStyle inputStyle = new GUIStyle(GUI.skin.textField);
                inputStyle.fontSize = 16;
                inputStyle.alignment = TextAnchor.MiddleCenter;
                inputStyle.normal.textColor = Color.white;
                inputStyle.normal.background = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.8f));
                
                float inputWidth = 200f;
                Rect inputRect = new Rect((windowRect.width - inputWidth) / 2f, startY, inputWidth, 35);
                secretKeyInput = GUI.TextField(inputRect, secretKeyInput, inputStyle);
                startY += 50;
                
                float btnWidth = 120f;
                Rect submitRect = new Rect((windowRect.width - btnWidth) / 2f, startY, btnWidth, 35);
                
                if (GUI.Button(submitRect, "Unlock", modBtnStyle))
                {
                    var status = StupidTemplate.Settings.CheckBetaAccess(secretKeyInput);
                    if (status == StupidTemplate.Settings.BetaAccessStatus.Granted)
                    {
                        secretTabUnlocked = true;
                        showBetaGUI = true;
                        secretKeyInput = "";
                        accessDeniedMessage = "";
                    }
                    else if (status == StupidTemplate.Settings.BetaAccessStatus.InvalidID)
                    {
                        accessDeniedMessage = "Access Denied: Key not bound to this ID";
                        accessDeniedTime = Time.time;
                        secretKeyInput = "";
                    }
                    else
                    {
                        accessDeniedMessage = "Access Denied: Invalid Key";
                        accessDeniedTime = Time.time;
                        secretKeyInput = "";
                    }
                }
                
                // Show access denied message
                if (!string.IsNullOrEmpty(accessDeniedMessage) && Time.time - accessDeniedTime < 3f)
                {
                    startY += 40;
                    GUIStyle errorStyle = new GUIStyle(infoStyle);
                    errorStyle.normal.textColor = new Color(1f, 0.3f, 0.3f);
                    GUI.Label(new Rect(25, startY, contentWidth, 25), accessDeniedMessage, errorStyle);
                }
            }
            else
            {
                GUIStyle successStyle = new GUIStyle(infoStyle);
                successStyle.normal.textColor = Color.green;
                successStyle.fontStyle = FontStyle.Bold;
                
                GUI.Label(new Rect(25, startY, contentWidth, 25), "Unlocked!", successStyle);
                startY += 40;
                
                float btnWidth = 180f;
                float spacing = 20f;
                float totalWidth = (btnWidth * 2) + spacing;
                float startX = (windowRect.width - totalWidth) / 2f;
                
                Rect betaBtnRect = new Rect(startX, startY, btnWidth, 40);
                if (GUI.Button(betaBtnRect, showBetaGUI ? "Close Beta GUI" : "Open Beta GUI", modBtnStyle))
                {
                    showBetaGUI = !showBetaGUI;
                }
                
                Rect logoutBtnRect = new Rect(startX + btnWidth + spacing, startY, btnWidth, 40);
                if (GUI.Button(logoutBtnRect, "Log Out", modBtnStyle))
                {
                    secretTabUnlocked = false;
                    showBetaGUI = false;
                    secretKeyInput = "";
                }
            }
            
            GUILayout.Space(startY + 100f);
        }

        private void DrawBetaWindow(int windowID)
        {
            // Background and top bar
            GUI.DrawTexture(new Rect(0, 0, betaWindowRect.width, betaWindowRect.height), bgTexture);
            GUI.DrawTexture(new Rect(0, 0, betaWindowRect.width, 50), topBarTexture);
            
            GUIStyle betaTitleStyle = new GUIStyle(GUI.skin.label);
            betaTitleStyle.fontSize = 20;
            betaTitleStyle.fontStyle = FontStyle.Bold;
            betaTitleStyle.alignment = TextAnchor.MiddleLeft;
            betaTitleStyle.normal.textColor = Color.white;
            
            GUI.Label(new Rect(20, 15, 200, 20), "Beta Mods", betaTitleStyle);
            
            // Close button
            if (GUI.Button(new Rect(betaWindowRect.width - 40, 15, 20, 20), "X", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 16, normal = { textColor = Color.white } }))
            {
                showBetaGUI = false;
            }
            
            // Content area
            GUILayout.BeginArea(new Rect(20, 70, betaWindowRect.width - 40, betaWindowRect.height - 90));
            
            string[] betaModNames = new string[] {
                "Lag All",
                "Kick All",
                "Barrel Fling Gun",
                "Barrel Fling Aura"
            };
            
            System.Action[] betaModActions = new System.Action[] {
                () => mods.LagAll(),
                () => mods.KickAllMaster(),
                () => mods.BarrelFlingGun(),
                () => mods.BarrelFlingAura()
            };
            
            for (int i = 0; i < betaModNames.Length; i++)
            {
                if (GUILayout.Button(betaModNames[i], modBtnStyle, GUILayout.Height(35)))
                {
                    betaModActions[i].Invoke();
                }
                GUILayout.Space(5);
            }
            
            GUILayout.EndArea();
            
            // Make window draggable
            GUI.DragWindow(new Rect(0, 0, betaWindowRect.width, 50));
        }

        private void DrawTab(string name, int defaultTargetPage, ref float currentX, ButtonInfo btn = null)
        {
            GUIContent content = new GUIContent(name.ToLower());
            Vector2 size = tabStyle.CalcSize(content);
            float width = size.x + 24f;
            Rect tabRect = new Rect(currentX, 0, width, 40);

            bool isActive = false;
            if (name == "user" && Main.buttonsType == 0) isActive = true;
            else if (btn != null && GetCategoryIndex(btn) == Main.buttonsType) isActive = true;

            GUIStyle styleToUse = isActive ? tabActiveStyle : tabStyle;

            if (GUI.Button(tabRect, name.ToLower(), styleToUse))
            {
                if (btn != null && btn.method != null)
                {
                    btn.method.Invoke();
                }
                else if (defaultTargetPage >= 0)
                {
                    Main.buttonsType = defaultTargetPage;
                }
            }

            if (isActive)
            {
                // Update the target position for the sliding underline animation
                targetTabX = currentX;
                targetTabWidth = width;
            }

            currentX += width;
        }

        private int GetCategoryIndex(ButtonInfo btn)
        {
            if (btn == null || btn.method == null) return -1;

            string bText = btn.buttonText.ToLower();
            if (bText == "settings") return 1;
            if (bText == "advantages") return 2;
            if (bText == "visuals") return 4;
            if (bText == "movement") return 3;
            if (bText == "overpowered") return 5;
            if (bText == "safety") return 6;
            if (bText == "fun") return 7;
            if (bText == "rig") return 8;
            if (bText == "gunlib") return 9;
            if (bText == "soundboard") return 15;
            if (bText == "spotify") return 14; 

            return -1;
        }
    }
}


