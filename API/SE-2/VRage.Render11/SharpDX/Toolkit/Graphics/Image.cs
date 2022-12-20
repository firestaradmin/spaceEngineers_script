using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using SharpDX.DXGI;
using SharpDX.IO;
using SharpDX.Toolkit.Content;

namespace SharpDX.Toolkit.Graphics
{
	/// <summary>
	/// Provides method to instantiate an image 1D/2D/3D supporting TextureArray and mipmaps on the CPU or to load/save an image from the disk.
	/// </summary>
	[ContentReader(typeof(ImageContentReader))]
	public sealed class Image : Component
	{
		public delegate ImageDescription? ImageLoadHeaderDelegate(Stream imageData, string debugName);

		public delegate Image ImageLoadDelegate(IntPtr dataPointer, int dataSize, bool makeACopy, GCHandle? handle, string debugName);

		public delegate void ImageSaveDelegate(PixelBuffer[] pixelBuffers, int count, ImageDescription description, Stream imageStream);

		[Flags]
		internal enum PitchFlags
		{
			None = 0x0,
			LegacyDword = 0x1,
			Bpp24 = 0x10000,
			Bpp16 = 0x20000,
			Bpp8 = 0x40000
		}

		private class LoadSaveDelegate
		{
			public readonly ImageFileType FileType;

			public readonly ImageLoadDelegate Load;

			public readonly ImageSaveDelegate Save;

			public readonly ImageLoadHeaderDelegate LoadHeader;

			public LoadSaveDelegate(ImageFileType fileType, ImageLoadDelegate load, ImageSaveDelegate save, ImageLoadHeaderDelegate loadHeader)
			{
				Load = load;
				Save = save;
				FileType = fileType;
				LoadHeader = loadHeader;
			}
		}

		/// <summary>
		/// Pixel buffers.
		/// </summary>
		internal PixelBuffer[] pixelBuffers;

		private DataBox[] dataBoxArray;

		private List<int> mipMapToZIndex;

		private int zBufferCountPerArraySlice;

		private MipMapDescription[] mipmapDescriptions;

		private static List<LoadSaveDelegate> loadSaveDelegates;

		/// <summary>
		/// Provides access to all pixel buffers.
		/// </summary>
		/// <remarks>
		/// For Texture3D, each z slice of the Texture3D has a pixelBufferArray * by the number of mipmaps.
		/// For other textures, there is Description.MipLevels * Description.ArraySize pixel buffers.
		/// </remarks>
		private PixelBufferArray pixelBufferArray;

		/// <summary>
		/// Gets the total number of bytes occupied by this image in memory.
		/// </summary>
		private int totalSizeInBytes;

		/// <summary>
		/// Pointer to the buffer.
		/// </summary>
		private IntPtr buffer;

		/// <summary>
		/// True if the buffer must be disposed.
		/// </summary>
		private bool bufferIsDisposable;

		/// <summary>
		/// Handle != null if the buffer is a pinned managed object on the LOH (Large Object Heap).
		/// </summary>
		private GCHandle? handle;

		/// <summary>
		/// Description of this image.
		/// </summary>
		public ImageDescription Description;

		/// <summary>
		/// Gets a pointer to the image buffer in memory.
		/// </summary>
		/// <value>A pointer to the image buffer in memory.</value>
		public IntPtr DataPointer => buffer;

		/// <summary>
		/// Provides access to all pixel buffers.
		/// </summary>
		/// <remarks>
		/// For Texture3D, each z slice of the Texture3D has a pixelBufferArray * by the number of mipmaps.
		/// For other textures, there is Description.MipLevels * Description.ArraySize pixel buffers.
		/// </remarks>
		public PixelBufferArray PixelBuffer => pixelBufferArray;

		/// <summary>
		/// Gets the total number of bytes occupied by this image in memory.
		/// </summary>
		public int TotalSizeInBytes => totalSizeInBytes;

		private Image()
		{
		}

