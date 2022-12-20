using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyGuiControlBloodOverlay : MyGuiControlBase
	{
		public override void Update()
		{
			base.Update();
			MyGuiScreenBase topMostOwnerScreen = GetTopMostOwnerScreen();
			if (topMostOwnerScreen == MyGuiScreenHudSpace.Static)
			{
				MyGuiScreenState state = topMostOwnerScreen.State;
				if ((uint)(state - 4) <= 2u)
				{
					Draw(1f, 1f);
				}
			}
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			MySession @static = MySession.Static;
			MyCharacter localCharacter = @static.LocalCharacter;
			IMyControllableEntity controlledEntity = @static.ControlledEntity;
			MyCockpit myCockpit;
			MyLargeTurretBase myLargeTurretBase;
			if (localCharacter != null && (controlledEntity == localCharacter || ((myCockpit = controlledEntity as MyCockpit) != null && myCockpit.Pilot == localCharacter) || ((myLargeTurretBase = controlledEntity as MyLargeTurretBase) != null && myLargeTurretBase.Pilot == localCharacter)))
			{
				float hUDBloodAlpha = localCharacter.Render.GetHUDBloodAlpha();
				if (hUDBloodAlpha > 0f)
				{
					Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
					RectangleF destination = new RectangleF(0f, 0f, fullscreenRectangle.Width, fullscreenRectangle.Height);
					MyRenderProxy.DrawSprite("Textures\\Gui\\Blood.dds", ref destination, null, new Color(new Vector4(1f, 1f, 1f, hUDBloodAlpha)), 0f, ignoreBounds: true, waitTillLoaded: true);
				}
			}
		}
	}
}
