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
        listing_Standard.Label("SRP.NoRestartWarning".Translate());
        listing_Standard.CheckboxLabeled("SRP.SandbagsUseSand".Translate(),
            ref SoilRelocationSettings.SandbagsUseSandEnabled,
            "SRP.SandbagsUseSandTT".Translate());
        listing_Standard.CheckboxLabeled("SRP.FungalGravelUsesRawFungus".Translate(),
            ref SoilRelocationSettings.FungalGravelUsesRawFungusEnabled,
            "SRP.FungalGravelUsesRawFungusTT".Translate());
        listing_Standard.CheckboxLabeled("SRP.DubsSkylightsGlassUsesSand".Translate(),
            ref SoilRelocationSettings.DubsSkylightsGlassUsesSandEnabled,
            "SRP.DubsSkylightsGlassUsesSandTT".Translate());
        listing_Standard.CheckboxLabeled("SRP.JustGlassGlassUsesSand".Translate(),
            ref SoilRelocationSettings.JustGlassGlassUsesSandEnabled,
            "SRP.JustGlassGlassUsesSandTT".Translate());
        listing_Standard.CheckboxLabeled("SRP.GlassPlusLightsGlassUsesSand".Translate(),
            ref SoilRelocationSettings.GlassPlusLightsGlassUsesSandEnabled,
            "SRP.GlassPlusLightsGlassUsesSandTT".Translate());
        listing_Standard.CheckboxLabeled("SRP.VFEArchitectPackedDirtCostsDirt".Translate(),
            ref SoilRelocationSettings.VFEArchitectPackedDirtCostsDirtEnabled,
            "SRP.VFEArchitectPackedDirtCostsDirtTT".Translate());
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