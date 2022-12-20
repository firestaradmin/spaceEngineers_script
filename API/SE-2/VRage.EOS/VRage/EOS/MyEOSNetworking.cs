using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;
using Epic.OnlineServices.Logging;
using Epic.OnlineServices.Platform;
using ParallelTasks;
using VRage.Collections;
using VRage.GameServices;
using VRage.Library.Collections.Comparers;
using VRage.Library.Memory;
using VRage.Utils;

namespace VRage.EOS
{
	internal class MyEOSNetworking : IMyNetworking, IDisposable
	{
		private readonly struct TimedCallback
		{
			private readonly Action m_action;

			public TimedCallback(Action action)
			{
				m_action = action;
			}

			public void Invoke()
			{
				m_action();
			}
		}

		public MyEOSUser Users;

		public PlatformInterface Platform;

		public MyEOSPeer2Peer EOSPeer2Peer;

		public readonly string[] Parameters;

		public readonly bool VerboseLogging;

		private bool m_initialized;

		private Dictionary<IntPtr, long> m_wrapperDictionary = new Dictionary<IntPtr, long>(IntPtrComparer.Instance);

		private long m_wrapperAllocationSize;

		private int m_frameCounter;

		private MyMemorySystem m_EOSMemory;

		private MyMemorySystem m_EOSWrapperMemory;

		private MyMemorySystem.AllocationRecord m_EOSAllocations;

		private MyMemorySystem.AllocationRecord m_EOSWrapperAllocations;

		private readonly ConcurrentQueue<Action> m_updateThreadInvocationQueue = new ConcurrentQueue<Action>();

		private readonly ConcurrentQueue<Action> m_networkThreadInvocationQueue = new ConcurrentQueue<Action>();

		private readonly Queue<Action> m_nextFrameNetworkThreadCallbacks = new Queue<Action>();

		private readonly MyBinaryStructHeap<TimeSpan, TimedCallback> m_networkThreadTimedInvocationQueue = new MyBinaryStructHeap<TimeSpan, TimedCallback>();

		private readonly Stopwatch m_timer = Stopwatch.StartNew();

		public const string CONNECT_STRING_PREFIX = "eos://";

		public IMyGameService Service { get; }

		public string ServiceName => "EOS";

		public string ProductName { get; }

		public IMyPeer2Peer Peer2Peer => EOSPeer2Peer;

		public IMyNetworkingChat Chat { get; }

		public IMyNetworkingInvite Invite { get; }

		public MyEOSGameServer EOSGameServer { get; private set; }

		public IMyEOSPlatform EOSPlatform { get; }

		public string EncryptionUrl { get; }

		internal Thread NetworkThread { get; private set; }

		public MyEOSNetworking(bool isDedicated, string productName, IMyGameService service, string clientId, string clientSecret, string productId, string sandboxId, string deploymentId, string encryptionUrl, IMyEOSPlatform platform, bool verboseLogging, IEnumerable<string> parameters, MyServerDiscoveryAggregator serverDiscoveryAggregator, byte[] channels)
		{
			MyEOSNetworking myEOSNetworking = this;
			Parameters = parameters.ToArray();
			EOSPlatform = platform;
			Chat = new SimpleNetworkingChat(service, 128);
			Invite = platform;
			Service = service;
			ProductName = productName;
			EncryptionUrl = encryptionUrl;
			VerboseLogging = verboseLogging;
			Service.OnUpdate += Update;
			Service.OnUpdateNetworkThread += UpdateNetworkThread;
			InvokeOnNetworkThread(delegate
			{
				myEOSNetworking.Init(isDedicated, clientId, clientSecret, productId, sandboxId, deploymentId, verboseLogging, serverDiscoveryAggregator, channels);
			});
		}

		private void WrapperDealloc(IntPtr target)
		{
			Marshal.FreeHGlobal(target);
			lock (m_wrapperDictionary)
			{
				if (m_wrapperDictionary.ContainsKey(target))
				{
					m_wrapperAllocationSize -= m_wrapperDictionary[target];
					m_wrapperDictionary.Remove(target);
				}
			}
		}

