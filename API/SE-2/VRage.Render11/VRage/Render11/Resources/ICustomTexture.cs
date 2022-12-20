namespace VRage.Render11.Resources
{
	internal interface ICustomTexture : IUavBindable, IResource
	{
		IRtvTexture SRgb { get; }

		IRtvTexture Linear { get; }
	}
}
