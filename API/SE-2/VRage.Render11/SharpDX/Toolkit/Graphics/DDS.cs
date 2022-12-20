using System;
using System.Runtime.InteropServices;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Multimedia;

namespace SharpDX.Toolkit.Graphics
{
	internal class DDS
	{
		/// <summary>
		/// Internal structure used to describe a DDS pixel format.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PixelFormat
		{
			public int Size;

			public PixelFormatFlags Flags;

			public int FourCC;

			public int RGBBitCount;

			public uint RBitMask;

			public uint GBitMask;

			public uint BBitMask;

			public uint ABitMask;

			public static readonly PixelFormat DXT1 = new PixelFormat(PixelFormatFlags.FourCC, new FourCC('D', 'X', 'T', '1'), 0, 0u, 0u, 0u, 0u);

			public static readonly PixelFormat DXT2 = new PixelFormat(PixelFormatFlags.FourCC, new FourCC('D', 'X', 'T', '2'), 0, 0u, 0u, 0u, 0u);

			public static readonly PixelFormat DXT3 = new PixelFormat(PixelFormatFlags.FourCC, new FourCC('D', 'X', 'T', '3'), 0, 0u, 0u, 0u, 0u);

			public static readonly PixelFormat DXT4 = new PixelFormat(PixelFormatFlags.FourCC, new FourCC('D', 'X', 'T', '4'), 0, 0u, 0u, 0u, 0u);

			public static readonly PixelFormat DXT5 = new PixelFormat(PixelFormatFlags.FourCC, new FourCC('D', 'X', 'T', '5'), 0, 0u, 0u, 0u, 0u);

			public static readonly PixelFormat BC4_UNorm = new PixelFormat(PixelFormatFlags.FourCC, new FourCC('B', 'C', '4', 'U'), 0, 0u, 0u, 0u, 0u);

			public static readonly PixelFormat BC4_SNorm = new PixelFormat(PixelFormatFlags.FourCC, new FourCC('B', 'C', '4', 'S'), 0, 0u, 0u, 0u, 0u);

			public static readonly PixelFormat BC5_UNorm = new PixelFormat(PixelFormatFlags.FourCC, new FourCC('B', 'C', '5', 'U'), 0, 0u, 0u, 0u, 0u);

			public static readonly PixelFormat BC5_SNorm = new PixelFormat(PixelFormatFlags.FourCC, new FourCC('B', 'C', '5', 'S'), 0, 0u, 0u, 0u, 0u);

			public static readonly PixelFormat R8G8_B8G8 = new PixelFormat(PixelFormatFlags.FourCC, new FourCC('R', 'G', 'B', 'G'), 0, 0u, 0u, 0u, 0u);

			public static readonly PixelFormat G8R8_G8B8 = new PixelFormat(PixelFormatFlags.FourCC, new FourCC('G', 'R', 'G', 'B'), 0, 0u, 0u, 0u, 0u);

			public static readonly PixelFormat A8R8G8B8 = new PixelFormat(PixelFormatFlags.Rgba, 0, 32, 16711680u, 65280u, 255u, 4278190080u);

			public static readonly PixelFormat X8R8G8B8 = new PixelFormat(PixelFormatFlags.Rgb, 0, 32, 16711680u, 65280u, 255u, 0u);

			public static readonly PixelFormat A8B8G8R8 = new PixelFormat(PixelFormatFlags.Rgba, 0, 32, 255u, 65280u, 16711680u, 4278190080u);

			public static readonly PixelFormat X8B8G8R8 = new PixelFormat(PixelFormatFlags.Rgb, 0, 32, 255u, 65280u, 16711680u, 0u);

			public static readonly PixelFormat G16R16 = new PixelFormat(PixelFormatFlags.Rgb, 0, 32, 65535u, 4294901760u, 0u, 0u);

			public static readonly PixelFormat R5G6B5 = new PixelFormat(PixelFormatFlags.Rgb, 0, 16, 63488u, 2016u, 31u, 0u);

			public static readonly PixelFormat A1R5G5B5 = new PixelFormat(PixelFormatFlags.Rgba, 0, 16, 31744u, 992u, 31u, 32768u);

			public static readonly PixelFormat A4R4G4B4 = new PixelFormat(PixelFormatFlags.Rgba, 0, 16, 3840u, 240u, 15u, 61440u);

			public static readonly PixelFormat R8G8B8 = new PixelFormat(PixelFormatFlags.Rgb, 0, 24, 16711680u, 65280u, 255u, 0u);

			public static readonly PixelFormat L8 = new PixelFormat(PixelFormatFlags.Luminance, 0, 8, 255u, 0u, 0u, 0u);

			public static readonly PixelFormat L16 = new PixelFormat(PixelFormatFlags.Luminance, 0, 16, 65535u, 0u, 0u, 0u);

			public static readonly PixelFormat A8L8 = new PixelFormat(PixelFormatFlags.LuminanceAlpha, 0, 16, 255u, 0u, 0u, 65280u);

			public static readonly PixelFormat A8 = new PixelFormat(PixelFormatFlags.Alpha, 0, 8, 0u, 0u, 0u, 255u);

			public static readonly PixelFormat DX10 = new PixelFormat(PixelFormatFlags.FourCC, new FourCC('D', 'X', '1', '0'), 0, 0u, 0u, 0u, 0u);

			/// <summary>
			/// Initializes a new instance of the <see cref="T:SharpDX.Toolkit.Graphics.DDS.PixelFormat" /> struct.
			/// </summary>
			/// <param name="flags">The flags.</param>
			/// <param name="fourCC">The four CC.</param>
			/// <param name="rgbBitCount">The RGB bit count.</param>
			/// <param name="rBitMask">The r bit mask.</param>
			/// <param name="gBitMask">The g bit mask.</param>
			/// <param name="bBitMask">The b bit mask.</param>
			/// <param name="aBitMask">A bit mask.</param>
			public PixelFormat(PixelFormatFlags flags, int fourCC, int rgbBitCount, uint rBitMask, uint gBitMask, uint bBitMask, uint aBitMask)
			{
				Size = Utilities.SizeOf<PixelFormat>();
				Flags = flags;
				FourCC = fourCC;
				RGBBitCount = rgbBitCount;
				RBitMask = rBitMask;
				GBitMask = gBitMask;
				BBitMask = bBitMask;
				ABitMask = aBitMask;
			}
		}

