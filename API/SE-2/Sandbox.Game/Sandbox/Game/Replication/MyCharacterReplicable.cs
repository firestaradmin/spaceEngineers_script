using System;
using System.Collections.Generic;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Game.World;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.Replication
{
	internal class MyCharacterReplicable : MyEntityReplicableBaseEvent<MyCharacter>
	{
		private MyPropertySyncStateGroup m_propertySync;

		private readonly HashSet<IMyReplicable> m_dependencies = new HashSet<IMyReplicable>();

		private readonly HashSet<VRage.Game.ModAPI.Ingame.IMyEntity> m_dependencyParents = new HashSet<VRage.Game.ModAPI.Ingame.IMyEntity>();

		private long m_ownerId;

		private long m_characterId;

		public HashSetReader<VRage.Game.ModAPI.Ingame.IMyEntity> CachedParentDependencies => new HashSetReader<VRage.Game.ModAPI.Ingame.IMyEntity>(m_dependencyParents);

		public override bool HasToBeChild => base.Instance.Parent != null;

		protected override IMyStateGroup CreatePhysicsGroup()
		{
			return new MyCharacterPhysicsStateGroup(base.Instance, this);
		}

		protected override void OnHook()
		{
			base.OnHook();
			if (base.Instance.Closed)
			{
				MyLog.Default.Error("Character is closed upon hooking in client.");
			}
			else if (base.Instance != null)
			{
				m_propertySync = new MyPropertySyncStateGroup(this, base.Instance.SyncType)
				{
					GlobalValidate = (MyEventContext context) => HasRights(context.ClientState.EndpointId.Id, ValidationType.Controlled)
				};
				base.Instance.Hierarchy.OnParentChanged += OnParentChanged;
			}
		}

		private void OnParentChanged(MyHierarchyComponentBase oldParent, MyHierarchyComponentBase newParent)
		{
			if (IsReadyForReplication)
			{
				(MyMultiplayer.ReplicationLayer as MyReplicationLayer).RefreshReplicableHierarchy(this);
			}
		}

		public override void GetStateGroups(List<IMyStateGroup> resultList)
		{
			base.GetStateGroups(resultList);
			if (m_propertySync != null && m_propertySync.PropertyCount > 0)
			{
				resultList.Add(m_propertySync);
			}
		}

		public override IMyReplicable GetParent()
		{
			if (base.Instance == null)
			{
				return null;
			}
			if (base.Instance.Parent != null)
			{
				return MyExternalReplicable.FindByObject(base.Instance.GetTopMostParent());
			}
			return null;
		}

		public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
		{
			if (base.Instance == null)
			{
				return false;
			}
			stream.WriteBool(base.Instance.IsUsing is MyShipController);
			if (base.Instance.IsUsing is MyShipController)
			{
				long value = base.Instance.IsUsing.EntityId;
				MySerializer.Write(stream, ref value);
				long value2 = base.Instance.EntityId;
				MySerializer.Write(stream, ref value2);
			}
			else
			{
				MyObjectBuilder_Character value3;
				using (MyReplicationLayer.StartSerializingReplicable(this, clientEndpoint))
				{
					value3 = (MyObjectBuilder_Character)base.Instance.GetObjectBuilder();
				}
				MySerializer.Write(stream, ref value3, MyObjectBuilderSerializer.Dynamic);
			}
			return true;
		}

		protected override void OnLoad(BitStream stream, Action<MyCharacter> loadingDoneHandler)
		{
			bool value = true;
			if (stream != null)
			{
				MySerializer.CreateAndRead<bool>(stream, out value);
			}
			if (value)
			{
				if (stream != null)
				{
					MySerializer.CreateAndRead<long>(stream, out m_ownerId);
					MySerializer.CreateAndRead<long>(stream, out m_characterId);
				}
				MyEntities.CallAsync(delegate
				{
					LoadAsync(m_ownerId, m_characterId, loadingDoneHandler);
				});
			}
			else
			{
				MyObjectBuilder_Character myObjectBuilder_Character = (MyObjectBuilder_Character)MySerializer.CreateAndRead<MyObjectBuilder_EntityBase>(stream, MyObjectBuilderSerializer.Dynamic);
				TryRemoveExistingEntity(myObjectBuilder_Character.EntityId);
				MyCharacter character = MyEntities.CreateFromObjectBuilderNoinit(myObjectBuilder_Character) as MyCharacter;
				MyEntities.InitAsync(character, myObjectBuilder_Character, addToScene: true, delegate
				{
					loadingDoneHandler(character);
				});
			}
		}

		private static void LoadAsync(long ownerId, long characterId, Action<MyCharacter> loadingDoneHandler)
		{
			MyEntities.TryGetEntityById(ownerId, out var entity);
			MyShipController myShipController = entity as MyShipController;
			if (myShipController != null)
			{
				if (myShipController.Pilot != null)
				{
					loadingDoneHandler(myShipController.Pilot);
					MySession.Static.Players.UpdatePlayerControllers(ownerId);
				}
				else
				{
					MyEntities.TryGetEntityById(characterId, out var entity2);
					MyCharacter obj = entity2 as MyCharacter;
					loadingDoneHandler(obj);
				}
			}
			else
			{
				loadingDoneHandler(null);
			}
		}

		public override HashSet<IMyReplicable> GetDependencies(bool forPlayer)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			if (!forPlayer)
			{
				return null;
			}
			m_dependencies.Clear();
			m_dependencyParents.Clear();
			if (!Sync.IsServer)
			{
				return m_dependencies;
			}
			Enumerator<MyDataBroadcaster> enumerator = MyAntennaSystem.Static.GetAllRelayedBroadcasters(base.Instance, base.Instance.GetPlayerIdentityId(), mutual: false).GetEnumerator();
			try
			{
<<<<<<< HEAD
				if (base.Instance.RadioBroadcaster == allRelayedBroadcaster || allRelayedBroadcaster.Closed)
				{
					continue;
				}
				MyFarBroadcasterReplicable myFarBroadcasterReplicable = MyExternalReplicable.FindByObject(allRelayedBroadcaster) as MyFarBroadcasterReplicable;
				if (myFarBroadcasterReplicable == null)
				{
					continue;
				}
				m_dependencies.Add(myFarBroadcasterReplicable);
				if (myFarBroadcasterReplicable.Instance != null && myFarBroadcasterReplicable.Instance.Entity != null)
				{
					VRage.ModAPI.IMyEntity topMostParent = myFarBroadcasterReplicable.Instance.Entity.GetTopMostParent();
					if (topMostParent != null)
					{
						m_dependencyParents.Add(topMostParent);
=======
				while (enumerator.MoveNext())
				{
					MyDataBroadcaster current = enumerator.get_Current();
					if (base.Instance.RadioBroadcaster == current || current.Closed)
					{
						continue;
					}
					MyFarBroadcasterReplicable myFarBroadcasterReplicable = MyExternalReplicable.FindByObject(current) as MyFarBroadcasterReplicable;
					if (myFarBroadcasterReplicable == null)
					{
						continue;
					}
					m_dependencies.Add((IMyReplicable)myFarBroadcasterReplicable);
					if (myFarBroadcasterReplicable.Instance != null && myFarBroadcasterReplicable.Instance.Entity != null)
					{
						VRage.ModAPI.IMyEntity topMostParent = myFarBroadcasterReplicable.Instance.Entity.GetTopMostParent();
						if (topMostParent != null)
						{
							m_dependencyParents.Add((VRage.Game.ModAPI.Ingame.IMyEntity)topMostParent);
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return m_dependencies;
		}

		public override ValidationResult HasRights(EndpointId endpointId, ValidationType validationFlags)
		{
			bool flag = true;
			if (validationFlags.HasFlag(ValidationType.Controlled))
			{
				flag &= endpointId.Value == base.Instance.GetClientIdentity().SteamId;
			}
			if (!flag)
			{
				return ValidationResult.Kick | ValidationResult.Controlled;
			}
			return ValidationResult.Passed;
		}

		public override bool ShouldReplicate(MyClientInfo client)
		{
			return !base.Instance.IsDead;
		}
	}
}
