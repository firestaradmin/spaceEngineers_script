using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.Chat;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.Replication.History;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.Models;
using VRage.Library.Debugging;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("VRage", "Network")]
	[StaticEventOwner]
	internal class MyGuiScreenDebugNetwork : MyGuiScreenDebugBase
	{
		private class IntArithmetic : MyTimedStatWindow.IStatArithmetic<int>
		{
<<<<<<< HEAD
			/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void Add(in int lhs, in int rhs, out int result)
			{
				result = lhs + rhs;
			}

<<<<<<< HEAD
			/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void Subtract(in int lhs, in int rhs, out int result)
			{
				result = lhs - rhs;
			}

			void MyTimedStatWindow.IStatArithmetic<int>.Add(in int lhs, in int rhs, out int result)
			{
				Add(in lhs, in rhs, out result);
			}

			void MyTimedStatWindow.IStatArithmetic<int>.Subtract(in int lhs, in int rhs, out int result)
			{
				Subtract(in lhs, in rhs, out result);
			}
		}

		[Serializable]
		private struct Layer
		{
			[Serializable]
			public struct Entity
			{
				protected class Sandbox_Game_Gui_MyGuiScreenDebugNetwork_003C_003ELayer_003C_003EEntity_003C_003EId_003C_003EAccessor : IMemberAccessor<Entity, long>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref Entity owner, in long value)
					{
						owner.Id = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref Entity owner, out long value)
					{
						value = owner.Id;
					}
				}

				protected class Sandbox_Game_Gui_MyGuiScreenDebugNetwork_003C_003ELayer_003C_003EEntity_003C_003EBounds_003C_003EAccessor : IMemberAccessor<Entity, BoundingBoxD?>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref Entity owner, in BoundingBoxD? value)
					{
						owner.Bounds = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref Entity owner, out BoundingBoxD? value)
					{
						value = owner.Bounds;
					}
				}

				public long Id;

				public BoundingBoxD? Bounds;

				public Entity(long id, BoundingBoxD? bounds)
				{
					Id = id;
					Bounds = bounds;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenDebugNetwork_003C_003ELayer_003C_003EBounds_003C_003EAccessor : IMemberAccessor<Layer, BoundingBox>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Layer owner, in BoundingBox value)
				{
					owner.Bounds = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Layer owner, out BoundingBox value)
				{
					value = owner.Bounds;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenDebugNetwork_003C_003ELayer_003C_003EEntities_003C_003EAccessor : IMemberAccessor<Layer, List<Entity>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Layer owner, in List<Entity> value)
				{
					owner.Entities = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Layer owner, out List<Entity> value)
				{
					value = owner.Entities;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenDebugNetwork_003C_003ELayer_003C_003EPCU_003C_003EAccessor : IMemberAccessor<Layer, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Layer owner, in int value)
				{
					owner.PCU = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Layer owner, out int value)
				{
					value = owner.PCU;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenDebugNetwork_003C_003ELayer_003C_003EEnabled_003C_003EAccessor : IMemberAccessor<Layer, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Layer owner, in bool value)
				{
					owner.Enabled = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Layer owner, out bool value)
				{
					value = owner.Enabled;
				}
			}

			public BoundingBox Bounds;

			public List<Entity> Entities;

			public int PCU;

			public bool Enabled;

			public Layer(BoundingBox bounds, List<Entity> entities, int pcu, bool enabled)
			{
				Bounds = bounds;
				Entities = entities;
				PCU = pcu;
				Enabled = enabled;
			}
		}

		protected sealed class OnSnapshotsMechanicalPivotsChange_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool state, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnSnapshotsMechanicalPivotsChange(state);
			}
		}

		protected sealed class OnWorldSnapshotsChange_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool state, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnWorldSnapshotsChange(state);
			}
		}

		protected sealed class SendDataServer_003C_003ESystem_Byte_003C_0023_003E : ICallSite<IMyEventOwner, byte[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in byte[] data, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SendDataServer(data);
			}
		}
