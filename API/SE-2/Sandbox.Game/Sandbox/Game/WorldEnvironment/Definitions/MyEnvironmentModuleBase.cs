using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using VRage.Game;
using VRage.Library.Utils;
using VRage.ObjectBuilders;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
	public abstract class MyEnvironmentModuleBase : IMyEnvironmentModule
	{
		protected MyLogicalEnvironmentSectorBase Sector;

		public virtual void ProcessItems(Dictionary<short, MyLodEnvironmentItemSet> items, int changedLodMin, int changedLodMax)
		{
			using MyEnvironmentModelUpdateBatch myEnvironmentModelUpdateBatch = new MyEnvironmentModelUpdateBatch(Sector);
			foreach (KeyValuePair<short, MyLodEnvironmentItemSet> item in items)
			{
				Sector.GetItemDefinition((ushort)item.Key, out var def);
				MyDefinitionId subtypeId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalModelCollectionDefinition), def.Subtype);
				MyPhysicalModelCollectionDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPhysicalModelCollectionDefinition>(subtypeId);
				if (definition == null)
				{
					continue;
				}
				foreach (int item2 in item.Value.Items)
				{
<<<<<<< HEAD
					Sector.GetItemDefinition((ushort)item.Key, out var def);
					MyDefinitionId subtypeId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalModelCollectionDefinition), def.Subtype);
					MyPhysicalModelCollectionDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPhysicalModelCollectionDefinition>(subtypeId);
					if (definition == null)
					{
						continue;
					}
					foreach (int item2 in item.Value.Items)
					{
						float sample = MyHashRandomUtils.UniformFloatFromSeed(item2);
						MyDefinitionId modelDef = definition.Items.Sample(sample);
						myEnvironmentModelUpdateBatch.Add(modelDef, item2);
					}
=======
					float sample = MyHashRandomUtils.UniformFloatFromSeed(item2);
					MyDefinitionId modelDef = definition.Items.Sample(sample);
					myEnvironmentModelUpdateBatch.Add(modelDef, item2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		public virtual void Init(MyLogicalEnvironmentSectorBase sector, MyObjectBuilder_Base ob)
		{
			Sector = sector;
		}

		public abstract void Close();

		public abstract MyObjectBuilder_EnvironmentModuleBase GetObjectBuilder();

		public abstract void OnItemEnable(int item, bool enable);

		public abstract void HandleSyncEvent(int logicalItem, object data, bool fromClient);

		public virtual void DebugDraw()
		{
		}
	}
}
