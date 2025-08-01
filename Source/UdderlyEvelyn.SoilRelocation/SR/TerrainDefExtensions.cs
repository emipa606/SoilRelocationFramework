using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace SR;

public static class TerrainDefExtensions
{
    private static readonly Dictionary<TerrainDef, bool> bridgeCache = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDiggable(this TerrainDef def)
    {
        if (def.affordances.Contains(DefOfs.Diggable))
        {
            return def.driesTo == null;
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBridge(this TerrainDef def)
    {
        if (!bridgeCache.ContainsKey(def))
        {
            bridgeCache[def] = def.bridge || def.label.ToLowerInvariant().Contains("bridge") ||
                               def.defName.ToLowerInvariant().Contains("bridge");
        }

        return bridgeCache[def];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasSoilPlaceWorker(this TerrainDef def)
    {
        return def.placeWorkers != null && def.placeWorkers.Contains(typeof(PlaceWorker_Soil));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsWetBridgeable(this TerrainDef def)
    {
        return def.driesTo != null && def.affordances.Contains(TerrainAffordanceDefOf.Bridgeable);
    }
}