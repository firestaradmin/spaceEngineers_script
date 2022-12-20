using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_BlockVariantGroup), null)]
	public class MyBlockVariantGroup : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyBlockVariantGroup_003C_003EActor : IActivator, IActivator<MyBlockVariantGroup>
		{
			private sealed override object CreateInstance()
			{
				return new MyBlockVariantGroup();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBlockVariantGroup CreateInstance()
			{
				return new MyBlockVariantGroup();
			}

			MyBlockVariantGroup IActivator<MyBlockVariantGroup>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyCubeBlockDefinition[] Blocks;

		private SerializableDefinitionId[] m_blockIdsToResolve;

		public MyCubeBlockDefinitionGroup[] BlockGroups { get; private set; }

		public MyCubeBlockDefinition PrimaryGUIBlock { get; private set; }

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_BlockVariantGroup myObjectBuilder_BlockVariantGroup = (MyObjectBuilder_BlockVariantGroup)builder;
			m_blockIdsToResolve = myObjectBuilder_BlockVariantGroup.Blocks;
		}

		public void ResolveBlocks()
		{
			Blocks = new MyCubeBlockDefinition[m_blockIdsToResolve.Length];
			bool flag = false;
			for (int i = 0; i < m_blockIdsToResolve.Length; i++)
			{
				SerializableDefinitionId serializableDefinitionId = m_blockIdsToResolve[i];
				if (MyDefinitionManager.Static.TryGetDefinition<MyCubeBlockDefinition>(serializableDefinitionId, out var definition))
				{
					Blocks[i] = definition;
				}
				else
				{
					flag = true;
				}
			}
			if (flag)
			{
<<<<<<< HEAD
				Blocks = Blocks.Where((MyCubeBlockDefinition x) => x != null).ToArray();
=======
				Blocks = Enumerable.ToArray<MyCubeBlockDefinition>(Enumerable.Where<MyCubeBlockDefinition>((IEnumerable<MyCubeBlockDefinition>)Blocks, (Func<MyCubeBlockDefinition, bool>)((MyCubeBlockDefinition x) => x != null)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_blockIdsToResolve = null;
		}

		public override void Postprocess()
		{
			HashSet<MyCubeBlockDefinitionGroup> val = new HashSet<MyCubeBlockDefinitionGroup>();
			MyCubeBlockDefinition[] blocks = Blocks;
			foreach (MyCubeBlockDefinition myCubeBlockDefinition in blocks)
			{
				val.Add(MyDefinitionManager.Static.GetDefinitionGroup(myCubeBlockDefinition.BlockPairName));
			}
			BlockGroups = Enumerable.ToArray<MyCubeBlockDefinitionGroup>((IEnumerable<MyCubeBlockDefinitionGroup>)val);
			CreateBlockStages();
			PrimaryGUIBlock = Blocks[0];
			blocks = Blocks;
			foreach (MyCubeBlockDefinition myCubeBlockDefinition2 in blocks)
			{
				myCubeBlockDefinition2.GuiVisible = PrimaryGUIBlock == myCubeBlockDefinition2;
			}
			if (Icons.IsNullOrEmpty())
			{
				Icons = PrimaryGUIBlock.Icons;
			}
			if (!DisplayNameEnum.HasValue)
			{
				if (!string.IsNullOrEmpty(DisplayNameString))
				{
					DisplayNameEnum = MyStringId.GetOrCompute(DisplayNameString);
				}
				else if (!string.IsNullOrEmpty(DisplayNameText))
				{
					DisplayNameEnum = MyStringId.GetOrCompute(DisplayNameText);
				}
				else if (PrimaryGUIBlock.DisplayNameEnum.HasValue)
				{
					DisplayNameEnum = PrimaryGUIBlock.DisplayNameEnum.Value;
				}
				else
				{
					DisplayNameEnum = MyStringId.GetOrCompute(PrimaryGUIBlock.DisplayNameText);
				}
			}
		}

		internal void CleanUp()
		{
			Blocks = null;
			BlockGroups = null;
			PrimaryGUIBlock = null;
		}

		private void CreateBlockStages()
		{
			MyCubeSize[] blockSizes = MyEnum<MyCubeSize>.Values;
			MyCubeBlockDefinitionGroup myCubeBlockDefinitionGroup = null;
			MyCubeBlockDefinition[] blocks = Blocks;
			foreach (MyCubeBlockDefinition myCubeBlockDefinition in blocks)
			{
				MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(myCubeBlockDefinition.BlockPairName);
				if (HasBlocksForAllSizes(definitionGroup))
				{
					myCubeBlockDefinitionGroup = definitionGroup;
					break;
				}
			}
			MyCubeSize[] array2;
			if (myCubeBlockDefinitionGroup != null)
			{
				int j;
				for (j = 0; j < Blocks.Length; j++)
				{
					MyCubeBlockDefinition myCubeBlockDefinition2 = Blocks[j];
					bool flag = false;
					array2 = blockSizes;
					foreach (MyCubeSize size2 in array2)
					{
						if (myCubeBlockDefinitionGroup[size2] == myCubeBlockDefinition2)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
				}
				array2 = blockSizes;
				foreach (MyCubeSize size3 in array2)
				{
					MyCubeBlockDefinition myCubeBlockDefinition3 = myCubeBlockDefinitionGroup[size3];
					ConstructFullVariantsFor(myCubeBlockDefinition3);
					if (MoveFront<MyCubeBlockDefinition>(Blocks, myCubeBlockDefinition3, j))
					{
						j++;
					}
				}
				MoveFront<MyCubeBlockDefinitionGroup>(BlockGroups, myCubeBlockDefinitionGroup, 0);
				return;
			}
			array2 = blockSizes;
			foreach (MyCubeSize size in array2)
			{
				MyCubeBlockDefinition myCubeBlockDefinition4 = Enumerable.FirstOrDefault<MyCubeBlockDefinition>((IEnumerable<MyCubeBlockDefinition>)Blocks, (Func<MyCubeBlockDefinition, bool>)((MyCubeBlockDefinition x) => x.CubeSize == size && x.Public));
				if (myCubeBlockDefinition4 != null)
				{
					ConstructFullVariantsFor(myCubeBlockDefinition4);
				}
			}
			void ConstructFullVariantsFor(MyCubeBlockDefinition block)
			{
				block.BlockStages = Enumerable.ToArray<MyDefinitionId>(Enumerable.Select<MyCubeBlockDefinition, MyDefinitionId>(Enumerable.Where<MyCubeBlockDefinition>((IEnumerable<MyCubeBlockDefinition>)Blocks, (Func<MyCubeBlockDefinition, bool>)((MyCubeBlockDefinition x) => x != block && x.CubeSize == block.CubeSize)), (Func<MyCubeBlockDefinition, MyDefinitionId>)((MyCubeBlockDefinition x) => x.Id)));
			}
			bool HasBlocksForAllSizes(MyCubeBlockDefinitionGroup blockPair)
			{
				MyCubeSize[] array3 = blockSizes;
				foreach (MyCubeSize size4 in array3)
				{
					if (blockPair[size4] == null)
					{
						return false;
					}
				}
				return true;
			}
<<<<<<< HEAD
			bool MoveFront<T>(T[] array, T element, int offset)
=======
			static bool MoveFront<T>(T[] array, T element, int offset)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				int num = Array.IndexOf(array, element);
				if (num > offset)
				{
					Array.Copy(array, offset, array, offset + 1, num - offset);
					array[offset] = element;
					return true;
				}
				return false;
			}
		}
	}
}
