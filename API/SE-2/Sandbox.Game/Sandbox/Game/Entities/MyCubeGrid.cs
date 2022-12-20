using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Havok;
using ParallelTasks;
using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.CoordinateSystem;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.Replication.ClientStates;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Compression;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.EntityComponents;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.GameServices;
using VRage.Groups;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Profiler;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageMath.PackedVector;
using VRageMath.Spatial;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Entities
{
	/// <summary>
	/// Grid - small ship, large ship, station
	/// Cubes (armor, walls...) are merge and rendered by this entity
	/// Blocks (turret, thrusts...) are rendered as child entities
	/// </summary>
	/// <summary>
	/// All logic related to grid merging
	/// </summary>
	/// <summary>
	/// Grid - small ship, large ship, station
	/// Cubes (armor, walls...) are merge and rendered by this entity
	/// Blocks (turret, thrusts...) are rendered as child entities
	/// </summary>
	[StaticEventOwner]
	[MyEntityType(typeof(MyObjectBuilder_CubeGrid), true)]
<<<<<<< HEAD
	public class MyCubeGrid : MyEntity, IMyGridConnectivityTest, IMyEventProxy, IMyEventOwner, IMySyncedEntity, IMyShootOrigin, IMyParallelUpdateable, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.Game.ModAPI.IMyCubeGrid, VRage.Game.ModAPI.Ingame.IMyCubeGrid
=======
	public class MyCubeGrid : MyEntity, IMyGridConnectivityTest, IMyEventProxy, IMyEventOwner, IMySyncedEntity, IMyParallelUpdateable, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.Game.ModAPI.IMyCubeGrid, VRage.Game.ModAPI.Ingame.IMyCubeGrid
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	{
		public enum HandbrakeToggleResult
		{
			RELEASED = 1,
			ENGAGED_SUCCESSFULLY,
			FAILED_TO_ENGAGE
		}

		public enum MyTestDisconnectsReason
		{
			NoReason,
			BlockRemoved,
			SplitBlock
		}

		internal enum MyTestDynamicReason
		{
			NoReason,
			GridCopied,
			GridSplit,
			GridSplitByBlock,
			ConvertToShip
		}

		private struct DeformationPostponedItem
		{
			public Vector3I Position;

			public Vector3I Min;

			public Vector3I Max;
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct MyBlockBuildArea
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EDefinitionId_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, DefinitionIdBlit>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in DefinitionIdBlit value)
				{
					owner.DefinitionId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out DefinitionIdBlit value)
				{
					value = owner.DefinitionId;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EColorMaskHSV_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, uint>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in uint value)
				{
					owner.ColorMaskHSV = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out uint value)
				{
					value = owner.ColorMaskHSV;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EPosInGrid_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in Vector3I value)
				{
					owner.PosInGrid = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out Vector3I value)
				{
					value = owner.PosInGrid;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EBlockMin_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, Vector3B>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in Vector3B value)
				{
					owner.BlockMin = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out Vector3B value)
				{
					value = owner.BlockMin;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EBlockMax_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, Vector3B>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in Vector3B value)
				{
					owner.BlockMax = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out Vector3B value)
				{
					value = owner.BlockMax;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EBuildAreaSize_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, Vector3UByte>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in Vector3UByte value)
				{
					owner.BuildAreaSize = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out Vector3UByte value)
				{
					value = owner.BuildAreaSize;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EStepDelta_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, Vector3B>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in Vector3B value)
				{
					owner.StepDelta = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out Vector3B value)
				{
					value = owner.StepDelta;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EOrientationForward_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, Base6Directions.Direction>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in Base6Directions.Direction value)
				{
					owner.OrientationForward = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out Base6Directions.Direction value)
				{
					value = owner.OrientationForward;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EOrientationUp_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, Base6Directions.Direction>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in Base6Directions.Direction value)
				{
					owner.OrientationUp = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out Base6Directions.Direction value)
				{
					value = owner.OrientationUp;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003ESkinId_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, MyStringHash>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in MyStringHash value)
				{
					owner.SkinId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out MyStringHash value)
				{
					value = owner.SkinId;
				}
			}

			public DefinitionIdBlit DefinitionId;

			public uint ColorMaskHSV;

			public Vector3I PosInGrid;

			public Vector3B BlockMin;

			public Vector3B BlockMax;

			public Vector3UByte BuildAreaSize;

			public Vector3B StepDelta;

			public Base6Directions.Direction OrientationForward;

			public Base6Directions.Direction OrientationUp;

			public MyStringHash SkinId;
		}

		[ProtoContract]
		public struct MyBlockLocation
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003EMin_003C_003EAccessor : IMemberAccessor<MyBlockLocation, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockLocation owner, in Vector3I value)
				{
					owner.Min = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockLocation owner, out Vector3I value)
				{
					value = owner.Min;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003EMax_003C_003EAccessor : IMemberAccessor<MyBlockLocation, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockLocation owner, in Vector3I value)
				{
					owner.Max = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockLocation owner, out Vector3I value)
				{
					value = owner.Max;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003ECenterPos_003C_003EAccessor : IMemberAccessor<MyBlockLocation, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockLocation owner, in Vector3I value)
				{
					owner.CenterPos = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockLocation owner, out Vector3I value)
				{
					value = owner.CenterPos;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003EOrientation_003C_003EAccessor : IMemberAccessor<MyBlockLocation, MyBlockOrientation>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockLocation owner, in MyBlockOrientation value)
				{
					owner.Orientation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockLocation owner, out MyBlockOrientation value)
				{
					value = owner.Orientation;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyBlockLocation, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockLocation owner, in long value)
				{
					owner.EntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockLocation owner, out long value)
				{
					value = owner.EntityId;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003EBlockDefinition_003C_003EAccessor : IMemberAccessor<MyBlockLocation, DefinitionIdBlit>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockLocation owner, in DefinitionIdBlit value)
				{
					owner.BlockDefinition = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockLocation owner, out DefinitionIdBlit value)
				{
					value = owner.BlockDefinition;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003EOwner_003C_003EAccessor : IMemberAccessor<MyBlockLocation, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockLocation owner, in long value)
				{
					owner.Owner = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockLocation owner, out long value)
				{
					value = owner.Owner;
				}
			}

			private class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003EActor : IActivator, IActivator<MyBlockLocation>
			{
				private sealed override object CreateInstance()
				{
					return default(MyBlockLocation);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyBlockLocation CreateInstance()
				{
					return (MyBlockLocation)(object)default(MyBlockLocation);
				}

				MyBlockLocation IActivator<MyBlockLocation>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public Vector3I Min;

			[ProtoMember(4)]
			public Vector3I Max;

			[ProtoMember(7)]
			public Vector3I CenterPos;

			[ProtoMember(10)]
			public MyBlockOrientation Orientation;

			[ProtoMember(13)]
			public long EntityId;

			[ProtoMember(16)]
			public DefinitionIdBlit BlockDefinition;

			[ProtoMember(19)]
			public long Owner;

			public MyBlockLocation(MyDefinitionId blockDefinition, Vector3I min, Vector3I max, Vector3I center, Quaternion orientation, long entityId, long owner)
			{
				BlockDefinition = blockDefinition;
				Min = min;
				Max = max;
				CenterPos = center;
				Orientation = new MyBlockOrientation(ref orientation);
				EntityId = entityId;
				Owner = owner;
			}
		}

		[ProtoContract]
		public struct BlockPositionId
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EBlockPositionId_003C_003EPosition_003C_003EAccessor : IMemberAccessor<BlockPositionId, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BlockPositionId owner, in Vector3I value)
				{
					owner.Position = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BlockPositionId owner, out Vector3I value)
				{
					value = owner.Position;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EBlockPositionId_003C_003ECompoundId_003C_003EAccessor : IMemberAccessor<BlockPositionId, uint>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BlockPositionId owner, in uint value)
				{
					owner.CompoundId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BlockPositionId owner, out uint value)
				{
					value = owner.CompoundId;
				}
			}

			private class Sandbox_Game_Entities_MyCubeGrid_003C_003EBlockPositionId_003C_003EActor : IActivator, IActivator<BlockPositionId>
			{
				private sealed override object CreateInstance()
				{
					return default(BlockPositionId);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override BlockPositionId CreateInstance()
				{
					return (BlockPositionId)(object)default(BlockPositionId);
				}

				BlockPositionId IActivator<BlockPositionId>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(22)]
			public Vector3I Position;

			[ProtoMember(25)]
			public uint CompoundId;
		}

		[ProtoContract]
		public struct MyBlockVisuals
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_003C_003EColorMaskHSV_003C_003EAccessor : IMemberAccessor<MyBlockVisuals, uint>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockVisuals owner, in uint value)
				{
					owner.ColorMaskHSV = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockVisuals owner, out uint value)
				{
					value = owner.ColorMaskHSV;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_003C_003ESkinId_003C_003EAccessor : IMemberAccessor<MyBlockVisuals, MyStringHash>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockVisuals owner, in MyStringHash value)
				{
					owner.SkinId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockVisuals owner, out MyStringHash value)
				{
					value = owner.SkinId;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_003C_003EApplyColor_003C_003EAccessor : IMemberAccessor<MyBlockVisuals, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockVisuals owner, in bool value)
				{
					owner.ApplyColor = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockVisuals owner, out bool value)
				{
					value = owner.ApplyColor;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_003C_003EApplySkin_003C_003EAccessor : IMemberAccessor<MyBlockVisuals, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockVisuals owner, in bool value)
				{
					owner.ApplySkin = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockVisuals owner, out bool value)
				{
					value = owner.ApplySkin;
				}
			}

			private class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_003C_003EActor : IActivator, IActivator<MyBlockVisuals>
			{
				private sealed override object CreateInstance()
				{
					return default(MyBlockVisuals);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyBlockVisuals CreateInstance()
				{
					return (MyBlockVisuals)(object)default(MyBlockVisuals);
				}

				MyBlockVisuals IActivator<MyBlockVisuals>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(28)]
			public uint ColorMaskHSV;

			[ProtoMember(31)]
			public MyStringHash SkinId;

			[ProtoMember(33)]
			public bool ApplyColor;

			[ProtoMember(35)]
			public bool ApplySkin;

			public MyBlockVisuals(uint colorMaskHsv, MyStringHash skinId, bool applyColor = true, bool applySkin = true)
			{
				ColorMaskHSV = colorMaskHsv;
				SkinId = skinId;
				ApplyColor = applyColor;
				ApplySkin = applySkin;
			}
		}

		private enum NeighborOffsetIndex
		{
			XUP,
			XDOWN,
			YUP,
			YDOWN,
			ZUP,
			ZDOWN,
			XUP_YUP,
			XUP_YDOWN,
			XDOWN_YUP,
			XDOWN_YDOWN,
			YUP_ZUP,
			YUP_ZDOWN,
			YDOWN_ZUP,
			YDOWN_ZDOWN,
			XUP_ZUP,
			XUP_ZDOWN,
			XDOWN_ZUP,
			XDOWN_ZDOWN,
			XUP_YUP_ZUP,
			XUP_YUP_ZDOWN,
			XUP_YDOWN_ZUP,
			XUP_YDOWN_ZDOWN,
			XDOWN_YUP_ZUP,
			XDOWN_YUP_ZDOWN,
			XDOWN_YDOWN_ZUP,
			XDOWN_YDOWN_ZDOWN
		}

		private struct MyNeighbourCachedBlock
		{
			public Vector3I Position;

			public MyCubeBlockDefinition BlockDefinition;

			public MyBlockOrientation Orientation;

			public override int GetHashCode()
			{
				return Position.GetHashCode();
			}
		}

		/// <summary>
		/// Used when calculating index of added block. Might not be count of
		/// blocks since removal of a block does not decrement this. Key is numerical
		/// ID of cube block definition.
		/// </summary>
		public class BlockTypeCounter
		{
			private Dictionary<MyDefinitionId, int> m_countById = new Dictionary<MyDefinitionId, int>(MyDefinitionId.Comparer);

			internal int GetNextNumber(MyDefinitionId blockType)
			{
				int value = 0;
				m_countById.TryGetValue(blockType, out value);
				value++;
				m_countById[blockType] = value;
				return value;
			}
		}

		private class PasteGridData : WorkData
		{
			private List<MyObjectBuilder_CubeGrid> m_gridObjectBuilders;

			private bool m_detectDisconnects;

			private Vector3 m_objectVelocity;

			private bool m_multiBlock;

			private bool m_instantBuild;

			private List<MyCubeGrid> m_pastedGrids;

			private bool m_canPlaceGrid;

			private List<VRage.ModAPI.IMyEntity> m_resultIDs;

			private bool m_removeScripts;

			public readonly EndpointId SenderEndpointId;

			public readonly bool IsLocallyInvoked;

			private List<ulong> m_clientsideDLCs;

			public Vector3D? m_offset;

			public PasteGridData(List<MyObjectBuilder_CubeGrid> entities, bool detectDisconnects, Vector3 objectVelocity, bool multiBlock, bool instantBuild, bool shouldRemoveScripts, EndpointId senderEndpointId, bool isLocallyInvoked, Vector3D? offset, List<ulong> clientsideDLCs = null)
			{
				m_gridObjectBuilders = new List<MyObjectBuilder_CubeGrid>(entities);
				m_detectDisconnects = detectDisconnects;
				m_objectVelocity = objectVelocity;
				m_multiBlock = multiBlock;
				m_instantBuild = instantBuild;
				SenderEndpointId = senderEndpointId;
				IsLocallyInvoked = isLocallyInvoked;
				m_removeScripts = shouldRemoveScripts;
				m_offset = offset;
				m_clientsideDLCs = clientsideDLCs;
			}

			public void TryPasteGrid()
			{
				bool flag = MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(SenderEndpointId.Value);
				if (MySession.Static.SurvivalMode && !flag)
				{
					return;
				}
				for (int i = 0; i < m_gridObjectBuilders.Count; i++)
				{
					m_gridObjectBuilders[i] = (MyObjectBuilder_CubeGrid)m_gridObjectBuilders[i].Clone();
				}
				MyEntities.RemapObjectBuilderCollection(m_gridObjectBuilders);
				MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
				MySessionComponentGameInventory component2 = MySession.Static.GetComponent<MySessionComponentGameInventory>();
				int num = -1;
				List<int> list = new List<int>();
				foreach (MyObjectBuilder_CubeGrid gridObjectBuilder in m_gridObjectBuilders)
				{
					num++;
					int num2 = 0;
					while (num2 < gridObjectBuilder.CubeBlocks.Count)
					{
						MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = gridObjectBuilder.CubeBlocks[num2];
						if (m_removeScripts)
						{
							MyObjectBuilder_MyProgrammableBlock myObjectBuilder_MyProgrammableBlock = myObjectBuilder_CubeBlock as MyObjectBuilder_MyProgrammableBlock;
							if (myObjectBuilder_MyProgrammableBlock != null)
							{
								myObjectBuilder_MyProgrammableBlock.Program = null;
							}
						}
						myObjectBuilder_CubeBlock.SkinSubtypeId = component2.ValidateArmor(MyStringHash.GetOrCompute(myObjectBuilder_CubeBlock.SkinSubtypeId), SenderEndpointId.Value).String;
						MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(new MyDefinitionId(myObjectBuilder_CubeBlock.TypeId, myObjectBuilder_CubeBlock.SubtypeId));
						bool num3 = component.HasDefinitionDLC(new MyDefinitionId(myObjectBuilder_CubeBlock.TypeId, myObjectBuilder_CubeBlock.SubtypeId), SenderEndpointId.Value);
						bool flag2 = component.ContainsRequiredDLC(definition, m_clientsideDLCs);
						if (!(num3 && flag2))
						{
							gridObjectBuilder.CubeBlocks.RemoveAt(num2);
						}
						else
						{
							num2++;
						}
					}
					if (gridObjectBuilder.CubeBlocks.Count == 0)
					{
						list.Add(num);
					}
				}
				if (list.Count > 0)
				{
					for (int num4 = list.Count - 1; num4 >= 0; num4--)
					{
						m_gridObjectBuilders.RemoveAt(list[num4]);
<<<<<<< HEAD
					}
				}
				if (m_gridObjectBuilders.Count == 0)
				{
					if (MyEventContext.Current.IsLocallyInvoked)
					{
						ShowMessageGridsRemovedWhilePastingInternal();
						return;
					}
=======
					}
				}
				if (m_gridObjectBuilders.Count == 0)
				{
					if (MyEventContext.Current.IsLocallyInvoked)
					{
						ShowMessageGridsRemovedWhilePastingInternal();
						return;
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowMessageGridsRemovedWhilePasting, SenderEndpointId);
					return;
				}
				m_pastedGrids = new List<MyCubeGrid>();
				MyEntityIdentifier.InEntityCreationBlock = true;
				MyEntityIdentifier.LazyInitPerThreadStorage(2048);
				m_canPlaceGrid = true;
				foreach (MyObjectBuilder_CubeGrid gridObjectBuilder2 in m_gridObjectBuilders)
				{
					MySandboxGame.Log.WriteLine("CreateCompressedMsg: Type: " + gridObjectBuilder2.GetType().Name.ToString() + "  Name: " + gridObjectBuilder2.Name + "  EntityID: " + gridObjectBuilder2.EntityId.ToString("X8"));
					MyCubeGrid myCubeGrid = MyEntities.CreateFromObjectBuilder(gridObjectBuilder2, fadeIn: false) as MyCubeGrid;
					if (myCubeGrid != null)
					{
						m_pastedGrids.Add(myCubeGrid);
						m_canPlaceGrid &= TestPastedGridPlacement(myCubeGrid, testPhysics: false);
						if (!m_canPlaceGrid)
						{
							break;
<<<<<<< HEAD
						}
						long inventoryEntityId = 0L;
						if (m_instantBuild && flag)
						{
							ChangeOwnership(inventoryEntityId, myCubeGrid);
						}
=======
						}
						long inventoryEntityId = 0L;
						if (m_instantBuild && flag)
						{
							ChangeOwnership(inventoryEntityId, myCubeGrid);
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						MySandboxGame.Log.WriteLine("Status: Exists(" + MyEntities.EntityExists(gridObjectBuilder2.EntityId) + ") InScene(" + ((gridObjectBuilder2.PersistentFlags & MyPersistentEntityFlags2.InScene) == MyPersistentEntityFlags2.InScene) + ")");
					}
				}
				m_resultIDs = new List<VRage.ModAPI.IMyEntity>();
				MyEntityIdentifier.GetPerThreadEntities(m_resultIDs);
				MyEntityIdentifier.ClearPerThreadEntities();
				MyEntityIdentifier.InEntityCreationBlock = false;
			}

			private bool TestPastedGridPlacement(MyCubeGrid grid, bool testPhysics)
			{
				MyGridPlacementSettings settings = MyClipboardComponent.ClipboardDefinition.PastingSettings.GetGridPlacementSettings(grid.GridSizeEnum, grid.IsStatic);
				return TestPlacementArea(grid, grid.IsStatic, ref settings, grid.PositionComp.LocalAABB, !grid.IsStatic, null, testVoxel: true, testPhysics);
			}

			public void Callback()
			{
				//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
				if (!IsLocallyInvoked)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SendHudNotificationAfterPaste, SenderEndpointId);
				}
				else if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					MyHud.PopRotatingWheelVisible();
				}
				if (m_canPlaceGrid)
				{
					foreach (MyCubeGrid pastedGrid in m_pastedGrids)
					{
						m_canPlaceGrid &= TestPastedGridPlacement(pastedGrid, testPhysics: true);
						if (!m_canPlaceGrid)
						{
							break;
						}
					}
				}
				if (m_offset.HasValue)
				{
					foreach (MyCubeGrid pastedGrid2 in m_pastedGrids)
					{
						MatrixD worldMatrix = pastedGrid2.WorldMatrix;
						worldMatrix.Translation += m_offset.Value;
						pastedGrid2.WorldMatrix = worldMatrix;
					}
				}
				if (m_canPlaceGrid && m_pastedGrids.Count > 0)
				{
					foreach (VRage.ModAPI.IMyEntity resultID in m_resultIDs)
					{
						MyEntityIdentifier.TryGetEntity(resultID.EntityId, out var entity);
						if (entity == null)
						{
							MyEntityIdentifier.AddEntityWithId(resultID);
						}
					}
					AfterPaste(m_pastedGrids, m_objectVelocity, m_detectDisconnects);
					return;
				}
				if (m_pastedGrids != null)
				{
					foreach (MyCubeGrid pastedGrid3 in m_pastedGrids)
					{
<<<<<<< HEAD
						foreach (MySlimBlock block in pastedGrid3.GetBlocks())
						{
							block.RemoveAuthorship();
						}
=======
						Enumerator<MySlimBlock> enumerator3 = pastedGrid3.GetBlocks().GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								enumerator3.get_Current().RemoveAuthorship();
							}
						}
						finally
						{
							((IDisposable)enumerator3).Dispose();
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						pastedGrid3.Close();
					}
				}
				if (!IsLocallyInvoked)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowPasteFailedOperation, SenderEndpointId);
				}
			}
		}

		[Serializable]
		public struct RelativeOffset
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003ERelativeOffset_003C_003EUse_003C_003EAccessor : IMemberAccessor<RelativeOffset, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RelativeOffset owner, in bool value)
				{
					owner.Use = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RelativeOffset owner, out bool value)
				{
					value = owner.Use;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003ERelativeOffset_003C_003ERelativeToEntity_003C_003EAccessor : IMemberAccessor<RelativeOffset, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RelativeOffset owner, in bool value)
				{
					owner.RelativeToEntity = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RelativeOffset owner, out bool value)
				{
					value = owner.RelativeToEntity;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003ERelativeOffset_003C_003ESpawnerId_003C_003EAccessor : IMemberAccessor<RelativeOffset, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RelativeOffset owner, in long value)
				{
					owner.SpawnerId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RelativeOffset owner, out long value)
				{
					value = owner.SpawnerId;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003ERelativeOffset_003C_003EOriginalSpawnPoint_003C_003EAccessor : IMemberAccessor<RelativeOffset, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RelativeOffset owner, in Vector3D value)
				{
					owner.OriginalSpawnPoint = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RelativeOffset owner, out Vector3D value)
				{
					value = owner.OriginalSpawnPoint;
				}
			}

			public bool Use;

			public bool RelativeToEntity;

			public long SpawnerId;

			public Vector3D OriginalSpawnPoint;
		}

		[Serializable]
		public struct MyPasteGridParameters
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyPasteGridParameters_003C_003EEntities_003C_003EAccessor : IMemberAccessor<MyPasteGridParameters, List<MyObjectBuilder_CubeGrid>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPasteGridParameters owner, in List<MyObjectBuilder_CubeGrid> value)
				{
					owner.Entities = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPasteGridParameters owner, out List<MyObjectBuilder_CubeGrid> value)
				{
					value = owner.Entities;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyPasteGridParameters_003C_003EClientsideDLCs_003C_003EAccessor : IMemberAccessor<MyPasteGridParameters, List<ulong>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPasteGridParameters owner, in List<ulong> value)
				{
					owner.ClientsideDLCs = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPasteGridParameters owner, out List<ulong> value)
				{
					value = owner.ClientsideDLCs;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyPasteGridParameters_003C_003EDetectDisconnects_003C_003EAccessor : IMemberAccessor<MyPasteGridParameters, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPasteGridParameters owner, in bool value)
				{
					owner.DetectDisconnects = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPasteGridParameters owner, out bool value)
				{
					value = owner.DetectDisconnects;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyPasteGridParameters_003C_003EMultiBlock_003C_003EAccessor : IMemberAccessor<MyPasteGridParameters, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPasteGridParameters owner, in bool value)
				{
					owner.MultiBlock = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPasteGridParameters owner, out bool value)
				{
					value = owner.MultiBlock;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyPasteGridParameters_003C_003EInstantBuild_003C_003EAccessor : IMemberAccessor<MyPasteGridParameters, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPasteGridParameters owner, in bool value)
				{
					owner.InstantBuild = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPasteGridParameters owner, out bool value)
				{
					value = owner.InstantBuild;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyPasteGridParameters_003C_003EObjectVelocity_003C_003EAccessor : IMemberAccessor<MyPasteGridParameters, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPasteGridParameters owner, in Vector3 value)
				{
					owner.ObjectVelocity = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPasteGridParameters owner, out Vector3 value)
				{
					value = owner.ObjectVelocity;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyPasteGridParameters_003C_003EOffset_003C_003EAccessor : IMemberAccessor<MyPasteGridParameters, RelativeOffset>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPasteGridParameters owner, in RelativeOffset value)
				{
					owner.Offset = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPasteGridParameters owner, out RelativeOffset value)
				{
					value = owner.Offset;
				}
			}

			[Serialize(MyObjectFlags.DefaultZero)]
			public List<MyObjectBuilder_CubeGrid> Entities;

			[Serialize(MyObjectFlags.DefaultZero)]
			public List<ulong> ClientsideDLCs;

			public bool DetectDisconnects;

			public bool MultiBlock;

			public bool InstantBuild;

			public Vector3 ObjectVelocity;

			public RelativeOffset Offset;

			public MyPasteGridParameters(List<MyObjectBuilder_CubeGrid> entities, bool detectDisconnects, bool multiBlock, Vector3 objectVelocity, bool instantBuild, RelativeOffset offset, List<ulong> clientsideDLCs)
			{
				Entities = entities;
				ClientsideDLCs = clientsideDLCs;
				DetectDisconnects = detectDisconnects;
				MultiBlock = multiBlock;
				InstantBuild = instantBuild;
				ObjectVelocity = objectVelocity;
				Offset = offset;
			}
		}

		[ProtoContract]
		public struct MySingleOwnershipRequest
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMySingleOwnershipRequest_003C_003EBlockId_003C_003EAccessor : IMemberAccessor<MySingleOwnershipRequest, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MySingleOwnershipRequest owner, in long value)
				{
					owner.BlockId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MySingleOwnershipRequest owner, out long value)
				{
					value = owner.BlockId;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMySingleOwnershipRequest_003C_003EOwner_003C_003EAccessor : IMemberAccessor<MySingleOwnershipRequest, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MySingleOwnershipRequest owner, in long value)
				{
					owner.Owner = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MySingleOwnershipRequest owner, out long value)
				{
					value = owner.Owner;
				}
			}

			private class Sandbox_Game_Entities_MyCubeGrid_003C_003EMySingleOwnershipRequest_003C_003EActor : IActivator, IActivator<MySingleOwnershipRequest>
			{
				private sealed override object CreateInstance()
				{
					return default(MySingleOwnershipRequest);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MySingleOwnershipRequest CreateInstance()
				{
					return (MySingleOwnershipRequest)(object)default(MySingleOwnershipRequest);
				}

				MySingleOwnershipRequest IActivator<MySingleOwnershipRequest>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(28)]
			public long BlockId;

			[ProtoMember(31)]
			public long Owner;
		}

		[ProtoContract]
		public struct LocationIdentity
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003ELocationIdentity_003C_003ELocation_003C_003EAccessor : IMemberAccessor<LocationIdentity, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref LocationIdentity owner, in Vector3I value)
				{
					owner.Location = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref LocationIdentity owner, out Vector3I value)
				{
					value = owner.Location;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003ELocationIdentity_003C_003EId_003C_003EAccessor : IMemberAccessor<LocationIdentity, ushort>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref LocationIdentity owner, in ushort value)
				{
					owner.Id = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref LocationIdentity owner, out ushort value)
				{
					value = owner.Id;
				}
			}

			private class Sandbox_Game_Entities_MyCubeGrid_003C_003ELocationIdentity_003C_003EActor : IActivator, IActivator<LocationIdentity>
			{
				private sealed override object CreateInstance()
				{
					return default(LocationIdentity);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override LocationIdentity CreateInstance()
				{
					return (LocationIdentity)(object)default(LocationIdentity);
				}

				LocationIdentity IActivator<LocationIdentity>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(34)]
			public Vector3I Location;

			[ProtoMember(37)]
			public ushort Id;
		}

		public class MyCubeGridHitInfo
		{
			public MyIntersectionResultLineTriangleEx Triangle;

			public Vector3I Position;

			public int CubePartIndex = -1;

			public void Reset()
			{
				Triangle = default(MyIntersectionResultLineTriangleEx);
				Position = default(Vector3I);
				CubePartIndex = -1;
			}
		}

		private class AreaConnectivityTest : IMyGridConnectivityTest
		{
			private readonly Dictionary<Vector3I, Vector3I> m_lookup = new Dictionary<Vector3I, Vector3I>();

			private MyBlockOrientation m_orientation;

			private MyCubeBlockDefinition m_definition;

			private Vector3I m_posInGrid;

			private Vector3I m_blockMin;

			private Vector3I m_blockMax;

			private Vector3I m_stepDelta;

			public void Initialize(ref MyBlockBuildArea area, MyCubeBlockDefinition definition)
			{
				m_definition = definition;
				m_orientation = new MyBlockOrientation(area.OrientationForward, area.OrientationUp);
				m_posInGrid = area.PosInGrid;
				m_blockMin = area.BlockMin;
				m_blockMax = area.BlockMax;
				m_stepDelta = area.StepDelta;
				m_lookup.Clear();
			}

			public void AddBlock(Vector3UByte offset)
			{
				Vector3I vector3I = m_posInGrid + offset * m_stepDelta;
				Vector3I vector3I2 = default(Vector3I);
				vector3I2.X = m_blockMin.X;
				while (vector3I2.X <= m_blockMax.X)
				{
					vector3I2.Y = m_blockMin.Y;
					while (vector3I2.Y <= m_blockMax.Y)
					{
						vector3I2.Z = m_blockMin.Z;
						while (vector3I2.Z <= m_blockMax.Z)
						{
							m_lookup.Add(vector3I + vector3I2, vector3I);
							vector3I2.Z++;
						}
						vector3I2.Y++;
					}
					vector3I2.X++;
				}
			}

			public void GetConnectedBlocks(Vector3I minI, Vector3I maxI, Dictionary<Vector3I, ConnectivityResult> outOverlappedCubeBlocks)
			{
				Vector3I key = default(Vector3I);
				key.X = minI.X;
				while (key.X <= maxI.X)
				{
					key.Y = minI.Y;
					while (key.Y <= maxI.Y)
					{
						key.Z = minI.Z;
						while (key.Z <= maxI.Z)
						{
							if (m_lookup.TryGetValue(key, out var value) && !outOverlappedCubeBlocks.ContainsKey(value))
							{
								outOverlappedCubeBlocks.Add(value, new ConnectivityResult
								{
									Definition = m_definition,
									FatBlock = null,
									Position = value,
									Orientation = m_orientation
								});
							}
							key.Z++;
						}
						key.Y++;
					}
					key.X++;
				}
			}
		}

		private struct TriangleWithMaterial
		{
			public MyTriangleVertexIndices triangle;

			public MyTriangleVertexIndices uvIndices;

			public string material;
		}

		public enum UpdateQueue : byte
		{
			Invalid = 0,
			BeforeSimulation = 1,
			OnceBeforeSimulation = 2,
			AfterSimulation = 3,
			OnceAfterSimulation = 4,
			QueueCount = 4
		}

		[DebuggerDisplay("{DebuggerDisplay}")]
		internal struct Update
		{
			public Action Callback;
<<<<<<< HEAD

			public int Priority;

			public bool Parallel;

			public static readonly Update Empty = new Update(null, int.MaxValue, parallel: false);

			public bool Removed => Callback == null;

			private string DebuggerDisplay
			{
				get
				{
					if (!Removed)
					{
						return string.Format("{0} ({1}) {2}", DebugFormatMethodName(Callback.Method), Priority, Parallel ? "P" : "");
					}
					return "Removed";
				}
			}

			public Update(Action callback, int priority, bool parallel)
			{
				Callback = callback;
				Priority = priority;
				Parallel = parallel;
			}

			public void SetRemoved()
			{
				Callback = null;
=======

			public int Priority;

			public bool Parallel;

			public static readonly Update Empty = new Update(null, int.MaxValue, parallel: false);

			public bool Removed => Callback == null;

			private string DebuggerDisplay
			{
				get
				{
					if (!Removed)
					{
						return string.Format("{0} ({1}) {2}", DebugFormatMethodName(Callback.Method), Priority, Parallel ? "P" : "");
					}
					return "Removed";
				}
			}

			public Update(Action callback, int priority, bool parallel)
			{
				Callback = callback;
				Priority = priority;
				Parallel = parallel;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

<<<<<<< HEAD
		private struct QueuedUpdateChange
		{
			public Action Callback;

			public int Priority;

			public UpdateQueue Queue;

			public MyCubeGrid Grid;

			public bool Parallel;

			public bool Add;

			public static QueuedUpdateChange MakeAdd(Action callback, int priority, UpdateQueue queue, MyCubeGrid grid, bool parallel)
			{
				QueuedUpdateChange result = default(QueuedUpdateChange);
				result.Callback = callback;
				result.Priority = priority;
				result.Queue = queue;
				result.Grid = grid;
				result.Parallel = parallel;
				result.Add = true;
				return result;
=======
			public void SetRemoved()
			{
				Callback = null;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

<<<<<<< HEAD
			public static QueuedUpdateChange MakeRemove(Action callback, UpdateQueue queue, MyCubeGrid grid)
			{
				QueuedUpdateChange result = default(QueuedUpdateChange);
				result.Callback = callback;
				result.Queue = queue;
				result.Grid = grid;
				result.Add = false;
=======
		private struct QueuedUpdateChange
		{
			public Action Callback;

			public int Priority;

			public UpdateQueue Queue;

			public MyCubeGrid Grid;

			public bool Parallel;

			public bool Add;

			public static QueuedUpdateChange MakeAdd(Action callback, int priority, UpdateQueue queue, MyCubeGrid grid, bool parallel)
			{
				QueuedUpdateChange result = default(QueuedUpdateChange);
				result.Callback = callback;
				result.Priority = priority;
				result.Queue = queue;
				result.Grid = grid;
				result.Parallel = parallel;
				result.Add = true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return result;
			}

<<<<<<< HEAD
		public struct DebugUpdateRecord
		{
			private sealed class MethodQueueEqualityComparer : IEqualityComparer<DebugUpdateRecord>
			{
				public bool Equals(DebugUpdateRecord x, DebugUpdateRecord y)
				{
					if (object.Equals(x.Method, y.Method))
					{
						return x.Queue == y.Queue;
					}
					return false;
				}

				public int GetHashCode(DebugUpdateRecord obj)
				{
					return (((obj.Method != null) ? obj.Method.GetHashCode() : 0) * 397) ^ (int)obj.Queue;
				}
			}

			public MethodInfo Method;

			public UpdateQueue Queue;

			public int Priority;

			public static IEqualityComparer<DebugUpdateRecord> Comparer { get; } = new MethodQueueEqualityComparer();


			public DebugUpdateRecord(MethodInfo method, UpdateQueue queue, int priority)
			{
				Method = method;
				Queue = queue;
				Priority = priority;
			}

			internal DebugUpdateRecord(in Update update, UpdateQueue queue)
			{
				Method = update.Callback.Method;
				Priority = update.Priority;
				Queue = queue;
			}

			/// <inheritdoc />
			public override string ToString()
			{
				return $"{Queue}: {DebugFormatMethodName(Method)} ({Priority})";
			}
		}

		public struct DebugUpdateStats
		{
			public Queue<int> Calls;

			public int LastFrame => Calls.First();

			public DebugUpdateStats(int frame)
			{
				Calls = new Queue<int>();
				Calls.Enqueue(frame);
			}

			/// <inheritdoc />
			public override string ToString()
			{
				return $"{LastFrame}, {Calls.Count}";
			}
		}

		protected sealed class OnGridChangedRPC_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnGridChangedRPC();
			}
		}

		protected sealed class CreateSplit_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Int64 : ICallSite<MyCubeGrid, List<Vector3I>, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> blocks, in long newEntityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateSplit_Implementation(blocks, newEntityId);
			}
		}

		protected sealed class CreateSplits_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_Cube_MyDisconnectHelper_003C_003EGroup_003E : ICallSite<MyCubeGrid, List<Vector3I>, List<MyDisconnectHelper.Group>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> blocks, in List<MyDisconnectHelper.Group> groups, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateSplits_Implementation(blocks, groups);
			}
		}

		protected sealed class ShowMessageGridsRemovedWhilePasting_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowMessageGridsRemovedWhilePasting();
			}
		}

		protected sealed class RemovedBlocks_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E : ICallSite<MyCubeGrid, List<Vector3I>, List<Vector3I>, List<Vector3I>, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> destroyLocations, in List<Vector3I> DestructionDeformationLocation, in List<Vector3I> LocationsWithoutGenerator, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RemovedBlocks(destroyLocations, DestructionDeformationLocation, LocationsWithoutGenerator);
			}
		}

		protected sealed class RemovedBlocksWithIds_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EBlockPositionId_003E_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EBlockPositionId_003E : ICallSite<MyCubeGrid, List<BlockPositionId>, List<BlockPositionId>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<BlockPositionId> destroyBlockWithIdQueueWithoutGenerators, in List<BlockPositionId> removeBlockWithIdQueueWithoutGenerators, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RemovedBlocksWithIds(destroyBlockWithIdQueueWithoutGenerators, removeBlockWithIdQueueWithoutGenerators);
			}
		}

		protected sealed class RemoveBlocksBuiltByID_003C_003ESystem_Int64 : ICallSite<MyCubeGrid, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long identityID, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RemoveBlocksBuiltByID(identityID);
			}
		}

		protected sealed class TransferBlocksBuiltByID_003C_003ESystem_Int64_0023System_Int64 : ICallSite<MyCubeGrid, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long oldAuthor, in long newAuthor, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.TransferBlocksBuiltByID(oldAuthor, newAuthor);
=======
			public static QueuedUpdateChange MakeRemove(Action callback, UpdateQueue queue, MyCubeGrid grid)
			{
				QueuedUpdateChange result = default(QueuedUpdateChange);
				result.Callback = callback;
				result.Queue = queue;
				result.Grid = grid;
				result.Add = false;
				return result;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public struct DebugUpdateRecord
		{
			private sealed class MethodQueueEqualityComparer : IEqualityComparer<DebugUpdateRecord>
			{
				public bool Equals(DebugUpdateRecord x, DebugUpdateRecord y)
				{
					if (object.Equals(x.Method, y.Method))
					{
						return x.Queue == y.Queue;
					}
					return false;
				}

				public int GetHashCode(DebugUpdateRecord obj)
				{
					return (((obj.Method != null) ? obj.Method.GetHashCode() : 0) * 397) ^ (int)obj.Queue;
				}
			}

			public MethodInfo Method;

			public UpdateQueue Queue;

			public int Priority;

			public static IEqualityComparer<DebugUpdateRecord> Comparer { get; } = new MethodQueueEqualityComparer();


			public DebugUpdateRecord(MethodInfo method, UpdateQueue queue, int priority)
			{
				Method = method;
				Queue = queue;
				Priority = priority;
			}

			internal DebugUpdateRecord(in Update update, UpdateQueue queue)
			{
				Method = update.Callback.Method;
				Priority = update.Priority;
				Queue = queue;
			}

			public override string ToString()
			{
				return $"{Queue}: {DebugFormatMethodName(Method)} ({Priority})";
			}
		}

		public struct DebugUpdateStats
		{
			public Queue<int> Calls;

			public int LastFrame => Enumerable.First<int>((IEnumerable<int>)Calls);

			public DebugUpdateStats(int frame)
			{
				Calls = new Queue<int>();
				Calls.Enqueue(frame);
			}

			public override string ToString()
			{
				return $"{LastFrame}, {Calls.get_Count()}";
			}
		}

		protected sealed class OnGridChangedRPC_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnGridChangedRPC();
			}
		}

		protected sealed class CreateSplit_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Int64 : ICallSite<MyCubeGrid, List<Vector3I>, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> blocks, in long newEntityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateSplit_Implementation(blocks, newEntityId);
			}
		}

		protected sealed class CreateSplits_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_Cube_MyDisconnectHelper_003C_003EGroup_003E : ICallSite<MyCubeGrid, List<Vector3I>, List<MyDisconnectHelper.Group>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> blocks, in List<MyDisconnectHelper.Group> groups, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateSplits_Implementation(blocks, groups);
			}
		}

		protected sealed class ShowMessageGridsRemovedWhilePasting_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowMessageGridsRemovedWhilePasting();
			}
		}

		protected sealed class RemovedBlocks_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E : ICallSite<MyCubeGrid, List<Vector3I>, List<Vector3I>, List<Vector3I>, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> destroyLocations, in List<Vector3I> DestructionDeformationLocation, in List<Vector3I> LocationsWithoutGenerator, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RemovedBlocks(destroyLocations, DestructionDeformationLocation, LocationsWithoutGenerator);
			}
		}

		protected sealed class RemovedBlocksWithIds_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EBlockPositionId_003E_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EBlockPositionId_003E : ICallSite<MyCubeGrid, List<BlockPositionId>, List<BlockPositionId>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<BlockPositionId> destroyBlockWithIdQueueWithoutGenerators, in List<BlockPositionId> removeBlockWithIdQueueWithoutGenerators, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RemovedBlocksWithIds(destroyBlockWithIdQueueWithoutGenerators, removeBlockWithIdQueueWithoutGenerators);
			}
		}

		protected sealed class RemoveBlocksBuiltByID_003C_003ESystem_Int64 : ICallSite<MyCubeGrid, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long identityID, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RemoveBlocksBuiltByID(identityID);
			}
		}

		protected sealed class TransferBlocksBuiltByID_003C_003ESystem_Int64_0023System_Int64 : ICallSite<MyCubeGrid, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long oldAuthor, in long newAuthor, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.TransferBlocksBuiltByID(oldAuthor, newAuthor);
			}
		}

		protected sealed class BuildBlockRequest_003C_003ESystem_UInt32_0023Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_0023VRage_Game_MyObjectBuilder_CubeBlock_0023System_Int64_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, uint, MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in uint colorMaskHsv, in MyBlockLocation location, in MyObjectBuilder_CubeBlock blockObjectBuilder, in long builderEntityId, in bool instantBuild, in long ownerId)
			{
				@this.BuildBlockRequest(colorMaskHsv, location, blockObjectBuilder, builderEntityId, instantBuild, ownerId);
			}
		}

		protected sealed class BuildBlockRequest_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_0023VRage_Game_MyObjectBuilder_CubeBlock_0023System_Int64_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, MyBlockVisuals, MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockVisuals visuals, in MyBlockLocation location, in MyObjectBuilder_CubeBlock blockObjectBuilder, in long builderEntityId, in bool instantBuild, in long ownerId)
			{
				@this.BuildBlockRequest(visuals, location, blockObjectBuilder, builderEntityId, instantBuild, ownerId);
			}
		}

		protected sealed class BuildBlockSucess_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_0023VRage_Game_MyObjectBuilder_CubeBlock_0023System_Int64_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, MyBlockVisuals, MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockVisuals visuals, in MyBlockLocation location, in MyObjectBuilder_CubeBlock blockObjectBuilder, in long builderEntityId, in bool instantBuild, in long ownerId)
			{
				@this.BuildBlockSucess(visuals, location, blockObjectBuilder, builderEntityId, instantBuild, ownerId);
			}
		}

		protected sealed class BuildBlocksRequest_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023System_Collections_Generic_HashSet_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003E_0023System_Int64_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, MyBlockVisuals, HashSet<MyBlockLocation>, long, bool, long, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockVisuals visuals, in HashSet<MyBlockLocation> locations, in long builderEntityId, in bool instantBuild, in long ownerId, in DBNull arg6)
			{
				@this.BuildBlocksRequest(visuals, locations, builderEntityId, instantBuild, ownerId);
			}
		}

		protected sealed class BuildBlocksFailedNotify_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.BuildBlocksFailedNotify();
			}
		}

		protected sealed class BuildBlocksClient_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023System_Collections_Generic_HashSet_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003E_0023System_Int64_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, MyBlockVisuals, HashSet<MyBlockLocation>, long, bool, long, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockVisuals visuals, in HashSet<MyBlockLocation> locations, in long builderEntityId, in bool instantBuild, in long ownerId, in DBNull arg6)
			{
				@this.BuildBlocksClient(visuals, locations, builderEntityId, instantBuild, ownerId);
			}
		}

		protected sealed class BuildBlocksAreaRequest_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_0023System_Int64_0023System_Boolean_0023System_Int64_0023System_UInt64_0023System_String : ICallSite<MyCubeGrid, MyBlockBuildArea, long, bool, long, ulong, string>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockBuildArea area, in long builderEntityId, in bool instantBuild, in long ownerId, in ulong placingPlayer, in string localizedDisplayNameBase)
			{
				@this.BuildBlocksAreaRequest(area, builderEntityId, instantBuild, ownerId, placingPlayer, localizedDisplayNameBase);
			}
		}

		protected sealed class BuildBlocksAreaClient_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_0023System_Int32_0023System_Collections_Generic_HashSet_00601_003CVRageMath_Vector3UByte_003E_0023System_Int64_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, MyBlockBuildArea, int, HashSet<Vector3UByte>, long, bool, long>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockBuildArea area, in int entityIdSeed, in HashSet<Vector3UByte> failList, in long builderEntityId, in bool isAdmin, in long ownerId)
			{
				@this.BuildBlocksAreaClient(area, entityIdSeed, failList, builderEntityId, isAdmin, ownerId);
			}
		}

		protected sealed class RazeBlocksAreaRequest_003C_003EVRageMath_Vector3I_0023VRageMath_Vector3UByte_0023System_Int64_0023System_UInt64 : ICallSite<MyCubeGrid, Vector3I, Vector3UByte, long, ulong, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I pos, in Vector3UByte size, in long builderEntityId, in ulong placingPlayer, in DBNull arg5, in DBNull arg6)
			{
				@this.RazeBlocksAreaRequest(pos, size, builderEntityId, placingPlayer);
			}
		}

		protected sealed class RazeBlocksAreaSuccess_003C_003EVRageMath_Vector3I_0023VRageMath_Vector3UByte_0023System_Collections_Generic_HashSet_00601_003CVRageMath_Vector3UByte_003E : ICallSite<MyCubeGrid, Vector3I, Vector3UByte, HashSet<Vector3UByte>, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I pos, in Vector3UByte size, in HashSet<Vector3UByte> resultFailList, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RazeBlocksAreaSuccess(pos, size, resultFailList);
			}
		}

		protected sealed class RazeBlocksRequest_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Int64_0023System_UInt64 : ICallSite<MyCubeGrid, List<Vector3I>, long, ulong, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> locations, in long builderEntityId, in ulong user, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RazeBlocksRequest(locations, builderEntityId, user);
			}
		}

		protected sealed class RazeBlocksClient_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E : ICallSite<MyCubeGrid, List<Vector3I>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> locations, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RazeBlocksClient(locations);
			}
		}

		protected sealed class ColorGridFriendlyRequest_003C_003EVRageMath_Vector3_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, Vector3, bool, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3 newHSV, in bool playSound, in long player, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ColorGridFriendlyRequest(newHSV, playSound, player);
			}
		}

		protected sealed class OnColorGridFriendly_003C_003EVRageMath_Vector3_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, Vector3, bool, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3 newHSV, in bool playSound, in long player, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnColorGridFriendly(newHSV, playSound, player);
			}
		}

		protected sealed class OnColorGridBlockFailed_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnColorGridBlockFailed();
			}
		}

		protected sealed class ColorBlockRequest_003C_003EVRageMath_Vector3I_0023VRageMath_Vector3I_0023VRageMath_Vector3_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, Vector3I, Vector3, bool, long, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I min, in Vector3I max, in Vector3 newHSV, in bool playSound, in long player, in DBNull arg6)
			{
				@this.ColorBlockRequest(min, max, newHSV, playSound, player);
			}
		}

		protected sealed class OnColorBlock_003C_003EVRageMath_Vector3I_0023VRageMath_Vector3I_0023VRageMath_Vector3_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, Vector3I, Vector3, bool, long, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I min, in Vector3I max, in Vector3 newHSV, in bool playSound, in long player, in DBNull arg6)
			{
				@this.OnColorBlock(min, max, newHSV, playSound, player);
			}
		}

		protected sealed class SkinGridFriendlyRequest_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, MyBlockVisuals, bool, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockVisuals visuals, in bool playSound, in long player, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SkinGridFriendlyRequest(visuals, playSound, player);
			}
		}

		protected sealed class OnSkinGridFriendly_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, MyBlockVisuals, bool, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockVisuals visuals, in bool playSound, in long player, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSkinGridFriendly(visuals, playSound, player);
			}
		}

		protected sealed class SkinBlockRequest_003C_003EVRageMath_Vector3I_0023VRageMath_Vector3I_0023Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, Vector3I, MyBlockVisuals, bool, long, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I min, in Vector3I max, in MyBlockVisuals visuals, in bool playSound, in long player, in DBNull arg6)
			{
				@this.SkinBlockRequest(min, max, visuals, playSound, player);
			}
		}

		protected sealed class OnSkinBlock_003C_003EVRageMath_Vector3I_0023VRageMath_Vector3I_0023Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, Vector3I, MyBlockVisuals, bool, long, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I min, in Vector3I max, in MyBlockVisuals visuals, in bool playSound, in long player, in DBNull arg6)
			{
				@this.OnSkinBlock(min, max, visuals, playSound, player);
			}
		}

		protected sealed class OnConvertToDynamic_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnConvertToDynamic();
			}
		}

		protected sealed class ConvertToStatic_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ConvertToStatic();
			}
		}

		protected sealed class BlockIntegrityChanged_003C_003EVRageMath_Vector3I_0023System_UInt16_0023System_Single_0023System_Single_0023VRage_Game_ModAPI_MyIntegrityChangeEnum_0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, ushort, float, float, MyIntegrityChangeEnum, long>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I pos, in ushort subBlockId, in float buildIntegrity, in float integrity, in MyIntegrityChangeEnum integrityChangeType, in long grinderOwner)
			{
				@this.BlockIntegrityChanged(pos, subBlockId, buildIntegrity, integrity, integrityChangeType, grinderOwner);
			}
		}

		protected sealed class BlockStockpileChanged_003C_003EVRageMath_Vector3I_0023System_UInt16_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyStockpileItem_003E : ICallSite<MyCubeGrid, Vector3I, ushort, List<MyStockpileItem>, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I pos, in ushort subBlockId, in List<MyStockpileItem> items, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.BlockStockpileChanged(pos, subBlockId, items);
			}
		}

		protected sealed class FractureComponentRepaired_003C_003EVRageMath_Vector3I_0023System_UInt16_0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, ushort, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I pos, in ushort subBlockId, in long toolOwner, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.FractureComponentRepaired(pos, subBlockId, toolOwner);
			}
		}

		protected sealed class PasteBlocksToGridServer_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CVRage_Game_MyObjectBuilder_CubeGrid_003E_0023System_Int64_0023System_Boolean_0023System_Boolean_0023System_Collections_Generic_List_00601_003CSystem_UInt64_003E : ICallSite<MyCubeGrid, List<MyObjectBuilder_CubeGrid>, long, bool, bool, List<ulong>, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<MyObjectBuilder_CubeGrid> gridsToMerge, in long inventoryEntityId, in bool multiBlock, in bool instantBuild, in List<ulong> clientDLCIDs, in DBNull arg6)
			{
				@this.PasteBlocksToGridServer_Implementation(gridsToMerge, inventoryEntityId, multiBlock, instantBuild, clientDLCIDs);
			}
		}

		protected sealed class PasteBlocksToGridClient_Implementation_003C_003EVRage_Game_MyObjectBuilder_CubeGrid_0023VRageMath_MatrixI : ICallSite<MyCubeGrid, MyObjectBuilder_CubeGrid, MatrixI, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyObjectBuilder_CubeGrid gridToMerge, in MatrixI mergeTransform, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.PasteBlocksToGridClient_Implementation(gridToMerge, mergeTransform);
			}
		}

		protected sealed class TryCreateGrid_Implementation_003C_003EVRage_Game_MyCubeSize_0023System_Boolean_0023VRage_MyPositionAndOrientation_0023System_Int64_0023System_Boolean : ICallSite<IMyEventOwner, MyCubeSize, bool, MyPositionAndOrientation, long, bool, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyCubeSize cubeSize, in bool isStatic, in MyPositionAndOrientation position, in long inventoryEntityId, in bool instantBuild, in DBNull arg6)
			{
				TryCreateGrid_Implementation(cubeSize, isStatic, position, inventoryEntityId, instantBuild);
			}
		}

		protected sealed class StationClosingDenied_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				StationClosingDenied();
			}
		}

		protected sealed class OnGridClosedRequest_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnGridClosedRequest(entityId);
			}
		}

		protected sealed class TryPasteGrid_Implementation_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyPasteGridParameters : ICallSite<IMyEventOwner, MyPasteGridParameters, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyPasteGridParameters parameters, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				TryPasteGrid_Implementation(parameters);
			}
		}

		protected sealed class ShowPasteFailedOperation_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowPasteFailedOperation();
			}
		}

		protected sealed class SendHudNotificationAfterPaste_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SendHudNotificationAfterPaste();
			}
		}

		protected sealed class OnBonesReceived_003C_003ESystem_Int32_0023System_Collections_Generic_List_00601_003CSystem_Byte_003E : ICallSite<MyCubeGrid, int, List<byte>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in int segmentsCount, in List<byte> boneByteList, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnBonesReceived(segmentsCount, boneByteList);
			}
		}

		protected sealed class OnBonesMultiplied_003C_003EVRageMath_Vector3I_0023System_Single : ICallSite<MyCubeGrid, Vector3I, float, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I blockLocation, in float multiplier, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnBonesMultiplied(blockLocation, multiplier);
			}
		}

		protected sealed class RelfectorStateRecived_003C_003EVRage_MyMultipleEnabledEnum : ICallSite<MyCubeGrid, MyMultipleEnabledEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyMultipleEnabledEnum value, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RelfectorStateRecived(value);
			}
		}

		protected sealed class OnStockpileFillRequest_003C_003EVRageMath_Vector3I_0023System_Int64_0023System_Byte : ICallSite<MyCubeGrid, Vector3I, long, byte, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I blockPosition, in long ownerEntityId, in byte inventoryIndex, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnStockpileFillRequest(blockPosition, ownerEntityId, inventoryIndex);
			}
		}

		protected sealed class OnSetToConstructionRequest_003C_003EVRageMath_Vector3I_0023System_Int64_0023System_Byte_0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, long, byte, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I blockPosition, in long ownerEntityId, in byte inventoryIndex, in long requestingPlayer, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSetToConstructionRequest(blockPosition, ownerEntityId, inventoryIndex, requestingPlayer);
			}
		}

		protected sealed class OnPowerProducerStateRequest_003C_003EVRage_MyMultipleEnabledEnum_0023System_Int64_0023System_Boolean : ICallSite<MyCubeGrid, MyMultipleEnabledEnum, long, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyMultipleEnabledEnum enabledState, in long playerId, in bool localGridOnly, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnPowerProducerStateRequest(enabledState, playerId, localGridOnly);
			}
		}

		protected sealed class OnConvertedToShipRequest_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyTestDynamicReason : ICallSite<MyCubeGrid, MyTestDynamicReason, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyTestDynamicReason reason, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnConvertedToShipRequest(reason);
			}
		}

		protected sealed class OnConvertToShipFailed_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnConvertToShipFailed();
			}
		}

		protected sealed class OnConvertedToStationRequest_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnConvertedToStationRequest();
			}
		}

		protected sealed class OnChangeOwnerRequest_003C_003ESystem_Int64_0023System_Int64_0023VRage_Game_MyOwnershipShareModeEnum : ICallSite<MyCubeGrid, long, long, MyOwnershipShareModeEnum, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long blockId, in long owner, in MyOwnershipShareModeEnum shareMode, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOwnerRequest(blockId, owner, shareMode);
			}
		}

		protected sealed class OnChangeOwner_003C_003ESystem_Int64_0023System_Int64_0023VRage_Game_MyOwnershipShareModeEnum : ICallSite<MyCubeGrid, long, long, MyOwnershipShareModeEnum, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long blockId, in long owner, in MyOwnershipShareModeEnum shareMode, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOwner(blockId, owner, shareMode);
			}
		}

		protected sealed class ToggleHandbrakeRequest_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ToggleHandbrakeRequest();
			}
		}

		protected sealed class ReceiveHandbrakeRequestResult_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EHandbrakeToggleResult_0023System_String : ICallSite<MyCubeGrid, HandbrakeToggleResult, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in HandbrakeToggleResult result, in string message, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveHandbrakeRequestResult(result, message);
			}
		}

		protected sealed class ToggleParkStateRequest_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ToggleParkStateRequest();
			}
		}

		protected sealed class ReceiveParkRequestResult_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EHandbrakeToggleResult_0023System_String : ICallSite<MyCubeGrid, HandbrakeToggleResult, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in HandbrakeToggleResult result, in string message, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveParkRequestResult(result, message);
			}
		}

		protected sealed class OnChangeGridOwner_003C_003ESystem_Int64_0023VRage_Game_MyOwnershipShareModeEnum : ICallSite<MyCubeGrid, long, MyOwnershipShareModeEnum, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long playerId, in MyOwnershipShareModeEnum shareMode, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeGridOwner(playerId, shareMode);
			}
		}

		protected sealed class OnRemoveSplit_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E : ICallSite<MyCubeGrid, List<Vector3I>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> removedBlocks, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveSplit(removedBlocks);
			}
		}

		protected sealed class OnChangeDisplayNameRequest_003C_003ESystem_String : ICallSite<MyCubeGrid, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in string displayName, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeDisplayNameRequest(displayName);
			}
		}

		protected sealed class OnModifyGroupSuccess_003C_003ESystem_String_0023System_Collections_Generic_List_00601_003CSystem_Int64_003E : ICallSite<MyCubeGrid, string, List<long>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in string name, in List<long> blocks, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnModifyGroupSuccess(name, blocks);
			}
		}

		protected sealed class OnRazeBlockInCompoundBlockRequest_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003ELocationIdentity_003E : ICallSite<MyCubeGrid, List<LocationIdentity>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<LocationIdentity> locationsAndIds, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRazeBlockInCompoundBlockRequest(locationsAndIds);
			}
		}

		protected sealed class OnRazeBlockInCompoundBlockSuccess_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003ELocationIdentity_003E : ICallSite<MyCubeGrid, List<LocationIdentity>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<LocationIdentity> locationsAndIds, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRazeBlockInCompoundBlockSuccess(locationsAndIds);
			}
		}

		protected sealed class OnChangeOwnersRequest_003C_003EVRage_Game_MyOwnershipShareModeEnum_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EMySingleOwnershipRequest_003E_0023System_Int64 : ICallSite<IMyEventOwner, MyOwnershipShareModeEnum, List<MySingleOwnershipRequest>, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyOwnershipShareModeEnum shareMode, in List<MySingleOwnershipRequest> requests, in long requestingPlayer, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnChangeOwnersRequest(shareMode, requests, requestingPlayer);
			}
		}

		protected sealed class OnChangeOwnersSuccess_003C_003EVRage_Game_MyOwnershipShareModeEnum_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EMySingleOwnershipRequest_003E : ICallSite<IMyEventOwner, MyOwnershipShareModeEnum, List<MySingleOwnershipRequest>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyOwnershipShareModeEnum shareMode, in List<MySingleOwnershipRequest> requests, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnChangeOwnersSuccess(shareMode, requests);
			}
		}

		protected sealed class OnLogHierarchy_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnLogHierarchy();
			}
		}

		protected sealed class DepressurizeEffect_003C_003ESystem_Int64_0023VRageMath_Vector3I_0023VRageMath_Vector3I : ICallSite<IMyEventOwner, long, Vector3I, Vector3I, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long gridId, in Vector3I from, in Vector3I to, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DepressurizeEffect(gridId, from, to);
			}
		}

		protected sealed class MergeGrid_MergeClient_003C_003ESystem_Int64_0023VRage_SerializableVector3I_0023VRageMath_Base6Directions_003C_003EDirection_0023VRageMath_Base6Directions_003C_003EDirection_0023VRageMath_Vector3I : ICallSite<MyCubeGrid, long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction, Vector3I, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long gridId, in SerializableVector3I gridOffset, in Base6Directions.Direction gridForward, in Base6Directions.Direction gridUp, in Vector3I mergingBlockPos, in DBNull arg6)
			{
				@this.MergeGrid_MergeClient(gridId, gridOffset, gridForward, gridUp, mergingBlockPos);
			}
		}

		protected sealed class MergeGrid_MergeBlockClient_003C_003ESystem_Int64_0023VRage_SerializableVector3I_0023VRageMath_Base6Directions_003C_003EDirection_0023VRageMath_Base6Directions_003C_003EDirection : ICallSite<MyCubeGrid, long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long gridId, in SerializableVector3I gridOffset, in Base6Directions.Direction gridForward, in Base6Directions.Direction gridUp, in DBNull arg5, in DBNull arg6)
			{
				@this.MergeGrid_MergeBlockClient(gridId, gridOffset, gridForward, gridUp);
			}
		}

		protected class m_isSolarOccluded_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isSolarOccluded;
				ISyncType result = (isSolarOccluded = new Sync<bool, SyncDirection.FromServer>(P_1, P_2));
				((MyCubeGrid)P_0).m_isSolarOccluded = (Sync<bool, SyncDirection.FromServer>)isSolarOccluded;
				return result;
			}
		}

		protected class m_handBrakeSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType handBrakeSync;
				ISyncType result = (handBrakeSync = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyCubeGrid)P_0).m_handBrakeSync = (Sync<bool, SyncDirection.BothWays>)handBrakeSync;
				return result;
			}
		}

		protected class m_dampenersEnabled_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType dampenersEnabled;
				ISyncType result = (dampenersEnabled = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyCubeGrid)P_0).m_dampenersEnabled = (Sync<bool, SyncDirection.BothWays>)dampenersEnabled;
				return result;
			}
		}

		protected class m_markedAsTrash_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType markedAsTrash;
				ISyncType result = (markedAsTrash = new Sync<bool, SyncDirection.FromServer>(P_1, P_2));
				((MyCubeGrid)P_0).m_markedAsTrash = (Sync<bool, SyncDirection.FromServer>)markedAsTrash;
				return result;
			}
		}

<<<<<<< HEAD
		protected class m_IsPowered_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isPowered;
				ISyncType result = (isPowered = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyCubeGrid)P_0).m_IsPowered = (Sync<bool, SyncDirection.BothWays>)isPowered;
				return result;
			}
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		protected class GridGeneralDamageModifier_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType gridGeneralDamageModifier;
				ISyncType result = (gridGeneralDamageModifier = new Sync<float, SyncDirection.FromServer>(P_1, P_2));
				((MyCubeGrid)P_0).GridGeneralDamageModifier = (Sync<float, SyncDirection.FromServer>)gridGeneralDamageModifier;
				return result;
			}
		}

		protected class m_isRespawnGrid_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isRespawnGrid;
				ISyncType result = (isRespawnGrid = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyCubeGrid)P_0).m_isRespawnGrid = (Sync<bool, SyncDirection.BothWays>)isRespawnGrid;
				return result;
			}
		}

		protected class m_destructibleBlocks_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType destructibleBlocks;
				ISyncType result = (destructibleBlocks = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyCubeGrid)P_0).m_destructibleBlocks = (Sync<bool, SyncDirection.BothWays>)destructibleBlocks;
				return result;
			}
		}

		protected class m_immune_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType immune;
				ISyncType result = (immune = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyCubeGrid)P_0).m_immune = (Sync<bool, SyncDirection.BothWays>)immune;
				return result;
			}
		}

		protected class m_editable_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType editable;
				ISyncType result = (editable = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyCubeGrid)P_0).m_editable = (Sync<bool, SyncDirection.BothWays>)editable;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyCubeGrid_003C_003EActor : IActivator, IActivator<MyCubeGrid>
		{
			private sealed override object CreateInstance()
			{
				return new MyCubeGrid();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCubeGrid CreateInstance()
			{
				return new MyCubeGrid();
			}

			MyCubeGrid IActivator<MyCubeGrid>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly int BLOCK_LIMIT_FOR_LARGE_DESTRUCTION;

		private static readonly int TRASH_HIGHLIGHT;

		private const float PASTE_GRIDS_SPEED_REDUCTION_FOR_SAFETY = 10f;

		private static MyCubeGridHitInfo m_hitInfoTmp;

		private static HashSet<MyBlockLocation> m_tmpBuildList;

		private static List<Vector3I> m_tmpPositionListReceive;

		private static List<Vector3I> m_tmpPositionListSend;

		private List<Vector3I> m_removeBlockQueue = new List<Vector3I>();

		private List<Vector3I> m_destroyBlockQueue = new List<Vector3I>();

		private List<Vector3I> m_destructionDeformationQueue = new List<Vector3I>();

		private List<BlockPositionId> m_destroyBlockWithIdQueue = new List<BlockPositionId>();

		private List<BlockPositionId> m_removeBlockWithIdQueue = new List<BlockPositionId>();

		[ThreadStatic]
		private static List<byte> m_boneByteList;

		private List<long> m_tmpBlockIdList = new List<long>();

		private HashSet<MyCubeBlock> m_inventoryBlocks = new HashSet<MyCubeBlock>();

		private HashSet<MyCubeBlock> m_unsafeBlocks = new HashSet<MyCubeBlock>();

		private HashSet<MyDecoy> m_decoys;

		private bool m_isRazeBatchDelayed;

		private MyDelayedRazeBatch m_delayedRazeBatch;

		public HashSet<MyCockpit> m_occupiedBlocks = new HashSet<MyCockpit>();

		private Vector3 m_gravity = Vector3.Zero;

		private readonly Sync<bool, SyncDirection.FromServer> m_isSolarOccluded;

		private readonly Sync<bool, SyncDirection.BothWays> m_handBrakeSync;

		private readonly Sync<bool, SyncDirection.BothWays> m_dampenersEnabled;

		private static List<MyObjectBuilder_CubeGrid> m_recievedGrids;

		public bool IsAccessibleForProgrammableBlock = true;

		private bool m_largeDestroyInProgress;

		private readonly Sync<bool, SyncDirection.FromServer> m_markedAsTrash;

		private int m_trashHighlightCounter;

<<<<<<< HEAD
		/// <summary>
		/// Fires when any inventory on the grid was changed (including logical group). This event works only on the server.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		internal Action<MyInventoryBase> OnAnyBlockInventoryChanged;

		private MyUpdateTiersGridPresence m_gridPresenceTier;

		private MyUpdateTiersPlayerPresence m_playerPresenceTier;

<<<<<<< HEAD
		/// <summary>
		/// Used when calculating damage from deformation application
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private float m_totalBoneDisplacement;

		/// <summary>
		/// Value used by MoveCornerBones() to precalculate the bones displacement distance.
		/// A performance boost is gained because we can then avoid having to use Vector3.Length() which means we don't need sqrt() each time.
		/// </summary>
		private static float m_precalculatedCornerBonesDisplacementDistance;

		internal MyVoxelSegmentation BonesToSend = new MyVoxelSegmentation();

		private MyVoxelSegmentation m_bonesToSendSecond = new MyVoxelSegmentation();

		private int m_bonesSendCounter;

		private MyDirtyRegion m_dirtyRegion = new MyDirtyRegion();

		private MyDirtyRegion m_dirtyRegionParallel = new MyDirtyRegion();

		private MyCubeSize m_gridSizeEnum;

		private Vector3I m_min = Vector3I.MaxValue;

		private Vector3I m_max = Vector3I.MinValue;

<<<<<<< HEAD
		private readonly ConcurrentDictionary<Vector3I, MyCube> m_cubes = new ConcurrentDictionary<Vector3I, MyCube>(Vector3I.Comparer);
=======
		private readonly ConcurrentDictionary<Vector3I, MyCube> m_cubes = new ConcurrentDictionary<Vector3I, MyCube>((IEqualityComparer<Vector3I>)Vector3I.Comparer);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private readonly FastResourceLock m_cubeLock = new FastResourceLock();

		/// <summary>
		/// This caches if grid can have physics, once set to false, it stays false and grid is eventually closed.
		/// </summary>
		private bool m_canHavePhysics = true;

		private readonly HashSet<MySlimBlock> m_cubeBlocks = new HashSet<MySlimBlock>();

		private MyConcurrentList<MyCubeBlock> m_fatBlocks = new MyConcurrentList<MyCubeBlock>(100);

		private MyLocalityGrouping m_explosions = new MyLocalityGrouping(MyLocalityGrouping.GroupingMode.Overlaps);

		private Dictionary<Vector3, int> m_colorStatistics = new Dictionary<Vector3, int>();

		private int m_PCU;

		private readonly int HUD_NOTIFICATION_TIMEOUT = 5000;

		private int m_lastTimeDisplayedPaintFail;

		private Sync<bool, SyncDirection.BothWays> m_IsPowered;

		private HashSet<MyCubeBlock> m_processedBlocks = new HashSet<MyCubeBlock>();

		private ConcurrentCachingHashSet<MyCubeBlock> m_blocksForDraw = new ConcurrentCachingHashSet<MyCubeBlock>();

		private List<MyCubeGrid> m_tmpGrids = new List<MyCubeGrid>();

		private MyTestDisconnectsReason m_disconnectsDirty;

		private bool m_blocksForDamageApplicationDirty;

		private bool m_boundsDirty;

		private int m_lastUpdatedDirtyBounds;

		private HashSet<MySlimBlock> m_blocksForDamageApplication = new HashSet<MySlimBlock>();

		private List<MySlimBlock> m_blocksForDamageApplicationCopy = new List<MySlimBlock>();

		private bool m_updatingDirty;

		private int m_resolvingSplits;

		private HashSet<Vector3UByte> m_tmpBuildFailList = new HashSet<Vector3UByte>();

		private List<Vector3UByte> m_tmpBuildOffsets = new List<Vector3UByte>();

		private List<MySlimBlock> m_tmpBuildSuccessBlocks = new List<MySlimBlock>();

		private static List<Vector3I> m_tmpBlockPositions;

		[ThreadStatic]
		private static List<MySlimBlock> m_tmpBlockListReceive;

		[ThreadStatic]
		private static List<MyCockpit> m_tmpOccupiedCockpitsPerThread;

		[ThreadStatic]
		private static List<MyObjectBuilder_BlockGroup> m_tmpBlockGroupsPerThread;

		public bool HasShipSoundEvents;

		public int NumberOfReactors;

		public readonly Sync<float, SyncDirection.FromServer> GridGeneralDamageModifier;

		internal MyGridSkeleton Skeleton;

		public readonly BlockTypeCounter BlockCounter = new BlockTypeCounter();

		public Dictionary<MyObjectBuilderType, int> BlocksCounters = new Dictionary<MyObjectBuilderType, int>();

		private const float m_gizmoMaxDistanceFromCamera = 100f;

		private const float m_gizmoDrawLineScale = 0.002f;

		private bool m_isStatic;

		public Vector3I? XSymmetryPlane;

		public Vector3I? YSymmetryPlane;

		public Vector3I? ZSymmetryPlane;

		public bool XSymmetryOdd;

		public bool YSymmetryOdd;

		public bool ZSymmetryOdd;

		/// <summary>
		/// Indicates if a grid corresponds to a respawn ship/cart.
		/// </summary>
		private readonly Sync<bool, SyncDirection.BothWays> m_isRespawnGrid;

		/// <summary>
		/// Grid play time with player. Used by respawn ship. 
		/// </summary>
		public int m_playedTime;

		public bool ControlledFromTurret;

		private readonly Sync<bool, SyncDirection.BothWays> m_destructibleBlocks;

		private readonly Sync<bool, SyncDirection.BothWays> m_immune;

		private Sync<bool, SyncDirection.BothWays> m_editable;

		internal readonly List<MyBlockGroup> BlockGroups = new List<MyBlockGroup>();

		internal MyCubeGridOwnershipManager m_ownershipManager;

		public MyProjectorBase Projector;

		private bool m_isMarkedForEarlyDeactivation;

		public Action<bool> GridPresenceUpdate;

		public bool CreatePhysics;

		private static readonly HashSet<MyResourceSinkComponent> m_tmpSinks;

		private static List<LocationIdentity> m_tmpLocationsAndIdsSend;

		private static List<Tuple<Vector3I, ushort>> m_tmpLocationsAndIdsReceive;

		private bool m_smallToLargeConnectionsInitialized;

		private bool m_enableSmallToLargeConnections = true;

		private MyTestDynamicReason m_testDynamic;

		public MyTerminalBlock MainCockpit;

		public MyTerminalBlock MainRemoteControl;

		private Dictionary<int, MyCubeGridMultiBlockInfo> m_multiBlockInfos;

		private float PREDICTION_SWITCH_TIME = 5f;

		private int PREDICTION_SWITCH_MIN_COUNTER = 30;

		private bool m_inventoryMassDirty;

		private bool m_dirtyRegionScheduled;

		private static List<MyVoxelBase> m_overlappingVoxelsTmp;

		private static HashSet<MyVoxelBase> m_rootVoxelsToCutTmp;

		private static ConcurrentQueue<MyTuple<int, MyVoxelBase, Vector3I, Vector3I>> m_notificationQueue;

		private int m_standAloneBlockCount;

		private List<DeformationPostponedItem> m_deformationPostponed = new List<DeformationPostponedItem>();

		private static MyConcurrentPool<List<DeformationPostponedItem>> m_postponedListsPool;

		private Action m_OnUpdateDirtyCompleted;

		private Action m_UpdateDirtyInternal;

		private bool m_bonesSending;

		private WorkData m_workData = new WorkData();

		[ThreadStatic]
		private static HashSet<MyEntity> m_tmpQueryCubeBlocks;

		[ThreadStatic]
		private static HashSet<MySlimBlock> m_tmpQuerySlimBlocks;

		private static readonly Vector3I[] m_tmpBlockSurroundingOffsets;

		private MyHudNotification m_inertiaDampenersNotification;

		private MyGridClientState m_lastNetState;

		private List<long> m_targetingList = new List<long>();

		private bool m_targetingListIsWhitelist;

		private bool m_usesTargetingList;

		private Action m_convertToShipResult;

		private long m_closestParentId;

		private bool m_isClientPredicted;

		private bool m_forceDisablePrediction;

		private bool m_allowPrediction = true;

		private Action m_pendingGridReleases;

		private Action<MatrixD> m_updateMergingGrids;

		private const float GRID_PLACING_AREA_FIX_VALUE = 0.11f;

		private const string EXPORT_DIRECTORY = "ExportedModels";

		private const string SOURCE_DIRECTORY = "SourceModels";

		private static readonly List<MyObjectBuilder_CubeGrid[]> m_prefabs;

		[ThreadStatic]
		private static List<MyEntity> m_tmpResultListPerThread;

		private static readonly List<MyVoxelBase> m_tmpVoxelList;

		private static int materialID;

		private static Vector2 tumbnailMultiplier;

		private static float m_maxDimensionPreviousRow;

		private static Vector3D m_newPositionForPlacedObject;

		private const int m_numRowsForPlacedObjects = 4;

		private static List<MyLineSegmentOverlapResult<MyEntity>> m_lineOverlapList;

		[ThreadStatic]
		private static List<HkBodyCollision> m_physicsBoxQueryListPerThread;

		[ThreadStatic]
		private static Dictionary<Vector3I, MySlimBlock> m_tmpCubeSet;

		private readonly MyDisconnectHelper m_disconnectHelper = new MyDisconnectHelper();

		private static readonly List<NeighborOffsetIndex> m_neighborOffsetIndices;

		private static readonly List<float> m_neighborDistances;

		private static readonly List<Vector3I> m_neighborOffsets;

		[ThreadStatic]
		private static MyRandom m_deformationRng;

		[ThreadStatic]
		private static List<Vector3I> m_cacheRayCastCellsPerThread;

		[ThreadStatic]
		private static Dictionary<Vector3I, ConnectivityResult> m_cacheNeighborBlocksPerThread;

		[ThreadStatic]
		private static List<MyCubeBlockDefinition.MountPoint> m_cacheMountPointsAPerThread;

		[ThreadStatic]
		private static List<MyCubeBlockDefinition.MountPoint> m_cacheMountPointsBPerThread;

		private static readonly MyComponentList m_buildComponents;

		[ThreadStatic]
		private static List<MyPhysics.HitInfo> m_tmpHitListPerThread;

		private static readonly HashSet<Vector3UByte> m_tmpAreaMountpointPass;

		private static readonly AreaConnectivityTest m_areaOverlapTest;

		private static readonly HashSet<Tuple<MySlimBlock, ushort?>> m_tmpBlocksInMultiBlock;

		private static readonly List<MySlimBlock> m_tmpSlimBlocks;

		[ThreadStatic]
		private static List<int> m_tmpMultiBlockIndicesPerThread;

		private static readonly Type m_gridSystemsType;

		private static readonly List<Tuple<Vector3I, ushort>> m_tmpRazeList;

		private static readonly List<Vector3I> m_tmpLocations;

		[ThreadStatic]
		private static Ref<HkBoxShape> m_lastQueryBoxPerThread;

		[ThreadStatic]
		private static MatrixD m_lastQueryTransform;

		private const double ROTATION_PRECISION = 0.0010000000474974513;

<<<<<<< HEAD
		/// <summary>
		/// Global update queue,
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static readonly List<QueuedUpdateChange> m_pendingAddedUpdates;

		private readonly List<Update>[] m_updateQueues = new List<Update>[4];

		private int m_totalQueuedSynchronousUpdates;

		private int m_totalQueuedParallelUpdates;

<<<<<<< HEAD
		/// <summary>
		/// Queue that is currently dispatched if any.
		/// </summary>
		private UpdateQueue m_updateInProgress;

		/// <summary>
		/// Whether this grid has any callback additions or removals pending.
		/// </summary>
=======
		private UpdateQueue m_updateInProgress;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private bool m_hasDelayedUpdate;

		private static readonly ConcurrentDictionary<MethodInfo, string> m_methodNames;

<<<<<<< HEAD
		/// <summary>
		/// Number of gameplay frames to keep the update history for. Defaults to 120 frames.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static int DebugUpdateHistoryDuration;

		public HashSet<MyCubeBlock> Inventories => m_inventoryBlocks;

		public HashSetReader<MyCubeBlock> UnsafeBlocks => m_unsafeBlocks;

		public HashSetReader<MyDecoy> Decoys => m_decoys;

		public HashSetReader<MyCockpit> OccupiedBlocks => m_occupiedBlocks;

		public SyncType SyncType { get; set; }
<<<<<<< HEAD

		internal float IntegrityMass { get; set; }

		public bool IsPowered => m_IsPowered.Value;

		public bool IsParked
		{
			get
			{
				if (!GridSystems.WheelSystem.IsParked && !GridSystems.LandingSystem.IsParked)
				{
					return GridSystems.ConveyorSystem.IsParked;
				}
				return true;
			}
		}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Return how many colors are on grid
		/// </summary>
		public int NumberOfGridColors => m_colorStatistics.Count;

		public ConcurrentCachingHashSet<Vector3I> DirtyBlocks
		{
			get
			{
				m_dirtyRegion.Cubes.ApplyChanges();
				return m_dirtyRegion.Cubes;
			}
		}

		public MyCubeGridRenderData RenderData => Render.RenderData;

		public ConcurrentCachingHashSet<MyCubeBlock> BlocksForDraw => m_blocksForDraw;

		public bool IsSplit { get; set; }

		private static List<MyCockpit> m_tmpOccupiedCockpits => MyUtils.Init(ref m_tmpOccupiedCockpitsPerThread);

		private static List<MyObjectBuilder_BlockGroup> m_tmpBlockGroups => MyUtils.Init(ref m_tmpBlockGroupsPerThread);

		public MyCubeGridSystems GridSystems { get; private set; }

		public bool IsStatic
		{
			get
			{
				return m_isStatic;
			}
			private set
			{
				if (m_isStatic != value)
				{
					m_isStatic = value;
					NotifyIsStaticChanged(m_isStatic);
				}
			}
		}

		public bool DampenersEnabled => m_dampenersEnabled;

		public bool MarkedAsTrash => m_markedAsTrash;

		public bool IsUnsupportedStation { get; private set; }

		public float GridSize { get; private set; }

		public float GridScale { get; private set; }

		public float GridSizeHalf { get; private set; }

		public Vector3 GridSizeHalfVector { get; private set; }

		public float GridSizeQuarter { get; private set; }

		public Vector3 GridSizeQuarterVector { get; private set; }

<<<<<<< HEAD
		public Vector3 LinearVelocity { get; private set; }

		/// <summary>
		/// Reciprocal of gridsize
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public float GridSizeR { get; private set; }

		public Vector3I Min => m_min;

		public Vector3I Max => m_max;

		/// <summary>
		/// Gets or sets indication if a grid coresponds to a respawn ship/cart.
		/// </summary>
		public bool IsRespawnGrid
		{
			get
			{
				return m_isRespawnGrid;
			}
			set
			{
				m_isRespawnGrid.Value = value;
			}
		}

		public bool DestructibleBlocks
		{
			get
			{
				return m_destructibleBlocks;
			}
			set
			{
				m_destructibleBlocks.Value = value;
			}
		}

		public bool Immune
		{
			get
			{
				return m_immune;
			}
			set
			{
				m_immune.Value = value;
			}
		}

		public bool Editable
		{
			get
			{
				return m_editable;
			}
			set
			{
				m_editable.ValidateAndSet(value);
			}
		}

		public bool BlocksDestructionEnabled
		{
			get
			{
				if (MySession.Static.Settings.DestructibleBlocks)
				{
					if ((bool)m_destructibleBlocks)
					{
						return !m_immune;
					}
					return false;
				}
				return false;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// players that have at least one block in cube grid
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public List<long> SmallOwners
		{
			get
			{
				if (m_ownershipManager.NeedRecalculateOwners)
				{
					m_ownershipManager.RecalculateOwnersThreadSafe();
				}
				return m_ownershipManager.SmallOwners;
			}
		}

		/// <summary>
		/// players that have the maximum number of functional blocks in cube grid
		/// </summary>
		public List<long> BigOwners
		{
			get
			{
				if (m_ownershipManager.NeedRecalculateOwners)
				{
					m_ownershipManager.RecalculateOwnersThreadSafe();
				}
				List<long> bigOwners = m_ownershipManager.BigOwners;
				if (bigOwners.Count == 0)
				{
<<<<<<< HEAD
					if (!MyEntities.IsAsyncUpdateInProgress && Thread.CurrentThread != MySandboxGame.Static.UpdateThread)
=======
					if (!MyEntities.IsAsyncUpdateInProgress && Thread.get_CurrentThread() != MySandboxGame.Static.UpdateThread)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						return bigOwners;
					}
					MyCubeGrid parent = MyGridPhysicalHierarchy.Static.GetParent(this);
					if (parent != null)
					{
						bigOwners = parent.BigOwners;
					}
				}
				return bigOwners;
			}
		}

		public MyCubeSize GridSizeEnum
		{
			get
			{
				return m_gridSizeEnum;
			}
			set
			{
				m_gridSizeEnum = value;
				GridSize = MyDefinitionManager.Static.GetCubeSize(value);
				GridSizeHalf = GridSize / 2f;
				GridSizeHalfVector = new Vector3(GridSizeHalf);
				GridSizeQuarter = GridSize / 4f;
				GridSizeQuarterVector = new Vector3(GridSizeQuarter);
				GridSizeR = 1f / GridSize;
			}
		}

		public new MyGridPhysics Physics
		{
			get
			{
				return (MyGridPhysics)base.Physics;
			}
			set
			{
				base.Physics = value;
			}
		}

		public int ShapeCount
		{
			get
			{
				if (Physics == null)
				{
					return 0;
				}
				return Physics.Shape.ShapeCount;
			}
		}

		public MyEntityThrustComponent EntityThrustComponent => base.Components.Get<MyEntityThrustComponent>();

		public bool IsMarkedForEarlyDeactivation
		{
			get
			{
				return m_isMarkedForEarlyDeactivation;
			}
			set
			{
				if (m_isMarkedForEarlyDeactivation != value)
				{
					m_isMarkedForEarlyDeactivation = value;
					if (Sync.IsServer)
					{
						Schedule(UpdateQueue.OnceAfterSimulation, CheckEarlyDeactivation, 3);
					}
				}
			}
		}

		public bool IsBlockTrasferInProgress { get; private set; }

<<<<<<< HEAD
		/// <summary>
		/// Gets or sets is generated, used for economy stations
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool IsGenerated { get; internal set; }

		public MyUpdateTiersGridPresence GridPresenceTier
		{
			get
			{
				return m_gridPresenceTier;
			}
			set
			{
				if (m_gridPresenceTier != value)
				{
					m_gridPresenceTier = value;
					this.GridPresenceTierChanged.InvokeIfNotNull(this);
				}
			}
		}

		public MyUpdateTiersPlayerPresence PlayerPresenceTier
		{
			get
			{
				return m_playerPresenceTier;
			}
			set
			{
				if (m_playerPresenceTier != value)
				{
					m_playerPresenceTier = value;
					this.PlayerPresenceTierChanged.InvokeIfNotNull(this);
				}
			}
		}

		public float Mass
		{
			get
			{
				if (Physics == null)
				{
					return 0f;
				}
				if (!Sync.IsServer && IsStatic && Physics != null && Physics.Shape != null)
				{
					if (!Physics.Shape.MassProperties.HasValue)
					{
						return 0f;
					}
					return Physics.Shape.MassProperties.Value.Mass;
				}
				return Physics.Mass;
			}
		}

		public static int GridCounter { get; private set; }

		public int BlocksCount => m_cubeBlocks.get_Count();

		public int BlocksPCU
		{
			get
			{
				return m_PCU;
			}
			set
			{
				m_PCU = value;
			}
		}

		public HashSet<MySlimBlock> CubeBlocks => m_cubeBlocks;

		internal bool SmallToLargeConnectionsInitialized => m_smallToLargeConnectionsInitialized;

		internal bool EnableSmallToLargeConnections => m_enableSmallToLargeConnections;

		internal MyTestDynamicReason TestDynamic
		{
			get
			{
				return m_testDynamic;
			}
			set
			{
				if (m_testDynamic != value)
				{
					m_testDynamic = value;
					if (Sync.IsServer)
					{
						Schedule(UpdateQueue.OnceAfterSimulation, CheckConvertToDynamic, 4);
					}
				}
			}
		}

		internal new MyRenderComponentCubeGrid Render
		{
			get
			{
				return (MyRenderComponentCubeGrid)base.Render;
			}
			set
			{
				base.Render = value;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Local coord system under which this cube exists. (its id)
		/// </summary>
		public long LocalCoordSystem { get; set; }

		public bool IsSolarOccluded => m_isSolarOccluded;
=======
		public long LocalCoordSystem { get; set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		internal bool NeedsPerFrameDraw
		{
			get
			{
				if (!MyFakes.OPTIMIZE_GRID_UPDATES)
				{
					return true;
				}
				int num = 0 | (IsDirty() ? 1 : 0) | ((ShowCenterOfMass || ShowGridPivot || ShowSenzorGizmos || ShowGravityGizmos || ShowAntennaGizmos) ? 1 : 0) | ((MyFakes.ENABLE_GRID_SYSTEM_UPDATE && GridSystems.NeedsPerFrameDraw) ? 1 : 0);
				BlocksForDraw.ApplyChanges();
				return (byte)((uint)num | ((BlocksForDraw.Count > 0) ? 1u : 0u) | (MarkedAsTrash ? 1u : 0u)) != 0;
			}
		}

		public bool IsLargeDestroyInProgress
		{
			get
			{
				if (m_destroyBlockQueue.Count <= BLOCK_LIMIT_FOR_LARGE_DESTRUCTION)
				{
					return m_largeDestroyInProgress;
				}
				return true;
			}
		}

		public bool UsesTargetingList => m_usesTargetingList;

		public long ClosestParentId
		{
			get
			{
				return m_closestParentId;
			}
			set
			{
				if (m_closestParentId != value)
				{
					if (MyEntities.TryGetEntityById(m_closestParentId, out MyCubeGrid entity, allowClosed: true))
					{
						MyGridPhysicalHierarchy.Static.RemoveNonGridNode(entity, this);
					}
					if (MyEntities.TryGetEntityById(value, out entity, allowClosed: false))
					{
						m_closestParentId = value;
						MyGridPhysicalHierarchy.Static.AddNonGridNode(entity, this);
					}
					else
					{
						m_closestParentId = 0L;
					}
				}
			}
		}

		public bool IsClientPredicted
		{
			get
			{
				return m_isClientPredicted;
			}
			private set
			{
				if (m_isClientPredicted != value)
				{
					m_isClientPredicted = value;
					if (!Sync.IsServer)
					{
						Schedule(UpdateQueue.OnceAfterSimulation, ClientPredictionStaticCheck, 5);
					}
				}
			}
		}

		public bool IsClientPredictedWheel { get; private set; }

		public bool IsClientPredictedCar { get; private set; }

		public bool ForceDisablePrediction
		{
			get
			{
				return m_forceDisablePrediction;
			}
			set
			{
				if (m_forceDisablePrediction != value)
				{
					m_forceDisablePrediction = value;
					CheckPredictionFlagScheduling();
				}
			}
		}

		public bool AllowPrediction
		{
			get
			{
				return m_allowPrediction;
			}
			set
			{
				if (m_allowPrediction != value)
				{
					m_allowPrediction = value;
					CheckPredictionFlagScheduling();
				}
			}
		}

		public int TrashHighlightCounter => m_trashHighlightCounter;

<<<<<<< HEAD
		public Vector3D ShootOrigin
		{
			get
			{
				if (MainCockpit != null)
				{
					return MainCockpit.WorldMatrix.Translation;
				}
				return MyGridPhysicalGroupData.GetGroupSharedProperties(this).CoMWorld;
			}
		}

		public MyDefinitionBase GetAmmoDefinition
		{
			get
			{
				foreach (MyCockpit occupiedBlock in OccupiedBlocks)
				{
					MyShipController current;
					if ((current = occupiedBlock) != null)
					{
						return current.GetAmmoDefinition;
					}
				}
				return null;
			}
		}

		public float MaxShootRange => (GetAmmoDefinition as MyAmmoDefinition)?.MaxTrajectory ?? 0f;

		private static List<MyEntity> m_tmpResultList => MyUtils.Init(ref m_tmpResultListPerThread);

		public static bool ShowSenzorGizmos { get; set; }

		public static bool ShowGravityGizmos { get; set; }

		public static bool ShowAntennaGizmos { get; set; }

		public static bool ShowCenterOfMass { get; set; }

=======
		private static List<MyEntity> m_tmpResultList => MyUtils.Init(ref m_tmpResultListPerThread);

		public static bool ShowSenzorGizmos { get; set; }

		public static bool ShowGravityGizmos { get; set; }

		public static bool ShowAntennaGizmos { get; set; }

		public static bool ShowCenterOfMass { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static bool ShowGridPivot { get; set; }

		private static List<HkBodyCollision> m_physicsBoxQueryList => MyUtils.Init(ref m_physicsBoxQueryListPerThread);

		private static List<Vector3I> m_cacheRayCastCells => MyUtils.Init(ref m_cacheRayCastCellsPerThread);

		private static Dictionary<Vector3I, ConnectivityResult> m_cacheNeighborBlocks => MyUtils.Init(ref m_cacheNeighborBlocksPerThread);

		private static List<MyCubeBlockDefinition.MountPoint> m_cacheMountPointsA => MyUtils.Init(ref m_cacheMountPointsAPerThread);

		private static List<MyCubeBlockDefinition.MountPoint> m_cacheMountPointsB => MyUtils.Init(ref m_cacheMountPointsBPerThread);

		private static List<MyPhysics.HitInfo> m_tmpHitList => MyUtils.Init(ref m_tmpHitListPerThread);

		private static List<int> m_tmpMultiBlockIndices => MyUtils.Init(ref m_tmpMultiBlockIndicesPerThread);

		private static Ref<HkBoxShape> m_lastQueryBox
		{
			get
			{
				if (m_lastQueryBoxPerThread == null)
				{
					m_lastQueryBoxPerThread = new Ref<HkBoxShape>();
					m_lastQueryBoxPerThread.Value = new HkBoxShape(Vector3.One);
				}
				return m_lastQueryBoxPerThread;
			}
		}

		public IEnumerable<KeyValuePair<DebugUpdateRecord, DebugUpdateStats>> LastUpdates
		{
			get
			{
				throw new InvalidOperationException("Feature only available in DEBUG builds.");
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override MyGameLogicComponent GameLogic
		{
			get
			{
				return base.GameLogic;
			}
			set
			{
				if (value != null && value.EntityUpdate)
				{
					Schedule(UpdateQueue.BeforeSimulation, value.UpdateBeforeSimulation);
					Schedule(UpdateQueue.AfterSimulation, value.UpdateAfterSimulation);
				}
				else if (GameLogic != null)
				{
					DeSchedule(UpdateQueue.BeforeSimulation, GameLogic.UpdateBeforeSimulation);
					DeSchedule(UpdateQueue.AfterSimulation, GameLogic.UpdateAfterSimulation);
				}
				base.GameLogic = value;
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyParallelUpdateFlags UpdateFlags
		{
			get
			{
				MyParallelUpdateFlags myParallelUpdateFlags = base.NeedsUpdate.GetParallel();
				if (m_totalQueuedParallelUpdates > 0)
				{
					myParallelUpdateFlags |= MyParallelUpdateFlags.EACH_FRAME_PARALLEL;
				}
				return myParallelUpdateFlags;
			}
		}

		string VRage.Game.ModAPI.Ingame.IMyCubeGrid.CustomName
		{
			get
			{
				return base.DisplayName;
			}
			set
			{
				if (IsAccessibleForProgrammableBlock)
				{
					ChangeDisplayNameRequest(value);
				}
			}
		}

		string VRage.Game.ModAPI.IMyCubeGrid.CustomName
		{
			get
			{
				return base.DisplayName;
			}
			set
			{
				ChangeDisplayNameRequest(value);
			}
		}

		List<long> VRage.Game.ModAPI.IMyCubeGrid.BigOwners => BigOwners;

		List<long> VRage.Game.ModAPI.IMyCubeGrid.SmallOwners => SmallOwners;

		bool VRage.Game.ModAPI.IMyCubeGrid.IsRespawnGrid
		{
			get
			{
				return IsRespawnGrid;
			}
			set
			{
				IsRespawnGrid = value;
			}
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.IsStatic
		{
			get
			{
				return IsStatic;
			}
			set
			{
				if (value)
				{
					RequestConversionToStation();
				}
				else
				{
					RequestConversionToShip(null);
				}
			}
		}

		Vector3I? VRage.Game.ModAPI.IMyCubeGrid.XSymmetryPlane
		{
			get
			{
				return XSymmetryPlane;
			}
			set
			{
				XSymmetryPlane = value;
			}
		}

		Vector3I? VRage.Game.ModAPI.IMyCubeGrid.YSymmetryPlane
		{
			get
			{
				return YSymmetryPlane;
			}
			set
			{
				YSymmetryPlane = value;
			}
		}

		Vector3I? VRage.Game.ModAPI.IMyCubeGrid.ZSymmetryPlane
		{
			get
			{
				return ZSymmetryPlane;
			}
			set
			{
				ZSymmetryPlane = value;
			}
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.XSymmetryOdd
		{
			get
			{
				return XSymmetryOdd;
			}
			set
			{
				XSymmetryOdd = value;
			}
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.YSymmetryOdd
		{
			get
			{
				return YSymmetryOdd;
			}
			set
			{
				YSymmetryOdd = value;
			}
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.ZSymmetryOdd
		{
			get
			{
				return ZSymmetryOdd;
			}
			set
			{
				ZSymmetryOdd = value;
			}
		}

		MyUpdateTiersGridPresence VRage.Game.ModAPI.IMyCubeGrid.GridPresenceTier => GridPresenceTier;

		MyUpdateTiersPlayerPresence VRage.Game.ModAPI.IMyCubeGrid.PlayerPresenceTier => PlayerPresenceTier;

<<<<<<< HEAD
		IMyResourceDistributorComponent VRage.Game.ModAPI.IMyCubeGrid.ResourceDistributor => GridSystems.ResourceDistributor;

		public IMyGridConveyorSystem ConveyorSystem => GridSystems.ConveyorSystem;

		IMyGridWeaponSystem VRage.Game.ModAPI.IMyCubeGrid.WeaponSystem => GridSystems.WeaponSystem;

		IMyGridControlSystem VRage.Game.ModAPI.IMyCubeGrid.ControlSystem => GridSystems.ControlSystem;

		IMyGridJumpDriveSystem VRage.Game.ModAPI.IMyCubeGrid.JumpSystem => GridSystems.JumpSystem;

		/// <summary>
		/// Can be null if oxygen is disabled
		/// </summary>
		IMyGridGasSystem VRage.Game.ModAPI.IMyCubeGrid.GasSystem => GridSystems.GasSystem ?? null;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public event Action<SyncBase> SyncPropertyChanged
		{
			add
			{
				SyncType.PropertyChanged += value;
			}
			remove
			{
				SyncType.PropertyChanged -= value;
			}
		}

<<<<<<< HEAD
		public event Action<bool> IsPoweredChanged;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public event Action<MyCubeGrid> GridPresenceTierChanged;

		public event Action<MyCubeGrid> PlayerPresenceTierChanged;

		public event Action<MySlimBlock> OnBlockAdded;

		public event Action<MyCubeBlock> OnFatBlockAdded;

		public event Action<MySlimBlock> OnBlockRemoved;

		public event Action<MyCubeBlock> OnFatBlockRemoved;

		public event Action<MySlimBlock> OnBlockIntegrityChanged;

		public event Action<MySlimBlock> OnBlockClosed;

		public event Action<MyCubeBlock> OnFatBlockClosed;

		public event Action<MyCubeGrid> OnMassPropertiesChanged;

		public static event Action<MyCubeGrid> OnSplitGridCreated;

		public event Action<MyCubeGrid> OnBlockOwnershipChanged;

		[Obsolete("Use OnStaticChanged")]
		public event Action<bool> OnIsStaticChanged;

		public event Action<MyCubeGrid, bool> OnStaticChanged;

		public event Action<MyCubeGrid, MyCubeGrid> OnGridSplit;

		/// <summary>
		/// Called, when 2 grids are merged with merge block. First grid - grid that will stay, second - will be merged into first, and deleted.
		/// Called for both grids
		/// </summary>
		public event Action<MyCubeGrid, MyCubeGrid> OnGridMerge;

		public event Action<MyCubeGrid> OnHierarchyUpdated;

		internal event Action<MyGridLogicalGroupData> AddedToLogicalGroup;

		internal event Action RemovedFromLogicalGroup;

		public event Action<int> OnHavokSystemIDChanged;

		public event Action<MyCubeGrid> OnNameChanged;

		public event Action<MyCubeGrid> OnGridChanged;

		/// <summary>
		/// Event triggered, when one of GridLinkTypeEnum connections changed. Used to determine changes in connections. Use may also need react to OnGridMerge/OnGridSplit
		/// </summary>
		public event Action<MyCubeGrid, GridLinkTypeEnum> OnConnectionChanged;

		event Action<VRage.Game.ModAPI.IMySlimBlock> VRage.Game.ModAPI.IMyCubeGrid.OnBlockAdded
		{
			add
			{
				OnBlockAdded += GetDelegate(value);
			}
			remove
			{
				OnBlockAdded -= GetDelegate(value);
			}
		}

		event Action<VRage.Game.ModAPI.IMySlimBlock> VRage.Game.ModAPI.IMyCubeGrid.OnBlockRemoved
		{
			add
			{
				OnBlockRemoved += GetDelegate(value);
			}
			remove
			{
				OnBlockRemoved -= GetDelegate(value);
			}
		}

		event Action<VRage.Game.ModAPI.IMyCubeGrid> VRage.Game.ModAPI.IMyCubeGrid.OnBlockOwnershipChanged
		{
			add
			{
				OnBlockOwnershipChanged += GetDelegate(value);
			}
			remove
			{
				OnBlockOwnershipChanged -= GetDelegate(value);
			}
		}

		event Action<VRage.Game.ModAPI.IMyCubeGrid> VRage.Game.ModAPI.IMyCubeGrid.OnGridChanged
		{
			add
			{
				OnGridChanged += GetDelegate(value);
			}
			remove
			{
				OnGridChanged -= GetDelegate(value);
			}
		}

		event Action<VRage.Game.ModAPI.IMyCubeGrid, VRage.Game.ModAPI.IMyCubeGrid> VRage.Game.ModAPI.IMyCubeGrid.OnGridSplit
		{
			add
			{
				OnGridSplit += GetDelegate(value);
			}
			remove
			{
				OnGridSplit -= GetDelegate(value);
			}
		}

		event Action<VRage.Game.ModAPI.IMyCubeGrid, bool> VRage.Game.ModAPI.IMyCubeGrid.OnIsStaticChanged
		{
			add
			{
				OnStaticChanged += GetDelegate(value);
			}
			remove
			{
				OnStaticChanged -= GetDelegate(value);
			}
		}

		event Action<VRage.Game.ModAPI.IMySlimBlock> VRage.Game.ModAPI.IMyCubeGrid.OnBlockIntegrityChanged
		{
			add
			{
				OnBlockIntegrityChanged += GetDelegate(value);
			}
			remove
			{
				OnBlockIntegrityChanged -= GetDelegate(value);
			}
		}

		event Action<VRage.Game.ModAPI.IMyCubeGrid> VRage.Game.ModAPI.IMyCubeGrid.GridPresenceTierChanged
		{
			add
			{
				GridPresenceTierChanged += GetDelegate(value);
			}
			remove
			{
				GridPresenceTierChanged -= GetDelegate(value);
			}
		}

		event Action<VRage.Game.ModAPI.IMyCubeGrid> VRage.Game.ModAPI.IMyCubeGrid.PlayerPresenceTierChanged
		{
			add
			{
				PlayerPresenceTierChanged += GetDelegate(value);
			}
			remove
			{
				PlayerPresenceTierChanged -= GetDelegate(value);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Called, when 2 grids are merged with merge block. First grid - grid that will stay, second - will be merged into first, and deleted.
		/// Called for both grids
		/// </summary>
		event Action<VRage.Game.ModAPI.IMyCubeGrid, VRage.Game.ModAPI.IMyCubeGrid> VRage.Game.ModAPI.IMyCubeGrid.OnGridMerge
		{
			add
			{
				OnGridMerge += GetDelegate(value);
			}
			remove
			{
				OnGridMerge -= GetDelegate(value);
			}
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		static MyCubeGrid()
		{
			BLOCK_LIMIT_FOR_LARGE_DESTRUCTION = 3;
			TRASH_HIGHLIGHT = 300;
			m_tmpBuildList = new HashSet<MyBlockLocation>();
			m_tmpPositionListReceive = new List<Vector3I>();
			m_tmpPositionListSend = new List<Vector3I>();
			m_recievedGrids = new List<MyObjectBuilder_CubeGrid>();
			m_precalculatedCornerBonesDisplacementDistance = 0f;
			m_tmpBlockPositions = new List<Vector3I>();
			m_tmpSinks = new HashSet<MyResourceSinkComponent>();
			m_tmpLocationsAndIdsSend = new List<LocationIdentity>();
			m_tmpLocationsAndIdsReceive = new List<Tuple<Vector3I, ushort>>();
			m_notificationQueue = new ConcurrentQueue<MyTuple<int, MyVoxelBase, Vector3I, Vector3I>>();
			m_postponedListsPool = new MyConcurrentPool<List<DeformationPostponedItem>>();
			m_tmpBlockSurroundingOffsets = new Vector3I[27]
			{
				new Vector3I(0, 0, 0),
				new Vector3I(1, 0, 0),
				new Vector3I(-1, 0, 0),
				new Vector3I(0, 0, 1),
				new Vector3I(0, 0, -1),
				new Vector3I(1, 0, 1),
				new Vector3I(-1, 0, 1),
				new Vector3I(1, 0, -1),
				new Vector3I(-1, 0, -1),
				new Vector3I(0, 1, 0),
				new Vector3I(1, 1, 0),
				new Vector3I(-1, 1, 0),
				new Vector3I(0, 1, 1),
				new Vector3I(0, 1, -1),
				new Vector3I(1, 1, 1),
				new Vector3I(-1, 1, 1),
				new Vector3I(1, 1, -1),
				new Vector3I(-1, 1, -1),
				new Vector3I(0, -1, 0),
				new Vector3I(1, -1, 0),
				new Vector3I(-1, -1, 0),
				new Vector3I(0, -1, 1),
				new Vector3I(0, -1, -1),
				new Vector3I(1, -1, 1),
				new Vector3I(-1, -1, 1),
				new Vector3I(1, -1, -1),
				new Vector3I(-1, -1, -1)
			};
			m_prefabs = new List<MyObjectBuilder_CubeGrid[]>();
			m_tmpVoxelList = new List<MyVoxelBase>();
			materialID = 0;
			tumbnailMultiplier = default(Vector2);
			m_maxDimensionPreviousRow = 0f;
			m_newPositionForPlacedObject = new Vector3D(0.0, 0.0, 0.0);
			m_lineOverlapList = new List<MyLineSegmentOverlapResult<MyEntity>>();
<<<<<<< HEAD
=======
			m_tmpCubeSet = new Dictionary<Vector3I, MySlimBlock>(Vector3I.Comparer);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_neighborOffsetIndices = new List<NeighborOffsetIndex>(26);
			m_neighborDistances = new List<float>(26);
			m_neighborOffsets = new List<Vector3I>(26);
			m_buildComponents = new MyComponentList();
			m_tmpAreaMountpointPass = new HashSet<Vector3UByte>();
			m_areaOverlapTest = new AreaConnectivityTest();
			m_tmpBlocksInMultiBlock = new HashSet<Tuple<MySlimBlock, ushort?>>();
			m_tmpSlimBlocks = new List<MySlimBlock>();
			m_gridSystemsType = ChooseGridSystemsType();
			m_tmpRazeList = new List<Tuple<Vector3I, ushort>>();
			m_tmpLocations = new List<Vector3I>();
			m_pendingAddedUpdates = new List<QueuedUpdateChange>();
			m_methodNames = new ConcurrentDictionary<MethodInfo, string>();
			DebugUpdateHistoryDuration = 120;
			for (int i = 0; i < 26; i++)
			{
				m_neighborOffsetIndices.Add((NeighborOffsetIndex)i);
				m_neighborDistances.Add(0f);
				m_neighborOffsets.Add(new Vector3I(0, 0, 0));
			}
			m_neighborOffsets[0] = new Vector3I(1, 0, 0);
			m_neighborOffsets[1] = new Vector3I(-1, 0, 0);
			m_neighborOffsets[2] = new Vector3I(0, 1, 0);
			m_neighborOffsets[3] = new Vector3I(0, -1, 0);
			m_neighborOffsets[4] = new Vector3I(0, 0, 1);
			m_neighborOffsets[5] = new Vector3I(0, 0, -1);
			m_neighborOffsets[6] = new Vector3I(1, 1, 0);
			m_neighborOffsets[7] = new Vector3I(1, -1, 0);
			m_neighborOffsets[8] = new Vector3I(-1, 1, 0);
			m_neighborOffsets[9] = new Vector3I(-1, -1, 0);
			m_neighborOffsets[10] = new Vector3I(0, 1, 1);
			m_neighborOffsets[11] = new Vector3I(0, 1, -1);
			m_neighborOffsets[12] = new Vector3I(0, -1, 1);
			m_neighborOffsets[13] = new Vector3I(0, -1, -1);
			m_neighborOffsets[14] = new Vector3I(1, 0, 1);
			m_neighborOffsets[15] = new Vector3I(1, 0, -1);
			m_neighborOffsets[16] = new Vector3I(-1, 0, 1);
			m_neighborOffsets[17] = new Vector3I(-1, 0, -1);
			m_neighborOffsets[18] = new Vector3I(1, 1, 1);
			m_neighborOffsets[19] = new Vector3I(1, 1, -1);
			m_neighborOffsets[20] = new Vector3I(1, -1, 1);
			m_neighborOffsets[21] = new Vector3I(1, -1, -1);
			m_neighborOffsets[22] = new Vector3I(-1, 1, 1);
			m_neighborOffsets[23] = new Vector3I(-1, 1, -1);
			m_neighborOffsets[24] = new Vector3I(-1, -1, 1);
			m_neighborOffsets[25] = new Vector3I(-1, -1, -1);
			GridCounter = 0;
		}

		public bool SwitchPower()
		{
			m_IsPowered.Value = !m_IsPowered.Value;
			MyDefinitionId typeId = MyResourceDistributorComponent.ElectricityId;
			GridSystems.ResourceDistributor.SetDataDirty(typeId);
			GridSystems.ResourceDistributor.RecomputeResourceDistribution(ref typeId);
			return m_IsPowered;
		}

		internal void NotifyMassPropertiesChanged()
		{
			this.OnMassPropertiesChanged.InvokeIfNotNull(this);
		}

		internal void NotifyBlockAdded(MySlimBlock block)
		{
			IntegrityMass += block.IntegrityMass;
			this.OnBlockAdded.InvokeIfNotNull(block);
			if (block.FatBlock != null)
			{
				this.OnFatBlockAdded.InvokeIfNotNull(block.FatBlock);
			}
			GridSystems.OnBlockAdded(block);
		}

		internal void NotifyBlockRemoved(MySlimBlock block)
		{
			IntegrityMass -= block.IntegrityMass;
			this.OnBlockRemoved.InvokeIfNotNull(block);
			if (block.FatBlock != null)
			{
				this.OnFatBlockRemoved.InvokeIfNotNull(block.FatBlock);
			}
			if (!MyEntities.IsClosingAll && MyVisualScriptLogicProvider.BlockDestroyed != null)
			{
				SingleKeyEntityNameGridNameEvent blockDestroyed = MyVisualScriptLogicProvider.BlockDestroyed;
				string entityName = ((block.FatBlock != null) ? block.FatBlock.Name : string.Empty);
<<<<<<< HEAD
				string name = base.Name;
=======
				string name = Name;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyObjectBuilderType typeId = block.BlockDefinition.Id.TypeId;
				blockDestroyed(entityName, name, typeId.ToString(), block.BlockDefinition.Id.SubtypeName);
			}
			MyCubeGrids.NotifyBlockDestroyed(this, block);
			GridSystems.OnBlockRemoved(block);
		}

		internal void NotifyBlockClosed(MySlimBlock block)
		{
			this.OnBlockClosed.InvokeIfNotNull(block);
			if (block.FatBlock != null)
			{
				this.OnFatBlockClosed.InvokeIfNotNull(block.FatBlock);
			}
		}

		internal void NotifyBlockIntegrityChanged(MySlimBlock block, bool handWelded)
		{
			this.OnBlockIntegrityChanged.InvokeIfNotNull(block);
			GridSystems.OnBlockIntegrityChanged(block);
			if (MyVisualScriptLogicProvider.BlockIntegrityChanged != null)
			{
				SingleKeyEntityNameGridNameEvent blockIntegrityChanged = MyVisualScriptLogicProvider.BlockIntegrityChanged;
				string entityName = ((block.FatBlock != null) ? block.FatBlock.Name : string.Empty);
<<<<<<< HEAD
				string name = base.Name;
=======
				string name = Name;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyObjectBuilderType typeId = block.BlockDefinition.Id.TypeId;
				blockIntegrityChanged(entityName, name, typeId.ToString(), block.BlockDefinition.Id.SubtypeName);
			}
			if (block.IsFullIntegrity)
			{
				MyCubeGrids.NotifyBlockFinished(this, block, handWelded);
			}
		}

		internal void NotifyBlockOwnershipChange()
		{
			if (this.OnBlockOwnershipChanged != null)
			{
				this.OnBlockOwnershipChanged(this);
			}
			GridSystems.OnBlockOwnershipChanged(this);
		}

		internal void NotifyIsStaticChanged(bool newIsStatic)
		{
			if (this.OnIsStaticChanged != null)
			{
				this.OnIsStaticChanged(newIsStatic);
			}
			if (this.OnStaticChanged != null)
			{
				this.OnStaticChanged(this, newIsStatic);
			}
		}

		public void RaiseGridChanged()
		{
			this.OnGridChanged.InvokeIfNotNull(this);
		}

		public void OnTerminalOpened()
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnGridChangedRPC);
		}

<<<<<<< HEAD
		[Event(null, 785)]
=======
		[Event(null, 740)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access)]
		private void OnGridChangedRPC()
		{
			RaiseGridChanged();
		}

		public bool HasMainCockpit()
		{
			return MainCockpit != null;
		}

		public bool IsMainCockpit(MyTerminalBlock cockpit)
		{
			return MainCockpit == cockpit;
		}

		public void SetMainCockpit(MyTerminalBlock cockpit)
		{
			MainCockpit = cockpit;
		}

		public bool HasMainRemoteControl()
		{
			return MainRemoteControl != null;
		}

		public bool IsMainRemoteControl(MyTerminalBlock remoteControl)
		{
			return MainRemoteControl == remoteControl;
		}

		public void SetMainRemoteControl(MyTerminalBlock remoteControl)
		{
			MainRemoteControl = remoteControl;
		}

		/// <summary>
		/// Return how much fat blocks defined by T is pressent in grid.
		/// </summary>
		/// <typeparam name="T">Type of Fatblock</typeparam>
		/// <returns></returns>
		public int GetFatBlockCount<T>() where T : MyCubeBlock
		{
			int num = 0;
			foreach (MyCubeBlock fatBlock in GetFatBlocks())
			{
				if (fatBlock is T)
				{
					num++;
				}
			}
			return num;
		}

		public MyCubeGrid()
			: this(MyCubeSize.Large)
		{
			GridScale = 1f;
			Render = new MyRenderComponentCubeGrid();
			Render.NeedsDraw = true;
			IsUnsupportedStation = false;
			base.Hierarchy.QueryAABBImpl = QueryAABB;
			base.Hierarchy.QuerySphereImpl = QuerySphere;
			base.Hierarchy.QueryLineImpl = QueryLine;
			base.Components.Add(new MyGridTargeting());
			SyncType = SyncHelpers.Compose(this);
			m_handBrakeSync.ValueChanged += delegate
			{
				HandBrakeChanged();
			};
			m_dampenersEnabled.ValueChanged += delegate
			{
				DampenersEnabledChanged();
			};
			m_contactPoint.ValueChanged += delegate
			{
				OnContactPointChanged();
			};
			m_markedAsTrash.ValueChanged += delegate
			{
				MarkedAsTrashChanged();
			};
			m_UpdateDirtyInternal = UpdateDirtyInternal;
			m_OnUpdateDirtyCompleted = OnUpdateDirtyCompleted;
		}

		private MyCubeGrid(MyCubeSize gridSize)
		{
			GridScale = 1f;
			GridSizeEnum = gridSize;
			GridSize = MyDefinitionManager.Static.GetCubeSize(gridSize);
			GridSizeHalf = GridSize / 2f;
			GridSizeHalfVector = new Vector3(GridSizeHalf);
			GridSizeQuarter = GridSize / 4f;
			GridSizeQuarterVector = new Vector3(GridSizeQuarter);
			GridSizeR = 1f / GridSize;
			base.NeedsUpdate = MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			Skeleton = new MyGridSkeleton();
			GridCounter++;
			AddDebugRenderComponent(new MyDebugRenderComponentCubeGrid(this));
			if (MyPerGameSettings.Destruction)
			{
				base.OnPhysicsChanged += delegate(MyEntity entity)
				{
					MyPhysics.RemoveDestructions(entity);
				};
			}
			if (MyFakes.ASSERT_CHANGES_IN_SIMULATION)
			{
				base.OnPhysicsChanged += delegate
				{
				};
				OnGridSplit += delegate
				{
				};
			}
		}

		private void M_IsPowered_ValueChanged(SyncBase obj)
		{
			if (this.IsPoweredChanged != null)
			{
				this.IsPoweredChanged(m_IsPowered);
			}
		}

		private void CreateSystems()
		{
			GridSystems = (MyCubeGridSystems)Activator.CreateInstance(m_gridSystemsType, this);
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			InitInternal(objectBuilder, rebuildGrid: true);
			m_isSolarOccluded.SetLocalValue(newValue: true);
		}

		[Conditional("DEBUG")]
		private void AssertNonPublicBlock(MyObjectBuilder_CubeBlock block)
		{
			MyCubeBlockDefinition blockDefinition;
			MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = UpgradeCubeBlock(block, out blockDefinition) as MyObjectBuilder_CompoundCubeBlock;
			if (myObjectBuilder_CompoundCubeBlock != null)
			{
				MyObjectBuilder_CubeBlock[] blocks = myObjectBuilder_CompoundCubeBlock.Blocks;
				for (int i = 0; i < blocks.Length; i++)
				{
					_ = blocks[i];
				}
			}
		}

		[Conditional("DEBUG")]
		private void AssertNonPublicBlocks(MyObjectBuilder_CubeGrid builder)
		{
			foreach (MyObjectBuilder_CubeBlock cubeBlock in builder.CubeBlocks)
			{
				_ = cubeBlock;
			}
		}

		private bool RemoveNonPublicBlock(MyObjectBuilder_CubeBlock block)
		{
			MyCubeBlockDefinition blockDefinition;
			MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = UpgradeCubeBlock(block, out blockDefinition) as MyObjectBuilder_CompoundCubeBlock;
			if (myObjectBuilder_CompoundCubeBlock != null)
			{
<<<<<<< HEAD
				myObjectBuilder_CompoundCubeBlock.Blocks = myObjectBuilder_CompoundCubeBlock.Blocks.Where((MyObjectBuilder_CubeBlock s) => !MyDefinitionManager.Static.TryGetCubeBlockDefinition(s.GetId(), out var def) || def.Public || def.IsGeneratedBlock).ToArray();
=======
				myObjectBuilder_CompoundCubeBlock.Blocks = Enumerable.ToArray<MyObjectBuilder_CubeBlock>(Enumerable.Where<MyObjectBuilder_CubeBlock>((IEnumerable<MyObjectBuilder_CubeBlock>)myObjectBuilder_CompoundCubeBlock.Blocks, (Func<MyObjectBuilder_CubeBlock, bool>)((MyObjectBuilder_CubeBlock s) => !MyDefinitionManager.Static.TryGetCubeBlockDefinition(s.GetId(), out var def) || def.Public || def.IsGeneratedBlock)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return myObjectBuilder_CompoundCubeBlock.Blocks.Length == 0;
			}
			if (blockDefinition != null && !blockDefinition.Public)
			{
				return true;
			}
			return false;
		}

		private void RemoveNonPublicBlocks(MyObjectBuilder_CubeGrid builder)
		{
			builder.CubeBlocks.RemoveAll((MyObjectBuilder_CubeBlock s) => RemoveNonPublicBlock(s));
		}

		private void InitInternal(MyObjectBuilder_EntityBase objectBuilder, bool rebuildGrid)
		{
			List<MyDefinitionId> list = new List<MyDefinitionId>();
			base.SyncFlag = true;
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = objectBuilder as MyObjectBuilder_CubeGrid;
			if (myObjectBuilder_CubeGrid != null)
			{
				GridSizeEnum = myObjectBuilder_CubeGrid.GridSizeEnum;
			}
			GridScale = MyDefinitionManager.Static.GetCubeSize(GridSizeEnum) / MyDefinitionManager.Static.GetCubeSizeOriginal(GridSizeEnum);
			base.Init(objectBuilder);
			Init(null, null, null, null);
<<<<<<< HEAD
			if (myObjectBuilder_CubeGrid != null)
			{
				m_destructibleBlocks.SetLocalValue(myObjectBuilder_CubeGrid.DestructibleBlocks);
				m_immune.SetLocalValue(myObjectBuilder_CubeGrid.Immune);
			}
=======
			m_destructibleBlocks.SetLocalValue(myObjectBuilder_CubeGrid.DestructibleBlocks);
			m_immune.SetLocalValue(myObjectBuilder_CubeGrid.Immune);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			_ = MyFakes.ASSERT_NON_PUBLIC_BLOCKS;
			if (MyFakes.REMOVE_NON_PUBLIC_BLOCKS)
			{
				RemoveNonPublicBlocks(myObjectBuilder_CubeGrid);
			}
			GridGeneralDamageModifier.SetLocalValue(1f);
			CreateSystems();
			if (myObjectBuilder_CubeGrid != null)
			{
				IsStatic = myObjectBuilder_CubeGrid.IsStatic;
				IsUnsupportedStation = myObjectBuilder_CubeGrid.IsUnsupportedStation;
				CreatePhysics = myObjectBuilder_CubeGrid.CreatePhysics;
				m_enableSmallToLargeConnections = myObjectBuilder_CubeGrid.EnableSmallToLargeConnections;
				GridSizeEnum = myObjectBuilder_CubeGrid.GridSizeEnum;
				Editable = myObjectBuilder_CubeGrid.Editable;
				m_IsPowered.Value = myObjectBuilder_CubeGrid.IsPowered;
				m_IsPowered.ValueChanged += M_IsPowered_ValueChanged;
				GridSystems.BeforeBlockDeserialization(myObjectBuilder_CubeGrid);
				m_cubes.Clear();
				m_cubeBlocks.Clear();
				m_fatBlocks.Clear();
				m_inventoryBlocks.Clear();
				if (myObjectBuilder_CubeGrid.DisplayName == null)
				{
					base.DisplayName = MakeCustomName();
				}
				else
				{
					base.DisplayName = myObjectBuilder_CubeGrid.DisplayName;
				}
				m_tmpOccupiedCockpits.Clear();
				if (Sync.IsServer)
				{
					GridPresenceTier = myObjectBuilder_CubeGrid.GridPresenceTier;
					if (Sync.IsDedicated)
					{
						PlayerPresenceTier = myObjectBuilder_CubeGrid.PlayerPresenceTier;
					}
					else
					{
						PlayerPresenceTier = MyUpdateTiersPlayerPresence.Normal;
					}
				}
				else
				{
					GridPresenceTier = MyUpdateTiersGridPresence.Normal;
					PlayerPresenceTier = MyUpdateTiersPlayerPresence.Normal;
				}
				for (int i = 0; i < myObjectBuilder_CubeGrid.CubeBlocks.Count; i++)
				{
					MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = myObjectBuilder_CubeGrid.CubeBlocks[i];
					if ((myObjectBuilder_CubeBlock.IntegrityPercent < 1.52590219E-05f || myObjectBuilder_CubeBlock.IntegrityPercent > 1f) && Sync.IsServer)
					{
						myObjectBuilder_CubeBlock.IntegrityPercent = MathHelper.Clamp(myObjectBuilder_CubeBlock.IntegrityPercent, 1.52590219E-05f, 1f);
					}
					MySlimBlock mySlimBlock = AddBlock(myObjectBuilder_CubeBlock, testMerge: false);
					if (mySlimBlock == null)
					{
						continue;
					}
					if (mySlimBlock.FatBlock is MyCompoundCubeBlock)
					{
						foreach (MySlimBlock block in (mySlimBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
						{
							if (!list.Contains(block.BlockDefinition.Id))
							{
								list.Add(block.BlockDefinition.Id);
							}
						}
					}
					else if (!list.Contains(mySlimBlock.BlockDefinition.Id))
					{
						list.Add(mySlimBlock.BlockDefinition.Id);
					}
					if (mySlimBlock.FatBlock is MyCockpit)
					{
						MyCockpit myCockpit = mySlimBlock.FatBlock as MyCockpit;
						if (myCockpit.Pilot != null)
						{
							m_tmpOccupiedCockpits.Add(myCockpit);
						}
					}
				}
				GridSystems.AfterBlockDeserialization();
				if (myObjectBuilder_CubeGrid.Skeleton != null)
				{
					Skeleton.Deserialize(myObjectBuilder_CubeGrid.Skeleton, GridSize, GridSize);
				}
				Render.RenderData.SetBasePositionHint(Min * GridSize - GridSize);
				if (rebuildGrid)
				{
					RebuildGrid();
				}
				foreach (MyObjectBuilder_BlockGroup blockGroup in myObjectBuilder_CubeGrid.BlockGroups)
				{
					AddGroup(blockGroup);
				}
				if (Physics != null)
				{
					Vector3 vector = myObjectBuilder_CubeGrid.LinearVelocity;
					Vector3 vector2 = myObjectBuilder_CubeGrid.AngularVelocity;
					Vector3.ClampToSphere(ref vector, Physics.GetMaxRelaxedLinearVelocity());
					Vector3.ClampToSphere(ref vector2, Physics.GetMaxRelaxedAngularVelocity());
					Physics.LinearVelocity = vector;
					Physics.AngularVelocity = vector2;
					if (!IsStatic)
					{
						Physics.Shape.BlocksConnectedToWorld.Clear();
					}
					SetInventoryMassDirty();
				}
				XSymmetryPlane = myObjectBuilder_CubeGrid.XMirroxPlane;
				YSymmetryPlane = myObjectBuilder_CubeGrid.YMirroxPlane;
				ZSymmetryPlane = myObjectBuilder_CubeGrid.ZMirroxPlane;
				XSymmetryOdd = myObjectBuilder_CubeGrid.XMirroxOdd;
				YSymmetryOdd = myObjectBuilder_CubeGrid.YMirroxOdd;
				ZSymmetryOdd = myObjectBuilder_CubeGrid.ZMirroxOdd;
				GridSystems.Init(myObjectBuilder_CubeGrid);
				if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
				{
					m_ownershipManager = new MyCubeGridOwnershipManager();
					m_ownershipManager.Init(this);
				}
				if (base.Hierarchy != null)
				{
					base.Hierarchy.OnChildRemoved += Hierarchy_OnChildRemoved;
				}
			}
			ScheduleDirtyRegion();
			Render.CastShadows = true;
			Render.NeedsResolveCastShadow = false;
			foreach (MyCockpit tmpOccupiedCockpit in m_tmpOccupiedCockpits)
			{
				tmpOccupiedCockpit.GiveControlToPilot();
			}
			m_tmpOccupiedCockpits.Clear();
			if (MyFakes.ENABLE_MULTIBLOCK_PART_IDS)
			{
				PrepareMultiBlockInfos();
			}
			m_isRespawnGrid.SetLocalValue(myObjectBuilder_CubeGrid.IsRespawnGrid);
			m_playedTime = myObjectBuilder_CubeGrid.playedTime;
			GridGeneralDamageModifier.SetLocalValue(myObjectBuilder_CubeGrid.GridGeneralDamageModifier);
			LocalCoordSystem = myObjectBuilder_CubeGrid.LocalCoordSys;
			m_dampenersEnabled.SetLocalValue(myObjectBuilder_CubeGrid.DampenersEnabled);
			if (myObjectBuilder_CubeGrid.TargetingTargets != null)
			{
				m_targetingList = myObjectBuilder_CubeGrid.TargetingTargets;
			}
			m_targetingListIsWhitelist = myObjectBuilder_CubeGrid.TargetingWhitelist;
			m_usesTargetingList = m_targetingList.Count > 0 || m_targetingListIsWhitelist;
			if (myObjectBuilder_CubeGrid.TargetingTargets != null)
			{
				m_targetingList = myObjectBuilder_CubeGrid.TargetingTargets;
			}
			m_targetingListIsWhitelist = myObjectBuilder_CubeGrid.TargetingWhitelist;
			m_usesTargetingList = m_targetingList.Count > 0 || m_targetingListIsWhitelist;
			if (BlocksPCU > 10000)
			{
				MyLog.Default.WriteLine($"Initialized large grid {base.DisplayName} {BlocksPCU} PCU");
			}
<<<<<<< HEAD
			Schedule(UpdateQueue.AfterSimulation, UpdateLinearVelocityAfterSimulation, 22);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void Hierarchy_OnChildRemoved(VRage.ModAPI.IMyEntity obj)
		{
			m_fatBlocks.Remove(obj as MyCubeBlock);
		}

		public void SetSolarOcclusion(bool isOccluded)
		{
			if (Sync.IsServer)
			{
				m_isSolarOccluded.Value = isOccluded;
			}
		}

		private static MyCubeGrid CreateGridForSplit(MyCubeGrid originalGrid, long newEntityId)
		{
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = MyObjectBuilderSerializer.CreateNewObject(typeof(MyObjectBuilder_CubeGrid)) as MyObjectBuilder_CubeGrid;
			if (myObjectBuilder_CubeGrid == null)
			{
				MyLog.Default.WriteLine("CreateForSplit builder shouldn't be null! Original Grid info: " + originalGrid.ToString());
				return null;
			}
			myObjectBuilder_CubeGrid.EntityId = newEntityId;
			myObjectBuilder_CubeGrid.GridSizeEnum = originalGrid.GridSizeEnum;
			myObjectBuilder_CubeGrid.IsStatic = originalGrid.IsStatic;
			myObjectBuilder_CubeGrid.PersistentFlags = originalGrid.Render.PersistentFlags;
			myObjectBuilder_CubeGrid.PositionAndOrientation = new MyPositionAndOrientation(originalGrid.WorldMatrix);
			myObjectBuilder_CubeGrid.DampenersEnabled = originalGrid.m_dampenersEnabled;
			myObjectBuilder_CubeGrid.IsPowered = originalGrid.m_IsPowered;
			myObjectBuilder_CubeGrid.IsUnsupportedStation = originalGrid.IsUnsupportedStation;
			myObjectBuilder_CubeGrid.GridPresenceTier = originalGrid.GridPresenceTier;
			myObjectBuilder_CubeGrid.PlayerPresenceTier = originalGrid.PlayerPresenceTier;
			MyCubeGrid myCubeGrid = MyEntities.CreateFromObjectBuilderNoinit(myObjectBuilder_CubeGrid) as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return null;
			}
			myCubeGrid.InitInternal(myObjectBuilder_CubeGrid, rebuildGrid: false);
			MyCubeGrid.OnSplitGridCreated.InvokeIfNotNull(myCubeGrid);
			return myCubeGrid;
		}

		public static void RemoveSplit(MyCubeGrid originalGrid, List<MySlimBlock> blocks, int offset, int count, bool sync = true)
		{
			for (int i = offset; i < offset + count; i++)
			{
				if (blocks.Count <= i)
				{
					continue;
				}
				MySlimBlock mySlimBlock = blocks[i];
				if (mySlimBlock != null)
				{
					if (mySlimBlock.FatBlock != null)
					{
						originalGrid.Hierarchy.RemoveChild(mySlimBlock.FatBlock);
					}
					originalGrid.RemoveBlockInternal(mySlimBlock, close: true, markDirtyDisconnects: false);
					originalGrid.Physics.AddDirtyBlock(mySlimBlock);
				}
			}
			originalGrid.RemoveEmptyBlockGroups();
			if (sync && Sync.IsServer)
			{
				originalGrid.AnnounceRemoveSplit(blocks);
			}
		}

		public MyCubeGrid SplitByPlane(PlaneD plane)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			m_tmpSlimBlocks.Clear();
			MyCubeGrid result = null;
			PlaneD plane2 = PlaneD.Transform(plane, base.PositionComp.WorldMatrixNormalizedInv);
			Enumerator<MySlimBlock> enumerator = GetBlocks().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					BoundingBoxD boundingBoxD = new BoundingBoxD(current.Min * GridSize, current.Max * GridSize);
					boundingBoxD.Inflate(GridSize / 2f);
					if (boundingBoxD.Intersects(plane2) == PlaneIntersectionType.Back)
					{
						m_tmpSlimBlocks.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			if (m_tmpSlimBlocks.Count != 0)
			{
				result = CreateSplit(this, m_tmpSlimBlocks, sync: true, 0L);
				m_tmpSlimBlocks.Clear();
			}
			return result;
		}

		public static MyCubeGrid CreateSplit(MyCubeGrid originalGrid, List<MySlimBlock> blocks, bool sync = true, long newEntityId = 0L)
		{
			MyCubeGrid myCubeGrid = CreateGridForSplit(originalGrid, newEntityId);
			if (myCubeGrid == null)
			{
				return null;
			}
<<<<<<< HEAD
			Vector3D centerOfMassWorld = originalGrid.Physics.CenterOfMassWorld;
=======
			Vector3 vector = originalGrid.Physics.CenterOfMassWorld;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyEntities.Add(myCubeGrid);
			MoveBlocks(originalGrid, myCubeGrid, blocks, 0, blocks.Count);
			myCubeGrid.RebuildGrid();
			if (!myCubeGrid.IsStatic)
			{
				myCubeGrid.Physics.UpdateMass();
			}
			if (originalGrid.IsStatic)
			{
				myCubeGrid.TestDynamic = MyTestDynamicReason.GridSplit;
				originalGrid.TestDynamic = MyTestDynamicReason.GridSplit;
			}
			myCubeGrid.Physics.AngularVelocity = originalGrid.Physics.AngularVelocity;
			myCubeGrid.Physics.LinearVelocity = originalGrid.Physics.GetVelocityAtPoint(myCubeGrid.Physics.CenterOfMassWorld);
			originalGrid.UpdatePhysicsShape();
			if (!originalGrid.IsStatic)
			{
				originalGrid.Physics.UpdateMass();
			}
<<<<<<< HEAD
			Vector3 vector = Vector3.Cross(originalGrid.Physics.AngularVelocity, originalGrid.Physics.CenterOfMassWorld - centerOfMassWorld);
			originalGrid.Physics.LinearVelocity = originalGrid.Physics.LinearVelocity + vector;
=======
			Vector3 vector2 = Vector3.Cross(originalGrid.Physics.AngularVelocity, originalGrid.Physics.CenterOfMassWorld - vector);
			originalGrid.Physics.LinearVelocity = originalGrid.Physics.LinearVelocity + vector2;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (originalGrid.OnGridSplit != null)
			{
				originalGrid.OnGridSplit(originalGrid, myCubeGrid);
			}
			if (sync)
			{
				if (!Sync.IsServer)
				{
					return myCubeGrid;
				}
				m_tmpBlockPositions.Clear();
				foreach (MySlimBlock block in blocks)
				{
					m_tmpBlockPositions.Add(block.Position);
				}
				MyMultiplayer.RemoveForClientIfIncomplete(originalGrid);
				MyMultiplayer.RaiseEvent(originalGrid, (MyCubeGrid x) => x.CreateSplit_Implementation, m_tmpBlockPositions, myCubeGrid.EntityId);
			}
			return myCubeGrid;
		}

<<<<<<< HEAD
		[Event(null, 1435)]
=======
		[Event(null, 1358)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		public void CreateSplit_Implementation(List<Vector3I> blocks, long newEntityId)
		{
			using (MyUtils.ReuseCollection(ref m_tmpBlockListReceive))
			{
				foreach (Vector3I block in blocks)
				{
					MySlimBlock cubeBlock = GetCubeBlock(block);
					if (cubeBlock == null)
					{
						MySandboxGame.Log.WriteLine("Block was null when trying to create a grid split. Desync?");
					}
					else
					{
						m_tmpBlockListReceive.Add(cubeBlock);
					}
				}
				CreateSplit(this, m_tmpBlockListReceive, sync: false, newEntityId);
			}
		}

		/// <summary>
		/// SplitBlocks list can contain null when received from network
		/// </summary>
		public static void CreateSplits(MyCubeGrid originalGrid, List<MySlimBlock> splitBlocks, List<MyDisconnectHelper.Group> groups, MyTestDisconnectsReason reason, bool sync = true)
		{
			if (originalGrid == null || originalGrid.Physics == null || groups == null || splitBlocks == null)
			{
				return;
			}
			Vector3D centerOfMassWorld = originalGrid.Physics.CenterOfMassWorld;
			try
			{
				if (MyCubeGridSmallToLargeConnection.Static != null)
				{
					MyCubeGridSmallToLargeConnection.Static.BeforeGridSplit_SmallToLargeGridConnectivity(originalGrid);
				}
				for (int i = 0; i < groups.Count; i++)
				{
					MyDisconnectHelper.Group group = groups[i];
					CreateSplitForGroup(originalGrid, splitBlocks, ref group);
					groups[i] = group;
				}
				originalGrid.UpdatePhysicsShape();
				foreach (MyCubeGrid tmpGrid in originalGrid.m_tmpGrids)
				{
					tmpGrid.RebuildGrid();
					if (originalGrid.IsStatic && !MySession.Static.Settings.StationVoxelSupport)
					{
						tmpGrid.TestDynamic = ((reason == MyTestDisconnectsReason.SplitBlock) ? MyTestDynamicReason.GridSplitByBlock : MyTestDynamicReason.GridSplit);
						originalGrid.TestDynamic = ((reason == MyTestDisconnectsReason.SplitBlock) ? MyTestDynamicReason.GridSplitByBlock : MyTestDynamicReason.GridSplit);
					}
					tmpGrid.Physics.AngularVelocity = originalGrid.Physics.AngularVelocity;
					tmpGrid.Physics.LinearVelocity = originalGrid.Physics.GetVelocityAtPoint(tmpGrid.Physics.CenterOfMassWorld);
					Interlocked.Increment(ref originalGrid.m_resolvingSplits);
					tmpGrid.UpdateDirty(delegate
					{
						Interlocked.Decrement(ref originalGrid.m_resolvingSplits);
					});
					tmpGrid.UpdateGravity();
				}
				Vector3 vector = Vector3.Cross(originalGrid.Physics.AngularVelocity, originalGrid.Physics.CenterOfMassWorld - centerOfMassWorld);
				originalGrid.Physics.LinearVelocity = originalGrid.Physics.LinearVelocity + vector;
				if (MyCubeGridSmallToLargeConnection.Static != null)
				{
					MyCubeGridSmallToLargeConnection.Static.AfterGridSplit_SmallToLargeGridConnectivity(originalGrid, originalGrid.m_tmpGrids);
				}
				Action<MyCubeGrid, MyCubeGrid> onGridSplit = originalGrid.OnGridSplit;
				if (onGridSplit != null)
				{
					foreach (MyCubeGrid tmpGrid2 in originalGrid.m_tmpGrids)
					{
						onGridSplit(originalGrid, tmpGrid2);
					}
				}
				foreach (MyCubeGrid tmpGrid3 in originalGrid.m_tmpGrids)
				{
					tmpGrid3.GridSystems.UpdatePower();
					if (tmpGrid3.GridSystems.ResourceDistributor != null)
					{
						tmpGrid3.GridSystems.ResourceDistributor.MarkForUpdate();
					}
				}
				if (!sync || !Sync.IsServer)
				{
					return;
				}
				MyMultiplayer.RemoveForClientIfIncomplete(originalGrid);
				m_tmpBlockPositions.Clear();
				foreach (MySlimBlock splitBlock in splitBlocks)
				{
					m_tmpBlockPositions.Add(splitBlock.Position);
				}
				foreach (MyCubeGrid tmpGrid4 in originalGrid.m_tmpGrids)
				{
					tmpGrid4.IsSplit = true;
					MyMultiplayer.ReplicateImmediatelly(MyExternalReplicable.FindByObject(tmpGrid4), MyExternalReplicable.FindByObject(originalGrid));
					tmpGrid4.IsSplit = false;
				}
				MyMultiplayer.RaiseEvent(originalGrid, (MyCubeGrid x) => x.CreateSplits_Implementation, m_tmpBlockPositions, groups);
			}
			finally
			{
				originalGrid.m_tmpGrids.Clear();
<<<<<<< HEAD
				originalGrid.GridSystems.ResourceDistributor?.UpdateBeforeSimulation();
			}
		}

		[Event(null, 1577)]
=======
			}
		}

		[Event(null, 1499)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		public void CreateSplits_Implementation(List<Vector3I> blocks, List<MyDisconnectHelper.Group> groups)
		{
			if (base.MarkedForClose)
			{
				return;
			}
			using (MyUtils.ReuseCollection(ref m_tmpBlockListReceive))
			{
				for (int i = 0; i < groups.Count; i++)
				{
					MyDisconnectHelper.Group value = groups[i];
					int num = value.BlockCount;
					for (int j = value.FirstBlockIndex; j < value.FirstBlockIndex + value.BlockCount; j++)
					{
						MySlimBlock cubeBlock = GetCubeBlock(blocks[j]);
						if (cubeBlock == null)
						{
							MySandboxGame.Log.WriteLine("Block was null when trying to create a grid split. Desync?");
							num--;
							if (num == 0)
							{
								value.IsValid = false;
							}
						}
						m_tmpBlockListReceive.Add(cubeBlock);
					}
					groups[i] = value;
				}
				CreateSplits(this, m_tmpBlockListReceive, groups, MyTestDisconnectsReason.BlockRemoved, sync: false);
			}
		}

		[Event(null, 1621)]
		[Reliable]
		[Client]
		public static void ShowMessageGridsRemovedWhilePasting()
		{
			ShowMessageGridsRemovedWhilePastingInternal();
		}

		public static void ShowMessageGridsRemovedWhilePastingInternal()
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.GridsRemovedWhilePasting)));
		}

		[Event(null, 1555)]
		[Reliable]
		[Client]
		public static void ShowMessageGridsRemovedWhilePasting()
		{
			ShowMessageGridsRemovedWhilePastingInternal();
		}

		public static void ShowMessageGridsRemovedWhilePastingInternal()
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.GridsRemovedWhilePasting)));
		}

		private static void CreateSplitForGroup(MyCubeGrid originalGrid, List<MySlimBlock> splitBlocks, ref MyDisconnectHelper.Group group)
		{
			bool flag = false;
			if (MySession.Static.SimplifiedSimulation && group.BlockCount < 10)
			{
				flag = true;
				for (int i = group.FirstBlockIndex; i < group.FirstBlockIndex + group.BlockCount; i++)
				{
					if (splitBlocks[i].FatBlock != null)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				group.IsValid = false;
			}
			else
			{
				if (!originalGrid.IsStatic && Sync.IsServer && group.IsValid)
				{
					int num = 0;
					for (int j = group.FirstBlockIndex; j < group.FirstBlockIndex + group.BlockCount; j++)
					{
						if (MyDisconnectHelper.IsDestroyedInVoxels(splitBlocks[j]))
						{
							num++;
							if ((float)num / (float)group.BlockCount > 0.4f)
							{
								group.IsValid = false;
								break;
							}
						}
					}
				}
				group.IsValid = group.IsValid && CanHavePhysics(splitBlocks, group.FirstBlockIndex, group.BlockCount) && HasStandAloneBlocks(splitBlocks, group.FirstBlockIndex, group.BlockCount);
				if (group.BlockCount == 1 && splitBlocks.Count > group.FirstBlockIndex && splitBlocks[group.FirstBlockIndex] != null)
				{
					MySlimBlock mySlimBlock = splitBlocks[group.FirstBlockIndex];
					if (mySlimBlock.FatBlock is MyFracturedBlock)
					{
						group.IsValid = false;
						if (Sync.IsServer)
						{
							MyDestructionHelper.CreateFracturePiece(mySlimBlock.FatBlock as MyFracturedBlock, sync: true);
						}
					}
					else if (mySlimBlock.FatBlock != null && mySlimBlock.FatBlock.Components.Has<MyFractureComponentBase>())
					{
						group.IsValid = false;
						if (Sync.IsServer)
						{
							MyFractureComponentCubeBlock fractureComponent = mySlimBlock.GetFractureComponent();
							if (fractureComponent != null)
							{
								MyDestructionHelper.CreateFracturePiece(fractureComponent, sync: true);
							}
						}
					}
					else if (mySlimBlock.FatBlock is MyCompoundCubeBlock)
					{
						MyCompoundCubeBlock myCompoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
						bool flag2 = true;
						foreach (MySlimBlock block in myCompoundCubeBlock.GetBlocks())
						{
							flag2 &= block.FatBlock.Components.Has<MyFractureComponentBase>();
							if (!flag2)
							{
								break;
							}
						}
						if (flag2)
						{
							group.IsValid = false;
							if (Sync.IsServer)
							{
								foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
								{
									MyFractureComponentCubeBlock fractureComponent2 = block2.GetFractureComponent();
									if (fractureComponent2 != null)
									{
										MyDestructionHelper.CreateFracturePiece(fractureComponent2, sync: true);
									}
								}
							}
						}
					}
				}
			}
			if (group.IsValid)
			{
				MyCubeGrid myCubeGrid = CreateGridForSplit(originalGrid, group.EntityId);
				if (myCubeGrid != null)
				{
					originalGrid.m_tmpGrids.Add(myCubeGrid);
					MoveBlocks(originalGrid, myCubeGrid, splitBlocks, group.FirstBlockIndex, group.BlockCount);
					myCubeGrid.SetInventoryMassDirty();
					myCubeGrid.Render.FadeIn = false;
					myCubeGrid.RebuildGrid();
					MyEntities.Add(myCubeGrid);
					group.EntityId = myCubeGrid.EntityId;
					if (myCubeGrid.IsStatic && Sync.IsServer)
					{
						MatrixD tranform = myCubeGrid.WorldMatrix;
						bool flag3 = MyCoordinateSystem.Static.IsLocalCoordSysExist(ref tranform, myCubeGrid.GridSize);
						if (myCubeGrid.GridSizeEnum == MyCubeSize.Large)
						{
							if (flag3)
							{
								MyCoordinateSystem.Static.RegisterCubeGrid(myCubeGrid);
							}
							else
							{
								MyCoordinateSystem.Static.CreateCoordSys(myCubeGrid, MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter, sync: true);
							}
						}
					}
				}
				else
				{
					group.IsValid = false;
				}
			}
			if (!group.IsValid)
			{
				RemoveSplit(originalGrid, splitBlocks, group.FirstBlockIndex, group.BlockCount, sync: false);
			}
		}

		private void AddGroup(MyObjectBuilder_BlockGroup groupBuilder)
		{
			if (groupBuilder.Blocks.Count != 0)
			{
				MyBlockGroup myBlockGroup = new MyBlockGroup();
				myBlockGroup.Init(this, groupBuilder);
				BlockGroups.Add(myBlockGroup);
			}
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = (MyObjectBuilder_CubeGrid)base.GetObjectBuilder(copy);
			GetObjectBuilderInternal(myObjectBuilder_CubeGrid, copy);
			return myObjectBuilder_CubeGrid;
		}

		private void GetObjectBuilderInternal(MyObjectBuilder_CubeGrid ob, bool copy)
		{
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			ob.GridSizeEnum = GridSizeEnum;
			if (ob.Skeleton == null)
			{
				ob.Skeleton = new List<BoneInfo>();
			}
			ob.Skeleton.Clear();
			Skeleton.Serialize(ob.Skeleton, GridSize, this);
			ob.IsStatic = IsStatic;
			ob.IsUnsupportedStation = IsUnsupportedStation;
			ob.Editable = Editable;
			ob.IsPowered = m_IsPowered;
			ob.CubeBlocks.Clear();
			Enumerator<MySlimBlock> enumerator = m_cubeBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = null;
					myObjectBuilder_CubeBlock = ((!copy) ? current.GetObjectBuilder() : current.GetCopyObjectBuilder());
					if (myObjectBuilder_CubeBlock != null)
					{
						ob.CubeBlocks.Add(myObjectBuilder_CubeBlock);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			ob.PersistentFlags = Render.PersistentFlags;
			if (Physics != null)
			{
				ob.LinearVelocity = Physics.LinearVelocity;
				ob.AngularVelocity = Physics.AngularVelocity;
			}
			ob.XMirroxPlane = XSymmetryPlane;
			ob.YMirroxPlane = YSymmetryPlane;
			ob.ZMirroxPlane = ZSymmetryPlane;
			ob.XMirroxOdd = XSymmetryOdd;
			ob.YMirroxOdd = YSymmetryOdd;
			ob.ZMirroxOdd = ZSymmetryOdd;
			if (copy)
			{
				ob.Name = null;
			}
			ob.BlockGroups.Clear();
			foreach (MyBlockGroup blockGroup in BlockGroups)
			{
				ob.BlockGroups.Add(blockGroup.GetObjectBuilder());
			}
			ob.DisplayName = base.DisplayName;
			ob.DestructibleBlocks = DestructibleBlocks;
			ob.Immune = Immune;
			ob.IsRespawnGrid = IsRespawnGrid;
			ob.playedTime = m_playedTime;
			ob.GridGeneralDamageModifier = GridGeneralDamageModifier;
			ob.LocalCoordSys = LocalCoordSystem;
			ob.TargetingWhitelist = m_targetingListIsWhitelist;
			ob.TargetingTargets = m_targetingList;
			ob.GridPresenceTier = GridPresenceTier;
			ob.PlayerPresenceTier = PlayerPresenceTier;
			GridSystems.GetObjectBuilder(ob);
		}

		internal void HavokSystemIDChanged(int id)
		{
			this.OnHavokSystemIDChanged.InvokeIfNotNull(id);
		}

		private void UpdatePhysicsShape()
		{
			Physics.UpdateShape();
		}

		public List<HkShape> GetShapesFromPosition(Vector3I pos)
		{
			return Physics.GetShapesFromPosition(pos);
		}

		private void UpdateGravity()
		{
			if (!IsStatic && Physics != null && Physics.Enabled && !Physics.IsWelded)
			{
				if (Physics.DisableGravity <= 0)
				{
					RecalculateGravity();
				}
				else
				{
					Physics.DisableGravity--;
				}
				if (!Physics.IsWelded && !Physics.RigidBody.Gravity.Equals(m_gravity, 0.01f))
				{
					Physics.Gravity = m_gravity;
					ActivatePhysics();
				}
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			UpdateGravity();
			base.UpdateOnceBeforeFrame();
			if (MyFakes.ENABLE_GRID_SYSTEM_UPDATE || MyFakes.ENABLE_GRID_SYSTEM_ONCE_BEFORE_FRAME_UPDATE)
			{
				GridSystems.UpdateOnceBeforeFrame();
			}
			ActivatePhysics();
		}

		public void CheckPredictionFlagScheduling()
		{
			if (!IsStatic && !ForceDisablePrediction && GridSystems?.ControlSystem?.GetShipController()?.TopGrid == this)
			{
				Schedule(UpdateQueue.BeforeSimulation, UpdatePredictionFlag, 2, parallel: true);
				return;
			}
			DeSchedule(UpdateQueue.BeforeSimulation, UpdatePredictionFlag);
			UpdatePredictionFlag();
		}

		public void UpdatePredictionFlag()
		{
			if (!base.InScene)
			{
				return;
			}
			bool flag = false;
			IsClientPredictedCar = false;
			if (!IsStatic && !ForceDisablePrediction && AllowPrediction)
			{
				MyCubeGrid root = MyGridPhysicalHierarchy.Static.GetRoot(this);
				if (root == this)
				{
					if ((!Sync.IsServer && MySession.Static.TopMostControlledEntity == this) || (Sync.IsServer && Sync.Players.GetControllingPlayer(this) != null))
					{
						if (!MyGridPhysicalHierarchy.Static.HasChildren(this) && !MyFixedGrids.IsRooted(this))
						{
							flag = true;
							if (Physics.PredictedContactsCounter > PREDICTION_SWITCH_MIN_COUNTER)
							{
								if (Physics.AnyPredictedContactEntities())
								{
									flag = false;
								}
								else if (Physics.PredictedContactLastTime + MyTimeSpan.FromSeconds(PREDICTION_SWITCH_TIME) < MySandboxGame.Static.SimulationTime)
								{
									Physics.PredictedContactsCounter = 0;
								}
							}
						}
						else if (MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CAR)
						{
							bool car = true;
							MyGridPhysicalHierarchy.Static.ApplyOnChildren(this, delegate(MyCubeGrid child)
							{
								if (MyGridPhysicalHierarchy.Static.GetEntityConnectingToParent(child) is MyMotorSuspension)
								{
									child.IsClientPredictedWheel = false;
									foreach (MyCubeBlock fatBlock in child.GetFatBlocks())
									{
										if (fatBlock is MyWheel)
										{
											child.IsClientPredictedWheel = true;
											break;
										}
									}
									if (!child.IsClientPredictedWheel)
									{
										car = false;
									}
								}
								else
								{
									car = false;
								}
							});
							flag = car;
							IsClientPredictedCar = car;
						}
					}
				}
				else if (root != this)
				{
					flag = root.IsClientPredicted;
				}
			}
			bool num = IsClientPredicted != flag;
			IsClientPredicted = flag;
			if (num)
			{
				MyEntities.InvokeLater(Physics.UpdateConstraintsForceDisable);
			}
		}

		public void ClientPredictionStaticCheck()
		{
			if (!Sync.IsServer && Physics != null && !IsStatic && IsClientPredicted == Physics.IsStatic)
			{
				Physics.ConvertToDynamic(GridSizeEnum == MyCubeSize.Large, IsClientPredicted);
				UpdateGravity();
			}
		}

		protected static float GetLineWidthForGizmo(IMyGizmoDrawableObject block, BoundingBox box)
		{
			float num = 100f;
			foreach (Vector3 corner in box.Corners)
			{
				num = (float)Math.Min(num, Math.Abs(MySector.MainCamera.GetDistanceFromPoint(Vector3.Transform(block.GetPositionInGrid() + corner, block.GetWorldMatrix()))));
<<<<<<< HEAD
			}
			Vector3 vector = box.Max - box.Min;
			float num2 = MathHelper.Max(1f, MathHelper.Min(MathHelper.Min(vector.X, vector.Y), vector.Z));
			return num * 0.002f / num2;
		}

		public bool IsGizmoDrawingEnabled()
		{
			if (!ShowSenzorGizmos && !ShowGravityGizmos)
			{
				return ShowAntennaGizmos;
			}
			return true;
		}

		public override void PrepareForDraw()
		{
			base.PrepareForDraw();
			GridSystems.PrepareForDraw();
			if (IsGizmoDrawingEnabled())
			{
				foreach (MySlimBlock cubeBlock in m_cubeBlocks)
				{
					if (cubeBlock.FatBlock is IMyGizmoDrawableObject)
					{
						DrawObjectGizmo(cubeBlock);
					}
				}
			}
			if (!NeedsPerFrameDraw)
			{
				Render.NeedsDraw = false;
			}
		}

		public void StartReplay()
		{
			Schedule(UpdateQueue.BeforeSimulation, UpdateReplay, 27);
		}

		public void StopReplay()
		{
			DeSchedule(UpdateQueue.BeforeSimulation, UpdateReplay);
		}

		private void UpdateReplay()
		{
			if (MySessionComponentReplay.Static.IsEntityBeingReplayed(base.EntityId, out var perFrameData))
			{
=======
			}
			Vector3 vector = box.Max - box.Min;
			float num2 = MathHelper.Max(1f, MathHelper.Min(MathHelper.Min(vector.X, vector.Y), vector.Z));
			return num * 0.002f / num2;
		}

		public bool IsGizmoDrawingEnabled()
		{
			if (!ShowSenzorGizmos && !ShowGravityGizmos)
			{
				return ShowAntennaGizmos;
			}
			return true;
		}

		public override void PrepareForDraw()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			base.PrepareForDraw();
			GridSystems.PrepareForDraw();
			if (IsGizmoDrawingEnabled())
			{
				Enumerator<MySlimBlock> enumerator = m_cubeBlocks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current = enumerator.get_Current();
						if (current.FatBlock is IMyGizmoDrawableObject)
						{
							DrawObjectGizmo(current);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (!NeedsPerFrameDraw)
			{
				Render.NeedsDraw = false;
			}
		}

		public void StartReplay()
		{
			Schedule(UpdateQueue.BeforeSimulation, UpdateReplay, 27);
		}

		public void StopReplay()
		{
			DeSchedule(UpdateQueue.BeforeSimulation, UpdateReplay);
		}

		private void UpdateReplay()
		{
			if (MySessionComponentReplay.Static.IsEntityBeingReplayed(base.EntityId, out var perFrameData))
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (perFrameData.MovementData.HasValue && !IsStatic && base.InScene)
				{
					MyShipController shipController = GridSystems.ControlSystem.GetShipController();
					if (shipController != null)
					{
						SerializableVector3 moveVector = perFrameData.MovementData.Value.MoveVector;
						Vector2 rotationIndicator = new Vector2(perFrameData.MovementData.Value.RotateVector.X, perFrameData.MovementData.Value.RotateVector.Y);
						float z = perFrameData.MovementData.Value.RotateVector.Z;
						shipController.MoveAndRotate(moveVector, rotationIndicator, z);
					}
				}
				if (perFrameData.SwitchWeaponData.HasValue)
				{
					MyShipController shipController2 = GridSystems.ControlSystem.GetShipController();
					if (shipController2 != null && perFrameData.SwitchWeaponData.Value.WeaponDefinition.HasValue && !perFrameData.SwitchWeaponData.Value.WeaponDefinition.Value.TypeId.IsNull)
					{
						shipController2.SwitchToWeapon(perFrameData.SwitchWeaponData.Value.WeaponDefinition.Value);
					}
				}
				if (perFrameData.ShootData.HasValue)
				{
					MyShipController shipController3 = GridSystems.ControlSystem.GetShipController();
					if (shipController3 != null)
					{
						if (perFrameData.ShootData.Value.Begin)
						{
							shipController3.BeginShoot((MyShootActionEnum)perFrameData.ShootData.Value.ShootAction);
						}
						else
						{
							shipController3.EndShoot((MyShootActionEnum)perFrameData.ShootData.Value.ShootAction);
						}
					}
				}
				if (perFrameData.ControlSwitchesData.HasValue)
				{
					MyShipController shipController4 = GridSystems.ControlSystem.GetShipController();
					if (shipController4 != null)
					{
						if (perFrameData.ControlSwitchesData.Value.SwitchDamping)
						{
							shipController4.SwitchDamping();
						}
						if (perFrameData.ControlSwitchesData.Value.SwitchLandingGears)
						{
							shipController4.SwitchLandingGears();
						}
						if (perFrameData.ControlSwitchesData.Value.SwitchLights)
						{
							shipController4.SwitchLights();
						}
						if (perFrameData.ControlSwitchesData.Value.SwitchReactors)
						{
							shipController4.SwitchReactors();
						}
						if (perFrameData.ControlSwitchesData.Value.SwitchThrusts)
						{
							shipController4.SwitchThrusts();
						}
					}
				}
				if (!perFrameData.UseData.HasValue)
				{
					return;
				}
				MyShipController shipController5 = GridSystems.ControlSystem.GetShipController();
				if (shipController5 != null)
				{
					if (perFrameData.UseData.Value.Use)
<<<<<<< HEAD
					{
						shipController5.Use();
					}
					else if (perFrameData.UseData.Value.UseContinues)
					{
						shipController5.UseContinues();
					}
					else if (perFrameData.UseData.Value.UseFinished)
					{
=======
					{
						shipController5.Use();
					}
					else if (perFrameData.UseData.Value.UseContinues)
					{
						shipController5.UseContinues();
					}
					else if (perFrameData.UseData.Value.UseFinished)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						shipController5.UseFinished();
					}
				}
			}
			else
			{
				DeSchedule(UpdateQueue.BeforeSimulation, UpdateReplay);
			}
		}

		private static void DrawObjectGizmo(MySlimBlock block)
		{
			IMyGizmoDrawableObject myGizmoDrawableObject = block.FatBlock as IMyGizmoDrawableObject;
			if (!myGizmoDrawableObject.CanBeDrawn())
			{
				return;
			}
			Color color = myGizmoDrawableObject.GetGizmoColor();
			MatrixD worldMatrix = myGizmoDrawableObject.GetWorldMatrix();
			BoundingBox? boundingBox = myGizmoDrawableObject.GetBoundingBox();
			if (boundingBox.HasValue)
			{
				float lineWidthForGizmo = GetLineWidthForGizmo(myGizmoDrawableObject, boundingBox.Value);
				BoundingBoxD localbox = boundingBox.Value;
				MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, MySimpleObjectRasterizer.SolidAndWireframe, 1, lineWidthForGizmo);
				return;
			}
			float radius = myGizmoDrawableObject.GetRadius();
			MySector.MainCamera.GetDistanceFromPoint(worldMatrix.Translation);
			float value = (float)((double)radius - MySector.MainCamera.GetDistanceFromPoint(worldMatrix.Translation));
			float lineThickness = 0.002f * Math.Min(100f, Math.Abs(value));
			int customViewProjectionMatrix = -1;
			MySimpleObjectDraw.DrawTransparentSphere(ref worldMatrix, radius, ref color, MySimpleObjectRasterizer.SolidAndWireframe, 20, null, null, lineThickness, customViewProjectionMatrix);
			if (myGizmoDrawableObject.EnableLongDrawDistance() && MyFakes.ENABLE_LONG_DISTANCE_GIZMO_DRAWING)
			{
				MyBillboardViewProjection billboardViewProjection = default(MyBillboardViewProjection);
				billboardViewProjection.CameraPosition = MySector.MainCamera.Position;
				billboardViewProjection.ViewAtZero = default(Matrix);
				billboardViewProjection.Viewport = MySector.MainCamera.Viewport;
				float aspectRatio = billboardViewProjection.Viewport.Width / billboardViewProjection.Viewport.Height;
				billboardViewProjection.Projection = Matrix.CreatePerspectiveFieldOfView(MySector.MainCamera.FieldOfView, aspectRatio, 1f, 100f);
				billboardViewProjection.Projection.M33 = -1f;
				billboardViewProjection.Projection.M34 = -1f;
				billboardViewProjection.Projection.M43 = 0f;
				billboardViewProjection.Projection.M44 = 0f;
				customViewProjectionMatrix = 10;
				MyRenderProxy.AddBillboardViewProjection(customViewProjectionMatrix, billboardViewProjection);
				MySimpleObjectDraw.DrawTransparentSphere(ref worldMatrix, radius, ref color, MySimpleObjectRasterizer.SolidAndWireframe, 20, null, null, lineThickness, customViewProjectionMatrix);
			}
		}

		public override void UpdateBeforeSimulation10()
		{
			MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateBeforeSimulation10");
			base.UpdateBeforeSimulation10();
			if (MyFakes.ENABLE_GRID_SYSTEM_UPDATE)
			{
				GridSystems.UpdateBeforeSimulation10();
			}
			MySimpleProfiler.End("UpdateBeforeSimulation10");
		}

		public override void UpdateBeforeSimulation100()
		{
			MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateBeforeSimulation100");
			base.UpdateBeforeSimulation100();
			if (MyFakes.ENABLE_GRID_SYSTEM_UPDATE)
			{
				GridSystems.UpdateBeforeSimulation100();
			}
			if (Physics != null)
			{
				Physics.LowSimulationQuality = !MySession.Static.HighSimulationQuality;
			}
			MySimpleProfiler.End("UpdateBeforeSimulation100");
		}

		internal void SetInventoryMassDirty()
		{
			if (!m_inventoryMassDirty)
			{
				m_inventoryMassDirty = true;
				Schedule(UpdateQueue.OnceAfterSimulation, UpdateInventoryMass, 6, parallel: true);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Raises global event when inventory was changed on the grid (including logical group). Works only for server.
		/// </summary>
		/// <param name="inventory">changed inventory</param>
		/// <param name="processGroup">true if logical group should be notified</param>
		internal void RaiseInventoryChanged(MyInventoryBase inventory, bool processGroup = true)
		{
=======
		internal void RaiseInventoryChanged(MyInventoryBase inventory, bool processGroup = true)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!Sync.IsServer)
			{
				return;
			}
			OnAnyBlockInventoryChanged.InvokeIfNotNull(inventory);
			if (!processGroup)
			{
				return;
			}
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(this);
			if (group == null)
			{
				return;
			}
<<<<<<< HEAD
			foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
			{
				if (node.NodeData != this && node.NodeData != null)
				{
					node.NodeData.RaiseInventoryChanged(inventory, processGroup: false);
				}
			}
=======
			Enumerator<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node> enumerator = group.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node current = enumerator.get_Current();
					if (current.NodeData != this && current.NodeData != null)
					{
						current.NodeData.RaiseInventoryChanged(inventory, processGroup: false);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void UpdateInventoryMass()
		{
			if (m_inventoryMassDirty)
			{
				m_inventoryMassDirty = false;
				if (Physics != null)
				{
					Physics.Shape.UpdateMassFromInventories(m_inventoryBlocks, Physics);
				}
			}
		}

		public float GetCurrentMass(GridLinkTypeEnum linking = GridLinkTypeEnum.Physical)
		{
			float baseMass;
			float physicalMass;
			return GetCurrentMass(out baseMass, out physicalMass, linking);
		}

		public float GetCurrentMass(out float baseMass, out float physicalMass, GridLinkTypeEnum linking = GridLinkTypeEnum.Physical)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			baseMass = 0f;
			physicalMass = 0f;
			float num = 0f;
			MyGroupsBase<MyCubeGrid> groups = MyCubeGridGroups.Static.GetGroups(linking);
			if (groups != null)
			{
				float blocksInventorySizeMultiplier = MySession.Static.Settings.BlocksInventorySizeMultiplier;
				Enumerator<MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> enumerator = group.Nodes.GetEnumerator();
				try
				{
<<<<<<< HEAD
					foreach (MyCubeGrid groupNode in groups.GetGroupNodes(this))
					{
						if (groupNode == null || groupNode.Physics == null || groupNode.Physics.Shape == null)
						{
							continue;
						}
						HkMassProperties? massProperties = groupNode.Physics.Shape.MassProperties;
						HkMassProperties? baseMassProperties = groupNode.Physics.Shape.BaseMassProperties;
						if (IsStatic || !massProperties.HasValue || !baseMassProperties.HasValue)
						{
							continue;
						}
						float num2 = massProperties.Value.Mass;
						float num3 = baseMassProperties.Value.Mass;
						foreach (MyCockpit occupiedBlock in groupNode.OccupiedBlocks)
						{
							MyCharacter pilot = occupiedBlock.Pilot;
							if (pilot != null)
							{
								float baseMass2 = pilot.BaseMass;
								float num4 = pilot.CurrentMass - baseMass2;
								num3 += baseMass2;
								num2 += num4 / blocksInventorySizeMultiplier;
							}
						}
						float num5 = (num2 - num3) * blocksInventorySizeMultiplier;
						baseMass += num3;
						num += num3 + num5;
						if (groupNode.Physics.WeldInfo.Parent == null || groupNode.Physics.WeldInfo.Parent == groupNode.Physics)
						{
							physicalMass += groupNode.Physics.Mass;
=======
					while (enumerator.MoveNext())
					{
						MyCubeGrid nodeData = enumerator.get_Current().NodeData;
						if (nodeData == null || nodeData.Physics == null || nodeData.Physics.Shape == null)
						{
							continue;
						}
						HkMassProperties? massProperties = nodeData.Physics.Shape.MassProperties;
						HkMassProperties? baseMassProperties = nodeData.Physics.Shape.BaseMassProperties;
						if (IsStatic || !massProperties.HasValue || !baseMassProperties.HasValue)
						{
							continue;
						}
						float num2 = massProperties.Value.Mass;
						float num3 = baseMassProperties.Value.Mass;
						Enumerator<MyCockpit> enumerator2 = nodeData.OccupiedBlocks.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								MyCharacter pilot = enumerator2.get_Current().Pilot;
								if (pilot != null)
								{
									float baseMass2 = pilot.BaseMass;
									float num4 = pilot.CurrentMass - baseMass2;
									num3 += baseMass2;
									num2 += num4 / blocksInventorySizeMultiplier;
								}
							}
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
						float num5 = (num2 - num3) * blocksInventorySizeMultiplier;
						baseMass += num3;
						num += num3 + num5;
						if (nodeData.Physics.WeldInfo.Parent == null || nodeData.Physics.WeldInfo.Parent == nodeData.Physics)
						{
							physicalMass += nodeData.Physics.Mass;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					return num;
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			return num;
		}

		public override void UpdateAfterSimulation100()
		{
			MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateAfterSimulation100");
			base.UpdateAfterSimulation100();
			UpdateGravity();
			if (MyFakes.ENABLE_BOUNDINGBOX_SHRINKING && m_boundsDirty && MySandboxGame.TotalSimulationTimeInMilliseconds - m_lastUpdatedDirtyBounds > 30000)
			{
				Vector3I min = m_min;
				Vector3I max = m_max;
				RecalcBounds();
				m_boundsDirty = false;
				m_lastUpdatedDirtyBounds = MySandboxGame.TotalSimulationTimeInMilliseconds;
				if (GridSystems.GasSystem != null && (min != m_min || max != m_max))
				{
					GridSystems.GasSystem.OnCubeGridShrinked();
				}
			}
			if (MyFakes.ENABLE_GRID_SYSTEM_UPDATE)
			{
				GridSystems.UpdateAfterSimulation100();
			}
			MySimpleProfiler.End("UpdateAfterSimulation100");
		}

		internal void MarkForDraw()
		{
			if (base.Closed || !NeedsPerFrameDraw || Render.NeedsDraw)
			{
				return;
			}
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.Closed)
				{
					Render.NeedsDraw = true;
				}
			}, "MarkForDraw()");
		}

		private void CreateFractureBlockComponent(MyFractureComponentBase.Info info)
		{
			if (info.Entity.MarkedForClose)
			{
				return;
			}
			MyFractureComponentCubeBlock myFractureComponentCubeBlock = new MyFractureComponentCubeBlock();
			info.Entity.Components.Add((MyFractureComponentBase)myFractureComponentCubeBlock);
			myFractureComponentCubeBlock.SetShape(info.Shape, info.Compound);
			if (!Sync.IsServer)
			{
				return;
			}
			MyCubeBlock myCubeBlock = info.Entity as MyCubeBlock;
			if (myCubeBlock == null)
			{
				return;
			}
			MyCubeGridSmallToLargeConnection.Static.RemoveBlockSmallToLargeConnection(myCubeBlock.SlimBlock);
			MySlimBlock cubeBlock = myCubeBlock.CubeGrid.GetCubeBlock(myCubeBlock.Position);
			MyCompoundCubeBlock myCompoundCubeBlock = ((cubeBlock != null) ? (cubeBlock.FatBlock as MyCompoundCubeBlock) : null);
			if (myCompoundCubeBlock != null)
			{
				ushort? blockId = myCompoundCubeBlock.GetBlockId(myCubeBlock.SlimBlock);
				if (blockId.HasValue)
				{
					MyObjectBuilder_FractureComponentBase component = (MyObjectBuilder_FractureComponentBase)myFractureComponentCubeBlock.Serialize();
					MySyncDestructions.CreateFractureComponent(myCubeBlock.CubeGrid.EntityId, myCubeBlock.Position, blockId.Value, component);
				}
			}
			else
			{
				MyObjectBuilder_FractureComponentBase component2 = (MyObjectBuilder_FractureComponentBase)myFractureComponentCubeBlock.Serialize();
				MySyncDestructions.CreateFractureComponent(myCubeBlock.CubeGrid.EntityId, myCubeBlock.Position, ushort.MaxValue, component2);
			}
			myCubeBlock.SlimBlock.ApplyDestructionDamage(myFractureComponentCubeBlock.GetIntegrityRatioFromFracturedPieceCounts());
		}

		internal void RemoveGroup(MyBlockGroup group)
		{
			BlockGroups.Remove(group);
			GridSystems.RemoveGroup(group);
		}

		internal void RemoveGroupByName(string name)
		{
			MyBlockGroup myBlockGroup = BlockGroups.Find((MyBlockGroup g) => g.Name.CompareTo(name) == 0);
			if (myBlockGroup != null)
			{
				BlockGroups.Remove(myBlockGroup);
				GridSystems.RemoveGroup(myBlockGroup);
			}
		}

		internal void AddGroup(MyBlockGroup group, bool unionSameNameGroups = true)
		{
			foreach (MyBlockGroup blockGroup in BlockGroups)
			{
				if (blockGroup.Name.CompareTo(group.Name) == 0)
				{
					BlockGroups.Remove(blockGroup);
<<<<<<< HEAD
					if (unionSameNameGroups)
					{
						group.Blocks.UnionWith(blockGroup.Blocks);
					}
=======
					group.Blocks.UnionWith((IEnumerable<MyTerminalBlock>)blockGroup.Blocks);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					break;
				}
			}
			BlockGroups.Add(group);
			GridSystems.AddGroup(group);
		}

		internal void OnAddedToGroup(MyGridLogicalGroupData group)
		{
			GridSystems.OnAddedToGroup(group);
			if (this.AddedToLogicalGroup != null)
			{
				this.AddedToLogicalGroup(group);
			}
		}

		internal void OnRemovedFromGroup(MyGridLogicalGroupData group)
		{
			GridSystems.OnRemovedFromGroup(group);
			if (this.RemovedFromLogicalGroup != null)
			{
				this.RemovedFromLogicalGroup();
			}
		}

		internal void OnConnectivityChanged(GridLinkTypeEnum type)
		{
			this.OnConnectionChanged?.Invoke(this, type);
		}

		/// <summary>
		/// Checks if 2 grids are connected
		/// </summary>
		/// <param name="testGrid">Grid to test</param>
		/// <param name="type">Type of connection</param>
		/// <returns></returns>
		public bool IsConnectedTo(MyCubeGrid testGrid, GridLinkTypeEnum type)
		{
			return MyCubeGridGroups.Static.GetGroups(type).HasSameGroup(this, testGrid);
		}

		/// <summary>
		/// Get all connected grids to current grid
		/// </summary>
		/// <param name="type">Type of connection</param>
		/// <param name="list">Cache list, you can keep it null, then new List will be allocated</param>
		/// <returns></returns>
		public List<MyCubeGrid> GetConnectedGrids(GridLinkTypeEnum type, List<MyCubeGrid> list = null)
		{
			if (list == null)
			{
				list = new List<MyCubeGrid>();
			}
			MyCubeGridGroups.Static.GetGroups(type).GetGroupNodes(this, list);
			return list;
		}

		internal void OnAddedToGroup(MyGridPhysicalGroupData groupData)
		{
			GridSystems.OnAddedToGroup(groupData);
		}

		internal void OnRemovedFromGroup(MyGridPhysicalGroupData group)
		{
			GridSystems.OnRemovedFromGroup(group);
		}

		/// <summary>
		/// Reduces the control of the current group if the current grid is the one that the player is sitting in
		/// </summary>
		private void TryReduceGroupControl()
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			MyEntityController entityController = Sync.Players.GetEntityController(this);
			if (entityController == null || !(entityController.ControlledEntity is MyCockpit))
			{
				return;
			}
			MyCockpit myCockpit = entityController.ControlledEntity as MyCockpit;
			if (myCockpit.CubeGrid != this)
<<<<<<< HEAD
			{
				return;
			}
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(this);
			if (group == null)
			{
				return;
			}
			foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
			{
				if (node.NodeData != this)
				{
					if (MySession.Static == null)
					{
						MyLog.Default.WriteLine("MySession.Static was null");
=======
			{
				return;
			}
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(this);
			if (group == null)
			{
				return;
			}
			Enumerator<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node> enumerator = group.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node current = enumerator.get_Current();
					if (current.NodeData != this)
					{
						if (MySession.Static == null)
						{
							MyLog.Default.WriteLine("MySession.Static was null");
						}
						else if (MySession.Static.SyncLayer == null)
						{
							MyLog.Default.WriteLine("MySession.Static.SyncLayer was null");
						}
						else if (Sync.Clients == null)
						{
							MyLog.Default.WriteLine("Sync.Clients was null");
						}
						Sync.Players.TryReduceControl(myCockpit, current.NodeData);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					else if (MySession.Static.SyncLayer == null)
					{
						MyLog.Default.WriteLine("MySession.Static.SyncLayer was null");
					}
					else if (Sync.Clients == null)
					{
						MyLog.Default.WriteLine("Sync.Clients was null");
					}
					Sync.Players.TryReduceControl(myCockpit, node.NodeData);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			MyCubeGridGroups.Static.AddNode(GridLinkTypeEnum.Logical, this);
			MyCubeGridGroups.Static.AddNode(GridLinkTypeEnum.Physical, this);
			MyCubeGridGroups.Static.AddNode(GridLinkTypeEnum.Mechanical, this);
			MyCubeGridGroups.Static.AddNode(GridLinkTypeEnum.Electrical, this);
			if (!base.IsPreview)
			{
				MyGridPhysicalHierarchy.Static.AddNode(this);
			}
			if (IsStatic)
			{
				MyFixedGrids.MarkGridRoot(this);
			}
			RecalculateGravity();
			UpdateGravity();
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			if (!MyEntities.IsClosingAll)
			{
				MyCubeGridGroups.Static.RemoveNode(GridLinkTypeEnum.Physical, this);
				MyCubeGridGroups.Static.RemoveNode(GridLinkTypeEnum.Logical, this);
				MyCubeGridGroups.Static.RemoveNode(GridLinkTypeEnum.Mechanical, this);
				MyCubeGridGroups.Static.RemoveNode(GridLinkTypeEnum.Electrical, this);
			}
			if (!base.IsPreview)
			{
				MyGridPhysicalHierarchy.Static.RemoveNode(this);
			}
			MyFixedGrids.UnmarkGridRoot(this);
			ReleaseMerginGrids();
			if (m_unsafeBlocks.get_Count() > 0)
			{
				MyUnsafeGridsSessionComponent.UnregisterGrid(this);
			}
		}

		protected override void BeforeDelete()
		{
			SendRemovedBlocks();
			SendRemovedBlocksWithIds();
			RemoveAuthorshipAll();
			m_cubes.Clear();
			m_targetingList.Clear();
			if (MyFakes.ENABLE_NEW_SOUNDS && MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS_QUICK_UPDATE)
			{
				MyEntity3DSoundEmitter.UpdateEntityEmitters(removeUnused: true, updatePlaying: false, updateNotPlaying: false);
			}
			MyEntities.Remove(this);
			UnregisterBlocksBeforeClose();
			base.BeforeDelete();
			GridCounter--;
			m_cubeBlocks.Clear();
			m_fatBlocks.Clear();
<<<<<<< HEAD
			m_occupiedBlocks.Clear();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void UnregisterBlocks(List<MyCubeBlock> cubeBlocks)
		{
			foreach (MyCubeBlock cubeBlock in cubeBlocks)
			{
				GridSystems.UnregisterFromSystems(cubeBlock);
			}
		}

		private void UnregisterBlocksBeforeClose()
		{
			GridSystems.BeforeGridClose();
			UnregisterBlocks(m_fatBlocks.List);
			GridSystems.AfterGridClose();
		}

		public override bool GetIntersectionWithLine(ref LineD line, out MyIntersectionResultLineTriangleEx? tri, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			bool intersectionWithLine = GetIntersectionWithLine(ref line, ref m_hitInfoTmp, flags);
			if (intersectionWithLine)
			{
				tri = m_hitInfoTmp.Triangle;
			}
			else
			{
				tri = null;
			}
			m_hitInfoTmp = null;
			return intersectionWithLine;
		}

		public bool GetIntersectionWithLine(ref LineD line, ref MyCubeGridHitInfo info, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			if (info == null)
			{
				info = new MyCubeGridHitInfo();
			}
			info.Reset();
			if (base.IsPreview)
			{
				return false;
			}
			if (Projector != null)
			{
				return false;
			}
			RayCastCells(line.From, line.To, m_cacheRayCastCells);
			if (m_cacheRayCastCells.Count == 0)
			{
				return false;
			}
			foreach (Vector3I cacheRayCastCell in m_cacheRayCastCells)
			{
				if (m_cubes.ContainsKey(cacheRayCastCell))
				{
<<<<<<< HEAD
					MyCube myCube = m_cubes[cacheRayCastCell];
=======
					MyCube myCube = m_cubes.get_Item(cacheRayCastCell);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					GetBlockIntersection(myCube, ref line, flags, out var t, out var cubePartIndex);
					if (t.HasValue)
					{
						info.Position = myCube.CubeBlock.Position;
						info.Triangle = t.Value;
						info.CubePartIndex = cubePartIndex;
						info.Triangle.UserObject = myCube;
						return true;
					}
				}
			}
			return false;
		}

		internal bool GetIntersectionWithLine(ref LineD line, out MyIntersectionResultLineTriangleEx? t, out MySlimBlock slimBlock, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			t = null;
			slimBlock = null;
			RayCastCells(line.From, line.To, m_cacheRayCastCells);
			if (m_cacheRayCastCells.Count == 0)
			{
				return false;
			}
			foreach (Vector3I cacheRayCastCell in m_cacheRayCastCells)
			{
				if (m_cubes.ContainsKey(cacheRayCastCell))
				{
<<<<<<< HEAD
					MyCube myCube = m_cubes[cacheRayCastCell];
=======
					MyCube myCube = m_cubes.get_Item(cacheRayCastCell);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					GetBlockIntersection(myCube, ref line, flags, out t, out var _);
					if (t.HasValue)
					{
						slimBlock = myCube.CubeBlock;
						break;
					}
				}
			}
			if (slimBlock != null && slimBlock.FatBlock is MyCompoundCubeBlock)
			{
				ListReader<MySlimBlock> blocks = (slimBlock.FatBlock as MyCompoundCubeBlock).GetBlocks();
				double num = double.MaxValue;
				MySlimBlock mySlimBlock = null;
				for (int i = 0; i < blocks.Count; i++)
				{
					MySlimBlock mySlimBlock2 = blocks.ItemAt(i);
					if (mySlimBlock2.FatBlock.GetIntersectionWithLine(ref line, out var t2, IntersectionFlags.ALL_TRIANGLES) && t2.HasValue)
					{
						double num2 = (t2.Value.IntersectionPointInWorldSpace - line.From).LengthSquared();
						if (num2 < num)
						{
							num = num2;
							mySlimBlock = mySlimBlock2;
						}
					}
				}
				slimBlock = mySlimBlock;
			}
			return t.HasValue;
		}

		public override bool GetIntersectionWithSphere(ref BoundingSphereD sphere)
		{
			try
			{
				BoundingBoxD boundingBoxD = new BoundingBoxD(sphere.Center - new Vector3D(sphere.Radius), sphere.Center + new Vector3D(sphere.Radius));
				MatrixD m = MatrixD.Invert(base.WorldMatrix);
				boundingBoxD = boundingBoxD.TransformFast(ref m);
				Vector3D min = boundingBoxD.Min;
				Vector3D max = boundingBoxD.Max;
				Vector3I value = new Vector3I((int)Math.Round(min.X * (double)GridSizeR), (int)Math.Round(min.Y * (double)GridSizeR), (int)Math.Round(min.Z * (double)GridSizeR));
				Vector3I value2 = new Vector3I((int)Math.Round(max.X * (double)GridSizeR), (int)Math.Round(max.Y * (double)GridSizeR), (int)Math.Round(max.Z * (double)GridSizeR));
				Vector3I vector3I = Vector3I.Min(value, value2);
				Vector3I vector3I2 = Vector3I.Max(value, value2);
				for (int i = vector3I.X; i <= vector3I2.X; i++)
				{
					for (int j = vector3I.Y; j <= vector3I2.Y; j++)
					{
						for (int k = vector3I.Z; k <= vector3I2.Z; k++)
						{
							if (!m_cubes.ContainsKey(new Vector3I(i, j, k)))
<<<<<<< HEAD
							{
								continue;
							}
							MyCube myCube = m_cubes[new Vector3I(i, j, k)];
							if (myCube.CubeBlock.FatBlock == null || myCube.CubeBlock.FatBlock.Model == null)
							{
								if (myCube.CubeBlock.BlockDefinition.CubeDefinition.CubeTopology == MyCubeTopology.Box)
								{
									return true;
								}
								MyCubePart[] parts = myCube.Parts;
								foreach (MyCubePart obj in parts)
								{
									MatrixD m2 = obj.InstanceData.LocalMatrix * base.WorldMatrix;
									Matrix m3 = Matrix.Invert(m2);
									Vector3 center = Vector3D.Transform(matrix: (MatrixD)m3, position: sphere.Center);
									BoundingSphere localSphere = new BoundingSphere(center, (float)sphere.Radius);
=======
							{
								continue;
							}
							MyCube myCube = m_cubes.get_Item(new Vector3I(i, j, k));
							if (myCube.CubeBlock.FatBlock == null || myCube.CubeBlock.FatBlock.Model == null)
							{
								if (myCube.CubeBlock.BlockDefinition.CubeDefinition.CubeTopology == MyCubeTopology.Box)
								{
									return true;
								}
								MyCubePart[] parts = myCube.Parts;
								foreach (MyCubePart obj in parts)
								{
									MatrixD m2 = obj.InstanceData.LocalMatrix * base.WorldMatrix;
									Matrix m3 = Matrix.Invert(m2);
									Vector3D vector3D = Vector3D.Transform(matrix: (MatrixD)m3, position: sphere.Center);
									BoundingSphere localSphere = new BoundingSphere(vector3D, (float)sphere.Radius);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
									if (obj.Model.GetTrianglePruningStructure().GetIntersectionWithSphere(ref localSphere))
									{
										return true;
									}
								}
							}
							else
							{
								MatrixD m4 = myCube.CubeBlock.FatBlock.WorldMatrix;
								Matrix m3 = Matrix.Invert(m4);
								MatrixD matrix2 = m3;
								_ = (BoundingSphereD)new BoundingSphere(Vector3D.Transform(sphere.Center, matrix2), (float)sphere.Radius);
								bool intersectionWithSphere = myCube.CubeBlock.FatBlock.Model.GetTrianglePruningStructure().GetIntersectionWithSphere(myCube.CubeBlock.FatBlock, ref sphere);
								if (intersectionWithSphere)
								{
									return intersectionWithSphere;
								}
							}
						}
					}
				}
				return false;
			}
			finally
			{
			}
		}

		public override string ToString()
		{
			string text = (IsStatic ? "S" : "D");
			string text2 = GridSizeEnum.ToString();
			return "Grid_" + text + "_" + text2 + "_" + m_cubeBlocks.get_Count() + " {" + base.EntityId.ToString("X8") + "}";
		}

		public Vector3I WorldToGridInteger(Vector3D coords)
		{
			return Vector3I.Round(Vector3D.Transform(coords, base.PositionComp.WorldMatrixNormalizedInv) * GridSizeR);
		}

		public Vector3D WorldToGridScaledLocal(Vector3D coords)
		{
			return Vector3D.Transform(coords, base.PositionComp.WorldMatrixNormalizedInv) * GridSizeR;
		}

		public static Vector3D GridIntegerToWorld(float gridSize, Vector3I gridCoords, MatrixD worldMatrix)
		{
			return Vector3D.Transform((Vector3D)(Vector3)gridCoords * (double)gridSize, worldMatrix);
		}

		public Vector3D GridIntegerToWorld(Vector3I gridCoords)
		{
			return GridIntegerToWorld(GridSize, gridCoords, base.WorldMatrix);
		}

		public Vector3D GridIntegerToWorld(Vector3D gridCoords)
		{
			return Vector3D.Transform(gridCoords * GridSize, base.WorldMatrix);
		}

		public Vector3I LocalToGridInteger(Vector3 localCoords)
		{
			localCoords *= GridSizeR;
			return Vector3I.Round(localCoords);
		}

		public bool CanAddCubes(Vector3I min, Vector3I max)
		{
			Vector3I next = min;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref min, ref max);
			while (vector3I_RangeIterator.IsValid())
			{
				if (m_cubes.ContainsKey(next))
				{
					return false;
				}
				vector3I_RangeIterator.GetNext(out next);
			}
			return true;
		}

		public bool CanAddCubes(Vector3I min, Vector3I max, MyBlockOrientation? orientation, MyCubeBlockDefinition definition)
		{
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && definition != null)
			{
				Vector3I next = min;
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref min, ref max);
				while (vector3I_RangeIterator.IsValid())
				{
					if (!CanAddCube(next, orientation, definition))
					{
						return false;
					}
					vector3I_RangeIterator.GetNext(out next);
				}
				return true;
			}
			return CanAddCubes(min, max);
		}

		public bool CanAddCube(Vector3I pos, MyBlockOrientation? orientation, MyCubeBlockDefinition definition, bool ignoreSame = false)
		{
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && definition != null)
			{
				if (!CubeExists(pos))
				{
					return true;
				}
				MySlimBlock cubeBlock = GetCubeBlock(pos);
				if (cubeBlock != null)
				{
					MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
					if (myCompoundCubeBlock != null)
					{
						return myCompoundCubeBlock.CanAddBlock(definition, orientation, 0, ignoreSame);
					}
				}
				return false;
			}
			return !CubeExists(pos);
		}

		public void ClearSymmetries()
		{
			XSymmetryPlane = null;
			YSymmetryPlane = null;
			ZSymmetryPlane = null;
		}

		public bool IsTouchingAnyNeighbor(Vector3I min, Vector3I max)
		{
			Vector3I min2 = min;
			min2.X--;
			Vector3I max2 = max;
			max2.X = min2.X;
			if (!CanAddCubes(min2, max2))
			{
				return true;
			}
			Vector3I min3 = min;
			min3.Y--;
			Vector3I max3 = max;
			max3.Y = min3.Y;
			if (!CanAddCubes(min3, max3))
			{
				return true;
			}
			Vector3I min4 = min;
			min4.Z--;
			Vector3I max4 = max;
			max4.Z = min4.Z;
			if (!CanAddCubes(min4, max4))
			{
				return true;
			}
			Vector3I max5 = max;
			max5.X++;
			Vector3I min5 = min;
			min5.X = max5.X;
			if (!CanAddCubes(min5, max5))
			{
				return true;
			}
			Vector3I max6 = max;
			max6.Y++;
			Vector3I min6 = min;
			min6.Y = max6.Y;
			if (!CanAddCubes(min6, max6))
			{
				return true;
			}
			Vector3I max7 = max;
			max7.Z++;
			Vector3I min7 = min;
			min7.Z = max7.Z;
			if (!CanAddCubes(min7, max7))
			{
				return true;
			}
			return false;
		}

		public bool CanPlaceBlock(Vector3I min, Vector3I max, MyBlockOrientation orientation, MyCubeBlockDefinition definition, ulong placingPlayer = 0uL, int? ignoreMultiblockId = null, bool ignoreFracturedPieces = false, bool isProjection = false)
		{
			MyGridPlacementSettings gridSettings = MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(GridSizeEnum, IsStatic);
			return CanPlaceBlock(min, max, orientation, definition, ref gridSettings, placingPlayer, ignoreMultiblockId, ignoreFracturedPieces, isProjection);
		}

		public bool CanPlaceBlock(Vector3I min, Vector3I max, MyBlockOrientation orientation, MyCubeBlockDefinition definition, ref MyGridPlacementSettings gridSettings, ulong placingPlayer = 0uL, int? ignoreMultiblockId = null, bool ignoreFracturedPieces = false, bool isProjection = false)
		{
			if (!CanAddCubes(min, max, orientation, definition))
			{
				return false;
			}
			if (MyFakes.ENABLE_MULTIBLOCKS && MyFakes.ENABLE_MULTIBLOCK_CONSTRUCTION && !CanAddOtherBlockInMultiBlock(min, max, orientation, definition, ignoreMultiblockId))
			{
				return false;
			}
			return TestPlacementAreaCube(this, ref gridSettings, min, max, orientation, definition, placingPlayer, this, ignoreFracturedPieces, isProjection);
		}

		/// <summary>
		/// Determines whether newly placed blocks still fit within block limits set by server
		/// </summary>
		private bool IsWithinWorldLimits(long ownerID, int blocksToBuild, int pcu, string name)
		{
			string failedBlockType;
			return MySession.Static.IsWithinWorldLimits(out failedBlockType, ownerID, name, pcu, blocksToBuild, BlocksCount) == MySession.LimitResult.Passed;
		}

		public void SetCubeDirty(Vector3I pos)
		{
			m_dirtyRegion.AddCube(pos);
			MySlimBlock cubeBlock = GetCubeBlock(pos);
			if (cubeBlock != null)
			{
				Physics.AddDirtyBlock(cubeBlock);
			}
			ScheduleDirtyRegion();
		}

		public void SetBlockDirty(MySlimBlock cubeBlock)
		{
			Vector3I next = cubeBlock.Min;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref cubeBlock.Min, ref cubeBlock.Max);
			while (vector3I_RangeIterator.IsValid())
			{
				m_dirtyRegion.AddCube(next);
				vector3I_RangeIterator.GetNext(out next);
			}
			ScheduleDirtyRegion();
		}

		public void DebugDrawRange(Vector3I min, Vector3I max)
		{
			Vector3I next = min;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref min, ref max);
			while (vector3I_RangeIterator.IsValid())
			{
				_ = next + 1;
				MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(next * GridSize, GridSizeHalfVector, Quaternion.Identity);
				obb.Transform(base.WorldMatrix);
				MyRenderProxy.DebugDrawOBB(obb, Color.White, 0.5f, depthRead: true, smooth: false);
				vector3I_RangeIterator.GetNext(out next);
			}
		}

		public void DebugDrawPositions(List<Vector3I> positions)
		{
			foreach (Vector3I position in positions)
			{
				_ = position + 1;
				MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(position * GridSize, GridSizeHalfVector, Quaternion.Identity);
				obb.Transform(base.WorldMatrix);
				MyRenderProxy.DebugDrawOBB(obb, Color.White.ToVector3(), 0.5f, depthRead: true, smooth: false);
			}
		}

		private MyObjectBuilder_CubeBlock UpgradeCubeBlock(MyObjectBuilder_CubeBlock block, out MyCubeBlockDefinition blockDefinition)
		{
			MyDefinitionId id = block.GetId();
			if (MyFakes.ENABLE_COMPOUND_BLOCKS)
			{
				if (block is MyObjectBuilder_CompoundCubeBlock)
				{
					MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = block as MyObjectBuilder_CompoundCubeBlock;
					blockDefinition = MyCompoundCubeBlock.GetCompoundCubeBlockDefinition();
					if (blockDefinition == null)
					{
						return null;
					}
					if (myObjectBuilder_CompoundCubeBlock.Blocks.Length == 1)
					{
						MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = myObjectBuilder_CompoundCubeBlock.Blocks[0];
						if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(myObjectBuilder_CubeBlock.GetId(), out var blockDefinition2) && !MyCompoundCubeBlock.IsCompoundEnabled(blockDefinition2))
						{
							blockDefinition = blockDefinition2;
							return myObjectBuilder_CubeBlock;
						}
					}
					return block;
				}
				if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(id, out blockDefinition) && MyCompoundCubeBlock.IsCompoundEnabled(blockDefinition))
				{
					MyObjectBuilder_CompoundCubeBlock result = MyCompoundCubeBlock.CreateBuilder(block);
					MyCubeBlockDefinition compoundCubeBlockDefinition = MyCompoundCubeBlock.GetCompoundCubeBlockDefinition();
					if (compoundCubeBlockDefinition != null)
					{
						blockDefinition = compoundCubeBlockDefinition;
						return result;
					}
				}
			}
			if (block is MyObjectBuilder_Ladder)
			{
				MyObjectBuilder_Passage myObjectBuilder_Passage = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Passage>(block.SubtypeName);
				myObjectBuilder_Passage.BlockOrientation = block.BlockOrientation;
				myObjectBuilder_Passage.BuildPercent = block.BuildPercent;
				myObjectBuilder_Passage.EntityId = block.EntityId;
				myObjectBuilder_Passage.IntegrityPercent = block.IntegrityPercent;
				myObjectBuilder_Passage.Min = block.Min;
				blockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(new MyDefinitionId(typeof(MyObjectBuilder_Passage), block.SubtypeId));
				block = myObjectBuilder_Passage;
				return block;
			}
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock2 = block;
			string[] array = new string[7] { "Red", "Yellow", "Blue", "Green", "Black", "White", "Gray" };
			Vector3[] array2 = new Vector3[7]
			{
				MyRenderComponentBase.OldRedToHSV,
				MyRenderComponentBase.OldYellowToHSV,
				MyRenderComponentBase.OldBlueToHSV,
				MyRenderComponentBase.OldGreenToHSV,
				MyRenderComponentBase.OldBlackToHSV,
				MyRenderComponentBase.OldWhiteToHSV,
				MyRenderComponentBase.OldGrayToHSV
			};
			if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(id, out blockDefinition))
			{
				myObjectBuilder_CubeBlock2 = FindDefinitionUpgrade(block, out blockDefinition);
				if (myObjectBuilder_CubeBlock2 == null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (id.SubtypeName.EndsWith(array[i], StringComparison.InvariantCultureIgnoreCase))
						{
							string subtypeName = id.SubtypeName.Substring(0, id.SubtypeName.Length - array[i].Length);
							MyDefinitionId defId = new MyDefinitionId(id.TypeId, subtypeName);
							if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out blockDefinition))
							{
								myObjectBuilder_CubeBlock2 = block;
								myObjectBuilder_CubeBlock2.ColorMaskHSV = array2[i];
								myObjectBuilder_CubeBlock2.SubtypeName = subtypeName;
								return myObjectBuilder_CubeBlock2;
							}
						}
					}
				}
				if (myObjectBuilder_CubeBlock2 == null)
				{
					return null;
				}
			}
			return myObjectBuilder_CubeBlock2;
		}

		private MySlimBlock AddBlock(MyObjectBuilder_CubeBlock objectBuilder, bool testMerge)
		{
			try
			{
				if (Skeleton == null)
				{
					Skeleton = new MyGridSkeleton();
				}
				objectBuilder = UpgradeCubeBlock(objectBuilder, out var blockDefinition);
				if (objectBuilder == null)
				{
					return null;
				}
				try
				{
					return AddCubeBlock(objectBuilder, testMerge, blockDefinition);
				}
<<<<<<< HEAD
				catch (DuplicateIdException ex)
				{
=======
				objectBuilder = UpgradeCubeBlock(objectBuilder, out var blockDefinition);
				if (objectBuilder == null)
				{
					return null;
				}
				try
				{
					return AddCubeBlock(objectBuilder, testMerge, blockDefinition);
				}
				catch (DuplicateIdException ex)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					string msg = "ERROR while adding cube " + blockDefinition.DisplayNameText.ToString() + ": " + ex.ToString();
					MyLog.Default.WriteLine(msg);
					return null;
				}
			}
			finally
			{
			}
		}

		private MySlimBlock AddCubeBlock(MyObjectBuilder_CubeBlock objectBuilder, bool testMerge, MyCubeBlockDefinition blockDefinition)
		{
			Vector3I min = objectBuilder.Min;
			MySlimBlock.ComputeMax(blockDefinition, objectBuilder.BlockOrientation, ref min, out var max);
			if (!CanAddCubes(min, max))
			{
				return null;
			}
			object obj = MyCubeBlockFactory.CreateCubeBlock(objectBuilder);
			MySlimBlock mySlimBlock = obj as MySlimBlock;
			if (mySlimBlock == null)
			{
				mySlimBlock = new MySlimBlock();
			}
			if (!mySlimBlock.Init(objectBuilder, this, obj as MyCubeBlock))
			{
				return null;
			}
			if (mySlimBlock.FatBlock is MyCompoundCubeBlock && (mySlimBlock.FatBlock as MyCompoundCubeBlock).GetBlocksCount() == 0)
			{
				return null;
			}
			if (mySlimBlock.FatBlock != null)
			{
				mySlimBlock.FatBlock.Render.FadeIn = Render.FadeIn;
				mySlimBlock.FatBlock.HookMultiplayer();
			}
			mySlimBlock.AddNeighbours();
			BoundsInclude(mySlimBlock);
			if (mySlimBlock.FatBlock != null)
			{
				base.Hierarchy.AddChild(mySlimBlock.FatBlock);
				GridSystems.RegisterInSystems(mySlimBlock.FatBlock);
				if (mySlimBlock.FatBlock.Render.NeedsDrawFromParent)
				{
					m_blocksForDraw.Add(mySlimBlock.FatBlock);
					mySlimBlock.FatBlock.Render.SetVisibilityUpdates(state: true);
				}
				MyObjectBuilderType typeId = mySlimBlock.BlockDefinition.Id.TypeId;
				if (typeId != typeof(MyObjectBuilder_CubeBlock))
				{
					if (!BlocksCounters.ContainsKey(typeId))
					{
						BlocksCounters.Add(typeId, 0);
					}
					BlocksCounters[typeId]++;
				}
			}
			m_cubeBlocks.Add(mySlimBlock);
			if (mySlimBlock.FatBlock != null)
			{
				m_fatBlocks.Add(mySlimBlock.FatBlock);
			}
			if (!m_colorStatistics.ContainsKey(mySlimBlock.ColorMaskHSV))
			{
				m_colorStatistics.Add(mySlimBlock.ColorMaskHSV, 0);
			}
			m_colorStatistics[mySlimBlock.ColorMaskHSV]++;
			((MyBlockOrientation)objectBuilder.BlockOrientation).GetMatrix(out var result);
			MyCubeGridDefinitions.GetRotatedBlockSize(blockDefinition, ref result, out var _);
			Vector3I normal = blockDefinition.Center;
			Vector3I.TransformNormal(ref normal, ref result, out var _);
			bool flag = true;
			Vector3I pos = mySlimBlock.Min;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref mySlimBlock.Min, ref mySlimBlock.Max);
			while (vector3I_RangeIterator.IsValid())
			{
				flag &= AddCube(mySlimBlock, ref pos, result, blockDefinition);
				vector3I_RangeIterator.GetNext(out pos);
			}
			if (mySlimBlock.BlockDefinition.IsStandAlone)
			{
				m_standAloneBlockCount++;
			}
			if (Physics != null)
			{
				Physics.AddBlock(mySlimBlock);
			}
			FixSkeleton(mySlimBlock);
			mySlimBlock.AddAuthorship();
			if (MyFakes.ENABLE_MULTIBLOCK_PART_IDS)
			{
				AddMultiBlockInfo(mySlimBlock);
			}
			if (testMerge)
			{
				MyCubeGrid myCubeGrid = DetectMerge(mySlimBlock);
				if (myCubeGrid != null && myCubeGrid != this)
				{
					mySlimBlock = myCubeGrid.GetCubeBlock(mySlimBlock.Position);
				}
				else
				{
					NotifyBlockAdded(mySlimBlock);
				}
			}
			else
			{
				NotifyBlockAdded(mySlimBlock);
			}
			m_PCU += (mySlimBlock.ComponentStack.IsFunctional ? mySlimBlock.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST);
			if (mySlimBlock.FatBlock is MyReactor)
			{
				NumberOfReactors++;
			}
			MarkForDraw();
			return mySlimBlock;
		}

		public void FixSkeleton(MySlimBlock cubeBlock, bool simplePhysicsUpdateOnly = false)
		{
			float maxBoneError = MyGridSkeleton.GetMaxBoneError(GridSize);
			maxBoneError *= maxBoneError;
			Vector3I end = (cubeBlock.Min + Vector3I.One) * 2;
			Vector3I start = cubeBlock.Min * 2;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
			while (vector3I_RangeIterator.IsValid())
			{
				Vector3 definitionOffsetWithNeighbours = Skeleton.GetDefinitionOffsetWithNeighbours(cubeBlock.Min, start, this);
				if (definitionOffsetWithNeighbours.LengthSquared() < maxBoneError)
				{
					Skeleton.Bones.Remove<Vector3I, Vector3>(start);
				}
				else
				{
					Skeleton.Bones.set_Item(start, definitionOffsetWithNeighbours);
				}
				vector3I_RangeIterator.GetNext(out start);
			}
			if (cubeBlock.BlockDefinition.Skeleton == null || cubeBlock.BlockDefinition.Skeleton.Count <= 0 || Physics == null)
			{
				return;
			}
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					for (int k = -1; k <= 1; k++)
					{
						if (simplePhysicsUpdateOnly)
						{
							MySlimBlock cubeBlock2 = GetCubeBlock(new Vector3I(i, j, k));
							if (cubeBlock2 != null && cubeBlock2.FatBlock != null)
							{
								cubeBlock2.FatBlock.RaisePhysicsChanged();
							}
						}
						else
						{
							SetCubeDirty(new Vector3I(i, j, k) + cubeBlock.Min);
						}
					}
				}
			}
			if (cubeBlock.FatBlock != null)
			{
				cubeBlock.FatBlock.RaisePhysicsChanged();
			}
		}

		public void EnqueueDestructionDeformationBlock(Vector3I position)
		{
			if (Sync.IsServer)
			{
				m_destructionDeformationQueue.Add(position);
				ScheduleSendDirtyBlocks();
			}
		}

		public void EnqueueDestroyedBlock(Vector3I position)
		{
			if (Sync.IsServer)
			{
				m_destroyBlockQueue.Add(position);
				ScheduleSendDirtyBlocks();
			}
		}

		public void EnqueueRemovedBlock(Vector3I position)
		{
			if (Sync.IsServer)
			{
				m_removeBlockQueue.Add(position);
				ScheduleSendDirtyBlocks();
			}
		}

		private void ScheduleSendDirtyBlocks()
		{
			if (m_destroyBlockQueue.Count + m_destructionDeformationQueue.Count + m_removeBlockQueue.Count == 1)
			{
				Schedule(UpdateQueue.OnceAfterSimulation, SendRemovedBlocks, 2);
			}
		}

		public void SendRemovedBlocks()
		{
			if (m_destroyBlockQueue.Count > 0 || m_destructionDeformationQueue.Count > 0 || m_removeBlockQueue.Count > 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RemovedBlocks, m_destroyBlockQueue, m_destructionDeformationQueue, m_removeBlockQueue);
				m_removeBlockQueue.Clear();
				m_destroyBlockQueue.Clear();
				m_destructionDeformationQueue.Clear();
			}
		}

<<<<<<< HEAD
		[Event(null, 3582)]
=======
		[Event(null, 3471)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void RemovedBlocks(List<Vector3I> destroyLocations, List<Vector3I> DestructionDeformationLocation, List<Vector3I> LocationsWithoutGenerator)
		{
			if (destroyLocations.Count > 0)
			{
				BlocksDestroyed(destroyLocations);
			}
			if (LocationsWithoutGenerator.Count > 0)
			{
				BlocksRemoved(LocationsWithoutGenerator);
			}
			if (DestructionDeformationLocation.Count > 0)
			{
				BlocksDeformed(DestructionDeformationLocation);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Server method, adds removed block with compound id into queue
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void EnqueueRemovedBlockWithId(Vector3I position, ushort? compoundId)
		{
			if (Sync.IsServer)
			{
				BlockPositionId blockPositionId = default(BlockPositionId);
				blockPositionId.Position = position;
				blockPositionId.CompoundId = (uint)(((int?)compoundId) ?? (-1));
				BlockPositionId item = blockPositionId;
				m_removeBlockWithIdQueue.Add(item);
				ScheduleSendDirtyBlocksWithIds();
			}
		}

		public void EnqueueDestroyedBlockWithId(Vector3I position, ushort? compoundId)
		{
			if (Sync.IsServer)
			{
				m_destroyBlockWithIdQueue.Add(new BlockPositionId
				{
					Position = position,
					CompoundId = (uint)(((int?)compoundId) ?? (-1))
				});
				ScheduleSendDirtyBlocksWithIds();
			}
		}

		private void ScheduleSendDirtyBlocksWithIds()
		{
			if (m_destroyBlockQueue.Count + m_destructionDeformationQueue.Count + m_removeBlockQueue.Count == 1)
			{
				Schedule(UpdateQueue.OnceAfterSimulation, SendRemovedBlocksWithIds, 2);
			}
		}

		public void SendRemovedBlocksWithIds()
		{
			if (m_removeBlockWithIdQueue.Count > 0 || m_destroyBlockWithIdQueue.Count > 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RemovedBlocksWithIds, m_destroyBlockWithIdQueue, m_removeBlockWithIdQueue);
				m_removeBlockWithIdQueue.Clear();
				m_destroyBlockWithIdQueue.Clear();
			}
		}

<<<<<<< HEAD
		[Event(null, 3642)]
=======
		[Event(null, 3531)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void RemovedBlocksWithIds(List<BlockPositionId> destroyBlockWithIdQueueWithoutGenerators, List<BlockPositionId> removeBlockWithIdQueueWithoutGenerators)
		{
			if (destroyBlockWithIdQueueWithoutGenerators.Count > 0)
			{
				BlocksWithIdRemoved(destroyBlockWithIdQueueWithoutGenerators);
			}
			if (removeBlockWithIdQueueWithoutGenerators.Count > 0)
			{
				BlocksWithIdRemoved(removeBlockWithIdQueueWithoutGenerators);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Remove all blocks from the grid built by specific player
		/// </summary>
		[Event(null, 3658)]
=======
		[Event(null, 3547)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		public void RemoveBlocksBuiltByID(long identityID)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = FindBlocksBuiltByID(identityID).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					RemoveBlock(current, updatePhysics: true);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Transfer all blocks built by a specific player to another player
		/// </summary>
		[Event(null, 3670)]
=======
		[Event(null, 3559)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		public void TransferBlocksBuiltByID(long oldAuthor, long newAuthor)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = FindBlocksBuiltByID(oldAuthor).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().TransferAuthorship(newAuthor);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void TransferBlocksBuiltByIDClient(long oldAuthor, long newAuthor)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = FindBlocksBuiltByID(oldAuthor).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().TransferAuthorshipClient(newAuthor);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void TransferBlockLimitsBuiltByID(long author, MyBlockLimits oldLimits, MyBlockLimits newLimits)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = FindBlocksBuiltByID(author).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().TransferLimits(oldLimits, newLimits);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		/// <summary>
		/// Find all blocks built by a specific player..
		/// </summary>
		public HashSet<MySlimBlock> FindBlocksBuiltByID(long identityID)
		{
			return FindBlocksBuiltByID(identityID, new HashSet<MySlimBlock>());
		}

		public HashSet<MySlimBlock> FindBlocksBuiltByID(long identityID, HashSet<MySlimBlock> builtBlocks)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = m_cubeBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					if (current.BuiltBy == identityID)
					{
						builtBlocks.Add(current);
					}
				}
				return builtBlocks;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public MySlimBlock BuildGeneratedBlock(MyBlockLocation location, Vector3 colorMaskHsv, MyStringHash skinId)
		{
			MyDefinitionId id = location.BlockDefinition;
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(id);
			location.Orientation.GetQuaternion(out var result);
			return BuildBlock(cubeBlockDefinition, colorMaskHsv, skinId, location.Min, result, location.Owner, location.EntityId, null);
		}

<<<<<<< HEAD
		[Event(null, 3724)]
=======
		[Event(null, 3613)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public void BuildBlockRequest(uint colorMaskHsv, MyBlockLocation location, [DynamicObjectBuilder(false)] MyObjectBuilder_CubeBlock blockObjectBuilder, long builderEntityId, bool instantBuild, long ownerId)
		{
			BuildBlockRequestInternal(new MyBlockVisuals(colorMaskHsv, MyStringHash.NullOrEmpty, applyColor: true, applySkin: false), location, blockObjectBuilder, builderEntityId, instantBuild, ownerId, MyEventContext.Current.Sender.Value);
		}

<<<<<<< HEAD
		[Event(null, 3731)]
=======
		[Event(null, 3620)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public void BuildBlockRequest(MyBlockVisuals visuals, MyBlockLocation location, [DynamicObjectBuilder(false)] MyObjectBuilder_CubeBlock blockObjectBuilder, long builderEntityId, bool instantBuild, long ownerId)
		{
			BuildBlockRequestInternal(visuals, location, blockObjectBuilder, builderEntityId, instantBuild, ownerId, MyEventContext.Current.Sender.Value);
		}

		public void BuildBlockRequestInternal(MyBlockVisuals visuals, MyBlockLocation location, MyObjectBuilder_CubeBlock blockObjectBuilder, long builderEntityId, bool instantBuild, long ownerId, ulong sender, bool isProjection = false)
		{
			MyEntity entity = null;
			MyEntities.TryGetEntityById(builderEntityId, out entity);
			bool flag = sender == Sync.MyId || MySession.Static.HasPlayerCreativeRights(sender);
			if ((entity == null && !flag && !MySession.Static.CreativeMode) || !MySessionComponentSafeZones.IsActionAllowed(this, isProjection ? MySafeZoneAction.BuildingProjections : MySafeZoneAction.Building, builderEntityId, 0uL))
			{
				return;
			}
			if (!MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(location.BlockDefinition, sender) || (MySession.Static.ResearchEnabled && !flag && !MySessionComponentResearch.Static.CanUse(ownerId, location.BlockDefinition)))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(sender);
				return;
			}
			MyBlockLocation? resultBlock = null;
			MyDefinitionManager.Static.TryGetCubeBlockDefinition(location.BlockDefinition, out var blockDefinition);
			MyBlockOrientation orientation = location.Orientation;
			location.Orientation.GetQuaternion(out var result);
			MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = blockDefinition.GetBuildProgressModelMountPoints(MyComponentStack.NewBlockIntegrity);
			int? ignoreMultiblockId = ((blockObjectBuilder != null && blockObjectBuilder.MultiBlockId != 0) ? new int?(blockObjectBuilder.MultiBlockId) : null);
			Vector3I position = location.CenterPos;
			visuals.SkinId = MySession.Static.GetComponent<MySessionComponentGameInventory>()?.ValidateArmor(visuals.SkinId, sender) ?? MyStringHash.NullOrEmpty;
			if (!CanPlaceBlock(location.Min, location.Max, orientation, blockDefinition, 0uL, ignoreMultiblockId, ignoreFracturedPieces: false, isProjection) || !CheckConnectivity(this, blockDefinition, buildProgressModelMountPoints, ref result, ref position))
			{
				return;
			}
			MySlimBlock mySlimBlock = BuildBlockSuccess(ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV), visuals.SkinId, location, blockObjectBuilder, ref resultBlock, entity, flag && instantBuild, ownerId);
			if (mySlimBlock != null && resultBlock.HasValue)
			{
				MyMultiplayer.RaiseEvent(mySlimBlock.CubeGrid, (MyCubeGrid x) => x.BuildBlockSucess, visuals, location, blockObjectBuilder, builderEntityId, flag && instantBuild, ownerId);
				AfterBuildBlockSuccess(resultBlock.Value, instantBuild);
			}
		}

<<<<<<< HEAD
		[Event(null, 3787)]
=======
		[Event(null, 3676)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		public void BuildBlockSucess(MyBlockVisuals visuals, MyBlockLocation location, [DynamicObjectBuilder(false)] MyObjectBuilder_CubeBlock blockObjectBuilder, long builderEntityId, bool instantBuild, long ownerId)
		{
			MyEntity entity = null;
			MyEntities.TryGetEntityById(builderEntityId, out entity);
			MyBlockLocation? resultBlock = null;
			BuildBlockSuccess(ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV), visuals.SkinId, location, blockObjectBuilder, ref resultBlock, entity, instantBuild, ownerId);
			if (resultBlock.HasValue)
			{
				AfterBuildBlockSuccess(resultBlock.Value, instantBuild);
			}
		}

		/// <summary>
		/// Network friendly alternative for building block
		/// </summary>
		public void BuildBlocks(ref MyBlockBuildArea area, long builderEntityId, long ownerId)
		{
			int num = area.BuildAreaSize.X * area.BuildAreaSize.Y * area.BuildAreaSize.Z;
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(area.DefinitionId);
			if (!MySession.Static.CheckLimitsAndNotify(ownerId, cubeBlockDefinition.BlockPairName, num * cubeBlockDefinition.PCU, num, BlocksCount))
			{
				return;
			}
			ulong steamId = MySession.Static.Players.TryGetSteamId(ownerId);
			if (MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(cubeBlockDefinition, steamId))
			{
				bool arg = MySession.Static.CreativeToolsEnabled(Sync.MyId);
<<<<<<< HEAD
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BuildBlocksAreaRequest, area, builderEntityId, arg, ownerId, Sync.MyId, cubeBlockDefinition?.DisplayNameText ?? "");
=======
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BuildBlocksAreaRequest, area, builderEntityId, arg, ownerId, Sync.MyId);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		/// <summary>
		/// Builds many same blocks, used when building lines or planes.
		/// </summary>
		public void BuildBlocks(Vector3 colorMaskHsv, MyStringHash skinId, HashSet<MyBlockLocation> locations, long builderEntityId, long ownerId)
		{
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(Enumerable.First<MyBlockLocation>((IEnumerable<MyBlockLocation>)locations).BlockDefinition);
			string blockPairName = cubeBlockDefinition.BlockPairName;
			bool flag = MySession.Static.CreativeToolsEnabled(Sync.MyId);
			bool flag2 = flag || MySession.Static.CreativeMode;
<<<<<<< HEAD
			if (!MySession.Static.CheckLimitsAndNotify(ownerId, blockPairName, flag2 ? (locations.Count * cubeBlockDefinition.PCU) : locations.Count, locations.Count, BlocksCount))
=======
			if (!MySession.Static.CheckLimitsAndNotify(ownerId, blockPairName, flag2 ? (locations.get_Count() * cubeBlockDefinition.PCU) : locations.get_Count(), locations.get_Count(), BlocksCount))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			ulong steamId = MySession.Static.Players.TryGetSteamId(ownerId);
			if (MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(cubeBlockDefinition, steamId))
			{
<<<<<<< HEAD
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BuildBlocksRequest, new MyBlockVisuals(colorMaskHsv.PackHSVToUint(), skinId), locations, builderEntityId, flag, ownerId);
			}
		}

		[Event(null, 3839)]
=======
				MyMultiplayer.RaiseEvent<MyCubeGrid, MyBlockVisuals, HashSet<MyBlockLocation>, long, bool, long>(this, (MyCubeGrid x) => x.BuildBlocksRequest, new MyBlockVisuals(colorMaskHsv.PackHSVToUint(), skinId), locations, builderEntityId, flag, ownerId);
			}
		}

		[Event(null, 3728)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private void BuildBlocksRequest(MyBlockVisuals visuals, HashSet<MyBlockLocation> locations, long builderEntityId, bool instantBuild, long ownerId)
		{
			if (!MySession.Static.CreativeMode && !MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				instantBuild = false;
			}
			m_tmpBuildList.Clear();
			MyEntity entity = null;
			MyEntities.TryGetEntityById(builderEntityId, out entity);
			MyCubeBuilder.BuildComponent.GetBlocksPlacementMaterials(locations, this);
			bool flag = MySession.Static.CreativeToolsEnabled(MyEventContext.Current.Sender.Value) || MySession.Static.CreativeMode;
			bool flag2 = MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value);
			if ((entity == null && !flag2 && !MySession.Static.CreativeMode) || !MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.Building, builderEntityId, MyEventContext.Current.Sender.Value) || (!MyCubeBuilder.BuildComponent.HasBuildingMaterials(entity) && !flag2))
			{
				return;
			}
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(Enumerable.First<MyBlockLocation>((IEnumerable<MyBlockLocation>)locations).BlockDefinition);
			string blockPairName = cubeBlockDefinition.BlockPairName;
			if (!IsWithinWorldLimits(ownerId, locations.get_Count(), flag ? (locations.get_Count() * cubeBlockDefinition.PCU) : locations.get_Count(), blockPairName))
			{
				return;
			}
			Vector3 colorMaskHsv = ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV);
			ulong value = MyEventContext.Current.Sender.Value;
			visuals.SkinId = MySession.Static.GetComponent<MySessionComponentGameInventory>()?.ValidateArmor(visuals.SkinId, value) ?? MyStringHash.NullOrEmpty;
			BuildBlocksSuccess(colorMaskHsv, visuals.SkinId, locations, m_tmpBuildList, entity, flag2 && instantBuild, ownerId, MyEventContext.Current.Sender.Value);
			if (m_tmpBuildList.get_Count() > 0)
			{
				MySession.Static.TotalBlocksCreated += (uint)m_tmpBuildList.get_Count();
				if (MySession.Static.ControlledEntity is MyCockpit)
				{
					MySession.Static.TotalBlocksCreatedFromShips += (uint)m_tmpBuildList.get_Count();
				}
				MyMultiplayer.RaiseEvent<MyCubeGrid, MyBlockVisuals, HashSet<MyBlockLocation>, long, bool, long>(this, (MyCubeGrid x) => x.BuildBlocksClient, visuals, m_tmpBuildList, builderEntityId, flag2 && instantBuild, ownerId);
				if (Sync.IsServer && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId == ownerId)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
				}
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BuildBlocksFailedNotify, new EndpointId(MyEventContext.Current.Sender.Value));
			}
			AfterBuildBlocksSuccess(m_tmpBuildList, instantBuild);
		}

<<<<<<< HEAD
		[Event(null, 3902)]
=======
		[Event(null, 3792)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		public void BuildBlocksFailedNotify()
		{
			if (MyCubeBuilder.Static != null)
			{
				MyCubeBuilder.Static.NotifyPlacementUnable();
			}
		}

<<<<<<< HEAD
		[Event(null, 3909)]
=======
		[Event(null, 3799)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		public void BuildBlocksClient(MyBlockVisuals visuals, HashSet<MyBlockLocation> locations, long builderEntityId, bool instantBuild, long ownerId)
		{
			m_tmpBuildList.Clear();
			MyEntity entity = null;
			MyEntities.TryGetEntityById(builderEntityId, out entity);
			BuildBlocksSuccess(ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV), visuals.SkinId, locations, m_tmpBuildList, entity, instantBuild, ownerId, 0uL);
			if (ownerId == MySession.Static.LocalPlayerId)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
			}
			AfterBuildBlocksSuccess(m_tmpBuildList, instantBuild);
		}

<<<<<<< HEAD
		[Event(null, 3923)]
=======
		[Event(null, 3813)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private void BuildBlocksAreaRequest(MyBlockBuildArea area, long builderEntityId, bool instantBuild, long ownerId, ulong placingPlayer, string localizedDisplayNameBase = "")
		{
			if (!MySession.Static.CreativeMode && !MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				instantBuild = false;
			}
			try
			{
				bool flag = MySession.Static.CreativeToolsEnabled(MyEventContext.Current.Sender.Value) || MySession.Static.CreativeMode;
				bool flag2 = MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value);
				if ((ownerId == 0L && !flag2 && !MySession.Static.CreativeMode) || !MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.Building, builderEntityId, placingPlayer))
				{
					return;
				}
				MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(area.DefinitionId);
				int num = area.BuildAreaSize.X * area.BuildAreaSize.Y * area.BuildAreaSize.Z;
				if (!IsWithinWorldLimits(ownerId, num, flag ? (num * cubeBlockDefinition.PCU) : num, cubeBlockDefinition.BlockPairName))
				{
					return;
				}
				int amount = area.BuildAreaSize.X * area.BuildAreaSize.Y * area.BuildAreaSize.Z;
				MyCubeBuilder.BuildComponent.GetBlockAmountPlacementMaterials(cubeBlockDefinition, amount);
				MyEntity entity = null;
				MyEntities.TryGetEntityById(builderEntityId, out entity);
				if (MyCubeBuilder.BuildComponent.HasBuildingMaterials(entity, testTotal: true) || flag2)
				{
					GetValidBuildOffsets(ref area, m_tmpBuildOffsets, m_tmpBuildFailList, placingPlayer);
					CheckAreaConnectivity(this, ref area, m_tmpBuildOffsets, m_tmpBuildFailList);
					int num2 = MyRandom.Instance.CreateRandomSeed();
					area.SkinId = MySession.Static.GetComponent<MySessionComponentGameInventory>()?.ValidateArmor(area.SkinId, MyEventContext.Current.Sender.Value) ?? MyStringHash.NullOrEmpty;
<<<<<<< HEAD
					MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BuildBlocksAreaClient, area, num2, m_tmpBuildFailList, builderEntityId, flag2, ownerId);
					BuildBlocksArea(ref area, m_tmpBuildOffsets, builderEntityId, flag2, ownerId, num2, localizedDisplayNameBase);
=======
					MyMultiplayer.RaiseEvent<MyCubeGrid, MyBlockBuildArea, int, HashSet<Vector3UByte>, long, bool, long>(this, (MyCubeGrid x) => x.BuildBlocksAreaClient, area, num2, m_tmpBuildFailList, builderEntityId, flag2, ownerId);
					BuildBlocksArea(ref area, m_tmpBuildOffsets, builderEntityId, flag2, ownerId, num2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				m_tmpBuildOffsets.Clear();
				m_tmpBuildFailList.Clear();
			}
		}

<<<<<<< HEAD
		[Event(null, 3979)]
=======
		[Event(null, 3869)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void BuildBlocksAreaClient(MyBlockBuildArea area, int entityIdSeed, HashSet<Vector3UByte> failList, long builderEntityId, bool isAdmin, long ownerId)
		{
			try
			{
				GetAllBuildOffsetsExcept(ref area, failList, m_tmpBuildOffsets);
				BuildBlocksArea(ref area, m_tmpBuildOffsets, builderEntityId, isAdmin, ownerId, entityIdSeed);
			}
			finally
			{
				m_tmpBuildOffsets.Clear();
			}
		}

		private void BuildBlocksArea(ref MyBlockBuildArea area, List<Vector3UByte> validOffsets, long builderEntityId, bool isAdmin, long ownerId, int entityIdSeed, string localizedDisplayNameBase = "")
		{
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(area.DefinitionId);
			if (cubeBlockDefinition == null)
			{
				return;
			}
			Quaternion orientation = Base6Directions.GetOrientation(area.OrientationForward, area.OrientationUp);
			Vector3I vector3I = area.StepDelta;
			MyEntity entity = null;
			MyEntities.TryGetEntityById(builderEntityId, out entity);
			try
			{
				bool flag = false;
				validOffsets.Sort(Vector3UByte.Comparer);
				using (MyRandom.Instance.PushSeed(entityIdSeed))
				{
					foreach (Vector3UByte validOffset in validOffsets)
					{
						Vector3I vector3I2 = area.PosInGrid + validOffset * vector3I;
<<<<<<< HEAD
						MySlimBlock mySlimBlock = BuildBlock(cubeBlockDefinition, ColorExtensions.UnpackHSVFromUint(area.ColorMaskHSV), area.SkinId, vector3I2 + area.BlockMin, orientation, ownerId, MyEntityIdentifier.AllocateId(), entity, null, updateVolume: false, testMerge: false, isAdmin, localizedDisplayNameBase);
=======
						MySlimBlock mySlimBlock = BuildBlock(cubeBlockDefinition, ColorExtensions.UnpackHSVFromUint(area.ColorMaskHSV), area.SkinId, vector3I2 + area.BlockMin, orientation, ownerId, MyEntityIdentifier.AllocateId(), entity, null, updateVolume: false, testMerge: false, isAdmin);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (mySlimBlock == null)
						{
							continue;
						}
						ChangeBlockOwner(mySlimBlock, ownerId);
						flag = true;
						m_tmpBuildSuccessBlocks.Add(mySlimBlock);
						if (ownerId == MySession.Static.LocalPlayerId)
						{
							MySession.Static.TotalBlocksCreated++;
							if (MySession.Static.ControlledEntity is MyCockpit)
							{
								MySession.Static.TotalBlocksCreatedFromShips++;
							}
						}
					}
				}
				BoundingBoxD boundingBox = BoundingBoxD.CreateInvalid();
				foreach (MySlimBlock tmpBuildSuccessBlock in m_tmpBuildSuccessBlocks)
				{
					tmpBuildSuccessBlock.GetWorldBoundingBox(out var aabb);
					boundingBox.Include(aabb);
					if (tmpBuildSuccessBlock.FatBlock != null)
					{
						tmpBuildSuccessBlock.FatBlock.OnBuildSuccess(ownerId, isAdmin);
					}
				}
				if (m_tmpBuildSuccessBlocks.Count > 0)
				{
					if (IsStatic && Sync.IsServer)
					{
						List<MyEntity> entitiesInAABB = MyEntities.GetEntitiesInAABB(ref boundingBox);
						foreach (MySlimBlock tmpBuildSuccessBlock2 in m_tmpBuildSuccessBlocks)
						{
							DetectMerge(tmpBuildSuccessBlock2, null, entitiesInAABB);
						}
						entitiesInAABB.Clear();
					}
					m_tmpBuildSuccessBlocks[0].PlayConstructionSound(MyIntegrityChangeEnum.ConstructionBegin);
					UpdateGridAABB();
				}
				if (MySession.Static.LocalPlayerId == ownerId)
				{
					if (flag)
					{
						MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
					}
					else
					{
						MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
					}
				}
			}
			finally
			{
				m_tmpBuildSuccessBlocks.Clear();
			}
		}

		private void GetAllBuildOffsetsExcept(ref MyBlockBuildArea area, HashSet<Vector3UByte> exceptList, List<Vector3UByte> resultOffsets)
		{
			Vector3UByte vector3UByte = default(Vector3UByte);
			vector3UByte.X = 0;
			while (vector3UByte.X < area.BuildAreaSize.X)
			{
				vector3UByte.Y = 0;
				while (vector3UByte.Y < area.BuildAreaSize.Y)
				{
					vector3UByte.Z = 0;
					while (vector3UByte.Z < area.BuildAreaSize.Z)
					{
						if (!exceptList.Contains(vector3UByte))
						{
							resultOffsets.Add(vector3UByte);
						}
						vector3UByte.Z++;
					}
					vector3UByte.Y++;
				}
				vector3UByte.X++;
			}
		}

		private void GetValidBuildOffsets(ref MyBlockBuildArea area, List<Vector3UByte> resultOffsets, HashSet<Vector3UByte> resultFailList, ulong placingPlayer = 0uL)
		{
			Vector3I vector3I = area.StepDelta;
			MyBlockOrientation orientation = new MyBlockOrientation(area.OrientationForward, area.OrientationUp);
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(area.DefinitionId);
			Vector3UByte vector3UByte = default(Vector3UByte);
			vector3UByte.X = 0;
			while (vector3UByte.X < area.BuildAreaSize.X)
			{
				vector3UByte.Y = 0;
				while (vector3UByte.Y < area.BuildAreaSize.Y)
				{
					vector3UByte.Z = 0;
					while (vector3UByte.Z < area.BuildAreaSize.Z)
					{
						Vector3I vector3I2 = area.PosInGrid + vector3UByte * vector3I;
						if (CanPlaceBlock(vector3I2 + area.BlockMin, vector3I2 + area.BlockMax, orientation, cubeBlockDefinition, placingPlayer))
						{
							resultOffsets.Add(vector3UByte);
						}
						else
						{
							resultFailList.Add(vector3UByte);
						}
						vector3UByte.Z++;
					}
					vector3UByte.Y++;
				}
				vector3UByte.X++;
			}
		}

		private void BuildBlocksSuccess(Vector3 colorMaskHsv, MyStringHash skinId, HashSet<MyBlockLocation> locations, HashSet<MyBlockLocation> resultBlocks, MyEntity builder, bool instantBuilt, long ownerId, ulong placingPlayer = 0uL)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			bool flag = true;
			while (locations.get_Count() > 0 && flag)
			{
				flag = false;
				Enumerator<MyBlockLocation> enumerator = locations.GetEnumerator();
				try
				{
<<<<<<< HEAD
					MyBlockOrientation orientation = location.Orientation;
					orientation.GetQuaternion(out var result);
					Vector3I center = location.CenterPos;
					MyDefinitionManager.Static.TryGetCubeBlockDefinition(location.BlockDefinition, out var blockDefinition);
					if (blockDefinition == null)
					{
						return;
					}
					MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = blockDefinition.GetBuildProgressModelMountPoints(MyComponentStack.NewBlockIntegrity);
					if (!Sync.IsServer || CanPlaceWithConnectivity(location, ref result, ref center, blockDefinition, buildProgressModelMountPoints, placingPlayer))
=======
					while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MyBlockLocation current = enumerator.get_Current();
						MyBlockOrientation orientation = current.Orientation;
						orientation.GetQuaternion(out var result);
						Vector3I center = current.CenterPos;
						MyDefinitionManager.Static.TryGetCubeBlockDefinition(current.BlockDefinition, out var blockDefinition);
						if (blockDefinition == null)
						{
							return;
						}
						MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = blockDefinition.GetBuildProgressModelMountPoints(MyComponentStack.NewBlockIntegrity);
						if (!Sync.IsServer || CanPlaceWithConnectivity(current, ref result, ref center, blockDefinition, buildProgressModelMountPoints, placingPlayer))
						{
							MySlimBlock mySlimBlock = BuildBlock(blockDefinition, colorMaskHsv, skinId, current.Min, result, current.Owner, current.EntityId, builder, null, updateVolume: true, testMerge: false, instantBuilt);
							if (mySlimBlock != null)
							{
								ChangeBlockOwner(mySlimBlock, ownerId);
								MyBlockLocation myBlockLocation = current;
								resultBlocks.Add(myBlockLocation);
							}
							flag = true;
							locations.Remove(current);
							break;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
		}

		private bool CanPlaceWithConnectivity(MyBlockLocation location, ref Quaternion orientation, ref Vector3I center, MyCubeBlockDefinition blockDefinition, MyCubeBlockDefinition.MountPoint[] mountPoints, ulong placingPlayer = 0uL)
		{
			if (CanPlaceBlock(location.Min, location.Max, location.Orientation, blockDefinition, placingPlayer))
			{
				return CheckConnectivity(this, blockDefinition, mountPoints, ref orientation, ref center);
			}
			return false;
		}

		private MySlimBlock BuildBlockSuccess(Vector3 colorMaskHsv, MyStringHash skinId, MyBlockLocation location, MyObjectBuilder_CubeBlock objectBuilder, ref MyBlockLocation? resultBlock, MyEntity builder, bool instantBuilt, long ownerId)
		{
			location.Orientation.GetQuaternion(out var result);
			MyDefinitionManager.Static.TryGetCubeBlockDefinition(location.BlockDefinition, out var blockDefinition);
			if (blockDefinition == null)
			{
				return null;
			}
			MySlimBlock mySlimBlock = BuildBlock(blockDefinition, colorMaskHsv, skinId, location.Min, result, location.Owner, location.EntityId, instantBuilt ? null : builder, objectBuilder);
			if (mySlimBlock != null)
			{
				ChangeBlockOwner(mySlimBlock, ownerId);
				resultBlock = location;
				mySlimBlock.PlayConstructionSound(MyIntegrityChangeEnum.ConstructionBegin);
			}
			else
			{
				resultBlock = null;
			}
			return mySlimBlock;
		}

		private static void ChangeBlockOwner(MySlimBlock block, long ownerId)
		{
			if (block.FatBlock != null)
			{
				block.FatBlock.ChangeOwner(ownerId, MyOwnershipShareModeEnum.Faction);
			}
		}

		private void AfterBuildBlocksSuccess(HashSet<MyBlockLocation> builtBlocks, bool instantBuild)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyBlockLocation> enumerator = builtBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyBlockLocation current = enumerator.get_Current();
					AfterBuildBlockSuccess(current, instantBuild);
					MySlimBlock cubeBlock = GetCubeBlock(current.CenterPos);
					DetectMerge(cubeBlock);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private void AfterBuildBlockSuccess(MyBlockLocation builtBlock, bool instantBuild)
		{
			MySlimBlock cubeBlock = GetCubeBlock(builtBlock.CenterPos);
			if (cubeBlock != null && cubeBlock.FatBlock != null)
			{
				cubeBlock.FatBlock.OnBuildSuccess(builtBlock.Owner, instantBuild);
			}
		}

		public void RazeBlocksDelayed(ref Vector3I pos, ref Vector3UByte size, long builderEntityId)
		{
			bool flag = false;
			Vector3UByte vector3UByte = default(Vector3UByte);
			vector3UByte.X = 0;
			while (vector3UByte.X <= size.X)
			{
				vector3UByte.Y = 0;
				while (vector3UByte.Y <= size.Y)
				{
					vector3UByte.Z = 0;
					while (vector3UByte.Z <= size.Z)
					{
						Vector3I pos2 = pos + vector3UByte;
						MySlimBlock cubeBlock = GetCubeBlock(pos2);
						if (cubeBlock != null && cubeBlock.FatBlock != null && !cubeBlock.FatBlock.IsSubBlock)
						{
							MyCockpit myCockpit = cubeBlock.FatBlock as MyCockpit;
							if (myCockpit != null && myCockpit.Pilot != null)
							{
								if (!flag)
								{
									flag = true;
									m_isRazeBatchDelayed = true;
									m_delayedRazeBatch = new MyDelayedRazeBatch(pos, size);
									m_delayedRazeBatch.Occupied = new HashSet<MyCockpit>();
								}
								m_delayedRazeBatch.Occupied.Add(myCockpit);
							}
						}
						vector3UByte.Z++;
					}
					vector3UByte.Y++;
				}
				vector3UByte.X++;
			}
			if (!flag)
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RazeBlocksAreaRequest, pos, size, builderEntityId, Sync.MyId);
			}
			else if (!MySession.Static.CreativeMode && MyMultiplayer.Static != null && MySession.Static.IsUserAdmin(Sync.MyId))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: OnClosedMessageBox, messageText: MyTexts.Get(MyCommonTexts.RemovePilotToo)));
			}
			else
			{
				OnClosedMessageBox(MyGuiScreenMessageBox.ResultEnum.NO);
			}
		}

		public void OnClosedMessageBox(MyGuiScreenMessageBox.ResultEnum result)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			if (!m_isRazeBatchDelayed)
			{
				return;
			}
			if (base.Closed)
			{
				m_delayedRazeBatch.Occupied.Clear();
				m_delayedRazeBatch = null;
				m_isRazeBatchDelayed = false;
				return;
			}
			if (result == MyGuiScreenMessageBox.ResultEnum.NO)
			{
				Enumerator<MyCockpit> enumerator = m_delayedRazeBatch.Occupied.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyCockpit current = enumerator.get_Current();
						if (current.Pilot != null && !current.MarkedForClose)
						{
							current.RequestRemovePilot();
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RazeBlocksAreaRequest, m_delayedRazeBatch.Pos, m_delayedRazeBatch.Size, MySession.Static.LocalCharacterEntityId, Sync.MyId);
			m_delayedRazeBatch.Occupied.Clear();
			m_delayedRazeBatch = null;
			m_isRazeBatchDelayed = false;
		}

		public void RazeBlocks(ref Vector3I pos, ref Vector3UByte size, long builderEntityId = 0L)
		{
			ulong arg = 0uL;
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RazeBlocksAreaRequest, pos, size, builderEntityId, arg);
		}

<<<<<<< HEAD
		[Event(null, 4316)]
=======
		[Event(null, 4205)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private void RazeBlocksAreaRequest(Vector3I pos, Vector3UByte size, long builderEntityId, ulong placingPlayer)
		{
			if (!MySession.Static.CreativeMode && !MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			try
			{
				Vector3UByte vector3UByte = default(Vector3UByte);
				vector3UByte.X = 0;
				while (vector3UByte.X <= size.X)
				{
					vector3UByte.Y = 0;
					while (vector3UByte.Y <= size.Y)
					{
						vector3UByte.Z = 0;
						while (vector3UByte.Z <= size.Z)
						{
							Vector3I pos2 = pos + vector3UByte;
							MySlimBlock cubeBlock = GetCubeBlock(pos2);
							if (cubeBlock == null || (cubeBlock.FatBlock != null && cubeBlock.FatBlock.IsSubBlock))
							{
								m_tmpBuildFailList.Add(vector3UByte);
							}
							vector3UByte.Z++;
						}
						vector3UByte.Y++;
					}
					vector3UByte.X++;
				}
				if (MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.Building, builderEntityId, placingPlayer))
				{
<<<<<<< HEAD
					MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RazeBlocksAreaSuccess, pos, size, m_tmpBuildFailList);
=======
					MyMultiplayer.RaiseEvent<MyCubeGrid, Vector3I, Vector3UByte, HashSet<Vector3UByte>>(this, (MyCubeGrid x) => x.RazeBlocksAreaSuccess, pos, size, m_tmpBuildFailList);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					RazeBlocksAreaSuccess(pos, size, m_tmpBuildFailList);
				}
			}
			finally
			{
				m_tmpBuildFailList.Clear();
			}
		}

<<<<<<< HEAD
		[Event(null, 4349)]
=======
		[Event(null, 4238)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void RazeBlocksAreaSuccess(Vector3I pos, Vector3UByte size, HashSet<Vector3UByte> resultFailList)
		{
			Vector3I min = Vector3I.MaxValue;
			Vector3I max = Vector3I.MinValue;
			Vector3UByte vector3UByte = default(Vector3UByte);
			if (MyFakes.ENABLE_MULTIBLOCKS)
			{
				vector3UByte.X = 0;
				while (vector3UByte.X <= size.X)
				{
					vector3UByte.Y = 0;
					while (vector3UByte.Y <= size.Y)
					{
						vector3UByte.Z = 0;
						while (vector3UByte.Z <= size.Z)
						{
							if (!resultFailList.Contains(vector3UByte))
							{
								Vector3I pos2 = pos + vector3UByte;
								MySlimBlock cubeBlock = GetCubeBlock(pos2);
								if (cubeBlock != null)
								{
									MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
									if (myCompoundCubeBlock != null)
									{
										m_tmpSlimBlocks.Clear();
										m_tmpSlimBlocks.AddRange(myCompoundCubeBlock.GetBlocks());
										foreach (MySlimBlock tmpSlimBlock in m_tmpSlimBlocks)
										{
											if (tmpSlimBlock.IsMultiBlockPart)
											{
												m_tmpBlocksInMultiBlock.Clear();
												GetBlocksInMultiBlock(tmpSlimBlock.MultiBlockId, m_tmpBlocksInMultiBlock);
												RemoveMultiBlocks(ref min, ref max, m_tmpBlocksInMultiBlock);
												m_tmpBlocksInMultiBlock.Clear();
											}
											else
											{
												ushort? blockId = myCompoundCubeBlock.GetBlockId(tmpSlimBlock);
												if (blockId.HasValue)
												{
													RemoveBlockInCompound(tmpSlimBlock.Position, blockId.Value, ref min, ref max);
												}
											}
										}
										m_tmpSlimBlocks.Clear();
									}
									else if (cubeBlock.IsMultiBlockPart)
									{
										m_tmpBlocksInMultiBlock.Clear();
										GetBlocksInMultiBlock(cubeBlock.MultiBlockId, m_tmpBlocksInMultiBlock);
										RemoveMultiBlocks(ref min, ref max, m_tmpBlocksInMultiBlock);
										m_tmpBlocksInMultiBlock.Clear();
									}
									else
									{
										MyFracturedBlock myFracturedBlock = cubeBlock.FatBlock as MyFracturedBlock;
										if (myFracturedBlock != null && myFracturedBlock.MultiBlocks != null && myFracturedBlock.MultiBlocks.Count > 0)
										{
											foreach (MyFracturedBlock.MultiBlockPartInfo multiBlock in myFracturedBlock.MultiBlocks)
											{
												if (multiBlock != null)
												{
													m_tmpBlocksInMultiBlock.Clear();
													if (MyDefinitionManager.Static.TryGetMultiBlockDefinition(multiBlock.MultiBlockDefinition) != null)
													{
														GetBlocksInMultiBlock(multiBlock.MultiBlockId, m_tmpBlocksInMultiBlock);
														RemoveMultiBlocks(ref min, ref max, m_tmpBlocksInMultiBlock);
													}
													m_tmpBlocksInMultiBlock.Clear();
												}
											}
										}
										else
										{
											min = Vector3I.Min(min, cubeBlock.Min);
											max = Vector3I.Max(max, cubeBlock.Max);
											RemoveBlockByCubeBuilder(cubeBlock);
										}
									}
								}
							}
							vector3UByte.Z++;
						}
						vector3UByte.Y++;
					}
					vector3UByte.X++;
				}
			}
			else
			{
				vector3UByte.X = 0;
				while (vector3UByte.X <= size.X)
				{
					vector3UByte.Y = 0;
					while (vector3UByte.Y <= size.Y)
					{
						vector3UByte.Z = 0;
						while (vector3UByte.Z <= size.Z)
						{
							if (!resultFailList.Contains(vector3UByte))
							{
								Vector3I pos3 = pos + vector3UByte;
								MySlimBlock cubeBlock2 = GetCubeBlock(pos3);
								if (cubeBlock2 != null)
								{
									min = Vector3I.Min(min, cubeBlock2.Min);
									max = Vector3I.Max(max, cubeBlock2.Max);
									RemoveBlockByCubeBuilder(cubeBlock2);
								}
							}
							vector3UByte.Z++;
						}
						vector3UByte.Y++;
					}
					vector3UByte.X++;
				}
			}
			if (Physics != null)
			{
				Physics.AddDirtyArea(min, max);
			}
		}

		private void RemoveMultiBlocks(ref Vector3I min, ref Vector3I max, HashSet<Tuple<MySlimBlock, ushort?>> tmpBlocksInMultiBlock)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<Tuple<MySlimBlock, ushort?>> enumerator = tmpBlocksInMultiBlock.GetEnumerator();
			try
			{
<<<<<<< HEAD
				if (item.Item2.HasValue)
				{
					RemoveBlockInCompound(item.Item1.Position, item.Item2.Value, ref min, ref max);
					continue;
				}
				min = Vector3I.Min(min, item.Item1.Min);
				max = Vector3I.Max(max, item.Item1.Max);
				RemoveBlockByCubeBuilder(item.Item1);
=======
				while (enumerator.MoveNext())
				{
					Tuple<MySlimBlock, ushort?> current = enumerator.get_Current();
					if (current.Item2.HasValue)
					{
						RemoveBlockInCompound(current.Item1.Position, current.Item2.Value, ref min, ref max);
						continue;
					}
					min = Vector3I.Min(min, current.Item1.Min);
					max = Vector3I.Max(max, current.Item1.Max);
					RemoveBlockByCubeBuilder(current.Item1);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		/// <summary>
		/// user is used only if called on server, when called from client, sender steam id will be used
		/// </summary>
		/// <param name="position"></param>
		/// <param name="user"></param>
		public void RazeBlock(Vector3I position, ulong user = 0uL)
		{
			m_tmpPositionListSend.Clear();
			m_tmpPositionListSend.Add(position);
			RazeBlocks(m_tmpPositionListSend, 0L, user);
		}

		/// <summary>
		/// Razes blocks (unbuild)
		/// user is used only if locally invoked, if invoked from client, sender steam id is used
		/// </summary>
		public void RazeBlocks(List<Vector3I> locations, long builderEntityId = 0L, ulong user = 0uL)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RazeBlocksRequest, locations, builderEntityId, user);
		}

<<<<<<< HEAD
		/// <summary>
		/// user is used only if locally invoked, if invoked from client, sender steam id is used
		/// </summary>
		/// <param name="locations"></param>
		/// <param name="builderEntityId"></param>
		/// <param name="user"></param>
		[Event(null, 4517)]
=======
		[Event(null, 4406)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public void RazeBlocksRequest(List<Vector3I> locations, long builderEntityId = 0L, ulong user = 0uL)
		{
			m_tmpPositionListReceive.Clear();
			if (MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.Grinding, builderEntityId, MyEventContext.Current.IsLocallyInvoked ? user : MyEventContext.Current.Sender.Value))
			{
				RazeBlocksSuccess(locations, m_tmpPositionListReceive);
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RazeBlocksClient, m_tmpPositionListReceive);
			}
		}

<<<<<<< HEAD
		[Event(null, 4530)]
=======
		[Event(null, 4419)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		public void RazeBlocksClient(List<Vector3I> locations)
		{
			m_tmpPositionListReceive.Clear();
			RazeBlocksSuccess(locations, m_tmpPositionListReceive);
		}

		private void RazeBlocksSuccess(List<Vector3I> locations, List<Vector3I> removedBlocks)
		{
			Vector3I vector3I = Vector3I.MaxValue;
			Vector3I vector3I2 = Vector3I.MinValue;
			foreach (Vector3I location in locations)
			{
				MySlimBlock cubeBlock = GetCubeBlock(location);
				if (cubeBlock != null)
				{
					removedBlocks.Add(location);
					vector3I = Vector3I.Min(vector3I, cubeBlock.Min);
					vector3I2 = Vector3I.Max(vector3I2, cubeBlock.Max);
					RemoveBlockByCubeBuilder(cubeBlock);
				}
			}
			if (Physics != null)
			{
				Physics.AddDirtyArea(vector3I, vector3I2);
			}
		}

		public void RazeGeneratedBlocks(List<Vector3I> locations)
		{
			Vector3I vector3I = Vector3I.MaxValue;
			Vector3I vector3I2 = Vector3I.MinValue;
			foreach (Vector3I location in locations)
			{
				MySlimBlock cubeBlock = GetCubeBlock(location);
				if (cubeBlock != null)
				{
					vector3I = Vector3I.Min(vector3I, cubeBlock.Min);
					vector3I2 = Vector3I.Max(vector3I2, cubeBlock.Max);
					RemoveBlockByCubeBuilder(cubeBlock);
				}
			}
			if (Physics != null)
			{
				Physics.AddDirtyArea(vector3I, vector3I2);
			}
		}

		private void RazeBlockInCompoundBlockSuccess(List<LocationIdentity> locationsAndIds, List<Tuple<Vector3I, ushort>> removedBlocks)
		{
			Vector3I min = Vector3I.MaxValue;
			Vector3I max = Vector3I.MinValue;
			foreach (LocationIdentity locationsAndId in locationsAndIds)
			{
				RemoveBlockInCompound(locationsAndId.Location, locationsAndId.Id, ref min, ref max, removedBlocks);
			}
			m_dirtyRegion.AddCubeRegion(min, max);
			ScheduleDirtyRegion();
			if (Physics != null)
			{
				Physics.AddDirtyArea(min, max);
			}
			MarkForDraw();
		}

		private void RemoveBlockInCompound(Vector3I position, ushort compoundBlockId, ref Vector3I min, ref Vector3I max, List<Tuple<Vector3I, ushort>> removedBlocks = null)
		{
			MySlimBlock cubeBlock = GetCubeBlock(position);
			if (cubeBlock != null && cubeBlock.FatBlock is MyCompoundCubeBlock)
			{
				MyCompoundCubeBlock compoundBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
				RemoveBlockInCompoundInternal(position, compoundBlockId, ref min, ref max, removedBlocks, cubeBlock, compoundBlock);
			}
		}

		public void RazeGeneratedBlocksInCompoundBlock(List<Tuple<Vector3I, ushort>> locationsAndIds)
		{
			Vector3I min = Vector3I.MaxValue;
			Vector3I max = Vector3I.MinValue;
			foreach (Tuple<Vector3I, ushort> locationsAndId in locationsAndIds)
			{
				MySlimBlock cubeBlock = GetCubeBlock(locationsAndId.Item1);
				if (cubeBlock != null && cubeBlock.FatBlock is MyCompoundCubeBlock)
				{
					MyCompoundCubeBlock compoundBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
					RemoveBlockInCompoundInternal(locationsAndId.Item1, locationsAndId.Item2, ref min, ref max, null, cubeBlock, compoundBlock);
				}
			}
			m_dirtyRegion.AddCubeRegion(min, max);
			if (Physics != null)
			{
				Physics.AddDirtyArea(min, max);
			}
			ScheduleDirtyRegion();
			MarkForDraw();
		}

		private void RemoveBlockInCompoundInternal(Vector3I position, ushort compoundBlockId, ref Vector3I min, ref Vector3I max, List<Tuple<Vector3I, ushort>> removedBlocks, MySlimBlock block, MyCompoundCubeBlock compoundBlock)
		{
			MySlimBlock block2 = compoundBlock.GetBlock(compoundBlockId);
			if (block2 != null && compoundBlock.Remove(block2))
			{
				removedBlocks?.Add(new Tuple<Vector3I, ushort>(position, compoundBlockId));
				min = Vector3I.Min(min, block.Min);
				max = Vector3I.Max(max, block.Max);
				if (MyCubeGridSmallToLargeConnection.Static != null && m_enableSmallToLargeConnections)
				{
					MyCubeGridSmallToLargeConnection.Static.RemoveBlockSmallToLargeConnection(block2);
				}
				NotifyBlockRemoved(block2);
			}
			if (compoundBlock.GetBlocksCount() == 0)
			{
				RemoveBlockByCubeBuilder(block);
			}
		}

		public void RazeGeneratedBlocks(List<MySlimBlock> generatedBlocks)
		{
			m_tmpRazeList.Clear();
			m_tmpLocations.Clear();
			foreach (MySlimBlock generatedBlock in generatedBlocks)
			{
				MySlimBlock cubeBlock = GetCubeBlock(generatedBlock.Position);
				if (cubeBlock == null)
				{
					continue;
				}
				if (cubeBlock.FatBlock is MyCompoundCubeBlock)
				{
					ushort? blockId = (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlockId(generatedBlock);
					if (blockId.HasValue)
					{
						m_tmpRazeList.Add(new Tuple<Vector3I, ushort>(generatedBlock.Position, blockId.Value));
					}
				}
				else
				{
					m_tmpLocations.Add(generatedBlock.Position);
				}
			}
			if (m_tmpLocations.Count > 0)
			{
				RazeGeneratedBlocks(m_tmpLocations);
			}
			if (m_tmpRazeList.Count > 0)
			{
				RazeGeneratedBlocksInCompoundBlock(m_tmpRazeList);
			}
			m_tmpRazeList.Clear();
			m_tmpLocations.Clear();
		}

		private void ScheduleDirtyRegion()
<<<<<<< HEAD
		{
			if (!m_dirtyRegionScheduled)
			{
				Schedule(UpdateQueue.AfterSimulation, UpdateDirtyRegion, 8, parallel: true);
				m_dirtyRegionScheduled = true;
			}
		}

		private void UpdateDirtyRegion()
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				ClearDirty();
				DeSchedule(UpdateQueue.AfterSimulation, UpdateDirtyRegion);
				m_dirtyRegionScheduled = false;
			}
			else if (!m_updatingDirty && m_dirtyRegion.IsDirty)
			{
				UpdateDirty();
			}
		}

		/// <summary>
		/// Color block in area. Verry slow.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="newHSV"></param>
		/// <param name="playSound"></param>
		/// <param name="validateOwnership"></param>
		public void ColorBlocks(Vector3I min, Vector3I max, Vector3 newHSV, bool playSound, bool validateOwnership)
		{
=======
		{
			if (!m_dirtyRegionScheduled)
			{
				Schedule(UpdateQueue.AfterSimulation, UpdateDirtyRegion, 8, parallel: true);
				m_dirtyRegionScheduled = true;
			}
		}

		private void UpdateDirtyRegion()
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				ClearDirty();
				DeSchedule(UpdateQueue.AfterSimulation, UpdateDirtyRegion);
				m_dirtyRegionScheduled = false;
			}
			else if (!m_updatingDirty && m_dirtyRegion.IsDirty)
			{
				UpdateDirty();
			}
		}

		public void ColorBlocks(Vector3I min, Vector3I max, Vector3 newHSV, bool playSound, bool validateOwnership)
		{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			long arg = (validateOwnership ? MySession.Static.LocalPlayerId : 0);
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.ColorBlockRequest, min, max, newHSV, playSound, arg);
		}

		public void ColorGrid(Vector3 newHSV, bool playSound, bool validateOwnership)
		{
			long arg = (validateOwnership ? MySession.Static.LocalPlayerId : 0);
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.ColorGridFriendlyRequest, newHSV, playSound, arg);
		}

<<<<<<< HEAD
		[Event(null, 4765)]
=======
		[Event(null, 4654)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private void ColorGridFriendlyRequest(Vector3 newHSV, bool playSound, long player)
		{
			if (ColorGridOrBlockRequestValidation(player))
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnColorGridFriendly, newHSV, playSound, player);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnColorGridBlockFailed, MyEventContext.Current.Sender);
			}
		}

<<<<<<< HEAD
		[Event(null, 4778)]
=======
		[Event(null, 4663)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private void OnColorGridFriendly(Vector3 newHSV, bool playSound, long player)
		{
<<<<<<< HEAD
=======
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!ColorGridOrBlockRequestValidation(player))
			{
				return;
			}
			bool flag = false;
<<<<<<< HEAD
			foreach (MySlimBlock cubeBlock in CubeBlocks)
			{
				flag |= ChangeColorAndSkin(cubeBlock, newHSV);
			}
			if (playSound && flag)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudColorBlock);
			}
			GridSystems.EmissiveSystem?.UpdateEmissivity();
		}

		[Event(null, 4803)]
		[Reliable]
		[Client]
		private void OnColorGridBlockFailed()
		{
			if (m_lastTimeDisplayedPaintFail + HUD_NOTIFICATION_TIMEOUT < MySandboxGame.TotalGamePlayTimeInMilliseconds)
			{
				MyHud.Notifications.Add(new MyHudNotification(MySpaceTexts.NotificationCannotPaint));
				m_lastTimeDisplayedPaintFail = MySandboxGame.TotalGamePlayTimeInMilliseconds;
=======
			Enumerator<MySlimBlock> enumerator = CubeBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					flag |= ChangeColorAndSkin(current, newHSV);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			if (playSound && flag)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudColorBlock);
			}
			GridSystems.EmissiveSystem?.UpdateEmissivity();
		}

<<<<<<< HEAD
		[Event(null, 4813)]
=======
		[Event(null, 4688)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private void ColorBlockRequest(Vector3I min, Vector3I max, Vector3 newHSV, bool playSound, long player)
		{
			if (ColorGridOrBlockRequestValidation(player))
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnColorBlock, min, max, newHSV, playSound, player);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnColorGridBlockFailed, MyEventContext.Current.Sender);
			}
		}

<<<<<<< HEAD
		[Event(null, 4827)]
=======
		[Event(null, 4697)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private void OnColorBlock(Vector3I min, Vector3I max, Vector3 newHSV, bool playSound, long player)
		{
			if (!ColorGridOrBlockRequestValidation(player))
			{
				return;
			}
			bool flag = false;
			Vector3I pos = default(Vector3I);
			pos.X = min.X;
			while (pos.X <= max.X)
			{
				pos.Y = min.Y;
				while (pos.Y <= max.Y)
				{
					pos.Z = min.Z;
					while (pos.Z <= max.Z)
					{
						MySlimBlock cubeBlock = GetCubeBlock(pos);
						if (cubeBlock != null)
						{
							flag |= ChangeColorAndSkin(cubeBlock, newHSV);
						}
						pos.Z++;
					}
					pos.Y++;
				}
				pos.X++;
			}
			if (playSound && flag && Vector3D.Distance(MySector.MainCamera.Position, Vector3D.Transform(min * GridSize, base.WorldMatrix)) < 200.0)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudColorBlock);
			}
			GridSystems.EmissiveSystem?.UpdateEmissivity();
		}

		public static MyGameInventoryItem GetArmorSkinItem(MyStringHash skinId)
		{
			if (skinId == MyStringHash.NullOrEmpty || MyGameService.InventoryItems == null)
			{
				return null;
			}
			foreach (MyGameInventoryItem inventoryItem in MyGameService.InventoryItems)
			{
				if (inventoryItem.ItemDefinition != null && inventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Armor && !(MyStringHash.GetOrCompute(inventoryItem.ItemDefinition.AssetModifierId) != skinId))
				{
					return inventoryItem;
				}
			}
			return null;
		}

		/// <summary>
		/// Skin block in area. Verry slow.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="newSkin"></param>
		/// <param name="playSound"></param>
		/// <param name="validateOwnership"></param>
		/// <param name="newHSV"></param>
		public void SkinBlocks(Vector3I min, Vector3I max, Vector3? newHSV, MyStringHash? newSkin, bool playSound, bool validateOwnership)
		{
			long arg = (validateOwnership ? MySession.Static.LocalPlayerId : 0);
			MyBlockVisuals arg2 = new MyBlockVisuals(newHSV.HasValue ? newHSV.Value.PackHSVToUint() : 0u, newSkin.HasValue ? newSkin.Value : MyStringHash.NullOrEmpty, newHSV.HasValue, newSkin.HasValue);
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.SkinBlockRequest, min, max, arg2, playSound, arg);
		}

		public void SkinGrid(Vector3 newHSV, MyStringHash newSkin, bool playSound, bool validateOwnership, bool applyColor, bool applySkin)
		{
			if (applyColor || applySkin)
			{
				long arg = (validateOwnership ? MySession.Static.LocalPlayerId : 0);
				MyMultiplayer.RaiseEvent(arg2: new MyBlockVisuals(newHSV.PackHSVToUint(), newSkin, applyColor, applySkin), arg1: this, action: (MyCubeGrid x) => x.SkinGridFriendlyRequest, arg3: playSound, arg4: arg);
			}
		}

<<<<<<< HEAD
		[Event(null, 4919)]
=======
		[Event(null, 4789)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private void SkinGridFriendlyRequest(MyBlockVisuals visuals, bool playSound, long player)
		{
			if (ColorGridOrBlockRequestValidation(player))
			{
				visuals.SkinId = MySession.Static.GetComponent<MySessionComponentGameInventory>()?.ValidateArmor(visuals.SkinId, MyEventContext.Current.Sender.Value) ?? MyStringHash.NullOrEmpty;
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnSkinGridFriendly, visuals, playSound, player);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnColorGridBlockFailed, MyEventContext.Current.Sender);
			}
		}

<<<<<<< HEAD
		[Event(null, 4936)]
=======
		[Event(null, 4801)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private void OnSkinGridFriendly(MyBlockVisuals visuals, bool playSound, long player)
		{
<<<<<<< HEAD
=======
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!ColorGridOrBlockRequestValidation(player))
			{
				return;
			}
			Vector3 value = ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV);
			bool flag = false;
<<<<<<< HEAD
			foreach (MySlimBlock cubeBlock in CubeBlocks)
			{
				flag |= ChangeColorAndSkin(cubeBlock, visuals.ApplyColor ? new Vector3?(value) : null, visuals.ApplySkin ? new MyStringHash?(visuals.SkinId) : null);
			}
			if (playSound && flag)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudColorBlock);
			}
			GridSystems.EmissiveSystem?.UpdateEmissivity();
		}

		[Event(null, 4964)]
=======
			Enumerator<MySlimBlock> enumerator = CubeBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					flag |= ChangeColorAndSkin(current, visuals.ApplyColor ? new Vector3?(value) : null, visuals.ApplySkin ? new MyStringHash?(visuals.SkinId) : null);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			if (playSound && flag)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudColorBlock);
			}
			GridSystems.EmissiveSystem?.UpdateEmissivity();
		}

		[Event(null, 4829)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private void SkinBlockRequest(Vector3I min, Vector3I max, MyBlockVisuals visuals, bool playSound, long player)
		{
			if (ColorGridOrBlockRequestValidation(player))
			{
				visuals.SkinId = MySession.Static.GetComponent<MySessionComponentGameInventory>()?.ValidateArmor(visuals.SkinId, MyEventContext.Current.Sender.Value) ?? MyStringHash.NullOrEmpty;
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnSkinBlock, min, max, visuals, playSound, player);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnColorGridBlockFailed, MyEventContext.Current.Sender);
			}
		}

<<<<<<< HEAD
		[Event(null, 4981)]
=======
		[Event(null, 4841)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private void OnSkinBlock(Vector3I min, Vector3I max, MyBlockVisuals visuals, bool playSound, long player)
		{
			if (!ColorGridOrBlockRequestValidation(player))
			{
				return;
			}
			Vector3 value = ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV);
			bool flag = false;
			Vector3I pos = default(Vector3I);
			pos.X = min.X;
			while (pos.X <= max.X)
			{
				pos.Y = min.Y;
				while (pos.Y <= max.Y)
				{
					pos.Z = min.Z;
					while (pos.Z <= max.Z)
					{
						MySlimBlock cubeBlock = GetCubeBlock(pos);
						if (cubeBlock != null)
						{
							flag |= ChangeColorAndSkin(cubeBlock, visuals.ApplyColor ? new Vector3?(value) : null, visuals.ApplySkin ? new MyStringHash?(visuals.SkinId) : null);
						}
						pos.Z++;
					}
					pos.Y++;
				}
				pos.X++;
			}
			if (playSound && flag && Vector3D.Distance(MySector.MainCamera.Position, Vector3D.Transform(min * GridSize, base.WorldMatrix)) < 200.0)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudColorBlock);
			}
			GridSystems.EmissiveSystem?.UpdateEmissivity();
		}

		public bool ColorGridOrBlockRequestValidation(long player)
		{
			if (player == 0L)
			{
				return true;
			}
			if (!Sync.IsServer)
			{
				return true;
			}
			if (BigOwners.Count == 0)
			{
				return true;
			}
			ulong num = MySession.Static.Players.TryGetSteamId(player);
			if (num != 0L && (MySession.Static.IsUserAdmin(num) || MySession.Static.IsUserSpaceMaster(num)))
			{
				return true;
			}
			using (List<long>.Enumerator enumerator = BigOwners.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (MyIDModule.GetRelationPlayerPlayer(enumerator.Current, player))
					{
					case MyRelationsBetweenPlayers.Self:
						return true;
					case MyRelationsBetweenPlayers.Allies:
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Builds block without checking connectivity
		/// </summary>
		private MySlimBlock BuildBlock(MyCubeBlockDefinition blockDefinition, Vector3 colorMaskHsv, MyStringHash skinId, Vector3I min, Quaternion orientation, long owner, long entityId, MyEntity builderEntity, MyObjectBuilder_CubeBlock blockObjectBuilder = null, bool updateVolume = true, bool testMerge = true, bool buildAsAdmin = false, string localizedDisplayNameBase = "")
		{
			MyBlockOrientation orientation2 = new MyBlockOrientation(ref orientation);
			if (blockObjectBuilder == null)
			{
				blockObjectBuilder = CreateBlockObjectBuilder(blockDefinition, min, orientation2, entityId, owner, builderEntity == null || !MySession.Static.SurvivalMode || buildAsAdmin);
				blockObjectBuilder.ColorMaskHSV = colorMaskHsv;
				blockObjectBuilder.SkinSubtypeId = skinId.String;
			}
			else
			{
				blockObjectBuilder.Min = min;
				blockObjectBuilder.Orientation = orientation;
			}
			MyCubeBuilder.BuildComponent.BeforeCreateBlock(blockDefinition, builderEntity, blockObjectBuilder, buildAsAdmin);
			MySlimBlock mySlimBlock = null;
			Vector3I vector3I = MySlimBlock.ComputePositionInGrid(new MatrixI(orientation2), blockDefinition, min);
			if (!MyEntities.IsInsideWorld(GridIntegerToWorld(vector3I)))
			{
				return null;
			}
			if (Sync.IsServer)
			{
				MyCubeBuilder.BuildComponent.GetBlockPlacementMaterials(blockDefinition, vector3I, blockObjectBuilder.BlockOrientation, this);
			}
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && MyCompoundCubeBlock.IsCompoundEnabled(blockDefinition))
			{
				MySlimBlock cubeBlock = GetCubeBlock(min);
				MyCompoundCubeBlock myCompoundCubeBlock = ((cubeBlock != null) ? (cubeBlock.FatBlock as MyCompoundCubeBlock) : null);
				if (myCompoundCubeBlock != null)
				{
					if (myCompoundCubeBlock.CanAddBlock(blockDefinition, new MyBlockOrientation(ref orientation)))
					{
						object obj = MyCubeBlockFactory.CreateCubeBlock(blockObjectBuilder);
						mySlimBlock = obj as MySlimBlock;
						if (mySlimBlock == null)
						{
							mySlimBlock = new MySlimBlock();
						}
						mySlimBlock.Init(blockObjectBuilder, this, obj as MyCubeBlock);
						mySlimBlock.FatBlock.HookMultiplayer();
						if (myCompoundCubeBlock.Add(mySlimBlock, out var _))
						{
							BoundsInclude(mySlimBlock);
							m_dirtyRegion.AddCube(min);
							ScheduleDirtyRegion();
							if (Physics != null)
							{
								Physics.AddDirtyBlock(cubeBlock);
							}
							NotifyBlockAdded(mySlimBlock);
						}
					}
				}
				else
				{
					MyObjectBuilder_CompoundCubeBlock objectBuilder = MyCompoundCubeBlock.CreateBuilder(blockObjectBuilder);
					mySlimBlock = AddBlock(objectBuilder, testMerge);
				}
				MarkForDraw();
			}
			else
			{
				mySlimBlock = AddBlock(blockObjectBuilder, testMerge);
			}
			if (mySlimBlock != null)
			{
				mySlimBlock.CubeGrid.BoundsInclude(mySlimBlock);
				if (updateVolume)
				{
					mySlimBlock.CubeGrid.UpdateGridAABB();
				}
				if (MyCubeGridSmallToLargeConnection.Static != null && m_enableSmallToLargeConnections)
				{
					MyCubeGridSmallToLargeConnection.Static.AddBlockSmallToLargeConnection(mySlimBlock);
				}
				if (Sync.IsServer)
				{
					MyCubeBuilder.BuildComponent.AfterSuccessfulBuild(builderEntity, buildAsAdmin);
				}
				MyCubeGrids.NotifyBlockBuilt(this, mySlimBlock);
			}
			return mySlimBlock;
		}

		internal void PerformCutouts(List<MyGridPhysics.ExplosionInfo> explosions)
		{
			if (explosions.Count == 0 || !MySession.Static.Settings.EnableVoxelDestruction)
			{
				return;
			}
			BoundingSphereD sphere = new BoundingSphereD(explosions[0].Position, explosions[0].Radius);
			for (int j = 0; j < explosions.Count; j++)
			{
				sphere.Include(new BoundingSphereD(explosions[j].Position, explosions[j].Radius));
			}
			using (MyUtils.ReuseCollection(ref m_rootVoxelsToCutTmp))
			{
				using (MyUtils.ReuseCollection(ref m_overlappingVoxelsTmp))
				{
					MySession.Static.VoxelMaps.GetAllOverlappingWithSphere(ref sphere, m_overlappingVoxelsTmp);
					foreach (MyVoxelBase item in m_overlappingVoxelsTmp)
					{
						m_rootVoxelsToCutTmp.Add(item.RootVoxel);
					}
					int skipCount = 0;
					Parallel.For(0, explosions.Count, delegate(int i)
					{
						//IL_009c: Unknown result type (might be due to invalid IL or missing references)
						//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
						MyGridPhysics.ExplosionInfo explosionInfo2 = explosions[i];
						BoundingSphereD sphere2 = new BoundingSphereD(explosionInfo2.Position, explosionInfo2.Radius);
						for (int k = 0; k < explosions.Count; k++)
						{
							if (k != i && new BoundingSphereD(explosions[k].Position, explosions[k].Radius).Contains(sphere2) == ContainmentType.Contains)
							{
								skipCount++;
								return;
							}
						}
						Enumerator<MyVoxelBase> enumerator3 = m_rootVoxelsToCutTmp.GetEnumerator();
						try
						{
<<<<<<< HEAD
							if (MyVoxelGenerator.CutOutSphereFast(item2, ref explosionInfo2.Position, explosionInfo2.Radius, out var cacheMin, out var cacheMax, notifyChanged: false))
=======
							while (enumerator3.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							{
								MyVoxelBase current3 = enumerator3.get_Current();
								if (MyVoxelGenerator.CutOutSphereFast(current3, ref explosionInfo2.Position, explosionInfo2.Radius, out var cacheMin, out var cacheMax, notifyChanged: false))
								{
									MyMultiplayer.RaiseEvent(current3, (MyVoxelBase x) => x.PerformCutOutSphereFast, explosionInfo2.Position, explosionInfo2.Radius, arg4: true);
									m_notificationQueue.Enqueue(MyTuple.Create(i, current3, cacheMin, cacheMax));
								}
							}
						}
						finally
						{
							((IDisposable)enumerator3).Dispose();
						}
					}, 1, WorkPriority.VeryHigh, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Voxels, "CutOutVoxel"), blocking: true);
				}
			}
			bool flag = false;
			BoundingBoxD boundaries = BoundingBoxD.CreateInvalid();
			foreach (MyTuple<int, MyVoxelBase, Vector3I, Vector3I> item2 in m_notificationQueue)
			{
				flag = true;
				MyGridPhysics.ExplosionInfo explosionInfo = explosions[item2.Item1];
				boundaries.Include(new BoundingSphereD(explosionInfo.Position, explosionInfo.Radius));
				Vector3I voxelRangeMin = item2.Item3;
				Vector3I voxelRangeMax = item2.Item4;
				item2.Item2.RootVoxel.Storage.NotifyRangeChanged(ref voxelRangeMin, ref voxelRangeMax, MyStorageDataTypeFlags.Content);
			}
			if (flag)
			{
				MyShapeBox myShapeBox = new MyShapeBox();
				myShapeBox.Boundaries = boundaries;
				MyTuple<int, MyVoxelBase, Vector3I, Vector3I> myTuple = default(MyTuple<int, MyVoxelBase, Vector3I, Vector3I>);
				while (m_notificationQueue.TryDequeue(ref myTuple))
				{
					BoundingBoxD cutOutBox = myShapeBox.GetWorldBoundaries();
					MyVoxelGenerator.NotifyVoxelChanged(MyVoxelBase.OperationType.Cut, myTuple.Item2, ref cutOutBox);
				}
			}
		}

		public void ResetBlockSkeleton(MySlimBlock block, bool updateSync = false)
		{
			MultiplyBlockSkeleton(block, 0f, updateSync);
		}

		public void MultiplyBlockSkeleton(MySlimBlock block, float factor, bool updateSync = false)
		{
			if (Skeleton == null)
			{
				MyLog.Default.WriteLine("Skeleton null in MultiplyBlockSkeleton!" + this);
			}
			if (Physics == null)
			{
				MyLog.Default.WriteLine("Physics null in MultiplyBlockSkeleton!" + this);
			}
			if (block == null || Skeleton == null || Physics == null)
			{
				return;
			}
			Vector3I vector3I = block.Min * 2;
			Vector3I vector3I2 = block.Max * 2 + 2;
			bool flag = false;
			Vector3I pos = default(Vector3I);
			pos.Z = vector3I.Z;
			while (pos.Z <= vector3I2.Z)
			{
				pos.Y = vector3I.Y;
				while (pos.Y <= vector3I2.Y)
				{
					pos.X = vector3I.X;
					while (pos.X <= vector3I2.X)
					{
						flag |= Skeleton.MultiplyBone(ref pos, factor, ref block.Min, this);
						pos.X++;
					}
					pos.Y++;
				}
				pos.Z++;
			}
			if (!flag)
			{
				return;
			}
			if (Sync.IsServer && updateSync)
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnBonesMultiplied, block.Position, factor);
			}
			vector3I = block.Min - Vector3I.One;
			vector3I2 = block.Max + Vector3I.One;
			pos.Z = vector3I.Z;
			while (pos.Z <= vector3I2.Z)
			{
				pos.Y = vector3I.Y;
				while (pos.Y <= vector3I2.Y)
				{
					pos.X = vector3I.X;
					while (pos.X <= vector3I2.X)
					{
						m_dirtyRegion.AddCube(pos);
						pos.X++;
					}
					pos.Y++;
				}
				pos.Z++;
			}
			Physics.AddDirtyArea(vector3I, vector3I2);
			ScheduleDirtyRegion();
			MarkForDraw();
		}

		public void AddDirtyBone(Vector3I gridPosition, Vector3I boneOffset)
		{
			Skeleton.Wrap(ref gridPosition, ref boneOffset);
			Vector3I value = boneOffset - new Vector3I(1, 1, 1);
			Vector3I start = Vector3I.Min(value, new Vector3I(0, 0, 0));
			Vector3I end = Vector3I.Max(value, new Vector3I(0, 0, 0));
			Vector3I next = start;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
			while (vector3I_RangeIterator.IsValid())
			{
				m_dirtyRegion.AddCube(gridPosition + next);
				vector3I_RangeIterator.GetNext(out next);
			}
			ScheduleDirtyRegion();
			MarkForDraw();
		}

		public MySlimBlock GetCubeBlock(Vector3I pos)
		{
<<<<<<< HEAD
			if (m_cubes.TryGetValue(pos, out var value))
=======
			MyCube myCube = default(MyCube);
			if (m_cubes.TryGetValue(pos, ref myCube))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return myCube.CubeBlock;
			}
			return null;
		}

		public MySlimBlock GetCubeBlock(Vector3I pos, ushort? compoundId)
		{
			if (!compoundId.HasValue)
			{
				return GetCubeBlock(pos);
			}
<<<<<<< HEAD
			if (m_cubes.TryGetValue(pos, out var value))
=======
			MyCube myCube = default(MyCube);
			if (m_cubes.TryGetValue(pos, ref myCube))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyCompoundCubeBlock myCompoundCubeBlock = myCube.CubeBlock.FatBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock != null)
				{
					return myCompoundCubeBlock.GetBlock(compoundId.Value);
				}
			}
			return null;
		}

		public T GetFirstBlockOfType<T>() where T : MyCubeBlock
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = m_cubeBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					if (current.FatBlock != null && current.FatBlock is T)
					{
						return current.FatBlock as T;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return null;
		}

		public void FixTargetCubeLite(out Vector3I cube, Vector3D fractionalGridPosition)
		{
			cube = Vector3I.Round(fractionalGridPosition - 0.5);
		}

		/// <summary>
		/// Iterate over all the neighbors of the cube and return when one of them exists.
		/// </summary> 
		public void FixTargetCube(out Vector3I cube, Vector3 fractionalGridPosition)
		{
			cube = Vector3I.Round(fractionalGridPosition);
			fractionalGridPosition += new Vector3(0.5f);
			if (m_cubes.ContainsKey(cube))
			{
				return;
			}
			Vector3 vector = fractionalGridPosition - cube;
			Vector3 vector2 = new Vector3(1f) - vector;
			m_neighborDistances[1] = vector.X;
			m_neighborDistances[0] = vector2.X;
			m_neighborDistances[3] = vector.Y;
			m_neighborDistances[2] = vector2.Y;
			m_neighborDistances[5] = vector.Z;
			m_neighborDistances[4] = vector2.Z;
			Vector3 vector3 = vector * vector;
			Vector3 vector4 = vector2 * vector2;
			m_neighborDistances[9] = (float)Math.Sqrt(vector3.X + vector3.Y);
			m_neighborDistances[8] = (float)Math.Sqrt(vector3.X + vector4.Y);
			m_neighborDistances[7] = (float)Math.Sqrt(vector4.X + vector3.Y);
			m_neighborDistances[6] = (float)Math.Sqrt(vector4.X + vector4.Y);
			m_neighborDistances[17] = (float)Math.Sqrt(vector3.X + vector3.Z);
			m_neighborDistances[16] = (float)Math.Sqrt(vector3.X + vector4.Z);
			m_neighborDistances[15] = (float)Math.Sqrt(vector4.X + vector3.Z);
			m_neighborDistances[14] = (float)Math.Sqrt(vector4.X + vector4.Z);
			m_neighborDistances[13] = (float)Math.Sqrt(vector3.Y + vector3.Z);
			m_neighborDistances[12] = (float)Math.Sqrt(vector3.Y + vector4.Z);
			m_neighborDistances[11] = (float)Math.Sqrt(vector4.Y + vector3.Z);
			m_neighborDistances[10] = (float)Math.Sqrt(vector4.Y + vector4.Z);
			Vector3 vector5 = vector3 * vector;
			Vector3 vector6 = vector4 * vector2;
			m_neighborDistances[25] = (float)Math.Pow(vector5.X + vector5.Y + vector5.Z, 0.33333333333333331);
			m_neighborDistances[24] = (float)Math.Pow(vector5.X + vector5.Y + vector6.Z, 0.33333333333333331);
			m_neighborDistances[23] = (float)Math.Pow(vector5.X + vector6.Y + vector5.Z, 0.33333333333333331);
			m_neighborDistances[22] = (float)Math.Pow(vector5.X + vector6.Y + vector6.Z, 0.33333333333333331);
			m_neighborDistances[21] = (float)Math.Pow(vector6.X + vector5.Y + vector5.Z, 0.33333333333333331);
			m_neighborDistances[20] = (float)Math.Pow(vector6.X + vector5.Y + vector6.Z, 0.33333333333333331);
			m_neighborDistances[19] = (float)Math.Pow(vector6.X + vector6.Y + vector5.Z, 0.33333333333333331);
			m_neighborDistances[18] = (float)Math.Pow(vector6.X + vector6.Y + vector6.Z, 0.33333333333333331);
			for (int i = 0; i < 25; i++)
			{
				for (int j = 0; j < 25 - i; j++)
				{
					float num = m_neighborDistances[(int)m_neighborOffsetIndices[j]];
					float num2 = m_neighborDistances[(int)m_neighborOffsetIndices[j + 1]];
					if (num > num2)
					{
						NeighborOffsetIndex value = m_neighborOffsetIndices[j];
						m_neighborOffsetIndices[j] = m_neighborOffsetIndices[j + 1];
						m_neighborOffsetIndices[j + 1] = value;
					}
				}
			}
			Vector3I vector3I = default(Vector3I);
			for (int k = 0; k < m_neighborOffsets.Count; k++)
			{
				vector3I = m_neighborOffsets[(int)m_neighborOffsetIndices[k]];
				if (m_cubes.ContainsKey(cube + vector3I))
				{
					cube += vector3I;
					break;
				}
			}
		}

		public HashSet<MySlimBlock> GetBlocks()
		{
			return m_cubeBlocks;
		}

		public ListReader<MyCubeBlock> GetFatBlocks()
		{
			return m_fatBlocks.ListUnsafe;
		}

		public MyFatBlockReader<T> GetFatBlocks<T>() where T : MyCubeBlock
		{
			return new MyFatBlockReader<T>(this);
		}

		/// <summary>
		/// Returns true when grid have at least one stand alone block 
		/// </summary>
		public bool HasStandAloneBlocks()
		{
			return m_standAloneBlockCount > 0;
		}

		/// <summary>
		/// Returns true when grid have at least one stand alone block 
		/// </summary>
		public static bool HasStandAloneBlocks(List<MySlimBlock> blocks, int offset, int count)
		{
			if (offset < 0)
			{
				MySandboxGame.Log.WriteLine($"Negative offset in HasStandAloneBlocks - {offset}");
				return false;
			}
			for (int i = offset; i < offset + count && i < blocks.Count; i++)
			{
				MySlimBlock mySlimBlock = blocks[i];
				if (mySlimBlock != null && mySlimBlock.BlockDefinition.IsStandAlone)
				{
					return true;
				}
			}
			return false;
		}

		private void CheckShouldCloseGrid()
		{
			if (!HasStandAloneBlocks() && !base.IsPreview && Sync.IsServer)
			{
				SetFadeOut(state: false);
				Close();
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Returns true when grid have at least one block which has physics (e.g. interior lights have no physics)
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool CanHavePhysics()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (m_canHavePhysics)
			{
				if (MyPerGameSettings.Game == GameEnum.SE_GAME)
				{
					Enumerator<MySlimBlock> enumerator = m_cubeBlocks.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.get_Current().BlockDefinition.HasPhysics)
							{
								return true;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					m_canHavePhysics = false;
				}
				else
				{
<<<<<<< HEAD
					m_canHavePhysics = m_cubeBlocks.Count > 0;
=======
					m_canHavePhysics = m_cubeBlocks.get_Count() > 0;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			return m_canHavePhysics;
		}

		/// <summary>
		/// Returns true when grid have at least one block which has physics (lights has no physics)
		/// </summary>
		public static bool CanHavePhysics(List<MySlimBlock> blocks, int offset, int count)
		{
			if (offset < 0)
			{
				MySandboxGame.Log.WriteLine($"Negative offset in CanHavePhysics - {offset}");
				return false;
			}
			for (int i = offset; i < offset + count && i < blocks.Count; i++)
			{
				MySlimBlock mySlimBlock = blocks[i];
				if (mySlimBlock != null && mySlimBlock.BlockDefinition.HasPhysics)
				{
					return true;
				}
			}
			return false;
		}

		private void RebuildGrid(bool staticPhysics = false)
		{
			if (!HasStandAloneBlocks() || !CanHavePhysics())
			{
				return;
			}
			RecalcBounds();
			RemoveRedundantParts();
			if (Physics != null)
			{
				Physics.Close();
				Physics = null;
			}
			if (CreatePhysics)
			{
				Physics = new MyGridPhysics(this, null, staticPhysics);
				RaisePhysicsChanged();
				if (!Sync.IsServer && !IsClientPredicted)
				{
					Physics.RigidBody.UpdateMotionType(HkMotionType.Fixed);
				}
			}
		}

		public new void RaisePhysicsChanged()
		{
			if (MyParallelEntityUpdateOrchestrator.ParallelUpdateInProgress)
			{
				Schedule(UpdateQueue.OnceAfterSimulation, base.RaisePhysicsChanged, 0);
			}
			else
			{
				base.RaisePhysicsChanged();
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Conversion from Unsupported station.
		/// For generic conversion to dynamic use physics component.
		/// Sets up the Unsupported station state.
		/// </summary>
		[Event(null, 5638)]
=======
		[Event(null, 5489)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		public void OnConvertToDynamic()
		{
			if (MyCubeGridSmallToLargeConnection.Static != null && m_enableSmallToLargeConnections)
			{
				MyCubeGridSmallToLargeConnection.Static.ConvertToDynamic(this);
			}
			IsStatic = false;
			IsUnsupportedStation = false;
			if (MyCubeGridGroups.Static != null)
			{
				MyCubeGridGroups.Static.UpdateDynamicState(this);
			}
			if (MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_GRID && !ForceDisablePrediction)
			{
				CheckPredictionFlagScheduling();
			}
			SetInventoryMassDirty();
			Physics.ConvertToDynamic(GridSizeEnum == MyCubeSize.Large, IsClientPredicted);
			RaisePhysicsChanged();
			Physics.RigidBody.AddGravity();
			RecalculateGravity();
			MyFixedGrids.UnmarkGridRoot(this);
		}

<<<<<<< HEAD
		/// <summary>
		/// Conversion to Unsupported station.
		/// For generic conversion to static use physics component.
		/// Sets up the Unsupported station state.
		/// </summary>
		[Event(null, 5673)]
=======
		[Event(null, 5524)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		public void ConvertToStatic()
		{
			if (!IsStatic && Physics != null && !((double)Physics.AngularVelocity.LengthSquared() > 0.0001) && !((double)Physics.LinearVelocity.LengthSquared() > 0.0001))
			{
				if (MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_GRID && !ForceDisablePrediction)
				{
					CheckPredictionFlagScheduling();
				}
				IsStatic = true;
				IsUnsupportedStation = true;
				Physics.ConvertToStatic();
				RaisePhysicsChanged();
				MyFixedGrids.MarkGridRoot(this);
			}
		}

		private void CheckConvertToDynamic()
		{
			if (TestDynamic != 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnConvertedToShipRequest, TestDynamic);
				TestDynamic = MyTestDynamicReason.NoReason;
			}
		}

		public void DoDamage(float damage, MyHitInfo hitInfo, Vector3? localPos = null, long attackerId = 0L)
		{
			if (!Sync.IsServer || !MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.Damage, 0L, 0uL))
			{
				return;
			}
			Vector3I cube;
			if (localPos.HasValue)
			{
				FixTargetCube(out cube, localPos.Value * GridSizeR);
			}
			else
			{
				FixTargetCube(out cube, Vector3D.Transform(hitInfo.Position, base.PositionComp.WorldMatrixInvScaled) * GridSizeR);
			}
			MySlimBlock mySlimBlock = GetCubeBlock(cube);
			if (mySlimBlock == null)
			{
				return;
			}
			if (MyFakes.ENABLE_FRACTURE_COMPONENT)
			{
				ushort? num = null;
				MyCompoundCubeBlock myCompoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock != null)
				{
					num = Physics.GetContactCompoundId(mySlimBlock.Position, hitInfo.Position);
					if (!num.HasValue)
					{
						return;
					}
					MySlimBlock block = myCompoundCubeBlock.GetBlock(num.Value);
					if (block == null)
					{
						return;
					}
					mySlimBlock = block;
				}
			}
			ApplyDestructionDeformation(mySlimBlock, damage, hitInfo, attackerId);
		}

		public void ApplyDestructionDeformation(MySlimBlock block, float damage = 1f, MyHitInfo? hitInfo = null, long attackerId = 0L)
		{
			if (MyPerGameSettings.Destruction)
			{
<<<<<<< HEAD
				((IMyDestroyableObject)block).DoDamage(damage, MyDamageType.Deformation, sync: true, hitInfo, attackerId, 0L, shouldDetonateAmmo: true);
=======
				((IMyDestroyableObject)block).DoDamage(damage, MyDamageType.Deformation, sync: true, hitInfo, attackerId, 0L);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return;
			}
			EnqueueDestructionDeformationBlock(block.Position);
			ApplyDestructionDeformationInternal(block, sync: true, damage, attackerId);
		}

		private void ApplyDeformationPostponed()
		{
			if (m_deformationPostponed.Count <= 0)
			{
				return;
			}
			List<DeformationPostponedItem> cloned = m_deformationPostponed;
			Parallel.Start(delegate
			{
				foreach (DeformationPostponedItem item in cloned)
				{
					ApplyDestructionDeformationInternal(item);
				}
				cloned.Clear();
				m_postponedListsPool.Return(cloned);
			});
			m_deformationPostponed = m_postponedListsPool.Get();
			m_deformationPostponed.Clear();
		}

		private void ApplyDestructionDeformationInternal(DeformationPostponedItem item)
		{
			if (!MySession.Static.HighSimulationQuality || base.Closed)
			{
				return;
			}
			if (m_deformationRng == null)
			{
				m_deformationRng = new MyRandom();
			}
			Vector3I minCube = Vector3I.MaxValue;
			Vector3I maxCube = Vector3I.MinValue;
			bool flag = false;
			for (int i = -1; i <= 1; i += 2)
			{
				for (int j = -1; j <= 1; j += 2)
				{
					flag |= MoveCornerBones(item.Min, new Vector3I(i, 0, j), ref minCube, ref maxCube);
					flag |= MoveCornerBones(item.Min, new Vector3I(i, j, 0), ref minCube, ref maxCube);
					flag |= MoveCornerBones(item.Min, new Vector3I(0, i, j), ref minCube, ref maxCube);
				}
			}
			if (flag)
			{
				m_dirtyRegion.AddCubeRegion(minCube, maxCube);
				ScheduleDirtyRegion();
			}
			m_deformationRng.SetSeed(item.Position.GetHashCode());
			float angleDeviation = (float)Math.PI / 8f;
			float gridSizeQuarter = GridSizeQuarter;
			Vector3I min = item.Min;
			for (int k = 0; k < 3; k++)
			{
				Vector3I dirtyMin = Vector3I.MaxValue;
				Vector3I dirtyMax = Vector3I.MinValue;
				flag = false;
				flag |= ApplyTable(min, MyCubeGridDeformationTables.ThinUpper[k], ref dirtyMin, ref dirtyMax, m_deformationRng, gridSizeQuarter, angleDeviation);
				if (flag | ApplyTable(min, MyCubeGridDeformationTables.ThinLower[k], ref dirtyMin, ref dirtyMax, m_deformationRng, gridSizeQuarter, angleDeviation))
				{
					dirtyMin -= Vector3I.One;
					dirtyMax += Vector3I.One;
					minCube = min;
					maxCube = min;
					Skeleton.Wrap(ref minCube, ref dirtyMin);
					Skeleton.Wrap(ref maxCube, ref dirtyMax);
					m_dirtyRegion.AddCubeRegion(minCube, maxCube);
					ScheduleDirtyRegion();
				}
			}
			MySandboxGame.Static.Invoke(delegate
			{
				MarkForDraw();
			}, "ApplyDestructionDeformationInternal::MarkForDraw");
		}

		private float ApplyDestructionDeformationInternal(MySlimBlock block, bool sync, float damage = 1f, long attackerId = 0L, bool postponed = false)
		{
			if (!BlocksDestructionEnabled)
			{
				return 0f;
			}
			if (block.UseDamageSystem)
			{
				MyDamageInformation info = new MyDamageInformation(isDeformation: true, 1f, MyDamageType.Deformation, attackerId);
				MyDamageSystem.Static.RaiseBeforeDamageApplied(block, ref info);
				if (info.Amount == 0f)
				{
					return 0f;
				}
			}
			DeformationPostponedItem deformationPostponedItem = default(DeformationPostponedItem);
			deformationPostponedItem.Position = block.Position;
			deformationPostponedItem.Min = block.Min;
			deformationPostponedItem.Max = block.Max;
			DeformationPostponedItem item = deformationPostponedItem;
			m_totalBoneDisplacement = 0f;
			if (postponed)
			{
				m_deformationPostponed.Add(item);
				Schedule(UpdateQueue.OnceAfterSimulation, ApplyDeformationPostponed, 1, parallel: true);
			}
			else
			{
				ApplyDestructionDeformationInternal(item);
			}
			if (sync)
			{
				float amount = m_totalBoneDisplacement * GridSize * 10f * damage;
				MyDamageInformation info2 = new MyDamageInformation(isDeformation: false, amount, MyDamageType.Deformation, attackerId);
				if (block.UseDamageSystem)
				{
					MyDamageSystem.Static.RaiseBeforeDamageApplied(block, ref info2);
				}
				if (info2.Amount > 0f)
				{
<<<<<<< HEAD
					((IMyDestroyableObject)block).DoDamage(info2.Amount, MyDamageType.Deformation, sync: true, (MyHitInfo?)null, attackerId, 0L, shouldDetonateAmmo: true);
=======
					((IMyDestroyableObject)block).DoDamage(info2.Amount, MyDamageType.Deformation, sync: true, (MyHitInfo?)null, attackerId, 0L);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			return m_totalBoneDisplacement;
		}

		/// <summary>
		/// Removes destroyed block, applies damage and deformation to close blocks
		/// Won't update physics!
		/// </summary>
		public void RemoveDestroyedBlock(MySlimBlock block, long attackerId = 0L)
		{
			if (!Sync.IsServer)
			{
				if (!MyFakes.ENABLE_FRACTURE_COMPONENT)
				{
					block.OnDestroyVisual();
				}
			}
			else
			{
				if (Physics == null)
				{
					return;
				}
				if (MyFakes.ENABLE_FRACTURE_COMPONENT)
				{
					MySlimBlock cubeBlock = GetCubeBlock(block.Position);
					if (cubeBlock == null)
					{
						return;
					}
					if (cubeBlock == block)
					{
						EnqueueDestroyedBlockWithId(block.Position, null);
						RemoveDestroyedBlockInternal(block);
						Physics.AddDirtyBlock(block);
					}
					else
					{
						MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
						if (myCompoundCubeBlock != null)
						{
							ushort? blockId = myCompoundCubeBlock.GetBlockId(block);
							if (blockId.HasValue)
							{
								EnqueueDestroyedBlockWithId(block.Position, blockId);
								RemoveDestroyedBlockInternal(block);
								Physics.AddDirtyBlock(block);
							}
						}
					}
					MyFractureComponentCubeBlock fractureComponent = block.GetFractureComponent();
					if (fractureComponent != null)
					{
						MyDestructionHelper.CreateFracturePiece(fractureComponent, sync: true);
					}
				}
				else
				{
					EnqueueDestroyedBlock(block.Position);
					RemoveDestroyedBlockInternal(block);
					Physics.AddDirtyBlock(block);
				}
			}
		}

		private void RemoveDestroyedBlockInternal(MySlimBlock block)
		{
			ApplyDestructionDeformationInternal(block, sync: false, 1f, 0L, postponed: true);
			((IMyDestroyableObject)block).OnDestroy();
			MySlimBlock cubeBlock = GetCubeBlock(block.Position);
			if (cubeBlock == block)
			{
				RemoveBlockInternal(block, close: true);
			}
			else
			{
				if (cubeBlock == null)
				{
					return;
				}
				MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock != null)
				{
					ushort? blockId = myCompoundCubeBlock.GetBlockId(block);
					if (blockId.HasValue)
					{
						Vector3I min = Vector3I.MaxValue;
						Vector3I max = Vector3I.MinValue;
						RemoveBlockInCompound(block.Position, blockId.Value, ref min, ref max);
					}
				}
			}
		}

		private bool ApplyTable(Vector3I cubePos, MyCubeGridDeformationTables.DeformationTable table, ref Vector3I dirtyMin, ref Vector3I dirtyMax, MyRandom random, float maxLinearDeviation, float angleDeviation)
		{
			if (!m_cubes.ContainsKey(cubePos + table.Normal))
			{
				float maxValue = GridSize / 10f;
				using (MyUtils.ReuseCollection(ref m_tmpCubeSet))
				{
					GetExistingCubes(cubePos, (IEnumerable<Vector3I>)table.CubeOffsets, m_tmpCubeSet);
					int num = 0;
					if (m_tmpCubeSet.Count > 0)
					{
						foreach (KeyValuePair<Vector3I, Matrix> item in table.OffsetTable)
						{
							Vector3I vector3I = item.Key >> 1;
							Vector3I vector3I2 = item.Key - Vector3I.One >> 1;
							if (m_tmpCubeSet.ContainsKey(vector3I) || (vector3I != vector3I2 && m_tmpCubeSet.ContainsKey(vector3I2)))
							{
								Vector3I boneOffset = item.Key;
								Vector3 clamp = new Vector3(GridSizeQuarter - random.NextFloat(0f, maxValue));
								Matrix matrix = item.Value;
								Vector3 moveDirection = random.NextDeviatingVector(ref matrix, angleDeviation) * random.NextFloat(1f, maxLinearDeviation);
								float displacementLength = moveDirection.Max();
								MoveBone(ref cubePos, ref boneOffset, ref moveDirection, ref displacementLength, ref clamp);
								num++;
							}
						}
					}
					m_tmpCubeSet.Clear();
				}
				dirtyMin = Vector3I.Min(dirtyMin, table.MinOffset);
				dirtyMax = Vector3I.Max(dirtyMax, table.MaxOffset);
				return true;
			}
			return false;
		}

<<<<<<< HEAD
		/// <summary>
		/// Client only method, not called on server
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void BlocksRemoved(List<Vector3I> blocksToRemove)
		{
			foreach (Vector3I item in blocksToRemove)
			{
				MySlimBlock cubeBlock = GetCubeBlock(item);
				if (cubeBlock != null)
				{
					RemoveBlockInternal(cubeBlock, close: true);
					Physics.AddDirtyBlock(cubeBlock);
				}
			}
		}

		private void BlocksWithIdRemoved(List<BlockPositionId> blocksToRemove)
		{
			foreach (BlockPositionId item in blocksToRemove)
			{
				if (item.CompoundId > 65535)
				{
					MySlimBlock cubeBlock = GetCubeBlock(item.Position);
					if (cubeBlock != null)
					{
						RemoveBlockInternal(cubeBlock, close: true);
						Physics.AddDirtyBlock(cubeBlock);
					}
					continue;
				}
				Vector3I min = Vector3I.MaxValue;
				Vector3I max = Vector3I.MinValue;
				RemoveBlockInCompound(item.Position, (ushort)item.CompoundId, ref min, ref max);
				if (min != Vector3I.MaxValue)
				{
					Physics.AddDirtyArea(min, max);
				}
			}
		}

		private void BlocksDestroyed(List<Vector3I> blockToDestroy)
		{
			m_largeDestroyInProgress = blockToDestroy.Count > BLOCK_LIMIT_FOR_LARGE_DESTRUCTION;
			foreach (Vector3I item in blockToDestroy)
			{
				MySlimBlock cubeBlock = GetCubeBlock(item);
				if (cubeBlock != null)
				{
					RemoveDestroyedBlockInternal(cubeBlock);
					Physics.AddDirtyBlock(cubeBlock);
				}
			}
			m_largeDestroyInProgress = false;
		}

		private void BlocksDeformed(List<Vector3I> blockToDestroy)
		{
			foreach (Vector3I item in blockToDestroy)
			{
				MySlimBlock cubeBlock = GetCubeBlock(item);
				if (cubeBlock != null)
				{
					ApplyDestructionDeformationInternal(cubeBlock, sync: false, 1f, 0L);
					Physics.AddDirtyBlock(cubeBlock);
				}
			}
		}

<<<<<<< HEAD
		[Event(null, 6111)]
=======
		[Event(null, 5962)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void BlockIntegrityChanged(Vector3I pos, ushort subBlockId, float buildIntegrity, float integrity, MyIntegrityChangeEnum integrityChangeType, long grinderOwner)
		{
			MyCompoundCubeBlock myCompoundCubeBlock = null;
			MySlimBlock mySlimBlock = GetCubeBlock(pos);
			if (mySlimBlock != null)
			{
				myCompoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
			}
			if (myCompoundCubeBlock != null)
			{
				mySlimBlock = myCompoundCubeBlock.GetBlock(subBlockId);
			}
			mySlimBlock?.SetIntegrity(buildIntegrity, integrity, integrityChangeType, grinderOwner);
		}

<<<<<<< HEAD
		[Event(null, 6128)]
=======
		[Event(null, 5979)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void BlockStockpileChanged(Vector3I pos, ushort subBlockId, List<MyStockpileItem> items)
		{
			MySlimBlock mySlimBlock = GetCubeBlock(pos);
			MyCompoundCubeBlock myCompoundCubeBlock = null;
			if (mySlimBlock != null)
			{
				myCompoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
			}
			if (myCompoundCubeBlock != null)
			{
				mySlimBlock = myCompoundCubeBlock.GetBlock(subBlockId);
			}
			mySlimBlock?.ChangeStockpile(items);
		}

<<<<<<< HEAD
		[Event(null, 6147)]
=======
		[Event(null, 5998)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void FractureComponentRepaired(Vector3I pos, ushort subBlockId, long toolOwner)
		{
			MyCompoundCubeBlock myCompoundCubeBlock = null;
			MySlimBlock mySlimBlock = GetCubeBlock(pos);
			if (mySlimBlock != null)
			{
				myCompoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
			}
			if (myCompoundCubeBlock != null)
			{
				mySlimBlock = myCompoundCubeBlock.GetBlock(subBlockId);
			}
			if (mySlimBlock != null && mySlimBlock.FatBlock != null)
			{
				mySlimBlock.RepairFracturedBlock(toolOwner);
			}
		}

		private void RemoveBlockByCubeBuilder(MySlimBlock block)
		{
			RemoveBlockInternal(block, close: true);
			if (block.FatBlock != null)
			{
				block.FatBlock.OnRemovedByCubeBuilder();
			}
		}

		/// <summary>
		/// Removes block, should be used only by server or on server request
		/// </summary>
		private void RemoveBlockInternal(MySlimBlock block, bool close, bool markDirtyDisconnects = true)
		{
			if (!m_cubeBlocks.Contains(block))
			{
				return;
			}
			if (block.BlockDefinition.IsStandAlone && --m_standAloneBlockCount == 0)
			{
				Schedule(UpdateQueue.OnceAfterSimulation, CheckShouldCloseGrid);
			}
			if (MyFakes.ENABLE_MULTIBLOCK_PART_IDS)
			{
				RemoveMultiBlockInfo(block);
			}
			RenderData.RemoveDecals(block.Position, immediately: true);
			MyTerminalBlock myTerminalBlock = block.FatBlock as MyTerminalBlock;
			if (myTerminalBlock != null)
			{
				for (int i = 0; i < BlockGroups.Count; i++)
				{
					MyBlockGroup myBlockGroup = BlockGroups[i];
					if (myBlockGroup.Blocks.Contains(myTerminalBlock))
					{
						myBlockGroup.Blocks.Remove(myTerminalBlock);
					}
<<<<<<< HEAD
					if (myBlockGroup.Blocks.Count <= 0)
=======
					if (myBlockGroup.Blocks.get_Count() <= 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						RemoveGroup(myBlockGroup);
						i--;
					}
				}
			}
			RemoveBlockParts(block);
			Parallel.Start(delegate
			{
				RemoveBlockEdges(block);
			});
			if (block.FatBlock != null)
			{
				if (block.FatBlock.InventoryCount > 0)
				{
					UnregisterInventory(block.FatBlock);
				}
				if (BlocksCounters.ContainsKey(block.BlockDefinition.Id.TypeId))
				{
					BlocksCounters[block.BlockDefinition.Id.TypeId]--;
				}
				block.FatBlock.IsBeingRemoved = true;
				GridSystems.UnregisterFromSystems(block.FatBlock);
				if (close)
				{
					block.FatBlock.Close();
				}
				else
				{
					base.Hierarchy.RemoveChild(block.FatBlock);
				}
				if (block.FatBlock.Render.NeedsDrawFromParent)
				{
					m_blocksForDraw.Remove(block.FatBlock);
					block.FatBlock.Render.SetVisibilityUpdates(state: false);
				}
			}
			block.RemoveNeighbours();
			block.RemoveAuthorship();
			m_PCU -= (block.ComponentStack.IsFunctional ? block.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST);
			m_cubeBlocks.Remove(block);
			if (block.FatBlock != null)
			{
				if (block.FatBlock is MyReactor)
				{
					NumberOfReactors--;
				}
				m_fatBlocks.Remove(block.FatBlock);
				block.FatBlock.IsBeingRemoved = false;
			}
			if (m_colorStatistics.ContainsKey(block.ColorMaskHSV))
			{
				m_colorStatistics[block.ColorMaskHSV]--;
				if (m_colorStatistics[block.ColorMaskHSV] <= 0)
				{
					m_colorStatistics.Remove(block.ColorMaskHSV);
				}
			}
			if (markDirtyDisconnects && m_disconnectsDirty == MyTestDisconnectsReason.NoReason)
			{
				m_disconnectsDirty = MyTestDisconnectsReason.BlockRemoved;
				Schedule(UpdateQueue.OnceAfterSimulation, DetectDisconnects, 18, parallel: true);
			}
			Vector3I pos = block.Min;
			bool flag = !Skeleton.HasUnusedBones;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref block.Min, ref block.Max);
			while (vector3I_RangeIterator.IsValid())
			{
				Skeleton.MarkCubeRemoved(ref pos);
				vector3I_RangeIterator.GetNext(out pos);
			}
			if (flag)
			{
				Schedule(UpdateQueue.OnceAfterSimulation, RemoveUnsusedBones, 19, parallel: true);
			}
			if (block.FatBlock != null && block.FatBlock.IDModule != null)
			{
				ChangeOwner(block.FatBlock, block.FatBlock.IDModule.Owner, 0L);
			}
			if (MyCubeGridSmallToLargeConnection.Static != null && m_enableSmallToLargeConnections)
			{
				MyCubeGridSmallToLargeConnection.Static.RemoveBlockSmallToLargeConnection(block);
			}
			NotifyBlockRemoved(block);
			if (close)
			{
				NotifyBlockClosed(block);
			}
			m_boundsDirty = true;
			MarkForDraw();
		}

		public void RemoveBlock(MySlimBlock block, bool updatePhysics = false)
		{
			if (Sync.IsServer && m_cubeBlocks.Contains(block))
			{
				EnqueueRemovedBlock(block.Min);
				RemoveBlockInternal(block, close: true);
				if (updatePhysics)
				{
					Physics.AddDirtyBlock(block);
				}
			}
		}

		public void RemoveBlockWithId(MySlimBlock block, bool updatePhysics = false)
		{
			MySlimBlock cubeBlock = GetCubeBlock(block.Min);
			if (cubeBlock == null)
			{
				return;
			}
			MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
			ushort? compoundId = null;
			if (myCompoundCubeBlock != null)
			{
				compoundId = myCompoundCubeBlock.GetBlockId(block);
				if (!compoundId.HasValue)
				{
					return;
				}
			}
			RemoveBlockWithId(block.Min, compoundId, updatePhysics);
		}

		public void RemoveBlockWithId(Vector3I position, ushort? compoundId, bool updatePhysics = false)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			MySlimBlock cubeBlock = GetCubeBlock(position);
			if (cubeBlock != null)
			{
				EnqueueRemovedBlockWithId(cubeBlock.Min, compoundId);
				if (compoundId.HasValue)
				{
					Vector3I min = Vector3I.Zero;
					Vector3I max = Vector3I.Zero;
					RemoveBlockInCompound(cubeBlock.Min, compoundId.Value, ref min, ref max);
				}
				else
				{
					RemoveBlockInternal(cubeBlock, close: true);
				}
				if (updatePhysics)
				{
					Physics.AddDirtyBlock(cubeBlock);
				}
			}
		}

		public void UpdateBlockNeighbours(MySlimBlock block)
		{
			if (m_cubeBlocks.Contains(block))
			{
				block.RemoveNeighbours();
				block.AddNeighbours();
				if (m_disconnectsDirty == MyTestDisconnectsReason.NoReason)
				{
					m_disconnectsDirty = MyTestDisconnectsReason.SplitBlock;
					Schedule(UpdateQueue.OnceAfterSimulation, DetectDisconnects, 18, parallel: true);
				}
			}
		}

		/// <summary>
		/// Returns cube corner which is closest to position
		/// </summary>
		public Vector3 GetClosestCorner(Vector3I gridPos, Vector3 position)
		{
			return gridPos * GridSize - Vector3.SignNonZero(gridPos * GridSize - position) * GridSizeHalf;
		}

		public void DetectDisconnectsAfterFrame()
		{
			if (m_disconnectsDirty == MyTestDisconnectsReason.NoReason)
			{
				m_disconnectsDirty = MyTestDisconnectsReason.BlockRemoved;
				Schedule(UpdateQueue.OnceAfterSimulation, DetectDisconnects, 18, parallel: true);
			}
		}

		private void DetectDisconnects()
		{
			if (MyFakes.DETECT_DISCONNECTS && m_cubes.get_Count() != 0 && Sync.IsServer)
			{
				m_disconnectHelper.Disconnect(this, m_disconnectsDirty);
				m_disconnectsDirty = MyTestDisconnectsReason.NoReason;
			}
		}

		public bool CubeExists(Vector3I pos)
		{
			return m_cubes.ContainsKey(pos);
		}

		public void UpdateDirty(Action callback = null, bool immediate = false)
		{
			if (!m_updatingDirty && m_resolvingSplits == 0)
			{
				m_updatingDirty = true;
				MyDirtyRegion dirtyRegion = m_dirtyRegion;
				m_dirtyRegion = m_dirtyRegionParallel;
				m_dirtyRegionParallel = dirtyRegion;
				if (immediate)
				{
					UpdateDirtyInternal();
					callback?.Invoke();
					OnUpdateDirtyCompleted();
				}
				else
				{
					Parallel.Start(m_UpdateDirtyInternal, callback = (Action)Delegate.Combine(callback, m_OnUpdateDirtyCompleted));
				}
			}
		}

		private void ClearDirty()
		{
			if (!m_updatingDirty && m_resolvingSplits == 0)
			{
				MyDirtyRegion dirtyRegion = m_dirtyRegion;
				m_dirtyRegion = m_dirtyRegionParallel;
				m_dirtyRegionParallel = dirtyRegion;
				m_dirtyRegionParallel.Cubes.Clear();
				MyCube myCube = default(MyCube);
				while (m_dirtyRegionParallel.PartsToRemove.TryDequeue(ref myCube))
				{
				}
			}
		}

		private void OnUpdateDirtyCompleted()
		{
			if (base.InScene)
			{
				UpdateInstanceData();
			}
			m_dirtyRegionParallel.Clear();
			m_updatingDirty = false;
			m_dirtyRegionScheduled = false;
			if (!m_dirtyRegion.IsDirty)
			{
				DeSchedule(UpdateQueue.AfterSimulation, UpdateDirtyRegion);
			}
			MarkForDraw();
			ReleaseMerginGrids();
		}

		public void UpdateDirtyInternal()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			using (Pin())
			{
				if (base.MarkedForClose)
				{
					return;
				}
				m_dirtyRegionParallel.Cubes.ApplyChanges();
<<<<<<< HEAD
				foreach (Vector3I cube in m_dirtyRegionParallel.Cubes)
				{
					UpdateParts(cube);
				}
				MyCube result;
				while (m_dirtyRegionParallel.PartsToRemove.TryDequeue(out result))
				{
					UpdateParts(result.CubeBlock.Position);
					MyCubePart[] parts = result.Parts;
=======
				Enumerator<Vector3I> enumerator = m_dirtyRegionParallel.Cubes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Vector3I current = enumerator.get_Current();
						UpdateParts(current);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				MyCube myCube = default(MyCube);
				while (m_dirtyRegionParallel.PartsToRemove.TryDequeue(ref myCube))
				{
					UpdateParts(myCube.CubeBlock.Position);
					MyCubePart[] parts = myCube.Parts;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					foreach (MyCubePart part in parts)
					{
						Render.RenderData.RemoveCubePart(part);
					}
				}
<<<<<<< HEAD
				foreach (Vector3I cube2 in m_dirtyRegionParallel.Cubes)
				{
					MySlimBlock cubeBlock = GetCubeBlock(cube2);
					if (cubeBlock != null && cubeBlock.ShowParts && MyFakes.ENABLE_EDGES)
					{
						if (cubeBlock.Dithering >= 0f)
=======
				enumerator = m_dirtyRegionParallel.Cubes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Vector3I current2 = enumerator.get_Current();
						MySlimBlock cubeBlock = GetCubeBlock(current2);
						if (cubeBlock != null && cubeBlock.ShowParts && MyFakes.ENABLE_EDGES)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							AddBlockEdges(cubeBlock);
						}
						else
						{
							RemoveBlockEdges(cubeBlock);
						}
						cubeBlock.UpdateMaxDeformation();
					}
					if (cubeBlock != null && cubeBlock.FatBlock != null && cubeBlock.FatBlock.Render != null && cubeBlock.FatBlock.Render.NeedsDrawFromParent)
					{
						m_blocksForDraw.Add(cubeBlock.FatBlock);
						cubeBlock.FatBlock.Render.SetVisibilityUpdates(state: true);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
		}

		public bool IsDirty()
		{
			return m_dirtyRegion.IsDirty;
		}

		private void CheckEarlyDeactivation()
		{
			if (Physics == null)
			{
				return;
			}
			bool flag = false;
			if (IsMarkedForEarlyDeactivation)
			{
				if (!Physics.IsStatic)
				{
					flag = true;
					Physics.ConvertToStatic();
				}
			}
			else if (!IsStatic && Physics.IsStatic)
			{
				flag = true;
				Physics.ConvertToDynamic(GridSizeEnum == MyCubeSize.Large, isPredicted: false);
			}
			if (flag)
			{
				RaisePhysicsChanged();
			}
		}

		public void UpdateInstanceData()
		{
			Render.RebuildDirtyCells();
		}

		private void UpdateLinearVelocityAfterSimulation()
		{
			if (Physics != null)
			{
				LinearVelocity = Physics.LinearVelocityUnsafe;
			}
			else
			{
				LinearVelocity = Vector3.Zero;
			}
		}

		public bool TryGetCube(Vector3I position, out MyCube cube)
		{
			return m_cubes.TryGetValue(position, ref cube);
		}

		/// <summary>
		/// Add new cube in the grid
		/// </summary>
		/// <param name="block"></param>
		/// <param name="pos"></param>
		/// <param name="rotation"></param>
		/// <param name="cubeBlockDefinition"></param>
		/// <returns>false if add failed (can be caused be block structure change during the development</returns>
		private bool AddCube(MySlimBlock block, ref Vector3I pos, Matrix rotation, MyCubeBlockDefinition cubeBlockDefinition)
		{
			MyCube myCube = new MyCube
			{
				Parts = GetCubeParts(block.SkinSubtypeId, cubeBlockDefinition, pos, rotation, GridSize, GridScale),
				CubeBlock = block
			};
			MyCube orAdd = m_cubes.GetOrAdd(pos, myCube);
			if (myCube != orAdd)
			{
				return false;
			}
			m_dirtyRegion.AddCube(pos);
			ScheduleDirtyRegion();
			MarkForDraw();
			return true;
		}

		private MyCube CreateCube(MySlimBlock block, Vector3I pos, Matrix rotation, MyCubeBlockDefinition cubeBlockDefinition)
		{
			return new MyCube
			{
				Parts = GetCubeParts(block.SkinSubtypeId, cubeBlockDefinition, pos, rotation, GridSize, GridScale),
				CubeBlock = block
			};
		}

		public bool ChangeColorAndSkin(MySlimBlock block, Vector3? newHSV = null, MyStringHash? skinSubtypeId = null)
		{
			try
			{
				MyStringHash skinSubtypeId2 = block.SkinSubtypeId;
				MyStringHash? myStringHash = skinSubtypeId;
				if (skinSubtypeId2 == myStringHash || !skinSubtypeId.HasValue)
				{
					Vector3 colorMaskHSV = block.ColorMaskHSV;
					Vector3? vector = newHSV;
					if (colorMaskHSV == vector || !newHSV.HasValue)
					{
						return false;
					}
				}
				if (newHSV.HasValue)
				{
					if (m_colorStatistics.TryGetValue(block.ColorMaskHSV, out var value))
					{
						m_colorStatistics[block.ColorMaskHSV] = value - 1;
						if (m_colorStatistics[block.ColorMaskHSV] <= 0)
						{
							m_colorStatistics.Remove(block.ColorMaskHSV);
						}
					}
					block.ColorMaskHSV = newHSV.Value;
				}
				if (skinSubtypeId.HasValue)
				{
					block.SkinSubtypeId = skinSubtypeId.Value;
				}
				block.UpdateVisual(updatePhysics: false);
				if (newHSV.HasValue)
				{
					if (!m_colorStatistics.ContainsKey(block.ColorMaskHSV))
					{
						m_colorStatistics.Add(block.ColorMaskHSV, 0);
					}
					m_colorStatistics[block.ColorMaskHSV]++;
				}
				block.RecolorTextureUpdateFlags |= MySlimBlock.MyRecolorTextureUpdateFlags.Default;
				block.RecolorTextureUpdateFlags |= MySlimBlock.MyRecolorTextureUpdateFlags.Script;
				block.RecolorTextureUpdateFlags |= MySlimBlock.MyRecolorTextureUpdateFlags.TextAndImage;
				return true;
			}
			finally
			{
			}
		}

		private void UpdatePartInstanceData(MyCubePart part, Vector3I cubePos)
		{
<<<<<<< HEAD
			if (!m_cubes.TryGetValue(cubePos, out var value))
=======
			MyCube myCube = default(MyCube);
			if (!m_cubes.TryGetValue(cubePos, ref myCube))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			MySlimBlock cubeBlock = myCube.CubeBlock;
			if (cubeBlock != null)
			{
				part.InstanceData.SetColorMaskHSV(new Vector4(cubeBlock.ColorMaskHSV, cubeBlock.Dithering));
				part.SkinSubtypeId = myCube.CubeBlock.SkinSubtypeId;
			}
			if (part.Model.BoneMapping == null)
			{
				return;
			}
			Matrix orientation = part.InstanceData.LocalMatrix.GetOrientation();
			bool enableSkinning = false;
			part.InstanceData.BoneRange = GridSize;
			for (int i = 0; i < Math.Min(part.Model.BoneMapping.Length, 9); i++)
			{
				Vector3I bonePos = Vector3I.Round(Vector3.Transform((part.Model.BoneMapping[i] * 1f - Vector3.One) * 1f, orientation) + Vector3.One);
				Vector3UByte vector3UByte = Vector3UByte.Normalize(Skeleton.GetBone(cubePos, bonePos), GridSize);
				if (!Vector3UByte.IsMiddle(vector3UByte))
				{
					enableSkinning = true;
				}
				part.InstanceData[i] = vector3UByte;
			}
			part.InstanceData.EnableSkinning = enableSkinning;
		}

		private void UpdateParts(Vector3I pos)
		{
			MyCube myCube = default(MyCube);
			bool flag = m_cubes.TryGetValue(pos, ref myCube);
			if (flag && !myCube.CubeBlock.ShowParts)
			{
				RemoveBlockEdges(myCube.CubeBlock);
			}
			if (flag && myCube.CubeBlock.ShowParts)
			{
<<<<<<< HEAD
				MyTileDefinition[] cubeTiles = MyCubeGridDefinitions.GetCubeTiles(value.CubeBlock.BlockDefinition);
				value.CubeBlock.Orientation.GetMatrix(out var result);
=======
				MyTileDefinition[] cubeTiles = MyCubeGridDefinitions.GetCubeTiles(myCube.CubeBlock.BlockDefinition);
				myCube.CubeBlock.Orientation.GetMatrix(out var result);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (Skeleton.IsDeformed(pos, 0.004f * GridSize, this, checkBlockDefinition: false))
				{
					RemoveBlockEdges(myCube.CubeBlock);
				}
				MyCube myCube2 = default(MyCube);
				for (int i = 0; i < myCube.Parts.Length; i++)
				{
					UpdatePartInstanceData(myCube.Parts[i], pos);
					Render.RenderData.AddCubePart(myCube.Parts[i]);
					MyTileDefinition myTileDefinition = cubeTiles[i];
					if (myTileDefinition.IsEmpty)
					{
						continue;
					}
					Vector3 vec = Vector3.TransformNormal(myTileDefinition.Normal, result);
					Vector3 vector = Vector3.TransformNormal(myTileDefinition.Up, result);
					if (!Base6Directions.IsBaseDirection(ref vec))
					{
						continue;
					}
<<<<<<< HEAD
					Vector3I key = pos + Vector3I.Round(vec);
					if (!m_cubes.TryGetValue(key, out var value2) || !value2.CubeBlock.ShowParts)
					{
						continue;
					}
					value2.CubeBlock.Orientation.GetMatrix(out var result2);
					MyTileDefinition[] cubeTiles2 = MyCubeGridDefinitions.GetCubeTiles(value2.CubeBlock.BlockDefinition);
					for (int j = 0; j < value2.Parts.Length; j++)
=======
					Vector3I vector3I = pos + Vector3I.Round(vec);
					if (!m_cubes.TryGetValue(vector3I, ref myCube2) || !myCube2.CubeBlock.ShowParts)
					{
						continue;
					}
					myCube2.CubeBlock.Orientation.GetMatrix(out var result2);
					MyTileDefinition[] cubeTiles2 = MyCubeGridDefinitions.GetCubeTiles(myCube2.CubeBlock.BlockDefinition);
					for (int j = 0; j < myCube2.Parts.Length; j++)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MyTileDefinition myTileDefinition2 = cubeTiles2[j];
						if (myTileDefinition2.IsEmpty)
						{
							continue;
						}
						Vector3 vector2 = Vector3.TransformNormal(myTileDefinition2.Normal, result2);
						if (!((vec + vector2).LengthSquared() < 0.001f))
						{
							continue;
						}
<<<<<<< HEAD
						if (value2.CubeBlock.Dithering != value.CubeBlock.Dithering)
						{
							Render.RenderData.AddCubePart(value2.Parts[j]);
=======
						if (myCube2.CubeBlock.Dithering != myCube.CubeBlock.Dithering)
						{
							Render.RenderData.AddCubePart(myCube2.Parts[j]);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							continue;
						}
						bool flag2 = false;
						if (myTileDefinition2.FullQuad && !myTileDefinition.IsRounded)
						{
							Render.RenderData.RemoveCubePart(myCube.Parts[i]);
							flag2 = true;
						}
						if (myTileDefinition.FullQuad && !myTileDefinition2.IsRounded)
						{
<<<<<<< HEAD
							Render.RenderData.RemoveCubePart(value2.Parts[j]);
=======
							Render.RenderData.RemoveCubePart(myCube2.Parts[j]);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							flag2 = true;
						}
						if (!flag2 && (myTileDefinition2.Up * myTileDefinition.Up).LengthSquared() > 0.001f && (Vector3.TransformNormal(myTileDefinition2.Up, result2) - vector).LengthSquared() < 0.001f)
						{
							if (!myTileDefinition.IsRounded && myTileDefinition2.IsRounded)
							{
								Render.RenderData.RemoveCubePart(myCube.Parts[i]);
							}
							if (myTileDefinition.IsRounded && !myTileDefinition2.IsRounded)
							{
<<<<<<< HEAD
								Render.RenderData.RemoveCubePart(value2.Parts[j]);
=======
								Render.RenderData.RemoveCubePart(myCube2.Parts[j]);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
						}
					}
				}
				return;
			}
			if (flag)
			{
				MyCubePart[] parts = myCube.Parts;
				foreach (MyCubePart part in parts)
				{
					Render.RenderData.RemoveCubePart(part);
				}
			}
			Vector3[] directions = Base6Directions.Directions;
<<<<<<< HEAD
			foreach (Vector3 vector3 in directions)
			{
				Vector3I key2 = pos + Vector3I.Round(vector3);
				if (!m_cubes.TryGetValue(key2, out var value3) || !value3.CubeBlock.ShowParts)
				{
					continue;
				}
				value3.CubeBlock.Orientation.GetMatrix(out var result3);
				MyTileDefinition[] cubeTiles3 = MyCubeGridDefinitions.GetCubeTiles(value3.CubeBlock.BlockDefinition);
				for (int l = 0; l < value3.Parts.Length; l++)
=======
			MyCube myCube3 = default(MyCube);
			foreach (Vector3 vector3 in directions)
			{
				Vector3I vector3I2 = pos + Vector3I.Round(vector3);
				if (!m_cubes.TryGetValue(vector3I2, ref myCube3) || !myCube3.CubeBlock.ShowParts)
				{
					continue;
				}
				myCube3.CubeBlock.Orientation.GetMatrix(out var result3);
				MyTileDefinition[] cubeTiles3 = MyCubeGridDefinitions.GetCubeTiles(myCube3.CubeBlock.BlockDefinition);
				for (int l = 0; l < myCube3.Parts.Length; l++)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					Vector3 vector4 = Vector3.Normalize(Vector3.TransformNormal(cubeTiles3[l].Normal, result3));
					if ((vector3 + vector4).LengthSquared() < 0.001f)
					{
<<<<<<< HEAD
						Render.RenderData.AddCubePart(value3.Parts[l]);
=======
						Render.RenderData.AddCubePart(myCube3.Parts[l]);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
		}

		private void RemoveRedundantParts()
		{
			foreach (KeyValuePair<Vector3I, MyCube> cube in m_cubes)
			{
				UpdateParts(cube.Key);
			}
		}

		private void BoundsInclude(MySlimBlock block)
		{
			if (block != null)
			{
				m_min = Vector3I.Min(m_min, block.Min);
				m_max = Vector3I.Max(m_max, block.Max);
			}
		}

		private void BoundsIncludeUpdateAABB(MySlimBlock block)
		{
			BoundsInclude(block);
			UpdateGridAABB();
		}

		private void RecalcBounds()
		{
			m_min = Vector3I.MaxValue;
			m_max = Vector3I.MinValue;
			foreach (KeyValuePair<Vector3I, MyCube> cube in m_cubes)
			{
				m_min = Vector3I.Min(m_min, cube.Key);
				m_max = Vector3I.Max(m_max, cube.Key);
			}
			if (m_cubes.get_Count() == 0)
			{
				m_min = -Vector3I.One;
				m_max = Vector3I.One;
			}
			UpdateGridAABB();
		}

		private void UpdateGridAABB()
		{
			base.PositionComp.LocalAABB = new BoundingBox(m_min * GridSize - GridSizeHalfVector, m_max * GridSize + GridSizeHalfVector);
		}

		private void ResetSkeleton()
		{
			Skeleton = new MyGridSkeleton();
		}

		/// <summary>
		/// For moving bones in corner, offset must contain two one's (positive or negative) and one zero
		/// </summary>
		private bool MoveCornerBones(Vector3I cubePos, Vector3I offset, ref Vector3I minCube, ref Vector3I maxCube)
		{
			Vector3I vector3I = Vector3I.Abs(offset);
			Vector3I vector3I2 = Vector3I.Shift(vector3I);
			Vector3I vector3I3 = offset * vector3I2;
			Vector3I vector3I4 = offset * Vector3I.Shift(vector3I2);
			Vector3 clamp = GridSizeQuarterVector;
			bool num = m_cubes.ContainsKey(cubePos + offset) & m_cubes.ContainsKey(cubePos + vector3I3) & m_cubes.ContainsKey(cubePos + vector3I4);
			if (num)
			{
				Vector3I vector3I5 = Vector3I.One - vector3I;
				Vector3I boneOffset = Vector3I.One + offset;
				Vector3I boneOffset2 = boneOffset + vector3I5;
				Vector3I boneOffset3 = boneOffset - vector3I5;
				Vector3 moveDirection = -offset * 0.25f;
				if (m_precalculatedCornerBonesDisplacementDistance <= 0f)
				{
					m_precalculatedCornerBonesDisplacementDistance = moveDirection.Length();
				}
				float precalculatedCornerBonesDisplacementDistance = m_precalculatedCornerBonesDisplacementDistance;
				precalculatedCornerBonesDisplacementDistance *= GridSize;
				moveDirection *= GridSize;
				MoveBone(ref cubePos, ref boneOffset, ref moveDirection, ref precalculatedCornerBonesDisplacementDistance, ref clamp);
				MoveBone(ref cubePos, ref boneOffset2, ref moveDirection, ref precalculatedCornerBonesDisplacementDistance, ref clamp);
				MoveBone(ref cubePos, ref boneOffset3, ref moveDirection, ref precalculatedCornerBonesDisplacementDistance, ref clamp);
				minCube = Vector3I.Min(Vector3I.Min(cubePos, minCube), cubePos + offset - vector3I5);
				maxCube = Vector3I.Max(Vector3I.Max(cubePos, maxCube), cubePos + offset + vector3I5);
			}
			return num;
		}

		private void GetExistingCubes(Vector3I cubePos, IEnumerable<Vector3I> offsets, Dictionary<Vector3I, MySlimBlock> resultSet)
		{
			resultSet.Clear();
			MyCube myCube = default(MyCube);
			foreach (Vector3I offset in offsets)
			{
<<<<<<< HEAD
				Vector3I key = cubePos + offset;
				if (m_cubes.TryGetValue(key, out var value) && !value.CubeBlock.IsDestroyed && value.CubeBlock.UsesDeformation)
=======
				Vector3I vector3I = cubePos + offset;
				if (m_cubes.TryGetValue(vector3I, ref myCube) && !myCube.CubeBlock.IsDestroyed && myCube.CubeBlock.UsesDeformation)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					resultSet[offset] = myCube.CubeBlock;
				}
			}
		}

		public void GetExistingCubes(Vector3I boneMin, Vector3I boneMax, Dictionary<Vector3I, MySlimBlock> resultSet, MyDamageInformation? damageInfo = null)
		{
			resultSet.Clear();
			Vector3I value = Vector3I.Floor((boneMin - Vector3I.One) / 2f);
			Vector3I value2 = Vector3I.Ceiling((boneMax - Vector3I.One) / 2f);
			MyDamageInformation info = (damageInfo.HasValue ? damageInfo.Value : default(MyDamageInformation));
			Vector3I.Max(ref value, ref m_min, out value);
			Vector3I.Min(ref value2, ref m_max, out value2);
			Vector3I vector3I = default(Vector3I);
			vector3I.X = value.X;
			MyCube myCube = default(MyCube);
			while (vector3I.X <= value2.X)
			{
				vector3I.Y = value.Y;
				while (vector3I.Y <= value2.Y)
				{
					vector3I.Z = value.Z;
					for (; vector3I.Z <= value2.Z; vector3I.Z++)
					{
<<<<<<< HEAD
						if (!m_cubes.TryGetValue(key, out var value3) || !value3.CubeBlock.UsesDeformation)
=======
						if (!m_cubes.TryGetValue(vector3I, ref myCube) || !myCube.CubeBlock.UsesDeformation)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							continue;
						}
						if (myCube.CubeBlock.UseDamageSystem && damageInfo.HasValue)
						{
							info.Amount = 1f;
							MyDamageSystem.Static.RaiseBeforeDamageApplied(myCube.CubeBlock, ref info);
							if (info.Amount == 0f)
							{
								continue;
							}
						}
						resultSet[vector3I] = myCube.CubeBlock;
					}
					vector3I.Y++;
				}
				vector3I.X++;
			}
		}

		public MySlimBlock GetExistingCubeForBoneDeformations(ref Vector3I cube, ref MyDamageInformation damageInfo)
		{
<<<<<<< HEAD
			if (m_cubes.TryGetValue(cube, out var value))
=======
			MyCube myCube = default(MyCube);
			if (m_cubes.TryGetValue(cube, ref myCube))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MySlimBlock cubeBlock = myCube.CubeBlock;
				if (cubeBlock.UsesDeformation)
				{
					if (cubeBlock.UseDamageSystem)
					{
						damageInfo.Amount = 1f;
						MyDamageSystem.Static.RaiseBeforeDamageApplied(cubeBlock, ref damageInfo);
						if (damageInfo.Amount == 0f)
						{
							return null;
						}
					}
					return cubeBlock;
				}
			}
			return null;
		}

		private void MoveBone(ref Vector3I cubePos, ref Vector3I boneOffset, ref Vector3 moveDirection, ref float displacementLength, ref Vector3 clamp)
		{
			m_totalBoneDisplacement += displacementLength;
			Vector3I pos = cubePos * 2 + boneOffset;
			Vector3 value = Vector3.Clamp(Skeleton[pos] + moveDirection, -clamp, clamp);
			Skeleton[pos] = value;
		}

		private void RemoveBlockParts(MySlimBlock block)
		{
			Vector3I vector3I = default(Vector3I);
			vector3I.X = block.Min.X;
			MyCube myCube = default(MyCube);
			while (vector3I.X <= block.Max.X)
			{
				vector3I.Y = block.Min.Y;
				while (vector3I.Y <= block.Max.Y)
				{
					vector3I.Z = block.Min.Z;
					while (vector3I.Z <= block.Max.Z)
					{
<<<<<<< HEAD
						if (m_cubes.TryRemove(key, out var value))
=======
						if (m_cubes.TryRemove(vector3I, ref myCube))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							m_dirtyRegion.PartsToRemove.Enqueue(myCube);
						}
						vector3I.Z++;
					}
					vector3I.Y++;
				}
				vector3I.X++;
			}
			ScheduleDirtyRegion();
			MarkForDraw();
		}

		/// <summary>
		/// Tries to merge this grid with any other grid
		/// Merges grids only when both are static
		/// Returns the merged grid (which does not necessarily have to be this grid)
		/// </summary>
		public MyCubeGrid DetectMerge(MySlimBlock block, MyCubeGrid ignore = null, List<MyEntity> nearEntities = null, bool newGrid = false)
		{
			if (!IsStatic)
			{
				return null;
			}
			if (!Sync.IsServer)
			{
				return null;
			}
			if (block == null)
			{
				return null;
			}
			if (block.CubeGrid != null && !MySessionComponentSafeZones.IsActionAllowed(block.WorldAABB, MySafeZoneAction.Building, block.CubeGrid.EntityId, Sync.MyId))
			{
				return null;
			}
			MyCubeGrid myCubeGrid = null;
			BoundingBoxD boundingBoxD = new BoundingBox(block.Min * GridSize - GridSizeHalf, block.Max * GridSize + GridSizeHalf);
			boundingBoxD.Inflate(GridSizeHalf);
			boundingBoxD = boundingBoxD.TransformFast(base.WorldMatrix);
			bool flag = false;
			if (nearEntities == null)
			{
				flag = true;
				nearEntities = MyEntities.GetEntitiesInAABB(ref boundingBoxD);
			}
			for (int i = 0; i < nearEntities.Count; i++)
			{
				MyCubeGrid myCubeGrid2 = nearEntities[i] as MyCubeGrid;
				MyCubeGrid myCubeGrid3 = myCubeGrid ?? this;
				if (myCubeGrid2 == null || myCubeGrid2 == this || myCubeGrid2 == ignore || myCubeGrid2.Physics == null || !myCubeGrid2.Physics.Enabled || !myCubeGrid2.IsStatic || myCubeGrid2.GridSizeEnum != myCubeGrid3.GridSizeEnum || !myCubeGrid3.IsMergePossible_Static(block, myCubeGrid2, out var _))
				{
					continue;
				}
				MyCubeGrid myCubeGrid4 = myCubeGrid3;
				MyCubeGrid myCubeGrid5 = myCubeGrid2;
				if (myCubeGrid2.BlocksCount > myCubeGrid3.BlocksCount || newGrid)
				{
					myCubeGrid4 = myCubeGrid2;
					myCubeGrid5 = myCubeGrid3;
				}
				Vector3I vector3I = Vector3I.Round(Vector3D.Transform(myCubeGrid5.PositionComp.GetPosition(), myCubeGrid4.PositionComp.WorldMatrixNormalizedInv) * GridSizeR);
				if (myCubeGrid4.CanMoveBlocksFrom(myCubeGrid5, vector3I))
				{
					if (newGrid)
					{
						MyMultiplayer.ReplicateImmediatelly(MyExternalReplicable.FindByObject(this), MyExternalReplicable.FindByObject(myCubeGrid4));
					}
					MyCubeGrid myCubeGrid6 = myCubeGrid4.MergeGrid_Static(myCubeGrid5, vector3I, block);
					if (myCubeGrid6 != null)
					{
						myCubeGrid = myCubeGrid6;
					}
				}
			}
			if (flag)
			{
				nearEntities.Clear();
			}
			return myCubeGrid;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="block"></param>
		/// <param name="gridToMerge"></param>        
		/// <param name="gridOffset">Offset of second grid</param>
		/// <returns></returns>
		private bool IsMergePossible_Static(MySlimBlock block, MyCubeGrid gridToMerge, out Vector3I gridOffset)
		{
			Vector3D position = base.PositionComp.GetPosition();
			position = Vector3D.Transform(position, gridToMerge.PositionComp.WorldMatrixNormalizedInv);
			gridOffset = -Vector3I.Round(position * GridSizeR);
			if (!IsOrientationsAligned(gridToMerge.WorldMatrix, base.WorldMatrix))
			{
				return false;
			}
			MatrixI matrix = gridToMerge.CalculateMergeTransform(this, -gridOffset);
			Vector3I.Transform(ref block.Position, ref matrix, out var result);
			MatrixI.Transform(ref block.Orientation, ref matrix).GetQuaternion(out var result2);
			MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
			return CheckConnectivity(gridToMerge, block.BlockDefinition, buildProgressModelMountPoints, ref result2, ref result);
		}

		public MatrixI CalculateMergeTransform(MyCubeGrid gridToMerge, Vector3I gridOffset)
		{
			Vector3 vec = Vector3D.TransformNormal(gridToMerge.WorldMatrix.Forward, base.PositionComp.WorldMatrixNormalizedInv);
			Vector3 vec2 = Vector3D.TransformNormal(gridToMerge.WorldMatrix.Up, base.PositionComp.WorldMatrixNormalizedInv);
			Base6Directions.Direction closestDirection = Base6Directions.GetClosestDirection(vec);
			Base6Directions.Direction direction = Base6Directions.GetClosestDirection(vec2);
			if (direction == closestDirection)
			{
				direction = Base6Directions.GetPerpendicular(closestDirection);
			}
			return new MatrixI(ref gridOffset, closestDirection, direction);
		}

		public bool CanMergeCubes(MyCubeGrid gridToMerge, Vector3I gridOffset)
		{
			MatrixI transform = CalculateMergeTransform(gridToMerge, gridOffset);
			foreach (KeyValuePair<Vector3I, MyCube> cube in gridToMerge.m_cubes)
			{
				Vector3I vector3I = Vector3I.Transform(cube.Key, transform);
				if (!m_cubes.ContainsKey(vector3I))
<<<<<<< HEAD
				{
					continue;
				}
				MySlimBlock cubeBlock = GetCubeBlock(vector3I);
				if (cubeBlock != null && cubeBlock.FatBlock is MyCompoundCubeBlock)
				{
=======
				{
					continue;
				}
				MySlimBlock cubeBlock = GetCubeBlock(vector3I);
				if (cubeBlock != null && cubeBlock.FatBlock is MyCompoundCubeBlock)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
					MySlimBlock cubeBlock2 = gridToMerge.GetCubeBlock(cube.Key);
					if (cubeBlock2.FatBlock is MyCompoundCubeBlock)
					{
						MyCompoundCubeBlock obj = cubeBlock2.FatBlock as MyCompoundCubeBlock;
						bool flag = true;
						foreach (MySlimBlock block in obj.GetBlocks())
						{
							MyBlockOrientation value = MatrixI.Transform(ref block.Orientation, ref transform);
							if (!myCompoundCubeBlock.CanAddBlock(block.BlockDefinition, value))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							continue;
						}
					}
					else
					{
						MyBlockOrientation value2 = MatrixI.Transform(ref cubeBlock2.Orientation, ref transform);
						if (myCompoundCubeBlock.CanAddBlock(cubeBlock2.BlockDefinition, value2))
						{
							continue;
						}
					}
				}
<<<<<<< HEAD
				if (cubeBlock?.FatBlock != null && cube.Value != null && cube.Value.CubeBlock != null && cube.Value.CubeBlock.FatBlock != null)
=======
				if (cubeBlock.FatBlock != null && cube.Value != null && cube.Value.CubeBlock != null && cube.Value.CubeBlock.FatBlock != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					IMyPistonTop myPistonTop;
					if (cube.Value.CubeBlock.FatBlock is IMyPistonTop)
					{
						myPistonTop = (IMyPistonTop)cube.Value.CubeBlock.FatBlock;
					}
					else
					{
						if (!(cubeBlock.FatBlock is IMyPistonTop))
						{
							return false;
						}
						myPistonTop = (IMyPistonTop)cubeBlock.FatBlock;
					}
					IMyPistonBase myPistonBase;
					if (cube.Value.CubeBlock.FatBlock is IMyPistonBase)
					{
						myPistonBase = (IMyPistonBase)cube.Value.CubeBlock.FatBlock;
					}
					else
					{
						if (!(cubeBlock.FatBlock is IMyPistonBase))
						{
							return false;
						}
						myPistonBase = (IMyPistonBase)cubeBlock.FatBlock;
					}
<<<<<<< HEAD
					if ((myPistonBase.Top != null && myPistonBase.Top.EntityId == myPistonTop.EntityId) || (myPistonTop.Base != null && myPistonTop.Base.EntityId == myPistonBase.EntityId))
=======
					if (myPistonBase.Top.EntityId == myPistonTop.EntityId && myPistonTop.Base.EntityId == myPistonBase.EntityId)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						continue;
					}
				}
				return false;
			}
			return true;
		}

		public void ChangeGridOwnership(long playerId, MyOwnershipShareModeEnum shareMode)
		{
			if (Sync.IsServer)
			{
				ChangeGridOwner(playerId, shareMode);
			}
		}

		private static void MoveBlocks(MyCubeGrid from, MyCubeGrid to, List<MySlimBlock> cubeBlocks, int offset, int count)
		{
			to.IsBlockTrasferInProgress = true;
			from.IsBlockTrasferInProgress = true;
			try
			{
				m_tmpBlockGroups.Clear();
				foreach (MyBlockGroup blockGroup in from.BlockGroups)
				{
					m_tmpBlockGroups.Add(blockGroup.GetObjectBuilder());
				}
				for (int i = offset; i < offset + count; i++)
				{
					MySlimBlock mySlimBlock = cubeBlocks[i];
					if (mySlimBlock != null)
					{
						if (mySlimBlock.FatBlock != null)
						{
							from.Hierarchy.RemoveChild(mySlimBlock.FatBlock);
						}
						from.RemoveBlockInternal(mySlimBlock, close: false, markDirtyDisconnects: false);
					}
				}
				if (from.Physics != null)
				{
					for (int j = offset; j < offset + count; j++)
					{
						MySlimBlock mySlimBlock2 = cubeBlocks[j];
						if (mySlimBlock2 != null)
						{
							from.Physics.AddDirtyBlock(mySlimBlock2);
						}
					}
				}
				for (int k = offset; k < offset + count; k++)
				{
					MySlimBlock mySlimBlock3 = cubeBlocks[k];
					if (mySlimBlock3 != null)
					{
						to.AddBlockInternal(mySlimBlock3);
						from.Skeleton.CopyTo(to.Skeleton, mySlimBlock3.Position);
					}
				}
				foreach (MyObjectBuilder_BlockGroup tmpBlockGroup in m_tmpBlockGroups)
				{
					MyBlockGroup myBlockGroup = new MyBlockGroup();
					myBlockGroup.Init(to, tmpBlockGroup);
					if (myBlockGroup.Blocks.get_Count() > 0)
					{
						to.AddGroup(myBlockGroup);
					}
				}
				m_tmpBlockGroups.Clear();
				from.RemoveEmptyBlockGroups();
			}
			finally
			{
				to.IsBlockTrasferInProgress = false;
				from.IsBlockTrasferInProgress = false;
			}
		}

		private static void MoveBlocksByObjectBuilders(MyCubeGrid from, MyCubeGrid to, List<MySlimBlock> cubeBlocks, int offset, int count)
		{
			try
			{
				List<MyObjectBuilder_CubeBlock> list = new List<MyObjectBuilder_CubeBlock>();
				for (int i = offset; i < offset + count; i++)
				{
					MySlimBlock mySlimBlock = cubeBlocks[i];
					list.Add(mySlimBlock.GetObjectBuilder(copy: true));
				}
				MyEntityIdRemapHelper remapHelper = new MyEntityIdRemapHelper();
				foreach (MyObjectBuilder_CubeBlock item in list)
				{
					item.Remap(remapHelper);
				}
				for (int j = offset; j < offset + count; j++)
				{
					MySlimBlock block = cubeBlocks[j];
					from.RemoveBlockInternal(block, close: true, markDirtyDisconnects: false);
				}
				foreach (MyObjectBuilder_CubeBlock item2 in list)
				{
					to.AddBlock(item2, testMerge: false);
				}
			}
			finally
			{
			}
		}

		private void RemoveEmptyBlockGroups()
		{
			for (int i = 0; i < BlockGroups.Count; i++)
			{
				MyBlockGroup myBlockGroup = BlockGroups[i];
				if (myBlockGroup.Blocks.get_Count() == 0)
				{
					RemoveGroup(myBlockGroup);
					i--;
				}
			}
		}

		/// <summary>
		/// Adds the block to the grid. The block's position and orientation in the grid should be set elsewhere
		/// </summary>
		private void AddBlockInternal(MySlimBlock block)
		{
			if (block.FatBlock != null)
			{
				block.FatBlock.UpdateWorldMatrix();
				if (block.FatBlock.InventoryCount > 0)
				{
					RegisterInventory(block.FatBlock);
				}
			}
			block.CubeGrid = this;
			if (block.BlockDefinition.IsStandAlone)
			{
				m_standAloneBlockCount++;
			}
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && block.FatBlock is MyCompoundCubeBlock)
			{
				MyCompoundCubeBlock myCompoundCubeBlock = block.FatBlock as MyCompoundCubeBlock;
				MySlimBlock cubeBlock = GetCubeBlock(block.Min);
				MyCompoundCubeBlock myCompoundCubeBlock2 = ((cubeBlock != null) ? (cubeBlock.FatBlock as MyCompoundCubeBlock) : null);
				if (myCompoundCubeBlock2 != null)
				{
					bool flag = false;
					myCompoundCubeBlock.UpdateWorldMatrix();
					m_tmpSlimBlocks.Clear();
					foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
					{
						if (myCompoundCubeBlock2.Add(block2, out var _))
						{
							BoundsInclude(block2);
							m_dirtyRegion.AddCube(block2.Min);
							Physics.AddDirtyBlock(cubeBlock);
							m_tmpSlimBlocks.Add(block2);
							flag = true;
						}
					}
					ScheduleDirtyRegion();
					MarkForDraw();
					foreach (MySlimBlock tmpSlimBlock in m_tmpSlimBlocks)
					{
						myCompoundCubeBlock.Remove(tmpSlimBlock, merged: true);
					}
					if (flag)
					{
						if (MyCubeGridSmallToLargeConnection.Static != null && m_enableSmallToLargeConnections)
						{
							MyCubeGridSmallToLargeConnection.Static.AddBlockSmallToLargeConnection(block);
						}
						foreach (MySlimBlock tmpSlimBlock2 in m_tmpSlimBlocks)
						{
							NotifyBlockAdded(tmpSlimBlock2);
						}
					}
					m_tmpSlimBlocks.Clear();
					return;
				}
			}
			m_cubeBlocks.Add(block);
			if (block.FatBlock != null)
			{
				m_fatBlocks.Add(block.FatBlock);
			}
			if (!m_colorStatistics.ContainsKey(block.ColorMaskHSV))
			{
				m_colorStatistics.Add(block.ColorMaskHSV, 0);
			}
			m_colorStatistics[block.ColorMaskHSV]++;
			block.AddNeighbours();
			BoundsInclude(block);
			if (block.FatBlock != null)
			{
				base.Hierarchy.AddChild(block.FatBlock);
				GridSystems.RegisterInSystems(block.FatBlock);
				if (block.FatBlock.Render.NeedsDrawFromParent)
				{
					m_blocksForDraw.Add(block.FatBlock);
					block.FatBlock.Render.SetVisibilityUpdates(state: true);
				}
				MyObjectBuilderType typeId = block.BlockDefinition.Id.TypeId;
				if (typeId != typeof(MyObjectBuilder_CubeBlock))
				{
					if (!BlocksCounters.ContainsKey(typeId))
					{
						BlocksCounters.Add(typeId, 0);
					}
					BlocksCounters[typeId]++;
				}
			}
			MyBlockOrientation orientation = block.Orientation;
			orientation.GetMatrix(out var result);
			bool flag2 = true;
			Vector3I pos = default(Vector3I);
			pos.X = block.Min.X;
			while (pos.X <= block.Max.X)
			{
				pos.Y = block.Min.Y;
				while (pos.Y <= block.Max.Y)
				{
					pos.Z = block.Min.Z;
					while (pos.Z <= block.Max.Z)
					{
						flag2 &= AddCube(block, ref pos, result, block.BlockDefinition);
						pos.Z++;
					}
					pos.Y++;
				}
				pos.X++;
			}
			if (Physics != null)
			{
				Physics.AddBlock(block);
			}
			if (block.FatBlock != null)
			{
				ChangeOwner(block.FatBlock, 0L, block.FatBlock.OwnerId);
			}
			if (MyCubeGridSmallToLargeConnection.Static != null && m_enableSmallToLargeConnections && flag2)
			{
				MyCubeGridSmallToLargeConnection.Static.AddBlockSmallToLargeConnection(block);
			}
			if (MyFakes.ENABLE_MULTIBLOCK_PART_IDS)
			{
				AddMultiBlockInfo(block);
			}
			NotifyBlockAdded(block);
			block.AddAuthorship();
			m_PCU += (block.ComponentStack.IsFunctional ? block.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST);
		}

		private bool IsDamaged(Vector3I bonePos, float epsilon = 0.04f)
		{
			if (Skeleton.TryGetBone(ref bonePos, out var bone))
			{
				return !MyUtils.IsZero(ref bone, epsilon * GridSize);
			}
			return false;
		}

		private void RemoveAuthorshipAll()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = GetBlocks().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					current.RemoveAuthorship();
					m_PCU -= (current.ComponentStack.IsFunctional ? current.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void DismountAllCockpits()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = GetBlocks().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyCockpit myCockpit = enumerator.get_Current().FatBlock as MyCockpit;
					if (myCockpit != null && myCockpit.Pilot != null)
					{
						myCockpit.Use();
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private void AddBlockEdges(MySlimBlock block)
		{
			MyCubeBlockDefinition blockDefinition = block.BlockDefinition;
			if (blockDefinition.BlockTopology != 0 || blockDefinition.CubeDefinition == null || !blockDefinition.CubeDefinition.ShowEdges)
			{
				return;
			}
			Vector3 translation = block.Position * GridSize;
			block.Orientation.GetMatrix(out var result);
			result.Translation = translation;
			MyCubeGridDefinitions.TableEntry topologyInfo = MyCubeGridDefinitions.GetTopologyInfo(blockDefinition.CubeDefinition.CubeTopology);
			Vector3I vector3I = block.Position * 2 + Vector3I.One;
			MyEdgeDefinition[] edges = topologyInfo.Edges;
			for (int i = 0; i < edges.Length; i++)
			{
				MyEdgeDefinition myEdgeDefinition = edges[i];
				Vector3 vector = Vector3.TransformNormal(myEdgeDefinition.Point0, block.Orientation);
				Vector3 vector2 = Vector3.TransformNormal(myEdgeDefinition.Point1, block.Orientation);
				Vector3 value = (vector + vector2) * 0.5f;
				if (!IsDamaged(vector3I + Vector3I.Round(vector)) && !IsDamaged(vector3I + Vector3I.Round(value)) && !IsDamaged(vector3I + Vector3I.Round(vector2)))
				{
					vector = Vector3.Transform(myEdgeDefinition.Point0 * GridSizeHalf, ref result);
					vector2 = Vector3.Transform(myEdgeDefinition.Point1 * GridSizeHalf, ref result);
					if (myEdgeDefinition.Side0 < topologyInfo.Tiles.Length && myEdgeDefinition.Side1 < topologyInfo.Tiles.Length && myEdgeDefinition.Side0 >= 0 && myEdgeDefinition.Side1 >= 0)
					{
						Vector3 normal = Vector3.TransformNormal(topologyInfo.Tiles[myEdgeDefinition.Side0].Normal, block.Orientation);
						Vector3 normal2 = Vector3.TransformNormal(topologyInfo.Tiles[myEdgeDefinition.Side1].Normal, block.Orientation);
						Vector3 colorMaskHSV = block.ColorMaskHSV;
						colorMaskHSV.Y = (colorMaskHSV.Y + 1f) * 0.5f;
						colorMaskHSV.Z = (colorMaskHSV.Z + 1f) * 0.5f;
						Render.RenderData.AddEdgeInfo(ref vector, ref vector2, ref normal, ref normal2, colorMaskHSV.HSVtoColor(), block);
					}
				}
			}
		}

		private void RemoveBlockEdges(MySlimBlock block)
		{
			using (Pin())
			{
				if (base.MarkedForClose)
<<<<<<< HEAD
				{
					return;
				}
				MyCubeBlockDefinition blockDefinition = block.BlockDefinition;
				if (blockDefinition.BlockTopology == MyBlockTopology.Cube && blockDefinition.CubeDefinition != null)
				{
=======
				{
					return;
				}
				MyCubeBlockDefinition blockDefinition = block.BlockDefinition;
				if (blockDefinition.BlockTopology == MyBlockTopology.Cube && blockDefinition.CubeDefinition != null)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					Vector3 translation = block.Position * GridSize;
					block.Orientation.GetMatrix(out var result);
					result.Translation = translation;
					MyEdgeDefinition[] edges = MyCubeGridDefinitions.GetTopologyInfo(blockDefinition.CubeDefinition.CubeTopology).Edges;
					for (int i = 0; i < edges.Length; i++)
					{
						MyEdgeDefinition myEdgeDefinition = edges[i];
						Vector3 point = Vector3.Transform(myEdgeDefinition.Point0 * GridSizeHalf, result);
						Vector3 point2 = Vector3.Transform(myEdgeDefinition.Point1 * GridSizeHalf, result);
						Render.RenderData.RemoveEdgeInfo(point, point2, block);
					}
				}
			}
		}

		private void SendBones()
		{
			if (BonesToSend.InputCount > 0)
			{
				if (m_bonesSendCounter++ > 10 && !m_bonesSending)
				{
					m_bonesSendCounter = 0;
					lock (BonesToSend)
					{
						MyVoxelSegmentation bonesToSend = BonesToSend;
						BonesToSend = m_bonesToSendSecond;
						m_bonesToSendSecond = bonesToSend;
					}
					_ = m_bonesToSendSecond.InputCount;
					if (Sync.IsServer)
					{
						m_bonesSending = true;
						m_workData.Priority = WorkPriority.Low;
						Parallel.Start(SendBonesAsync, null, m_workData);
					}
				}
			}
			else
			{
				DeSchedule(UpdateQueue.AfterSimulation, SendBones);
			}
		}

		internal void AddBoneToSend(Vector3I boneIndex)
		{
			lock (BonesToSend)
			{
				BonesToSend.AddInput(boneIndex);
				Schedule(UpdateQueue.AfterSimulation, SendBones, 16);
			}
		}

		private long SendBones(MyVoxelSegmentationType segmentationType, out int bytes, out int segmentsCount, out int emptyBones)
		{
			_ = m_bonesToSendSecond.InputCount;
			long timestamp = Stopwatch.GetTimestamp();
			List<MyVoxelSegmentation.Segment> list = m_bonesToSendSecond.FindSegments(segmentationType);
			if (m_boneByteList == null)
			{
				m_boneByteList = new List<byte>();
			}
			else
			{
				m_boneByteList.Clear();
			}
			emptyBones = 0;
			foreach (MyVoxelSegmentation.Segment item in list)
			{
				emptyBones += ((!Skeleton.SerializePart(item.Min, item.Max, GridSize, m_boneByteList)) ? 1 : 0);
			}
			if (emptyBones != list.Count)
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnBonesReceived, list.Count, m_boneByteList);
			}
			bytes = m_boneByteList.Count;
			segmentsCount = list.Count;
			return Stopwatch.GetTimestamp() - timestamp;
		}

		private void SendBonesAsync(WorkData workData)
		{
			_ = m_bonesToSendSecond.InputCount;
			MyTimeSpan.FromTicks(SendBones(MyVoxelSegmentationType.Simple, out var _, out var _, out var _));
			m_bonesToSendSecond.ClearInput();
			m_bonesSending = false;
		}

		private void RemoveUnsusedBones()
		{
			Skeleton.RemoveUnusedBones(this);
		}

		internal void AddForDamageApplication(MySlimBlock block)
		{
			m_blocksForDamageApplication.Add(block);
			m_blocksForDamageApplicationDirty = true;
<<<<<<< HEAD
			if (m_blocksForDamageApplication.Count == 1)
=======
			if (m_blocksForDamageApplication.get_Count() == 1)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Schedule(UpdateQueue.OnceAfterSimulation, ProcessDamageApplication, 17);
			}
		}

		internal void RemoveFromDamageApplication(MySlimBlock block)
		{
			m_blocksForDamageApplication.Remove(block);
<<<<<<< HEAD
			m_blocksForDamageApplicationDirty = m_blocksForDamageApplication.Count > 0;
=======
			m_blocksForDamageApplicationDirty = m_blocksForDamageApplication.get_Count() > 0;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void ProcessDamageApplication()
		{
			if (!m_blocksForDamageApplicationDirty)
<<<<<<< HEAD
			{
				return;
			}
			m_blocksForDamageApplicationCopy.AddRange(m_blocksForDamageApplication);
			foreach (MySlimBlock item in m_blocksForDamageApplicationCopy)
			{
=======
			{
				return;
			}
			m_blocksForDamageApplicationCopy.AddRange((IEnumerable<MySlimBlock>)m_blocksForDamageApplication);
			foreach (MySlimBlock item in m_blocksForDamageApplicationCopy)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (item.AccumulatedDamage > 0f)
				{
					item.ApplyAccumulatedDamage(addDirtyParts: true, 0L);
				}
			}
			m_blocksForDamageApplication.Clear();
			m_blocksForDamageApplicationCopy.Clear();
			m_blocksForDamageApplicationDirty = false;
		}

		public bool GetLineIntersectionExactGrid(ref LineD line, ref Vector3I position, ref double distanceSquared)
		{
			return GetLineIntersectionExactGrid(ref line, ref position, ref distanceSquared, null);
		}

		public bool GetLineIntersectionExactGrid(ref LineD line, ref Vector3I position, ref double distanceSquared, MyPhysics.HitInfo? hitInfo)
		{
			RayCastCells(line.From, line.To, m_cacheRayCastCells, null, havokWorld: true);
			if (m_cacheRayCastCells.Count == 0)
			{
				return false;
			}
			m_tmpHitList.Clear();
			if (hitInfo.HasValue)
			{
				m_tmpHitList.Add(hitInfo.Value);
			}
			else
			{
				MyPhysics.CastRay(line.From, line.To, m_tmpHitList, 24);
			}
			if (m_tmpHitList.Count == 0)
			{
				return false;
			}
			bool flag = false;
			MyCube myCube = default(MyCube);
			for (int i = 0; i < m_cacheRayCastCells.Count; i++)
			{
				Vector3I vector3I = m_cacheRayCastCells[i];
<<<<<<< HEAD
				m_cubes.TryGetValue(vector3I, out var value);
=======
				m_cubes.TryGetValue(vector3I, ref myCube);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				double num = double.MaxValue;
				if (myCube != null)
				{
					if (myCube.CubeBlock.FatBlock != null && !myCube.CubeBlock.FatBlock.BlockDefinition.UseModelIntersection)
					{
						if (m_tmpHitList.Count > 0)
						{
							int j = 0;
							if (MySession.Static.ControlledEntity != null)
							{
								for (; j < m_tmpHitList.Count - 1 && m_tmpHitList[j].HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity; j++)
								{
								}
							}
							if (j > 1 && m_tmpHitList[j].HkHitInfo.GetHitEntity() != this)
							{
								continue;
							}
							Vector3 gridSizeHalfVector = GridSizeHalfVector;
							Vector3D vector3D = Vector3D.Transform(m_tmpHitList[j].Position, base.PositionComp.WorldMatrixInvScaled);
							Vector3 vector = vector3I * GridSize;
							Vector3D vector3D2 = vector3D - vector;
							double num2 = ((vector3D2.Max() > Math.Abs(vector3D2.Min())) ? vector3D2.Max() : vector3D2.Min());
							vector3D2.X = ((vector3D2.X == num2) ? ((num2 > 0.0) ? 1 : (-1)) : 0);
							vector3D2.Y = ((vector3D2.Y == num2) ? ((num2 > 0.0) ? 1 : (-1)) : 0);
							vector3D2.Z = ((vector3D2.Z == num2) ? ((num2 > 0.0) ? 1 : (-1)) : 0);
							vector3D -= vector3D2 * 0.10000000149011612;
							if (Vector3D.Max(vector3D, vector - gridSizeHalfVector) == vector3D && Vector3D.Min(vector3D, vector + gridSizeHalfVector) == vector3D)
							{
								num = Vector3D.DistanceSquared(line.From, m_tmpHitList[j].Position);
								if (num < distanceSquared)
								{
									position = vector3I;
									distanceSquared = num;
									flag = true;
									continue;
								}
							}
						}
					}
					else
					{
<<<<<<< HEAD
						GetBlockIntersection(value, ref line, IntersectionFlags.ALL_TRIANGLES, out var t, out var _);
=======
						GetBlockIntersection(myCube, ref line, IntersectionFlags.ALL_TRIANGLES, out var t, out var _);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (t.HasValue)
						{
							num = Vector3D.DistanceSquared(line.From, t.Value.IntersectionPointInWorldSpace);
						}
					}
				}
				if (num < distanceSquared)
				{
					distanceSquared = num;
					position = vector3I;
					flag = true;
				}
			}
			if (!flag)
			{
				MyCube myCube2 = default(MyCube);
				for (int k = 0; k < m_cacheRayCastCells.Count; k++)
				{
					Vector3I vector3I2 = m_cacheRayCastCells[k];
<<<<<<< HEAD
					m_cubes.TryGetValue(vector3I2, out var value2);
					double num3 = double.MaxValue;
					if (value2 == null || value2.CubeBlock.FatBlock == null || !value2.CubeBlock.FatBlock.BlockDefinition.UseModelIntersection)
=======
					m_cubes.TryGetValue(vector3I2, ref myCube2);
					double num3 = double.MaxValue;
					if (myCube2 == null || myCube2.CubeBlock.FatBlock == null || !myCube2.CubeBlock.FatBlock.BlockDefinition.UseModelIntersection)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						if (m_tmpHitList.Count > 0)
						{
							int l = 0;
							if (MySession.Static.ControlledEntity != null)
							{
								for (; l < m_tmpHitList.Count - 1 && m_tmpHitList[l].HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity; l++)
								{
								}
							}
							if (l > 1 && m_tmpHitList[l].HkHitInfo.GetHitEntity() != this)
							{
								continue;
							}
							Vector3 gridSizeHalfVector2 = GridSizeHalfVector;
							Vector3D vector3D3 = Vector3D.Transform(m_tmpHitList[l].Position, base.PositionComp.WorldMatrixInvScaled);
							Vector3 vector2 = vector3I2 * GridSize;
							Vector3D vector3D4 = vector3D3 - vector2;
							double num4 = ((vector3D4.Max() > Math.Abs(vector3D4.Min())) ? vector3D4.Max() : vector3D4.Min());
							vector3D4.X = ((vector3D4.X == num4) ? ((num4 > 0.0) ? 1 : (-1)) : 0);
							vector3D4.Y = ((vector3D4.Y == num4) ? ((num4 > 0.0) ? 1 : (-1)) : 0);
							vector3D4.Z = ((vector3D4.Z == num4) ? ((num4 > 0.0) ? 1 : (-1)) : 0);
							vector3D3 -= vector3D4 * 0.059999998658895493;
							if (Vector3D.Max(vector3D3, vector2 - gridSizeHalfVector2) == vector3D3 && Vector3D.Min(vector3D3, vector2 + gridSizeHalfVector2) == vector3D3)
							{
<<<<<<< HEAD
								if (value2 == null)
								{
									FixTargetCube(out var cube, vector3D3 * GridSizeR);
									if (!m_cubes.TryGetValue(cube, out value2))
=======
								if (myCube2 == null)
								{
									FixTargetCube(out var cube, vector3D3 * GridSizeR);
									if (!m_cubes.TryGetValue(cube, ref myCube2))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
									{
										continue;
									}
									vector3I2 = cube;
								}
								num3 = Vector3D.DistanceSquared(line.From, m_tmpHitList[l].Position);
								if (num3 < distanceSquared)
								{
									position = vector3I2;
									distanceSquared = num3;
									flag = true;
									continue;
								}
							}
						}
					}
					else
					{
<<<<<<< HEAD
						GetBlockIntersection(value2, ref line, IntersectionFlags.ALL_TRIANGLES, out var t2, out var _);
=======
						GetBlockIntersection(myCube2, ref line, IntersectionFlags.ALL_TRIANGLES, out var t2, out var _);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (t2.HasValue)
						{
							num3 = Vector3D.DistanceSquared(line.From, t2.Value.IntersectionPointInWorldSpace);
						}
					}
					if (num3 < distanceSquared)
					{
						distanceSquared = num3;
						position = vector3I2;
						flag = true;
					}
				}
			}
			m_tmpHitList.Clear();
			return flag;
		}

		private void GetBlockIntersection(MyCube cube, ref LineD line, IntersectionFlags flags, out MyIntersectionResultLineTriangleEx? t, out int cubePartIndex)
		{
			if (cube.CubeBlock.FatBlock != null)
			{
				if (cube.CubeBlock.FatBlock is MyCompoundCubeBlock)
				{
					MyCompoundCubeBlock obj = cube.CubeBlock.FatBlock as MyCompoundCubeBlock;
					MyIntersectionResultLineTriangleEx? myIntersectionResultLineTriangleEx = null;
					double num = double.MaxValue;
					foreach (MySlimBlock block in obj.GetBlocks())
					{
						block.Orientation.GetMatrix(out var result);
						Vector3.TransformNormal(ref block.BlockDefinition.ModelOffset, ref result, out var result2);
						result.Translation = block.Position * GridSize + result2;
						MatrixD customInvMatrix = MatrixD.Invert(block.FatBlock.WorldMatrix);
						t = block.FatBlock.ModelCollision.GetTrianglePruningStructure().GetIntersectionWithLine(this, ref line, ref customInvMatrix, flags);
						if (!t.HasValue && block.FatBlock.Subparts != null)
						{
							foreach (KeyValuePair<string, MyEntitySubpart> subpart in block.FatBlock.Subparts)
							{
								customInvMatrix = MatrixD.Invert(subpart.Value.WorldMatrix);
								t = subpart.Value.ModelCollision.GetTrianglePruningStructure().GetIntersectionWithLine(this, ref line, ref customInvMatrix, flags);
								if (t.HasValue)
								{
									break;
								}
							}
						}
						if (t.HasValue)
						{
							MyIntersectionResultLineTriangleEx triangle = t.Value;
							double num2 = Vector3D.Distance(Vector3D.Transform(t.Value.IntersectionPointInObjectSpace, block.FatBlock.WorldMatrix), line.From);
							if (num2 < num)
							{
								num = num2;
								MatrixD? cubeWorldMatrix = block.FatBlock.WorldMatrix;
								TransformCubeToGrid(ref triangle, ref result, ref cubeWorldMatrix);
								myIntersectionResultLineTriangleEx = triangle;
							}
						}
					}
					t = myIntersectionResultLineTriangleEx;
				}
				else
				{
					cube.CubeBlock.FatBlock.GetIntersectionWithLine(ref line, out t, IntersectionFlags.ALL_TRIANGLES);
					if (t.HasValue)
					{
						cube.CubeBlock.Orientation.GetMatrix(out var result3);
						MyIntersectionResultLineTriangleEx triangle2 = t.Value;
						MatrixD? cubeWorldMatrix2 = cube.CubeBlock.FatBlock.WorldMatrix;
						TransformCubeToGrid(ref triangle2, ref result3, ref cubeWorldMatrix2);
						t = triangle2;
					}
				}
				cubePartIndex = -1;
				return;
			}
			MyIntersectionResultLineTriangleEx? myIntersectionResultLineTriangleEx2 = null;
			float num3 = float.MaxValue;
			int num4 = -1;
			for (int i = 0; i < cube.Parts.Length; i++)
			{
				MyCubePart myCubePart = cube.Parts[i];
				MatrixD matrix = myCubePart.InstanceData.LocalMatrix * base.WorldMatrix;
				MatrixD customInvMatrix2 = MatrixD.Invert(matrix);
				t = myCubePart.Model.GetTrianglePruningStructure().GetIntersectionWithLine(this, ref line, ref customInvMatrix2, flags);
				if (t.HasValue)
				{
					MyIntersectionResultLineTriangleEx triangle3 = t.Value;
					float num5 = Vector3.Distance(Vector3.Transform(t.Value.IntersectionPointInObjectSpace, matrix), line.From);
					if (num5 < num3)
					{
						num3 = num5;
						Matrix cubeLocalMatrix = myCubePart.InstanceData.LocalMatrix;
						MatrixD? cubeWorldMatrix3 = null;
						TransformCubeToGrid(ref triangle3, ref cubeLocalMatrix, ref cubeWorldMatrix3);
						myIntersectionResultLineTriangleEx2 = triangle3;
						num4 = i;
					}
				}
			}
			t = myIntersectionResultLineTriangleEx2;
			cubePartIndex = num4;
		}

		public static bool GetLineIntersection(ref LineD line, out MyCubeGrid grid, out Vector3I position, out double distanceSquared, Func<MyCubeGrid, bool> condition = null)
		{
			grid = null;
			position = default(Vector3I);
			distanceSquared = 3.4028234663852886E+38;
			MyEntities.OverlapAllLineSegment(ref line, m_lineOverlapList);
			foreach (MyLineSegmentOverlapResult<MyEntity> lineOverlap in m_lineOverlapList)
			{
				MyCubeGrid myCubeGrid = lineOverlap.Element as MyCubeGrid;
				if (myCubeGrid == null || (condition != null && !condition(myCubeGrid)))
<<<<<<< HEAD
				{
					continue;
				}
				Vector3I? vector3I = myCubeGrid.RayCastBlocks(line.From, line.To);
				if (vector3I.HasValue)
				{
=======
				{
					continue;
				}
				Vector3I? vector3I = myCubeGrid.RayCastBlocks(line.From, line.To);
				if (vector3I.HasValue)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					Vector3 closestCorner = myCubeGrid.GetClosestCorner(vector3I.Value, line.From);
					float num = (float)Vector3D.DistanceSquared(line.From, Vector3D.Transform(closestCorner, myCubeGrid.WorldMatrix));
					if ((double)num < distanceSquared)
					{
						distanceSquared = num;
						grid = myCubeGrid;
						position = vector3I.Value;
					}
				}
			}
			m_lineOverlapList.Clear();
			return grid != null;
		}

		public static bool GetLineIntersectionExact(ref LineD line, out MyCubeGrid grid, out Vector3I position, out double distanceSquared)
		{
			grid = null;
			position = default(Vector3I);
			distanceSquared = 3.4028234663852886E+38;
			double num = double.MaxValue;
			MyEntities.OverlapAllLineSegment(ref line, m_lineOverlapList);
			foreach (MyLineSegmentOverlapResult<MyEntity> lineOverlap in m_lineOverlapList)
			{
				MyCubeGrid myCubeGrid = lineOverlap.Element as MyCubeGrid;
				if (myCubeGrid != null && myCubeGrid.GetLineIntersectionExactAll(ref line, out var distance, out var _).HasValue && distance < num)
				{
					grid = myCubeGrid;
					num = distance;
				}
			}
			m_lineOverlapList.Clear();
			return grid != null;
		}

		/// <summary>
		/// Returns closest line (in world space) intersection with all cubes. 
		/// </summary>
		public Vector3D? GetLineIntersectionExactAll(ref LineD line, out double distance, out MySlimBlock intersectedBlock)
		{
			intersectedBlock = null;
			distance = 3.4028234663852886E+38;
			Vector3I? vector3I = null;
			Vector3I position = Vector3I.Zero;
			double distanceSquared = double.MaxValue;
			if (GetLineIntersectionExactGrid(ref line, ref position, ref distanceSquared))
			{
				distanceSquared = (float)Math.Sqrt(distanceSquared);
				vector3I = position;
			}
			if (vector3I.HasValue)
			{
				distance = distanceSquared;
				intersectedBlock = GetCubeBlock(vector3I.Value);
				if (intersectedBlock == null)
				{
					return null;
				}
				return position;
			}
			return null;
		}

		public void GetBlocksInsideSphere(ref BoundingSphereD sphere, HashSet<MySlimBlock> blocks, bool checkTriangles = false)
		{
			GetBlocksInsideSphereInternal(ref sphere, blocks, checkTriangles);
		}

		public void GetBlocksInsideSphereInternal(ref BoundingSphereD sphere, HashSet<MySlimBlock> blocks, bool checkTriangles = false, bool useOptimization = true)
		{
			blocks.Clear();
			if (base.PositionComp == null)
			{
				return;
			}
			BoundingBoxD aabb = BoundingBoxD.CreateFromSphere(sphere);
			MatrixD matrix = base.PositionComp.WorldMatrixNormalizedInv;
			Vector3D.Transform(ref sphere.Center, ref matrix, out var result);
			BoundingSphere localSphere = new BoundingSphere(result, (float)sphere.Radius);
			BoundingBox boundingBox = BoundingBox.CreateFromSphere(localSphere);
			Vector3D vector3D = boundingBox.Min;
			Vector3D vector3D2 = boundingBox.Max;
			Vector3I value = new Vector3I((int)Math.Round(vector3D.X * (double)GridSizeR), (int)Math.Round(vector3D.Y * (double)GridSizeR), (int)Math.Round(vector3D.Z * (double)GridSizeR));
			Vector3I value2 = new Vector3I((int)Math.Round(vector3D2.X * (double)GridSizeR), (int)Math.Round(vector3D2.Y * (double)GridSizeR), (int)Math.Round(vector3D2.Z * (double)GridSizeR));
			Vector3I start = Vector3I.Min(value, value2);
			Vector3I end = Vector3I.Max(value, value2);
<<<<<<< HEAD
			if (!useOptimization || (end - start).Volume() < m_cubes.Count)
=======
			if (!useOptimization || (end - start).Volume() < m_cubes.get_Count())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
				Vector3I next = vector3I_RangeIterator.Current;
				MyCube cube = default(MyCube);
				while (vector3I_RangeIterator.IsValid())
				{
<<<<<<< HEAD
					if (m_cubes.TryGetValue(next, out var value3))
=======
					if (m_cubes.TryGetValue(next, ref cube))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						AddBlockInSphere(ref aabb, blocks, checkTriangles, ref localSphere, cube);
					}
					vector3I_RangeIterator.GetNext(out next);
				}
				return;
			}
<<<<<<< HEAD
			foreach (MyCube value4 in m_cubes.Values)
			{
				AddBlockInSphere(ref aabb, blocks, checkTriangles, ref localSphere, value4);
=======
			foreach (MyCube value3 in m_cubes.get_Values())
			{
				AddBlockInSphere(ref aabb, blocks, checkTriangles, ref localSphere, value3);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void AddBlockInSphere(ref BoundingBoxD aabb, HashSet<MySlimBlock> blocks, bool checkTriangles, ref BoundingSphere localSphere, MyCube cube)
		{
			MySlimBlock cubeBlock = cube.CubeBlock;
			if (!new BoundingBox(cubeBlock.Min * GridSize - GridSizeHalf, cubeBlock.Max * GridSize + GridSizeHalf).Intersects(localSphere))
			{
				return;
			}
			if (checkTriangles)
			{
				if (cubeBlock.FatBlock == null || cubeBlock.FatBlock.GetIntersectionWithAABB(ref aabb))
				{
					blocks.Add(cubeBlock);
				}
			}
			else
			{
				blocks.Add(cubeBlock);
			}
		}

		private void QuerySphere(BoundingSphereD sphere, List<MyEntity> blocks)
		{
			if (base.PositionComp == null)
			{
				return;
			}
			if (base.Closed)
			{
				MyLog.Default.WriteLine("Grid was Closed in MyCubeGrid.QuerySphere!");
			}
			if (sphere.Contains(base.PositionComp.WorldVolume) == ContainmentType.Contains)
			{
				foreach (MyCubeBlock fatBlock in m_fatBlocks)
				{
					if (fatBlock.Closed)
<<<<<<< HEAD
					{
						continue;
					}
					blocks.Add(fatBlock);
					foreach (MyHierarchyComponentBase child in fatBlock.Hierarchy.Children)
					{
=======
					{
						continue;
					}
					blocks.Add(fatBlock);
					foreach (MyHierarchyComponentBase child in fatBlock.Hierarchy.Children)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						MyEntity myEntity = (MyEntity)child.Entity;
						if (myEntity != null)
						{
							blocks.Add(myEntity);
						}
					}
				}
				return;
			}
			BoundingBoxD boundingBoxD = new BoundingBoxD(sphere.Center - new Vector3D(sphere.Radius), sphere.Center + new Vector3D(sphere.Radius)).TransformFast(base.PositionComp.WorldMatrixNormalizedInv);
			Vector3D min = boundingBoxD.Min;
			Vector3D max = boundingBoxD.Max;
			Vector3I value = new Vector3I((int)Math.Round(min.X * (double)GridSizeR), (int)Math.Round(min.Y * (double)GridSizeR), (int)Math.Round(min.Z * (double)GridSizeR));
			Vector3I value2 = new Vector3I((int)Math.Round(max.X * (double)GridSizeR), (int)Math.Round(max.Y * (double)GridSizeR), (int)Math.Round(max.Z * (double)GridSizeR));
			Vector3I value3 = Vector3I.Min(value, value2);
			Vector3I value4 = Vector3I.Max(value, value2);
			value3 = Vector3I.Max(value3, Min);
			value4 = Vector3I.Min(value4, Max);
			if (value3.X > value4.X || value3.Y > value4.Y || value3.Z > value4.Z)
			{
				return;
			}
			Vector3 vector = new Vector3(0.5f);
			BoundingBox box = default(BoundingBox);
			BoundingSphere boundingSphere = new BoundingSphere((Vector3)boundingBoxD.Center * GridSizeR, (float)sphere.Radius * GridSizeR);
			if ((value4 - value3).Size > m_cubeBlocks.get_Count())
			{
				foreach (MyCubeBlock fatBlock2 in m_fatBlocks)
				{
					if (fatBlock2.Closed)
					{
						continue;
					}
					box.Min = fatBlock2.Min - vector;
					box.Max = fatBlock2.Max + vector;
					if (!boundingSphere.Intersects(box))
<<<<<<< HEAD
					{
						continue;
					}
					blocks.Add(fatBlock2);
					foreach (MyHierarchyComponentBase child2 in fatBlock2.Hierarchy.Children)
					{
=======
					{
						continue;
					}
					blocks.Add(fatBlock2);
					foreach (MyHierarchyComponentBase child2 in fatBlock2.Hierarchy.Children)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						MyEntity myEntity2 = (MyEntity)child2.Entity;
						if (myEntity2 != null)
						{
							blocks.Add(myEntity2);
						}
					}
				}
				return;
			}
			if (m_tmpQueryCubeBlocks == null)
			{
				m_tmpQueryCubeBlocks = new HashSet<MyEntity>();
			}
			if (m_cubes == null)
			{
				MyLog.Default.WriteLine("m_cubes null in MyCubeGrid.QuerySphere!");
			}
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref value3, ref value4);
			Vector3I next = vector3I_RangeIterator.Current;
			MyCube myCube = default(MyCube);
			while (vector3I_RangeIterator.IsValid())
			{
<<<<<<< HEAD
				if (m_cubes.TryGetValue(next, out var value5) && value5.CubeBlock.FatBlock != null && value5.CubeBlock.FatBlock != null && !value5.CubeBlock.FatBlock.Closed && !m_tmpQueryCubeBlocks.Contains(value5.CubeBlock.FatBlock))
				{
					box.Min = value5.CubeBlock.Min - vector;
					box.Max = value5.CubeBlock.Max + vector;
					if (boundingSphere.Intersects(box))
					{
						blocks.Add(value5.CubeBlock.FatBlock);
						m_tmpQueryCubeBlocks.Add(value5.CubeBlock.FatBlock);
						foreach (MyHierarchyComponentBase child3 in value5.CubeBlock.FatBlock.Hierarchy.Children)
=======
				if (m_cubes.TryGetValue(next, ref myCube) && myCube.CubeBlock.FatBlock != null && myCube.CubeBlock.FatBlock != null && !myCube.CubeBlock.FatBlock.Closed && !m_tmpQueryCubeBlocks.Contains((MyEntity)myCube.CubeBlock.FatBlock))
				{
					box.Min = myCube.CubeBlock.Min - vector;
					box.Max = myCube.CubeBlock.Max + vector;
					if (boundingSphere.Intersects(box))
					{
						blocks.Add(myCube.CubeBlock.FatBlock);
						m_tmpQueryCubeBlocks.Add((MyEntity)myCube.CubeBlock.FatBlock);
						foreach (MyHierarchyComponentBase child3 in myCube.CubeBlock.FatBlock.Hierarchy.Children)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							MyEntity myEntity3 = (MyEntity)child3.Entity;
							if (myEntity3 != null)
							{
								blocks.Add(myEntity3);
								m_tmpQueryCubeBlocks.Add(myEntity3);
							}
						}
					}
				}
				vector3I_RangeIterator.GetNext(out next);
			}
			m_tmpQueryCubeBlocks.Clear();
		}

		/// <summary>
		/// Correct interesection transforming vertices from cube to grid coordinates
		/// </summary>
		private void TransformCubeToGrid(ref MyIntersectionResultLineTriangleEx triangle, ref Matrix cubeLocalMatrix, ref MatrixD? cubeWorldMatrix)
		{
			if (!cubeWorldMatrix.HasValue)
			{
				MatrixD worldMatrix = base.WorldMatrix;
				triangle.IntersectionPointInObjectSpace = Vector3.Transform(triangle.IntersectionPointInObjectSpace, ref cubeLocalMatrix);
				triangle.IntersectionPointInWorldSpace = Vector3D.Transform(triangle.IntersectionPointInObjectSpace, worldMatrix);
				triangle.NormalInObjectSpace = Vector3.TransformNormal(triangle.NormalInObjectSpace, ref cubeLocalMatrix);
				triangle.NormalInWorldSpace = Vector3.TransformNormal(triangle.NormalInObjectSpace, worldMatrix);
			}
			else
			{
				Vector3 intersectionPointInObjectSpace = triangle.IntersectionPointInObjectSpace;
				Vector3 normalInObjectSpace = triangle.NormalInObjectSpace;
				triangle.IntersectionPointInObjectSpace = Vector3.Transform(intersectionPointInObjectSpace, ref cubeLocalMatrix);
				triangle.IntersectionPointInWorldSpace = Vector3D.Transform(intersectionPointInObjectSpace, cubeWorldMatrix.Value);
				triangle.NormalInObjectSpace = Vector3.TransformNormal(normalInObjectSpace, ref cubeLocalMatrix);
				triangle.NormalInWorldSpace = Vector3.TransformNormal(normalInObjectSpace, cubeWorldMatrix.Value);
			}
			triangle.Triangle.InputTriangle.Transform(ref cubeLocalMatrix);
		}

		private void QueryLine(LineD line, List<MyLineSegmentOverlapResult<MyEntity>> blocks)
		{
			MyLineSegmentOverlapResult<MyEntity> item = default(MyLineSegmentOverlapResult<MyEntity>);
			BoundingBoxD box = default(BoundingBoxD);
			MatrixD matrix = base.PositionComp.WorldMatrixNormalizedInv;
			Vector3D.Transform(ref line.From, ref matrix, out var result);
			Vector3D.Transform(ref line.To, ref matrix, out var result2);
			RayD rayD = new RayD(result, Vector3D.Normalize(result2 - result));
			RayCastCells(line.From, line.To, m_cacheRayCastCells);
			MyCube myCube = default(MyCube);
			foreach (Vector3I cacheRayCastCell in m_cacheRayCastCells)
			{
<<<<<<< HEAD
				if (m_cubes.TryGetValue(cacheRayCastCell, out var value) && value.CubeBlock.FatBlock != null)
=======
				if (m_cubes.TryGetValue(cacheRayCastCell, ref myCube) && myCube.CubeBlock.FatBlock != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyCubeBlock myCubeBlock = (MyCubeBlock)(item.Element = myCube.CubeBlock.FatBlock);
					box.Min = myCubeBlock.Min * GridSize - GridSizeHalfVector;
					box.Max = myCubeBlock.Max * GridSize + GridSizeHalfVector;
					double? num = rayD.Intersects(box);
					if (num.HasValue)
					{
						item.Distance = num.Value;
						blocks.Add(item);
					}
				}
			}
		}

		private void QueryAABB(BoundingBoxD box, List<MyEntity> blocks)
		{
			if (blocks == null || base.PositionComp == null)
			{
				return;
			}
			if (box.Contains(base.PositionComp.WorldAABB) == ContainmentType.Contains)
			{
				foreach (MyCubeBlock fatBlock2 in m_fatBlocks)
				{
					if (fatBlock2.Closed)
					{
						continue;
					}
					blocks.Add(fatBlock2);
					if (fatBlock2.Hierarchy == null)
					{
						continue;
					}
					foreach (MyHierarchyComponentBase child in fatBlock2.Hierarchy.Children)
					{
						if (child.Container != null)
						{
							blocks.Add((MyEntity)child.Container.Entity);
						}
					}
				}
				return;
			}
			MyOrientedBoundingBoxD myOrientedBoundingBoxD = MyOrientedBoundingBoxD.Create(box, base.PositionComp.WorldMatrixNormalizedInv);
			myOrientedBoundingBoxD.Center *= (double)GridSizeR;
			myOrientedBoundingBoxD.HalfExtent *= (double)GridSizeR;
			box = box.TransformFast(base.PositionComp.WorldMatrixNormalizedInv);
			Vector3D min = box.Min;
			Vector3D max = box.Max;
			Vector3I value = new Vector3I((int)Math.Round(min.X * (double)GridSizeR), (int)Math.Round(min.Y * (double)GridSizeR), (int)Math.Round(min.Z * (double)GridSizeR));
			Vector3I value2 = new Vector3I((int)Math.Round(max.X * (double)GridSizeR), (int)Math.Round(max.Y * (double)GridSizeR), (int)Math.Round(max.Z * (double)GridSizeR));
			Vector3I value3 = Vector3I.Min(value, value2);
			Vector3I value4 = Vector3I.Max(value, value2);
			value3 = Vector3I.Max(value3, Min);
			value4 = Vector3I.Min(value4, Max);
			if (value3.X > value4.X || value3.Y > value4.Y || value3.Z > value4.Z)
			{
				return;
			}
			Vector3 vector = new Vector3(0.5f);
			BoundingBoxD box2 = default(BoundingBoxD);
			if ((value4 - value3).Size > m_cubeBlocks.get_Count())
			{
				foreach (MyCubeBlock fatBlock3 in m_fatBlocks)
				{
					if (fatBlock3.Closed)
					{
						continue;
					}
					box2.Min = fatBlock3.Min - vector;
					box2.Max = fatBlock3.Max + vector;
					if (!myOrientedBoundingBoxD.Intersects(ref box2))
					{
						continue;
					}
					blocks.Add(fatBlock3);
					if (fatBlock3.Hierarchy == null)
					{
						continue;
					}
					foreach (MyHierarchyComponentBase child2 in fatBlock3.Hierarchy.Children)
					{
						if (child2.Container != null)
						{
							blocks.Add((MyEntity)child2.Container.Entity);
						}
					}
				}
				return;
			}
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref value3, ref value4);
			Vector3I next = vector3I_RangeIterator.Current;
			if (m_tmpQueryCubeBlocks == null)
			{
				m_tmpQueryCubeBlocks = new HashSet<MyEntity>();
			}
			MyCube myCube = default(MyCube);
			while (vector3I_RangeIterator.IsValid())
			{
<<<<<<< HEAD
				if (m_cubes != null && m_cubes.TryGetValue(next, out var value5) && value5.CubeBlock.FatBlock != null)
				{
					MyCubeBlock fatBlock = value5.CubeBlock.FatBlock;
					if (!m_tmpQueryCubeBlocks.Contains(fatBlock))
					{
						box2.Min = value5.CubeBlock.Min - vector;
						box2.Max = value5.CubeBlock.Max + vector;
=======
				if (m_cubes != null && m_cubes.TryGetValue(next, ref myCube) && myCube.CubeBlock.FatBlock != null)
				{
					MyCubeBlock fatBlock = myCube.CubeBlock.FatBlock;
					if (!m_tmpQueryCubeBlocks.Contains((MyEntity)fatBlock))
					{
						box2.Min = myCube.CubeBlock.Min - vector;
						box2.Max = myCube.CubeBlock.Max + vector;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (myOrientedBoundingBoxD.Intersects(ref box2))
						{
							m_tmpQueryCubeBlocks.Add((MyEntity)fatBlock);
							blocks.Add(fatBlock);
							if (fatBlock.Hierarchy != null)
							{
								foreach (MyHierarchyComponentBase child3 in fatBlock.Hierarchy.Children)
								{
									if (child3.Container != null)
									{
										blocks.Add((MyEntity)child3.Container.Entity);
										m_tmpQueryCubeBlocks.Add((MyEntity)fatBlock);
									}
								}
							}
						}
					}
				}
				vector3I_RangeIterator.GetNext(out next);
			}
			m_tmpQueryCubeBlocks.Clear();
		}

		public void GetBlocksIntersectingOBB(in BoundingBoxD box, in MatrixD boxTransform, List<MySlimBlock> blocks)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			if (blocks == null || base.PositionComp == null)
			{
				return;
			}
			MatrixD m = boxTransform * base.PositionComp.WorldMatrixNormalizedInv;
			MyOrientedBoundingBox myOrientedBoundingBox = MyOrientedBoundingBox.Create((BoundingBox)box, m);
			BoundingBox box2 = base.PositionComp.LocalAABB;
			if (myOrientedBoundingBox.Contains(ref box2) == ContainmentType.Contains)
			{
				Enumerator<MySlimBlock> enumerator = GetBlocks().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current = enumerator.get_Current();
						if (current.FatBlock == null || !current.FatBlock.Closed)
						{
							blocks.Add(current);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				return;
			}
			myOrientedBoundingBox.Center *= GridSizeR;
			myOrientedBoundingBox.HalfExtent *= GridSizeR;
			BoundingBoxD boundingBoxD = box.TransformFast(m);
			Vector3D min = boundingBoxD.Min;
			Vector3D max = boundingBoxD.Max;
			Vector3I value = new Vector3I((int)Math.Round(min.X * (double)GridSizeR), (int)Math.Round(min.Y * (double)GridSizeR), (int)Math.Round(min.Z * (double)GridSizeR));
			Vector3I value2 = new Vector3I((int)Math.Round(max.X * (double)GridSizeR), (int)Math.Round(max.Y * (double)GridSizeR), (int)Math.Round(max.Z * (double)GridSizeR));
			Vector3I value3 = Vector3I.Min(value, value2);
			Vector3I value4 = Vector3I.Max(value, value2);
			value3 = Vector3I.Max(value3, Min);
			value4 = Vector3I.Min(value4, Max);
			if (value3.X > value4.X || value3.Y > value4.Y || value3.Z > value4.Z)
			{
				return;
			}
			Vector3 vector = new Vector3(0.5f);
			BoundingBox box3 = default(BoundingBox);
<<<<<<< HEAD
			if ((value4 - value3).Size > m_cubeBlocks.Count)
=======
			if ((value4 - value3).Size > m_cubeBlocks.get_Count())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Enumerator<MySlimBlock> enumerator = GetBlocks().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
<<<<<<< HEAD
						box3.Min = block2.Min - vector;
						box3.Max = block2.Max + vector;
						if (myOrientedBoundingBox.Intersects(ref box3))
=======
						MySlimBlock current2 = enumerator.get_Current();
						if (current2.FatBlock == null || !current2.FatBlock.Closed)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							box3.Min = current2.Min - vector;
							box3.Max = current2.Max + vector;
							if (myOrientedBoundingBox.Intersects(ref box3))
							{
								blocks.Add(current2);
							}
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				return;
			}
			if (m_tmpQuerySlimBlocks == null)
			{
				m_tmpQuerySlimBlocks = new HashSet<MySlimBlock>();
			}
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref value3, ref value4);
			Vector3I next = vector3I_RangeIterator.Current;
			MyCube myCube = default(MyCube);
			while (vector3I_RangeIterator.IsValid())
			{
<<<<<<< HEAD
				if (m_cubes != null && m_cubes.TryGetValue(next, out var value5) && value5.CubeBlock != null)
				{
					MySlimBlock cubeBlock = value5.CubeBlock;
=======
				if (m_cubes != null && m_cubes.TryGetValue(next, ref myCube) && myCube.CubeBlock != null)
				{
					MySlimBlock cubeBlock = myCube.CubeBlock;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (!m_tmpQuerySlimBlocks.Contains(cubeBlock))
					{
						box3.Min = cubeBlock.Min - vector;
						box3.Max = cubeBlock.Max + vector;
						if (myOrientedBoundingBox.Intersects(ref box3))
						{
							m_tmpQuerySlimBlocks.Add(cubeBlock);
							blocks.Add(cubeBlock);
						}
					}
				}
				vector3I_RangeIterator.GetNext(out next);
			}
			m_tmpQuerySlimBlocks.Clear();
		}

		/// <summary>
		/// Optimized version where spheres are sorted from smallest to largest
		/// </summary>
		/// <param name="sphere1"></param>
		/// <param name="sphere2"></param>
		/// <param name="sphere3"></param>
		/// <param name="blocks1"></param>
		/// <param name="blocks2"></param>
		/// <param name="blocks3"></param>
		/// <param name="respectDeformationRatio"></param>
		/// <param name="detectionBlockHalfSize"></param>
		/// <param name="invWorldGrid"></param>
		public void GetBlocksInsideSpheres(ref BoundingSphereD sphere1, ref BoundingSphereD sphere2, ref BoundingSphereD sphere3, HashSet<MySlimBlock> blocks1, HashSet<MySlimBlock> blocks2, HashSet<MySlimBlock> blocks3, bool respectDeformationRatio, float detectionBlockHalfSize, ref MatrixD invWorldGrid)
		{
			blocks1.Clear();
			blocks2.Clear();
			blocks3.Clear();
			m_processedBlocks.Clear();
			Vector3D.Transform(ref sphere3.Center, ref invWorldGrid, out var result);
			Vector3I vector3I = Vector3I.Round((result - sphere3.Radius) * GridSizeR);
			Vector3I vector3I2 = Vector3I.Round((result + sphere3.Radius) * GridSizeR);
			Vector3 vector = new Vector3(detectionBlockHalfSize);
			BoundingSphereD boundingSphereD = new BoundingSphereD(result, sphere1.Radius);
			BoundingSphereD boundingSphereD2 = new BoundingSphereD(result, sphere2.Radius);
			BoundingSphereD boundingSphereD3 = new BoundingSphereD(result, sphere3.Radius);
<<<<<<< HEAD
			if ((vector3I2.X - vector3I.X) * (vector3I2.Y - vector3I.Y) * (vector3I2.Z - vector3I.Z) < m_cubes.Count)
=======
			if ((vector3I2.X - vector3I.X) * (vector3I2.Y - vector3I.Y) * (vector3I2.Z - vector3I.Z) < m_cubes.get_Count())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Vector3I vector3I3 = default(Vector3I);
				vector3I3.X = vector3I.X;
				MyCube myCube = default(MyCube);
				while (vector3I3.X <= vector3I2.X)
				{
					vector3I3.Y = vector3I.Y;
					while (vector3I3.Y <= vector3I2.Y)
					{
						vector3I3.Z = vector3I.Z;
						while (vector3I3.Z <= vector3I2.Z)
						{
<<<<<<< HEAD
							if (m_cubes.TryGetValue(key, out var value))
							{
								MySlimBlock cubeBlock = value.CubeBlock;
=======
							if (m_cubes.TryGetValue(vector3I3, ref myCube))
							{
								MySlimBlock cubeBlock = myCube.CubeBlock;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								if (cubeBlock.FatBlock == null || !m_processedBlocks.Contains(cubeBlock.FatBlock))
								{
									m_processedBlocks.Add(cubeBlock.FatBlock);
									if (respectDeformationRatio)
									{
										boundingSphereD.Radius = sphere1.Radius * (double)cubeBlock.DeformationRatio;
										boundingSphereD2.Radius = sphere2.Radius * (double)cubeBlock.DeformationRatio;
										boundingSphereD3.Radius = sphere3.Radius * (double)cubeBlock.DeformationRatio;
									}
									BoundingBox boundingBox = ((cubeBlock.FatBlock == null) ? new BoundingBox(cubeBlock.Position * GridSize - vector, cubeBlock.Position * GridSize + vector) : new BoundingBox(cubeBlock.Min * GridSize - GridSizeHalf, cubeBlock.Max * GridSize + GridSizeHalf));
									if (boundingBox.Intersects(boundingSphereD3))
									{
										if (boundingBox.Intersects(boundingSphereD2))
										{
											if (boundingBox.Intersects(boundingSphereD))
											{
												blocks1.Add(cubeBlock);
											}
											else
											{
												blocks2.Add(cubeBlock);
											}
										}
										else
										{
											blocks3.Add(cubeBlock);
										}
									}
								}
							}
							vector3I3.Z++;
						}
						vector3I3.Y++;
					}
					vector3I3.X++;
				}
			}
			else
			{
<<<<<<< HEAD
				foreach (MyCube value2 in m_cubes.Values)
				{
					MySlimBlock cubeBlock2 = value2.CubeBlock;
=======
				foreach (MyCube value in m_cubes.get_Values())
				{
					MySlimBlock cubeBlock2 = value.CubeBlock;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (cubeBlock2.FatBlock != null && m_processedBlocks.Contains(cubeBlock2.FatBlock))
					{
						continue;
					}
					m_processedBlocks.Add(cubeBlock2.FatBlock);
					if (respectDeformationRatio)
					{
						boundingSphereD.Radius = sphere1.Radius * (double)cubeBlock2.DeformationRatio;
						boundingSphereD2.Radius = sphere2.Radius * (double)cubeBlock2.DeformationRatio;
						boundingSphereD3.Radius = sphere3.Radius * (double)cubeBlock2.DeformationRatio;
					}
					BoundingBox boundingBox2 = ((cubeBlock2.FatBlock == null) ? new BoundingBox(cubeBlock2.Position * GridSize - vector, cubeBlock2.Position * GridSize + vector) : new BoundingBox(cubeBlock2.Min * GridSize - GridSizeHalf, cubeBlock2.Max * GridSize + GridSizeHalf));
					if (!boundingBox2.Intersects(boundingSphereD3))
					{
						continue;
					}
					if (boundingBox2.Intersects(boundingSphereD2))
					{
						if (boundingBox2.Intersects(boundingSphereD))
						{
							blocks1.Add(cubeBlock2);
						}
						else
						{
							blocks2.Add(cubeBlock2);
						}
					}
					else
					{
						blocks3.Add(cubeBlock2);
					}
				}
			}
			m_processedBlocks.Clear();
		}

		/// <summary>
		/// Obtains all blocks intersected by raycast
		/// </summary>
		internal HashSet<MyCube> RayCastBlocksAll(Vector3D worldStart, Vector3D worldEnd)
		{
			RayCastCells(worldStart, worldEnd, m_cacheRayCastCells);
			HashSet<MyCube> val = new HashSet<MyCube>();
			foreach (Vector3I cacheRayCastCell in m_cacheRayCastCells)
			{
				if (m_cubes.ContainsKey(cacheRayCastCell))
				{
					val.Add(m_cubes.get_Item(cacheRayCastCell));
				}
			}
			return val;
		}

		/// <summary>
		/// Obtains all blocks intersected by raycast
		/// </summary>
		internal List<MyCube> RayCastBlocksAllOrdered(Vector3D worldStart, Vector3D worldEnd)
		{
			RayCastCells(worldStart, worldEnd, m_cacheRayCastCells);
			List<MyCube> list = new List<MyCube>();
			foreach (Vector3I cacheRayCastCell in m_cacheRayCastCells)
			{
				if (m_cubes.ContainsKey(cacheRayCastCell) && !list.Contains(m_cubes.get_Item(cacheRayCastCell)))
				{
					list.Add(m_cubes.get_Item(cacheRayCastCell));
				}
			}
			return list;
		}

		/// <summary>
		/// Obtains position of first hit block.
		/// </summary>
		public Vector3I? RayCastBlocks(Vector3D worldStart, Vector3D worldEnd)
		{
			RayCastCells(worldStart, worldEnd, m_cacheRayCastCells);
			foreach (Vector3I cacheRayCastCell in m_cacheRayCastCells)
			{
				if (m_cubes.ContainsKey(cacheRayCastCell))
				{
					return cacheRayCastCell;
				}
			}
			return null;
		}

		/// <summary>
		/// Obtains positions of grid cells regardless of whether these cells are taken up by blocks or not.
		/// </summary>
		public void RayCastCells(Vector3D worldStart, Vector3D worldEnd, List<Vector3I> outHitPositions, Vector3I? gridSizeInflate = null, bool havokWorld = false, bool clearOutHitPositions = true)
		{
			MatrixD matrix = base.PositionComp.WorldMatrixNormalizedInv;
			Vector3D.Transform(ref worldStart, ref matrix, out var result);
			Vector3D.Transform(ref worldEnd, ref matrix, out var result2);
			Vector3 gridSizeHalfVector = GridSizeHalfVector;
			result += gridSizeHalfVector;
			result2 += gridSizeHalfVector;
			Vector3I min = Min - Vector3I.One;
			Vector3I max = Max + Vector3I.One;
			if (gridSizeInflate.HasValue)
			{
				min -= gridSizeInflate.Value;
				max += gridSizeInflate.Value;
			}
			if (clearOutHitPositions)
			{
				outHitPositions.Clear();
			}
			MyGridIntersection.Calculate(outHitPositions, GridSize, result, result2, min, max);
		}

		/// <summary>
		/// Obtains positions of static grid cells regardless of whether these cells are taken up by blocks or not. Usefull when placing block on voxel.
		/// </summary>
		public static void RayCastStaticCells(Vector3D worldStart, Vector3D worldEnd, List<Vector3I> outHitPositions, float gridSize, Vector3I? gridSizeInflate = null, bool havokWorld = false)
		{
			Vector3D lineStart = worldStart;
			Vector3D lineEnd = worldEnd;
			Vector3D vector3D = new Vector3D(gridSize * 0.5f);
			lineStart += vector3D;
			lineEnd += vector3D;
			Vector3I min = -Vector3I.One;
			Vector3I one = Vector3I.One;
			if (gridSizeInflate.HasValue)
			{
				min -= gridSizeInflate.Value;
				one += gridSizeInflate.Value;
			}
			outHitPositions.Clear();
			if (havokWorld)
			{
				MyGridIntersection.CalculateHavok(outHitPositions, gridSize, lineStart, lineEnd, min, one);
			}
			else
			{
				MyGridIntersection.Calculate(outHitPositions, gridSize, lineStart, lineEnd, min, one);
			}
		}

		void IMyGridConnectivityTest.GetConnectedBlocks(Vector3I minI, Vector3I maxI, Dictionary<Vector3I, ConnectivityResult> outOverlappedCubeBlocks)
		{
			Vector3I pos = default(Vector3I);
			pos.Z = minI.Z;
			while (pos.Z <= maxI.Z)
			{
				pos.Y = minI.Y;
				while (pos.Y <= maxI.Y)
				{
					pos.X = minI.X;
					while (pos.X <= maxI.X)
					{
						MySlimBlock cubeBlock = GetCubeBlock(pos);
						if (cubeBlock != null)
						{
							outOverlappedCubeBlocks[cubeBlock.Position] = new ConnectivityResult
							{
								Definition = cubeBlock.BlockDefinition,
								FatBlock = cubeBlock.FatBlock,
								Orientation = cubeBlock.Orientation,
								Position = cubeBlock.Position
							};
						}
						pos.X++;
					}
					pos.Y++;
				}
				pos.Z++;
			}
		}

		private string MakeCustomName()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int m = 10000;
			long num = MyMath.Mod(base.EntityId, m);
			string text = null;
			if (IsStatic)
			{
				text = MyTexts.GetString(MyCommonTexts.DetailStaticGrid);
			}
			else
			{
				switch (GridSizeEnum)
				{
				case MyCubeSize.Small:
					text = MyTexts.GetString(MyCommonTexts.DetailSmallGrid);
					break;
				case MyCubeSize.Large:
					text = MyTexts.GetString(MyCommonTexts.DetailLargeGrid);
					break;
				}
			}
			stringBuilder.Append(text ?? "Grid").Append(" ").Append(num.ToString());
			return stringBuilder.ToString();
		}

		public void ChangeOwner(MyCubeBlock block, long oldOwner, long newOwner)
		{
			if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
			{
				m_ownershipManager.ChangeBlockOwnership(block, oldOwner, newOwner);
			}
		}

		public void RecalculateOwners()
		{
			if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
			{
				m_ownershipManager.RecalculateOwnersThreadSafe();
			}
		}

		public void UpdateOwnership(long ownerId, bool isFunctional)
		{
			if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
			{
				m_ownershipManager.UpdateOnFunctionalChange(ownerId, isFunctional);
			}
		}

		public override void Teleport(MatrixD worldMatrix, object source = null, bool ignoreAssert = false)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0413: Unknown result type (might be due to invalid IL or missing references)
			//IL_049e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_052d: Unknown result type (might be due to invalid IL or missing references)
			Dictionary<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> dictionary = new Dictionary<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>>();
			Dictionary<MyCubeGrid, Tuple<Vector3, Vector3>> dictionary2 = new Dictionary<MyCubeGrid, Tuple<Vector3, Vector3>>();
			HashSet<VRage.ModAPI.IMyEntity> val = new HashSet<VRage.ModAPI.IMyEntity>();
			MyHashSetDictionary<MyCubeGrid, VRage.ModAPI.IMyEntity> myHashSetDictionary = new MyHashSetDictionary<MyCubeGrid, VRage.ModAPI.IMyEntity>();
<<<<<<< HEAD
			foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in MyCubeGridGroups.Static.Physical.GetGroup(this).Nodes)
			{
				HashSet<VRage.ModAPI.IMyEntity> hashSet2 = new HashSet<VRage.ModAPI.IMyEntity>();
				node.NodeData.Hierarchy.GetChildrenRecursive(hashSet2);
				foreach (VRage.ModAPI.IMyEntity item in hashSet2)
				{
					MyCubeBlock myCubeBlock;
					if ((myCubeBlock = item as MyCubeBlock) != null)
					{
						myCubeBlock.OnTeleport();
					}
				}
			}
			MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(this);
			foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node2 in group.Nodes)
			{
				HashSet<VRage.ModAPI.IMyEntity> hashSet3 = new HashSet<VRage.ModAPI.IMyEntity>();
				hashSet3.Add(node2.NodeData);
				node2.NodeData.Hierarchy.GetChildrenRecursive(hashSet3);
				foreach (VRage.ModAPI.IMyEntity item2 in hashSet3)
				{
					if (item2.Physics == null)
					{
						continue;
					}
					foreach (HkConstraint constraint in ((MyPhysicsBody)item2.Physics).Constraints)
					{
						VRage.ModAPI.IMyEntity entity = constraint.RigidBodyA.GetEntity(0u);
						VRage.ModAPI.IMyEntity entity2 = constraint.RigidBodyB.GetEntity(0u);
						VRage.ModAPI.IMyEntity myEntity = ((item2 == entity) ? entity2 : entity);
						if (!hashSet3.Contains(myEntity) && myEntity != null)
						{
							myHashSetDictionary.Add(node2.NodeData, myEntity);
=======
			Enumerator<MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> enumerator = MyCubeGridGroups.Static.Physical.GetGroup(this).Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node current = enumerator.get_Current();
					HashSet<VRage.ModAPI.IMyEntity> val2 = new HashSet<VRage.ModAPI.IMyEntity>();
					current.NodeData.Hierarchy.GetChildrenRecursive(val2);
					Enumerator<VRage.ModAPI.IMyEntity> enumerator2 = val2.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							MyCubeBlock myCubeBlock;
							if ((myCubeBlock = enumerator2.get_Current() as MyCubeBlock) != null)
							{
								myCubeBlock.OnTeleport();
							}
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(this);
			enumerator = group.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node current2 = enumerator.get_Current();
					HashSet<VRage.ModAPI.IMyEntity> val3 = new HashSet<VRage.ModAPI.IMyEntity>();
					val3.Add((VRage.ModAPI.IMyEntity)current2.NodeData);
					current2.NodeData.Hierarchy.GetChildrenRecursive(val3);
					Enumerator<VRage.ModAPI.IMyEntity> enumerator2 = val3.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							VRage.ModAPI.IMyEntity current3 = enumerator2.get_Current();
							if (current3.Physics == null)
							{
								continue;
							}
							Enumerator<HkConstraint> enumerator3 = ((MyPhysicsBody)current3.Physics).Constraints.GetEnumerator();
							try
							{
								while (enumerator3.MoveNext())
								{
									HkConstraint current4 = enumerator3.get_Current();
									VRage.ModAPI.IMyEntity entity = current4.RigidBodyA.GetEntity(0u);
									VRage.ModAPI.IMyEntity entity2 = current4.RigidBodyB.GetEntity(0u);
									VRage.ModAPI.IMyEntity myEntity = ((current3 == entity) ? entity2 : entity);
									if (!val3.Contains(myEntity) && myEntity != null)
									{
										myHashSetDictionary.Add(current2.NodeData, myEntity);
									}
								}
							}
							finally
							{
								((IDisposable)enumerator3).Dispose();
							}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					dictionary.Add(current2.NodeData, val3);
				}
<<<<<<< HEAD
				dictionary.Add(node2.NodeData, hashSet3);
			}
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item3 in dictionary)
			{
				foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item4 in dictionary)
				{
					if (myHashSetDictionary.TryGet(item3.Key, out var list))
					{
						list.Remove(item4.Key);
						if (list.Count == 0)
						{
							myHashSetDictionary.Remove(item3.Key);
=======
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item in dictionary)
			{
				foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item2 in dictionary)
				{
					if (myHashSetDictionary.TryGet(item.Key, out var list))
					{
						list.Remove((VRage.ModAPI.IMyEntity)item2.Key);
						if (list.get_Count() == 0)
						{
							myHashSetDictionary.Remove(item.Key);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
				}
			}
<<<<<<< HEAD
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item5 in dictionary.Reverse())
			{
				if (item5.Key.Physics == null)
				{
					continue;
				}
				dictionary2[item5.Key] = new Tuple<Vector3, Vector3>(item5.Key.Physics.LinearVelocity, item5.Key.Physics.AngularVelocity);
				foreach (VRage.ModAPI.IMyEntity item6 in item5.Value.Reverse())
				{
					if (item6.Physics != null && item6.Physics is MyPhysicsBody && !((MyPhysicsBody)item6.Physics).IsWelded)
					{
						if (item6.Physics.Enabled)
						{
							item6.Physics.Enabled = false;
						}
						else
						{
							hashSet.Add(item6);
=======
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item3 in Enumerable.Reverse<KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>>>((IEnumerable<KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>>>)dictionary))
			{
				if (item3.Key.Physics == null)
				{
					continue;
				}
				dictionary2[item3.Key] = new Tuple<Vector3, Vector3>(item3.Key.Physics.LinearVelocity, item3.Key.Physics.AngularVelocity);
				foreach (VRage.ModAPI.IMyEntity item4 in Enumerable.Reverse<VRage.ModAPI.IMyEntity>((IEnumerable<VRage.ModAPI.IMyEntity>)item3.Value))
				{
					if (item4.Physics != null && item4.Physics is MyPhysicsBody && !((MyPhysicsBody)item4.Physics).IsWelded)
					{
						if (item4.Physics.Enabled)
						{
							item4.Physics.Enabled = false;
						}
						else
						{
							val.Add(item4);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
				}
			}
			Vector3D vector3D = worldMatrix.Translation - base.PositionComp.GetPosition();
<<<<<<< HEAD
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item7 in dictionary)
			{
				MatrixD worldMatrix2 = item7.Key.PositionComp.WorldMatrixRef;
				worldMatrix2.Translation += vector3D;
				item7.Key.PositionComp.SetWorldMatrix(ref worldMatrix2, source, forceUpdate: false, updateChildren: true, updateLocal: true, skipTeleportCheck: true);
				if (!myHashSetDictionary.TryGet(item7.Key, out var list2))
				{
					continue;
				}
				foreach (VRage.ModAPI.IMyEntity item8 in list2)
				{
					MatrixD worldMatrix3 = item8.PositionComp.WorldMatrixRef;
					worldMatrix3.Translation += vector3D;
					item8.PositionComp.SetWorldMatrix(ref worldMatrix3, source, forceUpdate: false, updateChildren: true, updateLocal: true, skipTeleportCheck: true);
=======
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item5 in dictionary)
			{
				MatrixD worldMatrix2 = item5.Key.PositionComp.WorldMatrixRef;
				worldMatrix2.Translation += vector3D;
				item5.Key.PositionComp.SetWorldMatrix(ref worldMatrix2, source, forceUpdate: false, updateChildren: true, updateLocal: true, skipTeleportCheck: true);
				if (!myHashSetDictionary.TryGet(item5.Key, out var list2))
				{
					continue;
				}
				Enumerator<VRage.ModAPI.IMyEntity> enumerator2 = list2.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						VRage.ModAPI.IMyEntity current10 = enumerator2.get_Current();
						MatrixD worldMatrix3 = current10.PositionComp.WorldMatrixRef;
						worldMatrix3.Translation += vector3D;
						current10.PositionComp.SetWorldMatrix(ref worldMatrix3, source, forceUpdate: false, updateChildren: true, updateLocal: true, skipTeleportCheck: true);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
			}
			BoundingBoxD boundingBoxD = BoundingBoxD.CreateInvalid();
<<<<<<< HEAD
			foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node3 in group.Nodes)
			{
				boundingBoxD.Include(node3.NodeData.PositionComp.WorldAABB);
=======
			enumerator = group.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node current11 = enumerator.get_Current();
					boundingBoxD.Include(current11.NodeData.PositionComp.WorldAABB);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			boundingBoxD = boundingBoxD.GetInflated(MyClusterTree.MinimumDistanceFromBorder);
			MyPhysics.EnsurePhysicsSpace(boundingBoxD);
			HkWorld hkWorld = null;
<<<<<<< HEAD
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item9 in dictionary)
			{
				if (item9.Key.Physics == null)
				{
					continue;
				}
				foreach (VRage.ModAPI.IMyEntity item10 in item9.Value)
				{
					if (item10.Physics != null && !((MyPhysicsBody)item10.Physics).IsWelded && !hashSet.Contains(item10))
					{
						((MyPhysicsBody)item10.Physics).LinearVelocity = dictionary2[item9.Key].Item1;
						((MyPhysicsBody)item10.Physics).AngularVelocity = dictionary2[item9.Key].Item2;
						((MyPhysicsBody)item10.Physics).EnableBatched();
						if (hkWorld == null)
						{
							hkWorld = ((MyPhysicsBody)item10.Physics).HavokWorld;
=======
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item6 in dictionary)
			{
				if (item6.Key.Physics == null)
				{
					continue;
				}
				Enumerator<VRage.ModAPI.IMyEntity> enumerator2 = item6.Value.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						VRage.ModAPI.IMyEntity current13 = enumerator2.get_Current();
						if (current13.Physics != null && !((MyPhysicsBody)current13.Physics).IsWelded && !val.Contains(current13))
						{
							((MyPhysicsBody)current13.Physics).LinearVelocity = dictionary2[item6.Key].Item1;
							((MyPhysicsBody)current13.Physics).AngularVelocity = dictionary2[item6.Key].Item2;
							((MyPhysicsBody)current13.Physics).EnableBatched();
							if (hkWorld == null)
							{
								hkWorld = ((MyPhysicsBody)current13.Physics).HavokWorld;
							}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
			}
			hkWorld?.FinishBatch();
<<<<<<< HEAD
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item11 in dictionary.Reverse())
			{
				if (item11.Key.Physics == null)
				{
					continue;
				}
				foreach (VRage.ModAPI.IMyEntity item12 in item11.Value.Reverse())
				{
					if (item12.Physics != null && item12.Physics is MyPhysicsBody && !((MyPhysicsBody)item12.Physics).IsWelded && !hashSet.Contains(item12))
					{
						((MyPhysicsBody)item12.Physics).FinishAddBatch();
=======
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item7 in Enumerable.Reverse<KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>>>((IEnumerable<KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>>>)dictionary))
			{
				if (item7.Key.Physics == null)
				{
					continue;
				}
				foreach (VRage.ModAPI.IMyEntity item8 in Enumerable.Reverse<VRage.ModAPI.IMyEntity>((IEnumerable<VRage.ModAPI.IMyEntity>)item7.Value))
				{
					if (item8.Physics != null && item8.Physics is MyPhysicsBody && !((MyPhysicsBody)item8.Physics).IsWelded && !val.Contains(item8))
					{
						((MyPhysicsBody)item8.Physics).FinishAddBatch();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
		}

		public bool CanBeTeleported(MyGridJumpDriveSystem jumpingSystem, out MyGridJumpDriveSystem.MyJumpFailReason reason)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			reason = MyGridJumpDriveSystem.MyJumpFailReason.None;
			if (MyFixedGrids.IsRooted(this))
			{
				reason = MyGridJumpDriveSystem.MyJumpFailReason.Static;
				return false;
			}
			Enumerator<MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> enumerator = MyCubeGridGroups.Static.Physical.GetGroup(this).Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node current = enumerator.get_Current();
					if (current.NodeData.Physics != null)
					{
						if (current.NodeData.IsStatic)
						{
							reason = MyGridJumpDriveSystem.MyJumpFailReason.Locked;
							return false;
						}
						if (MyFixedGrids.IsRooted(current.NodeData))
						{
							reason = MyGridJumpDriveSystem.MyJumpFailReason.Static;
							return false;
						}
						if (current.NodeData.GridSystems.JumpSystem.IsJumping && current.NodeData.GridSystems.JumpSystem != jumpingSystem)
						{
							reason = MyGridJumpDriveSystem.MyJumpFailReason.AlreadyJumping;
							return false;
						}
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return true;
		}

		public BoundingBoxD GetPhysicalGroupAABB()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			if (base.MarkedForClose)
			{
				return BoundingBoxD.CreateInvalid();
			}
			BoundingBoxD worldAABB = base.PositionComp.WorldAABB;
			MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(this);
			if (group == null)
			{
				return worldAABB;
			}
			Enumerator<MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> enumerator = group.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node current = enumerator.get_Current();
					if (current.NodeData.PositionComp != null)
					{
						worldAABB.Include(current.NodeData.PositionComp.WorldAABB);
					}
				}
				return worldAABB;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public MyFracturedBlock CreateFracturedBlock(MyObjectBuilder_FracturedBlock fracturedBlockBuilder, Vector3I position)
		{
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_FracturedBlock), "FracturedBlockLarge");
			MyDefinitionManager.Static.GetCubeBlockDefinition(id);
<<<<<<< HEAD
			if (m_cubes.TryGetValue(position, out var value))
=======
			MyCube myCube = default(MyCube);
			if (m_cubes.TryGetValue(position, ref myCube))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				RemoveBlockInternal(myCube.CubeBlock, close: true);
			}
			fracturedBlockBuilder.CreatingFracturedBlock = true;
			MySlimBlock mySlimBlock = AddBlock(fracturedBlockBuilder, testMerge: false);
			if (mySlimBlock != null)
			{
				MyFracturedBlock myFracturedBlock = mySlimBlock.FatBlock as MyFracturedBlock;
				myFracturedBlock.Render.UpdateRenderObject(visible: true);
				UpdateBlockNeighbours(myFracturedBlock.SlimBlock);
				return myFracturedBlock;
			}
			return null;
		}

		private MyFracturedBlock CreateFracturedBlock(MyFracturedBlock.Info info)
		{
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_FracturedBlock), "FracturedBlockLarge");
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(id);
			Vector3I position = info.Position;
<<<<<<< HEAD
			if (m_cubes.TryGetValue(position, out var value))
=======
			MyCube myCube = default(MyCube);
			if (m_cubes.TryGetValue(position, ref myCube))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				RemoveBlock(myCube.CubeBlock);
			}
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = CreateBlockObjectBuilder(cubeBlockDefinition, position, new MyBlockOrientation(ref Quaternion.Identity), 0L, 0L, fullyBuilt: true);
			myObjectBuilder_CubeBlock.ColorMaskHSV = Vector3.Zero;
			(myObjectBuilder_CubeBlock as MyObjectBuilder_FracturedBlock).CreatingFracturedBlock = true;
			MySlimBlock mySlimBlock = AddBlock(myObjectBuilder_CubeBlock, testMerge: false);
			if (mySlimBlock == null)
			{
				info.Shape.RemoveReference();
				return null;
			}
			MyFracturedBlock myFracturedBlock = mySlimBlock.FatBlock as MyFracturedBlock;
			myFracturedBlock.OriginalBlocks = info.OriginalBlocks;
			myFracturedBlock.Orientations = info.Orientations;
			myFracturedBlock.MultiBlocks = info.MultiBlocks;
			myFracturedBlock.SetDataFromHavok(info.Shape, info.Compound);
			myFracturedBlock.Render.UpdateRenderObject(visible: true);
			UpdateBlockNeighbours(myFracturedBlock.SlimBlock);
			if (Sync.IsServer)
			{
				MySyncDestructions.CreateFracturedBlock((MyObjectBuilder_FracturedBlock)myFracturedBlock.GetObjectBuilderCubeBlock(), base.EntityId, position);
			}
			return myFracturedBlock;
		}

		public void OnIntegrityChanged(MySlimBlock block, bool handWelded)
		{
			NotifyBlockIntegrityChanged(block, handWelded);
		}

		public void PasteBlocksToGrid(List<MyObjectBuilder_CubeGrid> gridsToMerge, long inventoryEntityId, bool multiBlock, bool instantBuild, [Nullable] List<ulong> clientDLCIDs = null)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.PasteBlocksToGridServer_Implementation, gridsToMerge, inventoryEntityId, multiBlock, instantBuild, clientDLCIDs);
		}

<<<<<<< HEAD
		[Event(null, 9626)]
=======
		[Event(null, 9453)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private void PasteBlocksToGridServer_Implementation(List<MyObjectBuilder_CubeGrid> gridsToMerge, long inventoryEntityId, bool multiBlock, bool instantBuild, [Nullable] List<ulong> clientDLCIDs)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			bool flag = MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value);
			MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
			MySessionComponentGameInventory component2 = MySession.Static.GetComponent<MySessionComponentGameInventory>();
			MyEntities.RemapObjectBuilderCollection(gridsToMerge);
			EndpointId sender = MyEventContext.Current.Sender;
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = gridsToMerge[0];
			int num = 0;
			while (num < gridsToMerge[0].CubeBlocks.Count)
			{
				MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = myObjectBuilder_CubeGrid.CubeBlocks[num];
				myObjectBuilder_CubeBlock.SkinSubtypeId = component2.ValidateArmor(MyStringHash.GetOrCompute(myObjectBuilder_CubeBlock.SkinSubtypeId), sender.Value).String;
				MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(new MyDefinitionId(myObjectBuilder_CubeBlock.TypeId, myObjectBuilder_CubeBlock.SubtypeId));
				bool num2 = component.HasDefinitionDLC(new MyDefinitionId(myObjectBuilder_CubeBlock.TypeId, myObjectBuilder_CubeBlock.SubtypeId), MyEventContext.Current.Sender.Value);
				bool flag2 = component.ContainsRequiredDLC(definition, clientDLCIDs);
				if (!(num2 && flag2))
				{
					myObjectBuilder_CubeGrid.CubeBlocks.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
			if (myObjectBuilder_CubeGrid.CubeBlocks.Count == 0)
			{
				if (MyEventContext.Current.IsLocallyInvoked)
				{
					ShowMessageGridsRemovedWhilePastingInternal();
					return;
				}
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowMessageGridsRemovedWhilePasting, MyEventContext.Current.Sender);
				return;
			}
			MatrixI arg = PasteBlocksServer(gridsToMerge);
<<<<<<< HEAD
			if (!(flag && instantBuild) && MyEntities.TryGetEntityById(inventoryEntityId, out var entity) && entity != null)
=======
			if (!(num && instantBuild) && MyEntities.TryGetEntityById(inventoryEntityId, out var entity) && entity != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyInventoryBase builderInventory = MyCubeBuilder.BuildComponent.GetBuilderInventory(entity);
				if (builderInventory != null)
				{
					if (multiBlock)
					{
						MyMultiBlockClipboard.TakeMaterialsFromBuilder(gridsToMerge, entity);
					}
					else
					{
						MyGridClipboard.CalculateItemRequirements(gridsToMerge, m_buildComponents);
						foreach (KeyValuePair<MyDefinitionId, int> totalMaterial in m_buildComponents.TotalMaterials)
						{
							builderInventory.RemoveItemsOfType(totalMaterial.Value, totalMaterial.Key);
						}
					}
				}
			}
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.PasteBlocksToGridClient_Implementation, gridsToMerge[0], arg);
			MyMultiplayer.GetReplicationServer()?.ResendMissingReplicableChildren(this);
		}

<<<<<<< HEAD
		[Event(null, 9700)]
=======
		[Event(null, 9497)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void PasteBlocksToGridClient_Implementation(MyObjectBuilder_CubeGrid gridToMerge, MatrixI mergeTransform)
		{
			PasteBlocksClient(gridToMerge, mergeTransform);
		}

		private void PasteBlocksClient(MyObjectBuilder_CubeGrid gridToMerge, MatrixI mergeTransform)
		{
			MyCubeGrid myCubeGrid = MyEntities.CreateFromObjectBuilder(gridToMerge, fadeIn: false) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyEntities.Add(myCubeGrid);
				MergeGridInternal(myCubeGrid, ref mergeTransform);
			}
		}

		private MatrixI PasteBlocksServer(List<MyObjectBuilder_CubeGrid> gridsToMerge)
		{
			MyCubeGrid myCubeGrid = null;
			foreach (MyObjectBuilder_CubeGrid item in gridsToMerge)
			{
				MyCubeGrid myCubeGrid2 = MyEntities.CreateFromObjectBuilder(item, fadeIn: false) as MyCubeGrid;
				if (myCubeGrid2 != null)
				{
					if (myCubeGrid == null)
					{
						myCubeGrid = myCubeGrid2;
					}
					MyEntities.Add(myCubeGrid2);
				}
			}
			if (myCubeGrid != null)
			{
				MatrixI transform = CalculateMergeTransform(myCubeGrid, WorldToGridInteger(myCubeGrid.PositionComp.GetPosition()));
				MergeGridInternal(myCubeGrid, ref transform, disableBlockGenerators: false);
				return transform;
			}
			return default(MatrixI);
		}

		public static bool CanPasteGrid()
		{
			return MySession.Static.IsCopyPastingEnabled;
		}

		/// <summary>
		/// Returns biggest grid in physical group by AABB volume
		/// </summary>
		public MyCubeGrid GetBiggestGridInGroup()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			MyCubeGrid result = this;
			double num = 0.0;
			Enumerator<MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> enumerator = MyCubeGridGroups.Static.Physical.GetGroup(this).Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node current = enumerator.get_Current();
					double volume = current.NodeData.PositionComp.WorldAABB.Size.Volume;
					if (volume > num)
					{
						num = volume;
						result = current.NodeData;
					}
				}
				return result;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void ConvertFracturedBlocksToComponents()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			List<MyFracturedBlock> list = new List<MyFracturedBlock>();
<<<<<<< HEAD
			foreach (MySlimBlock cubeBlock in m_cubeBlocks)
			{
				MyFracturedBlock myFracturedBlock = cubeBlock.FatBlock as MyFracturedBlock;
				if (myFracturedBlock != null)
				{
					list.Add(myFracturedBlock);
				}
			}
			foreach (MyFracturedBlock item in list)
			{
				MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = item.ConvertToOriginalBlocksWithFractureComponent();
				RemoveBlockInternal(item.SlimBlock, close: true, markDirtyDisconnects: false);
				if (myObjectBuilder_CubeBlock != null)
				{
					AddBlock(myObjectBuilder_CubeBlock, testMerge: false);
				}
			}
=======
			Enumerator<MySlimBlock> enumerator = m_cubeBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyFracturedBlock myFracturedBlock = enumerator.get_Current().FatBlock as MyFracturedBlock;
					if (myFracturedBlock != null)
					{
						list.Add(myFracturedBlock);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			foreach (MyFracturedBlock item in list)
			{
				MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = item.ConvertToOriginalBlocksWithFractureComponent();
				RemoveBlockInternal(item.SlimBlock, close: true, markDirtyDisconnects: false);
				if (myObjectBuilder_CubeBlock != null)
				{
					AddBlock(myObjectBuilder_CubeBlock, testMerge: false);
				}
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void PrepareMultiBlockInfos()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = GetBlocks().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					AddMultiBlockInfo(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		internal void AddMultiBlockInfo(MySlimBlock block)
		{
			MyCompoundCubeBlock myCompoundCubeBlock = block.FatBlock as MyCompoundCubeBlock;
			if (myCompoundCubeBlock != null)
			{
				foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
				{
					if (block2.IsMultiBlockPart)
					{
						AddMultiBlockInfo(block2);
					}
				}
			}
			else if (block.IsMultiBlockPart)
			{
				if (m_multiBlockInfos == null)
				{
					m_multiBlockInfos = new Dictionary<int, MyCubeGridMultiBlockInfo>();
				}
				if (!m_multiBlockInfos.TryGetValue(block.MultiBlockId, out var value))
				{
					value = new MyCubeGridMultiBlockInfo();
					value.MultiBlockId = block.MultiBlockId;
					value.MultiBlockDefinition = block.MultiBlockDefinition;
					value.MainBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinitionForMultiBlock(block.MultiBlockDefinition.Id.SubtypeName);
					m_multiBlockInfos.Add(block.MultiBlockId, value);
				}
				value.Blocks.Add(block);
			}
		}

		internal void RemoveMultiBlockInfo(MySlimBlock block)
		{
			if (m_multiBlockInfos == null)
			{
				return;
			}
			MyCompoundCubeBlock myCompoundCubeBlock = block.FatBlock as MyCompoundCubeBlock;
			MyCubeGridMultiBlockInfo value;
			if (myCompoundCubeBlock != null)
			{
				foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
				{
					if (block2.IsMultiBlockPart)
					{
						RemoveMultiBlockInfo(block2);
					}
				}
			}
<<<<<<< HEAD
			else if (block.IsMultiBlockPart && m_multiBlockInfos.TryGetValue(block.MultiBlockId, out value) && value.Blocks.Remove(block) && value.Blocks.Count == 0 && m_multiBlockInfos.Remove(block.MultiBlockId) && m_multiBlockInfos.Count == 0)
=======
			else if (block.IsMultiBlockPart && m_multiBlockInfos.TryGetValue(block.MultiBlockId, out value) && value.Blocks.Remove(block) && value.Blocks.get_Count() == 0 && m_multiBlockInfos.Remove(block.MultiBlockId) && m_multiBlockInfos.Count == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_multiBlockInfos = null;
			}
		}

		public MyCubeGridMultiBlockInfo GetMultiBlockInfo(int multiBlockId)
		{
			if (m_multiBlockInfos != null && m_multiBlockInfos.TryGetValue(multiBlockId, out var value))
			{
				return value;
			}
			return null;
		}

		/// <summary>
		/// Writes multiblocks (compound block and block ID) to outMultiBlocks collection with the same multiblockId.
		/// </summary>
		public void GetBlocksInMultiBlock(int multiBlockId, HashSet<Tuple<MySlimBlock, ushort?>> outMultiBlocks)
		{
<<<<<<< HEAD
=======
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (multiBlockId == 0)
			{
				return;
			}
			MyCubeGridMultiBlockInfo multiBlockInfo = GetMultiBlockInfo(multiBlockId);
			if (multiBlockInfo == null)
			{
				return;
			}
<<<<<<< HEAD
			foreach (MySlimBlock block in multiBlockInfo.Blocks)
			{
				MySlimBlock cubeBlock = GetCubeBlock(block.Position);
				MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock != null)
				{
					ushort? blockId = myCompoundCubeBlock.GetBlockId(block);
					outMultiBlocks.Add(new Tuple<MySlimBlock, ushort?>(cubeBlock, blockId));
				}
				else
				{
					outMultiBlocks.Add(new Tuple<MySlimBlock, ushort?>(cubeBlock, null));
=======
			Enumerator<MySlimBlock> enumerator = multiBlockInfo.Blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					MySlimBlock cubeBlock = GetCubeBlock(current.Position);
					MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
					if (myCompoundCubeBlock != null)
					{
						ushort? blockId = myCompoundCubeBlock.GetBlockId(current);
						outMultiBlocks.Add(new Tuple<MySlimBlock, ushort?>(cubeBlock, blockId));
					}
					else
					{
						outMultiBlocks.Add(new Tuple<MySlimBlock, ushort?>(cubeBlock, null));
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public bool CanAddMultiBlocks(MyCubeGridMultiBlockInfo multiBlockInfo, ref MatrixI transform, List<int> multiBlockIndices)
		{
			foreach (int multiBlockIndex in multiBlockIndices)
			{
				if (multiBlockIndex < multiBlockInfo.MultiBlockDefinition.BlockDefinitions.Length)
				{
					MyMultiBlockDefinition.MyMultiBlockPartDefinition myMultiBlockPartDefinition = multiBlockInfo.MultiBlockDefinition.BlockDefinitions[multiBlockIndex];
					if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(myMultiBlockPartDefinition.Id, out var blockDefinition) || blockDefinition == null)
					{
						return false;
					}
					Vector3I vector3I = Vector3I.Transform(myMultiBlockPartDefinition.Min, ref transform);
					MatrixI leftMatrix = new MatrixI(myMultiBlockPartDefinition.Forward, myMultiBlockPartDefinition.Up);
					MatrixI.Multiply(ref leftMatrix, ref transform, out var result);
					MyBlockOrientation blockOrientation = result.GetBlockOrientation();
					if (!CanPlaceBlock(vector3I, vector3I, blockOrientation, blockDefinition, 0uL, multiBlockInfo.MultiBlockId, ignoreFracturedPieces: true))
					{
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Builds multiblock parts according to specified indices. 
		/// </summary>
		public bool BuildMultiBlocks(MyCubeGridMultiBlockInfo multiBlockInfo, ref MatrixI transform, List<int> multiBlockIndices, long builderEntityId, MyStringHash skinId)
		{
			List<MyBlockLocation> list = new List<MyBlockLocation>();
			List<MyObjectBuilder_CubeBlock> list2 = new List<MyObjectBuilder_CubeBlock>();
			foreach (int multiBlockIndex in multiBlockIndices)
			{
				if (multiBlockIndex < multiBlockInfo.MultiBlockDefinition.BlockDefinitions.Length)
				{
					MyMultiBlockDefinition.MyMultiBlockPartDefinition myMultiBlockPartDefinition = multiBlockInfo.MultiBlockDefinition.BlockDefinitions[multiBlockIndex];
					if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(myMultiBlockPartDefinition.Id, out var blockDefinition) || blockDefinition == null)
					{
						return false;
					}
					Vector3I vector3I = Vector3I.Transform(myMultiBlockPartDefinition.Min, ref transform);
					MatrixI leftMatrix = new MatrixI(myMultiBlockPartDefinition.Forward, myMultiBlockPartDefinition.Up);
					MatrixI.Multiply(ref leftMatrix, ref transform, out var result);
					MyBlockOrientation blockOrientation = result.GetBlockOrientation();
					if (!CanPlaceBlock(vector3I, vector3I, blockOrientation, blockDefinition, 0uL, multiBlockInfo.MultiBlockId))
					{
						return false;
					}
					MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = MyObjectBuilderSerializer.CreateNewObject(myMultiBlockPartDefinition.Id) as MyObjectBuilder_CubeBlock;
					myObjectBuilder_CubeBlock.Orientation = Base6Directions.GetOrientation(blockOrientation.Forward, blockOrientation.Up);
					myObjectBuilder_CubeBlock.Min = vector3I;
					myObjectBuilder_CubeBlock.ColorMaskHSV = MyPlayer.SelectedColor;
					myObjectBuilder_CubeBlock.SkinSubtypeId = MyPlayer.SelectedArmorSkin;
					myObjectBuilder_CubeBlock.MultiBlockId = multiBlockInfo.MultiBlockId;
					myObjectBuilder_CubeBlock.MultiBlockIndex = multiBlockIndex;
					myObjectBuilder_CubeBlock.MultiBlockDefinition = multiBlockInfo.MultiBlockDefinition.Id;
					list2.Add(myObjectBuilder_CubeBlock);
					MyBlockLocation item = default(MyBlockLocation);
					item.Min = vector3I;
					item.Max = vector3I;
					item.CenterPos = vector3I;
					item.Orientation = new MyBlockOrientation(blockOrientation.Forward, blockOrientation.Up);
					item.BlockDefinition = myMultiBlockPartDefinition.Id;
					item.EntityId = MyEntityIdentifier.AllocateId();
					item.Owner = builderEntityId;
					list.Add(item);
				}
			}
			if (MySession.Static.SurvivalMode)
			{
				MyEntity entityById = MyEntities.GetEntityById(builderEntityId);
				if (entityById == null)
				{
					return false;
				}
				HashSet<MyBlockLocation> hashSet = new HashSet<MyBlockLocation>((IEnumerable<MyBlockLocation>)list);
				MyCubeBuilder.BuildComponent.GetBlocksPlacementMaterials(hashSet, this);
				if (!MyCubeBuilder.BuildComponent.HasBuildingMaterials(entityById))
				{
					return false;
				}
			}
			MyBlockVisuals arg = new MyBlockVisuals(MyPlayer.SelectedColor.PackHSVToUint(), skinId);
			for (int i = 0; i < list.Count && i < list2.Count; i++)
			{
				MyBlockLocation arg2 = list[i];
				MyObjectBuilder_CubeBlock arg3 = list2[i];
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BuildBlockRequest, arg, arg2, arg3, builderEntityId, arg6: false, MySession.Static.LocalPlayerId);
			}
			return true;
		}

		private bool GetMissingBlocksMultiBlock(int multiblockId, out MyCubeGridMultiBlockInfo multiBlockInfo, out MatrixI transform, List<int> multiBlockIndices)
		{
			transform = default(MatrixI);
			multiBlockInfo = GetMultiBlockInfo(multiblockId);
			if (multiBlockInfo == null)
			{
				return false;
			}
			return multiBlockInfo.GetMissingBlocks(out transform, multiBlockIndices);
		}

		public bool CanAddMissingBlocksInMultiBlock(int multiBlockId)
		{
			try
			{
				if (!GetMissingBlocksMultiBlock(multiBlockId, out var multiBlockInfo, out var transform, m_tmpMultiBlockIndices))
				{
					return false;
				}
				return CanAddMultiBlocks(multiBlockInfo, ref transform, m_tmpMultiBlockIndices);
			}
			finally
			{
				m_tmpMultiBlockIndices.Clear();
			}
		}

		public void AddMissingBlocksInMultiBlock(int multiBlockId, long toolOwnerId)
		{
			try
			{
				if (GetMissingBlocksMultiBlock(multiBlockId, out var multiBlockInfo, out var transform, m_tmpMultiBlockIndices))
				{
					MyStringHash orCompute = MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin);
					BuildMultiBlocks(multiBlockInfo, ref transform, m_tmpMultiBlockIndices, toolOwnerId, orCompute);
				}
			}
			finally
			{
				m_tmpMultiBlockIndices.Clear();
			}
		}

		/// <summary>
		/// Checks if the given block can be added to place where multiblock area (note that even if some parts of multiblock are destroyed then they still 
		/// occupy - virtually - its place). 
		/// </summary>
		public bool CanAddOtherBlockInMultiBlock(Vector3I min, Vector3I max, MyBlockOrientation orientation, MyCubeBlockDefinition definition, int? ignoreMultiblockId)
		{
			if (m_multiBlockInfos == null)
			{
				return true;
			}
			foreach (KeyValuePair<int, MyCubeGridMultiBlockInfo> multiBlockInfo in m_multiBlockInfos)
			{
				if ((!ignoreMultiblockId.HasValue || ignoreMultiblockId.Value != multiBlockInfo.Key) && !multiBlockInfo.Value.CanAddBlock(ref min, ref max, orientation, definition))
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsGridInCompleteState(MyCubeGrid grid)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = grid.CubeBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					if (!current.IsFullIntegrity || current.BuildLevelRatio != 1f)
					{
						return false;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return true;
		}

		public bool WillRemoveBlockSplitGrid(MySlimBlock testBlock)
		{
			return m_disconnectHelper.TryDisconnect(testBlock);
		}

		/// <param name="position">In world coordinates</param>
		public MySlimBlock GetTargetedBlock(Vector3D position)
		{
<<<<<<< HEAD
			FixTargetCube(out var cube, (Vector3)Vector3D.Transform(position, base.PositionComp.WorldMatrixNormalizedInv) * GridSizeR);
=======
			FixTargetCube(out var cube, Vector3D.Transform(position, base.PositionComp.WorldMatrixNormalizedInv) * GridSizeR);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return GetCubeBlock(cube);
		}

		/// <param name="position">In world coordinates</param>
		public MySlimBlock GetTargetedBlockLite(Vector3D position)
		{
			FixTargetCubeLite(out var cube, Vector3D.Transform(position, base.PositionComp.WorldMatrixNormalizedInv) * GridSizeR);
			return GetCubeBlock(cube);
		}

<<<<<<< HEAD
		[Event(null, 10129)]
=======
		[Event(null, 9920)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public static void TryCreateGrid_Implementation(MyCubeSize cubeSize, bool isStatic, MyPositionAndOrientation position, long inventoryEntityId, bool instantBuild)
		{
			bool flag = MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value);
			MyDefinitionManager.Static.GetBaseBlockPrefabName(cubeSize, isStatic, MySession.Static.CreativeMode || (instantBuild && flag), out var prefabName);
			if (prefabName == null)
			{
				return;
			}
			MyObjectBuilder_CubeGrid[] gridPrefab = MyPrefabManager.Static.GetGridPrefab(prefabName);
			if (gridPrefab == null || gridPrefab.Length == 0)
			{
				return;
			}
			MyObjectBuilder_CubeGrid[] array = gridPrefab;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].PositionAndOrientation = position;
			}
			MyEntities.RemapObjectBuilderCollection(gridPrefab);
			if (!(instantBuild && flag))
			{
				if (MyEntities.TryGetEntityById(inventoryEntityId, out var entity) && entity != null)
				{
					MyInventoryBase builderInventory = MyCubeBuilder.BuildComponent.GetBuilderInventory(entity);
					if (builderInventory != null)
					{
						MyGridClipboard.CalculateItemRequirements(gridPrefab, m_buildComponents);
						foreach (KeyValuePair<MyDefinitionId, int> totalMaterial in m_buildComponents.TotalMaterials)
						{
							builderInventory.RemoveItemsOfType(totalMaterial.Value, totalMaterial.Key);
						}
					}
				}
				else if (!flag && !MySession.Static.CreativeMode)
				{
					(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
					return;
				}
			}
			List<MyCubeGrid> list = new List<MyCubeGrid>();
			array = gridPrefab;
			foreach (MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid in array)
			{
				MySandboxGame.Log.WriteLine("CreateCompressedMsg: Type: " + myObjectBuilder_CubeGrid.GetType().Name.ToString() + "  Name: " + myObjectBuilder_CubeGrid.Name + "  EntityID: " + myObjectBuilder_CubeGrid.EntityId.ToString("X8"));
				MyCubeGrid myCubeGrid = MyEntities.CreateFromObjectBuilder(myObjectBuilder_CubeGrid, fadeIn: false) as MyCubeGrid;
				if (myCubeGrid != null)
				{
					list.Add(myCubeGrid);
					if (instantBuild && flag)
					{
						ChangeOwnership(inventoryEntityId, myCubeGrid);
					}
					MySandboxGame.Log.WriteLine("Status: Exists(" + MyEntities.EntityExists(myObjectBuilder_CubeGrid.EntityId) + ") InScene(" + ((myObjectBuilder_CubeGrid.PersistentFlags & MyPersistentEntityFlags2.InScene) == MyPersistentEntityFlags2.InScene) + ")");
				}
			}
			AfterPaste(list, Vector3.Zero, detectDisconnects: false);
		}

		/// <summary>
		/// Use only for cut request
		/// </summary>
		public void SendGridCloseRequest()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnGridClosedRequest, base.EntityId);
		}

<<<<<<< HEAD
		[Event(null, 10205)]
=======
		[Event(null, 9996)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void StationClosingDenied()
		{
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MySpaceTexts.Economy_CantRemoveStation_Caption), messageText: MyTexts.Get(MySpaceTexts.Economy_CantRemoveStation_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: null, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
		}

<<<<<<< HEAD
		[Event(null, 10215)]
=======
		[Event(null, 10006)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void OnGridClosedRequest(long entityId)
		{
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component != null && component.IsGridStation(entityId))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => StationClosingDenied, MyEventContext.Current.Sender);
				return;
			}
			MyLog.Default.WriteLineAndConsole("Closing grid request by user: " + MyEventContext.Current.Sender);
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			MyEntities.TryGetEntityById(entityId, out var entity);
			if (entity == null)
			{
				return;
			}
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
				bool flag = false;
				bool flag2 = false;
				IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(num);
				if (myFaction != null)
				{
					flag2 = myFaction.IsLeader(num);
				}
				if (MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
				{
					flag = true;
				}
				else if (myCubeGrid.BigOwners.Count != 0)
				{
					foreach (long bigOwner in myCubeGrid.BigOwners)
					{
						if (bigOwner == num)
						{
							flag = true;
							break;
						}
						if (MySession.Static.Players.TryGetIdentity(bigOwner) != null && flag2)
						{
							IMyFaction myFaction2 = MySession.Static.Factions.TryGetPlayerFaction(bigOwner);
							if (myFaction2 != null && myFaction.FactionId == myFaction2.FactionId)
							{
								flag = true;
								break;
							}
						}
					}
				}
				else
				{
					flag = true;
				}
				if (!flag)
				{
					return;
				}
			}
			MyLog.Default.Info($"OnGridClosedRequest removed entity '{entity.Name}:{entity.DisplayName}' with entity id '{entity.EntityId}'");
			if (!entity.MarkedForClose)
			{
				entity.Close();
			}
		}

<<<<<<< HEAD
		[Event(null, 10541)]
=======
		[Event(null, 10332)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public static void TryPasteGrid_Implementation(MyPasteGridParameters parameters)
		{
			MyLog.Default.WriteLineAndConsole("Pasting grid request by user: " + MyEventContext.Current.Sender);
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsCopyPastingEnabledForUser(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			bool shouldRemoveScripts = !MySession.Static.IsUserScripter(MyEventContext.Current.Sender.Value);
			Vector3D? offset = null;
			if (parameters.Offset.Use && parameters.Offset.RelativeToEntity)
			{
				if (!MyEntityIdentifier.TryGetEntity(parameters.Offset.SpawnerId, out var entity))
				{
					return;
				}
				offset = (entity as MyEntity).WorldMatrix.Translation - parameters.Offset.OriginalSpawnPoint;
			}
			MyEntities.RemapObjectBuilderCollection(parameters.Entities);
			CleanCubeGridsBeforePaste(parameters.Entities);
			PasteGridData workData = new PasteGridData(parameters.Entities, parameters.DetectDisconnects, parameters.ObjectVelocity, parameters.MultiBlock, parameters.InstantBuild, shouldRemoveScripts, MyEventContext.Current.Sender, MyEventContext.Current.IsLocallyInvoked, offset, parameters.ClientsideDLCs);
			if (MySandboxGame.Config.SyncRendering)
			{
				MyEntityIdentifier.PrepareSwapData();
				MyEntityIdentifier.SwapPerThreadData();
			}
			Parallel.Start(TryPasteGrid_ImplementationInternal, OnPasteCompleted, workData);
			if (MySandboxGame.Config.SyncRendering)
			{
				MyEntityIdentifier.ClearSwapDataAndRestore();
			}
		}

		internal static void CleanCubeGridsBeforePaste(List<MyObjectBuilder_CubeGrid> grids)
		{
			foreach (MyObjectBuilder_CubeGrid grid in grids)
			{
				grid.SetupForGridPaste();
				foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
				{
					cubeBlock.SetupForGridPaste();
				}
			}
		}

		internal static void CleanCubeGridsBeforeSetupForProjector(List<MyObjectBuilder_CubeGrid> grids)
		{
			foreach (MyObjectBuilder_CubeGrid grid in grids)
			{
				grid.SetupForProjector();
				foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
				{
					cubeBlock.SetupForProjector();
				}
			}
		}

		private static void TryPasteGrid_ImplementationInternal(WorkData workData)
		{
			PasteGridData pasteGridData = workData as PasteGridData;
			if (pasteGridData == null)
			{
				workData.FlagAsFailed();
			}
			else
			{
				pasteGridData.TryPasteGrid();
			}
		}

		private static void OnPasteCompleted(WorkData workData)
		{
			PasteGridData pasteGridData = workData as PasteGridData;
			if (pasteGridData == null)
			{
				workData.FlagAsFailed();
			}
			else
			{
				pasteGridData.Callback();
			}
		}

<<<<<<< HEAD
		[Event(null, 10632)]
=======
		[Event(null, 10423)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		public static void ShowPasteFailedOperation()
		{
			MyHud.Notifications.Add(MyNotificationSingletons.PasteFailed);
		}

<<<<<<< HEAD
		[Event(null, 10638)]
=======
		[Event(null, 10429)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		public static void SendHudNotificationAfterPaste()
		{
			MyHud.PopRotatingWheelVisible();
		}

		internal static void RelocateGrids(List<MyObjectBuilder_CubeGrid> cubegrids, MatrixD worldMatrix0)
		{
			MatrixD m = cubegrids[0].PositionAndOrientation.Value.GetMatrix();
			MatrixD m2 = Matrix.Invert(m) * worldMatrix0.GetOrientation();
			Matrix matrix = m2;
			for (int i = 0; i < cubegrids.Count; i++)
			{
				if (cubegrids[i].PositionAndOrientation.HasValue)
				{
					MatrixD matrix2 = cubegrids[i].PositionAndOrientation.Value.GetMatrix();
					Vector3 vector = Vector3.TransformNormal(matrix2.Translation - m.Translation, matrix);
					matrix2 *= matrix;
					Vector3D translation = worldMatrix0.Translation + vector;
					matrix2.Translation = Vector3D.Zero;
					matrix2 = MatrixD.Orthogonalize(matrix2);
					matrix2.Translation = translation;
					cubegrids[i].PositionAndOrientation = new MyPositionAndOrientation(ref matrix2);
				}
			}
		}

		private static void ChangeOwnership(long inventoryEntityId, MyCubeGrid grid)
		{
			if (MyEntities.TryGetEntityById(inventoryEntityId, out var entity) && entity != null)
			{
				MyCharacter myCharacter = entity as MyCharacter;
				if (myCharacter != null)
				{
					grid.ChangeGridOwner(myCharacter.ControllerInfo.Controller.Player.Identity.IdentityId, MyOwnershipShareModeEnum.Faction);
				}
			}
		}

		private static void AfterPaste(List<MyCubeGrid> grids, Vector3 objectVelocity, bool detectDisconnects)
		{
<<<<<<< HEAD
			float num = Math.Min(MyGridPhysics.SmallShipMaxLinearVelocity(), MyGridPhysics.LargeShipMaxLinearVelocity()) - 10f;
			if (objectVelocity.LengthSquared() > num * num)
			{
				objectVelocity.Normalize();
				objectVelocity *= num;
			}
=======
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (MyCubeGrid grid in grids)
			{
				if (grid.IsStatic)
				{
					grid.TestDynamic = MyTestDynamicReason.GridCopied;
				}
				MyEntities.Add(grid);
				if (grid.Physics != null)
				{
					if (!grid.IsStatic)
					{
						grid.Physics.LinearVelocity = objectVelocity;
					}
					if (!grid.IsStatic && MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity.Physics != null && MySession.Static.ControlledEntity != null)
					{
						grid.Physics.AngularVelocity = MySession.Static.ControlledEntity.Entity.Physics.AngularVelocity;
					}
				}
				if (detectDisconnects)
				{
					grid.DetectDisconnectsAfterFrame();
				}
				if (grid.IsStatic)
				{
					Enumerator<MySlimBlock> enumerator2 = grid.CubeBlocks.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							MySlimBlock current2 = enumerator2.get_Current();
							if (grid.DetectMerge(current2, null, null, newGrid: true) != null)
							{
								break;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
				if (MyVisualScriptLogicProvider.GridSpawned != null)
				{
					MyVisualScriptLogicProvider.GridSpawned(grid.Name);
				}
				if (MyVisualScriptLogicProvider.GridSpawned != null)
				{
					MyVisualScriptLogicProvider.GridSpawned(grid.Name);
				}
			}
			MatrixD tranform = grids[0].PositionComp.WorldMatrixRef;
			bool flag = MyCoordinateSystem.Static.IsLocalCoordSysExist(ref tranform, grids[0].GridSize);
			if (grids[0].GridSizeEnum == MyCubeSize.Large)
			{
				if (flag)
				{
					MyCoordinateSystem.Static.RegisterCubeGrid(grids[0]);
				}
				else
				{
					MyCoordinateSystem.Static.CreateCoordSys(grids[0], MyClipboardComponent.ClipboardDefinition.PastingSettings.StaticGridAlignToCenter, sync: true);
				}
			}
		}

		public void RecalculateGravity()
		{
			Vector3D worldPoint = ((Physics == null || !(Physics.RigidBody != null)) ? base.PositionComp.GetPosition() : Physics.CenterOfMassWorld);
			m_gravity = MyGravityProviderSystem.CalculateNaturalGravityInPoint(worldPoint);
		}

		public void ActivatePhysics()
		{
			if (MyEntities.IsAsyncUpdateInProgress)
			{
				MyEntities.InvokeLater(ActivatePhysics);
			}
			else if (Physics != null && Physics.Enabled)
			{
				Physics.RigidBody.Activate();
				if (Physics.RigidBody2 != null)
				{
					Physics.RigidBody2.Activate();
				}
			}
		}

<<<<<<< HEAD
		[Event(null, 10787)]
=======
		[Event(null, 10570)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void OnBonesReceived(int segmentsCount, List<byte> boneByteList)
		{
			byte[] data = boneByteList.ToArray();
			int dataIndex = 0;
			Vector3I cubeDirty = default(Vector3I);
			for (int i = 0; i < segmentsCount; i++)
			{
				Skeleton.DeserializePart(GridSize, data, ref dataIndex, out var minBone, out var maxBone);
				Vector3I cube = Vector3I.Zero;
				Vector3I cube2 = Vector3I.Zero;
				Skeleton.Wrap(ref cube, ref minBone);
				Skeleton.Wrap(ref cube2, ref maxBone);
				cube -= Vector3I.One;
				cube2 += Vector3I.One;
				cubeDirty.X = cube.X;
				while (cubeDirty.X <= cube2.X)
				{
					cubeDirty.Y = cube.Y;
					while (cubeDirty.Y <= cube2.Y)
					{
						cubeDirty.Z = cube.Z;
						while (cubeDirty.Z <= cube2.Z)
						{
							SetCubeDirty(cubeDirty);
							cubeDirty.Z++;
						}
						cubeDirty.Y++;
					}
					cubeDirty.X++;
				}
			}
		}

<<<<<<< HEAD
		[Event(null, 10821)]
=======
		[Event(null, 10604)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void OnBonesMultiplied(Vector3I blockLocation, float multiplier)
		{
			MySlimBlock cubeBlock = GetCubeBlock(blockLocation);
			if (cubeBlock != null)
			{
				MultiplyBlockSkeleton(cubeBlock, multiplier);
			}
		}

		public void SendReflectorState(MyMultipleEnabledEnum value)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RelfectorStateRecived, value);
		}

<<<<<<< HEAD
		[Event(null, 10837)]
=======
		[Event(null, 10620)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Controlled)]
		[Broadcast]
		private void RelfectorStateRecived(MyMultipleEnabledEnum value)
		{
			GridSystems.ReflectorLightSystem.ReflectorStateChanged(value);
		}

		public void SendIntegrityChanged(MySlimBlock mySlimBlock, MyIntegrityChangeEnum integrityChangeType, long toolOwner)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BlockIntegrityChanged, mySlimBlock.Position, GetSubBlockId(mySlimBlock), mySlimBlock.BuildIntegrity, mySlimBlock.Integrity, integrityChangeType, toolOwner);
		}

		public void SendStockpileChanged(MySlimBlock mySlimBlock, List<MyStockpileItem> list)
		{
			if (list.Count > 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BlockStockpileChanged, mySlimBlock.Position, GetSubBlockId(mySlimBlock), list);
			}
		}

		public void SendFractureComponentRepaired(MySlimBlock mySlimBlock, long toolOwner)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.FractureComponentRepaired, mySlimBlock.Position, GetSubBlockId(mySlimBlock), toolOwner);
		}

		private ushort GetSubBlockId(MySlimBlock slimBlock)
		{
			MySlimBlock cubeBlock = slimBlock.CubeGrid.GetCubeBlock(slimBlock.Position);
			MyCompoundCubeBlock myCompoundCubeBlock = null;
			if (cubeBlock != null)
			{
				myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
			}
			if (myCompoundCubeBlock != null)
			{
				return myCompoundCubeBlock.GetBlockId(slimBlock) ?? 0;
			}
			return 0;
		}

		public void RequestFillStockpile(Vector3I blockPosition, MyInventory fromInventory)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnStockpileFillRequest, blockPosition, fromInventory.Owner.EntityId, fromInventory.InventoryIdx);
		}

<<<<<<< HEAD
		[Event(null, 10881)]
=======
		[Event(null, 10664)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access)]
		private void OnStockpileFillRequest(Vector3I blockPosition, long ownerEntityId, byte inventoryIndex)
		{
			MySlimBlock cubeBlock = GetCubeBlock(blockPosition);
			if (cubeBlock != null)
			{
				MyEntity entity = null;
				if (MyEntities.TryGetEntityById(ownerEntityId, out entity))
				{
					MyInventory inventory = ((entity != null && entity.HasInventory) ? entity : null).GetInventory(inventoryIndex);
					cubeBlock.MoveItemsToConstructionStockpile(inventory);
				}
			}
		}

		public void RequestSetToConstruction(Vector3I blockPosition, MyInventory fromInventory)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnSetToConstructionRequest, blockPosition, fromInventory.Owner.EntityId, fromInventory.InventoryIdx, MySession.Static.LocalPlayerId);
		}

<<<<<<< HEAD
		[Event(null, 10909)]
=======
		[Event(null, 10692)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access)]
		private void OnSetToConstructionRequest(Vector3I blockPosition, long ownerEntityId, byte inventoryIndex, long requestingPlayer)
		{
			MySlimBlock cubeBlock = GetCubeBlock(blockPosition);
			if (cubeBlock != null)
			{
				cubeBlock.SetToConstructionSite();
				MyEntity entity = null;
				if (MyEntities.TryGetEntityById(ownerEntityId, out entity))
				{
					MyInventoryBase inventory = ((entity != null && entity.HasInventory) ? entity : null).GetInventory(inventoryIndex);
					cubeBlock.MoveItemsToConstructionStockpile(inventory);
					cubeBlock.IncreaseMountLevel(MyWelder.WELDER_AMOUNT_PER_SECOND * 0.0166666675f, requestingPlayer);
				}
			}
		}

		public void ChangePowerProducerState(MyMultipleEnabledEnum enabledState, long playerId, bool localGridOnly = false)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnPowerProducerStateRequest, enabledState, playerId, localGridOnly);
		}

<<<<<<< HEAD
		[Event(null, 10940)]
=======
		[Event(null, 10723)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private void OnPowerProducerStateRequest(MyMultipleEnabledEnum enabledState, long playerId, bool localGridOnly = false)
		{
<<<<<<< HEAD
			GridSystems.SyncObject_PowerProducerStateChanged(enabledState, playerId, localGridOnly);
			if (!localGridOnly && (enabledState == MyMultipleEnabledEnum.AllDisabled || enabledState == MyMultipleEnabledEnum.AllEnabled))
			{
				m_IsPowered.Value = enabledState == MyMultipleEnabledEnum.AllEnabled;
=======
			GridSystems.SyncObject_PowerProducerStateChanged(enabledState, playerId);
			if (enabledState == MyMultipleEnabledEnum.AllDisabled || enabledState == MyMultipleEnabledEnum.AllEnabled)
			{
				m_IsPowered = enabledState == MyMultipleEnabledEnum.AllEnabled;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void RequestConversionToShip(Action result)
		{
			m_convertToShipResult = result;
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnConvertedToShipRequest, MyTestDynamicReason.ConvertToShip);
		}

		public void RequestConversionToStation()
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnConvertedToStationRequest);
		}

<<<<<<< HEAD
		[Event(null, 10960)]
=======
		[Event(null, 10743)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.BigOwnerSpaceMaster)]
		private void OnConvertedToShipRequest(MyTestDynamicReason reason)
		{
			if (!IsStatic || Physics == null || BlocksCount == 0 || ShouldBeStatic(this, reason, reason != MyTestDynamicReason.ConvertToShip))
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnConvertToShipFailed, MyEventContext.Current.Sender);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnConvertToDynamic);
			}
		}

<<<<<<< HEAD
		[Event(null, 10973)]
=======
		[Event(null, 10756)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private void OnConvertToShipFailed()
		{
			if (m_convertToShipResult != null)
			{
				m_convertToShipResult();
			}
			m_convertToShipResult = null;
		}

<<<<<<< HEAD
		[Event(null, 10981)]
=======
		[Event(null, 10764)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.BigOwnerSpaceMaster)]
		public void OnConvertedToStationRequest()
		{
			if (!IsStatic && MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.ConvertToStation, 0L, MyEventContext.Current.Sender.Value))
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.ConvertToStatic);
			}
		}

		public void ChangeOwnerRequest(MyCubeGrid grid, MyCubeBlock block, long playerId, MyOwnershipShareModeEnum shareMode)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnChangeOwnerRequest, block.EntityId, playerId, shareMode);
		}

<<<<<<< HEAD
		[Event(null, 11000)]
=======
		[Event(null, 10783)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access)]
		private void OnChangeOwnerRequest(long blockId, long owner, MyOwnershipShareModeEnum shareMode)
		{
			MyCubeBlock entity = null;
			if (!MyEntities.TryGetEntityById(blockId, out entity, allowClosed: false))
<<<<<<< HEAD
			{
				return;
			}
			if (Sandbox.Engine.Platform.Game.IsDedicated && MyEventContext.Current.Sender.Value != Sync.ServerId)
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
				MyLog.Default.WriteLine($"User {num} is trying to change ownership.");
				return;
			}
			MyEntityOwnershipComponent myEntityOwnershipComponent = entity.Components.Get<MyEntityOwnershipComponent>();
			if (Sync.IsServer && entity.IDModule != null && (entity.IDModule.Owner == 0L || entity.IDModule.Owner == owner || owner == 0L))
			{
				OnChangeOwner(blockId, owner, shareMode);
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnChangeOwner, blockId, owner, shareMode);
				return;
			}
			if (Sync.IsServer && myEntityOwnershipComponent != null && (myEntityOwnershipComponent.OwnerId == 0L || myEntityOwnershipComponent.OwnerId == owner || owner == 0L))
			{
				OnChangeOwner(blockId, owner, shareMode);
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnChangeOwner, blockId, owner, shareMode);
				return;
			}
			bool flag = entity.BlockDefinition.ContainsComputer();
			if (entity.UseObjectsComponent != null)
			{
				flag = flag || entity.UseObjectsComponent.GetDetectors("ownership").Count > 0;
			}
		}

<<<<<<< HEAD
		[Event(null, 11037)]
=======
		[Event(null, 10812)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void OnChangeOwner(long blockId, long owner, MyOwnershipShareModeEnum shareMode)
		{
			MyCubeBlock entity = null;
			if (MyEntities.TryGetEntityById(blockId, out entity, allowClosed: false))
			{
				entity.ChangeOwner(owner, shareMode);
			}
		}

		private void HandBrakeChanged()
		{
			GridSystems.WheelSystem.HandBrake = m_handBrakeSync;
		}

<<<<<<< HEAD
		private void SetHandbrakeInternal(bool v)
		{
			m_handBrakeSync.Value = v;
			GridSystems.WheelSystem.HandBrake = v;
		}

		private void SetParkedStateInternal(bool v)
=======
		[Event(null, 10827)]
		[Reliable]
		[Server]
		public void SetHandbrakeRequest(bool v)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			m_handBrakeSync.Value = v;
			GridSystems.WheelSystem.HandBrake = v;
			GridSystems.LandingSystem.Switch(v, forceSwitch: false);
			GridSystems.ConveyorSystem.ToggleConnectors(v, forceToggle: false);
		}

		[Event(null, 11068)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Controlled)]
		public void ToggleHandbrakeRequest()
		{
			bool isParked = GridSystems.WheelSystem.IsParked;
			MyCockpit myCockpit;
			bool num = GridSystems.ControlSystem != null && (GridSystems.ControlSystem.IsLocallyControlled || ((myCockpit = GridSystems.ControlSystem.GetShipController() as MyCockpit) != null && myCockpit?.Pilot?.GetClientIdentity().SteamId == MyEventContext.Current.Sender.Value));
			SetHandbrakeInternal(!isParked);
			bool isParked2 = GridSystems.WheelSystem.IsParked;
			HandbrakeToggleResult arg = (isParked ? HandbrakeToggleResult.RELEASED : (isParked2 ? HandbrakeToggleResult.ENGAGED_SUCCESSFULLY : HandbrakeToggleResult.FAILED_TO_ENGAGE));
			if (num)
			{
				string arg2 = GridSystems.ConveyorSystem.HudMessageCustom ?? string.Empty;
				GridSystems.ConveyorSystem.HudMessageCustom = string.Empty;
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.ReceiveHandbrakeRequestResult, arg, arg2, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 11089)]
		[Reliable]
		[Client]
		public void ReceiveHandbrakeRequestResult(HandbrakeToggleResult result, string message)
		{
			MyStringId myStringId = MyStringId.NullOrEmpty;
			switch (result)
			{
			case HandbrakeToggleResult.FAILED_TO_ENGAGE:
				myStringId = MySpaceTexts.NotificationHandbrakeFailedToEngage;
				break;
			case HandbrakeToggleResult.RELEASED:
				myStringId = MySpaceTexts.NotificationHandbrakeOff;
				break;
			case HandbrakeToggleResult.ENGAGED_SUCCESSFULLY:
				myStringId = MySpaceTexts.NotificationHandbrakeOn;
				break;
			}
			if (myStringId != MyStringId.NullOrEmpty)
			{
				MyHud.Notifications.Add(new MyHudNotification(myStringId));
			}
			if (!string.IsNullOrEmpty(message))
			{
				MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.Format_OneParameter);
				myHudNotification.SetTextFormatArguments(message);
				MyHud.Notifications.Add(myHudNotification);
			}
		}

		[Event(null, 11122)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Controlled)]
		public void ToggleParkStateRequest()
		{
			bool isParked = IsParked;
			MyCockpit myCockpit;
			bool num = GridSystems.ControlSystem != null && (GridSystems.ControlSystem.IsLocallyControlled || ((myCockpit = GridSystems.ControlSystem.GetShipController() as MyCockpit) != null && myCockpit?.Pilot?.GetClientIdentity().SteamId == MyEventContext.Current.Sender.Value));
			SetParkedStateInternal(!IsParked);
			bool isParked2 = IsParked;
			HandbrakeToggleResult arg = (isParked ? HandbrakeToggleResult.RELEASED : (isParked2 ? HandbrakeToggleResult.ENGAGED_SUCCESSFULLY : HandbrakeToggleResult.FAILED_TO_ENGAGE));
			if (num)
			{
				string arg2 = GridSystems.ConveyorSystem.HudMessageCustom ?? string.Empty;
				GridSystems.ConveyorSystem.HudMessageCustom = string.Empty;
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.ReceiveParkRequestResult, arg, arg2, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 11143)]
		[Reliable]
		[Client]
		public void ReceiveParkRequestResult(HandbrakeToggleResult result, string message)
		{
			MyStringId myStringId = MyStringId.NullOrEmpty;
			switch (result)
			{
			case HandbrakeToggleResult.FAILED_TO_ENGAGE:
				myStringId = MySpaceTexts.NotificationParkingFailed;
				break;
			case HandbrakeToggleResult.RELEASED:
				myStringId = MySpaceTexts.NotificationParkingReleased;
				break;
			case HandbrakeToggleResult.ENGAGED_SUCCESSFULLY:
				myStringId = MySpaceTexts.NotificationParkingSuccessful;
				break;
			}
			if (myStringId != MyStringId.NullOrEmpty)
			{
				MyHud.Notifications.Add(new MyHudNotification(myStringId));
			}
			if (!string.IsNullOrEmpty(message))
			{
				MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.Format_OneParameter);
				myHudNotification.SetTextFormatArguments(message);
				MyHud.Notifications.Add(myHudNotification);
			}
		}

		internal void EnableDampingInternal(bool enableDampeners, bool updateProxy)
		{
			if (EntityThrustComponent == null || EntityThrustComponent.DampenersEnabled == enableDampeners)
			{
				return;
			}
			EntityThrustComponent.DampenersEnabled = enableDampeners;
			m_dampenersEnabled.Value = enableDampeners;
			if (Physics != null && Physics.RigidBody != null && !Physics.RigidBody.IsActive)
			{
				ActivatePhysics();
			}
			if (MySession.Static.LocalHumanPlayer == null)
			{
				return;
			}
			MyCockpit myCockpit = MySession.Static.LocalHumanPlayer.Controller.ControlledEntity as MyCockpit;
			if (myCockpit != null && myCockpit.CubeGrid == this)
			{
				if (m_inertiaDampenersNotification == null)
				{
					m_inertiaDampenersNotification = new MyHudNotification();
				}
				m_inertiaDampenersNotification.Text = (EntityThrustComponent.DampenersEnabled ? MyCommonTexts.NotificationInertiaDampenersOn : MyCommonTexts.NotificationInertiaDampenersOff);
				MyHud.Notifications.Add(m_inertiaDampenersNotification);
				MyHud.SinkGroupInfo.Reload();
			}
		}

		private void DampenersEnabledChanged()
		{
			EnableDampingInternal(m_dampenersEnabled.Value, updateProxy: false);
		}

		public void ChangeGridOwner(long playerId, MyOwnershipShareModeEnum shareMode)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnChangeGridOwner, playerId, shareMode);
			OnChangeGridOwner(playerId, shareMode);
		}

<<<<<<< HEAD
		[Event(null, 11216)]
=======
		[Event(null, 10874)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void OnChangeGridOwner(long playerId, MyOwnershipShareModeEnum shareMode)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = GetBlocks().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					if (current.FatBlock != null && current.BlockDefinition.RatioEnoughForOwnership(current.BuildLevelRatio))
					{
						current.FatBlock.ChangeOwner(playerId, shareMode);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void AnnounceRemoveSplit(List<MySlimBlock> blocks)
		{
			m_tmpPositionListSend.Clear();
			foreach (MySlimBlock block in blocks)
			{
				m_tmpPositionListSend.Add(block.Position);
			}
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnRemoveSplit, m_tmpPositionListSend);
		}

<<<<<<< HEAD
		[Event(null, 11238)]
=======
		[Event(null, 10896)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void OnRemoveSplit(List<Vector3I> removedBlocks)
		{
			using (MyUtils.ReuseCollection(ref m_tmpBlockListReceive))
			{
				foreach (Vector3I removedBlock in removedBlocks)
				{
					MySlimBlock cubeBlock = GetCubeBlock(removedBlock);
					if (cubeBlock == null)
					{
						MySandboxGame.Log.WriteLine("Block was null when trying to remove a grid split. Desync?");
					}
					else
					{
						m_tmpBlockListReceive.Add(cubeBlock);
					}
				}
				RemoveSplit(this, m_tmpBlockListReceive, 0, m_tmpBlockListReceive.Count, sync: false);
			}
		}

		public void ChangeDisplayNameRequest(string displayName)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnChangeDisplayNameRequest, displayName);
		}

<<<<<<< HEAD
		[Event(null, 11265)]
=======
		[Event(null, 10923)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.BigOwner)]
		[Broadcast]
		private void OnChangeDisplayNameRequest(string displayName)
		{
			base.DisplayName = displayName;
			if (this.OnNameChanged != null)
			{
				this.OnNameChanged(this);
			}
		}

		public void ModifyGroup(MyBlockGroup group)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			m_tmpBlockIdList.Clear();
			Enumerator<MyTerminalBlock> enumerator = group.Blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					m_tmpBlockIdList.Add(current.EntityId);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnModifyGroupSuccess, group.Name.ToString(), m_tmpBlockIdList);
		}

<<<<<<< HEAD
		[Event(null, 11283)]
=======
		[Event(null, 10941)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access)]
		[BroadcastExcept]
		private void OnModifyGroupSuccess(string name, List<long> blocks)
		{
			if (blocks == null || blocks.Count == 0)
			{
				foreach (MyBlockGroup blockGroup in BlockGroups)
				{
					if (blockGroup.Name.ToString().Equals(name))
					{
						RemoveGroup(blockGroup);
						break;
					}
				}
				return;
			}
			MyBlockGroup myBlockGroup = new MyBlockGroup();
			myBlockGroup.Name.Clear().Append(name);
			foreach (long block in blocks)
			{
				MyTerminalBlock entity = null;
				if (MyEntities.TryGetEntityById(block, out entity, allowClosed: false))
				{
					myBlockGroup.Blocks.Add(entity);
				}
			}
			AddGroup(myBlockGroup, unionSameNameGroups: false);
		}

		public void RazeBlockInCompoundBlock(List<Tuple<Vector3I, ushort>> locationsAndIds)
		{
			ConvertToLocationIdentityList(locationsAndIds, m_tmpLocationsAndIdsSend);
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnRazeBlockInCompoundBlockRequest, m_tmpLocationsAndIdsSend);
		}

<<<<<<< HEAD
		[Event(null, 11317)]
=======
		[Event(null, 10975)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private void OnRazeBlockInCompoundBlockRequest(List<LocationIdentity> locationsAndIds)
		{
			OnRazeBlockInCompoundBlock(locationsAndIds);
			if (m_tmpLocationsAndIdsReceive.Count > 0)
			{
				ConvertToLocationIdentityList(m_tmpLocationsAndIdsReceive, m_tmpLocationsAndIdsSend);
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnRazeBlockInCompoundBlockSuccess, m_tmpLocationsAndIdsSend);
			}
		}

		public void OnGridPresenceUpdate(bool isAnyGridPresent)
		{
			GridPresenceUpdate.InvokeIfNotNull(isAnyGridPresent);
		}

<<<<<<< HEAD
		[Event(null, 11335)]
=======
		[Event(null, 10993)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void OnRazeBlockInCompoundBlockSuccess(List<LocationIdentity> locationsAndIds)
		{
			OnRazeBlockInCompoundBlock(locationsAndIds);
		}

		private void OnRazeBlockInCompoundBlock(List<LocationIdentity> locationsAndIds)
		{
			m_tmpLocationsAndIdsReceive.Clear();
			RazeBlockInCompoundBlockSuccess(locationsAndIds, m_tmpLocationsAndIdsReceive);
		}

		private static void ConvertToLocationIdentityList(List<Tuple<Vector3I, ushort>> locationsAndIdsFrom, List<LocationIdentity> locationsAndIdsTo)
		{
			locationsAndIdsTo.Clear();
			locationsAndIdsTo.Capacity = locationsAndIdsFrom.Count;
			foreach (Tuple<Vector3I, ushort> item in locationsAndIdsFrom)
			{
				locationsAndIdsTo.Add(new LocationIdentity
				{
					Location = item.Item1,
					Id = item.Item2
				});
			}
		}

		public static void ChangeOwnersRequest(MyOwnershipShareModeEnum shareMode, List<MySingleOwnershipRequest> requests, long requestingPlayer)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnChangeOwnersRequest, shareMode, requests, requestingPlayer);
		}

<<<<<<< HEAD
		[Event(null, 11362)]
=======
		[Event(null, 11020)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access)]
		private static void OnChangeOwnersRequest(MyOwnershipShareModeEnum shareMode, List<MySingleOwnershipRequest> requests, long requestingPlayer)
		{
			MyCubeBlock entity = null;
			int num = 0;
			ulong num2 = MySession.Static.Players.TryGetSteamId(requestingPlayer);
			bool flag = false;
			if (MySession.Static.IsUserAdmin(num2))
			{
				if (num2 != 0L && MySession.Static.RemoteAdminSettings.TryGetValue(num2, out var value) && value.HasFlag(AdminSettingsEnum.Untargetable))
				{
					num = requests.Count;
				}
				AdminSettingsEnum? adminSettingsEnum = null;
				if (num2 == Sync.MyId)
				{
					adminSettingsEnum = MySession.Static.AdminSettings;
				}
				else if (MySession.Static.RemoteAdminSettings.ContainsKey(num2))
				{
					adminSettingsEnum = MySession.Static.RemoteAdminSettings[num2];
				}
				if (((uint?)adminSettingsEnum & 4u) != 0)
				{
					flag = true;
				}
			}
			while (num < requests.Count)
			{
				if (MyEntities.TryGetEntityById(requests[num].BlockId, out entity, allowClosed: false))
				{
					MyEntityOwnershipComponent myEntityOwnershipComponent = entity.Components.Get<MyEntityOwnershipComponent>();
					if (Sync.IsServer && flag)
					{
						num++;
					}
					else if (Sync.IsServer && entity.IDModule != null && (entity.IDModule.Owner == 0L || entity.IDModule.Owner == requestingPlayer))
					{
						num++;
					}
					else if (Sync.IsServer && myEntityOwnershipComponent != null && (myEntityOwnershipComponent.OwnerId == 0L || myEntityOwnershipComponent.OwnerId == requestingPlayer))
					{
						num++;
					}
					else
					{
						requests.RemoveAtFast(num);
					}
				}
				else
				{
					num++;
				}
			}
			if (requests.Count > 0)
			{
				OnChangeOwnersSuccess(shareMode, requests);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnChangeOwnersSuccess, shareMode, requests);
			}
		}

<<<<<<< HEAD
		[Event(null, 11428)]
=======
		[Event(null, 11086)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private static void OnChangeOwnersSuccess(MyOwnershipShareModeEnum shareMode, List<MySingleOwnershipRequest> requests)
		{
			foreach (MySingleOwnershipRequest request in requests)
			{
				MyCubeBlock entity = null;
				if (MyEntities.TryGetEntityById(request.BlockId, out entity, allowClosed: false))
				{
					entity.ChangeOwner(request.Owner, shareMode);
				}
			}
		}

		public override void SerializeControls(BitStream stream)
		{
			MyShipController myShipController = null;
			if (base.InScene)
			{
				myShipController = GridSystems.ControlSystem.GetShipController();
			}
			if (myShipController != null)
			{
				stream.WriteBool(value: true);
				myShipController.GetNetState().Serialize(stream);
			}
			else
			{
				stream.WriteBool(value: false);
			}
		}

		public override void DeserializeControls(BitStream stream, bool outOfOrder)
		{
			if (stream.ReadBool())
			{
				MyGridClientState lastNetState = new MyGridClientState(stream);
				if (!outOfOrder)
				{
					m_lastNetState = lastNetState;
				}
				if (GridSystems.ControlSystem != null)
				{
					MyShipController shipController = GridSystems.ControlSystem.GetShipController();
					if (shipController != null && shipController.ControllerInfo != null && !shipController.ControllerInfo.IsLocallyControlled())
					{
						shipController.SetNetState(m_lastNetState);
					}
				}
			}
			else
			{
				m_lastNetState.Valid = false;
			}
		}

		public override void ResetControls()
		{
			m_lastNetState.Valid = false;
			MyShipController shipController = GridSystems.ControlSystem.GetShipController();
			if (shipController != null && !shipController.ControllerInfo.IsLocallyControlled())
			{
				shipController.ClearMovementControl();
			}
		}

		public override void ApplyLastControls()
		{
			if (m_lastNetState.Valid)
			{
				MyShipController shipController = GridSystems.ControlSystem.GetShipController();
				if (shipController != null && !shipController.ControllerInfo.IsLocallyControlled())
				{
					shipController.SetNetState(m_lastNetState);
				}
			}
		}

		public void TargetingAddId(long id)
		{
			if (!m_targetingList.Contains(id))
			{
				m_targetingList.Add(id);
			}
			m_usesTargetingList = m_targetingList.Count > 0 || m_targetingListIsWhitelist;
		}

		public void TargetingRemoveId(long id)
		{
			if (m_targetingList.Contains(id))
			{
				m_targetingList.Remove(id);
			}
			m_usesTargetingList = m_targetingList.Count > 0 || m_targetingListIsWhitelist;
		}

		public void TargetingSetWhitelist(bool whitelist)
		{
			m_targetingListIsWhitelist = whitelist;
			m_usesTargetingList = m_targetingList.Count > 0 || m_targetingListIsWhitelist;
		}

		public bool TargetingCanAttackGrid(long id)
		{
			if (m_targetingListIsWhitelist)
			{
				return m_targetingList.Contains(id);
			}
			return !m_targetingList.Contains(id);
		}

		public void HierarchyUpdated(MyCubeGrid root)
		{
			MyGridPhysics physics = Physics;
			if (physics != null)
			{
				if (this != root)
				{
					physics.SetRelaxedRigidBodyMaxVelocities();
				}
				else
				{
					physics.SetDefaultRigidBodyMaxVelocities();
				}
			}
			this.OnHierarchyUpdated.InvokeIfNotNull(this);
		}

		public void RegisterInventory(MyCubeBlock block)
		{
			m_inventoryBlocks.Add(block);
			if (((MyEntity)block).TryGetInventory(out MyInventoryBase inventoryBase) && inventoryBase.CurrentMass > 0)
			{
				SetInventoryMassDirty();
			}
		}

		public void UnregisterInventory(MyCubeBlock block)
		{
			m_inventoryBlocks.Remove(block);
			if (((MyEntity)block).TryGetInventory(out MyInventoryBase inventoryBase) && inventoryBase.CurrentMass > 0)
			{
				SetInventoryMassDirty();
			}
		}

		public void RegisterUnsafeBlock(MyCubeBlock block)
		{
			if (m_unsafeBlocks.Add(block))
			{
				if (m_unsafeBlocks.get_Count() == 1)
				{
					MyUnsafeGridsSessionComponent.RegisterGrid(this);
				}
				else
				{
					MyUnsafeGridsSessionComponent.OnGridChanged(this);
				}
			}
		}

		public void UnregisterUnsafeBlock(MyCubeBlock block)
		{
			if (m_unsafeBlocks.Remove(block))
			{
				if (m_unsafeBlocks.get_Count() == 0)
				{
					MyUnsafeGridsSessionComponent.UnregisterGrid(this);
				}
				else
				{
					MyUnsafeGridsSessionComponent.OnGridChanged(this);
				}
			}
		}

		public void RegisterDecoy(MyDecoy block)
		{
			if (m_decoys == null)
			{
				m_decoys = new HashSet<MyDecoy>();
			}
			m_decoys.Add(block);
		}

		public void UnregisterDecoy(MyDecoy block)
		{
			m_decoys.Remove(block);
		}

		public void RegisterOccupiedBlock(MyCockpit block)
		{
			m_occupiedBlocks.Add(block);
		}

		public void UnregisterOccupiedBlock(MyCockpit block)
		{
			if (!m_occupiedBlocks.Remove(block) && MySession.Static.IsUnloading)
			{
				_ = base.MarkedForClose;
			}
		}

		private void OnContactPointChanged()
		{
			if (Physics == null || base.Closed || base.MarkedForClose || Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return;
			}
			ContactPointData value = m_contactPoint.Value;
			MyEntity entity = null;
			if (MyEntities.TryGetEntityById(value.EntityId, out entity) && entity.Physics != null)
			{
				Vector3D worldPosition = value.LocalPosition + base.PositionComp.WorldMatrixRef.Translation;
				if ((value.ContactPointType & ContactPointData.ContactPointDataTypes.Sounds) != 0)
				{
					MyAudioComponent.PlayContactSoundInternal(this, entity, worldPosition, value.Normal, value.SeparatingSpeed);
				}
				if ((value.ContactPointType & ContactPointData.ContactPointDataTypes.AnyParticle) != 0)
				{
					Physics.PlayCollisionParticlesInternal(entity, ref worldPosition, ref value.Normal, ref value.SeparatingVelocity, value.SeparatingSpeed, value.Impulse, value.ContactPointType);
				}
			}
		}

		public void UpdateParticleContactPoint(long entityId, ref Vector3 relativePosition, ref Vector3 normal, ref Vector3 separatingVelocity, float separatingSpeed, float impulse, ContactPointData.ContactPointDataTypes flags)
		{
			if (flags != 0)
			{
				ContactPointData contactPointData = default(ContactPointData);
				contactPointData.EntityId = entityId;
				contactPointData.LocalPosition = relativePosition;
				contactPointData.Normal = normal;
				contactPointData.ContactPointType = flags;
				contactPointData.SeparatingVelocity = separatingVelocity;
				contactPointData.SeparatingSpeed = separatingSpeed;
				contactPointData.Impulse = impulse;
				ContactPointData localValue = contactPointData;
				m_contactPoint.SetLocalValue(localValue);
			}
		}

		public void MarkAsTrash()
		{
			m_markedAsTrash.Value = true;
		}

		private void MarkedAsTrashChanged()
		{
			if (MarkedAsTrash)
			{
				MarkForDraw();
				Schedule(UpdateQueue.AfterSimulation, UpdateTrash, 3);
				m_trashHighlightCounter = TRASH_HIGHLIGHT;
			}
			else
			{
				DeSchedule(UpdateQueue.AfterSimulation, UpdateTrash);
			}
		}

		private void UpdateTrash()
		{
			m_trashHighlightCounter--;
			if (TrashHighlightCounter <= 0 && Sync.IsServer)
			{
				MySessionComponentTrash.RemoveGrid(this);
			}
		}

		public void LogHierarchy()
		{
			OnLogHierarchy();
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnLogHierarchy);
		}

<<<<<<< HEAD
		[Event(null, 11862)]
=======
		[Event(null, 11483)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public void OnLogHierarchy()
		{
			MyGridPhysicalHierarchy.Static.Log(MyGridPhysicalHierarchy.Static.GetRoot(this));
		}

<<<<<<< HEAD
		[Event(null, 11869)]
=======
		[Event(null, 11490)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		public static void DepressurizeEffect(long gridId, Vector3I from, Vector3I to)
		{
			MySandboxGame.Static.Invoke(delegate
			{
				DepressurizeEffect_Implementation(gridId, from, to);
			}, "CubeGrid - DepressurizeEffect");
		}

		public static void DepressurizeEffect_Implementation(long gridId, Vector3I from, Vector3I to)
		{
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(gridId) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyGridGasSystem.AddDepressurizationEffects(myCubeGrid, from, to);
			}
		}

		public override void OnReplicationStarted()
		{
			base.OnReplicationStarted();
			MySession.Static.GetComponent<MyPhysics>()?.InformReplicationStarted(this);
		}

		public override void OnReplicationEnded()
		{
			base.OnReplicationEnded();
			MySession.Static.GetComponent<MyPhysics>()?.InformReplicationEnded(this);
		}

<<<<<<< HEAD
		/// <summary>
		/// Called from MergeBlock to initialte merge of two grids
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyCubeGrid MergeGrid_MergeBlock(MyCubeGrid gridToMerge, Vector3I gridOffset, bool checkMergeOrder = true)
		{
			if (checkMergeOrder && !ShouldBeMergedToThis(gridToMerge))
			{
				return null;
			}
			MatrixI transform = CalculateMergeTransform(gridToMerge, gridOffset);
			MyMultiplayer.RaiseBlockingEvent(this, gridToMerge, (Func<MyCubeGrid, Action<long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction>>)((MyCubeGrid x) => x.MergeGrid_MergeBlockClient), gridToMerge.EntityId, (SerializableVector3I)transform.Translation, transform.Forward, transform.Up, default(EndpointId));
			return MergeGridInternal(gridToMerge, ref transform);
		}

		private bool ShouldBeMergedToThis(MyCubeGrid gridToMerge)
		{
			bool flag = IsRooted(this);
			bool flag2 = IsRooted(gridToMerge);
			if (flag && !flag2)
			{
				return true;
			}
			if (flag2 && !flag)
			{
				return false;
			}
			return BlocksCount > gridToMerge.BlocksCount;
		}

		private static bool IsRooted(MyCubeGrid grid)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			if (grid.IsStatic)
			{
				return true;
			}
			MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(grid);
			if (group == null)
			{
				return false;
			}
			Enumerator<MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> enumerator = group.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (MyFixedGrids.IsRooted(enumerator.get_Current().NodeData))
					{
						return true;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return false;
		}

<<<<<<< HEAD
		/// <summary>
		/// Merges grids on client side.
		/// </summary>
		/// <param name="gridId">Grid id to be merge with.</param>
		/// <param name="gridOffset">Grid offset.</param>
		/// <param name="gridForward">Grid forward.</param>
		/// <param name="gridUp">Grid Up.</param>
		/// <param name="mergingBlockPos">Position of block that triggered merge.</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Event(null, 96)]
		[Reliable]
		[Broadcast]
		[Blocking]
		private void MergeGrid_MergeClient(long gridId, SerializableVector3I gridOffset, Base6Directions.Direction gridForward, Base6Directions.Direction gridUp, Vector3I mergingBlockPos)
		{
			MyCubeGrid entity = null;
			if (MyEntities.TryGetEntityById(gridId, out entity, allowClosed: false))
			{
				MatrixI transform = new MatrixI(gridOffset, gridForward, gridUp);
				MergeGridInternal(entity, ref transform);
			}
		}

		[Event(null, 110)]
		[Reliable]
		[Broadcast]
		[Blocking]
		private void MergeGrid_MergeBlockClient(long gridId, SerializableVector3I gridOffset, Base6Directions.Direction gridForward, Base6Directions.Direction gridUp)
		{
			MyCubeGrid entity = null;
			if (MyEntities.TryGetEntityById(gridId, out entity, allowClosed: false))
			{
				MatrixI transform = new MatrixI(gridOffset, gridForward, gridUp);
				MergeGridInternal(entity, ref transform);
			}
		}

		private MyCubeGrid MergeGrid_Static(MyCubeGrid gridToMerge, Vector3I gridOffset, MySlimBlock triggeringMergeBlock)
		{
			MatrixI transform = CalculateMergeTransform(gridToMerge, gridOffset);
			Vector3I vector3I = triggeringMergeBlock.Position;
			if (triggeringMergeBlock.CubeGrid != this)
			{
				vector3I = Vector3I.Transform(vector3I, transform);
			}
			MyMultiplayer.RaiseBlockingEvent(this, gridToMerge, (Func<MyCubeGrid, Action<long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction, Vector3I>>)((MyCubeGrid x) => x.MergeGrid_MergeClient), gridToMerge.EntityId, (SerializableVector3I)transform.Translation, transform.Forward, transform.Up, vector3I, default(EndpointId));
			return MergeGridInternal(gridToMerge, ref transform);
		}

		private MyCubeGrid MergeGridInternal(MyCubeGrid gridToMerge, ref MatrixI transform, bool disableBlockGenerators = true)
		{
			if (MyCubeGridSmallToLargeConnection.Static != null)
			{
				MyCubeGridSmallToLargeConnection.Static.BeforeGridMerge_SmallToLargeGridConnectivity(this, gridToMerge);
			}
			MyRenderComponentCubeGrid tmpRenderComponent = gridToMerge.Render;
			tmpRenderComponent.DeferRenderRelease = true;
			Matrix transformMatrix = transform.GetFloatMatrix();
			transformMatrix.Translation *= GridSize;
			Action<MatrixD> updateMergingComponentWM = delegate(MatrixD matrix)
			{
				MyRenderComponentCubeGrid myRenderComponentCubeGrid = tmpRenderComponent;
				MatrixD m = transformMatrix * matrix;
				myRenderComponentCubeGrid.UpdateRenderObjectMatrices(m);
			};
			Action releaseRenderOldRenderComponent = null;
			releaseRenderOldRenderComponent = delegate
			{
				tmpRenderComponent.DeferRenderRelease = false;
				m_updateMergingGrids = (Action<MatrixD>)Delegate.Remove(m_updateMergingGrids, updateMergingComponentWM);
				m_pendingGridReleases = (Action)Delegate.Remove(m_pendingGridReleases, releaseRenderOldRenderComponent);
			};
			m_updateMergingGrids = (Action<MatrixD>)Delegate.Combine(m_updateMergingGrids, updateMergingComponentWM);
			m_pendingGridReleases = (Action)Delegate.Combine(m_pendingGridReleases, releaseRenderOldRenderComponent);
			Schedule(UpdateQueue.AfterSimulation, UpdateMergingGrids, 7);
			MoveBlocksAndClose(gridToMerge, this, transform, disableBlockGenerators);
			UpdateGridAABB();
			if (Physics != null)
			{
				UpdatePhysicsShape();
			}
			if (MyCubeGridSmallToLargeConnection.Static != null)
			{
				MyCubeGridSmallToLargeConnection.Static.AfterGridMerge_SmallToLargeGridConnectivity(this);
			}
			updateMergingComponentWM(base.WorldMatrix);
			this.OnGridMerge?.Invoke(this, gridToMerge);
			gridToMerge.OnGridMerge?.Invoke(this, gridToMerge);
			return this;
		}

		/// <summary>
		/// Used only when all blocks of grid are moved.
		/// Moving only some blocks is unsupported now.
		/// </summary>
		private static void MoveBlocksAndClose(MyCubeGrid from, MyCubeGrid to, MatrixI transform, bool disableBlockGenerators = true)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			from.MarkedForClose = true;
			to.IsBlockTrasferInProgress = true;
			from.IsBlockTrasferInProgress = true;
			try
			{
				MyEntities.Remove(from);
				MyBlockGroup[] array = from.BlockGroups.ToArray();
				foreach (MyBlockGroup group in array)
				{
					to.AddGroup(group);
				}
				from.BlockGroups.Clear();
				from.UnregisterBlocksBeforeClose();
				Enumerator<MySlimBlock> enumerator = from.m_cubeBlocks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current = enumerator.get_Current();
						if (current.FatBlock != null)
						{
							from.Hierarchy.RemoveChild(current.FatBlock);
						}
						current.RemoveNeighbours();
						current.RemoveAuthorship();
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				if (from.Physics != null)
				{
					from.Physics.Close();
					from.Physics = null;
					from.RaisePhysicsChanged();
				}
				enumerator = from.m_cubeBlocks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current2 = enumerator.get_Current();
						current2.Transform(ref transform);
						to.AddBlockInternal(current2);
					}
				}
<<<<<<< HEAD
=======
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				from.Skeleton.CopyTo(to.Skeleton, transform, to);
				from.m_blocksForDraw.Clear();
				from.m_cubeBlocks.Clear();
				from.m_fatBlocks.Clear();
				from.m_cubes.Clear();
				from.MarkedForClose = false;
				if (Sync.IsServer)
				{
					from.Close();
				}
			}
			finally
			{
				to.IsBlockTrasferInProgress = false;
				from.IsBlockTrasferInProgress = false;
			}
		}

		private void UpdateMergingGrids()
		{
			if (m_updateMergingGrids != null)
			{
				m_updateMergingGrids(base.WorldMatrix);
			}
			else
			{
				DeSchedule(UpdateQueue.AfterSimulation, UpdateMergingGrids);
			}
		}

		private void ReleaseMerginGrids()
		{
			if (m_pendingGridReleases != null)
			{
				m_pendingGridReleases();
			}
		}

		private bool CanMoveBlocksFrom(MyCubeGrid grid, Vector3I blockOffset)
		{
			try
			{
				MatrixI transformation = CalculateMergeTransform(grid, blockOffset);
				foreach (KeyValuePair<Vector3I, MyCube> cube in grid.m_cubes)
				{
					Vector3I vector3I = Vector3I.Transform(cube.Key, transformation);
					if (m_cubes.ContainsKey(vector3I))
					{
						return false;
					}
				}
				return true;
			}
			finally
			{
			}
		}

		public static void Preload()
		{
		}

		public static void GetCubeParts(MyCubeBlockDefinition block, Vector3I inputPosition, Matrix rotation, float gridSize, List<string> outModels, List<MatrixD> outLocalMatrices, List<Vector3> outLocalNormals, List<Vector4UByte> outPatternOffsets, bool topologyCheck)
		{
			outModels.Clear();
			outLocalMatrices.Clear();
			outLocalNormals.Clear();
			outPatternOffsets.Clear();
			if (block.CubeDefinition == null)
			{
				return;
			}
			if (topologyCheck)
			{
				Base6Directions.Direction direction = Base6Directions.GetDirection(Vector3I.Round(rotation.Forward));
				Base6Directions.Direction direction2 = Base6Directions.GetDirection(Vector3I.Round(rotation.Up));
				MyCubeGridDefinitions.GetTopologyUniqueOrientation(block.CubeDefinition.CubeTopology, new MyBlockOrientation(direction, direction2)).GetMatrix(out rotation);
			}
			MyTileDefinition[] cubeTiles = MyCubeGridDefinitions.GetCubeTiles(block);
			int num = cubeTiles.Length;
			int num2 = 0;
			int num3 = 32768;
			float epsilon = 0.01f;
			for (int i = 0; i < num; i++)
			{
				MyTileDefinition myTileDefinition = cubeTiles[num2 + i];
				MatrixD item = (MatrixD)myTileDefinition.LocalMatrix * rotation;
				Vector3 vector = Vector3.Transform(myTileDefinition.Normal, rotation.GetOrientation());
				if (topologyCheck && myTileDefinition.Id != MyStringId.NullOrEmpty)
				{
					MyCubeGridDefinitions.TileGridOrientations.TryGetValue(myTileDefinition.Id, out var value);
					if (value.TryGetValue(new Vector3I(Vector3.Sign(vector)), out var value2))
					{
						item = value2.LocalMatrix;
					}
				}
				Vector3I vector3I = inputPosition;
				if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base && myTileDefinition.Id == MyStringId.NullOrEmpty)
				{
					Vector3I vector3I2 = new Vector3I(-Vector3.Sign(vector.MaxAbsComponent()));
					vector3I += vector3I2;
				}
				string text = block.CubeDefinition.Model[i];
				Vector2I vector2I = block.CubeDefinition.PatternSize[i];
				Vector2I vector2I2 = block.CubeDefinition.ScaleTile[i];
				int num4 = (int)MyModels.GetModelOnlyData(text).PatternScale;
				vector2I = new Vector2I(vector2I.X * num4, vector2I.Y * num4);
				int num5 = 0;
				int num6 = 0;
				float num7 = Vector3.Dot(Vector3.UnitY, vector);
				float num8 = Vector3.Dot(Vector3.UnitX, vector);
				float num9 = Vector3.Dot(Vector3.UnitZ, vector);
				if (MyUtils.IsZero(Math.Abs(num7) - 1f, epsilon))
				{
					int num10 = (vector3I.X + num3) / vector2I.Y;
					int num11 = MyMath.Mod(num10 + (int)((double)num10 * Math.Sin((float)num10 * 10f)), vector2I.X);
					num5 = MyMath.Mod(vector3I.Z + vector3I.Y + num11 + num3, vector2I.X);
					num6 = MyMath.Mod(vector3I.X + num3, vector2I.Y);
					if (Math.Sign(num7) == 1)
					{
						num6 = vector2I.Y - 1 - num6;
					}
				}
				else if (MyUtils.IsZero(Math.Abs(num8) - 1f, epsilon))
				{
					int num12 = (vector3I.Z + num3) / vector2I.Y;
					int num13 = MyMath.Mod(num12 + (int)((double)num12 * Math.Sin((float)num12 * 10f)), vector2I.X);
					num5 = MyMath.Mod(vector3I.X + vector3I.Y + num13 + num3, vector2I.X);
					num6 = MyMath.Mod(vector3I.Z + num3, vector2I.Y);
					if (Math.Sign(num8) == 1)
					{
						num6 = vector2I.Y - 1 - num6;
					}
				}
				else if (MyUtils.IsZero(Math.Abs(num9) - 1f, epsilon))
				{
					int num14 = (vector3I.Y + num3) / vector2I.Y;
					int num15 = MyMath.Mod(num14 + (int)((double)num14 * Math.Sin((float)num14 * 10f)), vector2I.X);
					num5 = MyMath.Mod(vector3I.X + num15 + num3, vector2I.X);
					num6 = MyMath.Mod(vector3I.Y + num3, vector2I.Y);
					if (Math.Sign(num9) == 1)
					{
						num5 = vector2I.X - 1 - num5;
					}
				}
				else if (MyUtils.IsZero(num8, epsilon))
				{
					num5 = MyMath.Mod(vector3I.X * vector2I2.X + num3, vector2I.X);
					num6 = MyMath.Mod(vector3I.Z * vector2I2.Y + num3, vector2I.Y);
					if (Math.Sign(num9) == -1)
					{
						if (Math.Sign(num7) == 1)
						{
							if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
							{
								if (num9 < -0.5f)
								{
									num5 = MyMath.Mod(vector3I.X * vector2I2.X + num3, vector2I.X);
									num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
								}
								else
								{
									num6 = vector2I.Y - 1 - num6;
									num5 = vector2I.X - 1 - num5;
								}
							}
							else
							{
								num6 = vector2I.Y - 1 - num6;
								num5 = vector2I.X - 1 - num5;
							}
						}
						else if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
						{
							if ((double)num9 < -0.5)
							{
								num5 = MyMath.Mod(vector3I.X * vector2I2.X + num3, vector2I.X);
								num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
								num5 = vector2I.X - 1 - num5;
								num6 = vector2I.Y - 1 - num6;
							}
							else
							{
								num6 = vector2I.Y - 1 - num6;
							}
						}
						else
						{
							num6 = vector2I.Y - 1 - num6;
						}
					}
					else if (Math.Sign(num7) == -1)
					{
						if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
						{
							if (num9 > 0.5f)
							{
								num5 = MyMath.Mod(vector3I.X * vector2I2.X + num3, vector2I.X);
								num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
								num6 = vector2I.Y - 1 - num6;
							}
							else
							{
								num5 = vector2I.X - 1 - num5;
							}
						}
						else
						{
							num5 = vector2I.X - 1 - num5;
						}
					}
					else if ((block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip) && !((double)num7 > 0.5))
					{
						num5 = MyMath.Mod(vector3I.X * vector2I2.X + num3, vector2I.X);
						num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
						num5 = vector2I.X - 1 - num5;
					}
				}
				else if (MyUtils.IsZero(num9, epsilon))
				{
					num5 = MyMath.Mod(vector3I.Z * vector2I2.X + num3, vector2I.X);
					num6 = MyMath.Mod(vector3I.X * vector2I2.Y + num3, vector2I.Y);
					if (Math.Sign(num8) == 1)
					{
						if (Math.Sign(num7) == 1)
						{
							if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
							{
								if (num8 > 0.5f)
								{
									num5 = MyMath.Mod(vector3I.Z * vector2I2.X + num3, vector2I.X);
									num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
								}
								else
								{
									num5 = vector2I.X - 1 - num5;
								}
							}
							else
							{
								num5 = vector2I.X - 1 - num5;
							}
						}
						else if ((block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip) && !(num7 < -0.5f))
						{
							num5 = MyMath.Mod(vector3I.Z * vector2I2.X + num3, vector2I.X);
							num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
							num5 = vector2I.X - 1 - num5;
							num6 = vector2I.Y - 1 - num6;
						}
					}
					else if (Math.Sign(num7) == 1)
					{
						if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
						{
							if (num7 > 0.5f)
							{
								num6 = vector2I.Y - 1 - num6;
							}
							else
							{
								num5 = MyMath.Mod(vector3I.Z * vector2I2.X + num3, vector2I.X);
								num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
								num5 = vector2I.X - 1 - num5;
							}
						}
						else
						{
							num6 = vector2I.Y - 1 - num6;
						}
					}
					else if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
					{
						if (num7 < -0.5f)
						{
							num5 = vector2I.X - 1 - num5;
							num6 = vector2I.Y - 1 - num6;
						}
						else
						{
							num5 = MyMath.Mod(vector3I.Z * vector2I2.X + num3, vector2I.X);
							num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
							num6 = vector2I.Y - 1 - num6;
						}
					}
					else
					{
						num5 = vector2I.X - 1 - num5;
						num6 = vector2I.Y - 1 - num6;
					}
				}
				else if (MyUtils.IsZero(num7, epsilon))
				{
					num5 = MyMath.Mod(vector3I.Y * vector2I2.X + num3, vector2I.X);
					num6 = MyMath.Mod(vector3I.Z * vector2I2.Y + num3, vector2I.Y);
					if (Math.Sign(num9) == -1)
					{
						if (Math.Sign(num8) == 1)
						{
							if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
							{
								if (num9 < -0.5f)
								{
									num5 = vector2I.X - 1 - num5;
									num6 = vector2I.Y - 1 - num6;
								}
								else
								{
									num6 = vector2I.Y - 1 - num6;
								}
							}
							else
							{
								num5 = vector2I.X - 1 - num5;
							}
						}
						else if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
						{
							if ((double)num9 < -0.5)
							{
								num6 = vector2I.Y - 1 - num6;
							}
							else
							{
								num5 = vector2I.X - 1 - num5;
								num6 = vector2I.Y - 1 - num6;
							}
						}
					}
					else if (Math.Sign(num8) == 1)
					{
						if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
						{
							if (num8 > 0.5f)
							{
								num5 = vector2I.X - 1 - num5;
							}
							else
							{
								num5 = MyMath.Mod(vector3I.Y * vector2I2.X + num3, vector2I.X);
								num6 = MyMath.Mod(vector3I.X * vector2I2.Y + num3, vector2I.Y);
							}
						}
						else
						{
							num6 = vector2I.Y - 1 - num6;
						}
					}
					else if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
					{
						if (!((double)num8 < -0.5))
						{
							num5 = MyMath.Mod(vector3I.Y * vector2I2.X + num3, vector2I.X);
							num6 = MyMath.Mod(vector3I.X * vector2I2.Y + num3, vector2I.Y);
							num5 = vector2I.X - 1 - num5;
							num6 = vector2I.Y - 1 - num6;
						}
					}
					else
					{
						num5 = vector2I.X - 1 - num5;
						num6 = vector2I.Y - 1 - num6;
					}
				}
				item.Translation = inputPosition * gridSize;
				if (myTileDefinition.DontOffsetTexture)
				{
					num5 = 0;
					num6 = 0;
				}
				outPatternOffsets.Add(new Vector4UByte((byte)num5, (byte)num6, (byte)vector2I.X, (byte)vector2I.Y));
				outModels.Add(text);
				outLocalMatrices.Add(item);
				outLocalNormals.Add(vector);
			}
		}

		public static void CheckAreaConnectivity(MyCubeGrid grid, ref MyBlockBuildArea area, List<Vector3UByte> validOffsets, HashSet<Vector3UByte> resultFailList)
		{
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(area.DefinitionId);
				if (cubeBlockDefinition == null)
<<<<<<< HEAD
				{
					return;
				}
				Quaternion rotation = Base6Directions.GetOrientation(area.OrientationForward, area.OrientationUp);
				Vector3I vector3I = area.StepDelta;
				MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = cubeBlockDefinition.GetBuildProgressModelMountPoints(MyComponentStack.NewBlockIntegrity);
				for (int num = validOffsets.Count - 1; num >= 0; num--)
				{
					Vector3I position = area.PosInGrid + validOffsets[num] * vector3I;
					if (CheckConnectivity(grid, cubeBlockDefinition, buildProgressModelMountPoints, ref rotation, ref position))
					{
						m_tmpAreaMountpointPass.Add(validOffsets[num]);
						validOffsets.RemoveAtFast(num);
					}
				}
				m_areaOverlapTest.Initialize(ref area, cubeBlockDefinition);
				foreach (Vector3UByte item in m_tmpAreaMountpointPass)
				{
					m_areaOverlapTest.AddBlock(item);
=======
				{
					return;
				}
				Quaternion rotation = Base6Directions.GetOrientation(area.OrientationForward, area.OrientationUp);
				Vector3I vector3I = area.StepDelta;
				MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = cubeBlockDefinition.GetBuildProgressModelMountPoints(MyComponentStack.NewBlockIntegrity);
				for (int num = validOffsets.Count - 1; num >= 0; num--)
				{
					Vector3I position = area.PosInGrid + validOffsets[num] * vector3I;
					if (CheckConnectivity(grid, cubeBlockDefinition, buildProgressModelMountPoints, ref rotation, ref position))
					{
						m_tmpAreaMountpointPass.Add(validOffsets[num]);
						validOffsets.RemoveAtFast(num);
					}
				}
				m_areaOverlapTest.Initialize(ref area, cubeBlockDefinition);
				Enumerator<Vector3UByte> enumerator = m_tmpAreaMountpointPass.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Vector3UByte current = enumerator.get_Current();
						m_areaOverlapTest.AddBlock(current);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				int num2 = int.MaxValue;
				while (validOffsets.Count > 0 && validOffsets.Count < num2)
				{
					num2 = validOffsets.Count;
					for (int num3 = validOffsets.Count - 1; num3 >= 0; num3--)
					{
						Vector3I position2 = area.PosInGrid + validOffsets[num3] * vector3I;
						if (CheckConnectivity(m_areaOverlapTest, cubeBlockDefinition, buildProgressModelMountPoints, ref rotation, ref position2))
						{
							m_tmpAreaMountpointPass.Add(validOffsets[num3]);
							m_areaOverlapTest.AddBlock(validOffsets[num3]);
							validOffsets.RemoveAtFast(num3);
						}
					}
				}
				foreach (Vector3UByte validOffset in validOffsets)
				{
					resultFailList.Add(validOffset);
				}
				validOffsets.Clear();
<<<<<<< HEAD
				validOffsets.AddRange(m_tmpAreaMountpointPass);
=======
				validOffsets.AddRange((IEnumerable<Vector3UByte>)m_tmpAreaMountpointPass);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				m_tmpAreaMountpointPass.Clear();
			}
		}

		public static bool CheckMergeConnectivity(MyCubeGrid hitGrid, MyCubeGrid gridToMerge, Vector3I gridOffset)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			MatrixI transformation = hitGrid.CalculateMergeTransform(gridToMerge, gridOffset);
			transformation.GetBlockOrientation().GetQuaternion(out var result);
<<<<<<< HEAD
			foreach (MySlimBlock block in gridToMerge.GetBlocks())
			{
				Vector3I position = Vector3I.Transform(block.Position, transformation);
				block.Orientation.GetQuaternion(out var result2);
				result2 = result * result2;
				MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
				if (CheckConnectivity(hitGrid, block.BlockDefinition, buildProgressModelMountPoints, ref result2, ref position))
=======
			Enumerator<MySlimBlock> enumerator = gridToMerge.GetBlocks().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MySlimBlock current = enumerator.get_Current();
					Vector3I position = Vector3I.Transform(current.Position, transformation);
					current.Orientation.GetQuaternion(out var result2);
					result2 = result * result2;
					MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = current.BlockDefinition.GetBuildProgressModelMountPoints(current.BuildLevelRatio);
					if (CheckConnectivity(hitGrid, current.BlockDefinition, buildProgressModelMountPoints, ref result2, ref position))
					{
						return true;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return false;
		}

		/// <summary>
		/// Performs check whether cube block given by its definition, rotation and position is connected to some other
		/// block in a given grid.
		/// </summary>
		/// <param name="grid">Grid in which the check is performed.</param>
		/// <param name="def"></param>
		/// <param name="mountPoints"></param>
		/// <param name="rotation">Rotation of the cube block within grid.</param>
		/// <param name="position">Position of the cube block within grid.</param>
		/// <returns>True when there is a connectable neighbor connected by a mount point, otherwise false.</returns>
		public static bool CheckConnectivity(IMyGridConnectivityTest grid, MyCubeBlockDefinition def, MyCubeBlockDefinition.MountPoint[] mountPoints, ref Quaternion rotation, ref Vector3I position)
		{
			try
			{
				if (mountPoints == null)
				{
					return false;
				}
				Vector3I value = def.Center;
				Vector3I value2 = def.Size;
				Vector3I.Transform(ref value, ref rotation, out var _);
				Vector3I.Transform(ref value2, ref rotation, out var _);
				for (int i = 0; i < mountPoints.Length; i++)
				{
					MyCubeBlockDefinition.MountPoint thisMountPoint = mountPoints[i];
					Vector3 value3 = thisMountPoint.Start - value;
					Vector3 value4 = thisMountPoint.End - value;
					if (MyFakes.ENABLE_TEST_BLOCK_CONNECTIVITY_CHECK)
					{
						Vector3 vector = Vector3.Min(thisMountPoint.Start, thisMountPoint.End);
						Vector3 vector2 = Vector3.Max(thisMountPoint.Start, thisMountPoint.End);
						Vector3I vector3I = Vector3I.One - Vector3I.Abs(thisMountPoint.Normal);
						Vector3I vector3I2 = Vector3I.One - vector3I;
						Vector3 vector3 = vector3I2 * vector + Vector3.Clamp(vector, Vector3.Zero, value2) * vector3I + 0.001f * vector3I;
						Vector3 vector4 = vector3I2 * vector2 + Vector3.Clamp(vector2, Vector3.Zero, value2) * vector3I - 0.001f * vector3I;
						value3 = vector3 - value;
						value4 = vector4 - value;
					}
					Vector3I value5 = Vector3I.Floor(value3);
					Vector3I value6 = Vector3I.Floor(value4);
					Vector3.Transform(ref value3, ref rotation, out var result3);
					Vector3.Transform(ref value4, ref rotation, out var result4);
					Vector3I.Transform(ref value5, ref rotation, out var result5);
					Vector3I.Transform(ref value6, ref rotation, out var result6);
					Vector3I vector3I3 = Vector3I.Floor(result3);
					Vector3I vector3I4 = Vector3I.Floor(result4);
					Vector3I vector3I5 = result5 - vector3I3;
					Vector3I vector3I6 = result6 - vector3I4;
					result3 += (Vector3)vector3I5;
					result4 += (Vector3)vector3I6;
					Vector3 value7 = position + result3;
					Vector3 value8 = position + result4;
					m_cacheNeighborBlocks.Clear();
					Vector3 vector5 = Vector3.Min(value7, value8);
					Vector3 vector6 = Vector3.Max(value7, value8);
					Vector3I otherBlockMinPos = Vector3I.Floor(vector5);
					Vector3I otherBlockMaxPos = Vector3I.Floor(vector6);
					grid.GetConnectedBlocks(otherBlockMinPos, otherBlockMaxPos, m_cacheNeighborBlocks);
					if (m_cacheNeighborBlocks.Count == 0)
					{
						continue;
					}
					Vector3I.Transform(ref thisMountPoint.Normal, ref rotation, out var result7);
					otherBlockMinPos -= result7;
					otherBlockMaxPos -= result7;
					Vector3I faceNormal = -result7;
					foreach (ConnectivityResult value9 in m_cacheNeighborBlocks.Values)
					{
						if (value9.Position == position)
						{
							if (!MyFakes.ENABLE_COMPOUND_BLOCKS || (value9.FatBlock != null && value9.FatBlock.CheckConnectionAllowed && !value9.FatBlock.ConnectionAllowed(ref otherBlockMinPos, ref otherBlockMaxPos, ref faceNormal, def)) || !(value9.FatBlock is MyCompoundCubeBlock))
							{
								continue;
							}
							foreach (MySlimBlock block in (value9.FatBlock as MyCompoundCubeBlock).GetBlocks())
							{
								MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
								if (CheckNeighborMountPointsForCompound(vector5, vector6, thisMountPoint, ref result7, def, value9.Position, block.BlockDefinition, buildProgressModelMountPoints, block.Orientation, m_cacheMountPointsA))
								{
									return true;
								}
							}
						}
						else
						{
							if (value9.FatBlock != null && value9.FatBlock.CheckConnectionAllowed && !value9.FatBlock.ConnectionAllowed(ref otherBlockMinPos, ref otherBlockMaxPos, ref faceNormal, def))
							{
								continue;
							}
							if (value9.FatBlock is MyCompoundCubeBlock)
							{
								foreach (MySlimBlock block2 in (value9.FatBlock as MyCompoundCubeBlock).GetBlocks())
								{
									MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints2 = block2.BlockDefinition.GetBuildProgressModelMountPoints(block2.BuildLevelRatio);
									if (CheckNeighborMountPoints(vector5, vector6, thisMountPoint, ref result7, def, value9.Position, block2.BlockDefinition, buildProgressModelMountPoints2, block2.Orientation, m_cacheMountPointsA))
									{
										return true;
									}
								}
								continue;
							}
							float currentIntegrityRatio = 1f;
							if (value9.FatBlock != null && value9.FatBlock.SlimBlock != null)
							{
								currentIntegrityRatio = value9.FatBlock.SlimBlock.BuildLevelRatio;
							}
							MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints3 = value9.Definition.GetBuildProgressModelMountPoints(currentIntegrityRatio);
							if (CheckNeighborMountPoints(vector5, vector6, thisMountPoint, ref result7, def, value9.Position, value9.Definition, buildProgressModelMountPoints3, value9.Orientation, m_cacheMountPointsA))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
			finally
			{
				m_cacheNeighborBlocks.Clear();
			}
		}

		/// <summary>
		/// Performs check whether small cube block given by its definition, rotation  can be connected to large grid. 
		/// Function checks whether a mount point on placed block exists in opposite direction than addNomal.
		/// </summary>
		/// <param name="grid">Grid in which the check is performed.</param>
		/// <param name="def">Definition of small cube block for checking.</param>
		/// <param name="rotation">Rotation of the small cube block.</param>
		/// <param name="addNormal">Grid hit normal.</param>
		/// <returns>True when small block can be connected, otherwise false.</returns>
		public static bool CheckConnectivitySmallBlockToLargeGrid(MyCubeGrid grid, MyCubeBlockDefinition def, ref Quaternion rotation, ref Vector3I addNormal)
		{
			try
			{
				MyCubeBlockDefinition.MountPoint[] mountPoints = def.MountPoints;
				if (mountPoints == null)
				{
					return false;
				}
				for (int i = 0; i < mountPoints.Length; i++)
				{
					MyCubeBlockDefinition.MountPoint mountPoint = mountPoints[i];
					Vector3I.Transform(ref mountPoint.Normal, ref rotation, out var result);
					if (addNormal == -result)
					{
						return true;
					}
				}
				return false;
			}
			finally
			{
				m_cacheNeighborBlocks.Clear();
			}
		}

		public static bool CheckNeighborMountPoints(Vector3 currentMin, Vector3 currentMax, MyCubeBlockDefinition.MountPoint thisMountPoint, ref Vector3I thisMountPointTransformedNormal, MyCubeBlockDefinition thisDefinition, Vector3I neighborPosition, MyCubeBlockDefinition neighborDefinition, MyCubeBlockDefinition.MountPoint[] neighborMountPoints, MyBlockOrientation neighborOrientation, List<MyCubeBlockDefinition.MountPoint> otherMountPoints)
		{
			if (!thisMountPoint.Enabled)
			{
				return false;
			}
			BoundingBox boundingBox = new BoundingBox(currentMin - neighborPosition, currentMax - neighborPosition);
			TransformMountPoints(otherMountPoints, neighborDefinition, neighborMountPoints, ref neighborOrientation);
			foreach (MyCubeBlockDefinition.MountPoint otherMountPoint in otherMountPoints)
			{
				if ((((thisMountPoint.ExclusionMask & otherMountPoint.PropertiesMask) == 0 && (thisMountPoint.PropertiesMask & otherMountPoint.ExclusionMask) == 0) || !(thisDefinition.Id != neighborDefinition.Id)) && otherMountPoint.Enabled && (!MyFakes.ENABLE_TEST_BLOCK_CONNECTIVITY_CHECK || !(thisMountPointTransformedNormal + otherMountPoint.Normal != Vector3I.Zero)))
				{
					BoundingBox box = new BoundingBox(Vector3.Min(otherMountPoint.Start, otherMountPoint.End), Vector3.Max(otherMountPoint.Start, otherMountPoint.End));
					if (boundingBox.Intersects(box))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool CheckNeighborMountPointsForCompound(Vector3 currentMin, Vector3 currentMax, MyCubeBlockDefinition.MountPoint thisMountPoint, ref Vector3I thisMountPointTransformedNormal, MyCubeBlockDefinition thisDefinition, Vector3I neighborPosition, MyCubeBlockDefinition neighborDefinition, MyCubeBlockDefinition.MountPoint[] neighborMountPoints, MyBlockOrientation neighborOrientation, List<MyCubeBlockDefinition.MountPoint> otherMountPoints)
		{
			if (!thisMountPoint.Enabled)
			{
				return false;
			}
			BoundingBox boundingBox = new BoundingBox(currentMin - neighborPosition, currentMax - neighborPosition);
			TransformMountPoints(otherMountPoints, neighborDefinition, neighborMountPoints, ref neighborOrientation);
			foreach (MyCubeBlockDefinition.MountPoint otherMountPoint in otherMountPoints)
			{
				if ((((thisMountPoint.ExclusionMask & otherMountPoint.PropertiesMask) == 0 && (thisMountPoint.PropertiesMask & otherMountPoint.ExclusionMask) == 0) || !(thisDefinition.Id != neighborDefinition.Id)) && otherMountPoint.Enabled && (!MyFakes.ENABLE_TEST_BLOCK_CONNECTIVITY_CHECK || !(thisMountPointTransformedNormal - otherMountPoint.Normal != Vector3I.Zero)))
				{
					BoundingBox box = new BoundingBox(Vector3.Min(otherMountPoint.Start, otherMountPoint.End), Vector3.Max(otherMountPoint.Start, otherMountPoint.End));
					if (boundingBox.Intersects(box))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Checkes whether blocks A and B have matching mount point on one of their sides. Each block is given by its
		/// definition, rotation and position in grid. Position has to be relative to same center. Also, normal relative to block A specifies
		/// wall which is used for checking.
		/// </summary>
		public static bool CheckMountPointsForSide(MyCubeBlockDefinition defA, MyCubeBlockDefinition.MountPoint[] mountPointsA, ref MyBlockOrientation orientationA, ref Vector3I positionA, ref Vector3I normalA, MyCubeBlockDefinition defB, MyCubeBlockDefinition.MountPoint[] mountPointsB, ref MyBlockOrientation orientationB, ref Vector3I positionB)
		{
			TransformMountPoints(m_cacheMountPointsA, defA, mountPointsA, ref orientationA);
			TransformMountPoints(m_cacheMountPointsB, defB, mountPointsB, ref orientationB);
			return CheckMountPointsForSide(m_cacheMountPointsA, ref orientationA, ref positionA, defA.Id, ref normalA, m_cacheMountPointsB, ref orientationB, ref positionB, defB.Id);
		}

		/// <summary>
		/// Checkes whether blocks A and B have matching mount point on one of their sides. Each block is given by its
		/// definition, rotation and position in grid. Position has to be relative to same center. Also, normal relative to block A specifies
		/// wall which is used for checking.
		/// </summary>
		public static bool CheckMountPointsForSide(List<MyCubeBlockDefinition.MountPoint> transormedA, ref MyBlockOrientation orientationA, ref Vector3I positionA, MyDefinitionId idA, ref Vector3I normalA, List<MyCubeBlockDefinition.MountPoint> transormedB, ref MyBlockOrientation orientationB, ref Vector3I positionB, MyDefinitionId idB)
		{
			Vector3I vector3I = positionB - positionA;
			Vector3I vector3I2 = -normalA;
			for (int i = 0; i < transormedA.Count; i++)
			{
				if (!transormedA[i].Enabled)
				{
					continue;
				}
				MyCubeBlockDefinition.MountPoint mountPoint = transormedA[i];
				if (mountPoint.Normal != normalA)
				{
					continue;
				}
				Vector3 min = Vector3.Min(mountPoint.Start, mountPoint.End);
				Vector3 max = Vector3.Max(mountPoint.Start, mountPoint.End);
				min -= (Vector3)vector3I;
				max -= (Vector3)vector3I;
				BoundingBox boundingBox = new BoundingBox(min, max);
				for (int j = 0; j < transormedB.Count; j++)
				{
					if (!transormedB[j].Enabled)
					{
						continue;
					}
					MyCubeBlockDefinition.MountPoint mountPoint2 = transormedB[j];
					if (!(mountPoint2.Normal != vector3I2) && (((mountPoint.ExclusionMask & mountPoint2.PropertiesMask) == 0 && (mountPoint.PropertiesMask & mountPoint2.ExclusionMask) == 0) || !(idA != idB)))
					{
						BoundingBox box = new BoundingBox(Vector3.Min(mountPoint2.Start, mountPoint2.End), Vector3.Max(mountPoint2.Start, mountPoint2.End));
						if (boundingBox.Intersects(box))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private static void ConvertNextGrid(bool placeOnly)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(MyTexts.GetString(MyCommonTexts.ConvertingObjs)), null, null, null, null, null, delegate
			{
				ConvertNextPrefab(m_prefabs, placeOnly);
			}, 1000));
		}

		private static void ConvertNextPrefab(List<MyObjectBuilder_CubeGrid[]> prefabs, bool placeOnly)
		{
			if (prefabs.Count > 0)
			{
				MyObjectBuilder_CubeGrid[] array = prefabs[0];
				_ = prefabs.Count;
				prefabs.RemoveAt(0);
				if (placeOnly)
				{
					float radius = GetBoundingSphereForGrids(array).Radius;
					m_maxDimensionPreviousRow = MathHelper.Max(radius, m_maxDimensionPreviousRow);
					if (prefabs.Count % 4 != 0)
					{
						m_newPositionForPlacedObject.X += 2f * radius + 10f;
					}
					else
					{
						m_newPositionForPlacedObject.X = 0f - (2f * radius + 10f);
						m_newPositionForPlacedObject.Z -= 2f * m_maxDimensionPreviousRow + 30f;
						m_maxDimensionPreviousRow = 0f;
					}
					PlacePrefabToWorld(array, MySector.MainCamera.Position + m_newPositionForPlacedObject);
					ConvertNextPrefab(m_prefabs, placeOnly);
					return;
				}
				List<MyCubeGrid> list = new List<MyCubeGrid>();
				MyObjectBuilder_CubeGrid[] array2 = array;
				foreach (MyObjectBuilder_CubeGrid objectBuilder in array2)
				{
					list.Add(MyEntities.CreateFromObjectBuilderAndAdd(objectBuilder, fadeIn: false) as MyCubeGrid);
				}
				ExportToObjFile(list, convertModelsFromSBC: true, exportObjAndSBC: false);
				foreach (MyCubeGrid item in list)
				{
					item.Close();
				}
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, new StringBuilder(MyTexts.GetString(MyCommonTexts.ConvertToObjDone))));
			}
		}

		private static BoundingSphere GetBoundingSphereForGrids(MyObjectBuilder_CubeGrid[] currentPrefab)
		{
			BoundingSphere result = new BoundingSphere(Vector3.Zero, float.MinValue);
			foreach (MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid in currentPrefab)
			{
				BoundingSphere boundingSphere = myObjectBuilder_CubeGrid.CalculateBoundingSphere();
				MatrixD m = (myObjectBuilder_CubeGrid.PositionAndOrientation.HasValue ? myObjectBuilder_CubeGrid.PositionAndOrientation.Value.GetMatrix() : MatrixD.Identity);
				result.Include(boundingSphere.Transform(m));
			}
			return result;
		}

		public static void StartConverting(bool placeOnly)
		{
<<<<<<< HEAD
			string path = Path.Combine(MyFileSystem.UserDataPath, "SourceModels");
			if (!Directory.Exists(path))
			{
				return;
			}
			m_prefabs.Clear();
			string[] files = Directory.GetFiles(path, "*.zip");
			for (int i = 0; i < files.Length; i++)
			{
=======
			string text = Path.Combine(MyFileSystem.UserDataPath, "SourceModels");
			if (!Directory.Exists(text))
			{
				return;
			}
			m_prefabs.Clear();
			string[] files = Directory.GetFiles(text, "*.zip");
			for (int i = 0; i < files.Length; i++)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				foreach (string file in MyFileSystem.GetFiles(files[i], "*.sbc", MySearchOption.AllDirectories))
				{
					if (MyFileSystem.FileExists(file))
					{
						MyObjectBuilder_Definitions objectBuilder = null;
						MyObjectBuilderSerializer.DeserializeXML(file, out objectBuilder);
						if (objectBuilder.Prefabs[0].CubeGrids != null)
						{
							m_prefabs.Add(objectBuilder.Prefabs[0].CubeGrids);
						}
					}
				}
			}
			ConvertNextPrefab(m_prefabs, placeOnly);
		}

		public static void ConvertPrefabsToObjs()
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(MyTexts.GetString(MyCommonTexts.ConvertingObjs)), null, null, null, null, null, delegate
			{
				StartConverting(placeOnly: false);
			}, 1000));
		}

		public static void PackFiles(string path, string objectName)
		{
			if (!Directory.Exists(path))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjFailed), path))));
				return;
			}
			using (MyZipArchive arc = MyZipArchive.OpenOnFile(Path.Combine(path, objectName + "_objFiles.zip"), ZipArchiveMode.Create))
			{
				PackFilesToDirectory(path, "*.png", arc);
				PackFilesToDirectory(path, "*.obj", arc);
				PackFilesToDirectory(path, "*.mtl", arc);
			}
			using (MyZipArchive arc2 = MyZipArchive.OpenOnFile(Path.Combine(path, objectName + ".zip"), ZipArchiveMode.Create))
			{
				PackFilesToDirectory(path, objectName + ".png", arc2);
				PackFilesToDirectory(path, "*.sbc", arc2);
			}
			RemoveFilesFromDirectory(path, "*.png");
			RemoveFilesFromDirectory(path, "*.sbc");
			RemoveFilesFromDirectory(path, "*.obj");
			RemoveFilesFromDirectory(path, "*.mtl");
		}

		private static void RemoveFilesFromDirectory(string path, string fileType)
		{
			string[] files = Directory.GetFiles(path, fileType);
			for (int i = 0; i < files.Length; i++)
			{
				File.Delete(files[i]);
			}
		}

		private static void PackFilesToDirectory(string path, string searchString, MyZipArchive arc)
		{
			int startIndex = path.Length + 1;
			string[] files = Directory.GetFiles(path, searchString, (SearchOption)1);
			foreach (string text in files)
			{
				using FileStream fileStream = File.Open(text, FileMode.Open, FileAccess.Read, FileShare.Read);
				using Stream destination = arc.AddFile(text.Substring(startIndex), (CompressionLevel)0).GetStream();
				fileStream.CopyTo(destination, 4096);
			}
		}

		public static void ExportObject(MyCubeGrid baseGrid, bool convertModelsFromSBC, bool exportObjAndSBC = false)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(MyTexts.GetString(MyCommonTexts.ExportingToObj)), null, null, null, null, null, delegate
			{
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				List<MyCubeGrid> list = new List<MyCubeGrid>();
				Enumerator<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node> enumerator = MyCubeGridGroups.Static.Logical.GetGroup(baseGrid).Nodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node current = enumerator.get_Current();
						list.Add(current.NodeData);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				ExportToObjFile(list, convertModelsFromSBC, exportObjAndSBC);
			}, 1000));
		}

		private static void ExportToObjFile(List<MyCubeGrid> baseGrids, bool convertModelsFromSBC, bool exportObjAndSBC)
		{
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			materialID = 0;
			MyValueFormatter.GetFormatedDateTimeForFilename(DateTime.Now);
			string name = MyUtils.StripInvalidChars(baseGrids[0].DisplayName.Replace(' ', '_'));
			string path = MyFileSystem.UserDataPath;
			string path2 = "ExportedModels";
			if (!convertModelsFromSBC || exportObjAndSBC)
			{
				path = Environment.GetFolderPath((SpecialFolder)0);
				path2 = MyPerGameSettings.GameNameSafe + "_ExportedModels";
			}
			string folder = Path.Combine(path, path2, name);
			int num = 0;
			while (Directory.Exists(folder))
			{
				num++;
				folder = Path.Combine(path, path2, $"{name}_{num:000}");
			}
			MyUtils.CreateFolder(folder);
			if (!convertModelsFromSBC || exportObjAndSBC)
			{
				bool flag = false;
				string prefabPath = Path.Combine(folder, name + ".sbc");
				foreach (MyCubeGrid baseGrid in baseGrids)
				{
					Enumerator<MySlimBlock> enumerator2 = baseGrid.CubeBlocks.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							if (!enumerator2.get_Current().BlockDefinition.Context.IsBaseGame)
							{
								flag = true;
								break;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
				if (!flag)
				{
					CreatePrefabFile(baseGrids, name, prefabPath);
					MyRenderProxy.TakeScreenshot(tumbnailMultiplier, Path.Combine(folder, name + ".png"), debug: false, ignoreSprites: true, showNotification: false);
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjComplete), folder)), null, null, null, null, null, delegate
					{
						PackFiles(folder, name);
					}));
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjModded), folder))));
				}
			}
			if (!(exportObjAndSBC || convertModelsFromSBC))
			{
				return;
			}
			List<Vector3> vertices = new List<Vector3>();
			List<TriangleWithMaterial> triangles = new List<TriangleWithMaterial>();
			List<Vector2> uvs = new List<Vector2>();
			List<MyExportModel.Material> materials = new List<MyExportModel.Material>();
			int currVerticesCount = 0;
			try
			{
				GetModelDataFromGrid(baseGrids, vertices, triangles, uvs, materials, currVerticesCount);
				string filename = Path.Combine(folder, name + ".obj");
				string matFilename = Path.Combine(folder, name + ".mtl");
				CreateObjFile(name, filename, matFilename, vertices, triangles, uvs, materials, currVerticesCount);
				List<renderColoredTextureProperties> list = new List<renderColoredTextureProperties>();
				CreateMaterialFile(folder, matFilename, materials, list);
				if (list.Count > 0)
				{
					MyRenderProxy.RenderColoredTextures(list);
				}
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjComplete), folder)), null, null, null, null, null, delegate
				{
					ConvertNextGrid(placeOnly: false);
				}, 1000));
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine("Error while exporting to obj file.");
				MySandboxGame.Log.WriteLine(ex.ToString());
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjFailed), folder))));
			}
		}

		private static void CreatePrefabFile(List<MyCubeGrid> baseGrid, string name, string prefabPath)
		{
			Vector2I backBufferResolution = MyRenderProxy.BackBufferResolution;
			tumbnailMultiplier.X = 400f / (float)backBufferResolution.X;
			tumbnailMultiplier.Y = 400f / (float)backBufferResolution.Y;
			List<MyObjectBuilder_CubeGrid> list = new List<MyObjectBuilder_CubeGrid>();
			foreach (MyCubeGrid item in baseGrid)
			{
				list.Add((MyObjectBuilder_CubeGrid)item.GetObjectBuilder());
			}
			MyPrefabManager.SavePrefabToPath(name, prefabPath, list);
		}

		private static void GetModelDataFromGrid(List<MyCubeGrid> baseGrid, List<Vector3> vertices, List<TriangleWithMaterial> triangles, List<Vector2> uvs, List<MyExportModel.Material> materials, int currVerticesCount)
		{
<<<<<<< HEAD
=======
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MatrixD matrixD = MatrixD.Invert(baseGrid[0].WorldMatrix);
			foreach (MyCubeGrid item in baseGrid)
			{
				MatrixD m = item.WorldMatrix * matrixD;
				foreach (KeyValuePair<Vector3I, MyCubeGridRenderCell> cell in item.RenderData.Cells)
				{
					foreach (KeyValuePair<MyCubePart, ConcurrentDictionary<uint, bool>> cubePart in cell.Value.CubeParts)
					{
						MyCubePart key = cubePart.Key;
						Vector3 colorMaskHSV = new Vector3(key.InstanceData.ColorMaskHSV.X, key.InstanceData.ColorMaskHSV.Y, key.InstanceData.ColorMaskHSV.Z);
						Vector2 offsetUV = new Vector2(key.InstanceData.GetTextureOffset(0), key.InstanceData.GetTextureOffset(1));
						ExtractModelDataForObj(key.Model, key.InstanceData.LocalMatrix * (Matrix)m, vertices, triangles, uvs, ref offsetUV, materials, ref currVerticesCount, colorMaskHSV);
					}
				}
				Enumerator<MySlimBlock> enumerator4 = item.GetBlocks().GetEnumerator();
				try
				{
<<<<<<< HEAD
					if (block.FatBlock == null)
					{
						continue;
					}
					MatrixD m2;
					if (block.FatBlock is MyPistonBase)
					{
						block.FatBlock.UpdateOnceBeforeFrame();
					}
					else if (block.FatBlock is MyCompoundCubeBlock)
					{
						foreach (MySlimBlock block2 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
						{
							MyModel model = block2.FatBlock.Model;
							m2 = block2.FatBlock.PositionComp.WorldMatrixRef * matrixD;
							ExtractModelDataForObj(model, m2, vertices, triangles, uvs, ref Vector2.Zero, materials, ref currVerticesCount, block2.ColorMaskHSV);
							m2 = block2.FatBlock.PositionComp.WorldMatrixRef * matrixD;
							ProcessChildrens(vertices, triangles, uvs, materials, ref currVerticesCount, m2, block2.ColorMaskHSV, block2.FatBlock.Hierarchy.Children);
						}
						continue;
=======
					while (enumerator4.MoveNext())
					{
						MySlimBlock current2 = enumerator4.get_Current();
						if (current2.FatBlock == null)
						{
							continue;
						}
						MatrixD m2;
						if (current2.FatBlock is MyPistonBase)
						{
							current2.FatBlock.UpdateOnceBeforeFrame();
						}
						else if (current2.FatBlock is MyCompoundCubeBlock)
						{
							foreach (MySlimBlock block in (current2.FatBlock as MyCompoundCubeBlock).GetBlocks())
							{
								MyModel model = block.FatBlock.Model;
								m2 = block.FatBlock.PositionComp.WorldMatrixRef * matrixD;
								ExtractModelDataForObj(model, m2, vertices, triangles, uvs, ref Vector2.Zero, materials, ref currVerticesCount, block.ColorMaskHSV);
								m2 = block.FatBlock.PositionComp.WorldMatrixRef * matrixD;
								ProcessChildrens(vertices, triangles, uvs, materials, ref currVerticesCount, m2, block.ColorMaskHSV, block.FatBlock.Hierarchy.Children);
							}
							continue;
						}
						MyModel model2 = current2.FatBlock.Model;
						m2 = current2.FatBlock.PositionComp.WorldMatrixRef * matrixD;
						ExtractModelDataForObj(model2, m2, vertices, triangles, uvs, ref Vector2.Zero, materials, ref currVerticesCount, current2.ColorMaskHSV);
						m2 = current2.FatBlock.PositionComp.WorldMatrixRef * matrixD;
						ProcessChildrens(vertices, triangles, uvs, materials, ref currVerticesCount, m2, current2.ColorMaskHSV, current2.FatBlock.Hierarchy.Children);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					MyModel model2 = block.FatBlock.Model;
					m2 = block.FatBlock.PositionComp.WorldMatrixRef * matrixD;
					ExtractModelDataForObj(model2, m2, vertices, triangles, uvs, ref Vector2.Zero, materials, ref currVerticesCount, block.ColorMaskHSV);
					m2 = block.FatBlock.PositionComp.WorldMatrixRef * matrixD;
					ProcessChildrens(vertices, triangles, uvs, materials, ref currVerticesCount, m2, block.ColorMaskHSV, block.FatBlock.Hierarchy.Children);
				}
				finally
				{
					((IDisposable)enumerator4).Dispose();
				}
			}
		}

		private static void CreateObjFile(string name, string filename, string matFilename, List<Vector3> vertices, List<TriangleWithMaterial> triangles, List<Vector2> uvs, List<MyExportModel.Material> materials, int currVerticesCount)
		{
			using StreamWriter streamWriter = new StreamWriter(filename);
			streamWriter.WriteLine("mtllib {0}", Path.GetFileName(matFilename));
			streamWriter.WriteLine();
			streamWriter.WriteLine("#");
			streamWriter.WriteLine("# {0}", name);
			streamWriter.WriteLine("#");
			streamWriter.WriteLine();
			streamWriter.WriteLine("# vertices");
			List<int> list = new List<int>(vertices.Count);
			Dictionary<Vector3D, int> dictionary = new Dictionary<Vector3D, int>(vertices.Count / 5);
			int num = 1;
			foreach (Vector3 vertex in vertices)
			{
				if (!dictionary.TryGetValue(vertex, out var value))
				{
					value = num++;
					dictionary.Add(vertex, value);
					streamWriter.WriteLine("v {0} {1} {2}", vertex.X, vertex.Y, vertex.Z);
				}
				list.Add(value);
			}
			dictionary = null;
			List<int> list2 = new List<int>(vertices.Count);
			Dictionary<Vector2, int> dictionary2 = new Dictionary<Vector2, int>(vertices.Count / 5);
			streamWriter.WriteLine("# {0} vertices", vertices.Count);
			streamWriter.WriteLine();
			streamWriter.WriteLine("# texture coordinates");
			num = 1;
			foreach (Vector2 uv in uvs)
			{
				if (!dictionary2.TryGetValue(uv, out var value2))
				{
					value2 = num++;
					dictionary2.Add(uv, value2);
					streamWriter.WriteLine("vt {0} {1}", uv.X, uv.Y);
				}
				list2.Add(value2);
			}
			dictionary2 = null;
			streamWriter.WriteLine("# {0} texture coords", uvs.Count);
			streamWriter.WriteLine();
			streamWriter.WriteLine("# faces");
			streamWriter.WriteLine("o {0}", name);
			int num2 = 0;
			foreach (MyExportModel.Material material in materials)
			{
<<<<<<< HEAD
				streamWriter.WriteLine("mtllib {0}", Path.GetFileName(matFilename));
				streamWriter.WriteLine();
				streamWriter.WriteLine("#");
				streamWriter.WriteLine("# {0}", name);
				streamWriter.WriteLine("#");
				streamWriter.WriteLine();
				streamWriter.WriteLine("# vertices");
				List<int> list = new List<int>(vertices.Count);
				Dictionary<Vector3D, int> dictionary = new Dictionary<Vector3D, int>(vertices.Count / 5);
				int num = 1;
				foreach (Vector3 vertex in vertices)
				{
					if (!dictionary.TryGetValue(vertex, out var value))
					{
						value = num++;
						dictionary.Add(vertex, value);
						streamWriter.WriteLine("v {0} {1} {2}", vertex.X, vertex.Y, vertex.Z);
					}
					list.Add(value);
				}
				dictionary = null;
				List<int> list2 = new List<int>(vertices.Count);
				Dictionary<Vector2, int> dictionary2 = new Dictionary<Vector2, int>(vertices.Count / 5);
				streamWriter.WriteLine("# {0} vertices", vertices.Count);
=======
				num2++;
				string exportedMaterialName = material.ExportedMaterialName;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				streamWriter.WriteLine();
				streamWriter.WriteLine("g {0}_part{1}", name, num2);
				streamWriter.WriteLine("usemtl {0}", exportedMaterialName);
				streamWriter.WriteLine("s off");
				for (int i = 0; i < triangles.Count; i++)
				{
<<<<<<< HEAD
					if (!dictionary2.TryGetValue(uv, out var value2))
=======
					if (exportedMaterialName == triangles[i].material)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						TriangleWithMaterial triangleWithMaterial = triangles[i];
						MyTriangleVertexIndices triangle = triangleWithMaterial.triangle;
						MyTriangleVertexIndices uvIndices = triangleWithMaterial.uvIndices;
						streamWriter.WriteLine("f {0}/{3} {1}/{4} {2}/{5}", list[triangle.I0 - 1], list[triangle.I1 - 1], list[triangle.I2 - 1], list2[uvIndices.I0 - 1], list2[uvIndices.I1 - 1], list2[uvIndices.I2 - 1]);
					}
				}
			}
			streamWriter.WriteLine("# {0} faces", triangles.Count);
		}

		private static void CreateMaterialFile(string folder, string matFilename, List<MyExportModel.Material> materials, List<renderColoredTextureProperties> texturesToRender)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			using StreamWriter streamWriter = new StreamWriter(matFilename);
			foreach (MyExportModel.Material material in materials)
			{
				string exportedMaterialName = material.ExportedMaterialName;
				streamWriter.WriteLine("newmtl {0}", exportedMaterialName);
				if (MyFakes.ENABLE_EXPORT_MTL_DIAGNOSTICS)
				{
					Vector3 colorMaskHSV = material.ColorMaskHSV;
					streamWriter.WriteLine("# HSV Mask: {0}", colorMaskHSV.ToString("F2"));
					streamWriter.WriteLine("# IsGlass: {0}", material.IsGlass);
					streamWriter.WriteLine("# AddMapsMap: {0}", material.AddMapsTexture ?? "Null");
					streamWriter.WriteLine("# AlphamaskMap: {0}", material.AlphamaskTexture ?? "Null");
					streamWriter.WriteLine("# ColorMetalMap: {0}", material.ColorMetalTexture ?? "Null");
					streamWriter.WriteLine("# NormalGlossMap: {0}", material.NormalGlossTexture ?? "Null");
				}
				if (!material.IsGlass)
				{
					streamWriter.WriteLine("Ka 1.000 1.000 1.000");
					streamWriter.WriteLine("Kd 1.000 1.000 1.000");
					streamWriter.WriteLine("Ks 0.100 0.100 0.100");
					streamWriter.WriteLine((material.AlphamaskTexture == null) ? "d 1.0" : "d 0.0");
				}
				else
				{
					streamWriter.WriteLine("Ka 0.000 0.000 0.000");
					streamWriter.WriteLine("Kd 0.000 0.000 0.000");
					streamWriter.WriteLine("Ks 0.900 0.900 0.900");
					streamWriter.WriteLine("d 0.350");
				}
				streamWriter.WriteLine("Ns 95.00");
				streamWriter.WriteLine("illum 2");
				if (material.ColorMetalTexture != null)
				{
					string format = exportedMaterialName + "_{0}.png";
					string text = string.Format(format, "ca");
					string text2 = string.Format(format, "ng");
					streamWriter.WriteLine("map_Ka {0}", text);
					streamWriter.WriteLine("map_Kd {0}", text);
					if (material.AlphamaskTexture != null)
					{
						streamWriter.WriteLine("map_d {0}", text);
					}
					bool flag = false;
					if (material.NormalGlossTexture != null)
					{
						if (dictionary.TryGetValue(material.NormalGlossTexture, out var value))
						{
							text2 = value;
						}
						else
						{
<<<<<<< HEAD
							if (dictionary.TryGetValue(material.NormalGlossTexture, out var value))
							{
								text2 = value;
							}
							else
							{
								flag = true;
								dictionary.Add(material.NormalGlossTexture, text2);
							}
							streamWriter.WriteLine("map_Bump {0}", text2);
=======
							flag = true;
							dictionary.Add(material.NormalGlossTexture, text2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						streamWriter.WriteLine("map_Bump {0}", text2);
					}
					texturesToRender.Add(new renderColoredTextureProperties
					{
						ColorMaskHSV = material.ColorMaskHSV,
						TextureAddMaps = material.AddMapsTexture,
						TextureAplhaMask = material.AlphamaskTexture,
						TextureColorMetal = material.ColorMetalTexture,
						TextureNormalGloss = (flag ? material.NormalGlossTexture : null),
						PathToSave_ColorAlpha = Path.Combine(folder, text),
						PathToSave_NormalGloss = Path.Combine(folder, text2)
					});
				}
				streamWriter.WriteLine();
			}
		}

		private static void ProcessChildrens(List<Vector3> vertices, List<TriangleWithMaterial> triangles, List<Vector2> uvs, List<MyExportModel.Material> materials, ref int currVerticesCount, Matrix parentMatrix, Vector3 HSV, ListReader<MyHierarchyComponentBase> childrens)
		{
			foreach (MyHierarchyComponentBase item in childrens)
			{
				VRage.ModAPI.IMyEntity entity = item.Container.Entity;
				MyModel model = (entity as MyEntity).Model;
				if (model != null)
				{
					ExtractModelDataForObj(model, entity.LocalMatrix * parentMatrix, vertices, triangles, uvs, ref Vector2.Zero, materials, ref currVerticesCount, HSV);
				}
				ProcessChildrens(vertices, triangles, uvs, materials, ref currVerticesCount, entity.LocalMatrix * parentMatrix, HSV, entity.Hierarchy.Children);
			}
		}

		public static void PlacePrefabsToWorld()
		{
			m_newPositionForPlacedObject = MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition();
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(MyTexts.GetString(MyCommonTexts.PlacingObjectsToScene)), null, null, null, null, null, delegate
			{
				StartConverting(placeOnly: true);
			}, 1000));
		}

		public static void PlacePrefabToWorld(MyObjectBuilder_CubeGrid[] currentPrefab, Vector3D position, List<MyCubeGrid> createdGrids = null)
		{
			Vector3D vector3D = Vector3D.Zero;
			Vector3D vector3D2 = Vector3D.Zero;
			bool flag = true;
			MyEntities.RemapObjectBuilderCollection(currentPrefab);
			foreach (MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid in currentPrefab)
			{
				if (myObjectBuilder_CubeGrid.PositionAndOrientation.HasValue)
				{
					if (flag)
					{
						vector3D2 = position - myObjectBuilder_CubeGrid.PositionAndOrientation.Value.Position;
						flag = false;
						vector3D = position;
					}
					else
					{
						vector3D = myObjectBuilder_CubeGrid.PositionAndOrientation.Value.Position + vector3D2;
					}
				}
				MyPositionAndOrientation value = myObjectBuilder_CubeGrid.PositionAndOrientation.Value;
				value.Position = vector3D;
				myObjectBuilder_CubeGrid.PositionAndOrientation = value;
				MyCubeGrid myCubeGrid = MyEntities.CreateFromObjectBuilder(myObjectBuilder_CubeGrid, fadeIn: false) as MyCubeGrid;
				if (myCubeGrid != null)
				{
					myCubeGrid.ClearSymmetries();
					myCubeGrid.Physics.LinearVelocity = Vector3.Zero;
					myCubeGrid.Physics.AngularVelocity = Vector3.Zero;
					createdGrids?.Add(myCubeGrid);
					MyEntities.Add(myCubeGrid);
				}
			}
		}

		/// <summary>
		/// Obtain grid that player is aiming/looking at.
		/// </summary>
		public static MyCubeGrid GetTargetGrid()
		{
			MyEntity myEntity = MyCubeBuilder.Static.FindClosestGrid();
			if (myEntity == null)
			{
				myEntity = GetTargetEntity();
			}
			return myEntity as MyCubeGrid;
		}

		/// <summary>
		/// Obtain entity that player is aiming/looking at.
		/// </summary>
		public static MyEntity GetTargetEntity()
		{
			LineD ray = new LineD(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 10000f);
			m_tmpHitList.AssertEmpty();
			try
			{
				MyPhysics.CastRay(ray.From, ray.To, m_tmpHitList, 15);
				m_tmpHitList.RemoveAll((MyPhysics.HitInfo hit) => MySession.Static.ControlledEntity != null && hit.HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity);
				if (m_tmpHitList.Count == 0)
				{
					using (MyUtils.ReuseCollection(ref m_lineOverlapList))
					{
						MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref ray, m_lineOverlapList);
						if (m_lineOverlapList.Count > 0)
						{
							return m_lineOverlapList[0].Element.GetTopMostParent();
						}
						return null;
					}
				}
				return m_tmpHitList[0].HkHitInfo.GetHitEntity() as MyEntity;
			}
			finally
			{
				m_tmpHitList.Clear();
			}
		}

		public static bool TryRayCastGrid(ref LineD worldRay, out MyCubeGrid hitGrid, out Vector3D worldHitPos)
		{
			try
			{
				MyPhysics.CastRay(worldRay.From, worldRay.To, m_tmpHitList);
				foreach (MyPhysics.HitInfo tmpHit in m_tmpHitList)
				{
					MyCubeGrid myCubeGrid = tmpHit.HkHitInfo.GetHitEntity() as MyCubeGrid;
					if (myCubeGrid != null)
					{
						worldHitPos = tmpHit.Position;
						MyRenderProxy.DebugDrawAABB(new BoundingBoxD(worldHitPos - 0.01, worldHitPos + 0.01), Color.Wheat.ToVector3());
						hitGrid = myCubeGrid;
						return true;
					}
				}
				hitGrid = null;
				worldHitPos = default(Vector3D);
				return false;
			}
			finally
			{
				m_tmpHitList.Clear();
			}
		}

		public static bool TestBlockPlacementArea(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, MyBlockOrientation blockOrientation, MyCubeBlockDefinition blockDefinition, ref Vector3D translation, ref Quaternion rotation, ref Vector3 halfExtents, ref BoundingBoxD localAabb, ulong placingPlayer = 0uL, MyEntity ignoredEntity = null, bool isProjected = false)
		{
			MyCubeGrid touchingGrid;
			return TestBlockPlacementArea(targetGrid, ref settings, blockOrientation, blockDefinition, ref translation, ref rotation, ref halfExtents, ref localAabb, out touchingGrid, placingPlayer, ignoredEntity, ignoreFracturedPieces: false, testVoxel: true, isProjected);
		}

<<<<<<< HEAD
		public static bool TestBlockPlacementArea(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, MyBlockOrientation blockOrientation, MyCubeBlockDefinition blockDefinition, ref Vector3D translationObsolete, ref Quaternion rotation, ref Vector3 halfExtentsObsolete, ref BoundingBoxD localAabb, out MyCubeGrid touchingGrid, ulong placingPlayer = 0uL, MyEntity ignoredEntity = null, bool ignoreFracturedPieces = false, bool testVoxel = true, bool isProjected = false, bool forceCheck = false)
=======
		public static bool TestBlockPlacementArea(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, MyBlockOrientation blockOrientation, MyCubeBlockDefinition blockDefinition, ref Vector3D translationObsolete, ref Quaternion rotation, ref Vector3 halfExtentsObsolete, ref BoundingBoxD localAabb, out MyCubeGrid touchingGrid, ulong placingPlayer = 0uL, MyEntity ignoredEntity = null, bool ignoreFracturedPieces = false, bool testVoxel = true, bool isProjected = false)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			touchingGrid = null;
			MatrixD m = targetGrid?.WorldMatrix ?? MatrixD.Identity;
			if (!MyEntities.IsInsideWorld(m.Translation))
			{
				return false;
			}
			Vector3 halfExtents = localAabb.HalfExtents;
			halfExtents += settings.SearchHalfExtentsDeltaAbsolute;
			if (MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA)
			{
				halfExtents -= new Vector3(0.11f);
			}
			Vector3D translation = localAabb.TransformFast(ref m).Center;
			Quaternion.CreateFromRotationMatrix(in m).Normalize();
			if (testVoxel && settings.VoxelPlacement.HasValue && settings.VoxelPlacement.Value.PlacementMode != VoxelPlacementMode.Both)
			{
				bool flag = IsAabbInsideVoxel(m, localAabb, settings);
				if (settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.InVoxel)
				{
					flag = !flag;
				}
				if (flag)
				{
					return false;
				}
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(localAabb.TransformFast(ref m), isProjected ? MySafeZoneAction.BuildingProjections : MySafeZoneAction.Building, 0L, placingPlayer))
			{
				return false;
			}
			if (blockDefinition != null && blockDefinition.UseModelIntersection)
			{
				MyModel modelOnlyData = MyModels.GetModelOnlyData(blockDefinition.Model);
				if (modelOnlyData != null)
				{
					modelOnlyData.CheckLoadingErrors(blockDefinition.Context, out var errorFound);
					if (errorFound)
					{
						MyDefinitionErrors.Add(blockDefinition.Context, "There was error during loading of model, please check log file.", TErrorSeverity.Error);
					}
				}
				if (modelOnlyData != null && modelOnlyData.HavokCollisionShapes != null)
				{
					blockOrientation.GetMatrix(out var result);
					Vector3.TransformNormal(ref blockDefinition.ModelOffset, ref result, out var result2);
					translation += result2;
					int num = modelOnlyData.HavokCollisionShapes.Length;
					HkShape[] array = new HkShape[num];
					for (int i = 0; i < num; i++)
					{
						array[i] = modelOnlyData.HavokCollisionShapes[i];
					}
					HkListShape hkListShape = new HkListShape(array, num, HkReferencePolicy.None);
					Quaternion quaternion = Quaternion.CreateFromForwardUp(Base6Directions.GetVector(blockOrientation.Forward), Base6Directions.GetVector(blockOrientation.Up));
					rotation *= quaternion;
					MyPhysics.GetPenetrationsShape(hkListShape, ref translation, ref rotation, m_physicsBoxQueryList, 7);
					hkListShape.Base.RemoveReference();
				}
				else
				{
					MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref rotation, m_physicsBoxQueryList, 7);
				}
			}
			else
			{
				MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref rotation, m_physicsBoxQueryList, 7);
			}
			m_lastQueryBox.Value.HalfExtents = halfExtents;
			m_lastQueryTransform = MatrixD.CreateFromQuaternion(rotation);
			m_lastQueryTransform.Translation = translation;
			return TestPlacementAreaInternal(targetGrid, ref settings, blockDefinition, blockOrientation, ref localAabb, ignoredEntity, ref m, out touchingGrid, dynamicBuildMode: false, ignoreFracturedPieces, forceCheck);
		}

<<<<<<< HEAD
		public static bool TestPlacementAreaCube(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, Vector3I min, Vector3I max, MyBlockOrientation blockOrientation, MyCubeBlockDefinition blockDefinition, ulong placingPlayer = 0uL, MyEntity ignoredEntity = null, bool ignoreFracturedPieces = false, bool isProjected = false, bool forceCheck = false)
		{
			MyCubeGrid touchingGrid = null;
			return TestPlacementAreaCube(targetGrid, ref settings, min, max, blockOrientation, blockDefinition, out touchingGrid, placingPlayer, ignoredEntity, ignoreFracturedPieces, isProjected, forceCheck);
		}

		/// <summary>
		/// Test cube block placement area in grid.
		/// </summary>
		public static bool TestPlacementAreaCube(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, Vector3I min, Vector3I max, MyBlockOrientation blockOrientation, MyCubeBlockDefinition blockDefinition, out MyCubeGrid touchingGrid, ulong placingPlayer = 0uL, MyEntity ignoredEntity = null, bool ignoreFracturedPieces = false, bool isProjected = false, bool forceCheck = false)
=======
		public static bool TestPlacementAreaCube(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, Vector3I min, Vector3I max, MyBlockOrientation blockOrientation, MyCubeBlockDefinition blockDefinition, ulong placingPlayer = 0uL, MyEntity ignoredEntity = null, bool ignoreFracturedPieces = false, bool isProjected = false)
		{
			MyCubeGrid touchingGrid = null;
			return TestPlacementAreaCube(targetGrid, ref settings, min, max, blockOrientation, blockDefinition, out touchingGrid, placingPlayer, ignoredEntity, ignoreFracturedPieces, isProjected);
		}

		public static bool TestPlacementAreaCube(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, Vector3I min, Vector3I max, MyBlockOrientation blockOrientation, MyCubeBlockDefinition blockDefinition, out MyCubeGrid touchingGrid, ulong placingPlayer = 0uL, MyEntity ignoredEntity = null, bool ignoreFracturedPieces = false, bool isProjected = false)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			touchingGrid = null;
			MatrixD matrixD = targetGrid?.WorldMatrix ?? MatrixD.Identity;
			if (!MyEntities.IsInsideWorld(matrixD.Translation))
			{
				return false;
			}
			float num = targetGrid?.GridSize ?? MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large);
			Vector3 halfExtentsObsolete = ((max - min) * num + num) / 2f;
			if (MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA)
			{
				halfExtentsObsolete -= new Vector3(0.11f);
			}
			else
			{
				halfExtentsObsolete -= new Vector3(0.03f, 0.03f, 0.03f);
			}
			MatrixD matrix = MatrixD.CreateTranslation((max + min) * 0.5f * num) * matrixD;
			BoundingBoxD localAabb = BoundingBoxD.CreateInvalid();
			localAabb.Include(min * num - num / 2f);
			localAabb.Include(max * num + num / 2f);
			Vector3D translationObsolete = matrix.Translation;
			Quaternion rotation = Quaternion.CreateFromRotationMatrix(in matrix);
<<<<<<< HEAD
			return TestBlockPlacementArea(targetGrid, ref settings, blockOrientation, blockDefinition, ref translationObsolete, ref rotation, ref halfExtentsObsolete, ref localAabb, out touchingGrid, placingPlayer, ignoredEntity, ignoreFracturedPieces, testVoxel: true, isProjected, forceCheck);
=======
			return TestBlockPlacementArea(targetGrid, ref settings, blockOrientation, blockDefinition, ref translationObsolete, ref rotation, ref halfExtentsObsolete, ref localAabb, out touchingGrid, placingPlayer, ignoredEntity, ignoreFracturedPieces, testVoxel: true, isProjected);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static bool TestPlacementAreaCubeNoAABBInflate(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, Vector3I min, Vector3I max, MyBlockOrientation blockOrientation, MyCubeBlockDefinition blockDefinition, out MyCubeGrid touchingGrid, ulong placingPlayer = 0uL, MyEntity ignoredEntity = null, bool isProjected = false)
		{
			touchingGrid = null;
			MatrixD matrixD = targetGrid?.WorldMatrix ?? MatrixD.Identity;
			if (!MyEntities.IsInsideWorld(matrixD.Translation))
			{
				return false;
			}
			float num = targetGrid?.GridSize ?? MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large);
			Vector3 halfExtentsObsolete = ((max - min) * num + num) / 2f;
			MatrixD matrix = MatrixD.CreateTranslation((max + min) * 0.5f * num) * matrixD;
			BoundingBoxD localAabb = BoundingBoxD.CreateInvalid();
			localAabb.Include(min * num - num / 2f);
			localAabb.Include(max * num + num / 2f);
			Vector3D translationObsolete = matrix.Translation;
			Quaternion rotation = Quaternion.CreateFromRotationMatrix(in matrix);
			return TestBlockPlacementArea(targetGrid, ref settings, blockOrientation, blockDefinition, ref translationObsolete, ref rotation, ref halfExtentsObsolete, ref localAabb, out touchingGrid, placingPlayer, ignoredEntity, ignoreFracturedPieces: false, testVoxel: true, isProjected);
		}

		public static bool TestPlacementArea(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, BoundingBoxD localAabb, bool dynamicBuildMode, MyEntity ignoredEntity = null)
		{
			MatrixD m = targetGrid.WorldMatrix;
			if (!MyEntities.IsInsideWorld(m.Translation))
			{
				return false;
			}
			Vector3 halfExtents = localAabb.HalfExtents;
			halfExtents += settings.SearchHalfExtentsDeltaAbsolute;
			if (MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA)
			{
				halfExtents -= new Vector3(0.11f);
			}
			Vector3D translation = localAabb.TransformFast(ref m).Center;
			Quaternion rotation = Quaternion.CreateFromRotationMatrix(in m);
			rotation.Normalize();
			MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref rotation, m_physicsBoxQueryList, 18);
			m_lastQueryBox.Value.HalfExtents = halfExtents;
			m_lastQueryTransform = MatrixD.CreateFromQuaternion(rotation);
			m_lastQueryTransform.Translation = translation;
			MyCubeGrid touchingGrid;
			return TestPlacementAreaInternal(targetGrid, ref settings, null, null, ref localAabb, ignoredEntity, ref m, out touchingGrid, dynamicBuildMode);
		}

		public static bool TestPlacementArea(MyCubeGrid targetGrid, bool targetGridIsStatic, ref MyGridPlacementSettings settings, BoundingBoxD localAabb, bool dynamicBuildMode, MyEntity ignoredEntity = null, bool testVoxel = true, bool testPhysics = true)
		{
			MatrixD m = targetGrid.WorldMatrix;
			if (!MyEntities.IsInsideWorld(m.Translation))
			{
				return false;
			}
			Vector3 halfExtents = localAabb.HalfExtents;
			halfExtents += settings.SearchHalfExtentsDeltaAbsolute;
			if (MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA)
			{
				halfExtents -= new Vector3(0.11f);
			}
			Vector3D translation = localAabb.TransformFast(ref m).Center;
			Quaternion rotation = Quaternion.CreateFromRotationMatrix(in m);
			rotation.Normalize();
			if (testVoxel && settings.VoxelPlacement.HasValue && settings.VoxelPlacement.Value.PlacementMode != VoxelPlacementMode.Both)
			{
				bool flag = IsAabbInsideVoxel(m, localAabb, settings);
				if (settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.InVoxel)
				{
					flag = !flag;
				}
				if (flag)
				{
					return false;
				}
			}
			bool result = true;
			if (testPhysics)
			{
				MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref rotation, m_physicsBoxQueryList, 7);
				m_lastQueryBox.Value.HalfExtents = halfExtents;
				m_lastQueryTransform = MatrixD.CreateFromQuaternion(rotation);
				m_lastQueryTransform.Translation = translation;
				result = TestPlacementAreaInternal(targetGrid, targetGridIsStatic, ref settings, null, null, ref localAabb, ignoredEntity, ref m, out var _, dynamicBuildMode);
			}
			return result;
		}

		/// <summary>
		/// Checks if aabb is in voxel. If settings provided it will return false if penetration settings allow for it.
		/// </summary>
		/// <param name="worldMatrix">World matrix of the aabb.</param>
		/// <param name="localAabb">Local aabb</param>
		/// <param name="settings">Game settings</param>
		/// <returns></returns>
		public static bool IsAabbInsideVoxel(MatrixD worldMatrix, BoundingBoxD localAabb, MyGridPlacementSettings settings)
		{
			if (!settings.VoxelPlacement.HasValue)
			{
				return false;
			}
			BoundingBoxD box = localAabb.TransformFast(ref worldMatrix);
			List<MyVoxelBase> list = new List<MyVoxelBase>();
			MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, list);
			foreach (MyVoxelBase item in list)
			{
				if (settings.VoxelPlacement.Value.PlacementMode != VoxelPlacementMode.Volumetric && item.IsAnyAabbCornerInside(ref worldMatrix, localAabb))
				{
					return true;
				}
				if (settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.Volumetric && !TestPlacementVoxelMapPenetration(item, settings, ref localAabb, ref worldMatrix))
				{
					return true;
				}
			}
			return false;
		}

		public static bool TestBlockPlacementArea(MyCubeBlockDefinition blockDefinition, MyBlockOrientation? blockOrientation, MatrixD worldMatrix, ref MyGridPlacementSettings settings, BoundingBoxD localAabb, bool dynamicBuildMode, MyEntity ignoredEntity = null, bool testVoxel = true)
		{
			if (!MyEntities.IsInsideWorld(worldMatrix.Translation))
			{
				return false;
			}
			Vector3 halfExtents = localAabb.HalfExtents;
			halfExtents += settings.SearchHalfExtentsDeltaAbsolute;
			if (MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA)
			{
				halfExtents -= new Vector3(0.11f);
			}
			Vector3D translation = localAabb.TransformFast(ref worldMatrix).Center;
			Quaternion rotation = Quaternion.CreateFromRotationMatrix(in worldMatrix);
			rotation.Normalize();
			MyGridPlacementSettings settings2 = settings;
			if (dynamicBuildMode && blockDefinition.CubeSize == MyCubeSize.Large)
			{
				settings2.VoxelPlacement = new VoxelPlacementSettings
				{
					PlacementMode = VoxelPlacementMode.Both
				};
			}
			if (testVoxel && !TestVoxelPlacement(blockDefinition, settings2, dynamicBuildMode, worldMatrix, localAabb))
			{
				return false;
			}
			MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref rotation, m_physicsBoxQueryList, 7);
			m_lastQueryBox.Value.HalfExtents = halfExtents;
			m_lastQueryTransform = MatrixD.CreateFromQuaternion(rotation);
			m_lastQueryTransform.Translation = translation;
			MyCubeGrid touchingGrid;
			return TestPlacementAreaInternal(null, ref settings2, blockDefinition, blockOrientation, ref localAabb, ignoredEntity, ref worldMatrix, out touchingGrid, dynamicBuildMode);
		}

		public static bool TestVoxelPlacement(MyCubeBlockDefinition blockDefinition, MyGridPlacementSettings settingsCopy, bool dynamicBuildMode, MatrixD worldMatrix, BoundingBoxD localAabb)
		{
			if (blockDefinition.VoxelPlacement.HasValue)
			{
				settingsCopy.VoxelPlacement = (dynamicBuildMode ? blockDefinition.VoxelPlacement.Value.DynamicMode : blockDefinition.VoxelPlacement.Value.StaticMode);
			}
			if (!MyEntities.IsInsideWorld(worldMatrix.Translation))
			{
				return false;
			}
			if (settingsCopy.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.None)
			{
				return false;
			}
			if (settingsCopy.VoxelPlacement.Value.PlacementMode != VoxelPlacementMode.Both)
			{
				bool flag = IsAabbInsideVoxel(worldMatrix, localAabb, settingsCopy);
				if (settingsCopy.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.InVoxel)
				{
					flag = !flag;
				}
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		private static void ExtractModelDataForObj(MyModel model, Matrix matrix, List<Vector3> vertices, List<TriangleWithMaterial> triangles, List<Vector2> uvs, ref Vector2 offsetUV, List<MyExportModel.Material> materials, ref int currVerticesCount, Vector3 colorMaskHSV)
		{
			if (!model.HasUV)
			{
				model.LoadUV = true;
				model.UnloadData();
				model.LoadData();
			}
			MyExportModel myExportModel = new MyExportModel(model);
			int verticesCount = myExportModel.GetVerticesCount();
			List<HalfVector2> uVsForModel = GetUVsForModel(myExportModel, verticesCount);
			if (uVsForModel.Count != verticesCount)
			{
				return;
			}
			List<MyExportModel.Material> list = CreateMaterialsForModel(materials, colorMaskHSV, myExportModel);
			for (int i = 0; i < verticesCount; i++)
			{
				vertices.Add(Vector3.Transform(myExportModel.GetVertex(i), matrix));
				Vector2 vector = uVsForModel[i].ToVector2() / myExportModel.PatternScale + offsetUV;
				uvs.Add(new Vector2(vector.X, 0f - vector.Y));
			}
			for (int j = 0; j < myExportModel.GetTrianglesCount(); j++)
			{
				int num = -1;
				for (int k = 0; k < list.Count; k++)
				{
					if (j <= list[k].LastTri)
					{
						num = k;
						break;
					}
				}
				MyTriangleVertexIndices triangle = myExportModel.GetTriangle(j);
				string material = "EmptyMaterial";
				if (num != -1)
				{
					material = list[num].ExportedMaterialName;
				}
				triangles.Add(new TriangleWithMaterial
				{
					material = material,
					triangle = new MyTriangleVertexIndices(triangle.I0 + 1 + currVerticesCount, triangle.I1 + 1 + currVerticesCount, triangle.I2 + 1 + currVerticesCount),
					uvIndices = new MyTriangleVertexIndices(triangle.I0 + 1 + currVerticesCount, triangle.I1 + 1 + currVerticesCount, triangle.I2 + 1 + currVerticesCount)
				});
			}
			currVerticesCount += verticesCount;
		}

		private static List<HalfVector2> GetUVsForModel(MyExportModel renderModel, int modelVerticesCount)
		{
			HalfVector2[] texCoords = renderModel.GetTexCoords();
			if (texCoords == null)
			{
				return new List<HalfVector2>();
			}
<<<<<<< HEAD
			return texCoords.ToList();
=======
			return Enumerable.ToList<HalfVector2>((IEnumerable<HalfVector2>)texCoords);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static List<MyExportModel.Material> CreateMaterialsForModel(List<MyExportModel.Material> materials, Vector3 colorMaskHSV, MyExportModel renderModel)
		{
			List<MyExportModel.Material> materials2 = renderModel.GetMaterials();
			List<MyExportModel.Material> list = new List<MyExportModel.Material>(materials2.Count);
			foreach (MyExportModel.Material item2 in materials2)
			{
				MyExportModel.Material? material = null;
				foreach (MyExportModel.Material material2 in materials)
				{
					if ((double)(colorMaskHSV - material2.ColorMaskHSV).AbsMax() < 0.01 && item2.EqualsMaterialWise(material2))
					{
						material = material2;
						break;
					}
				}
				MyExportModel.Material item = item2;
				item.ColorMaskHSV = colorMaskHSV;
				if (material.HasValue)
				{
					item.ExportedMaterialName = material.Value.ExportedMaterialName;
				}
				else
				{
					materialID++;
					item.ExportedMaterialName = "material_" + materialID;
					materials.Add(item);
				}
				list.Add(item);
			}
			return list;
		}

		private static MyCubePart[] GetCubeParts(MyStringHash skinSubtypeId, MyCubeBlockDefinition block, Vector3I position, MatrixD rotation, float gridSize, float gridScale)
		{
			List<string> list = new List<string>();
			List<MatrixD> list2 = new List<MatrixD>();
			List<Vector3> outLocalNormals = new List<Vector3>();
			List<Vector4UByte> list3 = new List<Vector4UByte>();
			GetCubeParts(block, position, rotation, gridSize, list, list2, outLocalNormals, list3, topologyCheck: true);
			MyCubePart[] array = new MyCubePart[list.Count];
			for (int i = 0; i < array.Length; i++)
			{
				MyCubePart myCubePart = new MyCubePart();
				MyModel modelOnlyData = MyModels.GetModelOnlyData(list[i]);
				modelOnlyData.Rescale(gridScale);
				MatrixD m = list2[i];
				myCubePart.Init(modelOnlyData, skinSubtypeId, m, gridScale);
				myCubePart.InstanceData.SetTextureOffset(list3[i]);
				array[i] = myCubePart;
			}
			return array;
		}

		private static bool TestPlacementAreaInternal(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, MyCubeBlockDefinition blockDefinition, MyBlockOrientation? blockOrientation, ref BoundingBoxD localAabb, MyEntity ignoredEntity, ref MatrixD worldMatrix, out MyCubeGrid touchingGrid, bool dynamicBuildMode = false, bool ignoreFracturedPieces = false, bool forceCheck = false)
		{
			return TestPlacementAreaInternal(targetGrid, targetGrid?.IsStatic ?? (!dynamicBuildMode), ref settings, blockDefinition, blockOrientation, ref localAabb, ignoredEntity, ref worldMatrix, out touchingGrid, dynamicBuildMode, ignoreFracturedPieces, forceCheck);
		}

		private static bool TestPlacementAreaInternalWithEntities(MyCubeGrid targetGrid, bool targetGridIsStatic, ref MyGridPlacementSettings settings, ref BoundingBoxD localAabb, MyEntity ignoredEntity, ref MatrixD worldMatrix, bool dynamicBuildMode = false)
		{
			MyCubeGrid touchingGrid = null;
			float gridSize = targetGrid.GridSize;
			bool flag = targetGridIsStatic;
			localAabb.TransformFast(ref worldMatrix);
			bool entityOverlap = false;
			bool touchingStaticGrid = false;
			foreach (MyEntity tmpResult in m_tmpResultList)
			{
				if ((ignoredEntity != null && (tmpResult == ignoredEntity || tmpResult.GetTopMostParent() == ignoredEntity)) || tmpResult.Physics == null)
				{
					continue;
				}
				MyCubeGrid myCubeGrid = tmpResult as MyCubeGrid;
				if (myCubeGrid != null)
				{
					if (flag != myCubeGrid.IsStatic || gridSize == myCubeGrid.GridSize)
					{
						TestGridPlacement(ref settings, ref worldMatrix, ref touchingGrid, gridSize, flag, ref localAabb, null, null, ref entityOverlap, ref touchingStaticGrid, myCubeGrid);
						if (entityOverlap)
						{
							break;
						}
					}
				}
				else
				{
					MyCharacter myCharacter = tmpResult as MyCharacter;
					if (myCharacter != null && myCharacter.PositionComp.WorldAABB.Intersects(targetGrid.PositionComp.WorldAABB))
					{
						entityOverlap = true;
						break;
					}
				}
			}
			m_tmpResultList.Clear();
			if (entityOverlap)
			{
				return false;
			}
			_ = targetGrid.IsStatic;
			return true;
		}

		private static void TestGridPlacement(ref MyGridPlacementSettings settings, ref MatrixD worldMatrix, ref MyCubeGrid touchingGrid, float gridSize, bool isStatic, ref BoundingBoxD localAABB, MyCubeBlockDefinition blockDefinition, MyBlockOrientation? blockOrientation, ref bool entityOverlap, ref bool touchingStaticGrid, MyCubeGrid grid, MyCubeGrid targetedGrid = null, bool forceCheck = false)
		{
			BoundingBoxD boundingBoxD = localAABB.TransformFast(ref worldMatrix);
			MatrixD m = grid.PositionComp.WorldMatrixNormalizedInv;
			boundingBoxD.TransformFast(ref m);
			Vector3D position = Vector3D.Transform(localAABB.Min, worldMatrix);
			Vector3D position2 = Vector3D.Transform(localAABB.Max, worldMatrix);
			Vector3D value = Vector3D.Transform(position, m);
			Vector3D value2 = Vector3D.Transform(position2, m);
			Vector3D vector3D = Vector3D.Min(value, value2);
			Vector3D vector3D2 = Vector3D.Max(value, value2);
			Vector3D value3 = (vector3D + gridSize / 2f) / grid.GridSize;
			Vector3D value4 = (vector3D2 - gridSize / 2f) / grid.GridSize;
			Vector3I value5 = Vector3I.Round(value3);
			Vector3I value6 = Vector3I.Round(value4);
			Vector3I min = Vector3I.Min(value5, value6);
			Vector3I max = Vector3I.Max(value5, value6);
			MyBlockOrientation? orientation = null;
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && isStatic && grid.IsStatic && blockOrientation.HasValue)
			{
				blockOrientation.Value.GetMatrix(out var result);
				MatrixD m2 = result * worldMatrix;
				Matrix matrix = m2;
				m2 = matrix * m;
				matrix = m2;
				matrix.Translation = Vector3.Zero;
				Base6Directions.Direction forward = Base6Directions.GetForward(ref matrix);
				Base6Directions.Direction up = Base6Directions.GetUp(ref matrix);
				if (Base6Directions.IsValidBlockOrientation(forward, up))
				{
					orientation = new MyBlockOrientation(forward, up);
				}
			}
			MyPhysicsBody physicsBody = grid.GetPhysicsBody();
			if (!forceCheck && targetedGrid != null && physicsBody != null)
			{
				HkBoxShape hkBoxShape = new HkBoxShape(targetedGrid.PositionComp.LocalAABB.HalfExtents);
				MatrixD m3 = grid.PositionComp.WorldMatrixRef;
				MatrixD m4 = targetedGrid.PositionComp.WorldMatrixRef;
				m4.Translation -= m3.Translation;
				m3.Translation = Vector3D.Zero;
				Matrix transform = m3;
				Matrix transform2 = m4;
				entityOverlap = MyPhysics.IsPenetratingShapeShape(physicsBody.GetShape(), ref transform, hkBoxShape, ref transform2);
			}
			else if (!grid.CanAddCubes(min, max, orientation, blockDefinition))
			{
				entityOverlap = true;
			}
			else if (settings.CanAnchorToStaticGrid && grid.IsTouchingAnyNeighbor(min, max))
			{
				touchingStaticGrid = true;
				if (touchingGrid == null)
				{
					touchingGrid = grid;
				}
			}
		}

		private static bool TestPlacementAreaInternal(MyCubeGrid targetGrid, bool targetGridIsStatic, ref MyGridPlacementSettings settings, MyCubeBlockDefinition blockDefinition, MyBlockOrientation? blockOrientation, ref BoundingBoxD localAabb, MyEntity ignoredEntity, ref MatrixD worldMatrix, out MyCubeGrid touchingGrid, bool dynamicBuildMode = false, bool ignoreFracturedPieces = false, bool forceCheck = false)
		{
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			touchingGrid = null;
			float num = targetGrid?.GridSize ?? ((blockDefinition != null) ? MyDefinitionManager.Static.GetCubeSize(blockDefinition.CubeSize) : MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large));
			bool flag = targetGridIsStatic;
			bool entityOverlap = false;
			bool touchingStaticGrid = false;
			foreach (HkBodyCollision physicsBoxQuery in m_physicsBoxQueryList)
			{
				MyEntity myEntity = physicsBoxQuery.Body.GetEntity(0u) as MyEntity;
				if (myEntity == null || myEntity.GetTopMostParent().GetPhysicsBody() == null || (ignoreFracturedPieces && myEntity is MyFracturedPiece))
<<<<<<< HEAD
				{
					continue;
				}
				bool num2 = myEntity.GetTopMostParent().GetPhysicsBody().WeldInfo.Children.Count == 0;
				bool flag2 = ignoredEntity != null;
				if ((num2 && flag2 && (myEntity == ignoredEntity || ShouldEntityBeIgnored(ignoredEntity, myEntity))) || (myEntity.Physics != null && myEntity.Physics.IsPhantom))
				{
					continue;
				}
				MyPhysicsComponentBase physics = myEntity.GetTopMostParent().Physics;
				if (physics != null && physics.IsPhantom)
				{
					continue;
				}
				MyCubeGrid myCubeGrid = myEntity.GetTopMostParent() as MyCubeGrid;
				if (myEntity.GetTopMostParent().GetPhysicsBody().WeldInfo.Children.Count > 0)
				{
=======
				{
					continue;
				}
				bool num2 = myEntity.GetTopMostParent().GetPhysicsBody().WeldInfo.Children.get_Count() == 0;
				bool flag2 = ignoredEntity != null;
				if (num2 && flag2 && (myEntity == ignoredEntity || ShouldEntityBeIgnored(ignoredEntity, myEntity)))
				{
					continue;
				}
				MyPhysicsComponentBase physics = myEntity.GetTopMostParent().Physics;
				if (physics != null && physics.IsPhantom)
				{
					continue;
				}
				MyCubeGrid myCubeGrid = myEntity.GetTopMostParent() as MyCubeGrid;
				if (myEntity.GetTopMostParent().GetPhysicsBody().WeldInfo.Children.get_Count() > 0)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (myEntity != ignoredEntity && TestQueryIntersection(myEntity.GetPhysicsBody().GetShape(), myEntity.WorldMatrix))
					{
						entityOverlap = true;
						if (touchingGrid == null)
						{
							touchingGrid = myEntity as MyCubeGrid;
						}
						break;
					}
<<<<<<< HEAD
					foreach (MyPhysicsBody child in myEntity.GetPhysicsBody().WeldInfo.Children)
					{
						if (child.Entity != ignoredEntity && TestQueryIntersection(child.WeldedRigidBody.GetShape(), child.Entity.WorldMatrix))
						{
							if (touchingGrid == null)
							{
								touchingGrid = child.Entity as MyCubeGrid;
							}
							entityOverlap = true;
							break;
						}
					}
					if (entityOverlap)
					{
						break;
					}
					continue;
				}
				if (myCubeGrid != null && ((flag && myCubeGrid.IsStatic) || (MyFakes.ENABLE_DYNAMIC_SMALL_GRID_MERGING && !flag && !myCubeGrid.IsStatic && blockDefinition != null && blockDefinition.CubeSize == myCubeGrid.GridSizeEnum) || (flag && myCubeGrid.IsStatic && blockDefinition != null && blockDefinition.CubeSize == myCubeGrid.GridSizeEnum)))
				{
					if (flag == myCubeGrid.IsStatic && num != myCubeGrid.GridSize)
					{
						continue;
					}
					if (!IsOrientationsAligned(myCubeGrid.WorldMatrix, worldMatrix))
					{
						entityOverlap = true;
						continue;
					}
					TestGridPlacement(ref settings, ref worldMatrix, ref touchingGrid, num, flag, ref localAabb, blockDefinition, blockOrientation, ref entityOverlap, ref touchingStaticGrid, myCubeGrid, targetGrid, forceCheck);
=======
					Enumerator<MyPhysicsBody> enumerator2 = myEntity.GetPhysicsBody().WeldInfo.Children.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							MyPhysicsBody current = enumerator2.get_Current();
							if (current.Entity != ignoredEntity && TestQueryIntersection(current.WeldedRigidBody.GetShape(), current.Entity.WorldMatrix))
							{
								if (touchingGrid == null)
								{
									touchingGrid = current.Entity as MyCubeGrid;
								}
								entityOverlap = true;
								break;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (entityOverlap)
					{
						break;
					}
					continue;
				}
<<<<<<< HEAD
=======
				if (myCubeGrid != null && ((flag && myCubeGrid.IsStatic) || (MyFakes.ENABLE_DYNAMIC_SMALL_GRID_MERGING && !flag && !myCubeGrid.IsStatic && blockDefinition != null && blockDefinition.CubeSize == myCubeGrid.GridSizeEnum) || (flag && myCubeGrid.IsStatic && blockDefinition != null && blockDefinition.CubeSize == myCubeGrid.GridSizeEnum)))
				{
					if (flag == myCubeGrid.IsStatic && num != myCubeGrid.GridSize)
					{
						continue;
					}
					if (!IsOrientationsAligned(myCubeGrid.WorldMatrix, worldMatrix))
					{
						entityOverlap = true;
						continue;
					}
					TestGridPlacement(ref settings, ref worldMatrix, ref touchingGrid, num, flag, ref localAabb, blockDefinition, blockOrientation, ref entityOverlap, ref touchingStaticGrid, myCubeGrid);
					if (entityOverlap)
					{
						break;
					}
					continue;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				entityOverlap = true;
				break;
			}
			m_tmpResultList.Clear();
			m_physicsBoxQueryList.Clear();
			if (entityOverlap)
			{
				return false;
			}
			return true;
		}

		private static bool ShouldEntityBeIgnored(MyEntity ignorable, MyEntity entity)
		{
			if (entity == null || ignorable == null)
			{
				return false;
			}
			if (entity is MyExtendedPistonBase)
			{
				return false;
			}
			return entity.GetTopMostParent() == ignorable;
		}

		private static bool IsOrientationsAligned(MatrixD transform1, MatrixD transform2)
		{
			double num = Vector3D.Dot(transform1.Forward, transform2.Forward);
			if ((num > 0.0010000000474974513 && num < 0.99899999995250255) || (num < -0.0010000000474974513 && num > -0.99899999995250255))
			{
				return false;
			}
			double num2 = Vector3D.Dot(transform1.Up, transform2.Up);
			if ((num2 > 0.0010000000474974513 && num2 < 0.99899999995250255) || (num2 < -0.0010000000474974513 && num2 > -0.99899999995250255))
			{
				return false;
			}
			double num3 = Vector3D.Dot(transform1.Right, transform2.Right);
			if ((num3 > 0.0010000000474974513 && num3 < 0.99899999995250255) || (num3 < -0.0010000000474974513 && num3 > -0.99899999995250255))
			{
				return false;
			}
			return true;
		}

		private static bool TestQueryIntersection(HkShape shape, MatrixD transform)
		{
			MatrixD m = m_lastQueryTransform;
			MatrixD m2 = transform;
			m2.Translation -= m.Translation;
			m.Translation = Vector3D.Zero;
			Matrix transform2 = m;
			Matrix transform3 = m2;
			return MyPhysics.IsPenetratingShapeShape(m_lastQueryBox.Value, ref transform2, shape, ref transform3);
		}

		public static bool TestPlacementVoxelMapOverlap(MyVoxelBase voxelMap, ref MyGridPlacementSettings settings, ref BoundingBoxD localAabb, ref MatrixD worldMatrix, bool touchingStaticGrid = false)
		{
			BoundingBoxD boundingBox = localAabb.TransformFast(ref worldMatrix);
			int num = 2;
			if (voxelMap == null)
			{
				voxelMap = MySession.Static.VoxelMaps.GetVoxelMapWhoseBoundingBoxIntersectsBox(ref boundingBox, null);
			}
			if (voxelMap != null && voxelMap.IsAnyAabbCornerInside(ref worldMatrix, localAabb))
			{
				num = 1;
			}
			bool result = true;
			switch (num)
			{
			case 1:
				result = settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.Both;
				break;
			case 2:
				result = settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.OutsideVoxel || (settings.CanAnchorToStaticGrid && touchingStaticGrid);
				break;
			}
			return result;
		}

		private static bool TestPlacementVoxelMapPenetration(MyVoxelBase voxelMap, MyGridPlacementSettings settings, ref BoundingBoxD localAabb, ref MatrixD worldMatrix, bool touchingStaticGrid = false)
		{
			float num = 0f;
			if (voxelMap != null)
			{
				bool stopIfFindAtLeastOneContent = settings.VoxelPlacement.Value.MaxAllowed <= 0f;
				MyTuple<float, float> voxelContentInBoundingBox_Fast = voxelMap.GetVoxelContentInBoundingBox_Fast(localAabb, worldMatrix, stopIfFindAtLeastOneContent);
				_ = localAabb.Volume;
				num = (voxelContentInBoundingBox_Fast.Item2.IsValid() ? voxelContentInBoundingBox_Fast.Item2 : 0f);
			}
			if (num <= settings.VoxelPlacement.Value.MaxAllowed)
			{
				if (!(num >= settings.VoxelPlacement.Value.MinAllowed))
				{
					return settings.CanAnchorToStaticGrid && touchingStaticGrid;
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// Fills passed lists with mount point data, which is transformed using orientation
		/// of the block.
		/// </summary>
		/// <param name="outMountPoints">Output buffer.</param>
		/// <param name="def"></param>
		/// <param name="mountPoints"></param>
		/// <param name="orientation"></param>
		public static void TransformMountPoints(List<MyCubeBlockDefinition.MountPoint> outMountPoints, MyCubeBlockDefinition def, MyCubeBlockDefinition.MountPoint[] mountPoints, ref MyBlockOrientation orientation)
		{
			outMountPoints.Clear();
			if (mountPoints != null)
			{
				orientation.GetMatrix(out var result);
				Vector3I center = def.Center;
				for (int i = 0; i < mountPoints.Length; i++)
				{
					MyCubeBlockDefinition.MountPoint mountPoint = mountPoints[i];
					MyCubeBlockDefinition.MountPoint item = default(MyCubeBlockDefinition.MountPoint);
					Vector3 position = mountPoint.Start - center;
					Vector3 position2 = mountPoint.End - center;
					Vector3I.Transform(ref mountPoint.Normal, ref result, out item.Normal);
					Vector3.Transform(ref position, ref result, out item.Start);
					Vector3.Transform(ref position2, ref result, out item.End);
					item.ExclusionMask = mountPoint.ExclusionMask;
					item.PropertiesMask = mountPoint.PropertiesMask;
					item.Enabled = mountPoint.Enabled;
					item.PressurizedWhenOpen = mountPoint.PressurizedWhenOpen;
					Vector3I position3 = Vector3I.Floor(mountPoint.Start) - center;
					Vector3I position4 = Vector3I.Floor(mountPoint.End) - center;
					Vector3I.Transform(ref position3, ref result, out position3);
					Vector3I.Transform(ref position4, ref result, out position4);
					Vector3I vector3I = Vector3I.Floor(item.Start);
					Vector3I vector3I2 = Vector3I.Floor(item.End);
					Vector3I vector3I3 = position3 - vector3I;
					Vector3I vector3I4 = position4 - vector3I2;
					item.Start += (Vector3)vector3I3;
					item.End += (Vector3)vector3I4;
					outMountPoints.Add(item);
				}
			}
		}

		internal static MyObjectBuilder_CubeBlock CreateBlockObjectBuilder(MyCubeBlockDefinition definition, Vector3I min, MyBlockOrientation orientation, long entityID, long owner, bool fullyBuilt)
		{
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = (MyObjectBuilder_CubeBlock)MyObjectBuilderSerializer.CreateNewObject(definition.Id);
			myObjectBuilder_CubeBlock.BuildPercent = (fullyBuilt ? 1f : 1.52590219E-05f);
			myObjectBuilder_CubeBlock.IntegrityPercent = (fullyBuilt ? 1f : 1.52590219E-05f);
			myObjectBuilder_CubeBlock.EntityId = entityID;
			myObjectBuilder_CubeBlock.Min = min;
			myObjectBuilder_CubeBlock.BlockOrientation = orientation;
			myObjectBuilder_CubeBlock.BuiltBy = owner;
			if (definition.ContainsComputer())
			{
				myObjectBuilder_CubeBlock.Owner = 0L;
				myObjectBuilder_CubeBlock.ShareMode = MyOwnershipShareModeEnum.All;
			}
			return myObjectBuilder_CubeBlock;
		}

		private static Vector3 ConvertVariantToHsvColor(Color variantColor)
		{
			return variantColor.PackedValue switch
			{
				4278190335u => MyRenderComponentBase.OldRedToHSV, 
				4278255615u => MyRenderComponentBase.OldYellowToHSV, 
				4294901760u => MyRenderComponentBase.OldBlueToHSV, 
				4278222848u => MyRenderComponentBase.OldGreenToHSV, 
				4278190080u => MyRenderComponentBase.OldBlackToHSV, 
				uint.MaxValue => MyRenderComponentBase.OldWhiteToHSV, 
				_ => MyRenderComponentBase.OldGrayToHSV, 
			};
		}

		internal static MyObjectBuilder_CubeBlock FindDefinitionUpgrade(MyObjectBuilder_CubeBlock block, out MyCubeBlockDefinition blockDefinition)
		{
			foreach (MyCubeBlockDefinition item in Enumerable.OfType<MyCubeBlockDefinition>((IEnumerable)MyDefinitionManager.Static.GetAllDefinitions()))
			{
				if (item.Id.SubtypeId == block.SubtypeId && !string.IsNullOrEmpty(block.SubtypeId.String))
				{
					blockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(item.Id);
					return MyObjectBuilder_CubeBlock.Upgrade(block, blockDefinition.Id.TypeId, block.SubtypeName);
				}
			}
			blockDefinition = null;
			return null;
		}

		/// <summary>
		/// Converts world coordinate to static global grid uniform coordinate (virtual large grid in whole world which every large grid is snapped to). 
		/// Grid size is already used inside calculation.
		/// </summary>
		public static Vector3I StaticGlobalGrid_WorldToUGInt(Vector3D worldPos, float gridSize, bool staticGridAlignToCenter)
		{
			return Vector3I.Round(StaticGlobalGrid_WorldToUG(worldPos, gridSize, staticGridAlignToCenter));
		}

		/// <summary>
		/// Converts world coordinate to static global grid uniform coordinate (virtual large grid in whole world which every large grid is snapped to). 
		/// Grid size is already used inside calculation.
		/// </summary>
		public static Vector3D StaticGlobalGrid_WorldToUG(Vector3D worldPos, float gridSize, bool staticGridAlignToCenter)
		{
			Vector3D result = worldPos / gridSize;
			if (!staticGridAlignToCenter)
			{
				result += Vector3D.Half;
			}
			return result;
		}

		/// <summary>
		/// Converts static global uniform grid coordinate to world coordinate. 
		/// Grid size is already used inside calculation.
		/// </summary>
		public static Vector3D StaticGlobalGrid_UGToWorld(Vector3D ugPos, float gridSize, bool staticGridAlignToCenter)
		{
			if (staticGridAlignToCenter)
			{
				return gridSize * ugPos;
			}
			return gridSize * (ugPos - Vector3D.Half);
		}

		private static Type ChooseGridSystemsType()
		{
			Type gridSystemsType = typeof(MyCubeGridSystems);
			ChooseGridSystemsType(ref gridSystemsType, MyPlugins.GameAssembly);
			ChooseGridSystemsType(ref gridSystemsType, MyPlugins.SandboxAssembly);
			ChooseGridSystemsType(ref gridSystemsType, MyPlugins.UserAssemblies);
			return gridSystemsType;
		}

		private static void ChooseGridSystemsType(ref Type gridSystemsType, Assembly[] assemblies)
		{
			if (assemblies != null)
			{
				foreach (Assembly assembly in assemblies)
				{
					ChooseGridSystemsType(ref gridSystemsType, assembly);
				}
			}
		}

		private static void ChooseGridSystemsType(ref Type gridSystemsType, Assembly assembly)
		{
			if (assembly == null)
			{
				return;
			}
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (typeof(MyCubeGridSystems).IsAssignableFrom(type))
				{
					gridSystemsType = type;
					break;
				}
			}
		}

		private static bool ShouldBeStatic(MyCubeGrid grid, MyTestDynamicReason testReason, bool isVolumetric = false)
		{
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			if (testReason == MyTestDynamicReason.NoReason)
			{
				return true;
			}
			if (grid.IsUnsupportedStation && testReason != MyTestDynamicReason.ConvertToShip)
			{
				return true;
			}
			if (grid.GridSizeEnum == MyCubeSize.Small && MyCubeGridSmallToLargeConnection.Static != null && MyCubeGridSmallToLargeConnection.Static.TestGridSmallToLargeConnection(grid))
			{
				return true;
			}
			if (testReason != MyTestDynamicReason.GridSplitByBlock && testReason != MyTestDynamicReason.ConvertToShip)
			{
				grid.RecalcBounds();
				MyGridPlacementSettings settings = default(MyGridPlacementSettings);
				VoxelPlacementSettings voxelPlacementSettings = default(VoxelPlacementSettings);
				voxelPlacementSettings.PlacementMode = VoxelPlacementMode.Volumetric;
				VoxelPlacementSettings value = voxelPlacementSettings;
				settings.VoxelPlacement = value;
				if (!IsAabbInsideVoxel(grid.WorldMatrix, grid.PositionComp.LocalAABB, settings))
				{
					return false;
				}
				if (grid.GetBlocks().get_Count() > 1024)
				{
					return grid.IsStatic;
				}
			}
			BoundingBoxD box = grid.PositionComp.WorldAABB;
			if (MyGamePruningStructure.AnyVoxelMapInBox(ref box))
			{
				Enumerator<MySlimBlock> enumerator = grid.GetBlocks().GetEnumerator();
				try
				{
<<<<<<< HEAD
					if (IsInVoxels(block, checkForPhysics: true, isVolumetric))
=======
					while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						if (IsInVoxels(enumerator.get_Current()))
						{
							return true;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			return false;
		}

		public static bool IsInVoxels(MySlimBlock block, bool checkForPhysics = true, bool isVolumetric = false)
		{
			if (block.CubeGrid.Physics == null && checkForPhysics)
			{
				return false;
			}
			if (MyPerGameSettings.Destruction && block.CubeGrid.GridSizeEnum == MyCubeSize.Large)
			{
				return block.CubeGrid.Physics.Shape.BlocksConnectedToWorld.Contains(block.Position);
			}
			block.GetWorldBoundingBox(out var aabb);
			m_tmpVoxelList.Clear();
			MyGamePruningStructure.GetAllVoxelMapsInBox(ref aabb, m_tmpVoxelList);
			float gridSize = block.CubeGrid.GridSize;
			BoundingBoxD boundingBoxD = new BoundingBoxD(gridSize * ((Vector3D)block.Min - 0.5), gridSize * ((Vector3D)block.Max + 0.5));
			MatrixD aabbWorldTransform = block.CubeGrid.WorldMatrix;
			foreach (MyVoxelBase tmpVoxel in m_tmpVoxelList)
			{
				if (isVolumetric)
				{
					MyTuple<float, float> voxelContentInBoundingBox_Fast = tmpVoxel.GetVoxelContentInBoundingBox_Fast(boundingBoxD, aabbWorldTransform, stopIfFindAtLeastOneContent: true);
					if ((voxelContentInBoundingBox_Fast.Item2.IsValid() ? voxelContentInBoundingBox_Fast.Item2 : 0f) > 0f)
					{
						return true;
					}
				}
				else if (tmpVoxel.IsAnyAabbCornerInside(ref aabbWorldTransform, boundingBoxD))
				{
					return true;
				}
			}
			return false;
		}

		public static void CreateGridGroupLink(GridLinkTypeEnum type, long linkId, MyCubeGrid parent, MyCubeGrid child)
		{
			MyCubeGridGroups.Static.CreateLink(type, linkId, parent, child);
		}

		public static bool BreakGridGroupLink(GridLinkTypeEnum type, long linkId, MyCubeGrid parent, MyCubeGrid child)
		{
			return MyCubeGridGroups.Static.BreakLink(type, linkId, parent, child);
		}

		public static void KillAllCharacters(MyCubeGrid grid)
		{
			if (grid == null || !Sync.IsServer)
			{
				return;
			}
			foreach (MyCockpit fatBlock in grid.GetFatBlocks<MyCockpit>())
			{
				if (fatBlock != null && fatBlock.Pilot != null && !fatBlock.Pilot.IsDead)
				{
					fatBlock.Pilot.DoDamage(1000f, MyDamageType.Suicide, updateSync: true, fatBlock.Pilot.EntityId);
					fatBlock.RemovePilot();
				}
			}
		}

		public static void ResetInfoGizmos()
		{
			ShowSenzorGizmos = false;
			ShowGravityGizmos = false;
			ShowCenterOfMass = false;
			ShowGridPivot = false;
			ShowAntennaGizmos = false;
		}

		private List<Update> GetQueue(UpdateQueue queue)
		{
			if (queue == UpdateQueue.Invalid)
			{
				throw new ArgumentException("Invalid queue.");
			}
			ref List<Update> reference = ref m_updateQueues[(uint)(queue - 1)];
			if (reference == null)
			{
				reference = new List<Update>();
			}
			return reference;
		}

<<<<<<< HEAD
		/// <summary>
		/// Dispatch all method in the target update queue.
		/// </summary>
		/// <param name="queue"></param>
		/// <param name="parallel">Whether to dispatch the parallel or synchronous queues.</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void Dispatch(UpdateQueue queue, bool parallel = false)
		{
			bool flag = queue.IsExecutedOnce();
			List<Update> list = BeginUpdate(queue);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				Update u = list[i];
				if (!u.Removed && u.Parallel == parallel)
				{
					Invoke(in u, queue);
				}
				if (u.Removed || (flag && u.Parallel == parallel))
				{
					num++;
					if (u.Parallel)
					{
						Interlocked.Decrement(ref m_totalQueuedParallelUpdates);
					}
					else
					{
						Interlocked.Decrement(ref m_totalQueuedSynchronousUpdates);
					}
				}
				else if (num > 0)
				{
					list[i - num] = u;
					list[i] = default(Update);
				}
			}
			while (num-- > 0)
			{
				list.RemoveAt(list.Count - 1);
			}
			EndUpdate();
		}

<<<<<<< HEAD
		/// <summary>
		/// Dispatch all method in the target update queue.
		/// </summary>
		/// <param name="queue"></param>
		/// <param name="parallel"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void DispatchOnce(UpdateQueue queue, bool parallel = false)
		{
			Dispatch(queue, parallel);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Invoke(in Update u, UpdateQueue queue)
		{
			u.Callback();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private List<Update> BeginUpdate(UpdateQueue queue)
		{
			lock (m_updateQueues)
			{
				if (m_updateInProgress != 0)
				{
					throw new InvalidOperationException("An update queue is already being dispatched for this entity.");
				}
				m_updateInProgress = queue;
				return GetQueue(queue);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void EndUpdate()
		{
			lock (m_updateQueues)
			{
				m_updateInProgress = UpdateQueue.Invalid;
				if (m_hasDelayedUpdate)
				{
					lock (m_pendingAddedUpdates)
					{
						for (int num = m_pendingAddedUpdates.Count - 1; num >= 0; num--)
						{
							QueuedUpdateChange queuedUpdateChange = m_pendingAddedUpdates[num];
							if (queuedUpdateChange.Grid == this)
							{
								if (queuedUpdateChange.Add)
								{
									Schedule(queuedUpdateChange.Queue, queuedUpdateChange.Callback, queuedUpdateChange.Priority, queuedUpdateChange.Parallel);
								}
								else
								{
									DeSchedule(queuedUpdateChange.Queue, queuedUpdateChange.Callback);
								}
								m_pendingAddedUpdates.RemoveAtFast(num);
							}
						}
					}
					m_hasDelayedUpdate = false;
				}
				if (m_totalQueuedSynchronousUpdates == 0)
				{
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
				}
				else if (m_totalQueuedParallelUpdates == 0 && base.InScene)
				{
					MyEntities.Orchestrator.EntityFlagsChanged(this);
				}
			}
		}

		private static string GetProfilerKey(Action cb)
		{
<<<<<<< HEAD
			if (!m_methodNames.TryGetValue(cb.Method, out var value))
			{
				m_methodNames.TryAdd(cb.Method, value = DebugFormatMethodName(cb.Method));
			}
			return value;
=======
			string result = default(string);
			if (!m_methodNames.TryGetValue(cb.Method, ref result))
			{
				m_methodNames.TryAdd(cb.Method, result = DebugFormatMethodName(cb.Method));
			}
			return result;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static string DebugFormatMethodName(MethodInfo method)
		{
			if (typeof(MyCubeGrid).IsAssignableFrom(method.DeclaringType))
			{
				return method.Name;
			}
			return method.DeclaringType.Name + "." + method.Name;
		}

<<<<<<< HEAD
		/// <summary>
		/// Remove a scheduled update.
		/// </summary>
		/// <param name="queue"></param>
		/// <param name="callback"></param>
		/// <param name="priority"></param>
		/// <param name="parallel"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void Schedule(UpdateQueue queue, Action callback, int priority = int.MaxValue, bool parallel = false)
		{
			lock (m_updateQueues)
			{
				if (m_updateInProgress == queue)
				{
					lock (m_pendingAddedUpdates)
					{
						m_pendingAddedUpdates.Add(QueuedUpdateChange.MakeAdd(callback, priority, queue, this, parallel));
						m_hasDelayedUpdate = true;
					}
					return;
				}
				List<Update> queue2 = GetQueue(queue);
				for (int i = 0; i < queue2.Count; i++)
				{
					if (queue2[i].Callback == callback)
					{
						return;
					}
				}
				queue2.Insert(FindInsertion(queue2, priority), new Update(callback, priority, parallel));
				if (parallel)
				{
					if (Interlocked.Increment(ref m_totalQueuedParallelUpdates) == 1 && base.InScene)
					{
						MyEntities.Orchestrator.EntityFlagsChanged(this);
					}
				}
				else if (Interlocked.Increment(ref m_totalQueuedSynchronousUpdates) == 1)
				{
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Schedule an update.
		/// </summary>
		/// <param name="queue"></param>
		/// <param name="callback"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void DeSchedule(UpdateQueue queue, Action callback)
		{
			lock (m_updateQueues)
			{
				List<Update> queue2 = GetQueue(queue);
				if (m_updateInProgress == queue)
				{
					lock (m_pendingAddedUpdates)
					{
						m_pendingAddedUpdates.Add(QueuedUpdateChange.MakeRemove(callback, queue, this));
						m_hasDelayedUpdate = true;
					}
					return;
				}
				int i;
				for (i = 0; i < queue2.Count && !(queue2[i].Callback == callback); i++)
				{
				}
				if (i < queue2.Count)
				{
					Update value = queue2[i];
					value.SetRemoved();
					queue2[i] = value;
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Binary search for the index where a update method should be inserted according to it's priority.
		/// </summary>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int FindInsertion(List<Update> list, int priority)
		{
			if (list.Count == 0)
			{
				return 0;
			}
			int num = 0;
			int num2 = list.Count;
			while (num2 - num > 1)
			{
				int num3 = (num + num2) / 2;
				if (priority >= list[num3].Priority)
				{
					num = num3;
				}
				else
				{
					num2 = num3;
				}
			}
			int result = num;
			if (priority >= list[num].Priority)
			{
				result = num2;
			}
			return result;
		}

<<<<<<< HEAD
		/// <summary>
		/// Get a debug friendly list of all scheduled update methods in this grid.
		/// </summary>
		/// <param name="gridDebugUpdateInfo"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void GetDebugUpdateInfo(List<DebugUpdateRecord> gridDebugUpdateInfo)
		{
			for (int i = 0; i < m_updateQueues.Length; i++)
			{
				List<Update> list = m_updateQueues[i];
				if (list == null || list.Count <= 0)
				{
					continue;
				}
				UpdateQueue queue = (UpdateQueue)(i + 1);
				foreach (Update item in list)
				{
					Update update = item;
					if (!update.Removed)
					{
						gridDebugUpdateInfo.Add(new DebugUpdateRecord(in update, queue));
					}
				}
			}
		}

		public override void UpdateBeforeSimulation()
		{
			MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateBeforeSimulation");
			DispatchOnce(UpdateQueue.OnceBeforeSimulation);
			Dispatch(UpdateQueue.BeforeSimulation);
			MySimpleProfiler.End("UpdateBeforeSimulation");
		}

		public override void UpdateAfterSimulation()
		{
			MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateAfterSimulation");
			DispatchOnce(UpdateQueue.OnceAfterSimulation);
			Dispatch(UpdateQueue.AfterSimulation);
			MySimpleProfiler.End("UpdateAfterSimulation");
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void UpdateBeforeSimulationParallel()
		{
			DispatchOnce(UpdateQueue.OnceBeforeSimulation, parallel: true);
			Dispatch(UpdateQueue.BeforeSimulation, parallel: true);
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void UpdateAfterSimulationParallel()
		{
			DispatchOnce(UpdateQueue.OnceAfterSimulation, parallel: true);
			Dispatch(UpdateQueue.AfterSimulation, parallel: true);
		}

		VRage.Game.ModAPI.IMySlimBlock VRage.Game.ModAPI.IMyCubeGrid.AddBlock(MyObjectBuilder_CubeBlock objectBuilder, bool testMerge)
		{
			return AddBlock(objectBuilder, testMerge);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.ApplyDestructionDeformation(VRage.Game.ModAPI.IMySlimBlock block)
		{
			if (block is MySlimBlock)
			{
				ApplyDestructionDeformation(block as MySlimBlock, 1f, null, 0L);
			}
		}

		MatrixI VRage.Game.ModAPI.IMyCubeGrid.CalculateMergeTransform(VRage.Game.ModAPI.IMyCubeGrid gridToMerge, Vector3I gridOffset)
		{
			if (gridToMerge is MyCubeGrid)
			{
				return CalculateMergeTransform(gridToMerge as MyCubeGrid, gridOffset);
			}
			return default(MatrixI);
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.CanMergeCubes(VRage.Game.ModAPI.IMyCubeGrid gridToMerge, Vector3I gridOffset)
		{
			if (gridToMerge is MyCubeGrid)
			{
				return CanMergeCubes(gridToMerge as MyCubeGrid, gridOffset);
			}
			return false;
		}

		void VRage.Game.ModAPI.IMyCubeGrid.GetBlocks(List<VRage.Game.ModAPI.IMySlimBlock> blocks, Func<VRage.Game.ModAPI.IMySlimBlock, bool> collect)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = GetBlocks().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					if (collect == null || collect(current))
					{
						blocks.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		List<VRage.Game.ModAPI.IMySlimBlock> VRage.Game.ModAPI.IMyCubeGrid.GetBlocksInsideSphere(ref BoundingSphereD sphere)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			HashSet<MySlimBlock> val = new HashSet<MySlimBlock>();
			GetBlocksInsideSphere(ref sphere, val);
			List<VRage.Game.ModAPI.IMySlimBlock> list = new List<VRage.Game.ModAPI.IMySlimBlock>(val.get_Count());
			Enumerator<MySlimBlock> enumerator = val.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					list.Add(current);
				}
				return list;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		VRage.Game.ModAPI.IMySlimBlock VRage.Game.ModAPI.IMyCubeGrid.GetCubeBlock(Vector3I pos)
		{
			return GetCubeBlock(pos);
		}

		bool VRage.Game.ModAPI.Ingame.IMyCubeGrid.IsSameConstructAs(VRage.Game.ModAPI.Ingame.IMyCubeGrid other)
		{
			return IsSameConstructAs((MyCubeGrid)other);
		}

		Vector3D? VRage.Game.ModAPI.IMyCubeGrid.GetLineIntersectionExactAll(ref LineD line, out double distance, out VRage.Game.ModAPI.IMySlimBlock intersectedBlock)
		{
			MySlimBlock intersectedBlock2;
			Vector3D? lineIntersectionExactAll = GetLineIntersectionExactAll(ref line, out distance, out intersectedBlock2);
			intersectedBlock = intersectedBlock2;
			return lineIntersectionExactAll;
		}

		VRage.Game.ModAPI.IMyCubeGrid VRage.Game.ModAPI.IMyCubeGrid.MergeGrid_MergeBlock(VRage.Game.ModAPI.IMyCubeGrid gridToMerge, Vector3I gridOffset)
		{
			if (gridToMerge is MyCubeGrid)
			{
				return MergeGrid_MergeBlock(gridToMerge as MyCubeGrid, gridOffset);
			}
			return null;
		}

		void VRage.Game.ModAPI.IMyCubeGrid.RemoveBlock(VRage.Game.ModAPI.IMySlimBlock block, bool updatePhysics)
		{
			if (block is MySlimBlock)
			{
				RemoveBlock(block as MySlimBlock, updatePhysics);
			}
		}

		void VRage.Game.ModAPI.IMyCubeGrid.RemoveDestroyedBlock(VRage.Game.ModAPI.IMySlimBlock block)
		{
			if (block is MySlimBlock)
			{
				RemoveDestroyedBlock(block as MySlimBlock, 0L);
			}
		}

		void VRage.Game.ModAPI.IMyCubeGrid.UpdateBlockNeighbours(VRage.Game.ModAPI.IMySlimBlock block)
		{
			if (block is MySlimBlock)
			{
				UpdateBlockNeighbours(block as MySlimBlock);
			}
		}

		void VRage.Game.ModAPI.IMyCubeGrid.ChangeGridOwnership(long playerId, MyOwnershipShareModeEnum shareMode)
		{
			ChangeGridOwnership(playerId, shareMode);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.ClearSymmetries()
		{
			ClearSymmetries();
		}

		void VRage.Game.ModAPI.IMyCubeGrid.ColorBlocks(Vector3I min, Vector3I max, Vector3 newHSV)
		{
			ColorBlocks(min, max, newHSV, playSound: false, validateOwnership: false);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.SkinBlocks(Vector3I min, Vector3I max, Vector3? newHSV, string newSkin)
		{
			SkinBlocks(min, max, newHSV, MyStringHash.GetOrCompute(newSkin), playSound: false, validateOwnership: false);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.FixTargetCube(out Vector3I cube, Vector3 fractionalGridPosition)
		{
			FixTargetCube(out cube, fractionalGridPosition);
		}

		Vector3 VRage.Game.ModAPI.IMyCubeGrid.GetClosestCorner(Vector3I gridPos, Vector3 position)
		{
			return GetClosestCorner(gridPos, position);
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.GetLineIntersectionExactGrid(ref LineD line, ref Vector3I position, ref double distanceSquared)
		{
			return GetLineIntersectionExactGrid(ref line, ref position, ref distanceSquared);
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.IsTouchingAnyNeighbor(Vector3I min, Vector3I max)
		{
			return IsTouchingAnyNeighbor(min, max);
		}

		Vector3I? VRage.Game.ModAPI.IMyCubeGrid.RayCastBlocks(Vector3D worldStart, Vector3D worldEnd)
		{
			return RayCastBlocks(worldStart, worldEnd);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.RayCastCells(Vector3D worldStart, Vector3D worldEnd, List<Vector3I> outHitPositions, Vector3I? gridSizeInflate, bool havokWorld)
		{
			RayCastCells(worldStart, worldEnd, outHitPositions, gridSizeInflate, havokWorld);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.RazeBlock(Vector3I position)
		{
			RazeBlock(position, 0uL);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.RazeBlocks(ref Vector3I pos, ref Vector3UByte size)
		{
			RazeBlocks(ref pos, ref size, 0L);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.RazeBlocks(List<Vector3I> locations)
		{
			RazeBlocks(locations, 0L, 0uL);
		}

		Vector3I VRage.Game.ModAPI.Ingame.IMyCubeGrid.WorldToGridInteger(Vector3D coords)
		{
			return WorldToGridInteger(coords);
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.WillRemoveBlockSplitGrid(VRage.Game.ModAPI.IMySlimBlock testBlock)
		{
			return WillRemoveBlockSplitGrid((MySlimBlock)testBlock);
		}

		private Action<MySlimBlock> GetDelegate(Action<VRage.Game.ModAPI.IMySlimBlock> value)
		{
			return (Action<MySlimBlock>)Delegate.CreateDelegate(typeof(Action<MySlimBlock>), value.Target, value.Method);
		}

		private Action<MyCubeGrid> GetDelegate(Action<VRage.Game.ModAPI.IMyCubeGrid> value)
		{
			return (Action<MyCubeGrid>)Delegate.CreateDelegate(typeof(Action<MyCubeGrid>), value.Target, value.Method);
		}

		private Action<MyCubeGrid, MyCubeGrid> GetDelegate(Action<VRage.Game.ModAPI.IMyCubeGrid, VRage.Game.ModAPI.IMyCubeGrid> value)
		{
			return (Action<MyCubeGrid, MyCubeGrid>)Delegate.CreateDelegate(typeof(Action<MyCubeGrid, MyCubeGrid>), value.Target, value.Method);
		}

		private Action<MyCubeGrid, bool> GetDelegate(Action<VRage.Game.ModAPI.IMyCubeGrid, bool> value)
		{
			return (Action<MyCubeGrid, bool>)Delegate.CreateDelegate(typeof(Action<MyCubeGrid, bool>), value.Target, value.Method);
		}

		VRage.Game.ModAPI.Ingame.IMySlimBlock VRage.Game.ModAPI.Ingame.IMyCubeGrid.GetCubeBlock(Vector3I position)
		{
			VRage.Game.ModAPI.Ingame.IMySlimBlock cubeBlock = GetCubeBlock(position);
			if (cubeBlock != null && cubeBlock.FatBlock != null && cubeBlock.FatBlock is MyTerminalBlock && (cubeBlock.FatBlock as MyTerminalBlock).IsAccessibleForProgrammableBlock)
			{
				return cubeBlock;
			}
			return null;
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.CanAddCube(Vector3I pos)
		{
			return CanAddCube(pos, null, null);
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.CanAddCubes(Vector3I min, Vector3I max)
		{
			return CanAddCubes(min, max);
		}

		VRage.Game.ModAPI.IMyCubeGrid VRage.Game.ModAPI.IMyCubeGrid.SplitByPlane(PlaneD plane)
		{
			return SplitByPlane(plane);
		}

		VRage.Game.ModAPI.IMyCubeGrid VRage.Game.ModAPI.IMyCubeGrid.Split(List<VRage.Game.ModAPI.IMySlimBlock> blocks, bool sync)
		{
			return CreateSplit(this, blocks.ConvertAll((VRage.Game.ModAPI.IMySlimBlock x) => (MySlimBlock)x), sync, 0L);
		}

		public bool IsInSameLogicalGroupAs(VRage.Game.ModAPI.IMyCubeGrid other)
		{
			if (this != other)
			{
				return MyCubeGridGroups.Static.Logical.GetGroup(this) == MyCubeGridGroups.Static.Logical.GetGroup((MyCubeGrid)other);
			}
			return true;
		}

		public bool IsSameConstructAs(VRage.Game.ModAPI.IMyCubeGrid other)
		{
			if (this != other)
			{
				return MyCubeGridGroups.Static.Mechanical.GetGroup(this) == MyCubeGridGroups.Static.Mechanical.GetGroup((MyCubeGrid)other);
			}
			return true;
		}

		public bool IsRoomAtPositionAirtight(Vector3I pos)
		{
			if (GridSystems.GasSystem == null)
			{
				return false;
			}
			return GridSystems.GasSystem.GetOxygenRoomForCubeGridPosition(ref pos)?.IsAirtight ?? false;
		}

		public IMyGridGroupData GetGridGroup(GridLinkTypeEnum linkTypeEnum)
		{
			return MyCubeGridGroups.GetGridGroup(linkTypeEnum, this);
		}

		IEnumerable<T> VRage.Game.ModAPI.IMyCubeGrid.GetFatBlocks<T>()
		{
			return new MyFatBlockReader<T>(this);
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.ApplyDeformation(float deformationOffset, float softAreaPlanar, float softAreaVertical, Vector3 localPos, Vector3 localNormal, MyStringHash damageType, out int blocksDestroyedByThisCp, float offsetThreshold, float lowerRatioLimit, long attackerId)
		{
			if (Physics == null)
			{
				blocksDestroyedByThisCp = 0;
				return false;
			}
			return Physics.ApplyDeformation(deformationOffset, softAreaPlanar, softAreaVertical, localPos, localNormal, damageType, out blocksDestroyedByThisCp, offsetThreshold, lowerRatioLimit, attackerId);
		}
	}
}
