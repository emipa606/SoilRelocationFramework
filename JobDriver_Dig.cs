using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using Verse.AI;

namespace SR
{

    public class JobDriver_Dig : JobDriver
    {

        private const TargetIndex SoilInd = TargetIndex.A;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, errorOnFailed: errorOnFailed);
        }

        private CompDiggable DiggableComp => TargetThingA.TryGetComp<CompDiggable>();

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(SoilInd);
            this.FailOnThingMissingDesignation(SoilInd, DesignationDefOf.SR_Dig);

            // Go to soil
            var goToToil = Toils_Goto.GotoThing(SoilInd, PathEndMode.Touch);
            goToToil.FailOnDespawnedNullOrForbidden(SoilInd);
            yield return goToToil;

            // Dig soil
            yield return DigToil();
        }

        private Toil DigToil()
        {
            var dig = new Toil();
            dig.initAction = () =>
            {
                digTicksLeft = DiggableComp.Props.workToDig;
            };
            dig.tickAction = () =>
            {
                if (digTicksLeft > 0)
                    digTicksLeft--;

                else
                {
                    var actor = dig.actor;
                    var soil = job.targetA.Thing;

                    // Dig
                    DiggableComp.Dig();

                    // Remove designator
                    var rearmDes = soil.Map.designationManager.DesignationOn(soil, DesignationDefOf.SR_Dig);
                    if (rearmDes != null)
                        rearmDes.Delete();

                    // Finalise
                    actor.records.Increment(RecordDefOf.SR_SoilDug);
                    actor.jobs.EndCurrentJob(JobCondition.Succeeded);
                }
            };
            dig.FailOnCannotTouch(SoilInd, PathEndMode.Touch);
            dig.WithProgressBar(SoilInd, () => 1 - ((float)digTicksLeft / DiggableComp.Props.workToDig));
            dig.defaultCompleteMode = ToilCompleteMode.Never;
            return dig;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref digTicksLeft, "digTicksLeft");
            base.ExposeData();
        }

        private int digTicksLeft;

    }
}
