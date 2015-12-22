using System;
using System.Linq;
using System.Windows.Input;
using LaunchSample.BLL.Services;
using LaunchSample.BLL.Services.LaunchService;
using LaunchSample.Core.Enumerations;
using LaunchSample.Domain.Models.Dtos;

namespace LaunchSample.WPF.ViewModel
{
	public class LaunchViewModel : ViewModelBase
	{
		#region Fields

		private readonly LaunchDto _launch;
		private readonly LaunchService _launchService;

		private bool _isHidden;
		private bool _isHiddenInList;
		private RelayCommand _saveCommand;
		private RelayCommand _cancelCommand;

		private LaunchStatus[] _launchStatusOptions;

		#endregion // Fields

		#region Constructor

		public LaunchViewModel(LaunchDto launch, LaunchService launchService)
		{
			if (launch == null)
			{
				throw new ArgumentNullException("launch");
			}

			if (launchService == null)
			{
				throw new ArgumentNullException("launchService");
			}

			_launch = launch;
			_launchService = launchService;

			IsHidden = true;
		}

		#endregion // Constructor

		#region Launch Properties

		public int Id
		{
			get { return _launch.Id; }
			set
			{
				if (value == _launch.Id)
				{
					return;
				}

				_launch.Id = value;

				base.OnPropertyChanged("Id");
			}
		}

		public string City
		{
			get
			{
				return _launch.City;
			}
			set
			{
				if (value == _launch.City)
				{
					return;
				}

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
				{
					return;
				}

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
				{
					return;
				}

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
				{
					return;
				}

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
				{
					return;
				}

				_launch.Status = value;

				base.OnPropertyChanged("Status");
			}
		}

		#endregion // Launch Properties

		#region Presentation Properties

		public LaunchStatus LaunchStatus
		{
			get { return _launch.Status; }
			set
			{
				if (value == _launch.Status)
				{
					return;
				}

				_launch.Status = value;

				base.OnPropertyChanged("LaunchStatus");
			}
		}

		public LaunchStatus[] LaunchStatusOptions
		{
			get
			{
				if (_launchStatusOptions == null)
				{
					_launchStatusOptions = Enum.GetValues(typeof (LaunchStatus)).Cast<LaunchStatus>().ToArray();
				}
				return _launchStatusOptions;
			}
		}

		public bool IsHidden
		{
			get
			{
				return _isHidden;
			}
			set
			{
				if (value == _isHidden)
				{
					return;
				}

				_isHidden = value;

				base.OnPropertyChanged("IsHidden");
			}
		}

		public bool IsHiddenInList
		{
			get
			{
				return _isHiddenInList;
			}
			set
			{
				if (value == _isHiddenInList)
				{
					return;
				}

				_isHiddenInList = value;

				base.OnPropertyChanged("IsHiddenInList");
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

		public ICommand CancelCommand
		{
			get
			{
				if (_cancelCommand == null)
				{
					_cancelCommand = new RelayCommand(
						param => Cancel(),
						param => CanCancel
						);
				}
				return _cancelCommand;
			}
		}

		#endregion // Presentation Properties

		#region Public Methods

		private void Save()
		{
			if (!_launch.IsValid)
			{
				throw new InvalidOperationException("Cannot save an invalid launch.");
			}

			if (IsNewLaunch)
			{
				_launchService.Create(_launch);
			}
			else
			{
				_launchService.Update(_launch);
			}

			IsHidden = true;

			base.OnPropertyChanged("DisplayName");
		}

		private void Cancel()
		{
			IsHidden = true;
		}

		#endregion // Public Methods

		#region Private Properties

		private bool IsNewLaunch { get { return !_launchService.IsAlreadyExists(_launch.Id); } }
		private bool CanSave { get { return _launch.IsValid; } }
		private bool CanCancel { get { return true; } }

		#endregion // Private Properties
	}
}