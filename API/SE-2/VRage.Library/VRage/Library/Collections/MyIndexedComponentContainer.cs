using System;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
	/// <summary>
	/// Component container that stores the components in a list and uses a shared index for quick access.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class MyIndexedComponentContainer<T> where T : class
	{
		private static readonly IndexHost Host = new IndexHost();

		private ComponentIndex m_componentIndex;

		private readonly List<T> m_components = new List<T>();

		public T this[int index] => m_components[index];

		public T this[Type type] => m_components[m_componentIndex.Index[type]];

		public int Count => m_components.Count;

		public MyIndexedComponentContainer()
		{
			m_componentIndex = Host.GetEmptyComponentIndex();
		}

		/// <summary>
		/// Create a component from a template.
		/// </summary>
		/// <param name="template">template</param>
		public MyIndexedComponentContainer(MyComponentContainerTemplate<T> template)
		{
			m_components.Capacity = template.Components.Count;
			for (int i = 0; i < template.Components.Count; i++)
			{
				Func<Type, T> func = template.Components[i];
				Type arg = template.Components.m_componentIndex.Types[i];
				m_components.Add(func(arg));
			}
			m_componentIndex = template.Components.m_componentIndex;
		}

		public TComponent GetComponent<TComponent>() where TComponent : T
		{
			return (TComponent)m_components[m_componentIndex.Index[typeof(TComponent)]];
		}

		public TComponent TryGetComponent<TComponent>() where TComponent : class, T
		{
			return (TComponent)TryGetComponent(typeof(TComponent));
		}

		public T TryGetComponent(Type t)
		{
			if (m_componentIndex.Index.TryGetValue(t, out var value))
			{
				return m_components[value];
			}
			return null;
		}

		public void Add(Type slot, T component)
		{
			if (Host == null)
			{
				throw new NullReferenceException("Host is null.");
			}
			if (m_componentIndex == null)
			{
				throw new NullReferenceException("m_componentIndex is null.");
			}
			if (m_componentIndex.Types == null)
			{
				throw new NullReferenceException("m_componentIndex.Types is null.");
			}
			if (component == null)
			{
				throw new NullReferenceException("component is null.");
			}
			if (!m_componentIndex.Index.ContainsKey(slot))
			{
				m_componentIndex = Host.GetAfterInsert(m_componentIndex, slot, out var insertionPoint);
				if (m_componentIndex == null)
				{
					throw new NullReferenceException("After adding new component m_componentIndex became null.");
				}
				m_components.Insert(insertionPoint, component);
			}
		}

		public void Remove(Type slot)
		{
			if (m_componentIndex.Index.ContainsKey(slot))
			{
				m_componentIndex = Host.GetAfterRemove(m_componentIndex, slot, out var removalPoint);
				m_components.RemoveAt(removalPoint);
			}
		}

		public void Clear()
		{
			m_components.Clear();
			m_componentIndex = Host.GetEmptyComponentIndex();
		}

		public bool Contains<TComponent>() where TComponent : T
		{
			return m_componentIndex.Index.ContainsKey(typeof(TComponent));
		}
	}
}
