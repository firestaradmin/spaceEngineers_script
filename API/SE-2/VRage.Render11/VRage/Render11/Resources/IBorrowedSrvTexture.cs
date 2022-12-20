namespace VRage.Render11.Resources
{
	internal interface IBorrowedSrvTexture : ISrvTexture, ISrvBindable, IResource, ITexture
	{
		void AddRef();

		void Release();
	}
}
