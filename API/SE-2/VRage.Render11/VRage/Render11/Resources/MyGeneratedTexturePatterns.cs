using SharpDX.DXGI;
using VRage.Render11.Common;

namespace VRage.Render11.Resources
{
	internal static class MyGeneratedTexturePatterns
	{
		public static readonly byte[] ColorMetal_BC7_SRgb;

		public static readonly byte[] NormalGloss_BC7;

		public static readonly byte[] Extension_BC7_SRgb;

		public static readonly byte[] Alphamask_BC4;

		static MyGeneratedTexturePatterns()
		{
			ColorMetal_BC7_SRgb = new byte[16]
			{
				8, 252, 255, 255, 63, 0, 0, 0, 252, 255,
				255, 63, 0, 0, 0, 0
			};
			NormalGloss_BC7 = new byte[16]
			{
				192, 223, 239, 247, 251, 255, 1, 128, 54, 51,
				51, 51, 51, 51, 51, 51
			};
			Extension_BC7_SRgb = new byte[16]
			{
				16, 255, 3, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0
			};
			Alphamask_BC4 = new byte[8] { 255, 0, 0, 0, 0, 0, 0, 0 };
		}

		public static byte[] GetBytePattern(MyChannel channel, Format format)
		{
			if (format == Format.Unknown)
			{
				return null;
			}
			switch (channel)
			{
			case MyChannel.ColorMetal:
				if (format == Format.BC7_UNorm_SRgb || format == Format.BC7_UNorm)
				{
					return ColorMetal_BC7_SRgb;
				}
				break;
			case MyChannel.NormalGloss:
				if (format == Format.BC7_UNorm)
				{
					return NormalGloss_BC7;
				}
				break;
			case MyChannel.Extension:
				if (format == Format.BC7_UNorm_SRgb || format == Format.BC7_UNorm)
				{
					return Extension_BC7_SRgb;
				}
				break;
			case MyChannel.Alphamask:
				if (format == Format.BC4_UNorm)
				{
					return Alphamask_BC4;
				}
				break;
			}
			return new byte[MyResourceUtils.GetTexelBitSize(format) * 16 / 8];
		}
	}
}
