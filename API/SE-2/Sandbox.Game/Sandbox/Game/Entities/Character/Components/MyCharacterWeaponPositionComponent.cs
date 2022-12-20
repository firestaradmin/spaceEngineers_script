using System;
using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.Utils;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace Sandbox.Game.Entities.Character.Components
{
	/// <summary>
	/// Weapon positioning.
	/// </summary>
	public class MyCharacterWeaponPositionComponent : MyCharacterComponent
	{
		private class Sandbox_Game_Entities_Character_Components_MyCharacterWeaponPositionComponent_003C_003EActor : IActivator, IActivator<MyCharacterWeaponPositionComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyCharacterWeaponPositionComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCharacterWeaponPositionComponent CreateInstance()
			{
				return new MyCharacterWeaponPositionComponent();
			}

			MyCharacterWeaponPositionComponent IActivator<MyCharacterWeaponPositionComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private float m_animationToIKDelay = 0.3f;

		private float m_currentAnimationToIkTime = 0.3f;

		private float m_currentScatterToAnimRatio = 1f;

		private int m_animationToIkState;

		private Vector4 m_weaponPositionVariantWeightCounters = new Vector4(1f, 0f, 0f, 0f);

		private float m_sprintStatusWeight;

		private float m_sprintStatusGainSpeed = 71f / (339f * (float)Math.PI);

		private float m_backkickSpeed;

		private float m_backkickPos;

		private bool m_lastStateWasFalling;

		private bool m_lastStateWasCrouching;

		private float m_suppressBouncingForTimeSec;

		private float m_lastLocalRotX;

		private readonly MyAverageFiltering m_spineRestPositionX = new MyAverageFiltering(16);

		private readonly MyAverageFiltering m_spineRestPositionY = new MyAverageFiltering(16);

		private readonly MyAverageFiltering m_spineRestPositionZ = new MyAverageFiltering(16);

		private float m_currentScatterBlend;

		private Vector3 m_currentScatterPos;

		private Vector3 m_lastScatterPos;

		private static readonly float m_suppressBouncingDelay = 0.5f;

		public Vector3D LogicalPositionLocalSpace { get; private set; }

		public Vector3D LogicalPositionWorld { get; private set; }

		public Vector3D LogicalOrientationWorld { get; private set; }

		public Vector3D LogicalCrosshairPoint { get; private set; }

		public bool IsShooting { get; private set; }

		public bool ShouldSupressShootAnimation { get; set; }

		public bool IsInIronSight { get; private set; }

		public Vector3D GraphicalPositionWorld { get; private set; }

		public float ArmsIkWeight { get; private set; }

		/// <summary>
		/// Initialize from character object builder.
		/// </summary>
		public virtual void Init(MyObjectBuilder_Character characterBuilder)
		{
		}

		/// <summary>
		/// Update weapon position, either logical and graphical.
		/// </summary>
		public void Update(bool timeAdvanced = true)
		{
<<<<<<< HEAD
			if (base.Character.Definition == null || (Sync.IsDedicated && !MySession.Static.Players.IdentityIsNpc(base.Character.GetIdentity().IdentityId) && base.Character.IsClientOnline.HasValue && !base.Character.IsClientOnline.Value))
=======
			if (base.Character.Definition == null || (base.Character.IsClientOnline.HasValue && !base.Character.IsClientOnline.Value))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			UpdateLogicalWeaponPosition();
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				if (timeAdvanced)
				{
					m_backkickSpeed *= 0.85f;
					m_backkickPos = m_backkickPos * 0.5f + m_backkickSpeed;
				}
				UpdateIkTransitions();
				UpdateGraphicalWeaponPosition();
			}
			m_lastStateWasFalling = base.Character.IsFalling;
			m_lastStateWasCrouching = base.Character.IsCrouching;
			if (timeAdvanced)
			{
				m_suppressBouncingForTimeSec -= 0.0166666675f;
				if (m_suppressBouncingForTimeSec < 0f)
				{
					m_suppressBouncingForTimeSec = 0f;
				}
			}
		}

		/// <summary>
		/// Update and get current weights of the weapon variants.
		/// </summary>
		/// <returns>Weights. Normalized.</returns>
		private Vector4D UpdateAndGetWeaponVariantWeights(MyHandItemDefinition handItemDefinition)
		{
			base.Character.AnimationController.Variables.GetValue(MyAnimationVariableStorageHints.StrIdSpeed, out var value);
			bool flag = (base.Character.IsSprinting || MyCharacter.IsRunningState(base.Character.GetPreviousMovementState())) && value > base.Character.Definition.MaxWalkSpeed;
			if (base.Character.CurrentWeapon != null)
			{
				IsShooting = ((base.Character.IsShooting(MyShootActionEnum.PrimaryAction) && base.Character.CurrentWeapon.GetShakeOnAction(MyShootActionEnum.PrimaryAction)) || (base.Character.IsShooting(MyShootActionEnum.SecondaryAction) && base.Character.CurrentWeapon.GetShakeOnAction(MyShootActionEnum.SecondaryAction)) || (base.Character.IsShooting(MyShootActionEnum.TertiaryAction) && base.Character.CurrentWeapon.GetShakeOnAction(MyShootActionEnum.TertiaryAction))) && !base.Character.IsSprinting;
			}
			IsInIronSight = base.Character.ZoomMode == MyZoomModeEnum.IronSight && !base.Character.IsSprinting;
			ShouldSupressShootAnimation = base.Character.ShouldSupressShootAnimation;
			bool isShooting = IsShooting;
			bool isInIronSight = IsInIronSight;
			float num = 0.0166666675f / handItemDefinition.BlendTime;
			float num2 = 0.0166666675f / handItemDefinition.ShootBlend;
			m_weaponPositionVariantWeightCounters.X += ((!flag && !isShooting && !isInIronSight) ? num : ((isShooting || isInIronSight) ? (0f - num2) : (0f - num)));
			m_weaponPositionVariantWeightCounters.Y += ((flag && !isShooting && !isInIronSight) ? num : ((isShooting || isInIronSight) ? (0f - num2) : (0f - num)));
			m_weaponPositionVariantWeightCounters.Z += ((isShooting && !isInIronSight) ? num2 : (isInIronSight ? (0f - num2) : (0f - num)));
			m_weaponPositionVariantWeightCounters.W += (isInIronSight ? num2 : (isShooting ? (0f - num2) : (0f - num)));
			m_weaponPositionVariantWeightCounters = Vector4.Clamp(m_weaponPositionVariantWeightCounters, Vector4.Zero, Vector4.One);
			Vector4D vector4D = new Vector4D(MathHelper.SmoothStep(0f, 1f, m_weaponPositionVariantWeightCounters.X), MathHelper.SmoothStep(0f, 1f, m_weaponPositionVariantWeightCounters.Y), MathHelper.SmoothStep(0f, 1f, m_weaponPositionVariantWeightCounters.Z), MathHelper.SmoothStep(0f, 1f, m_weaponPositionVariantWeightCounters.W));
			double num3 = vector4D.X + vector4D.Y + vector4D.Z + vector4D.W;
			return vector4D / num3;
		}

		/// <summary>
		/// Update shown position of the weapon.
		/// </summary>
		private void UpdateGraphicalWeaponPosition()
		{
			MyAnimationControllerComponent animationController = base.Character.AnimationController;
			MyHandItemDefinition handItemDefinition = base.Character.HandItemDefinition;
			if (handItemDefinition != null && base.Character.CurrentWeapon != null && animationController.CharacterBones != null)
			{
				bool flag = base.Character.ControllerInfo.IsLocallyControlled() && MySession.Static.CameraController == base.Character;
				bool flag2 = (base.Character.IsInFirstPersonView || base.Character.ForceFirstPersonCamera) && flag;
				if (MyFakes.FORCE_CHARTOOLS_1ST_PERSON)
				{
					flag2 = true;
				}
				bool jetpackRunning = base.Character.JetpackRunning;
				if (m_lastStateWasFalling && jetpackRunning)
				{
					m_currentAnimationToIkTime = m_animationToIKDelay * (float)Math.Cos(base.Character.HeadLocalXAngle - m_lastLocalRotX);
				}
				if (m_lastStateWasCrouching != base.Character.IsCrouching)
				{
					m_suppressBouncingForTimeSec = m_suppressBouncingDelay;
				}
				if (m_suppressBouncingForTimeSec > 0f)
				{
					m_spineRestPositionX.Clear();
					m_spineRestPositionY.Clear();
					m_spineRestPositionZ.Clear();
				}
				m_lastLocalRotX = base.Character.HeadLocalXAngle;
				if (flag2)
				{
					UpdateGraphicalWeaponPosition1st(handItemDefinition);
				}
				else
				{
					UpdateGraphicalWeaponPosition3rd(handItemDefinition);
				}
			}
		}

		private void UpdateGraphicalWeaponPosition1st(MyHandItemDefinition handItemDefinition)
		{
			bool jetpackRunning = base.Character.JetpackRunning;
			MyAnimationControllerComponent animationController = base.Character.AnimationController;
			MatrixD matrixD = base.Character.GetHeadMatrix(includeY: true, !jetpackRunning, forceHeadAnim: false, forceHeadBone: true, preferLocalOverSync: true) * base.Character.PositionComp.WorldMatrixInvScaled;
			MatrixD matrixD2 = handItemDefinition.ItemLocation;
			MatrixD matrixD3 = handItemDefinition.ItemWalkingLocation;
			MatrixD matrixD4 = handItemDefinition.ItemShootLocation;
			MatrixD matrixD5 = handItemDefinition.ItemIronsightLocation;
			MatrixD matrixD6 = (animationController.CharacterBones.IsValidIndex(base.Character.WeaponBone) ? (GetWeaponRelativeMatrix() * animationController.CharacterBones[base.Character.WeaponBone].AbsoluteTransform) : GetWeaponRelativeMatrix());
			Vector4D vector4D = UpdateAndGetWeaponVariantWeights(handItemDefinition);
			MatrixD matrix = vector4D.X * matrixD2 + vector4D.Y * matrixD3 + vector4D.Z * matrixD4 + vector4D.W * matrixD5;
			matrix = MatrixD.Normalize(matrix);
			double num = 0.0;
			if (handItemDefinition.ItemPositioning == MyItemPositioningEnum.TransformFromData)
			{
				num += vector4D.X;
			}
			if (handItemDefinition.ItemPositioningWalk == MyItemPositioningEnum.TransformFromData)
			{
				num += vector4D.Y;
			}
			if (handItemDefinition.ItemPositioningShoot == MyItemPositioningEnum.TransformFromData)
			{
				num += vector4D.Z;
			}
			if (handItemDefinition.ItemPositioningIronsight == MyItemPositioningEnum.TransformFromData)
			{
				num += vector4D.W;
			}
			num /= vector4D.X + vector4D.Y + vector4D.Z + vector4D.W;
			double num2 = 0.0;
			if (handItemDefinition.ItemPositioning != MyItemPositioningEnum.TransformFromAnim)
			{
				num2 += vector4D.X;
			}
			if (handItemDefinition.ItemPositioningWalk != MyItemPositioningEnum.TransformFromAnim)
			{
				num2 += vector4D.Y;
			}
			if (handItemDefinition.ItemPositioningShoot != MyItemPositioningEnum.TransformFromAnim)
			{
				num2 += vector4D.Z;
			}
			if (handItemDefinition.ItemPositioningIronsight != MyItemPositioningEnum.TransformFromAnim)
			{
				num2 += vector4D.W;
			}
			num2 /= vector4D.X + vector4D.Y + vector4D.Z + vector4D.W;
			MatrixD weaponMatrixLocal = matrix * matrixD;
			ApplyWeaponBouncing(handItemDefinition, ref weaponMatrixLocal, (float)(1.0 - 0.95 * vector4D.W), (float)vector4D.W);
			MyEngineerToolBase myEngineerToolBase = base.Character.CurrentWeapon as MyEngineerToolBase;
			if (myEngineerToolBase != null)
			{
				myEngineerToolBase.SensorDisplacement = -matrix.Translation;
			}
			double num3 = num * (double)m_currentAnimationToIkTime / (double)m_animationToIKDelay;
			MatrixD weaponAnimMatrix = MatrixD.Lerp(matrixD6, weaponMatrixLocal, num3);
			UpdateScattering(ref weaponAnimMatrix, handItemDefinition);
			ApplyBackkick(ref weaponAnimMatrix);
			MatrixD matrixD7 = weaponAnimMatrix * base.Character.WorldMatrix;
			GraphicalPositionWorld = matrixD7.Translation;
			ArmsIkWeight = (float)num2;
			((MyEntity)base.Character.CurrentWeapon).WorldMatrix = matrixD7;
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_TOOLS)
			{
				MyDebugDrawHelper.DrawNamedColoredAxis(matrixD6 * base.Character.WorldMatrix, 0.25f, "weapon anim " + (100.0 - 100.0 * num3) + "%", Color.Orange);
				MyDebugDrawHelper.DrawNamedColoredAxis(weaponMatrixLocal * base.Character.WorldMatrix, 0.25f, "weapon data " + 100.0 * num3 + "%", Color.Magenta);
				MyDebugDrawHelper.DrawNamedColoredAxis(matrixD7, 0.25f, "weapon final", Color.White);
			}
		}

		private void UpdateGraphicalWeaponPosition3rd(MyHandItemDefinition handItemDefinition)
		{
			bool jetpackRunning = base.Character.JetpackRunning;
			MyAnimationControllerComponent animationController = base.Character.AnimationController;
			MatrixD matrixD = base.Character.GetHeadMatrix(includeY: false, !jetpackRunning, forceHeadAnim: false, forceHeadBone: true, preferLocalOverSync: true) * base.Character.PositionComp.WorldMatrixInvScaled;
			if (animationController.CharacterBones.IsValidIndex(base.Character.HeadBoneIndex))
			{
				matrixD.M42 += animationController.CharacterBonesSorted[0].Translation.Y;
			}
			MatrixD matrixD2 = handItemDefinition.ItemLocation3rd;
			MatrixD matrixD3 = handItemDefinition.ItemWalkingLocation3rd;
			MatrixD matrixD4 = handItemDefinition.ItemShootLocation3rd;
			MatrixD matrixD5 = handItemDefinition.ItemIronsightLocation;
			MatrixD matrixD6 = (animationController.CharacterBones.IsValidIndex(base.Character.WeaponBone) ? (GetWeaponRelativeMatrix() * animationController.CharacterBones[base.Character.WeaponBone].AbsoluteTransform) : GetWeaponRelativeMatrix());
			Vector4D vector4D = UpdateAndGetWeaponVariantWeights(handItemDefinition);
			MatrixD matrix = vector4D.X * matrixD2 + vector4D.Y * matrixD3 + vector4D.Z * matrixD4 + vector4D.W * matrixD5;
			matrix = MatrixD.Normalize(matrix);
			double num = 0.0;
			if (handItemDefinition.ItemPositioning3rd == MyItemPositioningEnum.TransformFromData)
			{
				num += vector4D.X;
			}
			if (handItemDefinition.ItemPositioningWalk3rd == MyItemPositioningEnum.TransformFromData)
			{
				num += vector4D.Y;
			}
			if (handItemDefinition.ItemPositioningShoot3rd == MyItemPositioningEnum.TransformFromData)
			{
				num += vector4D.Z;
			}
			if (handItemDefinition.ItemPositioningIronsight3rd == MyItemPositioningEnum.TransformFromData)
			{
				num += vector4D.W;
			}
			num /= vector4D.X + vector4D.Y + vector4D.Z + vector4D.W;
			double num2 = 0.0;
			if (handItemDefinition.ItemPositioning3rd != MyItemPositioningEnum.TransformFromAnim)
			{
				num2 += vector4D.X;
			}
			if (handItemDefinition.ItemPositioningWalk3rd != MyItemPositioningEnum.TransformFromAnim)
			{
				num2 += vector4D.Y;
			}
			if (handItemDefinition.ItemPositioningShoot3rd != MyItemPositioningEnum.TransformFromAnim)
			{
				num2 += vector4D.Z;
			}
			if (handItemDefinition.ItemPositioningIronsight3rd != MyItemPositioningEnum.TransformFromAnim)
			{
				num2 += vector4D.W;
			}
			num2 /= vector4D.X + vector4D.Y + vector4D.Z + vector4D.W;
			ApplyWeaponBouncing(handItemDefinition, ref matrix, (float)(1.0 - 0.95 * vector4D.W), 0f);
			matrixD.M43 += 0.5 * matrix.M43 * Math.Max(0.0, matrixD.M32);
			matrixD.M42 += 0.5 * matrix.M42 * Math.Max(0.0, matrixD.M32);
			matrixD.M42 -= 0.25 * Math.Max(0.0, matrixD.M32);
			matrixD.M43 -= 0.05 * Math.Min(0.0, matrixD.M32);
			matrixD.M41 -= 0.25 * Math.Max(0.0, matrixD.M32);
			MatrixD matrixD7 = matrix * matrixD;
			MyEngineerToolBase myEngineerToolBase = base.Character.CurrentWeapon as MyEngineerToolBase;
			if (myEngineerToolBase != null)
			{
				myEngineerToolBase.SensorDisplacement = -matrix.Translation;
			}
			double num3 = num * (double)m_currentAnimationToIkTime / (double)m_animationToIKDelay;
			MatrixD weaponAnimMatrix = MatrixD.Lerp(matrixD6, matrixD7, num3);
			UpdateScattering(ref weaponAnimMatrix, handItemDefinition);
			ApplyBackkick(ref weaponAnimMatrix);
			MatrixD matrixD8 = weaponAnimMatrix * base.Character.WorldMatrix;
			GraphicalPositionWorld = matrixD8.Translation;
			ArmsIkWeight = (float)num2;
			((MyEntity)base.Character.CurrentWeapon).WorldMatrix = matrixD8;
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_TOOLS)
			{
				MyDebugDrawHelper.DrawNamedColoredAxis(matrixD6 * base.Character.WorldMatrix, 0.25f, "weapon anim " + (100.0 - 100.0 * num3) + "%", Color.Orange);
				MyDebugDrawHelper.DrawNamedColoredAxis(matrixD7 * base.Character.WorldMatrix, 0.25f, "weapon data " + 100.0 * num3 + "%", Color.Magenta);
				MyDebugDrawHelper.DrawNamedColoredAxis(matrixD8, 0.25f, "weapon final", Color.White);
			}
		}

		private void UpdateScattering(ref MatrixD weaponAnimMatrix, MyHandItemDefinition handItemDefinition)
		{
			MyEngineerToolBase myEngineerToolBase = base.Character.CurrentWeapon as MyEngineerToolBase;
			bool flag = false;
			if (!(handItemDefinition.ScatterSpeed > 0f))
			{
				return;
			}
			bool flag2 = false;
			if (myEngineerToolBase != null)
			{
				flag2 = myEngineerToolBase.HasHitBlock;
			}
			flag = IsShooting && flag2;
			if (flag || m_currentScatterToAnimRatio < 1f)
			{
				if (m_currentScatterBlend == 0f)
				{
					m_lastScatterPos = Vector3.Zero;
				}
				if (m_currentScatterBlend == handItemDefinition.ScatterSpeed)
				{
					m_lastScatterPos = m_currentScatterPos;
					m_currentScatterBlend = 0f;
				}
				if (m_currentScatterBlend == 0f || m_currentScatterBlend == handItemDefinition.ScatterSpeed)
				{
					m_currentScatterPos = new Vector3(MyUtils.GetRandomFloat((0f - handItemDefinition.ShootScatter.X) / 2f, handItemDefinition.ShootScatter.X / 2f), MyUtils.GetRandomFloat((0f - handItemDefinition.ShootScatter.Y) / 2f, handItemDefinition.ShootScatter.Y / 2f), MyUtils.GetRandomFloat((0f - handItemDefinition.ShootScatter.Z) / 2f, handItemDefinition.ShootScatter.Z / 2f));
				}
				m_currentScatterBlend += 0.01f;
				if (m_currentScatterBlend > handItemDefinition.ScatterSpeed)
				{
					m_currentScatterBlend = handItemDefinition.ScatterSpeed;
				}
				Vector3 vector = Vector3.Lerp(m_lastScatterPos, m_currentScatterPos, m_currentScatterBlend / handItemDefinition.ScatterSpeed);
				weaponAnimMatrix.Translation += (1f - m_currentScatterToAnimRatio) * vector;
			}
			else
			{
				m_currentScatterBlend = 0f;
			}
			m_currentScatterToAnimRatio += (flag ? (-0.1f) : 0.1f);
			if (m_currentScatterToAnimRatio > 1f)
			{
				m_currentScatterToAnimRatio = 1f;
			}
			else if (m_currentScatterToAnimRatio < 0f)
			{
				m_currentScatterToAnimRatio = 0f;
			}
		}

		/// <summary>
		/// Apply bouncing movement on weapon.
		/// </summary>
		/// <param name="handItemDefinition">definition of hand item</param>
		/// <param name="weaponMatrixLocal">current weapon matrix (character local space)</param>
		/// <param name="fpsBounceMultiplier"></param>
		/// <param name="ironsightWeight"></param>
		private void ApplyWeaponBouncing(MyHandItemDefinition handItemDefinition, ref MatrixD weaponMatrixLocal, float fpsBounceMultiplier, float ironsightWeight)
		{
			if (base.Character.AnimationController.CharacterBones.IsValidIndex(base.Character.SpineBoneIndex))
			{
				bool flag = base.Character.ControllerInfo.IsLocallyControlled();
				bool flag2 = (base.Character.IsInFirstPersonView || base.Character.ForceFirstPersonCamera) && flag;
				MyCharacterBone obj = base.Character.AnimationController.CharacterBones[base.Character.SpineBoneIndex];
				Vector3 translation = base.Character.AnimationController.CharacterBonesSorted[0].Translation;
				Vector3 vector = obj.AbsoluteTransform.Translation - translation;
				m_spineRestPositionX.Add(vector.X);
				m_spineRestPositionY.Add(vector.Y);
				m_spineRestPositionZ.Add(vector.Z);
				Vector3 translation2 = obj.GetAbsoluteRigTransform().Translation;
				Vector3 vector2 = new Vector3(translation2.X, m_spineRestPositionY.Get(), translation2.Z);
				Vector3 vector3 = (vector - vector2) * fpsBounceMultiplier;
				vector3.Z = (flag2 ? vector3.Z : 0f);
				m_sprintStatusWeight += (base.Character.IsSprinting ? m_sprintStatusGainSpeed : (0f - m_sprintStatusGainSpeed));
				m_sprintStatusWeight = MathHelper.Clamp(m_sprintStatusWeight, 0f, 1f);
				if (flag2)
				{
					vector3 *= 1f + Math.Max(0f, handItemDefinition.RunMultiplier - 1f) * m_sprintStatusWeight;
					vector3.X *= handItemDefinition.XAmplitudeScale;
					vector3.Y *= handItemDefinition.YAmplitudeScale;
					vector3.Z *= handItemDefinition.ZAmplitudeScale;
				}
				else
				{
					vector3 *= handItemDefinition.AmplitudeMultiplier3rd;
				}
				weaponMatrixLocal.Translation += vector3;
				BoundingBox localAABB = base.Character.PositionComp.LocalAABB;
				if (ironsightWeight < 1f && weaponMatrixLocal.M43 > (double)(translation2.Z + translation.Z) - (double)localAABB.Max.Z * 0.5 - (double)base.Character.HandItemDefinition.RightHand.Translation.Z * 0.75)
				{
					double value = (double)(translation2.Z + translation.Z) - (double)localAABB.Max.Z * 0.5 - (double)base.Character.HandItemDefinition.RightHand.Translation.Z * 0.75;
					weaponMatrixLocal.M43 = MathHelper.Lerp(value, weaponMatrixLocal.M43, ironsightWeight);
				}
				if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_TOOLS)
				{
					MyDebugDrawHelper.DrawNamedPoint(Vector3D.Transform(translation2, base.Character.WorldMatrix), "spine", Color.Gray);
				}
			}
		}

		private void ApplyBackkick(ref MatrixD weaponMatrixLocal)
		{
			weaponMatrixLocal.Translation += weaponMatrixLocal.Backward * m_backkickPos;
		}

		private MatrixD GetWeaponRelativeMatrix()
		{
			if (base.Character.CurrentWeapon != null && base.Character.HandItemDefinition != null && base.Character.AnimationController.CharacterBones.IsValidIndex(base.Character.WeaponBone))
			{
				return MatrixD.Invert(base.Character.HandItemDefinition.RightHand);
			}
			return MatrixD.Identity;
		}

		/// <summary>
		/// Update logical weapon position (model of the weapon will be placed there, bullet will spawn on logical position).
		/// </summary>
		private void UpdateLogicalWeaponPosition()
		{
			Vector3 vector = ((!base.Character.IsCrouching) ? new Vector3(0f, base.Character.Definition.CharacterCollisionHeight - base.Character.Definition.CharacterHeadHeight * 0.5f, 0f) : new Vector3(0f, base.Character.Definition.CharacterCollisionCrouchHeight - base.Character.Definition.CharacterHeadHeight * 0.5f, 0f));
			LogicalPositionLocalSpace = vector;
			LogicalPositionWorld = Vector3D.Transform(LogicalPositionLocalSpace, base.Character.PositionComp.WorldMatrixRef);
			LogicalOrientationWorld = base.Character.ShootDirection;
			LogicalCrosshairPoint = LogicalPositionWorld + LogicalOrientationWorld * 2000.0;
			if (base.Character.CurrentWeapon != null)
			{
				MyEngineerToolBase myEngineerToolBase = base.Character.CurrentWeapon as MyEngineerToolBase;
				if (myEngineerToolBase != null)
				{
					myEngineerToolBase.UpdateSensorPosition();
				}
				else
				{
					(base.Character.CurrentWeapon as MyHandDrill)?.WorldPositionChanged(null);
				}
			}
		}

		/// <summary>
		/// Update interpolation parameters.
		/// </summary>
		internal void UpdateIkTransitions()
		{
			m_animationToIkState = ((base.Character.HandItemDefinition != null && base.Character.CurrentWeapon != null) ? 1 : (-1));
			m_currentAnimationToIkTime += (float)m_animationToIkState * 0.0166666675f;
			if (m_currentAnimationToIkTime >= m_animationToIKDelay)
			{
				m_currentAnimationToIkTime = m_animationToIKDelay;
			}
			else if (m_currentAnimationToIkTime <= 0f)
			{
				m_currentAnimationToIkTime = 0f;
			}
		}

		public void AddBackkick(float backkickForce)
		{
			m_backkickSpeed = Math.Max(m_backkickSpeed, backkickForce * 1f);
		}
	}
}
