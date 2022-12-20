using System.Collections.Generic;
using ParallelTasks;
using VRage.Render.Scene;
using VRage.Render11.Culling.Frustum;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Culling
{
	internal class MyCullQuery
	{
		internal struct MyPointLightsInfo
		{
			public int GroupsIndex;

			public int Start;

			public int Count;

			public float CameraDistance;
		}

		internal int ViewBitmask;

		internal BoundingFrustumD Frustum;

		internal BoundingFrustumD FrustumFar;

		internal Vector3D ViewOrigin;

		internal Matrix ViewProjection;

		internal IDsvBindable DepthBufferRo;

		internal IRtvBindable Rtv;

		internal readonly MyCullResults Results = new MyCullResults();

		internal readonly List<MyManualCullTreeData> Groups = new List<MyManualCullTreeData>();

		internal readonly List<MyPointLightsInfo> PointLightsInfo = new List<MyPointLightsInfo>();

		internal int GroupsCount;

		internal int RootObjectsCount;

		internal int RootInstancesCount;

		internal MyCullingSmallObjects? SmallObjects;

		internal MyViewType ViewType;

		internal int ViewIndex;

		internal int ViewId;

		internal HashSet<uint> Ignored;

		internal MyRenderingPass RenderPass;

		internal MyFrustumCullingWork CullWork;

		internal DependencyResolver.JobToken Job;

		internal string DebugName;

		internal IRasterizerState Rasterizer;

		internal MyViewport Viewport;

		internal void Clear()
		{
			GroupsCount = 0;
			ViewBitmask = 0;
			RootObjectsCount = (RootInstancesCount = 0);
			Results.Clear();
			Groups.Clear();
			PointLightsInfo.Clear();
			SmallObjects = null;
			ViewType = MyViewType.Unassigned;
			ViewIndex = 0;
			ViewId = 0;
			Ignored = null;
		}
	}
}
