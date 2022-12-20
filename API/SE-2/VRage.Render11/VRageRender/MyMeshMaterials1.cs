using System;
using System.Collections.Generic;
using SharpDX.Direct3D;
using VRage.Render11.Resources;
using VRage.Utils;
using VRageRender.Import;

namespace VRageRender
{
	internal class MyMeshMaterials1
	{
		private static MyFreelist<MyMeshMaterialInfo> MaterialsPool;

		private static Dictionary<MyMeshMaterialId, MyMaterialProxyId> MaterialProxyIndex;

		internal static Dictionary<int, MyMeshMaterialId> MaterialRkIndex;

		private static Dictionary<MyStringId, MyMeshMaterialId> MaterialNameIndex;

		internal static HashSet<int> MergableRKs;

		private static List<MyMeshMaterialId> MaterialQueryResourcesTable;

		internal static MyMeshMaterialId DebugMaterialId;

		internal static MyMeshMaterialId NullMaterialId;

		private static readonly HashSet<MyStringId> MERGABLE_MATERIAL_NAMES;

		internal static MyMeshMaterialInfo[] Table => MaterialsPool.Data;

		static MyMeshMaterials1()
		{
			MaterialsPool = new MyFreelist<MyMeshMaterialInfo>(256);
			MaterialProxyIndex = new Dictionary<MyMeshMaterialId, MyMaterialProxyId>();
			MaterialRkIndex = new Dictionary<int, MyMeshMaterialId>();
			MaterialNameIndex = new Dictionary<MyStringId, MyMeshMaterialId>(MyStringId.Comparer);
			MergableRKs = new HashSet<int>();
			MaterialQueryResourcesTable = new List<MyMeshMaterialId>();
<<<<<<< HEAD
			MERGABLE_MATERIAL_NAMES = new HashSet<MyStringId>(MyStringId.Comparer);
=======
			MERGABLE_MATERIAL_NAMES = new HashSet<MyStringId>((IEqualityComparer<MyStringId>)MyStringId.Comparer);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MERGABLE_MATERIAL_NAMES.Add(X.TEXT_("BlockSheet"));
			MERGABLE_MATERIAL_NAMES.Add(X.TEXT_("CubesSheet"));
			MERGABLE_MATERIAL_NAMES.Add(X.TEXT_("CubesMetalSheet"));
			MERGABLE_MATERIAL_NAMES.Add(X.TEXT_("RoofSheet"));
			MERGABLE_MATERIAL_NAMES.Add(X.TEXT_("StoneSheet"));
			MERGABLE_MATERIAL_NAMES.Add(X.TEXT_("House_Texture"));
			MERGABLE_MATERIAL_NAMES.Add(X.TEXT_("RoofSheetRound"));
		}

		internal static bool IsMergable(MyMeshMaterialId matId)
		{
			return MergableRKs.Contains(Table[matId.Index].RepresentationKey);
		}

		internal static MyMeshMaterialId GetMaterialId(string name)
		{
			return MaterialNameIndex.Get(X.TEXT_(name));
		}

		internal static MyMaterialProxyId GetProxyId(MyMeshMaterialId id)
		{
			if (MaterialProxyIndex.TryGetValue(id, out var value))
			{
				return value;
			}
			MyRender11.Log.WriteLine("MeshMaterialId missing");
			return MaterialProxyIndex[DebugMaterialId];
		}

		internal static int CalculateRK(ref MyMeshMaterialInfo desc)
		{
			int h = desc.ColorMetal_Texture.GetHashCode();
			MyHashHelper.Combine(ref h, desc.NormalGloss_Texture.GetHashCode());
			MyHashHelper.Combine(ref h, desc.Extensions_Texture.GetHashCode());
			MyHashHelper.Combine(ref h, desc.Alphamask_Texture.GetHashCode());
			MyHashHelper.Combine(ref h, desc.Technique.GetHashCode());
			MyHashHelper.Combine(ref h, desc.Name.ToString().GetHashCode());
			return h;
		}

		internal static MyMeshMaterialId GetMaterialId(ref MyMeshMaterialInfo desc, string assetFile = null)
		{
			int num = CalculateRK(ref desc);
			if (!MaterialRkIndex.ContainsKey(num))
			{
				MyMeshMaterialId id = (MaterialRkIndex[num] = new MyMeshMaterialId
				{
					Index = MaterialsPool.Allocate()
				});
				MyMeshMaterialId myMeshMaterialId2 = (desc.Id = id);
				desc.RepresentationKey = num;
				MaterialsPool.Data[myMeshMaterialId2.Index] = desc;
				MaterialProxyIndex[myMeshMaterialId2] = MyMaterials1.AllocateProxy();
				MaterialQueryResourcesTable.Add(myMeshMaterialId2);
				MaterialNameIndex[desc.Name] = myMeshMaterialId2;
				if (MERGABLE_MATERIAL_NAMES.Contains(desc.Name))
				{
					MergableRKs.Add(desc.RepresentationKey);
				}
				return myMeshMaterialId2;
			}
			return MaterialRkIndex[num];
		}

