using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Utils;

namespace VRage.Input
{
	public static class MyControllerHelper
	{
		private class ButtonEvaluator : ITextEvaluator
		{
			private readonly Dictionary<string, string> ButtonCodes = new Dictionary<string, string>();

			public ButtonEvaluator()
			{
				char v;
				foreach (KeyValuePair<MyJoystickAxesEnum, char> xBOX_AXES_CODE in XBOX_AXES_CODES)
				{
					LinqExtensions.Deconstruct(xBOX_AXES_CODE, out var k, out v);
					MyJoystickAxesEnum myJoystickAxesEnum = k;
					char c = v;
					ButtonCodes.Add("AXIS_" + myJoystickAxesEnum.ToString().ToUpperInvariant(), c.ToString());
				}
				ButtonCodes.Add("AXIS_MOTION", "\ue009");
				ButtonCodes.Add("AXIS_MOTION_X", "\ue022");
				ButtonCodes.Add("AXIS_MOTION_Y", "\ue023");
				ButtonCodes.Add("AXIS_ROTATION", "\ue00a");
				ButtonCodes.Add("AXIS_ROTATION_X", "\ue024");
				ButtonCodes.Add("AXIS_ROTATION_Y", "\ue025");
				ButtonCodes.Add("AXIS_DPAD", "\ue00f");
				ButtonCodes.Add("LEFT_STICK_LEFTRIGHT", "\ue022");
				ButtonCodes.Add("LEFT_STICK_UPDOWN", "\ue023");
				ButtonCodes.Add("RIGHT_STICK_LEFTRIGHT", "\ue024");
				ButtonCodes.Add("RIGHT_STICK_UPDOWN", "\ue025");
				ButtonCodes.Add("DPAD_LEFTRIGHT", "\ue026");
				ButtonCodes.Add("DPAD_UPDOWN", "\ue027");
				foreach (KeyValuePair<MyJoystickButtonsEnum, char> xBOX_BUTTONS_CODE in XBOX_BUTTONS_CODES)
				{
					LinqExtensions.Deconstruct(xBOX_BUTTONS_CODE, out var k2, out v);
					MyJoystickButtonsEnum myJoystickButtonsEnum = k2;
					char c2 = v;
					ButtonCodes.Add("BUTTON_" + myJoystickButtonsEnum.ToString().ToUpperInvariant(), c2.ToString());
				}
			}

			public string TokenEvaluate(string token, string context)
			{
				if (ButtonCodes.TryGetValue(token, out var value))
				{
					return value;
				}
				return "<Invalid Button>";
			}
		}

		public class Context
		{
			public Context ParentContext;

			public Dictionary<MyStringId, IMyControllerControl> Bindings;

			public static readonly Context Empty = new Context();

			public IMyControllerControl this[MyStringId id]
			{
				get
				{
					if (Bindings.ContainsKey(id))
					{
						return Bindings[id];
					}
					if (ParentContext != null)
					{
						return ParentContext[id];
					}
					return m_nullControl;
				}
				set
				{
					Bindings[id] = value;
				}
			}

			public Context()
			{
				Bindings = new Dictionary<MyStringId, IMyControllerControl>(MyStringId.Comparer);
			}
		}

<<<<<<< HEAD
		public class EmptyControl : IMyControllerControl
=======
		private class EmptyControl : IMyControllerControl
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			public byte Code => 0;

			public Func<bool> Condition { get; private set; }

			public bool IsNewPressed()
			{
				return false;
			}

			public bool IsNewPressedRepeating()
			{
				return false;
			}

			public bool IsPressed()
			{
				return false;
			}

			public bool IsNewReleased()
			{
				return false;
			}

			public float AnalogValue()
			{
				return 0f;
			}

			public object ControlCode()
			{
				return " ";
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}

			public bool IsNewPressedXinput()
			{
				return false;
			}

			public bool IsNewReleasedXinput()
			{
				return false;
			}
		}

<<<<<<< HEAD
		public class JoystickAxis : IMyControllerControl
=======
		private class JoystickAxis : IMyControllerControl
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public MyJoystickAxesEnum Axis;

			public byte Code => (byte)Axis;

			public Func<bool> Condition { get; private set; }

			public JoystickAxis(MyJoystickAxesEnum axis, Func<bool> condition = null)
			{
				Axis = axis;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = MyInput.Static.IsJoystickAxisNewPressed(Axis);
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME);
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				return MyInput.Static.IsJoystickAxisPressed(Axis);
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				return MyInput.Static.IsNewJoystickAxisReleased(Axis);
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				return MyInput.Static.GetJoystickAxisStateForGameplay(Axis);
			}

			public object ControlCode()
			{
				return XBOX_AXES_CODES[Axis].ToString();
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}

			public bool IsNewPressedXinput()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = MyInput.Static.IsJoystickAxisNewPressedXinput(Axis);
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewReleasedXinput()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				return MyInput.Static.IsNewJoystickAxisReleasedXinput(Axis);
			}
		}

