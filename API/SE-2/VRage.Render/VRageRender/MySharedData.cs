using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Library.Threading;
using VRage.Library.Utils;

namespace VRageRender
{
	/// <summary>
	/// Data shared between render and update
	/// </summary>
	public class MySharedData
	{
		private readonly SpinLockRef m_lock = new SpinLockRef();

		private readonly MySwapQueue<HashSet<uint>> m_outputVisibleObjects = MySwapQueue.Create<HashSet<uint>>();

		private readonly MyMessageQueue m_outputRenderMessages = new MyMessageQueue();

		private readonly MyUpdateData m_inputRenderMessages = new MyUpdateData();

		private readonly MySwapQueue<MyBillboardBatch<MyBillboard>> m_inputBillboards = MySwapQueue.Create<MyBillboardBatch<MyBillboard>>();

		private readonly MySwapQueue<MyBillboardBatch<MyTriangleBillboard>> m_inputTriangleBillboards = MySwapQueue.Create<MyBillboardBatch<MyTriangleBillboard>>();

		private readonly ConcurrentCachingLinkedList<MyBillboard> m_persistentBillboards = new ConcurrentCachingLinkedList<MyBillboard>(delegate(MyBillboard x, LinkedListNode<MyBillboard> node)
		{
			x.Node = node;
		}, (MyBillboard x) => x.Node);

		public MyUpdateFrame MessagesForNextFrame = new MyUpdateFrame();

		public MySwapQueue<MyBillboardBatch<MyBillboard>> Billboards => m_inputBillboards;

		public MySwapQueue<MyBillboardBatch<MyTriangleBillboard>> TriangleBillboards => m_inputTriangleBillboards;

		public MySwapQueue<HashSet<uint>> VisibleObjects => m_outputVisibleObjects;

		public MyUpdateFrame CurrentUpdateFrame => m_inputRenderMessages.CurrentUpdateFrame;

		public MyMessageQueue RenderOutputMessageQueue => m_outputRenderMessages;

		public int PersistentBillboardsCount => m_persistentBillboards.Count;

		/// <summary>
		/// Refresh data from render (visible objects, render messages)
		/// </summary>
		public void BeforeUpdate()
		{
			using (m_lock.Acquire())
			{
				m_outputVisibleObjects.RefreshRead();
				m_outputRenderMessages.Commit();
			}
		}

		public void AfterUpdate(MyTimeSpan? updateTimestamp, bool gate = true)
		{
			using (m_lock.Acquire())
			{
				if (updateTimestamp.HasValue)
				{
					m_inputRenderMessages.CurrentUpdateFrame.UpdateTimestamp = updateTimestamp.Value;
				}
				m_inputRenderMessages.CommitUpdateFrame();
				m_inputBillboards.CommitWrite();
				m_inputBillboards.Write.Clear();
				m_inputTriangleBillboards.CommitWrite();
				m_inputTriangleBillboards.Write.Clear();
			}
			if (gate)
			{
				m_inputRenderMessages.OverflowGate();
			}
		}

		public void BeforeRender(MyTimeSpan? currentDrawTime)
		{
			using (m_lock.Acquire())
			{
				m_persistentBillboards.ApplyChanges();
				if (currentDrawTime.HasValue)
				{
					MyRenderProxy.CurrentDrawTime = currentDrawTime.Value;
				}
			}
		}

		public MyUpdateFrame GetDrawRenderFrame()
		{
			using (m_lock.Acquire())
			{
				MyUpdateFrame drawRenderFrame = m_inputRenderMessages.GetDrawRenderFrame();
				m_inputBillboards.RefreshRead();
				return drawRenderFrame;
			}
		}

		public MyUpdateFrame GetRenderFrame(out bool isPreFrame)
		{
			using (m_lock.Acquire())
			{
				MyUpdateFrame renderFrame = m_inputRenderMessages.GetRenderFrame(out isPreFrame);
				if (!isPreFrame)
				{
					m_inputBillboards.RefreshRead();
					m_inputTriangleBillboards.RefreshRead();
				}
				return renderFrame;
			}
		}

		public void ReturnPreFrame(MyUpdateFrame frame)
		{
			m_inputRenderMessages.ReturnPreFrame(frame);
		}

		public void WaitForNextFrame()
		{
			m_inputRenderMessages.WaitForNextFrame = true;
		}

		public void AfterRender()
		{
			using (m_lock.Acquire())
			{
				m_outputVisibleObjects.CommitWrite();
				m_outputVisibleObjects.Write.Clear();
			}
		}

		public MyBillboard AddPersistentBillboard(MyBillboard billboard = null)
		{
			billboard = billboard ?? new MyBillboard();
			m_persistentBillboards.Add(billboard);
			return billboard;
		}

		public void RemovePersistentBillboard(MyBillboard billboard, bool immediate = false)
		{
			m_persistentBillboards.Remove(billboard, immediate);
		}

		public void ApplyActionOnPersistentBillboards(Action<MyBillboard> action)
		{
			foreach (MyBillboard persistentBillboard in m_persistentBillboards)
			{
				action(persistentBillboard);
			}
		}

		public void ApplyActionOnPersistentBillboards(Action action)
		{
			m_persistentBillboards.Invoke(action);
		}

		public void ClearPersistentBillboards()
		{
			m_persistentBillboards.ClearImmediate();
		}
	}
}
