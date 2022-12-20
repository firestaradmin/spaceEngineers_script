using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using VRage.Utils;

namespace Sandbox.Engine.Networking
{
	internal static class MyNetworkReader
	{
		private class ChannelInfo
		{
			public MyReceiveQueue Queue;

			public NetworkMessageDelegate Handler;
		}

		private static int m_byteCountReceived;

		private static int m_tamperred;

		private static readonly ConcurrentDictionary<int, ChannelInfo> m_channels = new ConcurrentDictionary<int, ChannelInfo>();

		public static void SetHandler(int channel, NetworkMessageDelegate handler, Action<ulong> disconnectPeerOnError)
		{
<<<<<<< HEAD
			if (m_channels.TryGetValue(channel, out var value))
=======
			ChannelInfo channelInfo = default(ChannelInfo);
			if (m_channels.TryGetValue(channel, ref channelInfo))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				channelInfo.Queue.Dispose();
			}
			channelInfo = new ChannelInfo
			{
				Handler = handler,
				Queue = new MyReceiveQueue(channel, disconnectPeerOnError)
			};
			m_channels.set_Item(channel, channelInfo);
		}

		public static void ClearHandler(int channel)
		{
<<<<<<< HEAD
			if (m_channels.TryGetValue(channel, out var value))
=======
			ChannelInfo channelInfo = default(ChannelInfo);
			if (m_channels.TryGetValue(channel, ref channelInfo))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				channelInfo.Queue.Dispose();
			}
			m_channels.Remove<int, ChannelInfo>(channel);
		}

		public static void Clear()
		{
			foreach (KeyValuePair<int, ChannelInfo> channel in m_channels)
			{
				channel.Value.Queue.Dispose();
			}
			m_channels.Clear();
			MyLog.Default.WriteLine("Network readers disposed");
		}

		public static void Process()
		{
			foreach (KeyValuePair<int, ChannelInfo> channel in m_channels)
			{
				channel.Value.Queue.Process(channel.Value.Handler);
			}
		}

		public static void GetAndClearStats(out int received, out int tamperred)
		{
			received = Interlocked.Exchange(ref m_byteCountReceived, 0);
			tamperred = Interlocked.Exchange(ref m_tamperred, 0);
		}

		public static void ReceiveAll()
		{
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<int, ChannelInfo> channel in m_channels)
			{
				while (true)
				{
					uint length;
					MyReceiveQueue.ReceiveStatus receiveStatus = channel.Value.Queue.ReceiveOne(out length);
					if (receiveStatus == MyReceiveQueue.ReceiveStatus.None)
					{
						break;
					}
					num += (int)length;
					if (receiveStatus == MyReceiveQueue.ReceiveStatus.TamperredPacket)
					{
						num2++;
					}
				}
			}
			Interlocked.Add(ref m_byteCountReceived, num);
			Interlocked.Add(ref m_tamperred, num2);
		}
	}
}
