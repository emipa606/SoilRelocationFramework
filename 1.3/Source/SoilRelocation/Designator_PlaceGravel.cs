using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace SR
{
    public class Designator_PlaceGravel : Designator_Build
    {
        public Designator_PlaceGravel() : base(DefDatabase<BuildableDef>.GetNamed("Building_Gravel"))
        {

        }

        public override int DraggableDimensions
        {
            get
            {
                return 2;
            }
        }

        public override bool DragDrawMeasurements
        {
            get
            {
                return true;
            }
        }

        //private static BuildableDef buildableDef = DefDatabase<BuildableDef>.GetNamed("Building_Gravel");

        //public override BuildableDef PlacingDef
        //{
        //    get
        //    {
        //        return buildableDef;
        //    }
        //}

        //public override ThingStyleDef ThingStyleDefForPreview
        //{
        //    get
        //    {
        //        return null;
        //    }
        //}

        //public override ThingDef StuffDef
        //{
        //    get
        //    {
        //        return null;
        //    }
        //}

        public override AcceptanceReport CanDesignateCell(IntVec3 loc)
        {
            if (loc.GetTerrain(Map).defName != "Gravel")//only allow gravel, temp
			{
				return "Must designate gravel.";
			}
            return AcceptanceReport.WasAccepted;
        }
    }
}
