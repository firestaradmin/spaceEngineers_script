using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	/// <summary>
	/// Defines a 3d triangleVertexes. Each edge goes from the origin.
	/// Cross(edge0, edge1)  gives the triangleVertexes normal.
	/// </summary>
	public struct MyTriangle
	{
		private Vector3 origin;

		private Vector3 edge0;

		private Vector3 edge1;

		public Vector3 Centre => origin + 0.333333343f * (edge0 + edge1);

		public Vector3 Origin
		{
			get
			{
				return origin;
			}
			set
			{
				origin = value;
			}
		}

		public Vector3 Edge0
		{
			get
			{
				return edge0;
			}
			set
			{
				edge0 = value;
			}
		}

		public Vector3 Edge1
		{
			get
			{
				return edge1;
			}
			set
			{
				edge1 = value;
			}
		}

		/// <summary>
		/// Edge2 goes from pt1 to pt2
		/// </summary>
		public Vector3 Edge2 => edge1 - edge0;

		/// <summary>
		/// Gets the plane containing the triangleVertexes
		/// </summary>
		public Plane Plane => new Plane(GetPoint(0), GetPoint(1), GetPoint(2));

		/// <summary>
		/// Gets the triangleVertexes normal. If degenerate it will be normalised, but
		/// the direction may be wrong!
		/// </summary>
		public Vector3 Normal => Vector3.Normalize(Vector3.Cross(MyUtils.Normalize(edge0), MyUtils.Normalize(edge1)));

		/// <summary>
		/// Points specified so that pt1-pt0 is edge0 and p2-pt0 is edge1
		/// </summary>
		/// <param name="pt0"></param>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		public MyTriangle(Vector3 pt0, Vector3 pt1, Vector3 pt2)
		{
			origin = pt0;
			edge0 = pt1 - pt0;
			edge1 = pt2 - pt0;
		}

		/// <summary>
		/// Points specified so that pt1-pt0 is edge0 and p2-pt0 is edge1
		/// </summary>
		/// <param name="pt0"></param>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		public MyTriangle(ref Vector3 pt0, ref Vector3 pt1, ref Vector3 pt2)
		{
			origin = pt0;
			edge0 = pt1 - pt0;
			edge1 = pt2 - pt0;
		}

		/// <summary>
		/// Same numbering as in the constructor
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public Vector3 GetPoint(int i)
		{
			return i switch
			{
				1 => origin + edge0, 
				2 => origin + edge1, 
				_ => origin, 
			};
		}

		/// <summary>
		/// Same numbering as in the constructor
		/// </summary>
		/// <param name="i"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		public void GetPoint(int i, out Vector3 point)
		{
			switch (i)
			{
			case 1:
				point = origin + edge0;
				break;
			case 2:
				point = origin + edge1;
				break;
			default:
				point = origin;
				break;
			}
		}

		/// BEN-OPTIMISATION: New method with ref point, also accounts for the bug fix.
		/// <summary>
		/// Same numbering as in the constructor
		/// </summary>
		/// <param name="point"></param>
		/// <param name="i"></param>
		/// <returns></returns>
		public void GetPoint(ref Vector3 point, int i)
		{
			switch (i)
			{
			case 1:
				point.X = origin.X + edge0.X;
				point.Y = origin.Y + edge0.Y;
				point.Z = origin.Z + edge0.Z;
				break;
			case 2:
				point.X = origin.X + edge1.X;
				point.Y = origin.Y + edge1.Y;
				point.Z = origin.Z + edge1.Z;
				break;
			default:
				point.X = origin.X;
				point.Y = origin.Y;
				point.Z = origin.Z;
				break;
			}
		}

		/// <summary>
		/// Returns the point parameterised by t0 and t1
		/// </summary>
		/// <param name="t0"></param>
		/// <param name="t1"></param>
		/// <returns></returns>
		public Vector3 GetPoint(float t0, float t1)
		{
			return origin + t0 * edge0 + t1 * edge1;
		}

		/// <summary>
		/// Gets the minimum and maximum extents of the triangleVertexes along the axis
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="axis"></param>
		public void GetSpan(out float min, out float max, Vector3 axis)
		{
			float a = Vector3.Dot(GetPoint(0), axis);
			float b = Vector3.Dot(GetPoint(1), axis);
			float c = Vector3.Dot(GetPoint(2), axis);
			min = MathHelper.Min(a, b, c);
			max = MathHelper.Max(a, b, c);
		}

		/// <summary>
		/// Gets the minimum and maximum extents of the triangle along the axis
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="axis"></param>
		public void GetSpan(out float min, out float max, ref Vector3 axis)
		{
			Vector3 point = default(Vector3);
			GetPoint(ref point, 0);
			float a = point.X * axis.X + point.Y * axis.Y + point.Z * axis.Z;
			GetPoint(ref point, 1);
			float b = point.X * axis.X + point.Y * axis.Y + point.Z * axis.Z;
			GetPoint(ref point, 2);
			float c = point.X * axis.X + point.Y * axis.Y + point.Z * axis.Z;
			min = MathHelper.Min(a, b, c);
			max = MathHelper.Max(a, b, c);
		}
	}
}
