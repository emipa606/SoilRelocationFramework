using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace SR
{
    public class CompDiggable : ThingComp
    {

        public CompProperties_Diggable Props => (CompProperties_Diggable)props;

        public override string CompInspectStringExtra()
        {
            return (dug ? "SoilRelocation.Bedrock" : "SoilRelocation.Diggable").Translate();
        }

        public void Dig()
        {
            dug = true;
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            dug = false;
            base.PostSpawnSetup(respawningAfterLoad);
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref dug, "dug");
            base.PostExposeData();
        }

        public bool dug;
    }
}
