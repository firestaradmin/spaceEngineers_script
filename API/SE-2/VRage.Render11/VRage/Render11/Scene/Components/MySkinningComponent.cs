using System.Collections.Generic;
using VRage.Network;
using VRage.Render.Scene.Components;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.Scene.Components
{
	internal class MySkinningComponent : MyActorComponent
	{
		private class VRage_Render11_Scene_Components_MySkinningComponent_003C_003EActor : IActivator, IActivator<MySkinningComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MySkinningComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySkinningComponent CreateInstance()
			{
				return new MySkinningComponent();
			}

			MySkinningComponent IActivator<MySkinningComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly List<MyDecalPositionUpdate> m_decalUpdateCache = new List<MyDecalPositionUpdate>();

		internal const int CONSTANT_BUFFER_MATRIX_NUM = 60;

		private Matrix[] m_skinTransforms;

		private Matrix[] m_absoluteTransforms;

		private MySkeletonBoneDescription[] m_skeletonHierarchy;

		private int[] m_skeletonIndices;

		public Matrix[] SkinMatrices => m_skinTransforms;

		public override void Construct()
		{
			base.Construct();
			m_skinTransforms = null;
		}

		internal void SetSkeleton(MySkeletonBoneDescription[] hierarchy, int[] skeletonIndices)
		{
			m_skeletonHierarchy = hierarchy;
			m_skeletonIndices = skeletonIndices;
			m_skinTransforms = new Matrix[m_skeletonHierarchy.Length];
			m_absoluteTransforms = new Matrix[m_skeletonHierarchy.Length];
			base.Owner.GetRenderable()?.MarkDirty();
		}

		internal void SetAnimationBones(Matrix[] boneAbsoluteTransforms, IReadOnlyList<MyBoneDecalUpdate> boneDecals)
		{
			if (m_skeletonHierarchy == null)
			{
				return;
			}
			int num = m_skeletonHierarchy.Length;
			for (int i = 0; i < num; i++)
			{
				m_absoluteTransforms[i] = boneAbsoluteTransforms[i];
			}
			int num2 = m_skeletonIndices.Length;
			for (int j = 0; j < num2; j++)
			{
				m_skinTransforms[j] = Matrix.Transpose(m_skeletonHierarchy[m_skeletonIndices[j]].SkinTransform * m_absoluteTransforms[m_skeletonIndices[j]]);
			}
			m_decalUpdateCache.Clear();
			for (int k = 0; k < boneDecals.Count; k++)
			{
				MyBoneDecalUpdate myBoneDecalUpdate = boneDecals[k];
				if (MyScreenDecals.GetDecalTopoData(myBoneDecalUpdate.DecalID, out var data))
				{
					Matrix matrix = ComputeSkinning(data.BoneIndices, ref data.BoneWeights);
					Matrix transform = data.MatrixBinding * matrix;
					m_decalUpdateCache.Add(new MyDecalPositionUpdate
					{
						ID = myBoneDecalUpdate.DecalID,
						Transform = transform
					});
				}
			}
			MyScreenDecals.UpdateDecals(m_decalUpdateCache);
		}

		private Matrix ComputeSkinning(Vector4UByte indices, ref Vector4 weights)
		{
			Matrix result = default(Matrix);
			for (int i = 0; i < 4; i++)
			{
				float num = weights[i];
				if (num == 0f)
				{
					break;
				}
				Matrix.Transpose(ref m_skinTransforms[m_skeletonIndices[indices[i]]], out var result2);
				result2 *= num;
				result += result2;
			}
			return result;
		}
	}
}
