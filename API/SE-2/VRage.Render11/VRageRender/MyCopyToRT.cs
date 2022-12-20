using VRage.Render11.Common;
using VRage.Render11.Resources;

namespace VRageRender
{
	internal class MyCopyToRT : MyImmediateRC
	{
		private static MyPixelShaders.Id m_copyPs;

		private static MyPixelShaders.Id m_copyFilterPs;

		private static MyPixelShaders.Id m_clearAlphaPs;

		internal static MyPixelShaders.Id CopyPs => m_copyPs;

		internal static void Init()
		{
			m_copyPs = MyPixelShaders.Create("Postprocess/PostprocessCopy.hlsl");
			m_copyFilterPs = MyPixelShaders.Create("Postprocess/PostprocessCopyFilter.hlsl");
			m_clearAlphaPs = MyPixelShaders.Create("Postprocess/PostprocessClearAlpha.hlsl");
		}

		internal static void Run(IRtvBindable destination, ISrvBindable source, bool alphaBlended = false, MyViewport? customViewport = null)
		{
			if (alphaBlended)
			{
				MyImmediateRC.RC.SetBlendState(MyBlendStateManager.BlendAlphaPremult);
			}
			else
			{
				MyImmediateRC.RC.SetBlendState(null);
			}
			MyImmediateRC.RC.SetInputLayout(null);
			if (source.Size != destination.Size)
			{
				MyImmediateRC.RC.PixelShader.Set(m_copyFilterPs);
			}
			else
			{
				MyImmediateRC.RC.PixelShader.Set(m_copyPs);
			}
			MyImmediateRC.RC.SetRtv(destination);
			MyImmediateRC.RC.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
			MyImmediateRC.RC.PixelShader.SetSrv(0, source);
			MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC, customViewport ?? new MyViewport(destination.Size.X, destination.Size.Y));
		}
	}
}
