using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Entities.Cube.CubeBuilder
{
	/// <summary>
	/// Class that handles cube builder state.
	/// </summary>
	public class MyCubeBuilderState
	{
		/// <summary>
		/// Store last rotation for each block definition.
		/// </summary>
		public Dictionary<MyDefinitionId, Quaternion> RotationsByDefinitionHash = new Dictionary<MyDefinitionId, Quaternion>(MyDefinitionId.Comparer);

		/// <summary>
		/// Index of last selected block group within a variant groups
		/// </summary>
		public Dictionary<MyDefinitionId, int> LastSelectedStageIndexForGroup = new Dictionary<MyDefinitionId, int>(MyDefinitionId.Comparer);

		/// <summary>
		/// Block definition stages.
		/// </summary>
		public List<MyCubeBlockDefinition> CurrentBlockDefinitionStages = new List<MyCubeBlockDefinition>();

		/// <summary>
		/// Block definitions with variants.
		/// </summary>
		private MyCubeBlockDefinitionWithVariants m_definitionWithVariants;

		/// <summary>
		/// Indicates what build mode is on (small or big grid)
		/// </summary>
		private MyCubeSize m_cubeSizeMode;

		public MyCubeBlockDefinition CurrentBlockDefinition
		{
			get
			{
				return m_definitionWithVariants;
			}
			set
			{
				if (value == null)
				{
					m_definitionWithVariants = null;
					CurrentBlockDefinitionStages.Clear();
					return;
				}
				m_definitionWithVariants = new MyCubeBlockDefinitionWithVariants(value, -1);
				if (!MyFakes.ENABLE_BLOCK_STAGES || CurrentBlockDefinitionStages.Contains(value))
				{
					return;
				}
				CurrentBlockDefinitionStages.Clear();
				if (value.BlockStages == null)
				{
					return;
				}
				CurrentBlockDefinitionStages.Add(value);
				MyDefinitionId[] blockStages = value.BlockStages;
				foreach (MyDefinitionId defId in blockStages)
				{
					MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out var blockDefinition);
					if (blockDefinition != null)
					{
						CurrentBlockDefinitionStages.Add(blockDefinition);
					}
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Block definition set on activation of cube builder.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyCubeBlockDefinition StartBlockDefinition { get; private set; }

		/// <summary>
		/// Current cube size mode.
		/// </summary>
		public MyCubeSize CubeSizeMode => m_cubeSizeMode;

		public event Action<MyCubeSize> OnBlockSizeChanged;

		public void SetCurrentBlockForBlockVariantGroup(MyCubeBlockDefinitionGroup blockGroup)
		{
			MyBlockVariantGroup myBlockVariantGroup = blockGroup.AnyPublic?.BlockVariantsGroup;
			if (myBlockVariantGroup != null)
			{
				int value = Array.IndexOf(myBlockVariantGroup.BlockGroups, blockGroup);
				LastSelectedStageIndexForGroup[myBlockVariantGroup.Id] = value;
			}
		}

		public MyCubeBlockDefinitionGroup GetCurrentBlockForBlockVariantGroup(MyBlockVariantGroup variants, bool respectRestrictions = false)
		{
			int valueOrDefault = LastSelectedStageIndexForGroup.GetValueOrDefault(variants.Id, 0);
			return GetCurrentBlockForBlockVariantGroup(valueOrDefault, variants, respectRestrictions);
		}

		public MyCubeBlockDefinitionGroup GetFirstBlockForBlockVariantGroup(MyBlockVariantGroup variants, bool respectRestrictions = false)
		{
			return GetCurrentBlockForBlockVariantGroup(0, variants, respectRestrictions);
		}

		public MyCubeBlockDefinitionGroup GetCurrentBlockForBlockVariantGroup(int idx, MyBlockVariantGroup variants, bool respectRestrictions = false)
		{
			if (idx >= variants.BlockGroups.Length)
			{
				idx = 0;
			}
			int i = 0;
			int num;
			for (num = variants.BlockGroups.Length; i < num; i++)
			{
				MyCubeBlockDefinition anyPublic = variants.BlockGroups[(idx + i) % num].AnyPublic;
				if (!respectRestrictions || (MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(anyPublic, Sync.MyId) && (MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySessionComponentResearch.Static.CanUse(MySession.Static.LocalCharacter, anyPublic.Id))))
				{
					break;
				}
			}
			return variants.BlockGroups[(idx + i) % num];
		}

		public void UpdateCubeBlockDefinition(MyDefinitionId? id, MatrixD localMatrixAdd)
		{
			if (!id.HasValue)
			{
				return;
			}
			if (CurrentBlockDefinition != null)
			{
				MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(CurrentBlockDefinition.BlockPairName);
				if (CurrentBlockDefinitionStages.Count > 1)
				{
					definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(CurrentBlockDefinitionStages[0].BlockPairName);
				}
				Quaternion value = Quaternion.CreateFromRotationMatrix(in localMatrixAdd);
				if (definitionGroup.Small != null)
				{
					RotationsByDefinitionHash[definitionGroup.Small.Id] = value;
				}
				if (definitionGroup.Large != null)
				{
					RotationsByDefinitionHash[definitionGroup.Large.Id] = value;
				}
			}
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(id.Value);
			if (cubeBlockDefinition.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS)
			{
				CurrentBlockDefinition = cubeBlockDefinition;
			}
			else
			{
				CurrentBlockDefinition = ((cubeBlockDefinition.CubeSize == MyCubeSize.Large) ? MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName).Small : MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName).Large);
			}
			StartBlockDefinition = CurrentBlockDefinition;
		}

		public void UpdateCurrentBlockToLastSelectedVariant()
		{
			MyBlockVariantGroup myBlockVariantGroup = CurrentBlockDefinition?.BlockVariantsGroup;
			if (myBlockVariantGroup != null && CurrentBlockDefinitionStages.Count != 0)
			{
				MyCubeBlockDefinition myCubeBlockDefinition = GetCurrentBlockForBlockVariantGroup(myBlockVariantGroup, respectRestrictions: true)[CurrentBlockDefinition.CubeSize];
				if (myCubeBlockDefinition != null && myCubeBlockDefinition.Public)
				{
					CurrentBlockDefinition = myCubeBlockDefinition;
				}
			}
		}

		/// <summary>
		/// Chooses same cube but for different grid size
		/// </summary>
		public void ChooseComplementBlock()
		{
			MyCubeBlockDefinitionWithVariants definitionWithVariants = m_definitionWithVariants;
			if (definitionWithVariants == null)
			{
				return;
			}
			MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(definitionWithVariants.Base.BlockPairName);
			if (definitionWithVariants.Base.CubeSize == MyCubeSize.Small)
			{
				if (definitionGroup.Large != null && (definitionGroup.Large.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS))
				{
					CurrentBlockDefinition = definitionGroup.Large;
				}
			}
			else if (definitionWithVariants.Base.CubeSize == MyCubeSize.Large && definitionGroup.Small != null && (definitionGroup.Small.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS))
			{
				CurrentBlockDefinition = definitionGroup.Small;
			}
		}

		/// <summary>
		/// Checks if there is complementary block available
		/// </summary>
		public bool HasComplementBlock()
		{
			if (m_definitionWithVariants != null)
			{
				MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(m_definitionWithVariants.Base.BlockPairName);
				if (m_definitionWithVariants.Base.CubeSize == MyCubeSize.Small)
				{
					if (definitionGroup.Large != null && (definitionGroup.Large.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS))
					{
						return true;
					}
				}
				else if (m_definitionWithVariants.Base.CubeSize == MyCubeSize.Large && definitionGroup.Small != null && (definitionGroup.Small.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Sets cube size mode.
		/// </summary>
		/// <param name="newCubeSize">New cube size mode.</param>
		public void SetCubeSize(MyCubeSize newCubeSize)
		{
			m_cubeSizeMode = newCubeSize;
			bool flag = true;
			if (CurrentBlockDefinitionStages.Count != 0)
			{
				MyCubeBlockDefinition currentBlockDefinition = CurrentBlockDefinition;
				object obj;
				if (currentBlockDefinition == null)
				{
					obj = null;
				}
				else
				{
					MyBlockVariantGroup blockVariantsGroup = currentBlockDefinition.BlockVariantsGroup;
					if (blockVariantsGroup == null)
					{
						obj = null;
					}
					else
					{
						MyCubeBlockDefinition[] blocks = blockVariantsGroup.Blocks;
						obj = ((blocks != null) ? Enumerable.FirstOrDefault<MyCubeBlockDefinition>((IEnumerable<MyCubeBlockDefinition>)blocks, (Func<MyCubeBlockDefinition, bool>)((MyCubeBlockDefinition x) => x.CubeSize == m_cubeSizeMode && x.BlockStages != null)) : null);
					}
				}
				MyCubeBlockDefinition myCubeBlockDefinition = (MyCubeBlockDefinition)obj;
				if (myCubeBlockDefinition != null)
				{
					flag = false;
					CurrentBlockDefinition = myCubeBlockDefinition;
					UpdateCurrentBlockToLastSelectedVariant();
				}
			}
			if (flag)
			{
				UpdateComplementBlock();
			}
			this.OnBlockSizeChanged?.Invoke(newCubeSize);
		}

		/// <summary>
		/// Updates Current block definition with current cube size mode.
		/// </summary>
		internal void UpdateComplementBlock()
		{
			_ = CurrentBlockDefinition;
			_ = StartBlockDefinition;
			if (CurrentBlockDefinition == null || StartBlockDefinition == null)
			{
				return;
			}
			MyCubeBlockDefinition myCubeBlockDefinition = MyDefinitionManager.Static.GetDefinitionGroup(CurrentBlockDefinition.BlockPairName)[m_cubeSizeMode];
			if (myCubeBlockDefinition == null)
			{
				myCubeBlockDefinition = MyDefinitionManager.Static.GetDefinitionGroup(StartBlockDefinition.BlockPairName)[m_cubeSizeMode];
			}
			if (myCubeBlockDefinition == null && CurrentBlockDefinitionStages.Count != 0)
			{
				MyBlockVariantGroup blockVariantsGroup = StartBlockDefinition.BlockVariantsGroup;
				if (blockVariantsGroup != null)
				{
					myCubeBlockDefinition = Enumerable.FirstOrDefault<MyCubeBlockDefinition>((IEnumerable<MyCubeBlockDefinition>)blockVariantsGroup.Blocks, (Func<MyCubeBlockDefinition, bool>)((MyCubeBlockDefinition x) => x.CubeSize == m_cubeSizeMode));
				}
			}
			CurrentBlockDefinition = myCubeBlockDefinition;
		}
	}
}
