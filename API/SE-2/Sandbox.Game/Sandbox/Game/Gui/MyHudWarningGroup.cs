using System.Collections.Generic;

namespace Sandbox.Game.Gui
{
	/// <summary>
	/// This class represents HUD warning group. Only 1 warning can be signalized, from this group.
	/// </summary>
	internal class MyHudWarningGroup
	{
		private List<MyHudWarning> m_hudWarnings;

		private bool m_canBeTurnedOff;

		private int m_msSinceLastCuePlayed;

		private int m_highestWarnedPriority = int.MaxValue;

		/// <summary>
		/// Creates new instance of HUD warning group
		/// </summary>
		/// <param name="hudWarnings"></param>
		/// <param name="canBeTurnedOff"></param>
		public MyHudWarningGroup(List<MyHudWarning> hudWarnings, bool canBeTurnedOff)
		{
			m_hudWarnings = new List<MyHudWarning>(hudWarnings);
			SortByPriority();
			m_canBeTurnedOff = canBeTurnedOff;
			InitLastCuePlayed();
			foreach (MyHudWarning warning in hudWarnings)
			{
				warning.CanPlay = () => m_highestWarnedPriority > warning.WarningPriority || (m_msSinceLastCuePlayed > warning.RepeatInterval && m_highestWarnedPriority == warning.WarningPriority);
				warning.Played = delegate
				{
					m_msSinceLastCuePlayed = 0;
					m_highestWarnedPriority = warning.WarningPriority;
				};
			}
		}

		private void InitLastCuePlayed()
		{
			foreach (MyHudWarning hudWarning in m_hudWarnings)
			{
				if (hudWarning.RepeatInterval > m_msSinceLastCuePlayed)
				{
					m_msSinceLastCuePlayed = hudWarning.RepeatInterval;
				}
			}
		}

		/// <summary>
		/// Call it in each update.
		/// </summary>
		public void Update()
		{
			if (!MySandboxGame.IsGameReady)
			{
				return;
			}
			m_msSinceLastCuePlayed += 16 * MyHudWarnings.FRAMES_BETWEEN_UPDATE;
			bool flag = false;
			foreach (MyHudWarning hudWarning in m_hudWarnings)
			{
				if (hudWarning.Update(flag))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				m_highestWarnedPriority = int.MaxValue;
			}
		}

		/// <summary>
		/// Adds new HUD warning to this group
		/// </summary>
		/// <param name="hudWarning">HUD warning to add</param>
		public void Add(MyHudWarning hudWarning)
		{
			m_hudWarnings.Add(hudWarning);
			SortByPriority();
			InitLastCuePlayed();
			hudWarning.CanPlay = () => m_highestWarnedPriority > hudWarning.WarningPriority || (m_msSinceLastCuePlayed > hudWarning.RepeatInterval && m_highestWarnedPriority == hudWarning.WarningPriority);
			hudWarning.Played = delegate
			{
				m_msSinceLastCuePlayed = 0;
				m_highestWarnedPriority = hudWarning.WarningPriority;
			};
		}

		/// <summary>
		/// Removes HUD warning from this group
		/// </summary>
		/// <param name="hudWarning">HUD warning to remove</param>
		public void Remove(MyHudWarning hudWarning)
		{
			m_hudWarnings.Remove(hudWarning);
		}

		/// <summary>
		/// Removes all HUD warnings from this group
		/// </summary>        
		public void Clear()
		{
			m_hudWarnings.Clear();
		}

		private void SortByPriority()
		{
			m_hudWarnings.Sort((MyHudWarning x, MyHudWarning y) => x.WarningPriority.CompareTo(y.WarningPriority));
		}
	}
}
