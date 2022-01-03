namespace Scp035.Patches
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using Exiled.API.Features;
    using Exiled.CustomRoles;
    using Exiled.CustomRoles.API.Features;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    [HarmonyPatch(typeof(Recontainer079), nameof(Recontainer079.OnClassChanged))]
    internal class Recontain079Fix
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);
            const int offset = 3;
            int index = newInstructions.FindIndex(i => i.opcode == OpCodes.Ldnull) + offset;
            Label continueLabel = generator.DefineLabel();
            Label retLabel = generator.DefineLabel();

            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldsfld, Field(typeof(Plugin), nameof(Plugin.Instance))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Scp035.Config), nameof(Scp035.Config.Scp035RoleConfig))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Scp035Role), nameof(Scp035Role.Id))),
                new CodeInstruction(OpCodes.Call, Method(typeof(CustomRole), nameof(CustomRole.Get), new[] { typeof(int) })),
                new CodeInstruction(OpCodes.Ldloca_S, 1),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(KeyValuePair<GameObject, ReferenceHub>), nameof(KeyValuePair<GameObject, ReferenceHub>.Value))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Callvirt, Method(typeof(CustomRole), nameof(CustomRole.Check))),
                new CodeInstruction(OpCodes.Brtrue, continueLabel),
                new CodeInstruction(OpCodes.Leave_S, retLabel),
                new CodeInstruction(OpCodes.Nop).WithLabels(continueLabel),
            });

            newInstructions[newInstructions.Count - 1].labels.Add(retLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];
            
            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}