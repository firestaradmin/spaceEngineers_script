using System;

namespace SharpDX.Toolkit.Content
{
	/// <summary>
	/// Exception when an asset was not found from the <see cref="T:SharpDX.Toolkit.Content.ContentManager" />.
	/// </summary>
	public class AssetNotFoundException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.Toolkit.Content.AssetNotFoundException" /> class.
		/// </summary>
		public AssetNotFoundException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.Toolkit.Content.AssetNotFoundException" /> class with the specified message.
		/// </summary>
		/// <param name="message">The exception message.</param>
		public AssetNotFoundException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.Toolkit.Content.AssetNotFoundException" /> class with the specified message and inner exception.
		/// </summary>
		/// <param name="message">The exception message.</param>
		/// <param name="innerException">The inner exception.</param>
		public AssetNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
