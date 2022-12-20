using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Network;

namespace VRageRender
{
	[ProtoContract]
	public struct MyDecalBlacklistDesc
	{
		protected class VRageRender_MyDecalBlacklistDesc_003C_003EVoxelMaterials_003C_003EAccessor : IMemberAccessor<MyDecalBlacklistDesc, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDecalBlacklistDesc owner, in string[] value)
			{
				owner.VoxelMaterials = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDecalBlacklistDesc owner, out string[] value)
			{
				value = owner.VoxelMaterials;
			}
		}

		[ProtoMember(1)]
		public string[] VoxelMaterials;
	}
}
