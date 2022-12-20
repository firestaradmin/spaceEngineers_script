using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using SharpDX;
using SharpDX.Mathematics.Interop;
using SharpDX.Multimedia;
using SharpDX.X3DAudio;
using SharpDX.XAPO.Fx;
using SharpDX.XAudio2;
using SharpDX.XAudio2.Fx;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace VRage.Audio
{
	public class MyXAudio2 : IMyAudio
	{
		private struct MyMusicTransition
		{
			public int Priority;

			public MyStringId TransitionEnum;

			public MyStringId Category;

			public MyMusicTransition(int priority, MyStringId transitionEnum, MyStringId category)
			{
				Priority = priority;
				TransitionEnum = transitionEnum;
				Category = category;
			}
		}

		private delegate void VolumeChangeHandler(float newVolume);

		private static readonly object lockObject = new object();

		private VoiceSendDescriptor[] m_gameAudioVoiceDesc;

		private VoiceSendDescriptor[] m_musicAudioVoiceDesc;

		private VoiceSendDescriptor[] m_hudAudioVoiceDesc;

		private MyAudioInitParams m_initParams;

		private XAudio2 m_audioEngine;

		private DeviceDetails m_deviceDetails;

		private MasteringVoice m_masterVoice;

		private SubmixVoice m_gameAudioVoice;

		private SubmixVoice m_musicAudioVoice;

		private SubmixVoice m_hudAudioVoice;

		private MyCueBank m_cueBank;

		private MyEffectBank m_effectBank;

		private X3DAudio m_x3dAudio;

		private bool m_canPlay;

		private bool m_loading;

		private float m_volumeHud;

		private float m_volumeDefault;

		private float m_volumeMusic;

		private float m_volumeVoiceChat;

		private bool m_mute;

		private bool m_musicAllowed;

		private bool m_musicOn;

		private bool m_gameSoundsOn;

		private bool m_voiceChatEnabled;

		private MyMusicState m_musicState;

		private bool m_loopMusic;

		private MySourceVoice m_musicCue;

		private CalculateFlags m_calculateFlags;

		internal static MyXAudio2 Instance;

		private SortedList<int, MyMusicTransition> m_nextTransitions = new SortedList<int, MyMusicTransition>();

		private MyMusicTransition? m_currentTransition;

		private bool m_transitionForward;

		private float m_volumeAtTransitionStart;

		private int m_timeFromTransitionStart;

		private const int TRANSITION_TIME = 1000;

		private Listener m_listener;

		private Emitter m_helperEmitter;

		private List<IMy3DSoundEmitter> m_3Dsounds;

		private bool m_canUpdate3dSounds = true;

		private int m_soundInstancesTotal2D;

		private int m_soundInstancesTotal3D;

		private SharpDX.XAudio2.Fx.Reverb m_reverb;

		private bool m_applyReverb;

		private float m_globalVolumeLevel = 1f;

		private float m_globalVolumeTarget = 1f;

		private float m_globalVolumeIncrement = 1f;

		private bool m_globalVolumeRaising = true;

		private bool m_globalVolumeChanging;

		private bool m_useVolumeLimiter;

		private bool m_useSameSoundLimiter;

		private bool m_enableReverb;

		private bool m_enableDoppler = true;

		private bool m_reverbSet;

		private bool m_soundLimiterReady;

		private bool m_soundLimiterSet;

		private Thread m_updateInProgress;

		private volatile bool m_deviceLost;

		private int m_lastDeviceCount;

		private ListReader<MySoundData> m_sounds;

		private ListReader<MyAudioEffect> m_effects;

		private int m_deviceNumber;

		private readonly IMyPlatformAudio m_audioPlatform;

		private static MyStringId NO_RANDOM = MyStringId.GetOrCompute("NoRandom");

		private MyTimeSpan m_nextDeviceCountCheck;

		private static readonly float[] m_outputMatrixMono = new float[8] { 0.5f, 0.5f, 0f, 0f, 0.4f, 0.4f, 0f, 0f };

		private static readonly float[] m_outputMatrixStereo = new float[16]
		{
			1f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0.8f, 0f,
			0f, 0.8f, 0f, 0f, 0f, 0f
		};

		private MyConcurrentQueue<MySourceVoice> m_voicesForStopPlayingCallback = new MyConcurrentQueue<MySourceVoice>();

		Dictionary<MyCueId, MySoundData>.ValueCollection IMyAudio.CueDefinitions
		{
			get
			{
				if (!m_canPlay)
				{
					return null;
				}
				return m_cueBank.CueDefinitions;
			}
		}

		public int SampleRate
		{
			get
			{
				if (m_masterVoice == null || m_deviceLost)
				{
					return 0;
				}
				return m_masterVoice.VoiceDetails.InputSampleRate;
			}
		}

		public MySoundData SoloCue { get; set; }

		public bool GameSoundIsPaused { get; private set; }

		bool IMyAudio.UseVolumeLimiter
		{
			get
			{
				return m_useVolumeLimiter;
			}
			set
			{
				m_useVolumeLimiter = value;
			}
		}

		bool IMyAudio.UseSameSoundLimiter
		{
			get
			{
				return m_useSameSoundLimiter;
			}
			set
			{
				m_useSameSoundLimiter = value;
			}
		}

		bool IMyAudio.EnableReverb
		{
			get
			{
				return m_enableReverb;
			}
			set
			{
				m_enableReverb = value;
				if (!m_reverbSet && value && m_masterVoice != null && m_masterVoice.VoiceDetails.InputSampleRate <= MyAudio.MAX_SAMPLE_RATE)
				{
					try
					{
						m_reverb = new SharpDX.XAudio2.Fx.Reverb(m_audioEngine);
						EffectDescriptor effectDescriptor = new EffectDescriptor(m_reverb, m_masterVoice.VoiceDetails.InputChannelCount);
						m_gameAudioVoice.SetEffectChain(effectDescriptor);
						m_gameAudioVoice.DisableEffect(0);
						m_reverbSet = true;
					}
					catch (Exception ex)
					{
						MyLog.Default.WriteLine("Failed to enable Reverb" + ex.ToString());
					}
				}
			}
		}

		bool IMyAudio.EnableDoppler
		{
			get
			{
				return m_enableDoppler;
			}
			set
			{
				m_enableDoppler = value;
			}
		}

		public bool CacheLoaded
		{
			get
			{
				return m_initParams.CacheLoaded;
			}
			set
			{
				m_initParams.CacheLoaded = value;
			}
		}

		public bool CanPlay => m_canPlay;

		public IMyPlatformAudio AudioPlatform => m_audioPlatform;

		public bool ApplyReverb
		{
			get
			{
				if (!m_canPlay || !m_enableReverb)
				{
					return false;
				}
				if (m_cueBank == null)
				{
					return false;
				}
				if (m_masterVoice.VoiceDetails.InputSampleRate > MyAudio.MAX_SAMPLE_RATE)
				{
					return false;
				}
				return m_applyReverb;
			}
			set
			{
				if (!m_canPlay || !m_reverbSet || m_deviceLost || m_cueBank == null || (!m_enableReverb && !m_applyReverb))
				{
					return;
				}
				if (m_gameAudioVoice != null)
				{
					if (value)
					{
						m_gameAudioVoice.EnableEffect(0);
					}
					else
					{
						m_gameAudioVoice.IsEffectEnabled(0, out var enabledRef);
						if ((bool)enabledRef)
						{
							m_gameAudioVoice.DisableEffect(0);
						}
					}
				}
				m_cueBank.ApplyReverb = value;
				m_applyReverb = value;
			}
		}

		public float VolumeMusic
		{
			get
			{
				if (!m_canPlay || !m_musicOn)
				{
					return 0f;
				}
				return m_volumeMusic;
			}
			set
			{
				if (m_canPlay && m_musicOn && !m_deviceLost)
				{
					m_volumeMusic = MathHelper.Clamp(value, 0f, 1f);
					m_musicAudioVoice.SetVolume(m_volumeMusic * m_globalVolumeLevel);
					if (this.OnSetVolumeMusic != null)
					{
						this.OnSetVolumeMusic(m_volumeMusic);
					}
				}
			}
		}

		public float VolumeHud
		{
			get
			{
				if (!m_canPlay)
				{
					return 0f;
				}
				return m_volumeHud;
			}
			set
			{
				if (m_canPlay && !m_deviceLost)
				{
					m_volumeHud = MathHelper.Clamp(value, 0f, 1f);
					m_hudAudioVoice.SetVolume(m_volumeHud * m_globalVolumeLevel);
					if (this.OnSetVolumeHud != null)
					{
						this.OnSetVolumeHud(m_volumeHud);
					}
				}
			}
		}

		public float VolumeGame
		{
			get
			{
				if (!m_canPlay || !m_gameSoundsOn)
				{
					return 0f;
				}
				return m_volumeDefault;
			}
			set
			{
				if (m_canPlay && m_gameSoundsOn && !m_deviceLost)
				{
					m_volumeDefault = MathHelper.Clamp(value, 0f, 1f);
					m_gameAudioVoice.SetVolume(m_volumeDefault * m_globalVolumeLevel);
					if (this.OnSetVolumeGame != null)
					{
						this.OnSetVolumeGame(m_volumeDefault);
					}
				}
			}
		}

		public float VolumeVoiceChat
		{
			get
			{
				return m_volumeVoiceChat;
			}
			set
			{
				m_volumeVoiceChat = MathHelper.Clamp(value, 0f, 5f);
			}
		}

		public bool EnableVoiceChat
		{
			get
			{
				if (!m_canPlay)
				{
					return false;
				}
				return m_voiceChatEnabled;
			}
			set
			{
				if (m_voiceChatEnabled != value)
				{
					m_voiceChatEnabled = value;
					if (this.VoiceChatEnabled != null)
					{
						this.VoiceChatEnabled(m_voiceChatEnabled);
					}
				}
			}
		}

		public bool Mute
		{
			get
			{
				return m_mute;
			}
			set
			{
				if (m_mute == value)
				{
					return;
				}
				m_mute = value;
				if (m_mute)
				{
					if (m_canPlay)
					{
						m_gameAudioVoice.SetVolume(0f);
						m_musicAudioVoice.SetVolume(0f);
					}
				}
				else if (m_canPlay)
				{
					if (!GameSoundIsPaused)
					{
						m_gameAudioVoice.SetVolume(m_volumeDefault * m_globalVolumeLevel);
					}
					m_musicAudioVoice.SetVolume(m_volumeMusic * m_globalVolumeLevel);
					m_hudAudioVoice.SetVolume(m_volumeHud * m_globalVolumeLevel);
				}
			}
		}

		public bool MusicAllowed
		{
			get
			{
				return m_musicAllowed;
			}
			set
			{
				m_musicAllowed = value;
			}
		}

		private event VolumeChangeHandler OnSetVolumeHud;

		private event VolumeChangeHandler OnSetVolumeGame;

		private event VolumeChangeHandler OnSetVolumeMusic;

		public event Action<bool> VoiceChatEnabled;

		List<MyStringId> IMyAudio.GetCategories()
		{
			if (!m_canPlay)
			{
				return null;
			}
			return m_cueBank.GetCategories();
		}

		MySoundData IMyAudio.GetCue(MyCueId cueId)
		{
			if (!m_canPlay)
			{
				return null;
			}
			return m_cueBank.GetCue(cueId);
		}

		Dictionary<MyStringId, List<MyCueId>> IMyAudio.GetAllMusicCues()
		{
			if (m_cueBank == null)
			{
				return null;
			}
			return m_cueBank.GetMusicCues();
		}

		public MyXAudio2(IMyPlatformAudio audioPlatform)
		{
			m_audioPlatform = audioPlatform;
		}

		private void Init()
		{
			Instance = this;
			StartEngine();
			CreateX3DAudio();
		}

		private int GetDeviceCount()
		{
			return m_audioPlatform.DeviceCount;
		}

		private DeviceDetails GetDeviceDetails(int index)
		{
			return m_audioPlatform.GetDeviceDetails(index, forceRefresh: false);
		}

		private void StartEngine()
		{
			if (m_audioEngine != null)
			{
				DisposeVoices();
				m_audioEngine.CriticalError -= m_audioEngine_CriticalError;
				m_audioPlatform.DisposeAudioEngine();
			}
			m_audioEngine = m_audioPlatform.InitAudioEngine();
			m_audioEngine.CriticalError += m_audioEngine_CriticalError;
			m_lastDeviceCount = GetDeviceCount();
			m_deviceNumber = 0;
			bool flag = false;
			while (true)
			{
				try
				{
					m_deviceDetails = GetDeviceDetails(m_deviceNumber);
					if (m_deviceDetails.Role == DeviceRole.DefaultCommunicationsDevice)
					{
						m_deviceNumber++;
						if (m_deviceNumber != GetDeviceCount())
						{
							goto IL_00ed;
						}
						m_deviceNumber--;
					}
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine($"Failed to get device details:");
					MyLog.Default.WriteLine(ex.ToString());
					flag = true;
					goto IL_00ed;
				}
				break;
				IL_00ed:
				if (flag)
				{
					try
					{
						MyLog.Default.WriteLine($"Device no.: {m_deviceNumber}\n\tdevice count: {GetDeviceCount()}", LoggingOptions.AUDIO);
					}
					catch (Exception)
					{
					}
					m_deviceNumber = 0;
					m_deviceDetails = GetDeviceDetails(m_deviceNumber);
					break;
				}
			}
			m_masterVoice = m_audioPlatform.CreateMasteringVoice(m_deviceNumber);
			if (m_useVolumeLimiter)
			{
				MasteringLimiter masteringLimiter = new MasteringLimiter(m_audioEngine);
				MasteringLimiterParameters parameter = masteringLimiter.Parameter;
				parameter.Loudness = 0;
				masteringLimiter.Parameter = parameter;
				EffectDescriptor effectDescriptor = new EffectDescriptor(masteringLimiter);
				m_masterVoice.SetEffectChain(effectDescriptor);
				m_soundLimiterReady = true;
				m_masterVoice.DisableEffect(0);
			}
			m_calculateFlags = CalculateFlags.Matrix | CalculateFlags.Doppler;
			if ((m_deviceDetails.OutputFormat.ChannelMask & Speakers.LowFrequency) != 0)
			{
				m_calculateFlags |= CalculateFlags.RedirectToLfe;
			}
			VoiceDetails voiceDetails = m_masterVoice.VoiceDetails;
			m_gameAudioVoice = new SubmixVoice(m_audioEngine, voiceDetails.InputChannelCount, voiceDetails.InputSampleRate);
			m_musicAudioVoice = new SubmixVoice(m_audioEngine, voiceDetails.InputChannelCount, voiceDetails.InputSampleRate);
			m_hudAudioVoice = new SubmixVoice(m_audioEngine, voiceDetails.InputChannelCount, voiceDetails.InputSampleRate);
			m_gameAudioVoiceDesc = new VoiceSendDescriptor[1]
			{
				new VoiceSendDescriptor(m_gameAudioVoice)
			};
			m_musicAudioVoiceDesc = new VoiceSendDescriptor[1]
			{
				new VoiceSendDescriptor(m_musicAudioVoice)
			};
			m_hudAudioVoiceDesc = new VoiceSendDescriptor[1]
			{
				new VoiceSendDescriptor(m_hudAudioVoice)
			};
			if (m_mute)
			{
				m_gameAudioVoice.SetVolume(0f);
				m_musicAudioVoice.SetVolume(0f);
			}
		}

		public void SetReverbParameters(float diffusion, float roomSize)
		{
		}

		public void ChangeGlobalVolume(float level, float time)
		{
			level = MyMath.Clamp(level, 0f, 1f);
			m_globalVolumeChanging = false;
			if (level == m_globalVolumeLevel)
			{
				return;
			}
			if (time <= 0f)
			{
				m_globalVolumeLevel = level;
				if (m_musicAudioVoice != null && !m_musicAudioVoice.IsDisposed)
				{
					m_musicAudioVoice.SetVolume(m_volumeMusic * level);
				}
				if (m_hudAudioVoice != null && !m_hudAudioVoice.IsDisposed)
				{
					m_hudAudioVoice.SetVolume(m_volumeHud * level);
				}
				if (m_gameAudioVoice != null && !m_gameAudioVoice.IsDisposed)
				{
					m_gameAudioVoice.SetVolume(m_volumeDefault * level);
				}
			}
			else
			{
				m_globalVolumeChanging = true;
				m_globalVolumeIncrement = (level - m_globalVolumeLevel) / 60f / time;
				m_globalVolumeTarget = level;
				m_globalVolumeRaising = level > m_globalVolumeLevel;
			}
		}

		private void GlobalVolumeUpdate()
		{
			m_globalVolumeLevel += m_globalVolumeIncrement;
			if ((m_globalVolumeRaising && m_globalVolumeLevel >= m_globalVolumeTarget) || (!m_globalVolumeRaising && m_globalVolumeLevel <= m_globalVolumeTarget))
			{
				m_globalVolumeLevel = m_globalVolumeTarget;
				m_globalVolumeChanging = false;
			}
			if (m_musicAudioVoice != null)
			{
				m_musicAudioVoice.SetVolume(m_volumeMusic * m_globalVolumeLevel);
			}
			if (m_hudAudioVoice != null)
			{
				m_hudAudioVoice.SetVolume(m_volumeHud * m_globalVolumeLevel);
			}
			if (m_gameAudioVoice != null)
			{
				m_gameAudioVoice.SetVolume(m_volumeDefault * m_globalVolumeLevel);
			}
		}

		public void EnableMasterLimiter(bool enable)
		{
			if (m_useVolumeLimiter && m_soundLimiterReady && enable != m_soundLimiterSet)
			{
				if (enable)
				{
					m_masterVoice.EnableEffect(0);
				}
				else
				{
					m_masterVoice.DisableEffect(0);
				}
				m_soundLimiterSet = enable;
			}
		}

		private void m_audioEngine_CriticalError(object sender, ErrorEventArgs e)
		{
			m_cueBank.SetAudioEngine(null);
			if (e.ErrorCode.Code == -2004353023)
			{
				MyLog.Default.WriteLine("Audio device removed");
			}
			else
			{
				MyLog.Default.WriteLine("Audio error: " + e.ErrorCode);
			}
			m_deviceLost = true;
		}

		private void CreateX3DAudio()
		{
			if (m_audioEngine != null)
			{
				m_x3dAudio = new X3DAudio(m_deviceDetails.OutputFormat.ChannelMask);
				string text = m_deviceDetails.DisplayName;
				int num = text.IndexOf('\0');
				if (num != -1)
				{
					text = text.Substring(0, num);
				}
				MyLog.Default.WriteLine($"MyAudio.CreateX3DAudio - Device: {text} - Channel #: {m_deviceDetails.OutputFormat.Channels} - Sample rate: {SampleRate}");
			}
		}

		private void DisposeVoices()
		{
			m_hudAudioVoice?.Dispose();
			m_musicAudioVoice?.Dispose();
			m_gameAudioVoice?.Dispose();
			m_masterVoice?.Dispose();
		}

		private void CheckIfDeviceChanged()
		{
			MyTimeSpan myTimeSpan = new MyTimeSpan(Stopwatch.GetTimestamp());
			if (myTimeSpan > m_nextDeviceCountCheck)
			{
				m_nextDeviceCountCheck = myTimeSpan + MyTimeSpan.FromSeconds(1.0);
				try
				{
					m_deviceLost = m_lastDeviceCount != GetDeviceCount();
				}
				catch (SharpDXException)
				{
					m_deviceLost = true;
				}
			}
			if (!m_deviceLost)
			{
				return;
			}
			try
			{
				Init();
			}
			catch (Exception ex2)
			{
				MyLog.Default.WriteLine("Exception during loading audio engine. Game continues, but without sound. Details: " + ex2.ToString(), LoggingOptions.AUDIO);
				MyLog.Default.WriteLine("Device ID: " + m_deviceDetails.DeviceID, LoggingOptions.AUDIO);
				MyLog.Default.WriteLine("Device name: " + m_deviceDetails.DisplayName, LoggingOptions.AUDIO);
				MyLog.Default.WriteLine("Device role: " + m_deviceDetails.Role, LoggingOptions.AUDIO);
				MyLog.Default.WriteLine("Output format: " + m_deviceDetails.OutputFormat, LoggingOptions.AUDIO);
				m_canPlay = false;
				return;
			}
			m_deviceLost = false;
			if (m_initParams.SimulateNoSoundCard)
			{
				m_canPlay = false;
			}
			if (!m_canPlay)
			{
				return;
			}
			if (m_cueBank != null)
			{
				m_cueBank.SetAudioEngine(m_audioEngine, m_gameAudioVoiceDesc, m_hudAudioVoiceDesc, m_musicAudioVoiceDesc);
			}
			m_gameAudioVoice.SetVolume(m_volumeDefault * m_globalVolumeLevel);
			m_hudAudioVoice.SetVolume(m_volumeHud * m_globalVolumeLevel);
			m_musicAudioVoice.SetVolume(m_volumeMusic * m_globalVolumeLevel);
			lock (m_3Dsounds)
			{
				m_3Dsounds.Clear();
			}
			if (m_musicCue != null && m_musicCue.IsPlaying)
			{
				m_musicCue = PlaySound(m_musicCue.CueEnum, null, MySoundDimensions.D2, skipIntro: false, skipToEnd: false, isMusic: true);
				if (m_musicCue != null)
				{
					m_musicCue.SetOutputVoices(m_musicAudioVoiceDesc);
				}
				UpdateMusic(0);
			}
		}

		public void LoadData(MyAudioInitParams initParams, ListReader<MySoundData> sounds, ListReader<MyAudioEffect> effects)
		{
			MyLog.Default.WriteLine("MyAudio.LoadData - START");
			MyLog.Default.IncreaseIndent();
			m_initParams = initParams;
			m_sounds = sounds;
			m_effects = effects;
			m_canPlay = true;
			try
			{
				Init();
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Exception during loading audio engine. Game continues, but without sound. Details: " + ex.ToString(), LoggingOptions.AUDIO);
				MyLog.Default.WriteLine("Device ID: " + m_deviceDetails.DeviceID, LoggingOptions.AUDIO);
				MyLog.Default.WriteLine("Device name: " + m_deviceDetails.DisplayName, LoggingOptions.AUDIO);
				MyLog.Default.WriteLine("Device role: " + m_deviceDetails.Role, LoggingOptions.AUDIO);
				MyLog.Default.WriteLine("Output format: " + m_deviceDetails.OutputFormat, LoggingOptions.AUDIO);
				m_canPlay = false;
			}
			if (m_initParams.SimulateNoSoundCard)
			{
				m_canPlay = false;
			}
			if (m_canPlay)
			{
				m_cueBank = new MyCueBank(m_audioEngine, sounds, m_gameAudioVoiceDesc, m_hudAudioVoiceDesc, m_musicAudioVoiceDesc, initParams.CacheLoaded);
				m_cueBank.UseSameSoundLimiter = m_useSameSoundLimiter;
				m_cueBank.SetSameSoundLimiter();
				m_cueBank.DisablePooling = initParams.DisablePooling;
				m_effectBank = new MyEffectBank(effects, m_audioEngine);
				m_3Dsounds = new List<IMy3DSoundEmitter>();
				m_listener = new Listener();
				m_listener.SetDefaultValues();
				m_helperEmitter = new Emitter();
				m_helperEmitter.SetDefaultValues();
				m_musicOn = true;
				m_gameSoundsOn = true;
				m_musicAllowed = true;
				if (m_musicCue != null && m_musicCue.IsPlaying)
				{
					m_musicCue = PlaySound(m_musicCue.CueEnum, null, MySoundDimensions.D2, skipIntro: false, skipToEnd: false, isMusic: true);
					if (m_musicCue != null)
					{
						m_musicCue.SetOutputVoices(m_musicAudioVoiceDesc);
						m_musicAudioVoice.SetVolume(m_volumeMusic * m_globalVolumeLevel);
					}
					UpdateMusic(0);
				}
				else
				{
					m_musicState = MyMusicState.Stopped;
				}
				m_loopMusic = true;
				m_transitionForward = false;
				m_timeFromTransitionStart = 0;
				m_soundInstancesTotal2D = 0;
				m_soundInstancesTotal3D = 0;
			}
			MyLog.Default.DecreaseIndent();
			MyLog.Default.WriteLine("MyAudio.LoadData - END");
		}

		public void SetSameSoundLimiter()
		{
			if (m_cueBank != null)
			{
				m_cueBank.UseSameSoundLimiter = m_useSameSoundLimiter;
				m_cueBank.SetSameSoundLimiter();
			}
		}

		public void UnloadData()
		{
			MyLog.Default.WriteLine("MyAudio.UnloadData - START");
			if (m_3Dsounds != null)
			{
				lock (m_3Dsounds)
				{
					m_3Dsounds.Clear();
				}
			}
			if (m_canPlay)
			{
				m_audioEngine.StopEngine();
				m_cueBank?.Dispose();
			}
			SoloCue = null;
			DisposeVoices();
			if (m_audioEngine != null)
			{
				m_audioEngine.CriticalError -= m_audioEngine_CriticalError;
				m_audioPlatform.DisposeAudioEngine();
				m_audioEngine = null;
				m_cueBank = null;
				m_masterVoice = null;
				m_hudAudioVoice = null;
				m_gameAudioVoice = null;
				m_musicAudioVoice = null;
			}
			m_canPlay = false;
			m_reverbSet = false;
			MyLog.Default.WriteLine("MyAudio.UnloadData - END");
			Instance = null;
		}

		public void ClearSounds()
		{
			if (m_cueBank != null)
			{
				m_cueBank.ClearSounds();
			}
		}

		public void ReloadData()
		{
			UnloadData();
			LoadData(m_initParams, m_sounds, m_effects);
		}

		public void ReloadData(ListReader<MySoundData> sounds, ListReader<MyAudioEffect> effects)
		{
			lock (lockObject)
			{
				m_loading = true;
			}
			UnloadData();
			LoadData(m_initParams, sounds, effects);
			lock (lockObject)
			{
				m_loading = false;
			}
		}

		public void PauseGameSounds()
		{
			if (m_canPlay)
			{
				GameSoundIsPaused = true;
				m_gameAudioVoice.SetVolume(0f);
				m_canUpdate3dSounds = false;
				if (m_musicCue != null)
				{
					m_musicCue.VolumeMultiplier = 0f;
				}
			}
		}

		public void ResumeGameSounds()
		{
			if (m_canPlay)
			{
				GameSoundIsPaused = false;
				if (!Mute)
				{
					m_gameAudioVoice.SetVolume(m_volumeDefault * m_globalVolumeLevel);
				}
				m_canUpdate3dSounds = true;
				if (m_musicCue != null)
				{
					m_musicCue.VolumeMultiplier = 1f;
				}
			}
		}

		public bool IsValidTransitionCategory(MyStringId transitionCategory, MyStringId musicCategory)
		{
			if (m_canPlay)
			{
				return m_cueBank.IsValidTransitionCategory(transitionCategory, musicCategory);
			}
			return false;
		}

		public void PlayMusic(MyMusicTrack? track = null, int priorityForRandom = 0)
		{
			if (!m_canPlay || !m_musicAllowed)
			{
				return;
			}
			bool flag = false;
			if (track.HasValue)
			{
				if (HasAnyTransition())
				{
					m_nextTransitions.Clear();
				}
				if (!m_cueBank.IsValidTransitionCategory(track.Value.TransitionCategory, track.Value.MusicCategory))
				{
					flag = true;
				}
				else
				{
					ApplyTransition(track.Value.TransitionCategory, 1, track.Value.MusicCategory, loop: false);
				}
			}
			else if (m_musicState == MyMusicState.Stopped && !HasAnyTransition())
			{
				flag = true;
			}
			if (flag)
			{
				MyStringId? randomTransitionEnum = GetRandomTransitionEnum();
				if (randomTransitionEnum.HasValue)
				{
					ApplyTransition(randomTransitionEnum.Value, priorityForRandom, null, loop: false);
				}
			}
		}

		public IMySourceVoice PlayMusicCue(MyCueId musicCue, bool overrideMusicAllowed = false)
		{
			if (!m_canPlay || (!m_musicAllowed && !overrideMusicAllowed))
			{
				return null;
			}
			m_musicCue = PlaySound(musicCue, null, MySoundDimensions.D2, skipIntro: false, skipToEnd: false, isMusic: true);
			if (m_musicCue != null)
			{
				m_musicCue.SetOutputVoices(m_musicAudioVoiceDesc);
				m_musicAudioVoice.SetVolume(m_volumeMusic * m_globalVolumeLevel);
			}
			return m_musicCue;
		}

		public void StopMusic()
		{
			m_currentTransition = null;
			m_nextTransitions.Clear();
			m_musicState = MyMusicState.Stopped;
			if (m_musicCue == null)
			{
				return;
			}
			try
			{
				m_musicCue.Stop();
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
				if (m_audioEngine == null || m_audioEngine.IsDisposed)
				{
					MyLog.Default.WriteLine("Audio engine disposed!", LoggingOptions.AUDIO);
				}
			}
		}

		public void MuteHud(bool mute)
		{
			if (m_canPlay)
			{
				m_hudAudioVoice.SetVolume(mute ? 0f : (m_volumeHud * m_globalVolumeLevel));
			}
		}

		public bool HasAnyTransition()
		{
			return m_nextTransitions.Count > 0;
		}

		public void Update(int stepSizeInMS, Vector3 listenerPosition, Vector3 listenerUp, Vector3 listenerFront, Vector3 listenerVelocity)
		{
			lock (lockObject)
			{
				if (m_loading)
				{
					return;
				}
			}
			if (m_canPlay)
			{
				CheckIfDeviceChanged();
			}
			if (Mute)
			{
				return;
			}
			m_updateInProgress = Thread.CurrentThread;
			try
			{
				if (m_canPlay)
				{
					m_cueBank?.Update();
				}
				if (m_canPlay)
				{
					m_listener.Position = default(RawVector3);
					m_listener.OrientTop = new RawVector3(listenerUp.X, listenerUp.Y, listenerUp.Z);
					m_listener.OrientFront = new RawVector3(listenerFront.X, listenerFront.Y, listenerFront.Z);
					m_listener.Velocity = new RawVector3(listenerVelocity.X, listenerVelocity.Y, listenerVelocity.Z);
					FireCallbacks();
					UpdateMusic(stepSizeInMS);
					Update3DCuesPositions();
					m_effectBank.Update(stepSizeInMS);
					if (m_globalVolumeChanging)
					{
						GlobalVolumeUpdate();
					}
				}
			}
			finally
			{
				m_updateInProgress = null;
			}
		}

		private void FireCallbacks()
		{
			MySourceVoice instance;
			while (m_voicesForStopPlayingCallback.TryDequeue(out instance))
			{
				instance.StoppedPlaying.InvokeIfNotNull(instance);
			}
		}

		private void UpdateMusic(int stepSizeinMS)
		{
			if (m_musicState == MyMusicState.Transition)
			{
				m_timeFromTransitionStart += stepSizeinMS;
				if (m_timeFromTransitionStart >= 1000)
				{
					m_musicState = MyMusicState.Stopped;
					if (m_musicCue != null && m_musicCue.IsPlaying)
					{
						m_musicCue.Stop(force: true);
						m_musicCue = null;
					}
				}
				else if (m_musicCue != null && m_musicCue.IsPlaying)
				{
					m_musicAudioVoice.GetVolume(out var volumeRef);
					if (volumeRef > 0f && m_musicOn)
					{
						m_musicAudioVoice.SetVolume((1f - (float)m_timeFromTransitionStart / 1000f) * m_volumeAtTransitionStart * m_globalVolumeLevel);
					}
				}
			}
			if (m_musicState == MyMusicState.Stopped)
			{
				MyMusicTransition? nextTransition = GetNextTransition();
				if (m_currentTransition.HasValue && m_nextTransitions.Count > 0 && nextTransition.HasValue && nextTransition.Value.Priority > m_currentTransition.Value.Priority)
				{
					m_nextTransitions[m_currentTransition.Value.Priority] = m_currentTransition.Value;
				}
				m_currentTransition = nextTransition;
				if (m_currentTransition.HasValue)
				{
					m_musicAudioVoice.SetVolume(m_volumeMusic * m_globalVolumeLevel);
					PlayMusicByTransition(m_currentTransition.Value);
					m_nextTransitions.Remove(m_currentTransition.Value.Priority);
					m_musicState = MyMusicState.Playing;
				}
			}
			if (m_musicState != MyMusicState.Playing || (m_musicCue != null && m_musicCue.IsPlaying))
			{
				return;
			}
			if (m_loopMusic && m_currentTransition.HasValue)
			{
				PlayMusicByTransition(m_currentTransition.Value);
				return;
			}
			m_currentTransition = null;
			MyStringId? myStringId = MyStringId.GetOrCompute("Default");
			if (myStringId.HasValue)
			{
				ApplyTransition(myStringId.Value, 0, null, loop: false);
			}
		}

		private MyStringId? GetRandomTransitionEnum()
		{
			if (m_cueBank == null)
			{
				return null;
			}
			MyStringId? randomTransitionEnum = m_cueBank.GetRandomTransitionEnum();
			while (randomTransitionEnum == NO_RANDOM)
			{
				randomTransitionEnum = m_cueBank.GetRandomTransitionEnum();
			}
			return randomTransitionEnum;
		}

		bool IMyAudio.ApplyTransition(MyStringId transitionEnum, int priority, MyStringId? category, bool loop)
		{
			return ApplyTransition(transitionEnum, priority, category, loop);
		}

		private bool ApplyTransition(MyStringId transitionEnum, int priority, MyStringId? category, bool loop)
		{
			if (!m_canPlay)
			{
				return false;
			}
			if (!m_musicAllowed)
			{
				return false;
			}
			if (category.HasValue)
			{
				if (category.Value == MyStringId.NullOrEmpty)
				{
					category = null;
				}
				else if (!m_cueBank.IsValidTransitionCategory(transitionEnum, category.Value))
				{
					MyLog.Default.WriteLine($"Category {category} doesn't exist for this transition!");
					return false;
				}
			}
			if (m_currentTransition.HasValue && m_currentTransition.Value.Priority == priority && m_currentTransition.Value.TransitionEnum == transitionEnum)
			{
				if (category.HasValue)
				{
					MyStringId category2 = m_currentTransition.Value.Category;
					MyStringId? myStringId = category;
					if (!(category2 == myStringId))
					{
						goto IL_00f6;
					}
				}
				if (m_musicState == MyMusicState.Transition && !m_transitionForward)
				{
					m_musicState = MyMusicState.Playing;
					return true;
				}
				return false;
			}
			goto IL_00f6;
			IL_00f6:
			MyStringId category3 = category ?? m_cueBank.GetRandomTransitionCategory(ref transitionEnum, ref NO_RANDOM);
			m_nextTransitions[priority] = new MyMusicTransition(priority, transitionEnum, category3);
			if (m_currentTransition.HasValue && m_currentTransition.Value.Priority > priority)
			{
				return false;
			}
			m_loopMusic = loop;
			switch (m_musicState)
			{
			case MyMusicState.Playing:
				StartTransition(forward: true);
				break;
			default:
				throw new InvalidBranchException();
			case MyMusicState.Stopped:
			case MyMusicState.Transition:
				break;
			}
			return true;
		}

		private MyMusicTransition? GetNextTransition()
		{
			if (m_nextTransitions.Count > 0)
			{
				return m_nextTransitions[m_nextTransitions.Keys[m_nextTransitions.Keys.Count - 1]];
			}
			return null;
		}

		private void StartTransition(bool forward)
		{
			m_transitionForward = forward;
			m_musicState = MyMusicState.Transition;
			m_timeFromTransitionStart = 0;
			m_volumeAtTransitionStart = m_volumeMusic;
		}

		internal void StopTransition(int priority)
		{
			if (m_nextTransitions.ContainsKey(priority))
			{
				m_nextTransitions.Remove(priority);
			}
			if (m_currentTransition.HasValue && priority == m_currentTransition.Value.Priority && m_musicState != MyMusicState.Transition)
			{
				StartTransition(forward: false);
			}
		}

		private void PlayMusicByTransition(MyMusicTransition transition)
		{
			if (m_cueBank != null && m_musicAllowed)
			{
				m_musicCue = PlaySound(m_cueBank.GetTransitionCue(transition.TransitionEnum, transition.Category), null, MySoundDimensions.D2, skipIntro: false, skipToEnd: false, isMusic: true);
				if (m_musicCue != null)
				{
					m_musicCue.SetOutputVoices(m_musicAudioVoiceDesc);
					m_musicAudioVoice.SetVolume(m_volumeMusic * m_globalVolumeLevel);
				}
			}
		}

		private float VolumeVariation(MySoundData cue)
		{
			return MyUtils.GetRandomFloat(-1f, 1f) * cue.VolumeVariation * 0.07f;
		}

		private float PitchVariation(MySoundData cue)
		{
			return MyUtils.GetRandomFloat(-1f, 1f) * cue.PitchVariation / 100f;
		}

		float IMyAudio.SemitonesToFrequencyRatio(float semitones)
		{
			return SemitonesToFrequencyRatio(semitones);
		}

		private float SemitonesToFrequencyRatio(float semitones)
		{
			return XAudio2.SemitonesToFrequencyRatio(semitones);
		}

		private void Add3DCueToUpdateList(IMy3DSoundEmitter source)
		{
			lock (m_3Dsounds)
			{
				if (!m_3Dsounds.Contains(source))
				{
					m_3Dsounds.Add(source);
				}
			}
		}

		public int GetUpdating3DSoundsCount()
		{
			if (m_3Dsounds == null)
			{
				return 0;
			}
			return m_3Dsounds.Count;
		}

		public int GetSoundInstancesTotal2D()
		{
			return m_soundInstancesTotal2D;
		}

		public int GetSoundInstancesTotal3D()
		{
			return m_soundInstancesTotal3D;
		}

		private void Update3DCuesState(bool updatePosition = false)
		{
			if (!m_canUpdate3dSounds || m_3Dsounds == null || m_3Dsounds.Count <= 0)
			{
				return;
			}
			lock (m_3Dsounds)
			{
				int num = 0;
				while (num < m_3Dsounds.Count)
				{
					IMy3DSoundEmitter my3DSoundEmitter = m_3Dsounds[num];
					object syncRoot = my3DSoundEmitter.SyncRoot;
					if (Monitor.TryEnter(syncRoot))
					{
						try
						{
							IMySourceVoice sound = my3DSoundEmitter.Sound;
							if (sound == null)
							{
								m_3Dsounds.RemoveAt(num);
								continue;
							}
							if (!sound.IsPlaying && !sound.IsBuffered)
							{
								my3DSoundEmitter.SetSound(null, "Update3DCuesState");
								m_3Dsounds.RemoveAt(num);
								continue;
							}
							if (((MySourceVoice)sound).Emitter != my3DSoundEmitter)
							{
								MyLog.Default.WriteLine($"Emitter sound history: {my3DSoundEmitter.DebugData}");
							}
							if (updatePosition)
							{
								Update3DCuePosition(my3DSoundEmitter);
							}
							num++;
						}
						finally
						{
							Monitor.Exit(syncRoot);
						}
					}
					else
					{
						num++;
					}
				}
			}
		}

		private void Update3DCuesPositions()
		{
			if (m_canPlay)
			{
				Update3DCuesState(updatePosition: true);
			}
		}

		private void Update3DCuePosition(IMy3DSoundEmitter source)
		{
			MySoundData mySoundData = m_cueBank?.GetCue(source.SoundId);
			if (mySoundData == null || source.Sound == null)
			{
				return;
			}
			MySourceVoice mySourceVoice = source.Sound as MySourceVoice;
			if (mySourceVoice == null)
			{
				return;
			}
			int num = 0;
			try
			{
				float num2 = source.CustomMaxDistance ?? mySoundData.MaxDistance;
				if (!mySourceVoice.IsBuffered)
				{
					num = 1;
					Vector3D sourcePosition = source.SourcePosition;
					num = 2;
					Vector3 velocity = source.Velocity;
					num = 3;
					m_helperEmitter.UpdateValuesOmni(sourcePosition, velocity, mySoundData, m_deviceDetails.OutputFormat.Channels, num2, source.DopplerScaler);
					num = 4;
					mySourceVoice.DistanceToListener = Apply3D(mySourceVoice, m_listener, m_helperEmitter, source.SourceChannels, m_deviceDetails.OutputFormat.Channels, m_calculateFlags, num2, mySourceVoice.FrequencyRatio, mySourceVoice.Silent, !source.Realistic, m_enableDoppler);
				}
				else
				{
					num = 5;
					Vector3D sourcePosition2 = source.SourcePosition;
					num = 6;
					Vector3 velocity2 = source.Velocity;
					num = 7;
					m_helperEmitter.UpdateValuesOmni(sourcePosition2, velocity2, num2, m_deviceDetails.OutputFormat.Channels, mySoundData.VolumeCurve, source.DopplerScaler);
					num = 8;
					mySourceVoice.DistanceToListener = Apply3D(mySourceVoice, m_listener, m_helperEmitter, source.SourceChannels, m_deviceDetails.OutputFormat.Channels, m_calculateFlags, num2, mySourceVoice.FrequencyRatio, mySourceVoice.Silent, !source.Realistic, m_enableDoppler);
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
				MyLog.Default.WriteLine($"{mySourceVoice.IsBuffered} {num} {mySourceVoice.Voice == null} {m_listener == null}");
				MyLog.Default.WriteLine($"Emitter sound history: {source.DebugData}");
				MyLog.Default.WriteLine("SourceVoice history: " + mySourceVoice.DebugData);
				_ = mySourceVoice.Emitter;
				throw;
			}
		}

		private float Apply3D(MySourceVoice managedVoice, Listener listener, Emitter emitter, int srcChannels, int dstChannels, CalculateFlags flags, float maxDistance, float frequencyRatio, bool silent, bool use3DCalculation = true, bool fullDoppler = true)
		{
			DspSettings dspSettings = new DspSettings(srcChannels, dstChannels);
			int num = srcChannels * dstChannels;
			SourceVoice voice = managedVoice.Voice;
			if (use3DCalculation)
			{
				if (!fullDoppler)
				{
					listener.Velocity = (emitter.Velocity = default(RawVector3));
				}
				m_x3dAudio.Calculate(listener, emitter, flags, dspSettings);
				dspSettings.DopplerFactor = MathHelper.Clamp(dspSettings.DopplerFactor, 0.9f, 1f);
				voice.SetFrequencyRatio(frequencyRatio * dspSettings.DopplerFactor);
			}
			else
			{
				dspSettings.EmitterToListenerDistance = Vector3.Distance(new Vector3(listener.Position.X, listener.Position.Y, listener.Position.Z), new Vector3(emitter.Position.X, emitter.Position.Y, emitter.Position.Z));
				if (srcChannels <= 2 && dstChannels <= 8)
				{
					float[] array = ((srcChannels == 1) ? m_outputMatrixMono : m_outputMatrixStereo);
					for (int i = 0; i < dspSettings.MatrixCoefficients.Length; i++)
					{
						dspSettings.MatrixCoefficients[i] = array[i];
					}
				}
				else
				{
					for (int j = 0; j < num; j++)
					{
						dspSettings.MatrixCoefficients[j] = 1f;
					}
				}
			}
			if (emitter.InnerRadius == 0f)
			{
				float num2 = ((!silent) ? MathHelper.Clamp(1f - dspSettings.EmitterToListenerDistance / maxDistance, 0f, 1f) : 0f);
				for (int k = 0; k < num; k++)
				{
					dspSettings.MatrixCoefficients[k] *= num2;
				}
			}
			try
			{
				voice.SetOutputMatrix(null, dspSettings.SourceChannelCount, dspSettings.DestinationChannelCount, dspSettings.MatrixCoefficients);
			}
			catch
			{
				MyLog @default = MyLog.Default;
				@default.WriteLine($"Exception at SetOutputMatrix [{srcChannels}x{dstChannels}] IsValid: {voice.IsValid()}");
				try
				{
					@default.WriteLine($"{managedVoice.CueEnum.Hash.String};{managedVoice.GetOutputChannels()};{managedVoice.IsPlaying};{managedVoice.IsPaused}");
					@default.WriteLine("Output voices: ");
					VoiceSendDescriptor[] currentOutputVoices = managedVoice.CurrentOutputVoices;
					if (currentOutputVoices == null)
					{
						@default.WriteLine("NULL");
					}
					else
					{
						VoiceSendDescriptor[] array2 = currentOutputVoices;
						for (int l = 0; l < array2.Length; l++)
						{
							VoiceDetails voiceDetails = array2[l].OutputVoice.VoiceDetails;
							@default.WriteLine($"{(VoiceFlags)voiceDetails.ActiveFlags} {voiceDetails.InputChannelCount} {voiceDetails.InputSampleRate}");
						}
					}
					@default.WriteLine("Current matrix");
					try
					{
						voice.GetOutputMatrix(null, srcChannels, dstChannels, dspSettings.MatrixCoefficients);
						@default.WriteLine(string.Join(";", dspSettings.MatrixCoefficients));
					}
					catch
					{
						@default.WriteLine("Get failed");
						for (int m = 1; m <= 10; m++)
						{
							try
							{
								float[] array3 = new float[srcChannels * m];
								voice.GetOutputMatrix(null, srcChannels, m, array3);
								@default.WriteLine($"Success: GetOutputMatrix [{srcChannels}x{m}]");
								@default.WriteLine(string.Join(";", array3));
							}
							catch
							{
								continue;
							}
							break;
						}
					}
					if (!currentOutputVoices.IsNullOrEmpty())
					{
						try
						{
							@default.WriteLine("Retry with single");
							voice.SetOutputMatrix(currentOutputVoices[0].OutputVoice, dspSettings.SourceChannelCount, dspSettings.DestinationChannelCount, dspSettings.MatrixCoefficients);
						}
						catch
						{
							@default.WriteLine("Single failed");
						}
					}
					@default.WriteLine("Test outChannels");
					for (int n = 1; n <= 5; n++)
					{
						for (int num3 = 1; num3 <= 5; num3++)
						{
							try
							{
								float[] levelMatrixRef = new float[num3 * n];
								voice.SetOutputMatrix(null, num3, n, levelMatrixRef);
								@default.WriteLine($"Success: SetOutputMatrix [{num3}x{n}]");
							}
							catch
							{
								continue;
							}
							break;
						}
					}
					int outputChannels = managedVoice.GetOutputChannels();
					DeviceDetails deviceDetails = AudioPlatform.GetDeviceDetails(0, forceRefresh: true);
					@default.WriteLine($"Retry with new dimensions [{outputChannels}x{deviceDetails.OutputFormat.Channels}]");
					float[] levelMatrixRef2 = new float[outputChannels * deviceDetails.OutputFormat.Channels];
					voice.SetOutputMatrix(null, outputChannels, deviceDetails.OutputFormat.Channels, levelMatrixRef2);
					@default.WriteLine("Retry succeeded");
				}
				catch
				{
					@default.WriteLine("FAIL");
				}
				throw;
			}
			return dspSettings.EmitterToListenerDistance;
		}

		private void StopUpdating3DCue(IMy3DSoundEmitter source)
		{
			if (m_canPlay)
			{
				lock (m_3Dsounds)
				{
					m_3Dsounds.Remove(source);
				}
			}
		}

		public void StopUpdatingAll3DCues()
		{
			if (m_canPlay)
			{
				lock (m_3Dsounds)
				{
					m_3Dsounds.Clear();
				}
			}
		}

		bool IMyAudio.SourceIsCloseEnoughToPlaySound(Vector3 sourcePosition, MyCueId cueId, float? customMaxDistance)
		{
			return SourceIsCloseEnoughToPlaySound(sourcePosition, cueId, customMaxDistance);
		}

		public bool SourceIsCloseEnoughToPlaySound(Vector3 sourcePosition, MyCueId cueId, float? customMaxDistance = 0f)
		{
			if (m_cueBank == null || cueId.Hash == MyStringHash.NullOrEmpty)
			{
				return false;
			}
			MySoundData cue = m_cueBank.GetCue(cueId);
			if (cue == null)
			{
				return false;
			}
			float num = sourcePosition.LengthSquared();
			if (customMaxDistance > 0f)
			{
				return num <= customMaxDistance * customMaxDistance;
			}
			if (cue.UpdateDistance > 0f)
			{
				return num <= cue.UpdateDistance * cue.UpdateDistance;
			}
			return num <= cue.MaxDistance * cue.MaxDistance;
		}

		private MySourceVoice PlaySound(MyCueId cueId, IMy3DSoundEmitter source = null, MySoundDimensions type = MySoundDimensions.D2, bool skipIntro = false, bool skipToEnd = false, bool isMusic = false)
		{
			int waveNumber;
			MySourceVoice sound = GetSound(cueId, out waveNumber, source, type, isMusic);
			if (source != null)
			{
				source.LastPlayedWaveNumber = -1;
			}
			if (sound != null)
			{
				sound.Start(skipIntro, skipToEnd);
				if (source != null)
				{
					source.LastPlayedWaveNumber = waveNumber;
				}
			}
			return sound;
		}

		private MySourceVoice GetSound(MyCueId cueId, out int waveNumber, IMy3DSoundEmitter source = null, MySoundDimensions type = MySoundDimensions.D2, bool isMusic = false)
		{
			waveNumber = -1;
			if (cueId.Hash == MyStringHash.NullOrEmpty || !m_canPlay || m_cueBank == null)
			{
				return null;
			}
			MySoundData cue = m_cueBank.GetCue(cueId);
			if (cue == null)
			{
				return null;
			}
			if (SoloCue != null && SoloCue != cue)
			{
				return null;
			}
			MyVoicePoolType poolType = MyVoicePoolType.Sound;
			if (cue.IsHudCue)
			{
				poolType = MyVoicePoolType.Hud;
			}
			else if (isMusic)
			{
				poolType = MyVoicePoolType.Music;
			}
			int tryIgnoreWaveNumber = source?.LastPlayedWaveNumber ?? (-1);
			MySourceVoice voice = m_cueBank.GetVoice(cueId, out waveNumber, type, tryIgnoreWaveNumber, poolType);
			MySoundDimensions mySoundDimensions = type;
			if (voice == null && source != null && source.Force3D)
			{
				mySoundDimensions = ((type != MySoundDimensions.D3) ? MySoundDimensions.D3 : MySoundDimensions.D2);
				voice = m_cueBank.GetVoice(cueId, out waveNumber, mySoundDimensions, tryIgnoreWaveNumber, poolType);
			}
			if (voice == null)
			{
				return null;
			}
			float num = cue.Volume;
			if (source != null && source.CustomVolume.HasValue)
			{
				num = source.CustomVolume.Value;
			}
			if (cue.VolumeVariation != 0f)
			{
				float num2 = VolumeVariation(cue);
				num = MathHelper.Clamp(num + num2, 0f, 1f);
			}
			voice.Emitter = source;
			voice.VolumeMultiplier = 1f;
			voice.SetVolume(num);
			float num3 = cue.Pitch;
			if (cue.PitchVariation != 0f)
			{
				num3 += PitchVariation(cue);
			}
			if (cue.DisablePitchEffects)
			{
				num3 = 0f;
			}
			if (num3 != 0f)
			{
				voice.FrequencyRatio = SemitonesToFrequencyRatio(num3);
			}
			else
			{
				voice.FrequencyRatio = 1f;
			}
			if (source != null)
			{
				string text;
				if ((text = source.DebugData as string) != null && text.Length > 1000)
				{
					source.DebugData = string.Empty;
				}
				string debugData = voice.DebugData;
				if (debugData != null && debugData.Length > 1000)
				{
					voice.DebugData = string.Empty;
				}
			}
			if (type == MySoundDimensions.D3)
			{
				float num4 = source.CustomMaxDistance ?? cue.MaxDistance;
				m_helperEmitter.UpdateValuesOmni(source.SourcePosition, source.Velocity, cue, m_deviceDetails.OutputFormat.Channels, num4, source.DopplerScaler);
				source.SourceChannels = voice.GetOutputChannels();
				voice.DistanceToListener = Apply3D(voice, m_listener, m_helperEmitter, source.SourceChannels, m_deviceDetails.OutputFormat.Channels, m_calculateFlags, num4, voice.FrequencyRatio, voice.Silent, !source.Realistic, m_enableDoppler);
				Update3DCuesState();
				Add3DCueToUpdateList(source);
				m_soundInstancesTotal3D++;
			}
			else
			{
				voice.DistanceToListener = 0f;
				int outputChannels = voice.GetOutputChannels();
				int num5 = outputChannels * m_deviceDetails.OutputFormat.Channels;
				float[] array = new float[num5];
				for (int i = 0; i < num5; i++)
				{
					array[i] = 1f;
				}
				voice.Voice.SetOutputMatrix(null, outputChannels, m_deviceDetails.OutputFormat.Channels, array);
				StopUpdating3DCue(source);
				m_soundInstancesTotal2D++;
			}
			return voice;
		}

		IMySourceVoice IMyAudio.PlaySound(MyCueId cueId, IMy3DSoundEmitter source, MySoundDimensions type, bool skipIntro, bool skipToEnd)
		{
			return PlaySound(cueId, source, type, skipIntro, skipToEnd);
		}

		IMySourceVoice IMyAudio.GetSound(MyCueId cueId, IMy3DSoundEmitter source, MySoundDimensions type)
		{
			int waveNumber;
			return GetSound(cueId, out waveNumber, source, type);
		}

		IMySourceVoice IMyAudio.GetSound(IMy3DSoundEmitter source, MySoundDimensions dimension)
		{
			if (!m_canPlay || m_deviceLost)
			{
				return null;
			}
			WaveFormat waveFormat = new WaveFormat(24000, 16, 1);
			string text;
			if ((text = source.DebugData as string) != null && text.Length > 1000)
			{
				source.DebugData = string.Empty;
			}
			source.SourceChannels = waveFormat.Channels;
			MySourceVoice mySourceVoice = new MySourceVoice(m_audioEngine, waveFormat);
			mySourceVoice.Emitter = source;
			float volume = source.CustomVolume ?? 1f;
			float maxDistance = source.CustomMaxDistance ?? 0f;
			mySourceVoice.SetVolume(volume);
			if (dimension == MySoundDimensions.D3)
			{
				m_helperEmitter.UpdateValuesOmni(source.SourcePosition, source.Velocity, maxDistance, m_deviceDetails.OutputFormat.Channels, MyCurveType.Linear, source.DopplerScaler);
				mySourceVoice.DistanceToListener = Apply3D(mySourceVoice, m_listener, m_helperEmitter, source.SourceChannels, m_deviceDetails.OutputFormat.Channels, m_calculateFlags, maxDistance, mySourceVoice.FrequencyRatio, mySourceVoice.Silent, !source.Realistic, m_enableDoppler);
				Update3DCuesState();
				Add3DCueToUpdateList(source);
				m_soundInstancesTotal3D++;
			}
			return mySourceVoice;
		}

		public void WriteDebugInfo(StringBuilder sb)
		{
			if (m_cueBank != null)
			{
				m_cueBank.WriteDebugInfo(sb);
			}
		}

		public bool IsLoopable(MyCueId cueId)
		{
			if (cueId.Hash == MyStringHash.NullOrEmpty || m_cueBank == null)
			{
				return false;
			}
			return m_cueBank.GetCue(cueId)?.Loopable ?? false;
		}

		public ListReader<IMy3DSoundEmitter> Get3DSounds()
		{
			return m_3Dsounds;
		}

		public IMyAudioEffect ApplyEffect(IMySourceVoice input, MyStringHash effect, MyCueId[] cueIds = null, float? duration = null, bool musicEffect = false)
		{
			if (m_effectBank == null)
			{
				return null;
			}
			List<MySourceVoice> list = new List<MySourceVoice>();
			if (cueIds != null)
			{
				foreach (MyCueId cueId in cueIds)
				{
					int waveNumber;
					MySourceVoice sound = GetSound(cueId, out waveNumber);
					if (sound != null)
					{
						list.Add(sound);
					}
				}
			}
			IMyAudioEffect myAudioEffect = m_effectBank.CreateEffect(input, effect, list.ToArray(), duration);
			if (musicEffect && myAudioEffect.OutputSound is MySourceVoice)
			{
				(myAudioEffect.OutputSound as MySourceVoice).SetOutputVoices(m_musicAudioVoiceDesc);
			}
			return myAudioEffect;
		}

		public Vector3 GetListenerPosition()
		{
			if (m_listener == null)
			{
				return Vector3.Zero;
			}
			return new Vector3(m_listener.Position.X, m_listener.Position.Y, m_listener.Position.Z);
		}

		public void EnumerateLastSounds(Action<StringBuilder, bool> a)
		{
		}

		public void DisposeCache()
		{
			MyInMemoryWaveDataCache.Static.Dispose();
		}

		public void Preload(string soundFile)
		{
			MyInMemoryWaveDataCache.Static.Preload(soundFile);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("DEBUG")]
		private void CheckUpdate()
		{
		}

		internal void EnqueueStopPlayingCallback(MySourceVoice voice)
		{
			m_voicesForStopPlayingCallback.Enqueue(voice);
		}
	}
}
