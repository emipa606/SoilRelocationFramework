using HarmonyLib;
using RimWorld;
using Verse;

namespace SR;

[HarmonyPatch(typeof(Frame), nameof(Frame.WorkToBuild), MethodType.Getter)]
internal static class Frame_WorkToBuild_get
{
    internal static void Postfix(Frame __instance, ref float __result)
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