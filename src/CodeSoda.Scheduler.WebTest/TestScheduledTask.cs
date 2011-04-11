using System;

namespace CodeSoda.Scheduler.WebTest {

	public class TestTask : ITask {

		public void Run() {
			lock (this) {
				IsBusy = true;
			}

			DataTransfer.SomeDate = DateTime.Now;

			lock (this) {
				this.IsBusy = false;
			}
		}

		public bool IsBusy { get; private set; }
	}

	public static class DataTransfer {
		public static DateTime SomeDate;
	}
}
