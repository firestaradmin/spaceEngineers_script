using System.Collections.Generic;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders;
using VRage.Network;

namespace Sandbox.Game.Screens.Helpers
{
	[MyDefinitionType(typeof(MyObjectBuilder_RadialMenu), null)]
	public class MyRadialMenu : MyDefinitionBase
	{
		private class Sandbox_Game_Screens_Helpers_MyRadialMenu_003C_003EActor : IActivator, IActivator<MyRadialMenu>
		{
			private sealed override object CreateInstance()
			{
				return new MyRadialMenu();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRadialMenu CreateInstance()
			{
				return new MyRadialMenu();
			}

			MyRadialMenu IActivator<MyRadialMenu>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyRadialMenuSection> SectionsComplete;

		public List<MyRadialMenuSection> SectionsCreative;

		public List<MyRadialMenuSection> SectionsSurvival;

		public List<MyRadialMenuSection> CurrentSections
		{
			get
			{
				if (MySession.Static == null)
				{
					return SectionsComplete;
				}
				if (MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySession.Static.CreativeMode)
				{
					return SectionsComplete;
				}
				return SectionsSurvival;
			}
		}

		public MyRadialMenu()
		{
		}

		public MyRadialMenu(List<MyRadialMenuSection> sections)
		{
			SectionsComplete = sections;
			SectionsCreative = new List<MyRadialMenuSection>();
			SectionsSurvival = new List<MyRadialMenuSection>();
			foreach (MyRadialMenuSection section in sections)
			{
				if (section.IsEnabledCreative)
				{
					SectionsCreative.Add(section);
				}
				if (section.IsEnabledSurvival)
				{
					SectionsSurvival.Add(section);
				}
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_RadialMenu myObjectBuilder_RadialMenu = builder as MyObjectBuilder_RadialMenu;
			if (myObjectBuilder_RadialMenu == null)
			{
				return;
			}
			SectionsComplete = new List<MyRadialMenuSection>();
			SectionsCreative = new List<MyRadialMenuSection>();
			SectionsSurvival = new List<MyRadialMenuSection>();
			MyObjectBuilder_RadialMenuSection[] sections = myObjectBuilder_RadialMenu.Sections;
			foreach (MyObjectBuilder_RadialMenuSection builder2 in sections)
			{
				MyRadialMenuSection myRadialMenuSection = new MyRadialMenuSection();
				myRadialMenuSection.Init(builder2);
				SectionsComplete.Add(myRadialMenuSection);
				if (myRadialMenuSection.IsEnabledCreative)
				{
					SectionsCreative.Add(myRadialMenuSection);
				}
				if (myRadialMenuSection.IsEnabledSurvival)
				{
					SectionsSurvival.Add(myRadialMenuSection);
				}
			}
		}

		public override void Postprocess()
		{
			base.Postprocess();
			for (int i = 0; i < SectionsComplete.Count; i++)
			{
				MyRadialMenuSection myRadialMenuSection = SectionsComplete[i];
				myRadialMenuSection.Postprocess();
				if (myRadialMenuSection.Items.Count == 0)
				{
					SectionsComplete.RemoveAt(i);
					SectionsCreative.Remove(myRadialMenuSection);
					SectionsSurvival.Remove(myRadialMenuSection);
					i--;
				}
			}
		}
	}
}
