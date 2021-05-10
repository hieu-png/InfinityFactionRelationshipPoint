using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using RimWorld.Planet;
namespace NowanoInfinityRelationShip
{
    [StaticConstructorOnStartup]
    public static class InfinityRelation

    {
        static InfinityRelation()
        {
            var harmony = new Harmony("com.nowano.ininity.relationship");
            Log.Message("Mod loaded");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(Faction), nameof(Faction.TryAffectGoodwillWith))]
        static class RelationShipTranspilerPatch
        {
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {


                foreach (var inst in instructions)
                {
                    // if (inst.opcode == OpCodes.Ldc_I4 && (int)inst.operand == -100) inst.operand = -50;
                    //Credit:Wiri
                    if (inst.LoadsConstant(-100)) {
                        inst.opcode = OpCodes.Ldc_I4;
                        inst.operand = Int32.MinValue; 
                    }
                    if (inst.LoadsConstant(100)) {
                        inst.opcode = OpCodes.Ldc_I4;
                        inst.operand = Int32.MaxValue; 
                    }                   
                    yield return inst;
                }                
            }
        }
        [HarmonyPatch(typeof(FactionGiftUtility), "PostProcessedGoodwillChange")]
        static class OfferGiftsNoLimimt
        {
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var inst in instructions)
                {
                    if (inst.opcode == OpCodes.Ldc_R4 && (float)inst.operand == 200)
                    {
                        
                        inst.operand = 10000000.0f;
                    }
                    yield return inst;
                }

            }
        }
    }
}
