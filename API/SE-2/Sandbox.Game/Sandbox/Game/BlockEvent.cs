using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { true, true, true }, null)]
	public delegate void BlockEvent(string typeId, string subtypeId, string gridName, long blockId);
}
