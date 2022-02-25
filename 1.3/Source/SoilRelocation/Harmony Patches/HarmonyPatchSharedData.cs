using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace SR
{
    internal static class HarmonyPatchSharedData
    {
        internal static float GravelDivisor = 3;

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
                    multiplier -= GravelDivisor;
            }
            return multiplier;
        }
    }
}