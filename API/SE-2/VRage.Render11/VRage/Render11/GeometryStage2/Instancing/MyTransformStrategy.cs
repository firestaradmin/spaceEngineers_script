using System.Runtime.CompilerServices;
using VRageMath;
using VRageRender;

namespace VRage.Render11.GeometryStage2.Instancing
{
	internal struct MyTransformStrategy
	{
		public RowMatrix RowMatrix;

		public Vector3 Translation;

		public void GetWorldMatrixCols(out RowMatrix m)
		{
			m = RowMatrix;
		}

		public Vector3 GetCameraTranslation()
		{
			return Translation;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetWorldMatrix(ref MatrixD worldMatrix, ref Vector3D camPosition)
		{
			Translation = new Vector3(worldMatrix.M41 - camPosition.X, worldMatrix.M42 - camPosition.Y, worldMatrix.M43 - camPosition.Z);
			RowMatrix.Col0 = new Vector4((float)worldMatrix.M11, (float)worldMatrix.M21, (float)worldMatrix.M31, Translation.X);
			RowMatrix.Col1 = new Vector4((float)worldMatrix.M12, (float)worldMatrix.M22, (float)worldMatrix.M32, Translation.Y);
			RowMatrix.Col2 = new Vector4((float)worldMatrix.M13, (float)worldMatrix.M23, (float)worldMatrix.M33, Translation.Z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateTranslation(ref MatrixD worldMatrix, ref Vector3D camPosition)
		{
			Translation = new Vector3(worldMatrix.M41 - camPosition.X, worldMatrix.M42 - camPosition.Y, worldMatrix.M43 - camPosition.Z);
			RowMatrix.Col0.W = Translation.X;
			RowMatrix.Col1.W = Translation.Y;
			RowMatrix.Col2.W = Translation.Z;
		}

		public bool CheckMatrix(ref MatrixD worldMatrix)
		{
			Matrix matrix = worldMatrix;
			Vector3 vector = worldMatrix.Translation - MyRender11.Environment.Matrices.CameraPosition;
			if (RowMatrix.Col0.X == matrix.M11 && RowMatrix.Col0.Y == matrix.M21 && RowMatrix.Col0.Z == matrix.M31 && RowMatrix.Col1.X == matrix.M12 && RowMatrix.Col1.Y == matrix.M22 && RowMatrix.Col1.Z == matrix.M32 && RowMatrix.Col2.X == matrix.M13 && RowMatrix.Col2.Y == matrix.M23 && RowMatrix.Col2.Z == matrix.M33 && Translation.X == vector.X && Translation.Y == vector.Y && Translation.Z == vector.Z && RowMatrix.Col0.W == vector.X && RowMatrix.Col1.W == vector.Y)
			{
				return RowMatrix.Col2.W == vector.Z;
			}
			return false;
		}
	}
}
