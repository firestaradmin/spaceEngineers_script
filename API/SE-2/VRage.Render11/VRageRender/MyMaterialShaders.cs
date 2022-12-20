using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRage.Library.Extensions;
using VRage.Render11.Resources;
using VRage.Utils;
using VRageRender.Import;

namespace VRageRender
{
	internal static class MyMaterialShaders
	{
		public const string GEOMETRY_FOLDER = "Geometry";

		public const string GBUFFER_PASS = "GBuffer";

		public const string DEPTH_PASS = "Depth";

		public const string FORWARD_PASS = "Forward";

		public const string HIGHLIGHT_PASS = "Highlight";

		public const string FOLIAGE_STREAMING_PASS = "FoliageStreaming";

		public const string TRANSPARENT_MODEL_PASS = "Transparent";

		public const string TRANSPARENT_MODEL_PASS_FOR_DECALS = "TransparentForDecals";

		public static MyStringId GBUFFER_PASS_ID = X.TEXT_("GBuffer");

		public static MyStringId DEPTH_PASS_ID = X.TEXT_("Depth");

		public static MyStringId FORWARD_PASS_ID = X.TEXT_("Forward");

		public static MyStringId HIGHLIGHT_PASS_ID = X.TEXT_("Highlight");

		public static MyStringId FOLIAGE_STREAMING_PASS_ID = X.TEXT_("FoliageStreaming");

		public static MyStringId TRANSPARENT_MODEL_PASS_ID = X.TEXT_("Transparent");

		public static MyStringId DEFAULT_MATERIAL_TAG = X.TEXT_("Standard");

		public static MyStringId ALPHA_MASKED_MATERIAL_TAG = X.TEXT_("AlphaMasked");

		public static MyStringId TRIPLANAR_SINGLE_MATERIAL_TAG = X.TEXT_("TriplanarSingle");

		public static MyStringId TRIPLANAR_MULTI_MATERIAL_TAG = X.TEXT_("TriplanarMulti");

		public static MyStringId TRIPLANAR_DEBRIS_MATERIAL_TAG = X.TEXT_("TriplanarDebris");

		public static MyStringId GLASS_MATERIAL_TAG = X.TEXT_("Glass");

		public static MyStringId HOLO_MATERIAL_TAG = X.TEXT_("Holo");

		public static MyStringId SHIELD_MATERIAL_TAG = X.TEXT_("Shield");

		public static MyStringId SHIELD_LIT_MATERIAL_TAG = X.TEXT_("ShieldLit");

		private static readonly Dictionary<MyStringId, MyMaterialShaderInfo> m_materialSources = new Dictionary<MyStringId, MyMaterialShaderInfo>(MyStringId.Comparer);

		private static readonly Dictionary<int, MyMaterialShadersBundleId> m_hashIndex = new Dictionary<int, MyMaterialShadersBundleId>();

		internal static readonly MyFreelist<MyMaterialShadersInfo> BundleInfo = new MyFreelist<MyMaterialShadersInfo>(64);

		internal static MyMaterialShadersBundle[] Bundles = new MyMaterialShadersBundle[64];

		private static readonly int[] m_techniquesOrdering = new int[22]
		{
			0, 9, 6, 10, 11, 12, 13, 14, 15, 16,
			7, 8, 5, 1, 2, 17, 3, 4, 20, 21,
			18, 19
		};

		public static string MaterialsFolder => Path.Combine("Geometry", "Materials");

		public static string PassesFolder => Path.Combine("Geometry", "Passes");

