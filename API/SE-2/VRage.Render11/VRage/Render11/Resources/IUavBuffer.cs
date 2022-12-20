namespace VRage.Render11.Resources
{
	internal interface IUavBuffer : IUavBindable, IResource, IBuffer
	{
		MyUavType UavType { get; }
	}
}
