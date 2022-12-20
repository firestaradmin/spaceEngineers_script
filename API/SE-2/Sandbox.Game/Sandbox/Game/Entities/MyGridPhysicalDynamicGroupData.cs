using VRage.Groups;

namespace Sandbox.Game.Entities
{
	public class MyGridPhysicalDynamicGroupData : IGroupData<MyCubeGrid>
	{
		public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}

		public void OnPostCreate()
		{
		}

		public void OnPreRelease()
		{
		}

		public void OnRelease()
		{
		}

		public void OnNodeAdded<TGroupData>(MyCubeGrid entity, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}

		public void OnPostNodeAdded<TGroupData>(MyCubeGrid entity, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}

		public void OnNodeRemoved<TGroupData>(MyCubeGrid entity, TGroupData nextGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}

		public void OnPreNodeRemoved<TGroupData>(MyCubeGrid entity, TGroupData nextGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}
	}
}
