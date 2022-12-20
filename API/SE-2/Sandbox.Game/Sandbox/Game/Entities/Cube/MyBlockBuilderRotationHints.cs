using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
	/// <summary>
	/// Calculates and draws rotation hints.
	/// </summary>
	public class MyBlockBuilderRotationHints
	{
		private struct BoxEdge
		{
			public int Axis;

			public LineD Edge;
		}

		private static readonly MyStringId ID_SQUARE_FULL_COLOR = MyStringId.GetOrCompute("SquareFullColor");

		private static readonly MyStringId ID_ARROW_LEFT_GREEN = MyStringId.GetOrCompute("ArrowLeftGreen");

		private static readonly MyStringId ID_ARROW_RIGHT_GREEN = MyStringId.GetOrCompute("ArrowRightGreen");

		private static readonly MyStringId ID_ARROW_GREEN = MyStringId.GetOrCompute("ArrowGreen");

		private static readonly MyStringId ID_ARROW_LEFT_RED = MyStringId.GetOrCompute("ArrowLeftRed");

		private static readonly MyStringId ID_ARROW_RIGHT_RED = MyStringId.GetOrCompute("ArrowRightRed");

		private static readonly MyStringId ID_ARROW_RED = MyStringId.GetOrCompute("ArrowRed");

		private static readonly MyStringId ID_ARROW_LEFT_BLUE = MyStringId.GetOrCompute("ArrowLeftBlue");

		private static readonly MyStringId ID_ARROW_RIGHT_BLUE = MyStringId.GetOrCompute("ArrowRightBlue");

		private static readonly MyStringId ID_ARROW_BLUE = MyStringId.GetOrCompute("ArrowBlue");

		private Vector3D[] m_cubeVertices = new Vector3D[8];

		private List<BoxEdge> m_cubeEdges = new List<BoxEdge>(3);

		private MyBillboardViewProjection m_viewProjection;

		private const MyBillboard.BlendTypeEnum HINT_CUBE_BLENDTYPE = MyBillboard.BlendTypeEnum.LDR;

		public int RotationRightAxis { get; private set; }

		public int RotationRightDirection { get; private set; }

		public int RotationUpAxis { get; private set; }

		public int RotationUpDirection { get; private set; }

		public int RotationForwardAxis { get; private set; }

		public int RotationForwardDirection { get; private set; }

		public MyBlockBuilderRotationHints()
		{
			Clear();
		}

		private static int GetBestAxis(List<BoxEdge> edgeList, Vector3D fitVector, out int direction)
		{
			double num = double.MaxValue;
			int index = -1;
			direction = 0;
			for (int i = 0; i < edgeList.Count; i++)
			{
				double value = Vector3D.Dot(fitVector, edgeList[i].Edge.Direction);
				int num2 = Math.Sign(value);
				value = 1.0 - Math.Abs(value);
				if (value < num)
				{
					num = value;
					index = i;
					direction = num2;
				}
			}
			int axis = edgeList[index].Axis;
			edgeList.RemoveAt(index);
			return axis;
		}

		private static void GetClosestCubeEdge(Vector3D[] vertices, Vector3D cameraPosition, int[] startIndices, int[] endIndices, out int edgeIndex, out int edgeIndex2)
		{
			edgeIndex = -1;
			edgeIndex2 = -1;
			float num = float.MaxValue;
			float num2 = float.MaxValue;
			for (int i = 0; i < 4; i++)
			{
				Vector3D value = (vertices[startIndices[i]] + vertices[endIndices[i]]) * 0.5;
				float num3 = (float)Vector3D.Distance(cameraPosition, value);
				if (num3 < num)
				{
					edgeIndex2 = edgeIndex;
					edgeIndex = i;
					num2 = num;
					num = num3;
				}
				else if (num3 < num2)
				{
					edgeIndex2 = i;
					num2 = num3;
				}
			}
		}

		public void Clear()
		{
			RotationRightAxis = -1;
			RotationRightDirection = -1;
			RotationUpAxis = -1;
			RotationUpDirection = -1;
			RotationForwardAxis = -1;
			RotationForwardDirection = -1;
		}

		public void ReleaseRenderData()
		{
			MyRenderProxy.RemoveBillboardViewProjection(0);
		}

		public void CalculateRotationHints(MatrixD drawMatrix, bool draw, bool fixedAxes = false, bool hideForwardAndUpArrows = false)
		{
			Matrix m = MySector.MainCamera.ViewMatrix;
			MatrixD matrix = MatrixD.Invert(m);
			if (!drawMatrix.IsValid() || !m.IsValid())
			{
				return;
			}
			matrix.Translation = drawMatrix.Translation - 7.0 * matrix.Forward + 1.0 * matrix.Left - 0.60000002384185791 * matrix.Up;
			drawMatrix.Translation -= matrix.Translation;
			m_viewProjection.CameraPosition = matrix.Translation;
			matrix.Translation = Vector3D.Zero;
			MatrixD m2 = MatrixD.Transpose(matrix);
			Matrix viewAtZero = m2;
			m_viewProjection.ViewAtZero = viewAtZero;
			Vector2 screenSizeFromNormalizedSize = MyGuiManager.GetScreenSizeFromNormalizedSize(Vector2.One);
			float num = 2.75f;
			int num2 = (int)(screenSizeFromNormalizedSize.X / num);
			int num3 = (int)(screenSizeFromNormalizedSize.Y / num);
			int num4 = 0;
			int num5 = 0;
			m_viewProjection.Viewport = new MyViewport((int)MySector.MainCamera.Viewport.Width - num2 - num4, num5, num2, num3);
			m_viewProjection.Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, (float)num2 / (float)num3, 0.1f, 10f);
			BoundingBoxD localbox = new BoundingBoxD(-new Vector3(MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large) * 0.5f), new Vector3(MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large)) * 0.5f);
			int num6 = 0;
			MyRenderProxy.AddBillboardViewProjection(num6, m_viewProjection);
			if (draw)
			{
				Color faceX_P = Color.Red;
				Color faceY_P = Color.Green;
				Color faceZ_P = Color.Blue;
				Color faceX_N = Color.White;
				Color faceY_N = Color.White;
				Color faceZ_N = Color.White;
				Color wire = Color.White;
				MySimpleObjectDraw.DrawTransparentBox(ref drawMatrix, ref localbox, ref faceX_P, ref faceY_P, ref faceZ_P, ref faceX_N, ref faceY_N, ref faceZ_N, ref wire, MySimpleObjectRasterizer.Solid, 1, 0.04f, ID_SQUARE_FULL_COLOR, null, onlyFrontFaces: false, num6, MyBillboard.BlendTypeEnum.LDR);
			}
			new MyOrientedBoundingBoxD(Vector3D.Transform(localbox.Center, drawMatrix), localbox.HalfExtents, Quaternion.CreateFromRotationMatrix(in drawMatrix)).GetCorners(m_cubeVertices, 0);
			GetClosestCubeEdge(m_cubeVertices, Vector3D.Zero, MyOrientedBoundingBox.StartXVertices, MyOrientedBoundingBox.EndXVertices, out var edgeIndex, out var edgeIndex2);
			Vector3D vector3D = m_cubeVertices[MyOrientedBoundingBox.StartXVertices[edgeIndex]];
			Vector3D vector3D2 = m_cubeVertices[MyOrientedBoundingBox.EndXVertices[edgeIndex]];
			Vector3D vector3D3 = m_cubeVertices[MyOrientedBoundingBox.StartXVertices[edgeIndex2]];
			Vector3D vector3D4 = m_cubeVertices[MyOrientedBoundingBox.EndXVertices[edgeIndex2]];
			GetClosestCubeEdge(m_cubeVertices, Vector3D.Zero, MyOrientedBoundingBox.StartYVertices, MyOrientedBoundingBox.EndYVertices, out var edgeIndex3, out var edgeIndex4);
			Vector3D vector3D5 = m_cubeVertices[MyOrientedBoundingBox.StartYVertices[edgeIndex3]];
			Vector3D vector3D6 = m_cubeVertices[MyOrientedBoundingBox.EndYVertices[edgeIndex3]];
			Vector3D vector3D7 = m_cubeVertices[MyOrientedBoundingBox.StartYVertices[edgeIndex4]];
			Vector3D vector3D8 = m_cubeVertices[MyOrientedBoundingBox.EndYVertices[edgeIndex4]];
			GetClosestCubeEdge(m_cubeVertices, Vector3D.Zero, MyOrientedBoundingBox.StartZVertices, MyOrientedBoundingBox.EndZVertices, out var edgeIndex5, out var edgeIndex6);
			Vector3D vector3D9 = m_cubeVertices[MyOrientedBoundingBox.StartZVertices[edgeIndex5]];
			Vector3D vector3D10 = m_cubeVertices[MyOrientedBoundingBox.EndZVertices[edgeIndex5]];
			Vector3D vector3D11 = m_cubeVertices[MyOrientedBoundingBox.StartZVertices[edgeIndex6]];
			Vector3D vector3D12 = m_cubeVertices[MyOrientedBoundingBox.EndZVertices[edgeIndex6]];
			m_cubeEdges.Clear();
			List<BoxEdge> cubeEdges = m_cubeEdges;
			BoxEdge item = new BoxEdge
			{
				Axis = 0,
				Edge = new LineD(vector3D, vector3D2)
			};
			cubeEdges.Add(item);
			List<BoxEdge> cubeEdges2 = m_cubeEdges;
			item = new BoxEdge
			{
				Axis = 1,
				Edge = new LineD(vector3D5, vector3D6)
			};
			cubeEdges2.Add(item);
			List<BoxEdge> cubeEdges3 = m_cubeEdges;
			item = new BoxEdge
			{
				Axis = 2,
				Edge = new LineD(vector3D9, vector3D10)
			};
			cubeEdges3.Add(item);
			if (!fixedAxes)
			{
				RotationRightAxis = GetBestAxis(m_cubeEdges, MySector.MainCamera.WorldMatrix.Right, out var direction);
				RotationRightDirection = direction;
				RotationUpAxis = GetBestAxis(m_cubeEdges, MySector.MainCamera.WorldMatrix.Up, out direction);
				RotationUpDirection = direction;
				RotationForwardAxis = GetBestAxis(m_cubeEdges, MySector.MainCamera.WorldMatrix.Forward, out direction);
				RotationForwardDirection = direction;
			}
			string text = MyInput.Static.GetGameControl(MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
			string text2 = MyInput.Static.GetGameControl(MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
			string text3 = MyInput.Static.GetGameControl(MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
			string text4 = MyInput.Static.GetGameControl(MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
			string text5 = MyInput.Static.GetGameControl(MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
			string text6 = MyInput.Static.GetGameControl(MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
			if (MyInput.Static.IsJoystickConnected() && MyInput.Static.IsJoystickLastUsed)
			{
				text = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE).ToString();
				text2 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE).ToString();
				text3 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE).ToString();
				text4 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE).ToString();
				text5 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE).ToString();
				text6 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE).ToString();
			}
			Vector3D vector3D13 = Vector3D.Zero;
			Vector3D vector3D14 = Vector3D.Zero;
			Vector3D vector3D15 = Vector3D.Zero;
			Vector3D vector3D16 = Vector3D.Zero;
			Vector3D vector3D17 = Vector3D.Zero;
			Vector3D vector3D18 = Vector3D.Zero;
			Vector3D vector3D19 = Vector3D.Zero;
			Vector3D vector3D20 = Vector3D.Zero;
			Vector3D vector3D21 = Vector3D.Zero;
			Vector3D vector3D22 = Vector3D.Zero;
			Vector3D vector3D23 = Vector3D.Zero;
			Vector3D vector3D24 = Vector3D.Zero;
			int axis = -1;
			int axis2 = -1;
			int axis3 = -1;
			int num7 = -1;
			int num8 = -1;
			int num9 = -1;
			int num10 = -1;
			int num11 = -1;
			int num12 = -1;
			if (RotationRightAxis == 0)
			{
				vector3D13 = vector3D;
				vector3D14 = vector3D2;
				vector3D19 = vector3D3;
				vector3D20 = vector3D4;
				axis = 0;
				num7 = edgeIndex;
				num10 = edgeIndex2;
			}
			else if (RotationRightAxis == 1)
			{
				vector3D13 = vector3D5;
				vector3D14 = vector3D6;
				vector3D19 = vector3D7;
				vector3D20 = vector3D8;
				axis = 1;
				num7 = edgeIndex3;
				num10 = edgeIndex4;
			}
			else if (RotationRightAxis == 2)
			{
				vector3D13 = vector3D9;
				vector3D14 = vector3D10;
				vector3D19 = vector3D11;
				vector3D20 = vector3D12;
				axis = 2;
				num7 = edgeIndex5;
				num10 = edgeIndex6;
			}
			if (RotationUpAxis == 0)
			{
				vector3D15 = vector3D;
				vector3D16 = vector3D2;
				vector3D21 = vector3D3;
				vector3D22 = vector3D4;
				axis2 = 0;
				num8 = edgeIndex;
				num11 = edgeIndex2;
			}
			else if (RotationUpAxis == 1)
			{
				vector3D15 = vector3D5;
				vector3D16 = vector3D6;
				vector3D21 = vector3D7;
				vector3D22 = vector3D8;
				axis2 = 1;
				num8 = edgeIndex3;
				num11 = edgeIndex4;
			}
			else if (RotationUpAxis == 2)
			{
				vector3D15 = vector3D9;
				vector3D16 = vector3D10;
				vector3D21 = vector3D11;
				vector3D22 = vector3D12;
				axis2 = 2;
				num8 = edgeIndex5;
				num11 = edgeIndex6;
			}
			if (RotationForwardAxis == 0)
			{
				vector3D17 = vector3D;
				vector3D18 = vector3D2;
				vector3D23 = vector3D3;
				vector3D24 = vector3D4;
				axis3 = 0;
				num9 = edgeIndex;
				num12 = edgeIndex2;
			}
			else if (RotationForwardAxis == 1)
			{
				vector3D17 = vector3D5;
				vector3D18 = vector3D6;
				vector3D23 = vector3D7;
				vector3D24 = vector3D8;
				axis3 = 1;
				num9 = edgeIndex3;
				num12 = edgeIndex4;
			}
			else if (RotationForwardAxis == 2)
			{
				vector3D17 = vector3D9;
				vector3D18 = vector3D10;
				vector3D23 = vector3D11;
				vector3D24 = vector3D12;
				axis3 = 2;
				num9 = edgeIndex5;
				num12 = edgeIndex6;
			}
			float scale = 0.544864833f;
			if (!draw)
			{
				return;
			}
			Vector3D vector3D25 = MySector.MainCamera.ForwardVector;
			Vector3D vector3D26 = Vector3.Normalize(vector3D14 - vector3D13);
			Vector3D vector3D27 = Vector3.Normalize(vector3D16 - vector3D15);
			Vector3D vector3D28 = Vector3.Normalize(vector3D18 - vector3D17);
			float num13 = Math.Abs(Vector3.Dot(vector3D25, vector3D26));
			float num14 = Math.Abs(Vector3.Dot(vector3D25, vector3D27));
			float num15 = Math.Abs(Vector3.Dot(vector3D25, vector3D28));
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			float num16 = 0.4f;
			if (num13 < num16)
			{
				if (num14 < num16)
				{
					flag6 = true;
					flag = true;
					flag2 = true;
				}
				else if (num15 < num16)
				{
					flag5 = true;
					flag = true;
					flag3 = true;
				}
				else
				{
					flag2 = true;
					flag3 = true;
				}
			}
			else if (num14 < num16)
			{
				if (num13 < num16)
				{
					flag6 = true;
					flag = true;
					flag2 = true;
				}
				else if (num15 < num16)
				{
					flag4 = true;
					flag2 = true;
					flag3 = true;
				}
				else
				{
					flag = true;
					flag3 = true;
				}
			}
			else if (num15 < num16)
			{
				if (num13 < num16)
				{
					flag5 = true;
					flag = true;
					flag3 = true;
				}
				else if (num14 < num16)
				{
					flag5 = true;
					flag = true;
					flag3 = true;
				}
				else
				{
					flag2 = true;
					flag = true;
				}
			}
			if (!hideForwardAndUpArrows || RotationRightAxis == 1)
			{
				if (flag4)
				{
					Vector3D vector3D29 = (vector3D17 + vector3D18 + vector3D23 + vector3D24) * 0.25;
					MyTransparentGeometry.AddBillboardOriented(ID_ARROW_LEFT_GREEN, Vector4.One, vector3D29 - RotationForwardDirection * vector3D28 * 0.20000000298023224 - RotationRightDirection * vector3D26 * 0.0099999997764825821, -RotationUpDirection * vector3D27, -RotationForwardDirection * vector3D28, 0.2f, num6, MyBillboard.BlendTypeEnum.LDR);
					MyTransparentGeometry.AddBillboardOriented(ID_ARROW_RIGHT_GREEN, Vector4.One, vector3D29 + RotationForwardDirection * vector3D28 * 0.20000000298023224 - RotationRightDirection * vector3D26 * 0.0099999997764825821, RotationUpDirection * vector3D27, RotationForwardDirection * vector3D28, 0.2f, num6, MyBillboard.BlendTypeEnum.LDR);
					if (ShouldDrawText())
					{
						MyRenderProxy.DebugDrawText3D(vector3D29 - RotationForwardDirection * vector3D28 * 0.20000000298023224 - RotationRightDirection * vector3D26 * 0.0099999997764825821, text2, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
						MyRenderProxy.DebugDrawText3D(vector3D29 + RotationForwardDirection * vector3D28 * 0.20000000298023224 - RotationRightDirection * vector3D26 * 0.0099999997764825821, text, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
					}
				}
				else if (flag)
				{
					MyOrientedBoundingBox.GetNormalBetweenEdges(axis, num7, num10, out var normal);
					Vector3D vector3D30 = (vector3D13 + vector3D14) * 0.5;
					Vector3D vector3D31 = Vector3D.TransformNormal(normal, drawMatrix);
					MyOrientedBoundingBox.GetNormalBetweenEdges(axis, num10, num7, out var normal2);
					Vector3D vector3D32 = (vector3D19 + vector3D20) * 0.5;
					Vector3D vector3D33 = Vector3D.TransformNormal(normal2, drawMatrix);
					bool flag7 = false;
					int edge;
					if (num7 == 0 && num10 == 3)
					{
						edge = num7 + 1;
					}
					else if (num7 < num10 || (num7 == 3 && num10 == 0))
					{
						edge = num7 - 1;
						flag7 = true;
					}
					else
					{
						edge = num7 + 1;
					}
					if (RotationRightDirection < 0)
					{
						flag7 = !flag7;
					}
					MyOrientedBoundingBox.GetNormalBetweenEdges(axis, num7, edge, out var normal3);
					Vector3D vector3D34 = Vector3D.TransformNormal(normal3, drawMatrix);
					MyTransparentGeometry.AddBillboardOriented(ID_ARROW_GREEN, Vector4.One, vector3D30 + vector3D31 * 0.40000000596046448 - vector3D34 * 0.0099999997764825821, vector3D26, vector3D33, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
					MyTransparentGeometry.AddBillboardOriented(ID_ARROW_GREEN, Vector4.One, vector3D32 + vector3D33 * 0.40000000596046448 - vector3D34 * 0.0099999997764825821, vector3D26, vector3D31, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
					if (ShouldDrawText())
					{
						MyRenderProxy.DebugDrawText3D(vector3D30 + vector3D31 * 0.30000001192092896 - vector3D34 * 0.0099999997764825821, flag7 ? text : text2, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
						MyRenderProxy.DebugDrawText3D(vector3D32 + vector3D33 * 0.30000001192092896 - vector3D34 * 0.0099999997764825821, flag7 ? text2 : text, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
					}
				}
				else
				{
					MyOrientedBoundingBox.GetNormalBetweenEdges(axis, num7, num7 + 1, out var normal4);
					MyOrientedBoundingBox.GetNormalBetweenEdges(axis, num7, num7 - 1, out var normal5);
					Vector3D vector3D35 = (vector3D13 + vector3D14) * 0.5;
					Vector3D vector3D36 = Vector3D.TransformNormal(normal4, drawMatrix);
					Vector3D vector3D37 = Vector3D.TransformNormal(normal5, drawMatrix);
					MyTransparentGeometry.AddBillboardOriented(ID_ARROW_GREEN, Vector4.One, vector3D35 + vector3D36 * 0.30000001192092896 - vector3D37 * 0.0099999997764825821, vector3D26, vector3D36, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
					MyTransparentGeometry.AddBillboardOriented(ID_ARROW_GREEN, Vector4.One, vector3D35 + vector3D37 * 0.30000001192092896 - vector3D36 * 0.0099999997764825821, vector3D26, vector3D37, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
					if (ShouldDrawText())
					{
						MyRenderProxy.DebugDrawText3D(vector3D35 + vector3D36 * 0.30000001192092896 - vector3D37 * 0.0099999997764825821, (RotationRightDirection < 0) ? text : text2, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
						MyRenderProxy.DebugDrawText3D(vector3D35 + vector3D37 * 0.30000001192092896 - vector3D36 * 0.0099999997764825821, (RotationRightDirection < 0) ? text2 : text, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
					}
				}
			}
			if (!hideForwardAndUpArrows || RotationUpAxis == 1)
			{
				if (flag5)
				{
					Vector3D vector3D38 = (vector3D17 + vector3D18 + vector3D23 + vector3D24) * 0.25;
					MyTransparentGeometry.AddBillboardOriented(ID_ARROW_LEFT_RED, Vector4.One, vector3D38 - RotationRightDirection * vector3D26 * 0.20000000298023224 - RotationUpDirection * vector3D27 * 0.0099999997764825821, -RotationForwardDirection * vector3D28, -RotationRightDirection * vector3D26, 0.2f, num6, MyBillboard.BlendTypeEnum.LDR);
					MyTransparentGeometry.AddBillboardOriented(ID_ARROW_RIGHT_RED, Vector4.One, vector3D38 + RotationRightDirection * vector3D26 * 0.20000000298023224 - RotationUpDirection * vector3D27 * 0.0099999997764825821, RotationForwardDirection * vector3D28, RotationRightDirection * vector3D26, 0.2f, num6, MyBillboard.BlendTypeEnum.LDR);
					if (ShouldDrawText())
					{
						MyRenderProxy.DebugDrawText3D(vector3D38 - RotationRightDirection * vector3D26 * 0.20000000298023224 - RotationUpDirection * vector3D27 * 0.0099999997764825821, text3, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
						MyRenderProxy.DebugDrawText3D(vector3D38 + RotationRightDirection * vector3D26 * 0.20000000298023224 - RotationUpDirection * vector3D27 * 0.0099999997764825821, text4, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
					}
				}
				else if (flag2)
				{
					MyOrientedBoundingBox.GetNormalBetweenEdges(axis2, num8, num11, out var normal6);
					Vector3D vector3D39 = (vector3D15 + vector3D16) * 0.5;
					Vector3 vector = Vector3.TransformNormal(normal6, drawMatrix);
					MyOrientedBoundingBox.GetNormalBetweenEdges(axis2, num11, num8, out var normal7);
					Vector3D vector3D40 = (vector3D21 + vector3D22) * 0.5;
					Vector3 vector2 = Vector3.TransformNormal(normal7, drawMatrix);
					bool flag8 = false;
					int edge2;
					if (num8 == 0 && num11 == 3)
					{
						edge2 = num8 + 1;
					}
					else if (num8 < num11 || (num8 == 3 && num11 == 0))
					{
						edge2 = num8 - 1;
						flag8 = true;
					}
					else
					{
						edge2 = num8 + 1;
					}
					if (RotationUpDirection < 0)
					{
						flag8 = !flag8;
					}
					MyOrientedBoundingBox.GetNormalBetweenEdges(axis2, num8, edge2, out var normal8);
					Vector3 vector3 = Vector3.TransformNormal(normal8, drawMatrix);
					MyTransparentGeometry.AddBillboardOriented(ID_ARROW_RED, Vector4.One, vector3D39 + vector * 0.4f - vector3 * 0.01f, vector3D27, vector2, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
					MyTransparentGeometry.AddBillboardOriented(ID_ARROW_RED, Vector4.One, vector3D40 + vector2 * 0.4f - vector3 * 0.01f, vector3D27, vector, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
					if (ShouldDrawText())
					{
						MyRenderProxy.DebugDrawText3D(vector3D39 + vector * 0.3f - vector3 * 0.01f, flag8 ? text4 : text3, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
						MyRenderProxy.DebugDrawText3D(vector3D40 + vector2 * 0.3f - vector3 * 0.01f, flag8 ? text3 : text4, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
					}
				}
				else
				{
					MyOrientedBoundingBox.GetNormalBetweenEdges(axis2, num8, num8 + 1, out var normal9);
					MyOrientedBoundingBox.GetNormalBetweenEdges(axis2, num8, num8 - 1, out var normal10);
					Vector3D vector3D41 = (vector3D15 + vector3D16) * 0.5;
					Vector3 vector4 = Vector3.TransformNormal(normal9, drawMatrix);
					Vector3 vector5 = Vector3.TransformNormal(normal10, drawMatrix);
					MyTransparentGeometry.AddBillboardOriented(ID_ARROW_RED, Vector4.One, vector3D41 + vector4 * 0.3f - vector5 * 0.01f, vector3D27, vector4, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
					MyTransparentGeometry.AddBillboardOriented(ID_ARROW_RED, Vector4.One, vector3D41 + vector5 * 0.3f - vector4 * 0.01f, vector3D27, vector5, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
					if (ShouldDrawText())
					{
						MyRenderProxy.DebugDrawText3D(vector3D41 + vector4 * 0.6f - vector5 * 0.01f, (RotationUpDirection > 0) ? text3 : text4, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
						MyRenderProxy.DebugDrawText3D(vector3D41 + vector5 * 0.6f - vector4 * 0.01f, (RotationUpDirection > 0) ? text4 : text3, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
					}
				}
			}
			if (hideForwardAndUpArrows && RotationForwardAxis != 1)
			{
				return;
			}
			if (flag6)
			{
				Vector3D vector3D42 = (vector3D13 + vector3D14 + vector3D19 + vector3D20) * 0.25;
				MyTransparentGeometry.AddBillboardOriented(ID_ARROW_LEFT_BLUE, Vector4.One, vector3D42 + RotationUpDirection * vector3D27 * 0.20000000298023224 - RotationForwardDirection * vector3D28 * 0.0099999997764825821, -RotationRightDirection * vector3D26, RotationUpDirection * vector3D27, 0.2f, num6, MyBillboard.BlendTypeEnum.LDR);
				MyTransparentGeometry.AddBillboardOriented(ID_ARROW_RIGHT_BLUE, Vector4.One, vector3D42 - RotationUpDirection * vector3D27 * 0.20000000298023224 - RotationForwardDirection * vector3D28 * 0.0099999997764825821, RotationRightDirection * vector3D26, -RotationUpDirection * vector3D27, 0.2f, num6, MyBillboard.BlendTypeEnum.LDR);
				if (ShouldDrawText())
				{
					MyRenderProxy.DebugDrawText3D(vector3D42 + RotationUpDirection * vector3D27 * 0.20000000298023224 - RotationForwardDirection * vector3D28 * 0.0099999997764825821, text5, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, num6);
					MyRenderProxy.DebugDrawText3D(vector3D42 - RotationUpDirection * vector3D27 * 0.20000000298023224 - RotationForwardDirection * vector3D28 * 0.0099999997764825821, text6, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, num6);
				}
			}
			else if (flag3)
			{
				MyOrientedBoundingBox.GetNormalBetweenEdges(axis3, num9, num12, out var normal11);
				Vector3D vector3D43 = (vector3D17 + vector3D18) * 0.5;
				Vector3 vector6 = Vector3.TransformNormal(normal11, drawMatrix);
				MyOrientedBoundingBox.GetNormalBetweenEdges(axis3, num12, num9, out var normal12);
				Vector3D vector3D44 = (vector3D23 + vector3D24) * 0.5;
				Vector3 vector7 = Vector3.TransformNormal(normal12, drawMatrix);
				bool flag9 = false;
				int edge3;
				if (num9 == 0 && num12 == 3)
				{
					edge3 = num9 + 1;
				}
				else if (num9 < num12 || (num9 == 3 && num12 == 0))
				{
					edge3 = num9 - 1;
					flag9 = true;
				}
				else
				{
					edge3 = num9 + 1;
				}
				if (RotationForwardDirection < 0)
				{
					flag9 = !flag9;
				}
				MyOrientedBoundingBox.GetNormalBetweenEdges(axis3, num9, edge3, out var normal13);
				Vector3 vector8 = Vector3.TransformNormal(normal13, drawMatrix);
				MyTransparentGeometry.AddBillboardOriented(ID_ARROW_BLUE, Vector4.One, vector3D43 + vector6 * 0.4f - vector8 * 0.01f, vector3D28, vector7, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
				MyTransparentGeometry.AddBillboardOriented(ID_ARROW_BLUE, Vector4.One, vector3D44 + vector7 * 0.4f - vector8 * 0.01f, vector3D28, vector6, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
				if (ShouldDrawText())
				{
					MyRenderProxy.DebugDrawText3D(vector3D43 + vector6 * 0.3f - vector8 * 0.01f, flag9 ? text5 : text6, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
					MyRenderProxy.DebugDrawText3D(vector3D44 + vector7 * 0.3f - vector8 * 0.01f, flag9 ? text6 : text5, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
				}
			}
			else
			{
				MyOrientedBoundingBox.GetNormalBetweenEdges(axis3, num9, num9 + 1, out var normal14);
				MyOrientedBoundingBox.GetNormalBetweenEdges(axis3, num9, num9 - 1, out var normal15);
				Vector3D vector3D45 = (vector3D17 + vector3D18) * 0.5;
				Vector3 vector9 = Vector3.TransformNormal(normal14, drawMatrix);
				Vector3 vector10 = Vector3.TransformNormal(normal15, drawMatrix);
				MyTransparentGeometry.AddBillboardOriented(ID_ARROW_BLUE, Vector4.One, vector3D45 + vector9 * 0.3f - vector10 * 0.01f, vector3D28, vector9, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
				MyTransparentGeometry.AddBillboardOriented(ID_ARROW_BLUE, Vector4.One, vector3D45 + vector10 * 0.3f - vector9 * 0.01f, vector3D28, vector10, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
				if (ShouldDrawText())
				{
					MyRenderProxy.DebugDrawText3D(vector3D45 + vector9 * 0.3f - vector10 * 0.01f, (RotationForwardDirection < 0) ? text5 : text6, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
					MyRenderProxy.DebugDrawText3D(vector3D45 + vector10 * 0.3f - vector9 * 0.01f, (RotationForwardDirection < 0) ? text6 : text5, Color.White, scale, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
				}
			}
		}

		private bool ShouldDrawText()
		{
			if (MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay)
			{
				return true;
			}
			return false;
		}
	}
}
