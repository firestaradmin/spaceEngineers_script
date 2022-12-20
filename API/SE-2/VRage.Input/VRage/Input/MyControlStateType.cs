namespace VRage.Input
{
	/// <summary>
	/// Defines states of the control, newly means that control just changed to that state and it will change to next state right after
	/// </summary>
	public enum MyControlStateType
	{
		/// <summary>
		/// When control is newly pressed
		/// </summary>
		NEW_PRESSED,
		/// <summary>
		/// When control is pressed
		/// </summary>
		PRESSED,
		/// <summary>
		/// When control is newly released
		/// </summary>
		NEW_RELEASED,
		/// <summary>
		/// When control is newly pressed with repeat feature
		/// </summary>
		NEW_PRESSED_REPEATING
	}
}
