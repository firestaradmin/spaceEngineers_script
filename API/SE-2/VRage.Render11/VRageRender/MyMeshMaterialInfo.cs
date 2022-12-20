using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace VRageRender
{
	internal struct MyMeshMaterialInfo
	{
		internal MyMeshMaterialId Id;

		internal int RepresentationKey;

		internal MyStringId Name;

		internal MyFileTextureEnum TextureTypes;

		internal string ColorMetal_Texture;

		internal string NormalGloss_Texture;

		internal string Extensions_Texture;

		internal string Alphamask_Texture;

		public MyGeometryTextureSystemReference GeometryTextureRef;

		internal MyMeshDrawTechnique Technique;

		internal MyFacingEnum Facing;

		internal Vector2 WindScaleAndFreq;

		/// <summary>
		/// Register used textures and preload if requested
		/// </summary>
		internal static void RequestResources(ref MyMeshMaterialInfo info, bool preloadTextures)
		{
			if (!info.GeometryTextureRef.IsUsed)
			{
				MyTextureStreamingManager textures = MyManagers.Textures;
				IMyStreamedTexture texture = textures.GetTexture(info.ColorMetal_Texture, new MyTextureStreamingManager.QueryArgs
				{
					SkipQualityReduction = (info.Facing == MyFacingEnum.Impostor),
					TextureType = MyFileTextureEnum.COLOR_METAL
				});
				if (preloadTextures)
				{
					IMyStreamedTexture texture2 = textures.GetTexture(info.NormalGloss_Texture, MyFileTextureEnum.NORMALMAP_GLOSS);
					IMyStreamedTexture texture3 = textures.GetTexture(info.Extensions_Texture, MyFileTextureEnum.EXTENSIONS);
					IMyStreamedTexture texture4 = textures.GetTexture(info.Alphamask_Texture, MyFileTextureEnum.ALPHAMASK);
					texture?.Touch(100);
					texture2?.Touch(100);
					texture3?.Touch(100);
					texture4?.Touch(100);
				}
			}
			else
			{
				info.GeometryTextureRef.ColorMetalTexture?.Update(isDeviceInit: false);
				info.GeometryTextureRef.NormalGlossTexture?.Update(isDeviceInit: false);
				info.GeometryTextureRef.ExtensionTexture?.Update(isDeviceInit: false);
				info.GeometryTextureRef.AlphamaskTexture?.Update(isDeviceInit: false);
			}
		}

		internal static MyMaterialProxy_2 CreateProxy(ref MyMeshMaterialInfo info)
		{
			IMyStreamedTexture myStreamedTexture = null;
			IMyStreamedTexture myStreamedTexture2 = null;
			IMyStreamedTexture myStreamedTexture3 = null;
			IMyStreamedTexture myStreamedTexture4 = null;
			ISrvBindable srvBindable;
			ISrvBindable srvBindable2;
			ISrvBindable srvBindable3;
			ISrvBindable srvBindable4;
			if (!info.GeometryTextureRef.IsUsed)
			{
				MyTextureStreamingManager textures = MyManagers.Textures;
				myStreamedTexture = textures.GetTexture(info.ColorMetal_Texture, MyFileTextureEnum.COLOR_METAL);
				myStreamedTexture2 = textures.GetTexture(info.NormalGloss_Texture, MyFileTextureEnum.NORMALMAP_GLOSS);
				myStreamedTexture3 = textures.GetTexture(info.Extensions_Texture, MyFileTextureEnum.EXTENSIONS);
				myStreamedTexture4 = textures.GetTexture(info.Alphamask_Texture, MyFileTextureEnum.ALPHAMASK);
				srvBindable = myStreamedTexture.Texture;
				srvBindable2 = myStreamedTexture2.Texture;
				srvBindable3 = myStreamedTexture3.Texture;
				srvBindable4 = myStreamedTexture4.Texture;
			}
			else
			{
				srvBindable = info.GeometryTextureRef.ColorMetalTexture;
				srvBindable2 = info.GeometryTextureRef.NormalGlossTexture;
				srvBindable3 = info.GeometryTextureRef.ExtensionTexture;
				srvBindable4 = info.GeometryTextureRef.AlphamaskTexture;
			}
			MySrvTable mySrvTable = default(MySrvTable);
			mySrvTable.BindFlag = MyBindFlag.BIND_PS;
			mySrvTable.StartSlot = 0;
			mySrvTable.Srvs = new ISrvBindable[4] { srvBindable, srvBindable2, srvBindable3, srvBindable4 };
			mySrvTable.TextureHandles = new IMyStreamedTexture[4] { myStreamedTexture, myStreamedTexture2, myStreamedTexture3, myStreamedTexture4 };
			mySrvTable.Version = info.Id.GetHashCode();
			MySrvTable materialSrvs = mySrvTable;
			MyMaterialProxy_2 result = default(MyMaterialProxy_2);
			result.MaterialSrvs = materialSrvs;
			return result;
		}
	}
}
