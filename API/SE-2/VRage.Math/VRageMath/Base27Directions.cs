using System;

namespace VRageMath
{
	/// <summary>
	/// Base 26 directions and Vector3.Zero
	/// Each component is only 0,-1 or 1;
	/// </summary>
	public class Base27Directions
	{
		/// <summary>
		///
		/// </summary>
		[Flags]
		public enum Direction : byte
		{
			/// <summary>
			///
			/// </summary>
			Forward = 0x1,
			/// <summary>
			///
			/// </summary>
			Backward = 0x2,
			/// <summary>
			///
			/// </summary>
			Left = 0x4,
			/// <summary>
			///
			/// </summary>
			Right = 0x8,
			/// <summary>
			///
			/// </summary>
			Up = 0x10,
			/// <summary>
			///
			/// </summary>
			Down = 0x20
		}

		/// <summary>
		///
		/// </summary>
		public static readonly Vector3[] Directions = new Vector3[64]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, 1f),
			new Vector3(0f, 0f, 0f),
			new Vector3(-1f, 0f, 0f),
			new Vector3(-0.7071068f, 0f, -0.7071068f),
			new Vector3(-0.7071068f, 0f, 0.7071068f),
			new Vector3(-1f, 0f, 0f),
			new Vector3(1f, 0f, 0f),
			new Vector3(0.7071068f, 0f, -0.7071068f),
			new Vector3(0.7071068f, 0f, 0.7071068f),
			new Vector3(1f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, 1f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 1f, 0f),
			new Vector3(0f, 0.7071068f, -0.7071068f),
			new Vector3(0f, 0.7071068f, 0.7071068f),
			new Vector3(0f, 1f, 0f),
			new Vector3(-0.7071068f, 0.7071068f, 0f),
			new Vector3(-0.5773503f, 0.5773503f, -0.5773503f),
			new Vector3(-0.5773503f, 0.5773503f, 0.5773503f),
			new Vector3(-0.7071068f, 0.7071068f, 0f),
			new Vector3(0.7071068f, 0.7071068f, 0f),
			new Vector3(0.5773503f, 0.5773503f, -0.5773503f),
			new Vector3(0.5773503f, 0.5773503f, 0.5773503f),
			new Vector3(0.7071068f, 0.7071068f, 0f),
			new Vector3(0f, 1f, 0f),
			new Vector3(0f, 0.7071068f, -0.7071068f),
			new Vector3(0f, 0.7071068f, 0.7071068f),
			new Vector3(0f, 1f, 0f),
			new Vector3(0f, -1f, 0f),
			new Vector3(0f, -0.7071068f, -0.7071068f),
			new Vector3(0f, -0.7071068f, 0.7071068f),
			new Vector3(0f, -1f, 0f),
			new Vector3(-0.7071068f, -0.7071068f, 0f),
			new Vector3(-0.5773503f, -0.5773503f, -0.5773503f),
			new Vector3(-0.5773503f, -0.5773503f, 0.5773503f),
			new Vector3(-0.7071068f, -0.7071068f, 0f),
			new Vector3(0.7071068f, -0.7071068f, 0f),
			new Vector3(0.5773503f, -0.5773503f, -0.5773503f),
			new Vector3(0.5773503f, -0.5773503f, 0.5773503f),
			new Vector3(0.7071068f, -0.7071068f, 0f),
			new Vector3(0f, -1f, 0f),
			new Vector3(0f, -0.7071068f, -0.7071068f),
			new Vector3(0f, -0.7071068f, 0.7071068f),
			new Vector3(0f, -1f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, 1f),
			new Vector3(0f, 0f, 0f),
			new Vector3(-1f, 0f, 0f),
			new Vector3(-0.7071068f, 0f, -0.7071068f),
			new Vector3(-0.7071068f, 0f, 0.7071068f),
			new Vector3(-1f, 0f, 0f),
			new Vector3(1f, 0f, 0f),
			new Vector3(0.7071068f, 0f, -0.7071068f),
			new Vector3(0.7071068f, 0f, 0.7071068f),
			new Vector3(1f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, 1f),
			new Vector3(0f, 0f, 0f)
		};

		/// <summary>
		///
		/// </summary>
		public static readonly Vector3I[] DirectionsInt = new Vector3I[64]
		{
			new Vector3I(0, 0, 0),
			new Vector3I(0, 0, -1),
			new Vector3I(0, 0, 1),
			new Vector3I(0, 0, 0),
			new Vector3I(-1, 0, 0),
			new Vector3I(-1, 0, -1),
			new Vector3I(-1, 0, 1),
			new Vector3I(-1, 0, 0),
			new Vector3I(1, 0, 0),
			new Vector3I(1, 0, -1),
			new Vector3I(1, 0, 1),
			new Vector3I(1, 0, 0),
			new Vector3I(0, 0, 0),
			new Vector3I(0, 0, -1),
			new Vector3I(0, 0, 1),
			new Vector3I(0, 0, 0),
			new Vector3I(0, 1, 0),
			new Vector3I(0, 1, -1),
			new Vector3I(0, 1, 1),
			new Vector3I(0, 1, 0),
			new Vector3I(-1, 1, 0),
			new Vector3I(-1, 1, -1),
			new Vector3I(-1, 1, 1),
			new Vector3I(-1, 1, 0),
			new Vector3I(1, 1, 0),
			new Vector3I(1, 1, -1),
			new Vector3I(1, 1, 1),
			new Vector3I(1, 1, 0),
			new Vector3I(0, 1, 0),
			new Vector3I(0, 1, -1),
			new Vector3I(0, 1, 1),
			new Vector3I(0, 1, 0),
			new Vector3I(0, -1, 0),
			new Vector3I(0, -1, -1),
			new Vector3I(0, -1, 1),
			new Vector3I(0, -1, 0),
			new Vector3I(-1, -1, 0),
			new Vector3I(-1, -1, -1),
			new Vector3I(-1, -1, 1),
			new Vector3I(-1, -1, 0),
			new Vector3I(1, -1, 0),
			new Vector3I(1, -1, -1),
			new Vector3I(1, -1, 1),
			new Vector3I(1, -1, 0),
			new Vector3I(0, -1, 0),
			new Vector3I(0, -1, -1),
			new Vector3I(0, -1, 1),
			new Vector3I(0, -1, 0),
			new Vector3I(0, 0, 0),
			new Vector3I(0, 0, -1),
			new Vector3I(0, 0, 1),
			new Vector3I(0, 0, 0),
			new Vector3I(-1, 0, 0),
			new Vector3I(-1, 0, -1),
			new Vector3I(-1, 0, 1),
			new Vector3I(-1, 0, 0),
			new Vector3I(1, 0, 0),
			new Vector3I(1, 0, -1),
			new Vector3I(1, 0, 1),
			new Vector3I(1, 0, 0),
			new Vector3I(0, 0, 0),
			new Vector3I(0, 0, -1),
			new Vector3I(0, 0, 1),
			new Vector3I(0, 0, 0)
		};

		private const float DIRECTION_EPSILON = 1E-05f;

		private static readonly int[] ForwardBackward = new int[3] { 1, 0, 2 };

		private static readonly int[] LeftRight = new int[3] { 4, 0, 8 };

		private static readonly int[] UpDown = new int[3] { 32, 0, 16 };

		/// <summary>
		///
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static bool IsBaseDirection(ref Vector3 vec)
		{
			return vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z - 1f < 1E-05f;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static bool IsBaseDirection(ref Vector3I vec)
		{
			if (vec.X >= -1 && vec.X <= 1 && vec.Y >= -1 && vec.Y <= 1 && vec.Z >= -1)
			{
				return vec.Z <= 1;
			}
			return false;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static bool IsBaseDirection(Vector3 vec)
		{
			return IsBaseDirection(ref vec);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Vector3 GetVector(int direction)
		{
			return Directions[direction];
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Vector3I GetVectorInt(int direction)
		{
			return DirectionsInt[direction];
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		public static Vector3 GetVector(Direction dir)
		{
			return Directions[(uint)dir];
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		public static Vector3I GetVectorInt(Direction dir)
		{
			return DirectionsInt[(uint)dir];
		}

		/// <summary>
		/// Vector must be normalized, allowed values for components are: 0, 1, -1, 0.707, -0.707, 0.577, -0.577
		/// </summary>
		public static Direction GetDirection(Vector3 vec)
		{
			return GetDirection(ref vec);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static Direction GetDirection(Vector3I vec)
		{
			return GetDirection(ref vec);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static Direction GetDirection(ref Vector3 vec)
		{
			return (Direction)(0 + ForwardBackward[(int)Math.Round(vec.Z + 1f)] + LeftRight[(int)Math.Round(vec.X + 1f)] + UpDown[(int)Math.Round(vec.Y + 1f)]);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static Direction GetDirection(ref Vector3I vec)
		{
			return (Direction)(0 + ForwardBackward[vec.Z + 1] + LeftRight[vec.X + 1] + UpDown[vec.Y + 1]);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="rot"></param>
		/// <returns></returns>
		public static Direction GetForward(ref Quaternion rot)
		{
			Vector3.Transform(ref Vector3.Forward, ref rot, out var result);
			return GetDirection(ref result);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="rot"></param>
		/// <returns></returns>
		public static Direction GetUp(ref Quaternion rot)
		{
			Vector3.Transform(ref Vector3.Up, ref rot, out var result);
			return GetDirection(ref result);
		}
	}
}
