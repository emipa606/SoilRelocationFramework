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
			

			TerrainDef nt = TerrainDef.Named("Gravel"); //What we are setting the terrain to, stone is a placeholder.
			Map.terrainGrid.SetTerrain(TargetLocA, nt); //Set the terrain to the above.
			FilthMaker.RemoveAllFilth(TargetLocA, Map);
			//Code modified from part of GenLeaving.DoLeavingsFor
			ThingOwner<Thing> thingOwner = new ThingOwner<Thing>();
			Thing thing = ThingMaker.MakeThing(ThingDef.Named("Silver"), null);
			thing.stackCount = 1;
			thingOwner.TryAdd(thing, true);
			Thing item;
			thingOwner.TryDrop(thing, c, Map, ThingPlaceMode.Direct, out item);
		}
	}
}
