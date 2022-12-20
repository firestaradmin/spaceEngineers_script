using System;
using System.Collections.Generic;
using System.Diagnostics;
using ParallelTasks;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;
using VRage.Algorithms;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Groups;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
<<<<<<< HEAD
	public class MyGridConveyorSystem : MyUpdateableGridSystem, IMyGridConveyorSystem
=======
	public class MyGridConveyorSystem : MyUpdateableGridSystem
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	{
		/// <summary>
		/// Wrapper for various sets of inventory items for the pull requests
		/// </summary>         
		private class PullRequestItemSet
		{
			private bool m_all;

			private MyObjectBuilderType? m_obType;

			private MyInventoryConstraint m_constraint;

			public void Clear()
			{
				m_all = false;
				m_obType = null;
				m_constraint = null;
			}

			public void Set(bool all)
			{
				Clear();
				m_all = all;
			}

			public void Set(MyObjectBuilderType? itemTypeId)
			{
				Clear();
				m_obType = itemTypeId;
			}

			public void Set(MyInventoryConstraint inventoryConstraint)
			{
				Clear();
				m_constraint = inventoryConstraint;
			}

			public bool Contains(MyDefinitionId itemId)
			{
				if (m_all)
				{
					return true;
				}
				if (m_obType.HasValue && m_obType.Value == itemId.TypeId)
				{
					return true;
				}
				if (m_constraint != null && m_constraint.Check(itemId))
				{
					return true;
				}
				return false;
			}
		}

		private class TransferData : WorkData
		{
			public IMyConveyorEndpointBlock m_start;

			public IMyConveyorEndpointBlock m_endPoint;

			public MyDefinitionId m_itemId;

			public bool m_isPush;

			public bool m_canTransfer;

			public TransferData(IMyConveyorEndpointBlock start, IMyConveyorEndpointBlock endPoint, MyDefinitionId itemId, bool isPush)
			{
				m_start = start;
				m_endPoint = endPoint;
				m_itemId = itemId;
				m_isPush = isPush;
			}

			public void ComputeTransfer()
			{
				IMyConveyorEndpointBlock lhs = m_start;
				IMyConveyorEndpointBlock rhs = m_endPoint;
				if (!m_isPush)
				{
					MyUtils.Swap(ref lhs, ref rhs);
				}
				m_canTransfer = ComputeCanTransfer(lhs, rhs, m_itemId);
			}

			public void StoreTransferState()
			{
				(m_start as MyCubeBlock).CubeGrid.GridSystems.ConveyorSystem.GetConveyorEndpointMapping(m_start).AddTransfer(m_endPoint, m_itemId, m_isPush, m_canTransfer);
			}
		}

		private class ConveyorEndpointMapping
		{
			public List<IMyConveyorEndpointBlock> pullElements;

			public List<IMyConveyorEndpointBlock> pushElements;

			private readonly Dictionary<(IMyConveyorEndpointBlock, MyDefinitionId, bool), bool> testedTransfers = new Dictionary<(IMyConveyorEndpointBlock, MyDefinitionId, bool), bool>();

			public void AddTransfer(IMyConveyorEndpointBlock block, MyDefinitionId itemId, bool isPush, bool canTransfer)
			{
				testedTransfers[(block, itemId, isPush)] = canTransfer;
			}

			public bool TryGetTransfer(IMyConveyorEndpointBlock block, MyDefinitionId itemId, bool isPush, out bool canTransfer)
			{
				return testedTransfers.TryGetValue((block, itemId, isPush), out canTransfer);
			}
		}

		private const uint MAX_ITEMS_TO_PUSH_IN_ONE_REQUEST = 10u;

		private static readonly float CONVEYOR_SYSTEM_CONSUMPTION = 1E-07f;

		private static readonly int MAX_COMPUTATION_TASKS_FOR_ALL_REQUESTS = 10;

		private readonly HashSet<MyCubeBlock> m_inventoryBlocks = new HashSet<MyCubeBlock>();

		private readonly HashSet<IMyConveyorEndpointBlock> m_conveyorEndpointBlocks = new HashSet<IMyConveyorEndpointBlock>();

		private readonly HashSet<MyConveyorLine> m_lines = new HashSet<MyConveyorLine>();

		private readonly HashSet<MyShipConnector> m_connectors = new HashSet<MyShipConnector>();

		private bool m_needsRecomputation = true;

		private HashSet<MyCubeGrid> m_tmpConnectedGrids = new HashSet<MyCubeGrid>();

		[ThreadStatic]
		private static List<MyPhysicalInventoryItem> m_tmpInventoryItems;

		[ThreadStatic]
		private static PullRequestItemSet m_tmpRequestedItemSetPerThread;

		[ThreadStatic]
		private static MyPathFindingSystem<IMyConveyorEndpoint> m_pathfinding;

		private static Dictionary<(IMyConveyorEndpointBlock, IMyConveyorEndpointBlock), Task> m_currentTransferComputationTasks = new Dictionary<(IMyConveyorEndpointBlock, IMyConveyorEndpointBlock), Task>();

		private Dictionary<ConveyorLinePosition, MyConveyorLine> m_lineEndpoints;

		private Dictionary<Vector3I, MyConveyorLine> m_linePoints;

		private HashSet<MyConveyorLine> m_deserializedLines;

		public bool IsClosing;

		public string HudMessageCustom = string.Empty;

		[ThreadStatic]
		private static long m_playerIdForAccessiblePredicate;

		[ThreadStatic]
		private static MyDefinitionId m_inventoryItemDefinitionId;

		private static Predicate<IMyConveyorEndpoint> IsAccessAllowedPredicate = IsAccessAllowed;

		private static Predicate<IMyPathEdge<IMyConveyorEndpoint>> IsConveyorLargePredicate = IsConveyorLarge;

		private static Predicate<IMyPathEdge<IMyConveyorEndpoint>> IsConveyorSmallPredicate = IsConveyorSmall;

		[ThreadStatic]
		private static List<IMyConveyorEndpoint> m_reachableBuffer;

		private Dictionary<IMyConveyorEndpointBlock, ConveyorEndpointMapping> m_conveyorConnections = new Dictionary<IMyConveyorEndpointBlock, ConveyorEndpointMapping>();

		private bool m_isRecomputingGraph;

		private bool m_isRecomputationInterrupted;

		private bool m_isRecomputationIsAborted;

		private const double MAX_RECOMPUTE_DURATION_MILLISECONDS = 10.0;

		private Dictionary<IMyConveyorEndpointBlock, ConveyorEndpointMapping> m_conveyorConnectionsForThread = new Dictionary<IMyConveyorEndpointBlock, ConveyorEndpointMapping>();

		private IEnumerator<IMyConveyorEndpointBlock> m_endpointIterator;

		private FastResourceLock m_iteratorLock = new FastResourceLock();

		public bool NeedsUpdateLines;

		private static PullRequestItemSet m_tmpRequestedItemSet
		{
			get
			{
				if (m_tmpRequestedItemSetPerThread == null)
				{
					m_tmpRequestedItemSetPerThread = new PullRequestItemSet();
				}
				return m_tmpRequestedItemSetPerThread;
			}
		}

		private static MyPathFindingSystem<IMyConveyorEndpoint> Pathfinding
		{
			get
			{
				if (m_pathfinding == null)
				{
					m_pathfinding = new MyPathFindingSystem<IMyConveyorEndpoint>();
				}
				return m_pathfinding;
			}
		}

		public MyResourceSinkComponent ResourceSink { get; private set; }

		public bool IsInteractionPossible
		{
			get
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				bool flag = false;
				Enumerator<MyShipConnector> enumerator = m_connectors.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyShipConnector current = enumerator.get_Current();
						flag |= current.InConstraint;
					}
					return flag;
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
		}

		public bool Connected
		{
			get
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				bool flag = false;
				Enumerator<MyShipConnector> enumerator = m_connectors.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyShipConnector current = enumerator.get_Current();
						flag |= current.Connected;
					}
					return flag;
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
		}

		public bool IsParked
		{
			get
			{
				bool flag = false;
				foreach (MyShipConnector connector in m_connectors)
				{
					flag |= connector.Connected && connector.IsParkingEnabled;
				}
				return flag;
			}
		}

		public HashSetReader<IMyConveyorEndpointBlock> ConveyorEndpointBlocks => new HashSetReader<IMyConveyorEndpointBlock>(m_conveyorEndpointBlocks);

		public HashSetReader<MyCubeBlock> InventoryBlocks => new HashSetReader<MyCubeBlock>(m_inventoryBlocks);

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.OnceAfterSimulation;

		public event Action<MyCubeBlock> BlockAdded;

		public event Action<MyCubeBlock> BlockRemoved;

		public event Action<IMyConveyorEndpointBlock> OnBeforeRemoveEndpointBlock;

		public event Action<IMyConveyorSegmentBlock> OnBeforeRemoveSegmentBlock;

		public MyGridConveyorSystem(MyCubeGrid grid)
			: base(grid)
		{
			m_lineEndpoints = null;
			m_linePoints = null;
			m_deserializedLines = null;
			ResourceSink = new MyResourceSinkComponent();
<<<<<<< HEAD
			ResourceSink.Init(MyStringHash.GetOrCompute("Conveyors"), CONVEYOR_SYSTEM_CONSUMPTION, () => CONVEYOR_SYSTEM_CONSUMPTION, null);
			ResourceSink.Grid = base.Grid;
=======
			ResourceSink.Init(MyStringHash.GetOrCompute("Conveyors"), CONVEYOR_SYSTEM_CONSUMPTION, () => CONVEYOR_SYSTEM_CONSUMPTION);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			ResourceSink.Update();
		}

		public void BeforeBlockDeserialization(List<MyObjectBuilder_ConveyorLine> lines)
		{
			if (lines == null)
<<<<<<< HEAD
			{
				return;
			}
			m_lineEndpoints = new Dictionary<ConveyorLinePosition, MyConveyorLine>(lines.Count * 2);
			m_linePoints = new Dictionary<Vector3I, MyConveyorLine>(lines.Count * 4);
			m_deserializedLines = new HashSet<MyConveyorLine>();
			foreach (MyObjectBuilder_ConveyorLine line in lines)
			{
=======
			{
				return;
			}
			m_lineEndpoints = new Dictionary<ConveyorLinePosition, MyConveyorLine>(lines.Count * 2);
			m_linePoints = new Dictionary<Vector3I, MyConveyorLine>(lines.Count * 4);
			m_deserializedLines = new HashSet<MyConveyorLine>();
			foreach (MyObjectBuilder_ConveyorLine line in lines)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyConveyorLine myConveyorLine = new MyConveyorLine();
				myConveyorLine.Init(line, base.Grid);
				if (!myConveyorLine.CheckSectionConsistency())
				{
					continue;
				}
				ConveyorLinePosition key = new ConveyorLinePosition(line.StartPosition, line.StartDirection);
				ConveyorLinePosition key2 = new ConveyorLinePosition(line.EndPosition, line.EndDirection);
				try
				{
					m_lineEndpoints.Add(key, myConveyorLine);
					m_lineEndpoints.Add(key2, myConveyorLine);
					foreach (Vector3I item in myConveyorLine)
					{
						m_linePoints.Add(item, myConveyorLine);
					}
					m_deserializedLines.Add(myConveyorLine);
					m_lines.Add(myConveyorLine);
				}
				catch (ArgumentException)
				{
					m_lineEndpoints = null;
					m_deserializedLines = null;
					m_linePoints = null;
					m_lines.Clear();
					return;
				}
			}
		}

		public MyConveyorLine GetDeserializingLine(ConveyorLinePosition position)
		{
			if (m_lineEndpoints == null)
			{
				return null;
			}
			m_lineEndpoints.TryGetValue(position, out var value);
			return value;
		}

		public MyConveyorLine GetDeserializingLine(Vector3I position)
		{
			if (m_linePoints == null)
			{
				return null;
			}
			m_linePoints.TryGetValue(position, out var value);
			return value;
		}

		public void AfterBlockDeserialization()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			m_lineEndpoints = null;
			m_linePoints = null;
			m_deserializedLines = null;
			Enumerator<MyConveyorLine> enumerator = m_lines.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().UpdateIsFunctional();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void SerializeLines(List<MyObjectBuilder_ConveyorLine> resultList)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyConveyorLine> enumerator = m_lines.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyConveyorLine current = enumerator.get_Current();
					if (!current.IsEmpty || !current.IsDisconnected || current.Length != 1)
					{
						resultList.Add(current.GetObjectBuilder());
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void AfterGridClose()
		{
			m_lines.Clear();
			m_conveyorConnections.Clear();
		}

		public void Add(MyCubeBlock block)
		{
			m_inventoryBlocks.Add(block);
			this.BlockAdded?.Invoke(block);
		}

		public void Remove(MyCubeBlock block)
		{
			m_inventoryBlocks.Remove(block);
			this.BlockRemoved?.Invoke(block);
		}

		internal void GetGridInventories(MyEntity interactedAsEntity, List<MyEntity> outputInventories, long identityId)
		{
			GetGridInventories(interactedAsEntity, outputInventories, identityId, null);
		}

		internal void GetGridInventories(MyEntity interactedAsEntity, List<MyEntity> outputInventories, long identityId, List<long> inventoryIds = null)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyCubeBlock> enumerator = m_inventoryBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyCubeBlock current = enumerator.get_Current();
					if ((!(current is MyTerminalBlock) || (current as MyTerminalBlock).HasPlayerAccess(identityId)) && (interactedAsEntity == current || !(current is MyTerminalBlock) || (current as MyTerminalBlock).ShowInInventory))
					{
						outputInventories?.Add(current);
						inventoryIds?.Add(current.EntityId);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void AddConveyorBlock(IMyConveyorEndpointBlock endpointBlock)
		{
			using (m_iteratorLock.AcquireExclusiveUsing())
			{
				m_endpointIterator = null;
				m_conveyorEndpointBlocks.Add(endpointBlock);
				if (endpointBlock is MyShipConnector)
				{
					m_connectors.Add(endpointBlock as MyShipConnector);
				}
				IMyConveyorEndpoint conveyorEndpoint = endpointBlock.ConveyorEndpoint;
				for (int i = 0; i < conveyorEndpoint.GetLineCount(); i++)
				{
					ConveyorLinePosition position = conveyorEndpoint.GetPosition(i);
					MyConveyorLine conveyorLine = conveyorEndpoint.GetConveyorLine(i);
					if (m_deserializedLines != null && m_deserializedLines.Contains(conveyorLine))
<<<<<<< HEAD
					{
						continue;
					}
					MySlimBlock cubeBlock = base.Grid.GetCubeBlock(position.NeighbourGridPosition);
					if (cubeBlock == null)
					{
						m_lines.Add(conveyorLine);
						continue;
					}
					IMyConveyorEndpointBlock myConveyorEndpointBlock = cubeBlock.FatBlock as IMyConveyorEndpointBlock;
					IMyConveyorSegmentBlock myConveyorSegmentBlock = cubeBlock.FatBlock as IMyConveyorSegmentBlock;
					if (myConveyorSegmentBlock != null)
					{
=======
					{
						continue;
					}
					MySlimBlock cubeBlock = base.Grid.GetCubeBlock(position.NeighbourGridPosition);
					if (cubeBlock == null)
					{
						m_lines.Add(conveyorLine);
						continue;
					}
					IMyConveyorEndpointBlock myConveyorEndpointBlock = cubeBlock.FatBlock as IMyConveyorEndpointBlock;
					IMyConveyorSegmentBlock myConveyorSegmentBlock = cubeBlock.FatBlock as IMyConveyorSegmentBlock;
					if (myConveyorSegmentBlock != null)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (!TryMergeEndpointSegment(endpointBlock, myConveyorSegmentBlock, position))
						{
							m_lines.Add(conveyorLine);
						}
					}
					else if (myConveyorEndpointBlock != null)
					{
						if (!TryMergeEndpointEndpoint(endpointBlock, myConveyorEndpointBlock, position, position.GetConnectingPosition()))
						{
							m_lines.Add(conveyorLine);
						}
					}
					else
					{
						m_lines.Add(conveyorLine);
					}
				}
			}
		}

		public void DebugDraw(MyCubeGrid grid)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyConveyorLine> enumerator = m_lines.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().DebugDraw(grid);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(1f, 1f), "Conveyor lines: " + m_lines.get_Count(), Color.Red, 1f);
		}

		public void DebugDrawLinePackets()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyConveyorLine> enumerator = m_lines.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().DebugDrawPackets();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void UpdateBeforeSimulation()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			MySimpleProfiler.Begin("Conveyor", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateBeforeSimulation");
			Enumerator<MyConveyorLine> enumerator = m_lines.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyConveyorLine current = enumerator.get_Current();
					if (!current.IsEmpty)
					{
						current.Update();
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			MySimpleProfiler.End("UpdateBeforeSimulation");
		}

		public void UpdateBeforeSimulation10()
		{
			MySimpleProfiler.Begin("Conveyor", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateBeforeSimulation10");
			ResourceSink.Update();
			MySimpleProfiler.End("UpdateBeforeSimulation10");
		}

		public void FlagForRecomputation()
		{
<<<<<<< HEAD
=======
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Group group = MyGridPhysicalHierarchy.Static.GetGroup(base.Grid);
			if (group == null)
			{
				return;
			}
<<<<<<< HEAD
			foreach (MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node in group.Nodes)
			{
				node.NodeData.GridSystems.ConveyorSystem.m_needsRecomputation = true;
=======
			Enumerator<MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node> enumerator = group.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().NodeData.GridSystems.ConveyorSystem.m_needsRecomputation = true;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		protected override void Update()
		{
			UpdateLinesLazy();
		}

		public void UpdateAfterSimulation100()
		{
			if (m_needsRecomputation)
			{
				MySimpleProfiler.Begin("Conveyor", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateAfterSimulation100");
				RecomputeConveyorEndpoints();
				m_needsRecomputation = false;
				MySimpleProfiler.End("UpdateAfterSimulation100");
			}
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateLines();
		}

		public void UpdateLines()
		{
			Schedule();
			NeedsUpdateLines = true;
		}

		public void UpdateLinesLazy()
		{
			if (NeedsUpdateLines)
			{
				NeedsUpdateLines = false;
				FlagForRecomputation();
			}
		}

		public void RemoveConveyorBlock(IMyConveyorEndpointBlock block)
		{
			using (m_iteratorLock.AcquireExclusiveUsing())
			{
				m_endpointIterator = null;
				m_conveyorEndpointBlocks.Remove(block);
				if (block is MyShipConnector)
				{
					m_connectors.Remove(block as MyShipConnector);
				}
				if (IsClosing)
				{
					return;
				}
				if (this.OnBeforeRemoveEndpointBlock != null)
				{
					this.OnBeforeRemoveEndpointBlock(block);
				}
				for (int i = 0; i < block.ConveyorEndpoint.GetLineCount(); i++)
				{
					MyConveyorLine conveyorLine = block.ConveyorEndpoint.GetConveyorLine(i);
					conveyorLine.DisconnectEndpoint(block.ConveyorEndpoint);
					if (conveyorLine.IsDegenerate)
					{
						m_lines.Remove(conveyorLine);
					}
				}
			}
		}

		public void AddSegmentBlock(IMyConveyorSegmentBlock segmentBlock)
		{
			AddSegmentBlockInternal(segmentBlock, segmentBlock.ConveyorSegment.ConnectingPosition1);
			AddSegmentBlockInternal(segmentBlock, segmentBlock.ConveyorSegment.ConnectingPosition2);
			if (!m_lines.Contains(segmentBlock.ConveyorSegment.ConveyorLine) && segmentBlock.ConveyorSegment.ConveyorLine != null)
			{
				m_lines.Add(segmentBlock.ConveyorSegment.ConveyorLine);
			}
		}

		public void RemoveSegmentBlock(IMyConveyorSegmentBlock segmentBlock)
		{
			if (!IsClosing)
			{
				if (this.OnBeforeRemoveSegmentBlock != null)
				{
					this.OnBeforeRemoveSegmentBlock(segmentBlock);
				}
				MyConveyorLine conveyorLine = segmentBlock.ConveyorSegment.ConveyorLine;
				MyConveyorLine myConveyorLine = segmentBlock.ConveyorSegment.ConveyorLine.RemovePortion(segmentBlock.ConveyorSegment.ConnectingPosition1.NeighbourGridPosition, segmentBlock.ConveyorSegment.ConnectingPosition2.NeighbourGridPosition);
				if (conveyorLine.IsDegenerate)
				{
					m_lines.Remove(conveyorLine);
				}
				if (myConveyorLine != null)
				{
					UpdateLineReferences(myConveyorLine, myConveyorLine);
					m_lines.Add(myConveyorLine);
				}
			}
		}

		private void AddSegmentBlockInternal(IMyConveyorSegmentBlock segmentBlock, ConveyorLinePosition connectingPosition)
		{
			MySlimBlock cubeBlock = base.Grid.GetCubeBlock(connectingPosition.LocalGridPosition);
			if (cubeBlock == null || (m_deserializedLines != null && m_deserializedLines.Contains(segmentBlock.ConveyorSegment.ConveyorLine)))
			{
				return;
			}
			IMyConveyorEndpointBlock myConveyorEndpointBlock = cubeBlock.FatBlock as IMyConveyorEndpointBlock;
			IMyConveyorSegmentBlock myConveyorSegmentBlock = cubeBlock.FatBlock as IMyConveyorSegmentBlock;
			if (myConveyorSegmentBlock != null)
			{
				MyConveyorLine conveyorLine = segmentBlock.ConveyorSegment.ConveyorLine;
				if (m_lines.Contains(conveyorLine))
				{
					m_lines.Remove(conveyorLine);
				}
				if (myConveyorSegmentBlock.ConveyorSegment.CanConnectTo(connectingPosition, segmentBlock.ConveyorSegment.ConveyorLine.Type))
				{
					MergeSegmentSegment(segmentBlock, myConveyorSegmentBlock);
				}
			}
			if (myConveyorEndpointBlock != null)
			{
				MyConveyorLine conveyorLine2 = myConveyorEndpointBlock.ConveyorEndpoint.GetConveyorLine(connectingPosition);
				if (TryMergeEndpointSegment(myConveyorEndpointBlock, segmentBlock, connectingPosition))
				{
					m_lines.Remove(conveyorLine2);
				}
			}
		}

		/// <summary>
		/// Tries to merge the conveyor lines of a conveyor block and segment block.
		/// Also changes the reference in the endpoint block to the correct line.
		/// </summary>
		private bool TryMergeEndpointSegment(IMyConveyorEndpointBlock endpoint, IMyConveyorSegmentBlock segmentBlock, ConveyorLinePosition endpointPosition)
		{
			MyConveyorLine conveyorLine = endpoint.ConveyorEndpoint.GetConveyorLine(endpointPosition);
			if (conveyorLine == null)
			{
				return false;
			}
			if (!segmentBlock.ConveyorSegment.CanConnectTo(endpointPosition.GetConnectingPosition(), conveyorLine.Type))
			{
				return false;
			}
			MyConveyorLine conveyorLine2 = segmentBlock.ConveyorSegment.ConveyorLine;
			conveyorLine2.Merge(conveyorLine, segmentBlock);
			endpoint.ConveyorEndpoint.SetConveyorLine(endpointPosition, conveyorLine2);
			conveyorLine.RecalculateConductivity();
			conveyorLine2.RecalculateConductivity();
			return true;
		}

		private bool TryMergeEndpointEndpoint(IMyConveyorEndpointBlock endpointBlock1, IMyConveyorEndpointBlock endpointBlock2, ConveyorLinePosition pos1, ConveyorLinePosition pos2)
		{
			MyConveyorLine conveyorLine = endpointBlock1.ConveyorEndpoint.GetConveyorLine(pos1);
			if (conveyorLine == null)
			{
				return false;
			}
			MyConveyorLine conveyorLine2 = endpointBlock2.ConveyorEndpoint.GetConveyorLine(pos2);
			if (conveyorLine2 == null)
			{
				return false;
			}
			if (conveyorLine.Type != conveyorLine2.Type)
			{
				return false;
			}
			if (conveyorLine.GetEndpoint(1) == null)
			{
				conveyorLine.Reverse();
			}
			if (conveyorLine2.GetEndpoint(0) == null)
			{
				conveyorLine2.Reverse();
			}
			conveyorLine2.Merge(conveyorLine);
			endpointBlock1.ConveyorEndpoint.SetConveyorLine(pos1, conveyorLine2);
			conveyorLine.RecalculateConductivity();
			conveyorLine2.RecalculateConductivity();
			return true;
		}

		private void MergeSegmentSegment(IMyConveyorSegmentBlock newSegmentBlock, IMyConveyorSegmentBlock oldSegmentBlock)
		{
			MyConveyorLine conveyorLine = newSegmentBlock.ConveyorSegment.ConveyorLine;
			MyConveyorLine conveyorLine2 = oldSegmentBlock.ConveyorSegment.ConveyorLine;
			if (conveyorLine != conveyorLine2)
			{
				conveyorLine2.Merge(conveyorLine, newSegmentBlock);
			}
			UpdateLineReferences(conveyorLine, conveyorLine2);
			newSegmentBlock.ConveyorSegment.SetConveyorLine(conveyorLine2);
		}

		private void UpdateLineReferences(MyConveyorLine oldLine, MyConveyorLine newLine)
		{
			for (int i = 0; i < 2; i++)
			{
				if (oldLine.GetEndpoint(i) != null)
				{
					oldLine.GetEndpoint(i).SetConveyorLine(oldLine.GetEndpointPosition(i), newLine);
				}
			}
			foreach (Vector3I item in oldLine)
			{
				MySlimBlock cubeBlock = base.Grid.GetCubeBlock(item);
				if (cubeBlock != null)
				{
					(cubeBlock.FatBlock as IMyConveyorSegmentBlock)?.ConveyorSegment.SetConveyorLine(newLine);
				}
			}
			oldLine.RecalculateConductivity();
			newLine.RecalculateConductivity();
		}

		public void ToggleConnectors(bool? targetState = null, bool forceToggle = true)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
<<<<<<< HEAD
			if (!targetState.HasValue)
			{
				foreach (MyShipConnector connector in m_connectors)
				{
					flag |= connector.Connected && connector.IsParkingEnabled;
				}
			}
			else
			{
				flag = !targetState.Value;
=======
			Enumerator<MyShipConnector> enumerator = m_connectors.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyShipConnector current = enumerator.get_Current();
					flag |= current.Connected;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
<<<<<<< HEAD
				if (connector2.GetPlayerRelationToOwner() == MyRelationsBetweenPlayerAndBlock.Enemies)
				{
					continue;
				}
				if (flag && connector2.Connected && (forceToggle || connector2.IsParkingEnabled))
				{
					connector2.TryDisconnect();
				}
				if (flag)
				{
					continue;
				}
				if (connector2.IsProtectedFromLockingByTrading() || (connector2.InConstraint && connector2.Other.IsProtectedFromLockingByTrading()))
				{
					HudMessageCustom = string.Format(MyTexts.GetString(MySpaceTexts.Connector_TemporaryBlock), connector2.GetProtectionFromLockingTime());
					continue;
				}
				if (forceToggle || connector2.IsParkingEnabled)
				{
					connector2.TryConnect();
				}
				if (connector2.InConstraint && ((float)connector2.AutoUnlockTime > 0f || (float)connector2.Other.AutoUnlockTime > 0f))
				{
					float num = 0f;
					num = ((!((float)connector2.AutoUnlockTime > 0f)) ? ((float)connector2.Other.AutoUnlockTime) : ((!((float)connector2.Other.AutoUnlockTime > 0f)) ? ((float)connector2.AutoUnlockTime) : Math.Min(connector2.AutoUnlockTime, connector2.Other.AutoUnlockTime)));
					int num2 = (int)(num / 60f);
					int num3 = (int)(num - (float)(num2 * 60));
					HudMessageCustom = string.Format(MyTexts.GetString(MySpaceTexts.Connector_AutoUnlockWarning), num2, num3);
=======
				((IDisposable)enumerator).Dispose();
			}
			enumerator = m_connectors.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyShipConnector current2 = enumerator.get_Current();
					if (current2.GetPlayerRelationToOwner() == MyRelationsBetweenPlayerAndBlock.Enemies)
					{
						continue;
					}
					if (flag && current2.Connected)
					{
						current2.TryDisconnect();
						HudMessage = MySpaceTexts.NotificationConnectorsDisabled;
					}
					if (flag)
					{
						continue;
					}
					if (current2.IsProtectedFromLockingByTrading() || (current2.InConstraint && current2.Other.IsProtectedFromLockingByTrading()))
					{
						HudMessageCustom = string.Format(MyTexts.GetString(MySpaceTexts.Connector_TemporaryBlock), current2.GetProtectionFromLockingTime());
						continue;
					}
					current2.TryConnect();
					if (current2.InConstraint)
					{
						HudMessage = MySpaceTexts.NotificationConnectorsEnabled;
						if ((float)current2.AutoUnlockTime > 0f || (float)current2.Other.AutoUnlockTime > 0f)
						{
							float num = 0f;
							num = ((!((float)current2.AutoUnlockTime > 0f)) ? ((float)current2.Other.AutoUnlockTime) : ((!((float)current2.Other.AutoUnlockTime > 0f)) ? ((float)current2.AutoUnlockTime) : Math.Min(current2.AutoUnlockTime, current2.Other.AutoUnlockTime)));
							int num2 = (int)(num / 60f);
							int num3 = (int)(num - (float)(num2 * 60));
							HudMessageCustom = string.Format(MyTexts.GetString(MySpaceTexts.Connector_AutoUnlockWarning), num2, num3);
						}
					}
					else
					{
						HudMessage = MyStringId.NullOrEmpty;
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private static void SetTraversalPlayerId(long playerId)
		{
			m_playerIdForAccessiblePredicate = playerId;
		}

		private static void SetTraversalInventoryItemDefinitionId(MyDefinitionId item = default(MyDefinitionId))
		{
			m_inventoryItemDefinitionId = item;
		}

		private static bool IsAccessAllowed(IMyConveyorEndpoint endpoint)
		{
			if (endpoint.CubeBlock.GetUserRelationToOwner(m_playerIdForAccessiblePredicate) == MyRelationsBetweenPlayerAndBlock.Enemies)
			{
				return false;
			}
			MyConveyorSorter myConveyorSorter = endpoint.CubeBlock as MyConveyorSorter;
			if (myConveyorSorter != null && m_inventoryItemDefinitionId != default(MyDefinitionId))
			{
				return myConveyorSorter.IsAllowed(m_inventoryItemDefinitionId);
			}
			MyShipConnector myShipConnector = endpoint.CubeBlock as MyShipConnector;
			if (myShipConnector != null && (bool)myShipConnector.TradingEnabled)
			{
				return false;
			}
			return true;
		}

		private static bool IsConveyorLarge(IMyPathEdge<IMyConveyorEndpoint> conveyorLine)
		{
			if (conveyorLine is MyConveyorLine)
			{
				return (conveyorLine as MyConveyorLine).Type == MyObjectBuilder_ConveyorLine.LineType.LARGE_LINE;
			}
			return true;
		}

		private static bool IsConveyorSmall(IMyPathEdge<IMyConveyorEndpoint> conveyorLine)
		{
			if (conveyorLine is MyConveyorLine)
			{
				return (conveyorLine as MyConveyorLine).Type == MyObjectBuilder_ConveyorLine.LineType.SMALL_LINE;
			}
			return true;
		}

		private static bool NeedsLargeTube(MyDefinitionId itemDefinitionId)
		{
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(itemDefinitionId);
			if (physicalItemDefinition == null)
			{
				return true;
			}
			if (itemDefinitionId.TypeId == typeof(MyObjectBuilder_PhysicalGunObject))
			{
				return false;
			}
			return physicalItemDefinition.Size.AbsMax() > 0.25f;
		}

		public static void AppendReachableEndpoints(IMyConveyorEndpoint source, long playerId, List<IMyConveyorEndpoint> reachable, MyDefinitionId itemId, Predicate<IMyConveyorEndpoint> endpointFilter = null)
		{
			IMyConveyorEndpointBlock myConveyorEndpointBlock = source.CubeBlock as IMyConveyorEndpointBlock;
			if (myConveyorEndpointBlock != null)
			{
				lock (Pathfinding)
				{
					SetTraversalPlayerId(playerId);
					SetTraversalInventoryItemDefinitionId(itemId);
					Pathfinding.FindReachable(myConveyorEndpointBlock.ConveyorEndpoint, reachable, endpointFilter, IsAccessAllowedPredicate, NeedsLargeTube(itemId) ? IsConveyorLargePredicate : null);
				}
			}
		}

		public static bool Reachable(IMyConveyorEndpoint source, IMyConveyorEndpoint endPoint, long playerId, MyDefinitionId itemId, Predicate<IMyConveyorEndpoint> endpointFilter = null)
		{
			IMyConveyorEndpointBlock myConveyorEndpointBlock = source.CubeBlock as IMyConveyorEndpointBlock;
			if (myConveyorEndpointBlock == null)
			{
				return false;
			}
			lock (Pathfinding)
			{
				SetTraversalPlayerId(playerId);
				SetTraversalInventoryItemDefinitionId(itemId);
				return Pathfinding.Reachable(myConveyorEndpointBlock.ConveyorEndpoint, endPoint, endpointFilter, IsAccessAllowedPredicate, NeedsLargeTube(itemId) ? IsConveyorLargePredicate : null);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// ConvyeorSorters use this function to pull items
		/// </summary>
		/// <param name="start"></param>
		/// <param name="destinationInventory"></param>
		/// <param name="requestedTypeIds"></param>
		/// <param name="maxItemsToPull"></param>
		/// <returns>If you need try again shortly, as there were some uncalculated mapping</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static bool PullAllRequestForSorter(IMyConveyorEndpointBlock start, MyInventory destinationInventory, MyInventoryConstraint requestedTypeIds, int maxItemsToPull)
		{
			MyCubeBlock myCubeBlock = start as MyCubeBlock;
			if (myCubeBlock == null)
			{
				return false;
			}
			if (IsComputationTasksCountOverLimit())
			{
				return false;
			}
			m_tmpRequestedItemSet.Set(requestedTypeIds);
			MyGridConveyorSystem conveyorSystem = myCubeBlock.CubeGrid.GridSystems.ConveyorSystem;
			ConveyorEndpointMapping conveyorEndpointMapping = conveyorSystem.GetConveyorEndpointMapping(start);
			bool flag = false;
			bool flag2 = false;
			if (conveyorEndpointMapping.pullElements != null)
			{
				for (int i = 0; i < conveyorEndpointMapping.pullElements.Count; i++)
				{
					if (destinationInventory.VolumeFillFactor >= 0.99f)
					{
						break;
					}
					IMyConveyorEndpointBlock myConveyorEndpointBlock = conveyorEndpointMapping.pullElements[i];
					MyCubeBlock myCubeBlock2 = myConveyorEndpointBlock as MyCubeBlock;
					if (myCubeBlock2 == null || start == myConveyorEndpointBlock || HasComputationTask(start, myConveyorEndpointBlock))
					{
						continue;
					}
					int inventoryCount = myCubeBlock2.InventoryCount;
					for (int j = 0; j < inventoryCount; j++)
					{
						MyInventory inventory = myCubeBlock2.GetInventory(j);
						if ((inventory.GetFlags() & MyInventoryFlags.CanSend) == 0 || inventory == destinationInventory)
<<<<<<< HEAD
						{
							continue;
						}
						using (MyUtils.ReuseCollection(ref m_tmpInventoryItems))
						{
=======
						{
							continue;
						}
						using (MyUtils.ReuseCollection(ref m_tmpInventoryItems))
						{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							foreach (MyPhysicalInventoryItem item in inventory.GetItems())
							{
								MyDefinitionId id = item.Content.GetId();
								if (requestedTypeIds != null && !m_tmpRequestedItemSet.Contains(id))
								{
									continue;
								}
								if (!CanTransfer(start, myConveyorEndpointBlock, id, isPush: false))
								{
									if (HasComputationTask(start, myConveyorEndpointBlock))
									{
										flag = true;
										break;
									}
									continue;
								}
								flag2 = true;
								m_tmpInventoryItems.Add(item);
								if (m_tmpInventoryItems.Count == maxItemsToPull)
								{
									break;
								}
							}
							foreach (MyPhysicalInventoryItem tmpInventoryItem in m_tmpInventoryItems)
							{
								if (destinationInventory.VolumeFillFactor >= 1f)
								{
									return flag;
								}
								MyFixedPoint myFixedPoint = tmpInventoryItem.Amount;
								if (!MySession.Static.CreativeMode)
								{
									MyFixedPoint a = destinationInventory.ComputeAmountThatFits(tmpInventoryItem.Content.GetId());
									if (tmpInventoryItem.Content.TypeId != typeof(MyObjectBuilder_Ore) && tmpInventoryItem.Content.TypeId != typeof(MyObjectBuilder_Ingot))
									{
										a = MyFixedPoint.Floor(a);
									}
									myFixedPoint = MyFixedPoint.Min(a, myFixedPoint);
								}
								if (!(myFixedPoint == 0))
								{
									MyInventory.Transfer(inventory, destinationInventory, tmpInventoryItem.Content.GetId(), MyItemFlags.None, myFixedPoint);
									if (destinationInventory.VolumeFillFactor >= 0.99f)
									{
										break;
									}
								}
							}
						}
						if (IsComputationTasksCountOverLimit() || destinationInventory.VolumeFillFactor >= 0.99f)
						{
							break;
						}
					}
				}
			}
			else if (!conveyorSystem.m_isRecomputingGraph)
			{
				conveyorSystem.RecomputeConveyorEndpoints();
			}
			if (flag)
			{
				return !flag2;
			}
			return false;
		}

<<<<<<< HEAD
		/// <summary>
		/// Implements Pull all items - this method is highly optimized version for ship connector block with several early exits and protections for over utilization of CPU.
		/// Also it filters out all other connectors with Collect All turned on.
		/// </summary>
		/// <param name="start">start point</param>
		/// <param name="destinationInventory">destination inventory</param>
		/// <param name="playerId"></param>
		/// <param name="maxVolumeFillFactor">max volume fill factor</param>
		/// <returns>returns true if there was any pull</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static bool PullAllItemsForConnector(IMyConveyorEndpointBlock start, MyInventory destinationInventory, long playerId, float maxVolumeFillFactor)
		{
			MyCubeBlock myCubeBlock = start as MyCubeBlock;
			if (myCubeBlock == null)
			{
				return false;
			}
			if (IsComputationTasksCountOverLimit())
			{
				return false;
			}
			bool result = false;
			MyGridConveyorSystem conveyorSystem = myCubeBlock.CubeGrid.GridSystems.ConveyorSystem;
			ConveyorEndpointMapping conveyorEndpointMapping = conveyorSystem.GetConveyorEndpointMapping(start);
			if (conveyorEndpointMapping.pullElements != null)
			{
				for (int i = 0; i < conveyorEndpointMapping.pullElements.Count; i++)
				{
					if (destinationInventory.VolumeFillFactor >= maxVolumeFillFactor)
					{
						break;
					}
					IMyConveyorEndpointBlock myConveyorEndpointBlock = conveyorEndpointMapping.pullElements[i];
					MyCubeBlock myCubeBlock2 = myConveyorEndpointBlock as MyCubeBlock;
					MyShipConnector myShipConnector;
					if (myCubeBlock2 == null || start == myConveyorEndpointBlock || ((myShipConnector = myCubeBlock2 as MyShipConnector) != null && (bool)myShipConnector.CollectAll) || HasComputationTask(start, myConveyorEndpointBlock))
					{
						continue;
					}
					int inventoryCount = myCubeBlock2.InventoryCount;
					for (int j = 0; j < inventoryCount; j++)
					{
						MyInventory inventory = myCubeBlock2.GetInventory(j);
						if ((inventory.GetFlags() & MyInventoryFlags.CanSend) == 0 || inventory == destinationInventory)
						{
							continue;
						}
						List<MyPhysicalInventoryItem> items = inventory.GetItems();
						for (int k = 0; k < items.Count; k++)
						{
							MyDefinitionId definitionId = items[k].GetDefinitionId();
							MyFixedPoint myFixedPoint = destinationInventory.ComputeAmountThatFits(definitionId);
							if (myFixedPoint <= 0)
							{
								continue;
							}
							if (!CanTransfer(start, myConveyorEndpointBlock, definitionId, isPush: false))
							{
								if (HasComputationTask(start, myConveyorEndpointBlock))
								{
									break;
								}
								continue;
							}
							MyFixedPoint value = MyFixedPoint.Min(inventory.GetItemAmount(definitionId), myFixedPoint);
							MyInventory.Transfer(inventory, destinationInventory, definitionId, MyItemFlags.None, value);
							result = true;
							if (destinationInventory.VolumeFillFactor >= maxVolumeFillFactor)
							{
								break;
							}
						}
						if (IsComputationTasksCountOverLimit() || destinationInventory.VolumeFillFactor >= maxVolumeFillFactor)
						{
							break;
						}
					}
				}
			}
			else if (!conveyorSystem.m_isRecomputingGraph)
			{
				conveyorSystem.RecomputeConveyorEndpoints();
			}
			return result;
		}

		public static void PrepareTraversal(IMyConveyorEndpoint startingVertex, Predicate<IMyConveyorEndpoint> vertexFilter = null, Predicate<IMyConveyorEndpoint> vertexTraversable = null, Predicate<IMyPathEdge<IMyConveyorEndpoint>> edgeTraversable = null)
		{
			lock (Pathfinding)
			{
				Pathfinding.PrepareTraversal(startingVertex, vertexFilter, vertexTraversable, edgeTraversable);
			}
		}

		public static bool ComputeCanTransfer(IMyConveyorEndpointBlock start, IMyConveyorEndpointBlock end, MyDefinitionId? itemId)
		{
			using (MyUtils.ReuseCollection(ref m_reachableBuffer))
			{
				lock (Pathfinding)
				{
					SetTraversalPlayerId(start.ConveyorEndpoint.CubeBlock.OwnerId);
					if (itemId.HasValue)
					{
						SetTraversalInventoryItemDefinitionId(itemId.Value);
					}
					else
					{
						SetTraversalInventoryItemDefinitionId();
					}
					Predicate<IMyPathEdge<IMyConveyorEndpoint>> edgeTraversable = null;
					if (itemId.HasValue && NeedsLargeTube(itemId.Value))
					{
						edgeTraversable = IsConveyorLargePredicate;
					}
					Pathfinding.FindReachable(start.ConveyorEndpoint, m_reachableBuffer, (IMyConveyorEndpoint b) => b != null && b.CubeBlock == end, IsAccessAllowedPredicate, edgeTraversable);
				}
				return m_reachableBuffer.Count != 0;
			}
		}

		private static bool CanTransfer(IMyConveyorEndpointBlock start, IMyConveyorEndpointBlock endPoint, MyDefinitionId itemId, bool isPush, bool calcImmediately = false)
		{
			ConveyorEndpointMapping conveyorEndpointMapping = ((MyCubeBlock)start).CubeGrid.GridSystems.ConveyorSystem.GetConveyorEndpointMapping(start);
			if (calcImmediately)
			{
				if (conveyorEndpointMapping.TryGetTransfer(endPoint, itemId, isPush, out var canTransfer))
				{
					return canTransfer;
				}
				TransferData transferData = new TransferData(start, endPoint, itemId, isPush);
				transferData.ComputeTransfer();
				transferData.StoreTransferState();
				return transferData.m_canTransfer;
			}
			if (conveyorEndpointMapping.TryGetTransfer(endPoint, itemId, isPush, out var canTransfer2))
			{
				return canTransfer2;
			}
			lock (m_currentTransferComputationTasks)
			{
				(IMyConveyorEndpointBlock, IMyConveyorEndpointBlock) key = (start, endPoint);
				if (!m_currentTransferComputationTasks.ContainsKey(key))
				{
					TransferData workData = new TransferData(start, endPoint, itemId, isPush);
					Task value = Parallel.Start(ComputeTransferData, OnTransferDataComputed, workData);
					m_currentTransferComputationTasks.Add(key, value);
				}
			}
			return false;
		}

		private static bool HasComputationTask(IMyConveyorEndpointBlock start, IMyConveyorEndpointBlock endPoint)
		{
			lock (m_currentTransferComputationTasks)
			{
				(IMyConveyorEndpointBlock, IMyConveyorEndpointBlock) key = (start, endPoint);
				return m_currentTransferComputationTasks.ContainsKey(key);
			}
		}

		private static bool IsComputationTasksCountOverLimit()
		{
			lock (m_currentTransferComputationTasks)
			{
				return m_currentTransferComputationTasks.Count > MAX_COMPUTATION_TASKS_FOR_ALL_REQUESTS;
			}
		}

		private static void ComputeTransferData(WorkData workData)
		{
			if (workData != null)
			{
				TransferData transferData = workData as TransferData;
				if (transferData == null)
				{
					workData.FlagAsFailed();
				}
				else
				{
					transferData.ComputeTransfer();
				}
			}
		}

		private static void OnTransferDataComputed(WorkData workData)
		{
			if (workData == null && MyFakes.FORCE_NO_WORKER)
			{
				MyLog.Default.WriteLine("OnTransferDataComputed: workData is null on MyGridConveyorSystem to Check");
				return;
			}
			TransferData transferData = workData as TransferData;
			if (transferData == null && workData != null)
			{
				workData.FlagAsFailed();
				return;
			}
			transferData.StoreTransferState();
			lock (m_currentTransferComputationTasks)
			{
				m_currentTransferComputationTasks.Remove((transferData.m_start, transferData.m_endPoint));
			}
		}

		/// <summary>
		/// Implements Pull Item of specified item id and amount, this method does two steps, checks the amount and then transfer items if the amount was correct
		/// </summary>
		/// <param name="itemId">Item id</param>
		/// <param name="amount">Amount to transfer</param>
		/// <param name="start">start point</param>
		/// <param name="destinationInventory">destination inventory</param>
		/// <param name="calcImmediately">If false, item request will be done on parallel task. This mean it may not return the item immediately.
		///  Use true only if item is critical to be returned when this method is called, because it may be slow.. Use false if it does not matter if you have to ask the
		///  system multiple times (Ex. if you call often or in intervals).</param>
		/// <returns>if true operation was success</returns>
		internal bool PullItem(MyDefinitionId itemId, MyFixedPoint amount, IMyConveyorEndpointBlock start, MyInventory destinationInventory, bool calcImmediately = false)
		{
			ConveyorEndpointMapping conveyorEndpointMapping = GetConveyorEndpointMapping(start);
			if (conveyorEndpointMapping.pullElements != null)
			{
				List<Tuple<MyInventory, MyFixedPoint>> list = new List<Tuple<MyInventory, MyFixedPoint>>();
				MyFixedPoint myFixedPoint = amount;
				for (int i = 0; i < conveyorEndpointMapping.pullElements.Count; i++)
				{
					MyCubeBlock myCubeBlock = conveyorEndpointMapping.pullElements[i] as MyCubeBlock;
					if (myCubeBlock == null)
					{
						continue;
					}
					int inventoryCount = myCubeBlock.InventoryCount;
					for (int j = 0; j < inventoryCount; j++)
					{
						MyInventory inventory = myCubeBlock.GetInventory(j);
						if ((inventory.GetFlags() & MyInventoryFlags.CanSend) == 0 || inventory == destinationInventory || !CanTransfer(start, conveyorEndpointMapping.pullElements[i], itemId, isPush: false, calcImmediately))
						{
							continue;
						}
						MyFixedPoint itemAmount = inventory.GetItemAmount(itemId);
						itemAmount = MyFixedPoint.Min(itemAmount, myFixedPoint);
						if (!(itemAmount == 0))
						{
							list.Add(new Tuple<MyInventory, MyFixedPoint>(inventory, itemAmount));
							myFixedPoint -= itemAmount;
							if (myFixedPoint == 0)
							{
								break;
							}
						}
					}
					if (myFixedPoint == 0)
					{
						break;
					}
				}
				if (myFixedPoint != 0)
				{
					return false;
				}
				foreach (Tuple<MyInventory, MyFixedPoint> item in list)
				{
					MyInventory.Transfer(item.Item1, destinationInventory, itemId, MyItemFlags.None, item.Item2);
				}
			}
			else if (!m_isRecomputingGraph)
			{
				RecomputeConveyorEndpoints();
			}
			return true;
		}

		/// <summary>
		/// Implements pull item with possible optional remove. This method does everything in one step so it's bit faster.
		/// </summary>
		/// <param name="itemId">Item id</param>
		/// <param name="amount">Amount to transfer</param>
		/// <param name="start">start point</param>
		/// <param name="destinationInventory">destination inventory</param>
		/// <param name="remove">if true item is removed from inventories instead of transfer</param>
		/// <param name="calcImmediately">If false, item request will be done on parallel task. This mean it may not return the item immediately.
		///  Use true only if item is critical to be returned when this method is called, because it may be slow. Use false if it does not matter if you have to ask the
		///  system multiple times (Ex. if you call often or in intervals).</param>
		/// <returns>amount of item pulled</returns>
		public MyFixedPoint PullItem(MyDefinitionId itemId, MyFixedPoint? amount, IMyConveyorEndpointBlock start, MyInventory destinationInventory, bool remove, bool calcImmediately)
		{
			MyFixedPoint result = 0;
			ConveyorEndpointMapping conveyorEndpointMapping = GetConveyorEndpointMapping(start);
			if (conveyorEndpointMapping.pullElements != null)
			{
				for (int i = 0; i < conveyorEndpointMapping.pullElements.Count; i++)
				{
					MyCubeBlock myCubeBlock = conveyorEndpointMapping.pullElements[i] as MyCubeBlock;
					if (myCubeBlock == null)
					{
						continue;
					}
					int inventoryCount = myCubeBlock.InventoryCount;
					for (int j = 0; j < inventoryCount; j++)
					{
						MyInventory inventory = myCubeBlock.GetInventory(j);
						if ((inventory.GetFlags() & MyInventoryFlags.CanSend) == 0 || inventory == destinationInventory || !CanTransfer(start, conveyorEndpointMapping.pullElements[i], itemId, isPush: false, calcImmediately))
						{
							continue;
						}
						MyFixedPoint itemAmount = inventory.GetItemAmount(itemId);
						if (amount.HasValue)
						{
							itemAmount = (amount.HasValue ? MyFixedPoint.Min(itemAmount, amount.Value) : itemAmount);
							if (itemAmount == 0)
							{
								continue;
							}
							if (remove)
							{
								result += inventory.RemoveItemsOfType(itemAmount, itemId);
							}
							else
							{
								result += MyInventory.Transfer(inventory, destinationInventory, itemId, MyItemFlags.None, itemAmount);
							}
							amount -= itemAmount;
							if (amount.Value == 0)
							{
								return result;
							}
						}
						else if (remove)
						{
							result += inventory.RemoveItemsOfType(itemAmount, itemId);
						}
						else
						{
							result += MyInventory.Transfer(inventory, destinationInventory, itemId, MyItemFlags.None, itemAmount);
						}
						if (destinationInventory.CargoPercentage >= 0.99f)
						{
							break;
						}
					}
					if (destinationInventory.CargoPercentage >= 0.99f)
					{
						break;
					}
				}
			}
			else if (!m_isRecomputingGraph)
			{
				RecomputeConveyorEndpoints();
			}
			return result;
		}

<<<<<<< HEAD
		/// <summary>
		/// Implements Pull Items, very fast variant when pulling based on constraint. It does not pull full bottles.
		/// </summary>
		/// <param name="inventoryConstraint"></param>
		/// <param name="amount"></param>
		/// <param name="start"></param>
		/// <param name="destinationInventory"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyFixedPoint PullItems(MyInventoryConstraint inventoryConstraint, MyFixedPoint amount, IMyConveyorEndpointBlock start, MyInventory destinationInventory)
		{
			MyFixedPoint result = 0;
			if (destinationInventory.VolumeFillFactor >= 0.99f || inventoryConstraint == null || amount == 0)
			{
				return result;
			}
			m_tmpRequestedItemSet.Set(inventoryConstraint);
			ConveyorEndpointMapping conveyorEndpointMapping = GetConveyorEndpointMapping(start);
			if (conveyorEndpointMapping.pullElements != null)
			{
				for (int i = 0; i < conveyorEndpointMapping.pullElements.Count; i++)
				{
					MyCubeBlock myCubeBlock = conveyorEndpointMapping.pullElements[i] as MyCubeBlock;
					if (myCubeBlock == null)
<<<<<<< HEAD
					{
						continue;
					}
					int inventoryCount = myCubeBlock.InventoryCount;
					for (int j = 0; j < inventoryCount; j++)
					{
						MyInventory inventory = myCubeBlock.GetInventory(j);
						if ((inventory.GetFlags() & MyInventoryFlags.CanSend) == 0 || inventory == destinationInventory)
						{
							continue;
						}
						using (MyUtils.ReuseCollection(ref m_tmpInventoryItems))
						{
							foreach (MyPhysicalInventoryItem item in inventory.GetItems())
							{
								MyDefinitionId id = item.Content.GetId();
								if (m_tmpRequestedItemSet.Contains(id))
								{
									MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item.Content as MyObjectBuilder_GasContainerObject;
									if ((myObjectBuilder_GasContainerObject == null || !(myObjectBuilder_GasContainerObject.GasLevel >= 1f)) && CanTransfer(start, conveyorEndpointMapping.pullElements[i], id, isPush: false))
									{
										m_tmpInventoryItems.Add(item);
									}
								}
							}
							foreach (MyPhysicalInventoryItem tmpInventoryItem in m_tmpInventoryItems)
							{
								MyFixedPoint value = MyFixedPoint.Min(tmpInventoryItem.Amount, amount);
								MyFixedPoint myFixedPoint = MyInventory.Transfer(inventory, destinationInventory, tmpInventoryItem.ItemId, -1, value);
								result += myFixedPoint;
								amount -= myFixedPoint;
								if (destinationInventory.VolumeFillFactor >= 0.99f || amount <= 0)
								{
=======
					{
						continue;
					}
					int inventoryCount = myCubeBlock.InventoryCount;
					for (int j = 0; j < inventoryCount; j++)
					{
						MyInventory inventory = myCubeBlock.GetInventory(j);
						if ((inventory.GetFlags() & MyInventoryFlags.CanSend) == 0 || inventory == destinationInventory)
						{
							continue;
						}
						using (MyUtils.ReuseCollection(ref m_tmpInventoryItems))
						{
							foreach (MyPhysicalInventoryItem item in inventory.GetItems())
							{
								MyDefinitionId id = item.Content.GetId();
								if (m_tmpRequestedItemSet.Contains(id))
								{
									MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item.Content as MyObjectBuilder_GasContainerObject;
									if ((myObjectBuilder_GasContainerObject == null || !(myObjectBuilder_GasContainerObject.GasLevel >= 1f)) && CanTransfer(start, conveyorEndpointMapping.pullElements[i], id, isPush: false))
									{
										m_tmpInventoryItems.Add(item);
									}
								}
							}
							foreach (MyPhysicalInventoryItem tmpInventoryItem in m_tmpInventoryItems)
							{
								MyFixedPoint value = MyFixedPoint.Min(tmpInventoryItem.Amount, amount);
								MyFixedPoint myFixedPoint = MyInventory.Transfer(inventory, destinationInventory, tmpInventoryItem.ItemId, -1, value);
								result += myFixedPoint;
								amount -= myFixedPoint;
								if (destinationInventory.VolumeFillFactor >= 0.99f || amount <= 0)
								{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
									return result;
								}
							}
						}
						if (destinationInventory.VolumeFillFactor >= 0.99f)
						{
							return result;
						}
					}
				}
			}
			else if (!m_isRecomputingGraph)
			{
				RecomputeConveyorEndpoints();
			}
			return result;
		}

		public static MyFixedPoint ConveyorSystemItemAmount(IMyConveyorEndpointBlock start, MyInventory destinationInventory, long playerId, MyDefinitionId itemId)
		{
			MyFixedPoint result = 0;
			using (new MyConveyorLine.InvertedConductivity())
			{
				lock (Pathfinding)
				{
					SetTraversalPlayerId(playerId);
					SetTraversalInventoryItemDefinitionId(itemId);
					PrepareTraversal(start.ConveyorEndpoint, null, IsAccessAllowedPredicate, NeedsLargeTube(itemId) ? IsConveyorLargePredicate : null);
					foreach (IMyConveyorEndpoint item in Pathfinding)
					{
						MyCubeBlock myCubeBlock = ((item.CubeBlock != null && item.CubeBlock.HasInventory) ? item.CubeBlock : null);
						if (myCubeBlock == null)
						{
							continue;
						}
						for (int i = 0; i < myCubeBlock.InventoryCount; i++)
						{
							MyInventory inventory = myCubeBlock.GetInventory(i);
							if ((inventory.GetFlags() & MyInventoryFlags.CanSend) != 0 && inventory != destinationInventory)
							{
								result += inventory.GetItemAmount(itemId);
							}
						}
					}
					return result;
				}
			}
		}

		/// <summary>
		/// Implements push item from one source block. Item will be generated from source.
		/// </summary>
		/// <param name="start"> Source block</param>
		/// <param name="itemDefId">Item type to be transferred</param>
		/// <param name="amount">Amount of items to transfer</param>
		/// <param name="partialPush">If true, items fill be pushed even though not all can fit the conveyor system. Items that can't fit will be thrown away. If false, items will be pushed into system only when all of them fits.</param>
		/// <param name="calcImmediately">if conveyor should calculate network on call</param>
		/// <param name="transferedAmount">Returning amount of generated items</param>
		/// <returns>Returns info whether all items could fit in target network or not.</returns>
		public bool PushGenerateItem(MyDefinitionId itemDefId, MyFixedPoint? amount, IMyConveyorEndpointBlock start, out MyFixedPoint transferedAmount, bool partialPush = true, bool calcImmediately = false)
		{
			bool flag = false;
			List<Tuple<MyInventory, MyFixedPoint>> list = new List<Tuple<MyInventory, MyFixedPoint>>();
			ConveyorEndpointMapping conveyorEndpointMapping = GetConveyorEndpointMapping(start);
			MyFixedPoint myFixedPoint = 0;
			if (amount.HasValue)
			{
				myFixedPoint = amount.Value;
			}
			if (conveyorEndpointMapping.pushElements != null)
			{
				for (int i = 0; i < conveyorEndpointMapping.pushElements.Count; i++)
				{
					MyCubeBlock myCubeBlock = conveyorEndpointMapping.pushElements[i] as MyCubeBlock;
					if (myCubeBlock == null)
					{
						continue;
					}
					int inventoryCount = myCubeBlock.InventoryCount;
					for (int j = 0; j < inventoryCount; j++)
					{
						MyInventory inventory = myCubeBlock.GetInventory(j);
						if ((inventory.GetFlags() & MyInventoryFlags.CanReceive) != 0)
						{
							MyFixedPoint a = inventory.ComputeAmountThatFits(itemDefId);
							a = MyFixedPoint.Min(a, myFixedPoint);
							if (inventory.CheckConstraint(itemDefId) && !(a == 0) && CanTransfer(start, conveyorEndpointMapping.pushElements[i], itemDefId, isPush: true, calcImmediately))
							{
								list.Add(new Tuple<MyInventory, MyFixedPoint>(inventory, a));
								myFixedPoint -= a;
							}
						}
					}
					if (myFixedPoint <= 0)
					{
						flag = true;
						break;
					}
				}
			}
			else if (!m_isRecomputingGraph)
			{
				RecomputeConveyorEndpoints();
			}
			if (flag || partialPush)
			{
				MyObjectBuilder_Base objectBuilder = MyObjectBuilderSerializer.CreateNewObject(itemDefId);
				foreach (Tuple<MyInventory, MyFixedPoint> item in list)
				{
					item.Item1.AddItems(item.Item2, objectBuilder);
				}
				if (amount.HasValue)
				{
					transferedAmount = amount.Value - myFixedPoint;
				}
				else
				{
					transferedAmount = 0;
				}
			}
			else
			{
				transferedAmount = 0;
			}
			return flag;
		}

		public static void PushAnyRequest(IMyConveyorEndpointBlock start, MyInventory srcInventory, uint maxItemsToPush = 10u)
		{
			if (srcInventory.Empty() || maxItemsToPush == 0)
			{
				return;
			}
			MyPhysicalInventoryItem[] array = srcInventory.GetItems().ToArray();
			foreach (MyPhysicalInventoryItem toSend in array)
			{
				if (ItemPushRequest(start, srcInventory, toSend))
				{
					maxItemsToPush--;
					if (maxItemsToPush == 0)
					{
						break;
					}
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Push items from inventory to conveyor system.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="srcInventory"></param>        
		/// <param name="toSend"></param>        
		/// <returns>True if at least something was transferred.</returns>
=======
		public static void PushAnyRequest(IMyConveyorEndpointBlock start, MyInventory srcInventory, uint maxItemsToPush = 10u)
		{
			if (srcInventory.Empty() || maxItemsToPush == 0)
			{
				return;
			}
			MyPhysicalInventoryItem[] array = srcInventory.GetItems().ToArray();
			foreach (MyPhysicalInventoryItem toSend in array)
			{
				if (ItemPushRequest(start, srcInventory, toSend))
				{
					maxItemsToPush--;
					if (maxItemsToPush == 0)
					{
						break;
					}
				}
			}
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static bool ItemPushRequest(IMyConveyorEndpointBlock start, MyInventory srcInventory, MyPhysicalInventoryItem toSend)
		{
			MyCubeBlock myCubeBlock = start as MyCubeBlock;
			if (myCubeBlock == null)
			{
				return false;
			}
			bool result = false;
			MyGridConveyorSystem conveyorSystem = myCubeBlock.CubeGrid.GridSystems.ConveyorSystem;
			ConveyorEndpointMapping conveyorEndpointMapping = conveyorSystem.GetConveyorEndpointMapping(start);
			if (conveyorEndpointMapping.pushElements != null)
			{
				MyDefinitionId id = toSend.Content.GetId();
				for (int i = 0; i < conveyorEndpointMapping.pushElements.Count; i++)
				{
					MyCubeBlock myCubeBlock2 = conveyorEndpointMapping.pushElements[i] as MyCubeBlock;
					if (myCubeBlock2 == null)
					{
						continue;
					}
					int inventoryCount = myCubeBlock2.InventoryCount;
					for (int j = 0; j < inventoryCount; j++)
					{
						MyInventory inventory = myCubeBlock2.GetInventory(j);
						if ((inventory.GetFlags() & MyInventoryFlags.CanReceive) != 0 && inventory != srcInventory)
						{
							MyFixedPoint myFixedPoint = inventory.ComputeAmountThatFits(id);
							if (inventory.CheckConstraint(id) && !(myFixedPoint == 0) && CanTransfer(start, conveyorEndpointMapping.pushElements[i], toSend.GetDefinitionId(), isPush: true))
							{
								MyInventory.Transfer(srcInventory, inventory, toSend.ItemId, -1, myFixedPoint);
								result = true;
							}
						}
					}
				}
			}
			else if (!conveyorSystem.m_isRecomputingGraph)
			{
				conveyorSystem.RecomputeConveyorEndpoints();
			}
			return result;
		}

		/// <summary>
		/// Starts the conveyor endpoint mapping re-computation, aborts the current process if needed.
		/// </summary>
		private void RecomputeConveyorEndpoints()
		{
			m_conveyorConnections.Clear();
			if (m_isRecomputingGraph)
			{
				m_isRecomputationIsAborted = true;
			}
			else
			{
				StartRecomputationThread();
			}
		}

		/// <summary>
		/// Starts the conveyor endpoint mapping re-computation.
		/// </summary>
		private void StartRecomputationThread()
		{
			m_conveyorConnectionsForThread.Clear();
			m_isRecomputingGraph = true;
			m_isRecomputationIsAborted = false;
			m_isRecomputationInterrupted = false;
			m_endpointIterator = null;
			Parallel.Start(UpdateConveyorEndpointMapping, OnConveyorEndpointMappingUpdateCompleted);
		}

		public static void RecomputeMappingForBlock(IMyConveyorEndpointBlock processedBlock)
		{
			MyCubeBlock myCubeBlock = processedBlock as MyCubeBlock;
			if (myCubeBlock == null || myCubeBlock.CubeGrid == null || myCubeBlock.CubeGrid.GridSystems == null || myCubeBlock.CubeGrid.GridSystems.ConveyorSystem == null)
			{
				return;
			}
			ConveyorEndpointMapping value = myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.ComputeMappingForBlock(processedBlock);
			if (myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnections.ContainsKey(processedBlock))
			{
				myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnections[processedBlock] = value;
			}
			else
			{
				myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnections.Add(processedBlock, value);
			}
			if (myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_isRecomputingGraph)
			{
				if (myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnectionsForThread.ContainsKey(processedBlock))
				{
					myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnectionsForThread[processedBlock] = value;
				}
				else
				{
					myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnectionsForThread.Add(processedBlock, value);
				}
			}
		}

		private ConveyorEndpointMapping ComputeMappingForBlock(IMyConveyorEndpointBlock processedBlock)
		{
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			ConveyorEndpointMapping conveyorEndpointMapping = new ConveyorEndpointMapping();
			PullInformation pullInformation = processedBlock.GetPullInformation();
			if (pullInformation != null)
			{
				conveyorEndpointMapping.pullElements = new List<IMyConveyorEndpointBlock>();
				lock (Pathfinding)
				{
					SetTraversalPlayerId(pullInformation.OwnerID);
					if (pullInformation.ItemDefinition != default(MyDefinitionId))
					{
						SetTraversalInventoryItemDefinitionId(pullInformation.ItemDefinition);
						using (new MyConveyorLine.InvertedConductivity())
						{
							PrepareTraversal(processedBlock.ConveyorEndpoint, null, IsAccessAllowedPredicate, NeedsLargeTube(pullInformation.ItemDefinition) ? IsConveyorLargePredicate : null);
							AddReachableEndpoints(processedBlock, conveyorEndpointMapping.pullElements, MyInventoryFlags.CanSend);
						}
					}
					else if (pullInformation.Constraint != null)
					{
						SetTraversalInventoryItemDefinitionId();
						using (new MyConveyorLine.InvertedConductivity())
						{
							PrepareTraversal(processedBlock.ConveyorEndpoint, null, IsAccessAllowedPredicate);
							AddReachableEndpoints(processedBlock, conveyorEndpointMapping.pullElements, MyInventoryFlags.CanSend);
						}
					}
				}
			}
			PullInformation pushInformation = processedBlock.GetPushInformation();
			if (pushInformation != null)
			{
				conveyorEndpointMapping.pushElements = new List<IMyConveyorEndpointBlock>();
				lock (Pathfinding)
				{
					SetTraversalPlayerId(pushInformation.OwnerID);
					HashSet<MyDefinitionId> val = new HashSet<MyDefinitionId>();
					if (pushInformation.ItemDefinition != default(MyDefinitionId))
					{
						val.Add(pushInformation.ItemDefinition);
					}
					Enumerator<MyDefinitionId> enumerator;
					if (pushInformation.Constraint != null)
					{
						enumerator = pushInformation.Constraint.ConstrainedIds.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								MyDefinitionId current = enumerator.get_Current();
								val.Add(current);
							}
						}
						finally
						{
							((IDisposable)enumerator).Dispose();
						}
						Enumerator<MyObjectBuilderType> enumerator2 = pushInformation.Constraint.ConstrainedTypes.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								MyObjectBuilderType current2 = enumerator2.get_Current();
								MyDefinitionManager.Static.TryGetDefinitionsByTypeId(current2, val);
							}
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
					}
					if (val.get_Count() == 0 && (pushInformation.Constraint == null || pushInformation.Constraint.Description == "Empty constraint"))
					{
						SetTraversalInventoryItemDefinitionId();
						PrepareTraversal(processedBlock.ConveyorEndpoint, null, IsAccessAllowedPredicate);
						AddReachableEndpoints(processedBlock, conveyorEndpointMapping.pushElements, MyInventoryFlags.CanReceive);
						return conveyorEndpointMapping;
					}
<<<<<<< HEAD
					if (hashSet.Count == 0 && (pushInformation.Constraint == null || pushInformation.Constraint.Description == "Empty constraint"))
					{
						SetTraversalInventoryItemDefinitionId();
						PrepareTraversal(processedBlock.ConveyorEndpoint, null, IsAccessAllowedPredicate);
						AddReachableEndpoints(processedBlock, conveyorEndpointMapping.pushElements, MyInventoryFlags.CanReceive);
						return conveyorEndpointMapping;
					}
					foreach (MyDefinitionId item in hashSet)
					{
						SetTraversalInventoryItemDefinitionId(item);
						if (NeedsLargeTube(item))
						{
							PrepareTraversal(processedBlock.ConveyorEndpoint, null, IsAccessAllowedPredicate, IsConveyorLargePredicate);
=======
					enumerator = val.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							MyDefinitionId current3 = enumerator.get_Current();
							SetTraversalInventoryItemDefinitionId(current3);
							if (NeedsLargeTube(current3))
							{
								PrepareTraversal(processedBlock.ConveyorEndpoint, null, IsAccessAllowedPredicate, IsConveyorLargePredicate);
							}
							else
							{
								PrepareTraversal(processedBlock.ConveyorEndpoint, null, IsAccessAllowedPredicate);
							}
							AddReachableEndpoints(processedBlock, conveyorEndpointMapping.pushElements, MyInventoryFlags.CanReceive, current3);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						else
						{
							PrepareTraversal(processedBlock.ConveyorEndpoint, null, IsAccessAllowedPredicate);
						}
						AddReachableEndpoints(processedBlock, conveyorEndpointMapping.pushElements, MyInventoryFlags.CanReceive, item);
					}
<<<<<<< HEAD
					return conveyorEndpointMapping;
=======
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			return conveyorEndpointMapping;
		}

		private static void AddReachableEndpoints(IMyConveyorEndpointBlock processedBlock, List<IMyConveyorEndpointBlock> resultList, MyInventoryFlags flagToCheck, MyDefinitionId? definitionId = null)
		{
			foreach (IMyConveyorEndpoint item in Pathfinding)
			{
				if ((item.CubeBlock == processedBlock && !processedBlock.AllowSelfPulling()) || item.CubeBlock == null || !item.CubeBlock.HasInventory)
				{
					continue;
				}
				IMyConveyorEndpointBlock myConveyorEndpointBlock = item.CubeBlock as IMyConveyorEndpointBlock;
				if (myConveyorEndpointBlock == null)
				{
					continue;
				}
				MyCubeBlock cubeBlock = item.CubeBlock;
				bool flag = false;
				for (int i = 0; i < cubeBlock.InventoryCount; i++)
				{
					MyInventory inventory = cubeBlock.GetInventory(i);
					if ((inventory.GetFlags() & flagToCheck) != 0 && (!definitionId.HasValue || inventory.CheckConstraint(definitionId.Value)))
					{
						flag = true;
						break;
					}
				}
				if (flag && !resultList.Contains(myConveyorEndpointBlock))
				{
					resultList.Add(myConveyorEndpointBlock);
				}
			}
		}

		/// <summary>
		/// Computes the conveyor endpoint mappings.
		/// The conveyor endpoint blocks come from m_conveyorEndpointBlocks, and are processed iteratively.
		/// It does not process all of them at once, it processes them until the task has been running longer than MAX_RECOMPUTE_DURATION_MILLISECONDS
		/// If it exceeds this time, it will exit and restart itself the next frame.
		///
		/// The task can also be aborted, when it is aborted, it will throw away all intermediate data and restart.
		///
		/// Accessing the grid conveyor system will be slow until this has finished computing, after which a large performance gain should be had.
		/// </summary>
		private void UpdateConveyorEndpointMapping()
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			using (m_iteratorLock.AcquireExclusiveUsing())
			{
				long timestamp = Stopwatch.GetTimestamp();
				m_isRecomputationInterrupted = false;
				if (m_endpointIterator == null)
				{
					m_endpointIterator = (IEnumerator<IMyConveyorEndpointBlock>)(object)m_conveyorEndpointBlocks.GetEnumerator();
					m_endpointIterator.MoveNext();
				}
				IMyConveyorEndpointBlock current = m_endpointIterator.Current;
				while (current != null)
				{
					if (m_isRecomputationIsAborted)
					{
						m_isRecomputationInterrupted = true;
						break;
					}
					if (new TimeSpan(Stopwatch.GetTimestamp() - timestamp).TotalMilliseconds > 10.0)
					{
						m_isRecomputationInterrupted = true;
						break;
					}
					ConveyorEndpointMapping value = ComputeMappingForBlock(current);
					m_conveyorConnectionsForThread.Add(current, value);
					if (m_endpointIterator != null)
					{
						m_endpointIterator.MoveNext();
						current = m_endpointIterator.Current;
						continue;
					}
					m_isRecomputationIsAborted = true;
					m_isRecomputationInterrupted = true;
					break;
				}
			}
		}

		/// <summary>
		/// Called when the computation finishes. If it was aborted, restarts with a clean slate.
		/// Otherwise, adds the results of the computation to the main thread accessible connections map.
		/// Will continue the task if it was interrupted by timeout.
		/// </summary>
		private void OnConveyorEndpointMappingUpdateCompleted()
		{
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			using (m_iteratorLock.AcquireExclusiveUsing())
			{
				if (m_isRecomputationIsAborted)
				{
					StartRecomputationThread();
					return;
				}
				foreach (KeyValuePair<IMyConveyorEndpointBlock, ConveyorEndpointMapping> item in m_conveyorConnectionsForThread)
				{
					if (m_conveyorConnections.ContainsKey(item.Key))
					{
						m_conveyorConnections[item.Key] = item.Value;
<<<<<<< HEAD
=======
					}
					else
					{
						m_conveyorConnections.Add(item.Key, item.Value);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				m_conveyorConnectionsForThread.Clear();
				if (m_isRecomputationInterrupted)
				{
					Parallel.Start(UpdateConveyorEndpointMapping, OnConveyorEndpointMappingUpdateCompleted);
					return;
				}
				m_endpointIterator = null;
				m_isRecomputingGraph = false;
				Enumerator<MyConveyorLine> enumerator2 = m_lines.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
<<<<<<< HEAD
						m_conveyorConnections.Add(item.Key, item.Value);
					}
				}
				m_conveyorConnectionsForThread.Clear();
				if (m_isRecomputationInterrupted)
				{
					Parallel.Start(UpdateConveyorEndpointMapping, OnConveyorEndpointMappingUpdateCompleted);
					return;
				}
				m_endpointIterator = null;
				m_isRecomputingGraph = false;
				foreach (MyConveyorLine line in m_lines)
				{
					line.UpdateIsWorking();
=======
						enumerator2.get_Current().UpdateIsWorking();
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		private ConveyorEndpointMapping GetConveyorEndpointMapping(IMyConveyorEndpointBlock block)
		{
			if (m_conveyorConnections.ContainsKey(block))
			{
				return m_conveyorConnections[block];
			}
			return new ConveyorEndpointMapping();
		}

		public static void FindReachable(IMyConveyorEndpoint from, List<IMyConveyorEndpoint> reachableVertices, Predicate<IMyConveyorEndpoint> vertexFilter = null, Predicate<IMyConveyorEndpoint> vertexTraversable = null, Predicate<IMyPathEdge<IMyConveyorEndpoint>> edgeTraversable = null)
		{
			lock (Pathfinding)
			{
				Pathfinding.FindReachable(from, reachableVertices, vertexFilter, vertexTraversable, edgeTraversable);
			}
		}

		public static bool Reachable(IMyConveyorEndpoint from, IMyConveyorEndpoint to)
		{
			bool flag = false;
			lock (Pathfinding)
			{
				return Pathfinding.Reachable(from, to);
			}
		}

		/// <inheritdoc />
		public MyFixedPoint PullItem(MyDefinitionId itemDefinitionId, MyFixedPoint? amount, VRage.ModAPI.IMyEntity startingBlock, VRage.Game.ModAPI.IMyInventory destinationInventory, bool remove)
		{
			IMyConveyorEndpointBlock start;
			if ((start = startingBlock as IMyConveyorEndpointBlock) == null)
			{
				return 0;
			}
			MyInventory destinationInventory2;
			if ((destinationInventory2 = destinationInventory as MyInventory) == null)
			{
				return 0;
			}
			return PullItem(itemDefinitionId, amount, start, destinationInventory2, remove, calcImmediately: false);
		}

		/// <inheritdoc />
		public bool PushGenerateItem(MyDefinitionId itemDefinitionId, MyFixedPoint? amount, out MyFixedPoint transferredAmount, VRage.ModAPI.IMyEntity sourceBlock, bool partialPush)
		{
			IMyConveyorEndpointBlock start;
			if ((start = sourceBlock as IMyConveyorEndpointBlock) == null)
			{
				transferredAmount = MyFixedPoint.Zero;
				return false;
			}
			return PushGenerateItem(itemDefinitionId, amount, start, out transferredAmount, partialPush);
		}
	}
}
