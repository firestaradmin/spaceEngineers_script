using System.Runtime.CompilerServices;

namespace VRageMath.PackedVector
{
	public static class HalfUtils
	{
		private const int cFracBits = 10;

		private const int cExpBits = 5;

		private const int cSignBit = 15;

		private const uint cSignMask = 32768u;

		private const uint cFracMask = 1023u;

		private const int cExpBias = 15;

		private const uint cRoundBit = 4096u;

		private const uint eMax = 16u;

		private const int eMin = -14;

		private const uint wMaxNormal = 1207955455u;

		private const uint wMinNormal = 947912704u;

		private const uint BiasDiffo = 3355443200u;

		private const int cFracBitsDiff = 13;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static ushort Pack(float value)
		{
			int num = *(int*)(&value);
			uint num2 = (uint)(num & int.MinValue) >> 16;
			uint num3 = (uint)num & 0x7FFFFFFFu;
			if (num3 > 1207955455)
			{
				return (ushort)(num2 | 0x7FFFu);
			}
			if (num3 < 947912704)
			{
				uint num4 = (num3 & 0x7FFFFFu) | 0x800000u;
				int num5 = (int)(113 - (num3 >> 23));
				uint num6 = ((num5 <= 31) ? (num4 >> num5) : 0u);
				return (ushort)(num2 | (num6 + 4095 + ((num6 >> 13) & 1) >> 13));
			}
			return (ushort)(num2 | (num3 - 939524096 + 4095 + ((num3 >> 13) & 1) >> 13));
		}

		public unsafe static float Unpack(ushort value)
		{
			uint num4;
			if ((value & -33792) == 0)
			{
				if ((value & 0x3FFu) != 0)
				{
					uint num = 4294967282u;
					uint num2 = value & 0x3FFu;
					while ((num2 & 0x400) == 0)
					{
						num--;
						num2 <<= 1;
					}
					uint num3 = num2 & 0xFFFFFBFFu;
					num4 = (uint)((value & 0x8000) << 16) | (num + 127 << 23) | (num3 << 13);
				}
				else
				{
					num4 = (uint)((value & 0x8000) << 16);
				}
			}
			else
			{
				num4 = (uint)(((value & 0x8000) << 16) | (((value >> 10) & 0x1F) - 15 + 127 << 23) | ((value & 0x3FF) << 13));
			}
			return *(float*)(&num4);
		}
	}
}
