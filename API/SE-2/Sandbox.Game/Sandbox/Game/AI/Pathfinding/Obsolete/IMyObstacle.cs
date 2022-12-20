using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
	public interface IMyObstacle
	{
		bool Contains(ref Vector3D point);

		void Update();

		void DebugDraw();
	}
}
