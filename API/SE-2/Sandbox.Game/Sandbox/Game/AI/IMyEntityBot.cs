using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.AI
{
	public interface IMyEntityBot : IMyBot
	{
		MyEntity BotEntity { get; }

		void Spawn(Vector3D? spawnPosition, Vector3? direction, Vector3? up, bool spawnedByPlayer);
	}
}
