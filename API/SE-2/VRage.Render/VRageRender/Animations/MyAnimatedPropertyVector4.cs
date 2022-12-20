using System.Xml;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Animations
{
	public class MyAnimatedPropertyVector4 : MyAnimatedProperty<Vector4>
	{
		public MyAnimatedPropertyVector4()
		{
		}

		public MyAnimatedPropertyVector4(string name, string description)
			: base(name, description, interpolateAfterEnd: false)
		{
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedPropertyVector4 myAnimatedPropertyVector = new MyAnimatedPropertyVector4(base.Name, base.Description);
			Duplicate(myAnimatedPropertyVector);
			return myAnimatedPropertyVector;
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			writer.WriteElementString("W", ((Vector4)value).W.ToString());
			writer.WriteElementString("X", ((Vector4)value).X.ToString());
			writer.WriteElementString("Y", ((Vector4)value).Y.ToString());
			writer.WriteElementString("Z", ((Vector4)value).Z.ToString());
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			MyUtils.DeserializeValue(reader, out Vector4 value2);
			value = value2;
		}

		protected override bool EqualsValues(object value1, object value2)
		{
			return MyUtils.IsZero((Vector4)value1 - (Vector4)value2);
		}

		internal override void Interpolate(in Vector4 val1, in Vector4 val2, float time, out Vector4 value)
		{
			value.X = val1.X + (val2.X - val1.X) * time;
			value.Y = val1.Y + (val2.Y - val1.Y) * time;
			value.Z = val1.Z + (val2.Z - val1.Z) * time;
			value.W = val1.W + (val2.W - val1.W) * time;
		}
	}
}