		/// <summary>
		/// PixelFormat flags.
		/// </summary>
		[Flags]
		public enum PixelFormatFlags
		{
			FourCC = 0x4,
			Rgb = 0x40,
			Rgba = 0x41,
			Luminance = 0x20000,
			LuminanceAlpha = 0x20001,
			Alpha = 0x2,
			Pal8 = 0x20
		}

		/// <summary>
		/// DDS Header flags.
		/// </summary>
		[Flags]
		public enum HeaderFlags
		{
			Texture = 0x1007,
			Mipmap = 0x20000,
			Volume = 0x800000,
			Pitch = 0x8,
			LinearSize = 0x80000,
			Height = 0x2,
			Width = 0x4
		}

		/// <summary>
		/// DDS Surface flags.
		/// </summary>
		[Flags]
		public enum SurfaceFlags
		{
			Texture = 0x1000,
			Mipmap = 0x400008,
			Cubemap = 0x8
		}

		/// <summary>
		/// DDS Cubemap flags.
		/// </summary>
		[Flags]
		public enum CubemapFlags
		{
			CubeMap = 0x200,
			Volume = 0x200000,
			PositiveX = 0x600,
			NegativeX = 0xA00,
			PositiveY = 0x1200,
			NegativeY = 0x2200,
			PositiveZ = 0x4200,
			NegativeZ = 0x8200,
			AllFaces = 0xFE00
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct Header
		{
			public int Size;

			public HeaderFlags Flags;

			public int Height;

			public int Width;

			public int PitchOrLinearSize;

			public int Depth;

			public int MipMapCount;

			private readonly uint unused1;

			private readonly uint unused2;

			private readonly uint unused3;

			private readonly uint unused4;

			private readonly uint unused5;

			private readonly uint unused6;

			private readonly uint unused7;

			private readonly uint unused8;

			private readonly uint unused9;

			private readonly uint unused10;

			private readonly uint unused11;

			public PixelFormat PixelFormat;

			public SurfaceFlags SurfaceFlags;

			public CubemapFlags CubemapFlags;

			private readonly uint Unused12;

			private readonly uint Unused13;

			private readonly uint Unused14;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct HeaderDXT10
		{
			public Format DXGIFormat;

			public ResourceDimension ResourceDimension;

			public ResourceOptionFlags MiscFlags;

			public int ArraySize;

			private readonly uint Unused;
		}

		/// <summary>
		/// Magic code to identify DDS header
		/// </summary>
		public const uint MagicHeader = 542327876u;
	}
}
