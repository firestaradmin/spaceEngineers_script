using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Vertex;

namespace VRageRender
{
	internal class MyScreenPass
	{
		private static MyVertexShaders.Id m_VSCopy;

		private static MyInputLayouts.Id m_IL = MyInputLayouts.Id.NULL;

		private static IVertexBuffer m_VBFullscreen;

		private static IVertexBuffer m_VBLeftPart;

		private static IVertexBuffer m_VBRightPart;

		private static readonly MyVertexFormatPositionTextureH[] m_vbData = new MyVertexFormatPositionTextureH[4];

		internal static void Init(MyRenderContext rc)
		{
			m_VSCopy = MyVertexShaders.Create("Postprocess/PostprocessCopy.hlsl");
			m_VBFullscreen = MyManagers.Buffers.CreateVertexBuffer("MyScreenPass.VBFullscreen", 4, MyVertexFormatPositionTextureH.STRIDE, null, ResourceUsage.Dynamic, isStreamOutput: false, isGlobal: true);
			m_vbData[0] = new MyVertexFormatPositionTextureH(new Vector3(-1f, -1f, 0f), new HalfVector2(0f, 1f));
			m_vbData[1] = new MyVertexFormatPositionTextureH(new Vector3(-1f, 1f, 0f), new HalfVector2(0f, 0f));
			m_vbData[2] = new MyVertexFormatPositionTextureH(new Vector3(1f, -1f, 0f), new HalfVector2(1f, 1f));
			m_vbData[3] = new MyVertexFormatPositionTextureH(new Vector3(1f, 1f, 0f), new HalfVector2(1f, 0f));
			MyMapping myMapping = MyMapping.MapDiscard(rc, m_VBFullscreen);
			myMapping.WriteAndPosition(m_vbData, 4);
			myMapping.Unmap();
			m_VBLeftPart = MyManagers.Buffers.CreateVertexBuffer("MyVRScreenPass.VBLeftPart", 4, MyVertexFormatPositionTextureH.STRIDE, null, ResourceUsage.Dynamic, isStreamOutput: false, isGlobal: true);
			m_vbData[0] = new MyVertexFormatPositionTextureH(new Vector3(-1f, -1f, 0f), new HalfVector2(0f, 1f));
			m_vbData[1] = new MyVertexFormatPositionTextureH(new Vector3(-1f, 1f, 0f), new HalfVector2(0f, 0f));
			m_vbData[2] = new MyVertexFormatPositionTextureH(new Vector3(0f, -1f, 0f), new HalfVector2(0.5f, 1f));
			m_vbData[3] = new MyVertexFormatPositionTextureH(new Vector3(0f, 1f, 0f), new HalfVector2(0.5f, 0f));
			MyMapping myMapping2 = MyMapping.MapDiscard(rc, m_VBLeftPart);
			myMapping2.WriteAndPosition(m_vbData, 4);
			myMapping2.Unmap();
			m_VBRightPart = MyManagers.Buffers.CreateVertexBuffer("MyVRScreenPass.VBRightPart", 4, MyVertexFormatPositionTextureH.STRIDE, null, ResourceUsage.Dynamic, isStreamOutput: false, isGlobal: true);
			m_vbData[0] = new MyVertexFormatPositionTextureH(new Vector3(0f, -1f, 0f), new HalfVector2(0.5f, 1f));
			m_vbData[1] = new MyVertexFormatPositionTextureH(new Vector3(0f, 1f, 0f), new HalfVector2(0.5f, 0f));
			m_vbData[2] = new MyVertexFormatPositionTextureH(new Vector3(1f, -1f, 0f), new HalfVector2(1f, 1f));
			m_vbData[3] = new MyVertexFormatPositionTextureH(new Vector3(1f, 1f, 0f), new HalfVector2(1f, 0f));
			MyMapping myMapping3 = MyMapping.MapDiscard(rc, m_VBRightPart);
			myMapping3.WriteAndPosition(m_vbData, 4);
			myMapping3.Unmap();
			m_IL = MyInputLayouts.Create(m_VSCopy.InfoId, MyVertexLayouts.GetLayout(MyVertexInputComponentType.POSITION3, MyVertexInputComponentType.TEXCOORD0_H));
		}

		internal static void DrawFullscreenQuad(MyRenderContext rc, MyViewport? customViewport = null)
		{
			if (customViewport.HasValue)
			{
				rc.SetViewport(customViewport.Value.OffsetX, customViewport.Value.OffsetY, customViewport.Value.Width, customViewport.Value.Height);
			}
			else
			{
				rc.SetScreenViewport();
			}
			if (!MyStereoRender.Enable || MyStereoRender.RenderRegion == MyStereoRegion.FULLSCREEN)
			{
				rc.SetVertexBuffer(0, m_VBFullscreen);
			}
			else if (MyStereoRender.RenderRegion == MyStereoRegion.LEFT)
			{
				rc.SetVertexBuffer(0, m_VBLeftPart);
			}
			else if (MyStereoRender.RenderRegion == MyStereoRegion.RIGHT)
			{
				rc.SetVertexBuffer(0, m_VBRightPart);
			}
			if (MyStereoRender.Enable)
			{
				MyStereoRender.PSBindRawCB_FrameConstants(rc);
			}
			rc.SetPrimitiveTopology(PrimitiveTopology.TriangleStrip);
			rc.SetInputLayout(m_IL);
			rc.VertexShader.Set(m_VSCopy);
			rc.Draw(4, 0);
			rc.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			if (MyStereoRender.Enable)
			{
				rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			}
		}

		internal static void RunFullscreenPixelFreq(MyRenderContext rc, IRtvBindable RT)
		{
			if (MyRender11.MultisamplingEnabled)
			{
				rc.SetDepthStencilState(MyDepthStencilStateManager.TestEdgeStencil);
			}
			rc.SetRtv(MyGBuffer.Main.DepthStencil.DsvRo, RT);
			DrawFullscreenQuad(rc);
			if (MyRender11.MultisamplingEnabled)
			{
				rc.SetDepthStencilState(MyDepthStencilStateManager.DefaultDepthState);
			}
		}

		internal static void RunFullscreenSampleFreq(MyRenderContext rc, IRtvBindable RT)
		{
			rc.SetDepthStencilState(MyDepthStencilStateManager.TestEdgeStencil, 128);
			rc.SetRtv(MyGBuffer.Main.DepthStencil.DsvRo, RT);
			DrawFullscreenQuad(rc);
			rc.SetDepthStencilState(MyDepthStencilStateManager.DefaultDepthState);
		}
	}
}
