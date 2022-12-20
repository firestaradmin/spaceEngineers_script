#define VRAGE
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Library.Filesystem;
using VRage.Library.Utils;
using VRage.Profiler;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRage.Render11.Shader;
using VRage.Render11.Tools;
using VRage.Stats;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;
using VRageRender.Utils;

namespace VRageRender
{
	public class MyDX11Render : IMyRender
	{
		public FrameProcessStatusEnum FrameProcessStatus => MyRender11.FrameProcessStatus;

		public string RootDirectory
		{
			get
			{
				return MyRender11.RootDirectory;
			}
			set
			{
				MyRender11.RootDirectory = value;
			}
		}

		public string RootDirectoryEffects
		{
			get
			{
				return MyRender11.RootDirectoryEffects;
			}
			set
			{
				MyRender11.RootDirectoryEffects = value;
			}
		}

		public string RootDirectoryDebug
		{
			get
			{
				return MyRender11.RootDirectoryDebug;
			}
			set
			{
				MyRender11.RootDirectoryDebug = value;
			}
		}

		public MyLog Log => MyRender11.Log;

		public MySharedData SharedData => MyRender11.SharedData;

		public MyTimeSpan CurrentDrawTime
		{
			get
			{
				return MyRender11.CurrentDrawTime;
			}
			set
			{
				MyRender11.PreviousDrawTime = MyRender11.CurrentDrawTime;
				MyRender11.CurrentDrawTime = value;
			}
		}

		public Vector2I BackBufferResolution => MyRender11.BackBufferResolution;

		public MyViewport MainViewport
		{
			get
			{
				Vector2I backBufferResolution = BackBufferResolution;
				return new MyViewport(backBufferResolution.X, backBufferResolution.Y);
			}
		}

		public MyMessageQueue OutputQueue => MyRender11.OutputQueue;

		public uint GlobalMessageCounter
		{
			get
			{
				return MyRender11.GlobalMessageCounter;
			}
			set
			{
				MyRender11.GlobalMessageCounter = value;
			}
		}

		public bool IsSupported
		{
			get
			{
				try
				{
					MyAdapterInfo[] adaptersList = MyRender11.GetAdaptersList();
					for (int i = 0; i < adaptersList.Length; i++)
					{
						if (adaptersList[i].IsDx11Supported)
						{
							return true;
						}
					}
					return false;
				}
				catch (Exception)
				{
					return false;
				}
			}
		}

		public bool MessageProcessingSupported => true;

		public MyDX11Render(MyRenderSettings? initialRenderSettings = null)
		{
			if (initialRenderSettings.HasValue)
			{
				MyRender11.Settings = initialRenderSettings.Value;
			}
			if (MyRender11.Settings.EnableAnsel)
			{
				MyManagers.Ansel.EnableAnsel();
			}
			MyRender11.LogUpdateRenderSettings();
		}

		public MyRenderDeviceSettings CreateDevice(MyRenderDeviceSettings? settings, out MyAdapterInfo[] adaptersList)
		{
			MyRenderDeviceSettings result = MyRender11.CreateDevice(settings);
			adaptersList = (MyAdapterInfo[])MyRender11.GetAdaptersList().Clone();
			return result;
		}

		public void DisposeDevice()
		{
			MyRender11.DisposeDevice();
		}

		public long GetAvailableTextureMemory()
		{
			return MyRender11.GetAvailableTextureMemory();
		}

		public bool SettingsChanged(MyRenderDeviceSettings settings)
		{
			return MyRender11.SettingsChanged(settings);
		}

		public void ApplySettings(MyRenderDeviceSettings settings)
		{
			MyRender11.ApplySettings(settings);
		}

		public void Present()
		{
			MyRender11.Present();
		}

		public void EnqueueMessage(MyRenderMessageBase message, bool limitMaxQueueSize)
		{
			MyRender11.EnqueueMessage(message);
		}

