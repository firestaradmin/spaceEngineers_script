using System;
using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRage.Groups;

namespace Sandbox.Game.Entities.Cube
{
	public class MyGridGroupData<T> : IGroupData<MyCubeGrid>, IMyGridGroupData where T : IGroupData<MyCubeGrid>, new()
	{
		protected MyGroups<MyCubeGrid, T>.Group m_group;

		private readonly Dictionary<Guid, object> m_variables = new Dictionary<Guid, object>();

		public GridLinkTypeEnum LinkType { get; protected set; }

		public event Action<IMyGridGroupData, IMyCubeGrid, IMyGridGroupData> OnGridAdded;

		public event Action<IMyGridGroupData, IMyCubeGrid, IMyGridGroupData> OnGridRemoved;

		public event Action<IMyGridGroupData> OnReleased;

		public TC GetGrids<TC>(TC grids) where TC : ICollection<IMyCubeGrid>
		{
			foreach (MyGroups<MyCubeGrid, T>.Node node in m_group.Nodes)
			{
				grids.Add(node.m_node);
			}
			return grids;
		}

		public bool TryGetVariable<TK>(Guid key, out TK variable)
		{
			object obj;
			if (m_variables.TryGetValue(key, out var value) && (obj = value) is TK)
			{
				TK val = (variable = (TK)obj);
				return true;
			}
			variable = default(TK);
			return false;
		}

		public TK GetVariable<TK>(Guid key)
		{
			object obj;
			if (m_variables.TryGetValue(key, out var value) && (obj = value) is TK)
			{
				return (TK)obj;
			}
			return default(TK);
		}

		public void SetVariable(Guid key, object data)
		{
			m_variables[key] = data;
		}

		public bool RemoveVariable(Guid key)
		{
			return m_variables.Remove(key);
		}

		public virtual void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
			m_group = group as MyGroups<MyCubeGrid, T>.Group;
		}

		public void OnPostCreate()
		{
			MyCubeGridGroups.RaiseOnGridGroupCreated(this);
		}

		public void OnPreRelease()
		{
			this.OnReleased?.Invoke(this);
			MyCubeGridGroups.RaiseOnGridGroupDestroyed(this);
		}

		public virtual void OnRelease()
		{
			m_group = null;
			m_variables.Clear();
		}

		public virtual void OnNodeAdded<TGroupData>(MyCubeGrid entity, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
			entity.OnConnectivityChanged(LinkType);
		}

		public void OnPostNodeAdded<TGroupData>(MyCubeGrid entity, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
			this.OnGridAdded?.Invoke(this, entity, prevGroup as IMyGridGroupData);
		}

		public virtual void OnNodeRemoved<TGroupData>(MyCubeGrid entity, TGroupData nextGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}

		public void OnPreNodeRemoved<TGroupData>(MyCubeGrid entity, TGroupData nextGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
			this.OnGridRemoved?.Invoke(this, entity, nextGroup as IMyGridGroupData);
		}
	}
}
