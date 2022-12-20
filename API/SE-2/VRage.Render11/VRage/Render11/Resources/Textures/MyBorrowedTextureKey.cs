using System;
using System.Text;
using SharpDX.DXGI;

namespace VRage.Render11.Resources.Textures
{
	internal struct MyBorrowedTextureKey : IEquatable<MyBorrowedTextureKey>
	{
		public int Width;

		public int Height;

		public Format Format;

		public int SamplesCount;

		public int SamplesQuality;

		public bool HqDepth;

		private string m_toString;

		public override int GetHashCode()
		{
			return (Width << 1).GetHashCode() ^ (Height << 2).GetHashCode() ^ Format.GetHashCode() ^ (SamplesCount << 3).GetHashCode() ^ (SamplesQuality << 4).GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((MyBorrowedTextureKey)obj);
		}

		public override string ToString()
		{
			if (m_toString == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("{0}x{1}-{2}", Width, Height, Format);
				m_toString = stringBuilder.ToString();
			}
			return m_toString;
		}

		public bool Equals(MyBorrowedTextureKey other)
		{
			if (Width == other.Width && Height == other.Height && Format == other.Format && SamplesCount == other.SamplesCount)
			{
				return SamplesQuality == other.SamplesQuality;
			}
			return false;
		}
	}
}
