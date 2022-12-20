using System.Collections.Generic;
using System.Text;
using System.Threading;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Common;
using VRageRender;
using VRageRender.Import;

namespace VRage.Render11.GeometryStage2.Rendering
{
	internal class MyShaderBundleManager : IManager
	{
		private struct MyShaderBundleKey
		{
			public MyRenderPassType Pass;

			public MyMeshDrawTechnique Technique;

			public bool IsCm;

			public bool IsNg;

			public bool IsExt;

			public MyInstanceLodState State;

			public bool MetalnessColorable;
		}

		private enum MyShaderType
		{
			SHADER_TYPE_VERTEX,
			SHADER_TYPE_PIXEL
		}

		private readonly Dictionary<MyShaderBundleKey, MyShaderBundle> m_cache = new Dictionary<MyShaderBundleKey, MyShaderBundle>();

		private MyVertexInputComponent[] GetVertexInputComponents(MyRenderPassType pass)
		{
			switch (pass)
			{
			case MyRenderPassType.GBuffer:
			case MyRenderPassType.Transparent:
			case MyRenderPassType.TransparentForDecals:
			case MyRenderPassType.Forward:
				return new List<MyVertexInputComponent>
				{
					new MyVertexInputComponent(MyVertexInputComponentType.POSITION_PACKED),
					new MyVertexInputComponent(MyVertexInputComponentType.NORMAL, 1),
					new MyVertexInputComponent(MyVertexInputComponentType.TANGENT_SIGN_OF_BITANGENT, 1),
					new MyVertexInputComponent(MyVertexInputComponentType.TEXCOORD0_H),
					new MyVertexInputComponent(MyVertexInputComponentType.SIMPLE_INSTANCE, 2, MyVertexInputComponentFreq.PER_INSTANCE),
					new MyVertexInputComponent(MyVertexInputComponentType.SIMPLE_INSTANCE_COLORING, 2, MyVertexInputComponentFreq.PER_INSTANCE)
				}.ToArray();
			case MyRenderPassType.Depth:
				return new List<MyVertexInputComponent>
				{
					new MyVertexInputComponent(MyVertexInputComponentType.POSITION_PACKED),
					new MyVertexInputComponent(MyVertexInputComponentType.TEXCOORD0_H),
					new MyVertexInputComponent(MyVertexInputComponentType.SIMPLE_INSTANCE, 2, MyVertexInputComponentFreq.PER_INSTANCE)
				}.ToArray();
			case MyRenderPassType.Highlight:
				return new List<MyVertexInputComponent>
				{
					new MyVertexInputComponent(MyVertexInputComponentType.POSITION_PACKED),
					new MyVertexInputComponent(MyVertexInputComponentType.NORMAL, 1),
					new MyVertexInputComponent(MyVertexInputComponentType.TANGENT_SIGN_OF_BITANGENT, 1),
					new MyVertexInputComponent(MyVertexInputComponentType.TEXCOORD0_H)
				}.ToArray();
			default:
				MyRenderProxy.Error("Unknown pass");
				return null;
			}
		}

		private void AddMacrosForRenderingPass(MyRenderPassType pass, ref List<ShaderMacro> macros)
		{
			switch (pass)
			{
			case MyRenderPassType.GBuffer:
				macros.AddRange(new ShaderMacro[3]
				{
					new ShaderMacro("RENDERING_PASS", 0),
					new ShaderMacro("USE_SIMPLE_INSTANCING", null),
					new ShaderMacro("USE_SIMPLE_INSTANCING_COLORING", null)
				});
				break;
			case MyRenderPassType.Depth:
				macros.AddRange(new ShaderMacro[3]
				{
					new ShaderMacro("RENDERING_PASS", 1),
					new ShaderMacro("DEPTH_ONLY", null),
					new ShaderMacro("USE_SIMPLE_INSTANCING", null)
				});
				break;
			case MyRenderPassType.Highlight:
				macros.AddRange(new ShaderMacro[1]
				{
					new ShaderMacro("RENDERING_PASS", 3)
				});
				break;
			case MyRenderPassType.Forward:
				macros.AddRange(new ShaderMacro[3]
				{
					new ShaderMacro("RENDERING_PASS", 2),
					new ShaderMacro("USE_SIMPLE_INSTANCING", null),
					new ShaderMacro("USE_SIMPLE_INSTANCING_COLORING", null)
				});
				break;
			case MyRenderPassType.Transparent:
				macros.AddRange(new ShaderMacro[3]
				{
					new ShaderMacro("RENDERING_PASS", 5),
					new ShaderMacro("USE_SIMPLE_INSTANCING", null),
					new ShaderMacro("USE_SIMPLE_INSTANCING_COLORING", null)
				});
				break;
			case MyRenderPassType.TransparentForDecals:
				macros.AddRange(new ShaderMacro[3]
				{
					new ShaderMacro("RENDERING_PASS", 6),
					new ShaderMacro("USE_SIMPLE_INSTANCING", null),
					new ShaderMacro("USE_SIMPLE_INSTANCING_COLORING", null)
				});
				break;
			default:
				MyRenderProxy.Error("Unknown render pass type");
				break;
			}
		}

