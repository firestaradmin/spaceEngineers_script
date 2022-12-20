using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { true }, null)]
	public delegate void MatchStateStartedEvent(string stateName);
}
