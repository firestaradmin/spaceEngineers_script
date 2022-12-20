using System;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace VRage.ModAPI
{
	public struct PhysicsSettings
	{
		/// <summary>
		/// For modders:
		/// You can create your own entites with this code.
		///             var entity = new MyEntity();          
		///             entity.WorldMatrix = settings.WorldMatrix;
		///             entity.Init(new StringBuilder("Name"), "Models\\Cubes\\Large\\GeneratorLarge.mwm", null, null, "Models\\Cubes\\Large\\GeneratorLarge.mwm");
		///             MyAPIGateway.Entities.AddEntity(entity);
		/// </summary>
		public IMyEntity Entity;

		public MatrixD WorldMatrix;

		public Vector3 LocalCenter;

		public float LinearDamping;

		public float AngularDamping;

		public ushort CollisionLayer;

		public RigidBodyFlag RigidBodyFlags;

		public MyStringHash MaterialType;

		/// <summary>
		/// Is mainly used, to detect if block can be placed at this position
		/// </summary>
		public bool IsPhantom;

		/// <summary>
		/// If it is not null, then it would be call this callback each time entity Enters/Leaves it's collision.
		/// Also removes physical collision (Now all entities can go through it)
		/// CollisionCallback is called from parallel thread, and called once per HkBody. Grids can have more than 100 bodies.
		/// Try find best collision layer, to filter unneeded collisions and improve performance
		/// If you trying detect grids:
		///
		/// protected MyConcurrentHashSet&lt;long&gt; m_containedEntities = new MyConcurrentHashSet&lt;long&gt;();
		/// var topEntity = entity.GetTopMostParent() as MyEntity;
		/// if (m_containedEntities.Add(topEntity.EntityId))
		/// {
		///     MyAPIGateway.Utilities.InvokeOnGameThread(() =&gt;
		///     {
		///         //Called once in main thread
		///     });
		/// }
		/// </summary>
		public Action<IMyEntity, bool> DetectorColliderCallback;

		public ModAPIMass? Mass;
	}
}
