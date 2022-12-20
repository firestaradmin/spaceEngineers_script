using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("VRage", "Grid Systems")]
	internal class MyGuiScreenDebugCubeGridSystems : MyGuiScreenDebugBase
	{
		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugCubeGrid";
		}

		public MyGuiScreenDebugCubeGridSystems()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_scale = 0.7f;
			AddCaption("Cube Grid Systems", Color.Yellow.ToVector4());
			AddShareFocusHint();
			AddCheckBox("Conveyor line IDs", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS_LINE_IDS));
			AddCheckBox("Conveyor line capsules", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS_LINE_CAPSULES));
			AddCheckBox("Connectors and merge blocks", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CONNECTORS_AND_MERGE_BLOCKS));
			AddCheckBox("Terminal block names", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_BLOCK_NAMES));
			AddCheckBox("Radio broadcasters", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_RADIO_BROADCASTERS));
			AddCheckBox("Neutral ships", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_NEUTRAL_SHIPS));
			AddCheckBox("Resource Distribution", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS));
			AddCheckBox("Cockpit", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_COCKPIT));
			AddCheckBox("Conveyors", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS));
			AddCheckBox("Thruster damage", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_THRUSTER_DAMAGE));
			AddCheckBox("Block groups", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_BLOCK_GROUPS));
			AddCheckBox("Rotors", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_ROTORS));
			AddCheckBox("Gyros", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_GYROS));
			AddCheckBox("Local coordinate system", null, MemberHelper.GetMember(() => MyFakes.ENABLE_DEBUG_DRAW_COORD_SYS));
			AddCheckBox("Drill Clusters", null, MemberHelper.GetMember(() => MyShipMiningSystem.DebugDrawClusters));
			AddCheckBox("Drill Cut-Outs", null, MemberHelper.GetMember(() => MyShipMiningSystem.DebugDrawCutOuts));
			AddSlider("Drill Cut-Out Permanence", 1f, 60f, null, MemberHelper.GetMember(() => MyShipMiningSystem.DebugDrawCutOutPermanence));
			AddSlider("Drill Cut-Out Slowdown (s)", 0f, 5f, null, MemberHelper.GetMember(() => MyShipMiningSystem.DebugOperationDelay));
			AddCheckBox("Disable Mining System", null, MemberHelper.GetMember(() => MyShipMiningSystem.DebugDisable));
<<<<<<< HEAD
			AddCheckBox("Targeting DISTANCE Matters", null, MemberHelper.GetMember(() => MyFakes.TARGETING_DISTANCE_MODIFIER_ENABLED));
			AddSlider("Targeting DISTANCE POWER", 0.1f, 6f, null, MemberHelper.GetMember(() => MyFakes.TARGETING_DISTANCE_MODIFIER_POWER));
			AddSlider("Targeting DISTANCE POWER LIMIT", 0f, 100f, null, MemberHelper.GetMember(() => MyFakes.TARGETING_DISTANCE_MODIFIER_POWER_LIMIT));
			AddCheckBox("Targeting DRAW", null, MemberHelper.GetMember(() => MyFakes.ENABLE_TARGETING_CHANCE_DRAW));
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
