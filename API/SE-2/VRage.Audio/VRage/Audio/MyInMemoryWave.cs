using System;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using VRage.Data.Audio;

namespace VRage.Audio
{
	public class MyInMemoryWave : IDisposable
	{
		private SoundStream m_stream;

		private WaveFormat m_waveFormat;

		private AudioBuffer m_buffer;

		private MyWaveBank m_owner;

		private string m_path;

		private MyInMemoryWaveDataCache.CacheData? m_cacheToDispose;

		private int m_references = 1;

		public WaveFormat WaveFormat => m_waveFormat;

		public SoundStream Stream => m_stream;

		public AudioBuffer Buffer => m_buffer;

		public bool Streamed { get; private set; }

		public MyInMemoryWave(MySoundData cue, string path, MyWaveBank owner, bool streamed = false, bool cached = false)
		{
			bool owns = false;
			MyInMemoryWaveDataCache.CacheData value = ((!cached) ? MyInMemoryWaveDataCache.Static.Get(path, out owns) : MyInMemoryWaveDataCache.Static.LoadCached(path));
			if (owns)
			{
				m_cacheToDispose = value;
			}
			m_owner = owner;
			m_path = path;
			m_stream = value.SoundStream;
			m_waveFormat = m_stream.Format;
			m_buffer = new AudioBuffer
			{
				Stream = value.DataStream,
				AudioBytes = (int)m_stream.Length,
				Flags = BufferFlags.None
			};
			if (cue.Loopable)
			{
				m_buffer.LoopCount = 255;
			}
			((MyXAudio2)MyAudio.Static).AudioPlatform.InitializeAudioBuffer(m_buffer, m_waveFormat);
			Streamed = streamed;
		}

		public void Dereference()
		{
			if (Streamed && --m_references <= 0)
			{
				Dispose();
			}
		}

		public void Dispose()
		{
			if (m_cacheToDispose.HasValue)
			{
				m_cacheToDispose.Value.Dispose();
				m_cacheToDispose = null;
			}
			else
			{
				m_stream?.Dispose();
			}
			if (Streamed)
			{
				m_owner.LoadedStreamedWaves.Remove(m_path);
			}
			m_buffer = null;
			m_owner = null;
			m_waveFormat = null;
		}

		public void Reference()
		{
			m_references++;
		}
	}
}
