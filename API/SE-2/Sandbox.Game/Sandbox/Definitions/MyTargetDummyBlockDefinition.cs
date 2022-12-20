using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_TargetDummyBlockDefinition), null)]
<<<<<<< HEAD
	public class MyTargetDummyBlockDefinition : MyFunctionalBlockDefinition
=======
	public class MyTargetDummyBlockDefinition : MyCubeBlockDefinition
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	{
		public struct MyDummySubpartDescription
		{
			public bool IsCritical;

			public float Health;
		}

		private class Sandbox_Definitions_MyTargetDummyBlockDefinition_003C_003EActor : IActivator, IActivator<MyTargetDummyBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyTargetDummyBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTargetDummyBlockDefinition CreateInstance()
			{
				return new MyTargetDummyBlockDefinition();
			}

			MyTargetDummyBlockDefinition IActivator<MyTargetDummyBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Dictionary<string, MyDummySubpartDescription> SubpartDefinitions = new Dictionary<string, MyDummySubpartDescription>();

		public MyInventoryConstraint InventoryConstraint;

		public float InventoryMaxVolume;

		public Vector3 InventorySize;

		public MyDefinitionId ConstructionItem;

		public int ConstructionItemAmount;

		public float MinRegenerationTimeInS;

		public float MaxRegenerationTimeInS;

		public string RegenerationEffectName;

		public string DestructionEffectName;

		public float RegenerationEffectMultiplier;

		public float DestructionEffectMultiplier;

		public MySoundPair RegenerationSound;

		public MySoundPair DestructionSound;

		public float MinFillFactor;

		public float MaxFillFactor;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_TargetDummyBlockDefinition myObjectBuilder_TargetDummyBlockDefinition;
			if ((myObjectBuilder_TargetDummyBlockDefinition = builder as MyObjectBuilder_TargetDummyBlockDefinition) == null)
			{
				return;
			}
			if (myObjectBuilder_TargetDummyBlockDefinition.DummySubpartNames != null && myObjectBuilder_TargetDummyBlockDefinition.DummySubpartCritical != null && myObjectBuilder_TargetDummyBlockDefinition.DummySubpartHealth != null && myObjectBuilder_TargetDummyBlockDefinition.DummySubpartNames.Count == myObjectBuilder_TargetDummyBlockDefinition.DummySubpartCritical.Count && myObjectBuilder_TargetDummyBlockDefinition.DummySubpartCritical.Count == myObjectBuilder_TargetDummyBlockDefinition.DummySubpartHealth.Count)
			{
				SubpartDefinitions.Clear();
				for (int i = 0; i < myObjectBuilder_TargetDummyBlockDefinition.DummySubpartNames.Count; i++)
				{
					SubpartDefinitions.Add(myObjectBuilder_TargetDummyBlockDefinition.DummySubpartNames[i], new MyDummySubpartDescription
					{
						IsCritical = myObjectBuilder_TargetDummyBlockDefinition.DummySubpartCritical[i],
						Health = myObjectBuilder_TargetDummyBlockDefinition.DummySubpartHealth[i]
					});
				}
			}
			else
			{
				MyLog.Default.Error("Unequal TargetDummy subpart informations for: " + Id.SubtypeName);
			}
			InventoryMaxVolume = myObjectBuilder_TargetDummyBlockDefinition.InventoryMaxVolume;
			InventorySize = myObjectBuilder_TargetDummyBlockDefinition.InventorySize;
			ConstructionItem = new MyDefinitionId(typeof(MyObjectBuilder_Component), myObjectBuilder_TargetDummyBlockDefinition.ConstructionItemName);
			ConstructionItemAmount = myObjectBuilder_TargetDummyBlockDefinition.ConstructionItemAmount;
			InventoryConstraint = PrepareConstraint(MySpaceTexts.ToolTipItemFilter_GenericProductionBlockInput, ConstructionItem);
			MinRegenerationTimeInS = myObjectBuilder_TargetDummyBlockDefinition.MinRegenerationTimeInS;
			MaxRegenerationTimeInS = myObjectBuilder_TargetDummyBlockDefinition.MaxRegenerationTimeInS;
			RegenerationEffectName = myObjectBuilder_TargetDummyBlockDefinition.RegenerationEffectName;
			DestructionEffectName = myObjectBuilder_TargetDummyBlockDefinition.DestructionEffectName;
			RegenerationEffectMultiplier = myObjectBuilder_TargetDummyBlockDefinition.RegenerationEffectMultiplier;
			DestructionEffectMultiplier = myObjectBuilder_TargetDummyBlockDefinition.DestructionEffectMultiplier;
			MinFillFactor = myObjectBuilder_TargetDummyBlockDefinition.MinFillFactor;
			MaxFillFactor = myObjectBuilder_TargetDummyBlockDefinition.MaxFillFactor;
			RegenerationSound = new MySoundPair(myObjectBuilder_TargetDummyBlockDefinition.RegenerationSound);
			DestructionSound = new MySoundPair(myObjectBuilder_TargetDummyBlockDefinition.DestructionSound);
		}

		private MyInventoryConstraint PrepareConstraint(MyStringId descriptionId, MyDefinitionId itemId)
		{
			string icon = null;
			MyInventoryConstraint myInventoryConstraint = new MyInventoryConstraint(string.Format(MyTexts.GetString(descriptionId), DisplayNameText), icon);
			myInventoryConstraint.Add(itemId);
			return myInventoryConstraint;
		}
	}
}
