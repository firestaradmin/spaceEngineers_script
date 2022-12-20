using System.Runtime.InteropServices;

namespace VRage.Voxels
{
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct VrVoxelTriangle
	{
		public ushort V0;

		public ushort V1;

		public ushort V2;

		public override string ToString()
		{
			return $"{{{V0} {V1} {V2}}}";
		}
	}
}
