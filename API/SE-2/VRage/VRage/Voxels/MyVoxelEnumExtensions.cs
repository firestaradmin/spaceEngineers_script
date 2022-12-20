namespace VRage.Voxels
{
	public static class MyVoxelEnumExtensions
	{
		public static bool Requests(this MyStorageDataTypeFlags self, MyStorageDataTypeEnum value)
		{
			return ((uint)self & (uint)(1 << (int)value)) != 0;
		}

		public static MyStorageDataTypeFlags Without(this MyStorageDataTypeFlags self, MyStorageDataTypeEnum value)
		{
			return self & (MyStorageDataTypeFlags)(~(byte)(1 << (int)value)) & MyStorageDataTypeFlags.ContentAndMaterial;
		}

		public static MyStorageDataTypeFlags ToFlags(this MyStorageDataTypeEnum self)
		{
			return (MyStorageDataTypeFlags)(1 << (int)self);
		}
	}
}
