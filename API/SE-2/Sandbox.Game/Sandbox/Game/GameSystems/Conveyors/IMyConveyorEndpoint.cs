using System.Collections;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using VRage.Algorithms;

namespace Sandbox.Game.GameSystems.Conveyors
{
	public interface IMyConveyorEndpoint : IMyPathVertex<IMyConveyorEndpoint>, IEnumerable<IMyPathEdge<IMyConveyorEndpoint>>, IEnumerable
	{
		MyCubeBlock CubeBlock { get; }

		/// <summary>
		/// Returns a connecting line for the given line position, or null, if no such line exists
		/// </summary>
		MyConveyorLine GetConveyorLine(ConveyorLinePosition position);

		MyConveyorLine GetConveyorLine(int index);

		ConveyorLinePosition GetPosition(int index);

		void DebugDraw();

		/// <summary>
		/// Changes a conveyor line of this block
		/// </summary>
		void SetConveyorLine(ConveyorLinePosition position, MyConveyorLine newLine);

		int GetLineCount();
	}
}