<<<<<<< HEAD
		public class JoystickButton : IMyControllerControl
=======
		private class JoystickButton : IMyControllerControl
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public MyJoystickButtonsEnum Button;

			public byte Code => (byte)Button;

			public Func<bool> Condition { get; private set; }

			public JoystickButton(MyJoystickButtonsEnum button, Func<bool> condition = null)
			{
				Button = button;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = MyInput.Static.IsJoystickButtonNewPressed(Button);
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME);
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				return MyInput.Static.IsJoystickButtonPressed(Button);
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				return MyInput.Static.IsJoystickButtonNewReleased(Button);
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				return IsPressed() ? 1 : 0;
			}

			public object ControlCode()
			{
				return XBOX_BUTTONS_CODES[Button].ToString();
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}

			public bool IsNewPressedXinput()
			{
				return IsNewPressed();
			}

			public bool IsNewReleasedXinput()
			{
				return IsNewReleased();
			}
		}

		/// <summary>
		/// Loading hints require Controls for displaying, but some things cannot be binded to (any direction on Stick or DPad). For this reason fake controls exist, to be able to display control elements That cannot even have binding
		/// </summary>
<<<<<<< HEAD
		public class FakeControl : IMyControllerControl
=======
		private class FakeControl : IMyControllerControl
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			public string m_fakeCode;

			public byte Code => 0;

			public Func<bool> Condition { get; }

			public FakeControl(string fakecode)
			{
				m_fakeCode = fakecode;
			}

			public float AnalogValue()
			{
				return 0f;
			}

			public object ControlCode()
			{
				return m_fakeCode;
			}

			public bool IsNewPressed()
			{
				return false;
			}

			public bool IsNewPressedRepeating()
			{
				return false;
			}

			public bool IsNewReleased()
			{
				return false;
			}

			public bool IsPressed()
			{
				return false;
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}

			public bool IsNewPressedXinput()
			{
				return IsNewPressed();
			}

			public bool IsNewReleasedXinput()
			{
				return IsNewReleased();
			}
		}

<<<<<<< HEAD
		public class JoystickPressedModifier : IMyControllerControl
=======
		private class JoystickPressedModifier : IMyControllerControl
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public IMyControllerControl Modifier;

			public IMyControllerControl Control;

			public byte Code => Control.Code;

			public Func<bool> Condition { get; private set; }

			public JoystickPressedModifier(IMyControllerControl modifier, IMyControllerControl control, Func<bool> condition = null)
			{
				Modifier = modifier;
				Control = control;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = Modifier.IsPressed() && Control.IsNewPressed();
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME);
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (Modifier.IsPressed())
				{
					return Control.IsPressed();
				}
				return false;
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (!Modifier.IsNewReleased() || !Control.IsPressed())
				{
					if (Modifier.IsPressed())
					{
						return Control.IsNewReleased();
					}
					return false;
				}
				return true;
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				if (!Modifier.IsPressed())
				{
					return 0f;
				}
				return Control.AnalogValue();
			}

			public object ControlCode()
			{
				return string.Concat(Modifier.ControlCode(), " + ", Control.ControlCode());
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}

			public bool IsNewPressedXinput()
			{
				return IsNewPressed();
			}

			public bool IsNewReleasedXinput()
			{
				return IsNewReleased();
			}
		}

<<<<<<< HEAD
		public class JoystickReleasedModifier : IMyControllerControl
=======
		private class JoystickReleasedModifier : IMyControllerControl
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public IMyControllerControl Modifier;

			public IMyControllerControl Control;

			public byte Code => Control.Code;

			public Func<bool> Condition { get; private set; }

			public JoystickReleasedModifier(IMyControllerControl modifier, IMyControllerControl control, Func<bool> condition = null)
			{
				Modifier = modifier;
				Control = control;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = !Modifier.IsPressed() && Control.IsNewPressed();
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME);
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (!Modifier.IsPressed())
				{
					return Control.IsPressed();
				}
				return false;
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (!Control.IsPressed() || !Modifier.IsNewPressed())
				{
					if (Control.IsNewReleased())
					{
						return !Modifier.IsPressed();
					}
					return false;
				}
				return true;
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				if (Modifier.IsPressed())
				{
					return 0f;
				}
				return Control.AnalogValue();
			}

			public object ControlCode()
			{
				return Control.ControlCode();
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}

			public bool IsNewPressedXinput()
			{
				return IsNewPressed();
			}

			public bool IsNewReleasedXinput()
			{
				return IsNewReleased();
			}
		}

<<<<<<< HEAD
		public class JoystickPressedTwoModifiers : IMyControllerControl
