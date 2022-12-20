using VRage.Render11.Common;
using VRage.Render11.Profiler;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageRender;

namespace VRage.Render11.Culling.Occlusion
{
	internal static class MyOcclusionTask
	{
		private static MyFinishedContext m_finishedContext;

		public static void Schedule()
		{
		}

		public static void Consume()
		{
			if (m_finishedContext.CommandList != null)
			{
<<<<<<< HEAD
				MyGpuProfiler.IC_BeginBlock("MyOcclusionTask", "Consume", "E:\\Repo1\\Sources\\VRage.Render11\\Culling\\Occlusion\\MyOcclusionTask.cs");
				MyRender11.RC.ExecuteContext(ref m_finishedContext, "Consume", 26, "E:\\Repo1\\Sources\\VRage.Render11\\Culling\\Occlusion\\MyOcclusionTask.cs");
				MyGpuProfiler.IC_EndBlock(0f, "Consume", "E:\\Repo1\\Sources\\VRage.Render11\\Culling\\Occlusion\\MyOcclusionTask.cs");
=======
				MyGpuProfiler.IC_BeginBlock("MyOcclusionTask", "Consume", "E:\\Repo3\\Sources\\VRage.Render11\\Culling\\Occlusion\\MyOcclusionTask.cs");
				MyRender11.RC.ExecuteContext(ref m_finishedContext, "Consume", 26, "E:\\Repo3\\Sources\\VRage.Render11\\Culling\\Occlusion\\MyOcclusionTask.cs");
				MyGpuProfiler.IC_EndBlock(0f, "Consume", "E:\\Repo3\\Sources\\VRage.Render11\\Culling\\Occlusion\\MyOcclusionTask.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public static int DoWork(MyCullQuery cullQuery)
		{
			if (!MyRender11.DebugOverrides.Flares || MyManagers.Ansel.IsCaptureRunning)
			{
				return 0;
			}
			MyRenderContext myRenderContext = MyManagers.DeferredRCs.AcquireRC("MyOcclusionTask");
			MyOcclusionQuery myOcclusionQuery = MyOccllusionQueryFactory.CreateOcclusionQuery("Dummy");
			myOcclusionQuery.Begin(myRenderContext);
			IBorrowedSrvTexture borrowedSrvTexture = null;
			IDepthStencil ds;
			if (MyManagers.GeometryRenderer.HasTransparentInstances())
			{
				IDepthStencil resolvedDepthStencil = MyGBuffer.Main.ResolvedDepthStencil;
				IBorrowedDepthStencilTexture borrowedDepthStencilTexture = MyManagers.RwTexturesPool.BorrowDepthStencil("occlusion test", resolvedDepthStencil.Size.X, resolvedDepthStencil.Size.Y, MyRender11.Settings.User.HqDepth);
				myRenderContext.CopyResource(resolvedDepthStencil, borrowedDepthStencilTexture);
				myRenderContext.SetRtv(borrowedDepthStencilTexture.Dsv);
				MyManagers.GeometryRenderer.RenderTransparentFlareOccluders(myRenderContext);
				ds = borrowedDepthStencilTexture;
				borrowedSrvTexture = borrowedDepthStencilTexture;
			}
			else
			{
				ds = MyGBuffer.Main.ResolvedDepthStencil;
			}
			MyManagers.FlareOcclusionRenderer.Render(cullQuery.Results.PointLights, myRenderContext, ds, MyGBuffer.Main.LBuffer);
			borrowedSrvTexture?.Release();
			MyManagers.Cull.OcclusionCull(myRenderContext);
			myOcclusionQuery.End(myRenderContext);
			myOcclusionQuery.Return();
			m_finishedContext = myRenderContext.FinishDeferredContext();
			return 1;
		}
	}
}
