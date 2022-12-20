using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BulletXNA.BulletCollision;
using Havok;
using VRage.FileSystem;
using VRage.Game.ModAPI;
using VRage.Import;
using VRage.Utils;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Animations;
using VRageRender.Fractures;
using VRageRender.Import;
using VRageRender.Models;
using VRageRender.Utils;

namespace VRage.Game.Models
{
	public class MyModel : IDisposable, IPrimitiveManagerBase, IMyModel
	{
<<<<<<< HEAD
		private const int MAXIMUM_COLLISION_SHAPES = 10;

		private static readonly bool ENABLE_LOD1_GEOM_DATA = true;

		private readonly object m_lockObject = new object();

=======
		private static readonly bool ENABLE_LOD1_GEOM_DATA = true;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static int m_nextUniqueId = 0;

		private static Dictionary<int, string> m_uniqueModelNames = new Dictionary<int, string>();

		private static Dictionary<string, int> m_uniqueModelIds = new Dictionary<string, int>();

		[ThreadStatic]
		private static MyModelImporter m_perThreadImporter;

		public readonly int UniqueId;

		private string m_geometryDataAssetName;

		public Dictionary<string, MyModelDummy> Dummies;

		public MyModelInfo ModelInfo;

		public ModelAnimations Animations;

		public MyModelBone[] Bones;

		public MyModelFractures ModelFractures;

<<<<<<< HEAD
		public byte[] HavokData;

		public byte[] HavokDestructionData;
=======
		public bool ExportedWrong;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public HkShape[] HavokCollisionShapes;

		public HkdBreakableShape[] HavokBreakableShapes;

		private HalfVector2[] m_texCoords;

		private Byte4[] m_tangents;

		private BoundingSphere m_boundingSphere;

		private BoundingBox m_boundingBox;

		private Vector3 m_boundingBoxSize;

		private Vector3 m_boundingBoxSizeHalf;

		public Vector3I[] BoneMapping;

		public float PatternScale = 1f;

		private MyLODDescriptor[] m_lods;

		private readonly string m_assetName;

		private volatile bool m_loadedData;

		private Dictionary<string, MyMeshSection> m_meshSections = new Dictionary<string, MyMeshSection>();

		private bool m_loadingErrorProcessed;

		private float m_scaleFactor = 1f;

		private int m_verticesCount;

		private int m_trianglesCount;

		private bool m_hasUV;

		public bool m_loadUV;

		private MyCompressedVertexNormal[] m_vertices;

		private MyCompressedBoneIndicesWeights[] m_bonesIndicesWeights;

		private int[] m_Indices;

		private ushort[] m_Indices_16bit;

		public MyTriangleVertexIndices[] Triangles;

		private IMyTriangePruningStructure m_bvh;

		private List<MyMesh> m_meshContainer = new List<MyMesh>();

		public bool KeepInMemory { get; private set; }

		public int DataVersion { get; private set; }

		private static MyModelImporter m_importer
		{
			get
			{
				if (m_perThreadImporter == null)
				{
					m_perThreadImporter = new MyModelImporter();
				}
				return m_perThreadImporter;
			}
		}

		public bool LoadedData => m_loadedData;

		public MyLODDescriptor[] LODs
		{
			get
			{
				return m_lods;
			}
			private set
			{
				m_lods = value;
			}
		}

		public float ScaleFactor => m_scaleFactor;

		/// <summary>
		/// File path of the model
		/// </summary>
		public string AssetName => m_assetName;

		public BoundingSphere BoundingSphere => m_boundingSphere;

		public BoundingBox BoundingBox => m_boundingBox;

		public Vector3 BoundingBoxSize => m_boundingBoxSize;

		public Vector3 BoundingBoxSizeHalf => m_boundingBoxSizeHalf;

		int IMyModel.UniqueId => UniqueId;

		int IMyModel.DataVersion => DataVersion;

		Vector3I[] IMyModel.BoneMapping => BoneMapping.Clone() as Vector3I[];

		float IMyModel.PatternScale => PatternScale;

		public MyCompressedVertexNormal[] Vertices => m_vertices;

		public int[] Indices => m_Indices;

		public ushort[] Indices16 => m_Indices_16bit;

		public bool HasUV => m_hasUV;

		public bool LoadUV
		{
			set
			{
				m_loadUV = value;
			}
		}

		public HalfVector2[] TexCoords => m_texCoords;

		public static string GetById(int id)
		{
			return m_uniqueModelNames[id];
		}

