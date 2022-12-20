namespace BulletXNA.LinearMath
{
	public struct IndexedMatrix
	{
		private static IndexedMatrix _identity = new IndexedMatrix(1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f);

		public IndexedBasisMatrix _basis;

		public IndexedVector3 _origin;

		public static IndexedMatrix Identity => _identity;

		public IndexedMatrix(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33, float m41, float m42, float m43)
		{
			_basis = new IndexedBasisMatrix(m11, m12, m13, m21, m22, m23, m31, m32, m33);
			_origin = new IndexedVector3(m41, m42, m43);
		}

		public IndexedMatrix(IndexedBasisMatrix basis, IndexedVector3 origin)
		{
			_basis = basis;
			_origin = origin;
		}

		public static IndexedVector3 operator *(IndexedMatrix matrix1, IndexedVector3 v)
		{
			return new IndexedVector3(matrix1._basis._Row0.Dot(ref v) + matrix1._origin.X, matrix1._basis._Row1.Dot(ref v) + matrix1._origin.Y, matrix1._basis._Row2.Dot(ref v) + matrix1._origin.Z);
		}

		public static IndexedMatrix operator *(IndexedMatrix matrix1, IndexedMatrix matrix2)
		{
			IndexedMatrix result = default(IndexedMatrix);
			result._basis = matrix1._basis * matrix2._basis;
			result._origin = matrix1 * matrix2._origin;
			return result;
		}

		public IndexedMatrix Inverse()
		{
			IndexedBasisMatrix indexedBasisMatrix = _basis.Transpose();
			return new IndexedMatrix(indexedBasisMatrix, indexedBasisMatrix * -_origin);
		}
	}
}
