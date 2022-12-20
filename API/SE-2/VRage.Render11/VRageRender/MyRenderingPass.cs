#define VRAGE
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Scene.Components;
using VRage.Render11.Tools;
using VRageMath;
using VRageRender.Import;

namespace VRageRender
{
	internal abstract class MyRenderingPass : IPooledObject
	{
		internal MyPassLocals Locals;

		internal MyRendererStats.MyRenderStats Stats;

		internal long Elapsed;

		internal Matrix ViewProjection;

		internal Matrix Projection;

		internal MyViewport Viewport;

		internal string DebugName;

		internal int ViewId;

		private MyRenderContext m_rc;

		private bool m_ready;

		private MyMaterialType m_currentProfilingBlockRenderableType = MyMaterialType.INVALID;

		private MyMeshDrawTechnique m_currentProfilingBlockRenderableMaterial = MyMeshDrawTechnique.COUNT;

		private bool m_isImmediate;

		private long m_started;

		private MyFinishedContext m_finishedContext;

		private bool m_rcOwner;

		private (Buffer[], int[], int[]) m_constantBindingsCache;

		internal int ProcessingMask { get; set; }

		internal bool IsImmediate => m_isImmediate;

		internal MyRenderContext RC => m_rc;

		public void SetDeferred(MyRenderContext rc)
		{
			m_rc = rc;
			m_rcOwner = false;
			m_isImmediate = false;
		}

		internal void SetImmediate(bool isImmediate)
		{
			m_isImmediate = isImmediate;
			m_rc = (isImmediate ? MyImmediateRC.RC : null);
		}

		void IPooledObject.Cleanup()
		{
			Cleanup();
		}

		internal virtual void Cleanup()
		{
			m_rc = null;
			m_rcOwner = false;
			Locals.Clear();
			Stats = default(MyRendererStats.MyRenderStats);
			m_ready = true;
			m_currentProfilingBlockRenderableType = MyMaterialType.INVALID;
			m_currentProfilingBlockRenderableMaterial = MyMeshDrawTechnique.COUNT;
			m_isImmediate = false;
			ViewProjection = default(Matrix);
			Projection = default(Matrix);
			Viewport = default(MyViewport);
			DebugName = string.Empty;
			ProcessingMask = 0;
			ViewId = 0;
			m_finishedContext = default(MyFinishedContext);
		}

		internal virtual void Begin()
		{
			m_started = Stopwatch.GetTimestamp();
			Elapsed = 0L;
			m_ready = false;
			if (!m_isImmediate && m_rc == null)
			{
				m_rc = MyManagers.DeferredRCs.AcquireRC(DebugName);
				m_rcOwner = true;
			}
			ProfileBlock(start: true);
			Locals.Clear();
			Matrix data = Matrix.Transpose(ViewProjection);
			MyMapping myMapping = MyMapping.MapDiscard(RC, MyCommon.ProjectionConstants);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			RC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			RC.SetViewport(ref Viewport);
			RC.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			RC.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstants);
			RC.AllShaderStages.SetConstantBuffer(1, MyCommon.ProjectionConstants);
			RC.AllShaderStages.SetConstantBuffer(6, MyCommon.VoxelMaterialsConstants.Cb);
			RC.AllShaderStages.SetConstantBuffer(5, MyCommon.AlphamaskViewsConstants);
			RC.PixelShader.SetSrvs(10, MyGlobalResources.FileArrayTextureVoxelCM, MyGlobalResources.FileArrayTextureVoxelNG, MyGlobalResources.FileArrayTextureVoxelExt, MyGlobalResources.FileArrayTextureVoxelCMLow, MyGlobalResources.FileArrayTextureVoxelNGLow, MyGlobalResources.FileArrayTextureVoxelExtLow);
			RC.PixelShader.SetSrv(28, MyGeneratedTextureManager.Dithering8x8Tex);
			RC.PixelShader.SetSrv(29, MyGeneratedTextureManager.RandomTex);
			if (MyBigMeshTable.Table.m_IB != null)
			{
				int num = 10;
				RC.VertexShader.SetSrv(num++, MyBigMeshTable.Table.m_IB);
				RC.VertexShader.SetSrv(num++, MyBigMeshTable.Table.m_VB_positions);
				RC.VertexShader.SetSrv(num++, MyBigMeshTable.Table.m_VB_rest);
			}
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		internal void FeedProfiler(MyRenderableProxy proxy, bool next = true)
		{
			MyMaterialType myMaterialType;
			MyMeshDrawTechnique myMeshDrawTechnique;
			if (proxy != null)
			{
				myMaterialType = proxy.Type;
				myMeshDrawTechnique = proxy.Technique;
			}
			else
			{
				myMaterialType = MyMaterialType.INVALID;
				myMeshDrawTechnique = MyMeshDrawTechnique.COUNT;
			}
			if (myMaterialType != m_currentProfilingBlockRenderableType || !next)
			{
				_ = m_currentProfilingBlockRenderableType;
				_ = -1;
				if (next)
				{
					m_currentProfilingBlockRenderableType = myMaterialType;
					m_currentProfilingBlockRenderableMaterial = myMeshDrawTechnique;
				}
			}
			else if (myMeshDrawTechnique != m_currentProfilingBlockRenderableMaterial)
			{
				_ = m_currentProfilingBlockRenderableMaterial;
				_ = 22;
				if (next)
				{
					m_currentProfilingBlockRenderableMaterial = myMeshDrawTechnique;
				}
			}
		}