		public static int GetId(string assetName)
		{
			lock (m_uniqueModelIds)
			{
				if (!m_uniqueModelIds.TryGetValue(assetName, out var value))
				{
					value = m_nextUniqueId++;
					m_uniqueModelIds.Add(assetName, value);
					m_uniqueModelNames.Add(value, assetName);
					return value;
				}
				return value;
			}
		}

		public MyModel(string assetName)
			: this(assetName, keepInMemory: false)
		{
			UniqueId = GetId(assetName);
		}

		/// <summary>
		/// c-tor - this constructor should be used just for max models - not voxels!
		/// </summary>
		public MyModel(string assetName, bool keepInMemory)
		{
			m_assetName = assetName;
			m_loadedData = false;
			KeepInMemory = keepInMemory;
		}

		public MyMeshSection GetMeshSection(string name)
		{
			return m_meshSections[name];
		}

		public bool TryGetMeshSection(string name, out MyMeshSection section)
		{
			return m_meshSections.TryGetValue(name, out section);
		}

		public void LoadData(bool forceFullDetail = false)
		{
			lock (m_lockObject)
			{
				if (m_loadedData)
				{
					return;
				}
				MyLog.Default.WriteLine("MyModel.LoadData -> START", LoggingOptions.LOADING_MODELS);
				MyLog.Default.IncreaseIndent(LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine("m_assetName: " + m_assetName, LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine($"Importing asset {m_assetName}, path: {AssetName}", LoggingOptions.LOADING_MODELS);
				string assetFileName = AssetName;
				string text = (Path.IsPathRooted(AssetName) ? AssetName : Path.Combine(MyFileSystem.ContentPath, AssetName));
				if (!MyFileSystem.FileExists(text))
				{
					assetFileName = "Models\\Debug\\Error.mwm";
				}
				try
				{
					m_importer.ImportData(assetFileName);
				}
				catch
				{
					MyLog.Default.WriteLine($"Importing asset failed {m_assetName}");
					throw;
				}
				DataVersion = m_importer.DataVersion;
				Dictionary<string, object> tagData = m_importer.GetTagData();
				if (tagData.Count == 0)
				{
					throw new Exception($"Uncompleted tagData for asset: {m_assetName}, path: {AssetName}");
				}
				m_meshSections.Clear();
				if (tagData.ContainsKey("Sections"))
				{
					List<MyMeshSectionInfo> obj2 = tagData["Sections"] as List<MyMeshSectionInfo>;
					int num = 0;
					foreach (MyMeshSectionInfo item in obj2)
					{
						MyMeshSection myMeshSection = new MyMeshSection
						{
							Name = item.Name,
							Index = num
						};
						m_meshSections[myMeshSection.Name] = myMeshSection;
						num++;
					}
				}
				if (tagData.ContainsKey("LODs"))
				{
					m_lods = tagData["LODs"] as MyLODDescriptor[];
				}
				Animations = (ModelAnimations)tagData["Animations"];
				Bones = (MyModelBone[])tagData["Bones"];
				m_boundingBox = (BoundingBox)tagData["BoundingBox"];
				m_boundingSphere = (BoundingSphere)tagData["BoundingSphere"];
				m_boundingBoxSize = BoundingBox.Max - BoundingBox.Min;
				m_boundingBoxSizeHalf = BoundingBoxSize / 2f;
				Dummies = tagData["Dummies"] as Dictionary<string, MyModelDummy>;
				BoneMapping = tagData["BoneMapping"] as Vector3I[];
				if (BoneMapping.Length == 0)
				{
					BoneMapping = null;
				}
				if (tagData.ContainsKey("ModelFractures"))
				{
					ModelFractures = (MyModelFractures)tagData["ModelFractures"];
				}
				if (tagData.TryGetValue("PatternScale", out var value))
				{
					PatternScale = (float)value;
				}
				if (tagData.ContainsKey("HavokCollisionGeometry"))
				{
<<<<<<< HEAD
					byte[] array = (HavokData = (byte[])tagData["HavokCollisionGeometry"]);
					if (array.Length != 0 && HkBaseSystem.IsThreadInitialized)
					{
						List<HkShape> list = new List<HkShape>();
						if (!HkShapeLoader.LoadShapesListFromBuffer(array, list, out var _, out var containsDestructionData))
=======
					HavokData = (byte[])tagData["HavokCollisionGeometry"];
					byte[] array = (byte[])tagData["HavokCollisionGeometry"];
					if (array.Length != 0 && HkBaseSystem.IsThreadInitialized)
					{
						List<HkShape> list = new List<HkShape>();
						if (!HkShapeLoader.LoadShapesListFromBuffer(array, list, out var containsScene, out var containsDestructionData))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							MyLog.Default.WriteLine($"Model {AssetName} - Unable to load collision geometry", LoggingOptions.LOADING_MODELS);
						}
						if (list.Count > 10)
						{
<<<<<<< HEAD
							if (MyFileSystem.IsGameContent(m_assetName))
							{
								MyLog.Default.WriteLine("Model " + AssetName + " - Found too many collision shapes, only the first 10 will be used");
							}
							list.RemoveRange(10, list.Count - 10);
=======
							MyLog.Default.WriteLine($"Model {AssetName} - Found too many collision shapes, only the first 10 will be used", LoggingOptions.LOADING_MODELS);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						_ = HavokCollisionShapes;
						if (list.Count > 0)
						{
							HavokCollisionShapes = list.ToArray();
							HkShape[] havokCollisionShapes = HavokCollisionShapes;
							for (int i = 0; i < havokCollisionShapes.Length; i++)
							{
								HkShape.FlagShapeAsReadOnly(havokCollisionShapes[i]);
							}
						}
						else
						{
							MyLog.Default.WriteLine($"Model {AssetName} - Unable to load collision geometry from file, default collision will be used !");
						}
<<<<<<< HEAD
						if (containsDestructionData || (Bones != null && Bones.Length == 0))
=======
						if (containsDestructionData)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							HavokDestructionData = array;
						}
						ExportedWrong = !containsScene;
					}
				}
<<<<<<< HEAD
				if (tagData.ContainsKey("HavokDestruction"))
				{
					byte[] array2 = (byte[])tagData["HavokDestruction"];
					if (array2.Length != 0)
					{
						HavokDestructionData = array2;
					}
				}
				m_geometryDataAssetName = AssetName;
				if (!forceFullDetail && LODs != null && LODs.Length != 0 && ENABLE_LOD1_GEOM_DATA)
				{
					string modelAbsoluteFilePath = LODs[0].GetModelAbsoluteFilePath(text);
					if (modelAbsoluteFilePath != null)
					{
						m_geometryDataAssetName = modelAbsoluteFilePath;
					}
				}
				if (m_geometryDataAssetName == AssetName && tagData.ContainsKey("GeometryDataAsset"))
				{
					MyLODDescriptor myLODDescriptor = new MyLODDescriptor
					{
						Model = (string)tagData["GeometryDataAsset"]
					};
					m_geometryDataAssetName = myLODDescriptor.GetModelAbsoluteFilePath(text);
				}
				try
				{
					if (m_geometryDataAssetName != AssetName)
					{
						m_importer.ImportData(m_geometryDataAssetName);
					}
					LoadGeometryData(m_importer);
				}
				catch
				{
					throw new Exception($"Can't load geometry data for asset: {AssetName}, geometry data path: {m_geometryDataAssetName}");
				}
				MyLog.Default.WriteLine("Triangles.Length: " + Triangles.Length, LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine("Vertexes.Length: " + GetVerticesCount(), LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine("UseChannelTextures: " + (bool)tagData["UseChannelTextures"], LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine("BoundingBox: " + BoundingBox, LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine("BoundingSphere: " + BoundingSphere, LoggingOptions.LOADING_MODELS);
				VRageRender.Utils.Stats.PerAppLifetime.MyModelsCount++;
				VRageRender.Utils.Stats.PerAppLifetime.MyModelsMeshesCount += m_meshContainer.Count;
				VRageRender.Utils.Stats.PerAppLifetime.MyModelsVertexesCount += GetVerticesCount();
				VRageRender.Utils.Stats.PerAppLifetime.MyModelsTrianglesCount += Triangles.Length;
				ModelInfo = new MyModelInfo(GetTrianglesCount(), GetVerticesCount(), BoundingBoxSize);
				m_loadedData = true;
				m_loadingErrorProcessed = false;
				MyLog.Default.DecreaseIndent(LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine("MyModel.LoadData -> END", LoggingOptions.LOADING_MODELS);
				m_importer.Clear();
			}
		}

		public bool LoadTexCoordData()
		{
			if (!m_hasUV)
			{
				lock (m_lockObject)
				{
					if (!m_loadedData)
					{
						m_loadUV = true;
						LoadData();
					}
					else
					{
=======
				if (tagData.ContainsKey("HavokDestruction") && ((byte[])tagData["HavokDestruction"]).Length != 0)
				{
					HavokDestructionData = (byte[])tagData["HavokDestruction"];
				}
				m_geometryDataAssetName = AssetName;
				if (!forceFullDetail && LODs != null && LODs.Length != 0 && ENABLE_LOD1_GEOM_DATA)
				{
					string modelAbsoluteFilePath = LODs[0].GetModelAbsoluteFilePath(text);
					if (modelAbsoluteFilePath != null)
					{
						m_geometryDataAssetName = modelAbsoluteFilePath;
					}
				}
				if (m_geometryDataAssetName == AssetName && tagData.ContainsKey("GeometryDataAsset"))
				{
					MyLODDescriptor myLODDescriptor = new MyLODDescriptor
					{
						Model = (string)tagData["GeometryDataAsset"]
					};
					m_geometryDataAssetName = myLODDescriptor.GetModelAbsoluteFilePath(text);
				}
				try
				{
					if (m_geometryDataAssetName != AssetName)
					{
						m_importer.ImportData(m_geometryDataAssetName);
					}
					LoadGeometryData(m_importer);
				}
				catch
				{
					throw new Exception($"Can't load geometry data for asset: {AssetName}, geometry data path: {m_geometryDataAssetName}");
				}
				MyLog.Default.WriteLine("Triangles.Length: " + Triangles.Length, LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine("Vertexes.Length: " + GetVerticesCount(), LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine("UseChannelTextures: " + (bool)tagData["UseChannelTextures"], LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine("BoundingBox: " + BoundingBox, LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine("BoundingSphere: " + BoundingSphere, LoggingOptions.LOADING_MODELS);
				VRageRender.Utils.Stats.PerAppLifetime.MyModelsCount++;
				VRageRender.Utils.Stats.PerAppLifetime.MyModelsMeshesCount += m_meshContainer.Count;
				VRageRender.Utils.Stats.PerAppLifetime.MyModelsVertexesCount += GetVerticesCount();
				VRageRender.Utils.Stats.PerAppLifetime.MyModelsTrianglesCount += Triangles.Length;
				ModelInfo = new MyModelInfo(GetTrianglesCount(), GetVerticesCount(), BoundingBoxSize);
				m_loadedData = true;
				m_loadingErrorProcessed = false;
				MyLog.Default.DecreaseIndent(LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine("MyModel.LoadData -> END", LoggingOptions.LOADING_MODELS);
			}
		}

		public bool LoadTexCoordData()
		{
			if (!m_hasUV)
			{
				lock (this)
				{
					if (!m_loadedData)
					{
						m_loadUV = true;
						LoadData();
					}
					else
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						try
						{
							m_importer.ImportData(m_geometryDataAssetName, new string[1] { "TexCoords0" });
						}
						catch
						{
							MyLog.Default.WriteLine($"Importing asset failed {m_assetName}");
							return false;
						}
						Dictionary<string, object> tagData = m_importer.GetTagData();
						m_texCoords = (HalfVector2[])tagData["TexCoords0"];
<<<<<<< HEAD
						m_importer.Clear();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					m_hasUV = true;
					m_loadUV = false;
				}
			}
			return m_hasUV;
		}

		public void LoadAnimationData(bool forceReload = false)
		{
			if (m_loadedData && !forceReload)
<<<<<<< HEAD
			{
				return;
			}
			lock (m_lockObject)
			{
=======
			{
				return;
			}
			lock (this)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyLog.Default.WriteLine("MyModel.LoadData -> START", LoggingOptions.LOADING_MODELS);
				MyLog.Default.IncreaseIndent(LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine("m_assetName: " + m_assetName, LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine($"Importing asset {m_assetName}, path: {AssetName}", LoggingOptions.LOADING_MODELS);
				try
				{
					m_importer.ImportData(AssetName);
				}
				catch
				{
					MyLog.Default.WriteLine($"Importing asset failed {m_assetName}");
					throw;
				}
				Dictionary<string, object> tagData = m_importer.GetTagData();
				if (tagData.Count != 0)
				{
					DataVersion = m_importer.DataVersion;
					Animations = (ModelAnimations)tagData["Animations"];
					Bones = (MyModelBone[])tagData["Bones"];
					m_boundingBox = (BoundingBox)tagData["BoundingBox"];
					m_boundingSphere = (BoundingSphere)tagData["BoundingSphere"];
					m_boundingBoxSize = BoundingBox.Max - BoundingBox.Min;
					m_boundingBoxSizeHalf = BoundingBoxSize / 2f;
					Dummies = tagData["Dummies"] as Dictionary<string, MyModelDummy>;
					BoneMapping = tagData["BoneMapping"] as Vector3I[];
					if (BoneMapping.Length == 0)
					{
						BoneMapping = null;
					}
				}
				else
				{
					DataVersion = 0;
					Animations = null;
					Bones = null;
					m_boundingBox = default(BoundingBox);
					m_boundingSphere = default(BoundingSphere);
					m_boundingBoxSize = default(Vector3);
					m_boundingBoxSizeHalf = default(Vector3);
					Dummies = null;
					BoneMapping = null;
				}
				ModelInfo = new MyModelInfo(GetTrianglesCount(), GetVerticesCount(), BoundingBoxSize);
				if (tagData.Count != 0)
				{
					m_loadedData = true;
				}
				MyLog.Default.DecreaseIndent(LoggingOptions.LOADING_MODELS);
				MyLog.Default.WriteLine("MyModel.LoadAnimationData -> END", LoggingOptions.LOADING_MODELS);
<<<<<<< HEAD
				m_importer.Clear();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public bool UnloadData()
		{
			bool loadedData = m_loadedData;
			m_loadedData = false;
			if (m_bvh != null)
			{
				m_bvh.Close();
				m_bvh = null;
			}
			VRageRender.Utils.Stats.PerAppLifetime.MyModelsMeshesCount -= m_meshContainer.Count;
			if (m_vertices != null)
			{
				VRageRender.Utils.Stats.PerAppLifetime.MyModelsVertexesCount -= GetVerticesCount();
			}
			if (Triangles != null)
			{
				VRageRender.Utils.Stats.PerAppLifetime.MyModelsTrianglesCount -= Triangles.Length;
			}
			if (loadedData)
			{
				VRageRender.Utils.Stats.PerAppLifetime.MyModelsCount--;
			}
			if (HavokCollisionShapes != null)
			{
				for (int i = 0; i < HavokCollisionShapes.Length; i++)
				{
					HavokCollisionShapes[i].RemoveReference();
				}
				HavokCollisionShapes = null;
			}
			HavokBreakableShapes = null;
			Triangles = null;
			m_vertices = null;
			m_bonesIndicesWeights = null;
			m_meshContainer.Clear();
			m_meshSections.Clear();
			m_Indices_16bit = null;
			m_Indices = null;
			ModelInfo = null;
			Bones = null;
			Dummies = null;
			HavokData = null;
			HavokDestructionData = null;
			ModelFractures = null;
			m_texCoords = null;
			m_tangents = null;
			BoneMapping = null;
			m_lods = null;
			m_scaleFactor = 1f;
			Animations = null;
			return loadedData;
		}

		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			m_meshContainer.Clear();
		}

		public void LoadOnlyDummies()
		{
			if (m_loadedData)
<<<<<<< HEAD
			{
				return;
			}
			lock (m_lockObject)
			{
=======
			{
				return;
			}
			lock (this)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyLog.Default.WriteLine("MyModel.LoadSnapPoints -> START", LoggingOptions.LOADING_MODELS);
				using (MyLog.Default.IndentUsing(LoggingOptions.LOADING_MODELS))
				{
					MyLog.Default.WriteLine("m_assetName: " + m_assetName, LoggingOptions.LOADING_MODELS);
					MyModelImporter myModelImporter = new MyModelImporter();
					MyLog.Default.WriteLine($"Importing asset {m_assetName}, path: {AssetName}", LoggingOptions.LOADING_MODELS);
					try
					{
						myModelImporter.ImportData(AssetName, new string[1] { "Dummies" });
					}
					catch (Exception ex)
					{
						MyLog.Default.WriteLine($"Importing asset failed {m_assetName}, message: {ex.Message}, stack:{ex.StackTrace}");
					}
					Dictionary<string, object> tagData = myModelImporter.GetTagData();
					if (tagData.Count > 0)
					{
						Dummies = tagData["Dummies"] as Dictionary<string, MyModelDummy>;
					}
					else
					{
						Dummies = new Dictionary<string, MyModelDummy>();
					}
				}
			}
		}

		public void LoadOnlyModelInfo()
		{
			if (m_loadedData)
<<<<<<< HEAD
			{
				return;
			}
			lock (m_lockObject)
			{
=======
			{
				return;
			}
			lock (this)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyLog.Default.WriteLine("MyModel.LoadModelData -> START", LoggingOptions.LOADING_MODELS);
				using (MyLog.Default.IndentUsing(LoggingOptions.LOADING_MODELS))
				{
					MyLog.Default.WriteLine("m_assetName: " + m_assetName, LoggingOptions.LOADING_MODELS);
					MyModelImporter myModelImporter = new MyModelImporter();
					MyLog.Default.WriteLine($"Importing asset {m_assetName}, path: {AssetName}", LoggingOptions.LOADING_MODELS);
					try
					{
						myModelImporter.ImportData(AssetName, new string[1] { "ModelInfo" });
					}
					catch (Exception ex)
					{
						MyLog.Default.WriteLine($"Importing asset failed {m_assetName}, message: {ex.Message}, stack:{ex.StackTrace}");
					}
					Dictionary<string, object> tagData = myModelImporter.GetTagData();
					if (tagData.Count > 0)
					{
						ModelInfo = tagData["ModelInfo"] as MyModelInfo;
					}
					else
					{
						ModelInfo = new MyModelInfo(0, 0, Vector3.Zero);
					}
				}
			}
		}

		void IPrimitiveManagerBase.Cleanup()
		{
		}

		bool IPrimitiveManagerBase.IsTrimesh()
		{
			return true;
		}

		int IPrimitiveManagerBase.GetPrimitiveCount()
		{
			return m_trianglesCount;
		}

		void IPrimitiveManagerBase.GetPrimitiveBox(int prim_index, out AABB primbox)
		{
			BoundingBox boundingBox = BoundingBox.CreateInvalid();
			Vector3 p = GetVertex(Triangles[prim_index].I0);
			Vector3 p2 = GetVertex(Triangles[prim_index].I1);
			Vector3 p3 = GetVertex(Triangles[prim_index].I2);
			boundingBox.Include(ref p, ref p2, ref p3);
			primbox = new AABB
			{
				m_min = boundingBox.Min.ToBullet(),
				m_max = boundingBox.Max.ToBullet()
			};
		}

		void IPrimitiveManagerBase.GetPrimitiveTriangle(int prim_index, PrimitiveTriangle triangle)
		{
			triangle.m_vertices[0] = GetVertex(Triangles[prim_index].I0).ToBullet();
			triangle.m_vertices[1] = GetVertex(Triangles[prim_index].I1).ToBullet();
			triangle.m_vertices[2] = GetVertex(Triangles[prim_index].I2).ToBullet();
		}

		public void CheckLoadingErrors(MyModContext context, out bool errorFound)
		{
			if (!m_loadingErrorProcessed)
			{
				errorFound = true;
				m_loadingErrorProcessed = true;
			}
			else
			{
				errorFound = false;
			}
		}

		public void Rescale(float scaleFactor)
		{
			if (m_scaleFactor == scaleFactor)
			{
				return;
			}
			float num = scaleFactor / m_scaleFactor;
			m_scaleFactor = scaleFactor;
			for (int i = 0; i < m_verticesCount; i++)
			{
				Vector3 newPosition = GetVertex(i) * num;
				SetVertexPosition(i, ref newPosition);
			}
			if (Dummies != null)
			{
				foreach (KeyValuePair<string, MyModelDummy> dummy in Dummies)
				{
					Matrix matrix = dummy.Value.Matrix;
					matrix.Translation *= num;
					dummy.Value.Matrix = matrix;
				}
			}
			m_boundingBox.Min *= num;
			m_boundingBox.Max *= num;
			m_boundingBoxSize = BoundingBox.Max - BoundingBox.Min;
			m_boundingBoxSizeHalf = BoundingBoxSize / 2f;
			m_boundingSphere.Radius *= num;
		}

		int IMyModel.GetDummies(IDictionary<string, IMyModelDummy> dummies)
		{
			if (Dummies == null)
			{
				return 0;
			}
			if (dummies != null)
			{
				foreach (KeyValuePair<string, MyModelDummy> dummy in Dummies)
				{
					dummies.Add(dummy.Key, dummy.Value);
				}
			}
			return Dummies.Count;
		}

		public Vector3 GetVertexInt(int vertexIndex)
		{
			return VF_Packer.UnpackPosition(ref m_vertices[vertexIndex].Position);
		}

		public MyTriangleVertexIndices GetTriangle(int triangleIndex)
		{
			return Triangles[triangleIndex];
		}

<<<<<<< HEAD
		IMyTriangleVertexIndices IMyModel.GetTriangle(int triangleIndex)
		{
			return GetTriangle(triangleIndex);
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public Vector3 GetVertex(int vertexIndex)
		{
			return GetVertexInt(vertexIndex);
		}

		public void SetVertexPosition(int vertexIndex, ref Vector3 newPosition)
		{
			m_vertices[vertexIndex].Position = VF_Packer.PackPosition(newPosition);
		}

		public void GetVertex(int vertexIndex1, int vertexIndex2, int vertexIndex3, out Vector3 v1, out Vector3 v2, out Vector3 v3)
		{
			v1 = GetVertex(vertexIndex1);
			v2 = GetVertex(vertexIndex2);
			v3 = GetVertex(vertexIndex3);
		}

		public MyTriangle_BoneIndicesWeigths? GetBoneIndicesWeights(int triangleIndex)
		{
			if (m_bonesIndicesWeights == null)
			{
				return null;
			}
			MyTriangleVertexIndices myTriangleVertexIndices = Triangles[triangleIndex];
			MyCompressedBoneIndicesWeights myCompressedBoneIndicesWeights = m_bonesIndicesWeights[myTriangleVertexIndices.I0];
			MyCompressedBoneIndicesWeights myCompressedBoneIndicesWeights2 = m_bonesIndicesWeights[myTriangleVertexIndices.I1];
			MyCompressedBoneIndicesWeights myCompressedBoneIndicesWeights3 = m_bonesIndicesWeights[myTriangleVertexIndices.I2];
			Vector4UByte indices = myCompressedBoneIndicesWeights.Indices.ToVector4UByte();
			Vector4 weights = myCompressedBoneIndicesWeights.Weights.ToVector4();
			Vector4UByte indices2 = myCompressedBoneIndicesWeights2.Indices.ToVector4UByte();
			Vector4 weights2 = myCompressedBoneIndicesWeights2.Weights.ToVector4();
			Vector4UByte indices3 = myCompressedBoneIndicesWeights3.Indices.ToVector4UByte();
			Vector4 weights3 = myCompressedBoneIndicesWeights3.Weights.ToVector4();
			MyTriangle_BoneIndicesWeigths value = default(MyTriangle_BoneIndicesWeigths);
			MyVertex_BoneIndicesWeights myVertex_BoneIndicesWeights = (value.Vertex0 = new MyVertex_BoneIndicesWeights
			{
				Indices = indices,
				Weights = weights
			});
			myVertex_BoneIndicesWeights = (value.Vertex1 = new MyVertex_BoneIndicesWeights
			{
				Indices = indices2,
				Weights = weights2
			});
			myVertex_BoneIndicesWeights = (value.Vertex2 = new MyVertex_BoneIndicesWeights
			{
				Indices = indices3,
				Weights = weights3
			});
			return value;
		}

		public Vector3 GetVertexNormal(int vertexIndex)
		{
			return VF_Packer.UnpackNormal(ref m_vertices[vertexIndex].Normal);
		}

		public Vector3 GetVertexTangent(int vertexIndex)
		{
			if (m_tangents == null)
			{
				m_importer.ImportData(AssetName, new string[1] { "Tangents" });
				Dictionary<string, object> tagData = m_importer.GetTagData();
				if (tagData.ContainsKey("Tangents"))
				{
					m_tangents = (Byte4[])tagData["Tangents"];
				}
<<<<<<< HEAD
				m_importer.Clear();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (m_tangents != null)
			{
				return VF_Packer.UnpackNormal(m_tangents[vertexIndex]);
			}
			return Vector3.Zero;
		}

		public List<MyMesh> GetMeshList()
		{
			return m_meshContainer;
		}

		private int GetNumberOfTrianglesForColDet()
		{
			int num = 0;
			foreach (MyMesh item in m_meshContainer)
			{
				num += item.TriCount;
			}
			return num;
		}

		private void CopyTriangleIndices()
		{
			Triangles = new MyTriangleVertexIndices[GetNumberOfTrianglesForColDet()];
			int num = 0;
			foreach (MyMesh item in m_meshContainer)
			{
				item.TriStart = num;
				if (m_Indices != null)
				{
					for (int i = 0; i < item.TriCount; i++)
					{
						Triangles[num] = new MyTriangleVertexIndices(m_Indices[item.IndexStart + i * 3], m_Indices[item.IndexStart + i * 3 + 2], m_Indices[item.IndexStart + i * 3 + 1]);
						num++;
					}
					continue;
				}
				if (m_Indices_16bit != null)
				{
					for (int j = 0; j < item.TriCount; j++)
					{
						Triangles[num] = new MyTriangleVertexIndices(m_Indices_16bit[item.IndexStart + j * 3], m_Indices_16bit[item.IndexStart + j * 3 + 2], m_Indices_16bit[item.IndexStart + j * 3 + 1]);
						num++;
					}
					continue;
				}
				throw new InvalidBranchException();
			}
		}

		[Conditional("DEBUG")]
		private void CheckTriangles(int triangleCount)
		{
			bool flag = true;
			MyTriangleVertexIndices[] triangles = Triangles;
			for (int i = 0; i < triangles.Length; i++)
			{
				MyTriangleVertexIndices myTriangleVertexIndices = triangles[i];
				flag = flag & (myTriangleVertexIndices.I0 != myTriangleVertexIndices.I1) & (myTriangleVertexIndices.I1 != myTriangleVertexIndices.I2) & (myTriangleVertexIndices.I2 != myTriangleVertexIndices.I0) & ((myTriangleVertexIndices.I0 >= 0) & (myTriangleVertexIndices.I0 < m_verticesCount)) & ((myTriangleVertexIndices.I1 >= 0) & (myTriangleVertexIndices.I1 < m_verticesCount)) & ((myTriangleVertexIndices.I2 >= 0) & (myTriangleVertexIndices.I2 < m_verticesCount));
			}
		}

		public IMyTriangePruningStructure GetTrianglePruningStructure()
		{
			return m_bvh;
		}

		public void GetTriangleBoundingBox(int triangleIndex, ref BoundingBox boundingBox)
		{
			boundingBox = BoundingBox.CreateInvalid();
			GetVertex(Triangles[triangleIndex].I0, Triangles[triangleIndex].I1, Triangles[triangleIndex].I2, out var v, out var v2, out var v3);
			boundingBox.Include(v, v2, v3);
		}

		public int GetTrianglesCount()
		{
			return m_trianglesCount;
		}

		public int GetVerticesCount()
		{
			return m_verticesCount;
		}

		public int GetBVHSize()
		{
			if (m_bvh == null)
			{
				return 0;
			}
			return m_bvh.Size;
		}

		public MyMeshDrawTechnique GetDrawTechnique(int triangleIndex)
		{
			MyMeshDrawTechnique result = MyMeshDrawTechnique.MESH;
			for (int i = 0; i < m_meshContainer.Count; i++)
			{
				if (triangleIndex >= m_meshContainer[i].TriStart && triangleIndex < m_meshContainer[i].TriStart + m_meshContainer[i].TriCount)
				{
					result = m_meshContainer[i].Material.DrawTechnique;
				}
			}
			return result;
		}

		private void LoadGeometryData(MyModelImporter importer)
		{
			Dictionary<string, object> tagData = importer.GetTagData();
			HalfVector4[] array = (HalfVector4[])tagData["Vertices"];
			Byte4[] array2 = (Byte4[])tagData["Normals"];
			m_vertices = new MyCompressedVertexNormal[array.Length];
			MyCompressedVertexNormal myCompressedVertexNormal;
			if (array2.Length != 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					myCompressedVertexNormal = (m_vertices[i] = new MyCompressedVertexNormal
					{
						Position = array[i],
						Normal = array2[i]
					});
				}
			}
			else
			{
				for (int j = 0; j < array.Length; j++)
				{
					myCompressedVertexNormal = (m_vertices[j] = new MyCompressedVertexNormal
					{
						Position = array[j]
					});
				}
			}
			m_verticesCount = array.Length;
			if (m_loadUV)
			{
				m_texCoords = (HalfVector2[])tagData["TexCoords0"];
				m_hasUV = true;
				m_loadUV = false;
			}
			m_meshContainer.Clear();
			if (tagData.ContainsKey("MeshParts"))
			{
				List<int> list = new List<int>(GetVerticesCount());
				int num = 0;
				foreach (MyMeshPartInfo item in tagData["MeshParts"] as List<MyMeshPartInfo>)
				{
					MyMesh myMesh = new MyMesh(item, m_assetName);
					myMesh.IndexStart = list.Count;
					myMesh.TriCount = item.m_indices.Count / 3;
					if (myMesh.TriCount == 0)
					{
						return;
					}
					foreach (int index in item.m_indices)
					{
						list.Add(index);
						if (index > num)
						{
							num = index;
						}
					}
					m_meshContainer.Add(myMesh);
				}
				if (num <= 65535)
				{
					m_Indices_16bit = new ushort[list.Count];
					for (int k = 0; k < list.Count; k++)
					{
						m_Indices_16bit[k] = (ushort)list[k];
					}
				}
				else
				{
					m_Indices = list.ToArray();
				}
			}
			if (tagData.ContainsKey("ModelBvh"))
			{
				m_bvh = new MyQuantizedBvhAdapter(tagData["ModelBvh"] as GImpactQuantizedBvh, this);
			}
			Vector4I[] array3 = (Vector4I[])tagData["BlendIndices"];
			Vector4[] array4 = (Vector4[])tagData["BlendWeights"];
			if (array3 != null && array3.Length != 0 && array4 != null && array3.Length == array4.Length && array3.Length == m_vertices.Length)
			{
				m_bonesIndicesWeights = new MyCompressedBoneIndicesWeights[array3.Length];
				for (int l = 0; l < array3.Length; l++)
				{
					m_bonesIndicesWeights[l].Indices = new Byte4(array3[l].X, array3[l].Y, array3[l].Z, array3[l].W);
					m_bonesIndicesWeights[l].Weights = new HalfVector4(array4[l]);
				}
			}
			CopyTriangleIndices();
			m_trianglesCount = Triangles.Length;
		}
	}
}
