using Verse;

namespace SR.ToggleablePatches;

internal class DubsSkylights
{
    [ToggleablePatch] internal static readonly ToggleablePatchGroup GlassUsesSandPatch = new ToggleablePatchGroup
    {
        Name = "Dubs Skylights Glass Uses Sand",
        Enabled = SoilRelocationSettings.DubsSkylightsGlassUsesSandEnabled,
        Patches =
        [
            new ToggleablePatch<RecipeDef>
            {
                Name = "Dubs Skylights Glass Uses Sand",
                TargetDefName = "SmeltGlass",
                TargetModID = "Dubwise.DubsSkylights",
                ConflictingModIDs = ["Maaxar.DubsSkylights.glasslights.patch"],
                Patch = delegate(ToggleablePatch<RecipeDef> patch, RecipeDef def)
                {
                    patch.State = def.fixedIngredientFilter;
                    def.fixedIngredientFilter = new ThingFilter();
                    def.fixedIngredientFilter.SetAllow(SoilDefs.SR_Sand, true);
                    def.ingredients.Clear();
                    var ingredientCount2 = new IngredientCount
                    {
                        filter = def.fixedIngredientFilter
                    };
                    ingredientCount2.SetBaseCount(10f);
                    def.ingredients.Add(ingredientCount2);
                },
                Unpatch = delegate(ToggleablePatch<RecipeDef> patch, RecipeDef def)
                {
                    def.fixedIngredientFilter = (ThingFilter)patch.State;
                }
            },

            new ToggleablePatch<RecipeDef>
            {
                Name = "Dubs Skylights Glass 4x Uses Sand 4x",
                TargetDefName = "SmeltGlass4x",
                TargetModID = "Dubwise.DubsSkylights",
                ConflictingModIDs = ["Maaxar.DubsSkylights.glasslights.patch"],
                Patch = delegate(ToggleablePatch<RecipeDef> patch, RecipeDef def)
                {
                    patch.State = def.fixedIngredientFilter;
                    def.fixedIngredientFilter = new ThingFilter();
                    def.fixedIngredientFilter.SetAllow(SoilDefs.SR_Sand, true);
                    def.ingredients.Clear();
                    var ingredientCount = new IngredientCount
                    {
                        filter = def.fixedIngredientFilter
                    };
                    ingredientCount.SetBaseCount(40f);
                    def.ingredients.Add(ingredientCount);
                },
                Unpatch = delegate(ToggleablePatch<RecipeDef> patch, RecipeDef def)
                {
                    def.fixedIngredientFilter = (ThingFilter)patch.State;
                }
            }
        ]
    };
}