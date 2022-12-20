using System;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Cube;
using VRage.Game;
using VRage.Network;
using VRageRender;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_EmissiveBlock))]
	public class MyEmissiveBlock : MyCubeBlock
	{
		private class Sandbox_Game_Entities_MyEmissiveBlock_003C_003EActor : IActivator, IActivator<MyEmissiveBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyEmissiveBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEmissiveBlock CreateInstance()
			{
				return new MyEmissiveBlock();
			}

			MyEmissiveBlock IActivator<MyEmissiveBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const string EMISSIVE_PART = "EmissiveColorable";

		public event Action<MyEmissiveBlock> OnModelChanged;

		public void SetEmissivity(float emissivity)
		{
			SetEmissiveParts("EmissiveColorable", MyColorPickerConstants.HSVOffsetToHSV(base.Render.ColorMaskHsv).HsvToRgb(), emissivity);
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			this.OnModelChanged?.Invoke(this);
		}
	}
}
