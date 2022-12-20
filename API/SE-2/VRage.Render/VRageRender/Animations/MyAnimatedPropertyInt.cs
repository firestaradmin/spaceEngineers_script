using System;
using System.Globalization;
using System.Xml;

namespace VRageRender.Animations
{
	public class MyAnimatedPropertyInt : MyAnimatedProperty<int>
	{
		public MyAnimatedPropertyInt()
		{
		}

		public MyAnimatedPropertyInt(string name, string description)
			: base(name, description, interpolateAfterEnd: false)
		{
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedPropertyInt myAnimatedPropertyInt = new MyAnimatedPropertyInt(base.Name, base.Description);
			Duplicate(myAnimatedPropertyInt);
			return myAnimatedPropertyInt;
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			writer.WriteValue(((int)value).ToString(CultureInfo.InvariantCulture));
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			base.DeserializeValue(reader, out value);
			value = Convert.ToInt32(value, CultureInfo.InvariantCulture);
		}

		protected override bool EqualsValues(object value1, object value2)
		{
			return (int)value1 == (int)value2;
		}

		internal override void Interpolate(in int val1, in int val2, float time, out int value)
		{
			value = val1 + (int)((float)(val2 - val1) * time);
		}
	}
}
