using System;
using System.IO;
using System.Runtime.InteropServices;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.IO;
using SharpDX.Multimedia;

namespace SharpDX.Toolkit.Graphics
{
	internal class DDSHelper
	{
		[Flags]
		public enum ConversionFlags
		{
			None = 0x0,
			Expand = 0x1,
			NoAlpha = 0x2,
			Swizzle = 0x4,
			Pal8 = 0x8,
			Format888 = 0x10,
			Format565 = 0x20,
			Format5551 = 0x40,
			Format4444 = 0x80,
			Format44 = 0x100,
			Format332 = 0x200,
			Format8332 = 0x400,
			FormatA8P8 = 0x800,
			CopyMemory = 0x1000,
			DX10 = 0x10000
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct LegacyMap
		{
			public Format Format;

			public ConversionFlags ConversionFlags;

			public DDS.PixelFormat PixelFormat;

			/// <summary>
			/// Initializes a new instance of the <see cref="T:SharpDX.Toolkit.Graphics.DDSHelper.LegacyMap" /> struct.
			/// </summary>
			/// <param name="format">The format.</param>
			/// <param name="conversionFlags">The conversion flags.</param>
			/// <param name="pixelFormat">The pixel format.</param>
			public LegacyMap(Format format, ConversionFlags conversionFlags, DDS.PixelFormat pixelFormat)
			{
				Format = format;
				ConversionFlags = conversionFlags;
				PixelFormat = pixelFormat;
			}
		}

		private enum TEXP_LEGACY_FORMAT
		{
			UNKNOWN,
			R8G8B8,
			R3G3B2,
			A8R3G3B2,
			P8,
			A8P8,
			A4L4,
			B4G4R4A4
		}

		public delegate int MipSkipSelector(in ImageDescription description);

		[Flags]
		internal enum ScanlineFlags
		{
			None = 0x0,
			SetAlpha = 0x1,
			Legacy = 0x2
		}

		private static readonly LegacyMap[] LegacyMaps = new LegacyMap[44]
		{
			new LegacyMap(Format.BC1_UNorm, ConversionFlags.None, DDS.PixelFormat.DXT1),
			new LegacyMap(Format.BC2_UNorm, ConversionFlags.None, DDS.PixelFormat.DXT3),
			new LegacyMap(Format.BC3_UNorm, ConversionFlags.None, DDS.PixelFormat.DXT5),
			new LegacyMap(Format.BC2_UNorm, ConversionFlags.None, DDS.PixelFormat.DXT2),
			new LegacyMap(Format.BC3_UNorm, ConversionFlags.None, DDS.PixelFormat.DXT4),
			new LegacyMap(Format.BC4_UNorm, ConversionFlags.None, DDS.PixelFormat.BC4_UNorm),
			new LegacyMap(Format.BC4_SNorm, ConversionFlags.None, DDS.PixelFormat.BC4_SNorm),
			new LegacyMap(Format.BC5_UNorm, ConversionFlags.None, DDS.PixelFormat.BC5_UNorm),
			new LegacyMap(Format.BC5_SNorm, ConversionFlags.None, DDS.PixelFormat.BC5_SNorm),
			new LegacyMap(Format.BC4_UNorm, ConversionFlags.None, new DDS.PixelFormat(DDS.PixelFormatFlags.FourCC, new FourCC('A', 'T', 'I', '1'), 0, 0u, 0u, 0u, 0u)),
			new LegacyMap(Format.BC5_UNorm, ConversionFlags.None, new DDS.PixelFormat(DDS.PixelFormatFlags.FourCC, new FourCC('A', 'T', 'I', '2'), 0, 0u, 0u, 0u, 0u)),
			new LegacyMap(Format.R8G8_B8G8_UNorm, ConversionFlags.None, DDS.PixelFormat.R8G8_B8G8),
			new LegacyMap(Format.G8R8_G8B8_UNorm, ConversionFlags.None, DDS.PixelFormat.G8R8_G8B8),
			new LegacyMap(Format.B8G8R8A8_UNorm, ConversionFlags.None, DDS.PixelFormat.A8R8G8B8),
			new LegacyMap(Format.B8G8R8X8_UNorm, ConversionFlags.None, DDS.PixelFormat.X8R8G8B8),
			new LegacyMap(Format.R8G8B8A8_UNorm, ConversionFlags.None, DDS.PixelFormat.A8B8G8R8),
			new LegacyMap(Format.R8G8B8A8_UNorm, ConversionFlags.NoAlpha, DDS.PixelFormat.X8B8G8R8),
			new LegacyMap(Format.R16G16_UNorm, ConversionFlags.None, DDS.PixelFormat.G16R16),
			new LegacyMap(Format.R10G10B10A2_UNorm, ConversionFlags.Swizzle, new DDS.PixelFormat(DDS.PixelFormatFlags.Rgb, 0, 32, 1023u, 1047552u, 1072693248u, 3221225472u)),
			new LegacyMap(Format.R10G10B10A2_UNorm, ConversionFlags.None, new DDS.PixelFormat(DDS.PixelFormatFlags.Rgb, 0, 32, 1072693248u, 1047552u, 1023u, 3221225472u)),
			new LegacyMap(Format.R8G8B8A8_UNorm, ConversionFlags.Expand | ConversionFlags.NoAlpha | ConversionFlags.Format888, DDS.PixelFormat.R8G8B8),
			new LegacyMap(Format.B5G6R5_UNorm, ConversionFlags.Format565, DDS.PixelFormat.R5G6B5),
			new LegacyMap(Format.B5G5R5A1_UNorm, ConversionFlags.Format5551, DDS.PixelFormat.A1R5G5B5),
			new LegacyMap(Format.B5G5R5A1_UNorm, ConversionFlags.NoAlpha | ConversionFlags.Format5551, new DDS.PixelFormat(DDS.PixelFormatFlags.Rgb, 0, 16, 31744u, 992u, 31u, 0u)),
			new LegacyMap(Format.R8G8B8A8_UNorm, ConversionFlags.Expand | ConversionFlags.Format8332, new DDS.PixelFormat(DDS.PixelFormatFlags.Rgb, 0, 16, 224u, 28u, 3u, 65280u)),
			new LegacyMap(Format.B5G6R5_UNorm, ConversionFlags.Expand | ConversionFlags.Format332, new DDS.PixelFormat(DDS.PixelFormatFlags.Rgb, 0, 8, 224u, 28u, 3u, 0u)),
			new LegacyMap(Format.R8_UNorm, ConversionFlags.None, DDS.PixelFormat.L8),
			new LegacyMap(Format.R16_UNorm, ConversionFlags.None, DDS.PixelFormat.L16),
			new LegacyMap(Format.R8G8_UNorm, ConversionFlags.None, DDS.PixelFormat.A8L8),
			new LegacyMap(Format.A8_UNorm, ConversionFlags.None, DDS.PixelFormat.A8),
			new LegacyMap(Format.R16G16B16A16_UNorm, ConversionFlags.None, new DDS.PixelFormat(DDS.PixelFormatFlags.FourCC, 36, 0, 0u, 0u, 0u, 0u)),
			new LegacyMap(Format.R16G16B16A16_SNorm, ConversionFlags.None, new DDS.PixelFormat(DDS.PixelFormatFlags.FourCC, 110, 0, 0u, 0u, 0u, 0u)),
			new LegacyMap(Format.R16_Float, ConversionFlags.None, new DDS.PixelFormat(DDS.PixelFormatFlags.FourCC, 111, 0, 0u, 0u, 0u, 0u)),
			new LegacyMap(Format.R16G16_Float, ConversionFlags.None, new DDS.PixelFormat(DDS.PixelFormatFlags.FourCC, 112, 0, 0u, 0u, 0u, 0u)),
			new LegacyMap(Format.R16G16B16A16_Float, ConversionFlags.None, new DDS.PixelFormat(DDS.PixelFormatFlags.FourCC, 113, 0, 0u, 0u, 0u, 0u)),
			new LegacyMap(Format.R32_Float, ConversionFlags.None, new DDS.PixelFormat(DDS.PixelFormatFlags.FourCC, 114, 0, 0u, 0u, 0u, 0u)),
			new LegacyMap(Format.R32G32_Float, ConversionFlags.None, new DDS.PixelFormat(DDS.PixelFormatFlags.FourCC, 115, 0, 0u, 0u, 0u, 0u)),
			new LegacyMap(Format.R32G32B32A32_Float, ConversionFlags.None, new DDS.PixelFormat(DDS.PixelFormatFlags.FourCC, 116, 0, 0u, 0u, 0u, 0u)),
			new LegacyMap(Format.R32_Float, ConversionFlags.None, new DDS.PixelFormat(DDS.PixelFormatFlags.Rgb, 0, 32, uint.MaxValue, 0u, 0u, 0u)),
			new LegacyMap(Format.R8G8B8A8_UNorm, ConversionFlags.Expand | ConversionFlags.Pal8 | ConversionFlags.FormatA8P8, new DDS.PixelFormat(DDS.PixelFormatFlags.Pal8, 0, 16, 0u, 0u, 0u, 0u)),
			new LegacyMap(Format.R8G8B8A8_UNorm, ConversionFlags.Expand | ConversionFlags.Pal8, new DDS.PixelFormat(DDS.PixelFormatFlags.Pal8, 0, 8, 0u, 0u, 0u, 0u)),
			new LegacyMap(Format.R8G8B8A8_UNorm, ConversionFlags.Expand | ConversionFlags.Format4444, DDS.PixelFormat.A4R4G4B4),
			new LegacyMap(Format.R8G8B8A8_UNorm, ConversionFlags.Expand | ConversionFlags.NoAlpha | ConversionFlags.Format4444, new DDS.PixelFormat(DDS.PixelFormatFlags.Rgb, 0, 16, 3840u, 240u, 15u, 0u)),
			new LegacyMap(Format.R8G8B8A8_UNorm, ConversionFlags.Expand | ConversionFlags.Format44, new DDS.PixelFormat(DDS.PixelFormatFlags.Luminance, 0, 8, 15u, 0u, 0u, 240u))
		};

