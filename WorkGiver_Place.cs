using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace SR
{
	public class WorkGiver_Place : WorkGiver_ConstructAffectFloor
	{
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SR_Place;
			}
		}

		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.SR_Place, c);
		}
	}
}