=======
		private class JoystickPressedTwoModifiers : IMyControllerControl
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public IMyControllerControl Modifier1;

			public IMyControllerControl Modifier2;

			public IMyControllerControl Control;

			public byte Code => Control.Code;

			public Func<bool> Condition { get; private set; }

			public JoystickPressedTwoModifiers(IMyControllerControl modifier1, IMyControllerControl modifier2, IMyControllerControl control, Func<bool> condition = null)
			{
				Modifier1 = modifier1;
				Modifier2 = modifier2;
				Control = control;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = Modifier1.IsPressed() && Modifier2.IsPressed() && Control.IsNewPressed();
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME);
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (Modifier1.IsPressed() && Modifier2.IsPressed())
				{
					return Control.IsPressed();
				}
				return false;
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if ((!Modifier1.IsNewReleased() || !Modifier2.IsPressed() || !Control.IsPressed()) && (!Modifier1.IsPressed() || !Modifier2.IsNewReleased() || !Control.IsPressed()))
				{
					if (Modifier1.IsPressed() && Modifier2.IsPressed())
					{
						return Control.IsNewReleased();
					}
					return false;
				}
				return true;
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				if (!Modifier1.IsPressed() || !Modifier2.IsPressed())
				{
					return 0f;
				}
				return Control.AnalogValue();
			}

			public object ControlCode()
			{
				return string.Concat(Modifier1.ControlCode(), " + ", Modifier2.ControlCode(), " + ", Control.ControlCode());
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}

			public bool IsNewPressedXinput()
			{
				return IsNewPressed();
			}

			public bool IsNewReleasedXinput()
			{
				return IsNewReleased();
			}
		}

<<<<<<< HEAD
		public class JoystickReleasedTwoModifiers : IMyControllerControl
=======
		private class JoystickReleasedTwoModifiers : IMyControllerControl
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public IMyControllerControl Modifier1;

			public IMyControllerControl Modifier2;

			public IMyControllerControl Control;

			public byte Code => Control.Code;

			public Func<bool> Condition { get; private set; }

			public JoystickReleasedTwoModifiers(IMyControllerControl modifier1, IMyControllerControl modifier2, IMyControllerControl control, Func<bool> condition = null)
			{
				Modifier1 = modifier1;
				Modifier2 = modifier2;
				Control = control;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = !Modifier1.IsPressed() && !Modifier2.IsPressed() && Control.IsNewPressed();
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME);
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (!Modifier1.IsPressed() && !Modifier2.IsPressed())
				{
					return Control.IsPressed();
				}
				return false;
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (!Control.IsPressed() || (!Modifier1.IsNewPressed() && !Modifier2.IsNewPressed()))
				{
					if (Control.IsNewReleased() && !Modifier1.IsPressed())
					{
						return !Modifier2.IsPressed();
					}
					return false;
				}
				return true;
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				if (Modifier1.IsPressed() || Modifier2.IsPressed())
				{
					return 0f;
				}
				return Control.AnalogValue();
			}

			public object ControlCode()
			{
				return Control.ControlCode();
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}

			public bool IsNewPressedXinput()
			{
				return IsNewPressed();
			}

			public bool IsNewReleasedXinput()
			{
				return IsNewReleased();
			}
<<<<<<< HEAD
		}

		public class JoystickPressedThreeModifiers : IMyControllerControl
=======
		}

		private class JoystickPressedThreeModifiers : IMyControllerControl
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public IMyControllerControl Modifier1;

			public IMyControllerControl Modifier2;

			public IMyControllerControl Modifier3;

			public IMyControllerControl Control;

			public byte Code => Control.Code;

			public Func<bool> Condition { get; private set; }

			public JoystickPressedThreeModifiers(IMyControllerControl modifier1, IMyControllerControl modifier2, IMyControllerControl modifier3, IMyControllerControl control, Func<bool> condition = null)
			{
				Modifier1 = modifier1;
				Modifier2 = modifier2;
				Modifier3 = modifier3;
				Control = control;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = Modifier1.IsPressed() && Modifier2.IsPressed() && Modifier3.IsPressed() && Control.IsNewPressed();
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME);
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (Modifier1.IsPressed() && Modifier2.IsPressed() && Modifier3.IsPressed())
				{
					return Control.IsPressed();
				}
				return false;
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if ((!Modifier1.IsNewReleased() || !Modifier2.IsPressed() || !Modifier3.IsPressed() || !Control.IsPressed()) && (!Modifier1.IsPressed() || !Modifier2.IsNewReleased() || !Modifier3.IsPressed() || !Control.IsPressed()) && (!Modifier1.IsPressed() || !Modifier2.IsPressed() || !Modifier3.IsNewReleased() || !Control.IsPressed()))
				{
					if (Modifier1.IsPressed() && Modifier2.IsPressed() && Modifier3.IsPressed())
					{
						return Control.IsNewReleased();
					}
					return false;
				}
				return true;
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				if (!Modifier1.IsPressed() || !Modifier2.IsPressed() || !Modifier3.IsPressed())
				{
					return 0f;
				}
				return Control.AnalogValue();
			}

			public object ControlCode()
			{
				return string.Concat(Modifier1.ControlCode(), " + ", Modifier2.ControlCode(), " + ", Control.ControlCode());
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}

			public bool IsNewPressedXinput()
			{
				return IsNewPressed();
			}

			public bool IsNewReleasedXinput()
			{
				return IsNewReleased();
			}
		}

		private class JoystickReleasedThreeModifiers : IMyControllerControl
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public IMyControllerControl Modifier1;
<<<<<<< HEAD

			public IMyControllerControl Modifier2;

			public IMyControllerControl Modifier3;

