using System.Xml;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Animations
{
	public class MyAnimatedPropertyVector3 : MyAnimatedProperty<Vector3>
	{
		public MyAnimatedPropertyVector3()
		{
		}

		public MyAnimatedPropertyVector3(string name, string description)
			: this(name, description, interpolateAfterEnd: false)
		{
		}

		public MyAnimatedPropertyVector3(string name, string description, bool interpolateAfterEnd)
			: base(name, description, interpolateAfterEnd)
		{
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedPropertyVector3 myAnimatedPropertyVector = new MyAnimatedPropertyVector3(base.Name, base.Description);
			Duplicate(myAnimatedPropertyVector);
			return myAnimatedPropertyVector;
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			writer.WriteElementString("X", ((Vector3)value).X.ToString());
			writer.WriteElementString("Y", ((Vector3)value).Y.ToString());
			writer.WriteElementString("Z", ((Vector3)value).Z.ToString());
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			MyUtils.DeserializeValue(reader, out Vector3 value2);
			value = value2;
		}

		protected override bool EqualsValues(object value1, object value2)
		{
			return MyUtils.IsZero((Vector3)value1 - (Vector3)value2);
		}

		internal override void Interpolate(in Vector3 val1, in Vector3 val2, float time, out Vector3 value)
		{
			value.X = val1.X + (val2.X - val1.X) * time;
			value.Y = val1.Y + (val2.Y - val1.Y) * time;
			value.Z = val1.Z + (val2.Z - val1.Z) * time;
		}
	}
}
