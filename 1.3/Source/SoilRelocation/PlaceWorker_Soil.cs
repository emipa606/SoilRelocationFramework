using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace SR
{
    public class PlaceWorker_Soil : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            var underTerrain = map.terrainGrid.UnderTerrainAt(loc);
            if ((loc.GetTerrain(map).IsBridge() //It's a bridge..
                || (TerrainSystemOverhaul_Interop.TerrainSystemOverhaulPresent && TerrainSystemOverhaul_Interop.GetBridge(map.terrainGrid, loc) != null)) //Or it's a bridge (with TSO)..
                && !(underTerrain.IsWater || WaterFreezes_Interop.IsThawableIce(underTerrain))) //And it isn't water or thawable ice.
                    return "CannotPlaceOnBridgeUnlessWaterOrThawableIce".Translate();
            return true;
        }
    }
}