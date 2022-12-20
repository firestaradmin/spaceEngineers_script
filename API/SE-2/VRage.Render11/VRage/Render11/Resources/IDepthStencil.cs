namespace VRage.Render11.Resources
{
	internal interface IDepthStencil : IResource
	{
		ISrvBindable SrvDepth { get; }

		ISrvBindable SrvStencil { get; }

		IDsvBindable Dsv { get; }

		IDsvBindable DsvRo { get; }

		IDsvBindable DsvRoStencil { get; }

		IDsvBindable DsvRoDepth { get; }
	}
}
