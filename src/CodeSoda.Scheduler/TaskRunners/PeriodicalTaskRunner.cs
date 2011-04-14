using System;

namespace CodeSoda.Scheduler.TaskRunners {

	internal class PeriodicalTaskRunner : ITaskRunner {
		
		private DateTime _lastRun;
		private DateTime _nextRun;
		private readonly int _frequency;

		public ITask Task { get; private set; }

		public PeriodicalTaskRunner(ITask task, int frequency):this(task, frequency, DateTime.Now) {}

		public PeriodicalTaskRunner(ITask task, int frequency, DateTime lastRun) {
			Task = task;
			_frequency = frequency;
			_lastRun = lastRun;
			_nextRun = _lastRun.AddSeconds(_frequency);
		}

		public void Check() {

			if (DateTime.Now > _nextRun && !Task.IsBusy) {
				try {
					Task.Run();
					_lastRun = _nextRun;
				}
				catch (Exception ex) {
					// this is just here so i can set breakpoints,
					// please be kinda to me compiler and remove this
					throw;
				}
				_nextRun = _nextRun.AddSeconds(_frequency);
			}

		}
	}
}
