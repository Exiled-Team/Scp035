using Exiled.API.Features;

namespace scp035.API
{
	public static class Scp035Data
	{
		public static Player GetScp035()
		{
			return EventHandlers.scpPlayer;
		}

		public static void Spawn035(Player player)
		{
			EventHandlers.Spawn035(player, null, false);
		}
	}
}
