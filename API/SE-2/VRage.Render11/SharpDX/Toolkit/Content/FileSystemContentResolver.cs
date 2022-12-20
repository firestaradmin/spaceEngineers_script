using System.IO;
using SharpDX.IO;

namespace SharpDX.Toolkit.Content
{
	/// <summary>
	/// This <see cref="T:SharpDX.Toolkit.Content.IContentResolver" /> is loading an asset name from a root directory from a physical disk.
	/// </summary>
	public class FileSystemContentResolver : IContentResolver
	{
		/// <summary>
		/// The default extension for asset files which is appended to any asset names that do not specify an extension.
		/// </summary>
		public const string DefaultExtension = ".tkb";

		/// <summary>
		/// Gets the root directory from where assets will be loaded from the disk.
		/// </summary>
		/// <value>The root directory.</value>
		public string RootDirectory { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.Toolkit.Content.FileSystemContentResolver" /> class.
		/// </summary>
		/// <param name="rootDirectory">The root directory.</param>
		public FileSystemContentResolver(string rootDirectory)
		{
			RootDirectory = rootDirectory;
		}

		public bool Exists(string assetName)
		{
			if (!NativeFile.Exists(GetAssetPath(assetName)))
			{
				return NativeFile.Exists(GetAssetPath(assetName, forceAppendExtension: true));
			}
			return true;
		}

		public Stream Resolve(string assetName)
		{
			try
			{
				string assetPath = GetAssetPath(assetName);
				if (!NativeFile.Exists(assetPath))
				{
					assetPath = GetAssetPath(assetName, forceAppendExtension: true);
				}
				return new NativeFileStream(assetPath, NativeFileMode.Open, NativeFileAccess.Read);
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Gets the full asset path based on the root directory and default extension.
		/// </summary>
		/// <param name="assetName">The asset name.</param>
		/// <param name="forceAppendExtension">A value indicating whether to append the default extension even if the supplied name already has one.</param>
		/// <returns>The full asset path.</returns>
		protected string GetAssetPath(string assetName, bool forceAppendExtension = false)
		{
			if (string.IsNullOrEmpty(Path.GetExtension(assetName)) || forceAppendExtension)
			{
				assetName += ".tkb";
			}
			return PathUtility.GetNormalizedPath(Path.Combine(RootDirectory ?? string.Empty, assetName));
		}
	}
}
