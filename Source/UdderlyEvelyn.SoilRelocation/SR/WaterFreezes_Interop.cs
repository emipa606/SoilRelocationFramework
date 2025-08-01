using System;
using Verse;

namespace SR;

public static class WaterFreezes_Interop
{
    private static readonly Type _waterFreezesAPIType = Type.GetType("WF.API, UdderlyEvelyn.WaterFreezes");

    private static Func<Map, IntVec3, float> _takeCellIceDelegate;

    private static Func<Map, IntVec3, float> _queryCellIceDelegate;

    private static Func<Map, IntVec3, float> _queryCellWaterDelegate;

    private static Func<Map, IntVec3, TerrainDef> _queryCellNaturalWaterDelegate;

    private static Func<Map, IntVec3, TerrainDef> _queryCellAllWaterDelegate;

    private static Action<Map, IntVec3> _clearCellNaturalWaterDelegate;

    private static Action<Map, IntVec3> _clearCellWaterDelegate;

    private static Func<TerrainDef, bool> _isThawableIceDelegate;

    public static readonly bool InteropTargetIsPresent = _waterFreezesAPIType != null;

    public static bool IsThawableIce(TerrainDef def)
    {
        if (_waterFreezesAPIType == null)
        {
            return false;
        }

        _isThawableIceDelegate ??= (Func<TerrainDef, bool>)_waterFreezesAPIType.GetMethod("IsThawableIce")
            ?.CreateDelegate(typeof(Func<TerrainDef, bool>));

        return _isThawableIceDelegate != null && _isThawableIceDelegate(def);
    }

    public static TerrainDef QueryCellAllWater(Map map, IntVec3 cell)
    {
        if (_waterFreezesAPIType == null)
        {
            return null;
        }

        _queryCellAllWaterDelegate ??= (Func<Map, IntVec3, TerrainDef>)_waterFreezesAPIType
            .GetMethod("QueryCellAllWater")
            ?.CreateDelegate(typeof(Func<Map, IntVec3, TerrainDef>));

        return _queryCellAllWaterDelegate?.Invoke(map, cell);
    }

    public static TerrainDef QueryCellNaturalWater(Map map, IntVec3 cell)
    {
        if (_waterFreezesAPIType == null)
        {
            return null;
        }

        _queryCellNaturalWaterDelegate ??= (Func<Map, IntVec3, TerrainDef>)_waterFreezesAPIType
            .GetMethod("QueryCellNaturalWater")
            ?.CreateDelegate(typeof(Func<Map, IntVec3, TerrainDef>));

        return _queryCellNaturalWaterDelegate?.Invoke(map, cell);
    }

    public static float? QueryCellWater(Map map, IntVec3 cell)
    {
        if (_waterFreezesAPIType == null)
        {
            return null;
        }

        _queryCellWaterDelegate ??= (Func<Map, IntVec3, float>)_waterFreezesAPIType.GetMethod("QueryCellWater")
            ?.CreateDelegate(typeof(Func<Map, IntVec3, float>));

        return _queryCellWaterDelegate?.Invoke(map, cell);
    }

    public static float? TakeCellIce(Map map, IntVec3 cell)
    {
        if (_waterFreezesAPIType == null)
        {
            return null;
        }

        _takeCellIceDelegate ??= (Func<Map, IntVec3, float>)_waterFreezesAPIType.GetMethod("TakeCellIce")
            ?.CreateDelegate(typeof(Func<Map, IntVec3, float>));

        return _takeCellIceDelegate?.Invoke(map, cell);
    }

    public static float? QueryCellIce(Map map, IntVec3 cell)
    {
        if (_waterFreezesAPIType == null)
        {
            return null;
        }

        _queryCellIceDelegate ??= (Func<Map, IntVec3, float>)_waterFreezesAPIType.GetMethod("QueryCellIce")
            ?.CreateDelegate(typeof(Func<Map, IntVec3, float>));

        return _queryCellIceDelegate?.Invoke(map, cell);
    }

    public static void ClearCellNaturalWater(Map map, IntVec3 cell)
    {
        if (_waterFreezesAPIType == null)
        {
            return;
        }

        _clearCellNaturalWaterDelegate ??= (Action<Map, IntVec3>)_waterFreezesAPIType
            .GetMethod("ClearCellNaturalWater")
            ?.CreateDelegate(typeof(Action<Map, IntVec3>));

        _clearCellNaturalWaterDelegate?.Invoke(map, cell);
    }

    public static void ClearCellWater(Map map, IntVec3 cell)
    {
        if (_waterFreezesAPIType == null)
        {
            return;
        }

        _clearCellWaterDelegate ??= (Action<Map, IntVec3>)_waterFreezesAPIType.GetMethod("ClearCellWater")
            ?.CreateDelegate(typeof(Action<Map, IntVec3>));

        _clearCellWaterDelegate?.Invoke(map, cell);
    }
}