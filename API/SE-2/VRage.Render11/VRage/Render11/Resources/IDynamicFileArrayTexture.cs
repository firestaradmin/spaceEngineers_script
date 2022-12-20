namespace VRage.Render11.Resources
{
	internal interface IDynamicFileArrayTexture : IFileArrayTexture, ITexture, ISrvBindable, IResource
	{
		int MinSlices { get; set; }

		int GetOrAddSlice(string filepath);

		void SwapSlice(int sliceNum, string filepath);

		void Release();
	}
}
