using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.GameServices;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation, 888, typeof(MyObjectBuilder_SessionComponentContainerDropSystem), null, false)]
	public class MySessionComponentContainerDropSystem : MySessionComponentBase
	{
		private class MyPlayerContainerData
		{
			public long PlayerId;

			public int Timer;

			public bool Active;

			public bool Competetive;

			public MyTerminalBlock Container;

			public long ContainerId;

			public MyPlayerContainerData(long playerId, int timer, bool active, bool competetive, long cargoId)
			{
				PlayerId = playerId;
				Timer = timer;
				Active = active;
				Competetive = competetive;
				ContainerId = cargoId;
			}
		}

		private enum SpawnType
		{
			Space,
			Atmosphere,
			Moon
		}

		protected sealed class StopSmoke_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				StopSmoke(entityId);
			}
		}

		protected sealed class PlayParticleBroadcast_003C_003ESystem_Int64_0023System_String_0023System_Boolean_0023System_UInt32 : ICallSite<IMyEventOwner, long, string, bool, uint, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in string particleName, in bool smoke, in uint renderId, in DBNull arg5, in DBNull arg6)
			{
				PlayParticleBroadcast(entityId, particleName, smoke, renderId);
			}
		}

		protected sealed class SpawnContainerDrop_003C_003ESystem_UInt64 : ICallSite<IMyEventOwner, ulong, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong steamId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SpawnContainerDrop(steamId);
			}
		}

		protected sealed class SetAllContainerDropsToExpireSoon_003C_003ESystem_UInt64 : ICallSite<IMyEventOwner, ulong, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong steamId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetAllContainerDropsToExpireSoon(steamId);
			}
		}

		protected sealed class ShowNotificationSync_003C_003ESystem_String_0023System_Int32_0023System_String_0023System_Int64 : ICallSite<IMyEventOwner, string, int, string, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string message, in int showTime, in string font, in long playerId, in DBNull arg5, in DBNull arg6)
			{
				ShowNotificationSync(message, showTime, font, playerId);
			}
		}

		protected sealed class RemoveGPS_003C_003ESystem_String_0023System_Int64 : ICallSite<IMyEventOwner, string, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string name, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveGPS(name, playerId);
			}
		}

		protected sealed class UpdateGPSRemainingTime_003C_003ESystem_String_0023System_Int32 : ICallSite<IMyEventOwner, string, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string gpsName, in int remainingTime, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateGPSRemainingTime(gpsName, remainingTime);
			}
		}

		protected sealed class CompetetiveContainerOpened_003C_003ESystem_String_0023System_Int32_0023System_Int64_0023VRageMath_Color : ICallSite<IMyEventOwner, string, int, long, Color, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string name, in int time, in long playerId, in Color color, in DBNull arg5, in DBNull arg6)
			{
				CompetetiveContainerOpened(name, time, playerId, color);
			}
		}

		protected sealed class RemoveContainerDropComponent_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveContainerDropComponent(entityId);
			}
		}

		private readonly Random random = new Random();

		private readonly int DESPAWN_SMOKE_TIME = 15;

		private static readonly short ONE_MINUTE = 60;

		private static readonly short TWO_MINUTES = 120;

		public const string DROP_TRIGGER_NAME = "Special Content";

		public const string DROP_DEPOWER_NAME = "Special Content Power";

		private static MySoundPair m_explosionSound = new MySoundPair("WepSmallWarheadExpl", useLog: false);

		private MyContainerDropSystemDefinition m_definition;

		private int m_counter;

		private uint m_containerIdSmall = 1u;

		private uint m_containerIdLarge = 1u;

		private List<MyContainerGPS> m_delayedGPSForRemoval;

		private List<MyEntityForRemoval> m_delayedEntitiesForRemoval;

		private List<MyPlayerContainerData> m_playerData = new List<MyPlayerContainerData>();

		private Dictionary<MyTuple<SpawnType, bool>, List<MyDropContainerDefinition>> m_dropContainerLists;

		private MyTuple<SpawnType, bool> m_keyPersonalSpace;

		private MyTuple<SpawnType, bool> m_keyPersonalAtmosphere;

		private MyTuple<SpawnType, bool> m_keyPersonalMoon;

		private MyTuple<SpawnType, bool> m_keyCompetetiveSpace;

		private MyTuple<SpawnType, bool> m_keyCompetetiveAtmosphere;

		private MyTuple<SpawnType, bool> m_keyCompetetiveMoon;

		private bool m_hasNewItems;

		private bool m_expireAll;

		private List<MyGameInventoryItem> m_newGameItems;

		private Dictionary<MyEntity, MyParticleEffect> m_smokeParticles = new Dictionary<MyEntity, MyParticleEffect>();

		private Dictionary<MyGps, MyEntityForRemoval> m_gpsList = new Dictionary<MyGps, MyEntityForRemoval>();

		private List<MyGps> m_gpsToRemove = new List<MyGps>();

		private bool m_nothingDropped;

		private bool m_enableWindowPopups = true;

		private int m_minDropContainerRespawnTime;

		private int m_maxDropContainerRespawnTime;

		private float m_DropContainerRespawnTimeMultiplier = 1f;

		public bool EnableWindowPopups
		{
			get
			{
				return m_enableWindowPopups;
			}
			set
			{
				m_enableWindowPopups = value;
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			if (Sync.IsServer)
			{
				MyObjectBuilder_SessionComponentContainerDropSystem myObjectBuilder_SessionComponentContainerDropSystem = sessionComponent as MyObjectBuilder_SessionComponentContainerDropSystem;
				if (myObjectBuilder_SessionComponentContainerDropSystem.GPSForRemoval == null)
				{
					m_delayedGPSForRemoval = new List<MyContainerGPS>();
				}
				else
				{
					m_delayedGPSForRemoval = myObjectBuilder_SessionComponentContainerDropSystem.GPSForRemoval;
				}
				if (myObjectBuilder_SessionComponentContainerDropSystem.EntitiesForRemoval == null)
				{
					m_delayedEntitiesForRemoval = new List<MyEntityForRemoval>();
				}
				else
				{
					m_delayedEntitiesForRemoval = myObjectBuilder_SessionComponentContainerDropSystem.EntitiesForRemoval;
				}
				m_containerIdSmall = myObjectBuilder_SessionComponentContainerDropSystem.ContainerIdSmall;
				m_containerIdLarge = myObjectBuilder_SessionComponentContainerDropSystem.ContainerIdLarge;
				if (myObjectBuilder_SessionComponentContainerDropSystem.PlayerData != null)
				{
					foreach (PlayerContainerData playerDatum in myObjectBuilder_SessionComponentContainerDropSystem.PlayerData)
					{
						m_playerData.Add(new MyPlayerContainerData(playerDatum.PlayerId, playerDatum.Timer, playerDatum.Active, playerDatum.Competetive, playerDatum.ContainerId));
					}
				}
			}
			if (MyGameService.IsActive)
			{
				MyGameService.ItemsAdded += MyGameService_ItemsAdded;
				MyGameService.NoItemsReceived += MyGameServiceNoItemsReceived;
			}
			m_minDropContainerRespawnTime = MySession.Static.MinDropContainerRespawnTime;
			m_maxDropContainerRespawnTime = MySession.Static.MaxDropContainerRespawnTime;
			if (m_minDropContainerRespawnTime > m_maxDropContainerRespawnTime)
			{
				MyLog.Default.WriteLine("MinDropContainerRespawnTime is higher than MaxDropContainerRespawnTime. Clamping to Max.");
				m_minDropContainerRespawnTime = m_maxDropContainerRespawnTime;
			}
		}

		protected override void UnloadData()
		{
			if (MyGameService.IsActive)
			{
				MyGameService.ItemsAdded -= MyGameService_ItemsAdded;
				MyGameService.NoItemsReceived -= MyGameServiceNoItemsReceived;
			}
			base.UnloadData();
		}

		private void MyGameServiceNoItemsReceived(object sender, EventArgs e)
		{
			m_nothingDropped = true;
		}

		private void MyGameService_ItemsAdded(object sender, MyGameItemsEventArgs e)
		{
			m_newGameItems = e.NewItems;
			m_hasNewItems = m_newGameItems != null && m_newGameItems.Count > 0;
<<<<<<< HEAD
			if (m_newGameItems != null && m_newGameItems.Count == 1)
=======
			if (m_newGameItems.Count == 1)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_newGameItems[0].IsNew = true;
			}
		}

		public override void InitFromDefinition(MySessionComponentDefinition definition)
		{
			base.InitFromDefinition(definition);
			m_definition = definition as MyContainerDropSystemDefinition;
			DictionaryReader<string, MyDropContainerDefinition> dropContainerDefinitions = MyDefinitionManager.Static.GetDropContainerDefinitions();
			m_dropContainerLists = new Dictionary<MyTuple<SpawnType, bool>, List<MyDropContainerDefinition>>();
			m_keyPersonalSpace = new MyTuple<SpawnType, bool>(SpawnType.Space, item2: false);
			m_keyPersonalAtmosphere = new MyTuple<SpawnType, bool>(SpawnType.Atmosphere, item2: false);
			m_keyPersonalMoon = new MyTuple<SpawnType, bool>(SpawnType.Moon, item2: false);
			m_keyCompetetiveSpace = new MyTuple<SpawnType, bool>(SpawnType.Space, item2: true);
			m_keyCompetetiveAtmosphere = new MyTuple<SpawnType, bool>(SpawnType.Atmosphere, item2: true);
			m_keyCompetetiveMoon = new MyTuple<SpawnType, bool>(SpawnType.Moon, item2: true);
			m_dropContainerLists[m_keyPersonalSpace] = new List<MyDropContainerDefinition>();
			m_dropContainerLists[m_keyPersonalAtmosphere] = new List<MyDropContainerDefinition>();
			m_dropContainerLists[m_keyPersonalMoon] = new List<MyDropContainerDefinition>();
			m_dropContainerLists[m_keyCompetetiveSpace] = new List<MyDropContainerDefinition>();
			m_dropContainerLists[m_keyCompetetiveAtmosphere] = new List<MyDropContainerDefinition>();
			m_dropContainerLists[m_keyCompetetiveMoon] = new List<MyDropContainerDefinition>();
			foreach (KeyValuePair<string, MyDropContainerDefinition> item in dropContainerDefinitions)
			{
				if (item.Value.Priority <= 0f || item.Value.Prefab == null)
<<<<<<< HEAD
				{
					continue;
				}
				if (item.Value.SpawnRules.CanBePersonal)
				{
=======
				{
					continue;
				}
				if (item.Value.SpawnRules.CanBePersonal)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (item.Value.SpawnRules.CanSpawnInSpace)
					{
						m_dropContainerLists[m_keyPersonalSpace].Add(item.Value);
					}
					if (item.Value.SpawnRules.CanSpawnInAtmosphere)
					{
						m_dropContainerLists[m_keyPersonalAtmosphere].Add(item.Value);
					}
					if (item.Value.SpawnRules.CanSpawnOnMoon)
					{
						m_dropContainerLists[m_keyPersonalMoon].Add(item.Value);
					}
				}
				if (item.Value.SpawnRules.CanBeCompetetive)
				{
					if (item.Value.SpawnRules.CanSpawnInSpace)
					{
						m_dropContainerLists[m_keyCompetetiveSpace].Add(item.Value);
					}
					if (item.Value.SpawnRules.CanSpawnInAtmosphere)
					{
						m_dropContainerLists[m_keyCompetetiveAtmosphere].Add(item.Value);
					}
					if (item.Value.SpawnRules.CanSpawnOnMoon)
					{
						m_dropContainerLists[m_keyCompetetiveMoon].Add(item.Value);
					}
				}
			}
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_SessionComponentContainerDropSystem myObjectBuilder_SessionComponentContainerDropSystem = base.GetObjectBuilder() as MyObjectBuilder_SessionComponentContainerDropSystem;
			myObjectBuilder_SessionComponentContainerDropSystem.PlayerData = new List<PlayerContainerData>();
			foreach (MyPlayerContainerData playerDatum in m_playerData)
			{
				myObjectBuilder_SessionComponentContainerDropSystem.PlayerData.Add(new PlayerContainerData(playerDatum.PlayerId, playerDatum.Timer, playerDatum.Active, playerDatum.Competetive, (playerDatum.Container != null) ? playerDatum.Container.EntityId : 0));
			}
			myObjectBuilder_SessionComponentContainerDropSystem.GPSForRemoval = m_delayedGPSForRemoval;
			myObjectBuilder_SessionComponentContainerDropSystem.EntitiesForRemoval = m_delayedEntitiesForRemoval;
			myObjectBuilder_SessionComponentContainerDropSystem.ContainerIdSmall = m_containerIdSmall;
			myObjectBuilder_SessionComponentContainerDropSystem.ContainerIdLarge = m_containerIdLarge;
			return myObjectBuilder_SessionComponentContainerDropSystem;
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			UpdateSmokeParticles();
			if (m_counter % 60 != 0)
			{
				return;
			}
			foreach (KeyValuePair<MyGps, MyEntityForRemoval> gps in m_gpsList)
			{
				if (gps.Value.TimeLeft > TWO_MINUTES)
				{
					if (gps.Value.TimeLeft % ONE_MINUTE == 59)
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateGPSRemainingTime, gps.Key.Name, gps.Value.TimeLeft);
					}
				}
				else
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateGPSRemainingTime, gps.Key.Name, gps.Value.TimeLeft);
				}
				if (gps.Value.TimeLeft <= 0)
