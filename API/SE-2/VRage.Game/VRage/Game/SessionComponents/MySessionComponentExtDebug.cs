using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Debugging;
using VRage.Library;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.SessionComponents
{
	/// <summary>
	/// Communication between game and editor.
	/// </summary>
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MySessionComponentExtDebug : MySessionComponentBase
	{
		private class MyDebugClientInfo
		{
			public TcpClient TcpClient;

			public bool IsHeaderValid;

			public MyExternalDebugStructures.CommonMsgHeader Header;
		}

		public delegate void ReceivedMsgHandler(MyExternalDebugStructures.CommonMsgHeader messageHeader, byte[] messageData);

		public static MySessionComponentExtDebug Static;

		public static bool ForceDisable;

		public const int GameDebugPort = 13000;

		private const int MsgSizeLimit = 1048576;

		private Thread m_listenerThread;

		private TcpListener m_listener;

		private ConcurrentCachingList<MyDebugClientInfo> m_clients = new ConcurrentCachingList<MyDebugClientInfo>(1);

		private bool m_active;

		private byte[] m_arrayBuffer = new byte[1048576];

		private NativeArray m_tempBuffer;

		private ConcurrentCachingList<ReceivedMsgHandler> m_receivedMsgHandlers = new ConcurrentCachingList<ReceivedMsgHandler>();

		public bool HasClients => m_clients.Count > 0;

		public event ReceivedMsgHandler ReceivedMsg
		{
			add
			{
				m_receivedMsgHandlers.Add(value);
				m_receivedMsgHandlers.ApplyAdditions();
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

		public bool IsHandlerRegistered(ReceivedMsgHandler handler)
		{
			return Enumerable.Contains<ReceivedMsgHandler>((IEnumerable<ReceivedMsgHandler>)m_receivedMsgHandlers, handler);
		}

		public override void LoadData()
		{
			if (Static != null)
			{
				m_listenerThread = Static.m_listenerThread;
				m_listener = Static.m_listener;
				m_clients = Static.m_clients;
				m_active = Static.m_active;
				m_arrayBuffer = Static.m_arrayBuffer;
				m_tempBuffer = Static.m_tempBuffer;
				Static.m_tempBuffer = null;
				m_receivedMsgHandlers = Static.m_receivedMsgHandlers;
				Static = this;
				base.LoadData();
			}
			else
			{
				Static = this;
				if (!ForceDisable && MyVRage.Platform.System.IsRemoteDebuggingSupported)
				{
					m_tempBuffer = MyDebug.DebugMemoryAllocator.Allocate(1048576);
					StartServer();
				}
				base.LoadData();
			}
		}

		protected override void UnloadData()
		{
			m_receivedMsgHandlers.ClearImmediate();
			base.UnloadData();
			Session = null;
		}

		public void Dispose()
		{
			m_receivedMsgHandlers.ClearList();
			if (m_active)
			{
				StopServer();
			}
			if (m_tempBuffer != null)
			{
				MyDebug.DebugMemoryAllocator.Dispose(m_tempBuffer);
			}
		}

		/// <summary>
		/// Start using this component as server (game side).
		/// </summary>
		private bool StartServer()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			if (!m_active)
			{
				Thread val = new Thread((ThreadStart)ServerListenerProc);
				val.set_IsBackground(true);
				m_listenerThread = val;
				m_listenerThread.Start();
				m_active = true;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Stop the server on the game side. Called automatically.
		/// </summary>
		private void StopServer()
		{
			if (!m_active || m_listenerThread == null)
<<<<<<< HEAD
			{
				return;
			}
			m_listener.Stop();
			foreach (MyDebugClientInfo client in m_clients)
			{
				if (client.TcpClient != null)
				{
					client.TcpClient.Client.Disconnect(reuseSocket: true);
=======
			{
				return;
			}
			m_listener.Stop();
			foreach (MyDebugClientInfo client in m_clients)
			{
				if (client.TcpClient != null)
				{
					client.TcpClient.get_Client().Disconnect(true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					client.TcpClient.Close();
				}
			}
			m_clients.ClearImmediate();
			m_active = false;
		}

		/// <summary>
		/// Parallel thread - listener.
		/// </summary>
		private static void ServerListenerProc()
		{
<<<<<<< HEAD
			Thread.CurrentThread.Name = "External Debugging Listener";
			TcpListener tcpListener;
			try
			{
				tcpListener = new TcpListener(IPAddress.Loopback, 13000)
				{
					ExclusiveAddressUse = false
				};
				tcpListener.Start();
				Static.m_listener = tcpListener;
=======
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_003b: Expected O, but got Unknown
			//IL_00d7: Expected O, but got Unknown
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Invalid comparison between Unknown and I4
			Thread.get_CurrentThread().set_Name("External Debugging Listener");
			TcpListener val2;
			try
			{
				TcpListener val = new TcpListener(IPAddress.Loopback, 13000);
				val.set_ExclusiveAddressUse(false);
				val2 = val;
				val2.Start();
				Static.m_listener = val2;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (SocketException val3)
			{
				SocketException ex = val3;
				MyLog.Default.WriteLine("Cannot start debug listener.");
<<<<<<< HEAD
				MyLog.Default.WriteLine(ex);
=======
				MyLog.Default.WriteLine((Exception)(object)ex);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Static.m_active = false;
				return;
			}
			MyLog.Default.WriteLine("External debugger: listening...");
			while (true)
			{
				try
				{
<<<<<<< HEAD
					TcpClient tcpClient = tcpListener.AcceptTcpClient();
					tcpClient.Client.Blocking = true;
					MyLog.Default.WriteLine("External debugger: accepted client.");
					Static.m_clients.Add(new MyDebugClientInfo
					{
						TcpClient = tcpClient,
=======
					TcpClient val4 = val2.AcceptTcpClient();
					val4.get_Client().set_Blocking(true);
					MyLog.Default.WriteLine("External debugger: accepted client.");
					Static.m_clients.Add(new MyDebugClientInfo
					{
						TcpClient = val4,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						IsHeaderValid = false,
						Header = default(MyExternalDebugStructures.CommonMsgHeader)
					});
					Static.m_clients.ApplyAdditions();
				}
				catch (SocketException val5)
				{
<<<<<<< HEAD
					if (ex2.SocketErrorCode == SocketError.Interrupted)
					{
						tcpListener.Stop();
						tcpListener = null;
=======
					SocketException val6 = val5;
					if ((int)val6.get_SocketErrorCode() == 10004)
					{
						val2.Stop();
						val2 = null;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						MyLog.Default.WriteLine("External debugger: interrupted.");
						return;
					}
					if (MyLog.Default != null && MyLog.Default.LogEnabled)
					{
<<<<<<< HEAD
						MyLog.Default.WriteLine(ex2);
=======
						MyLog.Default.WriteLine((Exception)(object)val6);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					break;
				}
			}
<<<<<<< HEAD
			tcpListener.Stop();
=======
			val2.Stop();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Static.m_listener = null;
		}

		public override void UpdateBeforeSimulation()
		{
			foreach (MyDebugClientInfo client in m_clients)
			{
				if (client == null || client.TcpClient == null || client.TcpClient.get_Client() == null || !client.TcpClient.get_Connected())
				{
					if (client != null && client.TcpClient != null && client.TcpClient.get_Client() != null && client.TcpClient.get_Client().get_Connected())
					{
						client.TcpClient.get_Client().Disconnect(true);
						client.TcpClient.Close();
					}
					m_clients.Remove(client);
				}
				else if (client.TcpClient.get_Connected() && client.TcpClient.get_Available() > 0)
				{
					ReadMessagesFromClients(client);
				}
			}
			m_clients.ApplyRemovals();
		}

		private void ReadMessagesFromClients(MyDebugClientInfo clientInfo)
		{
			Socket client = clientInfo.TcpClient.get_Client();
			while (client.get_Available() >= 0)
			{
				bool flag = false;
<<<<<<< HEAD
				if (!clientInfo.IsHeaderValid && client.Available >= MyExternalDebugStructures.MsgHeaderSize)
				{
					client.Receive(m_arrayBuffer, MyExternalDebugStructures.MsgHeaderSize, SocketFlags.None);
=======
				if (!clientInfo.IsHeaderValid && client.get_Available() >= MyExternalDebugStructures.MsgHeaderSize)
				{
					client.Receive(m_arrayBuffer, MyExternalDebugStructures.MsgHeaderSize, (SocketFlags)0);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					Marshal.Copy(m_arrayBuffer, 0, m_tempBuffer.Ptr, MyExternalDebugStructures.MsgHeaderSize);
					MyExternalDebugStructures.CommonMsgHeader header = (MyExternalDebugStructures.CommonMsgHeader)Marshal.PtrToStructure(m_tempBuffer.Ptr, typeof(MyExternalDebugStructures.CommonMsgHeader));
					clientInfo.IsHeaderValid = true;
					clientInfo.Header = header;
					flag = true;
				}
<<<<<<< HEAD
				if (clientInfo.IsHeaderValid && client.Available >= clientInfo.Header.MsgSize)
				{
					if (clientInfo.Header.MsgSize > 0)
					{
						client.Receive(m_arrayBuffer, clientInfo.Header.MsgSize, SocketFlags.None);
=======
				if (clientInfo.IsHeaderValid && client.get_Available() >= clientInfo.Header.MsgSize)
				{
					if (clientInfo.Header.MsgSize > 0)
					{
						client.Receive(m_arrayBuffer, clientInfo.Header.MsgSize, (SocketFlags)0);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					if (m_receivedMsgHandlers != null && m_receivedMsgHandlers.Count > 0)
					{
						foreach (ReceivedMsgHandler receivedMsgHandler in m_receivedMsgHandlers)
						{
							receivedMsgHandler?.Invoke(clientInfo.Header, m_arrayBuffer);
						}
					}
					clientInfo.IsHeaderValid = false;
					flag = true;
				}
				if (!flag)
				{
					break;
				}
			}
		}

		public void SendMessageToClients<TMessage>(TMessage msg) where TMessage : struct, MyExternalDebugStructures.IExternalDebugMsg
		{
			if (m_tempBuffer == null)
			{
				return;
			}
			ISerializer<TMessage> serializer = MyExternalDebugStructures.GetSerializer<TMessage>();
			ByteStream byteStream = new ByteStream(256);
			serializer.Serialize(byteStream, ref msg);
			Marshal.StructureToPtr(MyExternalDebugStructures.CommonMsgHeader.Create(msg.GetTypeStr(), (int)byteStream.Position), m_tempBuffer.Ptr, fDeleteOld: true);
			Marshal.Copy(m_tempBuffer.Ptr, m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize);
			Array.Copy(byteStream.Data, 0L, m_arrayBuffer, MyExternalDebugStructures.MsgHeaderSize, byteStream.Position);
			foreach (MyDebugClientInfo client in m_clients)
			{
				try
				{
					if (client.TcpClient.get_Client() != null)
					{
<<<<<<< HEAD
						client.TcpClient.Client.Send(m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize + (int)byteStream.Position, SocketFlags.None);
=======
						client.TcpClient.get_Client().Send(m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize + (int)byteStream.Position, (SocketFlags)0);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				catch (SocketException)
				{
					client.TcpClient.Close();
				}
			}
		}
	}
}
