using Exiled.API.Features;
using HarmonyLib;

namespace scp035.Harmony
{
	[HarmonyPatch(typeof(FootstepSync), nameof(FootstepSync.PlayFootstepSound))]
	class FootstepSyncPatch
	{
		private static int count = 0;

		public static void Prefix(FootstepSync __instance)
		{
			if (!scp035.instance.Config.CorrodeTrail) return;

			Player player = Player.Get(__instance.gameObject);
			count++;
			if (player.Id == EventHandlers.scpPlayer?.Id && count >= scp035.instance.Config.CorrodeTrailInterval)
			{
				player.PlaceCorrosion();
				count = 0;
			}
		}
	}
}
