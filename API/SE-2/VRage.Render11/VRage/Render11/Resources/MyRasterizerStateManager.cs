using SharpDX.Direct3D11;
using VRage.Render11.Resources.Internal;

namespace VRage.Render11.Resources
{
	internal class MyRasterizerStateManager : MyPersistentResourceManager<MyRasterizerState, RasterizerStateDescription>
	{
		internal static IRasterizerState NocullRasterizerState;

		internal static IRasterizerState InvTriRasterizerState;

		internal static IRasterizerState WireframeRasterizerState;

		internal static IRasterizerState LinesRasterizerState;

		internal static IRasterizerState CascadesRasterizerState;

		internal static IRasterizerState ShadowRasterizerState;

		internal static IRasterizerState NocullWireframeRasterizerState;

		internal static IRasterizerState ScissorTestRasterizerState;

		protected override int GetAllocResourceCount()
		{
			return 16;
		}

		public MyRasterizerStateManager()
		{
			Init();
		}

		public void Init()
		{
			RasterizerStateDescription desc = default(RasterizerStateDescription);
			desc.FillMode = FillMode.Wireframe;
			desc.CullMode = CullMode.Back;
			WireframeRasterizerState = CreateResource("WireframeRasterizerState", ref desc);
			desc.FillMode = FillMode.Solid;
			desc.CullMode = CullMode.Front;
			InvTriRasterizerState = CreateResource("InvTriRasterizerState", ref desc);
			desc.FillMode = FillMode.Solid;
			desc.CullMode = CullMode.None;
			NocullRasterizerState = CreateResource("NocullRasterizerState", ref desc);
			desc.FillMode = FillMode.Wireframe;
			desc.CullMode = CullMode.None;
			NocullWireframeRasterizerState = CreateResource("NocullWireframeRasterizerState", ref desc);
			desc = default(RasterizerStateDescription);
			desc.FillMode = FillMode.Solid;
			desc.CullMode = CullMode.Back;
			LinesRasterizerState = CreateResource("LinesRasterizerState", ref desc);
			desc = default(RasterizerStateDescription);
			desc.FillMode = FillMode.Solid;
			desc.CullMode = CullMode.Back;
			desc.IsFrontCounterClockwise = true;
			desc.DepthBias = 20;
			desc.DepthBiasClamp = 2f;
			desc.SlopeScaledDepthBias = 4f;
			CascadesRasterizerState = CreateResource("CascadesRasterizerState", ref desc);
			desc = default(RasterizerStateDescription);
			desc.FillMode = FillMode.Solid;
			desc.CullMode = CullMode.None;
			desc.IsFrontCounterClockwise = true;
			desc.DepthBias = 2500;
			desc.DepthBiasClamp = 10000f;
			desc.SlopeScaledDepthBias = 4f;
			ShadowRasterizerState = CreateResource("ShadowRasterizerState", ref desc);
			desc.FillMode = FillMode.Solid;
			desc.CullMode = CullMode.Back;
			desc.IsFrontCounterClockwise = false;
			desc.IsScissorEnabled = true;
			ScissorTestRasterizerState = CreateResource("ScissorTestRasterizerState", ref desc);
		}
	}
}
