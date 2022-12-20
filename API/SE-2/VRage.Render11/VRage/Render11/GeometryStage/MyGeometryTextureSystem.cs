using System;
using System.Collections.Generic;
using System.Text;
using SharpDX.DXGI;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;

namespace VRage.Render11.GeometryStage
{
	internal class MyGeometryTextureSystem : IManager
	{
		private struct MyArrayTextureKey
		{
			public Vector2I ResolutionInFile;

			public int MipLevels;

			public Format Format;

			public MyChannel Channel;

			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("{0}-{1}x{2},{3}:{4}.dds", Channel, ResolutionInFile.X, ResolutionInFile.Y, MipLevels, Format);
				return stringBuilder.ToString();
			}
		}

		private struct MyMaterialKey
		{
			public string cmParams;

			public string ngParams;

			public string extParams;

			public string alphamaskParams;

			public MyMaterialKey(string cmFilepath, string ngFilepath, string extFilepath, string alphamaskFilepath)
			{
				cmParams = cmFilepath;
				ngParams = ngFilepath;
				extParams = extFilepath;
				alphamaskParams = alphamaskFilepath;
			}
		}

		private const int DEFAULT_ARRAY_TEXTURE_INDEX = -1;

		private Dictionary<MyArrayTextureKey, IDynamicFileArrayTexture> m_dictTextures = new Dictionary<MyArrayTextureKey, IDynamicFileArrayTexture>();

		private HashSet<MyMaterialKey> m_setDebugMaterials = new HashSet<MyMaterialKey>();

		private HashSet<string> m_checkedFilepaths = new HashSet<string>();

		private static MyFileTextureEnum GetTextureType(MyChannel channel)
		{
			MyFileTextureEnum result = MyFileTextureEnum.UNSPECIFIED;
			switch (channel)
			{
			case MyChannel.ColorMetal:
				result = MyFileTextureEnum.COLOR_METAL;
				break;
			case MyChannel.NormalGloss:
				result = MyFileTextureEnum.NORMALMAP_GLOSS;
				break;
			case MyChannel.Extension:
				result = MyFileTextureEnum.EXTENSIONS;
				break;
			case MyChannel.Alphamask:
				result = MyFileTextureEnum.ALPHAMASK;
				break;
			}
			return result;
		}

		private IDynamicFileArrayTexture GetArrayTextureFromKey(MyArrayTextureKey key)
		{
			if (m_dictTextures.TryGetValue(key, out var value))
			{
				return value;
			}
			string name = key.ToString();
			MyFileTextureEnum textureType = GetTextureType(key.Channel);
			byte[] bytePattern = MyGeneratedTexturePatterns.GetBytePattern(key.Channel, key.Format);
			value = MyManagers.DynamicFileArrayTextures.CreateTexture(name, textureType, bytePattern, key.Format, 0);
			m_dictTextures[key] = value;
			return value;
		}

		private IDynamicFileArrayTexture GetArrayTextureFromFilepath(string filepath, MyChannel channel, Vector2I defaultResolution)
		{
			MyArrayTextureKey key = default(MyArrayTextureKey);
			key.Channel = channel;
			if (MyFileTextureParamsManager.LoadFromFile(filepath, out var outParams))
			{
				Format format = outParams.Format;
				if (channel != MyChannel.NormalGloss)
				{
					format = MyResourceUtils.MakeSrgb(format);
				}
				key.Format = format;
				key.ResolutionInFile = outParams.Resolution;
				key.MipLevels = outParams.MipLevels;
			}
			else
			{
<<<<<<< HEAD
				Format format2;
				switch (channel)
				{
				case MyChannel.ColorMetal:
					format2 = Format.BC7_UNorm_SRgb;
					break;
				case MyChannel.NormalGloss:
					format2 = Format.BC7_UNorm;
					break;
				case MyChannel.Extension:
					format2 = Format.BC7_UNorm_SRgb;
					break;
				case MyChannel.Alphamask:
					format2 = Format.BC4_UNorm;
					break;
				default:
					format2 = Format.Unknown;
					break;
				}
				key.Format = format2;
=======
				key.Format = channel switch
				{
					MyChannel.ColorMetal => Format.BC7_UNorm_SRgb, 
					MyChannel.NormalGloss => Format.BC7_UNorm, 
					MyChannel.Extension => Format.BC7_UNorm_SRgb, 
					MyChannel.Alphamask => Format.BC4_UNorm, 
					_ => Format.Unknown, 
				};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				key.ResolutionInFile = defaultResolution;
				key.MipLevels = MyResourceUtils.GetMipLevels(Math.Max(key.ResolutionInFile.X, key.ResolutionInFile.Y));
			}
			return GetArrayTextureFromKey(key);
		}

