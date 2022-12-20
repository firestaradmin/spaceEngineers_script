using System.Collections.Concurrent;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using VRage.Collections;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	internal class MyShadowCascadesStats
	{
		private struct Issued
		{
			private MyGenericBuffer m_readBuffer;

			public int ViewId;

			public static Issued Issue(MyRenderContext rc, ISrvUavBuffer pixelCountsUav, int viewId)
			{
				Issued issued = default(Issued);
				issued.m_readBuffer = m_readBufferPool.Get(32);
				issued.ViewId = viewId;
				Issued result = issued;
				result.Copy(rc, pixelCountsUav);
				return result;
			}

			public bool IsValid()
			{
				return m_readBuffer != null;
			}

			public bool IsFinished()
			{
				if (MyRender11.RCForQueries.GetData<int>((Query)m_readBuffer.Query, AsynchronousFlags.DoNotFlush, out var result))
				{
					return result > 0;
				}
				return false;
			}

			public void Retrieve(uint[] pixelCounts)
			{
				MyMapping myMapping = MyMapping.MapRead(m_readBuffer);
				myMapping.ReadAndPosition(pixelCounts, 8);
				myMapping.Unmap();
			}

			public void Return()
			{
				m_readBufferPool.Return(m_readBuffer);
				m_readBuffer = null;
			}

			private void Copy(MyRenderContext rc, ISrvUavBuffer pixelCountsUav)
			{
				rc.CopyResource(pixelCountsUav, m_readBuffer);
				rc.End((Query)m_readBuffer.Query);
			}
		}

		private MyComputeShaders.Id m_statsCS = MyComputeShaders.Id.NULL;

		private ISrvUavBuffer PixelCountsUav;

		private readonly uint[,] m_pixelCountsPerView = new uint[19, 8];

		private readonly uint[] m_pixelCountsTemp = new uint[8];

		private static readonly MyConcurrentBucketPool<MyGenericBuffer> m_readBufferPool = new MyConcurrentBucketPool<MyGenericBuffer>("ShadowCascadesStatsPool", new MyGenericBufferAllocator(createQuery: true, new BufferDescription(32, ResourceUsage.Staging, BindFlags.None, CpuAccessFlags.Read, ResourceOptionFlags.None, 4), MyManagers.Buffers.BufferMemoryTacker.RegisterSubsystem("ShadowCascadesStatsBuffers")));

		private readonly ConcurrentQueue<Issued> m_issued = new ConcurrentQueue<Issued>();

		private readonly Issued[] m_finishedItems = new Issued[19];

		public uint[] PixelCounts { get; } = new uint[8];


		public void Init()
		{
			Done();
			m_statsCS = MyComputeShaders.Create("Shadows/ShadowStats.hlsl");
			PixelCountsUav = MyManagers.Buffers.CreateSrvUav("CascadeStats", 8, 4, null, MyUavType.Default, ResourceUsage.Default, isGlobal: true);
			for (int i = 0; i < 8; i++)
			{
				PixelCounts[i] = 0u;
				for (int j = 0; j < 19; j++)
				{
					m_pixelCountsPerView[j, i] = 0u;
				}
			}
			for (int k = 0; k < 19; k++)
			{
				m_finishedItems[k].ViewId = -1;
			}
		}

		public void Done()
		{
<<<<<<< HEAD
			Issued result;
			while (m_issued.TryDequeue(out result))
			{
				result.Return();
=======
			Issued issued = default(Issued);
			while (m_issued.TryDequeue(ref issued))
			{
				issued.Return();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			MyManagers.Buffers.Dispose(PixelCountsUav);
			m_readBufferPool.Clear();
		}

		public void IssueCopy(MyRenderContext rc, int viewId)
		{
			if (MyRender11.Settings.ShadowCascadeUsageBasedSkip)
			{
				m_issued.Enqueue(Issued.Issue(rc, PixelCountsUav, viewId));
			}
		}

		public void Update()
		{
			bool flag = false;
<<<<<<< HEAD
			Issued result;
			while (m_issued.TryPeek(out result) && result.IsFinished())
			{
				if (m_finishedItems[result.ViewId].IsValid())
				{
					m_finishedItems[result.ViewId].Return();
				}
				m_finishedItems[result.ViewId] = result;
				m_issued.TryDequeue(out result);
=======
			Issued issued = default(Issued);
			while (m_issued.TryPeek(ref issued) && issued.IsFinished())
			{
				if (m_finishedItems[issued.ViewId].IsValid())
				{
					m_finishedItems[issued.ViewId].Return();
				}
				m_finishedItems[issued.ViewId] = issued;
				m_issued.TryDequeue(ref issued);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			for (int i = 0; i < 19; i++)
			{
				if (m_finishedItems[i].IsValid())
				{
					int viewId = m_finishedItems[i].ViewId;
					m_finishedItems[i].Retrieve(m_pixelCountsTemp);
					m_finishedItems[i].Return();
					for (int j = 0; j < 8; j++)
					{
						int num = (int)(m_pixelCountsTemp[j] - m_pixelCountsPerView[viewId, j]);
						int num2 = (int)PixelCounts[j];
						PixelCounts[j] = (uint)(num2 + num);
						m_pixelCountsPerView[viewId, j] = m_pixelCountsTemp[j];
					}
				}
			}
		}

		private static Vector2I GetThreadGroupCount(Vector2I viewportRes)
		{
			Vector2I vector2I = new Vector2I(16, 16);
			Vector2I result = new Vector2I(viewportRes.X / vector2I.X, viewportRes.Y / vector2I.Y);
			result.X += ((viewportRes.X % vector2I.X != 0) ? 1 : 0);
			result.Y += ((viewportRes.Y % vector2I.Y != 0) ? 1 : 0);
			return result;
		}

		public void Gather(MyRenderContext rc, IConstantBuffer cascadeConstantBuffer, ref MyCommon.MyScreenLayout layout, ISrvBindable srvDepth, int viewId)
		{
			IConstantBuffer objectCB = rc.GetObjectCB(MyCommon.SCREEN_LAYOUT_SIZE);
			MyMapping myMapping = MyMapping.MapDiscard(rc, objectCB);
			myMapping.WriteAndPosition(ref layout);
			myMapping.Unmap();
			rc.ComputeShader.SetConstantBuffer(2, objectCB);
			rc.ComputeShader.Set(m_statsCS);
			rc.ClearUav(PixelCountsUav, default(RawInt4));
			rc.ComputeShader.SetUav(1, PixelCountsUav);
			rc.ComputeShader.SetSrv(0, srvDepth);
			rc.ComputeShader.SetConstantBuffer(4, cascadeConstantBuffer);
			Vector2I threadGroupCount = GetThreadGroupCount((Vector2I)layout.Resolution / 2);
			rc.Dispatch(threadGroupCount.X, threadGroupCount.Y, 1);
			rc.ComputeShader.SetUav(0, null);
			rc.ComputeShader.SetUav(1, null);
			rc.ComputeShader.SetSrv(0, null);
			rc.ComputeShader.SetSrv(1, null);
			IssueCopy(rc, viewId);
		}
	}
}
