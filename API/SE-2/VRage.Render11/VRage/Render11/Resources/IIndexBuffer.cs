namespace VRage.Render11.Resources
{
	internal interface IIndexBuffer : IBuffer, IResource
	{
		MyIndexBufferFormat Format { get; }
	}
}
