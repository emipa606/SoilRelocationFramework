using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using RimWorld.Planet;
using UnityEngine;

namespace SR
{
    [StaticConstructorOnStartup]
    public static class SoilRelocation
    {
        public static List<IToggleablePatch> Patches = new List<IToggleablePatch>();

        static SoilRelocation()
        {
            Log.Message("[Soil Relocation] Initializing..");
            Patches.Add(ToggleablePatches.Vanilla.SandbagsUseSandPatch);
            Patches.Add(ToggleablePatches.DubsSkylights.GlassUsesSandPatch);
            Patches.Add(ToggleablePatches.JustGlass.GlassUsesSandPatch);
            Patches.Add(ToggleablePatches.GlassPlusLights.GlassUsesSandPatch);
            Patches.Add(ToggleablePatches.VFEArchitect.PackedDirtCostsDirt);
            ProcessPatches();
        }

        /// <summary>
        /// Process the patches stored in SoilRelocation.Patches.
        /// </summary>
        /// <param name="reason">the reason to process them, optional, shown in logging</param>
        public static void ProcessPatches(string reason = null)
        {
            Log.Message("[Soil Relocation] Processing patches" + (reason != null ? " (" + reason + ")" : "") + "..");
            foreach (var patch in Patches)
                patch.Process();
        }
    }

    public class SoilRelocationMod : Mod
    {
        public SoilRelocationSettings Settings;

        public SoilRelocationMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<SoilRelocationSettings>();
        }

        public override string SettingsCategory()
        {
            return "Soil Relocation";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("The below settings take effect immediately, no restart required.");
            listingStandard.CheckboxLabeled("Sandbags Use Sand", ref SoilRelocationSettings.SandbagsUseSandEnabled, "Patch vanilla sandbags so they use sand in addition to cloth.");
            listingStandard.CheckboxLabeled("Dubs Skylights Glass Uses Sand", ref SoilRelocationSettings.DubsSkylightsGlassUsesSandEnabled, "Patch Dubs Skylights glass recipes so that they use sand instead of steel.");
            listingStandard.CheckboxLabeled("Just Glass Glass Uses Sand", ref SoilRelocationSettings.JustGlassGlassUsesSandEnabled, "Patch the Just Glass glass recipe so that it uses sand instead of a stone chunk.");
            listingStandard.CheckboxLabeled("Glass+Lights Glass Uses Sand", ref SoilRelocationSettings.GlassPlusLightsGlassUsesSandEnabled, "Patch the Glass+Lights glass recipe so that it uses sand instead of a stone chunk.");
            listingStandard.CheckboxLabeled("VFE Architect Packed Dirt Costs Dirt", ref SoilRelocationSettings.VFEArchitectPackedDirtCostsDirtEnabled, "Patches VFE Architect's packed dirt recipe to cost one soil to avoid an exploit that gives you free soil.");
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override void WriteSettings()
        {
            ToggleablePatches.Vanilla.SandbagsUseSandPatch.Enabled = SoilRelocationSettings.SandbagsUseSandEnabled;
            ToggleablePatches.DubsSkylights.GlassUsesSandPatch.Enabled = SoilRelocationSettings.DubsSkylightsGlassUsesSandEnabled;
            ToggleablePatches.JustGlass.GlassUsesSandPatch.Enabled = SoilRelocationSettings.JustGlassGlassUsesSandEnabled;
            ToggleablePatches.GlassPlusLights.GlassUsesSandPatch.Enabled = SoilRelocationSettings.GlassPlusLightsGlassUsesSandEnabled;
            ToggleablePatches.VFEArchitect.PackedDirtCostsDirt.Enabled = SoilRelocationSettings.VFEArchitectPackedDirtCostsDirtEnabled;
            SoilRelocation.ProcessPatches("settings were updated");
                
            base.WriteSettings();
        }
    }
}