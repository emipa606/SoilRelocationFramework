using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;
using System.Reflection.Emit;
using System.Reflection;

namespace SR
{
    [HarmonyPatch(typeof(TerrainGrid), "SetTerrain")]
    public class TerrainGrid_SetTerrain
    {
        internal static void Prefix(IntVec3 c, TerrainDef newTerr, Map ___map, ref (TerrainDef, bool) __state)
        {
            __state = (___map.terrainGrid.TerrainAt(c), ___map.terrainGrid.UnderTerrainAt(c) == null);
        }

        internal static void Postfix(IntVec3 c, TerrainDef newTerr, Map ___map, ref (TerrainDef, bool) __state)
        {
            if (__state.Item1 == newTerr || __state.Item1 == null) //If we're not actually changing anything or it had no terrain previously..
                return; //Who cares?
            if (newTerr.placeWorkers.Contains(typeof(PlaceWorker_Soil)) && __state.Item2) //If the new terrain is one of the soils we care about and there was no underTerrain before..
                ___map.terrainGrid.SetUnderTerrain(c, __state.Item1); //Store the old terrain as underTerrain.
        }
    }
}