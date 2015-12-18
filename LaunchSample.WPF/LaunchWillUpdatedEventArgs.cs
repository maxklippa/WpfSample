using System;
using LaunchSample.WPF.ViewModel;

namespace LaunchSample.WPF
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