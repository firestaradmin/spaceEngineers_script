using System.Collections;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public class MyHudOreMarkers : IEnumerable<MyEntityOreDeposit>, IEnumerable
	{
		private readonly HashSet<MyEntityOreDeposit> m_markers = new HashSet<MyEntityOreDeposit>((IEqualityComparer<MyEntityOreDeposit>)MyEntityOreDeposit.Comparer);

		private string[] m_oreNames;

		public bool Visible { get; set; }

		public MyHudOreMarkers()
		{
			Visible = true;
			Reload();
		}

		internal void RegisterMarker(MyEntityOreDeposit deposit)
		{
			m_markers.Add(deposit);
		}

		internal void UnregisterMarker(MyEntityOreDeposit deposit)
		{
			m_markers.Remove(deposit);
		}

		internal void Clear()
		{
			m_markers.Clear();
		}

		public Enumerator<MyEntityOreDeposit> GetEnumerator()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return m_markers.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return (IEnumerator)(object)GetEnumerator();
		}

		IEnumerator<MyEntityOreDeposit> IEnumerable<MyEntityOreDeposit>.GetEnumerator()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return (IEnumerator<MyEntityOreDeposit>)(object)GetEnumerator();
		}

		public void Reload()
		{
			DictionaryValuesReader<string, MyVoxelMaterialDefinition> voxelMaterialDefinitions = MyDefinitionManager.Static.GetVoxelMaterialDefinitions();
			m_oreNames = new string[voxelMaterialDefinitions.Count];
			foreach (MyVoxelMaterialDefinition item in voxelMaterialDefinitions)
			{
				m_oreNames[item.Index] = MyTexts.GetString(MyStringId.GetOrCompute(item.MinedOre));
			}
		}

		public string GetOreName(MyVoxelMaterialDefinition def)
		{
			return m_oreNames[def.Index];
		}

		public void Reload()
		{
			DictionaryValuesReader<string, MyVoxelMaterialDefinition> voxelMaterialDefinitions = MyDefinitionManager.Static.GetVoxelMaterialDefinitions();
			m_oreNames = new string[voxelMaterialDefinitions.Count];
			foreach (MyVoxelMaterialDefinition item in voxelMaterialDefinitions)
			{
				m_oreNames[item.Index] = MyTexts.GetString(MyStringId.GetOrCompute(item.MinedOre));
			}
		}

		public string GetOreName(MyVoxelMaterialDefinition def)
		{
			return m_oreNames[def.Index];
		}
	}
}
