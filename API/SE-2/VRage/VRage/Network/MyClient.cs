using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage.Collections;
using VRage.Library;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Replication;
using VRageMath;

namespace VRage.Network
{
	internal class MyClient
	{
		public class UpdateLayer
		{
			public readonly MyLayers.UpdateLayerDesc Descriptor;

			public readonly int Index;

			public readonly MyDistributedUpdater<List<IMyReplicable>, IMyReplicable> Sender;

			public HashSet<IMyReplicable> Replicables;

			public int UpdateTimer;

			public int LayerPCU;

			public int PreviousLayersPCU;

			public bool Enabled;

			public int TotalCumulativePCU => LayerPCU + PreviousLayersPCU;

			public UpdateLayer(MyLayers.UpdateLayerDesc descriptor, int index, int updateTimer)
			{
				Descriptor = descriptor;
				Index = index;
				UpdateTimer = updateTimer;
				Replicables = new HashSet<IMyReplicable>();
				Sender = new MyDistributedUpdater<List<IMyReplicable>, IMyReplicable>(descriptor.SendInterval);
				Enabled = true;
			}
		}

		private struct MyOrderedPacket
		{
			public byte Id;

			public MyPacket Packet;

			public override string ToString()
			{
				return Id.ToString();
			}
		}

		public readonly MyClientStateBase State;

		private readonly IReplicationServerCallback m_callback;

		public float PriorityMultiplier = 1f;

		public bool IsReady;

		public bool PlayerControllableUsesPredictedPhysics = true;

		private MyTimeSpan m_lastClientRealtime;

		private MyTimeSpan m_lastClientTimestamp;

		private MyTimeSpan m_lastStateSyncTimeStamp;

		private byte m_stateSyncPacketId;

		private byte m_streamingPacketId;

		private byte m_lastClientStreamingAck;

		public readonly Dictionary<IMyReplicable, byte> PermanentReplicables = new Dictionary<IMyReplicable, byte>();

		public readonly HashSet<IMyReplicable> CrucialReplicables = new HashSet<IMyReplicable>();

		public readonly MyConcurrentDictionary<IMyReplicable, MyReplicableClientData> Replicables = new MyConcurrentDictionary<IMyReplicable, MyReplicableClientData>(InstanceComparer<IMyReplicable>.Default);

		public int PendingReplicables;

		public bool WantsBatchCompleteConfirmation = true;

		public readonly MyConcurrentDictionary<IMyReplicable, MyReplicationServer.MyDestroyBlocker> BlockedReplicables = new MyConcurrentDictionary<IMyReplicable, MyReplicationServer.MyDestroyBlocker>();

		public readonly Dictionary<IMyStateGroup, MyStateDataEntry> StateGroups = new Dictionary<IMyStateGroup, MyStateDataEntry>(InstanceComparer<IMyStateGroup>.Default);

		public readonly FastPriorityQueue<MyStateDataEntry> DirtyQueue = new FastPriorityQueue<MyStateDataEntry>(1024);

		private readonly HashSet<string> m_clientCachedData = new HashSet<string>();

		/// <summary>
		/// Maximum PCU the client would like to receive.
		/// </summary>
		public int? PCULimit;

		public MyPacketStatistics Statistics;

		public UpdateLayer[] UpdateLayers;

		public readonly Dictionary<IMyReplicable, UpdateLayer> ReplicableToLayer = new Dictionary<IMyReplicable, UpdateLayer>();

		/// <summary>
		/// Index of the last layer that is enbled.
		/// </summary>
		public int LastEnabledLayer;

		private readonly List<MyOrderedPacket> m_incomingBuffer = new List<MyOrderedPacket>();

		private bool m_incomingBuffering = true;

		private byte m_lastProcessedClientPacketId = byte.MaxValue;

		private readonly MyPacketTracker m_clientTracker = new MyPacketTracker();

		private MyTimeSpan m_lastReceivedTimeStamp = MyTimeSpan.Zero;

		private const int MINIMUM_INCOMING_BUFFER = 4;

		private bool m_enablePlayoutDelayBuffer;

		private int m_orderedCounter;

		private const byte OUT_OF_ORDER_RESET_PROTECTION = 64;

		private const byte OUT_OF_ORDER_ACCEPT_THRESHOLD = 6;

		private byte m_lastReceivedAckId;

