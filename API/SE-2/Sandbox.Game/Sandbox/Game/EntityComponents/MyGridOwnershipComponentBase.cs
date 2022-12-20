using Sandbox.Game.Entities.Cube;
using VRage.Game.Components;

namespace Sandbox.Game.EntityComponents
{
	public abstract class MyGridOwnershipComponentBase : MyEntityComponentBase
	{
		public override string ComponentTypeDebugString => "Ownership";

		/// <summary>
		/// Returns the identity id of the block's owner
		/// </summary>
		public abstract long GetBlockOwnerId(MySlimBlock block);
	}
}
