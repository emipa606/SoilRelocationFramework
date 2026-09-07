using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace SR;

public static class TerrainDefExtensions
{
    private static readonly Dictionary<TerrainDef, bool> bridgeCache = new();

    extension(TerrainDef def)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsDiggable()
        {
            if (def.affordances.Contains(DefOfs.Diggable))
            {
                return def.driesTo == null;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsBridge()
        {
            if (!bridgeCache.ContainsKey(def))
            {
                bridgeCache[def] = def.bridge || def.label.ToLowerInvariant().Contains("bridge") ||
                                   def.defName.ToLowerInvariant().Contains("bridge");
            }

            return bridgeCache[def];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasSoilPlaceWorker()
        {
            return def.placeWorkers != null && def.placeWorkers.Contains(typeof(PlaceWorker_Soil));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsWetBridgeable()
        {
            return def.driesTo != null && def.affordances.Contains(TerrainAffordanceDefOf.Bridgeable);
        }
    }
}