using System;
using SharpDX.DXGI;

namespace SharpDX.Toolkit.Graphics
{
	/// <summary>
	/// A description for <see cref="T:SharpDX.Toolkit.Graphics.Image" />.
	/// </summary>
	public struct ImageDescription : IEquatable<ImageDescription>
	{
		/// <summary>
		/// The dimension of a texture.
		/// </summary>
		public TextureDimension Dimension;

		/// <summary>	
		/// <dd> <p>Texture width (in texels). The  range is from 1 to <see cref="F:SharpDX.Direct3D11.Resource.MaximumTexture1DSize" /> (16384). However, the range is actually constrained by the feature level at which you create the rendering device. For more information about restrictions, see Remarks.</p> </dd>	
		/// </summary>	
		/// <remarks>
<<<<<<< HEAD
		/// This field is valid for all textures.
=======
		/// This field is valid for all textures: <see cref="!:Texture1D" />, <see cref="!:Texture2D" />, <see cref="!:Texture3D" /> and <see cref="!:TextureCube" />.
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// </remarks>
		/// <msdn-id>ff476252</msdn-id>	
		/// <unmanaged>unsigned int Width</unmanaged>	
		/// <unmanaged-short>unsigned int Width</unmanaged-short>	
		public int Width;

		/// <summary>	
		/// <dd> <p>Texture height (in texels). The  range is from 1 to <see cref="F:SharpDX.Direct3D11.Resource.MaximumTexture3DSize" /> (2048). However, the range is actually constrained by the feature level at which you create the rendering device. For more information about restrictions, see Remarks.</p> </dd>	
		/// </summary>	
		/// <remarks>
<<<<<<< HEAD
		/// This field is only valid for Texture2D, Texture3D and TextureCube/&gt;.
=======
		/// This field is only valid for <see cref="!:Texture2D" />, <see cref="!:Texture3D" /> and <see cref="!:TextureCube" />.
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// </remarks>
		/// <msdn-id>ff476254</msdn-id>	
		/// <unmanaged>unsigned int Height</unmanaged>	
		/// <unmanaged-short>unsigned int Height</unmanaged-short>	
		public int Height;

		/// <summary>	
		/// <dd> <p>Texture depth (in texels). The  range is from 1 to <see cref="F:SharpDX.Direct3D11.Resource.MaximumTexture3DSize" /> (2048). However, the range is actually constrained by the feature level at which you create the rendering device. For more information about restrictions, see Remarks.</p> </dd>	
		/// </summary>	
		/// <remarks>
<<<<<<< HEAD
		/// This field is only valid for Texture3D/&gt;.
=======
		/// This field is only valid for <see cref="!:Texture3D" />.
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// </remarks>
		/// <msdn-id>ff476254</msdn-id>	
		/// <unmanaged>unsigned int Depth</unmanaged>	
		/// <unmanaged-short>unsigned int Depth</unmanaged-short>	
		public int Depth;

		/// <summary>	
		/// <dd> <p>Number of textures in the array. The  range is from 1 to <see cref="F:SharpDX.Direct3D11.Resource.MaximumTexture1DArraySize" /> (2048). However, the range is actually constrained by the feature level at which you create the rendering device. For more information about restrictions, see Remarks.</p> </dd>	
		/// </summary>	
		/// <remarks>
<<<<<<< HEAD
		/// This field is only valid for Texture1D, Texture2D and TextureCube/&gt;
		/// </remarks>
		/// <remarks>
		/// This field is only valid for textures: Texture1D, Texture2D and TextureCube/&gt;.
=======
		/// This field is only valid for <see cref="!:Texture1D" />, <see cref="!:Texture2D" /> and <see cref="!:TextureCube" />
		/// </remarks>
		/// <remarks>
		/// This field is only valid for textures: <see cref="!:Texture1D" />, <see cref="!:Texture2D" /> and <see cref="!:TextureCube" />.
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// </remarks>
		/// <msdn-id>ff476252</msdn-id>	
		/// <unmanaged>unsigned int ArraySize</unmanaged>	
		/// <unmanaged-short>unsigned int ArraySize</unmanaged-short>	
		public int ArraySize;

		/// <summary>	
		/// <dd> <p>The maximum number of mipmap levels in the texture. See the remarks in <strong><see cref="T:SharpDX.Direct3D11.ShaderResourceViewDescription.Texture1DResource" /></strong>. Use 1 for a multisampled texture; or 0 to generate a full set of subtextures.</p> </dd>	
		/// </summary>	
		/// <msdn-id>ff476252</msdn-id>	
		/// <unmanaged>unsigned int MipLevels</unmanaged>	
		/// <unmanaged-short>unsigned int MipLevels</unmanaged-short>	
		public int MipLevels;

		/// <summary>	
		/// <dd> <p>Texture format (see <strong><see cref="T:SharpDX.DXGI.Format" /></strong>).</p> </dd>	
		/// </summary>	
		/// <msdn-id>ff476252</msdn-id>	
		/// <unmanaged>DXGI_FORMAT Format</unmanaged>	
		/// <unmanaged-short>DXGI_FORMAT Format</unmanaged-short>	
		public Format Format;

		public bool Equals(ImageDescription other)
		{
			if (Dimension.Equals(other.Dimension) && Width == other.Width && Height == other.Height && Depth == other.Depth && ArraySize == other.ArraySize && MipLevels == other.MipLevels)
			{
				return Format.Equals(other.Format);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is ImageDescription)
			{
				return Equals((ImageDescription)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((((((((((Dimension.GetHashCode() * 397) ^ Width) * 397) ^ Height) * 397) ^ Depth) * 397) ^ ArraySize) * 397) ^ MipLevels) * 397) ^ Format.GetHashCode();
		}

		public static bool operator ==(ImageDescription left, ImageDescription right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ImageDescription left, ImageDescription right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"Dimension: {Dimension}, Width: {Width}, Height: {Height}, Depth: {Depth}, Format: {Format}, ArraySize: {ArraySize}, MipLevels: {MipLevels}";
		}
	}
}
