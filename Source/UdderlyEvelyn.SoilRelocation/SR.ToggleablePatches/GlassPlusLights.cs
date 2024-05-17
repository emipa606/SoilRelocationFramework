using Verse;

namespace SR.ToggleablePatches;

internal class GlassPlusLights
{
    [ToggleablePatch] internal static readonly ToggleablePatch<RecipeDef> GlassUsesSandPatch =
        new ToggleablePatch<RecipeDef>
        {
            Name = "Glass+Lights Glass Uses Sand",
            Enabled = SoilRelocationSettings.GlassPlusLightsGlassUsesSandEnabled,
            TargetDefName = "MakeGlass",
            TargetModID = "NanoCE.GlassLights",
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
                ingredientCount.SetBaseCount(10f);
                def.ingredients.Add(ingredientCount);
            },
            Unpatch = delegate(ToggleablePatch<RecipeDef> patch, RecipeDef def)
            {
                def.fixedIngredientFilter = (ThingFilter)patch.State;
            }
        };
}