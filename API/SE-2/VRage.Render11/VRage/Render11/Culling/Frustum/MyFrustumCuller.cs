using VRage.Render11.Tools;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Culling.Frustum
{
	internal static class MyFrustumCuller
	{
		/// <summary>
		/// Goes through all renderables and adds the ones that are in the given frustums to the lists in frustumCullQuery
		/// </summary>
		internal static void Init(MyCullQueries cullQueries, MyDynamicAABBTreeD renderables, MyDynamicAABBTreeD renderablesFar, MyDynamicAABBTreeD groups)
		{
			for (int i = 0; i < cullQueries.Size; i++)
			{
				MyFrustumCullingWork myFrustumCullingWork = MyObjectPoolManager.Allocate<MyFrustumCullingWork>();
				myFrustumCullingWork.Init(cullQueries.CullQueries[i], renderables, renderablesFar, groups);
				cullQueries.CullQueries[i].CullWork = myFrustumCullingWork;
			}
		}

		internal static void Done(MyCullQueries cullQueries)
		{
			for (int i = 0; i < cullQueries.Size; i++)
			{
				MyCullQuery myCullQuery = cullQueries.CullQueries[i];
				foreach (MyCullProxy cullProxy in myCullQuery.Results.CullProxies)
				{
					cullProxy.Updated = 0;
				}
				MyObjectPoolManager.Deallocate(myCullQuery.CullWork);
				MyRendererStats.MyCullStats myCullStats = default(MyRendererStats.MyCullStats);
				myCullStats.CullProxies = myCullQuery.Results.CullProxies.Count;
				myCullStats.RootObjects = myCullQuery.RootObjectsCount;
				myCullStats.RootInstances = myCullQuery.RootInstancesCount;
				myCullStats.Instances = myCullQuery.Results.Instances.Count;
				myCullStats.Groups = myCullQuery.GroupsCount;
				myCullStats.PointLights = myCullQuery.Results.PointLights.Count;
				myCullStats.SpotLights = myCullQuery.Results.SpotLights.Count;
				myCullStats.Foliage = myCullQuery.Results.Foliage.Count;
				MyRendererStats.MyCullStats myCullStats2 = myCullStats;
				MyRendererStats.ViewCullStats[myCullQuery.ViewId] = myCullStats2;
			}
		}
	}
}
