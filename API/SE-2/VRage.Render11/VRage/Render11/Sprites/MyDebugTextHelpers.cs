using System.Text;
using VRage.Render11.Common;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.Sprites
{
	internal static class MyDebugTextHelpers
	{
		internal static void CalculateSpriteClipspace(RectangleF destination, Vector2 screenSize, out Vector2 clipOffset, out Vector2 clipScale)
		{
			Vector2 vector = destination.Size / screenSize;
			Vector2 vector2 = destination.Position / screenSize;
			clipScale = vector * 2f;
			clipOffset = vector2 * 2f - 1f;
			clipOffset.Y = 0f - clipOffset.Y;
		}

		internal static Vector2 MeasureText(StringBuilder text, float scale)
		{
			return MyRender11.DebugFont.MeasureString(text, scale);
		}

		internal static void DrawText(Vector2 screenCoord, StringBuilder text, Color color, float scale, MyGuiDrawAlignEnum align = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
		{
			MyRenderMessageDrawString myRenderMessageDrawString = MyRenderProxy.MessagePool.Get<MyRenderMessageDrawString>(MyRenderMessageEnum.DrawString);
			myRenderMessageDrawString.Text = text.ToString();
			myRenderMessageDrawString.FontIndex = MyRender11.DebugFontId;
			myRenderMessageDrawString.ScreenCoord = MyUtils.GetCoordAligned(screenCoord, MyRender11.DebugFont.MeasureString(text, scale), align);
			myRenderMessageDrawString.ColorMask = color;
			myRenderMessageDrawString.ScreenScale = scale;
			myRenderMessageDrawString.ScreenMaxWidth = float.PositiveInfinity;
			myRenderMessageDrawString.TargetTexture = "DEBUG_TARGET";
			MyManagers.SpritesManager.DebugDrawMessages.Messages.Add(myRenderMessageDrawString);
		}

		internal static void DrawTextShadow(Vector2 screenCoord, StringBuilder text, Color color, float scale)
		{
			MyRenderMessageDrawString myRenderMessageDrawString = MyRenderProxy.MessagePool.Get<MyRenderMessageDrawString>(MyRenderMessageEnum.DrawString);
			myRenderMessageDrawString.Text = text.ToString();
			myRenderMessageDrawString.FontIndex = MyRender11.DebugFontId;
			myRenderMessageDrawString.ScreenCoord = screenCoord;
			myRenderMessageDrawString.ColorMask = color;
			myRenderMessageDrawString.ScreenScale = scale;
			myRenderMessageDrawString.ScreenMaxWidth = float.PositiveInfinity;
			myRenderMessageDrawString.TargetTexture = "DEBUG_TARGET";
			MyManagers.SpritesManager.DebugDrawMessages.Messages.Add(myRenderMessageDrawString);
		}
	}
}
