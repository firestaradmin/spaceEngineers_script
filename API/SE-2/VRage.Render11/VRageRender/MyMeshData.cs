using System;
using System.Collections.Generic;
using System.IO;
using VRage.Library.Collections;
using VRage.Render11.Common;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Import;

namespace VRageRender
{
	public class MyMeshData
	{
		public MyLODDescriptor[] Lods;

		public HalfVector4[] Positions;

		public Byte4[] Normals;

		public Byte4[] Tangents;

		public Byte4[] Bitangents;

		public HalfVector2[] Texcoords;

		public Vector4I[] TexIndices;

		public MyModelBone[] Bones;

		public Vector4I[] BoneIndices;

		public Vector4[] BoneWeights;

		public List<MyMeshPartInfo> PartInfos;

		public List<MyMeshSectionInfo> SectionInfos;

		public BoundingBox BoundindBox;

		public BoundingSphere BoundingSphere;

		public MyList<uint> NewIndices;

		public uint MaxIndex;

		public Byte4[] StoredTangents;

		public Dictionary<string, Tuple<int, int, int>> MatsIndices;

		public Dictionary<string, MyMeshMaterialId> Materials;

		public int VerticesNum => Positions.Length;

		public bool ValidStreams { get; private set; }

		public bool IsAnimated { get; set; }

		public float PatternScale { get; private set; }

		public MyMeshMaterialId Material(string name)
		{
			if (Materials != null && Materials.TryGetValue(name, out var value))
			{
				return value;
			}
			return MyMeshMaterialId.NULL;
		}

		public void Material(string name, MyMeshMaterialId matId)
		{
			if (Materials == null)
			{
				Materials = new Dictionary<string, MyMeshMaterialId>(1);
			}
			Materials[name] = matId;
		}

		public void DoImport(MyModelImporter importer, string fsPath)
		{
			string[] tags = new string[15]
			{
				"Vertices", "BlendIndices", "BlendWeights", "Normals", "TexCoords0", "Tangents", "Binormals", "Bones", "MeshParts", "Sections",
				"BoundingBox", "BoundingSphere", "LODs", "PatternScale", "GeometryDataAsset"
			};
			importer.ImportData(fsPath, tags);
			Dictionary<string, object> tagData = importer.GetTagData();
			if (tagData.ContainsKey("GeometryDataAsset"))
			{
				string text = (string)tagData["GeometryDataAsset"];
				string modelAbsoluteFilePath = new MyLODDescriptor
				{
					Model = text
				}.GetModelAbsoluteFilePath(fsPath);
				try
				{
					importer.ImportData(modelAbsoluteFilePath, tags);
					tagData = importer.GetTagData();
				}
				catch
				{
					throw new Exception($"Can't load geometry data for asset: {fsPath}, geometry data path: {text}");
				}
			}
			PartInfos = tagData["MeshParts"] as List<MyMeshPartInfo>;
			if (tagData.ContainsKey("Sections"))
			{
				SectionInfos = tagData["Sections"] as List<MyMeshSectionInfo>;
			}
			if (tagData.ContainsKey("LODs"))
			{
				Lods = (MyLODDescriptor[])tagData["LODs"];
			}
			Positions = (HalfVector4[])tagData["Vertices"];
			Normals = (Byte4[])tagData["Normals"];
			Tangents = (Byte4[])tagData["Tangents"];
			Bitangents = (Byte4[])tagData["Binormals"];
			Texcoords = (HalfVector2[])tagData["TexCoords0"];
			TexIndices = MyManagers.GeometryTextureSystem.CreateTextureIndices(PartInfos, VerticesNum, MyMeshes.GetContentPath(fsPath));
			BoneIndices = (Vector4I[])tagData["BlendIndices"];
			BoneWeights = (Vector4[])tagData["BlendWeights"];
			ValidStreams = Normals.Length != 0 && Normals.Length == VerticesNum && Texcoords.Length != 0 && Texcoords.Length == VerticesNum && Tangents.Length != 0 && Tangents.Length == VerticesNum && Bitangents.Length != 0 && Bitangents.Length == VerticesNum;
			if (!ValidStreams)
			{
				MyRender11.Log.WriteLine($"Mesh asset {Path.GetFileName(fsPath)} has inconsistent vertex streams");
				MyRender11.Log.IncreaseIndent();
				MyRender11.Log.WriteLine($"Normals length: {Normals.Length} expected length: {VerticesNum}");
				MyRender11.Log.WriteLine($"Texcoords length: {Texcoords.Length} expected length: {VerticesNum}");
				MyRender11.Log.WriteLine($"Tangents length: {Tangents.Length} expected length: {VerticesNum}");
				MyRender11.Log.WriteLine($"Bitangents length: {Bitangents.Length} expected length: {VerticesNum}");
				MyRender11.Log.DecreaseIndent();
				Normals = new Byte4[0];
				Texcoords = new HalfVector2[0];
				Tangents = new Byte4[0];
				Bitangents = new Byte4[0];
			}
			PatternScale = 1f;
			if (tagData.TryGetValue("PatternScale", out var value))
			{
				PatternScale = (float)value;
			}
			IsAnimated = BoneIndices.Length != 0 && BoneWeights.Length != 0 && BoneIndices.Length == VerticesNum && BoneWeights.Length == VerticesNum;
			Bones = (MyModelBone[])tagData["Bones"];
			BoundindBox = (BoundingBox)tagData["BoundingBox"];
			BoundingSphere = (BoundingSphere)tagData["BoundingSphere"];
		}
	}
}
