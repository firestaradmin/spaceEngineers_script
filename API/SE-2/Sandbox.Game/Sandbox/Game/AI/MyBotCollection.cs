using System;
using System.Collections.Generic;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI.BehaviorTree;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.Graphics;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI
{
	public class MyBotCollection
	{
		private readonly Dictionary<Type, ActionCollection> m_botActions;

		private readonly MyBehaviorTreeCollection m_behaviorTreeCollection;

		private readonly Dictionary<string, int> m_botsCountPerBehavior;

		private readonly List<int> m_botsQueue;

		private int m_botIndex = -1;

		public bool HasBot => m_botsQueue.Count > 0;

		public int TotalBotCount { get; set; }

		public Dictionary<int, IMyBot> BotsDictionary { get; }

		public MyBotCollection(MyBehaviorTreeCollection behaviorTreeCollection)
		{
			m_behaviorTreeCollection = behaviorTreeCollection;
			BotsDictionary = new Dictionary<int, IMyBot>(8);
			m_botActions = new Dictionary<Type, ActionCollection>(8);
			m_botsQueue = new List<int>(8);
			m_botsCountPerBehavior = new Dictionary<string, int>();
		}

		public void UnloadData()
		{
			foreach (KeyValuePair<int, IMyBot> item in BotsDictionary)
			{
				item.Value.Cleanup();
			}
		}

		public void Update()
		{
			foreach (KeyValuePair<int, IMyBot> item in BotsDictionary)
			{
				item.Value.Update();
			}
		}

		public void AddBot(int botHandler, IMyBot newBot)
		{
			if (!BotsDictionary.ContainsKey(botHandler))
			{
				ActionCollection actionCollection;
				if (!m_botActions.ContainsKey(newBot.BotActions.GetType()))
				{
					actionCollection = ActionCollection.CreateActionCollection(newBot);
					m_botActions[newBot.GetType()] = actionCollection;
				}
				else
				{
					actionCollection = m_botActions[newBot.GetType()];
				}
				newBot.InitActions(actionCollection);
				if (string.IsNullOrEmpty(newBot.BehaviorSubtypeName))
				{
					m_behaviorTreeCollection.AssignBotToBehaviorTree(newBot.BotDefinition.BotBehaviorTree.SubtypeName, newBot);
				}
				else
				{
					m_behaviorTreeCollection.AssignBotToBehaviorTree(newBot.BehaviorSubtypeName, newBot);
				}
				BotsDictionary.Add(botHandler, newBot);
				m_botsQueue.Add(botHandler);
				if (m_botsCountPerBehavior.ContainsKey(newBot.BotDefinition.BehaviorType))
				{
					m_botsCountPerBehavior[newBot.BotDefinition.BehaviorType]++;
				}
				else
				{
					m_botsCountPerBehavior[newBot.BotDefinition.BehaviorType] = 1;
				}
			}
		}

		public void ResetBots(string treeName)
		{
			foreach (IMyBot value in BotsDictionary.Values)
			{
				if (value.BehaviorSubtypeName == treeName)
				{
					value.Reset();
				}
			}
		}

		public void CheckCompatibilityWithBots(MyBehaviorTree behaviorTree)
		{
			foreach (IMyBot value in BotsDictionary.Values)
			{
				if (behaviorTree.BehaviorTreeName.CompareTo(value.BehaviorSubtypeName) == 0)
				{
					if (!behaviorTree.IsCompatibleWithBot(value.ActionCollection))
					{
						m_behaviorTreeCollection.UnassignBotBehaviorTree(value);
					}
					else
					{
						value.BotMemory.ResetMemory();
					}
				}
			}
		}

		public int GetHandleToFirstBot()
		{
			if (m_botsQueue.Count > 0)
			{
				return m_botsQueue[0];
			}
			return -1;
		}

		public int GetHandleToFirstBot(string behaviorType)
		{
			foreach (int item in m_botsQueue)
			{
				if (BotsDictionary[item].BotDefinition.BehaviorType == behaviorType)
				{
					return item;
				}
			}
			return -1;
		}

		public BotType GetBotType(int botHandler)
		{
			if (BotsDictionary.ContainsKey(botHandler))
			{
				MyBotLogic botLogic = BotsDictionary[botHandler].BotLogic;
				if (botLogic != null)
				{
					return botLogic.BotType;
				}
			}
			return BotType.UNKNOWN;
		}

		public void TryRemoveBot(int botHandler)
		{
			BotsDictionary.TryGetValue(botHandler, out var value);
			if (value == null)
			{
				return;
			}
			string behaviorType = value.BotDefinition.BehaviorType;
			value.Cleanup();
			if (m_botIndex != -1)
			{
				if (m_behaviorTreeCollection.DebugBot == value)
				{
					m_behaviorTreeCollection.DebugBot = null;
				}
				int num = m_botsQueue.IndexOf(botHandler);
				if (num < m_botIndex)
				{
					m_botIndex--;
				}
				else if (num == m_botIndex)
				{
					m_botIndex = -1;
				}
			}
			BotsDictionary.Remove(botHandler);
			m_botsQueue.Remove(botHandler);
			m_botsCountPerBehavior[behaviorType]--;
		}

		public int GetCurrentBotsCount(string behaviorType)
		{
			if (!m_botsCountPerBehavior.ContainsKey(behaviorType))
			{
				return 0;
			}
			return m_botsCountPerBehavior[behaviorType];
		}

		public BotType TryGetBot<BotType>(int botHandler) where BotType : class, IMyBot
		{
			BotsDictionary.TryGetValue(botHandler, out var value);
			return value as BotType;
		}

		public DictionaryReader<int, IMyBot> GetAllBots()
		{
			return new DictionaryReader<int, IMyBot>(BotsDictionary);
		}

		public void GetBotsData(List<MyObjectBuilder_AIComponent.BotData> botDataList, MyObjectBuilder_AIComponent ob)
		{
			if (ob.BotBrains == null)
			{
				ob.BotBrains = new List<MyObjectBuilder_AIComponent.BotData>();
			}
			foreach (KeyValuePair<int, IMyBot> item2 in BotsDictionary)
			{
				MyObjectBuilder_AIComponent.BotData botData = default(MyObjectBuilder_AIComponent.BotData);
				botData.BotBrain = item2.Value.GetObjectBuilder();
				botData.PlayerHandle = item2.Key;
				MyObjectBuilder_AIComponent.BotData item = botData;
				botDataList.Add(item);
				ob.BotBrains.Add(item);
			}
		}

		public int GetCreatedBotCount()
		{
			int num = 0;
			foreach (IMyBot value in BotsDictionary.Values)
			{
				if (value.CreatedByPlayer)
				{
					num++;
				}
			}
			return num;
		}

		public int GetGeneratedBotCount()
		{
			int num = 0;
			foreach (IMyBot value in BotsDictionary.Values)
			{
				if (!value.CreatedByPlayer)
				{
					num++;
				}
			}
			return num;
		}

		internal void SelectBotForDebugging(IMyBot bot)
		{
			m_behaviorTreeCollection.DebugBot = bot;
			for (int i = 0; i < m_botsQueue.Count; i++)
			{
				if (BotsDictionary[m_botsQueue[i]] == bot)
				{
					m_botIndex = i;
					break;
				}
			}
		}

		public bool IsBotSelectedForDebugging(IMyBot bot)
		{
			return m_behaviorTreeCollection.DebugBot == bot;
		}

		internal void SelectBotForDebugging(int index)
		{
			if (m_botIndex != -1)
			{
				int key = m_botsQueue[index];
				m_behaviorTreeCollection.DebugBot = BotsDictionary[key];
			}
		}

		internal void DebugSelectNextBot()
		{
			m_botIndex++;
			if (m_botIndex == m_botsQueue.Count)
			{
				if (m_botsQueue.Count == 0)
				{
					m_botIndex = -1;
				}
				else
				{
					m_botIndex = 0;
				}
			}
			SelectBotForDebugging(m_botIndex);
		}

		internal void DebugSelectPreviousBot()
		{
			m_botIndex--;
			if (m_botIndex < 0)
			{
				if (m_botsQueue.Count > 0)
				{
					m_botIndex = m_botsQueue.Count - 1;
				}
				else
				{
					m_botIndex = -1;
				}
			}
			SelectBotForDebugging(m_botIndex);
		}

		internal void DebugDrawBots()
		{
			if (!MyDebugDrawSettings.DEBUG_DRAW_BOTS)
			{
				return;
			}
			Vector2 normalizedCoord = new Vector2(0.01f, 0.4f);
			for (int i = 0; i < m_botsQueue.Count; i++)
			{
				IMyEntityBot myEntityBot;
				if ((myEntityBot = BotsDictionary[m_botsQueue[i]] as IMyEntityBot) != null)
				{
					Color color = Color.Green;
					if (m_botIndex == -1 || i != m_botIndex)
					{
						color = Color.Red;
					}
					Vector2 hudPixelCoordFromNormalizedCoord = MyGuiManager.GetHudPixelCoordFromNormalizedCoord(normalizedCoord);
					string text = $"Bot[{i}]: {myEntityBot.BehaviorSubtypeName}";
					if (myEntityBot is MyAgentBot)
					{
						text += (myEntityBot as MyAgentBot).LastActions.GetLastActionsString();
					}
					MyRenderProxy.DebugDrawText2D(hudPixelCoordFromNormalizedCoord, text, color, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
					MyCharacter myCharacter = myEntityBot.BotEntity as MyCharacter;
					IMyFaction myFaction = null;
					if (myCharacter != null)
					{
						long identityId = myCharacter.ControllerInfo.Controller.Player.Identity.IdentityId;
						myFaction = MySession.Static.Factions.TryGetPlayerFaction(identityId);
					}
					if (myEntityBot.BotEntity != null)
					{
						Vector3D center = myEntityBot.BotEntity.PositionComp.WorldAABB.Center;
						center.Y += myEntityBot.BotEntity.PositionComp.WorldAABB.HalfExtents.Y;
						MyRenderProxy.DebugDrawText3D(center, $"Bot:{i}", color, 1f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
						MyRenderProxy.DebugDrawText3D(center - new Vector3(0f, -0.5f, 0f), (myFaction == null) ? "NO_FACTION" : myFaction.Tag, color, 1f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
					}
					normalizedCoord.Y += 0.02f;
				}
			}
		}

		internal void DebugDraw()
		{
			if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || !MyDebugDrawSettings.DEBUG_DRAW_BOTS)
			{
				return;
			}
			foreach (KeyValuePair<int, IMyBot> item in BotsDictionary)
			{
				item.Value.DebugDraw();
			}
		}
	}
}