		internal static void AddMaterialShaderFlagMacrosTo(List<ShaderMacro> list, MyShaderUnifiedFlags flags, MyFileTextureEnum textureTypes = MyFileTextureEnum.UNSPECIFIED)
		{
			if ((flags & MyShaderUnifiedFlags.DEPTH_ONLY) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("DEPTH_ONLY", null));
			}
			if ((flags & MyShaderUnifiedFlags.ALPHA_MASKED) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("ALPHA_MASKED", null));
			}
			if ((flags & MyShaderUnifiedFlags.ALPHA_MASK_ARRAY) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("ALPHA_MASK_ARRAY", null));
			}
			if ((flags & MyShaderUnifiedFlags.STATIC_DECAL) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("STATIC_DECAL", null));
				list.AddRange(MyMeshMaterials1.GetMaterialTextureMacros(textureTypes));
			}
			if ((flags & MyShaderUnifiedFlags.STATIC_DECAL_CUTOUT) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("STATIC_DECAL_CUTOUT", null));
				list.AddRange(MyMeshMaterials1.GetMaterialTextureMacros(textureTypes));
			}
			if ((flags & MyShaderUnifiedFlags.TRANSPARENT) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("TRANSPARENT", null));
			}
			if ((flags & MyShaderUnifiedFlags.USE_SKINNING) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("USE_SKINNING", null));
			}
			if ((flags & MyShaderUnifiedFlags.USE_CUBE_INSTANCING) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("USE_CUBE_INSTANCING", null));
			}
			if ((flags & MyShaderUnifiedFlags.USE_DEFORMED_CUBE_INSTANCING) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("USE_DEFORMED_CUBE_INSTANCING", null));
			}
			if ((flags & MyShaderUnifiedFlags.USE_GENERIC_INSTANCING) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("USE_GENERIC_INSTANCING", null));
			}
			if ((flags & MyShaderUnifiedFlags.USE_MERGE_INSTANCING) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("USE_MERGE_INSTANCING", null));
			}
			if ((flags & MyShaderUnifiedFlags.USE_SINGLE_INSTANCE) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("USE_SINGLE_INSTANCE", null));
			}
			if ((flags & MyShaderUnifiedFlags.USE_VOXEL_MORPHING) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("USE_VOXEL_MORPHING", null));
			}
			if ((flags & MyShaderUnifiedFlags.USE_VOXEL_DATA) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("USE_VOXEL_DATA", null));
			}
			if ((flags & MyShaderUnifiedFlags.USE_TEXTURE_INDICES) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("USE_TEXTURE_INDICES", null));
			}
			if ((flags & MyShaderUnifiedFlags.USE_COLORMETAL_TEXTURE) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("USE_COLORMETAL_TEXTURE", null));
			}
			if ((flags & MyShaderUnifiedFlags.USE_NORMALGLOSS_TEXTURE) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("USE_NORMALGLOSS_TEXTURE", null));
			}
			if ((flags & MyShaderUnifiedFlags.USE_EXTENSIONS_TEXTURE) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("USE_EXTENSIONS_TEXTURE", null));
			}
			if ((flags & MyShaderUnifiedFlags.LQ) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("LQ", null));
			}
			if ((flags & MyShaderUnifiedFlags.MQ) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("MQ", null));
			}
			if ((flags & MyShaderUnifiedFlags.DISTANCE_FADE) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("DISTANCE_FADE", null));
			}
			if ((flags & MyShaderUnifiedFlags.METALNESS_COLORABLE) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("METALNESS_COLORABLE", null));
			}
			if ((flags & MyShaderUnifiedFlags.USE_SIMPLE_INSTANCING) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("USE_SIMPLE_INSTANCING", null));
			}
			if ((flags & MyShaderUnifiedFlags.USE_SIMPLE_INSTANCING_COLORING) > MyShaderUnifiedFlags.NONE)
			{
				list.Add(new ShaderMacro("USE_SIMPLE_INSTANCING_COLORING", null));
			}
		}

		internal static MyMaterialShadersBundleId Get(MyStringId material, MyStringId materialPass, VertexLayoutId vertexLayout, MyShaderUnifiedFlags flags, MyFileTextureEnum textureTypes)
		{
			Monitor.Enter(m_hashIndex);
			int h = 0;
			MyHashHelper.Combine(ref h, material.GetHashCode());
			MyHashHelper.Combine(ref h, materialPass.GetHashCode());
			MyHashHelper.Combine(ref h, vertexLayout.GetHashCode());
			MyHashHelper.Combine(ref h, (int)flags);
			if (m_hashIndex.TryGetValue(h, out var value))
			{
				Monitor.Exit(m_hashIndex);
				lock (Bundles[value.Index])
				{
					return value;
				}
			}
			MyMaterialShadersBundleId myMaterialShadersBundleId = default(MyMaterialShadersBundleId);
			myMaterialShadersBundleId.Index = BundleInfo.Allocate();
			value = myMaterialShadersBundleId;
			MyArrayHelpers.Reserve(ref Bundles, value.Index + 1);
			m_hashIndex[h] = value;
			BundleInfo.Data[value.Index] = new MyMaterialShadersInfo
			{
				Material = material,
				Pass = materialPass,
				Layout = vertexLayout,
				Flags = flags,
				TextureTypes = textureTypes
			};
			Bundles[value.Index] = new MyMaterialShadersBundle();
			lock (Bundles[value.Index])
			{
				Monitor.Exit(m_hashIndex);
				InitBundle(value);
				return value;
			}
		}

		private static void ClearSources()
		{
			m_materialSources.Clear();
		}

		internal static void Recompile()
		{
			ClearSources();
			foreach (MyMaterialShadersBundleId value in m_hashIndex.Values)
			{
				InitBundle(value);
			}
		}

		internal static void GetMaterialSources(MyStringId id, out MyMaterialShaderInfo info)
		{
			if (!m_materialSources.TryGetValue(id, out info))
			{
				info = default(MyMaterialShaderInfo);
				info.VertexShaderFilename = Path.Combine(MaterialsFolder, id.ToString(), "Vertex.hlsl");
				info.VertexShaderFilepath = Path.Combine(MyShaderCompiler.ShadersPath, info.VertexShaderFilename);
				info.PixelShaderFilename = Path.Combine(MaterialsFolder, id.ToString(), "Pixel.hlsl");
				info.PixelShaderFilepath = Path.Combine(MyShaderCompiler.ShadersPath, info.PixelShaderFilename);
				m_materialSources[id] = info;
			}
		}

		private static void InitBundle(MyMaterialShadersBundleId id, bool invalidateCache = false)
		{
			MyMaterialShadersInfo myMaterialShadersInfo = BundleInfo.Data[id.Index];
			List<ShaderMacro> obj = new List<ShaderMacro> { GetRenderingPassMacro(myMaterialShadersInfo.Pass.String) };
			AddMaterialShaderFlagMacrosTo(obj, myMaterialShadersInfo.Flags, myMaterialShadersInfo.TextureTypes);
			obj.AddRange(myMaterialShadersInfo.Layout.Info.Macros);
			GetMaterialSources(myMaterialShadersInfo.Material, out var info);
			ShaderMacro[] macros = obj.ToArray();
			string shaderDescriptor = GetShaderDescriptor(info.VertexShaderFilename, myMaterialShadersInfo.Material.String, myMaterialShadersInfo.Pass.String, myMaterialShadersInfo.Layout);
			byte[] array = MyShaderCompiler.Compile(info.VertexShaderFilepath, macros, MyShaderProfile.vs_5_0, shaderDescriptor, invalidateCache);
			string shaderDescriptor2 = GetShaderDescriptor(info.PixelShaderFilename, myMaterialShadersInfo.Material.String, myMaterialShadersInfo.Pass.String, myMaterialShadersInfo.Layout);
			byte[] array2 = MyShaderCompiler.Compile(info.PixelShaderFilepath, macros, MyShaderProfile.ps_5_0, shaderDescriptor2, invalidateCache);
			if (array != null && array2 != null)
			{
				if (Bundles[id.Index].IL != null)
				{
					Bundles[id.Index].IL.Dispose();
					Bundles[id.Index].IL = null;
				}
				if (Bundles[id.Index].VS != null)
				{
					Bundles[id.Index].VS.Dispose();
					Bundles[id.Index].VS = null;
				}
				if (Bundles[id.Index].PS != null)
				{
					Bundles[id.Index].PS.Dispose();
					Bundles[id.Index].PS = null;
				}
				try
				{
					Bundles[id.Index].VS = new VertexShader(MyRender11.DeviceInstance, array);
					Bundles[id.Index].VS.DebugName = shaderDescriptor;
					Bundles[id.Index].PS = new PixelShader(MyRender11.DeviceInstance, array2);
					Bundles[id.Index].PS.DebugName = shaderDescriptor2;
					Bundles[id.Index].IL = ((myMaterialShadersInfo.Layout.Elements.Length != 0) ? new InputLayout(MyRender11.DeviceInstance, array, myMaterialShadersInfo.Layout.Elements) : null);
				}
				catch (SharpDXException)
				{
					if (!invalidateCache)
					{
						InitBundle(id, invalidateCache: true);
						return;
					}
					string text = "Failed to initialize material shader" + myMaterialShadersInfo.Name + " for vertex " + myMaterialShadersInfo.Layout.Info.Components.GetString();
					MyRender11.Log.WriteLine(text);
					throw new MyRenderException(text);
				}
			}
			else
			{
				string msg = "Failed to compile material shader" + myMaterialShadersInfo.Name + " for vertex " + myMaterialShadersInfo.Layout.Info.Components.GetString();
				MyRender11.Log.WriteLine(msg);
				msg = ((array == null && array2 != null) ? ("vsByteCode is null, descriptor: " + shaderDescriptor) : ((array == null || array2 != null) ? ("vsByteCode and psByteCode are null, vsDescriptor: " + shaderDescriptor + "; psDescriptor: " + shaderDescriptor2) : ("psByteCode is null, descriptor: " + shaderDescriptor2)));
				MyRender11.Log.WriteLine(msg);
				if (Bundles[id.Index].VS == null && Bundles[id.Index].PS == null)
				{
					throw new MyRenderException(msg);
				}
			}
		}

		public static string GetShaderDescriptor(string shaderFilename, string material, string pass, VertexLayoutId layout)
		{
			return $"{shaderFilename}, {material}_{pass}_{layout.Info.Components.GetString()}";
		}

		internal static ShaderMacro GetRenderingPassMacro(string pass)
		{
<<<<<<< HEAD
			switch (pass)
			{
			case "GBuffer":
				return new ShaderMacro("RENDERING_PASS", 0);
			case "Depth":
				return new ShaderMacro("RENDERING_PASS", 1);
			case "Forward":
				return new ShaderMacro("RENDERING_PASS", 2);
			case "Highlight":
				return new ShaderMacro("RENDERING_PASS", 3);
			case "FoliageStreaming":
				return new ShaderMacro("RENDERING_PASS", 4);
			case "Transparent":
				return new ShaderMacro("RENDERING_PASS", 5);
			case "TransparentForDecals":
				return new ShaderMacro("RENDERING_PASS", 6);
			default:
				throw new Exception();
			}
=======
			return pass switch
			{
				"GBuffer" => new ShaderMacro("RENDERING_PASS", 0), 
				"Depth" => new ShaderMacro("RENDERING_PASS", 1), 
				"Forward" => new ShaderMacro("RENDERING_PASS", 2), 
				"Highlight" => new ShaderMacro("RENDERING_PASS", 3), 
				"FoliageStreaming" => new ShaderMacro("RENDERING_PASS", 4), 
				"Transparent" => new ShaderMacro("RENDERING_PASS", 5), 
				"TransparentForDecals" => new ShaderMacro("RENDERING_PASS", 6), 
				_ => throw new Exception(), 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		internal static MyStringId MapTechniqueToDefaultPass(MyMeshDrawTechnique technique)
		{
			if (technique == MyMeshDrawTechnique.HOLO || technique == MyMeshDrawTechnique.GLASS || technique - 20 <= MyMeshDrawTechnique.VOXELS_DEBRIS)
			{
				return TRANSPARENT_MODEL_PASS_ID;
			}
			return GBUFFER_PASS_ID;
		}

		internal static int MapTechniqueToSortKey(MyMeshDrawTechnique technique)
		{
			return m_techniquesOrdering[(uint)technique];
		}

		internal static bool IsDecal(MyMeshDrawTechnique technique)
		{
			if (technique - 6 <= MyMeshDrawTechnique.VOXEL_MAP)
			{
				return true;
			}
			return false;
		}

		internal static MyStringId MapTechniqueToShaderMaterial(MyMeshDrawTechnique technique)
		{
			switch (technique)
			{
			case MyMeshDrawTechnique.VOXEL_MAP_SINGLE:
				return TRIPLANAR_SINGLE_MATERIAL_TAG;
			case MyMeshDrawTechnique.VOXEL_MAP_MULTI:
				return TRIPLANAR_MULTI_MATERIAL_TAG;
			case MyMeshDrawTechnique.VOXELS_DEBRIS:
				return TRIPLANAR_DEBRIS_MATERIAL_TAG;
			case MyMeshDrawTechnique.ALPHA_MASKED:
			case MyMeshDrawTechnique.ALPHA_MASKED_SINGLE_SIDED:
			case MyMeshDrawTechnique.FOLIAGE:
				return ALPHA_MASKED_MATERIAL_TAG;
			case MyMeshDrawTechnique.GLASS:
				return GLASS_MATERIAL_TAG;
			case MyMeshDrawTechnique.HOLO:
				return HOLO_MATERIAL_TAG;
			case MyMeshDrawTechnique.SHIELD:
				return SHIELD_MATERIAL_TAG;
			case MyMeshDrawTechnique.SHIELD_LIT:
				return SHIELD_LIT_MATERIAL_TAG;
			default:
				return DEFAULT_MATERIAL_TAG;
			}
		}

		internal static void OnDeviceEnd()
		{
			foreach (MyMaterialShadersBundleId value in m_hashIndex.Values)
			{
				if (Bundles[value.Index].IL != null)
				{
					Bundles[value.Index].IL.Dispose();
					Bundles[value.Index].IL = null;
				}
				if (Bundles[value.Index].VS != null)
				{
					Bundles[value.Index].VS.Dispose();
					Bundles[value.Index].VS = null;
				}
				if (Bundles[value.Index].PS != null)
				{
					Bundles[value.Index].PS.Dispose();
					Bundles[value.Index].PS = null;
				}
			}
		}

		internal static void OnDeviceReset()
		{
			OnDeviceEnd();
			foreach (MyMaterialShadersBundleId value in m_hashIndex.Values)
			{
				InitBundle(value);
			}
		}
	}
}
