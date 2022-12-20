using System.Collections.Generic;

namespace VRage.Render11.GeometryStage2.Instancing
{
	internal class MyModelInstanceMaterialsStrategy
	{
		private readonly Dictionary<string, int> m_offsets = new Dictionary<string, int>();

		private readonly List<bool> m_activeMaterials = new List<bool>();

		public int Count => m_offsets.Count;

		public int GetInstanceMaterialOffset(string materialName)
		{
			if (!m_offsets.ContainsKey(materialName))
			{
				return -1;
			}
			return m_offsets[materialName];
		}

		internal Dictionary<string, int>.Enumerator Enumerator()
		{
			return m_offsets.GetEnumerator();
		}

		public void Init()
		{
			m_offsets.Clear();
			m_activeMaterials.Clear();
		}

		public int GetOrAddInstanceMaterialOffset(string materialName)
		{
			if (m_offsets.TryGetValue(materialName, out var value))
			{
				return value;
			}
			value = m_offsets.Count;
			m_offsets.Add(materialName, value);
			m_activeMaterials.Add(item: false);
			return value;
		}

		public void ActivateInstanceMaterial(string materialName)
		{
			if (m_offsets.TryGetValue(materialName, out var value))
			{
				m_activeMaterials[value] = true;
			}
		}

		public bool IsInstanceMaterialActivated(string materialName)
		{
			if (!m_offsets.TryGetValue(materialName, out var value))
			{
				return false;
			}
			return m_activeMaterials[value];
		}

		public bool IsUsedMaterial(string materialName)
		{
			return m_offsets.ContainsKey(materialName);
		}
	}
}
