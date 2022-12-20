using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.Lodding;
using VRage.Render11.GeometryStage2.PrepareGroupPass;
using VRage.Render11.GeometryStage2.PreparePass;
using VRage.Render11.GeometryStage2.RenderPass;
using VRage.Render11.GeometryStage2.StaticGroup;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender;

namespace VRage.Render11.GeometryStage2.Rendering
{
	internal class MyGeometryRenderer : IManager, IManagerUnloadData
	{
		private readonly MyTransparentRenderPass m_transparentFlareOccludersRenderPass = new MyTransparentRenderPass();

		private readonly MyTransparentRenderPass m_transparentRenderPass = new MyTransparentRenderPass();

		private readonly MyTransparentForDecalsRenderPass m_transparentForDecalsRenderPass = new MyTransparentForDecalsRenderPass();

		private List<IPrepareWork> m_tmpPreparePasses;

		private List<MyRenderPass> m_tmpRenderPasses;

		private MyList<MyInstance>[] m_visibleInstances;

		private MyList<MyStaticGroup>[] m_visibleStaticGroups;

		private MyRenderData[] m_instanceRenderData;

		private MyList<MyRenderData>[] m_staticGroupsRenderData;

		public bool IsLodUpdateEnabled = true;

		private Vector3D m_prevCameraPositionOnUpdateLods;

		private MyGlobalLoddingSettings m_globalLoddingSettings = MyGlobalLoddingSettings.Default;

		private readonly MyPassLoddingSetting[] m_loddingSetting = new MyPassLoddingSetting[19];

		private static int GBufferPassId => 0;

		private static Matrix GBufferViewProjection => MyRender11.Environment.Matrices.ViewProjectionAt0;

		private static MyViewport GBufferViewport => new MyViewport(0f, 0f, MyRender11.ResolutionI.X, MyRender11.ResolutionI.Y);

		private MyRenderData TransparentRenderData { get; set; }

		private void AllocateInternal()
		{
			m_tmpPreparePasses = new List<IPrepareWork>();
			m_tmpRenderPasses = new List<MyRenderPass>();
			m_visibleStaticGroups = new MyList<MyStaticGroup>[19];
			for (int i = 0; i < m_visibleStaticGroups.Length; i++)
			{
				m_visibleStaticGroups[i] = new MyList<MyStaticGroup>();
			}
			m_visibleInstances = new MyList<MyInstance>[19];
			for (int j = 0; j < m_visibleInstances.Length; j++)
			{
				m_visibleInstances[j] = new MyList<MyInstance>();
			}
			m_instanceRenderData = new MyRenderData[19];
			for (int k = 0; k < m_instanceRenderData.Length; k++)
			{
				m_instanceRenderData[k] = new MyRenderData();
			}
			TransparentRenderData = new MyRenderData();
			m_staticGroupsRenderData = new MyList<MyRenderData>[19];
			for (int l = 0; l < m_staticGroupsRenderData.Length; l++)
			{
				m_staticGroupsRenderData[l] = new MyList<MyRenderData>();
			}
			MyObjectPoolManager.RegisterPool(typeof(MyPreparePass<MyColorPreparePass0, MyColorPreparePass1>));
			MyObjectPoolManager.RegisterPool(typeof(MyPreparePass<MyDepthPreparePass0, MyDepthPreparePass1>));
			MyObjectPoolManager.RegisterPool(typeof(MyPreparePass<MyColorPreparePass0, MyForwardPreparePass1>));
		}

		private void DeallocateInternal()
		{
			m_tmpPreparePasses.Clear();
			m_tmpRenderPasses.Clear();
			for (int i = 0; i < m_visibleStaticGroups.Length; i++)
			{
				m_visibleStaticGroups[i].Clear();
			}
			for (int j = 0; j < m_visibleInstances.Length; j++)
			{
				m_visibleInstances[j].Clear();
			}
			for (int k = 0; k < m_instanceRenderData.Length; k++)
			{
				m_instanceRenderData[k].Dispose();
			}
			TransparentRenderData.Dispose();
			for (int l = 0; l < m_staticGroupsRenderData.Length; l++)
			{
				m_staticGroupsRenderData[l].Clear();
			}
		}

