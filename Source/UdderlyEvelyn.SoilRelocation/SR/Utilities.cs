using System.Collections.Generic;
using Verse;

namespace SR;

public static class Utilities
{
    public static void DropThing(Map map, IntVec3 cell, ThingDef thingDef, int count,
        ThingPlaceMode tpm = ThingPlaceMode.Direct)
    {
        var thing = ThingMaker.MakeThing(thingDef);
        thing.stackCount = count;
        GenPlace.TryPlaceThing(thing, cell, map, tpm);
    }

    public static void DropThings(Map map, IntVec3 cell, List<ThingDefCountClass> thingCountList,
        ThingPlaceMode tpm = ThingPlaceMode.Near)
    {
        foreach (var thingCount in thingCountList)
        {
            DropThing(map, cell, thingCount.thingDef, thingCount.count);
        }
    }

    public static void DropThings(Map map, IntVec3 cell, List<ThingDefCountClass> thingCountList,
        int downwardCountVariance, int upwardCountVariance, ThingPlaceMode tpm = ThingPlaceMode.Near)
    {
        foreach (var thingCount in thingCountList)
        {
            DropThing(map, cell, thingCount.thingDef,
                Rand.Range(thingCount.count - downwardCountVariance, thingCount.count + upwardCountVariance), tpm);
        }
    }
}