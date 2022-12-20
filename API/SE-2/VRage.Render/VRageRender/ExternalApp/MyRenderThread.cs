using System;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading;
using ParallelTasks;
using VRage;
using VRage.Collections;
using VRage.Library.Utils;
using VRage.Stats;
using VRage.Utils;
using VRageRender.Utils;

namespace VRageRender.ExternalApp
{
	public class MyRenderThread
	{
		private class StartParams
		{
			public InitHandler InitHandler;

			public MyRenderDeviceSettings? SettingsToTry;
		}

		private readonly MyGameTimer m_timer;

		private readonly WaitForTargetFrameRate m_waiter;

		private MyTimeSpan m_messageProcessingStart;

		private MyTimeSpan m_frameStart;

		private int m_stopped;

		private IVRageWindow m_renderWindow;

		private MyRenderDeviceSettings m_settings;

		private MyRenderDeviceSettings? m_newSettings;

		public readonly Thread SystemThread;

		public Thread ProcessStateChangesThread;

		private readonly bool m_separateThread;

		private readonly MyConcurrentQueue<EventWaitHandle> m_debugWaitForPresentHandles = new MyConcurrentQueue<EventWaitHandle>(16);

		private int m_debugWaitForPresentHandleCount;

		private MyAdapterInfo[] m_adapterList;

		private MyTimeSpan m_waitStart;

		private MyTimeSpan m_drawStart;

		private bool m_suspended;

		private bool m_suspendChanged;

		public IVRageWindow RenderWindow => m_renderWindow;

		public int CurrentAdapter => m_settings.AdapterOrdinal;

		public MyRenderDeviceSettings CurrentSettings => m_settings;

		public ManualResetEvent RenderUpdateSyncEvent { get; set; }

		public bool IsSeparateThread => m_separateThread;

		public bool Suspend
		{
			get
			{
				return m_suspended;
			}
			set
			{
				m_suspendChanged = m_suspended != value;
				m_suspended = value;
			}
		}

		public event Action BeforeDraw;

		public event SizeChangedHandler SizeChanged;

		private MyRenderThread(MyGameTimer timer, bool separateThread, float maxFrameRate)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			m_timer = timer;
			m_waiter = new WaitForTargetFrameRate(timer, maxFrameRate);
			m_separateThread = separateThread;
			if (separateThread)
			{
<<<<<<< HEAD
				SystemThread = new Thread(RenderThreadStart);
				SystemThread.IsBackground = true;
				SystemThread.Name = "Render thread";
				SystemThread.CurrentCulture = CultureInfo.InvariantCulture;
				SystemThread.CurrentUICulture = CultureInfo.InvariantCulture;
=======
				SystemThread = new Thread((ParameterizedThreadStart)RenderThreadStart);
				SystemThread.set_IsBackground(true);
				SystemThread.set_Name("Render thread");
				SystemThread.set_CurrentCulture(CultureInfo.InvariantCulture);
				SystemThread.set_CurrentUICulture(CultureInfo.InvariantCulture);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyRenderProxy.RenderSystemThread = SystemThread;
			}
			else
			{
				SystemThread = Thread.get_CurrentThread();
			}
		}

		public static MyRenderThread Start(MyGameTimer timer, InitHandler initHandler, MyRenderDeviceSettings? settingsToTry, float maxFrameRate)
		{
			MyRenderThread myRenderThread = new MyRenderThread(timer, separateThread: true, maxFrameRate);
			myRenderThread.SystemThread.Start((object)new StartParams
			{
				InitHandler = initHandler,
				SettingsToTry = settingsToTry
			});
			return myRenderThread;
		}

		public static MyRenderThread StartSync(MyGameTimer timer, IVRageWindow renderWindow, MyRenderDeviceSettings? settingsToTry, float maxFrameRate)
		{
			MyRenderThread myRenderThread = new MyRenderThread(timer, separateThread: false, maxFrameRate)
			{
				m_renderWindow = renderWindow
			};
			myRenderThread.m_settings = MyRenderProxy.CreateDevice(myRenderThread, settingsToTry, out myRenderThread.m_adapterList);
			MyRenderProxy.SendCreatedDeviceSettings(myRenderThread.m_settings);
			myRenderThread.UpdateSize();
			return myRenderThread;
		}

