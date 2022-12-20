#define VRAGE
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using ParallelTasks;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using VRage.Network;
using VRage.Profiler;
using VRage.Render11.Common;
using VRage.Render11.Profiler;
using VRage.Render11.RenderContext.Internal;
using VRage.Render11.Resources;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Render11.RenderContext
{
	[GenerateActivator]
	internal sealed class MyRenderContext : IDisposable
	{
		private class VRage_Render11_RenderContext_MyRenderContext_003C_003EActor : IActivator, IActivator<MyRenderContext>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderContext();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderContext CreateInstance()
			{
				return new MyRenderContext();
			}

			MyRenderContext IActivator<MyRenderContext>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const string AftermathSymbol = "VRAGE";

		private const string DebugSymbol = "__RANDOM_UNDEFINED_PROFILING_SYMBOL__";

		private DeviceContext1 m_deviceContext;

		private MyVertexStage m_vertexShaderStage = new MyVertexStage();

		private MyGeometryStage m_geometryShaderStage = new MyGeometryStage();

		private MyPixelStage m_pixelShaderStage = new MyPixelStage();

		private MyComputeStage m_computeShaderStage = new MyComputeStage();

		private readonly MyAllShaderStages m_allShaderStages;

		private UserDefinedAnnotation m_annotations;

		private MyFrameProfilingContext m_profilingQueries;

		private bool m_isDeferred;

		private MyRenderContextStatistics m_statistics;

		private readonly MyRenderContextState m_state = new MyRenderContextState();

<<<<<<< HEAD
=======
		[ThreadStatic]
		private static RenderTargetView[] m_tmpRtvs;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static MyRenderContext m_lastContext;

		public string DebugName;

		private bool m_disposed;

		private const int MAX_COMMANDLIST_QUEUE = 40;

		private readonly ConcurrentQueue<CommandList> m_commandListQueue = new ConcurrentQueue<CommandList>();

		private Task m_disposeCommandListsTask;

		private readonly Dictionary<int, IConstantBuffer> m_objectsConstantBuffers = new Dictionary<int, IConstantBuffer>();

		private readonly Dictionary<int, IConstantBuffer> m_materialsConstantBuffers = new Dictionary<int, IConstantBuffer>();

		public bool IsInitialized => m_deviceContext != null;

		public bool IsDeferred => m_isDeferred;

		public DeviceContext1 DeviceContext => m_deviceContext;

		private IntPtr NativePointer => m_deviceContext.NativePointer;

		internal MyVertexStage VertexShader => m_vertexShaderStage;

		internal MyGeometryStage GeometryShader => m_geometryShaderStage;

		internal MyPixelStage PixelShader => m_pixelShaderStage;

		internal MyComputeStage ComputeShader => m_computeShaderStage;

		internal MyAllShaderStages AllShaderStages => m_allShaderStages;

		public bool WasRunning => m_lastContext == this;

		public MyRenderContext()
		{
			m_allShaderStages = new MyAllShaderStages(m_vertexShaderStage, m_geometryShaderStage, m_pixelShaderStage, m_computeShaderStage);
		}

		internal void Initialize(DeviceContext1 context = null)
		{
			if (context == null)
			{
				context = new DeviceContext1(new DeviceContext(MyRender11.DeviceInstance).NativePointer);
				m_isDeferred = true;
				m_profilingQueries = MyObjectPoolManager.Allocate<MyFrameProfilingContext>();
			}
			else
			{
				m_isDeferred = false;
			}
			m_statistics = MyObjectPoolManager.Allocate<MyRenderContextStatistics>();
			m_deviceContext = context;
			m_vertexShaderStage.Init(m_deviceContext, m_deviceContext.VertexShader, m_statistics);
			m_geometryShaderStage.Init(m_deviceContext, m_deviceContext.GeometryShader, m_statistics);
			m_pixelShaderStage.Init(m_deviceContext, m_deviceContext.PixelShader, m_statistics);
			m_computeShaderStage.Init(m_deviceContext, m_deviceContext.ComputeShader, m_statistics);
			m_state.Init(m_deviceContext, m_statistics);
			SetEventMarker("Init");
			if (m_annotations == null)
			{
				m_annotations = (UserDefinedAnnotation)MyVRage.Platform.Render.CreateRenderAnnotation(m_deviceContext);
			}
			m_statistics.Clear();
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (m_disposed)
			{
				return;
			}
			foreach (KeyValuePair<int, IConstantBuffer> objectsConstantBuffer in m_objectsConstantBuffers)
			{
				MyManagers.Buffers.Dispose(objectsConstantBuffer.Value);
			}
			m_objectsConstantBuffers.Clear();
			foreach (KeyValuePair<int, IConstantBuffer> materialsConstantBuffer in m_materialsConstantBuffers)
			{
				MyManagers.Buffers.Dispose(materialsConstantBuffer.Value);
			}
			m_materialsConstantBuffers.Clear();
			if (disposing)
			{
				m_vertexShaderStage = null;
				m_geometryShaderStage = null;
				m_pixelShaderStage = null;
				m_computeShaderStage = null;
			}
			if (m_isDeferred)
			{
				try
				{
					m_deviceContext.Dispose();
				}
				catch (Exception ex)
				{
					MyRender11.Log.Log(MyLogSeverity.Error, "Exception disposing device context: {0}", ex.ToString());
					MyRender11.Log.Flush();
				}
			}
			m_disposed = true;
		}

		internal MyRenderContextStatistics GetStatistics()
		{
			return m_statistics;
		}

		internal void ClearStatistics()
		{
			m_statistics.Clear();
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		internal void CheckErrors()
		{
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		internal void BeginDxAnnotationBlock(string tag)
		{
			if (m_annotations != null)
			{
				m_annotations.BeginEvent(tag);
			}
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		internal void EndDxAnnotationBlock()
		{
			if (m_annotations != null)
			{
				m_annotations.EndEvent();
			}
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		internal void SetDxAnnotationMarker(string tag)
		{
			if (m_annotations != null)
			{
				m_annotations.SetMarker(tag);
			}
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		internal void BeginProfilingBlock(string tag, [CallerMemberName] string member = "", [CallerFilePath] string file = "")
		{
			SetEventMarker(tag);
			if (!MyGpuProfiler.Paused)
			{
				MyQuery myQuery = MyQueryFactory.CreateTimestampQuery();
				End((Query)myQuery);
				MyIssuedQuery myIssuedQuery = new MyIssuedQuery(myQuery, tag, MyIssuedQueryEnum.BlockStart, 0f, member, file);
				if (m_isDeferred)
				{
					m_profilingQueries.Issued.Enqueue(myIssuedQuery);
				}
				else
				{
					MyGpuProfiler.IC_Enqueue(myIssuedQuery);
				}
			}
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		internal void EndProfilingBlock(float customValue = 0f, [CallerMemberName] string member = "", [CallerFilePath] string file = "")
		{
			if (!MyGpuProfiler.Paused)
			{
				MyQuery myQuery = MyQueryFactory.CreateTimestampQuery();
				End((Query)myQuery);
				MyIssuedQuery myIssuedQuery = new MyIssuedQuery(myQuery, "", MyIssuedQueryEnum.BlockEnd, customValue, member, file);
				if (m_isDeferred)
				{
					m_profilingQueries.Issued.Enqueue(myIssuedQuery);
				}
				else
				{
					MyGpuProfiler.IC_Enqueue(myIssuedQuery);
				}
			}
		}

		internal void BeginProfilingBlockAlways(string tag, [CallerMemberName] string member = "", [CallerFilePath] string file = "")
		{
			SetEventMarker(tag);
			MyQuery myQuery = MyQueryFactory.CreateTimestampQuery();
			End((Query)myQuery);
			MyIssuedQuery myIssuedQuery = new MyIssuedQuery(myQuery, tag, MyIssuedQueryEnum.BlockStart, 0f, member, file);
			if (m_isDeferred)
			{
				m_profilingQueries.Issued.Enqueue(myIssuedQuery);
			}
			else
			{
				MyGpuProfiler.IC_Enqueue(myIssuedQuery);
			}
		}

		internal void EndProfilingBlockAlways(float customValue = 0f, [CallerMemberName] string member = "", [CallerFilePath] string file = "")
		{
			MyQuery myQuery = MyQueryFactory.CreateTimestampQuery();
			End((Query)myQuery);
			MyIssuedQuery myIssuedQuery = new MyIssuedQuery(myQuery, "", MyIssuedQueryEnum.BlockEnd, customValue, member, file);
			if (m_isDeferred)
			{
				m_profilingQueries.Issued.Enqueue(myIssuedQuery);
			}
			else
			{
				MyGpuProfiler.IC_Enqueue(myIssuedQuery);
			}
		}

		internal void Begin(Asynchronous asyncRef)
		{
			m_deviceContext.Begin(asyncRef);
		}

		internal void ClearDsv(IDepthStencil ds, DepthStencilClearFlags clearFlags, float depth, byte stencil)
		{
			m_deviceContext.ClearDepthStencilView(ds.Dsv.Dsv, clearFlags, depth, stencil);
		}

		internal void ClearDsv(IDsvBindable dsv, DepthStencilClearFlags clearFlags, float depth, byte stencil)
		{
			m_deviceContext.ClearDepthStencilView(dsv.Dsv, clearFlags, depth, stencil);
		}

		internal void ClearRtv(IRtvBindable rtv, RawColor4 colorRGBA)
		{
			m_deviceContext.ClearRenderTargetView(rtv.Rtv, colorRGBA);
		}

		internal void ClearState()
		{
			m_state.Clear();
			m_vertexShaderStage.ClearState();
			m_geometryShaderStage.ClearState();
			m_pixelShaderStage.ClearState();
			m_computeShaderStage.ClearState();
		}

		internal void ClearUav(IUavBindable uav, RawInt4 values)
		{
			m_deviceContext.ClearUnorderedAccessView(uav.Uav, values);
		}

		internal void CopyResource(IResource source, Resource destination)
		{
			m_deviceContext.CopyResource(source.Resource, destination);
		}

		internal void CopyResource(IResource source, IResource destination)
		{
			CopyResource(source, destination.Resource);
		}

		internal void CopyStructureCount(IBuffer dstBufferRef, int dstAlignedByteOffset, IUavBindable srcViewRef)
		{
			m_deviceContext.CopyStructureCount(dstBufferRef.Buffer, dstAlignedByteOffset, srcViewRef.Uav);
		}

		internal void CopySubresourceRegion(IResource source, int sourceSubresource, ResourceRegion? sourceRegion, Resource destination, int destinationSubResource, int dstX = 0, int dstY = 0, int dstZ = 0)
		{
			m_deviceContext.CopySubresourceRegion(source.Resource, sourceSubresource, sourceRegion, destination, destinationSubResource, dstX, dstY, dstZ);
		}

		internal void CopySubresourceRegion(IResource source, int sourceSubresource, ResourceRegion? sourceRegion, IResource destination, int destinationSubResource, int dstX = 0, int dstY = 0, int dstZ = 0)
		{
			CopySubresourceRegion(source, sourceSubresource, sourceRegion, destination.Resource, destinationSubResource, dstX, dstY, dstZ);
		}

		internal void Draw(int vertexCount, int startVertexLocation)
		{
			m_deviceContext.Draw(vertexCount, startVertexLocation);
			m_statistics.Draws++;
		}

		internal void DrawAuto()
		{
			m_deviceContext.DrawAuto();
			m_statistics.Draws++;
		}

		internal void DrawIndexed(int indexCount, int startIndexLocation, int baseVertexLocation)
		{
			m_deviceContext.DrawIndexed(indexCount, startIndexLocation, baseVertexLocation);
			m_statistics.Draws++;
		}

		internal void DrawInstanced(int vertexCountPerInstance, int instanceCount, int startVertexLocation, int startInstanceLocation)
		{
			m_deviceContext.DrawInstanced(vertexCountPerInstance, instanceCount, startVertexLocation, startInstanceLocation);
			m_statistics.Draws++;
		}

		internal void DrawIndexedInstanced(int indexCountPerInstance, int instanceCount, int startIndexLocation, int baseVertexLocation, int startInstanceLocation)
		{
			m_deviceContext.DrawIndexedInstanced(indexCountPerInstance, instanceCount, startIndexLocation, baseVertexLocation, startInstanceLocation);
			m_statistics.Draws++;
		}

		internal void DrawIndexedInstancedIndirect(IBuffer bufferForArgsRef, int alignedByteOffsetForArgs)
		{
			m_deviceContext.DrawIndexedInstancedIndirect(bufferForArgsRef.Buffer, alignedByteOffsetForArgs);
			m_statistics.Draws++;
		}

		internal void Dispatch(int threadGroupCountX, int threadGroupCountY, int threadGroupCountZ)
		{
			m_deviceContext.Dispatch(threadGroupCountX, threadGroupCountY, threadGroupCountZ);
			m_statistics.Dispatches++;
		}

		internal void DispatchIndirect(IBuffer bufferForArgsRef, int alignedByteOffsetForArgs)
		{
			m_deviceContext.DispatchIndirect(bufferForArgsRef.Buffer, alignedByteOffsetForArgs);
			m_statistics.Dispatches++;
		}

		internal void End(Asynchronous asyncRef)
		{
			m_deviceContext.End(asyncRef);
		}

		internal void ExecuteCommandList(CommandList commandListRef, RawBool restoreContextState)
		{
			m_lastContext = this;
			m_deviceContext.ExecuteCommandList(commandListRef, restoreContextState);
			m_lastContext = null;
		}

		internal CommandList FinishCommandList(bool restoreState)
		{
			m_lastContext = this;
			CommandList result = m_deviceContext.FinishCommandList(restoreState);
			m_lastContext = null;
			return result;
		}

		internal void GenerateMips(ISrvBindable srv)
		{
			m_deviceContext.GenerateMips(srv.Srv);
		}

		internal T GetData<T>(Asynchronous data, AsynchronousFlags flags) where T : struct
		{
			return m_deviceContext.GetData<T>(data, flags);
		}

		internal bool GetData<T>(Asynchronous data, out T result) where T : struct
		{
			return m_deviceContext.GetData(data, out result);
		}

		internal bool GetData<T>(Asynchronous data, AsynchronousFlags flags, out T result) where T : struct
		{
			return m_deviceContext.GetData<T>(data, flags, out result);
		}

		internal bool IsDataAvailable(Asynchronous data)
		{
			return m_deviceContext.IsDataAvailable(data);
		}

		internal bool IsDataAvailable(Asynchronous data, AsynchronousFlags flags)
		{
			return m_deviceContext.IsDataAvailable(data, flags);
		}

		internal DataBox MapSubresource(IResource resourceRef, int subresource, MapMode mapType, MapFlags mapFlags)
		{
			return m_deviceContext.MapSubresource(resourceRef.Resource, subresource, mapType, mapFlags);
		}

		internal DataBox MapSubresource(IResource resource, int mipSlice, int arraySlice, MapMode mode, MapFlags flags, out int mipSize)
		{
			return m_deviceContext.MapSubresource(resource.Resource, mipSlice, arraySlice, mode, flags, out mipSize);
		}

		internal DataBox MapSubresource(Texture1D resource, int mipSlice, int arraySlice, MapMode mode, MapFlags flags, out DataStream stream)
		{
			return m_deviceContext.MapSubresource(resource, mipSlice, arraySlice, mode, flags, out stream);
		}

		internal DataBox MapSubresource(Texture2D resource, int mipSlice, int arraySlice, MapMode mode, MapFlags flags, out DataStream stream)
		{
			return m_deviceContext.MapSubresource(resource, mipSlice, arraySlice, mode, flags, out stream);
		}

		internal DataBox MapSubresource(Texture3D resource, int mipSlice, int arraySlice, MapMode mode, MapFlags flags, out DataStream stream)
		{
			return m_deviceContext.MapSubresource(resource, mipSlice, arraySlice, mode, flags, out stream);
		}

		internal void UnmapSubresource(Resource resourceRef, int subresource)
		{
			m_deviceContext.UnmapSubresource(resourceRef, subresource);
		}

		internal void UnmapSubresource(IResource resourceRef, int subresource)
		{
			UnmapSubresource(resourceRef.Resource, subresource);
		}

		internal void UpdateSubresource(DataBox source, IResource resource, int subresource = 0)
		{
			m_deviceContext.UpdateSubresource(source, resource.Resource, subresource);
		}

		internal void SetInputLayout(InputLayout il)
		{
			m_state.SetInputLayout(il);
		}

		internal void SetPrimitiveTopology(PrimitiveTopology pt)
		{
			m_state.SetPrimitiveTopology(pt);
		}

		internal void SetIndexBuffer(IIndexBuffer indexBufferRef, int offset = 0)
		{
			m_state.SetIndexBuffer(indexBufferRef, indexBufferRef?.Format ?? ((MyIndexBufferFormat)0), offset);
		}

		internal void SetVertexBuffer(int slot, IVertexBuffer vb, int stride = -1, int byteOffset = 0)
		{
			if (vb != null && stride < 0)
			{
				stride = vb.Description.StructureByteStride;
			}
			m_state.SetVertexBuffer(slot, vb, stride, byteOffset);
		}

		internal void SetVertexBuffers(int startSlot, IVertexBuffer[] vbs, int[] strides = null)
		{
<<<<<<< HEAD
			strides = strides ?? vbs.Select((IVertexBuffer vb) => vb?.Description.StructureByteStride ?? (-1)).ToArray();
=======
			strides = strides ?? Enumerable.ToArray<int>(Enumerable.Select<IVertexBuffer, int>((IEnumerable<IVertexBuffer>)vbs, (Func<IVertexBuffer, int>)((IVertexBuffer vb) => vb?.Description.StructureByteStride ?? (-1))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_state.SetVertexBuffers(startSlot, vbs, strides);
		}

		internal void SetVertexBuffersFast(int startSlot, IVertexBuffer vb0, IVertexBuffer vb1, IVertexBuffer vb2 = null)
		{
			m_state.SetVertexBuffersFast(startSlot, vb0, vb1, vb2);
		}

		internal void InvalidateVertexBufferBindings(int startSlot, int count)
		{
			m_state.InvalidateVertexBufferBindings(startSlot, count);
		}

		internal void SetBlendState(IBlendState bs, RawColor4? blendFactor = null)
		{
			BlendState bs2 = null;
			if (bs != null)
			{
				bs2 = ((IBlendStateInternal)bs).Resource;
			}
			m_state.SetBlendState(bs2, blendFactor);
		}

		internal void ResetTargets()
		{
			m_state.SetTarget(null, null);
		}

		internal void SetDepthStencilState(IDepthStencilState dss, int stencilRef = 0)
		{
			DepthStencilState dss2 = null;
			if (dss != null)
			{
				dss2 = ((IDepthStencilStateInternal)dss).Resource;
			}
			m_state.SetDepthStencilState(dss2, stencilRef);
		}

		internal void SetRtv(IRtvBindable rtv)
		{
			m_state.SetTarget(null, rtv.Rtv);
		}

		internal void SetRtvs(RenderTargetView[] rtvs)
		{
			m_state.SetTargets(null, rtvs);
		}

		internal void SetRtvs(IDsvBindable dsvBind, RenderTargetView[] rtvs)
		{
			m_state.SetTargets(dsvBind.Dsv, rtvs);
		}

		internal void SetRtvNull()
		{
			m_state.SetTarget(null);
		}

		internal void SetRtv(IDsvBindable dsv)
		{
			m_state.SetTarget(dsv.Dsv);
		}

		internal void SetRtv(IDsvBindable dsv, IRtvBindable rtv)
		{
			m_state.SetTarget(dsv.Dsv, rtv.Rtv);
		}

		internal void SetRtvs(MyGBuffer gbuffer, MyDepthStencilAccess access)
		{
			m_state.SetTargets(GetDSV(gbuffer.DepthStencil, access), gbuffer.GbufferRtvs);
		}

		private DepthStencilView GetDSV(IDepthStencil ds, MyDepthStencilAccess access)
		{
			if (ds == null)
			{
				return null;
			}
<<<<<<< HEAD
			switch (access)
			{
			default:
				return ds.Dsv.Dsv;
			case MyDepthStencilAccess.DepthReadOnly:
				return ds.DsvRoDepth.Dsv;
			case MyDepthStencilAccess.StencilReadOnly:
				return ds.DsvRoStencil.Dsv;
			case MyDepthStencilAccess.ReadOnly:
				return ds.DsvRo.Dsv;
			}
=======
			return access switch
			{
				MyDepthStencilAccess.DepthReadOnly => ds.DsvRoDepth.Dsv, 
				MyDepthStencilAccess.StencilReadOnly => ds.DsvRoStencil.Dsv, 
				MyDepthStencilAccess.ReadOnly => ds.DsvRo.Dsv, 
				_ => ds.Dsv.Dsv, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		internal void SetRasterizerState(IRasterizerState rs)
		{
			RasterizerState rasterizerState = null;
			if (rs != null)
			{
				rasterizerState = ((IRasterizerStateInternal)rs).Resource;
			}
			m_state.SetRasterizerState(rasterizerState);
		}

		internal void SetScissorRectangle(int left, int top, int right, int bottom)
		{
			m_state.SetScissorRectangle(left, top, right, bottom);
		}

		internal void SetViewport(ref RawViewportF viewport)
		{
			m_state.SetViewport(ref viewport);
		}

		internal void SetViewport(ref MyViewport viewport)
		{
			RawViewportF rawViewportF = default(RawViewportF);
			rawViewportF.X = viewport.OffsetX;
			rawViewportF.Y = viewport.OffsetY;
			rawViewportF.Width = viewport.Width;
			rawViewportF.Height = viewport.Height;
			rawViewportF.MinDepth = 0f;
			rawViewportF.MaxDepth = 1f;
			RawViewportF viewport2 = rawViewportF;
			m_state.SetViewport(ref viewport2);
		}

		internal void SetViewport(float x, float y, float width, float height, float minZ = 0f, float maxZ = 1f)
		{
			RawViewportF rawViewportF = default(RawViewportF);
			rawViewportF.X = x;
			rawViewportF.Y = y;
			rawViewportF.Width = width;
			rawViewportF.Height = height;
			rawViewportF.MinDepth = minZ;
			rawViewportF.MaxDepth = maxZ;
			RawViewportF viewport = rawViewportF;
			m_state.SetViewport(ref viewport);
		}

		internal void SetScreenViewport()
		{
			RawViewportF rawViewportF = default(RawViewportF);
			rawViewportF.Width = MyRender11.ResolutionF.X;
			rawViewportF.Height = MyRender11.ResolutionF.Y;
			rawViewportF.MaxDepth = 1f;
			RawViewportF viewport = rawViewportF;
			m_state.SetViewport(ref viewport);
		}

		internal void SetTarget(IBuffer buffer, int offsets)
		{
			m_state.SetTarget(buffer.Buffer, offsets);
		}

		internal void ResetStreamTargets()
		{
			m_state.ResetStreamTargets();
		}

		[Conditional("VRAGE")]
		public void SetEventMarker(string tag)
		{
			if (tag != null && m_deviceContext != null && MyVRage.Platform.AfterMath != null)
			{
				MyVRage.Platform.AfterMath.SetEventMarker(NativePointer, tag);
			}
		}

		[Conditional("VRAGE")]
		public void GetLastAnnotation(ref string marker)
		{
			if (m_deviceContext != null && MyVRage.Platform.AfterMath != null)
			{
				marker = MyVRage.Platform.AfterMath.GetInfo(NativePointer);
			}
		}

		public MyFinishedContext FinishDeferredContext()
		{
			MyFinishedContext myFinishedContext = default(MyFinishedContext);
			myFinishedContext.CommandList = FinishCommandList(restoreState: false);
			myFinishedContext.ProfilingQueries = m_profilingQueries;
			myFinishedContext.Statistics = m_statistics;
			MyFinishedContext result = myFinishedContext;
			m_profilingQueries = MyObjectPoolManager.Allocate<MyFrameProfilingContext>();
			m_statistics = MyObjectPoolManager.Allocate<MyRenderContextStatistics>();
			MyManagers.DeferredRCs.FreeRC(this);
			return result;
		}

		public void CleanUpCommandLists()
		{
			if (!m_disposeCommandListsTask.valid || m_disposeCommandListsTask.IsComplete)
			{
<<<<<<< HEAD
				if (!m_commandListQueue.IsEmpty)
=======
				if (!m_commandListQueue.get_IsEmpty())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					m_disposeCommandListsTask = Parallel.Start(DisposeCommandListsWork, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.RenderPass, "DisposeCommandListsWork"));
				}
				return;
			}
<<<<<<< HEAD
			while (m_commandListQueue.Count > 40)
			{
				if (m_commandListQueue.TryDequeue(out var result))
				{
					result.Dispose();
=======
			CommandList commandList = default(CommandList);
			while (m_commandListQueue.get_Count() > 40)
			{
				if (m_commandListQueue.TryDequeue(ref commandList))
				{
					commandList.Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		private void DisposeCommandListsWork()
		{
<<<<<<< HEAD
			CommandList result;
			while (m_commandListQueue.TryDequeue(out result))
			{
				result.Dispose();
=======
			CommandList commandList = default(CommandList);
			while (m_commandListQueue.TryDequeue(ref commandList))
			{
				commandList.Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void ExecuteContext(ref MyFinishedContext fc, [CallerMemberName] string caller = null, [CallerLineNumber] int callerLine = 0, [CallerFilePath] string callerPath = null)
		{
			MyGpuProfiler.Join(fc.ProfilingQueries);
			MyObjectPoolManager.Deallocate(fc.ProfilingQueries);
			ExecuteCommandList(fc.CommandList, false);
			m_commandListQueue.Enqueue(fc.CommandList);
			m_statistics.Gather(fc.Statistics);
			MyObjectPoolManager.Deallocate(fc.Statistics);
			fc = default(MyFinishedContext);
		}

		public IConstantBuffer GetObjectCB(int size)
		{
			size = MathHelper.Align(size, 16);
			if (!m_objectsConstantBuffers.TryGetValue(size, out var value))
			{
				value = MyManagers.Buffers.CreateConstantBuffer("CommonObjectCB" + size, size, null, ResourceUsage.Dynamic);
				m_objectsConstantBuffers.Add(size, value);
			}
			return value;
		}

		internal IConstantBuffer GetMaterialCB(int size)
		{
			size = MathHelper.Align(size, 16);
			if (!m_materialsConstantBuffers.TryGetValue(size, out var value))
			{
				value = (m_materialsConstantBuffers[size] = MyManagers.Buffers.CreateConstantBuffer("CommonMaterialCB" + size, size, null, ResourceUsage.Dynamic));
			}
			return value;
		}

		public void UnloadData()
		{
			foreach (IConstantBuffer value in m_objectsConstantBuffers.Values)
			{
				MyManagers.Buffers.Dispose(value);
			}
			m_objectsConstantBuffers.Clear();
			foreach (IConstantBuffer value2 in m_materialsConstantBuffers.Values)
			{
				MyManagers.Buffers.Dispose(value2);
			}
			m_materialsConstantBuffers.Clear();
		}
	}
}
