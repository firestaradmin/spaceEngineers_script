using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.Collections.ObjectModel;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Definitions.GUI;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.AdminMenu;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using Sandbox.ModAPI;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.SessionComponents;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[PreloadRequired]
	[StaticEventOwner]
	public class MyGuiScreenAdminMenu : MyGuiScreenDebugBase
	{
		private enum TrashTab
		{
			General,
			Voxels
		}

		public enum MyPageEnum
		{
			AdminTools,
			TrashRemoval,
			CycleObjects,
			EntityList,
			SafeZones,
			GlobalSafeZone,
			ReplayTool,
			Economy,
			Weather,
			Spectator,
			Match
		}

		private struct MyIdNamePair
		{
			public long Id;

			public string Name;
		}

		private class MyIdNamePairComparer : IComparer<MyIdNamePair>
		{
			public int Compare(MyIdNamePair x, MyIdNamePair y)
			{
				return string.Compare(x.Name, y.Name);
			}
		}

		[Serializable]
		internal struct AdminSettings
		{
			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EFlags_003C_003EAccessor : IMemberAccessor<AdminSettings, MyTrashRemovalFlags>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in MyTrashRemovalFlags value)
				{
					owner.Flags = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out MyTrashRemovalFlags value)
				{
					value = owner.Flags;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EEnable_003C_003EAccessor : IMemberAccessor<AdminSettings, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in bool value)
				{
					owner.Enable = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out bool value)
				{
					value = owner.Enable;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EBlockCount_003C_003EAccessor : IMemberAccessor<AdminSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in int value)
				{
					owner.BlockCount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out int value)
				{
					value = owner.BlockCount;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EPlayerDistance_003C_003EAccessor : IMemberAccessor<AdminSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in float value)
				{
					owner.PlayerDistance = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out float value)
				{
					value = owner.PlayerDistance;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EGridCount_003C_003EAccessor : IMemberAccessor<AdminSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in int value)
				{
					owner.GridCount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out int value)
				{
					value = owner.GridCount;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EPlayerInactivity_003C_003EAccessor : IMemberAccessor<AdminSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in float value)
				{
					owner.PlayerInactivity = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out float value)
				{
					value = owner.PlayerInactivity;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003ECharacterRemovalThreshold_003C_003EAccessor : IMemberAccessor<AdminSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in int value)
				{
					owner.CharacterRemovalThreshold = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out int value)
				{
					value = owner.CharacterRemovalThreshold;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EAfkTimeout_003C_003EAccessor : IMemberAccessor<AdminSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in int value)
				{
					owner.AfkTimeout = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out int value)
				{
					value = owner.AfkTimeout;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EStopGridsPeriod_003C_003EAccessor : IMemberAccessor<AdminSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in int value)
				{
					owner.StopGridsPeriod = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out int value)
				{
					value = owner.StopGridsPeriod;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003ERemoveOldIdentities_003C_003EAccessor : IMemberAccessor<AdminSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in int value)
				{
					owner.RemoveOldIdentities = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out int value)
				{
					value = owner.RemoveOldIdentities;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EVoxelEnable_003C_003EAccessor : IMemberAccessor<AdminSettings, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in bool value)
				{
					owner.VoxelEnable = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out bool value)
				{
					value = owner.VoxelEnable;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EVoxelDistanceFromPlayer_003C_003EAccessor : IMemberAccessor<AdminSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in float value)
				{
					owner.VoxelDistanceFromPlayer = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out float value)
				{
					value = owner.VoxelDistanceFromPlayer;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EVoxelDistanceFromGrid_003C_003EAccessor : IMemberAccessor<AdminSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in float value)
				{
					owner.VoxelDistanceFromGrid = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out float value)
				{
					value = owner.VoxelDistanceFromGrid;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EVoxelAge_003C_003EAccessor : IMemberAccessor<AdminSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in int value)
				{
					owner.VoxelAge = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out int value)
				{
					value = owner.VoxelAge;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EAdminSettingsFlags_003C_003EAccessor : IMemberAccessor<AdminSettings, AdminSettingsEnum>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in AdminSettingsEnum value)
				{
					owner.AdminSettingsFlags = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out AdminSettingsEnum value)
				{
					value = owner.AdminSettingsFlags;
				}
			}

			public MyTrashRemovalFlags Flags;

			public bool Enable;

			public int BlockCount;

			public float PlayerDistance;

			public int GridCount;

			public float PlayerInactivity;

			public int CharacterRemovalThreshold;

			public int AfkTimeout;

			public int StopGridsPeriod;

			public int RemoveOldIdentities;

			public bool VoxelEnable;

			public float VoxelDistanceFromPlayer;

			public float VoxelDistanceFromGrid;

			public int VoxelAge;

			public AdminSettingsEnum AdminSettingsFlags;
		}

		public enum MyZoneAxisTypeEnum
		{
			X,
			Y,
			Z
		}

		public enum MyRestrictedTypeEnum
		{
			Player,
			Faction,
			Grid,
			FloatingObjects
		}

		private class MySafezoneNameComparer : IComparer<MySafeZone>
		{
			public int Compare(MySafeZone x, MySafeZone y)
			{
				if (x == null)
				{
					return -1;
				}
				if (y == null)
				{
					return 0;
				}
				return string.Compare(x.DisplayName, y.DisplayName);
			}
		}

		protected sealed class RequestReputation_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerIdentityId, in long factionId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestReputation(playerIdentityId, factionId);
			}
		}

		protected sealed class RequestReputationCallback_003C_003ESystem_Int64_0023System_Int64_0023System_Int32 : ICallSite<IMyEventOwner, long, long, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerIdentityId, in long factionId, in int reputation, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestReputationCallback(playerIdentityId, factionId, reputation);
			}
		}

		protected sealed class RequestChangeReputation_003C_003ESystem_Int64_0023System_Int64_0023System_Int32_0023System_Boolean : ICallSite<IMyEventOwner, long, long, int, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in long factionId, in int reputationChange, in bool shouldPropagate, in DBNull arg5, in DBNull arg6)
			{
				RequestChangeReputation(identityId, factionId, reputationChange, shouldPropagate);
			}
		}

		protected sealed class RequestBalance_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long accountOwner, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestBalance(accountOwner);
			}
		}

		protected sealed class RequestBalanceCallback_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long accountOwner, in long balance, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestBalanceCallback(accountOwner, balance);
			}
		}

		protected sealed class RequestChange_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long accountOwner, in long balanceChange, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestChange(accountOwner, balanceChange);
			}
		}

		protected sealed class RequestSettingFromServer_Implementation_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestSettingFromServer_Implementation();
			}
		}

		protected sealed class AskIsValidForEdit_Server_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AskIsValidForEdit_Server(entityId);
			}
		}

		protected sealed class AskIsValidForEdit_Reponse_003C_003ESystem_Int64_0023System_Boolean : ICallSite<IMyEventOwner, long, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in bool canEdit, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AskIsValidForEdit_Reponse(entityId, canEdit);
			}
		}

		protected sealed class EntityListRequest_003C_003ESandbox_Game_Entities_MyEntityList_003C_003EMyEntityTypeEnum_0023System_Boolean : ICallSite<IMyEventOwner, MyEntityList.MyEntityTypeEnum, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyEntityList.MyEntityTypeEnum selectedType, in bool requestedBySafeZoneFilter, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				EntityListRequest(selectedType, requestedBySafeZoneFilter);
			}
		}

		protected sealed class CycleRequest_Implementation_003C_003ESandbox_Game_Entities_MyEntityCyclingOrder_0023System_Boolean_0023System_Boolean_0023System_Single_0023System_Int64_0023Sandbox_Game_Entities_CyclingOptions : ICallSite<IMyEventOwner, MyEntityCyclingOrder, bool, bool, float, long, CyclingOptions>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyEntityCyclingOrder order, in bool reset, in bool findLarger, in float metricValue, in long currentEntityId, in CyclingOptions options)
			{
				CycleRequest_Implementation(order, reset, findLarger, metricValue, currentEntityId, options);
			}
		}

		protected sealed class RemoveOwner_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CSystem_Int64_003E_0023System_Collections_Generic_List_00601_003CSystem_Int64_003E : ICallSite<IMyEventOwner, List<long>, List<long>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<long> owners, in List<long> entityIds, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveOwner_Implementation(owners, entityIds);
			}
		}

		protected sealed class ProceedEntitiesAction_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CSystem_Int64_003E_0023Sandbox_Game_Entities_MyEntityList_003C_003EEntityListAction : ICallSite<IMyEventOwner, List<long>, MyEntityList.EntityListAction, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<long> entityIds, in MyEntityList.EntityListAction action, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ProceedEntitiesAction_Implementation(entityIds, action);
			}
		}

		protected sealed class UploadSettingsToServer_003C_003ESandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings : ICallSite<IMyEventOwner, AdminSettings, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AdminSettings settings, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UploadSettingsToServer(settings);
			}
		}

		protected sealed class ProceedEntity_Implementation_003C_003ESystem_Int64_0023Sandbox_Game_Entities_MyEntityList_003C_003EEntityListAction : ICallSite<IMyEventOwner, long, MyEntityList.EntityListAction, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in MyEntityList.EntityListAction action, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ProceedEntity_Implementation(entityId, action);
			}
		}

		protected sealed class ReplicateEverything_Implementation_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ReplicateEverything_Implementation();
			}
		}

		protected sealed class AdminSettingsChanged_003C_003ESandbox_Game_World_AdminSettingsEnum_0023System_UInt64 : ICallSite<IMyEventOwner, AdminSettingsEnum, ulong, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AdminSettingsEnum settings, in ulong steamId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AdminSettingsChanged(settings, steamId);
			}
		}

		protected sealed class AdminSettingsChangedClient_003C_003ESandbox_Game_World_AdminSettingsEnum_0023System_UInt64 : ICallSite<IMyEventOwner, AdminSettingsEnum, ulong, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AdminSettingsEnum settings, in ulong steamId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AdminSettingsChangedClient(settings, steamId);
			}
		}

		protected sealed class EntityListResponse_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003E : ICallSite<IMyEventOwner, List<MyEntityList.MyEntityListInfoItem>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<MyEntityList.MyEntityListInfoItem> entities, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				EntityListResponse(entities);
			}
		}

		protected sealed class Cycle_Implementation_003C_003ESystem_Single_0023System_Int64_0023VRageMath_Vector3D_0023System_Boolean : ICallSite<IMyEventOwner, float, long, Vector3D, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float newMetricValue, in long newEntityId, in Vector3D position, in bool isNpcStation, in DBNull arg5, in DBNull arg6)
			{
				Cycle_Implementation(newMetricValue, newEntityId, position, isNpcStation);
			}
		}

		protected sealed class DownloadSettingFromServer_003C_003ESandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings : ICallSite<IMyEventOwner, AdminSettings, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AdminSettings settings, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DownloadSettingFromServer(settings);
			}
		}

		protected sealed class SendDataToClient_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SendDataToClient();
			}
		}

<<<<<<< HEAD
		protected sealed class ReciveClientData_003C_003ESandbox_Game_SessionComponents_MyMatchState_0023System_Single_0023System_Boolean_0023System_Boolean : ICallSite<IMyEventOwner, MyMatchState, float, bool, bool, DBNull, DBNull>
=======
		protected sealed class ReciveClientData_003C_003E_MyMatchState_0023System_Single_0023System_Boolean_0023System_Boolean : ICallSite<IMyEventOwner, MyMatchState, float, bool, bool, DBNull, DBNull>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyMatchState state, in float remainingTime, in bool isRunning, in bool isEnabled, in DBNull arg5, in DBNull arg6)
			{
				ReciveClientData(state, remainingTime, isRunning, isEnabled);
			}
		}

		protected sealed class StartMatchInternal_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				StartMatchInternal();
			}
		}

		protected sealed class StopMatchInternal_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				StopMatchInternal();
			}
		}

		protected sealed class PauseMatchInternal_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				PauseMatchInternal();
			}
		}

		protected sealed class ProgressMatchInternal_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ProgressMatchInternal();
			}
		}

		protected sealed class SetTimeInternal_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float value, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetTimeInternal(value);
			}
		}

		protected sealed class AddTimeInternal_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float value, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AddTimeInternal(value);
			}
		}

