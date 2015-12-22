using System;
using System.Collections.Generic;
using System.Linq;
using LaunchSample.DAL.Tests.XmlLaunchRepository.Data;
using LaunchSample.Domain.Models.Entities;
using NSubstitute;
using NUnit.Framework;

using LR = LaunchSample.DAL.Repositories.LaunchRepository;

namespace LaunchSample.DAL.Tests.XmlLaunchRepository
{
	[TestFixture]
	public class Create
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
		public void ArgumentNullExceptionThrowed_WhenNullIsPassedAsArgument()
		{
			// Arrange
			var repository = CreateXmlRepository();

			// Act and Assert
			Assert.Throws<ArgumentNullException>(() => repository.Create(null));
		}

		[Test]
		public void NewItemAddedToRepository_WhenLaunchSerializerReturnEmptyList()
		{
			// Arrange
			const int EXPECTED_LAUNCH_ID = 1;

			var launches = new List<Launch>();

			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			

			// Act 
			var actualLaunch = repository.Create(new Launch());

			// Assert
			Assert.AreEqual(EXPECTED_LAUNCH_ID, actualLaunch.Id);
			_serializer.Received()
			           .Serialize(Arg.Is<List<Launch>>(x => x.Single() == actualLaunch));
		}

		[Test]
		public void NewItemAddedToRepository_WhenLaunchSerializerReturnNonEmptyList()
		{
			// Arrange
			const int EXPECTED_LAUNCH_ID = XmlRepositoryDataProvider.THIRD_LAUNCH_ID;
			const int EXPECTED_LAUNCHES_COUNT = 3;

			var launches = new List<Launch> {_dataProvider.Launch1, _dataProvider.Launch2};

			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			// Act 
			var actualLaunch = repository.Create(new Launch());

			// Assert
			Assert.AreEqual(EXPECTED_LAUNCH_ID, actualLaunch.Id);
			_serializer.Received()
			           .Serialize(Arg.Is<List<Launch>>(x => x.Single(l => l.Id == EXPECTED_LAUNCH_ID) == actualLaunch));
			_serializer.Received()
			           .Serialize(Arg.Is<List<Launch>>(x => x.Any(l => l.Id == XmlRepositoryDataProvider.FIRST_LAUNCH_ID)));
			_serializer.Received()
					   .Serialize(Arg.Is<List<Launch>>(x => x.Any(l => l.Id == XmlRepositoryDataProvider.SECOND_LAUNCH_ID)));
			_serializer.Received()
			           .Serialize(Arg.Is<List<Launch>>(x => x.Count == EXPECTED_LAUNCHES_COUNT));
		}

		[Test]
		public void NewItemAddedToRepository_WhenLaunchSerializerReturnNull()
		{
			// Arrange
			const int EXPECTED_LAUNCH_ID = XmlRepositoryDataProvider.FIRST_LAUNCH_ID;

			_serializer.Deserialize()
			           .Returns(l => null);

			var repository = CreateXmlRepository();

			// Act 
			var actualLaunch = repository.Create(new Launch());

			// Assert
			Assert.AreEqual(EXPECTED_LAUNCH_ID, actualLaunch.Id);
			_serializer.Received()
					   .Serialize(Arg.Is<List<Launch>>(x => x.Single(l => l.Id == EXPECTED_LAUNCH_ID) == actualLaunch));
		}

		private LR.XmlLaunchRepository CreateXmlRepository()
		{
			return new LR.XmlLaunchRepository(_serializer);
		}
	}
}
