using SR.ToggleablePatches;
using UnityEngine;
using Verse;

namespace SR;

public class SoilRelocationMod : Mod
{
    public SoilRelocationSettings Settings;

    public SoilRelocationMod(ModContentPack content)
        : base(content)
    {
        Settings = GetSettings<SoilRelocationSettings>();
    }

    public override string SettingsCategory()
    {
        return "Soil Relocation";
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(inRect);
        listing_Standard.Label("The below settings take effect immediately, no restart required.");
        listing_Standard.CheckboxLabeled("Sandbags Use Sand", ref SoilRelocationSettings.SandbagsUseSandEnabled,
            "Patch vanilla sandbags so they use sand in addition to cloth.");
        listing_Standard.CheckboxLabeled("Fungal Gravel Uses Raw Fungus",
            ref SoilRelocationSettings.FungalGravelUsesRawFungusEnabled,
            "Patch vanilla Fungal Gravel to cost a bit of Raw Fungus to avoid it being exploitable/unbalanced.");
        listing_Standard.CheckboxLabeled("Dubs Skylights Glass Uses Sand",
            ref SoilRelocationSettings.DubsSkylightsGlassUsesSandEnabled,
            "Patch Dubs Skylights glass recipes so that they use sand instead of steel.");
        listing_Standard.CheckboxLabeled("Just Glass Glass Uses Sand",
            ref SoilRelocationSettings.JustGlassGlassUsesSandEnabled,
            "Patch the Just Glass glass recipe so that it uses sand instead of a stone chunk.");
        listing_Standard.CheckboxLabeled("Glass+Lights Glass Uses Sand",
            ref SoilRelocationSettings.GlassPlusLightsGlassUsesSandEnabled,
            "Patch the Glass+Lights glass recipe so that it uses sand instead of a stone chunk.");
        listing_Standard.CheckboxLabeled("VFE Architect Packed Dirt Costs Dirt",
            ref SoilRelocationSettings.VFEArchitectPackedDirtCostsDirtEnabled,
            "Patches VFE Architect's packed dirt recipe to cost one soil to avoid an exploit that gives you free soil.");
        listing_Standard.End();
        base.DoSettingsWindowContents(inRect);
    }

    public override void WriteSettings()
    {
        Vanilla.SandbagsUseSandPatch.Enabled = SoilRelocationSettings.SandbagsUseSandEnabled;
        DubsSkylights.GlassUsesSandPatch.Enabled = SoilRelocationSettings.DubsSkylightsGlassUsesSandEnabled;
        JustGlass.GlassUsesSandPatch.Enabled = SoilRelocationSettings.JustGlassGlassUsesSandEnabled;
        GlassPlusLights.GlassUsesSandPatch.Enabled = SoilRelocationSettings.GlassPlusLightsGlassUsesSandEnabled;
        VFEArchitect.PackedDirtCostsDirt.Enabled = SoilRelocationSettings.VFEArchitectPackedDirtCostsDirtEnabled;
        ToggleablePatch.ProcessPatches("UdderlyEvelyn.SoilRelocation", "settings were updated");
        base.WriteSettings();
    }
}