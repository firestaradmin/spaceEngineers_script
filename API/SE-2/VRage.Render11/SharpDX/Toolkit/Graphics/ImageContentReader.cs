using SharpDX.Toolkit.Content;

namespace SharpDX.Toolkit.Graphics
{
	/// <summary>
	/// Content reader for an image.
	/// </summary>
	internal class ImageContentReader : IContentReader
	{
		public object ReadContent(IContentManager readerManager, ref ContentReaderParameters parameters)
		{
			parameters.KeepStreamOpen = false;
			Image image = Image.Load(parameters.Stream, parameters.AssetName);
			if (image != null)
			{
				image.Name = parameters.AssetName;
			}
			return image;
		}
	}
}
