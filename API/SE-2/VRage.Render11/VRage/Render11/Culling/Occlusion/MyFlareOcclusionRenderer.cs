using System;
using System.Collections.Generic;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRage.Library.Collections;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Scene.Components;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Culling.Occlusion
{
	internal class MyFlareOcclusionRenderer : IManager, IManagerDevice, IManagerUnloadData
	{
		private struct MyVbConstantElement
		{
			public Vector3 Position;

			public float Size;

			public float Shift;
		}

		private readonly Dictionary<FlareId, MyFlareOcclusionData> m_queries = new Dictionary<FlareId, MyFlareOcclusionData>(FlareId.Comparer);

		private MyVertexShaders.Id m_vs;

		private MyPixelShaders.Id m_ps;

		private IVertexBuffer m_vb;

		private MyVbConstantElement[] m_tempBuffer;

		private MyOcclusionQuery[] m_tempBuffer2;

		private MyInputLayouts.Id m_inputLayout;

		public void OnDeviceInit()
		{
			m_vs = MyVertexShaders.Create("Primitives/OcclusionQuery.hlsl");
			m_ps = MyPixelShaders.Create("Primitives/OcclusionQuery.hlsl");
			m_inputLayout = MyInputLayouts.Create(m_vs.InfoId, MyVertexLayouts.GetLayout(new MyVertexInputComponent(MyVertexInputComponentType.CUSTOM4_0, MyVertexInputComponentFreq.PER_INSTANCE), new MyVertexInputComponent(MyVertexInputComponentType.CUSTOM1_0, MyVertexInputComponentFreq.PER_INSTANCE)));
		}

		public void OnDeviceReset()
		{
		}

		public void OnDeviceEnd()
		{
		}

		internal MyFlareOcclusionData Get(FlareId flareId, string debugName)
		{
			MyFlareOcclusionData myFlareOcclusionData = new MyFlareOcclusionData
			{
				Query = MyOccllusionQueryFactory.CreateOcclusionQuery(debugName),
				Size = 1f,
				QueryTime = 0f,
				OcclusionFactor = 1f,
				LastOcclusionFactor = 1f,
				NextOcclusionFactor = 1f
			};
			m_queries[flareId] = myFlareOcclusionData;
			return myFlareOcclusionData;
		}

		internal void Remove(FlareId flareId)
		{
			if (m_queries.TryGetValue(flareId, out var value))
			{
				value.Query.Return();
				m_queries.Remove(flareId);
			}
		}

		public void OnUnloadData()
		{
			if (m_vb != null)
			{
				MyManagers.Buffers.Dispose(m_vb);
			}
			m_vb = null;
		}

		internal unsafe void Render(MyList<MyLightComponent> visibleLights, MyRenderContext RC, IDepthStencil ds, IRtvBindable rtv)
		{
			bool drawOcclusionQueriesDebug = MyRender11.Settings.DrawOcclusionQueriesDebug;
			if (m_vb == null || m_tempBuffer2.Length < visibleLights.Count)
			{
				int num = Math.Max(visibleLights.Count * 3 / 2, 32);
				if (m_vb == null)
				{
					m_vb = MyManagers.Buffers.CreateVertexBuffer("MyOcclusionQueryRenderer.VB", num, sizeof(MyVbConstantElement), null, ResourceUsage.Dynamic);
				}
				else
				{
					MyManagers.Buffers.Resize(m_vb, num);
				}
				m_tempBuffer = new MyVbConstantElement[num];
				m_tempBuffer2 = new MyOcclusionQuery[num];
			}
			int num2 = 0;
			float num3 = (float)MyCommon.FrameTime.Milliseconds;
			foreach (MyLightComponent visibleLight in visibleLights)
			{
				if (!m_queries.TryGetValue(visibleLight.FlareId, out var value))
				{
					continue;
				}
				value.Position = visibleLight.Owner.WorldMatrix.Translation;
				value.Size = visibleLight.Data.Glare.QuerySize;
				value.Shift = visibleLight.Data.Glare.QueryShift;
				value.FreqMinMs = visibleLight.Data.Glare.QueryFreqMinMs;
				value.FreqRndMs = visibleLight.Data.Glare.QueryFreqRndMs;
				if (drawOcclusionQueriesDebug)
				{
					value.OcclusionFactor = (value.LastOcclusionFactor = (value.NextOcclusionFactor = 1f));
				}
				else if (value.Query.Running)
				{
					long result = value.Query.GetResult();
					bool num4 = result != -1;
					if (num4 || MyRender11.Settings.IgnoreOcclusionQueries)
					{
						if (result == 0L)
						{
							value.LastOcclusionFactor = (value.NextOcclusionFactor = 1f);
						}
						else
						{
							value.LastOcclusionFactor = value.OcclusionFactor;
							value.NextOcclusionFactor = 1f - Math.Min((float)result / value.LastVolumeSquared, 1f);
						}
						value.LastQueryTime = num3;
						if (value.LastOcclusionFactor == 1f && value.NextOcclusionFactor != 1f)
						{
							value.QueryTime = num3 + value.FreqMinMs / 2f;
						}
						else
						{
							value.QueryTime = num3 + value.FreqMinMs + MyCommon.Random.NextFloat() * value.FreqRndMs;
						}
					}
					if (!num4)
					{
						continue;
					}
				}
				if (Math.Abs(value.QueryTime - value.LastQueryTime) < 0.1f)
				{
					value.OcclusionFactor = value.NextOcclusionFactor;
				}
				else
				{
					float amount = MathExt.Saturate((num3 - value.LastQueryTime) / (value.QueryTime - value.LastQueryTime));
					value.OcclusionFactor = MathHelper.Lerp(value.LastOcclusionFactor, value.NextOcclusionFactor, amount);
				}
				if (value.Visible && !(num3 < value.QueryTime))
				{
					Vector3 position = value.Position - MyRender11.Environment.Matrices.CameraPosition;
					float z = position.Length();
					Vector3 position2 = new Vector3(value.Size, value.Size, z);
					Vector3.Transform(ref position2, ref MyRender11.Environment.Matrices.Projection, out var result2);
					Vector2 vector = new Vector2(result2.X, result2.Y) * MyRender11.ResolutionF / 2f;
					value.LastVolumeSquared = Math.Abs(vector.X * vector.Y);
					MyVbConstantElement myVbConstantElement = default(MyVbConstantElement);
					myVbConstantElement.Position = position;
					myVbConstantElement.Size = value.Size;
					myVbConstantElement.Shift = value.Shift;
					MyVbConstantElement myVbConstantElement2 = myVbConstantElement;
					m_tempBuffer2[num2] = value.Query;
					m_tempBuffer[num2] = myVbConstantElement2;
					num2++;
					value.Visible = false;
				}
			}
			if (num2 <= 0)
			{
				return;
			}
			RC.SetViewport(0f, 0f, MyRender11.ViewportResolution.X, MyRender11.ViewportResolution.Y);
			RC.SetInputLayout(m_inputLayout);
			RC.SetPrimitiveTopology(PrimitiveTopology.TriangleStrip);
			RC.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			RC.SetRasterizerState(MyRasterizerStateManager.NocullRasterizerState);
			RC.SetDepthStencilState(MyDepthStencilStateManager.DepthTestReadOnly);
			RC.VertexShader.Set(m_vs);
			if (drawOcclusionQueriesDebug)
			{
				RC.PixelShader.Set(m_ps);
				RC.SetRtv(ds.DsvRo, rtv);
			}
			else
			{
				RC.SetRtv(ds.DsvRo);
				RC.PixelShader.Set(null);
			}
			MyMapping myMapping = MyMapping.MapDiscard(RC, m_vb);
			myMapping.WriteAndPosition(m_tempBuffer, num2);
			myMapping.Unmap();
			RC.SetVertexBuffer(0, m_vb);
			if (drawOcclusionQueriesDebug)
			{
				for (int i = 0; i < num2; i++)
				{
					RC.DrawInstanced(4, 1, 0, i);
				}
			}
			else
			{
				for (int j = 0; j < num2; j++)
				{
					m_tempBuffer2[j].Begin(RC);
					RC.DrawInstanced(4, 1, 0, j);
					m_tempBuffer2[j].End(RC);
				}
			}
			RC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
		}
	}
}
