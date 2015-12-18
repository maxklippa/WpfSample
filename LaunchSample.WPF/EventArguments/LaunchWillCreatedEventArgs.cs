using System;
using LaunchSample.Domain.Models.Dtos;
using LaunchSample.WPF.ViewModel;

namespace LaunchSample.WPF.EventArguments
{
	public class LaunchWillCreatedEventArgs : EventArgs
	{
		public LaunchWillCreatedEventArgs(LaunchViewModel newLaunch)
		{
			NewLaunch = newLaunch;
		}

		public LaunchViewModel NewLaunch { get; private set; }
	}
}