using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_SearchlightDefinition), null)]
	public class MySearchlightDefinition : MyFunctionalBlockDefinition
	{
		private class Sandbox_Definitions_MySearchlightDefinition_003C_003EActor : IActivator, IActivator<MySearchlightDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySearchlightDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySearchlightDefinition CreateInstance()
			{
				return new MySearchlightDefinition();
			}

			MySearchlightDefinition IActivator<MySearchlightDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string OverlayTexture;

		public int MinElevationDegrees;

		public int MaxElevationDegrees;

		public int MinAzimuthDegrees;

		public int MaxAzimuthDegrees;

		public bool IdleRotation;

		public float MaxRangeMeters;

		public float RotationSpeed;

		public float ElevationSpeed;

		public float MinFov;

		public float MaxFov;

		public float ForwardCameraOffset;

		public float UpCameraOffset;

		public bool AiEnabled;

		public MyBounds LightRadius;

		public MyBounds LightReflectorRadius;

		public MyBounds LightFalloff;

		public MyBounds LightIntensity;

		public MyBounds LightOffset;

		public MyBounds BlinkIntervalSeconds;

		public MyBounds BlinkLenght;

		public MyBounds BlinkOffset;

		public MyStringHash ResourceSinkGroup;

		public float RequiredPowerInput;

		public string Flare;

		public string PointLightEmissiveMaterial;

		public string SpotLightEmissiveMaterial;

		public float ReflectorConeDegrees;

		public string ReflectorTexture;

		public MyBounds RotationSpeedBounds;

		public string ReflectorConeMaterial;

		public float ReflectorThickness;

		public string LightDummyName;

		public float LightShaftOffset;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_SearchlightDefinition myObjectBuilder_SearchlightDefinition = builder as MyObjectBuilder_SearchlightDefinition;
			OverlayTexture = myObjectBuilder_SearchlightDefinition.OverlayTexture;
			MinElevationDegrees = myObjectBuilder_SearchlightDefinition.MinElevationDegrees;
			MaxElevationDegrees = myObjectBuilder_SearchlightDefinition.MaxElevationDegrees;
			MinAzimuthDegrees = myObjectBuilder_SearchlightDefinition.MinAzimuthDegrees;
			MaxAzimuthDegrees = myObjectBuilder_SearchlightDefinition.MaxAzimuthDegrees;
			IdleRotation = myObjectBuilder_SearchlightDefinition.IdleRotation;
			MaxRangeMeters = myObjectBuilder_SearchlightDefinition.MaxRangeMeters;
			RotationSpeed = myObjectBuilder_SearchlightDefinition.RotationSpeed;
			ElevationSpeed = myObjectBuilder_SearchlightDefinition.ElevationSpeed;
			MinFov = myObjectBuilder_SearchlightDefinition.MinFov;
			MaxFov = myObjectBuilder_SearchlightDefinition.MaxFov;
			ForwardCameraOffset = myObjectBuilder_SearchlightDefinition.ForwardCameraOffset;
			UpCameraOffset = myObjectBuilder_SearchlightDefinition.UpCameraOffset;
			AiEnabled = myObjectBuilder_SearchlightDefinition.AiEnabled;
			BlinkIntervalSeconds = myObjectBuilder_SearchlightDefinition.LightBlinkIntervalSeconds;
			BlinkLenght = myObjectBuilder_SearchlightDefinition.LightBlinkLenght;
			BlinkOffset = myObjectBuilder_SearchlightDefinition.LightBlinkOffset;
			LightRadius = myObjectBuilder_SearchlightDefinition.LightRadius;
			LightReflectorRadius = myObjectBuilder_SearchlightDefinition.LightReflectorRadius;
			LightFalloff = myObjectBuilder_SearchlightDefinition.LightFalloff;
			LightIntensity = myObjectBuilder_SearchlightDefinition.LightIntensity;
			LightOffset = myObjectBuilder_SearchlightDefinition.LightOffset;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_SearchlightDefinition.ResourceSinkGroup);
			RequiredPowerInput = myObjectBuilder_SearchlightDefinition.RequiredPowerInput;
			Flare = myObjectBuilder_SearchlightDefinition.Flare;
			PointLightEmissiveMaterial = myObjectBuilder_SearchlightDefinition.PointLightEmissiveMaterial;
			SpotLightEmissiveMaterial = myObjectBuilder_SearchlightDefinition.SpotLightEmissiveMaterial;
			ReflectorTexture = myObjectBuilder_SearchlightDefinition.ReflectorTexture;
			RotationSpeedBounds = myObjectBuilder_SearchlightDefinition.RotationSpeedBounds;
			ReflectorConeMaterial = myObjectBuilder_SearchlightDefinition.ReflectorConeMaterial;
			ReflectorThickness = myObjectBuilder_SearchlightDefinition.ReflectorThickness;
			LightDummyName = myObjectBuilder_SearchlightDefinition.LightDummyName;
			LightShaftOffset = myObjectBuilder_SearchlightDefinition.LightShaftOffset;
		}
	}
}
