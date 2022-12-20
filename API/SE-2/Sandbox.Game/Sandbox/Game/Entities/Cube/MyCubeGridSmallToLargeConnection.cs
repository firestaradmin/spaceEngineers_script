using System;
using System.Collections.Generic;
using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Multiplayer;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Groups;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	/// <summary>
	/// Class which handles small block to large one connection. Such connection creates block and grid groups so connected grids can be copied together.
	/// It is done on server and client, but client uses it only for groups (copying of all grids together), dynamic grid testing is processed on server only.
	/// </summary>
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyCubeGridSmallToLargeConnection : MySessionComponentBase
	{
		private struct MySlimBlockPair : IEquatable<MySlimBlockPair>
		{
			public MySlimBlock Parent;

			public MySlimBlock Child;

			public override int GetHashCode()
			{
				return Parent.GetHashCode() ^ Child.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				if (!(obj is MySlimBlockPair))
				{
					return false;
				}
				MySlimBlockPair mySlimBlockPair = (MySlimBlockPair)obj;
				if (Parent == mySlimBlockPair.Parent)
				{
					return Child == mySlimBlockPair.Child;
				}
				return false;
			}

			public bool Equals(MySlimBlockPair other)
			{
				if (Parent == other.Parent)
				{
					return Child == other.Child;
				}
				return false;
			}
		}

		private static readonly HashSet<MyCubeBlock> m_tmpBlocks = new HashSet<MyCubeBlock>();

		private static readonly HashSet<MySlimBlock> m_tmpSlimBlocks = new HashSet<MySlimBlock>();

		private static readonly HashSet<MySlimBlock> m_tmpSlimBlocks2 = new HashSet<MySlimBlock>();

		private static readonly List<MySlimBlock> m_tmpSlimBlocksList = new List<MySlimBlock>();

		private static readonly HashSet<MyCubeGrid> m_tmpGrids = new HashSet<MyCubeGrid>();

		private static readonly List<MyCubeGrid> m_tmpGridList = new List<MyCubeGrid>();

		private static bool m_smallToLargeCheckEnabled = true;

		private static readonly List<MySlimBlockPair> m_tmpBlockConnections = new List<MySlimBlockPair>();

		public static MyCubeGridSmallToLargeConnection Static;

		private readonly Dictionary<MyCubeGrid, HashSet<MySlimBlockPair>> m_mapLargeGridToConnectedBlocks = new Dictionary<MyCubeGrid, HashSet<MySlimBlockPair>>();

		private readonly Dictionary<MyCubeGrid, HashSet<MySlimBlockPair>> m_mapSmallGridToConnectedBlocks = new Dictionary<MyCubeGrid, HashSet<MySlimBlockPair>>();

		public override bool IsRequiredByGame
		{
			get
			{
				if (base.IsRequiredByGame)
				{
					return MyFakes.ENABLE_SMALL_BLOCK_TO_LARGE_STATIC_CONNECTIONS;
				}
				return false;
			}
		}

		public override void LoadData()
		{
			base.LoadData();
			Static = this;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Static = null;
		}

		/// <summary>
		/// Writes all surrounding blocks around the given one with the given size.
		/// </summary>
		private void GetSurroundingBlocksFromStaticGrids(MySlimBlock block, MyCubeSize cubeSizeEnum, HashSet<MyCubeBlock> outBlocks)
		{
			outBlocks.Clear();
			BoundingBoxD boundingBoxD = new BoundingBoxD(block.Min * block.CubeGrid.GridSize - block.CubeGrid.GridSize / 2f, block.Max * block.CubeGrid.GridSize + block.CubeGrid.GridSize / 2f);
			if (block.FatBlock != null)
			{
				Vector3D center = boundingBoxD.Center;
				boundingBoxD = block.FatBlock.Model.BoundingBox;
				block.FatBlock.Orientation.GetMatrix(out var result);
				boundingBoxD = boundingBoxD.TransformFast(result);
				boundingBoxD.Translate(center);
			}
			boundingBoxD = boundingBoxD.TransformFast(block.CubeGrid.WorldMatrix);
			boundingBoxD.Inflate(0.125);
			List<MyEntity> list = new List<MyEntity>();
			MyEntities.GetElementsInBox(ref boundingBoxD, list);
			for (int i = 0; i < list.Count; i++)
			{
				MyCubeBlock myCubeBlock = list[i] as MyCubeBlock;
				if (myCubeBlock == null || myCubeBlock.SlimBlock == block || !myCubeBlock.CubeGrid.IsStatic || !myCubeBlock.CubeGrid.EnableSmallToLargeConnections || !myCubeBlock.CubeGrid.SmallToLargeConnectionsInitialized || myCubeBlock.CubeGrid == block.CubeGrid || myCubeBlock.CubeGrid.GridSizeEnum != cubeSizeEnum || myCubeBlock is MyFracturedBlock || myCubeBlock.Components.Has<MyFractureComponentBase>())
<<<<<<< HEAD
				{
					continue;
				}
				MyCompoundCubeBlock myCompoundCubeBlock = myCubeBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock != null)
				{
=======
				{
					continue;
				}
				MyCompoundCubeBlock myCompoundCubeBlock = myCubeBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock != null)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
					{
						if (block2 != block && !block2.FatBlock.Components.Has<MyFractureComponentBase>())
						{
							outBlocks.Add(block2.FatBlock);
						}
					}
				}
				else
				{
					outBlocks.Add(myCubeBlock);
				}
			}
			list.Clear();
		}

		/// <summary>
		/// Writes all surrounding blocks around the given one with the given size.
		/// </summary>
		private void GetSurroundingBlocksFromStaticGrids(MySlimBlock block, MyCubeSize cubeSizeEnum, HashSet<MySlimBlock> outBlocks)
		{
			outBlocks.Clear();
			BoundingBoxD aabbForNeighbors = new BoundingBoxD(block.Min * block.CubeGrid.GridSize, block.Max * block.CubeGrid.GridSize);
			BoundingBoxD box = new BoundingBoxD(block.Min * block.CubeGrid.GridSize - block.CubeGrid.GridSize / 2f, block.Max * block.CubeGrid.GridSize + block.CubeGrid.GridSize / 2f);
			if (block.FatBlock != null)
			{
				Vector3D center = box.Center;
				box = block.FatBlock.Model.BoundingBox;
				block.FatBlock.Orientation.GetMatrix(out var result);
				box = box.TransformFast(result);
				box.Translate(center);
			}
			box.Inflate(0.125);
			BoundingBoxD boundingBox = box.TransformFast(block.CubeGrid.WorldMatrix);
			List<MyEntity> list = new List<MyEntity>();
			MyEntities.GetElementsInBox(ref boundingBox, list);
			for (int i = 0; i < list.Count; i++)
			{
				MyCubeGrid myCubeGrid = list[i] as MyCubeGrid;
				if (myCubeGrid == null || !myCubeGrid.IsStatic || myCubeGrid == block.CubeGrid || !myCubeGrid.EnableSmallToLargeConnections || !myCubeGrid.SmallToLargeConnectionsInitialized || myCubeGrid.GridSizeEnum != cubeSizeEnum)
<<<<<<< HEAD
				{
					continue;
				}
				m_tmpSlimBlocksList.Clear();
				MatrixD boxTransform = block.CubeGrid.WorldMatrix;
				myCubeGrid.GetBlocksIntersectingOBB(in box, in boxTransform, m_tmpSlimBlocksList);
				CheckNeighborBlocks(block, aabbForNeighbors, myCubeGrid, m_tmpSlimBlocksList);
				foreach (MySlimBlock tmpSlimBlocks in m_tmpSlimBlocksList)
				{
					if (tmpSlimBlocks.FatBlock != null)
					{
						if (tmpSlimBlocks.FatBlock is MyFracturedBlock || tmpSlimBlocks.FatBlock.Components.Has<MyFractureComponentBase>())
						{
							continue;
						}
						if (tmpSlimBlocks.FatBlock is MyCompoundCubeBlock)
						{
=======
				{
					continue;
				}
				m_tmpSlimBlocksList.Clear();
				MatrixD boxTransform = block.CubeGrid.WorldMatrix;
				myCubeGrid.GetBlocksIntersectingOBB(in box, in boxTransform, m_tmpSlimBlocksList);
				CheckNeighborBlocks(block, aabbForNeighbors, myCubeGrid, m_tmpSlimBlocksList);
				foreach (MySlimBlock tmpSlimBlocks in m_tmpSlimBlocksList)
				{
					if (tmpSlimBlocks.FatBlock != null)
					{
						if (tmpSlimBlocks.FatBlock is MyFracturedBlock || tmpSlimBlocks.FatBlock.Components.Has<MyFractureComponentBase>())
						{
							continue;
						}
						if (tmpSlimBlocks.FatBlock is MyCompoundCubeBlock)
						{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							foreach (MySlimBlock block2 in (tmpSlimBlocks.FatBlock as MyCompoundCubeBlock).GetBlocks())
							{
								if (!block2.FatBlock.Components.Has<MyFractureComponentBase>())
								{
									outBlocks.Add(block2);
								}
							}
						}
						else
						{
							outBlocks.Add(tmpSlimBlocks);
						}
					}
					else
					{
						outBlocks.Add(tmpSlimBlocks);
					}
				}
				m_tmpSlimBlocksList.Clear();
			}
			list.Clear();
		}

		/// <summary>
		/// Checks if blocks are neigbors to block(s) in aabbForNeighbors.
		/// </summary>
		private static void CheckNeighborBlocks(MySlimBlock block, BoundingBoxD aabbForNeighbors, MyCubeGrid cubeGrid, List<MySlimBlock> blocks)
		{
			MatrixD m = block.CubeGrid.WorldMatrix * cubeGrid.PositionComp.WorldMatrixNormalizedInv;
			BoundingBoxD boundingBoxD = aabbForNeighbors.TransformFast(ref m);
			Vector3I value = Vector3I.Round(cubeGrid.GridSizeR * boundingBoxD.Min);
			Vector3I value2 = Vector3I.Round(cubeGrid.GridSizeR * boundingBoxD.Max);
			Vector3I start = Vector3I.Min(value, value2);
			Vector3I end = Vector3I.Max(value, value2);
			for (int num = blocks.Count - 1; num >= 0; num--)
			{
				MySlimBlock mySlimBlock = blocks[num];
				bool flag = false;
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref mySlimBlock.Min, ref mySlimBlock.Max);
				Vector3I next = vector3I_RangeIterator.Current;
				while (vector3I_RangeIterator.IsValid())
				{
					Vector3I_RangeIterator vector3I_RangeIterator2 = new Vector3I_RangeIterator(ref start, ref end);
					Vector3I next2 = vector3I_RangeIterator2.Current;
					while (vector3I_RangeIterator2.IsValid())
					{
						Vector3I vector3I = Vector3I.Abs(next - next2);
						if (next2 == next || vector3I.X + vector3I.Y + vector3I.Z == 1)
						{
							flag = true;
							break;
						}
						vector3I_RangeIterator2.GetNext(out next2);
					}
					if (flag)
					{
						break;
					}
					vector3I_RangeIterator.GetNext(out next);
				}
				if (!flag)
				{
					blocks.RemoveAt(num);
				}
			}
		}

		/// <summary>
		/// Adds small/large block connection.
		/// </summary>
		private void ConnectSmallToLargeBlock(MySlimBlock smallBlock, MySlimBlock largeBlock)
		{
			if (GetCubeSize(smallBlock) != MyCubeSize.Small || GetCubeSize(largeBlock) != 0 || smallBlock.FatBlock is MyCompoundCubeBlock || largeBlock.FatBlock is MyCompoundCubeBlock)
			{
				return;
			}
			long linkId = ((long)largeBlock.UniqueId << 32) + smallBlock.UniqueId;
			if (!MyCubeGridGroups.Static.SmallToLargeBlockConnections.LinkExists(linkId, largeBlock))
			{
				MyCubeGridGroups.Static.SmallToLargeBlockConnections.CreateLink(linkId, largeBlock, smallBlock);
				MyCubeGridGroups.Static.Physical.CreateLink(linkId, largeBlock.CubeGrid, smallBlock.CubeGrid);
				MyCubeGridGroups.Static.Logical.CreateLink(linkId, largeBlock.CubeGrid, smallBlock.CubeGrid);
<<<<<<< HEAD
				MySlimBlockPair item = default(MySlimBlockPair);
				item.Parent = largeBlock;
				item.Child = smallBlock;
=======
				MySlimBlockPair mySlimBlockPair = default(MySlimBlockPair);
				mySlimBlockPair.Parent = largeBlock;
				mySlimBlockPair.Child = smallBlock;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!m_mapLargeGridToConnectedBlocks.TryGetValue(largeBlock.CubeGrid, out var value))
				{
					value = new HashSet<MySlimBlockPair>();
					m_mapLargeGridToConnectedBlocks.Add(largeBlock.CubeGrid, value);
					largeBlock.CubeGrid.OnClosing += CubeGrid_OnClosing;
				}
<<<<<<< HEAD
				value.Add(item);
=======
				value.Add(mySlimBlockPair);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!m_mapSmallGridToConnectedBlocks.TryGetValue(smallBlock.CubeGrid, out var value2))
				{
					value2 = new HashSet<MySlimBlockPair>();
					m_mapSmallGridToConnectedBlocks.Add(smallBlock.CubeGrid, value2);
					smallBlock.CubeGrid.OnClosing += CubeGrid_OnClosing;
				}
				value2.Add(mySlimBlockPair);
			}
		}

		/// <summary>
		/// Removes small/large block connection. Note that grids in parameters can be different from the ones in slimblock! Used for grid splits, etc.
		/// </summary>
		private void DisconnectSmallToLargeBlock(MySlimBlock smallBlock, MyCubeGrid smallGrid, MySlimBlock largeBlock, MyCubeGrid largeGrid)
		{
			if (GetCubeSize(smallBlock) != MyCubeSize.Small || GetCubeSize(largeBlock) != 0 || smallBlock.FatBlock is MyCompoundCubeBlock || largeBlock.FatBlock is MyCompoundCubeBlock)
			{
				return;
			}
			long linkId = ((long)largeBlock.UniqueId << 32) + smallBlock.UniqueId;
			MyCubeGridGroups.Static.SmallToLargeBlockConnections.BreakLink(linkId, largeBlock);
			MyCubeGridGroups.Static.Physical.BreakLink(linkId, largeGrid);
			MyCubeGridGroups.Static.Logical.BreakLink(linkId, largeGrid);
<<<<<<< HEAD
			MySlimBlockPair item = default(MySlimBlockPair);
			item.Parent = largeBlock;
			item.Child = smallBlock;
=======
			MySlimBlockPair mySlimBlockPair = default(MySlimBlockPair);
			mySlimBlockPair.Parent = largeBlock;
			mySlimBlockPair.Child = smallBlock;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (m_mapLargeGridToConnectedBlocks.TryGetValue(largeGrid, out var value))
			{
				value.Remove(mySlimBlockPair);
				if (value.get_Count() == 0)
				{
					m_mapLargeGridToConnectedBlocks.Remove(largeGrid);
					largeGrid.OnClosing -= CubeGrid_OnClosing;
				}
			}
			if (m_mapSmallGridToConnectedBlocks.TryGetValue(smallGrid, out var value2))
			{
				value2.Remove(mySlimBlockPair);
				if (value2.get_Count() == 0)
				{
					m_mapSmallGridToConnectedBlocks.Remove(smallGrid);
					smallGrid.OnClosing -= CubeGrid_OnClosing;
				}
			}
		}

		/// <summary>
		/// Removes small/large block connection.
		/// </summary>
		private void DisconnectSmallToLargeBlock(MySlimBlock smallBlock, MySlimBlock largeBlock)
		{
			DisconnectSmallToLargeBlock(smallBlock, smallBlock.CubeGrid, largeBlock, largeBlock.CubeGrid);
		}

		/// <summary>
		/// Adds possible connections of grid blocks.  
		/// </summary>
		/// <returns>Returns true when small/large block connection has been added otherwise false.</returns>
		internal bool AddGridSmallToLargeConnection(MyCubeGrid grid)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if (!grid.IsStatic)
			{
				return false;
			}
			if (!grid.EnableSmallToLargeConnections || !grid.SmallToLargeConnectionsInitialized)
			{
				return false;
			}
			bool flag = false;
			Enumerator<MySlimBlock> enumerator = grid.GetBlocks().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
