using System;
using System.Runtime.InteropServices;
using DirectShowLib;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Collections;
using VRage.Platform.Windows.Render;

namespace VRage.Platform.Windows.DShow
{
	internal class MyVideoPlayer : ISampleGrabberCB, IDisposable, IVideoPlayer
	{
		private Guid MEDIATYPE_Video = new Guid(1935960438, 0, 16, 128, 0, 0, 170, 0, 56, 155, 113);

		private Guid MEDIATYPE_Audio = new Guid(1935963489, 0, 16, 128, 0, 0, 170, 0, 56, 155, 113);

		private Guid MEDIASUBTYPE_RGB24 = new Guid(3828804477u, 21071, 4558, 159, 83, 0, 32, 175, 11, 167, 112);

		private Guid MEDIASUBTYPE_RGB32 = new Guid(3828804478u, 21071, 4558, 159, 83, 0, 32, 175, 11, 167, 112);

		private Guid FORMAT_VideoInfo = new Guid(89694080u, 50006, 4558, 191, 1, 0, 170, 0, 85, 89, 90);

		private object m_comObject;

		protected IGraphBuilder m_graphBuilder;

		private IMediaControl m_mediaControl;

		private IMediaEventEx m_mediaEvent;

		private IMediaPosition m_mediaPosition;

		private IBasicAudio m_basicAudio;

		private IMediaSeeking m_mediaSeeking;

		private MySwapQueue<byte[]> m_videoDataRgba;

		private int videoWidth;

		private int videoHeight;

		private long avgTimePerFrame;

		private int bitRate;

		private VideoState currentState;

		private bool isDisposed;

		private long currentPosition;

		private long videoDuration;

		private byte alphaTransparency = byte.MaxValue;

		private Texture2D m_texture;

		private ShaderResourceView m_srv;

		private const Format VIDEO_FORMAT = Format.B8G8R8A8_UNorm_SRgb;

		public int VideoWidth => videoWidth;

		public int VideoHeight => videoHeight;

		public double CurrentPosition
		{
			get
			{
				return (double)currentPosition / 10000000.0;
			}
			set
			{
				if (value < 0.0)
				{
					value = 0.0;
				}
				if (value > Duration)
				{
					value = Duration;
				}
				m_mediaPosition.put_CurrentPosition(value);
				currentPosition = (long)value * 10000000;
			}
		}

		public string CurrentPositionAsTimeString
		{
			get
			{
				double num = (double)currentPosition / 10000000.0;
				double num2 = num / 60.0;
				int num3 = (int)Math.Floor(num2 / 60.0);
				int num4 = (int)Math.Floor(num2 - (double)(num3 * 60));
				int num5 = (int)Math.Floor(num - (double)(num4 * 60));
				return ((num3 < 10) ? ("0" + num3) : num3.ToString()) + ":" + ((num4 < 10) ? ("0" + num4) : num4.ToString()) + ":" + ((num5 < 10) ? ("0" + num5) : num5.ToString());
			}
		}

		public double Duration => (double)videoDuration / 10000000.0;

		public string DurationAsTimeString
		{
			get
			{
				double num = (double)videoDuration / 10000000.0;
				double num2 = num / 60.0;
				int num3 = (int)Math.Floor(num2 / 60.0);
				int num4 = (int)Math.Floor(num2 - (double)(num3 * 60));
				int num5 = (int)Math.Floor(num - (double)(num4 * 60));
				return ((num3 < 10) ? ("0" + num3) : num3.ToString()) + ":" + ((num4 < 10) ? ("0" + num4) : num4.ToString()) + ":" + ((num5 < 10) ? ("0" + num5) : num5.ToString());
			}
		}

		public VideoState CurrentState
		{
			get
			{
				return currentState;
			}
			set
			{
				switch (value)
				{
				case VideoState.Playing:
					Play();
					break;
				case VideoState.Paused:
					Pause();
					break;
				case VideoState.Stopped:
					Stop();
					break;
				}
			}
		}

