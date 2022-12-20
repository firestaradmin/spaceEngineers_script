using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.Lights;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	public class MyRenderComponentReflectorLight : MyRenderComponentLight
	{
		private class Sandbox_Game_Components_MyRenderComponentReflectorLight_003C_003EActor : IActivator, IActivator<MyRenderComponentReflectorLight>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentReflectorLight();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentReflectorLight CreateInstance()
			{
				return new MyRenderComponentReflectorLight();
			}

			MyRenderComponentReflectorLight IActivator<MyRenderComponentReflectorLight>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const float RADIUS_TO_CONE_MULTIPLIER = 0.25f;

		private const float SMALL_LENGTH_MULTIPLIER = 0.5f;

		private MyReflectorLight m_reflectorLight;

		public List<MyLight> Lights { get; set; }

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_reflectorLight = base.Container.Entity as MyReflectorLight;
		}

		public override void AddRenderObjects()
		{
			base.AddRenderObjects();
			BoundingBox localAABB = m_reflectorLight.PositionComp.LocalAABB;
			localAABB.Inflate(m_reflectorLight.IsLargeLight ? 3f : 1f);
			float num = m_reflectorLight.ReflectorRadiusBounds.Max * 0.25f;
			if (!m_reflectorLight.IsLargeLight)
			{
				num *= 0.5f;
			}
			localAABB = localAABB.Include(new Vector3(0f, 0f, 0f - num));
			MyRenderProxy.UpdateRenderObject(m_renderObjectIDs[0], null, localAABB);
		}

		public override void Draw()
		{
			base.Draw();
			if (m_reflectorLight.IsReflectorEnabled)
			{
				DrawReflectorCone();
			}
		}

		private void DrawReflectorCone()
		{
			if (string.IsNullOrEmpty(m_reflectorLight.ReflectorConeMaterial))
			{
				return;
			}
<<<<<<< HEAD
			foreach (MyLight light in Lights)
=======
			foreach (MyLight light in m_lights)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Vector3 vector = Vector3.Normalize(MySector.MainCamera.Position - m_reflectorLight.PositionComp.GetPosition());
				Vector3.TransformNormal(light.ReflectorDirection, m_reflectorLight.PositionComp.WorldMatrixRef);
				float num = Math.Abs(Vector3.Dot(vector, light.ReflectorDirection));
				float num2 = MathHelper.Saturate(1f - (float)Math.Pow(num, 30.0));
				uint parentID = light.ParentID;
				Vector3D position = light.Position;
<<<<<<< HEAD
				Vector3 reflectorDirection = light.ReflectorDirection;
=======
				Vector3D vector3D = light.ReflectorDirection;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				float num3 = Math.Max(15f, m_reflectorLight.ReflectorRadius * 0.25f);
				if (!m_reflectorLight.IsLargeLight)
				{
					num3 *= 0.5f;
				}
				float reflectorThickness = m_reflectorLight.BlockDefinition.ReflectorThickness;
				Color color = m_reflectorLight.Color;
				float n = m_reflectorLight.CurrentLightPower * m_reflectorLight.Intensity * 0.8f;
<<<<<<< HEAD
				MyTransparentGeometry.AddLocalLineBillboard(MyStringId.GetOrCompute(m_reflectorLight.ReflectorConeMaterial), color.ToVector4() * num2 * MathHelper.Saturate(n), position, parentID, reflectorDirection, num3, reflectorThickness, MyBillboard.BlendTypeEnum.AdditiveBottom);
=======
				MyTransparentGeometry.AddLocalLineBillboard(MyStringId.GetOrCompute(m_reflectorLight.ReflectorConeMaterial), color.ToVector4() * num2 * MathHelper.Saturate(n), position, parentID, vector3D, num3, reflectorThickness, MyBillboard.BlendTypeEnum.AdditiveBottom);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
}
