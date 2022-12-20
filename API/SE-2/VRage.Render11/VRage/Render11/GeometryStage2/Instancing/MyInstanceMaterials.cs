using System.Collections.Generic;
using VRage.Render11.GeometryStage2.Model;
using VRageMath.PackedVector;

namespace VRage.Render11.GeometryStage2.Instancing
{
	internal struct MyInstanceMaterials
	{
		private MyInstanceMaterial[] m_instanceMaterialsCache;

		private Dictionary<string, MyInstanceMaterial> m_dictionaryInstanceMaterials;

		public void Init()
		{
			if (m_dictionaryInstanceMaterials == null)
			{
				m_dictionaryInstanceMaterials = new Dictionary<string, MyInstanceMaterial>();
			}
			else
			{
				m_dictionaryInstanceMaterials.Clear();
			}
		}

		public int GetSize()
		{
			return m_instanceMaterialsCache.Length;
		}

		public MyInstanceMaterial GetInstanceMaterial(int index)
		{
			return m_instanceMaterialsCache[index];
		}

		public HalfVector4 GetInstanceMaterialPackedColorMultEmissivity(int index)
		{
			return m_instanceMaterialsCache[index].PackedColorMultEmissivity;
		}

		public void SetInstanceMaterial(string materialName, int index, MyInstanceMaterial instanceMaterial)
		{
			if (index != -1)
			{
				m_instanceMaterialsCache[index] = instanceMaterial;
			}
			if (!m_dictionaryInstanceMaterials.ContainsKey(materialName))
			{
				m_dictionaryInstanceMaterials.Add(materialName, instanceMaterial);
			}
			else
			{
				m_dictionaryInstanceMaterials[materialName] = instanceMaterial;
			}
		}

		public void SetDefaultInstanceMaterial(MyInstanceMaterial instanceMaterial)
		{
			for (int i = 0; i < m_instanceMaterialsCache.Length; i++)
			{
				m_instanceMaterialsCache[i] = instanceMaterial;
			}
			m_dictionaryInstanceMaterials.Clear();
		}

		public void OnReloadModel(MyModel model, MyInstanceMaterial defaultInstanceMaterial)
		{
			m_instanceMaterialsCache = new MyInstanceMaterial[model.GetAllMaterialsCount()];
			for (int i = 0; i < m_instanceMaterialsCache.Length; i++)
			{
				m_instanceMaterialsCache[i] = defaultInstanceMaterial;
			}
			Dictionary<string, int>.Enumerator instanceMaterialOffsetsEnumeratorInternal = model.GetInstanceMaterialOffsetsEnumeratorInternal();
			while (instanceMaterialOffsetsEnumeratorInternal.MoveNext())
			{
				KeyValuePair<string, int> current = instanceMaterialOffsetsEnumeratorInternal.Current;
				string key = current.Key;
				if (m_dictionaryInstanceMaterials.ContainsKey(key))
				{
					MyInstanceMaterial myInstanceMaterial = m_dictionaryInstanceMaterials[key];
					m_instanceMaterialsCache[current.Value] = myInstanceMaterial;
					model.ActivateInstanceMaterial(key);
				}
			}
		}

		public Dictionary<string, MyInstanceMaterial> GetInstanceMaterials()
		{
			return m_dictionaryInstanceMaterials;
		}
	}
}
