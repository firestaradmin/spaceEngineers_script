using System;
using System.Collections.Generic;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using VRage;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public class MyHudBlockInfo
	{
		[Flags]
		public enum WhoWantsInfoDisplayed
		{
			None = 0x0,
			Tool = 0x1,
			CubeBuilder = 0x2,
			Cockpit = 0x4,
			Clipboard = 0x8
		}

		public struct ComponentInfo
		{
			public MyDefinitionId DefinitionId;

			public string[] Icons;

			public string ComponentName;

			public int MountedCount;

			public int StockpileCount;

			public int TotalCount;

			public int AvailableAmount;

			public int InstalledCount => MountedCount + StockpileCount;

			public override string ToString()
			{
				return $"{MountedCount}/{StockpileCount}/{TotalCount} {ComponentName}";
			}
		}

		public static readonly float EPSILON = 0.001f;

		public bool ShowDetails;

		/// <summary>
		/// First component in block component stack is also first in this list
		/// </summary>
		public List<ComponentInfo> Components = new List<ComponentInfo>(12);

		public string BlockName;

		private string m_contextHelp;

		private MyDefinitionId m_definitionId;

		public string[] BlockIcons;

		public float m_blockIntegrity;

		public float m_blockIntegrityChecked;

		public float CriticalIntegrity;

		public float OwnershipIntegrity;

		public bool ShowAvailable;

		public int CriticalComponentIndex = -1;

		public int MissingComponentIndex = -1;

		public int PCUCost;

		private long m_blockBuiltBy;

		public MyCubeSize GridSize;

		private WhoWantsInfoDisplayed m_displayers;

		public string ContextHelp => m_contextHelp;

		public MyDefinitionId DefinitionId
		{
			get
			{
				return m_definitionId;
			}
			set
			{
				m_definitionId = value;
				Version++;
			}
		}

		public float BlockIntegrity
		{
			get
			{
				return m_blockIntegrity;
			}
			set
			{
				if (Math.Abs(m_blockIntegrityChecked - value) > EPSILON)
				{
					m_blockIntegrityChecked = value;
					Version++;
				}
				m_blockIntegrity = value;
			}
		}

		public long BlockBuiltBy
		{
			get
			{
				return m_blockBuiltBy;
			}
			set
			{
				if (m_blockBuiltBy != value)
				{
					m_blockBuiltBy = value;
				}
			}
		}

		public bool Visible => m_displayers != WhoWantsInfoDisplayed.None;

		public int Version { get; private set; }

<<<<<<< HEAD
		/// <summary>
		/// Event raised whenever the contextual help text changes.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public event Action<string> ContextHelpChanged;

		public void AddDisplayer(WhoWantsInfoDisplayed displayer)
		{
			m_displayers |= displayer;
		}

		public void RemoveDisplayer(WhoWantsInfoDisplayed displayer)
		{
			m_displayers &= ~displayer;
		}

		public void ChangeDisplayer(WhoWantsInfoDisplayed displayer, bool display)
		{
			if (display)
			{
				AddDisplayer(displayer);
			}
			else
			{
				RemoveDisplayer(displayer);
			}
		}

		public void ClearDisplayers()
		{
			m_displayers = WhoWantsInfoDisplayed.None;
		}

		public void SetContextHelp(MyDefinitionBase definition)
		{
			if (!string.IsNullOrEmpty(definition.DescriptionText))
			{
				if (string.IsNullOrEmpty(definition.DescriptionArgs))
				{
					m_contextHelp = definition.DescriptionText;
				}
				else
				{
					string[] array = definition.DescriptionArgs.Split(new char[1] { ',' });
					object[] array2 = new object[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array2[i] = MyIngameHelpObjective.GetHighlightedControl(MyStringId.GetOrCompute(array[i]));
					}
					m_contextHelp = string.Format(definition.DescriptionText, array2);
				}
			}
			else
			{
				m_contextHelp = MyTexts.GetString(MySpaceTexts.Description_NotAvailable);
			}
			this.ContextHelpChanged?.Invoke(m_contextHelp);
		}
	}
}
