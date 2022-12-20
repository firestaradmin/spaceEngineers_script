<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Linq;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
<<<<<<< HEAD
using VRage.Game.ModAPI;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Groups;

namespace Sandbox.Game.Entities
{
	public class MyGridLogicalGroupData : MyGridGroupData<MyGridLogicalGroupData>
	{
		internal readonly MyGridTerminalSystem TerminalSystem;

		internal readonly MyGridWeaponSystem WeaponSystem = new MyGridWeaponSystem();

		internal readonly MyGridResourceDistributorSystem ResourceDistributor;

		public MyCubeGrid Root { get; private set; }

		public MyGridLogicalGroupData()
			: this(null)
		{
			base.LinkType = GridLinkTypeEnum.Logical;
		}

		public MyGridLogicalGroupData(string debugName)
		{
			TerminalSystem = new MyGridTerminalSystem(this);
			ResourceDistributor = new MyGridResourceDistributorSystem(debugName, this);
		}

		public override void OnRelease()
		{
<<<<<<< HEAD
			base.OnRelease();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ResourceDistributor.ClearData();
		}

		public override void OnNodeAdded<TGroupData>(MyCubeGrid entity, TGroupData prevGroup)
		{
			entity.OnAddedToGroup(this);
<<<<<<< HEAD
			base.OnNodeAdded(entity, prevGroup);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (Root == null)
			{
				Root = entity;
			}
		}

		public override void OnNodeRemoved<TGroupData>(MyCubeGrid entity, TGroupData nextGroup)
		{
			base.OnNodeRemoved(entity, nextGroup);
			entity.OnRemovedFromGroup(this);
			if (Root == entity)
			{
				MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(entity);
<<<<<<< HEAD
				MyCubeGrid myCubeGrid = ((group == null) ? null : group.Nodes.FirstOrDefault((MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node x) => x.NodeData != entity)?.NodeData);
=======
				MyCubeGrid myCubeGrid = ((group == null) ? null : Enumerable.FirstOrDefault<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node>((IEnumerable<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node>)group.Nodes, (Func<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node, bool>)((MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node x) => x.NodeData != entity))?.NodeData);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				ResourceDistributor.OnRootChanged(Root, myCubeGrid);
				TerminalSystem.OnRootChanged(Root, myCubeGrid);
				Root = myCubeGrid;
			}
		}

		public override void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group)
		{
			base.OnCreate(group);
		}

		internal void UpdateGridOwnership(List<MyCubeGrid> grids, long ownerID)
		{
			foreach (MyCubeGrid grid in grids)
			{
				grid.IsAccessibleForProgrammableBlock = grid.BigOwners.Contains(ownerID);
			}
		}
	}
}
