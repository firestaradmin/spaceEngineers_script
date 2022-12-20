using SharpDX.Direct3D;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRageMath;

namespace VRageRender
{
	internal class MyToneMapping : MyImmediateRC
	{
		private static MyComputeShaders.Id m_cs;

		private static MyComputeShaders.Id m_csAlphaLuminance;

		private static MyComputeShaders.Id m_csSkip;

		private const int m_numthreads = 8;

		internal static void Init()
		{
			m_cs = MyComputeShaders.Create("Postprocess/Tonemapping/Main.hlsl", new ShaderMacro[1]
			{
				new ShaderMacro("NUMTHREADS", 8)
			});
			m_csAlphaLuminance = MyComputeShaders.Create("Postprocess/Tonemapping/Main.hlsl", new ShaderMacro[2]
			{
				new ShaderMacro("NUMTHREADS", 8),
				new ShaderMacro("FILL_ALPHA_LUMINANCE", null)
			});
			m_csSkip = MyComputeShaders.Create("Postprocess/Tonemapping/Main.hlsl", new ShaderMacro[2]
			{
				new ShaderMacro("NUMTHREADS", 8),
				new ShaderMacro("DISABLE_TONEMAPPING", null)
			});
		}

		internal static IBorrowedCustomTexture Run(ISrvBindable src, ISrvBindable avgLum, ISrvBindable bloom, bool enableTonemapping, string dirtTexture, bool needsAlphaLuminance)
		{
			IBorrowedCustomTexture borrowedCustomTexture = MyManagers.RwTexturesPool.BorrowCustom("DrawGameScene.Tonemapped");
			MyImmediateRC.RC.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			MyImmediateRC.RC.ComputeShader.SetUav(0, borrowedCustomTexture);
			ITexture tempTexture = MyManagers.Textures.GetTempTexture(dirtTexture, new MyTextureStreamingManager.QueryArgs
			{
				TextureType = MyFileTextureEnum.ALPHAMASK,
				WaitUntilLoaded = true,
				SkipQualityReduction = true
			}, 100);
			MyImmediateRC.RC.ComputeShader.SetSrvs(0, src, avgLum, bloom, tempTexture);
			MyImmediateRC.RC.ComputeShader.SetSampler(0, MySamplerStateManager.Default);
			MyImmediateRC.RC.ComputeShader.SetSampler(1, MySamplerStateManager.Point);
			MyImmediateRC.RC.ComputeShader.SetSampler(2, MySamplerStateManager.Default);
			MyImmediateRC.RC.ComputeShader.SetSampler(3, MySamplerStateManager.Default);
			MyImmediateRC.RC.ComputeShader.Set((!enableTonemapping) ? m_csSkip : (needsAlphaLuminance ? m_csAlphaLuminance : m_cs));
			Vector2I size = borrowedCustomTexture.Size;
			MyImmediateRC.RC.Dispatch((size.X + 8 - 1) / 8, (size.Y + 8 - 1) / 8, 1);
			MyImmediateRC.RC.ComputeShader.SetUav(0, null);
			MyImmediateRC.RC.ComputeShader.Set(null);
			return borrowedCustomTexture;
		}
	}
}
