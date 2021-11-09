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
				Log.Message("Getting DesDef from WorkGiver_Place");
				return DesignationDefOf.SR_Place;
			}
		}

		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Log.Message("Running WorkGiver_Place.JobOnCell");
			return JobMaker.MakeJob(JobDefOf.SR_Place, c);
		}
	}
}
