using VRage.Voxels.Sewing;

namespace VRage.Voxels.Mesh
{
	public static class MyStitchOperations
	{
		public static VrSewOperation GetInstruction(bool x = false, bool y = false, bool z = false, bool xy = false, bool xz = false, bool yz = false, bool xyz = false)
		{
			return (VrSewOperation)(0u | (x ? 2u : 0u) | (y ? 4u : 0u) | (z ? 6u : 0u) | (xy ? 8u : 0u) | (xz ? 10u : 0u) | (yz ? 12u : 0u) | (xyz ? 14u : 0u));
		}

		public static bool Contains(this VrSewOperation self, VrSewOperation flags)
		{
			return (self & flags) == flags;
		}

		public static VrSewOperation Without(this VrSewOperation self, VrSewOperation flags)
		{
			return self & (VrSewOperation)(~(uint)flags);
		}

		public static VrSewOperation With(this VrSewOperation self, VrSewOperation flags)
		{
			return self | flags;
		}
	}
}
