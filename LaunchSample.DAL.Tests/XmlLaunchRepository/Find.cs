using System.Collections.Generic;
using System.Linq;
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

		[SetUp]
		public void BeforeTest()
		{
			_serializer = Substitute.For<ILaunchSerializer>();
		}

		[Test]
		public void NullReturned_WhenPassedIdIsOutOfRange()
		{
			// Arrange
			const int existingId1 = 1;
			const int existingId2 = 2;
			var launches = new List<Launch> { new Launch { Id = existingId1 }, new Launch { Id = existingId2 } };
			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();
			
			// Act 
			const int outOfRangeId = 3;
			var actualLaunch = repository.Find(outOfRangeId);
			
			// Assert
			Assert.IsNull(actualLaunch);
		}

		[Test]
		public void NullReturned_WhenPassedIdIsNonPositive()
		{
			// Arrange
			const int existingId1 = 1;
			const int existingId2 = 2;
			var launches = new List<Launch> { new Launch { Id = existingId1 }, new Launch { Id = existingId2 } };
			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			// Act 
			const int outOfRangeId = -1;
			var actualLaunch = repository.Find(outOfRangeId);

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
			var actualLaunch = repository.Find(id: 1);

			// Assert
			Assert.IsNull(actualLaunch);
		}

		[Test]
		public void ExistingItemWithSame_WhenLaunchSerializerReturnNonEmptyList()
		{
			// Arrange
			const int existingId1 = 1;
			const int existingId2 = 2;
			var expectedLaunch1 = new Launch { Id = existingId1 };
			var expectedLaunch2 = new Launch { Id = existingId2 };
			var launches = new List<Launch> {expectedLaunch1, expectedLaunch2};
			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			// Act 
			var actualLaunch = repository.Find(existingId1);

			// Assert
			Assert.AreEqual(expectedLaunch1, actualLaunch);
		}

		[Test]
		public void NullReturned_WhenLaunchSerializerReturnNull()
		{
			// Arrange
			_serializer.Deserialize()
			           .Returns(l => null);

			var repository = CreateXmlRepository();

			// Act 
			var actualLaunch = repository.Find(1);

			// Assert
			Assert.IsNull(actualLaunch);
		}

		private LR.XmlLaunchRepository CreateXmlRepository()
		{
			return new LR.XmlLaunchRepository(_serializer);
		}
	}
}