		public void EnqueueOutputMessage(MyRenderMessageBase message)
		{
			MyRender11.EnqueueOutputMessage(message);
		}

		public void Draw(bool draw = true)
		{
			MyRender11.Draw(draw);
		}

		public void Ansel_DrawScene()
		{
			MyRender11.Postprocess.Data.ChromaticFactor = 0f;
			MyRender11.Postprocess.Data.VignetteStart = 0f;
			MyRender11.Postprocess.Data.VignetteLength = 1f;
			MyRender11.Postprocess.Data.EyeAdaptationSpeedDown = 0f;
			MyRender11.Postprocess.Data.EyeAdaptationSpeedUp = 0f;
			MyRender11.Postprocess.Data.BloomDirtRatio = 0f;
			MyManagers.Ansel.BeginDrawScene();
			MyRender11.DrawScene();
			if (!MyManagers.Ansel.Is360Capturing)
			{
				Vector2 size = MyRender11.ViewportResolution;
				MyViewport myViewport = new MyViewport(MyRender11.ViewportResolution);
				MyViewport spriteViewport = MyManagers.Ansel.GetSpriteViewport();
				MyViewport viewportBound = MyRender11.ScaleMainViewport(myViewport);
				MyRender11.RenderMainSprites(MyRender11.Backbuffer, viewportBound, myViewport, size, spriteViewport);
				MyRender11.ConsumeMainSprites();
			}
			MyManagers.Ansel.EndDrawScene();
		}

		public MyRenderProfiler GetRenderProfiler()
		{
			return MyRender11.GetRenderProfiler();
		}

		public bool IsVideoValid(uint id)
		{
			return MyVideoFactory.GetVideo(id) != null;
		}

		public VideoState GetVideoState(uint id)
		{
			return MyVideoFactory.GetVideo(id)?.CurrentState ?? VideoState.Stopped;
		}

		public void GenerateShaderCache(bool clean, bool fastBuild, OnShaderCacheProgressDelegate onShaderCacheProgress)
		{
			MyShaderCacheGenerator.Generate(clean, fastBuild, onShaderCacheProgress);
		}

		public string GetLastExecutedAnnotation()
		{
			string marker = "";
			MyRender11.RC.GetLastAnnotation(ref marker);
			return $"Main({marker}) Deferred({MyManagers.DeferredRCs.GetLastAnnotations()})";
		}

		public string GetStatistics()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (List<MyStats> value in MyRenderStats.m_stats.Values)
			{
				foreach (MyStats item in value)
				{
					item.WriteTo(stringBuilder);
				}
			}
			MyRendererStats.UpdateStats();
			MyStatsDisplay.WriteTo(stringBuilder);
			return stringBuilder.ToString();
		}

		public void SetTimings(MyTimeSpan cpuDraw, MyTimeSpan cpuWait)
		{
			MyRender11.CPUDraw = cpuDraw;
			MyRender11.CPUWait = cpuWait;
		}

		public Vector2 GetTextureSize(string name)
		{
			if (ContentIndex.TryGetImageSize(name, out var width, out var height))
			{
				return new Vector2(width, height);
			}
			return MyFileTextureParamsManager.GetResolutionFromFile(name);
		}

		public bool IsTextureLoaded(string name)
		{
			if (MyManagers.FileTextures.TryGetTexture(name, out ITexture texture))
			{
				return texture.IsTextureLoaded();
			}
			return false;
		}

		public static void GenerateDx11MipCache()
		{
			IEnumerable<string> files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Textures"));
<<<<<<< HEAD
			MyManagers.FileTextures.GenerateMipCacheOffline(files.Select((string x) => (x, MyFileTextureEnum.COLOR_METAL)));
=======
			MyManagers.FileTextures.GenerateMipCacheOffline(Enumerable.Select<string, (string, MyFileTextureEnum)>(files, (Func<string, (string, MyFileTextureEnum)>)((string x) => (x, MyFileTextureEnum.COLOR_METAL))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
