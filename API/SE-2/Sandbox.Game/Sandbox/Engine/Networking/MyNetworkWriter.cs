using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
<<<<<<< HEAD
=======
using System.Security.Cryptography;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.GameServices;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Engine.Networking
{
	public static class MyNetworkWriter
	{
		[GenerateActivator]
		private class MyPacketDataBitStream : MyPacketDataBitStreamBase
		{
			private class Sandbox_Engine_Networking_MyNetworkWriter_003C_003EMyPacketDataBitStream_003C_003EActor : IActivator, IActivator<MyPacketDataBitStream>
			{
				private sealed override object CreateInstance()
				{
					return new MyPacketDataBitStream();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyPacketDataBitStream CreateInstance()
				{
					return new MyPacketDataBitStream();
				}

				MyPacketDataBitStream IActivator<MyPacketDataBitStream>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			public override IntPtr Ptr => base.Stream.DataPointer;

			public override int Size => base.Stream.BytePosition;

			public override byte[] Data => null;

			public override int Offset => 0;

			public override void Return()
			{
				base.Stream.ResetWrite();
				m_returned = true;
				m_bitStreamPool.Return(this);
			}

			public void Acquire()
			{
				m_returned = false;
			}
		}

		[GenerateActivator]
		private class MyPacketDataArray : IPacketData
		{
			private class Sandbox_Engine_Networking_MyNetworkWriter_003C_003EMyPacketDataArray_003C_003EActor : IActivator, IActivator<MyPacketDataArray>
			{
				private sealed override object CreateInstance()
				{
					return new MyPacketDataArray();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyPacketDataArray CreateInstance()
				{
					return new MyPacketDataArray();
				}

				MyPacketDataArray IActivator<MyPacketDataArray>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			public byte[] Data { get; set; }

			public IntPtr Ptr { get; set; }

			public int Size { get; set; }

			public int Offset { get; set; }

			public void Return()
			{
				Data = null;
				Ptr = (IntPtr)0;
				m_arrayPacketPool.Return(this);
			}
		}

		[GenerateActivator]
		public class MyPacketDescriptor
		{
			private class Sandbox_Engine_Networking_MyNetworkWriter_003C_003EMyPacketDescriptor_003C_003EActor : IActivator, IActivator<MyPacketDescriptor>
			{
				private sealed override object CreateInstance()
				{
					return new MyPacketDescriptor();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyPacketDescriptor CreateInstance()
				{
					return new MyPacketDescriptor();
				}

				MyPacketDescriptor IActivator<MyPacketDescriptor>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			public readonly List<EndpointId> Recipients = new List<EndpointId>();

			public MyP2PMessageEnum MsgType;

			public int Channel;

			public TimeSpan Stamp;

			public readonly ByteStream Header = new ByteStream(16);

			public IPacketData Data;

			public void Reset()
			{
				if (Data != null)
				{
					Data.Return();
				}
				Header.Position = 0L;
				Data = null;
				Recipients.Clear();
			}
		}

		public const byte PACKET_MAGIC = 206;

		public const int SIZE_MTR = 1000000;

		private static int m_byteCountSent;

		private static readonly Stopwatch m_timer;

		private static readonly ConcurrentQueue<MyPacketDescriptor> m_packetsToSend;

		private static readonly MyConcurrentPool<MyPacketDescriptor> m_descriptorPool;

		private static readonly MyConcurrentPool<MyPacketDataBitStream> m_bitStreamPool;

		private static readonly MyConcurrentPool<MyPacketDataArray> m_arrayPacketPool;

		private static readonly Crc32 m_crc32;

		private static readonly List<IPacketData> m_packetsTmp;

		private static readonly ByteStream m_streamTmp;

		public const int PACKET_HEADER_SIZE = 10;

<<<<<<< HEAD
		public static int QueuedBytes => m_packetsToSend.Sum((MyPacketDescriptor x) => x.Data?.Size ?? 0);

		/// <summary>
		/// Age of the oldest packet in the queue.
		/// </summary>
=======
		public static int QueuedBytes => Enumerable.Sum<MyPacketDescriptor>((IEnumerable<MyPacketDescriptor>)m_packetsToSend, (Func<MyPacketDescriptor, int>)((MyPacketDescriptor x) => x.Data?.Size ?? 0));

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static TimeSpan MaxPacketAge
		{
			get
			{
<<<<<<< HEAD
				if (!m_packetsToSend.TryPeek(out var result))
				{
					return TimeSpan.Zero;
				}
				return m_timer.Elapsed - result.Stamp;
=======
				MyPacketDescriptor myPacketDescriptor = default(MyPacketDescriptor);
				if (!m_packetsToSend.TryPeek(ref myPacketDescriptor))
				{
					return TimeSpan.Zero;
				}
				return m_timer.get_Elapsed() - myPacketDescriptor.Stamp;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		static MyNetworkWriter()
		{
			m_timer = Stopwatch.StartNew();
			m_packetsToSend = new ConcurrentQueue<MyPacketDescriptor>();
			m_descriptorPool = new MyConcurrentPool<MyPacketDescriptor>(0, delegate(MyPacketDescriptor x)
			{
				x.Reset();
			});
			m_bitStreamPool = new MyConcurrentPool<MyPacketDataBitStream>(0, null, 10000, null, delegate(MyPacketDataBitStream x)
			{
				x.Dispose();
			});
			m_arrayPacketPool = new MyConcurrentPool<MyPacketDataArray>();
			m_crc32 = new Crc32();
			m_packetsTmp = new List<IPacketData>();
			m_streamTmp = new ByteStream(1001024);
			MyVRage.RegisterExitCallback(delegate
			{
				foreach (MyPacketDescriptor item in m_packetsToSend)
				{
					item.Data?.Return();
				}
				m_bitStreamPool.Clean();
			});
		}

		public static void SendPacket(MyPacketDescriptor packet)
		{
<<<<<<< HEAD
			packet.Stamp = m_timer.Elapsed;
=======
			packet.Stamp = m_timer.get_Elapsed();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_packetsToSend.Enqueue(packet);
		}

		public static void SendAll()
		{
			int num = 0;
<<<<<<< HEAD
			int count = m_packetsToSend.Count;
			MyPacketDescriptor result;
			while (count-- > 0 && m_packetsToSend.TryDequeue(out result))
=======
			int count = m_packetsToSend.get_Count();
			MyPacketDescriptor myPacketDescriptor = default(MyPacketDescriptor);
			while (count-- > 0 && m_packetsToSend.TryDequeue(ref myPacketDescriptor))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_packetsTmp.Clear();
				m_packetsTmp.Add(myPacketDescriptor.Data);
				uint v = uint.MaxValue;
				switch (myPacketDescriptor.MsgType)
				{
				case MyP2PMessageEnum.ReliableWithBuffering:
					if (IsLastReliable(myPacketDescriptor.Channel))
					{
						myPacketDescriptor.MsgType = MyP2PMessageEnum.Reliable;
					}
					if (myPacketDescriptor.Data != null && myPacketDescriptor.Data.Size >= 999998)
					{
						Split(myPacketDescriptor);
					}
					break;
				case MyP2PMessageEnum.Reliable:
					if (myPacketDescriptor.Data != null && myPacketDescriptor.Data.Size >= 999998)
					{
						Split(myPacketDescriptor);
					}
					break;
				}
				byte b = 0;
<<<<<<< HEAD
				byte b2 = (byte)((result.MsgType == MyP2PMessageEnum.Unreliable || result.MsgType == MyP2PMessageEnum.UnreliableNoDelay) ? 1u : 0u);
=======
				byte b2 = (byte)((myPacketDescriptor.MsgType == MyP2PMessageEnum.Unreliable || myPacketDescriptor.MsgType == MyP2PMessageEnum.UnreliableNoDelay) ? 1u : 0u);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				foreach (IPacketData item in m_packetsTmp)
				{
					m_streamTmp.Position = 0L;
					((Stream)m_streamTmp).WriteNoAlloc((byte)206);
					m_streamTmp.WriteNoAlloc(b2);
					int num2 = (int)m_streamTmp.Position;
					m_streamTmp.WriteNoAlloc(v);
					m_streamTmp.WriteNoAlloc(b);
					m_streamTmp.WriteNoAlloc((byte)m_packetsTmp.Count);
					if (b == 0)
					{
						m_streamTmp.Write(myPacketDescriptor.Header.Data, 0, (int)myPacketDescriptor.Header.Position);
					}
					if (item != null)
					{
						int num3 = Math.Min(item.Size, 1000000);
						if (b == 0 && m_packetsTmp.Count > 1)
						{
							num3 -= (int)myPacketDescriptor.Header.Position;
						}
						if (item.Data != null)
						{
							m_streamTmp.Write(item.Data, item.Offset, num3);
						}
						else
						{
							m_streamTmp.Write(item.Ptr, item.Offset, num3);
						}
					}
					if (b2 > 0)
					{
						int num4 = (int)m_streamTmp.Position;
						((HashAlgorithm)m_crc32).Initialize();
						((HashAlgorithm)m_crc32).ComputeHash(m_streamTmp.Data, num2 + 4, num4 - num2 - 4);
						v = m_crc32.CrcValue;
						m_streamTmp.Position = num2;
						m_streamTmp.WriteNoAlloc(v);
						m_streamTmp.Position = num4;
					}
					foreach (EndpointId recipient in myPacketDescriptor.Recipients)
					{
						num += (int)m_streamTmp.Position;
<<<<<<< HEAD
						if (!MyGameService.Peer2Peer.SendPacket(recipient.Value, m_streamTmp.Data, (int)m_streamTmp.Position, result.MsgType, result.Channel))
=======
						if (!MyGameService.Peer2Peer.SendPacket(recipient.Value, m_streamTmp.Data, (int)m_streamTmp.Position, myPacketDescriptor.MsgType, myPacketDescriptor.Channel))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							MyLog.Default.WriteLine("P2P packet not sent");
						}
					}
					b = (byte)(b + 1);
				}
				foreach (IPacketData item2 in m_packetsTmp)
				{
					item2?.Return();
				}
				myPacketDescriptor.Data = null;
				m_descriptorPool.Return(myPacketDescriptor);
			}
			Interlocked.Add(ref m_byteCountSent, num);
		}

		private static void Split(MyPacketDescriptor packet)
		{
			int i = 1000000 - (int)packet.Header.Position;
			int num;
			for (int size = packet.Data.Size; i < size; i += num)
			{
				num = Math.Min(size - i, 1000000);
				IPacketData item = ((packet.Data.Data == null) ? GetPacketData(packet.Data.Ptr, i, num) : GetPacketData(packet.Data.Data, i, num));
				m_packetsTmp.Add(item);
			}
		}

		private static bool IsLastReliable(int channel)
		{
			foreach (MyPacketDescriptor item in m_packetsToSend)
			{
				if (item.Channel == channel && (item.MsgType == MyP2PMessageEnum.Reliable || item.MsgType == MyP2PMessageEnum.ReliableWithBuffering))
				{
					return false;
				}
			}
			return true;
		}

		public static int GetAndClearStats()
		{
			return Interlocked.Exchange(ref m_byteCountSent, 0);
		}

		public static MyPacketDataBitStreamBase GetBitStreamPacketData()
		{
			MyPacketDataBitStream myPacketDataBitStream = m_bitStreamPool.Get();
			myPacketDataBitStream.Acquire();
			return myPacketDataBitStream;
		}

		public static IPacketData GetPacketData(IntPtr data, int offset, int size)
		{
			MyPacketDataArray myPacketDataArray = m_arrayPacketPool.Get();
			myPacketDataArray.Ptr = data;
			myPacketDataArray.Offset = offset;
			myPacketDataArray.Size = size;
			return myPacketDataArray;
		}

		public static IPacketData GetPacketData(byte[] data, int offset, int size)
		{
			MyPacketDataArray myPacketDataArray = m_arrayPacketPool.Get();
			myPacketDataArray.Data = data;
			myPacketDataArray.Offset = offset;
			myPacketDataArray.Size = size;
			return myPacketDataArray;
		}

		internal static MyPacketDescriptor GetPacketDescriptor(EndpointId userId, MyP2PMessageEnum msgType, int channel)
		{
			MyPacketDescriptor myPacketDescriptor = m_descriptorPool.Get();
			myPacketDescriptor.MsgType = msgType;
			myPacketDescriptor.Channel = channel;
			if (userId.IsValid)
			{
				myPacketDescriptor.Recipients.Add(userId);
			}
			return myPacketDescriptor;
		}
	}
}
