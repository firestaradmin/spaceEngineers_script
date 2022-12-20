using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage.FileSystem;
using VRage.Game;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
	public class MyVoxelPathfindingLog : IMyPathfindingLog
	{
		private abstract class Operation
		{
			public abstract void Perform();
		}

		private class NavMeshOp : Operation
		{
			private readonly string m_navmeshName;

			private readonly bool m_isAddition;

			private readonly Vector3I m_cellCoord;

			public NavMeshOp(string navmeshName, bool addition, Vector3I cellCoord)
			{
				m_navmeshName = navmeshName;
				m_isAddition = addition;
				m_cellCoord = cellCoord;
			}

			public override void Perform()
			{
				MyVoxelBase myVoxelBase = MySession.Static.VoxelMaps.TryGetVoxelMapByNameStart(m_navmeshName.Split(new char[1] { '-' })[0]);
				if (myVoxelBase != null && MyCestmirPathfindingShorts.Pathfinding.VoxelPathfinding.GetVoxelMapNavmesh(myVoxelBase) != null)
				{
					_ = m_isAddition;
				}
			}
		}

		private class VoxelWriteOp : Operation
		{
			private readonly string m_voxelName;

			private readonly string m_data;

			private readonly MyStorageDataTypeFlags m_dataType;

			private readonly Vector3I m_voxelMin;

			private readonly Vector3I m_voxelMax;

			public VoxelWriteOp(string voxelName, string data, MyStorageDataTypeFlags dataToWrite, Vector3I voxelRangeMin, Vector3I voxelRangeMax)
			{
				m_voxelName = voxelName;
				m_data = data;
				m_dataType = dataToWrite;
				m_voxelMin = voxelRangeMin;
				m_voxelMax = voxelRangeMax;
			}

			public override void Perform()
			{
				MySession.Static.VoxelMaps.TryGetVoxelMapByNameStart(m_voxelName)?.Storage.WriteRange(MyStorageData.FromBase64(m_data), m_dataType, m_voxelMin, m_voxelMax);
			}
		}

		private readonly string m_navmeshName;

		private readonly List<Operation> m_operations = new List<Operation>();

		private readonly MyLog m_log;

		public int Counter { get; private set; }

		public MyVoxelPathfindingLog(string filename)
		{
			string text = Path.Combine(MyFileSystem.UserDataPath, filename);
			if (MyFakes.REPLAY_NAVMESH_GENERATION)
			{
				StreamReader streamReader = new StreamReader(text);
				string text2 = null;
<<<<<<< HEAD
				string pattern = "NMOP: Voxel NavMesh: (\\S+) (ADD|REM) \\[X:(\\d+), Y:(\\d+), Z:(\\d+)\\]";
				string pattern2 = "VOXOP: (\\S*) \\[X:(\\d+), Y:(\\d+), Z:(\\d+)\\] \\[X:(\\d+), Y:(\\d+), Z:(\\d+)\\] (\\S+) (\\S+)";
				while ((text2 = streamReader.ReadLine()) != null)
				{
					text2.Split(new char[1] { '[' });
					MatchCollection matchCollection = Regex.Matches(text2, pattern);
					if (matchCollection.Count == 1)
					{
						string value = matchCollection[0].Groups[1].Value;
=======
				string text3 = "NMOP: Voxel NavMesh: (\\S+) (ADD|REM) \\[X:(\\d+), Y:(\\d+), Z:(\\d+)\\]";
				string text4 = "VOXOP: (\\S*) \\[X:(\\d+), Y:(\\d+), Z:(\\d+)\\] \\[X:(\\d+), Y:(\\d+), Z:(\\d+)\\] (\\S+) (\\S+)";
				while ((text2 = streamReader.ReadLine()) != null)
				{
					text2.Split(new char[1] { '[' });
					MatchCollection val = Regex.Matches(text2, text3);
					if (val.get_Count() == 1)
					{
						string value = ((Capture)val.get_Item(0).get_Groups().get_Item(1)).get_Value();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (m_navmeshName == null)
						{
							m_navmeshName = value;
						}
<<<<<<< HEAD
						bool addition = matchCollection[0].Groups[2].Value == "ADD";
						int x = int.Parse(matchCollection[0].Groups[3].Value);
						int y = int.Parse(matchCollection[0].Groups[4].Value);
						int z = int.Parse(matchCollection[0].Groups[5].Value);
=======
						bool addition = ((Capture)val.get_Item(0).get_Groups().get_Item(2)).get_Value() == "ADD";
						int x = int.Parse(((Capture)val.get_Item(0).get_Groups().get_Item(3)).get_Value());
						int y = int.Parse(((Capture)val.get_Item(0).get_Groups().get_Item(4)).get_Value());
						int z = int.Parse(((Capture)val.get_Item(0).get_Groups().get_Item(5)).get_Value());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						Vector3I cellCoord = new Vector3I(x, y, z);
						m_operations.Add(new NavMeshOp(m_navmeshName, addition, cellCoord));
						continue;
					}
<<<<<<< HEAD
					matchCollection = Regex.Matches(text2, pattern2);
					if (matchCollection.Count == 1)
					{
						string value2 = matchCollection[0].Groups[1].Value;
						int x2 = int.Parse(matchCollection[0].Groups[2].Value);
						int y2 = int.Parse(matchCollection[0].Groups[3].Value);
						int z2 = int.Parse(matchCollection[0].Groups[4].Value);
						Vector3I voxelRangeMin = new Vector3I(x2, y2, z2);
						x2 = int.Parse(matchCollection[0].Groups[5].Value);
						y2 = int.Parse(matchCollection[0].Groups[6].Value);
						z2 = int.Parse(matchCollection[0].Groups[7].Value);
						Vector3I voxelRangeMax = new Vector3I(x2, y2, z2);
						MyStorageDataTypeFlags dataToWrite = (MyStorageDataTypeFlags)Enum.Parse(typeof(MyStorageDataTypeFlags), matchCollection[0].Groups[8].Value);
						string value3 = matchCollection[0].Groups[9].Value;
=======
					val = Regex.Matches(text2, text4);
					if (val.get_Count() == 1)
					{
						string value2 = ((Capture)val.get_Item(0).get_Groups().get_Item(1)).get_Value();
						int x2 = int.Parse(((Capture)val.get_Item(0).get_Groups().get_Item(2)).get_Value());
						int y2 = int.Parse(((Capture)val.get_Item(0).get_Groups().get_Item(3)).get_Value());
						int z2 = int.Parse(((Capture)val.get_Item(0).get_Groups().get_Item(4)).get_Value());
						Vector3I voxelRangeMin = new Vector3I(x2, y2, z2);
						x2 = int.Parse(((Capture)val.get_Item(0).get_Groups().get_Item(5)).get_Value());
						y2 = int.Parse(((Capture)val.get_Item(0).get_Groups().get_Item(6)).get_Value());
						z2 = int.Parse(((Capture)val.get_Item(0).get_Groups().get_Item(7)).get_Value());
						Vector3I voxelRangeMax = new Vector3I(x2, y2, z2);
						MyStorageDataTypeFlags dataToWrite = (MyStorageDataTypeFlags)Enum.Parse(typeof(MyStorageDataTypeFlags), ((Capture)val.get_Item(0).get_Groups().get_Item(8)).get_Value());
						string value3 = ((Capture)val.get_Item(0).get_Groups().get_Item(9)).get_Value();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						m_operations.Add(new VoxelWriteOp(value2, value3, dataToWrite, voxelRangeMin, voxelRangeMax));
					}
				}
				streamReader.Close();
			}
			if (MyFakes.LOG_NAVMESH_GENERATION)
			{
				m_log = new MyLog();
				m_log.Init(text, MyFinalBuildConstants.APP_VERSION_STRING);
			}
		}

		public void Close()
		{
			m_log?.Close();
		}

		public void LogCellAddition(MyVoxelNavigationMesh navMesh, Vector3I cell)
		{
			m_log.WriteLine(string.Concat("NMOP: ", navMesh, " ADD ", cell));
		}

		public void LogCellRemoval(MyVoxelNavigationMesh navMesh, Vector3I cell)
		{
			m_log.WriteLine(string.Concat("NMOP: ", navMesh, " REM ", cell));
		}

		public void LogStorageWrite(MyVoxelBase map, MyStorageData source, MyStorageDataTypeFlags dataToWrite, Vector3I voxelRangeMin, Vector3I voxelRangeMax)
		{
			string text = source.ToBase64();
			m_log.WriteLine($"VOXOP: {map.StorageName} {voxelRangeMin} {voxelRangeMax} {dataToWrite} {text}");
		}

		public void PerformOneOperation(bool triggerPressed)
		{
			if ((triggerPressed || Counter <= int.MaxValue) && Counter < m_operations.Count)
			{
				m_operations[Counter].Perform();
				Counter++;
			}
		}

		public void DebugDraw()
		{
			if (MyFakes.REPLAY_NAVMESH_GENERATION)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(500f, 10f), $"Next operation: {Counter}/{m_operations.Count}", Color.Red, 1f);
			}
		}
	}
}
