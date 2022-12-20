using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Large Ship properties")]
	internal class MyGuiScreenDebugShipLargeProperties : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugShipLargeProperties()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("System large ship properties", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddLabel("Front light", Color.Yellow.ToVector4(), 1.2f);
			AddButton(new StringBuilder("Set min. build level"), OnClick_SetMinBuildLevel);
			AddButton(new StringBuilder("Upgrade build level"), OnClick_UpgradeBuildLevel);
			AddButton(new StringBuilder("Randomize build level"), OnClick_RandomizeBuildLevel);
			m_currentPosition.Y += 0.01f;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugShipLargeProperties";
		}

		private void OnClick_RandomizeBuildLevel(MyGuiControlButton button)
		{
			MyCubeGrid targetShip = GetTargetShip();
			foreach (MySlimBlock block in targetShip.GetBlocks())
			{
				block.RandomizeBuildLevel();
				block.UpdateVisual();
			}
		}

		private void OnClick_UpgradeBuildLevel(MyGuiControlButton button)
		{
			MyCubeGrid targetShip = GetTargetShip();
			foreach (MySlimBlock block in targetShip.GetBlocks())
			{
				block.UpgradeBuildLevel();
				block.UpdateVisual();
			}
		}

		private void OnClick_SetMinBuildLevel(MyGuiControlButton button)
		{
			MyCubeGrid targetShip = GetTargetShip();
			foreach (MySlimBlock block in targetShip.GetBlocks())
			{
				block.SetToConstructionSite();
				block.UpdateVisual();
			}
		}

		private MyCubeGrid GetTargetShip()
		{
			MyEntity myEntity = MyCubeBuilder.Static.FindClosestGrid();
			if (myEntity == null)
			{
				LineD line = new LineD(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 10000f);
				List<MyLineSegmentOverlapResult<MyEntity>> list = new List<MyLineSegmentOverlapResult<MyEntity>>();
				MyEntities.OverlapAllLineSegment(ref line, list);
				if (list.Count > 0)
				{
					myEntity = list.OrderBy((MyLineSegmentOverlapResult<MyEntity> s) => s.Distance).First().Element;
					myEntity = myEntity.GetTopMostParent();
				}
			}
			return myEntity as MyCubeGrid;
		}
	}
}
