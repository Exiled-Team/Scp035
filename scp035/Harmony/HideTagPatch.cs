using Harmony;

namespace scp035.Harmony
{
	[HarmonyPatch(typeof(CharacterClassManager), "CmdRequestHideTag")]
	class HideTagPatch
	{
		private static bool Prefix(CharacterClassManager __instance)
		{
			return EventHandler.hInstance.HandleHideTagHook(__instance);
		}
	}
}