		public void TickSync()
		{
			if (MyRenderProxy.EnableAppEventsCall)
			{
				m_renderWindow.DoEvents();
			}
			RenderCallback(async: false);
		}

		public void SwitchSettings(MyRenderDeviceSettings settings)
		{
			m_newSettings = settings;
		}

		/// <summary>
		/// Signals the thread to exit and waits until it does so
		/// </summary>
		public void Exit()
		{
			if (Interlocked.Exchange(ref m_stopped, 1) == 1)
			{
				return;
			}
			if (SystemThread != null)
			{
				try
				{
					m_renderWindow.Exit();
				}
				catch
				{
				}
				if (Thread.get_CurrentThread() != SystemThread)
				{
					SystemThread.Join();
				}
			}
			else
			{
				UnloadContent();
				MyRenderProxy.DisposeDevice();
			}
		}

		private void RenderThreadStart(object param)
		{
			StartParams startParams = (StartParams)param;
			m_renderWindow = startParams.InitHandler();
			m_settings = MyRenderProxy.CreateDevice(this, startParams.SettingsToTry, out m_adapterList);
			if (m_settings.AdapterOrdinal == -1)
			{
				return;
			}
			MyRenderProxy.SendCreatedDeviceSettings(m_settings);
			UpdateSize();
			if (MyRenderProxy.Settings.EnableAnsel)
			{
				MyVRage.Platform.Ansel.Init(MyRenderProxy.Settings.EnableAnselWithSprites);
			}
			while (m_renderWindow.UpdateRenderThread())
			{
				if (RenderUpdateSyncEvent != null)
				{
					RenderUpdateSyncEvent.WaitOne();
				}
				RenderCallback(async: true);
			}
			MyRenderProxy.AfterUpdate(null);
			MyRenderProxy.BeforeUpdate();
			MyRenderProxy.ProcessMessages();
			UnloadContent();
			MyRenderProxy.DisposeDevice();
		}

		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		private void RenderCallback(bool async)
		{
			try
			{
				RenderFrame(async);
			}
			catch (Exception ex)
			{
				MyMiniDump.CollectExceptionDump(ex);
				string text = $"Exception in render!\n\nException: {ex}";
				MyVRage.Platform.System.LogToExternalDebugger(text);
				MyLog.Default.WriteLine(text);
				MyLog.Default.Flush();
				try
				{
					text = "Additional information --\n Aftermath: " + MyRenderProxy.GetLastExecutedAnnotation() + "\nStatistics: " + MyRenderProxy.GetStatistics();
					MyVRage.Platform.System.LogToExternalDebugger(text);
					MyLog.Default.WriteLine(text);
					MyLog.Default.Flush();
				}
				catch (Exception ex2)
				{
					MyLog.Default.WriteLine(ex2);
				}
				string text2 = "Graphics device driver has crashed.\n\nYour card is probably overheating or driver is malfunctioning. Please, update your graphics drivers and remove any overclocking";
				MyMessageBox.Show("Game crashed", text2);
				throw;
			}
		}

