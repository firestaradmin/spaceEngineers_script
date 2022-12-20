using System;
using System.Collections.Generic;
using Sandbox.Game.Entities.Cube;
<<<<<<< HEAD
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Game.ModAPI;
using VRage.Groups;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	public class MyCubeGridGroups : IMySceneComponent
	{
		public static MyCubeGridGroups Static;

		private MyGroupsBase<MyCubeGrid>[] m_groupsByType;

		public MyGroups<MyCubeGrid, MyGridLogicalGroupData> Logical = new MyGroups<MyCubeGrid, MyGridLogicalGroupData>(supportOphrans: true);

		public MyGroups<MyCubeGrid, MyGridPhysicalGroupData> Physical = new MyGroups<MyCubeGrid, MyGridPhysicalGroupData>(supportOphrans: true, MyGridPhysicalGroupData.IsMajorGroup);

		public MyGroups<MyCubeGrid, MyGridNoDamageGroupData> NoContactDamage = new MyGroups<MyCubeGrid, MyGridNoDamageGroupData>(supportOphrans: true);

		public MyGroups<MyCubeGrid, MyGridMechanicalGroupData> Mechanical = new MyGroups<MyCubeGrid, MyGridMechanicalGroupData>(supportOphrans: true);

		public MyGroups<MyCubeGrid, MyGridElectricalGroupData> Electrical = new MyGroups<MyCubeGrid, MyGridElectricalGroupData>(supportOphrans: true);

		public MyGroups<MySlimBlock, MyBlockGroupData> SmallToLargeBlockConnections = new MyGroups<MySlimBlock, MyBlockGroupData>();

		public MyGroups<MyCubeGrid, MyGridPhysicalDynamicGroupData> PhysicalDynamic = new MyGroups<MyCubeGrid, MyGridPhysicalDynamicGroupData>();

		private static readonly HashSet<object> m_tmpBlocksDebugHelper = new HashSet<object>();

		public event Action<IMyGridGroupData> OnGridGroupCreated;

		public event Action<IMyGridGroupData> OnGridGroupDestroyed;

		public MyCubeGridGroups()
		{
			m_groupsByType = new MyGroupsBase<MyCubeGrid>[5];
			m_groupsByType[0] = Logical;
			m_groupsByType[1] = Physical;
			m_groupsByType[2] = NoContactDamage;
			m_groupsByType[3] = Mechanical;
			m_groupsByType[4] = Electrical;
		}

		public void AddNode(GridLinkTypeEnum type, MyCubeGrid grid)
		{
			GetGroups(type).AddNode(grid);
		}

		public void RemoveNode(GridLinkTypeEnum type, MyCubeGrid grid)
		{
			GetGroups(type).RemoveNode(grid);
		}

		/// <summary>
		/// Creates link between parent and child.
		/// Parent is owner of constraint.
		/// LinkId must be unique only for parent, for grid it can be packed position of block which created constraint.
		/// </summary>
		public void CreateLink(GridLinkTypeEnum type, long linkId, MyCubeGrid parent, MyCubeGrid child)
		{
			GetGroups(type).CreateLink(linkId, parent, child);
			if (type == GridLinkTypeEnum.Physical && !parent.Physics.IsStatic && !child.Physics.IsStatic)
			{
				PhysicalDynamic.CreateLink(linkId, parent, child);
			}
			if (type == GridLinkTypeEnum.Electrical && parent?.GridSystems?.ResourceDistributor != null)
			{
				parent.GridSystems.ResourceDistributor.SetDataDirty(MyResourceDistributorComponent.ElectricityId);
			}
			if (type == GridLinkTypeEnum.Electrical && child?.GridSystems?.ResourceDistributor != null)
			{
				child.GridSystems.ResourceDistributor.SetDataDirty(MyResourceDistributorComponent.ElectricityId);
			}
		}

		/// <summary>
		/// Breaks link between parent and child, you can set child to null to find it by linkId.
		/// Returns true when link was removed, returns false when link was not found.
		/// </summary>
		public bool BreakLink(GridLinkTypeEnum type, long linkId, MyCubeGrid parent, MyCubeGrid child = null)
		{
			if (type == GridLinkTypeEnum.Physical)
			{
				PhysicalDynamic.BreakLink(linkId, parent, child);
			}
			if (type == GridLinkTypeEnum.Electrical && parent?.GridSystems?.ResourceDistributor != null)
			{
				parent.GridSystems.ResourceDistributor.SetDataDirty(MyResourceDistributorComponent.ElectricityId);
			}
			if (type == GridLinkTypeEnum.Electrical && child?.GridSystems?.ResourceDistributor != null)
			{
				child.GridSystems.ResourceDistributor.SetDataDirty(MyResourceDistributorComponent.ElectricityId);
			}
			return GetGroups(type).BreakLink(linkId, parent, child);
		}

		public void UpdateDynamicState(MyCubeGrid grid)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			bool flag = PhysicalDynamic.GetGroup(grid) != null;
			bool flag2 = !grid.IsStatic;
			if (flag && !flag2)
			{
				PhysicalDynamic.BreakAllLinks(grid);
			}
			else
			{
				if (!(!flag && flag2))
				{
					return;
				}
				MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node = Physical.GetNode(grid);
				if (node == null)
				{
					return;
				}
<<<<<<< HEAD
				foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> childLink in node.ChildLinks)
				{
					if (!childLink.Value.NodeData.IsStatic)
					{
						PhysicalDynamic.CreateLink(childLink.Key, grid, childLink.Value.NodeData);
					}
				}
=======
				Enumerator<long, MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> enumerator = node.ChildLinks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<long, MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> current = enumerator.get_Current();
						if (!current.Value.NodeData.IsStatic)
						{
							PhysicalDynamic.CreateLink(current.Key, grid, current.Value.NodeData);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> parentLink in node.ParentLinks)
				{
					if (!parentLink.Value.NodeData.IsStatic)
					{
						PhysicalDynamic.CreateLink(parentLink.Key, parentLink.Value.NodeData, grid);
					}
				}
			}
		}

		public MyGroupsBase<MyCubeGrid> GetGroups(GridLinkTypeEnum type)
		{
			return m_groupsByType[(int)type];
		}

		void IMySceneComponent.Load()
		{
			Static = new MyCubeGridGroups();
		}

		void IMySceneComponent.Unload()
		{
			Static = null;
			this.OnGridGroupCreated = null;
			this.OnGridGroupDestroyed = null;
		}

		internal static void RaiseOnGridGroupCreated(IMyGridGroupData data)
		{
			Static?.OnGridGroupCreated?.Invoke(data);
		}

		internal static void RaiseOnGridGroupDestroyed(IMyGridGroupData data)
		{
			Static?.OnGridGroupDestroyed?.Invoke(data);
		}

		public static T GetGridGroups<T>(GridLinkTypeEnum linking, T grids) where T : ICollection<IMyGridGroupData>
		{
			switch (linking)
			{
			case GridLinkTypeEnum.Electrical:
			{
				foreach (MyGroups<MyCubeGrid, MyGridElectricalGroupData>.Group group in Static.Electrical.Groups)
				{
					grids.Add(group.GroupData);
				}
				return grids;
			}
			case GridLinkTypeEnum.Logical:
			{
				foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group2 in Static.Logical.Groups)
				{
					grids.Add(group2.GroupData);
				}
				return grids;
			}
			case GridLinkTypeEnum.Mechanical:
			{
				foreach (MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Group group3 in Static.Mechanical.Groups)
				{
					grids.Add(group3.GroupData);
				}
				return grids;
			}
			case GridLinkTypeEnum.Physical:
			{
				foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group4 in Static.Physical.Groups)
				{
					grids.Add(group4.GroupData);
				}
				return grids;
			}
			case GridLinkTypeEnum.NoContactDamage:
			{
				foreach (MyGroups<MyCubeGrid, MyGridNoDamageGroupData>.Group group5 in Static.NoContactDamage.Groups)
				{
					grids.Add(group5.GroupData);
				}
				return grids;
			}
			default:
				return grids;
			}
		}

		public static IMyGridGroupData GetGridGroup(GridLinkTypeEnum linking, MyCubeGrid grid)
		{
			switch (linking)
			{
			case GridLinkTypeEnum.Electrical:
				return Static.Electrical.GetGroup(grid)?.GroupData;
			case GridLinkTypeEnum.Logical:
				return Static.Logical.GetGroup(grid)?.GroupData;
			case GridLinkTypeEnum.Mechanical:
				return Static.Mechanical.GetGroup(grid)?.GroupData;
			case GridLinkTypeEnum.Physical:
				return Static.Physical.GetGroup(grid)?.GroupData;
			case GridLinkTypeEnum.NoContactDamage:
				return Static.NoContactDamage.GetGroup(grid)?.GroupData;
			default:
				return null;
			}
		}

		public static void AddGridGroupLogic(GridLinkTypeEnum type, Func<IMyGridGroupData, MyGridGroupsDefaultEventHandler> creator)
		{
			MyAPIGateway.GridGroups.OnGridGroupCreated += delegate(IMyGridGroupData data)
			{
				if (data.LinkType == type)
				{
					creator(data);
				}
			};
		}

		internal static void DebugDrawBlockGroups<TNode, TGroupData>(MyGroups<TNode, TGroupData> groups) where TNode : MySlimBlock where TGroupData : IGroupData<TNode>, new()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			Enumerator<MyGroups<TNode, TGroupData>.Group> enumerator = groups.Groups.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<TNode, TGroupData>.Group current = enumerator.get_Current();
					Color color = new Vector3((float)(num++ % 15) / 15f, 1f, 1f).HSVtoColor();
					Enumerator<MyGroups<TNode, TGroupData>.Node> enumerator2 = current.Nodes.GetEnumerator();
					try
					{
<<<<<<< HEAD
						node2.NodeData.GetWorldBoundingBox(out var aabb);
						foreach (MyGroups<TNode, TGroupData>.Node child in node2.Children)
=======
						while (enumerator2.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							MyGroups<TNode, TGroupData>.Node current2 = enumerator2.get_Current();
							try
							{
								current2.NodeData.GetWorldBoundingBox(out var aabb);
								Enumerator<long, MyGroups<TNode, TGroupData>.Node> enumerator3 = current2.Children.GetEnumerator();
								try
								{
									while (enumerator3.MoveNext())
									{
										MyGroups<TNode, TGroupData>.Node current3 = enumerator3.get_Current();
										m_tmpBlocksDebugHelper.Add((object)current3);
									}
								}
								finally
								{
									((IDisposable)enumerator3).Dispose();
								}
								Enumerator<object> enumerator4 = m_tmpBlocksDebugHelper.GetEnumerator();
								try
								{
									while (enumerator4.MoveNext())
									{
										object current4 = enumerator4.get_Current();
										MyGroups<TNode, TGroupData>.Node node = null;
										int num2 = 0;
										enumerator3 = current2.Children.GetEnumerator();
										try
										{
											while (enumerator3.MoveNext())
											{
												MyGroups<TNode, TGroupData>.Node current5 = enumerator3.get_Current();
												if (current4 == current5)
												{
													node = current5;
													num2++;
												}
											}
										}
										finally
										{
											((IDisposable)enumerator3).Dispose();
										}
										node.NodeData.GetWorldBoundingBox(out var aabb2);
										MyRenderProxy.DebugDrawLine3D(aabb.Center, aabb2.Center, color, color, depthRead: false);
										MyRenderProxy.DebugDrawText3D((aabb.Center + aabb2.Center) * 0.5, num2.ToString(), color, 1f, depthRead: false);
									}
								}
								finally
								{
									((IDisposable)enumerator4).Dispose();
								}
								Color color2 = new Color(color.ToVector3() + 0.25f);
								MyRenderProxy.DebugDrawSphere(aabb.Center, 0.2f, color2.ToVector3(), 0.5f, depthRead: false, smooth: true);
								MyRenderProxy.DebugDrawText3D(aabb.Center, current2.LinkCount.ToString(), color2, 1f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
							}
							finally
							{
								m_tmpBlocksDebugHelper.Clear();
							}
<<<<<<< HEAD
							if (node != null)
							{
								node.NodeData.GetWorldBoundingBox(out var aabb2);
								MyRenderProxy.DebugDrawLine3D(aabb.Center, aabb2.Center, color, color, depthRead: false);
								MyRenderProxy.DebugDrawText3D((aabb.Center + aabb2.Center) * 0.5, num2.ToString(), color, 1f, depthRead: false);
							}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}
	}
}
