namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes results of inserting order or offers into store block
	/// </summary>
	public enum MyStoreInsertResults
	{
		/// <summary>
		/// Inserted successfuly 
		/// </summary>
		Success,
		/// <summary>
		/// 30 orders/offers Per player
		/// </summary>
		Fail_StoreLimitReached,
		/// <summary>
		/// Too cheap
		/// </summary>
		Fail_PricePerUnitIsLessThanMinimum,
		/// <summary>
		/// Other errors
		/// </summary>
		Error
	}
}
