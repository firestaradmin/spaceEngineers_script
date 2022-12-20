using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Network;
<<<<<<< HEAD
using VRage.Utils;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Game", "Travel")]
	[StaticEventOwner]
	internal class MyGuiScreenDebugTravel : MyGuiScreenDebugBase
	{
		protected sealed class HeatmapTeleportServer_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				HeatmapTeleportServer();
			}
		}

		protected sealed class HeatmapTeleportClient_003C_003EVRageMath_Vector3D_0023System_Single : ICallSite<IMyEventOwner, Vector3D, float, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in Vector3D heatPoint, in float heat, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				HeatmapTeleportClient(heatPoint, heat);
			}
		}

		private static Dictionary<string, Vector3> s_travelPoints = new Dictionary<string, Vector3>
		{
			{
				"Mercury",
				new Vector3(-39f, 0f, 46f)
			},
			{
				"Venus",
				new Vector3(-2f, 0f, 108f)
			},
			{
				"Earth",
				new Vector3(101f, 0f, -111f)
			},
			{
				"Moon",
				new Vector3(101f, 0f, -111f) + new Vector3(-0.015f, 0f, -0.2f)
			},
			{
				"Mars",
				new Vector3(-182f, 0f, 114f)
			},
			{
				"Jupiter",
				new Vector3(-778f, 0f, 155.6f)
			},
			{
				"Saturn",
				new Vector3(1120f, 0f, -840f)
			},
			{
				"Uranus",
				new Vector3(-2700f, 0f, -1500f)
			},
			{
				"Zero",
				new Vector3(0f, 0f, 0f)
			},
			{
				"Billion",
				new Vector3(1000f)
			},
			{
				"BillionFlat0",
				new Vector3(999f, 1000f, 1000f)
			},
			{
				"BillionFlat1",
				new Vector3(1001f, 1000f, 1000f)
			}
		};

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugDrawSettings";
		}

		public MyGuiScreenDebugTravel()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_scale = 0.7f;
			AddCaption("Travel", Color.Yellow.ToVector4());
			AddShareFocusHint();
			foreach (KeyValuePair<string, Vector3> travelPair in s_travelPoints)
			{
				AddButton(new StringBuilder(travelPair.Key), delegate
				{
					TravelTo(travelPair.Value);
				});
			}
			AddCheckBox("Testing jumpdrives", null, MemberHelper.GetMember(() => MyFakes.TESTING_JUMPDRIVE));
			AddLabel("Travel to the most grided place", Color.Yellow, 1f);
			AddButton("Heatmap and Teleport ", HeatmapTeleportButton);
		}

		private static void HeatmapTeleportButton(MyGuiControlButton obj)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => HeatmapTeleportServer);
		}

<<<<<<< HEAD
		[Event(null, 78)]
=======
		[Event(null, 77)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public static void HeatmapTeleportServer()
		{
<<<<<<< HEAD
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			if (!MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				MyLog.Default.Warning("User {0} is trying to cheat by using teleport.", num);
				return;
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyGridClusterAnalyzeHelper myGridClusterAnalyzeHelper = new MyGridClusterAnalyzeHelper();
			double heatRange = MySession.Static.Settings.SyncDistance;
			Vector3D heatPoint;
			float highestHeatPoint = myGridClusterAnalyzeHelper.GetHighestHeatPoint(out heatPoint, heatRange);
<<<<<<< HEAD
			MySession.Static.Players.TryGetPlayerId(num, out var result);
			MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
			if (playerById != null && playerById.Character != null)
			{
				playerById.Character.Teleport(MatrixD.CreateWorld(heatPoint));
=======
			long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			MySession.Static.Players.TryGetPlayerId(identityId, out var result);
			MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
			if (playerById != null && playerById.Character != null)
			{
				MyCharacter character = playerById.Character;
				Matrix m = Matrix.CreateWorld(heatPoint);
				character.Teleport(m);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => HeatmapTeleportClient, heatPoint, highestHeatPoint, MyEventContext.Current.Sender);
		}

<<<<<<< HEAD
		[Event(null, 105)]
=======
		[Event(null, 99)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		public static void HeatmapTeleportClient(Vector3D heatPoint, float heat)
		{
			if (MySession.Static.LocalCharacter != null)
			{
				MyObjectBuilder_Gps.Entry builder = default(MyObjectBuilder_Gps.Entry);
				builder.name = "HeatPoint: " + heat;
				builder.coords = heatPoint;
				builder.showOnHud = true;
				builder.alwaysVisible = true;
				MyGps gps = new MyGps(builder);
				MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps, 0L);
			}
		}

		private void TravelTo(Vector3 positionInMilions)
		{
			MyMultiplayer.TeleportControlledEntity((Vector3D)positionInMilions * 1000000.0);
		}
	}
}
