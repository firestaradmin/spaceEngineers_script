using System.Collections.Generic;
using VRage.Render.Scene;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.Scene;
using VRage.Render11.Scene.Components;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.GeometryStage2.Common
{
	internal static class MyComponentConverter
	{
		public static void ConvertActorToTheOldPipeline(MyActor actor)
		{
			if (actor.GetInstance() == null)
			{
				return;
			}
			MyInstanceComponent instance = actor.GetInstance();
			string mwmFilepath = instance.CompatibilityDataForTheOldPipeline.MwmFilepath;
			float rescale = instance.CompatibilityDataForTheOldPipeline.Rescale;
			RenderFlags renderFlags = instance.CompatibilityDataForTheOldPipeline.RenderFlags;
			int depthBias = instance.CompatibilityDataForTheOldPipeline.DepthBias;
			float dithering = instance.CompatibilityDataForTheOldPipeline.Dithering;
			Vector4 vector = instance.KeyColor.ToVector4();
			Dictionary<string, MyInstanceMaterial> instanceMaterials = instance.GetInstanceMaterials();
			Dictionary<string, MyTextureChange> textureChanges = instance.GetTextureChanges();
			actor.RemoveComponent<MyInstanceComponent>(instance);
			MyRenderableComponent myRenderableComponent = MyComponentFactory<MyRenderableComponent>.Create();
			actor.AddComponent<MyRenderableComponent>(myRenderableComponent);
			MeshId meshId = MyMeshes.GetMeshId(X.TEXT_(MyAssetsLoader.ModelRemap.Get(mwmFilepath, mwmFilepath)), rescale, MyRender11.DeferStateChangeBatch);
			if (meshId != MeshId.NULL)
			{
				myRenderableComponent.SetModel(meshId);
			}
			MyRenderableComponent renderable = actor.GetRenderable();
			renderable.AdditionalFlags |= MyProxiesFactory.GetRenderableProxyFlags(renderFlags);
			renderable.DepthBias = depthBias;
			renderable.SetKeyColor(new Vector3(vector.X, vector.Y, vector.Z));
			renderable.SetDithering(dithering);
			foreach (KeyValuePair<string, MyInstanceMaterial> item in instanceMaterials)
			{
				renderable.SetModelProperties(new MyEntityMaterialKey(item.Key), item.Value.Emissivity, item.Value.ColorMult);
			}
			if (textureChanges != null)
			{
				renderable.AddTextureChanges(textureChanges);
			}
		}
	}
}
