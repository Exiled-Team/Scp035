namespace scp035.API
{
	public static class Scp035Data
	{
		public static ReferenceHub GetScp035()
		{
			return EventHandlers.scpPlayer;
		}

		public static void Spawn035(ReferenceHub player)
		{
			EventHandlers.Spawn035(player, null, false);
		}
	}
}
