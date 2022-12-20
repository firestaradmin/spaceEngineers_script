using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentMotorStator : MyDebugRenderComponent
	{
		private MyMotorStator m_motor;

		public MyDebugRenderComponentMotorStator(MyMotorStator motor)
			: base(motor)
		{
			m_motor = motor;
		}

		public override void DebugDraw()
		{
			if (m_motor.CanDebugDraw() && MyDebugDrawSettings.DEBUG_DRAW_ROTORS)
			{
				MatrixD worldMatrixRef = m_motor.PositionComp.WorldMatrixRef;
				MatrixD worldMatrix = m_motor.Rotor.WorldMatrix;
<<<<<<< HEAD
				Vector3D vector3D = Vector3D.Lerp(worldMatrixRef.Translation, worldMatrix.Translation, 0.5);
				Vector3 vector = Vector3.Normalize(worldMatrixRef.Up);
				MyRenderProxy.DebugDrawLine3D(vector3D, vector3D + vector, Color.Yellow, Color.Yellow, depthRead: false);
=======
				Vector3 vector = Vector3.Lerp(worldMatrixRef.Translation, worldMatrix.Translation, 0.5f);
				Vector3 vector2 = Vector3.Normalize(worldMatrixRef.Up);
				MyRenderProxy.DebugDrawLine3D(vector, vector + vector2, Color.Yellow, Color.Yellow, depthRead: false);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyRenderProxy.DebugDrawLine3D(worldMatrixRef.Translation, worldMatrix.Translation, Color.Red, Color.Green, depthRead: false);
			}
		}
	}
}