		private void AddMacrosForTechnique(MyMeshDrawTechnique technique, bool isCm, bool isNg, bool isExt, ref List<ShaderMacro> macros)
		{
			switch (technique)
			{
			case MyMeshDrawTechnique.ALPHA_MASKED:
			case MyMeshDrawTechnique.ALPHA_MASKED_SINGLE_SIDED:
				macros.Add(new ShaderMacro("ALPHA_MASKED", null));
				break;
			case MyMeshDrawTechnique.DECAL:
			case MyMeshDrawTechnique.DECAL_NOPREMULT:
			case MyMeshDrawTechnique.DECAL_CUTOUT:
				if (technique == MyMeshDrawTechnique.DECAL_CUTOUT)
				{
					macros.Add(new ShaderMacro("STATIC_DECAL_CUTOUT", null));
				}
				else
				{
					macros.Add(new ShaderMacro("STATIC_DECAL", null));
				}
				if (isCm)
				{
					macros.Add(new ShaderMacro("USE_COLORMETAL_TEXTURE", null));
				}
				if (isNg)
				{
					macros.Add(new ShaderMacro("USE_NORMALGLOSS_TEXTURE", null));
				}
				if (isExt)
				{
					macros.Add(new ShaderMacro("USE_EXTENSIONS_TEXTURE", null));
				}
				break;
			default:
				if (!technique.IsTransparent())
				{
					MyRenderProxy.Error("The specific technique is not processed");
				}
				break;
			case MyMeshDrawTechnique.MESH:
				break;
			}
		}

		private void AddMacrosVertexInputComponents(MyVertexInputComponent[] components, ref List<ShaderMacro> macros)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			Dictionary<string, int> dict = new Dictionary<string, int>();
			List<InputElement> list = new List<InputElement>(components.Length);
			for (int i = 0; i < components.Length; i++)
			{
				MyVertexInputComponent component = components[i];
				MyVertexInputLayout.MapComponent[component.Type].AddComponent(component, list, dict, stringBuilder, stringBuilder2);
			}
			macros.Add(new ShaderMacro("VERTEX_COMPONENTS_DECLARATIONS", stringBuilder));
			macros.Add(new ShaderMacro("TRANSFER_VERTEX_COMPONENTS", stringBuilder2));
		}

		private string GetShaderFilepath(MyMeshDrawTechnique technique, MyShaderType type)
		{
			switch (technique)
			{
			case MyMeshDrawTechnique.MESH:
			case MyMeshDrawTechnique.DECAL:
			case MyMeshDrawTechnique.DECAL_NOPREMULT:
			case MyMeshDrawTechnique.DECAL_CUTOUT:
				if (type == MyShaderType.SHADER_TYPE_VERTEX)
				{
					return "Geometry\\Materials\\Standard\\Vertex.hlsl";
				}
				return "Geometry\\Materials\\Standard\\Pixel.hlsl";
			case MyMeshDrawTechnique.SHIELD_LIT:
				if (type == MyShaderType.SHADER_TYPE_VERTEX)
				{
					return "Geometry\\Materials\\ShieldLit\\Vertex.hlsl";
				}
				return "Geometry\\Materials\\ShieldLit\\Pixel.hlsl";
			case MyMeshDrawTechnique.SHIELD:
				if (type == MyShaderType.SHADER_TYPE_VERTEX)
				{
					return "Geometry\\Materials\\Shield\\Vertex.hlsl";
				}
				return "Geometry\\Materials\\Shield\\Pixel.hlsl";
			case MyMeshDrawTechnique.GLASS:
				if (type == MyShaderType.SHADER_TYPE_VERTEX)
				{
					return "Geometry\\Materials\\Glass\\Vertex.hlsl";
				}
				return "Geometry\\Materials\\Glass\\Pixel.hlsl";
			case MyMeshDrawTechnique.HOLO:
				if (type == MyShaderType.SHADER_TYPE_VERTEX)
				{
					return "Geometry\\Materials\\Holo\\Vertex.hlsl";
				}
				return "Geometry\\Materials\\Holo\\Pixel.hlsl";
			case MyMeshDrawTechnique.ALPHA_MASKED:
			case MyMeshDrawTechnique.ALPHA_MASKED_SINGLE_SIDED:
				if (type == MyShaderType.SHADER_TYPE_VERTEX)
				{
					return "Geometry\\Materials\\AlphaMasked\\Vertex.hlsl";
				}
				return "Geometry\\Materials\\AlphaMasked\\Pixel.hlsl";
			default:
				MyRenderProxy.Error("Unknown technique");
				return "";
			}
		}

