using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Definitions;
<<<<<<< HEAD
using Sandbox.Game.GameSystems.Chat;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions.Animation;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens.Helpers
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyEmoteSwitcher : MySessionComponentBase
	{
		private struct MyPrioritizedDefinition
		{
			public int Priority;

			public MyDefinitionBase Definition;
		}

		private class MyPrioritizedComparer : IComparer<MyPrioritizedDefinition>
		{
			public static MyPrioritizedComparer Static = new MyPrioritizedComparer();

			private MyPrioritizedComparer()
			{
			}

			public int Compare(MyPrioritizedDefinition x, MyPrioritizedDefinition y)
			{
				return y.Priority.CompareTo(x.Priority);
			}
		}

		private static readonly int PAGE_SIZE = 4;

		private List<MyPrioritizedDefinition> m_animations = new List<MyPrioritizedDefinition>();

		private int m_currentPage;

		private bool m_isActive;

		public static Vector4 DISABLED_COLOR = new Vector4(0.6f, 0.6f, 0.6f, 0.6f);

		public bool IsActive
		{
			get
			{
				return m_isActive;
			}
			private set
			{
				if (m_isActive != value)
				{
					m_isActive = value;
					this.OnActiveStateChanged?.Invoke();
				}
			}
		}

		public int AnimationCount { get; private set; }

		public int AnimationPageCount { get; private set; }

		public int CurrentPage
		{
			get
			{
				return m_currentPage;
			}
			private set
			{
				if (m_currentPage != value)
				{
					if (value < 0)
					{
						m_currentPage = 0;
					}
					else if (value < AnimationPageCount)
					{
						m_currentPage = value;
					}
					else if (AnimationPageCount >= 0)
					{
						m_currentPage = AnimationPageCount - 1;
					}
					else
					{
						m_currentPage = 0;
					}
					this.OnPageChanged?.Invoke();
				}
			}
		}

		public event Action OnActiveStateChanged;

		public event Action OnPageChanged;

		public MyEmoteSwitcher()
		{
			InitializeAnimationList();
		}

		public override void HandleInput()
		{
			if (MyScreenManager.GetScreenWithFocus() != MyGuiScreenGamePlay.Static)
			{
				return;
			}
			MyStringId context = MySession.Static.ControlledEntity?.AuxiliaryContext ?? MyStringId.NullOrEmpty;
			if (!MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SWITCHER, MyControlStateType.PRESSED))
			{
				IsActive = false;
				return;
			}
			IsActive = true;
			if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SWITCHER_LEFT))
			{
				PreviousPage();
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SWITCHER_RIGHT))
			{
				NextPage();
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SELECT_1))
			{
				ActivateEmote(0);
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SELECT_2))
			{
				ActivateEmote(1);
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SELECT_3))
			{
				ActivateEmote(2);
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SELECT_4))
			{
				ActivateEmote(3);
			}
		}

		private bool IsDefinitionValid(MyDefinitionBase def)
		{
			if (def.Icons != null)
			{
				string[] icons = def.Icons;
				for (int i = 0; i < icons.Length; i++)
				{
					if (icons[i] == null)
					{
						return false;
					}
				}
			}
			return true;
		}

		private void InitializeAnimationList()
		{
			m_animations.Clear();
			MyPrioritizedDefinition item;
			foreach (MyAnimationDefinition animationDefinition in MyDefinitionManager.Static.GetAnimationDefinitions())
			{
				if (animationDefinition.Public && IsDefinitionValid(animationDefinition))
				{
					List<MyPrioritizedDefinition> animations = m_animations;
					item = new MyPrioritizedDefinition
					{
						Definition = animationDefinition,
						Priority = animationDefinition.Priority
					};
					animations.Add(item);
				}
			}
			foreach (MyEmoteDefinition definition in MyDefinitionManager.Static.GetDefinitions<MyEmoteDefinition>())
			{
				if (definition.Public && IsDefinitionValid(definition))
				{
					List<MyPrioritizedDefinition> animations2 = m_animations;
					item = new MyPrioritizedDefinition
					{
						Definition = definition,
						Priority = definition.Priority
					};
					animations2.Add(item);
				}
			}
			m_animations.Sort(MyPrioritizedComparer.Static);
			AnimationCount = m_animations.Count;
			AnimationPageCount = ((AnimationCount % PAGE_SIZE == 0) ? (AnimationCount / PAGE_SIZE) : (AnimationCount / PAGE_SIZE + 1));
			CurrentPage = 0;
<<<<<<< HEAD
			MyRenderProxy.PreloadTextures(m_animations.SelectMany((MyPrioritizedDefinition x) => x.Definition.Icons), TextureType.GUI);
=======
			MyRenderProxy.PreloadTextures(Enumerable.SelectMany<MyPrioritizedDefinition, string>((IEnumerable<MyPrioritizedDefinition>)m_animations, (Func<MyPrioritizedDefinition, IEnumerable<string>>)((MyPrioritizedDefinition x) => x.Definition.Icons)), TextureType.GUI);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public string GetIconUp()
		{
			return GetIcon(0);
		}

		public string GetIconLeft()
		{
			return GetIcon(1);
		}

		public string GetIconRight()
		{
			return GetIcon(2);
		}

		public string GetIconDown()
		{
			return GetIcon(3);
		}

		public string GetIcon(int id)
		{
			int linearIndex = LinearizeIndex(id);
			return GetIconLinear(linearIndex);
		}

		private void NextPage()
		{
			CurrentPage++;
		}

		private void PreviousPage()
		{
			CurrentPage--;
		}

		public string GetIconLinear(int linearIndex)
		{
			if (linearIndex < 0 || linearIndex >= AnimationCount)
			{
				return string.Empty;
			}
			MyAnimationDefinition myAnimationDefinition;
			if ((myAnimationDefinition = m_animations[linearIndex].Definition as MyAnimationDefinition) != null)
			{
				if (myAnimationDefinition.Icons.Length == 0)
				{
					return string.Empty;
				}
				return myAnimationDefinition.Icons[0];
			}
			MyEmoteDefinition myEmoteDefinition;
			if ((myEmoteDefinition = m_animations[linearIndex].Definition as MyEmoteDefinition) != null)
			{
				if (myEmoteDefinition.Icons.Length == 0)
				{
					return string.Empty;
				}
				return myEmoteDefinition.Icons[0];
			}
			return string.Empty;
		}

		private void ActivateEmote(int id)
		{
			int linearIndex = LinearizeIndex(id);
			ActivateEmoteLinear(linearIndex);
		}

		private void ActivateEmoteLinear(int linearIndex)
		{
<<<<<<< HEAD
			if (linearIndex < 0 || linearIndex >= AnimationCount || !HasNecessaryDLCsLinear(linearIndex, out var _))
			{
				return;
			}
			MySession.Static.ControlledEntity.SwitchToWeapon(null);
			MyAnimationDefinition animationDefinition;
			if ((animationDefinition = m_animations[linearIndex].Definition as MyAnimationDefinition) != null)
			{
				MyAnimationActivator.Activate(animationDefinition);
			}
			else
			{
				MyEmoteDefinition myEmoteDefinition;
				if ((myEmoteDefinition = m_animations[linearIndex].Definition as MyEmoteDefinition) == null)
=======
			if (linearIndex >= 0 && linearIndex < AnimationCount && HasNecessaryDLCsLinear(linearIndex, out var _))
			{
				MySession.Static.ControlledEntity.SwitchToWeapon(null);
				MyAnimationDefinition animationDefinition;
				MyEmoteDefinition emoteDefinition;
				if ((animationDefinition = m_animations[linearIndex].Definition as MyAnimationDefinition) != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return;
				}
<<<<<<< HEAD
				if (myEmoteDefinition.Animations != null && myEmoteDefinition.Animations.Length != 0)
=======
				else if ((emoteDefinition = m_animations[linearIndex].Definition as MyEmoteDefinition) != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					string winningAnimationName = AnimationCommands.GetWinningAnimationName(myEmoteDefinition.Animations);
					foreach (MyEmoteDefinition emoteDefinition in MyDefinitionManager.Static.GetEmoteDefinitions())
					{
						if (winningAnimationName == emoteDefinition.Id.SubtypeId.String)
						{
							MyAnimationActivator.Activate(emoteDefinition);
							break;
						}
					}
				}
				else
				{
					MyAnimationActivator.Activate(myEmoteDefinition);
				}
			}
		}

		private int LinearizeIndex(int id)
		{
			return m_currentPage * PAGE_SIZE + id;
		}

		private bool HasNecessaryDLCs(int index, out string icon)
		{
			return HasNecessaryDLCsLinear(LinearizeIndex(index), out icon);
		}

		private bool HasNecessaryDLCsLinear(int linearIndex, out string icon)
		{
			if (linearIndex < 0 || linearIndex >= AnimationCount)
			{
				icon = string.Empty;
				return false;
			}
			return HasNecessaryDLCs(m_animations[linearIndex].Definition, out icon);
		}

		private bool HasNecessaryDLCs(MyDefinitionBase definition, out string icon)
		{
			if (definition.DLCs != null && definition.DLCs.Length != 0)
			{
				MyDLCs.MyDLC firstMissingDefinitionDLC = MySession.Static.GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC(definition, Sync.MyId);
				if (firstMissingDefinitionDLC != null)
				{
					icon = firstMissingDefinitionDLC.Icon;
					return false;
				}
				if (MyDLCs.TryGetDLC(definition.DLCs[0], out var dlc))
				{
					icon = dlc.Icon;
				}
				else
				{
					icon = string.Empty;
				}
				return true;
			}
			icon = string.Empty;
			return true;
		}

		public Vector4 GetIconMask(int i)
		{
			if (HasNecessaryDLCs(i, out var _))
			{
				return Vector4.One;
			}
			return DISABLED_COLOR;
		}

		internal Vector4 GetIconUpMask()
		{
			return GetIconMask(0);
		}

		internal Vector4 GetIconLeftMask()
		{
			return GetIconMask(1);
		}

		internal Vector4 GetIconRightMask()
		{
			return GetIconMask(2);
		}

		internal Vector4 GetIconDownMask()
		{
			return GetIconMask(3);
		}

		public string GetSubIcon(int i)
		{
			HasNecessaryDLCs(i, out var icon);
			return icon;
		}

		internal string GetSubIconUp()
		{
			return GetSubIcon(0);
		}

		internal string GetSubIconLeft()
		{
			return GetSubIcon(1);
		}

		internal string GetSubIconRight()
		{
			return GetSubIcon(2);
		}

		internal string GetSubIconDown()
		{
			return GetSubIcon(3);
		}
	}
}
