using VRage.Groups;

namespace Sandbox.Game.Entities.Cube
{
	public class MyBlockGroupData : IGroupData<MySlimBlock>
	{
		public void OnPreRelease()
		{
		}

		public void OnRelease()
		{
		}

		public void OnNodeAdded<TGroupData>(MySlimBlock entity, TGroupData prevGroup) where TGroupData : IGroupData<MySlimBlock>, new()
		{
		}

		public void OnPostNodeAdded<TGroupData>(MySlimBlock entity, TGroupData prevGroup) where TGroupData : IGroupData<MySlimBlock>, new()
		{
		}

		public void OnNodeRemoved<TGroupData>(MySlimBlock entity, TGroupData nextGroup) where TGroupData : IGroupData<MySlimBlock>, new()
		{
		}

		public void OnPreNodeRemoved<TGroupData>(MySlimBlock entity, TGroupData nextGroup) where TGroupData : IGroupData<MySlimBlock>, new()
		{
		}

		public void OnCreate<TGroupData>(MyGroups<MySlimBlock, TGroupData>.Group group) where TGroupData : IGroupData<MySlimBlock>, new()
		{
		}

		public void OnPostCreate()
		{
		}
	}
}
