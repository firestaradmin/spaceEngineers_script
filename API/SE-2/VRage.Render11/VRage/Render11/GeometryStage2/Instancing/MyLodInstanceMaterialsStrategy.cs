using System.Collections.Generic;

namespace VRage.Render11.GeometryStage2.Instancing
{
	internal class MyLodInstanceMaterialsStrategy
	{
		private readonly List<int> m_instanceMaterialOffsets = new List<int>();

		private readonly HashSet<string> m_allMaterialNames = new HashSet<string>();

		public int InstanceMaterialCount => m_instanceMaterialOffsets.Count;

		public List<int> InstanceMaterialOffsetsWithinLod => m_instanceMaterialOffsets;

		public void Init()
		{
			m_instanceMaterialOffsets.Clear();
			m_allMaterialNames.Clear();
		}

		public void AddValidMaterial(string materialName)
		{
			m_allMaterialNames.Add(materialName);
		}

		public bool ValidateMaterialName(string materialName)
		{
			return m_allMaterialNames.Contains(materialName);
		}

		public int AddInstanceMaterial(string materialName, int modelOffset)
		{
			int count = m_instanceMaterialOffsets.Count;
			m_instanceMaterialOffsets.Add(modelOffset);
			return count;
		}
	}
}
