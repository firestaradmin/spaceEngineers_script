using VRage.Generics;
using VRage.Render.Scene;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Culling
{
	internal class MyCullQueries
	{
		private const int MAX_CULL_QUERY_COUNT = 32;

		internal readonly MyCullQuery[] CullQueries = new MyCullQuery[32];

		internal readonly MyRenderingPass[] RenderingPasses = new MyRenderingPass[32];

		private static readonly MyDynamicObjectPool<BoundingFrustumD> m_frustumPool = new MyDynamicObjectPool<BoundingFrustumD>(8);

		internal int Size { get; private set; }

		internal MyCullQueries()
		{
			for (int i = 0; i < 32; i++)
			{
				CullQueries[i] = new MyCullQuery();
			}
		}

		internal void Reset()
		{
			for (int i = 0; i < Size; i++)
			{
				if (CullQueries[i].ViewType != MyViewType.Main)
				{
					DeallocateFrustum(CullQueries[i].Frustum);
				}
				CullQueries[i].Clear();
				if (RenderingPasses[i] != null)
				{
					MyObjectPoolManager.Deallocate(RenderingPasses[i]);
					RenderingPasses[i] = null;
				}
			}
			Size = 0;
		}

		internal void AddMainViewPass(ref MyViewport viewport, MyGBuffer gbuffer)
		{
			MyCullQuery query = AddView(MyViewType.Main, 0, MyRender11.Environment.Matrices.ViewFrustumClippedD, MyRender11.Environment.Matrices.ViewFrustumClippedFarD, ref MyRender11.Environment.Matrices.ViewProjectionAt0, ref MyRender11.Environment.Matrices.CameraPosition, MyGBuffer.Main.ResolvedDepthStencil.DsvRo, MyGBuffer.Main.LBuffer);
			AddRenderPass<MyGBufferPass>(query, ref viewport, ref MyRender11.Environment.Matrices.Projection).GBuffer = gbuffer;
		}

		internal void AddForwardPass(int index, ref Matrix offsetedViewProjection, ref MatrixD viewProjection, ref Vector3D viewOrigin, ref Matrix projection, ref MyViewport viewport, IDsvBindable dsv, ISrvBindable srvDepth, IRtvBindable rtv0, IRtvBindable rtv1)
		{
			BoundingFrustumD boundingFrustumD = AllocateFrustum();
			boundingFrustumD.Matrix = viewProjection;
			MyCullQuery query = AddView(MyViewType.EnvironmentProbe, index, boundingFrustumD, boundingFrustumD, ref offsetedViewProjection, ref viewOrigin, dsv, null);
			MyForwardPass myForwardPass = AddRenderPass<MyForwardPass>(query, ref viewport, ref projection);
			myForwardPass.Dsv = dsv;
			myForwardPass.DepthSrv = srvDepth;
			myForwardPass.Rtvs[0] = rtv0.Rtv;
			myForwardPass.Rtvs[1] = rtv1.Rtv;
		}

		internal void AddDepthPass(MyShadowmapQuery shadowmapQuery)
		{
			BoundingFrustumD boundingFrustumD = AllocateFrustum();
			boundingFrustumD.Matrix = shadowmapQuery.ProjectionInfo.WorldToProjection;
			MatrixD m = shadowmapQuery.ProjectionInfo.CurrentLocalToProjection;
			Matrix viewProj = m;
			MyCullQuery myCullQuery = AddView(shadowmapQuery.ViewType, shadowmapQuery.ViewIndex, boundingFrustumD, boundingFrustumD, ref viewProj, ref shadowmapQuery.ProjectionInfo.ViewOrigin, shadowmapQuery.DepthBufferRo, null);
			if (shadowmapQuery.ViewType == MyViewType.ShadowCascade)
			{
				MyCullingSmallObjects myCullingSmallObjects = default(MyCullingSmallObjects);
				myCullingSmallObjects.ProjectionFactor = shadowmapQuery.ProjectionFactor;
				myCullingSmallObjects.SkipThreshold = MyShadowCascades.Settings.Cascades[shadowmapQuery.ViewIndex].SkippingSmallObjectThreshold;
				MyCullingSmallObjects value = myCullingSmallObjects;
				myCullQuery.SmallObjects = value;
			}
			myCullQuery.Ignored = shadowmapQuery.IgnoredEntities;
			bool flag = shadowmapQuery.ViewType == MyViewType.ShadowCascade;
			myCullQuery.Rasterizer = (flag ? MyRasterizerStateManager.CascadesRasterizerState : MyRasterizerStateManager.ShadowRasterizerState);
			MyDepthPass myDepthPass = AddRenderPass<MyDepthPass>(myCullQuery, ref shadowmapQuery.Viewport, ref shadowmapQuery.ProjectionInfo.Projection);
			myDepthPass.Dsv = shadowmapQuery.DepthBuffer;
			myDepthPass.IsCascade = flag;
			myDepthPass.DefaultRasterizer = myCullQuery.Rasterizer;
		}

		private BoundingFrustumD AllocateFrustum()
		{
			return m_frustumPool.Allocate();
		}

		private void DeallocateFrustum(BoundingFrustumD frustum)
		{
			m_frustumPool.Deallocate(frustum);
		}

		private MyCullQuery AddView(MyViewType viewType, int viewIndex, BoundingFrustumD frustum, BoundingFrustumD frustumFar, ref Matrix viewProj, ref Vector3D viewOrigin, IDsvBindable depthBufferRo, IRtvBindable rtv)
		{
			MyCullQuery myCullQuery = CullQueries[Size];
			myCullQuery.Clear();
			myCullQuery.Frustum = frustum;
			myCullQuery.FrustumFar = frustumFar;
			myCullQuery.ViewProjection = viewProj;
			myCullQuery.ViewOrigin = viewOrigin;
			myCullQuery.DepthBufferRo = depthBufferRo;
			myCullQuery.Rtv = rtv;
			myCullQuery.ViewType = viewType;
			myCullQuery.ViewIndex = viewIndex;
			myCullQuery.ViewId = MyViewIds.GetId(viewType, viewIndex);
			myCullQuery.DebugName = MyViewIds.ViewNames[myCullQuery.ViewId];
			myCullQuery.ViewBitmask = 1 << myCullQuery.ViewId;
			Size++;
			return myCullQuery;
		}

		private T AddRenderPass<T>(MyCullQuery query, ref MyViewport viewport, ref Matrix proj) where T : MyRenderingPass
		{
			T val = (T)(query.RenderPass = MyObjectPoolManager.Allocate<T>());
			query.Viewport = viewport;
			val.DebugName = query.DebugName;
			val.ProcessingMask = query.ViewBitmask;
			val.ViewProjection = query.ViewProjection;
			val.Projection = proj;
			val.Viewport = viewport;
			val.ViewId = query.ViewId;
			val.SetImmediate(isImmediate: false);
			RenderingPasses[Size - 1] = val;
			return val;
		}
	}
}
