using System;
using VRage.Game;

namespace Sandbox.Game.World
{
	public class MyWorldInfo
	{
		public string SessionName;

		public string SessionPath;

		public string Description;

		public DateTime LastSaveTime;

		public ulong StorageSize;

		public WorkshopId[] WorkshopIds;

		public string Briefing;

		public bool ScenarioEditMode;

		public bool IsCorrupted;

		public bool IsExperimental;

		public bool HasPlanets;

		public bool IsCampaign;

<<<<<<< HEAD
		/// <summary>
		/// Directory of the save, used in DS GUI. It can be different from the SessionPath
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public string SaveDirectory { get; set; }
	}
}
