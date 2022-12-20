using System;
using System.Collections.Generic;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace VRageRender
{
	/// <summary>
	/// This class is used for storing and sorting particle billboards
	///
	/// You can change it's values ONLY WITH MyTransparentGeometry.ApplyActionOnPersistentBillboards
	/// </summary>
	[GenerateActivator]
	public class MyBillboard : IComparable
	{
		public enum BlendTypeEnum
		{
			Standard = 0,
			AdditiveBottom = 1,
			AdditiveTop = 2,
			LDR = 3,
			PostPP = 4,
			SDR = 3
		}

		public enum LocalTypeEnum
		{
			Custom,
			Line,
			Point
		}

		public MyStringId Material;

		public BlendTypeEnum BlendType;

		public Vector3D Position0;

		public Vector3D Position1;

		public Vector3D Position2;

		public Vector3D Position3;

		/// <summary>
		/// ColorExtensions.ToLinearRGB((Vector4)Color.Aqua)
		///
		/// Color depends on AlphaMisting of material. You can get material properties with MyDefinitionManager.Static.GetTransparentMaterialDefinitions()
		/// </summary>
		public Vector4 Color;

		/// <summary>
		/// Default is 1, zero makes billboard - black
		/// </summary>
		public float ColorIntensity;

		public float SoftParticleDistanceScale;

		/// <summary>
		/// Default is Vector2.Zero
		/// </summary>
		public Vector2 UVOffset;

		/// <summary>
		/// Default is Vector2 (1,1)
		/// </summary>
		public Vector2 UVSize;

		public LocalTypeEnum LocalType;

		public uint ParentID = uint.MaxValue;

		public float DistanceSquared;

		public float Reflectivity;

		public float AlphaCutout;

		public int CustomViewProjection;

		internal LinkedListNode<MyBillboard> Node;

		public int CompareTo(object compareToObject)
		{
			MyBillboard myBillboard = (MyBillboard)compareToObject;
			return Material.Id.CompareTo(myBillboard.Material.Id);
		}
	}
}