		private IntPtr WrapperAlloc(int length, IntPtr target)
		{
			target = Marshal.AllocHGlobal(length);
			lock (m_wrapperDictionary)
			{
				m_wrapperAllocationSize += length;
				m_wrapperDictionary.Add(target, length);
				return target;
			}
		}

		private void Init(bool isDedicated, string clientId, string clientSecret, string productId, string sandboxId, string deploymentId, bool verboseLogging, MyServerDiscoveryAggregator serverDiscoveryAggregator, byte[] channels)
		{
			NetworkThread = Thread.CurrentThread;
			Log("EOS Init Started");
			if (EOSPlatform.Initialize(ProductName) != 0)
			{
				throw new Exception("Failed to initialize platform");
			}
			if (verboseLogging)
			{
				Log("Verbose network logging enabled.");
			}
			LoggingInterface.SetLogLevel(LogCategory.AllCategories, verboseLogging ? LogLevel.VeryVerbose : LogLevel.Error);
			LoggingInterface.SetCallback(delegate(LogMessage message)
			{
				string msg = "EOS|" + message.Category + ": " + message.Message;
				if (!(message.Category == "Debugging") || (message.Level != LogLevel.Error && message.Level != LogLevel.Fatal))
				{
					MyLog.Default.WriteLine(msg);
				}
			});
			Options options = new Options
			{
				ClientCredentials = new ClientCredentials
				{
					ClientId = clientId,
					ClientSecret = clientSecret
				},
				ProductId = productId,
				SandboxId = sandboxId,
				DeploymentId = deploymentId,
				IsServer = false
			};
			Platform = PlatformInterface.Create(options);
			if (Platform == null)
			{
				throw new Exception("Failed to create platform");
			}
			Users = new MyEOSUser(this, isDedicated);
			EOSPeer2Peer = new MyEOSPeer2Peer(this, channels);
			if (isDedicated)
			{
				EOSGameServer = new MyEOSGameServer(this);
				MyServiceManager.Instance.AddService((IMyGameServer)EOSGameServer);
			}
			else
			{
				MyEOSServerDiscovery serverDiscovery = new MyEOSServerDiscovery(this);
				if (serverDiscoveryAggregator != null)
				{
					InvokeOnMainThread(delegate
					{
						serverDiscoveryAggregator.AddAggregate(serverDiscovery);
					});
				}
				else
				{
					MyServiceManager.Instance.AddService((IMyServerDiscovery)serverDiscovery);
				}
			}
			Log("EOS Init Done");
			m_initialized = true;
		}

		public void Update()
		{
			Action result;
			while (m_updateThreadInvocationQueue.TryDequeue(out result))
			{
				result();
			}
			lock (m_nextFrameNetworkThreadCallbacks)
			{
				Action result2;
				while (QueueExtensions.TryDequeue(m_nextFrameNetworkThreadCallbacks, out result2))
				{
					m_networkThreadInvocationQueue.Enqueue(result2);
				}
			}
			if (m_frameCounter++ % 100 == 0)
			{
				if (m_EOSMemory == null)
				{
					m_EOSMemory = Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("EpicOnlineServices");
				}
				else
				{
					m_EOSAllocations.Dispose();
				}
				m_EOSAllocations = m_EOSMemory.RegisterAllocation("Native", EOSPlatform.AllocatedMemory);
				if (m_EOSWrapperMemory == null)
				{
					m_EOSWrapperMemory = Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("EpicOnlineServicesWrapper");
				}
				else
				{
					m_EOSWrapperAllocations.Dispose();
				}
				m_EOSWrapperAllocations = m_EOSWrapperMemory.RegisterAllocation("EOSWrap", m_wrapperAllocationSize);
			}
		}

