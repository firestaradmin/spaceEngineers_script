using System;
using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct WorkshopId : IComparable
	{
		protected class VRage_Game_WorkshopId_003C_003EId_003C_003EAccessor : IMemberAccessor<WorkshopId, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref WorkshopId owner, in ulong value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref WorkshopId owner, out ulong value)
			{
				value = owner.Id;
			}
		}

		protected class VRage_Game_WorkshopId_003C_003EServiceName_003C_003EAccessor : IMemberAccessor<WorkshopId, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref WorkshopId owner, in string value)
			{
				owner.ServiceName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref WorkshopId owner, out string value)
			{
				value = owner.ServiceName;
			}
		}

		private class VRage_Game_WorkshopId_003C_003EActor : IActivator, IActivator<WorkshopId>
		{
			private sealed override object CreateInstance()
			{
				return default(WorkshopId);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override WorkshopId CreateInstance()
			{
				return (WorkshopId)(object)default(WorkshopId);
			}

			WorkshopId IActivator<WorkshopId>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public ulong Id;

		[ProtoMember(4)]
		public string ServiceName;

		public WorkshopId(ulong id, string serviceName)
		{
			Id = id;
			ServiceName = serviceName;
		}

		public int CompareTo(object obj)
		{
			WorkshopId workshopId = (WorkshopId)obj;
			int num = workshopId.ServiceName.CompareTo(ServiceName);
			if (num != 0)
			{
				return num;
			}
			return workshopId.Id.CompareTo(Id);
		}
	}
}
