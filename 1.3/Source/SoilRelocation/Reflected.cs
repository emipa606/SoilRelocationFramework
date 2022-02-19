using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;
using RimWorld;
using System.Reflection;

namespace SR
{
    internal static class ReflectionCache
    {
        internal static FieldInfo ThingDefCount_count = AccessTools.Field(typeof(ThingDefCount), "count");
    }
}