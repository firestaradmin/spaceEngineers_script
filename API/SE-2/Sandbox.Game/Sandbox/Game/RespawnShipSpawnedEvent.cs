using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { true, false }, null)]
	public delegate void RespawnShipSpawnedEvent(long shipEntityId, long playerId, string respawnShipPrefabName);
}