		internal void RecordCommands(MyRenderableProxy proxy, IConstantBuffer cb, int constantOffset)
		{
			if (true)
			{
				if (proxy.Mesh.Buffers.IB != null)
				{
					RC.SetEventMarker(proxy.Mesh.Info.FileName);
				}
				if (Locals.BindConstantBuffersBatched)
				{
					SetProxyConstantsBatched(cb, constantOffset, proxy.ObjectBufferSizeAligned);
				}
				RecordCommandsInternal(proxy);
			}
		}

		protected virtual void RecordCommandsInternal(MyRenderableProxy proxy)
		{
		}

		internal void RecordCommands(ref MyRenderableProxy_2 proxy, int instance = -1, int section = -1)
		{
			RecordCommandsInternal(ref proxy, instance, section);
		}

		protected virtual void RecordCommandsInternal(ref MyRenderableProxy_2 proxy, int instance, int section)
		{
		}

		private void ProfileBlock(bool start)
		{
		}

		internal virtual void End()
		{
			m_rc.InvalidateVertexBufferBindings(0, 3);
			ProfileBlock(start: false);
			if (!m_isImmediate)
			{
				if (m_rcOwner)
				{
					m_finishedContext = m_rc.FinishDeferredContext();
					m_rcOwner = false;
				}
				m_rc = null;
			}
			else
			{
				FlushStats();
			}
			Elapsed = Stopwatch.GetTimestamp() - m_started;
			m_ready = true;
		}

		private void FlushStats()
		{
			MyRendererStats.AddViewRenderStats(ViewId, ref Stats);
		}

		protected unsafe void SetProxyConstants(ref MyRenderableProxy_2 proxy, MyMergeInstancingConstants? arg = null)
		{
			MyMergeInstancingConstants myMergeInstancingConstants = arg ?? MyMergeInstancingConstants.Default;
			int hashCode = myMergeInstancingConstants.GetHashCode();
			if (myMergeInstancingConstants.GetHashCode() != proxy.ObjectConstants.Version)
			{
				int num = sizeof(MyMergeInstancingConstants);
				byte[] array = new byte[sizeof(MyMergeInstancingConstants)];
				proxy.ObjectConstants = new MyConstantsPack
				{
					BindFlag = MyBindFlag.BIND_VS,
					CB = RC.GetObjectCB(num),
					Version = hashCode,
					Data = array
				};
				fixed (byte* destination = array)
				{
					Unsafe.CopyBlockUnaligned(destination, &myMergeInstancingConstants, (uint)num);
				}
			}
			MyRenderUtils.SetConstants(RC, ref proxy.ObjectConstants, 2);
		}

