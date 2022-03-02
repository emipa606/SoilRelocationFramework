using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace SR
{
    [HarmonyPatch(typeof(Frame), "MaterialsNeeded")]
    internal static class Frame_MaterialsNeeded
    {
        internal static void Postfix(Frame __instance, ref List<ThingDefCountClass> __result)
        {
            var map = __instance.Map;
            if (map == null)
                return;
            var newTerrain = __instance.def.entityDefToBuild as TerrainDef;
            if (newTerrain == null) //If it's not a TerrainDef
                return; //We don't need to touch it.
            var cell = __instance.Position;
            var currentTerrain = __instance.Position.GetTerrain(map);
            float multiplier = HarmonyPatchSharedData.DeriveMultiplierForFill(currentTerrain, newTerrain);
            if (multiplier != 1) //If the multiplier will do anything..
            {
                var newList = new List<ThingDefCountClass>();
                for (int i = 0; i < __result.Count; i++)
                {
                    var oldPair = __result[i];
                    newList.Add(new ThingDefCountClass(oldPair.thingDef, Mathf.RoundToInt(oldPair.count * multiplier)));
                }
                __result = newList;
            }
        }
    }
}