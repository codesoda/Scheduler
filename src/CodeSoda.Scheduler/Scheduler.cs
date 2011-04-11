using System;
using System.Collections.Generic;
using System.Threading;

namespace CodeSoda.Scheduler {

	public class Scheduler: IScheduler, IDisposable {

		private Timer _timer;
		private readonly IList<ITask> _tasks;
		private readonly IList<ITaskRunner> _taskRunners;

		public int Frequency { get; private set; }

		public Scheduler(int frequency) {
			_taskRunners = new List<ITaskRunner>();
			_tasks = new List<ITask>();

			this.Frequency = frequency;
			_timer = new Timer(Timer_Tick);
		}

		~Scheduler() {
			this.Dispose();
		}


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

		public void Start() {
			lock(this)
				_timer.Change(Frequency*1000, Frequency*1000);
		}

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