		public MyGeometryRenderer()
		{
			AllocateInternal();
		}

		public void SetLoddingSetting(ref MyNewLoddingSettings settings)
		{
			m_globalLoddingSettings = settings.Global;
			for (int i = 0; i < 1; i++)
			{
				m_loddingSetting[MyViewIds.GetMainId(i)] = settings.GBuffer;
			}
			int num = Math.Min(settings.CascadeDepths.Length, 8);
			for (int j = 0; j < num; j++)
			{
				m_loddingSetting[MyViewIds.GetShadowCascadeId(j)] = settings.CascadeDepths[j];
			}
			for (int k = num; k < 8; k++)
			{
				m_loddingSetting[MyViewIds.GetShadowCascadeId(k)] = ((num == 0) ? MyPassLoddingSetting.Default : settings.CascadeDepths[num - 1]);
			}
			for (int l = 0; l < 4; l++)
			{
				m_loddingSetting[MyViewIds.GetShadowProjectionId(l)] = settings.SingleDepth;
			}
			for (int m = 0; m < 6; m++)
			{
				m_loddingSetting[MyViewIds.GetForwardId(m)] = settings.Forward;
			}
		}

		public void GetLoddingSetting(int viewId, out MyPassLoddingSetting settings)
		{
			settings = m_loddingSetting[viewId];
		}

		void IManagerUnloadData.OnUnloadData()
		{
			DeallocateInternal();
		}

