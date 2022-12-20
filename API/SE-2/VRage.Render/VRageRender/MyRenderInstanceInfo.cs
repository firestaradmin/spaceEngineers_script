namespace VRageRender
{
	public struct MyRenderInstanceInfo
	{
		public readonly uint InstanceBufferId;

		public readonly int InstanceStart;

		public readonly int InstanceCount;

		public readonly float MaxViewDistance;

		public readonly MyInstanceFlagsEnum Flags;

		public bool CastShadows => (Flags & MyInstanceFlagsEnum.CastShadows) != 0;

		public bool ShowLod1 => (Flags & MyInstanceFlagsEnum.ShowLod1) != 0;

		public bool EnableColorMask => (Flags & MyInstanceFlagsEnum.EnableColorMask) != 0;

		public MyRenderInstanceInfo(uint instanceBufferId, int instanceStart, int instanceCount, float maxViewDistance, MyInstanceFlagsEnum flags)
		{
			Flags = flags;
			InstanceBufferId = instanceBufferId;
			InstanceStart = instanceStart;
			InstanceCount = instanceCount;
			MaxViewDistance = maxViewDistance;
		}

		public MyRenderInstanceInfo(uint instanceBufferId, int instanceStart, int instanceCount, bool castShadows, bool showLod1, float maxViewDistance, bool enableColorMaskHsv)
		{
			Flags = (castShadows ? MyInstanceFlagsEnum.CastShadows : ((MyInstanceFlagsEnum)0)) | (showLod1 ? MyInstanceFlagsEnum.ShowLod1 : ((MyInstanceFlagsEnum)0)) | (enableColorMaskHsv ? MyInstanceFlagsEnum.EnableColorMask : ((MyInstanceFlagsEnum)0));
			InstanceBufferId = instanceBufferId;
			InstanceStart = instanceStart;
			InstanceCount = instanceCount;
			MaxViewDistance = maxViewDistance;
		}
	}
}
