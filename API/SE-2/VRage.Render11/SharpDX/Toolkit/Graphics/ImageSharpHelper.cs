using System;
using System.IO;
using System.Runtime.InteropServices;
using SharpDX.DXGI;
using VRage.Render.Image;

namespace SharpDX.Toolkit.Graphics
{
	internal class ImageSharpHelper
	{
		/// <summary>
		/// Load a file in memory
		/// </summary>
		/// <param name="pSource">Source buffer</param>
		/// <param name="size">Size of the DDS texture.</param>
		/// <param name="makeACopy">Whether or not to make a copy of the DDS</param>
		/// <param name="handle"></param>
<<<<<<< HEAD
		/// <param name="debugName"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <returns></returns>
		public static Image LoadFromMemory(IntPtr pSource, int size, bool makeACopy, GCHandle? handle, string debugName)
		{
			if (!BuildImageData(MyImage.Load(pSource, size, debugName), out var description, out var imageData))
			{
				return null;
			}
			Image result = new Image(description, imageData.AddrOfPinnedObject(), 0, imageData, bufferIsDisposable: false);
			handle?.Free();
			if (!makeACopy && !handle.HasValue)
			{
				Utilities.FreeMemory(pSource);
			}
			return result;
		}

		public static ImageDescription? LoadImageHeader(Stream imageData, string debugName)
		{
			if (BuildImageData(MyImage.Load(imageData, oneChannel: false, headerOnly: true, debugName), out var description, out var _))
			{
				return description;
			}
			return null;
		}

		private static bool BuildImageData(IMyImage image, out ImageDescription description, out GCHandle imageData)
		{
			if (image == null)
			{
				description = default(ImageDescription);
				return false;
			}
			description = new ImageDescription
			{
				Width = image.Size.X,
				Height = image.Size.Y,
				ArraySize = 1,
				Depth = 1,
				Dimension = TextureDimension.Texture2D,
				MipLevels = 1
			};
			if (image != null)
			{
				if (!(image is MyImage<uint>))
				{
					if (!(image is MyImage<byte>))
					{
						if (!(image is MyImage<ushort>))
						{
							goto IL_009c;
						}
						description.Format = Format.R16_UNorm;
					}
					else
					{
						description.Format = Format.R8_UNorm;
					}
				}
				else
				{
					description.Format = Format.R8G8B8A8_UNorm_SRgb;
				}
				if (image.Data != null)
				{
					imageData = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
				}
				return true;
			}
			goto IL_009c;
			IL_009c:
			return false;
		}

		public static void SaveToDDSStream(PixelBuffer[] pixelBuffers, int count, ImageDescription description, Stream imageStream)
		{
		}
	}
}
