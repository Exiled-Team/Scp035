using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;
using Exiled.API.Features;

namespace scp035.Harmony
{
	[HarmonyPatch(typeof(PlayableScps.Scp096), nameof(PlayableScps.Scp096.OnDamage))]
	static class Scp096Patches
	{
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			var newInstructions = instructions.ToList();

			var returnLabel = newInstructions.First(i => i.opcode == OpCodes.Brfalse_S).operand;
			var continueLabel = generator.DefineLabel();
			var firstOffset = newInstructions.FindIndex(i => i.opcode == OpCodes.Ldloc_0) + 1;

			var player = generator.DeclareLocal(typeof(Player));
			var scp035 = generator.DeclareLocal(typeof(Player));

			newInstructions.InsertRange(firstOffset, new CodeInstruction[] 
			{
				new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(UnityEngine.GameObject) })),
				new CodeInstruction(OpCodes.Stloc_1, player.LocalIndex),
				new CodeInstruction(OpCodes.Call, Method(typeof(API.Scp035Data), nameof(API.Scp035Data.GetScp035))),
				new CodeInstruction(OpCodes.Stloc_2, scp035.LocalIndex),
				new CodeInstruction(OpCodes.Ldloc_0)
			});

			var secondOffset = newInstructions.FindIndex(i => i.opcode == OpCodes.Ldnull) + 3;

			newInstructions.InsertRange(secondOffset, new CodeInstruction[]
			{
				new CodeInstruction(OpCodes.Ldloc_2),
				new CodeInstruction(OpCodes.Brfalse_S, continueLabel),
				new CodeInstruction(OpCodes.Ldloc_1),
				new CodeInstruction(OpCodes.Brfalse_S, continueLabel),
				new CodeInstruction(OpCodes.Ldloc_2),
				new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Player), nameof(Player.Id))),
				new CodeInstruction(OpCodes.Ldloc_1),
				new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Player), nameof(Player.Id))),
				new CodeInstruction(OpCodes.Beq_S, returnLabel)
			});

			newInstructions[secondOffset + 9].labels.Add(continueLabel);

			foreach (var code in newInstructions)
				yield return code;
		}
	}
}
