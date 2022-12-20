using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_ExhaustBlock))]
	public class MyExhaustBlock : MyFunctionalBlock, IMyExhaustBlock
	{
		private class MyExhaustPipe
		{
			public MyParticleEffect Effect;

			public MyObjectBuilder_ExhaustEffectDefinition.Pipe Data;
		}

		protected class m_exhaustEffect_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType exhaustEffect;
				ISyncType result = (exhaustEffect = new Sync<string, SyncDirection.BothWays>(P_1, P_2));
				((MyExhaustBlock)P_0).m_exhaustEffect = (Sync<string, SyncDirection.BothWays>)exhaustEffect;
				return result;
			}
		}

		protected class m_powerDependency_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType powerDependency;
				ISyncType result = (powerDependency = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyExhaustBlock)P_0).m_powerDependency = (Sync<float, SyncDirection.BothWays>)powerDependency;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyExhaustBlock_003C_003EActor : IActivator, IActivator<MyExhaustBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyExhaustBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyExhaustBlock CreateInstance()
			{
				return new MyExhaustBlock();
			}

			MyExhaustBlock IActivator<MyExhaustBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private List<MyExhaustPipe> m_exhaustPipes = new List<MyExhaustPipe>();

		private Dictionary<string, MatrixD> m_exhaustDummies = new Dictionary<string, MatrixD>();

		private readonly Sync<string, SyncDirection.BothWays> m_exhaustEffect;

		private readonly Sync<float, SyncDirection.BothWays> m_powerDependency;

		private float m_currentGridPower;

		private float m_lastGridPower;

		public new MyExhaustBlockDefinition BlockDefinition => (MyExhaustBlockDefinition)base.BlockDefinition;

		public MyExhaustBlock()
		{
			CreateTerminalControls();
			m_exhaustEffect.SetLocalValue("");
			m_exhaustEffect.ValueChanged += exhaustEffect_ValueChanged;
			m_powerDependency.ValueChanged += delegate
			{
				UpdateEffectValues();
			};
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
<<<<<<< HEAD
			myResourceSinkComponent.Init(MyStringHash.GetOrCompute("Utility"), BlockDefinition.RequiredPowerInput, () => (!Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), this);
=======
			myResourceSinkComponent.Init(MyStringHash.GetOrCompute("Utility"), BlockDefinition.RequiredPowerInput, () => (!base.Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink = myResourceSinkComponent;
			m_exhaustPipes.Clear();
			base.Init(objectBuilder, cubeGrid);
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.IsWorkingChanged += CubeBlock_OnWorkingChanged;
			MyObjectBuilder_ExhaustBlock myObjectBuilder_ExhaustBlock = (MyObjectBuilder_ExhaustBlock)objectBuilder;
			string text = myObjectBuilder_ExhaustBlock.ExhaustEffect;
<<<<<<< HEAD
			if (!string.IsNullOrEmpty(text) && !BlockDefinition.AvailableEffects.Contains(text))
			{
				text = null;
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (string.IsNullOrEmpty(text))
			{
				IEnumerable<MyExhaustEffectDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyExhaustEffectDefinition>();
				if (allDefinitions != null)
				{
<<<<<<< HEAD
					text = allDefinitions.FirstOrDefault((MyExhaustEffectDefinition x) => BlockDefinition.AvailableEffects.Contains(x.Id.SubtypeName))?.Id.SubtypeName;
				}
			}
			m_exhaustEffect.SetLocalValue(text);
			m_powerDependency.ValidateRange(0f, 1f);
=======
					text = Enumerable.First<MyExhaustEffectDefinition>(allDefinitions).Id.SubtypeName;
				}
			}
			m_exhaustEffect.SetLocalValue(text);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_powerDependency.SetLocalValue(myObjectBuilder_ExhaustBlock.PowerDependency);
			UpdatePipes();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			StopEffects();
			StartEffects();
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyExhaustBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<MyExhaustBlock>("EffectsCombo", MySpaceTexts.BlockPropertyTitle_ExhaustEffect, MySpaceTexts.Blank)
				{
<<<<<<< HEAD
					ComboBoxContentWithBlock = FillEffectsCombo,
=======
					ComboBoxContent = FillEffectsCombo,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					Getter = (MyExhaustBlock x) => (int)MyStringHash.GetOrCompute(x.m_exhaustEffect),
					Setter = delegate(MyExhaustBlock x, long v)
					{
						x.m_exhaustEffect.Value = MyStringHash.TryGet((int)v).ToString();
					}
				});
				MyTerminalControlSlider<MyExhaustBlock> myTerminalControlSlider = new MyTerminalControlSlider<MyExhaustBlock>("PowerDependency", MySpaceTexts.BlockPropertyTitle_PowerDependency, MySpaceTexts.Blank);
				myTerminalControlSlider.SetLimits((MyExhaustBlock x) => 0f, (MyExhaustBlock x) => 1f);
				myTerminalControlSlider.DefaultValue = 0f;
				myTerminalControlSlider.Getter = (MyExhaustBlock x) => x.m_powerDependency;
				myTerminalControlSlider.Setter = delegate(MyExhaustBlock x, float v)
				{
					x.m_powerDependency.Value = v;
				};
				myTerminalControlSlider.Writer = delegate(MyExhaustBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_powerDependency.Value * 100f, 2)).Append(" %");
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			}
		}

