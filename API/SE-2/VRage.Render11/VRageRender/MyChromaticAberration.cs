using SharpDX.Direct3D;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	internal static class MyChromaticAberration
	{
		private static int m_lastIterations = 4;

		private static MyComputeShaders.Id m_cs;

		private const int NUMTHREADS = 8;

		internal static void Init()
		{
			m_cs = MyComputeShaders.Create("Postprocess/ChromaticAberration/ChromaticAberration.hlsl", new ShaderMacro[2]
			{
				new ShaderMacro("NUMTHREADS", 8),
				new ShaderMacro("ITERATIONS", m_lastIterations)
			});
		}

		internal static void Run(IUavBindable dst, ISrvTexture src)
		{
			if (m_lastIterations != MyRender11.Postprocess.ChromaticIterations)
			{
				m_lastIterations = MyRender11.Postprocess.ChromaticIterations;
				Init();
			}
			MyRenderContext rC = MyRender11.RC;
			rC.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			rC.ComputeShader.SetUav(0, dst);
			rC.ComputeShader.SetSrv(0, src);
			rC.ComputeShader.SetSampler(0, MySamplerStateManager.Default);
			rC.ComputeShader.Set(m_cs);
			Vector2I size = dst.Size;
			rC.Dispatch((size.X + 8 - 1) / 8, (size.Y + 8 - 1) / 8, 1);
			rC.ComputeShader.SetUav(0, null);
			rC.ComputeShader.Set(null);
		}
	}
}
