using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;

namespace SR;

[HarmonyPatch(typeof(Frame), nameof(Frame.GetInspectString))]
internal static class Frame_GetInspectString
{
    private static readonly MethodInfo _harmonyPatchSharedData_GetWaterFillAdjustedCostListForFrame =
        AccessTools.Method(typeof(HarmonyPatchSharedData),
            nameof(HarmonyPatchSharedData.GetWaterFillAdjustedCostListForFrame));

    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var fired = false;
        var instructionList = instructions.ToList();
        var i = 0;
        while (i < instructionList.Count)
        {
            var instruction = instructionList[i];
            yield return instruction;
            int num;
            if (!fired && instruction.opcode == OpCodes.Ldc_I4_1)
            {
                num = i + 1;
                i = num;
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Call,
                    _harmonyPatchSharedData_GetWaterFillAdjustedCostListForFrame);
                fired = true;
            }

            num = i + 1;
            i = num;
        }
    }
}