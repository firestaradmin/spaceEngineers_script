using System.Threading;
using SharpDX.Direct3D11;
using VRage.Render11.RenderContext;
using VRageRender;

namespace VRage.Render11.Culling.Occlusion
{
	internal class MyOcclusionQuery
	{
		private readonly Query m_query;

		private long m_result;

		private bool m_open;

		private bool m_skipNext;

		internal bool Running { get; private set; }

		internal string DebugName
		{
			set
			{
				m_query.DebugName = value;
			}
		}

		internal MyOcclusionQuery(string debugName)
		{
			QueryDescription description = new QueryDescription
			{
				Type = QueryType.Occlusion
			};
			m_query = new Query(MyRender11.DeviceInstance, description)
			{
				DebugName = debugName
			};
		}

		internal void Return()
		{
			MyOccllusionQueryFactory.RelaseOcclusionQuery(this);
		}

		internal void Begin(MyRenderContext rc)
		{
			if (!Running)
			{
				rc.Begin(m_query);
				Running = true;
				m_open = true;
			}
		}

		internal void End(MyRenderContext rc)
		{
			if (m_open)
			{
				rc.End(m_query);
				m_open = false;
			}
		}

		internal long GetResult(bool stalling = false)
		{
			if (!Running)
			{
				return m_result;
			}
			long result;
			if (!stalling)
			{
				if (!MyRender11.RCForQueries.GetData<long>(m_query, AsynchronousFlags.DoNotFlush, out result))
				{
					return -1L;
				}
			}
			else
			{
				while (!MyRender11.RCForQueries.GetData<long>(m_query, AsynchronousFlags.None, out result))
				{
					Thread.Sleep(1);
				}
			}
			Running = false;
			if (!m_skipNext)
			{
				m_result = result;
			}
			else
			{
				m_skipNext = false;
			}
			return m_result;
		}

		public void Reset(long value)
		{
			m_skipNext = true;
			GetResult();
			m_skipNext = Running;
			m_result = value;
		}
	}
}
