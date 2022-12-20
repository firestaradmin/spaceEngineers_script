using VRage.Utils;
using VRageMath;

namespace VRage.Game.Graphics
{
	public interface IMyTrackTrails
	{
		MyTrailProperties LastTrail { get; set; }

		void AddTrails(MyTrailProperties trailProperties);

		void AddTrails(Vector3D position, Vector3D normal, Vector3D forwardDirection, long entityId, MyStringHash physicalMaterial, MyStringHash voxelMaterial);
	}
}
