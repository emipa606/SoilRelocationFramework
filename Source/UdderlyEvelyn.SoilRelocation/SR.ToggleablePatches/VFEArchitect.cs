using Verse;

namespace SR.ToggleablePatches;

internal class VFEArchitect
{
    [ToggleablePatch] internal static readonly ToggleablePatch<BuildableDef> PackedDirtCostsDirt =
        new ToggleablePatch<BuildableDef>
        {
            Name = "VFE Architect Packed Dirt Costs Dirt",
            Enabled = SoilRelocationSettings.VFEArchitectPackedDirtCostsDirtEnabled,
            TargetDefName = "VFEArch_PlayerPackedDirt",
            TargetModID = "VanillaExpanded.VFEArchitect",
            Patch = delegate(ToggleablePatch<BuildableDef> _, BuildableDef def)
            {
                if (def.costList == null)
                {
                    def.costList = [];
                }

                def.costList.Add(new ThingDefCountClass
                {
                    count = 1,
                    thingDef = SoilDefs.SR_Soil
                });
            },
            Unpatch = delegate(ToggleablePatch<BuildableDef> _, BuildableDef def)
            {
                def.CostList.RemoveAll(tdcc => tdcc.thingDef == SoilDefs.SR_Soil);
            }
        };
}