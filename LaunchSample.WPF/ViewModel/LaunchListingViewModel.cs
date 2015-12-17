using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using LaunchSample.BLL.EventArguments;
using LaunchSample.BLL.Services;
using LaunchSample.Core.Enumerations;
using LaunchSample.Domain.Models.Dtos;

namespace LaunchSample.WPF.ViewModel
{
	public class LaunchListingViewModel : ViewModelBase
	{
		#region Fields

		private readonly LaunchService _launchService;

		private RelayCommand _createLaunchCommand;
		private RelayCommand _updateLaunchCommand;
		private RelayCommand _deleteLaunchCommand;

		private string[] _launchStatusOptions;
		private string _launchStatus;
		private string[] _launchCityOptions;
		private string _launchCity;
		private DateTime _launchFrom;
		private DateTime _launchTo;

		#endregion // Fields

		#region Constructor

		public LaunchListingViewModel(LaunchService launchService)
		{
			if (launchService == null)
				throw new ArgumentNullException("launchService");

			_launchService = launchService;

			_launchService.LaunchCreated += OnLaunchCreated;
			_launchService.LaunchUpdated += OnLaunchUpdated;
			_launchService.LaunchDeleted += OnLaunchDeleted;

			_launchStatus = "All";
			_launchCity = "All";
			_launchFrom = DateTime.Now.AddYears(-1);
			_launchTo = DateTime.Now;

			CreateLaunchListing();
		}

		private void CreateLaunchListing(string city = null, DateTime? from = null, DateTime? to = null, LaunchStatus? status = null)
		{
			List<LaunchViewModel> all = _launchService.GetAll(city, from, to, status).Select(l => new LaunchViewModel(l, _launchService)).ToList();

			foreach (LaunchViewModel launchViewModel in all)
			{
				launchViewModel.PropertyChanged += OnLaunchViewModelPropertyChanged;
			}

			AllLaunches = new ObservableCollection<LaunchViewModel>(all);
			AllLaunches.CollectionChanged += OnCollectionChanged;
		}

		#endregion // Constructor

		#region Public Interface

		public ObservableCollection<LaunchViewModel> AllLaunches { get; private set; }

		public LaunchViewModel SelectedLaunch { get; set; }

		public string LaunchStatus
		{
			get { return _launchStatus; }
			set
			{
				if (value == _launchStatus)
					return;

				_launchStatus = value;

				if (_launchStatus == "All")
				{
					//CreateLaunchListing();
				}
				else
				{
//					AllLaunches.
				}

				base.OnPropertyChanged("LaunchStatus");
			}
		}

		public string[] LaunchStatusOptions
		{
			get
			{
				if (_launchStatusOptions == null)
				{
					var statuses = Enum.GetValues(typeof (LaunchStatus)).Cast<LaunchStatus>().Select(s => s.ToString()).ToList();
					statuses.Insert(0, "All");
					_launchStatusOptions = statuses.ToArray();
				}
				return _launchStatusOptions;
			}
		}

		public string LaunchCity
		{
			get { return _launchCity; }
			set
			{
				if (value == _launchCity)
					return;

				_launchCity = value;

				if (_launchCity == "All")
				{
					//CreateLaunchListing();
				}
				else
				{
					//CreateLaunchListing(null, null, null, (LaunchStatus)Enum.Parse(typeof(LaunchStatus), _launchStatus));
				}

				base.OnPropertyChanged("LaunchCity");
			}
		}

		public string[] LaunchCityOptions
		{
			get
			{
				if (_launchCityOptions == null)
				{
					var towns = _launchService.GetAll().Select(l => l.City);
					var cities = new HashSet<string>(towns).ToList();
					cities.Insert(0, "All");
					_launchCityOptions = cities.ToArray();
				}
				return _launchCityOptions;
			}
		}

