using System;
using HarmonyLib;
using System.Collections.Generic;
using Vintagestory.GameContent;
using System.Reflection.Emit;
using Vintagestory.API.Common;
using System.Linq;

namespace CraftingAdditions.Patches
{
    [HarmonyPatch]
    public class AnvilRecipeSelectorGUI : ModSystem
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(GuiDialogBlockEntityRecipeSelector), "SetupDialog")]
        public static IEnumerable<CodeInstruction> TranspileSetupDialog(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            for (int i = 0; i < codes.Count - 1; i++)
            {
                if (codes[i].opcode == OpCodes.Dup && codes[i + 1].opcode == OpCodes.Ldc_I4_7 &&
                    codes[i + 2].opcode == OpCodes.Call && codes[i + 3].opcode == OpCodes.Stloc_0)
                {
                    codes[i + 1].opcode = OpCodes.Ldc_I4;
                    codes[i + 1].operand = 14;
                }
            }

            return codes.AsEnumerable();
        }
    }
}