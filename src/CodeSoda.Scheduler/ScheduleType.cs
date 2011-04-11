

namespace CodeSoda.Scheduler {
	/// <summary>
	/// Indicates the type of timing used to determine how often a scheduled item should run
	/// </summary>
	public enum ScheduleType
	{
		/// <summary>
		/// Indicates a Task should be consecutively run after a predefined number of seconds
		/// </summary>
		Periodical,

		/// <summary>
		/// Indicates a task should be run at a pre-defined time every day
		/// </summary>
		Scheduled,

		/// <summary>
		/// Indicates a task is responsible for managing its own scheduled execution
		/// </summary>
		Task
	}
}
