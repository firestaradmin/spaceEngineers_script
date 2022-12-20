using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Input;
using VRage.Input;

namespace VRage.UserInterface.Input
{
	public class MyGamePadState : GamePadStateBase
	{
		public override PointF DPad
		{
			get
			{
				if (MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.JDUp))
				{
					return new PointF(0f, 1f);
				}
				if (MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.JDDown))
				{
					return new PointF(0f, -1f);
				}
				if (MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.JDLeft))
				{
					return new PointF(-1f, 0f);
				}
				if (MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.JDRight))
				{
					return new PointF(1f, 0f);
				}
				return default(PointF);
			}
		}

		public override bool IsAButtonPressed => MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.J01);

		public override bool IsBButtonPressed => MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.J02);

		public override bool IsCButtonPressed => MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.J03);

		public override bool IsDButtonPressed => MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.J04);

		public override bool IsLeftShoulderButtonPressed => MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.J05);

		public override bool IsLeftStickButtonPressed => MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.J09);

		public override bool IsRightShoulderButtonPressed => MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.J06);

		public override bool IsRightStickButtonPressed => MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.J10);

		public override bool IsSelectButtonPressed => MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.J08);

		public override bool IsStartButtonPressed => MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.J07);

		public override PointF LeftThumbStick
		{
			get
			{
				float joystickAxisStateForGameplay = MyInput.Static.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.Xneg);
				float joystickAxisStateForGameplay2 = MyInput.Static.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.Xpos);
				float joystickAxisStateForGameplay3 = MyInput.Static.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.Yneg);
				float joystickAxisStateForGameplay4 = MyInput.Static.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.Ypos);
				float x = joystickAxisStateForGameplay2 - joystickAxisStateForGameplay;
				float num = joystickAxisStateForGameplay4 - joystickAxisStateForGameplay3;
				return new PointF(x, 0f - num);
			}
		}

		public override float LeftTrigger => MyInput.Static.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.Zpos);

		public override int PlayerNumber => 0;

		public override PointF RightThumbStick
		{
			get
			{
				float joystickAxisStateForGameplay = MyInput.Static.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.RotationXneg);
				float joystickAxisStateForGameplay2 = MyInput.Static.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.RotationXpos);
				float joystickAxisStateForGameplay3 = MyInput.Static.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.RotationYneg);
				float joystickAxisStateForGameplay4 = MyInput.Static.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.RotationYpos);
				float x = joystickAxisStateForGameplay2 - joystickAxisStateForGameplay;
				float num = joystickAxisStateForGameplay4 - joystickAxisStateForGameplay3;
				return new PointF(x, 0f - num);
			}
		}

		public override float RightTrigger => MyInput.Static.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.Zneg);

		public override void Update(int gamePadIndex)
		{
		}
	}
}