		private void InitPasses(MyCullQueries cullQueries, IGBufferSrvStrategy srvStrategy, List<IPrepareWork> outPreparePasses, List<MyRenderPass> outRenderPasses)
		{
			MyManagers.IDGenerator.Lods.UpdateHighestID();
			outRenderPasses.Clear();
			if (!MyRender11.Settings.DrawDynamicInstances)
			{
				return;
			}
			for (int i = 0; i < cullQueries.Size; i++)
			{
				if (cullQueries.RenderingPasses[i] == null)
				{
					continue;
				}
				MyRenderingPass myRenderingPass = cullQueries.RenderingPasses[i];
				MyCullQuery myCullQuery = cullQueries.CullQueries[i];
				if (myRenderingPass is MyGBufferPass)
				{
					int gBufferPassId = GBufferPassId;
					MyRenderData myRenderData = m_instanceRenderData[gBufferPassId];
					myRenderData.Init(GBufferViewProjection, MyRender11.Environment.Matrices.Projection);
					MyViewport gBufferViewport = GBufferViewport;
					m_visibleInstances[gBufferPassId] = myCullQuery.Results.Instances;
					MyPreparePass<MyColorPreparePass0, MyColorPreparePass1> myPreparePass = MyObjectPoolManager.Allocate<MyPreparePass<MyColorPreparePass0, MyColorPreparePass1>>();
					myPreparePass.Init(gBufferPassId, m_visibleInstances[gBufferPassId], myRenderData, myRenderingPass.DebugName, myCullQuery.ViewIndex);
					outPreparePasses.Add(myPreparePass);
					m_visibleStaticGroups[gBufferPassId] = myCullQuery.Results.StaticGroups;
					if (myCullQuery.Results.StaticGroups.Count > 0)
					{
						MyPrepareGroupPass myPrepareGroupPass = MyObjectPoolManager.Allocate<MyPrepareGroupPass>();
						myPrepareGroupPass.Init(m_visibleStaticGroups[gBufferPassId], m_staticGroupsRenderData[gBufferPassId], myRenderingPass.ViewProjection, myRenderingPass.Projection, gBufferPassId);
						outPreparePasses.Add(myPrepareGroupPass);
					}
					MyGBuffer gBuffer = ((MyGBufferPass)myRenderingPass).GBuffer;
					MyGBufferRenderPass myGBufferRenderPass = MyObjectPoolManager.Allocate<MyGBufferRenderPass>();
					myGBufferRenderPass.Init(isUsedDeferredRC: true, gBufferPassId, gBufferViewport, gBuffer, srvStrategy, myRenderingPass.DebugName, myRenderData, m_staticGroupsRenderData[gBufferPassId]);
					outRenderPasses.Add(myGBufferRenderPass);
				}
				else if (myRenderingPass is MyDepthPass)
				{
					MyDepthPass obj = (MyDepthPass)myRenderingPass;
					int viewId = myRenderingPass.ViewId;
					MyRenderData myRenderData2 = m_instanceRenderData[viewId];
					myRenderData2.Init(myRenderingPass.ViewProjection, myRenderingPass.Projection);
					m_visibleInstances[viewId] = myCullQuery.Results.Instances;
					MyPreparePass<MyDepthPreparePass0, MyDepthPreparePass1> myPreparePass2 = MyObjectPoolManager.Allocate<MyPreparePass<MyDepthPreparePass0, MyDepthPreparePass1>>();
					myPreparePass2.Init(viewId, m_visibleInstances[viewId], myRenderData2, myRenderingPass.DebugName, myCullQuery.ViewIndex);
					outPreparePasses.Add(myPreparePass2);
					m_visibleStaticGroups[viewId] = myCullQuery.Results.StaticGroups;
					if (myCullQuery.Results.StaticGroups.Count > 0)
					{
						MyPrepareGroupPass myPrepareGroupPass2 = MyObjectPoolManager.Allocate<MyPrepareGroupPass>();
						myPrepareGroupPass2.Init(m_visibleStaticGroups[viewId], m_staticGroupsRenderData[viewId], myRenderingPass.ViewProjection, myRenderingPass.Projection, viewId);
						outPreparePasses.Add(myPrepareGroupPass2);
					}
					IDsvBindable dsv = obj.Dsv;
					bool isCascade = obj.IsCascade;
					MyDepthRenderPass myDepthRenderPass = MyObjectPoolManager.Allocate<MyDepthRenderPass>();
					myDepthRenderPass.Init(isUsedDeferredRC: true, viewId, myRenderingPass.Viewport, dsv, isCascade, myRenderingPass.DebugName, myRenderData2, m_staticGroupsRenderData[viewId], myCullQuery);
					outRenderPasses.Add(myDepthRenderPass);
				}
				else if (myRenderingPass is MyForwardPass && MyRenderProxy.Settings.RenderBlocksToEnvProbe)
				{
					MyForwardPass myForwardPass = (MyForwardPass)myRenderingPass;
					int viewId2 = myRenderingPass.ViewId;
					MyRenderData myRenderData3 = m_instanceRenderData[viewId2];
					myRenderData3.Init(myRenderingPass.ViewProjection, myRenderingPass.Projection);
					m_visibleInstances[viewId2] = myCullQuery.Results.Instances;
					MyPreparePass<MyColorPreparePass0, MyForwardPreparePass1> myPreparePass3 = MyObjectPoolManager.Allocate<MyPreparePass<MyColorPreparePass0, MyForwardPreparePass1>>();
					myPreparePass3.Init(viewId2, m_visibleInstances[viewId2], myRenderData3, myRenderingPass.DebugName, myCullQuery.ViewIndex);
					outPreparePasses.Add(myPreparePass3);
					m_visibleStaticGroups[viewId2] = myCullQuery.Results.StaticGroups;
					if (myCullQuery.Results.StaticGroups.Count > 0)
					{
						MyPrepareGroupPass myPrepareGroupPass3 = MyObjectPoolManager.Allocate<MyPrepareGroupPass>();
						myPrepareGroupPass3.Init(m_visibleStaticGroups[viewId2], m_staticGroupsRenderData[viewId2], myRenderingPass.ViewProjection, myRenderingPass.Projection, viewId2);
						outPreparePasses.Add(myPrepareGroupPass3);
					}
					MyForwardRenderPass myForwardRenderPass = MyObjectPoolManager.Allocate<MyForwardRenderPass>();
					myForwardRenderPass.Init(isUsedDeferredRC: true, viewId2, myRenderingPass.Viewport, myForwardPass.Rtvs, myForwardPass.Dsv, myForwardPass.DepthSrv, srvStrategy, myRenderingPass.DebugName, myRenderData3, m_staticGroupsRenderData[viewId2]);
					outRenderPasses.Add(myForwardRenderPass);
				}
			}
		}

