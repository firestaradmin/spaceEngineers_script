using System;
using System.Collections.Generic;
using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public interface IMyOctreeLeafNode : IDisposable
	{
		MyOctreeStorage.ChunkTypeEnum SerializedChunkType { get; }

		int SerializedChunkSize { get; }

		Vector3I VoxelRangeMin { get; }

		bool ReadOnly { get; }

		byte GetFilteredValue();

		/// <summary>
		///
		/// </summary>
		/// <param name="target"></param>
		/// <param name="types"></param>
		/// <param name="writeOffset"></param>
		/// <param name="lodIndex"></param>
		/// <param name="minInLod">Inclusive.</param>
		/// <param name="maxInLod">Inclusive.</param>
		/// <param name="flags"></param>
		void ReadRange(MyStorageData target, MyStorageDataTypeFlags types, ref Vector3I writeOffset, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod, ref MyVoxelRequestFlags flags);

		/// <summary>
		///
		/// </summary>
		/// <typeparam name="TOperator"></typeparam>
		/// <param name="source"></param>
		/// <param name="readOffset"></param>
		/// <param name="min">inclusive</param>
		/// <param name="max">inclusive</param>
		void ExecuteOperation<TOperator>(ref TOperator source, ref Vector3I readOffset, ref Vector3I min, ref Vector3I max) where TOperator : struct, IVoxelOperator;

		void OnDataProviderChanged(IMyStorageDataProvider newProvider);

		void ReplaceValues(Dictionary<byte, byte> oldToNewValueMap);

		ContainmentType Intersect(ref BoundingBoxI box, int lod, bool exhaustiveContainmentCheck = true);

		bool Intersect(ref LineD box, out double startOffset, out double endOffset);

		bool TryGetUniformValue(out byte uniformValue);
	}
}
