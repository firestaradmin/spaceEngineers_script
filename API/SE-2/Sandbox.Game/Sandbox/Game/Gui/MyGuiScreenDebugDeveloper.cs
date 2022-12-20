using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sandbox.Engine.Utils;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Plugins;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenDebugDeveloper : MyGuiScreenDebugBase
	{
		private class MyDevelopGroup
		{
			public string Name;

			public MyGuiControlBase GroupControl;

			public List<MyGuiControlBase> ControlList;

			public MyDevelopGroup(string name)
			{
				Name = name;
				ControlList = new List<MyGuiControlBase>();
			}
		}

		private class MyDevelopGroupTypes
		{
			public Type Name;

			public MyDirectXSupport DirectXSupport;

			public MyDevelopGroupTypes(Type name, MyDirectXSupport directXSupport)
			{
				Name = name;
				DirectXSupport = directXSupport;
			}
		}

		private class DevelopGroupComparer : IComparer<string>
		{
			public int Compare(string x, string y)
			{
				if (x == "Game" && y == "Game")
				{
					return 0;
				}
				if (x == "Game")
				{
					return -1;
				}
				if (y == "Game")
				{
					return 1;
				}
				if (x == "Render" && y == "Render")
				{
					return 0;
				}
				if (x == "Render")
				{
					return -1;
				}
				if (y == "Render")
				{
					return 1;
				}
				return x.CompareTo(y);
			}
		}

		private static MyGuiScreenBase s_activeScreen;

		private static List<MyGuiControlCheckbox> s_groupList;

		private static List<MyGuiControlCheckbox> s_inputList;

		private static MyDevelopGroup s_debugDrawGroup;

		private static MyDevelopGroup s_performanceGroup;

		private static List<MyDevelopGroup> s_mainGroups;

		private static MyDevelopGroup s_activeMainGroup;

		private static MyDevelopGroup s_debugInputGroup;

		private static MyDevelopGroup s_activeDevelopGroup;

		private static SortedDictionary<string, MyDevelopGroup> s_developGroups;

		private static Dictionary<string, SortedDictionary<string, MyDevelopGroupTypes>> s_developScreenTypes;

		private static bool EnableProfiler
		{
			get
			{
				return VRage.Profiler.MyRenderProfiler.ProfilerVisible;
			}
			set
			{
				if (VRage.Profiler.MyRenderProfiler.ProfilerVisible != value)
				{
					MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.Enable, 0, null);
					MyStatsGraph.Start();
				}
			}
		}

		private static void RegisterScreensFromAssembly(Assembly[] assemblies)
		{
			if (assemblies != null)
			{
				for (int i = 0; i < assemblies.Length; i++)
				{
					RegisterScreensFromAssembly(assemblies[i]);
				}
			}
		}

		private static void RegisterScreensFromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				return;
			}
			Type typeFromHandle = typeof(MyGuiScreenBase);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (!typeFromHandle.IsAssignableFrom(type))
				{
					continue;
				}
				object[] customAttributes = type.GetCustomAttributes(typeof(MyDebugScreenAttribute), inherit: false);
				if (customAttributes.Length != 0)
				{
					MyDebugScreenAttribute myDebugScreenAttribute = (MyDebugScreenAttribute)customAttributes[0];
					if (!s_developScreenTypes.TryGetValue(myDebugScreenAttribute.Group, out var value))
					{
						value = new SortedDictionary<string, MyDevelopGroupTypes>();
						s_developScreenTypes.Add(myDebugScreenAttribute.Group, value);
						s_developGroups.Add(myDebugScreenAttribute.Group, new MyDevelopGroup(myDebugScreenAttribute.Group));
					}
					MyDevelopGroupTypes myDevelopGroupTypes = new MyDevelopGroupTypes(type, myDebugScreenAttribute.DirectXSupport);
					value.Add(myDebugScreenAttribute.Name, myDevelopGroupTypes);
				}
			}
		}

		static MyGuiScreenDebugDeveloper()
		{
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			s_groupList = new List<MyGuiControlCheckbox>();
			s_inputList = new List<MyGuiControlCheckbox>();
			s_debugDrawGroup = new MyDevelopGroup("Debug draw");
			s_performanceGroup = new MyDevelopGroup("Performance");
			s_mainGroups = new List<MyDevelopGroup> { s_debugDrawGroup, s_performanceGroup };
			s_activeMainGroup = s_debugDrawGroup;
			s_debugInputGroup = new MyDevelopGroup("Debug Input");
			s_developGroups = new SortedDictionary<string, MyDevelopGroup>((IComparer<string>)new DevelopGroupComparer());
			s_developScreenTypes = new Dictionary<string, SortedDictionary<string, MyDevelopGroupTypes>>();
			RegisterScreensFromAssembly(Assembly.GetExecutingAssembly());
			RegisterScreensFromAssembly(MyPlugins.GameAssembly);
			RegisterScreensFromAssembly(MyPlugins.SandboxAssembly);
			RegisterScreensFromAssembly(MyPlugins.UserAssemblies);
			s_developGroups.Add(s_debugInputGroup.Name, s_debugInputGroup);
			Enumerator<string, MyDevelopGroup> enumerator = s_developGroups.get_Values().GetEnumerator();
			enumerator.MoveNext();
			s_activeDevelopGroup = enumerator.get_Current();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Sandbox.Game.Gui.MyGuiScreenDebugDeveloper" /> class.
		/// </summary>
		public MyGuiScreenDebugDeveloper()
			: base(new Vector2(0.5f, 0.5f), new Vector2(0.35f, 1f), 0.35f * Color.Yellow.ToVector4(), isTopMostScreen: true)
		{
			m_backgroundColor = null;
			base.EnabledBackgroundFade = true;
			m_backgroundFadeColor = new Color(1f, 1f, 1f, 0.2f);
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
			base.RecreateControls(constructor);
			s_groupList.Clear();
<<<<<<< HEAD
			foreach (MyDevelopGroup value4 in s_developGroups.Values)
=======
			Enumerator<string, MyDevelopGroup> enumerator = s_developGroups.get_Values().GetEnumerator();
			try
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				while (enumerator.MoveNext())
				{
					MyDevelopGroup current = enumerator.get_Current();
					if (current.ControlList.Count > 0)
					{
						EnableGroup(current, enable: false);
						current.ControlList.Clear();
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			foreach (MyDevelopGroup s_mainGroup in s_mainGroups)
			{
				if (s_mainGroup.ControlList.Count > 0)
				{
					EnableGroup(s_mainGroup, enable: false);
					s_mainGroup.ControlList.Clear();
				}
			}
			float num = -0.02f;
			AddCaption("Developer screen", Color.Yellow.ToVector4(), new Vector2(0f, num));
			m_scale = 0.9f;
			m_closeOnEsc = true;
			Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
			bool flag = (float)fullscreenRectangle.Width / (float)fullscreenRectangle.Height >= 1.66666663f;
			if (!flag)
			{
				m_currentPosition = -m_size.Value / 2f + new Vector2(0.03f, 0.1f);
				m_currentPosition.X = (0f - m_size.Value.X) * 0.8f;
			}
			else
			{
				m_currentPosition = -m_size.Value / 2f + new Vector2(0.03f, 0.1f);
			}
			m_currentPosition.Y += num;
			float num2 = 0f;
			Vector2 vector = new Vector2(0.09f, 0.03f);
			foreach (MyDevelopGroup s_mainGroup2 in s_mainGroups)
			{
				Vector2 value = new Vector2(-0.03f + m_currentPosition.X + num2, m_currentPosition.Y);
				s_mainGroup2.GroupControl = new MyGuiControlButton(value, MyGuiControlButtonStyleEnum.Debug, null, new Vector4(1f, 1f, 0.5f, 1f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, new StringBuilder(s_mainGroup2.Name), MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE * MyGuiConstants.DEBUG_LABEL_TEXT_SCALE * m_scale * 1.2f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnClickMainGroup);
				num2 += s_mainGroup2.GroupControl.Size.X * 1.1f;
				Controls.Add(s_mainGroup2.GroupControl);
			}
			m_currentPosition.Y += vector.Y * 1.1f;
			float y = m_currentPosition.Y;
			float value2 = y;
			CreateDebugDrawControls();
			value2 = MathHelper.Max(value2, m_currentPosition.Y);
			m_currentPosition.Y = y;
			CreatePerformanceControls();
			m_currentPosition.Y = MathHelper.Max(value2, m_currentPosition.Y);
			foreach (MyDevelopGroup s_mainGroup3 in s_mainGroups)
			{
				EnableGroup(s_mainGroup3, enable: false);
			}
			EnableGroup(s_activeMainGroup, enable: true);
			m_currentPosition.Y += 0.02f;
			num2 = 0f;
			enumerator = s_developGroups.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyDevelopGroup current5 = enumerator.get_Current();
					Vector2 value3 = new Vector2(-0.03f + m_currentPosition.X + num2, m_currentPosition.Y);
					current5.GroupControl = new MyGuiControlButton(value3, MyGuiControlButtonStyleEnum.Debug, null, new Vector4(1f, 1f, 0.5f, 1f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, new StringBuilder(current5.Name), 0.8f * MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE * m_scale * 1.2f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnClickGroup);
					num2 += current5.GroupControl.Size.X * 1.1f;
					Controls.Add(current5.GroupControl);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			num2 = (0f - num2) / 2f + (flag ? 0f : (-0.165f));
			enumerator = s_developGroups.get_Values().GetEnumerator();
			try
			{
<<<<<<< HEAD
				Vector2 value3 = new Vector2(-0.03f + m_currentPosition.X + num2, m_currentPosition.Y);
				value5.GroupControl = new MyGuiControlButton(value3, MyGuiControlButtonStyleEnum.Debug, null, new Vector4(1f, 1f, 0.5f, 1f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, new StringBuilder(value5.Name), 0.8f * MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE * m_scale * 1.2f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnClickGroup);
				num2 += value5.GroupControl.Size.X * 1.1f;
				Controls.Add(value5.GroupControl);
			}
			num2 = (0f - num2) / 2f + (flag ? 0f : (-0.165f));
			foreach (MyDevelopGroup value6 in s_developGroups.Values)
=======
				while (enumerator.MoveNext())
				{
					MyDevelopGroup current6 = enumerator.get_Current();
					current6.GroupControl.PositionX = num2;
					num2 += current6.GroupControl.Size.X * 1.1f;
				}
			}
			finally
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				((IDisposable)enumerator).Dispose();
			}
			m_currentPosition.Y += vector.Y * 1.1f;
			float x = m_currentPosition.X;
			float y2 = m_currentPosition.Y;
			string text = MySandboxGame.Config.GraphicsRenderer.ToString();
			MyStringId directX11RendererKey = MySandboxGame.DirectX11RendererKey;
			bool flag2 = text == directX11RendererKey.ToString();
			foreach (KeyValuePair<string, SortedDictionary<string, MyDevelopGroupTypes>> s_developScreenType in s_developScreenTypes)
			{
<<<<<<< HEAD
				MyDevelopGroup myDevelopGroup = s_developGroups[s_developScreenType.Key];
				int num3 = 20;
				float num4 = 0.3f;
				int num5 = s_developScreenType.Value.Count / num3;
				float num6 = num4 * (float)num5;
				int num7 = 0;
				m_currentPosition.X -= num6 / 2f;
				List<KeyValuePair<string, MyDevelopGroupTypes>> list = s_developScreenType.Value.ToList();
=======
				MyDevelopGroup myDevelopGroup = s_developGroups.get_Item(s_developScreenType.Key);
				int num3 = 20;
				float num4 = 0.3f;
				int num5 = s_developScreenType.Value.get_Count() / num3;
				float num6 = num4 * (float)num5;
				int num7 = 0;
				m_currentPosition.X -= num6 / 2f;
				List<KeyValuePair<string, MyDevelopGroupTypes>> list = Enumerable.ToList<KeyValuePair<string, MyDevelopGroupTypes>>((IEnumerable<KeyValuePair<string, MyDevelopGroupTypes>>)s_developScreenType.Value);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				for (int i = 0; i < num5 + 1; i++)
				{
					int num8 = 0;
					while (num8 < num3 && num7 < list.Count)
					{
						KeyValuePair<string, MyDevelopGroupTypes> keyValuePair = list[num7];
						if ((int)keyValuePair.Value.DirectXSupport >= 1 && flag2)
						{
							AddGroupBox(keyValuePair.Key, keyValuePair.Value.Name, myDevelopGroup.ControlList, new Vector2(-0.05f, 0f));
						}
						num8++;
						num7++;
					}
					m_currentPosition.X += num4;
					m_currentPosition.Y = y2;
				}
				m_currentPosition.Y = y2;
				m_currentPosition.X = x;
			}
			if (MyGuiSandbox.Gui is MyDX9Gui)
			{
				for (int j = 0; j < (MyGuiSandbox.Gui as MyDX9Gui).UserDebugInputComponents.Count; j++)
				{
					AddGroupInput($"{(MyGuiSandbox.Gui as MyDX9Gui).UserDebugInputComponents[j].GetName()} (Ctrl + numPad{j})", (MyGuiSandbox.Gui as MyDX9Gui).UserDebugInputComponents[j], s_debugInputGroup.ControlList);
				}
			}
			m_currentPosition.Y = y2;
			enumerator = s_developGroups.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyDevelopGroup current8 = enumerator.get_Current();
					EnableGroup(current8, enable: false);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			EnableGroup(s_activeDevelopGroup, enable: true);
		}

		private void CreateDebugDrawControls()
		{
			AddCheckBox("Debug draw", null, MemberHelper.GetMember(() => MyDebugDrawSettings.ENABLE_DEBUG_DRAW), enabled: true, s_debugDrawGroup.ControlList);
			AddCheckBox("Draw physics", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS), enabled: true, s_debugDrawGroup.ControlList);
			AddCheckBox("Audio debug draw", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_AUDIO), enabled: true, s_debugDrawGroup.ControlList);
			AddButton(new StringBuilder("Clear persistent"), delegate
			{
				MyRenderProxy.DebugClearPersistentMessages();
			}, s_debugDrawGroup.ControlList);
			m_currentPosition.Y += 0.01f;
		}

		private void CreatePerformanceControls()
		{
			AddCheckBox("Profiler", () => EnableProfiler, delegate(bool v)
			{
				EnableProfiler = v;
			}, enabled: true, s_performanceGroup.ControlList);
			AddCheckBox("Particles", null, MemberHelper.GetMember(() => MyParticlesManager.Enabled), enabled: true, s_performanceGroup.ControlList);
			m_currentPosition.Y += 0.01f;
		}

		protected void AddGroupInput(string text, MyDebugComponent component, List<MyGuiControlBase> controlGroup = null)
		{
			MyGuiControlCheckbox item = AddCheckBox(text, component, controlGroup);
			s_inputList.Add(item);
		}

		private void AddGroupBox(string text, Type screenType, List<MyGuiControlBase> controlGroup, Vector2? checkboxOffset = null)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = AddCheckBox(text, checkedState: true, null, enabled: true, controlGroup, null, checkboxOffset);
			myGuiControlCheckbox.IsChecked = s_activeScreen != null && s_activeScreen.GetType() == screenType;
			myGuiControlCheckbox.UserData = screenType;
			s_groupList.Add(myGuiControlCheckbox);
			myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, (Action<MyGuiControlCheckbox>)delegate(MyGuiControlCheckbox sender)
			{
				Type type = sender.UserData as Type;
				if (sender.IsChecked)
				{
					foreach (MyGuiControlCheckbox s_group in s_groupList)
					{
						if (s_group != sender)
						{
							s_group.IsChecked = false;
						}
					}
					MyGuiScreenBase obj = (MyGuiScreenBase)Activator.CreateInstance(type);
					obj.Closed += delegate(MyGuiScreenBase source, bool isUnloading)
					{
						if (source == s_activeScreen)
						{
							s_activeScreen = null;
						}
					};
					MyGuiSandbox.AddScreen(obj);
					s_activeScreen = obj;
				}
				else if (s_activeScreen != null && s_activeScreen.GetType() == type)
				{
					s_activeScreen.CloseScreen();
				}
			});
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugDeveloper";
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F12))
			{
				CloseScreen();
			}
		}

		private void OnClickGroup(MyGuiControlButton sender)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			EnableGroup(s_activeDevelopGroup, enable: false);
			Enumerator<string, MyDevelopGroup> enumerator = s_developGroups.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyDevelopGroup current = enumerator.get_Current();
					if (current.GroupControl == sender)
					{
						s_activeDevelopGroup = current;
						break;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			EnableGroup(s_activeDevelopGroup, enable: true);
		}

		private void OnClickMainGroup(MyGuiControlButton sender)
		{
			EnableGroup(s_activeMainGroup, enable: false);
			foreach (MyDevelopGroup s_mainGroup in s_mainGroups)
			{
				if (s_mainGroup.GroupControl == sender)
				{
					s_activeMainGroup = s_mainGroup;
					break;
				}
			}
			EnableGroup(s_activeMainGroup, enable: true);
		}

		private void EnableGroup(MyDevelopGroup group, bool enable)
		{
			foreach (MyGuiControlBase control in group.ControlList)
			{
				control.Visible = enable;
			}
		}
	}
}
