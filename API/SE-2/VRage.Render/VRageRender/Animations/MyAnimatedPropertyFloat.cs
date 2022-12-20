using System;
using System.Globalization;
using System.Xml;
using VRage.Utils;

namespace VRageRender.Animations
{
	public class MyAnimatedPropertyFloat : MyAnimatedProperty<float>
	{
		public MyAnimatedPropertyFloat()
		{
		}

		public MyAnimatedPropertyFloat(string name, string description)
			: this(name, description, interpolateAfterEnd: false)
		{
		}

		public MyAnimatedPropertyFloat(string name, string description, bool interpolateAfterEnd)
			: base(name, description, interpolateAfterEnd)
		{
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedPropertyFloat myAnimatedPropertyFloat = new MyAnimatedPropertyFloat(base.Name, base.Description);
			Duplicate(myAnimatedPropertyFloat);
			return myAnimatedPropertyFloat;
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			writer.WriteValue(((float)value).ToString(CultureInfo.InvariantCulture));
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			base.DeserializeValue(reader, out value);
			value = Convert.ToSingle(value, CultureInfo.InvariantCulture);
		}

		protected override bool EqualsValues(object value1, object value2)
		{
			return MyUtils.IsZero((float)value1 - (float)value2);
		}

		internal override void Interpolate(in float val1, in float val2, float time, out float value)
		{
			value = val1 + (val2 - val1) * time;
		}
	}
}
