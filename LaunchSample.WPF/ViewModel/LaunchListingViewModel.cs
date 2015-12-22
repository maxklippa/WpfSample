using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LaunchSample.BLL.Services.LaunchService;
using LaunchSample.Core.Enumerations;
using LaunchSample.Domain.Models.Dtos;

namespace LaunchSample.WPF.ViewModel
{
	public class LaunchListingViewModel : ViewModelBase
	{
		private const string ALL = "All";

		#region Fields

		private readonly LaunchService _launchService;

		private RelayCommand _createLaunchCommand;
		private RelayCommand _updateLaunchCommand;
		private RelayCommand _deleteLaunchCommand;

		private string[] _launchStatusFilterOptions;
		private string _launchStatusFilter;
		private string[] _launchCityFilterOptions;
		private string _launchCityFilter;
		private DateTime _launchFromFilter;
		private DateTime _launchToFilter;

		#endregion // Fields

		#region Constructor

		public LaunchListingViewModel(LaunchService launchService)
		{
			if (launchService == null)
			{
				throw new ArgumentNullException("launchService");
			}

			_launchService = launchService;

			_launchService.LaunchCreated += OnLaunchCreated;
			_launchService.LaunchUpdated += OnLaunchUpdated;
			_launchService.LaunchDeleted += OnLaunchDeleted;

			_launchStatusFilter = ALL;
			_launchCityFilter = ALL;
			_launchFromFilter = DateTime.Now.AddYears(-1);
			_launchToFilter = DateTime.Now;

			CreateLaunchListing();
		}

		private void CreateLaunchListing(string city = null,
		                                 DateTime? from = null,
		                                 DateTime? to = null,
		                                 LaunchStatus? status = null)
		{
			var all = _launchService.GetAll(city, from, to, status)
			                        .Select(l => new LaunchViewModel(l, _launchService))
			                        .ToList();

			AllLaunches = new ObservableCollection<LaunchViewModel>(all);

			AllLaunches.ToList().ForEach(l => l.IsHiddenInList = !IsSatisfyFilteringCondition(l));
		}

		#endregion // Constructor

		#region Public Interface

		public event EventHandler<LaunchWillCreatedEventArgs> LaunchWillCreated;
		public event EventHandler<LaunchWillUpdatedEventArgs> LaunchWillUpdated;

		public ObservableCollection<LaunchViewModel> AllLaunches { get; private set; }

		public LaunchViewModel SelectedLaunch { get; set; }

		public string LaunchStatusFilter
		{
			get { return _launchStatusFilter; }
			set
			{
				if (value == _launchStatusFilter)
				{
					return;
				}

				_launchStatusFilter = value;

				AllLaunches.ToList().ForEach(l => l.IsHiddenInList = !IsSatisfyFilteringCondition(l));

				base.OnPropertyChanged("LaunchStatusFilter");
			}
		}

		public string[] LaunchStatusFilterOptions
		{
			get
			{
				if (_launchStatusFilterOptions == null)
				{
					var statuses = Enum.GetValues(typeof (LaunchStatus))
					                   .Cast<LaunchStatus>()
					                   .Select(s => s.ToString())
					                   .ToList();
					statuses.Insert(0, ALL);
					_launchStatusFilterOptions = statuses.ToArray();
				}
				return _launchStatusFilterOptions;
			}
		}

		public string LaunchCityFilter
		{
			get { return _launchCityFilter; }
			set
			{
				if (value == _launchCityFilter)
				{
					return;
				}

				_launchCityFilter = value;

				AllLaunches.ToList().ForEach(l => l.IsHiddenInList = !IsSatisfyFilteringCondition(l));

				base.OnPropertyChanged("LaunchCityFilter");
			}
		}

		public string[] LaunchCityFilterOptions
		{
			get
			{
				if (_launchCityFilterOptions == null)
				{
					var towns = _launchService.GetAll().Select(l => l.City);
					var cities = new HashSet<string>(towns).ToList();
					cities.Insert(0, ALL);
					_launchCityFilterOptions = cities.ToArray();
				}
				return _launchCityFilterOptions;
			}
		}

		public DateTime LaunchFromFilter
		{
			get { return _launchFromFilter; }
			set
			{
				if (value == _launchFromFilter)
				{
					return;
				}

				_launchFromFilter = value;

				AllLaunches.ToList().ForEach(l => l.IsHiddenInList = !IsSatisfyFilteringCondition(l));

				base.OnPropertyChanged("LaunchFromFilter");
			}
		}

		public DateTime LaunchToFilter
		{
			get { return _launchToFilter; }
			set
			{
				if (value == _launchToFilter)
				{
					return;
				}

				_launchToFilter = value;

				AllLaunches.ToList().ForEach(l => l.IsHiddenInList = !IsSatisfyFilteringCondition(l));

				base.OnPropertyChanged("LaunchToFilter");
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
			var newLaunchViewModel = new LaunchViewModel(newLaunch, _launchService);

			if (LaunchWillCreated != null)
			{
				LaunchWillCreated(this, new LaunchWillCreatedEventArgs(newLaunchViewModel));
			}
		}

		private void UpdateLaunch()
		{
			if (SelectedLaunch == null)
			{
				return;
			}

			var editLaunch = AutoMapper.Mapper.Map<LaunchViewModel, LaunchDto>(SelectedLaunch);
			var editLaunchViewModel = new LaunchViewModel(editLaunch, _launchService);

			if (LaunchWillUpdated != null)
			{
				LaunchWillUpdated(this, new LaunchWillUpdatedEventArgs(editLaunchViewModel));
			}
		}

		private void DeleteLaunch()
		{
			if (SelectedLaunch == null)
			{
				return;
			}

			_launchService.Delete(SelectedLaunch.Id);
			AllLaunches.Remove(SelectedLaunch);
		}

		#endregion // Public Methods

		#region Private Methods

		private bool IsSatisfyFilteringCondition(LaunchViewModel launch)
		{
			var launchStatusFilter = _launchStatusFilter == ALL
				? (LaunchStatus?) null
				: (LaunchStatus) Enum.Parse(typeof (LaunchStatus), _launchStatusFilter);

			return (_launchStatusFilter == ALL || launch.Status == launchStatusFilter) &&
			       (_launchCityFilter == ALL || launch.City == _launchCityFilter) &&
			       (_launchFromFilter <= launch.StartDateTime) &&
			       (launch.EndDateTime <= _launchToFilter);
		}

		#endregion // Private Methods

		#region Base Class Overrides

		protected override void OnDispose()
		{
			foreach (var launchVM in AllLaunches)
			{
				launchVM.Dispose();
			}

			AllLaunches.Clear();

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

			var launch = AllLaunches.Select((v, i) => new {Launch = v, Index = i})
			                        .FirstOrDefault(x => x.Launch.Id == viewModel.Id);
			if (launch == null)
			{
				return;
			}

			AllLaunches.Remove(launch.Launch);
			AllLaunches.Insert(launch.Index, viewModel);
		}

		private void OnLaunchDeleted(object sender, LaunchDeletedEventArgs e)
		{
			var entity = AllLaunches.Select((v, i) => new {Launch = v, Index = i})
			                        .FirstOrDefault(x => x.Launch.Id == e.DeletedLaunchId);

			if (entity == null)
			{
				return;
			}

			AllLaunches.Remove(entity.Launch);
		}

		#endregion // Event Handling Methods
	}
}