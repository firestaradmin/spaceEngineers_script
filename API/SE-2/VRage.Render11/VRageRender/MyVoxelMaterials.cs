using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SharpDX.DXGI;
using VRage;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;

namespace VRageRender
{
	internal static class MyVoxelMaterials
	{
		private static readonly Dictionary<int, MyMaterialProxyId> m_materialProxyIndex = new Dictionary<int, MyMaterialProxyId>();

		internal static MyVoxelMaterial[] Table = new MyVoxelMaterial[0];

		private static MyVoxelMaterialTiles[] m_materialTiles = new MyVoxelMaterialTiles[0];

		private static bool m_updateProxies;

		private static HashSet<int> m_dirtyMaterials = new HashSet<int>();

		private static string[] CreateStringArray(HashSet<string> set, string str1, string str2, string str3)
		{
			string[] result = new string[3] { str1, str2, str3 };
			set.Add(str1);
			set.Add(str2);
			set.Add(str3);
			return result;
		}

		internal static void Set(MyRenderVoxelMaterialData[] list, bool update = false)
		{
			bool flag = Table == null;
<<<<<<< HEAD
			HashSet<string> hashSet = new HashSet<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			HashSet<string> hashSet3 = new HashSet<string>();
=======
			HashSet<string> val = new HashSet<string>();
			HashSet<string> val2 = new HashSet<string>();
			HashSet<string> val3 = new HashSet<string>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (Table == null)
			{
				Array.Resize(ref Table, list.Length);
			}
			MyFileArrayTextureManager fileArrayTextures = MyManagers.FileArrayTextures;
			for (int i = 0; i < list.Length; i++)
			{
				int num = (update ? list[i].Index : i);
				if (num >= Table.Length)
				{
					Array.Resize(ref Table, num + list.Length);
				}
				MyRenderVoxelMaterialData.TextureSet textureSet = list[i].TextureSets[0];
				MyRenderVoxelMaterialData.TextureSet textureSet2 = list[i].TextureSets[1];
				MyRenderVoxelMaterialData.TextureSet textureSet3 = list[i].TextureSets[2];
<<<<<<< HEAD
				Table[num].Resource.ColorMetalXZnY_Filepaths = CreateStringArray(hashSet, textureSet.ColorMetalXZnY, textureSet2.ColorMetalXZnY, textureSet3.ColorMetalXZnY);
				Table[num].Resource.ColorMetalY_Filepaths = CreateStringArray(hashSet, textureSet.ColorMetalY, textureSet2.ColorMetalY, textureSet3.ColorMetalY);
				Table[num].Resource.NormalGlossXZnY_Filepaths = CreateStringArray(hashSet2, textureSet.NormalGlossXZnY, textureSet2.NormalGlossXZnY, textureSet3.NormalGlossXZnY);
				Table[num].Resource.NormalGlossY_Filepaths = CreateStringArray(hashSet2, textureSet.NormalGlossY, textureSet2.NormalGlossY, textureSet3.NormalGlossY);
				Table[num].Resource.ExtXZnY_Filepaths = CreateStringArray(hashSet3, textureSet.ExtXZnY, textureSet2.ExtXZnY, textureSet3.ExtXZnY);
				Table[num].Resource.ExtY_Filepaths = CreateStringArray(hashSet3, textureSet.ExtY, textureSet2.ExtY, textureSet3.ExtY);
=======
				Table[num].Resource.ColorMetalXZnY_Filepaths = CreateStringArray(val, textureSet.ColorMetalXZnY, textureSet2.ColorMetalXZnY, textureSet3.ColorMetalXZnY);
				Table[num].Resource.ColorMetalY_Filepaths = CreateStringArray(val, textureSet.ColorMetalY, textureSet2.ColorMetalY, textureSet3.ColorMetalY);
				Table[num].Resource.NormalGlossXZnY_Filepaths = CreateStringArray(val2, textureSet.NormalGlossXZnY, textureSet2.NormalGlossXZnY, textureSet3.NormalGlossXZnY);
				Table[num].Resource.NormalGlossY_Filepaths = CreateStringArray(val2, textureSet.NormalGlossY, textureSet2.NormalGlossY, textureSet3.NormalGlossY);
				Table[num].Resource.ExtXZnY_Filepaths = CreateStringArray(val3, textureSet.ExtXZnY, textureSet2.ExtXZnY, textureSet3.ExtXZnY);
				Table[num].Resource.ExtY_Filepaths = CreateStringArray(val3, textureSet.ExtY, textureSet2.ExtY, textureSet3.ExtY);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				UpdateFoliage(list[i].Foliage, ref Table[i], fileArrayTextures);
				Table[num].SimpleTilingSetup = list[i].SimpleTilingSetup;
				Table[num].StandardTilingSetup = list[i].StandardTilingSetup;
				Table[num].SimpleTilingSetup.TilingScale = 1f / Table[num].SimpleTilingSetup.TilingScale;
				Table[num].StandardTilingSetup.TilingScale = 1f / Table[num].StandardTilingSetup.TilingScale;
				Table[num].Far3Color = list[i].Far3Color;
			}
			if (flag)
			{
<<<<<<< HEAD
				MyManagers.GlobalResources.CreateOnStartup(hashSet.Count, hashSet2.Count, hashSet3.Count);
=======
				MyManagers.GlobalResources.CreateOnStartup(val.get_Count(), val2.get_Count(), val3.get_Count());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			for (int j = 0; j < list.Length; j++)
			{
				int num2 = (update ? list[j].Index : j);
				if (num2 >= m_materialTiles.Length)
				{
					Array.Resize(ref m_materialTiles, num2 + list.Length);
				}
<<<<<<< HEAD
				if (m_materialTiles[num2] != null)
				{
					ResetTilesEvent(m_materialTiles[num2].ColorMetalXZnY);
					ResetTilesEvent(m_materialTiles[num2].ColorMetalY);
					ResetTilesEvent(m_materialTiles[num2].NormalGlossXZnY);
					ResetTilesEvent(m_materialTiles[num2].NormalGlossY);
					ResetTilesEvent(m_materialTiles[num2].ExtXZnY);
					ResetTilesEvent(m_materialTiles[num2].ExtY);
				}
				m_materialTiles[num2] = PrepareMaterialTiles(num2);
			}
			MyRender11.EnqueueUpdate(FeedGPU);
			void ResetTilesEvent(IMyStreamedTextureArrayTile[] tiles)
			{
				if (tiles != null && tiles.Length != 0)
				{
					for (int k = 0; k < tiles.Length; k++)
					{
						tiles[k].ResetOnTextureTileHandleChangedEvent();
					}
				}
			}
=======
				m_materialTiles[num2] = PrepareMaterialTiles(num2);
			}
			MyRender11.EnqueueUpdate(FeedGPU);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static MyVoxelMaterialTiles PrepareMaterialTiles(int materialID)
		{
			MyVoxelMaterialTiles myVoxelMaterialTiles = new MyVoxelMaterialTiles();
			IDynamicFileArrayTexture fileArrayTextureVoxelCM = MyGlobalResources.FileArrayTextureVoxelCM;
			IDynamicFileArrayTexture fileArrayTextureVoxelNG = MyGlobalResources.FileArrayTextureVoxelNG;
			IDynamicFileArrayTexture fileArrayTextureVoxelExt = MyGlobalResources.FileArrayTextureVoxelExt;
			IDynamicFileArrayTexture fileArrayTextureVoxelCMLow = MyGlobalResources.FileArrayTextureVoxelCMLow;
			IDynamicFileArrayTexture fileArrayTextureVoxelNGLow = MyGlobalResources.FileArrayTextureVoxelNGLow;
			IDynamicFileArrayTexture fileArrayTextureVoxelExtLow = MyGlobalResources.FileArrayTextureVoxelExtLow;
			MyVoxelMaterial myVoxelMaterial = Table[materialID];
			myVoxelMaterialTiles.ColorMetalXZnY = new IMyStreamedTextureArrayTile[3]
			{
				PrepareTile(fileArrayTextureVoxelCM, fileArrayTextureVoxelCMLow, myVoxelMaterial.Resource.ColorMetalXZnY_Filepaths[0]),
				PrepareTile(fileArrayTextureVoxelCM, fileArrayTextureVoxelCMLow, myVoxelMaterial.Resource.ColorMetalXZnY_Filepaths[1]),
				PrepareTile(fileArrayTextureVoxelCM, fileArrayTextureVoxelCMLow, myVoxelMaterial.Resource.ColorMetalXZnY_Filepaths[2])
			};
			myVoxelMaterialTiles.ColorMetalY = new IMyStreamedTextureArrayTile[3]
			{
				PrepareTile(fileArrayTextureVoxelCM, fileArrayTextureVoxelCMLow, myVoxelMaterial.Resource.ColorMetalY_Filepaths[0]),
				PrepareTile(fileArrayTextureVoxelCM, fileArrayTextureVoxelCMLow, myVoxelMaterial.Resource.ColorMetalY_Filepaths[1]),
				PrepareTile(fileArrayTextureVoxelCM, fileArrayTextureVoxelCMLow, myVoxelMaterial.Resource.ColorMetalY_Filepaths[2])
			};
			myVoxelMaterialTiles.NormalGlossXZnY = new IMyStreamedTextureArrayTile[3]
			{
				PrepareTile(fileArrayTextureVoxelNG, fileArrayTextureVoxelNGLow, myVoxelMaterial.Resource.NormalGlossXZnY_Filepaths[0]),
				PrepareTile(fileArrayTextureVoxelNG, fileArrayTextureVoxelNGLow, myVoxelMaterial.Resource.NormalGlossXZnY_Filepaths[1]),
				PrepareTile(fileArrayTextureVoxelNG, fileArrayTextureVoxelNGLow, myVoxelMaterial.Resource.NormalGlossXZnY_Filepaths[2])
			};
			myVoxelMaterialTiles.NormalGlossY = new IMyStreamedTextureArrayTile[3]
			{
				PrepareTile(fileArrayTextureVoxelNG, fileArrayTextureVoxelNGLow, myVoxelMaterial.Resource.NormalGlossY_Filepaths[0]),
				PrepareTile(fileArrayTextureVoxelNG, fileArrayTextureVoxelNGLow, myVoxelMaterial.Resource.NormalGlossY_Filepaths[1]),
				PrepareTile(fileArrayTextureVoxelNG, fileArrayTextureVoxelNGLow, myVoxelMaterial.Resource.NormalGlossY_Filepaths[2])
			};
			myVoxelMaterialTiles.ExtXZnY = new IMyStreamedTextureArrayTile[3]
			{
				PrepareTile(fileArrayTextureVoxelExt, fileArrayTextureVoxelExtLow, myVoxelMaterial.Resource.ExtXZnY_Filepaths[0]),
				PrepareTile(fileArrayTextureVoxelExt, fileArrayTextureVoxelExtLow, myVoxelMaterial.Resource.ExtXZnY_Filepaths[1]),
				PrepareTile(fileArrayTextureVoxelExt, fileArrayTextureVoxelExtLow, myVoxelMaterial.Resource.ExtXZnY_Filepaths[2])
			};
			myVoxelMaterialTiles.ExtY = new IMyStreamedTextureArrayTile[3]
			{
				PrepareTile(fileArrayTextureVoxelExt, fileArrayTextureVoxelExtLow, myVoxelMaterial.Resource.ExtY_Filepaths[0]),
				PrepareTile(fileArrayTextureVoxelExt, fileArrayTextureVoxelExtLow, myVoxelMaterial.Resource.ExtY_Filepaths[1]),
				PrepareTile(fileArrayTextureVoxelExt, fileArrayTextureVoxelExtLow, myVoxelMaterial.Resource.ExtY_Filepaths[2])
			};
			m_dirtyMaterials.Add(materialID);
			return myVoxelMaterialTiles;
			void InvalidateTile(IMyStreamedTextureArrayTile tile)
			{
				m_dirtyMaterials.Add(materialID);
			}
			IMyStreamedTextureArrayTile PrepareTile(IDynamicFileArrayTexture arrayTexture, IDynamicFileArrayTexture low, string path)
			{
				IMyStreamedTextureArrayTile textureArrayTile = MyManagers.Textures.GetTextureArrayTile(arrayTexture, path);
				textureArrayTile.OnTextureTileHandleChanged += InvalidateTile;
				low.GetOrAddSlice(path);
				return textureArrayTile;
			}
		}

		private static void FeedGPU()
		{
			MaterialFoliageData data = default(MaterialFoliageData);
			MyMapping myMapping = MyMapping.MapDiscard(MyCommon.MaterialFoliageTableConstants);
			for (int i = 0; i < Table.Length; i++)
			{
				if (Table[i].Foliage != null)
				{
					myMapping.WriteAndPosition(ref Table[i].Foliage.BoxedValue.Data);
				}
				else
				{
					myMapping.WriteAndPosition(ref data);
				}
			}
			myMapping.Unmap();
			m_updateProxies = true;
		}

		private static void UpdateFoliage(MyRenderFoliageData? data, ref MyVoxelMaterial material, MyFileArrayTextureManager arrayManager)
		{
			if (!data.HasValue || data.Value.Entries.Length == 0)
			{
				ClearFoliage(ref material);
				return;
			}
			MyRenderFoliageData value = data.Value;
			Boxed<MaterialFoliage> boxed = new Boxed<MaterialFoliage>(default(MaterialFoliage))
			{
				BoxedValue = 
				{
					Type = value.Type,
					Density = value.Density
				}
			};
			List<string> list = new List<string>(value.Entries.Length);
			List<string> list2 = new List<string>(value.Entries.Length);
			for (int i = 0; i < value.Entries.Length; i++)
			{
				list.Add(value.Entries[i].ColorAlphaTexture);
				list2.Add(value.Entries[i].NormalGlossTexture);
			}
<<<<<<< HEAD
			if (!material.HasFoliage || material.Foliage.BoxedValue.ColorTextureArray == null || material.Foliage.BoxedValue.NormalTextureArray == null || !material.Foliage.BoxedValue.ColorTextureArray.SubTextures.SequenceEqual(list) || !material.Foliage.BoxedValue.NormalTextureArray.SubTextures.SequenceEqual(list2))
=======
			if (!material.HasFoliage || material.Foliage.BoxedValue.ColorTextureArray == null || material.Foliage.BoxedValue.NormalTextureArray == null || !Enumerable.SequenceEqual<string>(material.Foliage.BoxedValue.ColorTextureArray.SubTextures, (IEnumerable<string>)list) || !Enumerable.SequenceEqual<string>(material.Foliage.BoxedValue.NormalTextureArray.SubTextures, (IEnumerable<string>)list2))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				ClearFoliage(ref material);
				boxed.BoxedValue.ColorTextureArray = arrayManager.InitFromFilesAsync("MyVoxelMaterial1.FoliageColorTextureArray", list.ToArray(), MyFileTextureEnum.COLOR_METAL, MyGeneratedTexturePatterns.ColorMetal_BC7_SRgb, Format.BC7_UNorm_SRgb, 0);
				boxed.BoxedValue.NormalTextureArray = arrayManager.InitFromFilesAsync("MyVoxelMaterial1.FoliageNormalTextureArray", list2.ToArray(), MyFileTextureEnum.NORMALMAP_GLOSS, MyGeneratedTexturePatterns.NormalGloss_BC7, Format.BC7_UNorm, 0);
			}
			else
			{
				boxed.BoxedValue.ColorTextureArray = material.Foliage.BoxedValue.ColorTextureArray;
				boxed.BoxedValue.NormalTextureArray = material.Foliage.BoxedValue.NormalTextureArray;
			}
			boxed.BoxedValue.Data = new MaterialFoliageData
			{
				Scale = value.Entries[0].Size,
				ScaleVar = value.Entries[0].SizeVariation,
				TexturesNum = (uint)value.Entries.Length
			};
			material.Foliage = boxed;
			if (material.Foliage.BoxedValue.ColorTextureArray == null)
			{
				ClearFoliage(ref material);
			}
		}

