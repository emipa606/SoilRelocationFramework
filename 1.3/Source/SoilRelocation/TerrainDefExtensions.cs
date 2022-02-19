using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace SR
{
    public static class TerrainDefExtensions
    {
        public static bool IsDiggable(this TerrainDef def)
        {
            return def.affordances.Contains(TerrainAffordanceDefOf.Diggable) && def.driesTo == null;
        }
    }
}
