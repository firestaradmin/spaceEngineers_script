using System.Collections.Generic;

namespace VRage.Render11.Resources
{
	internal struct MyFileTextureUsageReport
	{
		public int TexturesTotal;

		public int TexturesLoaded;

		public long TotalTextureMemory;

		public int TexturesTotalPeak;

		public int TexturesLoadedPeak;

		public long TotalTextureMemoryPeak;

		/// Loaded textures by type.
		public Dictionary<MyFileTextureEnum, MyPerTextureTypeUsageReport> TexturesLoadedByTypeData;
	}
}