		private static void ClearFoliage(ref MyVoxelMaterial material)
		{
			if (material.Foliage != null)
			{
				MaterialFoliage boxedValue = material.Foliage.BoxedValue;
				if (boxedValue.ColorTextureArray != null)
				{
					MyManagers.FileArrayTextures.DisposeTex(ref boxedValue.ColorTextureArray);
				}
				if (boxedValue.NormalTextureArray != null)
				{
					MyManagers.FileArrayTextures.DisposeTex(ref boxedValue.NormalTextureArray);
				}
			}
			material.Foliage = null;
		}

		internal static bool CheckIndices(MyRenderVoxelMaterialData[] list)
		{
			for (int i = 0; i < list.Length; i++)
			{
				if (list[i].Index != i)
				{
					return false;
				}
			}
			return true;
		}

		internal static MyMaterialProxyId GetMaterialProxyId(int materialIdx)
		{
			if (!m_materialProxyIndex.TryGetValue(materialIdx, out var value))
			{
				MyMaterialProxyId myMaterialProxyId2 = (m_materialProxyIndex[materialIdx] = MyMaterials1.AllocateProxy());
				value = myMaterialProxyId2;
				MyMaterials1.ProxyPool.Data[value.Index] = CreateProxyWithValidMaterialConstants(materialIdx);
			}
			return value;
		}

