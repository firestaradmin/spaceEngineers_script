using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.P2P;
using VRage.Library.Debugging;
using VRage.Library.Net;
using VRage.Utils;

namespace VRage.EOS
{
	internal class MyP2PQoSAdapter : IDisposable
	{
		private enum ControlMessageType : byte
		{
			None,
			NotifyDisconnect
		}

		private delegate void ControlHandler(ControlMessageType type, TransportAdapter sender, Span<byte> contents);

		private struct ChannelData
		{
			private enum MessageQueue
			{
				Reliable,
				Unreliable
			}

			public bool Reserved;

			public Queue<TransportAdapter> Adapters;

			public Queue<(byte[] Data, ProductUserId Peer)> UnreliableQueue;

			private MessageQueue m_nextRead;

			public bool HasAdapters
			{
				get
				{
					if (Adapters == null)
					{
						return false;
					}
					while (Adapters.Count > 0)
					{
						if (Adapters.Peek().Disposed)
						{
							Adapters.Dequeue();
							continue;
						}
						return true;
					}
					return false;
				}
			}

			public bool HasUnreliableMessages
			{
				get
				{
					Queue<(byte[] Data, ProductUserId Peer)> unreliableQueue = UnreliableQueue;
					if (unreliableQueue == null)
					{
						return false;
					}
					return unreliableQueue.Count > 0;
				}
			}

			public void EnqueueAdapter(TransportAdapter adapter)
			{
				if (Adapters == null)
				{
					Adapters = new Queue<TransportAdapter>();
				}
				Adapters.Enqueue(adapter);
			}

			public bool EnqueueUnreliablePacket(byte[] data, ProductUserId peer)
			{
				if (UnreliableQueue == null)
				{
					UnreliableQueue = new Queue<(byte[], ProductUserId)>();
				}
				if (UnreliableQueue.Count == MaxUnprocessedQueue)
				{
					return false;
				}
				UnreliableQueue.Enqueue((data, peer));
				return true;
			}

			public bool PeekMessage(out uint size)
			{
				bool hasAdapters = HasAdapters;
				bool hasUnreliableMessages = HasUnreliableMessages;
				if (!hasAdapters && !hasUnreliableMessages)
				{
					size = 0u;
					return false;
				}
				if (hasAdapters != hasUnreliableMessages)
				{
					m_nextRead = ((!hasAdapters) ? MessageQueue.Unreliable : MessageQueue.Reliable);
				}
				if (m_nextRead == MessageQueue.Reliable)
				{
					Adapters.Peek().TryPeekMessage(out size, out var _);
				}
				else
				{
					size = (uint)UnreliableQueue.Peek().Data.Length;
				}
				return true;
			}

			public bool TryReadMessage(Span<byte> destination, out uint size, out ProductUserId peer, out TransportAdapter adapter)
			{
				bool hasAdapters = HasAdapters;
				bool hasUnreliableMessages = HasUnreliableMessages;
				if (!hasAdapters && !hasUnreliableMessages)
				{
					size = 0u;
					peer = null;
					adapter = null;
					return false;
				}
				if (hasAdapters != hasUnreliableMessages)
				{
					m_nextRead = ((!hasAdapters) ? MessageQueue.Unreliable : MessageQueue.Reliable);
				}
				bool result = true;
				if (m_nextRead == MessageQueue.Reliable)
				{
					adapter = Adapters.Dequeue();
					if (adapter.TryReceiveMessage(ref destination, out var _))
					{
						peer = adapter.RemoteUser;
						size = (uint)destination.Length;
					}
					else
					{
						peer = null;
						size = 0u;
						result = false;
					}
				}
				else
				{
					(byte[], ProductUserId) tuple = UnreliableQueue.Dequeue();
					size = (uint)Math.Min(tuple.Item1.Length, destination.Length);
					peer = tuple.Item2;
					MemoryExtensions.AsSpan(tuple.Item1, 0, (int)size).CopyTo(destination);
					adapter = null;
				}
				m_nextRead = ((m_nextRead == MessageQueue.Reliable) ? MessageQueue.Unreliable : MessageQueue.Reliable);
				return result;
			}

			public void Clear()
			{
				Adapters?.Clear();
				UnreliableQueue?.Clear();
			}
		}

		private class MessageCounter
		{
			private uint m_receivedId;

			private uint m_sentId;

			private List<(DateTime Time, uint Id)> m_sentHistory = new List<(DateTime, uint)>();

			private List<(DateTime Time, uint Id)> m_receivedHistory = new List<(DateTime, uint)>();

			public void LogReceived()
			{
				m_receivedHistory.Add((DateTime.Now, m_receivedId++));
			}

			public void LogSent()
			{
				m_sentHistory.Add((DateTime.Now, m_sentId++));
			}

