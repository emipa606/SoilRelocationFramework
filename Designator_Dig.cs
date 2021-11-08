using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace SR
{
    public class Designator_Dig : Designator
    {

        public Designator_Dig()
        {
            defaultLabel = "SoilRelocation.DesignatorDig".Translate();
            defaultDesc = "SoilRelocation.DesignatorDig_Description".Translate();
            icon = TexCommand.AttackMelee;
            soundDragSustain = SoundDefOf.Designate_DragStandard;
            soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
            useMouseIcon = true;
            soundSucceeded = SoundDefOf.Designate_Haul;
        }

        public override int DraggableDimensions => 2;

        protected override DesignationDef Designation => DesignationDefOf.SR_Dig;

        public override AcceptanceReport CanDesignateCell(IntVec3 loc)
        {
            // Out of bounds
            if (!loc.InBounds(Map))
                return false;

            // No rearmables
            if (!RearmablesInCell(loc).Any())
                return "SoilRelocation.MessageMustDesignateDiggables".Translate();

            return true;
        }

        public override void DesignateSingleCell(IntVec3 c)
        {
            var thingList = c.GetThingList(Map);
            for (int i = 0; i < thingList.Count; i++)
                DesignateThing(thingList[i]);
        }

        public override void DesignateThing(Thing t)
        {
            Map.designationManager.RemoveAllDesignationsOn(t);
            Map.designationManager.AddDesignation(new Designation(t, Designation));
        }

        private IEnumerable<Thing> RearmablesInCell(IntVec3 c)
        {
            // Out of bounds
            if (!c.InBounds(Map))
                yield break;

            var thingList = c.GetThingList(Map);
            for (int i = 0; i < thingList.Count; i++)
            {
                var thing = thingList[i];
                if (CanDesignateThing(thing).Accepted)
                    yield return thing;
            }
        }

        public override AcceptanceReport CanDesignateThing(Thing t)
        {
            var diggableComp = t.TryGetComp<CompDiggable>();
            return diggableComp != null && !diggableComp.dug && Map.designationManager.DesignationOn(t, Designation) == null;
        }

    }
}
