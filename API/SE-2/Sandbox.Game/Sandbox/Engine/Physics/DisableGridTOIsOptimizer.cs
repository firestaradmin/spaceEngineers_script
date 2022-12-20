<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using Havok;
using Sandbox.Game.Entities.Cube;
using VRage;
using VRage.Library.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Physics
{
	internal class DisableGridTOIsOptimizer : PhysicsStepOptimizerBase
	{
		public static DisableGridTOIsOptimizer Static;

		private HashSet<MyGridPhysics> m_optimizedGrids = new HashSet<MyGridPhysics>();

		public DisableGridTOIsOptimizer()
		{
			Static = this;
		}

		public override void Unload()
		{
			Static = null;
		}

		public override void EnableOptimizations(List<MyTuple<HkWorld, MyTimeSpan>> timings)
		{
			PhysicsStepOptimizerBase.ForEverySignificantWorld(timings, delegate(HkWorld world)
			{
				PhysicsStepOptimizerBase.ForEveryActivePhysicsBodyOfType(world, delegate(MyGridPhysics body)
				{
					body.ConsiderDisablingTOIs();
				});
			});
		}

		public override void DisableOptimizations()
		{
			while (m_optimizedGrids.get_Count() > 0)
			{
				m_optimizedGrids.FirstElement<MyGridPhysics>().DisableTOIOptimization();
			}
		}

		public void Register(MyGridPhysics grid)
		{
			m_optimizedGrids.Add(grid);
		}

		public void Unregister(MyGridPhysics grid)
		{
			m_optimizedGrids.Remove(grid);
		}

		public void DebugDraw()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyGridPhysics> enumerator = Static.m_optimizedGrids.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGridPhysics current = enumerator.get_Current();
					MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(current.Entity.LocalAABB, current.Entity.WorldMatrix), Color.Yellow, 1f, depthRead: false, smooth: false);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}
	}
}