		private Vector2I GetDefaultTextureSize(string cmFilepath, string ngFilepath, string extFilepath, string alphamaskFilepath)
		{
			Vector2I resolutionFromFile = MyFileTextureParamsManager.GetResolutionFromFile(cmFilepath);
			Vector2I resolutionFromFile2 = MyFileTextureParamsManager.GetResolutionFromFile(ngFilepath);
			Vector2I resolutionFromFile3 = MyFileTextureParamsManager.GetResolutionFromFile(extFilepath);
			Vector2I resolutionFromFile4 = MyFileTextureParamsManager.GetResolutionFromFile(alphamaskFilepath);
			Vector2I vector2I = resolutionFromFile;
			if (vector2I == Vector2I.Zero)
			{
				vector2I = Vector2I.Max(resolutionFromFile2, Vector2I.Max(resolutionFromFile3, resolutionFromFile4));
			}
			if (vector2I == Vector2I.Zero)
			{
				vector2I = new Vector2I(1024, 1024);
			}
			return vector2I;
		}

		private int GetArrayIndexFromFilepath(string filepath, MyChannel channel, Vector2I resolution)
		{
			return GetArrayTextureFromFilepath(filepath, channel, resolution)?.GetOrAddSlice(filepath) ?? (-1);
		}

		public bool IsMaterialAcceptableForTheSystem(MyMeshMaterialInfo info)
		{
			if (MyFileTextureParamsManager.IsArrayTextureInFile(info.ColorMetal_Texture))
			{
				return false;
			}
			if (MyFileTextureParamsManager.IsArrayTextureInFile(info.NormalGloss_Texture))
			{
				return false;
			}
			if (MyFileTextureParamsManager.IsArrayTextureInFile(info.Extensions_Texture))
			{
				return false;
			}
			if (MyFileTextureParamsManager.IsArrayTextureInFile(info.Alphamask_Texture))
			{
				return false;
			}
			return true;
		}

		private void CheckTexture(string filepath, MyChannel channel, Format format, Vector2I texSize)
		{
			if (m_checkedFilepaths.Contains(filepath))
			{
				return;
			}
			m_checkedFilepaths.Add(filepath);
			if (MyFileTextureParamsManager.LoadFromFile(filepath, out var outParams))
			{
				if (outParams.Format != format)
				{
					MyRenderProxy.Log.WriteLineAndConsole($"{channel} texture '{filepath}' should be {format}");
				}
				if (outParams.Resolution != texSize)
				{
					MyRenderProxy.Log.WriteLineAndConsole($"{channel} texture '{filepath}' should be {texSize.X}x{texSize.Y}");
				}
			}
		}

		public void ValidateMaterialTextures(MyMeshMaterialInfo info)
		{
			Vector2I defaultTextureSize = GetDefaultTextureSize(info.ColorMetal_Texture, info.NormalGloss_Texture, info.Extensions_Texture, info.Alphamask_Texture);
			CheckTexture(info.ColorMetal_Texture, MyChannel.ColorMetal, Format.BC7_UNorm_SRgb, defaultTextureSize);
			CheckTexture(info.NormalGloss_Texture, MyChannel.NormalGloss, Format.BC7_UNorm, defaultTextureSize);
			CheckTexture(info.Extensions_Texture, MyChannel.Extension, Format.BC7_UNorm_SRgb, defaultTextureSize);
			CheckTexture(info.Alphamask_Texture, MyChannel.Alphamask, Format.BC4_UNorm, defaultTextureSize);
		}

		public MyMeshMaterialId GetOrCreateMaterialId(MyMeshMaterialInfo info)
		{
			Vector2I defaultTextureSize = GetDefaultTextureSize(info.ColorMetal_Texture, info.NormalGloss_Texture, info.Extensions_Texture, info.Alphamask_Texture);
			IDynamicFileArrayTexture arrayTextureFromFilepath = GetArrayTextureFromFilepath(info.ColorMetal_Texture, MyChannel.ColorMetal, defaultTextureSize);
			int orAddSlice = arrayTextureFromFilepath.GetOrAddSlice(info.ColorMetal_Texture);
			IDynamicFileArrayTexture arrayTextureFromFilepath2 = GetArrayTextureFromFilepath(info.NormalGloss_Texture, MyChannel.NormalGloss, defaultTextureSize);
			int orAddSlice2 = arrayTextureFromFilepath2.GetOrAddSlice(info.NormalGloss_Texture);
			IDynamicFileArrayTexture arrayTextureFromFilepath3 = GetArrayTextureFromFilepath(info.Extensions_Texture, MyChannel.Extension, defaultTextureSize);
			int orAddSlice3 = arrayTextureFromFilepath3.GetOrAddSlice(info.Extensions_Texture);
			info.ColorMetal_Texture = arrayTextureFromFilepath.Name;
			info.NormalGloss_Texture = arrayTextureFromFilepath2.Name;
			info.Extensions_Texture = arrayTextureFromFilepath3.Name;
<<<<<<< HEAD
			MyMaterialKey item = new MyMaterialKey(info.ColorMetal_Texture, info.NormalGloss_Texture, info.Extensions_Texture, info.Alphamask_Texture);
			m_setDebugMaterials.Add(item);
=======
			MyMaterialKey myMaterialKey = new MyMaterialKey(info.ColorMetal_Texture, info.NormalGloss_Texture, info.Extensions_Texture, info.Alphamask_Texture);
			m_setDebugMaterials.Add(myMaterialKey);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyGeometryTextureSystemReference geometryTextureRef = default(MyGeometryTextureSystemReference);
			geometryTextureRef.ColorMetalTexture = arrayTextureFromFilepath;
			geometryTextureRef.ColorMetalIndex = orAddSlice;
			geometryTextureRef.NormalGlossTexture = arrayTextureFromFilepath2;
			geometryTextureRef.NormalGlossIndex = orAddSlice2;
			geometryTextureRef.ExtensionTexture = arrayTextureFromFilepath3;
			geometryTextureRef.ExtensionIndex = orAddSlice3;
			geometryTextureRef.AlphamaskTexture = null;
			geometryTextureRef.AlphamaskIndex = -1;
			if (!string.IsNullOrEmpty(info.Alphamask_Texture))
			{
				IDynamicFileArrayTexture arrayTextureFromFilepath4 = GetArrayTextureFromFilepath(info.Alphamask_Texture, MyChannel.Alphamask, defaultTextureSize);
				int orAddSlice4 = arrayTextureFromFilepath4.GetOrAddSlice(info.Alphamask_Texture);
				info.Alphamask_Texture = arrayTextureFromFilepath4.Name;
				geometryTextureRef.AlphamaskTexture = arrayTextureFromFilepath4;
				geometryTextureRef.AlphamaskIndex = orAddSlice4;
			}
			geometryTextureRef.IsUsed = true;
			info.GeometryTextureRef = geometryTextureRef;
			return MyMeshMaterials1.GetMaterialId(ref info);
		}

