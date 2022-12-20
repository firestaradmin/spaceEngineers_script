using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using SharpDX;
using SharpDX.DirectInput;
using SharpDX.XInput;
using VRage.Input;
using VRage.Native;
using VRage.Platform.Windows.Forms;
using VRage.Utils;

namespace VRage.Platform.Windows.Input
{
	internal class MyDirectInput : IVRageInput2, IDisposable
	{
		private DirectInput m_directInput;

		private readonly MyWindowsWindows m_windows;

		private Device m_joystick;

		private Controller m_joystickXInput;

		private bool m_joystickXAxisSupported;

		private bool m_joystickYAxisSupported;

		private bool m_joystickZAxisSupported;

		private bool m_joystickRotationXAxisSupported;

		private bool m_joystickRotationYAxisSupported;

		private bool m_joystickRotationZAxisSupported;

		private bool m_joystickSlider1AxisSupported;

		private bool m_joystickSlider2AxisSupported;

		private MyJoystickState_Basic m_basicState;

		private State m_statexInput;

		private Mouse m_mouse;

		private MouseState m_mouseState = new MouseState();

		private readonly List<string> m_joystickList = new List<string>();

		public bool IsCorrectlyInitialized { get; private set; }

		public string JoystickUserId { get; }

		public string JoystickUserName { get; }

		public uint[] DeveloperKeys { get; } = new uint[4] { 2726635697u, 644003104u, 3810731010u, 2191594058u };


		public MyDirectInput(MyWindowsWindows windows)
		{
			m_windows = windows;
			try
			{
				m_directInput = new DirectInput();
				m_mouse = new Mouse(m_directInput);
				try
				{
					m_mouse.SetCooperativeLevel(m_windows.WindowHandle, CooperativeLevel.NonExclusive | CooperativeLevel.Foreground);
				}
				catch
				{
					MyLog.Default.WriteLine("WARNING: DirectInput SetCooperativeLevel failed");
				}
			}
			catch (SharpDXException ex)
			{
				MyLog.Default.WriteLine("DirectInput initialization error: " + ex);
			}
			IsCorrectlyInitialized = m_directInput != null;
		}

		public void Dispose()
		{
			if (m_mouse != null)
			{
				m_mouse.Dispose();
				m_mouse = null;
			}
			if (m_directInput != null)
			{
				m_directInput.Dispose();
				m_directInput = null;
			}
		}

		public void GetMouseState(out MyMouseState state)
		{
			if (m_mouse == null)
			{
				state = default(MyMouseState);
			}
			else if (m_mouse.TryAcquire().Success)
			{
				try
				{
					m_mouse.Acquire();
					m_mouse.GetCurrentState(ref m_mouseState);
					m_mouse.Poll();
					state = new MyMouseState
					{
						X = m_mouseState.X,
						Y = m_mouseState.Y,
						LeftButton = m_mouseState.Buttons[0],
						RightButton = m_mouseState.Buttons[1],
						MiddleButton = m_mouseState.Buttons[2],
						XButton1 = m_mouseState.Buttons[3],
						XButton2 = m_mouseState.Buttons[4],
						ScrollWheelValue = m_mouseState.Z
					};
				}
				catch (SharpDXException)
				{
					state = default(MyMouseState);
				}
			}
			else
			{
				state = default(MyMouseState);
			}
		}

		public List<string> EnumerateJoystickNames()
		{
			m_joystickList.Clear();
			IList<DeviceInstance> list = m_directInput?.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly);
			for (int i = 0; i < list?.Count; i++)
			{
				DeviceInstance deviceInstance = list[i];
				m_joystickList.Add(deviceInstance.InstanceName.Replace("\0", string.Empty));
			}
			return m_joystickList;
		}

		private void InitializeXInputJoystickIfPossible(UserIndex index)
		{
			try
			{
				m_joystickXInput = new Controller(index);
			}
			catch (SharpDXException)
			{
				m_joystickXInput = null;
				return;
			}
			RunXInputPolling();
		}