<<<<<<< HEAD
		protected sealed class SaveZoneRenameServer_003C_003ESystem_Int64_0023System_String : ICallSite<IMyEventOwner, long, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in string newName, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SaveZoneRenameServer(entityId, newName);
			}
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static readonly int BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE = 2000;

		internal static readonly float TEXT_ALIGN_CONST = 0.05f;

		private static readonly Vector2 CB_OFFSET = new Vector2(-0.05f, 0f);

		private static MyGuiScreenAdminMenu m_static;

		private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 1.2f);

		private static readonly float HIDDEN_PART_RIGHT = 0.04f;

		private readonly Vector2 m_controlPadding = new Vector2(0.02f, 0.02f);

		protected static MyEntityCyclingOrder m_order;

		private static float m_metricValue = 0f;

		private static long m_entityId;

		private long m_attachCamera;

		private bool m_attachIsNpcStation;

		private MyGuiControlLabel m_labelCurrentIndex;

		private MyGuiControlLabel m_labelEntityName;

		protected MyGuiControlButton m_removeItemButton;

		private MyGuiControlButton m_depowerItemButton;

		protected MyGuiControlButton m_stopItemButton;

		protected MyGuiControlCheckbox m_onlySmallGridsCheckbox;

		private MyGuiControlCheckbox m_onlyLargeGridsCheckbox;

		private static CyclingOptions m_cyclingOptions = default(CyclingOptions);

		protected Vector4 m_labelColor = Color.White.ToVector4();

		protected MyGuiControlCheckbox m_creativeCheckbox;

		private readonly List<IMyGps> m_gpsList = new List<IMyGps>();

		protected MyGuiControlCombobox m_modeCombo;

		protected MyGuiControlCombobox m_onlinePlayerCombo;

		protected long m_onlinePlayerCombo_SelectedPlayerIdentityId;

		protected MyGuiControlTextbox m_addCurrencyTextbox;

		protected MyGuiControlButton m_addCurrencyConfirmButton;

		protected MyGuiControlLabel m_labelCurrentBalanceValue;

		protected MyGuiControlLabel m_labelFinalBalanceValue;

		protected int m_playerCount;

		protected int m_factionCount;

		protected bool m_isPlayerSelected;

		protected bool m_isFactionSelected;

		protected long m_currentBalance;

		protected long m_finalBalance;

		protected long m_balanceDifference;

		protected MyGuiControlCombobox m_playerReputationCombo;

		protected long m_playerReputationCombo_SelectedPlayerIdentityId;

		protected MyGuiControlCombobox m_factionReputationCombo;

		protected long m_factionReputationCombo_SelectedPlayerIdentityId;

		protected MyGuiControlTextbox m_addReputationTextbox;

		protected MyGuiControlButton m_addReputationConfirmButton;

		protected MyGuiControlLabel m_labelCurrentReputationValue;

		protected MyGuiControlLabel m_labelFinalReputationValue;

		protected MyGuiControlCheckbox m_addReputationPropagate;

		protected int m_currentReputation;

		protected int m_finalReputation;

		protected int m_reputationDifference;

		protected MyGuiControlCheckbox m_invulnerableCheckbox;

		protected MyGuiControlCheckbox m_untargetableCheckbox;

		protected MyGuiControlCheckbox m_showPlayersCheckbox;

		protected MyGuiControlCheckbox m_keepOriginalOwnershipOnPasteCheckBox;

		protected MyGuiControlCheckbox m_ignoreSafeZonesCheckBox;

		protected MyGuiControlCheckbox m_ignorePcuCheckBox;

		protected MyGuiControlCheckbox m_canUseTerminals;

		protected MyGuiControlSlider m_timeDelta;

		protected MyGuiControlLabel m_timeDeltaValue;

		protected MyGuiControlListbox m_entityListbox;

		protected MyGuiControlCombobox m_entityTypeCombo;

		protected MyGuiControlCombobox m_entitySortCombo;

		private MyEntityList.MyEntityTypeEnum m_selectedType;

		private MyEntityList.MyEntitySortOrder m_selectedSort;

		private static bool m_invertOrder;

		private static HashSet<long> m_protectedCharacters = new HashSet<long>();

		private static MyPageEnum m_currentPage;

		private int m_currentGpsIndex;

		private bool m_unsavedTrashSettings;

		private AdminSettings m_newSettings;

		private bool m_unsavedTrashExitBoxIsOpened;

		private MyGuiControlCombobox m_trashRemovalCombo;

		private MyGuiControlStackPanel m_trashRemovalContentPanel;

		private Dictionary<MyTabControlEnum, MyTabContainer> m_tabs = new Dictionary<MyTabControlEnum, MyTabContainer>();

		private MyGuiScreenMessageBox m_cleanupRequestingMessageBox;

		protected MyGuiControlLabel m_enabledCheckboxGlobalLabel;

		protected MyGuiControlLabel m_damageCheckboxGlobalLabel;

		protected MyGuiControlLabel m_shootingCheckboxGlobalLabel;

		protected MyGuiControlLabel m_drillingCheckboxGlobalLabel;

		protected MyGuiControlLabel m_weldingCheckboxGlobalLabel;

		protected MyGuiControlLabel m_grindingCheckboxGlobalLabel;

		protected MyGuiControlLabel m_voxelHandCheckboxGlobalLabel;

		protected MyGuiControlLabel m_buildingCheckboxGlobalLabel;

		protected MyGuiControlLabel m_buildingProjectionsCheckboxGlobalLabel;

		protected MyGuiControlLabel m_landingGearCheckboxGlobalLabel;

		protected MyGuiControlLabel m_convertToStationCheckboxGlobalLabel;

		protected MyGuiControlCheckbox m_enabledGlobalCheckbox;

		protected MyGuiControlCheckbox m_damageGlobalCheckbox;

		protected MyGuiControlCheckbox m_shootingGlobalCheckbox;

		protected MyGuiControlCheckbox m_drillingGlobalCheckbox;

		protected MyGuiControlCheckbox m_weldingGlobalCheckbox;

		protected MyGuiControlCheckbox m_grindingGlobalCheckbox;

		protected MyGuiControlCheckbox m_voxelHandGlobalCheckbox;

		protected MyGuiControlCheckbox m_buildingGlobalCheckbox;

		protected MyGuiControlCheckbox m_buildingProjectionsGlobalCheckbox;

		protected MyGuiControlCheckbox m_landingGearGlobalCheckbox;

		protected MyGuiControlCheckbox m_convertToStationGlobalCheckbox;

		private static readonly int LOOP_LIMIT = 5;

		private static List<MyGuiScreenAdminMenu> m_matchSyncReceivers = new List<MyGuiScreenAdminMenu>();

		private MyGuiControlLabel m_labelEnabled;

		private MyGuiControlLabel m_labelRunning;

		private MyGuiControlLabel m_labelState;

		private MyGuiControlLabel m_labelTime;

		private MyGuiControlButton m_buttonStart;

		private MyGuiControlButton m_buttonStop;

		private MyGuiControlButton m_buttonPause;

		private MyGuiControlButton m_buttonAdvanced;

		private MyGuiControlButton m_buttonSetTime;

		private MyGuiControlButton m_buttonAddTime;

		private MyGuiControlTextbox m_textboxTime;

		private bool m_isMatchEnabled;

		private bool m_isMatchRunning;

		private MyMatchState m_matchCurrentState;

		private MyTimeSpan m_matchRemainingTime;

		private MyTimeSpan m_matchLastUpdateTime;

		protected MyGuiControlScrollablePanel m_optionsGroup;

		protected MyGuiControlLabel m_selectSafeZoneLabel;

		protected MyGuiControlLabel m_selectZoneShapeLabel;

		protected MyGuiControlLabel m_selectAxisLabel;

		protected MyGuiControlLabel m_zoneRadiusLabel;

		protected MyGuiControlLabel m_zoneSizeLabel;

		protected MyGuiControlLabel m_zoneRadiusValueLabel;

		protected MyGuiControlCombobox m_safeZonesCombo;

		protected MyGuiControlCombobox m_safeZonesTypeCombo;

		protected MyGuiControlCombobox m_safeZonesAxisCombo;

		protected MyGuiControlSlider m_sizeSlider;

		protected MyGuiControlSlider m_radiusSlider;

		protected MyGuiControlButton m_addSafeZoneButton;

		protected MyGuiControlButton m_repositionSafeZoneButton;

		protected MyGuiControlButton m_moveToSafeZoneButton;

		protected MyGuiControlButton m_removeSafeZoneButton;

		protected MyGuiControlButton m_renameSafeZoneButton;

		protected MyGuiControlButton m_configureFilterButton;

		protected MyGuiControlLabel m_enabledCheckboxLabel;

		protected MyGuiControlLabel m_damageCheckboxLabel;

		protected MyGuiControlLabel m_shootingCheckboxLabel;

		protected MyGuiControlLabel m_drillingCheckboxLabel;

		protected MyGuiControlLabel m_weldingCheckboxLabel;

		protected MyGuiControlLabel m_grindingCheckboxLabel;

		protected MyGuiControlLabel m_voxelHandCheckboxLabel;

		protected MyGuiControlLabel m_buildingCheckboxLabel;

		protected MyGuiControlLabel m_buildingProjectionsCheckboxLabel;

		protected MyGuiControlLabel m_landingGearLockCheckboxLabel;

		protected MyGuiControlLabel m_convertToStationCheckboxLabel;

		protected MyGuiControlCheckbox m_enabledCheckbox;

		protected MyGuiControlCheckbox m_damageCheckbox;

		protected MyGuiControlCheckbox m_shootingCheckbox;

		protected MyGuiControlCheckbox m_drillingCheckbox;

		protected MyGuiControlCheckbox m_weldingCheckbox;

		protected MyGuiControlCheckbox m_grindingCheckbox;

		protected MyGuiControlCheckbox m_voxelHandCheckbox;

		protected MyGuiControlCheckbox m_buildingCheckbox;

		protected MyGuiControlCheckbox m_buildingProjectionsCheckbox;

		protected MyGuiControlCheckbox m_landingGearLockCheckbox;

		protected MyGuiControlCheckbox m_convertToStationCheckbox;

		private MyGuiControlCombobox m_textureCombo;

		private MyGuiControlColor m_colorSelector;

		private MySafeZone m_selectedSafeZone;

		private bool m_recreateInProgress;

		private MyGuiControlCombobox m_cameraModeCombo;

		private MyGuiControlSlider m_cameraSmoothness;

		private MyGuiControlLabel m_cameraSmoothnessValueLabel;

		private MyGuiControlListbox m_trackedListbox;

		private const int UPDATE_INTERVAL = 100;

		private string m_selectedWeatherSubtypeId;

		private MyGuiControlCombobox m_weatherSelectionCombo;

		private MyGuiControlMultilineText messageMultiline;

		private Dictionary<int, Tuple<string, string>> m_weatherNamesAndSubtypesDictionary;

		private int m_weatherUpdateCounter;

		private MyWeatherEffectDefinition[] m_definitions;

		public MyGuiScreenAdminMenu()
			: base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - SCREEN_SIZE.X * 0.5f + HIDDEN_PART_RIGHT, 0.5f), SCREEN_SIZE, MyGuiConstants.SCREEN_BACKGROUND_COLOR, isTopMostScreen: false)
		{
			m_backgroundTransition = MySandboxGame.Config.UIBkOpacity;
			m_guiTransition = MySandboxGame.Config.UIOpacity;
			if (MyPlatformGameSettings.ENABLE_LOW_MEM_WORLD_LOCKDOWN && MySandboxGame.Static.MemoryState == MySandboxGame.MemState.Critical)
			{
				ShowCleanupRequest();
				m_currentPage = MyPageEnum.EntityList;
			}
			if (!Sync.IsServer)
			{
				m_static = this;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestSettingFromServer_Implementation);
			}
			else
			{
				CreateScreen();
			}
			MySessionComponentSafeZones.OnAddSafeZone += MySafeZones_OnAddSafeZone;
			MySessionComponentSafeZones.OnRemoveSafeZone += MySafeZones_OnRemoveSafeZone;
		}

		private void CreateScreen()
		{
			m_closeOnEsc = false;
			base.CanBeHidden = true;
			base.CanHideOthers = true;
			m_canCloseInCloseAllScreenCalls = true;
			m_canShareInput = true;
			m_isTopScreen = false;
			m_isTopMostScreen = false;
			StoreTrashSettings_RealToTmp();
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_tabs.Clear();
			Vector2 controlPadding = new Vector2(0.02f, 0.02f);
			float scale = 0.8f;
			float separatorSize = 0.01f;
			float num = SCREEN_SIZE.X - HIDDEN_PART_RIGHT - controlPadding.X * 2f;
			float num2 = (SCREEN_SIZE.Y - 1f) / 2f;
			m_static = this;
			m_currentPosition = -m_size.Value / 2f;
			m_currentPosition += controlPadding;
			m_currentPosition.Y += num2;
			m_scale = scale;
			AddCaption(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ModeSelect).ToString(), Color.White.ToVector4(), m_controlPadding + new Vector2(0f - HIDDEN_PART_RIGHT, num2 - 0.03f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.44f), m_size.Value.X * 0.73f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.365f), m_size.Value.X * 0.73f);
			Controls.Add(myGuiControlSeparatorList);
			m_currentPosition.X += 0.018f;
			m_currentPosition.Y += MyGuiConstants.SCREEN_CAPTION_DELTA_Y + controlPadding.Y - 0.012f;
			m_modeCombo = AddCombo();
			if (MySession.Static.IsUserSpaceMaster(Sync.MyId))
			{
				m_modeCombo.AddItem(0L, MySpaceTexts.ScreenDebugAdminMenu_AdminTools);
				m_modeCombo.AddItem(2L, MyCommonTexts.ScreenDebugAdminMenu_CycleObjects);
				if (MyPlatformGameSettings.ENABLE_TRASH_REMOVAL_SETTING || MyFakes.FORCE_ADD_TRASH_REMOVAL_MENU)
				{
					m_modeCombo.AddItem(1L, MySpaceTexts.ScreenDebugAdminMenu_Cleanup);
				}
				m_modeCombo.AddItem(3L, MySpaceTexts.ScreenDebugAdminMenu_EntityList);
				if (MySession.Static.IsUserAdmin(Sync.MyId))
				{
					m_modeCombo.AddItem(4L, MySpaceTexts.ScreenDebugAdminMenu_SafeZones);
					m_modeCombo.AddItem(5L, MySpaceTexts.ScreenDebugAdminMenu_GlobalSafeZone);
					m_modeCombo.AddItem(6L, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool);
					m_modeCombo.AddItem(7L, MySpaceTexts.ScreenDebugAdminMenu_Economy);
					m_modeCombo.AddItem(8L, MySpaceTexts.ScreenDebugAdminMenu_Weather);
<<<<<<< HEAD
					m_modeCombo.AddItem(9L, MySpaceTexts.ScreenDebugAdminMenu_Spectator);
=======
					m_modeCombo.AddItem(9L, "Spectator Tool");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_modeCombo.AddItem(10L, MySpaceTexts.ScreenDebugAdminMenu_Match);
				}
				else if (m_currentPage == MyPageEnum.GlobalSafeZone || m_currentPage == MyPageEnum.SafeZones || m_currentPage == MyPageEnum.ReplayTool || m_currentPage == MyPageEnum.Economy)
				{
					m_currentPage = MyPageEnum.CycleObjects;
				}
				m_modeCombo.SelectItemByKey((long)m_currentPage);
			}
			else
			{
				m_modeCombo.AddItem(0L, MySpaceTexts.ScreenDebugAdminMenu_AdminTools);
				m_currentPage = MyPageEnum.AdminTools;
				m_modeCombo.SelectItemByKey((long)m_currentPage);
			}
			m_modeCombo.ItemSelected += OnModeComboSelect;
			switch (m_currentPage)
			{
			case MyPageEnum.CycleObjects:
			{
				m_currentPosition.Y += 0.03f;
				MyGuiControlSeparatorList myGuiControlSeparatorList4 = new MyGuiControlSeparatorList();
				myGuiControlSeparatorList4.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.19f), m_size.Value.X * 0.73f);
				myGuiControlSeparatorList4.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, -0.138f), m_size.Value.X * 0.73f);
				myGuiControlSeparatorList4.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, -0.305f), m_size.Value.X * 0.73f);
				Controls.Add(myGuiControlSeparatorList4);
				MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel
				{
					Position = new Vector2(-0.16f, -0.335f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SortBy) + ":",
					IsAutoScaleEnabled = true,
					IsAutoEllipsisEnabled = true
				};
				myGuiControlLabel7.SetMaxWidth(0.065f);
				Controls.Add(myGuiControlLabel7);
				MyGuiControlCombobox myGuiControlCombobox = AddCombo(m_order, OnOrderChanged, enabled: true, 10, null, m_labelColor, isAutoscaleEnabled: true, isAutoEllipsisEnabled: true);
				myGuiControlCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				myGuiControlCombobox.PositionX = 0.122f;
				myGuiControlCombobox.Size = new Vector2(0.21f, 1f);
				m_currentPosition.Y += 0.005f;
				MyGuiControlLabel myGuiControlLabel8 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_SmallGrids)
				};
				myGuiControlLabel8.SetMaxWidth(0.065f);
				m_onlySmallGridsCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_onlySmallGridsCheckbox.IsCheckedChanged = OnSmallGridChanged;
				m_onlySmallGridsCheckbox.IsChecked = m_cyclingOptions.OnlySmallGrids;
				Controls.Add(m_onlySmallGridsCheckbox);
				Controls.Add(myGuiControlLabel8);
				m_currentPosition.Y += 0.045f;
				MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_LargeGrids)
				};
				myGuiControlLabel9.SetMaxWidth(0.065f);
				m_onlyLargeGridsCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_onlyLargeGridsCheckbox.IsCheckedChanged = OnLargeGridChanged;
				m_onlyLargeGridsCheckbox.IsChecked = m_cyclingOptions.OnlyLargeGrids;
				Controls.Add(m_onlyLargeGridsCheckbox);
				Controls.Add(myGuiControlLabel9);
				m_currentPosition.Y += 0.12f;
				float y = m_currentPosition.Y;
				MyGuiControlButton myGuiControlButton7 = CreateDebugButton(0.284f, MyCommonTexts.ScreenDebugAdminMenu_First, delegate
				{
					OnCycleClicked(reset: true, forward: true);
				});
				myGuiControlButton7.PositionX += 0.003f;
				myGuiControlButton7.PositionY -= 0.0435f;
				m_currentPosition.Y = y;
				CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Next, delegate
				{
					OnCycleClicked(reset: false, forward: false);
				}).PositionX = -0.088f;
				m_currentPosition.Y = y;
				CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Previous, delegate
				{
					OnCycleClicked(reset: false, forward: true);
				}).PositionX = 0.055f;
				m_labelEntityName = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EntityName) + " -",
					IsAutoScaleEnabled = true,
					IsAutoEllipsisEnabled = true
				};
				m_labelEntityName.SetMaxWidth(0.35f);
				Controls.Add(m_labelEntityName);
				m_currentPosition.Y += 0.035f;
				m_labelCurrentIndex = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_CurrentValue), (m_entityId == 0L) ? "-" : m_metricValue.ToString()).ToString()
				};
				Controls.Add(m_labelCurrentIndex);
				m_currentPosition.Y += 0.208f;
				y = m_currentPosition.Y;
				m_removeItemButton = CreateDebugButton(0.284f, MyCommonTexts.ScreenDebugAdminMenu_Remove, delegate
				{
					OnEntityOperationClicked(MyEntityList.EntityListAction.Remove);
				});
				m_removeItemButton.PositionX += 0.003f;
				m_currentPosition.Y = y;
				m_stopItemButton = CreateDebugButton(0.284f, MyCommonTexts.ScreenDebugAdminMenu_Stop, delegate
				{
					OnEntityOperationClicked(MyEntityList.EntityListAction.Stop);
				});
				m_stopItemButton.PositionX += 0.003f;
				m_stopItemButton.PositionY += 0.0435f;
				m_currentPosition.Y = y;
				m_depowerItemButton = CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugAdminMenu_Depower, delegate
				{
					OnEntityOperationClicked(MyEntityList.EntityListAction.Depower);
				});
				m_depowerItemButton.PositionX += 0.003f;
				m_depowerItemButton.PositionY += 0.087f;
				m_currentPosition.Y += 0.125f;
				y = m_currentPosition.Y;
				m_currentPosition.Y = y;
				MyGuiControlButton myGuiControlButton8 = CreateDebugButton(0.284f, MyCommonTexts.SpectatorControls_None, OnPlayerControl, enabled: true, MySpaceTexts.SpectatorControls_None_Desc, increaseSpacing: true, addToControls: true, isAutoScaleEnabled: true);
				myGuiControlButton8.PositionX += 0.003f;
				m_currentPosition.Y = y;
				MyGuiControlButton myGuiControlButton9 = CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugAdminMenu_TeleportHere, OnTeleportButton, MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.Parent == null && MySession.Static.IsUserSpaceMaster(Sync.MyId), MySpaceTexts.ScreenDebugAdminMenu_TeleportHereToolTip, increaseSpacing: true, addToControls: true, isAutoScaleEnabled: true);
				myGuiControlButton9.PositionX += 0.003f;
				myGuiControlButton9.PositionY += 0.0435f;
				myGuiControlButton9.TextScale = myGuiControlButton8.TextScale;
				bool flag2 = !Sync.IsServer;
				CreateDebugButton(0.284f, MyCommonTexts.ScreenDebugAdminMenu_ReplicateEverything, OnReplicateEverything, flag2, flag2 ? MyCommonTexts.ScreenDebugAdminMenu_ReplicateEverything_Tooltip : MySpaceTexts.ScreenDebugAdminMenu_ReplicateEverythingServer_Tooltip).PositionX += 0.003f;
				myGuiControlButton9.PositionY += 0.0435f;
				OnOrderChanged(m_order);
				break;
			}
			case MyPageEnum.TrashRemoval:
			{
				m_currentPosition.Y += 0.016f;
				bool flag = false;
				if (m_trashRemovalCombo == null)
				{
					m_trashRemovalCombo = new MyGuiControlCombobox
					{
						Position = m_currentPosition,
						OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
					};
					m_trashRemovalCombo.AddItem(0L, MyCommonTexts.ScreenDebugAdminMenu_GeneralTabButton);
					m_trashRemovalCombo.AddItem(1L, MyCommonTexts.ScreenDebugAdminMenu_VoxelTabButton);
					m_trashRemovalCombo.AddItem(2L, MyCommonTexts.ScreenDebugAdminMenu_OtherTabButton);
					m_trashRemovalCombo.ItemSelected += OnTrashRemovalItemSelected;
					flag = true;
				}
				Controls.Add(m_trashRemovalCombo);
				m_currentPosition.Y += m_trashRemovalCombo.Size.Y + 0.016f;
				m_tabs.Add(MyTabControlEnum.General, MyAdminMenuTabFactory.CreateTab(this, MyTabControlEnum.General));
				m_tabs.Add(MyTabControlEnum.Voxel, MyAdminMenuTabFactory.CreateTab(this, MyTabControlEnum.Voxel));
				m_tabs.Add(MyTabControlEnum.Other, MyAdminMenuTabFactory.CreateTab(this, MyTabControlEnum.Other));
				m_trashRemovalContentPanel = new MyGuiControlStackPanel
				{
					Position = m_currentPosition,
					Orientation = MyGuiOrientation.Vertical,
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
				};
				MyGuiControlStackPanel myGuiControlStackPanel = new MyGuiControlStackPanel
				{
					Position = Vector2.Zero,
					Orientation = MyGuiOrientation.Horizontal,
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
				};
				m_currentPosition.Y += 0.06f;
				MyGuiControlButton myGuiControlButton5 = CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_SubmitChangesButton, OnSubmitButtonClicked, enabled: true, MyCommonTexts.ScreenDebugAdminMenu_SubmitChangesButtonTooltip, increaseSpacing: true, addToControls: false);
				myGuiControlButton5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				myGuiControlButton5.PositionX = -0.088f;
				myGuiControlButton5.IsAutoScaleEnabled = true;
				myGuiControlButton5.IsAutoEllipsisEnabled = true;
				myGuiControlStackPanel.Add(myGuiControlButton5);
				MyGuiControlButton myGuiControlButton6 = CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_CancleChangesButton, OnCancelButtonClicked, enabled: true, null, increaseSpacing: true, addToControls: false);
				myGuiControlButton6.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				myGuiControlButton6.PositionX = 0.055f;
				myGuiControlButton6.PositionY -= 0.0435f;
				myGuiControlButton6.Margin = new Thickness(0.005f, 0f, 0f, 0f);
				myGuiControlButton6.IsAutoScaleEnabled = true;
				myGuiControlButton6.IsAutoEllipsisEnabled = true;
				myGuiControlStackPanel.Add(myGuiControlButton6);
				myGuiControlStackPanel.UpdateArrange();
				myGuiControlStackPanel.UpdateMeasure();
				m_trashRemovalContentPanel.Add(myGuiControlStackPanel);
				m_trashRemovalContentPanel.UpdateArrange();
				m_trashRemovalContentPanel.UpdateMeasure();
				Controls.Add(myGuiControlStackPanel);
				Controls.Add(myGuiControlButton5);
				Controls.Add(myGuiControlButton6);
				Controls.Add(m_trashRemovalContentPanel);
				if (flag)
				{
					m_trashRemovalCombo.SelectItemByKey(0L);
				}
				else
				{
					OnTrashRemovalItemSelected();
				}
				break;
			}
			case MyPageEnum.AdminTools:
			{
				m_currentPosition.Y += 0.03f;
				MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EnableAdminMode),
					IsAutoScaleEnabled = true,
					IsAutoEllipsisEnabled = true
				};
				myGuiControlLabel4.SetMaxSize(new Vector2(0.25f, float.PositiveInfinity));
				m_creativeCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_creativeCheckbox.IsCheckedChanged = OnEnableAdminModeChanged;
				m_creativeCheckbox.SetToolTip(MyCommonTexts.ScreenDebugAdminMenu_EnableAdminMode_Tooltip);
				m_creativeCheckbox.IsChecked = MySession.Static.CreativeToolsEnabled(Sync.MyId);
				m_creativeCheckbox.Enabled = MySession.Static.HasCreativeRights;
				Controls.Add(m_creativeCheckbox);
				Controls.Add(myGuiControlLabel4);
				m_currentPosition.Y += 0.045f;
				MyGuiControlLabel control12 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Invulnerable)
				};
				m_invulnerableCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_invulnerableCheckbox.IsCheckedChanged = OnInvulnerableChanged;
				m_invulnerableCheckbox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_InvulnerableToolTip);
				m_invulnerableCheckbox.IsChecked = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.Invulnerable);
				m_invulnerableCheckbox.Enabled = MySession.Static.IsUserAdmin(Sync.MyId);
				Controls.Add(m_invulnerableCheckbox);
				Controls.Add(control12);
				m_currentPosition.Y += 0.045f;
				MyGuiControlLabel control13 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Untargetable)
				};
				m_untargetableCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_untargetableCheckbox.IsCheckedChanged = OnUntargetableChanged;
				m_untargetableCheckbox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_UntargetableToolTip);
				m_untargetableCheckbox.IsChecked = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.Untargetable);
				m_untargetableCheckbox.Enabled = MySession.Static.IsUserAdmin(Sync.MyId);
				Controls.Add(m_untargetableCheckbox);
				Controls.Add(control13);
				m_currentPosition.Y += 0.045f;
				MyGuiControlLabel control14 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_ShowPlayers)
				};
				m_showPlayersCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_showPlayersCheckbox.IsCheckedChanged = OnShowPlayersChanged;
				m_showPlayersCheckbox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_ShowPlayersToolTip);
				m_showPlayersCheckbox.IsChecked = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.ShowPlayers);
				m_showPlayersCheckbox.Enabled = MySession.Static.IsUserModerator(Sync.MyId);
				Controls.Add(m_showPlayersCheckbox);
				Controls.Add(control14);
				m_currentPosition.Y += 0.045f;
				MyGuiControlLabel control15 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_UseTerminals)
				};
				m_canUseTerminals = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_canUseTerminals.IsCheckedChanged = OnUseTerminalsChanged;
				m_canUseTerminals.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_UseTerminalsToolTip);
				m_canUseTerminals.IsChecked = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals);
				m_canUseTerminals.Enabled = MySession.Static.IsUserAdmin(Sync.MyId);
				Controls.Add(m_canUseTerminals);
				Controls.Add(control15);
				if (MyPlatformGameSettings.IsIgnorePcuAllowed)
				{
					m_currentPosition.Y += 0.045f;
					MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel
					{
						Position = m_currentPosition + new Vector2(0.001f, 0f),
						OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
						Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_KeepOriginalOwnershipOnPaste),
						IsAutoEllipsisEnabled = true,
						IsAutoScaleEnabled = true
					};
					myGuiControlLabel5.SetMaxSize(new Vector2(0.25f, float.PositiveInfinity));
					m_keepOriginalOwnershipOnPasteCheckBox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
					m_keepOriginalOwnershipOnPasteCheckBox.IsCheckedChanged = OnKeepOwnershipChanged;
					m_keepOriginalOwnershipOnPasteCheckBox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_KeepOriginalOwnershipOnPasteTip);
					m_keepOriginalOwnershipOnPasteCheckBox.IsChecked = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.KeepOriginalOwnershipOnPaste);
					m_keepOriginalOwnershipOnPasteCheckBox.Enabled = MySession.Static.IsUserSpaceMaster(Sync.MyId);
					Controls.Add(m_keepOriginalOwnershipOnPasteCheckBox);
					Controls.Add(myGuiControlLabel5);
				}
				m_currentPosition.Y += 0.045f;
				MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_IgnoreSafeZones),
					IsAutoEllipsisEnabled = true
				};
				myGuiControlLabel6.SetMaxSize(new Vector2(0.25f, float.PositiveInfinity));
				m_ignoreSafeZonesCheckBox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_ignoreSafeZonesCheckBox.IsCheckedChanged = OnIgnoreSafeZonesChanged;
				m_ignoreSafeZonesCheckBox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_IgnoreSafeZonesTip);
				m_ignoreSafeZonesCheckBox.IsChecked = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.IgnoreSafeZones);
				m_ignoreSafeZonesCheckBox.Enabled = MySession.Static.IsUserAdmin(Sync.MyId);
				Controls.Add(m_ignoreSafeZonesCheckBox);
				Controls.Add(myGuiControlLabel6);
				if (MyPlatformGameSettings.IsIgnorePcuAllowed)
				{
					m_currentPosition.Y += 0.045f;
					MyGuiControlLabel control16 = new MyGuiControlLabel
					{
						Position = m_currentPosition + new Vector2(0.001f, 0f),
						OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
						Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Pcu)
					};
					m_ignorePcuCheckBox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
					m_ignorePcuCheckBox.IsCheckedChanged = OnIgnorePcuChanged;
					m_ignorePcuCheckBox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_IgnorePcuTip);
					m_ignorePcuCheckBox.IsChecked = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.IgnorePcu);
					m_ignorePcuCheckBox.Enabled = MySession.Static.IsUserAdmin(Sync.MyId) && (MySession.Static.IsRunningExperimental || MySession.Static.OnlineMode != MyOnlineModeEnum.OFFLINE);
					Controls.Add(m_ignorePcuCheckBox);
					Controls.Add(control16);
				}
				if (MySession.Static.IsUserAdmin(Sync.MyId))
				{
					m_currentPosition.Y += 0.045f;
					MyGuiControlLabel control17 = new MyGuiControlLabel
					{
						Position = m_currentPosition + new Vector2(0.001f, 0f),
						OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
						Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_TimeOfDay)
					};
					Controls.Add(control17);
					m_timeDeltaValue = new MyGuiControlLabel
					{
						Position = m_currentPosition + new Vector2(0.285f, 0f),
						OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
						Text = "0.00"
					};
					Controls.Add(m_timeDeltaValue);
					m_currentPosition.Y += 0.035f;
					m_timeDelta = new MyGuiControlSlider(m_currentPosition + new Vector2(0.001f, 0f), 0f, (MySession.Static == null) ? 1f : MySession.Static.Settings.SunRotationIntervalMinutes);
					m_timeDelta.Size = new Vector2(0.285f, 1f);
					m_timeDelta.Value = MyTimeOfDayHelper.TimeOfDay;
					m_timeDelta.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
					MyGuiControlSlider timeDelta = m_timeDelta;
					timeDelta.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(timeDelta.ValueChanged, new Action<MyGuiControlSlider>(TimeDeltaChanged));
					m_timeDeltaValue.Text = $"{m_timeDelta.Value:0.00}";
					Controls.Add(m_timeDelta);
					m_currentPosition.Y += 0.07f;
				}
				break;
			}
			case MyPageEnum.EntityList:
			{
				m_currentPosition.Y += 0.095f;
				MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel
				{
					Position = new Vector2(-0.16f, -0.334f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MyCommonTexts.Select) + ":",
					IsAutoEllipsisEnabled = true,
					IsAutoScaleEnabled = true
				};
				myGuiControlLabel.SetMaxWidth(0.064f);
				Controls.Add(myGuiControlLabel);
				m_currentPosition.Y -= 0.065f;
				m_entityTypeCombo = AddCombo(m_selectedType, ValueChanged, enabled: true, 10, null, m_labelColor);
				m_entityTypeCombo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				m_entityTypeCombo.PositionX = 0.122f;
				m_entityTypeCombo.Size = new Vector2(0.21f, 1f);
				MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel
				{
					Position = new Vector2(-0.16f, -0.284f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SortBy) + ":",
					IsAutoScaleEnabled = true,
					IsAutoEllipsisEnabled = true
				};
				myGuiControlLabel2.SetMaxWidth(0.064f);
				Controls.Add(myGuiControlLabel2);
				m_entitySortCombo = AddCombo(m_selectedSort, ValueChanged, enabled: true, 10, null, m_labelColor, isAutoscaleEnabled: true, isAutoEllipsisEnabled: true);
				m_entitySortCombo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				m_entitySortCombo.PositionX = 0.122f;
				m_entitySortCombo.Size = new Vector2(0.21f, 1f);
				MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
				myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.231f), m_size.Value.X * 0.73f);
				Controls.Add(myGuiControlSeparatorList2);
				MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel
				{
					Position = new Vector2(-0.153f, -0.205f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.SafeZone_ListOfEntities)
				};
				MyGuiControlPanel control11 = new MyGuiControlPanel(new Vector2(myGuiControlLabel3.PositionX - 0.0085f, myGuiControlLabel3.Position.Y - 0.005f), new Vector2(0.2865f, 0.035f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
				{
					BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
				};
				Controls.Add(control11);
				Controls.Add(myGuiControlLabel3);
				m_currentPosition.Y += 0.065f;
				m_entityListbox = new MyGuiControlListbox(Vector2.Zero, MyGuiControlListboxStyleEnum.Blueprints);
				m_entityListbox.Size = new Vector2(num, 0f);
				m_entityListbox.Enabled = true;
				m_entityListbox.VisibleRowsCount = 12;
				m_entityListbox.Position = m_entityListbox.Size / 2f + m_currentPosition;
				m_entityListbox.ItemClicked += EntityListItemClicked;
				m_entityListbox.MultiSelect = true;
				MyGuiControlSeparatorList myGuiControlSeparatorList3 = new MyGuiControlSeparatorList();
				myGuiControlSeparatorList3.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, -0.271f), m_size.Value.X * 0.73f);
				Controls.Add(myGuiControlSeparatorList3);
				m_currentPosition = m_entityListbox.GetPositionAbsoluteBottomLeft();
				m_currentPosition.Y += 0.045f;
				MyGuiControlButton myGuiControlButton = CreateDebugButton(0.14f, MyCommonTexts.SpectatorControls_None, OnPlayerControl, enabled: true, MySpaceTexts.SpectatorControls_None_Desc, increaseSpacing: true, addToControls: true, isAutoScaleEnabled: true);
				myGuiControlButton.PositionX = -0.088f;
				MyGuiControlButton myGuiControlButton2 = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_TeleportHere, OnTeleportButton, MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.Parent == null, MySpaceTexts.ScreenDebugAdminMenu_TeleportHereToolTip, increaseSpacing: true, addToControls: true, isAutoScaleEnabled: true);
				myGuiControlButton2.PositionX = 0.055f;
				myGuiControlButton2.PositionY = myGuiControlButton.PositionY;
				float y = m_currentPosition.Y;
				m_currentPosition.Y = y;
				m_stopItemButton = CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Stop, delegate
				{
					OnEntityListActionClicked(MyEntityList.EntityListAction.Stop);
				});
				m_stopItemButton.PositionX = -0.088f;
				m_stopItemButton.PositionY -= 0.0435f;
				m_currentPosition.Y = y;
				m_depowerItemButton = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_Depower, delegate
				{
					OnEntityListActionClicked(MyEntityList.EntityListAction.Depower);
				});
				m_depowerItemButton.PositionX = 0.055f;
				m_depowerItemButton.PositionY = m_stopItemButton.PositionY;
				m_removeItemButton = CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Remove, delegate
				{
					OnEntityListActionClicked(MyEntityList.EntityListAction.Remove);
				});
				m_removeItemButton.PositionX -= 0.068f;
				m_removeItemButton.PositionY -= 0.0435f;
				MyGuiControlButton myGuiControlButton3 = CreateDebugButton(0.14f, MySpaceTexts.buttonRefresh, OnRefreshButton, enabled: true, MySpaceTexts.buttonRefresh);
				myGuiControlButton3.PositionX += 0.075f;
				myGuiControlButton3.PositionY = m_removeItemButton.PositionY;
				MyGuiControlButton myGuiControlButton4 = CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugAdminMenu_RemoveOwner, OnRemoveOwnerButton, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_RemoveOwnerToolTip, increaseSpacing: true, addToControls: true, isAutoScaleEnabled: true);
				myGuiControlButton4.PositionX += 0.003f;
				myGuiControlButton4.PositionY -= 0.087f;
				Controls.Add(m_entityListbox);
				ValueChanged((MyEntityList.MyEntityTypeEnum)m_entityTypeCombo.GetSelectedKey());
				break;
			}
			case MyPageEnum.SafeZones:
				RecreateSafeZonesControls(ref controlPadding, separatorSize, num);
				break;
			case MyPageEnum.GlobalSafeZone:
				RecreateGlobalSafeZoneControls(ref controlPadding, separatorSize, num);
				break;
			case MyPageEnum.ReplayTool:
				RecreateReplayToolControls(ref controlPadding, separatorSize, num);
				break;
			case MyPageEnum.Economy:
			{
				m_currentPosition.X += 0.003f;
				m_currentPosition.Y += 0.03f;
				MyGuiControlLabel control = new MyGuiControlLabel
				{
					Position = m_currentPosition,
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_Label)
				};
				Controls.Add(control);
				m_currentPosition.Y += 0.05f;
				ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
				m_playerCount = onlinePlayers.Count;
				m_factionCount = Enumerable.Count<KeyValuePair<long, MyFaction>>((IEnumerable<KeyValuePair<long, MyFaction>>)MySession.Static.Factions);
				m_onlinePlayerCombo = new MyGuiControlCombobox();
				m_onlinePlayerCombo.SetTooltip(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_Player_Tooltip));
				List<MyIdNamePair> list = new List<MyIdNamePair>();
				List<MyIdNamePair> list2 = new List<MyIdNamePair>();
				MyIdNamePairComparer comparer = new MyIdNamePairComparer();
				MyIdNamePair item;
				foreach (MyPlayer item2 in onlinePlayers)
				{
					item = new MyIdNamePair
					{
						Id = item2.Identity.IdentityId,
						Name = item2.DisplayName
					};
					list.Add(item);
				}
				foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
				{
					item = new MyIdNamePair
					{
						Id = faction.Key,
						Name = faction.Value.Tag
					};
					list2.Add(item);
				}
				list.Sort(comparer);
				list2.Sort(comparer);
				int num3 = 0;
				foreach (MyIdNamePair item3 in list)
				{
					m_onlinePlayerCombo.AddItem(item3.Id, item3.Name, num3);
					num3++;
				}
				foreach (MyIdNamePair item4 in list2)
				{
					m_onlinePlayerCombo.AddItem(item4.Id, item4.Name, num3);
					num3++;
				}
				m_onlinePlayerCombo.ItemSelected += OnlinePlayerCombo_ItemSelected;
				m_onlinePlayerCombo.SelectItemByIndex(-1);
				m_onlinePlayerCombo.Position = m_currentPosition + new Vector2(0.14f, 0f);
				Controls.Add(m_onlinePlayerCombo);
				m_currentPosition.Y += 0.04f;
				string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
				string texture = ((icons != null && icons.Length != 0) ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty);
				Vector2 screenSizeFromNormalizedSize = MyGuiManager.GetScreenSizeFromNormalizedSize(new Vector2(1f));
				float num4 = screenSizeFromNormalizedSize.X / screenSizeFromNormalizedSize.Y;
				Vector2 vector = new Vector2(0.28f, 0.0033f);
				Vector2 size = new Vector2(0.018f, num4 * 0.018f);
				float num5 = size.X + 0.01f;
				MyGuiControlLabel control2 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_CurrentBalance,
					IsAutoEllipsisEnabled = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(control2);
				m_labelCurrentBalanceValue = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.28f - num5, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
					Text = MyBankingSystem.GetFormatedValue(0L),
					IsAutoEllipsisEnabled = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(m_labelCurrentBalanceValue);
				MyGuiControlImage myGuiControlImage = new MyGuiControlImage
				{
					Position = m_currentPosition + vector,
					Size = size,
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER
				};
				myGuiControlImage.SetTexture(texture);
				Controls.Add(myGuiControlImage);
				m_currentPosition.Y += 0.04f;
				MyGuiControlLabel control3 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_ChangeBalance,
					IsAutoEllipsisEnabled = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(control3);
				m_addCurrencyTextbox = new MyGuiControlTextbox(null, "0", 512, null, 0.8f, MyGuiControlTextboxType.DigitsOnly);
				m_addCurrencyTextbox.Position = m_currentPosition + new Vector2(0.218f - num5 / 2f, 0f);
				m_addCurrencyTextbox.Size = new Vector2(m_addCurrencyTextbox.Size.X * 0.4f - num5, m_addCurrencyTextbox.Size.Y);
				m_addCurrencyTextbox.TextChanged += AddCurrency_TextChanged;
				Controls.Add(m_addCurrencyTextbox);
				MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage
				{
					Position = m_currentPosition + vector,
					Size = size,
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER
				};
				myGuiControlImage2.SetTexture(texture);
				Controls.Add(myGuiControlImage2);
				m_currentPosition.Y += 0.04f;
				MyGuiControlLabel control4 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_FinalBalance,
					IsAutoEllipsisEnabled = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(control4);
				m_labelFinalBalanceValue = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.28f - num5, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
					Text = MyBankingSystem.GetFormatedValue(0L),
					IsAutoEllipsisEnabled = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(m_labelFinalBalanceValue);
				MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage
				{
					Position = m_currentPosition + vector,
					Size = size,
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER
				};
				myGuiControlImage3.SetTexture(texture);
				Controls.Add(myGuiControlImage3);
				m_currentPosition.Y += 0.06f;
				m_addCurrencyConfirmButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_CoonfirmButton), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, AddCurrency_ButtonClicked);
				m_addCurrencyConfirmButton.Position = m_currentPosition + new Vector2(0.14f, 0f);
				Controls.Add(m_addCurrencyConfirmButton);
				m_currentBalance = 0L;
				m_finalBalance = 0L;
				m_balanceDifference = 0L;
				m_currentPosition.Y += 0.1f;
				MyGuiControlLabel control5 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_PlayerLabel)
				};
				Controls.Add(control5);
				m_currentPosition.Y += 0.05f;
				m_playerReputationCombo = new MyGuiControlCombobox();
				m_playerReputationCombo.SetTooltip(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_Player_Tooltip));
				List<MyIdNamePair> list3 = new List<MyIdNamePair>();
				foreach (MyPlayer item5 in onlinePlayers)
				{
					item = new MyIdNamePair
					{
						Id = item5.Identity.IdentityId,
						Name = item5.DisplayName
					};
					list3.Add(item);
				}
				list3.Sort(comparer);
				num3 = 0;
				foreach (MyIdNamePair item6 in list3)
				{
					m_playerReputationCombo.AddItem(item6.Id, item6.Name, num3);
					num3++;
				}
				m_playerReputationCombo.ItemSelected += playerReputationCombo_ItemSelected;
				m_playerReputationCombo.SelectItemByIndex(-1);
				m_playerReputationCombo.Position = m_currentPosition + new Vector2(0.14f, 0f);
				Controls.Add(m_playerReputationCombo);
				m_currentPosition.Y += 0.03f;
				MyGuiControlLabel control6 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_FactionLabel)
				};
				Controls.Add(control6);
				m_currentPosition.Y += 0.05f;
				m_factionReputationCombo = new MyGuiControlCombobox();
				m_factionReputationCombo.SetTooltip(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_Faction_Tooltip));
				List<MyIdNamePair> list4 = new List<MyIdNamePair>();
				foreach (KeyValuePair<long, MyFaction> faction2 in MySession.Static.Factions)
				{
					item = new MyIdNamePair
					{
						Id = faction2.Key,
						Name = faction2.Value.Tag
					};
					list4.Add(item);
				}
				list4.Sort(comparer);
				num3 = 0;
				foreach (MyIdNamePair item7 in list4)
				{
					m_factionReputationCombo.AddItem(item7.Id, item7.Name, num3);
					num3++;
				}
				m_factionReputationCombo.ItemSelected += factionReputationCombo_ItemSelected;
				m_factionReputationCombo.SelectItemByIndex(-1);
				m_factionReputationCombo.Position = m_currentPosition + new Vector2(0.14f, 0f);
				Controls.Add(m_factionReputationCombo);
				m_currentPosition.Y += 0.04f;
				MyGuiControlLabel control7 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddReputation_CurrentReputation,
					IsAutoEllipsisEnabled = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(control7);
				m_labelCurrentReputationValue = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.28f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
					Text = 0.ToString(),
					IsAutoEllipsisEnabled = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(m_labelCurrentReputationValue);
				m_currentPosition.Y += 0.04f;
				MyGuiControlLabel control8 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddReputation_ChangeReputation,
					IsAutoEllipsisEnabled = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(control8);
				m_addReputationTextbox = new MyGuiControlTextbox(null, "0", 512, null, 0.8f, MyGuiControlTextboxType.DigitsOnly);
				m_addReputationTextbox.Position = m_currentPosition + new Vector2(0.218f, 0f);
				m_addReputationTextbox.Size = new Vector2(m_addReputationTextbox.Size.X * 0.4f, m_addCurrencyTextbox.Size.Y);
				m_addReputationTextbox.TextChanged += AddReputation_TextChanged;
				Controls.Add(m_addReputationTextbox);
				m_currentPosition.Y += 0.04f;
				MyGuiControlLabel control9 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddReputation_FinalReputation,
					IsAutoEllipsisEnabled = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(control9);
				m_labelFinalReputationValue = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.28f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
					Text = 0.ToString(),
					IsAutoEllipsisEnabled = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(m_labelFinalReputationValue);
				m_currentPosition.Y += 0.04f;
				MyGuiControlLabel control10 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddReputation_ReputationPropagate,
					IsAutoEllipsisEnabled = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(control10);
				m_addReputationPropagate = new MyGuiControlCheckbox
				{
					Position = m_currentPosition + new Vector2(0.273f, 0f),
					IsChecked = false
				};
				m_addReputationPropagate.SetToolTip(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_ReputationPropagate_Tooltip));
				Controls.Add(m_addReputationPropagate);
				m_currentPosition.Y += 0.06f;
				m_addReputationConfirmButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_ConfirmButton), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, AddReputation_ButtonClicked);
				m_addReputationConfirmButton.Position = m_currentPosition + new Vector2(0.14f, 0f);
				Controls.Add(m_addReputationConfirmButton);
				m_currentReputation = 0;
				m_finalReputation = 0;
				m_reputationDifference = 0;
				break;
			}
			case MyPageEnum.Weather:
				RecreateWeatherControls(constructor: false);
				break;
			case MyPageEnum.Spectator:
				RecreateSpectatorControls(constructor: false);
				break;
			case MyPageEnum.Match:
				RecreateMatchControls(constructor: false);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void factionReputationCombo_ItemSelected()
		{
			m_factionReputationCombo_SelectedPlayerIdentityId = m_factionReputationCombo.GetSelectedKey();
			UpdateReputation();
		}

		private void playerReputationCombo_ItemSelected()
		{
			m_playerReputationCombo_SelectedPlayerIdentityId = m_playerReputationCombo.GetSelectedKey();
			UpdateReputation();
		}

		private void UpdateReputation()
		{
			if (m_factionReputationCombo_SelectedPlayerIdentityId != 0L && m_playerReputationCombo_SelectedPlayerIdentityId != 0L)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestReputation, m_playerReputationCombo_SelectedPlayerIdentityId, m_factionReputationCombo_SelectedPlayerIdentityId);
			}
		}

