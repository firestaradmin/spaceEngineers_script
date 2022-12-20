using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { false, false }, null)]
	public delegate void MatchStateEndingEvent(string stateCurrentName, ref bool interruptStateChange);
}
