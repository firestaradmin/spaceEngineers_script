using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Components
{
	/// <summary>
	/// TODO: This should change in future. Cestmir should already know how to change this to some kind of dispatcher that will inform components
	/// Until then, this is now used to inform MyEntityDurabilityComponent if present in the container about changes
	/// </summary>
	public static class MyEntityContainerEventExtensions
	{
		/// <summary>
		/// Base class for passing parameters, derive for it to pass different params, keep the names consistent ie. EntityEventType = Hit =&gt; params should be type of HitParams
		/// </summary>
		public class EntityEventParams
		{
		}

		/// <summary>
		/// Params to be passed with ControlAcquiredEvent..
		/// </summary>
		public class ControlAcquiredParams : EntityEventParams
		{
			public MyEntity Owner;

			public ControlAcquiredParams(MyEntity owner)
			{
				Owner = owner;
			}
		}

		/// <summary>
		/// Params to be passed with ControlAcquiredEvent..
		/// </summary>
		public class ControlReleasedParams : EntityEventParams
		{
			public MyEntity Owner;

			public ControlReleasedParams(MyEntity owner)
			{
				Owner = owner;
			}
		}

		/// <summary>
		/// This class object is passed as argument with ModelChanged entity event
		/// </summary>
		public class ModelChangedParams : EntityEventParams
		{
			public Vector3 Size;

			public float Mass;

			public float Volume;

			public string Model;

			public string DisplayName;

			public string[] Icons;

			public ModelChangedParams(string model, Vector3 size, float mass, float volume, string displayName, string[] icons)
			{
				Model = model;
				Size = size;
				Mass = mass;
				Volume = volume;
				DisplayName = displayName;
				Icons = icons;
			}
		}

		/// <summary>
		/// This class is used to inform about changes in inventory
		/// </summary>
		public class InventoryChangedParams : EntityEventParams
		{
			public uint ItemId;

			public float Amount;

			public MyInventoryBase Inventory;

			public InventoryChangedParams(uint itemId, MyInventoryBase inventory, float amount)
			{
				ItemId = itemId;
				Inventory = inventory;
				Amount = amount;
			}
		}

		/// <summary>
		/// Params to pass hitted entity
		/// </summary>
		public class HitParams : EntityEventParams
		{
			public MyStringHash HitEntity;

			public MyStringHash HitAction;

			public HitParams(MyStringHash hitAction, MyStringHash hitEntity)
			{
				HitEntity = hitEntity;
				HitAction = hitAction;
			}
		}

		/// <summary>
		/// Handler to be called on event
		/// </summary>
		/// <param name="eventParams">These params can be also another type derived from this for different event types </param>
		public delegate void EntityEventHandler(EntityEventParams eventParams);

		/// <summary>
		/// This holds basically the delegate to be invoked but also a component for easier deregistration..
		/// </summary>
		private class RegisteredComponent
		{
			public MyComponentBase Component;

			public EntityEventHandler Handler;

			public RegisteredComponent(MyComponentBase component, EntityEventHandler handler)
			{
				Component = component;
				Handler = handler;
			}
		}

		/// <summary>
		/// This class is a dictionary of registered handlers for different events that happened on the entity
		/// </summary>
		private class RegisteredEvents : Dictionary<MyStringHash, List<RegisteredComponent>>
		{
			public RegisteredEvents(MyStringHash eventType, MyComponentBase component, EntityEventHandler handler)
			{
				base[eventType] = new List<RegisteredComponent>();
				base[eventType].Add(new RegisteredComponent(component, handler));
			}
		}

		/// <summary>
		/// This dictionary holds entityId as key, and value is the dictionary of registered events to be listened
		/// </summary>
		private static Dictionary<long, RegisteredEvents> RegisteredListeners = new Dictionary<long, RegisteredEvents>();

		/// <summary>
		/// This dictionary holds listeners (components) that are attached to other entities entities and listen on another entity
		/// </summary>
		private static Dictionary<MyComponentBase, List<long>> ExternalListeners = new Dictionary<MyComponentBase, List<long>>();

		/// <summary>
		/// This hashset contains entities, which's events are registered by components / handlers that don't belong to their containers.
		/// </summary>
		private static HashSet<long> ExternalyListenedEntities = new HashSet<long>();

		private static List<RegisteredComponent> m_tmpList = new List<RegisteredComponent>();

		private static List<MyComponentBase> m_tmpCompList = new List<MyComponentBase>();

		private static bool ProcessingEvents;

		private static bool HasPostponedOperations;

		private static List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash, EntityEventHandler>> PostponedRegistration = new List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash, EntityEventHandler>>();

		private static List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash>> PostponedUnregistration = new List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash>>();

		private static List<long> PostPonedRegisteredListenersRemoval = new List<long>();

		/// <summary>
		/// This counter is used to count how many events invoke other entity events, if this amount is too high, we have to rework the mechanism and hav postopned invokation..
		/// </summary>
		private static int m_debugCounter;

		public static void InitEntityEvents()
		{
			RegisteredListeners = new Dictionary<long, RegisteredEvents>();
			ExternalListeners = new Dictionary<MyComponentBase, List<long>>();
			ExternalyListenedEntities = new HashSet<long>();
			PostponedRegistration = new List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash, EntityEventHandler>>();
			PostponedUnregistration = new List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash>>();
			ProcessingEvents = false;
			HasPostponedOperations = false;
		}

		/// <summary>
		/// This will register the component to listen to some events..
		/// </summary>
		/// <param name="component">Component that is being registered</param>
		/// <param name="eventType">type of event</param>
		/// <param name="handler">handler to be called</param>
		public static void RegisterForEntityEvent(this MyEntityComponentBase component, MyStringHash eventType, EntityEventHandler handler)
		{
			if (ProcessingEvents)
			{
				AddPostponedRegistration(component, component.Entity as MyEntity, eventType, handler);
			}
			else
			{
				if (component.Entity == null)
				{
					return;
				}
				component.BeforeRemovedFromContainer += RegisteredComponentBeforeRemovedFromContainer;
				component.Entity.OnClose += RegisteredEntityOnClose;
				if (RegisteredListeners.ContainsKey(component.Entity.EntityId))
				{
					RegisteredEvents registeredEvents = RegisteredListeners[component.Entity.EntityId];
					if (registeredEvents.ContainsKey(eventType))
					{
						if (registeredEvents[eventType].Find((RegisteredComponent x) => x.Handler == handler) == null)
						{
							registeredEvents[eventType].Add(new RegisteredComponent(component, handler));
						}
					}
					else
					{
						registeredEvents[eventType] = new List<RegisteredComponent>();
						registeredEvents[eventType].Add(new RegisteredComponent(component, handler));
					}
				}
				else
				{
					RegisteredListeners[component.Entity.EntityId] = new RegisteredEvents(eventType, component, handler);
				}
			}
		}

		/// <summary>
		/// This will register the component to listen to some events on entity that is other than entity containing this component
		/// </summary>
		/// <param name="entity">Entity on which we listen to events</param>
		/// <param name="component">Component that is being registered</param>
		/// <param name="eventType">type of event</param>
		/// <param name="handler">handler to be called</param>
		public static void RegisterForEntityEvent(this MyEntityComponentBase component, MyEntity entity, MyStringHash eventType, EntityEventHandler handler)
		{
			if (ProcessingEvents)
			{
				AddPostponedRegistration(component, entity, eventType, handler);
			}
			else if (component.Entity == entity)
			{
				component.RegisterForEntityEvent(eventType, handler);
			}
			else
			{
				if (entity == null)
				{
					return;
				}
				component.BeforeRemovedFromContainer += RegisteredComponentBeforeRemovedFromContainer;
				entity.OnClose += RegisteredEntityOnClose;
				if (RegisteredListeners.ContainsKey(entity.EntityId))
				{
					RegisteredEvents registeredEvents = RegisteredListeners[entity.EntityId];
					if (registeredEvents.ContainsKey(eventType))
					{
						if (registeredEvents[eventType].Find((RegisteredComponent x) => x.Handler == handler) == null)
						{
							registeredEvents[eventType].Add(new RegisteredComponent(component, handler));
						}
					}
					else
					{
						registeredEvents[eventType] = new List<RegisteredComponent>();
						registeredEvents[eventType].Add(new RegisteredComponent(component, handler));
					}
				}
				else
				{
					RegisteredListeners[entity.EntityId] = new RegisteredEvents(eventType, component, handler);
				}
				if (ExternalListeners.ContainsKey(component) && !ExternalListeners[component].Contains(entity.EntityId))
				{
					ExternalListeners[component].Add(entity.EntityId);
				}
				else
				{
					ExternalListeners[component] = new List<long> { entity.EntityId };
				}
				ExternalyListenedEntities.Add(entity.EntityId);
			}
		}

		/// <summary>
		/// This will unregister the component to listen to some events on entity that is other than entity containing this component
		/// </summary>
		/// <param name="entity">Entity on which we listen to events</param>
		/// <param name="component">Component that is being registered</param>
		/// <param name="eventType">type of event</param>        
		public static void UnregisterForEntityEvent(this MyEntityComponentBase component, MyEntity entity, MyStringHash eventType)
		{
			if (ProcessingEvents)
			{
				AddPostponedUnregistration(component, entity, eventType);
			}
			else
			{
				if (entity == null)
				{
					return;
				}
				bool flag = true;
				if (RegisteredListeners.ContainsKey(entity.EntityId))
				{
					if (RegisteredListeners[entity.EntityId].ContainsKey(eventType))
					{
						RegisteredListeners[entity.EntityId][eventType].RemoveAll((RegisteredComponent x) => x.Component == component);
						if (RegisteredListeners[entity.EntityId][eventType].Count == 0)
						{
							RegisteredListeners[entity.EntityId].Remove(eventType);
						}
					}
					if (RegisteredListeners[entity.EntityId].Count == 0)
					{
						RegisteredListeners.Remove(entity.EntityId);
						ExternalyListenedEntities.Remove(entity.EntityId);
						flag = false;
					}
				}
				if (ExternalListeners.ContainsKey(component) && ExternalListeners[component].Contains(entity.EntityId))
				{
					ExternalListeners[component].Remove(entity.EntityId);
					if (ExternalListeners[component].Count == 0)
					{
						ExternalListeners.Remove(component);
					}
				}
				if (!flag)
				{
					entity.OnClose -= RegisteredEntityOnClose;
				}
			}
		}

		/// <summary>
		/// When entity is being closed, we need to clean it's records for events
		/// </summary>
		/// <param name="entity">entity being removed</param>
		private static void RegisteredEntityOnClose(IMyEntity entity)
		{
			entity.OnClose -= RegisteredEntityOnClose;
			if (RegisteredListeners.ContainsKey(entity.EntityId))
			{
				if (ProcessingEvents)
				{
					AddPostponedListenerRemoval(entity.EntityId);
				}
				else
				{
					RegisteredListeners.Remove(entity.EntityId);
				}
			}
			if (!ExternalyListenedEntities.Contains(entity.EntityId))
			{
				return;
			}
			ExternalyListenedEntities.Remove(entity.EntityId);
			m_tmpCompList.Clear();
			foreach (KeyValuePair<MyComponentBase, List<long>> externalListener in ExternalListeners)
			{
				externalListener.Value.Remove(entity.EntityId);
				if (externalListener.Value.Count == 0)
				{
					m_tmpCompList.Add(externalListener.Key);
				}
			}
			foreach (MyComponentBase tmpComp in m_tmpCompList)
			{
				ExternalListeners.Remove(tmpComp);
			}
		}

		/// <summary>
		/// When component is removed, clean it's records
		/// </summary>
		/// <param name="component">component being removed from its container (entity) </param>
		private static void RegisteredComponentBeforeRemovedFromContainer(MyEntityComponentBase component)
		{
			component.BeforeRemovedFromContainer -= RegisteredComponentBeforeRemovedFromContainer;
			if (component.Entity == null)
			{
				return;
			}
			if (RegisteredListeners.ContainsKey(component.Entity.EntityId))
			{
				m_tmpList.Clear();
				foreach (KeyValuePair<MyStringHash, List<RegisteredComponent>> item in RegisteredListeners[component.Entity.EntityId])
				{
					item.Value.RemoveAll((RegisteredComponent x) => x.Component == component);
				}
			}
			if (!ExternalListeners.ContainsKey(component))
			{
				return;
			}
			foreach (long item2 in ExternalListeners[component])
			{
				if (!RegisteredListeners.ContainsKey(item2))
				{
					continue;
				}
				foreach (KeyValuePair<MyStringHash, List<RegisteredComponent>> item3 in RegisteredListeners[item2])
				{
					item3.Value.RemoveAll((RegisteredComponent x) => x.Component == component);
				}
			}
			ExternalListeners.Remove(component);
		}

		/// <summary>
		/// Call this to raise event on entity, that will be processed by registered components
		/// </summary>
		/// <param name="entity">this is entity on which is this being invoked</param>
		/// <param name="eventType">type of event</param>
		/// <param name="eventParams">event params or derived type</param>
		public static void RaiseEntityEvent(this MyEntity entity, MyStringHash eventType, EntityEventParams eventParams)
		{
			if (entity.Components != null)
			{
				InvokeEventOnListeners(entity.EntityId, eventType, eventParams);
			}
		}

		/// <summary>
		/// Call this to raise event on entity, that will be processed by registered components
		/// </summary>
		/// <param name="entity">this is entity on which is this being invoked</param>
		/// <param name="eventType">type of event</param>
		/// <param name="eventParams">event params or derived type</param>
		public static void RaiseEntityEventOn(MyEntity entity, MyStringHash eventType, EntityEventParams eventParams)
		{
			if (entity.Components != null)
			{
				InvokeEventOnListeners(entity.EntityId, eventType, eventParams);
			}
		}

		/// <summary>
		/// Call this to raise event on entity, that will be processed by registered components
		/// </summary>
		/// <param name="component">component upon which container this is going to be invoke</param>
		/// <param name="eventType">type of event</param>
		/// <param name="eventParams">event params or derived type</param>
		public static void RaiseEntityEvent(this MyEntityComponentBase component, MyStringHash eventType, EntityEventParams eventParams)
		{
			if (component.Entity != null)
			{
				InvokeEventOnListeners(component.Entity.EntityId, eventType, eventParams);
			}
		}

		/// <summary>
		/// This just iterates through registered listeners and informs them..
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="eventType"></param>
		/// <param name="eventParams"></param>
		private static void InvokeEventOnListeners(long entityId, MyStringHash eventType, EntityEventParams eventParams)
		{
			bool processingEvents = ProcessingEvents;
			if (processingEvents)
			{
				m_debugCounter++;
			}
			if (m_debugCounter > 5)
			{
				return;
			}
			ProcessingEvents = true;
			if (RegisteredListeners.TryGetValue(entityId, out var value) && value.TryGetValue(eventType, out var value2))
			{
				foreach (RegisteredComponent item in value2)
				{
					try
					{
						item.Handler(eventParams);
					}
					catch (Exception)
					{
					}
				}
			}
			ProcessingEvents = processingEvents;
			if (!ProcessingEvents)
			{
				m_debugCounter = 0;
			}
			if (HasPostponedOperations && !ProcessingEvents)
			{
				ProcessPostponedRegistrations();
			}
		}

		private static void ProcessPostponedRegistrations()
		{
			foreach (Tuple<MyEntityComponentBase, MyEntity, MyStringHash, EntityEventHandler> item in PostponedRegistration)
			{
				item.Item1.RegisterForEntityEvent(item.Item2, item.Item3, item.Item4);
			}
			foreach (Tuple<MyEntityComponentBase, MyEntity, MyStringHash> item2 in PostponedUnregistration)
			{
				item2.Item1.UnregisterForEntityEvent(item2.Item2, item2.Item3);
			}
			foreach (long item3 in PostPonedRegisteredListenersRemoval)
			{
				RegisteredListeners.Remove(item3);
			}
			PostponedRegistration.Clear();
			PostponedUnregistration.Clear();
			PostPonedRegisteredListenersRemoval.Clear();
			HasPostponedOperations = false;
		}

		private static void AddPostponedRegistration(MyEntityComponentBase component, MyEntity entity, MyStringHash eventType, EntityEventHandler handler)
		{
			PostponedRegistration.Add(new Tuple<MyEntityComponentBase, MyEntity, MyStringHash, EntityEventHandler>(component, entity, eventType, handler));
			HasPostponedOperations = true;
		}

		private static void AddPostponedUnregistration(MyEntityComponentBase component, MyEntity entity, MyStringHash eventType)
		{
			PostponedUnregistration.Add(new Tuple<MyEntityComponentBase, MyEntity, MyStringHash>(component, entity, eventType));
			HasPostponedOperations = true;
		}

		private static void AddPostponedListenerRemoval(long id)
		{
			PostPonedRegisteredListenersRemoval.Add(id);
			HasPostponedOperations = true;
		}

		public static void SkipProcessingEvents(bool state)
		{
			ProcessingEvents = state;
			if (!state && HasPostponedOperations)
			{
				ProcessPostponedRegistrations();
			}
		}
	}
}
