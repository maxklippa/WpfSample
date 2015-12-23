using System;
using System.Linq;
using LaunchSample.BLL.Services.LaunchService;
using LaunchSample.Core.Enumerations;
using NSubstitute;
using NUnit.Framework;

using DataProvider = LaunchSample.WPF.Tests.LaunchListingViewModel.Data.LaunchListingVMDataProvider;
using LaunchListingVM = LaunchSample.WPF.ViewModel.LaunchListingViewModel;

namespace LaunchSample.WPF.Tests.LaunchListingViewModel
{
	[TestFixture]
	public class LaunchCityFilter
	{
		private const string ALL = "All";

		private ILaunchService _service;
		private DataProvider _dataProvider;

		[SetUp]
		public void BeforeTest()
		{
			_dataProvider = new DataProvider();
			_service = Substitute.For<ILaunchService>();
		}

		[Test]
		public void LaunchesFromAllCitiesReturned_WhenAllFilterItemSelected()
		{
			// Arrange
			_service.GetAll().Returns(_dataProvider.Launches);
			var launchListingVM = new LaunchListingVM(_service);
			var expectedLaunchIds = _dataProvider.Launches.Select(l => l.Id);

			// Act 
			launchListingVM.LaunchCityFilter = ALL;
			var actualLaunchIds = launchListingVM.AllLaunches.Where(l => !l.IsHiddenInList)
															 .Select(l => l.Id);

			// Assert
			CollectionAssert.AreEquivalent(expectedLaunchIds, actualLaunchIds);
		}

		[Test]
		public void LaunchesFilteredByCityReturned_WhenSpecificFilterItemSelected()
		{
			// Arrange
			_service.GetAll(Arg.Any<string>(), Arg.Any<DateTime?>(), Arg.Any<DateTime?>(), Arg.Any<LaunchStatus?>()).Returns(_dataProvider.Launches);
			var launchListingVM = new LaunchListingVM(_service);
			var expectedLaunchIds = _dataProvider.Launches.Where(l => l.City == DataProvider.FIRST_LAUNCH_CITY)
														  .Select(l => l.Id);

			// Act 
			launchListingVM.LaunchCityFilter = DataProvider.FIRST_LAUNCH_CITY;
			var actualLaunchIds = launchListingVM.AllLaunches.Where(l => !l.IsHiddenInList)
															 .Select(l => l.Id);

			// Assert
			CollectionAssert.AreEquivalent(expectedLaunchIds, actualLaunchIds);
		}

		[Test]
		public void LaunchesFilteredByCityAndOtherFiltersReturned_WhenSpecificCitySelectedWithOtherFilters()
		{
			// Arrange
			_service.GetAll(Arg.Any<string>(), Arg.Any<DateTime?>(), Arg.Any<DateTime?>(), Arg.Any<LaunchStatus?>()).Returns(_dataProvider.Launches);
			var launchListingVM = new LaunchListingVM(_service);
			var expectedLaunchIds = _dataProvider.Launches.Where(l => l.City == DataProvider.FIRST_LAUNCH_CITY &&
			                                                          l.Status.ToString() == DataProvider.FIRST_LAUNCH_STATUS.ToString() &&
			                                                          l.StartDateTime >= DataProvider.FIRST_LAUNCH_STARTTIME &&
			                                                          l.EndDateTime <= DataProvider.FIRST_LAUNCH_ENDTIME)
			                                     .Select(l => l.Id);

			// Act 
			launchListingVM.LaunchCityFilter = DataProvider.FIRST_LAUNCH_CITY;
			launchListingVM.LaunchStatusFilter = DataProvider.FIRST_LAUNCH_STATUS.ToString();
			launchListingVM.LaunchFromFilter = DataProvider.FIRST_LAUNCH_STARTTIME;
			launchListingVM.LaunchToFilter = DataProvider.FIRST_LAUNCH_ENDTIME;
			var actualLaunchIds = launchListingVM.AllLaunches.Where(l => !l.IsHiddenInList).Select(l => l.Id);

			// Assert
			CollectionAssert.AreEquivalent(expectedLaunchIds, actualLaunchIds);
		}
	}
}