		private Vector4I GetTextureIndices(string colorMetalTexture, string normalGlossTexture, string extensionTexture, string alphamaskTexture)
		{
			if (!MyRender11.Settings.UseGeometryArrayTextures)
			{
				return new Vector4I(-1, -1, -1, -1);
			}
			Vector2I defaultTextureSize = GetDefaultTextureSize(colorMetalTexture, normalGlossTexture, extensionTexture, alphamaskTexture);
			Vector4I result = default(Vector4I);
			result.X = GetArrayIndexFromFilepath(colorMetalTexture, MyChannel.ColorMetal, defaultTextureSize);
			result.Y = GetArrayIndexFromFilepath(normalGlossTexture, MyChannel.NormalGloss, defaultTextureSize);
			result.Z = GetArrayIndexFromFilepath(extensionTexture, MyChannel.Extension, defaultTextureSize);
			result.W = GetArrayIndexFromFilepath(alphamaskTexture, MyChannel.Alphamask, defaultTextureSize);
			return result;
		}

		public Vector4I[] CreateTextureIndices(List<MyMeshPartInfo> partInfos, int verticesNum, string contentPath)
		{
			Vector4I[] array = new Vector4I[verticesNum];
			for (int i = 0; i < verticesNum; i++)
			{
				array[i] = new Vector4I(-1, -1, -1, -1);
			}
			if (!MyRender11.Settings.UseGeometryArrayTextures)
			{
				return array;
			}
			foreach (MyMeshPartInfo partInfo in partInfos)
			{
				MyMaterialDescriptor materialDesc = partInfo.m_MaterialDesc;
				if (materialDesc == null || !materialDesc.Textures.TryGetValue("ColorMetalTexture", out var value) || !materialDesc.Textures.TryGetValue("NormalGlossTexture", out var value2))
				{
					continue;
				}
				materialDesc.Textures.TryGetValue("AddMapsTexture", out var value3);
				materialDesc.Textures.TryGetValue("AlphamaskTexture", out var value4);
				value = MyResourceUtils.GetTextureFullPath(value, contentPath);
				value2 = MyResourceUtils.GetTextureFullPath(value2, contentPath);
				value3 = MyResourceUtils.GetTextureFullPath(value3, contentPath);
				value4 = MyResourceUtils.GetTextureFullPath(value4, contentPath);
				Vector4I textureIndices = GetTextureIndices(value, value2, value3, value4);
				foreach (int index in partInfo.m_indices)
				{
					array[index] = textureIndices;
				}
			}
			return array;
		}

		public Vector4I[] CreateTextureIndices(List<MyRuntimeSectionInfo> sectionInfos, List<int> indices, int verticesNum)
		{
			Vector4I vector4I = new Vector4I(-1, -1, -1, -1);
			Vector4I[] array = new Vector4I[verticesNum];
			for (int i = 0; i < verticesNum; i++)
			{
				array[i] = vector4I;
			}
			if (!MyRender11.Settings.UseGeometryArrayTextures)
			{
				return array;
			}
			foreach (MyRuntimeSectionInfo sectionInfo in sectionInfos)
			{
				MyMeshMaterialId materialId = MyMeshMaterials1.GetMaterialId(sectionInfo.MaterialName);
				if (materialId.Info.GeometryTextureRef.IsUsed)
				{
					Vector4I textureSliceIndices = materialId.Info.GeometryTextureRef.TextureSliceIndices;
					for (int j = 0; j < sectionInfo.TriCount * 3; j++)
					{
						int num = indices[j + sectionInfo.IndexStart];
						_ = ref array[num];
						array[num] = textureSliceIndices;
					}
				}
			}
			return array;
		}
	}
}
