using System;
using ParallelTasks;
using VRage.Profiler;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.Culling.Frustum;
using VRage.Render11.Culling.Occlusion;
using VRage.Render11.GBufferResolve;
using VRage.Render11.Profiler;
using VRage.Render11.Resources;
using VRage.Render11.Scene;
using VRage.Render11.Tools;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Render
{
	internal class MyRenderScheduler : IManager, IManagerDevice
	{
		private class RenderJob
		{
			public MyCullQuery CullQuery;

			public readonly Action JobFunc;

			private readonly string m_taskName;

			private readonly MyProfiler.TaskType m_taskType;

			private readonly Func<MyCullQuery, int> m_jobFunction;

			public RenderJob(string taskName, MyProfiler.TaskType taskType, Func<MyCullQuery, int> job)
			{
				JobFunc = Invoke;
				m_jobFunction = job;
				m_taskType = taskType;
				m_taskName = taskName;
			}

			private void Invoke()
			{
				bool flag = CullQuery != null;
				m_jobFunction(CullQuery);
			}
		}

		private struct JobAllocator
		{
			private RenderJob[] m_pool;

			private int m_allocationIndex;

			private readonly Func<RenderJob> m_allocator;

			public JobAllocator(string taskName, MyProfiler.TaskType taskType, Func<MyCullQuery, int> job)
			{
				m_pool = null;
				m_allocationIndex = 0;
				m_allocator = () => new RenderJob(taskName, taskType, job);
			}

			public void Reset()
			{
				m_allocationIndex = 0;
			}

			public Action Get(MyCullQuery cullQuery)
			{
				int num = m_allocationIndex++;
				int num2 = ((m_pool != null) ? m_pool.Length : 0);
				if (num2 < m_allocationIndex)
				{
					Array.Resize(ref m_pool, Math.Max(num2 * 2, 10));
					for (int i = num2; i < m_pool.Length; i++)
					{
						m_pool[i] = m_allocator();
					}
				}
				RenderJob obj = m_pool[num];
				obj.CullQuery = cullQuery;
				return obj.JobFunc;
			}
		}

		private readonly DependencyBatch m_batch = new DependencyBatch(WorkPriority.VeryHigh);

		private DependencyResolver m_resolver;

		private readonly MyRendererStats.MyCullStats[] m_lastCullStats = new MyRendererStats.MyCullStats[19];

		private readonly DependencyResolver.JobToken[] m_oldPrepareJob = new DependencyResolver.JobToken[19];

		private JobAllocator m_cullJobAllocator = new JobAllocator("CullWork", MyProfiler.TaskType.RenderCull, MyFrustumCullingWork.ExecuteCulling);

		private JobAllocator m_processDecalsJobAllocator = new JobAllocator("ScreenDecals.Process", MyProfiler.TaskType.PreparePass, MyScreenDecals.Preprocess);

		private JobAllocator m_renderGbufferDecalsJobAllocator = new JobAllocator("ScreenDecals.RenderGbuffer", MyProfiler.TaskType.RenderPass, MyScreenDecals.RenderGbuffer);

		private JobAllocator m_gpuParticleUpdateJobAllocator = new JobAllocator("MyGPUParticleRenderer.Update", MyProfiler.TaskType.RenderPass, MyGPUParticleRenderer.Update);

		private JobAllocator m_foliageRenderJobAllocator = new JobAllocator("FoliageRenderer", MyProfiler.TaskType.RenderPass, MyManagers.FoliageRenderer.Render);

		private JobAllocator m_lightsPreprocessJobAllocator = new JobAllocator("MyLightsRendering.PostprocessCulling", MyProfiler.TaskType.PreparePass, MyManagers.Shadows.SetVisibleLights);

		private JobAllocator m_geometryRendererOldUpdateCullProxiesJobAllocator = new JobAllocator("GeometryRendererOld.UpdateCullProxies", MyProfiler.TaskType.PreparePass, MyManagers.GeometryRendererOld.ProcessCullProxies);

		private JobAllocator m_sendGlobalOutputMessagesJobAllocator = new JobAllocator("SendGlobalOutputMessages", MyProfiler.TaskType.RenderCull, MyManagers.Cull.SendGlobalOutputMessages);

		private JobAllocator m_geometryRendererOldPrepareJobAllocator = new JobAllocator("GeometryRendererOld.Prepare", MyProfiler.TaskType.PreparePass, MyManagers.GeometryRendererOld.Prepare);

		private JobAllocator m_geometryRendererPreprocessJobAllocator = new JobAllocator("GeometryRenderer.Preprocess", MyProfiler.TaskType.PreparePass, MyManagers.GeometryRenderer.Preprocess);

		private JobAllocator m_geometryRendererPreprocessTransparentJobAllocator = new JobAllocator("GeometryRenderer.TransparentPrepare", MyProfiler.TaskType.PreparePass, MyManagers.GeometryRenderer.PreprocessTransparentPass);

		private JobAllocator m_geometryRendererRenderJobAllocator = new JobAllocator("GeometryRenderer.Render", MyProfiler.TaskType.RenderPass, MyManagers.GeometryRenderer.Render);

		private JobAllocator m_geometryRendererOldRenderJobAllocator = new JobAllocator("GeometryRendererOld.Render", MyProfiler.TaskType.RenderPass, MyManagers.GeometryRendererOld.Render);

		private JobAllocator m_occlusionCullJobAllocator = new JobAllocator("MyOcclusionTask", MyProfiler.TaskType.RenderCull, MyOcclusionTask.DoWork);

		private JobAllocator m_gbufferResolverJobAllocator = new JobAllocator("MyGBufferResolver", MyProfiler.TaskType.RenderPass, MyGBufferResolver.DoWork);

		private JobAllocator m_transparentRendereringJobAllocator = new JobAllocator("MyTransparentRendering", MyProfiler.TaskType.RenderPass, MyTransparentRendering.DoWork);

		public void Init()
		{
			WorkOptions options = m_batch.Options;
			options.MaximumThreads = MyRender11.Settings.RenderThreadCount;
			m_batch.Options = options;
<<<<<<< HEAD
			MyGpuProfiler.IC_BeginBlock("Init", "Init", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRenderScheduler.cs");
=======
			MyGpuProfiler.IC_BeginBlock("Init", "Init", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRenderScheduler.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyGBuffer.Main.Clear(MyRender11.RC, Color.Black);
			MyManagers.Cull.InitFrame(MyRender11.RC);
			MyManagers.GeometryRendererOld.InitFrame();
			MyCullQueries cullQueries = MyManagers.Cull.GetCullQueries();
			MyManagers.GeometryRenderer.InitFrame(cullQueries);
			MyGBuffer.Main.InitFrame();
<<<<<<< HEAD
			MyGpuProfiler.IC_EndBlock(0f, "Init", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRenderScheduler.cs");
=======
			MyGpuProfiler.IC_EndBlock(0f, "Init", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRenderScheduler.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			using (m_resolver)
			{
				MyCullQuery cullQuery = null;
				for (int i = 0; i < cullQueries.Size; i++)
				{
					MyCullQuery myCullQuery = cullQueries.CullQueries[i];
					myCullQuery.Job = m_resolver.Add(m_cullJobAllocator.Get(myCullQuery));
					if (myCullQuery.ViewType == MyViewType.Main)
					{
						cullQuery = myCullQuery;
					}
				}
				DependencyResolver.JobToken parent = m_resolver.Add(m_processDecalsJobAllocator.Get(null));
				m_resolver.Add(m_renderGbufferDecalsJobAllocator.Get(null)).DependsOn(parent);
				DependencyResolver.JobToken parent2 = m_resolver.Add(m_gpuParticleUpdateJobAllocator.Get(null));
				m_resolver.Add(MyScene11.Instance.VicinityUpdatesJob);
				DependencyResolver.JobToken parent3 = default(DependencyResolver.JobToken);
				int num = int.MaxValue;
				DependencyResolver.JobToken jobToken = default(DependencyResolver.JobToken);
				DependencyResolver.JobToken jobToken2 = default(DependencyResolver.JobToken);
				for (int j = 0; j < cullQueries.Size; j++)
				{
					MyCullQuery myCullQuery2 = cullQueries.CullQueries[j];
					DependencyResolver.JobToken parent4 = m_resolver.Add(m_geometryRendererPreprocessJobAllocator.Get(myCullQuery2)).DependsOn(myCullQuery2.Job);
					DependencyResolver.JobToken jobToken3 = m_resolver.Add(m_geometryRendererOldUpdateCullProxiesJobAllocator.Get(myCullQuery2)).DependsOn(myCullQuery2.Job);
					m_oldPrepareJob[j] = m_resolver.Add(m_geometryRendererOldPrepareJobAllocator.Get(myCullQuery2)).DependsOn(jobToken3);
					if (myCullQuery2.ViewType == MyViewType.Main)
					{
						jobToken = m_resolver.Add(m_geometryRendererPreprocessTransparentJobAllocator.Get(myCullQuery2)).DependsOn(parent4);
						m_resolver.Add(m_foliageRenderJobAllocator.Get(myCullQuery2)).DependsOn(myCullQuery2.Job);
						jobToken2 = m_resolver.Add(m_lightsPreprocessJobAllocator.Get(myCullQuery2)).DependsOn(myCullQuery2.Job);
						m_resolver.Add(m_gbufferResolverJobAllocator.Get(myCullQuery2)).DependsOn(jobToken2);
						m_resolver.Add(m_transparentRendereringJobAllocator.Get(myCullQuery2)).DependsOn(parent).DependsOn(jobToken2)
							.DependsOn(jobToken)
							.DependsOn(parent2)
							.DependsOn(m_oldPrepareJob[j]);
						DependencyResolver.JobToken jobToken4 = m_resolver.Add(m_occlusionCullJobAllocator.Get(cullQuery)).DependsOn(jobToken).DependsOn(jobToken2);
						for (int k = 0; k < cullQueries.Size; k++)
						{
							MyCullQuery myCullQuery3 = cullQueries.CullQueries[k];
							if (MyCullManager.SupportsOcclusion(myCullQuery3.ViewType))
							{
								jobToken4.DependsOn(myCullQuery3.Job);
							}
						}
						m_resolver.Add(m_sendGlobalOutputMessagesJobAllocator.Get(myCullQuery2)).DependsOn(jobToken3);
					}
					if (myCullQuery2.ViewType == MyViewType.ShadowCascade)
					{
						if (num != int.MaxValue)
						{
							jobToken3.DependsOn(parent3);
						}
						parent3 = jobToken3;
						num = myCullQuery2.ViewIndex;
					}
					m_resolver.Add(m_geometryRendererRenderJobAllocator.Get(myCullQuery2)).DependsOn(parent4);
				}
				for (int l = 0; l < cullQueries.Size; l++)
				{
					MyCullQuery myCullQuery4 = cullQueries.CullQueries[l];
					int num2 = ((m_lastCullStats[myCullQuery4.ViewId].CullProxies <= 100) ? 1 : m_batch.MaxThreads);
					if (num2 == 1 && myCullQuery4.ViewType == MyViewType.Main)
					{
						num2++;
					}
					for (int m = 0; m < num2; m++)
					{
						m_resolver.Add(m_geometryRendererOldRenderJobAllocator.Get(myCullQuery4)).DependsOn(m_oldPrepareJob[l]);
					}
				}
			}
		}

		public void Execute()
		{
			MyRender11.LockImmediateRC = true;
			m_batch.Execute();
			MyRender11.LockImmediateRC = false;
		}

		public void Done()
		{
<<<<<<< HEAD
			MyGpuProfiler.IC_BeginBlock("GeometryRenderer", "Done", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRenderScheduler.cs");
			MyManagers.GeometryRendererOld.DoneFrame();
			MyGpuProfiler.IC_BeginNextBlock("GeometryInstanceRenderer", 0f, "Done", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRenderScheduler.cs");
			MyManagers.GeometryRenderer.DoneFrame();
			MyGpuProfiler.IC_EndBlock(0f, "Done", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRenderScheduler.cs");
=======
			MyManagers.Cull.DoneFrame();
			MyCullQueries cullQueries = MyManagers.Cull.GetCullQueries();
			for (int i = 0; i < cullQueries.Size; i++)
			{
				MyCullQuery myCullQuery = cullQueries.CullQueries[i];
				m_lastCullStats[myCullQuery.ViewId] = MyRendererStats.ViewCullStats[myCullQuery.ViewId];
			}
			MyGpuProfiler.IC_BeginBlock("GeometryRenderer", "Done", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRenderScheduler.cs");
			MyManagers.GeometryRendererOld.DoneFrame();
			MyGpuProfiler.IC_BeginNextBlock("GeometryInstanceRenderer", 0f, "Done", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRenderScheduler.cs");
			MyManagers.GeometryRenderer.DoneFrame();
			MyGpuProfiler.IC_EndBlock(0f, "Done", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRenderScheduler.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyHighlight.CopyDepthStencil(MyRender11.RC);
			MyGBuffer.Main.UpdateGbuffer1CopyRtv(MyRender11.RC);
			MyScreenDecals.ConsumeDrawDeferred();
			MyManagers.FoliageGenerator.Consume();
			MyManagers.FoliageRenderer.Consume();
			MyOcclusionTask.Consume();
			MyGBufferResolver.ConsumeWork();
			MyTransparentRendering.ConsumeWork();
<<<<<<< HEAD
			MyManagers.Cull.DoneFrame();
			MyCullQueries cullQueries = MyManagers.Cull.GetCullQueries();
			for (int i = 0; i < cullQueries.Size; i++)
			{
				MyCullQuery myCullQuery = cullQueries.CullQueries[i];
				m_lastCullStats[myCullQuery.ViewId] = MyRendererStats.ViewCullStats[myCullQuery.ViewId];
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyGBuffer.Main.DoneFrame();
			MyRender11.RC.ClearState();
			m_cullJobAllocator.Reset();
			m_processDecalsJobAllocator.Reset();
			m_renderGbufferDecalsJobAllocator.Reset();
			m_gpuParticleUpdateJobAllocator.Reset();
			m_foliageRenderJobAllocator.Reset();
			m_lightsPreprocessJobAllocator.Reset();
			m_geometryRendererOldUpdateCullProxiesJobAllocator.Reset();
			m_sendGlobalOutputMessagesJobAllocator.Reset();
			m_geometryRendererOldPrepareJobAllocator.Reset();
			m_geometryRendererPreprocessJobAllocator.Reset();
			m_geometryRendererPreprocessTransparentJobAllocator.Reset();
			m_geometryRendererRenderJobAllocator.Reset();
			m_geometryRendererOldRenderJobAllocator.Reset();
			m_occlusionCullJobAllocator.Reset();
			m_gbufferResolverJobAllocator.Reset();
			m_transparentRendereringJobAllocator.Reset();
		}

		public void OnDeviceInit()
		{
			m_resolver = new DependencyResolver(m_batch);
		}

		public void OnDeviceReset()
		{
			OnDeviceEnd();
			OnDeviceInit();
		}

		public void OnDeviceEnd()
		{
			m_batch.Dispose();
			m_resolver = null;
		}
	}
}