		private static void UpdateMaterialSlices(ref MyVoxelMaterialEntry entry, MyVoxelMaterialTiles tiles)
		{
			IDynamicFileArrayTexture fileArrayTextureVoxelCMLow = MyGlobalResources.FileArrayTextureVoxelCMLow;
			IDynamicFileArrayTexture fileArrayTextureVoxelNGLow = MyGlobalResources.FileArrayTextureVoxelNGLow;
			IDynamicFileArrayTexture fileArrayTextureVoxelExtLow = MyGlobalResources.FileArrayTextureVoxelExtLow;
			int num = 0;
			bool allLoaded2 = (entry.FullQuality = tiles.AreLoaded());
			entry.SliceNear1.X = GetTileID(tiles.ColorMetalXZnY[num], fileArrayTextureVoxelCMLow, allLoaded2);
			entry.SliceNear1.Y = GetTileID(tiles.ColorMetalY[num], fileArrayTextureVoxelCMLow, allLoaded2);
			entry.SliceNear1.Z = GetTileID(tiles.NormalGlossXZnY[num], fileArrayTextureVoxelNGLow, allLoaded2);
			entry.SliceNear1.W = GetTileID(tiles.NormalGlossY[num], fileArrayTextureVoxelNGLow, allLoaded2);
			entry.SliceNear2.X = GetTileID(tiles.ExtXZnY[num], fileArrayTextureVoxelExtLow, allLoaded2);
			entry.SliceNear2.Y = GetTileID(tiles.ExtY[num], fileArrayTextureVoxelExtLow, allLoaded2);
			num = 1;
			entry.SliceFar1.X = GetTileID(tiles.ColorMetalXZnY[num], fileArrayTextureVoxelCMLow, allLoaded2);
			entry.SliceFar1.Y = GetTileID(tiles.ColorMetalY[num], fileArrayTextureVoxelCMLow, allLoaded2);
			entry.SliceFar1.Z = GetTileID(tiles.NormalGlossXZnY[num], fileArrayTextureVoxelNGLow, allLoaded2);
			entry.SliceFar1.W = GetTileID(tiles.NormalGlossY[num], fileArrayTextureVoxelNGLow, allLoaded2);
			entry.SliceFar2.X = GetTileID(tiles.ExtXZnY[num], fileArrayTextureVoxelExtLow, allLoaded2);
			entry.SliceFar2.Y = GetTileID(tiles.ExtY[num], fileArrayTextureVoxelExtLow, allLoaded2);
			num = 2;
			entry.SliceFar21.X = GetTileID(tiles.ColorMetalXZnY[num], fileArrayTextureVoxelCMLow, allLoaded2);
			entry.SliceFar21.Y = GetTileID(tiles.ColorMetalY[num], fileArrayTextureVoxelCMLow, allLoaded2);
			entry.SliceFar21.Z = GetTileID(tiles.NormalGlossXZnY[num], fileArrayTextureVoxelNGLow, allLoaded2);
			entry.SliceFar21.W = GetTileID(tiles.NormalGlossY[num], fileArrayTextureVoxelNGLow, allLoaded2);
			entry.SliceFar22.X = GetTileID(tiles.ExtXZnY[num], fileArrayTextureVoxelExtLow, allLoaded2);
			entry.SliceFar22.Y = GetTileID(tiles.ExtY[num], fileArrayTextureVoxelExtLow, allLoaded2);
<<<<<<< HEAD
			int GetTileID(IMyStreamedTextureArrayTile tile, IDynamicFileArrayTexture lowTexArray, bool allLoaded)
=======
			static int GetTileID(IMyStreamedTextureArrayTile tile, IDynamicFileArrayTexture lowTexArray, bool allLoaded)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (!allLoaded)
				{
					return lowTexArray.GetOrAddSlice(tile.Filepath);
				}
				return tile.TileID;
			}
		}

