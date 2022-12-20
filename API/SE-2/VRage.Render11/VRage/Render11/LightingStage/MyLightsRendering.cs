using System;
using System.Collections.Generic;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRage.Library.Collections;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRage.Render11.Scene;
using VRage.Render11.Scene.Components;
using VRageMath;
using VRageRender;

namespace VRage.Render11.LightingStage
{
	internal class MyLightsRendering
	{
		private static MyPixelShaders.Id m_directionalEnvironmentLightNoShadow = MyPixelShaders.Id.NULL;

		private static MyPixelShaders.Id m_directionalEnvironmentLightPixel = MyPixelShaders.Id.NULL;

		private static MyPixelShaders.Id m_directionalEnvironmentLightSample = MyPixelShaders.Id.NULL;

		private static MyPixelShaders.Id m_pointlightsTiledPixel;

		private static MyPixelShaders.Id m_pointlightsTiledSample;

		private static MyComputeShaders.Id m_preparePointLights;

		private static MyVertexShaders.Id m_spotlightProxyVs;

		private static MyPixelShaders.Id m_spotlightPsPixel;

		private static MyPixelShaders.Id m_spotlightShadowsPsPixel;

		private static MyInputLayouts.Id m_spotlightProxyIl;

		private static MeshId? m_coneMesh;

		private static bool m_lastFrameVisiblePointlights = true;

		private const int TILE_SIZE = 16;

		private static ISrvUavBuffer m_tileIndices;

		private static int m_tilesNum;

		private static int m_tilesX;

		private static int m_tilesY;

<<<<<<< HEAD
		private static int m_maxPointLights = 256;

		private static MyPointlightConstants[] m_pointlightsCullBuffer;
=======
		private static readonly MyPointlightConstants[] m_pointlightsCullBuffer = new MyPointlightConstants[256];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private static ISrvBuffer m_pointlightCullHwBuffer;

		private const int SPOTLIGHTS_MAX = 32;

		private static MyList<MyLightComponent> m_outputList = new MyList<MyLightComponent>();

		private static readonly MyCullResults m_gatherTempResult = new MyCullResults();

		internal unsafe static void Init(IMyUpdateBatch meshBatch)
		{
			m_coneMesh = MyMeshes.GetMeshId(X.TEXT_("Models/Debug/Cone.mwm"), 1f, meshBatch);
			m_directionalEnvironmentLightPixel = MyPixelShaders.Create("Lighting/LightDir.hlsl");
			m_directionalEnvironmentLightSample = MyPixelShaders.Create("Lighting/LightDir.hlsl", MyRender11.ShaderSampleFrequencyDefine());
			m_directionalEnvironmentLightNoShadow = MyPixelShaders.Create("Lighting/LightDir.hlsl", new ShaderMacro[1]
			{
				new ShaderMacro("NO_SHADOWS", null)
			});
			m_pointlightsTiledPixel = MyPixelShaders.Create("Lighting/LightPoint.hlsl");
			m_pointlightsTiledSample = MyPixelShaders.Create("Lighting/LightPoint.hlsl", MyRender11.ShaderSampleFrequencyDefine());
			m_preparePointLights = MyComputeShaders.Create("Lighting/PrepareLights.hlsl", new ShaderMacro[1]
			{
				new ShaderMacro("NUMTHREADS", 16)
			});
			m_spotlightProxyVs = MyVertexShaders.Create("Lighting/LightSpot.hlsl");
			m_spotlightPsPixel = MyPixelShaders.Create("Lighting/LightSpot.hlsl");
			m_spotlightShadowsPsPixel = MyPixelShaders.Create("Lighting/LightSpot.hlsl", new ShaderMacro[1]
			{
				new ShaderMacro("SHADOWS", 0)
			});
			m_spotlightProxyIl = MyInputLayouts.Create(m_spotlightProxyVs.InfoId, MyVertexLayouts.GetLayout(default(MyVertexInputComponentType)));
<<<<<<< HEAD
			m_pointlightsCullBuffer = new MyPointlightConstants[m_maxPointLights];
			m_pointlightCullHwBuffer = MyManagers.Buffers.CreateSrv("MyLightRendering", m_maxPointLights, sizeof(MyPointlightConstants), null, ResourceUsage.Dynamic, isGlobal: true);
		}

