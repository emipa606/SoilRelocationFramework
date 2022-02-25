using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace SR.ToggleablePatches
{
    internal class Vanilla
    {
        internal static ToggleablePatch<BuildableDef> SandbagsUseSandPatch = new ToggleablePatch<BuildableDef>
        {
            Name = "Sandbags Use Sand",
            Enabled = SoilRelocationSettings.SandbagsUseSandEnabled,
            TargetDefName = "Sandbags",
            Patch = def =>
            {
                if (def.costList == null)
                    def.costList = new List<ThingDefCountClass>();
                def.costList.Add(new ThingDefCountClass { count = 5, thingDef = SoilDefs.SR_Sand }); //Add an additional cost of 5 sand.
            },
            Unpatch = def =>
            {
                var costListItems = def.costList.Where(tdcc => tdcc.thingDef == SoilDefs.SR_Sand); //Try to find our patch..
                if (costListItems.Any()) //If we find it..
                    def.CostList.Remove(costListItems.First()); //Yeet!
            },
        };

        internal static ToggleablePatch<BuildableDef> FungalGravelUsesRawFungusPatch = new ToggleablePatch<BuildableDef>
        {
            Name = "Fungal Gravel Uses Raw Fungus",
            Enabled = SoilRelocationSettings.FungalGravelUsesRawFungusEnabled,
            TargetDefName = TerrainDefOf.FungalGravel.defName,
            TargetModID = "Ludeon.RimWorld.Ideology",
            Patch = def =>
            {
                if (def.costList == null)
                    def.costList = new List<ThingDefCountClass>();
                def.costList.Add(new ThingDefCountClass { count = 5, thingDef = ThingDefs.RawFungus }); //Add an additional cost of 5 raw fungus.
            },
            Unpatch = def =>
            {
                var costListItems = def.costList.Where(tdcc => tdcc.thingDef == ThingDefs.RawFungus); //Try to find our patch..
                if (costListItems.Any()) //If we find it..
                    def.CostList.Remove(costListItems.First()); //Yeet!
            },
        };
    }
}
