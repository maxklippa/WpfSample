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
	public class LaunchToFilter
	{
		private ILaunchService _service;
		private DataProvider _dataProvider;

		[SetUp]
		public void BeforeTest()
		{
			_dataProvider = new DataProvider();
			_service = Substitute.For<ILaunchService>();
		}

		[Test]
		public void LaunchesFilteredByEndDateTimeReturned_WhenSpecificFilterItemSelected()
		{
			// Arrange
			_service.GetAll(Arg.Any<string>(), Arg.Any<DateTime?>(), Arg.Any<DateTime?>(), Arg.Any<LaunchStatus?>()).Returns(_dataProvider.Launches);
			var launchListingVM = new LaunchListingVM(_service);
			var expectedLaunchIds = launchListingVM.AllLaunches.Where(l => l.EndDateTime <= DataProvider.FIRST_LAUNCH_ENDTIME)
														  .Select(l => l.Id);

			// Act 
			launchListingVM.LaunchToFilter = DataProvider.FIRST_LAUNCH_ENDTIME;
			var actualLaunchIds = launchListingVM.AllLaunches.Where(l => !l.IsHiddenInList)
															 .Select(l => l.Id);

			// Assert
			CollectionAssert.AreEquivalent(expectedLaunchIds, actualLaunchIds);
		}

		[Test]
		public void LaunchesFilteredByStartDateTimeAndOtherFiltersReturned_WhenSpecificStartDateTimeSelectedWithOtherFilters()
		{
			// Arrange
			_service.GetAll(Arg.Any<string>(), Arg.Any<DateTime?>(), Arg.Any<DateTime?>(), Arg.Any<LaunchStatus?>()).Returns(_dataProvider.Launches);
			var launchListingVM = new LaunchListingVM(_service);
			var expectedLaunchIds = launchListingVM.AllLaunches.Where(l => l.City == DataProvider.THIRD_LAUNCH_CITY &&
																	  l.Status.ToString() == DataProvider.THIRD_LAUNCH_STATUS.ToString() &&
																	  l.StartDateTime >= DataProvider.THIRD_LAUNCH_STARTTIME &&
																	  l.EndDateTime <= DataProvider.THIRD_LAUNCH_ENDTIME &&
																	  l.IsHighlighted)
												 .Select(l => l.Id);

			// Act 
			launchListingVM.LaunchCityFilter = DataProvider.THIRD_LAUNCH_CITY;
			launchListingVM.LaunchStatusFilter = DataProvider.THIRD_LAUNCH_STATUS.ToString();
			launchListingVM.LaunchFromFilter = DataProvider.THIRD_LAUNCH_STARTTIME;
			launchListingVM.LaunchToFilter = DataProvider.THIRD_LAUNCH_ENDTIME;
			launchListingVM.IsHighlightedOnly = true;
			var actualLaunchIds = launchListingVM.AllLaunches.Where(l => !l.IsHiddenInList)
												 .Select(l => l.Id);

			// Assert
			CollectionAssert.AreEquivalent(expectedLaunchIds, actualLaunchIds);
		}
	}
}