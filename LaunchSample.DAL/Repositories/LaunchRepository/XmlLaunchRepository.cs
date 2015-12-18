using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using LaunchSample.Core.Patterns;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.DAL.Repositories.LaunchRepository
{
	public class XmlLaunchRepository : Singleton<XmlLaunchRepository>, ILaunchRepository
	{
		#region Private fields

		private readonly string _filename;
		private readonly XmlSerializer _serializer;
		private readonly IList<Launch> _launches;

		#endregion // Private fields

		#region Constructor

		public XmlLaunchRepository()
		{
			_serializer = new XmlSerializer(typeof(LaunchList));

			_filename = ConfigurationManager.AppSettings["XmlFileName"];

			if (string.IsNullOrEmpty(_filename))
			{
				throw new ConfigurationErrorsException();
			}

			if (!File.Exists(_filename))
			{
				File.Create(_filename);
			}

			var isFileEmpty = false;
			using (TextReader reader = new StreamReader(_filename))
			{
				var serializer = new XmlSerializer(typeof(LaunchList));
				try
				{
					var list = (LaunchList)serializer.Deserialize(reader);
				}
				catch (InvalidOperationException)
				{
					isFileEmpty = true;
				}
			}

			if (isFileEmpty)
			{
				SaveLaunchesToFile(new List<Launch>());
			}

			_launches = GetLaunchesFromFile().ToList();
		}

		#endregion // Constructor

		#region ILaunchRepository Members

		public IQueryable<Launch> All()
		{
			return _launches.AsQueryable();
		}

		public Launch Create(Launch launch)
		{
			var id = _launches.Any() ? _launches.Max(l => l.Id) + 1 : 1;
			launch.Id = id;
			_launches.Add(launch);
			SaveLaunchesToFile(_launches);
			return launch;
		}

		public Launch Find(int id)
		{
			return _launches.FirstOrDefault(l => l.Id == id);
		}

		public void Update(Launch launch)
		{
			var entity = _launches.Select((v, i) => new {Launch = v, Index = i})
			                      .FirstOrDefault(x => x.Launch.Id == launch.Id);
			if (entity == null)
			{
				return;
			}
			_launches[entity.Index] = launch;
			SaveLaunchesToFile(_launches);
		}

		public void Delete(int id)
		{
			var entity = _launches.Select((v, i) => new {Launch = v, Index = i})
			                      .FirstOrDefault(x => x.Launch.Id == id);
			if (entity == null)
			{
				return;
			}
			_launches.RemoveAt(entity.Index);
			SaveLaunchesToFile(_launches);
		}

		#endregion // ILaunchRepository Members

		#region Private Methods

		private IEnumerable<Launch> GetLaunchesFromFile()
		{
			using (TextReader reader = new StreamReader(_filename))
			{
				var launches = (LaunchList) _serializer.Deserialize(reader);
				return launches.Launches;
			}
		}

		private void SaveLaunchesToFile(IEnumerable<Launch> launches)
		{
			using (TextWriter writer = new StreamWriter(_filename))
			{
				_serializer.Serialize(writer, new LaunchList { Launches = launches.ToArray() });
			}
		}

		#endregion // Private Methods

		#region Internal Classes

		public class LaunchList
		{
			public Launch[] Launches { get; set; }
		}

		#endregion Internal Classes
	}
}