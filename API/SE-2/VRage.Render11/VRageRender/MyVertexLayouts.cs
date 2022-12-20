<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace VRageRender
{
	internal static class MyVertexLayouts
	{
		private static readonly Dictionary<int, VertexLayoutId> m_hashIndex;

		internal static readonly MyFreelist<MyVertexLayoutInfo> Layouts;

		internal static VertexLayoutId Empty;

		internal static InputElement[] GetElements(VertexLayoutId id)
		{
			return Layouts.Data[id.Index].Elements;
		}

		static MyVertexLayouts()
		{
			m_hashIndex = new Dictionary<int, VertexLayoutId>();
			MyVertexLayoutInfo myVertexLayoutInfo = default(MyVertexLayoutInfo);
			Layouts = new MyFreelist<MyVertexLayoutInfo>(64, myVertexLayoutInfo);
			VertexLayoutId vertexLayoutId = new VertexLayoutId
			{
				Index = Layouts.Allocate()
			};
			m_hashIndex[0] = vertexLayoutId;
			MyVertexLayoutInfo[] data = Layouts.Data;
			int index = vertexLayoutId.Index;
			myVertexLayoutInfo = new MyVertexLayoutInfo
			{
				Elements = new InputElement[0],
				Macros = new ShaderMacro[0]
			};
			data[index] = myVertexLayoutInfo;
			Empty = vertexLayoutId;
		}

		internal static void Init()
		{
		}

		internal static VertexLayoutId GetLayout(params MyVertexInputComponentType[] components)
		{
<<<<<<< HEAD
			return GetLayout(components.Select((MyVertexInputComponentType x) => new MyVertexInputComponent(x)).ToArray());
=======
			return GetLayout(Enumerable.ToArray<MyVertexInputComponent>(Enumerable.Select<MyVertexInputComponentType, MyVertexInputComponent>((IEnumerable<MyVertexInputComponentType>)components, (Func<MyVertexInputComponentType, MyVertexInputComponent>)((MyVertexInputComponentType x) => new MyVertexInputComponent(x)))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		internal static VertexLayoutId GetLayout(VertexLayoutId firstLayout, VertexLayoutId secondLayout)
		{
			_ = VertexLayoutId.NULL;
			List<MyVertexInputComponent> list = new List<MyVertexInputComponent>(firstLayout.Info.Components);
			MyVertexInputComponent[] components = secondLayout.Info.Components;
			list.AddRange(components);
			return GetLayout(list.ToArray());
		}

		internal static VertexLayoutId GetLayout(params MyVertexInputComponent[] components)
		{
			if (components == null || components.Length == 0)
			{
				return Empty;
			}
			int h = 0;
			MyVertexInputComponent[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				MyVertexInputComponent myVertexInputComponent = array[i];
				MyHashHelper.Combine(ref h, myVertexInputComponent.GetHashCode());
			}
			lock (m_hashIndex)
			{
				if (m_hashIndex.ContainsKey(h))
				{
					return m_hashIndex[h];
				}
				VertexLayoutId vertexLayoutId = default(VertexLayoutId);
				vertexLayoutId.Index = Layouts.Allocate();
				VertexLayoutId vertexLayoutId2 = vertexLayoutId;
				m_hashIndex[h] = vertexLayoutId2;
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				Dictionary<string, int> dict = new Dictionary<string, int>();
				List<InputElement> list = new List<InputElement>(components.Length);
				array = components;
				for (int i = 0; i < array.Length; i++)
				{
					MyVertexInputComponent component = array[i];
					MyVertexInputLayout.MapComponent[component.Type].AddComponent(component, list, dict, stringBuilder, stringBuilder2);
				}
				list.Capacity = list.Count;
				Layouts.Data[vertexLayoutId2.Index] = new MyVertexLayoutInfo
				{
					Components = components,
					Elements = list.ToArray(),
					Macros = MyComponent.GetComponentMacros(stringBuilder.ToString(), stringBuilder2.ToString(), components),
<<<<<<< HEAD
					HasBonesInfo = components.Any((MyVertexInputComponent x) => x.Type == MyVertexInputComponentType.BLEND_INDICES)
=======
					HasBonesInfo = Enumerable.Any<MyVertexInputComponent>((IEnumerable<MyVertexInputComponent>)components, (Func<MyVertexInputComponent, bool>)((MyVertexInputComponent x) => x.Type == MyVertexInputComponentType.BLEND_INDICES))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				};
				return vertexLayoutId2;
			}
		}
	}
}
