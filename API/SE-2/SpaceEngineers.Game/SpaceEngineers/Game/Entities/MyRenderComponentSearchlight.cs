using System;
using System.Collections.Generic;
using Sandbox.Game.Components;
using Sandbox.Game.Lights;
using Sandbox.Game.World;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.Entities
{
	public class MyRenderComponentSearchlight : MyRenderComponentLight
	{
		private const float RADIUS_TO_CONE_MULTIPLIER = 0.25f;

		private const float SMALL_LENGTH_MULTIPLIER = 0.5f;

		private MySearchlight m_reflectorLight;

		public List<MyLight> Lights { get; set; }

		public float LightShaftOffset { get; set; }

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_reflectorLight = base.Container.Entity as MySearchlight;
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
			foreach (MyLight light in Lights)
			{
				float num = Math.Abs(Vector3.Dot(Vector3.Normalize(MySector.MainCamera.Position - m_reflectorLight.PositionComp.GetPosition()), light.ReflectorDirection));
				float num2 = MathHelper.Saturate(1f - (float)Math.Pow(num, 30.0));
				uint parentID = light.ParentID;
				Vector3D position = light.Position;
				Vector3 reflectorDirection = light.ReflectorDirection;
				float num3 = Math.Max(15f, m_reflectorLight.ReflectorRadius * 0.25f);
				if (!m_reflectorLight.IsLargeLight)
				{
					num3 *= 0.5f;
				}
				float reflectorThickness = m_reflectorLight.BlockDefinition.ReflectorThickness;
				Color color = m_reflectorLight.Color;
				float n = m_reflectorLight.CurrentLightPower * m_reflectorLight.Intensity * 0.8f;
				MyTransparentGeometry.AddLocalLineBillboard(MyStringId.GetOrCompute(m_reflectorLight.ReflectorConeMaterial), color.ToVector4() * num2 * MathHelper.Saturate(n), position + reflectorDirection * LightShaftOffset, parentID, reflectorDirection, num3, reflectorThickness, MyBillboard.BlendTypeEnum.AdditiveBottom);
			}
		}
	}
}
