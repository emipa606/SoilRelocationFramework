using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SR;

[StaticConstructorOnStartup]
public static class SoilRelocation
{
    private static readonly string Version;

    static SoilRelocation()
    {
        var version = Assembly.GetAssembly(typeof(SoilRelocation)).GetName().Version;
        Version = $"{version.Major}.{version.Minor}.{version.Build}";
        ToggleablePatch.ProcessPatches("UdderlyEvelyn.SoilRelocation");
        var harmony = new Harmony("UdderlyEvelyn.SoilRelocation");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        if (WaterFreezes_Interop.InteropTargetIsPresent)
        {
            harmony.Patch(AccessTools.Method(typeof(Frame), nameof(Frame.CompleteConstruction)),
                new HarmonyMethod(typeof(Frame_CompleteConstruction), nameof(Frame_CompleteConstruction.Prefix)));
        }
    }

    public static void Log(string message, ErrorLevel errorLevel = ErrorLevel.Message, int errorOnceKey = 0,
        bool ignoreStopLoggingLimit = false)
    {
        if (ignoreStopLoggingLimit)
        {
            Verse.Log.ResetMessageCount();
        }

        var text = $"[Soil Relocation {Version}] {message}";
        switch (errorLevel)
        {
            case ErrorLevel.Message:
                Verse.Log.Message(text);
                break;
            case ErrorLevel.Warning:
                Verse.Log.Warning(text);
                break;
            case ErrorLevel.Error:
                Verse.Log.Error(text);
                break;
            case ErrorLevel.ErrorOnce:
                Verse.Log.ErrorOnce(text, errorOnceKey);
                break;
        }
    }
}