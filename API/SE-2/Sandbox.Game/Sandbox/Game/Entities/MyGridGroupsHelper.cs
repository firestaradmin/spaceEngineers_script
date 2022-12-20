using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Linq;
using VRage.Game.ModAPI;

namespace Sandbox.Game.Entities
{
	public class MyGridGroupsHelper : IMyGridGroups
	{
<<<<<<< HEAD
		public event Action<IMyGridGroupData> OnGridGroupCreated
		{
			add
			{
				MyCubeGridGroups.Static.OnGridGroupCreated += value;
			}
			remove
			{
				MyCubeGridGroups.Static.OnGridGroupCreated -= value;
			}
		}

		public event Action<IMyGridGroupData> OnGridGroupDestroyed
		{
			add
			{
				MyCubeGridGroups.Static.OnGridGroupDestroyed += value;
			}
			remove
			{
				MyCubeGridGroups.Static.OnGridGroupDestroyed -= value;
			}
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Obsolete("Use GetGroup with passing your own collection, it is better for simulation speed", false)]
		public List<IMyCubeGrid> GetGroup(IMyCubeGrid node, GridLinkTypeEnum type)
		{
			return Enumerable.ToList<IMyCubeGrid>(Enumerable.Cast<IMyCubeGrid>((IEnumerable)MyCubeGridGroups.Static.GetGroups(type).GetGroupNodes((MyCubeGrid)node)));
		}

		public void GetGroup(IMyCubeGrid node, GridLinkTypeEnum type, ICollection<IMyCubeGrid> collection)
		{
			foreach (MyCubeGrid groupNode in MyCubeGridGroups.Static.GetGroups(type).GetGroupNodes((MyCubeGrid)node))
			{
				collection.Add(groupNode);
			}
		}

		[Obsolete("Use MyCubeGrid.GetConnectedGrids instead", false)]
		public void GetGroup(IMyCubeGrid node, GridLinkTypeEnum type, ICollection<IMyCubeGrid> collection)
		{
			foreach (MyCubeGrid groupNode in MyCubeGridGroups.Static.GetGroups(type).GetGroupNodes((MyCubeGrid)node))
			{
				collection.Add(groupNode);
			}
		}

		[Obsolete("Use MyCubeGrid.IsConnectedTo instead", false)]
		public bool HasConnection(IMyCubeGrid grid1, IMyCubeGrid grid2, GridLinkTypeEnum type)
		{
			return MyCubeGridGroups.Static.GetGroups(type).HasSameGroup((MyCubeGrid)grid1, (MyCubeGrid)grid2);
		}

		public IMyGridGroupData GetGridGroup(GridLinkTypeEnum linking, IMyCubeGrid grid)
		{
			return MyCubeGridGroups.GetGridGroup(linking, (MyCubeGrid)grid);
		}

		public T GetGridGroups<T>(GridLinkTypeEnum linking, T grids) where T : ICollection<IMyGridGroupData>
		{
			return MyCubeGridGroups.GetGridGroups(linking, grids);
		}

		public void AddGridGroupLogic<T>(GridLinkTypeEnum type, Func<IMyGridGroupData, T> creator) where T : MyGridGroupsDefaultEventHandler
		{
			MyCubeGridGroups.AddGridGroupLogic(type, creator);
		}
	}
}
