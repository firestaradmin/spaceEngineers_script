using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Profiler;
using VRage.Replication;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Network
{
	public class MyReplicationClient : MyReplicationLayer
	{
		public enum TimingType
		{
			None,
			ServerTimestep,
			LastServerTime
		}

		private const float NO_RESPONSE_ACTION_SECONDS = 5f;

		public static SerializableVector3I StressSleep = new SerializableVector3I(0, 0, 0);

		private readonly MyClientStateBase m_clientState;

		private bool m_clientReady;

		private bool m_hasTypeTable;

		private readonly IReplicationClientCallback m_callback;

		private readonly CacheList<IMyStateGroup> m_tmpGroups = new CacheList<IMyStateGroup>(4);

		private readonly List<byte> m_acks = new List<byte>();

		private bool m_receivedStreamingPackets;

		private byte m_lastStateSyncPacketId;

		private byte m_lastStreamingPacketId;

		private byte m_clientPacketId;

		private MyTimeSpan m_lastServerTimestamp = MyTimeSpan.Zero;

		private MyTimeSpan m_lastServerTimeStampReceivedTime = MyTimeSpan.Zero;

		private bool m_clientPaused;

		private readonly CachingDictionary<NetworkId, MyPendingReplicable> m_pendingReplicables = new CachingDictionary<NetworkId, MyPendingReplicable>();

		private readonly HashSet<NetworkId> m_destroyedReplicables = new HashSet<NetworkId>();

		private readonly MyEventsBuffer m_eventBuffer;

		private readonly MyEventsBuffer.Handler m_eventHandler;

		private readonly MyEventsBuffer.IsBlockedHandler m_isBlockedHandler;

		private MyTimeSpan m_clientStartTimeStamp = MyTimeSpan.Zero;

		private readonly float m_simulationTimeStep;

		private readonly ConcurrentCachingHashSet<IMyStateGroup> m_stateGroupsForUpdate = new ConcurrentCachingHashSet<IMyStateGroup>();

		private readonly Action<string> m_failureCallback;

		private const int MAX_TIMESTAMP_DIFF_LOW = 80;

		private const int MAX_TIMESTAMP_DIFF_HIGH = 500;

		private const int MAX_TIMESTAMP_DIFF_VERY_HIGH = 5000;

		private MyTimeSpan m_lastTime;

		private MyTimeSpan m_ping;

		private MyTimeSpan m_smoothPing;

		private MyTimeSpan m_lastPingTime;

		private MyTimeSpan m_correctionSmooth;

		public static TimingType SynchronizationTimingType = TimingType.None;

		private MyTimeSpan m_lastClientTime;

		private MyTimeSpan m_lastServerTime;

		private MyPacketStatistics m_serverStats;

		private readonly MyPacketTracker m_serverTracker = new MyPacketTracker();

		private MyPacketStatistics m_clientStats;

		private readonly CacheList<IMyReplicable> m_tmp = new CacheList<IMyReplicable>();

		private readonly bool m_predictionReset;

		private MyTimeSpan m_lastClientTimestamp;

		private float m_timeDiffSmoothed;

		public MyTimeSpan Timestamp { get; private set; }

		public int PendingStreamingRelicablesCount { get; private set; }

		public float? ReplicationRange => m_clientState.ReplicationRange;

		public MyTimeSpan Ping
		{
			get
			{
				if (!base.UseSmoothPing)
				{
					return m_ping;
				}
				return m_smoothPing;
			}
		}

		public float? ServerReplicationRange { get; private set; }

		public event Action<IMyReplicable> OnReplicableReady;

		public MyReplicationClient(Endpoint endpointId, IReplicationClientCallback callback, MyClientStateBase clientState, float simulationTimeStep, Action<string> failureCallback, bool predictionReset, Thread mainThread)
			: base(isNetworkAuthority: false, endpointId.Id, mainThread)
		{
			m_eventBuffer = new MyEventsBuffer(mainThread);
			m_replicables = new MyReplicablesHierarchy(mainThread);
			m_simulationTimeStep = simulationTimeStep;
			m_callback = callback;
			m_clientState = clientState;
			m_clientState.EndpointId = endpointId;
			m_eventHandler = base.OnEvent;
			m_isBlockedHandler = IsBlocked;
			m_failureCallback = failureCallback;
			m_predictionReset = predictionReset;
		}

		public override void Dispose()
		{
			m_eventBuffer.Dispose();
			base.Dispose();
		}

		protected override MyPacketDataBitStreamBase GetBitStreamPacketData()
		{
			return m_callback.GetBitStreamPacketData();
		}

		public override void Disconnect()
		{
			m_callback.DisconnectFromHost();
		}

		public void OnLocalClientReady()
		{
			m_clientReady = true;
		}

		protected override void AddNetworkObject(NetworkId networkId, IMyNetObject obj)
		{
			base.AddNetworkObject(networkId, obj);
			IMyStateGroup myStateGroup = obj as IMyStateGroup;
			if (myStateGroup != null && myStateGroup.NeedsUpdate)
			{
				m_stateGroupsForUpdate.Add(myStateGroup);
			}
		}

		protected override void RemoveNetworkedObjectInternal(NetworkId networkID, IMyNetObject obj)
		{
			base.RemoveNetworkedObjectInternal(networkID, obj);
			IMyStateGroup myStateGroup = obj as IMyStateGroup;
			if (myStateGroup != null)
			{
				m_stateGroupsForUpdate.Remove(myStateGroup);
			}
		}

		/// <summary>
		/// Marks replicable as successfully created, ready to receive events and state groups data.
		/// </summary>
		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		private void SetReplicableReady(NetworkId networkId, NetworkId parentId, IMyReplicable replicable, bool loaded)
		{
			try
			{
				if (m_pendingReplicables.TryGetValue(networkId, out var value) && value.Replicable == replicable)
				{
					MyPendingReplicable value2;
					if (loaded)
					{
						m_pendingReplicables.Remove(networkId);
						m_pendingReplicables.ApplyRemovals();
						List<NetworkId> stateGroupIds = value.StateGroupIds;
						AddNetworkObject(networkId, replicable);
						m_replicables.Add(replicable, out var _);
						using (m_tmpGroups)
						{
							replicable.GetStateGroups(m_tmpGroups);
							for (int i = 0; i < m_tmpGroups.Count; i++)
							{
								if (m_tmpGroups[i].IsStreaming)
								{
									PendingStreamingRelicablesCount--;
								}
								else
								{
									AddNetworkObject(stateGroupIds[i], m_tmpGroups[i]);
								}
							}
						}
						if (value.DependentReplicables != null)
						{
							foreach (KeyValuePair<NetworkId, MyPendingReplicable> dependentReplicable in value.DependentReplicables)
							{
								dependentReplicable.Value.Replicable.Reload(delegate(bool dependentLoaded)
								{
									SetReplicableReady(dependentReplicable.Key, networkId, dependentReplicable.Value.Replicable, dependentLoaded);
								});
							}
						}
						m_eventBuffer.ProcessEvents(networkId, m_eventHandler, m_isBlockedHandler, NetworkId.Invalid);
						MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
						bitStreamPacketData.Stream.WriteNetworkId(networkId);
						bitStreamPacketData.Stream.WriteBool(loaded);
						bitStreamPacketData.Stream.Terminate();
						m_callback.SendReplicableReady(bitStreamPacketData);
						this.OnReplicableReady?.Invoke(replicable);
					}
					else if (m_pendingReplicables.TryGetValue(parentId, out value2))
					{
						if (value2.DependentReplicables == null)
						{
							value2.DependentReplicables = new Dictionary<NetworkId, MyPendingReplicable>();
						}
						value2.DependentReplicables.Add(networkId, value);
					}
					else
					{
						ReplicableDestroy(replicable, removeNetworkObject: false);
					}
				}
				else
				{
					replicable.OnDestroyClient();
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
				throw;
			}
		}

		public void ProcessReplicationCreateBegin(MyPacket packet)
		{
			TypeId typeId = packet.BitStream.ReadTypeId();
			NetworkId networkID = packet.BitStream.ReadNetworkId();
			NetworkId parentID = packet.BitStream.ReadNetworkId();
			byte b = packet.BitStream.ReadByte();
			MyPendingReplicable myPendingReplicable = new MyPendingReplicable();
			for (int i = 0; i < b; i++)
			{
				NetworkId item = packet.BitStream.ReadNetworkId();
				myPendingReplicable.StateGroupIds.Add(item);
			}
			Type typeByTypeId = GetTypeByTypeId(typeId);
			IMyReplicable replicable = (IMyReplicable)Activator.CreateInstance(typeByTypeId);
			myPendingReplicable.Replicable = replicable;
			myPendingReplicable.ParentID = parentID;
			if (!m_pendingReplicables.ContainsKey(networkID))
			{
				m_pendingReplicables.Add(networkID, myPendingReplicable);
				m_pendingReplicables.ApplyAdditionsAndModifications();
			}
			List<NetworkId> stateGroupIds = myPendingReplicable.StateGroupIds;
			IMyStreamableReplicable myStreamableReplicable = replicable as IMyStreamableReplicable;
			myPendingReplicable.IsStreaming = true;
			myStreamableReplicable.CreateStreamingStateGroup();
			PendingStreamingRelicablesCount++;
			AddNetworkObject(stateGroupIds[0], myStreamableReplicable.GetStreamingStateGroup());
			myPendingReplicable.StreamingGroupId = stateGroupIds[0];
			myStreamableReplicable.OnLoadBegin(delegate(bool loaded)
			{
				SetReplicableReady(networkID, parentID, replicable, loaded);
			});
			packet.Return();
		}

		public void ProcessReplicationCreate(MyPacket packet)
		{
			TypeId typeId = packet.BitStream.ReadTypeId();
			NetworkId networkID = packet.BitStream.ReadNetworkId();
			NetworkId parentID = packet.BitStream.ReadNetworkId();
			byte b = packet.BitStream.ReadByte();
			if (parentID.IsValid)
			{
				if (!m_pendingReplicables.ContainsKey(parentID) && GetObjectByNetworkId(parentID) == null)
				{
					packet.Return();
					return;
				}
			}
			else
			{
				m_destroyedReplicables.Remove(networkID);
			}
			MyPendingReplicable myPendingReplicable = new MyPendingReplicable();
			myPendingReplicable.ParentID = parentID;
			for (int i = 0; i < b; i++)
			{
				NetworkId item = packet.BitStream.ReadNetworkId();
				myPendingReplicable.StateGroupIds.Add(item);
			}
			Type typeByTypeId = GetTypeByTypeId(typeId);
			IMyReplicable replicable = (IMyReplicable)Activator.CreateInstance(typeByTypeId);
			myPendingReplicable.Replicable = replicable;
			myPendingReplicable.IsStreaming = false;
			if (!m_pendingReplicables.ContainsKey(networkID))
			{
				m_pendingReplicables.Add(networkID, myPendingReplicable);
				m_pendingReplicables.ApplyAdditionsAndModifications();
			}
			replicable.OnLoad(packet.BitStream, delegate(bool loaded)
			{
				SetReplicableReady(networkID, parentID, replicable, loaded);
			});
			packet.Return();
		}

		public void ProcessReplicationDestroy(MyPacket packet)
		{
			NetworkId networkId = packet.BitStream.ReadNetworkId();
			if (!m_pendingReplicables.TryGetValue(networkId, out var value))
			{
				IMyReplicable myReplicable = GetObjectByNetworkId(networkId) as IMyReplicable;
				if (myReplicable != null)
				{
					using (m_tmp)
					{
						m_replicables.GetAllChildren(myReplicable, m_tmp);
						foreach (IMyReplicable item in m_tmp)
						{
							ReplicableDestroy(item);
						}
						ReplicableDestroy(myReplicable);
					}
				}
			}
			else
			{
				PendingReplicableDestroy(networkId, value);
				m_pendingReplicables.ApplyRemovals();
			}
			m_destroyedReplicables.Add(networkId);
			packet.Return();
		}

		private void PendingReplicableDestroy(NetworkId networkID, MyPendingReplicable pendingReplicable, bool calledByParent = false)
		{
			if (pendingReplicable.DependentReplicables != null)
			{
				foreach (KeyValuePair<NetworkId, MyPendingReplicable> dependentReplicable in pendingReplicable.DependentReplicables)
				{
					PendingReplicableDestroy(dependentReplicable.Key, dependentReplicable.Value, calledByParent: true);
				}
			}
			foreach (KeyValuePair<NetworkId, MyPendingReplicable> pendingReplicable2 in m_pendingReplicables)
			{
				if (pendingReplicable2.Value.ParentID.Equals(networkID))
				{
					PendingReplicableDestroy(pendingReplicable2.Key, pendingReplicable2.Value);
				}
			}
			if (!calledByParent && m_pendingReplicables.TryGetValue(pendingReplicable.ParentID, out var value) && value.DependentReplicables != null)
			{
				value.DependentReplicables.Remove(networkID);
			}
			using (m_tmpGroups)
			{
				pendingReplicable.Replicable.GetStateGroups(m_tmpGroups);
				foreach (IMyStateGroup tmpGroup in m_tmpGroups)
				{
					if (tmpGroup != null)
					{
						if (tmpGroup.IsStreaming)
						{
							RemoveNetworkedObject(tmpGroup);
							PendingStreamingRelicablesCount--;
						}
						tmpGroup.Destroy();
					}
				}
			}
			m_eventBuffer.RemoveEvents(networkID);
			m_pendingReplicables.Remove(networkID);
		}

		private void ReplicableDestroy(IMyReplicable replicable, bool removeNetworkObject = true)
		{
			if (TryGetNetworkIdByObject(replicable, out var networkId))
			{
				m_pendingReplicables.Remove(networkId);
				m_pendingReplicables.ApplyRemovals();
				m_eventBuffer.RemoveEvents(networkId);
			}
			using (m_tmpGroups)
			{
				replicable.GetStateGroups(m_tmpGroups);
				foreach (IMyStateGroup tmpGroup in m_tmpGroups)
				{
					if (tmpGroup != null)
					{
						if (removeNetworkObject)
						{
							RemoveNetworkedObject(tmpGroup);
						}
						tmpGroup.Destroy();
					}
				}
			}
			if (removeNetworkObject)
			{
				RemoveNetworkedObject(replicable);
			}
			replicable.OnDestroyClient();
			m_replicables.RemoveHierarchy(replicable);
		}

		public void ProcessReplicationIslandDone(MyPacket packet)
		{
			byte index = packet.BitStream.ReadByte();
			int num = packet.BitStream.ReadInt32();
			Dictionary<long, MatrixD> dictionary = new Dictionary<long, MatrixD>();
			for (int i = 0; i < num; i++)
			{
				long num2 = packet.BitStream.ReadInt64();
				Vector3D translation = packet.BitStream.ReadVector3D();
				Quaternion quaternion = packet.BitStream.ReadQuaternion();
				if (num2 != 0L)
				{
					MatrixD value = MatrixD.CreateFromQuaternion(quaternion);
					value.Translation = translation;
					dictionary.Add(num2, value);
				}
			}
			m_callback.SetIslandDone(index, dictionary);
			packet.Return();
		}

		public void OnServerData(MyPacket packet)
		{
			_ = packet.BitStream.BitPosition;
			try
			{
				SerializeTypeTable(packet.BitStream);
				m_hasTypeTable = true;
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Server sent bad data!");
				byte[] array = new byte[packet.BitStream.ByteLength];
				packet.BitStream.ReadBytes(array, 0, packet.BitStream.ByteLength);
				MyLog.Default.WriteLine("Server Data: " + string.Join(", ", array));
				MyLog.Default.WriteLine(ex);
				m_failureCallback("Failed to connect to server. See log for details.");
			}
			packet.Return();
		}

		public override MyTimeSpan GetSimulationUpdateTime()
		{
			MyTimeSpan updateTime = m_callback.GetUpdateTime();
			if (m_clientStartTimeStamp == MyTimeSpan.Zero)
			{
				m_clientStartTimeStamp = updateTime;
			}
			return updateTime - m_clientStartTimeStamp;
		}

		public override void UpdateBefore()
		{
		}

		public override void UpdateAfter()
		{
			if (m_clientReady && m_hasTypeTable && m_clientState != null)
			{
				MyStatsGraph.ProfileAdvanced(begin: true);
<<<<<<< HEAD
				MyStatsGraph.Begin("Replication client update", 0, "UpdateAfter", 494, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
				UpdatePingSmoothing();
				MyStatsGraph.CustomTime("Ping", (float)m_ping.Milliseconds, "{0} ms", 0f, "", "UpdateAfter", 497, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
				MyStatsGraph.CustomTime("SmoothPing", (float)m_smoothPing.Milliseconds, "{0} ms", 0f, "", "UpdateAfter", 498, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
=======
				MyStatsGraph.Begin("Replication client update", 0, "UpdateAfter", 494, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
				UpdatePingSmoothing();
				MyStatsGraph.CustomTime("Ping", (float)m_ping.Milliseconds, "{0} ms", 0f, "", "UpdateAfter", 497, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
				MyStatsGraph.CustomTime("SmoothPing", (float)m_smoothPing.Milliseconds, "{0} ms", 0f, "", "UpdateAfter", 498, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				switch (SynchronizationTimingType)
				{
				case TimingType.ServerTimestep:
					Timestamp = UpdateServerTimestep();
					break;
				case TimingType.LastServerTime:
					Timestamp = UpdateLastServerTime();
					break;
				}
<<<<<<< HEAD
				MyStatsGraph.End(null, 0f, "", "{0} B", null, 0, "UpdateAfter", 512, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
				MyStatsGraph.ProfileAdvanced(begin: false);
				if (StressSleep.X > 0)
				{
					int millisecondsTimeout = ((StressSleep.Z != 0) ? ((int)(Math.Sin(GetSimulationUpdateTime().Milliseconds * Math.PI / (double)StressSleep.Z) * (double)StressSleep.Y + (double)StressSleep.X)) : MyRandom.Instance.Next(StressSleep.X, StressSleep.Y));
					Thread.Sleep(millisecondsTimeout);
=======
				MyStatsGraph.End(null, 0f, "", "{0} B", null, 0, "UpdateAfter", 512, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
				MyStatsGraph.ProfileAdvanced(begin: false);
				if (StressSleep.X > 0)
				{
					int num = ((StressSleep.Z != 0) ? ((int)(Math.Sin(GetSimulationUpdateTime().Milliseconds * Math.PI / (double)StressSleep.Z) * (double)StressSleep.Y + (double)StressSleep.X)) : MyRandom.Instance.Next(StressSleep.X, StressSleep.Y));
					Thread.Sleep(num);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				if ((MyTimeSpan.FromTicks(Stopwatch.GetTimestamp()) - m_lastServerTimeStampReceivedTime).Seconds > 5.0 && !m_clientPaused)
				{
					m_clientPaused = true;
					m_callback.PauseClient(pause: true);
				}
			}
		}

		private MyTimeSpan UpdateLastServerTime()
		{
			MyTimeSpan myTimeSpan = MyTimeSpan.FromTicks(Stopwatch.GetTimestamp());
			MyTimeSpan myTimeSpan2 = myTimeSpan - m_lastClientTime;
			m_lastClientTime = myTimeSpan;
			MyTimeSpan myTimeSpan3 = myTimeSpan - m_lastServerTimeStampReceivedTime;
			MyTimeSpan myTimeSpan4 = m_lastServerTimestamp + myTimeSpan3;
			MyTimeSpan myTimeSpan5 = myTimeSpan4 - m_lastServerTime;
			m_lastServerTime = myTimeSpan4;
<<<<<<< HEAD
			MyStatsGraph.CustomTime("ClientTimeDelta", (float)myTimeSpan2.Milliseconds, "{0} ms", 0f, "", "UpdateLastServerTime", 558, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
			MyStatsGraph.CustomTime("ServerTimeDelta", (float)myTimeSpan5.Milliseconds, "{0} ms", 0f, "", "UpdateLastServerTime", 559, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
			MyStatsGraph.CustomTime("TimeDeltaFromPacket", (float)myTimeSpan3.Milliseconds, "{0} ms", 0f, "", "UpdateLastServerTime", 560, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
=======
			MyStatsGraph.CustomTime("ClientTimeDelta", (float)myTimeSpan2.Milliseconds, "{0} ms", 0f, "", "UpdateLastServerTime", 558, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
			MyStatsGraph.CustomTime("ServerTimeDelta", (float)myTimeSpan5.Milliseconds, "{0} ms", 0f, "", "UpdateLastServerTime", 559, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
			MyStatsGraph.CustomTime("TimeDeltaFromPacket", (float)myTimeSpan3.Milliseconds, "{0} ms", 0f, "", "UpdateLastServerTime", 560, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (myTimeSpan3.Seconds > 1.0)
			{
				return Timestamp;
			}
			float num = (float)(Timestamp - myTimeSpan4).Seconds;
<<<<<<< HEAD
			MyStatsGraph.CustomTime("ServerClientTimeDiff", (0f - num) * 1000f, "{0} ms", 0f, "", "UpdateLastServerTime", 572, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
=======
			MyStatsGraph.CustomTime("ServerClientTimeDiff", (0f - num) * 1000f, "{0} ms", 0f, "", "UpdateLastServerTime", 572, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			float clientSimulationRatio = m_callback.GetClientSimulationRatio();
			float serverSimulationRatio = m_callback.GetServerSimulationRatio();
			float num2 = (float)(int)(clientSimulationRatio / serverSimulationRatio * 50f) / 50f;
			if (num2 > 0.8f)
			{
				num2 = 1f;
			}
			num += 0.06f * (0.6f - num2);
			MyTimeSpan result;
			if (num < -1f)
			{
				result = ((myTimeSpan3.Seconds < 1.0) ? myTimeSpan4 : Timestamp);
				m_clientStartTimeStamp -= Timestamp - GetSimulationUpdateTime();
				return result;
			}
			if (num > 0.2f)
			{
				m_clientStartTimeStamp -= Timestamp - GetSimulationUpdateTime();
				return Timestamp;
			}
			m_timeDiffSmoothed = MathHelper.Smooth(num, m_timeDiffSmoothed);
			num = m_timeDiffSmoothed;
			float value;
			if (Math.Sign(num) > 0)
			{
				value = 1f / (float)Math.Exp(num * 6f);
			}
			else
			{
				num = (float)Math.Max((double)(0f - num) - 0.04, 0.0);
				value = (float)Math.Exp(num * 2f);
			}
			value = MathHelper.Clamp(value, 0.1f, 10f);
			float num3 = Math.Max(Math.Min(num2, 1f), 0.1f);
			float num4 = m_simulationTimeStep / num3 * value;
<<<<<<< HEAD
			MyStatsGraph.CustomTime("TimeAdvance", num4, "{0} ms", 0f, "", "UpdateLastServerTime", 632, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
			MyStatsGraph.CustomTime("ServerClientSimRatio", num3, "{0}", 0f, "", "UpdateLastServerTime", 633, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
=======
			MyStatsGraph.CustomTime("TimeAdvance", num4, "{0} ms", 0f, "", "UpdateLastServerTime", 632, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
			MyStatsGraph.CustomTime("ServerClientSimRatio", num3, "{0}", 0f, "", "UpdateLastServerTime", 633, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			result = Timestamp + MyTimeSpan.FromMilliseconds(num4);
			m_clientStartTimeStamp -= result - GetSimulationUpdateTime();
			float num5 = m_simulationTimeStep / serverSimulationRatio - m_simulationTimeStep;
			if (num5 > 0f)
			{
				m_callback.SetNextFrameDelayDelta(num5);
			}
<<<<<<< HEAD
			MyStatsGraph.CustomTime("FrameDelayTime", num5, "{0} ms", 0f, "", "UpdateLastServerTime", 648, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
=======
			MyStatsGraph.CustomTime("FrameDelayTime", num5, "{0} ms", 0f, "", "UpdateLastServerTime", 648, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return result;
		}

		private MyTimeSpan UpdateServerTimestep()
		{
			MyTimeSpan simulationUpdateTime = GetSimulationUpdateTime();
			MyTimeSpan myTimeSpan = (base.UseSmoothPing ? m_smoothPing : m_ping);
			double num = (0.0 - myTimeSpan.Milliseconds) * (double)m_callback.GetServerSimulationRatio();
			double num2 = simulationUpdateTime.Milliseconds - m_lastServerTimestamp.Milliseconds;
			double num3 = num2 + num;
			int num4 = 0;
			int num5 = 0;
			MyTimeSpan myTimeSpan2 = MyTimeSpan.FromTicks(Stopwatch.GetTimestamp());
			MyTimeSpan myTimeSpan3 = myTimeSpan2 - m_lastTime;
			num3 -= (double)m_simulationTimeStep;
			double num6 = Math.Min(myTimeSpan3.Seconds / (double)base.SmoothCorrectionAmplitude, 1.0);
			m_correctionSmooth = MyTimeSpan.FromMilliseconds(num3 * num6 + m_correctionSmooth.Milliseconds * (1.0 - num6));
			int num7 = (int)(m_simulationTimeStep * 2f / m_callback.GetServerSimulationRatio());
			num3 = Math.Min(num3, num7);
			m_correctionSmooth = MyTimeSpan.FromMilliseconds(Math.Min(m_correctionSmooth.Milliseconds, num7));
<<<<<<< HEAD
			MyStatsGraph.CustomTime("Correction", (float)num3, "{0} ms", 0f, "", "UpdateServerTimestep", 674, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
			MyStatsGraph.CustomTime("SmoothCorrection", (float)m_correctionSmooth.Milliseconds, "{0} ms", 0f, "", "UpdateServerTimestep", 675, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
=======
			MyStatsGraph.CustomTime("Correction", (float)num3, "{0} ms", 0f, "", "UpdateServerTimestep", 674, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
			MyStatsGraph.CustomTime("SmoothCorrection", (float)m_correctionSmooth.Milliseconds, "{0} ms", 0f, "", "UpdateServerTimestep", 675, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (num2 < -80.0 || (num2 > 500.0 + myTimeSpan.Milliseconds && !m_predictionReset) || num2 > 5000.0)
			{
				m_clientStartTimeStamp = MyTimeSpan.FromMilliseconds(m_clientStartTimeStamp.Milliseconds + num2);
				simulationUpdateTime = GetSimulationUpdateTime();
				m_correctionSmooth = MyTimeSpan.Zero;
				if (m_predictionReset && num2 > 5000.0)
				{
					TimestampReset();
				}
				if (!MyCompilationSymbols.EnableNetworkPositionTracking)
				{
				}
			}
			else
			{
				num4 = ((!(num2 < 0.0)) ? (base.UseSmoothCorrection ? ((int)m_correctionSmooth.Milliseconds) : ((int)num3)) : ((int)num3));
				if ((float)(base.LastMessageFromServer - DateTime.UtcNow).Seconds < 1f)
				{
					if (num2 < 0.0)
					{
						num5 = num4;
						m_callback.SetNextFrameDelayDelta(num5);
					}
					else if (Math.Abs(num4) > base.TimestampCorrectionMinimum)
					{
						num5 = (Math.Abs(num4) - base.TimestampCorrectionMinimum) * Math.Sign(num4);
						m_callback.SetNextFrameDelayDelta(num5);
					}
				}
			}
<<<<<<< HEAD
			MyStatsGraph.CustomTime("GameTimeDelta", (float)(simulationUpdateTime - Timestamp).Milliseconds, "{0} ms", 0f, "", "UpdateServerTimestep", 712, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
			MyStatsGraph.CustomTime("RealTimeDelta", (float)myTimeSpan3.Milliseconds, "{0} ms", 0f, "", "UpdateServerTimestep", 713, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
=======
			MyStatsGraph.CustomTime("GameTimeDelta", (float)(simulationUpdateTime - Timestamp).Milliseconds, "{0} ms", 0f, "", "UpdateServerTimestep", 712, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
			MyStatsGraph.CustomTime("RealTimeDelta", (float)myTimeSpan3.Milliseconds, "{0} ms", 0f, "", "UpdateServerTimestep", 713, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			string.Concat("realtime delta: ", myTimeSpan3, ", client: ", Timestamp, ", server: ", m_lastServerTimestamp, ", diff: ", num2.ToString("##.#"), " => ", (Timestamp.Milliseconds - m_lastServerTimestamp.Milliseconds).ToString("##.#"), ", Ping: ", m_ping.Milliseconds.ToString("##.#"), " / ", m_smoothPing.Milliseconds.ToString("##.#"), "ms, Correction ", num3, " / ", m_correctionSmooth.Milliseconds, " / ", num5, ", ratio ", m_callback.GetServerSimulationRatio());
			m_lastTime = myTimeSpan2;
			return simulationUpdateTime;
		}

		public override void UpdateClientStateGroups()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			m_stateGroupsForUpdate.ApplyChanges();
			Enumerator<IMyStateGroup> enumerator = m_stateGroupsForUpdate.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IMyStateGroup current = enumerator.get_Current();
					current.ClientUpdate(Timestamp);
					if (!current.NeedsUpdate)
					{
						m_stateGroupsForUpdate.Remove(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public override void Simulate()
		{
			m_callback.UpdateSnapshotCache();
		}

		private void TimestampReset()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			m_stateGroupsForUpdate.ApplyChanges();
			Enumerator<IMyStateGroup> enumerator = m_stateGroupsForUpdate.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Reset(reinit: false, Timestamp);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public override void SendUpdate()
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			bitStreamPacketData.Stream.WriteByte(m_lastStateSyncPacketId);
			bitStreamPacketData.Stream.WriteBool(m_receivedStreamingPackets);
			m_receivedStreamingPackets = false;
			bitStreamPacketData.Stream.WriteByte(m_lastStreamingPacketId);
			byte b = (byte)m_acks.Count;
			bitStreamPacketData.Stream.WriteByte(b);
			for (int i = 0; i < b; i++)
			{
				bitStreamPacketData.Stream.WriteByte(m_acks[i]);
			}
			bitStreamPacketData.Stream.Terminate();
			m_acks.Clear();
			m_callback.SendClientAcks(bitStreamPacketData);
			bitStreamPacketData = m_callback.GetBitStreamPacketData();
			m_clientPacketId++;
			bitStreamPacketData.Stream.WriteByte(m_clientPacketId);
			bitStreamPacketData.Stream.WriteDouble(Timestamp.Milliseconds);
			bitStreamPacketData.Stream.WriteDouble(MyTimeSpan.FromTicks(Stopwatch.GetTimestamp()).Milliseconds);
			_ = MyCompilationSymbols.EnableNetworkPacketTracking;
			m_clientState.Serialize(bitStreamPacketData.Stream, outOfOrder: false);
			bitStreamPacketData.Stream.Terminate();
			m_callback.SendClientUpdate(bitStreamPacketData);
		}

		protected override bool DispatchBlockingEvent(IPacketData data, CallSite site, EndpointId recipient, IMyNetObject eventInstance, Vector3D? position, IMyNetObject blockedNetObj)
		{
			return DispatchEvent(data, site, recipient, eventInstance, position);
		}

		public override void UpdateStatisticsData(int outgoing, int incoming, int tamperred, float gcMemory, float processMemory)
		{
		}

		public override MyPacketStatistics ClearClientStatistics()
		{
			MyPacketStatistics clientStats = m_clientStats;
			m_clientStats.Reset();
			return clientStats;
		}

		public override MyPacketStatistics ClearServerStatistics()
		{
			MyPacketStatistics serverStats = m_serverStats;
			m_serverStats.Reset();
			return serverStats;
		}

		protected override bool DispatchEvent(IPacketData data, CallSite site, EndpointId target, IMyNetObject instance, Vector3D? position)
		{
			if (site.HasServerFlag)
			{
				m_callback.SendEvent(data, site.IsReliable);
			}
			else
			{
				_ = site.HasClientFlag;
				data.Return();
			}
			return false;
		}

		/// <summary>
		/// Checks if network id is blocked by other network id.
		/// </summary>
		/// <param name="networkId">Target network id.</param>
		/// <param name="blockedNetId">Blocking network id.</param>
		/// <returns></returns>
		private bool IsBlocked(NetworkId networkId, NetworkId blockedNetId)
		{
			bool flag = m_pendingReplicables.ContainsKey(networkId) || m_pendingReplicables.ContainsKey(blockedNetId);
			bool flag2 = GetObjectByNetworkId(networkId) == null || (blockedNetId.IsValid && GetObjectByNetworkId(blockedNetId) == null);
			if (networkId.IsValid && (flag || flag2))
			{
				return true;
			}
			return false;
		}

		protected override void OnEvent(MyPacketDataBitStreamBase data, NetworkId networkId, NetworkId blockedNetId, uint eventId, EndpointId sender, Vector3D? position)
		{
			base.LastMessageFromServer = DateTime.UtcNow;
			bool flag = m_eventBuffer.ContainsEvents(networkId) || m_eventBuffer.ContainsEvents(blockedNetId);
			if (IsBlocked(networkId, blockedNetId) || flag)
			{
				m_eventBuffer.EnqueueEvent(data, networkId, blockedNetId, eventId, sender, position);
				if (blockedNetId.IsValid)
				{
					m_eventBuffer.EnqueueBarrier(blockedNetId, networkId);
				}
			}
			else
			{
				base.OnEvent(data, networkId, blockedNetId, eventId, sender, position);
			}
		}

		protected override void OnEvent(MyPacketDataBitStreamBase data, CallSite site, object obj, IMyNetObject sendAs, Vector3D? position, EndpointId source)
		{
			base.LastMessageFromServer = DateTime.UtcNow;
			Invoke(site, data.Stream, obj, source, null, validate: false);
			data.Return();
		}

		/// <summary>
		/// Processes state sync sent by server.
		/// </summary>
		public void OnServerStateSync(MyPacket packet)
		{
			try
			{
				base.LastMessageFromServer = DateTime.UtcNow;
				bool flag = packet.BitStream.ReadBool();
				byte b = packet.BitStream.ReadByte();
				if (!flag)
				{
					MyPacketTracker.OrderType orderType = m_serverTracker.Add(b);
					m_clientStats.Update(orderType);
					if (orderType == MyPacketTracker.OrderType.Duplicate)
					{
						return;
					}
					m_lastStateSyncPacketId = b;
					if (!m_acks.Contains(b))
					{
						m_acks.Add(b);
					}
					goto IL_007d;
				}
				m_lastStreamingPacketId = b;
				m_receivedStreamingPackets = true;
				goto IL_007d;
				IL_007d:
				MyPacketStatistics statistics = default(MyPacketStatistics);
				statistics.Read(packet.BitStream);
				m_serverStats.Add(statistics);
				MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(packet.BitStream.ReadDouble());
				if (m_lastServerTimestamp < myTimeSpan)
				{
					m_lastServerTimestamp = myTimeSpan;
					m_lastServerTimeStampReceivedTime = packet.ReceivedTime;
				}
				MyTimeSpan myTimeSpan2 = MyTimeSpan.FromMilliseconds(packet.BitStream.ReadDouble());
				if (myTimeSpan2 > m_lastClientTimestamp)
				{
					m_lastClientTimestamp = myTimeSpan2;
				}
				double num = packet.BitStream.ReadDouble();
				if (num > 0.0)
				{
					MyTimeSpan ping = packet.ReceivedTime - MyTimeSpan.FromMilliseconds(num);
					if (ping.Milliseconds < 1000.0)
					{
						SetPing(ping);
					}
				}
				MyTimeSpan serverTimestamp = myTimeSpan;
				m_callback.ReadCustomState(packet.BitStream);
				while (packet.BitStream.BytePosition + 2 < packet.BitStream.ByteLength)
				{
					if (!packet.BitStream.CheckTerminator())
					{
						MyLog.Default.WriteLine("OnServerStateSync: Invalid stream terminator");
						return;
					}
					NetworkId id = packet.BitStream.ReadNetworkId();
					IMyStateGroup myStateGroup = GetObjectByNetworkId(id) as IMyStateGroup;
					int num2 = 0;
					num2 = ((!flag) ? packet.BitStream.ReadInt16() : packet.BitStream.ReadInt32());
					if (myStateGroup == null)
					{
						packet.BitStream.SetBitPositionRead(packet.BitStream.BitPosition + num2);
						continue;
					}
					if (flag != myStateGroup.IsStreaming)
					{
						MyLog.Default.WriteLine("received streaming flag but group is not streaming !");
						packet.BitStream.SetBitPositionRead(packet.BitStream.BitPosition + num2);
						continue;
					}
					int bytePosition = packet.BitStream.BytePosition;
					_ = packet.BitStream.BitPosition;
					if (MyCompilationSymbols.EnableNetworkPacketTracking)
					{
						_ = packet.BitStream.BitPosition;
						myStateGroup.Owner.ToString();
						_ = myStateGroup.GetType().FullName;
					}
<<<<<<< HEAD
					MyStatsGraph.Begin(myStateGroup.GetType().Name, int.MaxValue, "OnServerStateSync", 1027, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
					myStateGroup.Serialize(packet.BitStream, default(MyClientInfo), serverTimestamp, m_lastClientTimestamp, b, 0, null);
					MyStatsGraph.End(packet.BitStream.ByteLength - bytePosition, 0f, "", "{0} B", null, 0, "OnServerStateSync", 1029, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
=======
					MyStatsGraph.Begin(myStateGroup.GetType().Name, int.MaxValue, "OnServerStateSync", 1027, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
					myStateGroup.Serialize(packet.BitStream, default(MyClientInfo), serverTimestamp, m_lastClientTimestamp, b, 0, null);
					MyStatsGraph.End(packet.BitStream.ByteLength - bytePosition, 0f, "", "{0} B", null, 0, "OnServerStateSync", 1029, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				if (!packet.BitStream.CheckTerminator())
				{
					MyLog.Default.WriteLine("OnServerStateSync: Invalid stream terminator");
				}
				if (m_clientPaused)
				{
					m_clientPaused = false;
					m_callback.PauseClient(pause: false);
				}
			}
			finally
			{
				packet.Return();
			}
		}

		private void SetPing(MyTimeSpan ping)
		{
			m_ping = ping;
			UpdatePingSmoothing();
			m_callback.SetPing((long)m_smoothPing.Milliseconds);
			SetClientStatePing((short)m_smoothPing.Milliseconds);
		}

		private void UpdatePingSmoothing()
		{
			MyTimeSpan myTimeSpan = MyTimeSpan.FromTicks(Stopwatch.GetTimestamp());
			double num = Math.Min((myTimeSpan - m_lastPingTime).Seconds / (double)PingSmoothFactor, 1.0);
			m_smoothPing = MyTimeSpan.FromMilliseconds(m_ping.Milliseconds * num + m_smoothPing.Milliseconds * (1.0 - num));
			m_lastPingTime = myTimeSpan;
		}

		public JoinResultMsg OnJoinResult(MyPacket packet)
		{
			return MySerializer.CreateAndRead<JoinResultMsg>(packet.BitStream);
		}

		public ServerDataMsg OnWorldData(MyPacket packet)
		{
			return MySerializer.CreateAndRead<ServerDataMsg>(packet.BitStream);
		}

		public ChatMsg OnChatMessage(MyPacket packet)
		{
			return MySerializer.CreateAndRead<ChatMsg>(packet.BitStream);
		}

		public ConnectedClientDataMsg OnClientConnected(MyPacket packet)
		{
			return MySerializer.CreateAndRead<ConnectedClientDataMsg>(packet.BitStream);
		}

		public void SendClientConnected(ref ConnectedClientDataMsg msg)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			MySerializer.Write(bitStreamPacketData.Stream, ref msg);
			m_callback.SendConnectRequest(bitStreamPacketData);
		}

		public void SendClientReady(ref ClientReadyDataMsg msg)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			MySerializer.Write(bitStreamPacketData.Stream, ref msg);
			m_callback.SendClientReady(bitStreamPacketData);
			OnLocalClientReady();
		}

		public void AddToUpdates(IMyStateGroup group)
		{
			m_stateGroupsForUpdate.Add(group);
		}

		public void RequestReplicable(long entityId, byte layer, bool add)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			bitStreamPacketData.Stream.WriteInt64(entityId);
			bitStreamPacketData.Stream.WriteBool(add);
			if (add)
			{
				bitStreamPacketData.Stream.WriteByte(layer);
			}
			m_callback.SendReplicableRequest(bitStreamPacketData);
		}

		public void SetClientStatePing(short ping)
		{
			m_clientState.Ping = ping;
		}

		public void SetClientReplicationRange(float? range)
		{
			m_clientState.ReplicationRange = range;
		}

		public override string GetMultiplayerStat()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string multiplayerStat = base.GetMultiplayerStat();
			stringBuilder.Append(multiplayerStat);
			stringBuilder.AppendLine("Pending Replicables:");
			foreach (KeyValuePair<NetworkId, MyPendingReplicable> pendingReplicable in m_pendingReplicables)
			{
				string value = "   NetworkId: " + pendingReplicable.Key.ToString() + ", IsStreaming: " + pendingReplicable.Value.IsStreaming;
				stringBuilder.AppendLine(value);
			}
			stringBuilder.Append(m_eventBuffer.GetEventsBufferStat());
			return stringBuilder.ToString();
		}
	}
}
