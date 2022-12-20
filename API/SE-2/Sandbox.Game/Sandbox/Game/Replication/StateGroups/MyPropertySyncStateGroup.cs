using System;
using System.Collections.Generic;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Multiplayer;
using VRage;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Sync;

namespace Sandbox.Game.Replication.StateGroups
{
	/// <summary>
	/// Property sync state group.
	/// Synchronizes Sync members in SyncType.
	/// </summary>
	public sealed class MyPropertySyncStateGroup : IMyStateGroup, IMyNetObject, IMyEventOwner
	{
		/// <summary>
		/// Server-side data.
		/// </summary>
		private class ServerData
		{
			/// <summary>
			/// Data for each client.
			/// </summary>
			public class DataPerClient
			{
				/// <summary>
				/// Tiny array marking properties that were changed.
				/// </summary>
				public SmallBitField DirtyProperties = new SmallBitField(value: false);

				/// <summary>
				/// Properties sent in last state sync.
				/// </summary>
				public readonly SmallBitField[] SentProperties = new SmallBitField[256];
			}

			/// <summary>
			/// Dictionary containing server-side data for each connected client.
			/// </summary>
			public readonly Dictionary<Endpoint, DataPerClient> ServerClientData = new Dictionary<Endpoint, DataPerClient>();
		}

		/// <summary>
		/// Client-side data.
		/// </summary>
		private class ClientData
		{
			/// <summary>
			/// Client stores here dirty properties.
			/// </summary>
			public SmallBitField DirtyProperties;

			public uint LastUpdateFrame;
		}

		public delegate float PriorityAdjustDelegate(int frameCountWithoutSync, MyClientStateBase clientState, float basePriority);

		protected sealed class SyncPropertyChanged_Implementation_003C_003ESystem_Byte_0023System_Double_0023VRage_Library_Collections_BitReaderWriter : ICallSite<MyPropertySyncStateGroup, byte, double, BitReaderWriter, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyPropertySyncStateGroup @this, in byte propertyIndex, in double propertyTimestampMs, in BitReaderWriter reader, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SyncPropertyChanged_Implementation(propertyIndex, propertyTimestampMs, reader);
			}
		}

		public Func<MyEventContext, ValidationResult> GlobalValidate = (MyEventContext context) => ValidationResult.Passed;

		public PriorityAdjustDelegate PriorityAdjust = (int frames, MyClientStateBase state, float priority) => priority;

<<<<<<< HEAD
		/// <summary>
		/// Client-specific data, null on server.
		/// </summary>
		private readonly ClientData m_clientData = (Sync.IsServer ? null : new ClientData());

		/// <summary>
		/// Server-specific data, null on clients.
		/// </summary>
