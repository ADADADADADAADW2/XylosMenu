using static StupidTemplate.Menu.Main;
using static StupidTemplate.Settings;

namespace orgg.Menu
{
    internal class SettingsMods
    {
        public static void EnterSettings()
        {
            buttonsType = 0;
            pageNumber = 0;
        }

        public static void MainSettings()
        {
            buttonsType = 1;
            pageNumber = 0;
        }

        public static void advantages()
        {
            buttonsType = 2;
            pageNumber = 0;
        }

        public static void movement()
        {
            buttonsType = 3;
            pageNumber = 0;
        }

        public static void visuals()
        {
            buttonsType = 4;
            pageNumber = 0;
        }

        public static void overpowered()
        {
            buttonsType = 5;
            pageNumber = 0;
        }

        public static void safety()
        {
            buttonsType = 6;
            pageNumber = 0;
        }

        public static void fun()
        {
            buttonsType = 7;
            pageNumber = 0;
        }

        public static void rig()
        {
            buttonsType = 8;
            pageNumber = 0;
        }

      
        public static void GunLib()
        {
            buttonsType = 9;
            pageNumber = 0;
        }

        public static void MenuSettings()
        {
            buttonsType = 10;
            pageNumber = 0;
        }

        public static void Notification()
        {
            buttonsType = 11;
            pageNumber = 0;
        }

        public static void ProjectileSettings()
        {
            buttonsType = 12;
            pageNumber = 0;
        }

        public static void Projectiles()
        {
            buttonsType = 13;
            pageNumber = 0;
        }

        public static void Particles()
        {
            buttonsType = 14;
            pageNumber = 0;
        }

        public static void Spotify()
        {
            buttonsType = 14;
            pageNumber = 0;
        }

        public static void SoundBoard()
        {
            buttonsType = 15;
            pageNumber = 0;
        }

        public static void ModSettings()
        {
            buttonsType = 16;
            pageNumber = 0;
        }



        public static void RightHand()
        {
            rightHanded = true;
        }

        public static void LeftHand()
        {
            rightHanded = false;
        }

        public static void EnableFPSCounter()
        {
            fpsCounter = true;
        }

        public static void DisableFPSCounter()
        {
            fpsCounter = false;
        }

        public static void EnablePingCounter()
        {
            pingcounter = true;
        }

        public static void DisablePingCounter()
        {
            pingcounter = false;
        }

        public static void EnableversionCounter()
        {
            version = true;
        }

        public static void DisableversionCounter()
        {
            version = false;
        }

        public static void EnableAnimText()
        {
            animateTitle = true;
        }

        public static void DisableAnimText()
        {
            animateTitle = false;
        }

        public static void EnableGradientText()
        {
            gradientText = true;
        }

        public static void DisableGradientText()
        {
            gradientText = false;
        }

        public static void EnableNotifications()
        {
            disableNotifications = false;
        }

        public static void DisableNotifications()
        {
            disableNotifications = true;
        }

        public static void EnablePcNotifications()
        {
            enablePcNotifications = true;
        }

        public static void DisablePcNotifications()
        {
            enablePcNotifications = false;
        }

        public static void EnablePcGUI()
        {
            enablePcGUI = true;
        }

        public static void DisablePcGUI()
        {
            enablePcGUI = false;
        }

        public static void EnableBetaGUI()
        {
            StupidTemplate.Menu.PCGUI.showBetaGUI = true;
        }

        public static void DisableBetaGUI()
        {
            StupidTemplate.Menu.PCGUI.showBetaGUI = false;
        }

        public static void EnableDisconnectButton()
        {
            disconnectButton = true;
        }

        public static void DisableDisconnectButton()
        {
            disconnectButton = false;
        }

        public static void CycleTheme()
        {
            StupidTemplate.Settings.CycleTheme();
        }

        public static void CycleTitleAnimation()
        {
            StupidTemplate.Menu.Main.currentTitleAnimationIndex++;
            if (StupidTemplate.Menu.Main.currentTitleAnimationIndex >= StupidTemplate.Menu.Main.titleAnimationNames.Length)
            {
                StupidTemplate.Menu.Main.currentTitleAnimationIndex = 0;
            }
        }

        public static void EnableBarkMenu()
        {
            barkMenuEnabled = true;
        }

        public static void DisableBarkMenu()
        {
            barkMenuEnabled = false;
        }

        public static void EnableMenuAnimations()
        {
            menuAnimations = true;
        }

        public static void DisableMenuAnimations()
        {
            menuAnimations = false;
        }

        public static void EnableJoystickMenu()
        {
            joystickMenuEnabled = true;
        }

        public static void DisableJoystickMenu()
        {
            joystickMenuEnabled = false;
        }

        public static void EnableButtonSounds()
        {
            buttonSounds = true;
        }

        public static void DisableButtonSounds()
        {
            buttonSounds = false;
        }
    }
}


