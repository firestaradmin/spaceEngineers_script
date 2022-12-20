using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using VRage.FileSystem;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender;

namespace VRage.Render11.LightingStage.EnvironmentProbe
{
	internal class MyEnvProbeProcessing
	{
		private static MyPixelShaders.Id m_ps;

		private static MyPixelShaders.Id m_blend;

		private static MyPixelShaders.Id m_copy;

		private static MyPixelShaders.Id m_prefilter;

		private static IConstantBuffer m_transformConstants;

		private static readonly RenderTargetView[] m_rtmp2 = new RenderTargetView[6];

		internal unsafe static void Init()
		{
			m_ps = MyPixelShaders.Create("EnvProbe/ForwardPostprocess.hlsl");
			m_blend = MyPixelShaders.Create("EnvProbe/EnvProbeBlend.hlsl");
			m_copy = MyPixelShaders.Create("EnvProbe/EnvProbeCopy.hlsl");
			m_prefilter = MyPixelShaders.Create("EnvProbe/EnvPrefiltering.hlsl");
			m_transformConstants = MyManagers.Buffers.CreateConstantBuffer("TransformConstants", sizeof(Matrix) * 2 + sizeof(Vector4), null, ResourceUsage.Dynamic, isGlobal: true);
		}

		internal static void RunForwardPostprocess(MyRenderContext rc, IRtvBindable rt, ISrvBindable depthSrv, ref Matrix viewMatrix, ref Matrix projMatrix)
		{
			Matrix data = Matrix.Transpose(viewMatrix);
			Matrix data2 = Matrix.Transpose(projMatrix);
			MyMapping myMapping = MyMapping.MapDiscard(rc, m_transformConstants);
			myMapping.WriteAndPosition(ref data);
			myMapping.WriteAndPosition(ref data2);
			myMapping.WriteAndPosition(ref MyRender11.Environment.Data.EnvironmentLight.SunLightDirection);
			float data3 = rt.Size.X;
			myMapping.WriteAndPosition(ref data3);
			myMapping.Unmap();
			rc.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstants);
			rc.AllShaderStages.SetConstantBuffer(2, m_transformConstants);
			rc.SetBlendState(null);
			rc.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
			rc.SetRtv(rt);
			rc.PixelShader.SetSrv(0, depthSrv);
			string name = (MyFileSystem.FileExists(MyRender11.Environment.SkyboxIndirect) ? MyRender11.Environment.SkyboxIndirect : MyRender11.Environment.Data.Skybox);
			rc.PixelShader.SetSrv(10, MyManagers.Textures.GetTempTexture(name, MyFileTextureEnum.CUBEMAP, ushort.MaxValue));
			rc.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			rc.PixelShader.Set(m_ps);
			MyScreenPass.DrawFullscreenQuad(rc, new MyViewport(rt.Size));
			rc.SetRtvNull();
			rc.PixelShader.SetSrv(0, null);
		}

		internal static void Prefilter(MyRenderContext rc, int nProbe, int skipMipLevels, IArrayTexture probe, IRtvArrayTexture prefiltered)
		{
			rc.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
			rc.PixelShader.Set(m_prefilter);
			rc.PixelShader.SetSrv(1, probe);
			rc.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			int mipLevels = prefiltered.MipLevels;
			int x = prefiltered.Size.X;
			uint data = (uint)probe.Size.X;
			IConstantBuffer objectCB = rc.GetObjectCB(32);
			rc.PixelShader.SetConstantBuffer(1, objectCB);
			rc.SetBlendState(null);
			int num = x;
			mipLevels -= skipMipLevels;
			int mipSlice = skipMipLevels + 1;
			while (mipSlice-- > 0)
			{
				num >>= 1;
				int sourceSubresource = Resource.CalculateSubResourceIndex(mipSlice, nProbe, probe.MipLevels);
				int destinationSubResource = Resource.CalculateSubResourceIndex(mipSlice, nProbe, prefiltered.MipLevels);
				rc.CopySubresourceRegion(probe, sourceSubresource, null, prefiltered, destinationSubResource);
			}
			for (mipSlice = 1; mipSlice < mipLevels; mipSlice++)
			{
				uint data2 = 16u;
				uint data3 = (uint)num;
				uint data4 = (uint)nProbe;
				float data5 = 1f - (float)mipSlice / (float)(mipLevels - 1);
				MyMapping myMapping = MyMapping.MapDiscard(rc, objectCB);
				myMapping.WriteAndPosition(ref data2);
				myMapping.WriteAndPosition(ref data);
				myMapping.WriteAndPosition(ref data3);
				myMapping.WriteAndPosition(ref data4);
				myMapping.WriteAndPosition(ref data5);
				myMapping.Unmap();
				rc.SetRtv(prefiltered.SubresourceRtv(nProbe, mipSlice + skipMipLevels));
				MyScreenPass.DrawFullscreenQuad(rc, new MyViewport(num, num));
				num >>= 1;
			}
			rc.SetRtvNull();
			rc.PixelShader.SetSrv(1, null);
			rc.PixelShader.Set(null);
		}

