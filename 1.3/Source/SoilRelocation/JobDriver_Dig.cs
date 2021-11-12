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
			thing.stackCount = Rand.Range(190, 210);
			thingOwner.TryAdd(thing, true);
			Thing item;
			thingOwner.TryDrop(thing, c, Map, ThingPlaceMode.Direct, out item);

			TerrainDef nt = TerrainDef.Named("Gravel"); //What we are setting the terrain to, stone is a placeholder.
			Map.terrainGrid.SetTerrain(TargetLocA, nt); //Set the terrain to the above.
			FilthMaker.RemoveAllFilth(TargetLocA, Map);
		}
	}
}
