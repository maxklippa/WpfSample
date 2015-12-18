using System;
using LaunchSample.Domain.Models.Dtos;
using LaunchSample.WPF.ViewModel;

namespace LaunchSample.WPF.EventArguments
{
	public class LaunchWillUpdatedEventArgs : EventArgs
	{
		public LaunchWillUpdatedEventArgs(LaunchViewModel updatedLaunch)
		{
			UpdatedLaunch = updatedLaunch;
		}

		public LaunchViewModel UpdatedLaunch { get; private set; }
	}
}