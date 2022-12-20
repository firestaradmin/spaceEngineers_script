using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { true, true }, null)]
	public delegate void SingleKeyEntityNameGridNameEvent(string entityName, string gridName, string typeId, string subtypeId);
}
