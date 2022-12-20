using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpDX.Direct3D11;
using VRage.FileSystem;
using VRage.Import;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRage.Render11.Scene.Components;
using VRage.Utils;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Import;
using VRageRender.Vertex;

namespace VRageRender
{
	internal class MyAssetMesh : MyMesh
	{
		internal MyAssetMesh(string assetName)
		{
			m_name = assetName;
		}

		private MyRenderMeshInfo LoadMesh(string assetName)
		{
			MyLODDescriptor[] LodDescriptors;
			return LoadMesh(assetName, out LodDescriptors);
		}

		private unsafe MyRenderMeshInfo LoadMesh(string assetName, out MyLODDescriptor[] LodDescriptors)
		{
			if (!assetName.EndsWith(".mwm"))
			{
				assetName += ".mwm";
			}
			MyVertexInputLayout myVertexInputLayout = MyVertexInputLayout.Empty;
			LodDescriptors = null;
			MyRenderMeshInfo myRenderMeshInfo = new MyRenderMeshInfo();
			MyModelImporter myModelImporter = new MyModelImporter();
			string text = (Path.IsPathRooted(assetName) ? assetName : Path.Combine(MyFileSystem.ContentPath, assetName));
			if (!MyFileSystem.FileExists(text))
			{
				return MyAssetsLoader.GetDebugMesh().LODs[0].m_meshInfo;
			}
			string contentPath = null;
			if (Path.IsPathRooted(assetName) && assetName.ToLower().Contains("models"))
			{
				contentPath = assetName.Substring(0, assetName.ToLower().IndexOf("models"));
			}
			try
			{
				myModelImporter.ImportData(text, new string[12]
				{
					"Vertices", "BlendIndices", "BlendWeights", "Normals", "TexCoords0", "Tangents", "Binormals", "Bones", "MeshParts", "BoundingBox",
					"BoundingSphere", "LODs"
				});
				Dictionary<string, object> tagData = myModelImporter.GetTagData();
				HalfVector4[] array = (HalfVector4[])tagData["Vertices"];
				int num = array.Length;
				Vector4I[] array2 = (Vector4I[])tagData["BlendIndices"];
				Vector4[] array3 = (Vector4[])tagData["BlendWeights"];
				Byte4[] array4 = (Byte4[])tagData["Normals"];
				HalfVector2[] array5 = (HalfVector2[])tagData["TexCoords0"];
				Byte4[] array6 = (Byte4[])tagData["Tangents"];
				Byte4[] array7 = (Byte4[])tagData["Binormals"];
				Byte4[] array8 = new Byte4[num];
				for (int i = 0; i < num; i++)
				{
					Vector3 v = VF_Packer.UnpackNormal(array4[i].PackedValue);
					Vector3 vector = VF_Packer.UnpackNormal(array6[i].PackedValue);
					Vector3 v2 = VF_Packer.UnpackNormal(array7[i].PackedValue);
					Vector4 tangentW = new Vector4(vector.X, vector.Y, vector.Z, 0f);
					tangentW.W = ((!(vector.Cross(v).Dot(v2) < 0f)) ? 1 : (-1));
					array8[i] = VF_Packer.PackTangentSignB4(ref tangentW);
				}
				bool flag = array2.Length != 0 && array3.Length != 0;
				MyModelBone[] array9 = (MyModelBone[])tagData["Bones"];
				List<IVertexBuffer> list = new List<IVertexBuffer>();
				IIndexBuffer iB = null;
				Dictionary<MyMeshDrawTechnique, List<MyDrawSubmesh>> dictionary = new Dictionary<MyMeshDrawTechnique, List<MyDrawSubmesh>>();
				Dictionary<MyMeshDrawTechnique, List<MySubmeshInfo>> dictionary2 = new Dictionary<MyMeshDrawTechnique, List<MySubmeshInfo>>();
				List<MySubmeshInfo> list2 = new List<MySubmeshInfo>();
				int indicesNum = 0;
				bool flag2 = false;
				if (tagData.ContainsKey("MeshParts"))
				{
					List<uint> list3 = new List<uint>(array.Length);
					uint num2 = 0u;
					foreach (MyMeshPartInfo item in tagData["MeshParts"] as List<MyMeshPartInfo>)
					{
						int[] bonesMapping = null;
						if (array2.Length != 0 && array9.Length > 60)
						{
							Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
							Dictionary<int, int> dictionary4 = new Dictionary<int, int>();
							int num3 = item.m_indices.Count / 3;
							for (int j = 0; j < num3; j++)
							{
								for (int k = 0; k < 3; k++)
								{
									int num4 = item.m_indices[j * 3 + k];
									if (array3[num4].X > 0f)
									{
										dictionary4[array2[num4].X] = 1;
									}
									if (array3[num4].Y > 0f)
									{
										dictionary4[array2[num4].Y] = 1;
									}
									if (array3[num4].Z > 0f)
									{
										dictionary4[array2[num4].Z] = 1;
									}
									if (array3[num4].W > 0f)
									{
										dictionary4[array2[num4].W] = 1;
									}
								}
							}
							_ = dictionary4.Count;
							_ = 60;
							List<int> list4 = new List<int>(dictionary4.Keys);
							list4.Sort();
							if (list4.Count > 0 && list4[list4.Count - 1] >= 60)
							{
								for (int l = 0; l < list4.Count; l++)
								{
									dictionary4[list4[l]] = l;
								}
								Dictionary<int, int> dictionary5 = new Dictionary<int, int>();
								for (int m = 0; m < num3; m++)
								{
									for (int n = 0; n < 3; n++)
									{
										int num5 = item.m_indices[m * 3 + n];
										if (!dictionary5.ContainsKey(num5))
										{
											if (array3[num5].X > 0f)
											{
												array2[num5].X = dictionary4[array2[num5].X];
											}
											if (array3[num5].Y > 0f)
											{
												array2[num5].Y = dictionary4[array2[num5].Y];
											}
											if (array3[num5].Z > 0f)
											{
												array2[num5].Z = dictionary4[array2[num5].Z];
											}
											if (array3[num5].W > 0f)
											{
												array2[num5].W = dictionary4[array2[num5].W];
											}
											dictionary5[num5] = 1;
											int value = 0;
											dictionary3.TryGetValue(num5, out value);
											dictionary3[num5] = value + 1;
										}
									}
								}
								bonesMapping = list4.ToArray();
							}
							_ = dictionary3.Values.Count;
							_ = 0;
						}
						int count = list3.Count;
						int count2 = item.m_indices.Count;
						uint num6 = (uint)item.m_indices[0];
						foreach (int index in item.m_indices)
						{
							list3.Add((uint)index);
							num6 = Math.Min(num6, (uint)index);
						}
						uint baseVertex = num6;
						for (int num7 = count; num7 < count + count2; num7++)
						{
							list3[num7] -= num6;
							num2 = Math.Max(num2, list3[num7]);
						}
						MyMeshMaterialId materialId = MyMeshMaterials1.GetMaterialId(item.m_MaterialDesc, contentPath);
						MyMeshDrawTechnique technique = MyMeshMaterials1.Table[materialId.Index].Technique;
						MyStringId name = MyMeshMaterials1.Table[materialId.Index].Name;
						dictionary.SetDefault(technique, new List<MyDrawSubmesh>()).Add(new MyDrawSubmesh(count2, count, (int)baseVertex, MyMeshMaterials1.GetProxyId(materialId), bonesMapping));
						list2.Add(new MySubmeshInfo
						{
							IndexCount = count2,
							StartIndex = count,
							BaseVertex = (int)baseVertex,
							BonesMapping = bonesMapping,
							Material = name.ToString(),
							Technique = technique
						});
						dictionary2.SetDefault(technique, new List<MySubmeshInfo>()).Add(list2[list2.Count - 1]);
					}
					indicesNum = list3.Count;
					if (num2 <= 65535)
					{
						ushort[] array10 = new ushort[list3.Count];
						for (int num8 = 0; num8 < list3.Count; num8++)
						{
							array10[num8] = (ushort)list3[num8];
						}
						myRenderMeshInfo.Indices = array10;
						try
						{
							fixed (ushort* value2 = array10)
							{
								iB = MyManagers.Buffers.CreateIndexBuffer(assetName + " index buffer", array10.Length, new IntPtr(value2), MyIndexBufferFormat.UShort, ResourceUsage.Immutable);
							}
						}
						finally
						{
						}
					}
					else
					{
						uint[] array11 = list3.ToArray();
						try
						{
							fixed (uint* value3 = array11)
							{
								iB = MyManagers.Buffers.CreateIndexBuffer(assetName + " index buffer", list3.Count, new IntPtr(value3), MyIndexBufferFormat.UInt, ResourceUsage.Immutable);
							}
						}
						finally
						{
						}
					}
					if (!flag)
					{
						MyVertexFormatPositionH4[] array12 = new MyVertexFormatPositionH4[num];
						for (int num9 = 0; num9 < num; num9++)
						{
							array12[num9] = new MyVertexFormatPositionH4(array[num9]);
						}
						myVertexInputLayout = myVertexInputLayout.Append(MyVertexInputComponentType.POSITION_PACKED);
						myRenderMeshInfo.VertexPositions = array12;
						try
						{
							fixed (MyVertexFormatPositionH4* value4 = array12)
							{
								list.Add(MyManagers.Buffers.CreateVertexBuffer(assetName + " vertex buffer " + list.Count, num, sizeof(MyVertexFormatPositionH4), new IntPtr(value4), ResourceUsage.Immutable));
							}
						}
						finally
						{
						}
					}
					else
					{
						MyVertexFormatPositionSkinning[] array13 = new MyVertexFormatPositionSkinning[num];
						for (int num10 = 0; num10 < num; num10++)
						{
							array13[num10] = new MyVertexFormatPositionSkinning(array[num10], new Byte4(array2[num10].X, array2[num10].Y, array2[num10].Z, array2[num10].W), array3[num10]);
						}
						myVertexInputLayout = myVertexInputLayout.Append(MyVertexInputComponentType.POSITION_PACKED).Append(MyVertexInputComponentType.BLEND_WEIGHTS).Append(MyVertexInputComponentType.BLEND_INDICES);
						try
						{
							fixed (MyVertexFormatPositionSkinning* value5 = array13)
							{
								list.Add(MyManagers.Buffers.CreateVertexBuffer(assetName + " vertex buffer " + list.Count, num, sizeof(MyVertexFormatPositionSkinning), new IntPtr(value5), ResourceUsage.Immutable));
							}
						}
						finally
						{
						}
					}
					MyVertexFormatTexcoordNormalTangent[] array14 = new MyVertexFormatTexcoordNormalTangent[num];
					for (int num11 = 0; num11 < num; num11++)
					{
						array14[num11] = new MyVertexFormatTexcoordNormalTangent(array5[num11], array4[num11], array8[num11]);
					}
					try
					{
						fixed (MyVertexFormatTexcoordNormalTangent* value6 = array14)
						{
							list.Add(MyManagers.Buffers.CreateVertexBuffer(assetName + " vertex buffer " + list.Count, num, sizeof(MyVertexFormatTexcoordNormalTangent), new IntPtr(value6), ResourceUsage.Immutable));
						}
					}
					finally
					{
					}
					myRenderMeshInfo.VertexExtendedData = array14;
					myVertexInputLayout = myVertexInputLayout.Append(MyVertexInputComponentType.NORMAL, 1).Append(MyVertexInputComponentType.TANGENT_SIGN_OF_BITANGENT, 1).Append(MyVertexInputComponentType.TEXCOORD0_H, 1);
				}
				if (tagData.ContainsKey("LODs"))
				{
					object obj = tagData["LODs"];
					_ = ((MyLODDescriptor[])obj).LongLength;
					LodDescriptors = (MyLODDescriptor[])((MyLODDescriptor[])obj).Clone();
				}
				myRenderMeshInfo.BoundingBox = (BoundingBox)tagData["BoundingBox"];
				myRenderMeshInfo.BoundingSphere = (BoundingSphere)tagData["BoundingSphere"];
				myRenderMeshInfo.VerticesNum = num;
				myRenderMeshInfo.IndicesNum = indicesNum;
				myRenderMeshInfo.VertexLayout = myVertexInputLayout;
				myRenderMeshInfo.IB = iB;
				myRenderMeshInfo.VB = list.ToArray();
				myRenderMeshInfo.IsAnimated = flag;
<<<<<<< HEAD
				myRenderMeshInfo.Parts = dictionary.ToDictionary((KeyValuePair<MyMeshDrawTechnique, List<MyDrawSubmesh>> x) => x.Key, (KeyValuePair<MyMeshDrawTechnique, List<MyDrawSubmesh>> x) => x.Value.ToArray());
				myRenderMeshInfo.PartsMetadata = dictionary2.ToDictionary((KeyValuePair<MyMeshDrawTechnique, List<MySubmeshInfo>> x) => x.Key, (KeyValuePair<MyMeshDrawTechnique, List<MySubmeshInfo>> x) => x.Value.ToArray());
=======
				myRenderMeshInfo.Parts = Enumerable.ToDictionary<KeyValuePair<MyMeshDrawTechnique, List<MyDrawSubmesh>>, MyMeshDrawTechnique, MyDrawSubmesh[]>((IEnumerable<KeyValuePair<MyMeshDrawTechnique, List<MyDrawSubmesh>>>)dictionary, (Func<KeyValuePair<MyMeshDrawTechnique, List<MyDrawSubmesh>>, MyMeshDrawTechnique>)((KeyValuePair<MyMeshDrawTechnique, List<MyDrawSubmesh>> x) => x.Key), (Func<KeyValuePair<MyMeshDrawTechnique, List<MyDrawSubmesh>>, MyDrawSubmesh[]>)((KeyValuePair<MyMeshDrawTechnique, List<MyDrawSubmesh>> x) => x.Value.ToArray()));
				myRenderMeshInfo.PartsMetadata = Enumerable.ToDictionary<KeyValuePair<MyMeshDrawTechnique, List<MySubmeshInfo>>, MyMeshDrawTechnique, MySubmeshInfo[]>((IEnumerable<KeyValuePair<MyMeshDrawTechnique, List<MySubmeshInfo>>>)dictionary2, (Func<KeyValuePair<MyMeshDrawTechnique, List<MySubmeshInfo>>, MyMeshDrawTechnique>)((KeyValuePair<MyMeshDrawTechnique, List<MySubmeshInfo>> x) => x.Key), (Func<KeyValuePair<MyMeshDrawTechnique, List<MySubmeshInfo>>, MySubmeshInfo[]>)((KeyValuePair<MyMeshDrawTechnique, List<MySubmeshInfo>> x) => x.Value.ToArray()));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myRenderMeshInfo.m_submeshes = list2;
				base.IsAnimated |= myRenderMeshInfo.IsAnimated;
				myModelImporter.Clear();
				return myRenderMeshInfo;
			}
			catch (Exception)
			{
				return MyAssetsLoader.GetDebugMesh().LODs[0].m_meshInfo;
			}
		}

