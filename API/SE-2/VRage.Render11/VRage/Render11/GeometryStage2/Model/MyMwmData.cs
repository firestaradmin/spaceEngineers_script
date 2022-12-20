using System;
using System.Collections.Generic;
using System.Text;
using VRage.FileSystem;
using VRage.Import;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender;
using VRageRender.Import;

namespace VRage.Render11.GeometryStage2.Model
{
	internal struct MyMwmData
	{
		private class MyPartsComparer : IComparer<MyMeshPartInfo>
		{
			int IComparer<MyMeshPartInfo>.Compare(MyMeshPartInfo x, MyMeshPartInfo y)
			{
				int technique = (int)x.Technique;
				return technique.CompareTo((int)y.Technique);
			}
		}

		private static readonly MyPartsComparer m_partsComparer = new MyPartsComparer();

		private bool m_isSkinned;

		public List<int> Indices;

		public int LoadState;

		public string MwmFilepath { get; private set; }

		public string MwmContentPath { get; private set; }

		public MyLODDescriptor[] LodDescriptors { get; private set; }

		public List<MyMwmDataPart> Parts { get; private set; }

		public List<MyMeshSectionInfo> SectionInfos { get; private set; }

		public int VerticesCount => Positions.Length;

		public HalfVector4[] Positions { get; private set; }

		public Byte4[] Normals { get; private set; }

		public Byte4[] Tangents { get; private set; }

		public Byte4[] Bitangents { get; private set; }

		public HalfVector2[] Texcoords { get; private set; }

		public MyModelBone[] Bones { get; private set; }

		public Vector4I[] BoneIndices { get; private set; }

		public Vector4[] BoneWeights { get; private set; }

		public BoundingBox BoundindBox { get; private set; }

		public BoundingSphere BoundingSphere { get; private set; }

		public bool IsStub { get; private set; }

		public string GeometryDataPath { get; private set; }

		public bool IsSkinned
		{
			get
			{
				if ((!IsStub || !m_isSkinned) && BoneIndices.Length == 0 && BoneWeights.Length == 0)
				{
					return BoneIndices.Length == VerticesCount;
				}
				return true;
			}
		}

		public bool IsSkinnedProperly
		{
			get
			{
				if (BoneIndices.Length != 0 && BoneWeights.Length != 0 && BoneIndices.Length == VerticesCount)
				{
					return BoneWeights.Length == VerticesCount;
				}
				return false;
			}
		}

		public bool IsValid2ndStream
		{
			get
			{
				if (Normals.Length != 0 && Normals.Length == VerticesCount && Texcoords.Length != 0 && Texcoords.Length == VerticesCount && Tangents.Length != 0 && Tangents.Length == VerticesCount)
				{
					if (Bitangents.Length != 0)
					{
						return Bitangents.Length == VerticesCount;
					}
					return false;
				}
				return false;
			}
		}

		public bool ContainsTransparent
		{
			get
			{
				foreach (MyMwmDataPart part in Parts)
				{
					if (part.Technique.IsTransparent())
					{
						return true;
					}
				}
				return false;
			}
		}

		private static MyModelImporter GetModelImporter(string mwmFilepath)
		{
			MyModelImporter myModelImporter = new MyModelImporter();
			myModelImporter.ImportData(mwmFilepath, new string[16]
			{
				"Vertices", "BlendIndices", "BlendWeights", "Normals", "TexCoords0", "Tangents", "Binormals", "Bones", "MeshParts", "Sections",
				"BoundingBox", "BoundingSphere", "LODs", "PatternScale", "GeometryDataAsset", "IsSkinned"
			});
			return myModelImporter;
		}

		private Byte4[] CreateAlteredTangents(Byte4[] normals, Byte4[] tangents, Byte4[] bitangents)
		{
			int num = tangents.Length;
			Byte4[] array = new Byte4[num];
			if (tangents.Length != 0 && bitangents.Length != 0)
			{
				for (int i = 0; i < num; i++)
				{
					Vector3 v = VF_Packer.UnpackNormal(normals[i].PackedValue);
					Vector3 vector = VF_Packer.UnpackNormal(tangents[i].PackedValue);
					Vector3 v2 = VF_Packer.UnpackNormal(bitangents[i].PackedValue);
					Vector4 tangentW = new Vector4(vector.X, vector.Y, vector.Z, 0f);
					tangentW.W = ((!(vector.Cross(v).Dot(v2) < 0f)) ? 1 : (-1));
					array[i] = VF_Packer.PackTangentSignB4(ref tangentW);
				}
			}
			return array;
		}

