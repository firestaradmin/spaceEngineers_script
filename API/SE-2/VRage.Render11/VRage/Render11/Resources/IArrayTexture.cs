namespace VRage.Render11.Resources
{
	internal interface IArrayTexture : ISrvBindable, IResource
	{
		int NumSlices { get; }

		int MipLevels { get; }

		ISrvBindable SubresourceSrv(int nSlice, int mipLevel = 0);
	}
}
