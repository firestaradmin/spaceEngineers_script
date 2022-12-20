namespace VRage.Input
{
	/// <summary>
	/// Defines joystick/gamepad axes
	/// </summary>
	public enum MyJoystickAxesEnum : byte
	{
		/// <summary>
		/// None
		/// </summary>
		None,
		/// <summary>
		/// Left stick right
		/// </summary>
		Xpos,
		/// <summary>
		/// Left stick left
		/// </summary>
		Xneg,
		/// <summary>
		/// Left stick down
		/// </summary>
		Ypos,
		/// <summary>
		/// Left stick up
		/// </summary>
		Yneg,
		/// <summary>
		/// Left trigger
		/// </summary>
		Zpos,
		/// <summary>
		/// Right trigger
		/// </summary>
		Zneg,
		/// <summary>
		/// Right stick right
		/// </summary>
		RotationXpos,
		/// <summary>
		/// Right stick left
		/// </summary>
		RotationXneg,
		/// <summary>
		/// Right stick down
		/// </summary>
		RotationYpos,
		/// <summary>
		/// Right stick up
		/// </summary>
		RotationYneg,
		RotationZpos,
		RotationZneg,
		Slider1pos,
		Slider1neg,
		Slider2pos,
		Slider2neg,
		/// <summary>
		/// Left trigger - using Xinput
		/// </summary>
		ZLeft,
		/// <summary>
		/// Right trigger - using Xinput
		/// </summary>
		ZRight
	}
}
