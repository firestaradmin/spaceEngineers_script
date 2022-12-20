using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { true, true }, null)]
	public delegate void BlockDamagedEvent(string entityName, string gridName, string typeId, string subtypeId, float damage, string damageType, long attackerId);
}
