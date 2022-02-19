using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace SR.ToggleablePatches
{
    internal class VFEArchitect
    {
        internal static ToggleablePatch<BuildableDef> PackedDirtCostsDirt = new ToggleablePatch<BuildableDef>
        {
            Name = "VFE Architect Packed Dirt Costs Dirt",
            Enabled = SoilRelocationSettings.VFEArchitectPackedDirtCostsDirtEnabled,
            TargetDefName = "VFEArch_PlayerPackedDirt",
            TargetModID = "VanillaExpanded.VFEArchitect",
            Patch = def =>
            {
                if (def.costList == null)
                    def.costList = new List<ThingDefCountClass>();
                def.costList.Add(new ThingDefCountClass { count = 1, thingDef = SoilDefs.SR_Soil }); //Add an additional cost of 1 soil just to remove the exploit of free dirt.
            },
            Unpatch = def =>
            {
                var costListItems = def.costList.Where(tdcc => tdcc.thingDef == SoilDefs.SR_Soil); //Try to find our patch..
                if (costListItems.Any()) //If we find it..
                    def.CostList.Remove(costListItems.First()); //Yeet!
            },
        };
    }
}