<<<<<<< HEAD
=======

		protected sealed class SendDataClient_003C_003ESystem_Byte_003C_0023_003E : ICallSite<IMyEventOwner, byte[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in byte[] data, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SendDataClient(data);
			}
		}

		protected sealed class RequestLayersFromServer_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestLayersFromServer();
			}
		}

		protected sealed class ReceiveLayersFromServer_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Gui_MyGuiScreenDebugNetwork_003C_003ELayer_003E : ICallSite<IMyEventOwner, List<Layer>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<Layer> layers, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ReceiveLayersFromServer(layers);
			}
		}

		private MyGuiControlLabel m_entityLabel;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		protected sealed class SendDataClient_003C_003ESystem_Byte_003C_0023_003E : ICallSite<IMyEventOwner, byte[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in byte[] data, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SendDataClient(data);
			}
		}

		protected sealed class RequestLayersFromServer_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestLayersFromServer();
			}
		}

		protected sealed class ReceiveLayersFromServer_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Gui_MyGuiScreenDebugNetwork_003C_003ELayer_003E : ICallSite<IMyEventOwner, List<Layer>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<Layer> layers, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ReceiveLayersFromServer(layers);
			}
		}

		private MyGuiControlLabel m_entityLabel;

		private MyEntity m_currentEntity;

		private MyGuiControlButton m_kickButton;

		private MyGuiControlLabel m_profileLabel;

		private bool m_profileEntityLocked;

		private const float FORCED_PRIORITY = 1f;

		private readonly MyPredictedSnapshotSyncSetup m_kickSetup = new MyPredictedSnapshotSyncSetup
		{
			AllowForceStop = false,
			ApplyPhysicsAngular = false,
			ApplyPhysicsLinear = false,
			ApplyRotation = false,
			ApplyPosition = true,
			ExtrapolationSmoothing = true
		};

		private MyGuiControlLabel m_dataRateLabel;

		private Thread m_debugTransferThread;

		private bool m_debugDataTransfer;

		private bool m_sendInParallel = true;

		private volatile int m_debugTransferRate = 10000;

		private static byte[] m_message;

		private readonly MyTimedStatWindow<int> m_bytesPerSecond = new MyTimedStatWindow<int>(TimeSpan.FromSeconds(5.0), new IntArithmetic());

		public static bool DebugDrawSpatialReplicationLayers;

		private List<Layer> m_layers;

		public MyGuiScreenDebugNetwork()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 1f;
			AddCaption("Network", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			if (MyMultiplayer.Static != null)
			{
				AddSlider("Priority multiplier", 1f, 0f, 16f, delegate(MyGuiControlSlider slider)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyMultiplayerBase.OnSetPriorityMultiplier, slider.Value);
				});
				m_currentPosition.Y += 0.01f;
				AddCheckBox("Smooth ping", MyMultiplayer.Static.ReplicationLayer.UseSmoothPing, delegate(MyGuiControlCheckbox x)
				{
					MyMultiplayer.Static.ReplicationLayer.UseSmoothPing = x.IsChecked;
				});
				AddSlider("Ping smooth factor", MyMultiplayer.Static.ReplicationLayer.PingSmoothFactor, 0f, 3f, delegate(MyGuiControlSlider slider)
				{
					MyMultiplayer.Static.ReplicationLayer.PingSmoothFactor = slider.Value;
				});
				AddSlider("Timestamp correction minimum", MyMultiplayer.Static.ReplicationLayer.TimestampCorrectionMinimum, 0f, 100f, delegate(MyGuiControlSlider slider)
				{
					MyMultiplayer.Static.ReplicationLayer.TimestampCorrectionMinimum = (int)slider.Value;
				});
				AddCheckBox("Smooth timestamp correction", MyMultiplayer.Static.ReplicationLayer.UseSmoothCorrection, delegate(MyGuiControlCheckbox x)
				{
					MyMultiplayer.Static.ReplicationLayer.UseSmoothCorrection = x.IsChecked;
				});
				AddSlider("Smooth timestamp correction amplitude", MyMultiplayer.Static.ReplicationLayer.SmoothCorrectionAmplitude, 0f, 5f, delegate(MyGuiControlSlider slider)
				{
					MyMultiplayer.Static.ReplicationLayer.SmoothCorrectionAmplitude = (int)slider.Value;
				});
			}
			AddCheckBox("Physics World Locking", MyFakes.WORLD_LOCKING_IN_CLIENTUPDATE, delegate(MyGuiControlCheckbox x)
			{
				MyFakes.WORLD_LOCKING_IN_CLIENTUPDATE = x.IsChecked;
			});
			AddCheckBox("Pause physics", null, MemberHelper.GetMember(() => MyFakes.PAUSE_PHYSICS));
			AddCheckBox("Client physics constraints", null, MemberHelper.GetMember(() => MyFakes.MULTIPLAYER_CLIENT_CONSTRAINTS));
			AddCheckBox("New timing", MyReplicationClient.SynchronizationTimingType == MyReplicationClient.TimingType.LastServerTime, delegate(MyGuiControlCheckbox x)
			{
				MyReplicationClient.SynchronizationTimingType = ((!x.IsChecked) ? MyReplicationClient.TimingType.ServerTimestep : MyReplicationClient.TimingType.LastServerTime);
			});
			AddSlider("Animation time shift [ms]", (float)MyAnimatedSnapshotSync.TimeShift.Milliseconds, 0f, 1000f, delegate(MyGuiControlSlider slider)
			{
				MyAnimatedSnapshotSync.TimeShift = MyTimeSpan.FromMilliseconds(slider.Value);
			});
			AddCheckBox("Prediction in jetpack", null, MemberHelper.GetMember(() => MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER_IN_JETPACK));
			AddCheckBox("Prediction for grids", null, MemberHelper.GetMember(() => MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_GRID));
			AddCheckBox("Skip prediction", null, MemberHelper.GetMember(() => MyFakes.MULTIPLAYER_SKIP_PREDICTION));
			AddCheckBox("Skip prediction subgrids", null, MemberHelper.GetMember(() => MyFakes.MULTIPLAYER_SKIP_PREDICTION_SUBGRIDS));
			AddCheckBox("Extrapolation smoothing", null, MemberHelper.GetMember(() => MyFakes.MULTIPLAYER_EXTRAPOLATION_SMOOTHING));
			AddCheckBox("Skip animation", null, MemberHelper.GetMember(() => MyFakes.MULTIPLAYER_SKIP_ANIMATION));
			AddCheckBox("SnapshotCache Hierarchy Propagation", null, MemberHelper.GetMember(() => MyFakes.SNAPSHOTCACHE_HIERARCHY));
			AddCheckBox("Force Playout Delay Buffer", null, MemberHelper.GetMember(() => MyFakes.ForcePlayoutDelayBuffer));
			AddCheckBox("World snapshots", MyFakes.WORLD_SNAPSHOTS, delegate(MyGuiControlCheckbox x)
			{
				MyFakes.WORLD_SNAPSHOTS = x.IsChecked;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner y) => OnWorldSnapshotsChange, x.IsChecked);
			});
			AddCheckBox("Mechanical Pivots in Snapshots", MyFakes.SNAPSHOTS_MECHANICAL_PIVOTS, delegate(MyGuiControlCheckbox x)
			{
				MyFakes.SNAPSHOTS_MECHANICAL_PIVOTS = x.IsChecked;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner y) => OnSnapshotsMechanicalPivotsChange, x.IsChecked);
			});
			AddCheckBox("Draw Spatial Layers", null, MemberHelper.GetMember(() => DebugDrawSpatialReplicationLayers));
			AddCheckBox("Enable Debug Data Transfer", checkedState: false, delegate(MyGuiControlCheckbox cb)
			{
				m_debugDataTransfer = cb.IsChecked;
				WakeDebugSender();
			});
			AddCheckBox("Send Debug Data in Worker", checkedState: true, delegate(MyGuiControlCheckbox cb)
			{
				m_sendInParallel = cb.IsChecked;
				WakeDebugSender();
			});
			MyGuiControlSlider myGuiControlSlider = AddSlider("Debug Data Transfer", 10000f, 5000000f, () => m_debugTransferRate, delegate(float x)
			{
				m_debugTransferRate = (int)x;
			});
			myGuiControlSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(myGuiControlSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate
			{
				WakeDebugSender();
			});
			m_dataRateLabel = AddLabel("No Debug Data Transfer", Color.White, 1f);
