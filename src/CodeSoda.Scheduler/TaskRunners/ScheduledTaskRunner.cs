
using System;

namespace CodeSoda.Scheduler.TaskRunners {

	internal class ScheduledTaskRunner : ITaskRunner {
		private DateTime _lastRun;
		private readonly TimeSpan _runAt;

		public ITask Task { get; private set; }

		public ScheduledTaskRunner(ITask task, TimeSpan runAt) {
			Task = task;
			_runAt = runAt;
		}

		public void Check() {
			if (_lastRun.Date != DateTime.Now.Date && DateTime.Now.TimeOfDay > _runAt && !Task.IsBusy) {
				try {
					Task.Run();
				}
				catch { }
				_lastRun = DateTime.Now;
			}
		}
	}

}
