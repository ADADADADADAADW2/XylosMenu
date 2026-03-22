using BepInEx;
using System.ComponentModel;

namespace StupidTemplate.Patches
{
    [Description(StupidTemplate.PluginInfo.Description)]
    [BepInPlugin(StupidTemplate.PluginInfo.GUID, StupidTemplate.PluginInfo.Name, StupidTemplate.PluginInfo.Version)]
    public class HarmonyPatches : BaseUnityPlugin
    {
        private void OnEnable()
        {
            Menu.ApplyHarmonyPatches();
        }

        private void Update()
        {
            try 
            {
                StupidTemplate.Menu.mods.Boards();
                StupidTemplate.Menu.mods.ChangeMapInfoText();
                StupidTemplate.Menu.mods.SetCustomMenuProperties();
            } 
            catch { }
        }

        private void OnDisable()
        {
            Menu.RemoveHarmonyPatches();
        }
    }
}

