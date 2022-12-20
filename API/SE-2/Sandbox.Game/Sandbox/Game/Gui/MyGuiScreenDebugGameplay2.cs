using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Gameplay 2")]
	[StaticEventOwner]
	public class MyGuiScreenDebugGameplay2 : MyGuiScreenDebugBase
	{
		[Serializable]
		public struct MyStationDebugDrawStructure
		{
			protected class Sandbox_Game_Gui_MyGuiScreenDebugGameplay2_003C_003EMyStationDebugDrawStructure_003C_003EStart_003C_003EAccessor : IMemberAccessor<MyStationDebugDrawStructure, SerializableVector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStationDebugDrawStructure owner, in SerializableVector3D value)
				{
					owner.Start = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStationDebugDrawStructure owner, out SerializableVector3D value)
				{
					value = owner.Start;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenDebugGameplay2_003C_003EMyStationDebugDrawStructure_003C_003EEnd_003C_003EAccessor : IMemberAccessor<MyStationDebugDrawStructure, SerializableVector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStationDebugDrawStructure owner, in SerializableVector3D value)
				{
					owner.End = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStationDebugDrawStructure owner, out SerializableVector3D value)
				{
					value = owner.End;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenDebugGameplay2_003C_003EMyStationDebugDrawStructure_003C_003ETypeId_003C_003EAccessor : IMemberAccessor<MyStationDebugDrawStructure, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStationDebugDrawStructure owner, in int value)
				{
					owner.TypeId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStationDebugDrawStructure owner, out int value)
				{
					value = owner.TypeId;
				}
			}

			public SerializableVector3D Start;

			public SerializableVector3D End;

			public int TypeId;
		}

		protected sealed class UpdateEconomyRequest_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateEconomyRequest();
			}
		}

		protected sealed class RequestStationPositions_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestStationPositions();
			}
		}

		protected sealed class DrawStationsClient_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Gui_MyGuiScreenDebugGameplay2_003C_003EMyStationDebugDrawStructure_003E : ICallSite<IMyEventOwner, List<MyStationDebugDrawStructure>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<MyStationDebugDrawStructure> stations, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DrawStationsClient(stations);
			}
		}

		private const float TWO_BUTTON_XOFFSET = 0.05f;

		public MyGuiScreenDebugGameplay2()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Gameplay 2", Color.Yellow.ToVector4());
			AddButton("Draw positions of stations", delegate
			{
				DrawStations();
			});
			AddButton("Force economy update", delegate
			{
				UpdateEconomy();
			});
			AddCheckBox("Force Add Trash Removal Menu", null, MemberHelper.GetMember(() => MyFakes.FORCE_ADD_TRASH_REMOVAL_MENU));
			AddButton("Draw Solar occlusion Debug", delegate
			{
				UpdateSolarOcclusionDebug();
			});
		}

		private void UpdateSolarOcclusionDebug()
		{
			MyFakes.DrawSolarOcclusionOnce = true;
		}

		private void UpdateEconomy()
		{
			if (MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer)
			{
				UpdateEconomyInternal();
				return;
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => UpdateEconomyRequest);
		}

		[Event(null, 81)]
		[Reliable]
		[Server]
		private static void UpdateEconomyRequest()
		{
			UpdateEconomyInternal();
		}

		private static void UpdateEconomyInternal()
		{
			MySession.Static.GetComponent<MySessionComponentEconomy>()?.ForceEconomyTick();
		}

		private static void DrawStations()
		{
			if (MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer)
			{
				if (MySession.Static.Factions == null)
				{
					return;
				}
				foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
				{
					foreach (MyStation station in faction.Value.Stations)
					{
						switch (station.Type)
						{
						case MyStationTypeEnum.MiningStation:
							MyRenderProxy.DebugDrawSphere(station.Position, 150f, Color.Red, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
							MyRenderProxy.DebugDrawLine3D(station.Position, Vector3D.Zero, Color.Red, Color.Red, depthRead: false, persistent: true);
							break;
						case MyStationTypeEnum.OrbitalStation:
						{
							Vector3D pointTo = MyGamePruningStructure.GetClosestPlanet(station.Position)?.PositionComp.GetPosition() ?? Vector3D.Zero;
							MyRenderProxy.DebugDrawSphere(station.Position, 150f, Color.CornflowerBlue, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
							MyRenderProxy.DebugDrawLine3D(station.Position, pointTo, Color.CornflowerBlue, Color.CornflowerBlue, depthRead: false, persistent: true);
							break;
						}
						case MyStationTypeEnum.Outpost:
						{
							Vector3D pointTo2 = MyGamePruningStructure.GetClosestPlanet(station.Position)?.PositionComp.GetPosition() ?? Vector3D.Zero;
							MyRenderProxy.DebugDrawSphere(station.Position, 150f, Color.Yellow, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
							MyRenderProxy.DebugDrawLine3D(station.Position, pointTo2, Color.Yellow, Color.Yellow, depthRead: false, persistent: true);
							break;
						}
						case MyStationTypeEnum.SpaceStation:
						{
							Color color = ((!station.IsDeepSpaceStation) ? Color.Green : Color.Purple);
							MyRenderProxy.DebugDrawSphere(station.Position, 150f, color, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
							MyRenderProxy.DebugDrawLine3D(station.Position, Vector3D.Zero, color, color, depthRead: false, persistent: true);
							break;
						}
						}
					}
				}
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RequestStationPositions);
			}
		}

		[Event(null, 169)]
		[Reliable]
		[Server]
		private static void RequestStationPositions()
		{
			if ((MyMultiplayer.Static != null && !MyMultiplayer.Static.IsServer) || MySession.Static.Factions == null)
			{
				return;
			}
			List<MyStationDebugDrawStructure> list = new List<MyStationDebugDrawStructure>();
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				foreach (MyStation station in faction.Value.Stations)
				{
					MyStationDebugDrawStructure item = default(MyStationDebugDrawStructure);
					item.Start = Vector3D.Zero;
					item.End = station.Position;
					switch (station.Type)
					{
					case MyStationTypeEnum.MiningStation:
						item.TypeId = 0;
						break;
					case MyStationTypeEnum.OrbitalStation:
					{
						item.TypeId = 1;
						MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(station.Position);
						if (closestPlanet != null)
						{
							item.Start = closestPlanet.PositionComp.GetPosition();
						}
						break;
					}
					case MyStationTypeEnum.Outpost:
					{
						item.TypeId = 2;
						MyPlanet closestPlanet2 = MyGamePruningStructure.GetClosestPlanet(station.Position);
						if (closestPlanet2 != null)
						{
							item.Start = closestPlanet2.PositionComp.GetPosition();
						}
						break;
					}
					case MyStationTypeEnum.SpaceStation:
						if (station.IsDeepSpaceStation)
						{
							item.TypeId = 3;
						}
						else
						{
							item.TypeId = 4;
						}
						break;
					}
					list.Add(item);
				}
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => DrawStationsClient, list, MyEventContext.Current.Sender);
		}

		[Event(null, 227)]
		[Reliable]
		[Client]
		private static void DrawStationsClient(List<MyStationDebugDrawStructure> stations)
		{
			foreach (MyStationDebugDrawStructure station in stations)
			{
				Color color;
				switch (station.TypeId)
				{
				case 0:
					color = Color.Red;
					break;
				case 1:
					color = Color.CornflowerBlue;
					break;
				case 2:
					color = Color.Yellow;
					break;
				case 3:
					color = Color.Purple;
					break;
				case 4:
					color = Color.Green;
					break;
				default:
					color = Color.Pink;
					break;
				}
				MyRenderProxy.DebugDrawSphere(station.End, 150f, color, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
				MyRenderProxy.DebugDrawLine3D(station.End, station.Start, color, color, depthRead: false, persistent: true);
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugGameplay2";
		}
	}
}