<<<<<<< HEAD
				{
					m_gpsToRemove.Add(gps.Key);
				}
			}
			foreach (MyGps item in m_gpsToRemove)
			{
				m_gpsList.Remove(item);
			}
			m_gpsToRemove.Clear();
		}

		public void RemovePlayerDataForIdentity(long identityId)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			for (int i = 0; i < m_playerData.Count; i++)
			{
				if (m_playerData[i].PlayerId == identityId)
				{
=======
				{
					m_gpsToRemove.Add(gps.Key);
				}
			}
			foreach (MyGps item in m_gpsToRemove)
			{
				m_gpsList.Remove(item);
			}
			m_gpsToRemove.Clear();
		}

		public void RemovePlayerDataForIdentity(long identityId)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			for (int i = 0; i < m_playerData.Count; i++)
			{
				if (m_playerData[i].PlayerId == identityId)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_playerData.RemoveAt(i);
					i--;
				}
			}
		}

		private void UpdateSmokeParticles()
		{
			foreach (KeyValuePair<MyEntity, MyParticleEffect> smokeParticle in m_smokeParticles)
			{
				smokeParticle.Value.WorldMatrix = smokeParticle.Key.WorldMatrix;
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (!MySession.Static.EnableContainerDrops || MySandboxGame.IsPaused || m_counter++ % 60 != 0)
			{
				return;
			}
			if (EnableWindowPopups && m_hasNewItems && m_newGameItems != null)
			{
				m_hasNewItems = false;
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen<MyGuiScreenNewGameItems>(new object[1] { m_newGameItems }));
				m_newGameItems.Clear();
			}
			if (EnableWindowPopups && m_nothingDropped)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen<MyGuiScreenNoGameItemDrop>(Array.Empty<object>()));
				m_nothingDropped = false;
			}
			if (!Sync.IsServer)
			{
				return;
			}
			UpdateContainerSpawner();
			UpdateGPSRemoval();
			UpdateContainerEntityRemoval();
			int timer = MathHelper.RoundToInt((float)random.Next(m_minDropContainerRespawnTime, m_maxDropContainerRespawnTime) * m_DropContainerRespawnTimeMultiplier);
			if (m_playerData.Count == 0 && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_playerData.Add(new MyPlayerContainerData(MySession.Static.LocalPlayerId, timer, active: true, competetive: false, 0L));
			}
			if (m_counter < 3600)
			{
				return;
			}
			m_counter = 1;
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				bool flag = false;
				for (int i = 0; i < m_playerData.Count; i++)
				{
					if (m_playerData[i].PlayerId == onlinePlayer.Identity.IdentityId)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					m_playerData.Add(new MyPlayerContainerData(onlinePlayer.Identity.IdentityId, timer, active: true, competetive: false, 0L));
				}
			}
		}

		private void UpdateContainerSpawner()
		{
			for (int i = 0; i < m_playerData.Count; i++)
			{
				MyPlayerContainerData myPlayerContainerData = m_playerData[i];
				if (myPlayerContainerData.ContainerId != 0L)
				{
					MyEntity entity = null;
					MyEntities.TryGetEntityByName("Special Content", out entity);
					myPlayerContainerData.Container = entity as MyTerminalBlock;
					myPlayerContainerData.ContainerId = 0L;
				}
				if (myPlayerContainerData.Active)
				{
					myPlayerContainerData.Timer--;
					if (myPlayerContainerData.Timer <= 0)
					{
						bool flag = SpawnContainerDropInternal(myPlayerContainerData);
						int num = MathHelper.RoundToInt((float)random.Next(m_minDropContainerRespawnTime, m_maxDropContainerRespawnTime) * m_DropContainerRespawnTimeMultiplier);
						myPlayerContainerData.Timer = (flag ? num : ONE_MINUTE);
						myPlayerContainerData.Active = !flag || myPlayerContainerData.Competetive;
					}
				}
				else if (myPlayerContainerData.Container != null && (myPlayerContainerData.Container.Closed || !myPlayerContainerData.Container.InScene || !myPlayerContainerData.Container.Components.Contains(typeof(MyContainerDropComponent))))
				{
					myPlayerContainerData.Container = null;
					myPlayerContainerData.Active = true;
				}
			}
		}

		private void UpdateContainerEntityRemoval()
		{
			if (m_delayedEntitiesForRemoval != null)
			{
				for (int i = 0; i < m_delayedEntitiesForRemoval.Count; i++)
				{
<<<<<<< HEAD
					MyEntityForRemoval myEntityForRemoval = m_delayedEntitiesForRemoval[i];
					myEntityForRemoval.TimeLeft--;
					if (m_expireAll)
=======
					if (MyEntities.TryGetEntityById(myEntityForRemoval.EntityId, out var entity))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						myEntityForRemoval.TimeLeft = 20;
					}
					MyEntity entity2;
					if ((float)myEntityForRemoval.TimeLeft <= 0f)
					{
						if (MyEntities.TryGetEntityById(myEntityForRemoval.EntityId, out var entity))
						{
							MyCubeGrid myCubeGrid;
							if ((myCubeGrid = entity as MyCubeGrid) != null && myCubeGrid.OccupiedBlocks.Count > 0)
							{
								List<MyCockpit> list = new List<MyCockpit>();
								foreach (MyCockpit occupiedBlock in myCubeGrid.OccupiedBlocks)
								{
									MyCockpit current;
									if ((current = occupiedBlock) != null)
									{
										list.Add(current);
									}
								}
								foreach (MyCockpit item in list)
								{
									item.RemovePilot();
								}
							}
							MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => PlayParticleBroadcast, myEntityForRemoval.EntityId, "Explosion_Missile", arg4: false, entity.Render.ParentIDs[0]);
							MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => StopSmoke, myEntityForRemoval.EntityId);
							entity.Close();
						}
						m_delayedEntitiesForRemoval.RemoveAt(i);
						i--;
					}
					else if (myEntityForRemoval.TimeLeft == DESPAWN_SMOKE_TIME && MyEntities.TryGetEntityById(myEntityForRemoval.EntityId, out entity2) && !m_smokeParticles.ContainsKey(entity2))
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => PlayParticleBroadcast, myEntityForRemoval.EntityId, "Smoke_Container", arg4: true, entity2.Render.GetRenderObjectID());
					}
				}
			}
			if (m_expireAll)
			{
				m_expireAll = false;
			}
		}

