using System;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Multiplayer;
using VRage.Input;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Utils
{
	[StaticEventOwner]
	internal class MyAdvancedDebugDraw
	{
		protected sealed class DebugDrawLine3DSyncInternal_003C_003EVRageMath_Vector3D_0023VRageMath_Vector3D_0023VRageMath_Color_0023System_Nullable_00601_003CVRage_Input_MyKeys_003E : ICallSite<IMyEventOwner, Vector3D, Vector3D, Color, MyKeys?, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in Vector3D start, in Vector3D end, in Color color, in MyKeys? key, in DBNull arg5, in DBNull arg6)
			{
				DebugDrawLine3DSyncInternal(start, end, color, key);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Debug draw line that can be called both on server and client. If called on server, it will be broadcasted to all clients. Color can be different for both server called and client called ones.
		/// Client does nto see other client's calls. Can be assigned key, then lines will only be drawn when key is held, key == null if line should always be drawn.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="colorServer"></param>
		/// <param name="colorClient"></param>
		/// <param name="key"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static void DebugDrawLine3DSync(Vector3D start, Vector3D end, Color colorServer, Color colorClient, MyKeys? key = null)
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => DebugDrawLine3DSyncInternal, start, end, colorServer, key);
<<<<<<< HEAD
				if (!Sync.IsDedicated)
				{
					DebugDrawLine3DInternal(start, end, colorClient, key);
				}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				DebugDrawLine3DInternal(start, end, colorClient, key);
			}
		}

<<<<<<< HEAD
		[Event(null, 40)]
=======
		[Event(null, 36)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		protected static void DebugDrawLine3DSyncInternal(Vector3D start, Vector3D end, Color color, MyKeys? key)
		{
			DebugDrawLine3DInternal(start, end, color, key);
		}

		protected static void DebugDrawLine3DInternal(Vector3D start, Vector3D end, Color color, MyKeys? key)
		{
			if (!key.HasValue || MyInput.Static.IsKeyPress(key.Value))
			{
				MyRenderProxy.DebugDrawLine3D(start, end, color, color, depthRead: true, persistent: true);
			}
		}
	}
}
