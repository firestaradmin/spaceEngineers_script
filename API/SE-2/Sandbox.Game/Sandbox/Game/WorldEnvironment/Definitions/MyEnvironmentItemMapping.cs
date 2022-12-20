using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
	public class MyEnvironmentItemMapping
	{
		public MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>[] Samplers;

		public int[] Keys;

		public MyEnvironmentRule Rule;

		/// Weather this mapping is valid.
		public bool Valid => Samplers != null;

		public MyEnvironmentItemMapping(MyRuntimeEnvironmentItemInfo[] map, MyEnvironmentRule rule, MyProceduralEnvironmentDefinition env)
		{
			Rule = rule;
			SortedDictionary<int, List<MyRuntimeEnvironmentItemInfo>> val = new SortedDictionary<int, List<MyRuntimeEnvironmentItemInfo>>();
			List<MyRuntimeEnvironmentItemInfo> list = default(List<MyRuntimeEnvironmentItemInfo>);
			foreach (MyRuntimeEnvironmentItemInfo myRuntimeEnvironmentItemInfo in map)
			{
				MyItemTypeDefinition type = myRuntimeEnvironmentItemInfo.Type;
<<<<<<< HEAD
				if (!sortedDictionary.TryGetValue(type.LodFrom + 1, out var value))
=======
				if (!val.TryGetValue(type.LodFrom + 1, ref list))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					list = new List<MyRuntimeEnvironmentItemInfo>();
					val.set_Item(type.LodFrom + 1, list);
				}
				list.Add(myRuntimeEnvironmentItemInfo);
			}
			Keys = Enumerable.ToArray<int>((IEnumerable<int>)val.get_Keys());
			List<MyRuntimeEnvironmentItemInfo>[] array = Enumerable.ToArray<List<MyRuntimeEnvironmentItemInfo>>((IEnumerable<List<MyRuntimeEnvironmentItemInfo>>)val.get_Values());
			Samplers = new MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>[Keys.Length];
			for (int j = 0; j < Keys.Length; j++)
			{
				Samplers[j] = PrepareSampler(Enumerable.SelectMany<List<MyRuntimeEnvironmentItemInfo>, MyRuntimeEnvironmentItemInfo>((IEnumerable<List<MyRuntimeEnvironmentItemInfo>>)array.Range(j, array.Length), (Func<List<MyRuntimeEnvironmentItemInfo>, IEnumerable<MyRuntimeEnvironmentItemInfo>>)((List<MyRuntimeEnvironmentItemInfo> x) => x)));
			}
		}

		public MyDiscreteSampler<MyRuntimeEnvironmentItemInfo> PrepareSampler(IEnumerable<MyRuntimeEnvironmentItemInfo> items)
		{
			float num = 0f;
			foreach (MyRuntimeEnvironmentItemInfo item in items)
			{
				num += item.Density;
			}
			if (num < 1f)
			{
<<<<<<< HEAD
				return new MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>(items.Concat(new MyRuntimeEnvironmentItemInfo[1]), items.Select((MyRuntimeEnvironmentItemInfo x) => x.Density).Concat(new float[1] { 1f - num }));
=======
				return new MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>(Enumerable.Concat<MyRuntimeEnvironmentItemInfo>(items, (IEnumerable<MyRuntimeEnvironmentItemInfo>)new MyRuntimeEnvironmentItemInfo[1]), Enumerable.Concat<float>(Enumerable.Select<MyRuntimeEnvironmentItemInfo, float>(items, (Func<MyRuntimeEnvironmentItemInfo, float>)((MyRuntimeEnvironmentItemInfo x) => x.Density)), (IEnumerable<float>)new float[1] { 1f - num }));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return new MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>(items, Enumerable.Select<MyRuntimeEnvironmentItemInfo, float>(items, (Func<MyRuntimeEnvironmentItemInfo, float>)((MyRuntimeEnvironmentItemInfo x) => x.Density)));
		}

		/// Given a value between 0 and 1 this will return the id of a vegetation item in which
		/// range the value falls.
		///
		/// If the value of rate is uniformly distributed then the definitions will be distributed
		/// according to their defined densities.
		public MyRuntimeEnvironmentItemInfo GetItemRated(int lod, float rate)
		{
			int num = Keys.BinaryIntervalSearch(lod);
			if (num > Samplers.Length)
			{
				return null;
			}
			return Samplers[num].Sample(rate);
		}

		public bool ValidForLod(int lod)
		{
			if (Keys.BinaryIntervalSearch(lod) > Samplers.Length)
			{
				return false;
			}
			return true;
		}

		public MyDiscreteSampler<MyRuntimeEnvironmentItemInfo> Sampler(int lod)
		{
			int num = Keys.BinaryIntervalSearch(lod);
			if (num >= Samplers.Length)
			{
				return null;
			}
			return Samplers[num];
		}
	}
}
