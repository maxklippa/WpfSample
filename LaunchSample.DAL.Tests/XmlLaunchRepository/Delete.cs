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
	public class Delete
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
		public void SerializerDidNotSerialize_WhenPassedIdIsOutOfRange()
		{
			// Arrange
			const int OUT_OF_RANGE_ID = XmlRepositoryDataProvider.THIRD_LAUNCH_ID;

			var launches = new List<Launch> {_dataProvider.Launch1, _dataProvider.Launch2};

			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			// Act 
			repository.Delete(OUT_OF_RANGE_ID);

			// Assert
			_serializer.DidNotReceive()
			           .Serialize(Arg.Any<List<Launch>>());
		}

		[Test]
		public void SerializerDidNotSerialize_WhenPassedIdIsNonPositive()
		{
			// Arrange
			const int OUT_OF_RANGE_ID = -1;

			var launches = new List<Launch> {_dataProvider.Launch1, _dataProvider.Launch2};

			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			// Act 
			repository.Delete(OUT_OF_RANGE_ID);

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
			repository.Delete(XmlRepositoryDataProvider.FIRST_LAUNCH_ID);

			// Assert
			_serializer.DidNotReceive()
			           .Serialize(Arg.Any<List<Launch>>());
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
			repository.Delete(XmlRepositoryDataProvider.FIRST_LAUNCH_ID);

			// Assert
			_serializer.Received()
					   .Serialize(Arg.Is<List<Launch>>(x => x.Single() == _dataProvider.Launch2));
		}

		[Test]
		public void SerializerDidNotSerialize_WhenLaunchSerializerReturnNull()
		{
			// Arrange
			_serializer.Deserialize()
			           .Returns(l => null);

			var repository = CreateXmlRepository();

			// Act 
			repository.Delete(XmlRepositoryDataProvider.FIRST_LAUNCH_ID);

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