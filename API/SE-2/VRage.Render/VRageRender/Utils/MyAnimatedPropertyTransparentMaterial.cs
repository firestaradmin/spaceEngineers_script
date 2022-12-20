using System.Xml;
using VRage.Utils;
using VRageRender.Animations;

namespace VRageRender.Utils
{
	public class MyAnimatedPropertyTransparentMaterial : MyAnimatedProperty<MyTransparentMaterial>
	{
		public override string ValueType => "String";

		public override string BaseValueType => "MyTransparentMaterial";

		public MyAnimatedPropertyTransparentMaterial()
		{
		}

		public MyAnimatedPropertyTransparentMaterial(string name, string description)
			: base(name, description, interpolateAfterEnd: false)
		{
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedPropertyTransparentMaterial myAnimatedPropertyTransparentMaterial = new MyAnimatedPropertyTransparentMaterial(base.Name, base.Description);
			Duplicate(myAnimatedPropertyTransparentMaterial);
			return myAnimatedPropertyTransparentMaterial;
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			writer.WriteValue(((MyTransparentMaterial)value).Id.String);
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			base.DeserializeValue(reader, out value);
			value = MyTransparentMaterials.GetMaterial(MyStringId.GetOrCompute((string)value));
		}

		protected override bool EqualsValues(object value1, object value2)
		{
			return ((MyTransparentMaterial)value1).Id.String == ((MyTransparentMaterial)value2).Id.String;
		}

		internal override void Interpolate(in MyTransparentMaterial val1, in MyTransparentMaterial val2, float time, out MyTransparentMaterial value)
		{
			value = ((time < 0.5f) ? val1 : val2);
		}
	}
}
