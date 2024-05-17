using RimWorld;
using Verse;

namespace SR;

internal static class Frame_CompleteConstruction
{
    internal static void Prefix(Frame __instance)
    {
        var map = __instance.Map;
        if (map == null)
        {
            return;
        }

        var position = __instance.Position;
        if (position.GetTerrain(map).IsWater && __instance.def.entityDefToBuild is TerrainDef terrainDef &&
            terrainDef.IsDiggable() && terrainDef.defName != "Ice")
        {
            WaterFreezes_Interop.ClearCellNaturalWater(map, position);
        }
    }
}