<<<<<<< HEAD
		[Event(null, 1221)]
=======
		[Event(null, 1226)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void RequestReputation(long playerIdentityId, long factionId)
		{
			ulong value = MyEventContext.Current.Sender.Value;
			if (MySession.Static.IsUserAdmin(value))
			{
				Tuple<MyRelationsBetweenFactions, int> relationBetweenPlayerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(playerIdentityId, factionId);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestReputationCallback, playerIdentityId, factionId, relationBetweenPlayerAndFaction.Item2, MyEventContext.Current.Sender);
			}
		}

<<<<<<< HEAD
		[Event(null, 1233)]
=======
		[Event(null, 1238)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void RequestReputationCallback(long playerIdentityId, long factionId, int reputation)
		{
			MyScreenManager.GetFirstScreenOfType<MyGuiScreenAdminMenu>()?.RequestReputationCallback_Internal(playerIdentityId, factionId, reputation);
		}

		protected void RequestReputationCallback_Internal(long playerIdentityId, long factionId, int reputation)
		{
			if (m_playerReputationCombo_SelectedPlayerIdentityId == playerIdentityId && m_factionReputationCombo_SelectedPlayerIdentityId == factionId)
			{
				m_currentReputation = ClampReputation(reputation);
				m_finalReputation = ClampReputation(reputation + m_reputationDifference);
				UpdateReputationTexts();
			}
		}

		protected void UpdateReputationTexts()
		{
			m_labelCurrentReputationValue.Text = m_currentReputation.ToString();
			m_labelFinalReputationValue.Text = m_finalReputation.ToString();
		}

		private void AddReputation_ButtonClicked(MyGuiControlButton obj)
		{
			bool isChecked = m_addReputationPropagate.IsChecked;
			if (m_playerReputationCombo_SelectedPlayerIdentityId != 0L || m_factionReputationCombo_SelectedPlayerIdentityId != 0L)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestChangeReputation, m_playerReputationCombo_SelectedPlayerIdentityId, m_factionReputationCombo_SelectedPlayerIdentityId, m_reputationDifference, isChecked);
			}
		}

<<<<<<< HEAD
		[Event(null, 1261)]
=======
		[Event(null, 1266)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void RequestChangeReputation(long identityId, long factionId, int reputationChange, bool shouldPropagate)
		{
			ulong value = MyEventContext.Current.Sender.Value;
			if (MySession.Static.IsUserAdmin(value))
			{
				MySession.Static.Factions.AddFactionPlayerReputation(identityId, factionId, reputationChange, shouldPropagate);
				Tuple<MyRelationsBetweenFactions, int> relationBetweenPlayerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(identityId, factionId);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestReputationCallback, identityId, factionId, relationBetweenPlayerAndFaction.Item2, MyEventContext.Current.Sender);
			}
		}

		private void AddReputation_TextChanged(MyGuiControlTextbox obj)
		{
			if (int.TryParse(obj.Text, out m_reputationDifference))
			{
				m_finalReputation = ClampReputation(m_currentReputation + m_reputationDifference);
			}
			else
			{
				m_finalReputation = ClampReputation(m_currentReputation);
			}
			UpdateReputationTexts();
		}

		private int ClampReputation(int reputation)
		{
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			int hostileMax = component.GetHostileMax();
			if (reputation < hostileMax)
			{
				return hostileMax;
			}
			int friendlyMax = component.GetFriendlyMax();
			if (reputation > friendlyMax)
			{
				return friendlyMax;
			}
			return reputation;
		}

		private void OnTrashRemovalItemSelected()
		{
			MyTabControlEnum key = (MyTabControlEnum)m_trashRemovalCombo.GetSelectedKey();
			if (m_tabs.TryGetValue(key, out var _))
			{
				MyGuiControlParent control = m_tabs[key].Control;
				control.UpdateArrange();
				control.UpdateMeasure();
				if (m_trashRemovalContentPanel.GetControlCount() > 1)
				{
					MyGuiControlBase at = m_trashRemovalContentPanel.GetAt(0);
					at.Visible = false;
					m_trashRemovalContentPanel.Remove(at);
				}
				m_trashRemovalContentPanel.AddAt(0, control);
				control.Visible = true;
				m_trashRemovalContentPanel.UpdateArrange();
				m_trashRemovalContentPanel.UpdateMeasure();
			}
		}

		private void AddCurrency_TextChanged(MyGuiControlTextbox obj)
		{
			if (long.TryParse(obj.Text, out m_balanceDifference))
			{
				m_finalBalance = m_currentBalance + m_balanceDifference;
			}
			else
			{
				m_finalBalance = m_currentBalance;
			}
			UpdateBalanceTexts();
		}

		protected void UpdateBalanceTexts()
		{
			m_labelCurrentBalanceValue.Text = MyBankingSystem.GetFormatedValue(m_currentBalance);
			m_labelFinalBalanceValue.Text = MyBankingSystem.GetFormatedValue((m_finalBalance > 0) ? m_finalBalance : 0);
		}

		private void AddCurrency_ButtonClicked(MyGuiControlButton obj)
		{
			if (m_onlinePlayerCombo_SelectedPlayerIdentityId != 0L)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestChange, m_onlinePlayerCombo_SelectedPlayerIdentityId, m_balanceDifference);
			}
		}

		private void OnlinePlayerCombo_ItemSelected()
		{
			int selectedIndex = m_onlinePlayerCombo.GetSelectedIndex();
			m_onlinePlayerCombo_SelectedPlayerIdentityId = m_onlinePlayerCombo.GetSelectedKey();
			if (selectedIndex < m_playerCount)
			{
				m_isPlayerSelected = true;
				m_isFactionSelected = false;
			}
			else if (selectedIndex - m_playerCount < m_factionCount)
			{
				m_isPlayerSelected = false;
				m_isFactionSelected = true;
			}
			if (m_onlinePlayerCombo_SelectedPlayerIdentityId != 0L)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestBalance, m_onlinePlayerCombo_SelectedPlayerIdentityId);
			}
		}

<<<<<<< HEAD
		[Event(null, 1365)]
=======
		[Event(null, 1370)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void RequestBalance(long accountOwner)
		{
			ulong value = MyEventContext.Current.Sender.Value;
			if (MySession.Static.IsUserAdmin(value) && MyBankingSystem.Static.TryGetAccountInfo(accountOwner, out var account))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestBalanceCallback, accountOwner, account.Balance, MyEventContext.Current.Sender);
			}
		}

<<<<<<< HEAD
		[Event(null, 1378)]
=======
		[Event(null, 1383)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void RequestBalanceCallback(long accountOwner, long balance)
		{
			MyScreenManager.GetFirstScreenOfType<MyGuiScreenAdminMenu>()?.RequestBalanceCallback_Internal(accountOwner, balance);
		}

		protected void RequestBalanceCallback_Internal(long accountOwner, long balance)
		{
			if (m_onlinePlayerCombo_SelectedPlayerIdentityId == accountOwner)
			{
				m_currentBalance = balance;
				m_finalBalance = balance + m_balanceDifference;
				UpdateBalanceTexts();
			}
		}

<<<<<<< HEAD
		[Event(null, 1396)]
=======
		[Event(null, 1401)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void RequestChange(long accountOwner, long balanceChange)
		{
			ulong value = MyEventContext.Current.Sender.Value;
			if (MySession.Static.IsUserAdmin(value) && MyBankingSystem.ChangeBalance(accountOwner, balanceChange) && MyBankingSystem.Static.TryGetAccountInfo(accountOwner, out var account))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestBalanceCallback, accountOwner, account.Balance, MyEventContext.Current.Sender);
			}
		}

		private void CircleGps(bool reset, bool forward)
		{
			m_onlyLargeGridsCheckbox.Enabled = false;
			m_onlySmallGridsCheckbox.Enabled = false;
			m_depowerItemButton.Enabled = false;
			m_removeItemButton.Enabled = false;
			m_stopItemButton.Enabled = false;
			if (MySession.Static == null || MySession.Static.Gpss == null || MySession.Static.LocalHumanPlayer == null)
			{
				return;
			}
			if (forward)
			{
				m_currentGpsIndex--;
			}
			else
			{
				m_currentGpsIndex++;
			}
			m_gpsList.Clear();
			MySession.Static.Gpss.GetGpsList(MySession.Static.LocalPlayerId, m_gpsList);
			if (m_gpsList.Count == 0)
			{
				m_currentGpsIndex = 0;
				return;
			}
			if (m_currentGpsIndex < 0)
			{
				m_currentGpsIndex = m_gpsList.Count - 1;
			}
			if (m_gpsList.Count <= m_currentGpsIndex || reset)
			{
				m_currentGpsIndex = 0;
			}
			IMyGps myGps = m_gpsList[m_currentGpsIndex];
			Vector3D coords = myGps.Coords;
			m_labelEntityName.TextToDraw.Clear();
			m_labelEntityName.TextToDraw.Append(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EntityName));
			m_labelEntityName.TextToDraw.Append(string.IsNullOrEmpty(myGps.Name) ? "-" : myGps.Name);
			m_labelCurrentIndex.TextToDraw.Clear().AppendFormat(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_CurrentValue), m_currentGpsIndex);
			MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator);
			Vector3D? vector3D = MyEntities.FindFreePlace(coords + Vector3D.One, 2f, 30);
			MySpectatorCameraController.Static.Position = (vector3D.HasValue ? vector3D.Value : (coords + Vector3D.One));
			MySpectatorCameraController.Static.Target = coords;
		}

		internal static void RecalcTrash()
		{
			if (!Sync.IsServer)
			{
				AdminSettings adminSettings = default(AdminSettings);
				adminSettings.Flags = MySession.Static.Settings.TrashFlags;
				adminSettings.Enable = MySession.Static.Settings.TrashRemovalEnabled;
				adminSettings.BlockCount = MySession.Static.Settings.BlockCountThreshold;
				adminSettings.PlayerDistance = MySession.Static.Settings.PlayerDistanceThreshold;
				adminSettings.GridCount = MySession.Static.Settings.OptimalGridCount;
				adminSettings.PlayerInactivity = MySession.Static.Settings.PlayerInactivityThreshold;
				adminSettings.CharacterRemovalThreshold = MySession.Static.Settings.PlayerCharacterRemovalThreshold;
				adminSettings.StopGridsPeriod = MySession.Static.Settings.StopGridsPeriodMin;
				adminSettings.RemoveOldIdentities = MySession.Static.Settings.RemoveOldIdentitiesH;
				adminSettings.VoxelDistanceFromPlayer = MySession.Static.Settings.VoxelPlayerDistanceThreshold;
				adminSettings.VoxelDistanceFromGrid = MySession.Static.Settings.VoxelGridDistanceThreshold;
				adminSettings.VoxelAge = MySession.Static.Settings.VoxelAgeThreshold;
				adminSettings.VoxelEnable = MySession.Static.Settings.VoxelTrashRemovalEnabled;
				AdminSettings arg = adminSettings;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UploadSettingsToServer, arg);
			}
		}

		private static bool TryAttachCamera(long entityId)
		{
			if (MyEntities.TryGetEntityById(entityId, out var entity))
			{
				BoundingSphereD worldVolume = entity.PositionComp.WorldVolume;
				MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator);
				MySpectatorCameraController.Static.Position = worldVolume.Center + Math.Max((float)worldVolume.Radius, 1f) * Vector3.One;
				MySpectatorCameraController.Static.Target = worldVolume.Center;
				MySessionComponentAnimationSystem.Static.EntitySelectedForDebug = entity;
				return true;
			}
			return false;
		}

		private void UpdateCyclingAndDepower()
		{
			bool enabled = (m_cyclingOptions.Enabled = m_order != 0 && m_order != MyEntityCyclingOrder.FloatingObjects && m_order != MyEntityCyclingOrder.Gps);
			if (m_depowerItemButton != null)
			{
				m_depowerItemButton.Enabled = enabled;
			}
		}

		private void UpdateSmallLargeGridSelection()
		{
			if (m_currentPage == MyPageEnum.CycleObjects)
			{
				bool enabled = m_order != 0 && m_order != MyEntityCyclingOrder.FloatingObjects && m_order != MyEntityCyclingOrder.Gps;
				m_removeItemButton.Enabled = true;
				m_onlySmallGridsCheckbox.Enabled = enabled;
				m_onlyLargeGridsCheckbox.Enabled = enabled;
			}
		}

		private static void UpdateRemoveAndDepowerButton(MyGuiScreenAdminMenu menu, long entityId, bool disableOverride = false)
		{
			if (menu == null || menu.m_removeItemButton == null)
			{
				return;
			}
			MyEntities.TryGetEntityById(entityId, out var entity);
			if (entity == null)
			{
				return;
			}
			bool flag = m_currentPage != MyPageEnum.CycleObjects || m_order != MyEntityCyclingOrder.Gps;
			menu.m_removeItemButton.Enabled = (flag && !menu.m_attachIsNpcStation) || disableOverride;
			if (menu.m_depowerItemButton != null)
			{
				menu.m_depowerItemButton.Enabled = (entity is MyCubeGrid && flag && !menu.m_attachIsNpcStation) || disableOverride;
			}
			if (menu.m_stopItemButton != null)
			{
				menu.m_stopItemButton.Enabled = (entity != null && !(entity is MyVoxelBase) && flag && !menu.m_attachIsNpcStation) || disableOverride;
			}
			if (m_currentPage == MyPageEnum.CycleObjects)
			{
				string text = "";
<<<<<<< HEAD
				text = ((entity is MyVoxelBase) ? (MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EntityName) + ((MyVoxelBase)entity).StorageName) : (MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EntityName) + entity.DisplayName));
