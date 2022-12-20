using System;
using System.Collections.Generic;
using VRage.Network;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Vertex;

namespace VRageRender
{
	[GenerateActivator]
	internal class MyLinesBatch
	{
		private class VRageRender_MyLinesBatch_003C_003EActor : IActivator, IActivator<MyLinesBatch>
		{
			private sealed override object CreateInstance()
			{
				return new MyLinesBatch();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLinesBatch CreateInstance()
			{
				return new MyLinesBatch();
			}

			MyLinesBatch IActivator<MyLinesBatch>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		internal Matrix? CustomViewProjection;

		internal int VertexCount;

		internal int StartVertex;

		internal List<MyVertexFormatPositionColor> List;

		internal bool IgnoreDepth;

		internal void Construct()
		{
			CustomViewProjection = null;
			IgnoreDepth = false;
			if (List == null)
			{
				List = new List<MyVertexFormatPositionColor>(128);
			}
			else
			{
				List.Clear();
			}
			VertexCount = 0;
			StartVertex = 0;
		}

		internal void Add(MyVertexFormatPositionColor v)
		{
			List.Add(v);
		}

		internal void Add(MyVertexFormatPositionColor from, MyVertexFormatPositionColor to)
		{
			List.Add(from);
			List.Add(to);
		}

<<<<<<< HEAD
		internal void Add(Vector3D from, Vector3D to, Color colorFrom, Color? colorTo = null)
=======
		internal void Add(Vector3 from, Vector3 to, Color colorFrom, Color? colorTo = null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			List.Add(new MyVertexFormatPositionColor(from, new Byte4(colorFrom.PackedValue)));
			List.Add(new MyVertexFormatPositionColor(to, colorTo.HasValue ? new Byte4(colorTo.Value.PackedValue) : new Byte4(colorFrom.PackedValue)));
		}

		internal void AddQuad(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Color color)
		{
			Byte4 color2 = new Byte4((int)color.R, (int)color.G, (int)color.B, (int)color.A);
			Add(new MyVertexFormatPositionColor(v0, color2));
			Add(new MyVertexFormatPositionColor(v1, color2));
			Add(new MyVertexFormatPositionColor(v1, color2));
			Add(new MyVertexFormatPositionColor(v2, color2));
			Add(new MyVertexFormatPositionColor(v2, color2));
			Add(new MyVertexFormatPositionColor(v3, color2));
			Add(new MyVertexFormatPositionColor(v3, color2));
			Add(new MyVertexFormatPositionColor(v0, color2));
		}

		internal void AddCone(Vector3 translation, Vector3 directionVec, Vector3 baseVec, int tessalation, Color color)
		{
			Vector3 axis = directionVec;
			axis.Normalize();
<<<<<<< HEAD
			Vector3 vector = translation + directionVec;
=======
			Vector3 to = translation + directionVec;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			float num = (float)(Math.PI * 2.0 / (double)tessalation);
			for (int i = 0; i < 32; i++)
			{
				float angle = (float)i * num;
				float angle2 = (float)(i + 1) * num;
<<<<<<< HEAD
				Vector3 vector2 = translation + Vector3.Transform(baseVec, Matrix.CreateFromAxisAngle(axis, angle));
				Vector3 vector3 = translation + Vector3.Transform(baseVec, Matrix.CreateFromAxisAngle(axis, angle2));
				Add(vector2, vector3, color);
				Add(vector2, vector, color);
=======
				Vector3 from = translation + Vector3.Transform(baseVec, Matrix.CreateFromAxisAngle(axis, angle));
				Vector3 to2 = translation + Vector3.Transform(baseVec, Matrix.CreateFromAxisAngle(axis, angle2));
				Add(from, to2, color);
				Add(from, to, color);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		internal void AddFrustum(BoundingFrustum bf, Color color)
		{
			Add6FacedConvex(bf.GetCorners(), color);
		}

		internal void Add6FacedConvexWorld(Vector3D[] v, Color col)
		{
			Vector3D cameraPosition = MyRender11.Environment.Matrices.CameraPosition;
			Vector3D vector3D = v[0] - cameraPosition;
			Vector3D vector3D2 = v[1] - cameraPosition;
<<<<<<< HEAD
			Vector3D from = v[2] - cameraPosition;
			Vector3D vector3D3 = v[3] - cameraPosition;
			Vector3D vector3D4 = v[4] - cameraPosition;
			Vector3D vector3D5 = v[5] - cameraPosition;
			Vector3D vector3D6 = v[6] - cameraPosition;
			Vector3D to = v[7] - cameraPosition;
			Add(vector3D, vector3D2, col);
			Add(vector3D2, vector3D3, col);
			Add(from, vector3D3, col);
			Add(from, vector3D, col);
			Add(vector3D4, vector3D5, col);
			Add(vector3D5, to, col);
			Add(vector3D6, to, col);
			Add(vector3D6, vector3D4, col);
			Add(vector3D, vector3D4, col);
			Add(vector3D2, vector3D5, col);
			Add(from, vector3D6, col);
			Add(vector3D3, to, col);
=======
			Vector3D vector3D3 = v[2] - cameraPosition;
			Vector3D vector3D4 = v[3] - cameraPosition;
			Vector3D vector3D5 = v[4] - cameraPosition;
			Vector3D vector3D6 = v[5] - cameraPosition;
			Vector3D vector3D7 = v[6] - cameraPosition;
			Vector3D vector3D8 = v[7] - cameraPosition;
			Add(vector3D, vector3D2, col);
			Add(vector3D2, vector3D4, col);
			Add(vector3D3, vector3D4, col);
			Add(vector3D3, vector3D, col);
			Add(vector3D5, vector3D6, col);
			Add(vector3D6, vector3D8, col);
			Add(vector3D7, vector3D8, col);
			Add(vector3D7, vector3D5, col);
			Add(vector3D, vector3D5, col);
			Add(vector3D2, vector3D6, col);
			Add(vector3D3, vector3D7, col);
			Add(vector3D4, vector3D8, col);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		internal unsafe void Add6FacedConvex(Vector3[] vertices, Color color)
		{
			fixed (Vector3* vertices2 = vertices)
			{
				Add6FacedConvex(vertices2, color);
			}
		}

		internal unsafe void Add6FacedConvex(Vector3* vertices, Color color)
		{
			Add(*vertices, vertices[1], color);
			Add(vertices[1], vertices[2], color);
			Add(vertices[2], vertices[3], color);
			Add(vertices[3], *vertices, color);
			Add(vertices[4], vertices[5], color);
			Add(vertices[5], vertices[6], color);
			Add(vertices[6], vertices[7], color);
			Add(vertices[7], vertices[4], color);
			Add(*vertices, vertices[4], color);
			Add(vertices[1], vertices[5], color);
			Add(vertices[2], vertices[6], color);
			Add(vertices[3], vertices[7], color);
		}

		internal void AddCross(Vector3 pos, Color color)
		{
			Add(pos - Vector3.Forward, pos + Vector3.Forward, color);
			Add(pos - Vector3.Right, pos + Vector3.Right, color);
			Add(pos - Vector3.Up, pos + Vector3.Up, color);
		}

		internal void AddBoundingBox(BoundingBox bb, Color color)
		{
			Vector3 vector = bb.Center - bb.HalfExtents;
			Vector3 position = vector + new Vector3(bb.HalfExtents.X * 2f, 0f, 0f);
			Vector3 position2 = vector + new Vector3(bb.HalfExtents.X * 2f, bb.HalfExtents.Y * 2f, 0f);
			Vector3 position3 = vector + new Vector3(0f, bb.HalfExtents.Y * 2f, 0f);
			Vector3 vector2 = vector + new Vector3(0f, 0f, bb.HalfExtents.Z * 2f);
			Vector3 position4 = vector2 + new Vector3(bb.HalfExtents.X * 2f, 0f, 0f);
			Vector3 position5 = vector2 + new Vector3(bb.HalfExtents.X * 2f, bb.HalfExtents.Y * 2f, 0f);
			Vector3 position6 = vector2 + new Vector3(0f, bb.HalfExtents.Y * 2f, 0f);
			Byte4 color2 = new Byte4((int)color.R, (int)color.G, (int)color.B, (int)color.A);
			Add(new MyVertexFormatPositionColor(vector, color2));
			Add(new MyVertexFormatPositionColor(position, color2));
			Add(new MyVertexFormatPositionColor(position, color2));
			Add(new MyVertexFormatPositionColor(position2, color2));
			Add(new MyVertexFormatPositionColor(position2, color2));
			Add(new MyVertexFormatPositionColor(position3, color2));
			Add(new MyVertexFormatPositionColor(vector, color2));
			Add(new MyVertexFormatPositionColor(position3, color2));
			Add(new MyVertexFormatPositionColor(vector2, color2));
			Add(new MyVertexFormatPositionColor(position4, color2));
			Add(new MyVertexFormatPositionColor(position4, color2));
			Add(new MyVertexFormatPositionColor(position5, color2));
			Add(new MyVertexFormatPositionColor(position5, color2));
			Add(new MyVertexFormatPositionColor(position6, color2));
			Add(new MyVertexFormatPositionColor(vector2, color2));
			Add(new MyVertexFormatPositionColor(position6, color2));
			Add(new MyVertexFormatPositionColor(vector, color2));
			Add(new MyVertexFormatPositionColor(vector2, color2));
			Add(new MyVertexFormatPositionColor(position, color2));
			Add(new MyVertexFormatPositionColor(position4, color2));
			Add(new MyVertexFormatPositionColor(position2, color2));
			Add(new MyVertexFormatPositionColor(position5, color2));
			Add(new MyVertexFormatPositionColor(position3, color2));
			Add(new MyVertexFormatPositionColor(position6, color2));
		}

		internal void AddBoundingBox(BoundingBoxD bb, Color color, MatrixD m)
		{
			Vector3D vector3D = bb.Center - bb.HalfExtents;
			Vector3D position = vector3D + new Vector3(bb.HalfExtents.X * 2.0, 0.0, 0.0);
			Vector3D position2 = vector3D + new Vector3(bb.HalfExtents.X * 2.0, bb.HalfExtents.Y * 2.0, 0.0);
			Vector3D position3 = vector3D + new Vector3(0.0, bb.HalfExtents.Y * 2.0, 0.0);
			Vector3D vector3D2 = vector3D + new Vector3(0.0, 0.0, bb.HalfExtents.Z * 2.0);
			Vector3D position4 = vector3D2 + new Vector3(bb.HalfExtents.X * 2.0, 0.0, 0.0);
			Vector3D position5 = vector3D2 + new Vector3(bb.HalfExtents.X * 2.0, bb.HalfExtents.Y * 2.0, 0.0);
			Vector3D position6 = vector3D2 + new Vector3(0.0, bb.HalfExtents.Y * 2.0, 0.0);
			vector3D = Vector3D.Transform(vector3D, m);
			position = Vector3D.Transform(position, m);
			position2 = Vector3D.Transform(position2, m);
			position3 = Vector3D.Transform(position3, m);
			vector3D2 = Vector3D.Transform(vector3D2, m);
			position4 = Vector3D.Transform(position4, m);
			position5 = Vector3D.Transform(position5, m);
			position6 = Vector3D.Transform(position6, m);
			Byte4 color2 = new Byte4((int)color.R, (int)color.G, (int)color.B, (int)color.A);
			Add(new MyVertexFormatPositionColor(vector3D, color2));
			Add(new MyVertexFormatPositionColor(position, color2));
			Add(new MyVertexFormatPositionColor(position, color2));
			Add(new MyVertexFormatPositionColor(position2, color2));
			Add(new MyVertexFormatPositionColor(position2, color2));
			Add(new MyVertexFormatPositionColor(position3, color2));
			Add(new MyVertexFormatPositionColor(vector3D, color2));
			Add(new MyVertexFormatPositionColor(position3, color2));
			Add(new MyVertexFormatPositionColor(vector3D2, color2));
			Add(new MyVertexFormatPositionColor(position4, color2));
			Add(new MyVertexFormatPositionColor(position4, color2));
			Add(new MyVertexFormatPositionColor(position5, color2));
			Add(new MyVertexFormatPositionColor(position5, color2));
			Add(new MyVertexFormatPositionColor(position6, color2));
			Add(new MyVertexFormatPositionColor(vector3D2, color2));
			Add(new MyVertexFormatPositionColor(position6, color2));
			Add(new MyVertexFormatPositionColor(vector3D, color2));
			Add(new MyVertexFormatPositionColor(vector3D2, color2));
			Add(new MyVertexFormatPositionColor(position, color2));
			Add(new MyVertexFormatPositionColor(position4, color2));
			Add(new MyVertexFormatPositionColor(position2, color2));
			Add(new MyVertexFormatPositionColor(position5, color2));
			Add(new MyVertexFormatPositionColor(position3, color2));
			Add(new MyVertexFormatPositionColor(position6, color2));
		}

		internal void AddSphereRing(BoundingSphere sphere, Color color, Matrix onb)
		{
			float num = 0.03125f;
			for (float num2 = 0f; num2 < 1f; num2 += num)
			{
				float num3 = (float)Math.PI * 2f * num2;
				float num4 = (float)Math.PI * 2f * (num2 + num);
				Add(Vector3.Transform(new Vector3(Math.Cos(num3), 0.0, Math.Sin(num3)) * sphere.Radius, onb) + sphere.Center, Vector3.Transform(new Vector3(Math.Cos(num4), 0.0, Math.Sin(num4)) * sphere.Radius, onb) + sphere.Center, color);
			}
		}

		internal void Commit()
		{
			MyLinesRenderer.Commit(this);
		}
	}
}
