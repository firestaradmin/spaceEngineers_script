using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game
{
	public class MyTransparentGeometry
	{
		private static MyCamera m_camera;

		private const int MAX_TRANSPARENT_GEOMETRY_COUNT = 4000;

		private const int MAX_NEW_PARTICLES_COUNT = 2800;

		public static bool HasCamera => m_camera != null;

		public static MatrixD Camera => m_camera.WorldMatrix;

		public static MatrixD CameraView => m_camera.ViewMatrix;

		private static bool IsEnabled => MyRenderProxy.DebugOverrides.BillboardsStatic;

		public static void SetCamera(MyCamera camera)
		{
			m_camera = camera;
		}

		public static void AddLineBillboard(MyStringId material, Vector4 color, Vector3D origin, Vector3 directionNormalized, float length, float thickness, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, int customViewProjection = -1, float intensity = 1f, List<MyBillboard> persistentBillboards = null)
		{
			AddLineBillboard(material, color, origin, uint.MaxValue, ref MatrixD.Identity, directionNormalized, length, thickness, blendType, customViewProjection, intensity, persistentBillboards);
		}

		public static void AddLineBillboard(MyStringId material, Vector4 color, Vector3D origin, uint renderObjectID, ref MatrixD worldToLocal, Vector3 directionNormalized, float length, float thickness, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, int customViewProjection = -1, float intensity = 1f, List<MyBillboard> persistentBillboards = null)
		{
			if (!IsEnabled)
			{
				return;
			}
			MyBillboard item;
			if (persistentBillboards == null)
			{
				MyRenderProxy.BillboardsPoolWrite.AllocateOrCreate(out item);
			}
			else
			{
				item = MyRenderProxy.AddPersistentBillboard();
				persistentBillboards.Add(item);
			}
			item.BlendType = blendType;
			item.UVOffset = Vector2.Zero;
			item.UVSize = Vector2.One;
			item.LocalType = MyBillboard.LocalTypeEnum.Custom;
			MyPolyLineD polyLine = default(MyPolyLineD);
			polyLine.LineDirectionNormalized = directionNormalized;
			polyLine.Point0 = origin;
			polyLine.Point1 = origin + directionNormalized * length;
			polyLine.Thickness = thickness;
			Vector3D vector3D = ((customViewProjection == -1) ? Camera.Translation : MyRenderProxy.BillboardsViewProjectionWrite[customViewProjection].CameraPosition);
			if (!Vector3D.IsZero(vector3D - polyLine.Point0, 1E-06))
			{
				MyUtils.GetPolyLineQuad(out var retQuad, ref polyLine, vector3D);
				CreateBillboard(item, ref retQuad, material, ref color, ref origin, customViewProjection);
				if (renderObjectID != uint.MaxValue)
				{
					Vector3D.Transform(ref item.Position0, ref worldToLocal, out item.Position0);
					Vector3D.Transform(ref item.Position1, ref worldToLocal, out item.Position1);
					Vector3D.Transform(ref item.Position2, ref worldToLocal, out item.Position2);
					Vector3D.Transform(ref item.Position3, ref worldToLocal, out item.Position3);
					item.ParentID = renderObjectID;
				}
				item.ColorIntensity = intensity;
				MyRenderProxy.AddBillboard(item);
			}
		}

		public static void AddLocalLineBillboard(MyStringId material, Vector4 color, Vector3D origin, uint renderObjectID, Vector3 directionNormalized, float length, float thickness, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, int customViewProjection = -1, float intensity = 1f, List<MyBillboard> persistentBillboards = null)
		{
			if (IsEnabled)
			{
				MyBillboard item;
				if (persistentBillboards == null)
				{
					MyRenderProxy.BillboardsPoolWrite.AllocateOrCreate(out item);
				}
				else
				{
					item = MyRenderProxy.AddPersistentBillboard();
					persistentBillboards.Add(item);
				}
				item.BlendType = blendType;
				item.UVOffset = Vector2.Zero;
				item.UVSize = Vector2.One;
				MyQuadD quad = default(MyQuadD);
				CreateBillboard(item, ref quad, material, ref color, ref origin, customViewProjection);
				item.Position0 = origin;
				item.Position1 = directionNormalized;
				item.Position2 = new Vector3D(length, thickness, 0.0);
				item.ParentID = renderObjectID;
				item.LocalType = MyBillboard.LocalTypeEnum.Line;
				item.ColorIntensity = intensity;
				MyRenderProxy.AddBillboard(item);
			}
		}

		public static void AddLocalPointBillboard(MyStringId material, Vector4 color, Vector3D origin, uint renderObjectID, float radius, float angle, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, int customViewProjection = -1, float intensity = 1f, List<MyBillboard> persistentBillboards = null)
		{
			if (IsEnabled)
			{
				MyBillboard item;
				if (persistentBillboards == null)
				{
					MyRenderProxy.BillboardsPoolWrite.AllocateOrCreate(out item);
				}
				else
				{
					item = MyRenderProxy.AddPersistentBillboard();
					persistentBillboards.Add(item);
				}
				item.BlendType = blendType;
				item.UVOffset = Vector2.Zero;
				item.UVSize = Vector2.One;
				MyQuadD quad = default(MyQuadD);
				CreateBillboard(item, ref quad, material, ref color, ref origin, customViewProjection);
				item.ColorIntensity = intensity;
				item.Position0 = origin;
				item.Position2 = new Vector3D(radius, angle, 0.0);
				item.ParentID = renderObjectID;
				item.LocalType = MyBillboard.LocalTypeEnum.Point;
				MyRenderProxy.AddBillboard(item);
			}
		}

		public static void AddPointBillboard(MyStringId material, Vector4 color, Vector3D origin, float radius, float angle, int customViewProjection = -1, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, List<MyBillboard> persistentBillboards = null)
		{
			AddPointBillboard(material, color, origin, uint.MaxValue, ref MatrixD.Identity, radius, angle, customViewProjection, blendType, 1f, persistentBillboards);
		}

		public static void AddPointBillboard(MyStringId material, Vector4 color, Vector3D origin, uint renderObjectID, ref MatrixD worldToLocal, float radius, float angle, int customViewProjection = -1, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float intensity = 1f, List<MyBillboard> persistentBillboards = null)
		{
			if (!IsEnabled)
			{
				return;
			}
			Vector3D vector3D = Camera.Translation - origin;
			if (MyUtils.GetBillboardQuadAdvancedRotated(out var quad, origin, radius, radius, angle, origin + vector3D))
			{
				MyBillboard item;
				if (persistentBillboards == null)
				{
					MyRenderProxy.BillboardsPoolWrite.AllocateOrCreate(out item);
				}
				else
				{
					item = MyRenderProxy.AddPersistentBillboard();
					persistentBillboards.Add(item);
				}
				CreateBillboard(item, ref quad, material, ref color, ref origin, customViewProjection);
				item.BlendType = blendType;
				if (renderObjectID != uint.MaxValue)
				{
					Vector3D.Transform(ref item.Position0, ref worldToLocal, out item.Position0);
					Vector3D.Transform(ref item.Position1, ref worldToLocal, out item.Position1);
					Vector3D.Transform(ref item.Position2, ref worldToLocal, out item.Position2);
					Vector3D.Transform(ref item.Position3, ref worldToLocal, out item.Position3);
					item.ParentID = renderObjectID;
				}
				item.ColorIntensity = intensity;
				MyRenderProxy.AddBillboard(item);
			}
		}

		public static void AddBillboardOrientedCull(Vector3 cameraPos, MyStringId material, Vector4 color, Vector3 origin, Vector3 leftVector, Vector3 upVector, float radius, int customViewProjection = -1, float reflection = 0f, List<MyBillboard> persistentBillboards = null)
		{
			if (Vector3.Dot(Vector3.Cross(leftVector, upVector), origin - cameraPos) > 0f)
			{
				AddBillboardOriented(material, color, origin, leftVector, upVector, radius, MyBillboard.BlendTypeEnum.Standard, customViewProjection, reflection, persistentBillboards);
			}
		}

		[Obsolete("Only for modders")]
		public static void AddTriangleBillboard(Vector3D p0, Vector3D p1, Vector3D p2, Vector3 n0, Vector3 n1, Vector3 n2, Vector2 uv0, Vector2 uv1, Vector2 uv2, MyStringId material, uint parentID, Vector3D worldPosition, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard)
		{
			MyRenderProxy.TriangleBillboardsPoolWrite.AllocateOrCreate(out var item);
			MyTransparentMaterial material2 = MyTransparentMaterials.GetMaterial(material);
			item.BlendType = blendType;
			item.Position0 = p0;
			item.Position1 = p1;
			item.Position2 = p2;
			item.Position3 = p0;
			item.UV0 = uv0;
			item.UV1 = uv1;
			item.UV2 = uv2;
			item.Normal0 = n0;
			item.Normal1 = n1;
			item.Normal2 = n2;
			item.DistanceSquared = (float)Vector3D.DistanceSquared(Camera.Translation, worldPosition);
			item.Material = material;
			item.Color = material2.Color;
			item.ColorIntensity = 1f;
			item.CustomViewProjection = -1;
			item.Reflectivity = material2.Reflectivity;
			item.LocalType = MyBillboard.LocalTypeEnum.Custom;
			item.ParentID = parentID;
			MyRenderProxy.AddBillboard(item);
		}

		[Obsolete("Only for modders")]
		public static void AddTriangleBillboard(Vector3D p0, Vector3D p1, Vector3D p2, Vector3 n0, Vector3 n1, Vector3 n2, Vector2 uv0, Vector2 uv1, Vector2 uv2, MyStringId material, uint parentID, Vector3D worldPosition, Vector4 color, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard)
		{
			MyRenderProxy.TriangleBillboardsPoolWrite.AllocateOrCreate(out var item);
			MyTransparentMaterial material2 = MyTransparentMaterials.GetMaterial(material);
			item.BlendType = blendType;
			item.Position0 = p0;
			item.Position1 = p1;
			item.Position2 = p2;
			item.Position3 = p0;
			item.UV0 = uv0;
			item.UV1 = uv1;
			item.UV2 = uv2;
			item.Normal0 = n0;
			item.Normal1 = n1;
			item.Normal2 = n2;
			item.DistanceSquared = (float)Vector3D.DistanceSquared(Camera.Translation, worldPosition);
			item.Material = material;
			item.Color = color;
			item.ColorIntensity = 1f;
			item.CustomViewProjection = -1;
			item.Reflectivity = material2.Reflectivity;
			item.ParentID = parentID;
			MyRenderProxy.AddBillboard(item);
		}

		public static void AddBillboardOriented(MyStringId material, Vector4 color, Vector3D origin, Vector3 leftVector, Vector3 upVector, float radius, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, int customViewProjection = -1, float reflection = 0f, List<MyBillboard> persistentBillboards = null)
		{
			if (IsEnabled)
			{
				AddBillboardOriented(material, color, origin, leftVector, upVector, radius, radius, Vector2.Zero, blendType, customViewProjection, reflection, persistentBillboards);
			}
		}

		public static void CreateBillboard(MyBillboard billboard, ref MyQuadD quad, MyStringId material, ref Vector4 color, ref Vector3D origin, int customViewProjection = -1, float reflection = 0f)
		{
			CreateBillboard(billboard, ref quad, material, ref color, ref origin, Vector2.Zero, customViewProjection, reflection);
		}

		public static void CreateBillboard(MyBillboard billboard, ref MyQuadD quad, MyStringId material, ref Vector4 color, ref Vector3D origin, Vector2 uvOffset, int customViewProjection = -1, float reflectivity = 0f)
		{
			MyTransparentMaterial material2 = MyTransparentMaterials.GetMaterial(material);
			if (material2.Id == MyTransparentMaterials.ErrorMaterial.Id)
			{
				color = Vector4.One;
			}
			billboard.Material = material;
			billboard.LocalType = MyBillboard.LocalTypeEnum.Custom;
			billboard.Position0 = quad.Point0;
			billboard.Position1 = quad.Point1;
			billboard.Position2 = quad.Point2;
			billboard.Position3 = quad.Point3;
			billboard.UVOffset = uvOffset;
			billboard.UVSize = Vector2.One;
			Vector3D value = ((customViewProjection == -1) ? Camera.Translation : MyRenderProxy.BillboardsViewProjectionWrite[customViewProjection].CameraPosition);
			billboard.DistanceSquared = (float)Vector3D.DistanceSquared(value, origin);
			billboard.Color = color.ToLinearRGB();
			billboard.ColorIntensity = 1f;
			billboard.Reflectivity = reflectivity;
			billboard.CustomViewProjection = customViewProjection;
			billboard.ParentID = uint.MaxValue;
			billboard.SoftParticleDistanceScale = 1f;
			if (material2.AlphaMistingEnable)
			{
				billboard.Color *= MathHelper.Clamp(((float)Math.Sqrt(billboard.DistanceSquared) - material2.AlphaMistingStart) / (material2.AlphaMistingEnd - material2.AlphaMistingStart), 0f, 1f);
			}
			billboard.Color *= material2.Color;
		}

		public static void AddBillboardOriented(MyStringId material, Vector4 color, Vector3D origin, Vector3 leftVector, Vector3 upVector, float width, float height, List<MyBillboard> persistentBillboards = null)
		{
			AddBillboardOriented(material, color, origin, leftVector, upVector, width, height, Vector2.Zero, MyBillboard.BlendTypeEnum.Standard, -1, 0f, persistentBillboards);
		}

		public static void AddBillboardOriented(MyStringId material, Vector4 color, Vector3D origin, Vector3 leftVector, Vector3 upVector, float radius, int customProjection, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, List<MyBillboard> persistentBillboards = null)
		{
			AddBillboardOriented(material, color, origin, leftVector, upVector, radius, blendType, customProjection, 0f, persistentBillboards);
		}

		public static void AddBillboardOriented(MyStringId material, Vector4 color, Vector3D origin, Vector3 leftVector, Vector3 upVector, float width, float height, Vector2 uvOffset, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, int customViewProjection = -1, float reflection = 0f, List<MyBillboard> persistentBillboards = null)
		{
			if (IsEnabled)
			{
				MyBillboard item;
				if (persistentBillboards == null)
				{
					MyRenderProxy.BillboardsPoolWrite.AllocateOrCreate(out item);
				}
				else
				{
					item = MyRenderProxy.AddPersistentBillboard();
					persistentBillboards.Add(item);
				}
				MyUtils.GetBillboardQuadOriented(out var quad, ref origin, width, height, ref leftVector, ref upVector);
				CreateBillboard(item, ref quad, material, ref color, ref origin, uvOffset, customViewProjection);
				item.BlendType = blendType;
				item.Reflectivity = reflection;
				MyRenderProxy.AddBillboard(item);
			}
		}

		public static bool AddQuad(MyStringId material, ref MyQuadD quad, Vector4 color, ref Vector3D vctPos, int customViewProjection = -1, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, List<MyBillboard> persistentBillboards = null)
		{
			if (!IsEnabled)
			{
				return false;
			}
			MyBillboard item;
			if (persistentBillboards == null)
			{
				MyRenderProxy.BillboardsPoolWrite.AllocateOrCreate(out item);
			}
			else
			{
				item = MyRenderProxy.AddPersistentBillboard();
				persistentBillboards.Add(item);
			}
			CreateBillboard(item, ref quad, material, ref color, ref vctPos, customViewProjection);
			item.BlendType = blendType;
			MyRenderProxy.AddBillboard(item);
			return true;
		}

		public static bool AddAttachedQuad(MyStringId material, ref MyQuadD quad, Vector4 color, ref Vector3D vctPos, uint renderObjectID, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, List<MyBillboard> persistentBillboards = null)
		{
			if (!IsEnabled)
			{
				return false;
			}
			MyBillboard item;
			if (persistentBillboards == null)
			{
				MyRenderProxy.BillboardsPoolWrite.AllocateOrCreate(out item);
			}
			else
			{
				item = MyRenderProxy.AddPersistentBillboard();
				persistentBillboards.Add(item);
			}
			CreateBillboard(item, ref quad, material, ref color, ref vctPos);
			item.ParentID = renderObjectID;
			item.BlendType = blendType;
			MyRenderProxy.AddBillboard(item);
			return true;
		}

<<<<<<< HEAD
		/// <summary>
		/// Adds billboard to render. 
		/// Remember, that added persistent billboard you can change ONLY with <see cref="M:VRage.Game.MyTransparentGeometry.ApplyActionOnPersistentBillboards(System.Action{VRageRender.MyBillboard})" /> and <see cref="M:VRage.Game.MyTransparentGeometry.ApplyActionOnPersistentBillboards(System.Action)" /> methods
		/// FOR MODDERS: If you have more than 100 billboards to remove you should use another <see cref="M:VRage.Game.MyTransparentGeometry.AddBillboards(System.Collections.Generic.IEnumerable{VRageRender.MyBillboard},System.Boolean)" /> function
		/// </summary>
		/// <param name="billboard">Billboard to render</param>
		/// <param name="isPersistent">When true - billboard will continue rendering until <see cref="M:VRage.Game.MyTransparentGeometry.RemovePersistentBillboard(VRageRender.MyBillboard,System.Boolean)" /> or <see cref="M:VRage.Game.MyTransparentGeometry.RemovePersistentBillboards(System.Collections.Generic.IEnumerable{VRageRender.MyBillboard},System.Boolean)" /> is called</param>
		public static void AddBillboard(MyBillboard billboard, bool isPersistent)
		{
			if (isPersistent)
			{
				MyRenderProxy.AddPersistentBillboard(billboard);
			}
			MyRenderProxy.AddBillboard(billboard);
		}

		/// <summary>
		/// Adds billboards to render.
		/// Remember, that added persistent billboards you can change ONLY with <see cref="M:VRage.Game.MyTransparentGeometry.ApplyActionOnPersistentBillboards(System.Action{VRageRender.MyBillboard})" /> and <see cref="M:VRage.Game.MyTransparentGeometry.ApplyActionOnPersistentBillboards(System.Action)" /> methods
		/// </summary>
		/// <param name="billboards">Billboards to render</param>
		/// <param name="isPersistent">When true - billboard will continue rendering until <see cref="M:VRage.Game.MyTransparentGeometry.RemovePersistentBillboard(VRageRender.MyBillboard,System.Boolean)" /> or <see cref="M:VRage.Game.MyTransparentGeometry.RemovePersistentBillboards(System.Collections.Generic.IEnumerable{VRageRender.MyBillboard},System.Boolean)" /> is called</param>
		public static void AddBillboards(IEnumerable<MyBillboard> billboards, bool isPersistent)
		{
			if (isPersistent)
			{
				foreach (MyBillboard billboard in billboards)
				{
					MyRenderProxy.AddPersistentBillboard(billboard);
				}
			}
			MyRenderProxy.AddBillboards(billboards);
		}

		/// <summary>
		/// Removes one persistent billboard from draw
		/// FOR MODDERS: If you have more than 100 billboards to remove you should use another <see cref="M:VRage.Game.MyTransparentGeometry.RemovePersistentBillboards(System.Collections.Generic.IEnumerable{VRageRender.MyBillboard},System.Boolean)" /> function
		/// </summary>
		/// <param name="billboard">Billboard to remove</param>
		/// <param name="immediate">When true, instantly removes (causes crash if you call it from render draw thread), when false adds to remove list first</param>
		public static void RemovePersistentBillboard(MyBillboard billboard, bool immediate = false)
		{
			if (billboard != null)
			{
				MyRenderProxy.RemovePersistentBillboard(billboard, immediate);
			}
		}

		/// <summary>
		/// Removes multiple persistent billboard from draw 
		/// </summary>
		/// <param name="billboards">Billboards to remove</param>
		/// <param name="immediate">When true, instantly removes (causes crash if you call it from render draw thread), when false adds to remove list first</param>
		public static void RemovePersistentBillboards(IEnumerable<MyBillboard> billboards, bool immediate = false)
		{
			foreach (MyBillboard billboard in billboards)
			{
				if (billboard != null)
				{
					MyRenderProxy.RemovePersistentBillboard(billboard, immediate);
				}
			}
		}

		/// <summary>
		/// Used to update billboards. It iterates over each MyBillBoard. Use <see cref="M:VRage.Game.MyTransparentGeometry.ApplyActionOnPersistentBillboards(System.Action)" /> if you need iterate over specific billboards.
		/// </summary>
		/// <param name="action">action, which should be executed</param>
		public static void ApplyActionOnPersistentBillboards(Action<MyBillboard> action)
		{
			MyRenderProxy.ApplyActionOnPersistentBillboards(action);
		}

		/// <summary>
		/// Used to update billboards. You can change any values of MyBillboard inside of this action call. Used to iterate over your own set of Billboards, instead of all, that currently drawing.
		/// </summary>
		/// <param name="action">Action, which should be executed</param>
		public static void ApplyActionOnPersistentBillboards(Action action)
		{
			MyRenderProxy.ApplyActionOnPersistentBillboards(action);
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Conditional("PARTICLE_PROFILING")]
		public static void StartParticleProfilingBlock(string name)
		{
		}

		[Conditional("PARTICLE_PROFILING")]
		public static void EndParticleProfilingBlock()
		{
		}
	}
}
