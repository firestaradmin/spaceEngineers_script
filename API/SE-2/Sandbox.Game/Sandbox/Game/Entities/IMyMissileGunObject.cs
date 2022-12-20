using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Weapons;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;

namespace Sandbox.Game.Entities
{
	public interface IMyMissileGunObject : IMyGunObject<MyGunBase>, IMyShootOrigin
	{
		void ShootMissile(MyObjectBuilder_Missile builder);

		void RemoveMissile(long entityId);
	}
}
