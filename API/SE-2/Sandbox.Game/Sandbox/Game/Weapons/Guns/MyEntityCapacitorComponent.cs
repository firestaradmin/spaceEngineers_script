using System;
using System.Text;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Weapons.Guns
{
	[MyComponentType(typeof(MyEntityCapacitorComponent))]
	[MyComponentBuilder(typeof(MyObjectBuilder_EntityCapacitorComponent), true)]
	public class MyEntityCapacitorComponent : MyGameLogicComponent
	{
		protected class m_storedPower_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType storedPower;
				ISyncType result = (storedPower = new Sync<float, SyncDirection.FromServer>(P_1, P_2));
				((MyEntityCapacitorComponent)P_0).m_storedPower = (Sync<float, SyncDirection.FromServer>)storedPower;
				return result;
			}
		}

		private class Sandbox_Game_Weapons_Guns_MyEntityCapacitorComponent_003C_003EActor : IActivator, IActivator<MyEntityCapacitorComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityCapacitorComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityCapacitorComponent CreateInstance()
			{
				return new MyEntityCapacitorComponent();
			}

			MyEntityCapacitorComponent IActivator<MyEntityCapacitorComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyResourceSinkComponent m_sink;

		private Func<float> m_parentRequiredFunc;

		private float m_parentRequired;

		private Action<MyEntityCapacitorComponent> OnCharged;

		private readonly Sync<float, SyncDirection.FromServer> m_storedPower;

		private float InitialStoredPower;

		private MyFunctionalBlock m_functionalBlock;

		public float Capacity { get; private set; }

		public float RechargeDraw { get; private set; }

		public float TimeRemaining { get; private set; }

		public bool IsCharged => (m_storedPower?.Value ?? 0f) >= Capacity;

		public float StoredPower => m_storedPower.Value;

		public override string ComponentTypeDebugString => "MyEntityCapacitorComponent";

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
			MyEntityCapacitorComponentDefinition myEntityCapacitorComponentDefinition = definition as MyEntityCapacitorComponentDefinition;
			Capacity = myEntityCapacitorComponentDefinition.Capacity;
			RechargeDraw = myEntityCapacitorComponentDefinition.RechargeDraw;
		}

		private void StoredPowerOnValueChanged(SyncBase obj)
		{
			SetStoredPower(m_storedPower);
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			m_sink.Update();
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (m_functionalBlock != null && m_functionalBlock.Enabled)
			{
				StorePower(1666.66663f, m_sink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId));
				m_functionalBlock.SetDetailedInfoDirty();
			}
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_functionalBlock = base.Entity as MyFunctionalBlock;
			m_functionalBlock.EnabledChanged += OnEnabledChanged;
			m_sink = base.Entity.Components.Get<MyResourceSinkComponent>();
			m_parentRequired = m_sink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);
			m_sink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, RechargeDraw);
			m_parentRequiredFunc = m_sink.SetRequiredInputFuncByType(MyResourceDistributorComponent.ElectricityId, RequiredInputFunc);
			OnCharged = (Action<MyEntityCapacitorComponent>)Delegate.Combine(OnCharged, new Action<MyEntityCapacitorComponent>(OnFullyCharged));
			SetStoredPower(InitialStoredPower);
			if (!MySession.Static.IsServer)
			{
				m_storedPower.ValueChanged += StoredPowerOnValueChanged;
			}
		}

		private void OnEnabledChanged(MyTerminalBlock obj)
		{
			if (m_storedPower.Value >= Capacity)
			{
				OnCharged?.Invoke(this);
			}
			else
			{
				OnNotCharged();
			}
		}

		private void OnFullyCharged(MyEntityCapacitorComponent obj)
		{
			m_sink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, m_parentRequired);
			m_sink.Update();
			base.NeedsUpdate = MyEntityUpdateEnum.NONE;
			m_functionalBlock.SetDetailedInfoDirty();
			TimeRemaining = 0f;
		}

		private void OnNotCharged()
		{
			if (m_functionalBlock != null)
			{
				if (m_functionalBlock.Enabled)
				{
					m_sink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, RechargeDraw);
					base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
				}
				else
				{
					TimeRemaining = 0f;
					m_sink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, 0f);
					m_sink.Update();
					base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
				}
				m_functionalBlock.SetDetailedInfoDirty();
			}
		}

		public void StorePower(float deltaTime, float input)
		{
			float num = input / 3600000f;
			float num2 = deltaTime * num * 0.8f;
			bool flag = false;
			if (Sync.IsServer)
			{
				flag = m_storedPower.Value < Capacity && m_storedPower.Value + num2 >= Capacity;
				m_storedPower.Value = Math.Min(Capacity, m_storedPower.Value + num2);
			}
			if (num2 > 0f)
			{
				TimeRemaining = (Capacity - (float)m_storedPower) * deltaTime / 1000f / num2;
			}
			else
			{
				TimeRemaining = float.PositiveInfinity;
			}
			if (flag)
			{
				OnCharged?.Invoke(this);
			}
		}

		/// <summary>
		/// Server side only
		/// </summary>
		public void SetStoredPower(float newValue)
		{
			float num = MathHelper.Clamp(newValue, 0f, Capacity);
			if (Sync.IsServer)
			{
				m_storedPower.Value = num;
			}
			else
			{
				m_storedPower.SetLocalValue(num);
			}
			if (m_storedPower.Value >= Capacity)
			{
				OnCharged?.Invoke(this);
			}
			else
			{
				OnNotCharged();
			}
		}

		private float RequiredInputFunc()
		{
			if (!m_functionalBlock.Enabled)
			{
				return 0f;
			}
			if (IsCharged)
			{
				return m_parentRequiredFunc?.Invoke() ?? m_sink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId);
			}
			return RechargeDraw;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			MyObjectBuilder_EntityCapacitorComponent myObjectBuilder_EntityCapacitorComponent = builder as MyObjectBuilder_EntityCapacitorComponent;
			InitialStoredPower = myObjectBuilder_EntityCapacitorComponent.StoredPower;
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_EntityCapacitorComponent obj = base.Serialize(copy) as MyObjectBuilder_EntityCapacitorComponent;
			obj.StoredPower = m_storedPower.Value;
			return obj;
		}

		public override bool IsSerialized()
		{
			return true;
		}

		public void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
			MyValueFormatter.AppendWorkInBestUnit(m_sink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_StoredPower));
			MyValueFormatter.AppendWorkHoursInBestUnit(m_storedPower, detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_RechargedIn));
			MyValueFormatter.AppendTimeInBestUnit(TimeRemaining, detailedInfo);
		}
	}
}
