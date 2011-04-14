
namespace CodeSoda.Scheduler {

	public interface ITask {
		void Run();
		bool IsBusy { get; }

		// TODO MaxRunTime - maximum time this should be able to run for before scheduler thinks its bombed out
		// TODO Priority - used to prioritise scheduledtasks
		// TODO IdleWait - how long the scheduler has to be idle after starting before this task is "run" for the first time
	}

}
