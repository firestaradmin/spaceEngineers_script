using VRageMath;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes bounty contract that player can take in contract block (mods interface)
	/// Player should protect grid
	/// </summary>
	public interface IMyContractEscort : IMyContract
	{
		Vector3D Start { get; }

		Vector3D End { get; }

		long OwnerIdentityId { get; }
	}
}
