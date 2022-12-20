using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using Sandbox.Game.Entities.Cube;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	internal class MyGridSkeleton
	{
		public readonly ConcurrentDictionary<Vector3I, Vector3> Bones = new ConcurrentDictionary<Vector3I, Vector3>();

		private List<Vector3I> m_tmpRemovedCubes = new List<Vector3I>();

		private HashSet<Vector3I> m_usedBones = new HashSet<Vector3I>();

		private HashSet<Vector3I> m_testedCubes = new HashSet<Vector3I>();

		/// <summary>
		/// Multiply this with your grid size before use!
		/// </summary>
		private static readonly float MAX_BONE_ERROR;

		/// <summary>
		/// This needs to be ThreadStatic because mods can call MyCubeGrid.GetObjectBuilder from other threads
		/// </summary>
		[ThreadStatic]
		private static List<Vector3I> m_tempAffectedCubes;

		/// <summary>
		/// Density multiplier
		/// Many algorithms rely on this
		/// </summary>
		public const int BoneDensity = 2;

		public static readonly Vector3I[] BoneOffsets;

		public Vector3 this[Vector3I pos]
		{
			get
			{
<<<<<<< HEAD
				if (Bones.TryGetValue(pos, out var value))
=======
				Vector3 result = default(Vector3);
				if (Bones.TryGetValue(pos, ref result))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return result;
				}
				return Vector3.Zero;
			}
			set
			{
				Bones.set_Item(pos, value);
			}
		}

		public bool HasUnusedBones => m_tmpRemovedCubes.Count > 0;

		static MyGridSkeleton()
		{
			MAX_BONE_ERROR = Vector3UByte.Denormalize(new Vector3UByte(128, 128, 128), 1f).X * 0.75f;
			BoneOffsets = new Vector3I[(int)Math.Pow(3.0, 3.0)];
			int num = 0;
			Vector3I vector3I = default(Vector3I);
			vector3I.X = 0;
			while (vector3I.X <= 1)
			{
				vector3I.Y = 0;
				while (vector3I.Y <= 1)
				{
					vector3I.Z = 0;
					while (vector3I.Z <= 1)
					{
						BoneOffsets[num++] = vector3I * 2;
						vector3I.Z++;
					}
					vector3I.Y++;
				}
				vector3I.X++;
			}
			vector3I.X = 0;
			while (vector3I.X <= 2)
			{
				vector3I.Y = 0;
				while (vector3I.Y <= 2)
				{
					vector3I.Z = 0;
					while (vector3I.Z <= 2)
					{
						if (vector3I.X == 1 || vector3I.Y == 1 || vector3I.Z == 1)
						{
							BoneOffsets[num++] = vector3I;
						}
						vector3I.Z++;
					}
					vector3I.Y++;
				}
				vector3I.X++;
			}
		}

		public static float GetMaxBoneError(float gridSize)
		{
			return MAX_BONE_ERROR * gridSize;
		}

		public void Reset()
		{
			Bones.Clear();
		}

		public void CopyTo(MyGridSkeleton target, Vector3I fromGridPosition)
		{
			Vector3I vector3I = fromGridPosition * 2;
			Vector3I[] boneOffsets = BoneOffsets;
<<<<<<< HEAD
			foreach (Vector3I vector3I2 in boneOffsets)
			{
				Vector3I key = vector3I + vector3I2;
				if (Bones.TryGetValue(key, out var value))
=======
			Vector3 vector = default(Vector3);
			foreach (Vector3I vector3I2 in boneOffsets)
			{
				Vector3I vector3I3 = vector3I + vector3I2;
				if (Bones.TryGetValue(vector3I3, ref vector))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					target.Bones.set_Item(vector3I3, vector);
				}
			}
		}

		/// <summary>
		/// Copies part of skeleton to other skeleton, both positions are inclusive
		/// </summary>
		public void CopyTo(MyGridSkeleton target, Vector3I fromGridPosition, Vector3I toGridPosition)
		{
			Vector3I vector3I = fromGridPosition * 2;
			Vector3I vector3I2 = (toGridPosition - fromGridPosition + Vector3I.One) * 2;
			Vector3I vector3I3 = default(Vector3I);
			vector3I3.X = 0;
<<<<<<< HEAD
=======
			Vector3 vector = default(Vector3);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			while (vector3I3.X <= vector3I2.X)
			{
				vector3I3.Y = 0;
				while (vector3I3.Y <= vector3I2.Y)
				{
					vector3I3.Z = 0;
					while (vector3I3.Z <= vector3I2.Z)
					{
<<<<<<< HEAD
						Vector3I key = vector3I + vector3I3;
						if (Bones.TryGetValue(key, out var value))
=======
						Vector3I vector3I4 = vector3I + vector3I3;
						if (Bones.TryGetValue(vector3I4, ref vector))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							target.Bones.set_Item(vector3I4, vector);
						}
						else
						{
							target.Bones.Remove<Vector3I, Vector3>(vector3I4);
						}
						vector3I3.Z++;
					}
					vector3I3.Y++;
				}
				vector3I3.X++;
			}
		}

		public void CopyTo(MyGridSkeleton target, MatrixI transformationMatrix, MyCubeGrid targetGrid)
		{
			MatrixI rightMatrix = new MatrixI(new Vector3I(1, 1, 1), Base6Directions.Direction.Forward, Base6Directions.Direction.Up);
			MatrixI leftMatrix = new MatrixI(new Vector3I(-1, -1, -1), Base6Directions.Direction.Forward, Base6Directions.Direction.Up);
			transformationMatrix.Translation *= 2;
			MatrixI.Multiply(ref leftMatrix, ref transformationMatrix, out var result);
			MatrixI.Multiply(ref result, ref rightMatrix, out transformationMatrix);
			transformationMatrix.GetBlockOrientation().GetMatrix(out var result2);
<<<<<<< HEAD
=======
			Vector3 vector2 = default(Vector3);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (KeyValuePair<Vector3I, Vector3> bone in Bones)
			{
				Vector3I value = bone.Key;
				Vector3I.Transform(ref value, ref transformationMatrix, out var result3);
				Vector3 vector = Vector3.Transform(bone.Value, result2);
<<<<<<< HEAD
				if (target.Bones.TryGetValue(result3, out var value2))
=======
				if (target.Bones.TryGetValue(result3, ref vector2))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					Vector3 vector3 = (vector2 + vector) * 0.5f;
					target.Bones.set_Item(result3, vector3);
				}
				else
				{
					target.Bones.set_Item(result3, vector);
				}
				Vector3I vector3I = result3 / 2;
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						for (int k = -1; k <= 1; k++)
						{
							targetGrid.SetCubeDirty(vector3I + new Vector3I(i, j, k));
						}
					}
				}
			}
		}

		/// <summary>
		/// Fixes bone in way that it won't be further than neighbour bones.
		/// This helps fix invalid triangles during rendering.
		/// </summary>
		public void FixBone(Vector3I gridPosition, Vector3I boneOffset, float gridSize, float minBoneDist = 0.05f)
		{
			FixBone(gridPosition * 2 + boneOffset, minBoneDist);
		}

		private void FixBone(Vector3I bonePosition, float gridSize, float minBoneDist = 0.05f)
		{
			Vector3 defaultBone = -Vector3.One * gridSize;
			Vector3 defaultBone2 = Vector3.One * gridSize;
			Vector3 min = default(Vector3);
			min.X = TryGetBone(bonePosition - Vector3I.UnitX, ref defaultBone).X;
			min.Y = TryGetBone(bonePosition - Vector3I.UnitY, ref defaultBone).Y;
			min.Z = TryGetBone(bonePosition - Vector3I.UnitZ, ref defaultBone).Z;
			min -= new Vector3(gridSize / 2f);
			min += new Vector3(minBoneDist);
			Vector3 max = default(Vector3);
			max.X = TryGetBone(bonePosition + Vector3I.UnitX, ref defaultBone2).X;
			max.Y = TryGetBone(bonePosition + Vector3I.UnitY, ref defaultBone2).Y;
			max.Z = TryGetBone(bonePosition + Vector3I.UnitZ, ref defaultBone2).Z;
			max += new Vector3(gridSize / 2f);
			max -= new Vector3(minBoneDist);
			Bones.set_Item(bonePosition, Vector3.Clamp(Bones.get_Item(bonePosition), min, max));
		}

		private Vector3 TryGetBone(Vector3I bonePosition, ref Vector3 defaultBone)
		{
<<<<<<< HEAD
			if (Bones.TryGetValue(bonePosition, out var value))
=======
			Vector3 result = default(Vector3);
			if (Bones.TryGetValue(bonePosition, ref result))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return result;
			}
			return defaultBone;
		}

		public void Serialize(List<BoneInfo> result, float boneRange, MyCubeGrid grid)
		{
			BoneInfo item = default(BoneInfo);
			float maxBoneError = GetMaxBoneError(grid.GridSize);
			maxBoneError *= maxBoneError;
			foreach (KeyValuePair<Vector3I, Vector3> bone in Bones)
			{
				Vector3I? cubeFromBone = GetCubeFromBone(bone.Key, grid);
				if (cubeFromBone.HasValue && Math.Abs(GetDefinitionOffsetWithNeighbours(cubeFromBone.Value, bone.Key, grid).LengthSquared() - bone.Value.LengthSquared()) > maxBoneError)
				{
					item.BonePosition = bone.Key;
					item.BoneOffset = Vector3UByte.Normalize(bone.Value, boneRange);
					if (!Vector3UByte.IsMiddle(item.BoneOffset))
					{
						result.Add(item);
					}
				}
			}
		}

		private Vector3I? GetCubeFromBone(Vector3I bone, MyCubeGrid grid)
		{
			Vector3I zero = Vector3I.Zero;
			zero = bone / 2;
			if (grid.CubeExists(zero))
			{
				return zero;
			}
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					for (int k = -1; k <= 1; k++)
					{
						Vector3I vector3I = zero + new Vector3I(i, j, k);
						Vector3I vector3I2 = bone - vector3I * 2;
						if (vector3I2.X <= 2 && vector3I2.Y <= 2 && vector3I2.Z <= 2 && grid.CubeExists(vector3I))
						{
							return vector3I;
						}
					}
				}
			}
			return null;
		}

		public void Deserialize(List<BoneInfo> data, float boneRange, float gridSize, bool clear = false)
		{
			if (clear)
			{
				Bones.Clear();
			}
			foreach (BoneInfo datum in data)
			{
				Bones.set_Item((Vector3I)datum.BonePosition, Vector3UByte.Denormalize(datum.BoneOffset, boneRange));
			}
		}

		public bool SerializePart(Vector3I minBone, Vector3I maxBone, float boneRange, List<byte> result)
		{
			bool flag = false;
			minBone.ToBytes(result);
			maxBone.ToBytes(result);
			int count = result.Count;
			result.Add(1);
			Vector3I vector3I = default(Vector3I);
			vector3I.X = minBone.X;
			Vector3 vec = default(Vector3);
			while (vector3I.X <= maxBone.X)
			{
				vector3I.Y = minBone.Y;
				while (vector3I.Y <= maxBone.Y)
				{
					vector3I.Z = minBone.Z;
					while (vector3I.Z <= maxBone.Z)
					{
<<<<<<< HEAD
						flag |= Bones.TryGetValue(key, out var value);
						Vector3UByte vector3UByte = Vector3UByte.Normalize(value, boneRange);
=======
						flag |= Bones.TryGetValue(vector3I, ref vec);
						Vector3UByte vector3UByte = Vector3UByte.Normalize(vec, boneRange);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						result.Add(vector3UByte.X);
						result.Add(vector3UByte.Y);
						result.Add(vector3UByte.Z);
						vector3I.Z++;
					}
					vector3I.Y++;
				}
				vector3I.X++;
			}
			if (!flag)
			{
				result.RemoveRange(count, result.Count - count);
				result.Add(0);
			}
			return flag;
		}

		public int DeserializePart(float boneRange, byte[] data, ref int dataIndex, out Vector3I minBone, out Vector3I maxBone)
		{
			minBone = new Vector3I(data, dataIndex);
			dataIndex += 12;
			maxBone = new Vector3I(data, dataIndex);
			dataIndex += 12;
			bool flag = data[dataIndex] > 0;
			dataIndex++;
			Vector3I vector3I = maxBone - minBone;
			vector3I += Vector3I.One;
			if (flag && dataIndex + vector3I.Size * 3 > data.Length)
			{
				return dataIndex;
			}
			Vector3I vector3I2 = default(Vector3I);
			vector3I2.X = minBone.X;
			while (vector3I2.X <= maxBone.X)
			{
				vector3I2.Y = minBone.Y;
				while (vector3I2.Y <= maxBone.Y)
				{
					vector3I2.Z = minBone.Z;
					while (vector3I2.Z <= maxBone.Z)
					{
						if (flag)
						{
							this[vector3I2] = Vector3UByte.Denormalize(new Vector3UByte(data[dataIndex], data[dataIndex + 1], data[dataIndex + 2]), boneRange);
							dataIndex += 3;
						}
						else
						{
							Bones.Remove<Vector3I, Vector3>(vector3I2);
						}
						vector3I2.Z++;
					}
					vector3I2.Y++;
				}
				vector3I2.X++;
			}
			return dataIndex;
		}

		public Vector3 GetBone(Vector3I cubePos, Vector3I bonePos)
		{
<<<<<<< HEAD
			if (!Bones.TryGetValue(cubePos * 2 + bonePos, out var value))
=======
			Vector3 result = default(Vector3);
			if (!Bones.TryGetValue(cubePos * 2 + bonePos, ref result))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return Vector3.Zero;
			}
			return result;
		}

		public void GetBone(ref Vector3I pos, out Vector3 bone)
		{
			if (!Bones.TryGetValue(pos, ref bone))
			{
				bone = Vector3.Zero;
			}
		}

		public bool TryGetBone(ref Vector3I pos, out Vector3 bone)
		{
			return Bones.TryGetValue(pos, ref bone);
		}

		public void SetBone(ref Vector3I pos, ref Vector3 bone)
		{
			Bones.set_Item(pos, bone);
		}

		public void SetOrClearBone(ref Vector3I pos, ref Vector3 bone)
		{
			if (bone == Vector3.Zero)
			{
				Bones.Remove<Vector3I, Vector3>(pos);
			}
			else
			{
				Bones.set_Item(pos, bone);
			}
		}

		public void ClearBone(ref Vector3I pos)
		{
			Bones.Remove<Vector3I, Vector3>(pos);
		}

		/// <summary>
		/// Returns true when bone was changed.
		/// When new bone offset length is smaller than epsilon, it will remove bone.
		/// Factor is used as t paramter in a lerp. This is because the default position
		/// for a bone may not be 0
		/// </summary>
		public bool MultiplyBone(ref Vector3I pos, float factor, ref Vector3I cubePos, MyCubeGrid cubeGrid, float epsilon = 0.005f)
		{
<<<<<<< HEAD
			if (Bones.TryGetValue(pos, out var value))
=======
			Vector3 value = default(Vector3);
			if (Bones.TryGetValue(pos, ref value))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Vector3 definitionOffsetWithNeighbours = GetDefinitionOffsetWithNeighbours(cubePos, pos, cubeGrid);
				factor = 1f - factor;
				if (factor < 0.1f)
				{
					factor = 0.1f;
				}
				Vector3 vector = Vector3.Lerp(value, definitionOffsetWithNeighbours, factor);
				if (vector.LengthSquared() < epsilon * epsilon)
				{
					Bones.Remove<Vector3I, Vector3>(pos);
				}
				else
				{
					Bones.set_Item(pos, vector);
				}
				return true;
			}
			return false;
		}

		[Conditional("DEBUG")]
		private void AssertBone(Vector3 value, float range)
		{
		}

		public bool IsDeformed(Vector3I cube, float ignoredDeformation, MyCubeGrid cubeGrid, bool checkBlockDefinition)
		{
			float num = ignoredDeformation * ignoredDeformation;
			float maxBoneError = GetMaxBoneError(cubeGrid.GridSize);
			maxBoneError *= maxBoneError;
			Vector3I[] boneOffsets = BoneOffsets;
<<<<<<< HEAD
			foreach (Vector3I vector3I in boneOffsets)
			{
				if (!Bones.TryGetValue(cube * 2 + vector3I, out var value))
=======
			Vector3 vector = default(Vector3);
			foreach (Vector3I vector3I in boneOffsets)
			{
				if (!Bones.TryGetValue(cube * 2 + vector3I, ref vector))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					continue;
				}
				if (checkBlockDefinition)
				{
					float num2 = GetDefinitionOffsetWithNeighbours(cube, cube * 2 + vector3I, cubeGrid).LengthSquared();
<<<<<<< HEAD
					float num3 = value.LengthSquared();
=======
					float num3 = vector.LengthSquared();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (Math.Abs(num2 - num3) > maxBoneError)
					{
						return true;
					}
				}
				else if (vector.LengthSquared() > num)
				{
					return true;
				}
			}
			return false;
		}

		public float MaxDeformation(Vector3I cube, MyCubeGrid cubeGrid)
		{
			float num = 0f;
			float maxBoneError = GetMaxBoneError(cubeGrid.GridSize);
			maxBoneError *= maxBoneError;
			Vector3I[] boneOffsets = BoneOffsets;
			Vector3 offset = default(Vector3);
			foreach (Vector3I vector3I in boneOffsets)
			{
				Vector3I vector3I2 = cube * 2 + vector3I;
				bool num2 = Bones.TryGetValue(vector3I2, ref offset);
				float num3 = GetDefinitionOffsetWithNeighbours(cube, cube * 2 + vector3I, cubeGrid).LengthSquared();
				float num4 = offset.LengthSquared();
				float num5 = Math.Abs(num3 - num4);
				if (num5 > num)
				{
					num = num5;
				}
				if (!num2 && num5 > maxBoneError)
				{
					Bones.AddOrUpdate(vector3I2, offset, (Func<Vector3I, Vector3, Vector3>)((Vector3I k, Vector3 v) => v = offset));
					cubeGrid.AddDirtyBone(cube, vector3I);
				}
			}
			return (float)Math.Sqrt(num);
		}

		/// <summary>
		/// Bone offset is normally between 0 and BoneDensity (including)
		/// This function takes any other values and transforms it into cube and proper bone offset
		/// </summary>
		public void Wrap(ref Vector3I cube, ref Vector3I boneOffset)
		{
			Vector3I vector3I = cube * 2 + boneOffset;
			cube = Vector3I.Floor((Vector3D)(vector3I / 2));
			boneOffset = vector3I - cube * 2;
		}

		/// <summary>
		/// Gets all cubes which are affected by bone.
		/// </summary>
		public void GetAffectedCubes(Vector3I cube, Vector3I boneOffset, List<Vector3I> resultList, MyCubeGrid grid)
		{
			Vector3I value = boneOffset - Vector3I.One;
			Vector3I vector3I = Vector3I.Sign(value);
			value *= vector3I;
			Vector3I vector3I2 = default(Vector3I);
			vector3I2.X = 0;
			while (vector3I2.X <= value.X)
			{
				vector3I2.Y = 0;
				while (vector3I2.Y <= value.Y)
				{
					vector3I2.Z = 0;
					while (vector3I2.Z <= value.Z)
					{
						Vector3I vector3I3 = cube + vector3I2 * vector3I;
						if (grid.CubeExists(vector3I3))
						{
							resultList.Add(vector3I3);
						}
						vector3I2.Z++;
					}
					vector3I2.Y++;
				}
				vector3I2.X++;
			}
		}

		public void MarkCubeRemoved(ref Vector3I pos)
		{
			m_tmpRemovedCubes.Add(pos);
		}

		public void RemoveUnusedBones(MyCubeGrid grid)
		{
			if (m_tmpRemovedCubes.Count == 0)
			{
				return;
			}
			Vector3I vector3I = default(Vector3I);
			foreach (Vector3I tmpRemovedCube in m_tmpRemovedCubes)
			{
				if (grid.CubeExists(tmpRemovedCube))
				{
					if (!m_testedCubes.Contains(tmpRemovedCube))
					{
						m_testedCubes.Add(tmpRemovedCube);
						AddUsedBones(tmpRemovedCube);
					}
					continue;
				}
				_ = tmpRemovedCube * 2 + Vector3I.One;
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						for (int k = -1; k <= 1; k++)
						{
							vector3I.X = i;
							vector3I.Y = j;
							vector3I.Z = k;
							Vector3I vector3I2 = tmpRemovedCube + vector3I;
							if (grid.CubeExists(vector3I2) && !m_testedCubes.Contains(vector3I2))
							{
								m_testedCubes.Add(vector3I2);
								AddUsedBones(vector3I2);
							}
						}
					}
				}
			}
			foreach (Vector3I tmpRemovedCube2 in m_tmpRemovedCubes)
			{
				Vector3I pos = tmpRemovedCube2 * 2;
				for (int l = 0; l <= 2; l++)
				{
					for (int m = 0; m <= 2; m++)
					{
						for (int n = 0; n <= 2; n++)
						{
							if (!m_usedBones.Contains(pos))
							{
								ClearBone(ref pos);
							}
							pos.Z++;
						}
						pos.Y++;
						pos.Z -= 3;
					}
					pos.X++;
					pos.Y -= 3;
				}
			}
			m_testedCubes.Clear();
			m_usedBones.Clear();
			m_tmpRemovedCubes.Clear();
		}

		/// <summary>
		/// MArks the bones of the cube at position "pos" as used
		/// </summary>
		private void AddUsedBones(Vector3I pos)
		{
			pos *= 2;
			for (int i = 0; i <= 2; i++)
			{
				for (int j = 0; j <= 2; j++)
				{
					for (int k = 0; k <= 2; k++)
					{
						m_usedBones.Add(pos);
						pos.Z++;
					}
					pos.Y++;
					pos.Z -= 3;
				}
				pos.X++;
				pos.Y -= 3;
			}
		}

		public Vector3 GetDefinitionOffsetWithNeighbours(Vector3I cubePos, Vector3I bonePos, MyCubeGrid grid)
		{
			Vector3I cubeBoneOffset = GetCubeBoneOffset(cubePos, bonePos);
			Vector3 zero = Vector3.Zero;
			int num = 0;
			using (MyUtils.ReuseCollection(ref m_tempAffectedCubes))
			{
				GetAffectedCubes(cubePos, cubeBoneOffset, m_tempAffectedCubes, grid);
				foreach (Vector3I tempAffectedCube in m_tempAffectedCubes)
				{
					MySlimBlock cubeBlock = grid.GetCubeBlock(tempAffectedCube);
					if (cubeBlock != null && cubeBlock.BlockDefinition.Skeleton != null)
					{
						Vector3I cubeBoneOffset2 = GetCubeBoneOffset(tempAffectedCube, bonePos);
						Vector3? definitionOffset = GetDefinitionOffset(cubeBlock, cubeBoneOffset2);
						if (definitionOffset.HasValue)
						{
							zero += definitionOffset.Value;
							num++;
						}
					}
				}
			}
			if (num == 0)
			{
				return zero;
			}
			return zero / num;
		}

		private Vector3I GetCubeBoneOffset(Vector3I cubePos, Vector3I boneOffset)
		{
			Vector3I zero = Vector3I.Zero;
			if (boneOffset.X % 2 != 0)
			{
				zero.X = 1;
			}
			else if (boneOffset.X / 2 != cubePos.X)
			{
				zero.X = 2;
			}
			if (boneOffset.Y % 2 != 0)
			{
				zero.Y = 1;
			}
			else if (boneOffset.Y / 2 != cubePos.Y)
			{
				zero.Y = 2;
			}
			if (boneOffset.Z % 2 != 0)
			{
				zero.Z = 1;
			}
			else if (boneOffset.Z / 2 != cubePos.Z)
			{
				zero.Z = 2;
			}
			return zero;
		}

		/// <summary>
		/// Assumes cubeBlock is not null
		/// </summary>
		private Vector3? GetDefinitionOffset(MySlimBlock cubeBlock, Vector3I bonePos)
		{
			Vector3I position = bonePos;
			position -= Vector3I.One;
			cubeBlock.Orientation.GetMatrix(out var result);
			Matrix.Transpose(ref result, out var result2);
			Vector3I.Transform(ref position, ref result2, out var result3);
			result3 += Vector3I.One;
			if (cubeBlock.BlockDefinition.Bones.TryGetValue(result3, out var value))
			{
				Vector3.Transform(ref value, ref result, out var result4);
				return result4;
			}
			return null;
		}
	}
}
