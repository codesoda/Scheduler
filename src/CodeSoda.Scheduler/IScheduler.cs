
namespace CodeSoda.Scheduler {

	public interface IScheduler {
		int Frequency { get; }
		void Start();
		void Stop();
		void AddTask(ITask task, Schedule schedule);
	}
}
