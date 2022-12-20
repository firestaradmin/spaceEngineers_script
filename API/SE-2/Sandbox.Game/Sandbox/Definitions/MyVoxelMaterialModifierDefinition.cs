using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_VoxelMaterialModifierDefinition), typeof(Postprocessor))]
	public class MyVoxelMaterialModifierDefinition : MyDefinitionBase
	{
		private class Postprocessor : MyDefinitionPostprocessor
		{
			public override void AfterLoaded(ref Bundle definitions)
			{
			}

			public override void AfterPostprocess(MyDefinitionSet set, Dictionary<MyStringHash, MyDefinitionBase> definitions)
			{
				foreach (MyVoxelMaterialModifierDefinition value in definitions.Values)
				{
					value.Options = new MyDiscreteSampler<VoxelMapChange>(Enumerable.Select<MyVoxelMapModifierOption, VoxelMapChange>((IEnumerable<MyVoxelMapModifierOption>)value.m_ob.Options, (Func<MyVoxelMapModifierOption, VoxelMapChange>)delegate(MyVoxelMapModifierOption x)
					{
						VoxelMapChange result = default(VoxelMapChange);
						result.Changes = ((x.Changes == null) ? null : Enumerable.ToDictionary<MyVoxelMapModifierChange, byte, byte>((IEnumerable<MyVoxelMapModifierChange>)x.Changes, (Func<MyVoxelMapModifierChange, byte>)((MyVoxelMapModifierChange y) => MyDefinitionManager.Static.GetVoxelMaterialDefinition(y.From).Index), (Func<MyVoxelMapModifierChange, byte>)((MyVoxelMapModifierChange y) => MyDefinitionManager.Static.GetVoxelMaterialDefinition(y.To).Index)));
						return result;
					}), Enumerable.Select<MyVoxelMapModifierOption, float>((IEnumerable<MyVoxelMapModifierOption>)value.m_ob.Options, (Func<MyVoxelMapModifierOption, float>)((MyVoxelMapModifierOption x) => x.Chance)));
					value.m_ob = null;
				}
			}
		}

		private class Sandbox_Definitions_MyVoxelMaterialModifierDefinition_003C_003EActor : IActivator, IActivator<MyVoxelMaterialModifierDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyVoxelMaterialModifierDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVoxelMaterialModifierDefinition CreateInstance()
			{
				return new MyVoxelMaterialModifierDefinition();
			}

			MyVoxelMaterialModifierDefinition IActivator<MyVoxelMaterialModifierDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDiscreteSampler<VoxelMapChange> Options;

		private MyObjectBuilder_VoxelMaterialModifierDefinition m_ob;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			m_ob = (MyObjectBuilder_VoxelMaterialModifierDefinition)builder;
		}
	}
}
