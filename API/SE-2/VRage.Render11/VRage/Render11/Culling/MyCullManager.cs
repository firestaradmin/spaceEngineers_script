using System.Collections.Generic;
using VRage.Collections;
using VRage.Render.Scene;
using VRage.Render11.Ansel;
using VRage.Render11.Common;
using VRage.Render11.Culling.Frustum;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Scene;
using VRageRender;

namespace VRage.Render11.Culling
{
	internal class MyCullManager : IManager
	{
		private readonly MyShadows m_shadowHandler;

		private readonly MyCullQueries m_cullQueries;

		internal MyCullManager(MyShadows shadowHandler)
		{
			m_cullQueries = new MyCullQueries();
			m_shadowHandler = shadowHandler;
		}

		public void OnFrameEnd()
		{
			m_cullQueries.Reset();
		}

		internal void InitFrame(MyRenderContext rc)
		{
			ListReader<MyShadowmapQuery> listReader = ((!MyManagers.Ansel.IsMultiresCapturing) ? m_shadowHandler.PrepareQueries(rc) : ((ListReader<MyShadowmapQuery>)MyAnselRenderUtils.EmptyShadowmapQueries));
			MyViewport viewport = new MyViewport(MyRender11.ViewportResolution);
			m_cullQueries.AddMainViewPass(ref viewport, MyGBuffer.Main);
			foreach (MyShadowmapQuery item in listReader)
			{
				m_cullQueries.AddDepthPass(item);
			}
			MyManagers.EnvironmentProbe.UpdateCullQuery(rc, m_cullQueries);
			MyFrustumCuller.Init(m_cullQueries, MyScene11.DynamicRenderablesDBVH, MyScene11.DynamicRenderablesFarDBVH, MyScene11.ManualCullTree);
		}

		internal void DoneFrame()
		{
			MyFrustumCuller.Done(m_cullQueries);
		}

		internal int SendGlobalOutputMessages(MyCullQuery mainQuery)
		{
			HashSet<uint> visibleObjectsWrite = MyRenderProxy.VisibleObjectsWrite;
			int num = 0;
			int num2 = 0;
			foreach (MyManualCullTreeData group in mainQuery.Groups)
			{
				if (group.Actor.VisibilityUpdates)
				{
					visibleObjectsWrite.Add(group.Actor.ID);
					num++;
				}
			}
			num2 += num;
			num = 0;
			foreach (MyCullProxy cullProxy in mainQuery.Results.CullProxies)
			{
				if (cullProxy.Parent.Owner.VisibilityUpdates)
				{
					visibleObjectsWrite.Add(cullProxy.OwnerID);
					num++;
				}
			}
			num2 += num;
			num = 0;
			foreach (MyInstance instance in mainQuery.Results.Instances)
			{
				if (instance.Owner.Owner.VisibilityUpdates)
				{
					visibleObjectsWrite.Add(instance.ActorID);
					num++;
				}
			}
			return num2 + num;
		}

		public MyCullQuery GetGBufferCullQuery()
		{
			for (int i = 0; i < m_cullQueries.Size; i++)
			{
				if (m_cullQueries.CullQueries[i].ViewType == MyViewType.Main)
				{
					return m_cullQueries.CullQueries[i];
				}
			}
			return null;
		}

		public static bool SupportsOcclusion(MyViewType viewType)
		{
			if (viewType != MyViewType.EnvironmentProbe)
			{
				return viewType != MyViewType.ShadowProjection;
			}
			return false;
		}

		public void OcclusionCull(MyRenderContext rc)
		{
			for (int i = 0; i < m_cullQueries.Size; i++)
			{
				MyCullQuery myCullQuery = m_cullQueries.CullQueries[i];
				if (SupportsOcclusion(myCullQuery.ViewType))
				{
					MyManagers.ActorOcclusionRenderer.Render(rc, myCullQuery);
				}
			}
		}

		public MyCullQueries GetCullQueries()
		{
			return m_cullQueries;
		}
	}
}
