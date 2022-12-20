using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Render11.Common;
using VRage.Render11.Profiler;
using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	[GenerateActivator]
	internal class MyRenderingWork
	{
		internal struct MyItem
		{
			internal MyRenderingPass Pass;

			internal MyRenderCullResultBatch DecalsBatch;

			internal MyList<MyRenderCullResultBatch> Batches;

			internal MyList<MyRenderCullResultFlat> Renderables;

			internal MyRenderableProxy_2[] List2;
		}

		private class VRageRender_MyRenderingWork_003C_003EActor : IActivator, IActivator<MyRenderingWork>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderingWork();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderingWork CreateInstance()
			{
				return new MyRenderingWork();
			}

			MyRenderingWork IActivator<MyRenderingWork>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		internal List<MyRenderingPass> Passes = new List<MyRenderingPass>();

		private MyItem m_workItem;

		private const int WORK_BATCH_SIZE = 25;

		private int m_workCounter;

		private int m_workCounter2;

		private int m_startedCounter;

		private bool m_workDone;

		private MyRenderingPass m_decalsJob;

		internal MyItem WorkItem => m_workItem;

		public int DoWork()
		{
			if (Interlocked.Exchange(ref m_startedCounter, 1) == 0)
			{
				return Render(m_workItem.Pass);
			}
			MyRenderingPass myRenderingPass = m_workItem.Pass.Fork();
			lock (Passes)
			{
				Passes.Add(myRenderingPass);
			}
			return Render(myRenderingPass);
		}

		private int Render(MyRenderingPass pass)
		{
			pass.Begin();
			int num = 0;
			MyList<MyRenderCullResultBatch> batches = m_workItem.Batches;
			MyList<MyRenderCullResultFlat> renderables = m_workItem.Renderables;
			pass.Locals.BindConstantBuffersBatched &= MyRender11.BatchedConstantBufferMapping;
			bool bindConstantBuffersBatched = pass.Locals.BindConstantBuffersBatched;
			IConstantBuffer constantBuffer = null;
			MyConcurrentBufferPool<IConstantBuffer, MyGeometryRendererOld.MyConstantBufferAllocator> constantBufferPool = MyManagers.GeometryRendererOld.ConstantBufferPool;
			while (true)
			{
				int num2 = Interlocked.Increment(ref m_workCounter) - 1;
				MyRenderCullResultBatch myRenderCullResultBatch;
				if (num2 >= batches.Count)
				{
					if (pass.IsImmediate || m_workItem.DecalsBatch.Count <= 0 || Interlocked.CompareExchange(ref m_decalsJob, pass, null) != null)
					{
						break;
					}
					myRenderCullResultBatch = m_workItem.DecalsBatch;
				}
				else
				{
					myRenderCullResultBatch = batches[num2];
				}
				int begin = myRenderCullResultBatch.Begin;
				int num3 = myRenderCullResultBatch.Begin + myRenderCullResultBatch.Count;
				int num4 = 0;
				if (bindConstantBuffersBatched)
				{
					if (constantBuffer == null || MathHelper.GetNearestBiggerPowerOfTwo(myRenderCullResultBatch.CBSize) != constantBuffer.ByteSize)
					{
						if (constantBuffer != null)
						{
							constantBufferPool.Return(constantBuffer);
						}
						constantBuffer = constantBufferPool.Get(myRenderCullResultBatch.CBSize);
					}
					MyMapping mapping = MyMapping.MapDiscard(pass.RC, constantBuffer);
					for (int i = begin; i < num3; i++)
					{
						mapping.Position(num4);
						renderables[i].RenderProxy.UpdateObjectBuffer(ref mapping);
						num4 += renderables[i].ObjectBufferSize;
					}
					mapping.Unmap();
				}
				num4 = 0;
				for (int j = begin; j < num3; j++)
				{
					MyRenderCullResultFlat myRenderCullResultFlat = renderables[j];
					pass.RecordCommands(myRenderCullResultFlat.RenderProxy, constantBuffer, num4);
					num4 += myRenderCullResultFlat.ObjectBufferSize;
					num++;
				}
			}
			if (constantBuffer != null)
			{
				constantBufferPool.Return(constantBuffer);
			}
			if (m_workItem.List2 != null)
			{
				int num5 = m_workItem.List2.Length;
				while (true)
				{
					int workCounter = m_workCounter2;
					if (workCounter >= num5)
					{
						break;
					}
					if (Interlocked.CompareExchange(ref m_workCounter2, workCounter + 25, workCounter) == workCounter)
					{
						int num6 = Math.Min(workCounter, num5);
						int num7 = Math.Min(workCounter + 25, num5);
						for (int k = num6; k < num7; k++)
						{
							pass.RecordCommands(ref m_workItem.List2[k]);
							num++;
						}
					}
				}
			}
			m_workDone = true;
			pass.End();
			return num;
		}

		public void Cleanup()
		{
			lock (Passes)
			{
				for (int i = 1; i < Passes.Count; i++)
				{
					MyObjectPoolManager.Deallocate(Passes[i]);
				}
				Passes.Clear();
			}
			m_workCounter = 0;
			m_workCounter2 = 0;
			m_startedCounter = 0;
			m_workDone = false;
		}

		public void Setup(MyItem item)
		{
			m_workItem = item;
			lock (Passes)
			{
				Passes.Add(item.Pass);
			}
		}

		public bool WorkDone()
		{
			return m_workDone;
		}

		public void ConsumeWork()
		{
			lock (Passes)
			{
				if (!m_workDone)
				{
					MyRenderProxy.Log.WriteLine("Work not done! " + m_startedCounter + " / " + Passes.Count);
				}
				foreach (MyRenderingPass pass in Passes)
				{
					if (pass != m_decalsJob)
					{
						ConsumePass(pass);
					}
				}
				if (m_decalsJob != null)
				{
					ConsumePass(m_decalsJob);
					m_decalsJob = null;
				}
			}
<<<<<<< HEAD
			void ConsumePass(MyRenderingPass pass)
			{
				MyGpuProfiler.IC_BeginBlock(pass.DebugName, "ConsumeWork", "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage\\RenderingWork\\MyRenderingWork.cs");
				pass.ExecuteCommandList(MyRender11.RC);
				MyGpuProfiler.IC_EndBlock(0f, "ConsumeWork", "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage\\RenderingWork\\MyRenderingWork.cs");
=======
			static void ConsumePass(MyRenderingPass pass)
			{
				MyGpuProfiler.IC_BeginBlock(pass.DebugName, "ConsumeWork", "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage\\RenderingWork\\MyRenderingWork.cs");
				pass.ExecuteCommandList(MyRender11.RC);
				MyGpuProfiler.IC_EndBlock(0f, "ConsumeWork", "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage\\RenderingWork\\MyRenderingWork.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
}
