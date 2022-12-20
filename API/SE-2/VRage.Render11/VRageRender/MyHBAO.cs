using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Library.Utils;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender.Messages;

namespace VRageRender
{
	internal class MyHBAO
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct GlobalConstantBuffer
		{
			internal Vector2 InvQuarterResolution;

			internal Vector2 InvFullResolution;

			internal Vector2 UVToViewA;

			internal Vector2 UVToViewB;

			internal float RadiusToScreen;

			internal float R2;

			internal float NegInvR2;

			internal float NDotVBias;

			internal float SmallScaleAOAmount;

			internal float LargeScaleAOAmount;

			internal float PowExponent;

			private int _pad2;

			internal float BlurViewDepth0;

			internal float BlurViewDepth1;

			internal float BlurSharpness0;

			internal float BlurSharpness1;

			internal float LinearizeDepthA;

			internal float LinearizeDepthB;

			internal float InverseDepthRangeA;

			internal float InverseDepthRangeB;

			internal Vector2 InputViewportTopLeft;

			internal float ViewDepthThresholdNegInv;

			internal float ViewDepthThresholdSharpness;

			internal float BackgroundAORadiusPixels;

			internal float ForegroundAORadiusPixels;

			internal int DebugNormalComponent;

			private float _pad0;

			internal Matrix NormalMatrix;

			internal float NormalDecodeScale;

			internal float NormalDecodeBias;

			private Vector2 _pad1;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct PerPassConstantBuffer
		{
			internal Vector4 Jitter;

			internal Vector2 Offset;

			internal float SliceIndexFloat;

			internal uint SliceIndexInt;
		}

		private static MyHBAOData m_lastParams;

		private static readonly int GLOBALCONSTANTBUFFERSIZE;

		private static readonly int PERPASSCONSTANTBUFFERSIZE;

		private const int MAX_NUM_MRTS = 8;

		private const int NUM_DIRECTIONS = 8;

		private const int NUM_SLICES = 16;

		private static MyPixelShaders.Id m_linearizeDepthPS;

		private static MyPixelShaders.Id m_deinterleaveDepthPS;

		private static MyPixelShaders.Id m_reconstructNormalPS;

		private static MyPixelShaders.Id m_coarseAOPS;

		private static MyPixelShaders.Id m_copyPS;

		private static MyPixelShaders.Id m_reinterleaveAOPS;

		private static MyPixelShaders.Id m_reinterleaveAOPS_PreBlur;

		private static MyPixelShaders.Id m_blurXPS;

		private static MyPixelShaders.Id m_blurYPS;

		private static MyPixelShaders.Id m_drawNormalsPS;

		private static IConstantBuffer m_dataCB;

		private static IConstantBuffer[] m_perPassCBs;

		private static IRtvTexture m_fullResViewDepthTarget;

		private static IRtvTexture m_fullResNormalTexture;

		private static IRtvTexture m_fullResAOZTexture;

		private static IRtvTexture m_fullResAOZTexture2;

		private static IRtvArrayTexture m_quarterResViewDepthTextureArray;

		private static IRtvArrayTexture m_quarterResAOTextureArray;

		private static readonly RenderTargetView[] m_rtmp;

		private static readonly List<ShaderMacro> m_macros;

		private static float METERS_TO_VIEW_SPACE_UNITS;

		internal static MyHBAOData Params { get; set; }

		internal static void Run(MyRenderContext rc, IRtvTexture dst, MyGBuffer gbuffer, MyViewport? viewport = null)
		{
			CompilePS();
			if (!viewport.HasValue)
			{
				viewport = new MyViewport(0f, 0f, MyRender11.m_resolution.X, MyRender11.m_resolution.Y);
			}
			GlobalConstantBuffer data = InitConstantBuffer(viewport.Value);
			MyMapping myMapping = MyMapping.MapDiscard(rc, m_dataCB);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			rc.PixelShader.SetConstantBuffer(0, m_dataCB);
			rc.PixelShader.SetSamplers(0, MySamplerStateManager.PointHBAOClamp);
			rc.SetBlendState(null);
			rc.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
			DrawLinearDepthPS(rc, gbuffer.ResolvedDepthStencil.SrvDepth, m_fullResViewDepthTarget, viewport.Value);
			DrawDeinterleavedDepth(rc, viewport.Value);
			if (!Params.UseGBufferNormals)
			{
				DrawReconstructedNormal(rc, m_fullResViewDepthTarget, m_fullResNormalTexture, viewport.Value);
				DrawCoarseAO(rc, m_fullResNormalTexture, viewport.Value);
			}
			else
			{
				DrawCoarseAO(rc, gbuffer.GBuffer1, viewport.Value);
			}
			if (Params.BlurEnable)
			{
				Resolve(rc, blur: true, m_fullResAOZTexture2, viewport.Value);
				DrawBlurXPS(rc, viewport.Value);
				DrawBlurYPS(rc, dst, viewport.Value);
			}
			else
			{
				Resolve(rc, blur: false, dst, viewport.Value);
			}
			rc.SetRtvNull();
		}

		public static void DrawNormalsTexture(MyRenderContext rc)
		{
			if (!Params.UseGBufferNormals)
			{
				rc.PixelShader.SetSrv(1, m_fullResNormalTexture);
			}
			else
			{
				rc.PixelShader.SetSrv(1, MyGBuffer.Main.GBuffer1);
			}
			rc.PixelShader.Set(m_drawNormalsPS);
			MyScreenPass.DrawFullscreenQuad(rc);
		}

		private static void DrawLinearDepthPS(MyRenderContext rc, ISrvBindable resolvedDepth, IRtvBindable rtv, MyViewport viewport)
		{
			rc.PixelShader.Set(m_linearizeDepthPS);
			rc.SetRtv(rtv);
			rc.PixelShader.SetSrv(0, resolvedDepth);
			MyScreenPass.DrawFullscreenQuad(rc, viewport);
			rc.SetRtvNull();
		}

		private static MyViewport GetQuarterViewport(MyViewport viewport)
		{
			MyViewport result = default(MyViewport);
			result.OffsetX = 0f;
			result.OffsetY = 0f;
			result.Width = DivUp((int)viewport.Width, 4);
			result.Height = DivUp((int)viewport.Height, 4);
			return result;
		}

		private static void DrawDeinterleavedDepth(MyRenderContext rc, MyViewport viewport)
		{
			MyViewport quarterViewport = GetQuarterViewport(viewport);
			rc.PixelShader.Set(m_deinterleaveDepthPS);
			rc.PixelShader.SetSrv(0, m_fullResViewDepthTarget);
			for (int i = 0; i < 16; i += 8)
			{
				for (int j = 0; j < 8; j++)
				{
					m_rtmp[j] = m_quarterResViewDepthTextureArray.SubresourceRtv(i + j).Rtv;
				}
				rc.SetRtvs(m_rtmp);
				rc.PixelShader.SetConstantBuffer(1, m_perPassCBs[i]);
				MyScreenPass.DrawFullscreenQuad(rc, quarterViewport);
			}
		}

		private static void DrawReconstructedNormal(MyRenderContext rc, ISrvTexture depth, IRtvTexture normals, MyViewport viewport)
		{
			rc.SetRtv(normals);
			rc.PixelShader.Set(m_reconstructNormalPS);
			rc.PixelShader.SetSrv(0, depth);
			rc.PixelShader.SetSamplers(0, MySamplerStateManager.PointHBAOClamp);
			MyScreenPass.DrawFullscreenQuad(rc, viewport);
			rc.SetRtvNull();
		}

		private static void DrawCoarseAO(MyRenderContext rc, ISrvTexture normals, MyViewport viewport)
		{
			MyViewport quarterViewport = GetQuarterViewport(viewport);
			rc.PixelShader.Set(m_coarseAOPS);
			rc.PixelShader.SetSamplers(0, Params.DepthClampToEdge ? MySamplerStateManager.PointHBAOClamp : MySamplerStateManager.PointHBAOBorder);
			rc.PixelShader.SetSamplers(1, MySamplerStateManager.PointHBAOClamp);
			rc.PixelShader.SetSrv(1, normals);
			for (int i = 0; i < 16; i++)
			{
				rc.PixelShader.SetSrv(0, m_quarterResViewDepthTextureArray.SubresourceSrv(i));
				rc.PixelShader.SetConstantBuffer(1, m_perPassCBs[i]);
				rc.SetRtv(m_quarterResAOTextureArray.SubresourceRtv(i));
				MyScreenPass.DrawFullscreenQuad(rc, quarterViewport);
			}
		}

		private static void Resolve(MyRenderContext rc, bool blur, IRtvBindable dst, MyViewport viewport)
		{
			rc.SetRtv(dst);
			rc.PixelShader.SetSrv(0, m_quarterResAOTextureArray);
			if (blur)
			{
				rc.PixelShader.Set(m_reinterleaveAOPS_PreBlur);
				rc.PixelShader.SetSrv(1, m_fullResViewDepthTarget);
			}
			else
			{
				rc.PixelShader.Set(m_reinterleaveAOPS);
			}
			rc.PixelShader.SetSamplers(0, MySamplerStateManager.PointHBAOClamp);
			MyScreenPass.DrawFullscreenQuad(rc, viewport);
		}

		private static void DrawBlurXPS(MyRenderContext rc, MyViewport viewport)
		{
			rc.SetRtv(m_fullResAOZTexture);
			rc.PixelShader.Set(m_blurXPS);
			rc.PixelShader.SetSrv(0, m_fullResAOZTexture2);
			rc.PixelShader.SetSamplers(0, MySamplerStateManager.Point);
			rc.PixelShader.SetSamplers(1, MySamplerStateManager.Linear);
			MyScreenPass.DrawFullscreenQuad(rc, viewport);
		}

		private static void DrawBlurYPS(MyRenderContext rc, IRtvBindable dst, MyViewport viewport)
		{
			rc.SetRtv(dst);
			rc.PixelShader.Set(m_blurYPS);
			rc.PixelShader.SetSrv(0, m_fullResAOZTexture);
			MyScreenPass.DrawFullscreenQuad(rc, viewport);
		}

		private static void DebugDraw(MyRenderContext rc, ISrvBindable src, IRtvBindable dst, MyViewport viewport)
		{
			rc.PixelShader.Set(m_copyPS);
			rc.SetRtv(dst);
			rc.PixelShader.SetSrv(0, src);
			MyScreenPass.DrawFullscreenQuad(rc, viewport);
		}

		unsafe static MyHBAO()
		{
			GLOBALCONSTANTBUFFERSIZE = sizeof(GlobalConstantBuffer);
			PERPASSCONSTANTBUFFERSIZE = sizeof(PerPassConstantBuffer);
			m_linearizeDepthPS = MyPixelShaders.Id.NULL;
			m_deinterleaveDepthPS = MyPixelShaders.Id.NULL;
			m_reconstructNormalPS = MyPixelShaders.Id.NULL;
			m_coarseAOPS = MyPixelShaders.Id.NULL;
			m_copyPS = MyPixelShaders.Id.NULL;
			m_reinterleaveAOPS = MyPixelShaders.Id.NULL;
			m_reinterleaveAOPS_PreBlur = MyPixelShaders.Id.NULL;
			m_blurXPS = MyPixelShaders.Id.NULL;
			m_blurYPS = MyPixelShaders.Id.NULL;
			m_drawNormalsPS = MyPixelShaders.Id.NULL;
			m_perPassCBs = new IConstantBuffer[16];
			m_rtmp = new RenderTargetView[8];
			m_macros = new List<ShaderMacro>();
			METERS_TO_VIEW_SPACE_UNITS = 1f;
			Params = MyHBAOData.Default;
		}

		internal static void CompilePS()
		{
			if (m_coarseAOPS == MyPixelShaders.Id.NULL || m_lastParams.BackgroundAOEnable != Params.BackgroundAOEnable || m_lastParams.ForegroundAOEnable != Params.ForegroundAOEnable || m_lastParams.DepthThresholdEnable != Params.DepthThresholdEnable || m_lastParams.UseGBufferNormals != Params.UseGBufferNormals)
			{
				m_macros.Clear();
				if (Params.UseGBufferNormals)
				{
					m_macros.Add(new ShaderMacro("FETCH_GBUFFER_NORMAL", 1));
				}
				if (Params.BackgroundAOEnable)
				{
					m_macros.Add(new ShaderMacro("ENABLE_BACKGROUND_AO", 1));
				}
				if (Params.ForegroundAOEnable)
				{
					m_macros.Add(new ShaderMacro("ENABLE_FOREGROUND_AO", 1));
				}
				if (Params.DepthThresholdEnable)
				{
					m_macros.Add(new ShaderMacro("ENABLE_DEPTH_THRESHOLD", 1));
				}
				ShaderMacro[] macros = m_macros.ToArray();
				m_coarseAOPS = MyPixelShaders.Create("Postprocess/HBAO/CoarseAO.hlsl", macros);
				m_drawNormalsPS = MyPixelShaders.Create("Postprocess/HBAO/DrawNormals.hlsl", macros);
			}
			if (m_blurXPS == MyPixelShaders.Id.NULL || m_blurYPS == MyPixelShaders.Id.NULL || m_lastParams.BlurSharpnessFunctionEnable != Params.BlurSharpnessFunctionEnable || m_lastParams.BlurRadius4 != Params.BlurRadius4)
			{
				m_macros.Clear();
				if (Params.BlurSharpnessFunctionEnable)
				{
					m_macros.Add(new ShaderMacro("ENABLE_SHARPNESS_PROFILE", 1));
				}
				if (Params.BlurRadius4)
				{
					m_macros.Add(new ShaderMacro("KERNEL_RADIUS", 4));
				}
				else
				{
					m_macros.Add(new ShaderMacro("KERNEL_RADIUS", 2));
				}
				m_blurXPS = MyPixelShaders.Create("Postprocess/HBAO/BlurX.hlsl", m_macros.ToArray());
				m_blurYPS = MyPixelShaders.Create("Postprocess/HBAO/BlurY.hlsl", m_macros.ToArray());
			}
			m_lastParams = Params;
		}

		internal static void Init(MyRenderContext rc)
		{
			m_linearizeDepthPS = MyPixelShaders.Create("Postprocess/HBAO/LinearizeDepth.hlsl");
			m_deinterleaveDepthPS = MyPixelShaders.Create("Postprocess/HBAO/DeinterleaveDepth.hlsl");
			m_reconstructNormalPS = MyPixelShaders.Create("Postprocess/HBAO/ReconstructNormal.hlsl");
			m_reinterleaveAOPS = MyPixelShaders.Create("Postprocess/HBAO/ReinterleaveAO.hlsl");
			m_reinterleaveAOPS_PreBlur = MyPixelShaders.Create("Postprocess/HBAO/ReinterleaveAO.hlsl", new ShaderMacro[1]
			{
				new ShaderMacro("ENABLE_BLUR", 1)
			});
			m_copyPS = MyPixelShaders.Create("Postprocess/HBAO/Copy.hlsl");
			m_dataCB = MyManagers.Buffers.CreateConstantBuffer("MyHBAO::dataCB", GLOBALCONSTANTBUFFERSIZE, null, ResourceUsage.Dynamic, isGlobal: true);
			for (int i = 0; i < 16; i++)
			{
				m_perPassCBs[i] = null;
			}
			InitializeConstantBuffer(rc);
		}

		public unsafe static void InitializeConstantBuffer(MyRenderContext rc, int? randomSeed = null)
		{
			MyRandom myRandom = ((!randomSeed.HasValue) ? new MyRandom() : new MyRandom(randomSeed.Value));
			Vector4[] array = new Vector4[16];
			for (int i = 0; i < 16; i++)
			{
				float num = (float)Math.PI * 2f * myRandom.NextFloat() / 8f;
				array[i].X = (float)Math.Cos(num);
				array[i].Y = (float)Math.Sin(num);
				array[i].Z = myRandom.NextFloat();
				array[i].W = myRandom.NextFloat();
			}
			PerPassConstantBuffer perPassConstantBuffer = default(PerPassConstantBuffer);
			for (uint num2 = 0u; num2 < 16; num2++)
			{
				perPassConstantBuffer.Offset.X = (float)(num2 % 4u) + 0.5f;
				perPassConstantBuffer.Offset.Y = (float)(num2 / 4u) + 0.5f;
				perPassConstantBuffer.Jitter = array[num2];
				perPassConstantBuffer.SliceIndexFloat = num2;
				perPassConstantBuffer.SliceIndexInt = num2;
				IConstantBuffer constantBuffer = m_perPassCBs[num2];
				if (constantBuffer != null)
				{
					MyManagers.Buffers.Dispose(constantBuffer);
				}
				constantBuffer = MyManagers.Buffers.CreateConstantBuffer("MyHBAO::passCB " + num2, PERPASSCONSTANTBUFFERSIZE, new IntPtr(&perPassConstantBuffer), ResourceUsage.Immutable, isGlobal: true);
				m_perPassCBs[num2] = constantBuffer;
			}
		}

		private static int DivUp(int a, int b)
		{
			return (a + b - 1) / b;
		}

		internal static void InitScreenResources()
		{
			MyRwTextureManager rwTextures = MyManagers.RwTextures;
			m_fullResViewDepthTarget = rwTextures.CreateRtv("MyHBAO.FullResViewDepthTarget", MyRender11.m_resolution.X, MyRender11.m_resolution.Y, Format.R32_Float);
			m_fullResNormalTexture = rwTextures.CreateRtv("MyHBAO.FullResNormalTexture", MyRender11.m_resolution.X, MyRender11.m_resolution.Y, Format.R8G8B8A8_UNorm);
			m_fullResAOZTexture = rwTextures.CreateRtv("MyHBAO.FullResAOZTexture", MyRender11.m_resolution.X, MyRender11.m_resolution.Y, Format.R16G16_Float);
			m_fullResAOZTexture2 = rwTextures.CreateRtv("MyHBAO.FullResAOZTexture2", MyRender11.m_resolution.X, MyRender11.m_resolution.Y, Format.R16G16_Float);
			MyArrayTextureManager arrayTextures = MyManagers.ArrayTextures;
			m_quarterResViewDepthTextureArray = arrayTextures.CreateRtvArray("MyHBAO.QuarterResViewDepthTextureArray", DivUp(MyRender11.m_resolution.X, 4), DivUp(MyRender11.m_resolution.Y, 4), 16, Format.R16_Float);
			m_quarterResAOTextureArray = arrayTextures.CreateRtvArray("MyHBAO.QuarterResAOTextureArray", DivUp(MyRender11.m_resolution.X, 4), DivUp(MyRender11.m_resolution.Y, 4), 16, Format.R8_UNorm);
		}

		internal static void ReleaseScreenResources()
		{
			if (m_fullResViewDepthTarget != null)
			{
				MyRwTextureManager rwTextures = MyManagers.RwTextures;
				rwTextures.DisposeTex(ref m_fullResViewDepthTarget);
				rwTextures.DisposeTex(ref m_fullResNormalTexture);
				rwTextures.DisposeTex(ref m_fullResAOZTexture);
				rwTextures.DisposeTex(ref m_fullResAOZTexture2);
				MyArrayTextureManager arrayTextures = MyManagers.ArrayTextures;
				arrayTextures.DisposeTex(ref m_quarterResViewDepthTextureArray);
				arrayTextures.DisposeTex(ref m_quarterResAOTextureArray);
			}
		}

		private static GlobalConstantBuffer InitConstantBuffer(MyViewport viewport)
		{
			GlobalConstantBuffer result = default(GlobalConstantBuffer);
			Matrix projection = MyRender11.Environment.Matrices.Projection;
			float m = projection.M33;
			float m2 = projection.M43;
			float num = m2 / m;
			float num2 = m2 / (m + 1f);
			float num3 = 1f / Math.Abs(projection.M11);
			float num4 = 1f / Math.Abs(projection.M22);
			float num5 = Math.Max(1f / num, 1E-06f);
			float num6 = Math.Max(1f / num2, 1E-06f);
			result.LinearizeDepthA = num6 - num5;
			result.LinearizeDepthB = num5;
			result.InverseDepthRangeA = 1f;
			result.InverseDepthRangeB = 0f;
			result.InputViewportTopLeft.X = viewport.OffsetX;
			result.InputViewportTopLeft.Y = viewport.OffsetY;
			result.UVToViewA.X = 2f * num3;
			result.UVToViewA.Y = -2f * num4;
			result.UVToViewB.X = -1f * num3;
			result.UVToViewB.Y = 1f * num4;
			result.InvFullResolution.X = 1f / viewport.Width;
			result.InvFullResolution.Y = 1f / viewport.Height;
			result.InvQuarterResolution.X = 1f / (float)DivUp((int)viewport.Width, 4);
			result.InvQuarterResolution.Y = 1f / (float)DivUp((int)viewport.Height, 4);
			result.NormalMatrix = MyRender11.Environment.Matrices.ViewAt0;
			result.NormalDecodeScale = 2f;
			result.NormalDecodeBias = -1f;
			float num7 = Math.Max(Params.Radius, 1E-06f) * METERS_TO_VIEW_SPACE_UNITS;
			result.R2 = num7 * num7;
			result.NegInvR2 = -1f / result.R2;
			result.RadiusToScreen = num7 * 0.5f / num4 * viewport.Height;
			float num8 = Math.Max(Params.BackgroundViewDepth, 1E-06f);
			if (Params.AdaptToFOV)
			{
				float amount = Math.Min(1f, MyRender11.Environment.Matrices.FovH / MathHelper.ToRadians(30f));
				num8 = MathHelper.Lerp(6000f, num8, amount);
			}
			result.BackgroundAORadiusPixels = result.RadiusToScreen / num8;
			float num9 = Math.Max(Params.ForegroundViewDepth, 1E-06f);
			result.ForegroundAORadiusPixels = result.RadiusToScreen / num9;
			float num10 = Math.Max(Params.BlurSharpness, 0f);
			num10 /= METERS_TO_VIEW_SPACE_UNITS;
			if (Params.BlurSharpnessFunctionEnable)
			{
				result.BlurViewDepth0 = Math.Max(Params.BlurSharpnessFunctionForegroundViewDepth, 0f);
				result.BlurViewDepth1 = Math.Max(Params.BlurSharpnessFunctionBackgroundViewDepth, result.BlurViewDepth0 + 1E-06f);
				result.BlurSharpness0 = num10 * Math.Max(Params.BlurSharpnessFunctionForegroundScale, 0f);
				result.BlurSharpness1 = num10;
			}
			else
			{
				result.BlurSharpness0 = num10;
				result.BlurSharpness1 = num10;
				result.BlurViewDepth0 = 0f;
				result.BlurViewDepth1 = 1f;
			}
			if (Params.DepthThresholdEnable)
			{
				result.ViewDepthThresholdNegInv = -1f / Math.Max(Params.DepthThreshold, 1E-06f);
				result.ViewDepthThresholdSharpness = Math.Max(Params.DepthThresholdSharpness, 0f);
			}
			else
			{
				result.ViewDepthThresholdNegInv = 0f;
				result.ViewDepthThresholdSharpness = 1f;
			}
			result.PowExponent = Math.Min(Math.Max(Params.PowerExponent, 1f), 8f);
			result.NDotVBias = Math.Min(Math.Max(Params.Bias, 0f), 0.5f);
			float num11 = 1f / (1f - result.NDotVBias);
			result.SmallScaleAOAmount = Math.Min(Math.Max(Params.SmallScaleAO, 0f), 4f) * num11 * 2f;
			result.LargeScaleAOAmount = Math.Min(Math.Max(Params.LargeScaleAO, 0f), 4f) * num11;
			return result;
		}
	}
}
