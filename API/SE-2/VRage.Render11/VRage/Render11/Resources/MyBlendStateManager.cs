using SharpDX.Direct3D11;
using VRage.Render11.Resources.Internal;

namespace VRage.Render11.Resources
{
	internal class MyBlendStateManager : MyPersistentResourceManager<MyBlendState, BlendStateDescription>
	{
		internal static IBlendState BlendGui;

		internal static IBlendState BlendAdditive;

		internal static IBlendState BlendAtmosphere;

		internal static IBlendState BlendTransparent;

		internal static IBlendState BlendAlphaPremult;

		internal static IBlendState BlendAlphaPremultNoAlphaChannel;

		internal static IBlendState BlendReplace;

		internal static IBlendState BlendReplaceNoAlphaChannel;

		internal static IBlendState BlendOutscatter;

		internal static IBlendState BlendFactor;

		internal static IBlendState BlendDecalColor;

		internal static IBlendState BlendDecalNormal;

		internal static IBlendState BlendDecalNormalColor;

		internal static IBlendState BlendDecalNormalColorExt;

		internal static IBlendState BlendDecalColorNoPremult;

		internal static IBlendState BlendDecalNormalNoPremult;

		internal static IBlendState BlendDecalNormalColorNoPremult;

		internal static IBlendState BlendDecalNormalColorExtNoPremult;

		internal static IBlendState BlendWeightedTransparencyResolve;

		internal static IBlendState BlendWeightedTransparency;

		protected override int GetAllocResourceCount()
		{
			return 16;
		}

