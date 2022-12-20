using VRage.Network;
using VRage.Render.Particles;
using VRage.Render.Scene;
using VRage.Render11.Scene;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace VRage.Render11.Particles
{
	[GenerateActivator]
	internal sealed class MyParticleLight
	{
		private class VRage_Render11_Particles_MyParticleLight_003C_003EActor : IActivator, IActivator<MyParticleLight>
		{
			private sealed override object CreateInstance()
			{
				return new MyParticleLight();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyParticleLight CreateInstance()
			{
				return new MyParticleLight();
			}

			MyParticleLight IActivator<MyParticleLight>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private string m_name;

		private MyRenderParticleEffect m_effect;

		private MyParticleLightData m_data;

		private UpdateRenderLightData m_renderLightData;

		private Vector3D m_position;

		private Vector4 m_color;

		private float m_range;

		private float m_intensity;

		private float m_falloff;

		private Vector3 m_localPositionVarRnd;

		private float m_colorVarRnd;

		private float m_lastVarianceTime;

		private float m_rangeVarRnd;

		private float m_intensityRnd;

		private uint m_parentID;

		private MyActor m_light;

		public bool Enabled { get; set; }

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		public void Init(MyRenderParticleEffect effect, MyParticleLightData data)
		{
			Enabled = true;
			m_effect = effect;
			m_data = data;
			m_name = "ParticleLight";
			m_parentID = uint.MaxValue;
			m_renderLightData = new UpdateRenderLightData
			{
				PointLightOn = true,
				PointLight = new MyLightLayout
				{
					GlossFactor = 1f,
					DiffuseFactor = 3.14f
				},
				SpotLight = new MySpotLightLayout
				{
					Up = Vector3.Up,
					Direction = Vector3.Forward
				}
			};
		}

		public void Done()
		{
			m_effect = null;
			CloseLight();
		}

		private bool InitLight()
		{
			if (m_light == null)
			{
				m_light = MyActorFactory.CreateLight(Name);
				return true;
			}
			return false;
		}

		private void CloseLight()
		{
			m_light?.Destroy();
			m_light = null;
		}

		public void Update()
		{
			bool flag = false;
			if ((bool)m_data.Enabled && Enabled)
			{
				flag = InitLight();
				float num = m_effect.GetElapsedTime() - m_lastVarianceTime;
				bool num2 = num > (float)m_data.VarianceTimeout || num < 0f;
				if (num2)
				{
					m_lastVarianceTime = m_effect.GetElapsedTime();
				}
				m_data.Position.GetInterpolatedValue(m_effect.GetLoopTime(), out var value);
				if (num2)
				{
					m_data.PositionVar.GetInterpolatedValue(m_effect.GetLoopTime(), out var value2);
					m_localPositionVarRnd = new Vector3(MyUtils.GetRandomFloat(0f - value2.X, value2.X), MyUtils.GetRandomFloat(0f - value2.Y, value2.Y), MyUtils.GetRandomFloat(0f - value2.Z, value2.Z));
				}
				value += m_localPositionVarRnd;
				m_data.Color.GetInterpolatedValue(m_effect.GetLoopTime(), out var value3);
				if (num2)
				{
					m_data.ColorVar.GetInterpolatedValue(m_effect.GetLoopTime(), out var value4);
					m_colorVarRnd = MyUtils.GetRandomFloat(1f - value4, 1f + value4);
					value3.X = MathHelper.Clamp(value3.X * m_colorVarRnd, 0f, 1f);
					value3.Y = MathHelper.Clamp(value3.Y * m_colorVarRnd, 0f, 1f);
					value3.Z = MathHelper.Clamp(value3.Z * m_colorVarRnd, 0f, 1f);
				}
				m_data.Range.GetInterpolatedValue(m_effect.GetLoopTime(), out var value5);
				if (num2)
				{
					m_data.RangeVar.GetInterpolatedValue(m_effect.GetLoopTime(), out var value6);
					m_rangeVarRnd = MyUtils.GetRandomFloat(0f - value6, value6);
				}
				value5 += m_rangeVarRnd;
				m_data.Intensity.GetInterpolatedValue(m_effect.GetLoopTime(), out var value7);
				if (num2)
				{
					m_data.IntensityVar.GetInterpolatedValue(m_effect.GetLoopTime(), out var value8);
					m_intensityRnd = MyUtils.GetRandomFloat(0f - value8, value8);
				}
				value7 += m_intensityRnd;
				value7 *= m_effect.UserBirthMultiplier;
				if (m_effect.IsStopped)
				{
					value7 = 0f;
				}
<<<<<<< HEAD
=======
				Vector3D vector3D = Vector3D.Transform(value * m_effect.GetScale(), m_effect.WorldMatrix);
				if (m_effect.Gravity.LengthSquared() > 0.0001f)
				{
					Vector3 gravity = m_effect.Gravity;
					gravity.Normalize();
					vector3D += gravity * m_data.GravityDisplacement;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (m_parentID != m_effect.ParentID)
				{
					m_parentID = m_effect.ParentID;
					MyActor parent = MyIDTracker<MyActor>.FindByID(m_parentID);
					MyScene11.Instance.SetActorParent(parent, m_light, null);
				}
<<<<<<< HEAD
				MatrixD worldMatrix = m_effect.WorldMatrix;
				MyActor myActor = MyIDTracker<MyActor>.FindByID(m_parentID);
				Vector3D result = Vector3D.Zero;
				if (myActor != null)
				{
					MatrixD matrix = myActor.WorldMatrix;
					Vector3D translation = worldMatrix.Translation;
					value += (Vector3)translation;
					Vector3D.Transform(ref value, ref matrix, out result);
				}
				else
				{
					result = Vector3D.Transform(value * m_effect.GetScale(), worldMatrix);
				}
				if (m_effect.Gravity.LengthSquared() > 0.0001f)
				{
					Vector3 gravity = m_effect.Gravity;
					gravity.Normalize();
					result += gravity * m_data.GravityDisplacement;
				}
				bool flag2 = m_position != result;
=======
				bool flag2 = m_position != vector3D;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				bool flag3 = m_range != value5;
				bool flag4 = m_falloff != (float)m_data.Falloff;
				if (flag2 || m_color != value3 || flag3 || m_intensity != value7 || flag || flag4)
				{
					m_color = value3;
					m_intensity = value7;
					m_range = value5;
<<<<<<< HEAD
					m_position = result;
=======
					m_position = vector3D;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_falloff = m_data.Falloff;
					m_renderLightData.PointLight.Range = value5 * m_effect.GetScale();
					m_renderLightData.PointLight.Color = new Vector3(m_color) * m_intensity;
					m_renderLightData.PointLight.Falloff = m_falloff;
					m_renderLightData.Position = m_position;
					m_renderLightData.PositionChanged = flag2;
					m_renderLightData.AabbChanged = flag3;
					m_renderLightData.PointIntensity = m_intensity;
					m_light.GetLight().UpdateData(ref m_renderLightData);
				}
			}
			else
			{
				CloseLight();
			}
		}

		public MyRenderParticleEffect GetEffect()
		{
			return m_effect;
		}
	}
}