		private static Format GetDXGIFormat(ref DDS.PixelFormat pixelFormat, DDSFlags flags, out ConversionFlags conversionFlags)
		{
			conversionFlags = ConversionFlags.None;
			int num = 0;
			for (num = 0; num < LegacyMaps.Length; num++)
			{
				LegacyMap legacyMap = LegacyMaps[num];
				if ((pixelFormat.Flags & legacyMap.PixelFormat.Flags) == 0)
				{
					continue;
				}
				if ((legacyMap.PixelFormat.Flags & DDS.PixelFormatFlags.FourCC) != 0)
				{
					if (pixelFormat.FourCC == legacyMap.PixelFormat.FourCC)
					{
						break;
					}
				}
				else if ((legacyMap.PixelFormat.Flags & DDS.PixelFormatFlags.Pal8) != 0)
				{
					if (pixelFormat.RGBBitCount == legacyMap.PixelFormat.RGBBitCount)
					{
						break;
					}
				}
				else if (pixelFormat.RGBBitCount == legacyMap.PixelFormat.RGBBitCount && pixelFormat.RBitMask == legacyMap.PixelFormat.RBitMask && pixelFormat.GBitMask == legacyMap.PixelFormat.GBitMask && pixelFormat.BBitMask == legacyMap.PixelFormat.BBitMask && pixelFormat.ABitMask == legacyMap.PixelFormat.ABitMask)
				{
					break;
				}
			}
			if (num >= LegacyMaps.Length)
			{
				return Format.Unknown;
			}
			conversionFlags = LegacyMaps[num].ConversionFlags;
			Format format = LegacyMaps[num].Format;
			if ((conversionFlags & ConversionFlags.Expand) != 0 && (flags & DDSFlags.NoLegacyExpansion) != 0)
			{
				return Format.Unknown;
			}
			if (format == Format.R10G10B10A2_UNorm && (flags & DDSFlags.NoR10B10G10A2Fixup) != 0)
			{
				conversionFlags ^= ConversionFlags.Swizzle;
			}
			return format;
		}

		public static ImageDescription? ReadDDSHeader(Stream stream, string debugName)
		{
			if (TryReadDDSHeader(stream, out var description, out var _))
			{
				return description;
			}
			return null;
		}

		public unsafe static bool TryReadDDSHeader(Stream stream, out ImageDescription description, out ConversionFlags convFlags)
		{
			int num = 4 + Utilities.SizeOf<DDS.Header>() + Utilities.SizeOf<DDS.HeaderDXT10>();
			if (stream.Length < num)
			{
				description = default(ImageDescription);
				convFlags = ConversionFlags.None;
				return false;
			}
			byte[] array = new byte[num];
			stream.Read(array, 0, num);
			fixed (byte* value = array)
			{
				return DecodeDDSHeader(new IntPtr(value), num, DDSFlags.None, out description, out convFlags);
			}
		}

