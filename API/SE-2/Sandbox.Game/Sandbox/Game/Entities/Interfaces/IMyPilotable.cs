using Sandbox.Game.Entities.Character;
using VRage.ModAPI;

namespace Sandbox.Game.Entities.Interfaces
{
	public interface IMyPilotable
	{
		MyCharacter Pilot { get; }

		bool CanHavePreviousControlledEntity { get; }

		IMyControllableEntity PreviousControlledEntity { get; }

		bool CanHavePreviousCameraEntity { get; }

		IMyEntity GetPreviousCameraEntity { get; }
	}
}
