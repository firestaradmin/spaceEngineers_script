using SharpDX.Direct3D11;

namespace VRage.Render11.Resources
{
	internal interface IBuffer : IResource
	{
		/// <summary>
		/// It's the same as <see cref="P:VRage.Render11.Resources.IBuffer.Description" />.SizeInBytes.
		/// </summary>
		int ByteSize { get; }

		int ElementCount { get; }

		BufferDescription Description { get; }

		Buffer Buffer { get; }

		bool IsGlobal { get; }

		void DisposeInternal();
	}
}
