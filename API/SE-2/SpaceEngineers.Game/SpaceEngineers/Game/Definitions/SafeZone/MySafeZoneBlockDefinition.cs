using System.Collections.Generic;
using System.Linq;
using ObjectBuilders.Definitions.SafeZone;
using Sandbox.Definitions;
using VRage.Game;
using VRage.Game.Definitions;

namespace SpaceEngineers.Game.Definitions.SafeZone
{
	[MyDefinitionType(typeof(MyObjectBuilder_SafeZoneBlockDefinition), null)]
	public class MySafeZoneBlockDefinition : MyFunctionalBlockDefinition
	{
		public string ResourceSinkGroup;

		public float MaxSafeZoneRadius;

		public float MinSafeZoneRadius;

		public float DefaultSafeZoneRadius;

		public float MaxSafeZonePowerDrainkW;

		public float MinSafeZonePowerDrainkW;

		public uint SafeZoneActivationTimeS;

		public uint SafeZoneUpkeep;

		public uint SafeZoneUpkeepTimeM;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_SafeZoneBlockDefinition myObjectBuilder_SafeZoneBlockDefinition = (MyObjectBuilder_SafeZoneBlockDefinition)builder;
			ResourceSinkGroup = myObjectBuilder_SafeZoneBlockDefinition.ResourceSinkGroup;
			MaxSafeZoneRadius = myObjectBuilder_SafeZoneBlockDefinition.MaxSafeZoneRadius;
			MinSafeZoneRadius = myObjectBuilder_SafeZoneBlockDefinition.MinSafeZoneRadius;
			DefaultSafeZoneRadius = myObjectBuilder_SafeZoneBlockDefinition.DefaultSafeZoneRadius;
			MaxSafeZonePowerDrainkW = myObjectBuilder_SafeZoneBlockDefinition.MaxSafeZonePowerDrainkW;
			MinSafeZonePowerDrainkW = myObjectBuilder_SafeZoneBlockDefinition.MinSafeZonePowerDrainkW;
			SafeZoneActivationTimeS = myObjectBuilder_SafeZoneBlockDefinition.SafeZoneActivationTimeS;
			SafeZoneUpkeep = myObjectBuilder_SafeZoneBlockDefinition.SafeZoneUpkeep;
			SafeZoneUpkeepTimeM = myObjectBuilder_SafeZoneBlockDefinition.SafeZoneUpkeepTimeM;
<<<<<<< HEAD
=======
			ScreenAreas = ((myObjectBuilder_SafeZoneBlockDefinition.ScreenAreas != null) ? Enumerable.ToList<ScreenArea>((IEnumerable<ScreenArea>)myObjectBuilder_SafeZoneBlockDefinition.ScreenAreas) : null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
