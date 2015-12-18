using System;
using LaunchSample.Domain.Models.Dtos;

namespace LaunchSample.BLL.Services.LaunchService
{
	public class LaunchUpdatedEventArgs : EventArgs
	{
		public LaunchUpdatedEventArgs(LaunchDto updatedLaunch)
		{
			UpdatedLaunch = updatedLaunch;
		}

		public LaunchDto UpdatedLaunch { get; private set; }
	}
}