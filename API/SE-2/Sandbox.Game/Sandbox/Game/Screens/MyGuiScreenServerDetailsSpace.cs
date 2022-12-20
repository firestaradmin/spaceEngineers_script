using System.Collections.Generic;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenServerDetailsSpace : MyGuiScreenServerDetailsBase
	{
		public MyGuiScreenServerDetailsSpace(MyCachedServerItem server)
			: base(server)
		{
		}

		protected override void DrawSettings()
		{
			if (m_server.Rules != null)
			{
				m_server.Rules.TryGetValue("SM", out var value);
				if (!string.IsNullOrEmpty(value))
				{
					AddLabel(MySpaceTexts.ServerDetails_ServerManagement, value);
				}
			}
			if (!string.IsNullOrEmpty(m_server.Description))
			{
				AddLabel(MyCommonTexts.Description, null);
				m_currentPosition.Y += 0.008f;
				AddMultilineText(m_server.Description, 0.15f);
				m_currentPosition.Y += 0.008f;
			}
			MyGuiControlLabel myGuiControlLabel = AddLabel(MyCommonTexts.ServerDetails_WorldSettings, null);
			SortedList<string, object> val = LoadSessionSettings(VRage.Game.Game.SpaceEngineers);
			if (val == null)
			{
				Controls.Add(new MyGuiControlLabel(m_currentPosition, null, MyTexts.GetString(MyCommonTexts.ServerDetails_SettingError), null, 0.8f, "Red"));
				return;
			}
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent();
			MyGuiControlScrollablePanel myGuiControlScrollablePanel = new MyGuiControlScrollablePanel(myGuiControlParent);
			myGuiControlScrollablePanel.ScrollbarVEnabled = true;
			myGuiControlScrollablePanel.Position = m_currentPosition;
			myGuiControlScrollablePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlScrollablePanel.Size = new Vector2(base.Size.Value.X - 0.112f, base.Size.Value.Y / 2f - m_currentPosition.Y - 0.145f);
			myGuiControlScrollablePanel.BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST;
			myGuiControlScrollablePanel.ScrolledAreaPadding = new MyGuiBorderThickness(0.005f);
			myGuiControlScrollablePanel.CanHaveFocus = true;
			myGuiControlScrollablePanel.CanFocusChildren = false;
			base.FocusedControl = myGuiControlScrollablePanel;
			Controls.Add(myGuiControlScrollablePanel);
			Vector2 vector = -myGuiControlScrollablePanel.Size / 2f;
			float num = 0f;
			foreach (KeyValuePair<string, object> item in val)
			{
				num += myGuiControlLabel.Size.Y / 2f + m_padding;
				SerializableDictionary<string, short> serializableDictionary = item.Value as SerializableDictionary<string, short>;
				if (serializableDictionary != null)
				{
					int count = serializableDictionary.Dictionary.Count;
					num += (myGuiControlLabel.Size.Y / 2f + m_padding) * (float)count;
				}
			}
			vector.Y = (0f - num) / 2f + myGuiControlLabel.Size.Y / 2f;
			myGuiControlParent.Size = new Vector2(myGuiControlScrollablePanel.Size.X, num);
			foreach (KeyValuePair<string, object> item2 in val)
			{
				object value2 = item2.Value;
				if (!(value2 is SerializableDictionary<string, short>))
				{
					string text = string.Empty;
					if (value2 is bool)
					{
						text = (((bool)value2) ? MyTexts.GetString(MyCommonTexts.ControlMenuItemValue_On) : MyTexts.GetString(MyCommonTexts.ControlMenuItemValue_Off));
					}
					else if (value2 != null)
					{
						text = value2.ToString();
					}
					MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(vector, null, string.Concat(MyTexts.Get(MyStringId.GetOrCompute(item2.Key)), ":"));
					MyGuiControlLabel control = new MyGuiControlLabel(new Vector2(myGuiControlScrollablePanel.Size.X / 2.5f, vector.Y), null, text, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
					vector.Y += myGuiControlLabel2.Size.Y / 2f + m_padding;
					myGuiControlParent.Controls.Add(myGuiControlLabel2);
					myGuiControlParent.Controls.Add(control);
					AddSeparator(myGuiControlParent, vector);
					continue;
				}
				Dictionary<string, short> dictionary = (value2 as SerializableDictionary<string, short>).Dictionary;
				if (dictionary == null)
				{
					continue;
				}
				MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(vector, null, string.Concat(MyTexts.Get(MyStringId.GetOrCompute(item2.Key)), ":"));
				myGuiControlParent.Controls.Add(myGuiControlLabel3);
				vector.Y += myGuiControlLabel3.Size.Y / 2f + m_padding;
				AddSeparator(myGuiControlParent, vector);
				foreach (KeyValuePair<string, short> item3 in dictionary)
				{
					MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(vector, null, "     " + item3.Key);
					MyGuiControlLabel control2 = new MyGuiControlLabel(new Vector2(myGuiControlScrollablePanel.Size.X / 2.5f, vector.Y), null, item3.Value.ToString(), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
					myGuiControlParent.Controls.Add(myGuiControlLabel4);
					myGuiControlParent.Controls.Add(control2);
					vector.Y += myGuiControlLabel4.Size.Y / 2f + m_padding;
					AddSeparator(myGuiControlParent, vector);
				}
			}
		}
	}
}