		internal static void Blend(MyRenderContext rc, bool useFactor, IRtvArrayTexture dst, IArrayTexture src0, IArrayTexture src1, float blendStrength, float blendFactor)
		{
			if (!useFactor)
			{
				rc.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
				rc.PixelShader.Set(m_copy);
				rc.SetBlendState(MyBlendStateManager.BlendFactor, new RawColor4(blendStrength, blendStrength, blendStrength, blendStrength));
				int num = dst.Size.X;
				for (int i = 0; i < src0.MipLevels; i++)
				{
					for (int j = 0; j < 6; j++)
					{
						m_rtmp2[j] = dst.SubresourceRtv(j, i).Rtv;
					}
					rc.SetRtvs(m_rtmp2);
					ISrvBindable[] srvs = new ISrvBindable[6]
					{
						src0.SubresourceSrv(0, i),
						src0.SubresourceSrv(1, i),
						src0.SubresourceSrv(2, i),
						src0.SubresourceSrv(3, i),
						src0.SubresourceSrv(4, i),
						src0.SubresourceSrv(5, i)
					};
					rc.PixelShader.SetSrvs(0, srvs);
					MyScreenPass.DrawFullscreenQuad(rc, new MyViewport(num, num));
					num >>= 1;
				}
				rc.SetRtvNull();
			}
			else
			{
				if (!(blendFactor < 1f))
				{
					return;
				}
				rc.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
				rc.PixelShader.Set(m_blend);
				rc.SetBlendState(null);
				IConstantBuffer objectCB = rc.GetObjectCB(16);
				rc.PixelShader.SetConstantBuffer(1, objectCB);
				MyMapping myMapping = MyMapping.MapDiscard(rc, objectCB);
				myMapping.WriteAndPosition(ref blendFactor);
				myMapping.Unmap();
				int num2 = dst.Size.X;
				for (int k = 0; k < src1.MipLevels; k++)
				{
					for (int l = 0; l < 6; l++)
					{
						m_rtmp2[l] = dst.SubresourceRtv(l, k).Rtv;
					}
					rc.SetRtvs(m_rtmp2);
					ISrvBindable[] srvs2 = new ISrvBindable[12]
					{
						src0.SubresourceSrv(0, k),
						src0.SubresourceSrv(1, k),
						src0.SubresourceSrv(2, k),
						src0.SubresourceSrv(3, k),
						src0.SubresourceSrv(4, k),
						src0.SubresourceSrv(5, k),
						src1.SubresourceSrv(0, k),
						src1.SubresourceSrv(1, k),
						src1.SubresourceSrv(2, k),
						src1.SubresourceSrv(3, k),
						src1.SubresourceSrv(4, k),
						src1.SubresourceSrv(5, k)
					};
					rc.PixelShader.SetSrvs(0, srvs2);
					MyScreenPass.DrawFullscreenQuad(rc, new MyViewport(num2, num2));
					num2 >>= 1;
				}
				rc.SetRtvNull();
			}
		}
	}
}
