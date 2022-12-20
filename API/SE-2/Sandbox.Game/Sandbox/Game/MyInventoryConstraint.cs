using System.Collections.Generic;
using System.Linq;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game
{
	public class MyInventoryConstraint
	{
		public string Icon;

		public bool m_useDefaultIcon;

		public readonly string Description;

		private HashSet<MyDefinitionId> m_constrainedIds;

		private HashSet<MyObjectBuilderType> m_constrainedTypes;

		public bool IsWhitelist { get; set; }

		public HashSetReader<MyDefinitionId> ConstrainedIds => m_constrainedIds;

		public HashSetReader<MyObjectBuilderType> ConstrainedTypes => m_constrainedTypes;

		public MyInventoryConstraint(MyStringId description, string icon = null, bool whitelist = true)
		{
			Icon = icon;
			m_useDefaultIcon = icon == null;
			Description = MyTexts.GetString(description);
			m_constrainedIds = new HashSet<MyDefinitionId>();
			m_constrainedTypes = new HashSet<MyObjectBuilderType>();
			IsWhitelist = whitelist;
		}

		public MyInventoryConstraint(string description, string icon = null, bool whitelist = true)
		{
			Icon = icon;
			m_useDefaultIcon = icon == null;
			Description = description;
			m_constrainedIds = new HashSet<MyDefinitionId>();
			m_constrainedTypes = new HashSet<MyObjectBuilderType>();
			IsWhitelist = whitelist;
		}

		public MyInventoryConstraint Add(MyDefinitionId id)
		{
			m_constrainedIds.Add(id);
			UpdateIcon();
			return this;
		}

		public MyInventoryConstraint Remove(MyDefinitionId id)
		{
			m_constrainedIds.Remove(id);
			UpdateIcon();
			return this;
		}

		public MyInventoryConstraint AddObjectBuilderType(MyObjectBuilderType type)
		{
			m_constrainedTypes.Add(type);
			UpdateIcon();
			return this;
		}

		public MyInventoryConstraint RemoveObjectBuilderType(MyObjectBuilderType type)
		{
			m_constrainedTypes.Remove(type);
			UpdateIcon();
			return this;
		}

		public void Clear()
		{
			m_constrainedIds.Clear();
			m_constrainedTypes.Clear();
			UpdateIcon();
		}

		public bool Check(MyDefinitionId checkedId)
		{
			if (IsWhitelist)
			{
				if (m_constrainedTypes.Contains(checkedId.TypeId))
				{
					return true;
				}
				if (m_constrainedIds.Contains(checkedId))
				{
					return true;
				}
				return false;
			}
			if (m_constrainedTypes.Contains(checkedId.TypeId))
			{
				return false;
			}
			if (m_constrainedIds.Contains(checkedId))
			{
				return false;
			}
			return true;
		}

		public void UpdateIcon()
		{
			if (!m_useDefaultIcon)
			{
				return;
			}
			if (m_constrainedIds.get_Count() == 0 && m_constrainedTypes.get_Count() == 1)
			{
<<<<<<< HEAD
				MyObjectBuilderType myObjectBuilderType = m_constrainedTypes.First();
=======
				MyObjectBuilderType myObjectBuilderType = Enumerable.First<MyObjectBuilderType>((IEnumerable<MyObjectBuilderType>)m_constrainedTypes);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (myObjectBuilderType == typeof(MyObjectBuilder_Ore))
				{
					Icon = MyGuiConstants.TEXTURE_ICON_FILTER_ORE;
				}
				else if (myObjectBuilderType == typeof(MyObjectBuilder_Ingot))
				{
					Icon = MyGuiConstants.TEXTURE_ICON_FILTER_INGOT;
				}
				else if (myObjectBuilderType == typeof(MyObjectBuilder_Component))
				{
					Icon = MyGuiConstants.TEXTURE_ICON_FILTER_COMPONENT;
				}
			}
			else if (m_constrainedIds.get_Count() == 1 && m_constrainedTypes.get_Count() == 0)
			{
				if (Enumerable.First<MyDefinitionId>((IEnumerable<MyDefinitionId>)m_constrainedIds) == new MyDefinitionId(typeof(MyObjectBuilder_Ingot), "Uranium"))
				{
					Icon = MyGuiConstants.TEXTURE_ICON_FILTER_URANIUM;
				}
			}
			else
			{
				Icon = null;
			}
		}
	}
}
