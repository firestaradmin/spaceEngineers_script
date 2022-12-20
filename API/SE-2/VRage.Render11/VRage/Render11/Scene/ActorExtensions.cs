using VRage.Render.Scene;
using VRage.Render.Scene.Components;
using VRage.Render11.GeometryStage.Voxel;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.StaticGroup;
using VRage.Render11.Scene.Components;
using VRageRender;

namespace VRage.Render11.Scene
{
	internal static class ActorExtensions
	{
		public static MyRenderableComponent GetRenderable(this MyActor actor)
		{
			return actor.GetComponent<MyRenderableComponent>();
		}

		public static MyInstanceComponent GetInstance(this MyActor actor)
		{
			return actor.GetComponent<MyInstanceComponent>();
		}

		public static MyFoliageComponent GetFoliage(this MyActor actor)
		{
			return actor.GetComponent<MyFoliageComponent>();
		}

		public static MySkinningComponent GetSkinning(this MyActor actor)
		{
			return actor.GetComponent<MySkinningComponent>();
		}

		public static MyMergeGroupRootComponent GetMergeGroupRoot(this MyActor actor)
		{
			return actor.GetComponent<MyMergeGroupRootComponent>();
		}

		public static MyMergeGroupLeafComponent GetMergeGroupLeaf(this MyActor actor)
		{
			return actor.GetComponent<MyMergeGroupLeafComponent>();
		}

		public static VRage.Render11.Scene.Components.MyLightComponent GetLight(this MyActor actor)
		{
			return (VRage.Render11.Scene.Components.MyLightComponent)actor.GetComponent<VRage.Render.Scene.Components.MyLightComponent>();
		}

		public static MyStaticGroupComponent GetStaticGroup(this MyActor actor)
		{
			return actor.GetComponent<MyStaticGroupComponent>();
		}

		public static MyRenderVoxelActor GetVoxel(this MyActor actor)
		{
			return actor.GetComponent<MyRenderVoxelActor>();
		}

		public static MyVoxelCellComponent GetVoxelCell(this MyActor actor)
		{
			return actor.GetComponent<MyVoxelCellComponent>();
		}

		public static MyResourcePrioritizationComponent GetSceneResourcePrioritizationComponent(this MyActor actor, bool createIfNeeded = true)
		{
			MyResourcePrioritizationComponent myResourcePrioritizationComponent = actor.GetComponent<MyResourcePrioritizationComponent>();
			if (myResourcePrioritizationComponent == null && createIfNeeded)
			{
				myResourcePrioritizationComponent = MyComponentFactory<MyResourcePrioritizationComponent>.Create();
				actor.AddComponent<MyResourcePrioritizationComponent>(myResourcePrioritizationComponent);
			}
			return myResourcePrioritizationComponent;
		}
	}
}
