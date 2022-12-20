using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace VRageRender
{
	internal class MyVertexInputLayout
	{
		private MyVertexInputComponent[] m_components = new MyVertexInputComponent[0];

		private int m_hash;

<<<<<<< HEAD
=======
		private int m_id;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private InputElement[] m_elements;

		private ShaderMacro[] m_macros;

		private static readonly Dictionary<int, MyVertexInputLayout> m_cached;

		internal static readonly Dictionary<MyVertexInputComponentType, MyComponent> MapComponent;

		internal static MyVertexInputLayout Empty => m_cached[0];

		static MyVertexInputLayout()
		{
			m_cached = new Dictionary<int, MyVertexInputLayout>();
			MapComponent = new Dictionary<MyVertexInputComponentType, MyComponent>();
			MyVertexInputLayout myVertexInputLayout = new MyVertexInputLayout();
			myVertexInputLayout.Build();
			m_cached[0] = myVertexInputLayout;
			InitComponentsMap();
		}

		private MyVertexInputLayout Append(MyVertexInputComponent component)
		{
			return Append(component.Type, component.Slot, component.Freq);
		}

		internal MyVertexInputLayout Append(MyVertexInputComponentType type, int slot = 0, MyVertexInputComponentFreq freq = MyVertexInputComponentFreq.PER_VERTEX)
		{
			MyVertexInputComponent myVertexInputComponent = default(MyVertexInputComponent);
			myVertexInputComponent.Type = type;
			myVertexInputComponent.Slot = slot;
			myVertexInputComponent.Freq = freq;
			MyVertexInputComponent item = myVertexInputComponent;
			int num = MyHashHelper.Combine(m_hash, item.GetHashCode());
			if (m_cached.TryGetValue(num, out var value))
			{
				return value;
			}
			value = new MyVertexInputLayout
			{
				m_hash = num,
<<<<<<< HEAD
				m_components = m_components.Concat(item.Yield_()).ToArray()
=======
				m_id = m_cached.Count,
				m_components = Enumerable.ToArray<MyVertexInputComponent>(Enumerable.Concat<MyVertexInputComponent>((IEnumerable<MyVertexInputComponent>)m_components, item.Yield_()))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			};
			value.Build();
			m_cached[num] = value;
			return value;
		}

		private void Build()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			List<InputElement> list = new List<InputElement>();
			Dictionary<string, int> dict = new Dictionary<string, int>();
			MyVertexInputComponent[] components = m_components;
			for (int i = 0; i < components.Length; i++)
			{
				MyVertexInputComponent component = components[i];
				MapComponent[component.Type].AddComponent(component, list, dict, stringBuilder, stringBuilder2);
			}
			m_elements = list.ToArray();
			m_macros = MyComponent.GetComponentMacros(stringBuilder.ToString(), stringBuilder2.ToString(), m_components);
		}

		private static void InitComponentsMap()
		{
			MapComponent[MyVertexInputComponentType.POSITION_PACKED] = new MyPositionPackedComponent();
			MapComponent[MyVertexInputComponentType.POSITION2] = new MyPosition2Component();
			MapComponent[MyVertexInputComponentType.POSITION3] = new MyPosition3Component();
			MapComponent[MyVertexInputComponentType.POSITION4] = new MyPosition4Component();
			MapComponent[MyVertexInputComponentType.POSITION4_H] = new MyPosition4HalfComponent();
			MapComponent[MyVertexInputComponentType.VOXEL_POSITION_MAT] = new MyVoxelPositionMaterialComponent();
			MapComponent[MyVertexInputComponentType.CUBE_INSTANCE] = new MyCubeInstanceComponent();
			MapComponent[MyVertexInputComponentType.GENERIC_INSTANCE] = new MyGenericInstanceComponent();
			MapComponent[MyVertexInputComponentType.SIMPLE_INSTANCE] = new MySimpleInstanceComponent();
			MapComponent[MyVertexInputComponentType.SIMPLE_INSTANCE_COLORING] = new MySimpleInstanceColoringComponent();
			MapComponent[MyVertexInputComponentType.BLEND_INDICES] = new MyBlendIndicesComponent();
			MapComponent[MyVertexInputComponentType.BLEND_WEIGHTS] = new MyBlendWeightsComponent();
			MapComponent[MyVertexInputComponentType.COLOR4] = new MyColor4Component();
			MapComponent[MyVertexInputComponentType.CUSTOM_HALF4_0] = new MyCustomHalf4_0Component();
			MapComponent[MyVertexInputComponentType.CUSTOM_HALF4_1] = new MyCustomHalf4_1Component();
			MapComponent[MyVertexInputComponentType.CUSTOM_HALF4_2] = new MyCustomHalf4_2Component();
			MapComponent[MyVertexInputComponentType.CUSTOM_UNORM4_0] = new MyCustomUnorm4_0Component();
			MapComponent[MyVertexInputComponentType.CUSTOM_UNORM4_1] = new MyCustomUnorm4_1Component();
			MapComponent[MyVertexInputComponentType.CUSTOM_UINT] = new MyCustomUint_Component();
			MapComponent[MyVertexInputComponentType.TEXCOORD0_H] = new MyTexcoord0HalfComponent();
			MapComponent[MyVertexInputComponentType.TEXCOORD0] = new MyTexcoord0Component();
			MapComponent[MyVertexInputComponentType.TEXINDICES] = new MyTexIndicesComponent();
			MapComponent[MyVertexInputComponentType.NORMAL] = new MyNormalComponent();
			MapComponent[MyVertexInputComponentType.VOXEL_NORMAL] = new MyVoxelNormalComponent();
			MapComponent[MyVertexInputComponentType.TANGENT_SIGN_OF_BITANGENT] = new MyTangentBitanSgnComponent();
			MapComponent[MyVertexInputComponentType.CUSTOM4_0] = new MyCustom4_0Component();
			MapComponent[MyVertexInputComponentType.CUSTOM3_0] = new MyCustom3_0Component();
			MapComponent[MyVertexInputComponentType.CUSTOM1_0] = new MyCustom1_0Component();
		}
	}
}
