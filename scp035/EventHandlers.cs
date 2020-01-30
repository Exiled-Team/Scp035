using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXILED;

namespace scp035
{
	class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		private bool isRoundStarted = false;

		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}

		public void OnRoundStart()
		{
			if (!Configs.enabled) return;

			isRoundStarted = true;
		}

		public void OnRoundEnd()
		{

		}

		public void OnPickupItem(ref PickupItemEvent ev)
		{
			
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{

		}

		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{

		}

		public void OnPocketDimensionEnter(PocketDimEnterEvent ev)
		{

		}

		public void OnCheckRoundEnd(ref CheckRoundEndEvent ev)
		{

		}

		public void OnCheckEscape(ref CheckEscapeEvent ev)
		{

		}

		public void OnSetClass(SetClassEvent ev)
		{

		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{

		}

		public void OnContain106(Scp106ContainEvent ev)
		{
			
		}

		public void OnInsertTablet(ref GeneratorInsertTabletEvent ev)
		{

		}

		public void OnPocketDimensionDie(PocketDimDeathEvent ev)
		{

		}
	}
}
