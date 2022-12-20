using Sandbox.Game.GameSystems;
using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { false, false, false }, null)]
	public delegate void PlayerHealthRechargeEvent(long playerId, MyLifeSupportingBlockType blockType, float value);
}