		internal void SetMaterial_SLOW(MyMaterialProxyId materialId)
		{
			MyRenderLodInfo[] lODs = LODs;
			foreach (MyRenderLodInfo myRenderLodInfo in lODs)
			{
				if (myRenderLodInfo.m_meshInfo == null)
				{
					continue;
				}
				foreach (KeyValuePair<MyMeshDrawTechnique, MyDrawSubmesh[]> part in myRenderLodInfo.m_meshInfo.Parts)
				{
					for (int j = 0; j < part.Value.Length; j++)
					{
						MyDrawSubmesh myDrawSubmesh = part.Value[j];
						myDrawSubmesh.MaterialId = materialId;
						part.Value[j] = myDrawSubmesh;
					}
				}
			}
			MyRenderableComponent.MarkAllDirty();
		}

		internal static void LoadMaterials(string assetName)
		{
			if (!assetName.EndsWith(".mwm"))
			{
				assetName += ".mwm";
			}
			string contentPath = null;
			if (Path.IsPathRooted(assetName) && assetName.ToLower().Contains("models"))
			{
				contentPath = assetName.Substring(0, assetName.ToLower().IndexOf("models"));
			}
			new MyRenderMeshInfo();
			MyModelImporter myModelImporter = new MyModelImporter();
			string assetFileName = (Path.IsPathRooted(assetName) ? assetName : Path.Combine(MyFileSystem.ContentPath, assetName));
			myModelImporter.ImportData(assetFileName, new string[1] { "MeshParts" });
			Dictionary<string, object> tagData = myModelImporter.GetTagData();
			if (tagData.ContainsKey("MeshParts"))
			{
				foreach (MyMeshPartInfo item in tagData["MeshParts"] as List<MyMeshPartInfo>)
				{
					MyMeshMaterials1.GetMaterialId(item.m_MaterialDesc, contentPath);
				}
			}
			myModelImporter.Clear();
		}

