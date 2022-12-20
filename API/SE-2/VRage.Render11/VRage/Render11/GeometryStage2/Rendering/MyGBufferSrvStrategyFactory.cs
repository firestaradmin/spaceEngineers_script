using VRageRender;

namespace VRage.Render11.GeometryStage2.Rendering
{
	internal static class MyGBufferSrvStrategyFactory
	{
		private static readonly IGBufferSrvStrategy m_standardStrategy = new MyStandardGBufferSrvStrategy();

		private static readonly IGBufferSrvStrategy m_lodStrategy = new MyLodGBufferSrvStrategy();

		private static readonly IGBufferSrvStrategy m_mipmapStrategy = new MyMipmapGBufferSrvStrategy();

		public static IGBufferSrvStrategy GetStrategy()
		{
			if (MyRender11.Settings.DisplayGbufferLOD)
			{
				return m_lodStrategy;
			}
			if (MyRender11.Settings.DisplayMipmap)
			{
				return m_mipmapStrategy;
			}
			return m_standardStrategy;
		}
	}
}