=======
				text = ((entity is MyVoxelBase) ? (MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EntityName) + ((MyVoxelBase)entity).StorageName) : (MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EntityName) + ((entity == null) ? "-" : entity.DisplayName)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (menu.m_labelEntityName.Text != text)
				{
					menu.m_labelEntityName.Text = text;
					menu.m_labelEntityName.SetMaxWidth(0.27f);
					menu.m_labelEntityName.DoEllipsisAndScaleAdjust(RecalculateSize: true, 0.8f, resetEllipsis: true);
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Send SM settings back to requester
		/// </summary>
		[Event(null, 1607)]
=======
		[Event(null, 1612)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void RequestSettingFromServer_Implementation()
		{
			AdminSettings adminSettings = default(AdminSettings);
			adminSettings.Flags = MySession.Static.Settings.TrashFlags;
			adminSettings.Enable = MySession.Static.Settings.TrashRemovalEnabled;
			adminSettings.BlockCount = MySession.Static.Settings.BlockCountThreshold;
			adminSettings.PlayerDistance = MySession.Static.Settings.PlayerDistanceThreshold;
			adminSettings.GridCount = MySession.Static.Settings.OptimalGridCount;
			adminSettings.PlayerInactivity = MySession.Static.Settings.PlayerInactivityThreshold;
			adminSettings.CharacterRemovalThreshold = MySession.Static.Settings.PlayerCharacterRemovalThreshold;
			adminSettings.AdminSettingsFlags = MySession.Static.RemoteAdminSettings.GetValueOrDefault(MyEventContext.Current.Sender.Value, AdminSettingsEnum.None);
			adminSettings.StopGridsPeriod = MySession.Static.Settings.StopGridsPeriodMin;
			adminSettings.RemoveOldIdentities = MySession.Static.Settings.RemoveOldIdentitiesH;
			adminSettings.VoxelDistanceFromPlayer = MySession.Static.Settings.VoxelPlayerDistanceThreshold;
			adminSettings.VoxelDistanceFromGrid = MySession.Static.Settings.VoxelGridDistanceThreshold;
			adminSettings.VoxelAge = MySession.Static.Settings.VoxelAgeThreshold;
			adminSettings.VoxelEnable = MySession.Static.Settings.VoxelTrashRemovalEnabled;
			AdminSettings arg = adminSettings;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => DownloadSettingFromServer, arg, MyEventContext.Current.Sender);
		}

		private void ValueChanged(MyEntityList.MyEntitySortOrder selectedOrder)
		{
			if (m_selectedSort == selectedOrder)
			{
				m_invertOrder = !m_invertOrder;
			}
			else
			{
				m_invertOrder = false;
			}
			m_selectedSort = selectedOrder;
			List<MyEntityList.MyEntityListInfoItem> items = new List<MyEntityList.MyEntityListInfoItem>(((Collection<MyGuiControlListbox.Item>)(object)m_entityListbox.Items).Count);
			foreach (MyGuiControlListbox.Item item in m_entityListbox.Items)
			{
				items.Add((MyEntityList.MyEntityListInfoItem)item.UserData);
			}
			MyEntityList.SortEntityList(selectedOrder, ref items, m_invertOrder);
			((Collection<MyGuiControlListbox.Item>)(object)m_entityListbox.Items).Clear();
			MyEntityList.MyEntityTypeEnum myEntityTypeEnum = (MyEntityList.MyEntityTypeEnum)m_entityTypeCombo.GetSelectedKey();
			foreach (MyEntityList.MyEntityListInfoItem item2 in items)
			{
				StringBuilder formattedDisplayName = MyEntityList.GetFormattedDisplayName(selectedOrder, item2);
				m_entityListbox.Add(new MyGuiControlListbox.Item(formattedDisplayName, MyEntityList.GetDescriptionText(item2), null, item2));
			}
		}

		private void EntityListItemClicked(MyGuiControlListbox myGuiControlListbox)
		{
			if (myGuiControlListbox.SelectedItems.Count <= 0)
			{
				return;
			}
			MyEntityList.MyEntityListInfoItem myEntityListInfoItem = (MyEntityList.MyEntityListInfoItem)myGuiControlListbox.SelectedItems[myGuiControlListbox.SelectedItems.Count - 1].UserData;
			m_attachCamera = myEntityListInfoItem.EntityId;
			if (!TryAttachCamera(myEntityListInfoItem.EntityId))
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, null, myEntityListInfoItem.Position + Vector3.One * 50f);
			}
			if (m_attachCamera != 0L)
			{
				UpdateRemoveAndDepowerButton(this, m_attachCamera, disableOverride: true);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AskIsValidForEdit_Server, m_attachCamera);
			}
		}

<<<<<<< HEAD
		[Event(null, 1685)]
=======
		[Event(null, 1690)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void AskIsValidForEdit_Server(long entityId)
		{
			bool flag = true;
			if (MySession.Static != null && MySession.Static.Factions.GetStationByGridId(entityId) != null)
			{
				flag = false;
			}
			if (MyEventContext.Current.IsLocallyInvoked)
			{
				AskIsValidForEdit_Reponse(entityId, flag);
				return;
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AskIsValidForEdit_Reponse, entityId, flag, MyEventContext.Current.Sender);
		}

<<<<<<< HEAD
		[Event(null, 1702)]
=======
		[Event(null, 1707)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void AskIsValidForEdit_Reponse(long entityId, bool canEdit)
		{
			MyGuiScreenAdminMenu firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenAdminMenu>();
			if (firstScreenOfType != null && firstScreenOfType.m_attachCamera == entityId)
			{
				firstScreenOfType.m_attachIsNpcStation = !canEdit;
				UpdateRemoveAndDepowerButton(firstScreenOfType, firstScreenOfType.m_attachCamera);
			}
		}

		private void TimeDeltaChanged(MyGuiControlSlider slider)
		{
			MyTimeOfDayHelper.UpdateTimeOfDay(slider.Value);
			m_timeDeltaValue.Text = $"{slider.Value:0.00}";
		}

		public void ValueChanged(MyEntityList.MyEntityTypeEnum myEntityTypeEnum)
		{
			m_selectedType = myEntityTypeEnum;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => EntityListRequest, myEntityTypeEnum, arg3: false);
		}

		/// <summary>
		/// This method will only work with MyGuiScreenSafeZoneFilter.
		/// </summary>
		/// <param name="myEntityTypeEnum"></param>
		public static void RequestEntityList(MyEntityList.MyEntityTypeEnum myEntityTypeEnum)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => EntityListRequest, myEntityTypeEnum, arg3: true);
		}

		private void OnModeComboSelect()
		{
			if (m_currentPage == MyPageEnum.TrashRemoval && m_unsavedTrashSettings)
			{
				if (m_currentPage != (MyPageEnum)m_modeCombo.GetSelectedKey())
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: FinishTrashUnsavedTabChange, messageText: MyTexts.Get(MyCommonTexts.ScreenDebugAdminMenu_UnsavedTrash)));
				}
			}
			else
			{
				NewTabSelected();
			}
		}

		private void NewTabSelected()
		{
			m_currentPage = (MyPageEnum)m_modeCombo.GetSelectedKey();
			RecreateControls(constructor: false);
		}

		private void FinishTrashUnsavedTabChange(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				StoreTrashSettings_RealToTmp();
				NewTabSelected();
			}
			else
			{
				m_modeCombo.SelectItemByKey((long)m_currentPage);
			}
		}

		private void StoreTrashSettings_RealToTmp()
		{
			m_newSettings.Flags = MySession.Static.Settings.TrashFlags;
			m_newSettings.Enable = MySession.Static.Settings.TrashRemovalEnabled;
			m_newSettings.BlockCount = MySession.Static.Settings.BlockCountThreshold;
			m_newSettings.PlayerDistance = MySession.Static.Settings.PlayerDistanceThreshold;
			m_newSettings.GridCount = MySession.Static.Settings.OptimalGridCount;
			m_newSettings.PlayerInactivity = MySession.Static.Settings.PlayerInactivityThreshold;
			m_newSettings.CharacterRemovalThreshold = MySession.Static.Settings.PlayerCharacterRemovalThreshold;
			m_newSettings.StopGridsPeriod = MySession.Static.Settings.StopGridsPeriodMin;
			m_newSettings.RemoveOldIdentities = MySession.Static.Settings.RemoveOldIdentitiesH;
			m_newSettings.VoxelDistanceFromPlayer = MySession.Static.Settings.VoxelPlayerDistanceThreshold;
			m_newSettings.VoxelDistanceFromGrid = MySession.Static.Settings.VoxelGridDistanceThreshold;
			m_newSettings.VoxelAge = MySession.Static.Settings.VoxelAgeThreshold;
			m_newSettings.VoxelEnable = MySession.Static.Settings.VoxelTrashRemovalEnabled;
			m_newSettings.AfkTimeout = MySession.Static.Settings.AFKTimeountMin;
			m_unsavedTrashSettings = false;
		}

		private void StoreTrashSettings_TmpToReal()
		{
			if (Sync.IsServer && (((m_newSettings.Flags & MyTrashRemovalFlags.RevertBoulders) != 0 && (MySession.Static.Settings.TrashFlags & MyTrashRemovalFlags.RevertBoulders) == 0) || m_newSettings.VoxelAge != MySession.Static.Settings.VoxelAgeThreshold))
			{
				MySessionComponentTrash component = MySession.Static.GetComponent<MySessionComponentTrash>();
				if (component != null)
				{
					component.ClearBoulders = true;
				}
			}
			MySession.Static.Settings.TrashFlags = m_newSettings.Flags;
			MySession.Static.Settings.TrashRemovalEnabled = m_newSettings.Enable;
			MySession.Static.Settings.BlockCountThreshold = m_newSettings.BlockCount;
			MySession.Static.Settings.PlayerDistanceThreshold = m_newSettings.PlayerDistance;
			MySession.Static.Settings.OptimalGridCount = m_newSettings.GridCount;
			MySession.Static.Settings.PlayerInactivityThreshold = m_newSettings.PlayerInactivity;
			MySession.Static.Settings.PlayerCharacterRemovalThreshold = m_newSettings.CharacterRemovalThreshold;
			MySession.Static.Settings.StopGridsPeriodMin = m_newSettings.StopGridsPeriod;
			MySession.Static.Settings.RemoveOldIdentitiesH = m_newSettings.RemoveOldIdentities;
			MySession.Static.Settings.VoxelPlayerDistanceThreshold = m_newSettings.VoxelDistanceFromPlayer;
			MySession.Static.Settings.VoxelGridDistanceThreshold = m_newSettings.VoxelDistanceFromGrid;
			MySession.Static.Settings.VoxelAgeThreshold = m_newSettings.VoxelAge;
			MySession.Static.Settings.VoxelTrashRemovalEnabled = m_newSettings.VoxelEnable;
			MySession.Static.Settings.AFKTimeountMin = m_newSettings.AfkTimeout;
		}

		private void OnSubmitButtonClicked(MyGuiControlButton obj)
		{
			CheckAndStoreTrashTextboxChanges();
			if (MySession.Static.Settings.OptimalGridCount == 0 && MySession.Static.Settings.OptimalGridCount != m_newSettings.GridCount)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: FinishTrashSetting, messageText: MyTexts.Get(MyCommonTexts.ScreenDebugAdminMenu_GridCountWarning)));
			}
			else
			{
				FinishTrashSetting(MyGuiScreenMessageBox.ResultEnum.YES);
			}
		}

		private bool CheckAndStoreTrashTextboxChanges()
		{
			MyTabControlEnum key = (MyTabControlEnum)m_trashRemovalCombo.GetSelectedKey();
			if (!m_tabs.TryGetValue(key, out var _))
			{
				return false;
			}
			m_unsavedTrashSettings |= m_tabs[key].GetSettings(ref m_newSettings);
			return m_unsavedTrashSettings;
		}

		private void FinishTrashSetting(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				StoreTrashSettings_TmpToReal();
				m_unsavedTrashSettings = false;
				MySession.Static.GetComponent<MySessionComponentTrash>()?.SetPlayerAFKTimeout(m_newSettings.AfkTimeout);
				RecalcTrash();
				RecreateControls(constructor: false);
			}
		}

		private void OnCancelButtonClicked(MyGuiControlButton obj)
		{
			StoreTrashSettings_RealToTmp();
			RecreateControls(constructor: false);
			m_unsavedTrashSettings = false;
		}

		private void OnCycleClicked(bool reset, bool forward)
		{
			if (m_order != MyEntityCyclingOrder.Gps)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CycleRequest_Implementation, m_order, reset, forward, m_metricValue, m_entityId, m_cyclingOptions);
			}
			else
			{
				CircleGps(reset, forward);
			}
		}

		private void OnPlayerControl(MyGuiControlButton obj)
		{
			m_attachCamera = 0L;
			m_attachIsNpcStation = false;
			MySessionComponentAnimationSystem.Static.EntitySelectedForDebug = null;
			MyGuiScreenGamePlay.SetCameraController();
		}

		private void OnTeleportButton(MyGuiControlButton obj)
		{
			if (MySession.Static.CameraController != MySession.Static.LocalCharacter)
			{
				MyMultiplayer.TeleportControlledEntity(MySpectatorCameraController.Static.Position);
			}
		}

		private void OnRefreshButton(MyGuiControlButton obj)
		{
			RecreateControls(constructor: true);
		}

		private void OnRemoveOwnerButton(MyGuiControlButton obj)
		{
			HashSet<long> val = new HashSet<long>();
			List<long> list = new List<long>();
			foreach (MyGuiControlListbox.Item selectedItem in m_entityListbox.SelectedItems)
			{
				long owner = ((MyEntityList.MyEntityListInfoItem)selectedItem.UserData).Owner;
				MyPlayer.PlayerId result;
				MyPlayer player;
				if (owner == 0L)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder("No owner!"), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
				}
				else if (MySession.Static != null && MySession.Static.ControlledEntity != null && owner == MySession.Static.ControlledEntity.ControllerInfo.Controller.Player.Identity.IdentityId)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder("Cannot remove yourself!"), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
				}
				else if (MySession.Static.Players.TryGetPlayerId(owner, out result) && MySession.Static.Players.TryGetPlayerById(result, out player) && MySession.Static.Players.GetOnlinePlayers().Contains(player))
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder("Cannot remove online player " + player.DisplayName + ", kick him first!"), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
				}
				else
				{
					val.Add(owner);
				}
			}
			List<MyGuiControlListbox.Item> list2 = new List<MyGuiControlListbox.Item>();
			foreach (MyGuiControlListbox.Item item in m_entityListbox.Items)
			{
				if (val.Contains(((MyEntityList.MyEntityListInfoItem)item.UserData).Owner))
				{
					list2.Add(item);
					list.Add(((MyEntityList.MyEntityListInfoItem)item.UserData).EntityId);
				}
			}
			m_entityListbox.SelectedItems.Clear();
			foreach (MyGuiControlListbox.Item item2 in list2)
			{
				((Collection<MyGuiControlListbox.Item>)(object)m_entityListbox.Items).Remove(item2);
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RemoveOwner_Implementation, Enumerable.ToList<long>((IEnumerable<long>)val), list);
		}

		protected void OnOrderChanged(MyEntityCyclingOrder obj)
		{
			m_order = obj;
			UpdateSmallLargeGridSelection();
			UpdateCyclingAndDepower();
			OnCycleClicked(reset: true, forward: true);
		}

		private bool ValidCharacter(long entityId)
		{
			if (MyEntities.TryGetEntityById(entityId, out var entity))
			{
				MyCharacter myCharacter = entity as MyCharacter;
<<<<<<< HEAD
				if (myCharacter != null)
=======
				if (myCharacter != null && Sync.Players.TryGetPlayerId(myCharacter.ControllerInfo.ControllingIdentityId, out var result) && Sync.Players.GetPlayerById(result) != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					if (Sync.Players.TryGetPlayerId(myCharacter.ControllerInfo.ControllingIdentityId, out var result) && Sync.Players.GetPlayerById(result) != null)
					{
						MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_RemoveCharacterNotification)));
						return false;
					}
					if (myCharacter == MySession.Static.LocalCharacter)
					{
						MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_RemoveCharacterNotification)));
						return false;
					}
					if (myCharacter.IsSitting)
					{
						MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_RemoveSeatedCharacterNotification)));
						return false;
					}
				}
			}
			return true;
		}

		private void OnEntityListActionClicked(MyEntityList.EntityListAction action)
		{
			List<long> list = new List<long>();
			List<MyGuiControlListbox.Item> list2 = new List<MyGuiControlListbox.Item>();
			foreach (MyGuiControlListbox.Item selectedItem in m_entityListbox.SelectedItems)
			{
				if (!ValidCharacter(((MyEntityList.MyEntityListInfoItem)selectedItem.UserData).EntityId))
				{
					return;
				}
				list.Add(((MyEntityList.MyEntityListInfoItem)selectedItem.UserData).EntityId);
				list2.Add(selectedItem);
			}
			if (action == MyEntityList.EntityListAction.Remove)
			{
				m_entityListbox.SelectedItems.Clear();
				foreach (MyGuiControlListbox.Item item in list2)
				{
					((Collection<MyGuiControlListbox.Item>)(object)m_entityListbox.Items).Remove(item);
				}
				m_entityListbox.ScrollToolbarToTop();
				foreach (long item2 in list)
				{
					if (MyEntities.TryGetEntityById(item2, out var entity))
					{
						MyVoxelBase myVoxelBase = entity as MyVoxelBase;
						if (myVoxelBase != null && !myVoxelBase.SyncFlag)
						{
							myVoxelBase.Close();
						}
					}
				}
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ProceedEntitiesAction_Implementation, list, action);
		}

		private void OnEntityOperationClicked(MyEntityList.EntityListAction action)
		{
			if (m_attachCamera == 0L || !ValidCharacter(m_attachCamera))
			{
				return;
			}
			if (MyEntities.TryGetEntityById(m_attachCamera, out var entity))
			{
				MyVoxelBase myVoxelBase = entity as MyVoxelBase;
				if (myVoxelBase != null)
				{
					MyEntities.SendCloseRequest(myVoxelBase);
					return;
				}
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ProceedEntity_Implementation, m_attachCamera, action);
		}

		private void RaiseAdminSettingsChanged()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AdminSettingsChanged, MySession.Static.AdminSettings, Sync.MyId);
		}

		private void OnReplicateEverything(MyGuiControlButton button)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ReplicateEverything_Implementation);
		}

		private void OnEnableAdminModeChanged(MyGuiControlCheckbox checkbox)
		{
			MySession.Static.EnableCreativeTools(Sync.MyId, checkbox.IsChecked);
		}

		private void OnInvulnerableChanged(MyGuiControlCheckbox checkbox)
		{
			if (checkbox.IsChecked)
			{
				MySession.Static.AdminSettings |= AdminSettingsEnum.Invulnerable;
			}
			else
			{
				MySession.Static.AdminSettings &= ~AdminSettingsEnum.Invulnerable;
			}
			RaiseAdminSettingsChanged();
		}

		private void OnUntargetableChanged(MyGuiControlCheckbox checkbox)
		{
			if (checkbox.IsChecked)
			{
				MySession.Static.AdminSettings |= AdminSettingsEnum.Untargetable;
			}
			else
			{
				MySession.Static.AdminSettings &= ~AdminSettingsEnum.Untargetable;
			}
			RaiseAdminSettingsChanged();
		}

		private void OnKeepOwnershipChanged(MyGuiControlCheckbox checkbox)
		{
			if (checkbox.IsChecked)
			{
				MySession.Static.AdminSettings |= AdminSettingsEnum.KeepOriginalOwnershipOnPaste;
			}
			else
			{
				MySession.Static.AdminSettings &= ~AdminSettingsEnum.KeepOriginalOwnershipOnPaste;
			}
			RaiseAdminSettingsChanged();
		}

		private void OnIgnoreSafeZonesChanged(MyGuiControlCheckbox checkbox)
		{
			if (checkbox.IsChecked)
			{
				MySession.Static.AdminSettings |= AdminSettingsEnum.IgnoreSafeZones;
			}
			else
			{
				MySession.Static.AdminSettings &= ~AdminSettingsEnum.IgnoreSafeZones;
			}
			RaiseAdminSettingsChanged();
		}

		private void OnIgnorePcuChanged(MyGuiControlCheckbox checkbox)
		{
			if (checkbox.IsChecked)
			{
				MySession.Static.AdminSettings |= AdminSettingsEnum.IgnorePcu;
			}
			else
			{
				MySession.Static.AdminSettings &= ~AdminSettingsEnum.IgnorePcu;
			}
			RaiseAdminSettingsChanged();
		}

		private void OnUseTerminalsChanged(MyGuiControlCheckbox checkbox)
		{
			if (checkbox.IsChecked)
			{
				MySession.Static.AdminSettings |= AdminSettingsEnum.UseTerminals;
			}
			else
			{
				MySession.Static.AdminSettings &= ~AdminSettingsEnum.UseTerminals;
			}
			RaiseAdminSettingsChanged();
		}

		private void OnShowPlayersChanged(MyGuiControlCheckbox checkbox)
		{
			if (checkbox.IsChecked)
			{
				MySession.Static.AdminSettings |= AdminSettingsEnum.ShowPlayers;
			}
			else
			{
				MySession.Static.AdminSettings &= ~AdminSettingsEnum.ShowPlayers;
			}
			RaiseAdminSettingsChanged();
		}

		private void OnSmallGridChanged(MyGuiControlCheckbox checkbox)
		{
			m_cyclingOptions.OnlySmallGrids = checkbox.IsChecked;
			if (m_cyclingOptions.OnlySmallGrids && m_onlyLargeGridsCheckbox != null)
			{
				m_onlyLargeGridsCheckbox.IsChecked = false;
			}
		}

		private void OnLargeGridChanged(MyGuiControlCheckbox checkbox)
		{
			m_cyclingOptions.OnlyLargeGrids = checkbox.IsChecked;
			if (m_cyclingOptions.OnlyLargeGrids)
			{
				m_onlySmallGridsCheckbox.IsChecked = false;
			}
		}

<<<<<<< HEAD
		[Event(null, 2191)]
=======
		[Event(null, 2184)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void EntityListRequest(MyEntityList.MyEntityTypeEnum selectedType, bool requestedBySafeZoneFilter)
		{
			if (!requestedBySafeZoneFilter && !MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			List<MyEntityList.MyEntityListInfoItem> entityList = MyEntityList.GetEntityList(selectedType);
			if (!MyEventContext.Current.IsLocallyInvoked)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => EntityListResponse, entityList, MyEventContext.Current.Sender);
			}
			else
			{
				EntityListResponse(entityList);
			}
		}

<<<<<<< HEAD
		[Event(null, 2208)]
=======
		[Event(null, 2201)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void CycleRequest_Implementation(MyEntityCyclingOrder order, bool reset, bool findLarger, float metricValue, long currentEntityId, CyclingOptions options)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			if (reset)
			{
				metricValue = float.MinValue;
				currentEntityId = 0L;
				findLarger = false;
			}
			MyEntityCycling.FindNext(order, ref metricValue, ref currentEntityId, findLarger, options);
			Vector3D vector3D = MyEntities.GetEntityByIdOrDefault(currentEntityId)?.WorldMatrix.Translation ?? Vector3D.Zero;
			bool flag = false;
			if (MySession.Static != null)
			{
				MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
				if (component != null && component.IsGridStation(currentEntityId))
				{
					flag = true;
				}
			}
			if (MyEventContext.Current.IsLocallyInvoked)
			{
				Cycle_Implementation(metricValue, currentEntityId, vector3D, flag);
				return;
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => Cycle_Implementation, metricValue, currentEntityId, vector3D, flag, MyEventContext.Current.Sender);
		}

<<<<<<< HEAD
		[Event(null, 2247)]
=======
		[Event(null, 2240)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Server]
		[Reliable]
		private static void RemoveOwner_Implementation(List<long> owners, List<long> entityIds)
		{
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			foreach (long entityId in entityIds)
			{
				if (MyEntities.TryGetEntityById(entityId, out var entity))
				{
					MyEntityList.ProceedEntityAction(entity, MyEntityList.EntityListAction.Remove);
				}
			}
			foreach (long owner in owners)
			{
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(owner);
				if (myIdentity.Character != null)
				{
					myIdentity.Character.Close();
				}
				Enumerator<long> enumerator2 = myIdentity.SavedCharacters.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						if (MyEntities.TryGetEntityById(enumerator2.get_Current(), out MyCharacter entity2, allowClosed: true) && (!entity2.Closed || entity2.MarkedForClose))
						{
							entity2.Close();
						}
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
				if (myIdentity != null && myIdentity.BlockLimits.BlocksBuilt == 0)
				{
					MySession.Static.Players.RemoveIdentity(owner);
				}
			}
		}

<<<<<<< HEAD
		[Event(null, 2290)]
=======
		[Event(null, 2283)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Server]
		[Reliable]
		private static void ProceedEntitiesAction_Implementation(List<long> entityIds, MyEntityList.EntityListAction action)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			foreach (long entityId in entityIds)
			{
				if (MyEntities.TryGetEntityById(entityId, out var entity))
				{
					MyEntityList.ProceedEntityAction(entity, action);
				}
			}
		}

<<<<<<< HEAD
		[Event(null, 2307)]
