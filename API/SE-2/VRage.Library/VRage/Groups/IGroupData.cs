namespace VRage.Groups
{
	public interface IGroupData<TNode> where TNode : class
	{
		/// <summary>
		/// Group is taken from pool
		/// </summary>
		void OnCreate<TGroupData>(MyGroups<TNode, TGroupData>.Group group) where TGroupData : IGroupData<TNode>, new();

		void OnPostCreate();

		void OnPreRelease();

		/// <summary>
		/// Group is returned to pool
		/// </summary>
		void OnRelease();

		/// <summary>
		/// Node is added to group
		/// </summary>
		void OnNodeAdded<TGroupData>(TNode entity, TGroupData prevGroup) where TGroupData : IGroupData<TNode>, new();

		/// <summary>
		/// Node is added to group
		/// </summary>
		void OnPostNodeAdded<TGroupData>(TNode entity, TGroupData prevGroup) where TGroupData : IGroupData<TNode>, new();

		/// <summary>
		/// Node is removed from group
		/// </summary>
		void OnNodeRemoved<TGroupData>(TNode entity, TGroupData nextGroup) where TGroupData : IGroupData<TNode>, new();

		/// <summary>
		/// Node is removed from group
		/// </summary>
		void OnPreNodeRemoved<TGroupData>(TNode entity, TGroupData nextGroup) where TGroupData : IGroupData<TNode>, new();
	}
}
