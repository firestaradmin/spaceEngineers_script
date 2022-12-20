using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes meteor entity (mods interface)
	/// </summary>
	public interface IMyMeteor : VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyDestroyableObject, IMyDecalProxy
	{
	}
}
