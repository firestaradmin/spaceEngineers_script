using System;
using System.Threading;

namespace VRage.Library.Utils
{
	public class WaitForTargetFrameRate
	{
<<<<<<< HEAD
		public static bool ENABLE_TIMING_HOTFIX;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private long m_targetTicks;

		public bool EnableMaxSpeed;

<<<<<<< HEAD
		private const bool ENABLE_UPDATE_WAIT = true;
=======
		private const bool EnableUpdateWait = true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private readonly MyGameTimer m_timer;

		private readonly float m_targetFrequency;

		private float m_delta;

		public long TickPerFrame
		{
			get
			{
				int num = (int)Math.Round((float)MyGameTimer.Frequency / m_targetFrequency);
				return num;
			}
		}

		public WaitForTargetFrameRate(MyGameTimer timer, float targetFrequency = 60f)
		{
			m_timer = timer;
			m_targetFrequency = targetFrequency;
		}

		public void SetNextFrameDelayDelta(float delta)
		{
			m_delta = delta;
		}

		public void Wait()
		{
			m_timer.AddElapsed(MyTimeSpan.FromMilliseconds(0f - m_delta));
			long elapsedTicks = m_timer.ElapsedTicks;
			m_targetTicks += TickPerFrame;
			if (elapsedTicks > m_targetTicks + TickPerFrame * 5 || EnableMaxSpeed)
			{
				m_targetTicks = elapsedTicks;
			}
			else
			{
<<<<<<< HEAD
				int num = (int)(MyTimeSpan.FromTicks(m_targetTicks - elapsedTicks).Milliseconds - (double)(ENABLE_TIMING_HOTFIX ? 2f : 0.1f));
				if (num > 0)
				{
					Thread.Sleep(num);
=======
				int num = (int)(MyTimeSpan.FromTicks(m_targetTicks - elapsedTicks).Milliseconds - 0.1);
				if (num > 0)
				{
					Thread.get_CurrentThread().Join(num);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				if (m_targetTicks < m_timer.ElapsedTicks + TickPerFrame + TickPerFrame / 4)
				{
					while (m_timer.ElapsedTicks < m_targetTicks)
					{
					}
				}
				else
				{
					m_targetTicks = m_timer.ElapsedTicks;
				}
			}
			m_delta = 0f;
		}
	}
}
