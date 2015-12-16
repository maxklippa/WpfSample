using System.ComponentModel;
using LaunchSample.BLL.Services;
using LaunchSample.Domain.Models;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.WPF.ViewModel
{
	public class LaunchViewModel : ViewModelBase, IDataErrorInfo
	{
		private readonly Launch _launch;
		private readonly LaunchService _launchService;

		public string this[string columnName]
		{
			get
			{
				throw new System.NotImplementedException();
				
			}
		}

		public string Error { get; private set; }
	}
}