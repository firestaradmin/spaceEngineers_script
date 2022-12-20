namespace VRage.Voxels
{
	public enum VrVertexClassification : byte
	{
		Invalid = 0,
		Simple = 1,
		InteriorEdge = 2,
		Corner = 3,
		Boundary = 4,
		Complex = 5,
		Remove = 0x80
	}
}
