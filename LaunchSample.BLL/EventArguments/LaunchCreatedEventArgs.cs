using System;
using LaunchSample.Domain.Models.Dtos;

namespace LaunchSample.BLL.EventArguments
{
	public class LaunchCreatedEventArgs : EventArgs
	{
		public LaunchCreatedEventArgs(LaunchDto newLaunch)
		{
			NewLaunch = newLaunch;
		}

		public LaunchDto NewLaunch { get; private set; }
	}
}