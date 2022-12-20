using System;
using System.IO;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security;
using GameAnalyticsSDK.Net;
using SharpDX;
using SharpDX.Diagnostics;
using VRage.Analytics;
using VRage.Ansel;
using VRage.Audio;
using VRage.Http;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Platform.Windows.Audio;
using VRage.Platform.Windows.DShow;
using VRage.Platform.Windows.Forms;
using VRage.Platform.Windows.Http;
using VRage.Platform.Windows.IME;
using VRage.Platform.Windows.Input;
using VRage.Platform.Windows.Render;
using VRage.Platform.Windows.Serialization;
using VRage.Platform.Windows.Sys;
using VRage.Scripting;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Platform.Windows
{
	internal sealed class MyVRagePlatform : IVRagePlatform
	{
		private readonly MyWindowsHttpClient m_http;

		private readonly MyWindowsSystem m_system;

		private readonly MyWindowsRender m_render;

		private readonly MyWindowsWindows m_windows;

		private readonly bool m_detectLeaks;

		private IProtoTypeModel m_typeModel;

		private IMyAudio m_audio;

		private MyDirectInput m_input2;

		public IVRageHttp Http => m_http;

		public IVRageSystem System => m_system;

		public IVRageRender Render => m_render;

		public IVRageWindows Windows => m_windows;

		public IAnsel Ansel { get; private set; }

		public IAfterMath AfterMath { get; private set; }

		public IVRageInput Input { get; internal set; }

		public IMyImeProcessor ImeProcessor => MyImeProcessor.Instance;

		public IMyCrashReporting CrashReporting { get; } = new MyCrashReporting();


		public IVRageScripting Scripting { get; } = MyVRageScripting.Create();


		public bool SessionReady { get; set; }

		public IMyAudio Audio
		{
			get
			{
				if (m_audio == null)
				{
					MyPlatformAudio audioPlatform = new MyPlatformAudio();
					m_audio = new MyXAudio2(audioPlatform);
				}
				return m_audio;
			}
		}

		public IVRageInput2 Input2 => m_input2;

		[DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]
		[SuppressUnmanagedCodeSecurity]
		public static extern uint TimeBeginPeriod(uint uMilliseconds);

		[DllImport("winmm.dll", EntryPoint = "timeEndPeriod", SetLastError = true)]
		[SuppressUnmanagedCodeSecurity]
		public static extern uint TimeEndPeriod(uint uMilliseconds);

		public MyVRagePlatform(string applicationName, MyLog log, string appDataPath, bool detectLeaks)
		{
			m_detectLeaks = detectLeaks;
			m_http = new MyWindowsHttpClient(this);
			m_system = new MyWindowsSystem(applicationName, appDataPath, log);
			m_windows = new MyWindowsWindows(this);
			m_render = new MyWindowsRender(log, m_windows);
			string text = Path.Combine(m_system.GetAppDataPath(), "ProfileOptimization");
			Directory.CreateDirectory(text);
			ProfileOptimization.SetProfileRoot(text);
			ProfileOptimization.StartProfile("Startup.profile");
			MyDebugListenerProvider.Register();
			WaitForTargetFrameRate.ENABLE_TIMING_HOTFIX = true;
		}

		public void Init()
		{
			if (WaitForTargetFrameRate.ENABLE_TIMING_HOTFIX)
			{
				TimeBeginPeriod(1u);
			}
			if (m_detectLeaks)
			{
				Configuration.EnableObjectTracking = true;
			}
			m_system.Init();
			m_typeModel = new DynamicTypeModel();
			Ansel = new MyAnsel();
			AfterMath = new MyAfterMath();
		}

		public IVideoPlayer CreateVideoPlayer()
		{
			return new MyVideoPlayer();
		}

		public void Update()
		{
		}

		public IMyAnalytics InitAnalytics(string projectId, string version)
		{
			return new MyGameAnalytics(projectId, version);
		}

		public void Done()
		{
			if (WaitForTargetFrameRate.ENABLE_TIMING_HOTFIX)
			{
				TimeEndPeriod(1u);
			}
			GameAnalytics.EndSession();
			if (m_detectLeaks)
			{
				ObjectTracker.FindActiveObjects();
				Console.WriteLine(ObjectTracker.ReportActiveObjects());
			}
		}

		public IProtoTypeModel GetTypeModel()
		{
			return m_typeModel;
		}

		public bool CreateInput2()
		{
			m_input2 = new MyDirectInput(m_windows);
			return m_input2.IsCorrectlyInitialized;
		}
	}
}
