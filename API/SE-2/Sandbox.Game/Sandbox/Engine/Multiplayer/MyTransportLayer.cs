using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sandbox.Engine.Networking;
using VRage;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Profiler;

namespace Sandbox.Engine.Multiplayer
{
	internal class MyTransportLayer
	{
		private struct HandlerId : IEquatable<HandlerId>
		{
			public MyMessageId messageId;

			public byte receiverIndex;

			public bool Equals(HandlerId other)
			{
				if (messageId == other.messageId)
				{
					return receiverIndex == other.receiverIndex;
				}
				return false;
			}
		}

		private static readonly int m_messageTypeCount;

		private readonly Queue<int>[] m_slidingWindows = Enumerable.ToArray<Queue<int>>(Enumerable.Select<int, Queue<int>>(Enumerable.Range(0, m_messageTypeCount), (Func<int, Queue<int>>)((int s) => new Queue<int>(120))));

		private readonly int[] m_thisFrameTraffic = new int[m_messageTypeCount];

		private bool m_isBuffering;

		private readonly int m_channel;

		private List<MyPacket> m_buffer;

		private readonly Dictionary<HandlerId, Action<MyPacket>> m_handlers = new Dictionary<HandlerId, Action<MyPacket>>();

		public bool IsProcessingBuffer { get; private set; }

		/// <summary>
		/// Setting to false will process buffer
		/// </summary>
		public bool IsBuffering
		{
			get
			{
				return m_isBuffering;
			}
			set
			{
				m_isBuffering = value;
				if (m_isBuffering && m_buffer == null)
				{
					m_buffer = new List<MyPacket>();
				}
				else if (!m_isBuffering && m_buffer != null)
				{
					ProcessBuffer();
					m_buffer = null;
				}
			}
		}

		public Action<ulong> DisconnectPeerOnError { get; set; }

		static MyTransportLayer()
		{
			m_messageTypeCount = (int)(MyEnum<MyMessageId>.Range.Max + 1);
		}

		public MyTransportLayer(int channel)
		{
			m_channel = channel;
			DisconnectPeerOnError = null;
			MyNetworkReader.SetHandler(channel, HandleMessage, delegate(ulong x)
			{
				DisconnectPeerOnError(x);
			});
		}

		public void SendFlush(ulong sendTo)
		{
			MyNetworkWriter.SendPacket(InitSendStream(new EndpointId(sendTo), MyP2PMessageEnum.ReliableWithBuffering, MyMessageId.FLUSH, 0));
		}

		private MyNetworkWriter.MyPacketDescriptor InitSendStream(EndpointId endpoint, MyP2PMessageEnum msgType, MyMessageId msgId, byte index = 0)
		{
			MyNetworkWriter.MyPacketDescriptor packetDescriptor = MyNetworkWriter.GetPacketDescriptor(endpoint, msgType, m_channel);
			packetDescriptor.Header.WriteByte((byte)msgId);
			packetDescriptor.Header.WriteByte(index);
			return packetDescriptor;
		}

		public void SendMessage(MyMessageId id, IPacketData data, bool reliable, EndpointId endpoint, byte index = 0)
		{
			MyNetworkWriter.MyPacketDescriptor myPacketDescriptor = InitSendStream(endpoint, reliable ? MyP2PMessageEnum.ReliableWithBuffering : MyP2PMessageEnum.Unreliable, id, index);
			myPacketDescriptor.Data = data;
			MyNetworkWriter.SendPacket(myPacketDescriptor);
		}

		public void SendMessage(MyMessageId id, IPacketData data, bool reliable, List<EndpointId> endpoints, byte index = 0)
		{
			MyNetworkWriter.MyPacketDescriptor myPacketDescriptor = InitSendStream(EndpointId.Null, reliable ? MyP2PMessageEnum.ReliableWithBuffering : MyP2PMessageEnum.Unreliable, id, index);
			myPacketDescriptor.Recipients.AddRange(endpoints);
			myPacketDescriptor.Data = data;
			MyNetworkWriter.SendPacket(myPacketDescriptor);
		}

		private void ProfilePacketStatistics(bool begin)
		{
			if (begin)
			{
				MyStatsGraph.ProfileAdvanced(begin: true);
				MyStatsGraph.ProfilePacketStatistics(begin: true);
			}
			else
			{
				MyStatsGraph.ProfilePacketStatistics(begin: false);
				MyStatsGraph.ProfileAdvanced(begin: false);
			}
		}

