using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HarmonyLib.AccessTools;
using Exiled.API.Features;
using System.Reflection.Emit;
using System.Reflection;
using scp035.API;

namespace scp035.Harmony
{
    [HarmonyPatch(typeof(MicroHID), nameof(MicroHID.DealDamage))]
    static class MicroHIDPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = instructions.ToList();
            var attackerPlayer = generator.DeclareLocal(typeof(Player));
            var scp035Player = generator.DeclareLocal(typeof(Player));
            var targetPlayer = generator.DeclareLocal(typeof(Player));
            var localNum = generator.DeclareLocal(typeof(float));
            var localNum2 = generator.DeclareLocal(typeof(int));
            var ldlocLabel = generator.DefineLabel();
            var ldlocsLabel = generator.DefineLabel();
            var ldlocasLabel = generator.DefineLabel();
            var offset = newInstructions.FindIndex(c => c.opcode == OpCodes.Call && (MethodInfo)c.operand == Method(typeof(MicroHID), nameof(MicroHID.TargetSendHitmarker))) + 1;

			newInstructions[newInstructions.FindIndex(c => c.opcode == OpCodes.Call && 
            (MethodInfo)c.operand == Method(typeof(ReferenceHub), "op_Inequality")) + 1].operand = ldlocLabel;

			newInstructions[newInstructions.FindIndex(c => c.opcode == OpCodes.Call && 
            (MethodInfo)c.operand == Method(typeof(UnityEngine.Object), "op_Inequality")) + 1].operand = ldlocLabel;

			newInstructions[newInstructions.FindIndex(c => c.opcode == OpCodes.Callvirt && 
            (MethodInfo)c.operand == Method(typeof(WeaponManager), nameof(WeaponManager.GetShootPermission),
				new[] { typeof(CharacterClassManager), typeof(bool) })) + 1].operand = ldlocLabel;

            newInstructions[1].opcode = OpCodes.Brtrue_S;

            newInstructions.InsertRange(offset, new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Br, ldlocasLabel),
                new CodeInstruction(OpCodes.Ldloc_0).WithLabels(ldlocLabel),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Stloc_S, attackerPlayer.LocalIndex),
                new CodeInstruction(OpCodes.Call, Method(typeof(Scp035Data), nameof(Scp035Data.GetScp035))),
                new CodeInstruction(OpCodes.Stloc_S, scp035Player.LocalIndex),
                new CodeInstruction(OpCodes.Ldloc_S, 7),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Stloc_S, targetPlayer.LocalIndex),
                new CodeInstruction(OpCodes.Ldloc_S, scp035Player.LocalIndex),
                new CodeInstruction(OpCodes.Ldloc_S, targetPlayer.LocalIndex),
                new CodeInstruction(OpCodes.Beq_S, ldlocsLabel),
                new CodeInstruction(OpCodes.Ldloc_S, scp035Player.LocalIndex),
                new CodeInstruction(OpCodes.Ldloc_S, attackerPlayer.LocalIndex),
                new CodeInstruction(OpCodes.Bne_Un, ldlocasLabel),
                new CodeInstruction(OpCodes.Ldloc_S, targetPlayer.LocalIndex).WithLabels(ldlocsLabel),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Player), nameof(Player.Side))),
                new CodeInstruction(OpCodes.Ldloc_S, scp035Player.LocalIndex),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Player), nameof(Player.Side))),
                new CodeInstruction(OpCodes.Bne_Un_S, ldlocasLabel),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(MicroHID), nameof(MicroHID.maxDamageVariationPercent))),
                new CodeInstruction(OpCodes.Neg),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(MicroHID), nameof(MicroHID.maxDamageVariationPercent))),
                new CodeInstruction(OpCodes.Call, Method(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range), new[] { typeof(float), typeof(float) })),
                new CodeInstruction(OpCodes.Ldc_R4, 100f),
                new CodeInstruction(OpCodes.Div),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(MicroHID), nameof(MicroHID.damagePerSecond))),
                new CodeInstruction(OpCodes.Mul),
                new CodeInstruction(OpCodes.Stloc_S, localNum.LocalIndex),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(MicroHID), nameof(MicroHID.damagePerSecond))),
                new CodeInstruction(OpCodes.Ldloc_S, localNum.LocalIndex),
                new CodeInstruction(OpCodes.Add),
                new CodeInstruction(OpCodes.Ldc_R4, 0.2f),
                new CodeInstruction(OpCodes.Mul),
                new CodeInstruction(OpCodes.Call, Method(typeof(UnityEngine.Mathf), nameof(UnityEngine.Mathf.RoundToInt), new[] { typeof(float) })),
                new CodeInstruction(OpCodes.Stloc_S, localNum2.LocalIndex),
                new CodeInstruction(OpCodes.Ldloc_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(ReferenceHub), nameof(ReferenceHub.playerStats))),
                new CodeInstruction(OpCodes.Ldloc_S, localNum2.LocalIndex),
                new CodeInstruction(OpCodes.Conv_R4),
                new CodeInstruction(OpCodes.Ldloc_0),
                new CodeInstruction(OpCodes.Call, Method(typeof(Misc), nameof(Misc.LoggedNameFromRefHub), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Ldsfld, Field(typeof(DamageTypes), nameof(DamageTypes.MicroHid))),
                new CodeInstruction(OpCodes.Ldloc_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(ReferenceHub), nameof(ReferenceHub.queryProcessor))),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(RemoteAdmin.QueryProcessor), nameof(RemoteAdmin.QueryProcessor.PlayerId))),
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(PlayerStats.HitInfo))[0]),
                new CodeInstruction(OpCodes.Ldloc_S, 7),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(UnityEngine.Component), nameof(UnityEngine.Component.gameObject))),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Callvirt, Method(typeof(PlayerStats), nameof(PlayerStats.HurtPlayer), 
                    new[] { typeof(PlayerStats.HitInfo), typeof(UnityEngine.GameObject), typeof(bool), typeof(bool) })),
                 new CodeInstruction(OpCodes.Pop),
                 new CodeInstruction(OpCodes.Ldarg_0),
                 new CodeInstruction(OpCodes.Ldc_I4_0),
                 new CodeInstruction(OpCodes.Call, Method(typeof(MicroHID), nameof(MicroHID.TargetSendHitmarker)))
            });

            newInstructions[newInstructions.FindLastIndex(c => c.opcode == OpCodes.Pop) + 4].labels.Add(ldlocasLabel);

            foreach (var code in newInstructions)
                yield return code;
        }
    }
}