<<<<<<< HEAD
					bool flag2 = AddBlockSmallToLargeConnection(block);
					flag = flag || flag2;
=======
					MySlimBlock current = enumerator.get_Current();
					if (!(current.FatBlock is MyFracturedBlock))
					{
						bool flag2 = AddBlockSmallToLargeConnection(current);
						flag = flag || flag2;
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				return flag;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		/// <summary>
		/// Adds small/large block static connections and creates links. Returns true if the block connects to any other block.
		/// </summary>
		public bool AddBlockSmallToLargeConnection(MySlimBlock block)
		{
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			if (!m_smallToLargeCheckEnabled)
			{
				return true;
			}
			if (!block.CubeGrid.IsStatic || !block.CubeGrid.EnableSmallToLargeConnections || !block.CubeGrid.SmallToLargeConnectionsInitialized || (block.FatBlock != null && block.FatBlock.Components.Has<MyFractureComponentBase>()))
			{
				return false;
			}
			bool flag = false;
			if (block.FatBlock is MyCompoundCubeBlock)
			{
				foreach (MySlimBlock block2 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
				{
					bool flag2 = AddBlockSmallToLargeConnection(block2);
					flag = flag || flag2;
				}
				return flag;
			}
			MyCubeSize cubeSizeEnum = ((GetCubeSize(block) == MyCubeSize.Large) ? MyCubeSize.Small : MyCubeSize.Large);
			GetSurroundingBlocksFromStaticGrids(block, cubeSizeEnum, m_tmpSlimBlocks2);
			if (m_tmpSlimBlocks2.get_Count() == 0)
			{
				return false;
			}
			MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Small);
			block.GetWorldBoundingBox(out var aabb);
			aabb.Inflate(0.05);
			Enumerator<MySlimBlock> enumerator2;
			if (GetCubeSize(block) == MyCubeSize.Large)
			{
				enumerator2 = m_tmpSlimBlocks2.GetEnumerator();
				try
				{
<<<<<<< HEAD
					item.GetWorldBoundingBox(out var aabb2);
					if (aabb2.Intersects(aabb) && SmallBlockConnectsToLarge(item, ref aabb2, block, ref aabb))
=======
					while (enumerator2.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MySlimBlock current2 = enumerator2.get_Current();
						current2.GetWorldBoundingBox(out var aabb2);
						if (aabb2.Intersects(aabb) && SmallBlockConnectsToLarge(current2, ref aabb2, block, ref aabb))
						{
							ConnectSmallToLargeBlock(current2, block);
							flag = true;
						}
					}
					return flag;
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
			}
			enumerator2 = m_tmpSlimBlocks2.GetEnumerator();
			try
			{
<<<<<<< HEAD
				item2.GetWorldBoundingBox(out var aabb3);
				if (aabb3.Intersects(aabb) && SmallBlockConnectsToLarge(block, ref aabb, item2, ref aabb3))
=======
				while (enumerator2.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MySlimBlock current3 = enumerator2.get_Current();
					current3.GetWorldBoundingBox(out var aabb3);
					if (aabb3.Intersects(aabb) && SmallBlockConnectsToLarge(block, ref aabb, current3, ref aabb3))
					{
						ConnectSmallToLargeBlock(block, current3);
						flag = true;
					}
				}
				return flag;
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
		}

		/// <summary>
		/// Block has been removed and all small/large static connections must be removed.
		/// </summary>
		internal void RemoveBlockSmallToLargeConnection(MySlimBlock block)
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			if (!m_smallToLargeCheckEnabled || !block.CubeGrid.IsStatic)
			{
				return;
			}
			MyCompoundCubeBlock myCompoundCubeBlock = block.FatBlock as MyCompoundCubeBlock;
			if (myCompoundCubeBlock != null)
			{
				foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
				{
					RemoveBlockSmallToLargeConnection(block2);
				}
				return;
			}
			m_tmpGrids.Clear();
			if (GetCubeSize(block) == MyCubeSize.Large)
			{
				RemoveChangedLargeBlockConnectionToSmallBlocks(block, m_tmpGrids);
				if (Sync.IsServer)
				{
					Enumerator<MyCubeGrid> enumerator2 = m_tmpGrids.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							MyCubeGrid current2 = enumerator2.get_Current();
							if (current2.TestDynamic == MyCubeGrid.MyTestDynamicReason.NoReason && !SmallGridIsStatic(current2))
							{
								current2.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
				m_tmpGrids.Clear();
				return;
			}
			MyGroups<MySlimBlock, MyBlockGroupData>.Group group = MyCubeGridGroups.Static.SmallToLargeBlockConnections.GetGroup(block);
			if (group == null)
			{
				if (Sync.IsServer && block.CubeGrid.GetBlocks().get_Count() > 0 && block.CubeGrid.TestDynamic == MyCubeGrid.MyTestDynamicReason.NoReason && !SmallGridIsStatic(block.CubeGrid))
				{
					block.CubeGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
				}
				return;
			}
			m_tmpSlimBlocks.Clear();
			Enumerator<MyGroups<MySlimBlock, MyBlockGroupData>.Node> enumerator3 = group.Nodes.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					MyGroups<MySlimBlock, MyBlockGroupData>.Node current3 = enumerator3.get_Current();
					Enumerator<long, MyGroups<MySlimBlock, MyBlockGroupData>.Node> enumerator4 = current3.Children.GetEnumerator();
					try
					{
						while (enumerator4.MoveNext())
						{
							if (enumerator4.get_Current().NodeData == block)
							{
								m_tmpSlimBlocks.Add(current3.NodeData);
								break;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator4).Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)enumerator3).Dispose();
			}
			Enumerator<MySlimBlock> enumerator5 = m_tmpSlimBlocks.GetEnumerator();
			try
			{
				while (enumerator5.MoveNext())
				{
					MySlimBlock current4 = enumerator5.get_Current();
					DisconnectSmallToLargeBlock(block, current4);
				}
			}
			finally
			{
				((IDisposable)enumerator5).Dispose();
			}
			m_tmpSlimBlocks.Clear();
<<<<<<< HEAD
			if (Sync.IsServer && !m_mapSmallGridToConnectedBlocks.TryGetValue(block.CubeGrid, out var _) && block.CubeGrid.GetBlocks().Count > 0 && block.CubeGrid.TestDynamic == MyCubeGrid.MyTestDynamicReason.NoReason && !SmallGridIsStatic(block.CubeGrid))
=======
			if (Sync.IsServer && !m_mapSmallGridToConnectedBlocks.TryGetValue(block.CubeGrid, out var _) && block.CubeGrid.GetBlocks().get_Count() > 0 && block.CubeGrid.TestDynamic == MyCubeGrid.MyTestDynamicReason.NoReason && !SmallGridIsStatic(block.CubeGrid))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				block.CubeGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
			}
		}

		/// <summary>
		/// Grid has been converted to dynamic, all small to large connections must be removed.
		/// </summary>
		internal void ConvertToDynamic(MyCubeGrid grid)
		{
			if (grid.GridSizeEnum == MyCubeSize.Small)
			{
				RemoveSmallGridConnections(grid);
			}
			else
			{
				RemoveLargeGridConnections(grid);
			}
		}

		private void RemoveLargeGridConnections(MyCubeGrid grid)
		{
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			m_tmpGrids.Clear();
			if (!m_mapLargeGridToConnectedBlocks.TryGetValue(grid, out var value))
			{
				return;
			}
			m_tmpBlockConnections.Clear();
<<<<<<< HEAD
			m_tmpBlockConnections.AddRange(value);
=======
			m_tmpBlockConnections.AddRange((IEnumerable<MySlimBlockPair>)value);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (MySlimBlockPair tmpBlockConnection in m_tmpBlockConnections)
			{
				DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
				m_tmpGrids.Add(tmpBlockConnection.Child.CubeGrid);
			}
			m_tmpBlockConnections.Clear();
			if (Sync.IsServer)
			{
				m_tmpGridList.Clear();
<<<<<<< HEAD
				foreach (MyCubeGrid tmpGrid in m_tmpGrids)
				{
					if (m_mapSmallGridToConnectedBlocks.ContainsKey(tmpGrid))
					{
						m_tmpGridList.Add(tmpGrid);
					}
				}
				foreach (MyCubeGrid tmpGrid2 in m_tmpGridList)
				{
					m_tmpGrids.Remove(tmpGrid2);
				}
				m_tmpGridList.Clear();
				foreach (MyCubeGrid tmpGrid3 in m_tmpGrids)
				{
					if (tmpGrid3.IsStatic && tmpGrid3.TestDynamic == MyCubeGrid.MyTestDynamicReason.NoReason && !SmallGridIsStatic(tmpGrid3))
					{
						tmpGrid3.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
					}
				}
=======
				Enumerator<MyCubeGrid> enumerator2 = m_tmpGrids.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MyCubeGrid current2 = enumerator2.get_Current();
						if (m_mapSmallGridToConnectedBlocks.ContainsKey(current2))
						{
							m_tmpGridList.Add(current2);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
				foreach (MyCubeGrid tmpGrid in m_tmpGridList)
				{
					m_tmpGrids.Remove(tmpGrid);
				}
				m_tmpGridList.Clear();
				enumerator2 = m_tmpGrids.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MyCubeGrid current4 = enumerator2.get_Current();
						if (current4.IsStatic && current4.TestDynamic == MyCubeGrid.MyTestDynamicReason.NoReason && !SmallGridIsStatic(current4))
						{
							current4.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_tmpGrids.Clear();
		}

		private void RemoveSmallGridConnections(MyCubeGrid grid)
		{
			if (!m_mapSmallGridToConnectedBlocks.TryGetValue(grid, out var value))
			{
				return;
<<<<<<< HEAD
			}
			m_tmpBlockConnections.Clear();
			m_tmpBlockConnections.AddRange(value);
			foreach (MySlimBlockPair tmpBlockConnection in m_tmpBlockConnections)
			{
				DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
			}
			m_tmpBlockConnections.Clear();
=======
			}
			m_tmpBlockConnections.Clear();
			m_tmpBlockConnections.AddRange((IEnumerable<MySlimBlockPair>)value);
			foreach (MySlimBlockPair tmpBlockConnection in m_tmpBlockConnections)
			{
				DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
			}
			m_tmpBlockConnections.Clear();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		/// <summary>
		/// Tests whether the given small grid connects to any large static block.
		/// </summary>
		/// <returns>true if small grid connects to a latge grid otherwise false</returns>
		public bool TestGridSmallToLargeConnection(MyCubeGrid smallGrid)
		{
			if (!smallGrid.IsStatic)
			{
				return false;
			}
			if (!Sync.IsServer)
			{
				return true;
			}
<<<<<<< HEAD
			if (m_mapSmallGridToConnectedBlocks.TryGetValue(smallGrid, out var value) && value.Count > 0)
=======
			if (m_mapSmallGridToConnectedBlocks.TryGetValue(smallGrid, out var value) && value.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Returns small block add direction (returns large block add normal). 
		/// Assumes that smallBlockWorldAabb intersects largeBlockWorldAabb and smallBlockWorldAabbReduced does not intersects largeBlockWorldAabb.
		/// </summary>
		private Vector3I GetSmallBlockAddDirection(ref BoundingBoxD smallBlockWorldAabb, ref BoundingBoxD smallBlockWorldAabbReduced, ref BoundingBoxD largeBlockWorldAabb)
		{
			if (smallBlockWorldAabbReduced.Min.X > largeBlockWorldAabb.Max.X && smallBlockWorldAabb.Min.X <= largeBlockWorldAabb.Max.X)
			{
				return Vector3I.UnitX;
			}
			if (smallBlockWorldAabbReduced.Max.X < largeBlockWorldAabb.Min.X && smallBlockWorldAabb.Max.X >= largeBlockWorldAabb.Min.X)
			{
				return -Vector3I.UnitX;
			}
			if (smallBlockWorldAabbReduced.Min.Y > largeBlockWorldAabb.Max.Y && smallBlockWorldAabb.Min.Y <= largeBlockWorldAabb.Max.Y)
			{
				return Vector3I.UnitY;
			}
			if (smallBlockWorldAabbReduced.Max.Y < largeBlockWorldAabb.Min.Y && smallBlockWorldAabb.Max.Y >= largeBlockWorldAabb.Min.Y)
			{
				return -Vector3I.UnitY;
			}
			if (smallBlockWorldAabbReduced.Min.Z > largeBlockWorldAabb.Max.Z && smallBlockWorldAabb.Min.Z <= largeBlockWorldAabb.Max.Z)
			{
				return Vector3I.UnitZ;
			}
			return -Vector3I.UnitZ;
		}

		/// <summary>
		/// Returns true if the given small block connects to large one. One of the given AABB's is inflated with 0.05 to reduce inaccuracies.
		/// </summary>
		/// <param name="smallBlock">small block</param>
		/// <param name="smallBlockWorldAabb">small block world AABB</param>
		/// <param name="largeBlock">large block</param>
		/// <param name="largeBlockWorldAabb">large block wotld AABB</param>
		/// <returns>true when connected</returns>
		private bool SmallBlockConnectsToLarge(MySlimBlock smallBlock, ref BoundingBoxD smallBlockWorldAabb, MySlimBlock largeBlock, ref BoundingBoxD largeBlockWorldAabb)
		{
			BoundingBoxD smallBlockWorldAabbReduced = smallBlockWorldAabb;
			smallBlockWorldAabbReduced.Inflate((0f - smallBlock.CubeGrid.GridSize) / 4f);
			MatrixD matrix;
			if (!largeBlockWorldAabb.Intersects(smallBlockWorldAabbReduced))
			{
				Vector3I addNormal = GetSmallBlockAddDirection(ref smallBlockWorldAabb, ref smallBlockWorldAabbReduced, ref largeBlockWorldAabb);
				smallBlock.Orientation.GetQuaternion(out var result);
				matrix = smallBlock.CubeGrid.WorldMatrix;
				result = Quaternion.CreateFromRotationMatrix(in matrix) * result;
				if (!MyCubeGrid.CheckConnectivitySmallBlockToLargeGrid(largeBlock.CubeGrid, smallBlock.BlockDefinition, ref result, ref addNormal))
				{
					return false;
				}
			}
			BoundingBoxD boundingBoxD = smallBlockWorldAabb;
			boundingBoxD.Inflate(2f * smallBlock.CubeGrid.GridSize / 3f);
			BoundingBoxD boundingBoxD2 = boundingBoxD.Intersect(largeBlockWorldAabb);
			Vector3D translation = boundingBoxD2.Center;
			HkShape shape = new HkBoxShape(boundingBoxD2.HalfExtents);
			largeBlock.Orientation.GetQuaternion(out var result2);
			matrix = largeBlock.CubeGrid.WorldMatrix;
			result2 = Quaternion.CreateFromRotationMatrix(in matrix) * result2;
			largeBlock.ComputeWorldCenter(out var worldCenter);
			bool flag = false;
			try
			{
				if (largeBlock.FatBlock != null)
				{
					MyModel model = largeBlock.FatBlock.Model;
					if (model != null && model.HavokCollisionShapes != null)
					{
						HkShape[] havokCollisionShapes = model.HavokCollisionShapes;
						int num = 0;
						while (true)
						{
							if (num < havokCollisionShapes.Length)
							{
								flag = MyPhysics.IsPenetratingShapeShape(shape, ref translation, ref Quaternion.Identity, havokCollisionShapes[num], ref worldCenter, ref result2);
								if (flag)
								{
									break;
								}
								num++;
								continue;
							}
							return flag;
						}
						return flag;
					}
					HkShape shape2 = new HkBoxShape(largeBlock.BlockDefinition.Size * largeBlock.CubeGrid.GridSize / 2f);
					flag = MyPhysics.IsPenetratingShapeShape(shape, ref translation, ref Quaternion.Identity, shape2, ref worldCenter, ref result2);
					shape2.RemoveReference();
					return flag;
				}
				HkShape shape3 = new HkBoxShape(largeBlock.BlockDefinition.Size * largeBlock.CubeGrid.GridSize / 2f);
				flag = MyPhysics.IsPenetratingShapeShape(shape, ref translation, ref Quaternion.Identity, shape3, ref worldCenter, ref result2);
				shape3.RemoveReference();
				return flag;
			}
			finally
			{
				shape.RemoveReference();
			}
		}

		/// <summary>
		/// Remove all large block connections to small blocks (large block has been removed or its grid has been changed to dynamic).
		/// </summary>
		private void RemoveChangedLargeBlockConnectionToSmallBlocks(MySlimBlock block, HashSet<MyCubeGrid> outSmallGrids)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			MyGroups<MySlimBlock, MyBlockGroupData>.Group group = MyCubeGridGroups.Static.SmallToLargeBlockConnections.GetGroup(block);
			if (group == null)
			{
				return;
			}
			m_tmpSlimBlocks.Clear();
<<<<<<< HEAD
			foreach (MyGroups<MySlimBlock, MyBlockGroupData>.Node node in group.Nodes)
			{
				if (node.NodeData != block)
				{
					continue;
				}
				foreach (MyGroups<MySlimBlock, MyBlockGroupData>.Node child in node.Children)
				{
					m_tmpSlimBlocks.Add(child.NodeData);
				}
				break;
			}
			foreach (MySlimBlock tmpSlimBlock in m_tmpSlimBlocks)
			{
				DisconnectSmallToLargeBlock(tmpSlimBlock, block);
				outSmallGrids.Add(tmpSlimBlock.CubeGrid);
			}
			m_tmpSlimBlocks.Clear();
			m_tmpGridList.Clear();
			foreach (MyCubeGrid outSmallGrid in outSmallGrids)
			{
				if (m_mapSmallGridToConnectedBlocks.TryGetValue(outSmallGrid, out var _))
				{
					m_tmpGridList.Add(outSmallGrid);
				}
			}
=======
			Enumerator<MyGroups<MySlimBlock, MyBlockGroupData>.Node> enumerator = group.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<MySlimBlock, MyBlockGroupData>.Node current = enumerator.get_Current();
					if (current.NodeData != block)
					{
						continue;
					}
					Enumerator<long, MyGroups<MySlimBlock, MyBlockGroupData>.Node> enumerator2 = current.Children.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							MyGroups<MySlimBlock, MyBlockGroupData>.Node current2 = enumerator2.get_Current();
							m_tmpSlimBlocks.Add(current2.NodeData);
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					break;
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			Enumerator<MySlimBlock> enumerator3 = m_tmpSlimBlocks.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					MySlimBlock current3 = enumerator3.get_Current();
					DisconnectSmallToLargeBlock(current3, block);
					outSmallGrids.Add(current3.CubeGrid);
				}
			}
			finally
			{
				((IDisposable)enumerator3).Dispose();
			}
			m_tmpSlimBlocks.Clear();
			m_tmpGridList.Clear();
			Enumerator<MyCubeGrid> enumerator4 = outSmallGrids.GetEnumerator();
			try
			{
				while (enumerator4.MoveNext())
				{
					MyCubeGrid current4 = enumerator4.get_Current();
					if (m_mapSmallGridToConnectedBlocks.TryGetValue(current4, out var _))
					{
						m_tmpGridList.Add(current4);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator4).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (MyCubeGrid tmpGrid in m_tmpGridList)
			{
				outSmallGrids.Remove(tmpGrid);
			}
			m_tmpGridList.Clear();
		}

		/// <summary>
		/// Returns true if the given small grid connects to a large static grid, otherwise false.
		/// </summary>
		private bool SmallGridIsStatic(MyCubeGrid smallGrid)
		{
			if (TestGridSmallToLargeConnection(smallGrid))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Grid will be split. Called before split. All connections with the grid will be removed.
		/// </summary>
		internal void BeforeGridSplit_SmallToLargeGridConnectivity(MyCubeGrid originalGrid)
		{
			m_smallToLargeCheckEnabled = false;
		}

		/// <summary>
		/// Grid has been split. All connections will be recreated for original grid and also for all splits.
		/// </summary>
		internal void AfterGridSplit_SmallToLargeGridConnectivity(MyCubeGrid originalGrid, List<MyCubeGrid> gridSplits)
		{
			m_smallToLargeCheckEnabled = true;
			if (originalGrid.GridSizeEnum == MyCubeSize.Small)
			{
				AfterGridSplit_Small(originalGrid, gridSplits);
			}
			else
			{
				AfterGridSplit_Large(originalGrid, gridSplits);
			}
		}

		private void AfterGridSplit_Small(MyCubeGrid originalGrid, List<MyCubeGrid> gridSplits)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			if (!originalGrid.IsStatic)
			{
				return;
			}
			if (m_mapSmallGridToConnectedBlocks.TryGetValue(originalGrid, out var value))
			{
				m_tmpBlockConnections.Clear();
				Enumerator<MySlimBlockPair> enumerator = value.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlockPair current = enumerator.get_Current();
						if (current.Child.CubeGrid != originalGrid)
						{
							m_tmpBlockConnections.Add(current);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				foreach (MySlimBlockPair tmpBlockConnection in m_tmpBlockConnections)
				{
					DisconnectSmallToLargeBlock(tmpBlockConnection.Child, originalGrid, tmpBlockConnection.Parent, tmpBlockConnection.Parent.CubeGrid);
					ConnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
				}
				m_tmpBlockConnections.Clear();
			}
			if (!Sync.IsServer)
			{
				return;
			}
<<<<<<< HEAD
			if (!m_mapSmallGridToConnectedBlocks.TryGetValue(originalGrid, out value) || value.Count == 0)
=======
			if (!m_mapSmallGridToConnectedBlocks.TryGetValue(originalGrid, out value) || value.get_Count() == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				originalGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
			}
			foreach (MyCubeGrid gridSplit in gridSplits)
			{
<<<<<<< HEAD
				if (!m_mapSmallGridToConnectedBlocks.TryGetValue(gridSplit, out value) || value.Count == 0)
=======
				if (!m_mapSmallGridToConnectedBlocks.TryGetValue(gridSplit, out value) || value.get_Count() == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					gridSplit.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
				}
			}
		}

		private void AfterGridSplit_Large(MyCubeGrid originalGrid, List<MyCubeGrid> gridSplits)
		{
<<<<<<< HEAD
=======
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!originalGrid.IsStatic || !m_mapLargeGridToConnectedBlocks.TryGetValue(originalGrid, out var value))
			{
				return;
			}
			m_tmpBlockConnections.Clear();
<<<<<<< HEAD
			foreach (MySlimBlockPair item in value)
			{
				if (item.Parent.CubeGrid != originalGrid)
				{
					m_tmpBlockConnections.Add(item);
				}
			}
=======
			Enumerator<MySlimBlockPair> enumerator = value.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlockPair current = enumerator.get_Current();
					if (current.Parent.CubeGrid != originalGrid)
					{
						m_tmpBlockConnections.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (MySlimBlockPair tmpBlockConnection in m_tmpBlockConnections)
			{
				DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Child.CubeGrid, tmpBlockConnection.Parent, originalGrid);
				ConnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
			}
			m_tmpBlockConnections.Clear();
		}

		/// <summary>
		/// Grids will be merged. Called before merge. All grids connections will be removed.
		/// </summary>
		internal void BeforeGridMerge_SmallToLargeGridConnectivity(MyCubeGrid originalGrid, MyCubeGrid mergedGrid)
		{
			m_tmpGrids.Clear();
			if (originalGrid.IsStatic && mergedGrid.IsStatic)
			{
				m_tmpGrids.Add(mergedGrid);
			}
			m_smallToLargeCheckEnabled = false;
		}

		/// <summary>
		/// Grid has been merged. All connections will be recreated.
		/// </summary>
		internal void AfterGridMerge_SmallToLargeGridConnectivity(MyCubeGrid originalGrid)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			m_smallToLargeCheckEnabled = true;
<<<<<<< HEAD
			if (m_tmpGrids.Count == 0 || !originalGrid.IsStatic)
=======
			if (m_tmpGrids.get_Count() == 0 || !originalGrid.IsStatic)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			if (originalGrid.GridSizeEnum == MyCubeSize.Large)
			{
<<<<<<< HEAD
				foreach (MyCubeGrid tmpGrid in m_tmpGrids)
				{
					if (!m_mapLargeGridToConnectedBlocks.TryGetValue(tmpGrid, out var value))
					{
						continue;
					}
					m_tmpBlockConnections.Clear();
					m_tmpBlockConnections.AddRange(value);
					foreach (MySlimBlockPair tmpBlockConnection in m_tmpBlockConnections)
					{
						DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Child.CubeGrid, tmpBlockConnection.Parent, tmpGrid);
						ConnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
					}
				}
			}
			else
			{
				foreach (MyCubeGrid tmpGrid2 in m_tmpGrids)
				{
					if (!m_mapSmallGridToConnectedBlocks.TryGetValue(tmpGrid2, out var value2))
					{
						continue;
					}
					m_tmpBlockConnections.Clear();
					m_tmpBlockConnections.AddRange(value2);
					foreach (MySlimBlockPair tmpBlockConnection2 in m_tmpBlockConnections)
					{
						DisconnectSmallToLargeBlock(tmpBlockConnection2.Child, tmpGrid2, tmpBlockConnection2.Parent, tmpBlockConnection2.Parent.CubeGrid);
						ConnectSmallToLargeBlock(tmpBlockConnection2.Child, tmpBlockConnection2.Parent);
					}
				}
=======
				Enumerator<MyCubeGrid> enumerator = m_tmpGrids.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyCubeGrid current = enumerator.get_Current();
						if (!m_mapLargeGridToConnectedBlocks.TryGetValue(current, out var value))
						{
							continue;
						}
						m_tmpBlockConnections.Clear();
						m_tmpBlockConnections.AddRange((IEnumerable<MySlimBlockPair>)value);
						foreach (MySlimBlockPair tmpBlockConnection in m_tmpBlockConnections)
						{
							DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Child.CubeGrid, tmpBlockConnection.Parent, current);
							ConnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			else
			{
				Enumerator<MyCubeGrid> enumerator = m_tmpGrids.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyCubeGrid current3 = enumerator.get_Current();
						if (!m_mapSmallGridToConnectedBlocks.TryGetValue(current3, out var value2))
						{
							continue;
						}
						m_tmpBlockConnections.Clear();
						m_tmpBlockConnections.AddRange((IEnumerable<MySlimBlockPair>)value2);
						foreach (MySlimBlockPair tmpBlockConnection2 in m_tmpBlockConnections)
						{
							DisconnectSmallToLargeBlock(tmpBlockConnection2.Child, current3, tmpBlockConnection2.Parent, tmpBlockConnection2.Parent.CubeGrid);
							ConnectSmallToLargeBlock(tmpBlockConnection2.Child, tmpBlockConnection2.Parent);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_tmpGrids.Clear();
			m_tmpBlockConnections.Clear();
		}

		private void CubeGrid_OnClosing(MyEntity entity)
		{
			MyCubeGrid myCubeGrid = (MyCubeGrid)entity;
			if (myCubeGrid.GridSizeEnum == MyCubeSize.Small)
			{
				RemoveSmallGridConnections(myCubeGrid);
			}
			else
			{
				RemoveLargeGridConnections(myCubeGrid);
			}
		}

		private static MyCubeSize GetCubeSize(MySlimBlock block)
		{
			if (block.CubeGrid != null)
			{
				return block.CubeGrid.GridSizeEnum;
			}
			MyFracturedBlock myFracturedBlock = block.FatBlock as MyFracturedBlock;
			if (myFracturedBlock != null && myFracturedBlock.OriginalBlocks.Count > 0 && MyDefinitionManager.Static.TryGetCubeBlockDefinition(myFracturedBlock.OriginalBlocks[0], out var blockDefinition))
			{
				return blockDefinition.CubeSize;
			}
			return block.BlockDefinition.CubeSize;
		}
	}
}
