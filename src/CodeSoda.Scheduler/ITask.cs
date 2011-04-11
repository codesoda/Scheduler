
using System;

namespace CodeSoda.Scheduler
{
	public interface ITask {
		void Run();
		bool IsBusy { get; }

		// MaxRunTime - maximum time this should be able to run for before scheduler thinks its bombed out
		// Priority - used to prioritise scheduledtasks
		// IdleWait - how long the scheduler has to be idle after starting before this task is "run" for the first time
	}

	internal interface ITaskRunner {
		void Check();
		ITask Task { get; }
	}

	internal class PeriodicalTaskRunner: ITaskRunner {
		private DateTime _lastRun;
		private DateTime _nextRun;
		private readonly int _frequency;

		public ITask Task { get; private set; }

		public PeriodicalTaskRunner(ITask task, int frequency) {
			Task = task;
			_frequency = frequency;
			_lastRun = DateTime.Now; // load from config
			_nextRun = _lastRun.AddSeconds(_frequency);
		}

		public void Check() {

			if ( DateTime.Now > _nextRun && !Task.IsBusy) {
				try {
					Task.Run();
				}
				catch (Exception ex) {
					throw ex;
				}
				_nextRun = _nextRun.AddSeconds(_frequency);
			}

		}
	}

	internal class ScheduledTaskRunner: ITaskRunner {
		private DateTime _lastRun;
		private readonly TimeSpan _runAt;

		public ITask Task { get; private set; }

		public ScheduledTaskRunner(ITask task, TimeSpan runAt) {
			Task = task;
			_runAt = runAt;
		}

		public void Check()
		{
			if (_lastRun.Date != DateTime.Now.Date && DateTime.Now.TimeOfDay > _runAt && !Task.IsBusy)
			{
				try
				{
					Task.Run();
				}
				catch { }
				_lastRun = DateTime.Now;
			}
		}
	}

}
