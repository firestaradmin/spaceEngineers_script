using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Collections;

namespace VRage.Groups
{
	public class MyGroups<TNode, TGroupData> : MyGroupsBase<TNode> where TNode : class where TGroupData : IGroupData<TNode>, new()
	{
		/// <summary>
		/// Return true when "major" is really major group, otherwise false.
		/// </summary>
		public delegate bool MajorGroupComparer(Group major, Group minor);

		public class Node
		{
			private Group m_currentGroup;

			internal TNode m_node;

			internal readonly SortedDictionary<long, Node> m_children = (SortedDictionary<long, Node>)(object)new SortedDictionary<long, MyGroups<long, Node>.Node>();

			internal readonly Dictionary<long, Node> m_parents = new Dictionary<long, Node>();

			internal Group m_group
			{
				get
				{
					return m_currentGroup;
				}
				set
				{
					Group currentGroup = m_currentGroup;
					if (currentGroup != null)
					{
						TGroupData nextGroup = ((value != null) ? value.GroupData : default(TGroupData));
						TGroupData groupData = currentGroup.GroupData;
						groupData.OnPreNodeRemoved(m_node, nextGroup);
						groupData = currentGroup.GroupData;
						groupData.OnNodeRemoved(m_node, nextGroup);
					}
					m_currentGroup = value;
					if (m_currentGroup != null)
					{
						TGroupData prevGroup = ((currentGroup != null) ? currentGroup.GroupData : default(TGroupData));
						TGroupData groupData = m_currentGroup.GroupData;
						groupData.OnNodeAdded(m_node, prevGroup);
						groupData = m_currentGroup.GroupData;
						groupData.OnPostNodeAdded(m_node, prevGroup);
					}
				}
			}

			public int LinkCount => ((SortedDictionary<long, MyGroups<long, Node>.Node>)(object)m_children).get_Count() + m_parents.Count;

			public TNode NodeData => m_node;

			public Group Group => m_group;

			public int ChainLength { get; set; }

			public SortedDictionaryValuesReader<long, Node> Children => new SortedDictionaryValuesReader<long, Node>(m_children);

			public SortedDictionaryReader<long, Node> ChildLinks => new SortedDictionaryReader<long, Node>(m_children);

			public DictionaryReader<long, Node> ParentLinks => new DictionaryReader<long, Node>(m_parents);

			public override string ToString()
			{
				return m_node.ToString();
			}
		}

		public class Group
		{
			internal readonly HashSet<Node> m_members = (HashSet<Node>)(object)new HashSet<MyGroups<Node, _003F>.Node>();

			public readonly TGroupData GroupData = new TGroupData();

			public HashSetReader<Node> Nodes => new HashSetReader<Node>(m_members);
		}

		private Stack<Group> m_groupPool = (Stack<Group>)(object)new Stack<MyGroups<Group, _003F>.Group>(32);

		private Stack<Node> m_nodePool = (Stack<Node>)(object)new Stack<MyGroups<Node, _003F>.Node>(32);

		private Dictionary<TNode, Node> m_nodes = new Dictionary<TNode, Node>();

		private HashSet<Group> m_groups = (HashSet<Group>)(object)new HashSet<MyGroups<Group, _003F>.Group>();

		private HashSet<Node> m_disconnectHelper = (HashSet<Node>)(object)new HashSet<MyGroups<Node, _003F>.Node>();

		private MajorGroupComparer m_groupSelector;

		private bool m_isRecalculating;

		private HashSet<Node> m_tmpClosed = (HashSet<Node>)(object)new HashSet<MyGroups<Node, _003F>.Node>();

		private Queue<Node> m_tmpOpen = (Queue<Node>)(object)new Queue<MyGroups<Node, _003F>.Node>();

		private List<Node> m_tmpList = new List<Node>();

		/// <summary>
		/// When true, groups with one member are supported.
		/// You can use AddNode and RemoveNode.
		/// You have to manually call RemoveNode!
		/// </summary>
		public bool SupportsOphrans { get; protected set; }

		protected bool SupportsChildToChild { get; set; }

		public HashSetReader<Group> Groups => new HashSetReader<Group>(m_groups);

