using Sandbox.Game.Entities;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
	public class MyGridPhysicalHierarchyData : IGroupData<MyCubeGrid>
	{
		public MyCubeGrid m_root;

		private MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Group m_group;

		public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
			m_group = group as MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Group;
<<<<<<< HEAD
		}

		public void OnPostCreate()
		{
		}

		public void OnPreRelease()
		{
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void OnRelease()
		{
			m_root = null;
			m_group = null;
		}

		public void OnNodeAdded<TGroupData>(MyCubeGrid entity, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}

		public void OnPostNodeAdded<TGroupData>(MyCubeGrid entity, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}

		public void OnNodeRemoved<TGroupData>(MyCubeGrid entity, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}

		public void OnPreNodeRemoved<TGroupData>(MyCubeGrid entity, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}
	}
}
