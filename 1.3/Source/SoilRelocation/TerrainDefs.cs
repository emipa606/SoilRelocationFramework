using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace SR
{
    public static class TerrainDefs
    {
        public static TerrainDef Marsh = DefDatabase<TerrainDef>.GetNamed("Marsh");
        public static TerrainDef Mud = DefDatabase<TerrainDef>.GetNamed("Mud");
    }
}
