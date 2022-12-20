using System;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Systems;
using Sandbox.Game.Multiplayer;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.Components
{
	[MyComponentType(typeof(MyTimerComponent))]
	[MyComponentBuilder(typeof(MyObjectBuilder_TimerComponent), true)]
	public class MyTimerComponent : MyEntityComponentBase
	{
		private class Sandbox_Game_Components_MyTimerComponent_003C_003EActor : IActivator, IActivator<MyTimerComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyTimerComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTimerComponent CreateInstance()
			{
				return new MyTimerComponent();
			}

			MyTimerComponent IActivator<MyTimerComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private bool m_forceTrigger;

		public bool TimerEnabled { get; private set; } = true;


		public bool RemoveEntityOnTimer { get; set; }

		public bool Repeat { get; set; }

<<<<<<< HEAD
=======
		[Obsolete("Use FramesFromLastTrigger")]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public float TimeToEvent { get; set; }

		public MyTimerTypes TimerType { get; private set; }

		public Action<MyEntityComponentContainer> EventToTrigger { get; set; }

<<<<<<< HEAD
		/// <summary>
		/// Tick in minutes
		/// </summary>
=======
		[Obsolete("Use TimerTickInFrames")]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public float TimerTick { get; private set; }

		public uint FramesFromLastTrigger { get; set; }

		public uint TimerTickInFrames { get; private set; }

<<<<<<< HEAD
		/// <summary>
		/// Get or sets is session update enabled. Usefull if you don't want this timer to be updated by Timer Component System (session component).
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool IsSessionUpdateEnabled { get; set; } = true;


		public override string ComponentTypeDebugString => "Timer";

		public void SetRemoveEntityTimer(float timeMin)
		{
			RemoveEntityOnTimer = true;
			SetTimer(timeMin, GetRemoveEntityOnTimerEvent());
		}

		public void SetTimer(float timeMin, Action<MyEntityComponentContainer> triggerEvent, bool start = true, bool repeat = false)
		{
			uint timeTickInFrames = (uint)(timeMin * 60f * 60f);
			SetTimer(timeTickInFrames, triggerEvent, start, repeat);
		}

		public void SetTimer(uint timeTickInFrames, Action<MyEntityComponentContainer> triggerEvent, bool start = true, bool repeat = false)
		{
			TimerTickInFrames = timeTickInFrames;
			Repeat = repeat;
			EventToTrigger = triggerEvent;
			TimerEnabled = start;
			m_forceTrigger = start;
		}

		public void ChangeTimerTick(uint timeTickInFrames)
		{
			TimerTickInFrames = timeTickInFrames;
		}

		public void SetType(MyTimerTypes type)
		{
			bool num = MyTimerComponentSystem.Static.IsRegisteredAny(this);
			if (num)
			{
				MyTimerComponentSystem.Static.Unregister(this);
			}
			TimerType = type;
			if (num)
			{
				MyTimerComponentSystem.Static.Register(this);
			}
		}

		public void ClearEvent()
		{
			EventToTrigger = null;
		}

		public void Pause()
		{
			TimerEnabled = false;
		}

		public void Resume(bool forceTrigger = false)
		{
			TimerEnabled = true;
			m_forceTrigger = forceTrigger;
		}

		public void Update(bool forceUpdate = false)
		{
			if (!TimerEnabled && !forceUpdate)
			{
				return;
			}
			switch (TimerType)
			{
			case MyTimerTypes.Frame10:
				FramesFromLastTrigger += 10u;
				break;
			case MyTimerTypes.Frame100:
				FramesFromLastTrigger += 100u;
				break;
			}
			if (m_forceTrigger || forceUpdate)
			{
				m_forceTrigger = false;
				FramesFromLastTrigger = TimerTickInFrames;
			}
			if (FramesFromLastTrigger >= TimerTickInFrames)
			{
				EventToTrigger.InvokeIfNotNull(base.Container);
				if (Repeat)
				{
					FramesFromLastTrigger = 0u;
				}
				else
				{
					TimerEnabled = false;
				}
			}
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			if (TimerTick != 0f)
			{
				TimerTickInFrames = (uint)(TimerTick * 3600f);
			}
			if (TimeToEvent != 0f)
			{
				FramesFromLastTrigger = (uint)((TimerTick - TimeToEvent) * 3600f);
			}
			MyTimerComponentSystem.Static.Register(this);
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			if (MyTimerComponentSystem.Static != null)
			{
				MyTimerComponentSystem.Static.Unregister(this);
			}
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_TimerComponent obj = MyComponentFactory.CreateObjectBuilder(this) as MyObjectBuilder_TimerComponent;
			obj.Repeat = Repeat;
			obj.TimeToEvent = 0f;
			obj.SetTimeMinutes = 0f;
			obj.TimerEnabled = TimerEnabled;
			obj.RemoveEntityOnTimer = RemoveEntityOnTimer;
			obj.TimerType = TimerType;
			obj.FramesFromLastTrigger = FramesFromLastTrigger;
			obj.TimerTickInFrames = TimerTickInFrames;
			obj.IsSessionUpdateEnabled = IsSessionUpdateEnabled;
			return obj;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase baseBuilder)
		{
			MyObjectBuilder_TimerComponent myObjectBuilder_TimerComponent = baseBuilder as MyObjectBuilder_TimerComponent;
			Repeat = myObjectBuilder_TimerComponent.Repeat;
			TimeToEvent = myObjectBuilder_TimerComponent.TimeToEvent;
			TimerTick = myObjectBuilder_TimerComponent.SetTimeMinutes;
			TimerEnabled = myObjectBuilder_TimerComponent.TimerEnabled;
			RemoveEntityOnTimer = myObjectBuilder_TimerComponent.RemoveEntityOnTimer;
			TimerType = myObjectBuilder_TimerComponent.TimerType;
			FramesFromLastTrigger = myObjectBuilder_TimerComponent.FramesFromLastTrigger;
			TimerTickInFrames = myObjectBuilder_TimerComponent.TimerTickInFrames;
			IsSessionUpdateEnabled = myObjectBuilder_TimerComponent.IsSessionUpdateEnabled;
			if (RemoveEntityOnTimer && Sync.IsServer)
			{
				EventToTrigger = GetRemoveEntityOnTimerEvent();
			}
		}

		public override bool IsSerialized()
		{
			return true;
		}

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
			MyTimerComponentDefinition myTimerComponentDefinition = definition as MyTimerComponentDefinition;
			if (myTimerComponentDefinition != null)
			{
				TimerEnabled = myTimerComponentDefinition.TimeToRemoveMin > 0f;
				TimerTickInFrames = (uint)(myTimerComponentDefinition.TimeToRemoveMin * 3600f);
				FramesFromLastTrigger = 0u;
				RemoveEntityOnTimer = myTimerComponentDefinition.TimeToRemoveMin > 0f;
				if (RemoveEntityOnTimer && Sync.IsServer)
				{
					EventToTrigger = GetRemoveEntityOnTimerEvent();
				}
			}
		}

		private static Action<MyEntityComponentContainer> GetRemoveEntityOnTimerEvent()
		{
			return delegate(MyEntityComponentContainer container)
			{
				MyLog.Default.Info($"MyTimerComponent removed entity '{container.Entity.Name}:{container.Entity.DisplayName}' with entity id '{container.Entity.EntityId}'");
				if (!container.Entity.MarkedForClose)
				{
					container.Entity.Close();
				}
			};
		}
	}
}
