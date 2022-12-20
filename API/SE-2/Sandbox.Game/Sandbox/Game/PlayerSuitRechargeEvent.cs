using Sandbox.Game.GameSystems;
using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { false, false }, null)]
	public delegate void PlayerSuitRechargeEvent(long playerId, MyLifeSupportingBlockType blockType);
}
