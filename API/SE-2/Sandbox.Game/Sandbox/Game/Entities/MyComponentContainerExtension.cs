using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
	/// <summary>
	/// TODO: This should be later ideally some factory rather than just an extension on the MyComponentContainer
	/// </summary>
	public static class MyComponentContainerExtension
	{
		/// <summary>
		/// Tries to retrieve entity definition of the entity owning this container, check if the definition has some DefaultComponents,
		/// tries to retrieve these components definitions, create these components instances and add them
		///
		/// TODO: This should be ideally a behavior of the MyEntityComponentContainer when it is initialized (deserialized).. or by the factory, for now, this is an extension method
		/// </summary>        
		public static void InitComponents(this MyComponentContainer container, MyObjectBuilderType type, MyStringHash subtypeName, MyObjectBuilder_ComponentContainer builder)
		{
			if (MyDefinitionManager.Static == null)
			{
				return;
			}
			MyContainerDefinition definition = null;
			bool flag = builder == null;
			if (TryGetContainerDefinition(type, subtypeName, out definition))
			{
				container.Init(definition);
				if (definition.DefaultComponents != null)
				{
					foreach (MyContainerDefinition.DefaultComponent defaultComponent in definition.DefaultComponents)
					{
						MyComponentDefinitionBase componentDefinition = null;
						MyObjectBuilder_ComponentBase myObjectBuilder_ComponentBase = FindComponentBuilder(defaultComponent, builder);
						bool flag2 = myObjectBuilder_ComponentBase != null;
						Type type2 = null;
						MyComponentBase myComponentBase = null;
						MyStringHash subtypeName2 = subtypeName;
						if (defaultComponent.SubtypeId.HasValue)
						{
							subtypeName2 = defaultComponent.SubtypeId.Value;
						}
						if (TryGetComponentDefinition(defaultComponent.BuilderType, subtypeName2, out componentDefinition))
						{
							myComponentBase = MyComponentFactory.CreateInstanceByTypeId(componentDefinition.Id.TypeId);
							myComponentBase.Init(componentDefinition);
						}
						else if (defaultComponent.IsValid())
						{
							myComponentBase = (defaultComponent.BuilderType.IsNull ? MyComponentFactory.CreateInstanceByType(defaultComponent.InstanceType) : MyComponentFactory.CreateInstanceByTypeId(defaultComponent.BuilderType));
						}
						if (myComponentBase != null)
						{
							Type componentType = MyComponentTypeFactory.GetComponentType(myComponentBase.GetType());
							if (componentType != null)
							{
								type2 = componentType;
							}
						}
						if (type2 == null && myComponentBase != null)
						{
							type2 = myComponentBase.GetType();
						}
						if (myComponentBase != null && type2 != null && (flag || flag2 || defaultComponent.ForceCreate))
						{
							if (myObjectBuilder_ComponentBase != null)
							{
								myComponentBase.Deserialize(myObjectBuilder_ComponentBase);
							}
							container.Add(type2, myComponentBase);
						}
					}
				}
			}
			container.Deserialize(builder);
		}

		public static MyObjectBuilder_ComponentBase FindComponentBuilder(MyContainerDefinition.DefaultComponent component, MyObjectBuilder_ComponentContainer builder)
		{
			MyObjectBuilder_ComponentBase result = null;
			if (builder != null && component.IsValid())
			{
				_ = (MyObjectBuilderType)null;
				if (!component.BuilderType.IsNull)
				{
					MyObjectBuilder_ComponentContainer.ComponentData componentData = builder.Components.Find((MyObjectBuilder_ComponentContainer.ComponentData x) => x.Component.TypeId == component.BuilderType);
					if (componentData != null)
					{
						result = componentData.Component;
					}
				}
			}
			return result;
		}

		public static bool TryGetContainerDefinition(MyObjectBuilderType type, MyStringHash subtypeName, out MyContainerDefinition definition)
		{
			definition = null;
			if (MyDefinitionManager.Static != null)
			{
				MyDefinitionId containerId = new MyDefinitionId(type, subtypeName);
				if (MyDefinitionManager.Static.TryGetContainerDefinition(containerId, out definition))
				{
					return true;
				}
				if (subtypeName != MyStringHash.NullOrEmpty)
				{
					MyDefinitionId containerId2 = new MyDefinitionId(typeof(MyObjectBuilder_EntityBase), subtypeName);
					if (MyDefinitionManager.Static.TryGetContainerDefinition(containerId2, out definition))
					{
						return true;
					}
				}
				MyDefinitionId containerId3 = new MyDefinitionId(type);
				if (MyDefinitionManager.Static.TryGetContainerDefinition(containerId3, out definition))
				{
					return true;
				}
			}
			return false;
		}

		public static bool TryGetComponentDefinition(MyObjectBuilderType type, MyStringHash subtypeName, out MyComponentDefinitionBase componentDefinition)
		{
			componentDefinition = null;
			if (MyDefinitionManager.Static != null)
			{
				MyDefinitionId componentId = new MyDefinitionId(type, subtypeName);
				if (MyDefinitionManager.Static.TryGetEntityComponentDefinition(componentId, out componentDefinition))
				{
					return true;
				}
				if (subtypeName != MyStringHash.NullOrEmpty)
				{
					MyDefinitionId componentId2 = new MyDefinitionId(typeof(MyObjectBuilder_EntityBase), subtypeName);
					if (MyDefinitionManager.Static.TryGetEntityComponentDefinition(componentId2, out componentDefinition))
					{
						return true;
					}
				}
				MyDefinitionId componentId3 = new MyDefinitionId(type);
				if (MyDefinitionManager.Static.TryGetEntityComponentDefinition(componentId3, out componentDefinition))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// This will retrieve component types in the entity container. This method allocates, use only for debugging etc.
		/// </summary>
		/// <returns>true if success</returns>
		public static bool TryGetEntityComponentTypes(long entityId, out List<Type> components)
		{
			components = null;
			if (MyEntities.TryGetEntityById(entityId, out var entity))
			{
				components = new List<Type>();
				foreach (Type componentType in entity.Components.GetComponentTypes())
				{
					if (componentType != null)
					{
						components.Add(componentType);
					}
				}
				if (components.Count > 0)
				{
					return true;
				}
			}
			return false;
		}

		public static bool TryRemoveComponent(long entityId, Type componentType)
		{
			if (MyEntities.TryGetEntityById(entityId, out var entity))
			{
				entity.Components.Remove(componentType);
				return true;
			}
			return false;
		}

		/// <summary>
		/// This will look for the component definition and if found, it will create its instance and add to the entity with the give id
		/// </summary>
		/// <returns>true on success</returns>
		public static bool TryAddComponent(long entityId, MyDefinitionId componentDefinitionId)
		{
			if (MyEntities.TryGetEntityById(entityId, out var entity))
			{
				if (TryGetComponentDefinition(componentDefinitionId.TypeId, componentDefinitionId.SubtypeId, out var componentDefinition))
				{
					MyComponentBase myComponentBase = MyComponentFactory.CreateInstanceByTypeId(componentDefinition.Id.TypeId);
					Type componentType = MyComponentTypeFactory.GetComponentType(myComponentBase.GetType());
					if (componentType == null)
					{
						return false;
					}
					myComponentBase.Init(componentDefinition);
					entity.Components.Add(componentType, myComponentBase);
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// This will try to parse strings to types and create an instance of the component type. Don't use this in retail code, use for debug, modding etc.
		/// </summary>
		/// <param name="entityId">Id of entity which should get the component</param>
		/// <param name="instanceTypeStr">Type of the component instance, no the base type</param>
		/// <param name="componentTypeStr">The base type of the component to be added</param>
		/// <returns>true on success</returns>
		public static bool TryAddComponent(long entityId, string instanceTypeStr, string componentTypeStr)
		{
			Type type = null;
			Type type2 = null;
			try
			{
				type = Type.GetType(instanceTypeStr, throwOnError: true);
			}
			catch (Exception)
			{
			}
			try
			{
				type = Type.GetType(componentTypeStr, throwOnError: true);
			}
			catch (Exception)
			{
			}
			if (MyEntities.TryGetEntityById(entityId, out var entity) && type != null)
			{
				MyComponentBase myComponentBase = MyComponentFactory.CreateInstanceByType(type);
				if (entity.DefinitionId.HasValue && TryGetComponentDefinition(myComponentBase.GetType(), entity.DefinitionId.Value.SubtypeId, out var componentDefinition))
				{
					myComponentBase.Init(componentDefinition);
				}
				entity.Components.Add(type2, myComponentBase);
				return true;
			}
			return false;
		}
	}
}