		private bool m_waitingForReset;

<<<<<<< HEAD
		private readonly List<IMyStateGroup>[] m_pendingStateSyncAcks = (from s in Enumerable.Range(0, 512)
			select new List<IMyStateGroup>(8)).ToArray();
=======
		private readonly List<IMyStateGroup>[] m_pendingStateSyncAcks = Enumerable.ToArray<List<IMyStateGroup>>(Enumerable.Select<int, List<IMyStateGroup>>(Enumerable.Range(0, 512), (Func<int, List<IMyStateGroup>>)((int s) => new List<IMyStateGroup>(8))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private static readonly MyTimeSpan MAXIMUM_PACKET_GAP = MyTimeSpan.FromSeconds(0.40000000596046448);

		public bool ForcePlayoutDelayBuffer { get; set; }

		public bool UsePlayoutDelayBufferForCharacter { get; set; }

		public bool UsePlayoutDelayBufferForJetpack { get; set; }

		public bool UsePlayoutDelayBufferForGrids { get; set; }

		private bool UsePlayoutDelayBuffer
		{
			get
			{
				if (m_enablePlayoutDelayBuffer || ForcePlayoutDelayBuffer)
				{
					if ((!State.IsControllingCharacter || !UsePlayoutDelayBufferForCharacter) && (!State.IsControllingJetpack || !UsePlayoutDelayBufferForJetpack))
					{
						if (State.IsControllingGrid)
						{
							return UsePlayoutDelayBufferForGrids;
						}
						return false;
					}
					return true;
				}
				return false;
			}
		}

		public byte CurrentStreamingPacketId => m_streamingPacketId;

		public MyClient(MyClientStateBase emptyState, IReplicationServerCallback callback)
		{
			m_callback = callback;
			State = emptyState;
			InitLayers();
		}

		private void InitLayers()
		{
			UpdateLayers = new UpdateLayer[MyLayers.UpdateLayerDescriptors.Count];
			for (int i = 0; i < MyLayers.UpdateLayerDescriptors.Count; i++)
			{
				MyLayers.UpdateLayerDesc descriptor = MyLayers.UpdateLayerDescriptors[i];
				UpdateLayers[i] = new UpdateLayer(descriptor, i, i + 1);
			}
			LastEnabledLayer = UpdateLayers.Length - 1;
		}

		public UpdateLayer CalculateLayerOfReplicable(IMyReplicable rep)
		{
			BoundingBoxD aABB = rep.GetAABB();
			if (!State.Position.HasValue)
			{
				return null;
			}
			for (int i = 0; i < UpdateLayers.Length; i++)
			{
				UpdateLayer updateLayer = UpdateLayers[i];
				BoundingBoxD boundingBoxD = new BoundingBoxD(State.Position.Value - new Vector3D(updateLayer.Descriptor.Radius), State.Position.Value + new Vector3D(updateLayer.Descriptor.Radius));
				if (boundingBoxD.Intersects(aABB))
				{
					return updateLayer;
				}
			}
			return null;
		}

		private void AddIncomingPacketSorted(byte packetId, MyPacket packet)
		{
			MyOrderedPacket myOrderedPacket = default(MyOrderedPacket);
			myOrderedPacket.Id = packetId;
			myOrderedPacket.Packet = packet;
			MyOrderedPacket item = myOrderedPacket;
			int num = m_incomingBuffer.Count - 1;
			while (num >= 0 && packetId < m_incomingBuffer[num].Id && (packetId >= 64 || m_incomingBuffer[num].Id <= 192))
			{
				num--;
			}
			num++;
			m_incomingBuffer.Insert(num, item);
		}

		private bool ProcessIncomingPacket(MyPacket packet, bool skipControls, MyTimeSpan serverTimeStamp)
		{
			byte b = packet.BitStream.ReadByte();
			m_lastClientTimestamp = MyTimeSpan.FromMilliseconds(packet.BitStream.ReadDouble());
			m_lastClientRealtime = MyTimeSpan.FromMilliseconds(packet.BitStream.ReadDouble());
			m_lastReceivedTimeStamp = serverTimeStamp;
			Statistics.Update(m_clientTracker.Add(b));
			bool flag = b <= m_lastProcessedClientPacketId && (m_lastProcessedClientPacketId <= 192 || b >= 64);
			if (!flag)
			{
				m_lastProcessedClientPacketId = b;
			}
			State.Serialize(packet.BitStream, flag || skipControls);
			if (!packet.BitStream.CheckTerminator())
			{
				throw new EndOfStreamException("Invalid BitStream terminator");
			}
			return flag;
		}

		private void UpdateIncoming(MyTimeSpan serverTimeStamp, bool usePlayoutDelayBuffer, bool skipAll = false)
		{
			if (m_incomingBuffer.Count == 0 || (usePlayoutDelayBuffer && m_incomingBuffering && m_incomingBuffer.Count < 4 && !skipAll))
			{
				if (MyCompilationSymbols.EnableNetworkServerIncomingPacketTracking)
				{
					_ = m_incomingBuffer.Count;
				}
				m_incomingBuffering = true;
				m_lastProcessedClientPacketId = byte.MaxValue;
				State.Update();
				return;
			}
			if (m_incomingBuffering)
			{
				m_lastProcessedClientPacketId = (byte)(m_incomingBuffer[0].Id - 1);
			}
			m_incomingBuffering = false;
			string text = "";
			bool flag2;
			do
			{
				bool flag = m_incomingBuffer.Count > 4 || skipAll;
				bool num = ProcessIncomingPacket(m_incomingBuffer[0].Packet, flag, serverTimeStamp);
				flag2 = num || flag;
				if (num)
				{
					m_enablePlayoutDelayBuffer = true;
					m_orderedCounter = 0;
				}
				else if (m_enablePlayoutDelayBuffer)
				{
					m_orderedCounter++;
					if (m_orderedCounter > 3600)
					{
						m_enablePlayoutDelayBuffer = false;
					}
				}
				if (MyCompilationSymbols.EnableNetworkServerIncomingPacketTracking)
				{
					text = m_incomingBuffer[0].Id + ", " + text;
					if (flag2)
					{
						text = "-" + text;
					}
				}
				m_incomingBuffer[0].Packet.Return();
				m_incomingBuffer.RemoveAt(0);
			}
			while (m_incomingBuffer.Count > 4 || ((!usePlayoutDelayBuffer || flag2) && m_incomingBuffer.Count > 0));
			_ = MyCompilationSymbols.EnableNetworkServerIncomingPacketTracking;
		}

		private void ClearBufferedIncomingPackets(MyTimeSpan serverTimeStamp)
		{
			if (m_incomingBuffer.Count > 0)
			{
				UpdateIncoming(serverTimeStamp, usePlayoutDelayBuffer: false, skipAll: true);
			}
		}

		public void OnClientUpdate(MyPacket packet, MyTimeSpan serverTimeStamp)
		{
			if (!UsePlayoutDelayBuffer)
			{
				ClearBufferedIncomingPackets(serverTimeStamp);
			}
			long bitPosition = packet.BitStream.BitPosition;
			byte packetId = packet.BitStream.ReadByte();
			packet.BitStream.SetBitPositionRead(bitPosition);
			AddIncomingPacketSorted(packetId, packet);
		}

		public void Update(MyTimeSpan serverTimeStamp)
		{
			UpdateIncoming(serverTimeStamp, UsePlayoutDelayBuffer);
			if (serverTimeStamp > m_lastReceivedTimeStamp + MAXIMUM_PACKET_GAP)
			{
				State.ResetControlledEntityControls();
			}
			Statistics.PlayoutDelayBufferSize = (byte)m_incomingBuffer.Count;
		}

		/// <summary>
		/// Returns true when current packet is closely preceding last packet (is within threshold)
		/// </summary>
		private static bool IsPreceding(int currentPacketId, int lastPacketId, int threshold)
		{
			if (lastPacketId < currentPacketId)
			{
				lastPacketId += 256;
			}
			return lastPacketId - currentPacketId <= threshold;
		}

		public bool IsAckAvailable()
		{
			byte b = (byte)(m_lastReceivedAckId - 6);
			byte b2 = (byte)(m_stateSyncPacketId + 1);
			if (m_waitingForReset || b2 == b)
			{
				m_waitingForReset = true;
				return false;
			}
			return true;
		}

		public void OnClientAcks(MyPacket packet)
		{
			byte b = packet.BitStream.ReadByte();
			bool num = packet.BitStream.ReadBool();
			byte b2 = packet.BitStream.ReadByte();
			if (num)
			{
				byte b3 = m_lastClientStreamingAck;
				do
				{
					b3 = (byte)(b3 + 1);
					RaiseAck(b3, delivered: true, streaming: true);
				}
				while (b3 != b2);
				m_lastClientStreamingAck = b2;
			}
			byte b4 = packet.BitStream.ReadByte();
			for (int i = 0; i < b4; i++)
			{
				OnAck(packet.BitStream.ReadByte());
			}
			if (!packet.BitStream.CheckTerminator())
			{
				throw new EndOfStreamException("Invalid BitStream terminator");
			}
			byte b5;
			byte b6;
			if (m_waitingForReset)
			{
				m_stateSyncPacketId = (byte)(b + 64);
				CheckStateSyncPacketId();
				b5 = (byte)(m_stateSyncPacketId + 1);
				b6 = (byte)(b5 - 64);
				m_waitingForReset = false;
			}
			else
			{
				b5 = (byte)(m_stateSyncPacketId + 1);
				b6 = (byte)(m_lastReceivedAckId - 6);
			}
			for (byte b7 = b5; b7 != b6; b7 = (byte)(b7 + 1))
			{
				RaiseAck(b7, delivered: false);
			}
		}

		private void OnAck(byte ackId)
		{
			if (IsPreceding(ackId, m_lastReceivedAckId, 6))
			{
				RaiseAck(ackId, delivered: true);
				return;
			}
			RaiseAck(ackId, delivered: true);
			m_lastReceivedAckId = ackId;
		}

		private void RaiseAck(byte ackId, bool delivered, bool streaming = false)
		{
			List<IMyStateGroup> list = m_pendingStateSyncAcks[streaming ? (ackId + 256) : ackId];
			foreach (IMyStateGroup item in list)
			{
				if (StateGroups.ContainsKey(item))
				{
					item.OnAck(State, ackId, delivered);
				}
			}
			list.Clear();
		}

		private void AddPendingAck(byte stateSyncPacketId, IMyStateGroup group)
		{
			m_pendingStateSyncAcks[stateSyncPacketId].Add(group);
		}

		public void AddPendingAck(IMyStateGroup group, bool streaming)
		{
			int num = (streaming ? (m_streamingPacketId + 256) : m_stateSyncPacketId);
			m_pendingStateSyncAcks[num].Add(group);
		}

		public bool IsReplicableReady(IMyReplicable replicable)
		{
			if (Replicables.TryGetValue(replicable, out var value) && !value.IsPending)
			{
				return !value.IsStreaming;
			}
			return false;
		}

		public bool IsReplicablePending(IMyReplicable replicable)
		{
			if (Replicables.TryGetValue(replicable, out var value))
			{
				if (!value.IsPending)
				{
					return value.IsStreaming;
				}
				return true;
			}
			return false;
		}

		public bool HasReplicable(IMyReplicable replicable)
		{
			return Replicables.ContainsKey(replicable);
		}

		public bool WritePacketHeader(BitStream sendStream, bool streaming, MyTimeSpan serverTimeStamp, out MyTimeSpan clientTimestamp)
		{
			m_lastStateSyncTimeStamp = serverTimeStamp;
			if (streaming)
			{
				m_streamingPacketId++;
			}
			else
			{
				m_stateSyncPacketId++;
			}
			if (!CheckStateSyncPacketId(streaming))
			{
				clientTimestamp = MyTimeSpan.Zero;
				return false;
			}
			byte value = (streaming ? m_streamingPacketId : m_stateSyncPacketId);
			sendStream.WriteBool(streaming);
			sendStream.WriteByte(value);
			Statistics.Write(sendStream, m_callback.GetUpdateTime());
			sendStream.WriteDouble(serverTimeStamp.Milliseconds);
			sendStream.WriteDouble(m_lastClientTimestamp.Milliseconds);
			m_lastClientTimestamp = MyTimeSpan.FromMilliseconds(-1.0);
			sendStream.WriteDouble(m_lastClientRealtime.Milliseconds);
			m_lastClientRealtime = MyTimeSpan.FromMilliseconds(-1.0);
			m_callback.WriteCustomState(sendStream);
			clientTimestamp = serverTimeStamp;
			return true;
		}

		private bool CheckStateSyncPacketId(bool streaming = false)
		{
			byte b;
			int num;
			if (streaming)
			{
				b = m_streamingPacketId;
				num = 256;
			}
			else
			{
				b = m_stateSyncPacketId;
				num = 0;
			}
			byte b2 = 0;
			byte b3 = b;
			while (m_pendingStateSyncAcks[b3 + num].Count != 0)
			{
				b3 = (byte)(b3 + 1);
				b2 = (byte)(b2 + 1);
				if (b == b3)
				{
					Statistics.PendingPackets = b2;
					return false;
				}
			}
			if (streaming)
			{
				m_streamingPacketId = b3;
			}
			else
			{
				m_stateSyncPacketId = b3;
			}
			Statistics.PendingPackets = b2;
			return true;
		}

		public void Serialize(IMyStateGroup group, BitStream sendStream, MyTimeSpan timeStamp, int messageBitSize = int.MaxValue, bool streaming = false)
		{
			if (timeStamp == MyTimeSpan.Zero)
			{
				timeStamp = State.ClientTimeStamp;
			}
			group.Serialize(sendStream, new MyClientInfo(this), timeStamp, m_lastClientTimestamp, streaming ? m_streamingPacketId : m_stateSyncPacketId, messageBitSize, m_clientCachedData);
		}

		public bool SendStateSync(MyStateDataEntry stateGroupEntry, int mtuBytes, ref MyPacketDataBitStreamBase data, MyTimeSpan serverTimeStamp)
		{
			if (data == null)
			{
				data = m_callback.GetBitStreamPacketData();
				if (!WritePacketHeader(data.Stream, streaming: false, serverTimeStamp, out var clientTimestamp))
				{
					data.Return();
					data = null;
					return false;
				}
				State.ClientTimeStamp = clientTimestamp;
			}
			BitStream stream = data.Stream;
			int num = 8 * (mtuBytes - 2);
			long bitPosition = stream.BitPosition;
			_ = MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking;
			stream.Terminate();
			_ = MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking;
			stream.WriteNetworkId(stateGroupEntry.GroupId);
			if (MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking)
			{
				stateGroupEntry.Group.Owner.ToString();
				_ = stateGroupEntry.Group.GetType().FullName;
			}
			long bitPosition2 = stream.BitPosition;
			stream.WriteInt16(0);
			long bitPosition3 = stream.BitPosition;
			Serialize(stateGroupEntry.Group, stream, MyTimeSpan.Zero, num);
			long bitPosition4 = stream.BitPosition;
			stream.SetBitPositionWrite(bitPosition2);
			stream.WriteInt16((short)(bitPosition4 - bitPosition3));
			stream.SetBitPositionWrite(bitPosition4);
			long num2 = stream.BitPosition - bitPosition;
			_ = MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking;
			if (num2 > 0 && stream.BitPosition <= num)
			{
				AddPendingAck(m_stateSyncPacketId, stateGroupEntry.Group);
			}
			else
			{
				if (MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking)
				{
					stateGroupEntry.Group.Owner.ToString();
					_ = stateGroupEntry.Group.GetType().FullName;
				}
				stateGroupEntry.Group.OnAck(State, m_stateSyncPacketId, delivered: false);
				stream.SetBitPositionWrite(bitPosition);
				data.Stream.Terminate();
				m_callback.SendStateSync(data, State.EndpointId, reliable: false);
				data = null;
			}
			return true;
		}

		private void SendEmptyStateSync(MyTimeSpan serverTimeStamp)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			if (!WritePacketHeader(bitStreamPacketData.Stream, streaming: false, serverTimeStamp, out var _))
			{
				bitStreamPacketData.Return();
				return;
			}
			bitStreamPacketData.Stream.Terminate();
			m_callback.SendStateSync(bitStreamPacketData, State.EndpointId, reliable: false);
		}

		public void SendUpdate(MyTimeSpan serverTimeStamp)
		{
			if (serverTimeStamp > m_lastStateSyncTimeStamp + MAXIMUM_PACKET_GAP)
			{
				SendEmptyStateSync(serverTimeStamp);
			}
		}

		public bool RemoveCache(IMyReplicable replicable, string storageName)
		{
			if (replicable == null || !Replicables.ContainsKey(replicable))
			{
				return m_clientCachedData.Remove(storageName);
			}
			return false;
		}

		public void ResetLayerTimers()
		{
			int num = 0;
			UpdateLayer[] updateLayers = UpdateLayers;
			for (int i = 0; i < updateLayers.Length; i++)
			{
				updateLayers[i].UpdateTimer = num++;
			}
		}
	}
}
