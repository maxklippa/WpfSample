using System.Collections.Generic;
using System.Linq;
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

		[SetUp]
		public void BeforeTest()
		{
			_serializer = Substitute.For<ILaunchSerializer>();
		}

		[Test]
		public void NothingHappens_WhenPassedIdIsOutOfRange()
		{
			// Arrange
			const int existingId1 = 1;
			const int existingId2 = 2;
			var launches = new List<Launch> { new Launch { Id = existingId1 }, new Launch { Id = existingId2 } };
			_serializer.Deserialize().Returns(launches);

			var repository = CreateXmlRepository();

			// Act 
			const int outOfRangeId = 3;
			repository.Delete(outOfRangeId);

			// Assert
			_serializer.DidNotReceive().Serialize(Arg.Any<List<Launch>>());
		}

		[Test]
		public void NothingHappens_WhenPassedIdIsNonPositive()
		{
			// Arrange
			const int existingId1 = 1;
			const int existingId2 = 2;
			var launches = new List<Launch> { new Launch { Id = existingId1 }, new Launch { Id = existingId2 } };
			_serializer.Deserialize().Returns(launches);

			var repository = CreateXmlRepository();

			// Act 
			const int outOfRangeId = -1;
			repository.Delete(outOfRangeId);

			// Assert
			_serializer.DidNotReceive().Serialize(Arg.Any<List<Launch>>());
		}

		[Test]
		public void NothingHappens_WhenLaunchSerializerReturnEmptyList()
		{
			// Arrange
			var launches = new List<Launch>();
			_serializer.Deserialize().Returns(launches);

			var repository = CreateXmlRepository();

			// Act 
			repository.Delete(1);

			// Assert
			_serializer.DidNotReceive().Serialize(Arg.Any<List<Launch>>());
		}

		[Test]
		public void ExistingItemWithSame_WhenLaunchSerializerReturnNonEmptyList()
		{
			// Arrange
			const int existingId1 = 1;
			const int existingId2 = 2;
			var expectedLaunch1 = new Launch { Id = existingId1 };
			var expectedLaunch2 = new Launch { Id = existingId2 };
			var launches = new List<Launch> { expectedLaunch1, expectedLaunch2 };
			_serializer.Deserialize().Returns(launches);

			var repository = CreateXmlRepository();

			// Act 
			repository.Delete(existingId1);

			// Assert
			_serializer.Received().Serialize(Arg.Is<List<Launch>>(x => x.Single() == expectedLaunch2));
		}

		[Test]
		public void NothingHappens_WhenLaunchSerializerReturnNull()
		{
			// Arrange
			_serializer.Deserialize().Returns(l => null);

			var repository = CreateXmlRepository();

			// Act 
			repository.Delete(1);

			// Assert
			_serializer.DidNotReceive().Serialize(Arg.Any<List<Launch>>());
		}

		private LR.XmlLaunchRepository CreateXmlRepository()
		{
			return new LR.XmlLaunchRepository(_serializer);
		}
	}
}