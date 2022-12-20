namespace VRageRender
{
	internal struct MyDrawSubmesh_2
	{
		internal int Count;

		internal int Start;

		internal int BaseVertex;

		internal MyDrawCommandEnum DrawCommand;

		internal MyMaterialProxyId MaterialId;

		internal int[] BonesMapping;

		internal static readonly MyDrawSubmesh_2[] EmptyList = new MyDrawSubmesh_2[0];
	}
}