		public IntPtr TextureSrv => m_srv.NativePointer;

		public bool IsDisposed => isDisposed;

		public int FramesPerSecond
		{
			get
			{
				if (avgTimePerFrame == 0L)
				{
					return -1;
				}
				float num = (float)avgTimePerFrame / 1E+07f;
				return (int)Math.Round(1f / num, 0, MidpointRounding.ToEven);
			}
		}

		public float MillisecondsPerFrame
		{
			get
			{
				if (avgTimePerFrame == 0L)
				{
					return -1f;
				}
				return (float)avgTimePerFrame / 10000f;
			}
		}

		public byte AlphaTransparency
		{
			get
			{
				return alphaTransparency;
			}
			set
			{
				alphaTransparency = value;
			}
		}

		public float Volume
		{
			get
			{
				m_basicAudio.get_Volume(out var plVolume);
				return (float)plVolume / 10000f + 1f;
			}
			set
			{
				m_basicAudio.put_Volume((int)((value - 1f) * 10000f));
			}
		}

		private static void checkHR(int hr, string msg, bool throwException = true)
		{
			if (hr < 0)
			{
				Console.Write("\n" + hr + "  " + msg + "\n");
				if (throwException)
				{
					DsError.ThrowExceptionForHR(hr);
				}
			}
		}

		public void Init(string FileName)
		{
			try
			{
				currentState = VideoState.Stopped;
				m_graphBuilder = (IGraphBuilder)new FilterGraph();
				BuildGraph(m_graphBuilder, FileName);
				m_mediaControl = (IMediaControl)m_graphBuilder;
				m_mediaEvent = (IMediaEventEx)m_graphBuilder;
				m_mediaSeeking = (IMediaSeeking)m_graphBuilder;
				m_mediaPosition = (IMediaPosition)m_graphBuilder;
				m_basicAudio = (IBasicAudio)m_graphBuilder;
				m_mediaSeeking.GetDuration(out videoDuration);
				InitTextures();
			}
			catch (Exception innerException)
			{
				throw new Exception("Unable to Load or Play the video file", innerException);
			}
		}

		private void InitTextures()
		{
			Texture2DDescription texture2DDescription = default(Texture2DDescription);
			texture2DDescription.Width = VideoWidth;
			texture2DDescription.Height = VideoHeight;
			texture2DDescription.Format = Format.B8G8R8A8_UNorm_SRgb;
			texture2DDescription.ArraySize = 1;
			texture2DDescription.MipLevels = 1;
			texture2DDescription.BindFlags = BindFlags.ShaderResource;
			texture2DDescription.Usage = ResourceUsage.Dynamic;
			texture2DDescription.CpuAccessFlags = CpuAccessFlags.Write;
			texture2DDescription.SampleDescription.Count = 1;
			texture2DDescription.SampleDescription.Quality = 0;
			texture2DDescription.OptionFlags = ResourceOptionFlags.None;
			Texture2DDescription description = texture2DDescription;
			m_texture = new Texture2D(MyPlatformRender.DeviceInstance, description);
			ShaderResourceViewDescription shaderResourceViewDescription = default(ShaderResourceViewDescription);
			shaderResourceViewDescription.Format = Format.B8G8R8A8_UNorm_SRgb;
			shaderResourceViewDescription.Dimension = ShaderResourceViewDimension.Texture2D;
			shaderResourceViewDescription.Texture2D.MipLevels = 1;
			shaderResourceViewDescription.Texture2D.MostDetailedMip = 0;
			ShaderResourceViewDescription description2 = shaderResourceViewDescription;
			m_srv = new ShaderResourceView(MyPlatformRender.DeviceInstance, m_texture, description2);
			string text3 = (m_texture.DebugName = (m_srv.DebugName = "MyVideoPlayer.Texture"));
		}

