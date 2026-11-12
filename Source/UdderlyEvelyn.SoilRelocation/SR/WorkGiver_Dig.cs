using RimWorld;
using Verse;
using Verse.AI;

namespace SR;

public class WorkGiver_Dig : WorkGiver_ConstructAffectFloor
{
    protected override DesignationDef DesDef => DesignationDefOf.SR_Dig;

    public override Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false)
    {
        return JobMaker.MakeJob(JobDefs.SR_Dig, cell);
    }
}