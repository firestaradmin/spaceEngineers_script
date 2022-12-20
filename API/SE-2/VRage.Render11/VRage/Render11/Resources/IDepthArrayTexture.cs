namespace VRage.Render11.Resources
{
	internal interface IDepthArrayTexture : IArrayTexture, ISrvBindable, IResource
	{
		IDsvBindable SubresourceDsv(int nSlice, int mipLevel = 0);

		IDsvBindable SubresourceDsvRo(int nSlice, int mipLevel = 0);
	}
}
