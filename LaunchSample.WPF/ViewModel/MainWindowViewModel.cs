using System.Collections.Generic;
using LaunchSample.BLL.Services;

namespace LaunchSample.WPF.ViewModel
{
	

	public class MainWindowViewModel : WorkspaceViewModel
	{
		#region Fields

		private readonly LaunchService _launchService;
		private LaunchListingViewModel _launches;

		#endregion // Fields

		#region Constructor

		public MainWindowViewModel()
		{
			base.DisplayName = "MVVM Demo App";

			_launchService = new LaunchService();
		}

		#endregion // Constructor

		#region Workspace

		public LaunchListingViewModel Launches
		{
			get { return _launches ?? (_launches = new LaunchListingViewModel(_launchService)); }
		}

		#endregion // Workspace

	}
}
