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
        public static ToggleablePatch<BuildableDef> SandbagsUseSandPatch = new ToggleablePatch<BuildableDef>
        {
            Name = "Sandbags Use Sand",
            Enabled = SoilRelocationSettings.SandbagsUseSandEnabled,
            TargetDefName = "Sandbags",
            Patch = def =>
            {
                if (def.costList == null)
                    def.costList = new List<ThingDefCountClass>();
                def.costList.Add(new ThingDefCountClass { count = 5, thingDef = SoilDefs.SR_Sand }); //Add an additional cost of 5 sand.
            },
            Unpatch = def =>
            {
                var costListItems = def.costList.Where(tdcc => tdcc.thingDef == SoilDefs.SR_Sand); //Try to find our patch..
                if (costListItems.Any()) //If we find it..
                    def.CostList.Remove(costListItems.First()); //Yeet!
            },
        };
        public static ToggleablePatchGroup DubsSkylightsGlassUsesSandPatch = new ToggleablePatchGroup
        {
            Name = "Dubs Skylights Glass Uses Sand",
            Enabled = SoilRelocationSettings.DubsSkylightsGlassUsesSandEnabled,
            Patches = new List<IToggleablePatch>
                {
                    new ToggleablePatch<RecipeDef>
                    {
                        TargetDefName = "SmeltGlass",
                        TargetModID = "Dubwise.DubsSkylights",
                        Patch = def =>
                        {
                            def.fixedIngredientFilter = new ThingFilter();
                            def.fixedIngredientFilter.SetAllow(SoilDefs.SR_Sand, true);
                            def.ingredients.Clear();
                            var ingredient = new IngredientCount
                            {
                                filter = def.fixedIngredientFilter,
                            };
                            ingredient.SetBaseCount(10);
                            def.ingredients.Add(ingredient);
                        },
                        Unpatch = def =>
                        {
                            def.fixedIngredientFilter = new ThingFilter();
                            def.fixedIngredientFilter.SetAllow(ThingDefOf.Steel, true);
                            def.ingredients.Clear();
                            var ingredient = new IngredientCount
                            {
                                filter = def.fixedIngredientFilter,
                            };
                            ingredient.SetBaseCount(4);
                            def.ingredients.Add(ingredient);
                        },
                    },
                    new ToggleablePatch<RecipeDef>
                    {
                        TargetDefName = "SmeltGlass4x",
                        TargetModID = "Dubwise.DubsSkylights",
                        Patch = def =>
                        {
                            def.fixedIngredientFilter = new ThingFilter();
                            def.fixedIngredientFilter.SetAllow(SoilDefs.SR_Sand, true);
                            def.ingredients.Clear();
                            var ingredient = new IngredientCount
                            {
                                filter = def.fixedIngredientFilter,
                            };
                            ingredient.SetBaseCount(40);
                            def.ingredients.Add(ingredient);
                        },
                        Unpatch = def =>
                        {
                            def.fixedIngredientFilter = new ThingFilter();
                            def.fixedIngredientFilter.SetAllow(ThingDefOf.Steel, true);
                            def.ingredients.Clear();
                            var ingredient = new IngredientCount
                            {
                                filter = def.fixedIngredientFilter,
                            };
                            ingredient.SetBaseCount(16);
                            def.ingredients.Add(ingredient);
                        },
                    }
                }
        };
        //Really needs a PlaceWorker attached to it, no time for this release.
        public static ToggleablePatch<BuildableDef> VFEArchitectPackedDirtCostsDirt = new ToggleablePatch<BuildableDef>
        {
            Name = "VFE Architect Packed Dirt Costs Dirt",
            Enabled = SoilRelocationSettings.VFEArchitectPackedDirtCostsDirtEnabled,
            TargetDefName = "VFEArch_PlayerPackedDirt",
            TargetModID = "VanillaExpanded.VFEArchitect",
            Patch = def =>
            {
                if (def.costList == null)
                    def.costList = new List<ThingDefCountClass>();
                def.costList.Add(new ThingDefCountClass { count = 1, thingDef = SoilDefs.SR_Soil }); //Add an additional cost of 1 soil just to remove the exploit of free dirt.
            },
            Unpatch = def =>
            {
                var costListItems = def.costList.Where(tdcc => tdcc.thingDef == SoilDefs.SR_Soil); //Try to find our patch..
                if (costListItems.Any()) //If we find it..
                    def.CostList.Remove(costListItems.First()); //Yeet!
            },
        };

        static SoilRelocation()
        {
            Log.Message("[Soil Relocation] Initializing..");
            Patches.Add(SandbagsUseSandPatch);
            Patches.Add(DubsSkylightsGlassUsesSandPatch);
            Patches.Add(VFEArchitectPackedDirtCostsDirt);
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
            return "SoilRelocation";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("The below settings take effect immediately, no restart required.");
            listingStandard.CheckboxLabeled("Sandbags Use Sand", ref SoilRelocationSettings.SandbagsUseSandEnabled, "Patch vanilla sandbags so they use sand in addition to cloth.");
            listingStandard.CheckboxLabeled("Dubs Skylights Glass Uses Sand", ref SoilRelocationSettings.DubsSkylightsGlassUsesSandEnabled, "Patch Dubs Skylights glass recipes so that they use sand instead of steel.");
            listingStandard.CheckboxLabeled("VFE Architect Packed Dirt Costs Dirt", ref SoilRelocationSettings.VFEArchitectPackedDirtCostsDirtEnabled, "Patches VFE Architect's packed dirt recipe to cost one soil to avoid an exploit that gives you free soil.");
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override void WriteSettings()
        {
            SoilRelocation.SandbagsUseSandPatch.Enabled = SoilRelocationSettings.SandbagsUseSandEnabled;
            SoilRelocation.DubsSkylightsGlassUsesSandPatch.Enabled = SoilRelocationSettings.DubsSkylightsGlassUsesSandEnabled;
            SoilRelocation.VFEArchitectPackedDirtCostsDirt.Enabled = SoilRelocationSettings.VFEArchitectPackedDirtCostsDirtEnabled;
            SoilRelocation.ProcessPatches("settings were updated");
                
            base.WriteSettings();
        }
    }
}