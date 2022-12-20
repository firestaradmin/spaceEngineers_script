using System;
using System.Collections.Generic;
using VRage.Utils;

namespace VRageRender
{
	public class MyDecalMaterial
	{
		public bool Transparent;

		public string StringId { get; private set; }

		public MyDecalMaterialDesc Material { get; private set; }

		public MyStringHash Target { get; private set; }

		public MyStringHash Source { get; private set; }

		public float MinSize { get; private set; }

		public float MaxSize { get; private set; }

		public float Depth { get; private set; }

		public float Alpha { get; private set; }

		public float XOffset { get; private set; }

		public float YOffset { get; private set; }

		public float Spacing { get; private set; }

		/// <summary>
		/// Positive infinity for random rotation
		/// </summary>
		public float Rotation { get; private set; }

		public List<MyStringHash> Blacklist { get; private set; }

		public float RenderDistance { get; private set; }

		public MyDecalMaterial(MyDecalMaterialDesc materialDef, bool transparent, MyStringHash target, MyStringHash source, float minSize, float maxSize, float depth, float rotation, List<MyStringHash> blacklist = null, float xOffset = 0f, float yOffset = 0f, float alpha = 1f, float spacing = -1f, float renderDistance = 50f)
		{
			StringId = MyDecalMaterials.GetStringId(source, target);
			Material = materialDef;
			Target = target;
			Source = source;
			MinSize = minSize;
			MaxSize = maxSize;
			Depth = depth;
			Rotation = (float)((double)rotation * Math.PI / 180.0);
			Transparent = transparent;
			XOffset = xOffset;
			YOffset = yOffset;
			Alpha = alpha;
			Spacing = spacing;
			Blacklist = blacklist;
			RenderDistance = renderDistance;
		}

		public void SetBlacklist(List<MyStringHash> blacklist)
		{
			Blacklist = blacklist;
		}
	}
}
