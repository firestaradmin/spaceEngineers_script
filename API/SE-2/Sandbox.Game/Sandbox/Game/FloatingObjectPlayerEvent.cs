using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { true, true, true, false, false }, null)]
	public delegate void FloatingObjectPlayerEvent(string itemTypeName, string itemSubTypeName, string entityName, long playerId, int amount);
}
