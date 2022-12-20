using System;
using System.IO;
using SharpDX.DXGI;
using SharpDX.IO;

namespace SharpDX.Toolkit.Graphics
{
	/// <summary>
	/// An unmanaged buffer of pixels.
	/// </summary>
	public sealed class PixelBuffer
	{
		private int width;

		private int height;

		private Format format;

		private int rowStride;

		private int bufferStride;

		private readonly IntPtr dataPointer;

		private int pixelSize;

		/// <summary>
		/// True when RowStride == sizeof(pixelformat) * width
		/// </summary>
		private bool isStrictRowStride;

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>The width.</value>
		public int Width => width;

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		public int Height => height;

		/// <summary>
		/// Gets the format (this value can be changed)
		/// </summary>
		/// <value>The format.</value>
		public Format Format
		{
			get
			{
				return format;
			}
			set
			{
				if (PixelSize != value.SizeOfInBytes())
				{
					throw new ArgumentException($"Format [{value}] doesn't have same pixel size in bytes than current format [{format}]");
				}
				format = value;
			}
		}

		/// <summary>
		/// Gets the pixel size in bytes.
		/// </summary>
		/// <value>The pixel size in bytes.</value>
		public int PixelSize => pixelSize;

		/// <summary>
		/// Gets the row stride in number of bytes.
		/// </summary>
		/// <value>The row stride in number of bytes.</value>
		public int RowStride => rowStride;

		/// <summary>
		/// Gets the total size in bytes of this pixel buffer.
		/// </summary>
		/// <value>The size in bytes of the pixel buffer.</value>
		public int BufferStride => bufferStride;

		/// <summary>
		/// Gets the pointer to the pixel buffer.
		/// </summary>
		/// <value>The pointer to the pixel buffer.</value>
		public IntPtr DataPointer => dataPointer;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.Toolkit.Graphics.PixelBuffer" /> struct.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="format">The format.</param>
		/// <param name="rowStride">The row pitch.</param>
		/// <param name="bufferStride">The slice pitch.</param>
		/// <param name="dataPointer">The pixels.</param>
		public PixelBuffer(int width, int height, Format format, int rowStride, int bufferStride, IntPtr dataPointer)
		{
			if (dataPointer == IntPtr.Zero)
			{
				throw new ArgumentException("Pointer cannot be equal to IntPtr.Zero", "dataPointer");
			}
			this.width = width;
			this.height = height;
			this.format = format;
			this.rowStride = rowStride;
			this.bufferStride = bufferStride;
			this.dataPointer = dataPointer;
			pixelSize = this.format.SizeOfInBytes();
			isStrictRowStride = pixelSize * width == rowStride;
		}

		/// <summary>
		/// Copies this pixel buffer to a destination pixel buffer.
		/// </summary>
		/// <param name="pixelBuffer">The destination pixel buffer.</param>
		/// <remarks>
		/// The destination pixel buffer must have exactly the same dimensions (width, height) and format than this instance.
		/// Destination buffer can have different row stride.
		/// </remarks>
		public unsafe void CopyTo(PixelBuffer pixelBuffer)
		{
			if (Width != pixelBuffer.Width || Height != pixelBuffer.Height || PixelSize != pixelBuffer.Format.SizeOfInBytes())
			{
				throw new ArgumentException("Invalid destination pixelBufferArray. Mush have same Width, Height and Format", "pixelBuffer");
			}
			if (BufferStride == pixelBuffer.BufferStride)
			{
				Utilities.CopyMemory(pixelBuffer.DataPointer, DataPointer, BufferStride);
				return;
			}
			byte* ptr = (byte*)(void*)DataPointer;
			byte* ptr2 = (byte*)(void*)pixelBuffer.DataPointer;
			int sizeInBytesToCopy = Math.Min(RowStride, pixelBuffer.RowStride);
			for (int i = 0; i < Height; i++)
			{
				Utilities.CopyMemory(new IntPtr(ptr2), new IntPtr(ptr), sizeInBytesToCopy);
				ptr += RowStride;
				ptr2 += pixelBuffer.RowStride;
			}
		}

