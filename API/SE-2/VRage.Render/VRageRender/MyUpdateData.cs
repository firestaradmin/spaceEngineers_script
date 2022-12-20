using System.Threading;
using VRage.Collections;
using VRage.Library.Utils;

namespace VRageRender
{
	internal class MyUpdateData
	{
		private readonly bool MY_FAKE__ENABLE_FRAMES_OVERFLOW_BARRIER = true;

		private const int OVERFLOW_THRESHOLD = 60;

		private readonly ManualResetEvent m_overflowGate = new ManualResetEvent(initialState: true);

		private readonly MyConcurrentPool<MyUpdateFrame> m_frameDataPool;

		private readonly MyConcurrentQueue<MyUpdateFrame> m_updateDataQueue;

		public bool WaitForNextFrame;

		private MyUpdateFrame m_lastFrame;

		public MyUpdateFrame CurrentUpdateFrame { get; private set; }

		public MyUpdateData()
		{
			m_frameDataPool = new MyConcurrentPool<MyUpdateFrame>(5);
			m_updateDataQueue = new MyConcurrentQueue<MyUpdateFrame>(5);
			CurrentUpdateFrame = m_frameDataPool.Get();
		}

		/// <summary>
		/// Commits current frame as atomic operation and prepares new frame
		/// </summary>
		public void CommitUpdateFrame()
		{
			MyTimeSpan updateTimestamp = CurrentUpdateFrame.UpdateTimestamp;
			CurrentUpdateFrame.Processed = false;
			CurrentUpdateFrame.Cleared = false;
			m_lastFrame = CurrentUpdateFrame;
			m_updateDataQueue.Enqueue(CurrentUpdateFrame);
			MyUpdateFrame myUpdateFrame2 = (CurrentUpdateFrame = m_frameDataPool.Get());
			CurrentUpdateFrame.UpdateTimestamp = updateTimestamp;
		}

		public void OverflowGate()
		{
			if ((MY_FAKE__ENABLE_FRAMES_OVERFLOW_BARRIER && m_updateDataQueue.Count > 60) || WaitForNextFrame)
			{
				_ = m_updateDataQueue.Count;
				m_overflowGate.Reset();
				m_overflowGate.WaitOne();
				if (WaitForNextFrame)
				{
					m_overflowGate.Reset();
					m_overflowGate.WaitOne();
				}
				WaitForNextFrame = false;
			}
		}

		/// <summary>
		/// Gets next frame for rendering, can return null in case there's nothing for rendering (no update frame submitted).
		/// When isPreFrame is true, don't handle draw messages, just process update messages and call method again.
		/// Pre frame must release messages and must be returned.
		/// Final frame is kept unmodified in queue, in case of slower update, so we can interpolate and draw frame again.
		/// </summary>
		public MyUpdateFrame GetRenderFrame(out bool isPreFrame)
		{
			if (m_updateDataQueue.Count > 1)
			{
				isPreFrame = true;
				return m_updateDataQueue.Dequeue();
			}
			isPreFrame = false;
			m_overflowGate.Set();
			return m_lastFrame;
		}

		public MyUpdateFrame GetDrawRenderFrame()
		{
			if (!m_updateDataQueue.TryPeek(out var instance))
			{
				return null;
			}
			return instance;
		}

		/// <summary>
		/// PreFrame must be empty in this place
		/// </summary>
		public void ReturnPreFrame(MyUpdateFrame frame)
		{
			m_frameDataPool.Return(frame);
		}
	}
}
