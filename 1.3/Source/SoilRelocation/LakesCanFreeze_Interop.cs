using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace SR
{
    public static class LakesCanFreeze_Interop
	{
		private static Type _lakesCanFreezeMapComponentType = Type.GetType("LCF.MapComponent_LakesCanFreeze, UdderlyEvelyn.LakesCanFreeze");
		private static Dictionary<int, MapComponent> _lakesCanFreezeMapComponentCachePerMap = new Dictionary<int, MapComponent>();
		private static Func<IntVec3, float> _takeCellIceDelegate;
		private static Func<IntVec3, float> _queryCellIceDelegate;
		private static Func<IntVec3, float> _queryCellWaterDelegate;
		private static Func<IntVec3, TerrainDef> _queryCellNaturalWaterDelegate;

		/// <summary>
		/// Call MapComponent_LakesCanFreeze.QueryCellNaturalWater without needing to reference the assembly.
		/// </summary>
		/// <param name="cell">cell to get the natural water def for</param>
		/// <returns>TerrainDef of natural water at cell, null if none</returns>
		public static TerrainDef QueryCellNaturalWater(Map map, IntVec3 cell)
		{
			if (_lakesCanFreezeMapComponentType != null)
			{
				if (_queryCellNaturalWaterDelegate == null) //Everything in here should only execute once if the mod is present.
				{
					MapComponent lcfComp; //Set up var.
					if (!_lakesCanFreezeMapComponentCachePerMap.ContainsKey(map.uniqueID)) //If not cached..
						_lakesCanFreezeMapComponentCachePerMap.Add(map.uniqueID, lcfComp = map.GetComponent(_lakesCanFreezeMapComponentType)); //Get and cache.
					else
						lcfComp = _lakesCanFreezeMapComponentCachePerMap[map.uniqueID]; //Get from cache.
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
		public static float? QueryCellWater(Map map, IntVec3 cell)
		{
			if (_lakesCanFreezeMapComponentType != null)
			{
				if (_queryCellWaterDelegate == null) //Everything in here should only execute once if the mod is present.
				{
					MapComponent lcfComp; //Set up var.
					if (!_lakesCanFreezeMapComponentCachePerMap.ContainsKey(map.uniqueID)) //If not cached..
						_lakesCanFreezeMapComponentCachePerMap.Add(map.uniqueID, lcfComp = map.GetComponent(_lakesCanFreezeMapComponentType)); //Get and cache.
					else
						lcfComp = _lakesCanFreezeMapComponentCachePerMap[map.uniqueID]; //Get from cache.
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
		public static float? TakeCellIce(Map map, IntVec3 cell)
		{
			if (_lakesCanFreezeMapComponentType != null)
			{
				if (_takeCellIceDelegate == null) //Everything in here should only execute once if the mod is present.
				{
					MapComponent lcfComp; //Set up var.
					if (!_lakesCanFreezeMapComponentCachePerMap.ContainsKey(map.uniqueID)) //If not cached..
						_lakesCanFreezeMapComponentCachePerMap.Add(map.uniqueID, lcfComp = map.GetComponent(_lakesCanFreezeMapComponentType)); //Get and cache.
					else
						lcfComp = _lakesCanFreezeMapComponentCachePerMap[map.uniqueID]; //Get from cache.
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
		public static float? QueryCellIce(Map map, IntVec3 cell)
		{
			if (_lakesCanFreezeMapComponentType != null)
			{
				if (_queryCellIceDelegate == null) //Everything in here should only execute once if the mod is present.
				{
					MapComponent lcfComp; //Set up var.
					if (!_lakesCanFreezeMapComponentCachePerMap.ContainsKey(map.uniqueID)) //If not cached..
						_lakesCanFreezeMapComponentCachePerMap.Add(map.uniqueID, lcfComp = map.GetComponent(_lakesCanFreezeMapComponentType)); //Get and cache.
					else
						lcfComp = _lakesCanFreezeMapComponentCachePerMap[map.uniqueID]; //Get from cache.
					if (lcfComp != null) //It was found.
						_queryCellIceDelegate = (Func<IntVec3, float>)_lakesCanFreezeMapComponentType.GetMethod("QueryCellIce").CreateDelegate(typeof(Func<IntVec3, float>), lcfComp); //Cache it..
					else
						Log.Error("[Soil Relocation] Lakes Can Freeze was detected but MapComponent_LakesCanFreeze could not be retrieved for this map.");
				}
				return _queryCellIceDelegate(cell);
			}
			return null; //Mod not loaded, return null.
		}
	}
}