<<<<<<< HEAD
		}

		[ChatCommand("/nml", "", "", MyPromoteLevel.Admin)]
		private static void ChatCommandSetNetworkMonitorTimeout(string[] args)
		{
			if (args != null && args.Length == 1 && int.TryParse(args[0], out var result))
			{
				MyNetworkMonitor.UpdateLatency = result;
			}
		}

		[Event(null, 146)]
=======
		}

		[ChatCommand("/nml", "", "", MyPromoteLevel.Admin)]
		private static void ChatCommandSetNetworkMonitorTimeout(string[] args)
		{
			if (args != null && args.Length == 1 && int.TryParse(args[0], out var result))
			{
				MyNetworkMonitor.UpdateLatency = result;
			}
		}

		[Event(null, 149)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void OnSnapshotsMechanicalPivotsChange(bool state)
		{
			MyFakes.SNAPSHOTS_MECHANICAL_PIVOTS = state;
		}

<<<<<<< HEAD
		[Event(null, 152)]
=======
		[Event(null, 155)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void OnWorldSnapshotsChange(bool state)
		{
			MyFakes.WORLD_SNAPSHOTS = state;
		}

		public override bool Update(bool hasFocus)
		{
			bool result = base.Update(hasFocus);
			if (m_kickButton == null || m_entityLabel == null)
			{
				return result;
			}
			if (MySession.Static != null)
			{
				MyEntity myEntity = null;
				if (MySession.Static != null)
				{
					LineD line = new LineD(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * 500.0);
					MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, MySession.Static.LocalCharacter, null, ignoreChildren: false, ignoreFloatingObjects: true, ignoreHandWeapons: true, IntersectionFlags.ALL_TRIANGLES, 0f, ignoreObjectsWithoutPhysics: false);
					if (intersectionWithLine.HasValue)
					{
						myEntity = intersectionWithLine.Value.Entity as MyEntity;
					}
				}
				if (myEntity != m_currentEntity && !m_profileEntityLocked)
				{
					m_currentEntity = myEntity;
					m_kickButton.Enabled = m_currentEntity != null;
					m_entityLabel.Text = ((m_currentEntity != null) ? m_currentEntity.DisplayName : "");
					m_profileLabel.Text = m_entityLabel.Text;
					MySnapshotCache.DEBUG_ENTITY_ID = ((m_currentEntity != null) ? m_currentEntity.EntityId : 0);
					MyFakes.VDB_ENTITY = m_currentEntity;
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyMultiplayerBase.OnSetDebugEntity, (m_currentEntity == null) ? 0 : m_currentEntity.EntityId);
				}
			}
			return result;
		}

