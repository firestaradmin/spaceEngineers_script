using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;

namespace Sandbox.Game.Contracts
{
	[MyContractDescriptor(typeof(MyObjectBuilder_ContractCustom))]
	public class MyContractCustom : MyContract
	{
		private MyDefinitionId m_definitionId;

		private MySessionComponentContractSystem m_cachedContractSystem;

		public string ContractName { get; private set; }

		public string ContractDescription { get; private set; }

		protected MySessionComponentContractSystem ContractSystem
		{
			get
			{
				if (m_cachedContractSystem == null)
				{
					m_cachedContractSystem = MySession.Static.GetComponent<MySessionComponentContractSystem>();
				}
				return m_cachedContractSystem;
			}
		}

		public override void Init(MyObjectBuilder_Contract ob)
		{
			base.Init(ob);
			MyObjectBuilder_ContractCustom myObjectBuilder_ContractCustom;
			if ((myObjectBuilder_ContractCustom = ob as MyObjectBuilder_ContractCustom) != null)
			{
				m_definitionId = myObjectBuilder_ContractCustom.DefinitionId;
				ContractName = myObjectBuilder_ContractCustom.ContractName;
				ContractDescription = myObjectBuilder_ContractCustom.ContractDescription;
			}
		}

		public override MyObjectBuilder_Contract GetObjectBuilder()
		{
			MyObjectBuilder_Contract objectBuilder = base.GetObjectBuilder();
			MyObjectBuilder_ContractCustom myObjectBuilder_ContractCustom;
			if ((myObjectBuilder_ContractCustom = objectBuilder as MyObjectBuilder_ContractCustom) != null)
			{
				myObjectBuilder_ContractCustom.DefinitionId = m_definitionId;
				myObjectBuilder_ContractCustom.ContractName = ContractName;
				myObjectBuilder_ContractCustom.ContractDescription = ContractDescription;
			}
			return objectBuilder;
		}

		private MyActivationResults ConvertActivationResult(MyActivationCustomResults result)
		{
			return result switch
			{
				MyActivationCustomResults.Success => MyActivationResults.Success, 
				MyActivationCustomResults.Fail_General => MyActivationResults.Fail, 
				MyActivationCustomResults.Fail_InsufficientFunds => MyActivationResults.Fail_InsufficientFunds, 
				MyActivationCustomResults.Fail_InsufficientInventorySpace => MyActivationResults.Fail_InsufficientInventorySpace, 
				MyActivationCustomResults.Error_General => MyActivationResults.Error, 
				_ => MyActivationResults.Error, 
			};
		}

		protected override MyActivationResults CanActivate_Internal(long playerId)
		{
			MyActivationResults myActivationResults = base.CanActivate_Internal(playerId);
			if (myActivationResults != 0)
			{
				return myActivationResults;
			}
			MySessionComponentContractSystem contractSystem = ContractSystem;
			if (contractSystem.CustomCanActivateContract != null)
			{
				myActivationResults = ConvertActivationResult(contractSystem.CustomCanActivateContract(base.Id, playerId));
				if (myActivationResults != 0)
				{
					return myActivationResults;
				}
			}
			return MyActivationResults.Success;
		}

		protected override void Activate_Internal(MyTimeSpan timeOfActivation)
		{
			base.Activate_Internal(timeOfActivation);
			MySessionComponentContractSystem contractSystem = ContractSystem;
			long identityId = 0L;
			if (base.Owners.Count >= 1)
			{
				identityId = base.Owners[0];
			}
			contractSystem.OnCustomActivateContract(base.Id, identityId);
		}

		protected override void FailFor_Internal(long player, bool abandon = false)
		{
			base.FailFor_Internal(player, abandon);
			ContractSystem.OnCustomFailFor(base.Id, player, abandon);
		}

		protected override void FinishFor_Internal(long player, int rewardeeCount)
		{
			base.FinishFor_Internal(player, rewardeeCount);
			ContractSystem.OnCustomFinishFor(base.Id, player, rewardeeCount);
		}

		protected override bool NeedsUpdate_Internal()
		{
			MySessionComponentContractSystem contractSystem = ContractSystem;
			if (!base.NeedsUpdate_Internal())
			{
				if (contractSystem.CustomNeedsUpdate == null)
				{
					return false;
				}
				return contractSystem.CustomNeedsUpdate(base.Id);
			}
			return true;
		}

		protected override void Finish_Internal()
		{
			base.Finish_Internal();
			ContractSystem.OnCustomFinish(base.Id);
		}

		protected override void Fail_Internal()
		{
			base.Fail_Internal();
			ContractSystem.OnCustomFail(base.Id);
		}

		protected override void CleanUp_Internal()
		{
			base.CleanUp_Internal();
			ContractSystem.OnCustomCleanUp(base.Id);
		}

		public override void TimeRanOut_Internal()
		{
			ContractSystem.OnCustomTimeRanOut(base.Id);
			base.TimeRanOut_Internal();
		}

		public override void Update(MyTimeSpan currentTime)
		{
			base.Update(currentTime);
			ContractSystem.OnCustomUpdate(base.Id, ConvertContractState(base.State), currentTime);
		}

		internal static MyCustomContractStateEnum ConvertContractState(MyContractStateEnum state)
		{
			return state switch
			{
				MyContractStateEnum.Inactive => MyCustomContractStateEnum.Inactive, 
				MyContractStateEnum.Active => MyCustomContractStateEnum.Active, 
				MyContractStateEnum.Finished => MyCustomContractStateEnum.Finished, 
				MyContractStateEnum.Failed => MyCustomContractStateEnum.Failed, 
				MyContractStateEnum.ToBeDisposed => MyCustomContractStateEnum.ToBeDisposed, 
				MyContractStateEnum.Disposed => MyCustomContractStateEnum.Disposed, 
				_ => MyCustomContractStateEnum.Invalid, 
			};
		}

		public override MyDefinitionId? GetDefinitionId()
		{
			return m_definitionId;
		}
	}
}
