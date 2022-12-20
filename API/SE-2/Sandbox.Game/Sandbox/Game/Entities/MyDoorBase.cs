using System;
using System.Collections.Generic;
using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
	/// <summary>
	/// GR: Added this class to be used as a base for all door classes. Added only very basic functionallity no new definitions or object builders.
	/// The main issue was that door actions (open / close) couldn't be used in groups because they were not inheriting from same class.
	/// Instead were inheriting directly from MyFunctionalBlock so this class is used in between for common attributes.
	/// </summary>
	[MyCubeBlockType(typeof(MyObjectBuilder_DoorBase))]
	public abstract class MyDoorBase : MyFunctionalBlock
	{
		protected sealed class OpenRequest_003C_003ESystem_Boolean_0023System_Int64 : ICallSite<MyDoorBase, bool, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyDoorBase @this, in bool open, in long identityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OpenRequest(open, identityId);
			}
		}

		protected class m_open_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType open;
				ISyncType result = (open = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyDoorBase)P_0).m_open = (Sync<bool, SyncDirection.BothWays>)open;
				return result;
			}
		}

		protected class m_anyoneCanUse_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType anyoneCanUse;
				ISyncType result = (anyoneCanUse = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyDoorBase)P_0).m_anyoneCanUse = (Sync<bool, SyncDirection.BothWays>)anyoneCanUse;
				return result;
			}
		}

		private bool m_contactCallbackQueued;

		protected readonly Sync<bool, SyncDirection.BothWays> m_open;

		private readonly Sync<bool, SyncDirection.BothWays> m_anyoneCanUse;

		public bool Open
		{
			get
			{
				return m_open;
			}
			set
			{
				if ((bool)m_open != value && Enabled && base.IsWorking && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
				{
					m_open.Value = value;
					ResolveContactEnableSubparts(!value);
				}
			}
		}

		public bool AnyoneCanUse
		{
			get
			{
				return m_anyoneCanUse;
			}
			set
			{
				m_anyoneCanUse.Value = value;
			}
		}

		public override MyCubeBlockHighlightModes HighlightMode
		{
			get
			{
				if (AnyoneCanUse)
				{
					return MyCubeBlockHighlightModes.AlwaysCanUse;
				}
				return MyCubeBlockHighlightModes.Default;
			}
		}

		public MyDoorBase()
		{
			CreateTerminalControls();
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_DoorBase myObjectBuilder_DoorBase = objectBuilder as MyObjectBuilder_DoorBase;
			m_anyoneCanUse.SetLocalValue(myObjectBuilder_DoorBase.AnyoneCanUse);
			if (EnableContactCallbacks())
			{
				base.IsWorkingChanged += delegate
				{
					ResolveContactEnableSubparts();
				};
				base.EnabledChanged += delegate
				{
					ResolveContactEnableSubparts();
				};
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_DoorBase obj = (MyObjectBuilder_DoorBase)base.GetObjectBuilderCubeBlock(copy);
			obj.AnyoneCanUse = m_anyoneCanUse.Value;
			return obj;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyDoorBase>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MyDoorBase> obj = new MyTerminalControlOnOffSwitch<MyDoorBase>("Open", MySpaceTexts.Blank, default(MyStringId), MySpaceTexts.BlockAction_DoorOpen, MySpaceTexts.BlockAction_DoorClosed)
				{
					Getter = (MyDoorBase x) => x.Open,
					Setter = delegate(MyDoorBase x, bool v)
					{
						x.SetOpenRequest(v, x.OwnerId);
					},
					Enabled = (MyDoorBase x) => x.IsWorking
				};
				obj.EnableToggleAction();
				obj.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlCheckbox<MyDoorBase> obj2 = new MyTerminalControlCheckbox<MyDoorBase>("AnyoneCanUse", MySpaceTexts.BlockPropertyText_AnyoneCanUse, MySpaceTexts.BlockPropertyDescription_AnyoneCanUse)
				{
					Getter = (MyDoorBase x) => x.AnyoneCanUse,
					Setter = delegate(MyDoorBase x, bool v)
					{
						x.AnyoneCanUse = v;
					}
				};
				obj2.EnableAction();
				MyTerminalControlFactory.AddControl(obj2);
			}
		}

		public void SetOpenRequest(bool open, long identityId)
		{
			MyMultiplayer.RaiseEvent(this, (MyDoorBase x) => x.OpenRequest, open, identityId);
		}

<<<<<<< HEAD
		[Event(null, 125)]
=======
		[Event(null, 124)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access)]
		private void OpenRequest(bool open, long identityId)
		{
			bool flag = AnyoneCanUse || HasPlayerAccess(identityId);
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			MyPlayer myPlayer = ((myIdentity != null && myIdentity.Character != null) ? MyPlayer.GetPlayerFromCharacter(myIdentity.Character) : null);
			if (myPlayer != null && !flag && MySession.Static.RemoteAdminSettings.TryGetValue(myPlayer.Client.SteamUserId, out var value))
			{
				flag = value.HasFlag(AdminSettingsEnum.UseTerminals);
			}
			if (flag)
			{
				Open = open;
			}
		}

		protected void CreateSubpartConstraint(MyEntity subpart, out HkFixedConstraintData constraintData, out HkConstraint constraint)
		{
			constraintData = null;
			constraint = null;
			if (base.CubeGrid.Physics != null)
			{
				uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(subpart.GetPhysicsBody().RigidBody.Layer, base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, 1, 1);
				subpart.Physics.RigidBody.SetCollisionFilterInfo(collisionFilterInfo);
				if (EnableContactCallbacks())
				{
					subpart.Physics.RigidBody.ContactPointCallback += ContactCallback;
					ResolveContactEnableSubpart(subpart);
				}
				subpart.Physics.Enabled = true;
				constraintData = new HkFixedConstraintData();
				constraintData.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
				constraintData.SetInertiaStabilizationFactor(1f);
				constraint = new HkConstraint((base.CubeGrid.Physics.RigidBody2 != null && base.CubeGrid.Physics.Flags.HasFlag(RigidBodyFlag.RBF_DOUBLED_KINEMATIC)) ? base.CubeGrid.Physics.RigidBody2 : base.CubeGrid.Physics.RigidBody, subpart.Physics.RigidBody, constraintData);
				constraint.WantRuntime = true;
			}
		}

		private void ResolveContactEnableSubparts(bool skipClosingCheck = false)
		{
			bool contactEnableState = GetContactEnableState(skipClosingCheck);
			foreach (KeyValuePair<string, MyEntitySubpart> subpart in base.Subparts)
			{
				if (subpart.Value.Physics != null && subpart.Value.Physics.RigidBody.ContactPointCallbackEnabled != contactEnableState)
				{
					subpart.Value.Physics.RigidBody.ContactPointCallbackEnabled = contactEnableState;
				}
			}
		}

		private void DisableSubpartCallbacks()
		{
			foreach (KeyValuePair<string, MyEntitySubpart> subpart in base.Subparts)
			{
				if (subpart.Value.Physics != null)
				{
					subpart.Value.Physics.RigidBody.ContactPointCallbackEnabled = false;
				}
			}
		}

		private void ResolveContactEnableSubpart(MyEntity subpart)
		{
			bool contactEnableState = GetContactEnableState();
			if (subpart.Physics.RigidBody.ContactPointCallbackEnabled != contactEnableState)
			{
				subpart.Physics.RigidBody.ContactPointCallbackEnabled = contactEnableState;
			}
		}

		private bool GetContactEnableState(bool skipClosingCheck = false)
		{
			if (!EnableContactCallbacks())
			{
				return false;
			}
<<<<<<< HEAD
			if ((bool)m_open || !base.IsWorking || !Enabled)
=======
			if ((bool)m_open || !base.IsWorking || !base.Enabled)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return false;
			}
			if (!skipClosingCheck)
			{
				return IsClosing();
			}
			return true;
		}

		public abstract bool IsClosing();

		private void ContactCallback(ref HkContactPointEvent contactEvent)
		{
			if (!m_contactCallbackQueued)
			{
				m_contactCallbackQueued = true;
				MySandboxGame.Static.Invoke(delegate
				{
					ContactCallbackInternal();
				}, "Door Callback");
			}
		}

		public virtual void ContactCallbackInternal()
		{
			DisableSubpartCallbacks();
			m_contactCallbackQueued = false;
		}

		public virtual bool EnableContactCallbacks()
		{
			return MyFakes.ENABLE_DOOR_SAFETY;
		}

		protected void DisposeSubpartConstraint(ref HkConstraint constraint, ref HkFixedConstraintData constraintData)
		{
			if (!(constraint == null))
			{
				base.CubeGrid.Physics.RemoveConstraint(constraint);
				constraint.Dispose();
				constraint = null;
				constraintData = null;
			}
		}

		protected static void SetupDoorSubpart(MyEntitySubpart subpart, int havokCollisionSystemID, bool refreshInPlace)
		{
			if (subpart != null && subpart.Physics != null && subpart.ModelCollision.HavokCollisionShapes != null && subpart.ModelCollision.HavokCollisionShapes.Length != 0)
			{
				uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(subpart.GetPhysicsBody().RigidBody.Layer, havokCollisionSystemID, 1, 1);
				subpart.Physics.RigidBody.SetCollisionFilterInfo(collisionFilterInfo);
				if (subpart.GetPhysicsBody().HavokWorld != null && refreshInPlace)
				{
					MyPhysics.RefreshCollisionFilter(subpart.GetPhysicsBody());
				}
			}
		}
	}
}
