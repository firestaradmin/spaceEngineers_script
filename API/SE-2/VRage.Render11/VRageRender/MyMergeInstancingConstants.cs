namespace VRageRender
{
	internal struct MyMergeInstancingConstants
	{
		public static readonly MyMergeInstancingConstants Default;

		public int InstanceIndex;

		public int StartIndex;

		static MyMergeInstancingConstants()
		{
			Default = new MyMergeInstancingConstants
			{
				InstanceIndex = -1
			};
		}
	}
}
