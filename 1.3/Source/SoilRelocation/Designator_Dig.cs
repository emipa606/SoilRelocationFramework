using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace SR
{
	public class Designator_Dig : Designator
	{
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

		public Designator_Dig()
		{
			defaultLabel = "DesignatorDig".Translate();
			defaultDesc = "DesignatorDigDesc".Translate();
			icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveFloor", true);
			useMouseIcon = true;
			soundDragSustain = SoundDefOf.Designate_DragStandard;
			soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			soundSucceeded = SoundDefOf.Designate_Mine;
			//this.hotKey = KeyBindingDefOf.Misc5;
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t != null && CanDesignateCell(t.Position).Accepted)
			{
				return AcceptanceReport.WasAccepted;
			}
			return false;
		}

		public override void DesignateThing(Thing t)
		{
			DesignateSingleCell(t.Position);
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(Map))
			{
				return false;
			}
			if (c.Fogged(Map))
			{
				return false;
			}
			if (Map.designationManager.DesignationAt(c, DesignationDefOf.SR_Dig) != null)
			{
				return "Already digging.".Translate();
			}
			if (c.InNoBuildEdgeArea(Map))
			{
				return "TooCloseToMapEdge".Translate();
			}
			Building edifice = c.GetEdifice(Map);
			if (edifice != null)
			{
				return "Must remove building first.".Translate();
			}

			if (!c.GetTerrain(Map).affordances.Contains(TerrainAffordanceDefOf.GrowSoil))
			{
				return "Mest designate soil.".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.SR_Dig));
		}

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