		public int Preprocess(MyCullQuery cullQuery)
		{
			MyManagers.GeometryRenderer.UpdateMatrices(cullQuery);
			MyManagers.GeometryRenderer.UpdateLods(cullQuery);
			MyManagers.GeometryRenderer.Prepare(cullQuery);
			return cullQuery.Results.Instances.Count;
		}

		public void UpdateMatrices(MyCullQuery cullQuery)
		{
			MyList<MyInstance> instances = cullQuery.Results.Instances;
			Vector3D camPosition = MyRender11.Environment.Matrices.CameraPosition;
			foreach (MyInstance item in instances)
			{
				item.UpdateWorldMatrix(ref camPosition);
			}
		}

		public void UpdateLods(MyCullQuery cullQuery)
		{
			MyList<MyInstance> instances = cullQuery.Results.Instances;
			MyList<MyStaticGroup> staticGroups = cullQuery.Results.StaticGroups;
			if (!IsLodUpdateEnabled || MyManagers.Ansel.IsCaptureRunning)
			{
				return;
			}
			MyLodStrategyPreprocessor preprocessor = MyLodStrategyPreprocessor.Perform();
			Vector3D vector3D = MyRender11.Environment.Matrices.CameraPosition - m_prevCameraPositionOnUpdateLods;
			m_prevCameraPositionOnUpdateLods = MyRender11.Environment.Matrices.CameraPosition;
			bool flag = vector3D.Length() > m_globalLoddingSettings.MaxDistanceForSmoothCameraMovement;
			if (m_globalLoddingSettings.EnableLodSelection)
			{
				foreach (MyInstance item in instances)
				{
					item.LodStrategy.UpdateExplicitly(m_globalLoddingSettings.LodSelection);
				}
				foreach (MyStaticGroup item2 in staticGroups)
				{
					item2.LodStrategy.UpdateExplicitly(m_globalLoddingSettings.LodSelection);
				}
				return;
			}
			if (flag)
			{
				foreach (MyInstance item3 in instances)
				{
					item3.LodStrategy.UpdateWithoutTransition(ref item3.TransformStrategy.Translation, item3.Owner.Owner.WorldMatrixForwardScale, preprocessor);
				}
				foreach (MyStaticGroup item4 in staticGroups)
				{
					Vector3 cameraTranslation = item4.CameraTranslation;
					item4.LodStrategy.UpdateWithoutTransition(ref cameraTranslation, 1f, preprocessor);
				}
				return;
			}
			foreach (MyInstance item5 in instances)
			{
				item5.LodStrategy.UpdateSmoothly(ref item5.TransformStrategy.Translation, item5.Owner.Owner.WorldMatrixForwardScale, preprocessor);
			}
			foreach (MyStaticGroup item6 in staticGroups)
			{
				Vector3 cameraTranslation2 = item6.CameraTranslation;
				item6.LodStrategy.UpdateSmoothly(ref cameraTranslation2, 1f, preprocessor);
			}
		}

		internal void Prepare(MyCullQuery query)
		{
			foreach (IPrepareWork tmpPreparePass in m_tmpPreparePasses)
			{
				if (tmpPreparePass.PassId == query.ViewId)
				{
					tmpPreparePass.DoWork();
					break;
				}
			}
		}

