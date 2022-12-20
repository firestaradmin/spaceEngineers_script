using System;
using SharpDX.Multimedia;

namespace VRage.Audio
{
	public struct MyWaveFormat : IEquatable<MyWaveFormat>
	{
		public WaveFormatEncoding Encoding;

		public int Channels;

		public int SampleRate;

		public WaveFormat WaveFormat;

		public bool Equals(MyWaveFormat y)
		{
			if (Encoding != y.Encoding)
			{
				return false;
			}
			if (Channels != y.Channels)
			{
				return false;
			}
			if (SampleRate != y.SampleRate && Encoding != WaveFormatEncoding.Adpcm && y.Encoding != WaveFormatEncoding.Adpcm)
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			int encoding = (int)Encoding;
			encoding = (encoding * 397) ^ Channels;
			if (Encoding != WaveFormatEncoding.Adpcm)
			{
				encoding = (encoding * 397) ^ SampleRate;
			}
			return encoding;
		}
	}
}
