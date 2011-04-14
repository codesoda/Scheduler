
namespace CodeSoda.Scheduler{

	internal interface ITaskRunner {
		void Check();
		ITask Task { get; }
	}
}
