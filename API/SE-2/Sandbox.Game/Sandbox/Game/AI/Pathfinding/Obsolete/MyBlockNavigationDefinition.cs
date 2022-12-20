using System.Text;
using Sandbox.Definitions;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
	[MyDefinitionType(typeof(MyObjectBuilder_BlockNavigationDefinition), null)]
	public class MyBlockNavigationDefinition : MyDefinitionBase
	{
		private struct SizeAndCenter
		{
			private readonly Vector3I m_size;

			private readonly Vector3I m_center;

			public SizeAndCenter(Vector3I size, Vector3I center)
			{
				m_size = size;
				m_center = center;
			}

			private bool Equals(SizeAndCenter other)
			{
				if (other.m_size == m_size)
				{
					return other.m_center == m_center;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				if (obj.GetType() != typeof(SizeAndCenter))
				{
					return false;
				}
				return Equals((SizeAndCenter)obj);
			}

			public override int GetHashCode()
			{
				Vector3I size = m_size;
				int num = size.GetHashCode() * 1610612741;
				size = m_center;
				return num + size.GetHashCode();
			}
		}

		private class Sandbox_Game_AI_Pathfinding_Obsolete_MyBlockNavigationDefinition_003C_003EActor : IActivator, IActivator<MyBlockNavigationDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyBlockNavigationDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBlockNavigationDefinition CreateInstance()
			{
				return new MyBlockNavigationDefinition();
			}

			MyBlockNavigationDefinition IActivator<MyBlockNavigationDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly StringBuilder m_tmpStringBuilder = new StringBuilder();

		private static readonly MyObjectBuilder_BlockNavigationDefinition m_tmpDefaultOb = new MyObjectBuilder_BlockNavigationDefinition();

		public MyGridNavigationMesh Mesh { get; private set; }

		public bool NoEntry { get; private set; }

		public MyBlockNavigationDefinition()
		{
			Mesh = null;
			NoEntry = false;
		}

		public static MyObjectBuilder_BlockNavigationDefinition GetDefaultObjectBuilder(MyCubeBlockDefinition blockDefinition)
		{
			MyObjectBuilder_BlockNavigationDefinition tmpDefaultOb = m_tmpDefaultOb;
			m_tmpStringBuilder.Clear();
			m_tmpStringBuilder.Append("Default_");
			m_tmpStringBuilder.Append(blockDefinition.Size.X);
			m_tmpStringBuilder.Append("_");
			m_tmpStringBuilder.Append(blockDefinition.Size.Y);
			m_tmpStringBuilder.Append("_");
			m_tmpStringBuilder.Append(blockDefinition.Size.Z);
			tmpDefaultOb.Id = new MyDefinitionId(typeof(MyObjectBuilder_BlockNavigationDefinition), m_tmpStringBuilder.ToString());
			tmpDefaultOb.Size = blockDefinition.Size;
			tmpDefaultOb.Center = blockDefinition.Center;
			return tmpDefaultOb;
		}

		public static void CreateDefaultTriangles(MyObjectBuilder_BlockNavigationDefinition ob)
		{
			Vector3I vector3I = ob.Size;
			Vector3I vector3I2 = ob.Center;
			int num = 4 * (vector3I.X * vector3I.Y + vector3I.X * vector3I.Z + vector3I.Y * vector3I.Z);
			ob.Triangles = new MyObjectBuilder_BlockNavigationDefinition.Triangle[num];
			int num2 = 0;
			Vector3 vector = vector3I * 0.5f - vector3I2 - Vector3.Half;
			for (int i = 0; i < 6; i++)
			{
				Base6Directions.Direction direction = Base6Directions.EnumDirections[i];
				Vector3 vector2 = vector;
				Base6Directions.Direction direction2;
				Base6Directions.Direction direction3;
				switch (direction)
				{
				case Base6Directions.Direction.Right:
					direction2 = Base6Directions.Direction.Forward;
					direction3 = Base6Directions.Direction.Up;
					vector2 += new Vector3(0.5f, -0.5f, 0.5f) * vector3I;
					break;
				case Base6Directions.Direction.Left:
					direction2 = Base6Directions.Direction.Backward;
					direction3 = Base6Directions.Direction.Up;
					vector2 += new Vector3(-0.5f, -0.5f, -0.5f) * vector3I;
					break;
				case Base6Directions.Direction.Up:
					direction2 = Base6Directions.Direction.Right;
					direction3 = Base6Directions.Direction.Forward;
					vector2 += new Vector3(-0.5f, 0.5f, 0.5f) * vector3I;
					break;
				case Base6Directions.Direction.Down:
					direction2 = Base6Directions.Direction.Right;
					direction3 = Base6Directions.Direction.Backward;
					vector2 += new Vector3(-0.5f, -0.5f, -0.5f) * vector3I;
					break;
				case Base6Directions.Direction.Backward:
					direction2 = Base6Directions.Direction.Right;
					direction3 = Base6Directions.Direction.Up;
					vector2 += new Vector3(-0.5f, -0.5f, 0.5f) * vector3I;
					break;
				default:
					direction2 = Base6Directions.Direction.Left;
					direction3 = Base6Directions.Direction.Up;
					vector2 += new Vector3(0.5f, -0.5f, -0.5f) * vector3I;
					break;
				}
				Vector3 vector3 = Base6Directions.GetVector(direction2);
				Vector3 vector4 = Base6Directions.GetVector(direction3);
				int num3 = vector3I.AxisValue(Base6Directions.GetAxis(direction3));
				int num4 = vector3I.AxisValue(Base6Directions.GetAxis(direction2));
				for (int j = 0; j < num3; j++)
				{
					for (int k = 0; k < num4; k++)
					{
						MyObjectBuilder_BlockNavigationDefinition.Triangle triangle = new MyObjectBuilder_BlockNavigationDefinition.Triangle();
						triangle.Points = new SerializableVector3[3];
						triangle.Points[0] = vector2;
						triangle.Points[1] = vector2 + vector3;
						triangle.Points[2] = vector2 + vector4;
						ob.Triangles[num2++] = triangle;
						triangle = new MyObjectBuilder_BlockNavigationDefinition.Triangle();
						triangle.Points = new SerializableVector3[3];
						triangle.Points[0] = vector2 + vector3;
						triangle.Points[1] = vector2 + vector3 + vector4;
						triangle.Points[2] = vector2 + vector4;
						ob.Triangles[num2++] = triangle;
						vector2 += vector3;
					}
					vector2 -= vector3 * num4;
					vector2 += vector4;
				}
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase ob)
		{
			base.Init(ob);
			MyObjectBuilder_BlockNavigationDefinition myObjectBuilder_BlockNavigationDefinition = ob as MyObjectBuilder_BlockNavigationDefinition;
			if (ob == null)
			{
				return;
			}
			if (myObjectBuilder_BlockNavigationDefinition.NoEntry || myObjectBuilder_BlockNavigationDefinition.Triangles == null)
			{
				NoEntry = true;
				return;
			}
			NoEntry = false;
			MyGridNavigationMesh myGridNavigationMesh = new MyGridNavigationMesh(null, null, myObjectBuilder_BlockNavigationDefinition.Triangles.Length);
			Vector3I max = myObjectBuilder_BlockNavigationDefinition.Size - Vector3I.One - myObjectBuilder_BlockNavigationDefinition.Center;
			Vector3I min = -(Vector3I)myObjectBuilder_BlockNavigationDefinition.Center;
			MyObjectBuilder_BlockNavigationDefinition.Triangle[] triangles = myObjectBuilder_BlockNavigationDefinition.Triangles;
			foreach (MyObjectBuilder_BlockNavigationDefinition.Triangle obj in triangles)
			{
				Vector3 a = obj.Points[0];
				Vector3 b = obj.Points[1];
				Vector3 c = obj.Points[2];
				MyNavigationTriangle tri = myGridNavigationMesh.AddTriangle(ref a, ref b, ref c);
				Vector3 vector = (a + b + c) / 3f;
				Vector3 vector2 = (vector - a) * 0.0001f;
				Vector3 vector3 = (vector - b) * 0.0001f;
				Vector3 vector4 = (vector - c) * 0.0001f;
				Vector3I value = Vector3I.Round(a + vector2);
				Vector3I value2 = Vector3I.Round(b + vector3);
				Vector3I value3 = Vector3I.Round(c + vector4);
				Vector3I.Clamp(ref value, ref min, ref max, out value);
				Vector3I.Clamp(ref value2, ref min, ref max, out value2);
				Vector3I.Clamp(ref value3, ref min, ref max, out value3);
				Vector3I.Min(ref value, ref value2, out var result);
				Vector3I.Min(ref result, ref value3, out result);
				Vector3I.Max(ref value, ref value2, out var result2);
				Vector3I.Max(ref result2, ref value3, out result2);
				Vector3I gridPos = result;
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref result, ref result2);
				while (vector3I_RangeIterator.IsValid())
				{
					myGridNavigationMesh.RegisterTriangle(tri, ref gridPos);
					vector3I_RangeIterator.GetNext(out gridPos);
				}
			}
			Mesh = myGridNavigationMesh;
		}
	}
}
