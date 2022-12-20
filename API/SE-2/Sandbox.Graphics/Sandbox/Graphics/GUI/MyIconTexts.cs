using System;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyIconTexts : Dictionary<MyGuiDrawAlignEnum, MyColoredText>
	{
		/// <summary>
		/// Returns text's position from icon's position and size
		/// </summary>
		/// <param name="iconPosition">Icon's top-left position</param>
		/// <param name="iconSize">Icon's size</param>
		/// <param name="drawAlign">Text's draw align</param>
		/// <returns></returns>
		private Vector2 GetPosition(Vector2 iconPosition, Vector2 iconSize, MyGuiDrawAlignEnum drawAlign)
		{
			return drawAlign switch
			{
				MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM => iconPosition + new Vector2(iconSize.X / 2f, iconSize.Y), 
				MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER => iconPosition + new Vector2(iconSize.X / 2f, iconSize.Y / 2f), 
				MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP => iconPosition + new Vector2(iconSize.X / 2f, 0f), 
				MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM => iconPosition + new Vector2(0f, iconSize.Y), 
				MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER => iconPosition + new Vector2(0f, iconSize.Y / 2f), 
				MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP => iconPosition, 
				MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM => iconPosition + new Vector2(iconSize.X, iconSize.Y), 
				MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER => iconPosition + new Vector2(iconSize.X, iconSize.Y / 2f), 
				MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP => iconPosition + new Vector2(iconSize.X, 0f), 
				_ => throw new Exception(), 
			};
		}

		/// <summary>
		/// Draws non highlight texts into icon
		/// </summary>
		/// <param name="iconPosition">Icon's top-left position</param>
		/// <param name="iconSize">Icon's size</param>
		/// <param name="backgroundAlphaFade">Background's alpha fade</param>
		/// <param name="colorMultiplicator">Color multiplicator</param>
		public void Draw(Vector2 iconPosition, Vector2 iconSize, float backgroundAlphaFade, float colorMultiplicator = 1f)
		{
			Draw(iconPosition, iconSize, backgroundAlphaFade, isHighlight: false, colorMultiplicator);
		}

		/// <summary>
		/// Draws texts into icon
		/// </summary>
		/// <param name="iconPosition">Icon's top-left position</param>
		/// <param name="iconSize">Icon's size</param>
		/// <param name="backgroundAlphaFade">Background's alpha fade</param>
		/// <param name="isHighlight">Defines if texts will be highlighted</param>
		/// <param name="colorMultiplicator">Color multiplicator</param> 
		public void Draw(Vector2 iconPosition, Vector2 iconSize, float backgroundAlphaFade, bool isHighlight, float colorMultiplicator = 1f)
		{
			using Enumerator enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<MyGuiDrawAlignEnum, MyColoredText> current = enumerator.Current;
				Vector2 position = GetPosition(iconPosition, iconSize, current.Key);
				current.Value.Draw(position, current.Key, backgroundAlphaFade, isHighlight, colorMultiplicator);
			}
		}
	}
}
