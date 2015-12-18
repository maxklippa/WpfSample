using System;

namespace LaunchSample.BLL.Services.LaunchService
{
	public class LaunchDeletedEventArgs: EventArgs
	{
		public LaunchDeletedEventArgs(int deletedLaunchId)
		{
			DeletedLaunchId = deletedLaunchId;
		}

		public int DeletedLaunchId { get; private set; }
	}
}