		public MyBlendStateManager()
		{
			BlendStateDescription desc = default(BlendStateDescription);
			desc.RenderTarget[0].IsBlendEnabled = true;
			desc.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
			desc.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
			desc.RenderTarget[0].DestinationAlphaBlend = BlendOption.InverseSourceAlpha;
			desc.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
			desc.RenderTarget[0].SourceAlphaBlend = BlendOption.SourceAlpha;
			BlendGui = CreateResource("BlendGui", ref desc);
			BlendStateDescription desc2 = default(BlendStateDescription);
			desc2.RenderTarget[0].IsBlendEnabled = true;
			desc2.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
			desc2.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc2.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc2.RenderTarget[0].DestinationBlend = BlendOption.One;
			desc2.RenderTarget[0].DestinationAlphaBlend = BlendOption.One;
			desc2.RenderTarget[0].SourceBlend = BlendOption.One;
			desc2.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
			BlendAdditive = CreateResource("BlendAdditive", ref desc2);
			BlendStateDescription desc3 = default(BlendStateDescription);
			desc3.RenderTarget[0].IsBlendEnabled = true;
			desc3.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
			desc3.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc3.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc3.RenderTarget[0].DestinationBlend = BlendOption.InverseBlendFactor;
			desc3.RenderTarget[0].DestinationAlphaBlend = BlendOption.InverseBlendFactor;
			desc3.RenderTarget[0].SourceBlend = BlendOption.BlendFactor;
			desc3.RenderTarget[0].SourceAlphaBlend = BlendOption.BlendFactor;
			BlendFactor = CreateResource("BlendFactor", ref desc3);
			BlendStateDescription desc4 = default(BlendStateDescription);
			desc4.RenderTarget[0].IsBlendEnabled = true;
			desc4.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
			desc4.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc4.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc4.RenderTarget[0].DestinationBlend = BlendOption.SourceAlpha;
			desc4.RenderTarget[0].DestinationAlphaBlend = BlendOption.One;
			desc4.RenderTarget[0].SourceBlend = BlendOption.One;
			desc4.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
			BlendAtmosphere = CreateResource("BlendAtmosphere", ref desc4);
			BlendStateDescription desc5 = default(BlendStateDescription);
			desc5.RenderTarget[0].IsBlendEnabled = true;
			desc5.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
			desc5.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc5.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc5.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
			desc5.RenderTarget[0].DestinationAlphaBlend = BlendOption.InverseSourceAlpha;
			desc5.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
			desc5.RenderTarget[0].SourceAlphaBlend = BlendOption.SourceAlpha;
			BlendTransparent = CreateResource("BlendTransparent", ref desc5);
			BlendStateDescription desc6 = default(BlendStateDescription);
			desc6.RenderTarget[0].IsBlendEnabled = true;
			desc6.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
			desc6.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc6.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc6.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
			desc6.RenderTarget[0].DestinationAlphaBlend = BlendOption.InverseSourceAlpha;
			desc6.RenderTarget[0].SourceBlend = BlendOption.One;
			desc6.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
			BlendAlphaPremult = CreateResource("BlendAlphaPremult", ref desc6);
			BlendStateDescription desc7 = default(BlendStateDescription);
			desc7.RenderTarget[0].IsBlendEnabled = true;
			desc7.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.Red | ColorWriteMaskFlags.Green | ColorWriteMaskFlags.Blue;
			desc7.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc7.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc7.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
			desc7.RenderTarget[0].DestinationAlphaBlend = BlendOption.One;
			desc7.RenderTarget[0].SourceBlend = BlendOption.One;
			desc7.RenderTarget[0].SourceAlphaBlend = BlendOption.Zero;
			BlendAlphaPremultNoAlphaChannel = CreateResource("BlendAlphaPremultNoAlphaChannel", ref desc7);
			BlendStateDescription desc8 = default(BlendStateDescription);
			desc8.RenderTarget[0].IsBlendEnabled = true;
			desc8.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
			desc8.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc8.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc8.RenderTarget[0].DestinationBlend = BlendOption.Zero;
			desc8.RenderTarget[0].DestinationAlphaBlend = BlendOption.Zero;
			desc8.RenderTarget[0].SourceBlend = BlendOption.One;
			desc8.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
			BlendReplace = CreateResource("BlendReplace", ref desc8);
			BlendStateDescription desc9 = default(BlendStateDescription);
			desc9.RenderTarget[0].IsBlendEnabled = true;
			desc9.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.Red | ColorWriteMaskFlags.Green | ColorWriteMaskFlags.Blue;
			desc9.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc9.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc9.RenderTarget[0].DestinationBlend = BlendOption.Zero;
			desc9.RenderTarget[0].DestinationAlphaBlend = BlendOption.Zero;
			desc9.RenderTarget[0].SourceBlend = BlendOption.One;
			desc9.RenderTarget[0].SourceAlphaBlend = BlendOption.Zero;
			BlendReplaceNoAlphaChannel = CreateResource("BlendReplaceNoAlphaChannel", ref desc9);
			BlendStateDescription desc10 = default(BlendStateDescription);
			desc10.RenderTarget[0].IsBlendEnabled = true;
			desc10.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
			desc10.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc10.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc10.RenderTarget[0].DestinationBlend = BlendOption.SourceColor;
			desc10.RenderTarget[0].DestinationAlphaBlend = BlendOption.InverseSourceAlpha;
			desc10.RenderTarget[0].SourceBlend = BlendOption.Zero;
			desc10.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
			BlendOutscatter = CreateResource("BlendOutscatter", ref desc10);
			BlendStateDescription desc11 = new BlendStateDescription
			{
				IndependentBlendEnable = true
			};
			desc11.RenderTarget[0].IsBlendEnabled = true;
			desc11.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.Red | ColorWriteMaskFlags.Green | ColorWriteMaskFlags.Blue;
			desc11.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc11.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc11.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
			desc11.RenderTarget[0].DestinationAlphaBlend = BlendOption.One;
			desc11.RenderTarget[0].SourceBlend = BlendOption.One;
			desc11.RenderTarget[0].SourceAlphaBlend = BlendOption.Zero;
			desc11.RenderTarget[2].IsBlendEnabled = true;
			desc11.RenderTarget[2].RenderTargetWriteMask = ColorWriteMaskFlags.Red;
			desc11.RenderTarget[2].BlendOperation = BlendOperation.Add;
			desc11.RenderTarget[2].AlphaBlendOperation = BlendOperation.Add;
			desc11.RenderTarget[2].DestinationBlend = BlendOption.InverseSourceAlpha;
			desc11.RenderTarget[2].DestinationAlphaBlend = BlendOption.One;
			desc11.RenderTarget[2].SourceBlend = BlendOption.One;
			desc11.RenderTarget[2].SourceAlphaBlend = BlendOption.Zero;
			BlendDecalColor = CreateResource("BlendDecalColor", ref desc11);
			desc11.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
			desc11.RenderTarget[2].SourceBlend = BlendOption.SourceAlpha;
			BlendDecalColorNoPremult = CreateResource("BlendDecalColorNoPremult", ref desc11);
			BlendStateDescription desc12 = new BlendStateDescription
			{
				IndependentBlendEnable = true
			};
			desc12.RenderTarget[1].IsBlendEnabled = true;
			desc12.RenderTarget[1].RenderTargetWriteMask = ColorWriteMaskFlags.Red | ColorWriteMaskFlags.Green | ColorWriteMaskFlags.Blue;
			desc12.RenderTarget[1].BlendOperation = BlendOperation.Add;
			desc12.RenderTarget[1].AlphaBlendOperation = BlendOperation.Add;
			desc12.RenderTarget[1].DestinationBlend = BlendOption.InverseSourceAlpha;
			desc12.RenderTarget[1].DestinationAlphaBlend = BlendOption.One;
			desc12.RenderTarget[1].SourceBlend = BlendOption.SourceAlpha;
			desc12.RenderTarget[1].SourceAlphaBlend = BlendOption.Zero;
			desc12.RenderTarget[2].IsBlendEnabled = true;
			desc12.RenderTarget[2].RenderTargetWriteMask = ColorWriteMaskFlags.Green;
			desc12.RenderTarget[2].BlendOperation = BlendOperation.Add;
			desc12.RenderTarget[2].AlphaBlendOperation = BlendOperation.Add;
			desc12.RenderTarget[2].DestinationBlend = BlendOption.InverseSourceAlpha;
			desc12.RenderTarget[2].DestinationAlphaBlend = BlendOption.One;
			desc12.RenderTarget[2].SourceBlend = BlendOption.One;
			desc12.RenderTarget[2].SourceAlphaBlend = BlendOption.Zero;
			BlendDecalNormal = CreateResource("BlendDecalNormal", ref desc12);
			desc12.RenderTarget[2].SourceBlend = BlendOption.SourceAlpha;
			BlendDecalNormalNoPremult = CreateResource("BlendDecalNormalNoPremult", ref desc12);
			BlendStateDescription desc13 = new BlendStateDescription
			{
				IndependentBlendEnable = true
			};
			desc13.RenderTarget[0].IsBlendEnabled = true;
			desc13.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.Red | ColorWriteMaskFlags.Green | ColorWriteMaskFlags.Blue;
			desc13.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc13.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc13.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
			desc13.RenderTarget[0].DestinationAlphaBlend = BlendOption.One;
			desc13.RenderTarget[0].SourceBlend = BlendOption.One;
			desc13.RenderTarget[0].SourceAlphaBlend = BlendOption.Zero;
			desc13.RenderTarget[1].IsBlendEnabled = true;
			desc13.RenderTarget[1].RenderTargetWriteMask = ColorWriteMaskFlags.Red | ColorWriteMaskFlags.Green | ColorWriteMaskFlags.Blue;
			desc13.RenderTarget[1].BlendOperation = BlendOperation.Add;
			desc13.RenderTarget[1].AlphaBlendOperation = BlendOperation.Add;
			desc13.RenderTarget[1].DestinationBlend = BlendOption.InverseSourceAlpha;
			desc13.RenderTarget[1].DestinationAlphaBlend = BlendOption.One;
			desc13.RenderTarget[1].SourceBlend = BlendOption.SourceAlpha;
			desc13.RenderTarget[1].SourceAlphaBlend = BlendOption.Zero;
			desc13.RenderTarget[2].IsBlendEnabled = true;
			desc13.RenderTarget[2].BlendOperation = BlendOperation.Add;
			desc13.RenderTarget[2].AlphaBlendOperation = BlendOperation.Add;
			desc13.RenderTarget[2].DestinationBlend = BlendOption.InverseSourceAlpha;
			desc13.RenderTarget[2].DestinationAlphaBlend = BlendOption.One;
			desc13.RenderTarget[2].SourceBlend = BlendOption.One;
			desc13.RenderTarget[2].SourceAlphaBlend = BlendOption.Zero;
			desc13.RenderTarget[2].RenderTargetWriteMask = ColorWriteMaskFlags.Red | ColorWriteMaskFlags.Green;
			BlendDecalNormalColor = CreateResource("BlendDecalNormalColor", ref desc13);
			desc13.RenderTarget[2].RenderTargetWriteMask = ColorWriteMaskFlags.Red | ColorWriteMaskFlags.Green | ColorWriteMaskFlags.Blue;
			BlendDecalNormalColorExt = CreateResource("BlendDecalNormalColorExt", ref desc13);
			desc13.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
			desc13.RenderTarget[2].SourceBlend = BlendOption.SourceAlpha;
			desc13.RenderTarget[2].RenderTargetWriteMask = ColorWriteMaskFlags.Red | ColorWriteMaskFlags.Green;
			BlendDecalNormalColorNoPremult = CreateResource("BlendDecalNormalColorNoPremult", ref desc13);
			desc13.RenderTarget[2].RenderTargetWriteMask = ColorWriteMaskFlags.Red | ColorWriteMaskFlags.Green | ColorWriteMaskFlags.Blue;
			BlendDecalNormalColorExtNoPremult = CreateResource("BlendDecalNormalColorExtNoPremult", ref desc13);
			BlendStateDescription desc14 = default(BlendStateDescription);
			desc14.RenderTarget[0].IsBlendEnabled = true;
			desc14.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
			desc14.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc14.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc14.RenderTarget[0].DestinationBlend = BlendOption.SourceAlpha;
			desc14.RenderTarget[0].DestinationAlphaBlend = BlendOption.SourceAlpha;
			desc14.RenderTarget[0].SourceBlend = BlendOption.One;
			desc14.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
			BlendWeightedTransparencyResolve = CreateResource("BlendWeightedTransparencyResolve", ref desc14);
			BlendStateDescription desc15 = new BlendStateDescription
			{
				IndependentBlendEnable = true
			};
			desc15.RenderTarget[0].IsBlendEnabled = true;
			desc15.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
			desc15.RenderTarget[0].BlendOperation = BlendOperation.Add;
			desc15.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			desc15.RenderTarget[0].DestinationBlend = BlendOption.One;
			desc15.RenderTarget[0].DestinationAlphaBlend = BlendOption.One;
			desc15.RenderTarget[0].SourceBlend = BlendOption.One;
			desc15.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
			desc15.RenderTarget[1].IsBlendEnabled = true;
			desc15.RenderTarget[1].RenderTargetWriteMask = ColorWriteMaskFlags.All;
			desc15.RenderTarget[1].BlendOperation = BlendOperation.Add;
			desc15.RenderTarget[1].AlphaBlendOperation = BlendOperation.Add;
			desc15.RenderTarget[1].DestinationBlend = BlendOption.InverseSourceColor;
			desc15.RenderTarget[1].DestinationAlphaBlend = BlendOption.InverseSourceAlpha;
			desc15.RenderTarget[1].SourceBlend = BlendOption.Zero;
			desc15.RenderTarget[1].SourceAlphaBlend = BlendOption.Zero;
			BlendWeightedTransparency = CreateResource("BlendWeightedTransparency", ref desc15);
		}
	}
}
