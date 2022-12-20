using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sandbox.Game.World;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.EntityComponents.Interfaces;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
	internal static class MyEntityFactory
	{
		private static MyObjectFactory<MyEntityTypeAttribute, MyEntity> m_objectFactory = new MyObjectFactory<MyEntityTypeAttribute, MyEntity>();

		private static readonly HashSet<Type> m_emptySet = new HashSet<Type>();

		public static void RegisterDescriptor(MyEntityTypeAttribute descriptor, Type type)
		{
			if (type != null && descriptor != null)
			{
				m_objectFactory.RegisterDescriptor(descriptor, type);
			}
		}

		public static void RegisterDescriptorsFromAssembly(Assembly[] assemblies)
		{
			if (assemblies != null)
			{
				for (int i = 0; i < assemblies.Length; i++)
				{
					RegisterDescriptorsFromAssembly(assemblies[i]);
				}
			}
		}

		public static void RegisterDescriptorsFromAssembly(Assembly assembly)
		{
			if (assembly != null)
			{
				m_objectFactory.RegisterFromAssembly(assembly);
			}
		}

		public static MyEntity CreateEntity(MyObjectBuilder_Base builder)
		{
			return CreateEntity(builder.TypeId, builder.SubtypeName);
		}

		public static MyEntity CreateEntity(MyObjectBuilderType typeId, string subTypeName = null)
		{
			MyEntity myEntity = m_objectFactory.CreateInstance(typeId);
			AddScriptGameLogic(myEntity, typeId, subTypeName);
			MyEntities.RaiseEntityCreated(myEntity);
			return myEntity;
		}

		public static T CreateEntity<T>(MyObjectBuilder_Base builder) where T : MyEntity
		{
			T val = m_objectFactory.CreateInstance<T>(builder.TypeId);
			AddScriptGameLogic(val, builder.GetType(), builder.SubtypeName);
			MyEntities.RaiseEntityCreated(val);
			return val;
		}

		public static void AddScriptGameLogic(MyEntity entity, MyObjectBuilderType builderType, string subTypeName = null)
		{
			MyScriptManager @static = MyScriptManager.Static;
			if (@static == null || entity == null)
			{
				return;
			}
<<<<<<< HEAD
			HashSet<Type> hashSet;
			if (subTypeName != null)
			{
				Tuple<Type, string> key = new Tuple<Type, string>(builderType, subTypeName);
				hashSet = @static.SubEntityScripts.GetValueOrDefault(key, m_emptySet);
			}
			else
			{
				hashSet = m_emptySet;
			}
			HashSet<Type> valueOrDefault = @static.EntityScripts.GetValueOrDefault(builderType, m_emptySet);
			int num = hashSet.Count + valueOrDefault.Count;
=======
			HashSet<Type> val;
			if (subTypeName != null)
			{
				Tuple<Type, string> key = new Tuple<Type, string>(builderType, subTypeName);
				val = @static.SubEntityScripts.GetValueOrDefault(key, m_emptySet);
			}
			else
			{
				val = m_emptySet;
			}
			HashSet<Type> valueOrDefault = @static.EntityScripts.GetValueOrDefault(builderType, m_emptySet);
			int num = val.get_Count() + valueOrDefault.get_Count();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (num == 0)
			{
				return;
			}
			List<MyGameLogicComponent> list = new List<MyGameLogicComponent>(num);
<<<<<<< HEAD
			foreach (Type item in valueOrDefault.Concat(hashSet))
			{
				MyGameLogicComponent myGameLogicComponent = (MyGameLogicComponent)Activator.CreateInstance(item);
				MyEntityComponentDescriptor myEntityComponentDescriptor = (MyEntityComponentDescriptor)CustomAttributeExtensions.GetCustomAttribute(item, typeof(MyEntityComponentDescriptor), inherit: false);
=======
			foreach (Type item in Enumerable.Concat<Type>((IEnumerable<Type>)valueOrDefault, (IEnumerable<Type>)val))
			{
				MyGameLogicComponent myGameLogicComponent = (MyGameLogicComponent)Activator.CreateInstance(item);
				MyEntityComponentDescriptor myEntityComponentDescriptor = (MyEntityComponentDescriptor)item.GetCustomAttribute(typeof(MyEntityComponentDescriptor), inherit: false);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!myEntityComponentDescriptor.EntityUpdate.HasValue)
				{
					((IMyGameLogicComponent)myGameLogicComponent).EntityUpdate = true;
				}
				else if (myEntityComponentDescriptor.EntityUpdate.Value)
				{
					((IMyGameLogicComponent)myGameLogicComponent).EntityUpdate = true;
				}
<<<<<<< HEAD
				((IMyGameLogicComponent)myGameLogicComponent).ModContext = @static.TypeToModMap[item];
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				list.Add(myGameLogicComponent);
			}
			MyGameLogicComponent myGameLogicComponent3 = (entity.GameLogic = MyCompositeGameLogicComponent.Create(list, entity));
		}

		public static MyObjectBuilder_EntityBase CreateObjectBuilder(MyEntity entity)
		{
			return m_objectFactory.CreateObjectBuilder<MyObjectBuilder_EntityBase>(entity);
		}
	}
}
