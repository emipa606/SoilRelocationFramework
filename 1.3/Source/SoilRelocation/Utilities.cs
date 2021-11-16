using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace SR
{
    public static class Utilities
    {
		public static void DropThing(Map map, IntVec3 cell, ThingDef thingDef, int count)
		{
			ThingOwner<Thing> thingOwner = new ThingOwner<Thing>();
			Thing thing = ThingMaker.MakeThing(thingDef);
			thing.stackCount = count;
			thingOwner.TryAdd(thing, true);
			Thing item;
			thingOwner.TryDrop(thing, cell, map, ThingPlaceMode.Direct, out item);
		}

		public static void DropThingFromOwner(ThingOwner thingOwner, Map map, IntVec3 cell, ThingDef thingDef, int count, ThingPlaceMode tpm = ThingPlaceMode.Direct)
		{
			Thing thing = ThingMaker.MakeThing(thingDef);
			thing.stackCount = count;
			thingOwner.TryAdd(thing, true);
			Thing item;
			thingOwner.TryDrop(thing, cell, map, tpm, out item);
		}

		public static void DropThings(Map map, IntVec3 cell, List<ThingDefCountClass> thingCountList)
        {
			ThingOwner<Thing> thingOwner = new ThingOwner<Thing>();
			foreach (var thingCount in thingCountList)
				DropThingFromOwner(thingOwner, map, cell, thingCount.thingDef, thingCount.count, ThingPlaceMode.Near);
        }


		public static void DropThings(Map map, IntVec3 cell, List<ThingDefCountClass> thingCountList, int downwardCountVariance, int upwardCountVariance)
		{
			ThingOwner<Thing> thingOwner = new ThingOwner<Thing>();
			foreach (var thingCount in thingCountList)
				DropThingFromOwner(thingOwner, map, cell, thingCount.thingDef, Rand.Range(thingCount.count - downwardCountVariance, thingCount.count + upwardCountVariance), ThingPlaceMode.Near);
		}
	}
}
