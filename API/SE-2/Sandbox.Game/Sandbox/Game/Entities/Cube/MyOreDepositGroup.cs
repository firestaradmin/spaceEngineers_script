using System;
using System.Collections.Generic;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	internal class MyOreDepositGroup
	{
		private readonly MyVoxelBase m_voxelMap;

		private readonly MyOreDetectorComponent m_orderDetectorComponent;

		private readonly Action<List<MyEntityOreDeposit>, List<Vector3I>> m_onDepositQueryComplete;

		private Dictionary<Vector3I, MyEntityOreDeposit> m_depositsByCellCoord_Main = new Dictionary<Vector3I, MyEntityOreDeposit>(Vector3I.Comparer);

		private Dictionary<Vector3I, MyEntityOreDeposit> m_depositsByCellCoord_Swap = new Dictionary<Vector3I, MyEntityOreDeposit>(Vector3I.Comparer);

		private Vector3I m_lastDetectionMin;

		private Vector3I m_lastDetectionMax;

		private int m_skippedQueries;

		private const int MAX_SKIPPED_QUERIES = 5;

		private int m_tasksRunning;

		public ICollection<MyEntityOreDeposit> Deposits => m_depositsByCellCoord_Main.Values;

		public void ClearMinMax()
		{
			m_lastDetectionMin = (m_lastDetectionMax = Vector3I.Zero);
		}

		public MyOreDepositGroup(MyVoxelBase voxelMap, MyOreDetectorComponent oreDetector)
		{
			m_voxelMap = voxelMap;
			m_orderDetectorComponent = oreDetector;
			m_onDepositQueryComplete = OnDepositQueryComplete;
			m_lastDetectionMax = new Vector3I(int.MinValue);
			m_lastDetectionMin = new Vector3I(int.MaxValue);
			m_skippedQueries = 0;
		}

		private void OnDepositQueryComplete(List<MyEntityOreDeposit> deposits, List<Vector3I> emptyCells)
		{
			foreach (MyEntityOreDeposit deposit in deposits)
			{
				Vector3I cellCoord = deposit.CellCoord;
				m_depositsByCellCoord_Swap[cellCoord] = deposit;
			}
			m_tasksRunning--;
			if (m_tasksRunning != 0)
<<<<<<< HEAD
			{
				return;
			}
			if (m_orderDetectorComponent.WillDiscardNextQuery)
			{
=======
			{
				return;
			}
			if (m_orderDetectorComponent.WillDiscardNextQuery)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				foreach (MyEntityOreDeposit value in m_depositsByCellCoord_Main.Values)
				{
					MyHud.OreMarkers.UnregisterMarker(value);
				}
				foreach (MyEntityOreDeposit value2 in m_depositsByCellCoord_Swap.Values)
				{
					MyHud.OreMarkers.UnregisterMarker(value2);
				}
				m_depositsByCellCoord_Main.Clear();
				m_depositsByCellCoord_Swap.Clear();
				return;
			}
			Dictionary<Vector3I, MyEntityOreDeposit> depositsByCellCoord_Main = m_depositsByCellCoord_Main;
			m_depositsByCellCoord_Main = m_depositsByCellCoord_Swap;
			m_depositsByCellCoord_Swap = depositsByCellCoord_Main;
			foreach (MyEntityOreDeposit value3 in m_depositsByCellCoord_Swap.Values)
			{
				MyHud.OreMarkers.UnregisterMarker(value3);
			}
			m_depositsByCellCoord_Swap.Clear();
			foreach (MyEntityOreDeposit value4 in m_depositsByCellCoord_Main.Values)
			{
				MyHud.OreMarkers.RegisterMarker(value4);
			}
		}

		public void UpdateDeposits(ref BoundingSphereD worldDetectionSphere, long detectorId)
		{
			if (m_tasksRunning != 0)
			{
				return;
			}
			MySession @static = MySession.Static;
			if (@static == null || !@static.Ready)
			{
				return;
			}
			Vector3D worldPosition = worldDetectionSphere.Center - worldDetectionSphere.Radius;
			Vector3D worldPosition2 = worldDetectionSphere.Center + worldDetectionSphere.Radius;
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(m_voxelMap.PositionLeftBottomCorner, ref worldPosition, out var voxelCoord);
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(m_voxelMap.PositionLeftBottomCorner, ref worldPosition2, out var voxelCoord2);
			voxelCoord += m_voxelMap.StorageMin;
			voxelCoord2 += m_voxelMap.StorageMin;
			m_voxelMap.Storage.ClampVoxelCoord(ref voxelCoord);
			m_voxelMap.Storage.ClampVoxelCoord(ref voxelCoord2);
			voxelCoord >>= 5;
			voxelCoord2 >>= 5;
			if (voxelCoord == m_lastDetectionMin && voxelCoord2 == m_lastDetectionMax && m_skippedQueries < 5)
			{
				m_skippedQueries++;
				return;
			}
			m_lastDetectionMin = voxelCoord;
			m_lastDetectionMax = voxelCoord2;
			m_skippedQueries = 0;
			int num = Math.Max((voxelCoord2.X - voxelCoord.X) / 2, 1);
			int num2 = Math.Max((voxelCoord2.Y - voxelCoord.Y) / 2, 1);
			Vector3I min = default(Vector3I);
			min.Z = voxelCoord.Z;
			Vector3I max = default(Vector3I);
			max.Z = voxelCoord2.Z;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					min.X = voxelCoord.X + i * num;
					min.Y = voxelCoord.Y + j * num2;
					max.X = min.X + num;
					max.Y = min.Y + num2;
					MyDepositQuery.Start(min, max, detectorId, m_voxelMap, m_onDepositQueryComplete);
					m_tasksRunning++;
				}
			}
		}

		internal void RemoveMarks()
		{
			foreach (MyEntityOreDeposit value in m_depositsByCellCoord_Main.Values)
			{
				MyHud.OreMarkers.UnregisterMarker(value);
			}
		}
	}
}
