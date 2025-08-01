using System;
using System.Text;
using UnityEngine;
using Verse;

namespace SR;

public class CompMeltable : ThingComp
{
    private static readonly int hoursToMeltStart = 1;

    private readonly int meltBufferTicksMax = Mathf.RoundToInt(hoursToMeltStart * 2500f);

    private readonly float meltMultiplier = 0.01f;
    private float? floatHealth = 0f;

    private int meltBufferTicks;

    private float meltRate;

    private float meltRatePerHour;

    private MeltStage Stage => !(parent.AmbientTemperature <= 0f) ? MeltStage.Melting : MeltStage.Solid;

    public override void CompTick()
    {
        Tick(1);
    }

    public override void CompTickRare()
    {
        Tick(250);
    }

    private void Tick(int interval)
    {
        UpdateFloatHealthToRealHealth();
        if (Stage == MeltStage.Melting)
        {
            if (meltBufferTicks < meltBufferTicksMax)
            {
                meltBufferTicks++;
            }

            var num = GenTemperature.RotRateAtTemperature(parent.AmbientTemperature);
            meltRate = num * meltMultiplier / (parent.stackCount / 2f);
            meltRatePerHour = meltRate * 2500f;
            var num2 = meltRate * interval;
            var num3 = num * interval;
            if (parent.MapHeld != null)
            {
                GenTemperature.PushHeat(parent.PositionHeld, parent.MapHeld, 0f - num3);
            }

            if (parent.stackCount > 1 && floatHealth <= num2)
            {
                parent.stackCount--;
                floatHealth = parent.MaxHitPoints;
            }
            else
            {
                floatHealth -= num2;
            }

            UpdateRealHealthToFloatHealth();
        }
        else
        {
            meltBufferTicks = 0;
        }
    }

    private void UpdateFloatHealthToRealHealth()
    {
        if (!floatHealth.HasValue || (float)parent.HitPoints < floatHealth)
        {
            floatHealth = parent.HitPoints;
        }
    }

    private void UpdateRealHealthToFloatHealth()
    {
        if (floatHealth != null)
        {
            parent.HitPoints = Mathf.RoundToInt(floatHealth.Value);
        }

        if (parent.HitPoints <= 0)
        {
            parent.Destroy();
        }
    }

    public override void PreAbsorbStack(Thing otherStack, int count)
    {
        UpdateFloatHealthToRealHealth();
        var t = count / (float)(parent.stackCount + count);
        if (floatHealth != null)
        {
            floatHealth = Mathf.Lerp(floatHealth.Value, parent.MaxHitPoints, t);
        }

        UpdateRealHealthToFloatHealth();
    }

    public override string CompInspectStringExtra()
    {
        var stringBuilder = new StringBuilder();
        switch (Stage)
        {
            case MeltStage.Solid:
                stringBuilder.Append("MeltStateSolid".Translate() + ".");
                break;
            case MeltStage.Melting:
                stringBuilder.Append("MeltStateMelting".Translate() + "." +
                                     (meltRate > 0f
                                         ? $"\nMelt rate: {Math.Round(meltRatePerHour, 2)} / hour"
                                         : ""));
                break;
        }

        return stringBuilder.ToString();
    }
}