using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Animations
{
	/// <summary>
	/// Class providing various IK solutions.
	/// Feet IK is 
	/// </summary>
	public class MyAnimationInverseKinematics
	{
		/// <summary>
		/// List of all feet bones.
		/// </summary>
		private readonly List<MyAnimationIkChainExt> m_feet = new List<MyAnimationIkChainExt>(2);

		/// <summary>
		/// List of all ignored bones (that should not move during IK!).
		/// </summary>
		private bool[] m_ignoredBonesTable;

		/// <summary>
		/// All ignored bones (that should not move during IK!). Names of ignored bones.
		/// </summary>
		private readonly HashSet<string> m_ignoredBoneNames = new HashSet<string>();

		/// <summary>
		/// Character offset - used when the character is slightly above the terrain due to the capsule
		/// </summary>
		private float m_characterDirDownOffset;

		/// <summary>
		/// Maximum character offsets.
		/// </summary>
		private const float m_characterDirDownOffsetMin = -0.3f;

		private const float m_characterDirDownOffsetMax = 0.2f;

		/// <summary>
		/// Character offset smoothing.
		/// </summary>
		private float m_characterDirDownOffsetSmoothness = 0.5f;

		/// <summary>
		/// Current influence of the feet IK.
		/// </summary>
		private float m_currentFeetIkInfluence;

		private const float m_poleVectorChangeSmoothness = 0.85f;

		private const int m_offsetFilteringSampleCount = 30;

		private int m_offsetFilteringCursor;

		private float m_filteredOffsetValue;

		private readonly List<float> m_offsetFiltering = new List<float>(30);

		private static readonly int[] m_boneIndicesPreallocated = new int[64];

		public static MatrixD DebugTransform;

		private static bool m_showDebugDrawings = false;

		private readonly List<Matrix> m_ignoredBonesBackup = new List<Matrix>(8);

		private float m_rootBoneVerticalOffset;

		/// <summary>
		/// List of all feet bones.
		/// </summary>
		public ListReader<MyAnimationIkChainExt> Feet => m_feet;

		/// <summary>
		/// Interface providing results from ray casts.
		/// </summary>
		public IMyTerrainHeightProvider TerrainHeightProvider { get; set; }

		public float RootBoneVerticalOffset => m_rootBoneVerticalOffset;

		/// <summary>
		/// Immediately reset the IK influence to zero.
		/// </summary>
		public void ResetIkInfluence()
		{
			m_currentFeetIkInfluence = 0f;
		}

		/// <summary>
		/// Solve feet positions. 
		/// </summary>
		/// <param name="enabled">Feet resolving is enabled - this is a parameter because of blending over time.</param>
		/// <param name="characterBones">Character bones storage.</param>
		/// <param name="allowMovingWithBody">If feet cannot reach, move the body</param>
<<<<<<< HEAD
		/// <param name="feetOffset"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void SolveFeet(bool enabled, MyCharacterBone[] characterBones, bool allowMovingWithBody, float feetOffset = 0f)
		{
			m_currentFeetIkInfluence = MathHelper.Clamp(m_currentFeetIkInfluence + (enabled ? 0.1f : (-0.1f)), 0f, 1f);
			if (m_currentFeetIkInfluence <= 0f || TerrainHeightProvider == null || characterBones == null || characterBones.Length == 0 || m_feet.Count == 0)
			{
				return;
			}
			RecreateIgnoredBonesTableIfNeeded(characterBones);
			BackupIgnoredBones(characterBones);
			MoveTheBodyDown(characterBones, allowMovingWithBody);
			RestoreIgnoredBones(characterBones);
			float referenceTerrainHeight = TerrainHeightProvider.GetReferenceTerrainHeight();
			float num = 0.2f;
			bool flag = false;
			foreach (MyAnimationIkChainExt foot in m_feet)
			{
				if (foot.BoneIndex == -1)
				{
					foreach (MyCharacterBone myCharacterBone in characterBones)
					{
						if (myCharacterBone.Name == foot.BoneName)
						{
							foot.BoneIndex = myCharacterBone.Index;
							break;
						}
					}
					if (foot.BoneIndex == -1)
					{
						continue;
					}
				}
				MyCharacterBone myCharacterBone2 = characterBones[foot.BoneIndex];
				MyCharacterBone myCharacterBone3 = myCharacterBone2;
				MyCharacterBone myCharacterBone4 = myCharacterBone2;
				for (int j = 0; j < foot.ChainLength; j++)
				{
					myCharacterBone4 = myCharacterBone3;
					myCharacterBone3 = myCharacterBone3.Parent;
				}
				myCharacterBone3.ComputeAbsoluteTransform();
				Vector3 finalPosition = myCharacterBone2.AbsoluteTransform.Translation;
				Vector3 translation = myCharacterBone2.GetAbsoluteRigTransform().Translation;
				if (TerrainHeightProvider.GetTerrainHeight(foot.BoneIndex + 1, finalPosition, translation, out var terrainHeight, out var terrainNormal))
				{
					terrainNormal = Vector3.Lerp(foot.LastTerrainNormal, terrainNormal, 0.2f);
				}
				else
				{
					terrainHeight = foot.LastTerrainHeight;
					terrainNormal = Vector3.Lerp(foot.LastTerrainNormal, Vector3.Up, 0.1f);
					foot.LastTerrainHeight *= 0.9f;
				}
				terrainHeight = (foot.LastTerrainHeight = terrainHeight + feetOffset);
				foot.LastTerrainNormal = terrainNormal;
				float num2 = terrainHeight - referenceTerrainHeight;
				float y = finalPosition.Y;
				finalPosition.Y = Math.Min(val2: num2 + (finalPosition.Y - m_filteredOffsetValue) / terrainNormal.Y, val1: myCharacterBone4.AbsoluteTransform.Translation.Y);
				finalPosition.Y = MathHelper.Lerp(y, finalPosition.Y, m_currentFeetIkInfluence);
				num = MathHelper.Clamp(num2, -0.3f, num);
				flag = true;
				if (y > finalPosition.Y)
				{
					finalPosition.Y = y;
				}
				if (finalPosition.Y < translation.Y + m_filteredOffsetValue)
				{
					finalPosition.Y = translation.Y + m_filteredOffsetValue;
				}
				SolveIkTwoBones(characterBones, foot, ref finalPosition, ref terrainNormal, fromBindPose: false);
			}
			if (!flag)
			{
				num = 0f;
			}
			m_characterDirDownOffset = MathHelper.Lerp(num, m_characterDirDownOffset, m_characterDirDownOffsetSmoothness * (float)m_offsetFiltering.Count / 30f);
		}

		private void RestoreIgnoredBones(MyCharacterBone[] characterBones)
		{
			int num = 0;
			for (int i = 0; i < characterBones.Length; i++)
			{
				if (m_ignoredBonesTable[i])
				{
					characterBones[i].SetCompleteTransformFromAbsoluteMatrix(m_ignoredBonesBackup[num++], onlyRotation: false);
					characterBones[i].ComputeAbsoluteTransform();
				}
			}
		}

		private void BackupIgnoredBones(MyCharacterBone[] characterBones)
		{
			m_ignoredBonesBackup.Clear();
			for (int i = 0; i < characterBones.Length; i++)
			{
				if (m_ignoredBonesTable[i])
				{
					m_ignoredBonesBackup.Add(characterBones[i].AbsoluteTransform);
				}
			}
		}

		private void MoveTheBodyDown(MyCharacterBone[] characterBones, bool allowMoving)
		{
			if (allowMoving)
			{
				if (m_offsetFiltering.Count == 30)
				{
					m_offsetFiltering[m_offsetFilteringCursor++] = m_characterDirDownOffset;
					if (m_offsetFilteringCursor == 30)
					{
						m_offsetFilteringCursor = 0;
					}
				}
				else
				{
					m_offsetFiltering.Add(m_characterDirDownOffset);
				}
				float num = float.MinValue;
				foreach (float item in m_offsetFiltering)
				{
					num = Math.Max(num, item);
				}
				m_filteredOffsetValue = num;
			}
			else
			{
				m_filteredOffsetValue = 0f;
			}
			if (m_offsetFilteringCursor >= 30)
			{
				m_offsetFilteringCursor = 0;
			}
			MyCharacterBone myCharacterBone = characterBones[0];
			while (myCharacterBone.Parent != null)
			{
				myCharacterBone = myCharacterBone.Parent;
			}
			Vector3 translation = myCharacterBone.Translation;
			m_rootBoneVerticalOffset = m_filteredOffsetValue * m_currentFeetIkInfluence;
			translation.Y += m_rootBoneVerticalOffset;
			myCharacterBone.Translation = translation;
			myCharacterBone.ComputeAbsoluteTransform();
		}

		public void ClearCharacterOffsetFilteringSamples()
		{
			m_offsetFiltering.Clear();
			m_characterDirDownOffset = 0f;
		}

		/// <summary>
		/// Solve IK for chain of two bones + change rotation of end bone.
		/// </summary>
		/// <param name="characterBones">bone storage</param>
		/// <param name="ikChain">description of bone chain</param>
		/// <param name="finalPosition">desired position of end bone</param>
		/// <param name="finalNormal">desired normal of end bone - would be projected on plane first bone-second bone-third bone</param>
		/// <param name="fromBindPose">solve this starting from the bind pose</param>
		/// <returns>true on success</returns>
		public static bool SolveIkTwoBones(MyCharacterBone[] characterBones, MyAnimationIkChainExt ikChain, ref Vector3 finalPosition, ref Vector3 finalNormal, bool fromBindPose)
		{
			int boneIndex = ikChain.BoneIndex;
			float min = MathHelper.ToRadians(ikChain.MinEndPointRotation);
			float max = MathHelper.ToRadians(ikChain.MaxEndPointRotation);
			Vector3 value = ikChain.LastPoleVector;
			if (!value.IsValid())
			{
				value = Vector3.Left;
			}
			int chainLength = ikChain.ChainLength;
			bool alignBoneWithTerrain = ikChain.AlignBoneWithTerrain;
			MyCharacterBone myCharacterBone = characterBones[boneIndex];
			if (myCharacterBone == null)
			{
				return false;
			}
			MyCharacterBone parent = myCharacterBone.Parent;
			for (int i = 2; i < chainLength; i++)
			{
				parent = parent.Parent;
			}
			if (parent == null)
			{
				return false;
			}
			MyCharacterBone parent2 = parent.Parent;
			if (parent2 == null)
			{
				return false;
			}
			if (fromBindPose)
			{
				parent2.SetCompleteBindTransform();
				parent.SetCompleteBindTransform();
				myCharacterBone.SetCompleteBindTransform();
				parent2.ComputeAbsoluteTransform();
			}
			Matrix absoluteTransform = myCharacterBone.AbsoluteTransform;
			Vector3 translation = parent2.AbsoluteTransform.Translation;
			Vector3 translation2 = parent.AbsoluteTransform.Translation;
			Vector3 translation3 = myCharacterBone.AbsoluteTransform.Translation;
			Vector3 vector = translation2 - translation;
			Vector3 v = finalPosition - translation;
			Vector3 vector2 = translation3 - translation;
			Vector3.Cross(ref vector, ref vector2, out var result);
			result.Normalize();
			result = Vector3.Normalize(Vector3.Lerp(result, value, 0.85f));
			Vector3 vector3 = Vector3.Normalize(v);
			Vector3 vector4 = Vector3.Normalize(Vector3.Cross(vector3, result));
			Vector2 vector5 = new Vector2(vector4.Dot(ref vector), vector3.Dot(ref vector));
			Vector2 vector6 = new Vector2(vector4.Dot(ref vector2), vector3.Dot(ref vector2));
			Vector2 vector7 = new Vector2(vector4.Dot(ref v), vector3.Dot(ref v));
			Vector2 vector8 = new Vector2(vector4.Dot(ref finalNormal), vector3.Dot(ref finalNormal));
			float num = vector5.Length();
			float num2 = (vector6 - vector5).Length();
			float num3 = vector7.Length();
			if (num + num2 <= num3)
			{
				vector7 = (num + num2) * vector7 / num3;
			}
			Vector2 vector9 = default(Vector2);
			vector9.Y = (vector7.Y * vector7.Y - num2 * num2 + num * num) / (2f * vector7.Y);
			float num4 = num * num - vector9.Y * vector9.Y;
			vector9.X = (float)Math.Sqrt((num4 > 0f) ? num4 : 0f);
			Vector3 vector10 = translation + vector4 * vector9.X + vector3 * vector9.Y;
			Vector3 secondVector = vector10 - translation;
			Vector3 secondVector2 = finalPosition - vector10;
			Vector3 vector11 = vector4 * vector8.X + vector3 * vector8.Y;
			vector11.Normalize();
			Matrix absoluteMatrix = parent2.AbsoluteTransform;
			Quaternion rotation = Quaternion.CreateFromTwoVectors(vector, secondVector);
			absoluteMatrix.Right = Vector3.Transform(absoluteMatrix.Right, rotation);
			absoluteMatrix.Up = Vector3.Transform(absoluteMatrix.Up, rotation);
			absoluteMatrix.Forward = Vector3.Transform(absoluteMatrix.Forward, rotation);
			parent2.SetCompleteTransformFromAbsoluteMatrix(ref absoluteMatrix, onlyRotation: true);
			parent2.ComputeAbsoluteTransform();
			Matrix absoluteMatrix2 = parent.AbsoluteTransform;
			Quaternion rotation2 = Quaternion.CreateFromTwoVectors(myCharacterBone.AbsoluteTransform.Translation - parent.AbsoluteTransform.Translation, secondVector2);
			absoluteMatrix2.Right = Vector3.Transform(absoluteMatrix2.Right, rotation2);
			absoluteMatrix2.Up = Vector3.Transform(absoluteMatrix2.Up, rotation2);
			absoluteMatrix2.Forward = Vector3.Transform(absoluteMatrix2.Forward, rotation2);
			parent.SetCompleteTransformFromAbsoluteMatrix(ref absoluteMatrix2, onlyRotation: true);
			parent.ComputeAbsoluteTransform();
			if (ikChain.EndBoneTransform.HasValue)
			{
				Matrix value2 = ikChain.EndBoneTransform.Value;
				Matrix m = myCharacterBone.BindTransform;
				MatrixD matrixD = value2 * MatrixD.Invert((MatrixD)m * myCharacterBone.Parent.AbsoluteTransform);
				MatrixD m2 = matrixD.GetOrientation();
				myCharacterBone.Rotation = Quaternion.CreateFromRotationMatrix(Matrix.Normalize(m2));
				myCharacterBone.Translation = matrixD.Translation;
				myCharacterBone.ComputeAbsoluteTransform();
			}
			else if (alignBoneWithTerrain)
			{
				Vector3.Cross(ref vector11, ref Vector3.Up, out var result2);
				float epsilon = 0.2f;
				Matrix result3;
				if (MyUtils.IsValid(result2) && !MyUtils.IsZero(result2, epsilon))
				{
					float num5 = MyUtils.GetAngleBetweenVectors(vector11, Vector3.Up);
					if (result2.Dot(result) > 0f)
					{
						num5 = 0f - num5;
					}
					num5 = MathHelper.Clamp(num5, min, max);
					Matrix.CreateFromAxisAngle(ref result, num5, out result3);
				}
				else
				{
					result3 = Matrix.Identity;
				}
				ikChain.LastAligningRotationMatrix = Matrix.Lerp(ikChain.LastAligningRotationMatrix, result3, ikChain.AligningSmoothness);
				Matrix absoluteMatrix3 = absoluteTransform.GetOrientation() * ikChain.LastAligningRotationMatrix;
				absoluteMatrix3.Translation = myCharacterBone.AbsoluteTransform.Translation;
				myCharacterBone.SetCompleteTransformFromAbsoluteMatrix(ref absoluteMatrix3, onlyRotation: true);
				myCharacterBone.ComputeAbsoluteTransform();
			}
			if (m_showDebugDrawings)
			{
				MyRenderProxy.DebugDrawLine3D(Vector3D.Transform(translation, ref DebugTransform), Vector3D.Transform(translation2, ref DebugTransform), Color.Yellow, Color.Red, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(Vector3D.Transform(translation2, ref DebugTransform), Vector3D.Transform(translation3, ref DebugTransform), Color.Yellow, Color.Red, depthRead: false);
				MyRenderProxy.DebugDrawSphere(Vector3D.Transform(finalPosition, ref DebugTransform), 0.05f, Color.Cyan, 1f, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(Vector3D.Transform(translation2, ref DebugTransform), Vector3D.Transform(translation2 + result, ref DebugTransform), Color.PaleGreen, Color.PaleGreen, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(Vector3D.Transform(translation, ref DebugTransform), Vector3D.Transform(translation + vector4, ref DebugTransform), Color.White, Color.White, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(Vector3D.Transform(translation, ref DebugTransform), Vector3D.Transform(translation + vector3, ref DebugTransform), Color.White, Color.White, depthRead: false);
				MyRenderProxy.DebugDrawSphere(Vector3D.Transform(vector10, ref DebugTransform), 0.05f, Color.Green, 1f, depthRead: false);
				MyRenderProxy.DebugDrawAxis(parent2.AbsoluteTransform * DebugTransform, 0.5f, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(Vector3D.Transform(finalPosition, ref DebugTransform), Vector3D.Transform(finalPosition + vector11, ref DebugTransform), Color.Black, Color.LightBlue, depthRead: false);
				MyRenderProxy.DebugDrawArrow3D(Vector3D.Transform(translation2, ref DebugTransform), Vector3D.Transform(vector10, ref DebugTransform), Color.Green, Color.White);
			}
			ikChain.LastPoleVector = result;
			return true;
		}

		public static bool SolveIkCcd(MyCharacterBone[] characterBones, int boneIndex, int chainLength, ref Vector3D finalPosition)
		{
			Vector3D vector3D = finalPosition;
			int num = 0;
			int num2 = 50;
			float num3 = 2.5E-05f;
			MyCharacterBone myCharacterBone = characterBones[boneIndex];
			MyCharacterBone myCharacterBone2 = myCharacterBone;
			int[] boneIndicesPreallocated = m_boneIndicesPreallocated;
			for (int i = 0; i < chainLength; i++)
			{
				if (myCharacterBone2 == null)
				{
					chainLength = i;
					break;
				}
				boneIndicesPreallocated[i] = myCharacterBone2.Index;
				myCharacterBone2 = myCharacterBone2.Parent;
			}
			Vector3 translation = myCharacterBone.AbsoluteTransform.Translation;
			float num4 = 1f / (float)Vector3D.DistanceSquared(translation, vector3D);
			do
			{
				for (int j = 0; j < chainLength; j++)
				{
					MyCharacterBone myCharacterBone3 = characterBones[boneIndicesPreallocated[j]];
					myCharacterBone.ComputeAbsoluteTransform();
					Matrix absoluteTransform = myCharacterBone3.AbsoluteTransform;
					Vector3 translation2 = absoluteTransform.Translation;
					translation = myCharacterBone.AbsoluteTransform.Translation;
					double num5 = Vector3D.DistanceSquared(translation, vector3D);
					if (!(num5 > (double)num3))
					{
						continue;
					}
					Vector3 fromVector = translation - translation2;
					Vector3 vector = vector3D - translation2;
					double num6 = fromVector.LengthSquared();
					double num7 = vector.LengthSquared();
					double num8 = fromVector.Dot(vector);
					if (num8 < 0.0 || num8 * num8 < num6 * num7 * 0.99998998641967773)
					{
						float amount = 1f / (num4 * (float)num5 + 1f);
<<<<<<< HEAD
						Vector3 toVector = Vector3.Lerp(fromVector, vector, amount);
=======
						Vector3 toVector = Vector3.Lerp(fromVector, vector2, amount);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						Matrix.CreateRotationFromTwoVectors(ref fromVector, ref toVector, out var resultMatrix);
						Matrix matrix = Matrix.Normalize(absoluteTransform).GetOrientation() * resultMatrix;
						Matrix matrix2 = Matrix.Identity;
						if (myCharacterBone3.Parent != null)
						{
							matrix2 = myCharacterBone3.Parent.AbsoluteTransform;
						}
						matrix2 = Matrix.Normalize(matrix2);
						Matrix matrix3 = Matrix.Multiply(matrix, Matrix.Invert(myCharacterBone3.BindTransform * matrix2));
						myCharacterBone3.Rotation = Quaternion.CreateFromRotationMatrix(matrix3);
						myCharacterBone3.ComputeAbsoluteTransform();
					}
				}
			}
			while (num++ < num2 && Vector3D.DistanceSquared(translation, vector3D) > (double)num3);
			return Vector3D.DistanceSquared(translation, vector3D) <= (double)num3;
		}

		/// <summary>
		/// Register foot IK bone chain.
		/// </summary>
		public void RegisterFootBone(string boneName, int boneChainLength, bool alignBoneWithTerrain)
		{
			m_feet.Add(new MyAnimationIkChainExt
			{
				BoneIndex = -1,
				BoneName = boneName,
				ChainLength = boneChainLength,
				AlignBoneWithTerrain = alignBoneWithTerrain
			});
		}

		/// <summary>
		/// Register bone ignored by IK. IK will not move it.
		/// </summary>
		public void RegisterIgnoredBone(string boneName)
		{
			m_ignoredBoneNames.Add(boneName);
			m_ignoredBonesTable = null;
		}

		public void Clear()
		{
			m_characterDirDownOffset = 0f;
			m_feet.Clear();
			m_ignoredBoneNames.Clear();
			ClearCharacterOffsetFilteringSamples();
		}

		private void RecreateIgnoredBonesTableIfNeeded(MyCharacterBone[] characterBones)
		{
			if (m_ignoredBonesTable == null && characterBones != null)
			{
				m_ignoredBonesTable = new bool[characterBones.Length];
				for (int i = 0; i < characterBones.Length; i++)
				{
					m_ignoredBonesTable[i] = m_ignoredBoneNames.Contains(characterBones[i].Name);
				}
			}
		}
	}
}
