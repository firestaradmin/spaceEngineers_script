using VRage;
using VRage.Game;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes item in queue of production block (PB scripting interface)(mods interface)
	/// </summary>
	public struct MyProductionQueueItem
	{
		/// <summary>
		/// Gets or sets amount of items needed to be produced
		/// </summary>
		public MyFixedPoint Amount;

		/// <summary>
		/// Gets or sets blueprint for production
		/// </summary>
		public MyDefinitionBase Blueprint;

		/// <summary>
		/// Gets or sets blueprint for production
		/// </summary>
		public uint ItemId;
	}
}
