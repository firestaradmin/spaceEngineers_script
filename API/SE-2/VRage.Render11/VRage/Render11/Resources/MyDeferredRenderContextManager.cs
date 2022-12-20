#define VRAGE
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRageRender;

namespace VRage.Render11.Resources
{
	internal class MyDeferredRenderContextManager : IManager, IManagerDevice, IManagerUnloadData
	{
		private MyConcurrentPool<MyRenderContext> m_pool;

		private bool m_isDeviceInit;

		private static readonly List<MyRenderContext> m_tmpList;

		private readonly Dictionary<MyRenderContext, bool> m_acquired = new Dictionary<MyRenderContext, bool>();

		private const int MAX_DEFERRED_RC_COUNT = 48;

		static MyDeferredRenderContextManager()
		{
			m_tmpList = new List<MyRenderContext>();
		}

		public MyRenderContext AcquireRC(string debugName, bool clearState = true)
		{
			MyRenderContext myRenderContext = m_pool.Get();
			if (!myRenderContext.IsInitialized)
			{
				myRenderContext.Initialize();
			}
			if (clearState)
			{
				myRenderContext.ClearState();
			}
			myRenderContext.DebugName = debugName;
			lock (m_acquired)
			{
				if (!m_acquired.ContainsKey(myRenderContext))
				{
					m_acquired.Add(myRenderContext, value: true);
				}
				if (m_acquired.Count > 48)
				{
					string text = string.Empty;
					foreach (KeyValuePair<MyRenderContext, bool> item in m_acquired)
					{
						text += item.Key.DebugName;
						text += "\n";
					}
					MyRender11.Log.WriteLine("Acquired contexts:");
					MyRender11.Log.WriteLine(text);
					throw new MyRenderException("Too many render contexts allocated.");
				}
				return myRenderContext;
			}
		}

		public void FreeRC(MyRenderContext rc)
		{
			lock (m_acquired)
			{
				m_acquired.Remove(rc);
			}
			m_pool.Return(rc);
		}

		public void OnDeviceInit()
		{
			m_pool = new MyConcurrentPool<MyRenderContext>(48, null, 48);
			m_tmpList.Clear();
			int count = m_pool.Count;
			for (int i = 0; i < count; i++)
			{
				MyRenderContext myRenderContext = m_pool.Get();
				m_tmpList.Add(myRenderContext);
				myRenderContext.Initialize();
			}
			foreach (MyRenderContext tmp in m_tmpList)
			{
				m_pool.Return(tmp);
			}
			m_tmpList.Clear();
			m_isDeviceInit = true;
		}

		public void OnDeviceEnd()
		{
			if (!m_isDeviceInit)
			{
				return;
			}
			m_isDeviceInit = false;
			m_tmpList.Clear();
			for (int i = 0; i < m_pool.Count; i++)
			{
				MyRenderContext myRenderContext = m_pool.Get();
				m_tmpList.Add(myRenderContext);
				myRenderContext.Dispose();
			}
			foreach (MyRenderContext tmp in m_tmpList)
			{
				m_pool.Return(tmp);
			}
			m_tmpList.Clear();
		}

		public void OnDeviceReset()
		{
			OnDeviceEnd();
			OnDeviceInit();
		}

		public void OnUnloadData()
		{
			m_tmpList.Clear();
			for (int i = 0; i < m_pool.Count; i++)
			{
				MyRenderContext myRenderContext = m_pool.Get();
				myRenderContext.UnloadData();
				m_tmpList.Add(myRenderContext);
			}
			foreach (MyRenderContext tmp in m_tmpList)
			{
				m_pool.Return(tmp);
			}
			m_tmpList.Clear();
		}

		public string GetLastAnnotations()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string marker = "";
			lock (m_acquired)
			{
				foreach (MyRenderContext key in m_acquired.Keys)
				{
					key.GetLastAnnotation(ref marker);
					stringBuilder.Append(string.Format("{0}@{1}{2} ", key.DebugName, marker, key.WasRunning ? "*" : ""));
				}
			}
			return stringBuilder.ToString();
		}
	}
}
