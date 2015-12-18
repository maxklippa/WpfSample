using System.Collections.Generic;
using LaunchSample.BLL.Services;
using LaunchSample.Domain.Models.Dtos;
using LaunchSample.WPF.EventArguments;

namespace LaunchSample.WPF.ViewModel
{
	

	public class MainWindowViewModel : ViewModelBase
	{
		#region Fields

		private readonly LaunchService _launchService;
		private LaunchListingViewModel _launches;
		private LaunchViewModel _launch;

		#endregion // Fields

		#region Constructor

		public MainWindowViewModel()
		{
			base.DisplayName = "MVVM Demo App";

			_launchService = new LaunchService();

			_launch = new LaunchViewModel(new LaunchDto(), _launchService) {IsHidden = true};

			Launches.LaunchWillCreated += OnLaunchWillCreated;
			Launches.LaunchWillUpdated += OnLaunchWillUpdated;
		}

		#endregion // Constructor

		#region Public Interface

		public LaunchListingViewModel Launches
		{
			get { return _launches ?? (_launches = new LaunchListingViewModel(_launchService)); }
		}

		public LaunchViewModel Launch
		{
			get
			{
				return _launch;
			}
			set
			{
				if (value == _launch)
					return;

				_launch = value;

				base.OnPropertyChanged("Launch");
			}
		}

		#endregion // Public Interface

		#region Event Handling Methods

		private void OnLaunchWillCreated(object sender, LaunchWillCreatedEventArgs e)
		{
			Launch = e.NewLaunch;
			Launch.IsHidden = false;
		}

		private void OnLaunchWillUpdated(object sender, LaunchWillUpdatedEventArgs e)
		{
			Launch = e.UpdatedLaunch;
			Launch.IsHidden = false;
		}

		#endregion // Event Handling Methods
	}
}
