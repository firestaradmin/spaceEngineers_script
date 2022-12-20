using System;
using Sandbox.Engine.Networking;
using Sandbox.ModAPI;
using VRage.GameServices;

namespace Sandbox.Game.SessionComponents
{
	public abstract class MySteamAchievementBase
	{
		protected IMyAchievement m_remoteAchievement;

<<<<<<< HEAD
		/// <summary>
		/// Achievement will stop recieving updates when Achieved.
		/// </summary>
		public bool IsAchieved { get; protected set; }

		/// <summary>
		/// Tells if the Achievement needs to recieve updates.
		/// </summary>
=======
		public bool IsAchieved { get; protected set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public abstract bool NeedsUpdate { get; }

		/// <summary>
		/// Invoked when the achievement is achieved.
		/// Invocation list is cleared afterwards.
		/// </summary>
		public event Action<MySteamAchievementBase> Achieved;

		/// <summary>
		/// Called when session components are getting loaded.
		/// </summary>
		public virtual void SessionLoad()
		{
		}

		/// <summary>
		/// Update called in AfterSimulation.
		/// </summary>
		public virtual void SessionUpdate()
		{
		}

		/// <summary>
		/// Called when session components are getting saved.
		/// </summary>
		public virtual void SessionSave()
		{
		}

		/// <summary>
		/// Called when session gets unloaded.
		/// </summary>
		public virtual void SessionUnload()
		{
		}

		/// <summary>
		/// Called once after the session is loaded and before updates start.
		/// </summary>
		public virtual void SessionBeforeStart()
		{
		}

		protected MySteamAchievementBase()
		{
			(string achievementId, string statTag, float statMaxValue) achievementInfo = GetAchievementInfo();
			string item = achievementInfo.achievementId;
			string item2 = achievementInfo.statTag;
			float item3 = achievementInfo.statMaxValue;
			m_remoteAchievement = MyGameService.GetAchievement(item, item2, item3);
			if (!string.IsNullOrEmpty(item2))
			{
				m_remoteAchievement.OnStatValueChanged += LoadStatValue;
			}
		}

		/// <summary>
		/// Use to notify the achievement state change.
		/// </summary>
		protected void NotifyAchieved()
		{
			m_remoteAchievement.Unlock();
			if (MySteamAchievements.OFFLINE_ACHIEVEMENT_INFO)
			{
				string item = GetAchievementInfo().achievementId;
				MyAPIGateway.Utilities.ShowNotification("Achievement Unlocked: " + item, 10000, "Red");
			}
			IsAchieved = true;
			this.Achieved.InvokeIfNotNull(this);
			this.Achieved = null;
		}

		/// <summary>
		/// Called once when the session gets loaded for the first time.
		/// Always call base.Init()!
		/// </summary>
		public virtual void Init()
		{
			IsAchieved = m_remoteAchievement.IsUnlocked;
			if (!IsAchieved)
			{
				LoadStatValue();
			}
		}

		protected abstract (string achievementId, string statTag, float statMaxValue) GetAchievementInfo();

		protected virtual void LoadStatValue()
		{
		}
	}
}
