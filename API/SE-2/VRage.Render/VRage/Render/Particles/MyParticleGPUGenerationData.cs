using System.Collections.Generic;
using System.Linq;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;
using VRageRender.Utils;

namespace VRage.Render.Particles
{
	[GenerateActivator]
	public class MyParticleGPUGenerationData
	{
		public enum MyRotationReference
		{
			Camera,
			Local,
			LocalAndCamera
		}

		private enum MyGPUGenerationPropertiesEnum
		{
			ArraySize,
			ArrayOffset,
			ArrayModulo,
			Color,
			ColorIntensity,
			Bounciness,
			EmitterSize,
			EmitterSizeMin,
			Direction,
			Velocity,
			VelocityVar,
			DirectionInnerCone,
			DirectionConeVar,
			Acceleration,
			AccelerationFactor,
			RotationVelocity,
			Radius,
			Life,
			SoftParticleDistanceScale,
			StreakMultiplier,
			AnimationFrameTime,
			Enabled,
			ParticlesPerSecond,
			Material,
			OITWeightFactor,
			Collide,
			SleepState,
			Light,
			VolumetricLight,
			TargetCoverage,
			Gravity,
			Offset,
			RotationVelocityVar,
			HueVar,
			RotationEnabled,
			MotionInheritance,
			LifeVar,
			Streaks,
			RotationReference,
			Angle,
			AngleVar,
			Thickness,
			ParticlesPerFrame,
			CameraBias,
			Emissivity,
			ShadowAlphaMultiplier,
			UseEmissivityChannel,
			UseAlphaAnisotropy,
			AmbientFactor,
			RadiusVar,
			RotationVelocityCollisionMultiplier,
			CollisionCountToKill,
			DistanceScalingFactor,
			MotionInterpolation,
			CameraSoftRadius,
			NumMembers
		}

		private static readonly MyStringId DEFAULT_ATLAS = MyStringId.GetOrCompute("Atlas_D_01");

		private string m_name;

		private MyParticleEffectData m_owner;

		private static readonly string[] m_myRotationReferenceStrings = new string[3] { "Camera", "Local", "Local and camera" };

<<<<<<< HEAD
		private static readonly List<string> m_rotationReferenceStrings = m_myRotationReferenceStrings.ToList();
=======
		private static readonly List<string> m_rotationReferenceStrings = Enumerable.ToList<string>((IEnumerable<string>)m_myRotationReferenceStrings);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private readonly IMyConstProperty[] m_properties = new IMyConstProperty[55];

		private readonly MyAnimatedPropertyVector4 m_vec4Tmp = new MyAnimatedPropertyVector4();

		private readonly MyAnimatedPropertyFloat m_floatTmp = new MyAnimatedPropertyFloat();

