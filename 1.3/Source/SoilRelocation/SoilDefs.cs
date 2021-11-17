using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace SR
{
    public static class SoilDefs
    {
        public static ThingDef SR_Soil = DefDatabase<ThingDef>.GetNamed("SR_Soil");
        public static ThingDef SR_Sand = DefDatabase<ThingDef>.GetNamed("SR_Sand");
        public static ThingDef SR_Ice = DefDatabase<ThingDef>.GetNamed("SR_Ice");
    }
}
