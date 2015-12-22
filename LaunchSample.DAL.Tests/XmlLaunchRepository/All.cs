using System.Collections.Generic;
using LaunchSample.Domain.Models.Entities;
using NSubstitute;
using NUnit.Framework;

using LR = LaunchSample.DAL.Repositories.LaunchRepository;

namespace LaunchSample.DAL.Tests.XmlLaunchRepository
{
	[TestFixture]
	public class All
	{
		private ILaunchSerializer _serializer;

		[SetUp]
		public void BeforeTest()
		{
			_serializer = Substitute.For<ILaunchSerializer>();
		}

		[Test]
		public void EmptyListReturned_WhenLaunchSerializerReturnEmptyList()
		{
			// Arrange
			_serializer.Deserialize()
			           .Returns(new List<Launch>());

			var repository = CreateXmlRepository();

			// Act 
			var all = repository.All();

			// Assert
			Assert.IsEmpty(all);
		}

		[Test]
		public void AllDeserializedItemsReturned_WhenLaunchSerializerReturnNonEmptyList()
		{
			// Arrange
			var expectedLaunch1 = new Launch { Id = 0 };
			var expectedLaunch2 = new Launch { Id = 1 };
			var expectedLaunches = new[] {expectedLaunch1, expectedLaunch2};

			_serializer.Deserialize()
			           .Returns(expectedLaunches);

			var repository = CreateXmlRepository();

			// Act
			var actualLaunches = repository.All();

			// Assert
			CollectionAssert.AreEqual(expectedLaunches, actualLaunches);
		}

		[Test]
		public void EmptyListReturned_WhenLaunchSerializerReturnNull()
		{
			// Arrange
			_serializer.Deserialize()
			           .Returns(l => null);

			var repository = CreateXmlRepository();

			// Act 
			var all = repository.All();

			// Assert
			Assert.IsEmpty(all);
		}

		private LR.XmlLaunchRepository CreateXmlRepository()
		{
			return new LR.XmlLaunchRepository(_serializer);
		}
	}
}
