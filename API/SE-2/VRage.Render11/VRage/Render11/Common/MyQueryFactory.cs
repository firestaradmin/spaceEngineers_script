using SharpDX.Direct3D11;
using VRage.Collections;

namespace VRage.Render11.Common
{
	internal static class MyQueryFactory
	{
		internal const int MAX_FRAMES_LAG = 8;

		private const int MAX_TIMESTAMP_QUERIES = 4096;

		private static readonly MyConcurrentPool<MyQuery> m_disjointQueries;

		private static readonly MyConcurrentPool<MyQuery> m_timestampQueries;

		private static readonly MyConcurrentPool<MyQuery> m_eventQueries;

		static MyQueryFactory()
		{
			m_disjointQueries = new MyConcurrentPool<MyQuery>(8);
			m_timestampQueries = new MyConcurrentPool<MyQuery>(4096);
			m_eventQueries = new MyConcurrentPool<MyQuery>(10);
		}

		internal static MyQuery CreateTimestampQuery()
		{
			MyQuery myQuery = m_timestampQueries.Get();
			myQuery.LazyInit(QueryType.Timestamp);
			return myQuery;
		}

		internal static void RelaseTimestampQuery(MyQuery q)
		{
			m_timestampQueries.Return(q);
		}

		internal static MyQuery CreateDisjointQuery()
		{
			MyQuery myQuery = m_disjointQueries.Get();
			myQuery.LazyInit(QueryType.TimestampDisjoint);
			return myQuery;
		}

		internal static void RelaseDisjointQuery(MyQuery q)
		{
			m_disjointQueries.Return(q);
		}

		internal static MyQuery CreateEventQuery()
		{
			MyQuery myQuery = m_eventQueries.Get();
			myQuery.LazyInit(QueryType.Event);
			return myQuery;
		}

		internal static void RelaseEventQuery(MyQuery q)
		{
			m_eventQueries.Return(q);
		}
	}
}
