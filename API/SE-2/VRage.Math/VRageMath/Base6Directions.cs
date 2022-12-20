using System;

namespace VRageMath
{
	/// <summary>
	/// Workaround because .NET XML serializer is stupid and does not like enum inside static class 
	/// </summary>
	public class Base6Directions
	{
		/// <summary>
		///
		/// </summary>
		public enum Direction : byte
		{
			/// <summary>
			///
			/// </summary>
			Forward,
			/// <summary>
			///
			/// </summary>
			Backward,
			/// <summary>
			///
			/// </summary>
			Left,
			/// <summary>
			///
			/// </summary>
			Right,
			/// <summary>
			///
			/// </summary>
			Up,
			/// <summary>
			///
			/// </summary>
			Down
		}

		/// <summary>
		///
		/// </summary>
		[Flags]
		public enum DirectionFlags : byte
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
			Down = 0x20,
			/// <summary>
			///
			/// </summary>
			All = 0x3F
		}

		/// <summary>
		///
		/// </summary>
		public enum Axis : byte
		{
			/// <summary>
			///
			/// </summary>
			ForwardBackward,
			/// <summary>
			///
			/// </summary>
			LeftRight,
			/// <summary>
			///
			/// </summary>
			UpDown
		}

		/// <summary>
		/// Because Enum.GetValues(...) returns array of objects
		/// </summary>
		public static readonly Direction[] EnumDirections = new Direction[6]
		{
			Direction.Forward,
			Direction.Backward,
			Direction.Left,
			Direction.Right,
			Direction.Up,
			Direction.Down
		};

		/// <summary>
		///
		/// </summary>
		public static readonly Vector3[] Directions = new Vector3[6]
		{
			Vector3.Forward,
			Vector3.Backward,
			Vector3.Left,
			Vector3.Right,
			Vector3.Up,
			Vector3.Down
		};

		/// <summary>
		///
		/// </summary>
		public static readonly Vector3I[] IntDirections = new Vector3I[6]
		{
			Vector3I.Forward,
			Vector3I.Backward,
			Vector3I.Left,
			Vector3I.Right,
			Vector3I.Up,
			Vector3I.Down
		};

		/// <summary>
		/// Pre-calculated left directions for given forward (index / 6) and up (index % 6) directions
		/// </summary>
		private static readonly Direction[] LeftDirections = new Direction[36]
		{
			Direction.Forward,
			Direction.Forward,
			Direction.Down,
			Direction.Up,
			Direction.Left,
			Direction.Right,
			Direction.Forward,
			Direction.Forward,
			Direction.Up,
			Direction.Down,
			Direction.Right,
			Direction.Left,
			Direction.Up,
			Direction.Down,
			Direction.Left,
			Direction.Left,
			Direction.Backward,
			Direction.Forward,
			Direction.Down,
			Direction.Up,
			Direction.Left,
			Direction.Left,
			Direction.Forward,
			Direction.Backward,
			Direction.Right,
			Direction.Left,
			Direction.Forward,
			Direction.Backward,
			Direction.Left,
			Direction.Right,
			Direction.Left,
			Direction.Right,
			Direction.Backward,
			Direction.Forward,
			Direction.Left,
			Direction.Right
		};

		private const float DIRECTION_EPSILON = 1E-05f;

		private static readonly int[] ForwardBackward = new int[3] { 0, 0, 1 };

		private static readonly int[] LeftRight = new int[3] { 2, 0, 3 };

		private static readonly int[] UpDown = new int[3] { 5, 0, 4 };

