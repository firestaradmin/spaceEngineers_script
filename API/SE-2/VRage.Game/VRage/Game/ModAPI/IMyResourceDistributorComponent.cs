using System;

namespace VRage.Game.ModAPI
{
	public interface IMyResourceDistributorComponent
	{
		MyResourceStateEnum ResourceState { get; }

		MyMultipleEnabledEnum SourcesEnabled { get; }

		event Action<bool> OnPowerGenerationChanged;

		float MaxAvailableResourceByType(MyDefinitionId resourceTypeId, IMyCubeGrid grid = null);

		float TotalRequiredInputByType(MyDefinitionId resourceTypeId, IMyCubeGrid grid = null);
	}
}
