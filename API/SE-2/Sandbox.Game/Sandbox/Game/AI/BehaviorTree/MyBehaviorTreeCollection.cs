using System;
using System.Collections.Generic;
using System.IO;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Debugging;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.SessionComponents;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.AI.BehaviorTree
{
	public class MyBehaviorTreeCollection
	{
		private class BotData
		{
			public IMyBot Bot;

			public int UpdateCounter = 8;

			public BotData(IMyBot bot)
			{
				Bot = bot;
			}
		}

		private class BTData : IEqualityComparer<BotData>
		{
			private static readonly BotData m_searchData = new BotData(null);

			public readonly MyBehaviorTree BehaviorTree;

			public readonly HashSet<BotData> BotsData;

			public BTData(MyBehaviorTree behaviorTree)
			{
				BehaviorTree = behaviorTree;
				BotsData = new HashSet<BotData>((IEqualityComparer<BotData>)this);
			}

			public void RemoveBot(IMyBot bot)
			{
				m_searchData.Bot = bot;
				BotsData.Remove(m_searchData);
			}

			public bool ContainsBot(IMyBot bot)
			{
				m_searchData.Bot = bot;
				return BotsData.Contains(m_searchData);
			}

			bool IEqualityComparer<BotData>.Equals(BotData x, BotData y)
			{
				return x.Bot == y.Bot;
			}

			int IEqualityComparer<BotData>.GetHashCode(BotData obj)
			{
				return obj.Bot.GetHashCode();
			}
		}

		private IntPtr m_toolWindowHandle = IntPtr.Zero;

		public const int UPDATE_COUNTER = 10;

		public const int INIT_UPDATE_COUNTER = 8;

		public static readonly string DEFAULT_EXTENSION = ".sbc";

		private readonly Dictionary<MyStringHash, BTData> m_BTDataByName;

		private readonly Dictionary<IMyBot, MyStringHash> m_botBehaviorIds;

		private IMyBot m_debugBot;

		public bool DebugSelectedTreeHashSent { get; private set; }

		public IntPtr DebugLastWindowHandle { get; private set; }

		public bool DebugIsCurrentTreeVerified { get; private set; }

		public IMyBot DebugBot
		{
			get
			{
				return m_debugBot;
			}
			set
			{
				m_debugBot = value;
				DebugSelectedTreeHashSent = false;
			}
		}

		public bool DebugBreakDebugging { get; set; }

		public string DebugCurrentBehaviorTree { get; private set; }

		public bool TryGetValidToolWindow(out IntPtr windowHandle)
		{
			windowHandle = IntPtr.Zero;
			windowHandle = MyVRage.Platform.Windows.FindWindowInParent("VRageEditor", "BehaviorTreeWindow");
			if (windowHandle == IntPtr.Zero)
			{
				windowHandle = MyVRage.Platform.Windows.FindWindowInParent("Behavior tree tool", "BehaviorTreeWindow");
			}
			return windowHandle != IntPtr.Zero;
		}

		private void SendSelectedTreeForDebug(MyBehaviorTree behaviorTree)
		{
			if (MySessionComponentExtDebug.Static != null)
			{
				DebugSelectedTreeHashSent = true;
				DebugCurrentBehaviorTree = behaviorTree.BehaviorTreeName;
				SelectedTreeMsg selectedTreeMsg = default(SelectedTreeMsg);
				selectedTreeMsg.BehaviorTreeName = behaviorTree.BehaviorTreeName;
				SelectedTreeMsg msg = selectedTreeMsg;
				MySessionComponentExtDebug.Static.SendMessageToClients(msg);
			}
		}

		private void SendDataToTool(IMyBot bot, MyPerTreeBotMemory botTreeMemory)
		{
			if (!DebugIsCurrentTreeVerified || DebugLastWindowHandle.ToInt32() != m_toolWindowHandle.ToInt32())
			{
				int hashCode = m_BTDataByName[m_botBehaviorIds[bot]].BehaviorTree.GetHashCode();
				IntPtr wParam = new IntPtr(hashCode);
				MyVRage.Platform.Windows.PostMessage(m_toolWindowHandle, 1027u, wParam, IntPtr.Zero);
				DebugIsCurrentTreeVerified = true;
				DebugLastWindowHandle = new IntPtr(m_toolWindowHandle.ToInt32());
			}
			MyVRage.Platform.Windows.PostMessage(m_toolWindowHandle, 1025u, IntPtr.Zero, IntPtr.Zero);
			for (int i = 0; i < botTreeMemory.NodesMemoryCount; i++)
			{
				MyBehaviorTreeState nodeState = botTreeMemory.GetNodeMemoryByIndex(i).NodeState;
				if (nodeState != 0)
				{
					MyVRage.Platform.Windows.PostMessage(m_toolWindowHandle, 1024u, new IntPtr((uint)i), new IntPtr((int)nodeState));
				}
			}
			MyVRage.Platform.Windows.PostMessage(m_toolWindowHandle, 1026u, IntPtr.Zero, IntPtr.Zero);
		}

		public MyBehaviorTreeCollection()
		{
			m_BTDataByName = new Dictionary<MyStringHash, BTData>(MyStringHash.Comparer);
			m_botBehaviorIds = new Dictionary<IMyBot, MyStringHash>();
			DebugIsCurrentTreeVerified = false;
			foreach (MyBehaviorDefinition behaviorDefinition in MyDefinitionManager.Static.GetBehaviorDefinitions())
			{
				BuildBehaviorTree(behaviorDefinition);
			}
		}

		public void Update()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			foreach (BTData value in m_BTDataByName.Values)
			{
				MyBehaviorTree behaviorTree = value.BehaviorTree;
				Enumerator<BotData> enumerator2 = value.BotsData.GetEnumerator();
				try
				{
<<<<<<< HEAD
					IMyBot bot = botsDatum.Bot;
					if (!bot.IsValidForUpdate || ++botsDatum.UpdateCounter <= 10)
					{
						continue;
					}
					if (MyFakes.DEBUG_BEHAVIOR_TREE)
					{
						if (!MyFakes.DEBUG_BEHAVIOR_TREE_ONE_STEP)
=======
					while (enumerator2.MoveNext())
					{
						BotData current2 = enumerator2.get_Current();
						IMyBot bot = current2.Bot;
						if (!bot.IsValidForUpdate || ++current2.UpdateCounter <= 10)
						{
							continue;
						}
						if (MyFakes.DEBUG_BEHAVIOR_TREE)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							continue;
						}
<<<<<<< HEAD
						MyFakes.DEBUG_BEHAVIOR_TREE_ONE_STEP = false;
					}
					botsDatum.UpdateCounter = 0;
					bot.BotMemory.PreTickClear();
					try
					{
						behaviorTree.Tick(bot);
					}
					catch (ArgumentException arg)
					{
						MyLog.Default.WriteLine($"AI BehaviorTree Id: {behaviorTree.BehaviorTreeId}, name: {behaviorTree.BehaviorTreeName} caused ArgumentException: {arg}");
						throw;
					}
					if (MyPlatformGameSettings.ENABLE_BEHAVIOR_TREE_TOOL_COMMUNICATION && DebugBot == botsDatum.Bot && !DebugBreakDebugging && MyDebugDrawSettings.DEBUG_DRAW_BOTS && TryGetValidToolWindow(out m_toolWindowHandle))
					{
						if (!DebugSelectedTreeHashSent || m_toolWindowHandle != DebugLastWindowHandle || DebugCurrentBehaviorTree != m_botBehaviorIds[DebugBot].String)
						{
							SendSelectedTreeForDebug(behaviorTree);
=======
						current2.UpdateCounter = 0;
						bot.BotMemory.PreTickClear();
						try
						{
							behaviorTree.Tick(bot);
						}
						catch (ArgumentException arg)
						{
							MyLog.Default.WriteLine($"AI BehaviorTree Id: {behaviorTree.BehaviorTreeId}, name: {behaviorTree.BehaviorTreeName} caused ArgumentException: {arg}");
							throw;
						}
						if (MyPlatformGameSettings.ENABLE_BEHAVIOR_TREE_TOOL_COMMUNICATION && DebugBot == current2.Bot && !DebugBreakDebugging && MyDebugDrawSettings.DEBUG_DRAW_BOTS && TryGetValidToolWindow(out m_toolWindowHandle))
						{
							if (!DebugSelectedTreeHashSent || m_toolWindowHandle != DebugLastWindowHandle || DebugCurrentBehaviorTree != m_botBehaviorIds[DebugBot].String)
							{
								SendSelectedTreeForDebug(behaviorTree);
							}
							SendDataToTool(current2.Bot, current2.Bot.BotMemory.CurrentTreeBotMemory);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						SendDataToTool(botsDatum.Bot, botsDatum.Bot.BotMemory.CurrentTreeBotMemory);
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
			}
		}

		public bool AssignBotToBehaviorTree(string behaviorName, IMyBot bot)
		{
			MyStringHash myStringHash = MyStringHash.TryGet(behaviorName);
			if (myStringHash == MyStringHash.NullOrEmpty || !m_BTDataByName.ContainsKey(myStringHash))
			{
				return false;
			}
			return AssignBotToBehaviorTree(m_BTDataByName[myStringHash].BehaviorTree, bot);
		}

		public bool AssignBotToBehaviorTree(MyBehaviorTree behaviorTree, IMyBot bot)
		{
			if (!behaviorTree.IsCompatibleWithBot(bot.ActionCollection))
			{
				return false;
			}
			AssignBotBehaviorTreeInternal(behaviorTree, bot);
			return true;
		}

		private void AssignBotBehaviorTreeInternal(MyBehaviorTree behaviorTree, IMyBot bot)
		{
			bot.BotMemory.AssignBehaviorTree(behaviorTree);
			m_BTDataByName[behaviorTree.BehaviorTreeId].BotsData.Add(new BotData(bot));
			m_botBehaviorIds[bot] = behaviorTree.BehaviorTreeId;
		}

		public void UnassignBotBehaviorTree(IMyBot bot)
		{
			m_BTDataByName[m_botBehaviorIds[bot]].RemoveBot(bot);
			bot.BotMemory.UnassignCurrentBehaviorTree();
			m_botBehaviorIds[bot] = MyStringHash.NullOrEmpty;
		}

		public MyBehaviorTree TryGetBehaviorTreeForBot(IMyBot bot)
		{
			BTData value = null;
			m_BTDataByName.TryGetValue(m_botBehaviorIds[bot], out value);
			return value?.BehaviorTree;
		}

		public string GetBehaviorName(IMyBot bot)
		{
			m_botBehaviorIds.TryGetValue(bot, out var value);
			return value.String;
		}

		public void SetBehaviorName(IMyBot bot, string behaviorName)
		{
			m_botBehaviorIds[bot] = MyStringHash.GetOrCompute(behaviorName);
		}

		private bool BuildBehaviorTree(MyBehaviorDefinition behaviorDefinition)
		{
			if (m_BTDataByName.ContainsKey(behaviorDefinition.Id.SubtypeId))
			{
				return false;
			}
			MyBehaviorTree myBehaviorTree = new MyBehaviorTree(behaviorDefinition);
			myBehaviorTree.Construct();
			BTData value = new BTData(myBehaviorTree);
			m_BTDataByName.Add(behaviorDefinition.Id.SubtypeId, value);
			return true;
		}

		public bool ChangeBehaviorTree(string behaviorTreeName, IMyBot bot)
		{
			if (!TryGetBehaviorTreeByName(behaviorTreeName, out var behaviorTree))
			{
				return false;
			}
			if (!behaviorTree.IsCompatibleWithBot(bot.ActionCollection))
			{
				return false;
			}
			MyBehaviorTree myBehaviorTree = TryGetBehaviorTreeForBot(bot);
			bool flag;
			if (myBehaviorTree != null)
			{
				if (myBehaviorTree.BehaviorTreeId == behaviorTree.BehaviorTreeId)
				{
					flag = false;
				}
				else
				{
					UnassignBotBehaviorTree(bot);
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				AssignBotBehaviorTreeInternal(behaviorTree, bot);
			}
			return flag;
		}

		public bool RebuildBehaviorTree(MyBehaviorDefinition newDefinition, out MyBehaviorTree outBehaviorTree)
		{
			if (m_BTDataByName.ContainsKey(newDefinition.Id.SubtypeId))
			{
				outBehaviorTree = m_BTDataByName[newDefinition.Id.SubtypeId].BehaviorTree;
				outBehaviorTree.ReconstructTree(newDefinition);
				return true;
			}
			outBehaviorTree = null;
			return false;
		}

		public bool HasBehavior(MyStringHash id)
		{
			return m_BTDataByName.ContainsKey(id);
		}

		public bool TryGetBehaviorTreeByName(string name, out MyBehaviorTree behaviorTree)
		{
			MyStringHash.TryGet(name, out var id);
			if (id != MyStringHash.NullOrEmpty && m_BTDataByName.ContainsKey(id))
			{
				behaviorTree = m_BTDataByName[id].BehaviorTree;
				return behaviorTree != null;
			}
			behaviorTree = null;
			return false;
		}

		public static bool LoadUploadedBehaviorTree(out MyBehaviorDefinition definition)
		{
			MyBehaviorDefinition myBehaviorDefinition = (definition = LoadBehaviorTreeFromFile(Path.Combine(MyFileSystem.UserDataPath, "UploadTree" + DEFAULT_EXTENSION)));
			return definition != null;
		}

		private static MyBehaviorDefinition LoadBehaviorTreeFromFile(string path)
		{
			MyObjectBuilderSerializer.DeserializeXML(path, out MyObjectBuilder_Definitions objectBuilder);
			if (objectBuilder?.AIBehaviors != null && objectBuilder.AIBehaviors.Length != 0)
			{
				MyObjectBuilder_BehaviorTreeDefinition builder = objectBuilder.AIBehaviors[0];
				MyBehaviorDefinition myBehaviorDefinition = new MyBehaviorDefinition();
				MyModContext myModContext = new MyModContext();
				myModContext.Init("BehaviorDefinition", Path.GetFileName(path));
				myBehaviorDefinition.Init(builder, myModContext);
				return myBehaviorDefinition;
			}
			return null;
		}
	}
}
