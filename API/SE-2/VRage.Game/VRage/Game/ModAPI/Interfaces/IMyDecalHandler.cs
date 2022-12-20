using System.Collections.Generic;
using VRageRender;

namespace VRage.Game.ModAPI.Interfaces
{
	public interface IMyDecalHandler
	{
		/// <summary>
		/// Adds decal
		/// </summary>
		/// <param name="renderInfo">Information about decal </param>
		/// <param name="ids">If not null, generated decal ids would be added to that list</param>
		void AddDecal(ref MyDecalRenderInfo renderInfo, List<uint> ids = null);
	}
}
