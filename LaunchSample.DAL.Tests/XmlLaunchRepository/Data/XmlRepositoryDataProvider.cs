using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.DAL.Tests.XmlLaunchRepository.Data
{
	public class XmlRepositoryDataProvider
	{
		public const int FIRST_LAUNCH_ID = 1;
		public const int SECOND_LAUNCH_ID = 2;
		public const int THIRD_LAUNCH_ID = 3;

		public const string BRN_CITY = "Barnaul";
		public const string NSK_CITY = "Novosibirsk";
		public const string OMSK_CITY = "Omsk";

		public Launch Launch1 { get; private set; }

		public Launch Launch2 { get; private set; }

		public Launch Launch3 { get; private set; }

		public XmlRepositoryDataProvider()
		{
			Launch1 = new Launch
			           {
				           Id = FIRST_LAUNCH_ID,
				           City = NSK_CITY
			           };

			Launch2 = new Launch
			           {
				           Id = SECOND_LAUNCH_ID,
				           City = BRN_CITY
			           };

			Launch3 = new Launch
			           {
				           Id = THIRD_LAUNCH_ID,
				           City = OMSK_CITY
			           };
		}
	}
}