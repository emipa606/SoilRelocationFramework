using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace SR;

[HarmonyPatch(typeof(Blueprint_Build), nameof(Blueprint_Build.TotalMaterialCost))]
internal static class Blueprint_Build_TotalMaterialCost
{
    internal static void Postfix(Blueprint_Build __instance, ref List<ThingDefCountClass> __result)
    {
        var map = __instance.Map;
        if (map == null || __instance.def.entityDefToBuild is not TerrainDef terrainDef ||
            terrainDef == TerrainDefOf.Ice)
        {
            return;
        }

        _ = __instance.Position;
        var num = HarmonyPatchSharedData.DeriveMultiplierForFill(__instance.Position.GetTerrain(map), terrainDef);
        if (num == 1f)
        {
            return;
        }

        var list = new List<ThingDefCountClass>();
        foreach (var thingDefCountClass in __result)
        {
            list.Add(new ThingDefCountClass(thingDefCountClass.thingDef,
                Mathf.RoundToInt(thingDefCountClass.count * num)));
        }

        __result = list;
    }
}