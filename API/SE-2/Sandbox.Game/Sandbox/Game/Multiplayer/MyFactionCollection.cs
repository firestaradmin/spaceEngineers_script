using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ProtoBuf;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Definitions.Reputation;
using VRage.Game.Factions.Definitions;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions.Reputation;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
	[StaticEventOwner]
	public class MyFactionCollection : IEnumerable<KeyValuePair<long, MyFaction>>, IEnumerable, IMyFactionCollection
	{
		public enum MyFactionPeaceRequestState
		{
			None,
			Pending,
			Sent
		}

		public struct MyRelatablePair
		{
			public class ComparerType : IEqualityComparer<MyRelatablePair>
			{
				public bool Equals(MyRelatablePair x, MyRelatablePair y)
				{
					if (x.RelateeId1 != y.RelateeId1 || x.RelateeId2 != y.RelateeId2)
					{
						if (x.RelateeId1 == y.RelateeId2)
						{
							return x.RelateeId2 == y.RelateeId1;
						}
						return false;
					}
					return true;
				}

				public int GetHashCode(MyRelatablePair obj)
				{
					return obj.RelateeId1.GetHashCode() ^ obj.RelateeId2.GetHashCode();
				}
			}

			public long RelateeId1;

			public long RelateeId2;

			public static readonly ComparerType Comparer = new ComparerType();

			public MyRelatablePair(long id1, long id2)
			{
				RelateeId1 = id1;
				RelateeId2 = id2;
			}
		}

		[ProtoContract]
		public struct AddFactionMsg
		{
			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFounderId_003C_003EAccessor : IMemberAccessor<AddFactionMsg, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in long value)
				{
					owner.FounderId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out long value)
				{
					value = owner.FounderId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionId_003C_003EAccessor : IMemberAccessor<AddFactionMsg, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in long value)
				{
					owner.FactionId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out long value)
				{
					value = owner.FactionId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionTag_003C_003EAccessor : IMemberAccessor<AddFactionMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in string value)
				{
					owner.FactionTag = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out string value)
				{
					value = owner.FactionTag;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionName_003C_003EAccessor : IMemberAccessor<AddFactionMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in string value)
				{
					owner.FactionName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out string value)
				{
					value = owner.FactionName;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionDescription_003C_003EAccessor : IMemberAccessor<AddFactionMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in string value)
				{
					owner.FactionDescription = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out string value)
				{
					value = owner.FactionDescription;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionPrivateInfo_003C_003EAccessor : IMemberAccessor<AddFactionMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in string value)
				{
					owner.FactionPrivateInfo = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out string value)
				{
					value = owner.FactionPrivateInfo;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003ECreateFromDefinition_003C_003EAccessor : IMemberAccessor<AddFactionMsg, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in bool value)
				{
					owner.CreateFromDefinition = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out bool value)
				{
					value = owner.CreateFromDefinition;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EType_003C_003EAccessor : IMemberAccessor<AddFactionMsg, MyFactionTypes>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in MyFactionTypes value)
				{
					owner.Type = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out MyFactionTypes value)
				{
					value = owner.Type;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionColor_003C_003EAccessor : IMemberAccessor<AddFactionMsg, SerializableVector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in SerializableVector3 value)
				{
					owner.FactionColor = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out SerializableVector3 value)
				{
					value = owner.FactionColor;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionIconColor_003C_003EAccessor : IMemberAccessor<AddFactionMsg, SerializableVector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in SerializableVector3 value)
				{
					owner.FactionIconColor = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out SerializableVector3 value)
				{
					value = owner.FactionIconColor;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionIconGroupId_003C_003EAccessor : IMemberAccessor<AddFactionMsg, SerializableDefinitionId?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in SerializableDefinitionId? value)
				{
					owner.FactionIconGroupId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out SerializableDefinitionId? value)
				{
					value = owner.FactionIconGroupId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionIconId_003C_003EAccessor : IMemberAccessor<AddFactionMsg, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in int value)
				{
					owner.FactionIconId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out int value)
				{
					value = owner.FactionIconId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionScore_003C_003EAccessor : IMemberAccessor<AddFactionMsg, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in int value)
				{
					owner.FactionScore = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out int value)
				{
					value = owner.FactionScore;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionObjectivePercentageCompleted_003C_003EAccessor : IMemberAccessor<AddFactionMsg, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in float value)
				{
					owner.FactionObjectivePercentageCompleted = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out float value)
				{
					value = owner.FactionObjectivePercentageCompleted;
				}
			}

<<<<<<< HEAD
			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EisNPCFaction_003C_003EAccessor : IMemberAccessor<AddFactionMsg, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in bool value)
				{
					owner.isNPCFaction = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out bool value)
				{
					value = owner.isNPCFaction;
				}
			}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			private class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EActor : IActivator, IActivator<AddFactionMsg>
			{
				private sealed override object CreateInstance()
				{
					return default(AddFactionMsg);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override AddFactionMsg CreateInstance()
				{
					return (AddFactionMsg)(object)default(AddFactionMsg);
				}

				AddFactionMsg IActivator<AddFactionMsg>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public long FounderId;

			/// <summary>
			/// Do not use this for request, this value is always generated by server!
			/// </summary>
			[ProtoMember(4)]
			public long FactionId;

			[ProtoMember(7)]
			public string FactionTag;

			[ProtoMember(10)]
			public string FactionName;

			[Serialize(MyObjectFlags.DefaultZero)]
			[ProtoMember(13)]
			public string FactionDescription;

			[ProtoMember(16)]
			public string FactionPrivateInfo;

			[ProtoMember(19)]
			public bool CreateFromDefinition;

			[ProtoMember(22)]
			public MyFactionTypes Type;

			[ProtoMember(25)]
			public SerializableVector3 FactionColor;

			[ProtoMember(26)]
			public SerializableVector3 FactionIconColor;

			[ProtoMember(28)]
			public SerializableDefinitionId? FactionIconGroupId;

			[ProtoMember(31)]
			public int FactionIconId;

			[ProtoMember(35)]
			public int FactionScore;

			[ProtoMember(38)]
			public float FactionObjectivePercentageCompleted;
<<<<<<< HEAD

			[ProtoMember(41)]
			public bool isNPCFaction;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		[Serializable]
		public struct MyReputationChangeWrapper
		{
			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationChangeWrapper_003C_003EFactionId_003C_003EAccessor : IMemberAccessor<MyReputationChangeWrapper, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationChangeWrapper owner, in long value)
				{
					owner.FactionId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationChangeWrapper owner, out long value)
				{
					value = owner.FactionId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationChangeWrapper_003C_003ERepTotal_003C_003EAccessor : IMemberAccessor<MyReputationChangeWrapper, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationChangeWrapper owner, in int value)
				{
					owner.RepTotal = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationChangeWrapper owner, out int value)
				{
					value = owner.RepTotal;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationChangeWrapper_003C_003EChange_003C_003EAccessor : IMemberAccessor<MyReputationChangeWrapper, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationChangeWrapper owner, in int value)
				{
					owner.Change = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationChangeWrapper owner, out int value)
				{
					value = owner.Change;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationChangeWrapper_003C_003EShowNotification_003C_003EAccessor : IMemberAccessor<MyReputationChangeWrapper, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationChangeWrapper owner, in bool value)
				{
					owner.ShowNotification = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationChangeWrapper owner, out bool value)
				{
					value = owner.ShowNotification;
				}
			}

			public long FactionId;

			public int RepTotal;

			public int Change;

			public bool ShowNotification;
		}

		[Serializable]
		public struct MyReputationModifiers
		{
			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationModifiers_003C_003EOwner_003C_003EAccessor : IMemberAccessor<MyReputationModifiers, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationModifiers owner, in float value)
				{
					owner.Owner = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationModifiers owner, out float value)
				{
					value = owner.Owner;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationModifiers_003C_003EFriend_003C_003EAccessor : IMemberAccessor<MyReputationModifiers, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationModifiers owner, in float value)
				{
					owner.Friend = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationModifiers owner, out float value)
				{
					value = owner.Friend;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationModifiers_003C_003ENeutral_003C_003EAccessor : IMemberAccessor<MyReputationModifiers, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationModifiers owner, in float value)
				{
					owner.Neutral = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationModifiers owner, out float value)
				{
					value = owner.Neutral;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationModifiers_003C_003EHostile_003C_003EAccessor : IMemberAccessor<MyReputationModifiers, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationModifiers owner, in float value)
				{
					owner.Hostile = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationModifiers owner, out float value)
				{
					value = owner.Hostile;
				}
			}

			public float Owner;

			public float Friend;

			public float Neutral;

			public float Hostile;
		}

		protected sealed class CreateFactionByDefinition_003C_003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string tag, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateFactionByDefinition(tag);
			}
		}

		protected sealed class UnlockAchievementForClient_003C_003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string achievement, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UnlockAchievementForClient(achievement);
			}
		}

		protected sealed class Invoke_AddRep_DEBUG_003C_003ESystem_Int64_0023System_Int64_0023System_Int32 : ICallSite<IMyEventOwner, long, long, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerId, in long factionId, in int delta, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				Invoke_AddRep_DEBUG(playerId, factionId, delta);
			}
		}

		protected sealed class AddFactionPlayerReputationSuccess_003C_003ESystem_Int64_0023System_Collections_Generic_List_00601_003CSandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationChangeWrapper_003E : ICallSite<IMyEventOwner, long, List<MyReputationChangeWrapper>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerId, in List<MyReputationChangeWrapper> changes, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AddFactionPlayerReputationSuccess(playerId, changes);
			}
		}

		protected sealed class FactionStateChangeRequest_003C_003EVRage_Game_ModAPI_MyFactionStateChange_0023System_Int64_0023System_Int64_0023System_Int64 : ICallSite<IMyEventOwner, MyFactionStateChange, long, long, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyFactionStateChange action, in long fromFactionId, in long toFactionId, in long playerId, in DBNull arg5, in DBNull arg6)
			{
				FactionStateChangeRequest(action, fromFactionId, toFactionId, playerId);
			}
		}

		protected sealed class FactionStateChangeSuccess_003C_003EVRage_Game_ModAPI_MyFactionStateChange_0023System_Int64_0023System_Int64_0023System_Int64_0023System_Int64 : ICallSite<IMyEventOwner, MyFactionStateChange, long, long, long, long, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyFactionStateChange action, in long fromFactionId, in long toFactionId, in long playerId, in long senderId, in DBNull arg6)
			{
				FactionStateChangeSuccess(action, fromFactionId, toFactionId, playerId, senderId);
			}
		}

		protected sealed class ChangeAutoAcceptRequest_003C_003ESystem_Int64_0023System_Boolean_0023System_Boolean : ICallSite<IMyEventOwner, long, bool, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long factionId, in bool autoAcceptMember, in bool autoAcceptPeace, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ChangeAutoAcceptRequest(factionId, autoAcceptMember, autoAcceptPeace);
			}
		}

		protected sealed class ChangeAutoAcceptSuccess_003C_003ESystem_Int64_0023System_Boolean_0023System_Boolean : ICallSite<IMyEventOwner, long, bool, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long factionId, in bool autoAcceptMember, in bool autoAcceptPeace, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ChangeAutoAcceptSuccess(factionId, autoAcceptMember, autoAcceptPeace);
			}
		}

		protected sealed class EditFactionRequest_003C_003ESandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg : ICallSite<IMyEventOwner, AddFactionMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AddFactionMsg msgEdit, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				EditFactionRequest(msgEdit);
			}
		}

		protected sealed class EditFactionSuccess_003C_003ESandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_0023System_Int64 : ICallSite<IMyEventOwner, AddFactionMsg, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AddFactionMsg msgEdit, in long senderId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				EditFactionSuccess(msgEdit, senderId);
			}
		}

		protected sealed class RequestFactionScoreAndPercentageUpdate_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long factionId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestFactionScoreAndPercentageUpdate(factionId);
			}
		}

		protected sealed class RecieveFactionScoreAndPercentage_003C_003ESystem_Int64_0023System_Int32_0023System_Single : ICallSite<IMyEventOwner, long, int, float, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long factionId, in int score, in float percentageFinished, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RecieveFactionScoreAndPercentage(factionId, score, percentageFinished);
			}
		}

		protected sealed class RequestFactionScoreAndPercentageUpdate_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long factionId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestFactionScoreAndPercentageUpdate(factionId);
			}
		}

		protected sealed class RecieveFactionScoreAndPercentage_003C_003ESystem_Int64_0023System_Int32_0023System_Single : ICallSite<IMyEventOwner, long, int, float, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long factionId, in int score, in float percentageFinished, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RecieveFactionScoreAndPercentage(factionId, score, percentageFinished);
			}
		}

		protected sealed class CreateFactionRequest_003C_003ESandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg : ICallSite<IMyEventOwner, AddFactionMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AddFactionMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateFactionRequest(msg);
			}
		}

		protected sealed class CreateFactionSuccess_003C_003ESandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg : ICallSite<IMyEventOwner, AddFactionMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AddFactionMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateFactionSuccess(msg);
			}
		}

		protected sealed class SetDefaultFactionStates_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long recivedFactionId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetDefaultFactionStates(recivedFactionId);
			}
		}

		protected sealed class AddDiscoveredFaction_Clients_003C_003ESystem_UInt64_0023System_Int32_0023System_Int64 : ICallSite<IMyEventOwner, ulong, int, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong playerId, in int serialId, in long factionId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AddDiscoveredFaction_Clients(playerId, serialId, factionId);
			}
		}

		protected sealed class RemoveDiscoveredFaction_Clients_003C_003ESystem_UInt64_0023System_Int32_0023System_Int64 : ICallSite<IMyEventOwner, ulong, int, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong playerId, in int serialId, in long factionId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveDiscoveredFaction_Clients(playerId, serialId, factionId);
			}
		}

		protected sealed class RemovePlayerFromVisibility_Broadcast_003C_003ESandbox_Game_World_MyPlayer_003C_003EPlayerId : ICallSite<IMyEventOwner, MyPlayer.PlayerId, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyPlayer.PlayerId playerId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemovePlayerFromVisibility_Broadcast(playerId);
			}
		}

		public const int MAX_CHARACTER_FACTION = 512;

		public const string DLC_ECONOMY_ICON_CATEGORY = "Other";

		public const int ACHIEVEMENT_FRIEND_OF_FACTION_COUNT = 3;

		public const string ACHIEVEMENT_KEY_FRIEND_OF_FACTION = "FriendOfFactions";

		private const string SPIDER_FACTION_TAG = "SPID";

		public Action<long, long> PlayerKilledByPlayer;

		public Action<long> PlayerKilledByUnknown;

		/// <summary>
		/// All factions in a game.
		/// </summary>
		private Dictionary<long, MyFaction> m_factions = new Dictionary<long, MyFaction>();

