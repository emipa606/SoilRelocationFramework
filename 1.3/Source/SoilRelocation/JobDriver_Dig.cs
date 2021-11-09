using System;
using Verse;
using RimWorld;

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
			TerrainDef t = TerrainDef.Named("Gravel"); //What we are setting the terrain to, stone is a placeholder.
			Map.terrainGrid.SetTerrain(TargetLocA, t); //Set the terrain to the above.
			FilthMaker.RemoveAllFilth(TargetLocA, Map);
		}
	}
}
