using System;
using System.Collections.Generic;
using Havok;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Physics
{
	public class MyGridPhysicalHierarchy : MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>, IMySceneComponent
	{
		public delegate void MyActionWithData(MyCubeGrid grid, ref object data);

		public static MyGridPhysicalHierarchy Static;

		private readonly Dictionary<long, HashSet<MyEntity>> m_nonGridChildren = new Dictionary<long, HashSet<MyEntity>>();

		public void Load()
		{
			Static = this;
			base.SupportsOphrans = true;
			base.SupportsChildToChild = true;
		}

		public void Unload()
		{
			Static = null;
		}

		public override void AddNode(MyCubeGrid nodeToAdd)
		{
			base.AddNode(nodeToAdd);
			UpdateRoot(nodeToAdd);
		}

		public override void CreateLink(long linkId, MyCubeGrid parentNode, MyCubeGrid childNode)
		{
			base.CreateLink(linkId, parentNode, childNode);
			UpdateRoot(parentNode);
		}

		public override bool BreakLink(long linkId, MyCubeGrid parentNode, MyCubeGrid childNode = null)
		{
			if (childNode == null)
			{
				childNode = GetNode(parentNode).m_children.get_Item(linkId).NodeData;
			}
			bool flag = base.BreakLink(linkId, parentNode, childNode);
			if (!flag)
			{
				flag = base.BreakLink(linkId, childNode, parentNode);
			}
			if (flag)
			{
				UpdateRoot(parentNode);
				if (GetGroup(parentNode) != GetGroup(childNode))
				{
					UpdateRoot(childNode);
				}
			}
			return flag;
		}

		/// <summary>
		/// Gets the grid's parent in hierarchy
		/// </summary>
		public MyCubeGrid GetParent(MyCubeGrid grid)
		{
			Node node = GetNode(grid);
			if (node == null)
			{
				return null;
			}
			return GetParent(node);
		}

		public MyCubeGrid GetParent(Node node)
		{
			if (node.m_parents.Count == 0)
			{
				return null;
			}
			return node.m_parents.FirstPair().Value.NodeData;
		}

		public long GetParentLinkId(Node node)
		{
			if (node.m_parents.Count == 0)
			{
				return 0L;
			}
			return node.m_parents.FirstPair().Key;
		}

		public bool IsEntityParent(MyEntity entity)
		{
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return true;
			}
			return GetParent(myCubeGrid) == null;
		}

		public MyCubeGrid GetRoot(MyCubeGrid grid)
		{
			Group group = GetGroup(grid);
			if (group == null)
			{
				return grid;
			}
			MyCubeGrid myCubeGrid = group.GroupData.m_root;
			if (myCubeGrid == null)
			{
				myCubeGrid = grid;
			}
			return myCubeGrid;
		}

		/// <summary>
		/// Gets the entity through which the grid is connected to its parent (e.g. a MyMechanicalConnectionBlock). Returns null if it has no parent
		/// </summary>
		public MyEntity GetEntityConnectingToParent(MyCubeGrid grid)
		{
			Node node = GetNode(grid);
			if (node == null)
			{
				return null;
			}
			if (node.m_parents.Count == 0)
			{
				return null;
			}
			return MyEntities.GetEntityById(node.m_parents.FirstPair().Key);
		}

		public bool HasChildren(MyCubeGrid grid)
		{
			Node node = GetNode(grid);
			if (node != null)
			{
				return node.Children.Count > 0;
			}
			return false;
		}

		public bool IsCyclic(MyCubeGrid grid)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			Node node = GetNode(grid);
			if (node != null && node.Children.Count > 0)
			{
				Enumerator<long, Node> enumerator = node.ChildLinks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<long, Node> current = enumerator.get_Current();
						if (GetParentLinkId(current.Value) != current.Key)
						{
							return true;
						}
						if (IsCyclic(current.Value.NodeData))
						{
							return true;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			return false;
		}

		public void ApplyOnChildren(MyCubeGrid grid, Action<MyCubeGrid> action)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			Node node = GetNode(grid);
			if (node == null || node.Children.Count <= 0)
			{
				return;
			}
<<<<<<< HEAD
			foreach (KeyValuePair<long, Node> childLink in node.ChildLinks)
			{
				if (GetParentLinkId(childLink.Value) == childLink.Key)
				{
					action(childLink.Value.NodeData);
=======
			Enumerator<long, Node> enumerator = node.ChildLinks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<long, Node> current = enumerator.get_Current();
					if (GetParentLinkId(current.Value) == current.Key)
					{
						action(current.Value.NodeData);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void ApplyOnChildren(MyCubeGrid grid, ref object data, MyActionWithData action)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			Node node = GetNode(grid);
			if (node == null || node.Children.Count <= 0)
<<<<<<< HEAD
			{
				return;
			}
			foreach (KeyValuePair<long, Node> childLink in node.ChildLinks)
			{
				if (GetParentLinkId(childLink.Value) == childLink.Key)
				{
					action(childLink.Value.NodeData, ref data);
=======
			{
				return;
			}
			Enumerator<long, Node> enumerator = node.ChildLinks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<long, Node> current = enumerator.get_Current();
					if (GetParentLinkId(current.Value) == current.Key)
					{
						action(current.Value.NodeData, ref data);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void ApplyOnAllChildren(MyEntity entity, Action<MyEntity> action)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return;
			}
			Node node = GetNode(myCubeGrid);
			if (node != null && node.Children.Count > 0)
			{
<<<<<<< HEAD
				foreach (KeyValuePair<long, Node> childLink in node.ChildLinks)
				{
					if (GetParentLinkId(childLink.Value) == childLink.Key)
					{
						action(childLink.Value.NodeData);
					}
=======
				Enumerator<long, Node> enumerator = node.ChildLinks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<long, Node> current = enumerator.get_Current();
						if (GetParentLinkId(current.Value) == current.Key)
						{
							action(current.Value.NodeData);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			if (node == null || !m_nonGridChildren.TryGetValue(myCubeGrid.EntityId, out var value))
			{
				return;
			}
<<<<<<< HEAD
			foreach (MyEntity item in value)
			{
				action(item);
=======
			Enumerator<MyEntity> enumerator2 = value.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					MyEntity current2 = enumerator2.get_Current();
					action(current2);
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public bool InSameHierarchy(MyCubeGrid first, MyCubeGrid second)
		{
			MyCubeGrid root = GetRoot(first);
			MyCubeGrid root2 = GetRoot(second);
			return root == root2;
		}

		public bool IsChildOf(MyCubeGrid parentGrid, MyEntity entity)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			Node node = GetNode(parentGrid);
			if (node != null && node.Children.Count > 0)
			{
				Enumerator<long, Node> enumerator = node.ChildLinks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<long, Node> current = enumerator.get_Current();
						if (GetParentLinkId(current.Value) == current.Key && current.Value.NodeData == entity)
						{
							return true;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (node != null && m_nonGridChildren.TryGetValue(parentGrid.EntityId, out var value))
			{
				return value.Contains(entity);
			}
			return false;
		}

		/// <summary>
		/// Updates the root of the hierarchy node belongs to
		/// </summary>
		public void UpdateRoot(MyCubeGrid node)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			if (MyEntities.IsClosingAll)
			{
				return;
			}
			Group group = GetGroup(node);
			if (group == null)
<<<<<<< HEAD
			{
				return;
			}
			MyCubeGrid myCubeGrid = CalculateNewRoot(group);
			group.GroupData.m_root = myCubeGrid;
			if (myCubeGrid == null)
			{
				return;
			}
			ReplaceRoot(myCubeGrid);
			foreach (Node node2 in group.Nodes)
			{
				node2.NodeData.HierarchyUpdated(myCubeGrid);
=======
			{
				return;
			}
			MyCubeGrid myCubeGrid = CalculateNewRoot(group);
			group.GroupData.m_root = myCubeGrid;
			if (myCubeGrid == null)
			{
				return;
			}
			ReplaceRoot(myCubeGrid);
			Enumerator<Node> enumerator = group.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().NodeData.HierarchyUpdated(myCubeGrid);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		/// <summary>
		/// Calculates new root according to following criteria:
		/// Static grids have preference
		/// Controlled grids have preference except over static grids
		/// When multiple grids are preferred the one with higher mass is chosen
		/// In case of same mass, the one with lower entityId is chosen
		/// </summary>
		private MyCubeGrid CalculateNewRoot(Group group)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			if (group.m_members.get_Count() == 1)
			{
				return group.m_members.FirstElement<Node>().NodeData;
			}
			Node node = null;
			float num = 0f;
			List<Node> list = new List<Node>();
			if (group.m_members.get_Count() == 1)
			{
				return group.m_members.FirstElement<Node>().NodeData;
			}
			bool flag = false;
			long num2 = long.MaxValue;
			Enumerator<Node> enumerator = group.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Node current = enumerator.get_Current();
					if (current.NodeData.IsStatic || MyFixedGrids.IsRooted(current.NodeData))
					{
						if (!flag)
						{
							list.Clear();
							node = null;
							flag = true;
						}
						list.Add(current);
					}
					if (flag)
					{
						continue;
					}
<<<<<<< HEAD
					list.Add(node4);
				}
				if (flag)
				{
					continue;
				}
				if (IsGridControlled(node4.NodeData) && node4.NodeData.EntityId < num2)
				{
					node = node4;
					num2 = node4.NodeData.EntityId;
				}
				if (node4.NodeData.Physics != null)
				{
					float num3 = 0f;
					HkMassProperties? massProperties = node4.NodeData.Physics.Shape.MassProperties;
					if (massProperties.HasValue)
					{
						num3 = massProperties.Value.Mass;
					}
					if (num3 > num)
					{
						num = num3;
						list.Clear();
						list.Add(node4);
					}
					else if (num3 == num)
					{
						list.Add(node4);
=======
					if (IsGridControlled(current.NodeData) && current.NodeData.EntityId < num2)
					{
						node = current;
						num2 = current.NodeData.EntityId;
					}
					if (current.NodeData.Physics != null)
					{
						float num3 = 0f;
						HkMassProperties? massProperties = current.NodeData.Physics.Shape.MassProperties;
						if (massProperties.HasValue)
						{
							num3 = massProperties.Value.Mass;
						}
						if (num3 > num)
						{
							num = num3;
							list.Clear();
							list.Add(current);
						}
						else if (num3 == num)
						{
							list.Add(current);
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			Node node2 = null;
			if (list.Count == 1)
			{
				node2 = list[0];
			}
			else if (list.Count > 1)
			{
				long entityId = list[0].NodeData.EntityId;
				Node node3 = list[0];
				foreach (Node item in list)
				{
					if (MyWeldingGroups.Static.IsEntityParent(item.NodeData) && entityId > item.NodeData.EntityId)
					{
						entityId = item.NodeData.EntityId;
						node3 = item;
					}
				}
				node2 = node3;
			}
			if (node != null)
			{
				if (node2 == null)
				{
					node2 = node;
				}
				else if (node.NodeData.Physics != null && node2.NodeData.Physics != null)
				{
					float num4 = 0f;
					HkMassProperties? massProperties2 = node.NodeData.Physics.Shape.MassProperties;
					if (massProperties2.HasValue)
					{
						num4 = massProperties2.Value.Mass;
					}
					float num5 = 0f;
					massProperties2 = node2.NodeData.Physics.Shape.MassProperties;
					if (massProperties2.HasValue)
					{
						num5 = massProperties2.Value.Mass;
					}
					if (num5 / num4 < 2f)
					{
						node2 = node;
					}
				}
				else
				{
					node2 = node;
				}
			}
			return node2?.NodeData;
		}

		private bool IsGridControlled(MyCubeGrid grid)
		{
			MyShipController shipController = grid.GridSystems.ControlSystem.GetShipController();
			if (shipController != null)
			{
				return shipController.CubeGrid == grid;
			}
			return false;
		}

		/// <summary>
		/// Gets the position of pivot of constraint through which a grid is connected to its parent. 
		/// Returns null if it's not connected via mechanical connection block
		/// </summary>
		public Vector3? GetPivot(MyCubeGrid grid, bool parent = false)
		{
			return (GetEntityConnectingToParent(grid) as MyMechanicalConnectionBlockBase)?.GetConstraintPosition(grid, parent);
		}

		public void AddNonGridNode(MyCubeGrid parent, MyEntity entity)
		{
			if (GetGroup(parent) != null)
			{
				if (!m_nonGridChildren.TryGetValue(parent.EntityId, out var value))
				{
					value = new HashSet<MyEntity>();
					m_nonGridChildren.Add(parent.EntityId, value);
					parent.OnClose += RemoveAllNonGridNodes;
				}
				value.Add(entity);
			}
		}

		public void RemoveNonGridNode(MyCubeGrid parent, MyEntity entity)
		{
			if (GetGroup(parent) != null && m_nonGridChildren.TryGetValue(parent.EntityId, out var value))
			{
				value.Remove(entity);
				if (value.get_Count() == 0)
				{
					m_nonGridChildren.Remove(parent.EntityId);
					parent.OnClose -= RemoveAllNonGridNodes;
				}
			}
		}

		private void RemoveAllNonGridNodes(MyEntity parent)
		{
			m_nonGridChildren.Remove(parent.EntityId);
			parent.OnClose -= RemoveAllNonGridNodes;
		}

		public bool NonGridLinkExists(long parentId, MyEntity child)
		{
			if (m_nonGridChildren.TryGetValue(parentId, out var value))
			{
				return value.Contains(child);
			}
			return false;
		}

		public int GetNodeChainLength(MyCubeGrid grid)
		{
			return GetNode(grid)?.ChainLength ?? 0;
		}

		public void Log(MyCubeGrid grid)
		{
			MyLog.Default.IncreaseIndent();
			MyLog.Default.WriteLine(string.Format("{0}: name={1} physics={2} mass={3} static={4} controlled={5}", grid.EntityId, grid.DisplayName, grid.Physics != null, (grid.Physics != null && grid.Physics.Shape.MassProperties.HasValue) ? grid.Physics.Shape.MassProperties.Value.Mass.ToString() : "None", grid.IsStatic, IsGridControlled(grid)));
			ApplyOnChildren(grid, Log);
			MyLog.Default.DecreaseIndent();
		}

		public void Draw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_HIERARCHY)
			{
				ApplyOnNodes(DrawNode);
			}
		}

		private void DrawNode(MyCubeGrid grid, Node node)
		{
			if (node.m_parents.Count > 0)
			{
				MyRenderProxy.DebugDrawArrow3D(grid.PositionComp.GetPosition(), node.m_parents.FirstPair().Value.NodeData.PositionComp.GetPosition(), Color.Orange);
			}
			else
			{
				MyRenderProxy.DebugDrawAxis(grid.PositionComp.WorldMatrixRef, 1f, depthRead: false);
			}
		}

		public MyGridPhysicalHierarchy()
			: base(supportOphrans: false, (MajorGroupComparer)null)
		{
		}
	}
}
