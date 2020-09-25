using Exiled.API.Features;
using HarmonyLib;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scp035.Harmony
{
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CallCmdRegisterEscape))]
	class EscapePatch
	{
		public static bool Prefix(CharacterClassManager __instance) => Player.Get(((NetworkBehaviour)__instance).gameObject).Id != EventHandlers.scpPlayer?.Id;
	}
}
