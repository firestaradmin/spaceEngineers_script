using System.Collections.Concurrent;
using VRage.Collections;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public class MyDirtyRegion
	{
		public ConcurrentQueue<MyCube> PartsToRemove = new ConcurrentQueue<MyCube>();

		public ConcurrentCachingHashSet<Vector3I> Cubes = new ConcurrentCachingHashSet<Vector3I>();

		public bool IsDirty
		{
			get
			{
				Cubes.ApplyChanges();
				if (Cubes.Count <= 0)
				{
					return !PartsToRemove.get_IsEmpty();
				}
				return true;
			}
		}

<<<<<<< HEAD
		public int Count => Cubes.Count + PartsToRemove.Count;
=======
		public int Count => Cubes.Count + PartsToRemove.get_Count();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public void AddCube(Vector3I pos)
		{
			Cubes.Add(pos);
		}

		/// <summary>
		/// Adds dirty region, min and max are inclusive
		/// </summary>
		public void AddCubeRegion(Vector3I min, Vector3I max)
		{
			Vector3I item = default(Vector3I);
			item.X = min.X;
			while (item.X <= max.X)
			{
				item.Y = min.Y;
				while (item.Y <= max.Y)
				{
					item.Z = min.Z;
					while (item.Z <= max.Z)
					{
						Cubes.Add(item);
						item.Z++;
					}
					item.Y++;
				}
				item.X++;
			}
		}

		public void Clear()
		{
			Cubes.Clear();
		}
	}
}
