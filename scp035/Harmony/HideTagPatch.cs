using Exiled.API.Features;
using HarmonyLib;

namespace scp035.Harmony
{
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CallCmdRequestHideTag))]
	class HideTagPatch
	{
		private static bool Prefix(CharacterClassManager __instance)
		{
			bool a = Player.Get(__instance.gameObject).Id == EventHandlers.scpPlayer?.Id;
			if (a) __instance.TargetConsolePrint(__instance.connectionToClient, "You're not trying to exploit the system by hiding your tag as SCP-035 now, are you?", "green");
			return !a;
		}
	}
}
