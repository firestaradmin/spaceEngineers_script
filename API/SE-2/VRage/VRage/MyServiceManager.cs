using System;
using System.Collections.Generic;

namespace VRage
{
	public sealed class MyServiceManager
	{
		private static readonly MyServiceManager m_singleton = new MyServiceManager();

		private readonly Dictionary<Type, object> m_services;

		private readonly object m_lockObject;

		public static MyServiceManager Instance => m_singleton;

		public event Action<object> OnChanged;

		private MyServiceManager()
		{
			m_lockObject = new object();
			m_services = new Dictionary<Type, object>();
		}

		public void AddService<T>(T serviceInstance) where T : class
		{
			lock (m_lockObject)
			{
				m_services[typeof(T)] = serviceInstance;
				this.OnChanged?.Invoke(serviceInstance);
			}
		}

		public T GetService<T>() where T : class
		{
			object value;
			lock (m_lockObject)
			{
				m_services.TryGetValue(typeof(T), out value);
			}
			return value as T;
		}

		public void RemoveService<T>() where T : class
		{
			lock (m_lockObject)
			{
				T service = GetService<T>();
				if (m_services.Remove(typeof(T)))
				{
					this.OnChanged?.Invoke(service);
				}
			}
		}
	}
}