		private void AddDebugNameSuffix(ref StringBuilder builder, MyRenderPassType pass, MyMeshDrawTechnique technique, List<ShaderMacro> macros)
		{
			builder.Append(pass);
			builder.Append("_");
			builder.Append(technique);
			builder.Append("_");
			foreach (ShaderMacro macro in macros)
			{
				if (string.IsNullOrEmpty(macro.Definition))
				{
					builder.Append(macro.Name);
				}
				else
				{
					builder.Append(macro.Name);
					builder.Append("=");
					builder.Append(macro.Definition);
				}
				builder.Append(";");
			}
		}

		private string GetVsDebugName(MyRenderPassType pass, MyMeshDrawTechnique technique, List<ShaderMacro> macros)
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("GeoVsNew_");
			AddDebugNameSuffix(ref builder, pass, technique, macros);
			return builder.ToString();
		}

		private string GetPsDebugName(MyRenderPassType pass, MyMeshDrawTechnique technique, List<ShaderMacro> macros)
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("GeoPsNew_");
			AddDebugNameSuffix(ref builder, pass, technique, macros);
			return builder.ToString();
		}

		public MyShaderBundle GetShaderBundle(MyRenderPassType pass, MyMeshDrawTechnique technique, MyInstanceLodState state, bool isCm, bool isNg, bool isExt, bool metalnessColorable)
		{
			if (technique - 6 > MyMeshDrawTechnique.VOXEL_MAP)
			{
				isCm = true;
				isNg = true;
				isExt = true;
			}
			MyShaderBundleKey myShaderBundleKey = default(MyShaderBundleKey);
			myShaderBundleKey.Pass = pass;
			myShaderBundleKey.Technique = technique;
			myShaderBundleKey.IsCm = isCm;
			myShaderBundleKey.IsNg = isNg;
			myShaderBundleKey.IsExt = isExt;
			myShaderBundleKey.State = state;
			myShaderBundleKey.MetalnessColorable = metalnessColorable;
			MyShaderBundleKey key = myShaderBundleKey;
			Monitor.Enter(m_cache);
			if (m_cache.TryGetValue(key, out var value))
			{
				lock (value)
				{
					Monitor.Exit(m_cache);
					return value;
				}
			}
			value = new MyShaderBundle();
			m_cache.Add(key, value);
			lock (value)
			{
				Monitor.Exit(m_cache);
				MyVertexInputComponent[] vertexInputComponents = GetVertexInputComponents(pass);
				VertexLayoutId layout = MyVertexLayouts.GetLayout(vertexInputComponents);
				string shaderFilepath = GetShaderFilepath(technique, MyShaderType.SHADER_TYPE_VERTEX);
				string shaderFilepath2 = GetShaderFilepath(technique, MyShaderType.SHADER_TYPE_PIXEL);
				List<ShaderMacro> macros = new List<ShaderMacro>();
				if (metalnessColorable)
				{
					macros.Add(new ShaderMacro("METALNESS_COLORABLE", null));
				}
				AddMacrosForRenderingPass(pass, ref macros);
				AddMacrosForTechnique(technique, isCm, isNg, isExt, ref macros);
				AddMacrosVertexInputComponents(vertexInputComponents, ref macros);
				MyVertexShaders.Id id = MyVertexShaders.Create(shaderFilepath, macros.ToArray());
				((VertexShader)id).DebugName = GetVsDebugName(pass, technique, macros);
				MyPixelShaders.Id id2 = MyPixelShaders.Create(shaderFilepath2, macros.ToArray());
				((PixelShader)id2).DebugName = GetPsDebugName(pass, technique, macros);
				MyInputLayouts.Id il = MyInputLayouts.Create(id.InfoId, layout);
				value.Init(id2, id, il);
				return value;
			}
		}
	}
}
