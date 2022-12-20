using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Sandbox.Engine.Utils;
using VRage.GameServices;
using VRage.Profiler;
using VRage.Utils;

namespace Sandbox.Engine.Networking
{
	public static class MyNetworkMonitor
	{
<<<<<<< HEAD
		/// <summary>
		/// Delay between polling for network packets in milliseconds.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static volatile int UpdateLatency = 8;

		private static Thread m_workerThread;

		private static bool m_sessionEnabled;

		private static bool m_running;

		public static event Action OnTick;

		public static void Init()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			m_running = true;
			if (!MyFakes.NETWORK_SINGLE_THREADED && m_workerThread == null)
			{
<<<<<<< HEAD
				m_workerThread = new Thread(Worker)
				{
					CurrentCulture = CultureInfo.InvariantCulture,
					CurrentUICulture = CultureInfo.InvariantCulture,
					Name = "Network Monitor"
				};
=======
				Thread val = new Thread((ThreadStart)Worker);
				val.set_CurrentCulture(CultureInfo.InvariantCulture);
				val.set_CurrentUICulture(CultureInfo.InvariantCulture);
				val.set_Name("Network Monitor");
				m_workerThread = val;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_workerThread.Start();
			}
		}

		public static void Done()
		{
			m_running = false;
			if (m_workerThread != null)
			{
				m_workerThread.Join();
				m_workerThread = null;
			}
		}

		public static void StartSession()
		{
			m_sessionEnabled = true;
		}

		public static void EndSession()
		{
			m_sessionEnabled = false;
		}

		public static void Update()
		{
			if (m_workerThread == null)
			{
				UpdateInternal();
			}
		}

		private static int UpdateInternal()
		{
<<<<<<< HEAD
			MyGameService.UpdateNetworkThread(m_sessionEnabled);
=======
			MyGameService.UpdateNetworkThread();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			IMyPeer2Peer peer2Peer = MyGameService.Peer2Peer;
			int result = UpdateLatency;
			if (peer2Peer != null)
			{
				MyNetworkWriter.SendAll();
				peer2Peer.BeginFrameProcessing();
				try
				{
					if (m_sessionEnabled)
					{
						MyNetworkReader.ReceiveAll();
					}
				}
				finally
				{
					peer2Peer.EndFrameProcessing();
				}
				result = peer2Peer.NetworkUpdateLatency;
			}
			MyNetworkMonitor.OnTick?.Invoke();
			ProfilerShort.Commit(false);
			return result;
		}

		private static void Worker()
		{
			try
			{
				ProfilerShort.Autocommit = false;
				int val = UpdateLatency;
				while (m_running)
				{
					Thread.Sleep(Math.Max(Math.Min(UpdateLatency, val), 1));
					val = UpdateInternal();
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
				Debugger.Break();
				throw;
			}
		}
	}
}
