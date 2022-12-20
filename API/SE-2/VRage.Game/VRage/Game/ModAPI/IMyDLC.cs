using VRage.Utils;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Contains information about a particular DLC package.
	/// </summary>
	public interface IMyDLC
	{
		/// <summary>
		/// Gets the Steam AppID of the DLC.
		/// </summary>
		uint AppId { get; }

		/// <summary>
		/// Gets the internal name of the DLC. This is the name used in CubeBlocks.sbc, for example.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets localized name of the DLC.
		/// </summary>
		MyStringId DisplayName { get; }

		/// <summary>
		/// Gets localized description of the DLC.
		/// </summary>
		MyStringId Description { get; }

		/// <summary>
		/// Gets icon of the DLC. Displayed in G-screen, blueprints, etc ...
		/// </summary>
		string Icon { get; }

		/// <summary>
		/// Gets badge of the DLC. Displayed in main menu.
		/// </summary>
		string Badge { get; }
	}
}
