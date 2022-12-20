<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Linq;
using Sandbox.Definitions;
using Sandbox.Graphics.GUI;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Network;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	internal class MyAlesDebugInputComponent : MyDebugComponent
	{
		private bool m_questlogOpened;

		private static MyRandom random = new MyRandom();

		private MyRandom m_random;

		public override string GetName()
		{
			return "Ales";
		}

		public MyAlesDebugInputComponent()
		{
			m_random = new MyRandom();
			AddShortcut(MyKeys.U, newPress: true, control: false, shift: false, alt: false, () => "Reload particles", delegate
			{
				ReloadParticleDefinition();
				return true;
			});
			AddShortcut(MyKeys.NumPad0, newPress: true, control: false, shift: false, alt: false, () => "Teleport to gps", delegate
			{
				TravelToWaypointClient();
				return true;
			});
			AddShortcut(MyKeys.NumPad0, newPress: true, control: false, shift: false, alt: false, () => "Init questlog", delegate
			{
				ToggleQuestlog();
				return true;
			});
			AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "Show/Hide QL", delegate
			{
				m_questlogOpened = !m_questlogOpened;
				MyHud.Questlog.Visible = m_questlogOpened;
				return true;
			});
			AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "QL: Prew page", () => true);
			AddShortcut(MyKeys.NumPad3, newPress: true, control: false, shift: false, alt: false, () => "QL: Next page", () => true);
			int shortLine = 30;
			AddShortcut(MyKeys.NumPad4, newPress: true, control: false, shift: false, alt: false, () => "QL: Add short line", delegate
			{
				MyHud.Questlog.AddDetail(RandomString(shortLine));
				return true;
			});
			int longLine = 60;
			AddShortcut(MyKeys.NumPad5, newPress: true, control: false, shift: false, alt: false, () => "QL: Add long line", delegate
			{
				MyHud.Questlog.AddDetail(RandomString(longLine));
				return true;
			});
		}

		private void ToggleQuestlog()
		{
			MyHud.Questlog.QuestTitle = "Test Questlog title message";
			MyHud.Questlog.CleanDetails();
		}

		private void TravelToWaypointClient()
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenDialogTeleportCheat());
		}

		private void ReloadParticleDefinition()
		{
			MyDefinitionManager.Static.ReloadParticles();
		}

		public static string RandomString(int length)
		{
			return new string(Enumerable.ToArray<char>(Enumerable.Select<string, char>(Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789           ", length), (Func<string, char>)((string s) => s[random.Next(s.Length)])))).Trim();
		}
	}
}