<<<<<<< HEAD
		/// <summary>
		/// Ditto, indexed by their faction tag
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private Dictionary<string, MyFaction> m_factionsByTag = new Dictionary<string, MyFaction>(StringComparer.InvariantCultureIgnoreCase);

		/// <summary>
		///
		/// </summary>
		private Dictionary<long, HashSet<long>> m_factionRequests = new Dictionary<long, HashSet<long>>();

		/// <summary>
		/// Relations between factions.
		/// </summary>
		private Dictionary<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> m_relationsBetweenFactions = new Dictionary<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>>(MyRelatablePair.Comparer);

		private Dictionary<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> m_relationsBetweenPlayersAndFactions = new Dictionary<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>>(MyRelatablePair.Comparer);

		/// <summary>
		/// Player in faction dictionary.
		/// </summary>
		private Dictionary<long, long> m_playerFaction = new Dictionary<long, long>();

		/// <summary>
		/// Dictionary of player id to factions ids that player explored/discovered.
		/// </summary>
		private Dictionary<MyPlayer.PlayerId, List<long>> m_playerToFactionsVis = new Dictionary<MyPlayer.PlayerId, List<long>>();

		/// <summary>
		/// Reputation settings.
		/// </summary>
		private MyReputationSettingsDefinition m_reputationSettings;

		/// <summary>
		/// This dictionary tracks how much reputation was gain to pirates over time per player.
		/// </summary>
		private Dictionary<long, Tuple<int, TimeSpan>> m_playerToReputationLimits = new Dictionary<long, Tuple<int, TimeSpan>>();

		private MyReputationNotification m_notificationRepInc;

		private MyReputationNotification m_notificationRepDec;

		public bool JoinableFactionsPresent
		{
			get
			{
				foreach (KeyValuePair<long, MyFaction> faction in m_factions)
				{
					if (faction.Value.AcceptHumans)
					{
						return true;
					}
				}
				return false;
			}
		}

		public MyFaction this[long factionId] => m_factions[factionId];

		public Dictionary<long, IMyFaction> Factions => Enumerable.ToDictionary<KeyValuePair<long, MyFaction>, long, IMyFaction>((IEnumerable<KeyValuePair<long, MyFaction>>)m_factions, (Func<KeyValuePair<long, MyFaction>, long>)((KeyValuePair<long, MyFaction> e) => e.Key), (Func<KeyValuePair<long, MyFaction>, IMyFaction>)((KeyValuePair<long, MyFaction> e) => e.Value));

		public event Action<MyFaction, MyPlayer.PlayerId> OnFactionDiscovered;

		public event Action<MyFaction, long> OnPlayerJoined;

		public event Action<MyFaction, long> OnPlayerLeft;

		public event Action<MyFactionStateChange, long, long, long, long> FactionStateChanged;

		public event Action<long, bool, bool> FactionAutoAcceptChanged;

		public event Action<long> FactionEdited;

		public event Action<long, bool> FactionClientChanged;

		public event Action<long> FactionCreated;

		event Action<long, bool, bool> IMyFactionCollection.FactionAutoAcceptChanged
		{
			add
			{
				FactionAutoAcceptChanged += value;
			}
			remove
			{
				FactionAutoAcceptChanged -= value;
			}
		}

		event Action<long> IMyFactionCollection.FactionCreated
		{
			add
			{
				FactionCreated += value;
			}
			remove
			{
				FactionCreated -= value;
			}
		}

		event Action<long> IMyFactionCollection.FactionEdited
		{
			add
			{
				FactionEdited += value;
			}
			remove
			{
				FactionEdited -= value;
			}
		}

		event Action<MyFactionStateChange, long, long, long, long> IMyFactionCollection.FactionStateChanged
		{
			add
			{
				FactionStateChanged += value;
			}
			remove
			{
				FactionStateChanged -= value;
			}
		}

		/// <summary>
		/// Checks if faction exists.
		/// </summary>
		/// <param name="factionId">Faction id.</param>
		/// <returns>If true, faction exists.</returns>
		public bool Contains(long factionId)
		{
			return m_factions.ContainsKey(factionId);
		}

		public bool FactionTagExists(string tag, IMyFaction doNotCheck = null)
		{
			return TryGetFactionByTag(tag, doNotCheck) != null;
		}

		public bool FactionNameExists(string name, IMyFaction doNotCheck = null)
		{
			foreach (KeyValuePair<long, MyFaction> faction in m_factions)
			{
				MyFaction value = faction.Value;
				if ((doNotCheck == null || doNotCheck.FactionId != value.FactionId) && string.Equals(name, value.Name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		public IMyFaction TryGetFactionById(long factionId)
		{
			if (m_factions.TryGetValue(factionId, out var value))
			{
				return value;
			}
			return null;
		}

		public MyFaction TryGetOrCreateFactionByTag(string tag)
		{
			MyFaction myFaction = TryGetFactionByTag(tag);
			if (myFaction == null)
			{
				if (MyDefinitionManager.Static.TryGetFactionDefinition(tag) == null)
				{
					return null;
				}
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CreateFactionByDefinition, tag);
				myFaction = TryGetFactionByTag(tag);
			}
			return myFaction;
		}

		/// <summary>
		/// Returns if faction is NPC faction (Defined in Factions.sbc or if faction belongs to economy NPC factions (Miners, Traders, Builders))
		/// </summary>
		/// <param name="tag">Tag of the faction.</param>
		/// <returns>True if NPC faction (Defined in Factions.sbc or economy faction).</returns>
		public bool IsNpcFaction(string tag)
		{
			if (MyDefinitionManager.Static.TryGetFactionDefinition(tag) != null)
			{
				return true;
			}
			MyFaction myFaction = TryGetFactionByTag(tag);
			if (myFaction != null)
			{
				MyFactionTypes factionType = myFaction.FactionType;
				if ((uint)(factionType - 2) <= 2u)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsNpcFaction(long factionId)
		{
			if (!m_factions.ContainsKey(factionId))
			{
				return false;
			}
			MyFaction myFaction = m_factions[factionId];
			if (myFaction != null)
			{
				MyFactionTypes factionType = myFaction.FactionType;
				if ((uint)(factionType - 2) <= 2u)
				{
					return true;
				}
			}
			return false;
		}

		internal bool IsDiscoveredByDefault(string tag)
		{
			return MyDefinitionManager.Static.TryGetFactionDefinition(tag)?.DiscoveredByDefault ?? false;
		}

<<<<<<< HEAD
		[Event(null, 304)]
=======
		[Event(null, 300)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public static void CreateFactionByDefinition(string tag)
		{
			if (!MySession.Static.Factions.m_factionsByTag.ContainsKey(tag))
			{
				MyFactionDefinition myFactionDefinition = MyDefinitionManager.Static.TryGetFactionDefinition(tag);
				if (myFactionDefinition != null)
				{
					MyIdentity myIdentity = Sync.Players.CreateNewIdentity(myFactionDefinition.Founder);
					Sync.Players.MarkIdentityAsNPC(myIdentity.IdentityId);
					CreateFactionServer(myIdentity.IdentityId, tag.ToUpperInvariant(), myFactionDefinition.DisplayNameText, myFactionDefinition.DescriptionText, "", myFactionDefinition);
				}
			}
		}

		/// <summary>
		/// Creates adds default factions to the faction collection.
		/// </summary>
		public void CreateDefaultFactions()
		{
			foreach (MyFactionDefinition defaultFaction in MyDefinitionManager.Static.GetDefaultFactions())
			{
				if (TryGetFactionByTag(defaultFaction.Tag) != null)
				{
					continue;
				}
				MyIdentity myIdentity = Sync.Players.CreateNewIdentity(defaultFaction.Founder);
				if (myIdentity != null)
				{
					Sync.Players.MarkIdentityAsNPC(myIdentity.IdentityId);
					long num = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.FACTION);
					if (!CreateFactionInternal(myIdentity.IdentityId, num, defaultFaction))
					{
						Sync.Players.RemoveIdentity(myIdentity.IdentityId);
					}
					MyBankingSystem.Static.CreateAccount(num, defaultFaction.StartingBalance);
				}
			}
			MyPlayer.PlayerId? playerId = null;
			if (MySession.Static.LocalHumanPlayer != null)
			{
				playerId = MySession.Static.LocalHumanPlayer.Id;
			}
			CompatDefaultFactions(playerId);
		}

		internal void DeleteFactionRelations(long factionId)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			List<MyRelatablePair> list = new List<MyRelatablePair>();
			foreach (KeyValuePair<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> relationsBetweenPlayersAndFaction in m_relationsBetweenPlayersAndFactions)
			{
				if (relationsBetweenPlayersAndFaction.Key.RelateeId2 == factionId)
				{
					list.Add(relationsBetweenPlayersAndFaction.Key);
				}
			}
			foreach (MyRelatablePair item in list)
			{
				m_relationsBetweenPlayersAndFactions.Remove(item);
			}
			list.Clear();
			foreach (KeyValuePair<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> relationsBetweenFaction in m_relationsBetweenFactions)
			{
				if (relationsBetweenFaction.Key.RelateeId1 == factionId || relationsBetweenFaction.Key.RelateeId2 == factionId)
				{
					list.Add(relationsBetweenFaction.Key);
				}
			}
			foreach (MyRelatablePair item2 in list)
			{
				m_relationsBetweenFactions.Remove(item2);
			}
		}

		internal void DeletePlayerRelations(long identityId)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			List<MyRelatablePair> list = new List<MyRelatablePair>();
			foreach (KeyValuePair<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> relationsBetweenPlayersAndFaction in m_relationsBetweenPlayersAndFactions)
			{
				if (relationsBetweenPlayersAndFaction.Key.RelateeId1 == identityId)
				{
					list.Add(relationsBetweenPlayersAndFaction.Key);
				}
			}
			foreach (MyRelatablePair item in list)
			{
				m_relationsBetweenPlayersAndFactions.Remove(item);
			}
		}

		public void CompatDefaultFactions(MyPlayer.PlayerId? playerId = null)
		{
			foreach (MyFactionDefinition item in MyDefinitionManager.Static.GetFactionsFromDefinition())
			{
				MyFaction myFaction = TryGetFactionByTag(item.Tag);
				if (myFaction != null)
				{
					if (playerId.HasValue && IsDiscoveredByDefault(item.Tag) && !IsFactionDiscovered(playerId.Value, myFaction.FactionId))
					{
						AddDiscoveredFaction(playerId.Value, myFaction.FactionId, triggerEvent: false);
					}
					if (!MyBankingSystem.Static.TryGetAccountInfo(myFaction.FactionId, out var _))
					{
						MyBankingSystem.Static.CreateAccount(myFaction.FactionId, item.StartingBalance);
					}
					if (myFaction.FactionType != item.Type)
					{
						myFaction.FactionType = item.Type;
					}
					if (!myFaction.FactionIcon.HasValue && !string.IsNullOrEmpty(item.FactionIcon.String))
					{
						myFaction.FactionIcon = item.FactionIcon;
						myFaction.FactionIconWorkshopId = item.FactionIconWorkshopId;
						myFaction.RefreshIconPaths();
						myFaction.CustomColor = MyColorPickerConstants.HSVToHSVOffset(Vector3.Zero);
					}
				}
			}
		}

		public MyFaction TryGetFactionByTag(string tag, IMyFaction doNotCheck = null)
		{
			m_factionsByTag.TryGetValue(tag, out var value);
			if (value == null)
			{
				return null;
			}
			if (doNotCheck != null && value.FactionId == doNotCheck.FactionId)
			{
				return null;
			}
			return value;
		}

		private void UnregisterFactionTag(MyFaction faction)
		{
			if (faction != null)
			{
				m_factionsByTag.Remove(faction.Tag);
			}
		}

		private void RegisterFactionTag(MyFaction faction)
		{
			if (faction != null)
			{
				string tag = faction.Tag;
				m_factionsByTag.TryGetValue(tag, out var _);
				m_factionsByTag[tag] = faction;
			}
		}

		public IMyFaction TryGetPlayerFaction(long playerId)
		{
			return GetPlayerFaction(playerId);
		}

		public MyFaction GetPlayerFaction(long playerId)
		{
			MyFaction value = null;
			if (m_playerFaction.TryGetValue(playerId, out var value2))
			{
				m_factions.TryGetValue(value2, out value);
			}
			return value;
		}

		public void AddPlayerToFaction(long playerId, long factionId)
		{
			if (m_factions.TryGetValue(factionId, out var value))
			{
				value.AcceptJoin(playerId, autoaccept: true);
			}
			else
			{
				AddPlayerToFactionInternal(playerId, factionId);
			}
			foreach (KeyValuePair<long, MyFaction> faction in m_factions)
			{
				faction.Value.CancelJoinRequest(playerId);
			}
		}

		public void AddNewNPCToFaction(long factionId)
		{
			string npcName = m_factions[factionId].Tag + " NPC" + MyRandom.Instance.Next(1000, 9999);
			AddNewNPCToFaction(factionId, npcName);
		}

		public void AddNewNPCToFaction(long factionId, string npcName)
		{
			MyIdentity myIdentity = Sync.Players.CreateNewIdentity(npcName, null, null, initialPlayer: false, addToNpcs: true);
			AddPlayerToFaction(myIdentity.IdentityId, factionId);
		}

		internal void AddPlayerToFactionInternal(long playerId, long factionId)
		{
			m_playerFaction[playerId] = factionId;
		}

		public void KickPlayerFromFaction(long playerId)
		{
			m_playerFaction.Remove(playerId);
		}

		/// <summary>
		/// Use this for quick check of relation between two factions.
		/// </summary>
		/// <param name="factionId1">First faction id</param>
		/// <param name="factionId2">Second faction id</param>
		/// <returns>Enum that describes the relation between given factions</returns>
		public Tuple<MyRelationsBetweenFactions, int> GetRelationBetweenFactions(long factionId1, long factionId2)
		{
			return GetRelationBetweenFactions(factionId1, factionId2, MyPerGameSettings.DefaultFactionRelationshipAndReputation);
		}

		public Tuple<MyRelationsBetweenFactions, int> GetRelationBetweenFactions(long factionId1, long factionId2, Tuple<MyRelationsBetweenFactions, int> defaultState)
		{
			if (factionId1 == factionId2 && factionId1 != 0L)
			{
				return new Tuple<MyRelationsBetweenFactions, int>(MyRelationsBetweenFactions.Neutral, 0);
			}
			return m_relationsBetweenFactions.GetValueOrDefault(new MyRelatablePair(factionId1, factionId2), defaultState);
		}

		public Tuple<MyRelationsBetweenFactions, int> GetRelationBetweenPlayerAndFaction(long playerId, long factionId)
		{
			return GetRelationBetweenPlayerAndFaction(playerId, factionId, MyPerGameSettings.DefaultFactionRelationshipAndReputation);
		}

		public Tuple<MyRelationsBetweenFactions, int> GetRelationBetweenPlayerAndFaction(long playerId, long factionId, Tuple<MyRelationsBetweenFactions, int> defaultState)
		{
			return m_relationsBetweenPlayersAndFactions.GetValueOrDefault(new MyRelatablePair(playerId, factionId), defaultState);
		}

		public bool AreFactionsEnemies(long factionId1, long factionId2)
		{
			return GetRelationBetweenFactions(factionId1, factionId2).Item1 == MyRelationsBetweenFactions.Enemies;
		}

		public bool AreFactionsNeutrals(long factionId1, long factionId2)
		{
			return GetRelationBetweenFactions(factionId1, factionId2).Item1 == MyRelationsBetweenFactions.Neutral;
		}

		public bool AreFactionsFriends(long factionId1, long factionId2)
		{
			return GetRelationBetweenFactions(factionId1, factionId2).Item1 == MyRelationsBetweenFactions.Friends;
		}

		public bool IsFactionWithPlayerEnemy(long playerId, long factionId)
		{
			return GetRelationBetweenPlayerAndFaction(playerId, factionId).Item1 == MyRelationsBetweenFactions.Enemies;
		}

		public bool IsFactionWithPlayerNeutral(long playerId, long factionId)
		{
			return GetRelationBetweenPlayerAndFaction(playerId, factionId).Item1 == MyRelationsBetweenFactions.Neutral;
		}

		public bool IsFactionWithPlayerFriend(long playerId, long factionId)
		{
			return GetRelationBetweenPlayerAndFaction(playerId, factionId).Item1 == MyRelationsBetweenFactions.Friends;
		}

		public MyFactionPeaceRequestState GetRequestState(long myFactionId, long foreignFactionId)
		{
			if (m_factionRequests.ContainsKey(myFactionId) && m_factionRequests[myFactionId].Contains(foreignFactionId))
			{
				return MyFactionPeaceRequestState.Sent;
			}
			if (m_factionRequests.ContainsKey(foreignFactionId) && m_factionRequests[foreignFactionId].Contains(myFactionId))
			{
				return MyFactionPeaceRequestState.Pending;
			}
			return MyFactionPeaceRequestState.None;
		}

		public bool IsPeaceRequestStateSent(long myFactionId, long foreignFactionId)
		{
			return GetRequestState(myFactionId, foreignFactionId) == MyFactionPeaceRequestState.Sent;
		}

		public bool IsPeaceRequestStatePending(long myFactionId, long foreignFactionId)
		{
			return GetRequestState(myFactionId, foreignFactionId) == MyFactionPeaceRequestState.Pending;
		}

		public static void RemoveFaction(long factionId)
		{
			SendFactionChange(MyFactionStateChange.RemoveFaction, factionId, factionId, 0L);
		}

		public static void SendPeaceRequest(long fromFactionId, long toFactionId)
		{
			SendFactionChange(MyFactionStateChange.SendPeaceRequest, fromFactionId, toFactionId, 0L);
		}

		public static void CancelPeaceRequest(long fromFactionId, long toFactionId)
		{
			SendFactionChange(MyFactionStateChange.CancelPeaceRequest, fromFactionId, toFactionId, 0L);
		}

		public static void AcceptPeace(long fromFactionId, long toFactionId)
		{
			SendFactionChange(MyFactionStateChange.AcceptPeace, fromFactionId, toFactionId, 0L);
		}

		public static void DeclareWar(long fromFactionId, long toFactionId)
		{
			SendFactionChange(MyFactionStateChange.DeclareWar, fromFactionId, toFactionId, 0L);
		}

		public static void SendJoinRequest(long factionId, long playerId)
		{
			SendFactionChange(MyFactionStateChange.FactionMemberSendJoin, factionId, factionId, playerId);
		}

		public static void CancelJoinRequest(long factionId, long playerId)
		{
			SendFactionChange(MyFactionStateChange.FactionMemberCancelJoin, factionId, factionId, playerId);
		}

		public static void AcceptJoin(long factionId, long playerId)
		{
			SendFactionChange(MyFactionStateChange.FactionMemberAcceptJoin, factionId, factionId, playerId);
		}

		public static void KickMember(long factionId, long playerId)
		{
			SendFactionChange(MyFactionStateChange.FactionMemberKick, factionId, factionId, playerId);
		}

		public static void PromoteMember(long factionId, long playerId)
		{
			SendFactionChange(MyFactionStateChange.FactionMemberPromote, factionId, factionId, playerId);
		}

		public static void DemoteMember(long factionId, long playerId)
		{
			SendFactionChange(MyFactionStateChange.FactionMemberDemote, factionId, factionId, playerId);
		}

		public static void MemberLeaves(long factionId, long playerId)
		{
			SendFactionChange(MyFactionStateChange.FactionMemberLeave, factionId, factionId, playerId);
		}

		private bool CheckFactionStateChange(MyFactionStateChange action, long fromFactionId, long toFactionId, long playerId, long senderId)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			if (!m_factions.ContainsKey(fromFactionId) || !m_factions.ContainsKey(toFactionId))
			{
				return false;
			}
			if (senderId != 0L)
			{
				switch (action)
				{
				case MyFactionStateChange.RemoveFaction:
				case MyFactionStateChange.SendPeaceRequest:
				case MyFactionStateChange.CancelPeaceRequest:
				case MyFactionStateChange.AcceptPeace:
				case MyFactionStateChange.DeclareWar:
				case MyFactionStateChange.SendFriendRequest:
				case MyFactionStateChange.CancelFriendRequest:
				case MyFactionStateChange.AcceptFriendRequest:
				case MyFactionStateChange.FactionMemberAcceptJoin:
				case MyFactionStateChange.FactionMemberKick:
				case MyFactionStateChange.FactionMemberPromote:
				case MyFactionStateChange.FactionMemberDemote:
					if (!m_factions[fromFactionId].IsLeader(senderId) && !MySession.Static.IsUserAdmin(MySession.Static.Players.TryGetSteamId(senderId)))
					{
						MyLog.Default.Warning("Player is attempting a faction state change they have no rights to do - {0}", senderId);
						return false;
					}
					break;
				case MyFactionStateChange.FactionMemberSendJoin:
				case MyFactionStateChange.FactionMemberCancelJoin:
				case MyFactionStateChange.FactionMemberLeave:
					if (playerId != senderId && !MySession.Static.IsUserAdmin(MySession.Static.Players.TryGetSteamId(senderId)))
					{
						MyLog.Default.Warning("Player is attempting a faction state change they have no rights to do - {0}", senderId);
						return false;
					}
					break;
				default:
					return false;
				case MyFactionStateChange.FactionMemberNotPossibleJoin:
					break;
				}
			}
			HashSet<long> value;
			switch (action)
			{
			case MyFactionStateChange.RemoveFaction:
				return true;
			case MyFactionStateChange.SendPeaceRequest:
				if (!m_factionRequests.TryGetValue(fromFactionId, out value) || !value.Contains(toFactionId))
				{
					return GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Enemies;
				}
				return false;
			case MyFactionStateChange.CancelPeaceRequest:
				if (m_factionRequests.TryGetValue(fromFactionId, out value) && value.Contains(toFactionId))
				{
					return GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Enemies;
				}
				return false;
			case MyFactionStateChange.SendFriendRequest:
				if (!m_factionRequests.TryGetValue(fromFactionId, out value) || !value.Contains(toFactionId))
				{
					return GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Neutral;
				}
				return false;
			case MyFactionStateChange.CancelFriendRequest:
				if (m_factionRequests.TryGetValue(fromFactionId, out value) && value.Contains(toFactionId))
				{
					return GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Neutral;
				}
				return false;
			case MyFactionStateChange.AcceptPeace:
				return GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 != MyRelationsBetweenFactions.Neutral;
			case MyFactionStateChange.DeclareWar:
				return GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 != MyRelationsBetweenFactions.Enemies;
			case MyFactionStateChange.AcceptFriendRequest:
				return GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Friends;
			case MyFactionStateChange.FactionMemberSendJoin:
				if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION)
				{
					return !m_factions[fromFactionId].JoinRequests.ContainsKey(playerId);
				}
				if (!m_factions[fromFactionId].IsMember(playerId))
				{
					return !m_factions[fromFactionId].JoinRequests.ContainsKey(playerId);
				}
				return false;
			case MyFactionStateChange.FactionMemberCancelJoin:
				if (!m_factions[fromFactionId].IsMember(playerId))
				{
					return m_factions[fromFactionId].JoinRequests.ContainsKey(playerId);
				}
				return false;
			case MyFactionStateChange.FactionMemberAcceptJoin:
				return m_factions[fromFactionId].JoinRequests.ContainsKey(playerId);
			case MyFactionStateChange.FactionMemberKick:
				return m_factions[fromFactionId].IsMember(playerId);
			case MyFactionStateChange.FactionMemberPromote:
				return m_factions[fromFactionId].IsMember(playerId);
			case MyFactionStateChange.FactionMemberDemote:
				return m_factions[fromFactionId].IsLeader(playerId);
			case MyFactionStateChange.FactionMemberLeave:
			{
				ulong steamId = MySession.Static.Players.TryGetSteamId(playerId);
				if (m_factions[fromFactionId].IsMember(playerId))
				{
					if (MySession.Static.Settings.EnableTeamBalancing)
					{
						return MySession.Static.IsUserSpaceMaster(steamId);
					}
					return true;
				}
				return false;
			}
			default:
				return false;
			}
		}

		private void ApplyFactionStateChange(MyFactionStateChange action, long fromFactionId, long toFactionId, long playerId, long senderId)
		{
			switch (action)
			{
			case MyFactionStateChange.RemoveFaction:
			{
				if (m_factions[fromFactionId].IsMember(MySession.Static.LocalPlayerId))
				{
					m_playerFaction.Remove(playerId);
					if (MySession.Static.ChatSystem.CurrentChannel == ChatChannel.Faction)
					{
						MySession.Static.ChatSystem.ChangeChatChannel_Global();
					}
				}
				foreach (KeyValuePair<long, MyFaction> faction in m_factions)
				{
					if (faction.Key != fromFactionId)
					{
						ClearRequest(fromFactionId, faction.Key);
						RemoveRelation(fromFactionId, faction.Key);
					}
				}
				MyFaction value = null;
				m_factions.TryGetValue(fromFactionId, out value);
				UnregisterFactionTag(value);
				m_factions.Remove(fromFactionId);
				DeleteFactionRelations(fromFactionId);
				RemoveFactionFromVisibility(fromFactionId);
				break;
			}
			case MyFactionStateChange.SendPeaceRequest:
			case MyFactionStateChange.SendFriendRequest:
			{
				if (m_factionRequests.TryGetValue(fromFactionId, out var value2))
				{
					value2.Add(toFactionId);
					break;
				}
				value2 = new HashSet<long>();
				value2.Add(toFactionId);
				m_factionRequests.Add(fromFactionId, value2);
				break;
			}
			case MyFactionStateChange.CancelPeaceRequest:
			case MyFactionStateChange.CancelFriendRequest:
				ClearRequest(fromFactionId, toFactionId);
				break;
			case MyFactionStateChange.AcceptFriendRequest:
				ClearRequest(fromFactionId, toFactionId);
				ChangeFactionRelation(fromFactionId, toFactionId, MyRelationsBetweenFactions.Friends);
				break;
			case MyFactionStateChange.AcceptPeace:
				ClearRequest(fromFactionId, toFactionId);
				ChangeFactionRelation(fromFactionId, toFactionId, MyRelationsBetweenFactions.Neutral);
				break;
			case MyFactionStateChange.DeclareWar:
				ClearRequest(fromFactionId, toFactionId);
				ChangeFactionRelation(fromFactionId, toFactionId, MyRelationsBetweenFactions.Enemies);
				break;
			case MyFactionStateChange.FactionMemberSendJoin:
				m_factions[fromFactionId].AddJoinRequest(playerId);
				break;
			case MyFactionStateChange.FactionMemberCancelJoin:
				m_factions[fromFactionId].CancelJoinRequest(playerId);
				break;
			case MyFactionStateChange.FactionMemberAcceptJoin:
			{
				ulong steamId = MySession.Static.Players.TryGetSteamId(playerId);
				bool flag = MySession.Static.IsUserSpaceMaster(steamId) || m_factions[fromFactionId].Members.Count == 0;
				if (flag && m_factions[fromFactionId].IsEveryoneNpc())
				{
					m_factions[fromFactionId].AcceptJoin(playerId, flag);
					m_factions[fromFactionId].PromoteMember(playerId);
				}
				else
				{
					m_factions[fromFactionId].AcceptJoin(playerId, flag);
				}
				break;
			}
			case MyFactionStateChange.FactionMemberLeave:
				m_factions[fromFactionId].KickMember(playerId);
				break;
			case MyFactionStateChange.FactionMemberKick:
				if (Sync.IsServer && playerId != m_factions[fromFactionId].FounderId && MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION)
				{
					MyBlockLimits.TransferBlockLimits(playerId, m_factions[fromFactionId].FounderId);
				}
				m_factions[fromFactionId].KickMember(playerId);
				break;
			case MyFactionStateChange.FactionMemberPromote:
				m_factions[fromFactionId].PromoteMember(playerId);
				break;
			case MyFactionStateChange.FactionMemberDemote:
				m_factions[fromFactionId].DemoteMember(playerId);
				break;
			}
		}

		private void ClearRequest(long fromFactionId, long toFactionId)
		{
			if (m_factionRequests.ContainsKey(fromFactionId))
			{
				m_factionRequests[fromFactionId].Remove(toFactionId);
			}
			if (m_factionRequests.ContainsKey(toFactionId))
			{
				m_factionRequests[toFactionId].Remove(fromFactionId);
			}
		}

		private void ChangeFactionRelation(long fromFactionId, long toFactionId, MyRelationsBetweenFactions relation)
		{
			int num = TranslateRelationToReputation(relation);
			m_relationsBetweenFactions[new MyRelatablePair(fromFactionId, toFactionId)] = new Tuple<MyRelationsBetweenFactions, int>(relation, num);
			foreach (KeyValuePair<long, MyFactionMember> member in TryGetFactionById(fromFactionId).Members)
			{
				SetReputationBetweenPlayerAndFaction(member.Key, toFactionId, num);
			}
			foreach (KeyValuePair<long, MyFactionMember> member2 in TryGetFactionById(toFactionId).Members)
			{
				SetReputationBetweenPlayerAndFaction(member2.Key, fromFactionId, num);
			}
		}

		private void ChangeReputationBetweenFactions(long fromFactionId, long toFactionId, int reputation)
		{
			m_relationsBetweenFactions[new MyRelatablePair(fromFactionId, toFactionId)] = new Tuple<MyRelationsBetweenFactions, int>(TranslateReputationToRelation(reputation), reputation);
		}

		public void SetReputationBetweenFactions(long fromFactionId, long toFactionId, int reputation)
		{
			ChangeReputationBetweenFactions(fromFactionId, toFactionId, reputation);
		}

		public void SetReputationBetweenPlayerAndFaction(long identityId, long factionId, int reputation)
		{
			ChangeReputationWithPlayer(identityId, factionId, reputation);
		}

		public DictionaryReader<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> GetAllFactionRelations()
		{
			return new DictionaryReader<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>>(m_relationsBetweenFactions);
		}

		public int TranslateRelationToReputation(MyRelationsBetweenFactions relation)
		{
			return (MySession.Static?.GetComponent<MySessionComponentEconomy>())?.TranslateRelationToReputation(relation) ?? MyPerGameSettings.DefaultFactionReputation;
		}

		public MyRelationsBetweenFactions TranslateReputationToRelation(int reputation)
		{
			return (MySession.Static?.GetComponent<MySessionComponentEconomy>())?.TranslateReputationToRelationship(reputation) ?? MyPerGameSettings.DefaultFactionRelationship;
		}

		public int ClampReputation(int reputation)
		{
			return (MySession.Static?.GetComponent<MySessionComponentEconomy>())?.ClampReputation(reputation) ?? reputation;
		}

		private void ChangeRelationWithPlayer(long fromPlayerId, long toFactionId, MyRelationsBetweenFactions relation)
		{
			m_relationsBetweenPlayersAndFactions[new MyRelatablePair(fromPlayerId, toFactionId)] = new Tuple<MyRelationsBetweenFactions, int>(relation, TranslateRelationToReputation(relation));
		}

		private void ChangeReputationWithPlayer(long fromPlayerId, long toFactionId, int reputation)
		{
			MyRelatablePair key = new MyRelatablePair(fromPlayerId, toFactionId);
			Tuple<MyRelationsBetweenFactions, int> tuple = new Tuple<MyRelationsBetweenFactions, int>(TranslateReputationToRelation(reputation), reputation);
			if (m_relationsBetweenPlayersAndFactions.ContainsKey(key))
			{
				Tuple<MyRelationsBetweenFactions, int> tuple2 = m_relationsBetweenPlayersAndFactions[key];
				m_relationsBetweenPlayersAndFactions[key] = tuple;
				if (tuple2.Item1 != tuple.Item1)
				{
					PlayerReputationLevelChanged(fromPlayerId, toFactionId, tuple2.Item1, tuple.Item1);
				}
			}
			else
			{
				m_relationsBetweenPlayersAndFactions[key] = tuple;
			}
		}

		private void PlayerReputationLevelChanged(long fromPlayerId, long toFactionId, MyRelationsBetweenFactions oldRel, MyRelationsBetweenFactions newRel)
		{
			CheckPlayerReputationAchievements(fromPlayerId, toFactionId, oldRel, newRel);
		}

		private void CheckPlayerReputationAchievements(long fromPlayerId, long toFactionId, MyRelationsBetweenFactions oldRel, MyRelationsBetweenFactions newRel)
		{
			if (newRel != MyRelationsBetweenFactions.Friends || oldRel != 0)
			{
				return;
			}
			int num = 0;
			using (IEnumerator<KeyValuePair<long, MyFaction>> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<long, MyFaction> current = enumerator.Current;
					MyRelatablePair key = new MyRelatablePair(fromPlayerId, current.Key);
					if (current.Value.FactionType != MyFactionTypes.PlayerMade && m_relationsBetweenPlayersAndFactions.ContainsKey(key) && m_relationsBetweenPlayersAndFactions[key].Item1 == MyRelationsBetweenFactions.Friends)
					{
						num++;
					}
				}
			}
			if (num != 3)
			{
				return;
			}
			if (Sync.IsServer)
			{
				ulong value = MySession.Static.Players.TryGetSteamId(fromPlayerId);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UnlockAchievementForClient, "FriendOfFactions", new EndpointId(value));
			}
			else
			{
				UnlockAchievement_Internal("FriendOfFactions");
			}
		}

