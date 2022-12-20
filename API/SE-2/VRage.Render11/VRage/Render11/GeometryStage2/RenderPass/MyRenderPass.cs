#define VRAGE
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using ParallelTasks;
using VRage.Library.Collections;
using VRage.Profiler;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Rendering;
using VRage.Render11.Profiler;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Tools;
using VRageMath;
using VRageRender;

namespace VRage.Render11.GeometryStage2.RenderPass
{
	internal abstract class MyRenderPass : IPrioritizedWork, IWork, IPooledObject
	{
		private bool m_isDeferredRC;

		private bool m_rcOwner;

		protected MyRenderContext m_RC;

		protected MyRenderData m_singleRenderData;

		private MyList<MyRenderData> m_multiRenderData;

		private string m_debugName;

		protected MyRendererStats.MyRenderStats m_stats;

		private static readonly ConcurrentDictionary<string, string> m_debugPassNames = new ConcurrentDictionary<string, string>();

		private int m_logGroupsCount;

		private MyFinishedContext m_finishedContext;

		private long m_started;

		private long m_elapsed;

		private int m_ctr;

		private int m_beginCtr;

		private int m_endCtr;

		private int m_finishedCtr;

		private int m_executedCtr;

		public int ViewId { get; private set; }

		public WorkPriority Priority => WorkPriority.VeryHigh;

		public WorkOptions Options => Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.RenderPass, "RenderPass");

		protected abstract void BeginDraw(MyRenderContext RC);

		protected abstract void SetRenderData(MyRenderContext RC, IVertexBuffer vbInstanceBuffer, Matrix matrix);

		protected abstract void DrawInstanceLodGroup(MyRenderContext RC, MyInstanceLodGroup instanceLodGroup);

		protected abstract void EndDraw(MyRenderContext RC);

		protected void InitInternal(int viewId, string debugName, bool isUsedDeferredRC, MyRenderData singleRenderData, MyList<MyRenderData> multiRenderData = null, MyRenderContext rc = null)
		{
			m_ctr++;
			m_singleRenderData = singleRenderData;
			m_multiRenderData = multiRenderData;
			ViewId = viewId;
<<<<<<< HEAD
			if (!m_debugPassNames.TryGetValue(debugName, out m_debugName))
=======
			if (!m_debugPassNames.TryGetValue(debugName, ref m_debugName))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_debugPassNames.TryAdd(debugName, m_debugName = "N" + debugName);
			}
			m_stats = default(MyRendererStats.MyRenderStats);
			m_isDeferredRC = isUsedDeferredRC;
			m_rcOwner = rc == null;
			if (rc != null)
			{
				m_RC = rc;
			}
			m_stats = default(MyRendererStats.MyRenderStats);
			m_logGroupsCount = 0;
		}

		private void ProcessRenderData(MyRenderData renderData)
		{
			if (renderData.InstanceLodGroups.Count == 0)
			{
				return;
			}
			SetRenderData(m_RC, renderData.VBInstanceBuffer, renderData.ViewProjMatrix);
			foreach (MyInstanceLodGroup instanceLodGroup in renderData.InstanceLodGroups)
			{
				m_RC.SetEventMarker(instanceLodGroup.Lod.DebugName);
				DrawInstanceLodGroup(m_RC, instanceLodGroup);
			}
			m_logGroupsCount += renderData.InstanceLodGroups.Count;
		}

		public bool HasWork()
		{
			if (m_singleRenderData != null && m_singleRenderData.InstanceLodGroups.Count != 0)
			{
				return true;
			}
			if (m_multiRenderData != null)
			{
				foreach (MyRenderData multiRenderDatum in m_multiRenderData)
				{
					if (multiRenderDatum != null && multiRenderDatum.InstanceLodGroups.Count != 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void DoWork(WorkData workData = null)
		{
			Begin();
			m_RC.SetEventMarker("Begin");
			BeginDraw(m_RC);
			if (m_singleRenderData != null)
			{
				ProcessRenderData(m_singleRenderData);
			}
			if (m_multiRenderData != null)
			{
				foreach (MyRenderData multiRenderDatum in m_multiRenderData)
				{
					if (multiRenderDatum != null)
					{
						ProcessRenderData(multiRenderDatum);
					}
				}
			}
			m_RC.SetEventMarker("End");
			EndDraw(m_RC);
			End();
		}

		private void Begin()
		{
			m_beginCtr = m_ctr;
			m_started = Stopwatch.GetTimestamp();
			if (m_rcOwner)
			{
				m_RC = (m_isDeferredRC ? MyManagers.DeferredRCs.AcquireRC(m_debugName) : MyRender11.RC);
			}
		}

		private void End()
		{
			m_endCtr = m_ctr;
			if (m_rcOwner && m_isDeferredRC)
			{
				m_finishedCtr = m_ctr;
				m_finishedContext = m_RC.FinishDeferredContext();
			}
			m_RC = null;
			m_elapsed = Stopwatch.GetTimestamp() - m_started;
		}

		public void PostprocessWork()
		{
			if (m_isDeferredRC && m_rcOwner)
			{
<<<<<<< HEAD
				MyGpuProfiler.IC_BeginBlock(m_debugName, "PostprocessWork", "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage2\\RenderPass\\MyRenderPass.cs");
				try
				{
					MyRender11.RC.ExecuteContext(ref m_finishedContext, "PostprocessWork", 158, "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage2\\RenderPass\\MyRenderPass.cs");
=======
				MyGpuProfiler.IC_BeginBlock(m_debugName, "PostprocessWork", "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage2\\RenderPass\\MyRenderPass.cs");
				try
				{
					MyRender11.RC.ExecuteContext(ref m_finishedContext, "PostprocessWork", 158, "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage2\\RenderPass\\MyRenderPass.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				catch (Exception innerException)
				{
					MyRenderProxy.Log.WriteLine("Exception during MyRenderPass.PostprocessWork.ExecuteContext:");
					try
					{
						MyRenderProxy.Log.WriteLine($"{m_ctr} {m_beginCtr} {m_endCtr} {m_finishedCtr} {m_executedCtr}");
					}
					catch
					{
					}
					throw new Exception("Exception during ExecuteContext", innerException);
				}
				m_executedCtr = m_ctr;
<<<<<<< HEAD
				MyGpuProfiler.IC_EndBlock(m_logGroupsCount, "PostprocessWork", "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage2\\RenderPass\\MyRenderPass.cs");
=======
				MyGpuProfiler.IC_EndBlock(m_logGroupsCount, "PostprocessWork", "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage2\\RenderPass\\MyRenderPass.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			MyRendererStats.AddViewRenderStats(ViewId, ref m_stats);
		}

		/// <inheritdoc />
		void IPooledObject.Cleanup()
		{
		}
	}
}
