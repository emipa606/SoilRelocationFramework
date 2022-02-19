using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;
using System.Reflection;

namespace SR.Harmony_Patches
{
    [HarmonyPatch(typeof(Blueprint_Build), "MaterialsNeeded")]
    internal class Blueprint_Build_MaterialsNeeded
    {
        float shallowMultiplier = 4;
        float deepMultiplier = 32;
        float marshMultiplier = 2.8f;
        float deepOceanMultiplier = 64;

        internal void Postfix(Blueprint_Build __instance, List<ThingDefCount> __result)
        {
            var map = __instance.Map;
            var cell = __instance.Position;
            var currentTerrain = __instance.Position.GetTerrain(map);
            float multiplier = 1;
            if (currentTerrain.IsWater && __instance.def.entityDefToBuild is TerrainDef tDef && tDef.IsDiggable())
            {
                if (currentTerrain == TerrainDefOf.WaterShallow ||
                    currentTerrain == TerrainDefOf.WaterMovingShallow)
                    multiplier = shallowMultiplier;
                else if (currentTerrain == TerrainDefOf.WaterDeep ||
                    currentTerrain == TerrainDefOf.WaterMovingChestDeep ||
                    currentTerrain == TerrainDefOf.WaterOceanShallow)
                    multiplier = deepMultiplier;
                else if (currentTerrain == TerrainDefs.Marsh)
                    multiplier = marshMultiplier;
                else if (currentTerrain == TerrainDefOf.WaterOceanDeep)
                    multiplier = deepOceanMultiplier;
            }
            if (multiplier != 1) //If the multiplier will do anything..
                foreach (var pair in __result)
                    ReflectionCache.ThingDefCount_count.SetValue(pair, pair.Count * multiplier);
        }
    }
}