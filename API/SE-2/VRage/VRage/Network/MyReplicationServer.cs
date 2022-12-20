using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Replication;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Network
{
	public class MyReplicationServer : MyReplicationLayer
	{
		internal class MyDestroyBlocker
		{
			public bool Remove;

			public bool IsProcessing;

			public readonly List<IMyReplicable> Blockers = new List<IMyReplicable>();
		}

		private const int MAX_NUM_STATE_SYNC_PACKETS_PER_CLIENT = 7;

		private readonly IReplicationServerCallback m_callback;

		private readonly CacheList<IMyStateGroup> m_tmpGroups = new CacheList<IMyStateGroup>(4);

		private HashSet<IMyReplicable> m_toRecalculateHash = new HashSet<IMyReplicable>();

		private readonly List<IMyReplicable> m_tmpReplicableList = new List<IMyReplicable>();

		private readonly HashSet<IMyReplicable> m_tmpReplicableDepsList = new HashSet<IMyReplicable>();

		private readonly CacheList<IMyReplicable> m_tmp = new CacheList<IMyReplicable>();

		private readonly CacheList<IMyReplicable> m_tmpAdd = new CacheList<IMyReplicable>();

		private readonly HashSet<IMyReplicable> m_lastLayerAdditions = new HashSet<IMyReplicable>();

		private readonly CachingHashSet<IMyReplicable> m_postponedDestructionReplicables = new CachingHashSet<IMyReplicable>();

		private readonly ConcurrentCachingHashSet<IMyReplicable> m_priorityUpdates = new ConcurrentCachingHashSet<IMyReplicable>();

		private MyTimeSpan m_serverTimeStamp = MyTimeSpan.Zero;

		private long m_serverFrame;

		private readonly ConcurrentQueue<IMyStateGroup> m_dirtyGroups = new ConcurrentQueue<IMyStateGroup>();

		public static SerializableVector3I StressSleep = new SerializableVector3I(0, 0, 0);

		/// <summary>
		/// All replicables on server.
		/// </summary>
		/// <summary>
		/// All replicable state groups.
		/// </summary>
		private readonly Dictionary<IMyReplicable, List<IMyStateGroup>> m_replicableGroups = new Dictionary<IMyReplicable, List<IMyStateGroup>>();

		/// <summary>
		/// Network objects and states which are actively replicating to clients.
		/// </summary>
		private readonly ConcurrentDictionary<Endpoint, MyClient> m_clientStates = new ConcurrentDictionary<Endpoint, MyClient>();

		/// <summary>
		/// Clients that recently disconnected are saved here for some time so that the server doesn't freak out in case some calls are still pending for them
		/// </summary>
		private readonly ConcurrentDictionary<Endpoint, MyTimeSpan> m_recentClientsStates = new ConcurrentDictionary<Endpoint, MyTimeSpan>();

		private readonly HashSet<Endpoint> m_recentClientStatesToRemove = new HashSet<Endpoint>();

		private readonly MyTimeSpan SAVED_CLIENT_DURATION = MyTimeSpan.FromSeconds(60.0);

		[ThreadStatic]
		private static List<EndpointId> m_recipients;

		private readonly List<EndpointId> m_endpoints = new List<EndpointId>();

		public MyReplicationServer(IReplicationServerCallback callback, EndpointId localEndpoint, Thread mainThread)
			: base(isNetworkAuthority: true, localEndpoint, mainThread)
		{
			m_callback = callback;
			m_replicables = new MyReplicablesAABB(mainThread);
			m_replicables.OnChildAdded += OnReplicableChildAdded;
		}

		private void OnReplicableChildAdded(IMyReplicable child)
		{
<<<<<<< HEAD
			foreach (MyClient value2 in m_clientStates.Values)
=======
			foreach (MyClient value2 in m_clientStates.get_Values())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (value2.ReplicableToLayer.TryGetValue(child, out var value))
				{
					value.UpdateTimer = 0;
				}
			}
		}

		protected override MyPacketDataBitStreamBase GetBitStreamPacketData()
		{
			return m_callback.GetBitStreamPacketData();
		}

		public void Replicate(IMyReplicable obj)
		{
			if (!IsTypeReplicated(obj.GetType()))
			{
				return;
			}
			if (!obj.IsReadyForReplication)
			{
				obj.ReadyForReplicationAction.Add(obj, delegate
				{
					Replicate(obj);
				});
				return;
			}
			AddNetworkObjectServer(obj);
			m_replicables.Add(obj, out var _);
			AddStateGroups(obj);
			if (obj.PriorityUpdate)
			{
				m_priorityUpdates.Add(obj);
			}
		}

		/// <summary>
		/// Hack to allow thing like: CreateCharacter, Respawn sent from server
		/// </summary>
		public void ForceReplicable(IMyReplicable obj, IMyReplicable parent = null)
		{
			if (obj == null)
			{
				return;
			}
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				if ((parent == null || clientState.Value.Replicables.ContainsKey(parent)) && !clientState.Value.Replicables.ContainsKey(obj))
				{
					RefreshReplicable(obj, clientState.Key, clientState.Value, force: true);
				}
			}
		}

		/// <summary>
		/// Hack to allow thing like: CreateCharacter, Respawn sent from server
		/// </summary>
		private void ForceReplicable(IMyReplicable obj, Endpoint clientEndpoint)
		{
			if (!(m_localEndpoint == clientEndpoint.Id) && !clientEndpoint.Id.IsNull && obj != null && m_clientStates.ContainsKey(clientEndpoint))
			{
				MyClient myClient = m_clientStates.get_Item(clientEndpoint);
				if (!myClient.Replicables.ContainsKey(obj))
				{
					AddForClient(obj, clientEndpoint, myClient, force: true);
				}
			}
		}

		public void RemoveForClientIfIncomplete(IMyEventProxy objA)
		{
			if (objA != null)
			{
				IMyReplicable replicableA = GetProxyTarget(objA) as IMyReplicable;
				RemoveForClients(replicableA, (MyClient x) => x.IsReplicablePending(replicableA), sendDestroyToClient: true);
			}
		}

		/// <summary>
		/// Sends everything in the world to client. Use with extreme caution!
		/// </summary>
		public void ForceEverything(Endpoint clientEndpoint)
		{
			m_replicables.IterateRoots(delegate(IMyReplicable replicable)
			{
				ForceReplicable(replicable, clientEndpoint);
			});
		}

		public void Destroy(IMyReplicable obj)
		{
			if (!IsTypeReplicated(obj.GetType()) || !obj.IsReadyForReplication || (obj.ReadyForReplicationAction != null && obj.ReadyForReplicationAction.Count > 0) || GetNetworkIdByObject(obj).IsInvalid)
<<<<<<< HEAD
			{
				return;
			}
			m_priorityUpdates.Remove(obj);
			m_priorityUpdates.ApplyChanges();
			bool isAnyClientPending = false;
			bool flag = obj.GetParent() != null && !obj.GetParent().IsValid;
			RemoveForClients(obj, delegate(MyClient client)
			{
=======
			{
				return;
			}
			m_priorityUpdates.Remove(obj);
			m_priorityUpdates.ApplyChanges();
			bool isAnyClientPending = false;
			bool flag = obj.GetParent() != null && !obj.GetParent().IsValid;
			RemoveForClients(obj, delegate(MyClient client)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (client.BlockedReplicables.ContainsKey(obj))
				{
					client.BlockedReplicables[obj].Remove = true;
					if (!obj.HasToBeChild && !m_priorityUpdates.Contains(obj))
					{
						m_priorityUpdates.Add(obj);
					}
					isAnyClientPending = true;
					return false;
				}
				client.PermanentReplicables.Remove(obj);
				client.CrucialReplicables.Remove(obj);
				return true;
			}, !flag);
			m_replicables.RemoveHierarchy(obj);
			if (!isAnyClientPending)
			{
				RemoveStateGroups(obj);
				RemoveNetworkedObject(obj);
				m_postponedDestructionReplicables.Remove(obj);
				obj.OnRemovedFromReplication();
			}
			else
			{
				m_postponedDestructionReplicables.Add(obj);
			}
		}

		/// <summary>
		/// Destroys replicable for all clients (used for testing and debugging).
		/// </summary>
		public void ResetForClients(IMyReplicable obj)
		{
			RemoveForClients(obj, (MyClient client) => client.Replicables.ContainsKey(obj), sendDestroyToClient: true);
		}

		public void AddClient(Endpoint endpoint, MyClientStateBase clientState)
		{
			if (!m_clientStates.ContainsKey(endpoint))
			{
				clientState.EndpointId = endpoint;
				m_clientStates.TryAdd(endpoint, new MyClient(clientState, m_callback));
			}
		}

		private void OnClientConnected(EndpointId endpointId, MyClientStateBase clientState)
		{
			AddClient(new Endpoint(endpointId, 0), clientState);
		}

		public void OnClientReady(Endpoint endpointId, ref ClientReadyDataMsg msg)
		{
<<<<<<< HEAD
			if (m_clientStates.TryGetValue(endpointId, out var value))
			{
				value.IsReady = true;
				value.ForcePlayoutDelayBuffer = msg.ForcePlayoutDelayBuffer;
				value.UsePlayoutDelayBufferForCharacter = msg.UsePlayoutDelayBufferForCharacter;
				value.UsePlayoutDelayBufferForJetpack = msg.UsePlayoutDelayBufferForJetpack;
				value.UsePlayoutDelayBufferForGrids = msg.UsePlayoutDelayBufferForGrids;
=======
			MyClient myClient = default(MyClient);
			if (m_clientStates.TryGetValue(endpointId, ref myClient))
			{
				myClient.IsReady = true;
				myClient.ForcePlayoutDelayBuffer = msg.ForcePlayoutDelayBuffer;
				myClient.UsePlayoutDelayBufferForCharacter = msg.UsePlayoutDelayBufferForCharacter;
				myClient.UsePlayoutDelayBufferForJetpack = msg.UsePlayoutDelayBufferForJetpack;
				myClient.UsePlayoutDelayBufferForGrids = msg.UsePlayoutDelayBufferForGrids;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void OnClientReady(Endpoint endpointId, MyPacket packet)
		{
			ClientReadyDataMsg msg = MySerializer.CreateAndRead<ClientReadyDataMsg>(packet.BitStream);
			OnClientReady(endpointId, ref msg);
			SendServerData(endpointId);
		}

		private void SendServerData(Endpoint endpointId)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			SerializeTypeTable(bitStreamPacketData.Stream);
			m_callback.SendServerData(bitStreamPacketData, endpointId);
		}

		public void OnClientLeft(EndpointId endpointId)
		{
			bool flag;
			do
			{
				flag = false;
				foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
				{
					if (clientState.Key.Id == endpointId)
					{
						flag = true;
						RemoveClient(clientState.Key);
						break;
					}
				}
			}
			while (flag);
		}

		private void RemoveClient(Endpoint endpoint)
		{
<<<<<<< HEAD
			if (m_clientStates.TryGetValue(endpoint, out var value))
=======
			MyClient myClient = default(MyClient);
			if (m_clientStates.TryGetValue(endpoint, ref myClient))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				while (myClient.Replicables.Count > 0)
				{
					RemoveForClient(myClient.Replicables.FirstPair().Key, myClient, sendDestroyToClient: false);
				}
				m_clientStates.Remove<Endpoint, MyClient>(endpoint);
				m_recentClientsStates.set_Item(endpoint, m_callback.GetUpdateTime() + SAVED_CLIENT_DURATION);
			}
		}

		public override void Disconnect()
		{
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				RemoveClient(clientState.Key);
			}
		}

		public override void SetPriorityMultiplier(EndpointId id, float priority)
		{
<<<<<<< HEAD
			if (m_clientStates.TryGetValue(new Endpoint(id, 0), out var value))
=======
			MyClient myClient = default(MyClient);
			if (m_clientStates.TryGetValue(new Endpoint(id, 0), ref myClient))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				myClient.PriorityMultiplier = priority;
			}
		}

		public void OnClientJoined(EndpointId endpointId, MyClientStateBase clientState)
		{
			OnClientConnected(endpointId, clientState);
		}

		public void OnClientAcks(MyPacket packet)
		{
<<<<<<< HEAD
			if (!m_clientStates.TryGetValue(packet.Sender, out var value))
=======
			MyClient myClient = default(MyClient);
			if (!m_clientStates.TryGetValue(packet.Sender, ref myClient))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				packet.Return();
				return;
			}