		public void Tick()
		{
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			ProfilePacketStatistics(begin: true);
<<<<<<< HEAD
			MyStatsGraph.Begin("Average data", int.MaxValue, "Tick", 135, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
=======
			MyStatsGraph.Begin("Average data", int.MaxValue, "Tick", 137, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			for (int i = 0; i < m_messageTypeCount; i++)
			{
				Queue<int> val = m_slidingWindows[i];
				val.Enqueue(m_thisFrameTraffic[i]);
				m_thisFrameTraffic[i] = 0;
				while (val.get_Count() > 60)
				{
					val.Dequeue();
				}
				int num2 = 0;
				Enumerator<int> enumerator = val.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						num2 += current;
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				if (num2 > 0)
				{
<<<<<<< HEAD
					MyStatsGraph.Begin(MyEnum<MyMessageId>.GetName((MyMessageId)i), int.MaxValue, "Tick", 152, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
					MyStatsGraph.End((float)num2 / 60f, (float)num2 / 1024f, "{0} KB/s", "{0} B", null, 0, "Tick", 153, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
				}
				num += num2;
			}
			MyStatsGraph.End((float)num / 60f, (float)num / 1024f, "{0} KB/s", "{0} B", null, 0, "Tick", 157, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
=======
					MyStatsGraph.Begin(MyEnum<MyMessageId>.GetName((MyMessageId)i), int.MaxValue, "Tick", 154, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
					MyStatsGraph.End((float)num2 / 60f, (float)num2 / 1024f, "{0} KB/s", "{0} B", null, 0, "Tick", 155, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
				}
				num += num2;
			}
			MyStatsGraph.End((float)num / 60f, (float)num / 1024f, "{0} KB/s", "{0} B", null, 0, "Tick", 159, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ProfilePacketStatistics(begin: false);
		}

		private void ProcessBuffer()
		{
			try
			{
				IsProcessingBuffer = true;
				ProfilePacketStatistics(begin: true);
<<<<<<< HEAD
				MyStatsGraph.Begin("Live data", 0, "ProcessBuffer", 167, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
=======
				MyStatsGraph.Begin("Live data", 0, "ProcessBuffer", 169, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				foreach (MyPacket item in m_buffer)
				{
					ProcessMessage(item);
				}
<<<<<<< HEAD
				MyStatsGraph.End(null, 0f, "", "{0} B", null, 0, "ProcessBuffer", 172, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
=======
				MyStatsGraph.End(null, 0f, "", "{0} B", null, 0, "ProcessBuffer", 174, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				ProfilePacketStatistics(begin: false);
			}
			finally
			{
				IsProcessingBuffer = false;
			}
		}

		private void HandleMessage(MyPacket p)
		{
			long bitPosition = p.BitStream.BitPosition;
			MyMessageId myMessageId = (MyMessageId)p.BitStream.ReadByte();
			if (myMessageId == MyMessageId.FLUSH)
			{
				ClearBuffer();
				p.Return();
				return;
			}
			p.BitStream.SetBitPositionRead(bitPosition);
			if (IsBuffering && myMessageId != MyMessageId.JOIN_RESULT && myMessageId != MyMessageId.WORLD_DATA && myMessageId != MyMessageId.WORLD && myMessageId != MyMessageId.PLAYER_DATA)
			{
				m_buffer.Add(p);
				return;
			}
			ProfilePacketStatistics(begin: true);
<<<<<<< HEAD
			MyStatsGraph.Begin("Live data", 0, "HandleMessage", 202, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
			ProcessMessage(p);
			MyStatsGraph.End(null, 0f, "", "{0} B", null, 0, "HandleMessage", 204, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
=======
			MyStatsGraph.Begin("Live data", 0, "HandleMessage", 204, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
			ProcessMessage(p);
			MyStatsGraph.End(null, 0f, "", "{0} B", null, 0, "HandleMessage", 206, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ProfilePacketStatistics(begin: false);
		}

		private void ProcessMessage(MyPacket p)
		{
			HandlerId key = default(HandlerId);
			key.messageId = (MyMessageId)p.BitStream.ReadByte();
			key.receiverIndex = p.BitStream.ReadByte();
			if ((long)key.messageId < (long)m_thisFrameTraffic.Length)
			{
				m_thisFrameTraffic[(uint)key.messageId] += p.BitStream.ByteLength;
			}
			p.Sender = new Endpoint(p.Sender.Id, key.receiverIndex);
			if (!m_handlers.TryGetValue(key, out var value))
			{
				m_handlers.TryGetValue(new HandlerId
				{
					messageId = key.messageId,
					receiverIndex = byte.MaxValue
				}, out value);
			}
			if (value != null)
			{
				int byteLength = p.BitStream.ByteLength;
<<<<<<< HEAD
				MyStatsGraph.Begin(MyEnum<MyMessageId>.GetName(key.messageId), int.MaxValue, "ProcessMessage", 231, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
				value(p);
				MyStatsGraph.End(byteLength, 0f, "", "{0} B", null, 0, "ProcessMessage", 234, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
=======
				MyStatsGraph.Begin(MyEnum<MyMessageId>.GetName(key.messageId), int.MaxValue, "ProcessMessage", 233, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
				value(p);
				MyStatsGraph.End(byteLength, 0f, "", "{0} B", null, 0, "ProcessMessage", 236, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				p.Return();
			}
		}

		public void AddMessageToBuffer(MyPacket packet)
		{
			m_buffer.Add(packet);
		}

		[Conditional("DEBUG")]
		private void TraceMessage(string text, string messageText, ulong userId, long messageSize, MyP2PMessageEnum sendType)
		{
			if (MyMultiplayer.Static != null && MyMultiplayer.Static.SyncLayer.Clients.TryGetClient(userId, out var client))
			{
				_ = client.DisplayName;
			}
			else
			{
				userId.ToString();
			}
			if (sendType != MyP2PMessageEnum.Reliable)
			{
				_ = 3;
			}
		}

		public void Register(MyMessageId messageId, byte receiverIndex, Action<MyPacket> handler)
		{
			HandlerId handlerId = default(HandlerId);
			handlerId.messageId = messageId;
			handlerId.receiverIndex = receiverIndex;
			HandlerId key = handlerId;
			m_handlers.Add(key, handler);
		}

		public void Unregister(MyMessageId messageId, byte receiverIndex)
		{
			HandlerId handlerId = default(HandlerId);
			handlerId.messageId = messageId;
			handlerId.receiverIndex = receiverIndex;
			HandlerId key = handlerId;
			m_handlers.Remove(key);
		}

		private void ClearBuffer()
		{
			if (m_buffer == null)
			{
				return;
			}
			foreach (MyPacket item in m_buffer)
			{
				item.Return();
			}
			m_buffer.Clear();
		}

		public void Clear()
		{
			MyNetworkReader.ClearHandler(2);
			ClearBuffer();
		}
	}
}
