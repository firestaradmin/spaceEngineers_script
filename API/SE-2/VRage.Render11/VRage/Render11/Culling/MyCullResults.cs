using System.Collections.Generic;
using System.Diagnostics;
using VRage.Library.Collections;
using VRage.Render.Scene;
using VRage.Render11.Emit;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.StaticGroup;
using VRage.Render11.Scene.Components;

namespace VRage.Render11.Culling
{
	internal class MyCullResults : MyCullResultsBase
	{
		[CullData(SlaveData = "CullProxiesContained")]
		internal readonly MyList<MyCullProxy> CullProxies = new MyList<MyCullProxy>();

		internal readonly MyList<bool> CullProxiesContained = new MyList<bool>();

		[CullData(SlaveData = "CullProxies2Contained")]
		internal readonly MyList<MyCullProxy_2> CullProxies2 = new MyList<MyCullProxy_2>();

		internal readonly MyList<bool> CullProxies2Contained = new MyList<bool>();

		[CullData]
		internal readonly MyList<MyInstance> Instances = new MyList<MyInstance>();

		[CullData]
		internal MyList<MyLightComponent> PointLights = new MyList<MyLightComponent>();

		[CullData]
<<<<<<< HEAD
		internal MyList<MyLightComponent> SpotLights = new MyList<MyLightComponent>();
=======
		internal readonly MyList<MyLightComponent> SpotLights = new MyList<MyLightComponent>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		internal readonly MyList<MyStaticGroup> StaticGroups = new MyList<MyStaticGroup>();

		[CullData]
		internal readonly MyList<MyFoliageComponent> Foliage = new MyList<MyFoliageComponent>();

		/// <summary>
		/// Get a bitmask of the cull lists that have contents.
		/// </summary>
		/// <returns></returns>
		public byte GetFlags()
		{
			byte b = 0;
			if (CullProxies.Count > 0)
			{
				b = (byte)(b | 1u);
			}
			if (CullProxies2.Count > 0)
			{
				b = (byte)(b | 2u);
			}
			if (Instances.Count > 0)
			{
				b = (byte)(b | 4u);
			}
			if (PointLights.Count > 0)
			{
				b = (byte)(b | 8u);
			}
			if (SpotLights.Count > 0)
			{
				b = (byte)(b | 0x10u);
			}
			if (Foliage.Count > 0)
			{
				b = (byte)(b | 0x20u);
			}
			return b;
		}

		public static IEnumerable<string> GetFieldsFromMask(byte mask)
		{
			if (IsSet(0))
			{
				yield return "CullProxies";
			}
			if (IsSet(1))
			{
				yield return "CullProxies2";
			}
			if (IsSet(2))
			{
				yield return "Instances";
			}
			if (IsSet(3))
			{
				yield return "PointLights";
			}
			if (IsSet(4))
			{
				yield return "SpotLights";
			}
			if (IsSet(5))
			{
				yield return "Foliage";
			}
			bool IsSet(int bit)
			{
				return (mask & (1 << bit)) != 0;
			}
		}

		internal int GetCounts()
		{
			return CullProxies.Count + CullProxies2.Count + Instances.Count + PointLights.Count + SpotLights.Count + StaticGroups.Count + Foliage.Count;
		}

		public void Clear()
		{
			CullProxies.SetSize(0);
			CullProxiesContained.SetSize(0);
			CullProxies2.SetSize(0);
			CullProxies2Contained.SetSize(0);
			Instances.SetSize(0);
			PointLights.SetSize(0);
			SpotLights.SetSize(0);
			StaticGroups.SetSize(0);
			Foliage.SetSize(0);
		}

		public bool Empty()
		{
			if (GetCounts() == 0 && CullProxiesContained.Count == 0)
			{
				return CullProxies2Contained.Count == 0;
			}
			return false;
		}

		public void Append(MyCullResults all)
		{
			CullProxies.AddRange(all.CullProxies);
			CullProxiesContained.AddRange(all.CullProxiesContained);
			CullProxies2.AddRange(all.CullProxies2);
			CullProxies2Contained.AddRange(all.CullProxies2Contained);
			Instances.AddRange(all.Instances);
			StaticGroups.AddRange(all.StaticGroups);
			SpotLights.AddRange(all.SpotLights);
			PointLights.AddRange(all.PointLights);
			Foliage.AddRange(all.Foliage);
		}

		[Conditional("DEBUG")]
		public void AssertSingletons()
		{
		}
	}
}
