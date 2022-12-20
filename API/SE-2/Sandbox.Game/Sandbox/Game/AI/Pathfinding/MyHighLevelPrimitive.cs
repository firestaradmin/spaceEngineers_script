using System.Collections.Generic;
using VRage.Algorithms;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyHighLevelPrimitive : MyNavigationPrimitive
	{
		private readonly List<int> m_neighbors;

		private Vector3 m_position;

		public bool IsExpanded { get; set; }

		public int Index { get; }

		public override Vector3 Position => m_position;

		public override Vector3D WorldPosition => Parent.LocalToGlobal(m_position);

		public MyHighLevelGroup Parent { get; }

		public override IMyNavigationGroup Group => Parent;

		public MyHighLevelPrimitive(MyHighLevelGroup parent, int index, Vector3 position)
		{
			Parent = parent;
			m_neighbors = new List<int>(4);
			Index = index;
			m_position = position;
			IsExpanded = false;
		}

		public override string ToString()
		{
			return string.Concat("(", Parent, ")[", Index, "]");
		}

		public void GetNeighbours(List<int> output)
		{
			output.Clear();
			output.AddRange(m_neighbors);
		}

		public void Connect(int other)
		{
			m_neighbors.Add(other);
		}

		public void Disconnect(int other)
		{
			m_neighbors.Remove(other);
		}

		public void UpdatePosition(Vector3 position)
		{
			m_position = position;
		}

		public IMyHighLevelComponent GetComponent()
		{
			return Parent.LowLevelGroup.GetComponent(this);
		}

		public override int GetOwnNeighborCount()
		{
			return m_neighbors.Count;
		}

		public override IMyPathVertex<MyNavigationPrimitive> GetOwnNeighbor(int index)
		{
			return Parent.GetPrimitive(m_neighbors[index]);
		}

		public override IMyPathEdge<MyNavigationPrimitive> GetOwnEdge(int index)
		{
			MyNavigationEdge.Static.Init(this, GetOwnNeighbor(index) as MyNavigationPrimitive, 0);
			return MyNavigationEdge.Static;
		}

		public override MyHighLevelPrimitive GetHighLevelPrimitive()
		{
			return null;
		}
	}
}