		/// <summary>
		/// Initializes a new instance of MyGroups class.
		/// </summary>
		/// <param name="supportOphrans">When true, groups with one member are supported and you have to manually call RemoveNode!</param>
		/// <param name="groupSelector">Major group selector, when merging two groups, major group is preserved. By default it's larger group.</param>
		public MyGroups(bool supportOphrans = false, MajorGroupComparer groupSelector = null)
		{
			SupportsOphrans = supportOphrans;
			m_groupSelector = groupSelector ?? new MajorGroupComparer(IsMajorGroup);
		}

		public void ApplyOnNodes(Action<TNode, Node> action)
		{
			foreach (KeyValuePair<TNode, Node> node in m_nodes)
			{
				action(node.Key, node.Value);
			}
		}

		public override bool HasSameGroup(TNode a, TNode b)
		{
			Group group = GetGroup(a);
			Group group2 = GetGroup(b);
			if (group != null)
			{
				return group == group2;
			}
			return false;
		}

		public Group GetGroup(TNode Node)
		{
			if (m_nodes.TryGetValue(Node, out var value))
			{
				return value.m_group;
			}
			return null;
		}

		/// <summary>
		/// Adds node, asserts when node already exists
		/// </summary>
		public override void AddNode(TNode nodeToAdd)
		{
			if (!SupportsOphrans)
			{
				throw new InvalidOperationException("Cannot add/remove node when ophrans are not supported");
			}
			Node orCreateNode = GetOrCreateNode(nodeToAdd);
			if (orCreateNode.m_group == null)
			{
				orCreateNode.m_group = AcquireGroup();
				((HashSet<MyGroups<Node, _003F>.Node>)(object)orCreateNode.m_group.m_members).Add((MyGroups<Node, _003F>.Node)(object)orCreateNode);
			}
		}

		/// <summary>
		/// Removes node, asserts when node is not here or node has some existing links
		/// </summary>
		public override void RemoveNode(TNode nodeToRemove)
		{
			if (!SupportsOphrans)
			{
				throw new InvalidOperationException("Cannot add/remove node when ophrans are not supported");
			}
			if (m_nodes.TryGetValue(nodeToRemove, out var value))
			{
				BreakAllLinks(value);
				bool flag = TryReleaseNode(value);
			}
		}

		private unsafe void BreakAllLinks(Node node)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			while (node.m_parents.Count > 0)
			{
				Dictionary<long, Node>.Enumerator enumerator = node.m_parents.GetEnumerator();
				enumerator.MoveNext();
				KeyValuePair<long, Node> current = enumerator.Current;
				BreakLinkInternal(current.Key, current.Value, node);
			}
			while (((SortedDictionary<long, MyGroups<long, Node>.Node>)(object)node.m_children).get_Count() > 0)
			{
				Enumerator<long, Node> enumerator2 = ((SortedDictionary<long, MyGroups<long, Node>.Node>)(object)node.m_children).GetEnumerator();
				((Enumerator<long, MyGroups<long, Node>.Node>*)(&enumerator2))->MoveNext();
				KeyValuePair<long, Node> current2 = ((Enumerator<long, MyGroups<long, Node>.Node>*)(&enumerator2))->get_Current();
				BreakLinkInternal(current2.Key, node, current2.Value);
			}
		}

		/// <summary>
		/// Creates link between parent and child.
		/// Parent is owner of constraint.
		/// LinkId must be unique for parent and for child; LinkId is unique node-node identifier.
		/// </summary>
		public override void CreateLink(long linkId, TNode parentNode, TNode childNode)
		{
			Node orCreateNode = GetOrCreateNode(parentNode);
			Node orCreateNode2 = GetOrCreateNode(childNode);
			if (orCreateNode.m_group != null && orCreateNode2.m_group != null)
			{
				if (orCreateNode.m_group == orCreateNode2.m_group)
				{
					AddLink(linkId, orCreateNode, orCreateNode2);
					return;
				}
				MergeGroups(orCreateNode.m_group, orCreateNode2.m_group);
				AddLink(linkId, orCreateNode, orCreateNode2);
			}
			else if (orCreateNode.m_group != null)
			{
				orCreateNode2.m_group = orCreateNode.m_group;
				((HashSet<MyGroups<Node, _003F>.Node>)(object)orCreateNode.m_group.m_members).Add((MyGroups<Node, _003F>.Node)(object)orCreateNode2);
				AddLink(linkId, orCreateNode, orCreateNode2);
			}
			else if (orCreateNode2.m_group != null)
			{
				orCreateNode.m_group = orCreateNode2.m_group;
				((HashSet<MyGroups<Node, _003F>.Node>)(object)orCreateNode2.m_group.m_members).Add((MyGroups<Node, _003F>.Node)(object)orCreateNode);
				AddLink(linkId, orCreateNode, orCreateNode2);
			}
			else
			{
				Group group2 = (orCreateNode.m_group = AcquireGroup());
<<<<<<< HEAD
				group2.m_members.Add(orCreateNode);
=======
				((HashSet<MyGroups<Node, _003F>.Node>)(object)group2.m_members).Add((MyGroups<Node, _003F>.Node)(object)orCreateNode);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				orCreateNode2.m_group = group2;
				((HashSet<MyGroups<Node, _003F>.Node>)(object)group2.m_members).Add((MyGroups<Node, _003F>.Node)(object)orCreateNode2);
				AddLink(linkId, orCreateNode, orCreateNode2);
			}
		}