=======

			public IMyControllerControl Modifier2;

			public IMyControllerControl Modifier3;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public IMyControllerControl Control;

			public byte Code => Control.Code;

			public Func<bool> Condition { get; private set; }

<<<<<<< HEAD
			public JoystickPressedThreeModifiers(IMyControllerControl modifier1, IMyControllerControl modifier2, IMyControllerControl modifier3, IMyControllerControl control, Func<bool> condition = null)
=======
			public JoystickReleasedThreeModifiers(IMyControllerControl modifier1, IMyControllerControl modifier2, IMyControllerControl modifier3, IMyControllerControl control, Func<bool> condition = null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Modifier1 = modifier1;
				Modifier2 = modifier2;
				Modifier3 = modifier3;
				Control = control;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
<<<<<<< HEAD
				bool num = Modifier1.IsPressed() && Modifier2.IsPressed() && Modifier3.IsPressed() && Control.IsNewPressed();
=======
				bool num = !Modifier1.IsPressed() && !Modifier2.IsPressed() && !Modifier3.IsPressed() && Control.IsNewPressed();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
<<<<<<< HEAD
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME);
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (Modifier1.IsPressed() && Modifier2.IsPressed() && Modifier3.IsPressed())
				{
					return Control.IsPressed();
				}
				return false;
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if ((!Modifier1.IsNewReleased() || !Modifier2.IsPressed() || !Modifier3.IsPressed() || !Control.IsPressed()) && (!Modifier1.IsPressed() || !Modifier2.IsNewReleased() || !Modifier3.IsPressed() || !Control.IsPressed()) && (!Modifier1.IsPressed() || !Modifier2.IsPressed() || !Modifier3.IsNewReleased() || !Control.IsPressed()))
				{
					if (Modifier1.IsPressed() && Modifier2.IsPressed() && Modifier3.IsPressed())
					{
						return Control.IsNewReleased();
					}
					return false;
				}
				return true;
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				if (!Modifier1.IsPressed() || !Modifier2.IsPressed() || !Modifier3.IsPressed())
				{
					return 0f;
				}
				return Control.AnalogValue();
			}

			public object ControlCode()
			{
				return string.Concat(Modifier1.ControlCode(), " + ", Modifier2.ControlCode(), " + ", Control.ControlCode());
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}

			public bool IsNewPressedXinput()
			{
				return IsNewPressed();
			}

			public bool IsNewReleasedXinput()
			{
				return IsNewReleased();
			}
		}

		public class JoystickReleasedThreeModifiers : IMyControllerControl
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public IMyControllerControl Modifier1;

			public IMyControllerControl Modifier2;

			public IMyControllerControl Modifier3;

			public IMyControllerControl Control;

			public byte Code => Control.Code;

			public Func<bool> Condition { get; private set; }

			public JoystickReleasedThreeModifiers(IMyControllerControl modifier1, IMyControllerControl modifier2, IMyControllerControl modifier3, IMyControllerControl control, Func<bool> condition = null)
			{
				Modifier1 = modifier1;
				Modifier2 = modifier2;
				Modifier3 = modifier3;
				Control = control;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = !Modifier1.IsPressed() && !Modifier2.IsPressed() && !Modifier3.IsPressed() && Control.IsNewPressed();
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME);
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

=======
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME);
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (!Modifier1.IsPressed() && !Modifier2.IsPressed() && !Modifier3.IsPressed())
				{
					return Control.IsPressed();
				}
				return false;
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (!Control.IsPressed() || (!Modifier1.IsNewPressed() && !Modifier2.IsNewPressed() && !Modifier3.IsNewPressed()))
				{
					if (Control.IsNewReleased() && !Modifier1.IsPressed() && !Modifier2.IsPressed())
					{
						return !Modifier3.IsPressed();
					}
					return false;
				}
				return true;
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				if (Modifier1.IsPressed() || Modifier2.IsPressed() || Modifier3.IsPressed())
				{
					return 0f;
				}
				return Control.AnalogValue();
			}

			public object ControlCode()
			{
				return Control.ControlCode();
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}

			public bool IsNewPressedXinput()
			{
				return IsNewPressed();
			}

			public bool IsNewReleasedXinput()
			{
				return IsNewReleased();
			}
		}

<<<<<<< HEAD
		public class JoystickOneOfTwoModifiers : IMyControllerControl
