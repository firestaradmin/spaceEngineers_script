using Sandbox.Definitions;
using Sandbox.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.Entities.Interfaces
{
	public interface IMyTurretTerminalControlReceiver : IMyTargetingReceiver, IMyShootOrigin
	{
		MyStringHash TargetingGroup { get; }

		bool EnableIdleRotation { get; set; }

		float MinRange { get; }

		float MaxRange { get; }

		bool TargetLocking { get; set; }

		bool CanControl();

		void RequestControl();

		bool IsTurretTerminalVisible();

		void ForgetTarget();

		void CopyTarget();

		void SetTargetingMode(MyTargetingGroupDefinition definition);
	}
}
