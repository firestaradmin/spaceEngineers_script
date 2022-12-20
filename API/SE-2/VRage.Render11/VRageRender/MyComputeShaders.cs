using System.Collections.Generic;
using System.Diagnostics;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRage.Utils;

namespace VRageRender
{
	internal static class MyComputeShaders
	{
		internal struct Id
		{
			internal int Index;

			internal static readonly Id NULL = new Id
			{
				Index = -1
			};

			internal ShaderInfoId InfoId => GetInfoId(Index);

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
			public static implicit operator ComputeShader(Id id)
			{
				return GetShader(id);
			}
		}

		private static readonly MyFreelist<MyShaderInfo<ComputeShader>> m_shaders = new MyFreelist<MyShaderInfo<ComputeShader>>(256);

		private static readonly Dictionary<MyShaderKey, Id> m_keyToId = new Dictionary<MyShaderKey, Id>();

		internal static Id Create(string file, ShaderMacro[] macros = null)
		{
			MyStringId fileId = X.TEXT_(file);
			MyShaderKey myShaderKey = default(MyShaderKey);
			myShaderKey.FileId = fileId;
			myShaderKey.Macros = macros;
			MyShaderKey key = myShaderKey;
			Id value;
			lock (m_keyToId)
			{
				if (m_keyToId.TryGetValue(key, out value))
				{
					return value;
				}
			}
			MyShaderCompilationInfo myShaderCompilationInfo = default(MyShaderCompilationInfo);
			myShaderCompilationInfo.File = X.TEXT_(file);
			myShaderCompilationInfo.Profile = MyShaderProfile.cs_5_0;
			myShaderCompilationInfo.Macros = macros;
			MyShaderCompilationInfo info = myShaderCompilationInfo;
			Init(ref info, out var shader);
			lock (m_keyToId)
			{
				if (m_keyToId.TryGetValue(key, out value))
				{
					shader?.Dispose();
					return value;
				}
				Id id = default(Id);
				id.Index = m_shaders.Allocate();
				value = id;
				m_keyToId.Add(key, value);
				ShaderInfoId infoId = MyShaders.CreateInfo(null, ref info);
				m_shaders.Data[value.Index] = new MyShaderInfo<ComputeShader>
				{
					Shader = shader,
					InfoId = infoId,
					File = file
				};
				return value;
			}
		}

		private static ShaderInfoId GetInfoId(int index)
		{
			return m_shaders.Data[index].InfoId;
		}

		private static ComputeShader GetShader(Id id)
		{
			return m_shaders.Data[id.Index].Shader;
		}

		private static void Init(ref MyShaderCompilationInfo info, out ComputeShader shader)
		{
			byte[] shaderBytecode = MyShaderCompiler.Compile(ref info);
			try
			{
				shader = new ComputeShader(MyRender11.DeviceInstance, shaderBytecode)
				{
					DebugName = info.File.ToString()
				};
			}
			catch (SharpDXException)
			{
				Debugger.Break();
				string text = string.Concat("Failed to compile ", info.File, " @ profile ", info.Profile, " with defines ", info.Macros.GetString());
				MyRender11.Log.WriteLine(text);
				if (Debugger.IsAttached)
				{
					Init(ref info, out shader);
					return;
				}
				throw new MyRenderException(text);
			}
		}

		internal static void Recompile()
		{
			lock (m_keyToId)
			{
				OnDeviceEnd();
				foreach (Id value in m_keyToId.Values)
				{
					string file = m_shaders.Data[value.Index].File;
					ShaderInfoId infoId = m_shaders.Data[value.Index].InfoId;
					MyShaderCompilationInfo info = MyShaders.GetCompilationInfo(infoId);
					Init(ref info, out var shader);
					m_shaders.Data[value.Index] = new MyShaderInfo<ComputeShader>
					{
						Shader = shader,
						InfoId = infoId,
						File = file
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
					MyShaderInfo<ComputeShader> myShaderInfo = m_shaders.Data[value.Index];
					if (myShaderInfo.Shader != null)
					{
						myShaderInfo.Shader.Dispose();
						myShaderInfo.Shader = null;
						m_shaders.Data[value.Index] = myShaderInfo;
					}
				}
			}
		}
	}
}
