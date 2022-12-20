using VRageMath;

namespace VRage.Render.Image
{
	public interface IMyImage
	{
		Vector2I Size { get; }

		int Stride { get; }

		int BitsPerPixel { get; }

		object Data { get; }
	}
	public interface IMyImage<TData> : IMyImage where TData : unmanaged
	{
		new TData[] Data { get; }
	}
}
