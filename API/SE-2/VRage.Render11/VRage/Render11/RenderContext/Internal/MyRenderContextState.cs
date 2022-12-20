using System;
using System.Runtime.CompilerServices;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using VRage.Render11.Resources;
using VRageMath;

namespace VRage.Render11.RenderContext.Internal
{
	internal class MyRenderContextState
	{
		private DeviceContext m_deviceContext;

		private MyRenderContextStatistics m_statistics;

		private InputLayout m_inputLayout;

		private PrimitiveTopology m_primitiveTopology;

		private IIndexBuffer m_indexBufferRef;

		private MyIndexBufferFormat m_indexBufferFormat;

		private int m_indexBufferOffset;

		private readonly IVertexBuffer[] m_vertexBuffers = new IVertexBuffer[8];

		private readonly int[] m_vertexBuffersStrides = new int[8];

		private readonly int[] m_vertexBuffersByteOffset = new int[8];

		private BlendState m_blendState;

		private int m_stencilRef;

		private DepthStencilState m_depthStencilState;

		private int m_rtvsCount;

		private RenderTargetView[] m_rtvs = new RenderTargetView[8];

		private DepthStencilView m_dsv;

		private RasterizerState m_rasterizerState;

		private Vector2I m_scissorLeftTop;

		private Vector2I m_scissorRightBottom;

		private RawViewportF m_viewport;

		private SharpDX.Direct3D11.Buffer m_targetBuffer;

		private int m_targetOffsets;

		internal void Init(DeviceContext deviceContext, MyRenderContextStatistics statistics)
		{
			m_deviceContext = deviceContext;
			m_statistics = statistics;
		}

		internal void Clear()
		{
			if (m_deviceContext != null)
			{
				m_deviceContext.ClearState();
			}
			m_inputLayout = null;
			m_primitiveTopology = PrimitiveTopology.Undefined;
			m_indexBufferRef = null;
			m_indexBufferFormat = (MyIndexBufferFormat)0;
			m_indexBufferOffset = 0;
			for (int i = 0; i < m_vertexBuffers.Length; i++)
			{
				m_vertexBuffers[i] = null;
			}
			for (int j = 0; j < m_vertexBuffersStrides.Length; j++)
			{
				m_vertexBuffersStrides[j] = 0;
			}
			for (int k = 0; k < m_vertexBuffersByteOffset.Length; k++)
			{
				m_vertexBuffersByteOffset[k] = 0;
			}
			m_blendState = null;
			m_stencilRef = 0;
			m_depthStencilState = null;
			m_rtvsCount = 0;
			for (int l = 0; l < m_rtvs.Length; l++)
			{
				m_rtvs[l] = null;
			}
			m_dsv = null;
			m_rasterizerState = null;
			m_scissorLeftTop = new Vector2I(-1, -1);
			m_scissorRightBottom = new Vector2I(-1, -1);
			m_viewport = default(RawViewportF);
			m_targetBuffer = null;
			m_targetOffsets = 0;
			if (m_statistics != null)
			{
				m_statistics.ClearStates++;
			}
		}

		internal void SetInputLayout(InputLayout il)
		{
			if (il != m_inputLayout)
			{
				m_inputLayout = il;
				m_deviceContext.InputAssembler.InputLayout = il;
				m_statistics.SetInputLayout++;
			}
		}

		internal void SetPrimitiveTopology(PrimitiveTopology pt)
		{
			if (pt != m_primitiveTopology)
			{
				m_primitiveTopology = pt;
				m_deviceContext.InputAssembler.PrimitiveTopology = pt;
				m_statistics.SetPrimitiveTopologies++;
			}
		}

		internal void SetIndexBuffer(IIndexBuffer ib, MyIndexBufferFormat format, int offset)
		{
			if (ib != m_indexBufferRef || format != m_indexBufferFormat || offset != m_indexBufferOffset)
			{
				m_indexBufferRef = ib;
				m_indexBufferFormat = format;
				m_indexBufferOffset = offset;
				m_deviceContext.InputAssembler.SetIndexBuffer(ib?.Buffer, (Format)format, offset);
				m_statistics.SetIndexBuffers++;
			}
		}

		internal void SetVertexBuffer(int slot, IVertexBuffer vb, int stride, int byteOffset)
		{
			if (vb != m_vertexBuffers[slot] || stride != m_vertexBuffersStrides[slot] || byteOffset != m_vertexBuffersByteOffset[slot])
			{
				m_vertexBuffers[slot] = vb;
				m_vertexBuffersStrides[slot] = stride;
				m_vertexBuffersByteOffset[slot] = byteOffset;
				m_deviceContext.InputAssembler.SetVertexBuffers(slot, new VertexBufferBinding(vb?.Buffer, stride, byteOffset));
				m_statistics.SetVertexBuffers++;
			}
		}

		internal void SetVertexBuffers(int startSlot, IVertexBuffer[] vbs, int[] strides)
		{
			for (int i = startSlot; i < startSlot + vbs.Length; i++)
			{
				SetVertexBuffer(i, vbs[i], strides[i], 0);
			}
		}

