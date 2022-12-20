using System;
using System.Collections.Generic;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRage.Library.Collections;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender.Messages;
using VRageRender.Vertex;

namespace VRageRender
{
	internal class MyPrimitivesRenderer : MyImmediateRC
	{
		private static int m_currentBufferSize;

		private static IVertexBuffer m_vb;

		private static readonly MyList<MyVertexFormatPositionColor> m_vertexList = new MyList<MyVertexFormatPositionColor>(1024);

		private static readonly Dictionary<uint, MyDebugMesh> m_debugMeshes = new Dictionary<uint, MyDebugMesh>();

		private static MyVertexShaders.Id m_vs;

		private static MyPixelShaders.Id m_ps;

		private static MyInputLayouts.Id m_inputLayout;

		private static List<Vector3> m_normalizedSphere = new List<Vector3>();

		public static int MeshCount => m_debugMeshes.Count;

		internal unsafe static void Init()
		{
			m_currentBufferSize = 100000;
			m_vb = MyManagers.Buffers.CreateVertexBuffer("MyPrimitivesRenderer", m_currentBufferSize, sizeof(MyVertexFormatPositionColor), null, ResourceUsage.Dynamic, isStreamOutput: false, isGlobal: true);
			m_vs = MyVertexShaders.Create("Primitives/Primitives.hlsl");
			m_ps = MyPixelShaders.Create("Primitives/Primitives.hlsl");
			m_inputLayout = MyInputLayouts.Create(m_vs.InfoId, MyVertexLayouts.GetLayout(MyVertexInputComponentType.POSITION3, MyVertexInputComponentType.COLOR4));
			InitializeNormalizedSphere();
		}

		private static void InitializeNormalizedSphere()
		{
			m_normalizedSphere.Clear();
			int num = 6;
			int num2 = 6;
			List<Vector3> list = new List<Vector3>();
			float num3 = 6.283186f / (float)num2;
			float num4 = 3.141593f / (float)num;
			for (int i = 0; i <= num; i++)
			{
				float num5 = 1.57079649f - (float)i * num4;
				float num6 = (float)Math.Cos(num5);
				float z = (float)Math.Sin(num5);
				for (int j = 0; j <= num2; j++)
				{
					float num7 = (float)j * num3;
					float x = num6 * (float)Math.Cos(num7);
					float y = num6 * (float)Math.Sin(num7);
					list.Add(new Vector3(x, y, z));
				}
			}
			for (int k = 0; k < num; k++)
			{
				int num8 = k * (num2 + 1);
				int num9 = num8 + num2 + 1;
				int num10 = 0;
				while (num10 < num2)
				{
					if (k != 0)
					{
						m_normalizedSphere.Add(list[num8]);
						m_normalizedSphere.Add(list[num9]);
						m_normalizedSphere.Add(list[num8 + 1]);
					}
					if (k != num - 1)
					{
						m_normalizedSphere.Add(list[num8 + 1]);
						m_normalizedSphere.Add(list[num9]);
						m_normalizedSphere.Add(list[num9 + 1]);
					}
					num10++;
					num8++;
					num9++;
				}
			}
		}

		internal static void Unload()
		{
			foreach (MyDebugMesh value in m_debugMeshes.Values)
			{
				value.Close();
			}
			m_debugMeshes.Clear();
		}

		private static void CheckBufferSize(int requiredSize)
		{
			if (m_currentBufferSize < requiredSize)
			{
				m_currentBufferSize = (int)((float)requiredSize * 1.33f);
				MyManagers.Buffers.Resize(m_vb, m_currentBufferSize);
			}
		}

		internal static void DebugMesh(MyRenderMessageDebugDrawMesh message)
		{
			if (!m_debugMeshes.ContainsKey(message.ID))
			{
				m_debugMeshes.Add(message.ID, new MyDebugMesh(message));
			}
			else
			{
				m_debugMeshes[message.ID].Update(message);
			}
		}

		internal static void RemoveDebugMesh(uint ID)
		{
			if (m_debugMeshes.ContainsKey(ID))
			{
				m_debugMeshes[ID].Close();
				m_debugMeshes.Remove(ID);
			}
		}

		internal static void DrawTriangle(Vector3 v0, Vector3 v1, Vector3 v2, Color color)
		{
			m_vertexList.Add(new MyVertexFormatPositionColor(v0, color));
			m_vertexList.Add(new MyVertexFormatPositionColor(v1, color));
			m_vertexList.Add(new MyVertexFormatPositionColor(v2, color));
		}

		internal static void DrawSphere(Vector3 position, float radius, Color color)
		{
			for (int i = 0; i < m_normalizedSphere.Count; i++)
			{
				m_vertexList.Add(new MyVertexFormatPositionColor(position + m_normalizedSphere[i] * radius, color));
			}
		}

		internal static void DrawQuadClockWise(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Color color)
		{
			DrawTriangle(v0, v1, v2, color);
			DrawTriangle(v0, v2, v3, color);
		}

		internal static void DrawQuadRowWise(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Color color)
		{
			DrawTriangle(v0, v1, v2, color);
			DrawTriangle(v1, v3, v2, color);
		}

		internal unsafe static void Draw6FacedConvex(Vector3D[] vertices, Color color, float alpha)
		{
			fixed (Vector3D* vertices2 = vertices)
			{
				Draw6FacedConvex(vertices2, color, alpha);
			}
		}

