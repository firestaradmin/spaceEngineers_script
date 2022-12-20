using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Serialization;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_LargeTurretBaseDefinition), null)]
	public class MyLargeTurretBaseDefinition : MyWeaponBlockDefinition
	{
		private class Sandbox_Definitions_MyLargeTurretBaseDefinition_003C_003EActor : IActivator, IActivator<MyLargeTurretBaseDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyLargeTurretBaseDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLargeTurretBaseDefinition CreateInstance()
			{
				return new MyLargeTurretBaseDefinition();
			}

			MyLargeTurretBaseDefinition IActivator<MyLargeTurretBaseDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string OverlayTexture;

		public bool AiEnabled;

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

		public int AmmoPullAmount;

<<<<<<< HEAD
		public float InventoryFillFactorMax;

		public float ForwardCameraOffset;

		public float UpCameraOffset;

		public SerializableDictionary<string, string> SubpartPairing;

		public string CameraDummyName;

		public MyTurretTargetingOptions EnabledTargetingOptions;

		public MyTurretTargetingOptions HiddenTargetingOptions;
=======
		public new float InventoryFillFactorMin;

		public float InventoryFillFactorMax;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_LargeTurretBaseDefinition myObjectBuilder_LargeTurretBaseDefinition = builder as MyObjectBuilder_LargeTurretBaseDefinition;
			OverlayTexture = myObjectBuilder_LargeTurretBaseDefinition.OverlayTexture;
			AiEnabled = myObjectBuilder_LargeTurretBaseDefinition.AiEnabled;
			MinElevationDegrees = myObjectBuilder_LargeTurretBaseDefinition.MinElevationDegrees;
			MaxElevationDegrees = myObjectBuilder_LargeTurretBaseDefinition.MaxElevationDegrees;
			MinAzimuthDegrees = myObjectBuilder_LargeTurretBaseDefinition.MinAzimuthDegrees;
			MaxAzimuthDegrees = myObjectBuilder_LargeTurretBaseDefinition.MaxAzimuthDegrees;
			IdleRotation = myObjectBuilder_LargeTurretBaseDefinition.IdleRotation;
			MaxRangeMeters = myObjectBuilder_LargeTurretBaseDefinition.MaxRangeMeters;
			RotationSpeed = myObjectBuilder_LargeTurretBaseDefinition.RotationSpeed;
			ElevationSpeed = myObjectBuilder_LargeTurretBaseDefinition.ElevationSpeed;
			MinFov = myObjectBuilder_LargeTurretBaseDefinition.MinFov;
			MaxFov = myObjectBuilder_LargeTurretBaseDefinition.MaxFov;
			AmmoPullAmount = myObjectBuilder_LargeTurretBaseDefinition.AmmoPullAmountPerTick;
			InventoryFillFactorMin = myObjectBuilder_LargeTurretBaseDefinition.InventoryFillFactorMin;
			InventoryFillFactorMax = myObjectBuilder_LargeTurretBaseDefinition.InventoryFillFactorMax;
<<<<<<< HEAD
			ForwardCameraOffset = myObjectBuilder_LargeTurretBaseDefinition.ForwardCameraOffset;
			UpCameraOffset = myObjectBuilder_LargeTurretBaseDefinition.UpCameraOffset;
			SubpartPairing = myObjectBuilder_LargeTurretBaseDefinition.SubpartPairing;
			CameraDummyName = myObjectBuilder_LargeTurretBaseDefinition.CameraDummyName;
			EnabledTargetingOptions = myObjectBuilder_LargeTurretBaseDefinition.EnabledTargetingOptions;
			HiddenTargetingOptions = myObjectBuilder_LargeTurretBaseDefinition.HiddenTargetingOptions;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
