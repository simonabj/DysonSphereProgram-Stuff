using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using BepInEx;
using UnityEngine;

namespace FreeReplicatorMode
{
    [BepInPlugin("no.uio.simonabj.plugins.freereplicatormode", "Free Replicator Mode", "1.0")]
    public class FreeReplicatorMode : BaseUnityPlugin
    {
        private static float originalReplicateSpeed;
        private static double originalReplicatePower;

        void Awake()
        {
            Debug.Log("Patching for free replicator creation");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }
        [HarmonyPatch(typeof(Mecha), "Import")]
        class Mecha_Import
        {
            static void Postfix(ref Mecha __instance)
            {
                FreeReplicatorMode.originalReplicateSpeed = __instance.replicateSpeed;
                FreeReplicatorMode.originalReplicatePower = __instance.replicatePower;
                Debug.Log("Loaded mecha. Replicator speed was: " + __instance.replicateSpeed);
                __instance.replicateSpeed = 10.0f;
                Debug.Log("Replicator speed is now : " + __instance.replicateSpeed);
                Debug.Log("Removing energy cost for replication" + __instance.replicateSpeed);
                __instance.replicatePower = 0.0;
            }
        }
        [HarmonyPatch(typeof(Mecha), "Export")]
        class Mecha_Export
        {
            static bool Prefix(ref Mecha __instance)
            {
                Debug.Log("Restoring values for saving");
                __instance.replicateSpeed = FreeReplicatorMode.originalReplicateSpeed;
                __instance.replicatePower = FreeReplicatorMode.originalReplicatePower;
                return true;
            }
            static void Postfix(ref Mecha __instance)
            {
                Debug.Log("Saving done. Changing back to patched values");
                __instance.replicateSpeed = 10.0f;
                __instance.replicatePower = 0.0;
            }
        }

        [HarmonyPatch(typeof(StorageComponent), "TakeItem")]
        class StorageComponent_TakeItem
        {
            static bool Prefix(ref int __result, ref int count)
            {
                __result = count;
                return false;
            }
        }
        [HarmonyPatch(typeof(MechaForge), "CancelTask")]
        class MechaForge_CancelTask
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);

                bool foundAddItemStacked = false;
                
                MethodInfo methodInfo = typeof(StorageComponent).GetMethod("AddItemStacked",BindingFlags.Public | BindingFlags.Instance);

                for(int i = 0; i < codes.Count; i++)
                {
                    if (codes[i].opcode == OpCodes.Callvirt && codes[i].operand.Equals(methodInfo))
                        foundAddItemStacked = true;
                   
                    if(
                        foundAddItemStacked && 
                        codes[i].opcode == OpCodes.Ldloc_S && 
                        codes[i+1].opcode == OpCodes.Ldarg_1 && 
                        codes[i+2].opcode == OpCodes.Ble)
                    {
                        codes[i].opcode = OpCodes.Nop;
                        codes[i + 1].opcode = OpCodes.Nop;
                        codes[i + 2].opcode = OpCodes.Nop;

                        Debug.Log("Removed item return from MechaForge::CancelTask(int)");
                        break;
                    }

                }

                return codes.AsEnumerable();
            }
        }
    }
}
