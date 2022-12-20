using System;
using System.Collections.Generic;
using System.IO;
using SharpDX.Toolkit.Collections;

namespace SharpDX.Toolkit.Content
{
	/// <summary>
	/// The content manager implementation is responsible to load and store content data (texture, songs, effects...etc.) using 
	/// several <see cref="T:SharpDX.Toolkit.Content.IContentResolver" /> to resolve a stream from an asset name and several registered <see cref="T:SharpDX.Toolkit.Content.IContentReader" />
	/// to convert data from stream.
	/// </summary>
	public class ContentManager : Component, IContentManager
	{
		/// <summary>
		/// Use this key to store loaded assets.
		/// </summary>
		protected struct AssetKey : IEquatable<AssetKey>
		{
			public readonly Type AssetType;

			public readonly string AssetName;

			public AssetKey(Type assetType, string assetName)
			{
				AssetType = assetType;
				AssetName = assetName;
			}

			public bool Equals(AssetKey other)
			{
				if (AssetType == other.AssetType)
				{
					return string.Equals(AssetName, other.AssetName, StringComparison.OrdinalIgnoreCase);
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				if (obj is AssetKey)
				{
					return Equals((AssetKey)obj);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (AssetType.GetHashCode() * 397) ^ AssetName.GetHashCode();
			}

			public static bool operator ==(AssetKey left, AssetKey right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(AssetKey left, AssetKey right)
			{
				return !left.Equals(right);
			}
		}

		private readonly Dictionary<AssetKey, object> assetLockers;

		private readonly List<IContentResolver> registeredContentResolvers;

		private readonly Dictionary<Type, IContentReader> registeredContentReaders;

		private readonly List<IContentReaderFactory> registeredContentReaderFactories;

		protected readonly Dictionary<AssetKey, object> loadedAssets;

		private string rootDirectory;

		/// <summary>
		/// Add or remove registered <see cref="T:SharpDX.Toolkit.Content.IContentResolver" /> to this instance.
		/// </summary>
		public ObservableCollection<IContentResolver> Resolvers { get; private set; }

		/// <summary>
		/// Add or remove registered <see cref="T:SharpDX.Toolkit.Content.IContentReader" /> to this instance.
		/// </summary>
		public ObservableDictionary<Type, IContentReader> Readers { get; private set; }

		/// <summary>
		/// Add or remove a registered <see cref="T:SharpDX.Toolkit.Content.IContentReaderFactory" /> to this instance.
		/// </summary>
		/// <value>The reader factories.</value>
		public ObservableCollection<IContentReaderFactory> ReaderFactories { get; private set; }

		/// <summary>
		/// Gets the service provider associated with the ContentManager.
		/// </summary>
		/// <value>The service provider.</value>
		public IServiceProvider ServiceProvider { get; protected set; }

		/// <summary>
		/// Gets or sets the root directory.
		/// </summary>
		public string RootDirectory
		{
			get
			{
				return rootDirectory;
			}
			set
			{
				if (loadedAssets.Count > 0)
				{
					throw new InvalidOperationException("RootDirectory cannot be changed when a ContentManager has already assets loaded");
				}
				rootDirectory = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of ContentManager. Reference page contains code sample.
		/// </summary>
		/// <param name="serviceProvider">The service provider that the ContentManager should use to locate services.</param>
		public ContentManager(IServiceProvider serviceProvider)
			: base("ContentManager")
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}
			ServiceProvider = serviceProvider;
			Resolvers = new ObservableCollection<IContentResolver>();
			Resolvers.ItemAdded += ContentResolvers_ItemAdded;
			Resolvers.ItemRemoved += ContentResolvers_ItemRemoved;
			registeredContentResolvers = new List<IContentResolver>();
			Readers = new ObservableDictionary<Type, IContentReader>();
			Readers.ItemAdded += ContentReaders_ItemAdded;
			Readers.ItemRemoved += ContentReaders_ItemRemoved;
			registeredContentReaders = new Dictionary<Type, IContentReader>();
			ReaderFactories = new ObservableCollection<IContentReaderFactory>();
			ReaderFactories.ItemAdded += ReaderFactories_ItemAdded;
			ReaderFactories.ItemRemoved += ReaderFactories_ItemRemoved;
			registeredContentReaderFactories = new List<IContentReaderFactory>();
			loadedAssets = new Dictionary<AssetKey, object>();
			assetLockers = new Dictionary<AssetKey, object>();
		}

		/// <summary>
		/// Checks if the specified assets exists.
		/// </summary>
		/// <param name="assetName">The asset name with extension.</param>
		/// <returns><c>true</c> if the specified assets exists, <c>false</c> otherwise</returns>
		public virtual bool Exists(string assetName)
		{
			if (assetName == null)
			{
				throw new ArgumentNullException("assetName");
			}
			List<IContentResolver> list;
			lock (registeredContentResolvers)
			{
				list = new List<IContentResolver>(registeredContentResolvers);
			}
			if (list.Count == 0)
			{
				throw new InvalidOperationException("No resolver registered to this content manager");
			}
			string assetName2 = Path.Combine(rootDirectory ?? string.Empty, assetName);
			foreach (IContentResolver item in list)
			{
				if (item.Exists(assetName2))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Loads an asset that has been processed by the Content Pipeline.  Reference page contains code sample.
		/// </summary>
		/// <typeparam name="T">Type of the asset</typeparam>
		/// <param name="assetName">The asset name </param>
		/// <param name="options">The options to pass to the content reader (null by default).</param>
		/// <returns>``0.</returns>
		/// <exception cref="T:SharpDX.Toolkit.Content.AssetNotFoundException">If the asset was not found from all <see cref="T:SharpDX.Toolkit.Content.IContentResolver" />.</exception>
		/// <exception cref="T:System.NotSupportedException">If no content reader was suitable to decode the asset.</exception>
		public virtual T Load<T>(string assetName, object options = null)
		{
			return (T)Load(typeof(T), assetName, options);
		}

<<<<<<< HEAD
		/// <summary>
		/// Loads an asset that has been processed by the Content Pipeline.  Reference page contains code sample.
		/// </summary>
		/// <param name="assetType">Asset Type</param>
		/// <param name="assetName">The asset name </param>
		/// <param name="options">The options to pass to the content reader (null by default).</param>
		/// <returns>Asset</returns>
		/// <exception cref="T:SharpDX.Toolkit.Content.AssetNotFoundException">If the asset was not found from all <see cref="T:SharpDX.Toolkit.Content.IContentResolver" />.</exception>
		/// <exception cref="T:System.NotSupportedException">If no content reader was suitable to decode the asset.</exception>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public virtual object Load(Type assetType, string assetName, object options = null)
		{
			if (assetName == null)
			{
				throw new ArgumentNullException("assetName");
			}
			if (assetType == null)
			{
				throw new ArgumentNullException("assetType");
			}
			object value = null;
			AssetKey assetKey = new AssetKey(assetType, assetName);
			lock (GetAssetLocker(assetKey, create: true))
			{
				lock (loadedAssets)
				{
					if (loadedAssets.TryGetValue(assetKey, out value))
					{
						return value;
					}
				}
				string assetName2 = Path.Combine(rootDirectory ?? string.Empty, assetName);
				Stream stream = FindStream(assetName2);
				value = LoadAssetWithDynamicContentReader(assetType, assetName, stream, options);
				lock (loadedAssets)
				{
					loadedAssets.Add(assetKey, value);
					return value;
				}
			}
		}

		/// <summary>
		/// Unloads all data that was loaded by this ContentManager. All data will be disposed.
		/// </summary>
		/// <remarks>
		/// Unlike <see cref="M:SharpDX.Toolkit.Content.ContentManager.Load``1(System.String,System.Object)" /> method, this method is not thread safe and must be called by a single caller at a single time.
		/// </remarks>
		public virtual void Unload()
		{
			lock (assetLockers)
			{
				lock (loadedAssets)
				{
					foreach (object value in loadedAssets.Values)
					{
						(value as IDisposable)?.Dispose();
					}
					assetLockers.Clear();
					loadedAssets.Clear();
				}
			}
		}

		/// <summary>
		/// Unloads and disposes an asset.
		/// </summary>
		/// <param name="assetName">The asset name</param>
		/// <returns><c>true</c> if the asset exists and was unloaded, <c>false</c> otherwise.</returns>
		public virtual bool Unload<T>(string assetName)
		{
			return Unload(typeof(T), assetName);
		}

		/// <summary>
		/// Unloads and disposes an asset.
		/// </summary>
		/// <param name="assetType">The asset type</param>
		/// <param name="assetName">The asset name</param>
		/// <returns><c>true</c> if the asset exists and was unloaded, <c>false</c> otherwise.</returns>
		public virtual bool Unload(Type assetType, string assetName)
		{
			if (assetType == null)
			{
				throw new ArgumentNullException("assetType");
			}
			if (assetName == null)
			{
				throw new ArgumentNullException("assetName");
			}
			AssetKey assetKey = new AssetKey(assetType, assetName);
			object assetLocker = GetAssetLocker(assetKey, create: false);
			if (assetLocker == null)
			{
				return false;
			}
			object value;
			lock (assetLocker)
			{
				lock (loadedAssets)
				{
					if (!loadedAssets.TryGetValue(assetKey, out value))
					{
						return false;
					}
					loadedAssets.Remove(assetKey);
				}
				lock (assetLockers)
				{
					assetLockers.Remove(assetKey);
				}
			}
			(value as IDisposable)?.Dispose();
			return true;
		}

		protected object GetAssetLocker(AssetKey assetKey, bool create)
		{
			lock (assetLockers)
			{
				if (!assetLockers.TryGetValue(assetKey, out var value) && create)
				{
					value = new object();
					assetLockers.Add(assetKey, value);
					return value;
				}
				return value;
			}
		}

		private Stream FindStream(string assetName)
		{
			Stream stream = null;
			List<IContentResolver> list;
			lock (registeredContentResolvers)
			{
				list = new List<IContentResolver>(registeredContentResolvers);
			}
			if (list.Count == 0)
			{
				throw new InvalidOperationException("No resolver registered to this content manager");
			}
			Exception innerException = null;
			foreach (IContentResolver item in list)
			{
				try
				{
					if (item.Exists(assetName))
					{
						stream = item.Resolve(assetName);
						if (stream != null)
						{
							goto IL_008b;
						}
					}
				}
				catch (Exception ex)
				{
					innerException = ex;
				}
			}
			goto IL_008b;
			IL_008b:
			if (stream == null)
			{
				throw new AssetNotFoundException(assetName, innerException);
			}
			return stream;
		}

		private object LoadAssetWithDynamicContentReader(Type assetType, string assetName, Stream stream, object options)
		{
			ContentReaderParameters contentReaderParameters = default(ContentReaderParameters);
			contentReaderParameters.AssetName = assetName;
			contentReaderParameters.AssetType = assetType;
			contentReaderParameters.Stream = stream;
			contentReaderParameters.Options = options;
			ContentReaderParameters parameters = contentReaderParameters;
			try
			{
				IContentReader value;
				lock (registeredContentReaders)
				{
					if (!registeredContentReaders.TryGetValue(assetType, out value))
					{
						lock (registeredContentReaderFactories)
						{
							foreach (IContentReaderFactory registeredContentReaderFactory in registeredContentReaderFactories)
							{
								value = registeredContentReaderFactory.TryCreate(assetType);
								if (value != null)
								{
									registeredContentReaders.Add(assetType, value);
									break;
								}
							}
						}
						if (value == null)
						{
							ContentReaderAttribute customAttribute = Utilities.GetCustomAttribute<ContentReaderAttribute>(assetType, inherited: true);
							if (customAttribute != null)
							{
								value = Activator.CreateInstance(customAttribute.ContentReaderType) as IContentReader;
								if (value != null)
								{
									Readers.Add(assetType, value);
								}
							}
						}
					}
				}
				if (value == null)
				{
					throw new NotSupportedException($"Type [{assetType.FullName}] doesn't provide a ContentReaderAttribute, and there is no registered content reader/factory for it.");
				}
				object obj = value.ReadContent(this, ref parameters);
				if (obj == null)
				{
					throw new NotSupportedException($"Registered ContentReader of type [{value.GetType()}] fails to load content of type [{assetType.FullName}] from file [{assetName}].");
				}
				return obj;
			}
			finally
			{
				if (!parameters.KeepStreamOpen)
				{
					stream.Dispose();
				}
			}
		}

		protected override void Dispose(bool disposeManagedResources)
		{
			if (disposeManagedResources)
			{
				Unload();
			}
			base.Dispose(disposeManagedResources);
		}

		private void ContentResolvers_ItemAdded(object sender, ObservableCollectionEventArgs<IContentResolver> e)
		{
			if (e.Item == null)
			{
				throw new ArgumentNullException("Cannot add a null IContentResolver", "value");
			}
			lock (registeredContentResolvers)
			{
				registeredContentResolvers.Add(e.Item);
			}
		}

		private void ContentResolvers_ItemRemoved(object sender, ObservableCollectionEventArgs<IContentResolver> e)
		{
			lock (registeredContentResolvers)
			{
				registeredContentResolvers.Remove(e.Item);
			}
		}

		private void ContentReaders_ItemAdded(object sender, ObservableDictionaryEventArgs<Type, IContentReader> e)
		{
			if (e.Key == null || e.Value == null)
			{
				throw new ArgumentNullException("Cannot add a null Type/IContentReader", "value");
			}
			lock (registeredContentReaders)
			{
				registeredContentReaders.Add(e.Key, e.Value);
			}
		}

		private void ContentReaders_ItemRemoved(object sender, ObservableDictionaryEventArgs<Type, IContentReader> e)
		{
			lock (registeredContentReaders)
			{
				registeredContentReaders.Remove(e.Key);
			}
		}

		private void ReaderFactories_ItemAdded(object sender, ObservableCollectionEventArgs<IContentReaderFactory> e)
		{
			if (e.Item == null)
			{
				throw new ArgumentNullException("Cannot add a null IContentReader", "value");
			}
			lock (registeredContentReaderFactories)
			{
				registeredContentReaderFactories.Add(e.Item);
			}
		}

		private void ReaderFactories_ItemRemoved(object sender, ObservableCollectionEventArgs<IContentReaderFactory> e)
		{
			lock (registeredContentReaderFactories)
			{
				registeredContentReaderFactories.Remove(e.Item);
			}
		}
	}
}
