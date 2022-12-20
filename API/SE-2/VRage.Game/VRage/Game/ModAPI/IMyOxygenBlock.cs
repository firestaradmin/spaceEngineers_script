namespace VRage.Game.ModAPI
{
	public interface IMyOxygenBlock
	{
		float PreviousOxygenAmount { get; }

		int OxygenChangeTime { get; }

		IMyOxygenRoom Room { get; }

		float OxygenLevel(float gridSize);
	}
}
