using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.RenderDirect.ActorComponents;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.Render.Scene.Components;
using VRageRender;
using VRageRender.Messages;

namespace SpaceEngineers.Game.EntityComponents.Renders
{
	public class MyRenderComponentHeatVent : MyRenderComponentCubeBlockWithParentedSubpart
	{
		public class MyHeatVentSubpartRenderComponent<TComponent> : MyParentedSubpartRenderComponent where TComponent : MyRenderDirectComponent
		{
			private float m_speed;

			private bool m_animatorInitialized;

			protected MyHeatVentBlockDefinition Definition => ((MyHeatVentBlock)base.Entity.Parent).BlockDefinition;

			public void SetSpeed(float speed)
			{
				if (speed != m_speed)
				{
					m_speed = speed;
					SendSpeed();
				}
			}

			private void SendSpeed()
			{
				if (m_animatorInitialized)
				{
					uint renderObjectID = GetRenderObjectID();
					if (renderObjectID != uint.MaxValue)
					{
						FloatData.Update<TComponent>(renderObjectID, m_speed);
					}
				}
			}

			public override void OnParented()
			{
				base.OnParented();
				m_animatorInitialized = true;
				SendSpeed();
			}
		}

		public class MyChokeRenderComponent : MyHeatVentSubpartRenderComponent<MyRotationAnimator>
		{
			private MyObjectBuilder_HeatVentBlockDefinition.SubpartRotation m_subpartRotation;

			public float AnimationOffset { get; set; }

			public MyObjectBuilder_HeatVentBlockDefinition.SubpartRotation SubpartRotation
			{
				get
				{
					if (m_subpartRotation == null)
					{
						m_subpartRotation = new MyObjectBuilder_HeatVentBlockDefinition.SubpartRotation();
					}
					return m_subpartRotation;
				}
				set
				{
					m_subpartRotation = value;
				}
			}

			public override void OnParented()
			{
				if (!((MyCubeGrid)base.Entity.Parent.Parent).IsPreview)
				{
					MyRenderProxy.UpdateRenderComponent(GetRenderObjectID(), this, delegate(MyRotationAnimatorInitData message, MyChokeRenderComponent thiz)
					{
						message.SpinUpSpeed = 0.5f;
						message.SpinDownSpeed = 0.5f;
						message.RotationAxis = MyRotationAnimator.RotationAxis.AxisZ;
						message.MinTransformAdjust = m_subpartRotation.MinimalPositionAdjustDegrees;
						message.MaxTransformAdjust = m_subpartRotation.MaximalPositionAdjustDegrees;
						message.RelatedEntity = MyEntities.GetEntityById(base.Entity.EntityId);
						message.ReveseRotationDirection = SubpartRotation.ReveseRotationDirection;
						message.SkipToFinalPosition = true;
					});
					base.OnParented();
				}
			}

			public void SetDesiredPosition(float desiredPosition)
			{
				MyRenderProxy.UpdateRenderComponent(GetRenderObjectID(), this, delegate(MyRotationAnimatorInitData message, MyChokeRenderComponent thiz)
				{
					_ = base.Definition;
					message.SpinUpSpeed = 0.5f;
					message.SpinDownSpeed = 0.5f;
					message.RotationAxis = MyRotationAnimator.RotationAxis.AxisZ;
					message.MinTransformAdjust = m_subpartRotation.MinimalPositionAdjustDegrees;
					message.MaxTransformAdjust = m_subpartRotation.MaximalPositionAdjustDegrees;
					message.ReveseRotationDirection = SubpartRotation.ReveseRotationDirection;
					message.DesiredState = desiredPosition;
					message.SkipToFinalPosition = null;
				});
			}
		}
	}
}
