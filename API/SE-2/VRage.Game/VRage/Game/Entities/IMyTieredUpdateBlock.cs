namespace VRage.Game.Entities
{
	/// <summary>
	/// Describes tiered update block
	/// </summary>
	public interface IMyTieredUpdateBlock
	{
		/// <summary>
		/// Gets is tiered update supported value, changing value in runtime is not supported
		/// </summary>
		bool IsTieredUpdateSupported { get; }

		/// <summary>
		/// Called when block needs to change tier
		/// </summary>
		void ChangeTier();
	}
}
