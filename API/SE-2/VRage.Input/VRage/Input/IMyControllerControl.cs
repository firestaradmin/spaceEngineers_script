using System;

namespace VRage.Input
{
	/// <summary>
	/// Describes interface of the control
	/// </summary>
	public interface IMyControllerControl
	{
		/// <summary>
		/// Code of the control
		/// </summary>
		byte Code { get; }

		/// <summary>
		/// Condition if control should be active
		/// </summary>
		Func<bool> Condition { get; }

		/// <summary>
		/// Checks if control is newly pressed
		/// </summary>
		/// <returns></returns>
		bool IsNewPressed();

		/// <summary>
		/// Checks if control is newly pressed with repeat feature
		/// </summary>
		/// <returns></returns>
		bool IsNewPressedRepeating();

		/// <summary>
		/// Checks if control is pressed
		/// </summary>
		/// <returns></returns>
		bool IsPressed();

		/// <summary>
		/// Checks if control is newly released
		/// </summary>
		/// <returns></returns>
		bool IsNewReleased();

		/// <summary>
		/// Gets analog value of the control
		/// </summary>
		/// <returns></returns>
		float AnalogValue();

		/// <summary>
		/// Gets control code
		/// </summary>
		/// <returns></returns>
		object ControlCode();

		/// <summary>
		/// Checks if control is newly pressed (works only for LT and RT buttons)
		/// </summary>
		/// <returns></returns>
		bool IsNewPressedXinput();

		/// <summary>
		/// Checks if control is newly released (works only for LT and RT buttons)
		/// </summary>
		/// <returns></returns>
		bool IsNewReleasedXinput();
	}
}
