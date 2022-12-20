using System;
using VRage.Noise;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
	/// <summary>
	/// Creates simple axis aligned box with no noise applied to it. Meant to be simple
	/// example of how to implement CSG shape.
	///
	/// NOTE: Using this box might not always produce sharp edges due to rasterization
	/// of distance function. They will be sharp when box aligns with raster grid (which
	/// is, simply put, for positions and sizes that are power of two).
	/// </summary>
	internal class MyCsgBox : MyCsgShapeBase
	{
		private Vector3 m_translation;

		private float m_halfExtents;

		internal float HalfExtents => m_halfExtents;

		public MyCsgBox(Vector3 translation, float halfExtents)
		{
			m_translation = translation;
			m_halfExtents = halfExtents;
		}

		internal override ContainmentType Contains(ref BoundingBox queryAabb, ref BoundingSphere querySphere, float lodVoxelSize)
		{
			BoundingBox.CreateFromHalfExtent(m_translation, m_halfExtents + lodVoxelSize).Contains(ref queryAabb, out var result);
			if (result == ContainmentType.Disjoint)
			{
				return ContainmentType.Disjoint;
			}
			BoundingBox.CreateFromHalfExtent(m_translation, m_halfExtents - lodVoxelSize).Contains(ref queryAabb, out var result2);
			if (result2 == ContainmentType.Contains)
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Intersects;
		}

		internal override float SignedDistance(ref Vector3 position, float lodVoxelSize, IMyModule macroModulator, IMyModule detailModulator)
		{
			return MathHelper.Clamp(SignedDistanceUnchecked(ref position, lodVoxelSize, macroModulator, detailModulator), -1f, 1f);
		}

		internal override float SignedDistanceUnchecked(ref Vector3 position, float lodVoxelSize, IMyModule macroModulator, IMyModule detailModulator)
		{
			Vector3 value = Vector3.Abs(position - m_translation) - m_halfExtents;
			return (Math.Min(Math.Max(value.X, Math.Max(value.Y, value.Z)), 0f) + Vector3.Max(value, Vector3.Zero).Length()) / lodVoxelSize;
		}

		internal override void DebugDraw(ref MatrixD worldTranslation, Color color)
		{
			BoundingBoxD aabb = new BoundingBoxD(m_translation - m_halfExtents, m_translation + m_halfExtents).TransformFast(worldTranslation);
			MyRenderProxy.DebugDrawAABB(aabb, color, 0.5f, 1f, depthRead: false);
		}

		internal override MyCsgShapeBase DeepCopy()
		{
			return new MyCsgBox(m_translation, m_halfExtents);
		}

		internal override void ShrinkTo(float percentage)
		{
			m_halfExtents *= percentage;
		}

		internal override Vector3 Center()
		{
			return m_translation;
		}
	}
}
