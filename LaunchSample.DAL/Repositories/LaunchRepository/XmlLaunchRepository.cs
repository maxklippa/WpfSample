using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.DAL.Repositories.LaunchRepository
{
	public class XmlLaunchRepository : ILaunchRepository
	{
		#region Private fields

		private readonly ILaunchSerializer _serializer;

		#endregion // Private fields

		#region Constructor

		public XmlLaunchRepository(ILaunchSerializer serializer)
		{
			_serializer = serializer;
		}

		#endregion // Constructor

		#region ILaunchRepository Members

		public IQueryable<Launch> All()
		{
			var launches = _serializer.Deserialize() ?? new List<Launch>();
			return launches.AsQueryable();
		}

		public Launch Create(Launch launch)
		{
			var launches = _serializer.Deserialize().ToList();

			var id = launches.Any() ? launches.Max(l => l.Id) + 1 : 1;
			launch.Id = id;
			launches.Add(launch);

			_serializer.Serialize(launches);

			return launch;
		}

		public Launch Find(int id)
		{
			return _serializer.Deserialize().FirstOrDefault(l => l.Id == id);
		}

		public void Update(Launch launch)
		{
			var launches = _serializer.Deserialize().ToList();

			var entity = launches.Select((v, i) => new {Launch = v, Index = i})
			                      .FirstOrDefault(x => x.Launch.Id == launch.Id);
			if (entity == null)
			{
				return;
			}
			launches[entity.Index] = launch;

			_serializer.Serialize(launches);
		}

		public void Delete(int id)
		{
			var launches = _serializer.Deserialize().ToList();

			var entity = launches.Select((v, i) => new {Launch = v, Index = i})
			                      .FirstOrDefault(x => x.Launch.Id == id);
			if (entity == null)
			{
				return;
			}
			launches.RemoveAt(entity.Index);

			_serializer.Serialize(launches);
		}

		#endregion // ILaunchRepository Members
	}
}