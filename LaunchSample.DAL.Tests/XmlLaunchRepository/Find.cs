using System.Collections.Generic;
using LaunchSample.DAL.Tests.XmlLaunchRepository.Data;
using LaunchSample.Domain.Models.Entities;
using NSubstitute;
using NUnit.Framework;

using LR = LaunchSample.DAL.Repositories.LaunchRepository;

namespace LaunchSample.DAL.Tests.XmlLaunchRepository
{
	[TestFixture]
	public class Find
	{
		private ILaunchSerializer _serializer;
		private XmlRepositoryDataProvider _dataProvider;

		[SetUp]
		public void BeforeTest()
		{
			_serializer = Substitute.For<ILaunchSerializer>();
			_dataProvider = new XmlRepositoryDataProvider();
		}

		[Test]
		public void NullReturned_WhenPassedIdIsOutOfRange()
		{
			// Arrange
			const int OUT_OF_RANGE_ID = XmlRepositoryDataProvider.THIRD_LAUNCH_ID;

			var launches = new List<Launch> {_dataProvider.Launch1, _dataProvider.Launch2};

			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			// Act 
			var actualLaunch = repository.Find(OUT_OF_RANGE_ID);
			
			// Assert
			Assert.IsNull(actualLaunch);
		}

		[Test]
		public void NullReturned_WhenPassedIdIsNonPositive()
		{
			// Arrange
			const int OUT_OF_RANGE_ID = -1;

			var launches = new List<Launch> { _dataProvider.Launch1, _dataProvider.Launch2 };

			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			// Act 
			var actualLaunch = repository.Find(OUT_OF_RANGE_ID);

			// Assert
			Assert.IsNull(actualLaunch);
		}

		[Test]
		public void NullReturned_WhenLaunchSerializerReturnEmptyList()
		{
			// Arrange
			var launches = new List<Launch>();

			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			// Act 
			var actualLaunch = repository.Find(XmlRepositoryDataProvider.FIRST_LAUNCH_ID);

			// Assert
			Assert.IsNull(actualLaunch);
		}

		[Test]
		public void ExistingItemWithSame_WhenLaunchSerializerReturnNonEmptyList()
		{
			// Arrange
			var launches = new List<Launch> {_dataProvider.Launch1, _dataProvider.Launch2};
			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			// Act 
			var actualLaunch = repository.Find(XmlRepositoryDataProvider.FIRST_LAUNCH_ID);

			// Assert
			Assert.AreEqual(_dataProvider.Launch1, actualLaunch);
		}

		[Test]
		public void NullReturned_WhenLaunchSerializerReturnNull()
		{
			// Arrange
			_serializer.Deserialize()
			           .Returns(l => null);

			var repository = CreateXmlRepository();

			// Act 
			var actualLaunch = repository.Find(XmlRepositoryDataProvider.FIRST_LAUNCH_ID);

			// Assert
			Assert.IsNull(actualLaunch);
		}

		private LR.XmlLaunchRepository CreateXmlRepository()
		{
			return new LR.XmlLaunchRepository(_serializer);
		}
	}
}