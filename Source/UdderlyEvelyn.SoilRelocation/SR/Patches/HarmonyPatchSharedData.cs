using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace SR;

internal static class HarmonyPatchSharedData
{
    private static readonly float GravelMultiplier = 0.33f;

    private static readonly Dictionary<TerrainDef, float> MultiplierByWaterDef = new()
    {
        {
            TerrainDefs.Marsh,
            2.8f
        },
        {
            TerrainDefOf.WaterShallow,
            4f
        },
        {
            TerrainDefOf.WaterMovingShallow,
            10f
        },
        {
            TerrainDefOf.WaterDeep,
            32f
        },
        {
            TerrainDefOf.WaterMovingChestDeep,
            64f
        },
        {
            TerrainDefOf.WaterOceanShallow,
            64f
        },
        {
            TerrainDefOf.WaterOceanDeep,
            128f
        }
    };

    internal static float DeriveMultiplierForFill(TerrainDef currentTerrain, TerrainDef newTerrain,
        bool gravelAdjusts = true)
    {
        var num = 1f;
        var bridgeable = currentTerrain.IsWetBridgeable();
        var isWater = currentTerrain.IsWater;
        if (!isWater && !bridgeable || newTerrain == TerrainDefOf.Ice || !newTerrain.HasSoilPlaceWorker())
        {
            return num;
        }

        num = isWater && MultiplierByWaterDef.TryGetValue(currentTerrain, out var value) ? value :
            !bridgeable ? 4f : 2f;
        if (gravelAdjusts && newTerrain == TerrainDefOf.Gravel)
        {
            num *= GravelMultiplier;
        }

        return num;
    }

    internal static List<ThingDefCountClass> GetWaterFillAdjustedCostListForFrame(BuildableDef entDef, ThingDef stuff,
        bool errorOnNullStuff, Frame frame)
    {
        var list = entDef.CostListAdjusted(stuff);
        var map = frame.Map;
        if (map == null)
        {
            return list;
        }

        if (entDef is not TerrainDef terrainDef || terrainDef == TerrainDefOf.Ice)
        {
            return list;
        }

        _ = frame.Position;
        var num = DeriveMultiplierForFill(frame.Position.GetTerrain(map), terrainDef);
        if (num == 1f)
        {
            return list;
        }

        var list2 = new List<ThingDefCountClass>();
        foreach (var thingDefCountClass in list)
        {
            list2.Add(new ThingDefCountClass(thingDefCountClass.thingDef,
                Mathf.RoundToInt(thingDefCountClass.count * num)));
        }

        return list2;
    }
}