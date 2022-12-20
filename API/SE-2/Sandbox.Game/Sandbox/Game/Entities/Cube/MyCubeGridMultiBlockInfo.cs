<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Linq;
using Sandbox.Definitions;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	/// <summary>
	/// Helper data for multiblock in a grid.
	/// </summary>
	public class MyCubeGridMultiBlockInfo
	{
		private static List<MyMultiBlockDefinition.MyMultiBlockPartDefinition> m_tmpPartDefinitions = new List<MyMultiBlockDefinition.MyMultiBlockPartDefinition>();

		public int MultiBlockId;

		public MyMultiBlockDefinition MultiBlockDefinition;

		public MyCubeBlockDefinition MainBlockDefinition;

		public HashSet<MySlimBlock> Blocks = new HashSet<MySlimBlock>();

		public bool GetTransform(out MatrixI transform)
		{
			transform = default(MatrixI);
			if (Blocks.get_Count() != 0)
			{
				MySlimBlock mySlimBlock = Enumerable.First<MySlimBlock>((IEnumerable<MySlimBlock>)Blocks);
				if (mySlimBlock.MultiBlockIndex < MultiBlockDefinition.BlockDefinitions.Length)
				{
					MyMultiBlockDefinition.MyMultiBlockPartDefinition myMultiBlockPartDefinition = MultiBlockDefinition.BlockDefinitions[mySlimBlock.MultiBlockIndex];
					transform = MatrixI.CreateRotation(myMultiBlockPartDefinition.Forward, myMultiBlockPartDefinition.Up, mySlimBlock.Orientation.Forward, mySlimBlock.Orientation.Up);
					transform.Translation = mySlimBlock.Position - Vector3I.TransformNormal(myMultiBlockPartDefinition.Min, ref transform);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns multiblok min and max grid coordinates.
		/// </summary>
		public bool GetBoundingBox(out Vector3I min, out Vector3I max)
		{
			min = default(Vector3I);
			max = default(Vector3I);
			if (!GetTransform(out var transform))
			{
				return false;
			}
			Vector3I value = Vector3I.Transform(MultiBlockDefinition.Min, transform);
			Vector3I value2 = Vector3I.Transform(MultiBlockDefinition.Max, transform);
			min = Vector3I.Min(value, value2);
			max = Vector3I.Max(value, value2);
			return true;
		}

		public bool GetMissingBlocks(out MatrixI transform, List<int> multiBlockIndices)
		{
			int i;
			for (i = 0; i < MultiBlockDefinition.BlockDefinitions.Length; i++)
			{
				if (!Enumerable.Any<MySlimBlock>((IEnumerable<MySlimBlock>)Blocks, (Func<MySlimBlock, bool>)((MySlimBlock b) => b.MultiBlockIndex == i)))
				{
					multiBlockIndices.Add(i);
				}
			}
			return GetTransform(out transform);
		}

		/// <summary>
		/// Check if other block can be added to area of multiblock.
		/// </summary>
		public bool CanAddBlock(ref Vector3I otherGridPositionMin, ref Vector3I otherGridPositionMax, MyBlockOrientation otherOrientation, MyCubeBlockDefinition otherDefinition)
		{
			if (!GetTransform(out var transform))
			{
				return true;
			}
			try
			{
				MatrixI.Invert(ref transform, out var result);
				Vector3I value = Vector3I.Transform(otherGridPositionMin, ref result);
				Vector3I value2 = Vector3I.Transform(otherGridPositionMax, ref result);
				Vector3I minB = Vector3I.Min(value, value2);
				Vector3I maxB = Vector3I.Max(value, value2);
				if (!Vector3I.BoxIntersects(ref MultiBlockDefinition.Min, ref MultiBlockDefinition.Max, ref minB, ref maxB))
				{
					return true;
				}
				MatrixI leftMatrix = new MatrixI(otherOrientation);
				MatrixI.Multiply(ref leftMatrix, ref result, out var result2);
				MyBlockOrientation otherOrientation2 = new MyBlockOrientation(result2.Forward, result2.Up);
				m_tmpPartDefinitions.Clear();
				MyMultiBlockDefinition.MyMultiBlockPartDefinition[] blockDefinitions = MultiBlockDefinition.BlockDefinitions;
				foreach (MyMultiBlockDefinition.MyMultiBlockPartDefinition myMultiBlockPartDefinition in blockDefinitions)
				{
					if (Vector3I.BoxIntersects(ref myMultiBlockPartDefinition.Min, ref myMultiBlockPartDefinition.Max, ref minB, ref maxB))
					{
						if (!(minB == maxB) || !(myMultiBlockPartDefinition.Min == myMultiBlockPartDefinition.Max))
						{
							return false;
						}
						m_tmpPartDefinitions.Add(myMultiBlockPartDefinition);
					}
				}
				if (m_tmpPartDefinitions.Count == 0)
				{
					return true;
				}
				bool flag = true;
				foreach (MyMultiBlockDefinition.MyMultiBlockPartDefinition tmpPartDefinition in m_tmpPartDefinitions)
				{
					if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(tmpPartDefinition.Id, out var blockDefinition) && blockDefinition != null)
					{
						flag &= MyCompoundCubeBlock.CanAddBlocks(blockDefinition, new MyBlockOrientation(tmpPartDefinition.Forward, tmpPartDefinition.Up), otherDefinition, otherOrientation2);
						if (!flag)
						{
							break;
						}
					}
				}
				return flag;
			}
			finally
			{
				m_tmpPartDefinitions.Clear();
			}
		}

		public bool IsFractured()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = Blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.get_Current().GetFractureComponent() != null)
					{
						return true;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return false;
		}

		public float GetTotalMaxIntegrity()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			float num = 0f;
			Enumerator<MySlimBlock> enumerator = Blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					num += current.MaxIntegrity;
				}
				return num;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}
	}
}
