using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_HeatVentBlockDefinition), null)]
	public class MyHeatVentBlockDefinition : MyFunctionalBlockDefinition
	{
		private class Sandbox_Definitions_MyHeatVentBlockDefinition_003C_003EActor : IActivator, IActivator<MyHeatVentBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyHeatVentBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyHeatVentBlockDefinition CreateInstance()
			{
				return new MyHeatVentBlockDefinition();
			}

			MyHeatVentBlockDefinition IActivator<MyHeatVentBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float PowerDependency;

		public float RequiredPowerInput;

		public string EmissiveMaterialName;

		public string LightDummyName;

		public float ReflectorConeDegrees;

		public MyBounds LightFalloffBounds;

		public MyBounds LightIntensityBounds;

		public MyBounds LightRadiusBounds;

		public MyBounds LightOffsetBounds;

		public ColorDefinitionRGBA ColorMinimalPower;

		public ColorDefinitionRGBA ColorMaximalPower;

		public MyObjectBuilder_HeatVentBlockDefinition.SubpartRotation[] SubpartRotations;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_HeatVentBlockDefinition myObjectBuilder_HeatVentBlockDefinition = (MyObjectBuilder_HeatVentBlockDefinition)builder;
			PowerDependency = myObjectBuilder_HeatVentBlockDefinition.PowerDependency;
			ColorMinimalPower = myObjectBuilder_HeatVentBlockDefinition.ColorMinimalPower;
			ColorMaximalPower = myObjectBuilder_HeatVentBlockDefinition.ColorMaximalPower;
			RequiredPowerInput = myObjectBuilder_HeatVentBlockDefinition.RequiredPowerInput;
			SubpartRotations = myObjectBuilder_HeatVentBlockDefinition.SubpartRotations;
			EmissiveMaterialName = myObjectBuilder_HeatVentBlockDefinition.EmissiveMaterialName;
			LightDummyName = myObjectBuilder_HeatVentBlockDefinition.LightDummyName;
			LightFalloffBounds = myObjectBuilder_HeatVentBlockDefinition.LightFalloffBounds;
			LightIntensityBounds = myObjectBuilder_HeatVentBlockDefinition.LightIntensityBounds;
			LightRadiusBounds = myObjectBuilder_HeatVentBlockDefinition.LightRadiusBounds;
			LightOffsetBounds = myObjectBuilder_HeatVentBlockDefinition.LightOffsetBounds;
		}
	}
}
