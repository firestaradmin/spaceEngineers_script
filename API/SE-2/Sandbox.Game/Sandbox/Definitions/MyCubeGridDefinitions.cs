using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[PreloadRequired]
	public static class MyCubeGridDefinitions
	{
		public class TableEntry
		{
			public MyRotationOptionsEnum RotationOptions;

			public MyTileDefinition[] Tiles;

			public MyEdgeDefinition[] Edges;
		}

		public static readonly Dictionary<MyStringId, Dictionary<Vector3I, MyTileDefinition>> TileGridOrientations;

		public static readonly Dictionary<Vector3I, MyEdgeOrientationInfo> EdgeOrientations;

		private static TableEntry[] m_tileTable;

		private static MatrixI[] m_allPossible90rotations;

		private static MatrixI[][] m_uniqueTopologyRotationTable;

		public static MatrixI[] AllPossible90rotations => m_allPossible90rotations;

		static MyCubeGridDefinitions()
		{
			Dictionary<MyStringId, Dictionary<Vector3I, MyTileDefinition>> dictionary = new Dictionary<MyStringId, Dictionary<Vector3I, MyTileDefinition>>();
			MyStringId orCompute = MyStringId.GetOrCompute("Square");
			Dictionary<Vector3I, MyTileDefinition> dictionary2 = new Dictionary<Vector3I, MyTileDefinition>();
			Vector3I up = Vector3I.Up;
			MyTileDefinition myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
				Normal = Vector3.Up,
				FullQuad = true
			};
			dictionary2.Add(up, myTileDefinition);
			Vector3I forward = Vector3I.Forward;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true
			};
			dictionary2.Add(forward, myTileDefinition);
			Vector3I backward = Vector3I.Backward;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
				Normal = Vector3.Backward,
				FullQuad = true
			};
			dictionary2.Add(backward, myTileDefinition);
			Vector3I down = Vector3I.Down;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
				Normal = Vector3.Down,
				FullQuad = true
			};
			dictionary2.Add(down, myTileDefinition);
			Vector3I right = Vector3I.Right;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Right),
				Normal = Vector3.Right,
				FullQuad = true
			};
			dictionary2.Add(right, myTileDefinition);
			Vector3I left = Vector3I.Left;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
				Normal = Vector3.Left,
				FullQuad = true
			};
			dictionary2.Add(left, myTileDefinition);
			dictionary.Add(orCompute, dictionary2);
			MyStringId orCompute2 = MyStringId.GetOrCompute("Slope");
			Dictionary<Vector3I, MyTileDefinition> dictionary3 = new Dictionary<Vector3I, MyTileDefinition>();
			Vector3I key = new Vector3I(0, 1, 1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(0f, 1f, 1f)),
				IsEmpty = true
			};
			dictionary3.Add(key, myTileDefinition);
			Vector3I key2 = new Vector3I(0, 1, -1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Up),
				Normal = Vector3.Normalize(new Vector3(0f, 1f, -1f)),
				IsEmpty = true
			};
			dictionary3.Add(key2, myTileDefinition);
			Vector3I key3 = new Vector3I(-1, 1, 0);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
				Normal = Vector3.Normalize(new Vector3(1f, 1f, 0f)),
				IsEmpty = true
			};
			dictionary3.Add(key3, myTileDefinition);
			Vector3I key4 = new Vector3I(1, 1, 0);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Up),
				Normal = Vector3.Normalize(new Vector3(-1f, 1f, 0f)),
				IsEmpty = true
			};
			dictionary3.Add(key4, myTileDefinition);
			Vector3I key5 = new Vector3I(0, -1, 1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Down),
				Normal = Vector3.Normalize(new Vector3(0f, 1f, 1f)),
				IsEmpty = true
			};
			dictionary3.Add(key5, myTileDefinition);
			Vector3I key6 = new Vector3I(0, -1, -1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Down),
				Normal = Vector3.Normalize(new Vector3(0f, 1f, -1f)),
				IsEmpty = true
			};
			dictionary3.Add(key6, myTileDefinition);
			Vector3I key7 = new Vector3I(-1, -1, 0);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Down),
				Normal = Vector3.Normalize(new Vector3(1f, 1f, 0f)),
				IsEmpty = true
			};
			dictionary3.Add(key7, myTileDefinition);
			Vector3I key8 = new Vector3I(1, -1, 0);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
				Normal = Vector3.Normalize(new Vector3(-1f, 1f, 0f)),
				IsEmpty = true
			};
			dictionary3.Add(key8, myTileDefinition);
			Vector3I key9 = new Vector3I(-1, 0, -1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Forward),
				Normal = Vector3.Normalize(new Vector3(0f, 1f, 1f)),
				IsEmpty = true
			};
			dictionary3.Add(key9, myTileDefinition);
			Vector3I key10 = new Vector3I(-1, 0, 1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Backward),
				Normal = Vector3.Normalize(new Vector3(0f, 1f, -1f)),
				IsEmpty = true
			};
			dictionary3.Add(key10, myTileDefinition);
			Vector3I key11 = new Vector3I(1, 0, -1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Forward),
				Normal = Vector3.Normalize(new Vector3(1f, 1f, 0f)),
				IsEmpty = true
			};
			dictionary3.Add(key11, myTileDefinition);
			Vector3I key12 = new Vector3I(1, 0, 1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Backward),
				Normal = Vector3.Normalize(new Vector3(-1f, 1f, 0f)),
				IsEmpty = true
			};
			dictionary3.Add(key12, myTileDefinition);
			dictionary.Add(orCompute2, dictionary3);
			TileGridOrientations = dictionary;
			EdgeOrientations = new Dictionary<Vector3I, MyEdgeOrientationInfo>(new Vector3INormalEqualityComparer())
			{
				{
					new Vector3I(0, 0, 1),
					new MyEdgeOrientationInfo(Matrix.Identity, MyCubeEdgeType.Horizontal)
				},
				{
					new Vector3I(0, 0, -1),
					new MyEdgeOrientationInfo(Matrix.Identity, MyCubeEdgeType.Horizontal)
				},
				{
					new Vector3I(1, 0, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationY((float)Math.E * 449f / 777f), MyCubeEdgeType.Horizontal)
				},
				{
					new Vector3I(-1, 0, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationY((float)Math.E * 449f / 777f), MyCubeEdgeType.Horizontal)
				},
				{
					new Vector3I(0, 1, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationX((float)Math.E * 449f / 777f), MyCubeEdgeType.Vertical)
				},
				{
					new Vector3I(0, -1, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationX((float)Math.E * 449f / 777f), MyCubeEdgeType.Vertical)
				},
				{
					new Vector3I(-1, 0, -1),
					new MyEdgeOrientationInfo(Matrix.CreateRotationZ((float)Math.E * 449f / 777f), MyCubeEdgeType.Horizontal_Diagonal)
				},
				{
					new Vector3I(1, 0, 1),
					new MyEdgeOrientationInfo(Matrix.CreateRotationZ((float)Math.E * 449f / 777f), MyCubeEdgeType.Horizontal_Diagonal)
				},
				{
					new Vector3I(-1, 0, 1),
					new MyEdgeOrientationInfo(Matrix.CreateRotationZ((float)Math.E * -449f / 777f), MyCubeEdgeType.Horizontal_Diagonal)
				},
				{
					new Vector3I(1, 0, -1),
					new MyEdgeOrientationInfo(Matrix.CreateRotationZ((float)Math.E * -449f / 777f), MyCubeEdgeType.Horizontal_Diagonal)
				},
				{
					new Vector3I(0, 1, -1),
					new MyEdgeOrientationInfo(Matrix.Identity, MyCubeEdgeType.Vertical_Diagonal)
				},
				{
					new Vector3I(0, -1, 1),
					new MyEdgeOrientationInfo(Matrix.Identity, MyCubeEdgeType.Vertical_Diagonal)
				},
				{
					new Vector3I(-1, -1, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationY((float)Math.E * -449f / 777f), MyCubeEdgeType.Vertical_Diagonal)
				},
				{
					new Vector3I(0, -1, -1),
					new MyEdgeOrientationInfo(Matrix.CreateRotationX((float)Math.E * 449f / 777f), MyCubeEdgeType.Vertical_Diagonal)
				},
				{
					new Vector3I(1, -1, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationY((float)Math.E * 449f / 777f), MyCubeEdgeType.Vertical_Diagonal)
				},
				{
					new Vector3I(-1, 1, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationY((float)Math.E * 449f / 777f), MyCubeEdgeType.Vertical_Diagonal)
				},
				{
					new Vector3I(1, 1, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationY((float)Math.E * -449f / 777f), MyCubeEdgeType.Vertical_Diagonal)
				},
				{
					new Vector3I(0, 1, 1),
					new MyEdgeOrientationInfo(Matrix.CreateRotationX((float)Math.E * 449f / 777f), MyCubeEdgeType.Vertical_Diagonal)
				}
			};
			m_tileTable = new TableEntry[Enum.GetNames(typeof(MyCubeTopology)).Length];
			TableEntry[] tileTable = m_tileTable;
			TableEntry tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.None
			};
			TableEntry tableEntry2 = tableEntry;
			MyTileDefinition[] array = new MyTileDefinition[6];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
				Normal = Vector3.Up,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
				Normal = Vector3.Backward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
				Normal = Vector3.Down,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Right),
				Normal = Vector3.Right,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
				Normal = Vector3.Left,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array[5] = myTileDefinition;
			tableEntry2.Tiles = array;
			TableEntry tableEntry3 = tableEntry;
			MyEdgeDefinition[] array2 = new MyEdgeDefinition[12];
			MyEdgeDefinition myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, 1f, -1f),
				Point1 = new Vector3(1f, 1f, -1f),
				Side0 = 0,
				Side1 = 1
			};
			array2[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, 1f, -1f),
				Point1 = new Vector3(-1f, 1f, 1f),
				Side0 = 0,
				Side1 = 5
			};
			array2[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, 1f, -1f),
				Point1 = new Vector3(1f, 1f, 1f),
				Side0 = 0,
				Side1 = 4
			};
			array2[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, 1f, 1f),
				Point1 = new Vector3(1f, 1f, 1f),
				Side0 = 0,
				Side1 = 2
			};
			array2[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(1f, -1f, -1f),
				Side0 = 3,
				Side1 = 1
			};
			array2[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(-1f, -1f, 1f),
				Side0 = 3,
				Side1 = 5
			};
			array2[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, -1f),
				Point1 = new Vector3(1f, -1f, 1f),
				Side0 = 3,
				Side1 = 4
			};
			array2[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, 1f),
				Point1 = new Vector3(1f, -1f, 1f),
				Side0 = 3,
				Side1 = 2
			};
			array2[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, 1f, -1f),
				Point1 = new Vector3(-1f, -1f, -1f),
				Side0 = 1,
				Side1 = 5
			};
			array2[8] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, 1f, -1f),
				Point1 = new Vector3(1f, -1f, -1f),
				Side0 = 1,
				Side1 = 4
			};
			array2[9] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, 1f, 1f),
				Point1 = new Vector3(-1f, -1f, 1f),
				Side0 = 5,
				Side1 = 2
			};
			array2[10] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, 1f, 1f),
				Point1 = new Vector3(1f, -1f, 1f),
				Side0 = 4,
				Side1 = 2
			};
			array2[11] = myEdgeDefinition;
			tableEntry3.Edges = array2;
			tileTable[0] = tableEntry;
			TableEntry[] tileTable2 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry4 = tableEntry;
			MyTileDefinition[] array3 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(0f, 1f, 1f)),
				IsEmpty = true,
				Id = MyStringId.GetOrCompute("Slope")
			};
			array3[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Left,
				Up = new Vector3(0f, -1f, -1f)
			};
			array3[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, -1f)
			};
			array3[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(3.141593f),
				Normal = Vector3.Down,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array3[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array3[4] = myTileDefinition;
			tableEntry4.Tiles = array3;
			TableEntry tableEntry5 = tableEntry;
			MyEdgeDefinition[] array4 = new MyEdgeDefinition[9];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 4
			};
			array4[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 0,
				Side1 = 1
			};
			array4[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 0,
				Side1 = 2
			};
			array4[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 0,
				Side1 = 3
			};
			array4[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 4
			};
			array4[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array4[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 3
			};
			array4[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array4[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 2,
				Side1 = 4
			};
			array4[8] = myEdgeDefinition;
			tableEntry5.Edges = array4;
			tileTable2[1] = tableEntry;
			TableEntry[] tileTable3 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry6 = tableEntry;
			MyTileDefinition[] array5 = new MyTileDefinition[4];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationY((float)Math.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(1f, -1f, 0f)
			};
			array5[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, -1f)
			};
			array5[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(-1f, 1f, 1f)),
				IsEmpty = true
			};
			array5[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ((float)Math.E * 449f / 777f),
				Normal = Vector3.Down,
				Up = new Vector3(1f, 0f, -1f)
			};
			array5[3] = myTileDefinition;
			tableEntry6.Tiles = array5;
			TableEntry tableEntry7 = tableEntry;
			MyEdgeDefinition[] array6 = new MyEdgeDefinition[6];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 0,
				Side1 = 1
			};
			array6[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 0,
				Side1 = 3
			};
			array6[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 2
			};
			array6[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 1
			};
			array6[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array6[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 3
			};
			array6[5] = myEdgeDefinition;
			tableEntry7.Edges = array6;
			tileTable3[2] = tableEntry;
			TableEntry[] tileTable4 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry8 = tableEntry;
			MyTileDefinition[] array7 = new MyTileDefinition[7];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(1f, -1f, -1f)),
				IsEmpty = true
			};
			array7[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Right,
				Up = new Vector3(0f, 1f, 1f)
			};
			array7[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f) * Matrix.CreateRotationY((float)Math.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(-1f, 1f, 0f)
			};
			array7[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ((float)Math.E * -449f / 777f) * Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Down,
				Up = new Vector3(-1f, 0f, 1f)
			};
			array7[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Up,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array7[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ((float)Math.E * 449f / 777f),
				Normal = Vector3.Left,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array7[5] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX((float)Math.E * 449f / 777f),
				Normal = Vector3.Backward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array7[6] = myTileDefinition;
			tableEntry8.Tiles = array7;
			TableEntry tableEntry9 = tableEntry;
			MyEdgeDefinition[] array8 = new MyEdgeDefinition[12];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 2,
				Side1 = 4
			};
			array8[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 2,
				Side1 = 5
			};
			array8[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 2,
				Side1 = 0
			};
			array8[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 4,
				Side1 = 1
			};
			array8[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 6,
				Side1 = 1
			};
			array8[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, 1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 1
			};
			array8[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 0,
				Side1 = 3
			};
			array8[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 6,
				Side1 = 3
			};
			array8[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 5,
				Side1 = 3
			};
			array8[8] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 5,
				Side1 = 6
			};
			array8[9] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, 1, -1),
				Side0 = 5,
				Side1 = 4
			};
			array8[10] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 4,
				Side1 = 6
			};
			array8[11] = myEdgeDefinition;
			tableEntry9.Edges = array8;
			tileTable4[3] = tableEntry;
			TableEntry[] tileTable5 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry10 = tableEntry;
			MyTileDefinition[] array9 = new MyTileDefinition[6];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Right),
				Normal = Vector3.Right,
				FullQuad = true
			};
			array9[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
				Normal = Vector3.Up,
				FullQuad = true
			};
			array9[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true
			};
			array9[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Left),
				Normal = Vector3.Left,
				FullQuad = true
			};
			array9[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
				Normal = Vector3.Down,
				FullQuad = true
			};
			array9[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
				Normal = Vector3.Backward,
				FullQuad = true
			};
			array9[5] = myTileDefinition;
			tableEntry10.Tiles = array9;
			TableEntry tableEntry11 = tableEntry;
			MyEdgeDefinition[] array10 = new MyEdgeDefinition[12];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 1
			};
			array10[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, 1, 1),
				Side0 = 0,
				Side1 = 5
			};
			array10[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 0,
				Side1 = 4
			};
			array10[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 0,
				Side1 = 2
			};
			array10[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 1
			};
			array10[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 3,
				Side1 = 5
			};
			array10[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 3,
				Side1 = 4
			};
			array10[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 3,
				Side1 = 2
			};
			array10[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 5
			};
			array10[8] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array10[9] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 5,
				Side1 = 2
			};
			array10[10] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 4,
				Side1 = 2
			};
			array10[11] = myEdgeDefinition;
			tableEntry11.Edges = array10;
			tileTable5[4] = tableEntry;
			TableEntry[] tileTable6 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry12 = tableEntry;
			MyTileDefinition[] array11 = new MyTileDefinition[6];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
				Normal = Vector3.Up,
				FullQuad = true
			};
			array11[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true
			};
			array11[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
				Normal = Vector3.Backward,
				FullQuad = true
			};
			array11[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
				Normal = Vector3.Down,
				FullQuad = true
			};
			array11[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Right),
				Normal = Vector3.Right,
				FullQuad = true
			};
			array11[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
				Normal = Vector3.Left,
				FullQuad = true
			};
			array11[5] = myTileDefinition;
			tableEntry12.Tiles = array11;
			TableEntry tableEntry13 = tableEntry;
			MyEdgeDefinition[] array12 = new MyEdgeDefinition[12];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 1
			};
			array12[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, 1, 1),
				Side0 = 0,
				Side1 = 5
			};
			array12[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 0,
				Side1 = 4
			};
			array12[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 0,
				Side1 = 2
			};
			array12[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 1
			};
			array12[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 3,
				Side1 = 5
			};
			array12[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 3,
				Side1 = 4
			};
			array12[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 3,
				Side1 = 2
			};
			array12[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 5
			};
			array12[8] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array12[9] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 5,
				Side1 = 2
			};
			array12[10] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 4,
				Side1 = 2
			};
			array12[11] = myEdgeDefinition;
			tableEntry13.Edges = array12;
			tileTable6[5] = tableEntry;
			TableEntry[] tileTable7 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry14 = tableEntry;
			MyTileDefinition[] array13 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(0f, 1f, 1f)),
				IsEmpty = true,
				Id = MyStringId.GetOrCompute("Slope")
			};
			array13[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Left,
				Up = new Vector3(0f, -1f, -1f),
				IsRounded = true
			};
			array13[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, -1f),
				IsRounded = true
			};
			array13[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(3.141593f),
				Normal = Vector3.Down,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array13[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array13[4] = myTileDefinition;
			tableEntry14.Tiles = array13;
			TableEntry tableEntry15 = tableEntry;
			MyEdgeDefinition[] array14 = new MyEdgeDefinition[7];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 4
			};
			array14[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 0,
				Side1 = 3
			};
			array14[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 4
			};
			array14[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array14[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 3
			};
			array14[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array14[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 2,
				Side1 = 4
			};
			array14[6] = myEdgeDefinition;
			tableEntry15.Edges = array14;
			tileTable7[6] = tableEntry;
			TableEntry[] tileTable8 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry16 = tableEntry;
			MyTileDefinition[] array15 = new MyTileDefinition[4];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationY((float)Math.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(1f, -1f, 0f),
				IsRounded = true
			};
			array15[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, -1f),
				IsRounded = true
			};
			array15[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(-1f, 1f, 1f)),
				IsEmpty = true
			};
			array15[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ((float)Math.E * 449f / 777f),
				Normal = Vector3.Down,
				Up = new Vector3(1f, 0f, -1f),
				IsRounded = true
			};
			array15[3] = myTileDefinition;
			tableEntry16.Tiles = array15;
			TableEntry tableEntry17 = tableEntry;
			MyEdgeDefinition[] array16 = new MyEdgeDefinition[3];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 0,
				Side1 = 1
			};
			array16[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 0,
				Side1 = 3
			};
			array16[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array16[2] = myEdgeDefinition;
			tableEntry17.Edges = array16;
			tileTable8[7] = tableEntry;
			TableEntry[] tileTable9 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry18 = tableEntry;
			MyTileDefinition[] array17 = new MyTileDefinition[7];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ((float)Math.E * -449f / 777f) * Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Down,
				Up = new Vector3(-1f, 0f, 1f),
				IsRounded = true
			};
			array17[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Right,
				Up = new Vector3(0f, 1f, 1f),
				IsRounded = true
			};
			array17[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f) * Matrix.CreateRotationY((float)Math.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(-1f, 1f, 0f),
				IsRounded = true
			};
			array17[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Up,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array17[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ((float)Math.E * 449f / 777f),
				Normal = Vector3.Left,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array17[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX((float)Math.E * 449f / 777f),
				Normal = Vector3.Backward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array17[5] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(1f, -1f, -1f)),
				IsEmpty = true
			};
			array17[6] = myTileDefinition;
			tableEntry18.Tiles = array17;
			TableEntry tableEntry19 = tableEntry;
			MyEdgeDefinition[] array18 = new MyEdgeDefinition[9];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 2,
				Side1 = 3
			};
			array18[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 2,
				Side1 = 4
			};
			array18[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 3,
				Side1 = 1
			};
			array18[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 5,
				Side1 = 1
			};
			array18[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 5,
				Side1 = 0
			};
			array18[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 4,
				Side1 = 0
			};
			array18[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 4,
				Side1 = 5
			};
			array18[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, 1, -1),
				Side0 = 4,
				Side1 = 3
			};
			array18[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 3,
				Side1 = 5
			};
			array18[8] = myEdgeDefinition;
			tableEntry19.Edges = array18;
			tileTable9[8] = tableEntry;
			TableEntry[] tileTable10 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry20 = tableEntry;
			MyTileDefinition[] array19 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(0f, 1f, 1f)),
				IsEmpty = true
			};
			array19[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Left,
				Up = new Vector3(0f, -1f, -1f)
			};
			array19[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, -1f)
			};
			array19[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(3.141593f),
				Normal = Vector3.Down,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array19[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array19[4] = myTileDefinition;
			tableEntry20.Tiles = array19;
			TableEntry tableEntry21 = tableEntry;
			MyEdgeDefinition[] array20 = new MyEdgeDefinition[9];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 4
			};
			array20[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 0,
				Side1 = 1
			};
			array20[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 0,
				Side1 = 2
			};
			array20[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 0,
				Side1 = 3
			};
			array20[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 4
			};
			array20[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array20[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 3
			};
			array20[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array20[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 2,
				Side1 = 4
			};
			array20[8] = myEdgeDefinition;
			tableEntry21.Edges = array20;
			tileTable10[9] = tableEntry;
			TableEntry[] tileTable11 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry22 = tableEntry;
			MyTileDefinition[] array21 = new MyTileDefinition[4];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationY((float)Math.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(1f, -1f, 0f)
			};
			array21[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, -1f)
			};
			array21[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(-1f, 1f, 1f)),
				IsEmpty = true
			};
			array21[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ((float)Math.E * 449f / 777f),
				Normal = Vector3.Down,
				Up = new Vector3(1f, 0f, -1f)
			};
			array21[3] = myTileDefinition;
			tableEntry22.Tiles = array21;
			TableEntry tableEntry23 = tableEntry;
			MyEdgeDefinition[] array22 = new MyEdgeDefinition[6];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 0,
				Side1 = 1
			};
			array22[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 0,
				Side1 = 3
			};
			array22[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 2
			};
			array22[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 1
			};
			array22[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array22[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 3
			};
			array22[5] = myEdgeDefinition;
			tableEntry23.Edges = array22;
			tileTable11[10] = tableEntry;
			TableEntry[] tileTable12 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry24 = tableEntry;
			MyTileDefinition[] array23 = new MyTileDefinition[6];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up),
				Normal = Vector3.Normalize(new Vector3(0f, 2f, 1f)),
				IsEmpty = true
			};
			array23[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array23[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
				Normal = Vector3.Backward
			};
			array23[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
				Normal = Vector3.Down,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array23[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Right),
				Normal = Vector3.Right,
				Up = new Vector3(0f, -2f, -1f),
				DontOffsetTexture = true
			};
			array23[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
				Normal = Vector3.Left,
				Up = new Vector3(0f, -2f, -1f),
				DontOffsetTexture = true
			};
			array23[5] = myTileDefinition;
			tableEntry24.Tiles = array23;
			TableEntry tableEntry25 = tableEntry;
			MyEdgeDefinition[] array24 = new MyEdgeDefinition[7];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, 1f, -1f),
				Point1 = new Vector3(1f, 1f, -1f),
				Side0 = 0,
				Side1 = 1
			};
			array24[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 1
			};
			array24[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 3,
				Side1 = 5
			};
			array24[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 3,
				Side1 = 4
			};
			array24[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 3,
				Side1 = 2
			};
			array24[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 5
			};
			array24[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array24[6] = myEdgeDefinition;
			tableEntry25.Edges = array24;
			tileTable12[11] = tableEntry;
			TableEntry[] tileTable13 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry26 = tableEntry;
			MyTileDefinition[] array25 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(0f, 2f, 1f)),
				IsEmpty = true
			};
			array25[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Left,
				Up = new Vector3(0f, -2f, -1f),
				IsEmpty = true
			};
			array25[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -2f, -1f),
				IsEmpty = true
			};
			array25[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(3.141593f),
				Normal = Vector3.Down,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array25[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward
			};
			array25[4] = myTileDefinition;
			tableEntry26.Tiles = array25;
			TableEntry tableEntry27 = tableEntry;
			MyEdgeDefinition[] array26 = new MyEdgeDefinition[4];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 0,
				Side1 = 3
			};
			array26[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 4
			};
			array26[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array26[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 3
			};
			array26[3] = myEdgeDefinition;
			tableEntry27.Edges = array26;
			tileTable13[12] = tableEntry;
			TableEntry[] tileTable14 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry28 = tableEntry;
			MyTileDefinition[] array27 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(2f, 1f, 1f)),
				IsEmpty = true,
				DontOffsetTexture = true
			};
			array27[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Left,
				Up = new Vector3(0f, 1f, 1f),
				DontOffsetTexture = true
			};
			array27[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, 1f),
				IsEmpty = true,
				DontOffsetTexture = true
			};
			array27[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(3.141593f),
				Normal = Vector3.Down,
				Up = new Vector3(-2f, 0f, 1f),
				DontOffsetTexture = true
			};
			array27[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				Up = new Vector3(-2f, 1f, 0f),
				DontOffsetTexture = true
			};
			array27[4] = myTileDefinition;
			tableEntry28.Tiles = array27;
			TableEntry tableEntry29 = tableEntry;
			MyEdgeDefinition[] array28 = new MyEdgeDefinition[4];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 0,
				Side1 = 1
			};
			array28[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 4
			};
			array28[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array28[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array28[3] = myEdgeDefinition;
			tableEntry29.Edges = array28;
			tileTable14[13] = tableEntry;
			TableEntry[] tileTable15 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry30 = tableEntry;
			MyTileDefinition[] array29 = new MyTileDefinition[4];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationY((float)Math.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(1f, -2f, 0f),
				IsEmpty = true
			};
			array29[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -2f, -1f),
				IsEmpty = true
			};
			array29[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(-1f, 2f, 1f)),
				IsEmpty = true
			};
			array29[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ((float)Math.E * 449f / 777f),
				Normal = Vector3.Down,
				Up = new Vector3(1f, 0f, -1f),
				IsEmpty = true
			};
			array29[3] = myTileDefinition;
			tableEntry30.Tiles = array29;
			TableEntry tableEntry31 = tableEntry;
			MyEdgeDefinition[] array30 = new MyEdgeDefinition[1];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array30[0] = myEdgeDefinition;
			tableEntry31.Edges = array30;
			tileTable15[14] = tableEntry;
			TableEntry[] tileTable16 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry32 = tableEntry;
			MyTileDefinition[] array31 = new MyTileDefinition[7];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(2f, -2f, -1f)),
				IsEmpty = true
			};
			array31[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, 2f)
			};
			array31[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f) * Matrix.CreateRotationY((float)Math.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(2f, 0f, -1f)
			};
			array31[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ((float)Math.E * -449f / 777f) * Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Down,
				Up = new Vector3(1f, 0f, 2f)
			};
			array31[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Up,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array31[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ((float)Math.E * 449f / 777f),
				Normal = Vector3.Left,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array31[5] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX((float)Math.E * 449f / 777f),
				Normal = Vector3.Backward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array31[6] = myTileDefinition;
			tableEntry32.Tiles = array31;
			TableEntry tableEntry33 = tableEntry;
			MyEdgeDefinition[] array32 = new MyEdgeDefinition[9];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 2,
				Side1 = 4
			};
			array32[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 2,
				Side1 = 5
			};
			array32[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 4,
				Side1 = 1
			};
			array32[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 6,
				Side1 = 1
			};
			array32[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 6,
				Side1 = 3
			};
			array32[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 5,
				Side1 = 3
			};
			array32[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 5,
				Side1 = 6
			};
			array32[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, 1, -1),
				Side0 = 5,
				Side1 = 4
			};
			array32[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 4,
				Side1 = 6
			};
			array32[8] = myEdgeDefinition;
			tableEntry33.Edges = array32;
			tileTable16[15] = tableEntry;
			TableEntry[] tileTable17 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry34 = tableEntry;
			MyTileDefinition[] array33 = new MyTileDefinition[7];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(2f, -2f, -1f)),
				IsEmpty = true
			};
			array33[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Right,
				Up = new Vector3(0f, 1f, 1f)
			};
			array33[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f) * Matrix.CreateRotationY((float)Math.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(0f, -2f, -1f)
			};
			array33[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ((float)Math.E * -449f / 777f) * Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Down,
				Up = new Vector3(2f, 0f, -1f)
			};
			array33[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Up,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array33[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ((float)Math.E * 449f / 777f),
				Normal = Vector3.Left,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array33[5] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX((float)Math.E * 449f / 777f),
				Normal = Vector3.Backward
			};
			array33[6] = myTileDefinition;
			tableEntry34.Tiles = array33;
			TableEntry tableEntry35 = tableEntry;
			MyEdgeDefinition[] array34 = new MyEdgeDefinition[7];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 2,
				Side1 = 4
			};
			array34[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 2,
				Side1 = 5
			};
			array34[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 4,
				Side1 = 1
			};
			array34[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 5,
				Side1 = 3
			};
			array34[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 5,
				Side1 = 6
			};
			array34[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, 1, -1),
				Side0 = 5,
				Side1 = 4
			};
			array34[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 4,
				Side1 = 6
			};
			array34[6] = myEdgeDefinition;
			tableEntry35.Edges = array34;
			tileTable17[16] = tableEntry;
			TableEntry[] tileTable18 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Horizontal
			};
			TableEntry tableEntry36 = tableEntry;
			MyTileDefinition[] array35 = new MyTileDefinition[6];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Right),
				Normal = Vector3.Right,
				FullQuad = false
			};
			array35[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
				Normal = Vector3.Backward,
				FullQuad = false,
				IsEmpty = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array35[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up),
				Normal = Vector3.Up,
				FullQuad = false
			};
			array35[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
				Normal = Vector3.Left,
				FullQuad = false
			};
			array35[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array35[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Down),
				Normal = Vector3.Down,
				FullQuad = false
			};
			array35[5] = myTileDefinition;
			tableEntry36.Tiles = array35;
			TableEntry tableEntry37 = tableEntry;
			MyEdgeDefinition[] array36 = new MyEdgeDefinition[4];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 4,
				Side1 = 5
			};
			array36[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, 1, -1),
				Side0 = 4,
				Side1 = 3
			};
			array36[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 4,
				Side1 = 0
			};
			array36[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 4,
				Side1 = 2
			};
			array36[3] = myEdgeDefinition;
			tableEntry37.Edges = array36;
			tileTable18[17] = tableEntry;
			TableEntry[] tileTable19 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry38 = tableEntry;
			MyTileDefinition[] array37 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX((float)Math.E * -449f / 777f),
				Normal = Vector3.Forward,
				IsEmpty = true
			};
			array37[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(0f, 1f, 1f)),
				IsEmpty = true
			};
			array37[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Left,
				IsEmpty = true
			};
			array37[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(3.141593f),
				Normal = Vector3.Down,
				IsEmpty = true
			};
			array37[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX((float)Math.E * -449f / 777f) * Matrix.CreateRotationY(3.141593f),
				Normal = Vector3.Right,
				IsEmpty = true
			};
			array37[4] = myTileDefinition;
			tableEntry38.Tiles = array37;
			TableEntry tableEntry39 = tableEntry;
			MyEdgeDefinition[] array38 = new MyEdgeDefinition[1];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 0
			};
			array38[0] = myEdgeDefinition;
			tableEntry39.Edges = array38;
			tileTable19[18] = tableEntry;
			float angle = 2.0943954f;
			float angle2 = 4.188791f;
			TableEntry[] tileTable20 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry40 = tableEntry;
			MyTileDefinition[] array39 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(-1f, 0f, 0f),
				DontOffsetTexture = true
			};
			array39[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 1f, 0f), (float)Math.E * 449f / 777f),
				Normal = new Vector3(0f, 0f, -1f),
				DontOffsetTexture = true
			};
			array39[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0.0, 0.70711, 0.70711),
				DontOffsetTexture = true
			};
			array39[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0.70711, 0.70711, 0.0),
				DontOffsetTexture = true
			};
			array39[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 0f, 1f), 3.141593f),
				Normal = new Vector3(0f, -1f, 0f),
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array39[4] = myTileDefinition;
			tableEntry40.Tiles = array39;
			TableEntry tableEntry41 = tableEntry;
			MyEdgeDefinition[] array40 = new MyEdgeDefinition[7];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(-1f, 1f, -1f),
				Side0 = 1,
				Side1 = 0
			};
			array40[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, -1f),
				Point1 = new Vector3(-1f, 1f, -1f),
				Side0 = 1,
				Side1 = 3
			};
			array40[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, 1f),
				Point1 = new Vector3(-1f, 1f, -1f),
				Side0 = 0,
				Side1 = 2
			};
			array40[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, 1f),
				Point1 = new Vector3(-1f, -1f, 1f),
				Side0 = 4,
				Side1 = 2
			};
			array40[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, 1f),
				Point1 = new Vector3(1f, -1f, -1f),
				Side0 = 4,
				Side1 = 3
			};
			array40[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, -1f),
				Point1 = new Vector3(-1f, -1f, -1f),
				Side0 = 4,
				Side1 = 1
			};
			array40[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, 1f),
				Point1 = new Vector3(-1f, -1f, -1f),
				Side0 = 4,
				Side1 = 0
			};
			array40[6] = myEdgeDefinition;
			tableEntry41.Edges = array40;
			tileTable20[27] = tableEntry;
			TableEntry[] tileTable21 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry42 = tableEntry;
			MyTileDefinition[] array41 = new MyTileDefinition[7];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0.70711, 0.70711, 0.0),
				DontOffsetTexture = true
			};
			array41[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0.0, 0.70711, 0.70711),
				DontOffsetTexture = true
			};
			array41[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 1f, 0f), (float)Math.E * 449f / 777f),
				Normal = new Vector3(0f, 0f, 1f),
				DontOffsetTexture = true
			};
			array41[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(1f, 0f, 0f),
				DontOffsetTexture = true
			};
			array41[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 0f, 1f), (float)Math.E * 449f / 777f),
				Normal = new Vector3(-1f, 0f, 0f),
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array41[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, 0.57735), angle),
				Normal = new Vector3(0f, 0f, -1f),
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array41[5] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.707107, 0.0, 0.707107), 3.141593f),
				Normal = new Vector3(0f, -1f, 0f),
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array41[6] = myTileDefinition;
			tableEntry42.Tiles = array41;
			TableEntry tableEntry43 = tableEntry;
			MyEdgeDefinition[] array42 = new MyEdgeDefinition[9];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, -1f),
				Point1 = new Vector3(1f, 1f, -1f),
				Side0 = 3,
				Side1 = 5
			};
			array42[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, 1f),
				Point1 = new Vector3(-1f, 1f, 1f),
				Side0 = 4,
				Side1 = 2
			};
			array42[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, 1f),
				Point1 = new Vector3(1f, -1f, 1f),
				Side0 = 6,
				Side1 = 2
			};
			array42[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, 1f),
				Point1 = new Vector3(-1f, -1f, -1f),
				Side0 = 6,
				Side1 = 4
			};
			array42[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(1f, -1f, -1f),
				Side0 = 6,
				Side1 = 5
			};
			array42[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, 1f),
				Point1 = new Vector3(1f, -1f, -1f),
				Side0 = 6,
				Side1 = 3
			};
			array42[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(-1f, 1f, -1f),
				Side0 = 5,
				Side1 = 4
			};
			array42[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, 1f, 1f),
				Point1 = new Vector3(-1f, 1f, -1f),
				Side0 = 0,
				Side1 = 4
			};
			array42[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, 1f, -1f),
				Point1 = new Vector3(1f, 1f, -1f),
				Side0 = 1,
				Side1 = 5
			};
			array42[8] = myEdgeDefinition;
			tableEntry43.Edges = array42;
			tileTable21[28] = tableEntry;
			TableEntry[] tileTable22 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry44 = tableEntry;
			MyTileDefinition[] array43 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, -0.57735), angle2),
				Normal = new Vector3(0f, -1f, 0f),
				IsRounded = true,
				DontOffsetTexture = true
			};
			array43[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0.70711, 0.0, 0.70711),
				DontOffsetTexture = true
			};
			array43[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0f, 1f, 0f),
				IsRounded = true,
				DontOffsetTexture = true
			};
			array43[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-1f, 0f, 0f), (float)Math.E * 449f / 777f),
				Normal = new Vector3(0f, 0f, -1f),
				DontOffsetTexture = true
			};
			array43[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, 0.57735, 0.57735), angle),
				Normal = new Vector3(-1f, 0f, 0f),
				DontOffsetTexture = true
			};
			array43[4] = myTileDefinition;
			tableEntry44.Tiles = array43;
			TableEntry tableEntry45 = tableEntry;
			MyEdgeDefinition[] array44 = new MyEdgeDefinition[3];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(-1f, -1f, 1f),
				Side0 = 0,
				Side1 = 4
			};
			array44[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, -1f),
				Point1 = new Vector3(-1f, -1f, 1f),
				Side0 = 0,
				Side1 = 1
			};
			array44[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(1f, -1f, -1f),
				Side0 = 0,
				Side1 = 3
			};
			array44[2] = myEdgeDefinition;
			tableEntry45.Edges = array44;
			tileTable22[26] = tableEntry;
			TableEntry[] tileTable23 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry46 = tableEntry;
			MyTileDefinition[] array45 = new MyTileDefinition[4];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, -1f, 0f), 3.141593f),
				Normal = new Vector3(1f, 0f, 0f),
				DontOffsetTexture = true
			};
			array45[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, 0.57735, 0.57735), angle),
				Normal = new Vector3(0f, 0f, 1f),
				DontOffsetTexture = true
			};
			array45[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, 0.57735), angle),
				Normal = new Vector3(0f, -1f, 0f),
				DontOffsetTexture = true
			};
			array45[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(-0.57735, 0.57735, -0.57735),
				DontOffsetTexture = true
			};
			array45[3] = myTileDefinition;
			tableEntry46.Tiles = array45;
			tableEntry.Edges = new MyEdgeDefinition[0];
			tileTable23[20] = tableEntry;
			TableEntry[] tileTable24 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry47 = tableEntry;
			MyTileDefinition[] array46 = new MyTileDefinition[7];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(-0.57735, 0.57735, -0.57735),
				DontOffsetTexture = true
			};
			array46[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, -1f, 0f), 3.141593f),
				Normal = new Vector3(0f, 0f, -1f),
				DontOffsetTexture = true
			};
			array46[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, 0.57735, 0.57735), angle),
				Normal = new Vector3(0f, 1f, 0f),
				DontOffsetTexture = true
			};
			array46[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 1f, 0f), (float)Math.E * 449f / 777f),
				Normal = new Vector3(-1f, 0f, 0f),
				DontOffsetTexture = true
			};
			array46[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 0f, 1f), 3.141593f),
				Normal = new Vector3(0f, -1f, 0f),
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array46[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 0f, -1f), (float)Math.E * 449f / 777f),
				Normal = new Vector3(1f, 0f, 0f),
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array46[5] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.57735, -0.57735, -0.57735), angle),
				Normal = new Vector3(0f, 0f, 1f),
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array46[6] = myTileDefinition;
			tableEntry47.Tiles = array46;
			TableEntry tableEntry48 = tableEntry;
			MyEdgeDefinition[] array47 = new MyEdgeDefinition[9];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, -1f),
				Point1 = new Vector3(-1f, -1f, -1f),
				Side0 = 4,
				Side1 = 1
			};
			array47[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, 1f),
				Point1 = new Vector3(1f, -1f, -1f),
				Side0 = 4,
				Side1 = 5
			};
			array47[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, 1f),
				Point1 = new Vector3(-1f, -1f, 1f),
				Side0 = 4,
				Side1 = 6
			};
			array47[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, 1f),
				Point1 = new Vector3(-1f, -1f, -1f),
				Side0 = 4,
				Side1 = 3
			};
			array47[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, 1f, -1f),
				Point1 = new Vector3(1f, -1f, -1f),
				Side0 = 5,
				Side1 = 1
			};
			array47[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, 1f, 1f),
				Point1 = new Vector3(1f, -1f, 1f),
				Side0 = 5,
				Side1 = 6
			};
			array47[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, 1f, 1f),
				Point1 = new Vector3(-1f, -1f, 1f),
				Side0 = 6,
				Side1 = 3
			};
			array47[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, 1f, 1f),
				Point1 = new Vector3(-1f, 1f, 1f),
				Side0 = 2,
				Side1 = 6
			};
			array47[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, 1f, 1f),
				Point1 = new Vector3(1f, 1f, -1f),
				Side0 = 2,
				Side1 = 5
			};
			array47[8] = myEdgeDefinition;
			tableEntry48.Edges = array47;
			tileTable24[21] = tableEntry;
			TableEntry[] tileTable25 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry49 = tableEntry;
			MyTileDefinition[] array48 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, -0.57735), angle2),
				Normal = new Vector3(0f, -1f, 0f),
				DontOffsetTexture = true
			};
			array48[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0.70711, 0.0, 0.70711),
				DontOffsetTexture = true
			};
			array48[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0.40825, 0.8165, 0.40825),
				DontOffsetTexture = true
			};
			array48[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 0f, 1f), (float)Math.E * 449f / 777f),
				Normal = new Vector3(-1f, 0f, 0f),
				DontOffsetTexture = true
			};
			array48[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, 0.57735), angle),
				Normal = new Vector3(0f, 0f, -1f),
				DontOffsetTexture = true
			};
			array48[4] = myTileDefinition;
			tableEntry49.Tiles = array48;
			TableEntry tableEntry50 = tableEntry;
			MyEdgeDefinition[] array49 = new MyEdgeDefinition[4];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(-1f, 1f, -1f),
				Side0 = 4,
				Side1 = 3
			};
			array49[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(-1f, -1f, 1f),
				Side0 = 0,
				Side1 = 3
			};
			array49[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(1f, -1f, -1f),
				Side0 = 0,
				Side1 = 4
			};
			array49[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, -1f),
				Point1 = new Vector3(-1f, -1f, 1f),
				Side0 = 0,
				Side1 = 1
			};
			array49[3] = myEdgeDefinition;
			tableEntry50.Edges = array49;
			tileTable25[29] = tableEntry;
			TableEntry[] tileTable26 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry51 = tableEntry;
			MyTileDefinition[] array50 = new MyTileDefinition[7];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0f, 1f, 0f),
				IsRounded = true,
				DontOffsetTexture = true
			};
			array50[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0.40825, 0.8165, 0.40825),
				DontOffsetTexture = true
			};
			array50[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0f, 0f, 1f),
				DontOffsetTexture = true
			};
			array50[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, -1f, 0f), 3.141593f),
				Normal = new Vector3(1f, 0f, 0f),
				DontOffsetTexture = true
			};
			array50[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 0f, 1f), 3.141593f),
				Normal = new Vector3(0f, -1f, 0f),
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array50[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-1f, 0f, 0f), (float)Math.E * 449f / 777f),
				Normal = new Vector3(0f, 0f, -1f),
				DontOffsetTexture = true
			};
			array50[5] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, 0.57735, 0.57735), angle),
				Normal = new Vector3(-1f, 0f, 0f),
				DontOffsetTexture = true
			};
			array50[6] = myTileDefinition;
			tableEntry51.Tiles = array50;
			TableEntry tableEntry52 = tableEntry;
			MyEdgeDefinition[] array51 = new MyEdgeDefinition[4];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, 1f),
				Point1 = new Vector3(1f, -1f, -1f),
				Side0 = 4,
				Side1 = 3
			};
			array51[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, -1f),
				Point1 = new Vector3(-1f, -1f, -1f),
				Side0 = 4,
				Side1 = 5
			};
			array51[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, 1f),
				Point1 = new Vector3(-1f, -1f, -1f),
				Side0 = 4,
				Side1 = 6
			};
			array51[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, 1f),
				Point1 = new Vector3(-1f, -1f, 1f),
				Side0 = 4,
				Side1 = 2
			};
			array51[3] = myEdgeDefinition;
			tableEntry52.Edges = array51;
			tileTable26[25] = tableEntry;
			TableEntry[] tileTable27 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry53 = tableEntry;
			MyTileDefinition[] array52 = new MyTileDefinition[7];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0.70711, 0.70711, 0.0),
				DontOffsetTexture = true
			};
			array52[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0f, 0f, 1f),
				DontOffsetTexture = true
			};
			array52[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0f, 0f, -1f),
				DontOffsetTexture = true
			};
			array52[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 0f, 1f), 3.141593f),
				Normal = new Vector3(0f, -1f, 0f),
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array52[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 0f, 1f), (float)Math.E * 449f / 777f),
				Normal = new Vector3(-1f, 0f, 0f),
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array52[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 1f, 0f), (float)Math.E * 449f / 777f),
				Normal = new Vector3(0f, 1f, 0f),
				DontOffsetTexture = true
			};
			array52[5] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, -0.57735), angle),
				Normal = new Vector3(1f, 0f, 0f),
				DontOffsetTexture = true
			};
			array52[6] = myTileDefinition;
			tableEntry53.Tiles = array52;
			TableEntry tableEntry54 = tableEntry;
			MyEdgeDefinition[] array53 = new MyEdgeDefinition[7];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, 1f),
				Point1 = new Vector3(-1f, 1f, 1f),
				Side0 = 4,
				Side1 = 1
			};
			array53[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(-1f, 1f, -1f),
				Side0 = 4,
				Side1 = 2
			};
			array53[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, 1f, 1f),
				Point1 = new Vector3(-1f, 1f, -1f),
				Side0 = 4,
				Side1 = 5
			};
			array53[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, 1f),
				Point1 = new Vector3(-1f, -1f, 1f),
				Side0 = 3,
				Side1 = 1
			};
			array53[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, 1f),
				Point1 = new Vector3(-1f, -1f, -1f),
				Side0 = 3,
				Side1 = 4
			};
			array53[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, -1f),
				Point1 = new Vector3(-1f, -1f, -1f),
				Side0 = 3,
				Side1 = 2
			};
			array53[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, 1f),
				Point1 = new Vector3(1f, -1f, -1f),
				Side0 = 3,
				Side1 = 6
			};
			array53[6] = myEdgeDefinition;
			tableEntry54.Edges = array53;
			tileTable27[19] = tableEntry;
			TableEntry[] tileTable28 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry55 = tableEntry;
			MyTileDefinition[] array54 = new MyTileDefinition[6];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0f, 0f, 1f),
				DontOffsetTexture = true
			};
			array54[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, -1f, 0f), 3.141593f),
				Normal = new Vector3(1f, 0f, 0f),
				DontOffsetTexture = true
			};
			array54[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0.40825, 0.8165, 0.40825),
				DontOffsetTexture = true
			};
			array54[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 0f, 1f), 3.141593f),
				Normal = new Vector3(0f, -1f, 0f),
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array54[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 0f, 1f), (float)Math.E * 449f / 777f),
				Normal = new Vector3(-1f, 0f, 0f),
				DontOffsetTexture = true
			};
			array54[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, 0.57735), angle),
				Normal = new Vector3(0f, 0f, -1f),
				DontOffsetTexture = true
			};
			array54[5] = myTileDefinition;
			tableEntry55.Tiles = array54;
			TableEntry tableEntry56 = tableEntry;
			MyEdgeDefinition[] array55 = new MyEdgeDefinition[5];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, 1f),
				Point1 = new Vector3(1f, -1f, -1f),
				Side0 = 3,
				Side1 = 1
			};
			array55[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, -1f),
				Point1 = new Vector3(-1f, -1f, -1f),
				Side0 = 3,
				Side1 = 5
			};
			array55[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, 1f),
				Point1 = new Vector3(-1f, -1f, -1f),
				Side0 = 3,
				Side1 = 4
			};
			array55[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, 1f),
				Point1 = new Vector3(-1f, -1f, 1f),
				Side0 = 3,
				Side1 = 0
			};
			array55[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(-1f, 1f, -1f),
				Side0 = 5,
				Side1 = 4
			};
			array55[4] = myEdgeDefinition;
			tableEntry56.Edges = array55;
			tileTable28[24] = tableEntry;
			TableEntry[] tileTable29 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry57 = tableEntry;
			MyTileDefinition[] array56 = new MyTileDefinition[7];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.57735, -0.57735, -0.57735), angle),
				Normal = new Vector3(0f, 1f, 0f),
				DontOffsetTexture = true
			};
			array56[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, -1f, 0f), (float)Math.E * 449f / 777f),
				Normal = new Vector3(-0.40825, 0.8165, 0.40825),
				DontOffsetTexture = true
			};
			array56[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-1f, 0f, 0f), 3.141593f),
				Normal = new Vector3(0f, -1f, 0f),
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array56[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-1f, 0f, 0f), (float)Math.E * 449f / 777f),
				Normal = new Vector3(0f, 0f, -1f),
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array56[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, -0.57735), angle),
				Normal = new Vector3(1f, 0f, 0f),
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array56[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0f, 0f, 1f), (float)Math.E * 449f / 777f),
				Normal = new Vector3(-1f, 0f, 0f),
				DontOffsetTexture = true
			};
			array56[5] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, -0.57735), angle2),
				Normal = new Vector3(0f, 0f, 1f),
				DontOffsetTexture = true
			};
			array56[6] = myTileDefinition;
			tableEntry57.Tiles = array56;
			TableEntry tableEntry58 = tableEntry;
			MyEdgeDefinition[] array57 = new MyEdgeDefinition[9];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, 1f, 1f),
				Point1 = new Vector3(1f, -1f, 1f),
				Side0 = 4,
				Side1 = 6
			};
			array57[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, 1f, -1f),
				Point1 = new Vector3(1f, 1f, 1f),
				Side0 = 4,
				Side1 = 0
			};
			array57[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(-1f, 1f, -1f),
				Side0 = 5,
				Side1 = 3
			};
			array57[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, 1f, -1f),
				Point1 = new Vector3(1f, -1f, -1f),
				Side0 = 3,
				Side1 = 4
			};
			array57[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, 1f, -1f),
				Point1 = new Vector3(1f, 1f, -1f),
				Side0 = 3,
				Side1 = 0
			};
			array57[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(-1f, -1f, 1f),
				Side0 = 2,
				Side1 = 5
			};
			array57[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, 1f),
				Point1 = new Vector3(1f, -1f, 1f),
				Side0 = 2,
				Side1 = 6
			};
			array57[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(1f, -1f, -1f),
				Point1 = new Vector3(1f, -1f, 1f),
				Side0 = 2,
				Side1 = 4
			};
			array57[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(1f, -1f, -1f),
				Side0 = 2,
				Side1 = 3
			};
			array57[8] = myEdgeDefinition;
			tableEntry58.Edges = array57;
			tileTable29[23] = tableEntry;
			TableEntry[] tileTable30 = m_tileTable;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry59 = tableEntry;
			MyTileDefinition[] array58 = new MyTileDefinition[4];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.707107, -0.707107, 0.0), 3.141593f),
				Normal = new Vector3(0f, -1f, 0f),
				DontOffsetTexture = true
			};
			array58[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0f, 0f, 1f),
				DontOffsetTexture = true
			};
			array58[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(-1f, 0f, 0f),
				DontOffsetTexture = true
			};
			array58[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = new Vector3(0.40825, 0.8165, -0.40825),
				DontOffsetTexture = true
			};
			array58[3] = myTileDefinition;
			tableEntry59.Tiles = array58;
			TableEntry tableEntry60 = tableEntry;
			MyEdgeDefinition[] array59 = new MyEdgeDefinition[3];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, -1f),
				Point1 = new Vector3(1f, -1f, 1f),
				Side0 = 0,
				Side1 = 3
			};
			array59[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, 1f),
				Point1 = new Vector3(1f, -1f, 1f),
				Side0 = 0,
				Side1 = 1
			};
			array59[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, -1f, 1f),
				Point1 = new Vector3(-1f, -1f, -1f),
				Side0 = 0,
				Side1 = 2
			};
			array59[2] = myEdgeDefinition;
			tableEntry60.Edges = array59;
			tileTable30[22] = tableEntry;
			InitTopologyUniqueRotationsMatrices();
		}

		public static MyTileDefinition[] GetCubeTiles(MyCubeBlockDefinition block)
		{
			if (block.CubeDefinition == null)
			{
				return null;
			}
			return m_tileTable[(int)block.CubeDefinition.CubeTopology].Tiles;
		}

		public static TableEntry GetTopologyInfo(MyCubeTopology topology)
		{
			return m_tileTable[(int)topology];
		}

		public static MyRotationOptionsEnum GetCubeRotationOptions(MyCubeBlockDefinition block)
		{
			if (block.CubeDefinition == null)
			{
				return MyRotationOptionsEnum.Both;
			}
			return m_tileTable[(int)block.CubeDefinition.CubeTopology].RotationOptions;
		}

		public static void GetRotatedBlockSize(MyCubeBlockDefinition block, ref Matrix rotation, out Vector3I size)
		{
			Vector3I.TransformNormal(ref block.Size, ref rotation, out size);
		}

		private static void InitTopologyUniqueRotationsMatrices()
		{
			m_allPossible90rotations = new MatrixI[24]
			{
				new MatrixI(Base6Directions.Direction.Forward, Base6Directions.Direction.Up),
				new MatrixI(Base6Directions.Direction.Down, Base6Directions.Direction.Forward),
				new MatrixI(Base6Directions.Direction.Backward, Base6Directions.Direction.Down),
				new MatrixI(Base6Directions.Direction.Up, Base6Directions.Direction.Backward),
				new MatrixI(Base6Directions.Direction.Forward, Base6Directions.Direction.Right),
				new MatrixI(Base6Directions.Direction.Down, Base6Directions.Direction.Right),
				new MatrixI(Base6Directions.Direction.Backward, Base6Directions.Direction.Right),
				new MatrixI(Base6Directions.Direction.Up, Base6Directions.Direction.Right),
				new MatrixI(Base6Directions.Direction.Forward, Base6Directions.Direction.Down),
				new MatrixI(Base6Directions.Direction.Up, Base6Directions.Direction.Forward),
				new MatrixI(Base6Directions.Direction.Backward, Base6Directions.Direction.Up),
				new MatrixI(Base6Directions.Direction.Down, Base6Directions.Direction.Backward),
				new MatrixI(Base6Directions.Direction.Forward, Base6Directions.Direction.Left),
				new MatrixI(Base6Directions.Direction.Up, Base6Directions.Direction.Left),
				new MatrixI(Base6Directions.Direction.Backward, Base6Directions.Direction.Left),
				new MatrixI(Base6Directions.Direction.Down, Base6Directions.Direction.Left),
				new MatrixI(Base6Directions.Direction.Left, Base6Directions.Direction.Up),
				new MatrixI(Base6Directions.Direction.Left, Base6Directions.Direction.Backward),
				new MatrixI(Base6Directions.Direction.Left, Base6Directions.Direction.Down),
				new MatrixI(Base6Directions.Direction.Left, Base6Directions.Direction.Forward),
				new MatrixI(Base6Directions.Direction.Right, Base6Directions.Direction.Down),
				new MatrixI(Base6Directions.Direction.Right, Base6Directions.Direction.Backward),
				new MatrixI(Base6Directions.Direction.Right, Base6Directions.Direction.Up),
				new MatrixI(Base6Directions.Direction.Right, Base6Directions.Direction.Forward)
			};
			m_uniqueTopologyRotationTable = new MatrixI[Enum.GetValues(typeof(MyCubeTopology)).Length][];
			m_uniqueTopologyRotationTable[0] = null;
			FillRotationsForTopology(MyCubeTopology.Slope, 0);
			FillRotationsForTopology(MyCubeTopology.Corner, 2);
			FillRotationsForTopology(MyCubeTopology.InvCorner, 0);
			FillRotationsForTopology(MyCubeTopology.StandaloneBox, -1);
			FillRotationsForTopology(MyCubeTopology.RoundedSlope, -1);
			FillRotationsForTopology(MyCubeTopology.RoundSlope, 0);
			FillRotationsForTopology(MyCubeTopology.RoundCorner, 2);
			FillRotationsForTopology(MyCubeTopology.RoundInvCorner, -1);
			FillRotationsForTopology(MyCubeTopology.RotatedSlope, -1);
			FillRotationsForTopology(MyCubeTopology.RotatedCorner, -1);
			FillRotationsForTopology(MyCubeTopology.HalfBox, 1);
			FillRotationsForTopology(MyCubeTopology.Slope2Base, -1);
			FillRotationsForTopology(MyCubeTopology.Slope2Tip, -1);
			FillRotationsForTopology(MyCubeTopology.Corner2Base, -1);
			FillRotationsForTopology(MyCubeTopology.Corner2Tip, -1);
			FillRotationsForTopology(MyCubeTopology.InvCorner2Base, -1);
			FillRotationsForTopology(MyCubeTopology.InvCorner2Tip, -1);
			FillRotationsForTopology(MyCubeTopology.HalfSlopeBox, -1);
			FillRotationsForTopology(MyCubeTopology.HalfSlopeInverted, -1);
			FillRotationsForTopology(MyCubeTopology.HalfSlopeCorner, -1);
			FillRotationsForTopology(MyCubeTopology.SlopedCornerTip, -1);
			FillRotationsForTopology(MyCubeTopology.SlopedCornerBase, -1);
			FillRotationsForTopology(MyCubeTopology.SlopedCorner, -1);
			FillRotationsForTopology(MyCubeTopology.HalfSlopedCornerBase, -1);
			FillRotationsForTopology(MyCubeTopology.HalfCorner, -1);
			FillRotationsForTopology(MyCubeTopology.CornerSquare, -1);
			FillRotationsForTopology(MyCubeTopology.CornerSquareInverted, -1);
			FillRotationsForTopology(MyCubeTopology.HalfSlopedCorner, -1);
			FillRotationsForTopology(MyCubeTopology.HalfSlopeCornerInverted, -1);
		}

		/// <summary>
		/// Fills rotation table for topology. Any arbitrary 90deg. rotation can then be converted to one unique rotation
		/// </summary>
		/// <param name="topology"></param>
		/// <param name="mainTile">Tile which normal is tested to find unique rotations. If -1, all rotations are allowed</param>
		private static void FillRotationsForTopology(MyCubeTopology topology, int mainTile)
		{
			Vector3[] array = new Vector3[m_allPossible90rotations.Length];
			m_uniqueTopologyRotationTable[(int)topology] = new MatrixI[m_allPossible90rotations.Length];
			for (int i = 0; i < m_allPossible90rotations.Length; i++)
			{
				int num = -1;
				if (mainTile != -1)
				{
					Vector3.TransformNormal(ref m_tileTable[(int)topology].Tiles[mainTile].Normal, ref m_allPossible90rotations[i], out var result);
					array[i] = result;
					for (int j = 0; j < i; j++)
					{
						if (Vector3.Dot(array[j], result) > 0.98f)
						{
							num = j;
							break;
						}
					}
				}
				if (num != -1)
				{
					m_uniqueTopologyRotationTable[(int)topology][i] = m_uniqueTopologyRotationTable[(int)topology][num];
				}
				else
				{
					m_uniqueTopologyRotationTable[(int)topology][i] = m_allPossible90rotations[i];
				}
			}
		}

		/// <summary>
		/// From 90degrees rotations combinations returns one unique topology orientation, which can differ
		/// from input, but the resulted shape of topology is same
		/// </summary>
		/// <param name="myCubeTopology">cube topology</param>
		/// <param name="orientation">input orientation</param>
		/// <returns></returns>
		public static MyBlockOrientation GetTopologyUniqueOrientation(MyCubeTopology myCubeTopology, MyBlockOrientation orientation)
		{
			if (m_uniqueTopologyRotationTable[(int)myCubeTopology] == null)
			{
				return MyBlockOrientation.Identity;
			}
			for (int i = 0; i < m_allPossible90rotations.Length; i++)
			{
				MatrixI matrixI = m_allPossible90rotations[i];
				if (matrixI.Forward == orientation.Forward && matrixI.Up == orientation.Up)
				{
					return m_uniqueTopologyRotationTable[(int)myCubeTopology][i].GetBlockOrientation();
				}
			}
			return MyBlockOrientation.Identity;
		}
	}
}