<<<<<<< HEAD
		[Event(null, 447)]
=======
		[Event(null, 429)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void StopSmoke(long entityId)
		{
			if (MyEntities.TryGetEntityById(entityId, out var entity))
			{
				MySession.Static.GetComponent<MySessionComponentContainerDropSystem>().StopSmoke(entity);
			}
		}

		private void StopSmoke(MyEntity entity)
		{
			if (m_smokeParticles.ContainsKey(entity))
			{
				m_smokeParticles[entity].Stop();
				m_smokeParticles.Remove(entity);
			}
		}

<<<<<<< HEAD
		[Event(null, 467)]
=======
		[Event(null, 449)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void PlayParticleBroadcast(long entityId, string particleName, bool smoke, uint renderId = uint.MaxValue)
		{
			if (!MyEntities.TryGetEntityById(entityId, out var entity))
			{
				return;
			}
			MyParticleEffect myParticleEffect = PlayParticle(entity, particleName, renderId);
			if (smoke)
			{
				if (myParticleEffect != null)
				{
					MySession.Static.GetComponent<MySessionComponentContainerDropSystem>().AddSmoke(entity, myParticleEffect);
				}
				return;
			}
			MyEntity3DSoundEmitter myEntity3DSoundEmitter = MyAudioComponent.TryGetSoundEmitter();
			if (myEntity3DSoundEmitter != null)
			{
				myEntity3DSoundEmitter.SetPosition(entity.PositionComp.GetPosition());
				myEntity3DSoundEmitter.Entity = entity;
				myEntity3DSoundEmitter.PlaySound(m_explosionSound);
			}
		}

		private void AddSmoke(MyEntity entity, MyParticleEffect effect)
		{
			m_smokeParticles[entity] = effect;
		}

		private static MyParticleEffect PlayParticle(MyEntity entity, string particleName, uint renderId)
		{
			MyParticleEffect effect = null;
			MatrixD effectMatrix = entity.WorldMatrix;
			Vector3D worldPosition = entity.PositionComp.GetPosition();
			if (MyParticlesManager.TryCreateParticleEffect(particleName, ref effectMatrix, ref worldPosition, entity.Render.ParentIDs[0], out effect))
			{
				effect.Play();
			}
			return effect;
		}

		private void UpdateGPSRemoval()
		{
			if (m_delayedGPSForRemoval == null)
			{
				return;
			}
			for (int i = 0; i < m_delayedGPSForRemoval.Count; i++)
			{
				MyContainerGPS myContainerGPS = m_delayedGPSForRemoval[i];
				myContainerGPS.TimeLeft--;
				if ((float)myContainerGPS.TimeLeft <= 0f)
				{
					RemoveGPS(myContainerGPS.GPSName, 0L);
					m_delayedGPSForRemoval.RemoveAt(i);
					i--;
				}
			}
		}

		public void RegisterDelayedGPSRemovalInternal(string name, int time)
		{
			m_delayedGPSForRemoval.Add(new MyContainerGPS(time, name));
		}

		public void ContainerDestroyed(MyContainerDropComponent container)
		{
			if (!container.Competetive)
			{
				for (int i = 0; i < m_playerData.Count; i++)
				{
					if (m_playerData[i].PlayerId == container.Owner)
					{
						m_playerData[i].Active = true;
					}
				}
				RemoveGPS(container.GPSName, container.Owner);
			}
			else
			{
				RemoveGPS(container.GPSName, 0L);
			}
		}

		public void ContainerOpened(MyContainerDropComponent container, long playerId)
		{
			if (container.Entity == null)
			{
				return;
			}
			if (container.Competetive)
			{
				MyGameService.TriggerCompetitiveContainer();
				if (Sync.IsServer)
				{
					CompetetiveContainerOpened(container.GPSName, m_definition.CompetetiveContainerGPSTimeOut, playerId, m_definition.CompetetiveContainerGPSColorClaimed);
				}
				else
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CompetetiveContainerOpened, container.GPSName, m_definition.CompetetiveContainerGPSTimeOut, playerId, m_definition.CompetetiveContainerGPSColorClaimed);
				}
			}
			else
			{
				MyGameService.TriggerPersonalContainer();
				if (Sync.IsServer)
				{
					RemoveGPS(container.GPSName, container.Owner);
				}
				else
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RemoveGPS, container.GPSName, container.Owner);
				}
				for (int i = 0; i < m_playerData.Count; i++)
				{
					if (m_playerData[i].PlayerId == playerId)
					{
						m_playerData[i].Active = true;
					}
				}
			}
			if (container.Entity != null)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RemoveContainerDropComponent, container.Entity.EntityId);
			}
		}

		[Event(null, 597)]
		[Reliable]
		[Server]
		public static void SpawnContainerDrop(ulong steamId)
		{
			if (MySession.Static.IsUserAdmin(steamId) && Sync.IsServer)
			{
				MySessionComponentContainerDropSystem mySessionComponentContainerDropSystem = MySession.Static?.GetComponent<MySessionComponentContainerDropSystem>();
				for (int i = 0; i < mySessionComponentContainerDropSystem.m_playerData.Count; i++)
				{
					mySessionComponentContainerDropSystem.SpawnContainerDropInternal(mySessionComponentContainerDropSystem.m_playerData[i]);
				}
			}
		}

		[Event(null, 609)]
		[Reliable]
		[Server]
		public static void SetAllContainerDropsToExpireSoon(ulong steamId)
		{
			if (MySession.Static.IsUserAdmin(steamId) && Sync.IsServer)
			{
				(MySession.Static?.GetComponent<MySessionComponentContainerDropSystem>()).m_expireAll = true;
			}
		}

		private bool SpawnContainerDropInternal(MyPlayerContainerData playerData)
		{
			bool flag = false;
			ICollection<MyPlayer> players = Sync.Players.GetOnlinePlayers();
			Vector3D basePosition = Vector3D.Zero;
			foreach (MyPlayer item in players)
			{
				if (item.Identity.IdentityId == playerData.PlayerId && item.Controller.ControlledEntity != null)
				{
					flag = true;
					basePosition = item.Controller.ControlledEntity.Entity.PositionComp.GetPosition();
					break;
				}
			}
			if (!flag)
			{
				playerData.Competetive = true;
				return true;
			}
			bool personal = !Sync.MultiplayerActive || Sync.Players.GetOnlinePlayerCount() <= 1 || MyUtils.GetRandomFloat() <= 0.95f;
			playerData.Competetive = !personal;
			bool validSpawn;
			MyPlanet planet;
			Vector3D globalPos = FindNewSpawnPosition(personal, out validSpawn, out planet, basePosition);
			if (!validSpawn)
			{
				return false;
			}
			Vector3D gpsPosition = globalPos;
			Vector3D vector3D = MyGravityProviderSystem.CalculateNaturalGravityInPoint(gpsPosition);
			if (planet != null)
			{
				gpsPosition = planet.GetClosestSurfacePointGlobal(ref globalPos);
			}
			List<MyDropContainerDefinition> list = ((planet == null || vector3D == Vector3D.Zero) ? m_dropContainerLists[personal ? m_keyPersonalSpace : m_keyCompetetiveSpace] : ((!planet.HasAtmosphere) ? m_dropContainerLists[personal ? m_keyPersonalMoon : m_keyCompetetiveMoon] : m_dropContainerLists[personal ? m_keyPersonalAtmosphere : m_keyCompetetiveAtmosphere]));
			MyDropContainerDefinition myDropContainerDefinition = null;
			if (list.Count == 0)
			{
				return false;
			}
			if (list.Count == 1)
			{
				myDropContainerDefinition = list[0];
			}
			else
			{
				float num = 0f;
				foreach (MyDropContainerDefinition item2 in list)
				{
					num += item2.Priority;
				}
				float num2 = MyUtils.GetRandomFloat(0f, num);
				foreach (MyDropContainerDefinition item3 in list)
				{
					if (num2 <= item3.Priority)
					{
						myDropContainerDefinition = item3;
						break;
					}
					num2 -= item3.Priority;
				}
			}
			if (myDropContainerDefinition == null)
			{
				return false;
			}
			List<MyCubeGrid> resultGridList = new List<MyCubeGrid>();
			Action action = delegate
			{
				playerData.Container = null;
				MyCubeGrid myCubeGrid = ((resultGridList.Count > 0) ? resultGridList[0] : null);
				if (myCubeGrid != null)
				{
					foreach (MyTerminalBlock fatBlock in myCubeGrid.GetFatBlocks<MyTerminalBlock>())
					{
						if (fatBlock != null && ((fatBlock.CustomName != null) ? fatBlock.CustomName.ToString() : string.Empty).Equals("Special Content"))
						{
							playerData.Container = fatBlock;
							break;
						}
					}
				}
				if (myCubeGrid == null || playerData.Container == null)
				{
					playerData.Active = true;
				}
				else
				{
					myCubeGrid.IsRespawnGrid = true;
					MyEntityForRemoval myEntityForRemoval = new MyEntityForRemoval(playerData.Competetive ? m_definition.CompetetiveContainerGridTimeOut : m_definition.PersonalContainerGridTimeOut, myCubeGrid.EntityId);
					m_delayedEntitiesForRemoval.Add(myEntityForRemoval);
					string text = (playerData.Competetive ? MyTexts.GetString(MySpaceTexts.ContainerDropSystemContainerLarge) : MyTexts.GetString(MySpaceTexts.ContainerDropSystemContainerSmall));
					string text2 = string.Format(MyTexts.GetString(MySpaceTexts.ContainerDropSystemContainerWasDetected), text);
					string text3 = text + " ";
					if (personal)
					{
						text3 += m_containerIdSmall;
						m_containerIdSmall++;
					}
					else
					{
						text3 += m_containerIdLarge;
						m_containerIdLarge++;
					}
					MyContainerDropComponent component = new MyContainerDropComponent(playerData.Competetive, text3, playerData.PlayerId, m_definition.ContainerAudioCue)
					{
						GridEntityId = myCubeGrid.EntityId
					};
					playerData.Container.Components.Add(typeof(MyContainerDropComponent), component);
					playerData.Container.ChangeOwner(0L, MyOwnershipShareModeEnum.All);
					List<long> list2 = new List<long>();
					if (playerData.Competetive)
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowNotificationSync, text2, 5000, "Blue".ToString(), 0L);
						foreach (MyPlayer item4 in players)
						{
							list2.Add(item4.Identity.IdentityId);
						}
					}
					else
					{
						ShowNotificationSync(text2, 5000, "Blue".ToString(), playerData.PlayerId);
						list2.Add(playerData.PlayerId);
					}
					Color gPSColor = (playerData.Competetive ? m_definition.CompetetiveContainerGPSColorFree : m_definition.PersonalContainerGPSColor);
<<<<<<< HEAD
					foreach (long item7 in list2)
=======
					foreach (long item5 in list2)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MyGps gps = new MyGps
						{
							ShowOnHud = true,
							Name = text3,
							DisplayName = text,
							DiscardAt = null,
							Coords = gpsPosition,
							Description = "",
							AlwaysVisible = true,
							GPSColor = gPSColor,
							IsContainerGPS = true
						};
						m_gpsList.Add(gps, myEntityForRemoval);
						MySession.Static.Gpss.SendAddGps(item5, ref gps, playerData.Container.EntityId);
					}
				}
			};
			Action action2 = delegate
			{
				foreach (MyCubeGrid item6 in resultGridList)
				{
					foreach (MyCubeBlock fatBlock2 in item6.GetFatBlocks())
					{
						MyTerminalBlock myTerminalBlock = fatBlock2 as MyTerminalBlock;
						if (myTerminalBlock != null && myTerminalBlock.CustomName.ToString() != "Special Content")
						{
							myTerminalBlock.SetCustomName("Special Content Power");
						}
					}
				}
			};
