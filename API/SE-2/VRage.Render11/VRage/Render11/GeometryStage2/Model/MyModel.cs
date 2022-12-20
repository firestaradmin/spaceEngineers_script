using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.Lodding;
using VRage.Render11.GeometryStage2.Model.Preprocess;
using VRage.Render11.Resources.Textures;
using VRageMath;
using VRageRender.Import;
using VRageRender.Messages;

namespace VRage.Render11.GeometryStage2.Model
{
	internal class MyModel
	{
		private List<MyLod> m_lods;

		private MyLodStrategyInfo m_lodStrategyInfo;

		private MyModelInstanceMaterialsStrategy m_instanceMaterialsStrategy = new MyModelInstanceMaterialsStrategy();

		private BoundingBox m_boundingBox;

		private BoundingBoxD m_boundingBoxD;

		private readonly List<MyModelInstance> m_modelInstances = new List<MyModelInstance>();

		public int LoadState;

		public string Filepath { get; private set; }

		public bool IsValid { get; private set; }

		public BoundingBox BoundingBox => m_boundingBox;

		public BoundingBoxD BoundingBoxD => m_boundingBoxD;

		public MyLod GetLod(int lod)
		{
			return m_lods[lod];
		}

		public MyLodStrategyInfo GetLodStrategyInfo()
		{
			return m_lodStrategyInfo;
		}

		public int GetInstanceMaterialOffset(string materialName)
		{
			return m_instanceMaterialsStrategy.GetInstanceMaterialOffset(materialName);
		}

		public void ActivateInstanceMaterial(string materialName)
		{
			if (m_instanceMaterialsStrategy.IsInstanceMaterialActivated(materialName))
			{
				return;
			}
			m_instanceMaterialsStrategy.ActivateInstanceMaterial(materialName);
			int instanceMaterialOffset = m_instanceMaterialsStrategy.GetInstanceMaterialOffset(materialName);
			foreach (MyLod lod in m_lods)
			{
				lod.AddInstanceMaterial(materialName, instanceMaterialOffset);
			}
		}

		public bool IsUsedMaterial(string materialName)
		{
			return m_instanceMaterialsStrategy.IsUsedMaterial(materialName);
		}

		public Dictionary<string, int>.Enumerator GetInstanceMaterialOffsetsEnumeratorInternal()
		{
			return m_instanceMaterialsStrategy.Enumerator();
		}

		internal MyModelInstanceMaterialsStrategy GetInstanceMaterialsStrategyInternal()
		{
			return m_instanceMaterialsStrategy;
		}

		public int GetAllMaterialsCount()
		{
			return m_instanceMaterialsStrategy.Count;
		}

		private void UpdateBoundingBoxes()
		{
			BoundingBox boundingBox = m_lods[0].BoundingBox;
			for (int i = 1; i < m_lods.Count; i++)
			{
				boundingBox.Min = Vector3.Min(boundingBox.Min, m_lods[i].BoundingBox.Min);
				boundingBox.Max = Vector3.Max(boundingBox.Max, m_lods[i].BoundingBox.Max);
			}
			m_boundingBox = boundingBox;
			m_boundingBoxD = boundingBox;
		}

		private bool IsModelSuitable(MyMwmData mwmData)
		{
			if (mwmData.IsSkinned)
			{
				return false;
			}
			if (!mwmData.IsValid2ndStream)
			{
				return false;
			}
			foreach (MyMwmDataPart part in mwmData.Parts)
			{
				if (part.Facing != 0)
				{
					return false;
				}
				switch (part.Technique)
				{
				case MyMeshDrawTechnique.MESH:
				case MyMeshDrawTechnique.ALPHA_MASKED:
				case MyMeshDrawTechnique.ALPHA_MASKED_SINGLE_SIDED:
				case MyMeshDrawTechnique.DECAL:
				case MyMeshDrawTechnique.DECAL_NOPREMULT:
				case MyMeshDrawTechnique.DECAL_CUTOUT:
				case MyMeshDrawTechnique.HOLO:
				case MyMeshDrawTechnique.GLASS:
				case MyMeshDrawTechnique.SHIELD:
				case MyMeshDrawTechnique.SHIELD_LIT:
					continue;
				}
				return false;
			}
			return true;
		}

		public MyModel()
		{
			m_modelInstances.Add(new MyModelInstance(this, null, 0));
		}