		protected void SetProxyConstants(MyRenderableProxy proxy)
		{
			if (!Locals.BindConstantBuffersBatched)
			{
				IConstantBuffer constantBuffer = proxy.UpdateObjectBuffer(RC);
				RC.VertexShader.SetConstantBuffer(2, constantBuffer);
				RC.PixelShader.SetConstantBuffer(2, constantBuffer);
			}
		}

		protected void SetProxyConstantsBatched(IConstantBuffer cb, int offset, int size)
		{
			var (array, array2, array3) = m_constantBindingsCache;
			if (array == null)
			{
				array3 = new int[1];
				array2 = new int[1];
				array = new Buffer[1];
				m_constantBindingsCache = (array, array2, array3);
			}
			array[0] = cb.Buffer;
			array2[0] = offset / 16;
			array3[0] = size / 16;
			DeviceContext1 deviceContext = RC.DeviceContext;
			deviceContext.VSSetConstantBuffers1(2, 1, array, array2, array3);
			deviceContext.PSSetConstantBuffers1(2, 1, array, array2, array3);
		}

		internal void BindProxyGeometry(MyRenderableProxy proxy)
		{
			MyRenderContext rC = RC;
			LodMeshId mesh = proxy.Mesh;
			MyMeshBuffers buffers = mesh.Buffers;
			IVertexBuffer vertexBuffer = (proxy.InstancingEnabled ? proxy.Instancing.VB : null);
			if (Locals.BoundMeshIndex != mesh.Index)
			{
				Locals.BoundMeshIndex = mesh.Index;
				rC.SetVertexBuffersFast(0, buffers.VB0, buffers.VB1, vertexBuffer);
				rC.SetIndexBuffer(buffers.IB);
			}
			else if (vertexBuffer != null)
			{
				rC.DeviceContext.InputAssembler.SetVertexBuffers(2, new VertexBufferBinding(vertexBuffer.Buffer, vertexBuffer.Description.StructureByteStride, 0));
			}
		}

		[Conditional("DEBUG")]
		private void FilterRenderable(MyRenderableProxy proxy, ref bool draw)
		{
			if (proxy.Material == MyMeshMaterialId.NULL)
			{
				if (proxy.VoxelCommonObjectData.IsValid)
				{
					draw &= MyRender11.Settings.DrawVoxels;
				}
				return;
			}
			switch (proxy.Material.Info.Technique)
			{
			case MyMeshDrawTechnique.MESH:
				if (proxy.InstanceCount == 0)
				{
					draw &= MyRender11.Settings.DrawMeshes;
				}
				else
				{
					draw &= MyRender11.Settings.DrawInstancedMeshes;
				}
				break;
			case MyMeshDrawTechnique.ALPHA_MASKED:
			case MyMeshDrawTechnique.ALPHA_MASKED_SINGLE_SIDED:
				draw &= MyRender11.Settings.DrawAlphamasked;
				if (proxy.Material.Info.Facing == MyFacingEnum.Impostor)
				{
					draw &= MyRender11.Settings.DrawImpostors;
				}
				break;
			}
		}

		internal virtual MyRenderingPass Fork()
		{
			MyRenderingPass myRenderingPass = MyObjectPoolManager.Allocate<MyRenderingPass>(GetType());
			myRenderingPass.m_isImmediate = m_isImmediate;
			myRenderingPass.ViewProjection = ViewProjection;
			myRenderingPass.Projection = Projection;
			myRenderingPass.Viewport = Viewport;
			myRenderingPass.ViewId = ViewId;
			myRenderingPass.DebugName = DebugName;
			myRenderingPass.ProcessingMask = ProcessingMask;
			return myRenderingPass;
		}

		public void ExecuteCommandList(MyRenderContext immediateRC)
		{
			if (m_finishedContext.CommandList == null)
			{
				MyRenderProxy.Log.WriteLine($"{m_ready} {m_isImmediate} {m_rcOwner} {ViewId} {DebugName}");
			}
<<<<<<< HEAD
			immediateRC.ExecuteContext(ref m_finishedContext, "ExecuteCommandList", 448, "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage\\MyRenderingPass.cs");
=======
			immediateRC.ExecuteContext(ref m_finishedContext, "ExecuteCommandList", 448, "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage\\MyRenderingPass.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			FlushStats();
		}
	}
}
