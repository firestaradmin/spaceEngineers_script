using System;
using System.Collections.Generic;
using SharpDX.Direct3D11;

namespace VRageRender
{
	internal static class MyInputLayouts
	{
		internal struct Id
		{
			internal int Index;

			internal static readonly Id NULL = new Id
			{
				Index = -1
			};

			public static bool operator ==(Id x, Id y)
			{
				return x.Index == y.Index;
			}

			public static bool operator !=(Id x, Id y)
			{
				return x.Index != y.Index;
			}

<<<<<<< HEAD
			public override bool Equals(object obj)
			{
				object obj2;
				if ((obj2 = obj) is Id)
				{
					Id id = (Id)obj2;
					return Index == id.Index;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return Index.GetHashCode();
			}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public static implicit operator InputLayout(Id id)
			{
				return Get(id);
			}
		}

		private struct MyInputLayoutKey
		{
			public ShaderInfoId InfoId;

			public VertexLayoutId VertexLayoutId;
		}

		private struct InputLayoutInfo
		{
			internal ShaderInfoId InfoId;

			internal VertexLayoutId VertexLayoutId;

			internal InputLayout InputLayout;
		}

		private static readonly MyFreelist<InputLayoutInfo> m_layouts = new MyFreelist<InputLayoutInfo>(256);

		private static readonly Dictionary<MyInputLayoutKey, Id> m_keyToId = new Dictionary<MyInputLayoutKey, Id>();

		internal static Id Create(ShaderInfoId infoId, VertexLayoutId vertexLayoutId)
		{
			MyInputLayoutKey myInputLayoutKey = default(MyInputLayoutKey);
			myInputLayoutKey.InfoId = infoId;
			myInputLayoutKey.VertexLayoutId = vertexLayoutId;
			MyInputLayoutKey key = myInputLayoutKey;
			Id value;
			lock (m_keyToId)
			{
				if (m_keyToId.TryGetValue(key, out value))
				{
					return value;
				}
			}
			InputLayout inputLayout = Init(infoId, vertexLayoutId);
			lock (m_keyToId)
			{
				if (m_keyToId.TryGetValue(key, out value))
				{
					inputLayout.Dispose();
					return value;
				}
				Id id = default(Id);
				id.Index = m_layouts.Allocate();
				value = id;
				m_keyToId.Add(key, value);
				m_layouts.Data[value.Index] = new InputLayoutInfo
				{
					InfoId = infoId,
					VertexLayoutId = vertexLayoutId,
					InputLayout = inputLayout
				};
				return value;
			}
		}

		private static InputLayout Get(Id id)
		{
			return m_layouts.Data[id.Index].InputLayout;
		}

		private static InputLayout Init(ShaderInfoId infoId, VertexLayoutId vertexLayoutId)
		{
			try
			{
				return new InputLayout(MyRender11.DeviceInstance, MyShaders.GetByteCode(infoId), MyVertexLayouts.GetElements(vertexLayoutId));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		internal static void Recompile()
		{
			lock (m_keyToId)
			{
				OnDeviceEnd();
				foreach (Id value in m_keyToId.Values)
				{
					ShaderInfoId infoId = m_layouts.Data[value.Index].InfoId;
					VertexLayoutId vertexLayoutId = m_layouts.Data[value.Index].VertexLayoutId;
					InputLayout inputLayout = Init(infoId, vertexLayoutId);
					m_layouts.Data[value.Index] = new InputLayoutInfo
					{
						InfoId = infoId,
						InputLayout = inputLayout,
						VertexLayoutId = vertexLayoutId
					};
				}
			}
		}

		internal static void OnDeviceEnd()
		{
			lock (m_keyToId)
			{
				foreach (Id value in m_keyToId.Values)
				{
					InputLayoutInfo inputLayoutInfo = m_layouts.Data[value.Index];
					if (inputLayoutInfo.InputLayout != null)
					{
						inputLayoutInfo.InputLayout.Dispose();
						inputLayoutInfo.InputLayout = null;
						m_layouts.Data[value.Index] = inputLayoutInfo;
					}
				}
			}
		}
	}
}
