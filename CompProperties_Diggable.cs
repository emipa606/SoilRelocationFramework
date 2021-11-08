using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace SR
{
    public class CompProperties_Diggable : CompProperties
    {

        public CompProperties_Diggable()
        {
            compClass = typeof(CompDiggable);
        }

        public int workToDig;

    }
}