<<<<<<< HEAD
			value.OnClientAcks(packet);
=======
			myClient.OnClientAcks(packet);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			packet.Return();
		}

		public void OnClientUpdate(MyPacket packet)
		{
<<<<<<< HEAD
			if (!m_clientStates.TryGetValue(packet.Sender, out var value))
=======
			MyClient myClient = default(MyClient);
			if (!m_clientStates.TryGetValue(packet.Sender, ref myClient))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				packet.Return();
			}
			else
			{
				myClient.OnClientUpdate(packet, m_serverTimeStamp);
			}
		}

		public override MyTimeSpan GetSimulationUpdateTime()
		{
			return m_serverTimeStamp;
		}

		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		public override void UpdateBefore()
		{
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			Endpoint endpoint = default(Endpoint);
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				try
				{
					clientState.Value.Update(m_serverTimeStamp);
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
					endpoint = clientState.Key;
				}
			}
			if (endpoint.Id.IsValid)
			{
				m_callback.DisconnectClient(endpoint.Id.Value);
			}
			MyTimeSpan updateTime = m_callback.GetUpdateTime();
			foreach (KeyValuePair<Endpoint, MyTimeSpan> recentClientsState in m_recentClientsStates)
			{
				if (recentClientsState.Value < updateTime)
				{
					m_recentClientStatesToRemove.Add(recentClientsState.Key);
				}
			}
			Enumerator<Endpoint> enumerator3 = m_recentClientStatesToRemove.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					Endpoint current3 = enumerator3.get_Current();
					m_recentClientsStates.Remove<Endpoint, MyTimeSpan>(current3);
				}
			}
			finally
			{
				((IDisposable)enumerator3).Dispose();
			}
			m_recentClientStatesToRemove.Clear();
			m_postponedDestructionReplicables.ApplyAdditions();
			Enumerator<IMyReplicable> enumerator4 = m_postponedDestructionReplicables.GetEnumerator();
			try
			{
				while (enumerator4.MoveNext())
				{
					IMyReplicable current4 = enumerator4.get_Current();
					Destroy(current4);
				}
			}
			finally
			{
				((IDisposable)enumerator4).Dispose();
			}
			m_postponedDestructionReplicables.ApplyRemovals();
		}

		public override void UpdateAfter()
		{
		}

		public override void UpdateClientStateGroups()
		{
		}

		public override void Simulate()
		{
		}

		public override void SendUpdate()
		{
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0401: Unknown result type (might be due to invalid IL or missing references)
			//IL_053f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0544: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0650: Unknown result type (might be due to invalid IL or missing references)
			//IL_0655: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
			m_serverTimeStamp = m_callback.GetUpdateTime();
			m_serverFrame++;
			ApplyDirtyGroups();
<<<<<<< HEAD
			if (m_clientStates.Count == 0)
=======
			if (m_clientStates.get_Count() == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			m_priorityUpdates.ApplyChanges();
<<<<<<< HEAD
=======
			Enumerator<IMyReplicable> enumerator2;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				MyClient value = clientState.Value;
				if (!value.IsReady)
<<<<<<< HEAD
				{
					continue;
				}
				IMyReplicable controlledReplicable = value.State.ControlledReplicable;
				IMyReplicable characterReplicable = value.State.CharacterReplicable;
				MyClient.UpdateLayer updateLayer = value.UpdateLayers[value.UpdateLayers.Length - 1];
				for (int i = 0; i < value.UpdateLayers.Length; i++)
				{
=======
				{
					continue;
				}
				IMyReplicable controlledReplicable = value.State.ControlledReplicable;
				IMyReplicable characterReplicable = value.State.CharacterReplicable;
				MyClient.UpdateLayer updateLayer = value.UpdateLayers[value.UpdateLayers.Length - 1];
				for (int i = 0; i < value.UpdateLayers.Length; i++)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MyClient.UpdateLayer updateLayer2 = value.UpdateLayers[i];
					updateLayer2.UpdateTimer--;
					updateLayer2.PreviousLayersPCU = ((i > 0) ? value.UpdateLayers[i - 1].TotalCumulativePCU : 0);
					if (updateLayer2.UpdateTimer > 0)
					{
						continue;
					}
					updateLayer2.UpdateTimer = updateLayer2.Descriptor.UpdateInterval;
					if (value.State.Position.HasValue)
					{
						BoundingBoxD aabb = new BoundingBoxD(value.State.Position.Value - new Vector3D(updateLayer2.Descriptor.Radius), value.State.Position.Value + new Vector3D(updateLayer2.Descriptor.Radius));
						m_replicables.GetReplicablesInBox(aabb, m_tmpReplicableList);
					}
					HashSet<IMyReplicable> replicables = updateLayer2.Replicables;
					updateLayer2.Replicables = m_toRecalculateHash;
					updateLayer2.LayerPCU = 0;
					m_toRecalculateHash = replicables;
					updateLayer2.Sender.List.Clear();
<<<<<<< HEAD
					foreach (IMyReplicable item in m_toRecalculateHash)
					{
						if (value.ReplicableToLayer.TryGetValue(item, out var value2) && value2 == updateLayer2)
						{
							value.ReplicableToLayer.Remove(item);
						}
					}
					bool flag = updateLayer2 == updateLayer;
					foreach (IMyReplicable tmpReplicable in m_tmpReplicableList)
					{
						if (AddReplicableToLayer(tmpReplicable, updateLayer2, value) && flag)
						{
							m_tmpReplicableDepsList.Add(tmpReplicable);
						}
					}
					m_tmpReplicableList.Clear();
					foreach (KeyValuePair<IMyReplicable, byte> permanentReplicable in value.PermanentReplicables)
					{
						if (permanentReplicable.Value == i && AddReplicableToLayer(permanentReplicable.Key, updateLayer2, value))
						{
							m_tmpReplicableDepsList.Add(permanentReplicable.Key);
						}
					}
					if (updateLayer2 == updateLayer)
					{
						value.CrucialReplicables.Clear();
						if (controlledReplicable != null)
						{
							if (AddReplicableToLayer(controlledReplicable, updateLayer2, value))
							{
=======
					enumerator2 = m_toRecalculateHash.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							IMyReplicable current2 = enumerator2.get_Current();
							if (value.ReplicableToLayer.TryGetValue(current2, out var value2) && value2 == updateLayer2)
							{
								value.ReplicableToLayer.Remove(current2);
							}
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					bool flag = updateLayer2 == updateLayer;
					foreach (IMyReplicable tmpReplicable in m_tmpReplicableList)
					{
						if (AddReplicableToLayer(tmpReplicable, updateLayer2, value) && flag)
						{
							m_tmpReplicableDepsList.Add(tmpReplicable);
						}
					}
					m_tmpReplicableList.Clear();
					foreach (KeyValuePair<IMyReplicable, byte> permanentReplicable in value.PermanentReplicables)
					{
						if (permanentReplicable.Value == i && AddReplicableToLayer(permanentReplicable.Key, updateLayer2, value))
						{
							m_tmpReplicableDepsList.Add(permanentReplicable.Key);
						}
					}
					if (updateLayer2 == updateLayer)
					{
						value.CrucialReplicables.Clear();
						if (controlledReplicable != null)
						{
							if (AddReplicableToLayer(controlledReplicable, updateLayer2, value))
							{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								value.PlayerControllableUsesPredictedPhysics = AddReplicableDependenciesToLayer(controlledReplicable, updateLayer2, value);
							}
							if (AddReplicableToLayer(characterReplicable, updateLayer2, value))
							{
								AddReplicableDependenciesToLayer(characterReplicable, updateLayer2, value);
							}
							AddCrucialReplicable(value, controlledReplicable);
							AddReplicableDependenciesToLayer(controlledReplicable, updateLayer2, value);
							AddCrucialReplicable(value, characterReplicable);
							AddReplicableDependenciesToLayer(characterReplicable, updateLayer2, value);
							HashSet<IMyReplicable> dependencies = characterReplicable.GetDependencies(forPlayer: true);
							if (dependencies != null)
							{
<<<<<<< HEAD
								foreach (IMyReplicable item2 in dependencies)
								{
									AddCrucialReplicable(value, item2);
									if (AddReplicableToLayer(item2, updateLayer2, value))
									{
										m_tmpReplicableDepsList.Add(item2);
									}
								}
=======
								enumerator2 = dependencies.GetEnumerator();
								try
								{
									while (enumerator2.MoveNext())
									{
										IMyReplicable current5 = enumerator2.get_Current();
										AddCrucialReplicable(value, current5);
										if (AddReplicableToLayer(current5, updateLayer2, value))
										{
											m_tmpReplicableDepsList.Add(current5);
										}
									}
								}
								finally
								{
									((IDisposable)enumerator2).Dispose();
								}
							}
						}
						enumerator2 = m_lastLayerAdditions.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								IMyReplicable current6 = enumerator2.get_Current();
								AddCrucialReplicable(value, current6);
								m_tmpReplicableDepsList.Add(current6);
							}
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
						m_lastLayerAdditions.Clear();
					}
					enumerator2 = m_tmpReplicableDepsList.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							IMyReplicable current7 = enumerator2.get_Current();
							AddReplicableDependenciesToLayer(current7, updateLayer2, value);
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					m_tmpReplicableDepsList.Clear();
					updateLayer2.Enabled = i == 0 || !value.PCULimit.HasValue || updateLayer2.TotalCumulativePCU <= value.PCULimit;
					int num = -1;
					if (!updateLayer2.Enabled && i > 0 && value.UpdateLayers[i - 1].Enabled)
					{
						num = i - 1;
					}
					else if (updateLayer2.Enabled && updateLayer2 == updateLayer)
					{
						num = i;
					}
					if (num >= 0 && value.LastEnabledLayer != num)
					{
						value.LastEnabledLayer = num;
						value.State.ReplicationRange = ((num == updateLayer.Index) ? null : new int?(value.UpdateLayers[num].Descriptor.Radius));
					}
					Endpoint key = clientState.Key;
					enumerator2 = updateLayer2.Replicables.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							IMyReplicable current8 = enumerator2.get_Current();
							if (updateLayer2.Enabled || !current8.PCU.HasValue)
							{
								IMyReplicable parent = current8.GetParent();
								if (!value.HasReplicable(current8) && (parent == null || value.HasReplicable(parent)))
								{
									AddForClient(current8, key, value, force: false);
								}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
							else
							{
								m_toRecalculateHash.Add(current8);
							}
						}
<<<<<<< HEAD
						foreach (IMyReplicable lastLayerAddition in m_lastLayerAdditions)
						{
							AddCrucialReplicable(value, lastLayerAddition);
							m_tmpReplicableDepsList.Add(lastLayerAddition);
=======
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					enumerator2 = m_toRecalculateHash.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							IMyReplicable current9 = enumerator2.get_Current();
							RefreshReplicable(current9, key, value);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						m_lastLayerAdditions.Clear();
					}
					foreach (IMyReplicable tmpReplicableDeps in m_tmpReplicableDepsList)
					{
						AddReplicableDependenciesToLayer(tmpReplicableDeps, updateLayer2, value);
					}
					m_tmpReplicableDepsList.Clear();
					updateLayer2.Enabled = i == 0 || !value.PCULimit.HasValue || updateLayer2.TotalCumulativePCU <= value.PCULimit;
					int num = -1;
					if (!updateLayer2.Enabled && i > 0 && value.UpdateLayers[i - 1].Enabled)
					{
						num = i - 1;
					}
					else if (updateLayer2.Enabled && updateLayer2 == updateLayer)
					{
						num = i;
					}
					if (num >= 0 && value.LastEnabledLayer != num)
					{
						value.LastEnabledLayer = num;
						value.State.ReplicationRange = ((num == updateLayer.Index) ? null : new int?(value.UpdateLayers[num].Descriptor.Radius));
					}
					Endpoint key = clientState.Key;
					foreach (IMyReplicable replicable in updateLayer2.Replicables)
					{
						if (updateLayer2.Enabled || !replicable.PCU.HasValue)
						{
							IMyReplicable parent = replicable.GetParent();
							if (!value.HasReplicable(replicable) && (parent == null || value.HasReplicable(parent)))
							{
								AddForClient(replicable, key, value, force: false);
							}
						}
						else
						{
							m_toRecalculateHash.Add(replicable);
						}
					}
					foreach (IMyReplicable item3 in m_toRecalculateHash)
					{
						RefreshReplicable(item3, key, value);
					}
					m_toRecalculateHash.Clear();
					if (value.WantsBatchCompleteConfirmation && updateLayer2 == updateLayer && value.PendingReplicables == 0)
					{
						m_callback.SendPendingReplicablesDone(key);
						value.WantsBatchCompleteConfirmation = false;
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					m_toRecalculateHash.Clear();
					if (value.WantsBatchCompleteConfirmation && updateLayer2 == updateLayer && value.PendingReplicables == 0)
					{
						m_callback.SendPendingReplicablesDone(key);
						value.WantsBatchCompleteConfirmation = false;
					}
				}
<<<<<<< HEAD
				foreach (IMyReplicable priorityUpdate in m_priorityUpdates)
				{
					RefreshReplicable(priorityUpdate, clientState.Key, value);
				}
			}
			foreach (IMyReplicable priorityUpdate2 in m_priorityUpdates)
			{
				m_priorityUpdates.Remove(priorityUpdate2);
=======
				enumerator2 = m_priorityUpdates.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						IMyReplicable current10 = enumerator2.get_Current();
						RefreshReplicable(current10, clientState.Key, value);
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
			}
			enumerator2 = m_priorityUpdates.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					IMyReplicable current11 = enumerator2.get_Current();
					m_priorityUpdates.Remove(current11);
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_priorityUpdates.ApplyRemovals();
			foreach (KeyValuePair<Endpoint, MyClient> clientState2 in m_clientStates)
			{
				FilterStateSync(clientState2.Value);
			}
			foreach (KeyValuePair<Endpoint, MyClient> clientState3 in m_clientStates)
			{
				clientState3.Value.SendUpdate(m_serverTimeStamp);
			}
			if (StressSleep.X > 0)
			{
<<<<<<< HEAD
				int millisecondsTimeout = ((StressSleep.Z != 0) ? ((int)(Math.Sin(m_serverTimeStamp.Milliseconds * Math.PI / (double)StressSleep.Z) * (double)StressSleep.Y + (double)StressSleep.X)) : MyRandom.Instance.Next(StressSleep.X, StressSleep.Y));
				Thread.Sleep(millisecondsTimeout);
=======
				int num2 = ((StressSleep.Z != 0) ? ((int)(Math.Sin(m_serverTimeStamp.Milliseconds * Math.PI / (double)StressSleep.Z) * (double)StressSleep.Y + (double)StressSleep.X)) : MyRandom.Instance.Next(StressSleep.X, StressSleep.Y));
				Thread.Sleep(num2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		/// <summary>
		/// Adds replicable to a replication layer if it's not already in a smaller one.
		/// </summary>
		private bool AddReplicableToLayer(IMyReplicable rep, MyClient.UpdateLayer layer, MyClient client)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			if (IsReplicableInPreviousLayer(rep, layer, client))
			{
				return false;
			}
			AddReplicableToLayerSingle(rep, layer, client);
			HashSet<IMyReplicable> criticalDependencies = rep.GetCriticalDependencies();
			if (criticalDependencies != null)
			{
<<<<<<< HEAD
				foreach (IMyReplicable item in criticalDependencies)
				{
					if (!IsReplicableInPreviousLayer(item, layer, client))
					{
						AddReplicableToLayerSingle(item, layer, client);
					}
				}
=======
				Enumerator<IMyReplicable> enumerator = criticalDependencies.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						IMyReplicable current = enumerator.get_Current();
						if (!IsReplicableInPreviousLayer(current, layer, client))
						{
							AddReplicableToLayerSingle(current, layer, client);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return true;
		}

		/// <summary>
		/// Adds adds any physically connected replicables
		/// </summary>
		private bool AddReplicableDependenciesToLayer(IMyReplicable rep, MyClient.UpdateLayer layer, MyClient client)
		{
<<<<<<< HEAD
=======
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			bool flag = true;
			if (!client.PCULimit.HasValue || layer.TotalCumulativePCU < client.PCULimit)
			{
				HashSet<IMyReplicable> physicalDependencies = rep.GetPhysicalDependencies(m_serverTimeStamp, m_replicables);
				if (physicalDependencies != null)
				{
					if (client.PCULimit.HasValue)
<<<<<<< HEAD
					{
						int num = 0;
						foreach (IMyReplicable item in physicalDependencies)
						{
							if (!IsReplicableInPreviousLayer(item, layer, client) && !layer.Replicables.Contains(rep))
							{
								num += rep.PCU ?? 0;
							}
						}
						if (num + layer.TotalCumulativePCU > client.PCULimit)
						{
							flag = false;
						}
					}
					if (flag)
					{
						foreach (IMyReplicable item2 in physicalDependencies)
						{
							if (!IsReplicableInPreviousLayer(item2, layer, client))
							{
								AddReplicableToLayerSingle(item2, layer, client);
							}
=======
					{
						int num = 0;
						Enumerator<IMyReplicable> enumerator = physicalDependencies.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								IMyReplicable current = enumerator.get_Current();
								if (!IsReplicableInPreviousLayer(current, layer, client) && !layer.Replicables.Contains(rep))
								{
									num += rep.PCU ?? 0;
								}
							}
						}
						finally
						{
							((IDisposable)enumerator).Dispose();
						}
						if (num + layer.TotalCumulativePCU > client.PCULimit)
						{
							flag = false;
						}
					}
					if (flag)
					{
						Enumerator<IMyReplicable> enumerator = physicalDependencies.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								IMyReplicable current2 = enumerator.get_Current();
								if (!IsReplicableInPreviousLayer(current2, layer, client))
								{
									AddReplicableToLayerSingle(current2, layer, client);
								}
							}
							return flag;
						}
						finally
						{
							((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						return flag;
					}
				}
			}
			return flag;
		}

		private void AddReplicableToLayerSingle(IMyReplicable rep, MyClient.UpdateLayer layer, MyClient client, bool removeFromDelete = true)
		{
<<<<<<< HEAD
=======
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (layer.Replicables.Add(rep))
			{
				layer.Sender.List.Add(rep);
				layer.LayerPCU += rep.PCU ?? 0;
			}
			client.ReplicableToLayer[rep] = layer;
			if (removeFromDelete)
			{
				m_toRecalculateHash.Remove(rep);
			}
			HashSet<IMyReplicable> dependencies = rep.GetDependencies(forPlayer: false);
			if (dependencies == null)
			{
				return;
			}
<<<<<<< HEAD
			foreach (IMyReplicable item in dependencies)
			{
				m_lastLayerAdditions.Add(item);
=======
			Enumerator<IMyReplicable> enumerator = dependencies.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IMyReplicable current = enumerator.get_Current();
					m_lastLayerAdditions.Add(current);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private bool IsReplicableInPreviousLayer(IMyReplicable rep, MyClient.UpdateLayer layer, MyClient client)
		{
			if (client.ReplicableToLayer.TryGetValue(rep, out var value) && value != layer)
			{
				return value.Index < layer.Index;
			}
			return false;
		}

		/// <summary>
		/// Set the PCU limit for a client given it's endpoint.
		/// </summary>
		/// <param name="clientEndpoint"></param>
		/// <param name="pcuLimit"></param>
		public void SetClientPCULimit(EndpointId clientEndpoint, int pcuLimit)
		{
<<<<<<< HEAD
			if (m_clientStates.TryGetValue(new Endpoint(clientEndpoint, 0), out var value))
			{
				value.PCULimit = pcuLimit;
=======
			MyClient myClient = default(MyClient);
			if (m_clientStates.TryGetValue(new Endpoint(clientEndpoint, 0), ref myClient))
			{
				myClient.PCULimit = pcuLimit;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public IEnumerable<(BoundingBoxD Bounds, IEnumerable<IMyReplicable> Replicables, int PCU, bool Enabled)> GetLayerData(EndpointId clientEndpoint)
		{
<<<<<<< HEAD
			if (m_clientStates.TryGetValue(new Endpoint(clientEndpoint, 0), out var client) && client.State.Position.HasValue)
=======
			MyClient client = default(MyClient);
			if (m_clientStates.TryGetValue(new Endpoint(clientEndpoint, 0), ref client) && client.State.Position.HasValue)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyClient.UpdateLayer[] updateLayers = client.UpdateLayers;
				foreach (MyClient.UpdateLayer updateLayer in updateLayers)
				{
					Vector3D value = client.State.Position.Value;
					BoundingBoxD item = new BoundingBoxD(value - new Vector3D(updateLayer.Descriptor.Radius), value + new Vector3D(updateLayer.Descriptor.Radius));
<<<<<<< HEAD
					yield return (item, updateLayer.Replicables, updateLayer.LayerPCU, updateLayer.Enabled);
=======
					yield return (item, (IEnumerable<IMyReplicable>)updateLayer.Replicables, updateLayer.LayerPCU, updateLayer.Enabled);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		private void RefreshReplicable(IMyReplicable replicable, Endpoint endPoint, MyClient client, bool force = false)
		{
			if (!replicable.IsSpatial)
			{
				IMyReplicable parent = replicable.GetParent();
				if (parent == null && !client.CrucialReplicables.Contains(replicable))
				{
					RemoveReplicable();
				}
				else if (parent == null || client.HasReplicable(parent))
				{
					AddForClient(replicable, endPoint, client, force);
				}
				else if (replicable.HasToBeChild && !client.HasReplicable(parent))
				{
					RemoveReplicable();
				}
				return;
			}
			bool flag = true;
			if (replicable.HasToBeChild)
			{
				IMyReplicable parent2 = replicable.GetParent();
				if (parent2 != null)
				{
					flag = client.HasReplicable(parent2);
				}
			}
			MyClient.UpdateLayer updateLayer = client.CalculateLayerOfReplicable(replicable);
			if (updateLayer != null && flag && (updateLayer.Enabled || !replicable.PCU.HasValue))
			{
				AddForClient(replicable, endPoint, client, force);
				AddReplicableToLayerSingle(replicable, updateLayer, client, removeFromDelete: false);
			}
			else if ((replicable == client.State.ControlledReplicable || replicable == client.State.CharacterReplicable || client.CrucialReplicables.Contains(replicable) || client.PermanentReplicables.ContainsKey(replicable)) && flag)
			{
				AddReplicableToLayerSingle(replicable, client.UpdateLayers[0], client, removeFromDelete: false);
				AddForClient(replicable, endPoint, client, force);
			}
			else
			{
				RemoveReplicable();
			}
			void RemoveReplicable()
			{
				if (client.Replicables.ContainsKey(replicable) && !client.BlockedReplicables.ContainsKey(replicable))
				{
					RemoveForClient(replicable, client, sendDestroyToClient: true);
				}
			}
		}

		private void AddCrucialReplicable(MyClient client, IMyReplicable replicable)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			client.CrucialReplicables.Add(replicable);
			HashSet<IMyReplicable> criticalDependencies = replicable.GetCriticalDependencies();
			if (criticalDependencies == null)
<<<<<<< HEAD
			{
				return;
			}
			foreach (IMyReplicable item in criticalDependencies)
			{
				client.CrucialReplicables.Add(item);
=======
			{
				return;
			}
			Enumerator<IMyReplicable> enumerator = criticalDependencies.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IMyReplicable current = enumerator.get_Current();
					client.CrucialReplicables.Add(current);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void AddToDirtyGroups(IMyStateGroup group)
		{
			if (group.Owner.IsReadyForReplication)
			{
				m_dirtyGroups.Enqueue(group);
			}
		}

		private void ApplyDirtyGroups()
		{
			IMyStateGroup key = default(IMyStateGroup);
			while (m_dirtyGroups.TryDequeue(ref key))
			{
				foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
				{
<<<<<<< HEAD
					if (clientState.Value.StateGroups.TryGetValue(result, out var value))
=======
					if (clientState.Value.StateGroups.TryGetValue(key, out var value))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						ScheduleStateGroupSync(clientState.Value, value, SyncFrameCounter);
					}
				}
			}
		}

		private void SendStreamingEntry(MyClient client, MyStateDataEntry entry)
		{
			Endpoint endpointId = client.State.EndpointId;
			if (entry.Group.IsProcessingForClient(endpointId) == MyStreamProcessingState.Finished)
			{
				MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
				BitStream stream = bitStreamPacketData.Stream;
				if (!client.WritePacketHeader(stream, streaming: true, m_serverTimeStamp, out var clientTimestamp))
				{
					bitStreamPacketData.Return();
					return;
				}
				stream.Terminate();
				stream.WriteNetworkId(entry.GroupId);
				long bitPosition = stream.BitPosition;
				stream.WriteInt32(0);
				long bitPosition2 = stream.BitPosition;
				client.Serialize(entry.Group, stream, clientTimestamp, int.MaxValue, streaming: true);
				client.AddPendingAck(entry.Group, streaming: true);
				long bitPosition3 = stream.BitPosition;
				stream.SetBitPositionWrite(bitPosition);
				stream.WriteInt32((int)(bitPosition3 - bitPosition2));
				stream.SetBitPositionWrite(bitPosition3);
				stream.Terminate();
				m_callback.SendStateSync(bitStreamPacketData, endpointId, reliable: true);
			}
			else
			{
				client.Serialize(entry.Group, null, MyTimeSpan.Zero);
				ScheduleStateGroupSync(client, entry, SyncFrameCounter);
			}
			IMyReplicable owner = entry.Group.Owner;
			if (owner == null)
			{
				return;
			}
			using (m_tmpAdd)
			{
				m_replicables.GetAllChildren(owner, m_tmpAdd);
				foreach (IMyReplicable item in m_tmpAdd)
				{
					if (!client.HasReplicable(item))
					{
						AddForClient(item, endpointId, client, force: false);
					}
				}
			}
		}

		private void FilterStateSync(MyClient client)
		{
			if (!client.IsAckAvailable())
			{
				return;
			}
			ApplyDirtyGroups();
			int num = 0;
			MyPacketDataBitStreamBase data = null;
			List<MyStateDataEntry> list = PoolManager.Get<List<MyStateDataEntry>>();
			int mTUSize = m_callback.GetMTUSize();
			int count = client.DirtyQueue.Count;
			int num2 = 7;
			MyStateDataEntry myStateDataEntry = null;
			while (count-- > 0 && num2 > 0 && client.DirtyQueue.First.Priority < SyncFrameCounter)
			{
				MyStateDataEntry myStateDataEntry2 = client.DirtyQueue.Dequeue();
				list.Add(myStateDataEntry2);
				if (myStateDataEntry2.Owner != null && !myStateDataEntry2.Group.IsStreaming && (!client.Replicables.TryGetValue(myStateDataEntry2.Owner, out var value) || !value.HasActiveStateSync))
				{
					continue;
				}
				if (myStateDataEntry2.Group.IsStreaming)
				{
					if (myStateDataEntry == null && myStateDataEntry2.Group.IsProcessingForClient(client.State.EndpointId) != MyStreamProcessingState.Processing)
					{
						myStateDataEntry = myStateDataEntry2;
					}
					continue;
				}
				if (!client.SendStateSync(myStateDataEntry2, mTUSize, ref data, m_serverTimeStamp))
				{
					break;
				}
				num++;
				if (data == null)
				{
					num2--;
				}
			}
			if (data != null)
			{
				data.Stream.Terminate();
				m_callback.SendStateSync(data, client.State.EndpointId, reliable: false);
			}
			if (myStateDataEntry != null)
			{
				SendStreamingEntry(client, myStateDataEntry);
			}
			long syncFrameCounter = SyncFrameCounter;
			foreach (MyStateDataEntry item in list)
			{
				if (client.StateGroups.ContainsKey(item.Group) && item.Group.IsStillDirty(client.State.EndpointId))
				{
					ScheduleStateGroupSync(client, item, syncFrameCounter);
				}
			}
		}

		private void ScheduleStateGroupSync(MyClient client, MyStateDataEntry groupEntry, long currentTime, bool allowReplicableRemoval = true)
		{
			IMyReplicable parent = groupEntry.Owner.GetParent();
			IMyReplicable myReplicable = parent ?? groupEntry.Owner;
			MyClient.UpdateLayer value = null;
			if (!client.ReplicableToLayer.TryGetValue(myReplicable, out value))
			{
				value = client.CalculateLayerOfReplicable(myReplicable);
				if (value == null)
				{
					if (client.HasReplicable(myReplicable) && allowReplicableRemoval && client.State.Position.HasValue)
					{
						if (client.IsReplicableReady(myReplicable))
						{
							RemoveForClient(myReplicable, client, sendDestroyToClient: true);
						}
						else
						{
							MyLog.Default.Warning("Trying to remove entity with name " + myReplicable.InstanceName + " that is not yet replicated on client");
						}
						return;
					}
					value = client.UpdateLayers[client.UpdateLayers.Length - 1];
				}
			}
			int num = ((myReplicable == client.State.ControlledReplicable) ? 1 : (groupEntry.Group.IsHighPriority ? Math.Max(value.Descriptor.SendInterval >> 4, 1) : value.Descriptor.SendInterval));
			num = MyRandom.Instance.Next(1, num * 2);
			long num2 = num + currentTime;
			if (!client.DirtyQueue.Contains(groupEntry))
			{
				if (groupEntry.Owner.IsValid || m_postponedDestructionReplicables.Contains(groupEntry.Owner))
				{
					client.DirtyQueue.Enqueue(groupEntry, num2);
				}
			}
			else if (groupEntry.Owner.IsValid || m_postponedDestructionReplicables.Contains(groupEntry.Owner))
			{
				long priority = groupEntry.Priority;
				num2 = Math.Min(priority, num2);
				if (num2 != priority)
				{
					client.DirtyQueue.UpdatePriority(groupEntry, num2);
				}
			}
			else
			{
				client.DirtyQueue.Remove(groupEntry);
			}
		}

		public override void UpdateStatisticsData(int outgoing, int incoming, int tamperred, float gcMemory, float processMemory)
		{
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				clientState.Value.Statistics.UpdateData(outgoing, incoming, tamperred, gcMemory, processMemory);
			}
		}

		public override MyPacketStatistics ClearServerStatistics()
		{
			return default(MyPacketStatistics);
		}

		public override MyPacketStatistics ClearClientStatistics()
		{
			MyPacketStatistics result = default(MyPacketStatistics);
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				result.Add(clientState.Value.Statistics);
			}
			return result;
		}

		private void AddForClient(IMyReplicable replicable, Endpoint clientEndpoint, MyClient client, bool force, bool addDependencies = false)
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			if (!replicable.IsReadyForReplication || client.HasReplicable(replicable) || !replicable.ShouldReplicate(new MyClientInfo(client)) || !replicable.IsValid)
			{
				return;
			}
			AddClientReplicable(replicable, client, force);
			replicable.OnReplication();
			SendReplicationCreate(replicable, client, clientEndpoint);
			if (replicable is IMyStreamableReplicable)
			{
				return;
			}
<<<<<<< HEAD
			foreach (IMyReplicable child in m_replicables.GetChildren(replicable))
			{
				AddForClient(child, clientEndpoint, client, force);
=======
			Enumerator<IMyReplicable> enumerator = m_replicables.GetChildren(replicable).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IMyReplicable current = enumerator.get_Current();
					AddForClient(current, clientEndpoint, client, force);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private void RemoveForClients(IMyReplicable replicable, Func<MyClient, bool> validate, bool sendDestroyToClient)
		{
			using (m_tmp)
			{
				if (m_recipients == null)
				{
					m_recipients = new List<EndpointId>();
				}
				bool flag = true;
				foreach (MyClient value in m_clientStates.get_Values())
				{
					if (validate(value))
					{
						if (flag)
						{
							m_replicables.GetAllChildren(replicable, m_tmp);
							m_tmp.Add(replicable);
							flag = false;
						}
						RemoveForClientInternal(replicable, value);
						if (sendDestroyToClient)
						{
							m_recipients.Add(value.State.EndpointId.Id);
						}
					}
				}
				if (m_recipients.Count > 0)
				{
					SendReplicationDestroy(replicable, m_recipients);
					m_recipients.Clear();
				}
			}
		}

		private void RemoveForClient(IMyReplicable replicable, MyClient client, bool sendDestroyToClient)
		{
			if (m_recipients == null)
			{
				m_recipients = new List<EndpointId>();
			}
			using (m_tmp)
			{
				m_replicables.GetAllChildren(replicable, m_tmp);
				m_tmp.Add(replicable);
				RemoveForClientInternal(replicable, client);
				if (sendDestroyToClient)
				{
					m_recipients.Add(client.State.EndpointId.Id);
					SendReplicationDestroy(replicable, m_recipients);
					m_recipients.Clear();
				}
			}
		}

		private void RemoveForClientInternal(IMyReplicable replicable, MyClient client)
		{
			foreach (IMyReplicable item in m_tmp)
			{
				client.BlockedReplicables.Remove(item);
				RemoveClientReplicable(item, client);
			}
			MyClient.UpdateLayer[] updateLayers = client.UpdateLayers;
			foreach (MyClient.UpdateLayer updateLayer in updateLayers)
			{
				if (updateLayer.Replicables.Remove(replicable))
				{
					updateLayer.LayerPCU -= replicable.PCU ?? 0;
				}
			}
		}

		private void SendReplicationCreate(IMyReplicable obj, MyClient client, Endpoint clientEndpoint)
		{
			TypeId typeIdByType = GetTypeIdByType(obj.GetType());
			NetworkId networkIdByObject = GetNetworkIdByObject(obj);
			NetworkId networkId = NetworkId.Invalid;
			IMyReplicable parent = obj.GetParent();
			if (parent != null)
			{
				networkId = GetNetworkIdByObject(parent);
			}
			List<IMyStateGroup> list = m_replicableGroups[obj];
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			BitStream stream = bitStreamPacketData.Stream;
			stream.WriteTypeId(typeIdByType);
			stream.WriteNetworkId(networkIdByObject);
			stream.WriteNetworkId(networkId);
			IMyStreamableReplicable myStreamableReplicable = obj as IMyStreamableReplicable;
			bool flag = myStreamableReplicable?.NeedsToBeStreamed ?? false;
			if (myStreamableReplicable != null && !myStreamableReplicable.NeedsToBeStreamed)
			{
				stream.WriteByte((byte)(list.Count - 1));
			}
			else
			{
				stream.WriteByte((byte)list.Count);
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (flag || !list[i].IsStreaming)
				{
					stream.WriteNetworkId(GetNetworkIdByObject(list[i]));
				}
			}
			if (flag)
			{
				client.Replicables[obj].IsStreaming = true;
				m_callback.SendReplicationCreateStreamed(bitStreamPacketData, clientEndpoint);
			}
			else
			{
				obj.OnSave(stream, clientEndpoint);
				m_callback.SendReplicationCreate(bitStreamPacketData, clientEndpoint);
			}
		}

		private void SendReplicationDestroy(IMyReplicable obj, List<EndpointId> recipients)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			bitStreamPacketData.Stream.WriteNetworkId(GetNetworkIdByObject(obj));
			m_callback.SendReplicationDestroy(bitStreamPacketData, recipients);
		}

		public void ReplicableReady(MyPacket packet)
		{
			NetworkId id = packet.BitStream.ReadNetworkId();
			bool flag = packet.BitStream.ReadBool();
			if (!packet.BitStream.CheckTerminator())
			{
				throw new EndOfStreamException("Invalid BitStream terminator");
			}
<<<<<<< HEAD
			if (m_clientStates.TryGetValue(packet.Sender, out var value))
=======
			MyClient myClient = default(MyClient);
			if (m_clientStates.TryGetValue(packet.Sender, ref myClient))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				IMyReplicable myReplicable = GetObjectByNetworkId(id) as IMyReplicable;
				if (myReplicable != null)
				{
<<<<<<< HEAD
					if (value.Replicables.TryGetValue(myReplicable, out var value2))
=======
					if (myClient.Replicables.TryGetValue(myReplicable, out var value))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						if (flag)
						{
							value.IsPending = false;
							value.IsStreaming = false;
							myClient.PendingReplicables--;
							if (myClient.WantsBatchCompleteConfirmation && myClient.PendingReplicables == 0)
							{
								m_callback.SendPendingReplicablesDone(packet.Sender);
								myClient.WantsBatchCompleteConfirmation = false;
							}
						}
					}
					else if (!flag)
					{
						RemoveForClient(myReplicable, myClient, sendDestroyToClient: false);
					}
				}
				if (myReplicable != null)
				{
					ProcessBlocker(myReplicable, packet.Sender, myClient, null);
				}
			}
			packet.Return();
		}

		public void ReplicableRequest(MyPacket packet)
		{
			long entityId = packet.BitStream.ReadInt64();
			bool flag = packet.BitStream.ReadBool();
			byte value = 0;
			if (flag)
			{
				value = packet.BitStream.ReadByte();
			}
			IMyReplicable replicableByEntityId = m_callback.GetReplicableByEntityId(entityId);
<<<<<<< HEAD
			if (m_clientStates.TryGetValue(packet.Sender, out var value2))
=======
			MyClient myClient = default(MyClient);
			if (m_clientStates.TryGetValue(packet.Sender, ref myClient))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (flag)
				{
					if (replicableByEntityId != null)
					{
						myClient.PermanentReplicables[replicableByEntityId] = value;
					}
				}
				else if (replicableByEntityId != null)
				{
					myClient.PermanentReplicables.Remove(replicableByEntityId);
				}
			}
			packet.Return();
		}

		private bool ProcessBlocker(IMyReplicable replicable, Endpoint endpoint, MyClient client, IMyReplicable parent)
		{
			if (client.BlockedReplicables.ContainsKey(replicable))
			{
				MyDestroyBlocker myDestroyBlocker = client.BlockedReplicables[replicable];
				if (myDestroyBlocker.IsProcessing)
				{
					return true;
				}
				myDestroyBlocker.IsProcessing = true;
				foreach (IMyReplicable blocker in myDestroyBlocker.Blockers)
				{
					if (!client.IsReplicableReady(replicable) || !client.IsReplicableReady(blocker))
					{
						myDestroyBlocker.IsProcessing = false;
						return false;
					}
					bool flag = true;
					if (blocker != parent)
					{
						flag = ProcessBlocker(blocker, endpoint, client, replicable);
					}
					if (!flag)
					{
						myDestroyBlocker.IsProcessing = false;
						return false;
					}
				}
				client.BlockedReplicables.Remove(replicable);
				if (myDestroyBlocker.Remove)
				{
					RemoveForClient(replicable, client, sendDestroyToClient: true);
				}
				myDestroyBlocker.IsProcessing = false;
			}
			return true;
		}

		private void AddStateGroups(IMyReplicable replicable)
		{
			using (m_tmpGroups)
			{
				(replicable as IMyStreamableReplicable)?.CreateStreamingStateGroup();
				replicable.GetStateGroups(m_tmpGroups);
				foreach (IMyStateGroup tmpGroup in m_tmpGroups)
				{
					AddNetworkObjectServer(tmpGroup);
				}
				m_replicableGroups.Add(replicable, new List<IMyStateGroup>(m_tmpGroups));
			}
		}

		private void RemoveStateGroups(IMyReplicable replicable)
		{
			foreach (MyClient value in m_clientStates.get_Values())
			{
				RemoveClientReplicable(replicable, value);
			}
			foreach (IMyStateGroup item in m_replicableGroups[replicable])
			{
				RemoveNetworkedObject(item);
				item.Destroy();
			}
			m_replicableGroups.Remove(replicable);
		}

		private void AddClientReplicable(IMyReplicable replicable, MyClient client, bool force)
		{
			client.Replicables.Add(replicable, new MyReplicableClientData());
			client.PendingReplicables++;
			if (!m_replicableGroups.ContainsKey(replicable))
			{
				return;
			}
			foreach (IMyStateGroup item in m_replicableGroups[replicable])
			{
				NetworkId networkIdByObject = GetNetworkIdByObject(item);
				if (!item.IsStreaming || (replicable as IMyStreamableReplicable).NeedsToBeStreamed)
				{
					client.StateGroups.Add(item, new MyStateDataEntry(replicable, networkIdByObject, item));
					ScheduleStateGroupSync(client, client.StateGroups[item], SyncFrameCounter, allowReplicableRemoval: false);
					item.CreateClientData(client.State);
					if (force)
					{
						item.ForceSend(client.State);
					}
				}
			}
		}

		private void RemoveClientReplicable(IMyReplicable replicable, MyClient client)
		{
			if (!m_replicableGroups.ContainsKey(replicable))
			{
				return;
			}
			using (m_tmpGroups)
			{
				replicable.GetStateGroups(m_tmpGroups);
				foreach (IMyStateGroup item in m_replicableGroups[replicable])
				{
					item.DestroyClientData(client.State);
					if (client.StateGroups.ContainsKey(item))
					{
						if (client.DirtyQueue.Contains(client.StateGroups[item]))
						{
							client.DirtyQueue.Remove(client.StateGroups[item]);
						}
						client.StateGroups.Remove(item);
					}
				}
				if (client.Replicables.TryGetValue(replicable, out var value) && value.IsPending)
				{
					client.PendingReplicables--;
				}
				client.Replicables.Remove(replicable);
				replicable.OnUnreplication();
				m_tmpGroups.Clear();
			}
		}

		private bool ShouldSendEvent(IMyNetObject eventInstance, Vector3D? position, MyClient client, CallSite site)
		{
			if (position.HasValue || site.HasDistanceRadius)
			{
				Vector3D? vector3D = null;
				vector3D = (client.State.CharacterPosition.HasValue ? new Vector3D?(client.State.CharacterPosition.Value) : client.State.Position);
				if (!vector3D.HasValue)
				{
					return false;
				}
				float num;
				if (site.HasDistanceRadius)
				{
					num = site.DistanceRadiusSquared;
				}
				else
				{
					num = client.State.ReplicationRange ?? ((float)MyLayers.GetSyncDistance());
					num *= num;
				}
				Vector3D value = ((!position.HasValue) ? (eventInstance as IMyReplicable).GetAABB().Center : position.Value);
<<<<<<< HEAD
				if (Vector3D.DistanceSquared(value, vector3D.Value) > (double)num)
=======
				if (Vector3D.DistanceSquared(value, client.State.Position.Value) > (double)num)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return false;
				}
			}
			if (eventInstance == null)
			{
				return true;
			}
			if (eventInstance is IMyReplicable)
			{
				return client.Replicables.ContainsKey((IMyReplicable)eventInstance);
			}
			return false;
		}

		public override MyClientStateBase GetClientData(Endpoint endpointId)
		{
<<<<<<< HEAD
			if (!m_clientStates.TryGetValue(endpointId, out var value))
=======
			MyClient myClient = default(MyClient);
			if (!m_clientStates.TryGetValue(endpointId, ref myClient))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return null;
			}
			return myClient.State;
		}

		protected override bool DispatchBlockingEvent(IPacketData data, CallSite site, EndpointId target, IMyNetObject targetReplicable, Vector3D? position, IMyNetObject blockingReplicable)
		{
			Endpoint endpoint = new Endpoint(target, 0);
			IMyReplicable blockingReplicable2 = blockingReplicable as IMyReplicable;
			IMyReplicable targetReplicable2 = targetReplicable as IMyReplicable;
			MyClient client = default(MyClient);
			if (site.HasBroadcastFlag || site.HasBroadcastExceptFlag)
			{
				foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
				{
					if ((!site.HasBroadcastExceptFlag || !(clientState.Key.Id == target)) && clientState.Key.Index == 0 && ShouldSendEvent(targetReplicable, position, clientState.Value, site))
					{
						TryAddBlockerForClient(clientState.Value, targetReplicable2, blockingReplicable2);
					}
				}
			}
<<<<<<< HEAD
			else if (site.HasClientFlag && m_localEndpoint != target && m_clientStates.TryGetValue(key, out value) && ShouldSendEvent(targetReplicable, position, value, site))
=======
			else if (site.HasClientFlag && m_localEndpoint != target && m_clientStates.TryGetValue(endpoint, ref client) && ShouldSendEvent(targetReplicable, position, client, site))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				TryAddBlockerForClient(client, targetReplicable2, blockingReplicable2);
			}
			return DispatchEvent(data, site, target, targetReplicable, position);
		}

		private static void TryAddBlockerForClient(MyClient client, IMyReplicable targetReplicable, IMyReplicable blockingReplicable)
		{
			if (!client.IsReplicableReady(targetReplicable) || !client.IsReplicableReady(blockingReplicable) || client.BlockedReplicables.ContainsKey(targetReplicable) || client.BlockedReplicables.ContainsKey(blockingReplicable))
			{
				if (!client.BlockedReplicables.TryGetValue(targetReplicable, out var value))
				{
					value = new MyDestroyBlocker();
					client.BlockedReplicables.Add(targetReplicable, value);
				}
				value.Blockers.Add(blockingReplicable);
				if (!client.BlockedReplicables.TryGetValue(blockingReplicable, out var value2))
				{
					value2 = new MyDestroyBlocker();
					client.BlockedReplicables.Add(blockingReplicable, value2);
				}
				value2.Blockers.Add(targetReplicable);
			}
		}

		protected override bool DispatchEvent(IPacketData data, CallSite site, EndpointId target, IMyNetObject eventInstance, Vector3D? position)
		{
			if (m_recipients == null)
			{
				m_recipients = new List<EndpointId>();
			}
			Endpoint endpoint = new Endpoint(target, 0);
			bool flag = false;
			MyClient client = default(MyClient);
			if (site.HasBroadcastFlag || site.HasBroadcastExceptFlag)
			{
				foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
				{
					if ((!site.HasBroadcastExceptFlag || !(clientState.Key.Id == target)) && clientState.Key.Index == 0 && ShouldSendEvent(eventInstance, position, clientState.Value, site))
					{
						m_recipients.Add(clientState.Key.Id);
					}
				}
				if (m_recipients.Count > 0)
				{
					DispatchEvent(data, m_recipients, site.IsReliable);
					flag = true;
					m_recipients.Clear();
				}
			}
<<<<<<< HEAD
			else if (site.HasClientFlag && m_localEndpoint != target && m_clientStates.TryGetValue(key, out value) && ShouldSendEvent(eventInstance, position, value, site))
=======
			else if (site.HasClientFlag && m_localEndpoint != target && m_clientStates.TryGetValue(endpoint, ref client) && ShouldSendEvent(eventInstance, position, client, site))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				DispatchEvent(data, client, site.IsReliable);
				flag = true;
			}
			if (!flag)
			{
				data.Return();
			}
			return MyReplicationLayerBase.ShouldServerInvokeLocally(site, m_localEndpoint, target);
		}

		private void DispatchEvent(IPacketData data, MyClient client, bool reliable)
		{
			m_recipients.Add(client.State.EndpointId.Id);
			DispatchEvent(data, m_recipients, reliable);
			m_recipients.Clear();
		}

		private void DispatchEvent(IPacketData data, List<EndpointId> recipients, bool reliable)
		{
			m_callback.SendEvent(data, reliable, recipients);
		}

		protected override void OnEvent(MyPacketDataBitStreamBase data, CallSite site, object obj, IMyNetObject sendAs, Vector3D? position, EndpointId source)
		{
			MyClientStateBase clientData = GetClientData(new Endpoint(source, 0));
			if (clientData == null)
			{
				data.Return();
				return;
			}
			if (site.HasServerInvokedFlag)
			{
				data.Return();
				m_callback.ValidationFailed(source.Value, kick: true, "ServerInvoked " + site.ToString(), stackTrace: false);
				return;
			}
			IMyReplicable myReplicable = sendAs as IMyReplicable;
			if (myReplicable != null)
			{
				ValidationResult validationResult = myReplicable.HasRights(source, site.ValidationFlags);
				if (validationResult != 0)
				{
					data.Return();
					m_callback.ValidationFailed(source.Value, validationResult.HasFlag(ValidationResult.Kick), validationResult.ToString() + " " + site.ToString(), stackTrace: false);
					return;
				}
			}
			if (!Invoke(site, data.Stream, obj, source, clientData, validate: true))
			{
				data.Return();
				return;
			}
			if (!data.Stream.CheckTerminator())
			{
				throw new EndOfStreamException("Invalid BitStream terminator");
			}
			if (site.HasClientFlag || site.HasBroadcastFlag || site.HasBroadcastExceptFlag)
			{
				DispatchEvent(data, site, source, sendAs, position);
			}
			else
			{
				data.Return();
			}
		}

		public void SendJoinResult(ref JoinResultMsg msg, ulong sendTo)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			MySerializer.Write(bitStreamPacketData.Stream, ref msg);
			m_callback.SendJoinResult(bitStreamPacketData, new EndpointId(sendTo));
		}

		public void SendWorldData(ref ServerDataMsg msg)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			MySerializer.Write(bitStreamPacketData.Stream, ref msg);
			m_endpoints.Clear();
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				m_endpoints.Add(clientState.Key.Id);
			}
			m_callback.SendWorldData(bitStreamPacketData, m_endpoints);
		}

		public void SendWorld(byte[] worldData, EndpointId sendTo)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			MySerializer.Write(bitStreamPacketData.Stream, ref worldData);
			m_callback.SendWorld(bitStreamPacketData, sendTo);
		}

		public void SendPlayerData(Action<MyPacketDataBitStreamBase> serializer)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			serializer(bitStreamPacketData);
			m_endpoints.Clear();
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				m_endpoints.Add(clientState.Key.Id);
			}
			m_callback.SendPlayerData(bitStreamPacketData, m_endpoints);
		}

		public ConnectedClientDataMsg OnClientConnected(MyPacket packet)
		{
			return MySerializer.CreateAndRead<ConnectedClientDataMsg>(packet.BitStream);
		}

		public void SendClientConnected(ref ConnectedClientDataMsg msg, ulong sendTo)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			MySerializer.Write(bitStreamPacketData.Stream, ref msg);
			m_callback.SentClientJoined(bitStreamPacketData, new EndpointId(sendTo));
		}

		public void InvalidateClientCache(IMyReplicable replicable, string storageName)
		{
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				if (clientState.Value.RemoveCache(replicable, storageName))
				{
					m_callback.SendVoxelCacheInvalidated(storageName, clientState.Key.Id);
				}
			}
		}

		public void InvalidateSingleClientCache(string storageName, EndpointId clientId)
		{
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				if (clientState.Key.Id == clientId)
				{
					clientState.Value.RemoveCache(null, storageName);
				}
			}
		}

		public MyTimeSpan GetClientRelevantServerTimestamp(Endpoint clientEndpoint)
		{
			return m_serverTimeStamp;
		}

		public void GetClientPings(out SerializableDictionary<ulong, short> pings)
		{
			pings = new SerializableDictionary<ulong, short>();
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				pings[clientState.Key.Id.Value] = clientState.Value.State.Ping;
			}
		}

		public void ResendMissingReplicableChildren(IMyEventProxy target)
		{
			ResendMissingReplicableChildren(GetProxyTarget(target) as IMyReplicable);
		}

		private void ResendMissingReplicableChildren(IMyReplicable replicable)
		{
			m_replicables.GetAllChildren(replicable, m_tmpReplicableList);
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				if (!clientState.Value.HasReplicable(replicable))
				{
					continue;
				}
				foreach (IMyReplicable tmpReplicable in m_tmpReplicableList)
				{
					if (!clientState.Value.HasReplicable(tmpReplicable))
					{
						AddForClient(tmpReplicable, clientState.Key, clientState.Value, force: false);
					}
				}
			}
			m_tmpReplicableList.Clear();
		}

		public void SetClientBatchConfrmation(Endpoint clientEndpoint, bool value)
		{
<<<<<<< HEAD
			if (m_clientStates.TryGetValue(clientEndpoint, out var value2))
=======
			MyClient myClient = default(MyClient);
			if (m_clientStates.TryGetValue(clientEndpoint, ref myClient))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				myClient.WantsBatchCompleteConfirmation = value;
				if (value)
				{
					myClient.ResetLayerTimers();
				}
			}
		}

		/// <summary>
		/// Indicates if a replicable is replicated to at least one client.
		/// </summary>
		/// <param name="replicable">Replicable to check.</param>
		/// <returns>True if replicated at least to one client.</returns>
		public bool IsReplicated(IMyReplicable replicable)
		{
			foreach (MyClient value in m_clientStates.get_Values())
			{
				if (value.HasReplicable(replicable) && value.IsReplicableReady(replicable))
				{
					return true;
				}
			}
			return false;
		}

		public override string GetMultiplayerStat()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string multiplayerStat = base.GetMultiplayerStat();
			stringBuilder.Append(multiplayerStat);
			stringBuilder.AppendLine("Client state info:");
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				string value = string.Concat("    Endpoint: ", clientState.Key, ", Blocked Close Msgs Count: ", clientState.Value.BlockedReplicables.Count);
				stringBuilder.AppendLine(value);
			}
			return stringBuilder.ToString();
		}
	}
}