		private static void FillMaterialEntry(ref MyVoxelMaterialEntry entry, ref MyVoxelMaterial voxelMaterial1, MyVoxelMaterialTiles tiles)
		{
			entry.SimpleTilingSetup = voxelMaterial1.SimpleTilingSetup;
			entry.StandardTilingSetup = voxelMaterial1.StandardTilingSetup;
			entry.Far3Color = voxelMaterial1.Far3Color;
			UpdateMaterialSlices(ref entry, tiles);
		}

		public static bool IsMaterialValid(int matId)
		{
			return matId < Table.Length;
		}

		public static bool TouchMaterial(int matId)
		{
			if (IsMaterialValid(matId))
			{
				foreach (IMyStreamedTextureArrayTile allTile in m_materialTiles[matId].GetAllTiles())
				{
					allTile.Touch(100);
				}
				return true;
			}
			return false;
		}

		private static void UpdateMaterial(int matId)
		{
			MyVoxelMaterialEntry entry = default(MyVoxelMaterialEntry);
			FillMaterialEntry(ref entry, ref Table[matId], m_materialTiles[matId]);
			MyCommon.VoxelMaterialsConstants.UpdateEntry(matId, ref entry);
		}

		private unsafe static MyMaterialProxy_2 CreateProxyWithValidMaterialConstants(int materialIdx)
		{
			if (materialIdx >= Table.Length)
			{
				materialIdx = 0;
			}
			int num = sizeof(MyVoxelMaterialConstants);
			MyVoxelMaterialConstants myVoxelMaterialConstants = default(MyVoxelMaterialConstants);
			byte[] array = new byte[num];
			fixed (byte* destination = array)
			{
				FillMaterialEntry(ref myVoxelMaterialConstants.entry, ref Table[materialIdx], m_materialTiles[materialIdx]);
				Unsafe.CopyBlockUnaligned(destination, &myVoxelMaterialConstants, (uint)num);
			}
			int hashCode = materialIdx.GetHashCode();
			IConstantBuffer cB;
			fixed (byte* ptr = array)
			{
				void* value = ptr;
				cB = MyManagers.Buffers.CreateConstantBuffer("CommonMaterialCB" + num, num, new IntPtr(value));
			}
			MyConstantsPack myConstantsPack = default(MyConstantsPack);
			myConstantsPack.BindFlag = MyBindFlag.BIND_PS;
			myConstantsPack.CB = cB;
			myConstantsPack.Version = hashCode;
			myConstantsPack.Data = array;
			MyConstantsPack materialConstants = myConstantsPack;
			MySrvTable mySrvTable = default(MySrvTable);
			mySrvTable.BindFlag = MyBindFlag.BIND_PS;
			mySrvTable.StartSlot = 0;
			mySrvTable.Version = hashCode;
			mySrvTable.Srvs = new ISrvBindable[6]
			{
				MyGlobalResources.FileArrayTextureVoxelCM,
				MyGlobalResources.FileArrayTextureVoxelNG,
				MyGlobalResources.FileArrayTextureVoxelExt,
				MyGlobalResources.FileArrayTextureVoxelCMLow,
				MyGlobalResources.FileArrayTextureVoxelNGLow,
				MyGlobalResources.FileArrayTextureVoxelExtLow
			};
			MySrvTable materialSrvs = mySrvTable;
			MyMaterialProxy_2 result = default(MyMaterialProxy_2);
			result.MaterialConstants = materialConstants;
			result.MaterialSrvs = materialSrvs;
			return result;
		}

