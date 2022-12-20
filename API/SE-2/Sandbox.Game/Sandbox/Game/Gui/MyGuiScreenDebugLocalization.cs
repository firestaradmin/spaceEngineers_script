<<<<<<< HEAD
=======
using System.Collections.ObjectModel;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Text;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using VRage;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Localization")]
	internal class MyGuiScreenDebugLocalization : MyGuiScreenDebugBase
	{
		private MyGuiControlListbox m_quotesListbox;

		private MyGuiControlMultilineText m_quotesDisplay;

		public MyGuiScreenDebugLocalization()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Localization", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f * m_scale;
			AddLabel("Loading Screen Texts", Color.Yellow.ToVector4(), 1.2f);
			m_quotesListbox = AddListBox(0.185f);
			m_quotesListbox.MultiSelect = false;
			m_quotesListbox.VisibleRowsCount = 5;
			foreach (MyLoadingScreenText item2 in MyLoadingScreenText.TextsShared)
			{
				StringBuilder stringBuilder = new StringBuilder();
				MyLoadingScreenQuote myLoadingScreenQuote = item2 as MyLoadingScreenQuote;
				if (myLoadingScreenQuote != null)
				{
					stringBuilder.Append((object)MyTexts.Get(item2.Text));
					stringBuilder.AppendLine();
					stringBuilder.AppendLine().Append("- ").AppendStringBuilder(MyTexts.Get(myLoadingScreenQuote.Author))
						.Append(" -");
				}
				else
				{
					stringBuilder.AppendLine(item2.ToString());
				}
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(item2.Text.String), stringBuilder.ToString(), null, stringBuilder);
				((Collection<MyGuiControlListbox.Item>)(object)m_quotesListbox.Items).Add(item);
			}
			m_quotesDisplay = AddMultilineText(new Vector2(m_quotesListbox.Size.X, 0.2f));
			m_quotesDisplay.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
			m_quotesListbox.ItemsSelected += delegate(MyGuiControlListbox e)
			{
				m_quotesDisplay.Clear();
				if (e.SelectedItems.Count > 0)
				{
					m_quotesDisplay.AppendText((StringBuilder)e.GetLastSelected().UserData);
				}
			};
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugLocalization";
		}
	}
}
