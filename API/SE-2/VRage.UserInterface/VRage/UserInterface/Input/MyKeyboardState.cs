using EmptyKeys.UserInterface.Input;
using VRage.Input;

namespace VRage.UserInterface.Input
{
	public class MyKeyboardState : KeyboardStateBase
	{
		public override bool IsKeyPressed(KeyCode keyCode)
		{
			MyKeys key = (MyKeys)keyCode;
			return MyInput.Static.IsKeyPress(key);
		}

		public override void Update()
		{
		}
	}
}