=======
		[Event(null, 2300)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void UploadSettingsToServer(AdminSettings settings)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			if ((settings.Flags & MyTrashRemovalFlags.RevertBoulders) != 0 && settings.PlayerDistance < (float)BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE)
			{
				settings.PlayerDistance = BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE;
			}
			if (MySession.Static.Settings.TrashFlags != settings.Flags)
			{
				MyLog.Default.Info($"Trash flags changed by {MyEventContext.Current.Sender} to {settings.Flags}");
				if ((settings.Flags & MyTrashRemovalFlags.RevertBoulders) != 0 && (MySession.Static.Settings.TrashFlags & MyTrashRemovalFlags.RevertBoulders) == 0)
				{
					MySessionComponentTrash component = MySession.Static.GetComponent<MySessionComponentTrash>();
					if (component != null)
					{
						component.ClearBoulders = true;
					}
				}
			}
			MySession.Static.Settings.TrashFlags = settings.Flags;
			if (MySession.Static.Settings.TrashRemovalEnabled != settings.Enable)
			{
				MyLog.Default.Info($"Trash Trash Removal changed by {MyEventContext.Current.Sender} to {settings.Enable}");
			}
			MySession.Static.Settings.TrashRemovalEnabled = settings.Enable;
			if (MySession.Static.Settings.BlockCountThreshold != settings.BlockCount)
			{
				MyLog.Default.Info($"Trash Block Count changed by {MyEventContext.Current.Sender} to {settings.BlockCount}");
			}
			MySession.Static.Settings.BlockCountThreshold = settings.BlockCount;
			if (MySession.Static.Settings.PlayerDistanceThreshold != settings.PlayerDistance)
			{
				MyLog.Default.Info($"Trash Player Distance Treshold changed by {MyEventContext.Current.Sender} to {settings.PlayerDistance}");
			}
			MySession.Static.Settings.PlayerDistanceThreshold = settings.PlayerDistance;
			if (MySession.Static.Settings.OptimalGridCount != settings.GridCount)
			{
				MyLog.Default.Info($"Trash Optimal Grid Count changed by {MyEventContext.Current.Sender} to {settings.GridCount}");
			}
			MySession.Static.Settings.OptimalGridCount = settings.GridCount;
			if (MySession.Static.Settings.PlayerInactivityThreshold != settings.PlayerInactivity)
			{
				MyLog.Default.Info($"Trash Player Inactivity Threshold changed by {MyEventContext.Current.Sender} to {settings.PlayerInactivity}");
			}
			MySession.Static.Settings.PlayerInactivityThreshold = settings.PlayerInactivity;
			if (MySession.Static.Settings.PlayerCharacterRemovalThreshold != settings.CharacterRemovalThreshold)
			{
				MyLog.Default.Info($"Trash Player Character Removal Threshold changed by {MyEventContext.Current.Sender} to {settings.CharacterRemovalThreshold}");
			}
			MySession.Static.Settings.PlayerCharacterRemovalThreshold = settings.CharacterRemovalThreshold;
			if (MySession.Static.Settings.StopGridsPeriodMin != settings.StopGridsPeriod)
			{
				MyLog.Default.Info($"Trash Stop Grids Period changed by {MyEventContext.Current.Sender} to {settings.StopGridsPeriod}");
			}
			MySession.Static.Settings.StopGridsPeriodMin = settings.StopGridsPeriod;
			if (MySession.Static.Settings.VoxelPlayerDistanceThreshold != settings.VoxelDistanceFromPlayer)
			{
				MyLog.Default.Info($"Trash Voxel Player Distance Threshold changed by {MyEventContext.Current.Sender} to {settings.VoxelDistanceFromPlayer}");
			}
			MySession.Static.Settings.VoxelPlayerDistanceThreshold = settings.VoxelDistanceFromPlayer;
			if (MySession.Static.Settings.VoxelGridDistanceThreshold != settings.VoxelDistanceFromGrid)
			{
				MyLog.Default.Info($"Trash Voxel Grid Distance Threshold changed by {MyEventContext.Current.Sender} to {settings.VoxelDistanceFromGrid}");
			}
			MySession.Static.Settings.VoxelGridDistanceThreshold = settings.VoxelDistanceFromGrid;
			if (MySession.Static.Settings.VoxelAgeThreshold != settings.VoxelAge)
			{
				MyLog.Default.Info($"Trash Voxel Age Threshold changed by {MyEventContext.Current.Sender} to {settings.VoxelAge}");
				if ((settings.Flags & MyTrashRemovalFlags.RevertBoulders) != 0)
				{
					MySessionComponentTrash component2 = MySession.Static.GetComponent<MySessionComponentTrash>();
					if (component2 != null)
					{
						component2.ClearBoulders = true;
					}
				}
			}
			MySession.Static.Settings.VoxelAgeThreshold = settings.VoxelAge;
			if (MySession.Static.Settings.VoxelTrashRemovalEnabled != settings.VoxelEnable)
			{
				MyLog.Default.Info($"Trash Voxel Trash Removal Enabled changed by {MyEventContext.Current.Sender} to {settings.VoxelEnable}");
			}
			MySession.Static.Settings.VoxelTrashRemovalEnabled = settings.VoxelEnable;
			if (MySession.Static.Settings.RemoveOldIdentitiesH != settings.RemoveOldIdentities)
			{
				MyLog.Default.Info($"Trash Identities removal time changed by {MyEventContext.Current.Sender} to {settings.RemoveOldIdentities}");
			}
			MySession.Static.Settings.RemoveOldIdentitiesH = settings.RemoveOldIdentities;
		}

<<<<<<< HEAD
		[Event(null, 2393)]
=======
		[Event(null, 2386)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void ProceedEntity_Implementation(long entityId, MyEntityList.EntityListAction action)
		{
			MyEntity entity;
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else if (MyEntities.TryGetEntityById(entityId, out entity))
			{
				MyEntityList.ProceedEntityAction(entity, action);
			}
		}

<<<<<<< HEAD
		[Event(null, 2407)]
=======
		[Event(null, 2400)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void ReplicateEverything_Implementation()
		{
			if (!MyEventContext.Current.IsLocallyInvoked)
			{
				if (!MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
				{
					(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				}
				else
				{
					((MyReplicationServer)MyMultiplayer.Static.ReplicationLayer).ForceEverything(new Endpoint(MyEventContext.Current.Sender, 0));
				}
			}
		}

<<<<<<< HEAD
		[Event(null, 2423)]
=======
		[Event(null, 2416)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void AdminSettingsChanged(AdminSettingsEnum settings, ulong steamId)
		{
			if (MySession.Static.OnlineMode != 0 && (((settings & AdminSettingsEnum.AdminOnly) > AdminSettingsEnum.None && !MySession.Static.IsUserAdmin(steamId)) || !MySession.Static.IsUserModerator(steamId)))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			MySession.Static.RemoteAdminSettings[steamId] = settings;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AdminSettingsChangedClient, settings, steamId);
		}

<<<<<<< HEAD
		[Event(null, 2439)]
=======
		[Event(null, 2432)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[BroadcastExcept]
		private static void AdminSettingsChangedClient(AdminSettingsEnum settings, ulong steamId)
		{
			MySession.Static.RemoteAdminSettings[steamId] = settings;
		}

<<<<<<< HEAD
		[Event(null, 2449)]
=======
		[Event(null, 2442)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void EntityListResponse(List<MyEntityList.MyEntityListInfoItem> entities)
		{
			if (entities == null)
			{
				return;
			}
			MyGuiScreenSafeZoneFilter firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenSafeZoneFilter>();
			if (firstScreenOfType != null)
			{
				MyGuiControlListbox entityListbox = firstScreenOfType.m_entityListbox;
				((Collection<MyGuiControlListbox.Item>)(object)entityListbox.Items).Clear();
				MyEntityList.SortEntityList(MyEntityList.MyEntitySortOrder.DisplayName, ref entities, m_invertOrder);
				foreach (MyEntityList.MyEntityListInfoItem entity in entities)
				{
					if (firstScreenOfType.m_selectedSafeZone == null || firstScreenOfType.m_selectedSafeZone.Entities == null || !firstScreenOfType.m_selectedSafeZone.Entities.Contains(entity.EntityId))
					{
						StringBuilder formattedDisplayName = MyEntityList.GetFormattedDisplayName(MyEntityList.MyEntitySortOrder.DisplayName, entity);
						if (formattedDisplayName != null)
						{
<<<<<<< HEAD
							entityListbox.Items.Add(new MyGuiControlListbox.Item(formattedDisplayName, null, null, entity.EntityId));
=======
							((Collection<MyGuiControlListbox.Item>)(object)entityListbox.Items).Add(new MyGuiControlListbox.Item(formattedDisplayName, null, null, entity.EntityId));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
				}
				return;
			}
			MyGuiScreenAdminMenu @static = m_static;
			if (@static == null)
			{
				return;
			}
			MyGuiControlListbox entityListbox2 = @static.m_entityListbox;
			if (entityListbox2 == null)
			{
				return;
			}
<<<<<<< HEAD
			entityListbox2.Items.Clear();
=======
			((Collection<MyGuiControlListbox.Item>)(object)entityListbox2.Items).Clear();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyEntityList.SortEntityList(@static.m_selectedSort, ref entities, m_invertOrder);
			if (@static.m_selectedType == MyEntityList.MyEntityTypeEnum.Grids || @static.m_selectedType == MyEntityList.MyEntityTypeEnum.LargeGrids)
			{
				_ = 1;
			}
			else
				_ = @static.m_selectedType == MyEntityList.MyEntityTypeEnum.SmallGrids;
			foreach (MyEntityList.MyEntityListInfoItem entity2 in entities)
			{
				StringBuilder formattedDisplayName2 = MyEntityList.GetFormattedDisplayName(@static.m_selectedSort, entity2);
<<<<<<< HEAD
				entityListbox2.Items.Add(new MyGuiControlListbox.Item(formattedDisplayName2, MyEntityList.GetDescriptionText(entity2), null, entity2));
			}
		}

		[Event(null, 2497)]
=======
				((Collection<MyGuiControlListbox.Item>)(object)entityListbox2.Items).Add(new MyGuiControlListbox.Item(formattedDisplayName2, MyEntityList.GetDescriptionText(entity2), null, entity2));
			}
		}

		[Event(null, 2490)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void Cycle_Implementation(float newMetricValue, long newEntityId, Vector3D position, bool isNpcStation)
		{
			m_metricValue = newMetricValue;
			m_entityId = newEntityId;
			if (m_entityId != 0L && !TryAttachCamera(m_entityId))
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, null, position + Vector3.One * 50f);
			}
			MyGuiScreenAdminMenu firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenAdminMenu>();
			if (firstScreenOfType != null)
			{
				firstScreenOfType.m_attachCamera = m_entityId;
				firstScreenOfType.m_attachIsNpcStation = isNpcStation;
				UpdateRemoveAndDepowerButton(firstScreenOfType, m_entityId);
				firstScreenOfType.m_labelCurrentIndex?.TextToDraw.Clear().AppendFormat(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_CurrentValue), (m_entityId == 0L) ? "-" : m_metricValue.ToString());
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Catch response on SM settings. Save them and show SM.
		/// </summary>
		/// <param name="settings"></param>
		[Event(null, 2529)]
=======
		[Event(null, 2526)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void DownloadSettingFromServer(AdminSettings settings)
		{
			MySession.Static.Settings.TrashFlags = settings.Flags;
			MySession.Static.Settings.TrashRemovalEnabled = settings.Enable;
			MySession.Static.Settings.BlockCountThreshold = settings.BlockCount;
			MySession.Static.Settings.PlayerDistanceThreshold = settings.PlayerDistance;
			MySession.Static.Settings.OptimalGridCount = settings.GridCount;
			MySession.Static.Settings.PlayerInactivityThreshold = settings.PlayerInactivity;
			MySession.Static.Settings.PlayerCharacterRemovalThreshold = settings.CharacterRemovalThreshold;
			MySession.Static.Settings.StopGridsPeriodMin = settings.StopGridsPeriod;
			MySession.Static.Settings.RemoveOldIdentitiesH = settings.RemoveOldIdentities;
			MySession.Static.Settings.VoxelPlayerDistanceThreshold = settings.VoxelDistanceFromPlayer;
			MySession.Static.Settings.VoxelGridDistanceThreshold = settings.VoxelDistanceFromGrid;
			MySession.Static.Settings.VoxelAgeThreshold = settings.VoxelAge;
			MySession.Static.Settings.VoxelTrashRemovalEnabled = settings.VoxelEnable;
			MySession.Static.AdminSettings = settings.AdminSettingsFlags;
			if (m_static != null)
			{
				m_static.CreateScreen();
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (m_attachCamera != 0L)
			{
				TryAttachCamera(m_attachCamera);
				UpdateRemoveAndDepowerButton(this, m_attachCamera);
			}
			if (MyPlatformGameSettings.ENABLE_LOW_MEM_WORLD_LOCKDOWN)
			{
				if (m_cleanupRequestingMessageBox != null && MySandboxGame.Static.MemoryState != MySandboxGame.MemState.Critical)
				{
					m_cleanupRequestingMessageBox.CloseScreen();
					m_cleanupRequestingMessageBox = null;
				}
				else if (MySandboxGame.Static.MemoryState == MySandboxGame.MemState.Critical && m_cleanupRequestingMessageBox == null)
				{
					ShowCleanupRequest();
				}
			}
			UpdateWeatherInfo();
			UpdateMatch();
			return base.Update(hasFocus);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenAdminMenu";
		}

		public override bool RegisterClicks()
		{
			return true;
		}

		public override bool Draw()
		{
			if (base.Draw())
			{
				return true;
			}
			return false;
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.A) && base.FocusedControl == m_entityListbox)
			{
				m_entityListbox.SelectedItems.Clear();
				m_entityListbox.SelectedItems.AddRange((IEnumerable<MyGuiControlListbox.Item>)m_entityListbox.Items);
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || (m_defaultJoystickCancelUse && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL)) || MyInput.Static.IsNewKeyPressed(MyKeys.F12) || MyInput.Static.IsNewKeyPressed(MyKeys.F11) || MyInput.Static.IsNewKeyPressed(MyKeys.F10))
			{
				ExitButtonPressed();
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_NONE))
			{
				SelectNextCharacter();
			}
		}

		public void ExitButtonPressed()
		{
			if (m_cleanupRequestingMessageBox != null)
			{
				return;
			}
			if (m_currentPage == MyPageEnum.TrashRemoval)
			{
				CheckAndStoreTrashTextboxChanges();
				if (m_unsavedTrashSettings)
				{
					if (!m_unsavedTrashExitBoxIsOpened)
					{
						m_unsavedTrashExitBoxIsOpened = true;
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: FinishTrashUnsavedExiting, messageText: MyTexts.Get(MyCommonTexts.ScreenDebugAdminMenu_UnsavedTrash)));
					}
				}
				else
				{
					CloseScreen();
				}
			}
			else
			{
				CloseScreen();
			}
		}

		private void FinishTrashUnsavedExiting(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				StoreTrashSettings_RealToTmp();
				CloseScreen();
			}
			m_unsavedTrashExitBoxIsOpened = false;
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			m_static = null;
			MySessionComponentSafeZones.OnAddSafeZone -= MySafeZones_OnAddSafeZone;
			MySessionComponentSafeZones.OnRemoveSafeZone -= MySafeZones_OnRemoveSafeZone;
			return base.CloseScreen(isUnloading);
		}

		private void ShowCleanupRequest()
		{
			m_cleanupRequestingMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.NONE, MyTexts.Get(MyCommonTexts.MessageBoxTextCriticalMemory), MyTexts.Get(MyCommonTexts.MessageBoxCaptionCriticalMemory), null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: true, null, useOpacity: true, new Vector2(0.5f - m_size.Value.X / 2f, 0.5f), focusable: false);
			MyGuiSandbox.AddScreen(m_cleanupRequestingMessageBox);
		}

		protected virtual void CreateSelectionCombo()
		{
			AddCombo(m_order, OnOrderChanged, enabled: true, 10, null, m_labelColor);
		}

		private MyGuiControlButton CreateDebugButton(float usableWidth, MyStringId text, Action<MyGuiControlButton> onClick, bool enabled = true, MyStringId? tooltip = null, bool increaseSpacing = true, bool addToControls = true, bool isAutoScaleEnabled = false, bool isAutoEllipsisEnabled = false)
		{
			MyGuiControlButton myGuiControlButton = AddButton(MyTexts.Get(text), onClick, null, null, null, increaseSpacing, addToControls);
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
			myGuiControlButton.TextScale = m_scale;
			myGuiControlButton.Size = new Vector2(usableWidth, myGuiControlButton.Size.Y);
			myGuiControlButton.Position += new Vector2((0f - HIDDEN_PART_RIGHT) / 2f, 0f);
			myGuiControlButton.Enabled = enabled;
			myGuiControlButton.IsAutoScaleEnabled = isAutoScaleEnabled;
			myGuiControlButton.IsAutoEllipsisEnabled = isAutoEllipsisEnabled;
			if (tooltip.HasValue)
			{
				myGuiControlButton.SetToolTip(tooltip.Value);
			}
			return myGuiControlButton;
		}

		private void AddSeparator()
		{
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.Size = new Vector2(1f, 0.01f);
			myGuiControlSeparatorList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			myGuiControlSeparatorList.AddHorizontal(Vector2.Zero, 1f);
			Controls.Add(myGuiControlSeparatorList);
		}

		private MyGuiControlLabel CreateSliderWithDescription(MyGuiControlList list, float usableWidth, float min, float max, string description, ref MyGuiControlSlider slider)
		{
			MyGuiControlLabel control = AddLabel(description, Vector4.One, m_scale);
			Controls.Remove(control);
			list.Controls.Add(control);
			CreateSlider(list, usableWidth, min, max, ref slider);
			MyGuiControlLabel myGuiControlLabel = AddLabel("", Vector4.One, m_scale);
			Controls.Remove(myGuiControlLabel);
			list.Controls.Add(myGuiControlLabel);
			return myGuiControlLabel;
		}

		private void CreateSlider(MyGuiControlList list, float usableWidth, float min, float max, ref MyGuiControlSlider slider)
		{
			Vector2? position = m_currentPosition;
			float width = 400f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			slider = new MyGuiControlSlider(position, min, max, width, null, null, string.Empty, 4, 0.75f * m_scale, 0f, "Debug", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			slider.DebugScale = m_sliderDebugScale;
			slider.ColorMask = Color.White.ToVector4();
			list.Controls.Add(slider);
		}

		private void RecreateGlobalSafeZoneControls(ref Vector2 controlPadding, float separatorSize, float usableWidth)
		{
			m_recreateInProgress = true;
			m_currentPosition.Y += 0.03f;
			m_damageCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDamage)
			};
			m_damageGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_damageCheckboxGlobalLabel);
			Controls.Add(m_damageGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_shootingCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowShooting)
			};
			m_shootingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_shootingCheckboxGlobalLabel);
			Controls.Add(m_shootingGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_drillingCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDrilling)
			};
			m_drillingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_drillingCheckboxGlobalLabel);
			Controls.Add(m_drillingGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_weldingCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowWelding)
			};
			m_weldingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_weldingCheckboxGlobalLabel);
			Controls.Add(m_weldingGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_grindingCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowGrinding)
			};
			m_grindingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_grindingCheckboxGlobalLabel);
			Controls.Add(m_grindingGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_buildingCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuilding)
			};
			m_buildingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_buildingCheckboxGlobalLabel);
			Controls.Add(m_buildingGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_buildingProjectionsCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuildingProjections)
			};
			m_buildingProjectionsGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_buildingProjectionsCheckboxGlobalLabel);
			Controls.Add(m_buildingProjectionsGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_voxelHandCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowVoxelHands)
			};
			m_voxelHandGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_voxelHandCheckboxGlobalLabel);
			Controls.Add(m_voxelHandGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_landingGearCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowLandingGear),
				IsAutoScaleEnabled = true
			};
			m_landingGearCheckboxGlobalLabel.SetMaxWidth(0.25f);
			m_landingGearGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_landingGearGlobalCheckbox.UserData = MySafeZoneAction.LandingGearLock;
			Controls.Add(m_landingGearCheckboxGlobalLabel);
			Controls.Add(m_landingGearGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_convertToStationCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowConvertToStation)
			};
			m_convertToStationGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_convertToStationGlobalCheckbox.UserData = MySafeZoneAction.ConvertToStation;
			Controls.Add(m_convertToStationCheckboxGlobalLabel);
			Controls.Add(m_convertToStationGlobalCheckbox);
			UpdateSelectedGlobalData();
			m_voxelHandGlobalCheckbox.IsCheckedChanged = VoxelHandCheckGlobalChanged;
			m_buildingGlobalCheckbox.IsCheckedChanged = BuildingCheckGlobalChanged;
			m_buildingProjectionsGlobalCheckbox.IsCheckedChanged = BuildingProjectionsCheckGlobalChanged;
			m_grindingGlobalCheckbox.IsCheckedChanged = GrindingCheckGlobalChanged;
			m_weldingGlobalCheckbox.IsCheckedChanged = WeldingCheckGlobalChanged;
			m_drillingGlobalCheckbox.IsCheckedChanged = DrillingCheckGlobalChanged;
			m_shootingGlobalCheckbox.IsCheckedChanged = ShootingCheckGlobalChanged;
			m_damageGlobalCheckbox.IsCheckedChanged = DamageCheckGlobalChanged;
			m_landingGearGlobalCheckbox.IsCheckedChanged = OnSettingCheckGlobalChanged;
			m_convertToStationGlobalCheckbox.IsCheckedChanged = OnSettingCheckGlobalChanged;
		}

		private void UpdateSelectedGlobalData()
		{
			m_damageGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Damage) > (MySafeZoneAction)0;
			m_shootingGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Shooting) > (MySafeZoneAction)0;
			m_drillingGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Drilling) > (MySafeZoneAction)0;
			m_weldingGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Welding) > (MySafeZoneAction)0;
			m_grindingGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Grinding) > (MySafeZoneAction)0;
			m_voxelHandGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.VoxelHand) > (MySafeZoneAction)0;
			m_buildingGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Building) > (MySafeZoneAction)0;
			m_buildingProjectionsGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.BuildingProjections) > (MySafeZoneAction)0;
			m_landingGearGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.LandingGearLock) > (MySafeZoneAction)0;
			m_convertToStationGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.ConvertToStation) > (MySafeZoneAction)0;
		}

		private void DamageCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Damage;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Damage;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void ShootingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Shooting;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Shooting;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void DrillingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Drilling;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Drilling;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void WeldingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Welding;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Welding;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void GrindingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Grinding;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Grinding;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void VoxelHandCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.VoxelHand;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.VoxelHand;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void BuildingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Building;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Building;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void BuildingProjectionsCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.BuildingProjections;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.BuildingProjections;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void OnSettingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= (MySafeZoneAction)checkBox.UserData;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~(MySafeZoneAction)checkBox.UserData;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void RecreateMatchControls(bool constructor)
		{
			m_recreateInProgress = true;
			float num = 0.16f;
			Vector2 vector = new Vector2(0f, 0f);
			m_currentPosition.Y += 0.025f;
			m_labelEnabled = new MyGuiControlLabel
			{
				Position = m_currentPosition + vector,
<<<<<<< HEAD
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = "",
				IsAutoScaleEnabled = true,
				IsAutoEllipsisEnabled = true
			};
			m_labelEnabled.SetMaxWidth(0.3f);
			Controls.Add(m_labelEnabled);
			m_currentPosition.Y += 0.025f;
			m_labelRunning = new MyGuiControlLabel
			{
				Position = m_currentPosition + vector,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = "",
				IsAutoScaleEnabled = true,
				IsAutoEllipsisEnabled = true
			};
			m_labelRunning.SetMaxWidth(0.3f);
			Controls.Add(m_labelRunning);
			m_currentPosition.Y += 0.025f;
			m_labelState = new MyGuiControlLabel
			{
				Position = m_currentPosition + vector,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = "",
				IsAutoScaleEnabled = true,
				IsAutoEllipsisEnabled = true
			};
			m_labelState.SetMaxWidth(0.3f);
			Controls.Add(m_labelState);
			m_currentPosition.Y += 0.025f;
			m_labelTime = new MyGuiControlLabel
			{
				Position = m_currentPosition + vector,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = "",
				IsAutoScaleEnabled = true,
				IsAutoEllipsisEnabled = true
			};
			m_labelTime.SetMaxWidth(0.3f);
			Controls.Add(m_labelTime);
			m_currentPosition.Y += 0.1f;
			m_buttonStart = CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_Start, StartMatch, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Match_Start_Tooltip);
			m_currentPosition.Y += 0.002f;
			if (MyFakes.SHOW_MATCH_STOP)
			{
				m_buttonStop = CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_Stop, StopMatch, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Match_Stop_Tooltip);
				m_currentPosition.Y += 0.002f;
			}
			m_buttonPause = CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_Pause, PauseMatch, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Match_Pause_Tooltip);
			m_currentPosition.Y += 0.002f;
			m_buttonAdvanced = CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_Advance, ProgressMatch, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Match_Advance_Tooltip);
			UpdatePauseButtonTexts();
			m_currentPosition.Y += 0.04f;
			m_textboxTime = new MyGuiControlTextbox(null, "0", 512, null, 0.8f, MyGuiControlTextboxType.DigitsOnly);
			m_textboxTime.TextAlignment = TextAlingmentMode.Right;
			m_textboxTime.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			m_textboxTime.Position = m_currentPosition + new Vector2(num - HIDDEN_PART_RIGHT / 2f, 0f);
			m_textboxTime.Size = new Vector2(num, m_textboxTime.Size.Y);
			m_textboxTime.TextChanged += MatchTimeTextbox_Changed;
			Controls.Add(m_textboxTime);
			m_currentPosition.Y += 0.035f;
			m_buttonSetTime = CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_SetTime, SetTime, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Match_SetTime_Tooltip);
			m_currentPosition.Y += 0.002f;
			m_buttonAddTime = CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_AddTime, AddTime, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Match_AddTime_Tooltip);
			SyncData();
		}

		private void MatchTimeTextbox_Changed(MyGuiControlTextbox obj)
		{
			ClearTextColor();
		}

		private void SyncData()
		{
			m_matchSyncReceivers.Add(this);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SendDataToClient);
		}

		[Event(null, 146)]
		[Reliable]
		[Server]
		public static void SendDataToClient()
		{
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component != null)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ReciveClientData, component.State, component.RemainingMinutes, component.IsRunning, component.IsEnabled, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 156)]
		[Reliable]
		[Client]
		public static void ReciveClientData(MyMatchState state, float remainingTime, bool isRunning, bool isEnabled)
		{
			foreach (MyGuiScreenAdminMenu matchSyncReceiver in m_matchSyncReceivers)
			{
				matchSyncReceiver.SetSyncData(state, remainingTime, isRunning, isEnabled);
			}
			m_matchSyncReceivers.Clear();
		}

		private void SetSyncData(MyMatchState state, float remainingTime, bool isRunning, bool isEnabled)
		{
			m_matchCurrentState = state;
			m_matchRemainingTime = MyTimeSpan.FromMinutes(remainingTime);
			m_isMatchRunning = isRunning;
			m_isMatchEnabled = isEnabled;
			m_matchLastUpdateTime = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			ResetStateTexts();
			UpdatePauseButtonTexts();
		}

		private void ResetStateTexts()
		{
			if (m_isMatchEnabled)
			{
				m_labelEnabled.Text = string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_EnableText));
			}
			else
			{
				m_labelEnabled.Text = string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_DisableText));
			}
			if (m_isMatchRunning)
			{
				m_labelRunning.Text = string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_RunningText));
			}
			else
			{
				m_labelRunning.Text = string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_NotRunningText));
			}
			m_labelState.Text = string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_StateText), m_matchCurrentState.ToString());
			UpdateTimeText();
		}

		private void UpdatePauseButtonTexts()
		{
			if (m_isMatchRunning)
			{
				m_buttonPause.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_Pause);
				m_buttonPause.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_Match_Pause_Tooltip);
			}
			else
			{
				m_buttonPause.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_Unpause);
				m_buttonPause.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_Match_Unpause_Tooltip);
			}
		}

		private void StartMatch(MyGuiControlButton obj)
		{
			m_matchSyncReceivers.Add(this);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => StartMatchInternal);
		}

		private void StopMatch(MyGuiControlButton obj)
		{
			m_matchSyncReceivers.Add(this);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => StopMatchInternal);
		}

		private void PauseMatch(MyGuiControlButton obj)
		{
			m_matchSyncReceivers.Add(this);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => PauseMatchInternal);
		}

		private void ProgressMatch(MyGuiControlButton obj)
		{
			m_matchSyncReceivers.Add(this);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ProgressMatchInternal);
		}

		private void SetTime(MyGuiControlButton obj)
		{
			if (!float.TryParse(m_textboxTime.Text, out var result))
			{
				SetTextRed();
				return;
			}
			if (result < 0f)
			{
				result = 0f;
			}
			m_matchSyncReceivers.Add(this);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetTimeInternal, result);
			ClearTextColor();
		}

		private void AddTime(MyGuiControlButton obj)
		{
			if (float.TryParse(m_textboxTime.Text, out var result))
			{
				m_matchSyncReceivers.Add(this);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AddTimeInternal, result);
			}
		}

		[Event(null, 262)]
		[Reliable]
		[Server]
		public static void StartMatchInternal()
		{
			if (!ValidatePlayer())
			{
				return;
			}
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component != null)
			{
				component.ResetToFirstState();
				if (component.State == MyMatchState.PreMatch)
				{
					string playerName = GetPlayerName(MyEventContext.Current.Sender.Value);
					SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Started), playerName));
				}
				SendDataToClient();
			}
		}

		[Event(null, 282)]
		[Reliable]
		[Server]
		public static void StopMatchInternal()
		{
			if (!ValidatePlayer())
			{
				return;
			}
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component == null)
			{
				return;
			}
			int num = LOOP_LIMIT;
			bool flag = true;
			while (component.State != MyMatchState.Finished)
			{
				MyMatchState state = component.State;
				component.AdvanceToNextState();
				num = ((state != component.State) ? LOOP_LIMIT : (num - 1));
				if (num < 0)
				{
					flag = false;
					break;
				}
			}
			if (flag && component.State == MyMatchState.Finished)
			{
				string playerName = GetPlayerName(MyEventContext.Current.Sender.Value);
				SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Stopped), playerName));
			}
			else
			{
				SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_StopFailed)));
			}
			SendDataToClient();
		}

		[Event(null, 324)]
		[Reliable]
		[Server]
		public static void PauseMatchInternal()
		{
			if (!ValidatePlayer())
			{
				return;
			}
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component != null)
			{
				bool isRunning = component.IsRunning;
				component.SetIsRunning(!isRunning);
				if (isRunning)
				{
					string playerName = GetPlayerName(MyEventContext.Current.Sender.Value);
					SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Paused), playerName));
				}
				else
				{
					string playerName2 = GetPlayerName(MyEventContext.Current.Sender.Value);
					SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Unpaused), playerName2));
				}
				SendDataToClient();
			}
		}

		[Event(null, 352)]
		[Reliable]
		[Server]
		public static void ProgressMatchInternal()
		{
			if (!ValidatePlayer())
			{
				return;
			}
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component != null)
			{
				MyMatchState state = component.State;
				component.AdvanceToNextState();
				if (state != component.State)
				{
					string playerName = GetPlayerName(MyEventContext.Current.Sender.Value);
					SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Advanced), playerName, component.State.ToString()));
				}
				SendDataToClient();
			}
		}

		[Event(null, 374)]
		[Reliable]
		[Server]
		public static void SetTimeInternal(float value)
		{
			if (!ValidatePlayer())
			{
				return;
			}
			if (value < 0f)
			{
				value = 0f;
			}
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component != null)
			{
				float remainingMinutes = component.RemainingMinutes;
				component.SetRemainingTime(value);
				if (component.RemainingMinutes != remainingMinutes)
				{
					string playerName = GetPlayerName(MyEventContext.Current.Sender.Value);
					SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_SetTime), playerName, value));
				}
				SendDataToClient();
			}
		}

		[Event(null, 398)]
		[Reliable]
		[Server]
		public static void AddTimeInternal(float value)
		{
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component != null)
			{
				float remainingMinutes = component.RemainingMinutes;
				component.AddRemainingTime(value);
				if (component.RemainingMinutes != remainingMinutes)
				{
					string playerName = GetPlayerName(MyEventContext.Current.Sender.Value);
					SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_AddTime), playerName, value));
				}
				SendDataToClient();
			}
		}

		private static string GetPlayerName(ulong steamId)
		{
			long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			if (myIdentity == null)
			{
				return string.Empty;
			}
			return myIdentity.DisplayName;
		}

		private static void SendGlobalMessage(string message)
		{
			MyVisualScriptLogicProvider.SendChatMessage(message, "", 0L);
		}

		private void SetTextRed()
		{
			m_textboxTime.ColorMask = MyTerminalFactionController.COLOR_CUSTOM_RED;
		}

		private void ClearTextColor()
		{
			m_textboxTime.ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY;
		}

		/// <summary>
		/// Use only on RPC calls.
		/// </summary>
		private static bool ValidatePlayer()
		{
			ulong value = MyEventContext.Current.Sender.Value;
			return MySession.Static.IsUserAdmin(value);
		}

		protected void UpdateMatch()
		{
			UpdateTime();
		}

		private void UpdateTime()
		{
			if (m_currentPage == MyPageEnum.Match)
			{
				if (!m_isMatchEnabled || !m_isMatchRunning)
				{
					m_matchLastUpdateTime = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
				}
				MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
				MyTimeSpan myTimeSpan2 = myTimeSpan - m_matchLastUpdateTime;
				m_matchRemainingTime -= myTimeSpan2;
				if (m_matchRemainingTime < MyTimeSpan.Zero)
				{
					SyncData();
					m_matchRemainingTime = MyTimeSpan.Zero;
				}
				UpdateTimeText();
				m_matchLastUpdateTime = myTimeSpan;
			}
		}

		private void UpdateTimeText()
		{
			if (m_labelTime != null)
			{
				m_labelTime.Text = string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_TimeText), FormatTime(m_matchRemainingTime));
			}
		}

		private string FormatTime(MyTimeSpan remainingTime)
		{
			StringBuilder stringBuilder = new StringBuilder();
			MyValueFormatter.AppendTimeExactHoursMinSec((int)remainingTime.Seconds, stringBuilder);
			return stringBuilder.ToString();
		}

		private void RecreateReplayToolControls(ref Vector2 controlPadding, float separatorSize, float usableWidth)
		{
			m_recreateInProgress = true;
			m_currentPosition.Y += 0.03f;
			if (!MySession.Static.IsServer)
			{
				MyGuiControlButton myGuiControlButton = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorld, ReloadWorld, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorldClient_Tooltip);
				myGuiControlButton.Enabled = false;
				myGuiControlButton.ShowTooltipWhenDisabled = true;
			}
			else
			{
				CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorld, ReloadWorld, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorld_Tooltip);
			}
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = "",
				IsAutoScaleEnabled = true,
				IsAutoEllipsisEnabled = true
			};
