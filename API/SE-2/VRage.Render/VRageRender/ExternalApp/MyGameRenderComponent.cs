using System;
using VRage;
using VRage.Library.Utils;

namespace VRageRender.ExternalApp
{
	public class MyGameRenderComponent : IDisposable
	{
		public MyRenderThread RenderThread { get; private set; }

		/// <summary>
		/// Creates and starts render thread
		/// </summary>
		public void Start(MyGameTimer timer, InitHandler windowInitializer, MyRenderDeviceSettings? settingsToTry, float maxFrameRate)
		{
			RenderThread = MyRenderThread.Start(timer, windowInitializer, settingsToTry, maxFrameRate);
			MyVRage.Platform.Render.OnSuspending += delegate
			{
				RenderThread.Suspend = true;
			};
			MyVRage.Platform.Render.OnResuming += delegate
			{
				RenderThread.Suspend = false;
			};
		}

		/// <summary>
		/// Stops and clears render thread
		/// </summary>
		public void Stop()
		{
			RenderThread.Exit();
			RenderThread = null;
		}

		public void StartSync(MyGameTimer timer, IVRageWindow window, MyRenderDeviceSettings? settings, float maxFrameRate)
		{
			RenderThread = MyRenderThread.StartSync(timer, window, settings, maxFrameRate);
			MyVRage.Platform.Render.OnSuspending += delegate
			{
				RenderThread.Suspend = true;
			};
			MyVRage.Platform.Render.OnResuming += delegate
			{
				RenderThread.Suspend = false;
			};
		}

		public void Dispose()
		{
			if (RenderThread != null)
			{
				Stop();
			}
			MyRenderProxy.DisposeDevice();
		}
	}
}
