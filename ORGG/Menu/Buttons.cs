using StupidTemplate.Classes;
using StupidTemplate.Menu;
using StupidTemplate.Mods;
using System.IO;
using static StupidTemplate.Settings;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using GorillaNetworking;
using GorillaLocomotion;


namespace orgg.Menu
{
    internal class Buttons
    {
        public static ButtonInfo[][] buttons = new ButtonInfo[][]
        {
            // ============================
            // [0] HOME / MAIN CATEGORIES
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "Settings", method =() => SettingsMods.MainSettings(), isTogglable = false, toolTip = "Opens the main settings page for the menu."},
                new ButtonInfo { buttonText = "advantages", method =() => SettingsMods.advantages(), isTogglable = false, toolTip = "Opens the advantages page for the menu."},
                new ButtonInfo { buttonText = "visuals", method =() => SettingsMods.visuals(), isTogglable = false, toolTip = "Opens the visuals page for the menu."},
                new ButtonInfo { buttonText = "movement", method =() => SettingsMods.movement(), isTogglable = false, toolTip = "Opens the movement page for the menu."},
                new ButtonInfo { buttonText = "overpowered", method =() => SettingsMods.overpowered(), isTogglable = false, toolTip = "Opens the overpowered page for the menu."},
                new ButtonInfo { buttonText = "safety", method =() => SettingsMods.safety(), isTogglable = false, toolTip = "Opens the safety page for the menu."},
                new ButtonInfo { buttonText = "fun", method =() => SettingsMods.fun(), isTogglable = false, toolTip = "Opens the fun page for the menu."},
                new ButtonInfo { buttonText = "rig", method =() => SettingsMods.rig(), isTogglable = false, toolTip = "Opens the rig page for the menu."},
                new ButtonInfo { buttonText = "soundboard", method =() => SettingsMods.SoundBoard(), isTogglable = false, toolTip = "Opens the SoundBoard."},
                new ButtonInfo { buttonText = "Spotify", method =() => SettingsMods.Spotify(), isTogglable = false, toolTip = "Opens the Spotify controls."},
            },