<<<<<<< HEAD
			Stack<Action> stack = new Stack<Action>();
			stack.Push(item2);
			stack.Push(item);
=======
			Stack<Action> val = new Stack<Action>();
			val.Push(action2);
			val.Push(action);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Vector3 vector = ((vector3D != Vector3.Zero) ? (Vector3.Normalize(vector3D) * -1f) : Vector3.Normalize(MyUtils.GetRandomVector3()));
			Vector3 vector2 = ((vector != Vector3.Left && vector != Vector3.Right) ? Vector3.Right : Vector3.Forward);
			Vector3 forward = Vector3.Normalize(Vector3.Cross(vector, vector2));
			MyPrefabManager @static = MyPrefabManager.Static;
			List<MyCubeGrid> resultList = resultGridList;
			string subtypeName = myDropContainerDefinition.Prefab.Id.SubtypeName;
			Vector3D position = globalPos;
			string @string = MyTexts.GetString(MySpaceTexts.ContainerDropSystemBeaconText);
			Stack<Action> callbacks = val;
			@static.SpawnPrefab(resultList, subtypeName, position, forward, vector, vector3D, default(Vector3), @string, null, SpawningOptions.SpawnRandomCargo, 0L, updateSync: true, callbacks);
			return true;
		}

		private Vector3D FindNewSpawnPosition(bool personal, out bool validSpawn, out MyPlanet planet, Vector3D basePosition)
		{
			validSpawn = false;
			planet = null;
			Vector3D vector3D = Vector3D.Zero;
			float minValue = (personal ? m_definition.PersonalContainerDistMin : m_definition.CompetetiveContainerDistMin);
			float maxValue = (personal ? m_definition.PersonalContainerDistMax : m_definition.CompetetiveContainerDistMax);
			for (int num = 15; num > 0; num--)
			{
				vector3D = MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(minValue, maxValue) + basePosition;
				if (IsSpawnPositionFree(vector3D, 50.0))
				{
					if (!(MyGravityProviderSystem.CalculateNaturalGravityInPoint(vector3D) != Vector3D.Zero))
					{
						validSpawn = true;
						break;
					}
					planet = MyGamePruningStructure.GetClosestPlanet(vector3D);
					vector3D = GetPlanetarySpawnPosition(vector3D, planet);
					if (IsSpawnPositionFree(vector3D, 50.0))
					{
						validSpawn = true;
						break;
					}
				}
			}
			return vector3D;
		}

		private bool IsSpawnPositionFree(Vector3D position, double size)
		{
			BoundingSphereD sphere = new BoundingSphereD(position, size);
			List<MyEntity> list = new List<MyEntity>();
			MyGamePruningStructure.GetAllEntitiesInSphere(ref sphere, list);
			bool result = true;
			foreach (MyEntity item in list)
			{
				if (!(item is MyPlanet))
				{
					return false;
				}
			}
			return result;
		}

		private Vector3D GetPlanetarySpawnPosition(Vector3D position, MyPlanet planet)
		{
			if (planet == null)
			{
				return position;
			}
			Vector3D value = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
			return planet.GetClosestSurfacePointGlobal(ref position) - Vector3D.Normalize(value) * (planet.HasAtmosphere ? 2000 : 10);
		}