<<<<<<< HEAD
		[Event(null, 1016)]
=======
		[Event(null, 1004)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void UnlockAchievementForClient(string achievement)
		{
			UnlockAchievement_Internal(achievement);
		}

		private static void UnlockAchievement_Internal(string achievement)
		{
			MyGameService.GetAchievement(achievement, null, 0f).Unlock();
		}

		public bool HasRelationWithPlayer(long fromPlayerId, long toFactionId)
		{
			return m_relationsBetweenPlayersAndFactions.ContainsKey(new MyRelatablePair(fromPlayerId, toFactionId));
		}

		private void RemoveRelation(long fromFactionId, long toFactionId)
		{
			m_relationsBetweenFactions.Remove(new MyRelatablePair(fromFactionId, toFactionId));
		}

		public bool AddFactionPlayerReputation(long playerIdentityId, long factionId, int delta, bool propagate = true, bool adminChange = false)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			List<MyReputationChangeWrapper> list = GenerateChanges(playerIdentityId, factionId, delta, propagate, adminChange);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AddFactionPlayerReputationSuccess, playerIdentityId, list);
			AddFactionPlayerReputationSuccess(playerIdentityId, list);
			return true;
		}

		/// <summary>
		/// This method evaluates and determines damage to faction reputation dependent on the type of offense.
		/// </summary>
		/// <param name="playerIdentityId">Player identity id that triggering the damage.</param>
		/// <param name="attackedIdentityId">Attacked player identity id that the damage is done to.</param>
		/// <param name="repDamageType">Reputation damage type. (Type of offense)</param>        
		public void DamageFactionPlayerReputation(long playerIdentityId, long attackedIdentityId, MyReputationDamageType repDamageType)
		{
			if (!Sync.IsServer || attackedIdentityId == 0L)
			{
				return;
			}
			if (MySession.Static == null || MySession.Static.Factions == null)
			{
				MyLog.Default.Error("Session.Static or MySession.Static.Factions is null. Should not happen!");
				return;
			}
			long piratesId = MyPirateAntennas.GetPiratesId();
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(piratesId);
			MyFaction myFaction = TryGetPlayerFaction(attackedIdentityId) as MyFaction;
			if (myFaction == null && playerFaction != null)
			{
				int reputationDamageDelta = GetReputationDamageDelta(repDamageType, isPirates: true);
				AddFactionPlayerReputation(playerIdentityId, playerFaction.FactionId, reputationDamageDelta, propagate: false);
			}
			else if (myFaction != null && !myFaction.IsMember(playerIdentityId))
			{
				int reputationDamageDelta2 = GetReputationDamageDelta(repDamageType, playerFaction == myFaction);
				AddFactionPlayerReputation(playerIdentityId, myFaction.FactionId, -reputationDamageDelta2, propagate: false);
				if (playerFaction != null && myFaction != playerFaction)
				{
					AddFactionPlayerReputation(playerIdentityId, playerFaction.FactionId, reputationDamageDelta2, propagate: false);
				}
			}
		}

		private int GetReputationDamageDelta(MyReputationDamageType repDamageType, bool isPirates = false)
		{
			MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings myReputationDamageSettings = (isPirates ? m_reputationSettings.PirateDamageSettings : m_reputationSettings.DamageSettings);
			int result = 0;
			switch (repDamageType)
			{
			case MyReputationDamageType.GrindingWelding:
				result = myReputationDamageSettings.GrindingWelding;
				break;
			case MyReputationDamageType.Damaging:
				result = myReputationDamageSettings.Damaging;
				break;
			case MyReputationDamageType.Stealing:
				result = myReputationDamageSettings.Stealing;
				break;
			case MyReputationDamageType.Killing:
				result = myReputationDamageSettings.Killing;
				break;
			default:
				MyLog.Default.Error("Reputation damage type not handled. Check and update.");
				break;
			}
			return result;
		}

