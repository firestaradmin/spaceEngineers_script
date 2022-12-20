using System;
using SharpDX.Direct3D11;
using VRage.Platform.Windows.Forms;
using VRage.Utils;
using VRageRender;

namespace VRage.Platform.Windows.Render
{
	internal sealed class MyWindowsRender : IVRageRender
	{
		private readonly ulong RWTEXTURES_VRAM_POOL = 104857600uL;

		private readonly MyWindowsWindows m_windows;

		private ulong m_streamedResourcesMemoryBudget;

		private ulong m_generatedTexturesMemoryBudget;

		private ulong m_voxelTextureArraysMemoryBudget;

		public bool UseParallelRenderInit => false;

		public bool IsRenderOutputDebugSupported => true;

		public event Action OnResuming;

		public event Action OnSuspending;

		public MyWindowsRender(MyLog log, MyWindowsWindows windows)
		{
			MyPlatformRender.Log = log;
			m_windows = windows;
		}

		public void CreateRenderDevice(ref MyRenderDeviceSettings? settings, out object deviceInstance, out object swapChain)
		{
			MyPlatformRender.CreateRenderDevice(ref settings, m_windows.WindowHandle, out deviceInstance, out swapChain);
			m_windows.GameWindow?.OnModeChanged(settings.Value.WindowMode, settings.Value.BackBufferWidth, settings.Value.BackBufferHeight, MyPlatformRender.GetAdaptersList()[settings.Value.AdapterOrdinal].DesktopBounds);
			m_streamedResourcesMemoryBudget = MyVRage.Platform.System.GetTotalPhysicalMemory() / 5uL;
			m_generatedTexturesMemoryBudget = MyVRage.Platform.System.GetTotalPhysicalMemory() / 32uL;
			m_voxelTextureArraysMemoryBudget = MyVRage.Platform.System.GetTotalPhysicalMemory() / 10uL;
		}

		public void DisposeRenderDevice()
		{
			MyPlatformRender.DisposeRenderDevice();
		}

		public void SuspendRenderContext()
		{
		}

		public void ResumeRenderContext()
		{
		}

		public MyRenderPresetEnum GetRenderQualityHint()
		{
			return MyRenderPresetEnum.NORMAL;
		}

		public MyAdapterInfo[] GetRenderAdapterList()
		{
			return MyPlatformRender.GetAdaptersList();
		}

		public void ApplyRenderSettings(MyRenderDeviceSettings? settings)
		{
			MyPlatformRender.ApplySettings(settings);
			if (settings.HasValue)
			{
				m_windows.GameWindow?.OnModeChanged(settings.Value.WindowMode, settings.Value.BackBufferWidth, settings.Value.BackBufferHeight, MyPlatformRender.GetAdaptersList()[settings.Value.AdapterOrdinal].DesktopBounds);
				ulong vRAM = MyPlatformRender.GetAdaptersList()[settings.Value.AdapterOrdinal].VRAM;
				ulong totalPhysicalMemory = MyVRage.Platform.System.GetTotalPhysicalMemory();
				m_voxelTextureArraysMemoryBudget = (MyRenderProxy.Settings.VoxelTexturesStreamingPool = (ulong)Math.Min((double)vRAM * 0.4, (float)totalPhysicalMemory / 10f));
				vRAM -= MyRenderProxy.Settings.VoxelTexturesStreamingPool;
				vRAM -= RWTEXTURES_VRAM_POOL;
				m_streamedResourcesMemoryBudget = (MyRenderProxy.Settings.ResourceStreamingPool = Math.Min(vRAM, totalPhysicalMemory / 2uL));
			}
		}

		public object CreateRenderAnnotation(object deviceContext)
		{
			try
			{
				return ((DeviceContext)deviceContext).QueryInterface<UserDefinedAnnotation>();
			}
			catch (Exception)
			{
				MyRenderProxy.Log.WriteLine("Warning: Annotations for render context are not available");
			}
			return null;
		}

		public ulong GetMemoryBudgetForStreamedResources()
		{
			return m_streamedResourcesMemoryBudget;
		}

		public ulong GetMemoryBudgetForGeneratedTextures()
		{
			return m_generatedTexturesMemoryBudget;
		}

		public ulong GetMemoryBudgetForVoxelTextureArrays()
		{
			return m_voxelTextureArraysMemoryBudget;
		}

		public void RequestSuspendWait()
		{
		}

		public void SetMemoryUsedForImprovedGFX(long bytes)
		{
		}

		public void FlushIndirectArgsFromComputeShader(object deviceContext)
		{
		}
	}
}
