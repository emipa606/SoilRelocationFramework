using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace SR
{
	public class Designator_Place : Designator
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

		public Designator_Place()
		{
			defaultLabel = "DesignatorPlace".Translate();
			defaultDesc = "DesignatorPlaceDesc".Translate();
			this.icon = TexCommand.Install; 
			useMouseIcon = true;
			soundDragSustain = SoundDefOf.Designate_DragStandard;
			soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			soundSucceeded = SoundDefOf.Designate_SmoothSurface;
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
			if (Map.designationManager.DesignationAt(c, DesignationDefOf.SR_Place) != null)
			{
				return "Already placing soil.".Translate();
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
			if (c.GetTerrain(Map).defName != "Gravel")//affordances.Contains(TerrainAffordanceDefOf.GrowSoil)) only allow gravel, temp
			{
				return "Mest designate gravel.".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			Blueprint_Build blueprint_Build = GenConstruct.PlaceBlueprintForBuild_NewTemp(DefDatabase<BuildableDef>.GetNamed("Stool"), c, base.Map, Rot4.North, Faction.OfPlayer, null, null, null);
			//Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.SR_Place));
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