<<<<<<< HEAD
=======
			m_labelEnabled.SetMaxWidth(0.3f);
			Controls.Add(m_labelEnabled);
			m_currentPosition.Y += 0.025f;
			m_labelRunning = new MyGuiControlLabel
			{
				Position = m_currentPosition + vector,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = "",
				IsAutoScaleEnabled = true,
				IsAutoEllipsisEnabled = true
			};
			m_labelRunning.SetMaxWidth(0.3f);
			Controls.Add(m_labelRunning);
			m_currentPosition.Y += 0.025f;
			m_labelState = new MyGuiControlLabel
			{
				Position = m_currentPosition + vector,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = "",
				IsAutoScaleEnabled = true,
				IsAutoEllipsisEnabled = true
			};
			m_labelState.SetMaxWidth(0.3f);
			Controls.Add(m_labelState);
			m_currentPosition.Y += 0.025f;
			m_labelTime = new MyGuiControlLabel
			{
				Position = m_currentPosition + vector,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = "",
				IsAutoScaleEnabled = true,
				IsAutoEllipsisEnabled = true
			};
			m_labelTime.SetMaxWidth(0.3f);
			Controls.Add(m_labelTime);
			m_currentPosition.Y += 0.1f;
			m_buttonStart = CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_Start, StartMatch, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Match_Start_Tooltip);
			m_currentPosition.Y += 0.002f;
			if (MyFakes.SHOW_MATCH_STOP)
			{
				m_buttonStop = CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_Stop, StopMatch, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Match_Stop_Tooltip);
				m_currentPosition.Y += 0.002f;
			}
			m_buttonPause = CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_Pause, PauseMatch, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Match_Pause_Tooltip);
			m_currentPosition.Y += 0.002f;
			m_buttonAdvanced = CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_Advance, ProgressMatch, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Match_Advance_Tooltip);
			UpdatePauseButtonTexts();
			m_currentPosition.Y += 0.04f;
			m_textboxTime = new MyGuiControlTextbox(null, "0", 512, null, 0.8f, MyGuiControlTextboxType.DigitsOnly);
			m_textboxTime.TextAlignment = TextAlingmentMode.Right;
			m_textboxTime.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			m_textboxTime.Position = m_currentPosition + new Vector2(num - HIDDEN_PART_RIGHT / 2f, 0f);
			m_textboxTime.Size = new Vector2(num, m_textboxTime.Size.Y);
			m_textboxTime.TextChanged += MatchTimeTextbox_Changed;
			Controls.Add(m_textboxTime);
			m_currentPosition.Y += 0.035f;
			m_buttonSetTime = CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_SetTime, SetTime, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Match_SetTime_Tooltip);
			m_currentPosition.Y += 0.002f;
			m_buttonAddTime = CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_AddTime, AddTime, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Match_AddTime_Tooltip);
			SyncData();
		}

		private void MatchTimeTextbox_Changed(MyGuiControlTextbox obj)
		{
			ClearTextColor();
		}

		private void SyncData()
		{
			m_matchSyncReceivers.Add(this);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SendDataToClient);
		}

		[Event(null, 146)]
		[Reliable]
		[Server]
		public static void SendDataToClient()
		{
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component != null)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ReciveClientData, component.State, component.RemainingMinutes, component.IsRunning, component.IsEnabled, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 156)]
		[Reliable]
		[Client]
		public static void ReciveClientData(MyMatchState state, float remainingTime, bool isRunning, bool isEnabled)
		{
			foreach (MyGuiScreenAdminMenu matchSyncReceiver in m_matchSyncReceivers)
			{
				matchSyncReceiver.SetSyncData(state, remainingTime, isRunning, isEnabled);
			}
			m_matchSyncReceivers.Clear();
		}

		private void SetSyncData(MyMatchState state, float remainingTime, bool isRunning, bool isEnabled)
		{
			m_matchCurrentState = state;
			m_matchRemainingTime = MyTimeSpan.FromMinutes(remainingTime);
			m_isMatchRunning = isRunning;
			m_isMatchEnabled = isEnabled;
			m_matchLastUpdateTime = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			ResetStateTexts();
			UpdatePauseButtonTexts();
		}

		private void ResetStateTexts()
		{
			if (m_isMatchEnabled)
			{
				m_labelEnabled.Text = string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_EnableText));
			}
			else
			{
				m_labelEnabled.Text = string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_DisableText));
			}
			if (m_isMatchRunning)
			{
				m_labelRunning.Text = string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_RunningText));
			}
			else
			{
				m_labelRunning.Text = string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_NotRunningText));
			}
			m_labelState.Text = string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_StateText), m_matchCurrentState.ToString());
			UpdateTimeText();
		}

		private void UpdatePauseButtonTexts()
		{
			if (m_isMatchRunning)
			{
				m_buttonPause.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_Pause);
				m_buttonPause.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_Match_Pause_Tooltip);
			}
			else
			{
				m_buttonPause.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_Unpause);
				m_buttonPause.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_Match_Unpause_Tooltip);
			}
		}

		private void StartMatch(MyGuiControlButton obj)
		{
			m_matchSyncReceivers.Add(this);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => StartMatchInternal);
		}

		private void StopMatch(MyGuiControlButton obj)
		{
			m_matchSyncReceivers.Add(this);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => StopMatchInternal);
		}

		private void PauseMatch(MyGuiControlButton obj)
		{
			m_matchSyncReceivers.Add(this);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => PauseMatchInternal);
		}

		private void ProgressMatch(MyGuiControlButton obj)
		{
			m_matchSyncReceivers.Add(this);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ProgressMatchInternal);
		}

		private void SetTime(MyGuiControlButton obj)
		{
			if (!float.TryParse(m_textboxTime.Text, out var result))
			{
				SetTextRed();
				return;
			}
			if (result < 0f)
			{
				result = 0f;
			}
			m_matchSyncReceivers.Add(this);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetTimeInternal, result);
			ClearTextColor();
		}

		private void AddTime(MyGuiControlButton obj)
		{
			if (float.TryParse(m_textboxTime.Text, out var result))
			{
				m_matchSyncReceivers.Add(this);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AddTimeInternal, result);
			}
		}

		[Event(null, 262)]
		[Reliable]
		[Server]
		public static void StartMatchInternal()
		{
			if (!ValidatePlayer())
			{
				return;
			}
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component != null)
			{
				component.ResetToFirstState();
				if (component.State == MyMatchState.PreMatch)
				{
					string playerName = GetPlayerName(MyEventContext.Current.Sender.Value);
					SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Started), playerName));
				}
				SendDataToClient();
			}
		}

		[Event(null, 282)]
		[Reliable]
		[Server]
		public static void StopMatchInternal()
		{
			if (!ValidatePlayer())
			{
				return;
			}
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component == null)
			{
				return;
			}
			int num = LOOP_LIMIT;
			bool flag = true;
			while (component.State != MyMatchState.Finished)
			{
				MyMatchState state = component.State;
				component.AdvanceToNextState();
				num = ((state != component.State) ? LOOP_LIMIT : (num - 1));
				if (num < 0)
				{
					flag = false;
					break;
				}
			}
			if (flag && component.State == MyMatchState.Finished)
			{
				string playerName = GetPlayerName(MyEventContext.Current.Sender.Value);
				SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Stopped), playerName));
			}
			else
			{
				SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_StopFailed)));
			}
			SendDataToClient();
		}

		[Event(null, 324)]
		[Reliable]
		[Server]
		public static void PauseMatchInternal()
		{
			if (!ValidatePlayer())
			{
				return;
			}
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component != null)
			{
				bool isRunning = component.IsRunning;
				component.SetIsRunning(!isRunning);
				if (isRunning)
				{
					string playerName = GetPlayerName(MyEventContext.Current.Sender.Value);
					SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Paused), playerName));
				}
				else
				{
					string playerName2 = GetPlayerName(MyEventContext.Current.Sender.Value);
					SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Unpaused), playerName2));
				}
				SendDataToClient();
			}
		}

		[Event(null, 352)]
		[Reliable]
		[Server]
		public static void ProgressMatchInternal()
		{
			if (!ValidatePlayer())
			{
				return;
			}
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component != null)
			{
				MyMatchState state = component.State;
				component.AdvanceToNextState();
				if (state != component.State)
				{
					string playerName = GetPlayerName(MyEventContext.Current.Sender.Value);
					SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Advanced), playerName, component.State.ToString()));
				}
				SendDataToClient();
			}
		}

		[Event(null, 374)]
		[Reliable]
		[Server]
		public static void SetTimeInternal(float value)
		{
			if (!ValidatePlayer())
			{
				return;
			}
			if (value < 0f)
			{
				value = 0f;
			}
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component != null)
			{
				float remainingMinutes = component.RemainingMinutes;
				component.SetRemainingTime(value);
				if (component.RemainingMinutes != remainingMinutes)
				{
					string playerName = GetPlayerName(MyEventContext.Current.Sender.Value);
					SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_SetTime), playerName, value));
				}
				SendDataToClient();
			}
		}

		[Event(null, 398)]
		[Reliable]
		[Server]
		public static void AddTimeInternal(float value)
		{
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component != null)
			{
				float remainingMinutes = component.RemainingMinutes;
				component.AddRemainingTime(value);
				if (component.RemainingMinutes != remainingMinutes)
				{
					string playerName = GetPlayerName(MyEventContext.Current.Sender.Value);
					SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_AddTime), playerName, value));
				}
				SendDataToClient();
			}
		}

		private static string GetPlayerName(ulong steamId)
		{
			long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			if (myIdentity == null)
			{
				return string.Empty;
			}
			return myIdentity.DisplayName;
		}

		private static void SendGlobalMessage(string message)
		{
			MyVisualScriptLogicProvider.SendChatMessage(message, "", 0L);
		}

		private void SetTextRed()
		{
			m_textboxTime.ColorMask = MyTerminalFactionController.COLOR_CUSTOM_RED;
		}

		private void ClearTextColor()
		{
			m_textboxTime.ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY;
		}

		private static bool ValidatePlayer()
		{
			ulong value = MyEventContext.Current.Sender.Value;
			return MySession.Static.IsUserAdmin(value);
		}

		protected void UpdateMatch()
		{
			UpdateTime();
		}

		private void UpdateTime()
		{
			if (m_currentPage == MyPageEnum.Match)
			{
				_ = m_matchRemainingTime;
				if (!m_isMatchEnabled || !m_isMatchRunning)
				{
					m_matchLastUpdateTime = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
				}
				MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
				MyTimeSpan myTimeSpan2 = myTimeSpan - m_matchLastUpdateTime;
				m_matchRemainingTime -= myTimeSpan2;
				if (m_matchRemainingTime < MyTimeSpan.Zero)
				{
					SyncData();
					m_matchRemainingTime = MyTimeSpan.Zero;
				}
				UpdateTimeText();
				m_matchLastUpdateTime = myTimeSpan;
			}
		}

		private void UpdateTimeText()
		{
			if (m_labelTime != null)
			{
				m_labelTime.Text = string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_TimeText), FormatTime(m_matchRemainingTime));
			}
		}

		private string FormatTime(MyTimeSpan remainingTime)
		{
			StringBuilder stringBuilder = new StringBuilder();
			MyValueFormatter.AppendTimeExactHoursMinSec((int)remainingTime.Seconds, stringBuilder);
			return stringBuilder.ToString();
		}

		private void RecreateReplayToolControls(ref Vector2 controlPadding, float separatorSize, float usableWidth)
		{
			m_recreateInProgress = true;
			m_currentPosition.Y += 0.03f;
			if (!MySession.Static.IsServer)
			{
				MyGuiControlButton myGuiControlButton = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorld, ReloadWorld, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorldClient_Tooltip);
				myGuiControlButton.Enabled = false;
				myGuiControlButton.ShowTooltipWhenDisabled = true;
			}
			else
			{
				CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorld, ReloadWorld, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorld_Tooltip);
			}
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ManageCharacters)
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Controls.Add(control);
			m_currentPosition.Y += 0.03f;
			Vector2 currentPosition = m_currentPosition;
			m_buttonXOffset -= 0.075f;
			CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_AddCharacter, AddCharacter, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_AddCharacter_Tooltip, increaseSpacing: true, addToControls: true, isAutoScaleEnabled: true);
			m_currentPosition.Y = currentPosition.Y;
			m_buttonXOffset += 0.15f;
			CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_RemoveCharacter, TryRemoveCurrentCharacter, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_RemoveCharacter_Tooltip, increaseSpacing: true, addToControls: true, isAutoScaleEnabled: true);
			m_buttonXOffset = 0f;
			CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ChangeCharacter, TryChangeCurrentCharacter, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ChangeCharacter_Tooltip, increaseSpacing: true, addToControls: true, isAutoScaleEnabled: true);
			MyGuiControlLabel control2 = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ManageRecordings)
			};
			Controls.Add(control2);
			m_currentPosition.Y += 0.03f;
			if (MySessionComponentReplay.Static.IsReplaying)
			{
				CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_StopReplay, OnReplayButtonPressed, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_StopReplay_Tooltip);
			}
			else
			{
				CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Replay, OnReplayButtonPressed, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Replay_Tooltip);
			}
			if (MySessionComponentReplay.Static.IsRecording)
			{
				CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_StopRecording, OnRecordButtonPressed, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_StopRecording_Tooltip);
			}
			else
			{
				CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_RecordAndReplay, OnRecordButtonPressed, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_RecordAndReplay_Tooltip, increaseSpacing: true, addToControls: true, isAutoScaleEnabled: true);
			}
			CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_DeleteRecordings, DeleteRecordings, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_DeleteRecordings_Tooltip, increaseSpacing: true, addToControls: true, isAutoScaleEnabled: true);
			m_currentPosition.Y += 0.02f;
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				Size = new Vector2(0.3f, 0.6f),
				Font = "Blue",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			myGuiControlMultilineText.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_0));
			myGuiControlMultilineText.AppendLine();
			myGuiControlMultilineText.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_1));
			myGuiControlMultilineText.AppendLine();
			myGuiControlMultilineText.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_2));
			myGuiControlMultilineText.AppendLine();
			myGuiControlMultilineText.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_3));
			myGuiControlMultilineText.AppendLine();
			myGuiControlMultilineText.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_4));
			myGuiControlMultilineText.AppendLine();
			myGuiControlMultilineText.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_5));
			myGuiControlMultilineText.AppendLine();
			myGuiControlMultilineText.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_6));
			myGuiControlMultilineText.AppendLine();
			myGuiControlMultilineText.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_7));
			myGuiControlMultilineText.AppendLine();
			myGuiControlMultilineText.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_8));
			myGuiControlMultilineText.AppendLine();
			myGuiControlMultilineText.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_9));
			Controls.Add(myGuiControlMultilineText);
		}

		private void ReloadWorld(MyGuiControlButton obj)
		{
			if (Directory.Exists(MySession.Static.CurrentPath))
			{
				MyGuiScreenGamePlay.Static.ShowLoadMessageBox(MySession.Static.CurrentPath);
			}
		}

		private void AddCharacter(MyGuiControlButton obj)
		{
			MyCharacterInputComponent.SpawnCharacter();
		}

		private void TryRemoveCurrentCharacter(MyGuiControlButton obj)
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null)
			{
				SelectNextCharacter();
				if (MySession.Static.ControlledEntity != controlledEntity)
				{
					controlledEntity.Entity.Close();
				}
			}
		}

		private void TryChangeCurrentCharacter(MyGuiControlButton obj)
		{
			MyGuiScreenGamePlay.SetSpectatorNone();
		}

		/// <summary>
		/// Cycle to the next controllable character.
		/// </summary>
		private void SelectNextCharacter()
		{
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			MyCameraControllerEnum cameraControllerEnum = MySession.Static.GetCameraControllerEnum();
			if (cameraControllerEnum == MyCameraControllerEnum.Entity || cameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator)
			{
				if (MySession.Static.VirtualClients.Any() && Sync.Clients.LocalClient != null)
				{
					MyPlayer myPlayer = MySession.Static.VirtualClients.GetNextControlledPlayer(MySession.Static.LocalHumanPlayer) ?? Sync.Clients.LocalClient.GetPlayer(0);
					if (myPlayer != null)
					{
						Sync.Clients.LocalClient.ControlledPlayerSerialId = myPlayer.Id.SerialId;
					}
				}
				else
				{
					long identityId = MySession.Static.LocalHumanPlayer.Identity.IdentityId;
					List<MyEntity> list = new List<MyEntity>();
					foreach (MyEntity entity in MyEntities.GetEntities())
					{
						MyCharacter myCharacter = entity as MyCharacter;
						if (myCharacter != null && !myCharacter.IsDead && myCharacter.GetIdentity() != null && myCharacter.GetIdentity().IdentityId == identityId)
						{
							list.Add(entity);
						}
						MyCubeGrid myCubeGrid = entity as MyCubeGrid;
						if (myCubeGrid == null)
						{
							continue;
						}
<<<<<<< HEAD
						foreach (MySlimBlock block in myCubeGrid.GetBlocks())
						{
							MyCockpit myCockpit = block.FatBlock as MyCockpit;
							if (myCockpit != null && myCockpit.Pilot != null && myCockpit.Pilot.GetIdentity() != null && myCockpit.Pilot.GetIdentity().IdentityId == identityId)
							{
								list.Add(myCockpit);
=======
						Enumerator<MySlimBlock> enumerator = myCubeGrid.GetBlocks().GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								MyCockpit myCockpit = enumerator.get_Current().FatBlock as MyCockpit;
								if (myCockpit != null && myCockpit.Pilot != null && myCockpit.Pilot.GetIdentity() != null && myCockpit.Pilot.GetIdentity().IdentityId == identityId)
								{
									list.Add(myCockpit);
								}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
						}
						finally
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					int num = list.IndexOf(MySession.Static.ControlledEntity.Entity);
					List<MyEntity> list2 = new List<MyEntity>();
					if (num + 1 < list.Count)
					{
						list2.AddRange(list.GetRange(num + 1, list.Count - num - 1));
					}
					if (num != -1)
					{
						list2.AddRange(list.GetRange(0, num + 1));
					}
					IMyControllableEntity myControllableEntity = null;
					for (int i = 0; i < list2.Count; i++)
					{
						if (list2[i] is IMyControllableEntity)
						{
							myControllableEntity = list2[i] as IMyControllableEntity;
							break;
						}
					}
					if (MySession.Static.LocalHumanPlayer != null && myControllableEntity != null)
					{
						MySession.Static.LocalHumanPlayer.Controller.TakeControl(myControllableEntity);
						MyCharacter myCharacter2 = MySession.Static.ControlledEntity as MyCharacter;
						if (myCharacter2 == null && MySession.Static.ControlledEntity is MyCockpit)
						{
							myCharacter2 = (MySession.Static.ControlledEntity as MyCockpit).Pilot;
						}
						if (myCharacter2 != null)
						{
							MySession.Static.LocalHumanPlayer.Identity.ChangeCharacter(myCharacter2);
						}
					}
				}
			}
			if (!(MySession.Static.ControlledEntity is MyCharacter))
			{
				MySession.Static.GameFocusManager.Clear();
			}
		}

		private void OnReplayButtonPressed(MyGuiControlButton obj)
		{
			if (MySessionComponentReplay.Static == null)
			{
				return;
			}
			if (!MySessionComponentReplay.Static.IsReplaying)
			{
				if (MySessionComponentReplay.Static.HasRecordedData)
				{
					MySessionComponentReplay.Static.StartReplay();
					CloseScreen();
				}
			}
			else
			{
				MySessionComponentReplay.Static.StopReplay();
				RecreateControls(constructor: false);
			}
		}

		private void OnRecordButtonPressed(MyGuiControlButton obj)
		{
			if (MySessionComponentReplay.Static != null)
			{
				if (!MySessionComponentReplay.Static.IsRecording)
				{
					MySessionComponentReplay.Static.StartRecording();
					MySessionComponentReplay.Static.StartReplay();
					CloseScreen();
				}
				else
				{
					MySessionComponentReplay.Static.StopRecording();
					MySessionComponentReplay.Static.StopReplay();
					RecreateControls(constructor: false);
				}
			}
		}

		private void DeleteRecordings(MyGuiControlButton obj)
		{
			MySessionComponentReplay.Static.DeleteRecordings();
		}

		private void RecreateSafeZonesControls(ref Vector2 controlPadding, float separatorSize, float usableWidth)
		{
			m_recreateInProgress = true;
			m_currentPosition.Y += 0.015f;
			m_selectSafeZoneLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_SelectSafeZone)
			};
			Controls.Add(m_selectSafeZoneLabel);
			m_currentPosition.Y += 0.03f;
			m_safeZonesCombo = AddCombo();
			m_currentPosition.Y += 0.001f;
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = Vector2.Zero,
				Size = new Vector2(0.32f, 0.95f)
			};
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2((0f - m_size.Value.X) * 0.83f / 2f, m_currentPosition.Y), m_size.Value.X * 0.73f);
			Controls.Add(myGuiControlSeparatorList);
			m_currentPosition.Y += 0.005f;
			m_optionsGroup = new MyGuiControlScrollablePanel(myGuiControlParent)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = m_currentPosition,
				Size = new Vector2(0.32f, 0.62f)
			};
			m_optionsGroup.ScrollbarVEnabled = true;
			m_optionsGroup.ScrollBarOffset = new Vector2(-0.01f, 0f);
			Controls.Add(m_optionsGroup);
			Vector2 vector = -myGuiControlParent.Size * 0.5f;
			m_selectZoneShapeLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.SafeZone_SelectZoneShape)
			};
			myGuiControlParent.Controls.Add(m_selectZoneShapeLabel);
			vector.Y += 0.03f;
			m_safeZonesTypeCombo = AddCombo(null, null, null, 10, addToControls: false, vector);
			vector.Y += m_safeZonesTypeCombo.Size.Y + 0.01f + Spacing;
			m_safeZonesTypeCombo.AddItem(0L, MyTexts.GetString(MySpaceTexts.SafeZone_Spherical));
			m_safeZonesTypeCombo.AddItem(1L, MyTexts.GetString(MySpaceTexts.SafeZone_Cubical));
			myGuiControlParent.Controls.Add(m_safeZonesTypeCombo);
			vector.Y += 0.001f;
			m_zoneRadiusLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ZoneRadius)
			};
			myGuiControlParent.Controls.Add(m_zoneRadiusLabel);
			m_zoneRadiusLabel.Visible = false;
			m_zoneRadiusValueLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.285f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Text = "1"
			};
			myGuiControlParent.Controls.Add(m_zoneRadiusValueLabel);
			vector.Y += 0.03f;
			m_radiusSlider = new MyGuiControlSlider(vector, MySafeZone.MIN_RADIUS, MySafeZone.MAX_RADIUS, 0.285f, 1f, null, null, 1, 0.8f, 0f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_radiusSlider.Visible = false;
			MyGuiControlSlider radiusSlider = m_radiusSlider;
			radiusSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(radiusSlider.ValueChanged, new Action<MyGuiControlSlider>(OnRadiusChange));
			myGuiControlParent.Controls.Add(m_radiusSlider);
			vector.Y -= 0.03f;
			m_selectAxisLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.SafeZone_CubeAxis)
			};
			myGuiControlParent.Controls.Add(m_selectAxisLabel);
			m_zoneSizeLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.09f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.Size)
			};
			myGuiControlParent.Controls.Add(m_zoneSizeLabel);
			vector.Y += 0.03f;
			m_safeZonesAxisCombo = AddCombo(null, null, null, 10, addToControls: false, vector);
			vector.Y += m_safeZonesAxisCombo.Size.Y + 0.01f + Spacing;
			m_safeZonesAxisCombo.Size = new Vector2(0.08f, 1f);
			m_safeZonesAxisCombo.ItemSelected += m_safeZonesAxisCombo_ItemSelected;
			m_safeZonesAxisCombo.AddItem(0L, MyZoneAxisTypeEnum.X.ToString());
			m_safeZonesAxisCombo.AddItem(1L, MyZoneAxisTypeEnum.Y.ToString());
			m_safeZonesAxisCombo.AddItem(2L, MyZoneAxisTypeEnum.Z.ToString());
			m_safeZonesAxisCombo.SelectItemByIndex(0);
			myGuiControlParent.Controls.Add(m_safeZonesAxisCombo);
			m_sizeSlider = new MyGuiControlSlider(vector + new Vector2(0.09f, -0.05f), 20f, 500f, 0.195f, 1f, null, null, 1, 0.8f, 0f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			MyGuiControlSlider sizeSlider = m_sizeSlider;
			sizeSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sizeSlider.ValueChanged, new Action<MyGuiControlSlider>(OnSizeChange));
			myGuiControlParent.Controls.Add(m_sizeSlider);
			vector.Y += 0.018f;
			m_enabledCheckboxLabel = new MyGuiControlLabel
			{
				Position = vector + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ZoneEnabled)
			};
			m_enabledCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_enabledCheckbox.IsCheckedChanged = EnabledCheckedChanged;
			myGuiControlParent.Controls.Add(m_enabledCheckboxLabel);
			myGuiControlParent.Controls.Add(m_enabledCheckbox);
			vector.Y += 0.045f;
			m_damageCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDamage)
			};
			m_damageCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_damageCheckbox.IsCheckedChanged = DamageCheckChanged;
			myGuiControlParent.Controls.Add(m_damageCheckboxLabel);
			myGuiControlParent.Controls.Add(m_damageCheckbox);
			vector.Y += 0.045f;
			m_shootingCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowShooting)
			};
			m_shootingCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_shootingCheckbox.IsCheckedChanged = ShootingCheckChanged;
			myGuiControlParent.Controls.Add(m_shootingCheckboxLabel);
			myGuiControlParent.Controls.Add(m_shootingCheckbox);
			vector.Y += 0.045f;
			m_drillingCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDrilling)
			};
			m_drillingCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_drillingCheckbox.IsCheckedChanged = DrillingCheckChanged;
			myGuiControlParent.Controls.Add(m_drillingCheckboxLabel);
			myGuiControlParent.Controls.Add(m_drillingCheckbox);
			vector.Y += 0.045f;
			m_weldingCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowWelding)
			};
			m_weldingCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_weldingCheckbox.IsCheckedChanged = WeldingCheckChanged;
			myGuiControlParent.Controls.Add(m_weldingCheckboxLabel);
			myGuiControlParent.Controls.Add(m_weldingCheckbox);
			vector.Y += 0.045f;
			m_grindingCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowGrinding)
			};
			m_grindingCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_grindingCheckbox.IsCheckedChanged = GrindingCheckChanged;
			myGuiControlParent.Controls.Add(m_grindingCheckboxLabel);
			myGuiControlParent.Controls.Add(m_grindingCheckbox);
			vector.Y += 0.045f;
			m_buildingCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuilding)
			};
			m_buildingCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_buildingCheckbox.IsCheckedChanged = BuildingCheckChanged;
			myGuiControlParent.Controls.Add(m_buildingCheckboxLabel);
			myGuiControlParent.Controls.Add(m_buildingCheckbox);
			vector.Y += 0.045f;
			m_buildingProjectionsCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuildingProjections)
			};
			m_buildingProjectionsCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_buildingProjectionsCheckbox.IsCheckedChanged = BuildingProjectionsCheckChanged;
			myGuiControlParent.Controls.Add(m_buildingProjectionsCheckboxLabel);
			myGuiControlParent.Controls.Add(m_buildingProjectionsCheckbox);
			vector.Y += 0.045f;
			m_voxelHandCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowVoxelHands)
			};
			m_voxelHandCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_voxelHandCheckbox.IsCheckedChanged = VoxelHandCheckChanged;
			myGuiControlParent.Controls.Add(m_voxelHandCheckboxLabel);
			myGuiControlParent.Controls.Add(m_voxelHandCheckbox);
			vector.Y += 0.045f;
			m_landingGearLockCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowLandingGear),
				IsAutoScaleEnabled = true
			};
			m_landingGearLockCheckboxLabel.SetMaxWidth(0.25f);
			m_landingGearLockCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_landingGearLockCheckbox.IsCheckedChanged = OnSettingsCheckChanged;
			m_landingGearLockCheckbox.UserData = MySafeZoneAction.LandingGearLock;
			myGuiControlParent.Controls.Add(m_landingGearLockCheckboxLabel);
			myGuiControlParent.Controls.Add(m_landingGearLockCheckbox);
			vector.Y += 0.045f;
			m_convertToStationCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowConvertToStation)
			};
			m_convertToStationCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_convertToStationCheckbox.IsCheckedChanged = OnSettingsCheckChanged;
			m_convertToStationCheckbox.UserData = MySafeZoneAction.ConvertToStation;
			myGuiControlParent.Controls.Add(m_convertToStationCheckboxLabel);
			myGuiControlParent.Controls.Add(m_convertToStationCheckbox);
			vector.Y += 0.04f;
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenAdmin_Safezone_TextureColorLabel)
			};
			myGuiControlParent.Controls.Add(control);
			vector.Y += 0.03f;
			m_textureCombo = AddCombo(null, null, null, 10, addToControls: false, vector);
			IEnumerable<MySafeZoneTexturesDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MySafeZoneTexturesDefinition>();
			if (allDefinitions != null)
			{
				foreach (MySafeZoneTexturesDefinition item in allDefinitions)
				{
					m_textureCombo.AddItem((int)item.DisplayTextId, MyStringId.GetOrCompute(item.DisplayTextId.String));
				}
			}
			else
			{
				MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
			}
			myGuiControlParent.Controls.Add(m_textureCombo);
			vector.Y += 0.055f;
			m_colorSelector = new MyGuiControlColor(MyTexts.GetString(MySpaceTexts.ScreenAdmin_Safezone_ColorLabel), 1f, vector, Color.SkyBlue, Color.Red, MyCommonTexts.DialogAmount_SetValueCaption, placeSlidersVertically: true);
			m_colorSelector.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_colorSelector.Size = new Vector2(0.285f, m_colorSelector.Size.Y);
			myGuiControlParent.Controls.Add(m_colorSelector);
			vector.Y += 0.17f;
			m_optionsGroup.RefreshInternals();
			m_currentPosition.Y += m_optionsGroup.Size.Y;
			m_currentPosition.Y += 0.005f;
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2((0f - m_size.Value.X) * 0.83f / 2f, m_currentPosition.Y), m_size.Value.X * 0.73f);
			Controls.Add(myGuiControlSeparatorList2);
			m_currentPosition.Y += 0.018f;
			float y = m_currentPosition.Y;
			m_addSafeZoneButton = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_SafeZones_NewSafeZone, delegate
			{
				OnAddSafeZone();
			}, enabled: true, null, increaseSpacing: true, addToControls: true, isAutoScaleEnabled: true, isAutoEllipsisEnabled: true);
			m_addSafeZoneButton.PositionX = -0.088f;
			m_currentPosition.Y = y;
			m_moveToSafeZoneButton = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_SafeZones_MoveToSafeZone, delegate
			{
				OnMoveToSafeZone();
			}, enabled: true, null, increaseSpacing: true, addToControls: true, isAutoScaleEnabled: true, isAutoEllipsisEnabled: true);
			m_moveToSafeZoneButton.PositionX = 0.055f;
			y = m_currentPosition.Y;
			m_repositionSafeZoneButton = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ChangePosition, delegate
			{
				OnRepositionSafeZone();
			});
			m_repositionSafeZoneButton.PositionX = -0.088f;
			m_currentPosition.Y = y;
			m_configureFilterButton = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ConfigureFilter, delegate
			{
				OnConfigureFilter();
			});
			m_configureFilterButton.PositionX = 0.055f;
			y = m_currentPosition.Y;
			m_removeSafeZoneButton = CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Remove, delegate
			{
				OnRemoveSafeZone();
			});
			m_removeSafeZoneButton.PositionX = -0.088f;
			m_currentPosition.Y = y;
			m_renameSafeZoneButton = CreateDebugButton(0.14f, MySpaceTexts.DetailScreen_Button_Rename, delegate
			{
				OnRenameSafeZone();
			});
			m_renameSafeZoneButton.PositionX = 0.055f;
			RefreshSafeZones();
			UpdateZoneType();
			UpdateSelectedData();
			m_safeZonesCombo.ItemSelected += m_safeZonesCombo_ItemSelected;
			m_safeZonesTypeCombo.ItemSelected += m_safeZonesTypeCombo_ItemSelected;
			m_textureCombo.ItemSelected += OnTextureSelected;
			m_colorSelector.OnChange += OnColorChanged;
			m_recreateInProgress = false;
		}

		private void OnColorChanged(MyGuiControlColor obj)
		{
			if (m_selectedSafeZone != null)
			{
				MyObjectBuilder_SafeZone obj2 = (MyObjectBuilder_SafeZone)m_selectedSafeZone.GetObjectBuilder();
				obj2.ModelColor = obj.GetColor().ToVector3();
				MySessionComponentSafeZones.RequestUpdateSafeZone(obj2);
			}
		}

		private void OnTextureSelected()
		{
			if (m_selectedSafeZone == null)
			{
				return;
			}
			IEnumerable<MySafeZoneTexturesDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MySafeZoneTexturesDefinition>();
			if (allDefinitions == null)
			{
				MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
				return;
			}
			MyObjectBuilder_SafeZone myObjectBuilder_SafeZone = (MyObjectBuilder_SafeZone)m_selectedSafeZone.GetObjectBuilder();
			MyStringHash myStringHash = MyStringHash.TryGet((int)m_textureCombo.GetSelectedKey());
			bool flag = false;
			foreach (MySafeZoneTexturesDefinition item in allDefinitions)
			{
				if (item.DisplayTextId == myStringHash)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				MyLog.Default.Error("Safe zone texture not found.");
				return;
			}
			myObjectBuilder_SafeZone.Texture = myStringHash.String;
			MySessionComponentSafeZones.RequestUpdateSafeZone(myObjectBuilder_SafeZone);
		}

		private void UpdateSelectedData()
		{
			m_recreateInProgress = true;
			bool enabled = m_selectedSafeZone != null;
			m_enabledCheckbox.Enabled = enabled;
			m_damageCheckbox.Enabled = enabled;
			m_shootingCheckbox.Enabled = enabled;
			m_drillingCheckbox.Enabled = enabled;
			m_weldingCheckbox.Enabled = enabled;
			m_grindingCheckbox.Enabled = enabled;
			m_voxelHandCheckbox.Enabled = enabled;
			m_buildingCheckbox.Enabled = enabled;
			m_buildingProjectionsCheckbox.Enabled = enabled;
			m_convertToStationCheckbox.Enabled = enabled;
			m_landingGearLockCheckbox.Enabled = enabled;
			m_radiusSlider.Enabled = enabled;
			m_renameSafeZoneButton.Enabled = enabled;
			m_removeSafeZoneButton.Enabled = enabled;
			m_repositionSafeZoneButton.Enabled = enabled;
			m_moveToSafeZoneButton.Enabled = enabled;
			m_configureFilterButton.Enabled = enabled;
			m_safeZonesCombo.Enabled = enabled;
			m_safeZonesTypeCombo.Enabled = enabled;
			m_safeZonesAxisCombo.Enabled = enabled;
			m_sizeSlider.Enabled = enabled;
			m_colorSelector.Enabled = enabled;
			m_textureCombo.Enabled = enabled;
			if (m_selectedSafeZone != null)
			{
				m_enabledCheckbox.IsChecked = m_selectedSafeZone.Enabled;
				if (m_selectedSafeZone.Shape == MySafeZoneShape.Sphere)
				{
					m_radiusSlider.Value = m_selectedSafeZone.Radius;
					m_zoneRadiusValueLabel.Text = m_selectedSafeZone.Radius.ToString();
				}
				else if (m_safeZonesAxisCombo.GetSelectedIndex() == 0)
				{
					m_sizeSlider.Value = m_selectedSafeZone.Size.X;
					m_zoneRadiusValueLabel.Text = m_selectedSafeZone.Size.X.ToString();
				}
				else if (m_safeZonesAxisCombo.GetSelectedIndex() == 1)
				{
					m_sizeSlider.Value = m_selectedSafeZone.Size.Y;
					m_zoneRadiusValueLabel.Text = m_selectedSafeZone.Size.Y.ToString();
				}
				else if (m_safeZonesAxisCombo.GetSelectedIndex() == 2)
				{
					m_sizeSlider.Value = m_selectedSafeZone.Size.Z;
					m_zoneRadiusValueLabel.Text = m_selectedSafeZone.Size.Z.ToString();
				}
				m_safeZonesTypeCombo.SelectItemByKey((long)m_selectedSafeZone.Shape);
				m_damageCheckbox.IsChecked = (m_selectedSafeZone.AllowedActions & MySafeZoneAction.Damage) > (MySafeZoneAction)0;
				m_shootingCheckbox.IsChecked = (m_selectedSafeZone.AllowedActions & MySafeZoneAction.Shooting) > (MySafeZoneAction)0;
				m_drillingCheckbox.IsChecked = (m_selectedSafeZone.AllowedActions & MySafeZoneAction.Drilling) > (MySafeZoneAction)0;
				m_weldingCheckbox.IsChecked = (m_selectedSafeZone.AllowedActions & MySafeZoneAction.Welding) > (MySafeZoneAction)0;
				m_grindingCheckbox.IsChecked = (m_selectedSafeZone.AllowedActions & MySafeZoneAction.Grinding) > (MySafeZoneAction)0;
				m_voxelHandCheckbox.IsChecked = (m_selectedSafeZone.AllowedActions & MySafeZoneAction.VoxelHand) > (MySafeZoneAction)0;
				m_buildingCheckbox.IsChecked = (m_selectedSafeZone.AllowedActions & MySafeZoneAction.Building) > (MySafeZoneAction)0;
				m_buildingProjectionsCheckbox.IsChecked = (m_selectedSafeZone.AllowedActions & MySafeZoneAction.BuildingProjections) > (MySafeZoneAction)0;
				m_landingGearLockCheckbox.IsChecked = (m_selectedSafeZone.AllowedActions & MySafeZoneAction.LandingGearLock) > (MySafeZoneAction)0;
				m_convertToStationCheckbox.IsChecked = (m_selectedSafeZone.AllowedActions & MySafeZoneAction.ConvertToStation) > (MySafeZoneAction)0;
				m_textureCombo.SelectItemByKey((int)m_selectedSafeZone.CurrentTexture);
				m_colorSelector.SetColor(m_selectedSafeZone.ModelColor);
			}
			m_recreateInProgress = false;
		}

		private void m_safeZonesTypeCombo_ItemSelected()
		{
			if (m_selectedSafeZone.Shape != (MySafeZoneShape)m_safeZonesTypeCombo.GetSelectedKey())
			{
				m_selectedSafeZone.Shape = (MySafeZoneShape)m_safeZonesTypeCombo.GetSelectedKey();
				m_selectedSafeZone.RecreatePhysics();
				UpdateZoneType();
				RequestUpdateSafeZone();
			}
		}

		private void UpdateZoneType()
		{
			m_zoneRadiusLabel.Visible = false;
			m_radiusSlider.Visible = false;
			m_selectAxisLabel.Visible = false;
			m_zoneSizeLabel.Visible = false;
			m_safeZonesAxisCombo.Visible = false;
			m_sizeSlider.Visible = false;
			if (m_selectedSafeZone == null || m_selectedSafeZone.Shape == MySafeZoneShape.Box)
			{
				m_selectAxisLabel.Visible = true;
				m_zoneSizeLabel.Visible = true;
				m_safeZonesAxisCombo.Visible = true;
				m_sizeSlider.Visible = true;
			}
			else if (m_selectedSafeZone.Shape == MySafeZoneShape.Sphere)
			{
				m_zoneRadiusLabel.Visible = true;
				m_radiusSlider.Visible = true;
			}
			UpdateSelectedData();
		}

		private void m_safeZonesAxisCombo_ItemSelected()
		{
			if (m_selectedSafeZone != null)
			{
				if (m_safeZonesAxisCombo.GetSelectedIndex() == 0)
				{
					m_zoneRadiusValueLabel.Text = m_selectedSafeZone.Size.X.ToString();
				}
				else if (m_safeZonesAxisCombo.GetSelectedIndex() == 1)
				{
					m_zoneRadiusValueLabel.Text = m_selectedSafeZone.Size.Y.ToString();
				}
				else if (m_safeZonesAxisCombo.GetSelectedIndex() == 2)
				{
					m_zoneRadiusValueLabel.Text = m_selectedSafeZone.Size.Z.ToString();
				}
				UpdateSelectedData();
			}
		}

		private void m_safeZonesCombo_ItemSelected()
		{
			m_selectedSafeZone = (MySafeZone)MyEntities.GetEntityById(m_safeZonesCombo.GetItemByIndex(m_safeZonesCombo.GetSelectedIndex()).Key);
			UpdateZoneType();
			UpdateSelectedData();
		}

		private void OnAddSafeZone()
		{
			MySessionComponentSafeZones.RequestCreateSafeZone(MySector.MainCamera.Position + 2f * MySector.MainCamera.ForwardVector);
		}

		private void OnRemoveSafeZone()
		{
			if (m_selectedSafeZone != null)
			{
				MySessionComponentSafeZones.RequestDeleteSafeZone(m_selectedSafeZone.EntityId);
				RequestUpdateSafeZone();
			}
		}

		private void OnRenameSafeZone()
		{
			if (m_selectedSafeZone == null)
			{
				return;
<<<<<<< HEAD
			}
			MyScreenManager.AddScreen(new MyGuiBlueprintTextDialog(new Vector2(0.5f, 0.5f), delegate(string result)
			{
				if (result != null)
				{
					m_selectedSafeZone.DisplayName = result;
					RequestUpdateSafeZone();
					RefreshSafeZones();
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SaveZoneRenameServer, m_selectedSafeZone.EntityId, result);
				}
			}, caption: MyTexts.GetString(MySpaceTexts.DetailScreen_Button_Rename), defaultName: m_selectedSafeZone.DisplayName, maxLenght: 50, textBoxWidth: 0.3f));
		}

		[Event(null, 807)]
		[Reliable]
		[Server]
		private static void SaveZoneRenameServer(long entityId, string newName)
		{
			if (!MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				return;
			}
			foreach (MySafeZone safeZone in MySessionComponentSafeZones.SafeZones)
			{
				if (safeZone.EntityId == entityId)
				{
					safeZone.DisplayName = newName;
					break;
				}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			MyScreenManager.AddScreen(new MyGuiBlueprintTextDialog(new Vector2(0.5f, 0.5f), delegate(string result)
			{
				if (result != null)
				{
					m_selectedSafeZone.DisplayName = result;
					RequestUpdateSafeZone();
					RefreshSafeZones();
				}
			}, "New Name", MyTexts.GetString(MySpaceTexts.DetailScreen_Button_Rename), 50, 0.3f));
		}

		private void OnConfigureFilter()
		{
			if (m_selectedSafeZone != null)
			{
				MySafeZone selectedSafeZone = m_selectedSafeZone;
				MyScreenManager.AddScreen(new MyGuiScreenSafeZoneFilter(new Vector2(0.5f, 0.5f), selectedSafeZone));
			}
		}

		private void OnMoveToSafeZone()
		{
			if (m_selectedSafeZone != null && MySession.Static.ControlledEntity != null)
			{
				MyMultiplayer.TeleportControlledEntity(m_selectedSafeZone.PositionComp.WorldMatrixRef.Translation);
			}
		}

		private void OnRepositionSafeZone()
		{
			if (m_selectedSafeZone != null)
			{
				m_selectedSafeZone.PositionComp.SetWorldMatrix(ref MySector.MainCamera.WorldMatrix);
				m_selectedSafeZone.RecreatePhysics();
				RequestUpdateSafeZone();
			}
		}

		private void MySafeZones_OnAddSafeZone(object sender, EventArgs e)
		{
			m_selectedSafeZone = (MySafeZone)sender;
			if (m_currentPage == MyPageEnum.SafeZones)
			{
				m_recreateInProgress = true;
				RefreshSafeZones();
				UpdateSelectedData();
				m_recreateInProgress = false;
			}
		}

		private void MySafeZones_OnRemoveSafeZone(object sender, EventArgs e)
		{
			if (m_safeZonesCombo != null)
			{
				if (m_selectedSafeZone == sender)
				{
					m_selectedSafeZone = null;
					RefreshSafeZones();
					m_selectedSafeZone = ((m_safeZonesCombo.GetItemsCount() > 0) ? ((MySafeZone)MyEntities.GetEntityById(m_safeZonesCombo.GetItemByIndex(m_safeZonesCombo.GetItemsCount() - 1).Key)) : null);
					m_recreateInProgress = true;
					UpdateSelectedData();
					m_recreateInProgress = false;
				}
				else
				{
					m_safeZonesCombo.RemoveItem(((MySafeZone)sender).EntityId);
				}
			}
		}

		private void RequestUpdateSafeZone()
		{
			if (m_selectedSafeZone != null)
			{
				MySessionComponentSafeZones.RequestUpdateSafeZone((MyObjectBuilder_SafeZone)m_selectedSafeZone.GetObjectBuilder());
			}
		}

		private void RefreshSafeZones()
		{
			m_safeZonesCombo.ClearItems();
			List<MySafeZone> list = Enumerable.ToList<MySafeZone>((IEnumerable<MySafeZone>)MySessionComponentSafeZones.SafeZones);
			list.Sort(new MySafezoneNameComparer());
			foreach (MySafeZone item in list)
			{
				if (item.SafeZoneBlockId == 0L)
				{
					m_safeZonesCombo.AddItem(item.EntityId, (item.DisplayName != null) ? item.DisplayName : item.ToString(), 1);
				}
			}
			if (m_selectedSafeZone == null)
			{
				m_selectedSafeZone = ((m_safeZonesCombo.GetItemsCount() > 0) ? ((MySafeZone)MyEntities.GetEntityById(m_safeZonesCombo.GetItemByIndex(m_safeZonesCombo.GetItemsCount() - 1).Key)) : null);
			}
			if (m_selectedSafeZone != null)
			{
				m_safeZonesCombo.SelectItemByKey(m_selectedSafeZone.EntityId);
			}
		}

		private void EnabledCheckedChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone == null || m_recreateInProgress)
			{
				return;
			}
			if (checkBox.IsChecked && MySessionComponentSafeZones.IsSafeZoneColliding(m_selectedSafeZone.EntityId, m_selectedSafeZone.WorldMatrix, m_selectedSafeZone.Shape, m_selectedSafeZone.Radius, m_selectedSafeZone.Size))
			{
				checkBox.IsChecked = false;
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), messageText: MyTexts.Get(MySpaceTexts.AdminScreen_Safezone_Collision)));
				return;
			}
			if (m_selectedSafeZone.Enabled != checkBox.IsChecked)
			{
				m_selectedSafeZone.Enabled = checkBox.IsChecked;
				m_selectedSafeZone.RefreshGraphics();
			}
			RequestUpdateSafeZone();
		}

		private void OnRadiusChange(MyGuiControlSlider slider)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (m_selectedSafeZone.Enabled && MySessionComponentSafeZones.IsSafeZoneColliding(m_selectedSafeZone.EntityId, m_selectedSafeZone.WorldMatrix, m_selectedSafeZone.Shape, slider.Value))
				{
					slider.Value = m_selectedSafeZone.Radius;
					return;
				}
				m_zoneRadiusValueLabel.Text = slider.Value.ToString();
				m_selectedSafeZone.Radius = slider.Value;
				m_selectedSafeZone.RecreatePhysics();
				RequestUpdateSafeZone();
			}
		}

		private void OnSizeChange(MyGuiControlSlider slider)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				Vector3 vector = Vector3.Zero;
				float value = 0f;
				if (m_safeZonesAxisCombo.GetSelectedIndex() == 0)
				{
					value = m_selectedSafeZone.Size.X;
					vector = new Vector3(slider.Value, m_selectedSafeZone.Size.Y, m_selectedSafeZone.Size.Z);
				}
				else if (m_safeZonesAxisCombo.GetSelectedIndex() == 1)
				{
					value = m_selectedSafeZone.Size.Y;
					vector = new Vector3(m_selectedSafeZone.Size.X, slider.Value, m_selectedSafeZone.Size.Z);
				}
				else if (m_safeZonesAxisCombo.GetSelectedIndex() == 2)
				{
					value = m_selectedSafeZone.Size.Z;
					vector = new Vector3(m_selectedSafeZone.Size.X, m_selectedSafeZone.Size.Y, slider.Value);
				}
				if (m_selectedSafeZone.Enabled && MySessionComponentSafeZones.IsSafeZoneColliding(m_selectedSafeZone.EntityId, m_selectedSafeZone.WorldMatrix, m_selectedSafeZone.Shape, 0f, vector))
				{
					slider.Value = value;
					return;
				}
				m_zoneRadiusValueLabel.Text = slider.Value.ToString();
				m_selectedSafeZone.Size = vector;
				m_selectedSafeZone.RecreatePhysics();
				RequestUpdateSafeZone();
			}
		}

		private void DamageCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Damage;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Damage;
				}
				RequestUpdateSafeZone();
			}
		}

		private void ShootingCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Shooting;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Shooting;
				}
				RequestUpdateSafeZone();
			}
		}

		private void DrillingCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Drilling;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Drilling;
				}
				RequestUpdateSafeZone();
			}
		}

		private void WeldingCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Welding;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Welding;
				}
				RequestUpdateSafeZone();
			}
		}

		private void GrindingCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Grinding;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Grinding;
				}
				RequestUpdateSafeZone();
			}
		}

		private void VoxelHandCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.VoxelHand;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.VoxelHand;
				}
				RequestUpdateSafeZone();
			}
		}

		private void OnSettingsCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= (MySafeZoneAction)checkBox.UserData;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~(MySafeZoneAction)checkBox.UserData;
				}
				RequestUpdateSafeZone();
			}
		}

		private void BuildingCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Building;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Building;
				}
				RequestUpdateSafeZone();
			}
		}

		private void BuildingProjectionsCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.BuildingProjections;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.BuildingProjections;
				}
				RequestUpdateSafeZone();
			}
		}

		private void RecreateSpectatorControls(bool constructor)
		{
			m_recreateInProgress = true;
			Vector2 vector = new Vector2(0.02f, 0.02f);
			float x = SCREEN_SIZE.X - HIDDEN_PART_RIGHT - vector.X * 2f;
			m_currentPosition.Y += 0.03f;
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = m_currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = "Camera Mode"
			};
			Controls.Add(control);
			m_currentPosition.Y += 0.03f;
			MySessionComponentSpectatorTools component = MySession.Static.GetComponent<MySessionComponentSpectatorTools>();
			m_cameraModeCombo = AddCombo();
			m_cameraModeCombo.AddItem(0L, MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_None));
			m_cameraModeCombo.AddItem(1L, MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Free));
			m_cameraModeCombo.AddItem(2L, MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Follow));
			m_cameraModeCombo.AddItem(3L, MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Orbit));
			m_cameraModeCombo.SelectItemByKey((long)component.GetMode());
			m_cameraModeCombo.ItemSelected += OnCameraModeSelected;
			m_currentPosition.Y += 0.015f;
			MyGuiControlLabel control2 = new MyGuiControlLabel
			{
				Position = m_currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_CameraSmoothness)
			};
			Controls.Add(control2);
			m_cameraSmoothnessValueLabel = new MyGuiControlLabel
			{
				Position = m_currentPosition + new Vector2(0.285f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP
			};
			Controls.Add(m_cameraSmoothnessValueLabel);
			m_currentPosition.Y += 0.03f;
			m_cameraSmoothness = new MyGuiControlSlider(m_currentPosition + new Vector2(0.001f, 0f));
			m_cameraSmoothness.Size = new Vector2(0.285f, 1f);
			m_cameraSmoothness.Value = component.SmoothCameraLERP;
			m_cameraSmoothness.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			MyGuiControlSlider cameraSmoothness = m_cameraSmoothness;
			cameraSmoothness.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(cameraSmoothness.ValueChanged, new Action<MyGuiControlSlider>(CameraSmoothnessChanged));
			m_cameraSmoothnessValueLabel.Text = component.SmoothCameraLERP.ToString("0.00#.##");
			Controls.Add(m_cameraSmoothness);
			m_currentPosition.Y += 0.03f;
			m_currentPosition.Y += 0.03f;
			MyGuiControlLabel control3 = new MyGuiControlLabel
			{
				Position = m_currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenSpectatorAdminMenu_SavedPositions)
			};
			Controls.Add(control3);
			m_currentPosition.Y += 0.03f;
			m_trackedListbox = new MyGuiControlListbox(Vector2.Zero, MyGuiControlListboxStyleEnum.Blueprints);
			m_trackedListbox.Size = new Vector2(x, 0f);
			m_trackedListbox.Enabled = true;
			m_trackedListbox.VisibleRowsCount = 10;
			m_trackedListbox.Position = m_trackedListbox.Size / 2f + m_currentPosition;
			m_trackedListbox.ItemClicked += OnListboxClicked;
			m_trackedListbox.MultiSelect = false;
			UpdateTrackedEntities();
			Controls.Add(m_trackedListbox);
			m_currentPosition.Y += 0.4f;
			MyGuiControlLabel control4 = new MyGuiControlLabel
			{
				Position = m_currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenSpectatorAdminMenu_Shortcuts)
			};
			Controls.Add(control4);
			m_currentPosition.Y += 0.03f;
			MyGuiControlLabel control5 = new MyGuiControlLabel
			{
				Position = m_currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = GetControlButtonNameWithDesc(MyControlsSpace.SPECTATOR_LOCK)
			};
			Controls.Add(control5);
			m_currentPosition.Y += 0.03f;
			MyGuiControlLabel control6 = new MyGuiControlLabel
			{
				Position = m_currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = GetControlButtonNameWithDesc(MyControlsSpace.SPECTATOR_NEXTPLAYER)
			};
			Controls.Add(control6);
			m_currentPosition.Y += 0.03f;
			MyGuiControlLabel control7 = new MyGuiControlLabel
			{
				Position = m_currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = GetControlButtonNameWithDesc(MyControlsSpace.SPECTATOR_PREVPLAYER)
			};
			Controls.Add(control7);
			m_currentPosition.Y += 0.03f;
			MyGuiControlLabel control8 = new MyGuiControlLabel
			{
				Position = m_currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = GetControlButtonNameWithDesc(MyControlsSpace.SPECTATOR_SWITCHMODE)
			};
			Controls.Add(control8);
			m_currentPosition.Y += 0.03f;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel
			{
				Position = m_currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenSpectatorAdminMenu_Save),
				IsAutoScaleEnabled = true,
				IsAutoEllipsisEnabled = true
			};
			myGuiControlLabel.SetMaxWidth(m_cameraSmoothness.Size.X);
			Controls.Add(myGuiControlLabel);
			m_currentPosition.Y += 0.03f;
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel
			{
				Position = m_currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenSpectatorAdminMenu_Load),
				IsAutoScaleEnabled = true,
				IsAutoEllipsisEnabled = true
			};
			myGuiControlLabel2.SetMaxWidth(m_cameraSmoothness.Size.X);
			Controls.Add(myGuiControlLabel2);
			m_currentPosition.Y += 0.05f;
		}

		public string GetControlButtonNameWithDesc(MyStringId control)
		{
			MyControl gameControl = MyInput.Static.GetGameControl(control);
			StringBuilder output = new StringBuilder();
			gameControl.AppendBoundButtonNames(ref output, ", ", MyInput.Static.GetUnassignedName());
			return output.ToString() + " - " + MyTexts.GetString(gameControl.GetControlName());
		}

		private void UpdateTrackedEntities()
		{
			MySessionComponentSpectatorTools component = MySession.Static.GetComponent<MySessionComponentSpectatorTools>();
<<<<<<< HEAD
			m_trackedListbox.Items.Clear();
=======
			((Collection<MyGuiControlListbox.Item>)(object)m_trackedListbox.Items).Clear();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			int num = 0;
			foreach (MyLockEntityState trackedSlot in component.TrackedSlots)
			{
				string text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Empty);
				if (trackedSlot.LockEntityID != -1)
				{
					text = trackedSlot.LockEntityDisplayName;
				}
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder("Num " + num + " - " + text), null, null, num++);
<<<<<<< HEAD
				m_trackedListbox.Items.Add(item);
