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
	public class LaunchFromFilter
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
		public void LaunchesFilteredByStartDateTimeReturned_WhenSpecificFilterItemSelected()
		{
			// Arrange
			_service.GetAll(Arg.Any<string>(), Arg.Any<DateTime?>(), Arg.Any<DateTime?>(), Arg.Any<LaunchStatus?>()).Returns(_dataProvider.Launches);
			var launchListingVM = new LaunchListingVM(_service);
			var expectedLaunchIds = launchListingVM.AllLaunches.Where(l => l.StartDateTime >= DataProvider.FIRST_LAUNCH_STARTTIME)
														  .Select(l => l.Id);

			// Act 
			launchListingVM.LaunchFromFilter = DataProvider.FIRST_LAUNCH_STARTTIME;
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
			var expectedLaunchIds = launchListingVM.AllLaunches.Where(l => l.City == DataProvider.SECOND_LAUNCH_CITY &&
																	  l.Status.ToString() == DataProvider.SECOND_LAUNCH_STATUS.ToString() &&
																	  l.StartDateTime >= DataProvider.SECOND_LAUNCH_STARTTIME &&
																	  l.EndDateTime <= DataProvider.SECOND_LAUNCH_ENDTIME &&
																	  l.IsHighlighted)
												 .Select(l => l.Id);

			// Act 
			launchListingVM.LaunchCityFilter = DataProvider.SECOND_LAUNCH_CITY;
			launchListingVM.LaunchStatusFilter = DataProvider.SECOND_LAUNCH_STATUS.ToString();
			launchListingVM.LaunchFromFilter = DataProvider.SECOND_LAUNCH_STARTTIME;
			launchListingVM.LaunchToFilter = DataProvider.SECOND_LAUNCH_ENDTIME;
			launchListingVM.IsHighlightedOnly = true;
			var actualLaunchIds = launchListingVM.AllLaunches.Where(l => !l.IsHiddenInList)
												 .Select(l => l.Id);

			// Assert
			CollectionAssert.AreEquivalent(expectedLaunchIds, actualLaunchIds);
		}
	}
}