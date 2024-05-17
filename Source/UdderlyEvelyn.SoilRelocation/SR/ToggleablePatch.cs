using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace SR;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class ToggleablePatch : Attribute
{
    public static readonly bool AutoScan = true;

    protected static bool _performedPatchScan;

    public static readonly Action<string> MessageLoggingMethod = Log.Message;

    public static Action<string> WarningLoggingMethod = Log.Warning;

    public static readonly Action<string> ErrorLoggingMethod = Log.Error;

    public static readonly List<IToggleablePatch> Patches = [];

    public static void ScanForPatches()
    {
        if (_performedPatchScan)
        {
            return;
        }

        var enumerable = Assembly.GetExecutingAssembly().GetTypes().SelectMany(type =>
            type.GetMembers(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
        var typeFromHandle = typeof(IToggleablePatch);
        foreach (var item in enumerable)
        {
            if (!item.HasAttribute<ToggleablePatch>())
            {
                continue;
            }

            if (item is FieldInfo fieldInfo)
            {
                if (fieldInfo.FieldType.GetInterfaces().Contains(typeFromHandle))
                {
                    Patches.Add((IToggleablePatch)fieldInfo.GetValue(null));
                }
                else
                {
                    ErrorLoggingMethod(
                        $"[ToggleablePatch] Field \"{fieldInfo.Name}\" is marked with ToggleablePatch attribute but does not implement IToggleablePatch.");
                }
            }
            else if (item is PropertyInfo propertyInfo)
            {
                if (propertyInfo.PropertyType.GetInterfaces().Contains(typeFromHandle))
                {
                    Patches.Add((IToggleablePatch)propertyInfo.GetValue(null));
                }
                else
                {
                    ErrorLoggingMethod(
                        $"[ToggleablePatch] Property \"{propertyInfo.Name}\" is marked with ToggleablePatch attribute but does not implement IToggleablePatch.");
                }
            }
        }

        _performedPatchScan = true;
    }

    public static void AddPatches(params IToggleablePatch[] patches)
    {
        Patches.AddRange(patches);
    }

    public static void ProcessPatches(string modID, string reason = null)
    {
        if (AutoScan)
        {
            ScanForPatches();
        }

        MessageLoggingMethod(
            $"[ToggleablePatch] Processing {Patches.Count} patches{(reason != null ? " because " + reason : "")} for \"{modID}\"..");
        foreach (var patch in Patches)
        {
            patch.Process();
        }
    }
}

public class ToggleablePatch<T> : IToggleablePatch where T : Def
{
    public List<string> ConflictingModIDs = [];

    protected bool? modInstalled;

    public Action<ToggleablePatch<T>, T> Patch;

    public object State;
    protected T targetDef;

    public string TargetDefName;

    public string TargetModID;

    public Action<ToggleablePatch<T>, T> Unpatch;

    public string TargetDescriptionString =>
        $"{(TargetModID != null ? TargetModID + "." : "")}{TargetDefName} ({typeof(T).FullName})";

    public bool CanPatch
    {
        get
        {
            foreach (var conflictingModID in ConflictingModIDs)
            {
                if (ModLister.GetActiveModWithIdentifier(conflictingModID) == null)
                {
                    continue;
                }

                ToggleablePatch.MessageLoggingMethod(
                    $"[ToggleablePatch] Skipping patch \"{Name}\" because conflicting mod with ID \"{conflictingModID}\" was found.");
                return false;
            }

            if (TargetModID == null)
            {
                return true;
            }

            if (!modInstalled.HasValue)
            {
                modInstalled = ModLister.GetActiveModWithIdentifier(TargetModID) != null;
            }

            return modInstalled.Value;
        }
    }

    public string Name { get; set; }

    public bool Enabled { get; set; }

    public bool Applied { get; protected set; }

    public void Apply()
    {
        if (CanPatch)
        {
            if (!Applied)
            {
                ToggleablePatch.MessageLoggingMethod(
                    $"[ToggleablePatch] {(Name != null ? "Applying patch \"" + Name + "\", patching " : "Patching ")}{TargetDescriptionString}..");
                if (targetDef == null)
                {
                    targetDef = DefDatabase<T>.GetNamed(TargetDefName);
                }

                try
                {
                    Patch(this, targetDef);
                }
                catch (Exception ex)
                {
                    ToggleablePatch.ErrorLoggingMethod(
                        $"[ToggleablePatch] Error {(Name != null ? "applying patch \"" + Name + "\"" : "patching ")}. Most likely you have another mod that already patches {TargetDescriptionString}. Remove that mod or disable this patch in the mod options.\n\n{ex}");
                }

                Applied = true;
            }
            else
            {
                ToggleablePatch.MessageLoggingMethod(
                    $"[ToggleablePatch] Skipping application of patch \"{Name}\" because it is already applied.");
            }
        }
        else
        {
            ToggleablePatch.MessageLoggingMethod(
                $"[ToggleablePatch] Skipping application of patch \"{Name}\" because it cannot be applied.");
        }
    }

    public void Remove()
    {
        if (Applied)
        {
            ToggleablePatch.MessageLoggingMethod(
                $"[ToggleablePatch] {(Name != null ? "Removing patch \"" + Name + "\", unpatching " : "Unpatching ")}{TargetDescriptionString}..");
            if (targetDef == null)
            {
                targetDef = DefDatabase<T>.GetNamed(TargetDefName);
            }

            try
            {
                Unpatch(this, targetDef);
            }
            catch (Exception ex)
            {
                ToggleablePatch.ErrorLoggingMethod(
                    $"[ToggleablePatch] Error {(Name != null ? "removing patch \"" + Name + "\"" : "unpatching ")}. Most likely you have another mod that already patches {TargetDescriptionString}, and it failed to patch in the first place. Remove that mod or disable this patch in the mod options.\n\n{ex}");
            }

            Applied = false;
        }
        else
        {
            ToggleablePatch.MessageLoggingMethod(
                $"[ToggleablePatch] Skipping removal of patch \"{Name}\" because it is not applied.");
        }
    }
}