		internal void DoneFrame()
		{
			foreach (IPrepareWork tmpPreparePass in m_tmpPreparePasses)
			{
				tmpPreparePass.PostprocessWork();
			}
			foreach (MyRenderPass tmpRenderPass in m_tmpRenderPasses)
			{
				tmpRenderPass.PostprocessWork();
			}
			ClearInternal(m_tmpPreparePasses, m_tmpRenderPasses);
		}

		internal int Render(MyCullQuery query)
		{
			foreach (MyRenderPass tmpRenderPass in m_tmpRenderPasses)
			{
				if (tmpRenderPass.ViewId == query.ViewId)
				{
					tmpRenderPass.DoWork();
				}
			}
			return query.Results.Instances.Count;
		}

		private void ClearInternal(List<IPrepareWork> prepareWorks, List<MyRenderPass> renderPasses)
		{
			foreach (IPrepareWork prepareWork in prepareWorks)
			{
				MyObjectPoolManager.Deallocate(prepareWork);
			}
			prepareWorks.Clear();
			foreach (MyRenderPass renderPass in renderPasses)
			{
				MyObjectPoolManager.Deallocate(renderPass);
			}
			renderPasses.Clear();
		}

		public void InitFrame(MyCullQueries cullQueries)
		{
			IGBufferSrvStrategy strategy = MyGBufferSrvStrategyFactory.GetStrategy();
			InitPasses(cullQueries, strategy, m_tmpPreparePasses, m_tmpRenderPasses);
		}

		public int PreprocessTransparentPass(MyCullQuery cullQuery)
		{
			TransparentRenderData.InstanceLodGroups.Clear();
			MyRenderData myRenderData = m_instanceRenderData[GBufferPassId];
			foreach (MyInstanceLodGroup instanceLodGroup in myRenderData.InstanceLodGroups)
			{
				if (instanceLodGroup.Lod.PreprocessedParts.TransparentParts != null)
				{
					TransparentRenderData.InstanceLodGroups.Add(instanceLodGroup);
				}
			}
			TransparentRenderData.VBInstanceBuffer = myRenderData.VBInstanceBuffer;
			TransparentRenderData.ViewProjMatrix = myRenderData.ViewProjMatrix;
			return cullQuery.Results.Instances.Count;
		}

		public bool HasTransparentInstances()
		{
			return TransparentRenderData.InstanceLodGroups.Count > 0;
		}

		public void RenderTransparentFlareOccluders(MyRenderContext rc)
		{
			m_transparentFlareOccludersRenderPass.Init(rc != null, GBufferPassId, GBufferViewport, TransparentRenderData, flareOccludersOnly: true, MyGBuffer.Main.LBuffer, MyGBuffer.Main.ResolvedDepthStencil.SrvDepth, rc);
			m_transparentFlareOccludersRenderPass.DoWork();
			m_transparentFlareOccludersRenderPass.PostprocessWork();
		}

		public void RenderTransparent(MyRenderContext rc)
		{
			m_transparentRenderPass.Init(rc != null, GBufferPassId, GBufferViewport, TransparentRenderData, flareOccludersOnly: false, MyGBuffer.Main.LBuffer, MyGBuffer.Main.ResolvedDepthStencil.SrvDepth, rc);
			m_transparentRenderPass.DoWork();
			m_transparentRenderPass.PostprocessWork();
		}

		public void RenderTransparentForDecals(MyRenderContext rc, IDepthStencil outputDs, IDepthStencil inputFilterDepth, IRtvTexture gbuffer1)
		{
			m_transparentForDecalsRenderPass.Init(rc != null, GBufferPassId, GBufferViewport, outputDs, inputFilterDepth, gbuffer1, TransparentRenderData, rc);
			m_transparentForDecalsRenderPass.DoWork();
			m_transparentForDecalsRenderPass.PostprocessWork();
		}
	}
}
