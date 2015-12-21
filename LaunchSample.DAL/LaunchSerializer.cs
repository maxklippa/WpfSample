using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.DAL
{
	public class LaunchSerializer : ILaunchSerializer
	{
		private readonly XmlSerializer _serializer;
		private readonly string _filename;

		public LaunchSerializer(string filename)
		{
			_serializer = new XmlSerializer(typeof(LaunchList));
			_filename = filename;
		}

		public void Serialize(IEnumerable<Launch> launches)
		{
			using (TextWriter writer = new StreamWriter(_filename))
			{
				_serializer.Serialize(writer, new LaunchList { Launches = launches.ToArray() });
			}
		}

		public IEnumerable<Launch> Deserialize()
		{
			using (TextReader reader = new StreamReader(_filename))
			{
				var launches = (LaunchList)_serializer.Deserialize(reader);
				return launches.Launches;
			}
		}

		public class LaunchList
		{
			public Launch[] Launches { get; set; }
		}
	}
}