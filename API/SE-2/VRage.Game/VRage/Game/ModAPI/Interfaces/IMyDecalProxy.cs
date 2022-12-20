using System.Collections.Generic;
using VRage.Utils;
using VRageMath;
<<<<<<< HEAD
using VRageRender;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

namespace VRage.Game.ModAPI.Interfaces
{
	public interface IMyDecalProxy
	{
<<<<<<< HEAD
		/// <summary>
		/// Called from Sandbox.Game.MyDecals
		/// </summary>
		/// <param name="hitInfo">Describes where it should be placed</param>
		/// <param name="source">Decal material</param>
		/// <param name="forwardDirection">Use for rotation of decal</param>
		/// <param name="customdata">Custom information about how to position decals</param>
		/// <param name="decalHandler">Sandbox.Game.MyDecals instance.</param>
		/// <param name="physicalMaterial">Physical material</param>
		/// <param name="voxelMaterial">Voxel material</param>
		/// <param name="isTrail">Is it trail, that wheels are leaving</param>
		/// <param name="flags"><see cref="T:VRageRender.MyDecalFlags" /></param>
		/// <param name="aliveUntil">Time in frames. When it is less than <see cref="P:VRage.Game.ModAPI.IMySession.GameplayFrameCounter" />, it would be removed</param>
		/// <param name="outids">If not null, generated decal ids would be added to that list</param>
		void AddDecals(ref MyHitInfo hitInfo, MyStringHash source, Vector3 forwardDirection, object customdata, IMyDecalHandler decalHandler, MyStringHash physicalMaterial, MyStringHash voxelMaterial, bool isTrail, MyDecalFlags flags = MyDecalFlags.None, int aliveUntil = int.MaxValue, List<uint> outids = null);
=======
		/// <param name="hitInfo">Hithinfo on world coordinates</param>
		/// <param name="source"></param>
		/// <param name="customdata"></param>
		/// <param name="decalHandler"></param>
		/// <param name="physicalMaterial"></param>
		void AddDecals(ref MyHitInfo hitInfo, MyStringHash source, Vector3 forwardDirection, object customdata, IMyDecalHandler decalHandler, MyStringHash physicalMaterial, MyStringHash voxelMaterial, bool isTrail);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