		public bool LoadFromFile(string mwmFilepath)
		{
			LoadState = 0;
			MwmFilepath = MyMwmUtils.GetFullMwmFilepath(mwmFilepath);
			MwmContentPath = MyMwmUtils.GetFullMwmContentPath(mwmFilepath);
			GeometryDataPath = MwmFilepath;
			if (!MyFileSystem.FileExists(MwmFilepath))
			{
				MyRender11.Log.WriteLine($"Mesh asset {MwmFilepath} missing");
				return false;
			}
			LoadState = 1;
			MyModelImporter myModelImporter = null;
			try
			{
				myModelImporter = GetModelImporter(MwmFilepath);
			}
			catch (Exception ex)
			{
				string msg = $"Importing asset failed {MwmFilepath}, message: {ex.Message}, stack:{ex.StackTrace}";
				MyRender11.Log.WriteLine(msg);
				return false;
			}
			LoadState = 2;
			Dictionary<string, object> tagData = myModelImporter.GetTagData();
			if (tagData.TryGetValue("GeometryDataAsset", out var value))
			{
				GeometryDataPath = (string)value;
				IsStub = true;
			}
			if (tagData.TryGetValue("IsSkinned", out var value2))
			{
				m_isSkinned = (bool)value2;
			}
			if (tagData.ContainsKey("LODs"))
			{
				LodDescriptors = (MyLODDescriptor[])tagData["LODs"];
			}
			LoadState = 3;
			if (tagData.ContainsKey("Sections"))
			{
				SectionInfos = tagData["Sections"] as List<MyMeshSectionInfo>;
			}
			Bones = (MyModelBone[])tagData["Bones"];
			LoadState = 5;
			float num = 1f;
			if (tagData.TryGetValue("PatternScale", out var value3))
			{
				num = (float)value3;
			}
			BoundindBox = (BoundingBox)tagData["BoundingBox"];
			BoundingSphere = (BoundingSphere)tagData["BoundingSphere"];
			LoadState = 6;
			if (!IsStub)
			{
				Positions = (HalfVector4[])tagData["Vertices"];
				Normals = (Byte4[])tagData["Normals"];
				Tangents = (Byte4[])tagData["Tangents"];
				Bitangents = (Byte4[])tagData["Binormals"];
				Texcoords = (HalfVector2[])tagData["TexCoords0"];
				LoadState = 7;
				BoneIndices = (Vector4I[])tagData["BlendIndices"];
				BoneWeights = (Vector4[])tagData["BlendWeights"];
				if (num != 1f && Texcoords.Length != 0)
				{
					for (int i = 0; i < Texcoords.Length; i++)
					{
						Texcoords[i] = new HalfVector2(Texcoords[i].ToVector2() / num);
					}
				}
				LoadState = 8;
				if (Normals.Length != 0 && Tangents.Length != 0 && Bitangents.Length != 0)
				{
					Tangents = CreateAlteredTangents(Normals, Tangents, Bitangents);
				}
				LoadState = 9;
				List<MyMeshPartInfo> list = tagData["MeshParts"] as List<MyMeshPartInfo>;
				list.Sort(m_partsComparer);
				Parts = new List<MyMwmDataPart>();
				int num2 = 0;
				bool flag = false;
				foreach (MyMeshPartInfo item in list)
				{
					int count = item.m_indices.Count;
					MyMwmDataPart myMwmDataPart = new MyMwmDataPart(item, num2, count, MwmContentPath);
					Parts.Add(myMwmDataPart);
					flag |= !myMwmDataPart.Technique.IsTransparent() && myMwmDataPart.ColorMetalFilepath != null && myMwmDataPart.ColorMetalFilepath == "";
					num2 += count;
				}
				LoadState = 10;
				if (flag)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (MyMwmDataPart part in Parts)
					{
						if (part.ColorMetalFilepath != null && part.ColorMetalFilepath == "")
						{
							stringBuilder.Append(part.MaterialName);
							stringBuilder.Append(" (");
							if (part.ColorMetalFilepath == "")
							{
								stringBuilder.Append("ColorMetal ");
							}
							if (part.NormalGlossFilepath == "")
							{
								stringBuilder.Append("NormalGlossFilepath ");
							}
							if (part.ExtensionFilepath == "")
							{
								stringBuilder.Append("ExtensionFilepath ");
							}
							if (part.AlphamaskFilepath == "")
							{
								stringBuilder.Append("AlphamaskFilepath");
							}
							stringBuilder.Append(") ");
						}
					}
					MyRender11.Log.WriteLine($"Warning: Missing textures for part(s) {stringBuilder} in model {mwmFilepath}");
				}
				LoadState = 11;
				Indices = new List<int>();
				foreach (MyMeshPartInfo item2 in list)
				{
					foreach (int index in item2.m_indices)
					{
						Indices.Add(index);
					}
				}
			}
			else
			{
				Positions = new HalfVector4[0];
				Normals = new Byte4[0];
				Tangents = new Byte4[0];
				Bitangents = new Byte4[0];
				Texcoords = new HalfVector2[0];
				BoneIndices = new Vector4I[0];
				BoneWeights = new Vector4[0];
				Indices = new List<int>();
			}
			LoadState = 12;
			myModelImporter.Clear();
			return true;
		}
	}
}