		~Image()
		{
			if (handle.HasValue)
			{
				handle.Value.Free();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.Toolkit.Graphics.Image" /> class.
		/// </summary>
		/// <param name="description">The image description.</param>
		/// <param name="dataPointer">The pointer to the data buffer.</param>
		/// <param name="offset">The offset from the beginning of the data buffer.</param>
		/// <param name="handle">The handle (optional).</param>
		/// <param name="bufferIsDisposable">if set to <c>true</c> [buffer is disposable].</param>
<<<<<<< HEAD
		/// <param name="pitchFlags"></param>
		/// <exception cref="T:System.InvalidOperationException">If the format is invalid, or width/height/depth/array size is invalid with respect to the dimension.</exception>
=======
		/// <exception cref="T:System.InvalidOperationException">If the format is invalid, or width/height/depth/arraysize is invalid with respect to the dimension.</exception>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		internal Image(ImageDescription description, IntPtr dataPointer, int offset, GCHandle? handle, bool bufferIsDisposable, PitchFlags pitchFlags = PitchFlags.None)
		{
			Initialize(description, dataPointer, offset, handle, bufferIsDisposable, allocateData: true, pitchFlags);
		}

		internal Image(ImageDescription description)
		{
			Initialize(description, (IntPtr)0, 0, null, bufferIsDisposable: false, allocateData: false);
		}

		protected override void Dispose(bool disposeManagedResources)
		{
			if (handle.HasValue)
			{
				handle.Value.Free();
				handle = null;
			}
			if (bufferIsDisposable)
			{
				Utilities.FreeMemory(buffer);
			}
			base.Dispose(disposeManagedResources);
		}

		/// <summary>
		/// Gets the mipmap description of this instance for the specified mipmap level.
		/// </summary>
		/// <param name="mipmap">The mipmap.</param>
		/// <returns>A description of a particular mipmap for this texture.</returns>
		public MipMapDescription GetMipMapDescription(int mipmap)
		{
			return mipmapDescriptions[mipmap];
		}

		/// <summary>
		/// Gets the pixel buffer for the specified array/z slice and mipmap level.
		/// </summary>
		/// <param name="arrayOrZSliceIndex">For 3D image, the parameter is the Z slice, otherwise it is an index into the texture array.</param>
		/// <param name="mipmap">The mipmap.</param>
		/// <returns>A <see cref="F:SharpDX.Toolkit.Graphics.Image.pixelBufferArray" />.</returns>
		/// <exception cref="T:System.ArgumentException">If arrayOrZSliceIndex or mipmap are out of range.</exception>
		public PixelBuffer GetPixelBuffer(int arrayOrZSliceIndex, int mipmap)
		{
			if (mipmap > Description.MipLevels)
			{
				throw new ArgumentException("Invalid mipmap level", "mipmap");
			}
			if (Description.Dimension == TextureDimension.Texture3D)
			{
				if (arrayOrZSliceIndex > Description.Depth)
				{
					throw new ArgumentException("Invalid z slice index", "arrayOrZSliceIndex");
				}
				return GetPixelBufferUnsafe(0, arrayOrZSliceIndex, mipmap);
			}
			if (arrayOrZSliceIndex > Description.ArraySize)
			{
				throw new ArgumentException("Invalid array slice index", "arrayOrZSliceIndex");
			}
			return GetPixelBufferUnsafe(arrayOrZSliceIndex, 0, mipmap);
		}

		/// <summary>
		/// Gets the pixel buffer for the specified array/z slice and mipmap level.
		/// </summary>
		/// <param name="arrayIndex">Index into the texture array. Must be set to 0 for 3D images.</param>
		/// <param name="zIndex">Z index for 3D image. Must be set to 0 for all 1D/2D images.</param>
		/// <param name="mipmap">The mipmap.</param>
		/// <returns>A <see cref="F:SharpDX.Toolkit.Graphics.Image.pixelBufferArray" />.</returns>
		/// <exception cref="T:System.ArgumentException">If arrayIndex, zIndex or mipmap are out of range.</exception>
		public PixelBuffer GetPixelBuffer(int arrayIndex, int zIndex, int mipmap)
		{
			if (mipmap > Description.MipLevels)
			{
				throw new ArgumentException("Invalid mipmap level", "mipmap");
			}
			if (arrayIndex > Description.ArraySize)
			{
				throw new ArgumentException("Invalid array slice index", "arrayIndex");
			}
			if (zIndex > Description.Depth)
			{
				throw new ArgumentException("Invalid z slice index", "zIndex");
			}
			return GetPixelBufferUnsafe(arrayIndex, zIndex, mipmap);
		}

		/// <summary>
		/// Registers a loader/saver for a specified image file type.
		/// </summary>
		/// <param name="type">The file type (use integer and explicit casting to <see cref="T:SharpDX.Toolkit.Graphics.ImageFileType" /> to register other file format.</param>
		/// <param name="loader">The loader delegate (can be null).</param>
		/// <param name="saver">The saver delegate (can be null).</param>
<<<<<<< HEAD
		/// <param name="loadHeader"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <exception cref="T:System.ArgumentException"></exception>
		public static void Register(ImageFileType type, ImageLoadDelegate loader, ImageSaveDelegate saver, ImageLoadHeaderDelegate loadHeader)
		{
			if (loader == null && saver == null && loadHeader == null)
			{
				throw new ArgumentNullException("loader/saver", "Can set both loader and saver to null");
			}
			LoadSaveDelegate loadSaveDelegate = new LoadSaveDelegate(type, loader, saver, loadHeader);
			for (int i = 0; i < loadSaveDelegates.Count; i++)
			{
				if (loadSaveDelegates[i].FileType == type)
				{
					loadSaveDelegates[i] = loadSaveDelegate;
					return;
				}
			}
			loadSaveDelegates.Add(loadSaveDelegate);
		}

		/// <summary>
		/// Gets the databox from this image.
		/// </summary>
		/// <returns>The databox of this image.</returns>
		public DataBox[] ToDataBox()
		{
			return (DataBox[])dataBoxArray.Clone();
		}

		/// <summary>
		/// Gets the databox from this image.
		/// </summary>
		/// <returns>The databox of this image.</returns>
		private DataBox[] ComputeDataBox()
		{
			dataBoxArray = new DataBox[Description.ArraySize * Description.MipLevels];
			int num = 0;
			for (int i = 0; i < Description.ArraySize; i++)
			{
				for (int j = 0; j < Description.MipLevels; j++)
				{
					PixelBuffer pixelBufferUnsafe = GetPixelBufferUnsafe(i, 0, j);
					dataBoxArray[num].DataPointer = pixelBufferUnsafe.DataPointer;
					dataBoxArray[num].RowPitch = pixelBufferUnsafe.RowStride;
					dataBoxArray[num].SlicePitch = pixelBufferUnsafe.BufferStride;
					num++;
				}
			}
			return dataBoxArray;
		}

		/// <summary>
		/// Creates a new instance of <see cref="T:SharpDX.Toolkit.Graphics.Image" /> from an image description.
		/// </summary>
		/// <param name="description">The image description.</param>
		/// <returns>A new image.</returns>
		public static Image New(ImageDescription description)
		{
			return New(description, IntPtr.Zero);
		}

		/// <summary>
		/// Creates a new instance of a 1D <see cref="T:SharpDX.Toolkit.Graphics.Image" />.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="mipMapCount">The mip map count.</param>
		/// <param name="format">The format.</param>
		/// <param name="arraySize">Size of the array.</param>
		/// <returns>A new image.</returns>
		public static Image New1D(int width, MipMapCount mipMapCount, PixelFormat format, int arraySize = 1)
		{
			return New1D(width, mipMapCount, format, arraySize, IntPtr.Zero);
		}

		/// <summary>
		/// Creates a new instance of a 2D <see cref="T:SharpDX.Toolkit.Graphics.Image" />.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="mipMapCount">The mip map count.</param>
		/// <param name="format">The format.</param>
		/// <param name="arraySize">Size of the array.</param>
		/// <returns>A new image.</returns>
		public static Image New2D(int width, int height, MipMapCount mipMapCount, PixelFormat format, int arraySize = 1)
		{
			return New2D(width, height, mipMapCount, format, arraySize, IntPtr.Zero);
		}

		/// <summary>
		/// Creates a new instance of a Cube <see cref="T:SharpDX.Toolkit.Graphics.Image" />.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="mipMapCount">The mip map count.</param>
		/// <param name="format">The format.</param>
		/// <returns>A new image.</returns>
		public static Image NewCube(int width, MipMapCount mipMapCount, PixelFormat format)
		{
			return NewCube(width, mipMapCount, format, IntPtr.Zero);
		}

		/// <summary>
		/// Creates a new instance of a 3D <see cref="T:SharpDX.Toolkit.Graphics.Image" />.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="depth">The depth.</param>
		/// <param name="mipMapCount">The mip map count.</param>
		/// <param name="format">The format.</param>
		/// <returns>A new image.</returns>
		public static Image New3D(int width, int height, int depth, MipMapCount mipMapCount, PixelFormat format)
		{
			return New3D(width, height, depth, mipMapCount, format, IntPtr.Zero);
		}

		/// <summary>
		/// Creates a new instance of <see cref="T:SharpDX.Toolkit.Graphics.Image" /> from an image description.
		/// </summary>
		/// <param name="description">The image description.</param>
		/// <param name="dataPointer">Pointer to an existing buffer.</param>
		/// <returns>A new image.</returns>
		public static Image New(ImageDescription description, IntPtr dataPointer)
		{
			return new Image(description, dataPointer, 0, null, bufferIsDisposable: false);
		}

		/// <summary>
		/// Creates a new instance of a 1D <see cref="T:SharpDX.Toolkit.Graphics.Image" />.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="mipMapCount">The mip map count.</param>
		/// <param name="format">The format.</param>
		/// <param name="arraySize">Size of the array.</param>
		/// <param name="dataPointer">Pointer to an existing buffer.</param>
		/// <returns>A new image.</returns>
		public static Image New1D(int width, MipMapCount mipMapCount, PixelFormat format, int arraySize, IntPtr dataPointer)
		{
			return new Image(CreateDescription(TextureDimension.Texture1D, width, 1, 1, mipMapCount, format, arraySize), dataPointer, 0, null, bufferIsDisposable: false);
		}

		/// <summary>
		/// Creates a new instance of a 2D <see cref="T:SharpDX.Toolkit.Graphics.Image" />.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="mipMapCount">The mip map count.</param>
		/// <param name="format">The format.</param>
		/// <param name="arraySize">Size of the array.</param>
		/// <param name="dataPointer">Pointer to an existing buffer.</param>
		/// <returns>A new image.</returns>
		public static Image New2D(int width, int height, MipMapCount mipMapCount, PixelFormat format, int arraySize, IntPtr dataPointer)
		{
			return new Image(CreateDescription(TextureDimension.Texture2D, width, height, 1, mipMapCount, format, arraySize), dataPointer, 0, null, bufferIsDisposable: false);
		}

		/// <summary>
		/// Creates a new instance of a Cube <see cref="T:SharpDX.Toolkit.Graphics.Image" />.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="mipMapCount">The mip map count.</param>
		/// <param name="format">The format.</param>
		/// <param name="dataPointer">Pointer to an existing buffer.</param>
		/// <returns>A new image.</returns>
		public static Image NewCube(int width, MipMapCount mipMapCount, PixelFormat format, IntPtr dataPointer)
		{
			return new Image(CreateDescription(TextureDimension.TextureCube, width, width, 1, mipMapCount, format, 6), dataPointer, 0, null, bufferIsDisposable: false);
		}

		/// <summary>
		/// Creates a new instance of a 3D <see cref="T:SharpDX.Toolkit.Graphics.Image" />.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="depth">The depth.</param>
		/// <param name="mipMapCount">The mip map count.</param>
		/// <param name="format">The format.</param>
		/// <param name="dataPointer">Pointer to an existing buffer.</param>
		/// <returns>A new image.</returns>
		public static Image New3D(int width, int height, int depth, MipMapCount mipMapCount, PixelFormat format, IntPtr dataPointer)
		{
			return new Image(CreateDescription(TextureDimension.Texture3D, width, width, depth, mipMapCount, format, 1), dataPointer, 0, null, bufferIsDisposable: false);
		}

		/// <summary>
		/// Loads an image from an unmanaged memory pointer.
		/// </summary>
<<<<<<< HEAD
		/// <param name="dataBuffer">Pointer to an unmanaged memory. If makeACopy is false, this buffer must be allocated with <see cref="M:SharpDX.Utilities.AllocateMemory(System.Int32,System.Int32)" />.</param>
		/// <param name="debugName"></param>
		/// <param name="makeACopy">True to copy the content of the buffer to a new allocated buffer, false otherwhise.</param>
		/// <returns>An new image.</returns>
		/// <remarks>If makeACopy is set to false, the returned image is now the holder of the unmanaged pointer and will release it on Dispose. </remarks>
=======
		/// <param name="dataBuffer">Pointer to an unmanaged memory. If <see cref="!:makeACopy" /> is false, this buffer must be allocated with <see cref="M:SharpDX.Utilities.AllocateMemory(System.Int32,System.Int32)" />.</param>
		/// <param name="makeACopy">True to copy the content of the buffer to a new allocated buffer, false otherwhise.</param>
		/// <returns>An new image.</returns>
		/// <remarks>If <see cref="!:makeACopy" /> is set to false, the returned image is now the holder of the unmanaged pointer and will release it on Dispose. </remarks>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static Image Load(DataPointer dataBuffer, string debugName, bool makeACopy = false)
		{
			return Load(dataBuffer.Pointer, dataBuffer.Size, debugName, makeACopy);
		}

		/// <summary>
		/// Loads an image from an unmanaged memory pointer.
		/// </summary>
<<<<<<< HEAD
		/// <param name="dataPointer">Pointer to an unmanaged memory. If makeACopy is false, this buffer must be allocated with <see cref="M:SharpDX.Utilities.AllocateMemory(System.Int32,System.Int32)" />.</param>
		/// <param name="dataSize">Size of the unmanaged buffer.</param>
		/// <param name="debugName"></param>
		/// <param name="makeACopy">True to copy the content of the buffer to a new allocated buffer, false otherwise.</param>
		/// <returns>An new image.</returns>
		/// <remarks>If makeACopy is set to false, the returned image is now the holder of the unmanaged pointer and will release it on Dispose. </remarks>
=======
		/// <param name="dataPointer">Pointer to an unmanaged memory. If <see cref="!:makeACopy" /> is false, this buffer must be allocated with <see cref="M:SharpDX.Utilities.AllocateMemory(System.Int32,System.Int32)" />.</param>
		/// <param name="dataSize">Size of the unmanaged buffer.</param>
		/// <param name="makeACopy">True to copy the content of the buffer to a new allocated buffer, false otherwise.</param>
		/// <returns>An new image.</returns>
		/// <remarks>If <see cref="!:makeACopy" /> is set to false, the returned image is now the holder of the unmanaged pointer and will release it on Dispose. </remarks>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static Image Load(IntPtr dataPointer, int dataSize, string debugName, bool makeACopy = false)
		{
			return Load(dataPointer, dataSize, makeACopy, null, debugName);
		}

		/// <summary>
		/// Loads an image from a managed buffer.
		/// </summary>
		/// <param name="buffer">Reference to a managed buffer.</param>
<<<<<<< HEAD
		/// <param name="debugName"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <returns>An new image.</returns>
		/// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
		public unsafe static Image Load(byte[] buffer, string debugName)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = buffer.Length;
			if (num > 87040)
			{
				GCHandle value = GCHandle.Alloc(buffer, GCHandleType.Pinned);
				return Load(value.AddrOfPinnedObject(), num, makeACopy: false, value, debugName);
			}
			fixed (byte* ptr = buffer)
			{
				void* ptr2 = ptr;
				return Load((IntPtr)ptr2, num, debugName, makeACopy: true);
			}
		}

		/// <summary>
		/// Loads the specified image from a stream.
		/// </summary>
		/// <param name="imageStream">The image stream.</param>
<<<<<<< HEAD
		/// <param name="debugName"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <returns>An new image.</returns>
		/// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
		public static Image Load(Stream imageStream, string debugName)
		{
			NativeFileStream nativeFileStream = imageStream as NativeFileStream;
			if (nativeFileStream != null)
			{
				IntPtr intPtr = IntPtr.Zero;
				Image image = null;
				try
				{
					int num = (int)nativeFileStream.Length;
					intPtr = Utilities.AllocateMemory(num);
					nativeFileStream.Read(intPtr, 0, num);
					image = Load(intPtr, num, debugName);
				}
				finally
				{
					if (image == null)
					{
						Utilities.FreeMemory(intPtr);
					}
				}
				return image;
			}
			return Load(Utilities.ReadStream(imageStream), debugName);
		}

		/// <summary>
		/// Loads the specified image from a file.
		/// </summary>
		/// <param name="fileName">The filename.</param>
		/// <returns>An new image.</returns>
		/// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
		public static Image Load(string fileName)
		{
			NativeFileStream nativeFileStream = null;
			IntPtr intPtr = IntPtr.Zero;
			int num;
			try
			{
				nativeFileStream = new NativeFileStream(fileName, NativeFileMode.Open, NativeFileAccess.Read);
				num = (int)nativeFileStream.Length;
				intPtr = Utilities.AllocateMemory(num);
				nativeFileStream.Read(intPtr, 0, num);
			}
			catch (Exception)
			{
				if (intPtr != IntPtr.Zero)
				{
					Utilities.FreeMemory(intPtr);
				}
				throw;
			}
			finally
			{
				try
				{
					nativeFileStream?.Dispose();
				}
				catch
				{
				}
			}
			return Load(intPtr, num, fileName);
		}

		public static ImageDescription? LoadImageDescription(Stream imageStream, string debugName)
		{
			long position = imageStream.Position;
			foreach (LoadSaveDelegate loadSaveDelegate in loadSaveDelegates)
			{
				ImageDescription? result = loadSaveDelegate.LoadHeader?.Invoke(imageStream, debugName);
				if (result.HasValue)
				{
					return result;
				}
				imageStream.Position = position;
			}
			return null;
		}

		/// <summary>
		/// Saves this instance to a file.
		/// </summary>
		/// <param name="fileName">The destination file. Filename must end with a known extension (dds, bmp, jpg, png, gif, tiff, wmp, tga)</param>
		public void Save(string fileName)
		{
			string extension = Path.GetExtension(fileName);
			extension = extension ?? string.Empty;
<<<<<<< HEAD
			ImageFileType fileType;
			switch (extension.TrimStart(new char[1] { '.' }).ToLower())
			{
			case "jpg":
				fileType = ImageFileType.Jpg;
				break;
			case "dds":
				fileType = ImageFileType.Dds;
				break;
			case "gif":
				fileType = ImageFileType.Gif;
				break;
			case "bmp":
				fileType = ImageFileType.Bmp;
				break;
			case "png":
				fileType = ImageFileType.Png;
				break;
			case "tga":
				fileType = ImageFileType.Tga;
				break;
			case "tiff":
				fileType = ImageFileType.Tiff;
				break;
			case "tktx":
				fileType = ImageFileType.Tktx;
				break;
			case "wmp":
				fileType = ImageFileType.Wmp;
				break;
			default:
				throw new ArgumentException("Filename must have a supported image extension: dds, bmp, jpg, png, gif, tiff, wmp, tga");
			}
			Save(fileName, fileType);
=======
			Save(fileName, extension.TrimStart(new char[1] { '.' }).ToLower() switch
			{
				"jpg" => ImageFileType.Jpg, 
				"dds" => ImageFileType.Dds, 
				"gif" => ImageFileType.Gif, 
				"bmp" => ImageFileType.Bmp, 
				"png" => ImageFileType.Png, 
				"tga" => ImageFileType.Tga, 
				"tiff" => ImageFileType.Tiff, 
				"tktx" => ImageFileType.Tktx, 
				"wmp" => ImageFileType.Wmp, 
				_ => throw new ArgumentException("Filename must have a supported image extension: dds, bmp, jpg, png, gif, tiff, wmp, tga"), 
			});
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		/// <summary>
		/// Saves this instance to a file.
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
		/// Saves this instance to a stream.
		/// </summary>
		/// <param name="imageStream">The destination stream.</param>
		/// <param name="fileType">Specify the output format.</param>
		/// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
		public void Save(Stream imageStream, ImageFileType fileType)
		{
			Save(pixelBuffers, pixelBuffers.Length, Description, imageStream, fileType);
		}

		/// <summary>
		/// Loads an image from the specified pointer.
		/// </summary>
		/// <param name="dataPointer">The data pointer.</param>
		/// <param name="dataSize">Size of the data.</param>
		/// <param name="makeACopy">if set to <c>true</c> [make A copy].</param>
		/// <param name="handle">The handle.</param>
<<<<<<< HEAD
		/// <param name="debugName"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <returns></returns>
		/// <exception cref="T:System.NotSupportedException"></exception>
		private static Image Load(IntPtr dataPointer, int dataSize, bool makeACopy, GCHandle? handle, string debugName)
		{
			foreach (LoadSaveDelegate loadSaveDelegate in loadSaveDelegates)
			{
				if (loadSaveDelegate.Load != null)
				{
					Image image = loadSaveDelegate.Load(dataPointer, dataSize, makeACopy, handle, debugName);
					if (image != null)
					{
						return image;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Saves this instance to a stream.
		/// </summary>
		/// <param name="pixelBuffers">The buffers to save.</param>
		/// <param name="count">The number of buffers to save.</param>
		/// <param name="description">Global description of the buffer.</param>
		/// <param name="imageStream">The destination stream.</param>
		/// <param name="fileType">Specify the output format.</param>
		/// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
		internal static void Save(PixelBuffer[] pixelBuffers, int count, ImageDescription description, Stream imageStream, ImageFileType fileType)
		{
			foreach (LoadSaveDelegate loadSaveDelegate in loadSaveDelegates)
			{
				if (loadSaveDelegate.FileType == fileType)
				{
					loadSaveDelegate.Save(pixelBuffers, count, description, imageStream);
					return;
				}
			}
			throw new NotSupportedException("This file format is not yet implemented.");
		}

		static Image()
		{
			loadSaveDelegates = new List<LoadSaveDelegate>();
			Register(ImageFileType.Dds, DDSHelper.LoadFromDDSMemory, DDSHelper.SaveToDDSStream, DDSHelper.ReadDDSHeader);
			Register(ImageFileType.ImageSharp, ImageSharpHelper.LoadFromMemory, ImageSharpHelper.SaveToDDSStream, ImageSharpHelper.LoadImageHeader);
		}

		private static int CountMips(int width, int height)
		{
			int num = 1;
			while (height > 1 || width > 1)
			{
				num++;
				if (height > 1)
				{
					height >>= 1;
				}
				if (width > 1)
				{
					width >>= 1;
				}
			}
			return num;
		}

		/// <summary>
		/// Calculates the number of miplevels for a Texture 2D.
		/// </summary>
		/// <param name="width">The width of the texture.</param>
		/// <param name="height">The height of the texture.</param>
		/// <param name="mipLevels">A <see cref="T:SharpDX.Toolkit.Graphics.MipMapCount" />, set to true to calculates all mipmaps, to false to calculate only 1 miplevel, or &gt; 1 to calculate a specific amount of levels.</param>
		/// <returns>The number of miplevels.</returns>
		public static int CalculateMipLevels(int width, int height, MipMapCount mipLevels)
		{
			if ((int)mipLevels <= 1)
			{
				mipLevels = ((!(mipLevels == 0)) ? ((MipMapCount)1) : ((MipMapCount)CountMips(width, height)));
			}
			else
			{
				int num = CountMips(width, height);
				if ((int)mipLevels > num)
				{
					throw new InvalidOperationException($"MipLevels must be <= {num}");
				}
			}
			return mipLevels;
		}

		private static bool IsPow2(int x)
		{
			if (x != 0)
			{
				return (x & (x - 1)) == 0;
			}
			return false;
		}

		private static int CountMips(int width, int height, int depth)
		{
			int num = 1;
			while (height > 1 || width > 1 || depth > 1)
			{
				num++;
				if (height > 1)
				{
					height >>= 1;
				}
				if (width > 1)
				{
					width >>= 1;
				}
				if (depth > 1)
				{
					depth >>= 1;
				}
			}
			return num;
		}

		/// <summary>
		/// Calculates the number of miplevels for a Texture 2D.
		/// </summary>
		/// <param name="width">The width of the texture.</param>
		/// <param name="height">The height of the texture.</param>
		/// <param name="depth">The depth of the texture.</param>
		/// <param name="mipLevels">A <see cref="T:SharpDX.Toolkit.Graphics.MipMapCount" />, set to true to calculates all mipmaps, to false to calculate only 1 miplevel, or &gt; 1 to calculate a specific amount of levels.</param>
		/// <returns>The number of miplevels.</returns>
		public static int CalculateMipLevels(int width, int height, int depth, MipMapCount mipLevels)
		{
			if ((int)mipLevels > 1)
			{
				if (!IsPow2(width) || !IsPow2(height) || !IsPow2(depth))
				{
					throw new InvalidOperationException("Width/Height/Depth must be power of 2");
				}
				int num = CountMips(width, height, depth);
				if ((int)mipLevels > num)
				{
					throw new InvalidOperationException($"MipLevels must be <= {num}");
				}
			}
			else if (mipLevels == 0)
			{
				if (!IsPow2(width) || !IsPow2(height) || !IsPow2(depth))
				{
					throw new InvalidOperationException("Width/Height/Depth must be power of 2");
				}
				mipLevels = CountMips(width, height, depth);
			}
			else
			{
				mipLevels = 1;
			}
			return mipLevels;
		}

		internal unsafe void Initialize(ImageDescription description, IntPtr dataPointer, int offset, GCHandle? handle, bool bufferIsDisposable, bool allocateData, PitchFlags pitchFlags = PitchFlags.None)
		{
			if (!description.Format.IsValid() || description.Format.IsVideo())
			{
				throw new InvalidOperationException("Unsupported DXGI Format");
			}
			if (this.handle.HasValue)
			{
				this.handle.Value.Free();
			}
			this.handle = handle;
			switch (description.Dimension)
			{
			case TextureDimension.Texture1D:
				if (description.Width <= 0 || description.Height != 1 || description.Depth != 1 || description.ArraySize == 0)
				{
					throw new InvalidOperationException("Invalid Width/Height/Depth/ArraySize for Image 1D");
				}
				description.MipLevels = CalculateMipLevels(description.Width, 1, description.MipLevels);
				break;
			case TextureDimension.Texture2D:
			case TextureDimension.TextureCube:
				if (description.Width <= 0 || description.Height <= 0 || description.Depth != 1 || description.ArraySize == 0)
				{
					throw new InvalidOperationException("Invalid Width/Height/Depth/ArraySize for Image 2D");
				}
				if (description.Dimension == TextureDimension.TextureCube && description.ArraySize % 6 != 0)
				{
					throw new InvalidOperationException("TextureCube must have an arraysize = 6");
				}
				description.MipLevels = CalculateMipLevels(description.Width, description.Height, description.MipLevels);
				break;
			case TextureDimension.Texture3D:
				if (description.Width <= 0 || description.Height <= 0 || description.Depth <= 0 || description.ArraySize != 1)
				{
					throw new InvalidOperationException("Invalid Width/Height/Depth/ArraySize for Image 3D");
				}
				description.MipLevels = CalculateMipLevels(description.Width, description.Height, description.Depth, description.MipLevels);
				break;
			}
			Description = description;
			if (allocateData)
			{
				mipMapToZIndex = CalculateImageArray(description, pitchFlags, out var bufferCount, out totalSizeInBytes);
				mipmapDescriptions = CalculateMipMapDescription(description, pitchFlags);
				zBufferCountPerArraySlice = mipMapToZIndex[mipMapToZIndex.Count - 1];
				pixelBuffers = new PixelBuffer[bufferCount];
				pixelBufferArray = new PixelBufferArray(this);
				this.bufferIsDisposable = !handle.HasValue && bufferIsDisposable;
				buffer = dataPointer;
				if (dataPointer == IntPtr.Zero)
				{
					buffer = Utilities.AllocateMemory(totalSizeInBytes);
					offset = 0;
					this.bufferIsDisposable = true;
				}
				SetupImageArray((IntPtr)((byte*)(void*)buffer + offset), totalSizeInBytes, description, pitchFlags, pixelBuffers);
				dataBoxArray = ComputeDataBox();
			}
		}

		private PixelBuffer GetPixelBufferUnsafe(int arrayIndex, int zIndex, int mipmap)
		{
			int num = mipMapToZIndex[mipmap];
			int num2 = arrayIndex * zBufferCountPerArraySlice + num + zIndex;
			return pixelBuffers[num2];
		}

		private static ImageDescription CreateDescription(TextureDimension dimension, int width, int height, int depth, MipMapCount mipMapCount, PixelFormat format, int arraySize)
		{
			ImageDescription result = default(ImageDescription);
			result.Width = width;
			result.Height = height;
			result.Depth = depth;
			result.ArraySize = arraySize;
			result.Dimension = dimension;
			result.Format = format;
			result.MipLevels = mipMapCount;
			return result;
		}

		internal static void ComputePitch(Format fmt, int width, int height, out int rowPitch, out int slicePitch, out int widthCount, out int heightCount, PitchFlags flags = PitchFlags.None)
		{
			widthCount = width;
			heightCount = height;
			if (fmt.IsCompressed())
			{
				int num = ((fmt == Format.BC1_Typeless || fmt == Format.BC1_UNorm || fmt == Format.BC1_UNorm_SRgb || fmt == Format.BC4_Typeless || fmt == Format.BC4_UNorm || fmt == Format.BC4_SNorm) ? 8 : 16);
				widthCount = Math.Max(1, (width + 3) / 4);
				heightCount = Math.Max(1, (height + 3) / 4);
				rowPitch = widthCount * num;
				slicePitch = rowPitch * heightCount;
				return;
			}
			if (fmt.IsPacked())
			{
				rowPitch = (width + 1 >> 1) * 4;
				slicePitch = rowPitch * height;
				return;
			}
			int num2 = (((flags & PitchFlags.Bpp24) != 0) ? 24 : (((flags & PitchFlags.Bpp16) != 0) ? 16 : (((flags & PitchFlags.Bpp8) == 0) ? fmt.SizeOfInBits() : 8)));
			if ((flags & PitchFlags.LegacyDword) != 0)
			{
				rowPitch = (width * num2 + 31) / 32 * 4;
				slicePitch = rowPitch * height;
			}
			else
			{
				rowPitch = (width * num2 + 7) / 8;
				slicePitch = rowPitch * height;
			}
		}

		internal static MipMapDescription[] CalculateMipMapDescription(ImageDescription metadata, PitchFlags cpFlags = PitchFlags.None)
		{
			int nImages;
			int pixelSize;
			return CalculateMipMapDescription(metadata, cpFlags, out nImages, out pixelSize);
		}

		internal static MipMapDescription[] CalculateMipMapDescription(ImageDescription metadata, PitchFlags cpFlags, out int nImages, out int pixelSize)
		{
			pixelSize = 0;
			nImages = 0;
			int num = metadata.Width;
			int num2 = metadata.Height;
			int num3 = metadata.Depth;
			MipMapDescription[] array = new MipMapDescription[metadata.MipLevels];
			for (int i = 0; i < metadata.MipLevels; i++)
			{
				ComputePitch(metadata.Format, num, num2, out var rowPitch, out var slicePitch, out var widthCount, out var heightCount);
				array[i] = new MipMapDescription(num, num2, num3, rowPitch, slicePitch, widthCount, heightCount);
				pixelSize += num3 * slicePitch;
				nImages += num3;
				if (num2 > 1)
				{
					num2 >>= 1;
				}
				if (num > 1)
				{
					num >>= 1;
				}
				if (num3 > 1)
				{
					num3 >>= 1;
				}
			}
			return array;
		}

		/// <summary>
		/// Determines number of image array entries and pixel size.
		/// </summary>
		/// <param name="imageDesc">Description of the image to create.</param>
		/// <param name="pitchFlags">Pitch flags.</param>
		/// <param name="bufferCount">Output number of mipmap.</param>
		/// <param name="pixelSizeInBytes">Output total size to allocate pixel buffers for all images.</param>
		private static List<int> CalculateImageArray(ImageDescription imageDesc, PitchFlags pitchFlags, out int bufferCount, out int pixelSizeInBytes)
		{
			pixelSizeInBytes = 0;
			bufferCount = 0;
			List<int> list = new List<int>();
			for (int i = 0; i < imageDesc.ArraySize; i++)
			{
				int num = imageDesc.Width;
				int num2 = imageDesc.Height;
				int num3 = imageDesc.Depth;
				for (int j = 0; j < imageDesc.MipLevels; j++)
				{
					ComputePitch(imageDesc.Format, num, num2, out var _, out var slicePitch, out var _, out var _, pitchFlags);
					if (i == 0)
					{
						list.Add(bufferCount);
					}
					pixelSizeInBytes += num3 * slicePitch;
					bufferCount += num3;
					if (num2 > 1)
					{
						num2 >>= 1;
					}
					if (num > 1)
					{
						num >>= 1;
					}
					if (num3 > 1)
					{
						num3 >>= 1;
					}
				}
				if (i == 0)
				{
					list.Add(bufferCount);
				}
			}
			return list;
		}

		/// <summary>
		/// Allocates PixelBuffers 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="pixelSize"></param>
		/// <param name="imageDesc"></param>
		/// <param name="pitchFlags"></param>
		/// <param name="output"></param>
		private unsafe static void SetupImageArray(IntPtr buffer, int pixelSize, ImageDescription imageDesc, PitchFlags pitchFlags, PixelBuffer[] output)
		{
			int num = 0;
			byte* ptr = (byte*)(void*)buffer;
			for (uint num2 = 0u; num2 < imageDesc.ArraySize; num2++)
			{
				int num3 = imageDesc.Width;
				int num4 = imageDesc.Height;
				int num5 = imageDesc.Depth;
				for (uint num6 = 0u; num6 < imageDesc.MipLevels; num6++)
				{
					ComputePitch(imageDesc.Format, num3, num4, out var rowPitch, out var slicePitch, out var _, out var _, pitchFlags);
					for (uint num7 = 0u; num7 < num5; num7++)
					{
						output[num] = new PixelBuffer(num3, num4, imageDesc.Format, rowPitch, slicePitch, (IntPtr)ptr);
						num++;
						ptr += slicePitch;
					}
					if (num4 > 1)
					{
						num4 >>= 1;
					}
					if (num3 > 1)
					{
						num3 >>= 1;
					}
					if (num5 > 1)
					{
						num5 >>= 1;
					}
				}
			}
		}
	}
}