		/// <summary>
		/// Decodes DDS header including optional DX10 extended header
		/// </summary>
		/// <param name="headerPtr">Pointer to the DDS header.</param>
		/// <param name="size">Size of the DDS content.</param>
		/// <param name="flags">Flags used for decoding the DDS header.</param>
		/// <param name="description">Output texture description.</param>
		/// <param name="convFlags">Output conversion flags.</param>
		/// <exception cref="T:System.ArgumentException">If the argument headerPtr is null</exception>
		/// <exception cref="T:System.InvalidOperationException">If the DDS header contains invalid data.</exception>
		/// <returns>True if the decoding is successful, false if this is not a DDS header.</returns>
		private unsafe static bool DecodeDDSHeader(IntPtr headerPtr, int size, DDSFlags flags, out ImageDescription description, out ConversionFlags convFlags)
		{
			description = default(ImageDescription);
			convFlags = ConversionFlags.None;
			if (headerPtr == IntPtr.Zero)
			{
				throw new ArgumentException("Pointer to DDS header cannot be null", "headerPtr");
			}
			if (size < Utilities.SizeOf<DDS.Header>() + 4)
			{
				return false;
			}
			if (*(uint*)(void*)headerPtr != 542327876)
			{
				return false;
			}
			DDS.Header header = *(DDS.Header*)((byte*)(void*)headerPtr + 4);
			if (header.Size != Utilities.SizeOf<DDS.Header>() || header.PixelFormat.Size != Utilities.SizeOf<DDS.PixelFormat>())
			{
				return false;
			}
			description.MipLevels = header.MipMapCount;
			if (description.MipLevels == 0)
			{
				description.MipLevels = 1;
			}
			if ((header.PixelFormat.Flags & DDS.PixelFormatFlags.FourCC) != 0 && new FourCC('D', 'X', '1', '0') == header.PixelFormat.FourCC)
			{
				if (size < Utilities.SizeOf<DDS.Header>() + 4 + Utilities.SizeOf<DDS.HeaderDXT10>())
				{
					return false;
				}
				DDS.HeaderDXT10 headerDXT = *(DDS.HeaderDXT10*)((byte*)(void*)headerPtr + 4 + Utilities.SizeOf<DDS.Header>());
				convFlags |= ConversionFlags.DX10;
				description.ArraySize = headerDXT.ArraySize;
				if (description.ArraySize == 0)
				{
					throw new InvalidOperationException("Unexpected ArraySize == 0 from DDS HeaderDX10 ");
				}
				description.Format = headerDXT.DXGIFormat;
				if (!description.Format.IsValid())
				{
					throw new InvalidOperationException("Invalid Format from DDS HeaderDX10 ");
				}
				switch (headerDXT.ResourceDimension)
				{
				case ResourceDimension.Texture1D:
					if ((header.Flags & DDS.HeaderFlags.Height) != 0 && header.Height != 1)
					{
						throw new InvalidOperationException("Unexpected Height != 1 from DDS HeaderDX10 ");
					}
					description.Width = header.Width;
					description.Height = 1;
					description.Depth = 1;
					description.Dimension = TextureDimension.Texture1D;
					break;
				case ResourceDimension.Texture2D:
					if ((headerDXT.MiscFlags & ResourceOptionFlags.TextureCube) != 0)
					{
						description.ArraySize *= 6;
						description.Dimension = TextureDimension.TextureCube;
					}
					else
					{
						description.Dimension = TextureDimension.Texture2D;
					}
					description.Width = header.Width;
					description.Height = header.Height;
					description.Depth = 1;
					break;
				case ResourceDimension.Texture3D:
					if ((header.Flags & DDS.HeaderFlags.Volume) == 0)
					{
						throw new InvalidOperationException("Texture3D missing HeaderFlags.Volume from DDS HeaderDX10");
					}
					if (description.ArraySize > 1)
					{
						throw new InvalidOperationException("Unexpected ArraySize > 1 for Texture3D from DDS HeaderDX10");
					}
					description.Width = header.Width;
					description.Height = header.Height;
					description.Depth = header.Depth;
					description.Dimension = TextureDimension.Texture3D;
					break;
				default:
					throw new InvalidOperationException($"Unexpected dimension [{headerDXT.ResourceDimension}] from DDS HeaderDX10");
				}
			}
			else
			{
				description.ArraySize = 1;
				if ((header.Flags & DDS.HeaderFlags.Volume) != 0)
				{
					description.Width = header.Width;
					description.Height = header.Height;
					description.Depth = header.Depth;
					description.Dimension = TextureDimension.Texture3D;
				}
				else
				{
					if ((header.CubemapFlags & DDS.CubemapFlags.CubeMap) != 0)
					{
						if ((header.CubemapFlags & DDS.CubemapFlags.AllFaces) != DDS.CubemapFlags.AllFaces)
						{
							throw new InvalidOperationException("Unexpected CubeMap, expecting all faces from DDS Header");
						}
						description.ArraySize = 6;
						description.Dimension = TextureDimension.TextureCube;
					}
					else
					{
						description.Dimension = TextureDimension.Texture2D;
					}
					description.Width = header.Width;
					description.Height = header.Height;
					description.Depth = 1;
				}
				description.Format = GetDXGIFormat(ref header.PixelFormat, flags, out convFlags);
				if (description.Format == Format.Unknown)
				{
					throw new InvalidOperationException("Unsupported PixelFormat from DDS Header");
				}
			}
			if ((flags & DDSFlags.ForceRgb) != 0)
			{
				switch (description.Format)
				{
				case Format.B8G8R8A8_UNorm:
					description.Format = Format.R8G8B8A8_UNorm;
					convFlags |= ConversionFlags.Swizzle;
					break;
				case Format.B8G8R8X8_UNorm:
					description.Format = Format.R8G8B8A8_UNorm;
					convFlags |= ConversionFlags.NoAlpha | ConversionFlags.Swizzle;
					break;
				case Format.B8G8R8A8_Typeless:
					description.Format = Format.R8G8B8A8_Typeless;
					convFlags |= ConversionFlags.Swizzle;
					break;
				case Format.B8G8R8A8_UNorm_SRgb:
					description.Format = Format.R8G8B8A8_UNorm_SRgb;
					convFlags |= ConversionFlags.Swizzle;
					break;
				case Format.B8G8R8X8_Typeless:
					description.Format = Format.R8G8B8A8_Typeless;
					convFlags |= ConversionFlags.NoAlpha | ConversionFlags.Swizzle;
					break;
				case Format.B8G8R8X8_UNorm_SRgb:
					description.Format = Format.R8G8B8A8_UNorm_SRgb;
					convFlags |= ConversionFlags.NoAlpha | ConversionFlags.Swizzle;
					break;
				}
			}
			if ((flags & DDSFlags.CopyMemory) != 0)
			{
				convFlags |= ConversionFlags.CopyMemory;
			}
			if ((flags & DDSFlags.No16Bpp) != 0)
			{
				Format format = description.Format;
				if ((uint)(format - 85) <= 1u)
				{
					description.Format = Format.R8G8B8A8_UNorm;
					convFlags |= ConversionFlags.Expand;
					if (description.Format == Format.B5G6R5_UNorm)
					{
						convFlags |= ConversionFlags.NoAlpha;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Encodes DDS file header (magic value, header, optional DX10 extended header)
		/// </summary>
		/// <param name="flags">Flags used for decoding the DDS header.</param>
		/// <param name="description">Output texture description.</param>
		/// <param name="pDestination">Pointer to the DDS output header. Can be set to IntPtr.Zero to calculated the required bytes.</param>
		/// <param name="maxsize">The maximum size of the destination buffer.</param>
		/// <param name="required">Output the number of bytes required to write the DDS header.</param>
		/// <exception cref="T:System.ArgumentException">If the argument headerPtr is null</exception>
		/// <exception cref="T:System.InvalidOperationException">If the DDS header contains invalid data.</exception>
		/// <returns>True if the decoding is successful, false if this is not a DDS header.</returns>
		private unsafe static void EncodeDDSHeader(ImageDescription description, DDSFlags flags, IntPtr pDestination, int maxsize, out int required)
		{
			if (description.ArraySize > 1 && (description.ArraySize != 6 || description.Dimension != TextureDimension.Texture2D || description.Dimension != TextureDimension.TextureCube))
			{
				flags |= DDSFlags.ForceDX10Ext;
			}
			DDS.PixelFormat pixelFormat = default(DDS.PixelFormat);
			if ((flags & DDSFlags.ForceDX10Ext) == 0)
			{
				switch (description.Format)
				{
				case Format.R8G8B8A8_UNorm:
					pixelFormat = DDS.PixelFormat.A8B8G8R8;
					break;
				case Format.R16G16_UNorm:
					pixelFormat = DDS.PixelFormat.G16R16;
					break;
				case Format.R8G8_UNorm:
					pixelFormat = DDS.PixelFormat.A8L8;
					break;
				case Format.R16_UNorm:
					pixelFormat = DDS.PixelFormat.L16;
					break;
				case Format.R8_UNorm:
					pixelFormat = DDS.PixelFormat.L8;
					break;
				case Format.A8_UNorm:
					pixelFormat = DDS.PixelFormat.A8;
					break;
				case Format.R8G8_B8G8_UNorm:
					pixelFormat = DDS.PixelFormat.R8G8_B8G8;
					break;
				case Format.G8R8_G8B8_UNorm:
					pixelFormat = DDS.PixelFormat.G8R8_G8B8;
					break;
				case Format.BC1_UNorm:
					pixelFormat = DDS.PixelFormat.DXT1;
					break;
				case Format.BC2_UNorm:
					pixelFormat = DDS.PixelFormat.DXT3;
					break;
				case Format.BC3_UNorm:
					pixelFormat = DDS.PixelFormat.DXT5;
					break;
				case Format.BC4_UNorm:
					pixelFormat = DDS.PixelFormat.BC4_UNorm;
					break;
				case Format.BC4_SNorm:
					pixelFormat = DDS.PixelFormat.BC4_SNorm;
					break;
				case Format.BC5_UNorm:
					pixelFormat = DDS.PixelFormat.BC5_UNorm;
					break;
				case Format.BC5_SNorm:
					pixelFormat = DDS.PixelFormat.BC5_SNorm;
					break;
				case Format.B5G6R5_UNorm:
					pixelFormat = DDS.PixelFormat.R5G6B5;
					break;
				case Format.B5G5R5A1_UNorm:
					pixelFormat = DDS.PixelFormat.A1R5G5B5;
					break;
				case Format.B8G8R8A8_UNorm:
					pixelFormat = DDS.PixelFormat.A8R8G8B8;
					break;
				case Format.B8G8R8X8_UNorm:
					pixelFormat = DDS.PixelFormat.X8R8G8B8;
					break;
				case Format.R32G32B32A32_Float:
					pixelFormat.Size = Utilities.SizeOf<DDS.PixelFormat>();
					pixelFormat.Flags = DDS.PixelFormatFlags.FourCC;
					pixelFormat.FourCC = 116;
					break;
				case Format.R16G16B16A16_Float:
					pixelFormat.Size = Utilities.SizeOf<DDS.PixelFormat>();
					pixelFormat.Flags = DDS.PixelFormatFlags.FourCC;
					pixelFormat.FourCC = 113;
					break;
				case Format.R16G16B16A16_UNorm:
					pixelFormat.Size = Utilities.SizeOf<DDS.PixelFormat>();
					pixelFormat.Flags = DDS.PixelFormatFlags.FourCC;
					pixelFormat.FourCC = 36;
					break;
				case Format.R16G16B16A16_SNorm:
					pixelFormat.Size = Utilities.SizeOf<DDS.PixelFormat>();
					pixelFormat.Flags = DDS.PixelFormatFlags.FourCC;
					pixelFormat.FourCC = 110;
					break;
				case Format.R32G32_Float:
					pixelFormat.Size = Utilities.SizeOf<DDS.PixelFormat>();
					pixelFormat.Flags = DDS.PixelFormatFlags.FourCC;
					pixelFormat.FourCC = 115;
					break;
				case Format.R16G16_Float:
					pixelFormat.Size = Utilities.SizeOf<DDS.PixelFormat>();
					pixelFormat.Flags = DDS.PixelFormatFlags.FourCC;
					pixelFormat.FourCC = 112;
					break;
				case Format.R32_Float:
					pixelFormat.Size = Utilities.SizeOf<DDS.PixelFormat>();
					pixelFormat.Flags = DDS.PixelFormatFlags.FourCC;
					pixelFormat.FourCC = 114;
					break;
				case Format.R16_Float:
					pixelFormat.Size = Utilities.SizeOf<DDS.PixelFormat>();
					pixelFormat.Flags = DDS.PixelFormatFlags.FourCC;
					pixelFormat.FourCC = 111;
					break;
				}
			}
			required = 4 + Utilities.SizeOf<DDS.Header>();
			if (pixelFormat.Size == 0)
			{
				required += Utilities.SizeOf<DDS.HeaderDXT10>();
			}
			if (pDestination == IntPtr.Zero)
			{
				return;
			}
			if (maxsize < required)
			{
				throw new ArgumentException("Not enough size for destination buffer", "maxsize");
			}
			*(int*)(void*)pDestination = 542327876;
			DDS.Header* ptr = (DDS.Header*)((byte*)(void*)pDestination + 4);
			Utilities.ClearMemory((IntPtr)ptr, 0, Utilities.SizeOf<DDS.Header>());
			ptr->Size = Utilities.SizeOf<DDS.Header>();
			ptr->Flags = DDS.HeaderFlags.Texture;
			ptr->SurfaceFlags = DDS.SurfaceFlags.Texture;
			if (description.MipLevels > 0)
			{
				ptr->Flags |= DDS.HeaderFlags.Mipmap;
				ptr->MipMapCount = description.MipLevels;
				if (ptr->MipMapCount > 1)
				{
					ptr->SurfaceFlags |= DDS.SurfaceFlags.Mipmap;
				}
			}
			switch (description.Dimension)
			{
			case TextureDimension.Texture1D:
				ptr->Height = description.Height;
				ptr->Width = (ptr->Depth = 1);
				break;
			case TextureDimension.Texture2D:
			case TextureDimension.TextureCube:
				ptr->Height = description.Height;
				ptr->Width = description.Width;
				ptr->Depth = 1;
				if (description.Dimension == TextureDimension.TextureCube)
				{
					ptr->SurfaceFlags |= DDS.SurfaceFlags.Cubemap;
					ptr->CubemapFlags |= DDS.CubemapFlags.AllFaces;
				}
				break;
			case TextureDimension.Texture3D:
				ptr->Flags |= DDS.HeaderFlags.Volume;
				ptr->CubemapFlags |= DDS.CubemapFlags.Volume;
				ptr->Height = description.Height;
				ptr->Width = description.Width;
				ptr->Depth = description.Depth;
				break;
			}
			Image.ComputePitch(description.Format, description.Width, description.Height, out var rowPitch, out var slicePitch, out var _, out var _);
			if (description.Format.IsCompressed())
			{
				ptr->Flags |= DDS.HeaderFlags.LinearSize;
				ptr->PitchOrLinearSize = slicePitch;
			}
			else
			{
				ptr->Flags |= DDS.HeaderFlags.Pitch;
				ptr->PitchOrLinearSize = rowPitch;
			}
			if (pixelFormat.Size == 0)
			{
				ptr->PixelFormat = DDS.PixelFormat.DX10;
				DDS.HeaderDXT10* ptr2 = (DDS.HeaderDXT10*)((byte*)ptr + Utilities.SizeOf<DDS.Header>());
				Utilities.ClearMemory((IntPtr)ptr2, 0, Utilities.SizeOf<DDS.HeaderDXT10>());
				ptr2->DXGIFormat = description.Format;
				switch (description.Dimension)
				{
				case TextureDimension.Texture1D:
					ptr2->ResourceDimension = ResourceDimension.Texture1D;
					break;
				case TextureDimension.Texture2D:
				case TextureDimension.TextureCube:
					ptr2->ResourceDimension = ResourceDimension.Texture2D;
					break;
				case TextureDimension.Texture3D:
					ptr2->ResourceDimension = ResourceDimension.Texture3D;
					break;
				}
				if (description.Dimension == TextureDimension.TextureCube)
				{
					ptr2->MiscFlags |= ResourceOptionFlags.TextureCube;
					ptr2->ArraySize = description.ArraySize / 6;
				}
				else
				{
					ptr2->ArraySize = description.ArraySize;
				}
			}
			else
			{
				ptr->PixelFormat = pixelFormat;
			}
		}

		private static TEXP_LEGACY_FORMAT FindLegacyFormat(ConversionFlags flags)
		{
			TEXP_LEGACY_FORMAT result = TEXP_LEGACY_FORMAT.UNKNOWN;
			if ((flags & ConversionFlags.Pal8) != 0)
			{
				result = (((flags & ConversionFlags.FormatA8P8) != 0) ? TEXP_LEGACY_FORMAT.A8P8 : TEXP_LEGACY_FORMAT.P8);
			}
			else if ((flags & ConversionFlags.Format888) != 0)
			{
				result = TEXP_LEGACY_FORMAT.R8G8B8;
			}
			else if ((flags & ConversionFlags.Format332) != 0)
			{
				result = TEXP_LEGACY_FORMAT.R3G3B2;
			}
			else if ((flags & ConversionFlags.Format8332) != 0)
			{
				result = TEXP_LEGACY_FORMAT.A8R3G3B2;
			}
			else if ((flags & ConversionFlags.Format44) != 0)
			{
				result = TEXP_LEGACY_FORMAT.A4L4;
			}
			return result;
		}

<<<<<<< HEAD
		/// <summary>
		/// Converts an image row with optional clearing of alpha value to 1.0
		/// </summary>
		/// <param name="pDestination"></param>
		/// <param name="outSize"></param>
		/// <param name="outFormat"></param>
		/// <param name="pSource"></param>
		/// <param name="inSize"></param>
		/// <param name="inFormat"></param>
		/// <param name="pal8"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private unsafe static bool LegacyExpandScanline(IntPtr pDestination, int outSize, Format outFormat, IntPtr pSource, int inSize, TEXP_LEGACY_FORMAT inFormat, int* pal8, ScanlineFlags flags)
		{
			switch (inFormat)
			{
			case TEXP_LEGACY_FORMAT.R8G8B8:
			{
				if (outFormat != Format.R8G8B8A8_UNorm)
				{
					return false;
				}
				byte* ptr15 = (byte*)(void*)pSource;
				int* ptr16 = (int*)(void*)pDestination;
				int num36 = 0;
				int num37 = 0;
				while (num37 < inSize && num36 < outSize)
				{
					int num38 = *ptr15 << 16;
					int num39 = ptr15[1] << 8;
					int num40 = ptr15[2];
					int* intPtr11 = ptr16;
					ptr16 = intPtr11 + 1;
					*intPtr11 = (int)(num38 | num39 | num40 | 0xFF000000u);
					ptr15 += 3;
					num37 += 3;
					num36 += 4;
				}
				return true;
			}
			case TEXP_LEGACY_FORMAT.R3G3B2:
				switch (outFormat)
				{
				case Format.R8G8B8A8_UNorm:
				{
					byte* ptr7 = (byte*)(void*)pSource;
					int* ptr8 = (int*)(void*)pDestination;
					int num20 = 0;
					int num21 = 0;
					while (num21 < inSize && num20 < outSize)
					{
						byte b2 = *(ptr7++);
						int num22 = (b2 & 0xE0) | ((b2 & 0xE0) >> 3) | ((b2 & 0xC0) >> 6);
						int num23 = ((b2 & 0x1C) << 11) | ((b2 & 0x1C) << 8) | ((b2 & 0x18) << 5);
						int num24 = ((b2 & 3) << 22) | ((b2 & 3) << 20) | ((b2 & 3) << 18) | ((b2 & 3) << 16);
						int* intPtr6 = ptr8;
						ptr8 = intPtr6 + 1;
						*intPtr6 = (int)(num22 | num23 | num24 | 0xFF000000u);
						num21++;
						num20 += 4;
					}
					return true;
				}
				case Format.B5G6R5_UNorm:
				{
					byte* ptr5 = (byte*)(void*)pSource;
					short* ptr6 = (short*)(void*)pDestination;
					int num15 = 0;
					int num16 = 0;
					while (num16 < inSize && num15 < outSize)
					{
						byte b = *(ptr5++);
						short num17 = (short)(((b & 0xE0) << 8) | ((b & 0xC0) << 5));
						short num18 = (short)(((b & 0x1C) << 6) | ((b & 0x1C) << 3));
						short num19 = (short)(((b & 3) << 3) | ((b & 3) << 1) | ((b & 2) >> 1));
						short* intPtr5 = ptr6;
						ptr6 = intPtr5 + 1;
						*intPtr5 = (short)(num17 | num18 | num19);
						num16++;
						num15 += 2;
					}
					return true;
				}
				}
				break;
			case TEXP_LEGACY_FORMAT.A8R3G3B2:
			{
				if (outFormat != Format.R8G8B8A8_UNorm)
				{
					return false;
				}
				short* ptr3 = (short*)(void*)pSource;
				int* ptr4 = (int*)(void*)pDestination;
				int num8 = 0;
				int num9 = 0;
				while (num9 < inSize && num8 < outSize)
				{
					short* intPtr3 = ptr3;
					ptr3 = intPtr3 + 1;
					short num10 = *intPtr3;
					int num11 = (num10 & 0xE0) | ((num10 & 0xE0) >> 3) | ((num10 & 0xC0) >> 6);
					int num12 = ((num10 & 0x1C) << 11) | ((num10 & 0x1C) << 8) | ((num10 & 0x18) << 5);
					int num13 = ((num10 & 3) << 22) | ((num10 & 3) << 20) | ((num10 & 3) << 18) | ((num10 & 3) << 16);
					uint num14 = (((flags & ScanlineFlags.SetAlpha) != 0) ? 4278190080u : ((uint)((num10 & 0xFF00) << 16)));
					int* intPtr4 = ptr4;
					ptr4 = intPtr4 + 1;
					*intPtr4 = (int)(num11 | num12 | num13 | num14);
					num9 += 2;
					num8 += 4;
				}
				return true;
			}
			case TEXP_LEGACY_FORMAT.P8:
			{
				if (outFormat != Format.R8G8B8A8_UNorm || pal8 == null)
				{
					return false;
				}
				byte* ptr11 = (byte*)(void*)pSource;
				int* ptr12 = (int*)(void*)pDestination;
				int num30 = 0;
				int num31 = 0;
				while (num31 < inSize && num30 < outSize)
				{
					byte b3 = *(ptr11++);
					int* intPtr9 = ptr12;
					ptr12 = intPtr9 + 1;
					*intPtr9 = pal8[(int)b3];
					num31++;
					num30 += 4;
				}
				return true;
			}
			case TEXP_LEGACY_FORMAT.A8P8:
			{
				if (outFormat != Format.R8G8B8A8_UNorm || pal8 == null)
				{
					return false;
				}
				short* ptr9 = (short*)(void*)pSource;
				int* ptr10 = (int*)(void*)pDestination;
				int num25 = 0;
				int num26 = 0;
				while (num26 < inSize && num25 < outSize)
				{
					short* intPtr7 = ptr9;
					ptr9 = intPtr7 + 1;
					short num27 = *intPtr7;
					int num28 = pal8[num27 & 0xFF];
					uint num29 = (((flags & ScanlineFlags.SetAlpha) != 0) ? 4278190080u : ((uint)((num27 & 0xFF00) << 16)));
					int* intPtr8 = ptr10;
					ptr10 = intPtr8 + 1;
					*intPtr8 = (int)(num28 | num29);
					num26 += 2;
					num25 += 4;
				}
				return true;
			}
			case TEXP_LEGACY_FORMAT.A4L4:
				if (outFormat == Format.R8G8B8A8_UNorm)
				{
					byte* ptr13 = (byte*)(void*)pSource;
					int* ptr14 = (int*)(void*)pDestination;
					int num32 = 0;
					int num33 = 0;
					while (num33 < inSize && num32 < outSize)
					{
						byte b4 = *(ptr13++);
						int num34 = ((b4 & 0xF) << 4) | (b4 & 0xF);
						uint num35 = (((flags & ScanlineFlags.SetAlpha) != 0) ? 4278190080u : ((uint)(((b4 & 0xF0) << 24) | ((b4 & 0xF0) << 20))));
						int* intPtr10 = ptr14;
						ptr14 = intPtr10 + 1;
						*intPtr10 = (int)(num34 | (num34 << 8) | (num34 << 16) | num35);
						num33++;
						num32 += 4;
					}
					return true;
				}
				break;
			case TEXP_LEGACY_FORMAT.B4G4R4A4:
			{
				if (outFormat != Format.R8G8B8A8_UNorm)
				{
					return false;
				}
				short* ptr = (short*)(void*)pSource;
				int* ptr2 = (int*)(void*)pDestination;
				int num = 0;
				int num2 = 0;
				while (num2 < inSize && num < outSize)
				{
					short* intPtr = ptr;
					ptr = intPtr + 1;
					short num3 = *intPtr;
					int num4 = ((num3 & 0xF00) >> 4) | ((num3 & 0xF00) >> 8);
					int num5 = ((num3 & 0xF0) << 8) | ((num3 & 0xF0) << 4);
					int num6 = ((num3 & 0xF) << 20) | ((num3 & 0xF) << 16);
					uint num7 = (((flags & ScanlineFlags.SetAlpha) != 0) ? 4278190080u : ((uint)(((num3 & 0xF000) << 16) | ((num3 & 0xF000) << 12))));
					int* intPtr2 = ptr2;
					ptr2 = intPtr2 + 1;
					*intPtr2 = (int)(num4 | num5 | num6 | num7);
					num2 += 2;
					num += 4;
				}
				return true;
			}
			}
			return false;
		}

		/// <summary>
		/// Load a DDS file in memory
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
		public unsafe static Image LoadFromDDSMemory(IntPtr pSource, int size, bool makeACopy, GCHandle? handle, string debugName)
		{
			DDSFlags dDSFlags = (makeACopy ? DDSFlags.CopyMemory : DDSFlags.None);
			if (!DecodeDDSHeader(pSource, size, dDSFlags, out var description, out var convFlags))
			{
				return null;
			}
			int num = 4 + Utilities.SizeOf<DDS.Header>();
			if ((convFlags & ConversionFlags.DX10) != 0)
			{
				num += Utilities.SizeOf<DDS.HeaderDXT10>();
			}
			int* pal = null;
			if ((convFlags & ConversionFlags.Pal8) != 0)
			{
				pal = (int*)((byte*)(void*)pSource + num);
				num += 1024;
			}
			if (size < num)
			{
				throw new InvalidOperationException();
			}
			return CreateImageFromDDS(pSource, num, size - num, description, ((dDSFlags & DDSFlags.LegacyDword) != 0) ? Image.PitchFlags.LegacyDword : Image.PitchFlags.None, convFlags, pal, handle);
		}

		/// <summary>
<<<<<<< HEAD
		/// Tries to load a image file to memory
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="selector"></param>
		/// <param name="image"></param>
		/// <param name="mipsSkipped"></param>
		/// <param name="loadFailed"></param>
=======
		/// Load a DDS file in memory
		/// </summary>
		/// <param name="pSource">Source buffer</param>
		/// <param name="size">Size of the DDS texture.</param>
		/// <param name="makeACopy">Whether or not to make a copy of the DDS</param>
		/// <param name="handle"></param>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <returns></returns>
		public static bool TryLoadDDSStreamWithMipSelection(Stream stream, MipSkipSelector selector, out Image image, out int mipsSkipped, out bool loadFailed)
		{
			image = null;
			loadFailed = false;
			if (!TryReadDDSHeader(stream, out var description, out var convFlags))
			{
				mipsSkipped = 0;
				return false;
			}
			if (description.ArraySize > 1)
			{
				mipsSkipped = 0;
				return false;
			}
			if (!description.Format.IsCompressed())
			{
				mipsSkipped = 0;
				return false;
			}
			int num = selector(in description);
			int num2 = 4 + Utilities.SizeOf<DDS.Header>();
			if ((convFlags & ConversionFlags.DX10) != 0)
			{
				num2 += Utilities.SizeOf<DDS.HeaderDXT10>();
			}
			long length = stream.Length;
			if (length < num2)
			{
				throw new InvalidOperationException("Stream is too short");
			}
			stream.Seek(num2, SeekOrigin.Begin);
			image = CreateCompressedImageFromStream(stream, description, (int)length, num);
			if (image == null)
			{
				loadFailed = true;
				mipsSkipped = 0;
				return false;
			}
			mipsSkipped = num;
			return true;
		}

		public static void SaveToDDSStream(PixelBuffer[] pixelBuffers, int count, ImageDescription description, Stream imageStream)
		{
			SaveToDDSStream(pixelBuffers, count, description, DDSFlags.None, imageStream);
		}

		public unsafe static void SaveToDDSStream(PixelBuffer[] pixelBuffers, int count, ImageDescription metadata, DDSFlags flags, Stream stream)
		{
			int required = 0;
			int num = 0;
			EncodeDDSHeader(metadata, flags, IntPtr.Zero, 0, out required);
			num = required;
			int num2 = 0;
			for (int i = 0; i < pixelBuffers.Length; i++)
			{
				int bufferStride = pixelBuffers[i].BufferStride;
				required += bufferStride;
				if (bufferStride > num2)
				{
					num2 = bufferStride;
				}
			}
			byte[] array = new byte[Math.Max(num2, num)];
			fixed (byte* ptr = array)
			{
				void* ptr2 = ptr;
				EncodeDDSHeader(metadata, flags, (IntPtr)ptr2, num, out var _);
				stream.Write(array, 0, num);
			}
			int num3 = 0;
			for (int j = 0; j < metadata.ArraySize; j++)
			{
				int num4 = metadata.Depth;
				for (int k = 0; k < metadata.MipLevels; k++)
				{
					for (int l = 0; l < num4; l++)
					{
						int bufferStride2 = pixelBuffers[num3].BufferStride;
						Utilities.Read(pixelBuffers[num3].DataPointer, array, 0, bufferStride2);
						stream.Write(array, 0, bufferStride2);
						num3++;
					}
					if (num4 > 1)
					{
						num4 >>= 1;
					}
				}
			}
		}

		/// <summary>
		/// Converts or copies image data from pPixels into scratch image data
		/// </summary>
		/// <param name="pDDS"></param>
		/// <param name="offset"></param>
		/// <param name="size"></param>
		/// <param name="metadata"></param>
		/// <param name="cpFlags"></param>
		/// <param name="convFlags"></param>
		/// <param name="pal8"></param>
		/// <param name="handle"></param>
		/// <returns></returns>
		private unsafe static Image CreateImageFromDDS(IntPtr pDDS, int offset, int size, ImageDescription metadata, Image.PitchFlags cpFlags, ConversionFlags convFlags, int* pal8, GCHandle? handle)
		{
			if ((convFlags & ConversionFlags.Expand) != 0)
			{
				if ((convFlags & ConversionFlags.Format888) != 0)
				{
					cpFlags |= Image.PitchFlags.Bpp24;
				}
				else if ((convFlags & (ConversionFlags.Format565 | ConversionFlags.Format5551 | ConversionFlags.Format4444 | ConversionFlags.Format8332 | ConversionFlags.FormatA8P8)) != 0)
				{
					cpFlags |= Image.PitchFlags.Bpp16;
				}
				else if ((convFlags & (ConversionFlags.Pal8 | ConversionFlags.Format44 | ConversionFlags.Format332)) != 0)
				{
					cpFlags |= Image.PitchFlags.Bpp8;
				}
			}
			bool flag = (convFlags & (ConversionFlags.Expand | ConversionFlags.CopyMemory)) != 0 || (cpFlags & Image.PitchFlags.LegacyDword) != 0;
			Image image = new Image(metadata, pDDS, offset, handle, !flag, cpFlags);
			if (!flag && (convFlags & (ConversionFlags.NoAlpha | ConversionFlags.Swizzle)) == 0)
			{
				return image;
			}
			Image image2 = (flag ? new Image(metadata, IntPtr.Zero, 0, null, bufferIsDisposable: false) : image);
			PixelBufferArray pixelBuffer = image.PixelBuffer;
			PixelBufferArray pixelBuffer2 = image2.PixelBuffer;
			ScanlineFlags scanlineFlags = (((convFlags & ConversionFlags.NoAlpha) != 0) ? ScanlineFlags.SetAlpha : ScanlineFlags.None);
			if ((convFlags & ConversionFlags.Swizzle) != 0)
			{
				scanlineFlags |= ScanlineFlags.Legacy;
			}
			int num = 0;
			int num2 = size;
			for (int i = 0; i < metadata.ArraySize; i++)
			{
				int num3 = metadata.Depth;
				for (int j = 0; j < metadata.MipLevels; j++)
				{
					int num4 = 0;
					while (num4 < num3)
					{
						IntPtr intPtr = pixelBuffer[num].DataPointer;
						IntPtr intPtr2 = pixelBuffer2[num].DataPointer;
						num2 -= pixelBuffer[num].BufferStride;
						if (num2 < 0)
						{
							throw new InvalidOperationException("Unexpected end of buffer");
						}
						if (metadata.Format.IsCompressed())
						{
							Utilities.CopyMemory(intPtr2, intPtr, Math.Min(pixelBuffer[num].BufferStride, pixelBuffer2[num].BufferStride));
						}
						else
						{
							int rowStride = pixelBuffer[num].RowStride;
							int rowStride2 = pixelBuffer2[num].RowStride;
							for (int k = 0; k < pixelBuffer[num].Height; k++)
							{
								if ((convFlags & ConversionFlags.Expand) != 0)
								{
									if ((convFlags & (ConversionFlags.Format565 | ConversionFlags.Format5551)) != 0)
									{
										ExpandScanline(intPtr2, rowStride2, intPtr, rowStride, ((convFlags & ConversionFlags.Format565) != 0) ? Format.B5G6R5_UNorm : Format.B5G5R5A1_UNorm, scanlineFlags);
									}
									else
									{
										TEXP_LEGACY_FORMAT inFormat = FindLegacyFormat(convFlags);
										LegacyExpandScanline(intPtr2, rowStride2, metadata.Format, intPtr, rowStride, inFormat, pal8, scanlineFlags);
									}
								}
								else if ((convFlags & ConversionFlags.Swizzle) != 0)
								{
									SwizzleScanline(intPtr2, rowStride2, intPtr, rowStride, metadata.Format, scanlineFlags);
								}
								else if (intPtr != intPtr2)
								{
									CopyScanline(intPtr2, rowStride2, intPtr, rowStride, metadata.Format, scanlineFlags);
								}
								intPtr = (IntPtr)((byte*)(void*)intPtr + rowStride);
								intPtr2 = (IntPtr)((byte*)(void*)intPtr2 + rowStride2);
							}
						}
						num4++;
						num++;
					}
					if (num3 > 1)
					{
						num3 >>= 1;
					}
				}
			}
			if (flag)
			{
				image.Dispose();
				image = image2;
			}
			return image;
		}

		private static Image CreateCompressedImageFromStream(Stream stream, ImageDescription imageDesc, int length, int mipsToSkip)
		{
			int num = 0;
			int num2 = imageDesc.Width;
			int num3 = imageDesc.Height;
			int num4 = imageDesc.Depth;
			int num5 = num2;
			int num6 = num3;
			for (uint num7 = 0u; num7 < mipsToSkip; num7++)
			{
				Image.ComputePitch(imageDesc.Format, num2, num3, out var _, out var slicePitch, out var _, out var _);
				num += slicePitch * num4;
				if (num3 > 1)
				{
					num3 >>= 1;
					num6 = (num6 >> 1) + num6 % 2;
				}
				if (num2 > 1)
				{
					num2 >>= 1;
					num5 = (num5 >> 1) + num5 % 2;
				}
				if (num4 > 1)
				{
					num4 >>= 1;
				}
			}
			imageDesc.Width = num5;
			imageDesc.Height = num6;
			imageDesc.MipLevels -= mipsToSkip;
			stream.Seek(num, SeekOrigin.Current);
			int num8 = length - num;
			NativeFileStream nativeFileStream;
			if ((nativeFileStream = stream as NativeFileStream) != null)
			{
				IntPtr intPtr = Utilities.AllocateMemory(num8);
				nativeFileStream.Read(intPtr, 0, num8);
				return new Image(imageDesc, intPtr, 0, null, bufferIsDisposable: true);
			}
			byte[] array = new byte[num8];
			stream.Read(array, 0, num8);
			GCHandle value = GCHandle.Alloc(array, GCHandleType.Pinned);
			Image image = new Image(imageDesc, value.AddrOfPinnedObject(), 0, value, bufferIsDisposable: false);
			if (image.TotalSizeInBytes > num8)
			{
				return null;
			}
			return image;
		}

		/// <summary>
		/// Converts an image row with optional clearing of alpha value to 1.0
		/// </summary>
		/// <param name="pDestination"></param>
		/// <param name="outSize"></param>
		/// <param name="pSource"></param>
		/// <param name="inSize"></param>
		/// <param name="inFormat"></param>
		/// <param name="flags"></param>
		private unsafe static void ExpandScanline(IntPtr pDestination, int outSize, IntPtr pSource, int inSize, Format inFormat, ScanlineFlags flags)
		{
			switch (inFormat)
			{
			case Format.B5G6R5_UNorm:
			{
				ushort* ptr3 = (ushort*)(void*)pSource;
				uint* ptr4 = (uint*)(void*)pDestination;
				uint num8 = 0u;
				uint num9 = 0u;
				while (num9 < inSize && num8 < outSize)
				{
					ushort* intPtr3 = ptr3;
					ptr3 = intPtr3 + 1;
					ushort num10 = *intPtr3;
					uint num11 = (uint)(((num10 & 0xF800) >> 8) | ((num10 & 0xE000) >> 13));
					uint num12 = (uint)(((num10 & 0x7E0) << 5) | ((num10 & 0x600) >> 5));
					uint num13 = (uint)(((num10 & 0x1F) << 19) | ((num10 & 0x1C) << 14));
					uint* intPtr4 = ptr4;
					ptr4 = intPtr4 + 1;
					*intPtr4 = num11 | num12 | num13 | 0xFF000000u;
					num9 += 2;
					num8 += 4;
				}
				break;
			}
			case Format.B5G5R5A1_UNorm:
			{
				ushort* ptr = (ushort*)(void*)pSource;
				uint* ptr2 = (uint*)(void*)pDestination;
				uint num = 0u;
				uint num2 = 0u;
				while (num2 < inSize && num < outSize)
				{
					ushort* intPtr = ptr;
					ptr = intPtr + 1;
					ushort num3 = *intPtr;
					uint num4 = (uint)(((num3 & 0x7C00) >> 7) | ((num3 & 0x7000) >> 12));
					uint num5 = (uint)(((num3 & 0x3E0) << 6) | ((num3 & 0x380) << 1));
					uint num6 = (uint)(((num3 & 0x1F) << 19) | ((num3 & 0x1C) << 14));
					uint num7 = (((flags & ScanlineFlags.SetAlpha) != 0) ? 4278190080u : (((num3 & 0x8000u) != 0) ? 4278190080u : 0u));
					uint* intPtr2 = ptr2;
					ptr2 = intPtr2 + 1;
					*intPtr2 = num4 | num5 | num6 | num7;
					num2 += 2;
					num += 4;
				}
				break;
			}
			}
		}

		/// <summary>
		/// Copies an image row with optional clearing of alpha value to 1.0.
		/// </summary>
		/// <remarks>
		/// This method can be used in place as well, otherwise copies the image row unmodified.
		/// </remarks>
		/// <param name="pDestination">The destination buffer.</param>
		/// <param name="outSize">The destination size.</param>
		/// <param name="pSource">The source buffer.</param>
		/// <param name="inSize">The source size.</param>
		/// <param name="format">The <see cref="T:SharpDX.DXGI.Format" /> of the source scanline.</param>
		/// <param name="flags">Scanline flags used when copying the scanline.</param>
		internal unsafe static void CopyScanline(IntPtr pDestination, int outSize, IntPtr pSource, int inSize, Format format, ScanlineFlags flags)
		{
			if ((flags & ScanlineFlags.SetAlpha) != 0)
			{
				uint num11;
				ushort num2;
				int num3;
				ushort* ptr6;
				ushort* ptr5;
				int num13;
				uint* ptr15;
				uint* ptr14;
				switch (format)
				{
				case Format.R32G32B32A32_Float:
					num11 = 1065353216u;
					goto IL_00de;
				case Format.R32G32B32A32_SInt:
					num11 = 2147483647u;
					goto IL_00de;
				case Format.R32G32B32A32_Typeless:
				case Format.R32G32B32A32_UInt:
					num11 = uint.MaxValue;
					goto IL_00de;
				case Format.R16G16B16A16_Float:
					num2 = 15360;
					goto IL_0197;
				case Format.R16G16B16A16_SNorm:
				case Format.R16G16B16A16_SInt:
					num2 = 32767;
					goto IL_0197;
				case Format.R16G16B16A16_Typeless:
				case Format.R16G16B16A16_UNorm:
				case Format.R16G16B16A16_UInt:
					num2 = ushort.MaxValue;
					goto IL_0197;
				case Format.R10G10B10A2_Typeless:
				case Format.R10G10B10A2_UNorm:
				case Format.R10G10B10A2_UInt:
				case Format.R10G10B10_Xr_Bias_A2_UNorm:
				{
					if (pDestination == pSource)
					{
						uint* ptr7 = (uint*)(void*)pDestination;
						for (int m = 0; m < outSize; m += 4)
						{
							*ptr7 |= 3221225472u;
							ptr7++;
						}
						return;
					}
					uint* ptr8 = (uint*)(void*)pSource;
					uint* ptr9 = (uint*)(void*)pDestination;
					int num4 = Math.Min(outSize, inSize);
					for (int n = 0; n < num4; n += 4)
					{
						uint* intPtr12 = ptr9;
						ptr9 = intPtr12 + 1;
						uint* intPtr13 = ptr8;
						ptr8 = intPtr13 + 1;
						*intPtr12 = *intPtr13 | 0xC0000000u;
					}
					return;
				}
				case Format.R8G8B8A8_Typeless:
				case Format.R8G8B8A8_UNorm:
				case Format.R8G8B8A8_UNorm_SRgb:
				case Format.R8G8B8A8_UInt:
				case Format.R8G8B8A8_SNorm:
				case Format.R8G8B8A8_SInt:
				case Format.B8G8R8A8_UNorm:
				case Format.B8G8R8A8_Typeless:
				case Format.B8G8R8A8_UNorm_SRgb:
				{
					uint num5 = ((format == Format.R8G8B8A8_SNorm || format == Format.R8G8B8A8_SInt) ? 2130706432u : 4278190080u);
					if (pDestination == pSource)
					{
						uint* ptr10 = (uint*)(void*)pDestination;
						for (int num6 = 0; num6 < outSize; num6 += 4)
						{
							uint num7 = *ptr10 & 0xFFFFFFu;
							num7 |= num5;
							uint* intPtr14 = ptr10;
							ptr10 = intPtr14 + 1;
							*intPtr14 = num7;
						}
						return;
					}
					uint* ptr11 = (uint*)(void*)pSource;
					uint* ptr12 = (uint*)(void*)pDestination;
					int num8 = Math.Min(outSize, inSize);
					for (int num9 = 0; num9 < num8; num9 += 4)
					{
						uint* intPtr15 = ptr11;
						ptr11 = intPtr15 + 1;
						uint num10 = *intPtr15 & 0xFFFFFFu;
						num10 |= num5;
						uint* intPtr16 = ptr12;
						ptr12 = intPtr16 + 1;
						*intPtr16 = num10;
					}
					return;
				}
				case Format.B5G5R5A1_UNorm:
				{
					if (pDestination == pSource)
					{
						ushort* ptr = (ushort*)(void*)pDestination;
						for (int i = 0; i < outSize; i += 2)
						{
							ushort* intPtr = ptr;
							ptr = intPtr + 1;
							*intPtr = (ushort)(*intPtr | 0x8000u);
						}
						return;
					}
					ushort* ptr2 = (ushort*)(void*)pSource;
					ushort* ptr3 = (ushort*)(void*)pDestination;
					int num = Math.Min(outSize, inSize);
					for (int j = 0; j < num; j += 2)
					{
						ushort* intPtr2 = ptr3;
						ptr3 = intPtr2 + 1;
						ushort* intPtr3 = ptr2;
						ptr2 = intPtr3 + 1;
						*intPtr2 = (ushort)(*intPtr3 | 0x8000u);
					}
					return;
				}
				case Format.A8_UNorm:
					{
						Utilities.ClearMemory(pDestination, byte.MaxValue, outSize);
						return;
					}
					IL_0197:
					if (pDestination == pSource)
					{
						ushort* ptr4 = (ushort*)(void*)pDestination;
						for (int k = 0; k < outSize; k += 8)
						{
							ptr4 += 3;
							ushort* intPtr4 = ptr4;
							ptr4 = intPtr4 + 1;
							*intPtr4 = num2;
						}
						return;
					}
					ptr5 = (ushort*)(void*)pSource;
					ptr6 = (ushort*)(void*)pDestination;
					num3 = Math.Min(outSize, inSize);
					for (int l = 0; l < num3; l += 8)
					{
						ushort* intPtr5 = ptr6;
						ptr6 = intPtr5 + 1;
						ushort* intPtr6 = ptr5;
						ptr5 = intPtr6 + 1;
						*intPtr5 = *intPtr6;
						ushort* intPtr7 = ptr6;
						ptr6 = intPtr7 + 1;
						ushort* intPtr8 = ptr5;
						ptr5 = intPtr8 + 1;
						*intPtr7 = *intPtr8;
						ushort* intPtr9 = ptr6;
						ptr6 = intPtr9 + 1;
						ushort* intPtr10 = ptr5;
						ptr5 = intPtr10 + 1;
						*intPtr9 = *intPtr10;
						ushort* intPtr11 = ptr6;
						ptr6 = intPtr11 + 1;
						*intPtr11 = num2;
						ptr5++;
					}
					return;
					IL_00de:
					if (pDestination == pSource)
					{
						uint* ptr13 = (uint*)(void*)pDestination;
						for (int num12 = 0; num12 < outSize; num12 += 16)
						{
							ptr13 += 3;
							uint* intPtr17 = ptr13;
							ptr13 = intPtr17 + 1;
							*intPtr17 = num11;
						}
						return;
					}
					ptr14 = (uint*)(void*)pSource;
					ptr15 = (uint*)(void*)pDestination;
					num13 = Math.Min(outSize, inSize);
					for (int num14 = 0; num14 < num13; num14 += 16)
					{
						uint* intPtr18 = ptr15;
						ptr15 = intPtr18 + 1;
						uint* intPtr19 = ptr14;
						ptr14 = intPtr19 + 1;
						*intPtr18 = *intPtr19;
						uint* intPtr20 = ptr15;
						ptr15 = intPtr20 + 1;
						uint* intPtr21 = ptr14;
						ptr14 = intPtr21 + 1;
						*intPtr20 = *intPtr21;
						uint* intPtr22 = ptr15;
						ptr15 = intPtr22 + 1;
						uint* intPtr23 = ptr14;
						ptr14 = intPtr23 + 1;
						*intPtr22 = *intPtr23;
						uint* intPtr24 = ptr15;
						ptr15 = intPtr24 + 1;
						*intPtr24 = num11;
						ptr14++;
					}
					return;
				}
			}
			if (!(pDestination == pSource))
			{
				Utilities.CopyMemory(pDestination, pSource, Math.Min(outSize, inSize));
			}
		}

		/// <summary>
		/// Swizzles (RGB &lt;-&gt; BGR) an image row with optional clearing of alpha value to 1.0.
		/// </summary>
		/// <param name="pDestination">The destination buffer.</param>
		/// <param name="outSize">The destination size.</param>
		/// <param name="pSource">The source buffer.</param>
		/// <param name="inSize">The source size.</param>
		/// <param name="format">The <see cref="T:SharpDX.DXGI.Format" /> of the source scanline.</param>
		/// <param name="flags">Scanline flags used when copying the scanline.</param>
		/// <remarks>
		/// This method can be used in place as well, otherwise copies the image row unmodified.
		/// </remarks>
		internal unsafe static void SwizzleScanline(IntPtr pDestination, int outSize, IntPtr pSource, int inSize, Format format, ScanlineFlags flags)
		{
			switch (format)
			{
			case Format.R10G10B10A2_Typeless:
			case Format.R10G10B10A2_UNorm:
			case Format.R10G10B10A2_UInt:
			case Format.R10G10B10_Xr_Bias_A2_UNorm:
			{
				if ((flags & ScanlineFlags.Legacy) == 0)
				{
					break;
				}
				if (pDestination == pSource)
				{
					uint* ptr4 = (uint*)(void*)pDestination;
					for (int k = 0; k < outSize; k += 4)
					{
						uint num12 = *ptr4;
						uint num13 = (num12 & 0x3FF00000) >> 20;
						uint num14 = (num12 & 0x3FF) << 20;
						uint num15 = num12 & 0xFFC00u;
						uint num16 = (((flags & ScanlineFlags.SetAlpha) != 0) ? 3221225472u : (num12 & 0xC0000000u));
						uint* intPtr4 = ptr4;
						ptr4 = intPtr4 + 1;
						*intPtr4 = num13 | num14 | num15 | num16;
					}
					return;
				}
				uint* ptr5 = (uint*)(void*)pSource;
				uint* ptr6 = (uint*)(void*)pDestination;
				int num17 = Math.Min(outSize, inSize);
				for (int l = 0; l < num17; l += 4)
				{
					uint* intPtr5 = ptr5;
					ptr5 = intPtr5 + 1;
					uint num18 = *intPtr5;
					uint num19 = (num18 & 0x3FF00000) >> 20;
					uint num20 = (num18 & 0x3FF) << 20;
					uint num21 = num18 & 0xFFC00u;
					uint num22 = (((flags & ScanlineFlags.SetAlpha) != 0) ? 3221225472u : (num18 & 0xC0000000u));
					uint* intPtr6 = ptr6;
					ptr6 = intPtr6 + 1;
					*intPtr6 = num19 | num20 | num21 | num22;
				}
				return;
			}
			case Format.R8G8B8A8_Typeless:
			case Format.R8G8B8A8_UNorm:
			case Format.R8G8B8A8_UNorm_SRgb:
			case Format.B8G8R8A8_UNorm:
			case Format.B8G8R8X8_UNorm:
			case Format.B8G8R8A8_Typeless:
			case Format.B8G8R8A8_UNorm_SRgb:
			case Format.B8G8R8X8_Typeless:
			case Format.B8G8R8X8_UNorm_SRgb:
			{
				if (pDestination == pSource)
				{
					uint* ptr = (uint*)(void*)pDestination;
					for (int i = 0; i < outSize; i += 4)
					{
						uint num = *ptr;
						uint num2 = (num & 0xFF0000) >> 16;
						uint num3 = (num & 0xFF) << 16;
						uint num4 = num & 0xFF00u;
						uint num5 = (((flags & ScanlineFlags.SetAlpha) != 0) ? 4278190080u : (num & 0xFF000000u));
						uint* intPtr = ptr;
						ptr = intPtr + 1;
						*intPtr = num2 | num3 | num4 | num5;
					}
					return;
				}
				uint* ptr2 = (uint*)(void*)pSource;
				uint* ptr3 = (uint*)(void*)pDestination;
				int num6 = Math.Min(outSize, inSize);
				for (int j = 0; j < num6; j += 4)
				{
					uint* intPtr2 = ptr2;
					ptr2 = intPtr2 + 1;
					uint num7 = *intPtr2;
					uint num8 = (num7 & 0xFF0000) >> 16;
					uint num9 = (num7 & 0xFF) << 16;
					uint num10 = num7 & 0xFF00u;
					uint num11 = (((flags & ScanlineFlags.SetAlpha) != 0) ? 4278190080u : (num7 & 0xFF000000u));
					uint* intPtr3 = ptr3;
					ptr3 = intPtr3 + 1;
					*intPtr3 = num8 | num9 | num10 | num11;
				}
				return;
			}
			}
			if (!(pDestination == pSource))
			{
				Utilities.CopyMemory(pDestination, pSource, Math.Min(outSize, inSize));
			}
		}
	}
}
