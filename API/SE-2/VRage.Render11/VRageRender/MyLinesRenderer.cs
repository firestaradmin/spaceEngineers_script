using System.Collections.Generic;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRage.Generics;
using VRage.Library.Collections;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender.Vertex;

namespace VRageRender
{
	internal class MyLinesRenderer : MyImmediateRC
	{
		private static int m_currentBufferSize;

		private static IVertexBuffer m_VB;

		internal static MyList<MyVertexFormatPositionColor> m_vertices = new MyList<MyVertexFormatPositionColor>();

		private static List<MyLinesBatch> m_batches = new List<MyLinesBatch>(32);

		private static MyVertexShaders.Id m_vs;

		private static MyPixelShaders.Id m_ps;

		private static MyInputLayouts.Id m_inputLayout;

		private static MyObjectsPool<MyLinesBatch> m_batchesPool = new MyObjectsPool<MyLinesBatch>(4);

		internal unsafe static void Init()
		{
			m_vs = MyVertexShaders.Create("Primitives/Lines.hlsl");
			m_ps = MyPixelShaders.Create("Primitives/Lines.hlsl");
			m_inputLayout = MyInputLayouts.Create(m_vs.InfoId, MyVertexLayouts.GetLayout(MyVertexInputComponentType.POSITION3, MyVertexInputComponentType.COLOR4));
			m_currentBufferSize = 100000;
			m_VB = MyManagers.Buffers.CreateVertexBuffer("MyLinesRenderer", m_currentBufferSize, sizeof(MyVertexFormatPositionColor), null, ResourceUsage.Dynamic, isStreamOutput: false, isGlobal: true);
		}

		private static void CheckBufferSize(int requiredSize)
		{
			if (m_currentBufferSize < requiredSize)
			{
				m_currentBufferSize = (int)((float)requiredSize * 1.33f);
				MyManagers.Buffers.Resize(m_VB, m_currentBufferSize);
			}
		}

		internal static MyLinesBatch CreateBatch()
		{
			MyLinesBatch item = null;
			m_batchesPool.AllocateOrCreate(out item);
			item.Construct();
			return item;
		}

		internal static void Commit(MyLinesBatch batch)
		{
			batch.VertexCount = batch.List.Count;
			batch.StartVertex = m_vertices.Count;
			if (batch.VertexCount > 0)
			{
				m_batches.Add(batch);
				m_vertices.AddRange(batch.List);
				batch.List.Clear();
			}
			else
			{
				m_batchesPool.Deallocate(batch);
			}
		}

		internal static void Draw(IRtvBindable renderTarget, bool nullDepth = false)
		{
			MyImmediateRC.RC.SetScreenViewport();
			MyImmediateRC.RC.SetPrimitiveTopology(PrimitiveTopology.LineList);
			MyImmediateRC.RC.SetInputLayout(m_inputLayout);
			MyImmediateRC.RC.SetRasterizerState(MyRasterizerStateManager.LinesRasterizerState);
			MyImmediateRC.RC.VertexShader.Set(m_vs);
			MyImmediateRC.RC.PixelShader.Set(m_ps);
			MyImmediateRC.RC.SetBlendState(MyBlendStateManager.BlendAlphaPremult);
			CheckBufferSize(m_vertices.Count);
			MyImmediateRC.RC.SetVertexBuffer(0, m_VB);
			if (nullDepth)
			{
				MyImmediateRC.RC.SetRtv(renderTarget);
			}
			else
			{
				MyImmediateRC.RC.SetRtv(MyGBuffer.Main.ResolvedDepthStencil.DsvRo, renderTarget);
			}
			MyImmediateRC.RC.AllShaderStages.SetConstantBuffer(1, MyCommon.ProjectionConstants);
			if (m_batches.Count > 0)
			{
				MyMapping myMapping = MyMapping.MapDiscard(m_VB);
				myMapping.WriteAndPosition(m_vertices.GetInternalArray(), m_vertices.Count);
				myMapping.Unmap();
				Matrix matrix = Matrix.Zero;
				foreach (MyLinesBatch batch in m_batches)
				{
					Matrix matrix2 = ((!batch.CustomViewProjection.HasValue) ? MyRender11.Environment.Matrices.ViewProjectionAt0 : batch.CustomViewProjection.Value);
					if (matrix != matrix2)
					{
						matrix = matrix2;
						Matrix data = Matrix.Transpose(matrix2);
						myMapping = MyMapping.MapDiscard(MyCommon.ProjectionConstants);
						myMapping.WriteAndPosition(ref data);
						myMapping.Unmap();
					}
					if (batch.IgnoreDepth || nullDepth)
					{
						MyImmediateRC.RC.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
					}
					else
					{
						MyImmediateRC.RC.SetDepthStencilState(MyDepthStencilStateManager.DefaultDepthState);
					}
					MyImmediateRC.RC.Draw(batch.VertexCount, batch.StartVertex);
				}
			}
			MyImmediateRC.RC.SetDepthStencilState(null);
			MyImmediateRC.RC.SetRasterizerState(null);
			m_vertices.ClearFast();
			foreach (MyLinesBatch batch2 in m_batches)
			{
				m_batchesPool.Deallocate(batch2);
			}
			m_batches.Clear();
		}

		internal static void Clear()
		{
			m_vertices.Clear();
			foreach (MyLinesBatch batch in m_batches)
			{
				m_batchesPool.Deallocate(batch);
			}
			m_batches.Clear();
		}
	}
}
