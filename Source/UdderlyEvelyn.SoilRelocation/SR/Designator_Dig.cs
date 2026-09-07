using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using UnityEngine;
using Verse;

namespace SR;

public class Designator_Dig : Designator
{
    private static readonly FieldInfo StartDragCellField =
        typeof(DesignationDragger).GetField("startDragCell", BindingFlags.Instance | BindingFlags.NonPublic);

    public Designator_Dig()
    {
        defaultLabel = "Dig".Translate();
        defaultDesc = "DigDescNew".Translate();
        icon = ContentFinder<Texture2D>.Get("DesignatorDig");
        useMouseIcon = true;
        soundDragSustain = SoundDefOf.Designate_DragStandard;
        soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
        soundSucceeded = SoundDefOf.Designate_Mine;
    }

    private TerrainDef ShiftLockedTerrain
    {
        get
        {
            if (!TryGetStartDragCell(out var startCell))
            {
                return null;
            }

            field = startCell.GetTerrain(Map);
            if (field != null && field.affordances.Contains(DefOfs.Diggable))
            {
                return field;
            }

            return null;
        }
        set;
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

    public override AcceptanceReport CanDesignateCell(IntVec3 loc)
    {
        if (!loc.InBounds(Map))
        {
            return false;
        }

        if (loc.Fogged(Map))
        {
            return false;
        }

        if (Map.designationManager.DesignationAt(loc, DesignationDefOf.SR_Dig) != null)
        {
            return "AlreadyDigging".Translate();
        }

        if (loc.InNoBuildEdgeArea(Map))
        {
            return "TooCloseToMapEdge".Translate();
        }

        if (loc.GetFirstBuilding(Map) != null)
        {
            return "RemoveBuildingFirst".Translate();
        }

        var terrain = loc.GetTerrain(Map);
        if (!terrain.affordances.Contains(DefOfs.Diggable))
        {
            return "TerrainCannotBeDug".Translate();
        }

        if (terrain.driesTo != null)
        {
            return "TerrainTooMoist".Translate();
        }

        if (IsShiftHeld())
        {
            return terrain != ShiftLockedTerrain
                ? "Terrain must match the first selected soil while holding SHIFT."
                : AcceptanceReport.WasAccepted;
        }

        ShiftLockedTerrain = null;
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

    private static bool IsShiftHeld()
    {
        return Event.current != null && Event.current.shift || Input.GetKey(KeyCode.LeftShift) ||
               Input.GetKey(KeyCode.RightShift);
    }

    private static bool TryGetStartDragCell(out IntVec3 startCell)
    {
        startCell = IntVec3.Invalid;
        if (StartDragCellField == null)
        {
            return false;
        }

        var raw = StartDragCellField.GetValue(Find.DesignatorManager.Dragger);
        if (raw is not IntVec3 { IsValid: true } cell)
        {
            return false;
        }

        startCell = cell;
        return true;
    }
}