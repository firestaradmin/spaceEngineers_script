using System;
using VRage.Generics;
using VRageRender;

namespace VRage.Render11.Scene.Components
{
	internal class MyCullProxy_2
	{
		internal ulong[] SortingKeys;

		internal MyRenderableProxy_2[] Proxies;

		internal int FirstFullyContainingCascadeIndex;

		private static readonly MyObjectsPool<MyCullProxy_2> m_pool = new MyObjectsPool<MyCullProxy_2>(128);

		private void Construct()
		{
			SortingKeys = MyRenderableProxy_2.EmptyKeyList;
			Proxies = MyRenderableProxy_2.EmptyList;
		}

		private void Resize(int size)
		{
			Array.Resize(ref SortingKeys, size);
			Array.Resize(ref Proxies, size);
		}

		internal void Extend(int size)
		{
			Resize(size);
		}

		private void Clear()
		{
			SortingKeys = MyRenderableProxy_2.EmptyKeyList;
			Proxies = MyRenderableProxy_2.EmptyList;
		}

		internal static MyCullProxy_2 Allocate()
		{
			m_pool.AllocateOrCreate(out var item);
			item.Construct();
			return item;
		}

		internal static void Free(MyCullProxy_2 cullProxy2)
		{
			cullProxy2.Clear();
			m_pool.Deallocate(cullProxy2);
		}
	}
}