=======
		private class JoystickOneOfTwoModifiers : IMyControllerControl
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public IMyControllerControl PressedModifier;

			public IMyControllerControl ReleasedModifier;

			public IMyControllerControl Control;

			public byte Code => Control.Code;

			public Func<bool> Condition { get; private set; }

			public JoystickOneOfTwoModifiers(IMyControllerControl pressedModifier, IMyControllerControl releasedModifier, IMyControllerControl control, Func<bool> condition = null)
			{
				PressedModifier = pressedModifier;
				ReleasedModifier = releasedModifier;
				Control = control;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = Control.IsNewPressed() && PressedModifier.IsPressed() && !ReleasedModifier.IsPressed();
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME);
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (PressedModifier.IsPressed() && !ReleasedModifier.IsPressed())
				{
					return Control.IsPressed();
				}
				return false;
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (!PressedModifier.IsNewReleased() && !ReleasedModifier.IsNewPressed())
				{
					return Control.IsNewReleased();
				}
				return true;
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				if (!PressedModifier.IsPressed() || ReleasedModifier.IsPressed())
				{
					return 0f;
				}
				return Control.AnalogValue();
			}

			public object ControlCode()
			{
				return string.Concat(PressedModifier.ControlCode(), " + ", Control.ControlCode());
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}

			public bool IsNewPressedXinput()
			{
				return IsNewPressed();
			}

			public bool IsNewReleasedXinput()
			{
				return IsNewReleased();
			}
		}

		private static readonly Dictionary<MyJoystickAxesEnum, char> XBOX_AXES_CODES;

		private static readonly Dictionary<MyJoystickButtonsEnum, char> XBOX_BUTTONS_CODES;

		public static readonly MyStringId CX_BASE;

		public static readonly MyStringId CX_GUI;

		public static readonly MyStringId CX_CHARACTER;

<<<<<<< HEAD
		public static readonly MyStringId CX_SPACESHIP;

		public static readonly MyStringId CX_JETPACK;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <summary>
		/// Evaluator that can be used to replace button names with their icon.
		/// </summary>
		public static readonly ITextEvaluator ButtonTextEvaluator;

		public static float GAMEPAD_ANALOG_SCROLL_SPEED;

		public const float JOYSTICK_CARSTEERINGXYAXISPERCENTAGE_DEFAULT = 0.9f;

		public const float JOYSTICK_REVERSEPOINT_DEFAULT = 0.35f;

		private static readonly TimeSpan REPEAT_START_TIME;

		private static readonly TimeSpan REPEAT_TIME;

		/// <summary>
		/// if current axis is being stretched in negative direction m_isJoystickYAxisState_Reversing.value == true
		/// </summary>
		private static bool? m_isJoystickYAxisState_Reversing;

		private static EmptyControl m_nullControl;

		private static Dictionary<MyStringId, Context> m_bindings;

