using System;
using System.Runtime.InteropServices;
using VRageMath;

namespace VRage.Render11.Scene.Components
{
	[StructLayout(LayoutKind.Explicit, Size = 32)]
	internal struct MyObjectDataNonVoxel
	{
		[FieldOffset(0)]
		internal float Facing;

		[FieldOffset(4)]
		internal Vector2 WindScaleAndFreq;

		[FieldOffset(12)]
		internal float FoliageMultiplier;

		[FieldOffset(16)]
		internal Vector3 CenterOffset;

		[FieldOffset(28)]
		internal float __padding1;

		internal static MyObjectDataNonVoxel Invalid = new MyObjectDataNonVoxel
		{
			Facing = float.NaN,
			WindScaleAndFreq = Vector2.Zero,
			CenterOffset = Vector3.Zero
		};

		internal bool IsValid => Facing.IsValid();
	}
}
