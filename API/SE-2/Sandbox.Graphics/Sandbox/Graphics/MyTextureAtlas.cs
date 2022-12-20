using System.Collections.Generic;

namespace Sandbox.Graphics
{
	/// <summary>
	/// There's so little to this class you barely need it but it
	/// saves some typing if nothing else.
	/// </summary>
	public class MyTextureAtlas : Dictionary<string, MyTextureAtlasItem>
	{
		public MyTextureAtlas(int numItems)
			: base(numItems)
		{
		}
	}
}
