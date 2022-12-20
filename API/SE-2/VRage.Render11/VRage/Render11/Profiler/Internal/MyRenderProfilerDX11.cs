using System.Text;
using VRage.Render11.Sprites;
using VRageMath;
using VRageRender;
using VRageRender.Profiler;

namespace VRage.Render11.Profiler.Internal
{
	internal class MyRenderProfilerDX11 : MyRenderProfilerRendering
	{
		private MyRenderProfilerLineBatch m_lineBatch;

		protected override Vector2 ViewportSize => MyRender11.ViewportResolution;

		protected override Vector2 MeasureText(StringBuilder text, float scale)
		{
			return MyDebugTextHelpers.MeasureText(text, scale);
		}

		protected override void BeginDraw()
		{
		}

		protected override void DrawText(Vector2 screenCoord, StringBuilder text, Color color, float scale)
		{
			MyDebugTextHelpers.DrawText(screenCoord, text, color, scale);
		}

		protected override void DrawTextShadow(Vector2 screenCoord, StringBuilder text, Color color, float scale)
		{
			MyDebugTextHelpers.DrawTextShadow(screenCoord, text, color, scale);
		}

		protected override void DrawOnScreenLine(Vector3 v0, Vector3 v1, Color color)
		{
			m_lineBatch.DrawOnScreenLine(v0, v1, color);
		}

		protected override void Init()
		{
			m_lineBatch = new MyRenderProfilerLineBatch(Matrix.Identity, Matrix.CreateOrthographicOffCenter(0f, ViewportSize.X, ViewportSize.Y, 0f, 0f, -1f), 50000);
		}

		protected override void BeginLineBatch()
		{
			m_lineBatch.Begin();
		}

		protected override void BeginPrimitiveBatch()
		{
		}

		protected override void DrawOnScreenTriangle(Vector3 v0, Vector3 v1, Vector3 v2, Color color)
		{
			MyPrimitivesRenderer.DrawTriangle(v0, v1, v2, color);
		}

		protected override void DrawOnScreenQuad(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Color color)
		{
			MyPrimitivesRenderer.DrawQuadClockWise(v0, v1, v2, v3, color);
		}

		protected override void EndPrimitiveBatch()
		{
			MyPrimitivesRenderer.Draw(MyRender11.Backbuffer, ref Matrix.Identity, useDepth: false);
		}

		protected override void EndLineBatch()
		{
			m_lineBatch.End();
			MyLinesRenderer.Draw(MyRender11.Backbuffer, nullDepth: true);
		}
	}
}