		public void UpdateNetworkThread(bool sessionEnabled)
		{
			do
			{
				DispatchCallbacksNetworkThread();
				if (m_initialized)
				{
					EOSGameServer?.Update();
					Platform.Tick();
					if (sessionEnabled)
					{
						EOSPeer2Peer.ProcessQueues();
					}
				}
			}
			while (!m_networkThreadInvocationQueue.IsEmpty);
		}

		public void Error(string message)
		{
			Log(message, error: true);
		}

		public void Log(string message, bool error = false)
		{
			if (VerboseLogging || error)
			{
				if (error)
				{
					MyLog.Default.WriteLineAndConsole("EOS Networking: Error: " + message);
				}
				else
				{
					MyLog.Default.WriteLine("EOS Networking: " + message);
				}
			}
		}

		public void Dispose()
		{
			EOSPeer2Peer?.Dispose();
			PlatformInterface.Shutdown();
			Service.OnUpdate -= Update;
			Service.OnUpdateNetworkThread -= UpdateNetworkThread;
		}

		public void InvokeOnMainThread(Action action)
		{
			m_updateThreadInvocationQueue.Enqueue(action);
		}

		public void InvokeAfterMainThread(Action action)
		{
			lock (m_nextFrameNetworkThreadCallbacks)
			{
				m_nextFrameNetworkThreadCallbacks.Enqueue(action);
			}
		}

		public void InvokeOnNetworkThread(Action action)
		{
			m_networkThreadInvocationQueue.Enqueue(action);
		}

		public void InvokeOnNetworkThread(Action action, TimeSpan delay)
		{
			lock (m_networkThreadTimedInvocationQueue)
			{
				m_networkThreadTimedInvocationQueue.Insert(new TimedCallback(action), m_timer.Elapsed + delay);
			}
		}

		private void DispatchCallbacksNetworkThread()
		{
			Action result;
			while (m_networkThreadInvocationQueue.TryDequeue(out result))
			{
				result();
			}
			MyBinaryStructHeap<TimeSpan, TimedCallback> networkThreadTimedInvocationQueue = m_networkThreadTimedInvocationQueue;
			while (true)
			{
				TimedCallback timedCallback;
				lock (networkThreadTimedInvocationQueue)
				{
					if (networkThreadTimedInvocationQueue.Count == 0 || networkThreadTimedInvocationQueue.MinKey() > m_timer.Elapsed)
					{
						return;
					}
					timedCallback = networkThreadTimedInvocationQueue.RemoveMin();
				}
				timedCallback.Invoke();
			}
		}

		public void ApplySearchFilter(LobbySearch lobbySearch, MySessionSearchFilter filter)
		{
			lobbySearch.SetParameter("PRODUCT_NAME", ComparisonOp.Equal, ProductName);
			foreach (MySessionSearchFilter.Query query2 in filter.Queries)
			{
				MySessionSearchFilter.Query query = query2;
				switch (query.Property)
				{
				case "SERVER_PROP_NAMES":
					if (ValidateQueryType(in query, MySearchConditionFlags.Contains))
					{
						lobbySearch.SetParameter("SERVER_STRINGS", ComparisonOp.Contains, query.Value);
					}
					continue;
				case "SERVER_PROP_PLAYER_COUNT":
				{
					if (ValidateQueryType(in query, MySearchConditionFlags.GreaterOrEqual | MySearchConditionFlags.LesserOrEqual) && int.TryParse(query.Value, out var result))
					{
						lobbySearch.SetParameter("PLAYER_COUNT", ToEos(query.Condition), result);
					}
					continue;
				}
				case "SERVER_PROP_TAGS":
					if (ValidateQueryType(in query, MySearchConditionFlags.Equal | MySearchConditionFlags.Contains))
					{
						lobbySearch.SetParameter("TAGS", ToEos(query.Condition), query.Value);
					}
					continue;
				case "SERVER_PROP_DATA":
					if (ValidateQueryType(in query, MySearchConditionFlags.Equal | MySearchConditionFlags.Contains))
					{
						lobbySearch.SetParameter("GAME_DATA", ToEos(query.Condition), query.Value);
					}
					continue;
				}
				if (query.Property.StartsWith("SERVER_CPROP_"))
				{
					ValidateQueryType(in query, MySearchConditionFlags.Equal | MySearchConditionFlags.Contains);
					string key = "RULE_" + query.Property.Substring("SERVER_CPROP_".Length);
					lobbySearch.SetParameter(key, ToEos(query.Condition), query.Value);
				}
			}
		}

