namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes information about accepting contract result (mods struct)
	/// </summary>
	public struct MyAddContractResultWrapper
	{
		public bool Success;

		public long ContractId;

		public long ContractConditionId;
	}
}
