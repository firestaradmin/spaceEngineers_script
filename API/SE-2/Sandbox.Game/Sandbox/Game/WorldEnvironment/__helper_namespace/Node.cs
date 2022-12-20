namespace Sandbox.Game.WorldEnvironment.__helper_namespace
{
	/// <summary>
	/// We want the node type outside the My2DClipmap class because that class is generic.
	/// Still we don't want it to be visible elsewhere.
	/// </summary>
	internal struct Node
	{
		public unsafe fixed int Children[4];

		public int Lod;
	}
}
