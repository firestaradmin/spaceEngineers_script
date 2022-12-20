using System.Collections.Generic;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes one of block LCDs where you can write text or draw things (PB scripting interface)
	/// </summary>
	public interface IMyTextSurface
	{
		/// <summary>
		/// Gets image that is currently shown on the screen.
		///
		/// Returns NULL if there are no images selected OR the screen is in text mode.
		/// </summary>
		string CurrentlyShownImage { get; }

		/// <summary>
		/// Gets or sets font size
		/// </summary>
		float FontSize { get; set; }

		/// <summary>
		/// Gets or sets font color
		/// </summary>
		Color FontColor { get; set; }

		/// <summary>
		/// Gets or sets background color
		/// </summary>
		Color BackgroundColor { get; set; }

		/// <summary>
		/// Value for offscreen texture alpha channel
		/// - for PBR material it is metalness (should be 0)
		/// - for transparent texture it is opacity
		/// </summary>
		byte BackgroundAlpha { get; set; }

		/// <summary>
		/// Gets or sets the change interval for selected textures
		/// </summary>
		float ChangeInterval { get; set; }

		/// <summary>
		/// Gets or sets the font
		/// </summary>
		string Font { get; set; }

		/// <summary>
		/// How should the text be aligned
		/// </summary>
		TextAlignment Alignment { get; set; }

		/// <summary>
		/// Currently running script
		/// </summary>
		string Script { get; set; }

		/// <summary>
		/// Type of content to be displayed on the screen.
		/// </summary>
		ContentType ContentType { get; set; }

		/// <summary>
		/// Gets size of the drawing surface.
		/// </summary>
		Vector2 SurfaceSize { get; }

		/// <summary>
		/// Gets size of the texture the drawing surface is rendered to.
		/// </summary>
		Vector2 TextureSize { get; }

		/// <summary>
		/// Gets or sets preserve aspect ratio of images.
		/// </summary>
		bool PreserveAspectRatio { get; set; }

		/// <summary>
		/// Gets or sets text padding from all sides of the panel.
		/// </summary>
		float TextPadding { get; set; }

		/// <summary>
		/// Gets or sets background color used for scripts.
		/// </summary>
		Color ScriptBackgroundColor { get; set; }

		/// <summary>
		/// Gets or sets foreground color used for scripts.
		/// </summary>
		Color ScriptForegroundColor { get; set; }

		/// <summary>
		/// Gets identifier name of this surface.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Get localized name of this surface.
		/// </summary>
		string DisplayName { get; }

		/// <summary>
		/// Writes text to surface.
		/// If somebody opened LCD text in popup dialog, text can no longer be added to the surface.
		/// Resulting text must is capped with 100000 symbols
		/// </summary>
		/// <param name="value">Text to append</param>
		/// <param name="append">If true, appends, else replaces text with `<paramref name="value" />`</param>
		/// <returns>True if text was added, or replaced. False if somebody was looking at lcd</returns>
		bool WriteText(string value, bool append = false);

		/// <summary>
		/// Gets current text that is written on surface. Allocates memory (StringBuilder.ToString())! This method doesn't allocate memory <see cref="M:Sandbox.ModAPI.Ingame.IMyTextSurface.ReadText(System.Text.StringBuilder,System.Boolean)" />
		/// </summary>
		/// <returns>Current text</returns>
		string GetText();

		/// <summary>
		/// Writes text to surface.
		/// If somebody opened LCD text in popup dialog, text can no longer be added to the surface.
		/// Resulting text must is capped with 100000 symbols
		/// </summary>
		/// <param name="value">Text to append</param>
		/// <param name="append">If true, appends, else replaces text with `<paramref name="value" />`</param>
		/// <returns>True if text was added, or replaced. False if somebody was looking at lcd</returns>
		bool WriteText(StringBuilder value, bool append = false);

		/// <summary>
		/// Gets current text that is written on surface. 
		/// </summary>
		/// <param name="buffer">Where to write text</param>
		/// <param name="append">If true, text would be appended, else buffer would be cleared before text append</param>
		void ReadText(StringBuilder buffer, bool append = false);

		/// <summary>
		/// Adds image to list of shown images.
		/// You can get image ids by <see cref="M:Sandbox.ModAPI.Ingame.IMyTextSurface.GetSelectedImages(System.Collections.Generic.List{System.String})" />
		/// </summary>
		/// <param name="id">Id of image</param>
		/// <param name="checkExistence">If true, image can't be added twice</param>
		void AddImageToSelection(string id, bool checkExistence = false);

		/// <summary>
		/// Adds image to list of shown images.
		/// You can get image ids by <see cref="M:Sandbox.ModAPI.Ingame.IMyTextSurface.GetSelectedImages(System.Collections.Generic.List{System.String})" />
		/// </summary>
		/// <param name="ids">Ids of image</param>
		/// <param name="checkExistence">If true, image can't be added twice</param>
		void AddImagesToSelection(List<string> ids, bool checkExistence = false);

		/// <summary>
		/// Removes image from shown images.
		/// </summary>
		/// <param name="id">Id of image</param>
		/// <param name="removeDuplicates"></param>
		void RemoveImageFromSelection(string id, bool removeDuplicates = false);

		/// <summary>
		/// Removes images from shown images.
		/// </summary>
		/// <param name="ids">Images ids</param>
		/// <param name="removeDuplicates">If true, would remove all images with provided ids</param>
		void RemoveImagesFromSelection(List<string> ids, bool removeDuplicates = false);

		/// <summary>
		/// Removes images from shown images. 
		/// </summary>
		void ClearImagesFromSelection();

		/// <summary>
		/// Outputs the selected image ids to the specified list.
		///
		/// NOTE: List is not cleared internally.
		/// </summary>
		/// <param name="output">Image id's would be written into this array</param>
		void GetSelectedImages(List<string> output);

		/// <summary>
		/// Gets a list of available fonts
		/// </summary>
		/// <param name="fonts"></param>
		void GetFonts(List<string> fonts);

		/// <summary>
		/// Gets a list of available sprites
		/// </summary>
		/// <param name="sprites">Buffer array that would be filled with available sprites</param>
		void GetSprites(List<string> sprites);

		/// <summary>
		/// Gets a list of available scripts
		/// </summary>
		/// <param name="scripts"></param>
		void GetScripts(List<string> scripts);

		/// <summary>
		/// Creates a new draw frame where you can add sprites to be rendered.
		/// </summary>
		/// <returns></returns>
		MySpriteDrawFrame DrawFrame();

		/// <summary>
		/// Calculates how many pixels a string of a given font and scale will take up.
		/// </summary>
		/// <param name="text">Text to measure</param>
		/// <param name="font">Text font to measure</param>
		/// <param name="scale">Text scale to measure</param>
		/// <returns>Width and Height of text with specified text, font and scale</returns>
		Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale);
	}
}
