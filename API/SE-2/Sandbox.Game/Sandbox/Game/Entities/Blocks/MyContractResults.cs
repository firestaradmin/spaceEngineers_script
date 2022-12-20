namespace Sandbox.Game.Entities.Blocks
{
	public enum MyContractResults
	{
		Success,
		/// <summary>
		/// Something wrong happened
		/// </summary>
		Error_Unknown,
		/// <summary>
		/// Required thing (not managed by Contract system) is missing (session component, character, inventory on entity supposed to have one)
		/// </summary>
		Error_MissingKeyStructure,
		/// <summary>
		/// Data are not consistent
		/// </summary>
		Error_InvalidData,
		/// <summary>
		/// Player has no access to Contract block
		/// </summary>
		Fail_CannotAccess,
		Fail_NotPossible,
		Fail_ActivationConditionsNotMet,
		Fail_ActivationConditionsNotMet_InsufficientFunds,
		Fail_ActivationConditionsNotMet_InsufficientSpace,
		Fail_FinishConditionsNotMet,
		Fail_FinishConditionsNotMet_MissingPackage,
		Fail_FinishConditionsNotMet_IncorrectTargetEntity,
		Fail_ContractNotFound_Activation,
		Fail_ContractNotFound_Abandon,
		Fail_ContractNotFound_Finish,
		Fail_FinishConditionsNotMet_NotEnoughItems,
		Fail_ActivationConditionsNotMet_ContractLimitReachedHard,
		Fail_ActivationConditionsNotMet_TargetOffline,
		Fail_FinishConditionsNotMet_NotEnoughSpace,
		Fail_ActivationConditionsNotMet_YouAreTargetOfThisHunt
	}
}
