using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using VRage.Library.Collections;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.Profiler;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Scene.Components;
using VRage.Render11.Tools;
using VRageMath;

namespace VRageRender
{
	internal class MyTransparentRendering
	{
		private const float PROXIMITY_DECALS_SQ_TH = 2.25f;

		private static MyFinishedContext m_fc;

		private static MyPixelShaders.Id m_psResolve;

		private static MyPixelShaders.Id m_psOverlappingHeatMap;

		private static MyPixelShaders.Id m_psOverlappingHeatMapInGrayscale;

		private static float[] m_distances;

		private static bool PARALLEL_RENDERING = true;

		private static MyCullQuery m_cullQuery;

		private static readonly RenderTargetView[] m_rtmp = new RenderTargetView[2];

		private static readonly RenderTargetView[] m_rtmp1 = new RenderTargetView[2];

		private static IBorrowedUavTexture m_accumTarget;

		private static IBorrowedUavTexture m_coverageTarget;

		internal static void Init()
		{
			MyGPUParticleRenderer.Init();
			if (m_distances == null)
			{
				m_distances = new float[2] { 2.25f, 0f };
			}
			m_psResolve = MyPixelShaders.Create("Transparent/OIT/Resolve.hlsl");
			m_psOverlappingHeatMap = MyPixelShaders.Create("Transparent/ResolveAccumIntoHeatMap.hlsl");
			m_psOverlappingHeatMapInGrayscale = MyPixelShaders.Create("Transparent/ResolveAccumIntoHeatMap.hlsl", new ShaderMacro[1]
			{
				new ShaderMacro("USE_GRAYSCALE", null)
			});
		}

		internal static void OnDeviceReset()
		{
			MyGPUParticleRenderer.OnDeviceReset();
		}

		internal static void OnDeviceEnd()
		{
			MyGPUParticleRenderer.OnDeviceEnd();
		}

		internal static void OnSessionEnd()
		{
			MyGPUParticleRenderer.OnSessionEnd();
		}

		private static void SetupOIT(MyRenderContext rc, IDepthStencil depthStencil, IUavTexture accumTarget, IUavTexture coverageTarget, bool clear)
		{
			rc.SetScreenViewport();
			rc.SetBlendState(MyBlendStateManager.BlendWeightedTransparency);
			if (clear)
			{
				rc.ClearRtv(accumTarget, new RawColor4(0f, 0f, 0f, 0f));
				rc.ClearRtv(coverageTarget, new RawColor4(1f, 1f, 1f, 1f));
			}
			m_rtmp[0] = accumTarget.Rtv;
			m_rtmp[1] = coverageTarget.Rtv;
			rc.PixelShader.SetSrv(4, null);
			rc.SetRtvs(depthStencil.DsvRoDepth, m_rtmp);
		}

		private static void SetupStandard(MyRenderContext rc, IDepthStencil depthStencil)
		{
			rc.SetScreenViewport();
			rc.SetBlendState(MyBlendStateManager.BlendAlphaPremult);
			rc.SetRtv(depthStencil.DsvRoDepth, MyGBuffer.Main.LBuffer);
		}

		private static void SetupTargets(MyRenderContext rc, IDepthStencil depthStencil, IUavTexture accumTarget, IUavTexture coverageTarget, bool clear)
		{
			if (MyRender11.DebugOverrides.OIT)
			{
				SetupOIT(rc, depthStencil, accumTarget, coverageTarget, clear);
			}
			else
			{
				SetupStandard(rc, depthStencil);
			}
		}

		private static void ResolveOIT(MyRenderContext rc, ISrvBindable accumTarget, ISrvBindable coverageTarget)
		{
			rc.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
			rc.SetBlendState(MyBlendStateManager.BlendWeightedTransparencyResolve);
			rc.PixelShader.Set(m_psResolve);
			rc.SetRtv(MyGBuffer.Main.LBuffer);
			rc.PixelShader.SetSrv(0, accumTarget);
			rc.PixelShader.SetSrv(1, coverageTarget);
			MyScreenPass.DrawFullscreenQuad(rc);
		}

