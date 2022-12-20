namespace BulletXNA.LinearMath
{
	public struct IndexedBasisMatrix
	{
		public IndexedVector3 _Row0;

		public IndexedVector3 _Row1;

		public IndexedVector3 _Row2;

		public float this[int i, int j]
		{
			get
			{
				switch (i)
				{
				case 0:
					switch (j)
					{
					case 0:
						return _Row0.X;
					case 1:
						return _Row0.Y;
					case 2:
						return _Row0.Z;
					}
					break;
				case 1:
					switch (j)
					{
					case 0:
						return _Row1.X;
					case 1:
						return _Row1.Y;
					case 2:
						return _Row1.Z;
					}
					break;
				case 2:
					switch (j)
					{
					case 0:
						return _Row2.X;
					case 1:
						return _Row2.Y;
					case 2:
						return _Row2.Z;
					}
					break;
				}
				return 0f;
			}
			set
			{
				switch (i)
				{
				case 0:
					switch (j)
					{
					case 0:
						_Row0.X = value;
						break;
					case 1:
						_Row0.Y = value;
						break;
					case 2:
						_Row0.Z = value;
						break;
					}
					break;
				case 1:
					switch (j)
					{
					case 0:
						_Row1.X = value;
						break;
					case 1:
						_Row1.Y = value;
						break;
					case 2:
						_Row1.Z = value;
						break;
					}
					break;
				case 2:
					switch (j)
					{
					case 0:
						_Row2.X = value;
						break;
					case 1:
						_Row2.Y = value;
						break;
					case 2:
						_Row2.Z = value;
						break;
					}
					break;
				}
			}
		}

		public IndexedVector3 this[int i]
		{
			get
			{
<<<<<<< HEAD
				switch (i)
				{
				case 0:
					return _Row0;
				case 1:
					return _Row1;
				case 2:
					return _Row2;
				default:
					return IndexedVector3.Zero;
				}
=======
				return i switch
				{
					0 => _Row0, 
					1 => _Row1, 
					2 => _Row2, 
					_ => IndexedVector3.Zero, 
				};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			set
			{
				switch (i)
				{
				case 0:
					_Row0 = value;
					break;
				case 1:
					_Row1 = value;
					break;
				case 2:
					_Row2 = value;
					break;
				}
			}
		}

		public IndexedBasisMatrix(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
		{
			_Row0 = new IndexedVector3(m11, m12, m13);
			_Row1 = new IndexedVector3(m21, m22, m23);
			_Row2 = new IndexedVector3(m31, m32, m33);
		}

		public IndexedVector3 GetRow(int i)
		{
<<<<<<< HEAD
			switch (i)
			{
			case 0:
				return _Row0;
			case 1:
				return _Row1;
			case 2:
				return _Row2;
			default:
				return IndexedVector3.Zero;
			}
=======
			return i switch
			{
				0 => _Row0, 
				1 => _Row1, 
				2 => _Row2, 
				_ => IndexedVector3.Zero, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static IndexedVector3 operator *(IndexedBasisMatrix m, IndexedVector3 v)
		{
			return new IndexedVector3(m._Row0.Dot(ref v), m._Row1.Dot(ref v), m._Row2.Dot(ref v));
		}

		public static IndexedBasisMatrix operator *(IndexedBasisMatrix m1, IndexedBasisMatrix m2)
		{
			return new IndexedBasisMatrix(m2.TDotX(ref m1._Row0), m2.TDotY(ref m1._Row0), m2.TDotZ(ref m1._Row0), m2.TDotX(ref m1._Row1), m2.TDotY(ref m1._Row1), m2.TDotZ(ref m1._Row1), m2.TDotX(ref m1._Row2), m2.TDotY(ref m1._Row2), m2.TDotZ(ref m1._Row2));
		}

		public float TDotX(ref IndexedVector3 v)
		{
			return _Row0.X * v.X + _Row1.X * v.Y + _Row2.X * v.Z;
		}

		public float TDotY(ref IndexedVector3 v)
		{
			return _Row0.Y * v.X + _Row1.Y * v.Y + _Row2.Y * v.Z;
		}

		public float TDotZ(ref IndexedVector3 v)
		{
			return _Row0.Z * v.X + _Row1.Z * v.Y + _Row2.Z * v.Z;
		}

		public IndexedBasisMatrix Inverse()
		{
			IndexedVector3 v = new IndexedVector3(Cofac(1, 1, 2, 2), Cofac(1, 2, 2, 0), Cofac(1, 0, 2, 1));
			float num = this[0].Dot(v);
			float num2 = 1f / num;
			return new IndexedBasisMatrix(v.X * num2, Cofac(0, 2, 2, 1) * num2, Cofac(0, 1, 1, 2) * num2, v.Y * num2, Cofac(0, 0, 2, 2) * num2, Cofac(0, 2, 1, 0) * num2, v.Z * num2, Cofac(0, 1, 2, 0) * num2, Cofac(0, 0, 1, 1) * num2);
		}

		public float Cofac(int r1, int c1, int r2, int c2)
		{
			return this[r1][c1] * this[r2][c2] - this[r1][c2] * this[r2][c1];
		}

		public IndexedBasisMatrix Transpose()
		{
			return new IndexedBasisMatrix(_Row0.X, _Row1.X, _Row2.X, _Row0.Y, _Row1.Y, _Row2.Y, _Row0.Z, _Row1.Z, _Row2.Z);
		}
	}
}
