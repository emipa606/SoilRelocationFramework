using Verse;

namespace SR.ToggleablePatches;

internal class Vanilla
{
    [ToggleablePatch] internal static readonly ToggleablePatch<BuildableDef> SandbagsUseSandPatch =
        new()
        {
            Name = "Sandbags Use Sand",
            Enabled = SoilRelocationSettings.SandbagsUseSandEnabled,
            TargetDefName = "Sandbags",
            Patch = delegate(ToggleablePatch<BuildableDef> _, BuildableDef def)
            {
                def.costList ??= [];

                def.costList.Add(new ThingDefCountClass
                {
                    count = 5,
                    thingDef = SoilDefs.SR_Sand
                });
            },
            Unpatch = delegate(ToggleablePatch<BuildableDef> _, BuildableDef def)
            {
                def.costList.RemoveAll(tdcc => tdcc.thingDef == SoilDefs.SR_Sand);
            }
        };

    [ToggleablePatch] internal static ToggleablePatch<BuildableDef> FungalGravelUsesRawFungusPatch =
        new()
        {
            Name = "Fungal Gravel Uses Raw Fungus",
            Enabled = SoilRelocationSettings.FungalGravelUsesRawFungusEnabled,
            TargetDefName = "FungalGravel",
            TargetModID = "Ludeon.RimWorld.Ideology",
            Patch = delegate(ToggleablePatch<BuildableDef> _, BuildableDef def)
            {
                def.costList ??= [];

                def.costList.Add(new ThingDefCountClass
                {
                    count = 5,
                    thingDef = DefOfs.RawFungus
                });
            },
            Unpatch = delegate(ToggleablePatch<BuildableDef> _, BuildableDef def)
            {
                def.CostList.RemoveAll(tdcc => tdcc.thingDef == DefOfs.RawFungus);
            }
        };
}