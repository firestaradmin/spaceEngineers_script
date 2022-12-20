using Sandbox.Game.Entities.Cube;
using VRage.Library.Collections;

namespace Sandbox.Game.Gui
{
	public interface ITerminalControlSync
	{
		/// <summary>
		/// (De)serializes block data.
		/// </summary>
		void Serialize(BitStream stream, MyTerminalBlock block);
	}
}
