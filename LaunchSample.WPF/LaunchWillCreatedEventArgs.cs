using System;
using LaunchSample.WPF.ViewModel;

namespace LaunchSample.WPF
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