		internal static void OnResourcesGather()
		{
<<<<<<< HEAD
			foreach (int dirtyMaterial in m_dirtyMaterials)
			{
				UpdateMaterial(dirtyMaterial);
				m_updateProxies = true;
=======
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<int> enumerator = m_dirtyMaterials.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					UpdateMaterial(enumerator.get_Current());
					m_updateProxies = true;
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_dirtyMaterials.Clear();
			if (!m_updateProxies)
			{
				return;
			}
			foreach (KeyValuePair<int, MyMaterialProxyId> item in m_materialProxyIndex)
			{
				MyMaterialProxyId value = item.Value;
				MyManagers.Buffers.Dispose(value.Info.MaterialConstants.CB);
				value.Info = CreateProxyWithValidMaterialConstants(item.Key);
				MyMaterials1.ProxyPool.Data[value.Index] = value.Info;
			}
			m_updateProxies = false;
		}

		internal static void Init()
		{
		}

		internal static void OnDeviceReset()
		{
			InvalidateMaterials();
		}

		internal static void InvalidateMaterials()
		{
			m_updateProxies = true;
		}

		internal static void OnSessionEnd()
		{
			foreach (MyMaterialProxyId value in m_materialProxyIndex.Values)
			{
				IConstantBuffer cB = value.Info.MaterialConstants.CB;
				if (cB != null)
				{
					MyManagers.Buffers.Dispose(cB);
				}
			}
			m_materialProxyIndex.Clear();
		}

		internal static void OnDeviceEnd()
		{
			OnSessionEnd();
			if (Table != null)
			{
				for (int i = 0; i < Table.Length; i++)
				{
					ClearFoliage(ref Table[i]);
				}
			}
			Table = null;
			m_materialProxyIndex.Clear();
		}

		public static void Preload(byte[] materials)
		{
		}

		public static IEnumerable<IMyStreamedTextureArrayTile> GetMaterialTiles(int materialId)
		{
			return m_materialTiles[materialId].GetAllTiles();
		}
	}
}
