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
		protected Func<IntVec3, float> _queryCellIceDelegate;
		protected Func<IntVec3, float> _queryCellWaterDelegate;
		protected Func<IntVec3, TerrainDef> _queryCellNaturalWaterDelegate;
		protected Type _lakesCanFreezeMapComponentType = Type.GetType("LCF.MapComponent_LakesCanFreeze, UdderlyEvelyn.LakesCanFreeze");
		protected float workLeft = -1000f;
		protected float workTotal = 0;

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
			if (!ot.affordances.Contains(TerrainAffordanceDefOf.Diggable)) //If the terrain isn't diggable (maybe the terrain was swapped out dynamically, e.g., LakesCanFreeze)..
				return; //Abort.
			TerrainDef ut;
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
					{
						toDropAmount = Math.Max(1, Mathf.RoundToInt(LakesCanFreeze_TakeCellIce(c).Value / 25 * toDropAmount));
						ut = Map.terrainGrid.UnderTerrainAt(c); //Get under-terrain
						var utIsWater = ut == TerrainDefOf.WaterDeep || ut == TerrainDefOf.WaterShallow;
						var naturalWater = LakesCanFreeze_QueryCellNaturalWater(c);
						var isNaturalWater = naturalWater != null;
						var water = LakesCanFreeze_QueryCellWater(c);
						//Log.Message("[Soil Relocation] LCF Compat.. utIsWater: " + utIsWater + ", naturalWater: " + naturalWater?.defName + ", isNaturalWater: " + isNaturalWater + ", water: " + water);
						if ((isNaturalWater || utIsWater) && water <= 0) //If natural water isn't null or under-terrain is water but there's no water at that tile..
							Map.terrainGrid.SetTerrain(c, Map.GetComponent<CMS.MapComponent_StoneGrid>().StoneTerrainAt(c)); //Set the terrain to the natural stone for this area to represent bedrock
						else if (isNaturalWater && water > 0) //If it's natural water and there's more than 0 water..
							Map.terrainGrid.SetTerrain(c, naturalWater); //Set it to its natural water type.
						else if (ut != null) //It's got water at the cell but the cell isn't set to water, but water is in the under-terrain..
						{
							Map.terrainGrid.SetTerrain(c, ut); //Set the top layer to the under-terrain
							Map.terrainGrid.SetUnderTerrain(c, null); //Clear the under-terrain
						}
						else
							Log.Error("[Soil Relocation] Attempted to dig LakesCanFreeze ice but there was no under-terrain, it was not natural water with water depth, and it wasn't zero-depth natural or under-terrain water.");

						Utilities.DropThing(Map, c, toDrop, toDropAmount); //Drop the item
						return; //Don't need to run the rest of the code, LCF has special handling above.
					}
					Utilities.DropThing(Map, c, toDrop, toDropAmount); //Drop the item
				}
			}
			else
				Utilities.DropThings(Map, c, ot.costList, 2, 1);
			ut = Map.terrainGrid.UnderTerrainAt(c); //Get under-terrain
			if (ut != null) //If there was under-terrain..
			{
				Map.terrainGrid.SetTerrain(c, ut); //Set the top layer to the under-terrain
				Map.terrainGrid.SetUnderTerrain(c, null); //Clear the under-terrain
			}
			else //No under-terrain
				Map.terrainGrid.SetTerrain(c, Map.GetComponent<CMS.MapComponent_StoneGrid>().StoneTerrainAt(c)); //Set the terrain to the natural stone for this area to represent bedrock
			FilthMaker.RemoveAllFilth(c, Map);
		}

		/// <summary>
		/// Call MapComponent_LakesCanFreeze.QueryCellNaturalWater without needing to reference the assembly.
		/// </summary>
		/// <param name="cell">cell to get the natural water def for</param>
		/// <returns>TerrainDef of natural water at cell, null if none</returns>
		public TerrainDef LakesCanFreeze_QueryCellNaturalWater(IntVec3 cell)
		{
			if (_lakesCanFreezeMapComponentType != null)
			{
				if (_queryCellNaturalWaterDelegate == null) //Everything in here should only execute once if the mod is present.
				{
					MapComponent lcfComp = Map.GetComponent(_lakesCanFreezeMapComponentType); //Try to get it.
					if (lcfComp != null) //It was found.
						_queryCellNaturalWaterDelegate = (Func<IntVec3, TerrainDef>)_lakesCanFreezeMapComponentType.GetMethod("QueryCellNaturalWater").CreateDelegate(typeof(Func<IntVec3, TerrainDef>), lcfComp); //Cache it..
					else
						Log.Error("[Soil Relocation] Lakes Can Freeze was detected but MapComponent_LakesCanFreeze could not be retrieved for this map.");
				}
				return _queryCellNaturalWaterDelegate(cell);
			}
			return null; //Mod not loaded, return null.
		}

		/// <summary>
		/// Call MapComponent_LakesCanFreeze.QueryCellWater without needing to reference the assembly.
		/// </summary>
		/// <param name="cell">cell to get the water depth for</param>
		/// <returns>water depth at cell</returns>
		public float? LakesCanFreeze_QueryCellWater(IntVec3 cell)
		{
			if (_lakesCanFreezeMapComponentType != null)
			{
				if (_queryCellWaterDelegate == null) //Everything in here should only execute once if the mod is present.
				{
					MapComponent lcfComp = Map.GetComponent(_lakesCanFreezeMapComponentType); //Try to get it.
					if (lcfComp != null) //It was found.
						_queryCellWaterDelegate = (Func<IntVec3, float>)_lakesCanFreezeMapComponentType.GetMethod("QueryCellWater").CreateDelegate(typeof(Func<IntVec3, float>), lcfComp); //Cache it..
					else
						Log.Error("[Soil Relocation] Lakes Can Freeze was detected but MapComponent_LakesCanFreeze could not be retrieved for this map.");
				}
				return _queryCellWaterDelegate(cell);
			}
			return null; //Mod not loaded, return null.
		}

		/// <summary>
		/// Call MapComponent_LakesCanFreeze.TakeCellIce without needing to reference the assembly.
		/// </summary>
		/// <param name="cell">cell to get the ice thickness for and clear the ice at</param>
		/// <returns>ice thickness at cell prior to clearing</returns>
		public float? LakesCanFreeze_TakeCellIce(IntVec3 cell)
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

		/// <summary>
		/// Call MapComponent_LakesCanFreeze.QueryCellIce without needing to reference the assembly.
		/// </summary>
		/// <param name="cell">cell to get the ice thickness for</param>
		/// <returns>ice thickness at cell</returns>
		public float? LakesCanFreeze_QueryCellIce(IntVec3 cell)
		{
			if (_lakesCanFreezeMapComponentType != null)
			{
				if (_queryCellIceDelegate == null) //Everything in here should only execute once if the mod is present.
				{
					MapComponent lcfComp = Map.GetComponent(_lakesCanFreezeMapComponentType); //Try to get it.
					if (lcfComp != null) //It was found.
						_queryCellIceDelegate = (Func<IntVec3, float>)_lakesCanFreezeMapComponentType.GetMethod("QueryCellIce").CreateDelegate(typeof(Func<IntVec3, float>), lcfComp); //Cache it..
					else
						Log.Error("[Soil Relocation] Lakes Can Freeze was detected but MapComponent_LakesCanFreeze could not be retrieved for this map.");
				}
				return _queryCellIceDelegate(cell);
			}
			return null; //Mod not loaded, return null.
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => (!job.ignoreDesignations && base.Map.designationManager.DesignationAt(base.TargetLocA, DesDef) == null) ? true : false);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil();
			doWork.initAction = delegate
			{
				var target = base.TargetLocA;
				var currentTerrain = Map.terrainGrid.TerrainAt(target);
				if (!currentTerrain.affordances.Contains(TerrainAffordanceDefOf.Diggable))
				{
					Map.designationManager.DesignationAt(base.TargetLocA, DesDef)?.Delete(); //Get rid of the designation, invalid.
					ReadyForNextToil(); //Don't keep trying to do this job.
				}
				if (currentTerrain.defName == "LCF_LakeIceThin" || currentTerrain.defName == "LCF_LakeIce" || currentTerrain.defName == "LCF_LakeIceThick")
					workTotal = (BaseWorkAmount / 2) + (LakesCanFreeze_QueryCellIce(base.TargetLocA).Value / 100) * (BaseWorkAmount / 2);
				else
					workTotal = BaseWorkAmount;
				workLeft = workTotal;
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
			doWork.WithProgressBar(TargetIndex.A, () => 1f - workLeft / (float)workTotal);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.activeSkill = () => Skill;
			yield return doWork;
		}
	}
}
