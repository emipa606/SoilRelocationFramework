using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;

namespace SR.Harmony_Patches
{
    [HarmonyPatch(typeof(Blueprint_Build), "WorkTotal", MethodType.Getter)]
    internal class Blueprint_Build_WorkTotal
    {
        float shallowMultiplier = 4;
        float deepMultiplier = 32;
        float marshMultiplier = 2.8f;
        float deepOceanMultiplier = 64;

        internal void Postfix(Blueprint_Build __instance, float __result)
        {
            var map = __instance.Map;
            var cell = __instance.Position;
            var currentTerrain = __instance.Position.GetTerrain(map);
            if (currentTerrain.IsWater && __instance.def.entityDefToBuild is TerrainDef tDef && tDef.IsDiggable())
            {
                if (currentTerrain == TerrainDefOf.WaterShallow ||
                    currentTerrain == TerrainDefOf.WaterMovingShallow)
                    __result *= shallowMultiplier;
                else if (currentTerrain == TerrainDefOf.WaterDeep ||
                    currentTerrain == TerrainDefOf.WaterMovingChestDeep ||
                    currentTerrain == TerrainDefOf.WaterOceanShallow)
                    __result *= deepMultiplier;
                else if (currentTerrain == TerrainDefs.Marsh)
                    __result *= marshMultiplier;
                else if (currentTerrain == TerrainDefOf.WaterOceanDeep)
                    __result *= deepOceanMultiplier;
            }
        }
    }
}