using VRage.Game.VisualScripting;
using VRage.Network;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { false }, null)]
	public delegate void SingleKeyPlayerConnectRequestEvent(ulong steamId, ref JoinResult result);
}
