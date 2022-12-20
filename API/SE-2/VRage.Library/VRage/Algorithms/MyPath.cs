using System.Collections;
using System.Collections.Generic;

namespace VRage.Algorithms
{
	public class MyPath<V> : IEnumerable<MyPath<V>.PathNode>, IEnumerable where V : class, IMyPathVertex<V>, IEnumerable<IMyPathEdge<V>>
	{
		public struct PathNode
		{
			public IMyPathVertex<V> Vertex;

			public int nextVertex;
		}

		private List<PathNode> m_vertices;

		public int Count => m_vertices.Count;

		public PathNode this[int position]
		{
			get
			{
				return m_vertices[position];
			}
			set
			{
				m_vertices[position] = value;
			}
		}

		internal MyPath(int size)
		{
			m_vertices = new List<PathNode>(size);
		}

		internal void Add(IMyPathVertex<V> vertex, IMyPathVertex<V> nextVertex)
		{
			PathNode item = default(PathNode);
			item.Vertex = vertex;
			if (nextVertex == null)
			{
				m_vertices.Add(item);
				return;
			}
			int neighborCount = vertex.GetNeighborCount();
			for (int i = 0; i < neighborCount; i++)
			{
				IMyPathVertex<V> neighbor = vertex.GetNeighbor(i);
				if (neighbor == nextVertex)
				{
					item.nextVertex = i;
					m_vertices.Add(item);
					break;
				}
			}
		}

		public IEnumerator<PathNode> GetEnumerator()
		{
			return m_vertices.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_vertices.GetEnumerator();
		}
	}
}
