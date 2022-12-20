using System;
using VRageRender;

namespace VRage
{
	public interface IVRageRender
	{
		bool UseParallelRenderInit { get; }

		bool IsRenderOutputDebugSupported { get; }

		event Action OnResuming;

		event Action OnSuspending;

		void CreateRenderDevice(ref MyRenderDeviceSettings? settings, out object deviceInstance, out object swapChain);

		void DisposeRenderDevice();

		void SuspendRenderContext();

		void ResumeRenderContext();

		MyRenderPresetEnum GetRenderQualityHint();

		MyAdapterInfo[] GetRenderAdapterList();

		void ApplyRenderSettings(MyRenderDeviceSettings? settings);

		object CreateRenderAnnotation(object deviceContext);

		ulong GetMemoryBudgetForStreamedResources();

		/// <summary>
		/// Request the rendering system to wait after suspend for as long as it can for any last minute asynchronous operations to complete.
		/// </summary>
		void RequestSuspendWait();

		void SetMemoryUsedForImprovedGFX(long bytes);

		void FlushIndirectArgsFromComputeShader(object deviceContext);

		ulong GetMemoryBudgetForGeneratedTextures();

		ulong GetMemoryBudgetForVoxelTextureArrays();
	}
}