<<<<<<< HEAD
		[Event(null, 1151)]
=======
		[Event(null, 1140)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public static void Invoke_AddRep_DEBUG(long playerId, long factionId, int delta)
		{
		}

		private List<MyReputationChangeWrapper> GenerateChanges(long playerId, long factionId, int delta, bool propagate, bool adminChange = false)
		{
			List<MyReputationChangeWrapper> list = new List<MyReputationChangeWrapper>();
			MyReputationModifiers reputationModifiers = GetReputationModifiers();
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(factionId);
			if (myFaction != null)
			{
				Tuple<MyRelationsBetweenFactions, int> relationBetweenPlayerAndFaction = GetRelationBetweenPlayerAndFaction(playerId, factionId);
				int num = ClampReputation(relationBetweenPlayerAndFaction.Item2 + (int)(reputationModifiers.Owner * (float)delta));
				int num2 = num - relationBetweenPlayerAndFaction.Item2;
				bool flag = !adminChange && CheckIfMaxPirateRep(playerId, myFaction, num2);
				MyReputationChangeWrapper item;
				if (!flag)
				{
					bool showNotification = !MyCampaignManager.Static.IsCampaignRunning;
					item = new MyReputationChangeWrapper
					{
						FactionId = factionId,
						RepTotal = num,
						Change = num2,
						ShowNotification = showNotification
					};
					list.Add(item);
				}
				if (!propagate || flag)
				{
					return list;
				}
				using IEnumerator<KeyValuePair<long, MyFaction>> enumerator = GetEnumerator();
				while (enumerator.MoveNext())
				{
					KeyValuePair<long, MyFaction> current = enumerator.Current;
					if (current.Value.FactionId != factionId && current.Value.FactionType != 0 && current.Value.FactionType != MyFactionTypes.PlayerMade)
					{
						Tuple<MyRelationsBetweenFactions, int> relationBetweenFactions = GetRelationBetweenFactions(factionId, current.Value.FactionId);
						int num3 = 0;
						switch (relationBetweenFactions.Item1)
						{
						case MyRelationsBetweenFactions.Neutral:
							num3 = (int)(reputationModifiers.Neutral * (float)delta);
							break;
						case MyRelationsBetweenFactions.Enemies:
							num3 = (int)(reputationModifiers.Hostile * (float)delta);
							break;
						case MyRelationsBetweenFactions.Friends:
							num3 = (int)(reputationModifiers.Friend * (float)delta);
							break;
						default:
							continue;
						}
						relationBetweenPlayerAndFaction = GetRelationBetweenPlayerAndFaction(playerId, current.Value.FactionId);
						num = ClampReputation(relationBetweenPlayerAndFaction.Item2 + num3);
						num2 = num - relationBetweenPlayerAndFaction.Item2;
						bool flag2 = !adminChange && CheckIfMaxPirateRep(playerId, current.Value, num2);
						if (num2 != 0 && !flag2)
						{
							item = new MyReputationChangeWrapper
							{
								FactionId = current.Value.FactionId,
								RepTotal = num,
								Change = num2,
								ShowNotification = false
							};
							list.Add(item);
						}
					}
				}
				return list;
			}
			return list;
		}

		/// <summary>
		/// Checks if player reached max reputation he can gain per settings defined.
		/// </summary>
		/// <param name="playerId">Player id.</param>
		/// <param name="faction">Faction to change rep for.</param>
		/// <param name="clampedDelta">Change that will be happening.</param>
		/// <returns></returns>
		private bool CheckIfMaxPirateRep(long playerId, IMyFaction faction, int clampedDelta)
		{
			long piratesId = MyPirateAntennas.GetPiratesId();
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(piratesId);
			if (clampedDelta > 0 && faction == playerFaction)
			{
				if (!m_playerToReputationLimits.TryGetValue(playerId, out var value))
				{
					value = new Tuple<int, TimeSpan>(clampedDelta, MySession.Static.ElapsedGameTime + TimeSpan.FromMinutes(m_reputationSettings.ResetTimeMinForRepGain));
					m_playerToReputationLimits.Add(playerId, value);
					return false;
				}
				if (value.Item2 > MySession.Static.ElapsedGameTime)
				{
					if (value.Item1 < m_reputationSettings.MaxReputationGainInTime)
					{
						int item = Math.Min(value.Item1 + clampedDelta, m_reputationSettings.MaxReputationGainInTime);
						m_playerToReputationLimits[playerId] = new Tuple<int, TimeSpan>(item, value.Item2);
						return false;
					}
					return true;
				}
				int item2 = Math.Min(clampedDelta, m_reputationSettings.MaxReputationGainInTime);
				m_playerToReputationLimits[playerId] = new Tuple<int, TimeSpan>(item2, MySession.Static.ElapsedGameTime + TimeSpan.FromMinutes(m_reputationSettings.ResetTimeMinForRepGain));
				return false;
			}
			return false;
		}

		private MyReputationModifiers GetReputationModifiers(bool positive = true)
		{
			return MySession.Static.GetComponent<MySessionComponentEconomy>()?.GetReputationModifiers(positive) ?? default(MyReputationModifiers);
		}

<<<<<<< HEAD
		[Event(null, 1276)]
=======
		[Event(null, 1265)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private static void AddFactionPlayerReputationSuccess(long playerId, List<MyReputationChangeWrapper> changes)
		{
			MyFactionCollection factions = MySession.Static.Factions;
			bool flag = !Sandbox.Engine.Platform.Game.IsDedicated && playerId == MySession.Static.LocalPlayerId;
			foreach (MyReputationChangeWrapper change in changes)
			{
				factions.ChangeReputationWithPlayer(playerId, change.FactionId, change.RepTotal);
				if (change.ShowNotification && flag)
				{
					MyFaction myFaction = MySession.Static.Factions.TryGetFactionById(change.FactionId) as MyFaction;
					MySession.Static.Factions.DisplayReputationChangeNotification(myFaction.Tag, change.Change);
				}
			}
		}

		private void DisplayReputationChangeNotification(string tag, int value)
		{
			if (value > 0)
			{
				m_notificationRepInc.UpdateReputationNotification(in tag, in value);
			}
			else if (value < 0)
			{
				m_notificationRepDec.UpdateReputationNotification(in tag, in value);
			}
		}

		private static void SendFactionChange(MyFactionStateChange action, long fromFactionId, long toFactionId, long playerId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => FactionStateChangeRequest, action, fromFactionId, toFactionId, playerId);
		}

<<<<<<< HEAD
		[Event(null, 1311)]
=======
		[Event(null, 1300)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void FactionStateChangeRequest(MyFactionStateChange action, long fromFactionId, long toFactionId, long playerId)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(fromFactionId);
			IMyFaction myFaction2 = MySession.Static.Factions.TryGetFactionById(toFactionId);
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			if (myFaction == null || myFaction2 == null || !MySession.Static.Factions.CheckFactionStateChange(action, fromFactionId, toFactionId, playerId, num))
			{
				return;
			}
			if ((action == MyFactionStateChange.FactionMemberKick || action == MyFactionStateChange.FactionMemberLeave) && myFaction.Members.Count == 1 && MySession.Static.BlockLimitsEnabled != MyBlockLimitsEnabledEnum.PER_FACTION)
			{
				action = MyFactionStateChange.RemoveFaction;
			}
			else
			{
				switch (action)
				{
				case MyFactionStateChange.FactionMemberSendJoin:
				{
					ulong num2 = MySession.Static.Players.TryGetSteamId(playerId);
					bool flag = MySession.Static.IsUserSpaceMaster(num2);
					if (myFaction2.AutoAcceptMember || myFaction2.Members.Count == 0)
					{
						flag = true;
						if (!myFaction2.AcceptHumans && num2 != 0L && MySession.Static.Players.TryGetSerialId(playerId) == 0)
						{
							flag = false;
							action = MyFactionStateChange.FactionMemberCancelJoin;
						}
					}
					if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION && !MyBlockLimits.IsFactionChangePossible(playerId, myFaction2.FactionId))
					{
						flag = false;
						action = MyFactionStateChange.FactionMemberNotPossibleJoin;
					}
					if (flag)
					{
						action = MyFactionStateChange.FactionMemberAcceptJoin;
					}
					break;
				}
				case MyFactionStateChange.FactionMemberAcceptJoin:
					if (!MyBlockLimits.IsFactionChangePossible(playerId, myFaction2.FactionId))
					{
						action = MyFactionStateChange.FactionMemberNotPossibleJoin;
					}
					break;
				case MyFactionStateChange.SendPeaceRequest:
					if (myFaction2.AutoAcceptPeace)
					{
						action = MyFactionStateChange.AcceptPeace;
						num = 0L;
					}
					break;
				}
			}
			if (action == MyFactionStateChange.RemoveFaction)
			{
				MyBankingSystem.Static.RemoveAccount(toFactionId);
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => FactionStateChangeSuccess, action, fromFactionId, toFactionId, playerId, num);
		}