		public unsafe static void UpdateSettings()
		{
			switch (MyRender11.Settings.User.LightsQuality)
			{
			case MyRenderQualityEnum.LOW:
				m_maxPointLights = 256;
				break;
			case MyRenderQualityEnum.NORMAL:
				m_maxPointLights = 512;
				break;
			case MyRenderQualityEnum.HIGH:
				m_maxPointLights = 2048;
				break;
			case MyRenderQualityEnum.EXTREME:
				m_maxPointLights = 4096;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			m_pointlightsCullBuffer = new MyPointlightConstants[m_maxPointLights];
			MyManagers.Buffers.Dispose(m_pointlightCullHwBuffer);
			m_pointlightCullHwBuffer = MyManagers.Buffers.CreateSrv("MyLightRendering", m_maxPointLights, sizeof(MyPointlightConstants), null, ResourceUsage.Dynamic, isGlobal: true);
			if (m_tileIndices != null)
			{
				MyManagers.Buffers.Dispose(m_tileIndices);
			}
			m_tileIndices = MyManagers.Buffers.CreateSrvUav("MyScreenDependants::tileIndices", m_tilesNum + m_tilesNum * m_maxPointLights, 4, null, MyUavType.Default, ResourceUsage.Default, isGlobal: true);
=======
			m_pointlightCullHwBuffer = MyManagers.Buffers.CreateSrv("MyLightRendering", 256, sizeof(MyPointlightConstants), null, ResourceUsage.Dynamic, isGlobal: true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		internal static void OnDeviceEnd()
		{
			MyManagers.Buffers.Dispose(m_pointlightCullHwBuffer);
			if (m_coneMesh.HasValue)
			{
				MyMeshes.RemoveMesh(m_coneMesh.Value);
				m_coneMesh = null;
			}
		}

		public static void PostprocessCulling(MyCullQuery query)
		{
<<<<<<< HEAD
			CullPointLights(query);
			CullSpotLights(query);
		}

		private static void CullSpotLights(MyCullQuery query)
		{
			MyList<MyLightComponent> spotLights = query.Results.SpotLights;
			if (spotLights.Count <= 32)
			{
				return;
			}
			for (int i = 0; i < spotLights.Count; i++)
			{
				MyLightComponent myLightComponent = spotLights[i];
				myLightComponent.ViewerDistanceSquaredFast = MathHelper.RoundOn2(myLightComponent.Owner.CalculateCameraDistanceSquaredFast());
				m_outputList.Add(myLightComponent);
			}
			if (m_outputList.Count > 32)
			{
				m_outputList.Sort((MyLightComponent x, MyLightComponent y) => x.ViewerDistanceSquaredFast.CompareTo(y.ViewerDistanceSquaredFast));
				m_outputList.RemoveRange(32, m_outputList.Count - 32);
			}
			query.Results.SpotLights = m_outputList;
			m_outputList = spotLights;
			m_outputList.ClearFast();
		}

		private static void CullPointLights(MyCullQuery query)
		{
			MyList<MyLightComponent> pointLights = query.Results.PointLights;
			if (pointLights.Count <= m_maxPointLights)
=======
			MyList<MyLightComponent> pointLights = query.Results.PointLights;
			if (pointLights.Count <= 256)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			int num = ((query.PointLightsInfo.Count > 0) ? query.PointLightsInfo[0].Start : pointLights.Count);
<<<<<<< HEAD
			if (num > m_maxPointLights)
			{
				num = m_maxPointLights;
=======
			if (num > 256)
			{
				num = 256;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			num = Math.Min(pointLights.Count, num);
			for (int i = 0; i < num; i++)
			{
<<<<<<< HEAD
				MyLightComponent myLightComponent = pointLights[i];
				myLightComponent.ViewerDistanceSquaredFast = MathHelper.RoundOn2(myLightComponent.Owner.CalculateCameraDistanceSquaredFast());
				m_outputList.Add(myLightComponent);
			}
			if (num < m_maxPointLights)
=======
				m_outputList.Add(pointLights[i]);
			}
			if (num < 256)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				for (int j = 0; j < query.PointLightsInfo.Count; j++)
				{
					MyCullQuery.MyPointLightsInfo value = query.PointLightsInfo[j];
					value.CameraDistance = query.Groups[query.PointLightsInfo[j].GroupsIndex].Actor.CalculateCameraDistanceSquared();
					query.PointLightsInfo[j] = value;
<<<<<<< HEAD
				}
				query.PointLightsInfo.Sort((MyCullQuery.MyPointLightsInfo x, MyCullQuery.MyPointLightsInfo y) => x.CameraDistance.CompareTo(y.CameraDistance));
				float num2 = float.MaxValue;
				for (int k = 0; k < query.PointLightsInfo.Count; k++)
				{
					int count = query.PointLightsInfo[k].Count;
					int start = query.PointLightsInfo[k].Start;
					count = Math.Min(pointLights.Count - start, count);
					if (start < 0 || count <= 0 || start + count > pointLights.Count)
					{
						continue;
					}
					if (m_outputList.Count + count < m_maxPointLights)
					{
						for (int l = start; l < start + count; l++)
						{
							MyLightComponent myLightComponent2 = pointLights[l];
							myLightComponent2.ViewerDistanceSquaredFast = MathHelper.RoundOn2(myLightComponent2.Owner.CalculateCameraDistanceSquaredFast());
							m_outputList.Add(myLightComponent2);
						}
						continue;
					}
					MyList<MyLightComponent> range = pointLights.GetRange(start, count);
					bool flag = false;
					foreach (MyLightComponent item in range)
					{
						item.ViewerDistanceSquaredFast = MathHelper.RoundOn2(item.Owner.CalculateCameraDistanceSquaredFast());
						if (num2 > item.ViewerDistanceSquaredFast)
						{
							flag = true;
							m_outputList.Add(item);
						}
					}
					if (flag)
					{
						if (flag)
						{
							range.ClearFast();
							range.AddRange(m_outputList);
							m_outputList.ClearFast();
							count = range.Count;
						}
						range.Sort((MyLightComponent x, MyLightComponent y) => x.ViewerDistanceSquaredFast.CompareTo(y.ViewerDistanceSquaredFast));
						int val = m_outputList.Count + count - m_maxPointLights;
						val = Math.Min(count, val);
						start = count - val;
						if (start > 0 && val > 0 && start + val <= range.Count)
						{
							range.RemoveRange(start, val);
						}
						if (range.Count > 0)
						{
							m_outputList.AddRange(range);
						}
						if (m_outputList.Count != 0)
						{
							num2 = m_outputList[m_outputList.Count - 1].ViewerDistanceSquaredFast;
						}
=======
					if (value.CameraDistance == 0f)
					{
						for (int k = value.Start; k < value.Start + value.Count; k++)
						{
							m_outputList.Add(pointLights[k]);
						}
					}
				}
				if (m_outputList.Count >= 256)
				{
					foreach (MyLightComponent output in m_outputList)
					{
						output.ViewerDistanceSquaredFast = output.Owner.CalculateCameraDistanceSquaredFast();
					}
					m_outputList.Sort((MyLightComponent x, MyLightComponent y) => x.ViewerDistanceSquaredFast.CompareTo(y.ViewerDistanceSquaredFast));
					int count = m_outputList.Count - 256;
					m_outputList.RemoveRange(256, count);
				}
				else
				{
					query.PointLightsInfo.Sort((MyCullQuery.MyPointLightsInfo x, MyCullQuery.MyPointLightsInfo y) => x.CameraDistance.CompareTo(y.CameraDistance));
					for (int l = 0; l < query.PointLightsInfo.Count; l++)
					{
						if (query.PointLightsInfo[l].CameraDistance == 0f)
						{
							continue;
						}
						int count2 = query.PointLightsInfo[l].Count;
						int start = query.PointLightsInfo[l].Start;
						count2 = Math.Min(pointLights.Count - start, count2);
						if (start < 0 || count2 <= 0 || start + count2 > pointLights.Count)
						{
							continue;
						}
						if (m_outputList.Count + count2 < 256)
						{
							for (int m = start; m < start + count2; m++)
							{
								m_outputList.Add(pointLights[m]);
							}
							continue;
						}
						MyList<MyLightComponent> range = pointLights.GetRange(start, count2);
						range.Sort((MyLightComponent x, MyLightComponent y) => x.ViewerDistanceSquaredFast.CompareTo(y.ViewerDistanceSquaredFast));
						int num2 = m_outputList.Count + count2 - 256;
						count2 = Math.Min(count2, num2);
						start = count2 - num2;
						if (start >= 0 && num2 > 0 && start + num2 <= range.Count)
						{
							range.RemoveRange(start, num2);
							m_outputList.AddRange(range);
						}
						break;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			query.Results.PointLights = m_outputList;
			m_outputList = pointLights;
			m_outputList.SetSize(0);
		}

		private static void PreparePointLights(MyRenderContext rc, MyList<MyLightComponent> visibleLights)
		{
			int data = 0;
			foreach (MyLightComponent visibleLight in visibleLights)
			{
				if (visibleLight.Data.PointLightOn)
				{
					visibleLight.WritePointlightConstants(ref m_pointlightsCullBuffer[data]);
					data++;
				}
			}
			bool flag = data != 0;
			if (flag || m_lastFrameVisiblePointlights)
			{
				m_lastFrameVisiblePointlights = flag;
				MyMapping myMapping = MyMapping.MapDiscard(rc, rc.GetObjectCB(16));
				myMapping.WriteAndPosition(ref data);
				myMapping.Unmap();
				myMapping = MyMapping.MapDiscard(rc, m_pointlightCullHwBuffer);
<<<<<<< HEAD
				myMapping.WriteAndPosition(m_pointlightsCullBuffer, m_maxPointLights);
=======
				myMapping.WriteAndPosition(m_pointlightsCullBuffer, 256);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myMapping.Unmap();
				if (!MyStereoRender.Enable)
				{
					rc.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstants);
				}
				else
				{
					MyStereoRender.CSBindRawCB_FrameConstants(rc);
				}
				rc.ComputeShader.SetConstantBuffer(1, rc.GetObjectCB(16));
				rc.ComputeShader.SetUav(0, m_tileIndices);
				rc.ComputeShader.SetSrvs(0, MyGBuffer.Main);
				rc.ComputeShader.SetSrv(13, m_pointlightCullHwBuffer);
				rc.ComputeShader.Set(m_preparePointLights);
				Vector2I vector2I = new Vector2I(m_tilesX, m_tilesY);
				if (MyStereoRender.Enable && MyStereoRender.RenderRegion != 0)
				{
					vector2I.X /= 2;
				}
				rc.Dispatch(vector2I.X, vector2I.Y, 1);
				rc.ComputeShader.Set(null);
				rc.ComputeShader.SetUav(0, null);
				rc.ComputeShader.ResetSrvs(0, MyGBufferSrvFilter.ALL);
			}
		}

		private unsafe static void RenderSpotlights(MyRenderContext rc, IEnumerable<MyLightComponent> visibleLights)
		{
			rc.SetRtv(MyGBuffer.Main.DepthStencil.DsvRo, MyGBuffer.Main.LBuffer);
			rc.SetViewport(0f, 0f, MyRender11.ViewportResolution.X, MyRender11.ViewportResolution.Y);
			rc.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			if (MyStereoRender.Enable)
			{
				MyStereoRender.PSBindRawCB_FrameConstants(rc);
				MyStereoRender.SetViewport(rc);
			}
			MyMeshBuffers buffers = MyMeshes.GetLodMesh(m_coneMesh.Value, 0).Buffers;
			rc.SetVertexBuffer(0, buffers.VB0);
			rc.SetIndexBuffer(buffers.IB);
			rc.VertexShader.Set(m_spotlightProxyVs);
			rc.SetInputLayout(m_spotlightProxyIl);
			rc.SetRasterizerState(MyRasterizerStateManager.InvTriRasterizerState);
			IConstantBuffer objectCB = rc.GetObjectCB(sizeof(SpotlightConstants));
			rc.AllShaderStages.SetConstantBuffer(1, objectCB);
			rc.PixelShader.SetSampler(13, MySamplerStateManager.Alphamask);
			rc.PixelShader.SetSampler(14, MySamplerStateManager.Shadowmap);
			rc.PixelShader.SetSampler(15, MySamplerStateManager.Shadowmap);
			int num = 0;
			foreach (MyLightComponent visibleLight in visibleLights)
			{
				if (visibleLight.Data.SpotLightOn)
				{
					SpotlightConstants data = default(SpotlightConstants);
					ISrvBindable srvBind = visibleLight.WriteSpotlightConstants(ref data);
					IDepthTexture shadowMap = MyManagers.Shadows.GetShadowMap(visibleLight);
					if (shadowMap != null)
					{
						data.Spotlight.Resolution = shadowMap.Size.X;
					}
					rc.PixelShader.SetSrv(14, shadowMap);
					rc.PixelShader.Set((shadowMap == null) ? m_spotlightPsPixel : m_spotlightShadowsPsPixel);
					MyMapping myMapping = MyMapping.MapDiscard(rc, objectCB);
					myMapping.WriteAndPosition(ref data);
					myMapping.Unmap();
					rc.PixelShader.SetSrv(13, srvBind);
					if (MyRender11.MultisamplingEnabled)
					{
						rc.SetDepthStencilState(MyDepthStencilStateManager.TestEdgeStencil);
						rc.PixelShader.Set(m_spotlightPsPixel);
					}
					rc.DrawIndexed(MyMeshes.GetLodMesh(m_coneMesh.Value, 0).Info.IndicesNum, 0, 0);
					num++;
					if (num >= 32)
					{
						break;
					}
				}
			}
			if (MyRender11.MultisamplingEnabled)
			{
				rc.SetDepthStencilState(MyDepthStencilStateManager.DefaultDepthState);
			}
			rc.SetRasterizerState(null);
			rc.SetRtvNull();
		}

		internal static void Render(MyRenderContext rc, MyCullResults cullResults, ISrvTexture postProcessedShadows, IRtvTexture ambientOcclusion)
		{
			if (MyRender11.DebugOverrides.PointLights)
			{
				PreparePointLights(rc, cullResults.PointLights);
			}
			rc.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			if (!MyStereoRender.Enable)
			{
				rc.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstants);
				rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			}
			else
			{
				MyStereoRender.CSBindRawCB_FrameConstants(rc);
				MyStereoRender.PSBindRawCB_FrameConstants(rc);
			}
			rc.PixelShader.SetSrvs(0, MyGBuffer.Main);
			rc.SetBlendState(MyBlendStateManager.BlendAdditive);
			rc.SetDepthStencilState((!MyStereoRender.Enable) ? MyDepthStencilStateManager.IgnoreDepthStencil : MyDepthStencilStateManager.StereoIgnoreDepthStencil);
			rc.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			if (MyRender11.DebugOverrides.PointLights)
			{
				RenderPointlightsTiled(rc, ambientOcclusion);
			}
			if (MyRender11.DebugOverrides.SpotLights)
			{
				RenderSpotlights(rc, cullResults.SpotLights);
			}
			if (MyRender11.DebugOverrides.EnvLight)
			{
				RenderDirectionalEnvironmentLight(rc, postProcessedShadows, ambientOcclusion);
			}
			rc.AllShaderStages.SetSrv(0, null);
			rc.AllShaderStages.SetSrv(1, null);
			rc.AllShaderStages.SetSrv(2, null);
			rc.AllShaderStages.SetSrv(3, null);
			rc.AllShaderStages.SetSrv(4, null);
			rc.SetBlendState(null);
			rc.SetRtvNull();
		}

		private static void RenderPointlightsTiled(MyRenderContext rc, IRtvTexture ambientOcclusion)
		{
			rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			rc.PixelShader.SetSrv(14, m_tileIndices);
			rc.AllShaderStages.SetSrv(13, m_pointlightCullHwBuffer);
			rc.PixelShader.SetSrv(12, ambientOcclusion);
			rc.PixelShader.Set(m_pointlightsTiledPixel);
			MyScreenPass.RunFullscreenPixelFreq(rc, MyGBuffer.Main.LBuffer);
			if (MyRender11.MultisamplingEnabled)
			{
				rc.PixelShader.Set(m_pointlightsTiledSample);
				MyScreenPass.RunFullscreenSampleFreq(rc, MyGBuffer.Main.LBuffer);
			}
		}

		private static void RenderDirectionalEnvironmentLight(MyRenderContext rc, ISrvTexture postProcessedShadows, IRtvTexture ambientOcclusion)
		{
			MyShadowsQuality shadowQuality = MyRender11.Settings.User.ShadowQuality;
			MyPixelShaders.Id id = ((MyRender11.Settings.EnableShadows && MyRender11.DebugOverrides.Shadows && shadowQuality != MyShadowsQuality.DISABLED) ? m_directionalEnvironmentLightPixel : m_directionalEnvironmentLightNoShadow);
			rc.PixelShader.Set(id);
			rc.AllShaderStages.SetConstantBuffer(4, MyManagers.Shadows.ShadowCascades.CascadeConstantBuffer);
			rc.PixelShader.SetSampler(15, MySamplerStateManager.Shadowmap);
			rc.PixelShader.SetSrv(10, MyManagers.Textures.GetTempTexture(MyRender11.Environment.Data.Skybox, new MyTextureStreamingManager.QueryArgs
			{
				SkipQualityReduction = true,
				TextureType = MyFileTextureEnum.CUBEMAP
			}, 10000));
			IRtvArrayTexture closeCubemapFinal = MyManagers.EnvironmentProbe.CloseCubemapFinal;
			IRtvArrayTexture farCubemapFinal = MyManagers.EnvironmentProbe.FarCubemapFinal;
			rc.PixelShader.SetSrv(11, closeCubemapFinal);
			rc.PixelShader.SetSrv(17, farCubemapFinal);
			rc.PixelShader.SetSrv(15, MyManagers.Shadows.ShadowCascades.CascadeShadowmapArray);
			rc.PixelShader.SetSrv(19, postProcessedShadows);
			rc.PixelShader.SetSrv(16, MyCommon.GetAmbientBrdfLut());
			rc.PixelShader.SetSrv(12, ambientOcclusion);
			MyScreenPass.RunFullscreenPixelFreq(rc, MyGBuffer.Main.LBuffer);
			if (MyRender11.MultisamplingEnabled)
			{
				rc.PixelShader.Set(m_directionalEnvironmentLightSample);
				MyScreenPass.RunFullscreenSampleFreq(rc, MyGBuffer.Main.LBuffer);
			}
			rc.PixelShader.SetSrv(19, null);
		}

		internal static void Resize(int width, int height)
		{
			m_tilesX = (width + 16 - 1) / 16;
			m_tilesY = (height + 16 - 1) / 16;
			m_tilesNum = m_tilesX * m_tilesY;
			Dispose();
<<<<<<< HEAD
			m_tileIndices = MyManagers.Buffers.CreateSrvUav("MyScreenDependants::tileIndices", m_tilesNum + m_tilesNum * m_maxPointLights, 4, null, MyUavType.Default, ResourceUsage.Default, isGlobal: true);
=======
			m_tileIndices = MyManagers.Buffers.CreateSrvUav("MyScreenDependants::tileIndices", m_tilesNum + m_tilesNum * 256, 4, null, MyUavType.Default, ResourceUsage.Default, isGlobal: true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		internal static MyCullResults GatherLights(Vector3D position, float radius)
		{
			m_gatherTempResult.Clear();
			BoundingSphereD sphere = new BoundingSphereD(position, radius);
			MyScene11.DynamicRenderablesDBVH.OverlapAllBoundingSphere(ref sphere, delegate((Action<MyCullResultsBase, bool>, object) x)
			{
				x.Item1(m_gatherTempResult, arg2: true);
			});
			MyScene11.ManualCullTree.OverlapAllBoundingSphere(ref sphere, delegate(MyManualCullTreeData x)
			{
				m_gatherTempResult.PointLights.AddRange(((MyCullResults)x.All).PointLights);
				m_gatherTempResult.SpotLights.AddRange(((MyCullResults)x.All).SpotLights);
			});
			return m_gatherTempResult;
		}

		public static void GatherLightsClear()
		{
			m_gatherTempResult.Clear();
		}

		internal static void Dispose()
		{
			if (m_tileIndices != null)
			{
				MyManagers.Buffers.Dispose(m_tileIndices);
			}
			m_tileIndices = null;
		}

		public static int GetTilesNum()
		{
			return m_tilesNum;
		}

		public static int GetTilesX()
		{
			return m_tilesX;
		}
	}
}
