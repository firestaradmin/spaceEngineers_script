namespace Sandbox.Game.GameSystems
{
	public interface IMySolarOccludable
	{
		bool IsSolarOccluded { get; }

		long GetEntityId();
	}
}
