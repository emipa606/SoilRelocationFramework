using System;
using Verse;
using RimWorld;
using System.Collections.Generic;
using Verse.AI;
using System.Reflection;
using UnityEngine;

namespace SR
{
	public class JobDriver_Dig : JobDriver_AffectFloor
	{
		protected Dictionary<TerrainDef, ThingDef> _noCostItemGuessCache = new Dictionary<TerrainDef, ThingDef>();
		protected Func<IntVec3, float> _takeCellIceDelegate;
		protected Type _lakesCanFreezeMapComponentType = Type.GetType("LCF.MapComponent_LakesCanFreeze, UdderlyEvelyn.LakesCanFreeze");
		protected float workLeft = -1000f;

		protected override int BaseWorkAmount
		{
			get
			{
				return 800; //smoothing a floor was 2800
			}
		}

		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SR_Dig;
			}
		}

		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.MiningSpeed;
			}
		}

		protected virtual SkillDef Skill
        {
			get
            {
				return SkillDefOf.Mining;
            }
        }

		public JobDriver_Dig()
		{
			clearSnow = true;
		}

		protected override void DoEffect(IntVec3 c)
		{
			TerrainDef ot = c.GetTerrain(Map);
			//Spawn item based on old terrain (ot)..
			if (ot.costList == null || ot.costList.Count == 0)
			{
				bool newKey = true;
				ThingDef toDrop = null;
				int toDropAmount = Rand.Range(6, 10);
				if (_noCostItemGuessCache.ContainsKey(ot))
				{
					toDrop = _noCostItemGuessCache[ot];
					newKey = false;
				}
				else
				{
					if (ot.defName.ToLowerInvariant().Contains("soil") || ot.defName.ToLowerInvariant().Contains("dirt") || ot.label.ToLowerInvariant().Contains("soil") || ot.label.ToLowerInvariant().Contains("dirt"))
						toDrop = SoilDefs.SR_Soil;
					else if (ot.defName.ToLowerInvariant().Contains("ice") || ot.label.ToLowerInvariant().Contains("ice"))
						toDrop = SoilDefs.SR_Ice;
					else if (ot.defName.ToLowerInvariant().Contains("sand") || ot.label.ToLowerInvariant().Contains("sand"))
						toDrop = SoilDefs.SR_Sand;
					else
						Log.Warning("[Soil Relocation] Unsupported soil \"" + ot.defName + "\" AKA \"" + ot.label + "\" being dug, was not able to guess what to drop, report this to the creator of the mod it came from or UdderlyEvelyn to fix this.");
					if (newKey)
						_noCostItemGuessCache.Add(ot, toDrop);
				}
				if (toDrop != null)
				{
					//Handle LakesCanFreeze Ice..
					if (ot.defName == "LCF_LakeIceThin" || ot.defName == "LCF_LakeIce" || ot.defName == "LCF_LakeIceThick")
						toDropAmount = Mathf.RoundToInt(LakesCanFreeze_takeCellIce(c).Value) / 100 * toDropAmount;
					Utilities.DropThing(Map, c, toDrop, toDropAmount); //Drop the item
				}
			}
			else
				Utilities.DropThings(Map, c, ot.costList, 2, 1);
			TerrainDef ut = Map.terrainGrid.UnderTerrainAt(c); //Get under-terrain
			if (ut != null && !(ut == TerrainDefOf.WaterDeep || ut == TerrainDefOf.WaterShallow)) //If there was under-terrain but it wasn't water (LCF compat)..
			{
				Map.terrainGrid.SetTerrain(c, ut); //Set the top layer to the under-terrain
				Map.terrainGrid.SetUnderTerrain(c, null); //Clear the under-terrain
			}
			else //No under-terrain
			{
				var st = Map.GetComponent<CMS.MapComponent_StoneGrid>().StoneTerrainAt(c);
				Map.terrainGrid.SetTerrain(TargetLocA, st); //Set the terrain to the natural stone for this area to represent bedrock
			}
			FilthMaker.RemoveAllFilth(TargetLocA, Map);
		}

		/// <summary>
		/// Call MapComponent_LakesCanFreeze.takeCellIce without needing to reference the assembly.
		/// </summary>
		/// <param name="cell">cell to get the ice thickness for and clear the ice at</param>
		/// <returns>ice thickness at cell prior to clearing</returns>
		public float? LakesCanFreeze_takeCellIce(IntVec3 cell)
        {
			if (_lakesCanFreezeMapComponentType != null)
			{
				if (_takeCellIceDelegate == null) //Everything in here should only execute once if the mod is present.
				{
					MapComponent lcfComp = Map.GetComponent(_lakesCanFreezeMapComponentType); //Try to get it.
					if (lcfComp != null) //It was found.
						_takeCellIceDelegate = (Func<IntVec3, float>)_lakesCanFreezeMapComponentType.GetMethod("TakeCellIce").CreateDelegate(typeof(Func<IntVec3, float>), lcfComp); //Cache it..
					else
						Log.Error("[Soil Relocation] Lakes Can Freeze was detected but MapComponent_LakesCanFreeze could not be retrieved for this map.");
				}
				return _takeCellIceDelegate(cell);
			}
			return null; //Mod not loaded, return null.
		}

		//Tynan thought it was a good idea to accept a SpeedStat but not accept a SkillDef, so now I have to copy this just to change the hardcoded references.
		//Oh and also it used a private variable for workLeft even though the method is protected and overrideable so I have to reproduce that too. Mine will be protected, as it should be.
		//Come on..
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => (!job.ignoreDesignations && base.Map.designationManager.DesignationAt(base.TargetLocA, DesDef) == null) ? true : false);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil();
			doWork.initAction = delegate
			{
				workLeft = BaseWorkAmount;
			};
			doWork.tickAction = delegate
			{
				float num = ((SpeedStat != null) ? doWork.actor.GetStatValue(SpeedStat) : 1f);
				num *= 1.7f;
				workLeft -= num;
				if (doWork.actor.skills != null)
				{
					doWork.actor.skills.Learn(Skill, 0.1f);
				}
				if (clearSnow)
				{
					base.Map.snowGrid.SetDepth(base.TargetLocA, 0f);
				}
				if (workLeft <= 0f)
				{
					DoEffect(base.TargetLocA);
					base.Map.designationManager.DesignationAt(base.TargetLocA, DesDef)?.Delete();
					ReadyForNextToil();
				}
			};
			doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.WithProgressBar(TargetIndex.A, () => 1f - workLeft / (float)BaseWorkAmount);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.activeSkill = () => Skill;
			yield return doWork;
		}
	}
}
