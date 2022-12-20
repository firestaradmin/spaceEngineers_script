using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Linq;
using VRage.Generics;
using VRage.Render.Scene.Components;
using VRage.Render11.Tools;
<<<<<<< HEAD
using VRageRender;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

namespace VRage.Render11.Scene
{
	internal static class MyComponentFactory
	{
		private static readonly Dictionary<Type, MyRuntimeObjectsPool<MyActorComponent>> m_pools = new Dictionary<Type, MyRuntimeObjectsPool<MyActorComponent>>();

<<<<<<< HEAD
		private static readonly string[] m_adminOnlyDebugNames = new string[1] { "MySkinningComponent" };

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		internal static MyActorComponent Create(Type type)
		{
			if (!m_pools.TryGetValue(type, out var value))
			{
				value = (m_pools[type] = new MyRuntimeObjectsPool<MyActorComponent>(16, type));
			}
			value.AllocateOrCreate(out var item);
			item.Construct();
			return item;
		}

		public static void Deallocate(MyActorComponent item)
		{
			m_pools[item.GetType()].Deallocate(item);
		}

		internal static IEnumerable<MyActorComponent> GetAll(Type t)
		{
			if (!m_pools.TryGetValue(t, out var value))
			{
				return Enumerable.Empty<MyActorComponent>();
			}
			return value.Active;
		}

		public static void FillStats(string page)
		{
			foreach (KeyValuePair<Type, MyRuntimeObjectsPool<MyActorComponent>> pool in m_pools)
			{
<<<<<<< HEAD
				if (!m_adminOnlyDebugNames.Contains(pool.Key.Name) || MyRenderProxy.IsPlayerSpaceMaster)
				{
					MyStatsDisplay.Write("Actor Components", pool.Key.Name, pool.Value.ActiveCount, page);
				}
=======
				MyStatsDisplay.Write("Actor Components", pool.Key.Name, pool.Value.ActiveCount, page);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
	internal static class MyComponentFactory<T> where T : MyActorComponent, new()
	{
		internal static T Create()
		{
			return (T)MyComponentFactory.Create(typeof(T));
		}

		internal static void Deallocate(T item)
		{
			MyComponentFactory.Deallocate(item);
		}

		internal static IEnumerable<T> GetAll()
		{
<<<<<<< HEAD
			return MyComponentFactory.GetAll(typeof(T)).Cast<T>();
=======
			return Enumerable.Cast<T>((IEnumerable)MyComponentFactory.GetAll(typeof(T)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
