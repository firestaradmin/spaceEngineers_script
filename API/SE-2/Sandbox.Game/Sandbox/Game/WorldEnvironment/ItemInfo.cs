using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
	/// Information about a item.
	///
	/// Total Size: 32 bytes;
	public struct ItemInfo
	{
		public bool IsEnabled;

		public Vector3 Position;

		public short DefinitionIndex;

		public short ModelIndex;

		public Quaternion Rotation;

		public override string ToString()
		{
			return $"Model: {ModelIndex}; Def: {DefinitionIndex}";
		}
	}
}
