using System;
using System.Collections.Generic;
using System.Linq;
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

		[SetUp]
		public void BeforeTest()
		{
			_serializer = Substitute.For<ILaunchSerializer>();
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
		public void NothingHappens_WhenPassedLaunchIdIsOutOfRange()
		{
			// Arrange
			const int existingId1 = 1;
			const int existingId2 = 2;
			const string existingCity1 = "Brn";
			const string existingCity2 = "Nsk";
			var expectedLaunch1 = new Launch { Id = existingId1, City = existingCity1 };
			var expectedLaunch2 = new Launch { Id = existingId2, City = existingCity2 };
			var launches = new List<Launch> { expectedLaunch1, expectedLaunch2 };
			_serializer.Deserialize().Returns(launches);
			var repository = CreateXmlRepository();
			var updatedLaunch = new Launch { Id = 3, City = "Omsk" };

			// Act
			repository.Update(updatedLaunch);
			
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
			var updatedLaunch = new Launch { Id = 3, City = "Omsk" };
			repository.Update(updatedLaunch);

			// Assert
			_serializer.DidNotReceive().Serialize(Arg.Any<List<Launch>>());
		}

		[Test]
		public void ExistingItemUpdated_WhenLaunchSerializerReturnNonEmptyList()
		{
			// Arrange
			const int existingId1 = 1;
			const int existingId2 = 2;
			const string existingCity1 = "Brn";
			const string existingCity2 = "Nsk";
			const string existingCity3 = "Omsk";
			var expectedLaunch1 = new Launch { Id = existingId1, City = existingCity1 };
			var expectedLaunch2 = new Launch { Id = existingId2, City = existingCity2 };
			var launches = new List<Launch> { expectedLaunch1, expectedLaunch2 };
			_serializer.Deserialize().Returns(launches);

			var repository = CreateXmlRepository();

			// Act 
			var updatedLaunch = new Launch { Id = 2, City = existingCity3 };
			repository.Update(updatedLaunch);

			// Assert
			const int expectedLaunchesCount = 2;
			_serializer.Received()
			           .Serialize(Arg.Is<List<Launch>>(x => x.Any(l => l.Id == existingId1 &&
			                                                           l.City == existingCity1)));
			_serializer.Received().Serialize(Arg.Is<List<Launch>>(x => x.Any(l => l.Id == existingId2 && l.City == existingCity3)));
			_serializer.Received().Serialize(Arg.Is<List<Launch>>(x => x.Count == expectedLaunchesCount));
		}

		[Test]
		public void NothingHappens_WhenLaunchSerializerReturnNull()
		{
			// Arrange
			_serializer.Deserialize().Returns(l => null);

			var repository = CreateXmlRepository();

			// Act
			var updatedLaunch = new Launch { Id = 3, City = "Omsk" };
			repository.Update(updatedLaunch);

			// Assert
			_serializer.DidNotReceive().Serialize(Arg.Any<List<Launch>>());
		}

		private LR.XmlLaunchRepository CreateXmlRepository()
		{
			return new LR.XmlLaunchRepository(_serializer);
		}
	}
}