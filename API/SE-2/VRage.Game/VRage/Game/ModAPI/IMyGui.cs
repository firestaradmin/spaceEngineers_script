using System;
using VRage.ModAPI;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// ModAPI interface giving access to GUI
	/// </summary>
	public interface IMyGui
	{
		/// <summary>
		/// Gets the name of the currently open GUI screen.
		/// </summary>
		string ActiveGamePlayScreen { get; }

		/// <summary>
		/// Gets the entity the player is currently interacting with.
		/// </summary>
		IMyEntity InteractedEntity { get; }

		/// <summary>
		/// Gets an enum describing the currently open GUI screen.
		/// </summary>
		MyTerminalPageEnum GetCurrentScreen { get; }

		/// <summary>
		/// Checks if the chat entry box is visible.
		/// </summary>
		bool ChatEntryVisible { get; }

		/// <summary>
		/// Checks if the cursor is visible.
		/// </summary>
		bool IsCursorVisible { get; }

		/// <summary>
		/// Event triggered on gui control created.
		/// </summary>
		event Action<object> GuiControlCreated;

		/// <summary>
		/// Event triggered on gui control removed.
		/// </summary>
		event Action<object> GuiControlRemoved;

		/// <summary>
		/// Shows the terminal and opens a specific tab.
		/// </summary>
		/// <param name="page">Tab to open.</param>
		/// <param name="user">The user that will interact with the terminal.</param>
		/// <param name="interactedEntity">The entity the terminal page will be shown for. <see langword="null" /> for player.</param>
		/// <param name="isRemote">If terminal refers to a remote entity (over antenna).</param>
		void ShowTerminalPage(MyTerminalPageEnum page, IMyCharacter user, IMyEntity interactedEntity = null, bool isRemote = false);

		/// <summary>
		/// Switches the entity the terminal is activated for.
		/// </summary>
		/// <param name="interactedEntity">The entity the terminal page will be shown for. <see langword="null" /> for player.</param>
		/// <param name="isRemote">If terminal refers to a remote entity (over antenna).</param>
		void ChangeInteractedEntity(IMyEntity interactedEntity, bool isRemote);
	}
}