<<<<<<< HEAD
		/// <inheritdoc />
		public override bool Draw()
		{
=======
		public override bool Draw()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			DebugDraw();
			if (m_debugDataTransfer && MyScreenManager.GetFirstScreenOfType<MyGuiScreenGamePlay>() != null)
			{
				if (m_sendInParallel)
				{
					if (m_debugTransferThread == null)
					{
<<<<<<< HEAD
						m_debugTransferThread = new Thread(SendDataLoop)
						{
							Name = "Debug Data Transfer",
							IsBackground = true
						};
=======
						Thread val = new Thread((ThreadStart)SendDataLoop);
						val.set_Name("Debug Data Transfer");
						val.set_IsBackground(true);
						m_debugTransferThread = val;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						m_debugTransferThread.Start();
					}
				}
				else
				{
					SendPart(0.0166666675f);
				}
				lock (m_bytesPerSecond)
				{
<<<<<<< HEAD
					MyGuiControlLabel dataRateLabel = m_dataRateLabel;
					double num = m_bytesPerSecond.Total;
					TimeSpan maxTime = m_bytesPerSecond.MaxTime;
					dataRateLabel.Text = $"{num / maxTime.TotalSeconds} B/s";
=======
					m_dataRateLabel.Text = $"{(double)m_bytesPerSecond.Total / m_bytesPerSecond.MaxTime.TotalSeconds} B/s";
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			else
			{
				m_dataRateLabel.Text = "No Debug Data Transfer";
			}
			return base.Draw();
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		protected override void OnClosed()
		{
			base.OnClosed();
			m_debugTransferRate = -1;
<<<<<<< HEAD
			m_debugTransferThread?.Join();
=======
			Thread debugTransferThread = m_debugTransferThread;
			if (debugTransferThread != null)
			{
				debugTransferThread.Join();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_debugTransferThread = null;
		}

		private void SendDataLoop()
		{
			while (m_debugTransferRate >= 0)
			{
				if (!m_debugDataTransfer || !m_sendInParallel || !Sync.MultiplayerActive)
				{
					lock (m_debugTransferThread)
					{
						Monitor.Wait(m_debugTransferThread);
					}
					continue;
				}
				float factor = (float)MyNetworkMonitor.UpdateLatency / 1000f;
				SendPart(factor);
				lock (m_debugTransferThread)
				{
					Monitor.Wait(m_debugTransferThread, TimeSpan.FromMilliseconds(MyNetworkMonitor.UpdateLatency));
				}
			}
		}

		private void SendPart(float factor)
		{
			int num = (int)((float)m_debugTransferRate * factor);
			byte[] message = m_message;
			if (message == null || message.Length != num)
			{
				m_message = new byte[num];
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SendDataServer, m_message);
			lock (m_bytesPerSecond)
			{
				m_bytesPerSecond.Current += m_message.Length;
				m_bytesPerSecond.Advance();
			}
		}

		private void WakeDebugSender()
		{
			if (m_debugTransferThread != null && m_debugDataTransfer)
			{
				lock (m_debugTransferThread)
				{
					Monitor.Pulse(m_debugTransferThread);
				}
			}
		}

<<<<<<< HEAD
		[Event(null, 289)]
=======
		[Event(null, 292)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void SendDataServer(byte[] data)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SendDataClient, data, MyEventContext.Current.Sender);
		}

<<<<<<< HEAD
		[Event(null, 295)]
=======
		[Event(null, 298)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void SendDataClient(byte[] data)
		{
		}

		private void DebugDraw()
		{
			if (!DebugDrawSpatialReplicationLayers)
			{
				return;
			}
			if (!Sync.IsServer && MyScreenManager.GetFirstScreenOfType<MyGuiScreenGamePlay>() != null)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestLayersFromServer);
			}
			if (m_layers == null)
			{
				return;
			}
			for (int i = 0; i < m_layers.Count; i++)
			{
				Layer layer = m_layers[i];
				Color c = MyClipmap.LodColors[i];
				float num = (layer.Enabled ? 1f : 0.3f);
				c = c.Alpha(num);
				MyRenderProxy.DebugDrawAABB(layer.Bounds, c, num);
				foreach (Layer.Entity entity in layer.Entities)
				{
					if (entity.Bounds.HasValue)
					{
						Color color = c;
						if (!MyEntities.EntityExists(entity.Id))
						{
							color = c.Alpha(0.3f);
						}
						BoundingBoxD value = entity.Bounds.Value;
						MyRenderProxy.DebugDrawAABB(value, color, (float)(int)color.A / 255f);
						Vector3D center = value.Center;
						long id = entity.Id;
						MyRenderProxy.DebugDrawText3D(center, id.ToString(), color, 0.5f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					}
				}
				string text = string.Format("[{0}] {1} PCU {2}", i, layer.PCU, layer.Enabled ? "Enabled" : "Disabled");
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 200 + i * 17), text, c.Alpha(1f), 0.7f);
			}
		}

<<<<<<< HEAD
		[Event(null, 393)]
=======
		[Event(null, 396)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void RequestLayersFromServer()
		{
			List<Layer> list = new List<Layer>();
			GetLayerData(MyEventContext.Current.Sender, list);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ReceiveLayersFromServer, list, MyEventContext.Current.Sender);
		}

<<<<<<< HEAD
		[Event(null, 402)]
=======
		[Event(null, 405)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void ReceiveLayersFromServer(List<Layer> layers)
		{
			MyGuiScreenDebugNetwork firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenDebugNetwork>();
			if (firstScreenOfType != null)
			{
				firstScreenOfType.m_layers = layers;
			}
		}

		private static void GetLayerData(EndpointId endpoint, List<Layer> layerList)
		{
			foreach (var layerDatum in ((MyReplicationServer)MyMultiplayer.Static.ReplicationLayer).GetLayerData(endpoint))
			{
<<<<<<< HEAD
				layerList.Add(new Layer((BoundingBox)layerDatum.Bounds, (from x in layerDatum.Replicables.OfType<IMyEntityReplicable>()
					where !(x is MyVoxelReplicable)
					select x).Select(GetEntity).ToList(), layerDatum.PCU, layerDatum.Enabled));
			}
			Layer.Entity GetEntity(IMyEntityReplicable r)
=======
				layerList.Add(new Layer((BoundingBox)layerDatum.Bounds, Enumerable.ToList<Layer.Entity>(Enumerable.Select<IMyEntityReplicable, Layer.Entity>(Enumerable.Where<IMyEntityReplicable>(Enumerable.OfType<IMyEntityReplicable>((IEnumerable)layerDatum.Replicables), (Func<IMyEntityReplicable, bool>)((IMyEntityReplicable x) => !(x is MyVoxelReplicable))), (Func<IMyEntityReplicable, Layer.Entity>)GetEntity)), layerDatum.PCU, layerDatum.Enabled));
			}
			static Layer.Entity GetEntity(IMyEntityReplicable r)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				BoundingBoxD aABB = ((MyExternalReplicable)r).GetAABB();
				return new Layer.Entity(r.EntityId, aABB);
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugNetwork";
		}
	}
}