		private void BuildGraph(IGraphBuilder pGraph, string filename)
		{
			int num = 0;
			ICaptureGraphBuilder2 obj = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
			num = obj.SetFiltergraph(pGraph);
			checkHR(num, "Can't SetFilterGraph");
			num = pGraph.AddSourceFilter(filename, filename, out var ppFilter);
			checkHR(num, "Can't add source filter to graph");
			IBaseFilter baseFilter = (IBaseFilter)new WMAsfReader();
			num = pGraph.AddFilter(baseFilter, "WM ASF Reader");
			checkHR(num, "Can't add WM ASF Reader to graph");
			IFileSourceFilter obj2 = baseFilter as IFileSourceFilter;
			if (obj2 == null)
			{
				checkHR(-2147467262, "Can't get IFileSourceFilter");
			}
			num = obj2.Load(filename, null);
			checkHR(num, "Can't load file");
			IBaseFilter baseFilter2 = (IBaseFilter)new DMOWrapperFilter();
			IDMOWrapperFilter obj3 = baseFilter2 as IDMOWrapperFilter;
			if (obj3 == null)
			{
				checkHR(-2147467262, "Can't get WMVidero Decoder DMO");
			}
			num = obj3.Init(Clsid.WMVideoDecoderDMO, Clsid.WMVideoDecoderDMO_cat);
			checkHR(num, "DMO wrapper init failed");
			num = pGraph.AddFilter(baseFilter2, "WMVideo Decoder DMO");
			checkHR(num, "Can't add WMVideo Decoder DMO to graph");
			num = pGraph.ConnectDirect(GetPin(baseFilter, "Raw Video"), GetPin(baseFilter2, "in0"), null);
			checkHR(num, "Can't connect WM ASF Reader and WMVideo Decoder DMO");
			IBaseFilter baseFilter3 = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(Clsid.SampleGrabber));
			num = pGraph.AddFilter(baseFilter3, "Sample Grabber");
			checkHR(num, "Can't add Sample Grabber to graph");
			((IVideoWindow)m_graphBuilder).put_AutoShow(0);
			AMMediaType mediaType = new AMMediaType
			{
				majorType = MEDIATYPE_Video,
				subType = MediaSubType.RGB32,
				formatType = FORMAT_VideoInfo
			};
			num = ((ISampleGrabber)baseFilter3).SetMediaType(mediaType);
			num = ((ISampleGrabber)baseFilter3).SetBufferSamples(BufferThem: true);
			checkHR(num, "Can't set buffer samples");
			num = ((ISampleGrabber)baseFilter3).SetOneShot(OneShot: false);
			checkHR(num, "Can't set One shot false");
			num = ((ISampleGrabber)baseFilter3).SetCallback(this, 1);
			checkHR(num, "Can't set callback");
			num = pGraph.ConnectDirect(GetPin(baseFilter2, "out0"), GetPin(baseFilter3, "Input"), null);
			checkHR(num, "Can't connect WMVideo Decoder DMO to Sample Grabber");
			AMMediaType aMMediaType = new AMMediaType();
			num = ((ISampleGrabber)baseFilter3).GetConnectedMediaType(aMMediaType);
			checkHR(num, "Can't get media type");
			VideoInfoHeader videoInfoHeader = new VideoInfoHeader();
			Marshal.PtrToStructure(aMMediaType.formatPtr, videoInfoHeader);
			IBaseFilter baseFilter4 = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(Clsid.NullRenderer));
			num = pGraph.AddFilter(baseFilter4, "Null Renderer");
			checkHR(num, "Can't add Null Renderer to graph");
			num = obj.RenderStream(null, MEDIATYPE_Audio, ppFilter, null, null);
			if (num != -2147467259 || num == 0)
			{
				checkHR(num, "Can't add Audio Renderer to graph", throwException: false);
			}
			num = pGraph.ConnectDirect(GetPin(baseFilter3, "Output"), GetPin(baseFilter4, "In"), null);
			checkHR(num, "Can't connect Sample grabber to Null renderer");
			videoHeight = videoInfoHeader.BmiHeader.Height;
			videoWidth = videoInfoHeader.BmiHeader.Width;
			avgTimePerFrame = videoInfoHeader.AvgTimePerFrame;
			bitRate = videoInfoHeader.BitRate;
			m_videoDataRgba = new MySwapQueue<byte[]>(() => new byte[videoHeight * videoWidth * 4]);
		}

		private static IPin GetPin(IBaseFilter filter, string pinname)
		{
			checkHR(filter.EnumPins(out var ppEnum), "Can't enumerate pins");
			IntPtr pcFetched = Marshal.AllocCoTaskMem(4);
			IPin[] array = new IPin[1];
			while (ppEnum.Next(1, array, pcFetched) == 0)
			{
				array[0].QueryPinInfo(out var pInfo);
				bool num = pInfo.name.Contains(pinname);
				DsUtils.FreePinInfo(pInfo);
				if (num)
				{
					return array[0];
				}
			}
			checkHR(-1, pinname + "  Pin not found \n");
			return null;
		}

		private void CloseInterfaces()
		{
			if (m_mediaEvent != null)
			{
				m_mediaControl.Stop();
				m_mediaEvent.SetNotifyWindow(IntPtr.Zero, 32769, IntPtr.Zero);
			}
			m_mediaControl = null;
			m_mediaEvent = null;
			m_graphBuilder = null;
			m_mediaSeeking = null;
			m_mediaPosition = null;
			m_basicAudio = null;
			if (m_comObject != null)
			{
				Marshal.ReleaseComObject(m_comObject);
			}
			m_comObject = null;
		}

		private void OnFrame(DeviceContext context, byte[] frameData)
		{
			DataBox dataBox = context.MapSubresource(m_texture, 0, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None);
			if (!dataBox.IsEmpty)
			{
				int num = Format.B8G8R8A8_UNorm_SRgb.SizeOfInBytes() * VideoWidth;
				int num2 = num * (VideoHeight - 1);
				IntPtr dataPointer = dataBox.DataPointer;
				for (int i = 0; i < VideoHeight; i++)
				{
					Utilities.Write(dataPointer, frameData, num2, num);
					dataPointer += dataBox.RowPitch;
					num2 -= num;
				}
				context.UnmapSubresource(m_texture, 0);
			}
		}

		public void Update(object context)
		{
			byte[] array = null;
			if (m_videoDataRgba.RefreshRead())
			{
				array = m_videoDataRgba.Read;
			}
			m_mediaSeeking.GetCurrentPosition(out currentPosition);
			if (currentPosition >= videoDuration)
			{
				currentState = VideoState.Stopped;
			}
			if (array != null)
			{
				OnFrame(context as DeviceContext, array);
			}
		}

		public void Play()
		{
			if (currentState != 0)
			{
				checkHR(m_mediaControl.Run(), "Can't run the graph");
				currentState = VideoState.Playing;
			}
		}

		public void Pause()
		{
			m_mediaControl.Stop();
			currentState = VideoState.Paused;
		}

		public void Stop()
		{
			m_mediaControl.Stop();
			m_mediaSeeking.SetPositions(new DsOptInt64(0L), SeekingFlags.AbsolutePositioning, new DsOptInt64(0L), SeekingFlags.NoPositioning);
			currentState = VideoState.Stopped;
		}

		public void Rewind()
		{
			Stop();
			Play();
		}

		public int BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
		{
			byte[] write = m_videoDataRgba.Write;
			byte b = alphaTransparency;
			Marshal.Copy(pBuffer, write, 0, BufferLen);
			for (int i = 3; i < BufferLen; i += 4)
			{
				write[i] = b;
			}
			m_videoDataRgba.CommitWrite();
			return 0;
		}

		public int SampleCB(double SampleTime, IMediaSample pSample)
		{
			return 0;
		}

		public virtual void Dispose()
		{
			isDisposed = true;
			Stop();
			CloseInterfaces();
			m_videoDataRgba = null;
			m_srv.Dispose();
			m_texture.Dispose();
		}
	}
}
