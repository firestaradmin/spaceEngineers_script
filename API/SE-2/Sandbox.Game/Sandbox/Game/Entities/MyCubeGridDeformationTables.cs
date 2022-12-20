using System;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Entities
{
	internal static class MyCubeGridDeformationTables
	{
		public class DeformationTable
		{
			public readonly Dictionary<Vector3I, Matrix> OffsetTable = new Dictionary<Vector3I, Matrix>();

			public readonly HashSet<Vector3I> CubeOffsets = new HashSet<Vector3I>();

			public Vector3I Normal;

			public Vector3I MinOffset = Vector3I.MaxValue;

			public Vector3I MaxOffset = Vector3I.MinValue;
		}

		public static DeformationTable[] ThinUpper = new DeformationTable[3]
		{
			CreateTable(new Vector3I(1, 0, 0)),
			CreateTable(new Vector3I(0, 1, 0)),
			CreateTable(new Vector3I(0, 0, 1))
		};

		public static DeformationTable[] ThinLower = new DeformationTable[3]
		{
			CreateTable(new Vector3I(-1, 0, 0)),
			CreateTable(new Vector3I(0, -1, 0)),
			CreateTable(new Vector3I(0, 0, -1))
		};

		private static DeformationTable CreateTable(Vector3I normal)
		{
			DeformationTable deformationTable = new DeformationTable();
			deformationTable.Normal = normal;
			Vector3I vector3I = new Vector3I(1, 1, 1);
			Vector3I vector3I2 = Vector3I.Abs(normal);
			Vector3I vector3I3 = new Vector3I(1, 1, 1) - vector3I2;
			vector3I3 *= 2;
			for (int i = -vector3I3.X; i <= vector3I3.X; i++)
			{
				for (int j = -vector3I3.Y; j <= vector3I3.Y; j++)
				{
					for (int k = -vector3I3.Z; k <= vector3I3.Z; k++)
					{
						Vector3I value = new Vector3I(i, j, k);
						float num = Math.Max(Math.Abs(k), Math.Max(Math.Abs(i), Math.Abs(j)));
						float num2 = 1f;
						if (num > 1f)
						{
							num2 = 0.3f;
						}
						float num3 = num2 * 0.25f;
						Vector3I vector3I4 = vector3I + new Vector3I(i, j, k) + normal;
						Matrix value2 = Matrix.CreateFromDir(-normal * num3);
						deformationTable.OffsetTable.Add(vector3I4, value2);
<<<<<<< HEAD
						Vector3I item = vector3I4 >> 1;
						Vector3I item2 = vector3I4 - Vector3I.One >> 1;
						deformationTable.CubeOffsets.Add(item);
						deformationTable.CubeOffsets.Add(item2);
=======
						Vector3I vector3I5 = vector3I4 >> 1;
						Vector3I vector3I6 = vector3I4 - Vector3I.One >> 1;
						deformationTable.CubeOffsets.Add(vector3I5);
						deformationTable.CubeOffsets.Add(vector3I6);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						deformationTable.MinOffset = Vector3I.Min(deformationTable.MinOffset, value);
						deformationTable.MaxOffset = Vector3I.Max(deformationTable.MaxOffset, value);
					}
				}
			}
			return deformationTable;
		}
	}
}