		private void RunXInputPolling()
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				try
				{
					int millisecondsTimeout = 15;
					UserIndex userIndex = m_joystickXInput.UserIndex;
					while (m_joystickXInput != null && m_joystickXInput.IsConnected && userIndex == m_joystickXInput.UserIndex)
					{
						m_statexInput = m_joystickXInput.GetState();
						Thread.Sleep(millisecondsTimeout);
					}
				}
				catch
				{
				}
			});
		}

		public string InitializeJoystickIfPossible(string joystickInstanceName)
		{
			string text = null;
			if (m_joystick != null)
			{
				m_joystick.Dispose();
				m_joystick = null;
			}
			if (m_joystick == null && joystickInstanceName != null && m_directInput != null)
			{
				foreach (DeviceInstance device in m_directInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly))
				{
					if (joystickInstanceName == null || device.InstanceName.Contains(joystickInstanceName))
					{
						try
						{
							text = device.InstanceName;
							m_joystick = new Joystick(m_directInput, device.InstanceGuid);
							m_joystick.SetCooperativeLevel(m_windows.WindowHandle, CooperativeLevel.NonExclusive | CooperativeLevel.Background);
						}
						catch
						{
							continue;
						}
						break;
					}
				}
				if (m_joystick != null)
				{
					int num = 0;
					m_joystickXAxisSupported = (m_joystickYAxisSupported = (m_joystickZAxisSupported = false));
					m_joystickRotationXAxisSupported = (m_joystickRotationYAxisSupported = (m_joystickRotationZAxisSupported = false));
					m_joystickSlider1AxisSupported = (m_joystickSlider2AxisSupported = false);
					foreach (DeviceObjectInstance @object in m_joystick.GetObjects())
					{
						if ((@object.ObjectId.Flags & DeviceObjectTypeFlags.Axis) == 0)
						{
							continue;
						}
						m_joystick.GetObjectPropertiesById(@object.ObjectId).Range = new InputRange(0, 65535);
						if (@object.ObjectType == ObjectGuid.XAxis)
						{
							m_joystickXAxisSupported = true;
						}
						else if (@object.ObjectType == ObjectGuid.YAxis)
						{
							m_joystickYAxisSupported = true;
						}
						else if (@object.ObjectType == ObjectGuid.ZAxis)
						{
							m_joystickZAxisSupported = true;
						}
						else if (@object.ObjectType == ObjectGuid.RxAxis)
						{
							m_joystickRotationXAxisSupported = true;
						}
						else if (@object.ObjectType == ObjectGuid.RyAxis)
						{
							m_joystickRotationYAxisSupported = true;
						}
						else if (@object.ObjectType == ObjectGuid.RzAxis)
						{
							m_joystickRotationZAxisSupported = true;
						}
						else if (@object.ObjectType == ObjectGuid.Slider)
						{
							num++;
							if (num >= 1)
							{
								m_joystickSlider1AxisSupported = true;
							}
							if (num >= 2)
							{
								m_joystickSlider2AxisSupported = true;
							}
						}
					}
					try
					{
						m_joystick.Acquire();
					}
					catch
					{
					}
				}
			}
			if (text == null && joystickInstanceName != null)
			{
				return InitializeJoystickIfPossible(null);
			}
			if (m_joystick != null)
			{
				try
				{
					InitializeXInputJoystickIfPossible((UserIndex)m_joystick.Properties.JoystickId);
					return text;
				}
				catch (SharpDXException)
				{
					m_joystick = null;
					text = null;
					MyLog.Default.WriteLine("Selected controller is not supported");
					return text;
				}
			}
			return text;
		}

		public bool IsJoystickAxisSupported(MyJoystickAxesEnum axis)
		{
			switch (axis)
			{
			case MyJoystickAxesEnum.Xpos:
			case MyJoystickAxesEnum.Xneg:
				return m_joystickXAxisSupported;
			case MyJoystickAxesEnum.Ypos:
			case MyJoystickAxesEnum.Yneg:
				return m_joystickYAxisSupported;
			case MyJoystickAxesEnum.Zpos:
			case MyJoystickAxesEnum.Zneg:
				return m_joystickZAxisSupported;
			case MyJoystickAxesEnum.ZLeft:
				return m_joystickZAxisSupported;
			case MyJoystickAxesEnum.ZRight:
				return m_joystickZAxisSupported;
			case MyJoystickAxesEnum.RotationXpos:
			case MyJoystickAxesEnum.RotationXneg:
				return m_joystickRotationXAxisSupported;
			case MyJoystickAxesEnum.RotationYpos:
			case MyJoystickAxesEnum.RotationYneg:
				return m_joystickRotationYAxisSupported;
			case MyJoystickAxesEnum.RotationZpos:
			case MyJoystickAxesEnum.RotationZneg:
				return m_joystickRotationZAxisSupported;
			case MyJoystickAxesEnum.Slider1pos:
			case MyJoystickAxesEnum.Slider1neg:
				return m_joystickSlider1AxisSupported;
			case MyJoystickAxesEnum.Slider2pos:
			case MyJoystickAxesEnum.Slider2neg:
				return m_joystickSlider2AxisSupported;
			default:
				return false;
			}
		}

		public bool IsJoystickConnected()
		{
			if (m_joystick != null)
			{
				return m_directInput.IsDeviceAttached(m_joystick.Information.InstanceGuid);
			}
			return false;
		}

		private static void GetDeviceState(IntPtr nativePointer, int arg0, IntPtr arg1)
		{
			NativeCall<int>.Method(nativePointer, 9, arg0, arg1);
		}

		public unsafe void GetJoystickState(ref MyJoystickState state)
		{
			m_joystick.Acquire();
			m_joystick.Poll();
			int num = Utilities.SizeOf<MyJoystickState_Basic>();
			byte* ptr = stackalloc byte[(int)(uint)(num * 2)];
			GetDeviceState(m_joystick.NativePointer, num, (IntPtr)ptr);
			Utilities.Read((IntPtr)ptr, ref m_basicState);
			state.CopyFromJoystickBasicState(m_basicState);
			if (m_joystickXInput != null && m_joystickXInput.IsConnected)
			{
				state.Z_Left = m_statexInput.Gamepad.LeftTrigger * 256;
				state.Z_Right = m_statexInput.Gamepad.RightTrigger * 256;
			}
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern short GetKeyState(int keyCode);

		public bool GetLockKeyState(int keyCode)
		{
			return ((ushort)GetKeyState(keyCode) & 0xFFFF) != 0;
		}

		[DllImport("user32.dll")]
		private static extern short GetAsyncKeyState(int keyCode);

		public unsafe void GetAsyncKeyStates(byte* data)
		{
			for (int i = 0; i < 256; i++)
			{
				bool num = GetAsyncKeyState(i) >> 15 != 0;
				int num2 = i % 8;
				byte b = (byte)(1 << num2);
				if (num)
				{
					byte* intPtr = data + i / 8;
					*intPtr = (byte)(*intPtr | b);
				}
				else
				{
					byte* intPtr2 = data + i / 8;
					*intPtr2 = (byte)(*intPtr2 & (byte)(~b));
				}
			}
		}

		public void ShowVirtualKeyboardIfNeeded(Action<string> onSuccess, Action onCancel = null, string defaultText = null, string title = null, int maxLength = 0)
		{
			onSuccess(defaultText ?? string.Empty);
		}
	}
}
