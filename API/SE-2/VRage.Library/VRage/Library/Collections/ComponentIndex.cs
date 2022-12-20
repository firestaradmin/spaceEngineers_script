using System;
using System.Collections.Generic;
using VRage.Library.Collections.__helper_namespace;

namespace VRage.Library.Collections
{
	/// <summary>
	/// Index for a component container.
	/// </summary>
	public class ComponentIndex
	{
		internal readonly TypeList Types;

		public readonly Dictionary<Type, int> Index = new Dictionary<Type, int>();

		internal ComponentIndex(TypeList typeList)
		{
			for (int i = 0; i < typeList.Count; i++)
			{
				Index[typeList[i]] = i;
			}
			Types = typeList;
		}
	}
}