		private Base6Directions()
		{
		}

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
		public static bool IsBaseDirection(Vector3 vec)
		{
			return IsBaseDirection(ref vec);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static bool IsBaseDirection(ref Vector3I vec)
		{
			return vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z - 1 == 0;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Vector3 GetVector(int direction)
		{
			direction %= 6;
			return Directions[direction];
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		public static Vector3 GetVector(Direction dir)
		{
			return GetVector((int)dir);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Vector3I GetIntVector(int direction)
		{
			direction %= 6;
			return IntDirections[direction];
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		public static Vector3I GetIntVector(Direction dir)
		{
			int num = (int)dir % 6;
			return IntDirections[num];
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="dir"></param>
		/// <param name="result"></param>
		public static void GetVector(Direction dir, out Vector3 result)
		{
			int num = (int)dir % 6;
			result = Directions[num];
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		public static DirectionFlags GetDirectionFlag(Direction dir)
		{
			return (DirectionFlags)(1 << (int)dir);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		public static Direction GetPerpendicular(Direction dir)
		{
			if (GetAxis(dir) == Axis.UpDown)
			{
				return Direction.Right;
			}
			return Direction.Up;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static Direction GetDirection(Vector3 vec)
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
		public static Direction GetDirection(Vector3I vec)
		{
			return GetDirection(ref vec);
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
		/// <param name="vec"></param>
		/// <returns></returns>
		public static Direction GetClosestDirection(Vector3 vec)
		{
			return GetClosestDirection(ref vec);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static Direction GetClosestDirection(ref Vector3 vec)
		{
			Vector3 value = Vector3.DominantAxisProjection(vec);
			value = Vector3.Sign(value);
			return GetDirection(ref value);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="vec"></param>
		/// <param name="axis"></param>
		/// <returns></returns>
		public static Direction GetDirectionInAxis(Vector3 vec, Axis axis)
		{
			return GetDirectionInAxis(ref vec, axis);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="vec"></param>
		/// <param name="axis"></param>
		/// <returns></returns>
		public static Direction GetDirectionInAxis(ref Vector3 vec, Axis axis)
		{
			Direction baseAxisDirection = GetBaseAxisDirection(axis);
			Vector3 vector = IntDirections[(uint)baseAxisDirection];
			vector *= vec;
			if (vector.X + vector.Y + vector.Z >= 1f)
			{
				return baseAxisDirection;
			}
			return GetFlippedDirection(baseAxisDirection);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="rot"></param>
		/// <returns></returns>
		public static Direction GetForward(Quaternion rot)
		{
			Vector3.Transform(ref Vector3.Forward, ref rot, out var result);
			return GetDirection(ref result);
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
		/// <param name="rotation"></param>
		/// <returns></returns>
		public static Direction GetForward(ref Matrix rotation)
		{
			Vector3.TransformNormal(ref Vector3.Forward, ref rotation, out var result);
			return GetDirection(ref result);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="rot"></param>
		/// <returns></returns>
		public static Direction GetUp(Quaternion rot)
		{
			Vector3.Transform(ref Vector3.Up, ref rot, out var result);
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

		/// <summary>
		///
		/// </summary>
		/// <param name="rotation"></param>
		/// <returns></returns>
		public static Direction GetUp(ref Matrix rotation)
		{
			Vector3.TransformNormal(ref Vector3.Up, ref rotation, out var result);
			return GetDirection(ref result);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Axis GetAxis(Direction direction)
		{
			return (Axis)((int)direction >> 1);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public static Direction GetBaseAxisDirection(Axis axis)
		{
			return (Direction)((uint)axis << 1);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toFlip"></param>
		/// <returns></returns>
		public static Direction GetFlippedDirection(Direction toFlip)
		{
			return toFlip ^ Direction.Backward;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="dir1"></param>
		/// <param name="dir2"></param>
		/// <returns></returns>
		public static Direction GetCross(Direction dir1, Direction dir2)
		{
			return GetLeft(dir1, dir2);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="up"></param>
		/// <param name="forward"></param>
		/// <returns></returns>
		public static Direction GetLeft(Direction up, Direction forward)
		{
			return LeftDirections[(int)forward * 6 + (int)up];
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		public static Direction GetOppositeDirection(Direction dir)
		{
			return dir switch
			{
				Direction.Backward => Direction.Forward, 
				Direction.Up => Direction.Down, 
				Direction.Down => Direction.Up, 
				Direction.Left => Direction.Right, 
				Direction.Right => Direction.Left, 
				_ => Direction.Backward, 
			};
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="forward"></param>
		/// <param name="up"></param>
		/// <returns></returns>
		public static Quaternion GetOrientation(Direction forward, Direction up)
		{
			Vector3 vector = GetVector(forward);
			Vector3 vector2 = GetVector(up);
			return Quaternion.CreateFromForwardUp(vector, vector2);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="forward"></param>
		/// <param name="up"></param>
		/// <returns></returns>
		public static bool IsValidBlockOrientation(Direction forward, Direction up)
		{
			if ((int)forward <= 5 && (int)up <= 5)
			{
				return Vector3.Dot(GetVector(forward), GetVector(up)) == 0f;
			}
			return false;
		}
	}
}
