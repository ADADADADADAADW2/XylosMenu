using HarmonyLib;
using UnityEngine;
using System.Reflection;

namespace StupidTemplate.Patches
{
    [HarmonyPatch(typeof(VRRig), "PackCompetitiveData")]
    public class FPSPatch
    {
        public static bool enabled;
        public static int spoofFPSValue;

        public static void Postfix(ref short __result)
        {
            if (enabled)
            {
                var rig = GorillaTagger.Instance.offlineVRRig;
                int turnTypeInt = 0;

               
                Component snapTurn = rig.GetComponent("GorillaSnapTurn") ?? rig.GetComponent("GorillaSnapTurning");
                
                if (snapTurn != null)
                {
                    var turnTypeField = snapTurn.GetType().GetField("turnType", BindingFlags.Public | BindingFlags.Instance);
                    var turnFactorField = snapTurn.GetType().GetField("turnFactor", BindingFlags.Public | BindingFlags.Instance);

                    if (turnTypeField != null)
                    {
                        string turnType = turnTypeField.GetValue(snapTurn) as string;
                        if (turnType != "SNAP")
                        {
                            if (turnType == "SMOOTH")
                                turnTypeInt = 2;
                        }
                        else
                        {
                            turnTypeInt = 1;
                        }
                    }

                    if (turnFactorField != null)
                    {
                        turnTypeInt *= 10;
                        turnTypeInt += (int)turnFactorField.GetValue(snapTurn);
                    }
                }
                
                __result = (short)(spoofFPSValue + (turnTypeInt << 8));
            }
        }
    }
}