		/// <summary>
		/// Saves this pixel buffer to a file.
		/// </summary>
		/// <param name="fileName">The destination file.</param>
		/// <param name="fileType">Specify the output format.</param>
		/// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
		public void Save(string fileName, ImageFileType fileType)
		{
<<<<<<< HEAD
			using (NativeFileStream imageStream = new NativeFileStream(fileName, NativeFileMode.Create, NativeFileAccess.Write))
			{
				Save(imageStream, fileType);
			}
=======
			using NativeFileStream imageStream = new NativeFileStream(fileName, NativeFileMode.Create, NativeFileAccess.Write);
			Save(imageStream, fileType);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		/// <summary>
		/// Saves this pixel buffer to a stream.
		/// </summary>
		/// <param name="imageStream">The destination stream.</param>
		/// <param name="fileType">Specify the output format.</param>
		/// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
		public void Save(Stream imageStream, ImageFileType fileType)
		{
			ImageDescription imageDescription = default(ImageDescription);
			imageDescription.Width = width;
			imageDescription.Height = height;
			imageDescription.Depth = 1;
			imageDescription.ArraySize = 1;
			imageDescription.Dimension = TextureDimension.Texture2D;
			imageDescription.Format = format;
			imageDescription.MipLevels = 1;
			ImageDescription description = imageDescription;
			Image.Save(new PixelBuffer[1] { this }, 1, description, imageStream, fileType);
		}

		/// <summary>
		/// Gets the pixel value at a specified position.
		/// </summary>
		/// <typeparam name="T">Type of the pixel data</typeparam>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		/// <returns>The pixel value.</returns>
		/// <remarks>
		/// Caution, this method doesn't check bounding.
		/// </remarks>
		public unsafe T GetPixel<T>(int x, int y) where T : struct
		{
			return Utilities.Read<T>(new IntPtr((byte*)(void*)DataPointer + RowStride * y + x * PixelSize));
		}

		/// <summary>
		/// Gets the pixel value at a specified position.
		/// </summary>
		/// <typeparam name="T">Type of the pixel data</typeparam>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		/// <param name="value">The pixel value.</param>
		/// <remarks>
		/// Caution, this method doesn't check bounding.
		/// </remarks>
		public unsafe void SetPixel<T>(int x, int y, T value) where T : struct
		{
			Utilities.Write(new IntPtr((byte*)(void*)DataPointer + RowStride * y + x * PixelSize), ref value);
		}

		/// <summary>
		/// Gets scanline pixels from the buffer.
		/// </summary>
		/// <typeparam name="T">Type of the pixel data</typeparam>
		/// <param name="yOffset">The y line offset.</param>
		/// <returns>Scanline pixels from the buffer</returns>
		/// <exception cref="T:System.ArgumentException">If the sizeof(T) is an invalid size</exception>
		/// <remarks>
<<<<<<< HEAD
		/// This method is working on a row basis. The yOffset is specifying the first row to get 
=======
		/// This method is working on a row basis. The <see cref="!:yOffset" /> is specifying the first row to get 
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// the pixels from.
		/// </remarks>
		public T[] GetPixels<T>(int yOffset = 0) where T : struct
		{
			int num = Utilities.SizeOf<T>();
			int num2 = Width * Height * pixelSize;
			if (num2 % num != 0)
			{
				throw new ArgumentException($"Invalid sizeof(T), not a multiple of current size [{num2}]in bytes ");
			}
			T[] array = new T[num2 / num];
			GetPixels(array, yOffset);
			return array;
		}

		/// <summary>
		/// Gets scanline pixels from the buffer.
		/// </summary>
		/// <typeparam name="T">Type of the pixel data</typeparam>
		/// <param name="pixels">An allocated scanline pixel buffer</param>
		/// <param name="yOffset">The y line offset.</param>
		/// <returns>Scanline pixels from the buffer</returns>
		/// <exception cref="T:System.ArgumentException">If the sizeof(T) is an invalid size</exception>
		/// <remarks>
<<<<<<< HEAD
		/// This method is working on a row basis. The yOffset is specifying the first row to get 
=======
		/// This method is working on a row basis. The <see cref="!:yOffset" /> is specifying the first row to get 
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// the pixels from.
		/// </remarks>
		public void GetPixels<T>(T[] pixels, int yOffset = 0) where T : struct
		{
			GetPixels(pixels, yOffset, 0, pixels.Length);
		}

		/// <summary>
		/// Gets scanline pixels from the buffer.
		/// </summary>
		/// <typeparam name="T">Type of the pixel data</typeparam>
		/// <param name="pixels">An allocated scanline pixel buffer</param>
		/// <param name="yOffset">The y line offset.</param>
<<<<<<< HEAD
		/// <param name="pixelIndex">Offset into the destination pixels buffer.</param>
		/// <param name="pixelCount">Number of pixels to write into the destination pixels buffer.</param>
		/// <exception cref="T:System.ArgumentException">If the sizeof(T) is an invalid size</exception>
		/// <remarks>
		/// This method is working on a row basis. The yOffset is specifying the first row to get 
=======
		/// <param name="pixelIndex">Offset into the destination <see cref="!:pixels" /> buffer.</param>
		/// <param name="pixelCount">Number of pixels to write into the destination <see cref="!:pixels" /> buffer.</param>
		/// <exception cref="T:System.ArgumentException">If the sizeof(T) is an invalid size</exception>
		/// <remarks>
		/// This method is working on a row basis. The <see cref="!:yOffset" /> is specifying the first row to get 
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// the pixels from.
		/// </remarks>
		public unsafe void GetPixels<T>(T[] pixels, int yOffset, int pixelIndex, int pixelCount) where T : struct
		{
			byte* ptr = (byte*)(void*)DataPointer + yOffset * rowStride;
			if (isStrictRowStride)
			{
				Utilities.Read(new IntPtr(ptr), pixels, 0, pixelCount);
				return;
			}
			int num = Utilities.SizeOf<T>() * pixelCount;
			int num2 = num / Width;
			int num3 = num % Width;
			for (int i = 0; i < num2; i++)
			{
				Utilities.Read(new IntPtr(ptr), pixels, pixelIndex, Width);
				ptr += rowStride;
				pixelIndex += Width;
			}
			if (num3 > 0)
			{
				Utilities.Read(new IntPtr(ptr), pixels, pixelIndex, num3);
			}
		}

		/// <summary>
		/// Sets scanline pixels to the buffer.
		/// </summary>
		/// <typeparam name="T">Type of the pixel data</typeparam>
		/// <param name="sourcePixels">Source pixel buffer</param>
		/// <param name="yOffset">The y line offset.</param>
		/// <exception cref="T:System.ArgumentException">If the sizeof(T) is an invalid size</exception>
		/// <remarks>
<<<<<<< HEAD
		/// This method is working on a row basis. The yOffset is specifying the first row to get 
=======
		/// This method is working on a row basis. The <see cref="!:yOffset" /> is specifying the first row to get 
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// the pixels from.
		/// </remarks>
		public void SetPixels<T>(T[] sourcePixels, int yOffset = 0) where T : struct
		{
			SetPixels(sourcePixels, yOffset, 0, sourcePixels.Length);
		}

		/// <summary>
		/// Sets scanline pixels to the buffer.
		/// </summary>
		/// <typeparam name="T">Type of the pixel data</typeparam>
		/// <param name="sourcePixels">Source pixel buffer</param>
		/// <param name="yOffset">The y line offset.</param>
<<<<<<< HEAD
		/// <param name="pixelIndex">Offset into the source sourcePixels buffer.</param>
		/// <param name="pixelCount">Number of pixels to write into the source sourcePixels buffer.</param>
		/// <exception cref="T:System.ArgumentException">If the sizeof(T) is an invalid size</exception>
		/// <remarks>
		/// This method is working on a row basis. The yOffset is specifying the first row to get 
=======
		/// <param name="pixelIndex">Offset into the source <see cref="!:sourcePixels" /> buffer.</param>
		/// <param name="pixelCount">Number of pixels to write into the source <see cref="!:sourcePixels" /> buffer.</param>
		/// <exception cref="T:System.ArgumentException">If the sizeof(T) is an invalid size</exception>
		/// <remarks>
		/// This method is working on a row basis. The <see cref="!:yOffset" /> is specifying the first row to get 
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// the pixels from.
		/// </remarks>
		public unsafe void SetPixels<T>(T[] sourcePixels, int yOffset, int pixelIndex, int pixelCount) where T : struct
		{
			byte* ptr = (byte*)(void*)DataPointer + yOffset * rowStride;
			if (isStrictRowStride)
			{
				Utilities.Write(new IntPtr(ptr), sourcePixels, 0, pixelCount);
				return;
			}
			int num = Utilities.SizeOf<T>() * pixelCount;
			int num2 = num / Width;
			int num3 = num % Width;
			for (int i = 0; i < num2; i++)
			{
				Utilities.Write(new IntPtr(ptr), sourcePixels, pixelIndex, Width);
				ptr += rowStride;
				pixelIndex += Width;
			}
			if (num3 > 0)
			{
				Utilities.Write(new IntPtr(ptr), sourcePixels, pixelIndex, num3);
			}
		}
	}
}
