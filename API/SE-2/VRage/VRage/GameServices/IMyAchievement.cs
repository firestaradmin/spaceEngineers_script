using System;

namespace VRage.GameServices
{
	public interface IMyAchievement
	{
		bool IsUnlocked { get; }

		int StatValueInt { get; set; }

		float StatValueFloat { get; set; }

		int StatValueConditionBitField { get; set; }

		event Action OnStatValueChanged;

		event Action OnUnlocked;

		void Unlock();

		void IndicateProgress(uint value);
	}
}
