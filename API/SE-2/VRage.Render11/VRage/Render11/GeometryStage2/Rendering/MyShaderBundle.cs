using VRageRender;

namespace VRage.Render11.GeometryStage2.Rendering
{
	internal class MyShaderBundle
	{
		public MyPixelShaders.Id PixelShader { get; private set; }

		public MyVertexShaders.Id VertexShader { get; private set; }

		public MyInputLayouts.Id InputLayout { get; private set; }

		public void Init(MyPixelShaders.Id ps, MyVertexShaders.Id vs, MyInputLayouts.Id il)
		{
			PixelShader = ps;
			VertexShader = vs;
			InputLayout = il;
		}
	}
}
