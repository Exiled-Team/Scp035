namespace scp035.API
{
	public static class Scp035Data
	{
		public static ReferenceHub GetScp035()
		{
			return EventHandlers.scpPlayer;
		}

		public static void Make035(ReferenceHub player)
		{
			EventHandlers.InfectPlayer(player);
		}
	}
}
