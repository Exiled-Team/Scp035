using Harmony;

namespace scp035.Harmony
{
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CallCmdRequestHideTag))]
	class HideTagPatch
	{
		private static bool Prefix(CharacterClassManager __instance)
		{
			bool a = Plugin.GetPlayer(__instance.gameObject).queryProcessor.PlayerId == EventHandlers.scpPlayer?.queryProcessor.PlayerId;
			if (a) __instance.TargetConsolePrint(__instance.connectionToClient, "You're not trying to exploit the system by hiding your tag as SCP-035 now, are you?", "green");
			return !a;
		}
	}
}
