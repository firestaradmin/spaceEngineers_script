using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Runtime.CompilerServices;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
=======
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_JumpDrive))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyJumpDrive),
		typeof(Sandbox.ModAPI.Ingame.IMyJumpDrive)
	})]
	public class MyJumpDrive : MyFunctionalBlock, Sandbox.ModAPI.IMyJumpDrive, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyJumpDrive
	{
		[Serializable]
		public struct BeaconStub
		{
			protected class Sandbox_Game_Entities_MyJumpDrive_003C_003EBeaconStub_003C_003EId_003C_003EAccessor : IMemberAccessor<BeaconStub, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BeaconStub owner, in long value)
				{
					owner.Id = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BeaconStub owner, out long value)
				{
					value = owner.Id;
				}
			}

			protected class Sandbox_Game_Entities_MyJumpDrive_003C_003EBeaconStub_003C_003EName_003C_003EAccessor : IMemberAccessor<BeaconStub, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BeaconStub owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BeaconStub owner, out string value)
				{
					value = owner.Name;
				}
			}

			public long Id;

			public string Name;

			public static BeaconStub Empty = new BeaconStub
			{
				Id = 0L,
				Name = string.Empty
			};

			public override bool Equals(object obj)
			{
				object obj2;
				if ((obj2 = obj) is BeaconStub)
				{
					return ((BeaconStub)obj2).Id == Id;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (int)Id;
			}
		}

		protected sealed class RequestJumpUpdateBeaconServer_003C_003ESystem_Int64 : ICallSite<MyJumpDrive, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyJumpDrive @this, in long shipControllerId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RequestJumpUpdateBeaconServer(shipControllerId);
			}
		}

		protected sealed class RequestJumpUpdateBeaconCallback_003C_003ESystem_Int64_0023System_String_0023VRageMath_Vector3D : ICallSite<MyJumpDrive, long, string, Vector3D, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyJumpDrive @this, in long shipControllerId, in string beaconText, in Vector3D coords, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RequestJumpUpdateBeaconCallback(shipControllerId, beaconText, coords);
			}
		}

		protected sealed class GetBeaconsInRangeServer_003C_003E : ICallSite<MyJumpDrive, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyJumpDrive @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetBeaconsInRangeServer();
			}
		}

		protected sealed class SetBeaconAsTargetServer_003C_003ESystem_Int64 : ICallSite<MyJumpDrive, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyJumpDrive @this, in long beaconId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetBeaconAsTargetServer(beaconId);
			}
		}

		protected class m_storedPower_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType storedPower;
				ISyncType result = (storedPower = new Sync<float, SyncDirection.FromServer>(P_1, P_2));
				((MyJumpDrive)P_0).m_storedPower = (Sync<float, SyncDirection.FromServer>)storedPower;
				return result;
			}
		}

		protected class m_beaconStubsSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType beaconStubsSync;
				ISyncType result = (beaconStubsSync = new Sync<List<BeaconStub>, SyncDirection.FromServer>(P_1, P_2));
				((MyJumpDrive)P_0).m_beaconStubsSync = (Sync<List<BeaconStub>, SyncDirection.FromServer>)beaconStubsSync;
				return result;
			}
		}

		protected class m_selectedBeaconId_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType selectedBeaconId;
				ISyncType result = (selectedBeaconId = new Sync<long, SyncDirection.BothWays>(P_1, P_2));
				((MyJumpDrive)P_0).m_selectedBeaconId = (Sync<long, SyncDirection.BothWays>)selectedBeaconId;
				return result;
			}
		}

		protected class m_selectedBeaconCoords_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType selectedBeaconCoords;
				ISyncType result = (selectedBeaconCoords = new Sync<Vector3D, SyncDirection.BothWays>(P_1, P_2));
				((MyJumpDrive)P_0).m_selectedBeaconCoords = (Sync<Vector3D, SyncDirection.BothWays>)selectedBeaconCoords;
				return result;
			}
		}

		protected class m_targetSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetSync;
				ISyncType result = (targetSync = new Sync<int?, SyncDirection.BothWays>(P_1, P_2));
				((MyJumpDrive)P_0).m_targetSync = (Sync<int?, SyncDirection.BothWays>)targetSync;
				return result;
			}
		}

		protected class m_jumpDistanceRatio_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType jumpDistanceRatio;
				ISyncType result = (jumpDistanceRatio = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyJumpDrive)P_0).m_jumpDistanceRatio = (Sync<float, SyncDirection.BothWays>)jumpDistanceRatio;
				return result;
			}
		}

		protected class m_initedOnServer_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType initedOnServer;
				ISyncType result = (initedOnServer = new Sync<bool, SyncDirection.FromServer>(P_1, P_2));
				((MyJumpDrive)P_0).m_initedOnServer = (Sync<bool, SyncDirection.FromServer>)initedOnServer;
				return result;
			}
		}

		protected class m_isRecharging_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isRecharging;
				ISyncType result = (isRecharging = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyJumpDrive)P_0).m_isRecharging = (Sync<bool, SyncDirection.BothWays>)isRecharging;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyJumpDrive_003C_003EActor : IActivator, IActivator<MyJumpDrive>
		{
			private sealed override object CreateInstance()
			{
				return new MyJumpDrive();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyJumpDrive CreateInstance()
			{
				return new MyJumpDrive();
			}

			MyJumpDrive IActivator<MyJumpDrive>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly Sync<float, SyncDirection.FromServer> m_storedPower;

		private List<MyBeacon> m_beacons;

		private List<IMyGps> m_beaconGPSList;

		private Sync<List<BeaconStub>, SyncDirection.FromServer> m_beaconStubsSync;

		private long m_highlightedBeaconId;

		private Sync<long, SyncDirection.BothWays> m_selectedBeaconId;

		private Sync<Vector3D, SyncDirection.BothWays> m_selectedBeaconCoords;

		private string m_selectedBeaconName;

		private IMyGps m_selectedGps;

		private IMyGps m_jumpTarget;

		private readonly Sync<int?, SyncDirection.BothWays> m_targetSync;

		private readonly Sync<float, SyncDirection.BothWays> m_jumpDistanceRatio;

		private readonly Sync<bool, SyncDirection.FromServer> m_initedOnServer;

		private bool m_initedLocal;

		private int? m_storedJumpTarget;

		private float m_timeRemaining;

		private ulong m_lastFrameUpdateRequested;

		private readonly Sync<bool, SyncDirection.BothWays> m_isRecharging;

		public bool IsJumping;

		private static readonly string[] m_emissiveTextureNames = new string[4] { "Emissive0", "Emissive1", "Emissive2", "Emissive3" };

		private Color m_prevColor = Color.White;

		private int m_prevFillCount = -1;

		public new MyJumpDriveDefinition BlockDefinition => (MyJumpDriveDefinition)base.BlockDefinition;

		public float CurrentStoredPower
		{
			get
			{
				return m_storedPower;
			}
			set
			{
				if (m_storedPower.Value != value)
				{
					m_storedPower.Value = value;
					UpdateEmissivity();
				}
			}
		}

		public bool CanJump
		{
			get
			{
				if (base.IsWorking && base.IsFunctional)
				{
					return IsFull;
				}
				return false;
			}
		}

		public bool CanJumpIfFull
		{
			get
			{
				if (base.IsWorking)
				{
					return base.IsFunctional;
				}
				return false;
			}
		}

		public bool IsFull => (float)m_storedPower >= BlockDefinition.PowerNeededForJump;

		private float MaxJumpDistanceMeters => (float)Math.Max(base.CubeGrid.GridSystems.JumpSystem.GetMinJumpDistance(base.IDModule.Owner), base.CubeGrid.GridSystems.JumpSystem.GetMaxJumpDistance(base.IDModule.Owner));

		float Sandbox.ModAPI.Ingame.IMyJumpDrive.CurrentStoredPower => m_storedPower;

		float Sandbox.ModAPI.Ingame.IMyJumpDrive.MaxStoredPower => BlockDefinition.PowerNeededForJump;

		float Sandbox.ModAPI.Ingame.IMyJumpDrive.JumpDistanceRatio
		{
			get
			{
				return m_jumpDistanceRatio;
			}
			set
			{
				if (!float.IsNaN(value))
				{
					m_jumpDistanceRatio.Value = MathHelper.Clamp(value, 0f, 1f);
				}
			}
		}

		float Sandbox.ModAPI.Ingame.IMyJumpDrive.JumpDistanceMeters
		{
			get
			{
				return (float)ComputeMaxDistance();
			}
			set
			{
				if (!float.IsNaN(value))
				{
					double num = MaxJumpDistanceMeters;
					double minJumpDistance = base.CubeGrid.GridSystems.JumpSystem.GetMinJumpDistance(base.IDModule.Owner);
					double num2 = MathHelper.Clamp(value, minJumpDistance, num);
					double num3 = 1.0;
					if (num > minJumpDistance)
					{
						num3 = (num2 - minJumpDistance) / (num - minJumpDistance);
					}
					m_jumpDistanceRatio.Value = (float)num3;
				}
			}
		}

		float Sandbox.ModAPI.Ingame.IMyJumpDrive.MaxJumpDistanceMeters => MaxJumpDistanceMeters;

		float Sandbox.ModAPI.Ingame.IMyJumpDrive.MinJumpDistanceMeters => (float)base.CubeGrid.GridSystems.JumpSystem.GetMinJumpDistance(base.IDModule.Owner);

		float Sandbox.ModAPI.IMyJumpDrive.CurrentStoredPower
		{
			get
			{
				return CurrentStoredPower;
			}
			set
			{
				CurrentStoredPower = value;
			}
		}

		MyJumpDriveStatus Sandbox.ModAPI.Ingame.IMyJumpDrive.Status
		{
			get
			{
				if (IsJumping)
				{
					return MyJumpDriveStatus.Jumping;
				}
				if (CanJump)
				{
					return MyJumpDriveStatus.Ready;
				}
				return MyJumpDriveStatus.Charging;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyJumpDrive.Recharge
		{
			get
			{
				return m_isRecharging;
			}
			set
			{
				if ((bool)m_isRecharging != value)
				{
					m_isRecharging.Value = value;
				}
			}
		}

		public bool CanJumpAndHasAccess(long userId)
		{
			if (!CanJump)
			{
				return false;
			}
			return base.IDModule.GetUserRelationToOwner(userId).IsFriendly();
		}

		public bool CanJumpIfFullAndHasAccess(long userId)
		{
			if (!CanJumpIfFull)
			{
				return false;
			}
			return base.IDModule.GetUserRelationToOwner(userId).IsFriendly();
		}

		void Sandbox.ModAPI.IMyJumpDrive.Jump(bool usePilot)
		{
			RequestJump(usePilot);
		}

		public MyJumpDrive()
		{
			CreateTerminalControls();
			m_isRecharging.ValueChanged += delegate
			{
				RaisePropertiesChanged();
			};
			m_targetSync.ValueChanged += delegate
			{
				TargetChanged();
			};
			m_storedPower.AlwaysReject();
		}

		private void TargetChanged()
		{
			if (m_targetSync.Value.HasValue)
			{
				m_jumpTarget = MySession.Static.Gpss.GetGps(m_targetSync.Value.Value);
			}
			else
			{
				m_jumpTarget = null;
			}
			RaisePropertiesChanged();
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyJumpDrive>())
			{
				base.CreateTerminalControls();
				MyTerminalControlButton<MyJumpDrive> obj = new MyTerminalControlButton<MyJumpDrive>("Jump", MySpaceTexts.BlockActionTitle_Jump, MySpaceTexts.Blank, delegate(MyJumpDrive x)
				{
					x.RequestJump();
				})
				{
					Enabled = (MyJumpDrive x) => x.CanJump,
					SupportsMultipleBlocks = false,
					Visible = (MyJumpDrive x) => false
				};
				MyTerminalAction<MyJumpDrive> myTerminalAction = obj.EnableAction(MyTerminalActionIcons.TOGGLE);
				if (myTerminalAction != null)
				{
					myTerminalAction.InvalidToolbarTypes = new List<MyToolbarType>
					{
						MyToolbarType.ButtonPanel,
						MyToolbarType.Character,
						MyToolbarType.Seat
					};
					myTerminalAction.ValidForGroups = false;
				}
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlOnOffSwitch<MyJumpDrive> obj2 = new MyTerminalControlOnOffSwitch<MyJumpDrive>("Recharge", MySpaceTexts.BlockPropertyTitle_Recharge, MySpaceTexts.Blank)
				{
					Getter = (MyJumpDrive x) => x.m_isRecharging,
					Setter = delegate(MyJumpDrive x, bool v)
					{
						x.m_isRecharging.Value = v;
					}
				};
				obj2.EnableToggleAction();
				obj2.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj2);
				MyTerminalControlSlider<MyJumpDrive> myTerminalControlSlider = new MyTerminalControlSlider<MyJumpDrive>("JumpDistance", MySpaceTexts.BlockPropertyTitle_JumpDistance, MySpaceTexts.Blank);
				myTerminalControlSlider.SetLimits(0f, 100f);
				myTerminalControlSlider.DefaultValue = 100f;
				myTerminalControlSlider.Enabled = (MyJumpDrive x) => x.m_jumpTarget == null;
				myTerminalControlSlider.Getter = (MyJumpDrive x) => (float)x.m_jumpDistanceRatio * 100f;
				myTerminalControlSlider.Setter = delegate(MyJumpDrive x, float v)
				{
					x.m_jumpDistanceRatio.Value = v * 0.01f;
				};
				myTerminalControlSlider.Writer = delegate(MyJumpDrive x, StringBuilder v)
				{
					v.AppendFormatedDecimal(MathHelper.RoundOn2(x.m_jumpDistanceRatio) * 100f + "% (", (float)x.ComputeMaxDistance() / 1000f, 0, " km").Append(")");
				};
				myTerminalControlSlider.EnableActions(0.01f);
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<MyJumpDrive>("SelectedTarget", MySpaceTexts.BlockPropertyTitle_DestinationGPS, MySpaceTexts.Blank, multiSelect: false, 1)
				{
					ListContent = delegate(MyJumpDrive x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> focusedItem)
					{
						x.FillSelectedTarget(list1, list2);
					}
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyJumpDrive>("RemoveBtn", MySpaceTexts.RemoveProjectionButton, MySpaceTexts.Blank, delegate(MyJumpDrive x)
				{
					x.RemoveSelected();
				})
				{
					Enabled = (MyJumpDrive x) => x.CanRemove()
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyJumpDrive>("SelectBtn", MyCommonTexts.SelectBlueprint, MySpaceTexts.Blank, delegate(MyJumpDrive x)
				{
					x.SelectTarget();
				})
				{
					Enabled = (MyJumpDrive x) => x.CanSelect()
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<MyJumpDrive>("GpsList", MySpaceTexts.BlockPropertyTitle_GpsLocations, MySpaceTexts.Blank, multiSelect: true)
				{
					ListContent = delegate(MyJumpDrive x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> focusedItem)
					{
						x.FillGpsList(list1, list2);
					},
					ItemSelected = delegate(MyJumpDrive x, List<MyGuiControlListbox.Item> y)
					{
						x.SelectGps(y);
					}
				});
<<<<<<< HEAD
				MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<MyJumpDrive>("BeaconList", MySpaceTexts.BlockPropertyTitle_Beacons, MySpaceTexts.Blank, multiSelect: true)
				{
					ListContent = delegate(MyJumpDrive x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> focusedItem)
					{
						x.FillBeaconList(list1, list2);
					},
					ItemSelected = delegate(MyJumpDrive x, List<MyGuiControlListbox.Item> y)
					{
						x.SelectBeacon(y);
					}
				});
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private bool CanSelect()
		{
			if (m_selectedGps == null)
			{
				return m_highlightedBeaconId > 0;
			}
			return true;
		}

		private void SelectTarget()
		{
			if (CanSelect())
			{
				if (m_selectedGps != null)
				{
					m_targetSync.Value = m_selectedGps.Hash;
					m_selectedBeaconId.Value = 0L;
				}
				else if (m_highlightedBeaconId > 0)
				{
					SetBeaconAsTarget(m_highlightedBeaconId);
				}
			}
		}

		private bool CanRemove()
		{
			if (m_jumpTarget == null)
			{
				return m_selectedBeaconId.Value > 0;
			}
			return true;
		}

		private void RemoveSelected()
		{
			if (CanRemove())
			{
				m_targetSync.Value = null;
				m_selectedBeaconId.Value = 0L;
				m_selectedBeaconCoords.Value = Vector3D.Zero;
			}
		}

		private void RequestJump(bool usePlayer = true)
		{
			if (CanJump)
			{
				if (usePlayer && MySession.Static.LocalCharacter != null)
				{
					MyShipController myShipController = MySession.Static.LocalCharacter.Parent as MyShipController;
					if (myShipController == null && MySession.Static.ControlledEntity != null)
					{
						myShipController = MySession.Static.ControlledEntity.Entity as MyShipController;
					}
					RequestJumpSync(myShipController);
				}
				else if (!usePlayer)
				{
					MyShipController shipController = base.CubeGrid.GridSystems.ControlSystem.GetShipController();
					if (shipController != null)
					{
						RequestJumpSync(shipController);
					}
				}
			}
			else if (!IsJumping && !IsFull)
			{
				MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.NotificationJumpDriveNotFullyCharged, 1500);
				myHudNotification.SetTextFormatArguments(((float)m_storedPower / BlockDefinition.PowerNeededForJump).ToString("P"));
				MyHud.Notifications.Add(myHudNotification);
			}
		}

		private MyBeacon GetSelectedBeaconEntity()
		{
			MyEntities.TryGetEntityById(m_selectedBeaconId, out var entity);
			MyBeacon myBeacon = entity as MyBeacon;
			GetBeaconsInRange();
			if (myBeacon != null && m_beaconStubsSync.Value.FindIndex((BeaconStub x) => x.Id == m_selectedBeaconId.Value) != -1)
			{
				return myBeacon;
			}
			return null;
		}

		private void RequestJumpSync(Sandbox.ModAPI.Ingame.IMyShipController shipController)
		{
			if (m_selectedBeaconId.Value == 0L)
			{
				RequestJumpInternal(shipController);
				return;
			}
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyJumpDrive x) => x.RequestJumpUpdateBeaconServer, shipController.EntityId);
				return;
			}
			MyBeacon selectedBeaconEntity = GetSelectedBeaconEntity();
			if (selectedBeaconEntity != null)
			{
				RequestJumpUpdateBeaconCallback(shipController.EntityId, selectedBeaconEntity.DisplayNameText, selectedBeaconEntity.PositionComp.GetPosition());
			}
			else
			{
				RequestJumpUpdateBeaconCallback(shipController.EntityId, string.Empty, Vector3D.Zero);
			}
		}

		[Event(null, 415)]
		[Reliable]
		[Server]
		private void RequestJumpUpdateBeaconServer(long shipControllerId)
		{
			MyBeacon selectedBeaconEntity = GetSelectedBeaconEntity();
			if (selectedBeaconEntity != null && m_beaconStubsSync.Value.FindIndex((BeaconStub x) => x.Id == m_selectedBeaconId.Value) != -1)
			{
				MyMultiplayer.RaiseEvent(this, (MyJumpDrive x) => RequestJumpUpdateBeaconCallback, shipControllerId, selectedBeaconEntity.DisplayNameText, selectedBeaconEntity.PositionComp.GetPosition(), new EndpointId(MyEventContext.Current.Sender.Value));
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyJumpDrive x) => RequestJumpUpdateBeaconCallback, shipControllerId, string.Empty, Vector3D.Zero, new EndpointId(MyEventContext.Current.Sender.Value));
			}
		}

		[Event(null, 429)]
		[Reliable]
		[Client]
		private void RequestJumpUpdateBeaconCallback(long shipControllerId, string beaconText, Vector3D coords)
		{
			if (coords != Vector3D.Zero)
			{
				m_jumpTarget = new MyGps();
				m_jumpTarget.Coords = coords;
				m_jumpTarget.Name = beaconText;
				m_jumpTarget.UpdateHash();
			}
			else
			{
				m_jumpTarget = null;
			}
			MyEntities.TryGetEntityById(shipControllerId, out var entity);
			if (entity is Sandbox.ModAPI.Ingame.IMyShipController)
			{
				RequestJumpInternal(entity as Sandbox.ModAPI.Ingame.IMyShipController);
			}
		}

		private void RequestJumpInternal(Sandbox.ModAPI.Ingame.IMyShipController shipController)
		{
			GetBeaconsInRange();
			if (m_jumpTarget != null)
			{
				base.CubeGrid.GridSystems.JumpSystem.RequestJump(m_jumpTarget.Name, m_jumpTarget.Coords, shipController.OwnerId, shipController.WorldAABB);
				return;
			}
			Vector3D vector3D = Vector3D.Transform(Base6Directions.GetVector(shipController.Orientation.Forward), shipController.CubeGrid.WorldMatrix.GetOrientation());
			vector3D.Normalize();
			Vector3D destination = base.CubeGrid.WorldMatrix.Translation + vector3D * ComputeMaxDistance();
<<<<<<< HEAD
			base.CubeGrid.GridSystems.JumpSystem.RequestJump(MyTexts.Get(MySpaceTexts.Jump_Blind).ToString(), destination, shipController.OwnerId, shipController.WorldAABB);
=======
			base.CubeGrid.GridSystems.JumpSystem.RequestJump(MyTexts.Get(MySpaceTexts.Jump_Blind).ToString(), destination, shipController.OwnerId);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private double ComputeMaxDistance()
		{
			double maxJumpDistance = base.CubeGrid.GridSystems.JumpSystem.GetMaxJumpDistance(base.IDModule.Owner);
			double minJumpDistance = base.CubeGrid.GridSystems.JumpSystem.GetMinJumpDistance(base.IDModule.Owner);
			if (maxJumpDistance < minJumpDistance)
			{
				return minJumpDistance;
			}
			return minJumpDistance + (maxJumpDistance - minJumpDistance) * (double)(float)m_jumpDistanceRatio;
		}

		private void CreateBeaconGPSList(List<MyBeacon> beacons)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			m_beaconGPSList = new List<IMyGps>();
			foreach (MyBeacon beacon in beacons)
			{
				MyGps myGps = new MyGps();
				myGps.Name = beacon.DisplayNameText;
				myGps.DisplayName = beacon.DisplayNameText;
				myGps.Coords = beacon.PositionComp.GetPosition();
				myGps.UpdateHash();
				m_beaconGPSList.Add(myGps);
			}
		}

		private void FillBeaconList(ICollection<MyGuiControlListbox.Item> beaconItemList, ICollection<MyGuiControlListbox.Item> selectedBeaconItemList)
		{
			List<BeaconStub> list = new List<BeaconStub>();
			GetBeaconsInRange();
			if (m_beaconStubsSync.Value != null)
			{
				list = m_beaconStubsSync.Value;
				if (m_selectedBeaconId.Value > 0 && m_beaconStubsSync.Value.FindIndex((BeaconStub x) => x.Id == m_selectedBeaconId.Value) == -1)
				{
					m_selectedBeaconId.Value = 0L;
					m_jumpTarget = null;
				}
			}
			list.Sort((BeaconStub beacon1, BeaconStub beacon2) => string.Compare(beacon1.Name, beacon2.Name, StringComparison.OrdinalIgnoreCase));
			foreach (BeaconStub item2 in list)
			{
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(item2.Name), null, null, item2);
				beaconItemList.Add(item);
				if (m_highlightedBeaconId == item2.Id)
				{
					selectedBeaconItemList.Add(item);
				}
				if ((long)m_selectedBeaconId == item2.Id)
				{
					m_selectedBeaconName = item2.Name;
				}
			}
		}

		private void FillGpsList(ICollection<MyGuiControlListbox.Item> gpsItemList, ICollection<MyGuiControlListbox.Item> selectedGpsItemList)
		{
			List<IMyGps> list = new List<IMyGps>();
			MySession.Static.Gpss.GetGpsList(MySession.Static.LocalPlayerId, list);
			list.Sort((IMyGps gps1, IMyGps gps2) => string.Compare(gps1.Name, gps2.Name, StringComparison.OrdinalIgnoreCase));
			foreach (IMyGps item2 in list)
			{
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(item2.Name), null, null, item2);
				gpsItemList.Add(item);
				if (m_selectedGps == item2)
				{
					selectedGpsItemList.Add(item);
				}
			}
		}

		private void GetBeaconsInRange()
		{
			ulong simulationFrameCounter = MySandboxGame.Static.SimulationFrameCounter;
			if (simulationFrameCounter <= m_lastFrameUpdateRequested + 99)
			{
				return;
			}
			if (Sync.IsServer)
			{
				GetBeaconsInRangeServer();
				return;
			}
			MyMultiplayer.RaiseEvent(this, (MyJumpDrive x) => x.GetBeaconsInRangeServer);
			m_lastFrameUpdateRequested = simulationFrameCounter;
		}

		[Event(null, 560)]
		[Reliable]
		[Server]
		private void GetBeaconsInRangeServer()
		{
			List<BeaconStub> list = new List<BeaconStub>();
			m_beacons = new List<MyBeacon>();
			BoundingSphereD sphere = new BoundingSphereD(base.PositionComp.GetPosition(), 0.5);
			List<MyDataBroadcaster> list2 = new List<MyDataBroadcaster>();
			MyRadioBroadcasters.GetAllBroadcastersInSphere(sphere, list2);
			HashSet<string> hashSet = new HashSet<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			foreach (MyDataBroadcaster item2 in list2)
			{
				MyBeacon myBeacon = item2.Entity as MyBeacon;
				if (myBeacon != null && myBeacon.RadioBroadcaster.Enabled && (double)myBeacon.RadioBroadcaster.BroadcastRadius >= Vector3D.Distance(base.PositionComp.GetPosition(), myBeacon.PositionComp.GetPosition()))
				{
					string item = ((myBeacon.HudText.Length > 0) ? myBeacon.HudText.ToString() : myBeacon.DisplayNameText);
					if (!hashSet.Add(item))
					{
						hashSet2.Add(item);
					}
				}
			}
			foreach (MyDataBroadcaster item3 in list2)
			{
				MyBeacon myBeacon2 = item3.Entity as MyBeacon;
				if (myBeacon2 != null && myBeacon2.RadioBroadcaster.Enabled && (double)myBeacon2.RadioBroadcaster.BroadcastRadius >= Vector3D.Distance(base.PositionComp.GetPosition(), myBeacon2.PositionComp.GetPosition()))
				{
					string text = ((myBeacon2.HudText.Length > 0) ? myBeacon2.HudText.ToString() : myBeacon2.DisplayNameText);
					if (hashSet2.Contains(text))
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append(text).Append(" [");
						MyValueFormatter.AppendDistanceInBestUnit((float)Vector3D.Distance(myBeacon2.PositionComp.GetPosition(), base.PositionComp.GetPosition()), stringBuilder);
						stringBuilder.Append("]");
						text = stringBuilder.ToString();
					}
					m_beacons.Add(myBeacon2);
					list.Add(new BeaconStub
					{
						Id = myBeacon2.EntityId,
						Name = text
					});
					if (myBeacon2.EntityId == m_selectedBeaconId.Value)
					{
						m_selectedBeaconCoords.Value = myBeacon2.PositionComp.GetPosition();
					}
				}
			}
			m_beaconStubsSync.Value = list;
			CreateBeaconGPSList(m_beacons);
		}

		private void FillSelectedTarget(ICollection<MyGuiControlListbox.Item> selectedTargetList, ICollection<MyGuiControlListbox.Item> emptyList)
		{
			if (m_jumpTarget != null)
			{
				selectedTargetList.Add(new MyGuiControlListbox.Item(new StringBuilder(m_jumpTarget.Name), MyTexts.GetString(MySpaceTexts.BlockActionTooltip_SelectedJumpTarget), null, m_jumpTarget));
			}
			else if (m_selectedBeaconId.Value > 0)
			{
				selectedTargetList.Add(new MyGuiControlListbox.Item(new StringBuilder(m_selectedBeaconName), MyTexts.GetString(MySpaceTexts.BlockActionTooltip_SelectedJumpTarget)));
			}
			else
			{
				selectedTargetList.Add(new MyGuiControlListbox.Item(MyTexts.Get(MySpaceTexts.BlindJump), MyTexts.GetString(MySpaceTexts.BlockActionTooltip_SelectedJumpTarget)));
			}
		}

		private void SelectBeacon(List<MyGuiControlListbox.Item> selection)
		{
			if (selection.Count > 0)
			{
				m_selectedGps = null;
				object userData;
				m_highlightedBeaconId = ((!((userData = selection[0].UserData) is BeaconStub)) ? 0 : ((BeaconStub)userData).Id);
				RaisePropertiesChanged();
			}
		}

		private void SetBeaconAsTarget(long beaconId)
		{
			if (Sync.IsServer)
			{
				SetBeaconAsTargetServer(beaconId);
				return;
			}
			MyMultiplayer.RaiseEvent(this, (MyJumpDrive x) => x.SetBeaconAsTargetServer, beaconId);
		}

		[Event(null, 651)]
		[Reliable]
		[Server]
		private void SetBeaconAsTargetServer(long beaconId)
		{
			foreach (MyBeacon beacon in m_beacons)
			{
				if (beacon.EntityId == beaconId)
				{
					m_selectedBeaconId.Value = beaconId;
					m_selectedBeaconCoords.Value = beacon.PositionComp.GetPosition();
					m_targetSync.Value = 0;
					break;
				}
			}
		}

		private void SelectGps(List<MyGuiControlListbox.Item> selection)
		{
			if (selection.Count > 0)
			{
				m_selectedGps = (IMyGps)selection[0].UserData;
				m_selectedBeaconId.Value = 0L;
				RaisePropertiesChanged();
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.RequiredPowerInput, ComputeRequiredPower, this);
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			m_selectedBeaconId.Value = 0L;
			MyObjectBuilder_JumpDrive myObjectBuilder_JumpDrive = objectBuilder as MyObjectBuilder_JumpDrive;
<<<<<<< HEAD
			if (Sync.IsServer)
			{
				m_initedOnServer.Value = true;
				base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
			else
			{
				base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
			}
=======
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (Sync.IsServer)
			{
				m_storedPower.Value = Math.Min(myObjectBuilder_JumpDrive.StoredPower, BlockDefinition.PowerNeededForJump);
			}
			m_storedJumpTarget = myObjectBuilder_JumpDrive.JumpTarget;
			if (myObjectBuilder_JumpDrive.JumpTarget.HasValue)
			{
				m_jumpTarget = MySession.Static.Gpss.GetGps(myObjectBuilder_JumpDrive.JumpTarget.Value);
				if (m_jumpTarget == null && myObjectBuilder_JumpDrive.SelectedBeaconId > 0)
				{
					m_selectedBeaconId.Value = myObjectBuilder_JumpDrive.SelectedBeaconId;
				}
			}
			m_jumpDistanceRatio.ValidateRange(0f, 1f);
			m_jumpDistanceRatio.SetLocalValue(MathHelper.Clamp(myObjectBuilder_JumpDrive.JumpRatio, 0f, 1f));
			m_isRecharging.SetLocalValue(myObjectBuilder_JumpDrive.Recharging);
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.IsWorkingChanged += MyJumpDrive_IsWorkingChanged;
			base.ResourceSink.Update();
			UpdateEmissivity();
		}

		private void MyJumpDrive_IsWorkingChanged(MyCubeBlock obj)
		{
			CheckForAbort();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			CheckForAbort();
		}

		private void CheckForAbort()
		{
			if (Sync.IsServer && IsJumping && (!base.IsWorking || !base.IsFunctional))
			{
				IsJumping = false;
				base.CubeGrid.GridSystems.JumpSystem.RequestAbort();
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_JumpDrive myObjectBuilder_JumpDrive = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_JumpDrive;
			myObjectBuilder_JumpDrive.StoredPower = m_storedPower;
			if (m_jumpTarget != null)
			{
				myObjectBuilder_JumpDrive.JumpTarget = m_jumpTarget.Hash;
			}
			myObjectBuilder_JumpDrive.SelectedBeaconId = ((m_selectedBeaconId.Value > 0) ? m_selectedBeaconId.Value : 0);
			myObjectBuilder_JumpDrive.JumpRatio = m_jumpDistanceRatio;
			myObjectBuilder_JumpDrive.Recharging = m_isRecharging;
			return myObjectBuilder_JumpDrive;
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			base.CubeGrid.GridSystems.JumpSystem.RegisterJumpDrive(this);
		}

		public override void OnUnregisteredFromGridSystems()
		{
			base.OnUnregisteredFromGridSystems();
			base.CubeGrid.GridSystems.JumpSystem.AbortJump(MyGridJumpDriveSystem.MyJumpFailReason.None);
			base.CubeGrid.GridSystems.JumpSystem.UnregisterJumpDrive(this);
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			GetBeaconsInRange();
			if (Sync.IsServer && m_storedJumpTarget.HasValue)
			{
				m_jumpTarget = MySession.Static.Gpss.GetGps(m_storedJumpTarget.Value);
				if (m_jumpTarget != null)
				{
					m_targetSync.Value = m_jumpTarget.Hash;
				}
			}
			if (m_jumpTarget == null && m_selectedBeaconId.Value > 0 && m_beaconStubsSync.Value != null && m_beaconStubsSync.Value.FindIndex((BeaconStub x) => x.Id == m_selectedBeaconId.Value) != -1)
			{
				SetBeaconAsTarget(m_selectedBeaconId.Value);
			}
			if (Sync.IsServer && m_beaconStubsSync.Value == null)
			{
				m_beaconStubsSync.Value = new List<BeaconStub>();
			}
		}

		public override void UpdateAfterSimulation()
		{
			if ((bool)m_initedOnServer)
			{
				if (!m_initedLocal)
				{
					m_initedLocal = true;
					base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
				base.UpdateAfterSimulation();
				base.ResourceSink.Update();
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (base.IsFunctional && !IsFull && (bool)m_isRecharging)
			{
				StorePower(1666.66663f, base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId));
			}
			UpdateEmissivity();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(BlockDefinition.RequiredPowerInput, detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxStoredPower));
			MyValueFormatter.AppendWorkHoursInBestUnit(BlockDefinition.PowerNeededForJump, detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			detailedInfo.AppendFormat(" ({0}%)", BlockDefinition.PowerEfficiency * 100f);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_StoredPower));
			MyValueFormatter.AppendWorkHoursInBestUnit(m_storedPower, detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_RechargedIn));
			MyValueFormatter.AppendTimeInBestUnit(m_timeRemaining, detailedInfo);
			detailedInfo.Append("\n");
			int num = (int)(base.CubeGrid.GridSystems.JumpSystem.GetMaxJumpDistance(base.OwnerId) / 1000.0);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxJump));
			detailedInfo.Append(num).Append(" km");
			double num2 = 0.0;
			if (m_jumpTarget != null)
			{
				num2 = (m_jumpTarget.Coords - base.CubeGrid.WorldMatrix.Translation).Length();
			}
			else
			{
				_ = m_selectedBeaconCoords.Value;
				if (m_selectedBeaconCoords.Value != Vector3D.Zero)
				{
					num2 = (m_selectedBeaconCoords.Value - base.CubeGrid.WorldMatrix.Translation).Length();
				}
			}
			if (num2 > 0.0)
			{
				detailedInfo.Append("\n");
				float num3 = Math.Min(1f, (float)((double)num / num2));
				detailedInfo.Append(MyTexts.Get(MySpaceTexts.BlockPropertiesText_CurrentJump).ToString() + (num3 * 100f).ToString("F2") + "%");
			}
		}

		private float ComputeRequiredPower()
		{
			if (base.IsFunctional && base.IsWorking && (bool)m_isRecharging)
			{
				if (IsFull)
				{
					return 0f;
				}
				return BlockDefinition.RequiredPowerInput;
			}
			return 0f;
		}

		private void StorePower(float deltaTime, float input)
		{
			float num = input / 3600000f;
			float num2 = deltaTime * num * BlockDefinition.PowerEfficiency;
			if (Sync.IsServer)
			{
				m_storedPower.Value += num2;
			}
			deltaTime /= 1000f;
			if (Sync.IsServer && (float)m_storedPower > BlockDefinition.PowerNeededForJump)
			{
				m_storedPower.Value = BlockDefinition.PowerNeededForJump;
			}
			if (num2 > 0f)
			{
				m_timeRemaining = (BlockDefinition.PowerNeededForJump - (float)m_storedPower) * deltaTime / num2;
			}
			else
			{
				m_timeRemaining = 0f;
			}
		}

		public void SetStoredPower(float filledRatio)
		{
			if (filledRatio < 0f)
			{
				filledRatio = 0f;
			}
			if (filledRatio >= 1f)
			{
				filledRatio = 1f;
				m_timeRemaining = 0f;
			}
			if (Sync.IsServer)
			{
				CurrentStoredPower = filledRatio * BlockDefinition.PowerNeededForJump;
			}
			UpdateEmissivity();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			m_prevFillCount = -1;
			UpdateEmissivity();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			UpdateEmissivity(force: true);
		}

		public override bool SetEmissiveStateWorking()
		{
			return false;
		}

		public override bool SetEmissiveStateDamaged()
		{
			return false;
		}

		public override bool SetEmissiveStateDisabled()
		{
			return false;
		}

		private void UpdateEmissivity(bool force = false)
		{
			Color red = Color.Red;
			float fill = 1f;
			float emissivity = 1f;
			MyEmissiveColorStateResult result;
			if (base.IsWorking)
			{
				if (IsFull)
				{
					red = Color.Green;
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
					{
						red = result.EmissiveColor;
					}
				}
				else if (!m_isRecharging)
				{
					fill = (float)m_storedPower / BlockDefinition.PowerNeededForJump;
					red = Color.Red;
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
					{
						red = result.EmissiveColor;
					}
				}
				else if (base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) > 0f)
				{
					fill = (float)m_storedPower / BlockDefinition.PowerNeededForJump;
					red = Color.Yellow;
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Warning, out result))
					{
						red = result.EmissiveColor;
					}
				}
				else
				{
					fill = (float)m_storedPower / BlockDefinition.PowerNeededForJump;
					red = Color.Red;
					emissivity = 1f;
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
					{
						red = result.EmissiveColor;
					}
				}
			}
			else if (base.IsFunctional)
			{
				fill = 0f;
				red = Color.Red;
				emissivity = 1f;
				if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
				{
					red = result.EmissiveColor;
				}
			}
			else
			{
				fill = 0f;
				red = Color.Black;
				emissivity = 0f;
				if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
				{
					red = result.EmissiveColor;
				}
			}
			SetEmissive(red, fill, emissivity, force);
		}

		private void SetEmissive(Color color, float fill, float emissivity, bool force)
		{
			int num = (int)(fill * (float)m_emissiveTextureNames.Length);
			if (!force && (base.Render.RenderObjectIDs[0] == uint.MaxValue || (!(color != m_prevColor) && num == m_prevFillCount)))
			{
				return;
			}
			for (int i = 0; i < m_emissiveTextureNames.Length; i++)
			{
				if (i <= num)
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[i], color, emissivity);
				}
				else
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[i], Color.Black, 0f);
				}
			}
			MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], "Emissive", color, emissivity);
			m_prevColor = color;
			m_prevFillCount = num;
		}
	}
}
