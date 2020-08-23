using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace scp035
{
	public class Config : IConfig
	{
		[Description("Determines if the plugin is enabled or not.")]
		public bool IsEnabled { get; set; } = true;

		[Description("The items that are possible to spawn as SCP-035.")]
		public List<int> PossibleItems { get; set; } = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 19, 20, 21, 23, 24, 25, 26, 27, 30, 33, 34 };

		[Description("The health fo SCP-035")]
		public int Health { get; set; } = 300;
		[Description("The amount of infected items on the map at any given time.")]
		public int InfectedItemCount { get; set; } = 1;
		[Description("The amount of damage done to players nearby SCP-035 every corrosion interval.")]
		public int CorrodeDamage { get; set; } = 5;
		[Description("The amount of footsteps before corrosion is placed beneath SCP-035.")]
		public int CorrodeTrailInterval { get; set; } = 5;
		[Description("The amount of damage done to SCP-035 every corrode host interval.")]
		public int CorrodeHostAmount { get; set; } = 5;

		[Description("Determines if SCP-035 and SCPs can hurt each other.")]
		public bool ScpFriendlyFire { get; set; } = false;
		[Description("Determines if SCP-035 and Tutorials can hurt each other.")]
		public bool TutorialFriendlyFire { get; set; } = false;
		[Description("Determines if SCP-035 and Tutorial should win together.")]
		public bool WinWithTutorial { get; set; } = false;
		[Description("Determines if SCP-035 should corrode nearby players.")]
		public bool CorrodePlayers { get; set; } = true;
		[Description("Determines if SCP-035 should heal the corrosion damage he deals to other players.")]
		public bool CorrodeLifeSteal { get; set; } = true;
		[Description("Determines if SCP-035 should leave behind a trail of corrosion where he walks.")]
		public bool CorrodeTrail { get; set; } = false;
		[Description("Determines if SCP-035 will corrode his host body.")]
		public bool CorrodeHost { get; set; } = false;
		[Description("Determines if SCP-035 can use medical items.")]
		public bool CanUseMedicalItems { get; set; } = true;
		[Description("Determines if SCP-035 can use medical items to heal past the max health of his host body up to his maximum health.")]
		public bool CanHealBeyondHostHp { get; set; } = true;

		[Description("The distance SCP-035 can corrode other players from.")]
		public float CorrodeDistance { get; set; } = 1.5f;
		[Description("The internal in which infected items refresh.")]
		public float RotateInterval { get; set; } = 120f;
		[Description("The interval in which SCP-035 corrodes other players")]
		public float CorrodeInterval { get; set; } = 1f;
		[Description("The interval in which SCP-035 corrodes his host body.")]
		public float CorrodeHostInterval { get; set; } = 6f;
	}
}
