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
			string thingDefName = "SR_Soil";
			switch (ot.defName)
            {
				case "SoilRich":
					thingDefName = "SR_RichSoil";
					break;
				case "Sand":
					thingDefName = "SR_Sand";
					break;
				//Other cases will fall through, meaning Soil, MossyTerrain, Gravel, and anything added by mods which will yield SR_Soil (in the case of mods we'd wanna add code for support here).
            }
			//Code modified from part of GenLeaving.DoLeavingsFor
			ThingOwner<Thing> thingOwner = new ThingOwner<Thing>();
			Thing thing = ThingMaker.MakeThing(ThingDef.Named(thingDefName), null);
			thing.stackCount = Rand.Range(73, 76); //Decent chance to lose soil digging it up (cost is 75 for a tile), tiny chance to gain some to add some randomness to it, lossy process since soil is messy.
			thingOwner.TryAdd(thing, true);
			Thing item;
			thingOwner.TryDrop(thing, c, Map, ThingPlaceMode.Direct, out item);
			TerrainDef ut = Map.terrainGrid.UnderTerrainAt(c); //Get under-terrain
			if (ut != null) //If there was under-terrain
			{
				//Log.Warning("There was underterrain, it's \"" + ut.defName + "\", swapping it into place where \"" + ot.defName + "\" was.");
				Map.terrainGrid.SetTerrain(c, ut); //Set the top layer to the under-terrain
				Map.terrainGrid.SetUnderTerrain(c, null); //Clear the under-terrain
			}
			else //No under-terrain
			{
				var st = Map.GetComponent<CMS.MapComponent_StoneGrid>().StoneTypeAt(c);
				//Log.Warning("There was no underterrain, placing \"" + st.defName + "\" where \"" +  ot.defName + "\" was.");
				Map.terrainGrid.SetTerrain(TargetLocA, st); //Set the terrain to the natural stone for this area to represent bedrock
			}
			FilthMaker.RemoveAllFilth(TargetLocA, Map);
		}
	}
}
