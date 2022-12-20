using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { false }, null)]
	public delegate void TeamBalancerSortEvent(long playerId, string factionTag);
}
