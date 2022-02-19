using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace SR.ToggleablePatches
{
    internal class JustGlass
    {
        internal static ToggleablePatch<RecipeDef> GlassUsesSandPatch = new ToggleablePatch<RecipeDef>
        {
            Name = "Just Glass Glass Uses Sand",
            Enabled = SoilRelocationSettings.JustGlassGlassUsesSandEnabled,
            TargetDefName = "MakeGlass",
            TargetModID = "spoden.JustGlass",
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
                def.fixedIngredientFilter.SetAllow(ThingCategoryDefOf.StoneChunks, true);
                def.ingredients.Clear();
                var ingredient = new IngredientCount
                {
                    filter = def.fixedIngredientFilter,
                };
                ingredient.SetBaseCount(1);
                def.ingredients.Add(ingredient);
            },
        };
    }
}
