namespace VRage.Render11.Resources
{
	internal interface IUavArrayTexture : IArrayTexture, ISrvBindable, IResource
	{
		IRtvBindable SubresourceRtv(int nSlice, int mipLevel = 0);

		IUavBindable SubresourceUav(int nSlice, int mipLevel = 0);
	}
}
