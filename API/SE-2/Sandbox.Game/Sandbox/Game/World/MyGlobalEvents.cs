using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using VRage.Game;
using VRage.Game.Components;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MyGlobalEvents : MySessionComponentBase
	{
		private static SortedSet<MyGlobalEventBase> m_globalEvents = new SortedSet<MyGlobalEventBase>();

		private int m_elapsedTimeInMilliseconds;

		private int m_previousTime;

		private static readonly int GLOBAL_EVENT_UPDATE_RATIO_IN_MS = 2000;

		private static Predicate<MyGlobalEventBase> m_removalPredicate = RemovalPredicate;

		private static MyDefinitionId m_defIdToRemove;

		public static bool EventsEmpty => m_globalEvents.get_Count() == 0;

		public override void LoadData()
		{
			m_globalEvents.Clear();
			base.LoadData();
		}

		protected override void UnloadData()
		{
			m_globalEvents.Clear();
			base.UnloadData();
		}

		public void Init(MyObjectBuilder_GlobalEvents objectBuilder)
		{
			foreach (MyObjectBuilder_GlobalEventBase @event in objectBuilder.Events)
			{
				m_globalEvents.Add(MyGlobalEventFactory.CreateEvent(@event));
			}
		}

		public static MyObjectBuilder_GlobalEvents GetGlobalEventsObjectBuilder()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			MyObjectBuilder_GlobalEvents myObjectBuilder_GlobalEvents = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_GlobalEvents>();
			Enumerator<MyGlobalEventBase> enumerator = m_globalEvents.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGlobalEventBase current = enumerator.get_Current();
					myObjectBuilder_GlobalEvents.Events.Add(current.GetObjectBuilder());
				}
				return myObjectBuilder_GlobalEvents;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public override void BeforeStart()
		{
			m_previousTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		public override void UpdateBeforeSimulation()
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			if (!Sync.IsServer)
			{
				return;
			}
			m_elapsedTimeInMilliseconds += MySandboxGame.TotalGamePlayTimeInMilliseconds - m_previousTime;
			m_previousTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (m_elapsedTimeInMilliseconds < GLOBAL_EVENT_UPDATE_RATIO_IN_MS)
			{
				return;
			}
			Enumerator<MyGlobalEventBase> enumerator = m_globalEvents.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGlobalEventBase current = enumerator.get_Current();
					current.SetActivationTime(TimeSpan.FromTicks(current.ActivationTime.Ticks - (long)m_elapsedTimeInMilliseconds * 10000L));
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			MyGlobalEventBase myGlobalEventBase = Enumerable.FirstOrDefault<MyGlobalEventBase>((IEnumerable<MyGlobalEventBase>)m_globalEvents);
			while (myGlobalEventBase != null && myGlobalEventBase.IsInPast)
			{
				m_globalEvents.Remove(myGlobalEventBase);
				if (myGlobalEventBase.Enabled)
				{
					StartGlobalEvent(myGlobalEventBase);
				}
				if (myGlobalEventBase.IsPeriodic)
				{
					if (myGlobalEventBase.RemoveAfterHandlerExit)
					{
						m_globalEvents.Remove(myGlobalEventBase);
					}
					else if (!m_globalEvents.Contains(myGlobalEventBase))
					{
						myGlobalEventBase.RecalculateActivationTime();
						AddGlobalEvent(myGlobalEventBase);
					}
				}
				myGlobalEventBase = Enumerable.FirstOrDefault<MyGlobalEventBase>((IEnumerable<MyGlobalEventBase>)m_globalEvents);
			}
			m_elapsedTimeInMilliseconds = 0;
		}

		public override void Draw()
		{
<<<<<<< HEAD
=======
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || !MyDebugDrawSettings.DEBUG_DRAW_EVENTS)
			{
				return;
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, 500f), "Upcoming events:", Color.White, 1f);
			StringBuilder stringBuilder = new StringBuilder();
			float num = 530f;
