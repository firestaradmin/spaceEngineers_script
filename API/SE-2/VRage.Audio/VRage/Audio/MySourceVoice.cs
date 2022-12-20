using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using VRage.Data.Audio;
using VRage.Utils;

namespace VRage.Audio
{
	internal class MySourceVoice : IMySourceVoice
	{
		private MySourceVoicePool m_owner;

		private SourceVoice m_voice;

		private MyCueId m_cueId;

		private MyInMemoryWave[] m_loopBuffers = new MyInMemoryWave[3];

		private float m_frequencyRatio = 1f;

		private VoiceSendDescriptor[] m_currentDescriptor;

		private Queue<DataStream> m_dataStreams;

		private XAudio2 m_device;

		private bool m_isPlaying;

		private bool m_isPaused;

		private bool m_isLoopable;

		private int m_activeSourceBuffers;

		private bool m_valid;

		private bool m_buffered;

		private float m_volumeBase = 1f;

		private float m_volumeMultiplier = 1f;

		public string DebugData;

		public IMy3DSoundEmitter Emitter;

		public Action<IMySourceVoice> StoppedPlaying { get; set; }

		public float DistanceToListener { get; set; }

		public SourceVoice Voice => m_voice;

		public MyCueId CueEnum => m_cueId;

		public bool IsPlaying => m_isPlaying;

		public bool IsPaused => m_isPaused;

		public bool IsLoopable => m_isLoopable;

		public bool IsValid
		{
			get
			{
				if (m_valid)
				{
					return IsNativeValid;
				}
				return false;
			}
		}

		public bool IsNativeValid
		{
			get
			{
				if (m_voice != null && m_voice.IsValid() && m_device != null && !m_device.IsDisposed)
				{
					return m_device.NativePointer != IntPtr.Zero;
				}
				return false;
			}
		}

		public MySourceVoicePool Owner => m_owner;

		public VoiceSendDescriptor[] CurrentOutputVoices => m_currentDescriptor;

		public float FrequencyRatio
		{
			get
			{
				return m_frequencyRatio;
			}
			set
			{
				m_frequencyRatio = value;
				if (!IsValid)
				{
					return;
				}
				MySoundData cue = MyAudio.Static.GetCue(m_cueId);
				if (cue != null && cue.DisablePitchEffects)
				{
					return;
				}
				try
				{
					if (m_voice.State.BuffersQueued > 0)
					{
						m_voice.SetFrequencyRatio(FrequencyRatio);
					}
				}
				catch (NullReferenceException)
				{
				}
			}
		}

		public float Volume
		{
			get
			{
				if (!IsValid)
				{
					return 0f;
				}
				return m_volumeBase;
			}
		}

