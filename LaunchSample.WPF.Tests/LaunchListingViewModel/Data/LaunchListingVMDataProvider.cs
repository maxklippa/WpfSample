using System;
using System.Collections.Generic;
using LaunchSample.Core.Enumerations;
using LaunchSample.Domain.Models.Dtos;

namespace LaunchSample.WPF.Tests.LaunchListingViewModel.Data
{
	public class LaunchListingVMDataProvider
	{
		public const int FIRST_LAUNCH_ID = 1;
		public const int SECOND_LAUNCH_ID = 2;
		public const int THIRD_LAUNCH_ID = 3;

		public const LaunchStatus FIRST_LAUNCH_STATUS = LaunchStatus.Success;
		public const LaunchStatus SECOND_LAUNCH_STATUS = LaunchStatus.Success;
		public const LaunchStatus THIRD_LAUNCH_STATUS = LaunchStatus.Failed;

		public readonly static DateTime FIRST_LAUNCH_STARTTIME = new DateTime(2015, 1, 1);
		public readonly static DateTime SECOND_LAUNCH_STARTTIME = new DateTime(2015, 2, 1);
		public readonly static DateTime THIRD_LAUNCH_STARTTIME = new DateTime(2015, 3, 1);

		public readonly static DateTime FIRST_LAUNCH_ENDTIME = new DateTime(2015, 1, 2);
		public readonly static DateTime SECOND_LAUNCH_ENDTIME = new DateTime(2015, 2, 2);
		public readonly static DateTime THIRD_LAUNCH_ENDTIME = new DateTime(2015, 3, 2);

		public readonly static DateTime FIRST_LAUNCH_MONTH = new DateTime(2015, 1, 1);
		public readonly static DateTime SECOND_LAUNCH_MONTH = new DateTime(2015, 2, 1);
		public readonly static DateTime THIRD_LAUNCH_MONTH = new DateTime(2015, 3, 1);

		public const string FIRST_LAUNCH_CITY = "Barnaul";
		public const string SECOND_LAUNCH_CITY = "Novosibirsk";
		public const string THIRD_LAUNCH_CITY = "Omsk";

		public LaunchDto Launch1 { get; private set; }

		public LaunchDto Launch2 { get; private set; }

		public LaunchDto Launch3 { get; private set; }

		public IEnumerable<LaunchDto> Launches {
			get
			{
				return new[]
				       {
					       Launch1,
					       Launch2,
					       Launch3
				       };
			}
		}

		public LaunchListingVMDataProvider()
		{
			Launch1 = new LaunchDto
			          {
				          Id = FIRST_LAUNCH_ID,
				          City = FIRST_LAUNCH_CITY,
				          StartDateTime = FIRST_LAUNCH_STARTTIME,
				          EndDateTime = FIRST_LAUNCH_ENDTIME,
				          Month = FIRST_LAUNCH_MONTH,
				          Status = FIRST_LAUNCH_STATUS
			          };

			Launch2 = new LaunchDto
			          {
				          Id = SECOND_LAUNCH_ID,
				          City = SECOND_LAUNCH_CITY,
				          StartDateTime = SECOND_LAUNCH_STARTTIME,
				          EndDateTime = SECOND_LAUNCH_ENDTIME,
				          Month = SECOND_LAUNCH_MONTH,
				          Status = SECOND_LAUNCH_STATUS
			          };

			Launch3 = new LaunchDto
			          {
				          Id = THIRD_LAUNCH_ID,
				          City = THIRD_LAUNCH_CITY,
				          StartDateTime = THIRD_LAUNCH_STARTTIME,
				          EndDateTime = THIRD_LAUNCH_ENDTIME,
				          Month = THIRD_LAUNCH_MONTH,
				          Status = THIRD_LAUNCH_STATUS
			          };
		}
	}
}