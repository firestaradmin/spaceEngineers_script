using System;
using System.Xml;

namespace VRageRender.Animations
{
	public class MyConstPropertyBool : MyConstProperty<bool>
	{
		public override string ValueType => "Bool";

		public MyConstPropertyBool()
		{
		}

		public MyConstPropertyBool(string name, string description)
			: base(name, description)
		{
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			writer.WriteValue(value.ToString().ToLower());
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			base.DeserializeValue(reader, out value);
			value = Convert.ToBoolean(value);
		}

		public override IMyConstProperty Duplicate()
		{
			MyConstPropertyBool myConstPropertyBool = new MyConstPropertyBool(base.Name, base.Description);
			Duplicate(myConstPropertyBool);
			return myConstPropertyBool;
		}

		public static implicit operator bool(MyConstPropertyBool f)
		{
			return f?.GetValue() ?? false;
		}
	}
}