=======
				((Collection<MyGuiControlListbox.Item>)(object)m_trackedListbox.Items).Add(item);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void OnListboxClicked(MyGuiControlListbox obj)
		{
			if (m_trackedListbox.SelectedItems.Count > 0)
			{
				MySessionComponentSpectatorTools component = MySession.Static.GetComponent<MySessionComponentSpectatorTools>();
				int slotIndex = (int)m_trackedListbox.SelectedItems[0].UserData;
				component.SelectTrackedSlot(slotIndex);
			}
		}

		private void OnCameraModeSelected()
		{
			MySessionComponentSpectatorTools component = MySession.Static.GetComponent<MySessionComponentSpectatorTools>();
			MyCameraMode mode = (MyCameraMode)m_cameraModeCombo.GetSelectedKey();
			component.SetMode(mode);
		}

		private void CameraSmoothnessChanged(MyGuiControlSlider slider)
		{
			MySession.Static.GetComponent<MySessionComponentSpectatorTools>().SmoothCameraLERP = m_cameraSmoothness.Value;
			m_cameraSmoothnessValueLabel.Text = m_cameraSmoothness.Value.ToString("0.00#.##");
		}

		private void RecreateWeatherControls(bool constructor)
		{
			MyGamePruningStructure.GetClosestPlanet(MySector.MainCamera.WorldMatrix.Translation);
			m_recreateInProgress = true;
			m_currentPosition.Y += 0.03f;
			CreateDebugButton(0.16f, MySpaceTexts.ScreenDebugAdminMenu_Weather_Generate, GenerateWeather, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Weather_Generate_Tooltip, increaseSpacing: true, addToControls: true, isAutoScaleEnabled: true);
			CreateDebugButton(0.16f, MySpaceTexts.ScreenDebugAdminMenu_Weather_Lightning, CreateLightning, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Weather_Lightning_Tooltip);
			m_currentPosition.Y += 0.05f;
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = m_currentPosition + new Vector2(0.135f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Weather_Modify)
			};
			Controls.Add(control);
			m_currentPosition.Y += 0.05f;
			m_weatherSelectionCombo = AddCombo(null, m_labelColor);
			m_weatherSelectionCombo.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_Weather_CreateCombo_Tooltip);
			m_weatherSelectionCombo.ItemSelected += M_weatherSelectionCombo_ItemSelected;
			m_weatherSelectionCombo.BorderColor = m_labelColor;
			m_weatherSelectionCombo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			m_weatherSelectionCombo.PositionX = -0.0225f;
			m_weatherSelectionCombo.Size = new Vector2(0.22f, 1f);
			PrepareWeatherDefinitions();
			m_currentPosition.Y -= 0.01f;
			CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_Weather_Create, CreateWeather, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Weather_Create_Tooltip);
			CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_Weather_Replace, ReplaceWeather, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Weather_Replace_Tooltip);
			CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_Weather_Remove, RemoveWeather, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_Weather_Remove_Tooltip);
			m_currentPosition.Y += 0.05f;
			messageMultiline = new MyGuiControlMultilineText
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				Size = new Vector2(0.7f, 0.6f),
				Font = "Blue",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Name = "weatherinfo"
			};
			Controls.Add(messageMultiline);
			UpdateWeatherInfo();
		}

		private void PrepareWeatherDefinitions()
		{
<<<<<<< HEAD
			m_definitions = (from x in MyDefinitionManager.Static.GetWeatherDefinitions()
				orderby x.Id.SubtypeName
				select x).ToArray();
=======
			m_definitions = Enumerable.ToArray<MyWeatherEffectDefinition>((IEnumerable<MyWeatherEffectDefinition>)Enumerable.OrderBy<MyWeatherEffectDefinition, string>((IEnumerable<MyWeatherEffectDefinition>)MyDefinitionManager.Static.GetWeatherDefinitions(), (Func<MyWeatherEffectDefinition, string>)((MyWeatherEffectDefinition x) => x.Id.SubtypeName)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_weatherSelectionCombo.ClearItems();
			m_weatherNamesAndSubtypesDictionary = new Dictionary<int, Tuple<string, string>>();
			for (int i = 0; i < m_definitions.Length; i++)
			{
				MyStringHash subtypeId = m_definitions[i].Id.SubtypeId;
				string text = subtypeId.ToString();
				string text2 = MyTexts.GetString(text);
				if (string.IsNullOrEmpty(text2))
				{
					text2 = text;
				}
				m_weatherSelectionCombo.AddItem(i, text2);
				m_weatherNamesAndSubtypesDictionary.Add(i, new Tuple<string, string>(text2, text));
			}
		}

		private void M_weatherSelectionCombo_ItemSelected()
		{
			int key = (int)m_weatherSelectionCombo.GetSelectedKey();
			m_selectedWeatherSubtypeId = "Clear";
			if (m_weatherNamesAndSubtypesDictionary.ContainsKey(key))
			{
				m_selectedWeatherSubtypeId = m_weatherNamesAndSubtypesDictionary[key].Item2;
			}
		}

		private void UpdateWeatherInfo()
		{
			if (m_weatherUpdateCounter-- != 0 || MySector.MainCamera == null)
			{
				return;
			}
			MySectorWeatherComponent component = MySession.Static.GetComponent<MySectorWeatherComponent>();
			if (component == null)
			{
				return;
			}
			component.GetWeather(MySector.MainCamera.Position, out var weatherEffect);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_Current), ": ", (weatherEffect != null) ? weatherEffect.Weather : "None", "\n"));
			stringBuilder.Append(string.Concat(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_CurrentIntensity), ": ", component.GetWeatherIntensity(MySector.MainCamera.Position), "\n"));
