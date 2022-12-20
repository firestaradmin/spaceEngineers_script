using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender.Vertex;

namespace VRageRender
{
	internal class MyStereoStencilMask : MyImmediateRC
	{
		private static Vector2[] m_VBdata;

		private static IVertexBuffer m_VB;

		private static MyVertexShaders.Id m_vs;

		private static MyPixelShaders.Id m_ps;

		private static MyInputLayouts.Id m_il;

		private static readonly Vector2[] m_tmpInitUndefinedMaskVerts = new Vector2[6];

		private static void InitInternal(Vector2[] vertsForMask)
		{
			m_VB = MyManagers.Buffers.CreateVertexBuffer("MyStereoStencilMask.VB", vertsForMask.Length, MyVertexFormat2DPosition.STRIDE, null, ResourceUsage.Dynamic);
			MyMapping myMapping = MyMapping.MapDiscard(MyImmediateRC.RC, m_VB);
			myMapping.WriteAndPosition(vertsForMask, vertsForMask.Length);
			myMapping.Unmap();
			m_vs = MyVertexShaders.Create("Stereo/StereoStencilMask.hlsl");
			m_ps = MyPixelShaders.Create("Stereo/StereoStencilMask.hlsl");
			m_il = MyInputLayouts.Create(m_vs.InfoId, MyVertexLayouts.GetLayout(MyVertexInputComponentType.POSITION2));
		}

		private static Vector2[] GetUndefinedMask()
		{
			float num = 0.17f;
			Vector2 vector = new Vector2(0f - num, 1f);
			Vector2 vector2 = new Vector2(num, 1f);
			Vector2 vector3 = new Vector2(0f - num, -1f);
			Vector2 vector4 = new Vector2(num, -1f);
			Vector2[] tmpInitUndefinedMaskVerts = m_tmpInitUndefinedMaskVerts;
			tmpInitUndefinedMaskVerts[0] = vector;
			tmpInitUndefinedMaskVerts[1] = vector2;
			tmpInitUndefinedMaskVerts[2] = vector3;
			tmpInitUndefinedMaskVerts[3] = vector3;
			tmpInitUndefinedMaskVerts[4] = vector2;
			tmpInitUndefinedMaskVerts[5] = vector4;
			return tmpInitUndefinedMaskVerts;
		}

		internal static void InitUsingUndefinedMask()
		{
			m_VBdata = GetUndefinedMask();
			InitInternal(m_VBdata);
		}

		internal static void InitUsingOpenVR()
		{
		}

		internal static void Draw()
		{
			MyImmediateRC.RC.SetRtvs(MyGBuffer.Main, MyDepthStencilAccess.ReadWrite);
			MyImmediateRC.RC.SetScreenViewport();
			MyImmediateRC.RC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			MyImmediateRC.RC.SetVertexBuffer(0, m_VB);
			MyImmediateRC.RC.SetInputLayout(m_il);
			MyImmediateRC.RC.SetRasterizerState(MyRasterizerStateManager.NocullRasterizerState);
			MyImmediateRC.RC.SetDepthStencilState(MyDepthStencilStateManager.StereoStencilMask, MyDepthStencilStateManager.GetStereoMask());
			MyImmediateRC.RC.VertexShader.Set(m_vs);
			MyImmediateRC.RC.PixelShader.Set(m_ps);
			MyImmediateRC.RC.Draw(m_VBdata.Length, 0);
		}
	}
}