		/// <summary>
		/// Public members to easy access
		/// </summary>
		public MyConstPropertyVector3 ArraySize
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[0];
			}
			private set
			{
				m_properties[0] = value;
			}
		}

		public MyConstPropertyInt ArrayOffset
		{
			get
			{
				return (MyConstPropertyInt)m_properties[1];
			}
			private set
			{
				m_properties[1] = value;
			}
		}

		public MyConstPropertyInt ArrayModulo
		{
			get
			{
				return (MyConstPropertyInt)m_properties[2];
			}
			private set
			{
				m_properties[2] = value;
			}
		}

		public MyAnimatedProperty2DVector4 Color
		{
			get
			{
				return (MyAnimatedProperty2DVector4)m_properties[3];
			}
			private set
			{
				m_properties[3] = value;
			}
		}

		public MyAnimatedProperty2DFloat ColorIntensity
		{
			get
			{
				return (MyAnimatedProperty2DFloat)m_properties[4];
			}
			private set
			{
				m_properties[4] = value;
			}
		}

		public MyConstPropertyFloat HueVar
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[33];
			}
			private set
			{
				m_properties[33] = value;
			}
		}

		public MyConstPropertyFloat Bounciness
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[5];
			}
			private set
			{
				m_properties[5] = value;
			}
		}

		public MyConstPropertyFloat RotationVelocityCollisionMultiplier
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[50];
			}
			private set
			{
				m_properties[50] = value;
			}
		}

		public MyConstPropertyFloat DistanceScalingFactor
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[52];
			}
			private set
			{
				m_properties[52] = value;
			}
		}

		public MyConstPropertyInt CollisionCountToKill
		{
			get
			{
				return (MyConstPropertyInt)m_properties[51];
			}
			private set
			{
				m_properties[51] = value;
			}
		}

		public MyAnimatedPropertyVector3 EmitterSize
		{
			get
			{
				return (MyAnimatedPropertyVector3)m_properties[6];
			}
			private set
			{
				m_properties[6] = value;
			}
		}

		public MyAnimatedPropertyFloat EmitterSizeMin
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

		public MyConstPropertyVector3 Offset
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[31];
			}
			private set
			{
				m_properties[31] = value;
			}
		}

		public MyConstPropertyVector3 Direction
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[8];
			}
			private set
			{
				m_properties[8] = value;
			}
		}

		public MyAnimatedPropertyFloat Velocity
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[9];
			}
			private set
			{
				m_properties[9] = value;
			}
		}

		public MyAnimatedPropertyFloat VelocityVar
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[10];
			}
			private set
			{
				m_properties[10] = value;
			}
		}

		public MyAnimatedPropertyFloat DirectionInnerCone
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[11];
			}
			private set
			{
				m_properties[11] = value;
			}
		}

		public MyAnimatedPropertyFloat DirectionConeVar
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[12];
			}
			private set
			{
				m_properties[12] = value;
			}
		}

		public MyConstPropertyVector3 Acceleration
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[13];
			}
			private set
			{
				m_properties[13] = value;
			}
		}

		public MyAnimatedProperty2DFloat AccelerationFactor
		{
			get
			{
				return (MyAnimatedProperty2DFloat)m_properties[14];
			}
			private set
			{
				m_properties[14] = value;
			}
		}

		public MyConstPropertyFloat RotationVelocity
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[15];
			}
			private set
			{
				m_properties[15] = value;
			}
		}

		public MyConstPropertyFloat RotationVelocityVar
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[32];
			}
			private set
			{
				m_properties[32] = value;
			}
		}

		public MyAnimatedProperty2DFloat Radius
		{
			get
			{
				return (MyAnimatedProperty2DFloat)m_properties[16];
			}
			private set
			{
				m_properties[16] = value;
			}
		}

		public MyConstPropertyFloat RadiusVar
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[49];
			}
			private set
			{
				m_properties[49] = value;
			}
		}

		public MyConstPropertyFloat Life
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[17];
			}
			private set
			{
				m_properties[17] = value;
			}
		}

		public MyConstPropertyFloat LifeVar
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[36];
			}
			private set
			{
				m_properties[36] = value;
			}
		}

		public MyConstPropertyFloat SoftParticleDistanceScale
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[18];
			}
			private set
			{
				m_properties[18] = value;
			}
		}

		public MyConstPropertyFloat StreakMultiplier
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[19];
			}
			private set
			{
				m_properties[19] = value;
			}
		}

		public MyConstPropertyFloat AnimationFrameTime
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[20];
			}
			private set
			{
				m_properties[20] = value;
			}
		}

		public MyConstPropertyBool Enabled
		{
			get
			{
				return (MyConstPropertyBool)m_properties[21];
			}
			private set
			{
				m_properties[21] = value;
			}
		}

		public MyAnimatedPropertyFloat ParticlesPerSecond
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[22];
			}
			private set
			{
				m_properties[22] = value;
			}
		}

		public MyAnimatedPropertyFloat ParticlesPerFrame
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[42];
			}
			private set
			{
				m_properties[42] = value;
			}
		}

		public MyConstPropertyTransparentMaterial Material
		{
			get
			{
				return (MyConstPropertyTransparentMaterial)m_properties[23];
			}
			private set
			{
				m_properties[23] = value;
			}
		}

		public MyConstPropertyBool Streaks
		{
			get
			{
				return (MyConstPropertyBool)m_properties[37];
			}
			private set
			{
				m_properties[37] = value;
			}
		}

		public MyConstPropertyBool RotationEnabled
		{
			get
			{
				return (MyConstPropertyBool)m_properties[34];
			}
			private set
			{
				m_properties[34] = value;
			}
		}

		public MyConstPropertyBool Collide
		{
			get
			{
				return (MyConstPropertyBool)m_properties[25];
			}
			private set
			{
				m_properties[25] = value;
			}
		}

		public MyConstPropertyBool UseAlphaAnisotropy
		{
			get
			{
				return (MyConstPropertyBool)m_properties[47];
			}
			private set
			{
				m_properties[47] = value;
			}
		}

		public MyConstPropertyBool SleepState
		{
			get
			{
				return (MyConstPropertyBool)m_properties[26];
			}
			private set
			{
				m_properties[26] = value;
			}
		}

		public MyConstPropertyBool Light
		{
			get
			{
				return (MyConstPropertyBool)m_properties[27];
			}
			private set
			{
				m_properties[27] = value;
			}
		}

		public MyConstPropertyBool VolumetricLight
		{
			get
			{
				return (MyConstPropertyBool)m_properties[28];
			}
			private set
			{
				m_properties[28] = value;
			}
		}

		public MyConstPropertyFloat OITWeightFactor
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[24];
			}
			private set
			{
				m_properties[24] = value;
			}
		}

		public MyConstPropertyFloat TargetCoverage
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[29];
			}
			private set
			{
				m_properties[29] = value;
			}
		}

		public MyConstPropertyFloat Gravity
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[30];
			}
			private set
			{
				m_properties[30] = value;
			}
		}

		public MyConstPropertyFloat MotionInheritance
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[35];
			}
			private set
			{
				m_properties[35] = value;
			}
		}

		public MyConstPropertyBool MotionInterpolation
		{
			get
			{
				return (MyConstPropertyBool)m_properties[53];
			}
			private set
			{
				m_properties[53] = value;
			}
		}

		public MyRotationReference RotationReference
		{
			get
			{
				return (MyRotationReference)(int)(MyConstPropertyInt)m_properties[38];
			}
			set
			{
				m_properties[38].SetValue((int)value);
			}
		}

		public MyConstPropertyVector3 Angle
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[39];
			}
			private set
			{
				m_properties[39] = value;
			}
		}

		public MyConstPropertyVector3 AngleVar
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[40];
			}
			private set
			{
				m_properties[40] = value;
			}
		}

		public MyAnimatedProperty2DFloat Thickness
		{
			get
			{
				return (MyAnimatedProperty2DFloat)m_properties[41];
			}
			private set
			{
				m_properties[41] = value;
			}
		}

		public MyConstPropertyFloat CameraBias
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[43];
			}
			private set
			{
				m_properties[43] = value;
			}
		}

		public MyAnimatedProperty2DFloat Emissivity
		{
			get
			{
				return (MyAnimatedProperty2DFloat)m_properties[44];
			}
			private set
			{
				m_properties[44] = value;
			}
		}

		public MyConstPropertyFloat ShadowAlphaMultiplier
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[45];
			}
			private set
			{
				m_properties[45] = value;
			}
		}

		public MyConstPropertyFloat CameraSoftRadius
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[54];
			}
			private set
			{
				m_properties[54] = value;
			}
		}

		public MyConstPropertyFloat AmbientFactor
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[48];
			}
			private set
			{
				m_properties[48] = value;
			}
		}

		public bool Show { get; set; } = true;


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
			m_name = "ParticleGeneration GPU";
			AddProperty(MyGPUGenerationPropertiesEnum.ArraySize, new MyConstPropertyVector3("Array size", "Count of cells in grid in particle atlas texture."));
			AddProperty(MyGPUGenerationPropertiesEnum.ArrayOffset, new MyConstPropertyInt("Array offset", "The first cell index in particle atlas texture."));
			AddProperty(MyGPUGenerationPropertiesEnum.ArrayModulo, new MyConstPropertyInt("Array modulo", "The length of cells row if cells are used for animated texture."));
			AddProperty(MyGPUGenerationPropertiesEnum.Color, new MyAnimatedProperty2DVector4("Color", "Color of particle. Transparency is set with alpha component of the color."));
			AddProperty(MyGPUGenerationPropertiesEnum.ColorIntensity, new MyAnimatedProperty2DFloat("Color intensity", "HDR intensity multiplier."));
			AddProperty(MyGPUGenerationPropertiesEnum.HueVar, new MyConstPropertyFloat("Hue var", "Random hue variation for particle color."));
			AddProperty(MyGPUGenerationPropertiesEnum.Bounciness, new MyConstPropertyFloat("Bounciness", "Defines how much velocity is left in particle after it bounced. 1 = same velocity, 0 = zero velocity."));
			AddProperty(MyGPUGenerationPropertiesEnum.RotationVelocityCollisionMultiplier, new MyConstPropertyFloat("Rotation velocity collision multiplier", "Defines how much angular velocity is left in particle after it bounced. 1 = same velocity, 0 = zero velocity."));
			AddProperty(MyGPUGenerationPropertiesEnum.DistanceScalingFactor, new MyConstPropertyFloat("Distance scaling factor", "Factor changing relation of distance to particle size (= 0 same as 3d geometry, > 0 makes particles scale down slower)."));
			AddProperty(MyGPUGenerationPropertiesEnum.CollisionCountToKill, new MyConstPropertyInt("Collision count to kill particle", "Particle is killed when it is bounced for defined times."));
			AddProperty(MyGPUGenerationPropertiesEnum.EmitterSize, new MyAnimatedPropertyVector3("Emitter size", "Emitter size in local x,y,z coordinates. Emittor volume is ellipsoid."));
			AddProperty(MyGPUGenerationPropertiesEnum.EmitterSizeMin, new MyAnimatedPropertyFloat("Emitter inner size", "Size multiplier which defines inner emitter volume where no particles are created."));
			AddProperty(MyGPUGenerationPropertiesEnum.Offset, new MyConstPropertyVector3("Offset", "Offset from emitter center."));
			AddProperty(MyGPUGenerationPropertiesEnum.Direction, new MyConstPropertyVector3("Direction", "Local starting direction of the particle."));
			AddProperty(MyGPUGenerationPropertiesEnum.Velocity, new MyAnimatedPropertyFloat("Velocity", "Initial velocity of the particle."));
			AddProperty(MyGPUGenerationPropertiesEnum.VelocityVar, new MyAnimatedPropertyFloat("Velocity var", "Variation of initial velocity of the particle."));
			AddProperty(MyGPUGenerationPropertiesEnum.DirectionInnerCone, new MyAnimatedPropertyFloat("Direction inner cone", "Multiplier of direction cone defining inner cone where no particles are created."));
			AddProperty(MyGPUGenerationPropertiesEnum.DirectionConeVar, new MyAnimatedPropertyFloat("Direction cone", "Size of emittor cone defined around local z axis in degrees."));
			AddProperty(MyGPUGenerationPropertiesEnum.Acceleration, new MyConstPropertyVector3("Acceleration", "Acceleration direction of created particle in local x,y,z coordinates."));
			AddProperty(MyGPUGenerationPropertiesEnum.AccelerationFactor, new MyAnimatedProperty2DFloat("Acceleration factor [m/s^2]", "Acceleration multipler for created particles."));
			AddProperty(MyGPUGenerationPropertiesEnum.RotationVelocity, new MyConstPropertyFloat("Rotation velocity", "Starting angular velocity of particle desgrees per second."));
			AddProperty(MyGPUGenerationPropertiesEnum.RotationVelocityVar, new MyConstPropertyFloat("Rotation velocity var", "Variation of rotation velocity in degrees per second."));
			AddProperty(MyGPUGenerationPropertiesEnum.RotationEnabled, new MyConstPropertyBool("Rotation enabled", "Enables angular velocity of particles."));
			AddProperty(MyGPUGenerationPropertiesEnum.Radius, new MyAnimatedProperty2DFloat("Radius", "Particle radius in meters."));
			AddProperty(MyGPUGenerationPropertiesEnum.RadiusVar, new MyConstPropertyFloat("Radius var", "Variation of particle radius is meters."));
			AddProperty(MyGPUGenerationPropertiesEnum.Life, new MyConstPropertyFloat("Life", "Life of particle in seconds."));
			AddProperty(MyGPUGenerationPropertiesEnum.LifeVar, new MyConstPropertyFloat("Life var", "Variation of particle life in seconds."));
			AddProperty(MyGPUGenerationPropertiesEnum.SoftParticleDistanceScale, new MyConstPropertyFloat("Soft particle distance scale", "Distance from hard surface where particle starts to disappear."));
			AddProperty(MyGPUGenerationPropertiesEnum.StreakMultiplier, new MyConstPropertyFloat("Streak multiplier", "Count of particle ghosts in direction of velocity."));
			AddProperty(MyGPUGenerationPropertiesEnum.AnimationFrameTime, new MyConstPropertyFloat("Animation frame time", "Time for one texture cell if texture is animated."));
			AddProperty(MyGPUGenerationPropertiesEnum.Enabled, new MyConstPropertyBool("Enabled", "Generation simulation status."));
			AddProperty(MyGPUGenerationPropertiesEnum.ParticlesPerSecond, new MyAnimatedPropertyFloat("Particles per second", "Defines how many new particles are created per second."));
			AddProperty(MyGPUGenerationPropertiesEnum.ParticlesPerFrame, new MyAnimatedPropertyFloat("Particles per frame", "Defines how many particles per frame is created."));
			AddProperty(MyGPUGenerationPropertiesEnum.Material, new MyConstPropertyTransparentMaterial("Material", "Name of transparent material defined in game definitions used for particle atlas texture."));
			AddProperty(MyGPUGenerationPropertiesEnum.OITWeightFactor, new MyConstPropertyFloat("OIT weight factor", "Transparency weighted function factor: to help accommodate for very bright particle systems shining from behind other particle systems, default: 1, lesser value will make the particle system shine through less."));
			AddProperty(MyGPUGenerationPropertiesEnum.TargetCoverage, new MyConstPropertyFloat("Target coverage", ""));
			AddProperty(MyGPUGenerationPropertiesEnum.Streaks, new MyConstPropertyBool("Streaks", "Create particle ghosts in velocity direction to emulate streaks."));
			AddProperty(MyGPUGenerationPropertiesEnum.Collide, new MyConstPropertyBool("Collide", "Collide when hits the surface and bounce."));
			AddProperty(MyGPUGenerationPropertiesEnum.UseEmissivityChannel, new MyConstPropertyBool("Use Emissivity Channel", ""));
			AddProperty(MyGPUGenerationPropertiesEnum.UseAlphaAnisotropy, new MyConstPropertyBool("Use Alpha Anisotropy", "Particle transparecy is dependent on angle to camera."));
			AddProperty(MyGPUGenerationPropertiesEnum.SleepState, new MyConstPropertyBool("SleepState", "Particle is not killed after it reached maximum bounces."));
			AddProperty(MyGPUGenerationPropertiesEnum.Light, new MyConstPropertyBool("Light", "Particle is being lit by light sources."));
			AddProperty(MyGPUGenerationPropertiesEnum.VolumetricLight, new MyConstPropertyBool("VolumetricLight", "Particle is casting and receiving shadows"));
			AddProperty(MyGPUGenerationPropertiesEnum.Gravity, new MyConstPropertyFloat("Gravity", "Multiplier to define how much is particle affected by gravity."));
			AddProperty(MyGPUGenerationPropertiesEnum.MotionInheritance, new MyConstPropertyFloat("Motion inheritance", "Multiplier to define how much motion particle inherits when whole effect is moved."));
			AddProperty(MyGPUGenerationPropertiesEnum.RotationReference, new MyConstPropertyEnum("Rotation reference", "Relative source axis to which particles rotate.", typeof(MyRotationReference), m_rotationReferenceStrings));
			AddProperty(MyGPUGenerationPropertiesEnum.Angle, new MyConstPropertyVector3("Angle", "Initial angle of created particle in degrees."));
			AddProperty(MyGPUGenerationPropertiesEnum.AngleVar, new MyConstPropertyVector3("Angle var", "Initial angle variation of created particle in degrees."));
			AddProperty(MyGPUGenerationPropertiesEnum.Thickness, new MyAnimatedProperty2DFloat("Thickness", "Width multiplier of particle size."));
			AddProperty(MyGPUGenerationPropertiesEnum.CameraBias, new MyConstPropertyFloat("Camera bias", "Meters by which to move the emitter towards the camera."));
			AddProperty(MyGPUGenerationPropertiesEnum.Emissivity, new MyAnimatedProperty2DFloat("Emissivity", "Intenstity of the particle in the darkness."));
			AddProperty(MyGPUGenerationPropertiesEnum.ShadowAlphaMultiplier, new MyConstPropertyFloat("Shadow alpha multiplier", "Accentuate alpha in shadow (0 - particle gets invisible in shadow, 1 - alpha is the same in shadow as outside, > 1 - make particle more visible in shadow)."));
			AddProperty(MyGPUGenerationPropertiesEnum.AmbientFactor, new MyConstPropertyFloat("Ambient light factor", "Strength of Diffuse Ambient (from environment map);  0 - no ambient, 1 - standard, >1 - stronger ambient."));
			AddProperty(MyGPUGenerationPropertiesEnum.MotionInterpolation, new MyConstPropertyBool("Motion interpolation", ""));
			AddProperty(MyGPUGenerationPropertiesEnum.CameraSoftRadius, new MyConstPropertyFloat("Camera Soft Radius", "Safe radius around camera, where the particles will have 0 opacity."));
			InitDefault();
		}

		public void InitDefault()
		{
			ArraySize.SetDefaultValue(new Vector3(16f, 16f, 0f));
			ArrayModulo.SetDefaultValue(1);
			ArrayOffset.SetDefaultValue(137);
			MyAnimatedPropertyVector4 myAnimatedPropertyVector = new MyAnimatedPropertyVector4();
			myAnimatedPropertyVector.AddKey(0f, Vector4.One);
			myAnimatedPropertyVector.AddKey(0.33f, Vector4.One);
			myAnimatedPropertyVector.AddKey(0.66f, Vector4.One);
			myAnimatedPropertyVector.AddKey(1f, Vector4.One);
			Color.AddKey(0f, myAnimatedPropertyVector);
			Color.SetDefaultValue();
			MyAnimatedPropertyFloat myAnimatedPropertyFloat = new MyAnimatedPropertyFloat();
			myAnimatedPropertyFloat.AddKey(0f, 1f);
			myAnimatedPropertyFloat.AddKey(0.33f, 1f);
			myAnimatedPropertyFloat.AddKey(0.66f, 1f);
			myAnimatedPropertyFloat.AddKey(1f, 1f);
			ColorIntensity.AddKey(0f, myAnimatedPropertyFloat);
			ColorIntensity.SetDefaultValue();
			MyAnimatedPropertyFloat myAnimatedPropertyFloat2 = new MyAnimatedPropertyFloat();
			myAnimatedPropertyFloat2.AddKey(0f, 0f);
			myAnimatedPropertyFloat2.AddKey(0.33f, 0f);
			myAnimatedPropertyFloat2.AddKey(0.66f, 0f);
			myAnimatedPropertyFloat2.AddKey(1f, 0f);
			AccelerationFactor.AddKey(0f, myAnimatedPropertyFloat2);
			AccelerationFactor.SetDefaultValue();
			Offset.SetDefaultValue(new Vector3(0f, 0f, 0f));
			Direction.SetDefaultValue(new Vector3(0f, 0f, -1f));
			MyAnimatedPropertyFloat myAnimatedPropertyFloat3 = new MyAnimatedPropertyFloat();
			myAnimatedPropertyFloat3.AddKey(0f, 0.1f);
			myAnimatedPropertyFloat3.AddKey(0.33f, 0.1f);
			myAnimatedPropertyFloat3.AddKey(0.66f, 0.1f);
			myAnimatedPropertyFloat3.AddKey(1f, 0.1f);
			Radius.AddKey(0f, myAnimatedPropertyFloat3);
			Radius.SetDefaultValue();
			MyAnimatedPropertyFloat myAnimatedPropertyFloat4 = new MyAnimatedPropertyFloat();
			myAnimatedPropertyFloat4.AddKey(0f, 1f);
			myAnimatedPropertyFloat4.AddKey(0.33f, 1f);
			myAnimatedPropertyFloat4.AddKey(0.66f, 1f);
			myAnimatedPropertyFloat4.AddKey(1f, 1f);
			Thickness.AddKey(0f, myAnimatedPropertyFloat4);
			Thickness.SetDefaultValue();
			MyAnimatedPropertyFloat myAnimatedPropertyFloat5 = new MyAnimatedPropertyFloat();
			myAnimatedPropertyFloat5.AddKey(0f, 0f);
			myAnimatedPropertyFloat5.AddKey(0.33f, 0f);
			myAnimatedPropertyFloat5.AddKey(0.66f, 0f);
			myAnimatedPropertyFloat5.AddKey(1f, 0f);
			Emissivity.AddKey(0f, myAnimatedPropertyFloat5);
			Emissivity.SetDefaultValue();
			Life.SetDefaultValue(1f);
			LifeVar.SetDefaultValue(0f);
			StreakMultiplier.SetDefaultValue(4f);
			AnimationFrameTime.SetDefaultValue(1f);
			Enabled.SetDefaultValue(val: true);
			EmitterSize.AddKey(0f, new Vector3(0f, 0f, 0f));
			EmitterSize.SetDefaultValue();
			EmitterSizeMin.AddKey(0f, 0f);
			EmitterSizeMin.SetDefaultValue();
			DirectionInnerCone.AddKey(0f, 0f);
			DirectionInnerCone.SetDefaultValue();
			DirectionConeVar.AddKey(0f, 0f);
			DirectionConeVar.SetDefaultValue();
			Velocity.AddKey(0f, 1f);
			Velocity.SetDefaultValue();
			VelocityVar.AddKey(0f, 0f);
			VelocityVar.SetDefaultValue();
			ParticlesPerSecond.AddKey(0f, 1000f);
			ParticlesPerSecond.SetDefaultValue();
			Material.SetDefaultValue(MyTransparentMaterials.GetMaterial(DEFAULT_ATLAS));
			SoftParticleDistanceScale.SetDefaultValue(1f);
			Bounciness.SetDefaultValue(0.5f);
			RotationVelocityCollisionMultiplier.SetDefaultValue(1f);
			DistanceScalingFactor.SetDefaultValue(0f);
			CollisionCountToKill.SetDefaultValue(0);
			HueVar.SetDefaultValue(0f);
			RotationEnabled.SetDefaultValue(val: true);
			MotionInheritance.SetDefaultValue(0f);
			MotionInterpolation.SetDefaultValue(val: true);
			OITWeightFactor.SetDefaultValue(1f);
			TargetCoverage.SetDefaultValue(1f);
			CameraBias.SetDefaultValue(0f);
			ShadowAlphaMultiplier.SetDefaultValue(5f);
			CameraSoftRadius.SetDefaultValue(0f);
			AmbientFactor.SetDefaultValue(1f);
		}

		private T AddProperty<T>(MyGPUGenerationPropertiesEnum e, T property) where T : IMyConstProperty
		{
			m_properties[(int)e] = property;
			return property;
		}

		public IEnumerable<IMyConstProperty> GetProperties()
		{
			return m_properties;
		}

		public bool FillDataComplete(ref MyGPUEmitterData emitterData)
		{
			emitterData.Data.HueVar = HueVar;
			if (emitterData.Data.HueVar > 1f)
			{
				emitterData.Data.HueVar = 1f;
			}
			else if (emitterData.Data.HueVar < 0f)
			{
				emitterData.Data.HueVar = 0f;
			}
			emitterData.DistanceMaxSqr = m_owner.DistanceMax * m_owner.DistanceMax;
			emitterData.Data.MotionInheritance = MotionInheritance;
			emitterData.Data.Bounciness = Bounciness;
			emitterData.Data.RotationVelocityCollisionMultiplier = RotationVelocityCollisionMultiplier;
			emitterData.Data.DistanceScalingFactor = DistanceScalingFactor;
			emitterData.Data.CollisionCountToKill = (uint)(int)CollisionCountToKill;
			emitterData.Data.ParticleLifeSpan = Life;
			emitterData.Data.ParticleLifeSpanVar = LifeVar;
			emitterData.Data.Direction = Direction;
			emitterData.Data.RotationVelocity = RotationVelocity;
			emitterData.Data.RotationVelocityVar = RotationVelocityVar;
			emitterData.Data.AccelerationVector = Acceleration;
			emitterData.Data.RadiusVar = RadiusVar;
			emitterData.Data.StreakMultiplier = StreakMultiplier;
			emitterData.Data.SoftParticleDistanceScale = SoftParticleDistanceScale;
			emitterData.Data.AnimationFrameTime = AnimationFrameTime;
			emitterData.Data.OITWeightFactor = OITWeightFactor;
			emitterData.Data.ShadowAlphaMultiplier = ShadowAlphaMultiplier;
			emitterData.Data.CameraSoftRadius = CameraSoftRadius;
			emitterData.Data.AmbientFactor = AmbientFactor;
			emitterData.AtlasTexture = Material.GetValue().Texture;
			emitterData.AtlasDimension = new Vector2I((int)ArraySize.GetValue().X, (int)ArraySize.GetValue().Y);
			emitterData.AtlasFrameOffset = ArrayOffset;
			emitterData.AtlasFrameModulo = ArrayModulo;
			emitterData.Data.TextureIndex2 = (uint)emitterData.AtlasFrameModulo;
			emitterData.Data.Angle = MathHelper.ToRadians(Angle);
			emitterData.Data.AngleVar = MathHelper.ToRadians(AngleVar);
			emitterData.GravityFactor = Gravity;
			GPUEmitterFlags gPUEmitterFlags = (GPUEmitterFlags)0u;
<<<<<<< HEAD
			switch (RotationReference)
			{
			case MyRotationReference.Local:
				gPUEmitterFlags |= GPUEmitterFlags.LocalRotation;
				break;
			case MyRotationReference.LocalAndCamera:
				gPUEmitterFlags |= GPUEmitterFlags.LocalAndCameraRotation;
				break;
			default:
				gPUEmitterFlags |= (Streaks ? GPUEmitterFlags.Streaks : ((GPUEmitterFlags)0u));
				break;
			}
			gPUEmitterFlags |= (Collide ? GPUEmitterFlags.Collide : ((GPUEmitterFlags)0u));
=======
			gPUEmitterFlags = (RotationReference switch
			{
				MyRotationReference.Local => gPUEmitterFlags | GPUEmitterFlags.LocalRotation, 
				MyRotationReference.LocalAndCamera => gPUEmitterFlags | GPUEmitterFlags.LocalAndCameraRotation, 
				_ => gPUEmitterFlags | (Streaks ? GPUEmitterFlags.Streaks : ((GPUEmitterFlags)0u)), 
			}) | (Collide ? GPUEmitterFlags.Collide : ((GPUEmitterFlags)0u));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			gPUEmitterFlags |= (UseAlphaAnisotropy ? GPUEmitterFlags.UseAlphaAnisotropy : ((GPUEmitterFlags)0u));
			gPUEmitterFlags |= (SleepState ? GPUEmitterFlags.SleepState : ((GPUEmitterFlags)0u));
			gPUEmitterFlags |= (Light ? GPUEmitterFlags.Light : ((GPUEmitterFlags)0u));
			gPUEmitterFlags |= (VolumetricLight ? GPUEmitterFlags.VolumetricLight : ((GPUEmitterFlags)0u));
			gPUEmitterFlags |= (RotationEnabled ? GPUEmitterFlags.RandomRotationEnabled : ((GPUEmitterFlags)0u));
			emitterData.Data.Flags = gPUEmitterFlags;
			if (Velocity.GetKeysCount() <= 1 && VelocityVar.GetKeysCount() <= 1 && DirectionInnerCone.GetKeysCount() <= 1 && DirectionConeVar.GetKeysCount() <= 1 && EmitterSize.GetKeysCount() <= 1 && EmitterSizeMin.GetKeysCount() <= 1 && Color.GetKeysCount() <= 1 && ColorIntensity.GetKeysCount() <= 1 && AccelerationFactor.GetKeysCount() <= 1 && Radius.GetKeysCount() <= 1 && Emissivity.GetKeysCount() <= 1)
			{
				return Thickness.GetKeysCount() > 1;
			}
			return true;
		}

		private void CollectKeys(float time, MyAnimatedProperty2DFloat prop, out MyAnimatedPropertyFloat keys)
		{
			if (prop.GetKeysCount() == 1)
			{
				prop.GetKey(0, out var _, out keys);
				return;
			}
			prop.GetInterpolatedKeys(time, 1f, m_floatTmp);
			keys = m_floatTmp;
		}

		private void CollectKeys(float time, MyAnimatedProperty2DVector4 prop, out MyAnimatedPropertyVector4 keys)
		{
			if (prop.GetKeysCount() == 1)
			{
				prop.GetKey(0, out var _, out keys);
				return;
			}
			prop.GetInterpolatedKeys(time, 1f, m_vec4Tmp);
			keys = m_vec4Tmp;
		}

		public void FillData(float elapsedTime, ref MyGPUEmitterData emitterData)
		{
			emitterData.CameraBias = CameraBias;
			Velocity.GetInterpolatedValue(elapsedTime, out emitterData.Data.Velocity);
			VelocityVar.GetInterpolatedValue(elapsedTime, out emitterData.Data.VelocityVar);
			DirectionInnerCone.GetInterpolatedValue(elapsedTime, out var value);
			emitterData.Data.DirectionInnerCone = value;
			DirectionConeVar.GetInterpolatedValue(elapsedTime, out value);
			emitterData.Data.DirectionConeVar = MathHelper.ToRadians(value);
			EmitterSize.GetInterpolatedValue(elapsedTime, out emitterData.Data.EmitterSize);
			EmitterSizeMin.GetInterpolatedValue(elapsedTime, out emitterData.Data.EmitterSizeMin);
			CollectKeys(elapsedTime, Color, out var keys);
			keys.GetKey(0, out var time, out emitterData.Data.Color0);
			keys.GetKey(1, out emitterData.Data.ColorKey1, out emitterData.Data.Color1);
			keys.GetKey(2, out emitterData.Data.ColorKey2, out emitterData.Data.Color2);
			keys.GetKey(3, out time, out emitterData.Data.Color3);
			CollectKeys(elapsedTime, ColorIntensity, out var keys2);
			keys2.GetKey(0, out time, out emitterData.Data.Intensity0);
			keys2.GetKey(1, out emitterData.Data.IntensityKey1, out emitterData.Data.Intensity1);
			keys2.GetKey(2, out emitterData.Data.IntensityKey2, out emitterData.Data.Intensity2);
			keys2.GetKey(3, out time, out emitterData.Data.Intensity3);
			CollectKeys(elapsedTime, AccelerationFactor, out keys2);
			keys2.GetKey(0, out time, out emitterData.Data.Acceleration0);
			keys2.GetKey(1, out emitterData.Data.AccelerationKey1, out emitterData.Data.Acceleration1);
			keys2.GetKey(2, out emitterData.Data.AccelerationKey2, out emitterData.Data.Acceleration2);
			keys2.GetKey(3, out time, out emitterData.Data.Acceleration3);
			CollectKeys(elapsedTime, Radius, out keys2);
			keys2.GetKey(0, out time, out emitterData.Data.ParticleSize0);
			keys2.GetKey(1, out emitterData.Data.ParticleSizeKeys1, out emitterData.Data.ParticleSize1);
			keys2.GetKey(2, out emitterData.Data.ParticleSizeKeys2, out emitterData.Data.ParticleSize2);
			keys2.GetKey(3, out time, out emitterData.Data.ParticleSize3);
			CollectKeys(elapsedTime, Thickness, out keys2);
			keys2.GetKey(0, out time, out emitterData.Data.ParticleThickness0);
			keys2.GetKey(1, out emitterData.Data.ParticleThicknessKeys1, out emitterData.Data.ParticleThickness1);
			keys2.GetKey(2, out emitterData.Data.ParticleThicknessKeys2, out emitterData.Data.ParticleThickness2);
			keys2.GetKey(3, out time, out emitterData.Data.ParticleThickness3);
			CollectKeys(elapsedTime, Emissivity, out keys2);
			keys2.GetKey(0, out time, out emitterData.Data.Emissivity0);
			keys2.GetKey(1, out emitterData.Data.EmissivityKeys1, out emitterData.Data.Emissivity1);
			keys2.GetKey(2, out emitterData.Data.EmissivityKeys2, out emitterData.Data.Emissivity2);
			keys2.GetKey(3, out time, out emitterData.Data.Emissivity3);
		}

		public MyParticleGPUGenerationData Duplicate(MyParticleEffectData newOwner)
		{
			MyParticleGPUGenerationData myParticleGPUGenerationData = new MyParticleGPUGenerationData();
			myParticleGPUGenerationData.Start(newOwner);
			myParticleGPUGenerationData.Name = Name;
			for (int i = 0; i < m_properties.Length; i++)
			{
				myParticleGPUGenerationData.m_properties[i] = m_properties[i].Duplicate();
			}
			return myParticleGPUGenerationData;
		}

		public MyParticleEffectData GetOwner()
		{
			return m_owner;
		}

		public ParticleGeneration SerializeToObjectBuilder()
		{
			ParticleGeneration particleGeneration = new ParticleGeneration();
			particleGeneration.Name = m_name;
			particleGeneration.GenerationType = "GPU";
			particleGeneration.Properties = new List<GenerationProperty>();
			IMyConstProperty[] properties = m_properties;
			for (int i = 0; i < properties.Length; i++)
			{
				GenerationProperty item = properties[i].SerializeToObjectBuilder();
				particleGeneration.Properties.Add(item);
			}
			return particleGeneration;
		}

		public void DeserializeFromObjectBuilder(ParticleGeneration generation)
		{
			m_name = generation.Name;
			foreach (GenerationProperty property in generation.Properties)
			{
				for (int i = 0; i < m_properties.Length; i++)
				{
					if (m_properties[i].Name.Equals(property.Name))
					{
						m_properties[i].DeserializeFromObjectBuilder(property);
					}
				}
			}
			MyAnimatedPropertyFloat myAnimatedPropertyFloat = new MyAnimatedPropertyFloat();
			Emissivity.GetInterpolatedKeys(0f, 1f, myAnimatedPropertyFloat);
			if (myAnimatedPropertyFloat.GetKeysCount() < 4)
			{
				MyAnimatedPropertyFloat myAnimatedPropertyFloat2 = new MyAnimatedPropertyFloat();
				myAnimatedPropertyFloat2.AddKey(0f, 0f);
				myAnimatedPropertyFloat2.AddKey(0.33f, 0f);
				myAnimatedPropertyFloat2.AddKey(0.66f, 0f);
				myAnimatedPropertyFloat2.AddKey(1f, 0f);
				Emissivity.AddKey(0f, myAnimatedPropertyFloat2);
			}
		}

		public bool IsDirty()
		{
			return m_owner.IsDirty();
		}
	}
}
