using HarmonyLib;
using RimWorld;
using Verse;

namespace SR;

[HarmonyPatch(typeof(Blueprint_Build), "WorkTotal", MethodType.Getter)]
internal static class Blueprint_Build_WorkTotal
{
    internal static void Postfix(Blueprint_Build __instance, ref float __result)
    {
        var map = __instance.Map;
        if (map == null || __instance.def.entityDefToBuild is not TerrainDef newTerrain)
        {
            return;
        }

        _ = __instance.Position;
        var terrain = __instance.Position.GetTerrain(map);
        __result *= HarmonyPatchSharedData.DeriveMultiplierForFill(terrain, newTerrain, false);
    }
}