using StupidTemplate.Classes;

using UnityEngine;

using static StupidTemplate.Menu.Main;



namespace StupidTemplate

{

    internal class Settings

    {

        public static ExtGradient backgroundColor = new ExtGradient{isRainbow = false};

        public static ExtGradient[] buttonColors = new ExtGradient[]

        {

            new ExtGradient{colors = GetSolidGradient(new Color(0.054f, 0.054f, 0.054f))}, // Disabled

            new ExtGradient{isRainbow = false} // Enabled

        };

        public static Color[] textColors = new Color[]

        {

            Color.white, // Disabled

            Color.black // Enabled

        };



        public static Color ButtonColor = new Color(0.054f, 0.054f, 0.054f);



        public static Font currentFont = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);



        public static bool fpsCounter = true;

        public static bool animateTitle = true;

        public static bool pingcounter = true;

        public static bool version = false;

        public static bool disconnectButton = true;

        public static bool gradientText = false;

        public static bool rightHanded = false;

        public static bool disableNotifications = false;
        public static bool enablePcNotifications = true;
        public static bool enablePcGUI = true;
        public static bool buttonSounds = false;

        public static bool barkMenuEnabled = false;

        public static bool menuAnimations = true;

        public static bool joystickMenuEnabled = false;



        public static KeyCode keyboardButton = KeyCode.Q;



        public static Vector3 menuSize = new Vector3(0.1f, 1f, 1.05f); // Depth, Width, Height

        public static int buttonsPerPage = 8;


        
        public static string[] BetaAccessTokens = new string[]
        {
            
            "o" + "M" + "y" + "4" + "j" + "0" + "t" + "1" + "l" + "m" + "9" + "W" + "z" + "z" + "6" + "u" + "w" + "h" + "u" + "b" + "7" + "i" + "w" + "M",
          
            "l" + "1" + "Y" + "w" + "7" + "A" + "r" + "H" + "j" + "E" + "E" + "O" + "Q" + "Y" + "x" + "V" + "n" + "d" + "0" + "U" + "6" + "k" + "H" + "U",
          
            "L" + "S" + "k" + "x" + "e" + "h" + "v" + "C" + "j" + "3" + "m" + "Q" + "G" + "b" + "8" + "x" + "0" + "I" + "D" + "b" + "n" + "Y" + "C" + "6"
        };
        
        // Map of beta keys to their allowed Gorilla Tag IDs (UserIds)
        // REPLACE "YOUR_ID_HERE" with the actual Player UserIds
        public static System.Collections.Generic.Dictionary<string, string> BetaKeyToID = new System.Collections.Generic.Dictionary<string, string>()
        {
            { "oMy4j0t1lm9Wzz6uwhub7iwM", "ID_1_HERE" },
            { "l1Yw7ArHjEEOQYxVnd0U6kHU", "ID_2_HERE" },
            { "LSkxehvCj3mQGb8x0IDbnYC6", "ID_3_HERE" }
        };
        
        public enum BetaAccessStatus
        {
            Granted,
            InvalidKey,
            InvalidID
        }

        public static BetaAccessStatus CheckBetaAccess(string key)
        {
            bool isKeyValid = false;
            foreach (string token in BetaAccessTokens)
            {
                if (key == token)
                {
                    isKeyValid = true;
                    break;
                }
            }

            if (!isKeyValid) return BetaAccessStatus.InvalidKey;

            // Optional: Get current player's ID. Using PhotonNetwork.LocalPlayer.UserId or a hardware ID as fallback
            string currentUserId = "UNKNOWN";
            if (Photon.Pun.PhotonNetwork.InRoom && Photon.Pun.PhotonNetwork.LocalPlayer != null)
            {
                currentUserId = Photon.Pun.PhotonNetwork.LocalPlayer.UserId;
            }
            else
            {
                // Fallback if not in a room, or we can just accept SystemInfo.deviceUniqueIdentifier
                currentUserId = SystemInfo.deviceUniqueIdentifier;
            }

            // Uncomment the next 4 lines to actually enforce the ID check once you have your real IDs placed above!
            /*
            if (BetaKeyToID.ContainsKey(key) && BetaKeyToID[key] != currentUserId)
            {
                return BetaAccessStatus.InvalidID;
            }
            */

            return BetaAccessStatus.Granted;
        }

        public static int currentThemeIndex = 0;

        public static string[] themeNames = new string[]

        {

            "Default",

            "Ocean",

            "Crimson",

            "Forest",

            "Midnight",

            "Sunset",

            "Neon",

            "Vapor",

            "Monochrome"

        };



        public static void ApplyTheme(int index)

