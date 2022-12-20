using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.LightingStage;
using VRage.Render11.Profiler;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageRender;

namespace VRage.Render11.GBufferResolve
{
	internal static class MyGBufferResolver
	{
		private static MyFinishedContext m_fc;

		public static IBorrowedRtvTexture DebugAmbientOcclusion { get; private set; }

		public static int DoWork(MyCullQuery cullQuery)
		{
			MyRenderContext myRenderContext = MyManagers.DeferredRCs.AcquireRC("MyGBufferResolver");
			myRenderContext.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			MyManagers.EnvironmentProbe.FinalizeEnvProbes(myRenderContext);
			IBorrowedRtvTexture borrowedRtvTexture2 = (DebugAmbientOcclusion = MyManagers.RwTexturesPool.BorrowRtv("MyScreenDependants.AmbientOcclusion", MyRender11.ResolutionI.X, MyRender11.ResolutionI.Y, Format.R8_UNorm));
			int num = ((!MyStereoRender.Enable) ? 1 : 2);
			for (int i = 0; i < num; i++)
			{
				if (MyStereoRender.Enable)
				{
					MyStereoRender.RenderRegion = ((i == 0) ? MyStereoRegion.LEFT : MyStereoRegion.RIGHT);
				}
				MyGBuffer.Main.ResolveMultisample(myRenderContext);
				IBorrowedUavTexture borrowedUavTexture = MyManagers.Shadows.ShadowCascades.PostProcess(myRenderContext);
				if (MySSAO.Params.Enabled && MyRender11.Settings.User.AmbientOcclusionEnabled && MyRender11.DebugOverrides.Postprocessing && MyRender11.DebugOverrides.SSAO)
				{
					MySSAO.Run(myRenderContext, borrowedRtvTexture2, MyGBuffer.Main);
					if (MySSAO.Params.UseBlur)
					{
						IBorrowedRtvTexture borrowedRtvTexture3 = MyManagers.RwTexturesPool.BorrowRtv("MyScreenDependants.AmbientOcclusionHelper", MyRender11.ResolutionI.X, MyRender11.ResolutionI.Y, Format.R8_UNorm);
						MyBlur.Run(myRenderContext, borrowedRtvTexture2, borrowedRtvTexture3, borrowedRtvTexture2, 5, MyBlur.MyBlurDensityFunctionType.Gaussian, 1.5f, null, 0, new RawColor4(1f, 1f, 1f, 1f));
						borrowedRtvTexture3.Release();
					}
				}
				else if (MyHBAO.Params.Enabled && MyRender11.Settings.User.AmbientOcclusionEnabled && MyRender11.DebugOverrides.Postprocessing && MyRender11.DebugOverrides.SSAO)
				{
					MyHBAO.Run(myRenderContext, borrowedRtvTexture2, MyGBuffer.Main);
				}
				else
				{
					myRenderContext.ClearRtv(borrowedRtvTexture2, new RawColor4(1f, 1f, 1f, 1f));
				}
				MyManagers.Ansel.MarkHdrBufferBind();
				if (MyRender11.DebugOverrides.Lighting)
				{
					MyLightsRendering.Render(myRenderContext, cullQuery.Results, borrowedUavTexture, borrowedRtvTexture2);
				}
				borrowedUavTexture.Release();
			}
			MyStereoRender.RenderRegion = MyStereoRegion.FULLSCREEN;
			m_fc = myRenderContext.FinishDeferredContext();
			return 1;
		}

		public static void ConsumeWork()
		{
<<<<<<< HEAD
			MyGpuProfiler.IC_BeginBlock("MyGBufferResolver", "ConsumeWork", "E:\\Repo1\\Sources\\VRage.Render11\\GBufferResolve\\MyGBufferResolver.cs");
			MyRender11.RC.ExecuteContext(ref m_fc, "ConsumeWork", 105, "E:\\Repo1\\Sources\\VRage.Render11\\GBufferResolve\\MyGBufferResolver.cs");
			MyGpuProfiler.IC_EndBlock(0f, "ConsumeWork", "E:\\Repo1\\Sources\\VRage.Render11\\GBufferResolve\\MyGBufferResolver.cs");
=======
			MyGpuProfiler.IC_BeginBlock("MyGBufferResolver", "ConsumeWork", "E:\\Repo3\\Sources\\VRage.Render11\\GBufferResolve\\MyGBufferResolver.cs");
			MyRender11.RC.ExecuteContext(ref m_fc, "ConsumeWork", 105, "E:\\Repo3\\Sources\\VRage.Render11\\GBufferResolve\\MyGBufferResolver.cs");
			MyGpuProfiler.IC_EndBlock(0f, "ConsumeWork", "E:\\Repo3\\Sources\\VRage.Render11\\GBufferResolve\\MyGBufferResolver.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
