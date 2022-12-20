namespace VRageRender.Messages
{
	/// <summary>
	/// Material slot uses same semantics as texture params in MyMaterialDescriptor:
	/// "ColorMetalTexture"
	/// "NormalGlossTexture"
	/// "AddMapsTexture"
	/// "AlphamaskTexture"
	/// if MaterialSlot == null: will be treated as "ColorMetalTexture" (please don't mix explicit and implicit slot naming for same object)
	/// in dx9 renderer it is used only to change diffuse texture, so MaterialSlot can be null
	/// </summary>
	public struct MyTextureChange
	{
		public string ColorMetalFileName;

		public string NormalGlossFileName;

		public string ExtensionsFileName;

		public string AlphamaskFileName;

		public bool IsDefault()
		{
			if (ColorMetalFileName == null && NormalGlossFileName == null && ExtensionsFileName == null)
			{
				return AlphamaskFileName == null;
			}
			return false;
		}
	}
}
