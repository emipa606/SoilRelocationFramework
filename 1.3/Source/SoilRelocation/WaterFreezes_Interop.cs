using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace SR
{
    public static class WaterFreezes_Interop
	{
		private static Type _waterFreezesMapComponentType = Type.GetType("WF.MapComponent_WaterFreezes, UdderlyEvelyn.WaterFreezes");
		private static Dictionary<int, MapComponent> _waterFreezesMapComponentCachePerMap = new Dictionary<int, MapComponent>();
		private static Func<IntVec3, float> _takeCellIceDelegate;
		private static Func<IntVec3, float> _queryCellIceDelegate;
		private static Func<IntVec3, float> _queryCellWaterDelegate;
		private static Func<IntVec3, TerrainDef> _queryCellNaturalWaterDelegate;

		/// <summary>
		/// Call MapComponent_WaterFreezes.QueryCellNaturalWater without needing to reference the assembly.
		/// </summary>
		/// <param name="cell">cell to get the natural water def for</param>
		/// <returns>TerrainDef of natural water at cell, null if none</returns>
		public static TerrainDef QueryCellNaturalWater(Map map, IntVec3 cell)
		{
			if (_waterFreezesMapComponentType != null)
			{
				if (_queryCellNaturalWaterDelegate == null) //Everything in here should only execute once if the mod is present.
				{
					MapComponent wfComp; //Set up var.
					if (!_waterFreezesMapComponentCachePerMap.ContainsKey(map.uniqueID)) //If not cached..
						_waterFreezesMapComponentCachePerMap.Add(map.uniqueID, wfComp = map.GetComponent(_waterFreezesMapComponentType)); //Get and cache.
					else
						wfComp = _waterFreezesMapComponentCachePerMap[map.uniqueID]; //Get from cache.
					if (wfComp != null) //It was found.
						_queryCellNaturalWaterDelegate = (Func<IntVec3, TerrainDef>)_waterFreezesMapComponentType.GetMethod("QueryCellNaturalWater").CreateDelegate(typeof(Func<IntVec3, TerrainDef>), wfComp); //Cache it..
					else
						Log.Error("[Soil Relocation] Water Freezes was detected but MapComponent_WaterFreezes could not be retrieved for this map.");
				}
				return _queryCellNaturalWaterDelegate(cell);
			}
			return null; //Mod not loaded, return null.
		}

		/// <summary>
		/// Call MapComponent_WaterFreezes.QueryCellWater without needing to reference the assembly.
		/// </summary>
		/// <param name="cell">cell to get the water depth for</param>
		/// <returns>water depth at cell</returns>
		public static float? QueryCellWater(Map map, IntVec3 cell)
		{
			if (_waterFreezesMapComponentType != null)
			{
				if (_queryCellWaterDelegate == null) //Everything in here should only execute once if the mod is present.
				{
					MapComponent wfComp; //Set up var.
					if (!_waterFreezesMapComponentCachePerMap.ContainsKey(map.uniqueID)) //If not cached..
						_waterFreezesMapComponentCachePerMap.Add(map.uniqueID, wfComp = map.GetComponent(_waterFreezesMapComponentType)); //Get and cache.
					else
						wfComp = _waterFreezesMapComponentCachePerMap[map.uniqueID]; //Get from cache.
					if (wfComp != null) //It was found.
						_queryCellWaterDelegate = (Func<IntVec3, float>)_waterFreezesMapComponentType.GetMethod("QueryCellWater").CreateDelegate(typeof(Func<IntVec3, float>), wfComp); //Cache it..
					else
						Log.Error("[Soil Relocation] Water Freezes was detected but MapComponent_WaterFreezes could not be retrieved for this map.");
				}
				return _queryCellWaterDelegate(cell);
			}
			return null; //Mod not loaded, return null.
		}

		/// <summary>
		/// Call MapComponent_WaterFreezes.TakeCellIce without needing to reference the assembly.
		/// </summary>
		/// <param name="cell">cell to get the ice thickness for and clear the ice at</param>
		/// <returns>ice thickness at cell prior to clearing</returns>
		public static float? TakeCellIce(Map map, IntVec3 cell)
		{
			if (_waterFreezesMapComponentType != null)
			{
				if (_takeCellIceDelegate == null) //Everything in here should only execute once if the mod is present.
				{
					MapComponent wfComp; //Set up var.
					if (!_waterFreezesMapComponentCachePerMap.ContainsKey(map.uniqueID)) //If not cached..
						_waterFreezesMapComponentCachePerMap.Add(map.uniqueID, wfComp = map.GetComponent(_waterFreezesMapComponentType)); //Get and cache.
					else
						wfComp = _waterFreezesMapComponentCachePerMap[map.uniqueID]; //Get from cache.
					if (wfComp != null) //It was found.
						_takeCellIceDelegate = (Func<IntVec3, float>)_waterFreezesMapComponentType.GetMethod("TakeCellIce").CreateDelegate(typeof(Func<IntVec3, float>), wfComp); //Cache it..
					else
						Log.Error("[Soil Relocation] Water Freezes was detected but MapComponent_WaterFreezes could not be retrieved for this map.");
				}
				return _takeCellIceDelegate(cell);
			}
			return null; //Mod not loaded, return null.
		}

		/// <summary>
		/// Call MapComponent_WaterFreezes.QueryCellIce without needing to reference the assembly.
		/// </summary>
		/// <param name="cell">cell to get the ice thickness for</param>
		/// <returns>ice thickness at cell</returns>
		public static float? QueryCellIce(Map map, IntVec3 cell)
		{
			if (_waterFreezesMapComponentType != null)
			{
				if (_queryCellIceDelegate == null) //Everything in here should only execute once if the mod is present.
				{
					MapComponent wfComp; //Set up var.
					if (!_waterFreezesMapComponentCachePerMap.ContainsKey(map.uniqueID)) //If not cached..
						_waterFreezesMapComponentCachePerMap.Add(map.uniqueID, wfComp = map.GetComponent(_waterFreezesMapComponentType)); //Get and cache.
					else
						wfComp = _waterFreezesMapComponentCachePerMap[map.uniqueID]; //Get from cache.
					if (wfComp != null) //It was found.
						_queryCellIceDelegate = (Func<IntVec3, float>)_waterFreezesMapComponentType.GetMethod("QueryCellIce").CreateDelegate(typeof(Func<IntVec3, float>), wfComp); //Cache it..
					else
						Log.Error("[Soil Relocation] Water Freezes was detected but MapComponent_WaterFreezes could not be retrieved for this map.");
				}
				return _queryCellIceDelegate(cell);
			}
			return null; //Mod not loaded, return null.
		}
	}
}
