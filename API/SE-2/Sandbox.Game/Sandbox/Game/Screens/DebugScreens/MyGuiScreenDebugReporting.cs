<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Linq;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("VRage", "Reporting")]
	internal class MyGuiScreenDebugReporting : MyGuiScreenDebugBase
	{
		private MyGuiControlLabel m_cnt;

		private static int m_nextProfilerDumpIndex;

		private int m_recordCounter;

		public MyGuiScreenDebugReporting()
		{
			m_nextProfilerDumpIndex = MyObjectBuilder_ProfilerSnapshot.GetProfilerDumpCount();
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugReporting";
		}

		public override void RecreateControls(bool constructor)
		{
			m_scale = 0.7f;
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.13f);
			AddCaption("Reporting", Color.Yellow.ToVector4());
			AddButton("Send Logs", SendLog);
			AddLabel("Profiler", Color.Yellow, 1f);
			m_cnt = AddLabel($"Dump Count: {m_nextProfilerDumpIndex}", Color.Yellow, 1f);
			AddButton("Record Profiler Dump", RecordProfiler);
			AddButton("Clear Profiler Dumps", CleanDumps);
			AddButton("Send Logs With Profiler", SendLogWithProfilerData);
		}

		private void CleanDumps(MyGuiControlButton obj)
		{
			MyObjectBuilder_ProfilerSnapshot.ClearProfilerDumps();
			m_nextProfilerDumpIndex = MyObjectBuilder_ProfilerSnapshot.GetProfilerDumpCount();
			m_cnt.Text = $"Dump Count: {m_nextProfilerDumpIndex}";
		}

		private void RecordProfiler(MyGuiControlButton obj)
		{
			MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.Pause, 1, null);
			MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.EnableShallowProfile, 0, null);
			m_recordCounter = 5;
		}

		private void SendLog(MyGuiControlButton button)
		{
			MyLog.Default.WriteLine("Send log requested");
			MyLog.Default.Flush();
			MySandboxGame.PerformNotInteractiveReport?.Invoke(includeAdditionalLogs: true);
			button.Text = "Sent";
		}

		private void SendLogWithProfilerData(MyGuiControlButton button)
		{
			MyLog.Default.WriteLine("Send logs with profiler data.");
			MyLog.Default.Flush();
<<<<<<< HEAD
			MySandboxGame.PerformNotInteractiveReport?.Invoke(includeAdditionalLogs: true, Enumerable.Range(0, m_nextProfilerDumpIndex).Select(MyObjectBuilder_ProfilerSnapshot.GetProfilerDumpPath));
=======
			MySandboxGame.PerformNotInteractiveReport?.Invoke(includeAdditionalLogs: true, Enumerable.Select<int, string>(Enumerable.Range(0, m_nextProfilerDumpIndex), (Func<int, string>)MyObjectBuilder_ProfilerSnapshot.GetProfilerDumpPath));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			button.Text = "Sent";
		}

		public override bool Update(bool hasFocus)
		{
			if (m_recordCounter > 0)
			{
				m_recordCounter--;
				if (m_recordCounter == 0)
				{
					MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.Pause, -1, null);
					MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.EnableShallowProfile, 1, null);
					MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.SaveToFile, m_nextProfilerDumpIndex++, null);
					VRage.Profiler.MyRenderProfiler.OnProfilerSnapshotSaved = delegate
					{
						MyHud.Notifications?.Add(new MyHudNotificationDebug("Profiler Snapshot Saved"));
						VRage.Profiler.MyRenderProfiler.OnProfilerSnapshotSaved = null;
						m_cnt.Text = $"Dump Count: {m_nextProfilerDumpIndex}";
					};
				}
			}
			return base.Update(hasFocus);
		}
	}
}
