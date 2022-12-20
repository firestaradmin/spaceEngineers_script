using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Sandbox.Definitions;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Testing Tool")]
	internal class MyGuiScreenDebugTestingTool : MyGuiScreenDebugBase
	{
		private MyGuiControlListbox m_categoriesListbox;

		private MyGuiControlCheckbox m_smallGridCheckbox;

		private MyGuiControlCheckbox m_largeGridCheckbox;

		private List<string> buildCategoriesList = new List<string>();

		public MyGuiScreenDebugTestingTool()
		{
			RecreateControls(constructor: true);
		}

		private void CreateListOfBuildCategories()
		{
			foreach (KeyValuePair<string, MyGuiBlockCategoryDefinition> category in MyDefinitionManager.Static.GetCategories())
			{
				if (category.Key != null && category.Value.AvailableInSurvival && !category.Value.IsToolCategory && !category.Value.IsAnimationCategory)
				{
					buildCategoriesList.Add(category.Value.Name);
				}
			}
			buildCategoriesList.Sort();
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Test Tool Control", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddButton("Generate Tests", delegate
			{
				MyTestingToolHelper.Instance.Action_SpawnBlockSaveTestReload();
			});
			AddButton("Stop Test Generation", delegate
			{
				MyTestingToolHelper.Instance.Action1_State3_Finish();
			});
			AddButton("Spawn monolith", delegate
			{
				MyTestingToolHelper.Instance.Action_Test();
			});
			AddSlider("Screenshot distance multiplier", 0f, 5f, () => MyTestingToolHelper.ScreenshotDistanceMultiplier, delegate(float f)
			{
				MyTestingToolHelper.ScreenshotDistanceMultiplier = f;
			});
			AddSlider("Speed", 0f, 100f, () => MyTestingToolHelper.m_timer_Max, delegate(float f)
			{
				MyTestingToolHelper.m_timer_Max = (int)f;
			});
			AddLabel("Grid Selection for Automated Test Generator", Color.Yellow.ToVector4(), 1.2f);
			m_smallGridCheckbox = AddCheckBox("Small Grid", () => MyTestingToolHelper.IsSmallGridSelected, delegate(bool f)
			{
				MyTestingToolHelper.IsSmallGridSelected = f;
			});
			m_largeGridCheckbox = AddCheckBox("Large Grid", () => MyTestingToolHelper.IsLargeGridSelected, delegate(bool f)
			{
				MyTestingToolHelper.IsLargeGridSelected = f;
			});
			AddLabel("Group Selection for Automated Test Generator", Color.Yellow.ToVector4(), 1.2f);
			m_categoriesListbox = AddListBox(3f);
			m_categoriesListbox.MultiSelect = false;
			m_categoriesListbox.VisibleRowsCount = 17;
			CreateListOfBuildCategories();
			foreach (string buildCategories in buildCategoriesList)
			{
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(buildCategories));
				((Collection<MyGuiControlListbox.Item>)(object)m_categoriesListbox.Items).Add(item);
			}
			m_categoriesListbox.ItemsSelected += delegate(MyGuiControlListbox e)
			{
				if (e.SelectedItems.Count > 0)
				{
					MyTestingToolHelper.Instance.SelectedCategory = m_categoriesListbox.GetLastSelected().Text.ToString();
				}
			};
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugTestingTool";
		}
	}
}
