using System;
using System.Collections.Generic;
using CMS;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace SR;

public class JobDriver_Dig : JobDriver_AffectFloor
{
    private readonly Dictionary<TerrainDef, ThingDef> _noCostItemGuessCache = new();

    private float workLeft = -1000f;

    private float workTotal;

    public JobDriver_Dig()
    {
        clearSnow = true;
    }

    protected override int BaseWorkAmount => 800;

    protected override DesignationDef DesDef => DesignationDefOf.SR_Dig;

    protected override StatDef SpeedStat => StatDefOf.MiningSpeed;

    protected virtual SkillDef Skill => SkillDefOf.Mining;

    protected override void DoEffect(IntVec3 c)
    {
        var map = Map;
        var terrain = c.GetTerrain(map);
        if (!terrain.IsDiggable())
        {
            return;
        }

        if (terrain.costList == null || terrain.costList.Count == 0)
        {
            ThingDef thingDef = null;
            var num = Rand.Range(6, 10);
            if (_noCostItemGuessCache.TryGetValue(terrain, out var value))
            {
                thingDef = value;
            }
            else
            {
                var text = terrain.defName.ToLowerInvariant();
                var text2 = terrain.label.ToLowerInvariant();
                if (text.Contains("soil") || text.Contains("dirt") || text2.Contains("soil") || text2.Contains("dirt"))
                {
                    thingDef =
                        !text.Contains("fertile") && !text.Contains("rich") && !text2.Contains("fertile") &&
                        !text2.Contains("rich")
                            ? !text.Contains("stony") && !text.Contains("rocky") && !text2.Contains("stony") &&
                              !text2.Contains("rocky")
                                ? SoilDefs.SR_Soil
                                : SoilDefs.SR_Gravel
                            : SoilDefs.SR_RichSoil;
                }
                else if (text.Contains("ice") || text2.Contains("ice"))
                {
                    thingDef = SoilDefs.SR_Ice;
                }
                else if (text.Contains("sand") || text2.Contains("sand"))
                {
                    thingDef = SoilDefs.SR_Sand;
                }
                else if (text.Contains("gravel") || text2.Contains("gravel"))
                {
                    thingDef = SoilDefs.SR_Gravel;
                }
                else
                {
                    SoilRelocation.Log(
                        $"Unsupported soil \"{terrain.defName}\" AKA \"{terrain.label}\" being dug, was not able to guess what to drop, report this to the creator of the mod it came from or UdderlyEvelyn to fix this.",
                        ErrorLevel.Warning);
                }

                _noCostItemGuessCache.Add(terrain, thingDef);
            }

            if (thingDef != null)
            {
                if (WaterFreezes_Interop.IsThawableIce(terrain))
                {
                    num = Math.Max(1, Mathf.RoundToInt(WaterFreezes_Interop.TakeCellIce(map, c).Value / 25f * num));
                    var newTerr = WaterFreezes_Interop.QueryCellAllWater(map, c);
                    if (WaterFreezes_Interop.QueryCellWater(map, c) > 0f)
                    {
                        map.terrainGrid.SetTerrain(c, newTerr);
                    }
                    else if (map.Biome == BiomeDefOf.SeaIce)
                    {
                        map.terrainGrid.SetTerrain(c, TerrainDefOf.WaterOceanDeep);
                    }
                    else
                    {
                        map.terrainGrid.SetTerrain(c, TerrainDefs.Mud);
                    }

                    Utilities.DropThing(map, c, thingDef, num);
                    return;
                }

                Utilities.DropThing(map, c, thingDef, num);
            }
        }
        else
        {
            Utilities.DropThings(map, c, terrain.costList, 2, 1);
        }

        var terrainDef = map.terrainGrid.UnderTerrainAt(c);
        if (terrainDef != null)
        {
            map.terrainGrid.SetTerrain(c, terrainDef);
        }
        else if (terrain == TerrainDefOf.Ice && map.Biome == BiomeDefOf.SeaIce)
        {
            map.terrainGrid.SetTerrain(c, TerrainDefOf.WaterOceanDeep);
        }
        else
        {
            map.terrainGrid.SetTerrain(c, map.GetComponent<MapComponent_StoneGrid>().StoneTerrainAt(c));
        }

        FilthMaker.RemoveAllFilth(c, map);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        this.FailOn(() => !job.ignoreDesignations && Map.designationManager.DesignationAt(TargetLocA, DesDef) == null);
        yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
        var doWork = new Toil
        {
            initAction = delegate
            {
                var targetLocA = TargetLocA;
                var terrainDef = Map.terrainGrid.TerrainAt(targetLocA);
                if (!terrainDef.affordances.Contains(DefOfs.Diggable))
                {
                    Map.designationManager.DesignationAt(TargetLocA, DesDef)?.Delete();
                    ReadyForNextToil();
                }

                if (terrainDef.defName == "LCF_LakeIceThin" || terrainDef.defName == "LCF_LakeIce" ||
                    terrainDef.defName == "LCF_LakeIceThick")
                {
                    workTotal = (BaseWorkAmount / 2f) +
                                (WaterFreezes_Interop.QueryCellIce(Map, TargetLocA).Value / 100f *
                                 (BaseWorkAmount / 2f));
                }
                else
                {
                    workTotal = BaseWorkAmount;
                }

                workLeft = workTotal;
            }
        };
        doWork.tickAction = delegate
        {
            var num = SpeedStat != null ? doWork.actor.GetStatValue(SpeedStat) : 1f;
            num *= 1.7f;
            workLeft -= num;
            doWork.actor.skills?.Learn(Skill, 0.1f);

            if (clearSnow)
            {
                Map.snowGrid.SetDepth(TargetLocA, 0f);
            }

            if (!(workLeft <= 0f))
            {
                return;
            }

            DoEffect(TargetLocA);
            Map.designationManager.DesignationAt(TargetLocA, DesDef)?.Delete();
            ReadyForNextToil();
        };
        doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
        doWork.WithProgressBar(TargetIndex.A, () => 1f - (workLeft / workTotal));
        doWork.defaultCompleteMode = ToilCompleteMode.Never;
        doWork.activeSkill = () => Skill;
        yield return doWork;
    }
}