<<<<<<< HEAD
		private void FillEffectsCombo(MyExhaustBlock block, ICollection<MyTerminalControlComboBoxItem> list)
=======
		private void FillEffectsCombo(List<MyTerminalControlComboBoxItem> list)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			IEnumerable<MyExhaustEffectDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyExhaustEffectDefinition>();
			if (allDefinitions == null)
			{
				return;
			}
			foreach (MyExhaustEffectDefinition item in allDefinitions)
			{
<<<<<<< HEAD
				if (block.BlockDefinition.AvailableEffects.Contains(item.Id.SubtypeName))
				{
					list.Add(new MyTerminalControlComboBoxItem
					{
						Key = MyStringHash.GetOrCompute(item.Id.SubtypeName).GetHashCode(),
						Value = MyStringId.GetOrCompute(item.Id.SubtypeName)
					});
				}
=======
				list.Add(new MyTerminalControlComboBoxItem
				{
					Key = MyStringHash.GetOrCompute(item.Id.SubtypeName).GetHashCode(),
					Value = MyStringId.GetOrCompute(item.Id.SubtypeName)
				});
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void exhaustEffect_ValueChanged(SyncBase obj)
		{
			StopEffects();
			UpdatePipes();
			StartEffects();
		}

		public void SelectEffect(string name)
		{
			m_exhaustEffect.Value = name;
		}

		private void UpdatePipes()
		{
			m_exhaustPipes.Clear();
			if (string.IsNullOrEmpty(m_exhaustEffect.Value))
			{
				return;
			}
			MyExhaustEffectDefinition definition = MyDefinitionManager.Static.GetDefinition<MyExhaustEffectDefinition>(m_exhaustEffect.Value);
			if (definition == null)
			{
				return;
			}
			foreach (MyObjectBuilder_ExhaustEffectDefinition.Pipe exhaustPipe in definition.ExhaustPipes)
			{
				m_exhaustPipes.Add(new MyExhaustPipe
				{
					Data = exhaustPipe
				});
			}
		}

		public void StopEffects()
		{
			foreach (MyExhaustPipe exhaustPipe in m_exhaustPipes)
			{
				if (exhaustPipe.Effect != null)
				{
					exhaustPipe.Effect.Stop(instant: false);
					exhaustPipe.Effect = null;
				}
			}
		}

		public void StartEffects()
		{
<<<<<<< HEAD
			if (!base.IsWorking || !Enabled || base.IsPreview || base.CubeGrid.IsPreview || Sync.IsDedicated)
=======
			if (!base.IsWorking || !base.Enabled || base.IsPreview || base.CubeGrid.IsPreview || Sync.IsDedicated)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			Vector3D worldPosition = base.PositionComp.GetPosition();
			foreach (MyExhaustPipe exhaustPipe in m_exhaustPipes)
			{
				if (m_exhaustDummies.ContainsKey(exhaustPipe.Data.Dummy) && !string.IsNullOrEmpty(exhaustPipe.Data.Effect))
				{
<<<<<<< HEAD
					MatrixD effectMatrix = m_exhaustDummies[exhaustPipe.Data.Dummy] * base.PositionComp.LocalMatrixRef;
=======
					MatrixD effectMatrix = m_exhaustDummies[exhaustPipe.Data.Dummy] * base.PositionComp.LocalMatrix;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MyParticlesManager.TryCreateParticleEffect(exhaustPipe.Data.Effect, ref effectMatrix, ref worldPosition, base.Render.ParentIDs[0], out exhaustPipe.Effect);
					if (exhaustPipe.Effect != null)
					{
						UpdateEffectValues(exhaustPipe);
					}
				}
			}
		}

		private void UpdateEffectValues(MyExhaustPipe pipe)
		{
			if (pipe.Effect != null)
			{
				MyObjectBuilder_ExhaustEffectDefinition.Pipe data = pipe.Data;
				pipe.Effect.UserScale = ((data.EffectIntensity > 0f) ? data.EffectIntensity : 1f);
				float num = ((m_powerDependency.Value > 0f) ? MathHelper.Clamp(m_currentGridPower / m_powerDependency.Value, 0f, 1f) : 1f);
				pipe.Effect.UserFadeMultiplier = num;
				if (data.PowerToRadius == 0f)
				{
					pipe.Effect.UserRadiusMultiplier = num;
				}
				else
				{
					pipe.Effect.UserRadiusMultiplier = num * data.PowerToRadius;
				}
				if (data.PowerToBirth == 0f)
				{
					pipe.Effect.UserBirthMultiplier = num;
				}
				else
				{
					pipe.Effect.UserBirthMultiplier = num * data.PowerToBirth;
				}
				if (data.PowerToVelocity == 0f)
				{
					pipe.Effect.UserVelocityMultiplier = num;
				}
				else
				{
					pipe.Effect.UserVelocityMultiplier = num * data.PowerToVelocity;
				}
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			StopEffects();
			m_exhaustDummies.Clear();
			if (base.IsBuilt)
			{
				foreach (KeyValuePair<string, MyModelDummy> dummy in base.Model.Dummies)
				{
					m_exhaustDummies.Add(dummy.Key, MatrixD.Normalize(dummy.Value.Matrix));
				}
			}
			StartEffects();
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private void CubeBlock_OnWorkingChanged(MyCubeBlock block)
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void Closing()
		{
			base.Closing();
			StopEffects();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_ExhaustBlock obj = (MyObjectBuilder_ExhaustBlock)base.GetObjectBuilderCubeBlock(copy);
			obj.ExhaustEffect = m_exhaustEffect;
			obj.PowerDependency = m_powerDependency;
			return obj;
		}

		private void UpdateEffectValues()
		{
			foreach (MyExhaustPipe exhaustPipe in m_exhaustPipes)
			{
				UpdateEffectValues(exhaustPipe);
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			m_currentGridPower = 0f;
			MyGridResourceDistributorSystem resourceDistributor = base.CubeGrid.GridSystems.ResourceDistributor;
			if (resourceDistributor != null)
			{
<<<<<<< HEAD
				float num = resourceDistributor.MaxAvailableResourceByType(MyResourceDistributorComponent.ElectricityId, base.CubeGrid);
				float num2 = MyMath.Clamp(resourceDistributor.TotalRequiredInputByType(MyResourceDistributorComponent.ElectricityId, base.CubeGrid), 0f, num);
=======
				float num = resourceDistributor.MaxAvailableResourceByType(MyResourceDistributorComponent.ElectricityId);
				float num2 = MyMath.Clamp(resourceDistributor.TotalRequiredInputByType(MyResourceDistributorComponent.ElectricityId), 0f, num);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (num > 0f)
				{
					m_currentGridPower = num2 / num;
				}
			}
			foreach (MyExhaustPipe exhaustPipe in m_exhaustPipes)
			{
				if (exhaustPipe.Effect != null && exhaustPipe.Effect.GetName() != exhaustPipe.Data.Effect)
				{
					base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
			}
			if (m_lastGridPower != m_currentGridPower)
			{
				UpdateEffectValues();
				m_lastGridPower = m_currentGridPower;
			}
			_ = m_exhaustPipes.Count;
			_ = 0;
		}
	}
}
