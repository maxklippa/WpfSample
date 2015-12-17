using System;
using System.Windows.Input;
using LaunchSample.BLL.Services;
using LaunchSample.Core.Enumerations;
using LaunchSample.Domain.Models.Dtos;

namespace LaunchSample.WPF.ViewModel
{
	public class LaunchViewModel : WorkspaceViewModel
	{
		#region Fields

		private readonly LaunchDto _launch;
		private readonly LaunchService _launchService;


		private bool _isHidden;
		private RelayCommand _saveCommand;

		#endregion // Fields

		#region Constructor

		public LaunchViewModel(LaunchDto launch, LaunchService launchService)
		{
			if (launch == null)
				throw new ArgumentNullException("launch");

			if (launchService == null)
				throw new ArgumentNullException("launchService");

			_launch = launch;
			_launchService = launchService;
		}

		#endregion // Constructor

		#region Launch Properties

		public int Id
		{
			get { return _launch.Id; }
			set
			{
				if (value == _launch.Id)
					return;

				_launch.Id = value;

				base.OnPropertyChanged("Id");
			}
		}

		public string City
		{
			get { return _launch.City; }
			set
			{
				if (value == _launch.City)
					return;

				_launch.City = value;

				base.OnPropertyChanged("City");
			}
		}

		public DateTime StartDateTime
		{
			get { return _launch.StartDateTime; }
			set
			{
				if (value == _launch.StartDateTime)
					return;

				_launch.StartDateTime = value;

				base.OnPropertyChanged("City");
			}
		}

		public DateTime EndDateTime
		{
			get { return _launch.EndDateTime; }
			set
			{
				if (value == _launch.EndDateTime)
					return;

				_launch.EndDateTime = value;

				base.OnPropertyChanged("EndDateTime");
			}
		}

		public DateTime Month
		{
			get { return _launch.Month; }
			set
			{
				if (value == _launch.Month)
					return;

				_launch.Month = value;

				base.OnPropertyChanged("Month");
			}
		}

		public LaunchStatus Status
		{
			get { return _launch.Status; }
			set
			{
				if (value == _launch.Status)
					return;

				_launch.Status = value;

				base.OnPropertyChanged("Status");
			}
		}

		#endregion // Launch Properties

		#region Presentation Properties

		public bool IsHidden
		{
			get
			{
				return _isHidden;
			}
			set
			{
				if (value == _isHidden)
					return;

				_isHidden = value;

				base.OnPropertyChanged("IsHidden");
			}
		}

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new RelayCommand(
						param => Save(),
						param => CanSave
						);
				}
				return _saveCommand;
			}
		}

		#endregion // Presentation Properties

		#region Public Methods

		private void Save()
		{
			if (!_launch.IsValid)
				throw new InvalidOperationException("Cannot save an invalid launch.");

			if (this.IsNewLaunch)
			{
				_launchService.Create(_launch);
			}
			else
			{
				_launchService.Update(_launch);
			}

			base.OnPropertyChanged("DisplayName");
		}

		#endregion // Public Methods

		#region Private Helpers

		public bool IsNewLaunch { get { return !_launchService.IsAlreadyExists(_launch.Id); } }

		public bool CanSave { get { return _launch.IsValid; } }

		#endregion // Private Helpers
	}
}