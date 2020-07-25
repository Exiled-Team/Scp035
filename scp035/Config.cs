using Exiled.API.Interfaces;
using System.Collections.Generic;

namespace scp035
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		public List<int> PossibleItems { get; set; } = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 19, 20, 21, 23, 24, 25, 26, 27, 30, 33, 34 };

		public int Health { get; set; } = 300;
		public int InfectedItemCount { get; set; } = 1;
		public int CorrodeDamage { get; set; } = 5;
		public int CorrodeTrailInterval { get; set; } = 5;
		public int CorrodeHostAmount { get; set; } = 5;

		public bool ScpFriendlyFire { get; set; } = false;
		public bool TutorialFriendlyFire { get; set; } = false;
		public bool WinWithTutorial { get; set; } = false;
		public bool CorrodePlayers { get; set; } = true;
		public bool CorrodeLifeSteal { get; set; } = true;
		public bool CorrodeTrail { get; set; } = false;
		public bool CorrodeHost { get; set; } = false;
		public bool CanUseMedicalItems { get; set; } = true;
		public bool CanHealBeyondHostHp { get; set; } = true;

		public float CorrodeDistance { get; set; } = 1.5f;
		public float RotateInterval { get; set; } = 120f;
		public float CorrodeInterval { get; set; } = 1f;
		public float CorrodeHostInterval { get; set; } = 6f;
	}
}
