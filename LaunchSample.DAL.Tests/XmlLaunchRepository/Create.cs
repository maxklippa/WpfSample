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
	public class Create
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
			Assert.Throws<ArgumentNullException>(() => repository.Create(null));
		}

		[Test]
		public void NewItemAddedToRepository_WhenLaunchSerializerReturnEmptyList()
		{
			// Arrange
			var launches = new List<Launch>();
			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			const int expectedLaunchId = 1;

			// Act 
			var actualLaunch = repository.Create(new Launch());

			// Assert
			Assert.AreEqual(expectedLaunchId, actualLaunch.Id);
			_serializer.Received()
			           .Serialize(Arg.Is<List<Launch>>(x => x.Single() == actualLaunch));
		}

		[Test]
		public void NewItemAddedToRepository_WhenLaunchSerializerReturnNonEmptyList()
		{
			// Arrange
			const int existingId1 = 1;
			const int existingId2 = 2;
			var launches = new List<Launch> {new Launch {Id = existingId1}, new Launch {Id = existingId2}};
			_serializer.Deserialize()
			           .Returns(launches);

			var repository = CreateXmlRepository();

			const int expectedLaunchId = 3;
			const int expectedLaunchesCount = 3;

			// Act 
			var actualLaunch = repository.Create(new Launch());

			// Assert
			Assert.AreEqual(expectedLaunchId, actualLaunch.Id);
			_serializer.Received()
			           .Serialize(Arg.Is<List<Launch>>(x => x.Single(l => l.Id == expectedLaunchId) == actualLaunch));
			_serializer.Received()
			           .Serialize(Arg.Is<List<Launch>>(x => x.Any(l => l.Id == existingId1)));
			_serializer.Received()
			           .Serialize(Arg.Is<List<Launch>>(x => x.Any(l => l.Id == existingId2)));
			_serializer.Received()
			           .Serialize(Arg.Is<List<Launch>>(x => x.Count == expectedLaunchesCount));
		}

		[Test]
		public void NewItemAddedToRepository_WhenLaunchSerializerReturnNull()
		{
			// Arrange
			_serializer.Deserialize()
			           .Returns(l => null);

			var repository = CreateXmlRepository();

			const int expectedLaunchId = 1;

			// Act 
			var actualLaunch = repository.Create(new Launch());

			// Assert
			Assert.AreEqual(expectedLaunchId, actualLaunch.Id);
			_serializer.Received()
			           .Serialize(Arg.Is<List<Launch>>(x => x.Single(l => l.Id == expectedLaunchId) == actualLaunch));
		}

		private LR.XmlLaunchRepository CreateXmlRepository()
		{
			return new LR.XmlLaunchRepository(_serializer);
		}
	}
}