<<<<<<< HEAD
		[Event(null, 1386)]
=======
		[Event(null, 1375)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void FactionStateChangeSuccess(MyFactionStateChange action, long fromFactionId, long toFactionId, long playerId, long senderId)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(fromFactionId);
			IMyFaction myFaction2 = MySession.Static.Factions.TryGetFactionById(toFactionId);
			if (myFaction != null && myFaction2 != null)
			{
				MySession.Static.Factions.ApplyFactionStateChange(action, fromFactionId, toFactionId, playerId, senderId);
				MySession.Static.Factions.FactionStateChanged?.Invoke(action, fromFactionId, toFactionId, playerId, senderId);
			}
		}

		internal List<MyObjectBuilder_Faction> SaveFactions()
		{
			List<MyObjectBuilder_Faction> list = new List<MyObjectBuilder_Faction>();
			foreach (KeyValuePair<long, MyFaction> faction in m_factions)
			{
				MyObjectBuilder_Faction objectBuilder = faction.Value.GetObjectBuilder();
				list.Add(objectBuilder);
			}
			return list;
		}

		internal void LoadFactions(List<MyObjectBuilder_Faction> factionBuilders, bool removeOldData = true)
		{
			if (removeOldData)
			{
				Clear();
<<<<<<< HEAD
			}
			if (factionBuilders == null)
			{
				return;
			}
			foreach (MyObjectBuilder_Faction factionBuilder in factionBuilders)
			{
=======
			}
			if (factionBuilders == null)
			{
				return;
			}
			foreach (MyObjectBuilder_Faction factionBuilder in factionBuilders)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (m_factions.ContainsKey(factionBuilder.FactionId))
				{
					continue;
				}
				MyFaction myFaction = new MyFaction(factionBuilder);
				Add(myFaction);
				foreach (KeyValuePair<long, MyFactionMember> member in myFaction.Members)
				{
					AddPlayerToFaction(member.Value.PlayerId, myFaction.FactionId);
				}
			}
		}

		public void InvokePlayerJoined(MyFaction faction, long identityId)
		{
			this.OnPlayerJoined.InvokeIfNotNull(faction, identityId);
		}

		public void InvokePlayerLeft(MyFaction faction, long identityId)
		{
			this.OnPlayerLeft.InvokeIfNotNull(faction, identityId);
		}

		/// <summary>
		/// This method returns station for specific entity. Don't use it on client, because client don't have actual values.
		/// </summary>
		/// <param name="gridEntityId">grid entity id</param>
		/// <returns></returns>
		public MyStation GetStationByGridId(long gridEntityId)
		{
			using (IEnumerator<KeyValuePair<long, MyFaction>> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					foreach (MyStation station in enumerator.Current.Value.Stations)
					{
						if (station.StationEntityId == gridEntityId)
						{
							return station;
						}
					}
				}
			}
			return null;
		}

		/// <summary>
		/// This method returns station for specific entity. Don't use it on client, because client don't have actual values.
		/// </summary>
		/// <param name="stationId">grid entity id</param>
		/// <returns></returns>
		internal MyStation GetStationByStationId(long stationId)
		{
			using (IEnumerator<KeyValuePair<long, MyFaction>> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MyStation stationById = enumerator.Current.Value.GetStationById(stationId);
					if (stationById != null)
					{
						return stationById;
					}
				}
			}
			return null;
		}

		public void ChangeAutoAccept(long factionId, bool autoAcceptMember, bool autoAcceptPeace)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ChangeAutoAcceptRequest, factionId, autoAcceptMember, autoAcceptPeace);
		}

<<<<<<< HEAD
		[Event(null, 1504)]
=======
		[Event(null, 1493)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void ChangeAutoAcceptRequest(long factionId, bool autoAcceptMember, bool autoAcceptPeace)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(factionId);
			ulong value = MyEventContext.Current.Sender.Value;
			long playerId = MySession.Static.Players.TryGetIdentityId(value);
			if (myFaction != null && (myFaction.IsLeader(playerId) || (value != 0L && MySession.Static.IsUserAdmin(value))))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ChangeAutoAcceptSuccess, factionId, autoAcceptMember, autoAcceptPeace);
			}
			else if (!MyEventContext.Current.IsLocallyInvoked)
			{
				((MyMultiplayerServerBase)MyMultiplayer.Static).ValidationFailed(value);
			}
		}

<<<<<<< HEAD
		[Event(null, 1521)]
=======
		[Event(null, 1510)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void ChangeAutoAcceptSuccess(long factionId, bool autoAcceptMember, bool autoAcceptPeace)
		{
			MyFactionCollection factions = MySession.Static.Factions;
			MyFaction myFaction = factions.TryGetFactionById(factionId) as MyFaction;
			if (myFaction != null)
			{
				myFaction.AutoAcceptPeace = autoAcceptPeace;
				myFaction.AutoAcceptMember = autoAcceptMember;
				factions.FactionAutoAcceptChanged.InvokeIfNotNull(factionId, autoAcceptMember, autoAcceptPeace);
			}
		}

		public void EditFaction(long factionId, string tag, string name, string desc, string privateInfo, SerializableDefinitionId? factionIconGroupId = null, int factionIconId = 0, Vector3 factionColor = default(Vector3), Vector3 factionIconColor = default(Vector3), int score = 0, float objectivePercentage = 0f)
		{
			AddFactionMsg addFactionMsg = default(AddFactionMsg);
			addFactionMsg.FactionId = factionId;
			addFactionMsg.FactionTag = tag;
			addFactionMsg.FactionName = name;
			addFactionMsg.FactionDescription = (string.IsNullOrEmpty(desc) ? string.Empty : ((desc.Length > 512) ? desc.Substring(0, 512) : desc));
			addFactionMsg.FactionPrivateInfo = (string.IsNullOrEmpty(privateInfo) ? string.Empty : ((privateInfo.Length > 512) ? privateInfo.Substring(0, 512) : privateInfo));
			addFactionMsg.FactionColor = factionColor;
			addFactionMsg.FactionIconColor = factionIconColor;
			addFactionMsg.FactionIconGroupId = factionIconGroupId;
			addFactionMsg.FactionIconId = factionIconId;
			addFactionMsg.FactionScore = score;
			addFactionMsg.FactionObjectivePercentageCompleted = objectivePercentage;
			AddFactionMsg arg = addFactionMsg;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => EditFactionRequest, arg);
		}

<<<<<<< HEAD
		[Event(null, 1563)]
=======
		[Event(null, 1551)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void EditFactionRequest(AddFactionMsg msgEdit)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(msgEdit.FactionId);
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			if (myFaction != null && !MySession.Static.Factions.FactionTagExists(msgEdit.FactionTag, myFaction) && !MySession.Static.Factions.FactionNameExists(msgEdit.FactionName, myFaction) && (myFaction.IsLeader(num) || MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value)))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => EditFactionSuccess, msgEdit, num);
			}
			else if (!MyEventContext.Current.IsLocallyInvoked)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
		}

<<<<<<< HEAD
		[Event(null, 1579)]
=======
		[Event(null, 1567)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void EditFactionSuccess(AddFactionMsg msgEdit, long senderId)
		{
			MyFaction myFaction = MySession.Static.Factions.TryGetFactionById(msgEdit.FactionId) as MyFaction;
			if (myFaction != null)
			{
				MySession.Static.Factions.UnregisterFactionTag(myFaction);
				string str = string.Empty;
				WorkshopId? workshopId = null;
				if (msgEdit.FactionIconGroupId.HasValue)
				{
					str = GetFactionIcon(msgEdit.FactionIconGroupId.Value, msgEdit.FactionIconId, out workshopId);
				}
				myFaction.Tag = msgEdit.FactionTag;
				myFaction.Name = msgEdit.FactionName;
				myFaction.Description = msgEdit.FactionDescription;
				myFaction.PrivateInfo = msgEdit.FactionPrivateInfo;
				myFaction.FactionIcon = MyStringId.GetOrCompute(str);
				myFaction.FactionIconWorkshopId = workshopId;
				myFaction.RefreshIconPaths();
				myFaction.CustomColor = msgEdit.FactionColor;
				myFaction.IconColor = msgEdit.FactionIconColor;
				myFaction.Score = msgEdit.FactionScore;
				myFaction.ObjectivePercentageCompleted = msgEdit.FactionObjectivePercentageCompleted;
				MySession.Static.Factions.RegisterFactionTag(myFaction);
				MySession.Static.Factions.FactionEdited?.Invoke(msgEdit.FactionId);
				Action<long, bool> factionClientChanged = MySession.Static.Factions.FactionClientChanged;
				bool arg = senderId == MySession.Static.LocalPlayerId;
				factionClientChanged?.Invoke(msgEdit.FactionId, arg);
			}
		}

		[Event(null, 1621)]
		[Reliable]
		[Server]
		public static void RequestFactionScoreAndPercentageUpdate(long factionId)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(factionId);
			if (myFaction != null)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RecieveFactionScoreAndPercentage, factionId, myFaction.Score, myFaction.ObjectivePercentageCompleted, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 1630)]
		[Reliable]
		[Client]
		private static void RecieveFactionScoreAndPercentage(long factionId, int score, float percentageFinished)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(factionId);
			if (myFaction != null)
			{
				myFaction.Score = score;
				myFaction.ObjectivePercentageCompleted = percentageFinished;
			}
		}

<<<<<<< HEAD
		public void CreateFaction(long founderId, string tag, string name, string desc, string privateInfo, MyFactionTypes type, Vector3 factionColor = default(Vector3), Vector3 factionIconColor = default(Vector3), SerializableDefinitionId? factionIconGroupId = null, int factionIconId = 0, WorkshopId? factionIconWorkshopId = null)
=======
		[Event(null, 1599)]
		[Reliable]
		[Server]
		public static void RequestFactionScoreAndPercentageUpdate(long factionId)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(factionId);
			if (myFaction != null)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RecieveFactionScoreAndPercentage, factionId, myFaction.Score, myFaction.ObjectivePercentageCompleted, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 1608)]
		[Reliable]
		[Client]
		private static void RecieveFactionScoreAndPercentage(long factionId, int score, float percentageFinished)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(factionId);
			if (myFaction != null)
			{
				myFaction.Score = score;
				myFaction.ObjectivePercentageCompleted = percentageFinished;
			}
		}

		public void CreateFaction(long founderId, string tag, string name, string desc, string privateInfo, MyFactionTypes type, Vector3 factionColor = default(Vector3), Vector3 factionIconColor = default(Vector3), SerializableDefinitionId? factionIconGroupId = null, int factionIconId = 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			SendCreateFaction(founderId, tag, name, desc, privateInfo, type, factionColor, factionIconColor, factionIconGroupId, factionIconId, factionIconWorkshopId);
		}

		public void CreateNPCFaction(string tag, string name, string desc, string privateInfo)
		{
			SendCreateFaction(-1L, tag, name, desc, privateInfo, MyFactionTypes.None, default(Vector3), default(Vector3), null, 0, null, isNPCFaction: true);
		}

		private void Add(MyFaction faction)
		{
			m_factions.Add(faction.FactionId, faction);
			RegisterFactionTag(faction);
		}

		private void SendCreateFaction(long founderId, string factionTag, string factionName, string factionDesc, string factionPrivate, MyFactionTypes type, Vector3 factionColor, Vector3 factionIconColor, SerializableDefinitionId? factionIconGroupId = null, int factionIconId = 0, WorkshopId? factionIconWorkshopId = null, bool isNPCFaction = false)
		{
			AddFactionMsg arg = default(AddFactionMsg);
			arg.FounderId = founderId;
			arg.FactionTag = factionTag;
			arg.FactionName = factionName;
			arg.FactionDescription = factionDesc;
			arg.FactionPrivateInfo = factionPrivate;
			arg.Type = type;
			arg.FactionColor = factionColor;
			arg.FactionIconColor = factionIconColor;
			arg.FactionIconGroupId = factionIconGroupId;
			arg.FactionIconId = factionIconId;
			arg.isNPCFaction = isNPCFaction;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => CreateFactionRequest, arg);
		}

