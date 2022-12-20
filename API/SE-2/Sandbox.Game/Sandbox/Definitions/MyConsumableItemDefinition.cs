using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Input;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ConsumableItemDefinition), null)]
	public class MyConsumableItemDefinition : MyUsableItemDefinition
	{
		public struct StatValue
		{
			public string Name;

			public float Value;

			public float Time;

			public StatValue(string name, float value, float time)
			{
				Name = name;
				Value = value;
				Time = time;
			}
		}

		private class Sandbox_Definitions_MyConsumableItemDefinition_003C_003EActor : IActivator, IActivator<MyConsumableItemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyConsumableItemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyConsumableItemDefinition CreateInstance()
			{
				return new MyConsumableItemDefinition();
			}

			MyConsumableItemDefinition IActivator<MyConsumableItemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<StatValue> Stats;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ConsumableItemDefinition myObjectBuilder_ConsumableItemDefinition = builder as MyObjectBuilder_ConsumableItemDefinition;
			Stats = new List<StatValue>();
			if (myObjectBuilder_ConsumableItemDefinition.Stats != null)
			{
				MyObjectBuilder_ConsumableItemDefinition.StatValue[] stats = myObjectBuilder_ConsumableItemDefinition.Stats;
				foreach (MyObjectBuilder_ConsumableItemDefinition.StatValue statValue in stats)
				{
					Stats.Add(new StatValue(statValue.Name, statValue.Value, statValue.Time));
				}
			}
		}

		internal override string GetTooltipDisplayName(MyObjectBuilder_PhysicalObject content)
		{
			string empty = string.Empty;
			return string.Concat(str2: (!MyInput.Static.IsJoystickLastUsed) ? MyTexts.GetString(MyCommonTexts.Consumable_InventoryItem_TTIP_Keyboard) : MyTexts.GetString(MyCommonTexts.Consumable_InventoryItem_TTIP_Gamepad), str0: base.GetTooltipDisplayName(content), str1: "\n");
		}
	}
}