<<<<<<< HEAD
			foreach (MyGlobalEventBase globalEvent in m_globalEvents)
			{
				int num2 = (int)globalEvent.ActivationTime.TotalHours;
				int minutes = globalEvent.ActivationTime.Minutes;
				int seconds = globalEvent.ActivationTime.Seconds;
				stringBuilder.Clear();
				stringBuilder.AppendFormat("{0}:{1:D2}:{2:D2}", num2, minutes, seconds);
				stringBuilder.AppendFormat(" {0}: {1}", globalEvent.Enabled ? "ENABLED" : "--OFF--", globalEvent.Definition.DisplayNameString ?? globalEvent.Definition.Id.SubtypeName);
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, num), stringBuilder.ToString(), globalEvent.Enabled ? Color.White : Color.Gray, 0.8f);
				num += 20f;
=======
			Enumerator<MyGlobalEventBase> enumerator = m_globalEvents.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGlobalEventBase current = enumerator.get_Current();
					int num2 = (int)current.ActivationTime.TotalHours;
					int minutes = current.ActivationTime.Minutes;
					int seconds = current.ActivationTime.Seconds;
					stringBuilder.Clear();
					stringBuilder.AppendFormat("{0}:{1:D2}:{2:D2}", num2, minutes, seconds);
					stringBuilder.AppendFormat(" {0}: {1}", current.Enabled ? "ENABLED" : "--OFF--", current.Definition.DisplayNameString ?? current.Definition.Id.SubtypeName);
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, num), stringBuilder.ToString(), current.Enabled ? Color.White : Color.Gray, 0.8f);
					num += 20f;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public static MyGlobalEventBase GetEventById(MyDefinitionId defId)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyGlobalEventBase> enumerator = m_globalEvents.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGlobalEventBase current = enumerator.get_Current();
					if (current.Definition.Id == defId)
					{
						return current;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return null;
		}

		private static bool RemovalPredicate(MyGlobalEventBase globalEvent)
		{
			return globalEvent.Definition.Id == m_defIdToRemove;
		}

		public static void RemoveEventsById(MyDefinitionId defIdToRemove)
		{
			m_defIdToRemove = defIdToRemove;
			m_globalEvents.RemoveWhere(m_removalPredicate);
		}

		public static void AddGlobalEvent(MyGlobalEventBase globalEvent)
		{
			m_globalEvents.Add(globalEvent);
		}

		public static void RemoveGlobalEvent(MyGlobalEventBase globalEvent)
		{
			m_globalEvents.Remove(globalEvent);
		}

		public static void RescheduleEvent(MyGlobalEventBase globalEvent, TimeSpan time)
		{
			m_globalEvents.Remove(globalEvent);
			globalEvent.SetActivationTime(time);
			m_globalEvents.Add(globalEvent);
		}

		public static void LoadEvents(MyObjectBuilder_GlobalEvents eventsBuilder)
		{
			if (eventsBuilder == null)
<<<<<<< HEAD
			{
				return;
			}
			foreach (MyObjectBuilder_GlobalEventBase @event in eventsBuilder.Events)
			{
=======
			{
				return;
			}
			foreach (MyObjectBuilder_GlobalEventBase @event in eventsBuilder.Events)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyGlobalEventBase myGlobalEventBase = MyGlobalEventFactory.CreateEvent(@event);
				if (myGlobalEventBase != null && myGlobalEventBase.IsHandlerValid)
				{
					m_globalEvents.Add(myGlobalEventBase);
				}
			}
		}

		private void StartGlobalEvent(MyGlobalEventBase globalEvent)
		{
			AddGlobalEventToEventLog(globalEvent);
			if (globalEvent.IsHandlerValid)
			{
				globalEvent.Action.Invoke(this, new object[1] { globalEvent });
			}
		}

		private void AddGlobalEventToEventLog(MyGlobalEventBase globalEvent)
		{
			MySandboxGame.Log.WriteLine("MyGlobalEvents.StartGlobalEvent: " + globalEvent.Definition.Id.ToString());
		}

		public static void EnableEvents()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyGlobalEventBase> enumerator = m_globalEvents.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Enabled = true;
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		internal static void DisableEvents()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyGlobalEventBase> enumerator = m_globalEvents.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Enabled = false;
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}
	}
}