		internal static MyMeshMaterialId GetMaterialId(string name, string contentPath, string colorMetalTexture, string normalGlossTexture, string extensionTexture, MyMeshDrawTechnique technique)
		{
			MyMeshMaterialInfo myMeshMaterialInfo = default(MyMeshMaterialInfo);
			myMeshMaterialInfo.Name = X.TEXT_(name);
			myMeshMaterialInfo.ColorMetal_Texture = MyResourceUtils.GetTextureFullPath(colorMetalTexture, contentPath);
			myMeshMaterialInfo.NormalGloss_Texture = MyResourceUtils.GetTextureFullPath(normalGlossTexture, contentPath);
			myMeshMaterialInfo.Extensions_Texture = MyResourceUtils.GetTextureFullPath(extensionTexture, contentPath);
			myMeshMaterialInfo.Alphamask_Texture = string.Empty;
			myMeshMaterialInfo.Technique = technique;
			myMeshMaterialInfo.TextureTypes = GetMaterialTextureTypes(colorMetalTexture, normalGlossTexture, extensionTexture, null);
			myMeshMaterialInfo.Facing = MyFacingEnum.None;
			MyMeshMaterialInfo desc = myMeshMaterialInfo;
			return GetMaterialId(ref desc);
		}

		public static MyMeshMaterialInfo ConvertImportDescToMeshMaterialInfo(MyMaterialDescriptor importDesc, string contentPath, string assetFile = null)
		{
			string text = importDesc.Textures.Get("ColorMetalTexture", "");
			string text2 = importDesc.Textures.Get("NormalGlossTexture", "");
			string text3 = importDesc.Textures.Get("AddMapsTexture", "");
			string text4 = importDesc.Textures.Get("AlphamaskTexture");
			MyMeshMaterialInfo result = default(MyMeshMaterialInfo);
			result.Name = X.TEXT_(importDesc.MaterialName);
			result.ColorMetal_Texture = MyResourceUtils.GetTextureFullPath(text, contentPath);
			result.NormalGloss_Texture = MyResourceUtils.GetTextureFullPath(text2, contentPath);
			result.Extensions_Texture = MyResourceUtils.GetTextureFullPath(text3, contentPath);
			result.Alphamask_Texture = MyResourceUtils.GetTextureFullPath(text4, contentPath);
			result.TextureTypes = GetMaterialTextureTypes(text, text2, text3, text4);
			result.Technique = importDesc.TechniqueEnum;
			result.Facing = importDesc.Facing;
			result.WindScaleAndFreq = importDesc.WindScaleAndFreq;
			return result;
		}

		internal static MyMeshMaterialId GetMaterialId(MyMaterialDescriptor importDesc, string contentPath, string assetFile = null)
		{
			if (importDesc != null)
			{
				MyMeshMaterialInfo desc = ConvertImportDescToMeshMaterialInfo(importDesc, contentPath, assetFile);
				return GetMaterialId(ref desc, assetFile);
			}
			return NullMaterialId;
		}

		internal static void InvalidateMaterials(string name)
		{
			for (int i = 0; i < MaterialsPool.FilledSize; i++)
			{
				ref MyMeshMaterialInfo reference = ref MaterialsPool[i];
				if (reference.ColorMetal_Texture == name || reference.NormalGloss_Texture == name || reference.Alphamask_Texture == name || reference.Extensions_Texture == name)
				{
					InvalidateMaterial(reference.Id);
				}
			}
		}

		internal static void OnResourcesGathering(bool preloadTextures)
		{
			if (MaterialQueryResourcesTable.Count <= 0)
			{
				return;
			}
			foreach (MyMeshMaterialId item in MaterialQueryResourcesTable)
			{
				MyMeshMaterialInfo.RequestResources(ref MaterialsPool.Data[item.Index], preloadTextures);
			}
			foreach (MyMeshMaterialId item2 in MaterialQueryResourcesTable)
			{
				MyMaterials1.ProxyPool.Data[MaterialProxyIndex[item2].Index] = MyMeshMaterialInfo.CreateProxy(ref MaterialsPool.Data[item2.Index]);
			}
			MaterialQueryResourcesTable.Clear();
		}

