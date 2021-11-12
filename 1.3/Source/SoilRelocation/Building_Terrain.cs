using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace SR
{
    public abstract class Building_Terrain : Building
    {
        public Building_Terrain(string terrainDefName) : base()
        {
            TerrainDefName = terrainDefName;
        }

        protected virtual string TerrainDefName { get; set; }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            //Change terrain to specified type.
            TerrainDef nt = TerrainDef.Named(TerrainDefName); //What we are setting the terrain to, stone is a placeholder.
            Map.terrainGrid.SetTerrain(Position, nt); //Set the terrain to the above.
            FilthMaker.RemoveAllFilth(Position, Map);
            //Destroy the building instantly.
            if (!Destroyed)
            {
                Destroy(DestroyMode.Vanish);
            }
        }
    }
}
