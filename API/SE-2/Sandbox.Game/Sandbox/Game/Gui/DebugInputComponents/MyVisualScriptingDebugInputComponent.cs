using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Game.Components.Session;
using VRage.Game.ModAPI;
using VRage.Game.VisualScripting;
using VRage.Game.VisualScripting.Missions;
using VRage.Generics;
using VRage.Input;
using VRage.ModAPI;
using VRage.Serialization;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI.DebugInputComponents
{
	public class MyVisualScriptingDebugInputComponent : MyDebugComponent
	{
		public MyVisualScriptingDebugInputComponent()
		{
			AddSwitch(MyKeys.NumPad0, (MyKeys keys) => ToggleDebugDraw(), new MyRef<bool>(() => MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER, null), "Debug Draw");
			AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "Reset missions + run GameStarted", ResetMissionsAndRunGameStarted);
		}

		private bool ResetMissionsAndRunGameStarted()
		{
			MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.Reset();
			return true;
		}

		public override string GetName()
		{
			return "Visual Scripting";
		}

		public override void Update10()
		{
			base.Update10();
			_ = MyAPIGateway.Session;
		}

		public override void Draw()
		{
			base.Draw();
			DrawVariables();
			_ = MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER;
		}

		public static void DrawVariables()
		{
			if (MySession.Static == null)
			{
				return;
			}
			MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
			MySessionComponentScriptSharedStorage component2 = MySession.Static.GetComponent<MySessionComponentScriptSharedStorage>();
			if (component == null)
			{
				return;
			}
			IMyCamera camera = ((IMySession)MySession.Static).Camera;
			Vector2 vector = new Vector2(camera.ViewportSize.X * 0.01f, camera.ViewportSize.Y * 0.2f);
			Vector2 vector2 = new Vector2(0f, camera.ViewportSize.Y * 0.015f);
			new Vector2(camera.ViewportSize.X * 0.05f, 0f);
			float num = 0.65f * Math.Min(camera.ViewportSize.X / 1920f, camera.ViewportSize.Y / 1200f);
			int num2 = 0;
			if (component.LevelScripts != null)
			{
				foreach (IMyLevelScript levelScript in component.LevelScripts)
				{
					FieldInfo[] fields = levelScript.GetType().GetFields();
					MyRenderProxy.DebugDrawText2D(vector + num2 * vector2, $"Script : {levelScript.GetType().Name}", Color.Orange, num);
					num2++;
					num2 = DrawFields(vector, vector2, num, num2, levelScript, fields);
				}
			}
			num2++;
			if (component.SMManager != null)
			{
				foreach (MyVSStateMachine runningMachine in component.SMManager.RunningMachines)
				{
					MyRenderProxy.DebugDrawText2D(vector + num2 * vector2, $"Running SM : {runningMachine.Name}", Color.Orange, num);
					num2++;
					if (runningMachine.ActiveCursors.Count == 0)
					{
						MyRenderProxy.DebugDrawText2D(vector + num2 * vector2, $"No active cursor!", Color.Red, num);
						num2++;
					}
					else
					{
						foreach (MyStateMachineCursor activeCursor in runningMachine.ActiveCursors)
						{
							MyRenderProxy.DebugDrawText2D(vector + num2 * vector2, $"       Active cursor : {activeCursor.Node.Name}", Color.Yellow, num);
							num2++;
						}
					}
					foreach (MyStateMachineNode value in runningMachine.AllNodes.Values)
					{
						MyVSStateMachineNode myVSStateMachineNode = value as MyVSStateMachineNode;
						if (myVSStateMachineNode != null && myVSStateMachineNode.ScriptInstance != null)
						{
							FieldInfo[] fields2 = myVSStateMachineNode.ScriptInstance.GetType().GetFields();
							MyRenderProxy.DebugDrawText2D(vector + num2 * vector2, $"Script : {myVSStateMachineNode.Name}", Color.Orange, num);
							num2++;
							num2 = DrawFields(vector, vector2, num, num2, myVSStateMachineNode.ScriptInstance, fields2);
						}
					}
				}
			}
			num2 = 0;
			vector = new Vector2(camera.ViewportSize.X * 0.2f, camera.ViewportSize.Y * 0.2f);
			MyRenderProxy.DebugDrawText2D(vector + num2 * vector2, $"Stored variables:", Color.Orange, num);
			num2++;
			num2 = DrawDictionary(component2.GetBools(), "Bools:", vector, vector2, num, num2);
			num2 = DrawDictionary(component2.GetInts(), "Ints:", vector, vector2, num, num2);
			num2 = DrawDictionary(component2.GetLongs(), "Longs:", vector, vector2, num, num2);
			num2 = DrawDictionary(component2.GetStrings(), "Strings:", vector, vector2, num, num2);
			num2 = DrawDictionary(component2.GetFloats(), "Floats:", vector, vector2, num, num2);
			num2 = DrawDictionary(component2.GetVector3D(), "Vectors:", vector, vector2, num, num2);
			num2 = DrawDictionary(MyVisualScriptLogicProvider.GetTimers(), "Timers:", vector, vector2, num, num2, delegate((int Time, bool Running) x)
			{
				int num3;
				if (!x.Running)
				{
					(num3, _) = x;
				}
				else
				{
					num3 = MySandboxGame.TotalGamePlayTimeInMilliseconds - x.Time;
				}
				return num3 + ", " + x.Running.ToString();
			});
		}

		private static int DrawFields(Vector2 rowStart, Vector2 rowStep, float fontScale, int i, object instance, FieldInfo[] fields)
		{
			foreach (FieldInfo fieldInfo in fields)
			{
				object value = fieldInfo.GetValue(instance);
				if (value is IList && value.GetType().IsGenericType)
				{
					MyRenderProxy.DebugDrawText2D(rowStart + i * rowStep, $"   {fieldInfo.Name} :     {value.GetType()}", Color.Yellow, fontScale);
					i++;
					foreach (object item in (IList)value)
					{
						MyRenderProxy.DebugDrawText2D(rowStart + i * rowStep, $"       {item.ToString()}", Color.Yellow, fontScale);
						i++;
					}
				}
				else
				{
					MyRenderProxy.DebugDrawText2D(rowStart + i * rowStep, $"   {fieldInfo.Name} :     {value}", Color.Yellow, fontScale);
					i++;
				}
			}
			return i;
		}

		private static int DrawDictionary<T>(SerializableDictionary<string, T> dict, string title, Vector2 start, Vector2 offset, float fontScale, int startIndex, Func<T, string> valueFormatter = null)
		{
			if (dict.Dictionary.Count != 0)
			{
				MyRenderProxy.DebugDrawText2D(start + startIndex * offset, $"{title}", Color.Orange, fontScale);
				startIndex++;
				{
					foreach (KeyValuePair<string, T> item in dict.Dictionary)
					{
						string arg = ((valueFormatter != null) ? valueFormatter(item.Value) : item.Value.ToString());
						MyRenderProxy.DebugDrawText2D(start + startIndex * offset, $"{item.Key.ToString()} :    {arg}", Color.Yellow, fontScale);
						startIndex++;
					}
					return startIndex;
				}
			}
			return startIndex;
		}

		public bool ToggleDebugDraw()
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = false;
			}
			else
			{
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
			}
			return true;
		}
	}
}
