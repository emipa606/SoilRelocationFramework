using System;
using Verse;
using RimWorld;

namespace SR
{
	public class JobDriver_Place : JobDriver_AffectFloor
	{
		protected override int BaseWorkAmount
		{
			get
			{
				return 400; //smoothing a floor was 2800, digging is 800 so I figure with gravity helping it's half the work.
			}
		}

		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SR_Place;
			}
		}

		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.ConstructionSpeed;
			}
		}

		public JobDriver_Place()
		{
			clearSnow = true;
		}

		protected override void DoEffect(IntVec3 c)
		{
			TerrainDef t = TerrainDef.Named("Soil"); //What we are setting the terrain to, blindly placing soil is a placeholder.
			Map.terrainGrid.SetTerrain(TargetLocA, t); //Set the terrain to the above.
			FilthMaker.RemoveAllFilth(TargetLocA, Map);
		}
	}
}
