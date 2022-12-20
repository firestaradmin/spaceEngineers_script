<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Text;
using Sandbox.Definitions;
using VRage.Utils;

namespace Sandbox.Game.AI.BehaviorTree
{
	public class MyBehaviorTree
	{
		public class MyBehaviorTreeDesc
		{
			public List<MyBehaviorTreeNode> Nodes { get; }

			public HashSet<MyStringId> ActionIds { get; }

			public int MemorableNodesCounter { get; set; }

			public MyBehaviorTreeDesc()
			{
				Nodes = new List<MyBehaviorTreeNode>(20);
				ActionIds = new HashSet<MyStringId>((IEqualityComparer<MyStringId>)MyStringId.Comparer);
				MemorableNodesCounter = 0;
			}
		}

		private static readonly List<MyStringId> m_tmpHelper = new List<MyStringId>();

		private MyBehaviorTreeNode m_root;

		private readonly MyBehaviorTreeDesc m_treeDesc;

		public int TotalNodeCount => m_treeDesc.Nodes.Count;

		public MyBehaviorDefinition BehaviorDefinition { get; private set; }

		public string BehaviorTreeName => BehaviorDefinition.Id.SubtypeName;

		public MyStringHash BehaviorTreeId => BehaviorDefinition.Id.SubtypeId;

		public MyBehaviorTree(MyBehaviorDefinition def)
		{
			BehaviorDefinition = def;
			m_treeDesc = new MyBehaviorTreeDesc();
		}

		public void ReconstructTree(MyBehaviorDefinition def)
		{
			BehaviorDefinition = def;
			Construct();
		}

		public void Construct()
		{
			ClearData();
			m_root = new MyBehaviorTreeRoot();
			m_root.Construct(BehaviorDefinition.FirstNode, m_treeDesc);
		}

		public void ClearData()
		{
			m_treeDesc.MemorableNodesCounter = 0;
			m_treeDesc.ActionIds.Clear();
			m_treeDesc.Nodes.Clear();
		}

		public void Tick(IMyBot bot)
		{
			m_root.Tick(bot, bot.BotMemory.CurrentTreeBotMemory);
		}

		public void CallPostTickOnPath(IMyBot bot, MyPerTreeBotMemory botTreeMemory, IEnumerable<int> postTickNodes)
		{
			foreach (int postTickNode in postTickNodes)
			{
				m_treeDesc.Nodes[postTickNode].PostTick(bot, botTreeMemory);
			}
		}

		public bool IsCompatibleWithBot(ActionCollection botActions)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyStringId> enumerator = m_treeDesc.ActionIds.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyStringId current = enumerator.get_Current();
					if (!botActions.ContainsActionDesc(current))
					{
						m_tmpHelper.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			if (m_tmpHelper.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder("Error! The behavior tree is not compatible with the bot. Missing bot actions: ");
				foreach (MyStringId item in m_tmpHelper)
				{
					stringBuilder.Append(item.ToString());
					stringBuilder.Append(", ");
				}
				m_tmpHelper.Clear();
				return false;
			}
			return true;
		}

		public MyBehaviorTreeNode GetNodeByIndex(int index)
		{
			if (index >= m_treeDesc.Nodes.Count)
			{
				return null;
			}
			return m_treeDesc.Nodes[index];
		}

		public override int GetHashCode()
		{
			return m_root.GetHashCode();
		}
	}
}
