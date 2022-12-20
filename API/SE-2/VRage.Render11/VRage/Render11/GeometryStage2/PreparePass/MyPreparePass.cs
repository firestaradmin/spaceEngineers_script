#define VRAGE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SharpDX.Direct3D11;
using VRage.Library.Collections;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Common;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.Lodding;
using VRage.Render11.GeometryStage2.Model;
using VRage.Render11.GeometryStage2.PrepareGroupPass;
using VRage.Render11.GeometryStage2.Rendering;
using VRage.Render11.Profiler;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageRender;

namespace VRage.Render11.GeometryStage2.PreparePass
{
	internal class MyPreparePass<TCustomPass0, TCustomPass1> : IPrepareWork, IPooledObject where TCustomPass0 : struct, ICustomPreparePass0 where TCustomPass1 : struct, ICustomPreparePass1
	{
		private struct MyPreparedLod
		{
			public MyLod Lod;

			public MyLodInstance LodInstance;

			public MyInstanceLodState StateId;

			public float StateData;

			public MyInstance Instance;
		}

		private TCustomPass0 m_customPass0 = new TCustomPass0();

		private TCustomPass1 m_customPass1 = new TCustomPass1();

		private MyList<MyPreparedLod> m_preparedLodData = new MyList<MyPreparedLod>();

		private MyRenderContext m_RC;

		private MyList<MyInstance> m_visibleInstances;

		private MyRenderData m_outputRenderData;

		private MyInstanceLodGroupCounts m_tmpInstanceLodsCounts;

		private MyInstanceLodGroupCounts m_tmpInstanceLodsOffsets;

		private string m_debugName;

		private int m_frustumIndex;

		private int m_elementsCount;

		private int m_maxLodId;

		private MyFinishedContext m_finishedContext;

		private long m_started;

		private long m_elapsed;

		public int PassId { get; private set; }

		public void InitInstanceBuffer(MyRenderContext RC, int elementsCount, ref IVertexBuffer vbInstances)
		{
			if (elementsCount != 0 && (vbInstances == null || vbInstances.ElementCount < elementsCount))
			{
				if (vbInstances != null)
				{
					MyManagers.Buffers.Dispose(vbInstances);
				}
				vbInstances = MyManagers.Buffers.CreateVertexBuffer("MyDrawableGroupDepthStrategy.VbInstances", elementsCount * 2, m_customPass0.ElementSize, null, ResourceUsage.Dynamic);
			}
		}

