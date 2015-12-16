using System.Collections.ObjectModel;
using LaunchSample.BLL.Services;

namespace LaunchSample.WPF.ViewModel
{
	public class LaunchListingViewModel : ViewModelBase
	{
		private readonly LaunchService _launchService;

		public LaunchListingViewModel()
		{
			_launchService = new LaunchService();

		}
	}
}