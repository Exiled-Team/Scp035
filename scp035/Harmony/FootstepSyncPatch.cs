using Harmony;

namespace scp035.Harmony
{
	[HarmonyPatch(typeof(FootstepSync), nameof(FootstepSync.PlayFootstepSound))]
	class FootstepSyncPatch
	{
		private static int count = 0;

		public static void Prefix(FootstepSync __instance)
		{
			if (!Configs.corrodeTrail) return;

			ReferenceHub player = Plugin.GetPlayer(__instance.gameObject);
			count++;
			if (player.queryProcessor.PlayerId == EventHandlers.scpPlayer?.queryProcessor.PlayerId && count >= Configs.corrodeTrailInterval)
			{
				player.PlaceCorrosion();
				count = 0;
			}
		}
	}
}
