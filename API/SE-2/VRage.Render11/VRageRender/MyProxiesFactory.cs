namespace VRageRender
{
	internal static class MyProxiesFactory
	{
		internal static MyRenderableProxyFlags GetRenderableProxyFlags(RenderFlags flags)
		{
			MyRenderableProxyFlags myRenderableProxyFlags = MyRenderableProxyFlags.None;
			if (flags.HasFlags(RenderFlags.SkipIfTooSmall))
			{
				myRenderableProxyFlags |= MyRenderableProxyFlags.SkipIfTooSmall;
			}
			if (flags.HasFlags(RenderFlags.DrawOutsideViewDistance))
			{
				myRenderableProxyFlags |= MyRenderableProxyFlags.DrawOutsideViewDistance;
			}
			if (flags.HasFlags(RenderFlags.CastShadows))
			{
				myRenderableProxyFlags |= MyRenderableProxyFlags.CastShadows;
			}
			if (flags.HasFlags(RenderFlags.CastShadowsOnLow))
			{
				myRenderableProxyFlags |= MyRenderableProxyFlags.CastShadowsOnLow;
			}
			if (flags.HasFlags(RenderFlags.NoBackFaceCulling))
			{
				myRenderableProxyFlags |= MyRenderableProxyFlags.DisableFaceCulling;
			}
			if (flags.HasFlags(RenderFlags.SkipInForward))
			{
				myRenderableProxyFlags |= MyRenderableProxyFlags.SkipInForward;
			}
			if (flags.HasFlags(RenderFlags.SkipInDepth))
			{
				myRenderableProxyFlags |= MyRenderableProxyFlags.SkipInDepth;
			}
			if (flags.HasFlags(RenderFlags.SkipInMainView))
			{
				myRenderableProxyFlags |= MyRenderableProxyFlags.SkipInMainView;
			}
			if (!flags.HasFlags(RenderFlags.Visible))
			{
				myRenderableProxyFlags |= MyRenderableProxyFlags.SkipInMainView | MyRenderableProxyFlags.SkipInForward;
			}
			if (flags.HasFlags(RenderFlags.DrawInAllCascades))
			{
				myRenderableProxyFlags |= MyRenderableProxyFlags.DrawInAllCascades;
			}
			if (flags.HasFlags(RenderFlags.DistanceFade))
			{
				myRenderableProxyFlags |= MyRenderableProxyFlags.DistanceFade;
			}
			if (flags.HasFlags(RenderFlags.MetalnessColorable))
			{
				myRenderableProxyFlags |= MyRenderableProxyFlags.MetalnessColorable;
			}
			return myRenderableProxyFlags;
		}
	}
}
