using VRage.Game.ObjectBuilders.ComponentSystem;

namespace VRage.Game.Entities
{
	public interface IMyUpdateTimer
	{
		void CreateUpdateTimer(uint startingTimeInFrames, MyTimerTypes timerType, bool start);

		bool GetTimerEnabledState();

		uint GetFramesFromLastTrigger();

		void DoUpdateTimerTick();

		void ChangeTimerTick(uint timeTickInFrames);
	}
}
