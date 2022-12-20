using System;
using System.Runtime.InteropServices;

namespace Sandbox.Engine.Voxels
{
	internal class MyVoxelMaterialRequestHelper
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct ContouringFlagsProxy : IDisposable
		{
			public void Dispose()
			{
				WantsOcclusion = false;
				IsContouring = false;
			}
		}

		[ThreadStatic]
		public static bool WantsOcclusion;

		[ThreadStatic]
		public static bool IsContouring;

		public static ContouringFlagsProxy StartContouring()
		{
			WantsOcclusion = true;
			IsContouring = true;
			return default(ContouringFlagsProxy);
		}
	}
}
