using System;
using System.Collections.Generic;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game.Components
{
	[PreloadRequired]
	public class MyEntityComponentsDebugDraw
	{
		public static void DebugDraw()
		{
			if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || !MyDebugDrawSettings.DEBUG_DRAW_ENTITY_COMPONENTS || MySector.MainCamera == null)
			{
				return;
			}
			double num = 1.5;
			double num2 = num * 0.045;
			double num3 = 0.5;
			Vector3D position = MySector.MainCamera.Position;
			Vector3D vector3D = MySector.MainCamera.WorldMatrix.Up;
			Vector3D right = MySector.MainCamera.WorldMatrix.Right;
			Vector3D vector3D2 = MySector.MainCamera.ForwardVector;
			BoundingSphereD boundingSphere = new BoundingSphereD(position, 5.0);
			List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
			Vector3D value = Vector3D.Zero;
			Vector3D zero = Vector3D.Zero;
			MatrixD viewProjectionMatrix = MySector.MainCamera.ViewProjectionMatrix;
			Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
			float num4 = (float)safeGuiRectangle.Height / (float)safeGuiRectangle.Width;
			float num5 = 600f;
			float num6 = num5 * num4;
			Vector3D vector3D3 = position + 1.0 * vector3D2;
			Vector3D vector3D4 = Vector3D.Transform(vector3D3, viewProjectionMatrix);
			Vector3D vector3D5 = Vector3D.Transform(vector3D3 + Vector3D.Right * 0.10000000149011612, viewProjectionMatrix);
			Vector3D vector3D6 = Vector3D.Transform(vector3D3 + Vector3D.Up * 0.10000000149011612, viewProjectionMatrix);
			Vector3D vector3D7 = Vector3D.Transform(vector3D3 + Vector3D.Backward * 0.10000000149011612, viewProjectionMatrix);
			Vector2 vector = new Vector2((float)vector3D4.X * num5, (float)vector3D4.Y * (0f - num6) * num4);
			Vector2 vector2 = new Vector2((float)vector3D5.X * num5, (float)vector3D5.Y * (0f - num6) * num4) - vector;
			Vector2 vector3 = new Vector2((float)vector3D6.X * num5, (float)vector3D6.Y * (0f - num6) * num4) - vector;
			Vector2 vector4 = new Vector2((float)vector3D7.X * num5, (float)vector3D7.Y * (0f - num6) * num4) - vector;
			float num7 = 150f;
			Vector2 screenCoordinateFromNormalizedCoordinate = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(new Vector2(1f, 1f));
			_ = screenCoordinateFromNormalizedCoordinate + new Vector2(0f - num7, 0f);
			_ = screenCoordinateFromNormalizedCoordinate + new Vector2(0f, 0f - num7);
			Vector2 vector5 = screenCoordinateFromNormalizedCoordinate + new Vector2(0f - num7, 0f - num7);
			Vector2 vector6 = (screenCoordinateFromNormalizedCoordinate + vector5) * 0.5f;
			MyRenderProxy.DebugDrawLine2D(vector6, vector6 + vector2, Color.Red, Color.Red);
			MyRenderProxy.DebugDrawLine2D(vector6, vector6 + vector3, Color.Green, Color.Green);
			MyRenderProxy.DebugDrawLine2D(vector6, vector6 + vector4, Color.Blue, Color.Blue);
			MyRenderProxy.DebugDrawText2D(vector6 + vector2, "World X", Color.Red, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			MyRenderProxy.DebugDrawText2D(vector6 + vector3, "World Y", Color.Green, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			MyRenderProxy.DebugDrawText2D(vector6 + vector4, "World Z", Color.Blue, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			MyComponentsDebugInputComponent.DetectedEntities.Clear();
			foreach (MyEntity item in entitiesInSphere)
			{
				if (item.PositionComp == null)
				{
					continue;
				}
				Vector3D position2 = item.PositionComp.GetPosition();
				Vector3D vector3D8 = position2 + vector3D * 0.10000000149011612;
				Vector3D vector3D9 = vector3D8 - right * num3;
				if (Vector3D.Dot(Vector3D.Normalize(position2 - position), vector3D2) < 0.9995)
				{
					Vector3D vector3D10 = item.PositionComp.WorldMatrixRef.Right * 0.30000001192092896;
					Vector3D vector3D11 = item.PositionComp.WorldMatrixRef.Up * 0.30000001192092896;
					Vector3D vector3D12 = item.PositionComp.WorldMatrixRef.Backward * 0.30000001192092896;
					MyRenderProxy.DebugDrawSphere(position2, 0.01f, Color.White, 1f, depthRead: false);
					MyRenderProxy.DebugDrawArrow3D(position2, position2 + vector3D10, Color.Red, Color.Red, depthRead: false, 0.1, "X");
					MyRenderProxy.DebugDrawArrow3D(position2, position2 + vector3D11, Color.Green, Color.Green, depthRead: false, 0.1, "Y");
					MyRenderProxy.DebugDrawArrow3D(position2, position2 + vector3D12, Color.Blue, Color.Blue, depthRead: false, 0.1, "Z");
					continue;
				}
				if (Vector3D.Distance(position2, value) < 0.01)
				{
					zero += right * 0.30000001192092896;
					vector3D = -vector3D;
					vector3D8 = position2 + vector3D * 0.10000000149011612;
					vector3D9 = vector3D8 - right * num3;
				}
				value = position2;
				double val = Vector3D.Distance(vector3D9, position);
				double num8 = Math.Atan(num / Math.Max(val, 0.001));
				float num9 = 0f;
				Dictionary<Type, MyComponentBase>.ValueCollection.Enumerator enumerator2 = item.Components.GetEnumerator();
				MyComponentBase component = null;
				while (enumerator2.MoveNext())
				{
					component = enumerator2.Current;
					num9 += (float)GetComponentLines(component);
				}
				num9 += 1f;
				num9 -= (float)GetComponentLines(component);
				enumerator2.Dispose();
				Vector3D vector3D13 = vector3D9 + (num9 + 0.5f) * vector3D * num2;
				Vector3D worldCoord = vector3D9 + (num9 + 1f) * vector3D * num2 + 0.0099999997764825821 * right;
				MyRenderProxy.DebugDrawLine3D(position2, vector3D8, Color.White, Color.White, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(vector3D9, vector3D8, Color.White, Color.White, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(vector3D9, vector3D13, Color.White, Color.White, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(vector3D13, vector3D13 + right * 1.0, Color.White, Color.White, depthRead: false);
				MyRenderProxy.DebugDrawText3D(worldCoord, item.GetType().ToString() + " - " + item.DisplayName, Color.Orange, (float)num8, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				MyComponentsDebugInputComponent.DetectedEntities.Add(item);
				foreach (MyComponentBase component2 in item.Components)
				{
					worldCoord = vector3D9 + num9 * vector3D * num2;
					DebugDrawComponent(component2, worldCoord, right, vector3D, num2, (float)num8);
					MyEntityComponentBase myEntityComponentBase = component2 as MyEntityComponentBase;
					string text = ((myEntityComponentBase == null) ? "" : myEntityComponentBase.ComponentTypeDebugString);
					MyRenderProxy.DebugDrawText3D(worldCoord - 0.019999999552965164 * right, text, Color.Yellow, (float)num8, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
					num9 -= (float)GetComponentLines(component2);
				}
			}
			entitiesInSphere.Clear();
		}

		private static int GetComponentLines(MyComponentBase component, bool countAll = true)
		{
			int num = 1;
			if (component is IMyComponentAggregate)
			{
				int count = (component as IMyComponentAggregate).ChildList.Reader.Count;
				int num2 = 0;
				{
					foreach (MyComponentBase item in (component as IMyComponentAggregate).ChildList.Reader)
					{
						num2++;
						num = ((!(num2 < count || countAll)) ? (num + 1) : (num + GetComponentLines(item)));
					}
					return num;
				}
			}
			return num;
		}

		private static void DebugDrawComponent(MyComponentBase component, Vector3D origin, Vector3D rightVector, Vector3D upVector, double lineSize, float textSize)
		{
			Vector3D vector3D = rightVector * 0.02500000037252903;
			Vector3D vector3D2 = origin + vector3D * 3.5;
			Vector3D worldCoord = origin + 2.0 * vector3D + rightVector * 0.014999999664723873;
			MyRenderProxy.DebugDrawLine3D(origin, origin + 2.0 * vector3D, Color.White, Color.White, depthRead: false);
			MyRenderProxy.DebugDrawText3D(worldCoord, component.ToString(), Color.White, textSize, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			if (!(component is IMyComponentAggregate) || (component as IMyComponentAggregate).ChildList.Reader.Count == 0)
			{
				return;
			}
			int num = GetComponentLines(component, countAll: false) - 1;
			MyRenderProxy.DebugDrawLine3D(vector3D2 - 0.5 * lineSize * upVector, vector3D2 - (double)num * lineSize * upVector, Color.White, Color.White, depthRead: false);
			vector3D2 -= 1.0 * lineSize * upVector;
			foreach (MyComponentBase item in (component as IMyComponentAggregate).ChildList.Reader)
			{
				int componentLines = GetComponentLines(item);
				DebugDrawComponent(item, vector3D2, rightVector, upVector, lineSize, textSize);
				vector3D2 -= (double)componentLines * lineSize * upVector;
			}
		}
	}
}
