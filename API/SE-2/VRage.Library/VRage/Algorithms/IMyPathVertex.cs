using System.Collections;
using System.Collections.Generic;

namespace VRage.Algorithms
{
	public interface IMyPathVertex<V> : IEnumerable<IMyPathEdge<V>>, IEnumerable
	{
		MyPathfindingData PathfindingData { get; }

		/// <summary>
		/// Heuristic on the shortest path to another vertex. Used for finding the shortest path.
		/// </summary>
		float EstimateDistanceTo(IMyPathVertex<V> other);

		/// <summary>
		/// Returns the number of neighboring vertices.
		/// </summary>
		int GetNeighborCount();

		/// <summary>
		/// Gets N-th neighbor of this vertex.
		/// Must be consistent with the order in which IEnumerable&lt;IMyPathEdge&lt;V&gt;&gt; traverses the neighbors
		/// </summary>
		IMyPathVertex<V> GetNeighbor(int index);

		/// <summary>
		/// Gets N-th edge of this vertex.
		/// Must be consistent with the GetNeighbor() function.
		/// </summary>
		IMyPathEdge<V> GetEdge(int index);
	}
}
