using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Definitions;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.AI.Navigation;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI
{
	[MyBotType(typeof(MyObjectBuilder_AgentBot))]
	public class MyAgentBot : IMyEntityBot, IMyBot
	{
		public class SLastRunningState
		{
			public string actionName;

			public int counter;
		}

		public class MyLastActions
		{
			private readonly List<SLastRunningState> m_lastActions = new List<SLastRunningState>();

			private const int MAX_ACTIONS_COUNT = 5;

			private string GetLastAction()
			{
<<<<<<< HEAD
				return m_lastActions.Last().actionName;
=======
				return Enumerable.Last<SLastRunningState>((IEnumerable<SLastRunningState>)m_lastActions).actionName;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			public void AddLastAction(string lastAction)
			{
				if (m_lastActions.Count != 0)
				{
					if (lastAction == GetLastAction())
					{
<<<<<<< HEAD
						m_lastActions.Last().counter++;
=======
						Enumerable.Last<SLastRunningState>((IEnumerable<SLastRunningState>)m_lastActions).counter++;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						return;
					}
					if (m_lastActions.Count == 5)
					{
						m_lastActions.RemoveAt(0);
					}
				}
				m_lastActions.Add(new SLastRunningState
				{
					actionName = lastAction,
					counter = 1
				});
			}

			public void Clear()
			{
				m_lastActions.Clear();
			}

			public string GetLastActionsString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < m_lastActions.Count; i++)
				{
					stringBuilder.AppendFormat("{0}-{1}", m_lastActions[i].counter, m_lastActions[i].actionName);
					if (i != m_lastActions.Count - 1)
					{
						stringBuilder.AppendFormat(", ");
					}
				}
				return stringBuilder.ToString();
			}
		}

		protected MyPlayer m_player;

		protected MyBotNavigation m_navigation;

		protected ActionCollection m_actionCollection;

		protected MyBotMemory m_botMemory;

		protected MyAgentActions m_actions;

		protected MyAgentDefinition m_botDefinition;

		protected MyAgentLogic m_botLogic;

		private int m_deathCountdownMs;

		private int m_lastCountdownTime;

		private bool m_respawnRequestSent;

		private readonly bool m_removeAfterDeath;

		private bool m_botRemoved;

		private bool m_joinRequestSent;

		public MyLastActions LastActions = new MyLastActions();

		public MyPlayer Player => m_player;

		public MyBotNavigation Navigation => m_navigation;

		public MyCharacter AgentEntity => m_player.Controller.ControlledEntity as MyCharacter;

		public MyEntity BotEntity => AgentEntity;

		public string BehaviorSubtypeName => MyAIComponent.Static.BehaviorTrees.GetBehaviorName(this);

		public ActionCollection ActionCollection => m_actionCollection;

		public MyBotMemory BotMemory => m_botMemory;

		public MyBotMemory LastBotMemory { get; set; }

		public MyAgentActions AgentActions => m_actions;

		public MyBotActionsBase BotActions
		{
			get
			{
				return m_actions;
			}
			set
			{
				m_actions = value as MyAgentActions;
			}
		}

		public MyBotDefinition BotDefinition => m_botDefinition;

		public MyAgentDefinition AgentDefinition => m_botDefinition;

		public MyBotLogic BotLogic => m_botLogic;

		public MyAgentLogic AgentLogic => m_botLogic;

		public bool HasLogic => m_botLogic != null;

		public virtual bool IsValidForUpdate
		{
			get
			{
				if (m_player?.Controller.ControlledEntity?.Entity != null && AgentEntity != null)
				{
					return !AgentEntity.IsDead;
				}
				return false;
			}
		}

		public bool CreatedByPlayer { get; set; }

		public void ReturnToLastMemory()
		{
			if (LastBotMemory != null)
			{
				m_botMemory = LastBotMemory;
			}
		}

		public MyAgentBot(MyPlayer player, MyBotDefinition botDefinition)
		{
			m_player = player;
			m_navigation = new MyBotNavigation(player);
			m_actionCollection = null;
			m_botMemory = new MyBotMemory(this);
			m_botDefinition = botDefinition as MyAgentDefinition;
			m_removeAfterDeath = m_botDefinition.RemoveAfterDeath;
			m_respawnRequestSent = false;
			m_botRemoved = false;
			m_player.Controller.ControlledEntityChanged += Controller_ControlledEntityChanged;
			m_navigation.ChangeEntity(m_player.Controller.ControlledEntity);
			MyCestmirDebugInputComponent.PlacedAction += DebugGoto;
		}

		protected virtual void Controller_ControlledEntityChanged(IMyControllableEntity oldEntity, IMyControllableEntity newEntity)
		{
			if (oldEntity == null && newEntity is MyCharacter)
			{
				EraseRespawn();
			}
			m_navigation.ChangeEntity(newEntity);
			m_navigation.AimWithMovement();
			MyCharacter myCharacter;
			if ((myCharacter = newEntity as MyCharacter) != null)
			{
				myCharacter.JetpackComp?.TurnOnJetpack(newState: false);
			}
			if (HasLogic)
			{
				m_botLogic.OnControlledEntityChanged(newEntity);
			}
		}

		public virtual void Init(MyObjectBuilder_Bot botBuilder)
		{
			MyObjectBuilder_AgentBot myObjectBuilder_AgentBot;
			if ((myObjectBuilder_AgentBot = botBuilder as MyObjectBuilder_AgentBot) == null)
			{
				return;
			}
			m_deathCountdownMs = myObjectBuilder_AgentBot.RespawnCounter;
			if (AgentDefinition.FactionTag != null)
			{
				MyFaction myFaction = MySession.Static.Factions.TryGetOrCreateFactionByTag(AgentDefinition.FactionTag);
				if (myFaction != null)
				{
					MyFactionCollection.SendJoinRequest(myFaction.FactionId, Player.Identity.IdentityId);
					m_joinRequestSent = true;
				}
			}
			if (myObjectBuilder_AgentBot.AiTarget != null)
			{
				AgentActions.AiTargetBase.Init(myObjectBuilder_AgentBot.AiTarget);
			}
			if (botBuilder.BotMemory != null)
			{
				m_botMemory.Init(botBuilder.BotMemory);
			}
			MyAIComponent.Static.BehaviorTrees.SetBehaviorName(this, myObjectBuilder_AgentBot.LastBehaviorTree);
		}

		public virtual void InitActions(ActionCollection actionCollection)
		{
			m_actionCollection = actionCollection;
		}

		public virtual void InitLogic(MyBotLogic botLogic)
		{
			m_botLogic = botLogic as MyAgentLogic;
			if (HasLogic)
			{
				m_botLogic.Init();
				if (AgentEntity != null)
				{
					AgentLogic.OnCharacterControlAcquired(AgentEntity);
				}
			}
		}

		public virtual void Spawn(Vector3D? spawnPosition, Vector3? direction, Vector3? up, bool spawnedByPlayer)
		{
			CreatedByPlayer = spawnedByPlayer;
			MyCharacter myCharacter;
			if ((((myCharacter = m_player.Controller.ControlledEntity as MyCharacter) != null && myCharacter.IsDead) || m_player.Identity.IsDead) && !m_respawnRequestSent)
			{
				m_respawnRequestSent = true;
				MyPlayerCollection.OnRespawnRequest(joinGame: false, newIdentity: false, 0L, null, spawnPosition, direction, up, BotDefinition.Id, realPlayer: false, m_player.Id.SerialId, null, Color.Red);
			}
		}

		public virtual void Cleanup()
		{
			MyCestmirDebugInputComponent.PlacedAction -= DebugGoto;
			m_navigation.Cleanup();
			if (HasLogic)
			{
				m_botLogic.Cleanup();
			}
			m_player.Controller.ControlledEntityChanged -= Controller_ControlledEntityChanged;
			m_player = null;
		}

		public void Update()
		{
			if (m_player.Controller.ControlledEntity != null)
			{
				if (AgentEntity != null && AgentEntity.IsDead && !m_respawnRequestSent)
				{
					HandleDeadBot();
					return;
				}
				if (AgentEntity != null && !AgentEntity.IsDead && m_respawnRequestSent)
				{
					EraseRespawn();
				}
				UpdateInternal();
			}
			else if (!m_respawnRequestSent)
			{
				HandleDeadBot();
			}
		}

		private void StartRespawn()
		{
			m_lastCountdownTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_deathCountdownMs = AgentDefinition.RemoveTimeMs;
		}

		private void EraseRespawn()
		{
			m_deathCountdownMs = 0;
			m_respawnRequestSent = false;
		}

		protected virtual void UpdateInternal()
		{
			m_navigation.Update(m_botMemory.TickCounter);
			m_botLogic.Update();
			if (m_joinRequestSent || string.IsNullOrEmpty(m_botDefinition.FactionTag))
			{
				return;
			}
			string tag = m_botDefinition.FactionTag.ToUpperInvariant();
			MyFaction myFaction = MySession.Static.Factions.TryGetFactionByTag(tag);
			if (myFaction != null)
			{
				long controllingIdentityId = AgentEntity.ControllerInfo.ControllingIdentityId;
				if (MySession.Static.Factions.TryGetPlayerFaction(controllingIdentityId) == null && !m_joinRequestSent)
				{
					MyFactionCollection.SendJoinRequest(myFaction.FactionId, controllingIdentityId);
					m_joinRequestSent = true;
				}
			}
		}

		public virtual void Reset()
		{
			BotMemory.ResetMemory(clearMemory: true);
			m_navigation.StopImmediate(forceUpdate: true);
			AgentActions.AiTargetBase.UnsetTarget();
		}

		public virtual MyObjectBuilder_Bot GetObjectBuilder()
		{
			MyObjectBuilder_AgentBot myObjectBuilder_AgentBot = MyAIComponent.BotFactory.GetBotObjectBuilder(this) as MyObjectBuilder_AgentBot;
			myObjectBuilder_AgentBot.BotDefId = BotDefinition.Id;
			myObjectBuilder_AgentBot.AiTarget = AgentActions.AiTargetBase.GetObjectBuilder();
			myObjectBuilder_AgentBot.BotMemory = m_botMemory.GetObjectBuilder();
			myObjectBuilder_AgentBot.LastBehaviorTree = BehaviorSubtypeName;
			myObjectBuilder_AgentBot.RemoveAfterDeath = m_removeAfterDeath;
			myObjectBuilder_AgentBot.RespawnCounter = m_deathCountdownMs;
			if (Player != null)
			{
				myObjectBuilder_AgentBot.AsociatedMyPlayerId = Player.Id.SteamId;
			}
			return myObjectBuilder_AgentBot;
		}

		private void HandleDeadBot()
		{
			if (m_deathCountdownMs <= 0)
			{
				if (!m_removeAfterDeath && MyAIComponent.BotFactory.GetBotSpawnPosition(BotDefinition.BehaviorType, out var spawnPosition))
				{
					MyPlayerCollection.OnRespawnRequest(joinGame: false, newIdentity: false, 0L, null, spawnPosition, null, null, BotDefinition.Id, realPlayer: false, Player.Id.SerialId, null, Color.Red);
					m_respawnRequestSent = true;
				}
				else if (!m_botRemoved)
				{
					m_botRemoved = true;
					MyAIComponent.Static.RemoveBot(Player.Id.SerialId);
				}
			}
			else
			{
				int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				m_deathCountdownMs -= totalGamePlayTimeInMilliseconds - m_lastCountdownTime;
				m_lastCountdownTime = totalGamePlayTimeInMilliseconds;
			}
		}

		public virtual void DebugDraw()
		{
			if (AgentEntity == null)
			{
				return;
			}
			MyAiTargetBase aiTargetBase;
			if ((aiTargetBase = m_actions.AiTargetBase) != null && aiTargetBase.HasTarget())
			{
				MyRenderProxy.DebugDrawPoint(aiTargetBase.TargetPosition, Color.Aquamarine, depthRead: false);
				if (BotEntity != null && aiTargetBase.TargetEntity != null)
				{
					string text = ((aiTargetBase.TargetType == MyAiTargetEnum.CUBE) ? $"Target:{aiTargetBase.GetTargetBlock()}" : $"Target:{aiTargetBase.TargetEntity}");
					Vector3D center = BotEntity.PositionComp.WorldAABB.Center;
					center.Y += BotEntity.PositionComp.WorldAABB.HalfExtents.Y + 0.20000000298023224;
					MyRenderProxy.DebugDrawText3D(center, text, Color.Red, 1f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
				}
			}
			m_botLogic.DebugDraw();
		}

		public virtual void DebugGoto(Vector3D point, MyEntity entity = null)
		{
			if (m_player.Id.SerialId != 0)
			{
				m_navigation.AimWithMovement();
				m_navigation.GotoNoPath(point, 0f, entity);
			}
		}
	}
}
