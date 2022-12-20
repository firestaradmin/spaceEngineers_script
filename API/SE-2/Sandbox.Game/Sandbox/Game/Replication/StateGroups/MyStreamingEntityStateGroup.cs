using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sandbox.Engine.Multiplayer;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.Replication.StateGroups
{
	internal class MyStreamingEntityStateGroup<T> : IMyStreamingStateGroup, IMyStateGroup, IMyNetObject, IMyEventOwner where T : IMyStreamableReplicable
	{
		private class StreamPartInfo : IComparable<StreamPartInfo>
		{
			public int StartIndex;

			public long NumBits;

			public short Position;

			public int CompareTo(StreamPartInfo b)
			{
				return StartIndex.CompareTo(b.StartIndex);
			}
		}

		private class StreamClientData
		{
			public short CurrentPart;

			public short NumParts;

			public int LastPosition;

			public byte[] ObjectData;

			public bool CreatingData;

			public bool Incomplete = true;

			public bool Dirty = true;

			public long RemainingBits;

			public long UncompressedSize;

			public bool ForceSend;

			public byte? LastSent;

			public readonly Dictionary<byte, StreamPartInfo> SendPackets = new Dictionary<byte, StreamPartInfo>();

			public readonly List<StreamPartInfo> FailedIncompletePackets = new List<StreamPartInfo>();
		}

		private long m_streamSize = 8000L;

		private const int HEADER_SIZE = 97;

		private const int SAFE_VALUE = 128;

		private const int BitStreamLengthBits = 34;

		private bool m_streamed;

		private Dictionary<Endpoint, StreamClientData> m_clientStreamData;

		private SortedList<StreamPartInfo, byte[]> m_receivedParts;

		private short m_numPartsToReceive;

		private int m_receivedBytes;

		private long m_uncompressedSize;

		private T Instance { get; set; }

		public IMyReplicable Owner { get; private set; }

		public bool NeedsUpdate => false;

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

		public bool IsHighPriority => false;

		public bool IsStreaming => true;

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool HasStreamed(Endpoint endpoint)
		{
			if (m_clientStreamData != null && m_clientStreamData.TryGetValue(endpoint, out var value) && !value.Dirty)
			{
				return !value.Incomplete;
			}
			return false;
		}

		public MyStreamingEntityStateGroup(T obj, IMyReplicable owner)
		{
			Instance = obj;
			Owner = owner;
		}

		public void CreateClientData(MyClientStateBase forClient)
		{
			if (m_clientStreamData == null)
			{
				m_clientStreamData = new Dictionary<Endpoint, StreamClientData>();
			}
			if (!m_clientStreamData.TryGetValue(forClient.EndpointId, out var _))
			{
				m_clientStreamData[forClient.EndpointId] = new StreamClientData();
			}
		}

		public void DestroyClientData(MyClientStateBase forClient)
		{
			if (m_clientStreamData != null)
			{
				m_clientStreamData.Remove(forClient.EndpointId);
			}
		}

		public void ClientUpdate(MyTimeSpan clientTimestamp)
		{
		}

		public void Destroy()
		{
			if (m_receivedParts != null)
			{
				((SortedList<MyStreamingEntityStateGroup<StreamPartInfo>.StreamPartInfo, byte[]>)(object)m_receivedParts).Clear();
				m_receivedParts = null;
			}
		}

		private unsafe bool ReadPart(ref BitStream stream, byte packetId)
		{
			m_numPartsToReceive = stream.ReadInt16();
			short startIndex = stream.ReadInt16();
			long num = stream.ReadInt64(34);
			int num2 = (int)MyLibraryUtils.GetDivisionCeil(num, 8L);
			long num3 = stream.BitLength - stream.BitPosition;
			if (num3 < num)
			{
				MyLog.Default.WriteLine("trying to read more than there is in stream. Total num parts : " + m_numPartsToReceive + " current part : " + startIndex + " bits to read : " + num + " bits in stream : " + num3 + " replicable : " + Instance.ToString());
				return false;
			}
			if (m_receivedParts == null)
			{
				m_receivedParts = (SortedList<StreamPartInfo, byte[]>)(object)new SortedList<MyStreamingEntityStateGroup<StreamPartInfo>.StreamPartInfo, byte[]>();
			}
			m_receivedBytes += num2;
			byte[] array = new byte[num2];
			fixed (byte* ptr = array)
			{
				stream.ReadMemory(ptr, num);
			}
			StreamPartInfo streamPartInfo = new StreamPartInfo();
			streamPartInfo.NumBits = num;
			streamPartInfo.StartIndex = startIndex;
			((SortedList<MyStreamingEntityStateGroup<StreamPartInfo>.StreamPartInfo, byte[]>)(object)m_receivedParts).set_Item((MyStreamingEntityStateGroup<StreamPartInfo>.StreamPartInfo)(object)streamPartInfo, array);
			return true;
		}

		private void ProcessRead(BitStream stream, byte packetId)
		{
			if (stream.BitLength == stream.BitPosition)
			{
				return;
			}
			if (m_streamed)
			{
				stream.ReadBool();
				stream.ReadInt64(34);
			}
			else if (stream.ReadBool())
			{
				long num = stream.ReadInt64(34);
				if (num != 0L)
				{
					m_uncompressedSize = num;
					if (!ReadPart(ref stream, packetId))
					{
						m_receivedParts = null;
						Instance.LoadCancel();
					}
<<<<<<< HEAD
					else if (m_receivedParts.Count == m_numPartsToReceive)
=======
					else if (((SortedList<MyStreamingEntityStateGroup<StreamPartInfo>.StreamPartInfo, byte[]>)(object)m_receivedParts).get_Count() == m_numPartsToReceive)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						m_streamed = true;
						CreateReplicable(m_uncompressedSize);
					}
				}
			}
			else
			{
				MyLog.Default.WriteLine("received empty state group");
				if (m_receivedParts != null)
				{
					((SortedList<MyStreamingEntityStateGroup<StreamPartInfo>.StreamPartInfo, byte[]>)(object)m_receivedParts).Clear();
				}
				m_receivedParts = null;
				Instance.LoadCancel();
			}
		}

		private unsafe void CreateReplicable(long uncompressedSize)
		{
			byte[] array = new byte[m_receivedBytes];
			int num = 0;
			foreach (KeyValuePair<StreamPartInfo, byte[]> item in (SortedList<MyStreamingEntityStateGroup<StreamPartInfo>.StreamPartInfo, byte[]>)(object)m_receivedParts)
			{
				Buffer.BlockCopy(item.Value, 0, array, num, item.Value.Length);
				num += item.Value.Length;
			}
			byte[] array2 = MemoryCompressor.Decompress(array);
			BitStream bitStream = new BitStream();
			bitStream.ResetWrite();
			fixed (byte* ptr = array2)
			{
				bitStream.SerializeMemory(ptr, uncompressedSize);
			}
			bitStream.ResetRead();
			Instance.LoadDone(bitStream);
			if (!bitStream.CheckTerminator())
			{
				MyLog.Default.WriteLine("Streaming entity: Invalid stream terminator");
			}
			bitStream.Dispose();
			if (m_receivedParts != null)
			{
				((SortedList<MyStreamingEntityStateGroup<StreamPartInfo>.StreamPartInfo, byte[]>)(object)m_receivedParts).Clear();
			}
			m_receivedParts = null;
			m_receivedBytes = 0;
		}

		private void ProcessWrite(int maxBitPosition, BitStream stream, Endpoint forClient, byte packetId, HashSet<string> cachedData)
		{
			StreamClientData streamClientData = m_clientStreamData[forClient];
			if (streamClientData.FailedIncompletePackets.Count > 0)
			{
				WriteIncompletePacket(streamClientData, packetId, ref stream);
				return;
			}
			long num = 0L;
			if (streamClientData.ObjectData == null)
			{
				SaveReplicable(streamClientData, cachedData, forClient);
				return;
			}
			if (streamClientData.LastSent.HasValue || !streamClientData.Incomplete)
			{
				stream.WriteBool(value: true);
				stream.WriteInt64(0L, 34);
				return;
			}
			streamClientData.LastSent = packetId;
			int num2 = Math.Min(maxBitPosition, 8388608);
			m_streamSize = MyLibraryUtils.GetDivisionCeil(num2 - stream.BitPosition - 97 - 128, 8L) * 8;
			streamClientData.NumParts = (short)MyLibraryUtils.GetDivisionCeil(streamClientData.ObjectData.Length * 8, m_streamSize);
			num = streamClientData.RemainingBits;
			if (num == 0L)
			{
				streamClientData.ForceSend = false;
				streamClientData.Dirty = false;
				stream.WriteBool(value: false);
				return;
			}
			stream.WriteBool(value: true);
			stream.WriteInt64(streamClientData.UncompressedSize, 34);
			if (num > m_streamSize || streamClientData.Incomplete)
			{
				WritePart(ref num, streamClientData, packetId, ref stream);
				streamClientData.Incomplete = streamClientData.RemainingBits > 0;
			}
			else
			{
				WriteWhole(num, streamClientData, packetId, ref stream);
			}
		}

		private unsafe void WriteIncompletePacket(StreamClientData clientData, byte packetId, ref BitStream stream)
		{
			if (clientData.ObjectData == null)
			{
				clientData.FailedIncompletePackets.Clear();
				return;
			}
			StreamPartInfo streamPartInfo = clientData.FailedIncompletePackets[0];
			clientData.FailedIncompletePackets.Remove(streamPartInfo);
			clientData.SendPackets[packetId] = streamPartInfo;
			stream.WriteBool(value: true);
			stream.WriteInt64(clientData.UncompressedSize, 34);
			stream.WriteInt16(clientData.NumParts);
			stream.WriteInt16(streamPartInfo.Position);
			stream.WriteInt64(streamPartInfo.NumBits, 34);
			fixed (byte* ptr = &clientData.ObjectData[streamPartInfo.StartIndex])
			{
				stream.WriteMemory(ptr, streamPartInfo.NumBits);
			}
		}

		private unsafe void WritePart(ref long bitsToSend, StreamClientData clientData, byte packetId, ref BitStream stream)
		{
			bitsToSend = Math.Min(m_streamSize, clientData.RemainingBits);
			StreamPartInfo streamPartInfo = new StreamPartInfo
			{
				StartIndex = clientData.LastPosition,
				NumBits = bitsToSend
			};
			clientData.LastPosition = streamPartInfo.StartIndex + (int)MyLibraryUtils.GetDivisionCeil(m_streamSize, 8L);
			clientData.SendPackets[packetId] = streamPartInfo;
			clientData.RemainingBits = Math.Max(0L, clientData.RemainingBits - m_streamSize);
			stream.WriteInt16(clientData.NumParts);
			stream.WriteInt16(clientData.CurrentPart);
			streamPartInfo.Position = clientData.CurrentPart;
			clientData.CurrentPart++;
			stream.WriteInt64(bitsToSend, 34);
			fixed (byte* ptr = &clientData.ObjectData[streamPartInfo.StartIndex])
			{
				stream.WriteMemory(ptr, bitsToSend);
			}
		}

		private unsafe void WriteWhole(long bitsToSend, StreamClientData clientData, byte packetId, ref BitStream stream)
		{
			StreamPartInfo value = new StreamPartInfo
			{
				StartIndex = 0,
				NumBits = bitsToSend,
				Position = 0
			};
			clientData.SendPackets[packetId] = value;
			clientData.RemainingBits = 0L;
			clientData.Dirty = false;
			clientData.ForceSend = false;
			stream.WriteInt16(1);
			stream.WriteInt16(0);
			stream.WriteInt64(bitsToSend, 34);
			fixed (byte* ptr = clientData.ObjectData)
			{
				stream.WriteMemory(ptr, bitsToSend);
			}
		}

		public void Serialize(BitStream stream, MyClientInfo forClient, MyTimeSpan serverTimestamp, MyTimeSpan lastClientTimestamp, byte packetId, int maxBitPosition, HashSet<string> cachedData)
		{
			if (stream != null && stream.Reading)
			{
				ProcessRead(stream, packetId);
			}
			else
			{
				ProcessWrite(maxBitPosition, stream, forClient.EndpointId, packetId, cachedData);
			}
		}

		private void SaveReplicable(StreamClientData clientData, HashSet<string> cachedData, Endpoint forClient)
		{
			BitStream str = new BitStream();
			str.ResetWrite();
			clientData.CreatingData = true;
			Instance.Serialize(str, cachedData, forClient, delegate
			{
				WriteClientData(str, clientData);
			});
		}

		private unsafe void WriteClientData(BitStream str, StreamClientData clientData)
		{
			str.Terminate();
			str.ResetRead();
			long bitLength = str.BitLength;
			byte[] array = new byte[str.ByteLength];
			fixed (byte* ptr = array)
			{
				str.SerializeMemory(ptr, bitLength);
			}
			str.Dispose();
			clientData.CurrentPart = 0;
			clientData.ObjectData = MemoryCompressor.Compress(array);
			clientData.UncompressedSize = bitLength;
			clientData.RemainingBits = clientData.ObjectData.Length * 8;
			clientData.CreatingData = false;
		}

		public void OnAck(MyClientStateBase forClient, byte packetId, bool delivered)
		{
			if (m_clientStreamData.TryGetValue(forClient.EndpointId, out var value) && value.LastSent == packetId)
			{
				value.LastSent = null;
				if (value.RemainingBits == 0L)
				{
					value.Dirty = false;
					value.ForceSend = false;
				}
			}
		}

		public void ForceSend(MyClientStateBase clientData)
		{
			StreamClientData streamClientData = m_clientStreamData[clientData.EndpointId];
			streamClientData.ForceSend = true;
			SaveReplicable(streamClientData, null, clientData.EndpointId);
		}

		public void Reset(bool reinit, MyTimeSpan clientTimestamp)
		{
		}

		public bool IsStillDirty(Endpoint forClient)
		{
			return m_clientStreamData[forClient].Dirty;
		}

		public MyStreamProcessingState IsProcessingForClient(Endpoint forClient)
		{
			if (m_clientStreamData.TryGetValue(forClient, out var value))
			{
				if (!value.CreatingData)
				{
					if (value.ObjectData == null)
					{
						return MyStreamProcessingState.None;
					}
					return MyStreamProcessingState.Finished;
				}
				return MyStreamProcessingState.Processing;
			}
			return MyStreamProcessingState.None;
		}

		[Conditional("__RANDOM_UNDEFINED_SYMBOL__")]
		private void Log(string message)
		{
			((MyReplicationLayer)MyMultiplayer.ReplicationLayer).TryGetNetworkIdByObject(this, out var networkId);
			MyLog.Default.WriteLine($"Streaming[{networkId}]: {message}");
		}
	}
}
