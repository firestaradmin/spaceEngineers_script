using System;
using Sandbox.Game.Localization;
using VRage.Audio;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	/// <summary>
	/// This class represents HUD warning
	/// </summary>
	internal class MyHudWarning : MyHudNotification
	{
		private enum WarningState
		{
			NOT_STARTED,
			STARTED,
			PLAYED
		}

		public int RepeatInterval;

		public Func<bool> CanPlay;

		public Action Played;

		private MyWarningDetectionMethod m_warningDetectionMethod;

		private WarningState m_warningState;

		private bool m_warningDetected;

		private int m_msSinceLastStateChange;

		private int m_soundDelay;

<<<<<<< HEAD
		/// <summary>
		/// Warning's priority
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public int WarningPriority { get; private set; }

		/// <summary>
		/// Creates new instance of HUD warning
		/// </summary>
		/// <param name="detectionMethod">Warning's detection method</param>                
		/// <param name="priority">Warning's priority</param>
		/// <param name="repeatInterval"></param>
		/// <param name="soundDelay"></param>
		/// <param name="disappearTime"></param>
		public MyHudWarning(MyWarningDetectionMethod detectionMethod, int priority, int repeatInterval = 0, int soundDelay = 0, int disappearTime = 0)
			: base(default(MyStringId), disappearTime, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important)
		{
			m_warningDetectionMethod = detectionMethod;
			RepeatInterval = repeatInterval;
			m_soundDelay = soundDelay;
			WarningPriority = priority;
			m_warningDetected = false;
		}

		/// <summary>
		/// Call it in each update
		/// </summary>
		/// <param name="isWarnedHigherPriority">Indicated if warning with greater priority was signalized</param>
		/// <returns>Returns true if warning detected. Else returns false</returns>
		public bool Update(bool isWarnedHigherPriority)
		{
			MyGuiSounds cue = MyGuiSounds.None;
			MyStringId text = MySpaceTexts.Blank;
			m_warningDetected = false;
			if (!isWarnedHigherPriority)
			{
				m_warningDetected = m_warningDetectionMethod(out cue, out text) && MyHudWarnings.EnableWarnings;
			}
			m_msSinceLastStateChange += 16 * MyHudWarnings.FRAMES_BETWEEN_UPDATE;
			if (m_warningDetected)
			{
				switch (m_warningState)
				{
				case WarningState.NOT_STARTED:
					base.Text = text;
					MyHud.Notifications.Add(this);
					m_msSinceLastStateChange = 0;
					m_warningState = WarningState.STARTED;
					break;
				case WarningState.STARTED:
					if (m_msSinceLastStateChange >= m_soundDelay && CanPlay())
					{
						MyHudWarnings.EnqueueSound(cue);
						m_warningState = WarningState.PLAYED;
						Played();
					}
					break;
				case WarningState.PLAYED:
					if (RepeatInterval > 0 && CanPlay())
					{
						MyHud.Notifications.Remove(this);
						MyHud.Notifications.Add(this);
						MyHudWarnings.EnqueueSound(cue);
						Played();
					}
					break;
				}
			}
			else
			{
				MyHud.Notifications.Remove(this);
				MyHudWarnings.RemoveSound(cue);
				m_warningState = WarningState.NOT_STARTED;
			}
			return m_warningDetected;
		}
	}
}