		private bool ValidateQueryType(in MySessionSearchFilter.Query query, MySearchConditionFlags validConditions)
		{
			if (!validConditions.Contains(query.Condition))
			{
				return false;
			}
			return true;
		}

		private static ComparisonOp ToEos(MySearchCondition condition)
		{
			switch (condition)
			{
			case MySearchCondition.Equal:
				return ComparisonOp.Equal;
			case MySearchCondition.Contains:
				return ComparisonOp.Contains;
			case MySearchCondition.GreaterOrEqual:
				return ComparisonOp.Greaterthanorequal;
			case MySearchCondition.LesserOrEqual:
				return ComparisonOp.Lessthanorequal;
			default:
				throw new ArgumentOutOfRangeException("condition", condition, null);
			}
		}

		public static ProductUserId PuidFromPrefixedString(string connectionString)
		{
			return ProductUserId.FromString(ParseConnectionString(connectionString));
		}

		public static string ParseConnectionString(string connectionString)
		{
			int num = connectionString.IndexOf("://", StringComparison.Ordinal);
			if (num != -1)
			{
				int num2 = num + 3;
				if (connectionString.Substring(0, num2) != "eos://")
				{
					return null;
				}
				connectionString = connectionString.Substring(num2);
			}
			return connectionString;
		}

		public void SearchForLobby(string connectionString, Action<LobbyDetails, LobbyDetailsInfo> onResult)
		{
			ProductUserId targetHost = PuidFromPrefixedString(connectionString);
			if (!targetHost.IsValid())
			{
				onResult(null, null);
				return;
			}
			Platform.GetLobbyInterface().CreateLobbySearch(new CreateLobbySearchOptions
			{
				MaxResults = 5u
			}, out var searchHandle);
			searchHandle.SetParameter(new LobbySearchSetParameterOptions
			{
				ComparisonOp = ComparisonOp.Equal,
				Parameter = new AttributeData
				{
					Key = "mincurrentmembers",
					Value = 1L
				}
			});
			searchHandle.SetParameter(new LobbySearchSetParameterOptions
			{
				ComparisonOp = ComparisonOp.Equal,
				Parameter = new AttributeData
				{
					Key = "OWNER_EOS_ID",
					Value = new AttributeDataValue
					{
						AsUtf8 = targetHost.GetIdString()
					}
				}
			});
			searchHandle.Find(new LobbySearchFindOptions
			{
				LocalUserId = Users.ProductUserId
			}, null, delegate
			{
				uint searchResultCount = searchHandle.GetSearchResultCount(new LobbySearchGetSearchResultCountOptions());
				for (int i = 0; i < searchResultCount; i++)
				{
					if (searchHandle.CopySearchResultByIndex(new LobbySearchCopySearchResultByIndexOptions
					{
						LobbyIndex = (uint)i
					}, out var outLobbyDetailsHandle) != 0)
					{
						outLobbyDetailsHandle.Release();
					}
					else
					{
						outLobbyDetailsHandle.CopyInfo(new LobbyDetailsCopyInfoOptions(), out var outLobbyDetailsInfo);
						if (outLobbyDetailsInfo.LobbyOwnerUserId == null || !outLobbyDetailsInfo.LobbyOwnerUserId.IsValid())
						{
							outLobbyDetailsHandle.Release();
						}
						else if (!(outLobbyDetailsInfo.LobbyOwnerUserId.GetIdString() != targetHost.GetIdString()))
						{
							searchHandle.Release();
							onResult(outLobbyDetailsHandle, outLobbyDetailsInfo);
							return;
						}
					}
				}
				searchHandle.Release();
				onResult(null, null);
			});
		}
	}
}
