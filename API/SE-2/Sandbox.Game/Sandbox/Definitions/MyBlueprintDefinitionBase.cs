using System.Collections.Generic;
using System.Diagnostics;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Definitions
{
	public abstract class MyBlueprintDefinitionBase : MyDefinitionBase
	{
		public struct Item
		{
			public MyDefinitionId Id;

			/// <summary>
			/// Amount of item required or produced. For discrete objects this refers to
			/// pieces. For ingots and ore, this refers to volume in m^3.
			/// </summary>
			public MyFixedPoint Amount;

			public override string ToString()
			{
				return $"{Amount}x {Id}";
			}

			public static Item FromObjectBuilder(BlueprintItem obItem)
			{
				Item result = default(Item);
				result.Id = obItem.Id;
				result.Amount = MyFixedPoint.DeserializeStringSafe(obItem.Amount);
				return result;
			}
		}

		public struct ProductionInfo
		{
			public MyBlueprintDefinitionBase Blueprint;

			public MyFixedPoint Amount;
		}

		public Item[] Prerequisites;

		public Item[] Results;

		public string ProgressBarSoundCue;

		/// <summary>
		/// Base production time in seconds, which is affected by speed increase of
		/// refinery or assembler.
		/// </summary>
		public float BaseProductionTimeInSeconds = 1f;

		/// <summary>
		/// Total volume of products created by one unit of blueprint. This is for production calculation purposes.
		/// </summary>
		public float OutputVolume;

		/// <summary>
		/// Whether the the blueprint's outputs have to be produced as a whole at once (because you cannot divide some output items)
		/// </summary>
		public bool Atomic;

		public bool IsPrimary;

		/// <summary>
		/// Priority for sorting inside production screen within tabs. Higher priority blueprints will show up before the others
		/// </summary>
		public int Priority;

		public MyObjectBuilderType InputItemType => Prerequisites[0].Id.TypeId;

<<<<<<< HEAD
		/// <summary>
		/// Whether the Postprocess method still needs to be called.
		/// </summary>
		public bool PostprocessNeeded { get; protected set; }
=======
		public bool PostprocessNeeded { get; protected set; }

		public new abstract void Postprocess();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		[Conditional("DEBUG")]
		private void VerifyInputItemType(MyObjectBuilderType inputType)
		{
			Item[] prerequisites = Prerequisites;
			for (int i = 0; i < prerequisites.Length; i++)
			{
				_ = ref prerequisites[i];
			}
		}

		public override string ToString()
		{
			return string.Format("(" + DisplayNameEnum.GetValueOrDefault(MyStringId.NullOrEmpty).String + "){{{0}}}->{{{1}}}", string.Join(" ", Prerequisites), string.Join(" ", Results));
		}

		/// <summary>
		/// Should return the number of added blueprints (to make building hierarchical blueprint production infos easier)
		/// </summary>
		public abstract int GetBlueprints(List<ProductionInfo> blueprints);
	}
}
