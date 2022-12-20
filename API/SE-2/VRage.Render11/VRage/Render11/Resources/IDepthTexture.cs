namespace VRage.Render11.Resources
{
	internal interface IDepthTexture : ISrvTexture, ISrvBindable, IResource, ITexture, IDsvBindable
	{
		IDsvBindable DsvRo { get; }
	}
}