<<<<<<< HEAD
		[Event(null, 896)]
=======
		[Event(null, 853)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void ShowNotificationSync(string message, int showTime, string font, long playerId)
		{
			if (Sync.IsValidEventOnServer && !MyEventContext.Current.IsLocallyInvoked)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				MyEventContext.ValidationFailed();
			}
			else if (MyAPIGateway.Utilities != null && (playerId == 0L || playerId == MySession.Static.LocalPlayerId))
			{
				MyAPIGateway.Utilities.ShowNotification(message, showTime, font);
			}
		}

		public static void ModifyGPSColorForAll(string name, Color color)
		{
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				MyGps myGps = MySession.Static.Gpss.GetGpsByName(onlinePlayer.Identity.IdentityId, name) as MyGps;
				if (myGps != null)
				{
					myGps.GPSColor = color;
					MySession.Static.Gpss.SendModifyGps(onlinePlayer.Identity.IdentityId, myGps);
				}
			}
		}

<<<<<<< HEAD
		[Event(null, 925)]
=======
		[Event(null, 882)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public static void RemoveGPS(string name, long playerId = 0L)
		{
			if (playerId == 0L)
			{
				foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
				{
					IMyGps gpsByName = MySession.Static.Gpss.GetGpsByName(onlinePlayer.Identity.IdentityId, name);
					if (gpsByName != null)
					{
						MySession.Static.Gpss.SendDelete(onlinePlayer.Identity.IdentityId, gpsByName.Hash);
					}
				}
			}
			else
			{
				IMyGps gpsByName2 = MySession.Static.Gpss.GetGpsByName(playerId, name);
				if (gpsByName2 != null)
				{
					MySession.Static.Gpss.SendDelete(playerId, gpsByName2.Hash);
				}
			}
		}

<<<<<<< HEAD
		[Event(null, 946)]
=======
		[Event(null, 903)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		public static void UpdateGPSRemainingTime(string gpsName, int remainingTime)
		{
			if (Sync.IsServer && !MyEventContext.Current.IsLocallyInvoked)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				MyEventContext.ValidationFailed();
				return;
			}
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				IMyGps gpsByName = MySession.Static.Gpss.GetGpsByName(onlinePlayer.Identity.IdentityId, gpsName);
				if (gpsByName != null)
				{
					string empty = string.Empty;
					if (remainingTime >= TWO_MINUTES)
					{
						int num = remainingTime / ONE_MINUTE;
						empty = string.Format(MyTexts.GetString(MyCommonTexts.GpsContainerRemainingTimeMins), num);
					}
					else if (remainingTime < ONE_MINUTE)
					{
						empty = ((remainingTime <= 1 || remainingTime >= ONE_MINUTE) ? string.Format(MyTexts.GetString(MyCommonTexts.GpsContainerRemainingTimeSec), remainingTime) : string.Format(MyTexts.GetString(MyCommonTexts.GpsContainerRemainingTimeSecs), remainingTime));
					}
					else
					{
						int num2 = remainingTime / ONE_MINUTE;
						int num3 = remainingTime % ONE_MINUTE;
						empty = ((num3 != 1) ? string.Format(MyTexts.GetString(MyCommonTexts.GpsContainerRemainingTimeMinSecs), num2, num3) : string.Format(MyTexts.GetString(MyCommonTexts.GpsContainerRemainingTimeMinSec), num2, num3));
					}
					gpsByName.ContainerRemainingTime = empty;
				}
			}
		}

