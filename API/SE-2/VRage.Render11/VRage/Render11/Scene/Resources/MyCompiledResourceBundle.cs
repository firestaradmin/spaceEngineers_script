using System;
using VRage.Render11.Resources.Textures;

namespace VRage.Render11.Scene.Resources
{
	internal struct MyCompiledResourceBundle : IDisposable
	{
		private MyTextureStreamingManager.CompiledBatch m_textures;

		public MyCompiledResourceBundle(MyTextureStreamingManager.CompiledBatch textures)
		{
			m_textures = textures;
			m_textures.RegisterUsagePriority();
		}

		public void UpdateScenePriority(int distancePriority)
		{
			m_textures.UpdateScenePriority(distancePriority);
		}

		public void Dispose()
		{
			UpdateScenePriority(0);
			m_textures.UnregisterUsagePriority();
			m_textures.Dispose();
		}
	}
}
