using System.Collections.Generic;
using Sandbox.Game.Components;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;

namespace Sandbox.Game.EntityComponents.Systems
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyTimerComponentSystem : MySessionComponentBase
	{
		private object m_lockObject = new object();

		private const int UPDATE_FRAME_100 = 100;

		private const int UPDATE_FRAME_10 = 10;

		public static MyTimerComponentSystem Static;

		private List<MyTimerComponent> m_timerComponents10 = new List<MyTimerComponent>();

		private List<MyTimerComponent> m_timerComponents100 = new List<MyTimerComponent>();

		private int m_frameCounter10;

		private int m_frameCounter100;

		public override void LoadData()
		{
			base.LoadData();
			Static = this;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Static = null;
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (++m_frameCounter10 % 10 == 0)
			{
				m_frameCounter10 = 0;
				UpdateTimerComponents10();
			}
			if (++m_frameCounter100 % 100 == 0)
			{
				m_frameCounter100 = 0;
				UpdateTimerComponents100();
			}
		}

		private void UpdateTimerComponents10()
		{
			lock (m_lockObject)
			{
				foreach (MyTimerComponent item in m_timerComponents10)
				{
					if (item != null && item.IsSessionUpdateEnabled)
					{
						item.Update();
					}
				}
			}
		}

		internal bool IsRegisteredAny(MyTimerComponent timer)
		{
			if (!timer.IsSessionUpdateEnabled)
			{
				return false;
			}
			if (m_timerComponents10.Contains(timer))
			{
				return true;
			}
			if (m_timerComponents100.Contains(timer))
			{
				return true;
			}
			return false;
		}

		private void UpdateTimerComponents100()
		{
			lock (m_lockObject)
			{
				foreach (MyTimerComponent item in m_timerComponents100)
				{
					if (item != null && item.IsSessionUpdateEnabled)
					{
						item.Update();
					}
				}
			}
		}

		public void Register(MyTimerComponent timerComponent)
		{
			if (!timerComponent.IsSessionUpdateEnabled)
			{
				return;
			}
			lock (m_lockObject)
			{
				switch (timerComponent.TimerType)
				{
				case MyTimerTypes.Frame10:
					m_timerComponents10.Add(timerComponent);
					break;
				case MyTimerTypes.Frame100:
					m_timerComponents100.Add(timerComponent);
					break;
				}
			}
		}

		public void Unregister(MyTimerComponent timerComponent)
		{
			lock (m_lockObject)
			{
				switch (timerComponent.TimerType)
				{
				case MyTimerTypes.Frame10:
					m_timerComponents10.Remove(timerComponent);
					break;
				case MyTimerTypes.Frame100:
					m_timerComponents100.Remove(timerComponent);
					break;
				}
			}
		}
	}
}
