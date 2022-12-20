using System;

namespace SharpDX.Toolkit.Content
{
	/// <summary>
	/// This attributes is used by data that are providing a <see cref="T:SharpDX.Toolkit.Content.IContentReader" /> for decoding data from a stream.
	/// </summary>
	public class ContentReaderAttribute : Attribute
	{
		private readonly Type contentReaderType;

		/// <summary>
		/// Gets the type of the content reader.
		/// </summary>
		/// <value>The type of the content reader.</value>
		public Type ContentReaderType => contentReaderType;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.Toolkit.Content.ContentReaderAttribute" /> class.
		/// </summary>
		/// <param name="contentReaderType">Type of the content reader.</param>
		public ContentReaderAttribute(Type contentReaderType)
		{
			this.contentReaderType = contentReaderType;
		}
	}
}
