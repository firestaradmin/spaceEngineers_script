using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
	public interface IMyTextPanelProvider
	{
		int PanelTexturesByteCount { get; }

		Vector3D WorldPosition { get; }

		int RangeIndex { get; set; }
	}
}
