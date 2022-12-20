using System;
using Steamworks;

namespace VRage.GameServices
{
	internal class MySteamAchievement : IMyAchievement
	{
		private readonly float m_maxValue;

		private readonly string m_statName;

		private readonly string m_achievementName;

		public bool IsUnlocked
		{
			get
			{
				bool pbAchieved;
				return SteamUserStats.GetAchievement(m_achievementName, out pbAchieved) && pbAchieved;
			}
		}

		public int StatValueInt
		{
			get
			{
				SteamUserStats.GetStat(m_statName, out int pData);
				return pData;
			}
			set
			{
				SteamUserStats.SetStat(m_statName, value);
			}
		}

		public int StatValueConditionBitField
		{
			get
			{
				return StatValueInt;
			}
			set
			{
				StatValueInt = value;
			}
		}

		public float StatValueFloat
		{
			get
			{
				SteamUserStats.GetStat(m_statName, out float pData);
				return pData;
			}
			set
			{
				SteamUserStats.SetStat(m_statName, value);
			}
		}

		public event Action OnStatValueChanged;

		public event Action OnUnlocked;

		public MySteamAchievement(string achievementName, string statName, float maxValue)
		{
			m_maxValue = maxValue;
			m_statName = statName;
			m_achievementName = achievementName;
		}

		public void Unlock()
		{
			SteamUserStats.SetAchievement(m_achievementName);
		}

		public void IndicateProgress(uint value)
		{
			SteamUserStats.IndicateAchievementProgress(m_achievementName, value, (uint)m_maxValue);
			if ((float)value >= m_maxValue)
			{
				OnOnUnlocked();
			}
		}

		protected virtual void OnOnStatValueChanged()
		{
			this.OnStatValueChanged?.Invoke();
		}

		protected virtual void OnOnUnlocked()
		{
			this.OnUnlocked?.Invoke();
		}
	}
}