		public DateTime LaunchFrom
		{
			get { return _launchFrom; }
			set
			{
				if (value == _launchFrom)
					return;

				_launchFrom = value;

				// todo: filtering

				base.OnPropertyChanged("LaunchFrom");
			}
		}

		public DateTime LaunchTo
		{
			get { return _launchTo; }
			set
			{
				if (value == _launchTo)
					return;

				_launchTo = value;

				// todo: filtering

				base.OnPropertyChanged("LaunchTo");
			}
		}

		public ICommand CreateLaunchCommand
		{
			get
			{
				if (_createLaunchCommand == null)
				{
					_createLaunchCommand = new RelayCommand(param => CreateLaunch());
				}
				return _createLaunchCommand;
			}
		}

		public ICommand UpdateLaunchCommand
		{
			get
			{
				if (_updateLaunchCommand == null)
				{
					_updateLaunchCommand = new RelayCommand(param => UpdateLaunch());
				}
				return _updateLaunchCommand;
			}
		}

		public ICommand DeleteLaunchCommand
		{
			get
			{
				if (_deleteLaunchCommand == null)
				{
					_deleteLaunchCommand = new RelayCommand(param => DeleteLaunch());
				}
				return _deleteLaunchCommand;
			}
		}

		#endregion // Public Interface

		#region Public Methods

		private void CreateLaunch()
		{
			var newLaunch = new LaunchDto();
			var workspace = new LaunchViewModel(newLaunch, _launchService);
		}

		private void UpdateLaunch()
		{
			if (SelectedLaunch == null) return;

			var workspace = SelectedLaunch;
		}

		private void DeleteLaunch()
		{
			if (SelectedLaunch == null) return;

			_launchService.Delete(SelectedLaunch.Id);
			AllLaunches.Remove(SelectedLaunch);
		}

		#endregion // Public Methods

		#region Base Class Overrides

		protected override void OnDispose()
		{
			foreach (LaunchViewModel launchVM in AllLaunches)
			{
				launchVM.Dispose();
			}

			AllLaunches.Clear();
			AllLaunches.CollectionChanged -= OnCollectionChanged;

			_launchService.LaunchCreated -= OnLaunchCreated;
			_launchService.LaunchUpdated -= OnLaunchUpdated;
		}

		#endregion // Base Class Overrides

		#region Event Handling Methods

		private void OnLaunchCreated(object sender, LaunchCreatedEventArgs e)
		{
			var viewModel = new LaunchViewModel(e.NewLaunch, _launchService);
			AllLaunches.Add(viewModel);
		}

		private void OnLaunchUpdated(object sender, LaunchUpdatedEventArgs e)
		{
			var viewModel = new LaunchViewModel(e.UpdatedLaunch, _launchService);

			var entity = AllLaunches.Select((v, i) => new { Launch = v, Index = i }).FirstOrDefault(x => x.Launch.Id == viewModel.Id);
			if (entity == null)
			{
				return;
			}
			AllLaunches.Remove(entity.Launch);
			AllLaunches.Insert(entity.Index, viewModel);
		}

		private void OnLaunchDeleted(object sender, LaunchDeletedEventArgs e)
		{
			var entity = AllLaunches.Select((v, i) => new { Launch = v, Index = i }).FirstOrDefault(x => x.Launch.Id == e.DeletedLaunchId);
			if (entity == null)
			{
				return;
			}
			AllLaunches.Remove(entity.Launch);
		}

		private void OnLaunchViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null && e.NewItems.Count != 0)
			{
				foreach (LaunchViewModel launchVM in e.NewItems)
				{
					launchVM.PropertyChanged += OnLaunchViewModelPropertyChanged;
				}
			}

			if (e.OldItems != null && e.OldItems.Count != 0)
			{
				foreach (LaunchViewModel launchVM in e.OldItems)
				{
					launchVM.PropertyChanged -= OnLaunchViewModelPropertyChanged;
				}
			}
		}

		#endregion // Event Handling Methods
	}
}