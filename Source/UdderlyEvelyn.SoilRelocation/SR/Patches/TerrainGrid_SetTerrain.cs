using HarmonyLib;
using Verse;

namespace SR;

[HarmonyPatch(typeof(TerrainGrid), nameof(TerrainGrid.SetTerrain))]
public class TerrainGrid_SetTerrain
{
    internal static void Prefix(IntVec3 c, Map ___map, ref (TerrainDef, bool) __state)
    {
        __state = (___map.terrainGrid.TerrainAt(c), ___map.terrainGrid.UnderTerrainAt(c) == null);
    }

    internal static void Postfix(IntVec3 c, TerrainDef newTerr, Map ___map, ref (TerrainDef, bool) __state)
    {
        if (__state.Item1 == newTerr || __state is not { Item1: not null, Item2: true } ||
            !newTerr.HasSoilPlaceWorker() ||
            __state.Item1.IsWater || __state.Item1.IsWetBridgeable())
        {
            return;
        }

        //___map.terrainGrid.SetUnderTerrain(c, __state.Item1);
        ___map.fertilityGrid.FertilityGridUpdate();
    }
}