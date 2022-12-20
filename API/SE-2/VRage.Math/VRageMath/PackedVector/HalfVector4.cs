using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath.PackedVector
{
	/// <summary>
	/// Packed vector type containing four 16-bit floating-point values.
	/// </summary>
	[Serializable]
	public struct HalfVector4 : IPackedVector<ulong>, IPackedVector, IEquatable<HalfVector4>
	{
		protected class VRageMath_PackedVector_HalfVector4_003C_003EPackedValue_003C_003EAccessor : IMemberAccessor<HalfVector4, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref HalfVector4 owner, in ulong value)
			{
				owner.PackedValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref HalfVector4 owner, out ulong value)
			{
				value = owner.PackedValue;
			}
		}

		protected class VRageMath_PackedVector_HalfVector4_003C_003EVRageMath_002EPackedVector_002EIPackedVector_003CSystem_002EUInt64_003E_002EPackedValue_003C_003EAccessor : IMemberAccessor<HalfVector4, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref HalfVector4 owner, in ulong value)
			{
				owner.VRageMath_002EPackedVector_002EIPackedVector_003CSystem_002EUInt64_003E_002EPackedValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref HalfVector4 owner, out ulong value)
			{
				value = owner.VRageMath_002EPackedVector_002EIPackedVector_003CSystem_002EUInt64_003E_002EPackedValue;
			}
		}

		public ulong PackedValue;

		ulong IPackedVector<ulong>.PackedValue
		{
			get
			{
				return PackedValue;
			}
			set
			{
				PackedValue = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the HalfVector4 class.
		/// </summary>
		/// <param name="x">Initial value for the x component.</param><param name="y">Initial value for the y component.</param><param name="z">Initial value for the z component.</param><param name="w">Initial value for the w component.</param>
		public HalfVector4(float x, float y, float z, float w)
		{
			PackedValue = PackHelper(x, y, z, w);
		}

		/// <summary>
		/// Initializes a new instance of the HalfVector4 structure.
		/// </summary>
		/// <param name="vector">A vector containing the initial values for the components of the HalfVector4 structure.</param>
		public HalfVector4(Vector4 vector)
		{
			PackedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
		}

		public HalfVector4(HalfVector3 vector3, ushort w)
		{
			PackedValue = vector3.ToHalfVector4().PackedValue | ((ulong)w << 48);
		}

		public HalfVector4(ulong packedValue)
		{
			PackedValue = packedValue;
		}

		/// <summary>
		/// Compares the current instance of a class to another instance to determine whether they are the same.
		/// </summary>
		/// <param name="a">The object to the left of the equality operator.</param><param name="b">The object to the right of the equality operator.</param>
		public static bool operator ==(HalfVector4 a, HalfVector4 b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Compares the current instance of a class to another instance to determine whether they are different.
		/// </summary>
		/// <param name="a">The object to the left of the equality operator.</param><param name="b">The object to the right of the equality operator.</param>
		public static bool operator !=(HalfVector4 a, HalfVector4 b)
		{
			return !a.Equals(b);
		}

		void IPackedVector.PackFromVector4(Vector4 vector)
		{
			PackedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
		}

		private static ulong PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW)
		{
			return HalfUtils.Pack(vectorX) | ((ulong)HalfUtils.Pack(vectorY) << 16) | ((ulong)HalfUtils.Pack(vectorZ) << 32) | ((ulong)HalfUtils.Pack(vectorW) << 48);
		}

		/// <summary>
		/// Expands the packed representation into a Vector4.
		/// </summary>
		public Vector4 ToVector4()
		{
			Vector4 result = default(Vector4);
			result.X = HalfUtils.Unpack((ushort)PackedValue);
			result.Y = HalfUtils.Unpack((ushort)(PackedValue >> 16));
			result.Z = HalfUtils.Unpack((ushort)(PackedValue >> 32));
			result.W = HalfUtils.Unpack((ushort)(PackedValue >> 48));
			return result;
		}

		/// <summary>
		/// Returns a string representation of the current instance.
		/// </summary>
		public override string ToString()
		{
			return ToVector4().ToString();
		}

		/// <summary>
		/// Gets the hash code for the current instance.
		/// </summary>
		public override int GetHashCode()
		{
			return PackedValue.GetHashCode();
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">The object with which to make the comparison.</param>
		public override bool Equals(object obj)
		{
			if (obj is HalfVector4)
			{
				return Equals((HalfVector4)obj);
			}
			return false;
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object.
		/// </summary>
		/// <param name="other">The object with which to make the comparison.</param>
		public bool Equals(HalfVector4 other)
		{
			return PackedValue.Equals(other.PackedValue);
		}
	}
}
