using System;
using System.Collections.Generic;
using System.Reflection;

namespace VRageRender
{
	[PreloadRequired]
	internal static class MyObjectPoolManager
	{
		private static readonly Dictionary<Type, MyGenericObjectPool> m_poolsByType;

		internal static T Allocate<T>() where T : class, IPooledObject
		{
			return (T)Allocate(typeof(T));
		}

		internal static T Allocate<T>(Type subType) where T : class, IPooledObject
		{
			return (T)Allocate(subType);
		}

		private static object Allocate(Type typeToAllocate)
		{
			MyGenericObjectPool value = null;
			if (!m_poolsByType.TryGetValue(typeToAllocate, out value))
			{
				return null;
			}
			IPooledObject item = null;
			bool flag;
			lock (value)
			{
				flag = value.AllocateOrCreate(out item);
			}
			if (flag)
			{
				item.Cleanup();
			}
			return item;
		}

		internal static void Deallocate<T>(T objectToDeallocate) where T : class, IPooledObject
		{
			MyGenericObjectPool value = null;
			if (m_poolsByType.TryGetValue(objectToDeallocate.GetType(), out value))
			{
				objectToDeallocate.Cleanup();
				lock (value)
				{
					value.Deallocate(objectToDeallocate);
				}
			}
		}

		internal static void Init<T>(ref T objectToInit) where T : class, IPooledObject
		{
			if (objectToInit == null)
			{
				objectToInit = Allocate<T>();
			}
			else if (m_poolsByType.ContainsKey(typeof(T)))
			{
				objectToInit.Cleanup();
			}
		}

		static MyObjectPoolManager()
		{
			m_poolsByType = new Dictionary<Type, MyGenericObjectPool>();
			RegisterPoolsFromAssembly(typeof(MyObjectPoolManager).Assembly);
		}

		public static void RegisterPool(Type type, int preallocationSize = 2)
		{
<<<<<<< HEAD
=======
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!typeof(IPooledObject).IsAssignableFrom(type))
			{
				return;
			}
			MyGenericObjectPool myGenericObjectPool = new MyGenericObjectPool(preallocationSize, type);
<<<<<<< HEAD
			foreach (IPooledObject item in myGenericObjectPool.Unused)
			{
				item.Cleanup();
=======
			Enumerator<IPooledObject> enumerator = myGenericObjectPool.Unused.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Cleanup();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_poolsByType.Add(type, myGenericObjectPool);
		}

		private static void RegisterPoolsFromAssembly(Assembly assembly)
		{
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(PooledObjectAttribute), inherit: false);
				if (customAttributes != null && customAttributes.Length != 0)
				{
					PooledObjectAttribute pooledObjectAttribute = (PooledObjectAttribute)customAttributes[0];
					RegisterPool(type, pooledObjectAttribute.PoolPreallocationSize);
				}
			}
		}
	}
}