		public bool LoadFromFile(string filepath, int minLoadingLod)
		{
			Filepath = filepath;
			if (m_lods == null)
			{
				m_lods = new List<MyLod>();
			}
			else
			{
				m_lods.Clear();
			}
			IsValid = false;
			if (!MyMwmUtils.IsInContentPath(filepath))
			{
				minLoadingLod = 0;
			}
			LoadState = 1;
			MyMwmData mwmData2 = default(MyMwmData);
			if (!mwmData2.LoadFromFile(filepath))
			{
				return false;
			}
			m_instanceMaterialsStrategy.Init();
			m_lodStrategyInfo.Init(mwmData2.LodDescriptors);
			LoadState = 2;
			int val = minLoadingLod;
			MyLODDescriptor[] lodDescriptors = mwmData2.LodDescriptors;
			int num = Math.Min(val, (lodDescriptors != null) ? lodDescriptors.Length : 0);
			if (num == 0 && !AppendMwmDataToLod(mwmData2))
			{
				return false;
			}
			LoadState = 3;
			for (int i = Math.Max(num - 1, 0); i < mwmData2.LodDescriptors.Length; i++)
			{
				string modelAbsoluteFilePath = mwmData2.LodDescriptors[i].GetModelAbsoluteFilePath(filepath);
				if (modelAbsoluteFilePath == null)
				{
					return false;
				}
				MyMwmData mwmData3 = default(MyMwmData);
				if (!mwmData3.LoadFromFile(modelAbsoluteFilePath))
				{
					return false;
				}
				if (!AppendMwmDataToLod(mwmData3))
				{
					return false;
				}
			}
			for (int j = 0; j < num; j++)
			{
				m_lods.Insert(0, m_lods[0]);
			}
			m_lodStrategyInfo.ReduceLodsCount(m_lods.Count);
			LoadState = 4;
			UpdateBoundingBoxes();
			LoadState = 5;
			foreach (MyModelInstance modelInstance in m_modelInstances)
			{
				modelInstance.Reload();
			}
			IsValid = true;
			return true;
			bool AppendMwmDataToLod(MyMwmData mwmData)
			{
				if (mwmData.IsStub)
				{
					MyMwmData myMwmData = default(MyMwmData);
					MyLODDescriptor myLODDescriptor = new MyLODDescriptor
					{
						Model = mwmData.GeometryDataPath
					};
					myMwmData.LoadFromFile(myLODDescriptor.GetModelAbsoluteFilePath(mwmData.MwmFilepath));
					mwmData = myMwmData;
				}
				if (!IsModelSuitable(mwmData))
				{
					return false;
				}
				MyLod myLod = new MyLod();
				myLod.Create(mwmData, m_lods.Count, ref m_instanceMaterialsStrategy);
				m_lods.Add(myLod);
				return true;
			}
		}

		public void UnloadData()
		{
			if (m_lods != null)
			{
				foreach (MyLod lod in m_lods)
				{
					lod.UnloadData();
				}
			}
			m_lods = null;
		}

		public int GetChangeContentHash(Dictionary<string, MyTextureChange> changes)
		{
			int num = 0;
			foreach (KeyValuePair<string, MyTextureChange> change in changes)
			{
				LinqExtensions.Deconstruct(change, out var k, out var v);
				string self = k;
				MyTextureChange myTextureChange = v;
				num = (num, self.GetHashCode64(), myTextureChange.AlphamaskFileName?.GetHashCode64() ?? 0, myTextureChange.ExtensionsFileName?.GetHashCode64() ?? 0, myTextureChange.ColorMetalFileName?.GetHashCode64() ?? 0, myTextureChange.NormalGlossFileName?.GetHashCode64() ?? 0).GetHashCode();
			}
			return num;
		}

		public MyModelInstance GetInstance(Dictionary<string, MyTextureChange> changes)
		{
			int hashCode = changes.GetHashCode();
			foreach (MyModelInstance modelInstance in m_modelInstances)
			{
				if (hashCode == modelInstance.RefHashCode)
				{
					return modelInstance;
				}
			}
			int changeContentHash = GetChangeContentHash(changes);
			foreach (MyModelInstance modelInstance2 in m_modelInstances)
			{
				if (changeContentHash == modelInstance2.ContentHashCode)
				{
					modelInstance2.RefHashCode = hashCode;
					return modelInstance2;
				}
			}
			MyModelInstance myModelInstance = new MyModelInstance(this, changes, changeContentHash);
			m_modelInstances.Add(myModelInstance);
			return myModelInstance;
		}

		public MyModelInstance GetInstance()
		{
			return m_modelInstances[0];
		}

		public void PreloadTextures()
		{
			if (!IsValid || m_lods.Count <= 0)
			{
				return;
			}
			List<MyPreprocessedPart> gBufferParts = m_lods[0].PreprocessedParts.GBufferParts;
			if (gBufferParts == null)
			{
				return;
			}
			foreach (MyPreprocessedPart item in gBufferParts)
			{
				IMyStreamedTexture[] textureHandles = item.Material.Binding.TextureHandles;
				if (textureHandles != null)
				{
					IMyStreamedTexture[] array = textureHandles;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Touch(100);
					}
				}
			}
		}
	}
}
