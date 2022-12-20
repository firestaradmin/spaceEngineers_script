using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using VRage;
using VRage.Game.Entity;
using VRage.Library.Memory;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Profiler;
using VRage.Replication;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine
{
	public class MyGeneralStats
	{
		private struct MemoryLogger : MyMemoryTracker.ILogger
		{
			private string m_currentSystem;

			public void BeginSystem(string systemName)
			{
				if (m_currentSystem != null)
				{
<<<<<<< HEAD
					MyStatsGraph.Begin(m_currentSystem, int.MaxValue, string.Empty, 566, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
=======
					MyStatsGraph.Begin(m_currentSystem, int.MaxValue, string.Empty, 568, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				m_currentSystem = systemName;
			}

			public void EndSystem(long systemBytes, int totalAllocations)
			{
				float customValue = totalAllocations;
				string customValueFormat = ((totalAllocations > 0) ? "Allocs: {0}" : string.Empty);
				if (m_currentSystem != null)
				{
<<<<<<< HEAD
					MyStatsGraph.CustomTime(m_currentSystem, (float)systemBytes / 1024f / 1024f, "{0} MB", customValue, customValueFormat, "EndSystem", 579, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
=======
					MyStatsGraph.CustomTime(m_currentSystem, (float)systemBytes / 1024f / 1024f, "{0} MB", customValue, customValueFormat, "EndSystem", 581, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_currentSystem = null;
				}
				else
				{
<<<<<<< HEAD
					MyStatsGraph.End((float)systemBytes / 1024f / 1024f, customValue, customValueFormat, "{0} MB", null, 0, string.Empty, 584, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
=======
					MyStatsGraph.End((float)systemBytes / 1024f / 1024f, customValue, customValueFormat, "{0} MB", null, 0, string.Empty, 586, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		private struct MyStringMemoryLogger : MyMemoryTracker.ILogger
		{
			private const int MaxDepth = 4;

			private Stack<string> m_blockStack;

			private StringBuilder m_stringBuilder;

			private Dictionary<string, int> m_blockValues;

			private string[] m_blockValuesOrder;

			private string m_blockValuesHeader;

			public MyStringMemoryLogger(bool _)
			{
				m_blockStack = new Stack<string>();
				m_stringBuilder = new StringBuilder();
				m_blockValues = new Dictionary<string, int>();
				m_blockValuesOrder = new string[36]
				{
					"Srv", "Uav", "Read", "Debug", "Index", "Audio", "SrvUav", "Vertex", "Buffers", "Physics",
					"Planets", "Systems", "Textures", "Indirect", "Constant", "RwTextures", "Dx11Render", "MeshBuffers", "FileTextures", "DepthStencil",
					"TileTextures", "Voxels-Native", "CustomTextures", "HeightmapFaces", "Mesh GPU Buffers", "BitStreamBuffers", "GeneratedTextures", "FileArrayTextures", "NativeDictionaries", "CubemapDataBuffers",
					"HeightDetailTexture", "MyDeviceWriteBuffers", "ShadowCascadesStatsBuffers", "AI_PathFinding", "EpicOnlineServices", "EpicOnlineServicesWrapper"
				};
				m_blockValuesHeader = "MEMORY LEGEND," + string.Join(",", m_blockValuesOrder);
			}

			public void BeginSystem(string systemName)
			{
				m_blockStack.Push(systemName);
			}

			public void EndSystem(long systemBytes, int totalAllocations)
			{
				string key = m_blockStack.Pop();
<<<<<<< HEAD
				if (m_blockStack.Count < 4)
=======
				if (m_blockStack.get_Count() < 4)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					m_blockValues[key] = (int)((float)systemBytes / 1024f / 1024f);
				}
			}

			public void PrintToLog(MyMemorySystem memorySystem, MyLog log)
			{
				memorySystem.LogMemoryStats(ref this);
				log.WriteLine(m_blockValuesHeader);
				StringBuilder stringBuilder = m_stringBuilder;
				stringBuilder.Append("MEMORY VALUES");
				string[] blockValuesOrder = m_blockValuesOrder;
				foreach (string key in blockValuesOrder)
				{
					m_blockValues.TryGetValue(key, out var value);
					stringBuilder.Append(',').Append(value);
				}
				log.WriteLine(stringBuilder.ToString());
				stringBuilder.Clear();
			}
		}

		private MyTimeSpan m_lastTime;

		private static int AVERAGE_WINDOW_SIZE;

		private static int SERVER_AVERAGE_WINDOW_SIZE;

		private readonly MyMovingAverage m_received = new MyMovingAverage(AVERAGE_WINDOW_SIZE);

		private readonly MyMovingAverage m_sent = new MyMovingAverage(AVERAGE_WINDOW_SIZE);

		private readonly MyMovingAverage m_timeIntervals = new MyMovingAverage(AVERAGE_WINDOW_SIZE);

		private readonly MyMovingAverage m_serverReceived = new MyMovingAverage(SERVER_AVERAGE_WINDOW_SIZE);

		private readonly MyMovingAverage m_serverSent = new MyMovingAverage(SERVER_AVERAGE_WINDOW_SIZE);

		private readonly MyMovingAverage m_serverTimeIntervals = new MyMovingAverage(SERVER_AVERAGE_WINDOW_SIZE);

		public MyTimeSpan LogInterval = MyTimeSpan.FromSeconds(60.0);

		private bool m_first = true;

		private MyTimeSpan m_lastLogTime;

		private MyTimeSpan m_lastGridLogTime;

		private MyTimeSpan m_firstLogTime;

		private int m_gridsCount;

		private bool m_wasMemoryCritical;

		private int[] m_lastGcCount = new int[GC.MaxGeneration + 1];

		private int[] m_collectionsThisFrame = new int[GC.MaxGeneration + 1];

		private readonly Dictionary<string, (MyValueAggregator Aggregator, string AnalyticsName, bool Bytes)> m_statAggregators;

		private double m_aggregatorTime;

		private const string DataReceivedAggregatorName = "$$data_received";

		private const string DataSentAggregatorName = "$$data_sent";

		private const string PingAggregatorName = "$$ping";

		public readonly int[] PercentileValues = new int[8] { 50, 90, 91, 92, 93, 94, 95, 99 };

		private static MyStringMemoryLogger m_stringMemoryLogger;

		public static MyGeneralStats Static { get; private set; }

		public float Received { get; private set; }

		public float Sent { get; private set; }

		public float ReceivedPerSecond { get; private set; }

		public float SentPerSecond { get; private set; }

		public float PeakReceivedPerSecond { get; private set; }

		public float PeakSentPerSecond { get; private set; }

		public long OverallReceived { get; private set; }

		public long OverallSent { get; private set; }

		public float ServerReceivedPerSecond { get; private set; }

		public float ServerSentPerSecond { get; private set; }

		public byte PlayoutDelayBufferSize { get; private set; }

		public float ServerGCMemory { get; private set; }

		public float ServerProcessMemory { get; private set; }

		public int GridsCount => m_gridsCount;

		public long Ping { get; set; }
<<<<<<< HEAD

		public bool LowNetworkQuality { get; private set; }

=======

		public bool LowNetworkQuality { get; private set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public IEnumerable<(string Name, double[] Value, bool Bytes)> AggregatedStats
		{
			get
			{
				foreach (KeyValuePair<string, (MyValueAggregator, string, bool)> statAggregator in m_statAggregators)
				{
					if (statAggregator.Value.Item1.HasData)
					{
						yield return (statAggregator.Value.Item2, statAggregator.Value.Item1.PercentileValues, statAggregator.Value.Item3);
					}
				}
			}
		}

		static MyGeneralStats()
		{
			AVERAGE_WINDOW_SIZE = 60;
			SERVER_AVERAGE_WINDOW_SIZE = 6;
			m_stringMemoryLogger = new MyStringMemoryLogger(_: false);
			Static = new MyGeneralStats();
		}

		private MyGeneralStats()
		{
			m_statAggregators = new Dictionary<string, (MyValueAggregator, string, bool)>
			{
				{
					"Total Sent",
					(new MyValueAggregator(300, PercentileValues), "p2p_data_sent", true)
				},
				{
					"Total Received",
					(new MyValueAggregator(300, PercentileValues), "p2p_data_received", true)
				},
				{
					"RTT (ms)",
					(new MyValueAggregator(300, PercentileValues), "p2p_rtt", false)
				},
				{
					"RTT Var (ms)",
					(new MyValueAggregator(300, PercentileValues), "p2p_rtt_var", false)
				},
				{
					"$$data_received",
					(new MyValueAggregator(300, PercentileValues), "data_received", true)
				},
				{
					"$$data_sent",
					(new MyValueAggregator(300, PercentileValues), "data_sent", true)
				},
				{
					"$$ping",
					(new MyValueAggregator(300, PercentileValues), "ping", false)
				}
			};
		}

		public void Update()
		{
			MyNetworkReader.GetAndClearStats(out var received, out var tamperred);
			int andClearStats = MyNetworkWriter.GetAndClearStats();
			OverallReceived += received;
			OverallSent += andClearStats;
			MyTimeSpan simulationTime = MySandboxGame.Static.SimulationTime;
			float num = (float)(simulationTime - m_lastTime).Seconds;
			m_lastTime = simulationTime;
			m_received.Enqueue(received);
			m_sent.Enqueue(andClearStats);
			m_timeIntervals.Enqueue(num);
			Received = m_received.Avg;
			Sent = m_sent.Avg;
			ReceivedPerSecond = (float)(m_received.Sum / m_timeIntervals.Sum);
			SentPerSecond = (float)(m_sent.Sum / m_timeIntervals.Sum);
			m_aggregatorTime += num;
			if (m_aggregatorTime > 1.0)
			{
				m_aggregatorTime -= 1.0;
				AggregateStatistics();
			}
			if (ReceivedPerSecond > PeakReceivedPerSecond)
			{
				PeakReceivedPerSecond = ReceivedPerSecond;
			}
			if (SentPerSecond > PeakSentPerSecond)
			{
				PeakSentPerSecond = SentPerSecond;
			}
			MyVRage.Platform.System.GetGCMemory(out var allocated, out var used);
			float customTime = MyVRage.Platform.System.RemainingMemoryForGame;
			float num2 = (float)MyVRage.Platform.System.ProcessPrivateMemory / 1024f / 1024f;
			for (int i = 0; i < GC.MaxGeneration; i++)
			{
				int num3 = GC.CollectionCount(i);
				m_collectionsThisFrame[i] = num3 - m_lastGcCount[i];
				m_lastGcCount[i] = num3;
			}
			if (Sync.MultiplayerActive && Sync.IsServer)
			{
				MyMultiplayer.Static.ReplicationLayer.UpdateStatisticsData(andClearStats, received, tamperred, used, num2);
			}
			bool flag = MySandboxGame.Static.MemoryState == MySandboxGame.MemState.Critical;
			bool flag2 = flag && !m_wasMemoryCritical;
			m_wasMemoryCritical = flag;
			if (MySession.Static != null && (simulationTime > m_lastLogTime + LogInterval || flag2))
			{
				m_lastLogTime = simulationTime;
				if (m_first)
				{
					m_firstLogTime = simulationTime;
					m_first = false;
				}
				MyLog.Default.WriteLine("STATISTICS LEGEND,time,ReceivedPerSecond,SentPerSecond,PeakReceivedPerSecond,PeakSentPerSecond,OverallReceived,OverallSent,CPULoadSmooth,ThreadLoadSmooth,GetOnlinePlayerCount,Ping,GCMemoryUsed,ProcessMemory,PCUBuilt,PCU,GridsCount,RenderCPULoadSmooth,RenderGPULoadSmooth,HardwareCPULoad,HardwareAvailableMemory,FrameTime,LowSimQuality,FrameTimeLimit,FrameTimeCPU,FrameTimeGPU,CPULoadLimit,TrackedMemory,GCMemoryAllocated,PersistedEncounters,EncounterEntities");
				float cPUCounter = MyVRage.Platform.System.CPUCounter;
				float rAMCounter = MyVRage.Platform.System.RAMCounter;
				float num4 = 16.666666f;
				if (MyFakes.ENABLE_PERFORMANCELOGGING)
				{
<<<<<<< HEAD
					Console.WriteLine(PerformanceLogMessage.SerializeObject(new PerformanceLogMessage
=======
					PerformanceLogMessage performanceLogMessage = new PerformanceLogMessage
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						Time = (simulationTime - m_firstLogTime).Seconds,
						ReceivedPerSecond = ReceivedPerSecond / 1024f / 1024f,
						SentPerSecond = SentPerSecond / 1024f / 1024f,
						PeakReceivedPerSecond = PeakReceivedPerSecond / 1024f / 1024f,
						PeakSentPerSecond = PeakSentPerSecond / 1024f / 1024f,
						OverallReceived = (float)OverallReceived / 1024f / 1024f,
						OverallSent = (float)OverallSent / 1024f / 1024f,
						CPULoadSmooth = MySandboxGame.Static.CPULoadSmooth,
						ThreadLoadSmooth = MySandboxGame.Static.ThreadLoadSmooth,
						GetOnlinePlayerCount = Sync.Players.GetOnlinePlayerCount(),
						Ping = Ping,
						GCMemoryUsed = used,
						ProcessMemory = num2,
						PCUBuilt = MySession.Static.SessionBlockLimits.PCUBuilt,
						PCU = MySession.Static.SessionBlockLimits.PCU,
						GridsCount = GridsCount,
						RenderCPULoadSmooth = MyRenderProxy.CPULoadSmooth,
						RenderGPULoadSmooth = MyRenderProxy.GPULoadSmooth,
						HardwareCPULoad = cPUCounter,
						HardwareAvailableMemory = rAMCounter,
						FrameTime = MyFpsManager.FrameTimeAvg,
						LowSimQuality = ((!MySession.Static.HighSimulationQuality) ? 1 : 0),
						FrameTimeLimit = num4,
						FrameTimeCPU = MyRenderProxy.CPULoadSmooth * MyFpsManager.FrameTimeAvg / 100f,
						FrameTimeGPU = MyRenderProxy.GPULoadSmooth * MyFpsManager.FrameTimeAvg / 100f,
						CPULoadLimit = 100f,
						TrackedMemory = Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.GetTotalMemory(),
						GCMemoryAllocated = allocated,
<<<<<<< HEAD
						ExePath = Environment.CurrentDirectory,
						GameVersion = MyPerGameSettings.BasicGameInfo.GameVersion.Value,
						SavePath = MySession.Static.CurrentPath
					}));
=======
						ExePath = Environment.get_CurrentDirectory(),
						GameVersion = MyPerGameSettings.BasicGameInfo.GameVersion.Value,
						SavePath = MySession.Static.CurrentPath
					};
					if (MyTestingToolHelper.CurrentTestPath != null)
					{
						performanceLogMessage.SavePath = MyTestingToolHelper.CurrentTestPath;
					}
					Console.WriteLine(PerformanceLogMessage.SerializeObject(performanceLogMessage));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				int persistentEncounters = 0;
				int encounterEntities = 0;
				MyEncounterGenerator.Static?.GetStats(out persistentEncounters, out encounterEntities);
				MyLog.Default.WriteLine(string.Join(",", "STATISTICS", (simulationTime - m_firstLogTime).Seconds, ReceivedPerSecond / 1024f / 1024f, SentPerSecond / 1024f / 1024f, PeakReceivedPerSecond / 1024f / 1024f, PeakSentPerSecond / 1024f / 1024f, (float)OverallReceived / 1024f / 1024f, (float)OverallSent / 1024f / 1024f, MySandboxGame.Static.CPULoadSmooth, MySandboxGame.Static.ThreadLoadSmooth, (!Sync.MultiplayerActive) ? 1 : MyMultiplayer.Static.MemberCount, Ping, used, num2, MySession.Static.TotalSessionPCU, MySession.Static.SessionBlockLimits.PCU, GridsCount, MyRenderProxy.CPULoadSmooth, MyRenderProxy.GPULoadSmooth, cPUCounter, rAMCounter, MyFpsManager.FrameTimeAvg, (!MySession.Static.HighSimulationQuality) ? 1 : 0, num4, MyRenderProxy.CPULoadSmooth * MyFpsManager.FrameTimeAvg / 100f, MyRenderProxy.GPULoadSmooth * MyFpsManager.FrameTimeAvg / 100f, 100, Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.GetTotalMemory(), allocated, persistentEncounters, encounterEntities));
				m_stringMemoryLogger.PrintToLog(Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem, MyLog.Default);
				if (MyGameService.Peer2Peer != null && MyPlatformGameSettings.VERBOSE_NETWORK_LOGGING)
				{
<<<<<<< HEAD
					(string, double)[] array = MyGameService.Peer2Peer.Stats.ToArray();
					if (array.Length != 0)
					{
						MyLog.Default.WriteLine("Detailed Transport Stats Legend: " + string.Join(", ", array.Select(((string Name, double Value) x) => x.Name)));
						MyLog.Default.WriteLine("Detailed Transport Stats: " + string.Join(", ", array.Select(((string Name, double Value) x) => x.Value.ToString("#0.00"))));
					}
					bool flag3 = true;
					foreach (var (text, source) in MyGameService.Peer2Peer.ClientStats)
					{
						if (flag3)
						{
							MyLog.Default.WriteLine("Client Stats Legend: Client, " + string.Join(", ", source.Select<(string, double), string>(((string Stat, double Value) x) => x.Stat)));
							flag3 = false;
						}
						MyLog.Default.WriteLine("Client Stats: " + text + ", " + string.Join(", ", source.Select<(string, double), string>(((string Stat, double Value) x) => x.Value.ToString("#0.00"))));
=======
					(string, double)[] array = Enumerable.ToArray<(string, double)>(MyGameService.Peer2Peer.Stats);
					if (array.Length != 0)
					{
						MyLog.Default.WriteLine("Detailed Transport Stats Legend: " + string.Join(", ", Enumerable.Select<(string, double), string>((IEnumerable<(string, double)>)array, (Func<(string, double), string>)(((string Name, double Value) x) => x.Name))));
						MyLog.Default.WriteLine("Detailed Transport Stats: " + string.Join(", ", Enumerable.Select<(string, double), string>((IEnumerable<(string, double)>)array, (Func<(string, double), string>)(((string Name, double Value) x) => x.Value.ToString("#0.00")))));
					}
					bool flag3 = true;
					foreach (var (text, enumerable) in MyGameService.Peer2Peer.ClientStats)
					{
						if (flag3)
						{
							MyLog.Default.WriteLine("Client Stats Legend: Client, " + string.Join(", ", Enumerable.Select<(string, double), string>(enumerable, (Func<(string, double), string>)(((string Stat, double Value) x) => x.Stat))));
							flag3 = false;
						}
						MyLog.Default.WriteLine("Client Stats: " + text + ", " + string.Join(", ", Enumerable.Select<(string, double), string>(enumerable, (Func<(string, double), string>)(((string Stat, double Value) x) => x.Value.ToString("#0.00")))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					string detailedStats = MyGameService.Peer2Peer.DetailedStats;
					if (!string.IsNullOrWhiteSpace(detailedStats))
					{
						MyLog.Default.WriteLine("Detailed Transport Stats (Human Readable):\n" + detailedStats);
					}
					int queuedBytes = MyNetworkWriter.QueuedBytes;
					MyLog.Default.WriteLine($"Pending bytes in network writer: {queuedBytes}");
				}
			}
			MyPacketStatistics myPacketStatistics = default(MyPacketStatistics);
			if (Sync.IsServer)
			{
				ServerReceivedPerSecond = ReceivedPerSecond;
				ServerSentPerSecond = SentPerSecond;
			}
			else if (Sync.MultiplayerActive)
			{
				myPacketStatistics = MyMultiplayer.Static.ReplicationLayer.ClearServerStatistics();
				if (myPacketStatistics.TimeInterval > 0f)
				{
					m_serverReceived.Enqueue(myPacketStatistics.IncomingData);
					m_serverSent.Enqueue(myPacketStatistics.OutgoingData);
					m_serverTimeIntervals.Enqueue(myPacketStatistics.TimeInterval);
					ServerReceivedPerSecond = (float)(m_serverReceived.Sum / m_serverTimeIntervals.Sum);
					ServerSentPerSecond = (float)(m_serverSent.Sum / m_serverTimeIntervals.Sum);
					PlayoutDelayBufferSize = myPacketStatistics.PlayoutDelayBufferSize;
					ServerGCMemory = myPacketStatistics.GCMemory;
					ServerProcessMemory = myPacketStatistics.ProcessMemory;
				}
			}
			if (Sandbox.Engine.Platform.Game.IsDedicated || !MyStatsGraph.Started)
			{
				return;
			}
<<<<<<< HEAD
			MyStatsGraph.Begin("Client Traffic Avg", int.MaxValue, "Update", 340, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Outgoing avg", SentPerSecond / 1024f, "{0} kB/s", 0f, "", "Update", 341, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Incoming avg", ReceivedPerSecond / 1024f, "{0} kB/s", 0f, "", "Update", 342, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End((SentPerSecond + ReceivedPerSecond) / 1024f, 0f, "", "{0} kB/s", null, 0, "Update", 343, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Server Traffic Avg", int.MaxValue, "Update", 344, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Outgoing avg", ServerSentPerSecond / 1024f, "{0} kB/s", 0f, "", "Update", 345, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Incoming avg", ServerReceivedPerSecond / 1024f, "{0} kB/s", 0f, "", "Update", 346, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End((ServerSentPerSecond + ServerReceivedPerSecond) / 1024f, 0f, "", "{0} kB/s", null, 0, "Update", 347, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Client Perf Avg", int.MaxValue, "Update", 349, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", MySandboxGame.Static.CPULoadSmooth, "{0}%", 0f, "", "Update", 350, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", MySandboxGame.Static.ThreadLoadSmooth, "{0}%", 0f, "", "Update", 351, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render CPU", MyRenderProxy.CPULoadSmooth, "{0}%", 0f, "", "Update", 352, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render GPU", MyRenderProxy.GPULoadSmooth, "{0}%", 0f, "", "Update", 353, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render Frame", MyFpsManager.FrameTimeAvg, "{0}ms", 0f, "", "Update", 354, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(MySandboxGame.Static.CPULoadSmooth, 0f, null, "{0}%", null, 0, "Update", 355, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Server Perf Avg", int.MaxValue, "Update", 356, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", Sync.ServerCPULoadSmooth, "{0}%", 0f, "", "Update", 357, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", Sync.ServerThreadLoadSmooth, "{0}%", 0f, "", "Update", 358, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(Sync.ServerCPULoadSmooth, 0f, null, "{0}%", null, 0, "Update", 359, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			if (MySession.Static != null)
			{
				MyStatsGraph.Begin("World", int.MaxValue, "Update", 364, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("PCUBuilt", MySession.Static.SessionBlockLimits.PCUBuilt, "{0}", 0f, "", "Update", 365, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("PCU", MySession.Static.SessionBlockLimits.PCU, "{0}", 0f, "", "Update", 366, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("NPC PCUBuilt", MySession.Static.NPCBlockLimits.PCUBuilt, "{0}", 0f, "", "Update", 367, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("NPC PCU", MySession.Static.NPCBlockLimits.PCU, "{0}", 0f, "", "Update", 368, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Session PCU", MySession.Static.TotalSessionPCU, "{0}", 0f, "", "Update", 369, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("GridsCount", GridsCount, "{0}", 0f, "", "Update", 370, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyReplicationClient myReplicationClient;
				if (!Sync.IsServer && (myReplicationClient = MyMultiplayer.Static.ReplicationLayer as MyReplicationClient) != null)
				{
					MyStatsGraph.CustomTime("Sync Distance", myReplicationClient.ReplicationRange ?? ((float)MySession.Static.Settings.SyncDistance), "{0}", 0f, "", "Update", 373, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				}
				MyStatsGraph.End(null, 0f, "", "{0} B", null, 0, "Update", 375, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			}
			MyStatsGraph.Begin("Memory", int.MaxValue, "Update", 378, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Overview", int.MaxValue, "Update", 382, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			float customTime2 = num2 - allocated - (float)Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.GetTotalMemory() / 1024f / 1024f;
			MyStatsGraph.CustomTime("Untracked", customTime2, "{0} MB", 0f, "", "Update", 385, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Collections", int.MaxValue, "Update", 388, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			for (int j = 0; j < m_collectionsThisFrame.Length; j++)
			{
				MyStatsGraph.CustomTime("Gen" + j, m_collectionsThisFrame[j], "{0}", 0f, "", "Update", 391, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			}
			MyStatsGraph.End(null, 0f, "", "{0} B", null, 0, "Update", 393, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("GC Used", used, "{0} MB", 0f, "", "Update", 395, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("GC Allocated", allocated, "{0} MB", 0f, "", "Update", 396, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Client Process", num2, "{0} MB", 0f, "", "Update", 397, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Remaining game memory", customTime, "{0} MB", 0f, "", "Update", 398, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Server GC", ServerGCMemory, "{0} MB", 0f, "", "Update", 399, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Server Process", ServerProcessMemory, "{0} MB", 0f, "", "Update", 400, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(null, 0f, "", "{0} MB", null, 0, "Update", 404, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(null, 0f, "", "{0} B", null, 0, "Update", 410, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
=======
			MyStatsGraph.Begin("Client Traffic Avg", int.MaxValue, "Update", 342, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Outgoing avg", SentPerSecond / 1024f, "{0} kB/s", 0f, "", "Update", 343, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Incoming avg", ReceivedPerSecond / 1024f, "{0} kB/s", 0f, "", "Update", 344, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End((SentPerSecond + ReceivedPerSecond) / 1024f, 0f, "", "{0} kB/s", null, 0, "Update", 345, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Server Traffic Avg", int.MaxValue, "Update", 346, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Outgoing avg", ServerSentPerSecond / 1024f, "{0} kB/s", 0f, "", "Update", 347, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Incoming avg", ServerReceivedPerSecond / 1024f, "{0} kB/s", 0f, "", "Update", 348, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End((ServerSentPerSecond + ServerReceivedPerSecond) / 1024f, 0f, "", "{0} kB/s", null, 0, "Update", 349, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Client Perf Avg", int.MaxValue, "Update", 351, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", MySandboxGame.Static.CPULoadSmooth, "{0}%", 0f, "", "Update", 352, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", MySandboxGame.Static.ThreadLoadSmooth, "{0}%", 0f, "", "Update", 353, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render CPU", MyRenderProxy.CPULoadSmooth, "{0}%", 0f, "", "Update", 354, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render GPU", MyRenderProxy.GPULoadSmooth, "{0}%", 0f, "", "Update", 355, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render Frame", MyFpsManager.FrameTimeAvg, "{0}ms", 0f, "", "Update", 356, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(MySandboxGame.Static.CPULoadSmooth, 0f, null, "{0}%", null, 0, "Update", 357, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Server Perf Avg", int.MaxValue, "Update", 358, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", Sync.ServerCPULoadSmooth, "{0}%", 0f, "", "Update", 359, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", Sync.ServerThreadLoadSmooth, "{0}%", 0f, "", "Update", 360, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(Sync.ServerCPULoadSmooth, 0f, null, "{0}%", null, 0, "Update", 361, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			if (MySession.Static != null)
			{
				MyStatsGraph.Begin("World", int.MaxValue, "Update", 366, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("PCUBuilt", MySession.Static.SessionBlockLimits.PCUBuilt, "{0}", 0f, "", "Update", 367, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("PCU", MySession.Static.SessionBlockLimits.PCU, "{0}", 0f, "", "Update", 368, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("NPC PCUBuilt", MySession.Static.NPCBlockLimits.PCUBuilt, "{0}", 0f, "", "Update", 369, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("NPC PCU", MySession.Static.NPCBlockLimits.PCU, "{0}", 0f, "", "Update", 370, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Session PCU", MySession.Static.TotalSessionPCU, "{0}", 0f, "", "Update", 371, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("GridsCount", GridsCount, "{0}", 0f, "", "Update", 372, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyReplicationClient myReplicationClient;
				if (!Sync.IsServer && (myReplicationClient = MyMultiplayer.Static.ReplicationLayer as MyReplicationClient) != null)
				{
					MyStatsGraph.CustomTime("Sync Distance", myReplicationClient.ReplicationRange ?? ((float)MySession.Static.Settings.SyncDistance), "{0}", 0f, "", "Update", 375, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				}
				MyStatsGraph.End(null, 0f, "", "{0} B", null, 0, "Update", 377, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			}
			MyStatsGraph.Begin("Memory", int.MaxValue, "Update", 380, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Overview", int.MaxValue, "Update", 384, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			float customTime2 = num2 - allocated - (float)Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.GetTotalMemory() / 1024f / 1024f;
			MyStatsGraph.CustomTime("Untracked", customTime2, "{0} MB", 0f, "", "Update", 387, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Collections", int.MaxValue, "Update", 390, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			for (int j = 0; j < m_collectionsThisFrame.Length; j++)
			{
				MyStatsGraph.CustomTime("Gen" + j, m_collectionsThisFrame[j], "{0}", 0f, "", "Update", 393, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			}
			MyStatsGraph.End(null, 0f, "", "{0} B", null, 0, "Update", 395, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("GC Used", used, "{0} MB", 0f, "", "Update", 397, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("GC Allocated", allocated, "{0} MB", 0f, "", "Update", 398, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Client Process", num2, "{0} MB", 0f, "", "Update", 399, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Remaining game memory", customTime, "{0} MB", 0f, "", "Update", 400, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Server GC", ServerGCMemory, "{0} MB", 0f, "", "Update", 401, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Server Process", ServerProcessMemory, "{0} MB", 0f, "", "Update", 402, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(null, 0f, "", "{0} MB", null, 0, "Update", 406, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MemoryLogger logger = default(MemoryLogger);
			Singleton<MyMemoryTracker>.Instance.LogMemoryStats(ref logger);
			MyStatsGraph.End(null, 0f, "", "{0} B", null, 0, "Update", 412, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (Sync.MultiplayerActive)
			{
				MyPacketStatistics myPacketStatistics2 = MyMultiplayer.Static.ReplicationLayer.ClearClientStatistics();
				int num5 = myPacketStatistics2.Drops + myPacketStatistics2.OutOfOrder + myPacketStatistics2.Duplicates + myPacketStatistics.PendingPackets + myPacketStatistics.Drops + myPacketStatistics.OutOfOrder + myPacketStatistics.Duplicates;
<<<<<<< HEAD
				MyStatsGraph.Begin("Packet errors", int.MaxValue, "Update", 417, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Client Drops", myPacketStatistics2.Drops, "{0}", 0f, "", "Update", 418, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Client OutOfOrder", myPacketStatistics2.OutOfOrder, "{0}", 0f, "", "Update", 419, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Client Duplicates", myPacketStatistics2.Duplicates, "{0}", 0f, "", "Update", 420, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Client Tamperred", myPacketStatistics2.Tamperred, "{0}", 0f, "", "Update", 421, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server Pending Packets", (int)myPacketStatistics.PendingPackets, "{0}", 0f, "", "Update", 422, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server Drops", myPacketStatistics.Drops, "{0}", 0f, "", "Update", 423, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server OutOfOrder", myPacketStatistics.OutOfOrder, "{0}", 0f, "", "Update", 424, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server Duplicates", myPacketStatistics.Duplicates, "{0}", 0f, "", "Update", 425, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server Tamperred", myPacketStatistics2.Tamperred, "{0}", 0f, "", "Update", 426, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.End(num5, 0f, null, "{0}", null, 0, "Update", 427, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
=======
				MyStatsGraph.Begin("Packet errors", int.MaxValue, "Update", 419, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Client Drops", myPacketStatistics2.Drops, "{0}", 0f, "", "Update", 420, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Client OutOfOrder", myPacketStatistics2.OutOfOrder, "{0}", 0f, "", "Update", 421, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Client Duplicates", myPacketStatistics2.Duplicates, "{0}", 0f, "", "Update", 422, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Client Tamperred", myPacketStatistics2.Tamperred, "{0}", 0f, "", "Update", 423, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server Pending Packets", (int)myPacketStatistics.PendingPackets, "{0}", 0f, "", "Update", 424, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server Drops", myPacketStatistics.Drops, "{0}", 0f, "", "Update", 425, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server OutOfOrder", myPacketStatistics.OutOfOrder, "{0}", 0f, "", "Update", 426, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server Duplicates", myPacketStatistics.Duplicates, "{0}", 0f, "", "Update", 427, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server Tamperred", myPacketStatistics2.Tamperred, "{0}", 0f, "", "Update", 428, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.End(num5, 0f, null, "{0}", null, 0, "Update", 429, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				LowNetworkQuality = num5 > 5;
			}
			else
			{
				LowNetworkQuality = false;
			}
			if (MySession.Static != null)
			{
<<<<<<< HEAD
				MyStatsGraph.Begin("Physics", int.MaxValue, "Update", 437, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Clusters", MyPhysics.Clusters.GetClusters().Count, "{0}", 0f, "", "Update", 439, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("VoxelBodies", MyVoxelPhysicsBody.ActiveVoxelPhysicsBodies, "{0}", 0f, "", "Update", 440, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("LargeVoxelBodies", MyVoxelPhysicsBody.ActiveVoxelPhysicsBodiesWithExtendedCache, "{0}", 0f, "", "Update", 441, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.End(0f, 0f, null, "{0}", null, 0, "Update", 443, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			}
			MyStatsGraph.ProfileAdvanced(begin: true);
			MyStatsGraph.Begin("Traffic", int.MaxValue, "Update", 448, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Outgoing", Sent / 1024f, "{0} kB", 0f, "", "Update", 449, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Incoming", Received / 1024f, "{0} kB", 0f, "", "Update", 450, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End((SentPerSecond + ReceivedPerSecond) / 1024f, 0f, "", "{0} kB", null, 0, "Update", 451, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Server Perf Avg", int.MaxValue, "Update", 453, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", Sync.ServerCPULoadSmooth, "{0}%", 0f, "", "Update", 454, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", Sync.ServerThreadLoadSmooth, "{0}%", 0f, "", "Update", 455, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(0f, 0f, null, "{0}", null, 0, "Update", 456, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Client Performance", int.MaxValue, "Update", 458, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", MySandboxGame.Static.CPULoad, "{0}%", 0f, "", "Update", 459, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", MySandboxGame.Static.ThreadLoad, "{0}%", 0f, "", "Update", 460, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render CPU", MyRenderProxy.CPULoad, "{0}%", 0f, "", "Update", 461, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render GPU", MyRenderProxy.GPULoad, "{0}%", 0f, "", "Update", 462, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(0f, 0f, null, "{0}", null, 0, "Update", 463, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Server Performance", int.MaxValue, "Update", 465, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", Sync.ServerCPULoad, "{0}%", 0f, "", "Update", 466, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", Sync.ServerThreadLoad, "{0}%", 0f, "", "Update", 467, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(0f, 0f, null, "{0}", null, 0, "Update", 468, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
=======
				MyStatsGraph.Begin("Physics", int.MaxValue, "Update", 439, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Clusters", MyPhysics.Clusters.GetClusters().Count, "{0}", 0f, "", "Update", 441, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("VoxelBodies", MyVoxelPhysicsBody.ActiveVoxelPhysicsBodies, "{0}", 0f, "", "Update", 442, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("LargeVoxelBodies", MyVoxelPhysicsBody.ActiveVoxelPhysicsBodiesWithExtendedCache, "{0}", 0f, "", "Update", 443, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.End(0f, 0f, null, "{0}", null, 0, "Update", 445, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			}
			MyStatsGraph.ProfileAdvanced(begin: true);
			MyStatsGraph.Begin("Traffic", int.MaxValue, "Update", 450, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Outgoing", Sent / 1024f, "{0} kB", 0f, "", "Update", 451, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Incoming", Received / 1024f, "{0} kB", 0f, "", "Update", 452, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End((SentPerSecond + ReceivedPerSecond) / 1024f, 0f, "", "{0} kB", null, 0, "Update", 453, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Server Perf Avg", int.MaxValue, "Update", 455, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", Sync.ServerCPULoadSmooth, "{0}%", 0f, "", "Update", 456, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", Sync.ServerThreadLoadSmooth, "{0}%", 0f, "", "Update", 457, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(0f, 0f, null, "{0}", null, 0, "Update", 458, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Client Performance", int.MaxValue, "Update", 460, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", MySandboxGame.Static.CPULoad, "{0}%", 0f, "", "Update", 461, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", MySandboxGame.Static.ThreadLoad, "{0}%", 0f, "", "Update", 462, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render CPU", MyRenderProxy.CPULoad, "{0}%", 0f, "", "Update", 463, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render GPU", MyRenderProxy.GPULoad, "{0}%", 0f, "", "Update", 464, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(0f, 0f, null, "{0}", null, 0, "Update", 465, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Server Performance", int.MaxValue, "Update", 467, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", Sync.ServerCPULoad, "{0}%", 0f, "", "Update", 468, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", Sync.ServerThreadLoad, "{0}%", 0f, "", "Update", 469, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(0f, 0f, null, "{0}", null, 0, "Update", 470, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyMultiplayer.Static?.ReplicationLayer?.ReportEvents();
			MyStatsGraph.ProfileAdvanced(begin: false);
		}

		private void AggregateStatistics()
		{
			m_statAggregators["$$data_received"].Aggregator.Push(ReceivedPerSecond);
			m_statAggregators["$$data_sent"].Aggregator.Push(SentPerSecond);
			if (Ping != 0L)
			{
				m_statAggregators["$$ping"].Aggregator.Push(Ping);
			}
			if (MyGameService.Peer2Peer == null)
			{
				return;
			}
			foreach (var stat in MyGameService.Peer2Peer.Stats)
			{
				if (m_statAggregators.TryGetValue(stat.Name, out var value))
				{
					value.Item1.Push(stat.Value);
				}
			}
			foreach (var clientStat in MyGameService.Peer2Peer.ClientStats)
			{
				foreach (var item in clientStat.Stats)
				{
					if (m_statAggregators.TryGetValue(item.Stat, out var value2))
					{
						if (!value2.Item3 && item.Value > 0.0)
						{
							value2.Item1.Push(item.Value);
						}
						break;
					}
				}
			}
		}

		public void LoadData()
		{
			m_gridsCount = 0;
			MyEntities.OnEntityCreate += OnEntityCreate;
			MyEntities.OnEntityDelete += OnEntityDelete;
		}

		private void OnEntityCreate(MyEntity entity)
		{
			if (entity is MyCubeGrid)
			{
				Interlocked.Increment(ref m_gridsCount);
			}
		}

		private void OnEntityDelete(MyEntity entity)
		{
			if (entity is MyCubeGrid)
			{
				Interlocked.Decrement(ref m_gridsCount);
			}
		}

		public static void Clear()
		{
			MyNetworkWriter.GetAndClearStats();
			MyNetworkReader.GetAndClearStats(out var _, out var _);
			foreach (KeyValuePair<string, (MyValueAggregator, string, bool)> statAggregator in Static.m_statAggregators)
			{
				statAggregator.Value.Item1.Clear();
			}
		}

		public static void ToggleProfiler()
		{
			MyRenderProfiler.EnableAutoscale(MyStatsGraph.PROFILER_NAME);
			MyRenderProfiler.ToggleProfiler(MyStatsGraph.PROFILER_NAME);
		}
	}
}
