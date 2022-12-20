using System;
using EmptyKeys.UserInterface.Input;
using VRage.Input;
using VRageMath;

namespace VRage.UserInterface.Input
{
	public class MyMouseState : MouseStateBase
	{
		private Vector2 m_position;

		public override bool IsLeftButtonPressed => MyInput.Static.IsMousePressed(MyMouseButtonsEnum.Left);

		public override bool IsMiddleButtonPressed => MyInput.Static.IsMousePressed(MyMouseButtonsEnum.Middle);

		public override bool IsRightButtonPressed => MyInput.Static.IsMousePressed(MyMouseButtonsEnum.Right);

		public override bool IsVisible
		{
			get
			{
				return true;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override float NormalizedX
		{
			get
			{
				float x = MyInput.Static.GetMouseAreaSize().X;
				return m_position.X / x;
			}
		}

		public override float NormalizedY
		{
			get
			{
				float y = MyInput.Static.GetMouseAreaSize().Y;
				return m_position.Y / y;
			}
		}

		public override int ScrollWheelValue => MyInput.Static.MouseScrollWheelValue();

		public override void SetPosition(int x, int y)
		{
			MyInput.Static.SetMousePosition(x, y);
		}

		public override void Update()
		{
			m_position = MyInput.Static.GetMousePosition();
		}
	}
}
