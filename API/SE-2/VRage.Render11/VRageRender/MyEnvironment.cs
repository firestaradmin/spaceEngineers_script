using VRageRender.Messages;

namespace VRageRender
{
	internal class MyEnvironment
	{
		internal readonly MyEnvironmentMatrices Matrices = new MyEnvironmentMatrices();

		internal MyEnvironmentData Data;

		internal string SkyboxIndirect;

		internal MyRenderFogSettings Fog;

		public MyRenderPlanetSettings Planet;
	}
}
