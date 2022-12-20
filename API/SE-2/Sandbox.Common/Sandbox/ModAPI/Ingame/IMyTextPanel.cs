using System;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes LCD block (mods interface)
	/// </summary>
	public interface IMyTextPanel : IMyTextSurface, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Indicates what should be shown on the screen, none being an image.
		/// </summary>
		[Obsolete("LCD public text is deprecated")]
		ShowTextOnScreenFlag ShowOnScreen { get; }

		/// <summary>
		/// Returns true if the ShowOnScreen flag is set to either PUBLIC or PRIVATE
		/// </summary>
		[Obsolete("LCD public text is deprecated")]
		bool ShowText { get; }

		/// <summary>
		/// Writes LCD popup dialog title
		/// If somebody opened LCD text in popup dialog, text can no longer be added.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="append">If true, appends, else replaces text with `<paramref name="value" />`</param>
		/// <returns>False is if somebody currently has opened LCD text in popup dialog, true in other cases</returns>
		bool WritePublicTitle(string value, bool append = false);

		/// <summary>
		/// Gets popup dialog title
		/// </summary>
		/// <returns>Public title</returns>
		string GetPublicTitle();

		[Obsolete("LCD private text is deprecated")]
		bool WritePrivateText(string value, bool append = false);

		[Obsolete("LCD private text is deprecated")]
		string GetPrivateText();

		[Obsolete("LCD private text is deprecated")]
		bool WritePrivateTitle(string value, bool append = false);

		[Obsolete("LCD private text is deprecated")]
		string GetPrivateTitle();

		[Obsolete("LCD private text is deprecated")]
		void ShowPrivateTextOnScreen();

		[Obsolete("LCD public text is deprecated")]
		bool WritePublicText(string value, bool append = false);

		[Obsolete("LCD public text is deprecated")]
		string GetPublicText();

		[Obsolete("LCD public text is deprecated")]
		bool WritePublicText(StringBuilder value, bool append = false);

		[Obsolete("LCD public text is deprecated")]
		void ReadPublicText(StringBuilder buffer, bool append = false);

		[Obsolete("LCD public text is deprecated")]
		void ShowPublicTextOnScreen();

		[Obsolete("LCD public text is deprecated")]
		void ShowTextureOnScreen();

		[Obsolete("LCD public text is deprecated")]
		void SetShowOnScreen(ShowTextOnScreenFlag set);
	}
}