<<<<<<< HEAD
		[Event(null, 1682)]
=======
		[Event(null, 1663)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void CreateFactionRequest(AddFactionMsg msg)
		{
			if (MySession.Static.MaxFactionsCount == 0 || (MySession.Static.MaxFactionsCount > 0 && MySession.Static.Factions.HumansCount() < MySession.Static.MaxFactionsCount))
			{
				if (msg.isNPCFaction)
				{
					string name = msg.FactionTag + " NPC" + MyRandom.Instance.Next(1000, 9999);
					MyIdentity myIdentity = Sync.Players.CreateNewIdentity(name);
					Sync.Players.MarkIdentityAsNPC(myIdentity.IdentityId);
					msg.FounderId = myIdentity.IdentityId;
					MySession.Static.Factions.KickPlayerFromFaction(myIdentity.IdentityId);
				}
				CreateFactionServer(msg.FounderId, msg.FactionTag, msg.FactionName, msg.FactionDescription, msg.FactionPrivateInfo, null, msg.Type, msg.FactionIconGroupId, msg.FactionIconId, msg.FactionColor, msg.FactionIconColor);
			}
			else if (!MyEventContext.Current.IsLocallyInvoked)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Creates faction on server and sends message to client to create one there. If faction definition is provided than faction will be created from definition and
		/// faction name, description and private info will be taken from definition.
		/// </summary>
		/// <param name="founderId">Founder id</param>
		/// <param name="factionTag">Faction tag</param>
		/// <param name="factionName">Faction name</param>
		/// <param name="description">Faction Description</param>
		/// <param name="privateInfo">Private info</param>
		/// <param name="factionDef">Optional: faction definition.</param>
		/// <param name="type"></param>
		/// <param name="factionIconGroupId"></param>
		/// <param name="factionIconId"></param>
		/// <param name="factionColor"></param>
		/// <param name="factionIconColor"></param>
		/// <param name="score"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static void CreateFactionServer(long founderId, string factionTag, string factionName, string description, string privateInfo, MyFactionDefinition factionDef = null, MyFactionTypes type = MyFactionTypes.None, SerializableDefinitionId? factionIconGroupId = null, int factionIconId = 0, Vector3 factionColor = default(Vector3), Vector3 factionIconColor = default(Vector3), int score = 0)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			long num = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.FACTION);
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(num);
			if (MySession.Static.Factions.TryGetPlayerFaction(founderId) == null && myFaction == null && !MySession.Static.Factions.FactionTagExists(factionTag) && !MySession.Static.Factions.FactionNameExists(factionName) && Sync.Players.HasIdentity(founderId))
			{
				bool flag = ((factionDef != null) ? true : false);
				bool flag2 = false;
				if ((!flag) ? CreateFactionInternal(founderId, num, factionTag, factionName, description, privateInfo, type, factionColor, factionIconColor, factionIconGroupId, factionIconId, score) : CreateFactionInternal(founderId, num, factionDef))
				{
					MyBankingSystem.Static.CreateAccount(num, 0L);
					FactionCreationFinished(num, founderId, factionTag, factionName, description, privateInfo, flag, type, factionIconGroupId, factionIconId, factionColor, factionIconColor, score);
				}
			}
		}

		public static bool GetDefinitionIdsByIconName(string iconName, out SerializableDefinitionId? factionIconGroupId, out int factionIconId)
		{
			factionIconGroupId = null;
			factionIconId = 0;
			IEnumerable<MyFactionIconsDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyFactionIconsDefinition>();
			if (allDefinitions == null)
			{
				return false;
			}
			int num = 0;
			foreach (MyFactionIconsDefinition item in allDefinitions)
			{
				num = 0;
				string[] icons = item.Icons;
				foreach (string b in icons)
				{
					if (string.Equals(iconName, b))
					{
						factionIconGroupId = item.Id;
						factionIconId = num;
						return true;
					}
					num++;
				}
			}
			return false;
		}

		public static bool CanPlayerUseFactionIcon(SerializableDefinitionId factionIconGroupId, int factionIconId, long identityId, out string icon, out WorkshopId? iconWorkshopId)
		{
			ulong num = MySession.Static.Players.TryGetSteamId(identityId);
			if (num == 0L)
			{
				icon = string.Empty;
				iconWorkshopId = null;
				return false;
			}
			return CanPlayerUseFactionIcon(factionIconGroupId, factionIconId, num, out icon, out iconWorkshopId);
		}

		public static bool CanPlayerUseFactionIcon(SerializableDefinitionId factionIconGroupId, int factionIconId, ulong steamId, out string icon, out WorkshopId? iconWorkshopId)
		{
			iconWorkshopId = null;
			MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(factionIconGroupId);
			if (definition != null && Enumerable.Count<string>((IEnumerable<string>)definition.Icons) > factionIconId)
			{
				if (!definition.Context.IsBaseGame)
				{
					iconWorkshopId = definition.Context.ModItem.GetWorkshopId();
				}
				if (!(definition.Id.SubtypeId.String == "Other"))
				{
					icon = definition.Icons[factionIconId];
					return true;
				}
				if (MySession.Static.GetComponent<MySessionComponentDLC>().HasDLC("Economy", steamId))
				{
					icon = definition.Icons[factionIconId];
					return true;
				}
			}
			icon = "";
			return false;
		}

		public static string GetFactionIcon(SerializableDefinitionId factionIconGroupId, int factionIconId, out WorkshopId? workshopId)
		{
			workshopId = null;
			if (factionIconGroupId.IsNull())
			{
				return string.Empty;
			}
			MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(factionIconGroupId);
			if (definition != null && Enumerable.Count<string>((IEnumerable<string>)definition.Icons) > factionIconId)
			{
				if (!definition.Context.IsBaseGame)
				{
					workshopId = definition.Context.ModItem.GetWorkshopId();
				}
				return definition.Icons[factionIconId];
			}
			return string.Empty;
		}

		public static void FactionCreationFinished(long factionId, long founderId, string factionTag, string factionName, string description, string privateInfo, bool createFromDef = false, MyFactionTypes type = MyFactionTypes.None, SerializableDefinitionId? factionIconGroupId = null, int factionIconId = 0, Vector3 factionColor = default(Vector3), Vector3 factionIconColor = default(Vector3), int score = 0)
		{
			AddFactionMsg arg = default(AddFactionMsg);
			arg.FactionId = factionId;
			arg.FounderId = founderId;
			arg.FactionTag = factionTag;
			arg.FactionName = factionName;
			arg.FactionDescription = description;
			arg.FactionPrivateInfo = privateInfo;
			arg.CreateFromDefinition = createFromDef;
			arg.FactionColor = factionColor;
			arg.FactionIconColor = factionIconColor;
			arg.Type = type;
			arg.FactionIconGroupId = factionIconGroupId;
			arg.FactionIconId = factionIconId;
			arg.FactionScore = score;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CreateFactionSuccess, arg);
			SetDefaultFactionStates(factionId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetDefaultFactionStates, factionId);
		}

<<<<<<< HEAD
		[Event(null, 1873)]
=======
		[Event(null, 1820)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private static void CreateFactionSuccess(AddFactionMsg msg)
		{
			if (msg.CreateFromDefinition)
			{
				MyFactionDefinition myFactionDefinition = MyDefinitionManager.Static.TryGetFactionDefinition(msg.FactionTag);
				if (myFactionDefinition != null)
				{
					CreateFactionInternal(msg.FounderId, msg.FactionId, myFactionDefinition);
				}
			}
			else
			{
				CreateFactionInternal(msg.FounderId, msg.FactionId, msg.FactionTag, msg.FactionName, msg.FactionDescription, msg.FactionPrivateInfo, msg.Type, msg.FactionColor, msg.FactionIconColor, msg.FactionIconGroupId, msg.FactionIconId, msg.FactionScore);
			}
		}

		/// <summary>
		/// Creates faction from definition.
		/// </summary>
		/// <param name="founderId">Identity id of the owner.</param>
		/// <param name="factionId">Faction id to be used for the faction.</param>
		/// <param name="factionDef">Faction definition.</param>
		/// <param name="customColor"></param>
		/// <param name="iconColor"></param>
		/// <returns>If true than faction was created.</returns>
		public static bool CreateFactionInternal(long founderId, long factionId, MyFactionDefinition factionDef, Vector3? customColor = null, Vector3? iconColor = null)
		{
			if (MySession.Static.Factions.Contains(factionId))
			{
				return false;
			}
			if (MySession.Static.MaxFactionsCount > 0 && MySession.Static.Factions.HumansCount() >= MySession.Static.MaxFactionsCount)
			{
				return false;
			}
			MyFaction faction = new MyFaction(factionId, founderId, "", factionDef, customColor, iconColor);
			MySession.Static.Factions.Add(faction);
			MySession.Static.Factions.AddPlayerToFaction(founderId, factionId);
			AfterFactionCreated(founderId, factionId);
			return true;
		}

		private static bool CreateFactionInternal(long founderId, long factionId, string factionTag, string factionName, string factionDescription, string factionPrivateInfo, MyFactionTypes type, Vector3 factionColor, Vector3? factionIconColor = null, SerializableDefinitionId? factionIconGroupId = null, int factionIconId = 0, int factionScore = 0)
		{
			if (MySession.Static.MaxFactionsCount > 0 && MySession.Static.Factions.HumansCount() >= MySession.Static.MaxFactionsCount)
			{
				return false;
			}
			string icon = string.Empty;
			WorkshopId? iconWorkshopId = null;
			if (factionIconGroupId.HasValue)
			{
				if (Sync.IsServer)
				{
					if (!CanPlayerUseFactionIcon(factionIconGroupId.Value, factionIconId, founderId, out icon, out iconWorkshopId))
					{
						return false;
					}
				}
				else
				{
					icon = GetFactionIcon(factionIconGroupId.Value, factionIconId, out iconWorkshopId);
				}
			}
			MySession.Static.Factions.AddPlayerToFaction(founderId, factionId);
<<<<<<< HEAD
			MySession.Static.Factions.Add(new MyFaction(factionId, factionTag, factionName, factionDescription, factionPrivateInfo, founderId, type, factionColor, factionIconColor, icon, iconWorkshopId, factionScore));
=======
			MySession.Static.Factions.Add(new MyFaction(factionId, factionTag, factionName, factionDescription, factionPrivateInfo, founderId, type, factionColor, factionIconColor, icon, factionScore));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			AfterFactionCreated(founderId, factionId);
			return true;
		}

		/// <summary>
		/// Determines what kind of faction change request should be used depending on default faction
		/// relation to other ones.
		/// </summary>
		/// <returns></returns>
		private static MyFactionStateChange DetermineRequestFromRelation(MyRelationsBetweenFactions relation)
		{
			return relation switch
			{
				MyRelationsBetweenFactions.Enemies => MyFactionStateChange.DeclareWar, 
				MyRelationsBetweenFactions.Friends => MyFactionStateChange.SendFriendRequest, 
				_ => MyFactionStateChange.SendPeaceRequest, 
			};
		}

		private static void AfterFactionCreated(long founderId, long factionId)
		{
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				faction.Value.CancelJoinRequest(founderId);
			}
			MySession.Static.Factions.FactionCreated?.Invoke(factionId);
			Action<long, bool> factionClientChanged = MySession.Static.Factions.FactionClientChanged;
			bool arg = founderId == MySession.Static.LocalPlayerId;
			factionClientChanged?.Invoke(factionId, arg);
		}

<<<<<<< HEAD
		[Event(null, 2003)]
