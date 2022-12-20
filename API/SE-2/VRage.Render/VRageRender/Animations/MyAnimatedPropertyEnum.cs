using System;
using System.Collections.Generic;

namespace VRageRender.Animations
{
	public class MyAnimatedPropertyEnum : MyAnimatedPropertyInt
	{
		private Type m_enumType;

		private List<string> m_enumStrings;

		public MyAnimatedPropertyEnum()
		{
		}

		public MyAnimatedPropertyEnum(string name, string description)
			: this(name, description, null, null)
		{
		}

		public MyAnimatedPropertyEnum(string name, string description, Type enumType, List<string> enumStrings)
			: base(name, description)
		{
			m_enumType = enumType;
			m_enumStrings = enumStrings;
		}

		public Type GetEnumType()
		{
			return m_enumType;
		}

		public List<string> GetEnumStrings()
		{
			return m_enumStrings;
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedPropertyEnum myAnimatedPropertyEnum = new MyAnimatedPropertyEnum(base.Name, base.Description);
			Duplicate(myAnimatedPropertyEnum);
			myAnimatedPropertyEnum.m_enumType = m_enumType;
			myAnimatedPropertyEnum.m_enumStrings = m_enumStrings;
			return myAnimatedPropertyEnum;
		}
	}
}
