using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentCharacter : MyDebugRenderComponent
	{
		private MyCharacter m_character;

		private List<Matrix> m_simulatedBonesDebugDraw = new List<Matrix>();

		private List<Matrix> m_simulatedBonesAbsoluteDebugDraw = new List<Matrix>();

		public MyDebugRenderComponentCharacter(MyCharacter character)
			: base(character)
		{
			m_character = character;
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC && m_character.CurrentWeapon != null)
			{
				MyRenderProxy.DebugDrawAxis(((MyEntity)m_character.CurrentWeapon).WorldMatrix, 1.4f, depthRead: false);
				MyRenderProxy.DebugDrawText3D(((MyEntity)m_character.CurrentWeapon).WorldMatrix.Translation, "Weapon", Color.White, 0.7f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				MyRenderProxy.DebugDrawSphere((m_character.AnimationController.CharacterBones[m_character.WeaponBone].AbsoluteTransform * m_character.PositionComp.WorldMatrixRef).Translation, 0.02f, Color.White, 1f, depthRead: false);
				MyRenderProxy.DebugDrawText3D((m_character.AnimationController.CharacterBones[m_character.WeaponBone].AbsoluteTransform * m_character.PositionComp.WorldMatrixRef).Translation, "Weapon Bone", Color.White, 1f, depthRead: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC && m_character.IsUsing != null)
			{
				MatrixD m = m_character.IsUsing.WorldMatrix;
				Matrix m2 = m;
				m2.Translation = Vector3.Zero;
				m2 *= Matrix.CreateFromAxisAngle(m2.Up, 3.141593f);
				Vector3 vector = m_character.IsUsing.PositionComp.GetPosition() - m_character.IsUsing.WorldMatrix.Up * MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large) / 2.0;
				vector = (m2.Translation = vector + m2.Up * 0.28f - m2.Forward * 0.22f);
				MyRenderProxy.DebugDrawAxis(m2, 1.4f, depthRead: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_SUIT_BATTERY_CAPACITY)
			{
				MatrixD worldMatrixRef = m_character.PositionComp.WorldMatrixRef;
				MyRenderProxy.DebugDrawText3D(worldMatrixRef.Translation + 2.0 * worldMatrixRef.Up, $"{m_character.SuitBattery.ResourceSource.RemainingCapacity} MWh", Color.White, 1f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			}
			m_simulatedBonesDebugDraw.Clear();
			m_simulatedBonesAbsoluteDebugDraw.Clear();
			if (!MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_BONES)
			{
				return;
			}
			m_character.AnimationController.UpdateTransformations();
			for (int i = 0; i < m_character.AnimationController.CharacterBones.Length; i++)
			{
				MyCharacterBone myCharacterBone = m_character.AnimationController.CharacterBones[i];
				if (myCharacterBone.Parent != null)
				{
					MatrixD matrix = Matrix.CreateScale(0.1f) * myCharacterBone.AbsoluteTransform * m_character.PositionComp.WorldMatrixRef;
<<<<<<< HEAD
					Vector3D translation = matrix.Translation;
					Vector3D translation2 = (myCharacterBone.Parent.AbsoluteTransform * m_character.PositionComp.WorldMatrixRef).Translation;
					MyRenderProxy.DebugDrawLine3D(translation2, translation, Color.White, Color.White, depthRead: false);
					MyRenderProxy.DebugDrawText3D((translation2 + translation) * 0.5, myCharacterBone.Name + " (" + i + ")", Color.Red, 0.5f, depthRead: false);
=======
					Vector3 vector3 = matrix.Translation;
					Vector3 vector4 = (myCharacterBone.Parent.AbsoluteTransform * m_character.PositionComp.WorldMatrixRef).Translation;
					MyRenderProxy.DebugDrawLine3D(vector4, vector3, Color.White, Color.White, depthRead: false);
					MyRenderProxy.DebugDrawText3D((vector4 + vector3) * 0.5f, myCharacterBone.Name + " (" + i + ")", Color.Red, 0.5f, depthRead: false);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MyRenderProxy.DebugDrawAxis(matrix, 0.1f, depthRead: false);
				}
			}
		}
	}
}