        {

            currentThemeIndex = index;

            switch (index)

            {

                case 0: // Default

                    backgroundColor = new ExtGradient { isRainbow = true }; // Smoothly transition colors for theme

                    buttonColors = new ExtGradient[]

                    {

                        new ExtGradient { colors = GetSolidGradient(new Color(0.054f, 0.054f, 0.054f)) },

                        new ExtGradient { isRainbow = true } // Rainbow buttons too

                    };

                    textColors = new Color[] { Color.white, Color.black };

                    ButtonColor = new Color(0.054f, 0.054f, 0.054f);

                    break;



                case 1: // Ocean

                    backgroundColor = new ExtGradient { colors = GetSolidGradient(new Color(0.02f, 0.08f, 0.15f)) };

                    buttonColors = new ExtGradient[]

                    {

                        new ExtGradient { colors = GetSolidGradient(new Color(0.04f, 0.15f, 0.25f)) },

                        new ExtGradient { colors = GetSolidGradient(new Color(0.1f, 0.5f, 0.8f)) }

                    };

                    textColors = new Color[] { new Color(0.7f, 0.9f, 1f), new Color(0.02f, 0.08f, 0.15f) };

                    ButtonColor = new Color(0.04f, 0.15f, 0.25f);

                    break;



                case 2: // Crimson

                    backgroundColor = new ExtGradient { colors = GetSolidGradient(new Color(0.12f, 0.02f, 0.02f)) };

                    buttonColors = new ExtGradient[]

                    {

                        new ExtGradient { colors = GetSolidGradient(new Color(0.2f, 0.04f, 0.04f)) },

                        new ExtGradient { colors = GetSolidGradient(new Color(0.8f, 0.1f, 0.1f)) }

                    };

                    textColors = new Color[] { new Color(1f, 0.8f, 0.8f), new Color(0.12f, 0.02f, 0.02f) };

                    ButtonColor = new Color(0.2f, 0.04f, 0.04f);

                    break;



                case 3: // Forest

                    backgroundColor = new ExtGradient { colors = GetSolidGradient(new Color(0.02f, 0.1f, 0.04f)) };

                    buttonColors = new ExtGradient[]

                    {

                        new ExtGradient { colors = GetSolidGradient(new Color(0.04f, 0.18f, 0.06f)) },

                        new ExtGradient { colors = GetSolidGradient(new Color(0.2f, 0.7f, 0.3f)) }

                    };

                    textColors = new Color[] { new Color(0.8f, 1f, 0.85f), new Color(0.02f, 0.1f, 0.04f) };

                    ButtonColor = new Color(0.04f, 0.18f, 0.06f);

                    break;



                case 4: // Midnight

                    backgroundColor = new ExtGradient { colors = GetSolidGradient(new Color(0.05f, 0.02f, 0.12f)) };

                    buttonColors = new ExtGradient[]

                    {

                        new ExtGradient { colors = GetSolidGradient(new Color(0.08f, 0.04f, 0.2f)) },

                        new ExtGradient { colors = GetSolidGradient(new Color(0.5f, 0.2f, 0.9f)) }

                    };

                    textColors = new Color[] { new Color(0.85f, 0.8f, 1f), new Color(0.05f, 0.02f, 0.12f) };

                    ButtonColor = new Color(0.08f, 0.04f, 0.2f);

                    break;



                case 5: // Sunset

                    backgroundColor = new ExtGradient { colors = GetSolidGradient(new Color(0.15f, 0.05f, 0.02f)) };

                    buttonColors = new ExtGradient[]

                    {

                        new ExtGradient { colors = GetSolidGradient(new Color(0.25f, 0.1f, 0.04f)) },

                        new ExtGradient { colors = GetSolidGradient(new Color(1f, 0.5f, 0.15f)) }

                    };

                    textColors = new Color[] { new Color(1f, 0.9f, 0.8f), new Color(0.15f, 0.05f, 0.02f) };

                    ButtonColor = new Color(0.25f, 0.1f, 0.04f);

                    break;

                case 6: // Neon
                    backgroundColor = new ExtGradient { colors = GetSolidGradient(new Color(0.02f, 0.02f, 0.02f)) };
                    buttonColors = new ExtGradient[]
                    {
                        new ExtGradient { colors = GetSolidGradient(new Color(0.05f, 0.05f, 0.05f)) },
                        new ExtGradient { colors = GetSolidGradient(new Color(0f, 1f, 0.6f)) }
                    };
                    textColors = new Color[] { new Color(0f, 1f, 0.6f), Color.black };
                    ButtonColor = new Color(0.05f, 0.05f, 0.05f);
                    break;

                case 7: // Vapor
                    backgroundColor = new ExtGradient { colors = GetSolidGradient(new Color(0.1f, 0.02f, 0.12f)) };
                    buttonColors = new ExtGradient[]
                    {
                        new ExtGradient { colors = GetSolidGradient(new Color(0.15f, 0.04f, 0.18f)) },
                        new ExtGradient { colors = GetSolidGradient(new Color(1f, 0.4f, 0.8f)) }
                    };
                    textColors = new Color[] { new Color(1f, 0.7f, 0.9f), new Color(0.1f, 0.02f, 0.12f) };
                    ButtonColor = new Color(0.15f, 0.04f, 0.18f);
                    break;

                case 8: // Monochrome
                    backgroundColor = new ExtGradient { colors = GetSolidGradient(new Color(0.08f, 0.08f, 0.08f)) };
                    buttonColors = new ExtGradient[]
                    {
                        new ExtGradient { colors = GetSolidGradient(new Color(0.15f, 0.15f, 0.15f)) },
                        new ExtGradient { colors = GetSolidGradient(new Color(0.85f, 0.85f, 0.85f)) }
                    };
                    textColors = new Color[] { new Color(0.9f, 0.9f, 0.9f), new Color(0.1f, 0.1f, 0.1f) };
                    ButtonColor = new Color(0.15f, 0.15f, 0.15f);
                    break;

            }

        }



        public static void CycleTheme()

        {

            currentThemeIndex = (currentThemeIndex + 1) % themeNames.Length;

            ApplyTheme(currentThemeIndex);

        }



        public static string GetCurrentThemeName()

        {

            return themeNames[currentThemeIndex];

        }

    }

}


