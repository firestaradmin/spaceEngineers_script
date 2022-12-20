using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { true, true, false, false }, null)]
	public delegate void PlayerItemEvent(string itemTypeName, string itemSubTypeName, long playerId, int amount);
}
