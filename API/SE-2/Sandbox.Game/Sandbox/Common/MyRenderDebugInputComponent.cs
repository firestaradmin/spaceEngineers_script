using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sandbox.Game.Gui;
using VRage.Input;
using VRageMath;
using VRageRender;

namespace Sandbox.Common
{
	/// <summary>
	/// This debug component can be used to remember debug draws methods, aabbs or matrices to be drawn, even if the event occurs just once
	/// and data can not be retrieved to render them in next frames.
	/// </summary>
	public class MyRenderDebugInputComponent : MyDebugComponent
	{
		/// <summary>
		/// This list can be used to track down specific objects during runtime debug step -&gt; CheckedObjects.Add(this), and change then this later to -&gt; if (CheckedObjects.Contain(this)) Debugger.Break();
		/// </summary>
		public static List<object> CheckedObjects = new List<object>();

		/// <summary>
		/// Add your AABB and Color to draw it every update when this component is enabled
		/// </summary>
		public static List<Tuple<BoundingBoxD, Color>> AABBsToDraw = new List<Tuple<BoundingBoxD, Color>>();

		/// <summary>
		/// Add your matrix to be draw every update if this component is enabled
		/// </summary>
		public static List<Tuple<Matrix, Color>> MatricesToDraw = new List<Tuple<Matrix, Color>>();

		public static List<Tuple<CapsuleD, Color>> CapsulesToDraw = new List<Tuple<CapsuleD, Color>>();

		public static List<Tuple<Vector3, Vector3, Color>> LinesToDraw = new List<Tuple<Vector3, Vector3, Color>>();

		/// <summary>
		/// Subscribe to this event debug draw callbacks for specific objects to be draw independetly
		/// </summary>
		public static event Action OnDraw;

		public MyRenderDebugInputComponent()
		{
			AddShortcut(MyKeys.C, newPress: true, control: true, shift: false, alt: false, () => "Clears the drawed objects", () => ClearObjects());
		}

		private bool ClearObjects()
		{
			Clear();
			return true;
		}

		public override void Draw()
		{
			base.Draw();
			if (MyRenderDebugInputComponent.OnDraw != null)
			{
				try
				{
					MyRenderDebugInputComponent.OnDraw();
				}
				catch (Exception)
				{
					MyRenderDebugInputComponent.OnDraw = null;
				}
			}
			foreach (Tuple<BoundingBoxD, Color> item in AABBsToDraw)
			{
				MyRenderProxy.DebugDrawAABB(item.Item1, item.Item2, 1f, 1f, depthRead: false);
			}
			foreach (Tuple<Matrix, Color> item2 in MatricesToDraw)
			{
				Matrix m = item2.Item1;
				MyRenderProxy.DebugDrawAxis(m, 1f, depthRead: false);
				m = item2.Item1;
				MyRenderProxy.DebugDrawOBB(m, item2.Item2, 1f, depthRead: false, smooth: false);
			}
			foreach (Tuple<Vector3, Vector3, Color> item3 in LinesToDraw)
			{
				MyRenderProxy.DebugDrawLine3D(item3.Item1, item3.Item2, item3.Item3, item3.Item3, depthRead: false);
			}
		}

		/// <summary>
		/// Clears the lists.
		/// </summary>
		public static void Clear()
		{
			AABBsToDraw.Clear();
			MatricesToDraw.Clear();
			CapsulesToDraw.Clear();
			LinesToDraw.Clear();
			MyRenderDebugInputComponent.OnDraw = null;
		}

		/// <summary>
		/// Add matrix to be drawn as axes with OBB
		/// </summary>
		/// <param name="mat">World matrix</param>
		/// <param name="col">Color</param>
		public static void AddMatrix(Matrix mat, Color col)
		{
			MatricesToDraw.Add(new Tuple<Matrix, Color>(mat, col));
		}

		/// <summary>
		/// Add AABB box to be drawn
		/// </summary>
		/// <param name="aabb">AABB box in world coords</param>
		/// <param name="col">Color</param>
		public static void AddAABB(BoundingBoxD aabb, Color col)
		{
			AABBsToDraw.Add(new Tuple<BoundingBoxD, Color>(aabb, col));
		}

		public static void AddCapsule(CapsuleD capsule, Color col)
		{
			CapsulesToDraw.Add(new Tuple<CapsuleD, Color>(capsule, col));
		}

		public static void AddLine(Vector3 from, Vector3 to, Color color)
		{
			LinesToDraw.Add(new Tuple<Vector3, Vector3, Color>(from, to, color));
		}

		public override string GetName()
		{
			return "Render";
		}

		/// <summary>
		/// This will break the debugger, if passed object was added to MyRenderDebugInputComponent.CheckedObjects. Use for breaking in the code when you need to break at specific object.
		/// </summary>        
		public static void BreakIfChecked(object objectToCheck)
		{
			if (CheckedObjects.Contains(objectToCheck))
			{
				Debugger.Break();
			}
		}
	}
}
