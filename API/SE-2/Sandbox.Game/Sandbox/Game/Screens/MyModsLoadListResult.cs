using System.Collections.Generic;
using System.Linq;
using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using VRage.Game;
using VRage.GameServices;

namespace Sandbox.Game.Screens
{
	public class MyModsLoadListResult : IMyAsyncResult
	{
		public bool IsCompleted => Task.IsComplete;

		public Task Task { get; private set; }

		public (MyGameServiceCallResult, string) Result { get; private set; }

<<<<<<< HEAD
		/// <summary>
		/// List of mods user is subscribed to, or null if there was an error
		/// during operation.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public List<MyWorkshopItem> SubscribedMods { get; private set; }

		public List<MyWorkshopItem> SetMods { get; private set; }

		public MyModsLoadListResult(HashSet<WorkshopId> ids)
		{
			MyModsLoadListResult myModsLoadListResult = this;
<<<<<<< HEAD
			HashSet<WorkshopId> toGet = new HashSet<WorkshopId>(ids);
			Task = Parallel.Start(delegate
			{
				myModsLoadListResult.SubscribedMods = new List<MyWorkshopItem>(ids.Count);
=======
			HashSet<WorkshopId> toGet = new HashSet<WorkshopId>((IEnumerable<WorkshopId>)ids);
			Task = Parallel.Start(delegate
			{
				myModsLoadListResult.SubscribedMods = new List<MyWorkshopItem>(ids.get_Count());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myModsLoadListResult.SetMods = new List<MyWorkshopItem>();
				if (MyGameService.IsOnline)
				{
					(MyGameServiceCallResult, string) subscribedModsBlocking = MyWorkshop.GetSubscribedModsBlocking(myModsLoadListResult.SubscribedMods);
					foreach (MyWorkshopItem subscribedMod in myModsLoadListResult.SubscribedMods)
					{
						toGet.Remove(new WorkshopId(subscribedMod.Id, subscribedMod.ServiceName));
					}
<<<<<<< HEAD
					if (toGet.Count > 0)
					{
						MyWorkshop.GetItemsBlockingUGC(toGet.ToList(), myModsLoadListResult.SetMods);
=======
					if (toGet.get_Count() > 0)
					{
						MyWorkshop.GetItemsBlockingUGC(Enumerable.ToList<WorkshopId>((IEnumerable<WorkshopId>)toGet), myModsLoadListResult.SetMods);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					myModsLoadListResult.Result = subscribedModsBlocking;
				}
			});
		}
	}
}