		internal static bool DisplayTransparencyHeatMap()
		{
			if (!MyRender11.Settings.DisplayTransparencyHeatMap)
			{
				return MyRender11.Settings.DisplayTransparencyHeatMapInGrayscale;
			}
			return true;
		}

		private static void DisplayOverlappingHeatMap(MyRenderContext rc, IUavTexture accumTarget, IUavTexture coverageTarget, bool useGrayscale)
		{
			IBorrowedRtvTexture borrowedRtvTexture = MyManagers.RwTexturesPool.BorrowRtv("MyTransparentRendering.HeatMap", Format.R8G8B8A8_UNorm);
			rc.ClearRtv(borrowedRtvTexture, default(RawColor4));
			rc.SetRtv(borrowedRtvTexture);
			rc.PixelShader.SetSrv(0, accumTarget);
			rc.PixelShader.Set(useGrayscale ? m_psOverlappingHeatMapInGrayscale : m_psOverlappingHeatMap);
			rc.SetBlendState(MyBlendStateManager.BlendAdditive);
			rc.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
			MyScreenPass.DrawFullscreenQuad(rc);
			rc.PixelShader.Set(null);
			rc.PixelShader.SetSrv(0, null);
			rc.SetRtvNull();
			SetupOIT(rc, MyGBuffer.Main.DepthStencil, accumTarget, coverageTarget, clear: false);
			MyDebugTextureDisplay.Select(borrowedRtvTexture);
			borrowedRtvTexture.Release();
		}

		internal static int DoWork(MyCullQuery cullQuery)
		{
			if (!MyRender11.DebugOverrides.Transparent)
			{
				return 0;
			}
			if (!PARALLEL_RENDERING)
			{
				m_cullQuery = cullQuery;
				return 0;
			}
			MyRenderContext myRenderContext = MyManagers.DeferredRCs.AcquireRC("MyTransparentRendering");
			Render(myRenderContext, cullQuery.Results.PointLights, MyRender11.Settings.FlaresIntensity);
			m_fc = myRenderContext.FinishDeferredContext();
			return 1;
		}

