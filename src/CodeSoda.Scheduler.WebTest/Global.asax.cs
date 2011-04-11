using System;

namespace CodeSoda.Scheduler.WebTest
{
	public class Global : System.Web.HttpApplication
	{
		private IScheduler _scheduler;

		protected void Application_Start(object sender, EventArgs e)
		{
			_scheduler = new Scheduler( 1 );
			_scheduler.AddTask( new TestTask(), Schedule.At(new TimeSpan(1,30,0)));
			_scheduler.Start();
		}

		protected void Application_End(object sender, EventArgs e)
		{
			_scheduler.Stop();
		}

		public override void Dispose()
		{
			((IDisposable)_scheduler).Dispose();
			base.Dispose();
		}
	}
}