		/// <summary>
		/// Breaks link between parent and child, you can set child to null to find it by linkId.
		/// Returns true when link was removed, returns false when link was not found.
		/// </summary>
		public unsafe override bool BreakLink(long linkId, TNode parentNode, TNode childNode = null)
		{
<<<<<<< HEAD
			if (m_nodes.TryGetValue(parentNode, out var value) && value.m_children.TryGetValue(linkId, out var value2))
=======
			Node child = default(Node);
			if (m_nodes.TryGetValue(parentNode, out var value) && ((SortedDictionary<long, MyGroups<long, Node>.Node>)(object)value.m_children).TryGetValue(linkId, ref *(MyGroups<long, Node>.Node*)(&child)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return BreakLinkInternal(linkId, value, child);
			}
			return false;
		}

		public void BreakAllLinks(TNode node)
		{
			if (m_nodes.TryGetValue(node, out var value))
			{
				BreakAllLinks(value);
			}
		}

		public Node GetNode(TNode node)
		{
			return m_nodes.GetValueOrDefault(node);
		}

		public unsafe override bool LinkExists(long linkId, TNode parentNode, TNode childNode = null)
		{
<<<<<<< HEAD
			if (m_nodes.TryGetValue(parentNode, out var value) && value.m_children.TryGetValue(linkId, out var value2))
=======
			Node node = default(Node);
			if (m_nodes.TryGetValue(parentNode, out var value) && ((SortedDictionary<long, MyGroups<long, Node>.Node>)(object)value.m_children).TryGetValue(linkId, ref *(MyGroups<long, Node>.Node*)(&node)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (childNode == null)
				{
					return true;
				}
				return childNode == node.m_node;
			}
			return false;
		}

		private bool BreakLinkInternal(long linkId, Node parent, Node child)
		{
			bool flag = ((SortedDictionary<long, MyGroups<long, Node>.Node>)(object)parent.m_children).Remove(linkId);
			flag &= child.m_parents.Remove(linkId);
			if (!flag && SupportsChildToChild)
			{
				flag &= ((SortedDictionary<long, MyGroups<long, Node>.Node>)(object)child.m_children).Remove(linkId);
			}
			RecalculateConnectivity(parent, child);
			return flag;
		}

		[Conditional("DEBUG")]
		private void DebugCheckConsistency(long linkId, Node parent, Node child)
		{
		}

		private unsafe void AddNeighbours(HashSet<Node> nodes, Node nodeToAdd)
		{
<<<<<<< HEAD
			if (nodes.Contains(nodeToAdd))
			{
				return;
			}
			nodes.Add(nodeToAdd);
			foreach (KeyValuePair<long, Node> child in nodeToAdd.m_children)
			{
				AddNeighbours(nodes, child.Value);
			}
			foreach (KeyValuePair<long, Node> parent in nodeToAdd.m_parents)
			{
				AddNeighbours(nodes, parent.Value);
=======
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (((HashSet<MyGroups<Node, _003F>.Node>)(object)nodes).Contains((MyGroups<Node, _003F>.Node)(object)nodeToAdd))
			{
				return;
			}
			((HashSet<MyGroups<Node, _003F>.Node>)(object)nodes).Add((MyGroups<Node, _003F>.Node)(object)nodeToAdd);
			Enumerator<long, Node> enumerator = ((SortedDictionary<long, MyGroups<long, Node>.Node>)(object)nodeToAdd.m_children).GetEnumerator();
			try
			{
				while (((Enumerator<long, MyGroups<long, Node>.Node>*)(&enumerator))->MoveNext())
				{
					AddNeighbours(nodes, ((Enumerator<long, MyGroups<long, Node>.Node>*)(&enumerator))->get_Current().Value);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			foreach (KeyValuePair<long, Node> parent in nodeToAdd.m_parents)
			{
				AddNeighbours(nodes, parent.Value);
			}
		}

		/// <summary>
		/// Returns true when node was released completely and returned to pool.
		/// </summary>
		private bool TryReleaseNode(Node node)
		{
			if (node.m_node != null && node.m_group != null && ((SortedDictionary<long, MyGroups<long, Node>.Node>)(object)node.m_children).get_Count() == 0 && node.m_parents.Count == 0)
			{
				Group group = node.m_group;
				((HashSet<MyGroups<Node, _003F>.Node>)(object)node.m_group.m_members).Remove((MyGroups<Node, _003F>.Node)(object)node);
				m_nodes.Remove(node.m_node);
				node.m_group = null;
				node.m_node = null;
				ReturnNode(node);
				if (((HashSet<MyGroups<Node, _003F>.Node>)(object)group.m_members).get_Count() == 0)
				{
					ReturnGroup(group);
				}
				return true;
			}
			return false;
		}

		private unsafe void RecalculateConnectivity(Node parent, Node child)
		{
<<<<<<< HEAD
			if (m_isRecalculating || parent == null || parent.Group == null || child == null || child.Group == null)
			{
				return;
			}
			try
			{
				m_isRecalculating = true;
				if (!SupportsOphrans && !(!TryReleaseNode(parent) & !TryReleaseNode(child)))
				{
					return;
				}
				AddNeighbours(m_disconnectHelper, parent);
				if (m_disconnectHelper.Contains(child))
				{
					return;
				}
				if ((float)m_disconnectHelper.Count > (float)parent.Group.m_members.Count / 2f)
				{
					foreach (Node member in parent.Group.m_members)
					{
						if (!m_disconnectHelper.Add(member))
						{
							m_disconnectHelper.Remove(member);
=======
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			if (m_isRecalculating || parent == null || parent.Group == null || child == null || child.Group == null)
			{
				return;
			}
			try
			{
				m_isRecalculating = true;
				if (!SupportsOphrans && !(!TryReleaseNode(parent) & !TryReleaseNode(child)))
				{
					return;
				}
				AddNeighbours(m_disconnectHelper, parent);
				if (((HashSet<MyGroups<Node, _003F>.Node>)(object)m_disconnectHelper).Contains((MyGroups<Node, _003F>.Node)(object)child))
				{
					return;
				}
				if ((float)((HashSet<MyGroups<Node, _003F>.Node>)(object)m_disconnectHelper).get_Count() > (float)((HashSet<MyGroups<Node, _003F>.Node>)(object)parent.Group.m_members).get_Count() / 2f)
				{
					Enumerator<Node> enumerator = ((HashSet<MyGroups<Node, _003F>.Node>)(object)parent.Group.m_members).GetEnumerator();
					try
					{
						while (((Enumerator<MyGroups<Node, _003F>.Node>*)(&enumerator))->MoveNext())
						{
							Node current = ((Enumerator<MyGroups<Node, _003F>.Node>*)(&enumerator))->get_Current();
							if (!((HashSet<MyGroups<Node, _003F>.Node>)(object)m_disconnectHelper).Add((MyGroups<Node, _003F>.Node)(object)current))
							{
								((HashSet<MyGroups<Node, _003F>.Node>)(object)m_disconnectHelper).Remove((MyGroups<Node, _003F>.Node)(object)current);
							}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				Group group = AcquireGroup();
				Enumerator<Node> enumerator2 = ((HashSet<MyGroups<Node, _003F>.Node>)(object)m_disconnectHelper).GetEnumerator();
				try
				{
					while (((Enumerator<MyGroups<Node, _003F>.Node>*)(&enumerator2))->MoveNext())
					{
						Node current2 = ((Enumerator<MyGroups<Node, _003F>.Node>*)(&enumerator2))->get_Current();
						if (current2.m_group != null && current2.m_group.m_members != null)
						{
							bool flag = ((HashSet<MyGroups<Node, _003F>.Node>)(object)current2.m_group.m_members).Remove((MyGroups<Node, _003F>.Node)(object)current2);
							current2.m_group = group;
							((HashSet<MyGroups<Node, _003F>.Node>)(object)group.m_members).Add((MyGroups<Node, _003F>.Node)(object)current2);
						}
					}
				}
				Group group = AcquireGroup();
				foreach (Node item in m_disconnectHelper)
				{
<<<<<<< HEAD
					if (item.m_group != null && item.m_group.m_members != null)
					{
						bool flag = item.m_group.m_members.Remove(item);
						item.m_group = group;
						group.m_members.Add(item);
					}
=======
					((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
<<<<<<< HEAD
				m_disconnectHelper.Clear();
=======
				((HashSet<MyGroups<Node, _003F>.Node>)(object)m_disconnectHelper).Clear();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_isRecalculating = false;
			}
		}

		public static bool IsMajorGroup(Group groupA, Group groupB)
		{
			return ((HashSet<MyGroups<Node, _003F>.Node>)(object)groupA.m_members).get_Count() >= ((HashSet<MyGroups<Node, _003F>.Node>)(object)groupB.m_members).get_Count();
		}

		private void MergeGroups(Group groupA, Group groupB)
		{
			if (!m_groupSelector(groupA, groupB))
			{
				Group group = groupA;
				groupA = groupB;
				groupB = group;
			}
			if (m_tmpList.Capacity < ((HashSet<MyGroups<Node, _003F>.Node>)(object)groupB.m_members).get_Count())
			{
				m_tmpList.Capacity = ((HashSet<MyGroups<Node, _003F>.Node>)(object)groupB.m_members).get_Count();
			}
			m_tmpList.AddRange((IEnumerable<Node>)groupB.m_members);
			foreach (Node tmp in m_tmpList)
			{
				((HashSet<MyGroups<Node, _003F>.Node>)(object)groupB.m_members).Remove((MyGroups<Node, _003F>.Node)(object)tmp);
				tmp.m_group = groupA;
				((HashSet<MyGroups<Node, _003F>.Node>)(object)groupA.m_members).Add((MyGroups<Node, _003F>.Node)(object)tmp);
			}
			m_tmpList.Clear();
			((HashSet<MyGroups<Node, _003F>.Node>)(object)groupB.m_members).Clear();
			ReturnGroup(groupB);
		}

		private void AddLink(long linkId, Node parent, Node child)
		{
			((SortedDictionary<long, MyGroups<long, Node>.Node>)(object)parent.m_children).set_Item(linkId, (MyGroups<long, Node>.Node)(object)child);
			child.m_parents[linkId] = parent;
		}

		private Node GetOrCreateNode(TNode nodeData)
		{
			if (!m_nodes.TryGetValue(nodeData, out var value))
			{
				value = AcquireNode();
				value.m_node = nodeData;
				m_nodes[nodeData] = value;
			}
			return value;
		}

		private Group GetNodeOrNull(TNode Node)
		{
			m_nodes.TryGetValue(Node, out var value);
			return value?.m_group;
		}

		private Group AcquireGroup()
		{
<<<<<<< HEAD
			Group group = ((m_groupPool.Count > 0) ? m_groupPool.Pop() : new Group());
			m_groups.Add(group);
=======
			Group group = ((((Stack<MyGroups<Group, _003F>.Group>)(object)m_groupPool).get_Count() > 0) ? ((Stack<MyGroups<Group, _003F>.Group>)(object)m_groupPool).Pop() : new Group());
			((HashSet<MyGroups<Group, _003F>.Group>)(object)m_groups).Add((MyGroups<Group, _003F>.Group)(object)group);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TGroupData groupData = group.GroupData;
			groupData.OnCreate(group);
			groupData = group.GroupData;
			groupData.OnPostCreate();
			return group;
		}

		private void ReturnGroup(Group group)
		{
			TGroupData groupData = group.GroupData;
			groupData.OnPreRelease();
			groupData = group.GroupData;
			groupData.OnRelease();
			((HashSet<MyGroups<Group, _003F>.Group>)(object)m_groups).Remove((MyGroups<Group, _003F>.Group)(object)group);
			((Stack<MyGroups<Group, _003F>.Group>)(object)m_groupPool).Push((MyGroups<Group, _003F>.Group)(object)group);
		}

		private Node AcquireNode()
		{
			return (((Stack<MyGroups<Node, _003F>.Node>)(object)m_nodePool).get_Count() > 0) ? ((Stack<MyGroups<Node, _003F>.Node>)(object)m_nodePool).Pop() : new Node();
		}

		private void ReturnNode(Node node)
		{
			((Stack<MyGroups<Node, _003F>.Node>)(object)m_nodePool).Push((MyGroups<Node, _003F>.Node)(object)node);
		}

		public unsafe override List<TNode> GetGroupNodes(TNode nodeInGroup)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			List<TNode> list = null;
			Group group = GetGroup(nodeInGroup);
			if (group != null)
			{
				list = new List<TNode>(group.Nodes.Count);
				Enumerator<Node> enumerator = group.Nodes.GetEnumerator();
				try
				{
					while (((Enumerator<MyGroups<Node, _003F>.Node>*)(&enumerator))->MoveNext())
					{
						Node current = ((Enumerator<MyGroups<Node, _003F>.Node>*)(&enumerator))->get_Current();
						list.Add(current.NodeData);
					}
					return list;
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			list = new List<TNode>(1);
			list.Add(nodeInGroup);
			return list;
		}

		public unsafe override void GetGroupNodes(TNode nodeInGroup, List<TNode> result)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			Group group = GetGroup(nodeInGroup);
			if (group != null)
			{
				Enumerator<Node> enumerator = group.Nodes.GetEnumerator();
				try
				{
					while (((Enumerator<MyGroups<Node, _003F>.Node>*)(&enumerator))->MoveNext())
					{
						Node current = ((Enumerator<MyGroups<Node, _003F>.Node>*)(&enumerator))->get_Current();
						result.Add(current.NodeData);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			else
			{
				result.Add(nodeInGroup);
			}
		}

		public unsafe void ReplaceRoot(TNode newRoot)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Group group = GetGroup(newRoot);
			Enumerator<Node> enumerator = ((HashSet<MyGroups<Node, _003F>.Node>)(object)group.m_members).GetEnumerator();
			try
			{
				while (((Enumerator<MyGroups<Node, _003F>.Node>*)(&enumerator))->MoveNext())
				{
					Node current = ((Enumerator<MyGroups<Node, _003F>.Node>*)(&enumerator))->get_Current();
					foreach (KeyValuePair<long, Node> parent in current.m_parents)
					{
						((SortedDictionary<long, MyGroups<long, Node>.Node>)(object)current.m_children).set_Item(parent.Key, (MyGroups<long, Node>.Node)(object)parent.Value);
					}
					current.m_parents.Clear();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			Node node = GetNode(newRoot);
			node.ChainLength = 0;
			ReplaceParents(node);
		}

		private unsafe void ReplaceParents(Node newParent)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			((Queue<MyGroups<Node, _003F>.Node>)(object)m_tmpOpen).Enqueue((MyGroups<Node, _003F>.Node)(object)newParent);
			((HashSet<MyGroups<Node, _003F>.Node>)(object)m_tmpClosed).Add((MyGroups<Node, _003F>.Node)(object)newParent);
			while (((Queue<MyGroups<Node, _003F>.Node>)(object)m_tmpOpen).get_Count() > 0)
			{
				Node node = ((Queue<MyGroups<Node, _003F>.Node>)(object)m_tmpOpen).Dequeue();
				Enumerator<long, Node> enumerator = ((SortedDictionary<long, MyGroups<long, Node>.Node>)(object)node.m_children).GetEnumerator();
				try
				{
					while (((Enumerator<long, MyGroups<long, Node>.Node>*)(&enumerator))->MoveNext())
					{
						KeyValuePair<long, Node> current = ((Enumerator<long, MyGroups<long, Node>.Node>*)(&enumerator))->get_Current();
						current.Value.ChainLength = node.ChainLength + 1;
						if (!((HashSet<MyGroups<Node, _003F>.Node>)(object)m_tmpClosed).Contains((MyGroups<Node, _003F>.Node)(object)current.Value) && !current.Value.m_parents.ContainsKey(current.Key))
						{
							current.Value.m_parents.Add(current.Key, node);
							((SortedDictionary<long, MyGroups<long, Node>.Node>)(object)current.Value.m_children).Remove(current.Key);
							((Queue<MyGroups<Node, _003F>.Node>)(object)m_tmpOpen).Enqueue((MyGroups<Node, _003F>.Node)(object)current.Value);
							((HashSet<MyGroups<Node, _003F>.Node>)(object)m_tmpClosed).Add((MyGroups<Node, _003F>.Node)(object)current.Value);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			((Queue<MyGroups<Node, _003F>.Node>)(object)m_tmpOpen).Clear();
			((HashSet<MyGroups<Node, _003F>.Node>)(object)m_tmpClosed).Clear();
		}
	}
}
