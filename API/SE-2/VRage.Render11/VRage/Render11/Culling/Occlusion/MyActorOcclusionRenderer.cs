using System;
using System.Collections.Generic;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Culling.Occlusion
{
	internal class MyActorOcclusionRenderer : IManager, IManagerDevice, IManagerUnloadData
	{
		private class MyData
		{
			public MyOcclusionQuery Query;

			public long QueryFrameIndex;

			public MyActor Actor;
		}

		private struct MyVbConstantElement2
		{
			public Vector3 Min;

			public Vector3 Extents;
		}

		private const long MAX_FRAMES_TIMEOUT = 100L;

		private readonly Dictionary<uint, MyData>[] m_queries = new Dictionary<uint, MyData>[19];

		private MyVertexShaders.Id m_vs;

		private MyPixelShaders.Id m_ps;

		private MyInputLayouts.Id m_inputLayout;

		private IVertexBuffer m_vb;

		private MyVbConstantElement2[] m_vbTemp;

		private MyOcclusionQuery[] m_queriesTemp;

		public void OnDeviceInit()
		{
			m_vs = MyVertexShaders.Create("Primitives/GroupOcclusionQuery.hlsl");
			m_ps = MyPixelShaders.Create("Primitives/GroupOcclusionQuery.hlsl");
			m_inputLayout = MyInputLayouts.Create(m_vs.InfoId, MyVertexLayouts.GetLayout(new MyVertexInputComponent(MyVertexInputComponentType.CUSTOM3_0, MyVertexInputComponentFreq.PER_INSTANCE), new MyVertexInputComponent(MyVertexInputComponentType.CUSTOM3_0, MyVertexInputComponentFreq.PER_INSTANCE)));
		}

		public void OnDeviceReset()
		{
		}

		public void OnDeviceEnd()
		{
		}

		public void OnUnloadData()
		{
			Dictionary<uint, MyData>[] queries = m_queries;
			for (int i = 0; i < queries.Length; i++)
			{
				_ = queries[i];
			}
			if (m_vb != null)
			{
				MyManagers.Buffers.Dispose(m_vb);
			}
			m_vb = null;
		}

		internal unsafe void Render(MyRenderContext RC, MyCullQuery query)
		{
			bool drawGroupOcclusionQueriesDebug = MyRender11.Settings.DrawGroupOcclusionQueriesDebug;
			if (m_vb == null || m_queriesTemp.Length < query.Groups.Count)
			{
				int num = Math.Max(query.Groups.Count * 3 / 2, 32);
				if (m_vb == null)
				{
					m_vb = MyManagers.Buffers.CreateVertexBuffer("MyOcclusionQueryRenderer.VB", num, sizeof(MyVbConstantElement2), null, ResourceUsage.Dynamic);
				}
				else
				{
					MyManagers.Buffers.Resize(m_vb, num);
				}
				m_vbTemp = new MyVbConstantElement2[num];
				m_queriesTemp = new MyOcclusionQuery[num];
			}
			int num2 = 0;
			Dictionary<uint, MyData> dictionary = m_queries[query.ViewId] ?? (m_queries[query.ViewId] = new Dictionary<uint, MyData>());
			bool flag = MyRender11.Settings.DisableOcclusionQueries || (MyRender11.Settings.DisableShadowCascadeOcclusionQueries && MyViewIds.IsShadowCascadeId(query.ViewId));
			foreach (MyManualCullTreeData group in query.Groups)
			{
				if (!dictionary.TryGetValue(group.Actor.ID, out var value))
				{
					value = new MyData
					{
						Query = MyOccllusionQueryFactory.CreateOcclusionQuery(group.Actor.DebugName + " Occluder"),
						Actor = group.Actor
					};
					value.Query.Reset(long.MaxValue);
					dictionary.Add(group.Actor.ID, value);
					group.Actor.OnDestruct += OnGroupDestruct;
				}
				if (flag)
				{
					value.Actor.OccludedState[query.ViewId] = false;
					continue;
				}
				bool flag2 = false;
				BoundingBoxD worldAabb = group.Actor.WorldAabb;
				BoundingBoxD inflated = worldAabb.GetInflated(worldAabb.Size / 2.0);
				long num3 = value.Actor.FrameInView[query.ViewId];
				value.Actor.FrameInView[query.ViewId] = MyCommon.FrameCounter;
				bool flag3;
				if (num3 < MyCommon.FrameCounter - 1)
				{
					flag3 = false;
					flag2 = !drawGroupOcclusionQueriesDebug;
					value.Query.Reset(long.MaxValue);
				}
				else if (inflated.DistanceSquared(ref query.ViewOrigin) < (double)MyRender11.Environment.Matrices.NearClipping)
				{
					flag3 = false;
					flag2 = !drawGroupOcclusionQueriesDebug;
					value.Query.Reset(long.MaxValue);
				}
				else
				{
					long result = value.Query.GetResult();
					if (result == -1)
					{
						if (value.QueryFrameIndex > MyCommon.FrameCounter)
						{
							if (!drawGroupOcclusionQueriesDebug)
							{
								continue;
							}
							flag3 = value.Actor.OccludedState[query.ViewId];
						}
						else
						{
							flag3 = false;
							flag2 = !drawGroupOcclusionQueriesDebug;
						}
					}
					else
					{
						flag3 = result == 0;
					}
				}
				if (!MyRender11.Settings.IgnoreOcclusionQueries)
				{
					value.Actor.OccludedState[query.ViewId] = flag3;
				}
				if (!flag2)
				{
					MyVbConstantElement2 myVbConstantElement = default(MyVbConstantElement2);
					myVbConstantElement.Min = inflated.Min - MyRender11.Environment.Matrices.CameraPosition;
					myVbConstantElement.Extents = ((!value.Actor.OccludedState[query.ViewId]) ? 1 : (-1)) * inflated.Extents;
					MyVbConstantElement2 myVbConstantElement2 = myVbConstantElement;
					m_queriesTemp[num2] = value.Query;
					m_vbTemp[num2] = myVbConstantElement2;
					value.QueryFrameIndex = MyCommon.FrameCounter + 100;
					num2++;
				}
			}
			if (num2 > 0)
			{
				RC.SetViewport(ref query.Viewport);
				RC.SetInputLayout(m_inputLayout);
				RC.SetPrimitiveTopology(PrimitiveTopology.TriangleStrip);
				RC.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstants);
				MyMapping myMapping = MyMapping.MapDiscard(RC, MyCommon.ProjectionConstants);
				Matrix.Transpose(ref query.ViewProjection, out var result2);
				myMapping.WriteAndPosition(ref result2);
				myMapping.Unmap();
				RC.VertexShader.SetConstantBuffer(1, MyCommon.ProjectionConstants);
				RC.SetRasterizerState(query.Rasterizer);
				RC.SetDepthStencilState((query.ViewId == 0) ? MyDepthStencilStateManager.DepthTestReadOnly : null);
				RC.VertexShader.Set(m_vs);
				if (drawGroupOcclusionQueriesDebug && query.Rtv != null)
				{
					RC.PixelShader.Set(m_ps);
					RC.SetRtv(query.DepthBufferRo, query.Rtv);
				}
				else
				{
					RC.PixelShader.Set(null);
					RC.SetRtv(query.DepthBufferRo);
				}
				MyMapping myMapping2 = MyMapping.MapDiscard(RC, m_vb);
				myMapping2.WriteAndPosition(m_vbTemp, num2);
				myMapping2.Unmap();
				RC.SetVertexBuffer(0, m_vb);
				for (int i = 0; i < num2; i++)
				{
					m_queriesTemp[i].Begin(RC);
					RC.DrawInstanced(14, 1, 0, i);
					m_queriesTemp[i].End(RC);
				}
				RC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
				RC.SetRasterizerState(null);
			}
		}

		private void OnGroupDestruct(MyActor actor)
		{
			actor.OnDestruct -= OnGroupDestruct;
			Dictionary<uint, MyData>[] queries = m_queries;
			for (int i = 0; i < queries.Length; i++)
			{
				queries[i]?.Remove(actor.ID);
			}
		}
	}
}
