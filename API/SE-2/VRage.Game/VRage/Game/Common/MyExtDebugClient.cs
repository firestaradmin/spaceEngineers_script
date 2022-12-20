using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using VRage.Collections;
using VRage.Game.Debugging;
using VRage.Serialization;

namespace VRage.Game.Common
{
	/// <summary>
	/// Auto-debug client.
	/// </summary>
	public class MyExtDebugClient : IDisposable
	{
		public delegate void ReceivedMsgHandler(MyExternalDebugStructures.CommonMsgHeader messageHeader, byte[] messageData);

		public const int GameDebugPort = 13000;

		private const int MsgSizeLimit = 1048576;

		private TcpClient m_client;

		private readonly byte[] m_arrayBuffer = new byte[1048576];

		private IntPtr m_tempBuffer;

		private Thread m_clientThread;

		private bool m_finished;

		private readonly ConcurrentCachingList<ReceivedMsgHandler> m_receivedMsgHandlers = new ConcurrentCachingList<ReceivedMsgHandler>();

		public bool ConnectedToGame
		{
			get
			{
				if (m_client != null)
				{
					return m_client.get_Connected();
				}
				return false;
			}
		}

		public event ReceivedMsgHandler ReceivedMsg
		{
			add
			{
				if (!Enumerable.Contains<ReceivedMsgHandler>((IEnumerable<ReceivedMsgHandler>)m_receivedMsgHandlers, value))
				{
					m_receivedMsgHandlers.Add(value);
					m_receivedMsgHandlers.ApplyAdditions();
				}
			}
			remove
			{
				if (Enumerable.Contains<ReceivedMsgHandler>((IEnumerable<ReceivedMsgHandler>)m_receivedMsgHandlers, value))
				{
					m_receivedMsgHandlers.Remove(value);
					m_receivedMsgHandlers.ApplyRemovals();
				}
			}
		}

		public MyExtDebugClient()
		{
<<<<<<< HEAD
=======
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_tempBuffer = Marshal.AllocHGlobal(1048576);
			m_finished = false;
			Thread val = new Thread((ThreadStart)ClientThreadProc);
			val.set_IsBackground(true);
			m_clientThread = val;
			m_clientThread.Start();
		}

		public void Dispose()
		{
			m_finished = true;
			if (m_client != null)
			{
				if (m_client.get_Client().get_Connected())
				{
					m_client.get_Client().Disconnect(false);
				}
				m_client.Close();
			}
			Marshal.FreeHGlobal(m_tempBuffer);
		}

		private void ClientThreadProc()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			while (!m_finished)
			{
				if (m_client == null || m_client.get_Client() == null || !m_client.get_Connected())
				{
					try
					{
						m_client = new TcpClient();
						m_client.Connect(IPAddress.Loopback, 13000);
					}
					catch (Exception)
					{
					}
					if (m_client == null || m_client.get_Client() == null || !m_client.get_Connected())
					{
						Thread.Sleep(2500);
						continue;
					}
				}
				try
				{
<<<<<<< HEAD
					if (m_client.Client == null)
					{
						continue;
					}
					if (m_client.Client.Receive(m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize, SocketFlags.None) == 0)
					{
						m_client.Client.Close();
						m_client.Client = null;
						m_client = null;
						continue;
					}
					Marshal.Copy(m_arrayBuffer, 0, m_tempBuffer, MyExternalDebugStructures.MsgHeaderSize);
					MyExternalDebugStructures.CommonMsgHeader messageHeader = (MyExternalDebugStructures.CommonMsgHeader)Marshal.PtrToStructure(m_tempBuffer, typeof(MyExternalDebugStructures.CommonMsgHeader));
					if (!messageHeader.IsValid)
					{
						continue;
					}
					if (messageHeader.MsgSize > 0)
					{
						m_client.Client.Receive(m_arrayBuffer, messageHeader.MsgSize, SocketFlags.None);
						Marshal.Copy(m_arrayBuffer, 0, m_tempBuffer, messageHeader.MsgSize);
					}
					if (m_receivedMsgHandlers == null)
					{
						continue;
					}
					foreach (ReceivedMsgHandler receivedMsgHandler in m_receivedMsgHandlers)
					{
=======
					if (m_client.get_Client() == null)
					{
						continue;
					}
					if (m_client.get_Client().Receive(m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize, (SocketFlags)0) == 0)
					{
						m_client.get_Client().Close();
						m_client.set_Client((Socket)null);
						m_client = null;
						continue;
					}
					Marshal.Copy(m_arrayBuffer, 0, m_tempBuffer, MyExternalDebugStructures.MsgHeaderSize);
					MyExternalDebugStructures.CommonMsgHeader messageHeader = (MyExternalDebugStructures.CommonMsgHeader)Marshal.PtrToStructure(m_tempBuffer, typeof(MyExternalDebugStructures.CommonMsgHeader));
					if (!messageHeader.IsValid)
					{
						continue;
					}
					if (messageHeader.MsgSize > 0)
					{
						m_client.get_Client().Receive(m_arrayBuffer, messageHeader.MsgSize, (SocketFlags)0);
						Marshal.Copy(m_arrayBuffer, 0, m_tempBuffer, messageHeader.MsgSize);
					}
					if (m_receivedMsgHandlers == null)
					{
						continue;
					}
					foreach (ReceivedMsgHandler receivedMsgHandler in m_receivedMsgHandlers)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						receivedMsgHandler?.Invoke(messageHeader, m_arrayBuffer);
					}
				}
				catch (SocketException)
				{
<<<<<<< HEAD
					if (m_client?.Client != null)
=======
					if (m_client.get_Client() != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						m_client.get_Client().Close();
						m_client.set_Client((Socket)null);
						m_client = null;
					}
				}
				catch (ObjectDisposedException)
				{
<<<<<<< HEAD
					if (m_client?.Client != null)
=======
					if (m_client.get_Client() != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						m_client.get_Client().Close();
						m_client.set_Client((Socket)null);
						m_client = null;
					}
				}
				catch (Exception)
				{
				}
			}
		}

		public bool SendMessageToGame<TMessage>(TMessage msg) where TMessage : struct, MyExternalDebugStructures.IExternalDebugMsg
		{
			if (m_client == null || m_client.get_Client() == null || !m_client.get_Connected())
			{
				return false;
			}
			ISerializer<TMessage> serializer = MyExternalDebugStructures.GetSerializer<TMessage>();
			ByteStream byteStream = new ByteStream(256);
			serializer.Serialize(byteStream, ref msg);
			Marshal.StructureToPtr(MyExternalDebugStructures.CommonMsgHeader.Create(msg.GetTypeStr(), (int)byteStream.Position), m_tempBuffer, fDeleteOld: true);
			Marshal.Copy(m_tempBuffer, m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize);
			Array.Copy(byteStream.Data, 0L, m_arrayBuffer, MyExternalDebugStructures.MsgHeaderSize, byteStream.Position);
			try
			{
<<<<<<< HEAD
				m_client.Client.Send(m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize + (int)byteStream.Position, SocketFlags.None);
=======
				m_client.get_Client().Send(m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize + (int)byteStream.Position, (SocketFlags)0);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (SocketException)
			{
				return false;
			}
			return true;
		}
	}
}