<<<<<<< HEAD
		[Event(null, 997)]
=======
		[Event(null, 954)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public static void CompetetiveContainerOpened(string name, int time, long playerId, Color color)
		{
			RemoveGPS(name, playerId);
			MySession.Static.GetComponent<MySessionComponentContainerDropSystem>().RegisterDelayedGPSRemovalInternal(name, time);
			ModifyGPSColorForAll(name, color);
		}

<<<<<<< HEAD
		[Event(null, 1005)]
=======
		[Event(null, 962)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void RemoveContainerDropComponent(long entityId)
		{
			if (!MyEntities.TryGetEntityById(entityId, out var entity))
			{
				return;
			}
			MyContainerDropComponent myContainerDropComponent = entity.Components.Get<MyContainerDropComponent>();
			MySessionComponentContainerDropSystem component = MySession.Static.GetComponent<MySessionComponentContainerDropSystem>();
			if (component != null && myContainerDropComponent != null)
			{
				component.RemoveDelayedRemovalEntity(myContainerDropComponent.GridEntityId);
				MyEntity entity2;
				if (myContainerDropComponent.GridEntityId == 0L)
				{
					MyCubeBlock myCubeBlock = entity as MyCubeBlock;
					if (myCubeBlock != null && myCubeBlock.CubeGrid != null)
					{
						myCubeBlock.CubeGrid.ChangePowerProducerState(MyMultipleEnabledEnum.AllDisabled, -2L);
					}
				}
				else if (MyEntities.TryGetEntityById(myContainerDropComponent.GridEntityId, out entity2))
				{
					(entity2 as MyCubeGrid)?.ChangePowerProducerState(MyMultipleEnabledEnum.AllDisabled, -2L);
				}
			}
			if (entity.Components != null)
			{
				entity.Components.Remove<MyContainerDropComponent>();
			}
		}

		private void RemoveDelayedRemovalEntity(long entityId)
		{
			if (m_delayedEntitiesForRemoval != null)
			{
				MyEntityForRemoval myEntityForRemoval = Enumerable.FirstOrDefault<MyEntityForRemoval>((IEnumerable<MyEntityForRemoval>)m_delayedEntitiesForRemoval, (Func<MyEntityForRemoval, bool>)((MyEntityForRemoval e) => e.EntityId == entityId));
				if (myEntityForRemoval != null)
				{
					m_delayedEntitiesForRemoval.Remove(myEntityForRemoval);
				}
			}
		}

		[Conditional("DEBUG")]
		public void SetRespawnTimeMultiplier(float multiplier = 1f)
		{
			if (!(Math.Abs(multiplier) < 1E-05f))
			{
				m_DropContainerRespawnTimeMultiplier = multiplier;
			}
		}

		public float GetRespawnTimeMultiplier()
		{
			return m_DropContainerRespawnTimeMultiplier;
		}
	}
}