		internal unsafe static void Draw6FacedConvex(Vector3D* vertices, Color color, float alpha)
		{
			Color color2 = color;
			color2.A = (byte)(alpha * 255f);
			Vector3D cameraPosition = MyRender11.Environment.Matrices.CameraPosition;
			Vector3D vector3D = *vertices - cameraPosition;
			Vector3D vector3D2 = vertices[1] - cameraPosition;
			Vector3D vector3D3 = vertices[2] - cameraPosition;
			Vector3D vector3D4 = vertices[3] - cameraPosition;
			Vector3D vector3D5 = vertices[4] - cameraPosition;
			Vector3D vector3D6 = vertices[5] - cameraPosition;
			Vector3D vector3D7 = vertices[6] - cameraPosition;
			Vector3D vector3D8 = vertices[7] - cameraPosition;
			DrawQuadRowWise(vector3D, vector3D2, vector3D3, vector3D4, color2);
			DrawQuadRowWise(vector3D5, vector3D6, vector3D7, vector3D8, color2);
			DrawQuadRowWise(vector3D, vector3D2, vector3D5, vector3D6, color2);
			DrawQuadRowWise(vector3D, vector3D3, vector3D5, vector3D7, color2);
			DrawQuadRowWise(vector3D3, vector3D4, vector3D7, vector3D8, color2);
			DrawQuadRowWise(vector3D4, vector3D2, vector3D8, vector3D6, color2);
		}

		internal unsafe static void Draw6FacedConvexZ(Vector3[] vertices, Color color, float alpha)
		{
			fixed (Vector3* vertices2 = vertices)
			{
				Draw6FacedConvexZ(vertices2, color, alpha);
			}
		}

		internal unsafe static void Draw6FacedConvexZ(Vector3* vertices, Color color, float alpha)
		{
			Color color2 = color;
			color2.A = (byte)(alpha * 255f);
			DrawQuadClockWise(*vertices, vertices[1], vertices[2], vertices[3], color2);
			DrawQuadClockWise(vertices[4], vertices[5], vertices[6], vertices[7], color2);
			DrawQuadClockWise(*vertices, vertices[1], vertices[5], vertices[4], color2);
			DrawQuadClockWise(*vertices, vertices[3], vertices[7], vertices[4], color2);
			DrawQuadClockWise(vertices[3], vertices[2], vertices[6], vertices[7], color2);
			DrawQuadClockWise(vertices[2], vertices[1], vertices[5], vertices[6], color2);
		}

		internal static void DrawFrustum(BoundingFrustum frustum, Color color, float alpha)
		{
			Draw6FacedConvexZ(frustum.GetCorners(), color, alpha);
		}

		internal static void Draw(IRtvBindable renderTarget, ref Matrix viewProjMatrix, bool useDepth)
		{
			MyImmediateRC.RC.SetScreenViewport();
			MyImmediateRC.RC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			MyImmediateRC.RC.SetInputLayout(m_inputLayout);
			MyImmediateRC.RC.SetRasterizerState(MyRasterizerStateManager.NocullRasterizerState);
			MyImmediateRC.RC.SetDepthStencilState(useDepth ? MyDepthStencilStateManager.DefaultDepthState : MyDepthStencilStateManager.IgnoreDepthStencil);
			MyImmediateRC.RC.VertexShader.Set(m_vs);
			MyImmediateRC.RC.PixelShader.Set(m_ps);
			MyImmediateRC.RC.SetRtv(MyGBuffer.Main.ResolvedDepthStencil.DsvRo, renderTarget);
			MyImmediateRC.RC.AllShaderStages.SetConstantBuffer(1, MyCommon.ProjectionConstants);
			MyImmediateRC.RC.SetBlendState(MyBlendStateManager.BlendTransparent);
			Matrix data = Matrix.Transpose(viewProjMatrix);
			MyMapping myMapping = MyMapping.MapDiscard(MyCommon.ProjectionConstants);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			if (m_vertexList.Count > 0)
			{
				CheckBufferSize(m_vertexList.Count);
				MyImmediateRC.RC.SetVertexBuffer(0, m_vb);
				myMapping = MyMapping.MapDiscard(m_vb);
				myMapping.WriteAndPosition(m_vertexList.GetInternalArray(), m_vertexList.Count);
				myMapping.Unmap();
				MyImmediateRC.RC.Draw(m_vertexList.Count, 0);
			}
			foreach (MyDebugMesh value in m_debugMeshes.Values)
			{
				if (value.Depth)
				{
					MyImmediateRC.RC.SetRtv(MyGBuffer.Main.ResolvedDepthStencil.Dsv, renderTarget);
				}
				else
				{
					MyImmediateRC.RC.SetRtv(renderTarget);
				}
				if (value.Edges)
				{
					MyImmediateRC.RC.SetRasterizerState(MyRasterizerStateManager.NocullWireframeRasterizerState);
				}
				else
				{
					MyImmediateRC.RC.SetRasterizerState(MyRasterizerStateManager.NocullRasterizerState);
				}
				Matrix matrix = value.WorldMatrix;
				matrix.Translation = value.WorldMatrix.Translation - MyRender11.Environment.Matrices.CameraPosition;
				Matrix data2 = Matrix.Transpose(matrix * viewProjMatrix);
				MyMapping myMapping2 = MyMapping.MapDiscard(MyCommon.ProjectionConstants);
				myMapping2.WriteAndPosition(ref data2);
				myMapping2.Unmap();
				MyImmediateRC.RC.SetVertexBuffer(0, value.Buffer);
				MyImmediateRC.RC.Draw(value.Buffer.ElementCount, 0);
			}
			MyImmediateRC.RC.SetBlendState(null);
			m_vertexList.ClearFast();
		}
	}
}
