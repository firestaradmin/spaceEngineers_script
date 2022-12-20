using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { true, true, true, true }, null)]
	public delegate void RoomFullyPressurizedEvent(long entityId, long gridId, string entityName, string gridName);
}
