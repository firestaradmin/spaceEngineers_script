using System;

namespace SharpDX.Toolkit.Content
{
	/// <summary>
	/// A factory to create <see cref="T:SharpDX.Toolkit.Content.IContentReader" /> when a specific type is requested
	/// from the <see cref="T:SharpDX.Toolkit.Content.IContentManager" />.
	/// </summary>
	public interface IContentReaderFactory
	{
		/// <summary>
		/// Returns an instance of a <see cref="T:SharpDX.Toolkit.Content.IContentReader" /> for loading the specified type or null if not handled by this factory.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>An instance of a <see cref="T:SharpDX.Toolkit.Content.IContentReader" /> for loading the specified type or null if not handled..</returns>
		IContentReader TryCreate(Type type);
	}
}
