using RimWorld;
using Verse;

namespace SR;

public class PlaceWorker_Soil : PlaceWorker
{
    public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map,
        Thing thingToIgnore = null, Thing thing = null)
    {
        var terrain = loc.GetTerrain(map);
        var terrainDef = map.terrainGrid.UnderTerrainAt(loc);
        var isBridge = terrain.IsBridge() || TerrainSystemOverhaul_Interop.TerrainSystemOverhaulPresent &&
            TerrainSystemOverhaul_Interop.GetBridge(map.terrainGrid, loc) != null;
        if (checkingDef == TerrainDefOf.Ice)
        {
            if (isBridge)
            {
                return "CannotPlaceIceOnBridge".Translate();
            }

            if (terrain.IsWater || terrain.IsWetBridgeable())
            {
                return "CannotFillWithIce".Translate();
            }
        }

        if (terrain.driesTo != null && !terrain.affordances.Contains(TerrainAffordanceDefOf.Bridgeable))
        {
            return "CannotPlaceOnWetUnlessBridgeable".Translate();
        }

        if (isBridge && !terrainDef.IsWater && !WaterFreezes_Interop.IsThawableIce(terrainDef))
        {
            return "CannotPlaceOnBridgeUnlessWaterOrThawableIce".Translate();
        }

        if (terrain == TerrainDefOf.WaterOceanDeep && map.Biome == BiomeDefOf.SeaIce)
        {
            return "CannotFillDeepOceanOnSeaIce".Translate();
        }

        return true;
    }
}