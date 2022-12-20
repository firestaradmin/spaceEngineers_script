using System;
using System.IO;

namespace SharpDX.Toolkit.Content
{
	/// <summary>
	/// Parameters used by <see cref="M:SharpDX.Toolkit.Content.IContentReader.ReadContent(SharpDX.Toolkit.Content.IContentManager,SharpDX.Toolkit.Content.ContentReaderParameters@)" />
	/// </summary>
	public struct ContentReaderParameters
	{
		/// <summary>
		/// Name of the asset currently loaded when using <see cref="M:SharpDX.Toolkit.Content.IContentManager.Load``1(System.String,System.Object)" />.
		/// </summary>
		public string AssetName;

		/// <summary>
		/// Type of the asset currently loaded when using <see cref="M:SharpDX.Toolkit.Content.IContentManager.Load``1(System.String,System.Object)" />.
		/// </summary>
		public Type AssetType;

		/// <summary>
		/// Stream of the asset to load.
		/// </summary>
		public Stream Stream;

		/// <summary>
		/// This parameter is an out parameter for <see cref="M:SharpDX.Toolkit.Content.IContentReader.ReadContent(SharpDX.Toolkit.Content.IContentManager,SharpDX.Toolkit.Content.ContentReaderParameters@)" />. Set to true to let the ContentManager close the stream once the reader is done.
		/// </summary>
		public bool KeepStreamOpen;

		/// <summary>
		/// Custom options provided when using <see cref="M:SharpDX.Toolkit.Content.IContentManager.Load``1(System.String,System.Object)" />.
		/// </summary>
		public object Options;
	}
}