=======
		[Event(null, 1941)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private static void SetDefaultFactionStates(long recivedFactionId)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(recivedFactionId);
			MyFactionDefinition myFactionDefinition = MyDefinitionManager.Static.TryGetFactionDefinition(myFaction.Tag);
			MyFaction myFaction2 = myFaction as MyFaction;
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				MyFaction value = faction.Value;
				if (value.FactionId == recivedFactionId)
				{
					continue;
				}
				if (ShouldForceRelationToFactions(value, myFaction2))
				{
					MySession.Static.Factions.ForceRelationToFactions(value, myFaction2);
					continue;
<<<<<<< HEAD
=======
				}
				if (myFactionDefinition != null)
				{
					SetDefaultFactionStateInternal(value.FactionId, myFaction, myFactionDefinition);
					continue;
				}
				MyFactionDefinition myFactionDefinition2 = MyDefinitionManager.Static.TryGetFactionDefinition(value.Tag);
				if (myFactionDefinition2 != null)
				{
					SetDefaultFactionStateInternal(recivedFactionId, value, myFactionDefinition2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				if (myFactionDefinition != null)
				{
					SetDefaultFactionStateInternal(value.FactionId, myFaction, myFactionDefinition);
					continue;
				}
				MyFactionDefinition myFactionDefinition2 = MyDefinitionManager.Static.TryGetFactionDefinition(value.Tag);
				if (myFactionDefinition2 != null)
				{
					SetDefaultFactionStateInternal(recivedFactionId, value, myFactionDefinition2);
				}
			}
			if (!ShouldForceRelationToPlayers(myFaction2))
			{
				return;
			}
<<<<<<< HEAD
			foreach (MyIdentity allIdentity in MySession.Static.Players.GetAllIdentities())
			{
=======
			if (!ShouldForceRelationToPlayers(myFaction2))
			{
				return;
			}
			foreach (MyIdentity allIdentity in MySession.Static.Players.GetAllIdentities())
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!MySession.Static.Players.IdentityIsNpc(allIdentity.IdentityId))
				{
					MySession.Static.Factions.ForceRelationToPlayers(myFaction2, allIdentity.IdentityId);
				}
			}
		}

		public bool ForceRelationToPlayers(MyFaction faction, long playerId)
		{
			bool flag = false;
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component == null)
			{
				return false;
			}
			MyRelationsBetweenFactions relation;
			if ((MyDefinitionManager.Static.GetDefinition(component.EconomyDefinition.PirateId) as MyFactionDefinition).Tag == faction.Tag)
			{
				ChangeReputationWithPlayer(playerId, faction.FactionId, component.EconomyDefinition.ReputationHostileMin);
				relation = MyRelationsBetweenFactions.Enemies;
				flag = true;
			}
			else
			{
				switch (faction.FactionType)
				{
				case MyFactionTypes.PlayerMade:
					relation = MyRelationsBetweenFactions.Enemies;
					flag = true;
					break;
				case MyFactionTypes.Miner:
				case MyFactionTypes.Trader:
				case MyFactionTypes.Builder:
					if (component != null)
					{
						ChangeReputationWithPlayer(playerId, faction.FactionId, component.GetDefaultReputationPlayer());
						return true;
					}
					relation = MyRelationsBetweenFactions.Neutral;
					flag = true;
					break;
				default:
					relation = MyRelationsBetweenFactions.Enemies;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				ChangeRelationWithPlayer(playerId, faction.FactionId, relation);
				return true;
			}
			return false;
		}

		private static bool ShouldForceRelationToPlayers(MyFaction faction)
		{
			if (faction.FactionType != MyFactionTypes.Miner && faction.FactionType != MyFactionTypes.Trader)
			{
				return faction.FactionType == MyFactionTypes.Builder;
			}
			return true;
		}

		public bool ForceRelationsOnNewIdentity(long identityId)
		{
			if (MySession.Static.Players.IdentityIsNpc(identityId))
			{
				return false;
			}
			using (IEnumerator<KeyValuePair<long, MyFaction>> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<long, MyFaction> current = enumerator.Current;
					if (!HasRelationWithPlayer(current.Value.FactionId, identityId) && ShouldForceRelationToPlayers(current.Value))
					{
						ForceRelationToPlayers(current.Value, identityId);
					}
				}
			}
			return true;
		}

		private bool ForceRelationToFactions(MyFaction faction1, MyFaction faction2)
		{
			bool num = faction1.FactionType == MyFactionTypes.PlayerMade;
			bool flag = faction2.FactionType == MyFactionTypes.PlayerMade;
			MyFaction myFaction;
			MyFaction myFaction2;
			if (num)
			{
				myFaction = faction1;
				myFaction2 = faction2;
			}
			else
			{
				if (!flag)
				{
					SetFactionStateInternal(faction1.FactionId, faction2, MyFactionStateChange.SendPeaceRequest);
					SetFactionStateInternal(faction2.FactionId, faction1, MyFactionStateChange.AcceptPeace);
					return true;
				}
				myFaction = faction2;
				myFaction2 = faction1;
			}
			TupleExtensions.Deconstruct(GetRelationBetweenPlayerAndFaction(myFaction.FounderId, myFaction2.FactionId), out var item, out var item2);
			_ = (float)item2;
			MyRelationsBetweenFactions myRelationsBetweenFactions = item;
			MyFactionStateChange request;
			MyFactionStateChange request2 = (request = MyFactionStateChange.DeclareWar);
			bool flag2 = false;
			switch (myRelationsBetweenFactions)
			{
			case MyRelationsBetweenFactions.Neutral:
				request2 = MyFactionStateChange.SendPeaceRequest;
				request = MyFactionStateChange.AcceptPeace;
				flag2 = true;
				break;
			case MyRelationsBetweenFactions.Enemies:
				request2 = MyFactionStateChange.DeclareWar;
				request = MyFactionStateChange.DeclareWar;
				flag2 = true;
				break;
			case MyRelationsBetweenFactions.Friends:
				request2 = MyFactionStateChange.SendFriendRequest;
				request = MyFactionStateChange.AcceptFriendRequest;
				flag2 = true;
				break;
			}
			if (flag2)
			{
				SetFactionStateInternal(myFaction2.FactionId, myFaction, request2);
				SetFactionStateInternal(myFaction.FactionId, myFaction2, request);
				return true;
			}
			return false;
		}

		private static bool ShouldForceRelationToFactions(MyFaction faction, MyFaction fac)
		{
			bool num = faction.FactionType == MyFactionTypes.Miner || faction.FactionType == MyFactionTypes.Trader || faction.FactionType == MyFactionTypes.Builder;
			bool flag = fac.FactionType == MyFactionTypes.Miner || fac.FactionType == MyFactionTypes.Trader || fac.FactionType == MyFactionTypes.Builder;
			bool flag2 = faction.FactionType == MyFactionTypes.PlayerMade;
			bool flag3 = fac.FactionType == MyFactionTypes.PlayerMade;
			if (num != flag)
			{
				return flag2 != flag3;
			}
			return false;
		}

		/// <summary>
		/// Sets default faction relation on provided faction.
		/// </summary>
		/// <param name="factionId">Faction on which set the default faction relations.</param>
		/// <param name="defaultFaction">Default faction which contains definition of the relation.</param>
		/// <param name="defaultFactionDef">Default faction definition.</param>
		private static void SetDefaultFactionStateInternal(long factionId, IMyFaction defaultFaction, MyFactionDefinition defaultFactionDef)
		{
			MyFactionStateChange myFactionStateChange = DetermineRequestFromRelation(defaultFactionDef.DefaultRelation);
			MySession.Static.Factions.ApplyFactionStateChange(myFactionStateChange, defaultFaction.FactionId, factionId, defaultFaction.FounderId, defaultFaction.FounderId);
			MySession.Static.Factions.FactionStateChanged?.Invoke(myFactionStateChange, defaultFaction.FactionId, factionId, defaultFaction.FounderId, defaultFaction.FounderId);
		}

		private void SetFactionStateInternal(long factionId, IMyFaction faction, MyFactionStateChange request)
		{
			ApplyFactionStateChange(request, faction.FactionId, factionId, faction.FounderId, faction.FounderId);
			MySession.Static.Factions.FactionStateChanged?.Invoke(request, faction.FactionId, factionId, faction.FounderId, faction.FounderId);
		}

		public int HumansCount()
		{
			return Enumerable.Count<KeyValuePair<long, IMyFaction>>(Enumerable.Where<KeyValuePair<long, IMyFaction>>((IEnumerable<KeyValuePair<long, IMyFaction>>)Factions, (Func<KeyValuePair<long, IMyFaction>, bool>)((KeyValuePair<long, IMyFaction> x) => x.Value.AcceptHumans)));
		}

		/// <summary>
		/// Returns if a faction is visited/discovered for given player id. NOTE: Player factions are not on list of discovered factions. Only NPC factions are stored.
		/// </summary>
		/// <param name="playerId">Player id to test for.</param>
		/// <param name="factionId">Faction id to check for.</param>
		/// <returns>Returns true if faction is discovered by player id. Otherwise false.</returns>
		public bool IsFactionDiscovered(MyPlayer.PlayerId playerId, long factionId)
		{
<<<<<<< HEAD
			if (MySession.Static.CreativeToolsEnabled(playerId.SteamId))
			{
				return true;
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!m_playerToFactionsVis.TryGetValue(playerId, out var value))
			{
				return false;
			}
			if (!value.Contains(factionId))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Adds faction id to discovered faction for given player. Call only on server.
		/// </summary>
		/// <param name="playerId">Id of the player.</param>
		/// <param name="factionId">Faction id to be added as discovered.</param>
		/// <param name="triggerEvent"></param>
		public void AddDiscoveredFaction(MyPlayer.PlayerId playerId, long factionId, bool triggerEvent = true)
		{
			if (!Sync.IsServer)
			{
				MyLog.Default.Error("It is illegal to add discovered factions on clients.");
				return;
			}
			AddDiscoveredFaction_Internal(playerId, factionId, triggerEvent);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AddDiscoveredFaction_Clients, playerId.SteamId, playerId.SerialId, factionId);
		}

		/// <summary>
		/// Gets all npc factions currently existing.
		/// </summary>
		/// <returns>List of npc factions.</returns>
		public List<MyFaction> GetNpcFactions()
		{
			List<MyFaction> list = new List<MyFaction>();
			foreach (MyFaction value in m_factions.Values)
			{
				if (IsNpcFaction(value.Tag) && !(value.Tag == "SPID"))
				{
					list.Add(value);
				}
			}
			return list;
		}

<<<<<<< HEAD
		[Event(null, 2300)]
