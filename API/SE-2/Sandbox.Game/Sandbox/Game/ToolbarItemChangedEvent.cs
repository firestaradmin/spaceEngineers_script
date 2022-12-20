using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { true }, null)]
	public delegate void ToolbarItemChangedEvent(long entityId, string typeId, string subtypeId, int page, int slot);
}
