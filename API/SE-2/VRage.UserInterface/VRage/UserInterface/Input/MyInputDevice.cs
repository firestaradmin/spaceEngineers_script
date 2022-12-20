using System;
using System.Collections.Concurrent;
using EmptyKeys.UserInterface.Input;

namespace VRage.UserInterface.Input
{
	public class MyInputDevice : InputDeviceBase
	{
		private KeyboardStateBase m_keyboardState = new MyKeyboardState();

		private MouseStateBase m_mouseState = new MyMouseState();

		private GamePadStateBase m_gamePadState = new MyGamePadState();

		private TouchStateBase m_touchState = new MyTouchState();

		private Action<string> m_onSuccess;

		private static readonly ConcurrentQueue<Action> m_invoke = new ConcurrentQueue<Action>();

		public override GamePadStateBase GamePadState => m_gamePadState;

		public override KeyboardStateBase KeyboardState => m_keyboardState;

		public override MouseStateBase MouseState => m_mouseState;

		public override TouchStateBase TouchState => m_touchState;

		public override void ShowVirtualKeyboard(Action<string> onSuccess, Action onCancel = null, string defaultText = null, string title = null, int maxLength = 0)
		{
			m_onSuccess = onSuccess;
			MyVRage.Platform.Input2.ShowVirtualKeyboardIfNeeded(OnVirtualKeyboardDataReceived, null, defaultText, null, maxLength);
		}

		private void OnVirtualKeyboardDataReceived(string text)
		{
			m_invoke.Enqueue(delegate
			{
				m_onSuccess.InvokeIfNotNull(text);
			});
		}

		public override void Update()
		{
			Action result;
			while (m_invoke.TryDequeue(out result))
			{
				result();
			}
		}
	}
}