		public static void ConsumeWork()
		{
			if (MyRender11.DebugOverrides.Transparent)
			{
				if (!PARALLEL_RENDERING)
				{
					Render(MyRender11.RC, m_cullQuery.Results.PointLights, MyRender11.Settings.FlaresIntensity, immediateContext: true);
					m_cullQuery = null;
					return;
				}
				MyBillboardRenderer.TransferData(MyRender11.RC);
<<<<<<< HEAD
				MyGpuProfiler.IC_BeginBlock("MyTransparentRendering", "ConsumeWork", "E:\\Repo1\\Sources\\VRage.Render11\\TransparentStage\\MyTransparentRendering.cs");
				MyRender11.RC.ExecuteContext(ref m_fc, "ConsumeWork", 163, "E:\\Repo1\\Sources\\VRage.Render11\\TransparentStage\\MyTransparentRendering.cs");
				MyGpuProfiler.IC_EndBlock(0f, "ConsumeWork", "E:\\Repo1\\Sources\\VRage.Render11\\TransparentStage\\MyTransparentRendering.cs");
=======
				MyGpuProfiler.IC_BeginBlock("MyTransparentRendering", "ConsumeWork", "E:\\Repo3\\Sources\\VRage.Render11\\TransparentStage\\MyTransparentRendering.cs");
				MyRender11.RC.ExecuteContext(ref m_fc, "ConsumeWork", 163, "E:\\Repo3\\Sources\\VRage.Render11\\TransparentStage\\MyTransparentRendering.cs");
				MyGpuProfiler.IC_EndBlock(0f, "ConsumeWork", "E:\\Repo3\\Sources\\VRage.Render11\\TransparentStage\\MyTransparentRendering.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private static void Render(MyRenderContext rc, MyList<MyLightComponent> lightList, float flareIntensity, bool immediateContext = false)
		{
			m_accumTarget = MyManagers.RwTexturesPool.BorrowUav("MyTransparentRendering.AccumTarget", Format.R16G16B16A16_Float);
			m_coverageTarget = MyManagers.RwTexturesPool.BorrowUav("MyTransparentRendering.CoverageTarget", Format.R8_UNorm);
			rc.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			rc.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			rc.PixelShader.SetSampler(15, MySamplerStateManager.Shadowmap);
			rc.AllShaderStages.SetConstantBuffer(4, MyManagers.Shadows.ShadowCascades.CascadeConstantBufferOld);
			rc.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			MyAtmosphereRenderer.RenderGBuffer(rc);
			if (MyRender11.DebugOverrides.Clouds)
			{
				MyCloudRenderer.Render(rc, uint.MaxValue);
			}
			IDepthStencil resolvedDepthStencil = MyGBuffer.Main.ResolvedDepthStencil;
			if (MyRender11.Settings.DrawBillboards)
			{
				if (MyRender11.DebugOverrides.Flares)
				{
					MyFlareRenderer.Draw(lightList, flareIntensity);
				}
				MyBillboardRenderer.Gather(rc, immediateContext);
				if (MyRender11.Settings.DrawBillboardsBottom)
				{
					MyBillboardRenderer.RenderAdditiveBottom(rc, resolvedDepthStencil.SrvDepth);
				}
				SetupTargets(rc, MyGBuffer.Main.DepthStencil, m_accumTarget, m_coverageTarget, clear: true);
				if (MyRender11.Settings.DrawBillboardsStandard)
				{
					MyBillboardRenderer.RenderStandard(rc, resolvedDepthStencil.SrvDepth);
				}
			}
			else
			{
				SetupTargets(rc, MyGBuffer.Main.DepthStencil, m_accumTarget, m_coverageTarget, clear: true);
			}
			if (MyRender11.DebugOverrides.GPUParticles)
			{
				MyGPUParticleRenderer.Run(rc, resolvedDepthStencil.SrvDepth, MyGBuffer.Main.GBuffer1, immediateContext);
			}
			if (MyRender11.Settings.DrawTransparentModels || MyRender11.Settings.DrawTransparentModelsInstanced)
			{
				IBorrowedRtvTexture gbuffer1CopyRtv = MyGBuffer.Main.GetGbuffer1CopyRtv();
				if (MyTransparentModelRenderer.Renderables.Count > 0 && MyRender11.Settings.DrawTransparentModels)
				{
					MyTransparentModelRenderer.Render(rc, HandleTransparentModels);
					if (MyScreenDecals.AnyTransparentDecalsToDraw)
					{
						float squaredDistanceMax = MyScreenDecals.VISIBLE_DECALS_SQ_TH;
						IBorrowedDepthStencilTexture borrowedDepthStencilTexture = MyManagers.RwTexturesPool.BorrowDepthStencil("DsTheSecondLayer", resolvedDepthStencil.Size.X, resolvedDepthStencil.Size.Y, MyRender11.Settings.User.HqDepth);
						rc.CopyResource(resolvedDepthStencil, borrowedDepthStencilTexture);
						for (int i = 0; i < m_distances.Length; i++)
						{
							float num = m_distances[i];
							if (MyTransparentModelRenderer.RenderDepthOnly(rc, borrowedDepthStencilTexture, MyGBuffer.Main.LBuffer, num, squaredDistanceMax))
							{
								SetupTargets(rc, resolvedDepthStencil, m_accumTarget, m_coverageTarget, clear: false);
								MyScreenDecals.Render(rc, gbuffer1CopyRtv, borrowedDepthStencilTexture.SrvDepth, transparent: true);
							}
							squaredDistanceMax = num;
						}
						borrowedDepthStencilTexture.Release();
					}
				}
				if (MyManagers.GeometryRenderer.HasTransparentInstances() && MyRender11.Settings.DrawTransparentModelsInstanced)
				{
					SetupTargets(rc, MyGBuffer.Main.DepthStencil, m_accumTarget, m_coverageTarget, clear: false);
					MyManagers.GeometryRenderer.RenderTransparent(rc);
					if (MyScreenDecals.AnyTransparentDecalsToDraw)
					{
						IDepthStencil resolvedDepthStencil2 = MyGBuffer.Main.ResolvedDepthStencil;
						IBorrowedDepthStencilTexture borrowedDepthStencilTexture2 = MyManagers.RwTexturesPool.BorrowDepthStencil("DsTheSecondLayer", resolvedDepthStencil2.Size.X, resolvedDepthStencil2.Size.Y, MyRender11.Settings.User.HqDepth);
						rc.CopyResource(resolvedDepthStencil2, borrowedDepthStencilTexture2);
						IBorrowedDepthStencilTexture borrowedDepthStencilTexture3 = MyManagers.RwTexturesPool.BorrowDepthStencil("DsTheSecondLayer", borrowedDepthStencilTexture2.Size.X, borrowedDepthStencilTexture2.Size.Y, MyRender11.Settings.User.HqDepth);
						rc.CopyResource(resolvedDepthStencil2, borrowedDepthStencilTexture3);
						MyManagers.GeometryRenderer.RenderTransparentForDecals(rc, borrowedDepthStencilTexture2, borrowedDepthStencilTexture3, gbuffer1CopyRtv);
						SetupTargets(rc, borrowedDepthStencilTexture2, m_accumTarget, m_coverageTarget, clear: false);
						MyScreenDecals.Render(rc, gbuffer1CopyRtv, borrowedDepthStencilTexture2, transparent: true);
						MyManagers.GeometryRenderer.RenderTransparentForDecals(rc, borrowedDepthStencilTexture3, borrowedDepthStencilTexture2, gbuffer1CopyRtv);
						SetupTargets(rc, borrowedDepthStencilTexture3, m_accumTarget, m_coverageTarget, clear: false);
						MyScreenDecals.Render(rc, gbuffer1CopyRtv, borrowedDepthStencilTexture3, transparent: true);
						borrowedDepthStencilTexture2.Release();
						borrowedDepthStencilTexture3.Release();
					}
				}
			}
			if (DisplayTransparencyHeatMap())
			{
				DisplayOverlappingHeatMap(rc, m_accumTarget, m_coverageTarget, MyRender11.Settings.DisplayTransparencyHeatMapInGrayscale);
			}
			if (MyRender11.DebugOverrides.OIT)
			{
				ResolveOIT(rc, m_accumTarget, m_coverageTarget);
			}
			if (MyRender11.Settings.DrawBillboards && MyRender11.Settings.DrawBillboardsTop)
			{
				MyBillboardRenderer.RenderAdditiveTop(rc);
			}
			m_coverageTarget.Release();
			m_accumTarget.Release();
		}

		/// <returns>True if window Transparent models decals and is not too far</returns>
		private static bool HandleTransparentModels(MyRenderCullResultFlat result, double viewDistanceSquared)
		{
			if (MyScreenDecals.HasEntityDecals(result.RenderProxy.Parent.Owner.ID) && viewDistanceSquared < (double)MyScreenDecals.VISIBLE_DECALS_SQ_TH)
			{
				return true;
			}
			return false;
		}

		public static void RenderForward(MyRenderContext rc, IRtvBindable dest0, IRtvBindable dest1, IDsvBindable depthStencil, ref Matrix viewMatrix, ref Matrix projMatrix, ref Vector3D position, ISrvBindable dsv, uint? nearestAtmosphereId, ISrvBindable lastFrameSource, ISrvBindable lastFrame)
		{
			rc.SetViewport(0f, 0f, dest0.Size.X, dest0.Size.Y);
			m_rtmp1[0] = dest0.Rtv;
			m_rtmp1[1] = dest1.Rtv;
			Matrix viewProj = viewMatrix * projMatrix;
			if (nearestAtmosphereId.HasValue && MyAtmosphereRenderer.IsValid(nearestAtmosphereId.Value))
			{
				rc.SetRtvs(m_rtmp1);
				rc.PixelShader.SetSrv(0, dsv);
				MyAtmosphereRenderer.RenderEnvProbe(rc, position, ref viewProj, nearestAtmosphereId.Value);
				rc.PixelShader.SetSrv(0, null);
			}
			rc.SetRtvNull();
		}
	}
}
