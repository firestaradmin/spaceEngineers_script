using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	public static class MyEntityList
	{
		[Serializable]
		public class MyEntityListInfoItem
		{
			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EDisplayName_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in string value)
				{
					owner.DisplayName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out string value)
				{
					value = owner.DisplayName;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in long value)
				{
					owner.EntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out long value)
				{
					value = owner.EntityId;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EBlockCount_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in int value)
				{
					owner.BlockCount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out int value)
				{
					value = owner.BlockCount;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EPCU_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, int?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in int? value)
				{
					owner.PCU = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out int? value)
				{
					value = owner.PCU;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EMass_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in float value)
				{
					owner.Mass = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out float value)
				{
					value = owner.Mass;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in Vector3D value)
				{
					owner.Position = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out Vector3D value)
				{
					value = owner.Position;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EOwnerName_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in string value)
				{
					owner.OwnerName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out string value)
				{
					value = owner.OwnerName;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EOwner_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in long value)
				{
					owner.Owner = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out long value)
				{
					value = owner.Owner;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003ESpeed_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in float value)
				{
					owner.Speed = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out float value)
				{
					value = owner.Speed;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EDistanceFromPlayers_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in float value)
				{
					owner.DistanceFromPlayers = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out float value)
				{
					value = owner.DistanceFromPlayers;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EOwnerLoginTime_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in float value)
				{
					owner.OwnerLoginTime = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out float value)
				{
					value = owner.OwnerLoginTime;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EOwnerLogoutTime_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, float?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in float? value)
				{
					owner.OwnerLogoutTime = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out float? value)
				{
					value = owner.OwnerLogoutTime;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EPlayerPresenceTier_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, MyUpdateTiersPlayerPresence>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in MyUpdateTiersPlayerPresence value)
				{
					owner.PlayerPresenceTier = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out MyUpdateTiersPlayerPresence value)
				{
					value = owner.PlayerPresenceTier;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EGridPresenceTier_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, MyUpdateTiersGridPresence>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in MyUpdateTiersGridPresence value)
				{
					owner.GridPresenceTier = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out MyUpdateTiersGridPresence value)
				{
					value = owner.GridPresenceTier;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EIsReplicated_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, bool?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in bool? value)
				{
					owner.IsReplicated = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out bool? value)
				{
					value = owner.IsReplicated;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EIsGrid_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in bool value)
				{
					owner.IsGrid = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out bool value)
				{
					value = owner.IsGrid;
				}
			}

			public string DisplayName;

			public long EntityId;

			public int BlockCount;

			public int? PCU;

			public float Mass;

			public Vector3D Position;

			public string OwnerName;

			public long Owner;

			public float Speed;

			public float DistanceFromPlayers;

			public float OwnerLoginTime;

			public float? OwnerLogoutTime;

			public MyUpdateTiersPlayerPresence PlayerPresenceTier;

			public MyUpdateTiersGridPresence GridPresenceTier;

			public bool? IsReplicated;

			public bool IsGrid;

			public MyEntityListInfoItem()
			{
			}

			public MyEntityListInfoItem(string displayName, long entityId, int blockCount, int? pcu, float mass, Vector3D position, float speed, float distanceFromPlayers, string ownerName, long owner, float ownerLogin, float? ownerLogout, MyUpdateTiersPlayerPresence playerPresenceTier = MyUpdateTiersPlayerPresence.Normal, MyUpdateTiersGridPresence gridPresenceTier = MyUpdateTiersGridPresence.Normal, bool? isReplicated = null, bool isGrid = false)
			{
				if (string.IsNullOrEmpty(displayName))
				{
					DisplayName = "----";
				}
				else
				{
					DisplayName = ((displayName.Length < 50) ? displayName : displayName.Substring(0, 49));
				}
				EntityId = entityId;
				BlockCount = blockCount;
				PCU = pcu;
				Mass = mass;
				Position = position;
				OwnerName = ownerName;
				Owner = owner;
				Speed = speed;
				DistanceFromPlayers = distanceFromPlayers;
				OwnerLoginTime = ownerLogin;
				OwnerLogoutTime = ownerLogout;
				PlayerPresenceTier = playerPresenceTier;
				GridPresenceTier = gridPresenceTier;
				IsReplicated = isReplicated;
				IsGrid = isGrid;
			}

			public void Add(ref MyEntityListInfoItem item)
			{
				BlockCount += item.BlockCount;
				if (item.PCU.HasValue && item.PCU.HasValue)
				{
					PCU += item.PCU.Value;
				}
				Mass += item.Mass;
				OwnerLoginTime = Math.Min(item.OwnerLoginTime, OwnerLoginTime);
				if (item.OwnerLogoutTime.HasValue && item.OwnerLogoutTime.HasValue)
				{
					OwnerLogoutTime = Math.Min(item.OwnerLogoutTime.Value, OwnerLogoutTime.Value);
				}
			}
		}

		public enum MyEntityTypeEnum
		{
			Grids,
			SmallGrids,
			LargeGrids,
			Characters,
			FloatingObjects,
			Planets,
			Asteroids,
			Replicated,
			NotReplicated
		}

		public enum EntityListAction
		{
			Remove,
			Stop,
			Depower,
			Power
		}

		public enum MyEntitySortOrder
		{
			DisplayName,
			BlockCount,
			Mass,
			OwnerName,
			DistanceFromCenter,
			Speed,
			DistanceFromPlayers,
			OwnerLastLogout,
			PCU
		}

		[ThreadStatic]
		private static MyEntityListInfoItem m_gridItem;

		private static MyEntityListInfoItem GenerateInfo_Grid(MyEntity entity, ref ICollection<MyPlayer> players)
		{
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return null;
			}
			MyCubeGrid mechanicalRootGrid = GetMechanicalRootGrid(myCubeGrid);
			if (myCubeGrid.Closed || myCubeGrid.Physics == null || mechanicalRootGrid != myCubeGrid)
			{
				return null;
			}
			CreateListInfoForGrid(myCubeGrid, out m_gridItem);
			AccountChildren(myCubeGrid);
			return m_gridItem;
		}

		private static List<MyEntityListInfoItem> GenerateInfo_Character(MyIdentity identity, bool? IsReplicatedFilter = null)
		{
<<<<<<< HEAD
=======
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			List<MyEntityListInfoItem> list = new List<MyEntityListInfoItem>();
			string text = identity.DisplayName;
			if (Sync.Players.TryGetPlayerId(identity.IdentityId, out var result))
			{
				MyPlayer player = null;
				if (!Sync.Players.TryGetPlayerById(result, out player))
				{
					text = string.Concat(text, " (", MyTexts.Get(MyCommonTexts.OfflineStatus), ")");
				}
			}
			if (identity.Character != null)
			{
				if (!IsReplicatedFilter.HasValue || IsReplicatedFilter.Value == identity.Character.IsReplicated)
				{
					list.Add(new MyEntityListInfoItem(text, identity.Character.EntityId, 0, identity.BlockLimits.PCU, identity.Character.CurrentMass, identity.Character.PositionComp.GetPosition(), identity.Character.Physics.LinearVelocity.Length(), 0f, identity.DisplayName, identity.IdentityId, (int)(DateTime.Now - identity.LastLoginTime).TotalSeconds, (int)(DateTime.Now - identity.LastLogoutTime).TotalSeconds, MyUpdateTiersPlayerPresence.Normal, MyUpdateTiersGridPresence.Normal, identity.Character.IsReplicated));
				}
				return list;
			}
<<<<<<< HEAD
			foreach (long savedCharacter in identity.SavedCharacters)
			{
				if (MyEntities.TryGetEntityById(savedCharacter, out MyCharacter entity, allowClosed: false) && (!IsReplicatedFilter.HasValue || IsReplicatedFilter.Value == entity.IsReplicated))
				{
					list.Add(new MyEntityListInfoItem(text, savedCharacter, 0, null, entity.CurrentMass, entity.PositionComp.GetPosition(), entity.Physics.LinearVelocity.Length(), 0f, identity.DisplayName, identity.IdentityId, (int)(DateTime.Now - identity.LastLoginTime).TotalSeconds, (int)(DateTime.Now - identity.LastLogoutTime).TotalSeconds, MyUpdateTiersPlayerPresence.Normal, MyUpdateTiersGridPresence.Normal, entity.IsReplicated));
				}
			}
			return list;
=======
			Enumerator<long> enumerator = identity.SavedCharacters.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					long current = enumerator.get_Current();
					if (MyEntities.TryGetEntityById(current, out MyCharacter entity, allowClosed: false) && (!IsReplicatedFilter.HasValue || IsReplicatedFilter.Value == entity.IsReplicated))
					{
						list.Add(new MyEntityListInfoItem(text, current, 0, null, entity.CurrentMass, entity.PositionComp.GetPosition(), entity.Physics.LinearVelocity.Length(), 0f, identity.DisplayName, identity.IdentityId, (int)(DateTime.Now - identity.LastLoginTime).TotalSeconds, (int)(DateTime.Now - identity.LastLogoutTime).TotalSeconds, MyUpdateTiersPlayerPresence.Normal, MyUpdateTiersGridPresence.Normal, entity.IsReplicated));
					}
				}
				return list;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static MyEntityListInfoItem GenerateInfo_FloatingObject(MyEntity entity, ref ICollection<MyPlayer> players)
		{
			MyFloatingObject myFloatingObject = entity as MyFloatingObject;
			if (myFloatingObject == null || myFloatingObject.Closed || myFloatingObject.Physics == null)
			{
				return null;
			}
			return new MyEntityListInfoItem(myFloatingObject.DisplayName, myFloatingObject.EntityId, 0, null, myFloatingObject.Physics.Mass, myFloatingObject.PositionComp.GetPosition(), myFloatingObject.Physics.LinearVelocity.Length(), MySession.GetPlayerDistance(myFloatingObject, players), "", 0L, 0f, null, MyUpdateTiersPlayerPresence.Normal, MyUpdateTiersGridPresence.Normal, myFloatingObject.IsReplicated);
		}

		private static MyEntityListInfoItem GenerateInfo_Bag(MyEntity entity, ref ICollection<MyPlayer> players)
		{
			MyInventoryBagEntity myInventoryBagEntity = entity as MyInventoryBagEntity;
			if (myInventoryBagEntity == null)
			{
				return null;
			}
			if (myInventoryBagEntity.Closed || myInventoryBagEntity.Physics == null)
			{
				return null;
			}
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(myInventoryBagEntity.OwnerIdentityId);
			string ownerName = "";
			float ownerLogin = 0f;
			float value = 0f;
			if (myIdentity != null)
			{
				ownerName = myIdentity.DisplayName;
				ownerLogin = (int)(DateTime.Now - myIdentity.LastLoginTime).TotalSeconds;
				value = (int)(DateTime.Now - myIdentity.LastLogoutTime).TotalSeconds;
			}
			return new MyEntityListInfoItem(myInventoryBagEntity.DisplayName, myInventoryBagEntity.EntityId, 0, null, myInventoryBagEntity.Physics.Mass, myInventoryBagEntity.PositionComp.GetPosition(), myInventoryBagEntity.Physics.LinearVelocity.Length(), MySession.GetPlayerDistance(myInventoryBagEntity, players), ownerName, myInventoryBagEntity.OwnerIdentityId, ownerLogin, value, MyUpdateTiersPlayerPresence.Normal, MyUpdateTiersGridPresence.Normal, myInventoryBagEntity.IsReplicated);
		}

		private static MyEntityListInfoItem GenerateInfo_Planet(MyEntity entity, ref ICollection<MyPlayer> players)
		{
			MyPlanet myPlanet = entity as MyPlanet;
			if (myPlanet == null || myPlanet.Closed)
			{
				return null;
			}
			return new MyEntityListInfoItem(myPlanet.StorageName, myPlanet.EntityId, 0, null, 0f, myPlanet.PositionComp.GetPosition(), 0f, MySession.GetPlayerDistance(myPlanet, players), "", 0L, 0f, null, MyUpdateTiersPlayerPresence.Normal, MyUpdateTiersGridPresence.Normal, myPlanet.IsReplicated);
		}

		private static MyEntityListInfoItem GenerateInfo_Asteroid(MyEntity entity, ref ICollection<MyPlayer> players)
		{
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (myVoxelBase == null || myVoxelBase is MyPlanet || myVoxelBase.Closed)
			{
				return null;
			}
			return new MyEntityListInfoItem(myVoxelBase.StorageName, myVoxelBase.EntityId, 0, null, 0f, myVoxelBase.PositionComp.GetPosition(), 0f, MySession.GetPlayerDistance(myVoxelBase, players), "", 0L, 0f, null, MyUpdateTiersPlayerPresence.Normal, MyUpdateTiersGridPresence.Normal, myVoxelBase.IsReplicated);
		}

		public static List<MyEntityListInfoItem> GetEntityList(MyEntityTypeEnum selectedType)
		{
			MyConcurrentHashSet<MyEntity> entities = MyEntities.GetEntities();
			List<MyEntityListInfoItem> list = new List<MyEntityListInfoItem>(entities.Count);
			ICollection<MyPlayer> players = MySession.Static.Players.GetOnlinePlayers();
			switch (selectedType)
			{
			case MyEntityTypeEnum.Grids:
			case MyEntityTypeEnum.SmallGrids:
			case MyEntityTypeEnum.LargeGrids:
			{
				foreach (MyEntity item in entities)
				{
					MyCubeGrid myCubeGrid = item as MyCubeGrid;
					if (myCubeGrid != null && (selectedType != MyEntityTypeEnum.LargeGrids || myCubeGrid.GridSizeEnum != MyCubeSize.Small) && (selectedType != MyEntityTypeEnum.SmallGrids || myCubeGrid.GridSizeEnum != 0))
					{
						MyEntityListInfoItem myEntityListInfoItem10 = GenerateInfo_Grid(item, ref players);
						if (myEntityListInfoItem10 != null)
						{
							list.Add(myEntityListInfoItem10);
						}
					}
				}
				return list;
			}
			case MyEntityTypeEnum.Replicated:
			case MyEntityTypeEnum.NotReplicated:
			{
				bool? isReplicatedFilter = null;
				switch (selectedType)
<<<<<<< HEAD
				{
				case MyEntityTypeEnum.Replicated:
					isReplicatedFilter = true;
					break;
				case MyEntityTypeEnum.NotReplicated:
					isReplicatedFilter = false;
					break;
				}
				foreach (MyEntity item2 in entities)
				{
=======
				{
				case MyEntityTypeEnum.Replicated:
					isReplicatedFilter = true;
					break;
				case MyEntityTypeEnum.NotReplicated:
					isReplicatedFilter = false;
					break;
				}
				foreach (MyEntity item2 in entities)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if ((!item2.IsReplicated && selectedType == MyEntityTypeEnum.Replicated) || (item2.IsReplicated && selectedType == MyEntityTypeEnum.NotReplicated))
					{
						continue;
					}
					MyEntityListInfoItem myEntityListInfoItem5 = GenerateInfo_Grid(item2, ref players);
					if (myEntityListInfoItem5 != null)
					{
						list.Add(myEntityListInfoItem5);
						continue;
					}
					MyEntityListInfoItem myEntityListInfoItem6 = GenerateInfo_FloatingObject(item2, ref players);
					if (myEntityListInfoItem6 != null)
<<<<<<< HEAD
					{
						list.Add(myEntityListInfoItem6);
						continue;
					}
					MyEntityListInfoItem myEntityListInfoItem7 = GenerateInfo_Bag(item2, ref players);
					if (myEntityListInfoItem7 != null)
					{
						list.Add(myEntityListInfoItem7);
						continue;
					}
					MyEntityListInfoItem myEntityListInfoItem8 = GenerateInfo_Planet(item2, ref players);
					if (myEntityListInfoItem8 != null)
					{
						list.Add(myEntityListInfoItem8);
						continue;
					}
					MyEntityListInfoItem myEntityListInfoItem9 = GenerateInfo_Asteroid(item2, ref players);
					if (myEntityListInfoItem9 != null)
					{
=======
					{
						list.Add(myEntityListInfoItem6);
						continue;
					}
					MyEntityListInfoItem myEntityListInfoItem7 = GenerateInfo_Bag(item2, ref players);
					if (myEntityListInfoItem7 != null)
					{
						list.Add(myEntityListInfoItem7);
						continue;
					}
					MyEntityListInfoItem myEntityListInfoItem8 = GenerateInfo_Planet(item2, ref players);
					if (myEntityListInfoItem8 != null)
					{
						list.Add(myEntityListInfoItem8);
						continue;
					}
					MyEntityListInfoItem myEntityListInfoItem9 = GenerateInfo_Asteroid(item2, ref players);
					if (myEntityListInfoItem9 != null)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						list.Add(myEntityListInfoItem9);
					}
				}
				{
					foreach (MyIdentity allIdentity in MySession.Static.Players.GetAllIdentities())
					{
						List<MyEntityListInfoItem> list3 = GenerateInfo_Character(allIdentity, isReplicatedFilter);
						if (list3 == null || list3.Count <= 0)
<<<<<<< HEAD
						{
							continue;
						}
						foreach (MyEntityListInfoItem item3 in list3)
						{
=======
						{
							continue;
						}
						foreach (MyEntityListInfoItem item3 in list3)
						{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							list.Add(item3);
						}
					}
					return list;
				}
			}
			case MyEntityTypeEnum.Characters:
			{
				foreach (MyIdentity allIdentity2 in MySession.Static.Players.GetAllIdentities())
				{
					List<MyEntityListInfoItem> list2 = GenerateInfo_Character(allIdentity2);
					if (list2 == null || list2.Count <= 0)
					{
						continue;
					}
					foreach (MyEntityListInfoItem item4 in list2)
					{
						list.Add(item4);
					}
				}
				return list;
			}
			case MyEntityTypeEnum.FloatingObjects:
			{
				foreach (MyEntity item5 in entities)
				{
					MyEntityListInfoItem myEntityListInfoItem3 = GenerateInfo_FloatingObject(item5, ref players);
					if (myEntityListInfoItem3 != null)
					{
						list.Add(myEntityListInfoItem3);
					}
					MyEntityListInfoItem myEntityListInfoItem4 = GenerateInfo_Bag(item5, ref players);
					if (myEntityListInfoItem4 != null)
					{
						list.Add(myEntityListInfoItem4);
					}
				}
				return list;
			}
			case MyEntityTypeEnum.Planets:
			{
				foreach (MyEntity item6 in entities)
				{
					MyEntityListInfoItem myEntityListInfoItem2 = GenerateInfo_Planet(item6, ref players);
					if (myEntityListInfoItem2 != null)
					{
						list.Add(myEntityListInfoItem2);
					}
				}
				return list;
			}
			case MyEntityTypeEnum.Asteroids:
			{
				foreach (MyEntity item7 in entities)
				{
					MyEntityListInfoItem myEntityListInfoItem = GenerateInfo_Asteroid(item7, ref players);
					if (myEntityListInfoItem != null)
					{
						list.Add(myEntityListInfoItem);
					}
				}
				return list;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private static MyCubeGrid GetMechanicalRootGrid(MyCubeGrid grid)
		{
			MyCubeGrid myCubeGrid = null;
			foreach (MyCubeGrid groupNode in MyCubeGridGroups.Static.Mechanical.GetGroupNodes(grid))
			{
				if (myCubeGrid == null || groupNode.CubeBlocks.get_Count() > myCubeGrid.CubeBlocks.get_Count())
				{
					myCubeGrid = groupNode;
				}
			}
			return myCubeGrid;
		}

		public static string GetDescriptionText(MyEntityListInfoItem item)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!item.IsGrid)
			{
				stringBuilder.Append(string.Concat(MyEntitySortOrder.Mass, ": "));
				if (item.Mass > 0f)
				{
					MyValueFormatter.AppendWeightInBestUnit(item.Mass, stringBuilder);
				}
				else
				{
					stringBuilder.Append("-");
				}
				stringBuilder.AppendLine();
				stringBuilder.Append(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.DistanceFromCenter.ToString())), ": "));
				MyValueFormatter.AppendDistanceInBestUnit((float)item.Position.Length(), stringBuilder);
				stringBuilder.AppendLine();
				stringBuilder.Append(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.Speed.ToString())), ": ", item.Speed, " m/s"));
				stringBuilder.AppendLine();
				stringBuilder.Append((object)MyTexts.Get(MySpaceTexts.TieredUpdate_IsReplicated));
				stringBuilder.Append((object)MyTexts.Get((!item.IsReplicated.HasValue) ? MySpaceTexts.TieredUpdate_IsReplicated_NA : (item.IsReplicated.Value ? MySpaceTexts.TieredUpdate_IsReplicated_True : MySpaceTexts.TieredUpdate_IsReplicated_False)));
			}
			else
			{
				stringBuilder.AppendLine(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.BlockCount.ToString())), ": ", item.BlockCount));
				if (item.PCU.HasValue && item.PCU.HasValue)
				{
					stringBuilder.AppendLine(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.PCU.ToString())), ": ", item.PCU.Value));
				}
				stringBuilder.Append(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.Mass.ToString())), ": "));
				if (item.Mass > 0f)
				{
					MyValueFormatter.AppendWeightInBestUnit(item.Mass, stringBuilder);
				}
				else
				{
					stringBuilder.Append("-");
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.OwnerName.ToString())), ": ", item.OwnerName));
				stringBuilder.AppendLine(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.Speed.ToString())), ": ", item.Speed, " m/s"));
				stringBuilder.Append(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.DistanceFromCenter.ToString())), ": "));
				MyValueFormatter.AppendDistanceInBestUnit((float)item.Position.Length(), stringBuilder);
				stringBuilder.AppendLine();
				stringBuilder.Append(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.DistanceFromPlayers.ToString())), ": "));
				MyValueFormatter.AppendDistanceInBestUnit(item.DistanceFromPlayers, stringBuilder);
				stringBuilder.AppendLine();
				stringBuilder.Append(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.OwnerLastLogout.ToString())), ": "));
				if (item.OwnerLogoutTime.HasValue && item.OwnerLogoutTime.HasValue)
				{
					MyValueFormatter.AppendTimeInBestUnit(item.OwnerLogoutTime.Value, stringBuilder);
				}
				string value = item.PlayerPresenceTier.ToString();
				string value2 = item.GridPresenceTier.ToString();
				stringBuilder.AppendLine();
				stringBuilder.Append((object)MyTexts.Get(MySpaceTexts.TieredUpdate_PlayerPresenceTier));
				stringBuilder.Append(value);
				stringBuilder.AppendLine();
				stringBuilder.Append((object)MyTexts.Get(MySpaceTexts.TieredUpdate_GridPresenceTier));
				stringBuilder.Append(value2);
				stringBuilder.AppendLine();
				stringBuilder.Append((object)MyTexts.Get(MySpaceTexts.TieredUpdate_IsReplicated));
				stringBuilder.Append((object)MyTexts.Get((!item.IsReplicated.HasValue) ? MySpaceTexts.TieredUpdate_IsReplicated_NA : (item.IsReplicated.Value ? MySpaceTexts.TieredUpdate_IsReplicated_True : MySpaceTexts.TieredUpdate_IsReplicated_False)));
			}
			return stringBuilder.ToString();
		}

		public static StringBuilder GetFormattedDisplayName(MyEntitySortOrder selectedOrder, MyEntityListInfoItem item)
		{
			StringBuilder stringBuilder = new StringBuilder(item.DisplayName);
			switch (selectedOrder)
			{
			case MyEntitySortOrder.BlockCount:
				if (item.IsGrid)
				{
					stringBuilder.Append(" | " + item.BlockCount);
				}
				break;
			case MyEntitySortOrder.PCU:
				if (item.PCU.HasValue && item.PCU.HasValue)
				{
					string text = ((item.PCU.Value == int.MaxValue) ? "N/A" : item.PCU.Value.ToString());
					stringBuilder.Append(" | " + text);
				}
				break;
			case MyEntitySortOrder.Mass:
				stringBuilder.Append(" | ");
				if (item.Mass == 0f)
				{
					stringBuilder.Append("-");
				}
				else
				{
					MyValueFormatter.AppendWeightInBestUnit(item.Mass, stringBuilder);
				}
				break;
			case MyEntitySortOrder.OwnerName:
				if (item.IsGrid)
				{
					stringBuilder.Append(" | " + (string.IsNullOrEmpty(item.OwnerName) ? MyTexts.GetString(MySpaceTexts.BlockOwner_Nobody) : item.OwnerName));
				}
				break;
			case MyEntitySortOrder.DistanceFromCenter:
				stringBuilder.Append(" | ");
				MyValueFormatter.AppendDistanceInBestUnit((float)item.Position.Length(), stringBuilder);
				break;
			case MyEntitySortOrder.Speed:
				stringBuilder.Append(" | " + item.Speed.ToString("0.### m/s"));
				break;
			case MyEntitySortOrder.DistanceFromPlayers:
				stringBuilder.Append(" | ");
				MyValueFormatter.AppendDistanceInBestUnit(item.DistanceFromPlayers, stringBuilder);
				break;
			case MyEntitySortOrder.OwnerLastLogout:
				if (item.OwnerLogoutTime.HasValue && item.OwnerLogoutTime.HasValue && item.OwnerLogoutTime.Value >= 0f)
				{
					if (item.OwnerName != item.DisplayName)
					{
						stringBuilder.Append(" | " + (string.IsNullOrEmpty(item.OwnerName) ? MyTexts.GetString(MySpaceTexts.BlockOwner_Nobody) : item.OwnerName));
						stringBuilder.Append(": ");
					}
					else
					{
						stringBuilder.Append(" | ");
					}
					MyValueFormatter.AppendTimeInBestUnit(item.OwnerLogoutTime.Value, stringBuilder);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case MyEntitySortOrder.DisplayName:
				break;
			}
			return stringBuilder;
		}

		public static void SortEntityList(MyEntitySortOrder selectedOrder, ref List<MyEntityListInfoItem> items, bool invertOrder)
		{
			switch (selectedOrder)
			{
			case MyEntitySortOrder.DisplayName:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					int num = string.Compare(a.DisplayName, b.DisplayName, StringComparison.CurrentCultureIgnoreCase);
					return invertOrder ? (-num) : num;
				});
				break;
			case MyEntitySortOrder.BlockCount:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					int num2 = b.BlockCount.CompareTo(a.BlockCount);
					return invertOrder ? (-num2) : num2;
				});
				break;
			case MyEntitySortOrder.PCU:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					if (b.PCU.HasValue && b.PCU.HasValue && a.PCU.HasValue && a.PCU.HasValue)
					{
						int num3 = b.PCU.Value.CompareTo(a.PCU.Value);
						if (invertOrder)
						{
							return -num3;
						}
						return num3;
					}
					return 1;
				});
				break;
			case MyEntitySortOrder.Mass:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					if (a.Mass == b.Mass)
					{
						return 0;
					}
					int num4 = ((a.Mass == 0f) ? (-1) : ((b.Mass == 0f) ? 1 : b.Mass.CompareTo(a.Mass)));
					return invertOrder ? (-num4) : num4;
				});
				break;
			case MyEntitySortOrder.OwnerName:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					int num5 = string.Compare(a.OwnerName, b.OwnerName, StringComparison.CurrentCultureIgnoreCase);
					return invertOrder ? (-num5) : num5;
				});
				break;
			case MyEntitySortOrder.DistanceFromCenter:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					int num6 = a.Position.LengthSquared().CompareTo(b.Position.LengthSquared());
					return invertOrder ? (-num6) : num6;
				});
				break;
			case MyEntitySortOrder.Speed:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					int num7 = b.Speed.CompareTo(a.Speed);
					return invertOrder ? (-num7) : num7;
				});
				break;
			case MyEntitySortOrder.DistanceFromPlayers:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					int num8 = b.DistanceFromPlayers.CompareTo(a.DistanceFromPlayers);
					return invertOrder ? (-num8) : num8;
				});
				break;
			case MyEntitySortOrder.OwnerLastLogout:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					if (b.OwnerLogoutTime.HasValue && b.OwnerLogoutTime.HasValue && a.OwnerLogoutTime.HasValue && a.OwnerLogoutTime.HasValue)
					{
						int num9 = b.OwnerLogoutTime.Value.CompareTo(a.OwnerLogoutTime.Value);
						if (invertOrder)
						{
							return -num9;
						}
						return num9;
					}
					return 1;
				});
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public static void ProceedEntityAction(MyEntity entity, EntityListAction action)
		{
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyCubeGrid myCubeGrid2 = myCubeGrid.GetTopMostParent() as MyCubeGrid;
				if (myCubeGrid2 == null)
				{
					myCubeGrid2 = myCubeGrid;
				}
				if (!myCubeGrid2.IsGenerated)
				{
					if (action == EntityListAction.Remove)
					{
						myCubeGrid.DismountAllCockpits();
					}
					ProceedEntityActionHierarchy(MyGridPhysicalHierarchy.Static.GetRoot(myCubeGrid), action);
				}
			}
			else
			{
				ProceedEntityActionInternal(entity, action);
			}
		}

		private static void ProceedEntityActionHierarchy(MyCubeGrid grid, EntityListAction action)
		{
			MyGridPhysicalHierarchy.Static.ApplyOnChildren(grid, delegate(MyCubeGrid x)
			{
				ProceedEntityActionHierarchy(x, action);
			});
			ProceedEntityActionInternal(grid, action);
		}

		private static void ProceedEntityActionInternal(MyEntity entity, EntityListAction action)
		{
			switch (action)
			{
			case EntityListAction.Remove:
				entity.Close();
				break;
			case EntityListAction.Stop:
				Stop(entity);
				break;
			case EntityListAction.Depower:
				Depower(entity);
				break;
			case EntityListAction.Power:
				Power(entity);
				break;
			}
		}

		private static void Stop(MyEntity entity)
		{
			if (entity.Physics != null)
			{
				entity.Physics.LinearVelocity = Vector3.Zero;
				entity.Physics.AngularVelocity = Vector3.Zero;
			}
		}

		private static void Depower(MyEntity entity)
		{
			(entity as MyCubeGrid)?.ChangePowerProducerState(MyMultipleEnabledEnum.AllDisabled, -1L);
		}

		private static void Power(MyEntity entity)
		{
			(entity as MyCubeGrid)?.ChangePowerProducerState(MyMultipleEnabledEnum.AllEnabled, -1L);
		}

		private static void AccountChildren(MyCubeGrid grid)
		{
			MyGridPhysicalHierarchy.Static.ApplyOnChildren(grid, delegate(MyCubeGrid childGrid)
			{
				CreateListInfoForGrid(childGrid, out var item);
				m_gridItem.Add(ref item);
				AccountChildren(childGrid);
			});
		}

		private static void CreateListInfoForGrid(MyCubeGrid grid, out MyEntityListInfoItem item)
		{
			long owner = 0L;
			string ownerName = string.Empty;
			if (grid.BigOwners.Count > 0)
			{
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(grid.BigOwners[0]);
				if (myIdentity != null)
				{
					ownerName = myIdentity.DisplayName;
					owner = grid.BigOwners[0];
				}
			}
			MyUpdateTiersPlayerPresence playerPresenceTier = grid.PlayerPresenceTier;
			MyUpdateTiersGridPresence gridPresenceTier = grid.GridPresenceTier;
			item = new MyEntityListInfoItem(grid.DisplayName, grid.EntityId, grid.BlocksCount, grid.BlocksPCU, grid.Physics.Mass, grid.PositionComp.GetPosition(), grid.Physics.LinearVelocity.Length(), MySession.GetPlayerDistance(grid, MySession.Static.Players.GetOnlinePlayers()), ownerName, owner, MySession.GetOwnerLoginTimeSeconds(grid), MySession.GetOwnerLogoutTimeSeconds(grid), playerPresenceTier, gridPresenceTier, grid.IsReplicated, isGrid: true);
		}
	}
}
