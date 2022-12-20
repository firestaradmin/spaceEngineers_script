namespace VRage.Render11.Resources
{
	internal interface IAsyncTexture : ITexture, ISrvBindable, IResource
	{
		bool IsLoaded { get; }
	}
}
