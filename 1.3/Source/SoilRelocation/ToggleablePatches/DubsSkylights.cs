using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace SR.ToggleablePatches
{
    internal class DubsSkylights
    {
        internal static ToggleablePatchGroup GlassUsesSandPatch = new ToggleablePatchGroup
        {
            Name = "Dubs Skylights Glass Uses Sand",
            Enabled = SoilRelocationSettings.DubsSkylightsGlassUsesSandEnabled,
            Patches = new List<IToggleablePatch>
                {
                    new ToggleablePatch<RecipeDef>
                    {
                        TargetDefName = "SmeltGlass",
                        TargetModID = "Dubwise.DubsSkylights",
                        ConflictingModIDs = new()
                        {
                            "Maaxar.DubsSkylights.glasslights.patch",
                        },
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
                        ConflictingModIDs = new()
                        {
                            "Maaxar.DubsSkylights.glasslights.patch",
                        },
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
    }
}
