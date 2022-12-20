using System;
using System.Collections.Generic;
using VRage.Network;
using VRageMath;
using VRageRender.Animations;

namespace VRage.Render.Particles
{
	[GenerateActivator]
	public class MyParticleLightData
	{
		private enum MyLightPropertiesEnum
		{
			Position,
			PositionVar,
			Color,
			ColorVar,
			Range,
			RangeVar,
			Intensity,
			IntensityVar,
			Enabled,
			GravityDisplacement,
			Falloff,
			VarianceTimeout
		}

<<<<<<< HEAD
=======
		private static readonly int Version;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private string m_name;

		private MyParticleEffectData m_owner;

<<<<<<< HEAD
=======
		private uint m_renderObjectID = uint.MaxValue;

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

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private readonly IMyConstProperty[] m_properties = new IMyConstProperty[Enum.GetValues(typeof(MyLightPropertiesEnum)).Length];

		/// <summary>
		/// Public members to easy access
		/// </summary>
		public MyAnimatedPropertyVector3 Position
		{
			get
			{
				return (MyAnimatedPropertyVector3)m_properties[0];
			}
			private set
			{
				m_properties[0] = value;
			}
		}

		public MyAnimatedPropertyVector3 PositionVar
		{
			get
			{
				return (MyAnimatedPropertyVector3)m_properties[1];
			}
			private set
			{
				m_properties[1] = value;
			}
		}

		public MyAnimatedPropertyVector4 Color
		{
			get
			{
				return (MyAnimatedPropertyVector4)m_properties[2];
			}
			private set
			{
				m_properties[2] = value;
			}
		}

		public MyAnimatedPropertyFloat ColorVar
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[3];
			}
			private set
			{
				m_properties[3] = value;
			}
		}

		public MyAnimatedPropertyFloat Range
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[4];
			}
			private set
			{
				m_properties[4] = value;
			}
		}

		public MyAnimatedPropertyFloat RangeVar
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[5];
			}
			private set
			{
				m_properties[5] = value;
			}
		}

		public MyAnimatedPropertyFloat Intensity
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[6];
			}
			private set
			{
				m_properties[6] = value;
			}
		}

		public MyAnimatedPropertyFloat IntensityVar
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[7];
			}
			private set
			{
				m_properties[7] = value;
			}
		}

		public MyConstPropertyBool Enabled
		{
			get
			{
				return (MyConstPropertyBool)m_properties[8];
			}
			private set
			{
				m_properties[8] = value;
			}
		}

		public MyConstPropertyFloat GravityDisplacement
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[9];
			}
			private set
			{
				m_properties[9] = value;
			}
		}

		public MyConstPropertyFloat Falloff
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[10];
			}
			private set
			{
				m_properties[10] = value;
			}
		}

		public MyConstPropertyFloat VarianceTimeout
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[11];
			}
			private set
			{
				m_properties[11] = value;
			}
		}

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

		public void Start(MyParticleEffectData owner)
		{
			m_owner = owner;
			m_name = "ParticleLight";
			AddProperty(MyLightPropertiesEnum.Position, new MyAnimatedPropertyVector3("Position", "Position of the light relative to the effect position."));
			AddProperty(MyLightPropertiesEnum.PositionVar, new MyAnimatedPropertyVector3("Position var", "Random variation of the position."));
			AddProperty(MyLightPropertiesEnum.Color, new MyAnimatedPropertyVector4("Color", "Color of the light."));
			AddProperty(MyLightPropertiesEnum.ColorVar, new MyAnimatedPropertyFloat("Color var", "Light color random multiplier."));
			AddProperty(MyLightPropertiesEnum.Range, new MyAnimatedPropertyFloat("Range", "Radius of the light."));
			AddProperty(MyLightPropertiesEnum.RangeVar, new MyAnimatedPropertyFloat("Range var", "Random variabily of light radius."));
			AddProperty(MyLightPropertiesEnum.Intensity, new MyAnimatedPropertyFloat("Intensity", "Intensity strength of the light."));
			AddProperty(MyLightPropertiesEnum.IntensityVar, new MyAnimatedPropertyFloat("Intensity var", "Random variablity of the intensity."));
			AddProperty(MyLightPropertiesEnum.GravityDisplacement, new MyConstPropertyFloat("Gravity Displacement", "Light position offset in gravity direction."));
			AddProperty(MyLightPropertiesEnum.Falloff, new MyConstPropertyFloat("Falloff", "The range from full light to non lit distance."));
			AddProperty(MyLightPropertiesEnum.VarianceTimeout, new MyConstPropertyFloat("Variance Timeout", ""));
			AddProperty(MyLightPropertiesEnum.Enabled, new MyConstPropertyBool("Enabled", "Light enabled or disabled."));
			InitDefault();
		}

		private T AddProperty<T>(MyLightPropertiesEnum e, T property) where T : IMyConstProperty
		{
			m_properties[(int)e] = property;
			return property;
		}

		public IEnumerable<IMyConstProperty> GetProperties()
		{
			return m_properties;
		}

		public void InitDefault()
		{
			Color.AddKey(0f, Vector4.One);
			Range.AddKey(0f, 2.5f);
			Intensity.AddKey(0f, 10f);
			Falloff.SetValue(1f);
			VarianceTimeout.SetValue(0.1f);
			Enabled.SetValue(val: true);
		}

		public MyParticleLightData Duplicate(MyParticleEffectData newOwner)
		{
			MyParticleLightData myParticleLightData = new MyParticleLightData();
			myParticleLightData.Start(newOwner);
			myParticleLightData.Name = Name;
			for (int i = 0; i < m_properties.Length; i++)
			{
				myParticleLightData.m_properties[i] = m_properties[i].Duplicate();
			}
			return myParticleLightData;
		}

		public MyParticleEffectData GetOwner()
		{
			return m_owner;
		}

		public ParticleLight SerializeToObjectBuilder()
		{
			ParticleLight particleLight = new ParticleLight();
			particleLight.Name = m_name;
			particleLight.Properties = new List<GenerationProperty>();
			IMyConstProperty[] properties = m_properties;
			for (int i = 0; i < properties.Length; i++)
			{
				GenerationProperty item = properties[i].SerializeToObjectBuilder();
				particleLight.Properties.Add(item);
			}
			return particleLight;
		}

		public void DeserializeFromObjectBuilder(ParticleLight light)
		{
			m_name = light.Name;
			foreach (GenerationProperty property in light.Properties)
			{
				for (int i = 0; i < m_properties.Length; i++)
				{
					if (m_properties[i].Name.Equals(property.Name))
					{
						m_properties[i].DeserializeFromObjectBuilder(property);
					}
				}
			}
		}
	}
}
