using VRage.Algorithms;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyNavigationEdge : IMyPathEdge<MyNavigationPrimitive>
	{
		public static MyNavigationEdge Static = new MyNavigationEdge();

		private MyNavigationPrimitive m_triA;

		private MyNavigationPrimitive m_triB;

		public int Index { get; private set; }

		public void Init(MyNavigationPrimitive triA, MyNavigationPrimitive triB, int index)
		{
			m_triA = triA;
			m_triB = triB;
			Index = index;
		}

		public float GetWeight()
		{
			return (m_triA.Position - m_triB.Position).Length() * 1f;
		}

		public MyNavigationPrimitive GetOtherVertex(MyNavigationPrimitive vertex1)
		{
			if (vertex1 == m_triA)
			{
				return m_triB;
			}
			return m_triA;
		}
	}
}
