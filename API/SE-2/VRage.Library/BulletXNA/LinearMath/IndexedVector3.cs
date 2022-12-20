namespace BulletXNA.LinearMath
{
	public struct IndexedVector3
	{
		private static IndexedVector3 _zero = default(IndexedVector3);

		private static IndexedVector3 _one = new IndexedVector3(1f);

		private static IndexedVector3 _up = new IndexedVector3(0f, 1f, 0f);

		public float X;

		public float Y;

		public float Z;

		public float this[int i]
		{
			get
			{
<<<<<<< HEAD
				switch (i)
				{
				case 0:
					return X;
				case 1:
					return Y;
				case 2:
					return Z;
				default:
					return 0f;
				}
=======
				return i switch
				{
					0 => X, 
					1 => Y, 
					2 => Z, 
					_ => 0f, 
				};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			set
			{
				switch (i)
				{
				case 0:
					X = value;
					break;
				case 1:
					Y = value;
					break;
				case 2:
					Z = value;
					break;
				}
			}
		}

		public static IndexedVector3 Zero => _zero;

		public IndexedVector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public IndexedVector3(float x)
		{
			X = x;
			Y = x;
			Z = x;
		}

		public IndexedVector3(IndexedVector3 v)
		{
			X = v.X;
			Y = v.Y;
			Z = v.Z;
		}

		public static IndexedVector3 operator +(IndexedVector3 value1, IndexedVector3 value2)
		{
			IndexedVector3 result = default(IndexedVector3);
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
			result.Z = value1.Z + value2.Z;
			return result;
		}

		public static IndexedVector3 operator -(IndexedVector3 value1, IndexedVector3 value2)
		{
			IndexedVector3 result = default(IndexedVector3);
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
			result.Z = value1.Z - value2.Z;
			return result;
		}

		public static IndexedVector3 operator *(IndexedVector3 value, float scaleFactor)
		{
			IndexedVector3 result = default(IndexedVector3);
			result.X = value.X * scaleFactor;
			result.Y = value.Y * scaleFactor;
			result.Z = value.Z * scaleFactor;
			return result;
		}

		public static IndexedVector3 operator *(float scaleFactor, IndexedVector3 value)
		{
			IndexedVector3 result = default(IndexedVector3);
			result.X = value.X * scaleFactor;
			result.Y = value.Y * scaleFactor;
			result.Z = value.Z * scaleFactor;
			return result;
		}

		public static IndexedVector3 operator -(IndexedVector3 value)
		{
			IndexedVector3 result = default(IndexedVector3);
			result.X = 0f - value.X;
			result.Y = 0f - value.Y;
			result.Z = 0f - value.Z;
			return result;
		}

		public static IndexedVector3 operator *(IndexedVector3 value1, IndexedVector3 value2)
		{
			IndexedVector3 result = default(IndexedVector3);
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
			result.Z = value1.Z * value2.Z;
			return result;
		}

		public static IndexedVector3 operator /(IndexedVector3 value1, IndexedVector3 value2)
		{
			IndexedVector3 result = default(IndexedVector3);
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
			result.Z = value1.Z / value2.Z;
			return result;
		}

		public bool Equals(IndexedVector3 other)
		{
			if (X == other.X && Y == other.Y)
			{
				return Z == other.Z;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is IndexedVector3)
			{
				result = Equals((IndexedVector3)obj);
			}
			return result;
		}

		public float Dot(ref IndexedVector3 v)
		{
			return X * v.X + Y * v.Y + Z * v.Z;
		}

		public float Dot(IndexedVector3 v)
		{
			return X * v.X + Y * v.Y + Z * v.Z;
		}

		public override int GetHashCode()
		{
			int hashCode = X.GetHashCode();
			hashCode = (hashCode * 397) ^ Y.GetHashCode();
			return (hashCode * 397) ^ Z.GetHashCode();
		}
	}
}