<<<<<<< HEAD
		public static Dictionary<MyStringId, Context> GetAll()
		{
			return m_bindings;
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		static MyControllerHelper()
		{
			XBOX_AXES_CODES = new Dictionary<MyJoystickAxesEnum, char>
			{
				{
					MyJoystickAxesEnum.Xneg,
					'\ue016'
				},
				{
					MyJoystickAxesEnum.Xpos,
					'\ue015'
				},
				{
					MyJoystickAxesEnum.Ypos,
					'\ue014'
				},
				{
					MyJoystickAxesEnum.Yneg,
					'\ue017'
				},
				{
					MyJoystickAxesEnum.RotationXneg,
					'\ue020'
				},
				{
					MyJoystickAxesEnum.RotationXpos,
					'\ue019'
				},
				{
					MyJoystickAxesEnum.RotationYneg,
					'\ue021'
				},
				{
					MyJoystickAxesEnum.RotationYpos,
					'\ue018'
				},
				{
					MyJoystickAxesEnum.Zneg,
					'\ue007'
				},
				{
					MyJoystickAxesEnum.Zpos,
					'\ue008'
				}
			};
			XBOX_BUTTONS_CODES = new Dictionary<MyJoystickButtonsEnum, char>
			{
				{
					MyJoystickButtonsEnum.J01,
					'\ue001'
				},
				{
					MyJoystickButtonsEnum.J02,
					'\ue003'
				},
				{
					MyJoystickButtonsEnum.J03,
					'\ue002'
				},
				{
					MyJoystickButtonsEnum.J04,
					'\ue004'
				},
				{
					MyJoystickButtonsEnum.J05,
					'\ue005'
				},
				{
					MyJoystickButtonsEnum.J06,
					'\ue006'
				},
				{
					MyJoystickButtonsEnum.J07,
					'\ue00d'
				},
				{
					MyJoystickButtonsEnum.J08,
					'\ue00e'
				},
				{
					MyJoystickButtonsEnum.J09,
					'\ue00b'
				},
				{
					MyJoystickButtonsEnum.J10,
					'\ue00c'
				},
				{
					MyJoystickButtonsEnum.JDLeft,
					'\ue010'
				},
				{
					MyJoystickButtonsEnum.JDUp,
					'\ue011'
				},
				{
					MyJoystickButtonsEnum.JDRight,
					'\ue012'
				},
				{
					MyJoystickButtonsEnum.JDDown,
					'\ue013'
				},
				{
					MyJoystickButtonsEnum.J11,
					'\ue007'
				},
				{
					MyJoystickButtonsEnum.J12,
					'\ue008'
				}
			};
			CX_BASE = MyStringId.GetOrCompute("BASE");
			CX_GUI = MyStringId.GetOrCompute("GUI");
			CX_CHARACTER = MyStringId.GetOrCompute("CHARACTER");
<<<<<<< HEAD
			CX_SPACESHIP = MyStringId.GetOrCompute("SPACESHIP");
			CX_JETPACK = MyStringId.GetOrCompute("JETPACK");
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ButtonTextEvaluator = new ButtonEvaluator();
			GAMEPAD_ANALOG_SCROLL_SPEED = 0.08f;
			REPEAT_START_TIME = new TimeSpan(0, 0, 0, 0, 500);
			REPEAT_TIME = new TimeSpan(0, 0, 0, 0, 100);
			m_nullControl = new EmptyControl();
			m_bindings = new Dictionary<MyStringId, Context>(MyStringId.Comparer);
			Initialize();
		}

		private static void Initialize()
		{
			m_bindings.Add(MyStringId.NullOrEmpty, new Context());
		}

		public static void AddContext(MyStringId context, MyStringId? parent = null)
		{
			if (!m_bindings.ContainsKey(context))
			{
				Context context2 = new Context();
				m_bindings.Add(context, context2);
				if (parent.HasValue && m_bindings.ContainsKey(parent.Value))
				{
					context2.ParentContext = m_bindings[parent.Value];
				}
			}
		}

		public static void AddControl(MyStringId context, MyStringId stringId, string fakeCode)
		{
			m_bindings[context][stringId] = new FakeControl(fakeCode);
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickAxesEnum axis, Func<bool> condition = null)
		{
			m_bindings[context][stringId] = new JoystickAxis(axis, condition);
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum button, Func<bool> condition = null)
		{
			m_bindings[context][stringId] = new JoystickButton(button, condition);
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum modifier, MyJoystickButtonsEnum control, bool pressed, Func<bool> condition = null)
		{
			if (pressed)
			{
				m_bindings[context][stringId] = new JoystickPressedModifier(new JoystickButton(modifier), new JoystickButton(control), condition);
			}
			else
			{
				m_bindings[context][stringId] = new JoystickReleasedModifier(new JoystickButton(modifier), new JoystickButton(control), condition);
			}
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum modifier, MyJoystickAxesEnum control, bool pressed, Func<bool> condition = null)
		{
			if (pressed)
			{
				m_bindings[context][stringId] = new JoystickPressedModifier(new JoystickButton(modifier), new JoystickAxis(control), condition);
			}
			else
			{
				m_bindings[context][stringId] = new JoystickReleasedModifier(new JoystickButton(modifier), new JoystickAxis(control), condition);
			}
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum modifier1, MyJoystickButtonsEnum modifier2, MyJoystickButtonsEnum control, bool pressed, Func<bool> condition = null)
		{
			if (pressed)
			{
				m_bindings[context][stringId] = new JoystickPressedTwoModifiers(new JoystickButton(modifier1), new JoystickButton(modifier2), new JoystickButton(control), condition);
			}
			else
			{
				m_bindings[context][stringId] = new JoystickReleasedTwoModifiers(new JoystickButton(modifier1), new JoystickButton(modifier2), new JoystickButton(control), condition);
			}
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum modifier1, MyJoystickButtonsEnum modifier2, MyJoystickButtonsEnum modifier3, MyJoystickButtonsEnum control, bool pressed, Func<bool> condition = null)
		{
			if (pressed)
			{
				m_bindings[context][stringId] = new JoystickPressedThreeModifiers(new JoystickButton(modifier1), new JoystickButton(modifier2), new JoystickButton(modifier3), new JoystickButton(control), condition);
			}
			else
			{
				m_bindings[context][stringId] = new JoystickReleasedThreeModifiers(new JoystickButton(modifier1), new JoystickButton(modifier2), new JoystickButton(modifier3), new JoystickButton(control), condition);
			}
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum modifier1, MyJoystickButtonsEnum modifier2, MyJoystickAxesEnum control, bool pressed, Func<bool> condition = null)
		{
			if (pressed)
			{
				m_bindings[context][stringId] = new JoystickPressedTwoModifiers(new JoystickButton(modifier1), new JoystickButton(modifier2), new JoystickAxis(control), condition);
			}
			else
			{
				m_bindings[context][stringId] = new JoystickReleasedTwoModifiers(new JoystickButton(modifier1), new JoystickButton(modifier2), new JoystickAxis(control), condition);
			}
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum modifier1, MyJoystickButtonsEnum modifier2, MyJoystickButtonsEnum modifier3, MyJoystickAxesEnum control, bool pressed, Func<bool> condition = null)
		{
			if (!m_bindings.ContainsKey(context))
			{
				AddContext(context);
			}
			if (pressed)
			{
				m_bindings[context][stringId] = new JoystickPressedThreeModifiers(new JoystickButton(modifier1), new JoystickButton(modifier2), new JoystickButton(modifier3), new JoystickAxis(control), condition);
			}
			else
			{
				m_bindings[context][stringId] = new JoystickReleasedThreeModifiers(new JoystickButton(modifier1), new JoystickButton(modifier2), new JoystickButton(modifier3), new JoystickAxis(control), condition);
			}
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum pressedModifier, MyJoystickButtonsEnum releasedModifier, MyJoystickAxesEnum control, Func<bool> condition = null)
		{
			m_bindings[context][stringId] = new JoystickOneOfTwoModifiers(new JoystickButton(pressedModifier), new JoystickButton(releasedModifier), new JoystickAxis(control), condition);
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum pressedModifier, MyJoystickButtonsEnum releasedModifier, MyJoystickButtonsEnum control, Func<bool> condition = null)
		{
			m_bindings[context][stringId] = new JoystickOneOfTwoModifiers(new JoystickButton(pressedModifier), new JoystickButton(releasedModifier), new JoystickButton(control), condition);
		}

		public static void NullControl(MyStringId context, MyStringId stringId)
		{
			m_bindings[context][stringId] = m_nullControl;
		}

		public static void NullControl(MyStringId context, MyJoystickAxesEnum axis)
		{
			MyStringId myStringId = MyStringId.NullOrEmpty;
			foreach (KeyValuePair<MyStringId, IMyControllerControl> binding in m_bindings[context].Bindings)
			{
				if (binding.Value is JoystickAxis && (uint)binding.Value.Code == (uint)axis)
				{
					myStringId = binding.Key;
					break;
				}
			}
			if (myStringId != MyStringId.NullOrEmpty)
			{
				m_bindings[context][myStringId] = m_nullControl;
			}
		}

		public static bool IsNullControl(IMyControllerControl control)
		{
			return control == m_nullControl;
		}

		public static IMyControllerControl GetNullControl()
		{
			return m_nullControl;
		}

		public static void NullControl(MyStringId context, MyJoystickButtonsEnum button)
		{
			MyStringId myStringId = MyStringId.NullOrEmpty;
			foreach (KeyValuePair<MyStringId, IMyControllerControl> binding in m_bindings[context].Bindings)
			{
				if (binding.Value is JoystickButton && (uint)binding.Value.Code == (uint)button)
				{
					myStringId = binding.Key;
					break;
				}
			}
			if (myStringId != MyStringId.NullOrEmpty)
			{
				m_bindings[context][myStringId] = m_nullControl;
			}
		}

		public static bool IsControl(MyStringId context, MyStringId stringId, MyControlStateType type = MyControlStateType.NEW_PRESSED, bool joystickOnly = false, bool useXinput = false)
		{
			IMyControllerControl myControllerControl = m_bindings[context][stringId];
			if (useXinput && (myControllerControl.Code == 6 || myControllerControl.Code == 5) && (type == MyControlStateType.NEW_PRESSED || type == MyControlStateType.NEW_RELEASED))
			{
				switch (type)
				{
				case MyControlStateType.NEW_PRESSED:
					if (joystickOnly || !MyInput.Static.IsNewGameControlPressed(stringId))
					{
						return myControllerControl.IsNewPressedXinput();
					}
					return true;
				case MyControlStateType.NEW_RELEASED:
					if (joystickOnly || !MyInput.Static.IsNewGameControlReleased(stringId))
					{
						return myControllerControl.IsNewReleasedXinput();
					}
					return true;
				}
			}
			switch (type)
			{
			case MyControlStateType.NEW_PRESSED:
				if (joystickOnly || !MyInput.Static.IsNewGameControlPressed(stringId))
				{
					return myControllerControl.IsNewPressed();
				}
				return true;
			case MyControlStateType.NEW_RELEASED:
				if (joystickOnly || !MyInput.Static.IsNewGameControlReleased(stringId))
				{
					return myControllerControl.IsNewReleased();
				}
				return true;
			case MyControlStateType.PRESSED:
				if (joystickOnly || !MyInput.Static.IsGameControlPressed(stringId))
				{
					return myControllerControl.IsPressed();
				}
				return true;
			case MyControlStateType.NEW_PRESSED_REPEATING:
				if (joystickOnly || !MyInput.Static.IsNewGameControlPressed(stringId))
				{
					return myControllerControl.IsNewPressedRepeating();
				}
				return true;
			default:
				return false;
			}
		}

		public static float IsControlAnalog(MyStringId context, MyStringId stringId, bool gamepadShipControl = false)
		{
			if (gamepadShipControl)
			{
				return MyInput.Static.GetGameControlAnalogState(stringId) + GetJoystickAxisStateForShipGameplay((MyJoystickAxesEnum)m_bindings[context][stringId].Code);
			}
			return MyInput.Static.GetGameControlAnalogState(stringId) + m_bindings[context][stringId].AnalogValue();
		}

		public static bool IsDefined(MyStringId contextId, MyStringId controlId)
		{
			if (m_bindings.TryGetValue(contextId, out var value))
			{
				return value[controlId] != m_nullControl;
			}
			return false;
		}

		public static string GetCodeForControl(MyStringId context, MyStringId stringId)
		{
			return (string)m_bindings.GetValueOrDefault(context, Context.Empty)[stringId].ControlCode();
		}

		public static IMyControllerControl GetControl(MyStringId context, MyStringId stringId)
		{
			return m_bindings[context][stringId];
		}

		public static IMyControllerControl TryGetControl(MyStringId context, MyStringId stringId)
		{
			if (!m_bindings.ContainsKey(context))
			{
				return null;
			}
			return m_bindings[context][stringId];
		}

		public static void ClearBindings()
		{
			m_bindings.Clear();
			Initialize();
		}

		public static float GetJoystickAxisStateForShipGameplay(MyJoystickAxesEnum axis)
		{
			if (MyInput.Static.GetType() != typeof(MyVRageInput))
			{
				return 0f;
			}
			MyVRageInput myVRageInput = (MyVRageInput)MyInput.Static;
			if (axis != MyJoystickAxesEnum.Yneg && axis != MyJoystickAxesEnum.Ypos)
			{
				return myVRageInput.GetJoystickAxisStateForGameplay(axis);
			}
			float num = 6553.50146f;
			int num2 = (int)myVRageInput.GetJoystickAxisStateRaw(MyJoystickAxesEnum.Xneg);
			int num3 = (int)myVRageInput.GetJoystickAxisStateRaw(MyJoystickAxesEnum.Yneg);
			if (!((float)num2 <= num) && !((float)num2 >= 65535f - num) && !((float)num3 <= num) && !((float)num3 >= 65535f - num))
			{
				return myVRageInput.GetJoystickAxisStateForGameplay(axis);
			}
			if (myVRageInput.IsJoystickLastUsed && myVRageInput.IsJoystickAxisSupported(axis))
			{
				int num4 = (int)myVRageInput.GetJoystickAxisStateRaw(axis);
				int num5 = (int)(65535f * myVRageInput.GetJoystickDeadzone());
				int num6 = 32767 - num5 / 2;
				int num7 = 32767 + num5 / 2;
				if (num4 > num6 && num4 < num7)
				{
					int num8 = (int)myVRageInput.GetJoystickAxisStateRaw(MyJoystickAxesEnum.Xneg);
					if (num8 > num6 && num8 < num7)
					{
						if (m_isJoystickYAxisState_Reversing.HasValue)
						{
							m_isJoystickYAxisState_Reversing = null;
						}
						return 0f;
					}
				}
				if (!m_isJoystickYAxisState_Reversing.HasValue)
				{
					if (num4 >= 32767)
					{
						m_isJoystickYAxisState_Reversing = true;
					}
					else
					{
						m_isJoystickYAxisState_Reversing = false;
					}
				}
				float num9 = 65535f;
				float num10 = 22937.25f;
				float num11 = 65535f - num9;
				float num12 = 65535f - num10;
				float num13 = num12 - num11;
				float num14 = 0f;
				if ((m_isJoystickYAxisState_Reversing.Value && (float)num4 >= num9) || (!m_isJoystickYAxisState_Reversing.Value && (float)num4 <= num11))
				{
					num14 = 1f;
				}
				else
				{
					if ((m_isJoystickYAxisState_Reversing.Value && (float)num4 < num10) || (!m_isJoystickYAxisState_Reversing.Value && (float)num4 > num12))
					{
						m_isJoystickYAxisState_Reversing = !m_isJoystickYAxisState_Reversing.Value;
						return GetJoystickAxisStateForShipGameplay(axis);
					}
					float num15 = 0f;
					if (axis == MyJoystickAxesEnum.Yneg)
					{
						Math.Abs(num11 - (float)num4);
						num15 = Math.Abs(num12 - (float)num4);
					}
					else
					{
						Math.Abs(num9 - (float)num4);
						num15 = Math.Abs(num10 - (float)num4);
					}
					num14 = num15 / (num13 / 100f) / 100f;
					if (num14 > 1f)
					{
						num14 = 1f;
					}
				}
				if ((axis == MyJoystickAxesEnum.Yneg && m_isJoystickYAxisState_Reversing.Value) || (axis == MyJoystickAxesEnum.Ypos && !m_isJoystickYAxisState_Reversing.Value))
				{
					return 0f;
				}
				return myVRageInput.GetJoystickSensitivity() * (float)Math.Pow(num14, myVRageInput.GetJoystickExponent());
			}
			return 0f;
		}
	}
}
