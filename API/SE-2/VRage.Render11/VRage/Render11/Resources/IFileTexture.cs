namespace VRage.Render11.Resources
{
	internal interface IFileTexture : IAsyncTexture, ITexture, ISrvBindable, IResource
	{
		string Path { get; }

		bool LoadFailed { get; }
	}
}
