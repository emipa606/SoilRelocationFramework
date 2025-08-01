using System;
using Verse;

namespace SR;

public static class TerrainSystemOverhaul_Interop
{
    private static readonly Type _terrainSystemOverhaulUtilsType = Type.GetType("TSO.Utils, TSO");

    private static Func<TerrainGrid, IntVec3, TerrainDef> _getBridgeDelegate;

    public static bool TerrainSystemOverhaulPresent => _terrainSystemOverhaulUtilsType != null;

    public static TerrainDef GetBridge(TerrainGrid terrGrid, IntVec3 c)
    {
        if (_terrainSystemOverhaulUtilsType == null)
        {
            return null;
        }

        _getBridgeDelegate ??= (Func<TerrainGrid, IntVec3, TerrainDef>)_terrainSystemOverhaulUtilsType
            .GetMethod("GetBridge")
            ?.CreateDelegate(typeof(Func<TerrainGrid, IntVec3, TerrainDef>));

        return _getBridgeDelegate?.Invoke(terrGrid, c);
    }
}