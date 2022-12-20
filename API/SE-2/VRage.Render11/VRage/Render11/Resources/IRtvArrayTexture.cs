namespace VRage.Render11.Resources
{
	internal interface IRtvArrayTexture : IArrayTexture, ISrvBindable, IResource, IRtvBindable
	{
		IRtvBindable SubresourceRtv(int nSlice, int mipLevel = 0);
	}
}
