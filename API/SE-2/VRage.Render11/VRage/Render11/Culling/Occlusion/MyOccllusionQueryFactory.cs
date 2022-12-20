using System.Collections.Generic;

namespace VRage.Render11.Culling.Occlusion
{
	internal static class MyOccllusionQueryFactory
	{
		private static readonly List<MyOcclusionQuery> m_pool = new List<MyOcclusionQuery>();

		internal static MyOcclusionQuery CreateOcclusionQuery(string debugName)
		{
			lock (m_pool)
			{
				if (m_pool.Count > 0)
				{
					MyOcclusionQuery myOcclusionQuery = m_pool[m_pool.Count - 1];
					m_pool.RemoveAt(m_pool.Count - 1);
					myOcclusionQuery.DebugName = debugName;
					return myOcclusionQuery;
				}
			}
			return new MyOcclusionQuery(debugName);
		}

		internal static void RelaseOcclusionQuery(MyOcclusionQuery q)
		{
			m_pool.Add(q);
		}
	}
}
