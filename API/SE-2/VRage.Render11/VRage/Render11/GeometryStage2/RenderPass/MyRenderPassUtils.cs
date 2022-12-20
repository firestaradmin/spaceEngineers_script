using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Scene.Components;
using VRageMath;
using VRageRender;

namespace VRage.Render11.GeometryStage2.RenderPass
{
	internal static class MyRenderPassUtils
	{
		public static void FillConstantBuffer<T>(MyRenderContext RC, IConstantBuffer cb, T data) where T : struct
		{
			MyMapping myMapping = MyMapping.MapDiscard(RC, cb);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
		}

		public unsafe static IConstantBuffer GetPlaceholderObjectCB(MyRenderContext RC, uint lod)
		{
			int num = sizeof(MyObjectDataCommon);
			num += sizeof(MyObjectDataNonVoxel);
			IConstantBuffer objectCB = RC.GetObjectCB(num);
			MyMapping myMapping = MyMapping.MapDiscard(RC, objectCB);
			MyObjectDataNonVoxel data = default(MyObjectDataNonVoxel);
			myMapping.WriteAndPosition(ref data);
			MyObjectDataCommon myObjectDataCommon = default(MyObjectDataCommon);
			myObjectDataCommon.LocalMatrix = Matrix.Identity;
			myObjectDataCommon.ColorMul = Vector3.One;
			myObjectDataCommon.KeyColor = new Vector3(0f, 0f, 0f);
			myObjectDataCommon.LOD = lod;
			MyObjectDataCommon data2 = myObjectDataCommon;
			myMapping.WriteAndPosition(ref data2);
			myMapping.Unmap();
			return objectCB;
		}

		public unsafe static IConstantBuffer GetTransparentMaterialCB(MyRenderContext RC, MyTransparentMaterial material, float radius)
		{
			MyTransparentModelConstants myTransparentModelConstants = default(MyTransparentModelConstants);
			myTransparentModelConstants.Color = material.Color * MyRender11.Settings.TransparentColorMultiplier;
			myTransparentModelConstants.ColorAdd = material.ColorAdd;
			myTransparentModelConstants.ShadowMultiplier = material.ShadowMultiplier;
			myTransparentModelConstants.LightMultiplier = material.LightMultiplier;
			myTransparentModelConstants.Reflectivity = material.Reflectivity * MyRender11.Settings.TransparentReflectivityMultiplier;
			myTransparentModelConstants.Fresnel = material.Fresnel * MyRender11.Settings.TransparentFresnelMultiplier;
			myTransparentModelConstants.ReflectionShadow = material.ReflectionShadow * MyRender11.Settings.TransparentGlossMultiplier;
			myTransparentModelConstants.Gloss = material.Gloss * MyRender11.Settings.TransparentGlossMultiplier;
			myTransparentModelConstants.GlossTextureAdd = material.GlossTextureAdd;
			myTransparentModelConstants.SpecularColorFactor = material.SpecularColorFactor;
			myTransparentModelConstants.Radius = radius;
			myTransparentModelConstants.SoftParticleDistanceScale = material.SoftParticleDistanceScale;
			MyTransparentModelConstants data = myTransparentModelConstants;
			IConstantBuffer materialCB = RC.GetMaterialCB(sizeof(MyTransparentModelConstants));
			MyMapping myMapping = MyMapping.MapDiscard(RC, materialCB);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			return materialCB;
		}

		public unsafe static IConstantBuffer GetPlaceholderTransparentMaterialCB(MyRenderContext RC)
		{
			MyTransparentModelConstants myTransparentModelConstants = default(MyTransparentModelConstants);
			myTransparentModelConstants.Color = Vector4.Zero;
			myTransparentModelConstants.Reflectivity = 0f;
			MyTransparentModelConstants data = myTransparentModelConstants;
			IConstantBuffer materialCB = RC.GetMaterialCB(sizeof(MyTransparentModelConstants));
			MyMapping myMapping = MyMapping.MapDiscard(RC, materialCB);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			return materialCB;
		}
	}
}
