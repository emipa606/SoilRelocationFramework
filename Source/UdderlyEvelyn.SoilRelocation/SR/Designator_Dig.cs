using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace SR;

public class Designator_Dig : Designator
{
    public Designator_Dig()
    {
        defaultLabel = "Dig".Translate();
        defaultDesc = "DigDesc".Translate();
        icon = ContentFinder<Texture2D>.Get("DesignatorDig");
        useMouseIcon = true;
        soundDragSustain = SoundDefOf.Designate_DragStandard;
        soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
        soundSucceeded = SoundDefOf.Designate_Mine;
    }

    public override DrawStyleCategoryDef DrawStyleCategory => DrawStyleCategoryDefOf.Orders;

    public override bool DragDrawMeasurements => true;

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
            return "AlreadyDigging".Translate();
        }

        if (c.InNoBuildEdgeArea(Map))
        {
            return "TooCloseToMapEdge".Translate();
        }

        if (c.GetFirstBuilding(Map) != null)
        {
            return "RemoveBuildingFirst".Translate();
        }

        var terrain = c.GetTerrain(Map);
        if (!terrain.affordances.Contains(DefOfs.Diggable))
        {
            return "TerrainCannotBeDug".Translate();
        }

        return terrain.driesTo != null ? "TerrainTooMoist".Translate() : AcceptanceReport.WasAccepted;
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