<<<<<<< HEAD
			MyStringHash orCompute = MyStringHash.GetOrCompute("environment_temperature_level");
			MyHudDefinition hudDefinition = MyHud.HudDefinition;
			string text = MyTexts.GetString("Temperature" + MySectorWeatherComponent.TemperatureToLevel(MySectorWeatherComponent.GetTemperatureInPoint(MySector.MainCamera.Position)));
			if (hudDefinition.StatControls != null)
			{
				MyObjectBuilder_StatControls[] statControls = hudDefinition.StatControls;
				for (int i = 0; i < statControls.Length; i++)
				{
					MyObjectBuilder_StatVisualStyle[] statStyles = statControls[i].StatStyles;
					foreach (MyObjectBuilder_StatVisualStyle myObjectBuilder_StatVisualStyle in statStyles)
					{
						MyObjectBuilder_TextStatVisualStyle myObjectBuilder_TextStatVisualStyle;
						if (myObjectBuilder_StatVisualStyle.StatId == orCompute && (myObjectBuilder_TextStatVisualStyle = myObjectBuilder_StatVisualStyle as MyObjectBuilder_TextStatVisualStyle) != null && myObjectBuilder_TextStatVisualStyle.VisibleCondition.Eval())
						{
							text = MyTexts.SubstituteTexts(myObjectBuilder_TextStatVisualStyle.Text);
							break;
						}
					}
				}
			}
			stringBuilder.Append(string.Concat(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_CurrentTemperature), ": ", text, "\n"));
=======
			stringBuilder.Append(string.Concat(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_CurrentTemperature), ": ", MySectorWeatherComponent.TemperatureToLevel(MySectorWeatherComponent.GetTemperatureInPoint(MySector.MainCamera.Position)).ToString(), "\n"));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (weatherEffect != null)
			{
				if (component.GetWeatherIntensity(MySector.MainCamera.Position) == 0f)
				{
					stringBuilder.Append(string.Concat(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_Incoming), ": ", weatherEffect.Weather, "\n"));
				}
				else
				{
					stringBuilder.Append(string.Concat(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_Incoming), ": None\n"));
				}
			}
			else
			{
				stringBuilder.Append(string.Concat(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_Incoming), ": None\n"));
			}
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(MySector.MainCamera.Position);
			if (closestPlanet != null)
			{
				foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetDatum in component.GetWeatherPlanetData())
				{
					if (closestPlanet.EntityId == weatherPlanetDatum.PlanetId && weatherPlanetDatum.NextWeather > 0)
					{
						stringBuilder.Append(string.Concat(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_Next), ": ", (int)Math.Round((float)weatherPlanetDatum.NextWeather * 0.0166666675f), "s \n"));
						break;
					}
				}
			}
			if (Controls.GetControlByName("weatherinfo") != null)
			{
				(Controls.GetControlByName("weatherinfo") as MyGuiControlMultilineText).Text = stringBuilder;
			}
			m_weatherUpdateCounter = 100;
		}

		private void GenerateWeather(MyGuiControlButton obj)
		{
			MySectorWeatherComponent component = MySession.Static.GetComponent<MySectorWeatherComponent>();
			if (component.GetWeather(MySector.MainCamera.Position, out var weatherEffect))
			{
				component.RemoveWeather(weatherEffect);
			}
			component.CreateRandomWeather(MyGamePruningStructure.GetClosestPlanet(MySector.MainCamera.Position));
			UpdateWeatherInfo();
		}

		private void CreateLightning(MyGuiControlButton obj)
		{
			MyAPIGateway.Physics.CastRay(MySector.MainCamera.WorldMatrix.Translation, MySector.MainCamera.WorldMatrix.Translation + MySector.MainCamera.WorldMatrix.Forward * 10000.0, out var hitInfo);
			if (hitInfo != null)
			{
				MyObjectBuilder_WeatherLightning lightning = new MyObjectBuilder_WeatherLightning();
				MySectorWeatherComponent component = MySession.Static.GetComponent<MySectorWeatherComponent>();
				string weather = component.GetWeather(MySector.MainCamera.WorldMatrix.Translation);
				if (weather != null)
				{
					MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather);
					if (weatherEffect != null && weatherEffect.Lightning != null)
					{
						lightning = weatherEffect.Lightning;
					}
				}
				component.CreateLightning(hitInfo.Position, lightning);
			}
			UpdateWeatherInfo();
		}

		private void CreateWeather(MyGuiControlButton obj)
		{
			if (m_selectedWeatherSubtypeId == null)
			{
				m_selectedWeatherSubtypeId = "Clear";
			}
			MySession.Static.GetComponent<MySectorWeatherComponent>().SetWeather(m_selectedWeatherSubtypeId, 0f, null, verbose: false, Vector3.Zero);
			UpdateWeatherInfo();
		}

		private void ReplaceWeather(MyGuiControlButton obj)
		{
			MySectorWeatherComponent component = MySession.Static.GetComponent<MySectorWeatherComponent>();
			if (m_selectedWeatherSubtypeId != null)
			{
				component.ReplaceWeather(m_selectedWeatherSubtypeId, null);
			}
		}

		private void RemoveWeather(MyGuiControlButton obj)
		{
			MySession.Static.GetComponent<MySectorWeatherComponent>().SetWeather("Clear", 0f, null, verbose: false, Vector3.Zero);
			UpdateWeatherInfo();
		}
	}
}
