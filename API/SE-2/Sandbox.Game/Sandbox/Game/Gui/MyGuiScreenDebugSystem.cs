using System;
using System.Text;
using Havok;
using Sandbox.Engine;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	[MyDebugScreen("VRage", "System")]
	internal class MyGuiScreenDebugSystem : MyGuiScreenDebugBase
	{
		private enum SimulationQualityOptions
		{
			UseWorldSetting,
			Normal,
			Simplified
		}

		private enum VSTSimulationQualityOptions
		{
			Normal,
			Low,
			VeryLow,
			PlatformDefault
		}

		protected sealed class HavokMemoryStatsRequest_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				HavokMemoryStatsRequest();
			}
		}

		protected sealed class HavokMemoryStatsReply_003C_003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string stats, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				HavokMemoryStatsReply(stats);
			}
		}

		private MyGuiControlMultilineText m_havokStatsMultiline;

		private static StringBuilder m_buffer = new StringBuilder();

		private static string m_statsFromServer = string.Empty;

		public MyGuiScreenDebugSystem()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("System debug", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddLabel("System", Color.Yellow.ToVector4(), 1.2f);
			AddCheckBox("Simulate slow update", null, MemberHelper.GetMember(() => MyFakes.SIMULATE_SLOW_UPDATE));
			AddButton(new StringBuilder("Force GC"), OnClick_ForceGC);
			AddCheckBox("Pause physics", null, MemberHelper.GetMember(() => MyFakes.PAUSE_PHYSICS));
			AddCheckBox("Performance logging", null, MemberHelper.GetMember(() => MyFakes.ENABLE_PERFORMANCELOGGING));
			AddButton(new StringBuilder("Step physics"), delegate
			{
				MyFakes.STEP_PHYSICS = true;
			});
			AddSlider("Simulation speed", 0.001f, 3f, null, MemberHelper.GetMember(() => MyFakes.SIMULATION_SPEED));
			AddSlider("Statistics Logging Frequency [s]", (float)MyGeneralStats.Static.LogInterval.Seconds, 0f, 120f, delegate(MyGuiControlSlider slider)
			{
				MyGeneralStats.Static.LogInterval = MyTimeSpan.FromSeconds(slider.Value);
			});
			if (MySession.Static != null && MySession.Static.Settings != null)
			{
				AddCheckBox("Enable save", MySession.Static.Settings, MemberHelper.GetMember(() => MySession.Static.Settings.EnableSaving));
			}
			AddCheckBox("Enable scenario settings edit", null, MemberHelper.GetMember(() => MyGuiScreenLoadSandbox.ENABLE_SCENARIO_EDIT));
			AddCheckBox("Optimize grid update", null, MemberHelper.GetMember(() => MyFakes.OPTIMIZE_GRID_UPDATES));
			AddCheckBox("Enable Parallel Entity Update", () => !MyParallelEntityUpdateOrchestrator.ForceSerialUpdate, delegate(bool x)
			{
				MyParallelEntityUpdateOrchestrator.ForceSerialUpdate = !x;
			});
			AddCheckBox("UGC Test Environment", MyPlatformGameSettings.UGC_TEST_ENVIRONMENT, delegate(MyGuiControlCheckbox c)
			{
				MyPlatformGameSettings.UGC_TEST_ENVIRONMENT = c.IsChecked;
				MyGameService.WorkshopService.SetTestEnvironment(MyPlatformGameSettings.UGC_TEST_ENVIRONMENT);
			});
			AddCheckBox("GameService tracing", checkedState: false, delegate(MyGuiControlCheckbox c)
			{
				MyGameService.Trace(c.IsChecked);
			});
			AddButton(new StringBuilder("Clear achievements and stats"), delegate
			{
				MyGameService.ResetAllStats(achievementsToo: true);
				MyGameService.StoreStats();
			});
			AddLabel("Simplified simulation", Color.Yellow.ToVector4(), 1f);
			SimulationQualityOptions selectedItem = SimulationQualityOptions.UseWorldSetting;
			if (MyPlatformGameSettings.SIMPLIFIED_SIMULATION_OVERRIDE.HasValue)
			{
				selectedItem = ((!MyPlatformGameSettings.SIMPLIFIED_SIMULATION_OVERRIDE.Value) ? SimulationQualityOptions.Normal : SimulationQualityOptions.Simplified);
			}
			AddCombo(selectedItem, delegate(SimulationQualityOptions x)
			{
				switch (x)
				{
				case SimulationQualityOptions.UseWorldSetting:
					MyPlatformGameSettings.SIMPLIFIED_SIMULATION_OVERRIDE = null;
					break;
				case SimulationQualityOptions.Normal:
					MyPlatformGameSettings.SIMPLIFIED_SIMULATION_OVERRIDE = false;
					break;
				case SimulationQualityOptions.Simplified:
					MyPlatformGameSettings.SIMPLIFIED_SIMULATION_OVERRIDE = true;
					break;
				}
			});
			AddLabel("VST simulation quality", Color.Yellow.ToVector4(), 1f);
			int selectedItem2 = (int)(MyPlatformGameSettings.VST_SIMULATION_QUALITY_OVERRIDE ?? ((SimulationQuality)3));
			AddCombo((VSTSimulationQualityOptions)selectedItem2, delegate(VSTSimulationQualityOptions x)
			{
				if (x == VSTSimulationQualityOptions.PlatformDefault)
				{
					MyPlatformGameSettings.VST_SIMULATION_QUALITY_OVERRIDE = null;
				}
				else
				{
					MyPlatformGameSettings.VST_SIMULATION_QUALITY_OVERRIDE = (SimulationQuality)x;
				}
			});
			m_currentPosition.Y += 0.01f;
			m_havokStatsMultiline = AddMultilineText(null, null, 0.8f);
		}

		public override bool Draw()
		{
			m_havokStatsMultiline.Clear();
			m_havokStatsMultiline.AppendText(GetHavokMemoryStats());
			return base.Draw();
		}

		private static string GetHavokMemoryStats()
		{
			if (Sync.IsServer || MySession.Static == null)
			{
				m_buffer.Append("Out of mem: ").Append(HkBaseSystem.IsOutOfMemory).AppendLine();
				HkBaseSystem.GetMemoryStatistics(m_buffer);
				string result = m_buffer.ToString();
				m_buffer.Clear();
				return result;
			}
			if (MySession.Static.GameplayFrameCounter % 100 == 0)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => HavokMemoryStatsRequest);
			}
			return m_statsFromServer;
		}

		[Event(null, 163)]
		[Reliable]
		[Server]
		private static void HavokMemoryStatsRequest()
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => HavokMemoryStatsReply, GetHavokMemoryStats(), MyEventContext.Current.Sender);
		}

		[Event(null, 175)]
		[Reliable]
		[Client]
		private static void HavokMemoryStatsReply(string stats)
		{
			m_statsFromServer = stats;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugSystem";
		}

		private void OnClick_ForceGC(MyGuiControlButton button)
		{
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
		}
	}
}
