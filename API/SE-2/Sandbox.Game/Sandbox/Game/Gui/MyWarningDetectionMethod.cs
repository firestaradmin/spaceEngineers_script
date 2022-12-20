using VRage.Audio;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	/// <summary>
	/// Delegate of warning detection method
	/// </summary>
	/// <returns></returns>
	internal delegate bool MyWarningDetectionMethod(out MyGuiSounds cue, out MyStringId text);
}
