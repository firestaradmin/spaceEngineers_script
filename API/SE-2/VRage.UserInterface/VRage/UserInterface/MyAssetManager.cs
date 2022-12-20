using System.Collections.Generic;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Media;

namespace VRage.UserInterface
{
	public class MyAssetManager : AssetManager
	{
		public Dictionary<string, FontBase> Fonts { get; private set; } = new Dictionary<string, FontBase>();


		public HashSet<string> GeneratedTextures { get; private set; } = new HashSet<string>();


		public override FontBase LoadFont(object contentManager, string file)
		{
			if (!Fonts.TryGetValue(file, out var value))
			{
				throw new KeyNotFoundException("Font not found - " + file);
			}
			return value;
		}

		public override SoundBase LoadSound(object contentManager, string file)
		{
			return Engine.Instance.AudioDevice.CreateSound(file);
		}

		public override TextureBase LoadTexture(object contentManager, string file)
		{
			return Engine.Instance.Renderer.CreateTexture(file);
		}

		public override EffectBase LoadEffect(object contentManager, string file)
		{
			return null;
		}

		public void UnloadGeneratedTextures()
		{
			GeneratedTextures.Clear();
		}
	}
}
