using SharpDX.Direct3D11;
using VRage.Network;
using VRageRender;

namespace VRage.Render11.Common
{
	[GenerateActivator]
	internal class MyQuery
	{
		private class VRage_Render11_Common_MyQuery_003C_003EActor : IActivator, IActivator<MyQuery>
		{
			private sealed override object CreateInstance()
			{
				return new MyQuery();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyQuery CreateInstance()
			{
				return new MyQuery();
			}

			MyQuery IActivator<MyQuery>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		internal Query Query;

		private QueryType? m_type;

		internal void LazyInit(QueryType type)
		{
			if (Query == null)
			{
				m_type = type;
				QueryDescription queryDescription = default(QueryDescription);
				queryDescription.Type = type;
				QueryDescription description = queryDescription;
				Query = new Query(MyRender11.DeviceInstance, description);
			}
		}

		public static implicit operator Query(MyQuery q)
		{
			return q.Query;
		}
	}
}