		private void RenderFrame(bool async)
		{
			if (SystemThread != null)
			{
				ThreadPriority threadPriority = (MyRenderProxy.Settings.RenderThreadHighPriority ? ThreadPriority.Highest : ThreadPriority.Normal);
<<<<<<< HEAD
				if (SystemThread.Priority != threadPriority)
=======
				if (SystemThread.get_Priority() != threadPriority)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					SystemThread.set_Priority(threadPriority);
				}
			}
			if (MyVRage.Platform.Ansel.IsCaptureRunning)
			{
				MyRenderProxy.Ansel_DrawScene();
				MyRenderProxy.Present();
				return;
			}
			if (m_messageProcessingStart != MyTimeSpan.Zero)
			{
				_ = m_timer.Elapsed - m_messageProcessingStart;
				m_waiter.Wait();
			}
			MySimpleProfiler.BeginBlock("RenderFrame", MySimpleProfiler.ProfilingBlockType.RENDER);
			m_drawStart = m_timer.Elapsed;
			MyTimeSpan cpuWait = m_drawStart - m_waitStart;
			m_frameStart = m_timer.Elapsed;
			switch (MyRenderProxy.FrameProcessStatus)
			{
			}
			if (m_suspendChanged)
			{
				if (m_suspended)
				{
					MyLog.Default.WriteLine("Suspending all threads");
					MyLog.Default.Flush();
					MyVRage.Platform.System.LogToExternalDebugger("Suspending all threads");
					Parallel.Scheduler.SuspendThreads(TimeSpan.FromMilliseconds(500.0));
					MyLog.Default.WriteLine("Suspending render");
					MyLog.Default.Flush();
					MyVRage.Platform.System.LogToExternalDebugger("Suspending render");
					MyVRage.Platform.Render.SuspendRenderContext();
					MyLog.Default.WriteLine("Render suspended");
					MyLog.Default.Flush();
					MyVRage.Platform.System.LogToExternalDebugger("Render suspended");
				}
				else
				{
					MyLog.Default.WriteLine("Resuming render");
					MyLog.Default.Flush();
					MyVRage.Platform.Render.ResumeRenderContext();
					MyLog.Default.WriteLine("Resuming all threads");
					MyLog.Default.Flush();
					Parallel.Scheduler.ResumeThreads();
					MyLog.Default.WriteLine("Resuming done");
					MyLog.Default.Flush();
				}
				m_suspendChanged = false;
			}
			if (m_suspended)
			{
				MyLog.Default.WriteLine("Render suspended - early exit");
				MyLog.Default.Flush();
				return;
			}
			ApplySettingsChanges();
			MyRenderStats.Generic.WriteFormat("Available GPU memory: {0} MB", (float)MyRenderProxy.GetAvailableTextureMemory() / 1024f / 1024f, MyStatTypeEnum.CurrentValue, 300, 2);
			MyRenderProxy.BeforeRender(m_frameStart);
			if (this.BeforeDraw != null)
			{
				this.BeforeDraw();
			}
			if (!m_renderWindow.DrawEnabled)
			{
				MyRenderProxy.ProcessMessages();
			}
			else
			{
				Draw();
			}
			MyRenderProxy.AfterRender();
			_ = m_separateThread;
			m_waitStart = m_timer.Elapsed;
			MyTimeSpan cpuDraw = m_waitStart - m_drawStart;
			MySimpleProfiler.End("RenderFrame");
			DoBeforePresent();
			try
			{
				MyRenderProxy.Present();
			}
			catch (MyDeviceErrorException ex)
			{
				MyRenderProxy.Error(ex.Message, 0, shouldTerminate: true);
				Exit();
			}
			DoAfterPresent();
			MyRenderProxy.SetTimings(cpuDraw, cpuWait);
			m_messageProcessingStart = m_timer.Elapsed;
			if (MyRenderProxy.Settings.ForceSlowCPU)
			{
				Thread.Sleep(200);
			}
		}

		private void DoBeforePresent()
		{
			m_debugWaitForPresentHandleCount = m_debugWaitForPresentHandles.Count;
		}

		private void DoAfterPresent()
		{
			for (int i = 0; i < m_debugWaitForPresentHandleCount; i++)
			{
				if (m_debugWaitForPresentHandles.TryDequeue(out var instance))
				{
					instance?.Set();
				}
			}
			m_debugWaitForPresentHandleCount = 0;
		}

		public void DebugAddWaitingForPresent(EventWaitHandle handle)
		{
			m_debugWaitForPresentHandles.Enqueue(handle);
		}

		private void ApplySettingsChanges()
		{
			if (m_newSettings.HasValue && MyRenderProxy.SettingsChanged(m_newSettings.Value))
			{
				m_settings = m_newSettings.Value;
				m_newSettings = null;
				UnloadContent();
				MyRenderProxy.ApplySettings(m_settings);
				UpdateSize();
			}
		}

		public void UpdateSize()
		{
			this.SizeChanged?.Invoke(MyRenderProxy.BackBufferResolution.X, MyRenderProxy.BackBufferResolution.Y, MyRenderProxy.MainViewport);
		}

		private void UnloadContent()
		{
			MyRenderProxy.UnloadContent();
		}

		private void Draw()
		{
			MyRenderProxy.Draw();
<<<<<<< HEAD
			MyRenderProxy.GetRenderProfiler().Draw("Draw", 511, "E:\\Repo1\\Sources\\VRage.Render\\ExternalApp\\MyRenderThread.cs");
=======
			MyRenderProxy.GetRenderProfiler().Draw("Draw", 507, "E:\\Repo3\\Sources\\VRage.Render\\ExternalApp\\MyRenderThread.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
