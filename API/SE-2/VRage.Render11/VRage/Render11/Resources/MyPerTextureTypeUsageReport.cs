namespace VRage.Render11.Resources
{
	internal class MyPerTextureTypeUsageReport
	{
		/// Number of noncompressed loaded textures.
		public int NoncompressedCount;

		/// Number of loaded Compressed textures.
		public int CompressedCount;

		/// Memroy usage of compressed textures.
		public long CompressedMemory;

		/// Memory usage of noncompressed textures.
		public long NoncompressedMemory;
	}
}
