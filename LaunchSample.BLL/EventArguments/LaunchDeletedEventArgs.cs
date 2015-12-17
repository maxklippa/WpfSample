using System;
using LaunchSample.Domain.Models.Dtos;

namespace LaunchSample.BLL.EventArguments
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