		internal void LoadAsset()
		{
			LODs = null;
			base.IsAnimated = false;
			m_loadingStatus = MyAssetLoadingEnum.Waiting;
			MyRenderLodInfo myRenderLodInfo = new MyRenderLodInfo();
			myRenderLodInfo.LodNum = 0;
			myRenderLodInfo.m_meshInfo = LoadMesh(m_name, out var LodDescriptors);
			myRenderLodInfo.Distance = 0f;
			int num = 1;
			if (LodDescriptors != null)
			{
				num += LodDescriptors.Length;
			}
			LODs = new MyRenderLodInfo[num];
			LODs[0] = myRenderLodInfo;
			if (LodDescriptors != null)
			{
				int num2 = 1;
				MyLODDescriptor[] array = LodDescriptors;
				foreach (MyLODDescriptor myLODDescriptor in array)
				{
					MyRenderLodInfo myRenderLodInfo2 = new MyRenderLodInfo();
					myRenderLodInfo2.Distance = myLODDescriptor.Distance;
					myRenderLodInfo2.LodNum = num2;
					myRenderLodInfo2.m_meshInfo = LoadMesh(myLODDescriptor.GetModelAbsoluteFilePath(m_name));
					LODs[num2] = myRenderLodInfo2;
					num2++;
				}
			}
			m_loadingStatus = MyAssetLoadingEnum.Ready;
		}
	}
}