		internal static void CreateCommonMaterials()
		{
			MyMeshMaterialInfo myMeshMaterialInfo = default(MyMeshMaterialInfo);
			myMeshMaterialInfo.Name = X.TEXT_("__NULL_MATERIAL");
			myMeshMaterialInfo.ColorMetal_Texture = string.Empty;
			myMeshMaterialInfo.NormalGloss_Texture = string.Empty;
			myMeshMaterialInfo.Extensions_Texture = string.Empty;
			myMeshMaterialInfo.Alphamask_Texture = string.Empty;
			myMeshMaterialInfo.Technique = MyMeshDrawTechnique.MESH;
			MyMeshMaterialInfo desc = myMeshMaterialInfo;
			NullMaterialId = GetMaterialId(ref desc);
			myMeshMaterialInfo = default(MyMeshMaterialInfo);
			myMeshMaterialInfo.Name = X.TEXT_("__DEBUG_MATERIAL");
			myMeshMaterialInfo.ColorMetal_Texture = string.Empty;
			myMeshMaterialInfo.NormalGloss_Texture = string.Empty;
			myMeshMaterialInfo.Extensions_Texture = string.Empty;
			myMeshMaterialInfo.Alphamask_Texture = string.Empty;
			myMeshMaterialInfo.Technique = MyMeshDrawTechnique.MESH;
			MyMeshMaterialInfo desc2 = myMeshMaterialInfo;
			DebugMaterialId = GetMaterialId(ref desc2);
		}

		internal static void Init()
		{
			CreateCommonMaterials();
		}

		internal static void OnDeviceReset()
		{
			InvalidateMaterials();
		}

		internal static void InvalidateMaterial(MyMeshMaterialId id)
		{
			MaterialQueryResourcesTable.Add(id);
		}

		internal static void InvalidateMaterials()
		{
			foreach (MyMeshMaterialId value in MaterialRkIndex.Values)
			{
				MaterialQueryResourcesTable.Add(value);
			}
		}

		internal static void OnSessionEnd()
		{
		}

		public static void OnDeviceEnd()
		{
			MergableRKs.Clear();
			MaterialQueryResourcesTable.Clear();
			MaterialRkIndex.Clear();
			MaterialsPool.Clear();
			MaterialProxyIndex.Clear();
			MaterialNameIndex.Clear();
		}

		public static MyFileTextureEnum GetMaterialTextureTypes(string colorMetalTexture, string normalGlossTexture, string extensionTexture, string alphamaskTexture)
		{
			MyFileTextureEnum myFileTextureEnum = MyFileTextureEnum.UNSPECIFIED;
			if (!string.IsNullOrEmpty(colorMetalTexture))
			{
				myFileTextureEnum |= MyFileTextureEnum.COLOR_METAL;
			}
			if (!string.IsNullOrEmpty(normalGlossTexture))
			{
				myFileTextureEnum |= MyFileTextureEnum.NORMALMAP_GLOSS;
			}
			if (!string.IsNullOrEmpty(extensionTexture))
			{
				myFileTextureEnum |= MyFileTextureEnum.EXTENSIONS;
			}
			if (!string.IsNullOrEmpty(alphamaskTexture))
			{
				myFileTextureEnum |= MyFileTextureEnum.ALPHAMASK;
			}
			return myFileTextureEnum;
		}

		/// <summary>Get macro bundles for texture types</summary>
		public static ShaderMacro[] GetMaterialTextureMacros(MyFileTextureEnum textures)
		{
			List<ShaderMacro> list = new List<ShaderMacro>();
			if (textures.HasFlag(MyFileTextureEnum.COLOR_METAL))
			{
				list.Add(new ShaderMacro("USE_COLORMETAL_TEXTURE", null));
			}
			if (textures.HasFlag(MyFileTextureEnum.NORMALMAP_GLOSS))
			{
				list.Add(new ShaderMacro("USE_NORMALGLOSS_TEXTURE", null));
			}
			if (textures.HasFlag(MyFileTextureEnum.EXTENSIONS))
			{
				list.Add(new ShaderMacro("USE_EXTENSIONS_TEXTURE", null));
			}
			return list.ToArray();
		}

		/// <summary>Bind blend states for alpha blending</summary>
		public static IBlendState GetMaterialTextureBlendState(MyFileTextureEnum textures, bool premultipliedAlpha)
		{
			textures &= ~MyFileTextureEnum.ALPHAMASK;
			switch (textures)
			{
			case MyFileTextureEnum.COLOR_METAL:
				if (premultipliedAlpha)
				{
					return MyBlendStateManager.BlendDecalColor;
				}
				return MyBlendStateManager.BlendDecalColorNoPremult;
			case MyFileTextureEnum.NORMALMAP_GLOSS:
				if (premultipliedAlpha)
				{
					return MyBlendStateManager.BlendDecalNormal;
				}
				return MyBlendStateManager.BlendDecalNormalNoPremult;
			case MyFileTextureEnum.COLOR_METAL | MyFileTextureEnum.NORMALMAP_GLOSS:
				if (premultipliedAlpha)
				{
					return MyBlendStateManager.BlendDecalNormalColor;
				}
				return MyBlendStateManager.BlendDecalNormalColorNoPremult;
			case MyFileTextureEnum.COLOR_METAL | MyFileTextureEnum.NORMALMAP_GLOSS | MyFileTextureEnum.EXTENSIONS:
				if (premultipliedAlpha)
				{
					return MyBlendStateManager.BlendDecalNormalColorExt;
				}
				return MyBlendStateManager.BlendDecalNormalColorExtNoPremult;
			default:
				throw new Exception("Unknown texture bundle type");
			}
		}
	}
}
