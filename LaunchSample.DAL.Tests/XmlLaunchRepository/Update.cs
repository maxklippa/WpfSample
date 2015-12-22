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
	public class Update
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
			Assert.Throws<ArgumentNullException>(() => repository.Update(null));
		}

		[Test]
		public void SerializerDidNotSerialize_WhenPassedLaunchIdIsOutOfRange()
		{
			// Arrange
			var launches = new List<Launch> {_dataProvider.Launch1, _dataProvider.Launch2};

			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			// Act
			repository.Update(_dataProvider.Launch3);
			
			// Assert
			_serializer.DidNotReceive()
			           .Serialize(Arg.Any<List<Launch>>());
		}

		[Test]
		public void SerializerDidNotSerialize_WhenLaunchSerializerReturnEmptyList()
		{
			// Arrange
			var launches = new List<Launch>();

			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			// Act
			repository.Update(_dataProvider.Launch1);

			// Assert
			_serializer.DidNotReceive()
			           .Serialize(Arg.Any<List<Launch>>());
		}

		[Test]
		public void ExistingItemUpdated_WhenLaunchSerializerReturnNonEmptyList()
		{
			// Arrange
			const int expectedLaunchesCount = 2;

			var launches = new List<Launch> { _dataProvider.Launch1, _dataProvider.Launch2 };

			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			var launch = new Launch
			                    {
				                    Id = XmlRepositoryDataProvider.SECOND_LAUNCH_ID,
				                    City = XmlRepositoryDataProvider.OMSK_CITY
			                    };

			// Act 
			repository.Update(launch);

			// Assert
			_serializer.Received()
					   .Serialize(Arg.Is<List<Launch>>(x => x.Any(l => l.Id == XmlRepositoryDataProvider.FIRST_LAUNCH_ID &&
																	   l.City == XmlRepositoryDataProvider.NSK_CITY)));
			_serializer.Received()
			           .Serialize(Arg.Is<List<Launch>>(x => x.Any(l => l.Id == XmlRepositoryDataProvider.SECOND_LAUNCH_ID &&
			                                                           l.City == XmlRepositoryDataProvider.OMSK_CITY)));
			_serializer.Received()
			           .Serialize(Arg.Is<List<Launch>>(x => x.Count == expectedLaunchesCount));
		}

		[Test]
		public void SerializerDidNotSerialize_WhenLaunchSerializerReturnNull()
		{
			// Arrange
			_serializer.Deserialize()
			           .Returns(l => null);

			var repository = CreateXmlRepository();

			// Act
			repository.Update(_dataProvider.Launch1);

			// Assert
			_serializer.DidNotReceive()
			           .Serialize(Arg.Any<List<Launch>>());
		}

		private LR.XmlLaunchRepository CreateXmlRepository()
		{
			return new LR.XmlLaunchRepository(_serializer);
		}
	}
}