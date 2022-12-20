using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems.CoordinateSystem
{
	/// <summary>
	/// Local coord system.
	/// </summary>
	public class MyLocalCoordSys
	{
		private static readonly MyStringId ID_SQUARE = MyStringId.GetOrCompute("Square");

		private const float COLOR_ALPHA = 0.4f;

		private const int LOCAL_COORD_SIZE = 1000;

		private const float BBOX_BORDER_THICKNESS_MODIF = 0.0015f;

		/// <summary>
		/// Origin transform.
		/// </summary>
		private MyTransformD m_origin;

		/// <summary>
		/// Bouding box of the coord system.
		/// </summary>
		private MyOrientedBoundingBoxD m_boundingBox;

		/// <summary>
		/// Cached corner of the bbox in world coordinates.
		/// </summary>
		private Vector3D[] m_corners = new Vector3D[8];

		internal Color DebugColor;

		/// <summary>
		/// Gets origin transformation of the coord system.
		/// </summary>
		public MyTransformD Origin => m_origin;

<<<<<<< HEAD
		/// <summary>
		/// Indicates how many entities are in this coord system.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public long EntityCounter { get; set; }

		internal MyOrientedBoundingBoxD BoundingBox => m_boundingBox;

<<<<<<< HEAD
		/// <summary>
		/// Color of the bounding box.
		/// </summary>
		public Color RenderColor { get; set; }

		/// <summary>
		/// Id if this coord system
		/// </summary>
=======
		public Color RenderColor { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public long Id { get; set; }

		public MyLocalCoordSys(int size = 1000)
		{
			m_origin = new MyTransformD(MatrixD.Identity);
			float num = (float)size / 2f;
			Vector3 vector = new Vector3(num, num, num);
			BoundingBoxD box = new BoundingBoxD(-vector, vector);
			m_boundingBox = new MyOrientedBoundingBoxD(box, m_origin.TransformMatrix);
			m_boundingBox.GetCorners(m_corners, 0);
			RenderColor = GenerateRandomColor();
			DebugColor = GenerateDebugColor(RenderColor);
		}

		public MyLocalCoordSys(MyTransformD origin, int size = 1000)
		{
			m_origin = origin;
			Vector3 vector = new Vector3(size / 2, size / 2, size / 2);
			BoundingBoxD box = new BoundingBoxD(-vector, vector);
			m_boundingBox = new MyOrientedBoundingBoxD(box, m_origin.TransformMatrix);
			m_boundingBox.GetCorners(m_corners, 0);
			RenderColor = GenerateRandomColor();
			DebugColor = GenerateDebugColor(RenderColor);
		}

		private Color GenerateRandomColor()
		{
			float x = (float)MyRandom.Instance.Next(0, 100) / 100f * 0.4f;
			float y = (float)MyRandom.Instance.Next(0, 100) / 100f * 0.4f;
			float z = (float)MyRandom.Instance.Next(0, 100) / 100f * 0.4f;
			return new Vector4(x, y, z, 0.4f);
		}

		private Color GenerateDebugColor(Color original)
		{
			Vector3 hSV = new Color(original, 1f).ColorToHSV();
			hSV.Y = 0.8f;
			hSV.Z = 0.8f;
			return hSV.HSVtoColor();
		}

		public bool Contains(ref Vector3D vec)
		{
			return m_boundingBox.Contains(ref vec);
		}

		public void Draw()
		{
			MatrixD worldMatrix = Origin.TransformMatrix;
			Vector3D vector3D = Vector3D.One;
			Vector3D vector3D2 = Vector3D.Zero;
			for (int i = 0; i < 8; i++)
			{
				Vector3D value = MySector.MainCamera.WorldToScreen(ref m_corners[i]);
				vector3D = Vector3D.Min(vector3D, value);
				vector3D2 = Vector3D.Max(vector3D2, value);
			}
			float lineWidth = 0.0015f / (float)MathHelper.Clamp((vector3D2 - vector3D).Length(), 0.01, 1.0);
			Color color = (MyFakes.ENABLE_DEBUG_DRAW_COORD_SYS ? DebugColor : RenderColor);
			BoundingBoxD localbox = new BoundingBoxD(-m_boundingBox.HalfExtent, m_boundingBox.HalfExtent);
			MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, MySimpleObjectRasterizer.SolidAndWireframe, 1, lineWidth, ID_SQUARE, ID_SQUARE);
			if (MyFakes.ENABLE_DEBUG_DRAW_COORD_SYS)
			{
				Vector3D vector3D3 = worldMatrix.Translation - MySector.MainCamera.Position;
				MyRenderProxy.DebugDrawText3D(Origin.Position, $"LCS Id:{Id} Distance:{vector3D3.Length():###.00}m", color, 1f, depthRead: true);
				for (int j = -10; j < 11; j++)
				{
					Vector3D pointFrom = Origin.Position + worldMatrix.Forward * 20.0 + worldMatrix.Right * ((double)j * 2.5);
					Vector3D pointTo = Origin.Position - worldMatrix.Forward * 20.0 + worldMatrix.Right * ((double)j * 2.5);
					MyRenderProxy.DebugDrawLine3D(pointFrom, pointTo, color, color, depthRead: false);
				}
				for (int k = -10; k < 11; k++)
				{
					Vector3D pointFrom2 = Origin.Position + worldMatrix.Right * 20.0 + worldMatrix.Forward * ((double)k * 2.5);
					Vector3D pointTo2 = Origin.Position - worldMatrix.Right * 20.0 + worldMatrix.Forward * ((double)k * 2.5);
					MyRenderProxy.DebugDrawLine3D(pointFrom2, pointTo2, color, color, depthRead: false);
				}
			}
		}
	}
}
