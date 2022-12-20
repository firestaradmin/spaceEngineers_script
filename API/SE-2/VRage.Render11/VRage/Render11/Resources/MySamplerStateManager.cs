using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using VRage.Render11.Resources.Internal;
using VRageRender;

namespace VRage.Render11.Resources
{
	internal class MySamplerStateManager : MyPersistentResourceManager<MySamplerState, SamplerStateDescription>
	{
		internal static ISamplerState Default;

		internal static ISamplerState Point;

		internal static ISamplerState Linear;

		internal static ISamplerState Texture;

		internal static ISamplerState Shadowmap;

		internal static ISamplerState Alphamask;

		internal static ISamplerState AlphamaskArray;

		internal static ISamplerState CloudSampler;

		internal static ISamplerState PointHBAOClamp;

		internal static ISamplerState PointHBAOBorder;

		internal static ISamplerState[] StandardSamplers;

		protected override int GetAllocResourceCount()
		{
			return 32;
		}

		internal MySamplerStateManager()
		{
			SamplerStateDescription desc = new SamplerStateDescription
			{
				AddressU = TextureAddressMode.Clamp,
				AddressV = TextureAddressMode.Clamp,
				AddressW = TextureAddressMode.Clamp,
				Filter = Filter.MinMagMipLinear,
				MaximumLod = float.MaxValue
			};
			Default = CreateResource("Default", ref desc);
			desc.AddressU = TextureAddressMode.Border;
			desc.AddressV = TextureAddressMode.Border;
			desc.AddressW = TextureAddressMode.Border;
			desc.Filter = Filter.MinMagMipLinear;
			desc.MaximumLod = float.MaxValue;
			desc.BorderColor = new RawColor4(0f, 0f, 0f, 0f);
			Alphamask = CreateResource("Alphamask", ref desc);
			desc.AddressU = TextureAddressMode.Clamp;
			desc.AddressV = TextureAddressMode.Clamp;
			desc.AddressW = TextureAddressMode.Clamp;
			desc.Filter = Filter.MinMagMipPoint;
			desc.MaximumLod = float.MaxValue;
			Point = CreateResource("Point", ref desc);
			desc.Filter = Filter.MinMagMipLinear;
			desc.MaximumLod = float.MaxValue;
			Linear = CreateResource("Linear", ref desc);
			desc.AddressU = TextureAddressMode.Clamp;
			desc.AddressV = TextureAddressMode.Clamp;
			desc.AddressW = TextureAddressMode.Clamp;
			desc.Filter = Filter.ComparisonMinMagMipLinear;
			desc.MaximumLod = float.MaxValue;
			desc.ComparisonFunction = Comparison.LessEqual;
			Shadowmap = CreateResource("Shadowmap", ref desc);
			desc.MipLodBias = -1f;
			Texture = CreateResource("Texture", ref desc);
			AlphamaskArray = CreateResource("AlphamaskArray", ref desc);
			desc.AddressU = TextureAddressMode.Clamp;
			desc.AddressV = TextureAddressMode.Clamp;
			desc.AddressW = TextureAddressMode.Clamp;
			desc.Filter = Filter.MinMagMipPoint;
			desc.MaximumLod = float.MaxValue;
			desc.MaximumAnisotropy = 1;
			desc.ComparisonFunction = Comparison.Never;
			desc.BorderColor = new RawColor4(float.MinValue, float.MinValue, float.MinValue, float.MinValue);
			PointHBAOClamp = CreateResource("PointHBAOClamp", ref desc);
			desc.AddressU = TextureAddressMode.Border;
			desc.AddressV = TextureAddressMode.Border;
			PointHBAOBorder = CreateResource("PointHBAOBorder", ref desc);
			desc = new SamplerStateDescription
			{
				AddressU = TextureAddressMode.Wrap,
				AddressV = TextureAddressMode.Wrap,
				AddressW = TextureAddressMode.Wrap,
				Filter = Filter.MinMagMipLinear,
				MaximumLod = float.MaxValue
			};
			CloudSampler = CreateResource("CloudSampler", ref desc);
			StandardSamplers = new ISamplerState[6] { Default, Point, Linear, Texture, Alphamask, AlphamaskArray };
			UpdateFiltering();
		}

		internal static void UpdateFiltering()
		{
			UpdateTextureSampler((MySamplerState)Texture, TextureAddressMode.Wrap);
			UpdateTextureSampler((MySamplerState)AlphamaskArray, TextureAddressMode.Clamp);
		}

		private static void UpdateTextureSampler(MySamplerState samplerState, TextureAddressMode addressMode)
		{
			SamplerStateDescription desc = default(SamplerStateDescription);
			desc.AddressU = addressMode;
			desc.AddressV = addressMode;
			desc.AddressW = addressMode;
			desc.MaximumLod = float.MaxValue;
			if (MyRender11.Settings.User.AnisotropicFiltering == MyTextureAnisoFiltering.NONE)
			{
				desc.Filter = Filter.MinMagMipLinear;
			}
			else
			{
				desc.Filter = Filter.Anisotropic;
				switch (MyRender11.Settings.User.AnisotropicFiltering)
				{
				case MyTextureAnisoFiltering.ANISO_1:
					desc.MaximumAnisotropy = 1;
					break;
				case MyTextureAnisoFiltering.ANISO_4:
					desc.MaximumAnisotropy = 4;
					break;
				case MyTextureAnisoFiltering.ANISO_8:
					desc.MaximumAnisotropy = 8;
					break;
				case MyTextureAnisoFiltering.ANISO_16:
					desc.MaximumAnisotropy = 16;
					break;
				default:
					desc.MaximumAnisotropy = 1;
					break;
				}
			}
			samplerState.ChangeDescription(ref desc);
		}
	}
}
