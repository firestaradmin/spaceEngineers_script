using System;

namespace SharpDX.Toolkit.Content
{
	/// <summary>
	/// The content manager interface provides a service to load and store content data (texture, songs, effects...etc.).
	/// </summary>
	public interface IContentManager
	{
		/// <summary>
		/// Gets the service provider associated with the ContentManager.
		/// </summary>
		/// <value>The service provider.</value>
		/// <remarks>
<<<<<<< HEAD
		/// The service provider can be used by some <see cref="T:SharpDX.Toolkit.Content.IContentReader" /> when for example a GraphicsDevice needs to be 
=======
		/// The service provider can be used by some <see cref="T:SharpDX.Toolkit.Content.IContentReader" /> when for example a <see cref="!:SharpDX.Toolkit.Graphics.GraphicsDevice" /> needs to be 
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// used to instantiate a content.
		/// </remarks>
		IServiceProvider ServiceProvider { get; }

		/// <summary>
		/// Checks if the specified assets exists.
		/// </summary>
		/// <param name="assetName">The asset name with extension.</param>
		/// <returns><c>true</c> if the specified assets exists, <c>false</c> otherwise</returns>
		bool Exists(string assetName);

		/// <summary>
		/// Loads an asset that has been processed by the Content Pipeline.  Reference page contains code sample.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="assetName">Full asset name (with its extension)</param>
		/// <param name="options">The options to pass to the content reader (null by default).</param>
		/// <returns>``0.</returns>
		/// <exception cref="T:SharpDX.Toolkit.Content.AssetNotFoundException">If the asset was not found from all <see cref="T:SharpDX.Toolkit.Content.IContentResolver" />.</exception>
		/// <exception cref="T:System.NotSupportedException">If no content reader was suitable to decode the asset.</exception>
		T Load<T>(string assetName, object options = null);

<<<<<<< HEAD
		/// <summary>
		/// Loads an asset that has been processed by the Content Pipeline.  Reference page contains code sample.
		/// </summary>
		/// <param name="assetType">Asset Type</param>
		/// <param name="assetName">Full asset name (with its extension)</param>
		/// <param name="options">The options to pass to the content reader (null by default).</param>
		/// <returns>Asset</returns>
		/// <exception cref="T:SharpDX.Toolkit.Content.AssetNotFoundException">If the asset was not found from all <see cref="T:SharpDX.Toolkit.Content.IContentResolver" />.</exception>
		/// <exception cref="T:System.NotSupportedException">If no content reader was suitable to decode the asset.</exception>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		object Load(Type assetType, string assetName, object options = null);

		/// <summary>
		/// Unloads all data that was loaded by this ContentManager. All data will be disposed.
		/// </summary>
		/// <remarks>
		/// Unlike <see cref="M:SharpDX.Toolkit.Content.ContentManager.Load``1(System.String,System.Object)" /> method, this method is not thread safe and must be called by a single caller at a single time.
		/// </remarks>
		void Unload();

		/// <summary>
		/// Unloads and disposes an asset.
		/// </summary>
		/// <param name="assetName">The asset name</param>
		/// <returns><c>true</c> if the asset exists and was unloaded, <c>false</c> otherwise.</returns>
		bool Unload<T>(string assetName);

		/// <summary>
		/// Unloads and disposes an asset.
		/// </summary>
		/// <param name="assetType">The asset type</param>
		/// <param name="assetName">The asset name</param>
		/// <returns><c>true</c> if the asset exists and was unloaded, <c>false</c> otherwise.</returns>
		bool Unload(Type assetType, string assetName);
	}
}
