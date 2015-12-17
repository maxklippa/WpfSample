using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using LaunchSample.BLL.EventArguments;
using LaunchSample.BLL.Services;

namespace LaunchSample.WPF.ViewModel
{
	public class LaunchListingViewModel : ViewModelBase
	{
		#region Fields

		private readonly LaunchService _launchService;

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

			CreateLaunchListing();
		}

		private void CreateLaunchListing()
		{
			List<LaunchViewModel> all = _launchService.All().Select(l => new LaunchViewModel(l, _launchService)).ToList();

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

		#endregion // Public Interface

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