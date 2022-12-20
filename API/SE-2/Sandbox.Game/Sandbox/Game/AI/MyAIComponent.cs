using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI.BehaviorTree;
using Sandbox.Game.AI.Commands;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.AI.Pathfinding.RecastDetour;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI
{
	[MySessionComponentDescriptor(MyUpdateOrder.Simulation | MyUpdateOrder.AfterSimulation, 1000, typeof(MyObjectBuilder_AIComponent), null, false)]
	public class MyAIComponent : MySessionComponentBase
	{
		private struct AgentSpawnData
		{
			public readonly MyAgentDefinition AgentDefinition;

			public readonly Vector3D? SpawnPosition;

			public readonly bool CreatedByPlayer;

			public readonly int BotId;

			public bool IsWildlifeAgent;
<<<<<<< HEAD

			public ulong SteamId;

			public string Name;

			public Vector3? Up;

			public Vector3? Direction;

=======

			public ulong SteamId;

			public string Name;

			public Vector3? Up;

			public Vector3? Direction;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public AgentSpawnData(MyAgentDefinition agentDefinition, ulong steamId, int botId, Vector3D? spawnPosition = null, bool createAlways = false, bool isWildlifeAgent = false, Vector3? up = null, Vector3? direction = null, string name = null)
			{
				AgentDefinition = agentDefinition;
				SpawnPosition = spawnPosition;
				CreatedByPlayer = createAlways;
				BotId = botId;
				IsWildlifeAgent = isWildlifeAgent;
				SteamId = steamId;
				Direction = direction;
				Up = up;
				Name = name;
			}
		}

		public struct AgentGroupData
		{
			public MyAgentDefinition AgentDefinition;

			public int Count;

			public AgentGroupData(MyAgentDefinition agentDefinition, int count)
			{
				AgentDefinition = agentDefinition;
				Count = count;
			}
		}

		private struct BotRemovalRequest
		{
			public int SerialId;

			public bool RemoveCharacter;
		}

		private Dictionary<int, MyObjectBuilder_Bot> m_loadedBotObjectBuildersByHandle;

		private List<int> m_loadedLocalPlayers;

		private readonly List<Vector3D> m_tmpSpawnPoints = new List<Vector3D>();

		public static MyAIComponent Static;

		public static MyBotFactoryBase BotFactory;

		private int m_lastBotId;

		private Dictionary<int, AgentSpawnData> m_agentsToSpawn;

		private MyHudNotification m_maxBotNotification;

		private bool m_debugDrawPathfinding;

		public MyAgentDefinition BotToSpawn;

		public MyAiCommandDefinition CommandDefinition;

		private MyConcurrentQueue<BotRemovalRequest> m_removeQueue;

		private MyConcurrentQueue<AgentSpawnData> m_processQueue;

		private FastResourceLock m_lock;

		private BoundingBoxD m_debugTargetAABB;

		public MyBotCollection Bots { get; private set; }

		public IMyPathfinding Pathfinding { get; private set; }

		public MyBehaviorTreeCollection BehaviorTrees { get; private set; }

		public override Type[] Dependencies => new Type[1] { typeof(MyToolbarComponent) };

		public Vector3D? DebugTarget { get; private set; }

		public event Action<int, MyBotDefinition> BotCreatedEvent;

		public MyAIComponent()
		{
			Static = this;
			BotFactory = Activator.CreateInstance(MyPerGameSettings.BotFactoryType) as MyBotFactoryBase;
		}

		public override void LoadData()
		{
			base.LoadData();
			if (MyPerGameSettings.EnableAi)
			{
				Sync.Players.NewPlayerRequestSucceeded += PlayerCreated;
				Sync.Players.LocalPlayerLoaded += LocalPlayerLoaded;
				Sync.Players.NewPlayerRequestFailed += Players_NewPlayerRequestFailed;
				if (Sync.IsServer)
				{
					Sync.Players.PlayerRemoved += Players_PlayerRemoved;
					Sync.Players.PlayerRequesting += Players_PlayerRequesting;
				}
				if (MyPerGameSettings.PathfindingType != null)
				{
					Pathfinding = Activator.CreateInstance(MyPerGameSettings.PathfindingType) as IMyPathfinding;
				}
				BehaviorTrees = new MyBehaviorTreeCollection();
				Bots = new MyBotCollection(BehaviorTrees);
				m_loadedLocalPlayers = new List<int>();
				m_loadedBotObjectBuildersByHandle = new Dictionary<int, MyObjectBuilder_Bot>();
				m_agentsToSpawn = new Dictionary<int, AgentSpawnData>();
				m_removeQueue = new MyConcurrentQueue<BotRemovalRequest>();
				m_maxBotNotification = new MyHudNotification(MyCommonTexts.NotificationMaximumNumberBots, 2000, "Red");
				m_processQueue = new MyConcurrentQueue<AgentSpawnData>();
				m_lock = new FastResourceLock();
				if (MyPlatformGameSettings.ENABLE_BEHAVIOR_TREE_TOOL_COMMUNICATION && MyVRage.Platform.Windows.Window != null)
				{
					MyVRage.Platform.Windows.Window.AddMessageHandler(1034u, OnUploadNewTree);
					MyVRage.Platform.Windows.Window.AddMessageHandler(1036u, OnBreakDebugging);
					MyVRage.Platform.Windows.Window.AddMessageHandler(1035u, OnResumeDebugging);
				}
				MyToolbarComponent.CurrentToolbar.SelectedSlotChanged += CurrentToolbar_SelectedSlotChanged;
				MyToolbarComponent.CurrentToolbar.SlotActivated += CurrentToolbar_SlotActivated;
				MyToolbarComponent.CurrentToolbar.Unselected += CurrentToolbar_Unselected;
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponentBuilder)
		{
			if (!MyPerGameSettings.EnableAi)
			{
				return;
			}
			base.Init(sessionComponentBuilder);
			MyObjectBuilder_AIComponent myObjectBuilder_AIComponent = (MyObjectBuilder_AIComponent)sessionComponentBuilder;
			if (myObjectBuilder_AIComponent.BotBrains == null)
			{
				return;
			}
			foreach (MyObjectBuilder_AIComponent.BotData botBrain in myObjectBuilder_AIComponent.BotBrains)
			{
				m_loadedBotObjectBuildersByHandle[botBrain.PlayerHandle] = botBrain.BotBrain;
			}
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			if (!MyPerGameSettings.EnableAi)
			{
				return;
			}
			foreach (int loadedLocalPlayer in m_loadedLocalPlayers)
			{
				m_loadedBotObjectBuildersByHandle.TryGetValue(loadedLocalPlayer, out var value);
				if (value == null || value.TypeId == value.BotDefId.TypeId)
				{
					CreateBot(loadedLocalPlayer, value);
				}
			}
			m_loadedLocalPlayers.Clear();
			m_loadedBotObjectBuildersByHandle.Clear();
			Sync.Players.LocalPlayerRemoved += LocalPlayerRemoved;
		}

		public override void Simulate()
		{
			if (!MyPerGameSettings.EnableAi)
			{
				return;
			}
			if (MyFakes.DEBUG_ONE_VOXEL_PATHFINDING_STEP_SETTING)
			{
				if (!MyFakes.DEBUG_ONE_VOXEL_PATHFINDING_STEP)
				{
					return;
				}
			}
			else if (MyFakes.DEBUG_ONE_AI_STEP_SETTING)
			{
				if (!MyFakes.DEBUG_ONE_AI_STEP)
				{
					return;
				}
				MyFakes.DEBUG_ONE_AI_STEP = false;
			}
			MySimpleProfiler.Begin("AI", MySimpleProfiler.ProfilingBlockType.OTHER, "Simulate");
			Pathfinding?.Update();
			base.Simulate();
			BehaviorTrees.Update();
			Bots.Update();
			MySimpleProfiler.End("Simulate");
		}

		public void PathfindingSetDrawDebug(bool drawDebug)
		{
			m_debugDrawPathfinding = drawDebug;
		}

		public void PathfindingSetDrawNavmesh(bool drawNavmesh)
		{
			(Pathfinding as MyRDPathfinding)?.SetDrawNavmesh(drawNavmesh);
		}

		public void GenerateNavmeshTile(Vector3D? target)
		{
			if (target.HasValue)
			{
				Vector3D worldCenter = target.Value + 0.1f;
				MyDestinationSphere end = new MyDestinationSphere(ref worldCenter, 1f);
				Static.Pathfinding.FindPathGlobal(target.Value - 0.10000000149011612, end, null)?.GetNextTarget(target.Value, out var _, out var _, out var _);
			}
			DebugTarget = target;
		}

		public void InvalidateNavmeshPosition(Vector3D? target)
		{
			if (target.HasValue)
			{
				MyRDPathfinding myRDPathfinding = (MyRDPathfinding)Static.Pathfinding;
				if (myRDPathfinding != null)
				{
					BoundingBoxD areaBox = new BoundingBoxD(target.Value - 0.1, target.Value + 0.1);
					myRDPathfinding.InvalidateArea(areaBox);
				}
			}
			DebugTarget = target;
		}

		public void SetPathfindingDebugTarget(Vector3D? target)
		{
			MyExternalPathfinding myExternalPathfinding;
			if ((myExternalPathfinding = Pathfinding as MyExternalPathfinding) != null)
			{
				myExternalPathfinding.SetTarget(target);
			}
			else if (target.HasValue)
			{
				m_debugTargetAABB = new MyOrientedBoundingBoxD(target.Value, new Vector3D(5.0, 5.0, 5.0), Quaternion.Identity).GetAABB();
				List<MyEntity> result = new List<MyEntity>();
				MyGamePruningStructure.GetAllEntitiesInBox(ref m_debugTargetAABB, result);
			}
			DebugTarget = target;
		}

		private void DrawDebugTarget()
		{
			if (DebugTarget.HasValue)
			{
				MyRenderProxy.DebugDrawSphere(DebugTarget.Value, 0.2f, Color.Red, 0f, depthRead: false);
				MyRenderProxy.DebugDrawAABB(m_debugTargetAABB, Color.Green);
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (MyPerGameSettings.EnableAi)
			{
				PerformBotRemovals();
				AgentSpawnData instance;
				while (m_processQueue.TryDequeue(out instance))
				{
					SpawnAgent(ref instance);
				}
				if (m_debugDrawPathfinding)
				{
					Pathfinding?.DebugDraw();
				}
				Bots.DebugDraw();
				DebugDrawBots();
				DrawDebugTarget();
			}
		}

		private long SpawnAgent(ref AgentSpawnData newBotData)
		{
			m_agentsToSpawn[newBotData.BotId] = newBotData;
			return Sync.Players.RequestNewPlayer(newBotData.SteamId, newBotData.BotId, string.IsNullOrEmpty(newBotData.Name) ? MyDefinitionManager.Static.GetRandomCharacterName() : newBotData.Name, newBotData.AgentDefinition.BotModel, realPlayer: false, initialPlayer: false, newBotData.IsWildlifeAgent);
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			if (MyPerGameSettings.EnableAi)
			{
				Sync.Players.NewPlayerRequestSucceeded -= PlayerCreated;
				Sync.Players.LocalPlayerRemoved -= LocalPlayerRemoved;
				Sync.Players.LocalPlayerLoaded -= LocalPlayerLoaded;
				Sync.Players.NewPlayerRequestFailed -= Players_NewPlayerRequestFailed;
				if (Sync.IsServer)
				{
					Sync.Players.PlayerRequesting -= Players_PlayerRequesting;
					Sync.Players.PlayerRemoved -= Players_PlayerRemoved;
				}
				Pathfinding?.UnloadData();
				Bots.UnloadData();
				Bots = null;
				Pathfinding = null;
				if (MyPlatformGameSettings.ENABLE_BEHAVIOR_TREE_TOOL_COMMUNICATION && MyVRage.Platform?.Windows.Window != null)
				{
					MyVRage.Platform.Windows.Window.RemoveMessageHandler(1034u, OnUploadNewTree);
					MyVRage.Platform.Windows.Window.RemoveMessageHandler(1036u, OnBreakDebugging);
					MyVRage.Platform.Windows.Window.RemoveMessageHandler(1035u, OnResumeDebugging);
				}
				if (MyToolbarComponent.CurrentToolbar != null)
				{
					MyToolbarComponent.CurrentToolbar.SelectedSlotChanged -= CurrentToolbar_SelectedSlotChanged;
					MyToolbarComponent.CurrentToolbar.SlotActivated -= CurrentToolbar_SlotActivated;
					MyToolbarComponent.CurrentToolbar.Unselected -= CurrentToolbar_Unselected;
				}
			}
			Static = null;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			if (!MyPerGameSettings.EnableAi)
			{
				return null;
			}
			MyObjectBuilder_AIComponent myObjectBuilder_AIComponent = (MyObjectBuilder_AIComponent)base.GetObjectBuilder();
			Bots.GetBotsData(myObjectBuilder_AIComponent.BotBrains, myObjectBuilder_AIComponent);
			return myObjectBuilder_AIComponent;
		}

		public int SpawnNewBot(MyAgentDefinition agentDefinition)
		{
			Vector3D spawnPosition = default(Vector3D);
			if (!BotFactory.GetBotSpawnPosition(agentDefinition.BehaviorType, out spawnPosition))
			{
				return 0;
			}
			return SpawnNewBotInternal(agentDefinition, spawnPosition);
		}

		public int SpawnNewBot(MyAgentDefinition agentDefinition, Vector3D position, bool createdByPlayer = true, bool isWildlifeAgent = true)
		{
			return SpawnNewBotInternal(agentDefinition, position, createdByPlayer, isWildlifeAgent);
		}

		public bool SpawnNewBotGroup(string type, List<AgentGroupData> groupData, List<int> outIds)
		{
			int num = 0;
			foreach (AgentGroupData groupDatum in groupData)
			{
				num += groupDatum.Count;
			}
			BotFactory.GetBotGroupSpawnPositions(type, num, m_tmpSpawnPoints);
			int count = m_tmpSpawnPoints.Count;
			int i = 0;
			int num2 = 0;
			int num3 = 0;
			for (; i < count; i++)
			{
				int item = SpawnNewBotInternal(groupData[num2].AgentDefinition, m_tmpSpawnPoints[i]);
				outIds?.Add(item);
				if (groupData[num2].Count == ++num3)
				{
					num3 = 0;
					num2++;
				}
			}
			m_tmpSpawnPoints.Clear();
			return count == num;
		}

		private int SpawnNewBotInternal(MyAgentDefinition agentDefinition, Vector3D? spawnPosition = null, bool createdByPlayer = false, bool isWildlifeAgent = true)
		{
			int lastBotId;
			using (m_lock.AcquireExclusiveUsing())
			{
				foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
				{
					if (onlinePlayer.Id.SteamId == Sync.MyId && onlinePlayer.Id.SerialId > m_lastBotId)
					{
						m_lastBotId = onlinePlayer.Id.SerialId;
					}
				}
				m_lastBotId++;
				lastBotId = m_lastBotId;
			}
			m_processQueue.Enqueue(new AgentSpawnData(agentDefinition, Sync.MyId, lastBotId, spawnPosition, createdByPlayer, isWildlifeAgent));
			return lastBotId;
		}

		public long SpawnNewBotSync(MyAgentDefinition agentDefinition, ulong steamId, Vector3D spawnPosition, Vector3? direction, Vector3? up, string name)
		{
			int lastBotId;
			using (m_lock.AcquireExclusiveUsing())
			{
				foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
				{
					if (onlinePlayer.Id.SteamId == Sync.MyId && onlinePlayer.Id.SerialId > m_lastBotId)
					{
						m_lastBotId = onlinePlayer.Id.SerialId;
					}
				}
				m_lastBotId++;
				lastBotId = m_lastBotId;
			}
			AgentSpawnData newBotData = new AgentSpawnData(agentDefinition, steamId, lastBotId, spawnPosition, createAlways: false, isWildlifeAgent: true, up, direction, name);
			return SpawnAgent(ref newBotData);
		}

		public int SpawnNewBot(MyAgentDefinition agentDefinition, Vector3D? spawnPosition)
		{
			return SpawnNewBotInternal(agentDefinition, spawnPosition, createdByPlayer: true);
		}

		public bool CanSpawnMoreBots(MyPlayer.PlayerId pid)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			if (MyFakes.DEVELOPMENT_PRESET)
			{
				return true;
			}
			if (Sync.MyId == pid.SteamId)
			{
<<<<<<< HEAD
				if (m_agentsToSpawn.TryGetValue(pid.SerialId, out var _))
=======
				if (m_agentsToSpawn.TryGetValue(pid.SerialId, out var value))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return Bots.GetGeneratedBotCount() < Session.TotalBotLimit;
				}
				return false;
			}
			int num = 0;
			ulong steamId = pid.SteamId;
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Id.SteamId == steamId && onlinePlayer.Id.SerialId != 0)
				{
					num++;
				}
			}
			MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(pid.SteamId, 0));
			int num2 = 32;
			if (playerById != null)
			{
				num2 = MyGamePruningStructure.GetClosestPlanet(playerById.GetPosition())?.Generator.MaxBotsPerPlayer ?? 32;
			}
			return num < num2;
		}

		public int GetAvailableUncontrolledBotsCount()
		{
			return Session.TotalBotLimit - Bots.GetGeneratedBotCount();
		}

		public int GetBotCount(string behaviorType)
		{
			return Bots.GetCurrentBotsCount(behaviorType);
		}

		public void CleanUnusedIdentities()
		{
			List<MyPlayer.PlayerId> list = new List<MyPlayer.PlayerId>();
			foreach (MyPlayer.PlayerId allPlayer in Sync.Players.GetAllPlayers())
			{
				list.Add(allPlayer);
			}
			foreach (MyPlayer.PlayerId item in list)
			{
				if (item.SteamId == Sync.MyId && item.SerialId != 0 && Sync.Players.GetPlayerById(item) == null)
				{
					long num = Sync.Players.TryGetIdentityId(item.SteamId, item.SerialId);
					if (num != 0L)
					{
						Sync.Players.RemoveIdentity(num, item);
					}
				}
			}
		}

		private void PlayerCreated(MyPlayer.PlayerId playerId)
		{
			if (Sync.Players.GetPlayerById(playerId) != null && !Sync.Players.GetPlayerById(playerId).IsRealPlayer)
			{
				CreateBot(playerId.SerialId);
			}
		}

		private void LocalPlayerLoaded(int playerNumber)
		{
			if (playerNumber != 0 && !m_loadedLocalPlayers.Contains(playerNumber))
			{
				m_loadedLocalPlayers.Add(playerNumber);
			}
		}

		private void Players_NewPlayerRequestFailed(int serialId)
		{
			if (serialId != 0 && m_agentsToSpawn.ContainsKey(serialId))
			{
				AgentSpawnData agentSpawnData = m_agentsToSpawn[serialId];
				m_agentsToSpawn.Remove(serialId);
				if (agentSpawnData.CreatedByPlayer)
				{
					MyHud.Notifications.Add(m_maxBotNotification);
				}
			}
		}

		private void Players_PlayerRequesting(PlayerRequestArgs args)
		{
			if (args.PlayerId.SerialId != 0)
			{
				if (!CanSpawnMoreBots(args.PlayerId))
				{
					args.Cancel = true;
				}
				else
				{
					Bots.TotalBotCount++;
				}
			}
		}

		private void Players_PlayerRemoved(MyPlayer.PlayerId pid)
		{
			if (Sync.IsServer && pid.SerialId != 0)
			{
				Bots.TotalBotCount--;
			}
		}

		private void CreateBot(int playerNumber)
		{
			CreateBot(playerNumber, null);
		}

		private void CreateBot(int playerNumber, MyObjectBuilder_Bot botBuilder)
		{
			if (BotFactory == null || Bots == null)
			{
				return;
			}
			MyPlayer myPlayer = ((botBuilder != null) ? Sync.Clients.LocalClient.GetPlayer(botBuilder.AsociatedMyPlayerId, playerNumber) : Sync.Clients.LocalClient.GetPlayer(playerNumber));
			if (myPlayer == null)
			{
				return;
			}
			myPlayer.IsWildlifeAgent = true;
			bool flag = m_agentsToSpawn.ContainsKey(playerNumber);
			bool load = botBuilder != null;
			bool flag2 = false;
			AgentSpawnData agentSpawnData = default(AgentSpawnData);
			MyBotDefinition botDefinition;
			if (flag)
			{
				agentSpawnData = m_agentsToSpawn[playerNumber];
				flag2 = agentSpawnData.CreatedByPlayer;
				botDefinition = agentSpawnData.AgentDefinition;
				m_agentsToSpawn.Remove(playerNumber);
			}
			else
			{
				if (botBuilder == null || botBuilder.BotDefId.TypeId.IsNull)
				{
					if (Sync.Players.TryGetPlayerById(new MyPlayer.PlayerId(Sync.MyId, playerNumber), out var player))
					{
						Sync.Players.RemovePlayer(player);
					}
					return;
				}
				MyDefinitionManager.Static.TryGetBotDefinition(botBuilder.BotDefId, out botDefinition);
			}
			if (botDefinition == null)
			{
				return;
			}
			if (((myPlayer.Character == null || !myPlayer.Character.IsDead) && BotFactory.CanCreateBotOfType(botDefinition.BehaviorType, load)) || flag2)
			{
				IMyBot myBot = null;
				myBot = ((!flag) ? BotFactory.CreateBot(myPlayer, botBuilder, botDefinition) : BotFactory.CreateBot(myPlayer, botBuilder, agentSpawnData.AgentDefinition));
				if (myBot == null)
				{
					MyLog.Default.WriteLine(string.Concat("Could not create a bot for player ", myPlayer, "!"));
					return;
				}
				Bots.AddBot(playerNumber, myBot);
				IMyEntityBot myEntityBot;
				if (flag && (myEntityBot = myBot as IMyEntityBot) != null)
				{
					myEntityBot.Spawn(agentSpawnData.SpawnPosition, agentSpawnData.Direction, agentSpawnData.Up, flag2);
					if (myEntityBot.BotEntity == null)
					{
						MyLog.Default.WriteLine("Bot entity is null! bot: " + agentSpawnData.Name);
<<<<<<< HEAD
						return;
					}
					if (!string.IsNullOrEmpty(agentSpawnData.Name))
					{
						myEntityBot.BotEntity.Name = agentSpawnData.Name;
					}
=======
					}
					myEntityBot.BotEntity.Name = agentSpawnData.Name;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				if (myBot.BotDefinition == null)
				{
					MyLog.Default.WriteLine("Bot definition is null! bot: " + agentSpawnData.Name);
				}
				this.BotCreatedEvent?.Invoke(playerNumber, myBot.BotDefinition);
			}
			else
			{
				MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, playerNumber));
				Sync.Players.RemovePlayer(playerById);
			}
		}

		public void DespawnBotsOfType(string botType)
		{
			foreach (KeyValuePair<int, IMyBot> allBot in Bots.GetAllBots())
			{
				if (allBot.Value.BotDefinition.BehaviorType == botType)
				{
					Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, allBot.Key));
					RemoveBot(allBot.Key, removeCharacter: true);
				}
			}
			PerformBotRemovals();
		}

		private void PerformBotRemovals()
		{
			BotRemovalRequest instance;
			while (m_removeQueue.TryDequeue(out instance))
			{
				MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, instance.SerialId));
				if (playerById != null)
				{
					Sync.Players.RemovePlayer(playerById, instance.RemoveCharacter);
				}
			}
		}

		public void RemoveBot(int playerNumber, bool removeCharacter = false)
		{
			BotRemovalRequest botRemovalRequest = default(BotRemovalRequest);
			botRemovalRequest.SerialId = playerNumber;
			botRemovalRequest.RemoveCharacter = removeCharacter;
			BotRemovalRequest instance = botRemovalRequest;
			m_removeQueue.Enqueue(instance);
		}

		private void LocalPlayerRemoved(int playerNumber)
		{
			if (playerNumber != 0)
			{
				Bots.TryRemoveBot(playerNumber);
			}
		}

		public override void HandleInput()
		{
			base.HandleInput();
			if (MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.PRIMARY_TOOL_ACTION))
			{
				if (MySession.Static.ControlledEntity != null && BotToSpawn != null)
				{
					TrySpawnBot();
				}
				if (MySession.Static.ControlledEntity != null && CommandDefinition != null)
				{
					UseCommand();
				}
			}
		}

		public void TrySpawnBot(MyAgentDefinition agentDefinition)
		{
			BotToSpawn = agentDefinition;
			TrySpawnBot();
		}

		private void CurrentToolbar_SelectedSlotChanged(MyToolbar toolbar, MyToolbar.SlotArgs args)
		{
			if (!(toolbar.SelectedItem is MyToolbarItemBot))
			{
				BotToSpawn = null;
			}
			if (!(toolbar.SelectedItem is MyToolbarItemAiCommand))
			{
				CommandDefinition = null;
			}
		}

		private void CurrentToolbar_SlotActivated(MyToolbar toolbar, MyToolbar.SlotArgs args, bool userActivated)
		{
			if (!(toolbar.GetItemAtIndex(toolbar.SlotToIndex(args.SlotNumber.Value)) is MyToolbarItemBot))
			{
				BotToSpawn = null;
			}
			if (!(toolbar.GetItemAtIndex(toolbar.SlotToIndex(args.SlotNumber.Value)) is MyToolbarItemAiCommand))
			{
				CommandDefinition = null;
			}
		}

		private void CurrentToolbar_Unselected(MyToolbar toolbar)
		{
			BotToSpawn = null;
			CommandDefinition = null;
		}

		private void TrySpawnBot()
		{
			Vector3D position = ((MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.ThirdPersonSpectator && MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.Entity) ? MySector.MainCamera.Position : MySession.Static.ControlledEntity.GetHeadMatrix(includeY: true).Translation);
			List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
			LineD lineD = new LineD(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 1000f);
			MyPhysics.CastRay(lineD.From, lineD.To, list, 15);
			if (list.Count == 0)
			{
				Static.SpawnNewBot(BotToSpawn, position);
				return;
			}
			MyPhysics.HitInfo? hitInfo = null;
			foreach (MyPhysics.HitInfo item in list)
			{
				IMyEntity hitEntity = item.HkHitInfo.GetHitEntity();
				if (hitEntity is MyCubeGrid)
				{
					hitInfo = item;
					break;
				}
				if (hitEntity is MyVoxelBase)
				{
					hitInfo = item;
					break;
				}
				if (hitEntity is MyVoxelPhysics)
				{
					hitInfo = item;
					break;
				}
			}
			Vector3D position2 = ((!hitInfo.HasValue) ? MySector.MainCamera.Position : hitInfo.Value.Position);
			Static.SpawnNewBot(BotToSpawn, position2);
		}

		private void UseCommand()
		{
			MyAiCommandBehavior myAiCommandBehavior = new MyAiCommandBehavior();
			myAiCommandBehavior.InitCommand(CommandDefinition);
			myAiCommandBehavior.ActivateCommand();
		}

		public static int GenerateBotId(int lastSpawnedBot)
		{
			int num = lastSpawnedBot;
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Id.SteamId == Sync.MyId)
				{
					num = Math.Max(num, onlinePlayer.Id.SerialId);
				}
			}
			return num + 1;
		}

		public static int GenerateBotId()
		{
			int lastBotId = Static.m_lastBotId;
			Static.m_lastBotId = GenerateBotId(lastBotId);
			return Static.m_lastBotId;
		}

		public void DebugDrawBots()
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				Bots.DebugDrawBots();
			}
		}

		public void DebugSelectNextBot()
		{
			Bots.DebugSelectNextBot();
		}

		public void DebugSelectPreviousBot()
		{
			Bots.DebugSelectPreviousBot();
		}

		public void DebugRemoveFirstBot()
		{
			if (Bots.HasBot)
			{
				MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, Bots.GetHandleToFirstBot()));
				Sync.Players.RemovePlayer(playerById);
			}
		}

		private void OnUploadNewTree(ref MyMessage msg)
		{
			if (BehaviorTrees != null)
			{
				MyBehaviorTree outBehaviorTree = null;
				MyBehaviorDefinition definition = null;
				if (MyBehaviorTreeCollection.LoadUploadedBehaviorTree(out definition) && BehaviorTrees.HasBehavior(definition.Id.SubtypeId))
				{
					Bots.ResetBots(definition.Id.SubtypeName);
					BehaviorTrees.RebuildBehaviorTree(definition, out outBehaviorTree);
					Bots.CheckCompatibilityWithBots(outBehaviorTree);
				}
				IntPtr windowHandle = IntPtr.Zero;
				if (BehaviorTrees.TryGetValidToolWindow(out windowHandle))
				{
					MyVRage.Platform.Windows.PostMessage(windowHandle, 1028u, IntPtr.Zero, IntPtr.Zero);
				}
			}
		}

		private void OnBreakDebugging(ref MyMessage msg)
		{
			if (BehaviorTrees != null)
			{
				BehaviorTrees.DebugBreakDebugging = true;
			}
		}

		private void OnResumeDebugging(ref MyMessage msg)
		{
			if (BehaviorTrees != null)
			{
				BehaviorTrees.DebugBreakDebugging = false;
			}
		}
	}
}