            // ============================
            // [1] SETTINGS
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = "Menu Settings", method =() => SettingsMods.MenuSettings(), isTogglable = false, toolTip = "Opens the Menu settings for the menu."},
                new ButtonInfo { buttonText = "GunLib Settings", method =() => SettingsMods.GunLib(), isTogglable = false, toolTip = "Opens the GunLib settings for the menu."},
                new ButtonInfo { buttonText = "Notifications Settings", method =() => SettingsMods.Notification(), isTogglable = false, toolTip = "Opens the GunLib settings for the menu."},
                new ButtonInfo { buttonText = "Projectile Settings", method =() => SettingsMods.ProjectileSettings(), isTogglable = false, toolTip = "Opens the Projectile settings for the menu."},
                new ButtonInfo { buttonText = "Mod Settings", method =() => SettingsMods.ModSettings(), isTogglable = false, toolTip = "Opens the Mod settings for the menu."},
            },

            // ============================
            // [2] ADVANTAGES
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = "Tag All", method =() => mods.TagAll(), isTogglable = true, toolTip = "Teleports to everyone to tag them (Must be 'it')."},
                new ButtonInfo { buttonText = "Tag Gun", method =() => mods.TagGun(), disableMethod =() => mods.DisableTagGun(), isTogglable = true, toolTip = "Shoot players to tag them."},
                new ButtonInfo { buttonText = "Tag Aura", method =() => mods.TagAura(), disableMethod =() => mods.DisableTagAura(), isTogglable = true, toolTip = "Automatically tags nearby players."},
            },

            // ============================
            // [3] MOVEMENT
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = "Low Gravity", method =() => mods.LowGravity(), disableMethod =() => mods.DisableLowGravity(), isTogglable = true, toolTip = "Reduces gravity."},
                new ButtonInfo { buttonText = "Zero Gravity", method =() => mods.ZeroGravity(), disableMethod =() => mods.DisableZeroGravity(), isTogglable = true, toolTip = "Hold right trigger for zero gravity."},
                new ButtonInfo { buttonText = "Speed Boost", method =() => mods.SpeedBoost(), disableMethod =() => mods.DisableSpeedBoost(), isTogglable = true, toolTip = "Increases max jump speed."},
                new ButtonInfo { buttonText = "High Jump", method =() => mods.HighJump(), disableMethod =() => mods.DisableHighJump(), isTogglable = true, toolTip = "Jump higher."},

                new ButtonInfo { buttonText = "Fly", method =() => mods.Fly(), disableMethod =() => mods.DisableFly(), isTogglable = true, toolTip = "Hold right grip to fly."},
                new ButtonInfo { buttonText = "Platforms", method =() => mods.Platforms(), disableMethod =() => mods.DisablePlatforms(), isTogglable = true, toolTip = "Grip to spawn platforms."},
                new ButtonInfo { buttonText = "Iron Monke", method =() => mods.IronMonke(), isTogglable = true, toolTip = "Grip to boost like Iron Man."},
                new ButtonInfo { buttonText = "Freeze", method =() => mods.Freeze(), disableMethod =() => mods.DisableFreeze(), isTogglable = true, toolTip = "Hold left trigger to freeze."},
                new ButtonInfo { buttonText = "NoClip", method =() => mods.Noclip(), disableMethod =() => mods.DisableNoclip(), isTogglable = true, toolTip = "Hold right trigger to phase through walls."},
                new ButtonInfo { buttonText = "Spiderman", method =() => StupidTemplate.Menu.CustomSounds.Spiderman(), disableMethod =() => mods.DisableSpiderman(), isTogglable = true, toolTip = "Hold triggers to swing with web lines."},
                new ButtonInfo { buttonText = "No Gravity", method =() => mods.NoGravMenu(), disableMethod =() => mods.DisableNoGravMenu(), isTogglable = true, toolTip = "Zero gravity - float freely."},
                new ButtonInfo { buttonText = "Pull Mod", method =() => mods.PullMod(), isTogglable = true, toolTip = "Pulls you forward based on your jump direction."},
                new ButtonInfo { buttonText = "Enderpearl", method =() => mods.Enderpearl(), isTogglable = true, toolTip = "Hold right trigger to throw a pearl, release to teleport."},
                new ButtonInfo { buttonText = "Frozone", method =() => mods.Frozone(), isTogglable = true, toolTip = "Leaves a trail of ice under your feet to glide."},
                new ButtonInfo { buttonText = "Platform Spam", method =() => StupidTemplate.Menu.CustomSounds.PlatformSpam(), isTogglable = true, toolTip = "Spams platforms everywhere you move."},
                new ButtonInfo { buttonText = "Joystick Fly", method =() => mods.JoystickFly(), disableMethod =() => mods.DisableJoystickFly(), isTogglable = true, toolTip = "Fly using left joystick (also gives no gravity)."},
                new ButtonInfo { buttonText = "Invisible Platforms", method =() => mods.InvisPlatforms(), disableMethod =() => mods.DisableInvisPlatforms(), isTogglable = true, toolTip = "Platforms that are invisible to everyone."},
                new ButtonInfo { buttonText = "Free Cam", method =() => mods.FreeCam(), disableMethod =() => mods.DisableFreeCam(), isTogglable = true, toolTip = "Joystick Fly + Noclip for free movement."},
                new ButtonInfo { buttonText = "WASD Movement", method =() => mods.PCMovement(), isTogglable = true, toolTip = "Move the player using WASD keyboard keys."},
            },

            // ============================
            // [4] VISUALS
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = "Hand Trails", method =() => mods.HandTrails(), disableMethod =() => mods.DisableHandTrails(), isTogglable = true, toolTip = "Trails follow your hands."},
                new ButtonInfo { buttonText = "ESP", method =() => mods.ESP(), disableMethod =() => mods.DisableESP(), isTogglable = true, toolTip = "Lines to all players."},
                new ButtonInfo { buttonText = "Chams", method =() => mods.Chams(), disableMethod =() => mods.DisableChams(), isTogglable = true, toolTip = "See players through walls."},
                new ButtonInfo { buttonText = "Box ESP", method =() => mods.BoxESP(), disableMethod =() => mods.DisableBoxESP(), isTogglable = true, toolTip = "Boxes around players."},
                new ButtonInfo { buttonText = "Tracers", method =() => mods.Tracers(), disableMethod =() => mods.DisableTracers(), isTogglable = true, toolTip = "Lines from hand to players."},
                new ButtonInfo { buttonText = "Bread Crumbs", method =() => mods.BreadCrumbs(), isTogglable = true, toolTip = "Leave a trail of dots."},
                new ButtonInfo { buttonText = "Name Tags", method =() => StupidTemplate.Menu.CustomSounds.NameTags(), isTogglable = true, toolTip = "Shows player names above heads."},
                new ButtonInfo { buttonText = "Fancy Nametags", method =() => mods.CastingTags(), disableMethod =() => mods.DisableCastingTags(), isTogglable = true, toolTip = "A fancier way of seeing names."},
                new ButtonInfo { buttonText = "Spawn Robux", method =() => mods.SpawnRobux(), disableMethod =() => mods.DisableSpawnRobux(), isTogglable = true, toolTip = "Fake shiny rocks generator!"},
                new ButtonInfo { buttonText = "Third Person", method =() => mods.ThirdPersonVisual(), disableMethod =() => mods.DisableThirdPersonVisual(), isTogglable = true, toolTip = "Changes camera to third person."},
                new ButtonInfo { buttonText = "First Person", method =() => mods.FirstPersonVisual(), disableMethod =() => mods.DisableFirstPersonVisual(), isTogglable = true, toolTip = "Pin camera to your head."},
                new ButtonInfo { buttonText = "Orbital Spheres", method =() => mods.OrbitalSpheres(), disableMethod =() => mods.DisableOrbitalSpheres(), isTogglable = true, toolTip = "8 rainbow spheres orbit your body in shifting loops."},
                new ButtonInfo { buttonText = "Lightning Hands", method =() => mods.LightningHands(), disableMethod =() => mods.DisableLightningHands(), isTogglable = true, toolTip = "Crackling electric arcs bridge your two hands."},
                new ButtonInfo { buttonText = "Meteor Shower", method =() => mods.MeteorShower(), disableMethod =() => mods.DisableMeteorShower(), isTogglable = true, toolTip = "Colorful meteors with trails rain down around you."},
            },

            // ============================
            // [5] OVERPOWERED
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = "UnTag All", method =() => mods.UnTagAll(), disableMethod =() => mods.DisableUnTagAll(), isTogglable = false, requiresMaster = true, toolTip = "Removes tag from everyone (Master Client)."},
                new ButtonInfo { buttonText = "UnTag Gun", method =() => mods.UnTagGun(), disableMethod =() => mods.DisableUnTagGun(), isTogglable = true, requiresMaster = true, toolTip = "Shoot players to untag them (Master Client)."},
                new ButtonInfo { buttonText = "Gray All", method =() => mods.ActivateGrayAll(), disableMethod =() => mods.DeactivateGrayAll(), isTogglable = true, requiresMaster = true, toolTip = "Activates the grey zone for everyone (Master Client)."},
                new ButtonInfo { buttonText = "Slow All", method =() => mods.SlowAll(), isTogglable = true, requiresMaster = true, toolTip = "Freezes everyone (Master Client)."},
                new ButtonInfo { buttonText = "Freeze All", method =() => StupidTemplate.Menu.mods.FreezeAll(), isTogglable = true, requiresMaster = false, toolTip = "Freezes everyone without Master Client via Events."},
                new ButtonInfo { buttonText = "Crash All", method =() => StupidTemplate.Menu.mods.CrashAllMaster(), isTogglable = true, requiresMaster = true, toolTip = "Attempts to crash all other players (Master Client)."},
            },

            // ============================
            // [6] SAFETY
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = "Anti-Report", method =() => mods.AntiReport(), isTogglable = true, toolTip = "Disconnects if someone tries to report you."},
                new ButtonInfo { buttonText = "Visualize Anti-Report", method =() => mods.VisualizeAntiReport(), isTogglable = true, toolTip = "Shows the MOTD and visualizes report button hitboxes."},
                new ButtonInfo { buttonText = "Anti-Tag", method =() => mods.AntiTag(), isTogglable = true, toolTip = "Teleports you away when someone gets too close."},
                new ButtonInfo { buttonText = "Spoof FPS", method =() => mods.SpoofFPS(), disableMethod =() => mods.DisableSpoofFPS(), isTogglable = true, toolTip = "Spoof your FPS value over the network."},
                new ButtonInfo { buttonText = "Anti-Crash", method =() => mods.AntiCrash(), isTogglable = true, toolTip = "Attempts to prevent typical crashing methods."},
                new ButtonInfo { buttonText = "Mute Self", method =() => mods.MuteSelf(), disableMethod =() => mods.DisableMuteSelf(), isTogglable = true, toolTip = "Mutes your microphone globally."},
                new ButtonInfo { buttonText = "Mute Gun", method =() => mods.MuteGun(), isTogglable = true, toolTip = "Shoot a player to mute them locally."},
                new ButtonInfo { buttonText = "Unmute Self", method =() => mods.UnmuteSelf(), isTogglable = false, toolTip = "Unmutes your microphone."},
                new ButtonInfo { buttonText = "Unmute Gun", method =() => mods.UnmuteGun(), isTogglable = true, toolTip = "Shoot a player to unmute them."},
                new ButtonInfo { buttonText = "Unmute All", method =() => mods.UnmuteAll(), isTogglable = false, toolTip = "Unmutes everyone in the room."},
                new ButtonInfo { buttonText = "Report Gun", method =() => mods.ReportGun(), isTogglable = true, toolTip = "Shoot a player to open the report menu for them."},
                new ButtonInfo { buttonText = "No Finger Movement", method =() => mods.NoFinger(), disableMethod =() => mods.DisableNoFinger(), isTogglable = true, toolTip = "Stop your fingers from moving."},
                new ButtonInfo { buttonText = "Join Random", method =() => mods.JoinRandom(), isTogglable = false, toolTip = "Disconnect and join a random map zone."},
                new ButtonInfo { buttonText = "Joystick Disconnect", method =() => mods.JoystickDisconnect(), isTogglable = true, toolTip = "Disconnects if you click your joystick."},
                new ButtonInfo { buttonText = "Loud Microphone", method =() => mods.LoudMicrophone(), disableMethod =() => mods.DisableLoudMicrophone(), isTogglable = true, toolTip = "Sends your microphone data at maximum bitrate for extreme volume."},
                new ButtonInfo { buttonText = "High Pitch Mic", method =() => mods.HighPitchMicrophone(), disableMethod =() => mods.DisableHighPitchMicrophone(), isTogglable = true, toolTip = "Changes your microphone sampling rate for a higher pitch effect."},
                new ButtonInfo { buttonText = "Hear Self", method =() => mods.HearSelf(), disableMethod =() => mods.DisableHearSelf(), isTogglable = true, toolTip = "Hear your own voice echoed back."},
                new ButtonInfo { buttonText = "Disable Network Triggers", method =() => mods.DisableNetworkTriggers(), disableMethod =() => mods.EnableNetworkTriggers(), isTogglable = true, toolTip = "Prevents network triggers (like map changes) from being activated."},
                new ButtonInfo { buttonText = "Copy ID Self", method =() => mods.CopyIdSelf(), isTogglable = false, toolTip = "Copies your Photon ID to clipboard."},
                new ButtonInfo { buttonText = "Copy ID Gun", method =() => mods.CopyIdGun(), isTogglable = true, toolTip = "Shoot a player to copy their ID to clipboard."},
                new ButtonInfo { buttonText = "Copy ID All", method =() => mods.CopyIdAll(), isTogglable = false, toolTip = "Copies all player IDs in the room to clipboard."},
                new ButtonInfo { buttonText = "Anti-Moderator", method =() => mods.AntiModerator(), isTogglable = true, toolTip = "Auto-disconnects if a moderator is found in the room."},
                new ButtonInfo { buttonText = "Open Gtag Folder", method =() => mods.OpenGtagFolder(), isTogglable = false, toolTip = "Opens the game's installation folder on your PC."},
                new ButtonInfo { buttonText = "PC Button Click", method =() => mods.PCButtonClick(), isTogglable = true, toolTip = "Allows clicking buttons with mouse on PC."},
                new ButtonInfo { buttonText = "Dump Cosmetics", method =() => mods.DumpCosmetics(), isTogglable = false, toolTip = "Dump all cosmetics to file." },
            },

            // ============================
            // [7] FUN
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = "Projectiles", method =() => SettingsMods.Projectiles(), isTogglable = false, toolTip = "Opens projectiles page."},
                new ButtonInfo { buttonText = "Solid Water", method =() => mods.SolidWater(), disableMethod =() => mods.DisableSolidWater(), isTogglable = true, toolTip = "Makes water surfaces solid like regular ground."},
                new ButtonInfo { buttonText = "Water Spam", method =() => mods.WaterSpam(), isTogglable = true, toolTip = "Spams water splash effects on your head globally."},
                new ButtonInfo { buttonText = "Core Spam Master", method =() => mods.CoreSpamMaster(), isTogglable = true, requiresMaster = true, toolTip = "Spam Ghost Reactor cores (Master Client)."},
                new ButtonInfo { buttonText = "Elevator Door Spam", method =() => mods.ElevatorDoorSpam(), isTogglable = true, toolTip = "Rapidly spam elevator doors open/close."},
                new ButtonInfo { buttonText = "Spam Slam Effect", method =() => mods.SpamSlamEffects(), isTogglable = true, toolTip = "Spams the ground slam effect globally."},
                new ButtonInfo { buttonText = "Spam Drum Effect", method =() => mods.SpamDrumEffects(), isTogglable = true, toolTip = "Spams the drum instrument effect globally."},
                new ButtonInfo { buttonText = "Spam Whack-A-Mole", method =() => mods.SpamWhackAMole(), isTogglable = true, toolTip = "Spams the Whack-A-Mole button RPC globally."},
                new ButtonInfo { buttonText = "Giant Monke", method =() => mods.GiantMonke(), disableMethod =() => mods.DisableGiantMonke(), isTogglable = true, toolTip = "Hold right grip to grow."},
                new ButtonInfo { buttonText = "Tiny Monke", method =() => mods.TinyMonke(), disableMethod =() => mods.DisableTinyMonke(), isTogglable = true, toolTip = "Hold left grip to shrink."},
                new ButtonInfo { buttonText = "Spin Monke", method =() => mods.SpinMonke(), isTogglable = true, toolTip = "Spins your rig."},
                new ButtonInfo { buttonText = "Flick Right Hand", method =() => mods.FlickRightHand(), disableMethod =() => mods.DisableFlickRightHand(), isTogglable = true, toolTip = "Enlarges right hand."},
                new ButtonInfo { buttonText = "Flick Left Hand", method =() => mods.FlickLeftHand(), disableMethod =() => mods.DisableFlickLeftHand(), isTogglable = true, toolTip = "Enlarges left hand."},
                new ButtonInfo { buttonText = "Copy Monke", method =() => mods.CopyMonke(), isTogglable = true, toolTip = "Copies nearest player position."},
                new ButtonInfo { buttonText = "Teleport To Player", method =() => mods.TeleportToPlayer(), isTogglable = true, toolTip = "Pull right trigger to teleport."},
                new ButtonInfo { buttonText = "Tp Gun", method =() => mods.TpGun(), disableMethod =() => mods.DisableTpGun(), isTogglable = true, toolTip = "Shoot to teleport there."},
                new ButtonInfo { buttonText = "Air Swim", method =() => mods.AirSwim(), disableMethod =() => mods.DisableAirSwim(), isTogglable = true, toolTip = "Swing your arms to swim in the air."},
                new ButtonInfo { buttonText = "Swim Fast", method =() => mods.SwimFast(), disableMethod =() => mods.DisableSwimFast(), isTogglable = true, toolTip = "Increases your water movement speed."},
                new ButtonInfo { buttonText = "Jumpscare Gun", method =() => mods.JumpscareGun(), disableMethod =() => mods.DisableJumpscareGun(), isTogglable = true, toolTip = "Shoot a player to teleport to their face and tweak out."},
                new ButtonInfo { buttonText = "Barrel Fling Gun", method =() => mods.BarrelFlingGun(), disableMethod =() => mods.DisableBarrelFlingGun(), isTogglable = true, toolTip = "Shoot a player to barrel fling them."},
                new ButtonInfo { buttonText = "Barrel Fling Aura", method =() => mods.BarrelFlingAura(), isTogglable = true, toolTip = "Barrel flings all nearby players."},
                new ButtonInfo { buttonText = "Freeze Gun", method =() => mods.FreezeGun(), disableMethod =() => mods.DisableFreezeGun(), isTogglable = true, toolTip = "Shoot a player to freeze them."},
            },

            // ============================
            // [8] RIG
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = "Long Arms", method =() => mods.LongArms(), disableMethod =() => mods.DisableLongArms(), isTogglable = true, toolTip = "Extends your arms lengths."},
                new ButtonInfo { buttonText = "Ghost Monke", method =() => mods.Ghostmonke(), disableMethod =() => mods.DisableGhostmonke(), isTogglable = true, toolTip = "Disables your VRRig."},
                new ButtonInfo { buttonText = "Invis Monke", method =() => mods.InvisMonke(), disableMethod =() => mods.DisableInvisMonke(), isTogglable = true, toolTip = "Hides your skin."},
                new ButtonInfo { buttonText = "Rainbow Body [SS]", method =() => mods.RainbowBodySS(), disableMethod =() => mods.DisableRainbowBodySS(), isTogglable = true, toolTip = "Your body cycles rainbow colors."},
                new ButtonInfo { buttonText = "Strobe Body [SS]", method =() => mods.StrobeBodySS(), disableMethod =() => mods.DisableStrobeBodySS(), isTogglable = true, toolTip = "Your body flashes rapidly."},
                new ButtonInfo { buttonText = "Solid Player", method =() => mods.SolidPlayer(), disableMethod =() => mods.DisableSolidPlayer(), isTogglable = true, toolTip = "Makes other players' rigs solid (collidable)."},
                new ButtonInfo { buttonText = "Spaz", method =() => mods.Spaz(), disableMethod =() => mods.DisableSpaz(), isTogglable = true, toolTip = "Randomly rotates your rig."},
                new ButtonInfo { buttonText = "Barrel Roll", method =() => mods.BarrelRoll(), disableMethod =() => mods.DisableBarrelRoll(), isTogglable = true, toolTip = "Rolls your rig sideways."},
                new ButtonInfo { buttonText = "Helicopter", method =() => mods.Helicopter(), disableMethod =() => mods.DisableHelicopter(), isTogglable = true, toolTip = "Spins your rig like a helicopter."},
            },

            // ============================
            // [9] GUNLIB SETTINGS
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = "Equip Gun", method =() => mods.GunTemplate(), isTogglable = true, toolTip = "Equips a gun."},
                new ButtonInfo { buttonText = $"Smoothness: {(mods.num == 5f ? "Very Fast" : mods.num == 10f ? "Normal" : "Super Smooth")}", method = () => { mods.GunSmoothNess(); foreach (var category in Buttons.buttons) foreach (var button in category) if (button.buttonText.StartsWith("Smoothness")) button.buttonText = $"Smoothness: {(mods.num == 5f ? "Super Smooth" : mods.num == 10f ? "Normal" : "No Smooth")}"; }, isTogglable = false, toolTip = "Changes gun smoothness." },
                new ButtonInfo { buttonText = $"Gun Color: {mods.currentGunColor.name}", method = () => { mods.CycleGunColor(); Buttons.buttons.ForEach(category => category.ForEach(button => { if (button.buttonText.StartsWith("Gun Color")) button.buttonText = $"Gun Color: {mods.currentGunColor.name}"; })); }, isTogglable = false, toolTip = "Cycles through gun colors." },
                new ButtonInfo { buttonText = $"Toggle Sphere Size: {(mods.isSphereEnabled ? "Enabled" : "Disabled")}", method = () => { mods.isSphereEnabled = !mods.isSphereEnabled; if (mods.GunSphere != null) mods.GunSphere.transform.localScale = mods.isSphereEnabled ? new Vector3(0.1f, 0.1f, 0.1f) : new Vector3(0f, 0f, 0f); foreach (var category in Buttons.buttons) foreach (var button in category) if (button.buttonText.StartsWith("Toggle Sphere Size")) button.buttonText = $"Toggle Sphere Size: {(mods.isSphereEnabled ? "Enabled" : "Disabled")}"; }, isTogglable = false, toolTip = "Toggles the size of the gun sphere." },
                new ButtonInfo { buttonText = "Wavy Gun", method =() => mods.WavyGun(), isTogglable = true, toolTip = "Applies wavy/zigzag effect to gun line."},
                new ButtonInfo { buttonText = $"Gun Style: {mods.gunStyleNames[mods.gunStyleIndex]}", method = () => { mods.CycleGunStyle(); foreach (var category in Buttons.buttons) foreach (var button in category) if (button.buttonText.StartsWith("Gun Style:")) button.buttonText = $"Gun Style: {mods.gunStyleNames[mods.gunStyleIndex]}"; }, isTogglable = false, toolTip = "Cycles gun line style (Normal/Wavy/Zigzag)." },
            },

            // ============================
            // [10] MENU SETTINGS
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = "Right/Left Hand", enableMethod =() => SettingsMods.RightHand(), disableMethod =() => SettingsMods.LeftHand(), isTogglable = true, toolTip = "Puts the menu on your right hand."},
                new ButtonInfo { buttonText = "FPS Counter", enableMethod =() => SettingsMods.EnableFPSCounter(), disableMethod =() => SettingsMods.DisableFPSCounter(), isTogglable = true, enabled = fpsCounter, toolTip = "Toggles the FPS counter."},
                new ButtonInfo { buttonText = "Ping Counter", enableMethod =() => SettingsMods.EnablePingCounter(), disableMethod =() => SettingsMods.DisablePingCounter(), isTogglable = true, enabled = fpsCounter, toolTip = "Toggles the Ping counter."},
                new ButtonInfo { buttonText = "Animated Text", enableMethod =() => SettingsMods.EnableAnimText(), disableMethod =() => SettingsMods.DisableAnimText(), isTogglable = true, enabled = fpsCounter, toolTip = "Toggles the Animated Text."},
                new ButtonInfo { buttonText = "Gradient Text", enableMethod =() => SettingsMods.EnableGradientText(), disableMethod =() => SettingsMods.DisableGradientText(), isTogglable = true, enabled = gradientText, toolTip = "Makes text use a gradient color effect."},
                new ButtonInfo { buttonText = "Display Version", enableMethod =() => SettingsMods.EnableversionCounter(), disableMethod =() => SettingsMods.DisableversionCounter(), isTogglable = true, enabled = fpsCounter, toolTip = "Displays Version."},
                new ButtonInfo { buttonText = $"Title Animation: {Main.titleAnimationNames[Main.currentTitleAnimationIndex]}", method = () => { Main.currentTitleAnimationIndex = (Main.currentTitleAnimationIndex + 1) % Main.titleAnimationNames.Length; foreach (var category in Buttons.buttons) foreach (var button in category) if (button.buttonText.StartsWith("Title Animation")) button.buttonText = $"Title Animation: {Main.titleAnimationNames[Main.currentTitleAnimationIndex]}"; }, isTogglable = false, toolTip = "Cycles through title animation styles." },
                new ButtonInfo { buttonText = "Disconnect Button", enableMethod =() => SettingsMods.EnableDisconnectButton(), disableMethod =() => SettingsMods.DisableDisconnectButton(), isTogglable = true, enabled = disconnectButton, toolTip = "Toggles the disconnect button."},
                new ButtonInfo { buttonText = $"Delete Time: {(Main.num == 2f ? "Default" : Main.num == 5f ? "Long" : "Fast")}", method = () => { Main.MenuDeleteTime(); foreach (var category in Buttons.buttons) foreach (var button in category) if (button.buttonText.StartsWith("Delete Time")) button.buttonText = $"Delete Time: {(Main.num == 2f ? "Default" : Main.num == 5f ? "Long" : "Fast")}"; }, isTogglable = false, toolTip = "Changes menu delete time." },
                new ButtonInfo { buttonText = $"Theme: {StupidTemplate.Settings.GetCurrentThemeName()}", method = () => { SettingsMods.CycleTheme(); foreach (var category in Buttons.buttons) foreach (var button in category) if (button.buttonText.StartsWith("Theme:")) button.buttonText = $"Theme: {StupidTemplate.Settings.GetCurrentThemeName()}"; }, isTogglable = false, toolTip = "Cycles through menu themes." },
                new ButtonInfo { buttonText = "Bark Menu", enableMethod =() => SettingsMods.EnableBarkMenu(), disableMethod =() => SettingsMods.DisableBarkMenu(), isTogglable = true, enabled = barkMenuEnabled, toolTip = "Toggles the bark menu (tap chest to open)."},
                new ButtonInfo { buttonText = "Menu Animations", enableMethod =() => SettingsMods.EnableMenuAnimations(), disableMethod =() => SettingsMods.DisableMenuAnimations(), isTogglable = true, enabled = menuAnimations, toolTip = "Toggles open/close animations."},
                new ButtonInfo { buttonText = "Joystick Menu", enableMethod =() => SettingsMods.EnableJoystickMenu(), disableMethod =() => SettingsMods.DisableJoystickMenu(), isTogglable = true, enabled = joystickMenuEnabled, toolTip = "Toggles joystick page scrolling."},
                new ButtonInfo { buttonText = "Wide Menu", method =() => mods.WideMenu(), disableMethod =() => mods.DisableWideMenu(), isTogglable = true, toolTip = "Makes the menu wider."},
                new ButtonInfo { buttonText = "Tiny Menu", method =() => mods.TinyMenu(), disableMethod =() => mods.DisableTinyMenu(), isTogglable = true, toolTip = "Makes the menu tiny."},
                new ButtonInfo { buttonText = "Watch Menu", method =() => mods.WatchMenu(), disableMethod =() => mods.DisableWatchMenu(), isTogglable = true, toolTip = "Shrinks menu to a watch size."},
            },

            // ============================
            // [11] NOTIFICATIONS
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = "Notifications", enableMethod =() => SettingsMods.EnableNotifications(), disableMethod =() => SettingsMods.DisableNotifications(), enabled = !disableNotifications, isTogglable = true, toolTip = "Toggles the notifications."},
                new ButtonInfo { buttonText = "PC Notifications", enableMethod =() => SettingsMods.EnablePcNotifications(), disableMethod =() => SettingsMods.DisablePcNotifications(), enabled = enablePcNotifications, isTogglable = true, toolTip = "Toggles desktop PC GUI notifications."},
                new ButtonInfo { buttonText = "Modern PC GUI", enableMethod =() => SettingsMods.EnablePcGUI(), disableMethod =() => SettingsMods.DisablePcGUI(), enabled = enablePcGUI, isTogglable = true, toolTip = "Displays an interactive mod menu UI on the desktop screen." },
                new ButtonInfo { buttonText = "PC Beta Window", enableMethod =() => SettingsMods.EnableBetaGUI(), disableMethod =() => SettingsMods.DisableBetaGUI(), enabled = StupidTemplate.Menu.PCGUI.showBetaGUI, isTogglable = true, toolTip = "Shows or hides the Beta GUI on PC (Requires Unlock)." },
            },

            // ============================
            // [12] PROJECTILE SETTINGS
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
               



            },

            // ============================
            // [13] PROJECTILES
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = "Water Balloon Aura", method =() => mods.WaterBalloonAura(), isTogglable = true, toolTip = "Spawns water balloons around you."},
                new ButtonInfo { buttonText = "Snowball Aura", method =() => mods.SnowballAura(), isTogglable = true, toolTip = "Spawns snowballs around you."},

            },

            // ============================
            // [14] SPOTIFY
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = "Play / Pause", method =() => mods.SpotifyPlayPause(), isTogglable = false, toolTip = "Toggle play/pause on Spotify."},
                new ButtonInfo { buttonText = "Next Track", method =() => mods.SpotifyNext(), isTogglable = false, toolTip = "Skip to next track."},
                new ButtonInfo { buttonText = "Previous Track", method =() => mods.SpotifyPrevious(), isTogglable = false, toolTip = "Go to previous track."},
                new ButtonInfo { buttonText = "Volume Up", method =() => mods.SpotifyVolumeUp(), isTogglable = false, toolTip = "Increase Spotify volume."},
                new ButtonInfo { buttonText = "Volume Down", method =() => mods.SpotifyVolumeDown(), isTogglable = false, toolTip = "Decrease Spotify volume."},
            },

            // ============================
            // [15] SOUNDBOARD
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = "Loading Sounds...", method =() => { }, isTogglable = false, toolTip = "Wait for SoundBoard to load."},
            },

            // ============================
            // [16] MOD SETTINGS
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
                new ButtonInfo { buttonText = $"Speed Boost: {(mods.speedMult == 9.5f ? "Default" : mods.speedMult == 12.5f ? "Fast" : "Super")}", method = () => { mods.CycleSpeedMult(); foreach (var category in Buttons.buttons) foreach (var button in category) if (button.buttonText.StartsWith("Speed Boost:")) button.buttonText = $"Speed Boost: {(mods.speedMult == 9.5f ? "Default" : mods.speedMult == 12.5f ? "Fast" : "Super")}"; }, isTogglable = false, toolTip = "Cycles speed boost power." },
                new ButtonInfo { buttonText = $"Fly Speed: {mods.flyMult}", method = () => { mods.CycleFlyMult(); foreach (var category in Buttons.buttons) foreach (var button in category) if (button.buttonText.StartsWith("Fly Speed:")) button.buttonText = $"Fly Speed: {mods.flyMult}"; }, isTogglable = false, toolTip = "Cycles flying speed multiplier." },
                new ButtonInfo { buttonText = $"Anti-Report Dist: {mods.antiReportDistance}", method = () => { mods.CycleAntiReportDistance(); foreach (var category in Buttons.buttons) foreach (var button in category) if (button.buttonText.StartsWith("Anti-Report Dist:")) button.buttonText = $"Anti-Report Dist: {mods.antiReportDistance}"; }, isTogglable = false, toolTip = "Cycles anti-report detection distance." },
                new ButtonInfo { buttonText = $"Pull Power: {mods.pullPower:F2}", method = () => { mods.CyclePullPower(); foreach (var category in Buttons.buttons) foreach (var button in category) if (button.buttonText.StartsWith("Pull Power:")) button.buttonText = $"Pull Power: {mods.pullPower:F2}"; }, isTogglable = false, toolTip = "Cycles pull mod strength." }
            },

            // ============================
            // [17] DISCONNECT
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "Disconnect", method =() => mods.Disconnect(), isTogglable = false, toolTip = "Disconnected from lobby"},
            },

            // ============================
            // [18] PLACEHOLDER
            // ============================
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "home", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the home for the menu."},
            },

        };
    }
}


