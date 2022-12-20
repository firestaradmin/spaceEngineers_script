using System;
using System.Runtime.InteropServices;
using VRageMath;

namespace VRage.Render11.Scene.Components
{
	[StructLayout(LayoutKind.Explicit, Size = 48)]
	internal struct MyObjectDataVoxelCommon
	{
		[FieldOffset(0)]
		internal float VoxelLodSize;

		[FieldOffset(4)]
		internal Vector3 VoxelOffset;

		[FieldOffset(16)]
		internal Vector3 SpherizeCenter;

		[FieldOffset(28)]
		internal float SpherizeTo;

		[FieldOffset(32)]
		internal float SpherizeStart;

		[FieldOffset(36)]
		internal Vector3 VoxelScale;

		internal static MyObjectDataVoxelCommon Invalid = new MyObjectDataVoxelCommon
		{
			VoxelLodSize = float.NaN,
			VoxelOffset = Vector3.Zero,
			SpherizeCenter = Vector3.Zero,
			SpherizeTo = float.NaN,
			SpherizeStart = float.NaN,
			VoxelScale = Vector3.One
		};

		internal bool IsValid => VoxelLodSize.IsValid();
	}
}
