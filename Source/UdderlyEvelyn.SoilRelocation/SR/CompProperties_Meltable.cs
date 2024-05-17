using System.Collections.Generic;
using Verse;

namespace SR;

public class CompProperties_Meltable : CompProperties
{
    public float hoursToMeltStart = 1f;

    public float meltMultiplier = 0.25f;

    public CompProperties_Meltable()
    {
        compClass = typeof(CompMeltable);
    }

    public CompProperties_Meltable(float hoursToMeltStart)
    {
        this.hoursToMeltStart = hoursToMeltStart;
    }

    public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
    {
        foreach (var item in base.ConfigErrors(parentDef))
        {
            yield return item;
        }

        if (parentDef.tickerType != TickerType.Normal && parentDef.tickerType != TickerType.Rare)
        {
            yield return
                $"CompMeltable needs tickerType {TickerType.Rare} or {TickerType.Normal}, has {parentDef.tickerType}";
        }
    }
}