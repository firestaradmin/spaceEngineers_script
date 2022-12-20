using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Security;
<<<<<<< HEAD
=======
using System.Security.Cryptography;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.Multiplayer;
using VRage;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Networking
{
	/// <summary>
	/// Receives messages from Steam immediatelly on separate thread
	/// Precise receive time is required for proper interpolation
	/// </summary>
	internal sealed class MyReceiveQueue : IDisposable
	{
		private class MyPacketData : MyPacket
		{
			public byte[] Data;

			public override void Return()
			{
				Data = null;
				BitStream.Dispose();
				BitStream = null;
				ByteStream = null;
			}
		}

		private class MyPacketDataPooled : MyPacketData
		{
			private bool m_returned;

			public void Init()
			{
				m_returned = false;
			}

			public override void Return()
			{
				m_returned = true;
				m_messagePool.Return(this);
			}
		}

		private class MessageAllocator : IMyElementAllocator<MyPacketDataPooled>
		{
			public bool ExplicitlyDisposeAllElements => true;

			public MyPacketDataPooled Allocate(int size)
			{
				return new MyPacketDataPooled
				{
					Data = new byte[size],
					BitStream = new BitStream(0),
					ByteStream = new ByteStream()
				};
			}

			public void Dispose(MyPacketDataPooled message)
			{
				message.Data = null;
				message.BitStream.Dispose();
				message.BitStream = null;
				message.ByteStream = null;
			}

			public void Init(MyPacketDataPooled message)
			{
				message.Init();
			}

			public int GetBucketId(MyPacketDataPooled message)
			{
				return message.Data.Length;
			}

			public int GetBytes(MyPacketDataPooled message)
			{
				return message.Data.Length;
			}
		}

		public enum ReceiveStatus
		{
			None,
			TamperredPacket,
			Success
		}

		private static readonly MyConcurrentBucketPool<MyPacketDataPooled> m_messagePool = new MyConcurrentBucketPool<MyPacketDataPooled, MessageAllocator>("MessagePool");

		private readonly ConcurrentQueue<MyPacket> m_receiveQueue;

		private readonly int m_channel;

		private readonly Action<ulong> m_disconnectPeerOnError;

		private static readonly Crc32 m_crc32 = new Crc32();

		private byte[] m_largePacket;

		private int m_largePacketCounter;

		private const int PACKET_HEADER_SIZE = 8;

		public MyReceiveQueue(int channel, Action<ulong> disconnectPeerOnError)
		{
			m_channel = channel;
			m_receiveQueue = new ConcurrentQueue<MyPacket>();
			m_disconnectPeerOnError = disconnectPeerOnError;
		}

		private MyPacketDataPooled GetMessage(int size)
		{
			return m_messagePool.Get(MathHelper.GetNearestBiggerPowerOfTwo(Math.Max(size, 256)));
		}

		public ReceiveStatus ReceiveOne(out uint length)
		{
			length = 0u;
			if (MyGameService.Peer2Peer.IsPacketAvailable(out length, m_channel))
			{
				MyPacketDataPooled message = GetMessage((int)length);
				if (MyGameService.Peer2Peer.ReadPacket(message.Data, ref length, out var remoteUser, m_channel))
				{
					MyPacketData myPacketData = message;
					byte num = message.Data[0];
					bool flag = message.Data[1] > 0;
					if (num != 206 || (flag && !CheckCrc(message.Data, 2, 6, (int)(length - 6))))
					{
						string text = null;
						for (int i = 0; i < Math.Min(10u, length); i++)
						{
							text = text + message.Data[i].ToString("X") + " ";
						}
						MyLog.Default.WriteLine($"ERROR! Invalid packet from channel #{m_channel} length {length} from {remoteUser} initial bytes: {text}");
						message.Return();
						return ReceiveStatus.TamperredPacket;
					}
					byte b = message.Data[6];
					byte b2 = message.Data[7];
					int num2 = 8;
					if (b2 > 1)
					{
						if (b == 0)
						{
							int num3 = b2 * 1000000;
							m_largePacket = new byte[num3];
						}
						else if (m_largePacket == null)
						{
							MyLog.Default.WriteLine($"ERROR! Invalid packet from channel #{m_channel} length {length} from {remoteUser}." + $"Packet claims to be index {b} of a split message but previous parts were not received.");
							message.Return();
							return ReceiveStatus.TamperredPacket;
						}
						Array.Copy(message.Data, 8L, m_largePacket, b * 1000000, length - 8);
						m_largePacketCounter++;
						message.Return();
						if (b != b2 - 1)
						{
							return ReceiveOne(out length);
						}
						myPacketData = new MyPacketData
						{
							Data = m_largePacket,
							BitStream = new BitStream(0),
							ByteStream = new ByteStream()
						};
						num2 = 0;
						length -= 8u;
						length += (uint)((b2 - 1) * 1000000);
						m_largePacketCounter = 0;
						m_largePacket = null;
					}
					myPacketData.BitStream.ResetRead(myPacketData.Data, num2, (int)(length - num2) * 8, copy: false);
					myPacketData.ByteStream.Reset(myPacketData.Data, (int)length);
					myPacketData.ByteStream.Position = num2;
					myPacketData.ReceivedTime = MyTimeSpan.FromTicks(Stopwatch.GetTimestamp());
					myPacketData.Sender = new Endpoint(remoteUser, 0);
					m_receiveQueue.Enqueue((MyPacket)myPacketData);
					return ReceiveStatus.Success;
				}
				return ReceiveStatus.None;
			}
			return ReceiveStatus.None;
		}

		private unsafe bool CheckCrc(byte[] data, int crcIndex, int dataIndex, int dataLength)
		{
<<<<<<< HEAD
			m_crc32.Initialize();
			m_crc32.ComputeHash(data, dataIndex, dataLength);
=======
			((HashAlgorithm)m_crc32).Initialize();
			((HashAlgorithm)m_crc32).ComputeHash(data, dataIndex, dataLength);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			int num;
			fixed (byte* ptr = data)
			{
				num = *(int*)(ptr + crcIndex);
			}
			return num == (int)m_crc32.CrcValue;
		}

		public void Dispose()
		{
			MyPacket myPacket = default(MyPacket);
			while (m_receiveQueue.TryDequeue(ref myPacket))
			{
				myPacket.Return();
			}
		}

		/// <summary>
		/// Processes messages
		/// Delay indicates how long must be message in queue before processing (lag emulation)
		/// </summary>
		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		public void Process(NetworkMessageDelegate handler)
		{
			MyPacket myPacket = default(MyPacket);
			while (m_receiveQueue.TryDequeue(ref myPacket))
			{
				try
				{
					handler(myPacket);
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
<<<<<<< HEAD
					MyLog.Default.WriteLine("Packet processing error, disconnecting " + result.Sender);
=======
					MyLog.Default.WriteLine("Packet processing error, disconnecting " + myPacket.Sender);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (!Sync.IsServer)
					{
						throw;
					}
					m_disconnectPeerOnError(myPacket.Sender.Id.Value);
				}
			}
		}
	}
}
