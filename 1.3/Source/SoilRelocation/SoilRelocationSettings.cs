using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace SR
{
    public class SoilRelocationSettings : ModSettings
    {
        public static bool SandbagsUseSandEnabled = true;
        public static bool DubsSkylightsGlassUsesSandEnabled = true;
        public static bool VFEArchitectPackedDirtCostsDirtEnabled = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref SandbagsUseSandEnabled, "SandbagsUseSandEnabled");
            Scribe_Values.Look(ref DubsSkylightsGlassUsesSandEnabled, "DubsSkylightsGlassUsesSandEnabled");
            Scribe_Values.Look(ref VFEArchitectPackedDirtCostsDirtEnabled, "VFEArchitectPackedDirtCostsDirtEnabled");

            base.ExposeData();
        }
    }
}