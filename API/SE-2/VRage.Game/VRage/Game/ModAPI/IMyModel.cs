using System.Collections.Generic;
using VRageMath;
using VRageRender.Import;

namespace VRage.Game.ModAPI
{
	public interface IMyModel
	{
		int UniqueId { get; }

		int DataVersion { get; }

		BoundingSphere BoundingSphere { get; }

		BoundingBox BoundingBox { get; }

		Vector3 BoundingBoxSize { get; }

		Vector3 BoundingBoxSizeHalf { get; }

		Vector3I[] BoneMapping { get; }

		float PatternScale { get; }

		float ScaleFactor { get; }

		string AssetName { get; }

		int GetTrianglesCount();

		int GetVerticesCount();

		/// <summary>
		/// Gets the dummies from the model
		/// </summary>
		/// <param name="dummies">Dictionary of dummies, can be null to just return count</param>
		/// <returns>Number of dummies in model</returns>
		int GetDummies(IDictionary<string, IMyModelDummy> dummies);

		/// <summary>
		/// Gets the vertex indices that complete a triangle at the index
		/// </summary>
		IMyTriangleVertexIndices GetTriangle(int triangleIndex);

		/// <summary>
		/// Gets the vertex indices that complete a triangle at the index
		/// </summary>
		MyMeshDrawTechnique GetDrawTechnique(int triangleIndex);

		/// <summary>
		/// Gets the vertex
		/// </summary>
		Vector3 GetVertex(int vertexIndex);

		/// <summary>
		/// Gets a set of three vertices
		/// </summary>
		void GetVertex(int vertexIndex1, int vertexIndex2, int vertexIndex3, out Vector3 v1, out Vector3 v2, out Vector3 v3);
	}
}
