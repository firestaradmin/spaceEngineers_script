using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace VRage.Ansel
{
	public sealed class MyAnsel : IAnsel
	{
		private bool m_enableAnselWithSprites;

		private Mutex m_anselMutex;

		private CaptureType m_captureType;

		private UserControlCallback m_visibleOverlayCallback;

		private static int m_displayCounterForCursor;

		private readonly StopSessionCallback m_stopSessionDelegate;

		private readonly StopCaptureCallback m_stopCaptureDelegate;

		private readonly StartCaptureCallback m_startCaptureDelegate;

		private readonly StartSessionCallback m_startSessionDelegate;

		private MyAnselCamera m_camera;

		public IntPtr WindowHandle;

		public bool IsSessionEnabled { get; set; }

		public bool IsGamePausable { get; set; }

		public bool IsOverlayEnabled { get; private set; }

		public bool IsSessionRunning { get; private set; }

		public bool IsCaptureRunning { get; private set; }

		public bool IsMultiresCapturing
		{
			get
			{
				if (!IsCaptureRunning)
				{
					return false;
				}
				if (m_captureType != CaptureType.kCaptureTypeStereo)
				{
					return m_captureType == CaptureType.kCaptureTypeSuperResolution;
				}
				return true;
			}
		}

		public bool IsInitializedSuccessfuly { get; private set; }

		public bool Is360Capturing
		{
			get
			{
				if (!IsCaptureRunning)
				{
					return false;
				}
				if (m_captureType != 0)
				{
					return m_captureType == CaptureType.kCaptureType360Stereo;
				}
				return true;
			}
		}

		public event Action<int> StartCaptureDelegate;

		public event Action StopCaptureDelegate;

		public event Action<bool, bool> WarningMessageDelegate;

		public event Func<bool> IsSpectatorEnabledDelegate;

		[DllImport("user32.dll")]
		public static extern int ShowCursor(bool bVisible);

		public MyAnsel()
		{
			m_stopSessionDelegate = StopSession;
			m_stopCaptureDelegate = StopCapture;
			m_startCaptureDelegate = StartCapture;
			m_startSessionDelegate = StartSession;
		}

		public void SetCamera(ref MyCameraSetup cameraSetup)
		{
			m_camera = new MyAnselCamera(cameraSetup.ViewMatrix, cameraSetup.FOV, cameraSetup.AspectRatio, cameraSetup.NearPlane, cameraSetup.FarPlane, cameraSetup.FarPlane, cameraSetup.Position, cameraSetup.ProjectionOffsetX, cameraSetup.ProjectionOffsetY);
		}

		public void GetCamera(out MyCameraSetup cameraSetup)
		{
			m_camera.Update(this.IsSpectatorEnabledDelegate == null || this.IsSpectatorEnabledDelegate());
			cameraSetup.ViewMatrix = m_camera.ViewMatrix;
			cameraSetup.Position = m_camera.Position;
			cameraSetup.FarPlane = m_camera.FarFarPlane;
			cameraSetup.FOV = m_camera.FOV;
			cameraSetup.NearPlane = m_camera.NearPlane;
			cameraSetup.ProjectionMatrix = m_camera.ProjectionFarMatrix;
			cameraSetup.ProjectionOffsetX = m_camera.ProjectionOffset.X;
			cameraSetup.ProjectionOffsetY = m_camera.ProjectionOffset.Y;
			cameraSetup.AspectRatio = 0f;
		}

		private void VisibleOverlayUserControlCallback(ref UserControlInfo info)
		{
			byte b = Marshal.ReadByte(info.value);
			IsOverlayEnabled = b != 0;
		}

		private unsafe void AddControls()
		{
			bool isOverlayEnabled = IsOverlayEnabled;
			m_visibleOverlayCallback = VisibleOverlayUserControlCallback;
			UserControlDesc userControlDesc = default(UserControlDesc);
			userControlDesc.labelUtf8 = "Visible overlay";
			userControlDesc.callback = m_visibleOverlayCallback;
			userControlDesc.info = new UserControlInfo
			{
				userControlId = 1u,
				userControlType = UserControlType.kUserControlBoolean,
				value = new IntPtr(&isOverlayEnabled)
			};
			UserControlDesc desc = userControlDesc;
			NativeMethods.addUserControl(ref desc);
		}

		private StartSessionStatus StartSession(ref SessionConfiguration settings, IntPtr userPointer)
		{
			if (!IsSessionEnabled || !MyVRage.Platform.Windows.Window.IsActive)
			{
				return StartSessionStatus.kDisallowed;
			}
			this.WarningMessageDelegate.InvokeIfNotNull(IsGamePausable, this.IsSpectatorEnabledDelegate == null || this.IsSpectatorEnabledDelegate());
			m_displayCounterForCursor = ShowCursor(bVisible: false);
			settings.isRawAllowed = false;
			settings.isPauseAllowed = IsGamePausable;
			settings.isHighresAllowed = true;
			settings.isRotationAllowed = true;
			settings.isTranslationAllowed = true;
			settings.isFovChangeAllowed = true;
			settings.is360MonoAllowed = true;
			settings.is360StereoAllowed = true;
			settings.isRawAllowed = true;
			IsSessionRunning = true;
			return StartSessionStatus.kAllowed;
		}

		private void StopSession(IntPtr userPointer)
		{
			IsSessionRunning = false;
			ShowCursor(bVisible: true);
		}

		private void StartCapture(ref CaptureType captureType, IntPtr userPointer)
		{
			IsCaptureRunning = true;
			m_captureType = captureType;
			this.StartCaptureDelegate.InvokeIfNotNull((int)captureType);
		}

		private void StopCapture(IntPtr userPointer)
		{
			this.StopCaptureDelegate.InvokeIfNotNull();
			IsCaptureRunning = false;
		}

		private Configuration GetConfiguration(IntPtr windowHandle)
		{
			Configuration configuration = default(Configuration);
			configuration.right = new Vec3(1f, 0f, 0f);
			configuration.up = new Vec3(0f, 1f, 0f);
			configuration.forward = new Vec3(0f, 0f, -1f);
			configuration.translationalSpeedInWorldUnitsPerSecond = 5f;
			configuration.rotationalSpeedInDegreesPerSecond = 55f;
			configuration.captureLatency = 0u;
			configuration.captureSettleLatency = 1u;
			configuration.metersInWorldUnit = 1f;
			configuration.isCameraOffcenteredProjectionSupported = true;
			configuration.isCameraRotationSupported = true;
			configuration.isCameraTranslationSupported = true;
			configuration.isCameraFovSupported = true;
			configuration.fovType = FovType.kVerticalFov;
			configuration.isFilterOutsideSessionAllowed = false;
			configuration.sdkVersion = 281491311529170uL;
			Configuration result = configuration;
			result.gameWindowHandle = windowHandle;
			result.startSessionCallback = m_startSessionDelegate;
			result.startCaptureCallback = m_startCaptureDelegate;
			result.stopCaptureCallback = m_stopCaptureDelegate;
			result.stopSessionCallback = m_stopSessionDelegate;
			return result;
		}

		public void Enable()
		{
			int id = Process.GetCurrentProcess().Id;
			string name = $"NVIDIA/Ansel/{id}";
			m_anselMutex = new Mutex(initiallyOwned: false, name);
		}

		public int Init(bool enableAnselWithSprites)
		{
			IsInitializedSuccessfuly = false;
			m_enableAnselWithSprites = enableAnselWithSprites;
			if (!NativeMethods.isAnselAvailable())
			{
				return 3;
			}
			SetConfigurationStatus num = NativeMethods.setConfiguration(GetConfiguration(WindowHandle));
			if (m_enableAnselWithSprites)
			{
				AddControls();
			}
			if (num == SetConfigurationStatus.kSetConfigurationSuccess)
			{
				IsInitializedSuccessfuly = true;
			}
			return (int)num;
		}

		public void StartSession()
		{
			NativeMethods.startSession();
			IsSessionRunning = true;
		}

		public void StopSession()
		{
			IsSessionRunning = false;
			NativeMethods.stopSession();
		}

		public void MarkHdrBufferBind()
		{
			if (IsInitializedSuccessfuly)
			{
				NativeMethods.markHdrBufferBind(HintType.kHintTypePreBind, ulong.MaxValue);
			}
		}

		public void MarkHdrBufferFinished()
		{
			if (IsInitializedSuccessfuly)
			{
				NativeMethods.markHdrBufferFinished(ulong.MaxValue);
			}
		}
	}
}
