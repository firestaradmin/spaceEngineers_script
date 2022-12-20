using System.Collections.Generic;
using VRage;
using VRage.Input;

namespace Sandbox.Graphics.GUI
{
	public class MyKeyThrottler
	{
		private class MyKeyThrottleState
		{
			/// <summary>
			/// This is not for converting key to string, but for controling repeated key input with delay
			/// </summary>
			public int LastKeyPressTime = -60000;

			/// <summary>
			/// The required delay until the key is ready again.
			/// </summary>
			public int RequiredDelay;
		}

		private static int m_windowsCharacterInitialDelay = -1;

		private static int m_windowsCharacterRepeatDelay = -1;

		private static int m_windowsCharacterInitialDelayMs = 0;

		private static int m_windowsCharacterRepeatDelayMs = 0;

		private static Dictionary<MyKeys, MyKeyThrottleState> m_keyTimeControllers = new Dictionary<MyKeys, MyKeyThrottleState>();

		public static int WINDOWS_CharacterInitialDelayMs
		{
			get
			{
				if (m_windowsCharacterInitialDelay != MyVRage.Platform.Input.KeyboardDelay)
				{
					m_windowsCharacterInitialDelay = MyVRage.Platform.Input.KeyboardDelay;
					ComputeCharacterInitialDelay(m_windowsCharacterInitialDelay, out m_windowsCharacterInitialDelayMs);
				}
				return m_windowsCharacterInitialDelayMs;
			}
		}

		public static int WINDOWS_CharacterRepeatDelayMs
		{
			get
			{
				if (m_windowsCharacterRepeatDelay != MyVRage.Platform.Input.KeyboardSpeed)
				{
					m_windowsCharacterRepeatDelay = MyVRage.Platform.Input.KeyboardSpeed;
					ComputeCharacterRepeatDelay(m_windowsCharacterRepeatDelay, out m_windowsCharacterRepeatDelayMs);
				}
				return m_windowsCharacterRepeatDelayMs;
			}
		}

		private static void ComputeCharacterInitialDelay(int code, out int ms)
		{
			switch (code)
			{
			case 0:
				ms = 250;
				break;
			case 1:
				ms = 500;
				break;
			case 2:
				ms = 750;
				break;
			case 3:
				ms = 1000;
				break;
			default:
				ms = 500;
				break;
			}
		}

		private static void ComputeCharacterRepeatDelay(int code, out int ms)
		{
			if (code < 0 || code > 31)
			{
				ms = 25;
				return;
			}
			float num = 500f;
			float num2 = 25f;
			float num3 = (float)code / 31f;
			ms = (int)((1f - num3) * num + num3 * num2);
		}

		private MyKeyThrottleState GetKeyController(MyKeys key)
		{
			if (m_keyTimeControllers.TryGetValue(key, out var value))
			{
				return value;
			}
			value = new MyKeyThrottleState();
			m_keyTimeControllers[key] = value;
			return value;
		}

		/// <summary>
		/// Determines if the given key was pressed during this update cycle, but it also
		/// makes sure a minimum amount of time has passed before allowing a press.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsNewPressAndThrottled(MyKeys key)
		{
			if (!MyInput.Static.IsNewKeyPressed(key))
			{
				return false;
			}
			MyKeyThrottleState keyController = GetKeyController(key);
			if (keyController == null)
			{
				return true;
			}
			if (MyGuiManager.TotalTimeInMilliseconds - keyController.LastKeyPressTime > WINDOWS_CharacterRepeatDelayMs)
			{
				keyController.LastKeyPressTime = MyGuiManager.TotalTimeInMilliseconds;
				return true;
			}
			return false;
		}

		public ThrottledKeyStatus GetKeyStatus(MyKeys key)
		{
			if (!MyInput.Static.IsKeyPress(key))
			{
				return ThrottledKeyStatus.UNPRESSED;
			}
			MyKeyThrottleState keyController = GetKeyController(key);
			if (keyController == null)
			{
				return ThrottledKeyStatus.PRESSED_AND_READY;
			}
			if (MyInput.Static.IsNewKeyPressed(key))
			{
				keyController.RequiredDelay = WINDOWS_CharacterInitialDelayMs;
				keyController.LastKeyPressTime = MyGuiManager.TotalTimeInMilliseconds;
				return ThrottledKeyStatus.PRESSED_AND_READY;
			}
			if (MyGuiManager.TotalTimeInMilliseconds - keyController.LastKeyPressTime > keyController.RequiredDelay)
			{
				keyController.RequiredDelay = WINDOWS_CharacterRepeatDelayMs;
				keyController.LastKeyPressTime = MyGuiManager.TotalTimeInMilliseconds;
				return ThrottledKeyStatus.PRESSED_AND_READY;
			}
			return ThrottledKeyStatus.PRESSED_AND_WAITING;
		}
	}
}
