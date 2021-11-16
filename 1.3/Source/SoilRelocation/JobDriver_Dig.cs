using System;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace SR
{
	public class JobDriver_Dig : JobDriver_AffectFloor
	{
		protected override int BaseWorkAmount
		{
			get
			{
				return 800; //smoothing a floor was 2800
			}
		}

		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SR_Dig;
			}
		}

		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.ConstructionSpeed;
			}
		}

		public JobDriver_Dig()
		{
			clearSnow = true;
		}

		protected override void DoEffect(IntVec3 c)
		{
			TerrainDef ot = c.GetTerrain(Map);
			//Spawn item based on old terrain (ot)..
			if (ot.costList.Count == 0)
				Utilities.DropThing(Map, c, DefDatabase<ThingDef>.GetNamed("SR_Soil"), Rand.Range(73, 76)); //Drop regular soil, assume the item is supposed to yield that.
			else
				Utilities.DropThings(Map, c, ot.costList, 2, 1);
			TerrainDef ut = Map.terrainGrid.UnderTerrainAt(c); //Get under-terrain
			if (ut != null) //If there was under-terrain
			{
				Map.terrainGrid.SetTerrain(c, ut); //Set the top layer to the under-terrain
				Map.terrainGrid.SetUnderTerrain(c, null); //Clear the under-terrain
			}
			else //No under-terrain
			{
				var st = Map.GetComponent<CMS.MapComponent_StoneGrid>().StoneTerrainAt(c);
				Map.terrainGrid.SetTerrain(TargetLocA, st); //Set the terrain to the natural stone for this area to represent bedrock
			}
			FilthMaker.RemoveAllFilth(TargetLocA, Map);
		}
	}
}