		internal unsafe void SetVertexBuffersFast(int startSlot, IVertexBuffer vb0, IVertexBuffer vb1, IVertexBuffer vb2)
		{
			int numBuffers = ((vb2 == null) ? 2 : 3);
			IntPtr* value = stackalloc IntPtr[3]
			{
				vb0?.Buffer?.NativePointer ?? IntPtr.Zero,
				vb1?.Buffer?.NativePointer ?? IntPtr.Zero,
				vb2?.Buffer?.NativePointer ?? IntPtr.Zero
			};
			int* value2 = stackalloc int[3]
			{
				vb0?.Description.StructureByteStride ?? 0,
				vb1?.Description.StructureByteStride ?? 0,
				vb2?.Description.StructureByteStride ?? 0
			};
			byte* intPtr = stackalloc byte[12];
			// IL initblk instruction
			Unsafe.InitBlock(intPtr, 0, 12);
			int* value3 = (int*)intPtr;
			m_deviceContext.InputAssembler.SetVertexBuffers(startSlot, numBuffers, new IntPtr(value), new IntPtr(value2), new IntPtr(value3));
			m_statistics.SetVertexBuffers++;
		}

		internal void InvalidateVertexBufferBindings(int startSlot, int count)
		{
			for (int i = startSlot; i < startSlot + count; i++)
			{
				m_vertexBuffers[i] = null;
			}
		}

		internal void SetBlendState(BlendState bs, RawColor4? blendFactor = null)
		{
			if (bs != m_blendState || blendFactor.HasValue)
			{
				m_blendState = bs;
				m_deviceContext.OutputMerger.SetBlendState(bs, blendFactor);
				m_statistics.SetBlendStates++;
			}
		}

		internal void SetDepthStencilState(DepthStencilState dss, int stencilRef)
		{
			if (dss != m_depthStencilState || stencilRef != m_stencilRef)
			{
				m_depthStencilState = dss;
				m_stencilRef = stencilRef;
				m_deviceContext.OutputMerger.SetDepthStencilState(dss, stencilRef);
				m_statistics.SetDepthStencilStates++;
			}
		}

		internal void SetTarget(DepthStencilView dsv)
		{
			if (m_dsv != dsv || m_rtvsCount != 0)
			{
				m_dsv = dsv;
				m_rtvsCount = 0;
				m_deviceContext.OutputMerger.SetTargets(dsv);
				m_statistics.SetTargets++;
			}
		}

		internal void SetTarget(DepthStencilView dsv, RenderTargetView rtv)
		{
			if (m_dsv != dsv || m_rtvsCount != 1 || m_rtvs[0] != rtv)
			{
				m_dsv = dsv;
				m_rtvsCount = 1;
				m_rtvs[0] = rtv;
				m_deviceContext.OutputMerger.SetTargets(dsv, rtv);
				m_statistics.SetTargets++;
			}
		}

		public void SetTargets(DepthStencilView dsv, RenderTargetView[] rtvs)
		{
			bool flag = dsv == m_dsv && rtvs.Length == m_rtvsCount;
			for (int i = 0; i < rtvs.Length; i++)
			{
				if (m_rtvs[i] != rtvs[i])
				{
					m_rtvs[i] = rtvs[i];
					flag = false;
				}
			}
			if (!flag)
			{
				m_rtvsCount = rtvs.Length;
				m_dsv = dsv;
				m_deviceContext.OutputMerger.SetTargets(dsv, rtvs.Length, rtvs);
				m_statistics.SetTargets++;
			}
		}

		internal void SetRasterizerState(RasterizerState rs)
		{
			if (rs != m_rasterizerState)
			{
				m_rasterizerState = rs;
				m_deviceContext.Rasterizer.State = m_rasterizerState;
				m_statistics.SetRasterizerStates++;
			}
		}

		internal void SetScissorRectangle(int left, int top, int right, int bottom)
		{
			Vector2I vector2I = new Vector2I(left, top);
			Vector2I vector2I2 = new Vector2I(right, bottom);
			if (!(vector2I == m_scissorLeftTop) || !(vector2I2 == m_scissorRightBottom))
			{
				m_scissorLeftTop = vector2I;
				m_scissorRightBottom = vector2I2;
				m_deviceContext.Rasterizer.SetScissorRectangle(m_scissorLeftTop.X, m_scissorLeftTop.Y, m_scissorRightBottom.X, m_scissorRightBottom.Y);
			}
		}

		internal void SetViewport(ref RawViewportF viewport)
		{
			if (viewport.X != m_viewport.X || viewport.Y != m_viewport.Y || viewport.Width != m_viewport.Width || viewport.Height != m_viewport.Height || viewport.MinDepth != m_viewport.MinDepth || viewport.MaxDepth != m_viewport.MaxDepth)
			{
				m_viewport = viewport;
				m_deviceContext.Rasterizer.SetViewport(viewport);
				m_statistics.SetViewports++;
			}
		}

		internal void SetTarget(SharpDX.Direct3D11.Buffer buffer, int offsets)
		{
			if (buffer != m_targetBuffer || offsets != m_targetOffsets)
			{
				m_targetBuffer = buffer;
				m_targetOffsets = offsets;
				m_deviceContext.StreamOutput.SetTarget(buffer, offsets);
				m_statistics.SetTargets++;
			}
		}

		internal void ResetStreamTargets()
		{
			m_deviceContext.StreamOutput.SetTargets(null);
		}
	}
}