=======
		[Event(null, 2234)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private static void AddDiscoveredFaction_Clients(ulong playerId, int serialId, long factionId)
		{
			MyPlayer.PlayerId playerId2 = new MyPlayer.PlayerId(playerId, serialId);
			MySession.Static.Factions.AddDiscoveredFaction_Internal(playerId2, factionId);
		}

		private void AddDiscoveredFaction_Internal(MyPlayer.PlayerId playerId, long factionId, bool triggerEvent = true)
		{
			if (!m_playerToFactionsVis.TryGetValue(playerId, out var value))
			{
				value = new List<long>();
				m_playerToFactionsVis.Add(playerId, value);
			}
			if (!value.Contains(factionId))
			{
				value.Add(factionId);
				IMyFaction myFaction = TryGetFactionById(factionId);
				MyFaction arg;
				if (triggerEvent && (arg = myFaction as MyFaction) != null)
				{
					this.OnFactionDiscovered?.Invoke(arg, playerId);
				}
			}
		}

		/// <summary>
		/// Removes faction id from discovered faction for given player. Call only on server.
		/// </summary>
		/// <param name="playerId">Id of the player.</param>
		/// <param name="factionId">Faction id to be removed as discovered.</param>
		public void RemoveDiscoveredFaction(MyPlayer.PlayerId playerId, long factionId)
		{
			if (!Sync.IsServer)
			{
				MyLog.Default.Error("It is illegal to removed discovered factions on clients.");
				return;
			}
			RemoveDiscoveredFaction_Internal(playerId, factionId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RemoveDiscoveredFaction_Clients, playerId.SteamId, playerId.SerialId, factionId);
		}

<<<<<<< HEAD
		[Event(null, 2345)]
=======
		[Event(null, 2279)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private static void RemoveDiscoveredFaction_Clients(ulong playerId, int serialId, long factionId)
		{
			MyPlayer.PlayerId playerId2 = new MyPlayer.PlayerId(playerId, serialId);
			MySession.Static.Factions.RemoveDiscoveredFaction_Internal(playerId2, factionId);
		}

		private void RemoveDiscoveredFaction_Internal(MyPlayer.PlayerId playerId, long factionId)
		{
			if (m_playerToFactionsVis.TryGetValue(playerId, out var value))
			{
				value.Remove(factionId);
				if (value.Count == 0)
				{
					m_playerToFactionsVis.Remove(playerId);
				}
			}
		}

		public void RemovePlayerFromVisibility(MyPlayer.PlayerId playerId)
		{
			if (Sync.IsServer && m_playerToFactionsVis.ContainsKey(playerId))
			{
				RemovePlayerFromVisibility_Internal(playerId);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RemovePlayerFromVisibility_Broadcast, playerId);
			}
		}

<<<<<<< HEAD
		[Event(null, 2378)]
=======
		[Event(null, 2312)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		public static void RemovePlayerFromVisibility_Broadcast(MyPlayer.PlayerId playerId)
		{
			MySession.Static?.Factions?.RemovePlayerFromVisibility_Internal(playerId);
		}

		public void RemovePlayerFromVisibility_Internal(MyPlayer.PlayerId playerId)
		{
			if (m_playerToFactionsVis.ContainsKey(playerId))
			{
				m_playerToFactionsVis.Remove(playerId);
			}
		}

		public void RemoveFactionFromVisibility(long factionId)
		{
			if (Sync.IsServer)
			{
				RemoveFactionFromVisibility_Internal(factionId);
			}
		}

		public void RemoveFactionFromVisibility_Internal(long factionId)
		{
			foreach (KeyValuePair<MyPlayer.PlayerId, List<long>> playerToFactionsVi in m_playerToFactionsVis)
			{
				playerToFactionsVi.Value.Remove(factionId);
			}
		}

		public MyObjectBuilder_FactionCollection GetObjectBuilder()
		{
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			MyObjectBuilder_FactionCollection myObjectBuilder_FactionCollection = new MyObjectBuilder_FactionCollection();
			myObjectBuilder_FactionCollection.Factions = new List<MyObjectBuilder_Faction>(m_factions.Count);
			foreach (KeyValuePair<long, MyFaction> faction in m_factions)
			{
				myObjectBuilder_FactionCollection.Factions.Add(faction.Value.GetObjectBuilder());
			}
			myObjectBuilder_FactionCollection.Players = new SerializableDictionary<long, long>();
			foreach (KeyValuePair<long, long> item4 in m_playerFaction)
			{
				myObjectBuilder_FactionCollection.Players.Dictionary.Add(item4.Key, item4.Value);
			}
			myObjectBuilder_FactionCollection.Relations = new List<MyObjectBuilder_FactionRelation>(m_relationsBetweenFactions.Count);
			foreach (KeyValuePair<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> relationsBetweenFaction in m_relationsBetweenFactions)
			{
				MyObjectBuilder_FactionRelation item = default(MyObjectBuilder_FactionRelation);
				item.FactionId1 = relationsBetweenFaction.Key.RelateeId1;
				item.FactionId2 = relationsBetweenFaction.Key.RelateeId2;
				item.Relation = relationsBetweenFaction.Value.Item1;
				item.Reputation = relationsBetweenFaction.Value.Item2;
				myObjectBuilder_FactionCollection.Relations.Add(item);
			}
			myObjectBuilder_FactionCollection.Requests = new List<MyObjectBuilder_FactionRequests>();
			foreach (KeyValuePair<long, HashSet<long>> factionRequest in m_factionRequests)
			{
				List<long> list = new List<long>(factionRequest.Value.get_Count());
				Enumerator<long> enumerator5 = m_factionRequests[factionRequest.Key].GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						long current5 = enumerator5.get_Current();
						list.Add(current5);
					}
				}
				finally
				{
					((IDisposable)enumerator5).Dispose();
				}
				myObjectBuilder_FactionCollection.Requests.Add(new MyObjectBuilder_FactionRequests
				{
					FactionId = factionRequest.Key,
					FactionRequests = list
				});
			}
			myObjectBuilder_FactionCollection.RelationsWithPlayers = new List<MyObjectBuilder_PlayerFactionRelation>();
			foreach (KeyValuePair<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> relationsBetweenPlayersAndFaction in m_relationsBetweenPlayersAndFactions)
			{
				MyObjectBuilder_PlayerFactionRelation item2 = default(MyObjectBuilder_PlayerFactionRelation);
				item2.PlayerId = relationsBetweenPlayersAndFaction.Key.RelateeId1;
				item2.FactionId = relationsBetweenPlayersAndFaction.Key.RelateeId2;
				item2.Relation = relationsBetweenPlayersAndFaction.Value.Item1;
				item2.Reputation = relationsBetweenPlayersAndFaction.Value.Item2;
				myObjectBuilder_FactionCollection.RelationsWithPlayers.Add(item2);
			}
			myObjectBuilder_FactionCollection.PlayerToFactionsVis = new List<MyObjectBuilder_FactionsVisEntry>(m_playerToFactionsVis.Count);
			foreach (KeyValuePair<MyPlayer.PlayerId, List<long>> playerToFactionsVi in m_playerToFactionsVis)
			{
				MyObjectBuilder_FactionsVisEntry item3 = default(MyObjectBuilder_FactionsVisEntry);
				item3.IdentityId = Sync.Players.TryGetIdentityId(playerToFactionsVi.Key.SteamId, playerToFactionsVi.Key.SerialId);
				item3.DiscoveredFactions = new List<long>(playerToFactionsVi.Value.Count);
				foreach (long item5 in playerToFactionsVi.Value)
				{
					item3.DiscoveredFactions.Add(item5);
				}
				if (item3.IdentityId != 0L)
				{
					myObjectBuilder_FactionCollection.PlayerToFactionsVis.Add(item3);
				}
<<<<<<< HEAD
				if (item3.IdentityId != 0L)
				{
					myObjectBuilder_FactionCollection.PlayerToFactionsVis.Add(item3);
				}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return myObjectBuilder_FactionCollection;
		}

		public void Init(MyObjectBuilder_FactionCollection builder)
		{
			foreach (MyObjectBuilder_Faction faction in builder.Factions)
			{
				if (!MyBankingSystem.Static.TryGetAccountInfo(faction.FactionId, out var _))
				{
					MyBankingSystem.Static.CreateAccount(faction.FactionId, 0L);
				}
				MySession.Static.Factions.Add(new MyFaction(faction));
			}
			foreach (KeyValuePair<long, long> item3 in builder.Players.Dictionary)
			{
				m_playerFaction.Add(item3.Key, item3.Value);
			}
			MySessionComponentEconomy mySessionComponentEconomy = null;
			if (MySession.Static != null)
			{
				mySessionComponentEconomy = MySession.Static.GetComponent<MySessionComponentEconomy>();
			}
			MyRelationsBetweenFactions item;
			int item2;
			foreach (MyObjectBuilder_FactionRelation relation in builder.Relations)
			{
				MyRelationsBetweenFactions myRelationsBetweenFactions = relation.Relation;
				int num = relation.Reputation;
				if (mySessionComponentEconomy != null)
				{
					TupleExtensions.Deconstruct(mySessionComponentEconomy.ValidateReputationConsistency(myRelationsBetweenFactions, num), out item, out item2);
					myRelationsBetweenFactions = item;
					num = item2;
				}
				m_relationsBetweenFactions.Add(new MyRelatablePair(relation.FactionId1, relation.FactionId2), new Tuple<MyRelationsBetweenFactions, int>(myRelationsBetweenFactions, num));
			}
			foreach (MyObjectBuilder_PlayerFactionRelation relationsWithPlayer in builder.RelationsWithPlayers)
			{
				MyRelationsBetweenFactions myRelationsBetweenFactions = relationsWithPlayer.Relation;
				int num = relationsWithPlayer.Reputation;
				if (mySessionComponentEconomy != null)
				{
					TupleExtensions.Deconstruct(mySessionComponentEconomy.ValidateReputationConsistency(myRelationsBetweenFactions, num), out item, out item2);
					myRelationsBetweenFactions = item;
					num = item2;
				}
				m_relationsBetweenPlayersAndFactions.Add(new MyRelatablePair(relationsWithPlayer.PlayerId, relationsWithPlayer.FactionId), new Tuple<MyRelationsBetweenFactions, int>(myRelationsBetweenFactions, num));
			}
			foreach (MyObjectBuilder_FactionRequests request in builder.Requests)
			{
				HashSet<long> val = new HashSet<long>();
				foreach (long factionRequest in request.FactionRequests)
				{
					val.Add(factionRequest);
				}
				m_factionRequests.Add(request.FactionId, val);
			}
			if (builder.PlayerToFactionsVis != null)
			{
				m_playerToFactionsVis.Clear();
				foreach (MyObjectBuilder_FactionsVisEntry playerToFactionsVi in builder.PlayerToFactionsVis)
				{
					List<long> list = new List<long>();
					foreach (long discoveredFaction in playerToFactionsVi.DiscoveredFactions)
					{
						list.Add(discoveredFaction);
					}
					if (playerToFactionsVi.IdentityId != 0L)
					{
						if (Sync.Players.TryGetPlayerId(playerToFactionsVi.IdentityId, out var result))
						{
							m_playerToFactionsVis.Add(result, list);
						}
						continue;
					}
					MyPlayer.PlayerId key = new MyPlayer.PlayerId(playerToFactionsVi.PlayerId, playerToFactionsVi.SerialId);
					if (Sync.Players.TryGetIdentityId(key.SteamId, key.SerialId) != 0L)
					{
						m_playerToFactionsVis.Add(key, list);
					}
				}
			}
			m_reputationSettings = MyDefinitionManager.Static.GetDefinition<MyReputationSettingsDefinition>("DefaultReputationSettings");
			m_notificationRepInc = new MyReputationNotification(new MyHudNotification(MySpaceTexts.Economy_Notification_ReputationIncreased, 2500, "Green"));
			m_notificationRepDec = new MyReputationNotification(new MyHudNotification(MySpaceTexts.Economy_Notification_ReputationDecreased, 2500, "Red"));
			CompatDefaultFactions();
		}

		public IEnumerator<KeyValuePair<long, MyFaction>> GetEnumerator()
		{
			return m_factions.GetEnumerator();
		}

		public MyFaction[] GetAllFactions()
		{
<<<<<<< HEAD
			return m_factions.Values.ToArray();
=======
			return Enumerable.ToArray<MyFaction>((IEnumerable<MyFaction>)m_factions.Values);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator<KeyValuePair<long, MyFaction>> IEnumerable<KeyValuePair<long, MyFaction>>.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(KeyValuePair<long, MyFaction> value)
		{
			throw new NotImplementedException();
		}

		public bool GetRandomFriendlyStation(long factionId, long stationId, out MyFaction friendlyFaction, out MyStation friendlyStation, bool includeSameFaction = false)
		{
			friendlyFaction = null;
			friendlyStation = null;
			List<MyFaction> list = new List<MyFaction>();
			using (IEnumerator<KeyValuePair<long, MyFaction>> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<long, MyFaction> current = enumerator.Current;
					if (current.Value.FactionType != 0 && current.Value.FactionType != MyFactionTypes.PlayerMade && ((includeSameFaction && factionId == current.Value.FactionId) || GetRelationBetweenFactions(factionId, current.Value.FactionId).Item1 == MyRelationsBetweenFactions.Friends))
					{
						list.Add(current.Value);
					}
				}
			}
			if (list.Count <= 0)
			{
				return false;
			}
			int num = 0;
			int num2 = 10;
			bool flag = false;
			MyStation myStation;
			MyFaction myFaction;
			do
			{
				num++;
				myFaction = null;
				myStation = null;
				flag = false;
				myFaction = list[MyRandom.Instance.Next(0, list.Count)];
				if (myFaction.Stations.Count > ((myFaction.FactionId == factionId) ? 1 : 0))
				{
					int num3 = MyRandom.Instance.Next(0, myFaction.Stations.Count);
					myStation = Enumerable.ElementAt<MyStation>((IEnumerable<MyStation>)myFaction.Stations, num3);
					if (myStation.Id == stationId)
					{
						myStation = Enumerable.ElementAt<MyStation>((IEnumerable<MyStation>)myFaction.Stations, (num3 + 1) % myFaction.Stations.Count);
					}
					flag = true;
				}
			}
			while (!flag && num <= num2);
			if (!flag)
			{
				return false;
			}
			friendlyFaction = myFaction;
			friendlyStation = myStation;
			return true;
		}

		public void Clear()
		{
			m_factions.Clear();
			m_factionRequests.Clear();
			m_playerFaction.Clear();
			m_factionsByTag.Clear();
			m_playerToFactionsVis.Clear();
			m_playerToReputationLimits.Clear();
			m_relationsBetweenFactions.Clear();
			m_relationsBetweenPlayersAndFactions.Clear();
		}

		bool IMyFactionCollection.FactionTagExists(string tag, IMyFaction doNotCheck)
		{
			return FactionTagExists(tag, doNotCheck);
		}

		bool IMyFactionCollection.FactionNameExists(string name, IMyFaction doNotCheck)
		{
			return FactionNameExists(name, doNotCheck);
		}

		IMyFaction IMyFactionCollection.TryGetFactionById(long factionId)
		{
			return TryGetFactionById(factionId);
		}

		IMyFaction IMyFactionCollection.TryGetPlayerFaction(long playerId)
		{
			return TryGetPlayerFaction(playerId);
		}

		IMyFaction IMyFactionCollection.TryGetFactionByTag(string tag)
		{
			return TryGetFactionByTag(tag);
		}

		IMyFaction IMyFactionCollection.TryGetFactionByName(string name)
		{
			foreach (KeyValuePair<long, MyFaction> faction in m_factions)
			{
				MyFaction value = faction.Value;
				if (string.Equals(name, value.Name, StringComparison.OrdinalIgnoreCase))
				{
					return value;
				}
			}
			return null;
		}

		void IMyFactionCollection.AddPlayerToFaction(long playerId, long factionId)
		{
			AddPlayerToFaction(playerId, factionId);
		}

		void IMyFactionCollection.KickPlayerFromFaction(long playerId)
		{
			KickPlayerFromFaction(playerId);
		}

		MyRelationsBetweenFactions IMyFactionCollection.GetRelationBetweenFactions(long factionId1, long factionId2)
		{
			return GetRelationBetweenFactions(factionId1, factionId2).Item1;
		}

		int IMyFactionCollection.GetReputationBetweenFactions(long factionId1, long factionId2)
		{
			return GetRelationBetweenFactions(factionId1, factionId2).Item2;
		}

		void IMyFactionCollection.SetReputation(long fromFactionId, long toFactionId, int reputation)
		{
			SetReputationBetweenFactions(fromFactionId, toFactionId, ClampReputation(reputation));
		}

		int IMyFactionCollection.GetReputationBetweenPlayerAndFaction(long identityId, long factionId)
		{
			return GetRelationBetweenPlayerAndFaction(identityId, factionId).Item2;
		}

		void IMyFactionCollection.SetReputationBetweenPlayerAndFaction(long identityId, long factionId, int reputation)
		{
			SetReputationBetweenPlayerAndFaction(identityId, factionId, ClampReputation(reputation));
		}

		bool IMyFactionCollection.AreFactionsEnemies(long factionId1, long factionId2)
		{
			return AreFactionsEnemies(factionId1, factionId2);
		}

		bool IMyFactionCollection.IsPeaceRequestStateSent(long myFactionId, long foreignFactionId)
		{
			return IsPeaceRequestStateSent(myFactionId, foreignFactionId);
		}

		bool IMyFactionCollection.IsPeaceRequestStatePending(long myFactionId, long foreignFactionId)
		{
			return IsPeaceRequestStatePending(myFactionId, foreignFactionId);
		}

		void IMyFactionCollection.RemoveFaction(long factionId)
		{
			RemoveFaction(factionId);
		}

		void IMyFactionCollection.SendPeaceRequest(long fromFactionId, long toFactionId)
		{
			SendPeaceRequest(fromFactionId, toFactionId);
		}

		void IMyFactionCollection.CancelPeaceRequest(long fromFactionId, long toFactionId)
		{
			CancelPeaceRequest(fromFactionId, toFactionId);
		}

		void IMyFactionCollection.AcceptPeace(long fromFactionId, long toFactionId)
		{
			AcceptPeace(fromFactionId, toFactionId);
		}

		void IMyFactionCollection.DeclareWar(long fromFactionId, long toFactionId)
		{
			DeclareWar(fromFactionId, toFactionId);
		}

		void IMyFactionCollection.SendJoinRequest(long factionId, long playerId)
		{
			SendJoinRequest(factionId, playerId);
		}

		void IMyFactionCollection.CancelJoinRequest(long factionId, long playerId)
		{
			CancelJoinRequest(factionId, playerId);
		}

		void IMyFactionCollection.AcceptJoin(long factionId, long playerId)
		{
			AcceptJoin(factionId, playerId);
		}

		void IMyFactionCollection.KickMember(long factionId, long playerId)
		{
			KickMember(factionId, playerId);
		}

		void IMyFactionCollection.PromoteMember(long factionId, long playerId)
		{
			PromoteMember(factionId, playerId);
		}

		void IMyFactionCollection.DemoteMember(long factionId, long playerId)
		{
			DemoteMember(factionId, playerId);
		}

		void IMyFactionCollection.MemberLeaves(long factionId, long playerId)
		{
			MemberLeaves(factionId, playerId);
		}

		void IMyFactionCollection.ChangeAutoAccept(long factionId, long playerId, bool autoAcceptMember, bool autoAcceptPeace)
		{
			ChangeAutoAccept(factionId, autoAcceptMember, autoAcceptPeace);
		}

		void IMyFactionCollection.EditFaction(long factionId, string tag, string name, string desc, string privateInfo)
		{
			EditFaction(factionId, tag, name, desc, privateInfo);
		}

		void IMyFactionCollection.CreateFaction(long founderId, string tag, string name, string desc, string privateInfo)
		{
			CreateFaction(founderId, tag, name, desc, privateInfo, MyFactionTypes.None);
		}

		void IMyFactionCollection.CreateFaction(long founderId, string tag, string name, string desc, string privateInfo, MyFactionTypes type)
		{
			CreateFaction(founderId, tag, name, desc, privateInfo, type);
		}

		void IMyFactionCollection.CreateNPCFaction(string tag, string name, string desc, string privateInfo)
		{
			CreateNPCFaction(tag, name, desc, privateInfo);
		}

		MyObjectBuilder_FactionCollection IMyFactionCollection.GetObjectBuilder()
		{
			return GetObjectBuilder();
		}
	}
}
