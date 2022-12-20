using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Utils;
using VRageMath;

namespace VRage
{
	public class MyDebugUtils
	{
		private static readonly ConcurrentDictionary<MemberInfo, string> m_debugNames = new ConcurrentDictionary<MemberInfo, string>();

		/// <summary>
		/// Returns true if float is valid
		/// </summary>
		/// <param name="f"></param>
		/// <returns></returns>
		public static bool IsValid(float f)
		{
			if (!float.IsNaN(f))
			{
				return !float.IsInfinity(f);
			}
			return false;
		}

		public static bool IsValid(double d)
		{
			if (!double.IsNaN(d))
			{
				return !double.IsInfinity(d);
			}
			return false;
		}

		/// <summary>
		/// Returns true if Vector3 is valid
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static bool IsValid(Vector3 vec)
		{
			if (IsValid(vec.X) && IsValid(vec.Y))
			{
				return IsValid(vec.Z);
			}
			return false;
		}

		public static bool IsValid(Vector3? vec)
		{
			if (vec.HasValue)
			{
				if (IsValid(vec.Value.X) && IsValid(vec.Value.Y))
				{
					return IsValid(vec.Value.Z);
				}
				return false;
			}
			return true;
		}

		public static bool IsValid(Vector3D vec)
		{
			if (IsValid(vec.X) && IsValid(vec.Y))
			{
				return IsValid(vec.Z);
			}
			return false;
		}

		public static bool IsValid(Vector3D? vec)
		{
			if (vec.HasValue)
			{
				if (IsValid(vec.Value.X) && IsValid(vec.Value.Y))
				{
					return IsValid(vec.Value.Z);
				}
				return false;
			}
			return true;
		}

		public static bool IsValidNormal(Vector3 vec)
		{
			float num = vec.LengthSquared();
			if (IsValid(vec) && num > 0.999f)
			{
				return num < 1.001f;
			}
			return false;
		}

		/// <summary>
		/// Returns true if Vector2 is valid
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static bool IsValid(Vector2 vec)
		{
			if (IsValid(vec.X))
			{
				return IsValid(vec.Y);
			}
			return false;
		}

		public static bool IsValid(Matrix matrix)
		{
			if (IsValid(matrix.Up) && IsValid(matrix.Left) && IsValid(matrix.Forward) && IsValid(matrix.Translation))
			{
				return matrix != Matrix.Zero;
			}
			return false;
		}

		public static bool IsValid(Quaternion q)
		{
			if (IsValid(q.X) && IsValid(q.Y) && IsValid(q.Z) && IsValid(q.W))
			{
				return !MyUtils.IsZero(q);
			}
			return false;
		}

		/// <summary>
		/// Returns true if Vector3 is valid
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		[Conditional("DEBUG")]
		public static void AssertIsValid(Vector3D vec)
		{
		}

		/// <summary>
		/// Returns true if Vector3 is valid
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		[Conditional("DEBUG")]
		public static void AssertIsValid(Vector3D? vec)
		{
		}

		/// <summary>
		/// Returns true if Vector3 is valid
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		[Conditional("DEBUG")]
		public static void AssertIsValid(Vector3 vec)
		{
		}

		/// <summary>
		/// Returns true if Vector3 is valid
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		[Conditional("DEBUG")]
		public static void AssertIsValid(Vector3? vec)
		{
		}

		/// <summary>
		/// Returns true if Vector2 is valid
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		[Conditional("DEBUG")]
		public static void AssertIsValid(Vector2 vec)
		{
		}

		/// <summary>
		/// Returns true if float is valid
		/// </summary>
		/// <param name="f"></param>
		/// <returns></returns>
		[Conditional("DEBUG")]
		public static void AssertIsValid(float f)
		{
		}

		[Conditional("DEBUG")]
		public static void AssertIsValid(Matrix matrix)
		{
		}

		[Conditional("DEBUG")]
		public static void AssertIsValid(Quaternion q)
		{
		}

		/// <summary>
		/// Determine a user friendly debug name for a method or type.
		/// </summary>
		/// <param name="member"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetDebugName(MemberInfo member)
		{
<<<<<<< HEAD
			if (!m_debugNames.TryGetValue(member, out var value))
			{
				m_debugNames.TryAdd(member, value = member.Name);
			}
			return value;
=======
			string name = default(string);
			if (!m_debugNames.TryGetValue(member, ref name))
			{
				m_debugNames.TryAdd(member, name = member.Name);
			}
			return name;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		/// <summary>
		/// Determine a user friendly debug name for a method or type.
		/// </summary>
<<<<<<< HEAD
		/// <param name="object"></param>
=======
		/// <param name="member"></param>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetDebugName(object @object)
		{
			return GetDebugName(@object.GetType());
		}
	}
}
