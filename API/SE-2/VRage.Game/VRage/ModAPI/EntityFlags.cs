using System;

namespace VRage.ModAPI
{
	/// <summary>
	/// Entity flags.
	/// </summary>
	[Flags]
	public enum EntityFlags
	{
		/// <summary>
		/// No flags
		/// </summary>
		None = 0x1,
		/// <summary>
		/// Specifies whether draw this entity or not.
		/// </summary>
		Visible = 0x2,
		/// <summary>
		/// Specifies whether save entity when saving sector or not
		/// </summary>
		Save = 0x8,
		/// <summary>
		/// Specifies whether entity is "near", near entities are cockpit and weapons, these entities are rendered in special way
		/// </summary>
		Near = 0x10,
		/// <summary>
		/// On this entity and its children will be called UpdateBeforeSimulation and UpdateAfterSimulation each frame
		/// </summary>
		NeedsUpdate = 0x20,
		/// <summary>
		/// Flags would be delivered to render component. <see cref="F:VRageRender.RenderFlags.NeedsResolveCastShadow" />
		/// </summary>
		NeedsResolveCastShadow = 0x40,
		/// <summary>
		/// Flags would be delivered to render component. <see cref="F:VRageRender.RenderFlags.FastCastShadowResolve" />
		/// </summary>
		FastCastShadowResolve = 0x80,
		/// <summary>
		/// Flags would be delivered to render component. <see cref="F:VRageRender.RenderFlags.SkipIfTooSmall" />
		/// </summary>
		SkipIfTooSmall = 0x100,
		/// <summary>
		/// Entity updated each 10th frame
		/// </summary>
		NeedsUpdate10 = 0x200,
		/// <summary>
		/// Entity updated each 100th frame
		/// </summary>
		NeedsUpdate100 = 0x400,
		/// <summary>
		/// Draw method of this entity will be called when suitable. <see cref="P:VRage.Game.Components.MyRenderComponentBase.NeedsDraw" />
		/// </summary>
		NeedsDraw = 0x800,
		/// <summary>
		/// If object is moved, invalidate its renderobjects (update render)
		/// </summary>
		InvalidateOnMove = 0x1000,
		/// <summary>
		/// Synchronize object during multiplayer
		/// </summary>
		Sync = 0x2000,
		/// <summary>
		/// Draw method of this entity will be called when suitable and only from parent
		/// </summary>
		NeedsDrawFromParent = 0x4000,
		/// <summary>
		/// Draw LOD shadow as box
		/// </summary>
		ShadowBoxLod = 0x8000,
		/// <summary>
		/// Render the entity using dithering to simulate transparency
		/// </summary>
		Transparent = 0x10000,
		/// <summary>
		/// Entity updated once before first frame.
		/// </summary>
		NeedsUpdateBeforeNextFrame = 0x20000,
		/// <summary>
		/// Flags would be delivered to render component. <see cref="F:VRageRender.RenderFlags.DrawOutsideViewDistance" />
		/// </summary>
		DrawOutsideViewDistance = 0x40000,
		/// <summary>
		/// Can be added, removed, or updated in <b>Sandbox.Game.Entities.MyGamePruningStructure</b>
		/// </summary>
		IsGamePrunningStructureObject = 0x80000,
		/// <summary>
		/// If child, its world matrix must be always updated
		/// </summary>
		NeedsWorldMatrix = 0x100000,
		/// <summary>
		/// Do not use in prunning, even though it is a root entity
		/// </summary>
		IsNotGamePrunningStructureObject = 0x200000,
		/// <summary>
		/// Entity has special simulation update. <see cref="F:VRage.ModAPI.MyEntityUpdateEnum.SIMULATE" />
		/// </summary>
		NeedsSimulate = 0x400000,
		/// <summary>
		/// Entity call <see cref="M:VRage.Game.Components.MyRenderComponentBase.UpdateRenderObject(System.Boolean,System.Boolean)" /> on OnAddedToScene
		/// </summary>
		UpdateRender = 0x800000,
		Default = 0x90114A
	}
}
