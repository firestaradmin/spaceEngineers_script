using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Havok;
using Sandbox.Engine.Utils;
using VRage.Game;
using VRage.Library.Collections;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	internal class MyCubeBlockCollector : IDisposable
	{
		public struct ShapeInfo
		{
			public int Count;

			public Vector3I Min;

			public Vector3I Max;
		}

		public const bool SHRINK_CONVEX_SHAPE = true;

		private const bool ADD_INNER_BONES_TO_CONVEX = true;

		private const float MAX_BOX_EXTENT = 40f;

		public List<ShapeInfo> ShapeInfos = new List<ShapeInfo>();

		public List<HkShape> Shapes = new List<HkShape>();

		private HashSet<MySlimBlock> m_tmpRefreshSet = new HashSet<MySlimBlock>();

		private MyList<Vector3> m_tmpHelperVerts = new MyList<Vector3>();

		private List<Vector3I> m_tmpCubes = new List<Vector3I>();

		private HashSet<Vector3I> m_tmpCheck;

		public void Dispose()
		{
			Clear();
		}

		public void Clear()
		{
			ShapeInfos.Clear();
			foreach (HkShape shape in Shapes)
			{
				shape.RemoveReference();
			}
			Shapes.Clear();
		}

		/// <summary>
		/// Tests whether there are no overlaps.
		/// </summary>
		private bool IsValid()
		{
			if (m_tmpCheck == null)
			{
				m_tmpCheck = new HashSet<Vector3I>();
			}
			try
			{
<<<<<<< HEAD
				Vector3I item = default(Vector3I);
				foreach (ShapeInfo shapeInfo in ShapeInfos)
				{
					item.X = shapeInfo.Min.X;
					while (item.X <= shapeInfo.Max.X)
=======
				Vector3I vector3I = default(Vector3I);
				foreach (ShapeInfo shapeInfo in ShapeInfos)
				{
					vector3I.X = shapeInfo.Min.X;
					while (vector3I.X <= shapeInfo.Max.X)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						vector3I.Y = shapeInfo.Min.Y;
						while (vector3I.Y <= shapeInfo.Max.Y)
						{
							vector3I.Z = shapeInfo.Min.Z;
							while (vector3I.Z <= shapeInfo.Max.Z)
							{
								if (!m_tmpCheck.Add(vector3I))
								{
									return false;
								}
								vector3I.Z++;
							}
							vector3I.Y++;
						}
						vector3I.X++;
					}
				}
				return true;
			}
			finally
			{
				m_tmpCheck.Clear();
			}
		}

		public void Collect(MyCubeGrid grid, MyVoxelSegmentation segmenter, MyVoxelSegmentationType segmentationType, IDictionary<Vector3I, HkMassElement> massResults)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = grid.GetBlocks().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					if (current.FatBlock is MyCompoundCubeBlock)
					{
						CollectCompoundBlock((MyCompoundCubeBlock)current.FatBlock, massResults);
					}
					else
					{
						CollectBlock(current, current.BlockDefinition.PhysicsOption, massResults);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			AddSegmentedParts(grid.GridSize, segmenter, segmentationType);
			m_tmpCubes.Clear();
		}

		/// <summary>
		/// Intended for quite small refreshes (few blocks).
		/// Collect is faster for large refresh.
		/// Removes also dirty mass elements.
		/// </summary>
		public void CollectArea(MyCubeGrid grid, HashSet<Vector3I> dirtyBlocks, MyVoxelSegmentation segmenter, MyVoxelSegmentationType segmentationType, IDictionary<Vector3I, HkMassElement> massResults)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			using (MyUtils.ReuseCollection(ref m_tmpRefreshSet))
			{
				Enumerator<Vector3I> enumerator = dirtyBlocks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Vector3I current = enumerator.get_Current();
						massResults?.Remove(current);
						MySlimBlock cubeBlock = grid.GetCubeBlock(current);
						if (cubeBlock != null)
						{
							m_tmpRefreshSet.Add(cubeBlock);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				Enumerator<MySlimBlock> enumerator2 = m_tmpRefreshSet.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MySlimBlock current2 = enumerator2.get_Current();
						CollectBlock(current2, current2.BlockDefinition.PhysicsOption, massResults);
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
				AddSegmentedParts(grid.GridSize, segmenter, segmentationType);
				m_tmpCubes.Clear();
			}
		}

		public void CollectMassElements(MyCubeGrid grid, IDictionary<Vector3I, HkMassElement> massResults)
		{
<<<<<<< HEAD
			if (massResults == null)
			{
				return;
			}
			foreach (MySlimBlock block in grid.GetBlocks())
			{
				if (block.FatBlock is MyCompoundCubeBlock)
				{
					foreach (MySlimBlock block2 in ((MyCompoundCubeBlock)block.FatBlock).GetBlocks())
					{
						if (block2.BlockDefinition.BlockTopology == MyBlockTopology.TriangleMesh)
						{
							AddMass(block2, massResults);
						}
					}
				}
				else
				{
					AddMass(block, massResults);
=======
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (massResults == null)
			{
				return;
			}
			Enumerator<MySlimBlock> enumerator = grid.GetBlocks().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					if (current.FatBlock is MyCompoundCubeBlock)
					{
						foreach (MySlimBlock block in ((MyCompoundCubeBlock)current.FatBlock).GetBlocks())
						{
							if (block.BlockDefinition.BlockTopology == MyBlockTopology.TriangleMesh)
							{
								AddMass(block, massResults);
							}
						}
					}
					else
					{
						AddMass(current, massResults);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private void CollectCompoundBlock(MyCompoundCubeBlock compoundBlock, IDictionary<Vector3I, HkMassElement> massResults)
		{
			int count = ShapeInfos.Count;
			foreach (MySlimBlock block in compoundBlock.GetBlocks())
			{
				if (block.BlockDefinition.BlockTopology == MyBlockTopology.TriangleMesh)
				{
					CollectBlock(block, block.BlockDefinition.PhysicsOption, massResults, allowSegmentation: false);
				}
			}
			if (ShapeInfos.Count > count + 1)
			{
				ShapeInfo value = ShapeInfos[count];
				while (ShapeInfos.Count > count + 1)
				{
					int index = ShapeInfos.Count - 1;
					value.Count += ShapeInfos[index].Count;
					ShapeInfos.RemoveAt(index);
				}
				ShapeInfos[count] = value;
			}
		}

		private void AddSegmentedParts(float gridSize, MyVoxelSegmentation segmenter, MyVoxelSegmentationType segmentationType)
		{
			int num = (int)Math.Floor(40f / gridSize);
			Vector3 vector = new Vector3(gridSize * 0.5f);
			if (segmenter != null)
			{
				int mergeIterations = ((segmentationType == MyVoxelSegmentationType.Optimized) ? 1 : 0);
				segmenter.ClearInput();
				foreach (Vector3I tmpCube in m_tmpCubes)
				{
					segmenter.AddInput(tmpCube);
				}
				Vector3I vector3I = default(Vector3I);
				foreach (MyVoxelSegmentation.Segment item in segmenter.FindSegments(segmentationType, mergeIterations))
				{
					vector3I.X = item.Min.X;
					while (vector3I.X <= item.Max.X)
					{
						vector3I.Y = item.Min.Y;
						while (vector3I.Y <= item.Max.Y)
						{
							vector3I.Z = item.Min.Z;
							while (vector3I.Z <= item.Max.Z)
							{
								Vector3I vector3I2 = Vector3I.Min(vector3I + num - 1, item.Max);
								Vector3 min = vector3I * gridSize - vector;
								Vector3 max = vector3I2 * gridSize + vector;
								AddBox(vector3I, vector3I2, ref min, ref max);
								vector3I.Z += num;
							}
							vector3I.Y += num;
						}
						vector3I.X += num;
					}
				}
				return;
			}
			foreach (Vector3I tmpCube2 in m_tmpCubes)
			{
				Vector3 min2 = tmpCube2 * gridSize - vector;
				Vector3 max2 = tmpCube2 * gridSize + vector;
				AddBox(tmpCube2, tmpCube2, ref min2, ref max2);
			}
		}

		private void AddBox(Vector3I minPos, Vector3I maxPos, ref Vector3 min, ref Vector3 max)
		{
			Vector3 vector = (min + max) * 0.5f;
			Vector3 halfExtents = max - vector - MyPerGameSettings.PhysicsConvexRadius;
			HkBoxShape hkBoxShape = new HkBoxShape(halfExtents, MyPerGameSettings.PhysicsConvexRadius);
			HkConvexTranslateShape hkConvexTranslateShape = new HkConvexTranslateShape(hkBoxShape, vector, HkReferencePolicy.TakeOwnership);
			Shapes.Add(hkConvexTranslateShape);
			ShapeInfos.Add(new ShapeInfo
			{
				Count = 1,
				Min = minPos,
				Max = maxPos
			});
		}

		private void CollectBlock(MySlimBlock block, MyPhysicsOption physicsOption, IDictionary<Vector3I, HkMassElement> massResults, bool allowSegmentation = true)
		{
			if (!block.BlockDefinition.HasPhysics || block.CubeGrid == null)
			{
				return;
			}
			if (massResults != null)
			{
				AddMass(block, massResults);
			}
			if (block.BlockDefinition.BlockTopology == MyBlockTopology.Cube)
			{
				AddShapesCube(block, physicsOption);
			}
			else if (physicsOption != 0)
			{
				HkShape[] array = null;
				if (block.FatBlock != null)
				{
					array = block.FatBlock.ModelCollision.HavokCollisionShapes;
				}
				if (array != null && array.Length != 0 && !MyFakes.ENABLE_SIMPLE_GRID_PHYSICS)
				{
					AddShapesCustom(block, array);
				}
				else
				{
					AddShapesBox(block, allowSegmentation);
				}
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void AddShapesCustom(MySlimBlock block, HkShape[] shapes)
		{
<<<<<<< HEAD
			Vector3 translation = block.FatBlock.PositionComp.LocalMatrixRef.Translation;
=======
			Vector3 translation = ((!block.FatBlock.ModelCollision.ExportedWrong) ? block.FatBlock.PositionComp.LocalMatrixRef.Translation : (block.Position * block.CubeGrid.GridSize));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			block.Orientation.GetQuaternion(out var result);
			Vector3 scale = Vector3.One * block.FatBlock.ModelCollision.ScaleFactor;
			if (shapes.Length == 1 && shapes[0].ShapeType == HkShapeType.List)
			{
				HkListShape hkListShape = (HkListShape)shapes[0];
				for (int i = 0; i < hkListShape.TotalChildrenCount; i++)
				{
					HkShape childByIndex = hkListShape.GetChildByIndex(i);
					Shapes.Add(new HkConvexTransformShape((HkConvexShape)childByIndex, ref translation, ref result, ref scale, HkReferencePolicy.None));
				}
			}
			else if (shapes.Length == 1 && shapes[0].ShapeType == HkShapeType.Mopp)
			{
				HkMoppBvTreeShape hkMoppBvTreeShape = (HkMoppBvTreeShape)shapes[0];
				for (int j = 0; j < hkMoppBvTreeShape.ShapeCollection.ShapeCount; j++)
				{
					HkShape shape = hkMoppBvTreeShape.ShapeCollection.GetShape((uint)j, null);
					Shapes.Add(new HkConvexTransformShape((HkConvexShape)shape, ref translation, ref result, ref scale, HkReferencePolicy.None));
				}
			}
			else
			{
				for (int k = 0; k < shapes.Length; k++)
				{
					Shapes.Add(new HkConvexTransformShape((HkConvexShape)shapes[k], ref translation, ref result, ref scale, HkReferencePolicy.None));
				}
			}
			ShapeInfos.Add(new ShapeInfo
			{
				Count = shapes.Length,
				Min = block.Min,
				Max = block.Max
			});
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void AddShapesBox(MySlimBlock block, bool allowSegmentation)
		{
			for (int i = block.Min.X; i <= block.Max.X; i++)
			{
				for (int j = block.Min.Y; j <= block.Max.Y; j++)
				{
					for (int k = block.Min.Z; k <= block.Max.Z; k++)
					{
						Vector3I vector3I = new Vector3I(i, j, k);
						if (allowSegmentation)
						{
							m_tmpCubes.Add(vector3I);
							continue;
						}
						Vector3 min = vector3I * block.CubeGrid.GridSize - new Vector3(block.CubeGrid.GridSize / 2f);
						Vector3 max = vector3I * block.CubeGrid.GridSize + new Vector3(block.CubeGrid.GridSize / 2f);
						AddBox(vector3I, vector3I, ref min, ref max);
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void AddShapesCube(MySlimBlock block, MyPhysicsOption physicsOption)
		{
			MyCubeTopology myCubeTopology = ((block.BlockDefinition.CubeDefinition != null) ? block.BlockDefinition.CubeDefinition.CubeTopology : MyCubeTopology.Box);
			if (MyFakes.ENABLE_SIMPLE_GRID_PHYSICS)
			{
				physicsOption = MyPhysicsOption.Box;
			}
			else if (myCubeTopology == MyCubeTopology.Box)
			{
				if (!block.ShowParts)
				{
					physicsOption = MyPhysicsOption.Box;
				}
				else if (block.BlockDefinition.CubeDefinition != null && block.CubeGrid.Skeleton.IsDeformed(block.Min, 0.05f, block.CubeGrid, checkBlockDefinition: false))
				{
					physicsOption = MyPhysicsOption.Convex;
				}
			}
			switch (physicsOption)
			{
			case MyPhysicsOption.Box:
				AddBoxes(block);
				break;
			case MyPhysicsOption.Convex:
				AddConvexShape(block, block.ShowParts);
				break;
			}
		}

		private void AddMass(MySlimBlock block, IDictionary<Vector3I, HkMassElement> massResults)
		{
			float num = block.BlockDefinition.Mass;
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && block.FatBlock is MyCompoundCubeBlock)
			{
				num = 0f;
				foreach (MySlimBlock block2 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
				{
					num += block2.GetMass();
				}
			}
			Vector3 vector = (block.Max - block.Min + Vector3I.One) * block.CubeGrid.GridSize;
			Vector3 position = (block.Min + block.Max) * 0.5f * block.CubeGrid.GridSize;
			HkMassProperties hkMassProperties = default(HkMassProperties);
			hkMassProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(vector / 2f, num);
			massResults[block.Position] = new HkMassElement
			{
				Properties = hkMassProperties,
				Tranform = Matrix.CreateTranslation(position)
			};
		}

		private void AddConvexShape(MySlimBlock block, bool applySkeleton)
		{
			m_tmpHelperVerts.Clear();
			Vector3 vector = block.Min * block.CubeGrid.GridSize;
			Vector3I vector3I = block.Min * 2 + 1;
			MyGridSkeleton skeleton = block.CubeGrid.Skeleton;
			foreach (Vector3 blockVertex in MyBlockVerticesCache.GetBlockVertices(block.BlockDefinition.CubeDefinition.CubeTopology, block.Orientation))
			{
				Vector3I pos = vector3I + Vector3I.Round(blockVertex);
				Vector3 vector2 = blockVertex * block.CubeGrid.GridSizeHalf;
				if (applySkeleton && skeleton.TryGetBone(ref pos, out var bone))
				{
					vector2.Add(bone);
				}
				m_tmpHelperVerts.Add(vector2 + vector);
			}
			ShapeInfo item;
			if (block.BlockDefinition.CubeDefinition.CubeTopology == MyCubeTopology.CornerSquareInverted)
			{
				MyList<Vector3> range = m_tmpHelperVerts.GetRange(0, m_tmpHelperVerts.Count / 2);
				MyList<Vector3> range2 = m_tmpHelperVerts.GetRange(m_tmpHelperVerts.Count / 2, m_tmpHelperVerts.Count / 2);
				Shapes.Add(new HkConvexVerticesShape(range.GetInternalArray(), range.Count));
				Shapes.Add(new HkConvexVerticesShape(range2.GetInternalArray(), range2.Count));
				List<ShapeInfo> shapeInfos = ShapeInfos;
				item = new ShapeInfo
				{
					Count = 2,
					Min = block.Min,
					Max = block.Max
				};
				shapeInfos.Add(item);
			}
			else
			{
				Shapes.Add(new HkConvexVerticesShape(m_tmpHelperVerts.GetInternalArray(), m_tmpHelperVerts.Count, shrink: true, MyPerGameSettings.PhysicsConvexRadius));
				List<ShapeInfo> shapeInfos2 = ShapeInfos;
				item = new ShapeInfo
				{
					Count = 1,
					Min = block.Min,
					Max = block.Max
				};
				shapeInfos2.Add(item);
			}
		}

		/// <param name="block"></param>        
		private void AddBoxes(MySlimBlock block)
		{
			for (int i = block.Min.X; i <= block.Max.X; i++)
			{
				for (int j = block.Min.Y; j <= block.Max.Y; j++)
				{
					for (int k = block.Min.Z; k <= block.Max.Z; k++)
					{
						Vector3I item = new Vector3I(i, j, k);
						m_tmpCubes.Add(item);
					}
				}
			}
		}
	}
}
