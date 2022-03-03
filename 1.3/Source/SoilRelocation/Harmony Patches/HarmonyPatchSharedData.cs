using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using UnityEngine;

namespace SR
{
    internal static class HarmonyPatchSharedData
    {
        internal static float GravelMultiplier = .33f;

        internal static Dictionary<TerrainDef, float> MultiplierByWaterDef = new Dictionary<TerrainDef, float>
        {
            { TerrainDefs.Marsh, 2.8f },
            { TerrainDefOf.WaterShallow, 4 },
            { TerrainDefOf.WaterMovingShallow, 10 },
            { TerrainDefOf.WaterDeep, 32 },
            { TerrainDefOf.WaterMovingChestDeep, 64 },
            { TerrainDefOf.WaterOceanShallow, 64 },
            { TerrainDefOf.WaterOceanDeep, 128  },
        };

        internal static float DeriveMultiplierForFill(TerrainDef currentTerrain, TerrainDef newTerrain, bool gravelAdjusts = true)
        {
            float multiplier = 1;
            //If it's water now and it will become soil (but not ice).
            if (currentTerrain.IsWater && newTerrain.IsDiggable() && newTerrain.defName != "Ice")
            {
                if (MultiplierByWaterDef.ContainsKey(currentTerrain))
                    multiplier = MultiplierByWaterDef[currentTerrain];
                else //Default catch-all for modded water..
                    multiplier = 4; //Hope it's appropriate, lol.
                if (gravelAdjusts && newTerrain == TerrainDefOf.Gravel)
                    multiplier *= GravelMultiplier;
            }
            return multiplier;
        }

        internal static List<ThingDefCountClass> GetWaterFillAdjustedCostListForFrame(BuildableDef entDef, ThingDef stuff, bool errorOnNullStuff, Frame frame)
        {
            var originalList = entDef.CostListAdjusted(stuff);
            var map = frame.Map;
            if (map == null)
                return originalList;
            var newTerrain = entDef as TerrainDef;
            if (newTerrain == null) //If it's not a TerrainDef
                return originalList; //We don't need to touch it.
            var cell = frame.Position;
            var currentTerrain = frame.Position.GetTerrain(map);
            float multiplier = HarmonyPatchSharedData.DeriveMultiplierForFill(currentTerrain, newTerrain);
            if (multiplier != 1) //If the multiplier will do anything..
            {
                var newList = new List<ThingDefCountClass>();
                var sb = new StringBuilder();
                for (int i = 0; i < originalList.Count; i++)
                {
                    var oldPair = originalList[i];
                    var newPair = new ThingDefCountClass(oldPair.thingDef, Mathf.RoundToInt(oldPair.count * multiplier));
                    newList.Add(newPair);
                    sb.AppendLine(oldPair.thingDef.defName + ": " + oldPair.count + " -> " + newPair.count);
                }
                SoilRelocation.Log("Water Filling Cost Adjustment (Frame)\n" + sb.ToString());
                return newList;
            }
            return originalList;
        }
    }
}