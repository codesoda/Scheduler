using System;
using System.Collections.Generic;
using System.Threading;
using CodeSoda.Scheduler.TaskRunners;

namespace CodeSoda.Scheduler {

	/// <summary>
	/// Manages Tasks and the schedule that defines when they will run in the background
	/// </summary>
	public class Scheduler: IScheduler, IDisposable {

		/// <summary>
		/// This is the Default Frequency for any Schedulers created without a specific frequency.
		/// </summary>
		public static int DefaultFrequency = 5;

		private Timer _timer;
		private readonly IList<ITask> _tasks;
		private readonly IList<ITaskRunner> _taskRunners;

		/// <summary>
		/// The number of seconds that must elapse between checking for tasks to run
		/// </summary>
		public int Frequency { get; private set; }

		/// <summary>
		/// Instantiate a new Scheduler to run background tasks using the DefaultFrequency
		/// </summary>
		public Scheduler():this(DefaultFrequency) {}

		/// <summary>
		/// Instantiate a new Scheduler to run background tasks
		/// </summary>
		/// <param name="frequency">The number of seconds that must elapse between checking for tasks to run</param>
		public Scheduler(int frequency) {
			_taskRunners = new List<ITaskRunner>();
			_tasks = new List<ITask>();

			this.Frequency = frequency;
			_timer = new Timer(Timer_Tick);
		}

		~Scheduler() {
			this.Dispose();
		}

		/// <summary>
		/// Adds a task onto the schedule so it will be checked and run as defined by the schedule
		/// </summary>
		/// <param name="task">The task to run</param>
		/// <param name="schedule">The schedule that determines when the task should run</param>
		public void AddTask(ITask task, Schedule schedule) {
			
			switch(schedule.Type) {
				case ScheduleType.Periodical:
					var p = new PeriodicalTaskRunner(task, schedule.Frequency);
					_taskRunners.Add(p);
					break;

				case ScheduleType.Scheduled:
					var s = new ScheduledTaskRunner(task, schedule.RunAt);
					_taskRunners.Add(s);
					break;

				case ScheduleType.Task:
					_tasks.Add(task);
					break;

				default:
					throw new Exception(schedule.Type + " is not a supported schedule type.");

			}
		}

		/// <summary>
		/// Start checking if schedules are due to run their tasks
		/// </summary>
		public void Start() {
			lock(this)
				_timer.Change(Frequency*1000, Frequency*1000);
		}

		/// <summary>
		/// Stop schedules checking if they are due to run
		/// </summary>
		public void Stop() {
			lock(this)
				_timer.Change(Timeout.Infinite, Timeout.Infinite);
		}

		private void Timer_Tick(object obj) {
			// run tasks sequentially one ofter the other
			// could use threads here
			if (_taskRunners != null && _taskRunners.Count > 0) {
				foreach (var runner in _taskRunners)
					runner.Check();
			}

			// make sure any tasks are running, should be on their own threads
		}

		/// <summary>
		/// Close and clean up any opened resources
		/// </summary>
		public void Dispose() {

			// ensure the timer is stopped
			Stop();

			// kill the timer
			_timer.Dispose();
			_timer = null;

			// clean up any tasks which are disposable
			foreach (var runner in _taskRunners) {
				if (runner.Task is IDisposable)
					((IDisposable)runner.Task).Dispose();
			}

			// clean up any tasks which are disposable
			foreach(var task in _tasks) {
				if (task is IDisposable)
					((IDisposable)task).Dispose();
			}
		}

	}

}