			public void Write()
			{
				string name = Assembly.GetEntryAssembly().GetName().Name;
				string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), name + "_received.csv");
				string path2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), name + "_sent.csv");
				using (StreamWriter streamWriter = new StreamWriter(path))
				{
					foreach (var item in m_receivedHistory)
					{
						var (dateTime, _) = item;
						streamWriter.WriteLine($"{dateTime.TimeOfDay.TotalMilliseconds}, {item.Id}");
					}
				}
				using (StreamWriter streamWriter2 = new StreamWriter(path2))
				{
					foreach (var item2 in m_sentHistory)
					{
						var (dateTime, _) = item2;
						streamWriter2.WriteLine($"{dateTime.TimeOfDay.TotalMilliseconds}, {item2.Id}");
					}
				}
			}
		}

		private class ReliableEOSQosProvider : IQoSProvider, IDisposable
		{
			private struct MessageHeader
			{
				public uint Id;

				public byte Channel;
			}

			private Queue<(byte[] Data, byte Channel)> m_receivedMessages = new Queue<(byte[], byte)>();

			private readonly byte[] m_messageBuffer = new byte[1170];

			private uint m_lastSentMessageId;

			private uint m_lastReceivedMessageId;

			private readonly TransportAdapter m_adapterTransport;

			private readonly MyP2PQoSAdapter m_adapter;

			public bool MessagesAvailable => m_receivedMessages.Count > 0;

			public bool HasPendingDeliveries => false;

			public int MaxNonBlockingMessageSize => 1165;

			public ReliableEOSQosProvider(TransportAdapter adapterTransport, MyP2PQoSAdapter adapter)
			{
				m_adapterTransport = adapterTransport;
				m_adapter = adapter;
			}

			public void Dispose()
			{
			}

			public MessageTransferResult SendMessage(Span<byte> message, byte channel, SendMessageFlags flags = (SendMessageFlags)0)
			{
				if (message.Length + 5 > 1170)
				{
					return MessageTransferResult.OversizedMessage;
				}
				ref MessageHeader reference = ref Unsafe.As<byte, MessageHeader>(ref m_messageBuffer[0]);
				reference.Id = ++m_lastSentMessageId;
				reference.Channel = channel;
				message.CopyTo(MemoryExtensions.AsSpan(m_messageBuffer, 5));
				Span<byte> data = MemoryExtensions.AsSpan(m_messageBuffer, 0, message.Length + 5);
				m_adapter.Send(data, -1, m_adapterTransport, reliable: true);
				return MessageTransferResult.QueuedSuccessfully;
			}

			public bool TryReceiveMessage(ref Span<byte> message, out byte channel)
			{
				if (m_receivedMessages.Count > 0)
				{
					(byte[], byte) tuple = m_receivedMessages.Dequeue();
					int length = Math.Min(message.Length, tuple.Item1.Length);
					MemoryExtensions.AsSpan(tuple.Item1, 0, length).CopyTo(message);
					message = message.Slice(0, length);
					channel = tuple.Item2;
					return true;
				}
				channel = 0;
				return false;
			}

			public bool TryPeekMessage(out int size, out byte channel)
			{
				if (m_receivedMessages.Count > 0)
				{
					size = m_receivedMessages.Peek().Data.Length;
					channel = m_receivedMessages.Peek().Channel;
					return true;
				}
				size = 0;
				channel = 0;
				return false;
			}

			public void ProcessWriteQueues()
			{
			}

			public void ProcessReadQueues()
			{
				Span<byte> frame = MemoryExtensions.AsSpan(m_messageBuffer);
				if (!m_adapterTransport.TryGetFrame(ref frame))
				{
					return;
				}
				ref MessageHeader reference = ref Unsafe.As<byte, MessageHeader>(ref m_messageBuffer[0]);
				if (reference.Id != m_lastReceivedMessageId)
				{
					if (reference.Id != m_lastReceivedMessageId + 1)
					{
						throw new InvalidOperationException("Next message in the reliable transport should always be the next consecutive message id.");
					}
					m_lastReceivedMessageId = reference.Id;
					m_receivedMessages.Enqueue((frame.Slice(5).ToArray(), reference.Channel));
				}
			}
		}

		private struct ExtraStats : MyTimedStatWindow.IStatArithmetic<ExtraStats>
		{
			public int DiscardedFrames;

			public TimeSpan ProcessingTime;

			public TimeSpan SendTime;

			public int SendCount;

			public int SentBytes;

			public int ReceivedBytes;

			public double AverageSendTime
			{
				get
				{
					if (SendCount <= 0)
					{
						return 0.0;
					}
					return SendTime.TotalMilliseconds / (double)SendCount;
				}
			}

			void MyTimedStatWindow.IStatArithmetic<ExtraStats>.Add(in ExtraStats lhs, in ExtraStats rhs, out ExtraStats result)
			{
				result.DiscardedFrames = lhs.DiscardedFrames + rhs.DiscardedFrames;
				result.ProcessingTime = lhs.ProcessingTime + rhs.ProcessingTime;
				result.SendTime = lhs.SendTime + rhs.SendTime;
				result.SendCount = lhs.SendCount + rhs.SendCount;
				result.SentBytes = lhs.SentBytes + rhs.SentBytes;
				result.ReceivedBytes = lhs.ReceivedBytes + rhs.ReceivedBytes;
			}

			void MyTimedStatWindow.IStatArithmetic<ExtraStats>.Subtract(in ExtraStats lhs, in ExtraStats rhs, out ExtraStats result)
			{
				result.DiscardedFrames = lhs.DiscardedFrames - rhs.DiscardedFrames;
				result.ProcessingTime = lhs.ProcessingTime - rhs.ProcessingTime;
				result.SendTime = lhs.SendTime - rhs.SendTime;
				result.SendCount = lhs.SendCount - rhs.SendCount;
				result.SentBytes = lhs.SentBytes - rhs.SentBytes;
				result.ReceivedBytes = lhs.ReceivedBytes - rhs.ReceivedBytes;
			}
		}

		private class TransportAdapter : IUnreliableTransportChannel, IDisposable
		{
			public struct FractionedMessageState
			{
				public byte[] Message;

				public int ProcessedLength;

				public byte Channel;

				public int MessageLength => Message.Length;

				public bool IsEmpty => Message == null;

				public bool IsComplete
				{
					get
					{
						byte[] message = Message;
						return ((message != null) ? message.Length : 0) == ProcessedLength;
					}
				}

				public FractionedMessageState(byte[] message, int processedLength, byte channel)
				{
					Message = message;
					ProcessedLength = processedLength;
					Channel = channel;
				}

				public void Clear()
				{
					Message = null;
					ProcessedLength = 0;
				}
			}

			public struct UpDown
			{
				private class UDArithmetic : MyTimedStatWindow.IStatArithmetic<UpDown>
				{
					public void Add(in UpDown lhs, in UpDown rhs, out UpDown result)
					{
						result.Down = lhs.Down + rhs.Down;
						result.Up = lhs.Up + rhs.Up;
					}

					public void Subtract(in UpDown lhs, in UpDown rhs, out UpDown result)
					{
						result.Down = lhs.Down - rhs.Down;
						result.Up = lhs.Up - rhs.Up;
					}

					void MyTimedStatWindow.IStatArithmetic<UpDown>.Add(in UpDown lhs, in UpDown rhs, out UpDown result)
					{
						Add(in lhs, in rhs, out result);
					}

					void MyTimedStatWindow.IStatArithmetic<UpDown>.Subtract(in UpDown lhs, in UpDown rhs, out UpDown result)
					{
						Subtract(in lhs, in rhs, out result);
					}
				}

				public float Up;

				public float Down;

				public static readonly MyTimedStatWindow.IStatArithmetic<UpDown> Arithmetic = new UDArithmetic();

				public UpDown(float up, float down)
				{
					Up = up;
					Down = down;
				}
			}

			public readonly Queue<(byte[] Data, byte Channel)> SendQueue = new Queue<(byte[], byte)>();

			public FractionedMessageState OversizedSend;

			public FractionedMessageState OversizedReceive;

			private const bool UseIntegrityFilter = true;

			private byte[] m_data;

			private int m_size;

			private int m_messageChunkSize;

			private readonly MyP2PQoSAdapter m_adapter;

			public readonly ProductUserId RemoteUser;

			public readonly IQoSProvider Provider;

			public bool Queued;

			public int LastUpdateFrame;

			public string Name;

			private MessageCounter m_counter;

			private readonly MyTimedStatWindow<UpDown> m_upDownInstant;

			private readonly MyTimedStatWindow<UpDown> m_upDownAverage;

			private readonly MyTimedStatWindow<UpDown> m_upDownPeak;

			private static readonly byte[] m_messageBuffer = new byte[MaxMessageChunkSize];

			public int SendQueueBytes { get; private set; }

			public bool Disconnected { get; private set; }

			public bool Disposed { get; private set; }

			public int MinimumMTU => 1170;

			public bool CanSend
			{
				get
				{
					if (m_adapter.CanSendData)
					{
						return m_upDownInstant.Total.Up <= (float)PeerMaximumSendDataRate;
					}
					return false;
				}
			}

			public TransportAdapter(MyP2PQoSAdapter adapter, ProductUserId remoteUser)
			{
				TransportAdapter transportAdapter = this;
				m_adapter = adapter;
				RemoteUser = remoteUser;
				Name = remoteUser.GetIdString();
				m_upDownInstant = new MyTimedStatWindow<UpDown>(TimeSpan.FromSeconds(1.0), UpDown.Arithmetic);
				m_upDownPeak = new MyTimedStatWindow<UpDown>(TimeSpan.FromSeconds(m_adapter.PeakStatsWindow), UpDown.Arithmetic);
				m_upDownAverage = new MyTimedStatWindow<UpDown>(TimeSpan.FromSeconds(m_adapter.StatsWindow), UpDown.Arithmetic);
				ConnectInterface connect = adapter.m_networking.Platform.GetConnectInterface();
				connect.QueryProductUserIdMappings(new QueryProductUserIdMappingsOptions
				{
					LocalUserId = adapter.LocalId,
					ProductUserIds = new ProductUserId[1] { remoteUser }
				}, null, delegate(QueryProductUserIdMappingsCallbackInfo x)
				{
					if (x.ResultCode == Result.Success && connect.CopyProductUserInfo(new CopyProductUserInfoOptions
					{
						TargetUserId = remoteUser
					}, out var outExternalAccountInfo) == Result.Success)
					{
						transportAdapter.m_adapter.m_networking.Log("PUID [" + transportAdapter.Name + "] identified as " + outExternalAccountInfo.DisplayName + ".");
						transportAdapter.Name = outExternalAccountInfo.DisplayName;
					}
				});
				if (!UseBuiltInReliabilityService)
				{
					IUnreliableTransportChannel unreliableTransportChannel = this;
					FrameIntegrityFilter frameIntegrityFilter = new FrameIntegrityFilter(this);
					frameIntegrityFilter.ValidationFailure += delegate(uint crc, uint computedCrc, FrameIntegrityFilter instance)
					{
						transportAdapter.m_adapter.m_networking.Log($"Received CRC mismatching frame from {transportAdapter.Name}: Expected CRC {computedCrc:X8} but received {crc:X8}.");
					};
					unreliableTransportChannel = frameIntegrityFilter;
					DefaultQoSProvider defaultQoSProvider = new DefaultQoSProvider(unreliableTransportChannel, adapter.m_settings);
					defaultQoSProvider.SetStatisticsTracker(adapter.Statistics);
					Provider = defaultQoSProvider;
				}
				else
				{
					Provider = new ReliableEOSQosProvider(this, adapter);
				}
				m_messageChunkSize = Math.Min(Provider.MaxNonBlockingMessageSize, MaxMessageChunkSize);
			}

			public void UpdateStats()
			{
				m_upDownInstant.Advance();
				m_upDownPeak.Current = m_upDownInstant.Total;
				m_upDownPeak.Advance();
				m_upDownAverage.Advance();
			}

			public void RecordSend(int bytes)
			{
				m_upDownInstant.Current.Up += bytes;
				m_upDownAverage.Current.Up += bytes;
			}

			public void RecordReceive(int bytes)
			{
				m_upDownInstant.Current.Down += bytes;
				m_upDownAverage.Current.Down += bytes;
			}

			public void GetStats(out UpDown peak, out UpDown average)
			{
				peak = default(UpDown);
				foreach (UpDown item in m_upDownPeak)
				{
					if (item.Down > peak.Down)
					{
						peak.Down = item.Down;
					}
					if (item.Up > peak.Up)
					{
						peak.Up = item.Up;
					}
				}
				UpDown total = m_upDownAverage.Total;
				average = new UpDown(total.Up / 60f, total.Down / 60f);
			}

			public bool SendReliable(Span<byte> message, byte channel, bool highPriority)
			{
				if (Disconnected)
				{
					return false;
				}
				if (SendQueueBytes > MaxReliableQueuedBytes && SendQueue.Count > MaxReliableQueuedMessages)
				{
					m_adapter.m_networking.Log($"Dropping client [{Name}] for exceeding the maximum queue limit. Queue: {SendQueue.Count} messages totaling {SendQueueBytes} bytes.");
					SetDisconnected();
					m_adapter.m_networking.EOSPeer2Peer.RaiseConnectionFailed(RemoteUser, "Extended Queue Overflow");
					return false;
				}
				m_counter?.LogSent();
				if (SendQueue.Count > 0 || !OversizedSend.IsEmpty)
				{
					SendQueue.Enqueue((message.ToArray(), channel));
					SendQueueBytes += message.Length;
					return true;
				}
				SendMessageFlags sendMessageFlags = SendMessageFlags.NonBlocking;
				if (highPriority)
				{
					sendMessageFlags |= SendMessageFlags.HighPriority;
				}
				switch (Provider.SendMessage(message, channel, sendMessageFlags))
				{
				case MessageTransferResult.QueueFull:
					SendQueue.Enqueue((message.ToArray(), channel));
					SendQueueBytes += message.Length;
					break;
				case MessageTransferResult.OversizedMessage:
					EnqueueOversized(message, channel);
					break;
				}
				return true;
			}

			private bool EnqueueOversized(Span<byte> message, byte channel)
			{
				if (!OversizedSend.IsEmpty)
				{
					throw new InvalidOperationException("Already handling an oversized message.");
				}
				byte[] array = new byte[message.Length + 5];
				Unsafe.As<byte, int>(ref array[0]) = IPAddress.HostToNetworkOrder(message.Length);
				array[4] = channel;
				message.CopyTo(MemoryExtensions.AsSpan(array, 5));
				OversizedSend = new FractionedMessageState(array, 0, channel);
				return SendOversized();
			}

			public void ProcessReceiveQueue()
			{
				Provider.ProcessReadQueues();
			}

			public void ProcessSendQueue()
			{
				Provider.ProcessWriteQueues();
				while (!OversizedSend.IsEmpty || SendQueue.Count > 0)
				{
					if (OversizedSend.IsEmpty)
					{
						(byte[], byte) tuple = SendQueue.Peek();
						MessageTransferResult messageTransferResult = Provider.SendMessage(tuple.Item1, tuple.Item2, SendMessageFlags.NonBlocking);
						if (messageTransferResult == MessageTransferResult.QueueFull)
						{
							break;
						}
						SendQueue.Dequeue();
						SendQueueBytes -= tuple.Item1.Length;
						if (messageTransferResult == MessageTransferResult.OversizedMessage && !EnqueueOversized(tuple.Item1, tuple.Item2))
						{
							break;
						}
					}
					else if (!SendOversized())
					{
						break;
					}
				}
			}

			private bool SendOversized()
			{
				ref FractionedMessageState oversizedSend = ref OversizedSend;
				int num = Math.Min(oversizedSend.MessageLength - oversizedSend.ProcessedLength, m_messageChunkSize);
				switch (Provider.SendMessage(MemoryExtensions.AsSpan(oversizedSend.Message, oversizedSend.ProcessedLength, num), byte.MaxValue, SendMessageFlags.NonBlocking))
				{
				case MessageTransferResult.OversizedMessage:
					throw new InvalidOperationException("Message chunk size is too large.");
				case MessageTransferResult.QueueFull:
					return false;
				case MessageTransferResult.QueuedSuccessfully:
					oversizedSend.ProcessedLength += num;
					if (oversizedSend.IsComplete)
					{
						oversizedSend.Clear();
					}
					return true;
				default:
					throw new InvalidOperationException("Unexpected result from IQoSProvider.SendMessage.");
				}
			}

			public void FeedPacket(byte[] data, int size)
			{
				if (!Disconnected)
				{
					m_data = data;
					m_size = size;
					Provider.ProcessReadQueues();
				}
			}

			public bool TryPeekMessage(out uint size, out byte channel)
			{
				if (!OversizedReceive.IsEmpty && OversizedReceive.IsComplete)
				{
					channel = OversizedReceive.Channel;
					size = (uint)OversizedReceive.MessageLength;
					return true;
				}
				if (Provider.TryPeekMessage(out var size2, out channel))
				{
					if (channel == byte.MaxValue)
					{
						ProcessOversized();
					}
					if (OversizedReceive.IsEmpty)
					{
						size = (uint)size2;
						return true;
					}
					if (OversizedReceive.IsComplete)
					{
						channel = OversizedReceive.Channel;
						size = (uint)OversizedReceive.MessageLength;
						return true;
					}
				}
				size = 0u;
				channel = 0;
				return false;
			}

			private void ProcessOversized()
			{
				byte channel;
				if (OversizedReceive.IsEmpty)
				{
					Span<byte> message = MemoryExtensions.AsSpan(m_messageBuffer);
					if (Provider.TryReceiveMessage(ref message, out channel))
					{
						int num = IPAddress.NetworkToHostOrder(Unsafe.As<byte, int>(ref message[0]));
						byte channel2 = message[4];
						byte[] array = new byte[num];
						message.Slice(5).CopyTo(array);
						OversizedReceive = new FractionedMessageState(array, message.Length - 5, channel2);
					}
				}
				else
				{
					Span<byte> message2 = MemoryExtensions.AsSpan(OversizedReceive.Message, OversizedReceive.ProcessedLength);
					if (Provider.TryReceiveMessage(ref message2, out channel))
					{
						OversizedReceive.ProcessedLength += message2.Length;
					}
				}
			}

			public bool TryReceiveMessage(ref Span<byte> message, out byte channel)
			{
				try
				{
					if (OversizedReceive.IsEmpty)
					{
						bool num = Provider.TryReceiveMessage(ref message, out channel);
						if (num)
						{
							m_counter?.LogReceived();
						}
						return num;
					}
					int length = Math.Min(OversizedReceive.MessageLength, message.Length);
					MemoryExtensions.AsSpan(OversizedReceive.Message, 0, length).CopyTo(message);
					channel = OversizedReceive.Channel;
					OversizedReceive.Clear();
					m_counter?.LogReceived();
					return true;
				}
				catch (Exception arg)
				{
					m_adapter.m_networking.Log($"Could not receive message from {RemoteUser.GetIdString()}: {arg}");
					m_adapter.m_p2p.CloseConnection(new CloseConnectionOptions
					{
						LocalUserId = m_adapter.LocalId,
						RemoteUserId = RemoteUser,
						SocketId = m_adapter.m_sendOptions.SocketId
					});
					channel = 0;
					return false;
				}
			}

			public void SetDisconnected()
			{
				Disconnected = true;
			}

			public int Send(Span<byte> frame)
			{
				if (Disposed)
				{
					return 0;
				}
				if (frame[8] == 0 && !CanSend)
				{
					return 0;
				}
				Result result = m_adapter.Send(frame, -1, this);
				if (Disconnected)
				{
					return 0;
				}
				if (result != 0)
				{
					return 0;
				}
				return frame.Length;
			}

			public bool PeekFrame(out int frameSize)
			{
				if (Disposed)
				{
					frameSize = 0;
					return false;
				}
				if (m_data == null)
				{
					frameSize = 0;
					return false;
				}
				frameSize = m_data.Length;
				return true;
			}

			public bool TryGetFrame(ref Span<byte> frame)
			{
				if (Disposed)
				{
					return false;
				}
				if (m_data == null)
				{
					return false;
				}
				MemoryExtensions.AsSpan(m_data, 0, m_size).CopyTo(frame);
				frame = frame.Slice(0, m_size);
				m_data = null;
				return true;
			}

			public void QueryMTU(Action<int> result)
			{
				result(1170);
			}

			public void Dispose()
			{
				Disposed = true;
				Provider.Dispose();
				m_counter?.Write();
			}
		}

		private Dictionary<ControlMessageType, ControlHandler> m_controlMessageHandlers = new Dictionary<ControlMessageType, ControlHandler>();

		public static bool UseBuiltInReliabilityService = false;

		public const byte OversizedChannel = byte.MaxValue;

		public const byte ControlChannel = 254;

		private const int DefaultOversizedMessageChunkSize = 4096;

		private static int MaxMessageChunkSize = Math.Max(1170, 4096);

		public static int MaxUnprocessedQueue = 128;

		public static int MaxReliableQueuedBytes = 5242880;

		public static int MaxReliableQueuedMessages = 500;

		public static int GlobalMaximumSendDataRate;

		public static int PeerMaximumSendDataRate;

		private readonly P2PInterface m_p2p;

		private readonly MyEOSNetworking m_networking;

		public ProductUserId m_localId;

		private readonly Dictionary<ProductUserId, TransportAdapter> m_adapters = new Dictionary<ProductUserId, TransportAdapter>();

		private readonly ChannelData[] m_channels = new ChannelData[256];

		private List<byte> m_unReservedChannels = (from x in Enumerable.Range(0, 256)
			select (byte)x).ToList();

		private int m_frameCtr;

		private readonly DefaultQoSProvider.InitSettings m_settings;

		private readonly GetNextReceivedPacketSizeOptions m_getNextReceivedPacketSizeOptions = new GetNextReceivedPacketSizeOptions();

		private readonly SendPacketOptions m_sendOptions = new SendPacketOptions
		{
			AllowDelayedDelivery = false
		};

		private readonly ReceivePacketOptions m_receivePacketOptions = new ReceivePacketOptions
		{
			MaxDataSizeBytes = 1170u
		};

		private readonly object m_lock;

		private readonly MyTimedStatWindow<int> m_sentData = new MyTimedStatWindow<int>(TimeSpan.FromSeconds(1.0), MyTimedStatWindow.IntArithmetic);

		private int m_lastSentChannel;

		public float StatsWindow = 60f;

		public float PeakStatsWindow = 60f;

		public DefaultQoSProvider.Statistics Statistics;

		private readonly MyTimedStatWindow<ExtraStats> m_extraStats;

		private readonly StringBuilder m_statSb = new StringBuilder();

		private readonly List<string> m_statData = new List<string>();

		private readonly Stopwatch m_timer = new Stopwatch();

		private readonly Stopwatch m_sendTimer = new Stopwatch();

		private readonly Stopwatch m_statUpdateTimer = new Stopwatch();

		private readonly string m_watchName = Assembly.GetEntryAssembly().GetName().Name + " Transport Stats";

		public ProductUserId LocalId
		{
			get
			{
				return m_localId;
			}
			set
			{
				m_localId = value;
				m_getNextReceivedPacketSizeOptions.LocalUserId = m_localId;
				m_sendOptions.LocalUserId = m_localId;
				m_receivePacketOptions.LocalUserId = m_localId;
			}
		}

		private bool CanSendData => m_sentData.Total <= GlobalMaximumSendDataRate;

		private void SetupControlHandlers()
		{
			m_controlMessageHandlers = new Dictionary<ControlMessageType, ControlHandler> { 
			{
				ControlMessageType.NotifyDisconnect,
				HandleNotifyDisconnect
			} };
			ReserveChannel(254);
			MyVRage.Platform.Render.OnSuspending += RenderOnSuspending;
			MyVRage.Platform.CrashReporting.ExitingProcessOnCrash += HandleCrash;
		}

		private void CleanUpControlHandlers()
		{
			MyVRage.Platform.Render.OnSuspending -= RenderOnSuspending;
			MyVRage.Platform.CrashReporting.ExitingProcessOnCrash -= HandleCrash;
		}

		private void DispatchControlMessage(TransportAdapter source, Span<byte> message)
		{
			ControlMessageType controlMessageType = (ControlMessageType)message[0];
			if (m_controlMessageHandlers.TryGetValue(controlMessageType, out var value))
			{
				value(controlMessageType, source, message);
			}
		}

		private byte[] PrepareControlMessage(ControlMessageType type)
		{
			byte[] array = new byte[1170];
			array[0] = (byte)type;
			return array;
		}

		private void SendControlMessage(Span<byte> messageContents, TransportAdapter target)
		{
			Send(messageContents, 254, target);
		}

		private void SendNotifyDisconnectMessage(TransportAdapter target, string reason)
		{
			byte[] array = PrepareControlMessage(ControlMessageType.NotifyDisconnect);
			Encoding.UTF8.GetEncoder().Convert(reason.ToCharArray(), 0, reason.Length, array, 1, array.Length - 1, flush: true, out var _, out var bytesUsed, out var completed);
			if (!completed)
			{
				m_networking.Error("Disconnect reason \"" + reason + "\" is too long and was truncated before transmitting to remote.");
			}
			SendControlMessage(MemoryExtensions.AsSpan(array, 0, bytesUsed + 1), target);
		}

		private unsafe void HandleNotifyDisconnect(ControlMessageType type, TransportAdapter sender, Span<byte> contents)
		{
			char[] array = new char[contents.Length];
			fixed (byte* ptr = contents)
			{
				fixed (char* ptr2 = array)
				{
					Encoding.UTF8.GetDecoder().Convert(ptr + 1, contents.Length - 1, ptr2, contents.Length, flush: true, out var _, out var charsUsed, out var completed);
					if (completed)
					{
						string text = new string(ptr2, 0, charsUsed);
						m_networking.Log("Client " + sender.Name + " is disconnecting: " + text);
					}
				}
			}
		}

		private void RenderOnSuspending()
		{
			m_networking.InvokeOnNetworkThread(RenderOnSuspendingInternal);
			MyVRage.Platform.Render.RequestSuspendWait();
		}

		private void RenderOnSuspendingInternal()
		{
			foreach (TransportAdapter value in m_adapters.Values)
			{
				SendNotifyDisconnectMessage(value, "Suspending");
			}
		}

		private void HandleCrash(Exception e)
		{
			try
			{
				if (Thread.CurrentThread != m_networking.NetworkThread)
				{
					m_networking.InvokeOnNetworkThread(HandleCrashInternal);
					Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
					return;
				}
				HandleCrashInternal();
				Stopwatch stopwatch = new Stopwatch();
				while (stopwatch.Elapsed > TimeSpan.FromMilliseconds(100.0))
				{
					m_networking.Platform.Tick();
					Thread.Sleep(10);
				}
			}
			catch (Exception ex)
			{
				CleanUpControlHandlers();
				MyLog.Default.WriteLine("While reporting crash to peers:");
				MyLog.Default.WriteLine(ex);
			}
		}

		private void HandleCrashInternal()
		{
			foreach (TransportAdapter value in m_adapters.Values)
			{
				SendNotifyDisconnectMessage(value, "Crashing");
			}
		}

		public MyP2PQoSAdapter(P2PInterface p2P, ProductUserId localId, SocketId socket, MyEOSNetworking networking)
		{
			m_p2p = p2P;
			m_localId = localId;
			m_networking = networking;
			m_sendOptions.SocketId = socket;
			DefaultQoSProvider.InitSettings @default = DefaultQoSProvider.InitSettings.Default;
			@default.DisconnectTimeout = TimeSpan.MinValue;
			@default.MinimumWindowSize = 6144;
			@default.MaximumWindowSize = 262144;
			@default.MinimumQueueSize = (@default.MaximumQueueSize = 262144);
			m_settings = @default;
			m_lock = m_sendOptions;
			GlobalMaximumSendDataRate = GetParameter("globalMaxUpload", 600) * 1024;
			PeerMaximumSendDataRate = GetParameter("peerMaxUpload", 600) * 1024;
			StatsWindow = GetParameter("statWindow", 60f);
			PeakStatsWindow = GetParameter("peakStatWindow", 60f);
			Statistics = DefaultQoSProvider.Statistics.CreateMovingWindow(TimeSpan.FromSeconds(StatsWindow));
			m_extraStats = new MyTimedStatWindow<ExtraStats>(TimeSpan.FromSeconds(StatsWindow), default(ExtraStats));
			SetupControlHandlers();
		}

		private T GetParameter<T>(string name, T @default)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
			string text = name + ":";
			string[] parameters = m_networking.Parameters;
			foreach (string text2 in parameters)
			{
				if (text2.StartsWith(text))
				{
					string text3 = text2.Substring(text.Length).Trim();
					object obj = converter.ConvertFromString(text3);
					if (obj != null)
					{
						m_networking.Log($"Parameter[\"{name}\"] = {obj}");
						return (T)obj;
					}
					m_networking.Log("Parameter[\"" + name + "\"] parsing failed.");
					return @default;
				}
			}
			return @default;
		}

		public void AddPeer(ProductUserId peer)
		{
			lock (m_lock)
			{
				if (m_adapters.TryGetValue(peer, out var value))
				{
					value.Dispose();
				}
				m_adapters[peer] = new TransportAdapter(this, peer);
			}
		}

		public void NotifyDisconnected(ProductUserId peer)
		{
			if (m_adapters.TryGetValue(peer, out var value))
			{
				value.SetDisconnected();
			}
		}

		public void RemovePeer(ProductUserId peer)
		{
			lock (m_lock)
			{
				if (m_adapters.TryGetValue(peer, out var value))
				{
					m_networking.Log("Removing peer " + value.Name + ". Stats: " + FormatClientStats(value));
					value.Dispose();
					m_adapters.Remove(peer);
				}
			}
		}

		public void ProcessQueues()
		{
			m_timer.Restart();
			m_frameCtr++;
			m_sentData.Advance();
			ProcessFrames();
			UpdateStats();
			m_extraStats.Current.ProcessingTime += m_timer.Elapsed;
		}

		private void ProcessFrames()
		{
			ProductUserId outPeerId;
			SocketId outSocketId;
			byte outChannel;
			byte[] outData;
			while (m_p2p.ReceivePacket(m_receivePacketOptions, out outPeerId, out outSocketId, out outChannel, out outData) == Result.Success)
			{
				m_extraStats.Current.ReceivedBytes += outData.Length;
				if (!m_adapters.TryGetValue(outPeerId, out var value))
				{
					continue;
				}
				value.RecordReceive(outData.Length);
				if (outChannel == 254)
				{
					DispatchControlMessage(value, MemoryExtensions.AsSpan(outData));
				}
				if (m_channels[outChannel].Reserved)
				{
					if (!m_channels[outChannel].EnqueueUnreliablePacket(outData, outPeerId))
					{
						m_extraStats.Current.DiscardedFrames++;
					}
					continue;
				}
				value.LastUpdateFrame = m_frameCtr;
				value.FeedPacket(outData, outData.Length);
				if (!value.Queued)
				{
					UpdateChannelQueue(value);
				}
			}
			if (m_adapters.Count == 0)
			{
				return;
			}
			foreach (TransportAdapter value2 in m_adapters.Values)
			{
				if (m_frameCtr != value2.LastUpdateFrame)
				{
					value2.ProcessReceiveQueue();
				}
			}
			foreach (TransportAdapter value3 in m_adapters.Values)
			{
				value3.ProcessSendQueue();
			}
		}

		private void UpdateChannelQueue(TransportAdapter adapter)
		{
			if (adapter.TryPeekMessage(out var _, out var channel))
			{
				m_channels[channel].EnqueueAdapter(adapter);
				adapter.Queued = true;
			}
		}

		public bool Send(Span<byte> message, byte channel, ProductUserId peer, bool highPriority, bool reliable)
		{
			if (!m_channels[channel].Reserved)
			{
				throw new InvalidOperationException($"Channel {channel} is not reserved for traffic.");
			}
			m_timer.Restart();
			try
			{
				if (m_adapters.TryGetValue(peer, out var value) && !value.Disconnected)
				{
					try
					{
						if (reliable)
						{
							return value.SendReliable(message, channel, highPriority);
						}
						if (value.CanSend)
						{
							return Send(message, channel, value) == Result.Success;
						}
						return true;
					}
					catch
					{
						return false;
					}
				}
				return false;
			}
			finally
			{
				m_extraStats.Current.ProcessingTime += m_timer.Elapsed;
				CheckAndUpdateStats();
			}
		}

		public bool HasMessage(int channel, out uint size)
		{
			return m_channels[channel].PeekMessage(out size);
		}

		public bool TryReadMessage(int channel, Span<byte> destination, out uint size, out ProductUserId peer)
		{
			if (m_channels[channel].TryReadMessage(destination, out size, out peer, out var adapter))
			{
				if (adapter != null)
				{
					adapter.Queued = false;
					UpdateChannelQueue(adapter);
				}
				return true;
			}
			size = 0u;
			peer = null;
			return false;
		}

		public void ReserveChannel(byte channel)
		{
			m_unReservedChannels.Remove(channel);
			m_channels[channel].Reserved = true;
		}

		private Result Send(Span<byte> data, int channel, TransportAdapter adapter, bool reliable = false)
		{
			byte channel2;
			if (channel == -1)
			{
				m_lastSentChannel = ((m_lastSentChannel != m_unReservedChannels.Count - 1) ? (m_lastSentChannel + 1) : 0);
				channel2 = m_unReservedChannels[m_lastSentChannel];
			}
			else
			{
				channel2 = (byte)channel;
			}
			m_sendOptions.Data = data.ToArray();
			m_sendOptions.Channel = channel2;
			m_sendOptions.RemoteUserId = adapter.RemoteUser;
			m_sendOptions.Reliability = (reliable ? PacketReliability.ReliableOrdered : PacketReliability.UnreliableUnordered);
			m_sendTimer.Restart();
			Result result = m_p2p.SendPacket(m_sendOptions);
			m_extraStats.Current.SendTime += m_sendTimer.Elapsed;
			m_extraStats.Current.SendCount++;
			m_extraStats.Current.SentBytes += data.Length;
			adapter.RecordSend(data.Length);
			m_sentData.Current += data.Length;
			return result;
		}

		private void CheckAndUpdateStats()
		{
			if (m_statUpdateTimer.ElapsedMilliseconds > 500)
			{
				UpdateStats();
			}
		}

		private void UpdateStats()
		{
			lock (m_lock)
			{
				Statistics.Tick();
				m_extraStats.Advance();
				foreach (TransportAdapter value in m_adapters.Values)
				{
					value.UpdateStats();
				}
				m_statUpdateTimer.Restart();
			}
		}

		private void FormatStatList()
		{
			m_statData.Clear();
			m_statData.AddRange(from x in Statistics.EnumerateFields()
				select FormatStat(x.Name, x.Stat));
			ExtraStats total = m_extraStats.Total;
			m_statData.Add($"Processing Time: {total.ProcessingTime.TotalMilliseconds / (double)StatsWindow:#,#0.0}ms");
			m_statData.Add($"Packets Dropped While Blocking: {total.DiscardedFrames:#,#0.0}");
			m_statData.Add($"Total Sent: {(float)total.SentBytes / StatsWindow:#,#0.0}");
			m_statData.Add($"Total Received: {(float)total.ReceivedBytes / StatsWindow:#,#0.0}");
			m_statData.Add($"Avg Send Time: {total.AverageSendTime / (double)StatsWindow:#,#0.0}");
			m_statData.Add($"Avg Send Count: {(float)total.SendCount / StatsWindow:#,#0.0}");
			if (UseBuiltInReliabilityService)
			{
				return;
			}
			foreach (TransportAdapter value in m_adapters.Values)
			{
				m_statData.Add(value.Name + ": " + FormatClientStats(value));
			}
		}

		private string FormatClientStats(TransportAdapter adapter)
		{
			if (UseBuiltInReliabilityService)
			{
				return "";
			}
			DefaultQoSProvider defaultQoSProvider = (DefaultQoSProvider)adapter.Provider;
			m_statSb.Clear();
			m_statSb.AppendFormat("RTT{0:#,#0.0}~{1:#,#0.0}ms", defaultQoSProvider.EstimateRTT.TotalMilliseconds, defaultQoSProvider.EstimateRTTVariation.TotalMilliseconds);
			m_statSb.AppendFormat(" TTD{0:#,#0.0}", defaultQoSProvider.AverageAckTime.TotalMilliseconds);
			m_statSb.AppendFormat(" EQ{0:#,#0} Q{1} W{2}", adapter.SendQueueBytes, defaultQoSProvider.QueueSize, defaultQoSProvider.SlidingWindowSize);
			m_statSb.AppendFormat(" {0}% Loss", defaultQoSProvider.FrameLoss * 100f);
			adapter.GetStats(out var peak, out var average);
			AppendDataRate("U", average.Up);
			AppendDataRate("D", average.Down);
			AppendDataRate("UP", peak.Up);
			AppendDataRate("DP", peak.Down);
			return m_statSb.ToString();
			void AppendDataRate(string name, double value)
			{
				m_statSb.Append(" ");
				m_statSb.Append(name);
				FormatDataRate(m_statSb, value);
			}
		}

		private string FormatStat(string name, object stat)
		{
			m_statSb.Clear();
			int num = name.LastIndexOf('\t') + 1;
			m_statSb.Append(' ', num * 4);
			m_statSb.Append(name, num, name.Length - num).Append(": ");
			if (stat == null)
			{
				goto IL_00e5;
			}
			object obj;
			if ((obj = stat) is DefaultQoSProvider.Statistics.Traffic)
			{
				DefaultQoSProvider.Statistics.Traffic traffic = (DefaultQoSProvider.Statistics.Traffic)obj;
				DefaultQoSProvider.Statistics.TrafficF trafficF = traffic / StatsWindow;
				m_statSb.AppendFormat("{0:#,#0.0}, ", trafficF.Frames);
				FormatDataRate(m_statSb, trafficF.Bytes);
			}
			else
			{
				if (!((obj = stat) is DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement))
				{
					goto IL_00e5;
				}
				DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement acknowledgement = (DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement)obj;
				DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement acknowledgement2 = acknowledgement;
				m_statSb.AppendFormat("{0:#,#0.0}ms", acknowledgement2.Average.TotalMilliseconds);
			}
			goto IL_00f2;
			IL_00e5:
			m_statSb.Append(stat);
			goto IL_00f2;
			IL_00f2:
			return m_statSb.ToString();
		}

		private void FormatDataRate(StringBuilder sb, double rate)
		{
			MyValueFormatter.AppendFormattedValueInBestUnit((float)rate, new string[3] { "B/s", "KB/s", "MB/s" }, new float[3] { 1f, 1024f, 1048576f }, 1, sb);
		}

		public string GetStatsReadable()
		{
			lock (m_lock)
			{
				FormatStatList();
				return string.Join("\n", m_statData);
			}
		}

		public IEnumerable<(string Name, double Value)> GetStats()
		{
			if (!UseBuiltInReliabilityService)
			{
				DefaultQoSProvider.Statistics.SentTraffic sent;
				DefaultQoSProvider.Statistics.ReceivedTraffic received;
				DefaultQoSProvider.Statistics.DroppedTraffic dropped;
				lock (m_lock)
				{
					sent = Statistics.Sent;
					received = Statistics.Received;
					dropped = Statistics.Dropped;
				}
				yield return ("Sent", (float)sent.Total.Bytes / StatsWindow);
				yield return ("Sent Message", (float)sent.Message.Bytes / StatsWindow);
				yield return ("Sent Message Re-Sent", (float)sent.MessageResend.Bytes / StatsWindow);
				yield return ("Sent Message Acknowledged", sent.Acknowledged.Average.TotalMilliseconds);
				yield return ("Sent Control", (float)sent.TotalControl.Bytes / StatsWindow);
				yield return ("Sent Control Ack", (float)sent.Ack.Bytes / StatsWindow);
				yield return ("Sent Control AckControl", (float)sent.AckControl.Bytes / StatsWindow);
				yield return ("Sent Control Ping", (float)sent.Ping.Bytes / StatsWindow);
				yield return ("Received", (float)received.Total.Bytes / StatsWindow);
				yield return ("Received Message", (float)received.Message.Bytes / StatsWindow);
				yield return ("Received Control", (float)received.TotalControl.Bytes / StatsWindow);
				yield return ("Received Control Ack", (float)received.Ack.Bytes / StatsWindow);
				yield return ("Received Control AckControl", (float)received.AckControl.Bytes / StatsWindow);
				yield return ("Received Control Ping", (float)received.Ping.Bytes / StatsWindow);
				yield return ("Dropped", (float)dropped.Total.Bytes / StatsWindow);
				yield return ("Dropped Message", (float)dropped.TotalMessage.Bytes / StatsWindow);
				yield return ("Dropped Message Queue Full", (float)dropped.MessageQueueFull.Bytes / StatsWindow);
				yield return ("Dropped Message Window Full", (float)dropped.MessageWindowFull.Bytes / StatsWindow);
				yield return ("Dropped Message Out of Order", (float)dropped.MessageOutOfOrder.Bytes / StatsWindow);
				yield return ("Dropped Control", (float)dropped.Control.Bytes / StatsWindow);
			}
			ExtraStats extra;
			lock (m_lock)
			{
				extra = m_extraStats.Total;
			}
			yield return ("Processing Time", extra.ProcessingTime.TotalMilliseconds / (double)StatsWindow);
			yield return ("SendPacket Count", (float)extra.SendCount / StatsWindow);
			yield return ("Average SendPacket Time", extra.AverageSendTime);
			yield return ("Total Sent", (float)extra.SentBytes / StatsWindow);
			yield return ("Total Received", (float)extra.ReceivedBytes / StatsWindow);
		}

		public IEnumerable<(string Name, IEnumerable<(string Stat, double Value)> Stats)> GetClientStats()
		{
			lock (m_lock)
			{
				foreach (TransportAdapter value in m_adapters.Values)
				{
					DefaultQoSProvider defaultQoSProvider;
					(string, double)[] array;
					if ((defaultQoSProvider = value.Provider as DefaultQoSProvider) != null)
					{
						array = new(string, double)[11]
						{
							default((string, double)),
							("RTT (ms)", defaultQoSProvider.EstimateRTT.TotalMilliseconds),
							("RTT Var (ms)", defaultQoSProvider.EstimateRTTVariation.TotalMilliseconds),
							("Time To Deliver", defaultQoSProvider.AverageAckTime.TotalMilliseconds),
							("Queue Size", defaultQoSProvider.QueueSize),
							("Windows Size", defaultQoSProvider.SlidingWindowSize),
							("Frame Loss (%)", defaultQoSProvider.FrameLoss),
							default((string, double)),
							default((string, double)),
							default((string, double)),
							default((string, double))
						};
						value.GetStats(out var peak, out var average);
						array[7] = ("Up Average (B/s)", average.Up);
						array[8] = ("Down Average (B/s)", average.Down);
						array[9] = ("Up Peak (B/s)", peak.Up);
						array[10] = ("Down Peak (B/s)", peak.Down);
					}
					else
					{
						array = new(string, double)[1];
					}
					array[0] = ("Queued Bytes", value.SendQueueBytes);
					yield return (value.Name, array);
				}
			}
		}

		public void ClearAll()
		{
			foreach (TransportAdapter value in m_adapters.Values)
			{
				value.Dispose();
			}
			m_adapters.Clear();
			ChannelData[] channels = m_channels;
			foreach (ChannelData channelData in channels)
			{
				channelData.Clear();
			}
		}

		public void Dispose()
		{
			ClearAll();
			CleanUpControlHandlers();
		}
	}
}
