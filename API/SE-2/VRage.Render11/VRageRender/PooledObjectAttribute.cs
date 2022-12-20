using System;

namespace VRageRender
{
<<<<<<< HEAD
	/// <summary>
	/// Using this attribute on a class requires a public static method with
	/// the PooledObjectCleaner attribute in the class. There should be a public
	/// parameterless constructor defined too.
	/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class PooledObjectAttribute : Attribute
	{
		internal int PoolPreallocationSize;

		public PooledObjectAttribute(int poolPreallocationSize = 2)
		{
			PoolPreallocationSize = poolPreallocationSize;
		}
	}
}
