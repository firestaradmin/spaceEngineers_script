using System.Collections.Generic;
using ParallelTasks;
using VRage.Game;
using VRage.Profiler;

namespace Sandbox.Game.World.Generator
{
	public class MyStoreItemsGenerator
	{
		private Dictionary<MyFactionTypes, IMyStoreItemsGeneratorFactionTypeStrategy> m_generatorStrategies = new Dictionary<MyFactionTypes, IMyStoreItemsGeneratorFactionTypeStrategy>();

		private Task m_initTask;

		public MyStoreItemsGenerator()
		{
			m_initTask = Parallel.Start(delegate
			{
				m_generatorStrategies.Add(MyFactionTypes.Miner, new MyMinersFactionTypeStrategy());
				m_generatorStrategies.Add(MyFactionTypes.Trader, new MyTradersFactionTypeStrategy());
				m_generatorStrategies.Add(MyFactionTypes.Builder, new MyBuildersFactionTypeStrategy());
			}, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "MyStoreItemsGenerator.ctor"), WorkPriority.VeryLow);
		}

		public void Update(MyFaction faction, bool firstGeneration)
		{
			if (m_initTask.valid)
			{
				m_initTask.WaitOrExecute();
				m_initTask = default(Task);
			}
			if (m_generatorStrategies.TryGetValue(faction.FactionType, out var value))
			{
				value.UpdateStationsStoreItems(faction, firstGeneration);
			}
			else if (faction.FactionType != 0)
			{
				_ = faction.FactionType;
				_ = 1;
			}
		}
	}
}