		private void PrepareLodData(MyList<MyPreparedLod> preparedLodData, MyList<MyInstance> visibleInstances, int passId)
		{
<<<<<<< HEAD
			if (visibleInstances == null)
			{
				return;
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyManagers.GeometryRenderer.GetLoddingSetting(passId, out var settings);
			foreach (MyInstance visibleInstance in visibleInstances)
			{
				if (m_customPass1.IsInstanceVisible(visibleInstance))
				{
					MyLodStrategy lodStrategy = visibleInstance.LodStrategy;
					MyInstanceLodState stateId = lodStrategy.ExplicitLodState;
					float stateData = lodStrategy.ExplicitStateData;
					bool num = m_customPass1.IsTransitionLodUsed(visibleInstance);
					if (num)
					{
						stateId = MyInstanceLodState.Transition;
						stateData = 1f - lodStrategy.Transition;
					}
					int num2 = (visibleInstance.CheckGbufferVisible() ? settings.LodShiftVisible : settings.LodShift);
					int currentLod = lodStrategy.CurrentLod;
					MyPreparedLod item;
					if (currentLod != -1)
					{
						currentLod = Math.Min(val2: Math.Max(settings.MinLod, currentLod + num2), val1: lodStrategy.MaxLod);
						MyLod lod = visibleInstance.Model.GetLod(currentLod);
						MyLodInstance lodInstance = visibleInstance.ModelInstance.LodInstances[currentLod];
						item = new MyPreparedLod
						{
							Lod = lod,
							LodInstance = lodInstance,
							StateId = stateId,
							StateData = stateData,
							Instance = visibleInstance
						};
						preparedLodData.Add(item);
					}
					int transitionLod = lodStrategy.TransitionLod;
					if (num && transitionLod != -1)
					{
						transitionLod = Math.Max(settings.MinLod, transitionLod + num2);
						transitionLod = Math.Min(lodStrategy.MaxLod, transitionLod);
						MyLod lod2 = visibleInstance.Model.GetLod(transitionLod);
						MyLodInstance lodInstance2 = visibleInstance.ModelInstance.LodInstances[transitionLod];
						item = new MyPreparedLod
						{
							Lod = lod2,
							LodInstance = lodInstance2,
							StateId = MyInstanceLodState.Transition,
							StateData = 2f + lodStrategy.Transition,
							Instance = visibleInstance
						};
						preparedLodData.Add(item);
					}
				}
			}
		}

		private void ComputeCounts(out int out_sumInstanceMaterials, out int out_drawnLodsCount, MyList<MyPreparedLod> preparedLods)
		{
			out_drawnLodsCount = 0;
			out_sumInstanceMaterials = 0;
			m_tmpInstanceLodsCounts.PrepareByZeroes(m_maxLodId);
			foreach (MyPreparedLod preparedLod in preparedLods)
			{
				m_tmpInstanceLodsCounts.Inc(preparedLod.LodInstance.UniqueId, preparedLod.StateId);
				out_sumInstanceMaterials += m_customPass0.GetInstanceMaterialsCount(preparedLod.Lod);
				out_drawnLodsCount++;
			}
		}

		private void PrepareInstanceableGroups()
		{
			List<MyInstanceLodGroup> instanceLodGroups = m_outputRenderData.InstanceLodGroups;
			instanceLodGroups.Clear();
			m_tmpInstanceLodsOffsets.PrepareByNegativeOnes(m_maxLodId);
			int num = 0;
			int num2 = 0;
			foreach (MyPreparedLod preparedLodDatum in m_preparedLodData)
			{
				MyInstance instance = preparedLodDatum.Instance;
				MyLod lod = preparedLodDatum.Lod;
				MyLodInstance lodInstance = preparedLodDatum.LodInstance;
				MyInstanceLodState stateId = preparedLodDatum.StateId;
				float stateData = preparedLodDatum.StateData;
				int uniqueId = lodInstance.UniqueId;
				int directOffset = m_tmpInstanceLodsOffsets.GetDirectOffset(uniqueId, stateId);
				int num3 = m_tmpInstanceLodsOffsets.AtByDirectOffset(directOffset);
				MyInstanceLodGroup myInstanceLodGroup2;
				if (num3 == -1)
				{
					num3 = num;
					m_tmpInstanceLodsOffsets.SetAtDirectOffset(directOffset, num);
					int instanceMaterialsCount = m_customPass0.GetInstanceMaterialsCount(lod);
					int num4 = m_tmpInstanceLodsCounts.AtByDirectOffset(directOffset);
					MyInstanceLodGroup myInstanceLodGroup = default(MyInstanceLodGroup);
					myInstanceLodGroup.Lod = lod;
					myInstanceLodGroup.LodInstance = lodInstance;
					myInstanceLodGroup.State = stateId;
					myInstanceLodGroup.MetalnessColorable = preparedLodDatum.Instance.MetalnessColorable;
					myInstanceLodGroup.OffsetInInstanceBuffer = num2;
					myInstanceLodGroup.InstancesCount = num4;
					myInstanceLodGroup.InstancesIncrement = 0;
					myInstanceLodGroup.InstanceMaterialsCount = instanceMaterialsCount;
					myInstanceLodGroup2 = myInstanceLodGroup;
					instanceLodGroups.Add(myInstanceLodGroup2);
					num2 += num4 * (1 + instanceMaterialsCount);
					num++;
				}
				else
				{
					myInstanceLodGroup2 = instanceLodGroups[num3];
				}
				int num5 = myInstanceLodGroup2.OffsetInInstanceBuffer + myInstanceLodGroup2.InstancesIncrement;
				m_customPass0.AddInstanceIntoInstanceElements(num5, instance, -1, stateData);
				List<int> instanceMaterialOffsetsForThePass = m_customPass0.GetInstanceMaterialOffsetsForThePass(lod);
				for (int i = 0; i < myInstanceLodGroup2.InstanceMaterialsCount; i++)
				{
					num5 += myInstanceLodGroup2.InstancesCount;
					int instanceMaterialOffsetInData = instanceMaterialOffsetsForThePass[i];
					m_customPass0.AddInstanceIntoInstanceElements(num5, instance, instanceMaterialOffsetInData, stateData);
				}
				myInstanceLodGroup2.InstancesIncrement++;
				instanceLodGroups[num3] = myInstanceLodGroup2;
			}
		}

		private void Perform(MyRenderContext RC, MyList<MyInstance> visibleInstances)
		{
			m_maxLodId = MyManagers.IDGenerator.Lods.GetHighestID();
			PrepareLodData(m_preparedLodData, visibleInstances, PassId);
			ComputeCounts(out var out_sumInstanceMaterials, out var out_drawnLodsCount, m_preparedLodData);
			m_elementsCount = out_drawnLodsCount + out_sumInstanceMaterials;
			m_customPass0.InitInstanceElements(m_elementsCount);
			PrepareInstanceableGroups();
			InitInstanceBuffer(RC, m_elementsCount, ref m_outputRenderData.VBInstanceBuffer);
			if (MyRender11.ParallelVertexBufferMapping)
			{
				PerformCopy(RC);
			}
		}

		private void PerformCopy(MyRenderContext RC)
		{
			if (m_elementsCount != 0)
			{
				RC.SetEventMarker("Perform");
				MyMapping vb = MyMapping.MapDiscard(RC, m_outputRenderData.VBInstanceBuffer);
				m_customPass0.WriteData(ref vb);
				vb.Unmap();
			}
		}

		public void Init(int passId, MyList<MyInstance> visibleInstances, MyRenderData outputRenderData, string debugName, int frustumIndex)
		{
			m_debugName = debugName;
			m_frustumIndex = frustumIndex;
			PassId = passId;
			m_outputRenderData = outputRenderData;
			m_visibleInstances = visibleInstances;
		}

		public void DoWork()
		{
			Begin();
			Perform(m_RC, m_visibleInstances);
			End();
		}

		private void Begin()
		{
			m_started = Stopwatch.GetTimestamp();
			if (MyRender11.ParallelVertexBufferMapping)
			{
				m_RC = MyManagers.DeferredRCs.AcquireRC(m_debugName);
			}
			else
			{
				m_RC = null;
			}
		}

		private void End()
		{
			m_visibleInstances = null;
			if (MyRender11.ParallelVertexBufferMapping)
			{
				m_finishedContext = m_RC.FinishDeferredContext();
			}
			m_preparedLodData.SetSize(0);
			m_elapsed = Stopwatch.GetTimestamp() - m_started;
			m_RC = null;
		}

		public void PostprocessWork()
		{
			if (MyRender11.ParallelVertexBufferMapping)
			{
<<<<<<< HEAD
				MyGpuProfiler.IC_BeginBlock(m_debugName + m_frustumIndex, "PostprocessWork", "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage2\\PreparePass\\MyPreparePass.cs");
				MyRender11.RC.ExecuteContext(ref m_finishedContext, "PostprocessWork", 325, "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage2\\PreparePass\\MyPreparePass.cs");
				MyGpuProfiler.IC_EndBlock(m_elementsCount, "PostprocessWork", "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage2\\PreparePass\\MyPreparePass.cs");
=======
				MyGpuProfiler.IC_BeginBlock(m_debugName + m_frustumIndex, "PostprocessWork", "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage2\\PreparePass\\MyPreparePass.cs");
				MyRender11.RC.ExecuteContext(ref m_finishedContext, "PostprocessWork", 319, "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage2\\PreparePass\\MyPreparePass.cs");
				MyGpuProfiler.IC_EndBlock(m_elementsCount, "PostprocessWork", "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage2\\PreparePass\\MyPreparePass.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				PerformCopy(MyRender11.RC);
			}
		}

		/// <inheritdoc />
		void IPooledObject.Cleanup()
		{
		}
	}
}
