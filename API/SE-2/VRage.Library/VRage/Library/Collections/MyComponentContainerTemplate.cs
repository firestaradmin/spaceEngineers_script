using System;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
	/// <summary>
	/// Template for an indexed component container.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class MyComponentContainerTemplate<T> where T : class
	{
		internal MyIndexedComponentContainer<Func<Type, T>> Components = new MyIndexedComponentContainer<Func<Type, T>>();

		public MyComponentContainerTemplate(List<Type> types, List<Func<Type, T>> compoentFactories)
		{
			for (int i = 0; i < types.Count; i++)
			{
				Components.Add(types[i], compoentFactories[i]);
			}
		}

		public MyComponentContainerTemplate(Type[] types, Func<Type, T>[] compoentFactories)
		{
			for (int i = 0; i < types.Length; i++)
			{
				Components.Add(types[i], compoentFactories[i]);
			}
		}
	}
}