=======
		private readonly ClientData m_clientData = (Sync.IsServer ? null : new ClientData());

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private readonly ServerData m_serverData = (Sync.IsServer ? new ServerData() : null);

		/// <summary>
		/// List of sync properties.
		/// </summary>
		private ListReader<SyncBase> m_properties;

		/// <summary>
		/// List of timestamps indicating last change of each sync property.
		/// </summary>
		private readonly List<MyTimeSpan> m_propertyTimestamps;

		/// <summary>
		/// Helper constant - marking the timestamp as invalid.
		/// </summary>
		private readonly MyTimeSpan m_invalidTimestamp = MyTimeSpan.FromTicks(long.MinValue);

		public bool IsHighPriority => false;

		public IMyReplicable Owner { get; private set; }

		public bool IsStreaming => false;

		public bool IsValid
		{
			get
			{
				if (Owner != null)
				{
					return Owner.IsValid;
				}
				return false;
			}
		}

		/// <summary>
		/// Get the count of synchronized properties.
		/// </summary>
		public int PropertyCount => m_properties.Count;

		public bool NeedsUpdate => m_clientData.DirtyProperties.Bits != 0;

		/// <summary>
		/// Contructor of property sync state group.
		/// </summary>
		public MyPropertySyncStateGroup(IMyReplicable ownerReplicable, SyncType syncType)
		{
			Owner = ownerReplicable;
			syncType.PropertyChangedNotify += Notify;
			syncType.PropertyCountChanged += OnPropertyCountChanged;
			m_properties = syncType.Properties;
			m_propertyTimestamps = new List<MyTimeSpan>(m_properties.Count);
			if (Sync.IsServer)
			{
				for (int i = 0; i < m_properties.Count; i++)
				{
					m_propertyTimestamps.Add(MyMultiplayer.Static.ReplicationLayer.GetSimulationUpdateTime());
				}
			}
			else
			{
				for (int j = 0; j < m_properties.Count; j++)
				{
					m_propertyTimestamps.Add(m_invalidTimestamp);
				}
			}
		}

		/// <summary>
		/// This function is a callback from Sync property. It is called when the sync property value is changed locally.
		/// </summary>
		private void Notify(SyncBase sync)
		{
			if (Owner == null || sync == null || !Owner.IsValid)
<<<<<<< HEAD
			{
				return;
			}
			if (Sync.IsServer)
			{
=======
			{
				return;
			}
			if (Sync.IsServer)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyTimeSpan simulationUpdateTime = MyMultiplayer.Static.ReplicationLayer.GetSimulationUpdateTime();
				m_propertyTimestamps[sync.Id] = ((simulationUpdateTime >= m_propertyTimestamps[sync.Id]) ? simulationUpdateTime : m_propertyTimestamps[sync.Id]);
				foreach (KeyValuePair<Endpoint, ServerData.DataPerClient> serverClientDatum in m_serverData.ServerClientData)
				{
					if (serverClientDatum.Value != null)
					{
						serverClientDatum.Value.DirtyProperties[sync.Id] = true;
					}
				}
				MyMultiplayer.GetReplicationServer()?.AddToDirtyGroups(this);
			}
			else if (m_propertyTimestamps[sync.Id] != m_invalidTimestamp)
			{
				MyTimeSpan simulationUpdateTime2 = MyMultiplayer.Static.ReplicationLayer.GetSimulationUpdateTime();
				m_propertyTimestamps[sync.Id] = simulationUpdateTime2;
				m_clientData.DirtyProperties[sync.Id] = true;
				(MyMultiplayer.ReplicationLayer as MyReplicationClient)?.AddToUpdates(this);
			}
		}

		/// <summary>
		/// Marks dirty for all clients.
		/// </summary>
		public void MarkDirty()
		{
			if (m_properties.Count == 0)
			{
				return;
<<<<<<< HEAD
			}
			foreach (KeyValuePair<Endpoint, ServerData.DataPerClient> serverClientDatum in m_serverData.ServerClientData)
			{
				serverClientDatum.Value.DirtyProperties.Reset(value: true);
			}
=======
			}
			foreach (KeyValuePair<Endpoint, ServerData.DataPerClient> serverClientDatum in m_serverData.ServerClientData)
			{
				serverClientDatum.Value.DirtyProperties.Reset(value: true);
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyMultiplayer.GetReplicationServer().AddToDirtyGroups(this);
		}

		public void CreateClientData(MyClientStateBase forClient)
		{
			ServerData.DataPerClient dataPerClient = new ServerData.DataPerClient();
			m_serverData.ServerClientData.Add(forClient.EndpointId, dataPerClient);
			if (m_properties.Count > 0)
			{
				dataPerClient.DirtyProperties.Reset(value: true);
			}
		}

		public void DestroyClientData(MyClientStateBase forClient)
		{
			m_serverData.ServerClientData.Remove(forClient.EndpointId);
		}

		public void ClientUpdate(MyTimeSpan clientTimestamp)
		{
			if (m_clientData.DirtyProperties.Bits == 0L || MyMultiplayer.Static.FrameCounter - m_clientData.LastUpdateFrame < 6)
<<<<<<< HEAD
			{
				return;
			}
			foreach (SyncBase property in m_properties)
			{
=======
			{
				return;
			}
			foreach (SyncBase property in m_properties)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (m_clientData.DirtyProperties[property.Id])
				{
					MyMultiplayer.RaiseEvent(this, (Func<MyPropertySyncStateGroup, Action<byte, double, BitReaderWriter>>)((MyPropertySyncStateGroup x) => x.SyncPropertyChanged_Implementation), (byte)property.Id, m_propertyTimestamps[property.Id].Milliseconds, (BitReaderWriter)property, default(EndpointId));
				}
			}
			m_clientData.DirtyProperties.Reset(value: false);
			m_clientData.LastUpdateFrame = MyMultiplayer.Static.FrameCounter;
		}

		public void Destroy()
		{
			Owner = null;
		}

		[Event(null, 248)]
		[Reliable]
		[Server]
		private void SyncPropertyChanged_Implementation(byte propertyIndex, double propertyTimestampMs, BitReaderWriter reader)
		{
			ValidationResult validationResult = GlobalValidate(MyEventContext.Current);
			if (!MyEventContext.Current.IsLocallyInvoked && validationResult != 0)
			{
				SyncBase syncBase = null;
				if (propertyIndex < m_properties.Count)
				{
					syncBase = m_properties[propertyIndex];
				}
				if (syncBase != null && syncBase.ShouldValidate)
				{
					(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, validationResult.HasFlag(ValidationResult.Kick), validationResult.ToString() + " " + m_properties[propertyIndex].DebugName, stackTrace: false);
					return;
				}
			}
			if (propertyIndex >= m_properties.Count)
			{
				return;
			}
			MyMultiplayer.GetReplicationServer();
			Endpoint key = new Endpoint(MyEventContext.Current.Sender, 0);
			MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(propertyTimestampMs);
			bool flag = myTimeSpan >= m_propertyTimestamps[propertyIndex];
			if (reader.ReadData(m_properties[propertyIndex], validate: true, flag))
			{
				m_propertyTimestamps[propertyIndex] = myTimeSpan;
			}
			else if (flag)
			{
				m_serverData.ServerClientData.TryGetValue(key, out var value);
				if (value != null)
				{
					value.DirtyProperties[propertyIndex] = true;
				}
			}
		}

		public void Serialize(BitStream stream, MyClientInfo forClient, MyTimeSpan serverTimestamp, MyTimeSpan lastClientTimestamp, byte packetId, int maxBitPosition, HashSet<string> cachedData)
		{
			if (MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking)
			{
				MyDebug.WriteLine($"Property group - Bit Position Start: {stream.BitPosition}", "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\StateGroups\\MyPropertySyncStateGroup.cs", 296);
			}
			SmallBitField dirtyProperties = default(SmallBitField);
			if (stream.Writing)
			{
				dirtyProperties = m_serverData.ServerClientData[forClient.EndpointId].DirtyProperties;
				stream.WriteUInt64(dirtyProperties.Bits, m_properties.Count);
			}
			else
			{
				dirtyProperties.Bits = stream.ReadUInt64(m_properties.Count);
			}
			if (MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking)
			{
				MyDebug.WriteLine($"Property group - After Dirty Bit Position: {stream.BitPosition}", "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\StateGroups\\MyPropertySyncStateGroup.cs", 315);
				MyDebug.WriteLine($"Property group - Properties {m_properties.Count}", "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\StateGroups\\MyPropertySyncStateGroup.cs", 316);
			}
			for (int i = 0; i < m_properties.Count; i++)
			{
				if (!dirtyProperties[i])
				{
					continue;
				}
				if (stream.Reading)
				{
					MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(stream.ReadDouble());
					if (m_properties[i].Serialize(stream, validate: false, myTimeSpan >= m_propertyTimestamps[i]))
					{
						m_propertyTimestamps[i] = myTimeSpan;
						m_clientData.DirtyProperties[i] = false;
					}
					continue;
				}
				MyMultiplayer.GetReplicationServer();
				double milliseconds = m_propertyTimestamps[i].Milliseconds;
				if (MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking)
				{
					MyDebug.WriteLine($"Property: {i} Time: {milliseconds} Bit Position: {stream.BitPosition}", "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\StateGroups\\MyPropertySyncStateGroup.cs", 340);
				}
				stream.WriteDouble(milliseconds);
				m_properties[i].Serialize(stream, validate: false);
			}
			if (stream.Writing && stream.BitPosition <= maxBitPosition)
			{
				ServerData.DataPerClient dataPerClient = m_serverData.ServerClientData[forClient.EndpointId];
				dataPerClient.SentProperties[packetId].Bits = dataPerClient.DirtyProperties.Bits;
				dataPerClient.DirtyProperties.Bits = 0uL;
			}
			if (MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking)
			{
				MyDebug.WriteLine($"Property group - Bit Position End: {stream.BitPosition}", "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Replication\\StateGroups\\MyPropertySyncStateGroup.cs", 359);
			}
		}

		public void OnAck(MyClientStateBase forClient, byte packetId, bool delivered)
		{
			ServerData.DataPerClient dataPerClient = m_serverData.ServerClientData[forClient.EndpointId];
			if (!delivered)
			{
				dataPerClient.DirtyProperties.Bits |= dataPerClient.SentProperties[packetId].Bits;
				MyMultiplayer.GetReplicationServer().AddToDirtyGroups(this);
			}
		}

		public void ForceSend(MyClientStateBase clientData)
		{
		}

		public void Reset(bool reinit, MyTimeSpan clientTimestamp)
		{
		}

		public bool IsStillDirty(Endpoint forClient)
		{
			return m_serverData.ServerClientData[forClient].DirtyProperties.Bits != 0;
		}

		public MyStreamProcessingState IsProcessingForClient(Endpoint forClient)
		{
			return MyStreamProcessingState.None;
		}

		private void OnPropertyCountChanged()
		{
			if (Sync.IsServer)
			{
				for (int i = m_propertyTimestamps.Count; i < m_properties.Count; i++)
				{
					m_propertyTimestamps.Add(MyMultiplayer.Static.ReplicationLayer.GetSimulationUpdateTime());
				}
			}
			else
			{
				for (int j = m_propertyTimestamps.Count; j < m_properties.Count; j++)
				{
					m_propertyTimestamps.Add(m_invalidTimestamp);
				}
			}
		}
	}
}