		public float VolumeMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 1f;
				}
				return m_volumeMultiplier;
			}
			set
			{
				m_volumeMultiplier = value;
				SetVolume(m_volumeBase);
			}
		}

		public bool Silent { get; set; }

		public bool IsBuffered => m_buffered;

		public MySourceVoice(XAudio2 device, WaveFormat sourceFormat)
		{
			m_device = device;
			device.Disposing += OnDeviceDisposing;
			device.CriticalError += OnDeviceCrashed;
			m_voice = new SourceVoice(device, sourceFormat, enableCallbackEvents: true);
			m_voice.BufferEnd += OnStopPlayingBuffered;
			m_valid = true;
			m_dataStreams = new Queue<DataStream>();
			DistanceToListener = float.MaxValue;
			Flush();
		}

		public MySourceVoice(MySourceVoicePool owner, XAudio2 device, WaveFormat sourceFormat)
		{
			m_device = device;
			m_voice = new SourceVoice(device, sourceFormat, VoiceFlags.UseFilter, 2f, enableCallbackEvents: true);
			m_voice.BufferEnd += OnStopPlaying;
			m_valid = true;
			m_owner = owner;
			m_owner.OnAudioEngineChanged += m_owner_OnAudioEngineChanged;
			DistanceToListener = float.MaxValue;
			Flush();
		}

		private void OnDeviceDisposing(object sender, EventArgs eventArgs)
		{
			Destroy();
		}

		private void OnDeviceCrashed(object sender, ErrorEventArgs errorEventArgs)
		{
			m_valid = false;
			m_device = null;
		}

		private void m_owner_OnAudioEngineChanged()
		{
			m_valid = false;
		}

		public void Flush()
		{
			m_cueId = new MyCueId(MyStringHash.NullOrEmpty);
			DisposeWaves();
			m_isPlaying = false;
			m_isPaused = false;
			m_isLoopable = false;
		}

		public int GetOutputChannels()
		{
			if (m_loopBuffers == null)
			{
				return -1;
			}
			int? num = null;
			MyInMemoryWave[] loopBuffers = m_loopBuffers;
			foreach (MyInMemoryWave myInMemoryWave in loopBuffers)
			{
				if (myInMemoryWave != null)
				{
					int channels = myInMemoryWave.WaveFormat.Channels;
					if (!num.HasValue)
					{
						num = channels;
					}
				}
			}
			return num ?? 1;
		}

		internal void SubmitSourceBuffer(MyCueId cueId, MyInMemoryWave wave, MyCueBank.CuePart part)
		{
			m_loopBuffers[(int)part] = wave;
			m_cueId = cueId;
			m_isLoopable |= wave.Buffer.LoopCount > 0;
		}

		private void SubmitSourceBuffer(MyInMemoryWave wave)
		{
			if (wave == null)
			{
				return;
			}
			AudioBuffer buffer = wave.Buffer;
			_ = buffer.LoopCount;
			m_isLoopable |= buffer.LoopCount > 0;
			try
			{
				if (m_activeSourceBuffers == 0)
				{
					m_voice.SourceSampleRate = wave.WaveFormat.SampleRate;
				}
			}
			catch (SharpDXException)
			{
			}
			m_activeSourceBuffers++;
			m_voice.SubmitSourceBuffer(buffer, wave.Stream.DecodedPacketsInfo);
		}

		public void Start(bool skipIntro, bool skipToEnd = false)
		{
			if (!IsValid)
			{
				return;
			}
			if (!skipIntro)
			{
				SubmitSourceBuffer(m_loopBuffers[0]);
			}
			if (m_isLoopable || skipToEnd)
			{
				if (!skipToEnd)
				{
					SubmitSourceBuffer(m_loopBuffers[1]);
				}
				SubmitSourceBuffer(m_loopBuffers[2]);
			}
			if (m_voice.State.BuffersQueued > 0)
			{
				m_voice.SetFrequencyRatio(FrequencyRatio);
				m_voice.Start();
				m_isPlaying = true;
			}
			else
			{
				OnAllBuffersFinished();
			}
		}

		public int GetLengthInSeconds()
		{
			uint[] decodedPacketsInfo = m_loopBuffers[0].Stream.DecodedPacketsInfo;
			uint num = decodedPacketsInfo[decodedPacketsInfo.Length - 1];
			WaveFormat waveFormat = m_loopBuffers[0].WaveFormat;
			int num2 = waveFormat.Channels * waveFormat.BitsPerSample / 8;
			return (int)num / num2 / waveFormat.SampleRate;
		}

		public void StartBuffered()
		{
			if (IsValid)
			{
				if (m_voice.State.BuffersQueued > 0)
				{
					m_voice.SetFrequencyRatio(FrequencyRatio);
					m_voice.Start();
					m_isPlaying = true;
					m_buffered = true;
				}
				else
				{
					OnAllBuffersFinished();
				}
			}
		}

		public void SubmitBuffer(byte[] buffer)
		{
			if (IsValid && m_dataStreams != null && m_dataStreams.Count < 62)
			{
				DataStream dataStream = DataStream.Create(buffer, canRead: true, canWrite: false);
				AudioBuffer audioBuffer = new AudioBuffer(dataStream);
				audioBuffer.Flags = BufferFlags.None;
				lock (m_dataStreams)
				{
					m_dataStreams.Enqueue(dataStream);
				}
				try
				{
					m_voice.SubmitSourceBuffer(audioBuffer, null);
				}
				catch
				{
					MyLog.Default.WriteLine($"IsValid: {IsValid} Buffers: {m_dataStreams.Count} Buffer: {buffer.Length} DataPtr: {audioBuffer.AudioDataPointer}");
					throw;
				}
			}
		}

		private void OnStopPlayingBuffered(IntPtr context)
		{
			if (m_dataStreams == null)
			{
				return;
			}
			lock (m_dataStreams)
			{
				if (m_dataStreams.Count > 0)
				{
					m_dataStreams.Dequeue().Dispose();
				}
			}
			if (m_dataStreams.Count == 0)
			{
				OnAllBuffersFinished();
			}
		}

		private void OnStopPlaying(IntPtr context)
		{
			m_activeSourceBuffers--;
			if (m_activeSourceBuffers == 0)
			{
				OnAllBuffersFinished();
			}
		}

		private void OnAllBuffersFinished()
		{
			m_isPlaying = false;
			if (StoppedPlaying != null)
			{
				MyXAudio2.Instance.EnqueueStopPlayingCallback(this);
			}
			m_owner?.OnStopPlaying(this);
		}

		public void Stop(bool force = false)
		{
			if (!IsValid || !m_isPlaying)
			{
				return;
			}
			if ((force || m_isLoopable) && m_owner != null)
			{
				m_owner.AddToFadeoutList(this);
				return;
			}
			try
			{
				m_voice.Stop();
				m_voice.FlushSourceBuffers();
			}
			catch (NullReferenceException)
			{
			}
		}

		public void Pause()
		{
			if (IsValid)
			{
				m_voice.Stop();
				m_isPaused = true;
			}
		}

		public void Resume()
		{
			if (IsValid)
			{
				m_voice.Start();
				m_isPaused = false;
			}
		}

		public void SetVolume(float volume)
		{
			m_volumeBase = volume;
			if (IsValid)
			{
				try
				{
					m_voice.SetVolume(m_volumeBase * m_volumeMultiplier);
				}
				catch (NullReferenceException)
				{
				}
			}
		}

		public void SetOutputVoices(VoiceSendDescriptor[] descriptors)
		{
			if (IsValid && m_currentDescriptor != descriptors)
			{
				m_voice.SetOutputVoices(descriptors);
				m_currentDescriptor = descriptors;
			}
		}

		public override string ToString()
		{
			return string.Format(m_cueId.ToString());
		}

		public void Dispose()
		{
			m_valid = false;
			if (IsNativeValid)
			{
				m_device.Disposing -= OnDeviceDisposing;
				m_device.CriticalError -= OnDeviceCrashed;
				m_voice.DestroyVoice();
				m_voice.Dispose();
				m_voice = null;
				m_device = null;
			}
			while (true)
			{
				Queue<DataStream> dataStreams = m_dataStreams;
				if (dataStreams == null || dataStreams.Count <= 0)
				{
					break;
				}
				m_dataStreams.Dequeue().Dispose();
			}
			DisposeWaves();
		}

		private void DisposeWaves()
		{
			if (m_loopBuffers == null)
			{
				return;
			}
			for (int i = 0; i < m_loopBuffers.Length; i++)
			{
				if (m_loopBuffers[i] != null && m_loopBuffers[i].Streamed)
				{
					m_loopBuffers[i].Dereference();
				}
				m_loopBuffers[i] = null;
			}
		}

		internal void CleanupBeforeDispose()
		{
			m_valid = false;
			StoppedPlaying = null;
			m_currentDescriptor = null;
			if (m_owner != null)
			{
				m_owner.OnAudioEngineChanged -= m_owner_OnAudioEngineChanged;
				m_owner = null;
			}
			if (IsNativeValid)
			{
				m_voice.Stop();
			}
			m_valid = false;
		}

		public void Destroy()
		{
			CleanupBeforeDispose();
			Dispose();
		}
	}
}
