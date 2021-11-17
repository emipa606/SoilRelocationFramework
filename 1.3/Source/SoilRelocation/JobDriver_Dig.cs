using System;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace SR
{
	public class JobDriver_Dig : JobDriver_AffectFloor
	{
		private Dictionary<TerrainDef, ThingDef> _noCostItemGuessCache = new Dictionary<TerrainDef, ThingDef>();

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
			//cache Soil/Ice/Sand
			if (ot.costList == null || ot.costList.Count == 0)
			{
				bool newKey = true;
				ThingDef toDrop = null;
				if (_noCostItemGuessCache.ContainsKey(ot))
				{
					toDrop = _noCostItemGuessCache[ot];
					newKey = false;
				}
				else
				{
					if (ot.defName.Contains("soil") || ot.defName.Contains("dirt") || ot.label.Contains("soil") || ot.label.Contains("dirt"))
						toDrop = SoilDefs.SR_Soil;
					else if (ot.defName.Contains("ice") || ot.label.Contains("ice"))
						toDrop = SoilDefs.SR_Ice;
					else if (ot.defName.Contains("sand") || ot.label.Contains("sand"))
						toDrop = SoilDefs.SR_Sand;
					else
						Log.Warning("Unsupported soil \"" + ot.defName + "\" AKA \"" + ot.label + "\" being dug, was not able to guess what to drop, report this to the creator of the mod it came from or Evelyn from SR to fix this.");
					if (newKey)
						_noCostItemGuessCache.Add(ot, toDrop);
				}
				if (toDrop != null)
					Utilities.DropThing(Map, c, toDrop, Rand.Range(73, 76)); //Drop regular sand, assume the item is supposed to yield that.
			}
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
