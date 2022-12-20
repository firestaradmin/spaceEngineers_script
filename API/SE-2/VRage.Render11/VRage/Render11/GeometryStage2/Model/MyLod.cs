using System.Collections.Generic;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.Model.Preprocess;
using VRage.Render11.Resources;
using VRageMath;

namespace VRage.Render11.GeometryStage2.Model
{
	internal class MyLod
	{
		private readonly MyPreprocessedParts m_preprocessedParts = new MyPreprocessedParts();

		private readonly MyLodInstanceMaterialsStrategy m_instanceMaterialsStrategy = new MyLodInstanceMaterialsStrategy();

		public string DebugName;

		public int LodNum { get; private set; }

		public IVertexBuffer VB0 { get; private set; }

		public IVertexBuffer VB1 { get; private set; }

		public IIndexBuffer IB { get; private set; }

		public BoundingBox BoundingBox { get; private set; }

		public MyPreprocessedParts PreprocessedParts => m_preprocessedParts;

		public int InstanceMaterialsCount => m_instanceMaterialsStrategy.InstanceMaterialCount;

		public void AddInstanceMaterial(string materialName, int modelOffset)
		{
			if (m_instanceMaterialsStrategy.ValidateMaterialName(materialName))
			{
				int instanceMaterialOffsetWithinLod = m_instanceMaterialsStrategy.AddInstanceMaterial(materialName, modelOffset);
				m_preprocessedParts.AddInstanceMaterial(materialName, instanceMaterialOffsetWithinLod);
			}
		}

		public List<int> GetInstanceMaterialOffsets()
		{
			return m_instanceMaterialsStrategy.InstanceMaterialOffsetsWithinLod;
		}

		public bool Create(MyMwmData mwmData, int lodNum, ref MyModelInstanceMaterialsStrategy modelInstanceMaterialsStrategy)
		{
			DebugName = mwmData.MwmFilepath;
			LodNum = lodNum;
			IB = MyLodUtils.CreateSimpleIB(mwmData);
			VB0 = MyLodUtils.CreateSimpleVB0(mwmData);
			VB1 = MyLodUtils.CreateSimpleVB1(mwmData);
			BoundingBox = mwmData.BoundindBox;
			m_preprocessedParts.Init(mwmData, this);
			m_instanceMaterialsStrategy.Init();
			foreach (MyMwmDataPart part in mwmData.Parts)
			{
				string materialName = part.MaterialName;
				modelInstanceMaterialsStrategy.GetOrAddInstanceMaterialOffset(materialName);
				m_instanceMaterialsStrategy.AddValidMaterial(part.MaterialName);
			}
			return true;
		}

		public void UnloadData()
		{
			MyManagers.Buffers.Dispose(IB);
			MyManagers.Buffers.Dispose(VB0);
			MyManagers.Buffers.Dispose(VB1);
		}
	}
}
